using UnityEngine;

public interface GameEvents
{
    void OnMove(Vector2 destination);

    void OnPrepare();

    void OnAttack(Vector2 destination);

    void OnCollision(GameObject collisionObject);

    void OnBlink();
}
