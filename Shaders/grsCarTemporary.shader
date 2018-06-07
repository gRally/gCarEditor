Shader "gRally/grsCarTemporary"
{
    Properties
    {
        _Color("Color", Color) = (1,1,1,1)
        _MainTex("Template", 2D) = "white" {}
        _NumberPlate("NumberPlate", 2D) = "white" {}


        _DecalTex("Decal", 2D) = "white" {}
        
        // car number
        _Texts("Texts", 2D) = "white" {}
        // driver name
        _DriverName("DriverName", 2D) = "white" {}
        _DriverNameColor("DriverNameColor", Color) = (0,0,0)

        _Glossiness("Smoothness", Range(0.0, 1.0)) = 0.5
        [Gamma] _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _MetallicGlossMap("Metallic", 2D) = "white" {}

        _BumpScale("Scale", Float) = 1.0
        _BumpMap("Normal Map", 2D) = "bump" {}

        [Enum(UV0,0,UV1,1)] _UVSec("UV Set for secondary textures", Float) = 0

        // UI-only data
        [HideInInspector] _EmissionScaleUI("Scale", Float) = 0.0
        [HideInInspector] _EmissionColorUI("Color", Color) = (1,1,1)

        // Blending state
        [HideInInspector] _Mode("__mode", Float) = 0.0
        [HideInInspector] _SrcBlend("__src", Float) = 1.0
        [HideInInspector] _DstBlend("__dst", Float) = 0.0
        [HideInInspector] _ZWrite("__zw", Float) = 1.0
    }

    SubShader{
        Tags{ "RenderType" = "Opaque" }
        LOD 200

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0
        //#pragma shader_feature SMOOTHNESS_IN_ALBEDO
        #pragma shader_feature _METALLICGLOSSMAP

        sampler2D _MainTex;
        sampler2D _DecalTex;
        sampler2D _NumberPlate;
        sampler2D _Texts;
        sampler2D _DriverName;
        fixed4 _DriverNameColor;

        struct Input {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        fixed _USE_PLATES;

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            // template & color:
            fixed4 c = _Color;
            fixed4 cTemplate = tex2D(_MainTex, IN.uv_MainTex);
            c.rgb = c.rgb * (1 - cTemplate.a) + cTemplate.rgb * cTemplate.a;

            // numberplate
            fixed4 cPlate = tex2D(_NumberPlate, IN.uv_MainTex);
            c.rgb = c.rgb * (1 - cPlate.a) + cPlate.rgb * cPlate.a;
            
            // metallic
            //fixed4 cMetal = tex2D(_MetallicGlossMap, IN.uv_MainTex) * _Metallic;

            // output
            o.Albedo = c.rgb;
            //o.Metallic = _Metallic;
            //o.Smoothness = tex2D(_MainTex, IN.texcoord.xy).a;
            //o.Alpha = 1.0f;
            /*
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            fixed4 cDecal = tex2D(_DecalTex, IN.uv_MainTex);
            c.rgb = c.rgb * (1 - cDecal.a) + cDecal.rgb * cDecal.a;

            // if plate
            if (_USE_PLATES == 1.0f)
            {
                fixed4 cPlate = tex2D(_NumberPlate, IN.uv_MainTex);
                fixed4 cTexts = tex2D(_Texts, IN.uv_MainTex);
                c.rgb = c.rgb * (1 - cPlate.a) + cPlate.rgb * cPlate.a;
                c.rgb = c.rgb * (1 - cTexts.a) + cTexts.rgb * cTexts.a;

                fixed4 cName = tex2D(_DriverName, IN.uv_MainTex);
                c.rgb = c.rgb * (1 - cName.a) + _DriverNameColor.rgb * cName.a;
                c.rgb = c.rgb * (1 - cName.a) + _DriverNameColor.rgb * cName.a;
            }

            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Specular = _Metallic;
            o.Smoothness = _Glossiness;
            o.Alpha = c.a;
            */
        }
        ENDCG
    }
        //FallBack "VertexLit"
        //CustomEditor "grsCarTemplateShaderGUI"
}

