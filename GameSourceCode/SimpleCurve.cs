using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SimpleCurve : MonoBehaviour
{
	public Vector2[] curvePoints;

	public int segments = 50;

	private void Start()
	{
		if (curvePoints.Length != 4)
		{
			Debug.Log("Curve points array must have 4 elements only");
			return;
		}
		List<Vector2> points = new List<Vector2>(segments + 1);
		VectorLine vectorLine = new VectorLine("Curve", points, 2f, LineType.Continuous, Joins.Weld);
		vectorLine.MakeCurve(curvePoints, segments);
		vectorLine.Draw();
	}
}
