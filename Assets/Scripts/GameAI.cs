using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity
{

    protected override void OnMove()
    {
        Vector2 movePosition = Player.transform.position;
        MoveToWorldPoint(movePosition.x, movePosition.y, MoveSpeed);
		//ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
        GameObjectAnimator.speed = 1.0f;
        GameObjectAnimator.SetBool("Prepare", false);
    }

	protected override void OnAttack()
    {
        base.OnAttack();
        GameObjectAnimator.SetBool("Prepare", false);
        if (Position.Equals(_attackToPosition))
        {
            CanAttack = false;
        }
       
    }

    public override void OnCollision(GameObject collisionObject)
    {
        //get Player game object
    }

    protected override void OnBlink()
    {
    }
}
