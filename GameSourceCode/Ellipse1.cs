using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Ellipse1 : MonoBehaviour
{
	public Texture lineTexture;

	public float xRadius = 120f;

	public float yRadius = 120f;

	public int segments = 60;

	public float pointRotation;

	private void Start()
	{
		List<Vector2> points = new List<Vector2>(segments + 1);
		VectorLine vectorLine = new VectorLine("Line", points, lineTexture, 3f, LineType.Continuous);
		vectorLine.MakeEllipse(new Vector2(Screen.width / 2, Screen.height / 2), xRadius, yRadius, segments, pointRotation);
		vectorLine.Draw();
	}
}
