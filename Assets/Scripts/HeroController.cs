﻿using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    private enum axis { Left, Rigth, Up, Down };

    public Joystick joyStickLeft;

    private int[] moveAxis; //left, right, up, down

    private Vector3 heroParams;
    private Vector3 bordersParams;
    private Vector3 textureParams;

	[Range(1, 20)]
	public float moveForce;
    [Range(0, 1)]
    public float borderLeftRightWitdh;
    [Range(0, 1)]
    public float borderUpDownWitdh;

    void Awake() {
        moveAxis = new[] { 1, 1, 1, 1 };

		if (joyStickLeft == null)
			Debug.LogError ("Please set the joystick prefab!");
    }

    void Update() {

        heroParams = transform.position;
        bordersParams = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));
        textureParams = gameObject.GetComponent<SpriteRenderer>().bounds.size;

        if (joyStickLeft.position.x > 0.0f && heroParams.x + textureParams.x / 2 > bordersParams.x - borderLeftRightWitdh)
            moveAxis[(int)axis.Rigth] = 0;
        else
            moveAxis[(int)axis.Rigth] = 1;

        if (joyStickLeft.position.x < 0.0f && heroParams.x - textureParams.x / 2 < borderLeftRightWitdh - bordersParams.x)
            moveAxis[(int)axis.Left] = 0;
        else
            moveAxis[(int)axis.Left] = 1;

        if (joyStickLeft.position.y > 0.0f && heroParams.y + textureParams.y / 2 > bordersParams.y - borderUpDownWitdh)
            moveAxis[(int)axis.Up] = 0;
        else
            moveAxis[(int)axis.Up] = 1;

        if (joyStickLeft.position.y < 0.0f && heroParams.y - textureParams.y / 2 < borderUpDownWitdh - bordersParams.y)
            moveAxis[(int)axis.Down] = 0;
        else
            moveAxis[(int)axis.Down] = 1;

		if (joyStickLeft != null)
            gameObject.transform.Translate(new Vector3(moveAxis[(int)axis.Left] * moveAxis[(int)axis.Rigth] * joyStickLeft.position.x * Time.deltaTime * moveForce, moveAxis[(int)axis.Up] * moveAxis[(int)axis.Down] * joyStickLeft.position.y * Time.deltaTime * moveForce, 0));
	}
}
