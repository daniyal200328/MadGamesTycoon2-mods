using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_NightVisionV2 : PostProcessBase
{
	public Texture2D NoiseTex;

	public Color VisionColor = Color.green;

	public Color FadeColor = Color.black;

	public float NoiseAmount = 1f;

	[Range(0f, 1f)]
	public float Radius = 0.5f;

	[Range(0f, 1f)]
	public float Fade = 0.2f;

	[Range(0f, 1f)]
	public float Intensity = 0.5f;

	[Range(0f, 2.2f)]
	public float Gamma = 2.2f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/NightVisionV2");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		if (NoiseTex != null)
		{
			mat.SetTexture("_NoiseTex", NoiseTex);
		}
		mat.SetVector("_VisionColor", VisionColor);
		mat.SetVector("_FadeColor", FadeColor);
		mat.SetFloat("_NoiseAmount", NoiseAmount);
		mat.SetFloat("_Radius", Radius);
		mat.SetFloat("_Fade", Fade);
		mat.SetFloat("_Intensity", Intensity);
		mat.SetFloat("_Gamma", Gamma);
	}
}
