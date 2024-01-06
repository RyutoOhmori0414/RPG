#ifndef TOON_SHADER_DEPTH_ONLY_PASS_INCLUDED
#define TOON_SHADER_DEPTH_ONLY_PASS_INCLUDED

#include "../Include.hlsl"
#include "../Header.hlsl"
#include "../Macro.hlsl"

Varyings vert (Attributes input)
{
    Varyings output = (Varyings)0;

    output.positionHCS = TransformObjectToHClip(input.positionOS);
    output.texcoord = input.texcoord;

    return output;
}

half frag(Varyings input) : SV_Target
{
#if defined(_USE_ALPHA_CLIP_ON)
    AlphaClip(SAMPLE_TEXTURE2D(_BaseMap, CUSTOM_TOON_SAMPLER_STATE_LINEAR_REPEAT, input.texcoord).a)
#endif
    return input.positionHCS.z;
}

#endif