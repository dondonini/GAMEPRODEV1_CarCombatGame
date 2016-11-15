using UnityEngine;
using System.Collections;

public class T1_AttackState : EnemyStates_SM
{

    private readonly Turret1_SM enemy;

    private bool isAttacking = false;
    private float attackCounter = 0;

    private float blinkCounter = 0;
    private float localBlinkSpeed;
    private bool blinkToggle = false;

    public T1_AttackState(Turret1_SM statePattern_T1)
    {
        enemy = statePattern_T1;
        isAttacking = true;
        localBlinkSpeed = enemy.fireWarningBlinkSpeed;
    }

    // Update is called once per frame
    public void UpdateState()
    {
        if (isAttacking)
        {
            Attack();
        }
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
        enemy.currentState = enemy.chaseState;
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
            Quaternion lookAtRotation = Quaternion.LookRotation(enemy.chaseTarget.position - enemy.barrel.transform.position);
            enemy.barrel.gameObject.transform.rotation = Quaternion.Slerp(enemy.barrel.gameObject.transform.rotation, lookAtRotation, Time.deltaTime * (enemy.chaseTurnSpeed * Time.deltaTime));

            if (!isAttacking)
            {
                RaycastHit hit;
                Debug.DrawRay(enemy.projectileSpawnLocation.position, enemy.projectileSpawnLocation.up * enemy.radarArea.radius, Color.cyan);

                //if (Physics.Raycast(enemy.projectileSpawnLocation.position, Vector3.forward, out hit, enemy.radarArea.radius))
                if (Physics.SphereCast(enemy.projectileSpawnLocation.position, 1, enemy.projectileSpawnLocation.up, out hit, enemy.radarArea.radius))
                {
                    if (hit.collider.CompareTag("Player"))
                    {
                        Debug.DrawRay(enemy.projectileSpawnLocation.position, enemy.projectileSpawnLocation.up * Vector3.Distance(enemy.projectileSpawnLocation.position, hit.point), Color.red);
                        attackCounter = 0.0f;
                        blinkCounter = 0.0f;
                        localBlinkSpeed = enemy.fireWarningBlinkSpeed;
                        isAttacking = true;
                        return;
                    }
                }
            }
            else
            {
                if (!isAttacking)
                {
                    ToChaseState();
                }
            }
        }
        else
        {
            //ToWanderState();
            if (!isAttacking)
            {
                ToChaseState();
            }
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

    private void Attack()
    {
        Renderer turretRenderer = enemy.bulb.GetComponent<Renderer>();

        Debug.DrawRay(enemy.projectileSpawnLocation.position, enemy.projectileSpawnLocation.up * enemy.radarArea.radius, Color.red);

        // Toggles blinktoggle
        if (blinkCounter >= localBlinkSpeed)
        {
            blinkToggle = !blinkToggle;
        }

        // Blink bulb colour
        if (blinkToggle)
        {
            turretRenderer.material = enemy.warningWhite;
        }
        else
        {
            turretRenderer.material = enemy.warningBlack;
        }

        // Double the blink speed when threshold has been passed
        if (attackCounter >= enemy.fireWarningDuration * enemy.fireWarningDoubleTimePercentage && localBlinkSpeed == enemy.fireWarningBlinkSpeed)
        {
            localBlinkSpeed = localBlinkSpeed * 0.2f;
            Debug.Log(localBlinkSpeed);
        }

        if (attackCounter >= enemy.fireWarningDuration)
        {
            Fire();
            isAttacking = false;
        }

        // Increase counters
        attackCounter += Time.deltaTime;
        blinkCounter += Time.deltaTime;
    }

    private void Fire()
    {
        GameObject newProjectile = MonoBehaviour.Instantiate(enemy.projectile) as GameObject;

        newProjectile.transform.position = enemy.projectileSpawnLocation.position + (enemy.projectileSpawnLocation.up * enemy.projectileFireOffset);
        newProjectile.transform.rotation = enemy.projectileSpawnLocation.rotation;

        newProjectile.GetComponent<ProjectileFunction>().BuildCollisionIgnoreList(enemy.transform);

        newProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, enemy.projectileFireSpeed, 0));
    }

    #endregion
}
