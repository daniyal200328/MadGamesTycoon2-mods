using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Displacement : PostProcessBase
{
	public Texture2D BumpTexture;

	[Range(-1f, 1f)]
	public float Amount = 0.5f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Displacement");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		if (BumpTexture != null)
		{
			mat.SetTexture("_BumpTex", BumpTexture);
		}
		mat.SetFloat("_Amount", Amount);
	}
}
