using UnityEngine;

namespace UntitledTools.VertexWind;

public class WindEffectorRadius : MonoBehaviour
{
	[Tooltip("The radius of the effect multiplier")]
	public float radius = 10f;

	[Tooltip("The \"amount\" override used in the wind effector")]
	public Vector3 amount = Vector3.one * 0.5f;

	private void OnDrawGizmosSelected()
	{
	}

	private void OnDrawGizmos()
	{
	}
}
