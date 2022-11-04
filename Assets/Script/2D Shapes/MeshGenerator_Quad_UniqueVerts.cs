using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator_Quad_UniqueVerts : AbstractMeshGenerator
{
    [SerializeField] private Vector3[] vs = new Vector3[4];
    [SerializeField] private Vector2[] flexibleUVs = new Vector2[6];

    protected override void SetMeshNums()
    {
        numVertices = 6;
        numTriangles = 6;
    }

    protected override void SetNormals()
    {
       
    }

    protected override void SetTangents()
    {
        
    }

    protected override void SetTriangles()
    {
        //triangle 1
        triangles.Add(0);
        triangles.Add(1);
        triangles.Add(2);

        //triangle 2
        triangles.Add(3);
        triangles.Add(4);
        triangles.Add(5);
    }

    protected override void SetUVs()
    {
        uvs.AddRange(flexibleUVs);
    }

    protected override void SetVertices()
    {
        vertices.Add(vs[0]);
        vertices.Add(vs[1]);
        vertices.Add(vs[3]);

        vertices.Add(vs[0]);
        vertices.Add(vs[3]);
        vertices.Add(vs[2]);
    }

    protected override void SetVertexColours()
    {
       
    }
}
