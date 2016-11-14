using UnityEngine;
using System.Collections;

public class T1_WanderState : EnemyStates_SM {

    private readonly Turret1_SM enemy;

    private float rotationDuration = 0;
    private float rotationProgress = 0;

    private Vector3 startRot;
    private Vector3 endRot;

    public T1_WanderState(Turret1_SM statePattern_T1)
    {
        enemy = statePattern_T1;
        SetRandomWait();
    }

    // Update is called once per frame
    public void UpdateState()
    {
        Look();
        UpdateLookAt();

        if (rotationProgress >= rotationDuration)
        {
            LookAt();
            SetRandomWait();
        }
        else
        {
            rotationProgress += Time.deltaTime;
        }

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
        enemy.currentState = enemy.chaseState;
    }

    public void ToWanderState()
    {
        // Can't transition to same state
    }

    #endregion

    #region State Methods

    /// <summary>
    /// Wanders in circles
    /// </summary>
    private void Wander()
    {
        enemy.barrel.transform.Rotate(new Vector3(0, enemy.turnSpeed, 0));
    }

    /// <summary>
    /// Looks for player
    /// </summary>
    private void Look()
    {
        if (CanSeePlayer())
        {
            ToChaseState();
        }
    }

    private void LookAt()
    {
        startRot = enemy.barrel.transform.rotation.eulerAngles;

        endRot = new Vector3(0, Random.Range(10, 360), 0);
    }

    private void UpdateLookAt()
    {
        float progress = Mathf.Lerp(0.0f, 1.0f, rotationProgress / rotationDuration);
        Quaternion newRot = Quaternion.Slerp(Quaternion.Euler(startRot), Quaternion.Euler(endRot), EasingFunction.EaseInOutQuad(0.0f, 1.0f, progress));

        // Ease rotation
        enemy.barrel.transform.rotation = newRot;
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

    public void SetRandomWait()
    {
        // Set random time duration
        rotationDuration = Random.Range(enemy.wanderWaitMin, enemy.wanderWaitMax);

        // Set timer to duration
        rotationProgress = rotationDuration;
    }

    #endregion
}
