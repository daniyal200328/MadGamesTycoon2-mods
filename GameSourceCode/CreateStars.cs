using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class CreateStars : MonoBehaviour
{
	public int numberOfStars = 2000;

	private VectorLine stars;

	private void Start()
	{
		Vector3[] array = new Vector3[numberOfStars];
		for (int i = 0; i < numberOfStars; i++)
		{
			array[i] = Random.onUnitSphere * 100f;
		}
		float[] array2 = new float[numberOfStars];
		for (int j = 0; j < numberOfStars; j++)
		{
			array2[j] = Random.Range(1.5f, 2.5f);
		}
		Color32[] array3 = new Color32[numberOfStars];
		for (int k = 0; k < numberOfStars; k++)
		{
			float num = Random.value * 0.75f + 0.25f;
			array3[k] = new Color(num, num, num);
		}
		stars = new VectorLine("Stars", new List<Vector3>(array), 1f, LineType.Points);
		stars.SetColors(new List<Color32>(array3));
		stars.SetWidths(new List<float>(array2));
		stars.Draw();
		VectorLine.SetCanvasCamera(Camera.main);
		VectorLine.canvas.planeDistance = Camera.main.farClipPlane - 1f;
	}

	private void LateUpdate()
	{
		stars.Draw();
	}
}
