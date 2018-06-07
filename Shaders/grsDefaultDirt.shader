Shader "gRally/DefaultDirt" {
	Properties {
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB) Smoothness(A)", 2D) = "white" {}

        _SpecColor("Specular", Color) = (0.2, 0.2, 0.2)
		_Smoothness("Smoothness", Range(0,1)) = 0.5

        _BumpMap("Normal Map", 2D) = "bump" {}

        _DirtLevel("Dirt Level", Range(0,1)) = 0.0
        _DirtTex("Dirt Texture R", 2D) = "white" {}
        //_DirtColor("Dirt Color", Color) = (1,1,1,1)
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf StandardSpecular fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
        sampler2D _BumpMap;

        sampler2D _DirtTex;
        fixed4 _DIRT_1_COLOR;
        half _DirtLevel;
        half _DIRT_1_LEVEL;

		struct Input {
			float2 uv_MainTex;
		};

		half _Smoothness;
		half _Specular;
		fixed4 _Color;

		void surf (Input IN, inout SurfaceOutputStandardSpecular o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);

            // dirt
            if (_DirtLevel == 0.0f)
            {
                _DirtLevel = _DIRT_1_LEVEL;
            }
            fixed4 dirt = tex2D(_DirtTex, IN.uv_MainTex);
            c.rgb = _Color * c.rgb * (1 - dirt.r * _DirtLevel) + _DIRT_1_COLOR * dirt.r * _DirtLevel;

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_MainTex));
			o.Specular = _SpecColor.rgb;
			//o.Smoothness = _Glossiness;
            o.Smoothness = min(c.a * _Smoothness, (1 - dirt.r * _DirtLevel));
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
