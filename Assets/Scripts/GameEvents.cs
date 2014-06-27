using UnityEngine;
using System.Collections;

public interface GameEvents
{

    void OnMove(GameObject gameObject);

    void OnPrepare(GameObject gameObject);

    void OnAttack(GameObject gameObject);

    void OnDeath(GameObject gameObject);
}
