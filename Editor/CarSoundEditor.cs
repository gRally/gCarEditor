using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using UnityEditor.SceneManagement;

[CustomEditor(typeof(CarSound))]
public class CarSoundEditor : Editor
{
    public void Start()
    {
        CarSound carSound = target as CarSound;
    }

    public override void OnInspectorGUI()
    {
        CarSound carSound = target as CarSound;
        if (carSound.ExtCoasterSnd == null) carSound.ExtCoasterSnd = new CarSoundItem[1];
        if (carSound.IntCoasterSnd == null) carSound.IntCoasterSnd = new CarSoundItem[1];
        if (carSound.ExtPowerSnd == null) carSound.ExtPowerSnd = new CarSoundItem[1];
        if (carSound.IntPowerSnd == null) carSound.IntPowerSnd = new CarSoundItem[1];

        EditorGUILayout.TextField("rpm", carSound.rpm.ToString());
        EditorGUILayout.TextField("throttleVolume", carSound.throttleVolume.ToString());
        EditorGUILayout.TextField("coasterVolume", carSound.coasterVolume.ToString());
        EditorGUILayout.TextField("throttle", carSound.throttleStr);
        EditorGUILayout.TextField("coaster", carSound.coasterStr);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Test Sounds:");
        carSound.debugSound = EditorGUILayout.Toggle(carSound.debugSound);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Test Rpm:");
        carSound.fakeRpm = EditorGUILayout.IntSlider(carSound.fakeRpm , 0, Convert.ToInt32(FindObjectOfType<GR_PhEngine>().RpmLimit));
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Test Throttle:");
        carSound.ThrottleValue = EditorGUILayout.Slider(carSound.ThrottleValue, 0.0f, 1.0f);
        EditorGUILayout.EndHorizontal();

        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Shift Engage Sound:");
        carSound.ShiftEngageAudio = (AudioClip)EditorGUILayout.ObjectField(carSound.ShiftEngageAudio, typeof(AudioClip), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Shift Up Sound:");
        carSound.ShiftUpAudio = (AudioClip)EditorGUILayout.ObjectField(carSound.ShiftUpAudio, typeof(AudioClip), true);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Shift Dn Sound:");
        carSound.ShiftDnAudio = (AudioClip)EditorGUILayout.ObjectField(carSound.ShiftDnAudio, typeof(AudioClip), true);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Brake Sound:");
        carSound.BrakeAudio = (AudioClip)EditorGUILayout.ObjectField(carSound.BrakeAudio, typeof(AudioClip), true);
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch correction:");
        carSound.pitchCorrection = EditorGUILayout.Slider(carSound.pitchCorrection, 0.1f, 2.0f);
        EditorGUILayout.EndHorizontal();

        #region EXTERNAL

        GUILayout.Space(10);
        EditorGUILayout.LabelField("External sound", EditorStyles.boldLabel);

        // POWER ------------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Power: (minRpm|maxRpm|recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, false, false, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < carSound.ExtPowerSnd.Length; i++)
        {
            if (carSound.ExtPowerSnd[i].Volume == null) carSound.ExtPowerSnd[i].Volume = new AnimationCurve();
            if (carSound.ExtPowerSnd[i].Pitch == null) carSound.ExtPowerSnd[i].Pitch = new AnimationCurve();
            EditorGUILayout.BeginHorizontal();
            carSound.ExtPowerSnd[i].Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.ExtPowerSnd[i].Audio, typeof(AudioClip), true);
            GUI.color = new Color32(199,0,2, 255);
            if (GUILayout.Button("x"))
            {
                carSound.removeSound(false, false, false, false, false, i);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Volume:");
            carSound.ExtPowerSnd[i].Volume = EditorGUILayout.CurveField(carSound.ExtPowerSnd[i].Volume);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Pitch:");
            carSound.ExtPowerSnd[i].Pitch = EditorGUILayout.CurveField(carSound.ExtPowerSnd[i].Pitch);
            EditorGUILayout.EndHorizontal();
        }

        // COASTER ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Coaster: (minRpm|maxRpm|recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(true, false, false, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        for (int i = 0; i < carSound.ExtCoasterSnd.Length; i++)
        {
            if (carSound.ExtCoasterSnd[i].Volume == null) carSound.ExtCoasterSnd[i].Volume = new AnimationCurve();
            if (carSound.ExtCoasterSnd[i].Pitch == null) carSound.ExtCoasterSnd[i].Pitch = new AnimationCurve();
            EditorGUILayout.BeginHorizontal();
            carSound.ExtCoasterSnd[i].Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.ExtCoasterSnd[i].Audio, typeof(AudioClip), true);
            GUI.color = new Color32(199,0,2, 255);
            if (GUILayout.Button("x"))
            {
                carSound.removeSound(true, false, false, false, false, i);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Volume:");
            carSound.ExtCoasterSnd[i].Volume = EditorGUILayout.CurveField(carSound.ExtCoasterSnd[i].Volume);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Pitch:");
            carSound.ExtCoasterSnd[i].Pitch = EditorGUILayout.CurveField(carSound.ExtCoasterSnd[i].Pitch);
            EditorGUILayout.EndHorizontal();
        }

        // REVERSE ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reverse: (recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, false, true, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        if (carSound.ExtReverseSnd.Volume == null) carSound.ExtReverseSnd.Volume = new AnimationCurve();
        if (carSound.ExtReverseSnd.Pitch == null) carSound.ExtReverseSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.ExtReverseSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.ExtReverseSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199,0,2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, false, true, false, false, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.ExtReverseSnd.Volume = EditorGUILayout.CurveField(carSound.ExtReverseSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.ExtReverseSnd.Pitch = EditorGUILayout.CurveField(carSound.ExtReverseSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        // IDLE -------------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Idle: (recordRpm)");
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, false, false, true, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        if (carSound.ExtIdleSnd.Volume == null) carSound.ExtIdleSnd.Volume = new AnimationCurve();
        if (carSound.ExtIdleSnd.Pitch == null) carSound.ExtIdleSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.ExtIdleSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.ExtIdleSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199, 0, 2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, false, false, true, false, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.ExtIdleSnd.Volume = EditorGUILayout.CurveField(carSound.ExtIdleSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.ExtIdleSnd.Pitch = EditorGUILayout.CurveField(carSound.ExtIdleSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        // MAX RPM ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MaxRpm: (recordRpm)");
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, false, false, false, true);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        if (carSound.ExtLimiterSnd.Volume == null) carSound.ExtLimiterSnd.Volume = new AnimationCurve();
        if (carSound.ExtLimiterSnd.Pitch == null) carSound.ExtLimiterSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.ExtLimiterSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.ExtLimiterSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199, 0, 2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, false, false, false, true, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.ExtLimiterSnd.Volume = EditorGUILayout.CurveField(carSound.ExtLimiterSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.ExtLimiterSnd.Pitch = EditorGUILayout.CurveField(carSound.ExtLimiterSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        #endregion EXTERNAL

        GUI.color = new Color32(44, 203, 220, 255);
        if (GUILayout.Button("Copy curves Ext -> Int"))
        {
            if (EditorUtility.DisplayDialogComplex("Warning",
                    "Are you sure to copy the Ext curves value to the Int?\r\nAll the old data are owerwritten", "Ok",
                    "Cancel", "No") == 0)
            {
                // power
                for (int i = 0; i < carSound.ExtPowerSnd.Length; i++)
                {
                    carSound.IntPowerSnd[i].Volume = carSound.ExtPowerSnd[i].Volume;
                    carSound.IntPowerSnd[i].Pitch = carSound.ExtPowerSnd[i].Pitch;
                }
                // coaster
                for (int i = 0; i < carSound.ExtCoasterSnd.Length; i++)
                {
                    carSound.IntCoasterSnd[i].Volume = carSound.ExtCoasterSnd[i].Volume;
                    carSound.IntCoasterSnd[i].Pitch = carSound.ExtCoasterSnd[i].Pitch;
                }

                carSound.IntReverseSnd.Volume = carSound.ExtReverseSnd.Volume;
                carSound.IntReverseSnd.Pitch = carSound.ExtReverseSnd.Pitch;
                carSound.IntIdleSnd.Volume = carSound.ExtIdleSnd.Volume;
                carSound.IntIdleSnd.Pitch = carSound.ExtIdleSnd.Pitch;
                carSound.IntLimiterSnd.Volume = carSound.ExtLimiterSnd.Volume;
                carSound.IntLimiterSnd.Pitch = carSound.ExtLimiterSnd.Pitch;
            }
        }
        GUI.color = Color.white;

        #region INTERNAL

        GUILayout.Space(10);
        EditorGUILayout.LabelField("Internal sound", EditorStyles.boldLabel);

        // POWER ------------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Power: (minRpm|maxRpm|recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, true, false, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        for (int i = 0; i < carSound.IntPowerSnd.Length; i++)
        {
            if (carSound.IntPowerSnd[i].Volume == null) carSound.IntPowerSnd[i].Volume = new AnimationCurve();
            if (carSound.IntPowerSnd[i].Pitch == null) carSound.IntPowerSnd[i].Pitch = new AnimationCurve();
            EditorGUILayout.BeginHorizontal();
            carSound.IntPowerSnd[i].Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.IntPowerSnd[i].Audio, typeof(AudioClip), true);
            GUI.color = new Color32(199,0,2, 255);
            if (GUILayout.Button("x"))
            {
                carSound.removeSound(false, true, false, false, false, i);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Volume:");
            carSound.IntPowerSnd[i].Volume = EditorGUILayout.CurveField(carSound.IntPowerSnd[i].Volume);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Pitch:");
            carSound.IntPowerSnd[i].Pitch = EditorGUILayout.CurveField(carSound.IntPowerSnd[i].Pitch);
            EditorGUILayout.EndHorizontal();
        }

        // COASTER ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Coaster: (minRpm|maxRpm|recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(true, true, false, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        for (int i = 0; i < carSound.IntCoasterSnd.Length; i++)
        {
            if (carSound.IntCoasterSnd[i].Volume == null) carSound.IntCoasterSnd[i].Volume = new AnimationCurve();
            if (carSound.IntCoasterSnd[i].Pitch == null) carSound.IntCoasterSnd[i].Pitch = new AnimationCurve();
            EditorGUILayout.BeginHorizontal();
            carSound.IntCoasterSnd[i].Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.IntCoasterSnd[i].Audio, typeof(AudioClip), true);
            GUI.color = new Color32(199,0,2, 255);
            if (GUILayout.Button("x"))
            {
                carSound.removeSound(true, true, false, false, false, i);
            }
            GUI.color = Color.white;
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Volume:");
            carSound.IntCoasterSnd[i].Volume = EditorGUILayout.CurveField(carSound.IntCoasterSnd[i].Volume);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Pitch:");
            carSound.IntCoasterSnd[i].Pitch = EditorGUILayout.CurveField(carSound.IntCoasterSnd[i].Pitch);
            EditorGUILayout.EndHorizontal();
        }

        // REVERSE ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Reverse: (recordRpm)" );
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, true, true, false, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();
        
        if (carSound.IntReverseSnd.Volume == null) carSound.IntReverseSnd.Volume = new AnimationCurve();
        if (carSound.IntReverseSnd.Pitch == null) carSound.IntReverseSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.IntReverseSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.IntReverseSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199,0,2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, true, true, false, false, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.IntReverseSnd.Volume = EditorGUILayout.CurveField(carSound.IntReverseSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.IntReverseSnd.Pitch = EditorGUILayout.CurveField(carSound.IntReverseSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        // IDLE -------------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Idle: (recordRpm)");
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, true, false, true, false);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        if (carSound.IntIdleSnd.Volume == null) carSound.IntIdleSnd.Volume = new AnimationCurve();
        if (carSound.IntIdleSnd.Pitch == null) carSound.IntIdleSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.IntIdleSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.IntIdleSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199, 0, 2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, true, false, true, false, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.IntIdleSnd.Volume = EditorGUILayout.CurveField(carSound.IntIdleSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.IntIdleSnd.Pitch = EditorGUILayout.CurveField(carSound.IntIdleSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        // MAX RPM ----------------------------------------------------------------------------------------------------------------------------------
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("MaxRpm: (recordRpm)");
        GUI.color = new Color32(125, 255, 123, 255);
        if (GUILayout.Button("+"))
        {
            carSound.addSound(false, true, false, false, true);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        if (carSound.IntLimiterSnd.Volume == null) carSound.IntLimiterSnd.Volume = new AnimationCurve();
        if (carSound.IntLimiterSnd.Pitch == null) carSound.IntLimiterSnd.Pitch = new AnimationCurve();
        EditorGUILayout.BeginHorizontal();
        carSound.IntLimiterSnd.Audio = (AudioClip)EditorGUILayout.ObjectField(carSound.IntLimiterSnd.Audio, typeof(AudioClip), true);
        GUI.color = new Color32(199, 0, 2, 255);
        if (GUILayout.Button("x"))
        {
            carSound.removeSound(false, true, false, false, true, 0);
        }
        GUI.color = Color.white;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Volume:");
        carSound.IntLimiterSnd.Volume = EditorGUILayout.CurveField(carSound.IntLimiterSnd.Volume);
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PrefixLabel("Pitch:");
        carSound.IntLimiterSnd.Pitch = EditorGUILayout.CurveField(carSound.IntLimiterSnd.Pitch);
        EditorGUILayout.EndHorizontal();

        #endregion INTERNAL

        if (GUI.changed)
        {
            EditorUtility.SetDirty(carSound);
            if (!Application.isPlaying)
            {
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
        }
    }
}

