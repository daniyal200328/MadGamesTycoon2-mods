using System;
using System.Collections.Generic;
using UnityEngine;

namespace BrainFailProductions.PolyFew;

public class CombiningInformation
{
	public enum DiffuseColorSpace
	{
		NON_LINEAR,
		LINEAR
	}

	public enum CompressionType
	{
		UNCOMPRESSED,
		DXT1,
		ETC2_RGB,
		PVRTC_RGB4,
		ASTC_RGB
	}

	public enum CompressionQuality
	{
		LOW,
		MEDIUM,
		HIGH
	}

	[Serializable]
	public struct Resolution
	{
		public int width;

		public int height;
	}

	[Serializable]
	public class TextureArrayUserSettings
	{
		public Resolution resolution;

		public FilterMode filteringMode;

		public CompressionType compressionType;

		public CompressionQuality compressionQuality;

		public int anisotropicFilteringLevel;

		public int choiceResolutionW = 4;

		public int choiceResolutionH = 4;

		public int choiceFilteringMode;

		public int choiceCompressionQuality = 1;

		public int choiceCompressionType;

		public TextureArrayUserSettings(Resolution resolution, FilterMode filteringMode, CompressionType compressionType, CompressionQuality compressionQuality = CompressionQuality.MEDIUM, int anisotropicFilteringLevel = 1)
		{
			this.resolution = resolution;
			this.filteringMode = filteringMode;
			this.compressionType = compressionType;
			this.compressionQuality = compressionQuality;
			this.anisotropicFilteringLevel = anisotropicFilteringLevel;
		}
	}

	[Serializable]
	public class TextureArrayGroup
	{
		public TextureArrayUserSettings diffuseArraySettings;

		public TextureArrayUserSettings metallicArraySettings;

		public TextureArrayUserSettings specularArraySettings;

		public TextureArrayUserSettings normalArraySettings;

		public TextureArrayUserSettings heightArraySettings;

		public TextureArrayUserSettings occlusionArraySettings;

		public TextureArrayUserSettings emissiveArraySettings;

		public TextureArrayUserSettings detailMaskArraySettings;

		public TextureArrayUserSettings detailAlbedoArraySettings;

		public TextureArrayUserSettings detailNormalArraySettings;

		public void InitializeDefaultArraySettings(Resolution resolution, FilterMode filteringMode, CompressionType compressionType, CompressionQuality compressionQuality = CompressionQuality.MEDIUM, int anisotropicFilteringLevel = 1)
		{
			diffuseArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			metallicArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			specularArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			normalArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			heightArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			occlusionArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			emissiveArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			detailMaskArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			detailAlbedoArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
			detailNormalArraySettings = new TextureArrayUserSettings(resolution, filteringMode, compressionType, compressionQuality, anisotropicFilteringLevel);
		}
	}

	[Serializable]
	public class MaterialProperties
	{
		public bool foldOut = true;

		public int texArrIndex;

		public int matIndex;

		public string materialName;

		public Material originalMaterial;

		public Color albedoTint;

		public Vector4 uvTileOffset = new Vector4(1f, 1f, 0f, 0f);

		public float normalIntensity = 1f;

		public float occlusionIntensity = 1f;

		public float smoothnessIntensity = 1f;

		public float glossMapScale = 1f;

		public float metalIntensity = 1f;

		public Color emissionColor = Color.black;

		public Vector4 detailUVTileOffset = new Vector4(1f, 1f, 0f, 0f);

		public float alphaCutoff = 0.5f;

		public Color specularColor = Color.black;

		public float detailNormalScale = 1f;

		public float heightIntensity = 0.05f;

		public float uvSec;

		public int alphaMode;

		public bool specularWorkflow;

