using UnityEngine;
using System.Collections;

public class SoundControll : MonoBehaviour {

    private AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        if (PlayerPrefs.GetString("Sound") == "Off")
        {
            audioSource.mute = true;
        }
        else
        {
            audioSource.mute = false;
        }
    }
}
