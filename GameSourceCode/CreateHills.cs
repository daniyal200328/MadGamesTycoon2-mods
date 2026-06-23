using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class CreateHills : MonoBehaviour
{
	public Texture hillTexture;

	public PhysicsMaterial2D hillPhysicsMaterial;

	public int numberOfPoints = 100;

	public int numberOfHills = 4;

	public GameObject ball;

	private Vector3 storedPosition;

	private VectorLine hills;

	private Vector2[] splinePoints;

	private void Start()
	{
		storedPosition = ball.transform.position;
		splinePoints = new Vector2[numberOfHills * 2 + 1];
		hills = new VectorLine("Hills", new List<Vector2>(numberOfPoints), hillTexture, 12f, LineType.Continuous, Joins.Weld);
		hills.useViewportCoords = true;
		hills.collider = true;
		hills.physicsMaterial = hillPhysicsMaterial;
		Random.InitState(95);
		CreateHillLine();
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 150f, 40f), "Make new hills"))
		{
			CreateHillLine();
			ball.transform.position = storedPosition;
			ball.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			ball.GetComponent<Rigidbody2D>().WakeUp();
		}
	}

	private void CreateHillLine()
	{
		splinePoints[0] = new Vector2(-0.02f, Random.Range(0.1f, 0.6f));
		float num = 0f;
		float num2 = 1f / (float)(numberOfHills * 2);
		int i;
		for (i = 1; i < splinePoints.Length; i += 2)
		{
			num += num2;
			splinePoints[i] = new Vector2(num, Random.Range(0.3f, 0.7f));
			num += num2;
			splinePoints[i + 1] = new Vector2(num, Random.Range(0.1f, 0.6f));
		}
		splinePoints[i - 1] = new Vector2(1.02f, Random.Range(0.1f, 0.6f));
		hills.MakeSpline(splinePoints);
		hills.Draw();
	}
}
