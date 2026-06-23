using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class SuimonoObject : MonoBehaviour
{
	public float systemTime;

	public float systemLocalTime;

	public float flowSpeed = 0.1f;

	public float flowDirection = 180f;

	public bool useBeaufortScale;

	public float beaufortScale = 1f;

	public float turbulenceFactor = 1f;

	public float waveScale = 0.5f;

	public float lgWaveHeight;

	public float lgWaveScale = 1f;

	public float waveHeight = 1f;

	public float heightProjection = 1f;

	public float useHeightProjection = 1f;

	public float refractStrength = 1f;

	public float reflectProjection = 1f;

	public float reflectBlur;

	public float aberrationScale = 0.1f;

	public float specularPower = 1f;

	public float roughness = 0.1f;

	public float roughness2 = 0.35f;

	public float reflectTerm = 0.0255f;

	public float reflectSharpen;

	public bool showDepthMask;

	public bool showWorldMask;

	public float cameraDistance = 1000f;

	public float underwaterDepth = 5f;

	public bool useDX9Settings;

	public SuimonoModule moduleObject;

	private SuimonoModuleLib suimonoModuleLibrary;

	private GameObject suimonoObject;

	private Renderer surfaceRenderer;

	private MeshFilter surfaceMesh;

	private MeshCollider surfaceCollider;

	private cameraTools surfaceReflections;

	private Suimono_DistanceBlur surfaceReflBlur;

	private GameObject scaleObject;

	private Renderer scaleRenderer;

	private MeshCollider scaleCollider;

	private MeshFilter scaleMesh;

	private Renderer surfaceVolume;

	private Material tempMaterial;

	public string suimonoVersionNumber;

	public bool showGeneral;

	public int typeIndex = 1;

	[NonSerialized]
	public List<string> typeOptions = new List<string> { "Infinite 3D Ocean", "3D Waves", "Flat Plane" };

	public int editorIndex = 1;

	public int editorUseIndex = 1;

	[NonSerialized]
	public List<string> editorOptions = new List<string> { "Simple", "Advanced" };

	public bool enableCustomMesh;

	public float cmScaleX = 1f;

	public float cmScaleY = 1f;

	public int lodIndex;

	public int useLodIndex;

	[NonSerialized]
	public List<string> lodOptions = new List<string> { "High Detail", "Medium Detail", "Low Detail", "Single Quad" };

	public Mesh customMesh;

	public float oceanScale = 2f;

	private bool meshWasSet;

	public bool enableCausticFX = true;

	public float causticsFade = 0.55f;

	public Color causticsColor = new Color(1f, 1f, 1f, 1f);

	public bool enableTess = true;

	public bool useEnableTess = true;

	public float waveTessAmt = 8f;

	public float waveTessMin;

	public float waveTessSpread = 0.08f;

	public bool enableInteraction = true;

	public float dynamicReflectFlag = 1f;

	public bool enableReflections = true;

	public bool enableDynamicReflections = true;

	public bool useEnableReflections = true;

	public bool useEnableDynamicReflections = true;

	public bool useReflections = true;

	public bool useDynReflections = true;

	public int reflectLayer;

	public int reflectResolution = 4;

	public LayerMask reflectLayerMask;

	public float reflectionDistance = 1000f;

	[NonSerialized]
	public List<string> suiLayerMasks = new List<string>();

	[NonSerialized]
	public List<string> resOptions = new List<string> { "4096", "2048", "1024", "512", "256", "128", "64", "32", "16", "8" };

	[NonSerialized]
	public List<int> resolutions = new List<int> { 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8 };

	public int reflectFallback = 1;

	[NonSerialized]
	public List<string> resFallbackOptions = new List<string> { "None", "skybox", "Custom Cubemap", "Color" };

	public Texture customRefCubemap;

	public Color customRefColor = new Color(0.9f, 0.9f, 0.9f, 1f);

	public Color reflectionColor = new Color(1f, 1f, 1f, 1f);

	public bool enableCustomTextures;

	public Texture2D customTexNormal1;

	public Texture2D customTexNormal2;

	public Texture2D customTexNormal3;

	public Texture2D useTexNormal1;

	public Texture2D useTexNormal2;

	public Texture2D useTexNormal3;

	public bool showWaves;

	public bool customWaves;

	public float localTime;

	private Vector2 flow_dir = new Vector2(0f, 0f);

	private Vector3 tempAngle;

	public float beaufortVal = 1f;

	public bool showShore;

	public float shorelineHeight = 0.75f;

	public float shorelineFreq = 0.5f;

	public float shorelineScale = 0.15f;

	public float shorelineSpeed = 2.5f;

	public float shorelineNorm = 0.5f;

	public bool showSurface;

	public float overallBright = 1f;

	public float overallTransparency = 1f;

	public float depthAmt = 0.1f;

	public float shallowAmt = 0.1f;

	public Color depthColor;

	public Color shallowColor;

	public float edgeAmt = 0.1f;

	public Color specularColor;

	public Color sssColor;

	public Color blendColor;

	public Color overlayColor;

	public bool showFoam;

	public bool enableFoam = true;

	public Color foamColor = new Color(0.9f, 0.9f, 0.9f, 1f);

	public float foamScale = 40f;

	public float foamSpeed = 0.1f;

	public float edgeFoamAmt = 0.5f;

	public float shallowFoamAmt = 1f;

	public float hFoamHeight = 1f;

	public float hFoamSpread = 1f;

	public float heightFoamAmt = 0.5f;

	public bool showUnderwater;

	public Color underwaterColor = new Color(1f, 0f, 0f, 1f);

	public float underLightFactor = 1f;

	public float underRefractionAmount = 0.005f;

	public float underRefractionScale = 1.5f;

	public float underRefractionSpeed = 0.5f;

	public float underwaterFogDist = 20f;

	public float underwaterFogSpread;

	public bool enableUnderwater = true;

	public bool enableUnderDebris;

	public float underBlurAmount = 1f;

	public float underDarkRange = 40f;

	public float setScale = 1f;

	public Vector3 currentAngles = new Vector3(0f, 0f, 0f);

	public Vector3 currentPosition = new Vector3(0f, 0f, 0f);

	public Vector3 newPos = new Vector3(0f, 0f, 0f);

	public float spacer;

	public float setScaleX;

	public float setScaleZ;

	public float offamt;

	public Vector2 savePos = new Vector2(0f, 0f);

	public Vector2 recPos = new Vector2(0f, 0f);

	public Vector2 _suimono_uv = new Vector2(0f, 0f);

	public bool showSimpleEditor;

	public Shader useShader;

	public Shader currUseShader;

	public Shader shader_Surface;

	public Shader shader_Scale;

	public Shader shader_Under;

	[NonSerialized]
	public List<string> presetDirs;

	public string[] presetFiles;

	public int presetIndex = -1;

	public int presetUseIndex = -1;

	public int presetFileIndex;

	public int presetFileUseIndex;

	public string[] presetOptions;

	public bool showPresets;

	public bool presetStartTransition;

	public float presetTimer = 1f;

	public string currentPresetFolder = "Built-In Presets";

	public string currentPresetName = "";

	public int presetTransitionCurrent;

	public float presetTransitionTime = 1f;

	public int presetTransIndexFrm;

	public int presetTransIndexTo;

	public bool presetToggleSave;

	public bool presetsLoaded;

	public string[] presetDataArray;

	public string presetDataString;

	public string dir = "";

	public string baseDir = "SUIMONO - WATER SYSTEM 2/RESOURCES/";

	public string presetSaveName = "my custom preset";

	public string presetFile = "";

	public string workData;

	public string workData2;

	private Color temp_depthColor;

	private Color temp_shallowColor;

	private Color temp_blendColor;

	private Color temp_overlayColor;

	private Color temp_causticsColor;

	private Color temp_reflectionColor;

	private Color temp_specularColor;

	private Color temp_sssColor;

	private Color temp_foamColor;

	private Color temp_underwaterColor;

	private float temp_beaufortScale;

	private float temp_flowDirection;

	private float temp_flowSpeed;

	private float temp_waveScale;

	private float temp_waveHeight;

	private float temp_heightProjection;

	private float temp_turbulenceFactor;

	private float temp_lgWaveHeight;

	private float temp_lgWaveScale;

	private float temp_shorelineHeight;

	private float temp_shorelineFreq;

	private float temp_shorelineScale;

	private float temp_shorelineSpeed;

	private float temp_shorelineNorm;

	private float temp_overallBright;

	private float temp_overallTransparency;

	private float temp_edgeAmt;

	private float temp_depthAmt;

	private float temp_shallowAmt;

	private float temp_refractStrength;

	private float temp_aberrationScale;

	private float temp_causticsFade;

	private float temp_reflectProjection;

	private float temp_reflectBlur;

	private float temp_reflectTerm;

	private float temp_reflectSharpen;

	private float temp_roughness;

	private float temp_roughness2;

	private float temp_foamScale;

	private float temp_foamSpeed;

	private float temp_edgeFoamAmt;

	private float temp_shallowFoamAmt;

	private float temp_heightFoamAmt;

	private float temp_hFoamHeight;

	private float temp_hFoamSpread;

	private float temp_underLightFactor;

	private float temp_underRefractionAmount;

	private float temp_underRefractionScale;

	private float temp_underRefractionSpeed;

	private float temp_underBlurAmount;

	private float temp_underwaterFogDist;

	private float temp_underwaterFogSpread;

	private float temp_underDarkRange;

	public string materialPath;

	public float oceanUseScale;

	public float useSc;

	public Vector2 setSc;

	public Vector2 scaleOff;

	public int i;

	public string layerName;

	public Material skybox;

	[NonSerialized]
	public List<string> presetDirsArr = new List<string>();

	public int d;

	public int dn;

	[NonSerialized]
	public List<string> presetFilesArr = new List<string>();

	public string pdir;

	public FileInfo[] fileInfo;

	public int f;

	public int px;

	public int nx;

	public int ax;

	public int n;

	[NonSerialized]
	public List<string> tempPresetDirsArr = new List<string>();

	public FileInfo[] dirInfo;

	public string[] tempPresetDirs;

	[NonSerialized]
	public List<string> tempPresetFilesArr = new List<string>();

	public string[] tempPresetFiles;

	public string oldName;

	public string moveName;

	public int setNum;

	public StreamWriter sw;

	public StreamReader sr;

	public string key;

	public string dat;

	public int pFrom;

	public int pTo;

	public int dx;

	public TextAsset datFile;

	public string[] dataS;

	public string retData;

	public bool retVal;

	private float suimono_refl_off;

	private float suimono_refl_sky;

	private float suimono_refl_cube;

	private float suimono_refl_color;

	private static bool reloadData;

	private void Start()
	{
		if (GameObject.Find("SUIMONO_Module") != null)
		{
			moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
			if (moduleObject != null)
			{
				suimonoModuleLibrary = moduleObject.GetComponent<SuimonoModuleLib>();
			}
		}
		baseDir = "Resources/";
		dir = Path.Combine(Application.dataPath, baseDir);
		suimonoObject = base.transform.Find("Suimono_Object").gameObject;
		surfaceRenderer = base.transform.Find("Suimono_Object").gameObject.GetComponent<Renderer>();
		surfaceMesh = base.transform.Find("Suimono_Object").GetComponent<MeshFilter>();
		surfaceCollider = base.transform.Find("Suimono_Object").GetComponent<MeshCollider>();
		surfaceReflections = base.transform.Find("cam_LocalReflections").gameObject.GetComponent<cameraTools>();
		surfaceReflBlur = base.transform.Find("cam_LocalReflections").gameObject.GetComponent<Suimono_DistanceBlur>();
		scaleObject = base.transform.Find("Suimono_ObjectScale").gameObject;
		scaleRenderer = base.transform.Find("Suimono_ObjectScale").gameObject.GetComponent<Renderer>();
		scaleCollider = base.transform.Find("Suimono_ObjectScale").gameObject.GetComponent<MeshCollider>();
		if (scaleCollider == null)
		{
			scaleCollider = base.transform.Find("Suimono_ObjectScale").gameObject.AddComponent<MeshCollider>();
		}
		scaleMesh = base.transform.Find("Suimono_ObjectScale").GetComponent<MeshFilter>();
		shader_Surface = Shader.Find("Suimono2/surface");
		shader_Scale = Shader.Find("Suimono2/surface_scale");
		shader_Under = Shader.Find("Suimono2/surface_under");
		tempMaterial = new Material(suimonoModuleLibrary.materialSurface);
		if (suimonoObject != null)
		{
			tempMaterial.shader = shader_Surface;
			suimonoObject.GetComponent<Renderer>().sharedMaterial = tempMaterial;
			surfaceRenderer = base.transform.Find("Suimono_Object").gameObject.GetComponent<Renderer>();
		}
		ReloadData();
	}

	private void OnEnable()
	{
		if (Application.isPlaying)
		{
			if (moduleObject == null)
			{
				moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
			}
			if (moduleObject != null)
			{
				moduleObject.RegisterSurface(this);
			}
		}
	}

	private void OnDisable()
	{
		if (Application.isPlaying)
		{
			if (moduleObject == null)
			{
				moduleObject = (SuimonoModule)UnityEngine.Object.FindObjectOfType(typeof(SuimonoModule));
			}
			if (moduleObject != null)
			{
				moduleObject.DeregisterSurface(this);
			}
		}
	}

	private void ReloadData()
	{
		reloadData = false;
	}

	private void LateUpdate()
	{
		if (!(moduleObject != null))
		{
			return;
		}
		suimonoVersionNumber = moduleObject.suimonoVersionNumber;
		if (reloadData)
		{
			ReloadData();
		}
		systemLocalTime = moduleObject.systemTime;
		localTime = systemLocalTime * flowSpeed * (1f / waveScale);
		flow_dir = SuimonoConvertAngleToVector(flowDirection);
		if (surfaceRenderer != null)
		{
			surfaceRenderer.sharedMaterial.SetVector("_suimono_Dir", new Vector4(flow_dir.x, 1f, flow_dir.y, localTime));
		}
		if (moduleObject.autoSetLayers)
		{
			base.gameObject.layer = moduleObject.layerWaterNum;
			if (suimonoObject != null)
			{
				suimonoObject.layer = moduleObject.layerWaterNum;
			}
			if (scaleObject != null)
			{
				scaleObject.layer = moduleObject.layerWaterNum;
			}
			if (surfaceReflections != null)
			{
				surfaceReflections.gameObject.layer = moduleObject.layerWaterNum;
			}
		}
		if (underwaterDepth < 0.1f)
		{
			underwaterDepth = 0.1f;
		}
		if (!enableCustomMesh)
		{
			base.transform.localScale = new Vector3(base.transform.localScale.x, 1f, base.transform.localScale.z);
		}
		if (typeIndex == 0)
		{
			suimonoObject.transform.localScale = new Vector3(suimonoObject.transform.localScale.x, 1f, suimonoObject.transform.localScale.z);
			scaleObject.transform.localScale = new Vector3(scaleObject.transform.localScale.x, 1f, scaleObject.transform.localScale.z);
			surfaceReflections.transform.localScale = new Vector3(surfaceReflections.transform.localScale.x, 1f, surfaceReflections.transform.localScale.z);
		}
		useBeaufortScale = !customWaves;
		if (useBeaufortScale)
		{
			beaufortVal = beaufortScale / 12f;
			turbulenceFactor = Mathf.Clamp(Mathf.Lerp(-0.1f, 2.1f, beaufortVal) * 0.9f, 0f, 0.75f);
			waveHeight = Mathf.Clamp(Mathf.Lerp(0f, 5f, beaufortVal), 0f, 0.65f);
			waveHeight -= Mathf.Clamp(Mathf.Lerp(-1.5f, 1f, beaufortVal), 0f, 0.5f);
			lgWaveHeight = Mathf.Clamp(Mathf.Lerp(-0.2f, 1.1f, beaufortVal) * 2.8f, 0f, 3f);
			if (typeIndex == 0)
			{
				waveScale = 0.5f;
				lgWaveScale = 1f / 32f;
			}
		}
		if (presetStartTransition)
		{
			if (presetTimer >= 1f)
			{
				presetStartTransition = false;
			}
			else
			{
				presetTimer += Time.deltaTime / presetTransitionTime;
				PresetLoadBuild(currentPresetFolder, currentPresetName);
			}
		}
		if (typeIndex == 0)
		{
			useLodIndex = 0;
		}
		if (typeIndex == 1)
		{
			useLodIndex = lodIndex;
		}
		if (typeIndex == 2)
		{
			useLodIndex = 3;
		}
		if (typeIndex == 0)
		{
			enableCustomMesh = false;
		}
		if (!enableCustomMesh)
		{
			if ((bool)suimonoModuleLibrary && !meshWasSet)
			{
				if ((bool)suimonoModuleLibrary.texNormalC && surfaceMesh != null)
				{
					surfaceMesh.mesh = suimonoModuleLibrary.meshLevel[useLodIndex];
				}
				if ((bool)suimonoModuleLibrary.texNormalC && surfaceCollider != null)
				{
					surfaceCollider.sharedMesh = suimonoModuleLibrary.meshLevel[3];
				}
				meshWasSet = true;
			}
			else
			{
				meshWasSet = false;
			}
		}
		else if (customMesh != null)
		{
			if (surfaceMesh != null)
			{
				surfaceMesh.mesh = customMesh;
			}
			if (surfaceCollider != null)
			{
				surfaceCollider.sharedMesh = customMesh;
			}
		}
		else
		{
			if ((bool)suimonoModuleLibrary.texNormalC && surfaceMesh != null)
			{
				surfaceMesh.mesh = suimonoModuleLibrary.meshLevel[useLodIndex];
			}
			if ((bool)suimonoModuleLibrary.texNormalC && surfaceCollider != null)
			{
				surfaceCollider.sharedMesh = suimonoModuleLibrary.meshLevel[3];
			}
			meshWasSet = false;
		}
		if (useLodIndex == 3)
		{
			useHeightProjection = 0f;
			useEnableTess = false;
		}
		else
		{
			useHeightProjection = heightProjection;
			useEnableTess = enableTess;
		}
		Shader.SetGlobalFloat("cmScaleX", enableCustomMesh ? cmScaleX : 1f);
		Shader.SetGlobalFloat("cmScaleY", enableCustomMesh ? cmScaleY : 1f);
		if ((bool)suimonoModuleLibrary.texNormalC && scaleMesh != null)
		{
			scaleMesh.mesh = suimonoModuleLibrary.meshLevel[1];
		}
		if (enableCustomTextures)
		{
			if (customTexNormal1 != null)
			{
				useTexNormal1 = customTexNormal1;
			}
			else
			{
				useTexNormal1 = suimonoModuleLibrary.texNormalC;
			}
			if (customTexNormal2 != null)
			{
				useTexNormal2 = customTexNormal2;
			}
			else
			{
				useTexNormal2 = suimonoModuleLibrary.texNormalT;
			}
			if (customTexNormal3 != null)
			{
				useTexNormal3 = customTexNormal3;
			}
			else
			{
				useTexNormal3 = suimonoModuleLibrary.texNormalR;
			}
		}
		else if (suimonoModuleLibrary != null)
		{
			useTexNormal1 = suimonoModuleLibrary.texNormalC;
			useTexNormal2 = suimonoModuleLibrary.texNormalT;
			useTexNormal3 = suimonoModuleLibrary.texNormalR;
		}
		if (suimonoModuleLibrary != null && surfaceRenderer != null)
		{
			surfaceRenderer.sharedMaterial.SetTexture("_MaskTex", suimonoModuleLibrary.texMask);
		}
		if (surfaceReflections != null && moduleObject != null)
		{
			useEnableReflections = enableReflections;
			useEnableDynamicReflections = enableDynamicReflections;
			if (!moduleObject.enableReflections)
			{
				useEnableReflections = false;
			}
			if (!moduleObject.enableDynamicReflections)
			{
				useEnableDynamicReflections = false;
			}
			if (!useEnableReflections || !moduleObject.enableReflections)
			{
				useReflections = false;
				surfaceReflections.gameObject.SetActive(value: false);
			}
			else if (!useEnableDynamicReflections || !moduleObject.enableDynamicReflections)
			{
				surfaceReflections.gameObject.SetActive(value: false);
			}
			else
			{
				surfaceReflections.gameObject.SetActive(value: true);
				useReflections = true;
				reflectLayer &= ~(1 << moduleObject.layerWaterNum);
				reflectLayer &= ~(1 << moduleObject.layerDepthNum);
				reflectLayer &= ~(1 << moduleObject.layerScreenFXNum);
				surfaceReflections.setLayers = reflectLayer;
				surfaceReflections.resolution = Convert.ToInt32(resolutions[reflectResolution]);
				if (moduleObject.setCameraComponent != null)
				{
					reflectionDistance = moduleObject.setCameraComponent.farClipPlane + 200f;
				}
				surfaceReflections.reflectionDistance = reflectionDistance;
				surfaceReflBlur.blurAmt = reflectBlur;
				if (useShader == shader_Under)
				{
					surfaceReflections.isUnderwater = true;
				}
				else
				{
					surfaceReflections.isUnderwater = false;
				}
			}
		}
		if (surfaceRenderer != null)
		{
			if (Application.isPlaying && useShader != null && currUseShader != useShader)
			{
				currUseShader = useShader;
				surfaceRenderer.sharedMaterial.shader = currUseShader;
			}
			if (!Application.isPlaying)
			{
				surfaceRenderer.sharedMaterial.SetFloat("_isPlaying", 0f);
			}
			else
			{
				surfaceRenderer.sharedMaterial.SetFloat("_isPlaying", 1f);
			}
			surfaceRenderer.sharedMaterial.SetTexture("_NormalTexS", useTexNormal1);
			surfaceRenderer.sharedMaterial.SetTexture("_NormalTexD", useTexNormal2);
			surfaceRenderer.sharedMaterial.SetTexture("_NormalTexR", useTexNormal3);
			surfaceRenderer.sharedMaterial.SetFloat("_beaufortFlag", useBeaufortScale ? 1f : 0f);
			surfaceRenderer.sharedMaterial.SetFloat("_beaufortScale", beaufortVal);
			surfaceRenderer.sharedMaterial.SetFloat("_turbulenceFactor", turbulenceFactor);
			float num = (enableCustomMesh ? cmScaleX : 1f);
			float num2 = (enableCustomMesh ? cmScaleY : 1f);
			surfaceRenderer.sharedMaterial.SetTextureScale("_NormalTexS", new Vector2(suimonoObject.transform.localScale.x / waveScale * base.transform.localScale.x * num, suimonoObject.transform.localScale.z / waveScale * base.transform.localScale.z * num2));
			surfaceRenderer.sharedMaterial.SetVector("_scaleUVs", new Vector4(suimonoObject.transform.localScale.x / waveScale * num, suimonoObject.transform.localScale.z / waveScale * num2, 0f, 0f));
			surfaceRenderer.sharedMaterial.SetFloat("_lgWaveScale", lgWaveScale);
			surfaceRenderer.sharedMaterial.SetFloat("_lgWaveHeight", lgWaveHeight);
			if (typeIndex == 0)
			{
				surfaceRenderer.sharedMaterial.SetFloat("_tessScale", suimonoObject.transform.localScale.x);
			}
			else
			{
				surfaceRenderer.sharedMaterial.SetFloat("_tessScale", base.transform.localScale.x);
			}
			surfaceRenderer.sharedMaterial.SetFloat("_Tess", Mathf.Lerp(0.001f, waveTessAmt, useEnableTess ? 1f : 0f));
			surfaceRenderer.sharedMaterial.SetFloat("_minDist", Mathf.Lerp(-180f, 0f, waveTessMin));
			surfaceRenderer.sharedMaterial.SetFloat("_maxDist", Mathf.Lerp(20f, 500f, waveTessSpread));
			surfaceRenderer.sharedMaterial.SetFloat("_unity_fogstart", RenderSettings.fogStartDistance);
			surfaceRenderer.sharedMaterial.SetFloat("_unity_fogend", RenderSettings.fogEndDistance);
			surfaceRenderer.sharedMaterial.SetFloat("_causticsFlag", enableCausticFX ? 1f : 0f);
			surfaceRenderer.sharedMaterial.SetFloat("_CausticsFade", Mathf.Lerp(1f, 500f, causticsFade));
			surfaceRenderer.sharedMaterial.SetColor("_CausticsColor", causticsColor);
			surfaceRenderer.sharedMaterial.SetFloat("_aberrationScale", aberrationScale);
			surfaceRenderer.sharedMaterial.SetFloat("_enableFoam", enableFoam ? 1f : 0f);
			surfaceRenderer.sharedMaterial.SetFloat("_EdgeFoamFade", Mathf.Lerp(1500f, 5f, edgeFoamAmt));
			surfaceRenderer.sharedMaterial.SetFloat("_HeightFoamAmt", heightFoamAmt);
			surfaceRenderer.sharedMaterial.SetFloat("_HeightFoamHeight", hFoamHeight);
			surfaceRenderer.sharedMaterial.SetFloat("_HeightFoamSpread", hFoamSpread);
			surfaceRenderer.sharedMaterial.SetFloat("_foamSpeed", foamSpeed);
			surfaceRenderer.sharedMaterial.SetTextureScale("_FoamTex", foamScale * new Vector2(suimonoObject.transform.localScale.x / foamScale * base.transform.localScale.x * num, suimonoObject.transform.localScale.z / foamScale * base.transform.localScale.z * num2));
			surfaceRenderer.sharedMaterial.SetFloat("_foamScale", Mathf.Lerp(160f, 1f, foamScale));
			surfaceRenderer.sharedMaterial.SetColor("_FoamColor", foamColor);
			surfaceRenderer.sharedMaterial.SetFloat("_ShallowFoamAmt", shallowFoamAmt);
			surfaceRenderer.sharedMaterial.SetFloat("_heightScaleFac", 1f / base.transform.localScale.y);
			surfaceRenderer.sharedMaterial.SetFloat("_heightProjection", useHeightProjection);
			surfaceRenderer.sharedMaterial.SetFloat("_heightScale", waveHeight);
			surfaceRenderer.sharedMaterial.SetFloat("_RefractStrength", refractStrength);
			surfaceRenderer.sharedMaterial.SetFloat("_ReflectStrength", reflectProjection);
			surfaceRenderer.sharedMaterial.SetFloat("_shorelineHeight", shorelineHeight);
			surfaceRenderer.sharedMaterial.SetFloat("_shorelineFrequency", shorelineFreq);
			surfaceRenderer.sharedMaterial.SetFloat("_shorelineScale", 0.1f);
			surfaceRenderer.sharedMaterial.SetFloat("_shorelineSpeed", shorelineSpeed);
			surfaceRenderer.sharedMaterial.SetFloat("_shorelineNorm", shorelineNorm);
			surfaceRenderer.sharedMaterial.SetFloat("_specularPower", specularPower);
			surfaceRenderer.sharedMaterial.SetFloat("_roughness", roughness);
			surfaceRenderer.sharedMaterial.SetFloat("_roughness2", roughness2);
			surfaceRenderer.sharedMaterial.SetFloat("_reflecTerm", reflectTerm);
			surfaceRenderer.sharedMaterial.SetFloat("_reflecSharp", Mathf.Lerp(0f, -1.5f, reflectSharpen));
			surfaceRenderer.sharedMaterial.SetFloat("_overallBrightness", overallBright);
			surfaceRenderer.sharedMaterial.SetFloat("_overallTransparency", overallTransparency);
			surfaceRenderer.sharedMaterial.SetFloat("_DepthFade", Mathf.Lerp(0.1f, 200f, depthAmt));
			surfaceRenderer.sharedMaterial.SetFloat("_ShallowFade", Mathf.Lerp(0.1f, 800f, shallowAmt));
			surfaceRenderer.sharedMaterial.SetColor("_depthColor", depthColor);
			surfaceRenderer.sharedMaterial.SetColor("_shallowColor", shallowColor);
			surfaceRenderer.sharedMaterial.SetFloat("_EdgeFade", Mathf.Lerp(10f, 1000f, edgeAmt));
			surfaceRenderer.sharedMaterial.SetColor("_SpecularColor", specularColor);
			surfaceRenderer.sharedMaterial.SetColor("_SSSColor", sssColor);
			surfaceRenderer.sharedMaterial.SetColor("_BlendColor", blendColor);
			surfaceRenderer.sharedMaterial.SetColor("_OverlayColor", overlayColor);
			surfaceRenderer.sharedMaterial.SetColor("_UnderwaterColor", underwaterColor);
			surfaceRenderer.sharedMaterial.SetFloat("_reflectFlag", useEnableReflections ? 1f : 0f);
			surfaceRenderer.sharedMaterial.SetFloat("_reflectDynamicFlag", useEnableDynamicReflections ? 1f : 0f);
			surfaceRenderer.sharedMaterial.SetFloat("_reflectFallback", reflectFallback);
			surfaceRenderer.sharedMaterial.SetColor("_reflectFallbackColor", customRefColor);
			surfaceRenderer.sharedMaterial.SetColor("_ReflectionColor", reflectionColor);
			skybox = RenderSettings.skybox;
			if (skybox != null && skybox.HasProperty("_Tex") && skybox.HasProperty("_Tint") && skybox.HasProperty("_Exposure") && skybox.HasProperty("_Rotation"))
			{
				surfaceRenderer.sharedMaterial.SetTexture("_SkyCubemap", skybox.GetTexture("_Tex"));
				surfaceRenderer.sharedMaterial.SetColor("_SkyTint", skybox.GetColor("_Tint"));
				surfaceRenderer.sharedMaterial.SetFloat("_SkyExposure", skybox.GetFloat("_Exposure"));
				surfaceRenderer.sharedMaterial.SetFloat("_SkyRotation", skybox.GetFloat("_Rotation"));
			}
			if (customRefCubemap != null)
			{
				surfaceRenderer.sharedMaterial.SetTexture("_CubeTex", customRefCubemap);
			}
			surfaceRenderer.sharedMaterial.SetFloat("_cameraDistance", cameraDistance);
			Vector2 value = new Vector2(1f, 10f);
			surfaceRenderer.sharedMaterial.SetTextureScale("_WaveTex", value);
			scaleRenderer.sharedMaterial.SetTextureScale("_WaveTex", value);
			suimono_refl_off = 0f;
			suimono_refl_sky = 0f;
			suimono_refl_cube = 0f;
			suimono_refl_color = 0f;
			if (reflectFallback == 0)
			{
				suimono_refl_off = 1f;
			}
			if (reflectFallback == 1)
			{
				suimono_refl_sky = 1f;
			}
			if (reflectFallback == 2)
			{
				suimono_refl_cube = 1f;
			}
			if (reflectFallback == 3)
			{
				suimono_refl_color = 1f;
			}
			if (surfaceRenderer != null)
			{
				surfaceRenderer.sharedMaterial.SetFloat("suimono_tess_on", enableTess ? 1f : 0f);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_trans_on", moduleObject.enableTransparency ? 1f : 0f);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_caust_on", moduleObject.enableCaustics ? 1f : 0f);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_dynrefl_on", useDynReflections ? 1f : 0f);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_refl_off", suimono_refl_off);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_refl_sky", suimono_refl_sky);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_refl_cube", suimono_refl_cube);
				surfaceRenderer.sharedMaterial.SetFloat("suimono_refl_color", suimono_refl_color);
			}
			if (scaleRenderer != null && typeIndex == 0)
			{
				scaleRenderer.sharedMaterial.SetFloat("suimono_tess_on", enableTess ? 1f : 0f);
				scaleRenderer.sharedMaterial.SetFloat("suimono_trans_on", moduleObject.enableTransparency ? 1f : 0f);
				scaleRenderer.sharedMaterial.SetFloat("suimono_caust_on", moduleObject.enableCaustics ? 1f : 0f);
				scaleRenderer.sharedMaterial.SetFloat("suimono_dynrefl_on", useDynReflections ? 1f : 0f);
				scaleRenderer.sharedMaterial.SetFloat("suimono_refl_off", suimono_refl_off);
				scaleRenderer.sharedMaterial.SetFloat("suimono_refl_sky", suimono_refl_sky);
				scaleRenderer.sharedMaterial.SetFloat("suimono_refl_cube", suimono_refl_cube);
				scaleRenderer.sharedMaterial.SetFloat("suimono_refl_color", suimono_refl_color);
			}
		}
		if (typeIndex == 0 && Application.isPlaying)
		{
			if (moduleObject.isUnderwater)
			{
				if (scaleRenderer != null)
				{
					scaleRenderer.enabled = false;
				}
				if (scaleCollider != null)
				{
					scaleCollider.enabled = false;
				}
			}
			else
			{
				if (scaleRenderer != null)
				{
					scaleRenderer.enabled = true;
				}
				if (scaleCollider != null)
				{
					scaleCollider.enabled = true;
				}
			}
		}
		else
		{
			if (scaleRenderer != null)
			{
				scaleRenderer.enabled = false;
			}
			if (scaleCollider != null)
			{
				scaleCollider.enabled = false;
			}
		}
		if (Application.isPlaying)
		{
			if (typeIndex == 0)
			{
				base.transform.eulerAngles = new Vector3(base.transform.eulerAngles.x, 0f, base.transform.eulerAngles.z);
				if (oceanScale < 1f)
				{
					oceanScale = 1f;
				}
				offamt = 0.4027f * oceanScale / waveScale;
				spacer = suimonoObject.transform.localScale.x * 4f;
				newPos = new Vector3(moduleObject.setCamera.position.x, suimonoObject.transform.position.y, moduleObject.setCamera.position.z);
				if (Mathf.Abs(suimonoObject.transform.position.x - newPos.x) > spacer)
				{
					if (suimonoObject.transform.position.x > newPos.x)
					{
						setScaleX -= offamt;
					}
					if (suimonoObject.transform.position.x < newPos.x)
					{
						setScaleX += offamt;
					}
					suimonoObject.transform.position = new Vector3(newPos.x, suimonoObject.transform.position.y, suimonoObject.transform.position.z);
					scaleObject.transform.position = new Vector3(newPos.x, scaleObject.transform.position.y, scaleObject.transform.position.z);
				}
				if (Mathf.Abs(suimonoObject.transform.position.z - newPos.z) > spacer)
				{
					if (suimonoObject.transform.position.z > newPos.z)
					{
						setScaleZ -= offamt;
					}
					if (suimonoObject.transform.position.z < newPos.z)
					{
						setScaleZ += offamt;
					}
					suimonoObject.transform.position = new Vector3(suimonoObject.transform.position.x, suimonoObject.transform.position.y, newPos.z);
					scaleObject.transform.position = new Vector3(scaleObject.transform.position.x, scaleObject.transform.position.y, newPos.z);
				}
				if (currentPosition != suimonoObject.transform.position)
				{
					currentPosition = suimonoObject.transform.position;
					savePos = new Vector2(setScaleX, setScaleZ);
				}
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_uvx", 0f - savePos.x);
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_uvy", 0f - savePos.y);
				scaleObject.transform.localPosition = new Vector3(scaleObject.transform.localPosition.x, -0.1f, scaleObject.transform.localPosition.z);
				if (scaleRenderer != null)
				{
					setScale = Mathf.Ceil(moduleObject.setCameraComponent.farClipPlane / 20f) * suimonoObject.transform.localScale.x;
					scaleObject.transform.localScale = new Vector3(setScale * 0.5f, 1f, setScale * 0.5f);
					oceanUseScale = 4f;
					base.transform.localScale = new Vector3(1f, 1f, 1f);
					suimonoObject.transform.localScale = new Vector3(oceanUseScale * oceanScale, 1f, oceanUseScale * oceanScale);
					if (scaleRenderer != null)
					{
						scaleRenderer.material.CopyPropertiesFromMaterial(tempMaterial);
						scaleRenderer.sharedMaterial.SetFloat("_suimono_uvx", 0f - savePos.x);
						scaleRenderer.sharedMaterial.SetFloat("_suimono_uvy", 0f - savePos.y);
						setSc = scaleRenderer.sharedMaterial.GetTextureScale("_NormalTexS");
						useSc = scaleObject.transform.localScale.x / suimonoObject.transform.localScale.x;
						scaleRenderer.sharedMaterial.SetTextureScale("_NormalTexS", setSc * useSc);
						scaleRenderer.sharedMaterial.SetTextureScale("_FoamTex", setSc * useSc);
					}
				}
			}
			else
			{
				savePos = new Vector3(0f, 0f, 0f);
				suimonoObject.transform.localScale = new Vector3(1f, 1f, 1f);
				scaleObject.transform.localScale = new Vector3(1f, 1f, 1f);
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_uvx", 0f);
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_uvy", 0f);
			}
		}
		if (surfaceRenderer != null)
		{
			if (showDepthMask)
			{
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_DebugDepthMask", 1f);
			}
			else
			{
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_DebugDepthMask", 0f);
			}
			if (showWorldMask)
			{
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_DebugWorldNormalMask", 1f);
			}
			else
			{
				surfaceRenderer.sharedMaterial.SetFloat("_suimono_DebugWorldNormalMask", 0f);
			}
		}
	}

	public Vector2 SuimonoConvertAngleToVector(float convertAngle)
	{
		flow_dir = new Vector2(0f, 0f);
		tempAngle = new Vector3(0f, 0f, 0f);
		if (convertAngle <= 180f)
		{
			tempAngle = Vector3.Slerp(Vector3.forward, -Vector3.forward, convertAngle / 180f);
			flow_dir = new Vector2(tempAngle.x, tempAngle.z);
		}
		if (convertAngle > 180f)
		{
			tempAngle = Vector3.Slerp(-Vector3.forward, Vector3.forward, (convertAngle - 180f) / 180f);
			flow_dir = new Vector2(0f - tempAngle.x, tempAngle.z);
		}
		return flow_dir;
	}

	public void SuimonoSetPreset(string fName, string pName)
	{
		presetTimer = 1f;
		SetTemporaryPresetData();
		PresetLoadBuild(fName, pName);
	}

	public void SuimonoSavePreset(string fName, string pName)
	{
		int num = -1;
		int num2 = -1;
		num = PresetGetNum("folder", fName);
		num2 = PresetGetNum("preset", pName);
		if (num >= 0 && num2 >= 0)
		{
			PresetSave(num, num2);
			return;
		}
		Debug.Log("The Preset " + pName + " in folder " + fName + " cannot be found!");
	}

	private void PresetInit()
	{
		presetTimer = 1f;
		presetDirsArr = new List<string>();
		dirInfo = new DirectoryInfo(dir + "/").GetFiles("SUIMONO_PRESETS_*");
		if (new DirectoryInfo(dir + "/") != null)
		{
			for (d = 0; d < dirInfo.Length; d++)
			{
				presetDirsArr.Add(dirInfo[d].ToString());
			}
		}
		presetDirs = new List<string>(new string[presetDirsArr.Count]);
		for (dn = 0; dn < presetDirsArr.Count; dn++)
		{
			presetDirs[dn] = presetDirsArr[dn].ToString();
			presetDirs[dn] = presetDirs[dn].Remove(0, dir.Length);
			presetDirs[dn] = presetDirs[dn].Replace("SUIMONO_PRESETS_", "");
			presetDirs[dn] = presetDirs[dn].Replace(".meta", "");
		}
		presetFilesArr = new List<string>();
		pdir = dir + "/SUIMONO_PRESETS_" + presetDirs[presetFileIndex];
		fileInfo = new DirectoryInfo(pdir).GetFiles("SUIMONO_PRESET_*");
		if (new DirectoryInfo(pdir) != null)
		{
			for (f = 0; f < fileInfo.Length; f++)
			{
				presetFilesArr.Add(fileInfo[f].ToString());
			}
		}
		px = 0;
		for (nx = 0; nx < presetFilesArr.Count; nx++)
		{
			if (!presetFilesArr[nx].ToString().Contains(".meta"))
			{
				px++;
			}
		}
		presetFiles = new string[px];
		ax = 0;
		for (n = 0; n < presetFilesArr.Count; n++)
		{
			if (!presetFilesArr[n].ToString().Contains(".meta"))
			{
				presetFiles[ax] = presetFilesArr[n].ToString();
				presetFiles[ax] = presetFiles[ax].Remove(0, pdir.Length);
				presetFiles[ax] = presetFiles[ax].Replace("SUIMONO_PRESET_", "");
				presetFiles[ax] = presetFiles[ax].Replace(".txt", "");
				ax++;
			}
		}
	}

	public int PresetGetNum(string mode, string pName)
	{
		int result = -1;
		int num = -1;
		int num2 = -1;
		if (mode == "folder")
		{
			tempPresetDirsArr = new List<string>();
			dirInfo = new DirectoryInfo(dir + "/").GetFiles("SUIMONO_PRESETS_*");
			if (new DirectoryInfo(dir + "/") != null)
			{
				for (d = 0; d < dirInfo.Length; d++)
				{
					tempPresetDirsArr.Add(dirInfo[d].ToString());
				}
			}
			tempPresetDirs = new string[tempPresetDirsArr.Count];
			for (dn = 0; dn < tempPresetDirsArr.Count; dn++)
			{
				tempPresetDirs[dn] = tempPresetDirsArr[dn].ToString();
				tempPresetDirs[dn] = tempPresetDirs[dn].Remove(0, dir.Length);
				tempPresetDirs[dn] = tempPresetDirs[dn].Replace("SUIMONO_PRESETS_", "");
				tempPresetDirs[dn] = tempPresetDirs[dn].Replace(".meta", "");
				if (tempPresetDirs[dn] == pName)
				{
					num = dn;
				}
			}
			result = num;
		}
		if (mode == "preset")
		{
			tempPresetFilesArr = new List<string>();
			pdir = dir + "/SUIMONO_PRESETS_" + presetDirs[presetFileIndex];
			fileInfo = new DirectoryInfo(pdir).GetFiles("SUIMONO_PRESET_*");
			if (new DirectoryInfo(pdir) != null)
			{
				for (f = 0; f < fileInfo.Length; f++)
				{
					tempPresetFilesArr.Add(fileInfo[f].ToString());
				}
			}
			px = 0;
			for (nx = 0; nx < tempPresetFilesArr.Count; nx++)
			{
				if (!tempPresetFilesArr[nx].ToString().Contains(".meta"))
				{
					px++;
				}
			}
			tempPresetFiles = new string[px];
			ax = 0;
			for (n = 0; n < tempPresetFilesArr.Count; n++)
			{
				if (!tempPresetFilesArr[n].ToString().Contains(".meta"))
				{
					tempPresetFiles[ax] = tempPresetFilesArr[n].ToString();
					tempPresetFiles[ax] = tempPresetFiles[ax].Remove(0, pdir.Length);
					tempPresetFiles[ax] = tempPresetFiles[ax].Replace("SUIMONO_PRESET_", "");
					tempPresetFiles[ax] = tempPresetFiles[ax].Replace(".txt", "");
					if (tempPresetFiles[ax] == pName)
					{
						num2 = ax;
					}
					ax++;
				}
			}
			result = num2;
		}
		return result;
	}

	public void PresetRename(int ppos, string newName)
	{
	}

	public void PresetAdd()
	{
	}

	public void PresetDelete(int fpos, int ppos)
	{
	}

	public void PresetSave(int fpos, int ppos)
	{
	}

	public void PresetLoad(int ppos)
	{
		if (presetIndex < 0)
		{
			return;
		}
		pdir = dir + "/SUIMONO_PRESETS_" + presetDirs[presetFileIndex];
		sr = new StreamReader(pdir + "/SUIMONO_PRESET_" + presetFiles[ppos] + ".txt");
		presetDataString = sr.ReadToEnd();
		sr.Close();
		presetDataArray = presetDataString.Split("\n"[0]);
		for (dx = 0; dx < presetDataArray.Length; dx++)
		{
			if (presetDataArray[dx] != "" && presetDataArray[dx] != "\n")
			{
				pFrom = presetDataArray[dx].IndexOf("<") + "<".Length;
				pTo = presetDataArray[dx].LastIndexOf(">");
				key = presetDataArray[dx].Substring(pFrom, pTo - pFrom);
				pFrom = presetDataArray[dx].IndexOf("(") + "(".Length;
				pTo = presetDataArray[dx].LastIndexOf(")");
				dat = presetDataArray[dx].Substring(pFrom, pTo - pFrom);
				SetTemporaryPresetData();
				PresetDecode(key, dat);
			}
		}
	}

	private void PresetLoadBuild(string fName, string pName)
	{
		datFile = Resources.Load("SUIMONO_PRESETS_" + fName + "/SUIMONO_PRESET_" + pName) as TextAsset;
		presetDataString = datFile.text;
		presetDataArray = presetDataString.Split("\n"[0]);
		for (dx = 0; dx < presetDataArray.Length; dx++)
		{
			if (presetDataArray[dx] != "" && presetDataArray[dx] != "\n")
			{
				pFrom = presetDataArray[dx].IndexOf("<") + "<".Length;
				pTo = presetDataArray[dx].LastIndexOf(">");
				key = presetDataArray[dx].Substring(pFrom, pTo - pFrom);
				pFrom = presetDataArray[dx].IndexOf("(") + "(".Length;
				pTo = presetDataArray[dx].LastIndexOf(")");
				dat = presetDataArray[dx].Substring(pFrom, pTo - pFrom);
				PresetDecode(key, dat);
			}
		}
	}

	private void PresetDecode(string key, string dat)
	{
		dataS = dat.Split(","[0]);
		if (presetTimer > 1f)
		{
			presetTimer = 1f;
		}
		if (key == "color_depth")
		{
			depthColor = Color.Lerp(temp_depthColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_shallow")
		{
			shallowColor = Color.Lerp(temp_shallowColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_blend")
		{
			blendColor = Color.Lerp(temp_blendColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_overlay")
		{
			overlayColor = Color.Lerp(temp_overlayColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_caustics")
		{
			causticsColor = Color.Lerp(temp_causticsColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_reflection")
		{
			reflectionColor = Color.Lerp(temp_reflectionColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_specular")
		{
			specularColor = Color.Lerp(temp_specularColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_sss")
		{
			sssColor = Color.Lerp(temp_sssColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_foam")
		{
			foamColor = Color.Lerp(temp_foamColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "color_underwater")
		{
			underwaterColor = Color.Lerp(temp_underwaterColor, DecodeColor(dataS), presetTimer);
		}
		if (key == "data_beaufort")
		{
			beaufortScale = Mathf.Lerp(temp_beaufortScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_flowdir")
		{
			flowDirection = Mathf.Lerp(temp_flowDirection, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_flowspeed")
		{
			flowSpeed = Mathf.Lerp(temp_flowSpeed, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_wavescale")
		{
			waveScale = Mathf.Lerp(temp_waveScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_waveheight")
		{
			waveHeight = Mathf.Lerp(temp_waveHeight, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_heightprojection")
		{
			heightProjection = Mathf.Lerp(temp_heightProjection, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_turbulence")
		{
			turbulenceFactor = Mathf.Lerp(temp_turbulenceFactor, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_lgwaveheight")
		{
			lgWaveHeight = Mathf.Lerp(temp_lgWaveHeight, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_lgwavescale")
		{
			lgWaveScale = Mathf.Lerp(temp_lgWaveScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shorelineheight")
		{
			shorelineHeight = Mathf.Lerp(temp_shorelineHeight, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shorelinefreq")
		{
			shorelineFreq = Mathf.Lerp(temp_shorelineFreq, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shorelinescale")
		{
			shorelineScale = Mathf.Lerp(temp_shorelineScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shorelinespeed")
		{
			shorelineSpeed = Mathf.Lerp(temp_shorelineSpeed, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shorelinenorm")
		{
			shorelineNorm = Mathf.Lerp(temp_shorelineNorm, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_overallbright")
		{
			overallBright = Mathf.Lerp(temp_overallBright, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_overalltransparency")
		{
			overallTransparency = Mathf.Lerp(temp_overallTransparency, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_edgeamt")
		{
			edgeAmt = Mathf.Lerp(temp_edgeAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_depthamt")
		{
			depthAmt = Mathf.Lerp(temp_depthAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shallowamt")
		{
			shallowAmt = Mathf.Lerp(temp_shallowAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_refractstrength")
		{
			refractStrength = Mathf.Lerp(temp_refractStrength, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_aberrationscale")
		{
			aberrationScale = Mathf.Lerp(temp_aberrationScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_causticsfade")
		{
			causticsFade = Mathf.Lerp(temp_causticsFade, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_reflectprojection")
		{
			reflectProjection = Mathf.Lerp(temp_reflectProjection, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_reflectblur")
		{
			reflectBlur = Mathf.Lerp(temp_reflectBlur, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_reflectterm")
		{
			reflectTerm = Mathf.Lerp(temp_reflectTerm, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_reflectsharpen")
		{
			reflectSharpen = Mathf.Lerp(temp_reflectSharpen, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_roughness")
		{
			roughness = Mathf.Lerp(temp_roughness, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_roughness2")
		{
			roughness2 = Mathf.Lerp(temp_roughness2, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_foamscale")
		{
			foamScale = Mathf.Lerp(temp_foamScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_foamspeed")
		{
			foamSpeed = Mathf.Lerp(temp_foamSpeed, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_edgefoamamt")
		{
			edgeFoamAmt = Mathf.Lerp(temp_edgeFoamAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_shallowfoamamt")
		{
			shallowFoamAmt = Mathf.Lerp(temp_shallowFoamAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_heightfoamamt")
		{
			heightFoamAmt = Mathf.Lerp(temp_heightFoamAmt, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_hfoamheight")
		{
			hFoamHeight = Mathf.Lerp(temp_hFoamHeight, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_hfoamspread")
		{
			hFoamSpread = Mathf.Lerp(temp_hFoamSpread, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underlightfactor")
		{
			underLightFactor = Mathf.Lerp(temp_underLightFactor, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underrefractionamount")
		{
			underRefractionAmount = Mathf.Lerp(temp_underRefractionAmount, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underrefractionscale")
		{
			underRefractionScale = Mathf.Lerp(temp_underRefractionScale, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underrefractionspeed")
		{
			underRefractionSpeed = Mathf.Lerp(temp_underRefractionSpeed, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underbluramount")
		{
			underBlurAmount = Mathf.Lerp(temp_underBlurAmount, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underwaterfogdist")
		{
			underwaterFogDist = Mathf.Lerp(temp_underwaterFogDist, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underwaterfogspread")
		{
			underwaterFogSpread = Mathf.Lerp(temp_underwaterFogSpread, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_underDarkRange")
		{
			underDarkRange = Mathf.Lerp(temp_underDarkRange, DecodeFloat(dataS), presetTimer);
		}
		if (key == "data_customwaves")
		{
			customWaves = DecodeBool(dataS);
		}
		if (key == "data_enablefoam")
		{
			enableFoam = DecodeBool(dataS);
		}
		if (key == "data_enableunderdebris")
		{
			enableUnderDebris = DecodeBool(dataS);
		}
	}

	public Color DecodeColor(string[] data)
	{
		return new Color(DecodeSingleFloat(data[0]), DecodeSingleFloat(data[1]), DecodeSingleFloat(data[2]), DecodeSingleFloat(data[3]));
	}

	public float DecodeFloat(string[] data)
	{
		return DecodeSingleFloat(data[0]);
	}

	public int DecodeInt(string[] data)
	{
		return int.Parse(data[0]);
	}

	public bool DecodeBool(string[] data)
	{
		retVal = false;
		if (data[0] == "True")
		{
			retVal = true;
		}
		return retVal;
	}

	public float DecodeSingleFloat(string data)
	{
		return float.Parse(data, NumberStyles.Float, CultureInfo.InvariantCulture);
	}

	public string PresetEncode(string key)
	{
		retData = "";
		if (key == "color_depth")
		{
			retData = EncodeColor(depthColor);
		}
		if (key == "color_shallow")
		{
			retData = EncodeColor(shallowColor);
		}
		if (key == "color_blend")
		{
			retData = EncodeColor(blendColor);
		}
		if (key == "color_overlay")
		{
			retData = EncodeColor(overlayColor);
		}
		if (key == "color_caustics")
		{
			retData = EncodeColor(causticsColor);
		}
		if (key == "color_reflection")
		{
			retData = EncodeColor(reflectionColor);
		}
		if (key == "color_specular")
		{
			retData = EncodeColor(specularColor);
		}
		if (key == "color_sss")
		{
			retData = EncodeColor(sssColor);
		}
		if (key == "color_foam")
		{
			retData = EncodeColor(foamColor);
		}
		if (key == "color_underwater")
		{
			retData = EncodeColor(underwaterColor);
		}
		if (key == "data_customwaves")
		{
			retData = "(" + customWaves.ToString().Replace(" ", "") + ")";
		}
		if (key == "data_enableunderdebris")
		{
			retData = "(" + enableUnderDebris.ToString().Replace(" ", "") + ")";
		}
		if (key == "data_enablefoam")
		{
			retData = "(" + enableFoam.ToString().Replace(" ", "") + ")";
		}
		if (key == "data_beaufort")
		{
			retData = "(" + EncodeSingleFloat(beaufortScale) + ")";
		}
		if (key == "data_flowdir")
		{
			retData = "(" + EncodeSingleFloat(flowDirection) + ")";
		}
		if (key == "data_flowspeed")
		{
			retData = "(" + EncodeSingleFloat(flowSpeed) + ")";
		}
		if (key == "data_wavescale")
		{
			retData = "(" + EncodeSingleFloat(waveScale) + ")";
		}
		if (key == "data_waveheight")
		{
			retData = "(" + EncodeSingleFloat(waveHeight) + ")";
		}
		if (key == "data_heightprojection")
		{
			retData = "(" + EncodeSingleFloat(heightProjection) + ")";
		}
		if (key == "data_turbulence")
		{
			retData = "(" + EncodeSingleFloat(turbulenceFactor) + ")";
		}
		if (key == "data_lgwaveheight")
		{
			retData = "(" + EncodeSingleFloat(lgWaveHeight) + ")";
		}
		if (key == "data_lgwavescale")
		{
			retData = "(" + EncodeSingleFloat(lgWaveScale) + ")";
		}
		if (key == "data_shorelineheight")
		{
			retData = "(" + EncodeSingleFloat(shorelineHeight) + ")";
		}
		if (key == "data_shorelinefreq")
		{
			retData = "(" + EncodeSingleFloat(shorelineFreq) + ")";
		}
		if (key == "data_shorelinescale")
		{
			retData = "(" + EncodeSingleFloat(shorelineScale) + ")";
		}
		if (key == "data_shorelinespeed")
		{
			retData = "(" + EncodeSingleFloat(shorelineSpeed) + ")";
		}
		if (key == "data_shorelinenorm")
		{
			retData = "(" + EncodeSingleFloat(shorelineNorm) + ")";
		}
		if (key == "data_overallbright")
		{
			retData = "(" + EncodeSingleFloat(overallBright) + ")";
		}
		if (key == "data_overalltransparency")
		{
			retData = "(" + EncodeSingleFloat(overallTransparency) + ")";
		}
		if (key == "data_edgeamt")
		{
			retData = "(" + EncodeSingleFloat(edgeAmt) + ")";
		}
		if (key == "data_depthamt")
		{
			retData = "(" + EncodeSingleFloat(depthAmt) + ")";
		}
		if (key == "data_shallowamt")
		{
			retData = "(" + EncodeSingleFloat(shallowAmt) + ")";
		}
		if (key == "data_refractstrength")
		{
			retData = "(" + EncodeSingleFloat(refractStrength) + ")";
		}
		if (key == "data_aberrationscale")
		{
			retData = "(" + EncodeSingleFloat(aberrationScale) + ")";
		}
		if (key == "data_causticsfade")
		{
			retData = "(" + EncodeSingleFloat(causticsFade) + ")";
		}
		if (key == "data_reflectprojection")
		{
			retData = "(" + EncodeSingleFloat(reflectProjection) + ")";
		}
		if (key == "data_reflectblur")
		{
			retData = "(" + EncodeSingleFloat(reflectBlur) + ")";
		}
		if (key == "data_reflectterm")
		{
			retData = "(" + EncodeSingleFloat(reflectTerm) + ")";
		}
		if (key == "data_reflectsharpen")
		{
			retData = "(" + EncodeSingleFloat(reflectSharpen) + ")";
		}
		if (key == "data_roughness")
		{
			retData = "(" + EncodeSingleFloat(roughness) + ")";
		}
		if (key == "data_roughness2")
		{
			retData = "(" + EncodeSingleFloat(roughness2) + ")";
		}
		if (key == "data_foamscale")
		{
			retData = "(" + EncodeSingleFloat(foamScale) + ")";
		}
		if (key == "data_foamspeed")
		{
			retData = "(" + EncodeSingleFloat(foamSpeed) + ")";
		}
		if (key == "data_edgefoamamt")
		{
			retData = "(" + EncodeSingleFloat(edgeFoamAmt) + ")";
		}
		if (key == "data_shallowfoamamt")
		{
			retData = "(" + EncodeSingleFloat(shallowFoamAmt) + ")";
		}
		if (key == "data_heightfoamamt")
		{
			retData = "(" + EncodeSingleFloat(heightFoamAmt) + ")";
		}
		if (key == "data_hfoamheight")
		{
			retData = "(" + EncodeSingleFloat(hFoamHeight) + ")";
		}
		if (key == "data_hfoamspread")
		{
			retData = "(" + EncodeSingleFloat(hFoamSpread) + ")";
		}
		if (key == "data_underlightfactor")
		{
			retData = "(" + EncodeSingleFloat(underLightFactor) + ")";
		}
		if (key == "data_underrefractionamount")
		{
			retData = "(" + EncodeSingleFloat(underRefractionAmount) + ")";
		}
		if (key == "data_underrefractionscale")
		{
			retData = "(" + EncodeSingleFloat(underRefractionScale) + ")";
		}
		if (key == "data_underrefractionspeed")
		{
			retData = "(" + EncodeSingleFloat(underRefractionSpeed) + ")";
		}
		if (key == "data_underbluramount")
		{
			retData = "(" + EncodeSingleFloat(underBlurAmount) + ")";
		}
		if (key == "data_underwaterfogdist")
		{
			retData = "(" + EncodeSingleFloat(underwaterFogDist) + ")";
		}
		if (key == "data_underwaterfogspread")
		{
			retData = "(" + EncodeSingleFloat(underwaterFogSpread) + ")";
		}
		if (key == "data_underDarkRange")
		{
			retData = "(" + EncodeSingleFloat(underDarkRange) + ")";
		}
		retData = "<" + key + ">" + retData;
		return retData;
	}

	public string EncodeSingleFloat(float data)
	{
		return data.ToString(CultureInfo.InvariantCulture);
	}

	public string EncodeColor(Color data)
	{
		return $"({EncodeSingleFloat(data.r)},{EncodeSingleFloat(data.g)},{EncodeSingleFloat(data.b)},{EncodeSingleFloat(data.a)})";
	}

	public void SuimonoTransitionPreset(string pName, float transitionTime)
	{
		presetTimer = 0f;
		presetTransitionTime = transitionTime;
		presetStartTransition = true;
		currentPresetFolder = "Built-In Presets";
		currentPresetName = pName;
		SetTemporaryPresetData();
	}

	public void SuimonoTransitionPreset(string fName, string pName, float transitionTime)
	{
		presetTimer = 0f;
		presetTransitionTime = transitionTime;
		presetStartTransition = true;
		currentPresetFolder = fName;
		currentPresetName = pName;
		SetTemporaryPresetData();
	}

	public void SuimonoTransitionPreset(string fName, string pName0, string pName1, float transitionTime)
	{
		SuimonoSetPreset(fName, pName0);
		presetTimer = 0f;
		presetTransitionTime = transitionTime;
		presetStartTransition = true;
		currentPresetFolder = fName;
		currentPresetName = pName1;
		SetTemporaryPresetData();
	}

	public void SuimonoTransitionPreset(string fName0, string pName0, string fName1, string pName1, float transitionTime)
	{
		SuimonoSetPreset(fName0, pName0);
		presetTimer = 0f;
		presetTransitionTime = transitionTime;
		presetStartTransition = true;
		currentPresetFolder = fName1;
		currentPresetName = pName1;
		SetTemporaryPresetData();
	}

	private void SetTemporaryPresetData()
	{
		temp_depthColor = depthColor;
		temp_shallowColor = shallowColor;
		temp_blendColor = blendColor;
		temp_overlayColor = overlayColor;
		temp_causticsColor = causticsColor;
		temp_reflectionColor = reflectionColor;
		temp_specularColor = specularColor;
		temp_sssColor = sssColor;
		temp_foamColor = foamColor;
		temp_underwaterColor = underwaterColor;
		temp_beaufortScale = beaufortScale;
		temp_flowDirection = flowDirection;
		temp_flowSpeed = flowSpeed;
		temp_waveScale = waveScale;
		temp_waveHeight = waveHeight;
		temp_heightProjection = heightProjection;
		temp_turbulenceFactor = turbulenceFactor;
		temp_lgWaveHeight = lgWaveHeight;
		temp_lgWaveScale = lgWaveScale;
		temp_shorelineHeight = shorelineHeight;
		temp_shorelineFreq = shorelineFreq;
		temp_shorelineScale = shorelineScale;
		temp_shorelineSpeed = shorelineSpeed;
		temp_shorelineNorm = shorelineNorm;
		temp_overallBright = overallBright;
		temp_overallTransparency = overallTransparency;
		temp_edgeAmt = edgeAmt;
		temp_depthAmt = depthAmt;
		temp_shallowAmt = shallowAmt;
		temp_refractStrength = refractStrength;
		temp_aberrationScale = aberrationScale;
		temp_causticsFade = causticsFade;
		temp_reflectProjection = reflectProjection;
		temp_reflectBlur = reflectBlur;
		temp_reflectTerm = reflectTerm;
		temp_reflectSharpen = reflectSharpen;
		temp_roughness = roughness;
		temp_roughness2 = roughness2;
		temp_foamScale = foamScale;
		temp_foamSpeed = foamSpeed;
		temp_edgeFoamAmt = edgeFoamAmt;
		temp_shallowFoamAmt = shallowFoamAmt;
		temp_heightFoamAmt = heightFoamAmt;
		temp_hFoamHeight = hFoamHeight;
		temp_hFoamSpread = hFoamSpread;
		temp_underLightFactor = underLightFactor;
		temp_underRefractionAmount = underRefractionAmount;
		temp_underRefractionScale = underRefractionScale;
		temp_underRefractionSpeed = underRefractionSpeed;
		temp_underBlurAmount = underBlurAmount;
		temp_underwaterFogDist = underwaterFogDist;
		temp_underwaterFogSpread = underwaterFogSpread;
		temp_underDarkRange = underDarkRange;
	}
}
