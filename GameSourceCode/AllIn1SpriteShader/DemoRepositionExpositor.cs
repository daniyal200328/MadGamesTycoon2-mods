using UnityEngine;

namespace AllIn1SpriteShader;

public class DemoRepositionExpositor : MonoBehaviour
{
	[SerializeField]
	private float paddingX = 10f;

	[ContextMenu("RepositionExpositor")]
	private void RepositionExpositor()
	{
		int num = 0;
		Vector3 zero = Vector3.zero;
		foreach (Transform item in base.transform)
		{
			zero.x = (float)num * paddingX;
			item.localPosition = zero;
			num++;
		}
	}
}
