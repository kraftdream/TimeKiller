using UnityEngine;
using System.Collections;

public class ScreenScaler : MonoBehaviour {

    private float mScreenX = 0;
    private float mScreenY = 0;
    public float mScreenRatio;

	// Use this for initialization
	void Start () {
        FindingRatio();
        SetCameraAspect();
	}

    void FindingRatio() {
        mScreenX = Screen.width;
        mScreenY = Screen.height;
        mScreenRatio = mScreenX / mScreenY;
    }

    void SetCameraAspect() {
        gameObject.camera.aspect = mScreenRatio;
    }
}
