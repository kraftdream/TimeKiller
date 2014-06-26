﻿using UnityEngine;
using System.Collections;

public class MoveScript : MonoBehaviour {
    #region Input Variables
    [SerializeField]
    private float _moveSpeed;
    public float MoveSpeed
    {
        get { return _moveSpeed; }
        set { _moveSpeed = value; }
    }
    #endregion
    // похибка для координати х, щоб персонаж не відразу перевернувся з ліва вправо або навпаки
    private const float _error = 0.8f;
    private Animator _enemyAnimator;

    void Awake()
    {
        _enemyAnimator = GetComponent<Animator>();
    }

    public void FollowTarget(Vector3 _targetPosition) 
    {
        ChangeAnimationPosition(_targetPosition, transform.position);
        transform.position = Vector3.MoveTowards(transform.position, _targetPosition, MoveSpeed * Time.deltaTime);
    }

    void ChangeAnimationPosition(Vector3 _targetPos, Vector3 _currentPos)
    {
        _enemyAnimator.SetBool(Directions.TOP, false);
        _enemyAnimator.SetBool(Directions.BOTTOM, false);
        _enemyAnimator.SetBool(Directions.LEFT, false);
        _enemyAnimator.SetBool(Directions.RIGHT, false);
        if (Mathf.Abs(_targetPos.x - _currentPos.x) > _error)
        {

            if (_targetPos.x <= _currentPos.x)
                _enemyAnimator.SetBool(Directions.LEFT, true);
            else
                _enemyAnimator.SetBool(Directions.RIGHT, true);
        }
        else
        {
            if (_targetPos.y <= _currentPos.y)
                _enemyAnimator.SetBool(Directions.BOTTOM, true);
            else
                _enemyAnimator.SetBool(Directions.TOP, true);
        }
    }

    void MoveToPosition(Vector3 _endPosition)
    {

    }

}