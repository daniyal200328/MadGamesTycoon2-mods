using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Desaturate : PostProcessBase
{
	[Range(0f, 1f)]
	public float Amount = 0.5f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Desaturate");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Amount", Amount);
	}
}
