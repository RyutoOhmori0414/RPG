#ifndef TOON_SHADER_VERTEX_CURVE_INCLUDED
#define TOON_SHADER_VERTEX_CURVE_INCLUDED

#include "Header.hlsl"
#include "Include.hlsl"

float3 VertexCurve(float3 positionWS)
{
    float distance = length(TransformWorldToView(positionWS).z - _CurveOffset);
    distance = pow(distance, _CurveFactor);
    positionWS.y += distance * -0.1F;
    return positionWS;
}

#endif