using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public class CarLivery : MonoBehaviour
{
    [SerializeField]
    public List<Texture2D> Patterns = new List<Texture2D> ();

    [SerializeField]
    public Material LiveryMat;

    [SerializeField]
    public Texture2D PlatesSampler;
    public BMObject NumberPlateLeft;
    [SerializeField]
    public BMObject NumberPlateRight;

    [SerializeField]
    public Texture2D DriverNameSampler;
    public BMObject DriverNameLeft;
    [SerializeField]
    public BMObject DriverNameRight;

    [SerializeField]
    public Texture2D DecalsSampler;
    public List<BMObject> Decals;

    //public Material DestinationMaterial;
    private int _textureSize = 1024;

    gPhys.Livery.Helper helper;

    void Start()
    {
        helper = new gPhys.Livery.Helper();
    }

    public void DrawTexture()
    {
        if (helper == null)
        {
            helper = new gPhys.Livery.Helper();
        }
        var before = DateTime.Now;

        _textureSize = 1024;

        var texBase = helper.CreatefillTexture2D(Color.clear, _textureSize, _textureSize);

        // plates
        helper.Manage(NumberPlateLeft, helper.Clone(PlatesSampler), ref texBase);
        helper.Manage(NumberPlateRight, helper.Clone(PlatesSampler), ref texBase);

        // driver name
        helper.Manage(DriverNameLeft, helper.Clone(DriverNameSampler), ref texBase);
        helper.Manage(DriverNameRight, helper.Clone(DriverNameSampler), ref texBase);

        // decals
        for (int i = 0; i < Decals.Count; i++)
        {
            helper.Manage(Decals[i], helper.Clone(DecalsSampler), ref texBase);
        }
        texBase.Apply();
        LiveryMat.SetTexture("_Decals", texBase);

        var after = DateTime.Now;
        var diff = after - before;
        Debug.Log(string.Format("DrawTexture = {0}.{1}", diff.Seconds, diff.Milliseconds));
    }
}
