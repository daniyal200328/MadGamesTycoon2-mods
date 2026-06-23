using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Katalog : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private string searchStringA = "";

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
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
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
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
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
				string text = component.GetName();
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Item_Platform_Katalog component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Platform_Katalog>();
					component2.myID = component.myID;
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.kompatibleGames = component.GetAmountGamePassGames();
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	public bool CheckData_InPass(platformScript script_)
	{
		if ((bool)script_)
		{
			if (script_.CanBeInGamePass(ignoreGames: true) && script_.inGamePass)
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

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_Platform_Katalog component = gameObject.GetComponent<Item_Platform_Katalog>();
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

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[7].GetComponent<InputField>().text;
			Init();
		}
	}
}
