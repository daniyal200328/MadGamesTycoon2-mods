using UnityEngine;

public class maschineAnimSound : MonoBehaviour
{
	public AudioSource mySound;

	public Animation myAnimation;

	private mainScript mS_;

	private GameObject main_;

	private float oldGamespeed;

	private void Start()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
	}

	private void Update()
	{
		if (!myAnimation.isPlaying)
		{
			if (mySound.isPlaying)
			{
				mySound.Stop();
			}
			return;
		}
		if (!mySound.isPlaying)
		{
			mySound.Play();
		}
		if (oldGamespeed != mS_.GetGameSpeed())
		{
			oldGamespeed = mS_.GetGameSpeed();
			mySound.pitch = mS_.GetGameSpeed();
		}
	}
}