		public bool IsSameAs(MaterialProperties toCompare)
		{
			if (originalMaterial == toCompare.originalMaterial)
			{
				return true;
			}
			if (toCompare.albedoTint != albedoTint)
			{
				return false;
			}
			if (toCompare.normalIntensity != normalIntensity)
			{
				return false;
			}
			if (toCompare.occlusionIntensity != occlusionIntensity)
			{
				return false;
			}
			if (toCompare.smoothnessIntensity != smoothnessIntensity)
			{
				return false;
			}
			if (toCompare.glossMapScale != glossMapScale)
			{
				return false;
			}
			if (toCompare.uvTileOffset != uvTileOffset)
			{
				return false;
			}
			if (toCompare.metalIntensity != metalIntensity)
			{
				return false;
			}
			if (toCompare.emissionColor != emissionColor)
			{
				return false;
			}
			if (toCompare.detailUVTileOffset != detailUVTileOffset)
			{
				return false;
			}
			if (toCompare.alphaCutoff != alphaCutoff)
			{
				return false;
			}
			if (toCompare.specularColor != specularColor)
			{
				return false;
			}
			if (toCompare.detailNormalScale != detailNormalScale)
			{
				return false;
			}
			if (toCompare.heightIntensity != heightIntensity)
			{
				return false;
			}
			if (toCompare.uvSec != uvSec)
			{
				return false;
			}
			if (toCompare.alphaMode != alphaMode)
			{
				return false;
			}
			return true;
		}

