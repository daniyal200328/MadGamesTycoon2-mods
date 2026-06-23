using System;
using System.Collections.Generic;
using UnityEngine;

namespace Suimono.Core;

[ExecuteInEditMode]
public class SuimonoModule : MonoBehaviour
{
	public string suimonoVersionNumber = "";

	public float systemTime;

	public bool autoSetLayers = true;

	public string layerWater;

	public int layerWaterNum = -1;

	public string layerDepth;

	public int layerDepthNum = -1;

	public string layerScreenFX;

	public int layerScreenFXNum = -1;

	public bool layersAreSet;

	public bool autoSetCameraFX = true;

	public Transform manualCamera;

	public Transform mainCamera;

	public int cameraTypeIndex;

	[NonSerialized]
	public List<string> cameraTypeOptions = new List<string> { "Auto Select Camera", "Manual Select Camera" };

	public Transform setCamera;

	public Transform setTrack;

	public Light setLight;

	public bool enableUnderwaterFX = true;

	public bool enableInteraction = true;

	public float objectEnableUnderwaterFX = 1f;

	public bool disableMSAA;

	public bool enableRefraction = true;

	public bool enableReflections = true;

	public bool enableDynamicReflections = true;

	public bool enableCaustics = true;

	public bool enableCausticsBlending;

	public bool enableAdvancedEdge = true;

	public bool enableAdvancedDistort = true;

	public bool enableTenkoku;

	public bool enableAutoAdvance = true;

	public bool showPerformance;

	public bool showGeneral;

	public Color underwaterColor = new Color(0.58f, 0.61f, 0.61f, 0f);

	public bool enableTransition = true;

	public float transition_offset = 0.1f;

	public GameObject fxRippleObject;

	private float underLightAmt;

	private GameObject targetSurface;

	private float doTransitionTimer;

	public bool isUnderwater;

	private static bool doWaterTransition;

	public bool enableTransparency = true;

	private bool useEnableTransparency = true;

	public int transResolution = 3;

	public int transLayer;

	public LayerMask transLayerMask;

	public int causticLayer;

	public LayerMask causticLayerMask;

	[NonSerialized]
	public List<string> suiLayerMasks;

	[NonSerialized]
	public List<string> resOptions = new List<string> { "4096", "2048", "1024", "512", "256", "128", "64", "32", "16", "8" };

	[NonSerialized]
	public List<int> resolutions = new List<int> { 4096, 2048, 1024, 512, 256, 128, 64, 32, 16, 8 };

	public float transRenderDistance = 100f;

	public bool playSounds = true;

	public bool playSoundBelow = true;

	public bool playSoundAbove = true;

	public float maxVolume = 1f;

	public int maxSounds = 10;

	public AudioClip[] defaultSplashSound;

	private float setvolume = 0.65f;

	private GameObject underSoundObject;

	private AudioSource underSoundComponent;

	private AudioSource[] sndComponents;

	private int currentSound;

	public float transitionStrength = 1f;

	public float currentObjectIsOver;

	public float currentObjectDepth;

	public float currentTransitionDepth;

	public float currentSurfaceLevel;

	public float underwaterThreshold = 0.1f;

	public SuimonoObject suimonoObject;

	private ParticleSystem.Particle[] effectBubbles;

	public SuimonoModuleLib suimonoModuleLibrary;

	public Camera setCameraComponent;

	private float underTrans;

	public float useTenkoku;

	public float tenkokuWindDir;

	public float tenkokuWindAmt;

	public bool tenkokuUseWind = true;

	private GameObject tenObject;

	public bool showTenkoku = true;

	public bool tenkokuUseReflect = true;

	private WindZone tenkokuWindModule;

	private int lx;

	private int fx;

	private int px;

	private ParticleSystem.Particle[] setParticles;

	private AudioClip setstep;

	private float setpitch;

	private AudioSource useSoundAudioComponent;

	private float useRefract;

	private float useLight = 1f;

	private Color useLightCol;

	private Vector2 flow_dir;

	private Vector3 tempAngle;

	private float getheight;

	private float getheightC;

	private float getheightT;

	private float getheightR;

	private bool isOverWater;

	private float surfaceLevel;

	private int layer;

	private int layermask;

	private Vector3 testpos;

	private int i;

	private RaycastHit hit;

	private Vector2 pixelUV;

	private float returnValue;

	private float[] returnValueAll;

	private float h1;

	private float setDegrees;

	private float enabledUFX = 1f;

	private float enabledCaustics = 1f;

	private float setUnderBright;

	private float enCaustic;

	private float setEdge = 1f;

	private Suimono_UnderwaterFog underwaterObject;

	private GameObject currentSurfaceObject;

	public float[] heightValues;

	public float isForward;

	public float isAdvDist;

	public float waveScale = 1f;

	public float flowSpeed = 0.02f;

	public float offset;

	public Texture2D heightTex;

	public Texture2D heightTexT;

	public Texture2D heightTexR;

	public Transform heightObject;

	public Vector2 relativePos = new Vector2(0f, 0f);

	public Vector3 texCoord = new Vector3(0f, 0f, 0f);

	public Vector3 texCoord1 = new Vector3(0f, 0f, 0f);

	public Vector3 texCoordT = new Vector3(0f, 0f, 0f);

	public Vector3 texCoordT1 = new Vector3(0f, 0f, 0f);

	public Vector3 texCoordR = new Vector3(0f, 0f, 0f);

	public Vector3 texCoordR1 = new Vector3(0f, 0f, 0f);

	public Color heightVal0;

	public Color heightVal1;

	public Color heightValT0;

	public Color heightValT1;

	public Color heightValR0;

	public Color heightValR1;

	public float localTime;

	private float baseHeight;

	private float baseAngle;

	private Color[] pixelArray;

	private Color[] pixelArrayT;

	private Color[] pixelArrayR;

	private Texture2D useDecodeTex;

	private Color[] useDecodeArray;

	public int row;

	public int pixIndex;

	public Color pixCol;

