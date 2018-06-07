using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(GR_PhDifferential))]
public class GR_PhDifferentialEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = (GR_PhDifferential)serializedObject.targetObject;
        if (t == null)
            return;

        //NU float defaultLabelWidth = EditorGUIUtility.labelWidth;
        //NU float defaultFieldWidth = EditorGUIUtility.fieldWidth;
        
        GUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("TransmissionType"));

        //GUILayout.BeginHorizontal();
        if (t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.FWD)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Front Differential", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Front.FinalDrive"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Front.Type"));
            if (t.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.TORSEN)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Front.Tbr"));
                t.Front.LockedPercent = string.Format("{0:0.0}%", ((t.Front.Tbr - 1.0f) / (t.Front.Tbr + 1.0f)) * 100.0f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Front.LockedPercent"));
            }
        }

        if (t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.RWD)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Rear Differential", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Rear.FinalDrive"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Rear.Type"));
            if (t.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.TORSEN)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Rear.Tbr"));
                t.Rear.LockedPercent = string.Format("{0:0.0}%", ((t.Rear.Tbr - 1.0f) / (t.Rear.Tbr + 1.0f)) * 100.0f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Rear.LockedPercent"));
            }
        }

        //GUILayout.EndHorizontal();     
        //GUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfDirtyOrScript();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
        }
    }
}