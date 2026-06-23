using UnityEngine;
using UnityEngine.Rendering;

namespace Suimono.Core;

[ExecuteInEditMode]
public class cameraTools : MonoBehaviour
{
	public suiCamToolType cameraType;

	public suiCamToolRender renderType;

	public suiCamHdrMode hdrMode;

	public suiCamClearFlags clearMode;

	public Color clearFlagColor = Color.black;

	public int resolution = 256;

	public float cameraOffset;

	public float reflectionOffset;

	public RenderTexture renderTexDiff;

	public Shader renderShader;

	public bool executeInEditMode;

	public bool isUnderwater;

	[HideInInspector]
	public Renderer surfaceRenderer;

	[HideInInspector]
	public Renderer scaleRenderer;

	[HideInInspector]
	public float reflectionDistance = 200f;

	[HideInInspector]
	public int setLayers;

	private RenderingPath usePath;

	private SuimonoModule suimonoModuleObject;

	private Camera cam;

	private Camera copyCam;

	private int currResolution = 256;

	private float clipPlaneOffset = 0.07f;

	private Vector3 pos;

	private Vector3 normal;

	private float d;

	private Vector4 reflectionPlane;

	private Matrix4x4 reflection;

	private Vector3 oldpos;

	private Vector3 newpos;

	private Vector4 clipPlane;

	private Matrix4x4 projection;

	private Vector3 euler;

	private Matrix4x4 scaleOffset;

	private Vector3 scale;

	private Matrix4x4 mtx;

	private Vector3 offsetPos;

	private Matrix4x4 m;

	private Vector3 cpos;

	private Vector3 cnormal;

	private Matrix4x4 proj;

	private Vector4 q;

	private Vector4 c;

	private float hasStarted;

	private Vector3 cameraPos = Vector3.zero;

	private Suimono_ShorelineObject shoreObject;

	private void Start()
	{
		suimonoModuleObject = (SuimonoModule)Object.FindObjectOfType(typeof(SuimonoModule));
		if (cameraType != suiCamToolType.localReflection)
		{
			if (base.transform.parent != null)
			{
				surfaceRenderer = base.transform.parent.gameObject.GetComponent<Renderer>();
			}
		}
		else if (base.transform.parent != null)
		{
			surfaceRenderer = base.transform.parent.Find("Suimono_Object").gameObject.GetComponent<Renderer>();
			scaleRenderer = base.transform.parent.Find("Suimono_ObjectScale").gameObject.GetComponent<Renderer>();
		}
		cam = base.gameObject.GetComponent<Camera>();
		SetCopyCamera();
		UpdateRenderTex();
		CameraUpdate();
	}

	private void OnPreRender()
	{
		if (Application.isPlaying && cameraType == suiCamToolType.localReflection)
		{
			GL.invertCulling = true;
		}
	}

	private void OnPostRender()
	{
		if (Application.isPlaying)
		{
			GL.invertCulling = false;
		}
	}

	private void Update()
	{
		if (!Application.isPlaying && executeInEditMode)
		{
			CameraUpdate();
		}
		if (cam != null && cameraType == suiCamToolType.shorelineCapture)
		{
			cam.cullingMask = 1 << suimonoModuleObject.layerDepthNum;
		}
		bool flag = false;
		int num = 0;
		if (cameraType == suiCamToolType.shorelineObject)
		{
			return;
		}
		if (cam.transform.rotation.x == 0f)
		{
			num++;
		}
		if (cam.transform.rotation.y == 0f)
		{
			num++;
		}
		if (cam.transform.rotation.z == 0f)
		{
			num++;
		}
		if (num > 1)
		{
			flag = true;
		}
		if (flag)
		{
			Quaternion rotation = cam.transform.rotation;
			if (cam.transform.rotation.x == 0f)
			{
				rotation.x = 0.001f;
			}
			if (cam.transform.rotation.y == 0f)
			{
				rotation.y = 0.001f;
			}
			if (cam.transform.rotation.z == 0f)
			{
				rotation.z = 0.001f;
			}
			cam.transform.rotation = rotation;
		}
	}

