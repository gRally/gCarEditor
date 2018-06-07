using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

[ExecuteInEditMode]
public class CarSound : MonoBehaviour
{
#if UNITY_EDITOR
    public string DebugIdle;
    public string DebugPower;
    public string DebugCoaster;
    public string DebugRev;
    public string DebugLimit;

    public bool EnableIdle;
    public bool EnablePower;
    public bool EnableCoaster;
    public bool EnableRev;
    public bool EnableLimit;
#endif

    GameObject enginePosition;
    public CarSoundItem[] ExtPowerSnd;
    public CarSoundItem[] ExtCoasterSnd;
    public CarSoundItem[] IntPowerSnd;
    public CarSoundItem[] IntCoasterSnd;
    public CarSoundItem ExtIdleSnd;
    public CarSoundItem IntIdleSnd;
    public CarSoundItem ExtReverseSnd;
    public CarSoundItem IntReverseSnd;
    public CarSoundItem ExtLimiterSnd;
    public CarSoundItem IntLimiterSnd;

    public AudioClip ShiftEngageAudio;
    public AudioClip ShiftUpAudio;
    public AudioClip ShiftDnAudio;
    public AudioClip BrakeAudio;
    [Range(0.1f, 2.0f)]
    public float pitchCorrection = 1.0f;
    public float MainVolume = 0.70f;
    float revVolume = 1.0f;
    float fwdVolume = 1.0f;
    // audio source
    AudioSource[] extPowerSrc;
    AudioSource[] extCoasterSrc;
    AudioSource[] intPowerSrc;
    AudioSource[] intCoasterSrc;
    AudioSource extIdleSrc;
    AudioSource intIdleSrc;
    AudioSource extReverseSrc;
    AudioSource intReverseSrc;
    AudioSource extLimiterSrc;
    AudioSource intLimiterSrc;

    // debug
    public float throttleVolume;
    public float coasterVolume;
    public float rpm;
    public string throttleStr;
    public string coasterStr;
    public int fakeRpm;
    public bool Throttle;
    
    int lastGear;
    
    // debug
    public bool debugSound;
    public bool debugPower;
    public int debugRpm;
    public int debugMinRpm;
    public int debugMaxRpm;
    int fFakeRpm;

    public float ThrottleValue = 1.0f;
    private float clipsValue;

    private gPhys.Sound.Helper helper;

    // Use this for initialization
    void Start()
    {
        helper = new gPhys.Sound.Helper();

        Vector3 enginePositionV = Vector3.zero;
        if (enginePosition!=null)
        {
            enginePositionV = enginePosition.transform.position;
        }

        if (ExtCoasterSnd == null) ExtCoasterSnd = new CarSoundItem[0];
        if (IntCoasterSnd == null) IntCoasterSnd = new CarSoundItem[0];
        if (ExtPowerSnd == null) ExtPowerSnd = new CarSoundItem[0];
        if (IntPowerSnd == null) IntPowerSnd = new CarSoundItem[0];

        // ext
        // power
        System.Array.Resize(ref extPowerSrc, ExtPowerSnd.Length);
        for (int i = 0; i < ExtPowerSnd.Length; i++)
        {
            extPowerSrc[i] = CreateAudioSource(ExtPowerSnd[i].Audio, true, true, enginePositionV);
        }
        // coaster
        System.Array.Resize(ref extCoasterSrc, ExtCoasterSnd.Length);
        for (int i = 0; i < ExtCoasterSnd.Length; i++)
        {
            extCoasterSrc[i] = CreateAudioSource(ExtCoasterSnd[i].Audio, true, true, enginePositionV);
        }
        // idle
        if(ExtIdleSnd != null) extIdleSrc = CreateAudioSource(ExtIdleSnd.Audio, true, true, enginePositionV);
        // limiter
        if (ExtLimiterSnd != null) extLimiterSrc = CreateAudioSource(ExtLimiterSnd.Audio, true, true, enginePositionV);
        // reverse
        if (ExtReverseSnd != null) extReverseSrc = CreateAudioSource(ExtReverseSnd.Audio, true, true, enginePositionV);

        // int
        // power
        System.Array.Resize(ref intPowerSrc, IntPowerSnd.Length);
        for (int i = 0; i < IntPowerSnd.Length; i++)
        {
            intPowerSrc[i] = CreateAudioSource(IntPowerSnd[i].Audio, true, true, enginePositionV);
        }
        // coaster
        System.Array.Resize(ref intCoasterSrc, IntCoasterSnd.Length);
        for (int i = 0; i < IntCoasterSnd.Length; i++)
        {
            intCoasterSrc[i] = CreateAudioSource(IntCoasterSnd[i].Audio, true, true, enginePositionV);
        }
        // idle
        if (intIdleSrc != null) intIdleSrc = CreateAudioSource(IntIdleSnd.Audio, true, true, enginePositionV);
        // limiter
        if (intLimiterSrc != null) intLimiterSrc = CreateAudioSource(IntLimiterSnd.Audio, true, true, enginePositionV);
        // reverse
        if (intReverseSrc != null) intReverseSrc = CreateAudioSource(IntReverseSnd.Audio, true, true, enginePositionV);
    }
    
