using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class HeroControll : GameEntity
{

    private enum axis { Left, Rigth, Up, Down };

    public Joystick joyStickLeft;
    private Animator heroAnimation;
    private bool isFacingRight = true;

    private const string HERO_LEFT = "Left";
    private const string HERO_RIGHT = "Right";
    private const string HERO_TOP = "Top";
    private const string HERO_BOTTOM = "Bottom";
    private int[] moveAxis; //left, right, up, down

    private Vector3 heroParams;
    private Vector3 bordersParams;
    private Vector3 textureParams;

    [Range(0, 1)]
    public float borderLeftRightWitdh;
    [Range(0, 1)]
    public float borderUpDownWitdh;

    [SerializeField]
    private GameObject GUI;

    private My3DText _scoreText;
    private My3DText _comboText;

    private float _comboTime;
    private float _stopComboTime;

	private bool _useEditor = false;

    void Awake()
    {
		#if UNITY_EDITOR
		_useEditor = true;
		#endif

        moveAxis = new[] { 1, 1, 1, 1 };

        heroAnimation = GetComponent<Animator>();

        //combo will stop after 1 sec
        _stopComboTime = 1.0f;

        if (joyStickLeft == null)
            Debug.Log("Please set the joystick prefab!");

        foreach (My3DText guiScripts in GUI.GetComponents<My3DText>())
        {
            if (guiScripts.TextObjectName.Equals("Score"))
                _scoreText = guiScripts;
            if (guiScripts.TextObjectName.Equals("Combo"))
                _comboText = guiScripts;
        }
    }

    void Update()
    {
        heroParams = transform.position;
        bordersParams = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
		float positionX = _useEditor ? Input.GetAxis ("Horizontal") : joyStickLeft.position.x;
		float positionY = _useEditor ? Input.GetAxis ("Vertical") : joyStickLeft.position.y;

        textureParams = gameObject.GetComponent<SpriteRenderer>().bounds.size;

        if (joyStickLeft.position.x > 0.0f && heroParams.x + textureParams.x / 2 > bordersParams.x - borderLeftRightWitdh)
            moveAxis[(int)axis.Rigth] = 0;
        else
            moveAxis[(int)axis.Rigth] = 1;

        ChangeAnimationPosition(positionX, positionY);

        if (joyStickLeft.position.x < 0.0f && heroParams.x - textureParams.x / 2 < borderLeftRightWitdh - bordersParams.x)
            moveAxis[(int)axis.Left] = 0;
        else
            moveAxis[(int)axis.Left] = 1;

        if (joyStickLeft.position.y > 0.0f && heroParams.y + textureParams.y / 2 > bordersParams.y - borderUpDownWitdh)
            moveAxis[(int)axis.Up] = 0;
        else
            moveAxis[(int)axis.Up] = 1;

        if (joyStickLeft.position.y < 0.0f && heroParams.y - textureParams.y / 2 < borderUpDownWitdh - bordersParams.y)
            moveAxis[(int)axis.Down] = 0;
        else
            moveAxis[(int)axis.Down] = 1;

        if (joyStickLeft != null)
            gameObject.transform.Translate(new Vector3(moveAxis[(int)axis.Left] * moveAxis[(int)axis.Rigth] * joyStickLeft.position.x * Time.deltaTime * MoveSpeed, moveAxis[(int)axis.Up] * moveAxis[(int)axis.Down] * joyStickLeft.position.y * Time.deltaTime * MoveSpeed, 0));
		if(_useEditor)
			gameObject.transform.Translate (new Vector2(Input.GetAxis ("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis ("Vertical") * Time.deltaTime * MoveSpeed));

        //implement to stop combo
        if (_comboTime + _stopComboTime < Time.time)
        {
			_comboText.StopAnimation();
            _comboText.FontSize = _comboText.DefaultFontSize;
            _comboText.Value = 0;
        }
	}

    private void ChangeAnimationPosition(float positionX, float positionY)
    {
        heroAnimation.SetBool(HERO_TOP, false);
        heroAnimation.SetBool(HERO_BOTTOM, false);
		heroAnimation.SetBool (HERO_RIGHT, false);
		heroAnimation.SetBool (HERO_LEFT, false);
		Directions dir = GetDirection (new Vector2(positionX, positionY));
		heroAnimation.SetBool (dir.ToString(), true);
    }

    //Hero collision
    void OnCollision(GameObject other)
    {
        //if hero complete attack
        if (other.CompareTag("Enemy"))
        {
            var enemyScript = other.GetComponent<GameAI>();
            _scoreText.Value = enemyScript.ScorePoint + _scoreText.Value + _comboText.Value;
            _comboText.Value = _comboText.Value + 1;
            _comboTime = Time.time;
			_scoreText.PlayAmination();
			_comboText.PlayAmination();

			if (_comboText.FontSize < _comboText.MaxFontSize)
				_comboText.FontSize += 10;
        }
        //if hero dead
        //ToDO Hero dead
    }
}
