using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Text;

public class CarPhysics : MonoBehaviour
{
#if UNITY_EDITOR

    public float RaiseFrontAxle;
    public float RaiseRearAxle;

    /// <summary>
    /// create the physics stuff
    /// </summary>
    public void InitPhysics()
    {
        AddChild("Engine", typeof(GR_PhEngine));
        AddChild("Clutch", typeof(GR_PhClutch));
        AddChild("Transmission", typeof(GR_PhTransmission));
        AddChild("Differential", typeof(GR_PhDifferential));
        AddChild("FuelTank", typeof(GR_PhFuel));
        AddChild("WingFront", typeof(GR_PhWing));
        AddChild("Drag", typeof(GR_PhDrag));
        AddChild("WingRear", typeof(GR_PhWing));
        AddChild("Brakes", typeof(GR_PhBrake));
        AddChild("WheelFL", typeof(GR_PhWheel));
        AddChild("WheelFR", typeof(GR_PhWheel));
        AddChild("WheelBL", typeof(GR_PhWheel));
        AddChild("WheelBR", typeof(GR_PhWheel));
        AddChild("SteeringWheel", typeof(GR_PhSteering));
        // AddChild("WeightDrv", typeof(GR_PhWeight));
        AddChild("InertiaTensor", typeof(BoxCollider), typeof(GR_PhInertiaTensor));
        AddChild("COG", typeof(GR_PhCog));

        // init default things:
        var engine = FindObjectOfType<GR_PhEngine>();
        engine.PeakEngineRpm = 7000.0f;
        engine.RpmLimit = 8000.0f;
        engine.RpmUpshiftMax = new Vector2(9900.0f, 1.0f);
        engine.RpmDownshiftMax = new Vector2(9900.0f, 1.0f);
        engine.Inertia = 0.4f;
        engine.Idle = 0.08f;
        engine.StartRpm = 1000.0f;
        engine.StallRpm = 400.0f;
        engine.TorqueFriction = 0.001f;
        engine.Torque = new AnimationCurve();
        engine.Torque.AddKey(0000.0f, 000.0f);
        engine.Torque.AddKey(0500.0f, 048.0f);
        engine.Torque.AddKey(1000.0f, 095.0f);
        engine.Torque.AddKey(1500.0f, 142.0f);
        engine.Torque.AddKey(2000.0f, 185.0f);
        engine.Torque.AddKey(2500.0f, 225.0f);
        engine.Torque.AddKey(3000.0f, 260.0f);
        engine.Torque.AddKey(3500.0f, 289.0f);
        engine.Torque.AddKey(4000.0f, 310.0f);
        engine.Torque.AddKey(4500.0f, 323.0f);
        engine.Torque.AddKey(5000.0f, 326.0f);
        engine.Torque.AddKey(5500.0f, 318.0f);
        engine.Torque.AddKey(6000.0f, 297.0f);
        engine.Torque.AddKey(6500.0f, 262.0f);
        engine.Torque.AddKey(7000.0f, 213.0f);
        engine.Torque.AddKey(7500.0f, 147.0f);
        engine.Torque.AddKey(8000.0f, 64.0f);

        var clutch = FindObjectOfType<GR_PhClutch>();
        clutch.Torque = 670.0f;

        var trans = FindObjectOfType<GR_PhTransmission>();
        trans.Gears = 6;
        trans.RatioRear = -2.798f;
        trans.Ratio = new float[6];
        trans.Ratio[0] = 3.132f;
        trans.Ratio[1] = 2.046f;
        trans.Ratio[2] = 1.48f;
        trans.Ratio[3] = 1.162f;
        trans.Ratio[4] = 0.944f;
        trans.Ratio[5] = 0.762f;
        trans.ShiftTime = 0.12f;

        var diff = FindObjectOfType<GR_PhDifferential>();
        diff.TransmissionType = GR_PhDifferential.TRANSMISSION_TYPE.FWD;
        diff.Front = new Differential();
        diff.Front.Type = GR_PhDifferential.DIFFERENTIAL_TYPE.OPEN;
        diff.Front.FinalDrive = 4.2f;

        var tank = FindObjectOfType<GR_PhFuel>();
        tank.Capacity = 50.0f;

        foreach (var wing in FindObjectsOfType<GR_PhWing>())
        {
            if (wing.name == "WingFront")
            {
                wing.Area = 0.21f;
                wing.DownforceK = -0.52f;
                wing.Drag = 0.94f;
            }
            else if (wing.name == "WingRear")
            {
                wing.Area = 0.52f;
                wing.DownforceK = -0.185f;
                wing.Drag = 0.93f;
            }
        }

        var drag = FindObjectOfType<GR_PhDrag>();
        drag.Area = 0.21f;
        drag.K = 0.352f;

        var bk = FindObjectOfType<GR_PhBrake>();
        bk.FrontTorque = 4500.0f;
        bk.FrontRepartition = 65.0f;

        bk.RearTorque = 4500.0f;
        bk.HandBrake = 30.0f;

        var steer = FindObjectOfType<GR_PhSteering>();
        steer.LockToLock = 900.0f;
        steer.WheelRange = 56.0f;
        steer.Ackermann = 8.9f;

        var it = FindObjectOfType<GR_PhInertiaTensor>();
        it.transform.localScale = new Vector3(2.04f, 1.1f, 4.5f);
        it.transform.position = new Vector3(0.0f, 0.41f, 1.332f);
        it.Mass = 1000.0f;

        var cog = FindObjectOfType<GR_PhCog>();
        cog.transform.position = new Vector3(0.0f, 0.3f, 1.9f);
    }

