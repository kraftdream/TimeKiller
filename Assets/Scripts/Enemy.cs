using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Input Variables
    [SerializeField]
	private float _maxDistance;
    public float MaxDistance
    {
        get { return _maxDistance; }
        set { _maxDistance = value; }
    }
    #endregion

    private Transform _heroTarget;
    private MoveScript _moveScript;
	
	void Awake()
    {
        _moveScript = gameObject.GetComponent<MoveScript>();
	}
	
	void Start () 
    {
		GameObject go = GameObject.FindGameObjectWithTag("Player");
        _heroTarget = go.transform;
	}
	
	void Update () 
    {
        if (_heroTarget == null)
						return;
        if (Vector3.Distance(_heroTarget.position, transform.position) > MaxDistance)
        {
            _moveScript.FollowTarget(_heroTarget.position);
		}
	}

    

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player")) 
			Destroy(gameObject);
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter Enemy");
    }
}
