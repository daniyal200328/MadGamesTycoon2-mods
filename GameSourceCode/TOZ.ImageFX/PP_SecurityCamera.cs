using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_SecurityCamera : PostProcessBase
{
	public float Speed = 2f;

	[Range(0f, 1f)]
	public float Thickness = 0.25f;

	[Range(0f, 1f)]
	public float Luminance = 0.25f;

	[Range(0f, 1f)]
	public float Darkness = 0.75f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/SecurityCamera");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Speed", Speed);
		mat.SetFloat("_Thickness", Thickness);
		mat.SetFloat("_Luminance", Luminance);
		mat.SetFloat("_Darkness", Darkness);
	}
}
