Shader "Custom/NovelUIShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Texture", 2D) = "white" {}
        
        [Header(Common)]
        [KeywordEnum(NONE, DISSOLVE, GLITCH)] _Effect ("Effect Type", int) = 0
        
        [Header(Dissolve)]
        _Amount ("Effect Amount", Range(0, 1)) = 0.3
        _Range ("Effect Range", Range(0, 1)) = 0.3
        _DissolveTex ("DissolveTex", 2D) = "white" {}
        [HDR]_GlowColor ("Glow Color", Color) = (1, 0, 0, 1)
        _YRange ("Y Range", Range(0, 1)) = 0.3
        _Distortion ("Distortion", Range(0, 1)) = 0.3
        _Scroll ("Scroll", Vector) = (0, 4, 0, 0)
        
        [Header(Glitch)]
        _ScanLineJitter ("ScanLine Jitter", Vector) = (1, 0, 0, 0)
        _JitterSize ("Jitter Size", Range(1, 300)) = 50
        _VerticalJump ("Vertical Jump", Vector) = (0.5, 0, 0, 0)
        _HorizontalShake ("Horizontal Shake", Range(0, 1)) = 0.5
        _ColorDriftAmount ("Color Drift Amount", Range(-2, 2)) = 0
        _ColorDriftTime ("Color Drift Time", Float) = 0
            
        [HDR]_ScanlineColor ("Scanline Color", Color) = (0.8, 0.8, 0.8, 1)
        _ScanlineSize ("ScanlineSize", Float) = 1
        _ColorStrength ("Color Strength", Range(1, 5)) = 2
        
        // Stencil
        [HideInInspector] _StencilComp("StencilCinp", Float) = 8
        [HideInInspector] _Stencil("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
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
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One OneMinusSrcAlpha
        ColorMask [_ColorMask]

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "UvUtil.hlsl"
            
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma multi_compile_local _ USE_ATLAS

            #pragma multi_compile _EFFECT_NONE _EFFECT_DISSOLVE _EFFECT_GLITCH

            struct Attribute
            {
                float4 positionOS : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                half4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float4 worldPosition : TEXCOORD1;
                half4 mask : TEXCOORD2;
                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_DissolveTex);
            SAMPLER(sampler_DissolveTex);
            
            half4 _TextureSampleAdd;

            CBUFFER_START(UnityPerMaterial)

            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _DissolveTex_ST;
            float4 _ClipRect;
            float _UIMaskSoftnessX;
            float _UIMaskSoftnessY;

            // Effect Common
            float _Amount;
            float _Range;
            
            // Dissolve
            float3 _GlowColor;
            float _YRange;
            float4 _Scroll;
            float _Distortion;

            // Glitch
            float2 _ScanLineJitter; // (displacement, threshold)
            float _JitterSize;
            float2 _VerticalJump; // (amount, time)
            float _HorizontalShake;
            float _ColorDriftAmount; // (amount, time)
            float _ColorDriftTime;
            
            float4 _ScanlineColor;
            float _ScanlineSize;
            float _ColorStrength;
            
            CBUFFER_END

            float nrand(float x, float y)
            {
                return frac(sin(dot(float2(x, y), float2(12.9898, 78.233))) * 43758.5453);
            }
            
            Varyings vert(Attribute input)
            {
                Varyings output = (Varyings)0;
                UNITY_VERTEX_INPUT_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
                output.worldPosition = input.positionOS;

                float2 pixelSize = input.positionOS.w;
                pixelSize /= float2(1, 1) * abs(mul((float2x2)UNITY_MATRIX_P, _ScreenParams.xy));

                float4 clampedRect = clamp(_ClipRect, -2e10, 2e10);
                float2 maskUV = (input.positionOS.xy - clampedRect.xy) / (clampedRect.zw - clampedRect.xy);
                output.texcoord = TRANSFORM_TEX(input.texcoord.xy, _MainTex);
                output.mask = half4(input.positionOS.xy * 2 - clampedRect.xy - clampedRect.zw,
                                 0.25 / (0.25 * half2(_UIMaskSoftnessX, _UIMaskSoftnessY) + abs(pixelSize.xy)));

                
                output.texcoord = TRANSFORM_TEX(output.texcoord, _MainTex);
                output.color = input.color;

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                float2 uvForDissolveTex = input.texcoord;
                #if defined(USE_ATLAS)
                    // atlasを使っている場合、0.2 〜 0.6 のような中途半端なuv値が渡ってくる
                    // それを0 〜 1の正規化された情報に変形する
                    uvForDissolveTex = AtlasUVtoMeshUV(input.texcoord, _MainTex_TexelSize.zw, _textureRect);
                #endif
                
                half4 color = (half4)0;
                
                #if defined(_EFFECT_DISSOLVE)
                _Amount = remap(_Amount, 0, 1, -_YRange, 1 + _YRange);
                float fromY = _Amount - _YRange;

                float alphaY = remap(uvForDissolveTex.y, fromY, _Amount, 0, 1);
                alphaY = saturate(alphaY);

                uvForDissolveTex += _Scroll * _Time.x;
                half dissolveTexAlpha = SAMPLE_TEXTURE2D(_DissolveTex, sampler_DissolveTex, uvForDissolveTex).r;
                half reverseAlphaY = 1 - alphaY;

                half2 uvDiff = input.texcoord + half2(0, reverseAlphaY * dissolveTexAlpha * _Distortion);
                color = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uvDiff) + _TextureSampleAdd) * input.color;

                #if defined(USE_ATLAS)
                    color *= IsInner(uvDiff, _MainTex_TexelSize.zw, _textureRect);
                #endif
                
                dissolveTexAlpha *= alphaY;
                _Range *= reverseAlphaY;
                if (dissolveTexAlpha < reverseAlphaY + _Range)
                {
                    color.rgb += _GlowColor;
                }

                if (dissolveTexAlpha < reverseAlphaY)
                {
                    color = 0;
                }
                #elif defined(_EFFECT_GLITCH)
                const float u = input.texcoord.x;
                const float v = input.texcoord.y;

                float jitter = nrand(floor(v * _JitterSize * 2), _Time.x) * 2 - 1;
                jitter *= step(_ScanLineJitter.y, abs(jitter)) * _ScanLineJitter.x;

                float jump = lerp(v, frac(v + _VerticalJump.y), _VerticalJump.x);
                float shake = (nrand(_Time.x, 2) - 0.5) * _HorizontalShake;

                float drift = sin(jump + _ColorDriftTime) * _ColorDriftAmount;

                float u1 = saturate(u + jitter + shake);
                float u2 = saturate(u + jitter + shake + drift);
                float vv = saturate(jump);
                half4 src1 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(float2(u1, vv)));
                half4 src2 = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, frac(float2(u2, vv)));
                color = half4(src1.r, src2.g, src1.b, src1.a);

                float scanline = sin(v * _ScreenParams.y / _ScanlineSize + _Time.x * 400) * 0.5 + 0.5;
                scanline *= color.a;
                color = lerp(color, color * _ScanlineColor, scanline);
                color.rgb *= _ColorStrength;
                color.a *= input.color.a;
                #else
                color = (SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, input.texcoord) + _TextureSampleAdd) * input.color;
                #endif
                
                #ifdef UNITY_UI_CLIP_RECT
                half2 m = saturate((_ClipRect.zw - _ClipRect.xy - abs(input.mask.xy)) * input.mask.zw);
                color.a *= m.x * m.y;
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                clip (color.a - 0.001);
                #endif
                
                
                color.rgb *= color.a;
                return color;
            }
            
            ENDHLSL
        }
    }
}
