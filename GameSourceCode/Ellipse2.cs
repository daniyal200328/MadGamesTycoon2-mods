using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Ellipse2 : MonoBehaviour
{
	public Texture lineTexture;

	public int segments = 60;

	public int numberOfEllipses = 10;

	private void Start()
	{
		List<Vector2> points = new List<Vector2>(segments * 2 * numberOfEllipses);
		VectorLine vectorLine = new VectorLine("Line", points, lineTexture, 3f);
		for (int i = 0; i < numberOfEllipses; i++)
		{
			Vector2 vector = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height));
			vectorLine.MakeEllipse(vector, Random.Range(10, Screen.width / 2), Random.Range(10, Screen.height / 2), segments, i * (segments * 2));
		}
		vectorLine.Draw();
	}
}
