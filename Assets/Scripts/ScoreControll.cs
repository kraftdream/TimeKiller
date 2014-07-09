using System;
using UnityEngine;

public class ScoreControll
{

    private const String BEST_SCORE = "BestScore";

    [SerializeField]
    private float _bestCombo = 0;

    public float BestCombo
    {
        get { return _bestCombo; }
        set
        {
            if (value > _bestCombo)
                _bestCombo = value;
        }

    }

    public int GetBestScore()
	{
        if (PlayerPrefs.HasKey(BEST_SCORE))
	    {
            return PlayerPrefs.GetInt(BEST_SCORE);
	    }
        PlayerPrefs.SetInt(BEST_SCORE, 0);
	    return 0;
	}

    public void SaveScore(int score)
    {
        if (PlayerPrefs.HasKey(BEST_SCORE))
        {
            if (PlayerPrefs.GetInt(BEST_SCORE) < score)
            {
                PlayerPrefs.SetInt(BEST_SCORE, score);
            }
        }
        else
        {
            PlayerPrefs.SetInt(BEST_SCORE, score);
        }
    }
}
