using UnityEngine;

public class sui_demo_animCharacter : MonoBehaviour
{
	public bool isWalking;

	public bool isRunning;

	public bool isSprinting;

	public bool isInWater;

	public bool isInWaterDeep;

	public bool isUnderWater;

	public bool isAtSurface;

	public bool isFloating;

	public bool isFalling;

	public bool isInBoat;

	public float moveSideways;

	public float moveForward;

	public float moveVertical;

	public float wetAmount;

	public float gSlope;

	public float useSlope;

	private GameObject cameraObject;

	private Rigidbody physRigidbody;

	private Animation physAnimation;

	private string currClip;

	private string useClip;

	private string defaultClip;

	private float fadeSpeed;

	private float playSpeed = 1f;

	private float animTime;

	private float blinkTime;

	private bool doBlink;

	private float eyelidTime;

	private float randBlinkNum = 2f;

	private float eyeRand;

	private float headRand;

	private float headTgt;

	private float headTime;

	private float randHeadNum = 4f;

	private float randHeadSpd = 4f;

	private Transform boneRoot;

	private Transform boneLEye;

	private Transform boneREye;

	private Transform boneLEyelid;

	private Transform boneREyelid;

	private Transform boneHead;

	private Transform boneNeck;

	private void Start()
	{
		physRigidbody = GetComponent<Rigidbody>();
		physAnimation = GetComponent<Animation>();
		useClip = "anim_miho_idle_normal";
		defaultClip = useClip;
		SetBoneTransforms();
	}

