using UnityEngine;

public class characterGFXScript : MonoBehaviour
{
	public bool male;

	public GameObject boneHead;

	public GameObject objectSkin;

	public GameObject objectHose;

	public GameObject objectShirt;

	public GameObject objectAdd1;

	public GameObject addTasse1;

	public GameObject addMuel1;

	public GameObject addGiesskanne1;

	public GameObject addBook1;

	public GameObject addTelefon1;

	public GameObject addDartPfeil1;

	public GameObject addController1;

	public GameObject addStift1;

	public GameObject addHammer1;

	public GameObject addSchraubenzieher1;

	public GameObject addGolf;

	public GameObject myLOD;

	public int[] myMaterialSlots;

	public Material[] myMaterials;

	private clothScript clothScript_;

	private GameObject myEyes;

	private GameObject myHair;

	private GameObject myBeard;

	private GameObject myHat;

	private int indexSkinColor;

	private int indexHairColor;

	private characterScript cS_;

	private void Start()
	{
	}

	public void Init(bool forcedClothes)
	{
		movementScript component = base.transform.root.GetComponent<movementScript>();
		if ((bool)myLOD)
		{
			component.charAnimationLOD = myLOD.GetComponent<Animator>();
		}
		cS_ = base.transform.root.GetComponent<characterScript>();
		cS_.male = male;
		cS_.myRenderer = objectSkin.GetComponent<SkinnedMeshRenderer>();
		if ((bool)myLOD)
		{
			cS_.myLodRenderer = myLOD.transform.GetChild(0).GetComponent<SkinnedMeshRenderer>();
		}
		if ((bool)addTasse1)
		{
			cS_.addObjects[0] = addTasse1;
			cS_.addObjects[0].SetActive(value: false);
		}
		if ((bool)addMuel1)
		{
			cS_.addObjects[1] = addMuel1;
			cS_.addObjects[1].SetActive(value: false);
		}
		if ((bool)addGiesskanne1)
		{
			cS_.addObjects[2] = addGiesskanne1;
			cS_.addObjects[2].SetActive(value: false);
		}
		if ((bool)addBook1)
		{
			cS_.addObjects[3] = addBook1;
			cS_.addObjects[3].SetActive(value: false);
		}
		if ((bool)addTelefon1)
		{
			cS_.addObjects[4] = addTelefon1;
			cS_.addObjects[4].SetActive(value: false);
		}
		if ((bool)addDartPfeil1)
		{
			cS_.addObjects[5] = addDartPfeil1;
			cS_.addObjects[5].SetActive(value: false);
		}
		if ((bool)addController1)
		{
			cS_.addObjects[6] = addController1;
			cS_.addObjects[6].SetActive(value: false);
		}
		if ((bool)addStift1)
		{
			cS_.addObjects[7] = addStift1;
			cS_.addObjects[7].SetActive(value: false);
		}
		if ((bool)addHammer1)
		{
			cS_.addObjects[8] = addHammer1;
			cS_.addObjects[8].SetActive(value: false);
		}
		if ((bool)addSchraubenzieher1)
		{
			cS_.addObjects[9] = addSchraubenzieher1;
			cS_.addObjects[9].SetActive(value: false);
		}
		if ((bool)addGolf)
		{
			cS_.addObjects[10] = addGolf;
			cS_.addObjects[10].SetActive(value: false);
		}
		clothScript_ = GameObject.FindGameObjectWithTag("Main").GetComponent<clothScript>();
		SetEyes(forcedClothes, cS_.model_eyes);
		SetHairs(forcedClothes, cS_.model_hair);
		SetBeard(forcedClothes, cS_.model_beard);
		SetSkinColor(forcedClothes, cS_.model_skinColor);
		SetHairColor(forcedClothes, cS_.model_hairColor, cS_.model_beardColor);
		SetClothColors(forcedClothes, cS_.model_HoseColor, cS_.model_ShirtColor, cS_.model_Add1Color);
		if (myMaterialSlots.Length != 0)
		{
			objectSkin.GetComponent<SkinnedMeshRenderer>().sharedMaterials = myMaterials;
		}
		Object.Destroy(this);
	}

