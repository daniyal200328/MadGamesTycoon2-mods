using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_AuftragSelect : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	public roomScript rS_;

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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		if (uiObjects[4].GetComponent<Toggle>().isOn)
		{
			uiObjects[7].GetComponent<Toggle>().interactable = true;
		}
		else
		{
			uiObjects[7].GetComponent<Toggle>().interactable = false;
			uiObjects[7].GetComponent<Toggle>().isOn = false;
		}
		uiObjects[8].GetComponent<Button>().interactable = uiObjects[7].GetComponent<Toggle>().isOn;
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_ContractWork>().contract_.myID == id_)
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
		list.Add(tS_.GetText(600));
		list.Add(tS_.GetText(601));
		list.Add(tS_.GetText(602));
		list.Add(tS_.GetText(603));
		list.Add(tS_.GetText(604));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(roomScript room_)
	{
		FindScripts();
		rS_ = room_;
		if (rS_.typ != 14)
		{
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(597);
		}
		else
		{
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(1130);
		}
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		if (!rS_)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				contractWork component = array[i].GetComponent<contractWork>();
				if ((bool)component && ((rS_.typ == 1 && component.art == 0) || (rS_.typ == 3 && component.art == 1) || (rS_.typ == 4 && component.art == 2) || (rS_.typ == 5 && component.art == 3) || (rS_.typ == 10 && component.art == 4) || (rS_.typ == 14 && component.art == 5) || (rS_.typ == 17 && component.art == 6) || (rS_.typ == 8 && component.art == 7)) && !component.IsAngenommen() && !Exists(uiObjects[0], component.myID))
				{
					GameObject gameObject = ((rS_.typ == 14) ? Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform) : Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform));
					Item_ContractWork component2 = gameObject.GetComponent<Item_ContractWork>();
					component2.contract_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
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
				Item_ContractWork component = gameObject.GetComponent<Item_ContractWork>();
				switch (value)
				{
				case 0:
					gameObject.name = component.contract_.GetGehalt().ToString();
					break;
				case 1:
					gameObject.name = component.contract_.GetStrafe().ToString();
					break;
				case 2:
					gameObject.name = component.contract_.GetWochen().ToString();
					break;
				case 3:
					gameObject.name = component.contract_.GetArbeitsaufwand().ToString();
					break;
				case 4:
					gameObject.name = component.contract_.auftraggeberID.ToString();
					break;
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		if (rS_.typ == 1)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[95]);
		}
		else
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		taskContractWait taskContractWait2 = guiMain_.AddTask_ContractWait();
		taskContractWait2.Init(fromSavegame: false);
		switch (rS_.typ)
		{
		case 1:
			taskContractWait2.art = 0;
			break;
		case 3:
			taskContractWait2.art = 1;
			break;
		case 4:
			taskContractWait2.art = 2;
			break;
		case 5:
			taskContractWait2.art = 3;
			break;
		case 10:
			taskContractWait2.art = 4;
			break;
		case 14:
			taskContractWait2.art = 5;
			break;
		case 17:
			taskContractWait2.art = 6;
			break;
		case 8:
			taskContractWait2.art = 7;
			break;
		}
		rS_.taskID = taskContractWait2.myID;
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
