using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plexus : MonoBehaviour
{
	public ComputeShader plexus;

	public int amountOfPoints = 100;

	public int PPPS = 2;

	public float lineWidth = 0.02f;

	public Material lineMaterial;

	public Vector3 box = new Vector3(4f, 4f, 4f);

	public float particleSpeed = 1f;

	public float maxConnDistance = 3f;

	private float maxConnDistanceSqr;

	private Vector3[] defaultPositions;

	private Vector3[] velocities;

	private Vector3[] positions;

	private Mesh lineMesh;

	private Vector3 normal;

	private Vector3 side;

	private Vector3 p1;

	private Vector3 p2;

	private int startingVerticesIndex;

	private List<int> lineTrigs = new List<int>();

	private List<Vector3> lineVerts = new List<Vector3>();

	private Vector3[] verts = new Vector3[4];

	private int[] trigs = new int[6];

	[HideInInspector]
	public bool isEnabled;

	private List<KeyValuePair<int, int>> connected = new List<KeyValuePair<int, int>>();

	private HashSet<KeyValuePair<int, int>> connectedHashSet = new HashSet<KeyValuePair<int, int>>();

	private void Start()
	{
		lineMaterial.SetVector("_BoxDims", new Vector4(box.x, box.y, box.z, 1f));
		positions = new Vector3[amountOfPoints];
		defaultPositions = new Vector3[amountOfPoints];
		for (int i = 0; i < amountOfPoints; i++)
		{
			positions[i] = new Vector3(Random.Range(0f - box.x, box.x), Random.Range(0f - box.y, box.y), Random.Range(0f - box.z, box.z));
			defaultPositions[i] = positions[i];
		}
		lineMesh = new Mesh();
		int[] triangles = new int[6] { 0, 1, 2, 3, 2, 1 };
		lineMesh.vertices = verts;
		lineMesh.triangles = triangles;
		velocities = new Vector3[amountOfPoints];
		StartCoroutine(ConnectDots());
	}

	private void Update()
	{
		MovePoints();
		RenderLines();
	}

	private void MovePoints()
	{
		int kernelIndex = plexus.FindKernel("MoveParticels");
		ComputeBuffer computeBuffer = new ComputeBuffer(positions.Length, 12);
		computeBuffer.SetData(positions);
		plexus.SetBuffer(kernelIndex, "positions", computeBuffer);
		ComputeBuffer computeBuffer2 = new ComputeBuffer(defaultPositions.Length, 12);
		computeBuffer2.SetData(defaultPositions);
		plexus.SetBuffer(kernelIndex, "defaultPositions", computeBuffer2);
		ComputeBuffer computeBuffer3 = new ComputeBuffer(velocities.Length, 12);
		computeBuffer3.SetData(velocities);
		plexus.SetBuffer(kernelIndex, "velocities", computeBuffer3);
		plexus.SetFloat("deltaTime", Time.deltaTime);
		plexus.SetFloat("elapsedTime", Time.time);
		plexus.SetFloat("particleSpeed", particleSpeed);
		plexus.Dispatch(kernelIndex, positions.Length, 1, 1);
		computeBuffer.GetData(positions);
		computeBuffer.Release();
		computeBuffer2.Release();
		computeBuffer3.Release();
	}

	private static float DistanceSqr(Vector3 p1, Vector3 p2)
	{
		return (p1.x - p2.x) * (p1.x - p2.x) + (p1.y - p2.y) * (p1.y - p2.y) + (p1.z - p2.z) * (p1.z - p2.z);
	}

	private void RenderLines()
	{
		lineMesh = new Mesh();
		for (int i = 0; i < connected.Count; i++)
		{
			p1 = positions[connected[i].Key];
			p2 = positions[connected[i].Value];
			normal = Vector3.Cross(p1, p2);
			side = Vector3.Cross(normal, p2 - p1);
			side.Normalize();
			startingVerticesIndex = lineVerts.Count;
			verts[0] = p1 + side * (lineWidth / 2f);
			verts[1] = p1 + side * (lineWidth / -2f);
			verts[2] = p2 + side * (lineWidth / 2f);
			verts[3] = p2 + side * (lineWidth / -2f);
			trigs[0] = startingVerticesIndex;
			trigs[1] = (trigs[5] = startingVerticesIndex + 1);
			trigs[2] = (trigs[4] = startingVerticesIndex + 2);
			trigs[3] = startingVerticesIndex + 3;
			lineVerts.AddRange(verts);
			lineTrigs.AddRange(trigs);
		}
		lineMesh.vertices = lineVerts.ToArray();
		lineMesh.triangles = lineTrigs.ToArray();
		lineMesh.RecalculateBounds();
		Graphics.DrawMesh(lineMesh, base.transform.localToWorldMatrix, lineMaterial, 0);
		lineTrigs.Clear();
		lineVerts.Clear();
	}

	private IEnumerator ConnectDots()
	{
		WaitForEndOfFrame wfeof = new WaitForEndOfFrame();
		int indx = 0;
		maxConnDistanceSqr = maxConnDistance * maxConnDistance;
		do
		{
			yield return wfeof;
			for (int i = 0; i < PPPS; i++)
			{
				Vector3 vector = positions[indx];
				connected.RemoveAll((KeyValuePair<int, int> x) => x.Key == indx || x.Value == indx);
				connectedHashSet.RemoveWhere((KeyValuePair<int, int> x) => x.Key == indx || x.Value == indx);
				for (int num = 0; num < amountOfPoints; num++)
				{
					if (num != indx && DistanceSqr(vector, positions[num]) < maxConnDistanceSqr)
					{
						KeyValuePair<int, int> item = new KeyValuePair<int, int>(indx, num);
						if (connectedHashSet.Add(item))
						{
							connected.Add(new KeyValuePair<int, int>(indx, num));
						}
					}
				}
				int num2 = indx + 1;
				indx = num2;
				if (indx >= amountOfPoints)
				{
					indx = 0;
				}
			}
		}
		while (!isEnabled);
	}
}
