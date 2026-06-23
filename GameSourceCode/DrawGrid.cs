using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawGrid : MonoBehaviour
{
	public int gridPixels = 50;

	private VectorLine gridLine;

	private void Start()
	{
		gridLine = new VectorLine("Grid", new List<Vector2>(), 1f);
		gridLine.alignOddWidthToPixels = true;
		MakeGrid();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 30f, 20f), gridPixels.ToString());
		gridPixels = (int)GUI.HorizontalSlider(new Rect(40f, 15f, 590f, 20f), gridPixels, 5f, 200f);
		if (GUI.changed)
		{
			MakeGrid();
		}
	}

	private void MakeGrid()
	{
		int newCount = (Screen.width / gridPixels + 1 + (Screen.height / gridPixels + 1)) * 2;
		gridLine.Resize(newCount);
		int num = 0;
		for (int i = 0; i < Screen.width; i += gridPixels)
		{
			gridLine.points2[num++] = new Vector2(i, 0f);
			gridLine.points2[num++] = new Vector2(i, Screen.height - 1);
		}
		for (int j = 0; j < Screen.height; j += gridPixels)
		{
			gridLine.points2[num++] = new Vector2(0f, j);
			gridLine.points2[num++] = new Vector2(Screen.width - 1, j);
		}
		gridLine.Draw();
	}
}
