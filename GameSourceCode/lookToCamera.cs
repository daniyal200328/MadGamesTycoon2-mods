using UnityEngine;

public class lookToCamera : MonoBehaviour
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

	private void Update()
	{
		base.gameObject.transform.rotation = camera_.transform.rotation;
		base.gameObject.transform.position = new Vector3(base.transform.position.x, camera_.transform.position.z + 5f, base.transform.position.z);
	}
}