	private void SetEyes(bool force, int model1)
	{
		int num = 0;
		if (!force)
		{
			if (male)
			{
				if (Random.Range(0, 100) < 20)
				{
					num = Random.Range(1, clothScript_.prefabMaleEyes.Length);
				}
				myEyes = Object.Instantiate(clothScript_.prefabMaleEyes[num], boneHead.transform);
				cS_.model_eyes = num;
			}
			else
			{
				if (Random.Range(0, 100) < 20)
				{
					num = Random.Range(1, clothScript_.prefabFemaleEyes.Length);
				}
				myEyes = Object.Instantiate(clothScript_.prefabFemaleEyes[num], boneHead.transform);
				cS_.model_eyes = num;
			}
		}
		else
		{
			if (model1 <= -1)
			{
				return;
			}
			if (male)
			{
				if (model1 >= clothScript_.prefabMaleEyes.Length)
				{
					model1 = 0;
				}
				myEyes = Object.Instantiate(clothScript_.prefabMaleEyes[model1], boneHead.transform);
			}
			else
			{
				if (model1 >= clothScript_.prefabFemaleEyes.Length)
				{
					model1 = 0;
				}
				myEyes = Object.Instantiate(clothScript_.prefabFemaleEyes[model1], boneHead.transform);
			}
		}
	}

	private void SetHairs(bool force, int model1)
	{
		int num = 0;
		if (!force)
		{
			if (male)
			{
				if (Random.Range(0, 100) >= 10)
				{
					num = Random.Range(0, clothScript_.prefabMaleHairs.Length);
					myHair = Object.Instantiate(clothScript_.prefabMaleHairs[num], boneHead.transform);
					cS_.model_hair = num;
				}
			}
			else
			{
				num = Random.Range(0, clothScript_.prefabFemaleHairs.Length);
				myHair = Object.Instantiate(clothScript_.prefabFemaleHairs[num], boneHead.transform);
				cS_.model_hair = num;
			}
		}
		else
		{
			if (model1 <= -1)
			{
				return;
			}
			if (male)
			{
				if (model1 >= clothScript_.prefabMaleHairs.Length)
				{
					model1 = 0;
				}
				myHair = Object.Instantiate(clothScript_.prefabMaleHairs[model1], boneHead.transform);
			}
			else
			{
				if (model1 >= clothScript_.prefabFemaleHairs.Length)
				{
					model1 = 0;
				}
				myHair = Object.Instantiate(clothScript_.prefabFemaleHairs[model1], boneHead.transform);
			}
		}
	}

	private void SetBeard(bool force, int model1)
	{
		if (!male)
		{
			return;
		}
		if (!force)
		{
			if (Random.Range(0, 100) < 33)
			{
				int num = Random.Range(0, clothScript_.prefabBeards.Length);
				myBeard = Object.Instantiate(clothScript_.prefabBeards[num], boneHead.transform);
				cS_.model_beard = num;
			}
		}
		else if (model1 > -1)
		{
			if (model1 >= clothScript_.prefabBeards.Length)
			{
				model1 = 0;
			}
			myBeard = Object.Instantiate(clothScript_.prefabBeards[model1], boneHead.transform);
		}
	}

	private void SetSkinColor(bool force, int color1)
	{
		indexSkinColor = 0;
		if (!force)
		{
			if (Random.Range(0, 100) < 60)
			{
				int num = Random.Range(0, clothScript_.matColor_Skin.Length);
				myMaterials[myMaterialSlots[0]] = clothScript_.matColor_Skin[num];
				cS_.model_skinColor = num;
			}
			else
			{
				cS_.model_skinColor = 0;
			}
		}
		else
		{
			if (color1 >= clothScript_.matColor_Skin.Length)
			{
				color1 = 0;
			}
			myMaterials[myMaterialSlots[0]] = clothScript_.matColor_Skin[color1];
		}
	}

