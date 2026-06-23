using UnityEngine;

public class taskWait : MonoBehaviour
{
	public int myID = -1;

	public int art = -1;

	private float waitTimer = 10f;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private void Awake()
	{
		base.transform.position = new Vector3(260f, 0f, 0f);
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
		FindScripts();
		if (art == 0)
		{
			return guiMain_.uiSprites[18];
		}
		return null;
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
		if (!mS_ || !mS_.games_ || art != 0)
		{
			return;
		}
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if (!mS_.games_.arrayGamesScripts[i])
			{
				continue;
			}
			gameScript gameScript2 = mS_.games_.arrayGamesScripts[i];
			if (!gameScript2 || gameScript2.spielbericht || !guiMain_.uiObjects[181].GetComponent<Menu_QA_NewSpielberichtSelectGame>().CheckGameData(gameScript2))
			{
				continue;
			}
			taskSpielbericht taskSpielbericht2 = guiMain_.AddTask_Spielbericht();
			taskSpielbericht2.Init(fromSavegame: false);
			taskSpielbericht2.targetID = gameScript2.myID;
			taskSpielbericht2.automatic = true;
			taskSpielbericht2.automaticWait = true;
			taskSpielbericht2.points = guiMain_.uiObjects[181].GetComponent<Menu_QA_NewSpielberichtSelectGame>().GetWorkPoints(gameScript2);
			taskSpielbericht2.pointsLeft = taskSpielbericht2.points;
			for (int j = 0; j < mS_.arrayRoomScripts.Length; j++)
			{
				if ((bool)mS_.arrayRoomScripts[j] && (bool)mS_.arrayRoomScripts[j] && mS_.arrayRoomScripts[j].taskID == myID)
				{
					mS_.arrayRoomScripts[j].taskID = taskSpielbericht2.myID;
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
