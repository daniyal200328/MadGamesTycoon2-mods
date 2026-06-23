using System.Collections.Generic;
using System.IO;
using AllIn1SpriteShader;
using UnityEngine;
using UnityEngine.UI;

[ExecuteInEditMode]
[DisallowMultipleComponent]
[AddComponentMenu("AllIn1SpriteShader/AddAllIn1Shader")]
public class AllIn1Shader : MonoBehaviour
{
	public enum ShaderTypes
	{
		Default,
		ScaledTime,
		MaskedUI,
		Urp2dRenderer,
		Invalid
	}

	private enum AfterSetAction
	{
		Clear,
		CopyMaterial,
		Reset
	}

	public ShaderTypes shaderTypes = ShaderTypes.Invalid;

	private Material currMaterial;

	private Material prevMaterial;

	private bool matAssigned;

	private bool destroyed;

	[Range(1f, 20f)]
	public float normalStrenght = 5f;

	[Range(0f, 3f)]
	public int normalSmoothing = 1;

	[HideInInspector]
	public bool computingNormal;

	private bool needToWait;

	private int waitingCycles;

	private int timesWeWaited;

	private SpriteRenderer normalMapSr;

	private Renderer normalMapRenderer;

	private bool isSpriteRenderer;

	private string path;

	private string subPath;

	public void MakeNewMaterial(bool getShaderTypeFromPrefs, string shaderName = "AllIn1SpriteShader")
	{
		SetMaterial(AfterSetAction.Clear, getShaderTypeFromPrefs, shaderName);
	}

	public void MakeCopy()
	{
		SetMaterial(AfterSetAction.CopyMaterial, getShaderTypeFromPrefs: false, GetStringFromShaderType());
	}

	private void ResetAllProperties(bool getShaderTypeFromPrefs, string shaderName)
	{
		SetMaterial(AfterSetAction.Reset, getShaderTypeFromPrefs, shaderName);
	}

	private string GetStringFromShaderType()
	{
		if (shaderTypes == ShaderTypes.Default)
		{
			return "AllIn1SpriteShader";
		}
		if (shaderTypes == ShaderTypes.ScaledTime)
		{
			return "AllIn1SpriteShaderScaledTime";
		}
		if (shaderTypes == ShaderTypes.MaskedUI)
		{
			return "AllIn1SpriteShaderUiMask";
		}
		if (shaderTypes == ShaderTypes.Urp2dRenderer)
		{
			return "AllIn1Urp2dRenderer";
		}
		return "AllIn1SpriteShader";
	}

	private void SetMaterial(AfterSetAction action, bool getShaderTypeFromPrefs, string shaderName)
	{
		Shader shader = Resources.Load(shaderName, typeof(Shader)) as Shader;
		if (getShaderTypeFromPrefs)
		{
			switch (PlayerPrefs.GetInt("allIn1DefaultShader"))
			{
			case 1:
				shader = Resources.Load("AllIn1SpriteShaderScaledTime", typeof(Shader)) as Shader;
				break;
			case 2:
				shader = Resources.Load("AllIn1SpriteShaderUiMask", typeof(Shader)) as Shader;
				break;
			case 3:
				shader = Resources.Load("AllIn1Urp2dRenderer", typeof(Shader)) as Shader;
				break;
			}
		}
		if (!Application.isPlaying && Application.isEditor && shader != null)
		{
			bool flag = false;
			if (GetComponent<Renderer>() != null)
			{
				flag = true;
				prevMaterial = new Material(GetComponent<Renderer>().sharedMaterial);
				currMaterial = new Material(shader);
				GetComponent<Renderer>().sharedMaterial = currMaterial;
				GetComponent<Renderer>().sharedMaterial.hideFlags = HideFlags.None;
				matAssigned = true;
				DoAfterSetAction(action);
			}
			else
			{
				Graphic component = GetComponent<Graphic>();
				if (component != null)
				{
					flag = true;
					prevMaterial = new Material(component.material);
					currMaterial = new Material(shader);
					component.material = currMaterial;
					component.material.hideFlags = HideFlags.None;
					matAssigned = true;
					DoAfterSetAction(action);
				}
			}
			if (!flag)
			{
				MissingRenderer();
			}
			else
			{
				SetSceneDirty();
			}
		}
		else if (shader == null)
		{
			Debug.LogError("Make sure the AllIn1SpriteShader shader variants are inside the Resource folder!");
		}
	}

