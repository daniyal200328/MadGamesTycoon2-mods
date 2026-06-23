using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Posterize : PostProcessBase
{
	public int Colors = 4;

	[Range(0f, 2.2f)]
	public float Gamma = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Posterize");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetInt("_Colors", Colors);
		mat.SetFloat("_Gamma", Gamma);
	}
}
