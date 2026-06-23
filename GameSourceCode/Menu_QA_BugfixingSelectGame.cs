using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_QA_BugfixingSelectGame : MonoBehaviour
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_QA_Bugfixing>().game_.myID == id_)
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
		list.Add(tS_.GetText(424));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(325));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
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
		if (!rS_)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && CheckGameData(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_QA_Bugfixing component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_QA_Bugfixing>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.rS_ = rS_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.developerID == mS_.myID && script_.inDevelopment && !script_.isOnMarket)
		{
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
				Item_QA_Bugfixing component = gameObject.GetComponent<Item_QA_Bugfixing>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.GetNameSimple();
					break;
				case 1:
					gameObject.name = Mathf.RoundToInt(component.game_.points_bugs).ToString();
					break;
				case 2:
					gameObject.name = component.game_.maingenre.ToString();
					break;
				case 3:
					gameObject.name = (-component.game_.GetPlatformTypINT()).ToString();
					break;
				case 4:
					gameObject.name = (-component.game_.GetTypINT()).ToString();
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
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void StartBugfixing(gameScript gS_)
	{
		if ((bool)gS_ && (bool)rS_)
		{
			taskBugfixing taskBugfixing2 = guiMain_.AddTask_Bugfixing();
			taskBugfixing2.Init(fromSavegame: false);
			taskBugfixing2.targetID = gS_.myID;
			taskBugfixing2.points = 5f;
			taskBugfixing2.pointsLeft = taskBugfixing2.points;
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskID = taskBugfixing2.myID;
			}
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}

	public void StartBugfixingAutomatic(gameScript gS_, int taskID)
	{
		if (!gS_)
		{
			return;
		}
		if (!guiMain_)
		{
			FindScripts();
		}
		taskBugfixing taskBugfixing2 = guiMain_.AddTask_Bugfixing();
		taskBugfixing2.Init(fromSavegame: false);
		taskBugfixing2.targetID = gS_.myID;
		taskBugfixing2.points = 5f;
		taskBugfixing2.pointsLeft = taskBugfixing2.points;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == taskID)
			{
				mS_.arrayRoomScripts[i].taskGameObject = taskBugfixing2.gameObject;
				mS_.arrayRoomScripts[i].taskID = taskBugfixing2.myID;
				break;
			}
		}
	}
}
