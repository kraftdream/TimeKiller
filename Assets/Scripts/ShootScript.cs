using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour {
	[SerializeField]
	private Vector2 _gunSpeed = new Vector2(10, 10);

	public Vector2 GunSpeed 
	{
		set { _gunSpeed = value; }
		get { return _gunSpeed; }
	}

    public Vector2 AttackPosition;

    private ShooterEnemy _enemyObject;
    public ShooterEnemy EnemyObject
    {
        set { _enemyObject = value; }
    }

	void Update () {
        if (renderer.isVisible)
            transform.position = Vector2.MoveTowards(transform.position, new Vector2(AttackPosition.x * _gunSpeed.x, AttackPosition.y * _gunSpeed.y), 0.1f);
        else
        {
            _enemyObject.CanAttack = true;
            Destroy(gameObject);
        }
	}
}