	public int t;

	public int y;

	public Vector3 dir;

	public Vector3 pivotPoint;

	public float useLocalTime;

	public Vector2 flow_dirC;

	public Vector2 flowSpeed0;

	public Vector2 flowSpeed1;

	public Vector2 flowSpeed2;

	public Vector2 flowSpeed3;

	public float tScale;

	public Vector2 oPos;

	private int renderCount;

	private int randSeed;

	private Random modRand;

	private List<SuimonoObject> sObjects;

	private List<Renderer> sRends;

	private List<Renderer> sRendSCs;

	private ParticleSystem.EmissionModule debrisEmission;

	private ColorSpace _colorspace;

	private float _deltaTime;

	private void Awake()
	{
		suimonoVersionNumber = "2.1.13";
		base.gameObject.name = "SUIMONO_Module";
		sObjects = new List<SuimonoObject>();
		sRends = new List<Renderer>();
		sRendSCs = new List<Renderer>();
	}

	private void Start()
	{
		randSeed = Environment.TickCount;
		modRand = new Random(randSeed);
		InitLayers();
		Suimono_CheckCamera();
		if (autoSetLayers)
		{
			for (lx = 0; lx < 32; lx++)
			{
				Physics.IgnoreLayerCollision(lx, layerWaterNum);
			}
		}
		suimonoModuleLibrary = base.gameObject.GetComponent<SuimonoModuleLib>();
		if (suimonoModuleLibrary.sndparentobj != null)
		{
			maxSounds = suimonoModuleLibrary.sndparentobj.maxSounds;
			sndComponents = new AudioSource[maxSounds];
			GameObject[] array = new GameObject[maxSounds];
			for (int i = 0; i < maxSounds; i++)
			{
				array[i] = new GameObject("SuimonoAudioObject");
				array[i].transform.parent = suimonoModuleLibrary.sndparentobj.transform;
				array[i].AddComponent<AudioSource>();
				sndComponents[i] = array[i].GetComponent<AudioSource>();
			}
			if (suimonoModuleLibrary.sndparentobj.underwaterSound != null)
			{
				underSoundObject = new GameObject("Underwater Sound");
				underSoundObject.AddComponent<AudioSource>();
				underSoundObject.transform.parent = suimonoModuleLibrary.sndparentobj.transform;
				underSoundComponent = underSoundObject.GetComponent<AudioSource>();
			}
		}
		if (disableMSAA)
		{
			QualitySettings.antiAliasing = 0;
		}
		_colorspace = QualitySettings.activeColorSpace;
		if (_colorspace == ColorSpace.Linear)
		{
			Shader.SetGlobalFloat("_Suimono_isLinear", 1f);
		}
		else
		{
			Shader.SetGlobalFloat("_Suimono_isLinear", 0f);
		}
		if (suimonoModuleLibrary != null)
		{
			if (suimonoModuleLibrary.texNormalC != null)
			{
				heightTex = suimonoModuleLibrary.texNormalC;
				pixelArray = suimonoModuleLibrary.texNormalC.GetPixels(0);
			}
			if (suimonoModuleLibrary.texNormalT != null)
			{
				heightTexT = suimonoModuleLibrary.texNormalT;
				pixelArrayT = suimonoModuleLibrary.texNormalT.GetPixels(0);
			}
			if (suimonoModuleLibrary.texNormalR != null)
			{
				heightTexR = suimonoModuleLibrary.texNormalR;
				pixelArrayR = suimonoModuleLibrary.texNormalR.GetPixels(0);
			}
		}
		tenObject = GameObject.Find("Tenkoku DynamicSky");
		Shader.SetGlobalFloat("_useTenkoku", 0f);
	}

	private void InitLayers()
	{
		if (autoSetLayers && !layersAreSet)
		{
			layersAreSet = true;
		}
	}

