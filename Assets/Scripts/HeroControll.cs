using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

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
    private Camera _cameraToAnimate;

    private My3DText _scoreText;
    private My3DText _comboText;

    private float _comboTime;
    private float _stopComboTime;

    private Vector3 _prevPoss;
    private Vector3 _currPoss;

    private float _maxScreenWidth;
    private float _maxScreenHeight;

    private bool _useEditor;

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
		base.Awake ();

        _useEditor = false;
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
            _comboText.Value = 0;
        }

        if (Input.GetKey(KeyCode.Escape))
        {
            Application.LoadLevel("MainMenu");
        }
	}

    protected override void OnMove()
    {
        _cameraToAnimate.GetComponent<Animator>().SetBool("Shake", false);
        Vector2 movePosition;

        if (_useEditor)
            movePosition = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed);
        else
            movePosition = new Vector2(_joystick.position.x * Time.deltaTime * MoveSpeed, _joystick.position.y * Time.deltaTime * MoveSpeed);

        gameObject.transform.Translate(movePosition);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxScreenWidth, _maxScreenWidth),
            Mathf.Clamp(transform.position.y, -_maxScreenHeight, _maxScreenHeight));

        GameObjectAnimator.SetBool("Move", true);
        ChangeAnimationDirection(GameObjectAnimator, movePosition);

        _prevPoss = _currPoss;
        _currPoss = Position;

        _attackToPosition = GetPositionOnDistance(AttackDistance + 2, GetMoveDirection(_prevPoss, _currPoss));
    }

    protected override void OnAttack()
    {
        GameObjectAnimator.SetBool("Move", false);
        GameObjectAnimator.SetBool("Attack", true);
        MoveToWorldPoint(_attackToPosition.x, _attackToPosition.y, MoveSpeed * MoveSpeed);

        if (Position.Equals(_attackToPosition))
        {
            CanAttack = false;
            SetDefaultAnimation(GameObjectAnimator);
        }
    }

    protected override void OnCollision(GameEntity collisionObject)
    {
        //if hero complete attack
        /*if (collisionObject is GameAI || collisionObject is ShooterEnemy)
        {
            object enemyScript = collisionObject.GetComponent<GameAI>();
            if (enemyScript == null)
                enemyScript = collisionObject.GetComponent<ShooterEnemy>();
            _scoreText.Value = ((GameEntity) enemyScript).ScorePoint + _scoreText.Value + _comboText.Value;
            _comboText.Value = _comboText.Value + 1;
            _comboTime = Time.time;
            _scoreText.PlayAmination();
            _comboText.PlayAmination();

            if (_comboText.FontSize < _comboText.MaxFontSize)
                _comboText.FontSize += 10;
        }*/
        //if hero dead
        //ToDO Hero dead
    }

    protected override void OnBlink()
    {
    }

    void OnActionButtonClicked(object sender, EventArgs eventArgs)
    {
        if (State.Equals(GameEntityState.Move))
            CanAttack = true;
        _cameraToAnimate.GetComponent<Animator>().SetBool("Shake", true);
    }

    bool GetJoystickMove()
    {
        float positionX = _useEditor ? Input.GetAxis("Horizontal") : _joystick.position.x;
        float positionY = _useEditor ? Input.GetAxis("Vertical") : _joystick.position.y;

        if (new Vector2(positionX, positionY).Equals(Vector2.zero))
            return false;

        return true;
    }
}
