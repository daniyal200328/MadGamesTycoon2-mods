using UnityEngine;

public class CameraZoom : MonoBehaviour
{
	public float zoomSpeed = 10f;

	public float keyZoomSpeed = 20f;

	private void Update()
	{
		base.transform.Translate(Vector3.forward * zoomSpeed * Input.GetAxis("Mouse ScrollWheel"));
		base.transform.Translate(Vector3.forward * keyZoomSpeed * Time.deltaTime * Input.GetAxis("Vertical"));
	}
}
