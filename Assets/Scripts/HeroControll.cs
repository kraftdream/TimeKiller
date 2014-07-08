using System;
using UnityEngine;
using System.Collections;

public class HeroControll : GameEntity
{

    #region Input Value

    [SerializeField]
    private float borderLeftRightWitdh;

    [SerializeField]
    private float borderUpDownWitdh;

    [SerializeField]
    private ActionButton _actionButton;

    [SerializeField]
    private GameObject _gui;

    [SerializeField]
    private Joystick _joystick;

    [SerializeField]
    private Camera _mainCamera;

    [SerializeField]
    private Camera _guiCamera;

    [SerializeField]
    private GameObject _playerAttackSlash;

    [SerializeField]
    private AudioClip _attackAudioClip;

    [SerializeField]
    private AudioClip _bloodAudioClip;

    [SerializeField]
    private AudioClip _cryAudioClip;

    [SerializeField]
    private AudioSource _backgroundSound;

    private My3DText _scoreText;
    private My3DText _comboText;

    private float _comboTime;
    private float _stopComboTime;

    private Vector3 _prevPoss;
    private Vector3 _currPoss;

    private float _maxScreenWidth;
    private float _maxScreenHeight;

    private bool _useEditor;

    private float _defaultAttackSpeed;

    private Directions _currDirection;

    private ParticleSystem _playerDeathBlood;
    private GameObject _collidedGameObject;
    private GameEntity _collidedEnemyScript;
    private Material _darkScreen;

    private ScoreControll _scoreControll;

    private AudioSource _attackSound;
    private AudioSource _bloodSound;
    private AudioSource _crySound;

    public float BorderLeftRightWitdh
    {
        get { return borderLeftRightWitdh; }
        set { borderLeftRightWitdh = value; }
    }

    public float BorderUpDownWitdh
    {
        get { return borderUpDownWitdh; }
        set { borderUpDownWitdh = value; }
    }

    #endregion

    protected void Awake()
    {
        base.Awake();

        _useEditor = false;
        _defaultAttackSpeed = AttackSpeed;

#if UNITY_EDITOR
        _useEditor = true;
#endif

        CanAttack = false;

        //combo will stop after 1 sec
        _stopComboTime = 1.0f;

        foreach (My3DText guiScripts in _gui.GetComponents<My3DText>())
        {
            if (guiScripts.TextObjectName.Equals("Score"))
                _scoreText = guiScripts;
            if (guiScripts.TextObjectName.Equals("Combo"))
                _comboText = guiScripts;
        }

        Vector3 targetWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float heroWidth = renderer.bounds.extents.x;
        float heroHeight = renderer.bounds.extents.y;
        _maxScreenWidth = targetWidth.x - heroWidth;
        _maxScreenHeight = targetWidth.y - heroHeight;

        _actionButton.OnBtnClick += new ActionButton.OnButtonClickListener(OnActionButtonClicked);

        _attackSound = gameObject.AddComponent<AudioSource>();
        _attackSound.clip = _attackAudioClip;
        _attackSound.pitch = 1.1f;

        _bloodSound = gameObject.AddComponent<AudioSource>();
        _bloodSound.clip = _bloodAudioClip;
        _bloodSound.pitch = 0.7f;

        _crySound = gameObject.AddComponent<AudioSource>();
        _crySound.clip = _cryAudioClip;
        _crySound.pitch = 0.5f;
        _crySound.volume = 0.5f;

        _playerDeathBlood = GetComponentInChildren<ParticleSystem>();
        _playerDeathBlood.active = false;
        _darkScreen = _mainCamera.GetComponentInChildren<Renderer>().material;

        _scoreControll = new ScoreControll();
    }

