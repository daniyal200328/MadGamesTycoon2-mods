using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_LensCircle : PostProcessBase
{
	[Range(0f, 1f)]
	public float CenterX = 0.5f;

	[Range(0f, 1f)]
	public float CenterY = 0.5f;

	[Range(0f, 1f)]
	public float RadiusX = 1f;

	[Range(0f, 1f)]
	public float RadiusY;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/LensCircle");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_CenterX", CenterX);
		mat.SetFloat("_CenterY", CenterY);
		mat.SetFloat("_RadiusX", RadiusX);
		mat.SetFloat("_RadiusY", RadiusY);
	}
}
