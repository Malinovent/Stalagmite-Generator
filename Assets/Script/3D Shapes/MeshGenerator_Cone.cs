using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshGenerator_Cone : AbstractMeshGenerator
{
	[SerializeField, Range(3, 50)] private int numSides = 3;
	[SerializeField] private float baseRadius;
	[SerializeField] private float height;

	[SerializeField] private Gradient gradient;

	protected override void SetMeshNums()
	{
		numVertices = numSides * 3; //numSides vertices on each end, 4 on each length-side
		numTriangles = 6 * numSides; //there are 3 * (numSides - 2) on each end and 6 on each length-side: 6*numSides
	}

	protected override void SetVertices()
	{
		//building block vertices
		Vector3[] vs = new Vector3[numSides * 3];

		//Set the vs ring
		for (int i = 0; i < numSides; i++)
		{
			float angle = 2 * Mathf.PI * i / numSides;
			//one end
			vs[i] = new Vector3(baseRadius * Mathf.Cos(angle), 0, baseRadius * Mathf.Sin(angle));
			vertices.Add(vs[i]);
			//other end
			//vs[i + numSides] = new Vector3(backRadius * Mathf.Cos(angle), backRadius * Mathf.Sin(angle), length);
		}

		//Unique Base Vert Index 1,2 ...num of side + 1
		for (int i = 0; i < numSides; i++)
		{
			vertices.Add(Vector3.zero);
		}

		//Unique Tip Vert Index 1,2 ...num of side + 1
		for (int i = 0; i < numSides; i++)
		{
			vertices.Add(new Vector3(0, height, 0));
		}


	}

	protected override void SetTriangles()
	{

		//sides
		for (int i = 0; i < numSides; i++)
		{
			if (i == numSides - 1)
			{
				//Base
				triangles.Add(i);
				triangles.Add(0);
				triangles.Add(i + numSides);
				

				//Top
				triangles.Add(i);
				triangles.Add(i + (numSides * 2));
				triangles.Add(0);
			}
			else
			{
				//Base
				triangles.Add(i);
				triangles.Add(i + 1);
				triangles.Add(i + numSides);
				

				//Top
				triangles.Add(i);
				triangles.Add(i + (numSides * 2));
				triangles.Add(i + 1);
			}
		}	
	}

	protected override void SetVertexColours()
	{
		for (int i = 0; i < numVertices; i++)
		{
			//use the values in the gradient to colour
			vertexColours.Add(gradient.Evaluate((float)i / numVertices));
		}
	}


	protected override void SetNormals() { }
	protected override void SetTangents() { }
	protected override void SetUVs() { }
}