    private void AddChild(string childName, Type childType, Type secondaryChildType = null)
    {
        if (transform.Find(childName) != null) return;
        var phElement = new GameObject(childName);
        phElement.layer = 8;
        phElement.transform.parent = transform;
        phElement.AddComponent(childType);
        if (secondaryChildType == null) return;
        phElement.AddComponent(secondaryChildType);
    }

    [ContextMenu("Export XML Parameters")]
    public void ExportXml(string path)
    {
        //var xml = new gUtility.CXml(path, true);
        var x = new StringBuilder();
        const string t = "    ";
        var engine = transform.GetComponentInChildren<GR_PhEngine>();
        x.AppendLine("<xml>");
        x.AppendLine(string.Format("{0}<Engine", t));
        x.AppendLine(string.Format("{0}{0}position=\"{1}\"", t, GetVector3(engine.transform.position)));
        x.AppendLine(string.Format("{0}{0}rpm=\"{1}, {2}, {3}, {4}\"", t, engine.StallRpm, engine.StartRpm, engine.PeakEngineRpm, engine.RpmLimit));
        x.AppendLine(string.Format("{0}{0}limiter=\"{1}\"", t, engine.RpmLimiter));
        x.AppendLine(string.Format("{0}{0}limiterEngage=\"{1}, {2}\"", t, engine.RpmLimterEngage, engine.CutoffTime));
        x.AppendLine(string.Format("{0}{0}rpmUpshift=\"{1}\"", t, GetVector2(engine.RpmUpshiftMax)));
        x.AppendLine(string.Format("{0}{0}rpmDownshift=\"{1}\"", t, GetVector2(engine.RpmDownshiftMax)));
        x.AppendLine(string.Format("{0}{0}inertia=\"{1}\"", t, engine.Inertia));
        x.AppendLine(string.Format("{0}{0}idle=\"{1}\">", t, engine.Idle));
        x.AppendLine(string.Format("{0}{0}<Torque friction=\"{1}\">", t, engine.TorqueFriction));
        int it = 0;
        var maxRpm = engine.Torque.keys[engine.Torque.length - 1].value;
        foreach (var item in engine.Torque.keys)
        {
            x.AppendLine(string.Format("{0}{0}{0}<Point value=\"{1}, {2}\"/>", t, item.time, item.value));
            it++;
        }
        x.AppendLine(string.Format("{0}{0}</Torque>", t));
        // Turbo
        x.AppendLine(string.Format("{0}{0}<Turbo", t));
        x.AppendLine(string.Format("{0}{0}{0}maxBoost=\"{1}\"", t, engine.MaxBoost));
        x.AppendLine(string.Format("{0}{0}{0}lag=\"{1}, {2}\"", t, engine.LagPower, engine.LagCoaster));
        x.AppendLine(string.Format("{0}{0}{0}wastegate=\"{1}\"", t, engine.Wastegate));
        x.AppendLine(string.Format("{0}{0}{0}rpmMax=\"{1}\">", t, engine.RpmTurboMax));
        x.AppendLine(string.Format("{0}{0}</Turbo>", t));
        x.AppendLine(string.Format("{0}</Engine>", t));


        // clutch
        var clutch = transform.GetComponentInChildren<GR_PhClutch>();
        x.AppendLine(string.Format("{0}<Clutch", t));
        x.AppendLine(string.Format("{0}{0}torque=\"{1}\">", t, clutch.Torque));
        x.AppendLine(string.Format("{0}</Clutch>", t));

        // gearbox
        var gearbox = transform.GetComponentInChildren<GR_PhTransmission>();
        x.AppendLine(string.Format("{0}<Gearbox", t));
        x.AppendLine(string.Format("{0}{0}gears=\"{1}\"", t, gearbox.Gears));
        x.AppendLine(string.Format("{0}{0}ratioReverse=\"{1}\"", t, gearbox.RatioRear));
        for (int i = 0; i < gearbox.Gears; i++)
        {
            x.AppendLine(string.Format("{0}{0}ratio{1}=\"{2}\"", t, i + 1, gearbox.Ratio[i]));
        }
        x.AppendLine(string.Format("{0}{0}time=\"{1}\">", t, gearbox.ShiftTime));
        x.AppendLine(string.Format("{0}</Gearbox>", t));

        // differentals
        x.AppendLine(string.Format("{0}<Differentials>", t));
        var differential = transform.GetComponentInChildren<GR_PhDifferential>();

        var front = differential.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.FWD;
        var rear = differential.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.RWD;

        if (front)
        {
            x.AppendLine(string.Format("{0}{0}<Front", t));
            x.AppendLine(string.Format("{0}{0}{0}type=\"{1}\"", t, (int)differential.Front.Type));
            if (differential.Front.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.TORSEN)
            {
                x.AppendLine(string.Format("{0}{0}{0}tbr=\"{1}\"", t, differential.Front.Tbr));
            }
            x.AppendLine(string.Format("{0}{0}{0}final=\"{1}\">", t, differential.Front.FinalDrive));
            //x.AppendLine(string.Format("{0}{0}{0}antiSlipTorque=\"{1}\">S", t, differential.Front.AntiSlipTorque));
            //x.AppendLine(string.Format("{0}{0}{0}decelerationFactor=\"{1}\"", t, differential.Front.DecelerationFactor));
            //x.AppendLine(string.Format("{0}{0}{0}split=\"{1}\">", t, differential.Front.Split * 0.01f));
            x.AppendLine(string.Format("{0}{0}</Front>", t));
        }
        if (rear)
        {
            x.AppendLine(string.Format("{0}{0}<Rear", t));
            x.AppendLine(string.Format("{0}{0}{0}type=\"{1}\"", t, (int)differential.Rear.Type));
            if (differential.Rear.Type == GR_PhDifferential.DIFFERENTIAL_TYPE.TORSEN)
            {
                x.AppendLine(string.Format("{0}{0}{0}tbr=\"{1}\"", t, differential.Rear.Tbr));
            }
            x.AppendLine(string.Format("{0}{0}{0}final=\"{1}\">", t, differential.Rear.FinalDrive));
            //x.AppendLine(string.Format("{0}{0}{0}antiSlipTorque=\"{1}\">", t, differential.Rear.AntiSlipTorque));
            //x.AppendLine(string.Format("{0}{0}{0}decelerationFactor=\"{1}\"", t, differential.Rear.DecelerationFactor));
            //x.AppendLine(string.Format("{0}{0}{0}split=\"{1}\">", t, differential.Rear.Split * 0.01f));
            x.AppendLine(string.Format("{0}{0}</Rear>", t));
        }
        x.AppendLine(string.Format("{0}</Differentials>", t));
        
        // fuel tank
        var tank = transform.GetComponentInChildren<GR_PhFuel>();
        x.AppendLine(string.Format("{0}<Tank", t));
        x.AppendLine(string.Format("{0}{0}position=\"{1}\"", t, GetVector3(tank.transform.position)));
        x.AppendLine(string.Format("{0}{0}capacity=\"{1}\">", t, tank.Capacity));
        x.AppendLine(string.Format("{0}</Tank>", t));

        // Aero
        var drags = transform.GetComponentsInChildren<GR_PhDrag>();
        var wings = transform.GetComponentsInChildren<GR_PhWing>();
        x.AppendLine(string.Format("{0}<Aerodynamics>", t));
        foreach (var item in drags)
        {
            x.AppendLine(string.Format("{0}{0}<Drag", t));
            x.AppendLine(string.Format("{0}{0}{0}position=\"{1}\"", t, GetVector3(item.transform.position)));
            x.AppendLine(string.Format("{0}{0}{0}area=\"{1}\"", t, item.Area));
            x.AppendLine(string.Format("{0}{0}{0}k=\"{1}\">", t, item.K));
            x.AppendLine(string.Format("{0}{0}</Drag>", t));
        }

        foreach (var item in wings)
        {
            x.AppendLine(string.Format("{0}{0}<Wing", t));
            x.AppendLine(string.Format("{0}{0}{0}position=\"{1}\"", t, GetVector3(item.transform.position)));
            x.AppendLine(string.Format("{0}{0}{0}area=\"{1}\"", t, item.Area));
            x.AppendLine(string.Format("{0}{0}{0}downforceK=\"{1}\"", t, item.DownforceK));
            x.AppendLine(string.Format("{0}{0}{0}drag=\"{1}\">", t, item.Drag));
            x.AppendLine(string.Format("{0}{0}</Wing>", t));
        }
        x.AppendLine(string.Format("{0}</Aerodynamics>", t));

        // steering
        var steer = transform.GetComponentInChildren<GR_PhSteering>();
        x.AppendLine(string.Format("{0}<Steering", t));
        x.AppendLine(string.Format("{0}{0}lockToLock=\"{1}\"", t, steer.LockToLock));
        x.AppendLine(string.Format("{0}{0}wheelRange=\"{1}\"", t, steer.WheelRange));
        x.AppendLine(string.Format("{0}{0}ackermann=\"{1}\">", t, steer.Ackermann));
        x.AppendLine(string.Format("{0}</Steering>", t));

        var wheel = transform.GetComponentsInChildren<GR_PhWheel>();
        // suspensions
        x.AppendLine(string.Format("{0}<Suspensions>", t));
        foreach (var item in wheel)
        {
            if (item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT || item.WheelPosition == gPhys.WHEEL_POSITION.REAR_LEFT)
            {
                x.AppendLine(item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT ? string.Format("{0}{0}<Front", t) : string.Format("{0}{0}<Rear", t));

                x.AppendLine(string.Format("{0}{0}{0}antiRoll=\"{1}\"", t, item.AntiRoll));
                x.AppendLine(string.Format("{0}{0}{0}spring=\"{1}\"", t, item.SpringConstant));
                x.AppendLine(string.Format("{0}{0}{0}damper=\"{1}, {2}\"", t, item.Bounce, item.Rebound));
                x.AppendLine(string.Format("{0}{0}{0}travel=\"{1}\"", t, item.Travel));
                x.AppendLine(string.Format("{0}{0}{0}rideHeight=\"0.0\">", t));
                if (item.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.MACPHERSON)
                {
                    x.AppendLine(string.Format("{0}{0}{0}<MacPherson", t));
                    x.AppendLine(string.Format("{0}{0}{0}{0}top=\"{1}\"", t, GetVector3(item.StrutTop)));
                    x.AppendLine(string.Format("{0}{0}{0}{0}bottom=\"{1}\"", t, GetVector3(item.StrutEnd)));
                    x.AppendLine(string.Format("{0}{0}{0}{0}hinge=\"{1}\"", t, GetVector3(item.StrutHinge)));
                    x.AppendLine(string.Format("{0}{0}{0}{0}length=\"{1}\"", t, item.StrutLen));
                    x.AppendLine(string.Format("{0}{0}{0}{0}hubHeight=\"{1}\"", t, item.HubHeight));
                    x.AppendLine(string.Format("{0}{0}{0}{0}hubWidth=\"{1}\"", t, item.HubWidth));
                    x.AppendLine(string.Format("{0}{0}{0}{0}minSuspLength=\"{1}\">", t, item.MinSuspLen));
                    x.AppendLine(string.Format("{0}{0}{0}</MacPherson>", t));
                }
                else
                {
                    x.AppendLine(string.Format("{0}{0}{0}<Simple", t));
                    x.AppendLine(string.Format("{0}{0}{0}{0}position=\"{1}\"", t, GetVector3(item.WheelPos)));
                    x.AppendLine(string.Format("{0}{0}{0}{0}hinge=\"{1}\"", t, GetVector3(item.WheelHinge)));
                    x.AppendLine(string.Format("{0}{0}{0}{0}chassis=\"{1}\">", t, GetVector3(item.WheelChassis)));
                    x.AppendLine(string.Format("{0}{0}{0}</Simple>", t));
                }

                x.AppendLine(item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT ? string.Format("{0}{0}</Front>", t) : string.Format("{0}{0}</Rear>", t));
            }
        }
        x.AppendLine(string.Format("{0}</Suspensions>", t));

        // wheels
        x.AppendLine(string.Format("{0}<Wheels>", t));
        foreach (var item in wheel)
        {
            if (item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT || item.WheelPosition == gPhys.WHEEL_POSITION.REAR_LEFT)
            {
                x.AppendLine(item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT ? string.Format("{0}{0}<Front", t) : string.Format("{0}{0}<Rear", t));

                x.AppendLine(string.Format("{0}{0}{0}position=\"{1}\"", t, GetVector3(item.gameObject.transform.position)));
                x.AppendLine(string.Format("{0}{0}{0}camber=\"{1}\"", t, item.Camber));
                x.AppendLine(string.Format("{0}{0}{0}camberSetup=\"0.0\"", t));
                x.AppendLine(string.Format("{0}{0}{0}caster=\"{1}\"", t, item.Caster));
                x.AppendLine(string.Format("{0}{0}{0}toe=\"{1}\">", t, item.Toe));

                x.AppendLine(string.Format("{0}{0}{0}<Tyre", t));
                x.AppendLine(string.Format("{0}{0}{0}{0}size=\"{1}, {2}, {3}\"", t, Convert.ToInt32(item.WheelSize.x), Convert.ToInt32(item.WheelSize.z), Convert.ToInt32(item.WheelSize.y)));
                x.AppendLine(string.Format("{0}{0}{0}{0}type=\"tyres/SavunGrip\"", t));
                x.AppendLine(string.Format("{0}{0}{0}{0}pressure=\"{1}\">", t, item.NominalPressure));
                x.AppendLine(string.Format("{0}{0}{0}</Tyre>", t));

                x.AppendLine(item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT ? string.Format("{0}{0}</Front>", t) : string.Format("{0}{0}</Rear>", t));
            }
        }
        x.AppendLine(string.Format("{0}</Wheels>", t));

        // brakes
        var brake = transform.GetComponentInChildren<GR_PhBrake>();
        x.AppendLine(string.Format("{0}<Brakes>", t));
        x.AppendLine(string.Format("{0}{0}<Front", t));
        x.AppendLine(string.Format("{0}{0}{0}repartition=\"{1}\"", t, brake.FrontRepartition * 0.01f));
        x.AppendLine(string.Format("{0}{0}{0}torque=\"{1}\">", t, brake.FrontTorque));
        x.AppendLine(string.Format("{0}{0}</Front>", t));
        x.AppendLine(string.Format("{0}{0}<Rear", t));
        x.AppendLine(string.Format("{0}{0}{0}handBrake=\"{1}\"", t, brake.HandBrake));
        x.AppendLine(string.Format("{0}{0}{0}torque=\"{1}\">", t, brake.RearTorque));
        x.AppendLine(string.Format("{0}{0}</Rear>", t));
        x.AppendLine(string.Format("{0}</Brakes>", t));

        // body
        var tensor = transform.GetComponentInChildren<GR_PhInertiaTensor>();
        var cog = transform.GetComponentInChildren<GR_PhCog>();
        x.AppendLine(string.Format("{0}<Body", t));
        x.AppendLine(string.Format("{0}{0}mass=\"{1}\"", t, tensor.Mass));
        x.AppendLine(string.Format("{0}{0}inertiaTensor=\"{1}\"", t, GetVector3(tensor.transform.localScale)));
        x.AppendLine(string.Format("{0}{0}cog=\"{1}\"", t, GetVector3(-cog.transform.position)));
        x.AppendLine(string.Format("{0}{0}center=\"{1}\">", t, GetVector3(cog.transform.position)));
        x.AppendLine(string.Format("{0}</Body>", t));

        // footer
        x.AppendLine("</xml>");
        File.WriteAllText(path, x.ToString());
    }

