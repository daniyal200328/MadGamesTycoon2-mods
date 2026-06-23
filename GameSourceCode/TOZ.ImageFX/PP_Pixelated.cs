using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Pixelated : PostProcessBase
{
	public int PixelWidth = 16;

	public int PixelHeight = 16;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Pixelated");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_PixWidth", PixelWidth);
		mat.SetFloat("_PixHeight", PixelHeight);
	}
}
