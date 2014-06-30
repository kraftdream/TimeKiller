using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{
    #region Input Variables

    [SerializeField]
    private List<GameObject> _enemyList;

    [SerializeField]
	private float _attackDistance;
    public float AttackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
    }

    [SerializeField]
    private int _scoreValue;

    public int ScoreValue
    {
        get { return _scoreValue; }
        set { _scoreValue = value; }
    }

    public List<GameObject> EnemyList
    {
        get { return _enemyList; }
        set { _enemyList = value; }
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
        //if (Vector3.Distance(_heroTarget.position, transform.position) > AttackDistance)
        //{
            _moveScript.FollowTarget(_heroTarget.position);
		//}
	}

    

	void OnTriggerEnter2D(Collider2D other)
	{
	    if (other.gameObject.CompareTag("Player"))
	    {
			EnemyList.Remove(gameObject);
	        Destroy(gameObject);
	    }
	}

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter Enemy");
    }
}
