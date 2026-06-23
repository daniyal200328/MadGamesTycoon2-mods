using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Grid3D : MonoBehaviour
{
	public int numberOfLines = 20;

	public float distanceBetweenLines = 2f;

	public float moveSpeed = 8f;

	public float rotateSpeed = 70f;

	public float lineWidth = 2f;

	private void Start()
	{
		numberOfLines = Mathf.Clamp(numberOfLines, 2, 8190);
		List<Vector3> list = new List<Vector3>();
		for (int i = 0; i < numberOfLines; i++)
		{
			list.Add(new Vector3((float)i * distanceBetweenLines, 0f, 0f));
			list.Add(new Vector3((float)i * distanceBetweenLines, 0f, (float)(numberOfLines - 1) * distanceBetweenLines));
		}
		for (int j = 0; j < numberOfLines; j++)
		{
			list.Add(new Vector3(0f, 0f, (float)j * distanceBetweenLines));
			list.Add(new Vector3((float)(numberOfLines - 1) * distanceBetweenLines, 0f, (float)j * distanceBetweenLines));
		}
		new VectorLine("Grid", list, lineWidth).Draw3DAuto();
		Vector3 position = base.transform.position;
		position.x = (float)(numberOfLines - 1) * distanceBetweenLines / 2f;
		base.transform.position = position;
	}

	private void Update()
	{
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			base.transform.Rotate(Vector3.up * Input.GetAxis("Horizontal") * Time.deltaTime * rotateSpeed);
			base.transform.Translate(Vector3.up * Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed);
		}
		else
		{
			base.transform.Translate(new Vector3(Input.GetAxis("Horizontal") * Time.deltaTime * moveSpeed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * moveSpeed));
		}
	}

	private void OnGUI()
	{
		GUILayout.Label(" Use arrow keys to move camera. Hold Shift + arrow up/down to move vertically. Hold Shift + arrow left/right to rotate.");
	}
}
