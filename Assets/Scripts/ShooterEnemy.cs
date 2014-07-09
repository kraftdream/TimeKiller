using UnityEngine;
using System.Collections;

public class ShooterEnemy : GameEntity {

    private const float _prepare_decrement = 0.01f;

    [SerializeField]
    private Transform _gunTransform;

    public static float Prioroty { get; set; }

    // Update is called once per frame

    void Update()
    {
        base.Update();
    }

    protected override void OnMove()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        Vector2 movePosition = Player.transform.position;
        MoveToWorldPoint(movePosition.x, movePosition.y, MoveSpeed);
        //ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;

        if (!GameObjectAnimator.GetBool("Move"))
        {
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Move", true);
            GameObjectAnimator.speed = 100;
        }

        ChangeAnimationDirection(GameObjectAnimator, movePosition);
    }

    protected override void OnPrepare()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        CanAttack = true;
        _attackToPosition = GetMoveDirection(Position, new Vector2(Player.Position.x, Player.Position.y));

        if (!GameObjectAnimator.GetBool("Prepare"))
        {
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Prepare", true);
            GameObjectAnimator.speed = 100;
        }

        ChangeAnimationDirection(GameObjectAnimator, _attackToPosition);

        PrepareTime -= _prepare_decrement;
        GameObjectAnimator.speed += _prepare_decrement;
    }

    protected override void OnAttack()
    {
        if (_gunTransform != null && CanAttack)
        {
            CanAttack = false;

            BulletObject = Instantiate(_gunTransform) as Transform;
            BulletObject.position = transform.position;
            ShootScript script = BulletObject.GetComponent<ShootScript>();
            script.AttackPosition = _attackToPosition;
            script.EnemyObject = this;
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
