using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class HeroControll : GameEntity
{

    // need to make borders for player
    public Camera cam;
    private float _maxScreenWidth;
    private float _maxScreenHeight;

    public Joystick joyStickLeft;
    private Animator heroAnimation;

    private const string HERO_LEFT = "Left";
    private const string HERO_RIGHT = "Right";
    private const string HERO_TOP = "Top";
    private const string HERO_BOTTOM = "Bottom";

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

        // need to make borders for player
        if (cam == null)
        {
            cam = Camera.main;
        }
        Vector3 targetWidth = cam.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float heroWidth = renderer.bounds.extents.x;
        float heroHeight = renderer.bounds.extents.y;
        _maxScreenWidth = targetWidth.x - heroWidth;
        _maxScreenHeight = targetWidth.y - heroHeight;
    }

    void Update()
    {
        float positionX = _useEditor ? Input.GetAxis("Horizontal") : joyStickLeft.position.x;
        float positionY = _useEditor ? Input.GetAxis("Vertical") : joyStickLeft.position.y;

        ChangeAnimationPosition(positionX, positionY);

        if (joyStickLeft != null)
            gameObject.transform.Translate(new Vector3(joyStickLeft.position.x * Time.deltaTime * MoveSpeed, joyStickLeft.position.y * Time.deltaTime * MoveSpeed, 0));
        if (_useEditor)
            gameObject.transform.Translate(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed));

        // need to constrain player movement
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxScreenWidth, _maxScreenWidth),
            Mathf.Clamp(transform.position.y, -_maxScreenHeight, _maxScreenHeight));

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
        heroAnimation.SetBool(HERO_RIGHT, false);
        heroAnimation.SetBool(HERO_LEFT, false);
        Directions dir = GetDirection(new Vector2(positionX, positionY));
        heroAnimation.SetBool(dir.ToString(), true);
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
