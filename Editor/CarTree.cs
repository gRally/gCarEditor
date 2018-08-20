using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEditor;
using UnityEngine;
using System.IO;

public class CarTree : EditorWindow
{
    public string WheelBase = "2.470";
    public string TrackTreadFront = "1.500";
    public string TrackTreadRear = "1.500";
    public string FrontWheel ="24/61-17";
    public string RearWheel = "24/61-17";

    float _wheelBase;
    float _trackTreadFront;
    float _trackTreadRear;

    void OnGUI()
    {
        //string wb = WheelBase.ToString(CultureInfo.InvariantCulture);
        //string tf = TrackTreadFront.ToString(CultureInfo.InvariantCulture);
        //string tr = TrackTreadRear.ToString(CultureInfo.InvariantCulture);
        
        WheelBase = EditorGUILayout.TextField("Wheel base:", WheelBase);
        TrackTreadFront = EditorGUILayout.TextField("Track/tread (front)", TrackTreadFront);
        TrackTreadRear = EditorGUILayout.TextField("Track/tread (rear)", TrackTreadRear);
        FrontWheel = EditorGUILayout.TextField("Wheel size (front)", FrontWheel);
        RearWheel = EditorGUILayout.TextField("Wheel size (rear)", RearWheel);

        if (GUILayout.Button("Create car tree"))
        {
            _wheelBase = Convert.ToSingle(WheelBase, CultureInfo.InvariantCulture);
            _trackTreadFront = Convert.ToSingle(TrackTreadFront, CultureInfo.InvariantCulture);
            _trackTreadRear = Convert.ToSingle(TrackTreadRear, CultureInfo.InvariantCulture);

            var rc = GameObject.Find("RallyCar");
            if (rc != null)
            {
                EditorUtility.DisplayDialog("Create Car", "Error! RallyCar GambeObject exists", "Ok!!");
            }
            else
            {
                CreateCarTree();
                DefaultValues();
            }

            Close();
        }
            //CreateNewProject(inputText);

        if (GUILayout.Button("Abort"))
        {
            Close();
        }
    }

    void CreateCarTree()
    {
        // tag manager
        if (!File.Exists("ProjectSettings/TagManager.asset.bak"))
        {
            if (File.Exists("ProjectSettings/TagManager.asset"))
            {
                File.Move("ProjectSettings/TagManager.asset", "ProjectSettings/TagManager.asset.bak");
            }
            File.Copy("Assets/gCarEditor/Resources/TagManager.asset.txt", "ProjectSettings/TagManager.asset");
            Debug.Log("Layers file created successfully.");
            Debug.Log("Remember to save the Project to validate the changes.");
        }

        AddChild(null, "RallyCar", typeof(RallyCar), typeof(CarDamage));
        // rally car
        var rc = GameObject.Find("RallyCar");
        AddChild(rc, "Body", typeof(CarLivery), typeof(CarSound));
        AddChild(rc, "WheelFL");
        AddChild(rc, "WheelFR");
        AddChild(rc, "WheelBL");
        AddChild(rc, "WheelBR");
        AddChild(rc, "SuspFL");
        AddChild(rc, "SuspFR");
        AddChild(rc, "SuspBL");
        AddChild(rc, "SuspBR");
        
        // body
        var bd = GameObject.Find("Body");
        AddChild(bd, "Physics", typeof(CarPhysics));
        UnityEngine.Object.FindObjectOfType<CarPhysics>().InitPhysics();
        AddChild(bd, "DetectionPoint");
        AddChild(bd, "SteeringWheel");
        AddChild(bd, "Common");
        var cmn = GameObject.Find("Common");
        AddChild(cmn, "Cameras", typeof(GR_Mirrors));
        AddChild(bd, "Ext");
        AddChild(bd, "Int");
        AddChild(bd, "Cameras");
        AddChild(bd, "DashCanvas", typeof(Canvas));
        
        // dash canvas
        var cnv = GameObject.Find("DashCanvas");
        cnv.transform.position = new Vector3(-0.32f, 0.48f, 1.795f);
        cnv.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0.35f, 0.15f);

