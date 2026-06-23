using UnityEngine;

public class rotateScript : MonoBehaviour
{
	public float rotX;

	public float rotY;

	public float rotZ;

	private void Start()
	{
	}

	private void Update()
	{
		base.transform.Rotate(rotX * Time.deltaTime, rotY * Time.deltaTime, rotZ * Time.deltaTime, Space.Self);
	}
}
