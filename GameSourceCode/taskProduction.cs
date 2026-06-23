using UnityEngine;

public class taskProduction : MonoBehaviour
{
	public int myID = -1;

	public int targetID = -1;

	public bool automatic;

	public int amountStandard;

	public int amountDeluxe;

	public int amountCollectors;

	public int gesamtProduktion;

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

	private void Awake()
	{
		base.transform.position = new Vector3(160f, 0f, 0f);
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
		if ((bool)gS_)
		{
			GameRemovedFromMarket();
		}
		if (automatic && (bool)gS_)
		{
			CheckAutomatic();
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

	private void GameRemovedFromMarket()
	{
		if ((bool)gS_ && !gS_.isOnMarket)
		{
			if (!produceAutomatikAllGames)
			{
				Abbrechen();
				return;
			}
			amountStandard = 1;
			amountDeluxe = 0;
			amountCollectors = 0;
			gesamtProduktion = 0;
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

	public float GetProzent()
	{
		float num = gesamtProduktion;
		num *= 0.01f;
		return (float)(gesamtProduktion - (amountStandard + amountDeluxe + amountCollectors)) / num;
	}

	public int GetAmount()
	{
		return amountStandard + amountDeluxe + amountCollectors;
	}

	public Sprite GetPic()
	{
		return guiMain_.uiSprites[26];
	}

	public void Work(long i, Vector3 pos)
	{
		if (!gS_ || (amountStandard <= 0 && amountDeluxe <= 0 && amountCollectors <= 0))
		{
			return;
		}
		if (games_.GetFreeLagerplatz() < i)
		{
			i = games_.GetFreeLagerplatz();
		}
		if (i > 0 && amountStandard > 0)
		{
			if (amountStandard >= i)
			{
				amountStandard -= (int)i;
				gS_.lagerbestand[0] += i;
				games_.LagerplatzVerteilenEinGame(i);
				int num = Mathf.RoundToInt((float)i * gS_.GetProduktionskosten(0));
				gS_.costs_production += num;
				mS_.Pay(num, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num, pos, green: false));
				i = 0L;
			}
			else
			{
				i -= amountStandard;
				gS_.lagerbestand[0] += amountStandard;
				games_.LagerplatzVerteilenEinGame(amountStandard);
				int num2 = Mathf.RoundToInt((float)amountStandard * gS_.GetProduktionskosten(0));
				gS_.costs_production += num2;
				mS_.Pay(num2, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num2, pos, green: false));
				amountStandard = 0;
			}
		}
		if (i > 0 && amountDeluxe > 0)
		{
			if (amountDeluxe >= i)
			{
				amountDeluxe -= (int)i;
				gS_.lagerbestand[1] += i;
				games_.LagerplatzVerteilenEinGame(i);
				int num3 = Mathf.RoundToInt((float)i * gS_.GetProduktionskosten(1));
				gS_.costs_production += num3;
				mS_.Pay(num3, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num3, pos, green: false));
				i = 0L;
			}
			else
			{
				i -= amountDeluxe;
				gS_.lagerbestand[1] += amountDeluxe;
				games_.LagerplatzVerteilenEinGame(amountDeluxe);
				int num4 = Mathf.RoundToInt((float)amountDeluxe * gS_.GetProduktionskosten(1));
				gS_.costs_production += num4;
				mS_.Pay(num4, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num4, pos, green: false));
				amountDeluxe = 0;
			}
		}
		if (i > 0 && amountCollectors > 0)
		{
			if (amountCollectors >= i)
			{
				amountCollectors -= (int)i;
				gS_.lagerbestand[2] += i;
				games_.LagerplatzVerteilenEinGame(i);
				int num5 = Mathf.RoundToInt((float)i * gS_.GetProduktionskosten(2));
				gS_.costs_production += num5;
				mS_.Pay(num5, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num5, pos, green: false));
				i = 0L;
			}
			else
			{
				i -= amountCollectors;
				gS_.lagerbestand[2] += amountCollectors;
				games_.LagerplatzVerteilenEinGame(amountCollectors);
				int num6 = Mathf.RoundToInt((float)amountCollectors * gS_.GetProduktionskosten(2));
				gS_.costs_production += num6;
				mS_.Pay(num6, 21);
				StartCoroutine(guiMain_.MoneyPopEnumerate(num6, pos, green: false));
				amountCollectors = 0;
			}
		}
		Complete();
	}

	public bool WaitForAutomatic()
	{
		if (!automatic)
		{
			return false;
		}
		if (amountStandard <= 0 && amountDeluxe <= 0 && amountCollectors <= 0)
		{
			return true;
		}
		return false;
	}

	private bool CheckAutomatic()
	{
		if (!automatic)
		{
			return false;
		}
		if (amountStandard <= 0 && amountDeluxe <= 0 && amountCollectors <= 0)
		{
			FindMyObject();
			float num = gS_.vorbestellungen + gS_.sellsStandard_forProduction * 2;
			num -= (float)gS_.lagerbestand[0];
			if (num >= 1f)
			{
				amountStandard = Mathf.RoundToInt(num + 1f);
				if (gS_.lagerbestand[1] <= gS_.vorbestellungen / 20)
				{
					amountDeluxe = Mathf.RoundToInt(num * 0.03f);
				}
				if (gS_.lagerbestand[2] <= gS_.vorbestellungen / 20)
				{
					amountCollectors = Mathf.RoundToInt(num * 0.02f);
				}
				if (gS_.typ_budget || gS_.typ_bundle || gS_.typ_goty || gS_.typ_addon || gS_.typ_mmoaddon || gS_.typ_addonStandalone)
				{
					amountDeluxe = 0;
					amountCollectors = 0;
				}
				gesamtProduktion = amountStandard + amountDeluxe + amountCollectors;
				return true;
			}
		}
		return false;
	}

	private void Complete()
	{
		if (produceAutomatikAllGames)
		{
			ProduceAutomatikAllGames();
		}
		else if (!automatic && amountStandard <= 0 && amountDeluxe <= 0 && amountCollectors <= 0)
		{
			FindMyObject();
			int roomID_ = FindMyRoomWithTask();
			string text = tS_.GetText(1137);
			text = text.Replace("<NAME1>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
			guiMain_.CreateLeftNews(roomID_, GetPic(), text, rdS_.roomData_SPRITE[14]);
			Object.Destroy(base.gameObject);
		}
	}

	private void ProduceAutomatikAllGames()
	{
		if (!produceAutomatikAllGames || ((bool)gS_ && (amountStandard > 0 || amountDeluxe > 0 || amountCollectors > 0)))
		{
			return;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && CheckGameData(games_.arrayGamesScripts[i]))
			{
				gS_ = games_.arrayGamesScripts[i];
				if (CheckAutomatic())
				{
					targetID = games_.arrayGamesScripts[i].myID;
					gS_ = games_.arrayGamesScripts[i];
					FindMyObject();
					return;
				}
			}
		}
		targetID = -1;
		gS_ = null;
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.publisherID == mS_.myID && !script_.inDevelopment && script_.isOnMarket && script_.retailVersion && script_.gameTyp != 2 && !script_.handy && !script_.arcade)
		{
			return true;
		}
		return false;
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
		return 0;
	}

	public void Abbrechen()
	{
		Object.Destroy(base.gameObject);
	}
}
