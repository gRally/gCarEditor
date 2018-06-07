using UnityEditor;
using UnityEngine;
using System.Collections;

public class MenuTest : MonoBehaviour
{
    [MenuItem("GameObject/Create convex hull", false, 10)]
    static void CreateCustomGameObject() //MenuCommand menuCommand)
    {
        /*
        // Create a custom game object
        GameObject go = new GameObject("Custom Game Object");
        // Ensure it gets reparented if this was a context click (otherwise does nothing)
        GameObjectUtility.SetParentAndAlign(go, menuCommand.context as GameObject);
        // Register the creation in the undo system
        */
        //GameObjectUtility
        //Undo.RegisterCreatedObjectUndo(go, "Create " + go.name);
        //Selection.activeObject = go;
        Debug.Log("obj: " + Selection.activeObject.name);
    }
}

[CustomEditor (typeof(CreateConvexHull))]
public class CreateConvexHullEditor : Editor
{
 
    CreateConvexHull _target;
 
    public override void OnInspectorGUI ()
    {
        _target = (CreateConvexHull)target;
        DrawDefaultInspector();
        if (GUILayout.Button("Create Hull"))
        {
            _target.CreateHull();
        }
    }
}
