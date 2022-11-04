using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator_Polygon : AbstractMeshGenerator
{
    [SerializeField, Range(3, 50)] private int numOfSides = 3;
    [SerializeField] private float radius = 1;

    [SerializeField] private float xTiling = 1;
    [SerializeField] private float yTiling = 2;

    [SerializeField] private float xOffset = 0;
    [SerializeField] private float yOffset = 0;

    [SerializeField] private float rotation = 0;

    protected override void SetMeshNums()
    {
        numVertices = numOfSides;
        numTriangles = 3 * (numOfSides - 2);
    }

    protected override void SetVertices()
    {
        for (int i = 0; i < numVertices; i++)
        {
            float angle = 2 * Mathf.PI * i / numOfSides;
            vertices.Add(new Vector3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0) );
        }
    }


    protected override void SetTriangles()
    {
        for (int i = 1; i < numOfSides - 1; i++)
        {
            triangles.Add(0);
            triangles.Add(i + 1);
            triangles.Add(i);
        }
    }

    protected override void SetUVs()    
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            float uvX = vertices[i].x * xTiling + xOffset;
            float uvY = vertices[i].y * yTiling + yOffset;
            Vector2 uv = new Vector2(uvX, uvY);

            uvs.Add(Quaternion.AngleAxis(rotation, Vector3.forward) * uv);
        }
    }


    protected override void SetNormals()
    {
        Vector3 normal = new Vector3(0, 0, -1);
        for (int i = 0; i < numVertices; i++)
        {
            normals.Add(normal);
        }
    }

    protected override void SetTangents()   
    {
        //Vector4 tangent = new Vector4(1, 0, 0, -1);
        Vector3 tangent3 = new Vector3(1, 0, 0); //Because this is how the UVs are oriented at angle = 0
        //Rotate clockwise as alpha increase;
        Vector3 rotatedTangent = Quaternion.AngleAxis(rotation, -Vector3.forward) * tangent3;
        Vector4 tangent = rotatedTangent;
        tangent.w = -1;


        for (int i = 0; i < numVertices; i++)
        {
            tangents.Add(tangent);
        }

    }
    
    protected override void SetVertexColours()    {    }
}
