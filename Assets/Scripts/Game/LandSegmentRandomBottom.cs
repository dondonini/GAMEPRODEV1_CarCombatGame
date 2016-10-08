using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LandSegmentRandomBottom : MonoBehaviour {

    [Tooltip("Transform that holds all dirt chunks")]
    public Transform container;

    [Tooltip("Minimum size for dirt chunk")]
    public float depthMin = 2;

    [Tooltip("Maximum size for dirt chunk")]
    public float depthMax = 10;

    public bool update = false;

    private List<GameObject> DirtBlocks = new List<GameObject>();

    

	// Use this for initialization
	void OnValidate()
    {
        // Updates chunks
        update = false;

        // Changes all dirt chunk sizes to random
        for (int i = 0; i < container.childCount; i++)
        {
            if (container.GetChild(i).name == "Ground")
            {
                Transform chunk = container.GetChild(i);

                float xScale = chunk.localScale.x;
                float yScale = Random.Range(depthMin, depthMax);
                float zScale = chunk.localScale.z;

                chunk.localScale = new Vector3(xScale, yScale, zScale);

                float xPos = chunk.localPosition.x;
                float yPos = (yScale * -1.0f) * 0.5f - 0.5f;
                float zPos = chunk.localPosition.z;

                chunk.localPosition = new Vector3(xPos, yPos, zPos);
            }
        }
    }

    void Start()
    {
        OnValidate();
    }
	
}
