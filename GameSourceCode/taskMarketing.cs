using UnityEngine;

public class taskMarketing : MonoBehaviour
{
	public int myID = -1;

	public int typ = -1;

	public int targetID = -1;

	public int kampagne = -1;

	public bool automatic;

	public bool stopAutomatic;

	public bool disableWarten;

	public float points;

	public float pointsLeft;

	private GameObject main_;

	private mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private games games_;

	public Menu_Marketing_GameKampagne scriptMarketing_;

	public Menu_Marketing_KonsoleKampagne scriptMarketingKonsole_;

	public gameScript gS_;

	public platformScript pS_;

	private void Awake()
	{
		base.transform.position = new Vector3(60f, 0f, 0f);
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
			if (!scriptMarketing_)
			{
				scriptMarketing_ = guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>();
			}
			if (!scriptMarketingKonsole_)
			{
				scriptMarketingKonsole_ = guiMain_.uiObjects[321].GetComponent<Menu_Marketing_KonsoleKampagne>();
			}
		}
	}

	private void Update()
	{
		FindMyObject();
		if ((bool)gS_ && !gS_.isOnMarket && gS_.sellsTotal > 0)
		{
			Abbrechen();
		}
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
		if ((bool)gS_ || (bool)pS_)
		{
			return;
		}
		switch (typ)
		{
		case 0:
			if (!gS_)
			{
				gS_ = FindGameScriptWithID(targetID);
			}
			break;
		case 1:
			if (!pS_)
			{
				pS_ = FindPlatformScriptWithID(targetID);
			}
			break;
		}
		if (!gS_ && !pS_)
		{
			Abbrechen();
		}
	}

	private platformScript FindPlatformScriptWithID(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == id_)
			{
				return mS_.arrayPlatformsScripts[i];
			}
		}
		return null;
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
		return guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>().sprites[kampagne];
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
		int roomID_ = FindMyRoomWithTask();
		string text = "";
		switch (typ)
		{
		case 0:
			text = tS_.GetText(529);
			text = text.Replace("<NAME1>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[6]);
			if (gS_.hype < (float)scriptMarketing_.maxHype[kampagne])
			{
				gS_.AddHype(scriptMarketing_.hypeProKampagne[kampagne]);
				if (gS_.hype > (float)scriptMarketing_.maxHype[kampagne])
				{
					gS_.hype = scriptMarketing_.maxHype[kampagne];
				}
			}
			gS_.costs_marketing += scriptMarketing_.preise[kampagne];
			break;
		case 1:
			text = tS_.GetText(529);
			text = text.Replace("<NAME1>", "<b><color=blue>" + pS_.GetName() + "</color></b>");
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[6]);
			if (pS_.hype < (float)scriptMarketing_.maxHype[kampagne])
			{
				pS_.AddHype(scriptMarketing_.hypeProKampagne[kampagne]);
				if (pS_.hype > (float)scriptMarketing_.maxHype[kampagne])
				{
					pS_.hype = scriptMarketing_.maxHype[kampagne];
				}
			}
			pS_.costs_marketing += scriptMarketing_.preise[kampagne];
			break;
		}
		if (!DoAutomatic())
		{
			Object.Destroy(base.gameObject);
		}
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

	public bool WaitForMinimumHype()
	{
		if (automatic)
		{
			FindScripts();
			FindMyObject();
			if (disableWarten)
			{
				return false;
			}
			if (typ == 0)
			{
				if ((bool)gS_)
				{
					if (gS_.hype + (float)scriptMarketing_.hypeProKampagne[kampagne] >= (float)scriptMarketing_.maxHype[kampagne])
					{
						return true;
					}
					return false;
				}
			}
			else if ((bool)pS_)
			{
				if (pS_.hype + (float)scriptMarketingKonsole_.hypeProKampagne[kampagne] >= (float)scriptMarketingKonsole_.maxHype[kampagne])
				{
					return true;
				}
				return false;
			}
		}
		return false;
	}

	private bool DoAutomatic()
	{
		FindMyObject();
		if (!automatic)
		{
			return false;
		}
		if (stopAutomatic)
		{
			if ((bool)gS_ && gS_.hype >= (float)scriptMarketing_.maxHype[kampagne])
			{
				LeftNews(tS_.GetText(730), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[6]);
				return false;
			}
			if ((bool)pS_ && pS_.hype >= (float)scriptMarketingKonsole_.maxHype[kampagne])
			{
				LeftNews(tS_.GetText(730), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[6]);
				return false;
			}
		}
		if (typ == 0)
		{
			if (mS_.money < scriptMarketing_.preise[kampagne])
			{
				LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[6]);
				return false;
			}
			mS_.Pay(scriptMarketing_.preise[kampagne], 12);
		}
		if (typ == 1)
		{
			if (mS_.money < scriptMarketingKonsole_.preise[kampagne])
			{
				LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[6]);
				return false;
			}
			mS_.Pay(scriptMarketingKonsole_.preise[kampagne], 12);
		}
		pointsLeft = points;
		return true;
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
		float f = 0f;
		if (typ == 0)
		{
			f = (float)scriptMarketing_.preise[kampagne] * ((100f - GetProzent()) * 0.01f);
		}
		if (typ == 1)
		{
			f = (float)scriptMarketingKonsole_.preise[kampagne] * ((100f - GetProzent()) * 0.01f);
		}
		return Mathf.RoundToInt(f);
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
