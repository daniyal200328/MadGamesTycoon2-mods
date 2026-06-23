using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_SimpleBloom : PostProcessBase
{
	[Range(0f, 2.2f)]
	public float Gamma = 2.2f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/SimpleBloom");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Gamma", Gamma);
	}
}
