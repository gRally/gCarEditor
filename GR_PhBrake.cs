using UnityEngine;
using System.Collections;

public class GR_PhBrake : MonoBehaviour
{
    [Header("Front")]
    public float FrontTorque;
    [Space(5)]
    public float FrontRepartition;

    [Header("Rear")]
    [Space(10)]
    public float RearTorque;
    [Space(5)]
    public float HandBrake;
}
