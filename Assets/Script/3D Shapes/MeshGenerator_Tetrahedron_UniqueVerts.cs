using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator_Tetrahedron_UniqueVerts : AbstractMeshGenerator
{

    [SerializeField] Vector3[] vs = new Vector3[4];

    protected override void SetMeshNums()
    {
        numVertices = 12;
        numTriangles = 12;

    }

    protected override void SetVertices()
    {
        //Base
        vertices.Add(vs[0]);
        vertices.Add(vs[1]);
        vertices.Add(vs[2]);

        //Sides
        vertices.Add(vs[0]);
        vertices.Add(vs[2]);
        vertices.Add(vs[3]);

        vertices.Add(vs[2]);
        vertices.Add(vs[1]);
        vertices.Add(vs[3]);

        vertices.Add(vs[1]);
        vertices.Add(vs[0]);
        vertices.Add(vs[3]);


    }

    protected override void SetTriangles()
    {
        for (int i = 0; i < numTriangles; i++)
        {
            triangles.Add(i);
        }
    }


    protected override void SetNormals() { }
    protected override void SetTangents() { }
    protected override void SetUVs() { }
    protected override void SetVertexColours() { }
}
