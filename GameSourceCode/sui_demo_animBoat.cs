using UnityEngine;

public class sui_demo_animBoat : MonoBehaviour
{
	public GameObject propObject;

	public GameObject rudderObject;

	public float propellerSpeed;

	public float engineRotation;

	public Transform playerPosition;

	public Transform playerExit;

	public AudioClip audioEngineStart;

	public AudioClip audioEngineStop;

	public AudioClip audioEngineIdle;

	public AudioClip audioEngineRev;

	public AudioClip audioEngineRevHigh;

	public AudioClip audioEngineRevAbove;

	public bool behaviorIsOn;

	public bool behaviorIsInWater;

	public bool behaviorIsRevving;

	public bool behaviorIsRevvingBack;

	public bool behaviorIsRevvingHigh;

	private AudioSource audioObjectA;

	private AudioSource audioObjectB;

	private AudioClip useClip;

	private AudioClip currentClip;

	private float engineRot = 90f;

	private bool isOn;

	private float onTime;

	private float propSpd;

	private void Awake()
	{
		GameObject gameObject = new GameObject();
		gameObject.name = "BoatAudioObjectA";
		gameObject.AddComponent<AudioSource>();
		gameObject.transform.position = base.transform.position;
		gameObject.transform.parent = base.transform;
		audioObjectA = gameObject.GetComponent<AudioSource>();
		GameObject gameObject2 = new GameObject();
		gameObject2.name = "BoatAudioObjectB";
		gameObject2.AddComponent<AudioSource>();
		gameObject2.transform.position = base.transform.position;
		gameObject2.transform.parent = base.transform;
		audioObjectB = gameObject2.GetComponent<AudioSource>();
	}

	private void LateUpdate()
	{
		if (rudderObject != null)
		{
			if (engineRotation == 0f)
			{
				engineRot = Mathf.Lerp(engineRot, 90f, Time.deltaTime * 2.5f);
			}
			else
			{
				engineRot = Mathf.Lerp(engineRot, 90f - 60f * engineRotation, Time.deltaTime);
			}
			rudderObject.transform.localEulerAngles = new Vector3(rudderObject.transform.localEulerAngles.x, engineRot, rudderObject.transform.localEulerAngles.z);
		}
		if (propObject != null)
		{
			propSpd = 0f;
			if (behaviorIsOn)
			{
				propSpd = 200f;
				if (behaviorIsRevving)
				{
					propSpd = 1200f;
				}
				if (behaviorIsRevvingHigh)
				{
					propSpd = 3000f;
				}
				if (behaviorIsRevvingBack)
				{
					propSpd = -800f;
				}
			}
			propellerSpeed = Mathf.Lerp(propellerSpeed, propSpd, Time.deltaTime);
			propObject.transform.localEulerAngles = new Vector3(propObject.transform.localEulerAngles.x, propObject.transform.localEulerAngles.y, propObject.transform.localEulerAngles.z + Time.deltaTime * propellerSpeed);
		}
		if (!(audioObjectA != null) || !(audioObjectB != null))
		{
			return;
		}
		float num = 1f;
		audioObjectA.minDistance = 10f;
		audioObjectA.maxDistance = 30f;
		audioObjectB.minDistance = 10f;
		audioObjectB.maxDistance = 30f;
		if (behaviorIsOn)
		{
			audioObjectA.loop = true;
			audioObjectB.loop = true;
			if (isOn)
			{
				useClip = audioEngineIdle;
				if (behaviorIsRevving)
				{
					num = 10f;
					if (currentClip == audioEngineRevAbove)
					{
						num = 10f;
					}
					if (currentClip == audioEngineRevHigh)
					{
						num = 10f;
					}
					useClip = audioEngineRev;
					if (behaviorIsRevvingHigh)
					{
						num = 10f;
						useClip = audioEngineRevHigh;
					}
				}
			}
			if (!isOn)
			{
				onTime += Time.deltaTime;
				if (onTime >= 1f)
				{
					isOn = true;
				}
				num = 10f;
				useClip = audioEngineStart;
			}
		}
		else
		{
			audioObjectA.loop = false;
			audioObjectB.loop = false;
			if (isOn)
			{
				onTime -= Time.deltaTime;
				if (onTime <= -0.5f)
				{
					isOn = false;
				}
				num = 10f;
				useClip = audioEngineStop;
			}
			else
			{
				onTime = 0f;
				isOn = false;
				if (audioObjectA.isPlaying)
				{
					audioObjectA.Stop();
				}
				if (audioObjectB.isPlaying)
				{
					audioObjectB.Stop();
				}
			}
		}
		if (currentClip != useClip)
		{
			audioObjectA.Stop();
			audioObjectA.clip = useClip;
			audioObjectA.volume = 0f;
			audioObjectB.Stop();
			audioObjectB.clip = currentClip;
			audioObjectB.volume = 1f;
			currentClip = useClip;
		}
		audioObjectA.volume = Mathf.Lerp(audioObjectA.volume, 1f, Time.deltaTime * num);
		audioObjectB.volume = Mathf.Lerp(audioObjectB.volume, 0f, Time.deltaTime * num);
		if (behaviorIsOn || isOn)
		{
			if (!audioObjectA.isPlaying)
			{
				audioObjectA.Play();
			}
			if (!audioObjectB.isPlaying)
			{
				audioObjectB.Play();
			}
		}
	}
}
