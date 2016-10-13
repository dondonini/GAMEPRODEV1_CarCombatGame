using UnityEngine;
using System.Collections;

public class RotationScript : MonoBehaviour {

    public Vector3 m_rotationOffset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        transform.Rotate(m_rotationOffset);
	}
}
