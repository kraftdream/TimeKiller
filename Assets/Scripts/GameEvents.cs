using UnityEngine;

public interface GameEvents
{
    void OnMove();

    void OnPrepare();

    void OnAttack();

    void OnCollision(GameObject collisionObject);
}
