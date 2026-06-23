using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_BlurH : PostProcessBase
{
	public float Strength = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/BlurH");
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
