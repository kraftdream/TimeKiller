using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;

public class RestartMenu : MonoBehaviour
{
    private const string _leaderBoardID = "CgkIoML55u0ZEAIQAQ";
    private bool _isShowRestart;
    private Rect _postResult;
    private Rect _gameOver;
    private Rect _gameKills;
    private Rect _gameScore;
    private Rect _gameBestScore;
    private Rect _gameCombo;
    private Rect _gameRestart;
    private Rect _gameMainMenu;

    private float _textCenterPoint;
    private const float _textWidth = 250.0f;
    private bool _needToShowStatistics;

    public GUIStyle gameOverStyle;
    public GUIStyle gameButtonsStyle;

    public bool IsShowRestart
    {
        get { return _isShowRestart; }
        set { _isShowRestart = value; }
    }

    public int GameScore
    {
        get;
        set;
    }

    public int GameBestScore
    {
        get;
        set;
    }

    public int Kills
    {
        get; 
        set;
    }

    public int BestCombo
    {
        get; 
        set;
    }

    void Start()
    {
        _textCenterPoint = GetCenterScreen(_textWidth);
		Social.Active = new UnityEngine.SocialPlatforms.GPGSocial();

        _gameMainMenu = new Rect(20, Screen.height - 70, _textWidth, 50);
        _gameRestart = new Rect(Screen.width - 170, Screen.height - 70, _textWidth, 50);
        _postResult = new Rect(Screen.width / 2 - 125, Screen.height - 70, _textWidth, 50);
        _gameOver = new Rect(_textCenterPoint, Screen.height / 2 - 180, _textWidth, 50);
        _gameKills = new Rect(_textCenterPoint / 2, Screen.height / 2 - 50, _textWidth, 50);
        _gameCombo = new Rect(_textCenterPoint / 2, Screen.height / 2 + 10, _textWidth, 50);
        _gameScore = new Rect(_textCenterPoint / 2, Screen.height / 2 + 70, _textWidth, 50);
        _gameBestScore = new Rect(_textCenterPoint / 2, Screen.height / 2 + 130, _textWidth, 50);
    }

    void Update()
    {
        if (_isShowRestart)
        {
            StartCoroutine(StatisticsShow());
        }
    }

    void OnGUI()
    {
        if (_isShowRestart)
        {
            GUI.skin.button = gameButtonsStyle;
            GUI.skin.label = gameButtonsStyle;

            GUI.Label(_gameOver, "GAME OVER", gameOverStyle);

            if (_needToShowStatistics)
            {
                GUI.Label(_gameKills, "Kills: " + Kills);
                GUI.Label(_gameCombo, "Best Combo: "  + BestCombo);
                GUI.Label(_gameScore, "Your Score: " + GameScore);
                GUI.Label(_gameBestScore, "Best Score: " + GameBestScore);

                if (GUI.Button(_gameMainMenu, "Main Menu"))
                {
                    SetNormalGameSpeed();
                    Application.LoadLevel("MainMenu");
                }

                if (GUI.Button(_gameRestart, "Restart"))
                {
                    SetNormalGameSpeed();
                    Application.LoadLevel("GameScene");
                }

                if(GUI.Button(_postResult, "Report Score"))
                {
                    PostResults();
                }
                StopAllCoroutines();
            }
        }
    }

	private void PostResults() 
	{
		Social.localUser.Authenticate(OnLoginSuccess);
	}

	void OnLoginSuccess(bool result)
	{
		if (result) 
			Social.ReportScore(GameScore, _leaderBoardID, OnReportResult);
	}

    private void OnReportResult(bool result)
    {
        if(result)
            NerdGPG.Instance().showLeaderBoards(_leaderBoardID);
    }

    IEnumerator StatisticsShow()
    {
        yield return new WaitForSeconds(0.5f);
        _needToShowStatistics = true;
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