using UnityEditor;
using UnityEngine;
using System.Collections;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CarPhysics))]
public class CarPhysicsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        var t = (CarPhysics)target;
        if (t == null)
        {
            return;
        }

        GUILayout.BeginHorizontal();
        GUI.color = new Color32(66, 133, 244, 255);
        if (GUILayout.Button("Export XML"))
        {
            var path = UnityEditor.EditorUtility.SaveFilePanel("Export physics", "", "exportPhysics", "xml");
            if (string.IsNullOrEmpty(path)) return;
            t.ExportXml(path);
        }
        GUILayout.Space(20);
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("Init"))
        {
            t.InitPhysics();
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        // axles
        GUILayout.BeginHorizontal();
        t.RaiseFrontAxle = EditorGUILayout.FloatField("Raise Front Axle:", t.RaiseFrontAxle);
        GUILayout.Space(20);
        if (GUILayout.Button("Raise"))
        {
            if (t.RaiseFront(t.RaiseFrontAxle))
            {
                EditorUtility.DisplayDialog("Raise Axle", string.Format("Front axle raised of {0:0.000} M", t.RaiseFrontAxle), "Ok!!");
                t.RaiseFrontAxle = 0.0f;
            }
            else
            {
                EditorUtility.DisplayDialog("Error Raise Axle", "Error during front axle rising..", "Ok :(");
            }
        }
        GUILayout.EndHorizontal();
        GUILayout.BeginHorizontal();
        t.RaiseRearAxle = EditorGUILayout.FloatField("Raise Rear Axle:", t.RaiseRearAxle);
        GUILayout.Space(20);
        if (GUILayout.Button("Raise"))
        {
            if (t.RaiseRear(t.RaiseRearAxle))
            {
                EditorUtility.DisplayDialog("Raise Axle", string.Format("Rear axle raised of {0:0.000} M", t.RaiseRearAxle), "Ok!!");
                t.RaiseRearAxle = 0.0f;
            }
            else
            {
                EditorUtility.DisplayDialog("Error Raise Axle", "Error during rear axle rising..", "Ok :(");
            }
        }
        GUILayout.EndHorizontal();
        GUI.color = Color.white;

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}
