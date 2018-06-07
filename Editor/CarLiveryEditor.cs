using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor(typeof(CarLivery))]
public class CarLiveryEditor : Editor
{
    public void Start()
    {
        CarLivery carLivery = target as CarLivery;
    }
    
    private bool showPattern = true;
    private bool showNumberPlates = true;
    private bool showDriverName = true;
    private bool showDecals = true;

    BMObject fNumberPlateLeft;
    BMObject fNumberPlateRight;
    BMObject fDriverNameLeft;
    BMObject fDriverNameRight;
    List<BMObject> fDecals;
    
    public override void OnInspectorGUI()
    {
        CarLivery carLivery = target as CarLivery;

        // copy
        if (fNumberPlateLeft == null) fNumberPlateLeft = new BMObject();
        if (fNumberPlateRight == null) fNumberPlateRight = new BMObject();
        if (fDriverNameLeft == null) fDriverNameLeft = new BMObject();
        if (fDriverNameRight == null) fDriverNameRight = new BMObject();
        if (fDecals == null) fDecals = new List<BMObject>();

        var obj = new SerializedObject(target);

        // TODO: capire quale � quello da gestire, se il LiveryMat o DestinationMaterial
        EditorGUILayout.LabelField ("Livery Material");
        carLivery.LiveryMat = (Material)EditorGUILayout.ObjectField(carLivery.LiveryMat, typeof(Material), true);
        EditorGUILayout.Separator();
        
        EditorGUILayout.PropertyField(obj.FindProperty("Patterns"), true);
        obj.ApplyModifiedProperties();

        EditorGUILayout.Space();
        //carLivery.DestinationMaterial = (Material)EditorGUILayout.ObjectField(carLivery.DestinationMaterial, typeof(Material), true);
        showNumberPlates = EditorGUILayout.Foldout(showNumberPlates, "Number Plates");
        if (showNumberPlates)
        {
            EditorGUI.indentLevel++;
            carLivery.PlatesSampler = (Texture2D)EditorGUILayout.ObjectField(carLivery.PlatesSampler, typeof(Texture2D), true);
            EditorGUILayout.PropertyField(obj.FindProperty("NumberPlateLeft"), true);
            EditorGUILayout.PropertyField(obj.FindProperty("NumberPlateRight"), true);
            obj.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        showDriverName = EditorGUILayout.Foldout(showDriverName, "Driver Name");
        if (showDriverName)
        {
            EditorGUI.indentLevel++;
            carLivery.DriverNameSampler = (Texture2D)EditorGUILayout.ObjectField(carLivery.DriverNameSampler, typeof(Texture2D), true);
            EditorGUILayout.PropertyField(obj.FindProperty("DriverNameLeft"), true);
            EditorGUILayout.PropertyField(obj.FindProperty("DriverNameRight"), true);
            obj.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        showDecals = EditorGUILayout.Foldout(showDecals, "Decals");
        if (showDecals)
        {
            EditorGUI.indentLevel++;
            carLivery.DecalsSampler = (Texture2D)EditorGUILayout.ObjectField(carLivery.DecalsSampler, typeof(Texture2D), true);
            EditorGUILayout.PropertyField(obj.FindProperty("Decals"), true);
            obj.ApplyModifiedProperties();
            EditorGUI.indentLevel--;
        }

        GUI.color = new Color32(134, 214, 164, 255);
        if (GUILayout.Button("Draw Texture"))
        {
            carLivery.DrawTexture();
        }

        GUI.color = new Color32(250, 231, 5, 255);
        if (GUILayout.Button("Save Decal Texture"))
        {
            var path = EditorUtility.SaveFilePanel(
                           "Save Decal Texture as PNG",
                           "",
                           "decal.png",
                           "png");

            if (path.Length != 0)
            {
                carLivery.DrawTexture();


                var tex = carLivery.LiveryMat.GetTexture("_Decals") as Texture2D;
                var bb = tex.EncodeToPNG();
                if (bb != null)
                {
                    File.WriteAllBytes(path, bb);
                }
            }
        }

        GUI.color = Color.white;

        // check if is changed
        if (fNumberPlateLeft.Check(carLivery.NumberPlateLeft) ||
            fNumberPlateRight.Check(carLivery.NumberPlateRight) ||
            fDriverNameLeft.Check(carLivery.DriverNameLeft) ||
            fDriverNameRight.Check(carLivery.DriverNameRight))
        {
            carLivery.DrawTexture();

            fNumberPlateLeft.Set(carLivery.NumberPlateLeft);
            fNumberPlateRight.Set(carLivery.NumberPlateRight);
            fDriverNameLeft.Set(carLivery.DriverNameLeft);
            fDriverNameRight.Set(carLivery.DriverNameRight);
            //fDecals.Set(carLivery.Decals);
        }

        if (carLivery.Decals != null)
        {
            if (carLivery.Decals.Count != fDecals.Count)
            {
                carLivery.DrawTexture();
                fDecals.Clear();
                foreach (var item in carLivery.Decals)
                {
                    fDecals.Add(new BMObject(item));
                }
            }
            else
            {
                for (int i = 0; i < fDecals.Count; i++)
                {
                    if (fDecals[i].Check(carLivery.Decals[i]))
                    {
                        carLivery.DrawTexture();
                        fDecals.Clear();
                        foreach (var item in carLivery.Decals)
                        {
                            fDecals.Add(new BMObject(item));
                        }
                        break;
                    }
                }
            }
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(carLivery);
        }
    }
}

