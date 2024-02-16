Shader "Custom/Wireframe"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WireWidth ("Width", Range(0, 800)) = 100
        _FrontColor ("Front", Color) = (1, 0, 0, 1)
        _BackColor ("Back", Color) = (1, 1, 0, 1)
    }
    SubShader
    {
        Tags
        {
            "RenderType"="Transparent"
        }

        Pass
        {
            Cull Off
            Blend SrcAlpha OneMinusSrcAlpha
            
            HLSLPROGRAM
            #pragma require geometry
            
            #pragma vertex vert
            #pragma geometry geom
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2g
            {
                float4 positionCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;

                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct g2f
            {
                float4 positionHCS : SV_POSITION;
                float3 positionWS : TEXCOORD0;
                float4 dist : TEXCOORD1;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);

            CBUFFER_START(UnityPerMaterial)

            half4 _FrontColor;
            half4 _BackColor;
            float _WireWidth;
            
            CBUFFER_END

            v2g vert (Attributes input)
            {
                v2g output = (v2g)0;

                UNITY_SETUP_INSTANCE_ID(input);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
                
                output.positionWS = TransformObjectToWorld(input.positionOS.xyz);
                output.positionCS = TransformWorldToHClip(output.positionWS);
                return output;
            }

            #define CalcWireframe(pos)\
                o.positionWS = pos;\
                o.positionHCS = TransformWorldToHClip(pos);\
                triangleStream.Append(o);
            
            [maxvertexcount(3)]
            void geom(triangle v2g i[3], inout TriangleStream<g2f> triangleStream)
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i[0]);

                float2 p0 = i[0].positionCS.xy / i[0].positionCS.w;
                float2 p1 = i[1].positionCS.xy / i[1].positionCS.w;
                float2 p2 = i[2].positionCS.xy / i[2].positionCS.w;

                float2 edge0 = p2 - p1;
                float2 edge1 = p2 - p0;
                float2 edge2 = p1 - p0;

                float area = abs(edge1.x * edge2.y - edge1.y * edge2.x);
                float wireThickness = 800 - _WireWidth;

                g2f o;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.positionWS = i[0].positionWS;
                o.positionHCS = i[0].positionCS;
                o.dist.xyz = float3( (area / length(edge0)), 0.0, 0.0) * o.positionHCS.w * wireThickness;
                o.dist.w = 1.0 / o.positionHCS.w;
                triangleStream.Append(o);

                o.positionWS = i[1].positionWS;
                o.positionHCS = i[1].positionCS;
                o.dist.xyz = float3(0.0, (area / length(edge1)), 0.0) * o.positionHCS.w * wireThickness;
                o.dist.w = 1.0 / o.positionHCS.w;
                triangleStream.Append(o);

                o.positionWS = i[2].positionWS;
                o.positionHCS = i[2].positionCS;
                o.dist.xyz = float3(0.0, 0.0, (area / length(edge2))) * o.positionHCS.w * wireThickness;
                o.dist.w = 1.0 / o.positionHCS.w;
                triangleStream.Append(o);
            }

            half4 frag (g2f i, half facing : VFACE) : SV_Target
            {
                float minDistanceToEdge = min(i.dist[0], min(i.dist[1], i.dist[2])) * i.dist[3];

                // Early out if we know we are not on a line segment.
                if(minDistanceToEdge > 0.9)
                {
                    discard;
                    return half4(0,0,0,0);
                }

                // Smooth our line out
                float t = exp2(-2 * minDistanceToEdge * minDistanceToEdge);

                

                half4 finalColor = step(0.9F, t) * facing > 0 ? _FrontColor : _BackColor;
                finalColor.a = t;

                clip(finalColor.a - 0.9F);
                
                return finalColor;
            }
            ENDHLSL
        }
    }
}
