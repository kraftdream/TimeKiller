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

    protected void Awake()
    {
		base.Awake ();
        //combo will stop after 1 sec
        _stopComboTime = 1.0f;

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

    protected void Update()
    {
        base.Update();
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

    protected override void OnMove(Vector2 _movePosition)
    {
        gameObject.transform.Translate(_movePosition);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxScreenWidth, _maxScreenWidth),
            Mathf.Clamp(transform.position.y, -_maxScreenHeight, _maxScreenHeight));
        ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
    }

    protected override void OnCollision(GameEntity collisionObject)
    {
        //if hero complete attack
        if (collisionObject is GameAI)
        {
            var enemyScript = collisionObject.GetComponent<GameAI>();
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

    protected override void OnBlink()
    {
    }    
}
