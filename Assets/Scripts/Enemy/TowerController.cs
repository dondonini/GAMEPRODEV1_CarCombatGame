using UnityEngine;
using System.Collections;

public class TowerController : MonoBehaviour {

    [Range(0f,360f)]
    public float fieldOfView = 45f;

    [Range(0f,100f)]
    public float followAccuracyPercentage = 100f;

    public float launchSpeed = 10;

    public GameObject projectile;
    public Transform projectileSpawnPoint;
    

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
