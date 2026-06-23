using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class SelectionBox2 : MonoBehaviour
{
	public Texture lineTexture;

	public float textureScale = 4f;

	private VectorLine selectionLine;

	private Vector2 originalPos;

	private void Start()
	{
		selectionLine = new VectorLine("Selection", new List<Vector2>(5), lineTexture, 4f, LineType.Continuous);
		selectionLine.textureScale = textureScale;
		selectionLine.alignOddWidthToPixels = true;
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(10f, 10f, 300f, 25f), "Click & drag to make a selection box");
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			originalPos = Input.mousePosition;
		}
		if (Input.GetMouseButton(0))
		{
			selectionLine.MakeRect(originalPos, Input.mousePosition);
			selectionLine.Draw();
		}
		selectionLine.textureOffset = (0f - Time.time) * 2f % 1f;
	}
}
