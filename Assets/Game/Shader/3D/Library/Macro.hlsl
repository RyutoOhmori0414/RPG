#ifndef TOON_SHADER_MACRO_INCLUDED
#define TOON_SHADER_MACRO_INCLUDED

// デフォルト関数の拡張
#define max3(a, b, c) max(max(a, b), c)
#define max4(a, b, c, d) max(max(a, b), max(c, d))

// 型のラッピング
// 色
#define Color3 half3
#define Color4 half4

// ベクトル
#define Vector3 float3
#define Vector4 float4

// スカラー
#define Scalar half
#define Scalar2 half2
#define Scalar3 half3
#define Scalar4 half4

inline half Remap (half value, half inMin, half inMax, half outMin, half outMax)
{
    return (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin;
}

inline float Remap (float value, float inMin, float inMax, float outMin, float outMax)
{
    return (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin;
}

#endif