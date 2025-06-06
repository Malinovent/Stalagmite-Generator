using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(AbstractMeshGenerator), true)]
public class AbstractMeshEditorInspector : Editor
{

    public override void OnInspectorGUI()
    {
        AbstractMeshGenerator meshGenerator = (AbstractMeshGenerator)target;
        DrawDefaultInspector();

        if (GUILayout.Button("Regenerate Mesh"))
        {
            meshGenerator.GenerateMesh();
        }        
    }
}