	private void LateUpdate()
	{
		_deltaTime = Time.deltaTime;
		if (modRand == null)
		{
			modRand = new Random(randSeed);
		}
		if (systemTime < 0f)
		{
			systemTime = 0f;
		}
		if (enableAutoAdvance)
		{
			systemTime += _deltaTime;
		}
		SetCullFunction();
		useTenkoku = 0f;
		if (tenObject != null)
		{
			if (tenObject.activeInHierarchy)
			{
				useTenkoku = 1f;
			}
			if (useTenkoku == 1f)
			{
				if (setLight == null)
				{
					setLight = GameObject.Find("LIGHT_World").GetComponent<Light>();
				}
				if (tenkokuWindModule == null)
				{
					tenkokuWindModule = GameObject.Find("Tenkoku_WindZone").GetComponent<WindZone>();
				}
				else
				{
					tenkokuWindDir = tenkokuWindModule.transform.eulerAngles.y;
					tenkokuWindAmt = tenkokuWindModule.windMain;
				}
			}
		}
		Shader.SetGlobalFloat("_useTenkoku", useTenkoku);
		if (Application.isPlaying && fxRippleObject == null)
		{
			fxRippleObject = GameObject.Find("fx_rippleNormals(Clone)");
		}
		if (fxRippleObject != null)
		{
			fxRippleObject.layer = layerScreenFXNum;
		}
		if (suimonoModuleLibrary.normalsCamObject != null)
		{
			suimonoModuleLibrary.normalsCamObject.cullingMask = 1 << layerScreenFXNum;
		}
		if (suimonoModuleLibrary.wakeCamObject != null)
		{
			suimonoModuleLibrary.wakeCamObject.cullingMask = 1 << layerScreenFXNum;
		}
		if (suimonoModuleLibrary.transCamObject != null)
		{
			transLayer &= ~(1 << layerWaterNum);
			transLayer &= ~(1 << layerDepthNum);
			transLayer &= ~(1 << layerScreenFXNum);
			suimonoModuleLibrary.transCamObject.cullingMask = transLayer;
			suimonoModuleLibrary.transCamObject.farClipPlane = transRenderDistance * 1.2f;
			Shader.SetGlobalFloat("_suimonoTransDist", transRenderDistance);
		}
		else
		{
			suimonoModuleLibrary.transCamObject = base.transform.Find("cam_SuimonoTrans").gameObject.GetComponent<Camera>();
		}
		if (suimonoModuleLibrary.transToolsObject != null)
		{
			suimonoModuleLibrary.transToolsObject.resolution = Convert.ToInt32(resolutions[transResolution]);
			suimonoModuleLibrary.transToolsObject.gameObject.SetActive(enableTransparency);
		}
		else
		{
			suimonoModuleLibrary.transToolsObject = base.transform.Find("cam_SuimonoTrans").gameObject.GetComponent<cameraTools>();
		}
		if (suimonoModuleLibrary.causticCamObject != null)
		{
			if (enableCaustics)
			{
				suimonoModuleLibrary.causticCamObject.gameObject.SetActive(enableCausticsBlending);
				transLayer &= ~(1 << layerDepthNum);
				transLayer &= ~(1 << layerScreenFXNum);
				suimonoModuleLibrary.causticCamObject.cullingMask = transLayer;
				suimonoModuleLibrary.causticCamObject.farClipPlane = transRenderDistance * 1.2f;
			}
			if (suimonoModuleLibrary.causticHandlerObjectTrans != null)
			{
				suimonoModuleLibrary.causticHandlerObjectTrans.enabled = !enableCausticsBlending;
			}
		}
		else
		{
			suimonoModuleLibrary.causticCamObject = base.transform.Find("cam_SuimonoCaustic").gameObject.GetComponent<Camera>();
		}
		if (suimonoModuleLibrary.causticToolsObject != null)
		{
			suimonoModuleLibrary.causticToolsObject.resolution = Convert.ToInt32(resolutions[transResolution]);
		}
		else
		{
			suimonoModuleLibrary.causticToolsObject = base.transform.Find("cam_SuimonoCaustic").gameObject.GetComponent<cameraTools>();
		}
		Shader.SetGlobalFloat("_enableTransparency", useEnableTransparency ? 1f : 0f);
		enCaustic = 0f;
		if (enableCaustics)
		{
			enCaustic = 1f;
		}
		Shader.SetGlobalFloat("_suimono_enableCaustic", enCaustic);
		causticLayer = (causticLayer &= ~(1 << layerWaterNum << layerDepthNum << layerScreenFXNum));
		setEdge = 1f;
		if (!enableAdvancedEdge)
		{
			setEdge = 0f;
		}
		Shader.SetGlobalFloat("_suimono_advancedEdge", setEdge);
	}

	private void FixedUpdate()
	{
		if (autoSetLayers)
		{
			for (lx = 0; lx < 20; lx++)
			{
				Physics.IgnoreLayerCollision(lx, layerWaterNum);
			}
		}
		Suimono_CheckCamera();
		if (suimonoModuleLibrary.causticObject != null)
		{
			if (Application.isPlaying)
			{
				suimonoModuleLibrary.causticObject.enableCaustics = enableCaustics;
			}
			else
			{
				suimonoModuleLibrary.causticObject.enableCaustics = false;
			}
			if (setLight != null)
			{
				suimonoModuleLibrary.causticObject.sceneLightObject = setLight;
			}
		}
		PlayUnderwaterSound();
		if (setCamera != null)
		{
			isForward = 0f;
			if (setCameraComponent.actualRenderingPath == RenderingPath.Forward)
			{
				isForward = 1f;
			}
			Shader.SetGlobalFloat("_isForward", isForward);
		}
		if (enableAdvancedDistort)
		{
			isAdvDist = 1f;
			suimonoModuleLibrary.wakeObject.SetActive(value: true);
			suimonoModuleLibrary.normalsObject.SetActive(value: true);
		}
		else
		{
			isAdvDist = 0f;
			suimonoModuleLibrary.wakeObject.SetActive(value: false);
			suimonoModuleLibrary.normalsObject.SetActive(value: false);
		}
		Shader.SetGlobalFloat("_suimono_advancedDistort", isAdvDist);
		if (setCameraComponent != null)
		{
			if (suimonoObject != null)
			{
				setCameraComponent.backgroundColor = suimonoObject.underwaterColor;
			}
			Shader.SetGlobalColor("_cameraBGColor", setCameraComponent.backgroundColor);
		}
		if (setCameraComponent != null)
		{
			if (setCameraComponent.actualRenderingPath == RenderingPath.DeferredShading)
			{
				setCameraComponent.depthTextureMode = DepthTextureMode.Depth;
			}
			else if (setCameraComponent.actualRenderingPath == RenderingPath.DeferredLighting)
			{
				setCameraComponent.depthTextureMode = DepthTextureMode.Depth;
			}
			else
			{
				setCameraComponent.depthTextureMode = DepthTextureMode.DepthNormals;
			}
			setCameraComponent.cullingMask |= 1 << layerWaterNum;
			setCameraComponent.cullingMask = setCameraComponent.cullingMask & ~(1 << layerDepthNum) & ~(1 << layerScreenFXNum);
		}
	}

	private void OnDisable()
	{
		CancelInvoke("StoreSurfaceHeight");
	}

	private void OnEnable()
	{
		InvokeRepeating("StoreSurfaceHeight", 0.01f, 0.1f);
	}

	private void StoreSurfaceHeight()
	{
		if (base.enabled && setCamera != null)
		{
			heightValues = SuimonoGetHeightAll(setCamera.transform.position);
			currentSurfaceLevel = heightValues[1];
			currentObjectDepth = heightValues[3];
			currentObjectIsOver = heightValues[4];
			currentTransitionDepth = heightValues[9];
			objectEnableUnderwaterFX = heightValues[10];
			checkUnderwaterEffects();
			checkWaterTransition();
		}
	}

