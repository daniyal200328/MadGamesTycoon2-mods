using UnityEngine;
using Vectrosity;

public class ReallyBasicLine : MonoBehaviour
{
	private void Start()
	{
		VectorLine.SetLine(Color.white, new Vector2(0f, 0f), new Vector2(Screen.width - 1, Screen.height - 1));
	}
}
