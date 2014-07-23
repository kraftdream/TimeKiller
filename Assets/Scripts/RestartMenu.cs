using UnityEngine;
using System.Collections;

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

    private const string MAIN_MENU = "MainMenu";
    private const string REPORT_SCORE = "Report Score";
    private const string RESTART = "Restart";
    private const string PURCHASE_HEALTH = "Buy More Health";

    private float _textCenterPoint;
    private float _textWidth = 250.0f;
    private bool _needToShowStatistics;

    public GUIStyle gameOverStyle;
    public GUIStyle gameButtonsStyle;

    private const float WIDTH_SG = 1280;
    private const float HEIGHT_SG = 720;

    private float _koefX;
    private float _koefY;

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

    void Awake()
    {
        _koefX = Screen.width / WIDTH_SG;
        _koefY = Screen.height / HEIGHT_SG;
    }

    void Start()
    {
        SetTextSize();

        _textCenterPoint = GetCenterScreen(_textWidth);
		

        _gameMainMenu = new Rect((20 * _koefX), Screen.height - (70 * _koefY), GetTextWidth(MAIN_MENU), (50 * _koefY));
        _gameRestart = new Rect(Screen.width - (170 * _koefX), Screen.height - (70 * _koefY), GetTextWidth(RESTART), (50 * _koefY));
        _postResult = new Rect(Screen.width / 2 - (125 * _koefX), Screen.height - (70 * _koefX), GetTextWidth(REPORT_SCORE), (50 * _koefY));
        _gameOver = new Rect(_textCenterPoint, Screen.height / 2 - (180 * _koefY), _textWidth, (50 * _koefY));
        _gameKills = new Rect(_textCenterPoint / 2, Screen.height / 2 - (50 * _koefY), _textWidth, (50 * _koefY));
        _gameCombo = new Rect(_textCenterPoint / 2, Screen.height / 2 + (10 * _koefY), _textWidth, (50 * _koefY));
        _gameScore = new Rect(_textCenterPoint / 2, Screen.height / 2 + (70 * _koefY), _textWidth, (50 * _koefY));
        _gameBestScore = new Rect(_textCenterPoint / 2, Screen.height / 2 + (130 * _koefY), _textWidth, (50 * _koefY));
    }

    int GetTextWidth(string target)
    {
        return (int) gameButtonsStyle.CalcSize(new GUIContent(target)).x;
    }

    void SetTextSize()
    {
        float gameOverSize = gameOverStyle.fontSize * _koefX;
        gameOverStyle.fontSize = (int) gameOverSize;
        float optionsNameSize = gameButtonsStyle.fontSize * _koefX;
        gameButtonsStyle.fontSize = (int)optionsNameSize;
        _textWidth = _textWidth * _koefX;
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
                    Application.LoadLevel(MAIN_MENU);
                }

                if (GUI.Button(_gameRestart, RESTART))
                {
                    SetNormalGameSpeed();
                    Application.LoadLevel("GameScene");
                }

                if (GUI.Button(_postResult, REPORT_SCORE))
                {
                    PostResults();
                }

                StopAllCoroutines();
            }
        }
    }

	private void PostResults() 
	{
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