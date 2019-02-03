using UnityEngine;
using System.Collections;

public class GR_PhBrake : MonoBehaviour
{
    [Header("Front")]
    public float FrontTorque;
    public float FrontRepartition;
    public float FrontDiscDiameter;
    [Tooltip("0 down, 90 front")]
    public float FrontCaliperAngle;

    [Header("Rear")]
    public float RearTorque;
    public float HandBrake;
    public float RearDiscDiameter;
    [Tooltip("0 down, 90 front")]
    public float RearCaliperAngle;
}
