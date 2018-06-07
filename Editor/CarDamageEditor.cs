using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

[CustomEditor (typeof(CarDamage))]
public class CarDamageEditor : Editor
{
 
    CarDamage t;
 
    public override void OnInspectorGUI ()
    {
        t = (CarDamage)target;
        GUI.color = new Color32(255,0,51, 255);
        if (GUILayout.Button("Find Meshes"))
        {
            t.FindMeshes();
        }
        GUI.color = Color.white;
        DrawDefaultInspector();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
    }
}