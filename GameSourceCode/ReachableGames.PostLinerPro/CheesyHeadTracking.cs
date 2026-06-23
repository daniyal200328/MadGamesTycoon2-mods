using UnityEngine;

namespace ReachableGames.PostLinerPro;

public class CheesyHeadTracking : MonoBehaviour
{
	public Quaternion startRot;

	private float trackRate = 0.1f;

	private float trackDelay = 2f;

	private float nextTrackStart;

	private void Start()
	{
		startRot = base.transform.rotation;
		trackRate *= Random.value + 0.25f;
		nextTrackStart = Time.time + Random.value * trackDelay;
	}

	private void Update()
	{
		if (Time.time > nextTrackStart)
		{
			Quaternion quaternion = Quaternion.LookRotation(base.transform.position - Camera.main.transform.position, Vector3.up) * startRot;
			if (Quaternion.Angle(quaternion, base.transform.rotation) > 0.01f)
			{
				base.transform.rotation = Quaternion.Lerp(base.transform.rotation, quaternion, trackRate);
			}
			else
			{
				nextTrackStart = Time.time + Random.value * trackDelay;
			}
		}
	}
}
