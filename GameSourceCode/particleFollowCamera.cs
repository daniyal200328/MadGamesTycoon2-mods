using UnityEngine;

public class particleFollowCamera : MonoBehaviour
{
	public GameObject cameraObject;

	private void Start()
	{
		base.transform.SetParent(null);
	}

	private void Update()
	{
		base.transform.position = new Vector3(cameraObject.transform.position.x, base.transform.position.y, cameraObject.transform.position.z);
	}
}
