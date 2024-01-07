using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomGizmo
{
    private static Vector3[] CreateFanVertices(float angle, int triangleCount)
    {
        if (angle <= 0.0f || triangleCount <= 0)
        {
            return Array.Empty<Vector3>();
        }

        angle = Mathf.Min(angle, 360F);

        var vertices = new Vector3[triangleCount + 2];

        vertices[0] = Vector3.zero;

        float radian = angle * Mathf.Deg2Rad;
        float startRad = -radian / 2;
        float incRad = radian / triangleCount;

        for (int i = 1; i < triangleCount + 2; i++)
        {
            var currentRad = startRad + (incRad * i);

            Vector3 vert = new Vector3(Mathf.Sin(currentRad), 0.0F, Mathf.Cos(currentRad));
            vertices[i] = vert;
        }

        return vertices;
    }

    private static Mesh CreateFanMesh(float angle, int triangleCount)
    {
        var mesh = new Mesh();
        var vertices = CreateFanVertices(angle, triangleCount);

        var triangleIndices = new int[triangleCount * 3];

        var temp = 0;
        
        for (int i = 0; i < triangleIndices.Length; i += 3)
        {
            triangleIndices[i] = 0;
            triangleIndices[i + 1] = temp + 1;
            triangleIndices[i + 2] = temp + 2;
            temp++;
        }
        
        mesh.SetVertices(vertices);
        mesh.triangles = triangleIndices;
        
        mesh.RecalculateNormals();

        return mesh;
    }

    public static void DrawFunGizmo(Transform transform, float angle, float range)
    {
        var mesh = CreateFanMesh(angle * 2, 60);

        var pos = transform.position + Vector3.up * 0.01F;
        var rot = transform.rotation;
        Vector3 scale = Vector3.one * range;

        Gizmos.DrawMesh(mesh, pos, rot, scale);
    }
}