	private void LateUpdate()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (cameraType == suiCamToolType.shorelineObject)
		{
			if (hasStarted == 0f && Time.time > 0.2f)
			{
				CameraUpdate();
				hasStarted = 1f;
			}
		}
		else
		{
			CameraUpdate();
		}
	}

	private void SetCopyCamera()
	{
		if (suimonoModuleObject != null && suimonoModuleObject.setCamera != null)
		{
			if (suimonoModuleObject.setCameraComponent != null)
			{
				copyCam = suimonoModuleObject.setCameraComponent;
			}
			else
			{
				copyCam = suimonoModuleObject.setCamera.GetComponent<Camera>();
			}
		}
	}

	private void CameraRender()
	{
		if (cameraType == suiCamToolType.localReflection)
		{
			ReflectionPreRender();
		}
		cam.targetTexture = renderTexDiff;
		if (Application.isPlaying && cameraType == suiCamToolType.shorelineObject)
		{
			cam.enabled = false;
			cameraPos.y = 3f;
			cam.transform.localPosition = cameraPos;
			cam.nearClipPlane = 0.01f;
			cam.farClipPlane = 50f;
			cam.Render();
		}
		else
		{
			cam.enabled = true;
		}
		if (cameraType == suiCamToolType.localReflection)
		{
			ReflectionPostRender();
		}
	}

	public void CameraUpdate()
	{
		SetCopyCamera();
		if (!(copyCam != null) || !(cam != null))
		{
			return;
		}
		if (cameraType != suiCamToolType.shorelineObject)
		{
			cam.transform.position = copyCam.transform.position;
			cam.transform.rotation = copyCam.transform.rotation;
			cam.projectionMatrix = copyCam.projectionMatrix;
			cam.fieldOfView = copyCam.fieldOfView;
		}
		if (cameraOffset != 0f)
		{
			cam.transform.Translate(Vector3.forward * cameraOffset);
		}
		if (renderType == suiCamToolRender.automatic)
		{
			usePath = copyCam.actualRenderingPath;
			if (cameraType == suiCamToolType.transparent)
			{
				if (copyCam.renderingPath == RenderingPath.Forward)
				{
					usePath = RenderingPath.DeferredLighting;
				}
				else
				{
					usePath = copyCam.renderingPath;
				}
			}
		}
		else if (renderType == suiCamToolRender.deferredShading)
		{
			usePath = RenderingPath.DeferredShading;
		}
		else if (renderType == suiCamToolRender.deferredLighting)
		{
			usePath = RenderingPath.DeferredLighting;
		}
		else if (renderType == suiCamToolRender.forward)
		{
			usePath = RenderingPath.Forward;
		}
		cam.renderingPath = usePath;
		if (renderTexDiff != null)
		{
			if (resolution != currResolution)
			{
				if (cameraType == suiCamToolType.shorelineObject)
				{
					shoreObject = base.transform.parent.gameObject.GetComponent<Suimono_ShorelineObject>();
					if (shoreObject != null)
					{
						resolution = shoreObject.useResolution;
					}
				}
				currResolution = resolution;
				UpdateRenderTex();
			}
			if (cameraType == suiCamToolType.normals)
			{
				if (suimonoModuleObject.enableAdvancedDistort)
				{
					cam.allowHDR = false;
					cam.SetReplacementShader(renderShader, "RenderType");
					CameraRender();
				}
				else
				{
					renderTexDiff = null;
				}
			}
			else if (cameraType == suiCamToolType.wakeEffects)
			{
				if (suimonoModuleObject.enableAdvancedDistort)
				{
					cam.SetReplacementShader(renderShader, "RenderType");
					CameraRender();
				}
				else
				{
					renderTexDiff = null;
				}
			}
			else if (cameraType == suiCamToolType.transparent)
			{
				if (suimonoModuleObject.enableTransparency)
				{
					CameraRender();
				}
				else
				{
					renderTexDiff = null;
				}
			}
			else if (cameraType == suiCamToolType.transparentCaustic)
			{
				if (suimonoModuleObject.enableCaustics)
				{
					CameraRender();
				}
				else
				{
					renderTexDiff = null;
				}
			}
			else
			{
				CameraRender();
			}
			if (cameraType == suiCamToolType.transparent)
			{
				Shader.SetGlobalTexture("_suimono_TransTex", renderTexDiff);
				if (!suimonoModuleObject.enableCausticsBlending)
				{
					Shader.SetGlobalTexture("_suimono_CausticTex", renderTexDiff);
				}
			}
			if (cameraType == suiCamToolType.transparentCaustic && suimonoModuleObject.enableCausticsBlending && suimonoModuleObject.enableCaustics)
			{
				Shader.SetGlobalTexture("_suimono_CausticTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.wakeEffects)
			{
				Shader.SetGlobalTexture("_suimono_WakeTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.normals)
			{
				Shader.SetGlobalTexture("_suimono_NormalsTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.depthMask)
			{
				Shader.SetGlobalTexture("_suimono_depthMaskTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.underwaterMask)
			{
				Shader.SetGlobalTexture("_suimono_underwaterMaskTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.underwater)
			{
				Shader.SetGlobalTexture("_suimono_underwaterTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.localReflection && surfaceRenderer != null)
			{
				surfaceRenderer.sharedMaterial.SetTexture("_ReflectionTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.shorelineObject && surfaceRenderer != null)
			{
				surfaceRenderer.sharedMaterial.SetTexture("_MainTex", renderTexDiff);
			}
			if (cameraType == suiCamToolType.shorelineCapture)
			{
				Shader.SetGlobalTexture("_suimono_shorelineTex", renderTexDiff);
			}
		}
		else
		{
			UpdateRenderTex();
		}
	}

	private void UpdateRenderTex()
	{
		if (resolution < 4)
		{
			resolution = 4;
		}
		if (renderTexDiff != null)
		{
			if (cam != null)
			{
				cam.targetTexture = null;
			}
			Object.DestroyImmediate(renderTexDiff);
		}
		renderTexDiff = new RenderTexture(resolution, resolution, 24, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
		renderTexDiff.dimension = TextureDimension.Tex2D;
		renderTexDiff.autoGenerateMips = false;
		renderTexDiff.anisoLevel = 1;
		renderTexDiff.filterMode = FilterMode.Trilinear;
		renderTexDiff.wrapMode = TextureWrapMode.Clamp;
	}

	private void ReflectionPreRender()
	{
		pos = base.transform.parent.position;
		if (isUnderwater)
		{
			normal = -base.transform.parent.transform.up;
		}
		else
		{
			normal = base.transform.parent.transform.up;
		}
		cam.CopyFrom(copyCam);
		cam.backgroundColor = clearFlagColor;
		if (hdrMode == suiCamHdrMode.off)
		{
			cam.allowHDR = false;
		}
		else if (hdrMode == suiCamHdrMode.on)
		{
			cam.allowHDR = true;
		}
		if (isUnderwater)
		{
			cam.farClipPlane = 3f;
			cam.clearFlags = CameraClearFlags.Color;
			cam.depthTextureMode = DepthTextureMode.Depth;
		}
		else if (clearMode != suiCamClearFlags.automatic)
		{
			if (clearMode == suiCamClearFlags.skybox)
			{
				cam.clearFlags = CameraClearFlags.Skybox;
			}
			if (clearMode == suiCamClearFlags.color)
			{
				cam.clearFlags = CameraClearFlags.Color;
				cam.backgroundColor = clearFlagColor;
			}
		}
		if (cameraType == suiCamToolType.localReflection && renderShader != null)
		{
			cam.SetReplacementShader(renderShader, null);
		}
		cam.cullingMask = setLayers;
		d = 0f - Vector3.Dot(normal, pos) - clipPlaneOffset;
		reflectionPlane = new Vector4(normal.x, normal.y - reflectionOffset, normal.z, d);
		reflection = Matrix4x4.zero;
		reflection = Set_CalculateReflectionMatrix(reflectionPlane);
		oldpos = copyCam.transform.position;
		newpos = reflection.MultiplyPoint(oldpos);
		cam.worldToCameraMatrix = copyCam.worldToCameraMatrix * reflection;
		clipPlane = Set_CameraSpacePlane(cam, pos, normal, 1f);
		projection = copyCam.projectionMatrix;
		projection = Set_CalculateObliqueMatrix(clipPlane);
		cam.projectionMatrix = projection;
		GL.invertCulling = true;
		cam.transform.position = newpos;
		euler = copyCam.transform.eulerAngles;
		cam.transform.eulerAngles = new Vector3(0f, euler.y, euler.z);
	}

	private void ReflectionPostRender()
	{
		cam.transform.position = oldpos;
		GL.invertCulling = false;
		scaleOffset = Matrix4x4.TRS(new Vector3(0.5f, 0.5f, 0.5f), Quaternion.identity, new Vector3(0.5f, 0.5f, 0.5f));
		scale = base.transform.lossyScale;
		mtx = base.transform.localToWorldMatrix * Matrix4x4.Scale(new Vector3(1f / scale.x, -1f / scale.y, 1f / scale.z));
		mtx = scaleOffset * copyCam.projectionMatrix * copyCam.worldToCameraMatrix * mtx;
	}

	public float Set_sgn(float a)
	{
		if (a > 0f)
		{
			return 1f;
		}
		if (a < 0f)
		{
			return -1f;
		}
		return 0f;
	}

	public Vector4 Set_CameraSpacePlane(Camera cm, Vector3 pos, Vector3 normal, float sideSign)
	{
		offsetPos = pos + normal * clipPlaneOffset;
		m = cm.worldToCameraMatrix;
		cpos = m.MultiplyPoint(offsetPos);
		cnormal = m.MultiplyVector(normal).normalized * sideSign;
		return new Vector4(cnormal.x, cnormal.y, cnormal.z, 0f - Vector3.Dot(cpos, cnormal));
	}

	public Matrix4x4 Set_CalculateObliqueMatrix(Vector4 clipPlane)
	{
		proj = copyCam.projectionMatrix;
		q = proj.inverse * new Vector4(Set_sgn(clipPlane.x), Set_sgn(clipPlane.y), 1f, 1f);
		c = clipPlane * (2f / Vector4.Dot(clipPlane, q));
		proj[2] = c.x - proj[3];
		proj[6] = c.y - proj[7];
		proj[10] = c.z - proj[11];
		proj[14] = c.w - proj[15];
		return proj;
	}

	public Matrix4x4 Set_CalculateReflectionMatrix(Vector4 plane)
	{
		Matrix4x4 zero = Matrix4x4.zero;
		zero.m00 = 1f - 2f * plane[0] * plane[0];
		zero.m01 = -2f * plane[0] * plane[1];
		zero.m02 = -2f * plane[0] * plane[2];
		zero.m03 = -2f * plane[3] * plane[0];
		zero.m10 = -2f * plane[1] * plane[0];
		zero.m11 = 1f - 2f * plane[1] * plane[1];
		zero.m12 = -2f * plane[1] * plane[2];
		zero.m13 = -2f * plane[3] * plane[1];
		zero.m20 = -2f * plane[2] * plane[0];
		zero.m21 = -2f * plane[2] * plane[1];
		zero.m22 = 1f - 2f * plane[2] * plane[2];
		zero.m23 = -2f * plane[3] * plane[2];
		zero.m30 = 0f;
		zero.m31 = 0f;
		zero.m32 = 0f;
		zero.m33 = 1f;
		return zero;
	}
}
