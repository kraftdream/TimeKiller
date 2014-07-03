using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity
{

    #region Input Variables
	private const float _prepare_decrement = 0.01f;

    [SerializeField]
    private Transform _targetTransform;

    public Transform TargetTransform
    {
        get { return _targetTransform; }
        set { _targetTransform = value; }
    }

    public List<GameObject> EnemyList { get; set; }

    #endregion

	// Update is called once per frame

    protected override void OnMove(Vector2 _movePosition)
    {
        MoveToWorldPoint(_movePosition.x, _movePosition.y, MoveSpeed);
		ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
		_gameObjectAnimator.speed = 1.0f;
		_gameObjectAnimator.SetBool ("Prepare", false);
    }

	protected override void OnPrepare(Vector2 _attackToPossition)
    {
		_gameObjectAnimator.SetBool ("Prepare", true);
		Directions direction = GetDirection (_attackToPossition);
		switch (direction) 
		{
			case Directions.Bottom:
				_gameObjectAnimator.SetFloat("Direction", 0.0f);
				break;
			case Directions.Top:
				_gameObjectAnimator.SetFloat("Direction", 1.0f);
				break;
			case Directions.Left:
				_gameObjectAnimator.SetFloat("Direction", 2.0f);
				break;
			case Directions.Right:
				_gameObjectAnimator.SetFloat("Direction", 3.0f);
				break;
		}
		PrepareTime -= _prepare_decrement;
		_gameObjectAnimator.speed += _prepare_decrement;
    }

	protected void OnAttack(Vector2 _attackPosition)
    {
        //base.OnAttack(_attackPosition);
		_gameObjectAnimator.SetBool ("Prepare", false);
       
    }

    protected override void OnCollision(GameEntity collisionObject)
    {
        //get Player game object
    }

    protected override void OnBlink()
    {
    }
}
