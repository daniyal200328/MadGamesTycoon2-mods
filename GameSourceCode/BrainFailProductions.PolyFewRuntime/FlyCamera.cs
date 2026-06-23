using UnityEngine;

namespace BrainFailProductions.PolyFewRuntime;

public class FlyCamera : MonoBehaviour
{
	public Transform target;

	public float distance = 5f;

	public float xSpeed = 120f;

	public float ySpeed = 120f;

	public float panSpeed = 0.05f;

	public float yMinLimit = -20f;

	public float yMaxLimit = 80f;

	public float distanceMin = 0.5f;

	public float distanceMax = 15f;

	private Rigidbody rigidbody;

	private float x;

	private float y;

	public static bool deactivated;

	private void Start()
	{
		Vector3 eulerAngles = base.transform.eulerAngles;
		x = eulerAngles.y;
		y = eulerAngles.x;
		rigidbody = GetComponent<Rigidbody>();
		if (rigidbody != null)
		{
			rigidbody.freezeRotation = true;
		}
	}

	private void Update()
	{
		if (!deactivated && (bool)target)
		{
			float num = Input.GetAxis("Mouse X");
			float num2 = Input.GetAxis("Mouse Y");
			if (!Input.GetMouseButton(0))
			{
				num = 0f;
				num2 = 0f;
			}
			x += num * xSpeed * distance * 0.02f;
			y -= num2 * ySpeed * 0.02f;
			y = ClampAngle(y, yMinLimit, yMaxLimit);
			Quaternion quaternion = Quaternion.Euler(y, x, 0f);
			distance = Mathf.Clamp(distance - Input.GetAxis("Mouse ScrollWheel") * 5f, distanceMin, distanceMax);
			if (Physics.Linecast(target.position, base.transform.position, out var hitInfo))
			{
				distance -= hitInfo.distance;
			}
			Vector3 vector = new Vector3(0f, 0f, 0f - distance);
			Vector3 position = quaternion * vector + target.position;
			base.transform.position = position;
			base.transform.rotation = quaternion;
		}
	}

	public static float ClampAngle(float angle, float min, float max)
	{
		if (angle < -360f)
		{
			angle += 360f;
		}
		if (angle > 360f)
		{
			angle -= 360f;
		}
		return Mathf.Clamp(angle, min, max);
	}
}
