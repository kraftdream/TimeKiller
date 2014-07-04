using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity
{

	// Update is called once per frame

    protected override void OnMove()
    {
        Vector2 movePosition = Player.transform.position;
        MoveToWorldPoint(movePosition.x, movePosition.y, MoveSpeed);
		//ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
        GameObjectAnimator.speed = 1.0f;
        GameObjectAnimator.SetBool("Prepare", false);
    }

	protected void OnAttack(Vector2 _attackPosition)
    {
        //base.OnAttack(_attackPosition);
        GameObjectAnimator.SetBool("Prepare", false);
       
    }

    protected override void OnCollision(GameEntity collisionObject)
    {
        //get Player game object
    }

    protected override void OnBlink()
    {
    }
}
