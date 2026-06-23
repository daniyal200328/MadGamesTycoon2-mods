using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class PowerBar : MonoBehaviour
{
	public float speed = 0.25f;

	public int lineWidth = 25;

	public float radius = 60f;

	public int segmentCount = 200;

	private VectorLine bar;

	private Vector2 position;

	private float currentPower;

	private float targetPower;

	private void Start()
	{
		position = new Vector2(radius + 20f, (float)Screen.height - (radius + 20f));
		VectorLine vectorLine = new VectorLine("BarBackground", new List<Vector2>(50), null, lineWidth, LineType.Continuous, Joins.Weld);
		vectorLine.MakeCircle(position, radius);
		vectorLine.Draw();
		bar = new VectorLine("TotalBar", new List<Vector2>(segmentCount + 1), null, lineWidth - 4, LineType.Continuous, Joins.Weld);
		bar.color = Color.black;
		bar.MakeArc(position, radius, radius, 0f, 270f);
		bar.Draw();
		currentPower = Random.value;
		SetTargetPower();
		bar.SetColor(Color.red, 0, (int)Mathf.Lerp(0f, segmentCount, currentPower));
	}

	private void SetTargetPower()
	{
		targetPower = Random.value;
	}

	private void Update()
	{
		float t = currentPower;
		if (targetPower < currentPower)
		{
			currentPower -= speed * Time.deltaTime;
			if (currentPower < targetPower)
			{
				SetTargetPower();
			}
			bar.SetColor(Color.black, (int)Mathf.Lerp(0f, segmentCount, currentPower), (int)Mathf.Lerp(0f, segmentCount, t));
		}
		else
		{
			currentPower += speed * Time.deltaTime;
			if (currentPower > targetPower)
			{
				SetTargetPower();
			}
			bar.SetColor(Color.red, (int)Mathf.Lerp(0f, segmentCount, t), (int)Mathf.Lerp(0f, segmentCount, currentPower));
		}
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(Screen.width / 2 - 40, Screen.height / 2 - 15, 80f, 30f), "Power: " + (currentPower * 100f).ToString("f0") + "%");
	}
}
