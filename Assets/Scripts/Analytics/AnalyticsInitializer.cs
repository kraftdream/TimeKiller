using UnityEngine;

public class AnalyticsInitializer
{
    private const string FIRST_LAUNCH = "firstLaunch";
    private const int NO = 0;
    private const int YES = 1;

    private static AnalyticsInitializer _instance;
    private AnalyticsInitializer() { }


    public static AnalyticsInitializer Init()
    {
        if (_instance == null)
        {
            _instance = new AnalyticsInitializer();
            AnalyticsInit();
            FirstLaunchEvent();
        }
        return _instance;
    }

    private static void AnalyticsInit()
    {
        Mixpanel.Token = "9a65c57437954c50050e141138bd2c90";

        Mixpanel.SuperProperties.Add("Platform", Application.platform.ToString());
        Mixpanel.SuperProperties.Add("Operating System", SystemInfo.operatingSystem);
        Mixpanel.SuperProperties.Add("Device Model", SystemInfo.deviceModel);
        Mixpanel.SuperProperties.Add("Device Identifier", SystemInfo.deviceUniqueIdentifier);
    }

    private static void FirstLaunchEvent()
    {
        int result = PlayerPrefs.GetInt(FIRST_LAUNCH, YES);
        if (result == YES)
        {
            Mixpanel.SendEvent(AnalyticsEvents.INSTALLS);
            PlayerPrefs.SetInt(FIRST_LAUNCH, NO);
        }
    }
}
