#ifndef TOON_SHADER_ALPHA_CLIP_INCLUDED
#define TOON_SHADER_ALPHA_CLIP_INCLUDED

inline void AlphaClip(float alpha)
{
    clip(alpha - _AlphaThreshold);
}

#endif