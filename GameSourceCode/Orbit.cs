using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Orbit : MonoBehaviour
{
	public float orbitSpeed = -45f;

	public float rotateSpeed = 200f;

	public int orbitLineResolution = 150;

	public Material lineMaterial;

	private void Start()
	{
		VectorLine vectorLine = new VectorLine("OrbitLine", new List<Vector3>(orbitLineResolution), 2f, LineType.Continuous);
		vectorLine.material = lineMaterial;
		vectorLine.MakeCircle(Vector3.zero, Vector3.up, Vector3.Distance(base.transform.position, Vector3.zero));
		vectorLine.Draw3DAuto();
	}

	private void Update()
	{
		base.transform.RotateAround(Vector3.zero, Vector3.up, orbitSpeed * Time.deltaTime);
		base.transform.Rotate(Vector3.up * rotateSpeed * Time.deltaTime);
	}
}
