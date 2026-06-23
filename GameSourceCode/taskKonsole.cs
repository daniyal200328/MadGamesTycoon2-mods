using UnityEngine;

public class taskKonsole : MonoBehaviour
{
	public int myID = -1;

	public int konsoleID = -1;

	public int leitenderTechnikerID = -1;

	public int proKonsoleID = -1;

	public characterScript techniker_;

	public platformScript pS_;

	private GameObject main_;

	public mainScript mS_;

	private engineFeatures eF_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private games games_;

	private void Awake()
	{
		base.transform.position = new Vector3(180f, 0f, 0f);
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
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!hardware_)
			{
				hardware_ = main_.GetComponent<hardware>();
			}
			if (!hardwareFeatures_)
			{
				hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
			}
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
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

	private void Update()
	{
		FindMyKonsole();
		FindMyLeitenderTechniker();
	}

	private void FindMyKonsole()
	{
		if (!pS_)
		{
			pS_ = FindPlatformScriptWithID(konsoleID);
			if (!pS_)
			{
				Object.Destroy(base.gameObject);
			}
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

	private void FindMyLeitenderTechniker()
	{
		if (leitenderTechnikerID == -1)
		{
			return;
		}
		if (!techniker_)
		{
			GameObject gameObject = GameObject.Find("CHAR_" + leitenderTechnikerID);
			if ((bool)gameObject)
			{
				techniker_ = gameObject.GetComponent<characterScript>();
				return;
			}
			leitenderTechnikerID = -1;
			techniker_ = null;
		}
		else if ((bool)techniker_.roomS_)
		{
			if (techniker_.roomS_.taskID != myID)
			{
				leitenderTechnikerID = -1;
				techniker_ = null;
			}
		}
		else
		{
			leitenderTechnikerID = -1;
			techniker_ = null;
		}
	}

	public float GetProzent()
	{
		FindScripts();
		if (!pS_)
		{
			return -1f;
		}
		return pS_.GetProzent();
	}

	public void Work(float f)
	{
		FindScripts();
		if (!pS_)
		{
			FindMyKonsole();
		}
		if ((bool)pS_ && pS_.devPoints > 0f)
		{
			pS_.devPoints -= f;
			pS_.AddHaltbarkeit(f);
			if (pS_.devPoints <= 0f)
			{
				pS_.devPoints = 0f;
				Complete();
			}
		}
	}

	private void Complete()
	{
		FindScripts();
		if (!pS_)
		{
			FindMyKonsole();
		}
		if ((bool)pS_)
		{
			int roomID_ = FindMyRoomWithTask();
			string tooltip_ = tS_.GetText(1635) + "\n<b>" + pS_.GetName() + "</b>";
			guiMain_.CreateLeftNews(roomID_, pS_.GetTypSprite(), tooltip_, rdS_.roomData_SPRITE[8]);
			if (!mS_.multiplayer)
			{
				CompleteOpenMenue();
			}
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

	public void CompleteOpenMenue()
	{
		FindScripts();
		if (!pS_)
		{
			FindMyKonsole();
		}
		if ((bool)pS_)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[326]);
			guiMain_.uiObjects[326].GetComponent<Menu_Dev_KonsoleComplete>().Init(pS_, this);
			guiMain_.OpenMenu(hideChars: false);
			if ((bool)mS_.sfx_)
			{
				mS_.sfx_.PlaySound(37, force: false);
			}
		}
	}

	public int GetRueckgeld()
	{
		if ((bool)pS_)
		{
			long num = pS_.entwicklungsKosten / 100 * (100 - Mathf.RoundToInt(GetProzent()));
			if (num > 2000000000)
			{
				num = 2000000000L;
			}
			return Mathf.RoundToInt(num);
		}
		return 0;
	}

	public void Abbrechen()
	{
		FindScripts();
		if (!pS_)
		{
			FindMyKonsole();
		}
		if (!pS_)
		{
			return;
		}
		int rueckgeld = GetRueckgeld();
		if (proKonsoleID == -1 && pS_.vorgaengerID != -1)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + pS_.vorgaengerID);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component)
				{
					component.nachfolgerID = -1;
				}
			}
		}
		if (proKonsoleID != 1)
		{
			GameObject gameObject2 = GameObject.Find("PLATFORM_" + proKonsoleID);
			if ((bool)gameObject2)
			{
				platformScript component2 = gameObject2.GetComponent<platformScript>();
				if ((bool)component2)
				{
					component2.proVersion = false;
				}
			}
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i])
			{
				continue;
			}
			for (int j = 0; j < games_.arrayGamesScripts[i].gamePlatform.Length; j++)
			{
				if (games_.arrayGamesScripts[i].gamePlatform[j] == konsoleID)
				{
					games_.arrayGamesScripts[i].gamePlatform[j] = -1;
					games_.arrayGamesScripts[i].gamePlatformScript[j] = null;
				}
			}
		}
		if (rueckgeld > 0)
		{
			mS_.Earn(rueckgeld, 1);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int k = 0; k < array.Length; k++)
			{
				roomScript component3 = array[k].GetComponent<roomScript>();
				if ((bool)component3 && component3.taskID == myID)
				{
					guiMain_.MoneyPop(rueckgeld, new Vector3(component3.uiPos.x, component3.uiPos.y + 3f, component3.uiPos.z), green: true);
					break;
				}
			}
		}
		Object.Destroy(pS_.gameObject);
		Object.Destroy(base.gameObject);
	}
}
