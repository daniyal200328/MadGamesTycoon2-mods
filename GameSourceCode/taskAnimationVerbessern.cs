using UnityEngine;

public class taskAnimationVerbessern : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public float points;

	public float pointsLeft;

	public bool[] adds = new bool[6];

	public int aktuellerAdd = -1;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public gameScript gS_;

	private Menu_MOCAP_AnimationVerbessern menuMOCAP_;

	private games games_;

	public roomScript rS_;

	private float findMyRoomTimer;

	private void Awake()
	{
		base.transform.position = new Vector3(150f, 0f, 0f);
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
			if (!menuMOCAP_)
			{
				menuMOCAP_ = guiMain_.uiObjects[178].GetComponent<Menu_MOCAP_AnimationVerbessern>();
			}
		}
	}

	private void Update()
	{
		FindMyObject();
		FindMyRoom();
		GamePublished();
	}

	private void GamePublished()
	{
		if ((bool)gS_ && !gS_.inDevelopment)
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
		if (!gS_)
		{
			gS_ = FindGameScriptWithID(targetID);
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

	private void FindMyRoom()
	{
		if (!gS_)
		{
			return;
		}
		findMyRoomTimer += Time.deltaTime;
		if (findMyRoomTimer < 0.2f)
		{
			return;
		}
		findMyRoomTimer = 0f;
		if ((bool)rS_ && rS_.taskID != -1)
		{
			GameObject taskGameObject = rS_.taskGameObject;
			if ((bool)taskGameObject)
			{
				taskGame component = taskGameObject.GetComponent<taskGame>();
				if ((bool)component && component.gameID == targetID)
				{
					return;
				}
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			roomScript component2 = array[i].GetComponent<roomScript>();
			if (!component2 || component2.taskID == -1)
			{
				continue;
			}
			GameObject taskGameObject2 = component2.taskGameObject;
			if ((bool)taskGameObject2)
			{
				taskGame component3 = taskGameObject2.GetComponent<taskGame>();
				if ((bool)component3 && component3.gameID == targetID)
				{
					rS_ = component2;
					return;
				}
			}
		}
		Abbrechen();
	}

	public float GetProzent()
	{
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		if (aktuellerAdd >= 0 && aktuellerAdd < games_.gameAdds.Length)
		{
			return games_.gameAdds[aktuellerAdd + 18];
		}
		return null;
	}

	public void Work(float f, int what)
	{
		FindScripts();
		if (!gS_)
		{
			FindMyObject();
		}
		if (!gS_ || !(pointsLeft > 0f))
		{
			return;
		}
		pointsLeft -= 1f;
		if ((bool)gS_)
		{
			switch (what)
			{
			case 3:
				gS_.points_technik += f;
				break;
			case 5:
				if (gS_.GetHype() < 50f)
				{
					gS_.AddHype(f);
				}
				break;
			}
		}
		if (pointsLeft <= 0f)
		{
			pointsLeft = 0f;
			Complete();
		}
	}

	public void FindNewAdd()
	{
		FindScripts();
		FindMyObject();
		aktuellerAdd = -1;
		for (int i = 0; i < adds.Length; i++)
		{
			if (adds[i])
			{
				aktuellerAdd = i;
				break;
			}
		}
		if (aktuellerAdd != -1)
		{
			float num = gS_.GetGesamtDevPoints();
			points = num * menuMOCAP_.pointsInPercent[aktuellerAdd];
			pointsLeft = points;
		}
		else
		{
			guiMain_.uiObjects[279].GetComponent<Menu_ROOM_Polishing>().StartPolishingAutomatic(gS_, myID);
			Object.Destroy(base.gameObject);
		}
	}

	private void Complete()
	{
		FindMyObject();
		adds[aktuellerAdd] = false;
		gS_.motionCaptureStudio[aktuellerAdd] = true;
		gS_.costs_entwicklung += menuMOCAP_.GetCosts(aktuellerAdd, gS_);
		int roomID_ = FindMyRoomWithTask();
		string text = tS_.GetText(923);
		text = text.Replace("<NAME1>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[10]);
		FindNewAdd();
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
		float num = 0f;
		for (int i = 0; i < adds.Length; i++)
		{
			if (aktuellerAdd != i && adds[i])
			{
				num += (float)menuMOCAP_.GetCosts(i, gS_);
			}
		}
		float num2 = menuMOCAP_.GetCosts(aktuellerAdd, gS_);
		num2 -= num2 * (GetProzent() * 0.01f);
		return Mathf.RoundToInt(num + num2);
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
