using UnityEngine;

public class taskGame : MonoBehaviour
{
	public int myID = -1;

	public int gameID = -1;

	public int leitenderDesignerID = -1;

	public bool randomEvent;

	public gameScript gS_;

	public characterScript designer_;

	private GameObject main_;

	private mainScript mS_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private GUI_Main guiMain_;

	private textScript tS_;

	private roomDataScript rdS_;

	private sfxScript sfx_;

	private games games_;

	private void Awake()
	{
		base.transform.position = new Vector3(40f, 0f, 0f);
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
			if (!gF_)
			{
				gF_ = main_.GetComponent<gameplayFeatures>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!rdS_)
			{
				rdS_ = main_.GetComponent<roomDataScript>();
			}
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
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
		FindMyGame();
		FindMyLeitenderDesigner();
		FindMyMainMMO();
	}

	private void FindMyMainMMO()
	{
		if (!gS_ || !gS_.typ_mmoaddon)
		{
			return;
		}
		gameScript gameScript2 = gS_.FindVorgaengerScript();
		if (!gameScript2 || gameScript2.isOnMarket)
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
		string text = tS_.GetText(1259);
		text = text.Replace("<NAME>", "<b><color=blue>" + gS_.GetNameWithTag() + "</color></b>");
		guiMain_.CreateLeftNews(roomID_, guiMain_.uiSprites[3], text, rdS_.roomData_SPRITE[1]);
		Abbrechen();
	}

