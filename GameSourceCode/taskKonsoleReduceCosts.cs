using UnityEngine;

public class taskKonsoleReduceCosts : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public float points;

	public float pointsLeft;

	public bool[] adds = new bool[9];

	public int aktuellerAdd = -1;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public platformScript pS_;

	public Menu_Dev_KonsoleKostenreduktion menu_;

	private void Awake()
	{
		base.transform.position = new Vector3(290f, 0f, 0f);
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
			if (!menu_)
			{
				menu_ = guiMain_.uiObjects[450].GetComponent<Menu_Dev_KonsoleKostenreduktion>();
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
		if (!pS_)
		{
			pS_ = FindKonsoleScriptWithID(targetID);
			if (!pS_)
			{
				Abbrechen();
			}
		}
	}

	private platformScript FindKonsoleScriptWithID(int id_)
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

	public float GetProzent()
	{
		if (points <= 0f)
		{
			points = 10f;
		}
		return 100f / points * (points - pointsLeft);
	}

	public Sprite GetPic()
	{
		if (aktuellerAdd >= 0 && aktuellerAdd < menu_.sprites.Length)
		{
			return menu_.sprites[aktuellerAdd];
		}
		return null;
	}

	public void Work(float f)
	{
		FindScripts();
		if (!pS_)
		{
			FindMyObject();
		}
		if ((bool)pS_ && pointsLeft > 0f)
		{
			pointsLeft -= f;
			if ((bool)pS_ && pointsLeft <= 0f)
			{
				pointsLeft = 0f;
				Complete();
			}
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
		if (pS_.kostenreduktion >= 100f)
		{
			aktuellerAdd = -1;
		}
		if (aktuellerAdd != -1)
		{
			float devPointsStart = pS_.devPointsStart;
			points = devPointsStart * menu_.workPoints[aktuellerAdd];
			pointsLeft = points;
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Complete()
	{
		FindMyObject();
		adds[aktuellerAdd] = false;
		pS_.kostenreduktionDone[aktuellerAdd] = true;
		pS_.costs_preisreduktion += (int)menu_.GetCosts(aktuellerAdd, pS_);
		if (!pS_.IsOutdatet())
		{
			pS_.kostenreduktion += menu_.reduktionProKampagne[aktuellerAdd];
			if (pS_.kostenreduktion > 100f)
			{
				pS_.kostenreduktion = 100f;
			}
			pS_.AutoPreis();
		}
		int roomID_ = FindMyRoomWithTask();
		string text = tS_.GetText(2366);
		text = text.Replace("<NAME1>", "<b><color=blue>" + pS_.GetName() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[8]);
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
				num += (float)menu_.GetCosts(i, pS_);
			}
		}
		float num2 = menu_.GetCosts(aktuellerAdd, pS_);
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
