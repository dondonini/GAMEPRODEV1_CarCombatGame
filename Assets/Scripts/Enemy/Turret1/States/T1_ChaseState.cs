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
            RaycastHit hit;

            if (Physics.Raycast(enemy.projectileSpawnLocation.transform.position, Vector3.up, out hit, enemy.radarArea.radius)) 
            {
                if (hit.collider.CompareTag("Player"))
                {
                    ToAttackState();
                }
            }
        }
        else
        {
            ToWanderState();
        }
    }

    /// <summary>
    /// Checks if player is in enemy's field of view
    /// </summary>
    /// <returns></returns>
    private bool CanSeePlayer()
    {
        Vector3 playerPosition = enemy.chaseTarget.transform.position;
        Vector3 playerDirection = playerPosition - enemy.barrel.transform.position;

        if (Vector3.Distance(playerPosition, enemy.barrel.transform.position) > enemy.radarArea.radius)
        {
            return false;
        }
        else
        {
            if (Vector3.Angle(playerDirection, enemy.barrel.transform.position) < enemy.fieldOfViewRange)
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
