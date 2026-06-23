using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class ScribbleCube : MonoBehaviour
{
	public Texture lineTexture;

	public Material lineMaterial;

	public int lineWidth = 14;

	private Color color1 = Color.green;

	private Color color2 = Color.blue;

	private VectorLine line;

	private List<Color32> lineColors;

	private int numberOfPoints = 350;

	private void Start()
	{
		line = new VectorLine("Line", new List<Vector3>(numberOfPoints), lineTexture, lineWidth, LineType.Continuous);
		line.material = lineMaterial;
		line.drawTransform = base.transform;
		LineSetup(resize: false);
	}

	private void LineSetup(bool resize)
	{
		if (resize)
		{
			lineColors = null;
			line.Resize(numberOfPoints);
		}
		for (int i = 0; i < line.points3.Count; i++)
		{
			line.points3[i] = new Vector3(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(-5f, 5f));
		}
		SetLineColors();
	}

	private void SetLineColors()
	{
		if (lineColors == null)
		{
			lineColors = new List<Color32>(new Color32[numberOfPoints - 1]);
		}
		for (int i = 0; i < lineColors.Count; i++)
		{
			lineColors[i] = Color.Lerp(color1, color2, (float)i / (float)lineColors.Count);
		}
		line.SetColors(lineColors);
	}

	private void LateUpdate()
	{
		line.Draw();
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(20f, 10f, 250f, 30f), "Zoom with scrollwheel or arrow keys");
		if (GUI.Button(new Rect(20f, 50f, 100f, 30f), "Change colors"))
		{
			int num = Random.Range(0, 3);
			int num2 = 0;
			do
			{
				num2 = Random.Range(0, 3);
			}
			while (num2 == num);
			color1 = RandomColor(color1, num);
			color2 = RandomColor(color2, num2);
			SetLineColors();
		}
		GUI.Label(new Rect(20f, 100f, 150f, 30f), "Number of points: " + numberOfPoints);
		numberOfPoints = (int)GUI.HorizontalSlider(new Rect(20f, 130f, 120f, 30f), numberOfPoints, 50f, 1000f);
		if (GUI.Button(new Rect(160f, 120f, 40f, 30f), "Set"))
		{
			LineSetup(resize: true);
		}
	}

	private Color RandomColor(Color color, int component)
	{
		for (int i = 0; i < 3; i++)
		{
			if (i == component)
			{
				color[i] = Random.value * 0.25f;
			}
			else
			{
				color[i] = Random.value * 0.5f + 0.5f;
			}
		}
		return color;
	}
}
