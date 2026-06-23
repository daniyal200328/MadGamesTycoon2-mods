using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class DrawPath : MonoBehaviour
{
	public Texture lineTex;

	public Color lineColor = Color.green;

	public int maxPoints = 500;

	public bool continuousUpdate = true;

	public GameObject ballPrefab;

	public float force = 16f;

	private VectorLine pathLine;

	private int pathIndex;

	private GameObject ball;

	private void Start()
	{
		pathLine = new VectorLine("Path", new List<Vector3>(), lineTex, 12f, LineType.Continuous);
		pathLine.color = Color.green;
		pathLine.textureScale = 1f;
		MakeBall();
		StartCoroutine(SamplePoints(ball.transform));
	}

	private void MakeBall()
	{
		if ((bool)ball)
		{
			Object.Destroy(ball);
		}
		ball = Object.Instantiate(ballPrefab, new Vector3(-2.25f, -4.4f, -1.9f), Quaternion.Euler(300f, 70f, 310f));
		ball.GetComponent<Rigidbody>().useGravity = true;
		ball.GetComponent<Rigidbody>().AddForce(ball.transform.forward * force, ForceMode.Impulse);
	}

	private IEnumerator SamplePoints(Transform thisTransform)
	{
		bool running = true;
		while (running)
		{
			pathLine.points3.Add(thisTransform.position);
			if (++pathIndex == maxPoints)
			{
				running = false;
			}
			yield return new WaitForSeconds(0.05f);
			if (continuousUpdate)
			{
				pathLine.Draw();
			}
		}
	}

	private void OnGUI()
	{
		if (GUI.Button(new Rect(10f, 10f, 100f, 30f), "Reset"))
		{
			Reset();
		}
		if (!continuousUpdate && GUI.Button(new Rect(10f, 45f, 100f, 30f), "Draw Path"))
		{
			pathLine.Draw();
		}
	}

	private void Reset()
	{
		StopAllCoroutines();
		MakeBall();
		pathLine.points3.Clear();
		pathLine.Draw();
		pathIndex = 0;
		StartCoroutine(SamplePoints(ball.transform));
	}
}
