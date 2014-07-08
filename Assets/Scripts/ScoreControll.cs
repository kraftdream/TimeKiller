using UnityEngine;

public class ScoreControll
{
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
	    if (PlayerPrefs.HasKey("BestScore"))
	    {
	        return PlayerPrefs.GetInt("BastScore");
	    }
        PlayerPrefs.SetInt("BestScore", 0);
	    return 0;
	}

    public void SaveScore(int score)
    {
        if (PlayerPrefs.HasKey("BestScore"))
        {
            if (PlayerPrefs.GetInt("BestScore") < score)
            {
                PlayerPrefs.SetInt("BestScore", score);
            }
        }
        else
        {
            PlayerPrefs.SetInt("BestScore", score);
        }
    }
}
