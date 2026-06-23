using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SelectionBox : MonoBehaviour
{
	private VectorLine selectionLine;

	private Vector2 originalPos;

	private List<Color32> lineColors;

	private void Start()
	{
		lineColors = new List<Color32>(new Color32[4]);
		selectionLine = new VectorLine("Selection", new List<Vector2>(5), 3f, LineType.Continuous);
		selectionLine.capLength = 1.5f;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 300f, 25f), "Click & drag to make a selection box");
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			StopCoroutine("CycleColor");
			selectionLine.SetColor(Color.white);
			originalPos = Input.mousePosition;
		}
		if (Input.GetMouseButton(0))
		{
			selectionLine.MakeRect(originalPos, Input.mousePosition);
			selectionLine.Draw();
		}
		if (Input.GetMouseButtonUp(0))
		{
			StartCoroutine("CycleColor");
		}
	}

	private IEnumerator CycleColor()
	{
		while (true)
		{
			for (int i = 0; i < 4; i++)
			{
				lineColors[i] = Color.Lerp(Color.yellow, Color.red, Mathf.PingPong((Time.time + (float)i * 0.25f) * 3f, 1f));
			}
			selectionLine.SetColors(lineColors);
			yield return null;
		}
	}
}
