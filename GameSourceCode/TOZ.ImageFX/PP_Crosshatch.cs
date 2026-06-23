using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Crosshatch : PostProcessBase
{
	[Range(1E-05f, 0.1f)]
	public float Strength = 0.01f;

	public Color LineColor = Color.white;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Crosshatch");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetVector("_LineColor", LineColor);
		mat.SetFloat("_Strength", Strength);
	}
}
