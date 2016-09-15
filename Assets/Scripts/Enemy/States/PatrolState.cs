using UnityEngine;
using System.Collections;

public class PatrolState : IEnemyState

{
    private readonly StateMachineEnemy enemy;
    private float spinX = 0;

    public PatrolState(StateMachineEnemy statePatternEnemy)
    {
        enemy = statePatternEnemy;
    }

    public void UpdateState()
    {
        Look();
        Patrol();
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            ToChaseState();
    }

    public void ToPatrolState()
    {
        Debug.Log("Can't transition to same state");
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    private void Look()
    {
        RaycastHit hit;

        Debug.DrawRay(enemy.projectileSpawnPoint.position, enemy.head.transform.forward + new Vector3(0,0,10),Color.red);

        // Checks if target is in radar area
        if (Physics.SphereCast(enemy.projectileSpawnPoint.position, enemy.radar.radius, enemy.head.transform.forward, out hit, enemy.radar.radius) && hit.collider.CompareTag("Player"))
        {
            Vector3 targetDir = hit.collider.transform.position - enemy.head.transform.position;

            Debug.Log(Vector3.Angle(targetDir, enemy.head.transform.forward));
            // Checks if target is in field of view
            if (Vector3.Angle(targetDir, enemy.head.transform.forward) < enemy.fieldOfView)
            {
                enemy.chaseTarget = hit.collider.transform;
                ToChaseState();
            }
        }
    }

    void Patrol()
    {
        Transform headTransform = enemy.head.transform;

        headTransform.eulerAngles = Vector3.Slerp(headTransform.eulerAngles, new Vector3(0, headTransform.eulerAngles.y + enemy.searchingTurnSpeed, 0), Time.deltaTime);

    }
}