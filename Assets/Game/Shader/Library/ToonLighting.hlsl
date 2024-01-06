#ifndef TOON_SHADER_TOONLIGHTING_INCLUDED
#define TOON_SHADER_TOONLIGHTING_INCLUDED

#include "Macro.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

Color4 CustomToonFragmentLighting(InputData inputData, SurfaceData surfaceData)
{
    return half4(surfaceData.albedo, surfaceData.alpha);
}

#endif