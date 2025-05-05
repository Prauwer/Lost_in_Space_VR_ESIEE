Shader "EGA/Particles/FireSphere_URP"
{
    Properties
    {
        _MainTex("Main Tex", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _Emission("Emission", Float) = 2
        _StartFrequency("Start Frequency", Float) = 4
        _Frequency("Frequency", Float) = 10
        _Amplitude("Amplitude", Float) = 1
        [Toggle]_Usedepth("Use depth?", Float) = 0
        _Depthpower("Depth power", Float) = 1
        [Toggle]_Useblack("Use black", Float) = 0
        _Opacity("Opacity", Float) = 1
    }

    SubShader
    {
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 200
        Pass
        {
            Name "ForwardLit"
            Tags { "LightMode" = "UniversalForward" }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            Cull Back

            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Uniforms
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Emission;
            float _StartFrequency;
            float _Frequency;
            float _Amplitude;
            float _Usedepth;
            float _Depthpower;
            float _Opacity;
            float _Useblack;

            // Input structure
            struct Attributes
            {
                float4 positionOS : POSITION;
                float3 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float3 uv : TEXCOORD0;
                float4 color : COLOR;
                float4 screenPos : TEXCOORD1;
            };

            // Vertex function
            Varyings vert(Attributes v)
            {
                Varyings output;
                output.positionHCS = TransformObjectToHClip(v.positionOS);
                output.uv = v.uv;
                output.color = v.color;
                output.screenPos = ComputeScreenPos(output.positionHCS);
                return output;
            }

            // Fragment function
            half4 frag(Varyings i) : SV_Target
            {
                // Base color and emission calculation
                float4 temp_output = _Emission * _Color * i.color;
                float2 uv_mod = ((float2(0.2, 0) * _Time.y) + i.uv.xy + i.uv.z) * _StartFrequency;
                float2 cell = floor(uv_mod);
                float cell_index = cell.x + cell.y * 57.0;
                float2 frac_uv = frac(uv_mod);
                float2 perlin = frac_uv * frac_uv * (float2(3.0, 3.0) - frac_uv * 2.0);
                float random = frac(473.5 * sin(cell_index));

                float3 mod_uv = (float3(0.5, 0.5, 0) * _Time.y + (i.uv * (random * _Amplitude)) + i.uv.z) * _Frequency;
                float3 cell_3d = floor(mod_uv);
                float cell_index_3d = cell_3d.x + cell_3d.y * 57.0;
                float3 frac_uv_3d = frac(mod_uv);
                float3 perlin_3d = frac_uv_3d * frac_uv_3d * (float3(3.0, 3.0, 3.0) - frac_uv_3d * 2.0);

                // Texture sampling
                float4 tex_sample = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv.xy + 0.2 * random * _Amplitude);
                float4 color_output = lerp(temp_output, temp_output * tex_sample, _Useblack);

                // Depth blending
                float screen_depth = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.screenPos.xy));
                float object_depth = LinearEyeDepth(i.screenPos.z);
                float depth_blend = abs((screen_depth - object_depth) / _Depthpower);
                depth_blend = clamp(depth_blend, 0.0, 1.0);

                // Final color
                float opacity = clamp(i.color.a * tex_sample * _Opacity, 0.0, 1.0);
                opacity = lerp(opacity, opacity * depth_blend, _Usedepth);
                return half4(color_output.rgb, opacity);
            }
            ENDHLSL
        }
    }
    FallBack "Unlit/Transparent"
}
