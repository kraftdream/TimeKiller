using UnityEngine;
using Random = UnityEngine.Random;

public class MainMenuBatmanScript : MonoBehaviour
{
    public Transform _batman;

    private Vector3 _direction = Vector3.right;
    private bool _newDirection = false;
    private float _currentSpeed;
    private float _move;

    void Start()
    {
        _currentSpeed = Random.Range(0.5f, 1.5f);
    }

    void Update()
    {
        transform.Translate(_direction * Time.deltaTime * _currentSpeed, Space.World);
    }

    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
        _direction = transform.InverseTransformDirection(Vector3.left);
    }

    void SetRandomDirection()
    {
        Vector2 randDirect = Random.insideUnitCircle * 5;
        _direction = new Vector3(randDirect.normalized.x, randDirect.normalized.y, 0);
    }
}
