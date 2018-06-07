using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;

public class MenuCar
{
    static string assetPath = "Assets/Car.data.asset";

    [MenuItem("gRally/1. Create default car data", false, 1)]
    public static void GenerateStageData()
    {
        var asset = ScriptableObject.CreateInstance<CarData>();
        AssetDatabase.CreateAsset(asset, assetPath);

        EditorUtility.SetDirty(asset);
        AssetDatabase.SaveAssets();
        Debug.Log(string.Format("Car data in {0} is created successfully.", assetPath));
    }

    [MenuItem("gRally/2. Create car tree", false, 2)]
    public static void CreateCarTree()
    {
        var size = new CarTree();
        size.ShowUtility();
        return;
    }

    [MenuItem("gRally/Export Car", false, 99)]
    public static void ExportCar()
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTargetGroup.Standalone, BuildTarget.StandaloneWindows);

        var car = (CarData)AssetDatabase.LoadAssetAtPath(assetPath, typeof(CarData));

        string path = EditorUtility.OpenFolderPanel("Save Car", car.exportPath, "");

        if (!string.IsNullOrEmpty(path))
        {
            car.exportPath = path;
            EditorUtility.SetDirty(car);
            AssetDatabase.SaveAssets();

            GameObject go = GameObject.Find("RallyCar");
            if (go == null)
            {
                EditorUtility.DisplayDialog("Create Car", "Error! RallyCar GambeObject not found", "Ok!!");
                return;
            }
            SetLayerRecursively(go, 8);

            string name = go.name;

            UnityEngine.Object prefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
            PrefabUtility.ReplacePrefab(go, prefab);
            AssetDatabase.Refresh();

            foreach (var item in AssetDatabase.GetAllAssetBundleNames())
            {
                Debug.Log(item + " deleted!");
                AssetDatabase.RemoveAssetBundleName(item, true);
            }

            AssetImporter carImporter = AssetImporter.GetAtPath("Assets/RallyCar.prefab");

            carImporter.assetBundleName = "car.grPack";
            BuildPipeline.BuildAssetBundles(path, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);

            EditorUtility.DisplayDialog("Create Car", "Car created in\r\n" + path, "Ok!!");

            // write data car
            gUtility.CXml carXml = new gUtility.CXml(path + @"/car.xml", true);
            string aPath = "Assets/Car.data.asset";
            var carData = (CarData)AssetDatabase.LoadAssetAtPath(aPath, typeof(CarData));
            carXml.Settings["Car"].WriteString("name", carData.Name);
            carXml.Settings["Car"].WriteString("description", carData.Description);
            carXml.Settings["Car"].WriteString("brand", carData.Brand);
            carXml.Settings["Car"].WriteString("group", carData.Group);
            carXml.Settings["Car"].WriteString("transmission", carData.Transmission);
            carXml.Settings["Car"].WriteString("tags", carData.Tags);
            carXml.Commit();

            // export physics
            GameObject.FindObjectOfType<CarPhysics>().ExportXml(path + "/dumpPhysics.xml");

            // export setups
            if (!File.Exists(path + "/Setup.xml"))
            {
                File.Copy("Assets/gCarEditor/Resources/Setup/Setup.xml.txt", path + "/Setup.xml");
            }
            if (!File.Exists(path + "/default.xml"))
            {
                File.Copy("Assets/gCarEditor/Resources/Setup/default.xml.txt", path + "/default.xml");
            }

            // docs folder
            if (!Directory.Exists(path + "/docs"))
            {
                Directory.CreateDirectory(path + "/docs");
            }
            if (!File.Exists(path + "/docs/livery.png"))
            {
                File.Copy("Assets/gCarEditor/Resources/Textures/livery.png", path + "/docs/livery.png");
            }
            if (!File.Exists(path + "/docs/car.png"))
            {
                File.Copy("Assets/gCarEditor/Resources/Textures/car.png", path + "/docs/car.png");
            }
        }
    }

    static void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (null == obj)
        {
            return;
        }
       
        obj.layer = newLayer;
       
        foreach (Transform child in obj.transform)
        {
            if (null == child)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
}
