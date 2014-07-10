using UnityEngine;
using System.Collections;

public class MusicControll : MonoBehaviour {

    private AudioSource audioSource;

	void Awake () {
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("Music") == "On")
        {
            audioSource.Play();
        }
	}
}
