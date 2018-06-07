using UnityEngine;
using System.Collections;

public class GR_PhDifferential : MonoBehaviour
{
    public enum DIFFERENTIAL_POSITION
    {
        DIFFERENTIAL_FRONT,
        DIFFERENTIAL_REAR
    }

    public enum DIFFERENTIAL_TYPE
    {
        LOCKED,
        OPEN,
        TORSEN
    }

    public enum TRANSMISSION_TYPE
    {
        FWD,
        RWD
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
    [Tooltip("Maximum anti slip Torque")]
    public GR_PhDifferential.DIFFERENTIAL_TYPE Type; 
    [Range(1, 6)]
    public int Tbr = 3;
     [ShowOnly]
    public string LockedPercent;
}
