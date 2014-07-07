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
                if (enemy == null)
                    continue;
                GameEntity enemyScript = enemy.GetComponent<GameEntity>();
                GameObject collisionObject = null;
                // check if the collision is enemy or a bullet
                if (enemyScript.BulletObject != null)
                {
                    collisionObject = enemyScript.BulletObject.gameObject;
                }
                else
                {
                    collisionObject = enemy;
                }

                if (IsIntersects(collisionObject.renderer.bounds))
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
