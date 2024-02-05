Shader "Custom/CurveToonShader"
{
    Properties
    {
        _BaseMap("Base Map", 2D) = "white" {}
        _BaseColor("Base Color", Color) = (1, 1, 1, 1)
        [Normal] _NormalMap("Normal Map", 2D) = "bump" {}
        _Metallic("Metallic", Range(0.0, 1.0)) = 0.0
        _Smoothness("Smoothness", Range(0.0, 1.0)) = 0.0
        [NoScaleOffset]_EmissionMap ("EmissionMap", 2D) = "white" {}
        [HDR]_EmissiveColor ("EmissiveColor", Color) = (0.0, 0.0, 0.0, 1.0)
        [Enum(UnityEngine.Rendering.CullMode)] _ForwardCullMode("Culling Mode", Float) = 2
        
        [Space(20)]
        [Header(AlphaClip)]
        [Toggle(_USE_ALPHA_CLIP_ON)] _USE_ALPHA_CLIP_ON("AlphaClip Enable", Float) = 1
        _AlphaThreshold ("Threshold", Range(0.0, 1.0)) = 0
        
        [Space(20)]
        [Header(Dithering)]
        [Toggle(_CUSTOM_TOON_DITHER_ON)] _CUSTOM_TOON_DITHER_ON("Dithering Enable", float) = 1
        _DitheringStart ("Start", Float) = 1.0
        _DitheringEnd ("End", Float) = 5.0
        
        [Space(20)]
        [Header(Shade1)]
        _Shade1Color ("Color", Color) = (0.9, 0.9, 0.9, 1.0)
        _Shade1Amount ("Amount", Range(0.0, 1.0)) = 0.5
        
        [Space(20)]
        [Header(Shade2)]
        _Shade2Color ("Color", Color) = (0.8, 0.8, 0.8, 1.0)
        _Shade2Amount ("Amount", Range(0.0, 1.0)) = 0.5
        
        [Space(20)]
        [Header(Rim)]
        _RimPower ("Power", Range(0.01, 5.0)) = 1
        _RimColor ("Color", Color) = (1.0, 1.0, 0.0, 1.0)
        _RimStrength ("Strength", Range(0.0, 1.0)) = 0.5
        
        [Space(20)]
        [Header(Vertex Curve)]
        [Toggle(_CUSTOM_TOON_CURVED)] _CUSTOM_TOON_CURVED("Curve Enable", float) = 0
        _CurveOffset ("Curve Offset", float) = 5
        _CurveFactor ("CurveFactor", Range(0.0, 3.0)) = 1.5
        
        [Space(20)]
        [Header(Outline Pass Command)]
        [Enum(UnityEngine.Rendering.CullMode)] _OutlineCull ("Culling", Float) = 2
        [Enum(UnityEngine.Rendering.CompareFunction)] _OutlineZTest ("ZTest", Float) = 4
        [Enum(Off, 0, On, 1)] _OutlineZWrite ("ZWrite", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] _OutlineSrcFactor ("Src Factor", Float) = 5
        [Enum(UnityEngine.Rendering.BlendMode)] _OutlineDstFactor ("Dst Factor", Float) = 10
        [KeywordEnum(Normal_Used, Color_Used)] _Custom_Outline_Vertex("UseNormalType", Int) = 0
        [IntRange]_OutlineStencilRef ("Stencil Ref", Range(0, 255)) = 1
        [Header(Outline Param)]
        [HDR] _OutlineColor ("Color", Color) = (0.0, 0.0, 0.0, 1)
        _OutlineWidth ("Width", Range(0.0, 0.1)) = 0.5
    }
    SubShader
    {
        
        Tags
        {
            "RenderType" = "Transparent"            "RenderPipeline" = "UniversalPipeline"
            "UniversalMaterialType" = "Lit"
            "IgnoreProjector" = "True"
            "Queue" = "Overlay"
        }
        
        // ForwardPass
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            Cull [_ForwardCullMode]
            
            Tags
            {
                "LightMode" = "UniversalForward"
            }
            
            Stencil
            {
                Ref [_OutlineStencilRef]
                Comp Always
                Pass Replace
            }
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // URP keywords
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
            #pragma multi_compile_fragment _ _ADDITIONAL_LIGHT_SHADOWS
            #pragma multi_compile_fragment _ _SHADOWS_SOFT
            #pragma multi_compile_fragment _ _SCREEN_SPACE_OCCLUSION
            #pragma multi_compile _ LIGHTMAP_SHADOW_MIXING
            #pragma multi_compile _ SHADOWS_SHADOWMASK

            // Unity Keywords
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile_fog

            // Custom Keyword
            #pragma multi_compile _ _CUSTOM_TOON_CURVED
            #pragma multi_compile _ _CUSTOM_TOON_DITHER_ON
            #pragma multi_compile _ _USE_ALPHA_CLIP_ON
            #define CUSTOM_TOON_PASS_UNIVERSAL_FORWARD

            #include "Library/Pass/UniversalForward.hlsl"
            ENDHLSL
        }
        
        // ShadowCasterPass
        Pass
        {
            Tags
            {
                "LightMode" = "ShadowCaster"
            }
            
            ZWrite On
            ZTest LEqual
            ColorMask 0
            Cull Off
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            #pragma multi_compile_instancing

            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            #pragma multi_compile_vertex _ _CASTING_PUNCTUAL_LIGHT_SHADOW

            // Custom Keyword
            #pragma multi_compile _ _CUSTOM_TOON_CURVED
            #define CUSTOM_TOON_PASS_UNIVERSAL_SHADOW_CASTER

            #include "Library/Pass/ShadowCaster.hlsl"
            ENDHLSL
        }
        
        // DepthOnlyPass
        Pass
        {
            Tags
            {
                "LightMode" = "DepthOnly"
            }
            
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite On
            ColorMask R
            Cull[_ForwardCullMode]
            
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            
            // Material Keywords
            #pragma shader_feature_local_fragment _ALPHATEST_ON
            #pragma shader_feature_local_fragment _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

            // Unity defined keywords
            #pragma multi_compile_fragment _ LOD_FADE_CROSSFADE

            // GPU Instancing
            #pragma multi_compile_instancing

            // Custom keywords
            #pragma multi_compile _ _CUSTOM_TOON_CURVED
            #pragma multi_compile _ _CUSTOM_TOON_DITHER_ON
            #pragma multi_compile _ _USE_ALPHA_CLIP_ON
            #define CUSTOM_TOON_PASS_UNIVERSAL_DEPTH_ONLY

            #include "Library/Pass/DepthOnly.hlsl"
            
            ENDHLSL
        }
        
        // Outline用Pass
        Pass
        {
            Tags
            {
                "LightMode" = "Outline"
            }
            
            Cull [_OutlineCull]
            ZTest [_OutlineZTest]
            ZWrite [_OutlineZWrite]
            Blend [_OutlineSrcFactor][_OutlineDstFactor]
            Stencil
            {
                Ref [_OutlineStencilRef]
                Comp NotEqual
                Pass Replace
            }
            
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            // Unity Keyword
            #pragma multi_compile_fog

            // Custom Keyword
            #pragma multi_compile _ _CUSTOM_TOON_CURVED
            #pragma multi_compile _CUSTOM_OUTLINE_VERTEX_NORMAL_USED _CUSTOM_OUTLINE_VERTEX_COLOR_USED
            #pragma multi_compile _ _CUSTOM_TOON_DITHER_ON
            #define CUSTOM_TOON_PASS_OUTLINE

            #include "Library/Pass/Outline.hlsl"
            ENDHLSL
        }
    }
}
