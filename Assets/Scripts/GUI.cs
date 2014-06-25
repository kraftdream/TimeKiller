using UnityEngine;
using System.Collections;

public class GUI : MonoBehaviour {

    public int score;
    public int killCoount;
    private Time timer;

	void Start () {
        GameObject score = new GameObject("Score");
        score.transform.parent = transform;

        score.AddComponent<TextMesh>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
