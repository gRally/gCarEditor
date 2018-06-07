using UnityEngine;
using System.Collections;
using System.Globalization;

[ExecuteInEditMode]
public class GR_PhTransmission : MonoBehaviour
{
    public int Gears;
    public float ShiftTime;
    public float RatioRear;
    public float[] Ratio = new float[10];
    [Space(10)]
    [ShowOnly]
    public string[] MaxSpeed = new string[10];
    [ShowOnly]
    public string MaxSpeedCalc;

    void Update()
    {
        var engine = FindObjectOfType<GR_PhEngine>();
        var diff = FindObjectOfType<GR_PhDifferential>();
        var wheels = FindObjectsOfType<GR_PhWheel>();
        var drag = FindObjectOfType<GR_PhDrag>();

        var radius = 0.0f;
        var finalDrive = 0.0f;
        if (diff.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.FWD)
        {
            finalDrive = diff.Front.FinalDrive;
            foreach (var wheel in wheels)
            {
                if (wheel.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT)
                {
                    radius = wheel.GetRadius();
                }
            }
        }
        else if (diff.TransmissionType == GR_PhDifferential.TRANSMISSION_TYPE.RWD)
        {
            finalDrive = diff.Rear.FinalDrive;
            foreach (var wheel in wheels)
            {
                if (wheel.WheelPosition == gPhys.WHEEL_POSITION.REAR_LEFT)
                {
                    radius = wheel.GetRadius();
                }
            }
        }

        var ratio = Ratio[Gears - 1] * finalDrive;
        var rpm = engine.RpmLimit;
        if (engine.RpmLimiter)
        {
            rpm = engine.RpmLimterEngage;
        }

        for (int i = 0; i < Gears; i++)
        {
            var speed = radius * rpm * Mathf.PI / 30.0f / (Ratio[i] * finalDrive);
            MaxSpeed[i] = string.Format("{0:0.00} Km/h with gear {1}", speed * 3.6f, i + 1);
        }

        // speed calc
        var P = engine.MaxPowerHp * 735.499f;
        var c = drag.K;
        var D = 1.25f;
        var A = drag.Area;
        var speedCalc = Mathf.Pow((2.0f * P / ( c * D * A)), 1.0f / 3.0f);
        MaxSpeedCalc = string.Format("{0:0.00} Km/h", speedCalc * 3.6f);
    }
}
