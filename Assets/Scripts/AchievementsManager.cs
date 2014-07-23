using System;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class AchievementsManager : MonoBehaviour
{
    public HeroControll hero;
    private const String LOCKED = "Locked";
    private const String UNLOCKED = "Unlocked";

    private const String KILLVAMPIRES_ID = "CgkIoML55u0ZEAIQAw";

    private Achievement[] achievements;
    public delegate void OnAchievementUnlocked (Achievement achievement);
    public event OnAchievementUnlocked AchievementUnlocked;

    private static AchievementsManager instance;
 
    void Awake()
    {
        if (instance)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }
        achievements = new []{ new KillVampires("KillVampires", 10, hero) };
    }

    public static AchievementsManager GetInstance()
    {
        return instance;
    }

    private void UnlockAchievement(Achievement achievement)
    {
        String achievementName = achievement.GetName();
        if (!PlayerPrefs.HasKey(achievementName) && PlayerPrefs.GetString(achievementName) == LOCKED)
        {
            PlayerPrefs.SetString(achievementName, UNLOCKED);
            AchievementUnlocked(achievement);
            if (Social.localUser.authenticated)
            {
                Social.ReportProgress(KILLVAMPIRES_ID, 100.0, OnUnlockAC);
            }
        }
    }

    private Achievement GetAchievementByName(string achievementName)
    {
        return achievements.FirstOrDefault(achievement => achievement.GetName() == achievementName);
    }

    public void ReportAchievementProgress(string achivementName, int progress)
    {
        Achievement achivement = GetAchievementByName(achivementName);
        if (achivement.AddProgress(progress))
        {
            achivement.AddBonus();
            UnlockAchievement(achivement);
        }
    }

    public void OnUnlockAC(bool result)
    {
        Debug.Log("GPGUI: OnUnlockAC " + result);
    }
}
