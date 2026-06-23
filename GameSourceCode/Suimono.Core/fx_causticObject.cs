using UnityEngine;

namespace Suimono.Core;

public class fx_causticObject : MonoBehaviour
{
	public bool manualPlacement;

	private SuimonoModule moduleObject;

	private fx_causticModule causticObject;

	private Light lightComponent;

	private float heightMult = 1f;

	private void Start()
	{
		moduleObject = GameObject.Find("SUIMONO_Module").GetComponent<SuimonoModule>();
		causticObject = GameObject.Find("_caustic_effects").GetComponent<fx_causticModule>();
		lightComponent = GetComponent<Light>();
	}

	private void LateUpdate()
	{
		if (!causticObject.enableCaustics)
		{
			return;
		}
		lightComponent.cookie = causticObject.useTex;
		lightComponent.cullingMask = moduleObject.causticLayer;
		lightComponent.color = causticObject.causticTint;
		heightMult = 1f;
		if (manualPlacement)
		{
			heightMult = 1f - causticObject.heightFac;
		}
		lightComponent.intensity = causticObject.causticIntensity * heightMult;
		lightComponent.cookieSize = causticObject.causticScale;
		if (causticObject.sceneLightObject != null)
		{
			if (causticObject.inheritLightColor)
			{
				lightComponent.color = causticObject.sceneLightObject.color * causticObject.causticTint;
				lightComponent.intensity *= causticObject.sceneLightObject.intensity;
			}
			else
			{
				lightComponent.color = causticObject.causticTint;
			}
			if (causticObject.inheritLightDirection)
			{
				base.transform.eulerAngles = causticObject.sceneLightObject.transform.eulerAngles;
			}
			else
			{
				base.transform.eulerAngles = new Vector3(90f, 0f, 0f);
			}
		}
	}
}
