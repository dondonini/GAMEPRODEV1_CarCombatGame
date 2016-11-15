using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject turret;
    public Transform projectileSpawnLocation;
    public GameObject projectile;
    public float projectileSpawnOffset = 1.0f;
    public float projectileLaunchSpeed = 100.0f;

    LayerMask layerMask = 1 << 9;

    private Vector3 gizmoPosition = Vector3.zero;

	// Use this for initialization
	void Start () {

 
	}
	
	// Update is called once per frame
	void Update () {

        AimTurret();
        InputManager();
    }

    void OnDrawGizmosSelected()
    {
        // Mouse Position relative to aim plane
        Gizmos.color = new Color(0, 1, 0);
        Gizmos.DrawSphere(gizmoPosition, 0.2f);
        Gizmos.DrawLine(turret.transform.position, gizmoPosition);
    }

    void InputManager()
    {
        if (Input.GetMouseButtonDown(0))
        {
            FireWeapon();
        }
    }

    void AimTurret()
    {
        Camera currentCamera = Camera.current;
        Vector3 mousePoint = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            //Vector3 lookAtPos = hit.point - turret.transform.position;

            //lookAtPos.y += -90.0f;

            //Debug.Log(lookAtPos);

            //Quaternion rotation = Quaternion.LookRotation(lookAtPos);

            ////rotation.x = 1.0f;
            ////rotation.y = 0.0f;

            //turret.transform.rotation = rotation;

            Vector3 lookAtPos = hit.point;

            lookAtPos.y = turret.transform.position.y;

            turret.transform.LookAt(lookAtPos);

            gizmoPosition = hit.point;
        }
    }

    void FireWeapon()
    {
        GameObject newProjectile = Instantiate(projectile) as GameObject;

        newProjectile.transform.position = projectileSpawnLocation.transform.position + (projectileSpawnLocation.transform.up * projectileSpawnOffset);
        newProjectile.transform.rotation = projectileSpawnLocation.transform.rotation;

        newProjectile.GetComponent<ProjectileFunction>().BuildCollisionIgnoreList(transform);

        newProjectile.GetComponent<Rigidbody>().AddRelativeForce(new Vector3(0, projectileLaunchSpeed, 0));

    }
}
