using UnityEngine;
using System.Collections;

public class ChaseState : IEnemyState

{

    private readonly StateMachineEnemy enemy;


    public ChaseState(StateMachineEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Chase();
    }

    public void OnTriggerEnter(Collider other)
    {

    }

    public void ToPatrolState()
    {

    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {

    }

    private void Look()
    {
        RaycastHit hit;

        //if ()

    }

    private void Chase()
    {
        Vector3 targetDir = enemy.chaseTarget.position - enemy.head.transform.position;

        enemy.head.transform.eulerAngles = Vector3.Lerp(enemy.head.transform.eulerAngles, targetDir, Time.deltaTime);
    }


}