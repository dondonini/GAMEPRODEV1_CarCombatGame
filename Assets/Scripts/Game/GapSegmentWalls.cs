using UnityEngine;
using System.Collections;

public class GapSegmentWalls : MonoBehaviour {

    public GameObject frontWall;
    public GameObject backWall;
    public GameObject leftWall;
    public GameObject rightWall;

    private Vector3 originOffset = new Vector3(0, 5, 0);
    private float maxCheckDistance = 100;

    // Use this for initialization
    void Start () {

        // Front wall detection

        RaycastHit hit;

        Debug.DrawRay(new Vector3(transform.position.x, 0, transform.position.z) + originOffset + new Vector3(0, 0, 10), Vector3.down * maxCheckDistance, Color.green, 5,true);

        if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z) + originOffset + new Vector3(0, 0, 10), Vector3.down, out hit, maxCheckDistance))
        {
            if (hit.collider && hit.collider.transform.parent.GetComponent<SegmentInfo>() && hit.collider.transform.parent.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Gap)
            {
                frontWall.SetActive(false);
            }
        }

        Debug.Log(hit.collider.transform.parent);


        // Back wall detection
        if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z) + originOffset + new Vector3(0, 0, -10), Vector3.down, out hit, maxCheckDistance))
        {
            if (hit.collider && hit.collider.transform.parent.GetComponent<SegmentInfo>() && hit.collider.transform.parent.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Gap)
            {
                backWall.SetActive(false);
            }
        }

        // Left wall detection
        if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z) + originOffset + new Vector3(-10, 0, 0), Vector3.down, out hit, maxCheckDistance))
        {
            if (hit.collider && hit.collider.transform.parent.GetComponent<SegmentInfo>() && hit.collider.transform.parent.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Gap)
            {
                leftWall.SetActive(false);
            }
        }

        // Right wall detection
        if (Physics.Raycast(new Vector3(transform.position.x, 0, transform.position.z) + originOffset + new Vector3(10, 0, 0), Vector3.down, out hit, maxCheckDistance))
        {
            if (hit.collider && hit.collider.transform.parent.GetComponent<SegmentInfo>() && hit.collider.transform.parent.GetComponent<SegmentInfo>().type == SegmentInfo.Type.Gap)
            {
                rightWall.SetActive(false);
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
