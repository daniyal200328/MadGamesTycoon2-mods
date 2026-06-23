using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_LineArt : PostProcessBase
{
	public Color Color = Color.black;

	public float Strength = 80f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/LineArt");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		mat.SetVector("_Color", Color);
		mat.SetFloat("_Strength", Strength);
	}
}
