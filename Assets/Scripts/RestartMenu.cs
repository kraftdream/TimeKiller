using UnityEngine;
using System.Collections;

public class RestartMenu : MonoBehaviour
{
    private bool _isShowRestart;
    private Rect _gameStart;
    private Rect _gameOptions;
    private Rect _gameExit;
    private float _textCenterPoint;
    private float _textWidth = 250.0f;
    public GUIStyle gameButtonsStyle;

    void Start()
    {
        _textCenterPoint = GetCenterScreen(_textWidth);

        _gameStart = new Rect(_textCenterPoint, Screen.height / 2 - 90, _textWidth, 50);
        _gameOptions = new Rect(_textCenterPoint, Screen.height / 2, _textWidth, 50);
        _gameExit = new Rect(_textCenterPoint, Screen.height / 2 + 90, _textWidth, 50);
    }

    public bool IsShowRestart
    {
        get { return _isShowRestart; }
        set { _isShowRestart = value; }
    }

    void OnGUI()
    {
        if (_isShowRestart)
        {
            GUI.skin.button = gameButtonsStyle;

            if (GUI.Button(_gameStart, "Restart"))
            {
                SetNormalGameSpeed();
                Application.LoadLevel("GameScene");
            }

            if (GUI.Button(_gameOptions, "Main Menu"))
            {
                SetNormalGameSpeed();
                Application.LoadLevel("MainMenu");
            }

            if (GUI.Button(_gameExit, "Exit"))
            {
                SetNormalGameSpeed();
                Application.Quit();
            }
        }
    }

    private float GetCenterScreen(float textWidth)
    {
        return Screen.width / 2 - (textWidth / 2);
    }

    void SetNormalGameSpeed()
    {
        Time.timeScale = 1;        
    }
}


