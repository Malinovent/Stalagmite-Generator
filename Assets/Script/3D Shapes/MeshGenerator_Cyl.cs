using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator_Cyl : MonoBehaviour
{
    Mesh mesh;

    List<Vector3> vertices = new List<Vector3>();
    int[] triangles;

    public float initialRadius = 1;
    public float lastRadius = 0.2f;
    public int rings = 6;
    public int segments = 8;
    public float distBetweenRings = 1;

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;

        DrawCircles(segments, rings, distBetweenRings, initialRadius);
    }

    private void Update()
    {
        UpdateMesh();
    }

    void DrawCircles(int segments, int rings, float yDist, float radius = 1)
    {
        float radiusDecrement = (initialRadius - lastRadius) / rings;
        for (int i = 0; i < rings; i++)
        {
            vertices.AddRange(DrawCircle(radius, segments, yDist * i));
            radius -= radiusDecrement;
        }
    }


    Vector3[] DrawCircle(float radius = 1, int vertexCount = 6, float height = 0)
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

    void UpdateMesh()
    {
        mesh.Clear();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles;

        mesh.RecalculateNormals();

    }

    private void OnDrawGizmos()
    {
        if(vertices.Count <= 0) { return; }

        foreach (Vector3 vertex in vertices)
        {
            Gizmos.DrawSphere(vertex, 0.1f);
        }
    }
}
