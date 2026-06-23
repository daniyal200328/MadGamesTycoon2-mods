using UnityEngine;

public class cullUiImage : MonoBehaviour
{
	private void Start()
	{
		GetComponent<CanvasRenderer>().EnableRectClipping(new Rect(-Screen.width / 2, -Screen.height / 2, Screen.width, Screen.height));
		Object.Destroy(this);
	}
}
