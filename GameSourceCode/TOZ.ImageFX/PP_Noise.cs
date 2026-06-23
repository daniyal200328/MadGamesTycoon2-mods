using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Noise : PostProcessBase
{
	[Range(0f, 2f)]
	public float Scale = 0.5f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Noise");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Scale", Scale);
	}
}
