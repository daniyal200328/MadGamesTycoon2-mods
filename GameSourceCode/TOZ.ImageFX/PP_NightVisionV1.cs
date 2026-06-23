using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_NightVisionV1 : PostProcessBase
{
	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/NightVisionV1");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		Graphics.Blit(src, dest, mat);
	}
}
