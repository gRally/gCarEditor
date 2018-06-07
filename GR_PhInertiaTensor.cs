using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GR_PhInertiaTensor : MonoBehaviour
{
    public float TensorX;
    public float TensorY;
    public float TensorZ;

    public float Mass = 1050.0f;

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        var x = transform.localScale.x;
        var y = transform.localScale.y;
        var z = transform.localScale.z;

        TensorX = Mass / 12.0f * (y * y + z * z);
        TensorY = Mass / 12.0f * (x * x + z * z);
        TensorZ = Mass / 12.0f * (x * x + y * y);
    }
}
