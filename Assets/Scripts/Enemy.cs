using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {
	public Transform target;
	public float moveSpeed;
	public float maxdistance;
	
	private Transform myTransform;
	
	void Awake(){
		myTransform = transform;
	}
	
	void Start () {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
		target = go.transform;
	}
	
	void Update () {
		if (target == null)
						return;
		if(Vector3.Distance(target.position, myTransform.position) > maxdistance){
			myTransform.position = Vector3.MoveTowards(myTransform.position, target.position, moveSpeed * Time.deltaTime);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player")) {
			Destroy(gameObject);
		}
	}
}
