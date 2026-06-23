using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class AnimatePartialLine : MonoBehaviour
{
	public Texture lineTexture;

	public int segments = 60;

	public int visibleLineSegments = 20;

	public float speed = 60f;

	private float startIndex;

	private float endIndex;

	private VectorLine line;

	private void Start()
	{
		startIndex = -visibleLineSegments;
		endIndex = 0f;
		List<Vector2> points = new List<Vector2>(segments + 1);
		line = new VectorLine("Spline", points, lineTexture, 30f, LineType.Continuous, Joins.Weld);
		int num = Screen.width / 5;
		int num2 = Screen.height / 3;
		line.MakeSpline(new Vector2[4]
		{
			new Vector2(num, num2),
			new Vector2(num * 2, num2 * 2),
			new Vector2(num * 3, num2 * 2),
			new Vector2(num * 4, num2)
		});
	}

	private void Update()
	{
		startIndex += Time.deltaTime * speed;
		endIndex += Time.deltaTime * speed;
		if (startIndex >= (float)(segments + 1))
		{
			startIndex = -visibleLineSegments;
			endIndex = 0f;
		}
		else if (startIndex < (float)(-visibleLineSegments))
		{
			startIndex = segments;
			endIndex = segments + visibleLineSegments;
		}
		line.drawStart = (int)startIndex;
		line.drawEnd = (int)endIndex;
		line.Draw();
	}
}
