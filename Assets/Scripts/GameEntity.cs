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

    private GameEntity _player;
    protected Animator _gameObjectAnimator;
	protected Vector2 _attackToPosition;
    private List<GameObject> _enemyList;
    private Joystick _joyStickLeft;
    private bool _useEditor = false;
    private bool _isPlayer;
    private ActionButton _actionButton;


    #endregion

    protected void Awake()
    {
        #if UNITY_EDITOR
            _useEditor = true;
        #endif

        _gameObjectAnimator = GetComponent<Animator>();
		_player = GameObject.FindGameObjectWithTag ("Player").GetComponent<GameEntity> ();
        _enemyList = GameObject.FindGameObjectWithTag("EnemyCreator").GetComponent<EnemyCreator>().EnemyList;
        _joyStickLeft = GameObject.FindGameObjectWithTag("JoystickTag").GetComponent<Joystick>();
        _actionButton = GameObject.FindGameObjectWithTag("ActionButton").GetComponent<ActionButton>();
        _prepareDefault = _prepareTime;
        _isPlayer = _player.Equals(this);
        if (_isPlayer)
            _actionButton.OnBtnClick += new ActionButton.OnButtonClickListener(OnActionButtonClicked);
    }

    void OnActionButtonClicked(object sender, EventArgs eventArgs) 
    {
        //OnAttack(GetPositionOnDistance(0.2f, GetMoveDirection(Position, new Vector2(_player.transform.position.x, _player.transform.position.y) + GetDirectionAsVector(_player.transform.position))));
    }

    protected void Update()
    {
        _state = GetCurrentState();
        switch (_state)
        {
            case GameEntityState.Move:
                if (_isPlayer)
                {
                    float positionX = _useEditor ? Input.GetAxis("Horizontal") : _joyStickLeft.position.x;
                    float positionY = _useEditor ? Input.GetAxis("Vertical") : _joyStickLeft.position.y;
                    if (_useEditor)
                        OnMove(new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * MoveSpeed, Input.GetAxis("Vertical") * Time.deltaTime * MoveSpeed));
                    else
                        OnMove(new Vector2(_joyStickLeft.position.x * Time.deltaTime * MoveSpeed, _joyStickLeft.position.y * Time.deltaTime * MoveSpeed));
                }
                else
                {
                    OnMove(_player.transform.position);
                }
                break;
            case GameEntityState.Attack:
                OnAttack(_attackToPosition);
                break;
            case GameEntityState.Prepare:
                OnPrepare(_attackToPosition = GetPositionOnDistance(AttackDistance + 2,
                            GetMoveDirection(Position, new Vector2(_player.transform.position.x, _player.transform.position.y))));
                break;

            case GameEntityState.Blink:
                //InvokeRepeating();
                break;
            case GameEntityState.Collision:
                _player.OnCollision(this);
                OnCollision(_player);
                Destroy(this.gameObject);
                _enemyList.Remove(this.gameObject);
                break;
        }        
    }

    private void BlinkAnimation()
    {

    }

    GameEntityState GetCurrentState(Vector2 _position = new Vector2())
    {
        if (!_isPlayer)
        {
            float attackDistance = this.GetComponent<GameAI>().AttackDistance;
            float prepareTiming = this.GetComponent<GameAI>().PrepareTime;
            if (Vector2.Distance(this.transform.position, _player.transform.position) >= attackDistance && prepareTiming == _prepareDefault)
            {
                return GameEntityState.Move;
            }
            else if (prepareTiming > 0)
            {
                return GameEntityState.Prepare;
            }

            if (this.renderer.bounds.Intersects(_player.renderer.bounds))
            {
                if (_health > 1)
                    return GameEntityState.Blink;
                else
                    return GameEntityState.Collision;
            }
            if (prepareTiming <= 0 && !Position.Equals(_attackToPosition))
            {
                return GameEntityState.Attack;
            } 
			else if(Position.Equals(_attackToPosition))
			{
				return GameEntityState.Move;
			}
            return GameEntityState.Move;
        }
        else
        {
            return GameEntityState.Move;
        }
    }

    #region Events

    protected virtual void OnMove(Vector2 destination) { }

    protected virtual void OnPrepare(Vector2 destination) { }

    protected virtual void OnAttack(Vector2 destination) { MoveToWorldPoint(destination.x, destination.y, MoveSpeed * MoveSpeed); }

    protected virtual void OnCollision(GameEntity collisionObject) { }

    protected virtual void OnBlink() { }

    #endregion 

    public void MoveToWorldPoint(float x, float y, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, 0), Time.deltaTime * speed);
    }

    protected void ChangeAnimationPosition(Animator objectAnimator, Vector2 _movePosition)
    {
        objectAnimator.SetBool(Directions.Top.ToString(), false);
        objectAnimator.SetBool(Directions.Bottom.ToString(), false);
        objectAnimator.SetBool(Directions.Right.ToString(), false);
        objectAnimator.SetBool(Directions.Left.ToString(), false);
        objectAnimator.SetBool(GetDirection(_movePosition).ToString(), true);
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

        if (crossProd.z >= 0)
        {
            if (angle <= 90)
                return Directions.Top;
            else if (angle <= 180)
                return Directions.Left;
		}
        else
        {
            if (angle <= 90)
                return Directions.Right;
            else if (angle <= 180)
                return Directions.Bottom;
        }
        
        return Directions.Bottom;
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
