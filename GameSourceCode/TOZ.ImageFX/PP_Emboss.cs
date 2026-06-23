using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Emboss : PostProcessBase
{
	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Emboss");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
