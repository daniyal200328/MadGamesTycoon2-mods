using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Platform : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private Menu_DevGame devGame_;

	private Menu_Dev_ChangePlatform changePlatform_;

	public int platformNR;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!devGame_)
		{
			devGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!changePlatform_)
		{
			changePlatform_ = guiMain_.uiObjects[102].GetComponent<Menu_Dev_ChangePlatform>();
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
				SetData(platformNR);
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevGame_Platform>().pS_.myID == id_)
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
		uiObjects[9].SetActive(value: false);
		if (guiMain_.uiObjects[56].activeSelf && devGame_.uiObjects[146].GetComponent<Dropdown>().value == 1)
		{
			uiObjects[9].SetActive(value: true);
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
		list.Add(tS_.GetText(218));
		list.Add(tS_.GetText(219));
		list.Add(tS_.GetText(220));
		list.Add(tS_.GetText(6));
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
		bool isOn = uiObjects[5].GetComponent<Toggle>().isOn;
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(360 + nr);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		int num = 0;
		if (devGame_.gameObject.activeSelf)
		{
			num = devGame_.GetEngineTechLevel();
			uiObjects[7].GetComponent<Text>().text = tS_.GetText(376) + ": " + num;
		}
		if (changePlatform_.gameObject.activeSelf)
		{
			num = GetMindestTechStufe(changePlatform_.gS_);
			uiObjects[7].GetComponent<Text>().text = tS_.GetText(376) + ": " + GetMindestTechStufe(changePlatform_.gS_);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int j = 0; j < array.Length; j++)
		{
			if (!array[j])
			{
				continue;
			}
			platformScript component = array[j].GetComponent<platformScript>();
			if (!component || component.IsProConsoleInDev() || (!component.isUnlocked && component.ownerID != mS_.myID) || !component.inBesitz || !CorrectPlatformFilter(component) || ((devGame_.uiObjects[146].GetComponent<Dropdown>().value == 3 || component.vomMarktGenommen) && (devGame_.uiObjects[146].GetComponent<Dropdown>().value != 3 || !component.vomMarktGenommen) && !isOn))
			{
				continue;
			}
			if (devGame_.gameObject.activeSelf && ((devGame_.g_GamePlatform[0] != component.myID && devGame_.g_GamePlatform[1] != component.myID && devGame_.g_GamePlatform[2] != component.myID && devGame_.g_GamePlatform[3] != component.myID) || isOn))
			{
				bool flag = false;
				if (component.ownerID != mS_.myID && devGame_.uiObjects[146].GetComponent<Dropdown>().value == 2)
				{
					flag = true;
				}
				if (component.typ == 3 && devGame_.uiObjects[146].GetComponent<Dropdown>().value != 5)
				{
					flag = true;
				}
				if (component.typ != 3 && devGame_.uiObjects[146].GetComponent<Dropdown>().value == 5)
				{
					flag = true;
				}
				if (component.typ == 4 && devGame_.uiObjects[146].GetComponent<Dropdown>().value != 4)
				{
					flag = true;
				}
				if (component.typ != 4 && devGame_.uiObjects[146].GetComponent<Dropdown>().value == 4)
				{
					flag = true;
				}
				if (uiObjects[8].GetComponent<Toggle>().isOn && num > component.tech)
				{
					flag = true;
				}
				if (!flag && !Exists(uiObjects[0], component.myID))
				{
					Item_DevGame_Platform component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_Platform>();
					component2.myID = component.myID;
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.devGame_ = devGame_;
				}
			}
			if (changePlatform_.gameObject.activeSelf && ((changePlatform_.g_GamePlatform[0] != component.myID && changePlatform_.g_GamePlatform[1] != component.myID && changePlatform_.g_GamePlatform[2] != component.myID && changePlatform_.g_GamePlatform[3] != component.myID) || isOn) && component.tech >= num)
			{
				bool flag2 = false;
				if (component.ownerID != mS_.myID && changePlatform_.gS_.herstellerExklusiv)
				{
					flag2 = true;
				}
				if (component.typ == 3 && !changePlatform_.gS_.handy)
				{
					flag2 = true;
				}
				if (component.typ != 3 && changePlatform_.gS_.handy)
				{
					flag2 = true;
				}
				if (component.typ == 4 && !changePlatform_.gS_.arcade)
				{
					flag2 = true;
				}
				if (component.typ != 4 && changePlatform_.gS_.arcade)
				{
					flag2 = true;
				}
				if (uiObjects[8].GetComponent<Toggle>().isOn && num > component.tech)
				{
					flag2 = true;
				}
				if (!flag2 && !Exists(uiObjects[0], component.myID))
				{
					Item_DevGame_Platform component3 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_Platform>();
					component3.myID = component.myID;
					component3.pS_ = component;
					component3.mS_ = mS_;
					component3.tS_ = tS_;
					component3.sfx_ = sfx_;
					component3.guiMain_ = guiMain_;
					component3.changePlatform_ = changePlatform_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	private int GetMindestTechStufe(gameScript script_)
	{
		int num = 1;
		for (int i = 0; i < script_.gameEngineFeature.Length; i++)
		{
			if (eF_.engineFeatures_TECH[script_.gameEngineFeature[i]] > num)
			{
				num = eF_.engineFeatures_TECH[script_.gameEngineFeature[i]];
			}
		}
		return num;
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
				Item_DevGame_Platform component = gameObject.GetComponent<Item_DevGame_Platform>();
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

	public void BUTTON_PlatformEntfernen()
	{
		if (devGame_.gameObject.activeSelf)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetPlatform(platformNR, -1);
		}
		if (changePlatform_.gameObject.activeSelf)
		{
			guiMain_.uiObjects[102].GetComponent<Menu_Dev_ChangePlatform>().SetPlatform(platformNR, -1, init: false);
		}
		BUTTON_Close();
	}

	public void TOGGLE_VomMarktGenommen()
	{
		Init(platformNR);
	}

	public void TOGGLE_FitTechLevel()
	{
		Init(platformNR);
	}

	private bool CorrectPlatformFilter(platformScript script_)
	{
		if ((bool)script_)
		{
			if (script_.typ == 0 && uiObjects[10].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 1 && uiObjects[11].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 2 && uiObjects[12].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 4 && uiObjects[13].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 3 && uiObjects[14].GetComponent<Toggle>().isOn)
			{
				return true;
			}
		}
		return false;
	}

	public void TOGGLE_PlatformsFilter()
	{
		Init(platformNR);
	}
}
