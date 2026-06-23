using UnityEngine;

public class RotateAroundY : MonoBehaviour
{
	public float rotateSpeed = 10f;

	private void Update()
	{
		base.transform.Rotate(Vector3.up * Time.deltaTime * rotateSpeed);
	}
}
