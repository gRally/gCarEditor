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
        if (t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.FWD || t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.AWD)
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
            else if (t.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_1_WAY ||
                t.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_2_WAY ||
                t.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_15_WAY ||
                t.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.SPEED)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Front.AntiSlip"));
            }
        }

        if (t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.AWD)
        {
            GUILayout.Space(10);
            EditorGUILayout.LabelField("Central Differential", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.FinalDrive"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.Split"));
            EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.Type"));
            if (t.Central.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.TORSEN)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.Tbr"));
                t.Central.LockedPercent = string.Format("{0:0.0}%", ((t.Central.Tbr - 1.0f) / (t.Central.Tbr + 1.0f)) * 100.0f);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.LockedPercent"));
            }
            else if (t.Central.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_1_WAY ||
                t.Central.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_2_WAY ||
                t.Central.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_15_WAY ||
                t.Central.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.SPEED)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Central.AntiSlip"));
            }
        }
 
        if (t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.RWD || t.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.AWD)
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
            else if (t.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_1_WAY ||
                t.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_2_WAY ||
                t.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.LSD_15_WAY ||
                t.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.SPEED)
            {
                EditorGUILayout.PropertyField(serializedObject.FindProperty("Rear.AntiSlip"));
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