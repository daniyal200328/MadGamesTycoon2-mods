using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class SuimonoModuleFX : MonoBehaviour
{
	public string[] effectsLabels;

	public Transform[] effectsSystems;

	public Sui_FX_ClampType systemClampType;

	public Transform[] fxObjects;

	public ParticleSystem[] fxParticles;

	public int[] clampIndex;

	public List<string> clampOptions = new List<string> { "No Clamp", "Clamp to Surface", "Keep Below Surface", "Keep Above Surface" };

	public List<ParticleSystem.Particle> particleReserve = new List<ParticleSystem.Particle>();

	private Transform fxParentObject;

	private SuimonoModule moduleObject;

	private int fx;

	private int px;

	private float currPXWaterPos;

	private ParticleSystem useParticleComponent;

	private ParticleSystem.Particle[] setParticles;

	private Transform[] tempSystems;

	private int[] tempClamp;

	private int aR;

	private int efx;

	private int epx;

	private int sx;

	private int endLP;

	private int setInt;

	public List<string> sysNames = new List<string>();

	public int sN;

	public int s;

	public string setName;

	private static int staggerOffset = 0;

	private static int staggerModulus = 20;

	private float stagger;

	private void Start()
	{
		fxParentObject = base.transform.Find("_particle_effects");
		moduleObject = (SuimonoModule)Object.FindObjectOfType(typeof(SuimonoModule));
		if (Application.isPlaying && effectsSystems.Length != 0 && fxParentObject != null)
		{
			Vector3 position = new Vector3(base.transform.position.x, -10000f, base.transform.position.z);
			fxObjects = new Transform[effectsSystems.Length];
			fxParticles = new ParticleSystem[effectsSystems.Length];
			for (int i = 0; i < effectsSystems.Length; i++)
			{
				Transform transform = Object.Instantiate(effectsSystems[i], position, base.transform.rotation);
				transform.transform.parent = fxParentObject.transform;
				fxObjects[i] = transform;
				fxParticles[i] = transform.gameObject.GetComponent<ParticleSystem>();
			}
		}
		staggerOffset++;
		stagger = ((float)staggerOffset + 0f) * 0.05f;
		staggerOffset %= staggerModulus;
		float repeatRate = 0.25f;
		InvokeRepeating("ClampSystems", 0.15f + stagger, repeatRate);
		InvokeRepeating("UpdateSystems", 0.2f + stagger, 1f);
	}

	private void LateUpdate()
	{
		if (Application.isPlaying)
		{
			return;
		}
		sysNames = new List<string>();
		sysNames.Add("None");
		for (sN = 0; sN < effectsSystems.Length; sN++)
		{
			setName = "---";
			if (effectsSystems[sN] != null)
			{
				setName = effectsSystems[sN].transform.name;
			}
			for (s = 0; s < sN; s++)
			{
				setName += " ";
			}
			sysNames.Add(setName);
		}
	}

	private void UpdateSystems()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		sysNames = new List<string>();
		sysNames.Add("None");
		for (sN = 0; sN < effectsSystems.Length; sN++)
		{
			setName = "---";
			if (effectsSystems[sN] != null)
			{
				setName = effectsSystems[sN].transform.name;
			}
			for (s = 0; s < sN; s++)
			{
				setName += " ";
			}
			sysNames.Add(setName);
		}
	}

	private void ClampSystems()
	{
		for (fx = 0; fx < fxObjects.Length; fx++)
		{
			if (fxObjects[fx] != null && clampIndex[fx] != 0)
			{
				currPXWaterPos = 0f;
				useParticleComponent = fxParticles[fx];
				if (setParticles == null)
				{
					setParticles = new ParticleSystem.Particle[useParticleComponent.particleCount];
				}
				useParticleComponent.GetParticles(setParticles);
				if ((float)useParticleComponent.particleCount > 0f)
				{
					for (px = 0; px < useParticleComponent.particleCount; px++)
					{
						currPXWaterPos = moduleObject.SuimonoGetHeight(setParticles[px].position, "surfaceLevel");
						if (clampIndex[fx] == 1)
						{
							setParticles[px].position = new Vector3(setParticles[px].position.x, currPXWaterPos + 0.2f, setParticles[px].position.z);
						}
						if (clampIndex[fx] == 2 && setParticles[px].position.y > currPXWaterPos - 0.2f)
						{
							setParticles[px].position = new Vector3(setParticles[px].position.x, currPXWaterPos - 0.2f, setParticles[px].position.z);
						}
						if (clampIndex[fx] == 3 && setParticles[px].position.y < currPXWaterPos + 0.2f)
						{
							setParticles[px].position = new Vector3(setParticles[px].position.x, currPXWaterPos + 0.2f, setParticles[px].position.z);
						}
					}
					useParticleComponent.SetParticles(setParticles, setParticles.Length);
					useParticleComponent.Play();
				}
			}
		}
	}

	public void AddSystem()
	{
		tempSystems = effectsSystems;
		tempClamp = clampIndex;
		effectsSystems = new Transform[tempSystems.Length + 1];
		clampIndex = new int[tempClamp.Length + 1];
		for (aR = 0; aR < tempSystems.Length; aR++)
		{
			effectsSystems[aR] = tempSystems[aR];
			clampIndex[aR] = tempClamp[aR];
		}
		effectsSystems[tempSystems.Length] = null;
		clampIndex[tempClamp.Length] = 0;
	}

	public void AddParticle(ParticleSystem.Particle particleData)
	{
		particleReserve.Add(particleData);
	}

	private IEnumerator updateFX()
	{
		for (efx = 0; efx < effectsSystems.Length; efx++)
		{
			for (epx = 0; epx < particleReserve.Count; epx++)
			{
				if (Mathf.Floor(particleReserve[epx].angularVelocity) == (float)efx)
				{
					fxParticles[fx].Emit(1);
				}
			}
		}
		for (fx = 0; fx < effectsSystems.Length; fx++)
		{
			for (px = 0; px < particleReserve.Count; px++)
			{
				if (Mathf.Floor(particleReserve[px].angularVelocity) == (float)fx)
				{
					useParticleComponent = fxParticles[fx];
					if (setParticles == null)
					{
						setParticles = new ParticleSystem.Particle[useParticleComponent.particleCount];
					}
					useParticleComponent.GetParticles(setParticles);
					for (sx = useParticleComponent.particleCount - 1; sx < useParticleComponent.particleCount; sx++)
					{
						setParticles[px].position = particleReserve[px].position;
						setParticles[px].startSize = particleReserve[px].startSize;
						setParticles[px].rotation = particleReserve[px].rotation;
						setParticles[px].velocity = particleReserve[px].velocity;
					}
					useParticleComponent.SetParticles(setParticles, setParticles.Length);
				}
			}
		}
		yield return null;
		if (particleReserve == null)
		{
			particleReserve = new List<ParticleSystem.Particle>();
		}
	}

	public void DeleteSystem(int sysNum)
	{
		tempSystems = effectsSystems;
		tempClamp = clampIndex;
		endLP = tempSystems.Length - 1;
		if (endLP <= 0)
		{
			endLP = 0;
			if (effectsSystems == null)
			{
				effectsSystems = new Transform[tempSystems.Length + 1];
			}
			if (clampIndex == null)
			{
				clampIndex = new int[tempSystems.Length + 1];
			}
			return;
		}
		tempSystems = new Transform[endLP];
		setInt = 0;
		for (aR = 0; aR < endLP + 1; aR++)
		{
			if (aR != sysNum)
			{
				tempSystems[setInt] = effectsSystems[aR];
				setInt++;
			}
		}
		effectsSystems = tempSystems;
	}

	private void OnApplicationQuit()
	{
		for (fx = 0; fx < effectsSystems.Length; fx++)
		{
			Object.Destroy(fxObjects[fx]);
		}
	}
}
