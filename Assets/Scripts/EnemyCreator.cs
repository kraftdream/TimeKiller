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

    [SerializeField] private float _time = 1f;

    [SerializeField] private float _delay = 1f;

    [SerializeField] private GameObject[] _enemy;

    [SerializeField] private int _maxEnemyCount;

    [SerializeField] private GameObject _player;

    [SerializeField]
    private float _distanceToPlayer;

    private Camera _mainCamera;
    private Vector3 _cameraSize;
    private Vector3 _enemySize;

    private volatile List<GameObject> _enemyList;

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

    public int MaxEnemyCount
    {
        get { return _maxEnemyCount; }
        set { _maxEnemyCount = value; }
    }

    public float DistanceToPlayer
    {
        get { return _distanceToPlayer; }
        set { _distanceToPlayer = value; }
    }

    #endregion

    private void Awake()
    {
        _enemyList = new List<GameObject>(MaxEnemyCount);
        _mainCamera = Camera.main;
        _cameraSize = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height));
        _enemySize = _enemy[0].renderer.bounds.size;
    }

    private void Start()
    {
        for (int i = 0; i < MaxEnemyCount; i++)
        {
            GameObject enemy = (GameObject) Instantiate(_enemy[Random.Range(0, _enemy.Length)]);
            enemy.transform.parent = transform;
            enemy.SetActive(false);
            EnemyList.Add(enemy);
        }
        InvokeRepeating("CreateEnemy", Delay, Time); 
    }

    private void CreateEnemy()
    {
        for (int i = 0; i < EnemyList.Count; i++)
        {
            if (!EnemyList[i].activeInHierarchy)
            {
                EnemyList[i].transform.position = GetRandomPosition();
                EnemyList[i].transform.rotation = transform.rotation;
                EnemyList[i].GetComponent<GameEntity>().Player = _player.GetComponent<GameEntity>();
                EnemyList[i].SetActive(true); 
                return;
            }
        }
    }

    private Vector3 GetRandomPosition()
    {
        try
        {
            float x = Random.Range(_enemySize.x/2 - _cameraSize.x, _cameraSize.x - (_enemySize.x/2));
            float y = Random.Range(_enemySize.y/2 - _cameraSize.y, _cameraSize.y - (_enemySize.y/2));

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
