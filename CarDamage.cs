using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarDamage : MonoBehaviour
{
    public Material MaterialToDamage;
    public MeshFilter[] meshes;

    public void FindMeshes()
    {
        List<MeshFilter> meshesTmp = new List<MeshFilter>();
        var arrend = (Renderer[])Resources.FindObjectsOfTypeAll(typeof(Renderer));
        foreach (var rend in arrend)
        {
            if (rend != null)
            {
                if (rend.sharedMaterials != null)
                {
                    var mesh = rend.gameObject.GetComponent<MeshFilter>();
                    if (mesh != null)
                    {
                        foreach (var mat in rend.sharedMaterials)
                        {
                            if (mat == MaterialToDamage)
                            {

                                meshesTmp.Add(mesh);
                            }
                        }
                    }
                }
            }
        }
        meshes = meshesTmp.ToArray();
    }
}
