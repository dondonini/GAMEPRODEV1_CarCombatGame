using UnityEngine;
using System.Collections;

public class SegmentInfo : MonoBehaviour {

    public enum Type
    {
        None,
        Bridge,
        Gap,
        Land
    };

    // # segments it takes on X axis
    public Vector2 size = new Vector2(1,1);

    // Segment type
    public Type type;

    // This is for matching multiple segments together
    public string[] matchTag;
	
}
