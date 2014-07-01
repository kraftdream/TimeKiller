using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using System.Collections;

public class EventCatcher : MonoBehaviour
{

    #region Input values

    [SerializeField]
    private GameObject _player;

    [SerializeField]
    private GameObject _enemyCreator;

    private List<GameObject> _enemyList;

    #endregion

    // Use this for initialization
	void Start ()
	{
	    _enemyList = _enemyCreator.GetComponent<EnemyCreator>().EnemyList;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    CatchEvents();
	}
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
    private float _attackRadius;
    void CatchEvents()
    {
        if (_enemyList != null)
        {
            foreach (GameObject enemy in _enemyList)
            {
                float health = enemy.GetComponent<GameAI>().Health;
                float armour = enemy.GetComponent<GameAI>().Armour;
                float damege = enemy.GetComponent<GameAI>().Damage;
                float attackDistance = enemy.GetComponent<GameAI>().AttackDistance;
                float prepareTiming = enemy.GetComponent<GameAI>().PrepareTime;
                GameAI.GameEntityState state = enemy.GetComponent<GameAI>().State;

				if (state.Equals(GameAI.GameEntityState.Move) || state.Equals(GameAI.GameEntityState.Prepare))
                {
                    if (Vector2.Distance(enemy.transform.position, _player.transform.position) >= attackDistance 
					    && !state.Equals(GameAI.GameEntityState.Prepare))
                    {
                        enemy.SendMessage("OnMove");
                    }
                    else
                    {
                        enemy.SendMessage("OnPrepare");
                    }
                }

                if (!state.Equals(GameAI.GameEntityState.Move) && prepareTiming <= 0)
                {
                    enemy.SendMessage("OnAttack");
                }
            }
        }
    }
}