        AddChild(cnv, "DashKmh", typeof(UnityEngine.UI.RawImage), typeof(Dash));
        AddChild(cnv, "DashRpm", typeof(UnityEngine.UI.RawImage), typeof(Dash));
        
        // collision
        AddChild(bd, "collision", typeof(CreateConvexHull), typeof(MeshCollider));
        
        // lights
        AddChild(bd, "FrontLight L", typeof(Light));
        AddChild(bd, "FrontLight R", typeof(Light));
        AddChild(bd, "RearLight L", typeof(Light));
        AddChild(bd, "RearLight R", typeof(Light));
        AddChild(bd, "BrakeLight L", typeof(Light));
        AddChild(bd, "BrakeLight R", typeof(Light));
        AddChild(bd, "RevLight L", typeof(Light));
        AddChild(bd, "RevLight R", typeof(Light));

        // car livery
        var lvr = FindObjectOfType<CarLivery>();
        lvr.PlatesSampler = Resources.Load("Textures/Plates_Sampler") as Texture2D;
        lvr.DriverNameSampler = Resources.Load("Textures/Driver_Sampler") as Texture2D;
        lvr.DecalsSampler = Resources.Load("Textures/Decals_Sampler") as Texture2D;
        lvr.Patterns = new List<Texture2D>();
        lvr.Patterns.Add(Resources.Load("Textures/carPattern00") as Texture2D);

        // projector
        AddChild(bd, "Projector", typeof(Projector));
        var proj = GameObject.FindObjectOfType<Projector>();
        proj.transform.rotation = Quaternion.Euler(new Vector3(90.0f, 0.0f, 0.0f));
        proj.transform.position = new Vector3(0.0f, 1.35f, _wheelBase * 0.5f);
        proj.material = Resources.Load("Materials/Projection") as Material;
        proj.nearClipPlane = 1.1f;
        proj.farClipPlane = 2.04f;
        proj.fieldOfView = 98.0f;
        proj.aspectRatio = 0.6f;
        proj.orthographicSize = 1.83f;
        proj.ignoreLayers = (1 << 8) | (1 << 9);

