Shader "Custom/GaussianBlur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags 
        {
            "RenderType" = "Opaque"
            "Queue" = "Transparent"
        }
        
        Pass
        {
            Tags
            {
                "LightMode" = "AfterTransparentEffect"
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attribute
            {
                float4 positionOS : POSITION;
                half4 color : COLOR;
            };

            struct Varyings
            {
                float4 positionHCS : SV_POSITION;
                half4 vertexColor : COLOR;
                float4 positionSS : TEXCOORD0;
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            TEXTURE2D(_CustomGrabTex);
            SAMPLER(sampler_CustomGrabTex);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_ST;
            float4 _CustomGrabTex_TexelSize;
            CBUFFER_END
            
            Varyings vert (Attribute v)
            {
                Varyings o = (Varyings)0;
                o.positionHCS = TransformObjectToHClip(v.positionOS.xyz);
                o.positionSS = ComputeScreenPos(o.positionHCS);
                return o;
            }

            half4 frag (Varyings input) : SV_Target
            {
                // sample the texture
                // apply fog
                return SAMPLE_TEXTURE2D(_CustomGrabTex, sampler_CustomGrabTex, input.positionSS.xy / input.positionSS.w);
            }
            ENDHLSL
        }
    }
}
