using UnityEngine;

public class taskUpdate : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public float points;

	public float pointsLeft;

	public float quality;

	public bool[] sprachen;

	public int devCosts;

	public int pointsGameplay;

	public int pointsSound;

	public int pointsGrafik;

	public int pointsTechnik;

	public int pointsBugs;

	public bool automatic;

	public int autoAmount;

	private GameObject main_;

	private mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private games games_;

	public gameScript gS_;

	private void Awake()
	{
		base.transform.position = new Vector3(30f, 0f, 0f);
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
		CheckAbbruch();
	}

	public void Init(bool fromSavegame)
	{
		if (!fromSavegame)
		{
			myID = Random.Range(1, 100000000);
		}
		base.name = "Task_" + myID;
	}

	private void CheckAbbruch()
	{
		if (!gS_ || gS_.isOnMarket)
		{
			return;
		}
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
		string text = tS_.GetText(842);
		text = text.Replace("<NAME>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, guiMain_.uiSprites[3], text, rdS_.roomData_SPRITE[1]);
		Abbrechen();
	}

	private void FindMyObject()
	{
		if (!gS_)
		{
			gS_ = FindGameScriptWithID(targetID);
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
		return guiMain_.uiSprites[15];
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
		gS_.costs_updates += devCosts;
		gS_.amountUpdates++;
		gS_.bonusSellsUpdates += quality / (float)gS_.amountUpdates;
		if ((double)gS_.bonusSellsUpdates > 1.0)
		{
			gS_.bonusSellsUpdates = 1f;
		}
		gS_.points_gameplay += pointsGameplay;
		gS_.points_grafik += pointsGrafik;
		gS_.points_sound += pointsSound;
		gS_.points_technik += pointsTechnik;
		gS_.points_bugs -= pointsBugs;
		if (gS_.points_bugs < 0f)
		{
			gS_.points_bugs = 0f;
		}
		for (int i = 0; i < sprachen.Length; i++)
		{
			if (sprachen[i])
			{
				gS_.gameLanguage[i] = true;
			}
		}
		int roomID_ = FindMyRoomWithTask();
		string text = tS_.GetText(663);
		text = text.Replace("<NAME>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[1]);
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

	private bool DoAutomatic()
	{
		if (!automatic)
		{
			return false;
		}
		if (autoAmount == 2)
		{
			return false;
		}
		for (int i = 0; i < sprachen.Length; i++)
		{
			if (sprachen[i])
			{
				points -= 10f;
				if (!mS_.Muttersprache(i))
				{
					devCosts -= gS_.GetGesamtDevPoints() * 5;
				}
				if (devCosts < 1)
				{
					devCosts = 1;
				}
				sprachen[i] = false;
			}
		}
		pointsLeft = points;
		if (mS_.money < devCosts)
		{
			LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[1]);
			return false;
		}
		mS_.Pay(devCosts, 15);
		if (autoAmount > 0)
		{
			autoAmount--;
		}
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
		return Mathf.RoundToInt((float)devCosts * ((100f - GetProzent()) * 0.01f));
	}

	public void Abbrechen()
	{
		FindMyObject();
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
