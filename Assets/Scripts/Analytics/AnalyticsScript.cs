using UnityEngine;

public class AnalyticsScript : MonoBehaviour {

	void Start ()
	{
        Mixpanel.Token = "9a65c57437954c50050e141138bd2c90";

        Mixpanel.SuperProperties.Add("Platform", Application.platform.ToString());
        Mixpanel.SuperProperties.Add("Operating System", SystemInfo.operatingSystem);
        Mixpanel.SuperProperties.Add("Device Model", SystemInfo.deviceModel);
        Mixpanel.SuperProperties.Add("Device Identifier", SystemInfo.deviceUniqueIdentifier);
	}

//    void OnApplicationQuit()
//    {
//        Debug.Log("OnApplicationQuit");
//    }
//
//    void OnApplicationPause()
//    {
//        Debug.Log("OnApplicationPause");
//    }
//
//    void OnDestroy()
//    {
//        Debug.Log("OnDestroy");        
//    }
}
