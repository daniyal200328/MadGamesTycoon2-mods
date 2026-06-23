using UnityEngine;

public class taskMarketingSpezial : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public int kampagne = -1;

	public float points;

	public float pointsLeft;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private games games_;

	public Menu_MarketingSpezial menu_;

	public gameScript gS_;

	private void Awake()
	{
		base.transform.position = new Vector3(70f, 0f, 0f);
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
			if (!menu_)
			{
				menu_ = guiMain_.uiObjects[294].GetComponent<Menu_MarketingSpezial>();
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
		FindScripts();
		return menu_.sprites[kampagne];
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
		FindScripts();
		FindMyObject();
		int roomID_ = FindMyRoomWithTask();
		string text = "";
		text = tS_.GetText(1425);
		text = text.Replace("<NAME1>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[6]);
		gS_.costs_marketing += menu_.preise[kampagne];
		gS_.specialMarketing[kampagne] = 1;
		Object.Destroy(base.gameObject);
	}

	private int FindMyRoomWithTask()
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				return mS_.arrayRoomScripts[i].myID;
			}
		}
		return -1;
	}

	public int GetRueckgeld()
	{
		return Mathf.RoundToInt((float)menu_.preise[kampagne] * ((100f - GetProzent()) * 0.01f));
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
