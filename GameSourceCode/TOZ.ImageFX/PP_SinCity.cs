using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_SinCity : PostProcessBase
{
	public Color SelectedColor = Color.red;

	public Color ReplacementColor = Color.white;

	[Range(0f, 1f)]
	public float Brightness = 1f;

	[Range(0f, 1f)]
	public float Tolerance = 0.5f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/SinCity");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetColor("_SelectedColor", SelectedColor);
		mat.SetColor("_ReplacedColor", ReplacementColor);
		mat.SetFloat("_Brightness", Brightness);
		mat.SetFloat("_Tolerance", Tolerance);
	}
}
