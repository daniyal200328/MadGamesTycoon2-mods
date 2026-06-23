using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class MaskLine2 : MonoBehaviour
{
	public int numberOfPoints = 100;

	public Color lineColor = Color.yellow;

	public GameObject mask;

	public float lineWidth = 9f;

	public float lineHeight = 17f;

	private VectorLine spikeLine;

	private float t;

	private Vector3 startPos;

	private void Start()
	{
		spikeLine = new VectorLine("SpikeLine", new List<Vector3>(numberOfPoints), 2f, LineType.Continuous);
		float num = lineHeight / 2f;
		for (int i = 0; i < numberOfPoints; i++)
		{
			spikeLine.points3[i] = new Vector2(Random.Range((0f - lineWidth) / 2f, lineWidth / 2f), num);
			num -= lineHeight / (float)numberOfPoints;
		}
		spikeLine.color = lineColor;
		spikeLine.drawTransform = base.transform;
		spikeLine.SetMask(mask);
		startPos = base.transform.position;
	}

	private void Update()
	{
		t = Mathf.Repeat(t + Time.deltaTime, 360f);
		base.transform.position = new Vector2(startPos.x, startPos.y + Mathf.Cos(t) * 4f);
		spikeLine.Draw();
	}
}