    protected void Update()
    {
        base.Update();

        IsMoveJoystick = GetJoystickMove();

        //implement to stop combo
        if (_comboTime + _stopComboTime < Time.time)
        {
            _comboText.StopAnimation();
            _comboText.FontSize = _comboText.DefaultFontSize;
            _scoreControll.BestCombo = _comboText.Value;
            _comboText.Value = 0;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
    }

    protected override void OnMove()
    {
        Vector2 movePosition;

        if (_useEditor)
            movePosition = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed);
        else
            movePosition = new Vector2(_joystick.position.x * Time.deltaTime * MoveSpeed, _joystick.position.y * Time.deltaTime * MoveSpeed);

        gameObject.transform.Translate(movePosition);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxScreenWidth, _maxScreenWidth),
            Mathf.Clamp(transform.position.y, -_maxScreenHeight, _maxScreenHeight));

        ChangeAnimationDirection(GameObjectAnimator, movePosition);
        _currDirection = GetDirection(movePosition);

        if (!GameObjectAnimator.GetBool("Move"))
        {
            GameObjectAnimator.speed = 100;
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Move", true);
        }
        else
            AnimationWithMoveSpeed();

        _prevPoss = _currPoss;
        _currPoss = Position;

        _attackToPosition = GetPositionOnDistance(AttackDistance, GetMoveDirection(_prevPoss, _currPoss));
    }

    protected override void OnAttack()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        if (!_attackSound.isPlaying)
            _attackSound.Play();

        CheckPlayerCrossScreenBoundaries();

        MoveToWorldPoint(_attackToPosition.x, _attackToPosition.y, AttackSpeed);

        if (AttackSpeed > _defaultAttackSpeed * 0.5f)
        {
            float currentDistance = Vector2.Distance(Position, _attackToPosition);
            float distanceToPercent = (currentDistance * 100.0f) / AttackDistance;
            AttackSpeed = (distanceToPercent * _defaultAttackSpeed) / 50.0f;
        }

        if (!GameObjectAnimator.GetBool("Attack"))
        {
            GameObjectAnimator.speed = 100;
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Attack", true);
        }

        ChangeSlashEffectPosition(_currDirection);
        GameObjectAnimator.speed -= AttackSpeed * 0.001f;

        _playerAttackSlash.SetActive(true);

        if (Position.Equals(_attackToPosition))
        {
            GameObjectAnimator.speed = 100;
            CanAttack = false;
            AttackSpeed = _defaultAttackSpeed;
            _mainCamera.GetComponent<Animator>().SetBool("Shake", false);

            _playerAttackSlash.SetActive(false);
        }
    }

    public override void OnCollision(GameObject collisionObject)
    {

        if (collisionObject.CompareTag("Enemy"))
        {
            _collidedGameObject = collisionObject;
            _collidedEnemyScript = collisionObject.GetComponent<GameEntity>();

            if (State == GameEntityState.Attack)
            {
                if (_collidedEnemyScript.BulletObject != null && _collidedEnemyScript.BulletObject.renderer.bounds.Intersects(renderer.bounds))
                    DestroyBullet();
                else
                    HeroKillsEnemy();
            }

            if (State != GameEntityState.Attack && _collidedEnemyScript.State == GameEntityState.Attack)
            {
                DestroyBullet();
                HeroDead();
            }
            _collidedEnemyScript.CollisionDetected = false;
        }
    }

    void HeroKillsEnemy()
    {
        _collidedEnemyScript.Health--;

        if (!_collidedGameObject.audio.isPlaying && _collidedGameObject.activeInHierarchy)
            _collidedGameObject.audio.Play();
        _mainCamera.GetComponent<Animator>().SetBool("Shake", true); 

        _scoreText.Value = _collidedEnemyScript.ScorePoint + _scoreText.Value + _comboText.Value;
        _comboText.Value = _comboText.Value + 1;
        _comboTime = Time.time;
        _scoreText.PlayAmination();
        _comboText.PlayAmination();

        if (_comboText.FontSize < _comboText.MaxFontSize)
            _comboText.FontSize += 10;
    }

    void HeroDead()
    {
        Health -= 1.0f;
        _bloodSound.Play();
        _backgroundSound.pitch = 0.5f;
        _crySound.Play();

        _playerDeathBlood.active = true;
        _scoreControll.SaveScore((int)_scoreText.Value);
        StartCoroutine(DeathScreen());
        RestartMenu();
        HideJoystickAndGuiLayer();

        if (!GameObjectAnimator.GetBool("Death"))
        {
            GameObjectAnimator.speed = 100;
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Death", true);
        }
        GameObjectAnimator.speed = 1;
    }

    IEnumerator DeathScreen()
    {
        String colorComponent = "_Color";
        Color color = _darkScreen.GetColor(colorComponent);

        while (color.a < 0.5f)
        {
            yield return new WaitForEndOfFrame();
            color.a = color.a + 0.01f;
            _darkScreen.SetColor(colorComponent, color);

            if (Time.timeScale > 0.15)
                Time.timeScale -= 0.018f;
        }
    }

    void RestartMenu()
    {
        RestartMenu restartMenu = _guiCamera.GetComponent<RestartMenu>();
        restartMenu.IsShowRestart = true;
		restartMenu.GameScore = _scoreText.Value;
    }

    void HideJoystickAndGuiLayer()
    {
        _guiCamera.enabled = false;
    }

    void DestroyBullet()
    {
        if (_collidedEnemyScript.BulletObject != null)
        {
            Destroy(_collidedEnemyScript.BulletObject.gameObject);
        }
    }

    protected override void OnBlink()
    {

    }

    void OnActionButtonClicked(object sender, EventArgs eventArgs)
    {
        if (State.Equals(GameEntityState.Move))
        {
            GameObjectAnimator.speed = 1;
            CanAttack = true;
        }
    }

    bool GetJoystickMove()
    {
        float positionX = _useEditor ? Input.GetAxis("Horizontal") : _joystick.position.x;
        float positionY = _useEditor ? Input.GetAxis("Vertical") : _joystick.position.y;

        if (new Vector2(positionX, positionY).Equals(Vector2.zero))
            return false;

        return true;
    }

    void AnimationWithMoveSpeed()
    {
        if (!_useEditor)
        {
            if ((Mathf.Abs(_joystick.position.x) > Mathf.Abs(_joystick.position.y)))
                GameObjectAnimator.speed = Mathf.Abs(_joystick.position.x);
            else
                GameObjectAnimator.speed = Mathf.Abs(_joystick.position.y);
        }
        else
        {
            if ((Input.GetAxis("Horizontal") * Input.GetAxis("Vertical")) > 0)
                GameObjectAnimator.speed = (Input.GetAxis("Horizontal") * Input.GetAxis("Vertical")) / 2;
            else if (_joystick.position.x > 0)
                GameObjectAnimator.speed = Input.GetAxis("Horizontal");
            else if (_joystick.position.y > 0)
                GameObjectAnimator.speed = Input.GetAxis("Vertical");
        }
    }

    void ChangeSlashEffectPosition(Directions direction)
    {
        if (direction.Equals(Directions.Left))
        {
            _playerAttackSlash.transform.localPosition = new Vector3(-renderer.bounds.size.x * 0.1f, renderer.bounds.size.y * 0.225f, 0);
        }
        else if (direction.Equals(Directions.Right))
        {
            _playerAttackSlash.transform.localPosition = new Vector3(renderer.bounds.size.x * 0.1f, renderer.bounds.size.y * 0.225f, 0);
        }
        else if (direction.Equals(Directions.Top))
        {
            _playerAttackSlash.transform.localPosition = new Vector3(renderer.bounds.size.x * 0.1f, renderer.bounds.size.y * 0.1f, 0);
        }
        else if (direction.Equals(Directions.Bottom))
        {
            _playerAttackSlash.transform.localPosition = new Vector3(-renderer.bounds.size.x * 0.1f, -renderer.bounds.size.y * 0.1f, 0);
        }
    }

    void CheckPlayerCrossScreenBoundaries()
    {
        if (_attackToPosition.x > _maxScreenWidth)
        {
            _attackToPosition = new Vector2(_maxScreenWidth, _attackToPosition.y);
        }
        else if (_attackToPosition.x < -_maxScreenWidth)
        {
            _attackToPosition = new Vector2(-_maxScreenWidth, _attackToPosition.y);
        }
        else if (_attackToPosition.y > _maxScreenHeight)
        {
            _attackToPosition = new Vector2(_attackToPosition.x, _maxScreenHeight);
        }
        else if (_attackToPosition.y < -_maxScreenHeight)
        {
            _attackToPosition = new Vector2(_attackToPosition.x, -_maxScreenHeight);
        }   
    }
}
