using UnityEngine;
using System.Collections;

public class AimPlane : MonoBehaviour {

    public Transform playerPos;
    public float yOffset = 10.0f;

    private Vector3 offset = Vector3.zero;

	// Update is called once per frame
	void Update () {

        offset = new Vector3(0, yOffset, 0);

        if (playerPos)
        {
            transform.position = playerPos.position + offset;
        }
	}
}
