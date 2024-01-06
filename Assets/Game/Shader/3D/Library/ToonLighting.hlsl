#ifndef TOON_SHADER_TOONLIGHTING_INCLUDED
#define TOON_SHADER_TOONLIGHTING_INCLUDED

#include "Macro.hlsl"
#include "Header.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

struct ToonLightingData
{
    float diffuse;
    half3 color;
};

half4 RimLight(InputData inputData)
{
    float3 viewDirWS = GetWorldSpaceViewDir(inputData.positionWS);
    float3 normalWS = inputData.normalWS;
    float VdotN = saturate(dot(viewDirWS, normalWS));

    VdotN = 1 - VdotN;

    return _RimColor * pow(VdotN, _RimPower) * _RimStrength;
}

void CalcDiffuse(inout ToonLightingData lightingData, Light light, InputData inputData, SurfaceData surfaceData)
{
    float attenuation = light.shadowAttenuation * light.distanceAttenuation;
    float NdotL = saturate((dot(inputData.normalWS, light.direction) + 1) / 2);

    lightingData.diffuse = max(lightingData.diffuse, NdotL * attenuation);
    lightingData.color += light.color;
}

half4 CalcFinalDiffuse(ToonLightingData lightingData, InputData inputData, SurfaceData surfaceData)
{
    half4 color = half4(surfaceData.albedo, surfaceData.alpha);
    
    if (lightingData.diffuse < _Shade2Amount)
    {
        float threshold = Remap(lightingData.diffuse, 0, _Shade2Amount, 0, 1);
        
        if (threshold < _Shade1Amount)
        {
            color.rgb *= _Shade1Color.rgb;
        }
        else
        {
            color.rgb *= _Shade2Color.rgb;
        }
    }

    color += RimLight(inputData);
    color.rgb += surfaceData.emission;
    
    return color;
}

half4 CustomToonFragmentLighting(InputData inputData, SurfaceData surfaceData)
{
    half4 col = half4(surfaceData.albedo, surfaceData.alpha);
    col += RimLight(inputData);

    ToonLightingData lightingData = (ToonLightingData)0;
    half4 shadowMask = CalculateShadowMask(inputData);
    AmbientOcclusionFactor aoFactor = CreateAmbientOcclusionFactor(inputData, surfaceData);
    uint meshRenderingLayers = GetMeshRenderingLayer();
    Light mainLight = GetMainLight(inputData, shadowMask, aoFactor);

    MixRealtimeAndBakedGI(mainLight, inputData.normalWS, inputData.bakedGI);
    
#ifdef _LIGHT_LAYERS
    if (IsMatchingLightLayer(mainLight.layerMask, meshRenderingLayers))
#endif
    {
        CalcDiffuse(lightingData, mainLight, inputData, surfaceData);
    }

    #if defined(_ADDITIONAL_LIGHTS)
    uint pixelLightCount = GetAdditionalLightsCount();

    #if USE_FORWARD_PLUS
    for (uint lightIndex = 0; lightIndex < min(URP_FP_DIRECTIONAL_LIGHTS_COUNT, MAX_VISIBLE_LIGHTS); lightIndex++)
    {
        FORWARD_PLUS_SUBTRACTIVE_LIGHT_CHECK

        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

#ifdef _LIGHT_LAYERS
        if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
#endif
        {
            CalcDiffuse(lightingData, light, inputData, surfaceData);
        }
    }
    #endif

    LIGHT_LOOP_BEGIN(pixelLightCount)
        Light light = GetAdditionalLight(lightIndex, inputData, shadowMask, aoFactor);

#ifdef _LIGHT_LAYERS
    if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers))
#endif
    {
        CalcDiffuse(lightingData, light, inputData, surfaceData);
    }
    LIGHT_LOOP_END
    #endif

    #if defined(_ADDITIONAL_LIGHTS_VERTEX)
    lightingData.vertexLightingColor += inputData.vertexLighting * lightingData.diffuse;
    #endif

#if REAL_IS_HALF
    // Clamp any half.inf+ to HALF_MAX
    return min(CalcFinalDiffuse(lightingData, inputData, surfaceData), HALF_MAX);
#else
    return CalcFinalDiffuse(lightingData, inputData, surfaceData);
#endif
}

#endif