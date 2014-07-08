using UnityEngine;
using System.Collections;

public class ShootScript : MonoBehaviour {
    [SerializeField]
    private float _gunSpeed = 5f;
	public float GunSpeed 
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
        if (renderer.isVisible && _enemyObject.State != GameEntityState.Wait)
            transform.Translate(AttackPosition.x * Time.deltaTime * _gunSpeed, AttackPosition.y * Time.deltaTime * _gunSpeed, 0);
        else
            Destroy(gameObject);
	}
}
