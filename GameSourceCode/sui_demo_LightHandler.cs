using UnityEngine;

public class sui_demo_LightHandler : MonoBehaviour
{
	public float dayIntensity = 1f;

	public float nightIntensity = 0.01f;

	public float sunsetDegrees = 20f;

	public float lightDegrees = 10f;

	public Color dayColor = new Color(1f, 0.968f, 0.933f, 1f);

	public Color sunsetColor = new Color(0.77f, 0.33f, 0f, 1f);

	private Light lightObject;

	private float lightFac;

	private float sunsetFac;

	private void Start()
	{
		lightObject = base.gameObject.GetComponent<Light>();
	}

	private void LateUpdate()
	{
		if (lightObject != null)
		{
			sunsetDegrees = Mathf.Clamp(sunsetDegrees, 0f, 90f);
			lightFac = base.transform.eulerAngles.x;
			if (lightFac > 90f)
			{
				lightFac = 0f;
			}
			sunsetFac = Mathf.Clamp01(lightFac / sunsetDegrees);
			lightFac = Mathf.Clamp01(lightFac / lightDegrees);
			lightObject.intensity = Mathf.Lerp(nightIntensity, dayIntensity, lightFac);
			if (lightObject.intensity < 0.01f)
			{
				lightObject.intensity = 0.01f;
			}
			lightObject.color = Color.Lerp(sunsetColor, dayColor, sunsetFac);
		}
	}
}
