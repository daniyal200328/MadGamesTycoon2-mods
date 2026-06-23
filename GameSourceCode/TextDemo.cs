using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class TextDemo : MonoBehaviour
{
	public string text = "Vectrosity!";

	public int textSize = 40;

	private VectorLine textLine;

	private void Start()
	{
		textLine = new VectorLine("Text", new List<Vector2>(), 1f);
		textLine.color = Color.yellow;
		textLine.drawTransform = base.transform;
		textLine.MakeText(text, new Vector2(Screen.width / 2 - text.Length * textSize / 2, Screen.height / 2 + textSize / 2), textSize);
	}

	private void Update()
	{
		base.transform.RotateAround(new Vector2(Screen.width / 2, Screen.height / 2), Vector3.forward, Time.deltaTime * 45f);
		Vector3 localScale = base.transform.localScale;
		localScale.x = 1f + Mathf.Sin(Time.time * 3f) * 0.3f;
		localScale.y = 1f + Mathf.Cos(Time.time * 3f) * 0.3f;
		base.transform.localScale = localScale;
		textLine.Draw();
	}
}
