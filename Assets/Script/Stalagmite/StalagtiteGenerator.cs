using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagtiteGenerator : AbstractMeshGenerator
{
    [SerializeField] private float initialRadius = 1;
    [SerializeField] private float radialDecrease = 0.1f;
    [SerializeField, Range(0.1f, 0.5f)] private float ringHeight = 0.25f;
    [SerializeField, Range(0.5f, 10)] private float distanceBetweenRings = 1;
    [SerializeField, Range(3, 50)] private int segments = 4;

    [SerializeField] private int vertexRingSize = 0;
    [SerializeField] private int triangleRingSize = 0;

    [SerializeField] private int iterations = 4;
    [HideInInspector] public int currentIteration = 0;

    private float currentRadius = 0;
    private float currentHeight = 0;
    [SerializeField] private int newTriangleRingIndex = 0;

    [Header("Speleothem Parameters")]
    [SerializeField, Range(0, 3)] private int pluvimetricIndex = 0;
    [SerializeField, Range(0, 3)] private int CO2Concentration = 3;


    protected override void SetMeshNums()
    {
        //numVertices = segments * 2;  //Vertices equal to one rings
        //numTriangles = segments * 6; //Triangles equal to triangles * 3 
    }


    protected override void SetVertices()
    {

    }

    protected override void SetTriangles()
    {

    }

    private void GenerateStalagmite()
    {

        for (int i = 0; i < iterations; i++)
        {
            currentIteration++;
            Debug.Log("Generating iteration " + currentIteration + " Current Height: " + currentHeight);
            CreateRing();
            ConnectRings();
        }

        UpdateMesh();
    }

    public void DoOneIteration()
    {
        currentIteration++;
        Debug.Log("Generating iteration " + currentIteration + " Current Height: " + currentHeight);
        CreateRing();
        ConnectRings();
        UpdateMesh();
    }

    public void CreateRing()
    {
        //Debug.Log("Creating Ring " + currentIteration);
        CreateRingVertices();
        CreateRingTriangles();
        UpdateMesh();
    }

    private void CreateRingVertices()
    {
        numVertices += vertexRingSize;

        vertices.AddRange(MeshGenerationUtils.DrawCircle(currentRadius, vertexRingSize / 2, currentHeight));
        vertices.AddRange(MeshGenerationUtils.DrawCircle(currentRadius, vertexRingSize / 2, currentHeight - ringHeight));
        currentHeight -= distanceBetweenRings;
        currentRadius -= Random.Range(radialDecrease, radialDecrease + 0.1f);
    }

    private void CreateRingTriangles()
    {
        numTriangles += triangleRingSize;
        newTriangleRingIndex = vertexRingSize * currentIteration;
        Debug.Log("New Triangle Ring Index: " + newTriangleRingIndex);
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
                triangles.Add(i + 1 + segments + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);
            }
        }
    }

    private void ConnectRings()
    {
        numTriangles += triangleRingSize;

        int offset = (currentIteration - 1) * vertexRingSize;
        Debug.Log("Offset : " + offset);

        //offset = 0;

        //Add Triangles connecting the bottom vertices of the first ring to the top vertices of the bottom ring - Shared Vertices
        for (int i = 0; i < segments; i++)
        {
            if (i == segments - 1)
            {
                //Triangle 1
                triangles.Add(i + segments + offset);
                triangles.Add(segments + offset);
                triangles.Add(i + newTriangleRingIndex);

                //Triangle 2
                triangles.Add(segments + offset);
                triangles.Add(newTriangleRingIndex);
                triangles.Add(i + newTriangleRingIndex);
            }
            else
            {
                //Triangle 1
                triangles.Add(i + segments + offset);
                triangles.Add(i + segments + 1 + offset);
                triangles.Add(i + newTriangleRingIndex);

                //Triangle 2
                triangles.Add(i + segments + 1 + offset);
                triangles.Add(i + newTriangleRingIndex + 1);
                triangles.Add(i + newTriangleRingIndex);
            }
        }
    }

    protected override void SetNormals() { }
    protected override void SetTangents() { }
    protected override void SetUVs() { }
    protected override void SetVertexColours() { }


    public void StartGeneration()
    {
        Debug.Log("Starting Generation");
        Reset();

        CreateRing();

        GenerateStalagmite();
    }

    public void Reset()
    {
        currentRadius = initialRadius;
        vertexRingSize = segments * 2;
        triangleRingSize = segments * 6;
        numVertices = 0;
        numTriangles = 0;
        currentIteration = 0;
        currentHeight = 0;
        newTriangleRingIndex = 0;
        triangles.Clear();
        vertices.Clear();
        normals.Clear();
        UpdateMesh();
    }


    public void ContinueGeneration()
    {
        currentIteration++;
    }

    public void HorizontalDisplacement()
    {
        foreach (Vector3 vertex in vertices)
        {

        }
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = Color.black;
        //foreach (Vector3 v in vertices)
        //{
        //    Gizmos.DrawSphere(v, 0.05f);
        //}



        //Gizmos.color = Color.white;
        //    for(int i = 0; i < triangles.Count - 1; i++)
        //    { 

        //        Gizmos.DrawLine(vertices[triangles[i]], vertices[triangles[i + 1]]);
        //    }
    }
}