#ifndef TOON_SHADER_INCLUDE_INCLUDED
#define TOON_SHADER_INCLUDE_INCLUDED

// URP Include
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

// Custom Include
#include "Macro.hlsl"
#include "Header.hlsl"

#if defined(_CUSTOM_TOON_CURVED)
    #include "VertexCurve.hlsl"
#endif

#endif