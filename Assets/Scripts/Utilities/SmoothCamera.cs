using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class SmoothCamera : MonoBehaviour {

    public Transform target;
    public float m_displacementY = 70.0f;
    public float m_displacementZ = -40.0f;
    public float m_stiffness = 100;
    public bool m_lockAtAxisX;
    public bool m_lockAtAxisY;
    public bool m_lockAtAxisZ;

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        float wantedHeight = m_lockAtAxisY ? transform.position.y : target.position.y + m_displacementY;
        

    }
}
