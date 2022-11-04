using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter)), RequireComponent(typeof(MeshRenderer))]
public abstract class AbstractMeshGenerator : MonoBehaviour
{
    [SerializeField] protected Material material;

    [SerializeField] protected List<Vector3> vertices;
    [SerializeField] protected List<int> triangles;

    protected List<Vector3> normals;
    protected List<Vector4> tangents;
    protected List<Vector2> uvs;
    protected List<Color32> vertexColours;

    protected int numVertices;
    protected int numTriangles;

    MeshFilter meshFilter;
    protected MeshRenderer meshRenderer;
    MeshCollider meshCollider;
    Mesh mesh;

    // Update is called once per frame
    void Start()
    {
        //Cache components
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        meshRenderer.material = material;

        //Initialize
        Init();
        SetMeshNums();

        //Create the mesh
        CreateMesh();
    }

    protected abstract void SetMeshNums();

    /// <summary>
    /// Determines if the number of vertices, triangles, tangents, uvs and vertext colors match expected outcomes
    /// </summary>
    private bool ValidateMesh()
    {

        string errorStr = "";

        errorStr += vertices.Count == numVertices ? "" : " Should be " + numVertices + " vertices, but there are " + vertices.Count;
        errorStr += triangles.Count == numTriangles ? "" : " Should be " + numTriangles + " triangles, but there are " + triangles.Count;
        //errorStr += (normals.Count == numVertices || normals.Count == 0) ? "" : " Should be " + numVertices + " normals, but there are " + normals.Count;
        errorStr += (tangents.Count == numVertices || tangents.Count == 0) ? "" : " Should be " + numVertices + " tangents, but there are " + tangents.Count;
        errorStr += (uvs.Count == numVertices || uvs.Count == 0) ? "" : " Should be " + numVertices + " uvs, but there are " + uvs.Count;
        errorStr += (vertexColours.Count == numVertices || vertexColours.Count == 0) ? "" : " Should be " + numVertices + " vertexColors, but there are " + vertexColours.Count;


        bool isValid =  string.IsNullOrEmpty(errorStr);

        if (!isValid)
        {
            Debug.LogError("Not drawing mesh." + errorStr);
        }

        return isValid;
    }

    /// <summary>
    /// Initialize lists
    /// </summary>
    public void Init()
    {
        vertices = new List<Vector3>();
        triangles = new List<int>();

        normals = new List<Vector3>();
        tangents = new List<Vector4>();
        uvs = new List<Vector2>();
        vertexColours = new List<Color32>();
    }

    /// <summary>
    /// Creates the mesh by setting the vertices first, then the triangles and then the rest
    /// </summary>
    private void CreateMesh()
    {
        mesh = new Mesh();

        SetVertices();
        SetTriangles();

        SetNormals();
        SetUVs();
        SetTangents();
        SetVertexColours();

        UpdateMesh();
    }

    protected void UpdateMesh()
    {
        if (ValidateMesh())
        {
            mesh = new Mesh();
            //This should always be done vertices first, triangles second.
            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);

            //if (normals.Count == 0)
            //{
            //mesh.RecalculateNormals();
            //normals.Clear();
            //normals.AddRange(mesh.normals);
            //}

            //mesh.SetNormals(normals);
            mesh.SetUVs(0, uvs);
            mesh.SetTangents(tangents);
            mesh.SetColors(vertexColours);

            if (!meshFilter) { meshFilter = GetComponent<MeshFilter>(); }
            meshFilter.mesh = mesh;
            if (meshCollider) { meshCollider.sharedMesh = mesh; }
        }
    }

    #region ABSTRACT FUNCTIONS
    protected abstract void SetVertices();
    protected abstract void SetTriangles();
    protected abstract void SetNormals();
    protected abstract void SetTangents();
    protected abstract void SetUVs();
    protected abstract void SetVertexColours();
    #endregion

    /// <summary>
    /// General function to calculate normals for any object
    /// </summary>
    protected void SetGeneralNormals()
    {
        int numGeometricTriangles = numTriangles / 3;
        Vector3[] norms = new Vector3[numVertices];
        int index = 0;

        for (int i = 0; i < numGeometricTriangles; i++)
        {
            //Get the vertices of the triangle at index
            int triA = triangles[index];
            int triB = triangles[index + 1];
            int triC = triangles[index + 2];

            //Get directions from the triangle vertices
            Vector3 dirA = vertices[triB] - vertices[triA];
            Vector3 dirB = vertices[triC] - vertices[triA];

            //Get the normal using the directions
            Vector3 normal = Vector3.Cross(dirA, dirB);

            //Add the normal to each vertex of this triangle
            norms[triA] += normal;
            norms[triB] += normal;
            norms[triC] += normal;

            //Increase index for the next iteration of triangles
            index += 3;
        }

        for (int i = 0; i < numVertices; i++)
        {
            normals.Add(norms[i].normalized);
        }
    }

    /// <summary>
    /// General function to calculate tangents for any object
    /// </summary>
    protected void SetGeneralTangents()
    {

        if (uvs.Count == 0 || normals.Count == 0)
        {
            Debug.Log("Set UVs and Normals before adding tangents");
            return;
        }

        int numGeometricTriangles = numTriangles / 3;
        Vector3[] tans = new Vector3[numVertices];
        Vector3[] bitans = new Vector3[numVertices];
        int index = 0;

        for (int i = 0; i < numGeometricTriangles; i++)
        {
            //Get the vertices of the triangle at index
            int triA = triangles[index];
            int triB = triangles[index + 1];
            int triC = triangles[index + 2];

            //The coresponding uvs
            Vector2 uvA = uvs[triA];
            Vector2 uvB = uvs[triB];
            Vector2 uvC = uvs[triC];

            //Get directions from the triangle vertices
            Vector3 dirA = vertices[triB] - vertices[triA];
            Vector3 dirB = vertices[triC] - vertices[triA];

            //From matrix equation
            Vector2 uvDiffA = new Vector2(uvB.x - uvA.x, uvC.x - uvA.x);
            Vector2 uvDiffB = new Vector2(uvB.y - uvA.y, uvC.y - uvA.y);

            float determinant = 1f / (uvDiffA.x * uvDiffB.y - uvDiffA.y * uvDiffB.x);
            Vector3 sDir = determinant * (new Vector3(
                uvDiffB.y * dirA.x - uvDiffB.x * dirB.x, 
                uvDiffB.y * dirA.y - uvDiffB.x * dirB.y, 
                uvDiffB.y * dirA.z - uvDiffB.x * dirB.z));

            Vector3 tDir = determinant * (new Vector3(
                uvDiffA.x * dirB.x - uvDiffA.y * dirA.x, 
                uvDiffA.x * dirB.y - uvDiffA.y * dirA.y, 
                uvDiffA.x * dirB.z - uvDiffA.y * dirA.z));

            tans[triA] += sDir;
            tans[triB] += sDir;
            tans[triC] += sDir;


            bitans[triA] += tDir;
            bitans[triB] += tDir;
            bitans[triC] += tDir;

            index += 3;
        }


        for (int i = 0; i < numVertices; i++)
        {
            Vector3 normal = normals[i];
            Vector3 tan = tans[i];

            Vector3 tangent3 = (tan - Vector3.Dot(normal, tan) * normal).normalized;
            Vector4 tangent = tangent3;
            tangent.w = Vector3.Dot(Vector3.Cross(normal, tan), bitans[i]) < 0f ? -1f : 1f;

            tangents.Add(tangent);
            
        }
    }
}
