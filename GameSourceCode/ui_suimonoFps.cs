using UnityEngine;
using UnityEngine.UI;

public class ui_suimonoFps : MonoBehaviour
{
	public Text textObj_fps;

	public bool showFPS = true;

	private float updateInterval = 0.5f;

	private float accum;

	private float frames;

	private float timeleft;

	private void Start()
	{
		InvokeRepeating("SetType", 0.1f, 0.5f);
	}

	private void LateUpdate()
	{
		if (showFPS)
		{
			timeleft -= Time.deltaTime;
			accum += Time.timeScale / Time.deltaTime;
			frames += 1f;
			if (timeleft <= 0f)
			{
				timeleft = updateInterval;
				accum = 0f;
				frames = 0f;
			}
		}
		else
		{
			textObj_fps.text = "";
		}
	}

	private void SetType()
	{
		if (textObj_fps != null && accum > 0f && frames > 0f)
		{
			textObj_fps.text = "FPS: " + (accum / frames).ToString("f0");
		}
	}
}
