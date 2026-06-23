using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_EnginePlatform : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private gameplayFeatures gF_;

	private Menu_Dev_Engine menuDevEngine_;

	private float updateTimer;

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
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!menuDevEngine_)
		{
			menuDevEngine_ = guiMain_.uiObjects[37].GetComponent<Menu_Dev_Engine>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevEngine_Platform>().pS_.myID == id_)
			{
				return true;
			}
		}
		return false;
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
		list.Add(tS_.GetText(216));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(218));
		list.Add(tS_.GetText(219));
		list.Add(tS_.GetText(220));
		list.Add(tS_.GetText(6));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(1485));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init()
	{
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		bool isOn = uiObjects[5].GetComponent<Toggle>().isOn;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(376) + ": " + menuDevEngine_.techLevel;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				platformScript component = array[j].GetComponent<platformScript>();
				if ((bool)component && (component.isUnlocked || component.ownerID == mS_.myID) && component.inBesitz && CorrectPlatformFilter(component) && !component.IsProConsoleInDev() && ((!component.vomMarktGenommen && component.tech >= menuDevEngine_.techLevel) || isOn) && !Exists(uiObjects[0], component.myID))
				{
					Item_DevEngine_Platform component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevEngine_Platform>();
					component2.myID = component.myID;
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.gF_ = gF_;
					component2.guiMain_ = guiMain_;
					component2.menuDevEngine_ = menuDevEngine_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
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
				Item_DevEngine_Platform component = gameObject.GetComponent<Item_DevEngine_Platform>();
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
					gameObject.name = component.pS_.GetPrice().ToString();
					break;
				case 5:
					gameObject.name = component.pS_.GetMarktanteil().ToString();
					break;
				case 6:
					gameObject.name = component.pS_.GetGames().ToString();
					break;
				case 7:
					gameObject.name = component.pS_.GetDevCosts().ToString();
					break;
				case 8:
					gameObject.name = (100 - component.pS_.typ).ToString();
					break;
				case 9:
					gameObject.name = component.pS_.GetAktiveNutzer().ToString();
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

	public void TOGGLE_HiddenAnzeigen()
	{
		Init();
	}

	public void TOGGLE_PlatformsFilter()
	{
		Init();
	}

	private bool CorrectPlatformFilter(platformScript script_)
	{
		if ((bool)script_)
		{
			if (script_.typ == 0 && uiObjects[8].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 1 && uiObjects[9].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 2 && uiObjects[10].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 4 && uiObjects[11].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 3 && uiObjects[12].GetComponent<Toggle>().isOn)
			{
				return true;
			}
		}
		return false;
	}
}
