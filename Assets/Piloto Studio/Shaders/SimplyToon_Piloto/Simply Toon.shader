Shader "Piloto Studio/Simply Toon URP"
{
    Properties
    {
        _MainTex ("Base Color", 2D) = "white" {}
        [HDR]_RimColor ("Rim Color", Color) = (0, 0.5549643, 1, 0)
        _RimOffset ("Rim Offset", Float) = 0.24
        _RimFalloff ("Rim Falloff", Vector) = (0, 0, 0, 0)
        _RimShadow ("Rim Shadow", Range(0, 1)) = 0
        _Dimming ("Dimming", Range(0, 1)) = 0.75
        _BandingBias ("Banding Bias", Float) = 2
    }

    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent" 
            "RenderPipeline"="UniversalPipeline"
        }

        LOD 100
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Back

        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode"="UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float3 normalWS : TEXCOORD1;
                float2 uv : TEXCOORD2;
                float4 color : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _RimColor;
                float _RimOffset;
                float2 _RimFalloff;
                float _RimShadow;
                float _Dimming;
                float _BandingBias;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.normalWS = TransformObjectToWorldNormal(input.normalOS);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                return output;
            }

            float4 frag(Varyings input) : SV_Target
            {
                // Main texture sampling
                float4 baseColor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.uv);

                // Light direction and normal
                float3 lightDir = normalize(_MainLightPosition.xyz);
                float3 normalWS = normalize(input.normalWS);

                // Lighting calculation
                float NdotL = dot(normalWS, lightDir);
                float lightIntensity = smoothstep(0, 0.01, NdotL * _BandingBias);

                // View direction and rim calculation
                float3 viewDir = normalize(_WorldSpaceCameraPos - input.positionWS);
                float rimDot = 1 - saturate(dot(viewDir, normalWS) + _RimOffset);
                float rimIntensity = smoothstep(_RimFalloff.x, _RimFalloff.y, rimDot);
                
                // Color blending
                float3 rimColor = lerp(1, _RimShadow, rimIntensity) * rimDot * _RimColor.rgb;
                float3 finalColor = baseColor.rgb * lightIntensity * _Dimming + rimColor;

                // Final color with vertex color and alpha
                return float4(finalColor * input.color.rgb, input.color.a);
            }
            ENDHLSL
        }

        // Shadow casting pass
        Pass
        {
            Name "ShadowCaster"
            Tags{"LightMode" = "ShadowCaster"}

            ZWrite On
            ZTest LEqual
            ColorMask 0

            HLSLPROGRAM
            #pragma vertex ShadowPassVertex
            #pragma fragment ShadowPassFragment

            #include "Packages/com.unity.render-pipelines.universal/Shaders/ShadowCasterPass.hlsl"
            ENDHLSL
        }
    }
}