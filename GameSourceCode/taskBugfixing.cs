using UnityEngine;

public class taskBugfixing : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public float points;

	public float pointsLeft;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public gameScript gS_;

	public roomScript rS_;

	public games games_;

	private float findMyRoomTimer;

	private void Awake()
	{
		base.transform.position = new Vector3(120f, 0f, 0f);
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
			if (!gS_)
			{
				gS_ = FindGameScriptWithID(targetID);
			}
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
		return null;
	}

	public void Work(float f)
	{
		if (!(pointsLeft > 0f))
		{
			return;
		}
		pointsLeft -= f;
		if (!(pointsLeft <= 0f))
		{
			return;
		}
		FindMyObject();
		pointsLeft = points;
		if (!gS_)
		{
			return;
		}
		gS_.points_bugs -= 1f;
		if (gS_.points_bugs <= 0f)
		{
			gS_.points_bugs = 0f;
			if ((bool)mS_.settings_ && mS_.settings_.returnNullBugs && !mS_.multiplayer)
			{
				CompleteOpenMenue();
			}
		}
	}

	public void CompleteOpenMenue()
	{
		if ((bool)rS_)
		{
			taskGame taskGame2 = rS_.GetTaskGame();
			if ((bool)taskGame2)
			{
				taskGame2.CompleteOpenMenue();
			}
		}
	}

	private void Complete()
	{
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
