using UnityEngine;
using System.Collections;

public class FollowZ : MonoBehaviour {

    public Transform m_target;
    public Vector3 m_offset;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        Vector3 newPos = new Vector3(transform.position.x, transform.position.y, m_target.position.z) + m_offset;
        transform.position = newPos;

    }
}
