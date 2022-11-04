using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshNormalDisplayer : MonoBehaviour
{

    [SerializeField] bool drawNormals;
    [SerializeField] float normalLength = 1;

    Mesh mesh;

    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        if (drawNormals)
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;

            if (!mesh) { return; }

            Gizmos.color = Color.blue;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector3 vertex = transform.TransformPoint(mesh.vertices[i]);
                Vector3 normal = transform.TransformDirection(mesh.normals[i]);

                Gizmos.DrawLine(vertex, vertex + normal * normalLength);
            }
        }
    }
}
