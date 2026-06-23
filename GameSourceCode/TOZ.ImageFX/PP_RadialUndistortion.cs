using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_RadialUndistortion : PostProcessBase
{
	public float CenterX = 320f;

	public float CenterY = 240f;

	public float F = 0.9f;

	public float K1 = -0.27f;

	public float K2 = 0.08f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/RadialUndistortion");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_F", F);
		mat.SetFloat("_OX", CenterX);
		mat.SetFloat("_OY", CenterY);
		mat.SetFloat("_K1", K1);
		mat.SetFloat("_K2", K2);
	}
}
