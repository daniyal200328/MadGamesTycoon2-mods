using UnityEngine;

public class utility_AdjustCulling : MonoBehaviour
{
	public float nearCullAtBase = 0.3f;

	public float nearCullAtAltitude = 5f;

	public float altitudeLowerThreshold = 50f;

	public float altitudeUpperThreshold = 250f;

	private Camera cam;

	private float useThreshold;

	private void Start()
	{
		cam = base.gameObject.GetComponent<Camera>();
	}

	private void LateUpdate()
	{
		if (cam != null)
		{
			if (base.transform.position.y > altitudeLowerThreshold)
			{
				useThreshold = Mathf.Clamp01((base.transform.position.y - altitudeLowerThreshold) / (altitudeUpperThreshold - altitudeLowerThreshold));
			}
			else
			{
				useThreshold = 0f;
			}
			cam.nearClipPlane = Mathf.Lerp(nearCullAtBase, nearCullAtAltitude, useThreshold);
		}
	}
}
