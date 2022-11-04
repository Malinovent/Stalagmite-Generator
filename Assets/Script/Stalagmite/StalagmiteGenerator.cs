using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;


/// <summary>
/// [WIP] This class inhericts from abstract mesh generator to create a stalagmite
/// </summary>
public class StalagmiteGenerator : AbstractMeshGenerator
{
    [SerializeField] private float initialRadius = 1;                           // Radius of the first ring 
    [SerializeField] private float radialDecrease = 0.1f;                       // Decrease in radius of the next ring
    [SerializeField, Range(0.1f, 0.5f)] private float ringHeight = 0.25f;       // Each ring has is a set of quads. This is the distance in height between quads
    [SerializeField, Range(0.5f, 10)] private float distanceBetweenRings = 1;   // DIstance from one ring to the next
    [SerializeField, Range(3, 50)] private int segments = 4;                    // How many quads per ring

    private int vertexRingSize = 0;                                             // Stores the rings size after calculating on generation                                         
    private int triangleRingSize = 0;                                           // Stores the triagnle size after calculating on generation  

    [SerializeField] private int iterations = 4;                                // The number of rings to create
    [HideInInspector] public int currentIteration = 0;                          // The number of iterations saved doing generation

    private float currentRadius = 0;                                            // The radius of the ring being generated
    private float currentHeight = 0;                                            // The height between the current ring being generated to the previous ring already generated
    [SerializeField] private int newTriangleRingIndex = 0;                      // Stores the number of triangles create during generation

    [Header("Speleothem Parameters")]
    [SerializeField, Range(0, 3)] private int pluvimetricIndex = 0;             // Steleothem parameter to indicate contact with water
    [SerializeField, Range(0, 3)] private int CO2Concentration = 3;             // Steleothem parameter to indicate contact with C02
    [SerializeField] private Vector3 windDirection;                             // Steleothem parameter to indicate simulated wind direction
    [SerializeField, Range(0, 1)] private float windIntensity = 0;              // Steleothem parameter to indicate simulated wind intensity

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
        //Each loop we will create an additional ring, then we connect this ring with the previous
        for (int i = 0; i < iterations; i++)
        {
            currentIteration++;
            Debug.Log("Generating iteration " + currentIteration + " Current Height: " + currentHeight);
            CreateRing();
            ConnectRings();
        }

        UpdateMesh();
    }

    /// <summary>
    /// Creates one ring and connects it
    /// </summary>
    public void DoOneIteration()
    {
        currentIteration++;
        Debug.Log("Generating iteration " + currentIteration + " Current Height: " + currentHeight);
        CreateRing();
        ConnectRings();
        UpdateMesh();
    }

    /// <summary>
    /// Creates one ring
    /// </summary>
    public void CreateRing()
    {
        //Debug.Log("Creating Ring " + currentIteration);
        CreateRingVertices();
        CreateVertexNormals();
        CreateRingTriangles();
        UpdateMesh();
    }

    /// <summary>
    /// Determines and create the ring vertices in a circle
    /// </summary>
    private void CreateRingVertices()
    {
        numVertices += vertexRingSize;

        vertices.AddRange(MeshGenerationUtils.DrawCircle(currentRadius, vertexRingSize / 2, currentHeight));
        vertices.AddRange(MeshGenerationUtils.DrawCircle(currentRadius, vertexRingSize / 2, currentHeight - ringHeight));
        currentHeight -= distanceBetweenRings;
        currentRadius -= Random.Range(radialDecrease, radialDecrease + 0.3f);
    }

    private void CreateVertexNormals()
    { 
    
    }

    public void CreateNewVertex(Vector3 vertex, float height, float radius, Vector3 normalVector)
    {
        vertex = (vertex.y + height + radius) * normalVector;
    }

    /// <summary>
    /// Determines and create the triangles in the correct order to define normal facing direction
    /// </summary>
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
                triangles.Add(i + 1 + segments  + newTriangleRingIndex);
                triangles.Add(i + segments + newTriangleRingIndex);
            }
        }
    }

    /// <summary>
    /// Create the mesh quads between two rings
    /// </summary>
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

    /// <summary>
    /// Reset all values
    /// </summary>
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

    /// <summary>
    /// [WIP] displaces the vertices based on wind conditoins
    /// </summary>
    public void HorizontalDisplacement()
    {
        foreach (Vector3 vertex in vertices)
        {
            Vector3 horizontalDisplacement = Vector3.zero;
            
        }
    }

    /// <summary>
    /// [WIP] Determines new vertex positions based on wind conditions
    /// </summary>
    public void AirFlow()
    {
        Debug.Log("Adding air flow in " + windDirection + " direction ");
        for(int i = 0; i < vertices.Count; i++)
        {
            Vector3 newPos;

            newPos = windIntensity * (vertices[i] + windDirection);

            Debug.Log("Old Vertex: " + vertices[i] + "-- New Vertex: " + newPos);
            vertices[i] += newPos;
            
        }

        UpdateMesh();
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
