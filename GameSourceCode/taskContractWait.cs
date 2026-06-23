using UnityEngine;

public class taskContractWait : MonoBehaviour
{
	public int myID = -1;

	public int art = -1;

	private float waitTimer = 10f;

	public GameObject main_;

	public mainScript mS_;

	public GUI_Main guiMain_;

	public textScript tS_;

	public roomDataScript rdS_;

	private void Awake()
	{
		base.transform.position = new Vector3(220f, 0f, 0f);
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
		AutomaticWait(art);
	}

	public Sprite GetPic()
	{
		return guiMain_.uiSprites[10];
	}

	private void AutomaticWait(int art_)
	{
		if (art_ == -1)
		{
			return;
		}
		waitTimer += Time.deltaTime;
		if (waitTimer < 5f)
		{
			return;
		}
		waitTimer = 0f;
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			contractWork component = array[i].GetComponent<contractWork>();
			if (!component || component.art != art_ || component.IsAngenommen())
			{
				continue;
			}
			component.angenommen = true;
			taskContractWork taskContractWork2 = guiMain_.AddTask_ContractWork();
			taskContractWork2.Init(fromSavegame: false);
			taskContractWork2.contractID = component.myID;
			taskContractWork2.points = component.GetArbeitsaufwand();
			taskContractWork2.pointsLeft = component.GetArbeitsaufwand();
			taskContractWork2.automatic = true;
			taskContractWork2.automaticWait = true;
			for (int j = 0; j < mS_.arrayRoomScripts.Length; j++)
			{
				if ((bool)mS_.arrayRoomScripts[j] && (bool)mS_.arrayRoomScripts[j] && mS_.arrayRoomScripts[j].taskID == myID)
				{
					mS_.arrayRoomScripts[j].taskID = taskContractWork2.myID;
				}
			}
			Object.Destroy(base.gameObject);
			break;
		}
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
