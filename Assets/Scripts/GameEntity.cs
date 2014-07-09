﻿﻿using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{

    #region Input variables

    private const float _prepare_decrement = 0.01f;

    [SerializeField]
    private float _defaultHealth;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _attackSpeed;

    [SerializeField]
    private float _armour;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private int _scorePoint;

    [SerializeField]
    private float _attackDistance;

    [SerializeField]
    private GameEntityState _state;

    [SerializeField]
    private float _prepareTime;

    [SerializeField]
    private GameEntity _player;

    [SerializeField]
    private ParticleSystem _deathBlood;

    private float _prepareDefault;

    public bool IsMoveJoystick { get; set; }

    protected Vector2 _attackToPosition;

    private List<GameObject> _enemyList;

    private Directions _currentDirection;

    public float DefaultHealth
    {
        get { return _defaultHealth; }
        set { _defaultHealth = value; }
    }

    public float Health { get; set; }

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }

    public float AttackSpeed
    {
        get { return _attackSpeed; }
        set { _attackSpeed = value; }
    }

    public float Armour
    {
        get { return _armour; }
        set { _armour = value; }
    }

    public float Damage
    {
        get { return _damage; }
        set { _damage = value; }
    }

    public int ScorePoint
    {
        get { return _scorePoint; }
        set { _scorePoint = value; }
    }

    public Vector2 Position
    {
        get { return new Vector2(transform.position.x, transform.position.y); }
        set { transform.localPosition = value; }
    }

    public float AttackDistance
    {
        get { return _attackDistance; }
        set { _attackDistance = value; }
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
    private bool _canAttack = true;

    public bool CanAttack
    {
        get { return _canAttack; }
        set { _canAttack = value; }
    }

    private bool _collisionDetected;
    public bool CollisionDetected
    {
        get { return _collisionDetected; }
        set { _collisionDetected = value; }
    }

    public Animator GameObjectAnimator { get; set; }
    private Transform _bulletObject;

    public Transform BulletObject
    {
        get { return _bulletObject; }
        set { _bulletObject = value; }
    }


    public GameEntity Player
    {
        get { return _player; }
        set { _player = value; }
    }

    #endregion

    protected void Awake()
    {

        GameObjectAnimator = GetComponent<Animator>();

        Health = DefaultHealth;
        _prepareDefault = _prepareTime;
        _currentDirection = Directions.Bottom;
    }

    protected void Update()
    {
        GameEntityState _checkingState = GetCurrentState();
        if (_checkingState != GameEntityState.Collision)
            _state = _checkingState;

        switch (_checkingState)
        {
            case GameEntityState.Wait:
                OnWait();
                break;

            case GameEntityState.Move:
                OnMove();
                break;
            case GameEntityState.Attack:
                OnAttack();
                break;
            case GameEntityState.Prepare:
                OnPrepare();
                break;

            case GameEntityState.Blink:
                //InvokeRepeating();
                break;

            case GameEntityState.Death:
                OnDeath();
                break;
        }
    }

    GameEntityState GetCurrentState()
    {
        float attackDistance = AttackDistance;
        float prepareTiming = PrepareTime;

        if ((tag.Equals("Player") && !IsMoveJoystick && !CanAttack) || (Player != null && Player.Health <= 0))
            return GameEntityState.Wait;

        if (Health <= 0)
            return GameEntityState.Death;

        if (Player != null && Vector2.Distance(transform.position, Player.transform.position) >= attackDistance && prepareTiming == _prepareDefault)
        {
            return GameEntityState.Move;
        }
        else if (prepareTiming <= _prepareDefault && prepareTiming > 0)
        {
            return GameEntityState.Prepare;
        }
        else if ((prepareTiming <= 0 && _canAttack) || (BulletObject != null && !BulletObject.position.Equals(_attackToPosition)))
        {
            return GameEntityState.Attack;
        }
        else if ((!_canAttack || BulletObject == null) && Player != null)
        {
            _canAttack = true;
            _prepareTime = _prepareDefault;
            return GameEntityState.Move;
        }
        return GameEntityState.Move;
    }

    #region Events

    protected virtual void OnWait()
    {
        GameObjectAnimator.speed = 1;
        SetDefaultAnimation(GameObjectAnimator);
    }

    protected virtual void OnMove()
    {

    }

    protected void OnDeath()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        _deathBlood.active = true;

        if (!GameObjectAnimator.GetBool("Death"))
        {
            SetDefaultAnimation(GameObjectAnimator);
            GameObjectAnimator.SetBool("Death", true);
            GameObjectAnimator.speed = 100;
            Invoke("Disable", 10);
        }
    }

    private void OnEnable()
    {
        Health = DefaultHealth;
    }

    private void Disable()
    {
        gameObject.SetActive(false);
        _deathBlood.active = false;
    }

    protected virtual void OnPrepare()
    {
        if (GameObjectAnimator.speed > 50)
            GameObjectAnimator.speed = 1;

        CanAttack = true;
        _attackToPosition = GetPositionOnDistance(AttackDistance + 2,
            GetMoveDirection(Position, new Vector2(Player.Position.x, Player.Position.y)));

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

    protected virtual void OnAttack()
    {
        GameObjectAnimator.SetBool("Attack", true);
        MoveToWorldPoint(_attackToPosition.x, _attackToPosition.y, AttackSpeed);
    }

    public virtual void OnCollision(GameObject collisionObject) { }

    protected virtual void OnBlink() { }

    #endregion

    public void MoveToWorldPoint(float x, float y, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, 0), Time.deltaTime * speed);
    }

    protected void ChangeAnimationDirection(Animator objectAnimator, Vector2 _movePosition)
    {
        objectAnimator.SetFloat("Direction", (int)GetDirection(_movePosition));
    }

    protected void SetDefaultAnimation(Animator objectAnimator)
    {
        objectAnimator.SetBool("Move", false);
        objectAnimator.SetBool("Prepare", false);
        objectAnimator.SetBool("Attack", false);
        objectAnimator.SetBool("Blink", false);
    }

    public Vector2 GetMoveDirection(Vector2 startPoint, Vector2 endPoint)
    {
        return new Vector2(endPoint.x - startPoint.x, endPoint.y - startPoint.y).normalized;
    }

    public Vector2 GetPositionOnDistance(float distance, Vector2 direction)
    {
        return new Vector2(direction.x * distance + Position.x, direction.y * distance + Position.y);
    }

    public Directions GetDirection(Vector2 point)
    {
        Vector2 nPoint = point.normalized;
        Vector3 crossProd = Vector3.Cross(new Vector3(1, 1), new Vector3(nPoint.x, nPoint.y));
        float angle = Vector2.Angle(new Vector3(1, 1), nPoint);

        if (!point.Equals(Vector2.zero))
        {
            if (crossProd.z >= 0)
            {
                if (angle <= 90)
                {
                    _currentDirection = Directions.Top;
                    return Directions.Top;
                }
                else if (angle <= 180)
                {
                    _currentDirection = Directions.Left;
                    return Directions.Left;
                }
            }
            else
            {
                if (angle <= 90)
                {
                    _currentDirection = Directions.Right;
                    return Directions.Right;
                }
                else if (angle <= 180)
                {
                    _currentDirection = Directions.Bottom;
                    return Directions.Bottom;
                }
            }
        }

        return _currentDirection;
    }
}