	private void LateUpdate()
	{
		if (!isInWater)
		{
			wetAmount -= Time.deltaTime * 0.05f;
			wetAmount = Mathf.Clamp(wetAmount, 0f, 1f);
		}
		else
		{
			wetAmount = 1f;
		}
		useClip = defaultClip;
		playSpeed = 1f;
		if (!isInBoat)
		{
			useClip = "anim_miho_idle_normal";
			fadeSpeed = 1.2f;
			playSpeed = 1f;
			if (isWalking)
			{
				useClip = "anim_miho_walk_normal";
				fadeSpeed = 0.5f;
				playSpeed = 1.1f;
				if (moveForward != 0f && moveSideways != 0f)
				{
					fadeSpeed = 0.5f;
					playSpeed = 1.1f;
				}
			}
			if (isRunning)
			{
				useClip = "anim_miho_run_normal";
				fadeSpeed = 0.8f;
				playSpeed = 0.9f;
				if (moveForward != 0f && moveSideways != 0f)
				{
					fadeSpeed = 0.8f;
					playSpeed = 0.9f;
				}
			}
			if (isSprinting)
			{
				useClip = "anim_miho_sprint_normal";
				fadeSpeed = 1.3f;
				playSpeed = 1.1f;
				if (moveForward != 0f && moveSideways != 0f)
				{
					fadeSpeed = 0.3f;
					playSpeed = 1.1f;
				}
			}
			if (isInWater)
			{
				wetAmount = 1f;
			}
			if (isInWaterDeep)
			{
				wetAmount = 1f;
				if (isWalking)
				{
					useClip = "anim_miho_walk_water";
					fadeSpeed = 0.8f;
					playSpeed = 0.8f;
				}
			}
			if (isUnderWater)
			{
				wetAmount = 1f;
				useClip = "anim_miho_swim_idle";
				fadeSpeed = 1.2f;
				playSpeed = 1f;
				if (isWalking || isRunning)
				{
					useClip = "anim_miho_swim_forward";
					fadeSpeed = 1.8f;
					playSpeed = 1f;
					if (isRunning)
					{
						playSpeed = 1.4f;
					}
				}
				if (physRigidbody != null)
				{
					physRigidbody.useGravity = false;
				}
				if (physRigidbody != null)
				{
					physRigidbody.Sleep();
				}
			}
			if (isAtSurface)
			{
				useClip = "anim_miho_swim_surface_idle";
				fadeSpeed = 0.8f;
				playSpeed = 1f;
				if (physRigidbody != null)
				{
					physRigidbody.useGravity = true;
				}
			}
		}
		else if (isInBoat)
		{
			useClip = "anim_miho_boat_sit_idle";
			fadeSpeed = 0.4f;
			playSpeed = 1f;
		}
		animTime += Time.deltaTime;
		if (physAnimation[useClip] != null && physAnimation[currClip] != null)
		{
			physAnimation[useClip].time = physAnimation[currClip].time;
		}
		currClip = useClip;
		animTime = 0f;
		if (physAnimation[currClip] != null)
		{
			physAnimation.CrossFade(currClip, fadeSpeed);
			physAnimation[currClip].speed = playSpeed;
			if (gSlope > 0f && useSlope > 15f && useSlope < 90f && (isWalking || isRunning || isSprinting))
			{
				physAnimation.Blend("anim_miho_walk_water", useSlope / 90f * 2f, 0.1f);
			}
			if (isFalling)
			{
				physAnimation.Blend("anim_miho_fall_normal", 1f, 0.1f);
			}
		}
		else
		{
			Debug.Log("animation " + currClip + " cannot be found!");
		}
		if (!doBlink)
		{
			blinkTime += Time.smoothDeltaTime;
			if (blinkTime > randBlinkNum)
			{
				blinkTime = 0f;
				randBlinkNum = Random.Range(2f, 4f);
				doBlink = true;
			}
		}
		headTime += Time.smoothDeltaTime;
		if (headTime > randHeadNum)
		{
			headTime = 0f;
			if (Random.Range(0f, 5f) > 0.3f)
			{
				headTgt = 0f;
			}
			else
			{
				headTgt = Random.Range(-80f, 80f);
			}
			randHeadNum = Random.Range(2f, 7f);
			randHeadSpd = Random.Range(1f, 5f);
		}
		if (isRunning || isSprinting)
		{
			headTgt = 0f;
			randHeadSpd = 5f;
		}
		headRand = Mathf.SmoothStep(headRand, headTgt, Time.deltaTime * randHeadSpd);
		eyeRand = Mathf.SmoothStep(eyeRand, headTgt * 0.75f, Time.deltaTime * (randHeadSpd * 2f));
		if (eyeRand >= 35f)
		{
			eyeRand = 35f;
		}
		if (eyeRand <= -35f)
		{
			eyeRand = -35f;
		}
		if (doBlink)
		{
			float num = 0.5f;
			eyelidTime += Time.deltaTime;
			if (eyelidTime <= num)
			{
				boneLEyelid.transform.localEulerAngles = new Vector3(boneLEyelid.transform.localEulerAngles.x, boneLEyelid.transform.localEulerAngles.y, Mathf.SmoothStep(265f, 295f, eyelidTime * 5f));
				boneREyelid.transform.localEulerAngles = new Vector3(boneREyelid.transform.localEulerAngles.x, boneREyelid.transform.localEulerAngles.y, Mathf.SmoothStep(265f, 295f, eyelidTime * 5f));
			}
			if (eyelidTime > num)
			{
				eyelidTime = 0f;
				doBlink = false;
			}
		}
		else
		{
			boneLEyelid.transform.localEulerAngles = new Vector3(boneLEyelid.transform.localEulerAngles.x, boneLEyelid.transform.localEulerAngles.y, 295f);
			boneREyelid.transform.localEulerAngles = new Vector3(boneREyelid.transform.localEulerAngles.x, boneREyelid.transform.localEulerAngles.y, 295f);
		}
		boneHead.transform.localEulerAngles = new Vector3(headRand, boneHead.transform.localEulerAngles.y, boneHead.transform.localEulerAngles.z);
		boneNeck.transform.localEulerAngles = new Vector3(headRand * 0.5f, boneNeck.transform.localEulerAngles.y, boneNeck.transform.localEulerAngles.z);
		boneLEye.transform.localEulerAngles = new Vector3(eyeRand, boneLEye.transform.localEulerAngles.y, boneLEye.transform.localEulerAngles.z);
		boneREye.transform.localEulerAngles = new Vector3(eyeRand, boneREye.transform.localEulerAngles.y, boneREye.transform.localEulerAngles.z);
	}

	private void resetPos()
	{
		float y = base.transform.position.y;
		base.transform.position = new Vector3(boneRoot.transform.position.x, y, boneRoot.transform.position.z);
	}

	private void SetBoneTransforms()
	{
		boneRoot = base.transform.Find("Bip01");
		boneNeck = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck");
		boneHead = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head");
		boneLEye = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head/Bip01 EyeLeft");
		boneREye = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head/Bip01 EyeRight");
		boneLEyelid = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head/Bip01 EyeLidLeft");
		boneREyelid = base.transform.Find("Bip01/Bip01 Pelvis/Bip01 Spine/Bip01 Spine1/Bip01 Spine2/Bip01 Spine3/Bip01 Neck/Bip01 Head/Bip01 EyeLidRight");
		boneHead.transform.localEulerAngles = new Vector3(headRand, boneHead.transform.localEulerAngles.y, boneHead.transform.localEulerAngles.z);
	}
}
