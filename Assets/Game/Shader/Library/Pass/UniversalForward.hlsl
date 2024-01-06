#ifndef TOON_SHADER_UNIVERSALFORWARD_INCLUDED
#define TOON_SHADER_UNIVERSALFORWARD_INCLUDED

// インクルード
#include "../Include.hlsl"
#include "../Header.hlsl"
#include "../Macro.hlsl"

Varyings vert(Attributes input)
{
    Varyings output = (Varyings)0;

    output.texcoord = TRANSFORM_TEX(input.texcoord, _BaseMap);
    output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
    output.positionHCS = TransformWorldToHClip(output.positionWS);
    
    // 頂点カーブ
#if defined(_CUSTOM_TOON_CURVED)
    
    output.positionWS = VertexCurve(output.positionWS);
    output.positionHCS = TransformWorldToHClip(output.positionWS);
#endif
    
    output.normalWS = TransformObjectToWorldNormal(input.normalOS);
    output.tangentWS = TransformObjectToWorldDir(input.tangentOS.xyz);
    output.bitangentWS = cross(output.normalWS, output.tangentWS) * input.tangentOS.w;
    OUTPUT_LIGHTMAP_UV(input.lightmapUV, unity_LightmapST, output.lightmapUV);
    OUTPUT_SH(output.normalWS, output.vertexSH);
    output.fogFactor = ComputeFogFactor(output.positionHCS.z);
    output.vertexLight = VertexLighting(output.positionWS, output.normalWS);
    
    return output;
}

half4 frag(Varyings input) : SV_Target
{
    mainTexUV = input.texcoord;
    
    // surfaceDataを作成
    SurfaceData surfaceData;
    surfaceData.normalTS = UnpackNormal(SAMPLE_TEXTURE2D(_NormalMap, CUSTOM_TOON_SAMPLER_STATE_LINEAR_REPEAT, mainTexUV));
    half4 col = SAMPLE_TEXTURE2D(_BaseMap, CUSTOM_TOON_SAMPLER_STATE_LINEAR_REPEAT, mainTexUV) * _BaseColor;
    surfaceData.albedo = col.rgb;
    surfaceData.alpha = col.a;
    surfaceData.emission = 0.0H;
    surfaceData.metallic = _Metallic;
    surfaceData.occlusion = 1.0H;
    surfaceData.smoothness = _Smoothness;
    surfaceData.specular = 0.0H;
    surfaceData.clearCoatMask = 0.0H;
    surfaceData.clearCoatSmoothness = 0.0H;

    // InputDataを作成
    InputData inputData = (InputData)0;
    inputData.positionWS = input.positionWS;
    inputData.normalWS = TransformTangentToWorld(surfaceData.normalTS, half3x3(input.tangentWS.xyz, input.bitangentWS.xyz, input.normalWS.xyz));
    inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
    inputData.viewDirectionWS = SafeNormalize(GetWorldSpaceViewDir(input.positionWS));
    inputData.fogCoord = input.fogFactor;
    inputData.vertexLighting = input.vertexLight;
    inputData.bakedGI = SAMPLE_GI(input.lightmapUV, input.vertexSH, inputData.normalWS);
    inputData.normalizedScreenSpaceUV = GetNormalizedScreenSpaceUV(input.positionHCS);
    inputData.shadowMask = SAMPLE_SHADOWMASK(input.lightmapUV);
    #if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
    inputData.shadowCoord = input.shadowCoord;
    #elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
    inputData.shadowCoord = TransformWorldToShadowCoord(input.positionWS);
    #else
    inputData.shadowCoord = float4(0, 0, 0, 0);
    #endif

    half4 color = UniversalFragmentPBR(inputData, surfaceData);

    color.rgb = MixFog(color.rgb, inputData.fogCoord);
    
    return color;
}


#endif