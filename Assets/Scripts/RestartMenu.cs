using UnityEngine;
using System.Collections;

public class RestartMenu : MonoBehaviour
{

    private bool _isShowRestart;
    private Rect _gameStart;
    private Rect _gameOptions;
    private Rect _gameExit;

    void Start()
    {
        _gameStart = new Rect(Screen.width / 2, Screen.height / 2, 150, 50);
        _gameOptions = new Rect(Screen.width / 2, Screen.height / 2 + 80, 150, 50);
        _gameExit = new Rect(Screen.width / 2, Screen.height / 2 + 160, 150, 50);
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

            if (GUI.Button(_gameStart, "Restart"))
            {
                Time.timeScale = 1;
                Application.LoadLevel("GameScene");
            }

            if (GUI.Button(_gameOptions, "Main Menu"))
            {
                Time.timeScale = 1;
                Application.LoadLevel("MainMenu");
            }

            if (GUI.Button(_gameExit, "Exit"))
            {
                Time.timeScale = 1;
                Application.Quit();
            }
        }
    }
}


