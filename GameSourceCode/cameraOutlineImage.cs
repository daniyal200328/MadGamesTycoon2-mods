using UnityEngine;
using UnityEngine.UI;

public class cameraOutlineImage : MonoBehaviour
{
	private RawImage myImage;

	public bool blink = true;

	private void Start()
	{
		myImage = GetComponent<RawImage>();
	}

	private void Update()
	{
		if (blink)
		{
			float a = 0.1f + Mathf.PingPong(Time.realtimeSinceStartup * 2f, 1f) * 0.5f;
			myImage.color = new Color(myImage.color.r, myImage.color.g, myImage.color.b, a);
		}
	}
}
