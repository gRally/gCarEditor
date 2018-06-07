using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(GR_PhWheel))]
public class GR_PhWheelEditorEditor : Editor
{
    private void OnSceneGUI()
    {
        var t = target as GR_PhWheel;

        switch (t.SuspensionType)
        {
            case GR_PhWheel.SUSPENSION_TYPE.SIMPLE:
                Handles.color = Color.yellow;
                Handles.SphereHandleCap(0, t.WheelHinge, Quaternion.identity, 0.05f, EventType.Repaint);
                Handles.color = new Color32(205, 107, 0, 255);
                Handles.SphereHandleCap(0, t.WheelChassis, Quaternion.identity, 0.05f, EventType.Repaint);
                Handles.color = Color.green;
                Handles.SphereHandleCap(0, t.WheelPos, Quaternion.identity, 0.025f, EventType.Repaint);
                Handles.color = Color.yellow;
                Handles.DrawLine(t.WheelHinge, t.WheelChassis);
                break;
            case GR_PhWheel.SUSPENSION_TYPE.MACPHERSON:
                Handles.color = Color.white;
                Handles.SphereHandleCap(0, t.StrutTop, Quaternion.identity, 0.02f, EventType.Repaint);

                Handles.color = Color.yellow;
                Handles.SphereHandleCap(0, t.StrutHingeTop, Quaternion.identity, 0.02f, EventType.Repaint);

                Handles.color = new Color32(255, 147, 0, 255);
                Handles.SphereHandleCap(0, t.StrutEnd, Quaternion.identity, 0.02f, EventType.Repaint);

                Handles.color = new Color32(255, 0, 0, 255);
                Handles.SphereHandleCap(0, t.StrutHinge, Quaternion.identity, 0.02f, EventType.Repaint);
                /* not right
                Handles.color = new Color32(0, 159, 255, 255);
                Handles.SphereHandleCap(0, t.debugCenterRadius, Quaternion.identity, 0.025f, EventType.Repaint);
                Handles.DrawWireArc(t.debugCenterRadius, Vector3.forward, Vector3.up, 360.0f, (t.StrutEnd - t.debugCenterRadius).magnitude);
                */
                Handles.color = Color.green;
                Handles.SphereHandleCap(0, t.WheelPos, Quaternion.identity, 0.025f, EventType.Repaint);

                // draw hub
                Handles.color = Color.cyan;
                Handles.DrawLine(t.StrutHingeTop, t.WheelPos);
                Handles.DrawLine(t.WheelPos, t.StrutEnd);
                Handles.DrawLine(t.StrutEnd, t.StrutHingeTop);
                /* not right
                Handles.DrawLine(t.StrutTop, t.debugCenterRadius);
                Handles.DrawLine(t.StrutEnd, t.debugCenterRadius);
                */
                Handles.color = Color.yellow;
                Handles.DrawLine(t.StrutTop, t.StrutEnd);

                Handles.color = new Color32(238, 129, 152, 255);
                Handles.DrawLine(t.StrutEnd, t.StrutHinge);

                // draw limit suspension
                Handles.color = Color.white;
                var susp = t.StrutEnd - t.StrutTop;
                Handles.SphereHandleCap(0, t.StrutTop + susp.normalized * t.MinSuspLen, Quaternion.identity, 0.02f, EventType.Repaint);

                // draw wheel
                Handles.color = new Color32(255, 255, 255, 255); //76, 76, 76, 128);
                //Handles.DrawLine(t.WheelPos + Vector3.up * t.tyreRadius, t.WheelPos - Vector3.up * t.tyreRadius);
                //Handles.DrawLine(t.WheelPos + Vector3.left * t.tyreWidth * 0.5f, t.WheelPos - Vector3.left * t.tyreWidth * 0.5f);
                //Handles.DrawLine(t.WheelPos - Vector3.up * t.tyreRadius + Vector3.left * t.tyreWidth * 0.5f, t.WheelPos - Vector3.up * t.tyreRadius - Vector3.left * t.tyreWidth * 0.5f);
                //Handles.DrawLine(t.WheelPos + Vector3.up * t.tyreRadius + Vector3.left * t.tyreWidth * 0.5f, t.WheelPos + Vector3.up * t.tyreRadius - Vector3.left * t.tyreWidth * 0.5f);

                Handles.DrawLine(t.WheelPosDn, t.WheelPosUp);

                //Handles.DrawSolidDisc(t.WheelPos, Vector3.left, t.tyreRadius);
                //Handles.DrawSolidRectangleWithOutline(new Rect(t.wh))

                // Draw O
                Handles.DrawDottedLine(t.Opos, t.StrutHinge, 1.0f);
                Handles.DrawDottedLine(t.Opos, t.StrutTop, 1.0f);

                Handles.color = Color.red;
                Handles.DrawWireArc(t.StrutHinge, Vector3.forward, Vector3.up, 360.0f, t.StrutLen);
                break;
            default:
                break;
        }
    }

