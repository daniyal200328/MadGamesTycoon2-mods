using System;
using UnityEngine;

namespace MeshDistortLite;

[Serializable]
public class DistortData
{
	public bool enabled = true;

	public string name = "Effect";

	public float animationSpeed = 1f;

	public Distort.Type type;

	public float force = 1f;

	public float movementDisplacement;

	public Vector3 tile = Vector3.one;

	public AnimationCurve displacedForceX = new AnimationCurve();

	public AnimationCurve displacedForceY = new AnimationCurve();

	public AnimationCurve displacedForceZ = new AnimationCurve();

	public AnimationCurve displacedForceXY = new AnimationCurve();

	public AnimationCurve displacedForceXZ = new AnimationCurve();

	public AnimationCurve displacedForceYX = new AnimationCurve();

	public AnimationCurve displacedForceYZ = new AnimationCurve();

	public AnimationCurve displacedForceZX = new AnimationCurve();

	public AnimationCurve displacedForceZY = new AnimationCurve();

	public AnimationCurve staticForceX = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	public AnimationCurve staticForceY = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	public AnimationCurve staticForceZ = new AnimationCurve(new Keyframe(0f, 1f), new Keyframe(1f, 1f));

	public bool isPingPong = true;

	public bool showInEditor = true;

	public bool calculateInWorldSpace;

	private float x;

	private float y;

	private float z;

	private Vector3 bMin;

	private Vector3 bNormalized;

	private Vector3 percentage;

	private float multiplier;

	private Vector3 dir;

	public void SetBounds(Bounds bounds)
	{
		bMin = bounds.min;
		bNormalized = bounds.max - bounds.min;
		if (bNormalized.x == 0f)
		{
			bNormalized.x = 0.1f;
		}
		if (bNormalized.y == 0f)
		{
			bNormalized.y = 0.1f;
		}
		if (bNormalized.z == 0f)
		{
			bNormalized.z = 0.1f;
		}
	}

	public void DistortVertice(ref Vector3 vertice)
	{
		x = 0f;
		y = 0f;
		z = 0f;
		percentage = vertice;
		if (calculateInWorldSpace)
		{
			multiplier = staticForceX.Evaluate((percentage.x - bMin.x) / bNormalized.x) * staticForceY.Evaluate((percentage.y - bMin.y) / bNormalized.y) * staticForceZ.Evaluate((percentage.z - bMin.z) / bNormalized.z);
			percentage.x /= bNormalized.x;
			percentage.y /= bNormalized.y;
			percentage.z /= bNormalized.z;
		}
		else
		{
			percentage.x -= bMin.x;
			percentage.y -= bMin.y;
			percentage.z -= bMin.z;
			percentage.x /= bNormalized.x;
			percentage.y /= bNormalized.y;
			percentage.z /= bNormalized.z;
			multiplier = staticForceX.Evaluate(percentage.x) * staticForceY.Evaluate(percentage.y) * staticForceZ.Evaluate(percentage.z);
		}
		if (isPingPong)
		{
			percentage.x = Math.PingPong((percentage.x + movementDisplacement) * tile.x, 0f, 1f);
			percentage.y = Math.PingPong((percentage.y + movementDisplacement) * tile.y, 0f, 1f);
			percentage.z = Math.PingPong((percentage.z + movementDisplacement) * tile.z, 0f, 1f);
		}
		else
		{
			percentage.x = (percentage.x + movementDisplacement) * tile.x;
			percentage.y = (percentage.y + movementDisplacement) * tile.y;
			percentage.z = (percentage.z + movementDisplacement) * tile.z;
		}
		x += displacedForceXY.Evaluate(percentage.y) * force;
		x += displacedForceXZ.Evaluate(percentage.z) * force;
		y += displacedForceYX.Evaluate(percentage.x) * force;
		y += displacedForceYZ.Evaluate(percentage.z) * force;
		z += displacedForceZX.Evaluate(percentage.x) * force;
		z += displacedForceZY.Evaluate(percentage.y) * force;
		vertice.x += x * multiplier;
		vertice.y += y * multiplier;
		vertice.z += z * multiplier;
	}
}
