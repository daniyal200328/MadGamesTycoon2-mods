using System.Collections.Generic;
using BrainFailProductions.PolyFewRuntime;
using UnityEngine;

namespace BrainFailProductions.PolyFew;

[ExecuteInEditMode]
public class ObjectMaterialLinks : MonoBehaviour
{
	[SerializeField]
	private List<CombiningInformation.MaterialEntity> linkedEntities;

	public List<PolyfewRuntime.MaterialProperties> materialsProperties;

	public Texture2D linkedAttrImg;

	public List<CombiningInformation.MaterialEntity> linkedMaterialEntities
	{
		get
		{
			return linkedEntities;
		}
		set
		{
			linkedEntities = value;
			if (value == null)
			{
				return;
			}
			materialsProperties = new List<PolyfewRuntime.MaterialProperties>();
			for (int i = 0; i < value.Count; i++)
			{
				CombiningInformation.MaterialEntity materialEntity = value[i];
				if (materialEntity != null)
				{
					for (int j = 0; j < materialEntity.combinedMats.Count; j++)
					{
						CombiningInformation.MaterialProperties materialProperties = materialEntity.combinedMats[j].materialProperties;
						PolyfewRuntime.MaterialProperties item = new PolyfewRuntime.MaterialProperties(materialProperties.texArrIndex, materialProperties.matIndex, materialProperties.materialName, materialProperties.originalMaterial, materialProperties.albedoTint, materialProperties.uvTileOffset, materialProperties.normalIntensity, materialProperties.occlusionIntensity, materialProperties.smoothnessIntensity, materialProperties.glossMapScale, materialProperties.metalIntensity, materialProperties.emissionColor, materialProperties.detailUVTileOffset, materialProperties.alphaCutoff, materialProperties.specularColor, materialProperties.detailNormalScale, materialProperties.heightIntensity, materialProperties.uvSec);
						materialsProperties.Add(item);
					}
				}
			}
		}
	}

	private void Start()
	{
		MeshRenderer component = GetComponent<MeshRenderer>();
		SkinnedMeshRenderer component2 = GetComponent<SkinnedMeshRenderer>();
		if (component != null)
		{
			Material[] sharedMaterials = component.sharedMaterials;
			if (sharedMaterials != null && sharedMaterials.Length != 0)
			{
				bool flag = false;
				Material[] array = sharedMaterials;
				foreach (Material material in array)
				{
					if (!(material == null))
					{
						string text = material.shader.name.ToLower();
						if (text == "batchfewstandard" || text == "batchfewstandardspecular")
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					Object.DestroyImmediate(this);
				}
			}
			else
			{
				Object.DestroyImmediate(this);
			}
		}
		else if (component2 != null)
		{
			Material[] sharedMaterials = component2.sharedMaterials;
			if (sharedMaterials != null && sharedMaterials.Length != 0)
			{
				bool flag2 = false;
				Material[] array = sharedMaterials;
				foreach (Material material2 in array)
				{
					if (!(material2 == null))
					{
						string text2 = material2.shader.name.ToLower();
						if (text2 == "batchfewstandard" || text2 == "batchfewstandardspecular")
						{
							flag2 = true;
							break;
						}
					}
				}
				if (!flag2)
				{
					Object.DestroyImmediate(this);
				}
			}
			else
			{
				Object.DestroyImmediate(this);
			}
		}
		else
		{
			Object.DestroyImmediate(this);
		}
	}
}
