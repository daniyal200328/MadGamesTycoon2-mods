using UnityEngine;

namespace TOZ.ImageFX;

[RequireComponent(typeof(Camera))]
public abstract class PostProcessBase : MonoBehaviour
{
	protected Shader shd;

	protected Material mat;

	private void OnEnable()
	{
		if (!SystemInfo.supportsImageEffects || shd == null || !shd.isSupported)
		{
			base.enabled = false;
		}
		else if (mat == null)
		{
			mat = new Material(shd);
			mat.hideFlags = HideFlags.HideAndDontSave;
		}
	}

	private void OnDisable()
	{
		if (mat != null)
		{
			Object.DestroyImmediate(mat);
		}
	}
}