	private void FindMyGame()
	{
		if (!gS_)
		{
			gS_ = FindGameScriptWithID(gameID);
			if (!gS_)
			{
				Object.Destroy(base.gameObject);
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

	private void FindMyLeitenderDesigner()
	{
		if (leitenderDesignerID == -1)
		{
			return;
		}
		if (!designer_)
		{
			GameObject gameObject = GameObject.Find("CHAR_" + leitenderDesignerID);
			if ((bool)gameObject)
			{
				designer_ = gameObject.GetComponent<characterScript>();
				return;
			}
			leitenderDesignerID = -1;
			designer_ = null;
		}
		else if ((bool)designer_.roomS_)
		{
			if (designer_.roomS_.taskID != myID)
			{
				leitenderDesignerID = -1;
				designer_ = null;
			}
		}
		else
		{
			leitenderDesignerID = -1;
			designer_ = null;
		}
	}

	public float GetProzent()
	{
		FindScripts();
		if (!gS_)
		{
			return -1f;
		}
		return gS_.GetProzentGesamt();
	}

	public void Work(float f, int what)
	{
		FindScripts();
		if (!gS_)
		{
			FindMyGame();
		}
		if (!gS_)
		{
			return;
		}
		if (gS_.devPoints > 0f)
		{
			if (mS_.settings_randomEvents != 2 && !randomEvent && Random.Range(0, 1000) == 1 && !guiMain_.menuOpen && !guiMain_.uiObjects[215].activeSelf)
			{
				randomEvent = true;
				guiMain_.uiObjects[215].SetActive(value: true);
				guiMain_.uiObjects[215].GetComponent<Menu_RandomEventDev>().Init(gS_);
			}
			switch (what)
			{
			case 0:
				gS_.points_gameplay += f;
				break;
			case 1:
				gS_.points_grafik += f;
				break;
			case 2:
				gS_.points_sound += f;
				break;
			case 3:
				gS_.points_technik += f;
				break;
			case 4:
				gS_.points_bugs += f;
				if (f > 0f && Random.Range(0, 100) < 30)
				{
					gS_.points_bugsInvis += f;
				}
				break;
			case 5:
				if (gS_.GetHype() < 50f)
				{
					gS_.AddHype(f);
				}
				break;
			}
			gS_.devPoints -= 1f;
			gS_.devPoints_Gesamt -= 1f;
			if (gS_.devPoints <= 0f)
			{
				CompleteFeature();
				gS_.devPoints_Gesamt += Mathf.Abs(gS_.devPoints);
				gS_.devPoints = 0f;
				gS_.FindNextFeatureForDevelopment();
				if (gS_.devPointsStart <= 0f)
				{
					gS_.devPoints_Gesamt = 0f;
					Complete();
				}
			}
			return;
		}
		switch (what)
		{
		case 0:
			gS_.points_gameplay += f;
			RemoveInvisBug();
			break;
		case 1:
			gS_.points_grafik += f;
			RemoveInvisBug();
			break;
		case 2:
			gS_.points_sound += f;
			RemoveInvisBug();
			break;
		case 3:
			gS_.points_technik += f;
			RemoveInvisBug();
			break;
		case 5:
			if (gS_.GetHype() < 50f)
			{
				gS_.AddHype(f);
			}
			break;
		case 6:
			if (gS_.points_bugs > 0f)
			{
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
			RemoveInvisBug();
			break;
		}
		if (gS_.devFortsetzen == 1)
		{
			gS_.devFortsetzen = 0;
			CompleteOpenMenue();
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

	private void CompleteFeature()
	{
		if (gS_.devAktFeature == -5)
		{
			return;
		}
		if (gS_.devAktFeature < 0)
		{
			if (gS_.finanzierung_Technology >= 100)
			{
				gS_.points_gameplay += eF_.GetGameplay(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
				gS_.points_grafik += eF_.GetGraphic(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
				gS_.points_sound += eF_.GetSound(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
				gS_.points_technik += eF_.GetTechnik(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
				return;
			}
			float num = gS_.finanzierung_Technology;
			num *= 0.01f;
			float num2 = eF_.GetGameplay(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
			num2 *= Random.Range(num, 1f);
			gS_.points_gameplay += num2;
			num2 = eF_.GetGraphic(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
			num2 *= Random.Range(num, 1f);
			gS_.points_grafik += num2;
			num2 = eF_.GetSound(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
			num2 *= Random.Range(num, 1f);
			gS_.points_sound += num2;
			num2 = eF_.GetTechnik(gS_.gameEngineFeature[gS_.devAktFeature + 4]);
			num2 *= Random.Range(num, 1f);
			gS_.points_technik += num2;
		}
		else if (gS_.finanzierung_Kontent >= 100)
		{
			gS_.points_gameplay += gF_.GetGameplay(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			gS_.points_grafik += gF_.GetGraphic(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			gS_.points_sound += gF_.GetSound(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			gS_.points_technik += gF_.GetTechnik(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
		}
		else
		{
			float num3 = gS_.finanzierung_Kontent;
			num3 *= 0.01f;
			float num4 = gF_.GetGameplay(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			num4 *= Random.Range(num3, 1f);
			gS_.points_gameplay += num4;
			num4 = gF_.GetGraphic(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			num4 *= Random.Range(num3, 1f);
			gS_.points_grafik += num4;
			num4 = gF_.GetSound(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			num4 *= Random.Range(num3, 1f);
			gS_.points_sound += num4;
			num4 = gF_.GetTechnik(gS_.devAktFeature, gS_.maingenre, gS_.subgenre);
			num4 *= Random.Range(num3, 1f);
			gS_.points_technik += num4;
		}
	}

	public void Complete()
	{
		FindScripts();
		if (!gS_)
		{
			FindMyGame();
		}
		if ((bool)gS_)
		{
			int roomID_ = FindMyRoomWithTask();
			string tooltip_ = tS_.GetText(1128) + "\n<b>" + gS_.GetNameWithTag() + "</b>";
			guiMain_.CreateLeftNews(roomID_, games_.gameTypSprites[0], tooltip_, rdS_.roomData_SPRITE[1]);
			if (!mS_.multiplayer)
			{
				CompleteOpenMenue();
			}
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

	public void CompleteOpenMenue()
	{
		FindScripts();
		if (!gS_)
		{
			FindMyGame();
		}
		if ((bool)gS_ && gS_.devPoints_Gesamt <= 0f && !guiMain_.uiObjects[69].activeSelf)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[69]);
			guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().Init(gS_, this);
			guiMain_.OpenMenu(hideChars: false);
			sfx_.PlaySound(37, force: false);
		}
	}

	public int GetRueckgeld()
	{
		return gS_.GetRueckggeld();
	}

	public void Abbrechen()
	{
		FindScripts();
		if (!gS_)
		{
			FindMyGame();
		}
		if (!gS_)
		{
			return;
		}
		int rueckgeld = GetRueckgeld();
		if (rueckgeld >= 0)
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
		if (gS_.gameLicence != -1 && gS_.portID == -1 && !gS_.typ_addon && !gS_.typ_mmoaddon)
		{
			mS_.licences_.licence_GEKAUFT[gS_.gameLicence]++;
		}
		if (gS_.typ_contractGame)
		{
			mS_.Pay(Mathf.Abs(rueckgeld), 14);
			GameObject[] array2 = GameObject.FindGameObjectsWithTag("Room");
			for (int j = 0; j < array2.Length; j++)
			{
				roomScript component2 = array2[j].GetComponent<roomScript>();
				if ((bool)component2 && component2.taskID == myID)
				{
					guiMain_.MoneyPop(Mathf.Abs(rueckgeld), new Vector3(component2.uiPos.x, component2.uiPos.y + 3f, component2.uiPos.z), green: false);
					break;
				}
			}
			guiMain_.UpdateAuftragsansehen(-5f);
			gS_.FreeGameContract();
			Object.Destroy(base.gameObject);
		}
		else
		{
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
			Object.Destroy(base.gameObject);
		}
	}
}
