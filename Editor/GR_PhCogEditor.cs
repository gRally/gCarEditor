using UnityEditor;
using UnityEngine;
using System.Collections;
using System;

[CustomEditor(typeof(GR_PhCog))]
public class GR_PhCogEditor : Editor
{
    protected virtual void OnSceneGUI()
    {
        GR_PhCog t = (GR_PhCog)target;
    }

    public override void OnInspectorGUI()
    {
        var t = (GR_PhCog)serializedObject.targetObject;
        if (t == null)
            return;

        DrawDefaultInspector();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
        }
    }
}
