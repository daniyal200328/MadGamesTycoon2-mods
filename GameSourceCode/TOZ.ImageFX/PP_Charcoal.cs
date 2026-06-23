using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Charcoal : PostProcessBase
{
	[Range(0f, 1f)]
	public float Strength = 1f;

	public Color LineColor = Color.black;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Charcoal");
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
