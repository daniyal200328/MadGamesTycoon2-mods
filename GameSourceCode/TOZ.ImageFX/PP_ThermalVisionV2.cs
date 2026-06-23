using System;
using UnityEngine;

namespace TOZ.ImageFX;

[ExecuteInEditMode]
public sealed class PP_ThermalVisionV2 : PostProcessBase
{
	public Texture2D ThermalTex;

	public Texture2D NoiseTex;

	public float NoiseAmount = 1f;

	[Range(0f, 2.2f)]
	public float Gamma = 2.2f;

	private Camera cam;

	private void Awake()
	{
		shd = Shader.Find("Hidden/TOZ/ImageFX/ThermalVisionV2");
		cam = GetComponent<Camera>();
		cam.depthTextureMode |= DepthTextureMode.DepthNormals;
	}

	private void OnRenderImage(RenderTexture src, RenderTexture dest)
	{
		if (!base.enabled)
		{
			Graphics.Blit(src, dest);
			return;
		}
		float nearClipPlane = cam.nearClipPlane;
		float farClipPlane = cam.farClipPlane;
		float num = cam.fieldOfView * 0.5f;
		float aspect = cam.aspect;
		Matrix4x4 identity = Matrix4x4.identity;
		Vector3 vector = base.transform.right * nearClipPlane * Mathf.Tan(num * (MathF.PI / 180f)) * aspect;
		Vector3 vector2 = base.transform.up * nearClipPlane * Mathf.Tan(num * (MathF.PI / 180f));
		Vector3 vector3 = base.transform.forward * nearClipPlane - vector + vector2;
		float num2 = vector3.magnitude * farClipPlane / nearClipPlane;
		vector3.Normalize();
		vector3 *= num2;
		identity.SetRow(0, vector3);
		vector3 = base.transform.forward * nearClipPlane + vector + vector2;
		vector3.Normalize();
		vector3 *= num2;
		identity.SetRow(1, vector3);
		vector3 = base.transform.forward * nearClipPlane + vector - vector2;
		vector3.Normalize();
		vector3 *= num2;
		identity.SetRow(2, vector3);
		vector3 = base.transform.forward * nearClipPlane - vector - vector2;
		vector3.Normalize();
		vector3 *= num2;
		identity.SetRow(3, vector3);
		mat.SetMatrix("_WS_FrustumCorners", identity);
		mat.SetVector("_WS_CameraPosition", base.transform.position);
		ApplyVariables();
		CustomGraphicsBlit(src, dest, mat, 0);
	}

	private static void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material mat, int pass)
	{
		RenderTexture.active = dest;
		mat.SetTexture("_MainTex", source);
		GL.PushMatrix();
		GL.LoadOrtho();
		mat.SetPass(pass);
		GL.Begin(7);
		GL.MultiTexCoord2(0, 0f, 0f);
		GL.Vertex3(0f, 0f, 3f);
		GL.MultiTexCoord2(0, 1f, 0f);
		GL.Vertex3(1f, 0f, 2f);
		GL.MultiTexCoord2(0, 1f, 1f);
		GL.Vertex3(1f, 1f, 1f);
		GL.MultiTexCoord2(0, 0f, 1f);
		GL.Vertex3(0f, 1f, 0f);
		GL.End();
		GL.PopMatrix();
	}

	private void ApplyVariables()
	{
		if (NoiseTex != null)
		{
			mat.SetTexture("_NoiseTex", NoiseTex);
		}
		if (ThermalTex != null)
		{
			mat.SetTexture("_ThermalTex", ThermalTex);
		}
		mat.SetFloat("_NoiseAmount", NoiseAmount);
		mat.SetFloat("_Gamma", Gamma);
	}
}
