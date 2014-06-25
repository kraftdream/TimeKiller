using UnityEngine;
using System.Collections;

public class Controller : MonoBehaviour
{

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
        #if UNITY_EDITOR
		if(Input.GetMouseButton(1))
			maxSpeed = 50f;
		else
			maxSpeed = 3.5f;
        if (Input.GetMouseButton(0))
        {
            Vector3 touchDeltaPosition = Input.mousePosition;
            rawPosition = cam.ScreenToWorldPoint(touchDeltaPosition);
			rawPosition.z = 0;
			anim.SetFloat("SpeedX", rawPosition.x);
			anim.SetFloat("SpeedY", rawPosition.y);
			//transform.Translate(rawPosition.x * maxSpeed * Time.deltaTime, rawPosition.y * maxSpeed * Time.deltaTime, 0);
			transform.position = Vector3.MoveTowards(transform.position, rawPosition, maxSpeed * Time.deltaTime);
        }

        #endif
		if(Input.touchCount > 1 && Input.GetTouch(1).phase == TouchPhase.Began)
			maxSpeed = 50f;
		else
			maxSpeed = 3.5f;
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector3 touchDeltaPosition = Input.GetTouch(0).position;
            rawPosition = cam.ScreenToWorldPoint(touchDeltaPosition);
            //transform.Translate(rawPosition.x * maxSpeed * Time.deltaTime, rawPosition.y * maxSpeed * Time.deltaTime, 0);
			rawPosition.z = 0;
			transform.position = Vector3.MoveTowards(transform.position, rawPosition, maxSpeed * Time.deltaTime);
        }
    }

	//void OnTriggerEnter2D(Collider2D other) {
	//	if (other.gameObject.CompareTag("Enemy")){
	//		Destroy(gameObject);
	//		Application.Quit();
	//	}
	//}
}
