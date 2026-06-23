using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLinesTouch : MonoBehaviour
{
	public Texture2D lineTex;

	public int maxPoints = 5000;

	public float lineWidth = 4f;

	public int minPixelMove = 5;

	public bool useEndCap;

	public Texture2D capLineTex;

	public Texture2D capTex;

	public float capLineWidth = 20f;

	private VectorLine line;

	private Vector2 previousPosition;

	private int sqrMinPixelMove;

	private bool canDraw;

	private Touch touch;

	private void Start()
	{
		Texture2D texture;
		float width;
		if (useEndCap)
		{
			VectorLine.SetEndCap("RoundCap", EndCap.Mirror, capLineTex, capTex);
			texture = capLineTex;
			width = capLineWidth;
		}
		else
		{
			texture = lineTex;
			width = lineWidth;
		}
		line = new VectorLine("DrawnLine", new List<Vector2>(), texture, width, LineType.Continuous, Joins.Weld);
		line.endPointsUpdate = 2;
		if (useEndCap)
		{
			line.endCap = "RoundCap";
		}
		sqrMinPixelMove = minPixelMove * minPixelMove;
	}

	private void Update()
	{
		if (Input.touchCount <= 0)
		{
			return;
		}
		touch = Input.GetTouch(0);
		if (touch.phase == TouchPhase.Began)
		{
			line.points2.Clear();
			line.Draw();
			previousPosition = touch.position;
			line.points2.Add(touch.position);
			canDraw = true;
		}
		else if (touch.phase == TouchPhase.Moved && (touch.position - previousPosition).sqrMagnitude > (float)sqrMinPixelMove && canDraw)
		{
			previousPosition = touch.position;
			line.points2.Add(touch.position);
			if (line.points2.Count >= maxPoints)
			{
				canDraw = false;
			}
			line.Draw();
		}
	}
}
