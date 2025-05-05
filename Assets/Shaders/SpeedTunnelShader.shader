Shader "Custom/RadialBlur"
{
    Properties
    {
        _MainTex ("Base Texture", 2D) = "white" {}
        _BlurStrength ("Blur Strength", Float) = 0.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize; // Provides texture size information
            float _BlurStrength;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 center = float2(0.5, 0.5); // Center of the screen
                float2 direction = i.uv - center;
                float distance = length(direction);

                // Normalize direction
                direction = normalize(direction);

                fixed4 color = fixed4(0, 0, 0, 0);
                int samples = 8; // Number of blur samples

                // Accumulate colors
                for (int j = 0; j < samples; j++)
                {
                    float t = distance * _BlurStrength * (j / (float)samples);
                    float2 sampleUV = i.uv - direction * t;
                    color += tex2D(_MainTex, sampleUV);
                }

                // Average the color
                color /= samples;

                return color;
            }
            ENDCG
        }
    }
}
