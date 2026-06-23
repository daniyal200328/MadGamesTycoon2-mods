using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ROOM_Polishing : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private roomScript rS_;

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

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Polishing>().game_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void Init(roomScript script_)
	{
		rS_ = script_;
		FindScripts();
		SetData();
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(325));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && CheckGameData(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_Polishing component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Polishing>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.game_ = component;
					component2.rS_ = rS_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.developerID == mS_.myID && script_.inDevelopment)
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
				Item_Polishing component = gameObject.GetComponent<Item_Polishing>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.GetNameSimple();
					break;
				case 1:
					gameObject.name = component.game_.maingenre.ToString();
					break;
				case 2:
					gameObject.name = (-component.game_.GetPlatformTypINT()).ToString();
					break;
				case 3:
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

	public void StartPolishing(gameScript gS_)
	{
		if ((bool)gS_ && (bool)rS_)
		{
			taskPolishing taskPolishing2 = guiMain_.AddTask_Polishing();
			taskPolishing2.Init(fromSavegame: false);
			taskPolishing2.targetID = gS_.myID;
			taskPolishing2.points = 200f;
			taskPolishing2.pointsLeft = taskPolishing2.points;
			GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject)
			{
				gameObject.GetComponent<roomScript>().taskGameObject = null;
				gameObject.GetComponent<roomScript>().taskID = taskPolishing2.myID;
			}
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}

	public void StartPolishingAutomatic(gameScript gS_, int taskID)
	{
		if (!gS_)
		{
			return;
		}
		if (!guiMain_)
		{
			FindScripts();
		}
		taskPolishing taskPolishing2 = guiMain_.AddTask_Polishing();
		taskPolishing2.Init(fromSavegame: false);
		taskPolishing2.targetID = gS_.myID;
		taskPolishing2.points = 200f;
		taskPolishing2.pointsLeft = taskPolishing2.points;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == taskID)
			{
				mS_.arrayRoomScripts[i].taskGameObject = taskPolishing2.gameObject;
				mS_.arrayRoomScripts[i].taskID = taskPolishing2.myID;
				break;
			}
		}
	}
}
