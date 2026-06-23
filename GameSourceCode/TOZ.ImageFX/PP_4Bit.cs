using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_4Bit : PostProcessBase
{
	public int BitDepth = 2;

	[Range(0f, 1f)]
	public float Contrast = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/4Bit");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetInt("_BitDepth", BitDepth);
		mat.SetFloat("_Contrast", Contrast);
	}
}