    public override void OnInspectorGUI()
    {
        var t = (GR_PhWheel)serializedObject.targetObject;
        if (t == null)
            return;

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Wheel", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelPosition"), new GUIContent("[O]"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelSize.x"), new GUIContent ("Tire Width [mm]"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelSize.z"), new GUIContent("Height/Width Ratio"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelSize.y"), new GUIContent("Rim Diameter [inch]"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("NominalPressure"), new GUIContent("Nominal Pressure [bar]"));
        GUILayout.Space(5);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Camber"), new GUIContent("Camber [°]"));
        //EditorGUILayout.PropertyField(serializedObject.FindProperty("CurrentCamber"), new GUIContent("Curr.Camber [°]"));

        GUILayout.BeginHorizontal();
        GUILayout.Label("Curr.Camber[°]"); //, EditorStyles.wordWrappedMiniLabel, new GUILayoutOption[0]);
        EditorGUILayout.LabelField(string.Format("{0:0.000}", t.CurrentCamber), EditorStyles.textArea); //, new GUILayoutOption[0]);
        GUILayout.EndHorizontal();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("Caster"), new GUIContent("Caster [°]"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Toe"), new GUIContent("Toe [°]"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("DrawWireFrame"), new GUIContent("Draw Wheel Wireframe")); 

        GUILayout.Space(10);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SuspensionType"));
        
        switch (t.SuspensionType)
        {
            case GR_PhWheel.SUSPENSION_TYPE.SIMPLE:
                EditorGUILayout.LabelField("Simple Suspension", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelHinge"), new GUIContent("Wheel Hinge (Yellow)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelChassis"), new GUIContent("Wheel Chassis (Orange)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelPos"), new GUIContent("Wheel Pos (Green)"));
                break;
            case GR_PhWheel.SUSPENSION_TYPE.MACPHERSON:
                EditorGUILayout.LabelField("MacPherson Suspension", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("known things:");
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StrutTop"), new GUIContent("Strut Top (White)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StrutHinge"), new GUIContent("Strut Hinge (Red)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StrutLen"), new GUIContent("Bottom Strut Len (Pink)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HubHeight"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("HubWidth"));
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StrutEnd"), new GUIContent("Strut End (Orange)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("StrutHingeTop"), new GUIContent("Strut Hinge Top (Yellow)"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty("WheelPos"), new GUIContent("Wheel Pos (Green)"));
                GUILayout.Space(10);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("MinSuspLen")); 
                break;
            default:
                break;
        }

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Coilover", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(serializedObject.FindProperty("SpringConstant"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Bounce"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Rebound"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("Travel"));
        EditorGUILayout.PropertyField(serializedObject.FindProperty("AntiRoll"));


        if (t.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.MACPHERSON)
        {
            GUILayout.Space(15);
            EditorGUILayout.LabelField("Debug", EditorStyles.boldLabel);
            //EditorGUILayout.PropertyField(serializedObject.FindProperty("DistanceTraveled"));
            t.DistanceTraveled = EditorGUILayout.Slider(new GUIContent("Distance Traveled"), t.DistanceTraveled, 0, t.Travel);
        }

        serializedObject.ApplyModifiedProperties();
        serializedObject.UpdateIfRequiredOrScript();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(t);
        }
    }
}
