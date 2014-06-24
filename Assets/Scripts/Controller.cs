using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour {

    public float maxSpeed = 3.5f;
    private bool isFacingRight = true;
    private Animator anim;

    Vector3 rawPosition;
    public Camera cam;

    private void Start()
    {
        if (cam == null)
        {
            cam = Camera.main;
        }

        anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 touchDeltaPosition = Input.GetTouch(0).position;
            rawPosition = cam.ScreenToWorldPoint(touchDeltaPosition);
            transform.Translate(rawPosition.x * Time.deltaTime, rawPosition.y * Time.deltaTime, 0);
        }
    }
}
