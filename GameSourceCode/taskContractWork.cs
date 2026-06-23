using UnityEngine;

public class taskContractWork : MonoBehaviour
{
	public int myID = -1;

	public int contractID = -1;

	public bool automatic;

	public float points;

	public float pointsLeft;

	public contractWork contract_;

	public bool automaticWait;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private void Awake()
	{
		base.transform.position = new Vector3(210f, 0f, 0f);
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
		FindMyContractWork();
	}

	private void FindMyContractWork()
	{
		if (!contract_)
		{
			GameObject gameObject = GameObject.Find("CONTRACTWORK_" + contractID);
			if ((bool)gameObject)
			{
				contract_ = gameObject.GetComponent<contractWork>();
				return;
			}
			Debug.Log("DESTROY TASK-CONTRACTWORK!");
			Object.Destroy(base.gameObject);
		}
	}

	public float GetProzent()
	{
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		return guiMain_.uiSprites[10];
	}

	public string GetName()
	{
		FindMyContractWork();
		if ((bool)contract_)
		{
			return contract_.GetName();
		}
		return "";
	}

	public int GetStrafe()
	{
		FindMyContractWork();
		if ((bool)contract_)
		{
			return contract_.GetStrafe();
		}
		return 0;
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
		FindMyContractWork();
		mS_.Earn(contract_.GetGehalt(), 5);
		guiMain_.UpdateAuftragsansehen(contract_.GetAuftragsansehen());
		mS_.AddStudioPoints(1);
		roomScript roomScript2 = FindMyRoomWithTask();
		if (!roomScript2)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		int roomID_ = roomScript2.myID;
		string text = tS_.GetText(607);
		text = text.Replace("<NAME>", "<b><color=blue>" + contract_.GetName() + "</color></b>");
		text = text.Replace("<NUM>", "<b><color=green>" + mS_.GetMoney(contract_.GetGehalt(), showDollar: true) + "</color></b>");
		switch (contract_.art)
		{
		case 0:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[1]);
			break;
		case 1:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[3]);
			break;
		case 2:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[4]);
			break;
		case 3:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[5]);
			break;
		case 4:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[10]);
			break;
		case 5:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[14]);
			break;
		case 6:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[17]);
			break;
		case 7:
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[8]);
			break;
		}
		int art = contract_.art;
		if ((bool)contract_)
		{
			Object.Destroy(contract_.gameObject);
		}
		if (!DoAutomatic(art))
		{
			if (automatic && automaticWait)
			{
				taskContractWait taskContractWait2 = guiMain_.AddTask_ContractWait();
				taskContractWait2.Init(fromSavegame: false);
				taskContractWait2.art = art;
				roomScript2.taskID = taskContractWait2.myID;
				Debug.Log("ContractWorkWait");
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

	private bool DoAutomatic(int art_)
	{
		if (!automatic)
		{
			return false;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				contractWork component = array[i].GetComponent<contractWork>();
				if ((bool)component && component.art == art_ && !component.IsAngenommen())
				{
					component.angenommen = true;
					contract_ = null;
					contractID = component.myID;
					points = component.GetArbeitsaufwand();
					pointsLeft = component.GetArbeitsaufwand();
					return true;
				}
			}
		}
		if (automaticWait)
		{
			return false;
		}
		switch (art_)
		{
		case 0:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[1]);
			break;
		case 1:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[3]);
			break;
		case 2:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[4]);
			break;
		case 3:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[5]);
			break;
		case 4:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[10]);
			break;
		case 5:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[14]);
			break;
		case 6:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[17]);
			break;
		case 7:
			LeftNews(tS_.GetText(729), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[8]);
			break;
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

	public void Abbrechen()
	{
		FindMyContractWork();
		int strafe = GetStrafe();
		if (strafe > 0)
		{
			mS_.Pay(Mathf.RoundToInt(strafe), 14);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == myID)
				{
					guiMain_.MoneyPop(Mathf.RoundToInt(strafe), new Vector3(component.uiPos.x, component.uiPos.y + 3f, component.uiPos.z), green: false);
					break;
				}
			}
		}
		if ((bool)contract_)
		{
			contract_.angenommen = false;
			guiMain_.UpdateAuftragsansehen(0f - contract_.GetAuftragsansehen());
		}
		Object.Destroy(base.gameObject);
	}
}
