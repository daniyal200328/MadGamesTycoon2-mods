using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class cameraCausticsHandler : MonoBehaviour
{
	public bool isUnderwater;

	public Light causticLight;

	public suiCausToolType causticType;

	private bool enableCaustics = true;

	private SuimonoModule moduleObject;

	private void Start()
	{
		moduleObject = (SuimonoModule)Object.FindObjectOfType(typeof(SuimonoModule));
		if (moduleObject != null)
		{
			causticLight = moduleObject.suimonoModuleLibrary.causticObjectLight;
		}
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			causticLight.enabled = false;
		}
	}

	private void OnPreCull()
	{
		if (!(causticLight != null))
		{
			return;
		}
		if (moduleObject != null)
		{
			enableCaustics = moduleObject.enableCaustics;
			if (moduleObject.setLight != null && (!moduleObject.setLight.enabled || !moduleObject.setLight.gameObject.activeSelf))
			{
				enableCaustics = false;
			}
		}
		if (causticType == suiCausToolType.aboveWater)
		{
			causticLight.enabled = false;
		}
		else if (causticType == suiCausToolType.belowWater)
		{
			causticLight.enabled = enableCaustics;
		}
		else
		{
			causticLight.enabled = false;
		}
		if (isUnderwater)
		{
			causticLight.enabled = false;
		}
		if (!Application.isPlaying)
		{
			causticLight.enabled = false;
		}
	}

	private void OnPostRender()
	{
		if (causticLight != null)
		{
			if (isUnderwater)
			{
				causticLight.enabled = true;
			}
			else
			{
				causticLight.enabled = false;
			}
			if (!Application.isPlaying)
			{
				causticLight.enabled = false;
			}
		}
	}
}
