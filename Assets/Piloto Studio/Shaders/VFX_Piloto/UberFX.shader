Shader "Piloto Studio/URP/UberFX"
{
    Properties
    {
        [Enum(UnityEngine.Rendering.BlendMode)]_SourceBlendRGB("Blend Mode", Float) = 10
        _MainTex("Main Texture", 2D) = "white" {}
        _MainTextureChannel("Main Texture Channel", Vector) = (1,1,1,0)
        _MainAlphaChannel("Main Alpha Channel", Vector) = (0,0,0,1)
        _MainTexturePanning("Main Texture Panning ", Vector) = (0.2522222,0,0,0)
        _Desaturate("Desaturate? ", Range( 0 , 1)) = 0
        [Toggle(_USESOFTALPHA_ON)] _UseSoftAlpha("Use Soft Particles?", Float) = 0
        _SoftFadeFactor("Soft Fade Factor", Range( 0.1 , 1)) = 0.1
        [Toggle(_USEALPHAOVERRIDE_ON)] _UseAlphaOverride("Use Alpha Override", Float) = 0
        _AlphaOverride("Alpha Override", 2D) = "white" {}
        _AlphaOverrideChannel("Alpha Override Channel", Vector) = (0,0,0,1)
        _AlphaOverridePanning("Alpha Override Panning", Vector) = (0,0,0,0)
        _DetailNoise("Detail Noise", 2D) = "white" {}
        _DetailNoisePanning("Detail Noise Panning", Vector) = (0.2,0,0,0)
        _DetailDistortionChannel("Detail Distortion Channel", Vector) = (0,0,0,1)
        _DistortionIntensity("Distortion Intensity", Range( 0 , 3)) = 2
        _DetailMultiplyChannel("Detail Multiply Channel", Vector) = (1,1,1,0)
        _MultiplyNoiseDesaturation("Multiply Noise Desaturation", Range( 0 , 1)) = 1
        _DetailAdditiveChannel("Detail Additive Channel", Vector) = (0,0,0,1)
        _DetailDisolveChannel("Detail Disolve Channel", Vector) = (0,0,0,1)
        [Toggle(_USERAMP_ON)] _UseRamp("Use Color Ramping?", Float) = 0
        _MiddlePointPos("Middle Point Position", Range( 0 , 1)) = 0.5
        _WhiteColor("Highs", Color) = (1,0.8950032,0,0)
        _MidColor("Middles", Color) = (1,0.4447915,0,0)
        _LastColor("Lows", Color) = (1,0,0,0)
        [Toggle(_USEUVOFFSET_ON)] _UseUVOffset("Use UV Offset", Float) = 0
        [Toggle(_FRESNEL_ON)] _Fresnel("Fresnel", Float) = 0
        _FresnelPower("Fresnel Power", Float) = 1
        _FresnelScale("Fresnel Scale", Float) = 1
        [HDR]_FresnelColor("Fresnel Color", Color) = (1,1,1,1)
        [Toggle(_DISABLEEROSION_ON)] _DisableErosion("Disable Erosion", Float) = 0
        [Enum(UnityEngine.Rendering.CullMode)]_CullMode("CullMode", Float) = 0
    }

    SubShader
    {
        Tags 
        { 
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "Queue"="Transparent" 
        }

        Blend SrcAlpha [_SourceBlendRGB]
        Cull [_CullMode]
        ZWrite Off

        Pass
        {
            Name "Unlit"
            Tags { "LightMode" = "UniversalForward" }

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x

            #pragma shader_feature_local _FRESNEL_ON
            #pragma shader_feature_local _USERAMP_ON
            #pragma shader_feature_local _USEUVOFFSET_ON
            #pragma shader_feature_local _DISABLEEROSION_ON
            #pragma shader_feature_local _USESOFTALPHA_ON
            #pragma shader_feature_local _USEALPHAOVERRIDE_ON

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/DeclareDepthTexture.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 color : COLOR;
                float4 uv : TEXCOORD0;
                float4 uv2 : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionCS : SV_POSITION;
                float4 color : COLOR;
                float4 uv : TEXCOORD0;
                float4 uv2 : TEXCOORD1;
                float4 screenPos : TEXCOORD2;
                float3 worldPos : TEXCOORD3;
                float3 worldNormal : TEXCOORD4;
                float fogFactor : TEXCOORD5;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_DetailNoise);
            SAMPLER(sampler_DetailNoise);
            TEXTURE2D(_AlphaOverride);
            SAMPLER(sampler_AlphaOverride);

            CBUFFER_START(UnityPerMaterial)
                float4 _MainTex_ST;
                float4 _DetailNoise_ST;
                float4 _AlphaOverride_ST;
                float2 _MainTexturePanning;
                float2 _DetailNoisePanning;
                float2 _AlphaOverridePanning;
                float4 _MainTextureChannel;
                float4 _MainAlphaChannel;
                float4 _DetailDistortionChannel;
                float4 _DetailMultiplyChannel;
                float4 _DetailAdditiveChannel;
                float4 _DetailDisolveChannel;
                float4 _AlphaOverrideChannel;
                float4 _WhiteColor;
                float4 _MidColor;
                float4 _LastColor;
                float4 _FresnelColor;
                float _DistortionIntensity;
                float _Desaturate;
                float _MultiplyNoiseDesaturation;
                float _MiddlePointPos;
                float _FresnelScale;
                float _FresnelPower;
                float _SoftFadeFactor;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                VertexNormalInputs normalInput = GetVertexNormalInputs(input.normalOS);

                output.positionCS = vertexInput.positionCS;
                output.worldPos = vertexInput.positionWS;
                output.worldNormal = normalInput.normalWS;
                output.color = input.color;
                output.uv = input.uv;
                output.uv2 = input.uv2;
                output.screenPos = ComputeScreenPos(output.positionCS);
                output.fogFactor = ComputeFogFactor(vertexInput.positionCS.z);

                return output;
            }

            // Les fonctions helper ici (comme dans le shader original)
            float3 desaturate(float3 color, float factor)
            {
                float3 lum = float3(0.299, 0.587, 0.114);
                float luminance = dot(color, lum);
                return lerp(color, luminance.xxx, factor);
            }

            half4 frag(Varyings input) : SV_Target
            {
                // La logique du fragment shader ici, convertie pour URP
                float2 mainUV = input.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw;
                float2 noiseUV = input.uv.xy * _DetailNoise_ST.xy + _DetailNoise_ST.zw;
                
                // Calcul du bruit de distorsion
                float2 noisePanner = noiseUV + _DetailNoisePanning * _Time.y;
                float4 detailNoise = SAMPLE_TEXTURE2D(_DetailNoise, sampler_DetailNoise, noisePanner);
                
                // Application de la distorsion
                float distortionNoise = dot(detailNoise * _DetailDistortionChannel, 1);
                float2 distortion = distortionNoise * _DistortionIntensity;
                
                #ifdef _USEUVOFFSET_ON
                    distortion += input.uv2.xy;
                #endif
                
                // Échantillonnage de la texture principale avec distorsion
                float2 finalUV = mainUV + distortion + _MainTexturePanning * _Time.y;
                float4 mainTex = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, finalUV);
                
                // Traitement des canaux et de la désaturation
                float4 mainColor = mainTex * _MainTextureChannel;
                float3 desaturatedMain = desaturate(mainColor.rgb, _Desaturate);
                
                // Application des effets de couleur et du ramping
                float3 finalColor = desaturatedMain;
                #ifdef _USERAMP_ON
                    // Logique de ramping de couleur
                    float rampPosition = dot(desaturatedMain, float3(0.299, 0.587, 0.114));
                    float4 rampedColor = lerp(_LastColor, _MidColor, saturate(rampPosition / _MiddlePointPos));
                    rampedColor = lerp(rampedColor, _WhiteColor, saturate((rampPosition - _MiddlePointPos) / (1 - _MiddlePointPos)));
                    finalColor = rampedColor.rgb;
                #endif
                
                // Application du Fresnel
                #ifdef _FRESNEL_ON
                    float3 viewDir = normalize(_WorldSpaceCameraPos - input.worldPos);
                    float fresnelTerm = pow(1 - saturate(dot(normalize(input.worldNormal), viewDir)), _FresnelPower);
                    fresnelTerm *= _FresnelScale;
                    finalColor = lerp(finalColor, _FresnelColor.rgb, fresnelTerm);
                #endif
                
                // Calcul de l'alpha final
                float alpha = mainTex.a * _MainAlphaChannel.a;
                
                #ifdef _USEALPHAOVERRIDE_ON
                    float2 alphaUV = input.uv.xy * _AlphaOverride_ST.xy + _AlphaOverride_ST.zw + _AlphaOverridePanning * _Time.y;
                    float4 alphaOverride = SAMPLE_TEXTURE2D(_AlphaOverride, sampler_AlphaOverride, alphaUV);
                    alpha *= dot(alphaOverride * _AlphaOverrideChannel, 1);
                #endif
                
                #ifdef _USESOFTALPHA_ON
                    float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, sampler_CameraDepthTexture, input.screenPos.xy / input.screenPos.w), _ZBufferParams);
                    float partZ = input.screenPos.z;
                    float fadeAlpha = saturate((sceneZ - partZ) / _SoftFadeFactor);
                    alpha *= fadeAlpha;
                #endif
                
                // Application de la couleur vertex et fog
                finalColor *= input.color.rgb;
                finalColor = MixFog(finalColor, input.fogFactor);
                
                return half4(finalColor, alpha * input.color.a);
            }
            ENDHLSL
        }
    }
    
    CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
}