using UnityEngine;

namespace Suimono.Core;

public class fx_causticModule : MonoBehaviour
{
	public bool enableCaustics = true;

	public Light sceneLightObject;

	public bool inheritLightColor;

	public bool inheritLightDirection;

	public Color causticTint = new Color(1f, 1f, 1f, 1f);

	public float causticIntensity = 2f;

	public float causticScale = 4f;

	public float heightFac;

	public int causticFPS = 32;

	public Texture2D[] causticFrames;

	public Texture2D useTex;

	private float causticsTime;

	private SuimonoModule moduleObject;

	private GameObject lightObject;

	private int frameIndex;

	private void Start()
	{
		moduleObject = (SuimonoModule)Object.FindObjectOfType(typeof(SuimonoModule));
		lightObject = base.transform.Find("mainCausticObject").gameObject;
	}

	private void LateUpdate()
	{
		if (!base.enabled)
		{
			return;
		}
		useTex = causticFrames[frameIndex];
		causticsTime += Time.deltaTime;
		if (causticsTime > 1f / ((float)causticFPS * 1f))
		{
			causticsTime = 0f;
			frameIndex++;
		}
		if (frameIndex == causticFrames.Length)
		{
			frameIndex = 0;
		}
		if (moduleObject != null)
		{
			if (moduleObject.setLight != null)
			{
				sceneLightObject = moduleObject.setLight;
			}
			if (lightObject != null)
			{
				lightObject.SetActive(moduleObject.enableCaustics);
			}
		}
	}
}
