using System;
using System.Linq;
using System.Linq.Expressions;
using UnityEngine;
using System.Collections.Generic;
using Random = UnityEngine.Random;
using System.Runtime.CompilerServices;

public class EnemyCreator : MonoBehaviour
{
    #region Input values

    [SerializeField]
    private float _time = 1f;

    [SerializeField]
    private float _delay = 1f;

    [SerializeField]
    private GameObject[] _enemy;

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private float _distanceToPlayer;

    private int _gameAIPercentage = 60;
    private int _shootEnemyPercentage = 40;

    private Camera _mainCamera;
    private Vector3 _cameraSize;
    private Vector3 _enemySize;

    private volatile List<GameObject> _enemyList;

    private float _maxVisibleCount = 10f;

    private bool _isCancel = false;

    public List<GameObject> EnemyList
    {
        [MethodImpl(MethodImplOptions.Synchronized)]
        get { return _enemyList; }
    }

    public float Time
    {
        get { return _time; }
        set { _time = value; }
    }

    public float Delay
    {
        get { return _delay; }
        set { _delay = value; }
    }

    public float DistanceToPlayer
    {
        get { return _distanceToPlayer; }
        set { _distanceToPlayer = value; }
    }

    #endregion

    private void Awake()
    {
        _enemyList = new List<GameObject>();
        _mainCamera = Camera.main;
        _cameraSize = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        _enemySize = _enemy[0].renderer.bounds.size;
    }

    private void Start()
    {
        InvokeRepeating("CreateEnemy", Delay, Time);
        _isCancel = false;
    }

    void Update()
    {
        if (_player.GetComponent<GameEntity>().Health <= 0 && !_isCancel)
        {
            _isCancel = true;
            CancelInvoke("CreateEnemy");
        }
        if (_player.GetComponent<GameEntity>().Health > 0 && _isCancel)
        {
            Start();
        }
    }

    private void CreateEnemy()
    {
        if (EnemyList.Count(gameObj => gameObj.activeInHierarchy && gameObj.GetComponent<GameEntity>().State != GameEntityState.Death) < _maxVisibleCount)
            CreateEnemyByPriority();
    }

    private void CreateEnemyByPriority()
    {
        int createdGameAICount = EnemyList.Count(gameObj => gameObj.GetComponent<GameEntity>() is GameAI && gameObj.activeInHierarchy && gameObj.GetComponent<GameEntity>().State != GameEntityState.Death);
        GameAI.Prioroty = GetPercentage(createdGameAICount, _gameAIPercentage);

        int createdShootEnemyCount = EnemyList.Count(gameObj => gameObj.GetComponent<GameEntity>() is ShooterEnemy && gameObj.activeInHierarchy && gameObj.GetComponent<GameEntity>().State != GameEntityState.Death);
        ShooterEnemy.Prioroty = GetPercentage(createdShootEnemyCount, _shootEnemyPercentage);

        if (ShooterEnemy.Prioroty > GameAI.Prioroty && createdGameAICount > 3)
            CreateNewEnemy<ShooterEnemy>();
        else
            CreateNewEnemy<GameAI>();
    }

    private void CreateNewEnemy<T>() where T : GameEntity
    {
        List<GameObject> enemys = EnemyList.FindAll(gameObj => gameObj.GetComponent<GameEntity>() is T && !gameObj.activeInHierarchy);
        if (enemys.Count > 0)
        {
            enemys[0].transform.position = GetRandomPosition();
            enemys[0].transform.rotation = transform.rotation;
            enemys[0].GetComponent<GameEntity>().Player = _player.GetComponent<GameEntity>();
            enemys[0].SetActive(true);
        }
        else
        {
            GameObject enemy = (GameObject)Instantiate(_enemy[Type.Equals(typeof(T), typeof(ShooterEnemy)) ? 0 : 1]);
            enemy.transform.position = GetRandomPosition();
            enemy.transform.parent = transform;
            enemy.transform.rotation = transform.rotation;
			enemy.SetActive(true);
            enemy.GetComponent<GameEntity>().Player = _player.GetComponent<GameEntity>();
            enemy.transform.parent = gameObject.transform;
            EnemyList.Add(enemy);
        }

    }

    private float GetPercentage(int createdEnemyCount, int enemyPercentage)
    {
        return 100 - ((createdEnemyCount * 100) / (_maxVisibleCount * enemyPercentage / 100));
    }

    private Vector3 GetRandomPosition()
    {
        try
        {
            float x = Random.Range(_enemySize.x / 2 - _cameraSize.x, _cameraSize.x - (_enemySize.x / 2));
            float y = Random.Range(_enemySize.y / 2 - _cameraSize.y, _cameraSize.y - (_enemySize.y / 2));

            if (IsIntersects(new Bounds(new Vector3(x, y, 0), new Vector3(_enemySize.x, _enemySize.y, _enemySize.z))))
                return GetRandomPosition();

            if (Vector3.Distance(new Vector3(x, y, 0), _player.transform.position) < DistanceToPlayer)
                return GetRandomPosition();

            return new Vector3(x, y, 0);
        }
        catch (StackOverflowException)
        {
            return new Vector3(0, 0, 0);
        }
    }

    private bool IsIntersects(Bounds newEnemyBounds)
    {
        return (from enemy in EnemyList
                where enemy != null && enemy.renderer.bounds.Intersects(newEnemyBounds)
                select enemy).ToList<GameObject>().Count > 0;
        //return EnemyList.FindAll(enemy => enemy != null).Exists(enemy => enemy.renderer.bounds.Intersects(newEnemyBounds));
    }
}
