using UnityEngine;
using System.Collections;

public class AttackState : IEnemyState

{
    private readonly StateMachineEnemy enemy;

    public AttackState(StateMachineEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Attack();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToAttackState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit hit;
        if (Physics.Raycast(enemy.projectileSpawnPoint.transform.position, enemy.projectileSpawnPoint.transform.forward, out hit, enemy.radar.radius) && hit.collider.CompareTag("Player"))
        {
            enemy.chaseTarget = hit.transform;
            // TODO Attack player
        }
        else
        {
            ToChaseState();
        }
    }

    private void Attack()
    {


    }

}