	private void PlayUnderwaterSound()
	{
		if (!Application.isPlaying || !(underSoundObject != null) || !(setTrack != null) || !(underSoundComponent != null))
		{
			return;
		}
		underSoundObject.transform.position = setTrack.position;
		if (currentTransitionDepth > 0f)
		{
			if (playSoundBelow && playSounds)
			{
				underSoundComponent.clip = suimonoModuleLibrary.sndparentobj.underwaterSound;
				underSoundComponent.volume = maxVolume;
				underSoundComponent.loop = true;
				if (!underSoundComponent.isPlaying)
				{
					underSoundComponent.Play();
				}
			}
			else
			{
				underSoundComponent.Stop();
			}
		}
		else
		{
			if (!(suimonoModuleLibrary.sndparentobj.underwaterSound != null))
			{
				return;
			}
			if (playSoundAbove && playSounds)
			{
				underSoundComponent.clip = suimonoModuleLibrary.sndparentobj.abovewaterSound;
				underSoundComponent.volume = 0.45f * maxVolume;
				underSoundComponent.loop = true;
				if (!underSoundComponent.isPlaying)
				{
					underSoundComponent.Play();
				}
			}
			else
			{
				underSoundComponent.Stop();
			}
		}
	}

	public void AddFX(int fxSystem, Vector3 effectPos, int addRate, float addSize, float addRot, float addARot, Vector3 addVeloc, Color addCol)
	{
		if (!(suimonoModuleLibrary.fxObject != null))
		{
			return;
		}
		fx = fxSystem;
		if (!(suimonoModuleLibrary.fxObject.fxParticles[fx] != null))
		{
			return;
		}
		suimonoModuleLibrary.fxObject.fxParticles[fx].Emit(addRate);
		if (setParticles != null)
		{
			setParticles = null;
		}
		setParticles = new ParticleSystem.Particle[suimonoModuleLibrary.fxObject.fxParticles[fx].particleCount];
		suimonoModuleLibrary.fxObject.fxParticles[fx].GetParticles(setParticles);
		if ((float)suimonoModuleLibrary.fxObject.fxParticles[fx].particleCount > 0f)
		{
			for (px = suimonoModuleLibrary.fxObject.fxParticles[fx].particleCount - addRate; px < suimonoModuleLibrary.fxObject.fxParticles[fx].particleCount; px++)
			{
				setParticles[px].position = new Vector3(effectPos.x, effectPos.y, effectPos.z);
				setParticles[px].startSize = addSize;
				setParticles[px].rotation = addRot;
				setParticles[px].angularVelocity = addARot;
				setParticles[px].velocity = new Vector3(addVeloc.x, addVeloc.y, addVeloc.z);
				ref ParticleSystem.Particle reference = ref setParticles[px];
				reference.startColor *= addCol;
			}
			suimonoModuleLibrary.fxObject.fxParticles[fx].SetParticles(setParticles, setParticles.Length);
			suimonoModuleLibrary.fxObject.fxParticles[fx].Play();
		}
	}

	public void AddSoundFX(AudioClip sndClip, Vector3 soundPos, Vector3 sndVelocity)
	{
		if (!enableInteraction)
		{
			return;
		}
		setpitch = 1f;
		setvolume = 1f;
		if (playSounds && suimonoModuleLibrary.sndparentobj.defaultSplashSound.Length >= 1)
		{
			setstep = suimonoModuleLibrary.sndparentobj.defaultSplashSound[modRand.Next(0, defaultSplashSound.Length - 1)];
			setpitch = sndVelocity.y;
			setvolume = sndVelocity.z;
			setvolume = Mathf.Lerp(0f, 1f, setvolume) * maxVolume;
			if (currentObjectDepth > 0f)
			{
				setpitch *= 0.25f;
				setvolume *= 0.5f;
			}
			useSoundAudioComponent = sndComponents[currentSound];
			useSoundAudioComponent.clip = sndClip;
			if (!useSoundAudioComponent.isPlaying)
			{
				useSoundAudioComponent.transform.localPosition = soundPos;
				useSoundAudioComponent.volume = setvolume;
				useSoundAudioComponent.pitch = setpitch;
				useSoundAudioComponent.minDistance = 4f;
				useSoundAudioComponent.maxDistance = 20f;
				useSoundAudioComponent.clip = setstep;
				useSoundAudioComponent.loop = false;
				useSoundAudioComponent.Play();
			}
			currentSound++;
			if (currentSound >= maxSounds - 1)
			{
				currentSound = 0;
			}
		}
	}

	public void AddSound(string sndMode, Vector3 soundPos, Vector3 sndVelocity)
	{
		if (!enableInteraction)
		{
			return;
		}
		setpitch = 1f;
		setvolume = 1f;
		if (playSounds && suimonoModuleLibrary.sndparentobj.defaultSplashSound.Length >= 1)
		{
			setstep = suimonoModuleLibrary.sndparentobj.defaultSplashSound[modRand.Next(0, suimonoModuleLibrary.sndparentobj.defaultSplashSound.Length - 1)];
			setpitch = sndVelocity.y;
			setvolume = sndVelocity.z;
			setvolume = Mathf.Lerp(0f, 10f, setvolume);
			if (currentObjectDepth > 0f)
			{
				setpitch *= 0.25f;
				setvolume *= 0.5f;
			}
			useSoundAudioComponent = sndComponents[currentSound];
			if (!useSoundAudioComponent.isPlaying)
			{
				useSoundAudioComponent.transform.localPosition = soundPos;
				useSoundAudioComponent.volume = setvolume;
				useSoundAudioComponent.pitch = setpitch;
				useSoundAudioComponent.minDistance = 4f;
				useSoundAudioComponent.maxDistance = 20f;
				useSoundAudioComponent.clip = setstep;
				useSoundAudioComponent.loop = false;
				useSoundAudioComponent.Play();
			}
			currentSound++;
			if (currentSound >= maxSounds - 1)
			{
				currentSound = 0;
			}
		}
	}

