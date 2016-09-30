using UnityEngine;
using System.Collections;
using UnityEditor;

[RequireComponent(typeof(Camera))]
public class SmoothCamera : MonoBehaviour {

    public Transform target;
    public float m_offsetY = 70.0f;
    public float m_offsetZ = -40.0f;
    public float m_stiffness = 100;
    public float m_closestZoom = 10;
    public bool m_lockAtAxisX;
    public bool m_lockAtAxisY;
    public bool m_lockAtAxisZ;

    private float m_lockedX;
    private float m_lockedY;
    private float m_lockedZ;

    private float m_zoomPercentage = 1;

    private float m_lowestY;
    private float m_lowestZ;

    void OnValidate()
    {
        // Calculates closest zoom
        if (Mathf.Abs(m_offsetY) > Mathf.Abs(m_offsetZ))
        {
            if (m_offsetZ < 0.0f)
                m_lowestZ = -m_closestZoom;
            else
                m_lowestZ = m_closestZoom;

            float subtractionPercentage = m_lowestZ / m_offsetZ;

            m_lowestY = m_offsetY * subtractionPercentage;
        }
        else
        {
            if (m_offsetY < 0.0f)
                m_lowestY = -m_closestZoom;
            else
                m_lowestY = m_closestZoom;

            float subtractionPercentage = m_lowestY / m_offsetY;

            m_lowestZ = m_offsetZ * subtractionPercentage;
        }
    }

    void Start()
    {
        m_lockedX = transform.position.x;
        m_lockedY = transform.position.y;
        m_lockedZ = transform.position.z;
    }

    // Update is called once per frame
    void Update() {

    }

    void LateUpdate()
    {
        // Early out if we don't have a target
        if (!target)
            return;

        Vector3 wantedPos = new Vector3(
            m_lockAtAxisX ? m_lockedX : target.position.x,
            m_lockAtAxisY ? m_lockedY : target.position.y + Mathf.Lerp(m_lowestY, m_offsetY, m_zoomPercentage),
            m_lockAtAxisZ ? m_lockedZ : target.position.z + Mathf.Lerp(m_lowestZ, m_offsetZ, m_zoomPercentage)
            );

        float wantedRotationAngle = target.eulerAngles.y;

        float currentRotationAngle = transform.eulerAngles.y;
        Vector3 currentPos = transform.position;

        // Damp the position
        currentPos = Vector3.Lerp(currentPos, wantedPos, m_stiffness * Time.deltaTime);

        // Set the height of the camera
        transform.position = currentPos;

        // Always look at the target
        transform.LookAt(target);
    }
}
