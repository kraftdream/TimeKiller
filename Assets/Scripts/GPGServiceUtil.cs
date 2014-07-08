using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine.SocialPlatforms;

public class GPGServiceUtil : MonoBehaviour {

	private string _leaderBoardID = "CgkIoML55u0ZEAIQAQ";

	// Use this for initialization
	void Start () {
		Social.Active = new UnityEngine.SocialPlatforms.GPGSocial();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
