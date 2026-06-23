using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_BlurV : PostProcessBase
{
	public float Strength = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/BlurV");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Strength", Strength);
	}
}
