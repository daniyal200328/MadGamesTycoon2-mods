using UnityEngine;

public class U10PS_MouseRotation : MonoBehaviour
{
	public float sensitivity;

	public bool lockZRot;

	public bool lockMouse;

	private float yValue;

	private float zValue;

	private void Start()
	{
		if (lockMouse)
		{
			Cursor.lockState = CursorLockMode.Locked;
			Cursor.visible = false;
		}
	}

	private void Update()
	{
		yValue = base.transform.eulerAngles.y + sensitivity * Input.GetAxis("Mouse X");
		zValue = base.transform.eulerAngles.z + sensitivity * Input.GetAxis("Mouse Y");
		base.transform.eulerAngles = new Vector3(0f, Mathf.Clamp((yValue > 180f) ? (yValue - 360f) : yValue, -50f, 50f), (!lockZRot) ? Mathf.Clamp((zValue > 180f) ? (zValue - 360f) : zValue, -20f, 20f) : 0f);
	}
}
