using UnityEngine;

public class taskEngine : MonoBehaviour
{
	public int myID = -1;

	public int engineID = -1;

	public engineScript eS_;

	private GameObject main_;

	private mainScript mS_;

	private engineFeatures eF_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private void Awake()
	{
		base.transform.position = new Vector3(20f, 0f, 0f);
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
		FindMyEngine();
	}

	private void FindMyEngine()
	{
		if (!eS_)
		{
			eS_ = FindEngineScriptWithID(engineID);
			if (!eS_)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private engineScript FindEngineScriptWithID(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == id_)
			{
				return mS_.arrayEnginesScripts[i];
			}
		}
		return null;
	}

	public float GetProzent()
	{
		FindScripts();
		if (!eS_)
		{
			return -1f;
		}
		return eS_.GetProzent();
	}

	public void Work(float f)
	{
		FindScripts();
		if (!eS_)
		{
			FindMyEngine();
		}
		if ((bool)eS_ && eS_.devPoints > 0f)
		{
			eS_.devPoints -= f;
			if (eS_.devPoints <= 0f)
			{
				eS_.devPoints = 0f;
				Complete();
			}
		}
	}

	private void Complete()
	{
		FindScripts();
		if (!eS_)
		{
			FindMyEngine();
		}
		if ((bool)eS_)
		{
			int roomID_ = FindMyRoomWithTask();
			string tooltip_ = tS_.GetText(284) + "\n<b>" + eS_.GetName() + "</b>";
			guiMain_.CreateLeftNews(roomID_, guiMain_.uiSprites[4], tooltip_, rdS_.roomData_SPRITE[1]);
			eS_.SetComplete();
			if ((bool)mS_.achScript_)
			{
				mS_.achScript_.SetAchivement(23);
			}
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

	public int GetRueckgeld()
	{
		int num = 0;
		for (int i = 0; i < eS_.featuresInDev.Length; i++)
		{
			if (eS_.featuresInDev[i])
			{
				num += eF_.GetDevCostsForEngine(i);
			}
		}
		return num;
	}

	public void Abbrechen()
	{
		FindScripts();
		if (!eS_)
		{
			FindMyEngine();
		}
		if (!eS_)
		{
			return;
		}
		int rueckgeld = GetRueckgeld();
		if (rueckgeld > 0)
		{
			mS_.Earn(rueckgeld, 1);
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == myID)
				{
					guiMain_.MoneyPop(rueckgeld, new Vector3(component.uiPos.x, component.uiPos.y + 3f, component.uiPos.z), green: true);
					break;
				}
			}
		}
		if (!eS_.updating)
		{
			Object.Destroy(eS_.gameObject);
		}
		else
		{
			eS_.EntwicklungBeenden();
		}
		Object.Destroy(base.gameObject);
	}
}
