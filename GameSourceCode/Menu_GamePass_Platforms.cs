using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Platforms : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private genres genres_;

	private games games_;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	public void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		ResetData();
	}

	private void Update()
	{
		string text = tS_.GetText(2108);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount.ToString());
		uiObjects[9].GetComponent<Text>().text = text;
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
			uiObjects[5].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(216));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(219));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(1485));
		list.Add(tS_.GetText(2111));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[6].name);
		list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(216));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(219));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(1485));
		list.Add(tS_.GetText(2111));
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[6].GetComponent<Dropdown>().value = value;
	}

	public void ResetData()
	{
		Item_GamePass_Platform[] componentsInChildren = uiObjects[0].GetComponentsInChildren<Item_GamePass_Platform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].gameObject.transform.SetParent(null);
				Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_GamePass_Platform>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if ((bool)componentsInChildren[j])
			{
				componentsInChildren[j].gameObject.transform.SetParent(null);
				Object.Destroy(componentsInChildren[j].gameObject);
			}
		}
		SetData_InPass();
		SetData_OutPass();
		UpdateMarketshare();
	}

	private void SetData_OutPass()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && CheckData_OutPass(component))
				{
					Item_GamePass_Platform component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[4].transform).GetComponent<Item_GamePass_Platform>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.pS_ = component;
					component2.games_ = games_;
					component2.UpdateKompatibleSpiele();
				}
			}
		}
		DROPDOWN_Sort_OutPass();
		guiMain_.KeinEintrag(uiObjects[4], uiObjects[8]);
	}

	private void SetData_InPass()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && CheckData_InPass(component))
				{
					Item_GamePass_Platform component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_GamePass_Platform>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.pS_ = component;
					component2.games_ = games_;
					component2.UpdateKompatibleSpiele();
				}
			}
		}
		DROPDOWN_Sort_InPass();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[7]);
	}

	public bool CheckData_OutPass(platformScript script_)
	{
		if ((bool)script_ && script_.CanBeInGamePass(ignoreGames: true) && !script_.inGamePass)
		{
			return true;
		}
		return false;
	}

	public bool CheckData_InPass(platformScript script_)
	{
		if ((bool)script_)
		{
			if (script_.CanBeInGamePass(ignoreGames: true) && script_.inGamePass && script_.GetAmountGamePassGames() >= script_.minGamePassGames)
			{
				return true;
			}
			if (script_.inGamePass)
			{
				script_.inGamePass = false;
			}
		}
		return false;
	}

	public void UpdateMarketshare()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		long num4 = 0L;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			platformScript component = array[i].GetComponent<platformScript>();
			if ((bool)component && CheckData_InPass(component))
			{
				num4 += component.GetAktiveNutzer();
				switch (component.typ)
				{
				case 0:
					num += component.GetMarktanteil();
					break;
				case 1:
					num2 += component.GetMarktanteil();
					break;
				case 2:
					num3 += component.GetMarktanteil();
					break;
				}
			}
		}
		uiObjects[10].GetComponent<Text>().text = mS_.Round(num, 1) + "%";
		uiObjects[11].GetComponent<Text>().text = mS_.Round(num2, 1) + "%";
		uiObjects[12].GetComponent<Text>().text = mS_.Round(num3, 1) + "%";
		uiObjects[13].GetComponent<Text>().text = mS_.Round((float)num4 / 1000000f, 1) + " " + tS_.GetText(1483);
	}

	public void DROPDOWN_Sort_OutPass()
	{
		int value = uiObjects[6].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[6].name, value);
		int childCount = uiObjects[4].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[4].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_GamePass_Platform component = gameObject.GetComponent<Item_GamePass_Platform>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
					gameObject.name = component.pS_.GetManufacturer().ToString();
					break;
				case 2:
				{
					float num = component.pS_.date_month;
					num /= 13f;
					gameObject.name = component.pS_.date_year.ToString() + num;
					break;
				}
				case 3:
					gameObject.name = component.pS_.tech.ToString();
					break;
				case 4:
					gameObject.name = component.pS_.GetMarktanteil().ToString();
					break;
				case 5:
					gameObject.name = (100 - component.pS_.typ).ToString();
					break;
				case 6:
					gameObject.name = component.pS_.GetAktiveNutzer().ToString();
					break;
				case 7:
					gameObject.name = component.kompatibleGames.ToString();
					break;
				}
			}
		}
		if (value == 0 || value == 1)
		{
			mS_.SortChildrenByName(uiObjects[4]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[4]);
		}
	}

	public void DROPDOWN_Sort_InPass()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_GamePass_Platform component = gameObject.GetComponent<Item_GamePass_Platform>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
					gameObject.name = component.pS_.GetManufacturer().ToString();
					break;
				case 2:
				{
					float num = component.pS_.date_month;
					num /= 13f;
					gameObject.name = component.pS_.date_year.ToString() + num;
					break;
				}
				case 3:
					gameObject.name = component.pS_.tech.ToString();
					break;
				case 4:
					gameObject.name = component.pS_.GetMarktanteil().ToString();
					break;
				case 5:
					gameObject.name = (100 - component.pS_.typ).ToString();
					break;
				case 6:
					gameObject.name = component.pS_.GetAktiveNutzer().ToString();
					break;
				case 7:
					gameObject.name = component.kompatibleGames.ToString();
					break;
				}
			}
		}
		if (value == 0 || value == 1)
		{
			mS_.SortChildrenByName(uiObjects[0]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[0]);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_AddAllPlatforms()
	{
		sfx_.PlaySound(3, force: true);
		Item_GamePass_Platform[] componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_GamePass_Platform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(allPlatforms: true);
			}
		}
		ResetData();
	}

	public void BUTTON_RemoveAllPlatforms()
	{
		sfx_.PlaySound(3, force: true);
		Item_GamePass_Platform[] componentsInChildren = uiObjects[0].GetComponentsInChildren<Item_GamePass_Platform>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(allPlatforms: true);
			}
		}
		ResetData();
	}
}
