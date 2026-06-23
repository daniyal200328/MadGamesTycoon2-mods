using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class UniformTexturedLine : MonoBehaviour
{
	public Texture lineTexture;

	public float lineWidth = 8f;

	public float textureScale = 1f;

	private void Start()
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(0f, Random.Range(0, Screen.height / 2)));
		list.Add(new Vector2(Screen.width - 1, Random.Range(0, Screen.height)));
		VectorLine vectorLine = new VectorLine("Line", list, lineTexture, lineWidth);
		vectorLine.textureScale = textureScale;
		vectorLine.Draw();
	}
}
