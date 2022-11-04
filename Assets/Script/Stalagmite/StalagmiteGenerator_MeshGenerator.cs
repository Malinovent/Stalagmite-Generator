using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteGenerator_MeshGenerator : AbstractMeshGenerator
{
    [SerializeField] private float initialRadius = 1;
    [SerializeField, Range(0.1f, 0.5f)] private float ringHeight = 0.25f;
    [SerializeField, Range(0.5f, 10)] private float distanceBetweenRings = 1;
    [SerializeField, Range(3, 50)] private int segments = 4;

    [SerializeField] private int vertexRingSize = 0;
    [SerializeField] private int triangleRingSize = 0;
    [SerializeField] private int iterations = 4;

    [SerializeField] private int currentIteration = 0;
    private float currentHeight = 0;

    private int newTriangleRingIndex = 0;

    

    #region OVERRIDES
    protected override void SetMeshNums()
    {
        numVertices = segments * 2;  //Vertices equal to one rings
        numTriangles = segments * 6; //Triangles equal to triangles * 3 
    }

    protected override void SetVertices()
    {
        if (currentIteration >= iterations) { return; }

        numTriangles += triangleRingSize;
        for (int i = 0; i < segments; i++)
        {
            //Debug.Log("Iteration " + iteration + " loop " + (i + 1));
            if (i == segments - 1)
            {
                //Triangle 1
                triangles.Add(i + newTriangleRingIndex);
                triangles.Add(0 + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);


                //Triangle 2
                triangles.Add(i + 1 + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);
                triangles.Add(0 + newTriangleRingIndex);
            }
            else
            {
                //Triangle 1
                triangles.Add(i + newTriangleRingIndex);
                triangles.Add(i + 1 + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);


                //Triangle 2
                triangles.Add(i + 1 + newTriangleRingIndex);
                triangles.Add(i + segments + 1 + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);
            }
        }
    }

    protected override void SetTriangles()
    {
        if (currentIteration >= iterations) { return; }

        CreateRingVertices();
        iterations++;
    }

    protected override void SetNormals()   {           }

    protected override void SetTangents()   {          }

    protected override void SetUVs()    {            }

    protected override void SetVertexColours()    {            }
    #endregion

    private void CreateRingVertices()
    {
        numVertices += vertexRingSize;
        vertices.AddRange(MeshGenerationUtils.DrawCircle(initialRadius, vertexRingSize / 2, currentHeight));
        vertices.AddRange(MeshGenerationUtils.DrawCircle(initialRadius, vertexRingSize / 2, currentHeight - ringHeight));
    }

}