	private void SetHairColor(bool force, int colorHair, int colorBeard)
	{
		if (!force)
		{
			if (male)
			{
				indexHairColor = Random.Range(0, clothScript_.matColor_MaleHair.Length);
				if ((bool)myHair)
				{
					myHair.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_MaleHair[indexHairColor];
					cS_.model_hairColor = indexHairColor;
				}
				if ((bool)myBeard)
				{
					myBeard.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_MaleHair[indexHairColor];
					cS_.model_beardColor = indexHairColor;
				}
			}
			else
			{
				indexHairColor = Random.Range(0, clothScript_.matColor_FemaleHair.Length);
				if ((bool)myHair)
				{
					myHair.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_FemaleHair[indexHairColor];
					cS_.model_hairColor = indexHairColor;
				}
				if ((bool)myBeard)
				{
					myBeard.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_FemaleHair[indexHairColor];
					cS_.model_beardColor = indexHairColor;
				}
			}
			return;
		}
		if (male)
		{
			if ((bool)myHair)
			{
				if (colorHair >= clothScript_.matColor_MaleHair.Length)
				{
					colorHair = 0;
				}
				myHair.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_MaleHair[colorHair];
			}
			if ((bool)myBeard)
			{
				if (colorBeard >= clothScript_.matColor_MaleHair.Length)
				{
					colorBeard = 0;
				}
				myBeard.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_MaleHair[colorBeard];
			}
			return;
		}
		if ((bool)myHair)
		{
			if (colorHair >= clothScript_.matColor_FemaleHair.Length)
			{
				colorHair = 0;
			}
			myHair.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_FemaleHair[colorHair];
		}
		if ((bool)myBeard)
		{
			if (colorBeard >= clothScript_.matColor_FemaleHair.Length)
			{
				colorBeard = 0;
			}
			myBeard.GetComponentInChildren<Renderer>().sharedMaterial = clothScript_.matColor_FemaleHair[colorBeard];
		}
	}

	private void SetClothColors(bool force, int colorHose, int colorShirt, int colorAdd1)
	{
		if (!force)
		{
			int num = 0;
			if (male)
			{
				num = Random.Range(0, clothScript_.matColor_MaleHose.Length);
				myMaterials[myMaterialSlots[2]] = clothScript_.matColor_MaleHose[num];
				cS_.model_HoseColor = num;
			}
			else
			{
				num = Random.Range(0, clothScript_.matColor_FemaleHose.Length);
				myMaterials[myMaterialSlots[2]] = clothScript_.matColor_FemaleHose[num];
				cS_.model_HoseColor = num;
			}
			if (male)
			{
				num = Random.Range(0, clothScript_.matColor_MaleShirt.Length);
				myMaterials[myMaterialSlots[1]] = clothScript_.matColor_MaleShirt[num];
				cS_.model_ShirtColor = num;
			}
			else
			{
				num = Random.Range(0, clothScript_.matColor_FemaleShirt.Length);
				myMaterials[myMaterialSlots[1]] = clothScript_.matColor_FemaleShirt[num];
				cS_.model_ShirtColor = num;
			}
			if (myMaterialSlots.Length >= 5)
			{
				num = Random.Range(0, clothScript_.matColor_AllColors.Length);
				myMaterials[myMaterialSlots[4]] = clothScript_.matColor_AllColors[num];
				cS_.model_Add1Color = num;
			}
			return;
		}
		if (male)
		{
			if (colorHose >= clothScript_.matColor_MaleHose.Length)
			{
				colorHose = 0;
			}
			myMaterials[myMaterialSlots[2]] = clothScript_.matColor_MaleHose[colorHose];
		}
		else
		{
			if (colorHose >= clothScript_.matColor_FemaleHose.Length)
			{
				colorHose = 0;
			}
			myMaterials[myMaterialSlots[2]] = clothScript_.matColor_FemaleHose[colorHose];
		}
		if (male)
		{
			if (colorShirt >= clothScript_.matColor_MaleShirt.Length)
			{
				colorShirt = 0;
			}
			myMaterials[myMaterialSlots[1]] = clothScript_.matColor_MaleShirt[colorShirt];
		}
		else
		{
			if (colorShirt >= clothScript_.matColor_FemaleShirt.Length)
			{
				colorShirt = 0;
			}
			myMaterials[myMaterialSlots[1]] = clothScript_.matColor_FemaleShirt[colorShirt];
		}
		if (myMaterialSlots.Length >= 5)
		{
			if (colorAdd1 >= clothScript_.matColor_AllColors.Length)
			{
				colorAdd1 = 0;
			}
			myMaterials[myMaterialSlots[4]] = clothScript_.matColor_AllColors[colorAdd1];
		}
	}
}
