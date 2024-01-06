using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CustomGizmo
{
    private static Mesh CreateFunMesh(float angle, int triangleCount)
    {
        var mesh = new Mesh();
        
        //mesh.SetVertices();
        //mesh.SetTriangles();
        
        mesh.RecalculateNormals();

        return mesh;
    }

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

        return vertices;
    }
}
