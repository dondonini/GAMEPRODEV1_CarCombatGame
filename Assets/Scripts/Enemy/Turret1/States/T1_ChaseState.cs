using UnityEngine;
using System.Collections;

public class T1_ChaseState : EnemyStates_SM {

    private readonly Turret1_SM enemy;

    public T1_ChaseState(Turret1_SM statePattern_T1)
    {
        enemy = statePattern_T1;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        Look();

    }

    #region Trigger Events

    public void OnTriggerEnter(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {

    }

    #endregion

    /// <summary>
    /// State Transitions
    /// </summary>
    #region Transitions

    public void ToPatrolState()
    {
        enemy.currentState = enemy.patrolState;
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        // Can't transition to same state
    }

    public void ToWanderState()
    {
        enemy.currentState = enemy.wanderState;
    }

    #endregion

    #region State Methods

    /// <summary>
    /// Looks for player
    /// </summary>
    private void Look()
    {
        if (CanSeePlayer())
        {
            // Turn towards player
            Quaternion lookAtRotation = Quaternion.LookRotation(enemy.chaseTarget.position - enemy.barrel.transform.position);
            enemy.barrel.gameObject.transform.rotation = Quaternion.Slerp(enemy.barrel.gameObject.transform.rotation, lookAtRotation, Time.deltaTime * (enemy.chaseTurnSpeed * Time.deltaTime));

            RaycastHit hit;

            Debug.DrawRay(enemy.projectileSpawnLocation.position, enemy.projectileSpawnLocation.up * enemy.radarArea.radius, Color.cyan);

            //if (Physics.Raycast(enemy.projectileSpawnLocation.position, Vector3.forward, out hit, enemy.radarArea.radius))
            if (Physics.SphereCast(enemy.projectileSpawnLocation.position, 1, enemy.projectileSpawnLocation.up, out hit, enemy.radarArea.radius))
            {
                if (hit.collider.CompareTag("Player"))
                {
                    Debug.DrawRay(enemy.projectileSpawnLocation.position, enemy.projectileSpawnLocation.up * Vector3.Distance(enemy.projectileSpawnLocation.position, hit.point), Color.red);
                    ToAttackState();
                }
            }
        }
        else
        {
            //ToWanderState();
            ToPatrolState();
        }
    }

    /// <summary>
    /// Checks if player is in enemy's field of view
    /// </summary>
    /// <returns></returns>
    private bool CanSeePlayer()
    {
        Vector3 playerPosition = enemy.chaseTarget.position;
        Vector3 playerDirection = playerPosition - enemy.barrel.transform.position;

        if (Vector3.Distance(playerPosition, enemy.barrel.transform.position) > enemy.radarArea.radius)
        {
            return false;
        }
        else
        {
            if (Vector3.Angle(playerDirection, enemy.barrel.transform.forward) < enemy.fieldOfViewRange)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }

    #endregion
}
