using UnityEngine;

public class GameEntity : MonoBehaviour
{
    public enum Directions
    {
        Bottom, Top, Left, Right
    }

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

    #endregion

    public void MoveToWorldPoint(float x, float y, float speed)
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(x, y, 0), Time.deltaTime * speed);
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
}
