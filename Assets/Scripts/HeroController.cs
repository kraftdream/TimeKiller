using UnityEngine;
using System.Collections;

public class HeroController : MonoBehaviour {

    public Joystick joyStickLeft = null;

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

        if (joyStickLeft.position.x < 0) 
        { 
            xMovement = xMovement - 0.05f; 
        } 
        
        if (joyStickLeft.position.x > 0)
        {
            xMovement = xMovement + 0.05f;
        }

        if (joyStickLeft.position.y < 0) 
        { 
            yMovement = yMovement - 0.05f; 
        } 
        
        if (joyStickLeft.position.y > 0)
        {
            yMovement = yMovement + 0.05f;
        }

        position.x = position.x + xMovement;
        position.y = position.y + yMovement;
        this.transform.position = position;

        Debug.Log("x " + joyStickLeft.position.x + ", y " + joyStickLeft.position.y);
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
