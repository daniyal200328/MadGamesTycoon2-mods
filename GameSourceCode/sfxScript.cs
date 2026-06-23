using System.Collections;
using UnityEngine;

public class sfxScript : MonoBehaviour
{
	private mainScript mS_;

	public AudioClip[] musicClips;

	public GameObject[] sfxObjects;

	private AudioSource[] sfxAudioSource;

	private AudioSource musicSource;

	private settingsScript sS_;

	private savegameScript savegame_;

	public int aktMusik;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if ((bool)mS_)
		{
			return;
		}
		if (!mS_)
		{
			mS_ = GameObject.FindGameObjectWithTag("Main").GetComponent<mainScript>();
		}
		if (!sS_)
		{
			sS_ = GameObject.FindGameObjectWithTag("Main").GetComponent<settingsScript>();
		}
		if (!savegame_)
		{
			savegame_ = GameObject.FindGameObjectWithTag("Main").GetComponent<savegameScript>();
		}
		sfxAudioSource = new AudioSource[sfxObjects.Length];
		for (int i = 0; i < sfxObjects.Length; i++)
		{
			if ((bool)sfxObjects[i])
			{
				sfxAudioSource[i] = sfxObjects[i].GetComponent<AudioSource>();
			}
		}
		musicSource = GetComponent<AudioSource>();
	}

	private void Update()
	{
		PlayMusic();
	}

	public void PlaySound(int i, bool force)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame && (bool)sfxAudioSource[i] && (force || (!force && !sfxAudioSource[i].isPlaying)))
		{
			sfxAudioSource[i].Play();
		}
	}

	public void PlaySound(int i)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame && sfxAudioSource != null && (bool)sfxAudioSource[i])
		{
			sfxAudioSource[i].Play();
		}
	}

	public void Play3DSound(int i, float time, bool force, Vector3 pos)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame)
		{
			float num = time;
			if (mS_.GetGameSpeed() > 0f)
			{
				num /= mS_.GetGameSpeed();
			}
			StartCoroutine(iPlay3DSound(i, num, force, pos));
		}
	}

	private IEnumerator iPlay3DSound(int i, float time, bool force, Vector3 pos)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame)
		{
			yield return new WaitForSeconds(time);
			if (force || (!force && !sfxAudioSource[i].isPlaying))
			{
				sfxObjects[i].transform.position = pos;
				sfxAudioSource[i].Play();
			}
		}
	}

	public void PlaySoundDelay(int i, float time, bool force)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame)
		{
			StartCoroutine(iPlaySoundDelay(i, time / mS_.GetGameSpeed(), force));
		}
	}

	private IEnumerator iPlaySoundDelay(int i, float time, bool force)
	{
		if ((bool)savegame_ && !savegame_.loadingSavegame)
		{
			yield return new WaitForSeconds(time);
			if (force || (!force && !sfxAudioSource[i].isPlaying))
			{
				sfxAudioSource[i].Play();
			}
		}
	}

	public AudioSource GetAudioSource(int i)
	{
		return sfxAudioSource[i];
	}

	private void PlayMusic()
	{
		if (!musicSource.isPlaying)
		{
			SetVolume();
			aktMusik++;
			if (aktMusik >= musicClips.Length)
			{
				aktMusik = 0;
			}
			musicSource.clip = musicClips[aktMusik];
			musicSource.Play();
		}
	}

	public void SetRandomMusic()
	{
		FindScripts();
		musicSource.Stop();
		aktMusik = Random.Range(0, musicClips.Length);
		PlayMusic();
	}

	public void SetVolume()
	{
		AudioListener.volume = sS_.masterVolume;
		musicSource.volume = sS_.musicVolume;
	}
}
