using System;
using System.Collections.Generic;
using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class fx_EffectObject : MonoBehaviour
{
	public delegate void TriggerHandler(Vector3 position, Quaternion rotatoin);

	public SuimonoModuleFX fxObject;

	public Sui_FX_Rules[] effectRule;

	public float[] effectData;

	public Sui_FX_Rules[] resetRule;

	public string[] effectSystemName;

	public Sui_FX_System[] effectSystem;

	public Vector2 effectDelay = new Vector2(1f, 1f);

	public Vector2 emitTime = new Vector2(1f, 1f);

	public Vector2 emitNum = new Vector2(1f, 1f);

	public Vector2 effectSize = new Vector2(1f, 1f);

	public float emitSpeed;

	public float speedThreshold;

	public float directionMultiplier;

	public bool emitAtWaterLevel;

	public float effectDistance = 100f;

	public AudioClip audioObj;

	public Vector2 audioVol = new Vector2(0.9f, 1f);

	public Vector2 audioPit = new Vector2(0.8f, 1.2f);

	public float audioSpeed;

	public bool enableEvents;

	public Color tintCol = new Color(1f, 1f, 1f, 1f);

	public bool clampRot;

	public int actionIndex = 1;

	[NonSerialized]
	public List<string> actionOptions = new List<string> { "Once", "Repeat", "Specific" };

	public int actionNum = 5;

	public float actionReset = 15f;

	public int typeIndex;

	public int[] ruleIndex;

	[NonSerialized]
	public List<string> ruleOptions = new List<string> { "None", "Object Is Underwater", "Object Is Above Water", "Object Is At Surface", "Object Speed Is Greater Than", "Object Speed Is Less Than", "Water Depth Is Greater Than", "Water Depth Is Less Than" };

	public int systemIndex;

	public List<string> sysNames = new List<string>();

	public float currentSpeed;

	private int actionCount;

	private float actionTimer;

	private Vector3 savePos = new Vector3(0f, 0f, 0f);

	private SuimonoModule moduleObject;

	private float emitTimer;

	private bool delayPass = true;

	private bool actionPass = true;

	private float useSpd;

	private float useAudioSpd;

	private float isOverWater;

	private float currentWaterPos;

	private Vector3 emitPos;

	private bool rulepass;

	private float timerAudio;

	private float timerParticle;

	private float currentCamDistance;

	private Vector3 gizPos;

	private int sN;

	private int s;

	private bool[] ruleCheck;

	private int ruleCKNum;

	private bool[] resetCheck;

	private int rCK;

	private int emitN;

	private float emitS;

	private Vector3 emitV;

	private float emitR;

	private float emitAR;

	private bool rp;

	private float ruleData;

	private float depth;

	private Sui_FX_Rules[] tempRules;

	private int[] tempIndex;

	private float[] tempData;

	private int aR;

	private int endLP;

	private int setInt;

	private float[] heightValues;

	private Transform transf;

	private int randSeed;

	private Random fxRand;

	private static int staggerOffset = 0;

	private static int staggerModulus = 20;

	private float stagger;

	private float _deltaTime;

	public float eventInterval = 1f;

	public bool eventAtSurface;

	private float timerEvent;

	public event TriggerHandler OnTrigger;

	private void OnDrawGizmos()
	{
		gizPos = base.transform.position;
		gizPos.y += 0.03f;
		Gizmos.DrawIcon(gizPos, "gui_icon_fxobj.psd", allowScaling: true);
	}

	private void Start()
	{
		transf = base.transform;
		if ((bool)GameObject.Find("SUIMONO_Module"))
		{
			moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
			if (moduleObject != null)
			{
				fxObject = moduleObject.suimonoModuleLibrary.fxObject;
			}
		}
		if (fxObject != null)
		{
			sysNames = fxObject.sysNames;
		}
		randSeed = Environment.TickCount;
		fxRand = new Random(randSeed);
		staggerOffset++;
		stagger = ((float)staggerOffset + 0f) * 0.05f;
		staggerOffset %= staggerModulus;
		InvokeRepeating("SetUpdate", 0.1f + stagger, 1f / 30f);
	}

	private void SetUpdate()
	{
		if (!(moduleObject != null))
		{
			return;
		}
		_deltaTime = Time.deltaTime;
		actionPass = false;
		if (actionIndex == 0 && actionCount < 1)
		{
			actionPass = true;
		}
		if (actionIndex == 2 && actionCount < actionNum)
		{
			actionPass = true;
		}
		if (actionIndex == 1)
		{
			actionPass = true;
		}
		if (actionCount > 0 && (actionIndex == 0 || actionIndex == 2))
		{
			actionTimer += _deltaTime;
			if (actionTimer > actionReset && actionReset > 0f)
			{
				actionCount = 0;
				actionTimer = 0f;
			}
		}
		if (fxRand == null)
		{
			fxRand = new Random(randSeed);
		}
		if (!Application.isPlaying)
		{
			return;
		}
		if (moduleObject.setTrack != null)
		{
			currentCamDistance = Vector3.Distance(transf.position, moduleObject.setTrack.transform.position);
			if (currentCamDistance <= effectDistance)
			{
				if (savePos != transf.position)
				{
					currentSpeed = Vector3.Distance(savePos, new Vector3(transf.position.x, transf.position.y, transf.position.z)) / _deltaTime;
				}
				savePos = transf.position;
				timerParticle += _deltaTime;
				timerAudio += _deltaTime;
				EmitFX();
				if (timerAudio > audioSpeed)
				{
					timerAudio = 0f;
					EmitSoundFX();
				}
			}
		}
		if (enableEvents)
		{
			timerEvent += _deltaTime;
			BroadcastEvent();
		}
	}

	private void EmitSoundFX()
	{
		if (actionPass && audioObj != null && moduleObject != null && moduleObject.gameObject.activeInHierarchy && rulepass)
		{
			moduleObject.AddSoundFX(audioObj, emitPos, new Vector3(0f, fxRand.Next(audioPit.x, audioPit.y), fxRand.Next(audioVol.x, audioVol.y)));
		}
	}

	private void EmitFX()
	{
		if (!moduleObject.enableInteraction || !Application.isPlaying || !(moduleObject != null) || !moduleObject.gameObject.activeInHierarchy || !actionPass)
		{
			return;
		}
		delayPass = false;
		emitTimer += _deltaTime;
		if (emitTimer >= emitSpeed)
		{
			emitTimer = 0f;
			delayPass = true;
		}
		heightValues = moduleObject.SuimonoGetHeightAll(transf.position);
		currentWaterPos = heightValues[3];
		isOverWater = heightValues[4];
		rulepass = false;
		if (ruleCheck == null)
		{
			ruleCheck = new bool[effectRule.Length];
		}
		ruleCKNum = 0;
		if (resetCheck == null)
		{
			resetCheck = new bool[resetRule.Length];
		}
		if (Application.isPlaying)
		{
			bool flag = false;
			for (rCK = 0; rCK < effectRule.Length; rCK++)
			{
				flag = false;
				ruleData = speedThreshold;
				depth = currentWaterPos;
				if (rCK < effectData.Length)
				{
					ruleData = effectData[rCK];
				}
				if (ruleIndex[rCK] == 0)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 1 && isOverWater == 1f && depth > 0f)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 2 && isOverWater == 1f && depth <= 0f)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 3 && isOverWater == 1f && depth < 0.15f && depth > -0.15f)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 4 && isOverWater == 1f && currentSpeed > ruleData)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 5 && isOverWater == 1f && currentSpeed < ruleData)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 6 && isOverWater == 1f && depth > ruleData)
				{
					flag = true;
				}
				if (ruleIndex[rCK] == 7 && isOverWater == 1f && depth < ruleData)
				{
					flag = true;
				}
				ruleCheck[rCK] = flag;
			}
		}
		for (rCK = 0; rCK < effectRule.Length; rCK++)
		{
			if (ruleCheck[rCK])
			{
				ruleCKNum++;
			}
		}
		if (ruleCKNum == effectRule.Length)
		{
			rulepass = true;
		}
		if (effectRule.Length == 0)
		{
			rulepass = true;
		}
		if (!delayPass || !rulepass)
		{
			return;
		}
		emitN = Mathf.FloorToInt(fxRand.Next(emitNum.x, emitNum.y));
		emitS = fxRand.Next(effectSize.x, effectSize.y);
		emitV = new Vector3(0f, 0f, 0f);
		emitPos = base.transform.position;
		emitR = base.transform.eulerAngles.y - 180f;
		if (!clampRot)
		{
			emitR = fxRand.Next(-30f, 10f);
		}
		emitAR = fxRand.Next(-360f, 360f);
		if (emitAtWaterLevel)
		{
			emitPos = new Vector3(emitPos.x, base.transform.position.y + currentWaterPos - 0.35f, emitPos.z);
		}
		if (directionMultiplier > 0f)
		{
			emitV = base.transform.up * (directionMultiplier * Mathf.Clamp01(currentSpeed / speedThreshold));
		}
		if (timerParticle > emitSpeed)
		{
			timerParticle = 0f;
			if (systemIndex - 1 >= 0)
			{
				emitPos.y += emitS * 0.4f;
				emitPos.x += fxRand.Next(-0.2f, 0.2f);
				emitPos.z += fxRand.Next(-0.2f, 0.2f);
				moduleObject.AddFX(systemIndex - 1, emitPos, emitN, fxRand.Next(0.5f, 0.75f) * emitS, emitR, emitAR, emitV, tintCol);
			}
			actionCount++;
		}
	}

	public void AddRule()
	{
		tempRules = effectRule;
		tempIndex = ruleIndex;
		tempData = effectData;
		effectRule = new Sui_FX_Rules[tempRules.Length + 1];
		ruleIndex = new int[tempRules.Length + 1];
		effectData = new float[tempRules.Length + 1];
		for (aR = 0; aR < tempRules.Length; aR++)
		{
			effectRule[aR] = tempRules[aR];
			ruleIndex[aR] = tempIndex[aR];
			effectData[aR] = tempData[aR];
		}
		effectRule[tempRules.Length] = Sui_FX_Rules.none;
		ruleIndex[tempRules.Length] = 0;
		effectData[tempRules.Length] = 0f;
	}

	public void DeleteRule(int ruleNum)
	{
		tempRules = effectRule;
		tempIndex = ruleIndex;
		tempData = effectData;
		endLP = tempRules.Length - 1;
		if (endLP <= 0)
		{
			endLP = 0;
			effectRule = new Sui_FX_Rules[0];
			ruleIndex = new int[0];
			effectData = new float[0];
			return;
		}
		effectRule = new Sui_FX_Rules[endLP];
		ruleIndex = new int[endLP];
		effectData = new float[endLP];
		setInt = -1;
		for (aR = 0; aR <= endLP; aR++)
		{
			if (aR != ruleNum)
			{
				setInt++;
			}
			else
			{
				setInt += 2;
			}
			if (setInt <= endLP)
			{
				effectRule[aR] = tempRules[setInt];
				ruleIndex[aR] = tempIndex[setInt];
				effectData[aR] = tempData[setInt];
			}
		}
	}

	private void OnDisable()
	{
		CancelInvoke("SetUpdate");
	}

	private void OnEnable()
	{
		staggerOffset++;
		stagger = ((float)staggerOffset + 0f) * 0.05f;
		staggerOffset %= staggerModulus;
		CancelInvoke("SetUpdate");
		InvokeRepeating("SetUpdate", 0.1f + stagger, 0.1f);
	}

	private void BroadcastEvent()
	{
		if (moduleObject.isActiveAndEnabled && rulepass && this.OnTrigger != null && !(timerEvent < eventInterval))
		{
			timerEvent = 0f;
			this.OnTrigger(eventAtSurface ? new Vector3(emitPos.x, transf.position.y + currentWaterPos - 0.35f, emitPos.z) : transf.position, transf.rotation);
		}
	}
}
