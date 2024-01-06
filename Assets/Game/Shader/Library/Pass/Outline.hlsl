#ifndef TOON_SHADER_OUTLINE_INCLUDED
#define TOON_SHADER_OUTLINE_INCLUDED

#include "../Include.hlsl"
#include "../Header.hlsl"
#include "../Macro.hlsl"

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;

#if defined(_CUSTOM_TOON_CURVED)
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    positionWS = VertexCurve(positionWS);
    input.positionOS.xyz = TransformWorldToObject(positionWS);
#endif
#if defined(_CUSTOM_OUTLINE_VERTEX_NORMAL_USED)
    input.positionOS.xyz += input.normalOS * _OutlineWidth;
#elif defined(_CUSTOM_OUTLINE_VERTEX_COLOR_USED)
    input.positionOS.xyz += input.vertexColor.xyz * _OutlineWidth;
#endif

    output.positionHCS = TransformObjectToHClip(input.positionOS.xyz);
    output.fogFactor = ComputeFogFactor(output.positionHCS.z);

    return output;
}

half4 frag(Varyings input) : SV_Target
{
    return half4(MixFog(_OutlineColor.rgb, input.fogFactor), _OutlineColor.a);
}

#endif