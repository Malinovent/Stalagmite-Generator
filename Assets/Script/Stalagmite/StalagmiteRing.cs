using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalagmiteRing
{
    List<Vector3> vertices = new List<Vector3>();

    public float initialRingRadius;
    public float actualRingRadius;
    public float ringHeight;
    public int segments;

    public StalagmiteRing(float irr, float height, int segments)
    {
        initialRingRadius = irr;
        ringHeight = height;
        this.segments = segments;
    }


    public void ApplyHorizontalDisplacement()
    {
        for (int i = 0; i < vertices.Count; i++)
        { 
            
        }
    }

   


    #region GETTERS
    public List<Vector3> GetVertices()
    {
        return vertices;
    }
    #endregion
}
