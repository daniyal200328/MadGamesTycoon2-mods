using UnityEngine;

public class taskSpielbericht : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public bool automatic;

	public float points;

	public float pointsLeft;

	public bool automaticWait;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private games games_;

	public gameScript gS_;

	private void Awake()
	{
		base.transform.position = new Vector3(100f, 0f, 0f);
	}

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			if (!main_)
			{
				main_ = GameObject.FindGameObjectWithTag("Main");
			}
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
		}
	}

	private void Update()
	{
		FindMyObject();
	}

	public void Init(bool fromSavegame)
	{
		if (!fromSavegame)
		{
			myID = Random.Range(1, 100000000);
		}
		base.name = "Task_" + myID;
	}

	private void FindMyObject()
	{
		if (!gS_)
		{
			if (!gS_)
			{
				gS_ = FindGameScriptWithID(targetID);
			}
			if (!gS_)
			{
				Abbrechen();
			}
		}
	}

	private gameScript FindGameScriptWithID(int id_)
	{
		if (!games_)
		{
			FindScripts();
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == id_)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	public float GetProzent()
	{
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		return guiMain_.uiSprites[18];
	}

	public void Work(float f)
	{
		if (pointsLeft > 0f)
		{
			pointsLeft -= f;
			if (pointsLeft <= 0f)
			{
				pointsLeft = 0f;
				Complete();
			}
		}
	}

	private void Complete()
	{
		FindMyObject();
		gS_.SetSpielbericht();
		roomScript roomScript2 = FindMyRoomWithTask();
		if (!roomScript2)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		int roomID_ = roomScript2.myID;
		string text = "";
		text = tS_.GetText(929);
		text = text.Replace("<NAME1>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[3]);
		if (!DoAutomatic())
		{
			if (automatic && automaticWait)
			{
				taskWait taskWait2 = guiMain_.AddTask_Wait();
				taskWait2.Init(fromSavegame: false);
				taskWait2.art = 0;
				roomScript2.taskID = taskWait2.myID;
			}
			Object.Destroy(base.gameObject);
		}
	}

	private roomScript FindMyRoomWithTask()
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				return mS_.arrayRoomScripts[i];
			}
		}
		return null;
	}

	private bool DoAutomatic()
	{
		FindMyObject();
		if (!automatic)
		{
			return false;
		}
		Menu_QA_NewSpielberichtSelectGame component = guiMain_.uiObjects[181].GetComponent<Menu_QA_NewSpielberichtSelectGame>();
		if ((bool)component)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
			for (int i = 0; i < array.Length; i++)
			{
				if ((bool)array[i])
				{
					gameScript component2 = array[i].GetComponent<gameScript>();
					if ((bool)component2 && component.CheckGameData(component2))
					{
						targetID = component2.myID;
						gS_ = component2;
						points = component.GetWorkPoints(component2);
						pointsLeft = points;
						return true;
					}
				}
			}
		}
		return false;
	}

	private void LeftNews(string c, Sprite icon, Sprite iconRoom)
	{
		int roomID_ = -1;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			roomScript component = array[i].GetComponent<roomScript>();
			if ((bool)component && component.taskID == myID)
			{
				roomID_ = component.myID;
				break;
			}
		}
		guiMain_.CreateLeftNews(roomID_, icon, c, iconRoom);
	}

	public int GetRueckgeld()
	{
		return 0;
	}

	public void Abbrechen()
	{
		int rueckgeld = GetRueckgeld();
		if (rueckgeld > 0)
		{
			mS_.Earn(Mathf.RoundToInt(rueckgeld), 1);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == myID)
				{
					guiMain_.MoneyPop(Mathf.RoundToInt(rueckgeld), new Vector3(component.uiPos.x, component.uiPos.y + 3f, component.uiPos.z), green: true);
					break;
				}
			}
		}
		Object.Destroy(base.gameObject);
	}
}