    void Update ()
    {
        if (!debugSound)
        {
            muteSound(extCoasterSrc);
            muteSound(extPowerSrc);
            if (extIdleSrc != null) muteSound(extIdleSrc);
            if (extLimiterSrc != null) muteSound(extLimiterSrc);
            if (extReverseSrc != null) muteSound(extReverseSrc);

            muteSound(intCoasterSrc);
            muteSound(intPowerSrc);
            if (intIdleSrc != null) muteSound(intIdleSrc);
            if (intLimiterSrc != null) muteSound(intLimiterSrc);
            if (intReverseSrc != null) muteSound(intReverseSrc);
            return;
        }
        if (fakeRpm > fFakeRpm)
        {
            Throttle = true;
        }
        if (fakeRpm < fFakeRpm)
        {
            Throttle = false;
        }
        fFakeRpm = fakeRpm;

        rpm = Convert.ToSingle(fakeRpm);
        clipsValue = rpm / FindObjectOfType<GR_PhEngine>().RpmLimit;

        if (Throttle)
        {
            coasterVolume = 0.0f;
            throttleVolume = 1.0f;
        }
        else
        {
            coasterVolume = 1.0f;
            throttleVolume = 0.0f;
        }

        bool interior = false;

        helper.Update(interior, pitchCorrection, clipsValue, throttleVolume,
            ref ExtPowerSnd, ref ExtCoasterSnd,
            ref IntPowerSnd, ref IntCoasterSnd,
            ref ExtIdleSnd, ref IntIdleSnd,
            ref ExtReverseSnd, ref IntReverseSnd,
            ref ExtLimiterSnd, ref IntLimiterSnd,
            ref extPowerSrc, ref extCoasterSrc,
            ref intPowerSrc, ref intCoasterSrc,
            ref extIdleSrc, ref intIdleSrc,
            ref extReverseSrc, ref intReverseSrc,
            ref extLimiterSrc, ref intLimiterSrc);

        if (!interior)
        {
#if UNITY_EDITOR
            if (!EnableIdle) extIdleSrc.volume = 0.0f;
            if (!EnableRev) extReverseSrc.volume = 0.0f;
            if (!EnableLimit) extLimiterSrc.volume = 0.0f;

            DebugIdle = string.Format("{0:0.00}", extIdleSrc.volume);
            DebugRev = string.Format("{0:0.00}", extReverseSrc.volume);
            DebugLimit = string.Format("{0:0.00}", extLimiterSrc.volume);

            string tmp = "";
            foreach(var item in extPowerSrc)
            {
                if (!EnablePower) item.volume = 0.0f;
                tmp += string.Format("{0:0.00} ", item.volume);
            }
            DebugPower = tmp;
            tmp = "";
            foreach(var item in extCoasterSrc)
            {
                if (!EnableCoaster) item.volume = 0.0f;
                tmp += string.Format("{0:0.00} ", item.volume);
            }
            DebugCoaster = tmp;
#endif
        }
        else
        {

        } 
    }

