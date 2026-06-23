using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SelectLine : MonoBehaviour
{
	public float lineThickness = 10f;

	public int extraThickness = 2;

	public int numberOfLines = 2;

	private VectorLine[] lines;

	private bool[] wasSelected;

	private void Start()
	{
		lines = new VectorLine[numberOfLines];
		wasSelected = new bool[numberOfLines];
		for (int i = 0; i < numberOfLines; i++)
		{
			lines[i] = new VectorLine("SelectLine", new List<Vector2>(5), lineThickness, LineType.Continuous, Joins.Fill);
			SetPoints(i);
		}
	}

	private void SetPoints(int i)
	{
		for (int j = 0; j < lines[i].points2.Count; j++)
		{
			lines[i].points2[j] = new Vector2(Random.Range(0, Screen.width), Random.Range(0, Screen.height - 20));
		}
		lines[i].Draw();
	}

	private void Update()
	{
		for (int i = 0; i < numberOfLines; i++)
		{
			if (lines[i].Selected(Input.mousePosition, extraThickness, out var _))
			{
				if (!wasSelected[i])
				{
					lines[i].SetColor(Color.green);
					wasSelected[i] = true;
				}
				if (Input.GetMouseButtonDown(0))
				{
					SetPoints(i);
				}
			}
			else if (wasSelected[i])
			{
				wasSelected[i] = false;
				lines[i].SetColor(Color.white);
			}
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 800f, 30f), "Click a line to make a new line");
	}
}
