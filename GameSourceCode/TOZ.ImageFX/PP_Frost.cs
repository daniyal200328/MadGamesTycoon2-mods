using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Frost : PostProcessBase
{
	public Texture2D NoiseTex;

	public float Amount = 1f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Frost");
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
		mat.SetFloat("_Amount", Amount);
	}
}
