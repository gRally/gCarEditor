Shader "gRally/grsCarPattern"
{
	Properties
	{
		_Color("Color for R", Color) = (1,1,1,1)
		_Color1("Color for G", Color) = (1,1,1,1)
		_Color2("Color for B", Color) = (1,1,1,1)
		_Color3("Color for A", Color) = (1,1,1,1)
		
		_DecalTex("Decal", 2D) = "white" {}
        _NumberPlate("NumberPlate", 2D) = "white" {}
		_MainTex("Albedo", 2D) = "white" {}
		
		_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5

		_Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
		[Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
		_MetallicGlossMap("Metallic", 2D) = "white" {}

		_BumpScale("Scale", Float) = 1.0
		_BumpMap("Normal Map", 2D) = "bump" {}

		_Parallax ("Height Scale", Range (0.005, 0.08)) = 0.02
		_ParallaxMap ("Height Map", 2D) = "black" {}

		_OcclusionStrength("Strength", Range(0.0, 1.0)) = 1.0
		_OcclusionMap("Occlusion", 2D) = "white" {}

		_EmissionColor("Color", Color) = (0,0,0)
		_EmissionMap("Emission", 2D) = "white" {}
		
		_DetailMask("Detail Mask", 2D) = "white" {}

		_DetailAlbedoMap("Detail Albedo x2", 2D) = "grey" {}
		_DetailNormalMapScale("Scale", Float) = 1.0
		_DetailNormalMap("Normal Map", 2D) = "bump" {}

		[Enum(UV0,0,UV1,1)] _UVSec ("UV Set for secondary textures", Float) = 0

		// UI-only data
		[HideInInspector] _EmissionScaleUI("Scale", Float) = 0.0
		[HideInInspector] _EmissionColorUI("Color", Color) = (1,1,1)

		// Blending state
		[HideInInspector] _Mode ("__mode", Float) = 0.0
		[HideInInspector] _SrcBlend ("__src", Float) = 1.0
		[HideInInspector] _DstBlend ("__dst", Float) = 0.0
		[HideInInspector] _ZWrite ("__zw", Float) = 1.0
	}

	SubShader {
		Tags { "RenderType"="Opaque" }
		LOD 200
		
		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _DecalTex;
        sampler2D _NumberPlate;

		struct Input {
			float2 uv_MainTex;
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;
		fixed4 _Color1;
		fixed4 _Color2;
		fixed4 _Color3;

		void surf (Input IN, inout SurfaceOutputStandard o) {
			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			float pR = (c.r * _Color.r) + (c.g * _Color1.r) + (c.b * _Color2.r) + (c.a * _Color3.r);
			float pG = (c.r * _Color.g) + (c.g * _Color1.g) + (c.b * _Color2.g) + (c.a * _Color3.g);
			float pB = (c.r * _Color.b) + (c.g * _Color1.b) + (c.b * _Color2.b) + (c.a * _Color3.b);
			
			c.r = pR;
			c.g = pG;
			c.b = pB;
			fixed4 cDecal = tex2D (_DecalTex, IN.uv_MainTex);
			
			o.Albedo = c.rgb * (1 - cDecal.a) + cDecal.rgb * cDecal.a;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Glossiness;
			o.Alpha = c.a;
		}
		ENDCG
	} 

	//FallBack "VertexLit"
	//CustomEditor "grsCarPatternShaderGUI"
}
