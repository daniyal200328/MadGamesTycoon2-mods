using UnityEngine;

public class billboardScript : MonoBehaviour
{
	private GameObject camera_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		camera_ = GameObject.Find("Camera");
	}

	private void LateUpdate()
	{
		base.transform.LookAt(base.transform.position + camera_.transform.forward);
		base.transform.position = base.transform.parent.position;
		float num = Vector3.Distance(base.transform.position, camera_.transform.position);
		base.transform.Translate(-Vector3.forward * (num * 1f));
	}
}
