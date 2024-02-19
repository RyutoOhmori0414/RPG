Shader "Custom/UI/Hologram"
{
    Properties
    {
        [PerRendererData]_MainTex ("Texture", 2D) = "white" {}
        
        [Header(Glitch)]
        _FrameRate ("FrameRate", Range(0.1, 30)) = 15
        _Frequency ("Frequency", Range(0, 1)) = 0.1
        [PowerSlider(1.5)]_GlitchStrength ("Strengh", Range(0, 1)) = 0.1
        
        [Header(Noise)]
        [HDR]_NoiseColor ("Color", Color) = (1, 1, 1, 1)
        _NoisePower ("Power", Range(1, 5)) = 1
        _NoiseStrength ("Strength", Float) = 1
        _NoiseScale ("Scale", Float) = 2
        _NoiseScrollSpeed ("ScrollSpeed", Float) = 2
        _NoiseDistance ("Distance", Range(0, 1)) = 0
        
        [Enum(UnityEngine.Rendering.CompareFunction)]_StencilComp ("Stencil Comparison", Float) = 8
        [IntRange]_Stencil ("Stencil ID", Range(0, 255)) = 0
        [Enum(UnityEngine.Rendering.StencilOp)]_StencilOp ("Stencil Operation", Float) = 0
        [IntRange]_StencilWriteMask ("Stencil Write Mask", Range(0, 255)) = 255
        [IntRange]_StencilReadMask ("Stencil Read Mask", Range(0, 255)) = 255

        [HideInInspector]_ColorMask ("Color Mask", Float) = 15

        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType"="Plane"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest Off
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog
            #pragma multi_compile_instancing

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Assets/Game/Shader/Library/Glitch.hlsl"
            #include "Assets/Game/Shader/Library/Macro.hlsl"

            struct Attributes
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                half4 color : COLOR;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                half4 color : COLOR;

                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;

            // Glitch

            float _FrameRate;
            float _Frequency;
            float _GlitchStrength;

            // Noise
            half3 _NoiseColor;
            float _NoisePower;
            float _NoiseStrength;
            float _NoiseScale;
            float _NoiseScrollSpeed;
            float _NoiseDistance;
            
            CBUFFER_END

            Varyings vert (Attributes input)
            {
                Varyings output = (Varyings)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_TRANSFER_INSTANCE_ID(input, output);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                output.vertex = TransformObjectToHClip(input.vertex.xyz);
                output.uv = TRANSFORM_TEX(input.uv, _MainTex);
                output.color = input.color;
                return output;
            }

            half4 frag (Varyings input) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);
                
                // sample the texture
                float2 glitchUV = Glitch(input.uv, _FrameRate, _Frequency, _GlitchStrength);
                half4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, glitchUV) * input.color;

                float noise = sin(input.uv.yy * _NoiseScale + _Time.y * _NoiseScrollSpeed);
                noise = (noise + 1) / 2;
                noise = remap(clamp(noise, _NoiseDistance, 1), _NoiseDistance, 1, 0, 1);
                noise *= rand(input.uv + _Time.xx);
                noise = pow(saturate(noise), _NoisePower);
                noise *= _NoiseStrength;
                
                col.rgb = _NoiseColor * noise;
                col.a = noise;
                col.rgb *= col.a;

                return col;
            }
            ENDHLSL
        }
    }
}
