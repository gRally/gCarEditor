using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GR_PhWheel : MonoBehaviour
{
    //Mesh w;
    public Vector3 WheelSize = new Vector3(195f, 15f, 50f);
    public float NominalPressure;
    public float Camber;
    public float CurrentCamber;
    public float Caster;
    public float Toe;
    public bool DrawWireFrame;
    public gPhys.WHEEL_POSITION WheelPosition;

    // default suspension
    public Vector3 WheelHinge;
    public Vector3 WheelChassis;

    // macpherson suspension
    public Vector3 StrutTop;
    public Vector3 StrutEnd;
    public Vector3 StrutHingeTop;
    public Vector3 StrutHinge;
    public Vector3 WheelPos;

    public Vector3 WheelPosUp;
    public Vector3 WheelPosDn;

    [Range(0.02f, 0.2f)]
    public float HubHeight;
    [Range(0.02f, 0.3f)]
    public float HubWidth;

    // coilover
    public float SpringConstant;
    public float Bounce;
    public float Rebound;
    public float Travel;
    public float MinSuspLen = 0.44f;
    public float AntiRoll;

    // debug
    public float StrutLen = 0.3f;
    //[Range(0.0f, 0.4f)]
    public float DistanceTraveled;
    
    Vector3 wheelPosFullCompress;
    Vector3 debugWheelPos;
    public Vector3 debugCenterRadius;
    public Vector3 Opos;
    Quaternion mountrot = Quaternion.identity;

    private gPhys.Suspensions.Suspension susp = new gPhys.Suspensions.Suspension();
    private gPhys.Wheel.Gizmo gizmo = new gPhys.Wheel.Gizmo();

    public enum SUSPENSION_TYPE
    {
        SIMPLE,
        MACPHERSON
    }

    public SUSPENSION_TYPE SuspensionType;

    // Use this for initialization
	void Start ()
    {
        gizmo.SetCommercialSize(WheelSize);
        gizmo.Create();
        //CreateGizmoWheel();
	}
	
	// Update is called once per frame
	void Update ()
    {
        gizmo.SetCommercialSize(WheelSize);

        float displacement = DistanceTraveled;
        if (displacement > Travel)
        {
            displacement = Travel;
        }
        if (displacement < 0)
        {
            displacement = 0;
        }

        switch (SuspensionType)
        {
            case SUSPENSION_TYPE.MACPHERSON:
                susp.CalcMacPherson(WheelPosition, displacement,
                    StrutTop, StrutHinge, StrutLen, Travel, MinSuspLen, HubHeight, HubWidth, Camber, gizmo.GetRadius(),
                    ref StrutEnd, ref StrutHingeTop, ref WheelPos, ref CurrentCamber, ref WheelPosUp, ref WheelPosDn, ref debugCenterRadius, ref angleAtZero, ref quatCamber);
                break;
            case SUSPENSION_TYPE.SIMPLE:
                susp.CalcSimple(displacement, WheelChassis, WheelHinge, 
                    ref quatCamber, ref WheelPos);
                break;
        }
    }

    private float angleAtZero;
    private Quaternion quatCamber;
    
    public float GetWidth()
    {
        return gizmo.GetWidth();
    }

    public float GetRadius()
    {
        return gizmo.GetRadius();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(0.886f, 0.149f, 0.204f);

        if (DrawWireFrame)
        {
            gizmo.SetCommercialSize(WheelSize);
            gizmo.Draw(WheelPos, quatCamber);
        }
    }
}
