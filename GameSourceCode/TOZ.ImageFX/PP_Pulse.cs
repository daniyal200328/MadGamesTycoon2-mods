using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Pulse : PostProcessBase
{
	[Range(0f, 1f)]
	public float DirectionX = 0.5f;

	[Range(0f, 1f)]
	public float DirectionY = 0.5f;

	public float Speed = 5f;

	[Range(-1f, 1f)]
	public float Amplitude = 0.1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Pulse");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_DirectionX", DirectionX);
		mat.SetFloat("_DirectionY", DirectionY);
		mat.SetFloat("_Speed", Speed);
		mat.SetFloat("_Amplitude", Amplitude);
	}
}
