#ifndef TOON_SHADER_DITHERING_INCLUDED
#define TOON_SHADER_DITHERING_INCLUDED

#include "Include.hlsl"

static const float4x4 DITHER_PATTERN =
{
    1, 8, 2, 10,
    12, 4, 14, 6,
    3, 11, 1, 9,
    15, 7, 13, 5
};

static const int PATTERN_ROW_COUNT = 4;

void DitherTest(float4 positionSS, float ditherAmount)
{
    float2 screenPos = positionSS.xy / positionSS.w;
    float2 screenPosInPixel = screenPos.xy * _ScreenParams.xy;

    int ditherUV_x = (int)fmod(screenPosInPixel.x, PATTERN_ROW_COUNT);
    int ditherUV_y = (int)fmod(screenPosInPixel.y, PATTERN_ROW_COUNT);
    float dither = DITHER_PATTERN[ditherUV_x][ditherUV_y] / 16;

    clip(ditherAmount - dither);
}

float ComputeDitheringFactor(float3 positionWS)
{
    float dist = distance(GetCameraPositionWS(), positionWS);
    float diff = _DitheringEnd - _DitheringStart;

    dist -= _DitheringStart;
    float value = saturate(dist / diff);

    return value;
}

#endif