using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Line : MonoBehaviour
{
	private void Start()
	{
		List<Vector2> list = new List<Vector2>();
		list.Add(new Vector2(0f, Random.Range(0, Screen.height)));
		list.Add(new Vector2(Screen.width - 1, Random.Range(0, Screen.height)));
		new VectorLine("Line", list, 2f).Draw();
	}
}
