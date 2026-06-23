using System.Collections.Generic;
using UnityEngine;
using Vectrosity;

public class Simple3D2 : MonoBehaviour
{
	public TextAsset vectorCube;

	private void Start()
	{
		List<Vector3> points = VectorLine.BytesToVector3List(vectorCube.bytes);
		VectorLine line = new VectorLine(base.gameObject.name, points, 2f);
		VectorManager.ObjectSetup(base.gameObject, line, Visibility.Dynamic, Brightness.None);
	}
}
