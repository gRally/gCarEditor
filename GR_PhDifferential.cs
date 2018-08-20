using UnityEngine;
using System.Collections;

public class GR_PhDifferential : MonoBehaviour
{
    public enum DIFFERENTIAL_POSITION
    {
        DIFFERENTIAL_FRONT,
        DIFFERENTIAL_CENTRAL,
        DIFFERENTIAL_REAR
    }

    public enum DIFFERENTIAL_TYPE
    {
        LOCKED,
        OPEN,
        TORSEN,
        LSD_1_WAY,
        LSD_2_WAY,
        LSD_15_WAY,
        SPEED
    }

    public enum TRANSMISSION_TYPE
    {
        FWD,
        RWD,
        AWD
    }

    public TRANSMISSION_TYPE TransmissionType;

    [Header("Front")]
    [Space(10)]
    public Differential Front;

    [Header("Center")]
    [Space(10)]
    public Differential Central;

    [Header("Rear")]
    [Space(10)]
    public Differential Rear;
}

[System.Serializable]
public class Differential
{
    [Tooltip("Additional gear reduction.")]
    public float FinalDrive;
    [Tooltip("Maximum anti slip Torque (or radian speed)")]
    public float AntiSlip;
    public GR_PhDifferential.DIFFERENTIAL_TYPE Type; 
    [Range(1, 6)]
    public int Tbr = 3;
    [ShowOnly]
    public string LockedPercent;
    [Range(0, 100), Tooltip("Front/rear repartition percentage (0 => full front)")]
    public float Split = 50.0f;
}
