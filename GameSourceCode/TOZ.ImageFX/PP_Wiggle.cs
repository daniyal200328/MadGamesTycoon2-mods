using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Wiggle : PostProcessBase
{
	public float Speed = 10f;

	public float Amplitude = 0.05f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Wiggle");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Speed", Speed);
		mat.SetFloat("_Amplitude", Amplitude);
	}
}
