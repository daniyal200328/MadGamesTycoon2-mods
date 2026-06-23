using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SplineFollow3D : MonoBehaviour
{
	public int segments = 250;

	public bool doLoop = true;

	public Transform cube;

	public float speed = 0.05f;

	private IEnumerator Start()
	{
		List<Vector3> list = new List<Vector3>();
		int num = 1;
		GameObject gameObject = GameObject.Find("Sphere" + num++);
		while (gameObject != null)
		{
			list.Add(gameObject.transform.position);
			gameObject = GameObject.Find("Sphere" + num++);
		}
		VectorLine line = new VectorLine("Spline", new List<Vector3>(segments + 1), 2f, LineType.Continuous);
		line.MakeSpline(list.ToArray(), segments, doLoop);
		line.Draw3D();
		do
		{
			for (float dist = 0f; dist < 1f; dist += Time.deltaTime * speed)
			{
				cube.position = line.GetPoint3D01(dist);
				yield return null;
			}
		}
		while (doLoop);
	}
}
