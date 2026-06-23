using UnityEngine;

public class taskArcadeProduction : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public float points;

	public float pointsLeft;

	public bool produceAutomatikAllGames;

	private GameObject main_;

	public mainScript mS_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	public gameScript gS_;

	private games games_;

	public roomScript rS_;

	private float timerProduceAutomatikAllGames;

	private float findMyRoomTimer;

	private void Awake()
	{
		base.transform.position = new Vector3(170f, 0f, 0f);
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
		if (!produceAutomatikAllGames)
		{
			IsGameFromMarket();
		}
		if (produceAutomatikAllGames)
		{
			timerProduceAutomatikAllGames += Time.deltaTime;
			if (!(timerProduceAutomatikAllGames < 1f))
			{
				timerProduceAutomatikAllGames = 0f;
				ProduceAutomatikAllGames();
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

	private void IsGameFromMarket()
	{
		if ((bool)gS_ && !gS_.isOnMarket)
		{
			Abbrechen();
		}
	}

	private void FindMyObject()
	{
		if (!gS_ && targetID != -1)
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
		if (!gS_ || !mS_)
		{
			return;
		}
		findMyRoomTimer += Time.deltaTime;
		if (findMyRoomTimer < 0.2f)
		{
			return;
		}
		findMyRoomTimer = 0f;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == myID)
			{
				rS_ = mS_.arrayRoomScripts[i];
				break;
			}
		}
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
		pointsLeft = points;
		if (!gS_ || !guiMain_)
		{
			return;
		}
		int num = 50;
		if (num > gS_.vorbestellungen)
		{
			num = gS_.vorbestellungen;
		}
		if (num > 0)
		{
			int num2 = gS_.verkaufspreis[0] * num;
			int num3 = gS_.arcadeProdCosts * num;
			gS_.vorbestellungen -= num;
			gS_.sellsTotal += num;
			gS_.umsatzTotal += num2;
			gS_.costs_production += num3;
			mS_.Earn(num2, 3);
			mS_.Pay(num3, 21);
			gS_.PlayerPayEngineLicence(num2);
			if ((bool)rS_)
			{
				StartCoroutine(guiMain_.MoneyPopEnumerate(num2 - num3, rS_.uiPos, green: true));
			}
			if (gS_.vorbestellungen <= 0)
			{
				gS_.vorbestellungen = 0;
				ProduceAutomatikAllGames();
			}
		}
	}

	private void ProduceAutomatikAllGames()
	{
		if (!produceAutomatikAllGames || ((bool)gS_ && gS_.vorbestellungen > 0))
		{
			return;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && CheckGameData(games_.arrayGamesScripts[i]) && games_.arrayGamesScripts[i].vorbestellungen > 0)
			{
				gameScript gameScript2 = games_.arrayGamesScripts[i];
				targetID = gameScript2.myID;
				gS_ = null;
				FindMyObject();
				return;
			}
		}
		targetID = -1;
		gS_ = null;
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && !script_.inDevelopment && script_.isOnMarket && script_.arcade && script_.publisherID == mS_.myID && script_.gameTyp != 2)
		{
			return true;
		}
		return false;
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