        // cameras
        var cm = GameObject.Find("RallyCar/Body/Cameras");
        AddChild(cm, "Fixed_Camera_1");
        AddChild(cm, "Fixed_Camera_2");
        AddChild(cm, "Fixed_Camera_Driver_View");
        AddChild(cm, "Fixed_Camera_Driver_Locked");
        AddChild(cm, "Fixed_Camera_Bonnet");
        AddChild(cm, "Fixed_Camera_Bumper");
    }

    void DefaultValues()
    {
        // create rearAxle
        var test = GameObject.Find("rearAxle");
        if (test == null)
        {
            var rearAxle = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            var mat = new Material(Shader.Find("Standard"));
            mat.color = new Color32(170, 229, 151, 255);
            rearAxle.GetComponent<Renderer>().sharedMaterial = mat;
            rearAxle.name = "rearAxle";
            rearAxle.transform.rotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            rearAxle.transform.position = Vector3.zero;
            rearAxle.transform.localScale = new Vector3(0.05f, 3.0f, 0.05f);
        }

        // wheels:
        GameObject.Find("RallyCar/WheelFL").transform.position = new Vector3(_trackTreadFront * -0.5f, 0.0f, _wheelBase);
        GameObject.Find("RallyCar/WheelFR").transform.position = new Vector3(_trackTreadFront * 0.5f, 0.0f, _wheelBase);
        GameObject.Find("RallyCar/WheelBL").transform.position = new Vector3(_trackTreadRear * -0.5f, 0.0f, 0.0f);
        GameObject.Find("RallyCar/WheelBR").transform.position = new Vector3(_trackTreadRear * 0.5f, 0.0f, 0.0f);

        // suspension
        GameObject.Find("RallyCar/SuspFL").transform.position = new Vector3(_trackTreadFront * -0.5f, 0.0f, _wheelBase);
        GameObject.Find("RallyCar/SuspFR").transform.position = new Vector3(_trackTreadFront * 0.5f, 0.0f, _wheelBase);
        GameObject.Find("RallyCar/SuspBL").transform.position = new Vector3(_trackTreadRear * -0.5f, 0.0f, 0.0f);
        GameObject.Find("RallyCar/SuspBR").transform.position = new Vector3(_trackTreadRear * 0.5f, 0.0f, 0.0f);

        // GR_PhWheel
        var matW = new Material(Shader.Find("Standard"));
        matW.color = new Color32(40, 40, 40, 255);

        foreach (var wh in GameObject.FindObjectsOfType<GR_PhWheel>())
        {
            if (wh.name == "WheelFL")
            {
                wh.transform.position = new Vector3(_trackTreadFront * -0.5f, 0.0f, _wheelBase);
                wh.SuspensionType = GR_PhWheel.SUSPENSION_TYPE.MACPHERSON;
                wh.WheelPosition = gPhys.WHEEL_POSITION.FRONT_LEFT;
                wh.WheelSize = FrontWheel;
               
                wh.StrutTop = wh.transform.position + new Vector3(0.19f, 0.55f, 0.0f);
                wh.StrutHinge = wh.transform.position + new Vector3(0.385f, 0.06f, 0.0f);
                wh.Camber = 4.0f;
                wh.Caster = 2.5f;
                wh.StrutLen = 0.28f;
                wh.HubHeight = 0.115f;
                wh.HubWidth = 0.134f;

                wh.SpringConstant = 30.0f;
                wh.Bounce = 4.0f;
                wh.Rebound = 5.0f;
                wh.Travel = 0.21f;
                wh.AntiRoll = 4.5f;
            }
            else if (wh.name == "WheelFR")
            {
                wh.transform.position = new Vector3(_trackTreadFront * 0.5f, 0.0f, _wheelBase);
                wh.SuspensionType = GR_PhWheel.SUSPENSION_TYPE.MACPHERSON;
                wh.WheelPosition = gPhys.WHEEL_POSITION.FRONT_RIGHT;
                wh.WheelSize = FrontWheel;
                
                wh.StrutTop = wh.transform.position + new Vector3(-0.19f, 0.55f, 0.0f);
                wh.StrutHinge = wh.transform.position + new Vector3(-0.385f, 0.06f, 0.0f);
                wh.Camber = -4.0f;
                wh.Caster = 2.5f;
                wh.StrutLen = 0.28f;
                wh.HubHeight = 0.115f;
                wh.HubWidth = 0.134f;

                wh.SpringConstant = 30.0f;
                wh.Bounce = 4.0f;
                wh.Rebound = 5.0f;
                wh.Travel = 0.21f;
                wh.AntiRoll = 4.5f;
            }
            else if (wh.name == "WheelBL")
            {
                wh.transform.position = new Vector3(_trackTreadRear * -0.5f, 0.0f, 0.0f);
                wh.SuspensionType = GR_PhWheel.SUSPENSION_TYPE.SIMPLE;
                wh.WheelPosition = gPhys.WHEEL_POSITION.REAR_LEFT;
                wh.WheelPos = wh.transform.position;
                wh.WheelSize = RearWheel;

                wh.WheelHinge = wh.transform.position + new Vector3(-0.02f, -0.01f, 0.0f);
                wh.WheelChassis = wh.transform.position + new Vector3(1.261f, 0.351f, 0.0f);

                wh.SpringConstant = 30.0f;
                wh.Bounce = 4.0f;
                wh.Rebound = 6.0f;
                wh.Travel = 0.28f;
                wh.AntiRoll = 4.5f;
            }
            else if (wh.name == "WheelBR")
            {
                wh.transform.position = new Vector3(_trackTreadRear * 0.5f, 0.0f, 0.0f);
                wh.SuspensionType = GR_PhWheel.SUSPENSION_TYPE.SIMPLE;
                wh.WheelPosition = gPhys.WHEEL_POSITION.REAR_RIGHT;
                wh.WheelPos = wh.transform.position;
                wh.WheelSize = RearWheel;

                wh.WheelHinge = wh.transform.position + new Vector3(0.02f, -0.01f, 0.0f);
                wh.WheelChassis = wh.transform.position + new Vector3(-1.261f, 0.351f, 0.0f);

                wh.SpringConstant = 30.0f;
                wh.Bounce = 4.0f;
                wh.Rebound = 6.0f;
                wh.Travel = 0.28f;
                wh.AntiRoll = 4.5f;
            }

            wh.NominalPressure = 1.85f;
            wh.InitGizmo();

            // create the cylinder
            var cylinder = GameObject.CreatePrimitive(PrimitiveType.Cylinder);
            cylinder.GetComponent<Renderer>().sharedMaterial = matW;
            cylinder.name = "dummy" + wh.name;
            cylinder.transform.parent = GameObject.Find("RallyCar/" + wh.name).transform;
            cylinder.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 90.0f);
            cylinder.transform.localPosition = Vector3.zero;
            cylinder.transform.localScale = new Vector3(wh.GetRadius() * 2.0f, wh.GetWidth() * 0.5f, wh.GetRadius() * 2.0f);
        }

        // dash
        foreach (var dsh in GameObject.FindObjectsOfType<Dash>())
        {
            dsh.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
            dsh.gameObject.GetComponent<RectTransform>().sizeDelta = new Vector2(0.2f, 0.2f);
            dsh.gameObject.GetComponent<UnityEngine.UI.RawImage>().material = Resources.Load("Materials/DashMat") as Material;
            if (dsh.name == "DashKmh")
            {
                dsh.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(-0.05f, 0.0f, 0.0f);
                dsh.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 120.0f));
                dsh.Type = Dash.DashType.Kmh;
                dsh.MaxDegree = -120.0f;
                dsh.MaxValue = 200.0f;
            }
            else if (dsh.name == "DashRpm")
            {
                dsh.Type = Dash.DashType.Rpm;
                dsh.gameObject.GetComponent<RectTransform>().localPosition = new Vector3(0.05f, 0.0f, 0.0f);
                dsh.gameObject.GetComponent<RectTransform>().localRotation = Quaternion.Euler(new Vector3(0.0f, 0.0f, 120.0f));
                dsh.MaxDegree = -120.0f;
                dsh.MaxValue = 10000.0f;
            }
        }

        // engine
        GameObject.FindObjectOfType<GR_PhEngine>().transform.position = new Vector3(0.0f, 0.2f, _wheelBase);

        // clutch
        GameObject.FindObjectOfType<GR_PhClutch>().transform.position = new Vector3(0.0f, 0.2f, _wheelBase - 0.2f);

        // transmission
        GameObject.FindObjectOfType<GR_PhTransmission>().transform.position = new Vector3(0.0f, 0.2f, _wheelBase - 0.4f);

        // differential
        GameObject.FindObjectOfType<GR_PhDifferential>().transform.position = new Vector3(0.0f, 0.2f, _wheelBase - 0.6f);

        // fuel tank
        GameObject.FindObjectOfType<GR_PhFuel>().transform.position = new Vector3(0.0f, 0.6f, 0.0f);

        // lights
        var cookie = Resources.Load("Textures/test_carlights_2") as Texture;
        var rc = FindObjectOfType<RallyCar>();
        rc.frontLights = new Light[2];
        rc.rearLights = new Light[2];
        rc.rearBrakeLights = new Light[2];
        rc.reverseLights = new Light[2];
        foreach (var light in GameObject.FindObjectsOfType<Light>())
        {
            if (light.name.StartsWith("FrontLight "))
            {
                if (light.name.EndsWith("L"))
                {
                    light.transform.position = new Vector3(_trackTreadFront * -0.4f, 0.22f, _wheelBase + 0.6f);
                    rc.frontLights[0] = light;
                }
                else
                {
                    light.transform.position = new Vector3(_trackTreadFront * 0.4f, 0.22f, _wheelBase + 0.6f);
                    rc.frontLights[1] = light;
                }
                light.type = LightType.Spot;
                light.shadows = LightShadows.Hard;
                light.spotAngle = 130.0f;
                light.range = 80.0f;
                light.cookie = cookie;
                light.enabled = true;
                light.transform.gameObject.SetActive(false);
            }
            else if (light.name.StartsWith("RearLight "))
            {
                if (light.name.EndsWith("L"))
                {
                    light.transform.position = new Vector3(_trackTreadRear * -0.4f, 0.32f, -0.6f);
                    rc.rearLights[0] = light;
                }
                else
                {
                    light.transform.position = new Vector3(_trackTreadRear * 0.4f, 0.32f, -0.6f);
                    rc.rearLights[1] = light;
                }
                light.type = LightType.Spot;
                light.spotAngle = 93.0f;
                light.range = 10.0f;
                light.color = Color.red;
                light.intensity = 0.5f;
                light.transform.rotation = Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f));
                light.cookie = cookie;
                light.enabled = true;
                light.transform.gameObject.SetActive(false);
            }
            else if (light.name.StartsWith("BrakeLight "))
            {
                if (light.name.EndsWith("L"))
                {
                    light.transform.position = new Vector3(_trackTreadRear * -0.41f, 0.322f, -0.6f);
                    rc.rearBrakeLights[0] = light;
                }
                else
                {
                    light.transform.position = new Vector3(_trackTreadRear * 0.41f, 0.322f, -0.6f);
                    rc.rearBrakeLights[1] = light;
                }
                light.type = LightType.Spot;
                light.spotAngle = 93.0f;
                light.range = 50.0f;
                light.color = Color.red;
                light.intensity = 0.75f;
                light.transform.rotation = Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f));
                light.cookie = cookie;
                light.enabled = true;
                light.transform.gameObject.SetActive(false);
            }
            else if (light.name.StartsWith("RevLight "))
            {
                if (light.name.EndsWith("L"))
                {
                    light.transform.position = new Vector3(_trackTreadRear * -0.36f, 0.32f, -0.6f);
                    rc.reverseLights[0] = light;
                }
                else
                {
                    light.transform.position = new Vector3(_trackTreadRear * 0.36f, 0.32f, -0.6f);
                    rc.reverseLights[1] = light;
                }
                
                light.type = LightType.Spot;
                light.spotAngle = 95.0f;
                light.range = 10.0f;
                light.color = new Color32(255, 255, 240, 255);
                light.intensity = 1.0f;
                light.transform.rotation = Quaternion.Euler(new Vector3(180.0f, 0.0f, 0.0f));
                light.cookie = cookie;
                light.enabled = true;
                light.transform.gameObject.SetActive(false);
            }
        }

        // detection point
        var dp = GameObject.Find("DetectionPoint");
        dp.transform.position = new Vector3(0.0f, 0.22f, _wheelBase + 0.61f);
        foreach (var w in FindObjectsOfType<GR_PhWing>())
        {
            if (w.name == "WingFront")
            {
                w.transform.position = new Vector3(0.0f, 0.22f, _wheelBase + 0.61f);
            }
            else if (w.name == "WingRear")
            {
                w.transform.position = new Vector3(0.0f, 0.7f, -0.5f);
            }
        }

        var dg = FindObjectOfType<GR_PhDrag>();
        dg.transform.position = new Vector3(0.0f, 0.8f, _wheelBase * 0.6f);

        // steering wheel
        var sw = GameObject.Find("Body/SteeringWheel");
        sw.transform.position = new Vector3(-0.337f, 0.5f, 1.5f);
        sw.transform.rotation = Quaternion.Euler(new Vector3(17.0f, 0.0f, 0.0f));

        // cameras
        var cm = GameObject.Find("Fixed_Camera_1");
        cm.transform.position = new Vector3(-_trackTreadRear * 0.9f, 0.277f, 1.003f);
        cm = GameObject.Find("Fixed_Camera_2");
        cm.transform.position = new Vector3(_trackTreadRear * 0.9f, 0.277f, 1.003f);
        cm = GameObject.Find("Fixed_Camera_Driver_View");
        cm.transform.position = new Vector3(-0.331f, 0.69f, 1.02f);
        cm = GameObject.Find("Fixed_Camera_Driver_Locked");
        cm.transform.position = new Vector3(-0.331f, 0.69f, 1.4f);
        cm = GameObject.Find("Fixed_Camera_Bonnet");
        cm.transform.position = new Vector3(0.0f, 0.66f, 2.06f);
        cm = GameObject.Find("Fixed_Camera_Bumper");
        cm.transform.position = new Vector3(0.0f, 0.25f, _wheelBase + 0.65f);

        // mirrors
        var cmn = GameObject.Find("Common/Cameras");
        AddChild(cmn, "L_Mirror", typeof(Camera));
        AddChild(cmn, "R_Mirror", typeof(Camera));

        var camObj = GameObject.Find("L_Mirror");
        var cam = camObj.GetComponentInChildren<Camera>();
        camObj.transform.position = new Vector3(_trackTreadFront * -1.1f, 0.7f, _wheelBase * 0.8f);
        camObj.transform.rotation = Quaternion.Euler(new Vector3(1.5f, 216.4f, 0.33f));
        cam.nearClipPlane = 0.2f;
        cam.farClipPlane = 400.0f;
        cam.targetTexture = Resources.Load("Textures/L_MirrorTex") as RenderTexture;
        
        camObj = GameObject.Find("R_Mirror");
        cam = camObj.GetComponentInChildren<Camera>();
        camObj.transform.position = new Vector3(_trackTreadFront * 1.1f, 0.7f, _wheelBase * 0.8f);
        camObj.transform.rotation = Quaternion.Euler(new Vector3(1.5f, -209.7f, 0.33f));
        cam.nearClipPlane = 0.2f;
        cam.farClipPlane = 400.0f;
        cam.targetTexture = Resources.Load("Textures/R_MirrorTex") as RenderTexture;

        var mrrMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mrrMesh.name = "L_MirrorMesh";
        mrrMesh.transform.parent = GameObject.Find("Common/Cameras").transform;
        mrrMesh.transform.position = new Vector3(_trackTreadFront * -1.1f, 0.7f, _wheelBase * 0.8f - 0.08f);
        mrrMesh.transform.localScale = new Vector3(0.017f, 1.0f, -0.0115f);
        mrrMesh.transform.rotation = Quaternion.Euler(new Vector3(-89.755f, -13.551f, 0.0f));
        mrrMesh.GetComponent<MeshRenderer>().material = Resources.Load("Materials/L_MirrorMat") as Material;

        mrrMesh = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mrrMesh.name = "R_MirrorMesh";
        mrrMesh.transform.parent = GameObject.Find("Common/Cameras").transform;
        mrrMesh.transform.position = new Vector3(_trackTreadFront * 1.1f, 0.7f, _wheelBase * 0.8f - 0.08f);
        mrrMesh.transform.localScale = new Vector3(0.017f, 1.0f, -0.0115f);
        mrrMesh.transform.rotation = Quaternion.Euler(new Vector3(-89.755f, 13.551f, 0.0f));
        mrrMesh.GetComponent<MeshRenderer>().material = Resources.Load("Materials/R_MirrorMat") as Material;
    }

    void AddChild(GameObject parent, string childName, Type childType = null, Type secondaryChildType = null)
    {
        if (parent == null)
        {
            // i'm on the root
            if (GameObject.Find(childName) != null) return;
        }
        else
        {
            // not the root expected
            if (parent.transform.Find(childName) != null) return;
        }
        var phElement = new GameObject(childName);
        phElement.layer = 8;
        if (parent != null)
        {
            phElement.transform.parent = parent.transform;
        }
        if (childType == null) return;
        phElement.AddComponent(childType);
        if (secondaryChildType == null) return;
        phElement.AddComponent(secondaryChildType);
    }    
}