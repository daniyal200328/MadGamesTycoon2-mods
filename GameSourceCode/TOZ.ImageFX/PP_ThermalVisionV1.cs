using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_ThermalVisionV1 : PostProcessBase
{
	public Color Color1 = Color.blue;

	public Color Color2 = Color.yellow;

	public Color Color3 = Color.red;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/ThermalVisionV1");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetVector("_Color1", Color1);
		mat.SetVector("_Color2", Color2);
		mat.SetVector("_Color3", Color3);
	}
}
