#ifndef CUSTOM_SHADER_TWIRL_INCLUDED
#define CUSTOM_SHADER_TWIRL_INCLUDED

inline float2 Twirl (float2 uv, float2 center, float Strength, float2 Offset)
{
    float2 delta = uv - center;
    float angle = Strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    return float2(x + center.x + Offset.x, y + center.x + Offset.y);
}

#endif