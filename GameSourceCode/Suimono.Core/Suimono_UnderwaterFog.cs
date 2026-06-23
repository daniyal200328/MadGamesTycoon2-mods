using System;
using UnityEngine;

namespace Suimono.Core;

[AddComponentMenu("Image Effects/Suimono/UnderwaterFX")]
public class Suimono_UnderwaterFog : MonoBehaviour
{
	public bool showScreenMask;

	public bool doTransition;

	public bool cancelTransition;

	public bool useUnderSurfaceView;

	public bool distanceFog = true;

	public bool useRadialDistance = true;

	public bool heightFog;

	public float height = 1f;

	public float heightDensity = 2f;

	public float startDistance;

	public float fogStart;

	public float fogEnd = 20f;

	public float refractAmt = 0.005f;

	public float refractSpd = 1.5f;

	public float refractScale = 0.5f;

	public float lightFactor = 1f;

	public Color underwaterColor;

	public float dropsTime = 2f;

	public float wipeTime = 1f;

	public float transitionStrength = 1f;

	public int iterations = 2;

	public float blurSpread = 1f;

	public float darkRange = 40f;

	public float heightDepth = 1f;

	public float hFac;

	public Texture distortTex;

	public Texture mask2Tex;

	public Shader fogShader;

	public Material fogMaterial;

	private SuimonoModule moduleObject;

	private SuimonoModuleLib moduleLibrary;

	private float trans1Time = 1.1f;

	private float trans2Time = 1.1f;

	private int randSeed;

	private Random dropRand;

	private Vector2 dropOff;

	private Camera cam;

	private Transform camtr;

	private int pass;

	private int rtW;

	private int rtH;

	private RenderTexture buffer;

	private int i;

	private RenderTexture buffer2;

	private Vector3 camPos;

	private float FdotC;

	private float paramK;

	private float sceneStart;

	private float sceneEnd;

	private Vector4 sceneParams;

	private float diff;

	private float invDiff;

	private Matrix4x4 frustumCorners;

	private float fovWHalf;

	private Vector3 toRight;

	private Vector3 toTop;

	private Vector3 topLeft;

	private float camScale;

	private Vector3 topRight;

	private Vector3 bottomRight;

	private Vector3 bottomLeft;

	private float offc;

	private float off;

	private Transform trackobject;

	private float _deltaTime;

