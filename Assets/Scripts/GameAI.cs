using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity, GameEvents
{
    public enum GameEntityState
    {
        Move, Prepare, Attack
    }

    #region Input Variables
	private const float _prepare_decrement = 0.01f;

    [SerializeField]
    private float _prepareTime;

    [SerializeField]
    private Transform _targetTransform;

    private Animator _enemyAnimator;
    private GameEntityState _state;
    private Vector2 _attackToPossition;

    public Transform TargetTransform
    {
        get { return _targetTransform; }
        set { _targetTransform = value; }
    }

    public GameEntityState State
    {
        get { return _state; }
    }

    public float PrepareTime
    {
        get { return _prepareTime; }
        set { _prepareTime = value; }
    }

    public List<GameObject> EnemyList { get; set; }

    #endregion
	// Use this for initialization
	void Awake ()
	{
        _enemyAnimator = GetComponent<Animator>();
	}

	// Update is called once per frame
	void Update () {

	}

    void ChangeAnimationPosition(Vector3 _targetPos, Vector3 _currentPos)
    {
        _enemyAnimator.SetBool(Directions.Top.ToString(), false);
		_enemyAnimator.SetBool(Directions.Bottom.ToString(), false);
		_enemyAnimator.SetBool(Directions.Left.ToString(), false);
        _enemyAnimator.SetBool(Directions.Right.ToString(), false);
		_enemyAnimator.SetBool (GetDirection (TargetTransform.position).ToString(), true);
    }

    public void OnMove()
    {
        MoveToWorldPoint(TargetTransform.position.x, TargetTransform.position.y, MoveSpeed);
        ChangeAnimationPosition(TargetTransform.position, Position);
        _state = GameEntityState.Move;
		_prepareTime = 1.0f;
		_enemyAnimator.speed = 1.0f;
		_enemyAnimator.SetBool ("Prepare", false);
    }

    public void OnPrepare()
    {
        _attackToPossition = GetPositionOnDistance(AttackDistance + 2,
        GetMoveDirection(Position, new Vector2(TargetTransform.position.x, TargetTransform.position.y)));
        _state = GameEntityState.Prepare;
		_enemyAnimator.SetBool ("Prepare", true);
		Directions direction = GetDirection (_attackToPossition);
		switch (direction) 
		{
			case Directions.Bottom:
				_enemyAnimator.SetFloat("Direction", 0.0f);
				break;
			case Directions.Top:
				_enemyAnimator.SetFloat("Direction", 1.0f);
				break;
			case Directions.Left:
				_enemyAnimator.SetFloat("Direction", 2.0f);
				break;
			case Directions.Right:
				_enemyAnimator.SetFloat("Direction", 3.0f);
				break;
		}
		_prepareTime -= _prepare_decrement;
		_enemyAnimator.speed += _prepare_decrement;
        //Timer to prepare
        //Debug.Log("OnPrepare");
    }

    public void OnAttack()
    {
		_enemyAnimator.SetBool ("Prepare", false);
        _state = GameEntityState.Attack;
        MoveToWorldPoint(_attackToPossition.x, _attackToPossition.y, MoveSpeed * MoveSpeed);

        if (Position.Equals(_attackToPossition))
        {
            _state = GameEntityState.Move;
            OnMove();
        }

        //Debug.Log("OnAttack");
    }

    public void OnCollision(GameObject collisionObject)
    {
        //get Player game object
    }
}
