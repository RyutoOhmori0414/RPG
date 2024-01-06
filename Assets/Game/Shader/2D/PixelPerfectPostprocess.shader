Shader "Hidden/PixelPerfectPostprocess"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Scale ("Scale", Float) = 1
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
            "RenderPipeline" = "UniversalPipeline"
            "Queue" = "Transparent"
        }
        
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM
            #pragma vertex Vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.core/Runtime/Utilities/Blit.hlsl"

            TEXTURE2D_X(_CameraOpaqueTexture);
            SAMPLER(sampler_CameraOpaqueTexture);
            
            float4 _CameraOpaqueTexture_TexelSize;
            float _Scale;
            
            half4 frag (Varyings i) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
                
                float2 scale = _CameraOpaqueTexture_TexelSize.xy * _Scale;
                float2 uv = (floor(i.texcoord / scale) + 0.5) * scale;
                
                half4 col = SAMPLE_TEXTURE2D_X(_BlitTexture, sampler_LinearRepeat, uv);
                return col;
            }
            ENDHLSL
        }
    }
}
