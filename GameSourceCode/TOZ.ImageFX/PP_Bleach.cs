using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Bleach : PostProcessBase
{
	[Range(0f, 1f)]
	public float Opacity = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Bleach");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Opacity", Opacity);
	}
}
