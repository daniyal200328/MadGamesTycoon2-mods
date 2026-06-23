using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawPoints : MonoBehaviour
{
	public float dotSize = 2f;

	public int numberOfDots = 100;

	public int numberOfRings = 8;

	public Color dotColor = Color.cyan;

	private void Start()
	{
		int num = numberOfDots * numberOfRings;
		Vector2[] collection = new Vector2[num];
		Color32[] array = new Color32[num];
		float num2 = 1f - 0.75f / (float)num;
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = dotColor;
			dotColor *= num2;
		}
		VectorLine vectorLine = new VectorLine("Dots", new List<Vector2>(collection), dotSize, LineType.Points);
		vectorLine.SetColors(new List<Color32>(array));
		for (int j = 0; j < numberOfRings; j++)
		{
			vectorLine.MakeCircle(new Vector2(Screen.width / 2, Screen.height / 2), Screen.height / (j + 2), numberOfDots, numberOfDots * j);
		}
		vectorLine.Draw();
	}
}
