using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLinesMouse : MonoBehaviour
{
	public Texture2D lineTex;

	public int maxPoints = 5000;

	public float lineWidth = 4f;

	public int minPixelMove = 5;

	public bool useEndCap;

	public Texture2D capLineTex;

	public Texture2D capTex;

	public float capLineWidth = 20f;

	public bool line3D;

	public float distanceFromCamera = 1f;

	private VectorLine line;

	private Vector3 previousPosition;

	private int sqrMinPixelMove;

	private bool canDraw;

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
		if (line3D)
		{
			line = new VectorLine("DrawnLine3D", new List<Vector3>(), texture, width, LineType.Continuous, Joins.Weld);
		}
		else
		{
			line = new VectorLine("DrawnLine", new List<Vector2>(), texture, width, LineType.Continuous, Joins.Weld);
		}
		line.endPointsUpdate = 2;
		if (useEndCap)
		{
			line.endCap = "RoundCap";
		}
		sqrMinPixelMove = minPixelMove * minPixelMove;
	}

	private void Update()
	{
		Vector3 mousePos = GetMousePos();
		if (Input.GetMouseButtonDown(0))
		{
			if (line3D)
			{
				line.points3.Clear();
				line.Draw3D();
			}
			else
			{
				line.points2.Clear();
				line.Draw();
			}
			previousPosition = Input.mousePosition;
			if (line3D)
			{
				line.points3.Add(mousePos);
			}
			else
			{
				line.points2.Add(mousePos);
			}
			canDraw = true;
		}
		else if (Input.GetMouseButton(0) && (Input.mousePosition - previousPosition).sqrMagnitude > (float)sqrMinPixelMove && canDraw)
		{
			previousPosition = Input.mousePosition;
			int count;
			if (line3D)
			{
				line.points3.Add(mousePos);
				count = line.points3.Count;
				line.Draw3D();
			}
			else
			{
				line.points2.Add(mousePos);
				count = line.points2.Count;
				line.Draw();
			}
			if (count >= maxPoints)
			{
				canDraw = false;
			}
		}
	}

	private Vector3 GetMousePos()
	{
		Vector3 mousePosition = Input.mousePosition;
		if (line3D)
		{
			mousePosition.z = distanceFromCamera;
			return Camera.main.ScreenToWorldPoint(mousePosition);
		}
		return mousePosition;
	}
}
