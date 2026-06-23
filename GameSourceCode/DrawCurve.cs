using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawCurve : MonoBehaviour
{
	public Texture lineTexture;

	public Color lineColor = Color.white;

	public Texture dottedLineTexture;

	public Color dottedLineColor = Color.yellow;

	public int segments = 60;

	public GameObject anchorPoint;

	public GameObject controlPoint;

	public Transform canvas;

	private int numberOfCurves = 1;

	private VectorLine line;

	private VectorLine controlLine;

	private int pointIndex;

	private GameObject anchorObject;

	private int oldWidth;

	private bool useDottedLine;

	private bool oldDottedLineSetting;

	private int oldSegments;

	private bool listPoints;

	public static DrawCurve use;

	private void Start()
	{
		use = this;
		oldWidth = Screen.width;
		oldSegments = segments;
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2((float)Screen.width * 0.25f, (float)Screen.height * 0.25f));
		list.Add(new Vector2((float)Screen.width * 0.125f, (float)Screen.height * 0.5f));
		list.Add(new Vector2((float)Screen.width - (float)Screen.width * 0.25f, (float)Screen.height - (float)Screen.height * 0.25f));
		list.Add(new Vector2((float)Screen.width - (float)Screen.width * 0.125f, (float)Screen.height * 0.5f));
		controlLine = new VectorLine("Control Line", list, 2f);
		controlLine.color = new Color(0f, 0.75f, 0.1f, 0.6f);
		controlLine.Draw();
		line = new VectorLine("Curve", new List<Vector2>(segments + 1), lineTexture, 5f, LineType.Continuous, Joins.Weld);
		line.MakeCurve(list[0], list[1], list[2], list[3], segments);
		line.Draw();
		AddControlObjects();
		AddControlObjects();
	}

	private void SetLine()
	{
		if (useDottedLine)
		{
			line.texture = dottedLineTexture;
			line.color = dottedLineColor;
			line.lineWidth = 8f;
			line.textureScale = 1f;
		}
		else
		{
			line.texture = lineTexture;
			line.color = lineColor;
			line.lineWidth = 5f;
			line.textureScale = 0f;
		}
	}

	private void AddControlObjects()
	{
		anchorObject = Object.Instantiate(anchorPoint, controlLine.points2[pointIndex], Quaternion.identity);
		anchorObject.transform.SetParent(canvas, worldPositionStays: true);
		anchorObject.GetComponent<CurvePointControl>().objectNumber = pointIndex++;
		GameObject obj = Object.Instantiate(controlPoint, controlLine.points2[pointIndex], Quaternion.identity);
		obj.transform.SetParent(anchorObject.transform, worldPositionStays: true);
		obj.GetComponent<CurvePointControl>().objectNumber = pointIndex++;
	}

	public void UpdateLine(int objectNumber, Vector2 pos)
	{
		Vector2 vector = controlLine.points2[objectNumber];
		controlLine.points2[objectNumber] = pos;
		int num = objectNumber / 4;
		int num2 = num * 4;
		line.MakeCurve(controlLine.points2[num2], controlLine.points2[num2 + 1], controlLine.points2[num2 + 2], controlLine.points2[num2 + 3], segments, num * (segments + 1));
		if (objectNumber % 2 == 0)
		{
			controlLine.points2[objectNumber + 1] += pos - vector;
			if (objectNumber > 0 && objectNumber < controlLine.points2.Count - 2)
			{
				controlLine.points2[objectNumber + 2] = pos;
				controlLine.points2[objectNumber + 3] += pos - vector;
				line.MakeCurve(controlLine.points2[num2 + 4], controlLine.points2[num2 + 5], controlLine.points2[num2 + 6], controlLine.points2[num2 + 7], segments, (num + 1) * (segments + 1));
			}
		}
		line.Draw();
		controlLine.Draw();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(20f, 20f, 100f, 30f), "Add Point"))
		{
			AddPoint();
		}
		GUI.Label(new Rect(20f, 59f, 200f, 30f), "Curve resolution: " + segments);
		segments = (int)GUI.HorizontalSlider(new Rect(20f, 80f, 150f, 30f), segments, 3f, 60f);
		if (oldSegments != segments)
		{
			oldSegments = segments;
			ChangeSegments();
		}
		useDottedLine = GUI.Toggle(new Rect(20f, 105f, 80f, 20f), useDottedLine, " Dotted line");
		if (oldDottedLineSetting != useDottedLine)
		{
			oldDottedLineSetting = useDottedLine;
			SetLine();
			line.Draw();
		}
		GUILayout.BeginArea(new Rect(20f, 150f, 150f, 800f));
		if (GUILayout.Button(listPoints ? "Hide points" : "List points", GUILayout.Width(100f)))
		{
			listPoints = !listPoints;
		}
		if (listPoints)
		{
			int num = 0;
			for (int i = 0; i < controlLine.points2.Count; i += 2)
			{
				GUILayout.Label("Anchor " + num + ": (" + (int)controlLine.points2[i].x + ", " + (int)controlLine.points2[i].y + ")");
				GUILayout.Label("Control " + num++ + ": (" + (int)controlLine.points2[i + 1].x + ", " + (int)controlLine.points2[i + 1].y + ")");
			}
		}
		GUILayout.EndArea();
	}

	private void AddPoint()
	{
		if (line.points2.Count + controlLine.points2.Count + segments + 4 <= 16383)
		{
			controlLine.points2.Add(controlLine.points2[pointIndex - 2]);
			controlLine.points2.Add(controlLine.points2[pointIndex - 1]);
			Vector2 vector = (controlLine.points2[pointIndex - 2] - controlLine.points2[pointIndex - 4]) * 0.25f;
			controlLine.points2.Add(controlLine.points2[pointIndex - 2] + vector);
			controlLine.points2.Add(controlLine.points2[pointIndex - 1] + vector);
			if (controlLine.points2[pointIndex + 2].x > (float)Screen.width || controlLine.points2[pointIndex + 2].y > (float)Screen.height || controlLine.points2[pointIndex + 2].x < 0f || controlLine.points2[pointIndex + 2].y < 0f)
			{
				controlLine.points2[pointIndex + 2] = controlLine.points2[pointIndex - 2] - vector;
				controlLine.points2[pointIndex + 3] = controlLine.points2[pointIndex - 1] - vector;
			}
			Vector2 vector2 = controlLine.points2[pointIndex - 1] + (controlLine.points2[pointIndex] - controlLine.points2[pointIndex - 1]) * 2f;
			pointIndex++;
			controlLine.points2[pointIndex] = vector2;
			GameObject obj = Object.Instantiate(controlPoint, vector2, Quaternion.identity);
			obj.transform.SetParent(anchorObject.transform, worldPositionStays: true);
			obj.GetComponent<CurvePointControl>().objectNumber = pointIndex++;
			AddControlObjects();
			controlLine.Draw();
			line.Resize((segments + 1) * ++numberOfCurves);
			line.MakeCurve(controlLine.points2[pointIndex - 4], controlLine.points2[pointIndex - 3], controlLine.points2[pointIndex - 2], controlLine.points2[pointIndex - 1], segments, (segments + 1) * (numberOfCurves - 1));
			line.Draw();
		}
	}

	private void ChangeSegments()
	{
		if (segments * 4 * numberOfCurves <= 65534)
		{
			line.Resize((segments + 1) * numberOfCurves);
			for (int i = 0; i < numberOfCurves; i++)
			{
				line.MakeCurve(controlLine.points2[i * 4], controlLine.points2[i * 4 + 1], controlLine.points2[i * 4 + 2], controlLine.points2[i * 4 + 3], segments, (segments + 1) * i);
			}
			line.Draw();
		}
	}

	private void Update()
	{
		if (Screen.width != oldWidth)
		{
			oldWidth = Screen.width;
			ChangeResolution();
		}
	}

	private void ChangeResolution()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("GameController");
		foreach (GameObject gameObject in array)
		{
			gameObject.transform.position = controlLine.points2[gameObject.GetComponent<CurvePointControl>().objectNumber];
		}
	}
}
