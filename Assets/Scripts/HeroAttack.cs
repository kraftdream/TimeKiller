using UnityEngine;
using System.Collections;

public class HeroAttack : MonoBehaviour
{
    private const float _distance = 5.0f;
    private const float _speed = 10.0f;
    private const float _time = 0.4f;

    private float passedTime;

    private Vector3 _currentPos;
    private Vector3 _prevPos;
    private Vector3 _direction;
    private Vector3 _heading;

    private float prevX;
    private float prevY;

    public bool IsAttacked { get; set; }

    void Start()
    {
        _currentPos = Vector3.right;
        _prevPos = Vector3.zero;
    }

    void Update()
    {
        _currentPos = transform.position;

        if (Vector3.Distance(_currentPos, _prevPos) != 0)
        {
            _heading = _currentPos - _prevPos;
            _direction = _heading.normalized;
        }

        if (IsAttacked)
        {
            if (passedTime < _time)
            {
                passedTime += Time.deltaTime;
                transform.Translate(_direction * Time.deltaTime * _speed, Space.Self);
            }
            else
            {
                IsAttacked = false;
                passedTime = 0;
            }
        }

        _prevPos.x = _currentPos.x;
        _prevPos.y = _currentPos.y;
    }
}
