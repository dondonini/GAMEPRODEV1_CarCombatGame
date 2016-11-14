using UnityEngine;
using System.Collections;

public class Turret1_SM : MonoBehaviour
{

    public SphereCollider radarArea;

    [Range(0.0f, 360.0f)]
    public float fieldOfViewRange = 45.0f;
    public float turnSpeed = 10.0f;
    public float projectileFireSpeed = 100.0f;
    public float wanderWaitMin = 0.5f;
    public float wanderWaitMax = 1.5f;
    public float fireWarningDuration = 1.0f;
    [Tooltip("Set percentage of the fire warning time will be double time (A.K.A. Will be flashing faster).")]
    public float fireWarningDoubleTimePercentage = 0.9f;


    public GameObject bulb;
    public GameObject barrel;
    public GameObject projectile;
    public Transform projectileSpawnLocation;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public EnemyStates_SM currentState;
    [HideInInspector]
    public T1_ChaseState chaseState;
    [HideInInspector]
    public T1_AttackState attackState;
    [HideInInspector]
    public T1_PatrolState patrolState;
    [HideInInspector]
    public T1_WanderState wanderState;

    void Awake()
    {
        chaseState = new T1_ChaseState(this);
        attackState = new T1_AttackState(this);
        patrolState = new T1_PatrolState(this);
        wanderState = new T1_WanderState(this);
    }

    // Use this for initialization
    void Start()
    {
        currentState = patrolState;
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState();
        //Debug.Log(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }

    private void OnTriggerExit(Collider other)
    {
        currentState.OnTriggerExit(other);
    }
}
