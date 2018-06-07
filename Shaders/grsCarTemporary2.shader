// Upgrade NOTE: upgraded instancing buffer 'Props' to new syntax.

Shader "gRally/grsCarTemporary2"
{
	Properties {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Template", 2D) = "white" {}
        _Decals("Decals", 2D) = "white" {}
        _Occlusion("Ambient Occlusion", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
		//_Metallic ("Metallic", Range(0,1)) = 0.0
        //_GlossMapScale("glossmapscale", Range(0,1)) = 0.5
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        [NoScaleOffset] _BumpMap("Normal Map", 2D) = "bump" {}
        _BumpScale("Normal Influence", Range(0,10)) = 0.5
	}
	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 5.0
        //#pragma shader_feature _NORMALMAP
        //#pragma shader_feature SMOOTHNESS_IN_ALBEDO
        //#pragma shader_feature _METALLICGLOSSMAP

		sampler2D _MainTex;
        sampler2D _Decals;
        sampler2D _MetallicGlossMap;
        sampler2D _BumpMap;
        sampler2D _Occlusion;

        struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color;
        float _Glossiness;
        float _BumpScale;

		// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
		// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
		// #pragma instancing_options assumeuniformscaling
		UNITY_INSTANCING_BUFFER_START(Props)
			// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

		void surf (Input IN, inout SurfaceOutputStandard o) {
            // template & color:
            fixed4 c = _Color;
            c.a = 1.0f;
            fixed4 cTemplate = tex2D(_MainTex, IN.uv_MainTex);
            c.rgb = c.rgb * (1 - cTemplate.a) + cTemplate.rgb * cTemplate.a;

            // decals
            fixed4 cDecals = tex2D(_Decals, IN.uv_MainTex);
            c.rgb = c.rgb * (1 - cDecals.a) + cDecals.rgb * cDecals.a;

            // normalize
            o.Normal = normalize(UnpackScaleNormal(tex2D(_BumpMap, IN.uv_MainTex), _BumpScale));

            // spec
            fixed4 cSpec = tex2D(_MetallicGlossMap, IN.uv_MainTex);
            o.Metallic = cSpec.rgb;// *_Metallic;
            o.Smoothness = _Glossiness * cSpec.a;

            // finalize
            o.Albedo = c;
            
            // Metallic and smoothness come from slider variables
            //o.Metallic = tex2D(_MetallicGlossMap, IN.uv_MainTex); //_Metallic;
            //o.Smoothness = _Glossiness;
            //o.Alpha = c.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
