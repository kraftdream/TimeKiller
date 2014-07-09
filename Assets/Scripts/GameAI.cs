﻿using System.Collections.Generic;
using UnityEngine;

public class GameAI : GameEntity
{
    public static float Prioroty { get; set; }
    protected override void OnMove()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        Vector2 movePosition = Player.transform.position;
        MoveToWorldPoint(movePosition.x, movePosition.y, MoveSpeed);
        PrepareTime = 1.0f;

        if (!GameObjectAnimator.GetBool("Move"))
        {
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Move", true);
            GameObjectAnimator.speed = 100;
        }

        ChangeAnimationDirection(GameObjectAnimator, movePosition);
    }

    protected override void OnAttack()
    {
        MoveToWorldPoint(_attackToPosition.x, _attackToPosition.y, AttackSpeed);

        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        if (!GameObjectAnimator.GetBool("Attack"))
        {
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Attack", true);
            GameObjectAnimator.speed = 100;
        }

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
