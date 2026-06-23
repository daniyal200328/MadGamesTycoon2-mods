using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ChangeGameplayFeatures : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

	private platforms platforms_;

	private Menu_DevGame menuDevGame_;

	public gameScript gS_;

	public int g_GameSize;

	public int g_GameMainGenre;

	public int g_GameSubGenre;

	public bool[] g_GameGameplayFeatures;

	private string searchStringA = "";

	private bool allFeatures;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	public void Init(gameScript game_)
	{
		allFeatures = false;
		FindScripts();
		gS_ = game_;
		CopyGameData();
		InitDropdowns_GameplayFeatures();
		Init_GameplayFeatures();
		uiObjects[0].GetComponent<Image>().sprite = games_.gameSizeSprites[gS_.gameSize];
		UpdateGesamtGameplayFeatures();
		CalcDevCosts();
	}

	private void Update()
	{
		if (uiObjects[7].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[8].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void CopyGameData()
	{
		g_GameSize = gS_.gameSize;
		g_GameMainGenre = gS_.maingenre;
		g_GameSubGenre = gS_.subgenre;
		g_GameGameplayFeatures = (bool[])gS_.gameGameplayFeatures.Clone();
	}

	public void InitDropdowns_GameplayFeatures()
	{
		int value = PlayerPrefs.GetInt(uiObjects[2].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(6));
		list.Add(tS_.GetText(413));
		list.Add(tS_.GetText(1438));
		uiObjects[2].GetComponent<Dropdown>().ClearOptions();
		uiObjects[2].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[2].GetComponent<Dropdown>().value = value;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_Ok()
	{
		if (UpdateGesamtGameplayFeatures() > menuDevGame_.maxFeatures_gameSize[g_GameSize])
		{
			guiMain_.MessageBox(tS_.GetText(1724), closeMenu: false);
			return;
		}
		bool flag = true;
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + gS_.gamePlatform[i]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component && !component.internet)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			string text = "";
			for (int j = 0; j < g_GameGameplayFeatures.Length; j++)
			{
				if (g_GameGameplayFeatures[j] && gF_.gameplayFeatures_INTERNET[j])
				{
					text = text + "\n<color=blue>" + gF_.GetName(j) + "</color>";
				}
			}
			if (text.Length > 0)
			{
				guiMain_.MessageBox(tS_.GetText(2198) + text, closeMenu: false);
				return;
			}
		}
		for (int k = 0; k < g_GameGameplayFeatures.Length; k++)
		{
			if (g_GameGameplayFeatures[k] && gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k] != -1 && !g_GameGameplayFeatures[gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k]])
			{
				string text2 = tS_.GetText(2197);
				text2 = text2.Replace("<NAME1>", "<color=blue>" + gF_.GetName(k) + "</color>");
				text2 = text2.Replace("<NAME2>", "<color=blue>" + gF_.GetName(gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k]) + "</color>");
				guiMain_.MessageBox(text2, closeMenu: false);
				return;
			}
		}
		int num = CalcDevCosts();
		mS_.Pay(num, 10);
		gS_.costs_entwicklung += num;
		for (int l = 0; l < g_GameGameplayFeatures.Length; l++)
		{
			if (!gS_.gameplayFeatures_DevDone[l] && !gS_.gameGameplayFeatures[l] && g_GameGameplayFeatures[l])
			{
				float points = gF_.GetDevPoints(l);
				points = gS_.CalcPlatformComplex(points);
				gS_.devPointsStart_Gesamt += points;
				gS_.devPoints_Gesamt += points;
			}
		}
		gS_.gameGameplayFeatures = (bool[])g_GameGameplayFeatures.Clone();
		if (gS_.devPoints <= 0f)
		{
			gS_.FindNextFeatureForDevelopment();
		}
		BUTTON_Close();
	}

	private int CalcDevCosts()
	{
		int num = 0;
		num += CalcDevCosts_Kontent();
		float num2 = GetPreisnachlass() * 100f;
		if (num2 > 0f)
		{
			tS_.GetText(624).Replace("<NUM>", mS_.GetMoney(num, showDollar: true));
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true) + " (" + Mathf.RoundToInt(100f - num2) + "%)";
			return num;
		}
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		return num;
	}

	private float GetPreisnachlass()
	{
		float num = 0f;
		if (gS_.typ_remaster)
		{
			num += 0.2f;
		}
		if (gS_.handy)
		{
			num += 0.25f;
		}
		return num;
	}

	private int CalcDevCosts_Kontent()
	{
		int num = 0;
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i] && !gS_.gameplayFeatures_DevDone[i] && !gS_.gameGameplayFeatures[i])
			{
				num += gF_.GetDevCosts(i);
			}
		}
		if (gS_.typ_remaster)
		{
			num /= 2;
		}
		if (gS_.handy)
		{
			num /= 4;
		}
		return num;
	}

	private int UpdateGesamtGameplayFeatures()
	{
		int num = 0;
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i])
			{
				num++;
			}
		}
		int num2 = 1;
		if (g_GameSize > 0)
		{
			num2 = menuDevGame_.maxFeatures_gameSize[g_GameSize - 1] / 2;
		}
		if (g_GameSize < 4)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " / " + menuDevGame_.maxFeatures_gameSize[g_GameSize] + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		if (num > menuDevGame_.maxFeatures_gameSize[g_GameSize])
		{
			uiObjects[1].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[1].GetComponent<Text>().color = Color.black;
		}
		return num;
	}

	private void Init_GameplayFeatures()
	{
		FindScripts();
		if (g_GameGameplayFeatures.Length == 0)
		{
			g_GameGameplayFeatures = new bool[gF_.gameplayFeatures_LEVEL.Length];
		}
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[3].transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < gF_.gameplayFeatures_LEVEL.Length; j++)
		{
			if (gF_.IsErforscht(j))
			{
				string text = gF_.GetName(j);
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[4].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Item_DevGame_ChangeGameplayFeature component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[3].transform).GetComponent<Item_DevGame_ChangeGameplayFeature>();
					component.myID = j;
					component.gS_ = gS_;
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.gF_ = gF_;
					component.menu_ = this;
					component.SetGoodBadIcon();
					component.BUTTON_Click();
					component.BUTTON_Click();
				}
			}
		}
		DROPDOWN_SortGameplayFeatures();
		guiMain_.KeinEintrag(uiObjects[3], uiObjects[5]);
	}

	public void DROPDOWN_SortGameplayFeatures()
	{
		int value = uiObjects[2].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[2].name, value);
		int childCount = uiObjects[3].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[3].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_ChangeGameplayFeature component = gameObject.GetComponent<Item_DevGame_ChangeGameplayFeature>();
				switch (value)
				{
				case 0:
					gameObject.name = gF_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = gF_.GetDevCosts(component.myID).ToString();
					break;
				case 2:
					gameObject.name = gF_.gameplayFeatures_TYP[component.myID].ToString();
					break;
				case 3:
					gameObject.name = component.goodBad.ToString();
					break;
				}
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[3]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[3]);
		}
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[3].transform.childCount; i++)
			{
				uiObjects[3].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[4].GetComponent<InputField>().text;
			Init_GameplayFeatures();
		}
	}

	public bool SetGameplayFeature(int i)
	{
		g_GameGameplayFeatures[i] = !g_GameGameplayFeatures[i];
		CalcDevCosts();
		UpdateGesamtGameplayFeatures();
		return g_GameGameplayFeatures[i];
	}

	public bool DisableGameplayFeature(int i)
	{
		g_GameGameplayFeatures[i] = false;
		CalcDevCosts();
		UpdateGesamtGameplayFeatures();
		return g_GameGameplayFeatures[i];
	}

	public void BUTTON_AllGameplayFeatures()
	{
		allFeatures = !allFeatures;
		if (!allFeatures)
		{
			DisableAllGameplayFeatures();
			return;
		}
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[3].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Item_DevGame_ChangeGameplayFeature>().BUTTON_Click();
			}
		}
	}

	public void BUTTON_AllPassendenGameplayFeatures()
	{
		if (g_GameMainGenre < 0)
		{
			return;
		}
		allFeatures = !allFeatures;
		if (!allFeatures)
		{
			DisableAllGameplayFeatures();
			return;
		}
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[3].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_ChangeGameplayFeature component = gameObject.GetComponent<Item_DevGame_ChangeGameplayFeature>();
				if (gF_.GetBonus(component.myID, g_GameMainGenre, g_GameSubGenre) > 0.9f && gF_.gameplayFeatures_GAMEPLAY[component.myID] >= 0)
				{
					component.BUTTON_Click();
				}
			}
		}
	}

	public void DisableAllGameplayFeatures()
	{
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (!gS_.gameplayFeatures_DevDone[i] && !gS_.gameGameplayFeatures[i])
			{
				g_GameGameplayFeatures[i] = false;
			}
		}
		CalcDevCosts();
		UpdateGesamtGameplayFeatures();
		sfx_.PlaySound(3, force: true);
	}
}
