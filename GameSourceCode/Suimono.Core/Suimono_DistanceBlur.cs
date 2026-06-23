using UnityEngine;

namespace Suimono.Core;

public class Suimono_DistanceBlur : MonoBehaviour
{
	public float blurAmt;

	public int iterations = 3;

	public float blurSpread = 0.6f;

	public Shader blurShader;

	public Material material;

	private float offc;

	private float off;

	private int rtW;

	private int rtH;

	private int i;

	private RenderTexture buffer;

	private RenderTexture buffer2;

	[Range(0f, 2f)]
	public int downsample = 1;

	[Range(0f, 10f)]
	public float blurSize = 3f;

	private void Start()
	{
		CreateMaterial();
	}

	private void CreateMaterial()
	{
		if (material == null)
		{
			material = new Material(blurShader);
			material.hideFlags = HideFlags.DontSave;
		}
	}

	public void OnDisable()
	{
		if ((bool)material)
		{
			Object.DestroyImmediate(material);
		}
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		if (material == null)
		{
			CreateMaterial();
		}
		iterations = Mathf.FloorToInt(Mathf.Lerp(0f, 2f, blurAmt));
		downsample = Mathf.FloorToInt(Mathf.Lerp(0f, 2f, blurAmt));
		blurSpread = Mathf.Lerp(0f, 2f, blurAmt);
		float num = 1f / (1f * (float)(1 << downsample));
		material.SetVector("_Parameter", new Vector4(blurSpread * num, (0f - blurSpread) * num, 0f, 0f));
		source.filterMode = FilterMode.Bilinear;
		int width = source.width >> downsample;
		int height = source.height >> downsample;
		RenderTexture renderTexture = RenderTexture.GetTemporary(width, height, 0, source.format);
		renderTexture.filterMode = FilterMode.Bilinear;
		Graphics.Blit(source, renderTexture, material, 0);
		int num2 = 0;
		for (int i = 0; i < iterations; i++)
		{
			float num3 = (float)i * 1f;
			material.SetVector("_Parameter", new Vector4(blurAmt * num + num3, (0f - blurAmt) * num - num3, 0f, 0f));
			RenderTexture temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, material, 1 + num2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
			temporary = RenderTexture.GetTemporary(width, height, 0, source.format);
			temporary.filterMode = FilterMode.Bilinear;
			Graphics.Blit(renderTexture, temporary, material, 2 + num2);
			RenderTexture.ReleaseTemporary(renderTexture);
			renderTexture = temporary;
		}
		Graphics.Blit(renderTexture, destination);
		RenderTexture.ReleaseTemporary(renderTexture);
	}
}
