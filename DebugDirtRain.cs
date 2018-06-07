using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class DebugDirtRain : MonoBehaviour
{
    [Range(0.0f, 1.0f)]
    public float Dirt;
    private float _dirt;

	[Range(0.0f, 1.0f)]
	public float Dirt2;
	private float _dirt2;

    [Range(0.0f, 1.0f)]
    public float Rain;
    private float _rain;

    void Update ()
    {
        if (_dirt != Dirt)
        {
            Shader.SetGlobalFloat("_DIRT_1_LEVEL", Dirt);
            _dirt = Dirt;
        }

		if (_dirt2 != Dirt2)
		{
			Shader.SetGlobalFloat("_DIRT_2_LEVEL", Dirt2);
			_dirt2 = Dirt2;
		}

        if (_rain != Rain)
        {
            Shader.SetGlobalFloat("_GR_Rain", Rain);
            _rain = Rain;
        }
    }
}
