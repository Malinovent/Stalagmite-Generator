using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropGenChunk : AbstractLandscapeMeshGenerator
{
    [SerializeField] private int xStartPos;
    [SerializeField] private int zStartPos;
    [SerializeField] private int xEndPos;
    [SerializeField] private int zEndPos;

    public void InitInfiniteLandscape(Material mat, int xRes, int zRes, float meshScale, float yScale, int octaves, float lacunarity, float gain, float perlinScale, Vector2 startPosition)
    {
        this.material = mat;
        this.xResolution = xRes;
        this.zResolution = zRes;
        this.yScale = yScale;

        this.octaves = octaves;
        this.lacunarity = lacunarity;
        this.gain = gain;
        this.perlinScale = perlinScale;

        type = FallOffType.none;

        xStartPos = (int)startPosition.x;
        zStartPos = (int)startPosition.y;

        xEndPos = xStartPos + xResolution;
        zEndPos = zStartPos + zResolution;
    }


    protected override void SetMeshNums()
    {
        numVertices = (xResolution + 1) * (zResolution + 1);
        numTriangles = 6 * xResolution * zResolution;
    }

    protected override void SetVertices()
    {
        float xx, y, zz = 0;
        NoiseGenerator noise = new NoiseGenerator(octaves, gain, lacunarity, perlinScale);

        for (int z = zStartPos; z <= zEndPos; z++)
        {
            for (int x = xStartPos; x <= xEndPos; x++)
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

        for (int z = zStartPos; z < zEndPos; z++)
        {
            for (int x = xStartPos; x < xEndPos; x++)
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
        //for (int z = 0; z <= zResolution; z++)
        //{
        //    for (int x = 0; x <= xResolution; x++)
        //    {
        //        uvs.Add(new Vector2(x / (uvScale * xResolution), z / (uvScale * zResolution)));
        //    }
        //}
    }


    protected override void SetVertexColours()
    {

        
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
