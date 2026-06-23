using System.Collections.Generic;
using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class Suimono_ShorelineObject : MonoBehaviour
{
	public int lodIndex;

	public int shorelineModeIndex;

	public List<string> shorelineModeOptions = new List<string> { "Auto-Generate", "Custom Texture" };

	public int shorelineRunIndex;

	public List<string> shorelineRunOptions = new List<string> { "At Start", "Continuous" };

	public Transform attachToSurface;

	public bool autoPosition = true;

	public float maxDepth = 25f;

	public float sceneDepth = 14.5f;

	public float shoreDepth = 27.7f;

	public bool debug;

	public string suimonoVersionNumber;

	public int depthLayer = 2;

	public List<string> suiLayerMasks = new List<string>();

	public Texture2D customDepthTex;

	public int useResolution = 512;

	private Material useMat;

	private Texture reflTex;

	private Texture envTex;

	private Matrix4x4 MV;

	private Camera CamInfo;

	private cameraTools CamTools;

	private SuimonoCamera_depth CamDepth;

	private float curr_sceneDepth;

	private float curr_shoreDepth;

	private float curr_foamDepth;

	private float curr_edgeDepth;

	private Vector3 currPos;

	private Vector3 currScale;

	private Quaternion currRot;

	private Vector4 camCoords = new Vector4(1f, 1f, 0f, 0f);

	private Material localMaterial;

	private Renderer renderObject;

	private MeshFilter meshObject;

	private Material matObject;

	public SuimonoModule moduleObject;

	private float maxScale;

	private int i;

	private string layerName;

	private bool hasRendered;

	private bool renderPass = true;

	private int saveMode = -1;

	private Vector3 gizPos;

	private void OnDrawGizmos()
	{
		gizPos = base.transform.position;
		gizPos.y += 0.03f;
		Gizmos.DrawIcon(gizPos, "gui_icon_shore.psd", allowScaling: true);
		gizPos.y -= 0.03f;
	}

	private void Start()
	{
		if (Application.isPlaying)
		{
			debug = false;
		}
		if (GameObject.Find("SUIMONO_Module") != null)
		{
			moduleObject = (SuimonoModule)Object.FindObjectOfType(typeof(SuimonoModule));
			suimonoVersionNumber = moduleObject.suimonoVersionNumber;
		}
		CamInfo = base.transform.Find("cam_LocalShore").gameObject.GetComponent<Camera>();
		CamInfo.depthTextureMode = DepthTextureMode.DepthNormals;
		CamTools = base.transform.Find("cam_LocalShore").gameObject.GetComponent<cameraTools>();
		CamDepth = base.transform.Find("cam_LocalShore").gameObject.GetComponent<SuimonoCamera_depth>();
		renderObject = base.gameObject.GetComponent<Renderer>();
		meshObject = base.gameObject.GetComponent<MeshFilter>();
		if ((bool)base.transform.parent && base.transform.parent.gameObject.GetComponent<SuimonoObject>() != null)
		{
			attachToSurface = base.transform.parent;
		}
		if (attachToSurface != null)
		{
			attachToSurface.Find("Suimono_Object").gameObject.GetComponent<Renderer>().enabled = true;
		}
		matObject = new Material(Shader.Find("Suimono2/Suimono2_FX_ShorelineObject"));
		renderObject.material = matObject;
		hasRendered = false;
	}

	private void LateUpdate()
	{
		if (!(moduleObject != null))
		{
			return;
		}
		suimonoVersionNumber = moduleObject.suimonoVersionNumber;
		base.gameObject.layer = moduleObject.layerDepthNum;
		CamInfo.gameObject.layer = moduleObject.layerDepthNum;
		CamInfo.farClipPlane = maxDepth;
		base.gameObject.tag = "Untagged";
		CamInfo.gameObject.tag = "Untagged";
		suiLayerMasks = new List<string>();
		for (i = 0; i < 32; i++)
		{
			layerName = LayerMask.LayerToName(i);
			suiLayerMasks.Add(layerName);
		}
		if (!Application.isPlaying && attachToSurface != null)
		{
			if (debug)
			{
				attachToSurface.Find("Suimono_Object").gameObject.GetComponent<Renderer>().enabled = false;
			}
			else
			{
				attachToSurface.Find("Suimono_Object").gameObject.GetComponent<Renderer>().enabled = true;
			}
		}
		if (shorelineModeIndex == 0)
		{
			if (CamInfo != null)
			{
				CamInfo.enabled = true;
				CamInfo.cullingMask = depthLayer;
			}
		}
		else if (CamInfo != null)
		{
			CamInfo.enabled = false;
		}
		if (debug)
		{
			if (renderObject != null)
			{
				renderObject.hideFlags = HideFlags.None;
			}
			if (meshObject != null)
			{
				meshObject.hideFlags = HideFlags.None;
			}
			if (matObject != null)
			{
				matObject.hideFlags = HideFlags.None;
			}
			if (shorelineModeIndex == 0)
			{
				if (CamInfo != null)
				{
					CamInfo.gameObject.hideFlags = HideFlags.None;
				}
				if (CamTools != null)
				{
					CamTools.executeInEditMode = true;
					CamTools.CameraUpdate();
				}
			}
			if (renderObject != null)
			{
				renderObject.enabled = true;
			}
		}
		else
		{
			if (renderObject != null)
			{
				renderObject.hideFlags = HideFlags.HideInInspector;
			}
			if (meshObject != null)
			{
				meshObject.hideFlags = HideFlags.HideInInspector;
			}
			if (matObject != null)
			{
				matObject.hideFlags = HideFlags.HideInInspector;
			}
			if (shorelineModeIndex == 0)
			{
				if (CamInfo != null)
				{
					CamInfo.gameObject.hideFlags = HideFlags.HideInHierarchy;
				}
				if (CamTools != null)
				{
					CamTools.executeInEditMode = false;
				}
			}
			if (!Application.isPlaying && renderObject != null)
			{
				renderObject.enabled = false;
			}
			else
			{
				renderObject.enabled = true;
			}
		}
		if (saveMode != shorelineModeIndex)
		{
			saveMode = shorelineModeIndex;
			hasRendered = false;
		}
		renderPass = true;
		if (shorelineModeIndex == 0)
		{
			if (shorelineRunIndex == 0 && hasRendered && Application.isPlaying)
			{
				renderPass = false;
			}
			if (shorelineRunIndex == 1)
			{
				renderPass = true;
			}
		}
		if (shorelineModeIndex == 1 && hasRendered && Application.isPlaying)
		{
			renderPass = false;
		}
		if (!renderPass)
		{
			if (CamInfo != null)
			{
				CamInfo.enabled = false;
			}
			if (CamTools != null)
			{
				CamTools.enabled = false;
			}
			return;
		}
		if (CamInfo != null)
		{
			CamInfo.enabled = true;
		}
		if (CamTools != null)
		{
			CamTools.enabled = true;
		}
		if (CamDepth != null)
		{
			CamDepth.enabled = true;
		}
		if (shorelineModeIndex == 0)
		{
			CamDepth._sceneDepth = sceneDepth;
			CamDepth._shoreDepth = shoreDepth;
		}
		if (!(attachToSurface != null))
		{
			return;
		}
		base.transform.localScale = new Vector3(base.transform.localScale.x, 1f, base.transform.localScale.z);
		if (attachToSurface != null && autoPosition)
		{
			base.transform.position = new Vector3(base.transform.position.x, attachToSurface.position.y, base.transform.position.z);
		}
		if (shorelineModeIndex == 0)
		{
			maxScale = Mathf.Max(base.transform.localScale.x, base.transform.localScale.z);
			CamInfo.orthographicSize = maxScale * 20f;
			if (base.transform.localScale.x < base.transform.localScale.z)
			{
				camCoords = new Vector4(base.transform.localScale.x / base.transform.localScale.z, 1f, 0.5f - base.transform.localScale.x / base.transform.localScale.z * 0.5f, 0f);
			}
			else if (base.transform.localScale.x > base.transform.localScale.z)
			{
				camCoords = new Vector4(1f, base.transform.localScale.z / base.transform.localScale.x, 0f, 0.5f - base.transform.localScale.z / base.transform.localScale.x * 0.5f);
			}
			CamTools.surfaceRenderer.sharedMaterial.SetColor("_Mult", camCoords);
			if (CamTools != null)
			{
				if (currPos != base.transform.position)
				{
					currPos = base.transform.position;
					CamTools.CameraUpdate();
				}
				if (currScale != base.transform.localScale)
				{
					currScale = base.transform.localScale;
					CamTools.CameraUpdate();
				}
				if (currRot != base.transform.rotation)
				{
					currRot = base.transform.rotation;
					CamTools.CameraUpdate();
				}
				if (curr_sceneDepth != sceneDepth)
				{
					curr_sceneDepth = sceneDepth;
					CamTools.CameraUpdate();
				}
				if (curr_shoreDepth != shoreDepth)
				{
					curr_shoreDepth = shoreDepth;
					CamTools.CameraUpdate();
				}
				if (Application.isPlaying)
				{
					CamTools.CameraUpdate();
				}
			}
		}
		if (shorelineModeIndex == 1 && customDepthTex != null && renderObject != null)
		{
			renderObject.sharedMaterial.SetColor("_Mult", new Vector4(1f, 1f, 0f, 0f));
			renderObject.sharedMaterial.SetTexture("_MainTex", customDepthTex);
		}
		if (Application.isPlaying && Time.time > 1f)
		{
			hasRendered = true;
		}
	}
}
