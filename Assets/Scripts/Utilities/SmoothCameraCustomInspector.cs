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
        GUILayout.BeginHorizontal();
        script.target = EditorGUILayout.ObjectField("Target", script.target, typeof(Transform), true) as Transform;
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        if (!script.m_lockAtAxisY || !script.m_lockAtAxisZ)
        {
            GUILayout.Label("Displacement", GUILayout.Width(200));
            if (!script.m_lockAtAxisY)
            {
                GUILayout.Label("Y");
                script.m_displacementY = EditorGUILayout.FloatField(script.m_displacementY);
            }
            if (!script.m_lockAtAxisZ)
            {
                GUILayout.Label("Z");
                script.m_displacementZ = EditorGUILayout.FloatField(script.m_displacementZ);
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        script.m_stiffness = EditorGUILayout.FloatField("Stiffness", script.m_stiffness);
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Lock On Position Axis");
        m_lockOnAxis = EditorGUILayout.Toggle(m_lockOnAxis);
        GUILayout.EndHorizontal();
        if (m_lockOnAxis)
        {
            GUILayout.Space(5);
            GUILayout.BeginHorizontal();
            GUILayout.Label("Axis", GUILayout.Width(200));
            GUILayout.Label("X");
            script.m_lockAtAxisX = EditorGUILayout.Toggle(script.m_lockAtAxisX);
            GUILayout.Label("Y");
            script.m_lockAtAxisY = EditorGUILayout.Toggle(script.m_lockAtAxisY);
            GUILayout.Label("Z");
            script.m_lockAtAxisZ = EditorGUILayout.Toggle(script.m_lockAtAxisZ);
            GUILayout.EndHorizontal();
        }
    }
}
