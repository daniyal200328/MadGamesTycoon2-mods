using UnityEngine;

public class RotateViewpoint : MonoBehaviour
{
	public float rotateSpeed = 5f;

	private void Update()
	{
		base.transform.RotateAround(Vector3.zero, Vector3.right, rotateSpeed * Time.deltaTime);
	}
}
