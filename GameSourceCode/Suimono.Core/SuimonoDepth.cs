using UnityEngine;

namespace Suimono.Core;

public class SuimonoDepth : MonoBehaviour
{
	public Shader useShader;

	private Material useMat;

	private void Start()
	{
		useMat = new Material(useShader);
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (useMat != null)
		{
			Graphics.Blit(source, destination, useMat);
		}
	}
}
