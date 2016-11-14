using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

    public GameObject turret;
    public GameObject turretBarrel;
    public Transform projectileSpawnLocation;

    LayerMask layerMask = 1 << 9;

	// Use this for initialization
	void Start () {

 
	}
	
	// Update is called once per frame
	void Update () {

        AimTurret();
    }

    void AimTurret()
    {
        Camera currentCamera = Camera.current;
        Vector3 mousePoint = Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            Vector3 lookAtPos = hit.point - turret.transform.position;

            lookAtPos.y = -90.0f;

            Quaternion rotation = Quaternion.LookRotation(lookAtPos);

            turret.transform.rotation = rotation;
        }
    }
}
