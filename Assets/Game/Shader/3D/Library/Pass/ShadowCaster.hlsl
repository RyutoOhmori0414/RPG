#ifndef TOON_SHADER_SHADOW_CASTER_INCLUDED
#define TOON_SHADER_SHADOW_CASTER_INCLUDED

#include "../Include.hlsl"
#include "../Header.hlsl"
#include "../Macro.hlsl"

float3 _LightDirection;
float3 _LightPosition;

float4 GetShadowPositionHClip(Attributes input)
{
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    float3 normalWS = TransformObjectToWorldNormal(input.normalOS);

#if _CASTING_PUNCTUAL_LIGHT_SHADOW
    float3 lightDirectionWS = normalize(_LightPosition - positionWS);
#else
    float3 lightDirectionWS = _LightDirection;
#endif

    float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));

#if UNITY_REVERSED_Z
    positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#else
    positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
#endif

    return positionCS;
}

Varyings vert(Attributes input)
{
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input);

    output.texcoord = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionHCS = GetShadowPositionHClip(input);
    return output;
}

half4 frag(Varyings input) : SV_TARGET
{
    #ifdef LOD_FADE_CROSSFADE
    LODFadeCrossFade(input.positionCS);
    #endif

    return 0;
}

#endif