    void muteSound(AudioSource[] src)
    {
        for (int i = 0; i < src.Length; i++)
        {
            src[i].volume = 0.0f;
        }
    }

    void muteSound(AudioSource src)
    {
        src.volume = 0.0f;
    }

    public void createSound()
    {
        //SoundItems = new List<CarSoundItem>();	
    }
    
    public void addSound(bool coaster, bool intern, bool reverse, bool idle, bool limiter)
    {
        if (intern)
        {
            // internal
            if (idle) IntIdleSnd = new CarSoundItem();
            else if (reverse) IntReverseSnd = new CarSoundItem();
            else if (limiter) IntLimiterSnd = new CarSoundItem();
            else if (coaster)
            {
                System.Array.Resize(ref IntCoasterSnd, IntCoasterSnd.Length + 1);
                IntCoasterSnd[IntCoasterSnd.Length - 1] = new CarSoundItem();
            }
            else
            {
                System.Array.Resize(ref IntPowerSnd, IntPowerSnd.Length + 1);
                IntPowerSnd[IntPowerSnd.Length - 1] = new CarSoundItem();
            }
        }
        else
        {
            // external
            if (idle) ExtIdleSnd = new CarSoundItem();
            if (reverse) ExtReverseSnd = new CarSoundItem();
            else if (limiter) ExtLimiterSnd = new CarSoundItem();
            else if (coaster)
            {
                System.Array.Resize(ref ExtCoasterSnd, ExtCoasterSnd.Length + 1);
                ExtCoasterSnd[ExtCoasterSnd.Length - 1] = new CarSoundItem();
            }
            else
            {
                System.Array.Resize(ref ExtPowerSnd, ExtPowerSnd.Length + 1);
                ExtPowerSnd[ExtPowerSnd.Length - 1] = new CarSoundItem();
            }
        }
    }
    
    public void removeSound(bool coaster, bool intern, bool reverse, bool idle, bool limiter, int i)
    {
        if (intern)
        {
            // internal
            if (idle) IntIdleSnd = null;
            else if (reverse) IntReverseSnd = null;
            else if (limiter) IntLimiterSnd = null;
            else if (coaster)
            {
                var tmp = IntCoasterSnd.ToList();
                tmp.RemoveAt(i);
                IntCoasterSnd = tmp.ToArray();
            }
            else
            {
                var tmp = IntPowerSnd.ToList();
                tmp.RemoveAt(i);
                IntPowerSnd = tmp.ToArray();
            }
        }
        else
        {
            // external
            if (idle) ExtIdleSnd = null;
            if (reverse) ExtReverseSnd = null;
            else if (limiter) ExtLimiterSnd = null;
            else if (coaster)
            {
                var tmp = ExtCoasterSnd.ToList();
                tmp.RemoveAt(i);
                ExtCoasterSnd = tmp.ToArray();
            }
            else
            {
                var tmp = ExtPowerSnd.ToList();
                tmp.RemoveAt(i);
                ExtPowerSnd = tmp.ToArray();
            }
        }        
    }

    AudioSource CreateAudioSource(AudioClip clip, bool loop, bool playImmediately, Vector3 position)
    {
        GameObject go = new GameObject("audio");
        go.transform.parent = transform;
        go.transform.localPosition = position;
        go.transform.localRotation = Quaternion.identity;
        go.AddComponent(typeof(AudioSource));
        go.GetComponent<AudioSource>().clip = clip;
        go.GetComponent<AudioSource>().playOnAwake = false;
        if (loop==true){
            go.GetComponent<AudioSource>().volume = 0;
            go.GetComponent<AudioSource>().loop = true;
        }
        else 
            go.GetComponent<AudioSource>().loop = false;
        
        if (playImmediately) go.GetComponent<AudioSource>().Play();
        
        return go.GetComponent<AudioSource>();
    }
}

