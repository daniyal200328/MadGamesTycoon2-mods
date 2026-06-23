using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawLines : MonoBehaviour
{
	public float rotateSpeed = 90f;

	public float maxPoints = 500f;

	private VectorLine line;

	private bool endReached;

	private bool continuous = true;

	private bool oldContinuous = true;

	private bool fillJoins;

	private bool oldFillJoins;

	private bool weldJoins;

	private bool oldWeldJoins;

	private bool thickLine;

	private bool canClick = true;

	private void Start()
	{
		SetLine();
	}

	private void SetLine()
	{
		VectorLine.Destroy(ref line);
		if (!continuous)
		{
			fillJoins = false;
		}
		LineType lineType = ((!continuous) ? LineType.Discrete : LineType.Continuous);
		Joins joins = ((!fillJoins) ? Joins.None : Joins.Fill);
		int num = (thickLine ? 24 : 2);
		line = new VectorLine("Line", new List<Vector2>(), num, lineType, joins);
		line.drawTransform = base.transform;
		endReached = false;
	}

	private void Update()
	{
		Vector3 vector = base.transform.InverseTransformPoint(Input.mousePosition);
		if (Input.GetMouseButtonDown(0) && canClick && !endReached)
		{
			line.points2.Add(vector);
			if (line.points2.Count == 1)
			{
				line.points2.Add(Vector2.zero);
			}
			if ((float)line.points2.Count == maxPoints)
			{
				endReached = true;
			}
		}
		if (line.points2.Count >= 2)
		{
			line.points2[line.points2.Count - 1] = vector;
			line.Draw();
		}
		base.transform.RotateAround(new Vector2(Screen.width / 2, Screen.height / 2), Vector3.forward, Time.deltaTime * rotateSpeed * Input.GetAxis("Horizontal"));
	}

	private void OnGUI()
	{
		Rect screenRect = new Rect(20f, 20f, 265f, 220f);
		canClick = !screenRect.Contains(Event.current.mousePosition);
		GUILayout.BeginArea(screenRect);
		GUI.contentColor = Color.black;
		GUILayout.Label("Click to add points to the line\nRotate with the right/left arrow keys");
		GUILayout.Space(5f);
		continuous = GUILayout.Toggle(continuous, "Continuous line");
		thickLine = GUILayout.Toggle(thickLine, "Thick line");
		line.lineWidth = (thickLine ? 24 : 2);
		fillJoins = GUILayout.Toggle(fillJoins, "Fill joins (only works with continuous line)");
		if (line.lineType != LineType.Continuous)
		{
			fillJoins = false;
		}
		weldJoins = GUILayout.Toggle(weldJoins, "Weld joins");
		if (oldContinuous != continuous)
		{
			oldContinuous = continuous;
			line.lineType = ((!continuous) ? LineType.Discrete : LineType.Continuous);
		}
		if (oldFillJoins != fillJoins)
		{
			if (fillJoins)
			{
				weldJoins = false;
			}
			oldFillJoins = fillJoins;
		}
		else if (oldWeldJoins != weldJoins)
		{
			if (weldJoins)
			{
				fillJoins = false;
			}
			oldWeldJoins = weldJoins;
		}
		if (fillJoins)
		{
			line.joins = Joins.Fill;
		}
		else if (weldJoins)
		{
			line.joins = Joins.Weld;
		}
		else
		{
			line.joins = Joins.None;
		}
		GUILayout.Space(10f);
		GUI.contentColor = Color.white;
		if (GUILayout.Button("Randomize Color", GUILayout.Width(150f)))
		{
			RandomizeColor();
		}
		if (GUILayout.Button("Randomize All Colors", GUILayout.Width(150f)))
		{
			RandomizeAllColors();
		}
		if (GUILayout.Button("Reset line", GUILayout.Width(150f)))
		{
			SetLine();
		}
		if (endReached)
		{
			GUI.contentColor = Color.black;
			GUILayout.Label("No more points available. You must be bored!");
		}
		GUILayout.EndArea();
	}

	private void RandomizeColor()
	{
		line.color = new Color(Random.value, Random.value, Random.value);
	}

	private void RandomizeAllColors()
	{
		int segmentNumber = line.GetSegmentNumber();
		for (int i = 0; i < segmentNumber; i++)
		{
			line.SetColor(new Color(Random.value, Random.value, Random.value), i);
		}
	}
}