	private void checkUnderwaterEffects()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		if (currentTransitionDepth > underwaterThreshold)
		{
			if (suimonoObject != null && enableUnderwaterFX && suimonoObject.enableUnderwater && currentObjectIsOver == 1f)
			{
				isUnderwater = true;
				Shader.SetGlobalFloat("_Suimono_IsUnderwater", 1f);
				if (suimonoObject != null)
				{
					suimonoObject.useShader = suimonoObject.shader_Under;
				}
				if (suimonoModuleLibrary.causticHandlerObject != null)
				{
					suimonoModuleLibrary.causticHandlerObjectTrans.isUnderwater = true;
					suimonoModuleLibrary.causticHandlerObject.isUnderwater = true;
				}
			}
		}
		else
		{
			isUnderwater = false;
			Shader.SetGlobalFloat("_Suimono_IsUnderwater", 0f);
			if (suimonoObject != null)
			{
				suimonoObject.useShader = suimonoObject.shader_Surface;
			}
			if (suimonoModuleLibrary.causticHandlerObject != null)
			{
				suimonoModuleLibrary.causticHandlerObjectTrans.isUnderwater = false;
				suimonoModuleLibrary.causticHandlerObject.isUnderwater = false;
			}
		}
	}

	private void checkWaterTransition()
	{
		if (!Application.isPlaying)
		{
			return;
		}
		doTransitionTimer += _deltaTime;
		if (currentTransitionDepth > underwaterThreshold && currentObjectIsOver == 1f)
		{
			doWaterTransition = true;
			if (suimonoObject != null && setCamera != null)
			{
				if (enableUnderwaterFX && suimonoObject.enableUnderwater && objectEnableUnderwaterFX == 1f)
				{
					if (suimonoObject.enableUnderDebris)
					{
						suimonoModuleLibrary.underwaterDebrisTransform.position = setCamera.transform.position;
						suimonoModuleLibrary.underwaterDebrisTransform.rotation = setCamera.transform.rotation;
						suimonoModuleLibrary.underwaterDebrisTransform.Translate(Vector3.forward * 5f);
						suimonoModuleLibrary.underwaterDebrisRendererComponent.enabled = true;
						debrisEmission = suimonoModuleLibrary.underwaterDebris.emission;
						debrisEmission.enabled = isUnderwater;
					}
					else if (suimonoModuleLibrary.underwaterDebris != null)
					{
						suimonoModuleLibrary.underwaterDebrisRendererComponent.enabled = false;
					}
					setUnderBright = underLightAmt;
					setUnderBright *= 0.5f;
					useLight = 1f;
					useLightCol = new Color(1f, 1f, 1f, 1f);
					useRefract = 1f;
					if (setLight != null)
					{
						useLight = setLight.intensity;
						useLightCol = setLight.color;
					}
					if (!enableRefraction)
					{
						useRefract = 0f;
					}
					if (underwaterObject == null && setCamera.gameObject.GetComponent<Suimono_UnderwaterFog>() != null)
					{
						underwaterObject = setCamera.gameObject.GetComponent<Suimono_UnderwaterFog>();
					}
					if (underwaterObject != null)
					{
						underwaterObject.lightFactor = suimonoObject.underLightFactor * useLight;
						underwaterObject.refractAmt = suimonoObject.underRefractionAmount * useRefract;
						underwaterObject.refractScale = suimonoObject.underRefractionScale;
						underwaterObject.refractSpd = suimonoObject.underRefractionSpeed * useRefract;
						underwaterObject.fogEnd = suimonoObject.underwaterFogDist;
						underwaterObject.fogStart = suimonoObject.underwaterFogSpread;
						underwaterObject.blurSpread = suimonoObject.underBlurAmount;
						underwaterObject.underwaterColor = suimonoObject.underwaterColor;
						underwaterObject.darkRange = suimonoObject.underDarkRange;
						underwaterObject.transitionStrength = transitionStrength;
						Shader.SetGlobalColor("_suimono_lightColor", useLightCol);
						underwaterObject.doTransition = false;
						if (suimonoModuleLibrary.causticObject != null && Application.isPlaying && suimonoModuleLibrary.causticObject != null)
						{
							suimonoModuleLibrary.causticObject.heightFac = underwaterObject.hFac * 2f;
						}
					}
				}
				else if (suimonoModuleLibrary.underwaterDebris != null)
				{
					suimonoModuleLibrary.underwaterDebrisRendererComponent.enabled = false;
				}
			}
			if (underwaterObject != null)
			{
				underwaterObject.cancelTransition = true;
			}
		}
		else
		{
			if (suimonoModuleLibrary.underwaterDebris != null)
			{
				suimonoModuleLibrary.underwaterDebrisRendererComponent.enabled = false;
			}
			if (enableTransition)
			{
				if (doWaterTransition && setCamera != null)
				{
					doTransitionTimer = 0f;
					if (underwaterObject != null)
					{
						underwaterObject.doTransition = true;
					}
					doWaterTransition = false;
				}
				else
				{
					underTrans = 1f;
				}
			}
		}
		if (!enableUnderwaterFX && suimonoModuleLibrary.underwaterDebrisRendererComponent != null)
		{
			suimonoModuleLibrary.underwaterDebrisRendererComponent.enabled = false;
		}
	}

	private void Suimono_CheckCamera()
	{
		if (cameraTypeIndex == 0)
		{
			if (Camera.main != null)
			{
				mainCamera = Camera.main.transform;
			}
			manualCamera = null;
		}
		if (cameraTypeIndex == 1)
		{
			if (manualCamera != null)
			{
				mainCamera = manualCamera;
			}
			else if (Camera.main != null)
			{
				mainCamera = Camera.main.transform;
			}
		}
		if (setCamera != mainCamera)
		{
			setCamera = mainCamera;
			setCameraComponent = setCamera.gameObject.GetComponent<Camera>();
			underwaterObject = setCamera.gameObject.GetComponent<Suimono_UnderwaterFog>();
		}
		if (setCameraComponent == null && setCamera != null)
		{
			setCameraComponent = setCamera.gameObject.GetComponent<Camera>();
		}
		if (setCamera != null && setCameraComponent != null && setCameraComponent.transform != setCamera)
		{
			setCameraComponent = setCamera.gameObject.GetComponent<Camera>();
			underwaterObject = setCamera.gameObject.GetComponent<Suimono_UnderwaterFog>();
		}
		if (setTrack == null && setCamera != null)
		{
			setTrack = setCamera.transform;
		}
		InstallCameraEffect();
	}

	public Vector2 SuimonoConvertAngleToDegrees(float convertAngle)
	{
		flow_dir = new Vector3(0f, 0f, 0f);
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

	public Vector2 SuimonoConvertAngleToVector(float convertAngle)
	{
		flow_dir = new Vector3(0f, 0f, 0f);
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

	public float SuimonoGetHeight(Vector3 testObject, string returnMode)
	{
		CalculateHeights(testObject);
		returnValue = 0f;
		if (returnMode == "height")
		{
			returnValue = getheight;
		}
		if (returnMode == "surfaceLevel")
		{
			returnValue = surfaceLevel + getheight;
		}
		if (returnMode == "baseLevel")
		{
			returnValue = surfaceLevel;
		}
		if (returnMode == "object depth")
		{
			returnValue = getheight - testObject.y;
		}
		if (returnMode == "isOverWater" && isOverWater)
		{
			returnValue = 1f;
		}
		if (returnMode == "isOverWater" && !isOverWater)
		{
			returnValue = 0f;
		}
		if (returnMode == "isAtSurface" && surfaceLevel + getheight - testObject.y < 0.25f && surfaceLevel + getheight - testObject.y > -0.25f)
		{
			returnValue = 1f;
		}
		if (suimonoObject != null)
		{
			if (returnMode == "direction")
			{
				returnValue = suimonoObject.flowDirection;
			}
			if (returnMode == "speed")
			{
				returnValue = suimonoObject.flowSpeed;
			}
			if (returnMode == "wave height")
			{
				h1 = 0f;
				returnValue = getheight / h1;
			}
		}
		if (returnMode == "transitionDepth")
		{
			returnValue = surfaceLevel + getheight - (testObject.y - transition_offset * underTrans);
		}
		if (returnMode == "underwaterEnabled")
		{
			enabledUFX = 1f;
			if (!suimonoObject.enableUnderwater)
			{
				enabledUFX = 0f;
			}
			returnValue = enabledUFX;
		}
		if (returnMode == "causticsEnabled")
		{
			enabledCaustics = 1f;
			if (!suimonoObject.enableCausticFX)
			{
				enabledCaustics = 0f;
			}
			returnValue = enabledCaustics;
		}
		return returnValue;
	}

	public float[] SuimonoGetHeightAll(Vector3 testObject)
	{
		CalculateHeights(testObject);
		returnValueAll = new float[12];
		returnValueAll[0] = getheight;
		returnValueAll[1] = getheight;
		returnValueAll[2] = surfaceLevel;
		returnValueAll[3] = getheight - testObject.y;
		returnValue = 1f;
		if (!isOverWater)
		{
			returnValue = 0f;
		}
		returnValueAll[4] = returnValue;
		returnValue = 0f;
		if (getheight - testObject.y < 0.25f && getheight - testObject.y > -0.25f)
		{
			returnValue = 1f;
		}
		returnValueAll[5] = returnValue;
		if (suimonoObject != null)
		{
			setDegrees = suimonoObject.flowDirection + suimonoObject.transform.eulerAngles.y;
			if (setDegrees < 0f)
			{
				setDegrees = 365f + setDegrees;
			}
			if (setDegrees > 365f)
			{
				setDegrees -= 365f;
			}
			if (suimonoObject != null)
			{
				returnValueAll[6] = setDegrees;
			}
			if (suimonoObject == null)
			{
				returnValueAll[6] = 0f;
			}
			if (suimonoObject != null)
			{
				returnValueAll[7] = suimonoObject.flowSpeed;
			}
			if (suimonoObject == null)
			{
				returnValueAll[7] = 0f;
			}
			if (suimonoObject != null)
			{
				h1 = suimonoObject.lgWaveHeight;
			}
			if (suimonoObject == null)
			{
				h1 = 0f;
			}
			returnValueAll[8] = getheight / h1;
		}
		returnValueAll[9] = getheight - (testObject.y - transition_offset * underTrans);
		enabledUFX = 1f;
		if (suimonoObject != null)
		{
			if (!suimonoObject.enableUnderwater)
			{
				enabledUFX = 0f;
			}
			returnValueAll[10] = enabledUFX;
		}
		enabledCaustics = 1f;
		if (suimonoObject != null)
		{
			if (!suimonoObject.enableCausticFX)
			{
				enabledCaustics = 0f;
			}
			returnValueAll[11] = enabledCaustics;
		}
		return returnValueAll;
	}

	public Vector3 RotatePointAroundPivot(Vector3 point, Vector3 pivot, Vector3 angles)
	{
		dir = point - pivot;
		dir = Quaternion.Euler(angles * -1f) * dir;
		point = dir + pivot;
		return point;
	}

	public Color DecodeHeightPixels(float texPosx, float texPosy, int texNum)
	{
		if (texNum == 0)
		{
			useDecodeTex = heightTex;
			useDecodeArray = pixelArray;
		}
		if (texNum == 1)
		{
			useDecodeTex = heightTexT;
			useDecodeArray = pixelArrayT;
		}
		if (texNum == 2)
		{
			useDecodeTex = heightTexR;
			useDecodeArray = pixelArrayR;
		}
		texPosx %= (float)useDecodeTex.width;
		texPosy %= (float)useDecodeTex.height;
		if (texPosx < 0f)
		{
			texPosx = (float)useDecodeTex.width - Mathf.Abs(texPosx);
		}
		if (texPosy < 0f)
		{
			texPosy = (float)useDecodeTex.height - Mathf.Abs(texPosy);
		}
		if (texPosx > (float)useDecodeTex.width)
		{
			texPosx -= (float)useDecodeTex.width;
		}
		if (texPosy > (float)useDecodeTex.height)
		{
			texPosy -= (float)useDecodeTex.height;
		}
		row = useDecodeArray.Length / useDecodeTex.height - Mathf.FloorToInt(texPosy);
		pixIndex = Mathf.FloorToInt(texPosx) + 1 + (useDecodeArray.Length - useDecodeTex.width * row) - 1;
		if (pixIndex > useDecodeArray.Length)
		{
			pixIndex -= useDecodeArray.Length;
		}
		if (pixIndex < 0)
		{
			pixIndex = useDecodeArray.Length - pixIndex;
		}
		pixCol = useDecodeArray[pixIndex];
		if (_colorspace == ColorSpace.Linear)
		{
			pixCol.a = Mathf.Clamp(Mathf.Lerp(-0.035f, 0.5f, pixCol.a), 0f, 1f);
		}
		if (_colorspace == ColorSpace.Gamma)
		{
			pixCol.a = Mathf.Clamp(Mathf.Lerp(-0.035f, 0.5f, pixCol.a), 0f, 1f);
		}
		return pixCol;
	}

	private void CalculateHeights(Vector3 testObject)
	{
		getheight = -1f;
		getheightC = -1f;
		getheightT = -1f;
		getheightR = 0f;
		isOverWater = false;
		surfaceLevel = -1f;
		layermask = 1 << layerWaterNum;
		testpos = new Vector3(testObject.x, testObject.y + 30000f, testObject.z);
		if (!Physics.Raycast(testpos, -Vector3.up, out hit, 60000f, layermask))
		{
			return;
		}
		targetSurface = hit.transform.gameObject;
		if (currentSurfaceObject != targetSurface || suimonoObject == null)
		{
			currentSurfaceObject = targetSurface;
			if (hit.transform.parent != null && hit.transform.parent.gameObject.GetComponent<SuimonoObject>() != null)
			{
				suimonoObject = hit.transform.parent.gameObject.GetComponent<SuimonoObject>();
			}
		}
		if (!(suimonoObject != null) || !enableInteraction || !suimonoObject.enableInteraction)
		{
			return;
		}
		if (suimonoObject.typeIndex == 0)
		{
			heightObject = hit.transform;
		}
		else
		{
			heightObject = hit.transform.parent;
		}
		if (!(suimonoObject != null) || !(hit.collider != null))
		{
			return;
		}
		isOverWater = true;
		surfaceLevel = heightObject.position.y;
		if (heightObject != null)
		{
			baseHeight = heightObject.position.y;
			baseAngle = heightObject.rotation.y;
			relativePos.x = ((heightObject.position.x - testObject.x) / (20f * heightObject.localScale.x) + 1f) * 0.5f * heightObject.localScale.x;
			relativePos.y = ((heightObject.position.z - testObject.z) / (20f * heightObject.localScale.z) + 1f) * 0.5f * heightObject.localScale.z;
		}
		float num = (suimonoObject.enableCustomMesh ? suimonoObject.cmScaleX : 1f);
		float num2 = (suimonoObject.enableCustomMesh ? suimonoObject.cmScaleY : 1f);
		relativePos.x *= num;
		relativePos.y *= num2;
		useLocalTime = suimonoObject.localTime;
		flow_dirC = SuimonoConvertAngleToVector(suimonoObject.flowDirection);
		flowSpeed0 = new Vector2(flow_dirC.x * useLocalTime, flow_dirC.y * useLocalTime);
		flowSpeed1 = new Vector2(flow_dirC.x * useLocalTime * 0.25f, flow_dirC.y * useLocalTime * 0.25f);
		flowSpeed2 = new Vector2(flow_dirC.x * useLocalTime * 0.0625f, flow_dirC.y * useLocalTime * 0.0625f);
		flowSpeed3 = new Vector2(flow_dirC.x * useLocalTime * 0.125f, flow_dirC.y * useLocalTime * 0.125f);
		tScale = 1f / suimonoObject.waveScale;
		oPos = new Vector2(0f - suimonoObject.savePos.x, 0f - suimonoObject.savePos.y);
		if (heightTex != null)
		{
			texCoord.x = (relativePos.x * tScale + flowSpeed0.x + oPos.x) * (float)heightTex.width;
			texCoord.z = (relativePos.y * tScale + flowSpeed0.y + oPos.y) * (float)heightTex.height;
			texCoord1.x = (relativePos.x * tScale * 0.75f - flowSpeed1.x + oPos.x * 0.75f) * (float)heightTex.width;
			texCoord1.z = (relativePos.y * tScale * 0.75f - flowSpeed1.y + oPos.y * 0.75f) * (float)heightTex.height;
			texCoordT.x = (relativePos.x * tScale + flowSpeed0.x + oPos.x) * (float)heightTexT.width;
			texCoordT.z = (relativePos.y * tScale + flowSpeed0.y + oPos.y) * (float)heightTexT.height;
			texCoordT1.x = (relativePos.x * tScale * 0.5f - flowSpeed1.x + oPos.x * 0.5f) * (float)heightTexT.width;
			texCoordT1.z = (relativePos.y * tScale * 0.5f - flowSpeed1.y + oPos.y * 0.5f) * (float)heightTexT.height;
			texCoordR.x = (relativePos.x * suimonoObject.lgWaveScale * tScale + flowSpeed2.x + oPos.x * suimonoObject.lgWaveScale) * (float)heightTexR.width;
			texCoordR.z = (relativePos.y * suimonoObject.lgWaveScale * tScale + flowSpeed2.y + oPos.y * suimonoObject.lgWaveScale) * (float)heightTexR.height;
			texCoordR1.x = (relativePos.x * suimonoObject.lgWaveScale * tScale + flowSpeed3.x + oPos.x * suimonoObject.lgWaveScale) * (float)heightTexR.width;
			texCoordR1.z = (relativePos.y * suimonoObject.lgWaveScale * tScale + flowSpeed3.y + oPos.y * suimonoObject.lgWaveScale) * (float)heightTexR.height;
			if (baseAngle != 0f)
			{
				pivotPoint = new Vector3((float)heightTex.width * heightObject.localScale.x * tScale * 0.5f + flowSpeed0.x * (float)heightTex.width, 0f, (float)heightTex.height * heightObject.localScale.z * tScale * 0.5f + flowSpeed0.y * (float)heightTex.height);
				texCoord = RotatePointAroundPivot(texCoord, pivotPoint, heightObject.eulerAngles);
				pivotPoint = new Vector3((float)heightTex.width * heightObject.localScale.x * tScale * 0.5f * 0.75f - flowSpeed1.x * (float)heightTex.width, 0f, (float)heightTex.height * heightObject.localScale.z * tScale * 0.5f * 0.75f - flowSpeed1.y * (float)heightTex.height);
				texCoord1 = RotatePointAroundPivot(texCoord1, pivotPoint, heightObject.eulerAngles);
				pivotPoint = new Vector3((float)heightTexT.width * heightObject.localScale.x * tScale * 0.5f + flowSpeed0.x * (float)heightTexT.width, 0f, (float)heightTexT.height * heightObject.localScale.z * tScale * 0.5f + flowSpeed0.y * (float)heightTexT.height);
				texCoordT = RotatePointAroundPivot(texCoordT, pivotPoint, heightObject.eulerAngles);
				pivotPoint = new Vector3((float)heightTexT.width * heightObject.localScale.x * tScale * 0.5f * 0.5f - flowSpeed1.x * (float)heightTexT.width, 0f, (float)heightTexT.height * heightObject.localScale.z * tScale * 0.5f * 0.5f - flowSpeed1.y * (float)heightTexT.height);
				texCoordT1 = RotatePointAroundPivot(texCoordT1, pivotPoint, heightObject.eulerAngles);
				pivotPoint = new Vector3((float)heightTexR.width * heightObject.localScale.x * suimonoObject.lgWaveScale * tScale * 0.5f + flowSpeed2.x * (float)heightTexR.width, 0f, (float)heightTexR.height * heightObject.localScale.z * suimonoObject.lgWaveScale * tScale * 0.5f + flowSpeed2.y * (float)heightTexR.height);
				texCoordR = RotatePointAroundPivot(texCoordR, pivotPoint, heightObject.eulerAngles);
				pivotPoint = new Vector3((float)heightTexR.width * heightObject.localScale.x * suimonoObject.lgWaveScale * tScale * 0.5f + flowSpeed3.x * (float)heightTexR.width, 0f, (float)heightTexR.height * heightObject.localScale.z * suimonoObject.lgWaveScale * tScale * 0.5f + flowSpeed3.y * (float)heightTexR.height);
				texCoordR1 = RotatePointAroundPivot(texCoordR1, pivotPoint, heightObject.eulerAngles);
			}
			heightVal0 = DecodeHeightPixels(texCoord.x, texCoord.z, 0);
			heightVal1 = DecodeHeightPixels(texCoord1.x, texCoord1.z, 0);
			heightValT0 = DecodeHeightPixels(texCoordT.x, texCoordT.z, 1);
			heightValT1 = DecodeHeightPixels(texCoordT1.x, texCoordT1.z, 1);
			heightValR0 = DecodeHeightPixels(texCoordR.x, texCoordR.z, 2);
			heightValR1 = DecodeHeightPixels(texCoordR1.x, texCoordR1.z, 2);
			getheightC = (heightVal0.a + heightVal1.a) * 0.8f;
			getheightT = (heightValT0.a * 0.2f + heightValT0.a * heightValT1.a * 0.8f) * suimonoObject.turbulenceFactor * 0.5f;
			getheightR = heightValR0.a * 4f + heightValR1.a * 3f;
			getheight = baseHeight + getheightC * suimonoObject.waveHeight;
			getheight += getheightT * suimonoObject.waveHeight;
			getheight += getheightR * suimonoObject.lgWaveHeight;
			getheight = Mathf.Lerp(baseHeight, getheight, suimonoObject.useHeightProjection);
		}
	}

	public void RegisterSurface(SuimonoObject surface)
	{
		if (Application.isPlaying && surface != null && sObjects != null && sObjects.IndexOf(surface) <= -1)
		{
			sObjects.Add(surface);
			sRends.Add(surface.transform.Find("Suimono_Object").gameObject.GetComponent<Renderer>());
			sRendSCs.Add(surface.transform.Find("Suimono_ObjectScale").gameObject.GetComponent<Renderer>());
		}
	}

	public void DeregisterSurface(SuimonoObject surface)
	{
		if (Application.isPlaying && surface != null && sObjects != null)
		{
			int num = sObjects.IndexOf(surface);
			if (num >= 0)
			{
				sObjects.RemoveAt(num);
				sRends.RemoveAt(num);
				sRendSCs.RemoveAt(num);
			}
		}
	}

	private void SetCullFunction()
	{
		renderCount = 0;
		for (int i = 0; i < sObjects.Count; i++)
		{
			if (sRends[i] == null || !sRends[i].isVisible)
			{
				continue;
			}
			if (sObjects[i].typeIndex == 0)
			{
				if (sRendSCs[i] != null && sRendSCs[i].isVisible)
				{
					renderCount++;
				}
			}
			else
			{
				renderCount++;
			}
		}
		if (renderCount > 0 || isUnderwater)
		{
			useEnableTransparency = enableTransparency;
		}
		if (renderCount <= 0 && !isUnderwater)
		{
			useEnableTransparency = false;
		}
	}

	private void InstallCameraEffect()
	{
		if (setCameraComponent != null && autoSetCameraFX && !(setCameraComponent.gameObject.GetComponent<Suimono_UnderwaterFog>() != null) && enableUnderwaterFX)
		{
			setCameraComponent.gameObject.AddComponent<Suimono_UnderwaterFog>();
		}
	}
}
