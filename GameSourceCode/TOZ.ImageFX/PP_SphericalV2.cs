using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_SphericalV2 : PostProcessBase
{
	public float Radius = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/SphericalV2");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetFloat("_Radius", Radius);
	}
}
