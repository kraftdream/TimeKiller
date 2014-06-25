using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    private Joystick joyStickLeft = null;

	[Range(1, 20)]
	public float moveForce;

    void Start()
    {
        if (joyStickLeft == null)
        {
            GameObject objjoyStickLeft = GameObject.FindGameObjectWithTag("JoystickTag") as GameObject;
            joyStickLeft = objjoyStickLeft.GetComponent<Joystick>();
        }
    }

    void Update()
    {
        Vector3 position = this.transform.position;

        float xMovement = 0;
        float yMovement = 0;

		gameObject.transform.Translate (new Vector3 (joyStickLeft.position.x * Time.deltaTime * moveForce, joyStickLeft.position.y * Time.deltaTime * moveForce, 0));

    }

    float joyStickInput(Joystick jstick)
    {
        Vector2 absJoyPos = new Vector2(Mathf.Abs(jstick.position.x),
                                        Mathf.Abs(jstick.position.y));

        int xDirection = (jstick.position.x > 0) ? 1 : -1;
        int yDirection = (jstick.position.y > 0) ? 1 : -1;

        return ((absJoyPos.x > absJoyPos.y) ? absJoyPos.x * xDirection : absJoyPos.y * yDirection);
    }
}
