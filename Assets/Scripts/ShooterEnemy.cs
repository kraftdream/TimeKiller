using UnityEngine;
using System.Collections;

public class ShooterEnemy : GameEntity {

    private const float _prepare_decrement = 0.01f;

    [SerializeField]
    private Transform _gunTransform;

    // Update is called once per frame

    void Update()
    {
        base.Update();
    }

    protected override void OnMove()
    {
        /*MoveToWorldPoint(_movePosition.x, _movePosition.y, MoveSpeed);
        //ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
        _gameObjectAnimator.speed = 1.0f;
        _gameObjectAnimator.SetBool("Prepare", false);*/

        Vector2 movePosition = Player.transform.position;
        MoveToWorldPoint(movePosition.x, movePosition.y, MoveSpeed);
        //ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
        GameObjectAnimator.speed = 1.0f;
        GameObjectAnimator.SetBool("Prepare", false);
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
            Directions dir = GetDirection(transform.position);
            if (dir == Directions.Right)
                Flip(script.transform);

            GameObjectAnimator.SetBool("Prepare", false);
        }
    }

    private void Flip(Transform shot)
    {
        Vector3 theScale = shot.localScale;
        theScale.x *= -1;
        shot.localScale = theScale;
    }

    public override void OnCollision(GameObject collisionObject)
    {
        //get Player game object
    }

    protected override void OnBlink()
    {
    }
}
