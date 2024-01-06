#ifndef TOON_SHADER_HEADER_INCLUDED
#define TOON_SHADER_HEADER_INCLUDED

// Attributes構造体の要素を定義するためのキーワード
#define CUSTOM_TOON_ATTR_POSITION_OS_ON

#define CUSTOM_TOON_ATTR_NORMAL_OS_ON

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_ATTR_TANGENT_OS_ON
#endif

#if defined(CUSTOM_TOON_PASS_OUTLINE)
    #define CUSTOM_TOON_ATTR_COLOR_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD) || defined(CUSTOM_TOON_PASS_UNIVERSAL_SHADOW_CASTER)
    #define CUSTOM_TOON_ATTR_TEXCOORD0_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_ATTR_LIGHTMAP_TEXCOORD_ON
#endif

// Varyings構造体の要素を定義するためのキーワード
#define CUSTOM_TOON_VARY_POSITION_HCS_ON

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_NORMAL_WS_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD) || defined(CUSTOM_TOON_PASS_UNIVERSAL_SHADOW_CASTER) || defined(CUSTOM_TOON_PASS_UNIVERSAL_DEPTH_ONLY)
    #define CUSTOM_TOON_VARY_TEXCOORD0_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_POSITION_WS_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_TANGENT_WS_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_BITANGENT_WS_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD) || defined(CUSTOM_TOON_PASS_OUTLINE)
    #define CUSTOM_TOON_VARY_FOGFACTOR_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_VERTEX_LIGHT_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD) || defined(CUSTOM_TOON_PASS_OUTLINE)
    #define CUSTOM_TOON_VARY_POSITION_SS_ON
#endif

#if defined(_CUSTOM_TOON_DITHER_ON)
    #define CUSTOM_TOON_VARY_DITHERING_FACTOR_ON
#endif

#if defined(CUSTOM_TOON_PASS_UNIVERSAL_FORWARD)
    #define CUSTOM_TOON_VARY_LIGHTMAP_OR_SH_ON
#endif

// 仮
#if defined(CUSTOM_TOON_PASS_OUTLINE)
    #define CUSTOM_TOON_ATTR_TEXCOORD_XY_ON;
    #define CUSTOM_TOON_ATTR_TEXCOORD_ZW_ON;
#endif

// 頂点データ
struct Attributes
{
#if defined(CUSTOM_TOON_ATTR_POSITION_OS_ON)
    float4 positionOS : POSITION;
#endif
#if defined(CUSTOM_TOON_ATTR_NORMAL_OS_ON)
    float3 normalOS : NORMAL;
#endif
#if defined(CUSTOM_TOON_ATTR_TANGENT_OS_ON)
    float4 tangentOS : TANGENT;
#endif
#if defined(CUSTOM_TOON_ATTR_COLOR_ON)
    float4 vertexColor : COLOR;
#endif
#if defined(CUSTOM_TOON_ATTR_TEXCOORD0_ON) || defined(CUSTOM_TOON_PASS_UNIVERSAL_DEPTH_ONLY)
    float2 texcoord : TEXCOORD0;
#endif
#if defined(CUSTOM_TOON_ATTR_LIGHTMAP_TEXCOORD_ON)
    float2 lightmapUV : TEXCOORD1;
#endif

// 仮
#if defined(CUSTOM_TOON_ATTR_TEXCOORD_XY_ON)
    float2 outlineNormal_XY : TEXCOORD1;
#endif
#if defined(CUSTOM_TOON_ATTR_TEXCOORD_XY_ON)
    float2 outlineNormal_ZW : TEXCOORD2;
#endif
};

struct Varyings
{
#if defined(CUSTOM_TOON_VARY_POSITION_HCS_ON)
    float4 positionHCS : SV_POSITION;
#endif
#if defined(CUSTOM_TOON_VARY_TEXCOORD0_ON)
    float2 texcoord : TEXCOORD0;
#endif
#if defined(CUSTOM_TOON_VARY_POSITION_WS_ON)
    float3 positionWS : TEXCOORD1;
#endif
#if defined(CUSTOM_TOON_VARY_NORMAL_WS_ON)
    float3 normalWS : TEXCOORD2;
#endif
#if defined(CUSTOM_TOON_VARY_TANGENT_WS_ON)
    float3 tangentWS : TEXCOORD3;
#endif
#if defined(CUSTOM_TOON_VARY_BITANGENT_WS_ON)
    float3 bitangentWS : TEXCOORD4;
#endif
#if defined(CUSTOM_TOON_VARY_FOGFACTOR_ON)
    half fogFactor : TEXCOORD5;
#endif
#if defined(CUSTOM_TOON_VARY_VERTEX_LIGHT_ON)
    half3 vertexLight : TEXCOORD6;
#endif
#if defined(CUSTOM_TOON_VARY_POSITION_SS_ON)
    float4 positionSS : TEXCOORD7;
#endif
#if defined(CUSTOM_TOON_VARY_DITHERING_FACTOR_ON)
    float4 ditheringFactor : TEXCOORD8;
#endif
    
#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    float4 shadowCoord : TEXCOORD9;
#endif
#if defined(CUSTOM_TOON_VARY_LIGHTMAP_OR_SH_ON)
    DECLARE_LIGHTMAP_OR_SH(lightmapUV, vertexSH, 10);
#endif
};

// static
static float2 mainTexUV = float2(0, 0);

// uniform
TEXTURE2D(_BaseMap);
TEXTURE2D(_NormalMap);
TEXTURE2D(_EmissionMap);

SAMPLER(CUSTOM_TOON_SAMPLER_STATE_LINEAR_REPEAT);

// CBUFFER
CBUFFER_START(UnityPerMaterial)
float4 _BaseMap_ST;
half4 _BaseColor;
float4 _NormalMap_ST;
float _NormalMapScale;
float _Metallic;
float _Smoothness;

// AlphaClip
float _AlphaThreshold;

// Emissive
half3 _EmissiveColor;

// Dither
float _DitheringStart;
float _DitheringEnd;

// Shade1
half4 _Shade1Color;
float _Shade1Amount;
// Shade2
half4 _Shade2Color;
float _Shade2Amount;

// Rim
float _RimPower;
half4 _RimColor;
float _RimStrength;

// カーブ関係
float _CurveOffset;
float _CurveFactor;

// アウトライン関係
half4 _OutlineColor;
float _OutlineWidth;
CBUFFER_END

#endif