	private void DoAfterSetAction(AfterSetAction action)
	{
		switch (action)
		{
		case AfterSetAction.Clear:
			ClearAllKeywords();
			break;
		case AfterSetAction.CopyMaterial:
			currMaterial.CopyPropertiesFromMaterial(prevMaterial);
			break;
		}
	}

	public void TryCreateNew()
	{
		bool flag = false;
		Renderer component = GetComponent<Renderer>();
		if (component != null)
		{
			flag = true;
			if (component != null && component.sharedMaterial != null && component.sharedMaterial.name.Contains("AllIn1"))
			{
				ResetAllProperties(getShaderTypeFromPrefs: false, GetStringFromShaderType());
				ClearAllKeywords();
			}
			else
			{
				CleanMaterial();
				MakeNewMaterial(getShaderTypeFromPrefs: false, GetStringFromShaderType());
			}
		}
		else
		{
			Graphic component2 = GetComponent<Graphic>();
			if (component2 != null)
			{
				flag = true;
				if (component2.material.name.Contains("AllIn1"))
				{
					ResetAllProperties(getShaderTypeFromPrefs: false, GetStringFromShaderType());
					ClearAllKeywords();
				}
				else
				{
					MakeNewMaterial(getShaderTypeFromPrefs: false, GetStringFromShaderType());
				}
			}
		}
		if (!flag)
		{
			MissingRenderer();
		}
		SetSceneDirty();
	}

	public void ClearAllKeywords()
	{
		SetKeyword("RECTSIZE_ON");
		SetKeyword("OFFSETUV_ON");
		SetKeyword("CLIPPING_ON");
		SetKeyword("POLARUV_ON");
		SetKeyword("TWISTUV_ON");
		SetKeyword("ROTATEUV_ON");
		SetKeyword("FISHEYE_ON");
		SetKeyword("PINCH_ON");
		SetKeyword("SHAKEUV_ON");
		SetKeyword("WAVEUV_ON");
		SetKeyword("ROUNDWAVEUV_ON");
		SetKeyword("DOODLE_ON");
		SetKeyword("ZOOMUV_ON");
		SetKeyword("FADE_ON");
		SetKeyword("TEXTURESCROLL_ON");
		SetKeyword("GLOW_ON");
		SetKeyword("OUTBASE_ON");
		SetKeyword("ONLYOUTLINE_ON");
		SetKeyword("OUTTEX_ON");
		SetKeyword("OUTDIST_ON");
		SetKeyword("DISTORT_ON");
		SetKeyword("WIND_ON");
		SetKeyword("GRADIENT_ON");
		SetKeyword("GRADIENT2COL_ON");
		SetKeyword("RADIALGRADIENT_ON");
		SetKeyword("COLORSWAP_ON");
		SetKeyword("HSV_ON");
		SetKeyword("HITEFFECT_ON");
		SetKeyword("PIXELATE_ON");
		SetKeyword("NEGATIVE_ON");
		SetKeyword("GRADIENTCOLORRAMP_ON");
		SetKeyword("COLORRAMP_ON");
		SetKeyword("GREYSCALE_ON");
		SetKeyword("POSTERIZE_ON");
		SetKeyword("BLUR_ON");
		SetKeyword("MOTIONBLUR_ON");
		SetKeyword("GHOST_ON");
		SetKeyword("ALPHAOUTLINE_ON");
		SetKeyword("INNEROUTLINE_ON");
		SetKeyword("ONLYINNEROUTLINE_ON");
		SetKeyword("HOLOGRAM_ON");
		SetKeyword("CHROMABERR_ON");
		SetKeyword("GLITCH_ON");
		SetKeyword("FLICKER_ON");
		SetKeyword("SHADOW_ON");
		SetKeyword("SHINE_ON");
		SetKeyword("CONTRAST_ON");
		SetKeyword("OVERLAY_ON");
		SetKeyword("OVERLAYMULT_ON");
		SetKeyword("ALPHACUTOFF_ON");
		SetKeyword("ALPHAROUND_ON");
		SetKeyword("CHANGECOLOR_ON");
		SetKeyword("CHANGECOLOR2_ON");
		SetKeyword("CHANGECOLOR3_ON");
		SetKeyword("FOG_ON");
		SetSceneDirty();
	}

