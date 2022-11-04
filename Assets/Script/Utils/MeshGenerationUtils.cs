using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MeshGenerationUtils
{ 
    public static Vector3[] DrawCircle(float radius = 1, int vertexCount = 6, float height = 0)
    {
        Vector3[] vertices = new Vector3[vertexCount];

        float degrees = 360 / vertexCount;

        for (int i = 0; i < vertices.Length; i++)
        {
            float value = degrees * i * Mathf.Deg2Rad;

            float z = Mathf.Sin(value) * radius;
            float x = Mathf.Cos(value) * radius;

            vertices[i] = new Vector3(x, height, z);
        }

        return vertices;
    }
}
