using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_RadialBlur : PostProcessBase
{
	[Range(0f, 1f)]
	public float CenterX = 0.5f;

	[Range(0f, 1f)]
	public float CenterY = 0.5f;

	public float Strength = 0.2f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/RadialBlur");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_CenterX", CenterX);
		mat.SetFloat("_CenterY", CenterY);
		mat.SetFloat("_Strength", Strength);
	}
}
