Shader "Custom/OmnidirectionalLightURP"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _SunPosition ("Sun Position", Vector) = (0, 0, 0)
        _LightColor ("Light Color", Color) = (1, 1, 0, 1)
    }
    SubShader
    {
        Tags { "RenderPipeline" = "UniversalPipeline" }
        Pass
        {
            Name "UniversalForward"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Textures et lumières
            sampler2D _MainTex;
            float3 _SunPosition;
            float4 _LightColor;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                float3 normalWS : TEXCOORD2;
            };

            Varyings vert(Attributes input)
            {
                Varyings output;
                float4 positionWS = TransformObjectToWorld(input.positionOS);
                output.positionHCS = TransformWorldToHClip(positionWS);
                output.worldPos = positionWS.xyz;
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = input.uv;
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // Direction de la lumière par rapport à la position du soleil
                float3 lightDir = normalize(_SunPosition - input.worldPos);
                float intensity = saturate(dot(input.normalWS, lightDir));

                // Ajout de l'éclairage
                half4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);
                return baseColor * _LightColor * intensity;
            }
            ENDHLSL
        }
    }
}
