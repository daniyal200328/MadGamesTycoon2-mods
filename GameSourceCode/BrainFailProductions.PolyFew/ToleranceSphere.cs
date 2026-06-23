using System;
using UnityEngine;

namespace BrainFailProductions.PolyFew;

[Serializable]
public class ToleranceSphere : ScriptableObject
{
	public Vector3 worldPosition;

	public float diameter;

	public Color color;

	public float preservationStrength;

	public bool isHidden;

	public ToleranceSphere(Vector3 worldPosition, float diameter, Color color, float preservationStrength, bool isHidden = false)
	{
		this.worldPosition = worldPosition;
		this.diameter = diameter;
		this.color = color;
		this.preservationStrength = preservationStrength;
		this.isHidden = isHidden;
	}

	public void SetProperties(ToleranceSphereJson tSphereJson)
	{
		worldPosition = tSphereJson.worldPosition;
		diameter = tSphereJson.diameter;
		color = tSphereJson.color;
		preservationStrength = tSphereJson.preservationStrength;
		isHidden = tSphereJson.isHidden;
	}

	public void SetProperties(Vector3 worldPosition, float diameter, Color color, float preservationStrength, bool isHidden = false)
	{
		this.worldPosition = worldPosition;
		this.diameter = diameter;
		this.color = color;
		this.preservationStrength = preservationStrength;
		this.isHidden = isHidden;
	}
}
