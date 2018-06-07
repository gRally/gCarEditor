using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class Dash : MonoBehaviour
{
    public enum DashType
    {
        Rpm,
        Kmh,
        Fuel,
        OilTemp,
        WaterTemp,
        Turbo
    }

    public bool SetZeroAngle;
    private bool fSetZeroAngle;

    public float MaxValue;
    public float MaxDegree;
    public DashType Type;

    public float Value;

    private float zeroAngle = 0.0f;
    //private Vector3 zeroPos;
    private Quaternion newRotation = Quaternion.identity;

    void Update()
    {
        if (SetZeroAngle)
        {
            // nothing, do the zero value
        }
        else
        {
            if (fSetZeroAngle)
            {
                zeroAngle = transform.rotation.eulerAngles.z;
            }
            else
            {
                float rot = zeroAngle + Value * (MaxDegree - zeroAngle) / MaxValue;
                newRotation.eulerAngles = new Vector3(0, 0, rot);
                transform.rotation = newRotation;
            }
        }

        fSetZeroAngle = SetZeroAngle;
    }
}
