using UnityEngine;
using System.Collections;

public class T1_PatrolState : EnemyStates_SM {

    private readonly Turret1_SM enemy;

    private Transform playerInArea;

    public T1_PatrolState(Turret1_SM statePattern_T1)
    {
        enemy = statePattern_T1;
    }
	
	// Update is called once per frame
	public void UpdateState()
    {
        Patrol();

        // If the player is in radar, check if player is in field of view
        if (playerInArea)
        {
            Look();
        }
        
    }

    #region Trigger Events

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInArea = other.transform;
            Debug.Log("Player found!");
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInArea = null;
            Debug.Log("Player left");
        }
    }

    #endregion

    /// <summary>
    /// State Transitions
    /// </summary>
    #region Transitions

    public void ToPatrolState()
    {
        // Can't transition to same state
    }

    public void ToAttackState()
    {
        enemy.currentState = enemy.attackState;
    }

    public void ToChaseState()
    {
        enemy.currentState = enemy.chaseState;
    }

    public void ToWanderState()
    {
        enemy.currentState = enemy.wanderState;
    }

    #endregion

    #region State Methods

    /// <summary>
    /// Wanders in circles
    /// </summary>
    private void Patrol()
    {
        enemy.barrel.transform.Rotate(new Vector3(0, enemy.turnSpeed * Time.deltaTime, 0));
    }

    /// <summary>
    /// Looks for player
    /// </summary>
    private void Look()
    {
        if (CanSeePlayer())
        {
            enemy.chaseTarget = playerInArea;
            ToChaseState();
        }
    }

    /// <summary>
    /// Checks if player is in enemy's field of view
    /// </summary>
    /// <returns></returns>
    private bool CanSeePlayer()
    {
        Vector3 playerDirection = playerInArea.position - enemy.barrel.transform.position;

        Debug.Log("Angle to player: " + Vector3.Angle(playerDirection, enemy.barrel.transform.forward));

        if (Vector3.Angle(playerDirection, enemy.barrel.transform.forward) < enemy.fieldOfViewRange)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion

}
