using System;
using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private Rect _gameName;
    private Rect _gameStart;
    private Rect _gameOptions;
    private Rect _gameExit;
    private Rect _optionsBack;
    private Rect _optionsMute;
    private float _textCenterPoint;

    private const float _textSpeed = 2000.0f;
    private const float _delayTime = 0.2f;
    private const int _screenOffset = 300;

    private bool options = false;
    private bool back = true;

    public GUIStyle gameNameStyle;
    public GUIStyle gameButtonsStyle;

    private Matrix4x4 transMatrix;
    private Vector3 positionVec;

    private bool sound = true;

    void Start()
    {
        transMatrix = Matrix4x4.identity;
        positionVec = Vector3.zero;

        _gameName = new Rect(Screen.width / 2, Screen.height / 2 - 150, 150, 10);
        _gameStart = new Rect(Screen.width / 2, Screen.height / 2, 150, 50);
        _gameOptions = new Rect(Screen.width / 2, Screen.height / 2 + 80, 150, 50);
        _gameExit = new Rect(Screen.width / 2, Screen.height / 2 + 160, 150, 50);
        _optionsBack = new Rect((Screen.width + (Screen.width / 2)) - 75, Screen.height / 2 + 160, 150, 50);
        _optionsMute = new Rect((Screen.width + (Screen.width / 2)) - 75, Screen.height / 2 + 80, 150, 50);

        _gameName.x = _gameOptions.x = - _screenOffset;
        _gameStart.x = _gameExit.x = Screen.width + _screenOffset;


        _textCenterPoint = GetCenterScreen(_gameStart.width);
    }

    void Update()
    {
        StartCoroutine(StartTextAnimation());

        if (options)
        {
            positionVec.x = Mathf.SmoothStep(positionVec.x, -Screen.width, Time.deltaTime*10);
            transMatrix = Matrix4x4.TRS(positionVec, Quaternion.identity, Vector3.one); 
        } else if (back)
        {
            positionVec.x = Mathf.SmoothStep(positionVec.x, 0, Time.deltaTime * 10);
            transMatrix = Matrix4x4.TRS(positionVec, Quaternion.identity, Vector3.one);
        }


        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();            
        }
    }

    void OnGUI()
    {
        GUI.matrix = transMatrix;
        
        //Main Menu Screen
        GUI.Label(_gameName, "Time Killer", gameNameStyle);
        GUI.skin.button = gameButtonsStyle;

        if (GUI.Button(_gameStart, "Start"))
        {
            Application.LoadLevel("GameScene");
        }

        if (GUI.Button(_gameOptions, "Options"))
        {
            options = true;
            back = false;
        }

        if (GUI.Button(_gameExit, "Exit"))
        {
            Application.Quit();
        }
        
        //Options Screen

        if (GUI.Button(_optionsMute, CheckSound()))
        {
            SwitchSound();
        }

        if (GUI.Button(_optionsBack, "Back"))
        {
            options = false;
            back = true;
        }
    }

    void SwitchSound()
    {
        if (PlayerPrefs.GetString("Sound") == "On")
        {
            PlayerPrefs.SetString("Sound", "Off");
        }
        else if (PlayerPrefs.GetString("Sound") == "Off")
        {
            PlayerPrefs.SetString("Sound", "On");
        }
    }

    String CheckSound()
    {
        if (PlayerPrefs.HasKey("Sound"))
        {
            if (PlayerPrefs.GetString("Sound") == "On")
            {
                return "Sound Off";
            }
            if (PlayerPrefs.GetString("Sound") == "Off")
            {
                return "Sound On";
            }
        }
        else
        {
            PlayerPrefs.SetString("Sound", "On");
        }
        return "Sound Off";
    }

    IEnumerator StartTextAnimation()
    {
        if (_textCenterPoint >= _gameName.x)
        {
            _gameName.x = Mathf.MoveTowards(_gameName.x, _textCenterPoint, Time.deltaTime * _textSpeed);
        }
        yield return new WaitForSeconds(_delayTime);

        if (_textCenterPoint <= _gameStart.x)
        {
            _gameStart.x = Mathf.MoveTowards(_gameStart.x, _textCenterPoint, Time.deltaTime * _textSpeed);
        }

        yield return new WaitForSeconds(_delayTime);

        if (_textCenterPoint >= _gameOptions.x)
        {
            _gameOptions.x = Mathf.MoveTowards(_gameOptions.x, _textCenterPoint, Time.deltaTime * _textSpeed);
        }

        yield return new WaitForSeconds(_delayTime);

        if (_textCenterPoint <= _gameExit.x)
        {
            _gameExit.x = Mathf.MoveTowards(_gameExit.x, _textCenterPoint, Time.deltaTime * _textSpeed);
        }
    }

    private float GetCenterScreen(float textWidth)
    {
        return Screen.width / 2 - (textWidth / 2);
    }
}
