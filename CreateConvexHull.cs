using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

public class CreateConvexHull : MonoBehaviour
{
    public GameObject ParentMesh;

    public void CreateHull()
    {
        if (transform.gameObject.GetComponent<MeshFilter>() == null)
        {
            transform.gameObject.AddComponent<MeshFilter>();
        }
        if (transform.gameObject.GetComponent<MeshCollider>() == null)
        {
            transform.gameObject.AddComponent<MeshCollider>();
        }

        if (ParentMesh != null)
        {
            var meshes = ParentMesh.GetComponentsInChildren<MeshFilter>();
            //List<Mesh> newMeshes = new List<Mesh>();
            List<Vertex> vertices = new List<Vertex>();
            foreach (var mesh in meshes)
            {
                //var m = Matrix4x4.TRS(mesh.transform.localPosition, mesh.transform.localRotation, mesh.transform.localScale);
                var m = Matrix4x4.TRS(mesh.transform.position, mesh.transform.rotation, mesh.transform.lossyScale);
                var vertex = mesh.sharedMesh.vertices;
                var verticesTmp = vertex.Select(x => new Vertex(x)).ToList();

                foreach (var item in verticesTmp)
                {
                    vertices.Add(new Vertex(m.MultiplyPoint3x4(new Vector3(Convert.ToSingle(item.Position[0]), Convert.ToSingle(item.Position[1]), Convert.ToSingle(item.Position[2])))));
                }
                //vertices.AddRange(verticesTmp);
            }
            var result = MIConvexHull.ConvexHull.Create(vertices, 0.035);

            string name = string.Format("{0}/hull.obj", Application.dataPath);
            Debug.Log("export to " + name);
            var newMesh = CreateMesh(result.Points.Select(x => x.ToVec()));
#if UNITY_EDITOR
            UnityEditor.MeshUtility.Optimize(newMesh);
#endif
            MeshToFile(newMesh, name);

            //transform.gameObject.GetComponent<MeshFilter>().mesh = newMesh;
            //transform.gameObject.GetComponent<MeshCollider>().sharedMesh = newMesh;
        }
        else
        {
            Debug.LogWarning("no ParentMesh!!");
        }

    }

    public void MeshToFile(Mesh m, string filename)
    {
        if (File.Exists(filename))
        {
            File.Delete(filename);
        }

        using (StreamWriter sw = new StreamWriter(filename))
        {
            sw.Write(MeshToString(m));
        }
    }

    public string MeshToString(Mesh m)
    {
        //Mesh m = mf.mesh;
        Material[] mats = new Material[1];// mf.renderer.sharedMaterials;
        //mats[0] = HullMaterial;

        StringBuilder sb = new StringBuilder();

        sb.Append("g ").Append("hull").Append("\n");
        foreach (Vector3 v in m.vertices)
        {
            sb.Append(string.Format("v {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.normals)
        {
            sb.Append(string.Format("vn {0} {1} {2}\n", v.x, v.y, v.z));
        }
        sb.Append("\n");
        foreach (Vector3 v in m.uv)
        {
            sb.Append(string.Format("vt {0} {1}\n", v.x, v.y));
        }
        /*
        for (int material = 0; material < m.subMeshCount; material++)
        {
            sb.Append("\n");
            sb.Append("usemtl ").Append(mats[material].name).Append("\n");
            sb.Append("usemap ").Append(mats[material].name).Append("\n");

            int[] triangles = m.GetTriangles(material);
            for (int i = 0; i < triangles.Length; i += 3)
            {
                sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                    triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
            }
        }
        */
        sb.Append("\n");
        sb.Append("usemtl ").Append("matHull").Append("\n");
        sb.Append("usemap ").Append("matHull").Append("\n");

        int[] triangles = m.GetTriangles(0);
        for (int i = 0; i < triangles.Length; i += 3)
        {
            sb.Append(string.Format("f {0}/{0}/{0} {1}/{1}/{1} {2}/{2}/{2}\n",
                triangles[i] + 1, triangles[i + 1] + 1, triangles[i + 2] + 1));
        }

        return sb.ToString();
    }

    Mesh CreateMesh(IEnumerable<Vector3> stars)
    {
        Mesh m = new Mesh();
        m.name = "ScriptedMesh";
        List<int> triangles = new List<int>();

        var vertices = stars.Select(x => new Vertex(x)).ToList();

        var result = MIConvexHull.ConvexHull.Create(vertices);
        m.vertices = result.Points.Select(x => x.ToVec()).ToArray();
        var xxx = result.Points.ToList();

        foreach (var face in result.Faces)
        {
            triangles.Add(xxx.IndexOf(face.Vertices[0]));
            triangles.Add(xxx.IndexOf(face.Vertices[1]));
            triangles.Add(xxx.IndexOf(face.Vertices[2]));
        }

        m.triangles = triangles.ToArray();
        m.RecalculateNormals();
        return m;
    }
}
