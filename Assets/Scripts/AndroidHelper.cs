using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class AndroidHelperClass : MonoBehaviour
{

    private static AndroidHelperClass _instance;
    public static AndroidHelperClass Instance
    {
        get
        {
            if (_instance == null) _instance = new AndroidHelperClass();
            return _instance;
        }
    }

    public void logAndroid(string tag, string message)
    {
        AndroidJavaObject jo = new AndroidJavaObject("com.nexxmobile.unityplugin.AndroidLogger");
        jo.Call("logAndroid", tag, message);
    }

#if UNITY_ANDROID
    private static AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");


    public void makeToast(string message)
    {
        jo.Call("makeToast", message);
    }

#endif
    void OnApplicationQuit()
    {
        _instance = null;
    }
}
