using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MakeSpline : MonoBehaviour
{
	public int segments = 250;

	public bool loop = true;

	public bool usePoints;

	private void Start()
	{
		List<Vector3> list = new List<Vector3>();
		int num = 1;
		GameObject gameObject = GameObject.Find("Sphere" + num++);
		while (gameObject != null)
		{
			list.Add(gameObject.transform.position);
			gameObject = GameObject.Find("Sphere" + num++);
		}
		if (usePoints)
		{
			VectorLine vectorLine = new VectorLine("Spline", new List<Vector3>(segments + 1), 2f, LineType.Points);
			vectorLine.MakeSpline(list.ToArray(), segments, loop);
			vectorLine.Draw();
		}
		else
		{
			VectorLine vectorLine2 = new VectorLine("Spline", new List<Vector3>(segments + 1), 2f, LineType.Continuous);
			vectorLine2.MakeSpline(list.ToArray(), segments, loop);
			vectorLine2.Draw3D();
		}
	}
}
