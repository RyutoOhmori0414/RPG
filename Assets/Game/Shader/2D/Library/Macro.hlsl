#ifndef CUSTOM_SHADER_MACRO_2D_INCLUDED
#define CUSTOM_SHADER_MACRO_2D_INCLUDED

inline half Remap (half value, half inMin, half inMax, half outMin, half outMax)
{
    return (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin;
}

inline float Remap (float value, float inMin, float inMax, float outMin, float outMax)
{
    return (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin;
}

#endif