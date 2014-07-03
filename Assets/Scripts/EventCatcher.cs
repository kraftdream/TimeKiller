using System.Collections.Generic;
using System.Runtime.InteropServices;
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
    private List<GameObject> _enemiesToDelete;

    #endregion

    // Use this for initialization
	void Start ()
	{
	    _enemyList = _enemyCreator.GetComponent<EnemyCreator>().EnemyList;
        _enemiesToDelete = new List<GameObject>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    //CatchEvents();
	}

    void CatchEvents()
    {
        _enemiesToDelete.Clear();

        if (_enemyList != null)
        {
            foreach (GameObject enemy in _enemyList)
            {
                float attackDistance = enemy.GetComponent<GameAI>().AttackDistance;
                float prepareTiming = enemy.GetComponent<GameAI>().PrepareTime;
                GameEntityState state = enemy.GetComponent<GameAI>().State;

		        if (state.Equals(GameEntityState.Move) || state.Equals(GameEntityState.Prepare))
                {
                    if (Vector2.Distance(enemy.transform.position, _player.transform.position) >= attackDistance 
					    && !state.Equals(GameEntityState.Prepare))
		            {
                    	enemy.SendMessage("OnMove");
                    }
                    else
                    {
                    	enemy.SendMessage("OnPrepare");
                    }
		        }
		
                if (!state.Equals(GameEntityState.Move) && prepareTiming <= 0)
                {
                    enemy.SendMessage("OnAttack");
                }

                if (enemy.renderer.bounds.Intersects(_player.renderer.bounds))
                {
                    _player.SendMessage("OnCollision", enemy);
                    enemy.SendMessage("OnCollision", _player);
                    //if (_player.GetComponent<GameAI>().State.Equals(GameAI.GameEntityState.Attack))
                        _enemiesToDelete.Add(enemy);
                }

            }

            _enemiesToDelete.ForEach(Destroy);
            _enemiesToDelete.ForEach(gObj => _enemyList.Remove(gObj));

        }
    }
}
