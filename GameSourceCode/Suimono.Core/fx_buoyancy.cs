using System;
using UnityEngine;

namespace Suimono.Core;

public class fx_buoyancy : MonoBehaviour
{
	public bool applyToParent;

	public bool engageBuoyancy;

	public float activationRange = 5000f;

	public bool inheritForce;

	public bool keepAtSurface;

	public float buoyancyOffset;

	public float buoyancyStrength = 1f;

	public float forceAmount = 1f;

	public float forceHeightFactor;

	private float maxVerticalSpeed = 5f;

	private float surfaceRange = 0.2f;

	private float buoyancy;

	private float surfaceLevel;

	private float underwaterLevel;

	private bool isUnderwater;

	private Transform physTarget;

	private SuimonoModule moduleObject;

	private float waveHeight;

	private float modTime;

	private float splitFac = 1f;

	private Rigidbody rigidbodyComponent;

	private float isOver;

	private Vector2 forceAngles = new Vector2(0f, 0f);

	private float forceSpeed;

	private float waveHt;

	private int randSeed;

	private Random buyRand;

	private Vector3 gizPos;

	private float testObjectHeight;

	private float buoyancyFactor;

	private float forceMod;

	private float waveFac;

	private float[] heightValues;

	private bool isEnabled = true;

	private bool performHeight;

	private float currRange = -1f;

	private Vector3 physPosition;

	private bool saveRigidbodyState;

	private float lerpSurfacePosTime;

	private float targetYPosition;

	private float startYPosition;

	private bool saveKeepAtSurface;

	private void OnDrawGizmos()
	{
		gizPos = base.transform.position;
		gizPos.y += 0.03f;
		Gizmos.DrawIcon(gizPos, "gui_icon_buoy.psd", allowScaling: true);
		gizPos.y -= 0.03f;
	}

	private void Start()
	{
		if (GameObject.Find("SUIMONO_Module") != null)
		{
			moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
		}
		randSeed = Environment.TickCount;
		buyRand = new Random(randSeed);
		if (applyToParent)
		{
			fx_buoyancy[] componentsInChildren = base.transform.parent.gameObject.GetComponentsInChildren<fx_buoyancy>();
			if (componentsInChildren != null)
			{
				splitFac = 1f / (float)componentsInChildren.Length;
			}
		}
		if (applyToParent)
		{
			physTarget = base.transform.parent.transform;
			if (physTarget != null && rigidbodyComponent == null)
			{
				rigidbodyComponent = physTarget.GetComponent<Rigidbody>();
			}
		}
		else
		{
			physTarget = base.transform;
			if (physTarget != null && rigidbodyComponent == null)
			{
				rigidbodyComponent = GetComponent<Rigidbody>();
			}
		}
	}

	private void FixedUpdate()
	{
		SetUpdate();
		if (!isUnderwater)
		{
			maxVerticalSpeed = 0.25f;
		}
		else if (isUnderwater)
		{
			maxVerticalSpeed = Mathf.Clamp(surfaceLevel - (base.transform.position.y + buoyancyOffset - 0.5f), 0f, 5f);
			if (maxVerticalSpeed > 4f)
			{
				maxVerticalSpeed = 4f;
			}
		}
		buoyancy = 1f + maxVerticalSpeed * buoyancyStrength;
	}