    public bool RaiseFront(float value)
    {
        int count = 0;
        var raise = new Vector3(0.0f, value, 0.0f);

        var wheel = transform.GetComponentsInChildren<GR_PhWheel>();
        foreach (var item in wheel)
        {
            if (item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT || item.WheelPosition == gPhys.WHEEL_POSITION.FRONT_RIGHT)
            {
                // ok!
                if (item.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.MACPHERSON)
                {
                    item.StrutTop += raise;
                    item.StrutHinge += raise;
                    count ++;
                }
                else if (item.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.SIMPLE)
                {
                    item.WheelHinge += raise;
                    item.WheelChassis += raise;
                    item.WheelPos += raise;
                    count ++;
                }
            }
        }
        return count == 2 && value != 0.0f;
    }

    public bool RaiseRear(float value)
    {
        int count = 0;
        var raise = new Vector3(0.0f, value, 0.0f);

        var wheel = transform.GetComponentsInChildren<GR_PhWheel>();
        foreach (var item in wheel)
        {
            if (item.WheelPosition == gPhys.WHEEL_POSITION.REAR_LEFT || item.WheelPosition == gPhys.WHEEL_POSITION.REAR_RIGHT)
            {
                // ok!
                if (item.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.MACPHERSON)
                {
                    item.StrutTop += raise;
                    item.StrutHinge += raise;
                    count ++;
                }
                else if (item.SuspensionType == GR_PhWheel.SUSPENSION_TYPE.SIMPLE)
                {
                    item.WheelHinge += raise;
                    item.WheelChassis += raise;
                    item.WheelPos += raise;
                    count ++;
                }
            }
        }
        return count == 2 && value != 0.0f;
    }

#endif // UNITY_EDITOR

    private string GetVector3I(Vector3 vector)
    {
        return string.Format("{0}, {1}, {2}", vector.x, vector.z, vector.y);
    }

    private string GetVector3(Vector3 vector)
    {
        return string.Format("{0}, {1}, {2}", vector.x, vector.y, vector.z);
    }

    private string GetVector2(Vector2 vector)
    {
        return string.Format("{0}, {1}", vector.x, vector.y);
    }
}
