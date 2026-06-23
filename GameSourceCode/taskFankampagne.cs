using UnityEngine;

public class taskFankampagne : MonoBehaviour
{
	public int myID = -1;

	public int kampagne = -1;

	public bool automatic;

	public bool stopAutomatic;

	public float points;

	public float pointsLeft;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public gameScript gS_;

	public platformScript pS_;

	private void Awake()
	{
		base.transform.position = new Vector3(190f, 0f, 0f);
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

	public float GetProzent()
	{
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		FindScripts();
		return guiMain_.uiObjects[139].GetComponent<Menu_Support_Fankampagne>().sprites[kampagne];
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
		int roomID_ = FindMyRoomWithTask();
		string text = tS_.GetText(748);
		text = text.Replace("<NAME>", "<b><color=blue>" + tS_.GetText(740 + kampagne) + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[7]);
		Menu_Support_Fankampagne component = guiMain_.uiObjects[139].GetComponent<Menu_Support_Fankampagne>();
		mS_.AddFans(component.fans[kampagne], -1);
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
		Menu_Support_Fankampagne component = guiMain_.uiObjects[139].GetComponent<Menu_Support_Fankampagne>();
		if (mS_.money < component.preise[kampagne])
		{
			LeftNews(tS_.GetText(728), guiMain_.uiSprites[16], rdS_.roomData_SPRITE[7]);
			return false;
		}
		mS_.Pay(component.preise[kampagne], 16);
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
		return Mathf.RoundToInt((float)guiMain_.uiObjects[139].GetComponent<Menu_Support_Fankampagne>().preise[kampagne] * ((100f - GetProzent()) * 0.01f));
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
