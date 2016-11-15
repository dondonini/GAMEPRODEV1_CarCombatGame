using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapBuildScript))]
public class MapBuildCustomInspector : Editor {

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapBuildScript mainScript = (MapBuildScript)target;

        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Total Segments: ", mainScript.m_mapPocket.childCount.ToString());
        GUILayout.EndHorizontal();

        if (GUILayout.Button("Build Map"))
        {
            mainScript.BuildMap();
        }
    }
}
