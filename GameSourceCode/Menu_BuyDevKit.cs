using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_BuyDevKit : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private int TAB;

	private float updateTimer;

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
		if (!mS_.multiplayer)
		{
			return;
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 5f))
		{
			updateTimer = 0f;
			switch (TAB)
			{
			case 0:
				SetData(inBesitz: false);
				break;
			case 1:
				SetData(inBesitz: true);
				break;
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Platform_BuyDevKit>().pS_.myID == id_)
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
		TAB_DevKitsBuy(0);
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

	private void Init(bool inBesitz)
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData(inBesitz);
	}

	private void SetData(bool inBesitz)
	{
		bool isOn = uiObjects[5].GetComponent<Toggle>().isOn;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.isUnlocked && component.inBesitz == inBesitz && (component.OwnerIsNPC() || component.thridPartyGames || !component.OwnerIsNPC()) && (!component.vomMarktGenommen || isOn) && CorrectPlatformFilter(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_Platform_BuyDevKit component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Platform_BuyDevKit>();
					component2.myID = component.myID;
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	private bool CorrectPlatformFilter(platformScript script_)
	{
		if ((bool)script_)
		{
			if (script_.typ == 0 && uiObjects[7].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 1 && uiObjects[8].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 2 && uiObjects[9].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 4 && uiObjects[10].GetComponent<Toggle>().isOn)
			{
				return true;
			}
			if (script_.typ == 3 && uiObjects[11].GetComponent<Toggle>().isOn)
			{
				return true;
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
				Item_Platform_BuyDevKit component = gameObject.GetComponent<Item_Platform_BuyDevKit>();
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
		if (!guiMain_.uiObjects[56].activeSelf && !guiMain_.uiObjects[102].activeSelf && !guiMain_.uiObjects[37].activeSelf && !guiMain_.uiObjects[99].activeSelf)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void TAB_DevKitsBuy(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(inBesitz: false);
	}

	public void TAB_MyDevKits(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(inBesitz: true);
	}

	public void TOGGLE_VomMarktGenommen()
	{
		switch (TAB)
		{
		case 0:
			TAB_DevKitsBuy(0);
			break;
		case 1:
			TAB_MyDevKits(1);
			break;
		}
	}

	public void TOGGLE_PlatformsFilter()
	{
		switch (TAB)
		{
		case 0:
			TAB_DevKitsBuy(0);
			break;
		case 1:
			TAB_MyDevKits(1);
			break;
		}
	}
}
