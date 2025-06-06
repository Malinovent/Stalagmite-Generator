using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(StalagmiteGenerator))]
public class StalagmiteGeneratorEditorInspector : Editor
{
    
    public override void OnInspectorGUI()
    {
        StalagmiteGenerator Target = (StalagmiteGenerator)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Generate Stalagmite"))
        {
            //Begin Process
            Target.Init();
            Target.StartGeneration();
        }

        if (GUILayout.Button("Do one iteration"))
        {
            if (Target.currentIteration == 0) { Target.Init(); Target.Reset(); }
            else Target.DoOneIteration();
        }

        if (GUILayout.Button("Reset"))
        {
            Target.Reset();
        }

        if (GUILayout.Button("Add Air Flow"))
        {
            Target.AirFlow();
        }

        if (GUILayout.Button("Close Mesh"))
        {
            Target.CloseMesh();
        }
    }


}
