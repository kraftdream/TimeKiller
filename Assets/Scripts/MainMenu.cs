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
    private Rect _optionsSound;
    private Rect _optionsMusic;
    private Rect _optionsVibrate;
    private Rect _optionsBlood;

    private float _textCenterPoint;

    private const float _textSpeed = 2000.0f;
    private const float _delayTime = 0.2f;
    private const int _screenOffset = 300;

    private bool options = false;
    private bool back = true;
    private bool _touchPhaseBegan;

    public GUIStyle gameNameStyle;
    public GUIStyle gameButtonsStyle;

    private Matrix4x4 transMatrix;
    private Vector3 positionVec;

    private AudioSource audioSource;

    private bool sound = true;

    private const String ON = "On";
    private const String OFF = "Off";
    private const String MUSIC = "Music";
    private const String SOUND = "Sound";
    private const String VIBRATE = "Vibrate";
    private const String BLOOD = "Blood";

    private const float WIDTH_SG = 1280;
    private const float HEIGHT_SG = 720;

    private float _koefX;
    private float _koefY;

    void Awake()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (PlayerPrefs.GetString(MUSIC) == ON)
        {
            audioSource.mute = false;
        }
        else
        {
            audioSource.mute = true;
        }

        _koefX = Screen.width / WIDTH_SG;
        _koefY = Screen.height / HEIGHT_SG;
    }

    void Start()
    {

        transMatrix = Matrix4x4.identity;
        positionVec = Vector3.zero;

        _gameName = new Rect(Screen.width / 2, Screen.height / 2 - (140 * _koefY), (150 * _koefX), (10 * _koefY));
        _gameStart = new Rect(Screen.width / 2, Screen.height / 2, (150 * _koefX), (60 * _koefY));
        _gameOptions = new Rect(Screen.width / 2, Screen.height / 2 + (90 * _koefY), (150 * _koefX), (60 * _koefY));
        _gameExit = new Rect(Screen.width / 2, Screen.height / 2 + (180 * _koefY), (150 * _koefX), (60 * _koefY));

        _optionsMusic = new Rect((Screen.width + (Screen.width / 2)) - (150 * _koefX), Screen.height / 2 - (160 * _koefY), (300 * _koefX), (50 * _koefY));
        _optionsSound = new Rect((Screen.width + (Screen.width / 2)) - (150 * _koefX), Screen.height / 2 - (80 * _koefY), (300 * _koefX), (50 * _koefY));
        _optionsVibrate = new Rect((Screen.width + (Screen.width / 2)) - (150 * _koefX), Screen.height / 2, (300 * _koefX), (50 * _koefY));
        _optionsBlood = new Rect((Screen.width + (Screen.width / 2)) - (150 * _koefX), Screen.height / 2 + (80 * _koefY), (300 * _koefX), (50 * _koefY));
        _optionsBack = new Rect((Screen.width + (Screen.width / 2)) - (75 * _koefX), Screen.height / 2 + (240 * _koefY), (150 * _koefX), (50 * _koefY));

        _gameName.x = _gameOptions.x = -_screenOffset;
        _gameStart.x = _gameExit.x = Screen.width + _screenOffset;

        _textCenterPoint = GetCenterScreen(_gameOptions.width);

        SetTextSize();
    }

    void SetTextSize()
    {
        float gameNameSize = gameNameStyle.fontSize *_koefX;
        gameNameStyle.fontSize = (int) gameNameSize;
        float optionsNameSize = gameButtonsStyle.fontSize * _koefX;
        gameButtonsStyle.fontSize = (int) optionsNameSize;
    }

    void Update()
    {
        StartCoroutine(StartTextAnimation());

        if (options)
        {
            positionVec.x = Mathf.SmoothStep(positionVec.x, -Screen.width, Time.deltaTime * 10);
            transMatrix = Matrix4x4.TRS(positionVec, Quaternion.identity, Vector3.one);
        }
        else if (back)
        {
            positionVec.x = Mathf.SmoothStep(positionVec.x, 0, Time.deltaTime * 10);
            transMatrix = Matrix4x4.TRS(positionVec, Quaternion.identity, Vector3.one);
        }

        if (PlayerPrefs.GetString(MUSIC) == OFF)
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (options)
            {
                options = false;
                back = true;
            }
            else
            {
                Application.Quit();
            }
        }
    }

    void OnGUI()
    {
        GUI.matrix = transMatrix;

        //Main Menu Screen
        GUI.Label(_gameName, "Blade Hunter", gameNameStyle);

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

        if (GUI.Button(_optionsSound, CheckSound()))
        {
            SwitchSound();
        }

        if (GUI.Button(_optionsMusic, CheckMusic()))
        {
            SwitchMusic();
        }

        if (GUI.Button(_optionsVibrate, CheckVibrate()))
        {
            SwitchVibrate();
        }

        if (GUI.Button(_optionsBlood, CheckBlood()))
        {
            SwitchBlood();
        }

        if (GUI.Button(_optionsBack, "Back"))
        {
            options = false;
            back = true;
        }
    }

    private string CheckMusic()
    {
        if (PlayerPrefs.HasKey(MUSIC))
        {
            if (PlayerPrefs.GetString(MUSIC) == ON)
            {
                return MUSIC + " " + OFF;
            }
            if (PlayerPrefs.GetString(MUSIC) == OFF)
            {
                return MUSIC + " " + ON;
            }
        }
        else
        {
            PlayerPrefs.SetString(MUSIC, ON);
        }
        return MUSIC + " " + OFF;
    }

    void SwitchMusic()
    {
        if (PlayerPrefs.GetString(MUSIC) == ON)
        {
            PlayerPrefs.SetString(MUSIC, OFF);
        }
        else if (PlayerPrefs.GetString(MUSIC) == OFF)
        {
            PlayerPrefs.SetString(MUSIC, ON);
        }
    }

    void SwitchSound()
    {
        if (PlayerPrefs.GetString(SOUND) == ON)
        {
            PlayerPrefs.SetString(SOUND, OFF);
        }
        else if (PlayerPrefs.GetString(SOUND) == OFF)
        {
            PlayerPrefs.SetString(SOUND, ON);
        }
    }

    String CheckSound()
    {
        if (PlayerPrefs.HasKey(SOUND))
        {
            if (PlayerPrefs.GetString(SOUND) == ON)
            {
                return SOUND + " " + OFF;
            }
            if (PlayerPrefs.GetString(SOUND) == OFF)
            {
                return SOUND + " " + ON;
            }
        }
        else
        {
            PlayerPrefs.SetString(SOUND, ON);
        }
        return SOUND + " " + OFF;
    }

    private string CheckVibrate()
    {
        if (PlayerPrefs.HasKey(VIBRATE))
        {
            if (PlayerPrefs.GetString(VIBRATE) == OFF)
            {
                return VIBRATE + " " + ON;
            }
            if (PlayerPrefs.GetString(VIBRATE) == ON)
            {
                return VIBRATE + " " + OFF;
            }
        }
        else
        {
            PlayerPrefs.SetString(VIBRATE, OFF);
        }
        return VIBRATE + " " + ON;
    }

    void SwitchVibrate()
    {
        if (PlayerPrefs.GetString(VIBRATE) == ON)
        {
            PlayerPrefs.SetString(VIBRATE, OFF);
        }
        else if (PlayerPrefs.GetString(VIBRATE) == OFF)
        {
            PlayerPrefs.SetString(VIBRATE, ON);
        }
    }

    private string CheckBlood()
    {
        if (PlayerPrefs.HasKey(BLOOD))
        {
            if (PlayerPrefs.GetString(BLOOD) == OFF)
            {
                return BLOOD + " " + ON;
            }
            if (PlayerPrefs.GetString(BLOOD) == ON)
            {
                return BLOOD + " " + OFF;
            }
        }
        else
        {
            PlayerPrefs.SetString(BLOOD, ON);
        }
        return BLOOD + " " + OFF;
    }

    void SwitchBlood()
    {
        if (PlayerPrefs.GetString(BLOOD) == ON)
        {
            PlayerPrefs.SetString(BLOOD, OFF);
        }
        else if (PlayerPrefs.GetString(BLOOD) == OFF)
        {
            PlayerPrefs.SetString(BLOOD, ON);
        }
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
