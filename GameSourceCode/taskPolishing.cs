using UnityEngine;

public class taskPolishing : MonoBehaviour
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

	private games games_;

	public gameScript gS_;

	public roomScript rS_;

	private float findMyRoomTimer;

	private void Awake()
	{
		base.transform.position = new Vector3(240f, 0f, 0f);
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

	public void Work(float f, roomScript myRoomS_, bool hype)
	{
		FindScripts();
		if (!gS_)
		{
			FindMyObject();
		}
		if (!gS_)
		{
			return;
		}
		if ((bool)gS_)
		{
			if (myRoomS_.typ == 4)
			{
				gS_.points_grafik += f;
				RemoveInvisBug();
			}
			if (myRoomS_.typ == 5)
			{
				gS_.points_sound += f;
				RemoveInvisBug();
			}
			if (myRoomS_.typ == 3)
			{
				gS_.points_gameplay += f;
				RemoveInvisBug();
			}
			if (myRoomS_.typ == 10)
			{
				gS_.points_technik += f;
				RemoveInvisBug();
			}
			if (hype && gS_.GetHype() < 50f)
			{
				gS_.AddHype(f);
			}
		}
		if (pointsLeft > 0f)
		{
			pointsLeft -= 1f;
			if (pointsLeft <= 0f)
			{
				FindMyObject();
				pointsLeft = points;
				Complete(myRoomS_);
			}
		}
	}

	private void RemoveInvisBug()
	{
		if (Random.Range(0, 100) >= 90)
		{
			gS_.points_bugsInvis -= 1f;
			if (gS_.points_bugsInvis < 0f)
			{
				gS_.points_bugsInvis = 0f;
			}
		}
	}

	private void Complete(roomScript myRoomS_)
	{
		if ((bool)gS_)
		{
			if (myRoomS_.typ == 4)
			{
				gS_.points_grafik += 10f;
			}
			if (myRoomS_.typ == 5)
			{
				gS_.points_sound += 10f;
			}
			if (myRoomS_.typ == 3)
			{
				gS_.points_gameplay += 10f;
			}
			if (myRoomS_.typ == 10)
			{
				gS_.points_technik += 10f;
			}
		}
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
