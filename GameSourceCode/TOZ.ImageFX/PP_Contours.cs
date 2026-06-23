using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Contours : PostProcessBase
{
	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Contours");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
