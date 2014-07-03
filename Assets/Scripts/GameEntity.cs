using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameEntity : MonoBehaviour
{

    #region Input variables

    [SerializeField]
    private float _health;

    [SerializeField]
    private float _moveSpeed;

    [SerializeField]
    private float _armour;

    [SerializeField]
    private float _damage;

    [SerializeField]
    private float _scorePoint;

    [SerializeField]
    private float _attackDistance;

    [SerializeField]
    private GameEntityState _state;

    [SerializeField]
    private float _prepareTime;

    private float _prepareDefault;

    private Directions _currentDirection;

    public float Health
    {
        get { return _health; }
        set { _health = value; }
    }

    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
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

    public float ScorePoint
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

    protected Animator _gameObjectAnimator;
	protected Vector2 _attackToPosition;
    private List<GameObject> _enemyList;

    [SerializeField]
    private Joystick _joyStickLeft;
    protected bool _useEditor = false;

    public Joystick JoyStickLeft
    {
        get { return _joyStickLeft; }
        set { _joyStickLeft = value; }
    }

    public GameEntity Player { get; set; }

    #endregion

    protected void Awake()
    {
        #if UNITY_EDITOR
            _useEditor = true;
        #endif

        _gameObjectAnimator = GetComponent<Animator>();
		Player = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameEntity> ();
        _enemyList = GameObject.FindGameObjectWithTag("EnemyCreator").GetComponent<EnemyCreator>().EnemyList;

        _prepareDefault = _prepareTime;
        _currentDirection = Directions.Bottom;
    }

    protected void Update()
    {
        GameEntityState _checkingState = GetCurrentState();
        if (_checkingState != GameEntityState.Collision)
            _state = _checkingState;
        if(!_isPlayer)
            Debug.Log("State = " + _checkingState);
        switch (_checkingState)
        {
            case GameEntityState.Wait:
                OnWait();
                break;

            case GameEntityState.Move:
                OnMove();
                break;
            case GameEntityState.Attack:
                OnAttack(_attackToPosition);
                break;
            case GameEntityState.Prepare:
                OnPrepare(_attackToPosition = GetPositionOnDistance(AttackDistance + 2,
                            GetMoveDirection(Position, new Vector2(Player.transform.position.x, Player.transform.position.y))));
                break;

            case GameEntityState.Blink:
                //InvokeRepeating();
                break;
            case GameEntityState.Collision:
                Player.OnCollision(this);
                OnCollision(Player);
                Destroy(gameObject);
                _enemyList.Remove(gameObject);
                break;
        }
    }

    private void BlinkAnimation()
    {

    }

    GameEntityState GetCurrentState(Vector2 _position = new Vector2())
    {
        float attackDistance = AttackDistance;
        float prepareTiming = PrepareTime;

        float positionX = _useEditor ? Input.GetAxis("Horizontal") : _joyStickLeft.position.x;
        float positionY = _useEditor ? Input.GetAxis("Vertical") : _joyStickLeft.position.y;

        if (new Vector2(positionX, positionY).Equals(Vector2.zero))
            return GameEntityState.Wait;

        if (Vector2.Distance(this.transform.position, Player.transform.position) >= attackDistance && prepareTiming == _prepareDefault)
        {
            return GameEntityState.Move;
        }
        else if (prepareTiming > 0)
        {
            return GameEntityState.Prepare;
        }

        if (!tag.Equals("Player") && renderer.bounds.Intersects(Player.renderer.bounds))
        {
            if (_health > 1)
                return GameEntityState.Blink;
            else
                return GameEntityState.Collision;
        }
            if (prepareTiming <= 0 && !Position.Equals(_attackToPosition) && _canAttack)
        {
            return GameEntityState.Attack;
            }
            else if (Position.Equals(_attackToPosition) || !_canAttack)
		{
                _prepareTime = _prepareDefault;
			return GameEntityState.Move;
		}

        return GameEntityState.Move;
    }
            /*List<GameObject> intersected = _enemyList.FindAll(gObj => renderer.bounds.Intersects(gObj.renderer.bounds));
            if(intersected.Count > 0)
                Debug.Log("Intersected");*/

    #region Events

    protected virtual void OnWait()
    {
        SetDefaultAnimation(_gameObjectAnimator);
    }

    protected virtual void OnMove()
    {
    }

    protected virtual void OnPrepare(Vector2 destination)
    {
        _gameObjectAnimator.SetBool("Prepare", true);
    }

    protected virtual void OnAttack(Vector2 destination)
    {
        MoveToWorldPoint(destination.x, destination.y, MoveSpeed * MoveSpeed);
        _gameObjectAnimator.SetBool("Attack", true);
    }

    protected virtual void OnCollision(GameEntity collisionObject) { }

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

    public Rect GetEntityRectSize()
    {
        Vector3 size = transform.renderer.bounds.size;
        Vector3 center = transform.renderer.bounds.center;
        return new Rect(center.x, center.y, size.x, size.y);
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

    public Vector2 GetDirectionAsVector(Vector2 point)
    {
        Vector2 nPoint = point.normalized;
        Vector3 crossProd = Vector3.Cross(new Vector3(1, 1), new Vector3(nPoint.x, nPoint.y));
        float angle = Vector2.Angle(new Vector3(1, 1), nPoint);

        if (crossProd.z >= 0)
        {
            if (angle <= 90)
                return new Vector2(0f, 0.1f);
            else if (angle <= 180)
                return new Vector2(-0.1f, 0f);
        }
        else
        {
            if (angle <= 90)
                return new Vector2(0.1f, 0.0f);
            else if (angle <= 180)
                return new Vector2(0f, -0.1f);
        }

        return new Vector2(0f, -0.1f);
    }
}
