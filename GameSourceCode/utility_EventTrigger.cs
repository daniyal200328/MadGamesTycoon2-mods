using Suimono.Core;
using UnityEngine;

public class utility_EventTrigger : MonoBehaviour
{
	private fx_EffectObject target;

	private void Start()
	{
		target = GetComponent<fx_EffectObject>();
		if (target != null)
		{
			target.OnTrigger += OnTrigger;
		}
		else
		{
			Debug.Log("#EffectTriggerUsage# Can't find fx_EffectObject on " + base.transform.name, base.gameObject);
		}
	}

	private void OnTrigger(Vector3 position, Quaternion rotatoin)
	{
		Debug.LogFormat(base.gameObject, "#EffectTriggerUsage# Trigger, position={0}, rotation={1}", position, rotatoin.eulerAngles);
	}
}
