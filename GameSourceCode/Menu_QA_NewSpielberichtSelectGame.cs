using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_QA_NewSpielberichtSelectGame : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	public roomScript rS_;

	private float updateTimer;

	private float getNumSpielberichteCanCreate_Timer;

	private int numSpielberichteCanCreate;

	private string searchStringA = "";

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_QA_CreateSpielbericht>().game_.myID == id_)
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
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(217));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[9].name);
		list.Clear();
		list.Add(tS_.GetText(1902));
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (genres_.IsErforscht(i))
			{
				list.Add(genres_.GetName(i));
			}
			else
			{
				list.Add("<color=grey>" + genres_.GetName(i) + "</color>");
			}
		}
		uiObjects[9].GetComponent<Dropdown>().ClearOptions();
		uiObjects[9].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[9].GetComponent<Dropdown>().value = value;
	}

	public void Init(roomScript room_)
	{
		FindScripts();
		rS_ = room_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		int value = uiObjects[9].GetComponent<Dropdown>().value;
		if (!rS_)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if ((bool)component && CheckGameData(component) && (value - 1 == component.maingenre || value == 0))
			{
				string nameSimple = component.GetNameSimple();
				searchStringA = searchStringA.ToLower();
				nameSimple = nameSimple.ToLower();
				if ((uiObjects[6].GetComponent<InputField>().text.Length <= 0 || nameSimple.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
				{
					Item_QA_CreateSpielbericht component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_QA_CreateSpielbericht>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if ((bool)script_ && script_.developerID == mS_.myID && !script_.inDevelopment && !script_.spielbericht && !script_.typ_bundle && !script_.typ_budget && !script_.pubOffer && !script_.typ_bundleAddon && !script_.typ_goty && !script_.schublade && (script_.typ_standard || script_.typ_nachfolger || script_.typ_spinoff) && !BereitsInAnderenRaumAktiv(script_.myID))
		{
			return true;
		}
		return false;
	}

	public int GetNumSpielberichteCanCreate()
	{
		getNumSpielberichteCanCreate_Timer += Time.deltaTime;
		if (getNumSpielberichteCanCreate_Timer < 1f)
		{
			return numSpielberichteCanCreate;
		}
		getNumSpielberichteCanCreate_Timer = 0f;
		numSpielberichteCanCreate = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && CheckGameData(component))
				{
					numSpielberichteCanCreate++;
				}
			}
		}
		return numSpielberichteCanCreate;
	}

	public void DROPDOWN_Genres()
	{
		int value = uiObjects[9].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[9].name, value);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_QA_CreateSpielbericht component = gameObject.GetComponent<Item_QA_CreateSpielbericht>();
			switch (value)
			{
			case 0:
				gameObject.name = component.game_.GetNameSimple();
				break;
			case 1:
				gameObject.name = component.game_.reviewTotal.ToString();
				break;
			case 2:
				gameObject.name = component.game_.maingenre.ToString();
				break;
			case 3:
			{
				float num = component.game_.date_month;
				num /= 13f;
				gameObject.name = component.game_.date_year.ToString() + num;
				if (component.game_.inDevelopment || component.game_.schublade)
				{
					gameObject.name = "999999";
				}
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

	public void StartSpielbericht(gameScript gS_)
	{
		if ((bool)gS_ && (bool)rS_)
		{
			taskSpielbericht taskSpielbericht2 = guiMain_.AddTask_Spielbericht();
			taskSpielbericht2.Init(fromSavegame: false);
			taskSpielbericht2.targetID = gS_.myID;
			taskSpielbericht2.automatic = uiObjects[4].GetComponent<Toggle>().isOn;
			taskSpielbericht2.automaticWait = uiObjects[7].GetComponent<Toggle>().isOn;
			taskSpielbericht2.points = GetWorkPoints(gS_);
			taskSpielbericht2.pointsLeft = taskSpielbericht2.points;
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskSpielbericht2.myID;
			}
			guiMain_.CloseMenu();
			guiMain_.uiObjects[180].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}

	public int GetWorkPoints(gameScript gS_)
	{
		return Mathf.RoundToInt((float)gS_.GetGesamtDevPoints() * 0.1f + 25f);
	}

	public bool BereitsInAnderenRaumAktiv(int id_)
	{
		FindScripts();
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 3 && (bool)mS_.arrayRoomScripts[i].taskGameObject)
			{
				taskSpielbericht component = mS_.arrayRoomScripts[i].taskGameObject.GetComponent<taskSpielbericht>();
				if ((bool)component && component.targetID == id_)
				{
					return true;
				}
			}
		}
		return false;
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		taskWait taskWait2 = guiMain_.AddTask_Wait();
		taskWait2.Init(fromSavegame: false);
		taskWait2.art = 0;
		rS_.taskID = taskWait2.myID;
		guiMain_.CloseMenu();
		guiMain_.uiObjects[180].SetActive(value: false);
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
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			SetData();
		}
	}
}
