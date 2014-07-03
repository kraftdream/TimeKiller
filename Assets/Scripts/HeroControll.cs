﻿using System;
using System.Threading;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.SocialPlatforms.Impl;

public class HeroControll : GameEntity
{
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

    [SerializeField]
    private ActionButton _actionButton;

    public ActionButton ActionButton
    {
        get { return _actionButton; }
        set { _actionButton = value; }
    }

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

        Vector3 targetWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        float heroWidth = renderer.bounds.extents.x;
        float heroHeight = renderer.bounds.extents.y;
        _maxScreenWidth = targetWidth.x - heroWidth;
        _maxScreenHeight = targetWidth.y - heroHeight;

        ActionButton.OnBtnClick += new ActionButton.OnButtonClickListener(OnActionButtonClicked);
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

    protected override void OnMove()
    {
        Vector2 movePosition;
        float positionX = _useEditor ? Input.GetAxis("Horizontal") : JoyStickLeft.position.x;
        float positionY = _useEditor ? Input.GetAxis("Vertical") : JoyStickLeft.position.y;

        if (_useEditor)
            movePosition = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed);
        else
            movePosition = new Vector2(JoyStickLeft.position.x * Time.deltaTime * MoveSpeed, JoyStickLeft.position.y * Time.deltaTime * MoveSpeed);

        gameObject.transform.Translate(movePosition);
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -_maxScreenWidth, _maxScreenWidth),
            Mathf.Clamp(transform.position.y, -_maxScreenHeight, _maxScreenHeight));

        _gameObjectAnimator.SetBool("Move", true);
        ChangeAnimationDirection(_gameObjectAnimator, movePosition);
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
        //OnAttack(GetPositionOnDistance(0.2f, GetMoveDirection(Position, new Vector2(_player.transform.position.x, _player.transform.position.y) + GetDirectionAsVector(_player.transform.position))));
    }
}
