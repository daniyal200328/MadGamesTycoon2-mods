using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_DreamBlur : PostProcessBase
{
	[Range(0f, 1f)]
	public float Desaturation = 1f;

	public float Strength = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/DreamBlur");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Desaturation", Desaturation);
		mat.SetFloat("_Strength", Strength);
	}
}
