using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Vignette : PostProcessBase
{
	public float Radius = 10f;

	public float Darkness = 0.5f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Vignette");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Radius", Radius);
		mat.SetFloat("_Darkness", Darkness);
	}
}
