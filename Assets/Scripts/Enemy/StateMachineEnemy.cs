using UnityEngine;
using System.Collections;

public class StateMachineEnemy : MonoBehaviour {

    public SphereCollider radar;

    [Range(0f, 360f)]
    public float fieldOfView = 45f;

    [Range(0f, 360f)]
    public float attackZone = 10f;

    public float searchingTurnSpeed = 100f;

    public float launchSpeed = 10;

    public GameObject head;
    public GameObject projectile;
    public Transform projectileSpawnPoint;

    [HideInInspector]
    public Transform chaseTarget;
    [HideInInspector]
    public IEnemyState currentState;
    [HideInInspector]
    public ChaseState chaseState;
    [HideInInspector]
    public AttackState attackState;
    [HideInInspector]
    public PatrolState patrolState;

    private void Awake()
    {
        chaseState = new ChaseState(this);
        attackState = new AttackState(this);
        patrolState = new PatrolState(this);
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
        Debug.Log(currentState);
    }

    private void OnTriggerEnter(Collider other)
    {
        currentState.OnTriggerEnter(other);
    }
}
