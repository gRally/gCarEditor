using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RallyCar))]
public class RallyCarEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = target as RallyCar;
        if (t == null)
        {
            return;
        }

        EditorGUILayout.LabelField("Lights", EditorStyles.boldLabel);
        GUILayout.BeginHorizontal();
        if (t.DebugLights) GUI.color = Color.yellow; 
        else GUI.color = Color.gray;
        if (GUILayout.Button("Lights"))
        {
            t.DebugLights = !t.DebugLights;
        }

        GUILayout.Space(20);
        if (t.DebugBrakes) GUI.color = Color.red;
        else GUI.color = Color.gray;
        if (GUILayout.Button("Brake"))
        {
            t.DebugBrakes = !t.DebugBrakes;
        }
        GUILayout.Space(20);
        if (t.DebugReverse) GUI.color = Color.white;
        else GUI.color = Color.gray;
        if (GUILayout.Button("Reverse"))
        {
            t.DebugReverse = !t.DebugReverse;
        }
        GUI.color = Color.white;
        GUILayout.EndHorizontal();

        DrawDefaultInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
        }
    }
}