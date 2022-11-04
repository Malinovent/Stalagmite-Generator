using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralLandscapeGenerator : AbstractLandscapeMeshGenerator
{
    //[SerializeField] public int xResolution = 20;
    //[SerializeField] public int zResolution = 20;

    //[SerializeField] public float meshScale = 1;
    //[SerializeField] public float yScale = 1;


    //[SerializeField, Range(1, 8)] public int octaves = 1;
    //[SerializeField] public float lacunarity = 2;
    //[SerializeField, Range(0, 1)] public float gain = 0.5f;
    //[SerializeField] public float perlinScale = 1;

    //[SerializeField] FallOffType type;
    //[SerializeField] float fallOffSize = 1;
    //[SerializeField] float seaLevel = 0;

    [SerializeField] float uvScale = 1;

    [SerializeField] private Gradient gradient;
    [SerializeField] private int gradMin;
    [SerializeField] private int gradMax;

    protected override void SetMeshNums()
    {
        numVertices = (xResolution + 1) * (zResolution + 1);
        numTriangles = 6 * xResolution * zResolution;
    }

    protected override void SetVertices()
    {
        float xx, y, zz = 0;
        NoiseGenerator noise = new NoiseGenerator(octaves, gain, lacunarity, perlinScale);

        for (int z = 0; z <= zResolution; z++)
        {
            for (int x = 0; x <= xResolution; x++)
            {
                xx = ((float)x / xResolution) * meshScale;
                zz = ((float)z / zResolution) * meshScale;

                y = yScale * noise.GetFractalNoise(xx, zz);
                y = FallOff((float)x, y, (float)z);

                vertices.Add(new Vector3(xx, y, zz));
            }

        }
    }

    protected override void SetTriangles()
    {
        int triCount = 0;

        for (int z = 0; z < zResolution; z++)
        {
            for (int x = 0; x < xResolution; x++)
            {
                //Triangle 1
                triangles.Add(triCount);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + 1);

                //Triangle 2
                triangles.Add(triCount + 1);
                triangles.Add(triCount + xResolution + 1);
                triangles.Add(triCount + xResolution + 2);

                triCount++;
            }

            triCount++;          
        }
    }


    protected override void SetUVs()
    {
        for (int z = 0; z <= zResolution; z++)
        {
            for (int x = 0; x <= xResolution; x++)
            {
                uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
            }
        }
    }


    protected override void SetVertexColours()
    {

        float diff = gradMax - gradMin;

        for (int i = 0; i < numVertices; i++)
        {
            vertexColours.Add(gradient.Evaluate((vertices[i].y - gradMin) / diff));
        }
    }

    protected override void SetNormals()
    {
        SetGeneralNormals();
    }

    protected override void SetTangents()
    {
        SetGeneralTangents();
    }


}
