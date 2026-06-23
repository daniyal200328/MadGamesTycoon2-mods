using System.Collections;
using UnityEngine;
using Vectrosity;

public class DrawBox : MonoBehaviour
{
	public float moveSpeed = 1f;

	public float explodePower = 20f;

	public Camera vectorCam;

	private bool mouseDown;

	private Rigidbody[] rigidbodies;

	private bool canClick = true;

	private bool boxDrawn;

	private IEnumerator Start()
	{
		GetComponent<Renderer>().enabled = false;
		rigidbodies = Object.FindObjectsOfType(typeof(Rigidbody)) as Rigidbody[];
		VectorLine.canvas.planeDistance = 0.5f;
		yield return null;
		VectorLine.SetCanvasCamera(vectorCam);
	}

	private void Update()
	{
		Vector3 mousePosition = Input.mousePosition;
		mousePosition.z = 1f;
		Vector3 position = Camera.main.ScreenToWorldPoint(mousePosition);
		if (Input.GetMouseButtonDown(0) && canClick)
		{
			GetComponent<Renderer>().enabled = true;
			base.transform.position = position;
			mouseDown = true;
		}
		if (mouseDown)
		{
			base.transform.localScale = new Vector3(position.x - base.transform.position.x, position.y - base.transform.position.y, 1f);
		}
		if (Input.GetMouseButtonUp(0))
		{
			mouseDown = false;
			boxDrawn = true;
		}
		base.transform.Translate(-Vector3.up * Time.deltaTime * moveSpeed * Input.GetAxis("Vertical"));
		base.transform.Translate(Vector3.right * Time.deltaTime * moveSpeed * Input.GetAxis("Horizontal"));
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(20f, 20f, 320f, 38f), "Draw a box by clicking and dragging with the mouse\nMove the drawn box with the arrow keys");
		Rect position = new Rect(20f, 62f, 60f, 30f);
		canClick = !position.Contains(Event.current.mousePosition);
		if (boxDrawn && GUI.Button(position, "Boom!"))
		{
			Rigidbody[] array = rigidbodies;
			for (int i = 0; i < array.Length; i++)
			{
				array[i].AddExplosionForce(explodePower, new Vector3(0f, -6.5f, -1.5f), 20f, 0f, ForceMode.VelocityChange);
			}
		}
	}
}
