using UnityEngine;

namespace ReachableGames.PostLinerPro;

public class FlyCamera : MonoBehaviour
{
	public float Speed = 1f;

	public float MouseSensitivity = 1f;

	public bool InvertMouse;

	private void Start()
	{
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	private void Update()
	{
		if (Cursor.visible)
		{
			if (Input.GetKeyDown(KeyCode.BackQuote))
			{
				Cursor.visible = false;
				Cursor.lockState = CursorLockMode.Locked;
			}
			return;
		}
		Vector3 zero = Vector3.zero;
		if (Input.GetKey(KeyCode.W))
		{
			zero.z += 1f;
		}
		if (Input.GetKey(KeyCode.S))
		{
			zero.z += -1f;
		}
		if (Input.GetKey(KeyCode.A))
		{
			zero.x += -1f;
		}
		if (Input.GetKey(KeyCode.D))
		{
			zero.x += 1f;
		}
		base.transform.localPosition += base.transform.TransformVector(zero * Speed * Time.deltaTime);
		float num = Input.GetAxis("Mouse Y") * MouseSensitivity * (InvertMouse ? (-1f) : 1f);
		float num2 = Input.GetAxis("Mouse X") * MouseSensitivity;
		Vector3 euler = new Vector3(Mathf.LerpAngle(base.transform.localEulerAngles.x, base.transform.localEulerAngles.x + num, 1f), Mathf.LerpAngle(base.transform.localEulerAngles.y, base.transform.localEulerAngles.y + num2, 1f), 0f);
		base.transform.localRotation = Quaternion.Euler(euler);
		if (Input.GetKeyDown(KeyCode.BackQuote))
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}
}
