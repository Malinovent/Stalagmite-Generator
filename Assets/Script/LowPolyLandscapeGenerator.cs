using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowPolyLandscapeGenerator : AbstractLandscapeMeshGenerator
{
    protected override void SetMeshNums()
    {
        numTriangles = 6 * xResolution * zResolution;
        numVertices = numTriangles;
    }

    protected override void SetVertices() 
    {
        NoiseGenerator noise = new NoiseGenerator(octaves, gain, lacunarity, perlinScale);

        int xx = 0;
        int zz = 0;

        bool isBottomTriangle = false;

        for (int vertexIndex = 0; vertexIndex < numVertices; vertexIndex++)
        {
            //Increment xx and zz appropriately
            //Check if it's a bottom or top of a triangle   
            if (IsNewRow(vertexIndex))
            {
                isBottomTriangle = !isBottomTriangle;
            }

            //Increase xx by one when it's a new position
            if (!IsNewRow(vertexIndex))
            {
                if (isBottomTriangle)
                {
                    if (vertexIndex % 3 == 1)
                    {
                        xx++;
                    }
                }
                else
                {
                    if (vertexIndex % 3 == 2)
                    {
                        xx++;
                    }
                }
            }

            //Increase zz by one when it's a new row. Reset xx to zero
            if (IsNewRow(vertexIndex))
            {
                //Reset xx on new row
                xx = 0;

                //Actually go up a level
                if (!isBottomTriangle)
                {
                    zz++;
                }
            }

            //Set Vertices
            float xValue = ((float)xx / xResolution) * meshScale;
            float zValue = ((float)zz / zResolution) * meshScale;

            float y = yScale * noise.GetFractalNoise(xValue, zValue);
            y = FallOff((float)xx, y, (float)zz);

            vertices.Add(new Vector3(xValue, y, zValue));
        }
    }

    private bool IsNewRow(int vertexIndex)
    {
        return vertexIndex % (3 * xResolution) == 0;
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
                triangles.Add(triCount + 3*xResolution + 1);
                triangles.Add(triCount + 1);

                //Triangle 2
                triangles.Add(triCount + 2);
                triangles.Add(triCount + 3 * xResolution + 1);
                triangles.Add(triCount + 3 * xResolution + 2);

                triCount+= 3;
            }

            triCount += 3 * xResolution;
        }
    }
    protected override void SetNormals() { }
    protected override void SetTangents() { }
    protected override void SetUVs() { }
    protected override void SetVertexColours() { }
}
