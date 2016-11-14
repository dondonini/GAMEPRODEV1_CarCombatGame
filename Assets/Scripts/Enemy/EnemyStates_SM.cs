using UnityEngine;
using System.Collections;

public interface EnemyStates_SM
{

    void UpdateState();

    void OnTriggerEnter(Collider other);

    void OnTriggerExit(Collider other);

    void ToPatrolState();

    void ToAttackState();

    void ToChaseState();

    void ToWanderState();
}
