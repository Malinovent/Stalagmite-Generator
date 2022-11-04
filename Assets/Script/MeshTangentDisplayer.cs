using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshTangentDisplayer : MonoBehaviour
{
    [SerializeField] bool drawTangents;
    [SerializeField] float tangentLength = 1;

    Mesh mesh;

    // Update is called once per frame
    void OnDrawGizmosSelected()
    {
        if (drawTangents)
        {
            mesh = GetComponent<MeshFilter>().sharedMesh;

            if (!mesh) { return; }

            Gizmos.color = Color.red;

            for (int i = 0; i < mesh.vertexCount; i++)
            {
                Vector3 vertex = transform.TransformPoint(mesh.vertices[i]);
                Vector3 tangent = transform.TransformDirection(mesh.tangents[i]);

                Gizmos.DrawLine(vertex, vertex + tangent * tangentLength);
            }
        }
    }
}