	private void SetUpdate()
	{
		if (!(moduleObject != null))
		{
			return;
		}
		if (buyRand == null)
		{
			buyRand = new Random(randSeed);
		}
		performHeight = true;
		if (physTarget != null && moduleObject.setCamera != null)
		{
			if (activationRange > 0f)
			{
				currRange = Vector3.Distance(moduleObject.setCamera.transform.position, physTarget.transform.position);
				if (currRange >= activationRange)
				{
					performHeight = false;
				}
			}
			if (activationRange <= 0f)
			{
				performHeight = true;
			}
			if (!isEnabled)
			{
				performHeight = false;
			}
		}
		if (performHeight)
		{
			heightValues = moduleObject.SuimonoGetHeightAll(base.transform.position);
			isOver = heightValues[4];
			waveHt = heightValues[8];
			surfaceLevel = heightValues[0];
			forceAngles = moduleObject.SuimonoConvertAngleToVector(heightValues[6]);
			forceSpeed = heightValues[7] * 0.1f;
		}
		forceHeightFactor = Mathf.Clamp01(forceHeightFactor);
		isUnderwater = false;
		underwaterLevel = 0f;
		testObjectHeight = base.transform.position.y + buoyancyOffset - 0.5f;
		waveHeight = surfaceLevel;
		if (testObjectHeight < waveHeight)
		{
			isUnderwater = true;
		}
		underwaterLevel = waveHeight - testObjectHeight;
		if (!keepAtSurface && (bool)rigidbodyComponent)
		{
			rigidbodyComponent.isKinematic = saveRigidbodyState;
		}
		if (!keepAtSurface && engageBuoyancy && isOver == 1f && (bool)rigidbodyComponent && !rigidbodyComponent.isKinematic)
		{
			if (rigidbodyComponent.isKinematic)
			{
				rigidbodyComponent.isKinematic = saveRigidbodyState;
			}
			buoyancyFactor = 10f;
			if (isUnderwater)
			{
				if (base.transform.position.y + buoyancyOffset - 0.5f < waveHeight - surfaceRange)
				{
					forceMod = buoyancyFactor * (buoyancy * rigidbodyComponent.mass) * underwaterLevel * splitFac * (isUnderwater ? 1f : 0f);
					if (rigidbodyComponent.velocity.y < maxVerticalSpeed)
					{
						rigidbodyComponent.AddForceAtPosition(new Vector3(0f, 1f, 0f) * forceMod, base.transform.position);
					}
					modTime = 0f;
				}
				else
				{
					modTime = (base.transform.position.y + buoyancyOffset - 0.5f) / (waveHeight + buyRand.Next(0f, 0.25f) * (isUnderwater ? 1f : 0f));
					if (rigidbodyComponent.velocity.y > 0f)
					{
						rigidbodyComponent.velocity = new Vector3(rigidbodyComponent.velocity.x, Mathf.SmoothStep(rigidbodyComponent.velocity.y, 0f, modTime), rigidbodyComponent.velocity.z);
					}
				}
				if (inheritForce && base.transform.position.y + buoyancyOffset - 0.5f <= waveHeight)
				{
					waveFac = Mathf.Lerp(0f, forceHeightFactor, waveHt);
					if (forceHeightFactor == 0f)
					{
						waveFac = 1f;
					}
					rigidbodyComponent.AddForceAtPosition(new Vector3(forceAngles.x, 0f, forceAngles.y) * (buoyancyFactor * 2f) * forceSpeed * waveFac * splitFac * forceAmount, base.transform.position);
				}
			}
		}
		if (!keepAtSurface || isOver != 1f)
		{
			return;
		}
		saveKeepAtSurface = keepAtSurface;
		if (surfaceLevel - physTarget.position.y - buoyancyOffset >= -0.25f)
		{
			if (rigidbodyComponent != null && !rigidbodyComponent.isKinematic)
			{
				saveRigidbodyState = false;
				rigidbodyComponent.isKinematic = true;
			}
			physPosition = physTarget.position;
			physPosition.y = Mathf.Lerp(startYPosition, targetYPosition, lerpSurfacePosTime);
			physTarget.position = physPosition;
		}
		else
		{
			rigidbodyComponent.isKinematic = saveRigidbodyState;
		}
		lerpSurfacePosTime += Time.deltaTime * 4f;
		if (lerpSurfacePosTime > 1f || keepAtSurface != saveKeepAtSurface)
		{
			lerpSurfacePosTime = 0f;
			startYPosition = physTarget.position.y;
			targetYPosition = surfaceLevel - buoyancyOffset;
		}
	}
}
