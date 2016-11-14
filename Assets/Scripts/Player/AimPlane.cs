using UnityEngine;
using System.Collections;

public class AimPlane : MonoBehaviour {

    public Transform playerPos;
    public float yOffset = 10.0f;

    private Vector3 offset = Vector3.zero;

    void Start()
    {
        offset = new Vector3(0, yOffset, 0);
    }

	// Update is called once per frame
	void Update () {
	
        if (playerPos)
        {
            transform.position = playerPos.position + offset;
        }
	}
}