	private void Start()
	{
		cam = base.gameObject.GetComponent<Camera>();
		camtr = cam.transform;
		if (GameObject.Find("SUIMONO_Module") != null)
		{
			moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
			moduleLibrary = (SuimonoModuleLib)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModuleLib));
		}
		if (moduleLibrary != null)
		{
			distortTex = moduleLibrary.texNormalC;
			mask2Tex = moduleLibrary.texDrops;
		}
		randSeed = Environment.TickCount;
		dropRand = new Random(randSeed);
		fogShader = Shader.Find("Hidden/SuimonoUnderwaterFog");
		fogMaterial = new Material(fogShader);
	}

	private void LateUpdate()
	{
		if (dropRand == null)
		{
			dropRand = new Random(randSeed);
		}
		_deltaTime = Time.deltaTime;
		if (cancelTransition)
		{
			doTransition = false;
			cancelTransition = false;
			trans1Time = 1.1f;
			trans2Time = 1.1f;
		}
		if (doTransition)
		{
			doTransition = false;
			trans1Time = 0f;
			trans2Time = 0f;
			dropOff = new Vector2(dropRand.Next(0f, 1f), dropRand.Next(0f, 1f));
		}
		trans1Time += _deltaTime * 0.7f * wipeTime;
		trans2Time += _deltaTime * 0.1f * dropsTime;
	}

	private void OnRenderImage(RenderTexture source, RenderTexture destination)
	{
		Graphics.Blit(source, destination);
		frustumCorners = Matrix4x4.identity;
		fovWHalf = cam.fieldOfView * 0.5f;
		toRight = camtr.right * cam.nearClipPlane * Mathf.Tan(fovWHalf * (MathF.PI / 180f)) * cam.aspect;
		toTop = camtr.up * cam.nearClipPlane * Mathf.Tan(fovWHalf * (MathF.PI / 180f));
		topLeft = camtr.forward * cam.nearClipPlane - toRight + toTop;
		camScale = topLeft.magnitude * cam.farClipPlane / cam.nearClipPlane;
		topLeft.Normalize();
		topLeft *= camScale;
		topRight = camtr.forward * cam.nearClipPlane + toRight + toTop;
		topRight.Normalize();
		topRight *= camScale;
		bottomRight = camtr.forward * cam.nearClipPlane + toRight - toTop;
		bottomRight.Normalize();
		bottomRight *= camScale;
		bottomLeft = camtr.forward * cam.nearClipPlane - toRight - toTop;
		bottomLeft.Normalize();
		bottomLeft *= camScale;
		frustumCorners.SetRow(0, topLeft);
		frustumCorners.SetRow(1, topRight);
		frustumCorners.SetRow(2, bottomRight);
		frustumCorners.SetRow(3, bottomLeft);
		if (heightFog && base.transform.parent != null)
		{
			height = base.transform.parent.transform.position.y + 1f;
			heightDensity = 2f;
		}
		camPos = camtr.position;
		FdotC = camPos.y - height;
		paramK = ((FdotC <= 0f) ? 1f : 0f);
		sceneStart = fogStart;
		sceneEnd = fogEnd;
		diff = sceneEnd - sceneStart;
		invDiff = ((Mathf.Abs(diff) > 0.0001f) ? (1f / diff) : 0f);
		sceneParams.x = 0f;
		sceneParams.y = 0f;
		sceneParams.z = 0f - invDiff;
		sceneParams.w = sceneEnd * invDiff;
		if (!(fogMaterial != null))
		{
			return;
		}
		fogMaterial.SetMatrix("_FrustumCornersWS", frustumCorners);
		fogMaterial.SetVector("_CameraWS", camPos);
		fogMaterial.SetVector("_HeightParams", new Vector4(height, FdotC, paramK, heightDensity * 0.5f));
		fogMaterial.SetVector("_DistanceParams", new Vector4(0f - Mathf.Max(startDistance, 0f), 0f, 0f, 0f));
		fogMaterial.SetVector("_SceneFogParams", sceneParams);
		fogMaterial.SetVector("_SceneFogMode", new Vector4(1f, useRadialDistance ? 1f : 0f, 0f, 0f));
		fogMaterial.SetColor("_underwaterColor", underwaterColor);
		if (distortTex != null)
		{
			fogMaterial.SetTexture("_underwaterDistort", distortTex);
			fogMaterial.SetFloat("_distortAmt", refractAmt);
			fogMaterial.SetFloat("_distortSpeed", refractSpd);
			fogMaterial.SetFloat("_distortScale", refractScale);
			fogMaterial.SetFloat("_lightFactor", lightFactor);
		}
		if (distortTex != null)
		{
			fogMaterial.SetTexture("_distort1Mask", distortTex);
		}
		if (mask2Tex != null)
		{
			fogMaterial.SetTexture("_distort2Mask", mask2Tex);
		}
		fogMaterial.SetFloat("_trans1", trans1Time);
		fogMaterial.SetFloat("_trans2", trans2Time);
		fogMaterial.SetFloat("_transStrength", transitionStrength);
		fogMaterial.SetFloat("_dropOffx", dropOff.x);
		fogMaterial.SetFloat("_dropOffy", dropOff.y);
		fogMaterial.SetFloat("_showScreenMask", showScreenMask ? 1f : 0f);
		blurSpread = Mathf.Clamp01(blurSpread);
		fogMaterial.SetFloat("_blur", blurSpread);
		if (moduleObject != null)
		{
			if (moduleObject.setTrack != null)
			{
				trackobject = moduleObject.setTrack.transform;
			}
			else if (moduleObject.setCamera != null)
			{
				trackobject = moduleObject.setCamera.transform;
			}
			if (trackobject != null)
			{
				hFac = Mathf.Clamp(11.5f - trackobject.localPosition.y, 0f, 500f);
			}
			heightDepth = hFac;
			hFac = Mathf.Clamp01(Mathf.Lerp(-0.2f, 1f, Mathf.Clamp01(hFac / darkRange)));
			fogMaterial.SetFloat("_hDepth", hFac);
			fogMaterial.SetFloat("_enableUnderwater", moduleObject.enableUnderwaterFX ? 1f : 0f);
		}
		rtW = source.width / 4;
		rtH = source.height / 4;
		buffer = RenderTexture.GetTemporary(rtW, rtH, 0);
		DownSample4x(source, buffer);
		for (i = 0; i < iterations; i++)
		{
			buffer2 = RenderTexture.GetTemporary(rtW, rtH, 0);
			FourTapCone(buffer, buffer2, i);
			RenderTexture.ReleaseTemporary(buffer);
			buffer = buffer2;
		}
		Graphics.Blit(buffer, destination);
		RenderTexture.ReleaseTemporary(buffer);
		pass = 0;
		if (distanceFog && heightFog)
		{
			pass = 0;
		}
		else if (distanceFog)
		{
			pass = 1;
		}
		else
		{
			pass = 2;
		}
		CustomGraphicsBlit(source, destination, fogMaterial, pass);
	}

	private void CustomGraphicsBlit(RenderTexture source, RenderTexture dest, Material fxMaterial, int passNr)
	{
		RenderTexture.active = dest;
		fxMaterial.SetTexture("_MainTex", source);
		GL.PushMatrix();
		GL.LoadOrtho();
		fxMaterial.SetPass(passNr);
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

	private void FourTapCone(RenderTexture source, RenderTexture dest, int iteration)
	{
		offc = 0.5f + (float)iteration * blurSpread * 2f;
		Graphics.BlitMultiTap(source, dest, fogMaterial, new Vector2(0f - offc, 0f - offc), new Vector2(0f - offc, offc), new Vector2(offc, offc), new Vector2(offc, 0f - offc));
	}

	private void DownSample4x(RenderTexture source, RenderTexture dest)
	{
		off = 1f;
		Graphics.BlitMultiTap(source, dest, fogMaterial, new Vector2(0f - off, 0f - off), new Vector2(0f - off, off), new Vector2(off, off), new Vector2(off, 0f - off));
	}
}
