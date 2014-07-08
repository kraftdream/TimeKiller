using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollisionDetector : MonoBehaviour
{

    [SerializeField]
    private GameObject _enemyCreator;

    [SerializeField]
    private GameObject _player;

    public EnemyCreator EnemyCreator
    {
        get { return _enemyCreator.GetComponent<EnemyCreator>(); }
    }

    public GameObject Player
    {
        get { return _player; }
    }

    // Use this for initialization
    void Start()
    {
    }

    void Update()
    {
        CheckForCollision();
    }

    void CheckForCollision()
    {
        if (EnemyCreator.EnemyList != null)
        {
            foreach (GameObject enemy in EnemyCreator.EnemyList)
            {
                if (enemy == null || enemy.GetComponent<GameEntity>().State.Equals(GameEntityState.Death))
                    continue;

                GameEntity enemyScript = enemy.GetComponent<GameEntity>();

                if ((enemyScript is ShooterEnemy && enemyScript.BulletObject != null && IsIntersects(enemyScript.BulletObject.gameObject.renderer.bounds))  ||
                    IsIntersects(enemy.renderer.bounds))
                {
                    if (!enemyScript.CollisionDetected)
                    {
                        enemyScript.CollisionDetected = true;
                        _player.GetComponent<GameEntity>().OnCollision(enemy);
                    }
                }
                else
                    enemyScript.CollisionDetected = false;
            }
        }
        EnemyCreator.EnemyList.RemoveAll(item => item == null);
    }

    bool IsIntersects(Bounds _enemyBounds)
    {
        return Player.renderer.bounds.Intersects(_enemyBounds);
    }
}
