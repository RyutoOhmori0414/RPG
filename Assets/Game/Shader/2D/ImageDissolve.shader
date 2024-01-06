Shader "Custom/ImageDissolveOutline"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        [Space(20)]
        [Header(Outline)]
        _OutlineRange ("Range", Range(0.0, 1.0)) = 0.0
        [HDR]_OutlineColor ("Color", Color) = (1, 1, 0, 1)
        [Space(20)]
        [Header(Dissolve)]
        _DissolveTex ("Texture", 2D) = "white" {}
        _DissolveAmount ("Amount", Range(0.0, 1.0)) = 0.2
        [HDR]_DissolveColor ("Color", Color) = (1.2, 0, 1.2,1)
        _ScrollSpeed("ScrollSpeed", Vector) = (1, 1, 0, 0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP Include
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            // Custom Include
            #include "Library/Macro.hlsl"

            struct Attribute
            {
                float4 positionOS : POSITION;
                float2 texcoord : TEXCOORD0;
                half4 vertColor : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                float2 texCoord : TEXCOORD0;
                float2 dissolveTexcoord : TEXCOORD1;
                half4 vertColor : COLOR;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            // Dissolve
            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);
            
            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            half4 _TextureSampleAdd;

            // Outline
            float _OutlineRange;
            half4 _OutlineColor;
            
            // Dissolve
            float4 _DissolveTex_ST;
            float _DissolveAmount;
            half4 _DissolveColor;
            half4 _ScrollSpeed;
            
            CBUFFER_END

            Varyings vert (Attribute input)
            {
                Varyings output = (Varyings)0;

                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.texCoord = TRANSFORM_TEX(input.texcoord, _MainTex);
                output.dissolveTexcoord = TRANSFORM_TEX(input.texcoord, _DissolveTex) + _Time.xx * _ScrollSpeed.xy;
                output.vertColor = input.vertColor;

                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                half alpha = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(input.texCoord.x + _OutlineRange / 10, input.texCoord.y)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(input.texCoord.x, input.texCoord.y + _OutlineRange / 10)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(input.texCoord.x - _OutlineRange / 10, input.texCoord.y)).a +
                    SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, float2(input.texCoord.x, input.texCoord.y - _OutlineRange / 10)).a;

                alpha = saturate(alpha);

                half dissolve = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, input.dissolveTexcoord).r;

                half4 col = dissolve < _DissolveAmount ? half4(_OutlineColor.rgb, alpha) : half4(_DissolveColor.rgb, alpha);

                half4 overCol = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.texCoord) + _TextureSampleAdd) * input.vertColor;

                col = overCol.a > 0.0 ? overCol : col;
                
                return col;
            }
            
            ENDHLSL
        }
    }
}
