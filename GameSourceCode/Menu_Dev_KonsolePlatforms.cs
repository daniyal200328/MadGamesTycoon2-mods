using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsolePlatforms : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private Menu_Dev_Konsole menu_;

	public int platformNR;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
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
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(219));
		list.Add(tS_.GetText(220));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(1485));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = 5;
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(int nr)
	{
		platformNR = nr;
		FindScripts();
		SetData(platformNR);
	}

	private void SetData(int nr)
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		int techLevel = menu_.GetTechLevel();
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(2149) + ": " + techLevel;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				platformScript component = array[j].GetComponent<platformScript>();
				if ((bool)component && CheckData(component) && menu_.platformCompatible[0] != component.myID && menu_.platformCompatible[1] != component.myID && menu_.platformCompatible[2] != component.myID && menu_.platformCompatible[3] != component.myID)
				{
					Item_DevKonsole_Platform component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevKonsole_Platform>();
					component2.myID = component.myID;
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.menu_ = menu_;
					component2.menuSelectPlatform_ = this;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	public bool CheckData(platformScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && script_.isUnlocked && script_.tech <= menu_.GetTechLevel())
		{
			if ((bool)menu_.proScript_ && menu_.proScript_.myID == script_.myID)
			{
				return false;
			}
			return true;
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
				Item_DevKonsole_Platform component = gameObject.GetComponent<Item_DevKonsole_Platform>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
				{
					float num = component.pS_.date_month;
					num /= 13f;
					gameObject.name = component.pS_.date_year.ToString() + num;
					break;
				}
				case 2:
					gameObject.name = component.pS_.tech.ToString();
					break;
				case 3:
					gameObject.name = component.pS_.GetMarktanteil().ToString();
					break;
				case 4:
					gameObject.name = component.pS_.GetGames().ToString();
					break;
				case 5:
					gameObject.name = (100 - component.pS_.typ).ToString();
					break;
				case 6:
					gameObject.name = component.pS_.GetAktiveNutzer().ToString();
					break;
				}
			}
		}
		if (value == 0)
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

	public void BUTTON_PlatformEntfernen()
	{
		menu_.SetPlatform(platformNR, -1);
		BUTTON_Close();
	}
}
