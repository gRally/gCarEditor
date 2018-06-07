using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class LightSet
{
    public Material material;
    public Color emissionColorOn;
    public Color emissionColorOff;
    public float emissionOn;
    public float emissionOff;

    public LightSet()
    {
        emissionColorOn = Color.white;
        emissionColorOff = Color.black;

        emissionOn = 1.0f;
        emissionOff = 0.0f;
    }

    public void On()
    {
        material.SetColor("_EmissionColor", emissionColorOn * emissionOn);
        material.EnableKeyword("_EMISSION");
    }

    public void Off()
    {
        material.SetColor("_EmissionColor", emissionColorOff * emissionOff);
        if (emissionOff > 0.0f)
        {
            material.EnableKeyword("_EMISSION");
        }
        else
        {
            material.DisableKeyword("_EMISSION");
        }
    }

    public void Update(bool on)
    {
        if (on)
        {
            On();
        }
        else
        {
            Off();
        }
    }
}

[ExecuteInEditMode]
public class RallyCar : MonoBehaviour
{
    private float K_EMERGENCY = 0.75f;
    
    public bool DebugLights;
    public bool DebugBrakes;
    public bool DebugEmergency;
    public bool DebugReverse;
    
    bool fDebugLights;
    bool fDebugBrakes;
    bool fDebugEmergency;
    bool fDebugReverse;
    
    public Light[] frontLights = new Light[6];
    public Light[] rearLights = new Light[6];
    public Light[] rearBrakeLights = new Light[6];
    public Light[] reverseLights = new Light[6];

    public LightSet[] LightsMaterial;
    public LightSet[] CockpitMaterial;
    //public Material DashMaterial;
    public LightSet[] BrakeMaterial;
    public LightSet[] EmergencyMaterial;
    public LightSet[] ReverseMaterial;
    
    bool emergency = false;
    bool emergencyOn = false;
    float emergencyTimer = 1.0f;

    // Use this for initialization
    void Start ()
    {
        foreach (var item in frontLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }

        foreach (var item in rearLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }

        foreach (var item in reverseLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }

        foreach (var item in rearBrakeLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(false);
            }
        }
        /*
        LightsMaterial.shader = Shader.Find("Standard");
        CockpitMaterial.shader = Shader.Find("Standard");
        DashMaterial.shader = Shader.Find("Standard");
        BrakeMaterial.shader = Shader.Find("Standard");
        EmergencyMaterial.shader = Shader.Find("Standard");
        ReverseMaterial.shader = Shader.Find("Standard");
        */
    }

#if UNITY_EDITOR
    // Update is called once per frame
    void Update ()
    {
        if (fDebugBrakes != DebugBrakes)
        {
            BrakeLights(DebugBrakes);
            fDebugBrakes = DebugBrakes;
        }
        
        if (fDebugLights != DebugLights)
        {
            Lights(DebugLights);
            fDebugLights = DebugLights;
        }
        
        if (fDebugEmergency != DebugEmergency)
        {
            EmergencyLights(DebugEmergency);
            fDebugEmergency = DebugEmergency;
        }
        
        if (fDebugReverse != DebugReverse)
        {
            ReverseLights(DebugReverse);
            fDebugReverse = DebugReverse;
        }
        
        if (emergency)
        {
            if (emergencyTimer <= 0.0f)
            {
                emergencyTimer = K_EMERGENCY;
                emergencyOn = !emergencyOn;
                emergencyLightsTick(emergencyOn);
            }
            emergencyTimer -= Time.deltaTime;
        }
    }
#endif

    public void Lights(bool lightsOn)
    {
        foreach (var item in frontLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(lightsOn);
            }
        }

        foreach (var item in rearLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(lightsOn);
            }
        }

        foreach (var item in LightsMaterial)
        {
            item.Update(lightsOn);
        }

        foreach (var item in CockpitMaterial)
        {
            item.Update(lightsOn);
        }
    }
    
    public void EmergencyLights(bool lightsOn)
    {
        emergencyTimer = K_EMERGENCY;
        emergency = lightsOn;
    }
    
    private void emergencyLightsTick(bool lightsOn)
    {
        foreach (var item in EmergencyMaterial)
        {
            item.Update(lightsOn);
        }
    }
    
    public void ReverseLights(bool lightsOn)
    {
        foreach (var item in reverseLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(lightsOn);
            }
        }

        foreach (var item in ReverseMaterial)
        {
            item.Update(lightsOn);
        }
    }
    
    public void BrakeLights(bool brakeOn)
    {
        foreach (var item in rearBrakeLights)
        {
            if (item != null)
            {
                item.gameObject.SetActive(brakeOn);
            }
        }
        foreach (var item in BrakeMaterial)
        {
            item.Update(brakeOn);
        }
    }
}