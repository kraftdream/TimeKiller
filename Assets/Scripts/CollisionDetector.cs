﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class CollisionDetector : MonoBehaviour {

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
	void Start () {
	}

	void Update () {
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
                if (enemyScript is ShooterEnemy && !((ShooterEnemy)enemyScript).CanAttack && ((ShooterEnemy)enemyScript).BulletObject != null)
                    collisionObject = ((ShooterEnemy)enemyScript).BulletObject.gameObject;
                else
                    collisionObject = enemy;
                if (IsIntersects(collisionObject.renderer.bounds))
                {
                    _player.GetComponent<GameEntity>().OnCollision(enemy);
                }
            }
        }
        EnemyCreator.EnemyList.RemoveAll(item => item == null);

    }

    bool IsIntersects(Bounds _enemyBounds)
    {
        return Player.renderer.bounds.Intersects(_enemyBounds);
    } 
}