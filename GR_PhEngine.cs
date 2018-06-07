using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Collections;

[ExecuteInEditMode]
public class GR_PhEngine : MonoBehaviour
{
    //[Header("Engine")]
    public float PeakEngineRpm;
    public float RpmLimit;
    public bool RpmLimiter = false;
    public float RpmLimterEngage;
    [Range(0.0f, 1.0f)]
    public float CutoffTime;
    public Vector2 RpmUpshiftMax;
    public Vector2 RpmDownshiftMax;
    [Space(10)]
    public float Inertia;
    public float Idle;
    public float StartRpm;
    public float StallRpm;
    [Space(10)]
    public float TorqueFriction;
    public AnimationCurve Torque = new AnimationCurve();
    [Space(10)]
    [ShowOnly]
    public string MaxTorque;
    [ShowOnly]
    public string MaxPower;
    [HideInInspector]
    public float MaxPowerHp;

    void Update()
    {
        var maxTorque = 0.0f;
        var maxTorqueRpm = 0.0f;
        var maxHp = 0.0f;
        var maxHpRpm = 0.0f;
        var step = 5.0f;
        for (float i = 0.0f; i < RpmLimit; i+= step)
        {
            var eval = Torque.Evaluate(i);
            var evalHp = i * eval / 7121.0f;

            if (eval > maxTorque)
            {
                maxTorque = eval;
                maxTorqueRpm = i;
            }

            if (evalHp > maxHp)
            {
                maxHp = evalHp;
                maxHpRpm = i;
            }
        }

        MaxTorque = string.Format("{0:0.0}Nm @ {1:0}rpm", maxTorque, maxTorqueRpm);
        MaxPower = string.Format("{0:0.0}Hp @ {1:0}rpm", maxHp, maxHpRpm);
        MaxPowerHp = maxHp;
    }
}
