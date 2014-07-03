using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity, GameEvents
{
    public enum GameEntityState
    {
        Move, Prepare, Attack
    }

    // похибка для координати х, щоб персонаж не відразу перевернувся з ліва вправо або навпаки
    private const float _error = 0.8f;
	private const float _prepare_decrement = 0.01f;

    private float _prepareTime;

    [SerializeField]
    private Animator _enemyAnimator;
    private GameEntityState _state;
    private Vector2 _attackToPossition;

    public GameEntityState State
    {
        get { return _state; }
    }

    public float PrepareTime
    {
        get { return _prepareTime; }
        set { _prepareTime = value; }
    }

	// Use this for initialization
	void Awake ()
	{
        _enemyAnimator = GetComponent<Animator>();
	}

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
    }
	// Update is called once per frame
	void Update () {

	}

    void ChangeAnimationPosition(Vector3 _targetPos, Vector3 _currentPos)
    {
        /*_enemyAnimator.SetBool(Directions.TOP, false);
        _enemyAnimator.SetBool(Directions.BOTTOM, false);
        _enemyAnimator.SetBool(Directions.LEFT, false);
        _enemyAnimator.SetBool(Directions.RIGHT, false);
        if (Mathf.Abs(_targetPos.x - _currentPos.x) > _error)
        {

            if (_targetPos.x <= _currentPos.x)
                _enemyAnimator.SetBool(Directions.LEFT, true);
            else
                _enemyAnimator.SetBool(Directions.RIGHT, true);
        }
        else
        {
            if (_targetPos.y <= _currentPos.y)
                _enemyAnimator.SetBool(Directions.BOTTOM, true);
            else
                _enemyAnimator.SetBool(Directions.TOP, true);
        }*/
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
        //gameObject.ch
        //get Player game object
    }
}
