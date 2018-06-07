Shader "gRally/Window"
{
    Properties
    {
        _Color("Spec Color", Color) = (1, 1, 1, 1)
        _Smooth("Smoothness", Range(0,1)) = 0.5
        //|_|_HueVariation("Hue Variation", Color) = (1.0,0.5,0.0,0.1)
        _MainTex("Albedo", 2D) = "white" {}
        _Dirty("Dirty", Range(0,5)) = 0.0
        _Interior("Interior", Range(0,1)) = 1.0
    }

    SubShader
    {
        Tags{ "Queue" = "Transparent" "IgnoreProjector" = "False" "RenderType" = "Transparent" }
        LOD 300

        CGPROGRAM
        #pragma surface surf StandardSpecular alpha:premul vertex:vert
        #pragma target 3.0
        

        sampler2D _MainTex;
        half _MainTexID;
        half _Dirty;
        half _Interior;
        half _Smooth;
        fixed _DIRT_1_LEVEL;

        fixed4 _Color;

        struct Input
        {
            float2 uv_MainTex;
            half3 normal;
            float3 worldRefl;
            //|_|half3 color;
            //|_|half3 interpolator1;
        };

        //|_|#ifdef EFFECT_HUE_VARIATION
        //|_|#define HueVariationAmount interpolator1.z
        //|_|half4 _HueVariation;
        //|_|#endif

        void vert(inout appdata_full v, out Input o)
        {
            // appdata_full
            UNITY_INITIALIZE_OUTPUT(Input, o);

            // normals up
            //v.normal = half3(0,0,1);
            if (_Interior == 1.0f)
            {
                v.normal = v.normal * -1.0f;
            }
            o.normal = v.normal;
        }

        void surf(Input IN, inout SurfaceOutputStandardSpecular o)
        {
            // diffuse color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex); // *_Color;
            
            float3 lightDir = _WorldSpaceLightPos0.xyz;
            half NdotL = dot(IN.normal, lightDir);
            half diff = NdotL * 0.5 + 0.5;

            if (_Interior == 0.0f || _WorldSpaceLightPos0 .w == 1)
            {
                diff = 1.0f;
            }

            //|_|o.Albedo = c.rgb * IN.color.rgb;
            o.Albedo = c.rgb;//float3(diff, diff, diff);// c.rgb;
                             // alpha for transparencies
                             //o.Alpha = c.a;
            o.Specular = _Color;

            float3 reflectedDir = IN.worldRefl;
            //float4 reflection = UNITY_SAMPLE_TEXCUBE(unity_SpecCube1, reflectedDir);

            if (_Dirty == 0.0f)
            {
                _Dirty = _DIRT_1_LEVEL;
            }

            if (c.a < 0.8f)
            {
                o.Alpha = c.a * diff * _Dirty;
            }
            else
            {
                o.Alpha = c.a;
            }
            // specular power in 0..1 range
            o.Smoothness = _Smooth;
            // specular intensity
            //o.Gloss = 0.5f;
        }
        ENDCG
    }
    //FallBack "Legacy Shaders/Transparent/Cutout/Diffuse"
}