		public static Texture2D NewTexture()
		{
			Texture2D texture2D = new Texture2D(8, 4, TextureFormat.RGBAHalf, mipChain: false, linear: true);
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 4; j++)
				{
					texture2D.SetPixel(i, j, Color.black);
				}
			}
			texture2D.Apply();
			return texture2D;
		}

		public void BurnAttrToImg(ref Texture2D burnOn, int index, int textureArrayIndex)
		{
			if (index >= burnOn.height)
			{
				Texture2D texture2D = new Texture2D(burnOn.width, index + 1, TextureFormat.RGBAHalf, mipChain: false, linear: true);
				Color[] pixels = burnOn.GetPixels();
				texture2D.SetPixels(0, 0, burnOn.width, burnOn.height, pixels);
				burnOn = texture2D;
			}
			if (burnOn.width < 8)
			{
				Texture2D texture2D2 = new Texture2D(8, burnOn.height, TextureFormat.RGBAHalf, mipChain: false, linear: true);
				Color[] pixels2 = burnOn.GetPixels();
				texture2D2.SetPixels(0, 0, burnOn.width, burnOn.height, pixels2);
				burnOn = texture2D2;
			}
			burnOn.SetPixel(0, index, new Color(uvTileOffset.x - 1f, uvTileOffset.y - 1f, uvTileOffset.z, uvTileOffset.w));
			burnOn.SetPixel(1, index, new Color(normalIntensity, occlusionIntensity, smoothnessIntensity, metalIntensity));
			burnOn.SetPixel(2, index, albedoTint);
			burnOn.SetPixel(3, index, emissionColor);
			burnOn.SetPixel(4, index, new Color(specularColor.r, specularColor.g, specularColor.b, glossMapScale));
			burnOn.SetPixel(5, index, new Color(detailUVTileOffset.x, detailUVTileOffset.y, detailUVTileOffset.z, detailUVTileOffset.w));
			burnOn.SetPixel(6, index, new Color(alphaCutoff, detailNormalScale, heightIntensity, uvSec));
			burnOn.SetPixel(7, index, new Color(textureArrayIndex, textureArrayIndex, textureArrayIndex, textureArrayIndex));
			burnOn.Apply();
		}

		public void FillPropertiesFromMaterial(Material material, CombiningInformation combineInfo)
		{
			materialName = material.name;
			originalMaterial = material;
			normalIntensity = 1f;
			occlusionIntensity = 1f;
			smoothnessIntensity = 1f;
			albedoTint = Color.white;
			metalIntensity = 1f;
			uvTileOffset = new Vector4(1f, 1f, 0f, 0f);
			detailUVTileOffset = new Vector4(1f, 1f, 0f, 0f);
			emissionColor = Color.black;
			alphaCutoff = 0.5f;
			specularColor = Color.black;
			detailNormalScale = 1f;
			heightIntensity = 0.05f;
			alphaMode = 0;
			glossMapScale = 0f;
			if (material.shader.name.ToLower() == "standard (specular setup)")
			{
				specularWorkflow = true;
			}
			if (material.HasProperty("_Color"))
			{
				albedoTint = material.GetColor("_Color");
			}
			if (material.HasProperty("_MainTex") && material.HasProperty("_MainTex_ST"))
			{
				uvTileOffset = material.GetVector("_MainTex_ST");
			}
			if (material.HasProperty("_GlossMapScale"))
			{
				glossMapScale = material.GetFloat("_GlossMapScale");
			}
			if (material.HasProperty("_Glossiness"))
			{
				smoothnessIntensity = material.GetFloat("_Glossiness");
			}
			if (material.HasProperty("_Smoothness"))
			{
				smoothnessIntensity = material.GetFloat("_Smoothness");
			}
			if (material.HasProperty("_MetallicGlossMap") && material.GetTexture("_MetallicGlossMap") != null)
			{
				smoothnessIntensity = glossMapScale;
			}
			if (material.HasProperty("_SpecColor"))
			{
				specularColor = material.GetColor("_SpecColor");
			}
			if (material.HasProperty("_Metallic"))
			{
				metalIntensity = material.GetFloat("_Metallic");
			}
			if (material.HasProperty("_OcclusionStrength"))
			{
				occlusionIntensity = material.GetFloat("_OcclusionStrength") + 1f;
			}
			if (material.HasProperty("_BumpScale"))
			{
				normalIntensity = material.GetFloat("_BumpScale");
			}
			if (material.HasProperty("_DetailNormalMapScale"))
			{
				detailNormalScale = material.GetFloat("_DetailNormalMapScale");
			}
			if (material.HasProperty("_EmissionColor") && material.HasProperty("_EmissionMap") && combineInfo.ShouldGenerateEmissionArray())
			{
				emissionColor = Color.black;
			}
			else if (material.HasProperty("_EmissionColor"))
			{
				emissionColor = material.GetColor("_EmissionColor");
			}
			if (material.HasProperty("_Parallax"))
			{
				heightIntensity = material.GetFloat("_Parallax");
			}
			if (material.HasProperty("_UVSec"))
			{
				uvSec = material.GetFloat("_UVSec");
			}
			if (material.HasProperty("_DetailAlbedoMap") && material.HasProperty("_DetailAlbedoMap_ST"))
			{
				detailUVTileOffset = material.GetVector("_DetailAlbedoMap_ST");
			}
			if (material.HasProperty("_Mode"))
			{
				alphaMode = (int)material.GetFloat("_Mode");
			}
		}
	}

	[Serializable]
	public class MeshData
	{
		public List<MeshFilter> meshFilters;

		public List<MeshRenderer> meshRenderers;

		public List<SkinnedMeshRenderer> skinnedMeshRenderers;

		public Material[] originalMaterials;

		public Mesh[] outputMeshes;

		public Matrix4x4[] outputMatrices;
	}

	[Serializable]
	public class CombineMetaData
	{
		public Material material;

		public MaterialProperties materialProperties;

		public MaterialProperties tempMaterialProperties;

		public List<MeshData> meshesData = new List<MeshData>();
	}

	[Serializable]
	public class MaterialEntity
	{
		public List<CombineMetaData> combinedMats = new List<CombineMetaData>();

		public int textArrIndex;

		public Texture2D diffuseMap;

		public Texture2D metallicMap;

		public Texture2D specularMap;

		public Texture2D normalMap;

		public Texture2D heightMap;

		public Texture2D occlusionMap;

		public Texture2D emissionMap;

		public Texture2D detailMaskMap;

		public Texture2D detailAlbedoMap;

		public Texture2D detailNormalMap;

		public bool HasAnyTextures()
		{
			if (!(diffuseMap != null) && !(heightMap != null) && !(normalMap != null) && !(metallicMap != null) && !(detailAlbedoMap != null) && !(detailNormalMap != null) && !(detailMaskMap != null) && !(emissionMap != null) && !(specularMap != null))
			{
				return occlusionMap != null;
			}
			return true;
		}
	}

	public List<MaterialEntity> materialEntities = new List<MaterialEntity>();

	public TextureArrayGroup textureArraysSettings = new TextureArrayGroup();

	public DiffuseColorSpace diffuseColorSpace;

	public Material[] combinedMaterials;

	public bool ShouldGenerateMetallicArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.metallicMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateSpecularArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.specularMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateNormalArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.normalMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateHeightArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.heightMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateOcclusionArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.occlusionMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateEmissionArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.emissionMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateDetailMaskArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.detailMaskMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateDetailAlbedoArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.detailAlbedoMap != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ShouldGenerateDetailNormalArray()
	{
		foreach (MaterialEntity materialEntity in materialEntities)
		{
			if (materialEntity.detailNormalMap != null)
			{
				return true;
			}
		}
		return false;
	}
}
