using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_Hollywood : PostProcessBase
{
	[Range(0f, 1f)]
	public float Threshold = 0.25f;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/Hollywood");
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		ApplyVariables();
		Graphics.Blit(src, dest, mat);
	}

	private void ApplyVariables()
	{
		Matrix4x4 zero = Matrix4x4.zero;
		zero.m00 = 0.5149f;
		zero.m01 = 0.3244f;
		zero.m02 = 0.1607f;
		zero.m03 = 0f;
		zero.m10 = 0.2654f;
		zero.m11 = 0.6704f;
		zero.m12 = 0.0642f;
		zero.m13 = 0f;
		zero.m20 = 0.0248f;
		zero.m21 = 0.1248f;
		zero.m22 = 0.8504f;
		zero.m23 = 0f;
		zero.m30 = 0f;
		zero.m31 = 0f;
		zero.m32 = 0f;
		zero.m33 = 0f;
		mat.SetMatrix("_MtxColor", zero);
		mat.SetFloat("_Threshold", Threshold);
	}
}
