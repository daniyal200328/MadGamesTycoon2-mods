using UnityEngine;

[ExecuteInEditMode]
public class PostProcessing_Sobel : MonoBehaviour
{
	private Material sobelMat;

	[Range(0f, 3f)]
	public float SobelResolution = 1f;

	public Color outlineColor;

	private void Start()
	{
		Camera.main.depthTextureMode = DepthTextureMode.Depth;
		sobelMat = new Material(Shader.Find("Nasty-Screen/SobelOutline"));
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		sobelMat.SetFloat("_ResX", (float)Screen.width * SobelResolution);
		sobelMat.SetFloat("_ResY", (float)Screen.height * SobelResolution);
		sobelMat.SetColor("_Outline", outlineColor);
		Graphics.Blit(source, destination, sobelMat);
	}
}
