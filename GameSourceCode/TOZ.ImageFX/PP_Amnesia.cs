using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Amnesia : PostProcessBase
{
	[Range(0f, 1f)]
	public float Visibility = 1f;

	public float Speed = 3f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Amnesia");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Visibility", Visibility);
		mat.SetFloat("_Speed", Speed);
	}
}