	private void SetKeyword(string keyword, bool state = false)
	{
		if (destroyed)
		{
			return;
		}
		if (currMaterial == null)
		{
			FindCurrMaterial();
			if (currMaterial == null)
			{
				MissingRenderer();
				return;
			}
		}
		if (!state)
		{
			currMaterial.DisableKeyword(keyword);
		}
		else
		{
			currMaterial.EnableKeyword(keyword);
		}
	}

	private void FindCurrMaterial()
	{
		if (GetComponent<Renderer>() != null)
		{
			currMaterial = GetComponent<Renderer>().sharedMaterial;
			matAssigned = true;
			return;
		}
		Graphic component = GetComponent<Graphic>();
		if (component != null)
		{
			currMaterial = component.material;
			matAssigned = true;
		}
	}

	public void CleanMaterial()
	{
		Renderer component = GetComponent<Renderer>();
		if (component != null)
		{
			component.sharedMaterial = new Material(Shader.Find("Sprites/Default"));
			matAssigned = false;
		}
		else
		{
			Graphic component2 = GetComponent<Graphic>();
			if (component2 != null)
			{
				component2.material = new Material(Shader.Find("Sprites/Default"));
				matAssigned = false;
			}
		}
		SetSceneDirty();
	}

	public void SaveMaterial()
	{
	}

	private void SaveMaterialWithOtherName(string path, int i = 1)
	{
		int num = i;
		string fileName = string.Concat(path + "_" + num, ".mat");
		if (File.Exists(fileName))
		{
			num++;
			SaveMaterialWithOtherName(path, num);
		}
		else
		{
			DoSaving(fileName);
		}
	}

	private void DoSaving(string fileName)
	{
	}

	public void SetSceneDirty()
	{
	}

	private void MissingRenderer()
	{
	}

	public void ToggleSetAtlasUvs(bool activate)
	{
		SetAtlasUvs setAtlasUvs = GetComponent<SetAtlasUvs>();
		if (activate)
		{
			if (setAtlasUvs == null)
			{
				setAtlasUvs = base.gameObject.AddComponent<SetAtlasUvs>();
			}
			setAtlasUvs.GetAndSetUVs();
			SetKeyword("ATLAS_ON", state: true);
		}
		else
		{
			if (setAtlasUvs != null)
			{
				setAtlasUvs.ResetAtlasUvs();
				Object.DestroyImmediate(setAtlasUvs);
			}
			SetKeyword("ATLAS_ON");
		}
		SetSceneDirty();
	}

	public void ApplyMaterialToHierarchy()
	{
		Renderer component = GetComponent<Renderer>();
		Graphic component2 = GetComponent<Graphic>();
		Material material = null;
		if (component != null)
		{
			material = component.sharedMaterial;
		}
		else
		{
			if (!(component2 != null))
			{
				MissingRenderer();
				return;
			}
			material = component2.material;
		}
		List<Transform> transforms = new List<Transform>();
		GetAllChildren(base.transform, ref transforms);
		foreach (Transform item in transforms)
		{
			component = item.gameObject.GetComponent<Renderer>();
			if (component != null)
			{
				component.material = material;
				continue;
			}
			component2 = item.gameObject.GetComponent<Graphic>();
			if (component2 != null)
			{
				component2.material = material;
			}
		}
	}

	public void CheckIfValidTarget()
	{
		Renderer component = GetComponent<Renderer>();
		Graphic component2 = GetComponent<Graphic>();
		if (component == null && component2 == null)
		{
			MissingRenderer();
		}
	}

	private void GetAllChildren(Transform parent, ref List<Transform> transforms)
	{
		foreach (Transform item in parent)
		{
			transforms.Add(item);
			GetAllChildren(item, ref transforms);
		}
	}

	public void RenderToImage()
	{
	}

	public void RenderAndSaveTexture(Material targetMaterial, Texture targetTexture)
	{
	}

	private string GetNewValidPath(string path, int i = 1)
	{
		int num = i;
		string result = string.Concat(path + "_" + num, ".png");
		if (File.Exists(result))
		{
			num++;
			result = GetNewValidPath(path, num);
		}
		return result;
	}

	protected virtual void OnEnable()
	{
	}

	protected virtual void OnDisable()
	{
	}

	protected virtual void OnEditorUpdate()
	{
		if (!computingNormal)
		{
			return;
		}
		if (needToWait)
		{
			waitingCycles++;
			if (waitingCycles > 5)
			{
				needToWait = false;
				timesWeWaited++;
			}
			return;
		}
		if (timesWeWaited == 1)
		{
			SetNewNormalTexture2();
		}
		if (timesWeWaited == 2)
		{
			SetNewNormalTexture3();
		}
		if (timesWeWaited == 3)
		{
			SetNewNormalTexture4();
		}
		needToWait = true;
	}

