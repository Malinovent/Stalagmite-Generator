using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGenerator2D : AbstractMeshGenerator
{
    [SerializeField] public int resolution = 20;

    [SerializeField] public float xScale = 1;
    [SerializeField] public float yScale = 1;

    [SerializeField] public float meshHeight = 1;

    [SerializeField, Range(1, 8)] public int octaves = 1;
    [SerializeField] public float lacunarity = 2;
    [SerializeField, Range(0, 1)] public float gain = 0.5f;
    [SerializeField] public float perlinScale = 1;

    [SerializeField] bool uvFollowSurface;
    [SerializeField] float uvScale = 1;
    [SerializeField] float numTexPerSquare = 1;

    [SerializeField] private int sortingOrder = 1;

    protected override void SetMeshNums()
    {
        numVertices = 2 * resolution;
        numTriangles = 6 * (resolution - 1);
    }

    protected override void SetVertices()
    {
        float x, y = 0;
        Vector3[] vs = new Vector3[numVertices];

        NoiseGenerator noise = new NoiseGenerator(octaves, gain, lacunarity, perlinScale);

        for (int i = 0; i < resolution; i++)
        {
            x = ((float)i / resolution) * xScale;
            y = yScale * noise.GetFractalNoise(x, 0);

            //top
            vs[i] = new Vector3(x, y, 0);
            //bottom
            vs[i + resolution] = new Vector3(x, y - meshHeight, 0);

        }

        vertices.AddRange(vs);
    }


    protected override void SetTriangles()
    {
        for (int i = 0; i < resolution - 1; i++)
        {
            triangles.Add(i);
            triangles.Add(i + resolution + 1);
            triangles.Add(i + resolution);
        }

        for (int i = 0; i < resolution - 1; i++)
        {
            triangles.Add(i);
            triangles.Add(i + 1);
            triangles.Add(i + resolution + 1);
        }
    }


    protected override void SetUVs()
    {
        meshRenderer.sortingOrder = sortingOrder;

        Vector2[] uvArrays = new Vector2[numVertices];

        if (uvFollowSurface)
        {
            for (int i = 0; i < resolution; i++)
            {
                uvArrays[i] = new Vector2(i * numTexPerSquare, 1);
                uvArrays[i + resolution] = new Vector2(i * numTexPerSquare, 0);
            }
        }
        else
        {
            for (int i = 0; i < resolution; i++)
            {
                uvArrays[i] = new Vector2(vertices[i].x / uvScale, vertices[i].y / uvScale);
                uvArrays[i + resolution] = new Vector2(vertices[i].x / uvScale, vertices[i + resolution].y / uvScale);
            }
        }

        uvs.AddRange(uvArrays);
    }

    protected override void SetNormals()
    {
        SetGeneralNormals();
    }

    protected override void SetTangents()
    {
        SetGeneralTangents();
    }


    protected override void SetVertexColours()
    {
        
    }


}
