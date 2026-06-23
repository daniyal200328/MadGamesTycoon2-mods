using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_LightWave : PostProcessBase
{
	public float Red = 4f;

	public float Green = 4f;

	public float Blue = 4f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/LightWave");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Red", Red);
		mat.SetFloat("_Green", Green);
		mat.SetFloat("_Blue", Blue);
	}
}
