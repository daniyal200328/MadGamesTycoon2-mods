using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MaskLine1 : MonoBehaviour
{
	public int numberOfRects = 30;

	public Color lineColor = Color.green;

	public GameObject mask;

	public float moveSpeed = 2f;

	private VectorLine rectLine;

	private float t;

	private Vector3 startPos;

	private void Start()
	{
		rectLine = new VectorLine("Rects", new List<Vector3>(numberOfRects * 8), 2f);
		int num = 0;
		for (int i = 0; i < numberOfRects; i++)
		{
			rectLine.MakeRect(new Rect(Random.Range(-5f, 5f), Random.Range(-5f, 5f), Random.Range(0.25f, 3f), Random.Range(0.25f, 2f)), num);
			num += 8;
		}
		rectLine.color = lineColor;
		rectLine.capLength = 1f;
		rectLine.drawTransform = base.transform;
		rectLine.SetMask(mask);
		startPos = base.transform.position;
	}

	private void Update()
	{
		t = Mathf.Repeat(t + Time.deltaTime * moveSpeed, 360f);
		base.transform.position = new Vector2(startPos.x + Mathf.Sin(t) * 1.5f, startPos.y + Mathf.Cos(t) * 1.5f);
		rectLine.Draw();
	}
}
