using UnityEngine;

public class fanletterTime : MonoBehaviour
{
	public float anzeigeDauer = 1f;

	public float timer;

	public Animation myAnimation;

	public bool pause;

	private void Start()
	{
	}

	public void Init(float f)
	{
		anzeigeDauer = f;
		timer = 0f;
		pause = false;
	}

	private void Update()
	{
		timer += Time.deltaTime;
		if (!pause)
		{
			if (timer > 1f)
			{
				pause = true;
				myAnimation["showFanLetter"].speed = 0f;
			}
		}
		else if (timer > anzeigeDauer)
		{
			pause = false;
			myAnimation["showFanLetter"].speed = 1f;
		}
	}
}
