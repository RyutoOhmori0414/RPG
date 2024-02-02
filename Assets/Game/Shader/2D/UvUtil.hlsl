#ifndef UVUTIL_LIBRARY_INCLUDED
#define UVUTIL_LIBRARY_INCLUDED

float4 _textureRect;

int IsInner(float2 uv, float2 allasSize, float4 textureRect)
{
    float width = allasSize.x;
    float minX = textureRect.x / width;
    float maxX = (textureRect.x + textureRect.z) / width;

    int insideOfLeftEdge = step(minX, uv.x);
    int insideOfRightEdge = step(uv.x, maxX);

    float height = allasSize.y;
    float minY = textureRect.y / height;
    float maxY = (textureRect.y + textureRect.w) / height;

    int insideOfBottomEdge = step(minY, uv.y);
    int insideOfTopEdge = step(uv.y, maxY);

    return insideOfLeftEdge * insideOfRightEdge * insideOfBottomEdge * insideOfTopEdge;
}

#define remap(value, inMin, inMax, outMin, outMax) (value - inMin) * ((outMax - outMin) / (inMax - inMin)) + outMin

float2 AtlasUVtoMeshUV(float2 uv, float2 allasSize, float4 textureRect)
{
    float u = uv.x;
    float width = allasSize.x;
    float minX = textureRect.x / width;
    float maxX = (textureRect.x + textureRect.z) / width;
    u = remap(u, minX, maxX, 0, 1);

    float v = uv.y;
    float height = allasSize.y;
    float minY = textureRect.y / height;
    float maxY = (textureRect.y + textureRect.w) / height;
    v = remap(v, minY, maxY, 0, 1);

    float2 localUv = float2(u, v);
    return localUv;
}

float2 MeshUVtoAtlasUV(float2 localUV, float2 allasSize, float4 textureRect)
{
    float width = textureRect.z;
    // atlas内のpixel座標を求める
    float x = textureRect.x + width * localUV.x;
    float height = textureRect.w;
    float y = textureRect.y + height * localUV.y;

    // 0 〜 1に正規化する
    return float2(x / allasSize.x, y / allasSize.y);
}

#endif