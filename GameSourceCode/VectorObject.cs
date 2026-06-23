using UnityEngine;
using Vectrosity;

public class VectorObject : MonoBehaviour
{
	public enum Shape
	{
		Cube,
		Sphere
	}

	public Shape shape;

	private void Start()
	{
		VectorLine vectorLine = new VectorLine("Shape", XrayLineData.use.shapePoints[(int)shape], XrayLineData.use.lineTexture, XrayLineData.use.lineWidth);
		vectorLine.color = Color.green;
		VectorManager.ObjectSetup(base.gameObject, vectorLine, Visibility.Always, Brightness.None);
	}
}
