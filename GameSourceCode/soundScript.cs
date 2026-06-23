using UnityEngine;

public class soundScript : MonoBehaviour
{
	private AudioSource soundSource;

	private settingsScript sS_;

	private mainScript mS_;

	private GameObject main_;

	private MeshRenderer myRenderer;

	private float orgVolume = 1f;

	private float oldGamespeed = 1f;

	public bool pitchToGamespeed;

	public bool muteOnPausedGame;

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
		if (!sS_)
		{
			sS_ = main_.GetComponent<settingsScript>();
		}
		if (!soundSource)
		{
			soundSource = GetComponent<AudioSource>();
			orgVolume = soundSource.volume;
		}
		myRenderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		if (muteOnPausedGame && mS_.GetGameSpeed() <= 0f)
		{
			soundSource.volume = 0f;
			return;
		}
		if (pitchToGamespeed && oldGamespeed != mS_.GetGameSpeed())
		{
			oldGamespeed = mS_.GetGameSpeed();
			soundSource.pitch = mS_.GetGameSpeed();
		}
		soundSource.volume = orgVolume * sS_.masterVolume;
	}
}
