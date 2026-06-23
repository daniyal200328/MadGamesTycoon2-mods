using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_SobelEdge : PostProcessBase
{
	public float Threshold = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/SobelEdge");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Threshold", Threshold);
	}
}