	public void CreateAndAssignNormalMap()
	{
	}

	private void SetNewNormalTexture()
	{
		computingNormal = false;
	}

	private void SetNewNormalTexture2()
	{
	}

	private void SetNewNormalTexture3()
	{
	}

	private void SetNewNormalTexture4()
	{
	}

	private Texture2D CreateNormalMap(Texture2D t, float normalMult = 5f, int normalSmooth = 0)
	{
		Color[] array = new Color[t.width * t.height];
		Texture2D texture2D = new Texture2D(t.width, t.height, TextureFormat.RGB24, mipChain: false, linear: false);
		Vector3 rhs = new Vector3(0.3333f, 0.3333f, 0.3333f);
		for (int i = 0; i < t.height; i++)
		{
			for (int j = 0; j < t.width; j++)
			{
				Color pixel = t.GetPixel(j - 1, i - 1);
				Vector3 lhs = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j, i - 1);
				Vector3 lhs2 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i - 1);
				Vector3 lhs3 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j - 1, i);
				Vector3 lhs4 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i);
				Vector3 lhs5 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j - 1, i + 1);
				Vector3 lhs6 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j, i + 1);
				Vector3 lhs7 = new Vector3(pixel.r, pixel.g, pixel.g);
				pixel = t.GetPixel(j + 1, i + 1);
				Vector3 lhs8 = new Vector3(pixel.r, pixel.g, pixel.g);
				float num = Vector3.Dot(lhs, rhs);
				float num2 = Vector3.Dot(lhs2, rhs);
				float num3 = Vector3.Dot(lhs3, rhs);
				float num4 = Vector3.Dot(lhs4, rhs);
				float num5 = Vector3.Dot(lhs5, rhs);
				float num6 = Vector3.Dot(lhs6, rhs);
				float num7 = Vector3.Dot(lhs7, rhs);
				float num8 = Vector3.Dot(lhs8, rhs);
				float x = (num - num3) * 0.25f + (num4 - num5) * 0.5f + (num6 - num8) * 0.25f;
				float y = (num - num6) * 0.25f + (num2 - num7) * 0.5f + (num3 - num8) * 0.25f;
				Vector2 vector = new Vector2(x, y) * normalMult;
				Vector3 normalized = new Vector3(vector.x, vector.y, 1f).normalized;
				Color color = new Color(normalized.x * 0.5f + 0.5f, normalized.y * 0.5f + 0.5f, normalized.z * 0.5f + 0.5f, 1f);
				array[j + i * t.width] = color;
			}
		}
		if ((float)normalSmooth > 0f)
		{
			for (int k = 0; k < t.height; k++)
			{
				for (int l = 0; l < t.width; l++)
				{
					float num9 = 0f;
					Color color2 = array[l + k * t.width];
					num9 += 1f;
					if (l - normalSmooth > 0)
					{
						if (k - normalSmooth > 0)
						{
							color2 += array[l - normalSmooth + (k - normalSmooth) * t.width];
							num9 += 1f;
						}
						color2 += array[l - normalSmooth + k * t.width];
						num9 += 1f;
						if (k + normalSmooth < t.height)
						{
							color2 += array[l - normalSmooth + (k + normalSmooth) * t.width];
							num9 += 1f;
						}
					}
					if (k - normalSmooth > 0)
					{
						color2 += array[l + (k - normalSmooth) * t.width];
						num9 += 1f;
					}
					if (k + normalSmooth < t.height)
					{
						color2 += array[l + (k + normalSmooth) * t.width];
						num9 += 1f;
					}
					if (l + normalSmooth < t.width)
					{
						if (k - normalSmooth > 0)
						{
							color2 += array[l + normalSmooth + (k - normalSmooth) * t.width];
							num9 += 1f;
						}
						color2 += array[l + normalSmooth + k * t.width];
						num9 += 1f;
						if (k + normalSmooth < t.height)
						{
							color2 += array[l + normalSmooth + (k + normalSmooth) * t.width];
							num9 += 1f;
						}
					}
					array[l + k * t.width] = color2 / num9;
				}
			}
		}
		texture2D.SetPixels(array);
		texture2D.Apply();
		return texture2D;
	}
}
