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
        Collider[] hitColliders = Physics.OverlapSphere(enemy.head.transform.position, enemy.radar.radius);
        Debug.DrawRay(enemy.projectileSpawnPoint.position, enemy.head.transform.forward * enemy.radar.radius, Color.red);
        Debug.DrawRay(enemy.projectileSpawnPoint.position, (enemy.head.transform.forward * enemy.radar.radius) + (enemy.head.transform.right * (Mathf.Tan(enemy.fieldOfView * Mathf.Deg2Rad * 0.5f) * enemy.radar.radius)), Color.blue);
        Debug.DrawRay(enemy.projectileSpawnPoint.position, (enemy.head.transform.forward * enemy.radar.radius) + (-enemy.head.transform.right * (Mathf.Tan(enemy.fieldOfView * Mathf.Deg2Rad * 0.5f) * enemy.radar.radius)), Color.blue);

        // Checks if target is in radar area
        //if (Physics.SphereCast(enemy.projectileSpawnPoint.position, enemy.radar.radius, enemy.head.transform.forward, out hit, enemy.radar.radius) && hit.collider.CompareTag("Player"))

        Collider target = FindPlayer(hitColliders);

        if (target != null)
        {
            Vector3 targetDir = target.transform.position - enemy.head.transform.position;

            Debug.Log("Player in area");

            Debug.Log(Vector3.Angle(targetDir, enemy.head.transform.forward));
            // Checks if target is in field of view
            if (Vector3.Angle(targetDir, enemy.head.transform.forward) < enemy.fieldOfView)
            {
                enemy.chaseTarget = target.transform;
                ToChaseState();
            }
        }
    }

    Collider FindPlayer(Collider[] colliders)
    {
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].CompareTag("Player"))
            {
                return colliders[i];
            }
        }

        return null;
    }

    void Patrol()
    {
        Transform headTransform = enemy.head.transform;

        headTransform.eulerAngles = Vector3.Slerp(headTransform.eulerAngles, new Vector3(0, headTransform.eulerAngles.y + enemy.searchingTurnSpeed, 0), Time.deltaTime);

    }
}