using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private Rect _gameName;
    private Rect _gameStart;
    private Rect _gameOptions;
    private Rect _gameExit;

    public GUIStyle gameNameStyle;
    public GUIStyle gameButtonsStyle;
	
	void Start () {
	    _gameName = new Rect(Screen.width / 2, Screen.height / 2 - 150, 10, 10);
        _gameStart = new Rect(Screen.width / 2 - 90 , Screen.height / 2, 160, 50);
        _gameOptions = new Rect(Screen.width / 2 - 90, Screen.height / 2 + 80, 160, 50);
        _gameExit = new Rect(Screen.width / 2 - 90, Screen.height / 2 + 160, 160, 50);
	}
	
	void OnGUI () {
        
        GUI.Label(_gameName, "Time Killer", gameNameStyle);
	    GUI.skin.button = gameButtonsStyle;
        
        if (GUI.Button(_gameStart, "Start"))
	    {
            Application.LoadLevel("GameScene");
	    }

        if (GUI.Button(_gameOptions, "Options"))
        {
            
        }

        if (GUI.Button(_gameExit, "Exit"))
        {
            Application.Quit();
        }
	}
}
