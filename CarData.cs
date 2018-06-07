using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class CarData : ScriptableObject
{
    [Header("gCarEditor data")]
    public string exportPath = "";
    [Space(10)]
    [Header("Car Info")]
    public string Name;
    [TextArea(5, 50)]
    public string Description;
    public string Group;
    public string Brand;
    public string Transmission;
    [Space(10)]
    public string Tags;
}