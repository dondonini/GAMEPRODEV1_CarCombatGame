using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(SmoothCamera))]
public class SmoothCameraCustomInspector : Editor
{

    private bool m_lockOnAxis = false;
    private SmoothCamera script = null;

    void OnEnable()
    {
        script = (SmoothCamera)target;
    }

    override public void OnInspectorGUI()
    {
        EditorGUILayout.BeginHorizontal();
        script = EditorGUILayout.ObjectField("Script", script,typeof(SmoothCamera), true) as SmoothCamera;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        script.target = EditorGUILayout.ObjectField("Target", script.target, typeof(Transform), true) as Transform;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        script.m_closestZoom = EditorGUILayout.FloatField("Closest Zoom", script.m_closestZoom);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (!script.m_lockAtAxisY || !script.m_lockAtAxisZ)
        {
            EditorGUILayout.PrefixLabel("Offset");
            if (!script.m_lockAtAxisY)
            {
                GUILayout.Label("Y");
                script.m_offsetY = EditorGUILayout.FloatField(script.m_offsetY);
            }
            if (!script.m_lockAtAxisZ)
            {
                GUILayout.Label("Z");
                script.m_offsetZ = EditorGUILayout.FloatField(script.m_offsetZ);
            }
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        script.m_stiffness = EditorGUILayout.FloatField("Stiffness", script.m_stiffness);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Lock On Position Axis");
        m_lockOnAxis = EditorGUILayout.Toggle(m_lockOnAxis);
        EditorGUILayout.EndHorizontal();

        if (m_lockOnAxis)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Axis To Lock");
            GUILayout.Label("X");
            script.m_lockAtAxisX = EditorGUILayout.Toggle(script.m_lockAtAxisX);
            GUILayout.Label("Y");
            script.m_lockAtAxisY = EditorGUILayout.Toggle(script.m_lockAtAxisY);
            GUILayout.Label("Z");
            script.m_lockAtAxisZ = EditorGUILayout.Toggle(script.m_lockAtAxisZ);
            EditorGUILayout.EndHorizontal();
        }
        else
        {
            // Sets all to false when disabled
            script.m_lockAtAxisX = false;
            script.m_lockAtAxisY = false;
            script.m_lockAtAxisZ = false;
        }
    }
}
