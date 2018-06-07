using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GR_PhCog : MonoBehaviour
{
    [ShowOnly] public float WheelBase;
    [ShowOnly] public float W;
    [ShowOnly] public float Rf;
    [ShowOnly] public float Rr;
    [ShowOnly] public float a;
    [ShowOnly] public float b;

    [Header("Forces")]
    public float FrontForce;
    public float RearForce;
    public float FrontPercentage;

    [Header("Front"), TextArea(5,30)]
    public string FrontInfo;

    [Header("Rear"), TextArea(5, 30)]
    public string RearInfo;

    public void Update()
    {
        var wheels = FindObjectsOfType<GR_PhWheel>();
        var front = 0.0f;
        var rear = 0.0f;
        var frontSpring = 0.0f;
        var rearSpring = 0.0f;
        var frontBounce = 0.0f;
        var frontRebound = 0.0f;
        var rearBounce = 0.0f;
        var rearRebound = 0.0f;
        foreach (var wheel in wheels)
        {
            if (wheel.WheelPosition == gPhys.WHEEL_POSITION.FRONT_LEFT)
            {
                front = wheel.transform.position.z;
                frontSpring = wheel.SpringConstant * 1000.0f;
                frontBounce = wheel.Bounce * 1000.0f;
                frontRebound = wheel.Rebound * 1000.0f;
                a = wheel.transform.position.z - transform.position.z;
            }
            if (wheel.WheelPosition == gPhys.WHEEL_POSITION.REAR_LEFT)
            {
                
                rear = wheel.transform.position.z;
                rearSpring = wheel.SpringConstant * 1000.0f;
                rearBounce = wheel.Bounce * 1000.0f;
                rearRebound = wheel.Rebound * 1000.0f;
                b = transform.position.z - wheel.transform.position.z;
            }
        }
        WheelBase = front - rear;
        
        W = FindObjectOfType<GR_PhInertiaTensor>().Mass;

        Rr = W * a / WheelBase;
        Rf = W * b / WheelBase;
        FrontForce = Rf * 0.5f;
        RearForce = Rr * 0.5f;

        FrontPercentage = 100.0f / W * Rf;

        FrontInfo = gPhys.Suspensions.Helper.SuggestSettings(FrontForce, frontSpring, frontBounce, frontRebound);
        RearInfo = gPhys.Suspensions.Helper.SuggestSettings(RearForce, rearSpring, rearBounce, rearRebound);
    }
}
