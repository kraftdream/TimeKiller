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

    protected override void OnMove(Vector2 _movePosition)
    {
        MoveToWorldPoint(_movePosition.x, _movePosition.y, MoveSpeed);
        ChangeAnimationPosition(_gameObjectAnimator, _movePosition);
        PrepareTime = 1.0f;
        _gameObjectAnimator.speed = 1.0f;
        _gameObjectAnimator.SetBool("Prepare", false);
    }

    protected override void OnPrepare(Vector2 _attackToPossition)
    {
        _gameObjectAnimator.SetBool("Prepare", true);
        Directions direction = GetDirection(_attackToPossition);
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

    protected override void OnAttack(Vector2 _attackPosition)
    {
        if (_gunTransform != null && CanAttack)
        {
            CanAttack = false;
            var shootObject = Instantiate(_gunTransform) as Transform;
            shootObject.position = transform.position;
            ShootScript script = shootObject.GetComponent<ShootScript>();
            script.AttackPosition = _attackPosition;
            script.EnemyObject = this;
            Directions dir = GetDirection(transform.position);
            if (dir == Directions.Right)
                Flip(script.transform);
            
            _gameObjectAnimator.SetBool("Prepare", false);
        }
    }

    private void Flip(Transform shot)
    {
        Vector3 theScale = shot.localScale;
        theScale.x *= -1;
        shot.localScale = theScale;
    }

    protected override void OnCollision(GameEntity collisionObject)
    {
        //get Player game object
    }

    protected override void OnBlink()
    {
    }
}
