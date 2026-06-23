using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_FakeHDR : PostProcessBase
{
	[Range(0f, 1f)]
	public float Amount = 0.5f;

	[Range(0f, 1f)]
	public float Multiplier = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/FakeHDR");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Amount", Amount);
		mat.SetFloat("_Multiplier", Multiplier);
	}
}
