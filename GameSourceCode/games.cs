using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class games : MonoBehaviour
{
	public GameObject prefabGame;

	public Sprite[] gameTypSprites;

	public Sprite[] gamePlatformTypSprites;

	public Sprite[] gameSizeSprites;

	public Sprite[] gameAdds;

	public Sprite[] gamePEGI;

	public float[] inAppPurchasePrice;

	public float[] inAppPurchaseHate;

	public GameObject[] arrayGames;

	public gameScript[] arrayGamesScripts;

	public List<gameScript> arrayMyIpScripts = new List<gameScript>();

	public float[] preise_inhalt;

	public float tf_gewinnbeteiligungSelfPublish;

	public float tf_gewinnbeteiligungTochterfirma;

	public AnimationCurve curveSellsBewertung;

	public AnimationCurve curveReview;

	public AnimationCurve curveSells;

	public AnimationCurve curveSellsDigital;

	public AnimationCurve curveSellsDeluxe;

	public AnimationCurve curveSellsCollectors;

	public AnimationCurve curveSellsArcade;

	public AnimationCurve curveInternetUser;

	public AnimationCurve curveGamePassInteresse;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private publishingOfferMain poM_;

	private gamepassScript gpS_;

	public List<gameScript> gamePassGames = new List<gameScript>();

	public long freeServerPlatz;

	public List<ChartsWeek> chartsWeekList = new List<ChartsWeek>();

	public List<ChartsWeek> chartsWeekList_Handy = new List<ChartsWeek>();

	public List<ChartsWeek> chartsWeekList_Arcade = new List<ChartsWeek>();

	public List<ChartsWeek> chartsWeekList_F2P = new List<ChartsWeek>();

	public List<ChartsList> chartsList = new List<ChartsList>();

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!poM_)
		{
			poM_ = main_.GetComponent<publishingOfferMain>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
	}

	public float GetGrundkosten()
	{
		float num = mS_.difficulty;
		num *= 0.5f;
		return 2.5f + num;
	}

	public float GetDigitalSells()
	{
		if (unlock_.Get(59))
		{
			float num = mS_.PassedMonth();
			if (num > 600f)
			{
				num = 600f;
			}
			return curveSellsDigital.Evaluate(num / 600f);
		}
		return 0f;
	}

	public float GetSells()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return curveSells.Evaluate(num / 600f) * 30f;
	}

	public float GetSellsArcade()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return curveSellsArcade.Evaluate(num / 600f) * 30f;
	}

	public float GetInternetUser()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return curveInternetUser.Evaluate(num / 600f);
	}

	public float GetGamePassInteressted()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return curveInternetUser.Evaluate(num / 600f) * curveGamePassInteresse.Evaluate(num / 600f);
	}

	public float GetReviewCurve()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return curveReview.Evaluate(num / 600f);
	}

	public float GetArcadeCurve()
	{
		float t = mS_.PassedMonth();
		t = Mathf.PingPong(t, 600f);
		return curveSellsArcade.Evaluate(t / 600f);
	}

	public float GetDeluxeCurve()
	{
		float t = mS_.PassedMonth();
		t = Mathf.PingPong(t, 600f);
		return curveSellsDeluxe.Evaluate(t / 600f);
	}

	public float GetCollectorsCurve()
	{
		float t = mS_.PassedMonth();
		t = Mathf.PingPong(t, 600f);
		return curveSellsCollectors.Evaluate(t / 600f);
	}

	public gameScript CreateNewGame(bool fromSavegame, bool setDate)
	{
		gameScript component = Object.Instantiate(prefabGame).GetComponent<gameScript>();
		if (!fromSavegame)
		{
			component.myID = GetNewID();
			component.SetGameObjectName();
		}
		component.main_ = main_;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.eF_ = eF_;
		component.genres_ = genres_;
		component.guiMain_ = guiMain_;
		component.sfx_ = sfx_;
		component.gF_ = gF_;
		component.games_ = this;
		component.themes_ = themes_;
		component.unlock_ = unlock_;
		component.licences_ = licences_;
		component.gpS_ = gpS_;
		component.InitArrays();
		if (setDate)
		{
			component.date_start_year = mS_.year;
			component.date_start_month = mS_.month;
		}
		if (!fromSavegame)
		{
			FindGames();
		}
		return component;
	}

	public void InitMMOtoF2PGame(gameScript script_)
	{
		script_.myID = GetNewID();
		script_.SetGameObjectName();
		script_.goty = false;
		script_.exklusivKonsolenSells = 0L;
		script_.originalGameID = -1;
		script_.costs_updates = 0L;
		for (int i = 0; i < script_.specialMarketing.Length; i++)
		{
			script_.specialMarketing[i] = 0;
		}
		script_.devS_ = null;
		script_.pS_ = null;
		script_.ownerS_ = null;
		script_.engineS_ = null;
		script_.script_vorgaenger = null;
		script_.script_nachfolger = null;
		script_.script_mainIP = null;
		script_.script_portOriginal = null;
		script_.gameCopyProtectScript_ = null;
		script_.gameAntiCheatScript_ = null;
		for (int j = 0; j < script_.gamePlatformScript.Length; j++)
		{
			script_.gamePlatformScript[j] = null;
		}
		FindGames();
	}

	public void InitBudgetGame(gameScript script_)
	{
		script_.myID = GetNewID();
		script_.SetGameObjectName();
		script_.goty = false;
		script_.exklusivKonsolenSells = 0L;
		script_.originalGameID = -1;
		script_.costs_updates = 0L;
		script_.hype *= 0.5f;
		script_.hype -= mS_.year - script_.date_year;
		if (script_.hype < 0f)
		{
			script_.hype = 0f;
		}
		script_.ipPunkte = 0f;
		script_.ipTime = 0;
		script_.script_mainIP = null;
		script_.subvention = 0L;
		script_.realsticPower = 0f;
		script_.inGamePass = false;
		script_.gamePassPlayer = 0;
		for (int i = 0; i < script_.specialMarketing.Length; i++)
		{
			script_.specialMarketing[i] = 0;
		}
		for (int j = 0; j < script_.sellsPerWeekOnline.Length; j++)
		{
			script_.sellsPerWeekOnline[j] = 0f;
		}
		script_.devS_ = null;
		script_.pS_ = null;
		script_.ownerS_ = null;
		script_.engineS_ = null;
		script_.script_vorgaenger = null;
		script_.script_nachfolger = null;
		script_.script_mainIP = null;
		script_.script_portOriginal = null;
		script_.gameCopyProtectScript_ = null;
		script_.gameAntiCheatScript_ = null;
		for (int k = 0; k < script_.gamePlatformScript.Length; k++)
		{
			script_.gamePlatformScript[k] = null;
		}
		FindGames();
	}

	public void InitAddonBundle(gameScript script_)
	{
		script_.myID = GetNewID();
		script_.SetGameObjectName();
		script_.goty = false;
		script_.exklusivKonsolenSells = 0L;
		script_.originalGameID = -1;
		script_.costs_updates = 0L;
		script_.hype *= 0.5f;
		script_.hype -= mS_.year - script_.date_year;
		if (script_.hype < 0f)
		{
			script_.hype = 0f;
		}
		script_.ipPunkte = 0f;
		script_.ipTime = 0;
		script_.script_mainIP = null;
		script_.subvention = 0L;
		script_.realsticPower = 0f;
		script_.inGamePass = false;
		script_.gamePassPlayer = 0;
		for (int i = 0; i < script_.inAppPurchase.Length; i++)
		{
			script_.inAppPurchase[i] = false;
		}
		for (int j = 0; j < script_.specialMarketing.Length; j++)
		{
			script_.specialMarketing[j] = 0;
		}
		for (int k = 0; k < script_.sellsPerWeekOnline.Length; k++)
		{
			script_.sellsPerWeekOnline[k] = 0f;
		}
		script_.devS_ = null;
		script_.pS_ = null;
		script_.ownerS_ = null;
		script_.engineS_ = null;
		script_.script_vorgaenger = null;
		script_.script_nachfolger = null;
		script_.script_mainIP = null;
		script_.script_portOriginal = null;
		script_.gameCopyProtectScript_ = null;
		script_.gameAntiCheatScript_ = null;
		for (int l = 0; l < script_.gamePlatformScript.Length; l++)
		{
			script_.gamePlatformScript[l] = null;
		}
		FindGames();
	}

	public void InitGotyGame(gameScript script_)
	{
		script_.myID = GetNewID();
		script_.SetGameObjectName();
		script_.goty = false;
		script_.exklusivKonsolenSells = 0L;
		script_.originalGameID = -1;
		script_.costs_updates = 0L;
		script_.hype *= 0.5f;
		script_.hype -= mS_.year - script_.date_year;
		if (script_.hype < 0f)
		{
			script_.hype = 0f;
		}
		script_.ipPunkte = 0f;
		script_.ipTime = 0;
		script_.script_mainIP = null;
		script_.subvention = 0L;
		script_.realsticPower = 0f;
		script_.inGamePass = false;
		script_.gamePassPlayer = 0;
		for (int i = 0; i < script_.specialMarketing.Length; i++)
		{
			script_.specialMarketing[i] = 0;
		}
		for (int j = 0; j < script_.sellsPerWeekOnline.Length; j++)
		{
			script_.sellsPerWeekOnline[j] = 0f;
		}
		script_.devS_ = null;
		script_.pS_ = null;
		script_.ownerS_ = null;
		script_.engineS_ = null;
		script_.script_vorgaenger = null;
		script_.script_nachfolger = null;
		script_.script_mainIP = null;
		script_.script_portOriginal = null;
		script_.gameCopyProtectScript_ = null;
		script_.gameAntiCheatScript_ = null;
		for (int k = 0; k < script_.gamePlatformScript.Length; k++)
		{
			script_.gamePlatformScript[k] = null;
		}
		FindGames();
	}

	private int GetNewID()
	{
		return Random.Range(1, 2000000000);
	}

	public void RemovePortFlags(gameScript scriptPort_)
	{
		if (scriptPort_.portID == -1)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("GAME_" + scriptPort_.portID);
		if (!gameObject)
		{
			return;
		}
		gameScript component = gameObject.GetComponent<gameScript>();
		if ((bool)component)
		{
			if (scriptPort_.handy)
			{
				component.portExist[1] = false;
			}
			else if (scriptPort_.arcade)
			{
				component.portExist[2] = false;
			}
			else
			{
				component.portExist[0] = false;
			}
		}
	}

	public void SetPortFlags(gameScript scriptPort_)
	{
		if (scriptPort_.portID == -1)
		{
			return;
		}
		GameObject gameObject = GameObject.Find("GAME_" + scriptPort_.portID);
		if (!gameObject)
		{
			return;
		}
		gameScript component = gameObject.GetComponent<gameScript>();
		if ((bool)component)
		{
			if (scriptPort_.handy)
			{
				component.portExist[1] = true;
			}
			else if (scriptPort_.arcade)
			{
				component.portExist[2] = true;
			}
			else
			{
				component.portExist[0] = true;
			}
		}
	}

	public void FindGames()
	{
		arrayGames = GameObject.FindGameObjectsWithTag("Game");
		arrayGamesScripts = new gameScript[arrayGames.Length];
		arrayMyIpScripts.Clear();
		for (int i = 0; i < arrayGames.Length; i++)
		{
			if ((bool)arrayGames[i])
			{
				arrayGamesScripts[i] = arrayGames[i].GetComponent<gameScript>();
				if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].ownerID == mS_.myID && arrayGamesScripts[i].myID == arrayGamesScripts[i].mainIP)
				{
					arrayMyIpScripts.Add(arrayGamesScripts[i]);
				}
			}
		}
	}

	public void SellAllGames()
	{
		int num = 0;
		poM_.amountPublishingOffers = 0;
		gamePassGames.Clear();
		for (int i = 0; i < themes_.themes_MARKT.Length; i++)
		{
			themes_.themes_MARKT[i] = 0;
		}
		for (int j = 0; j < genres_.genres_MARKT.Length; j++)
		{
			genres_.genres_MARKT[j] = 0;
		}
		gpS_.gamePass_AbosLetzteWoche = 0L;
		freeServerPlatz = 0L;
		for (int k = 0; k < mS_.arrayRoomScripts.Length; k++)
		{
			if ((bool)mS_.arrayRoomScripts[k] && mS_.arrayRoomScripts[k].typ == 15)
			{
				freeServerPlatz += mS_.arrayRoomScripts[k].GetFreeServerplatz();
				mS_.arrayRoomScripts[k].serverplatzUsed = 0L;
			}
		}
		for (int l = 0; l < arrayGamesScripts.Length; l++)
		{
			if (!arrayGamesScripts[l])
			{
				continue;
			}
			if (arrayGamesScripts[l].ownerID == mS_.myID && arrayGamesScripts[l].ipToSell && Random.Range(0, 100) < 10 && !guiMain_.uiObjects[437].activeSelf)
			{
				guiMain_.OpenMenu(hideChars: false);
				guiMain_.ActivateMenu(guiMain_.uiObjects[437]);
				guiMain_.uiObjects[437].GetComponent<Menu_Result_IpVerkauf>().Init(arrayGamesScripts[l]);
			}
			if (arrayGamesScripts[l].inGamePass)
			{
				if (!arrayGamesScripts[l].CanBeInGamePass())
				{
					gpS_.GAMEPASS_RemoveGame(arrayGamesScripts[l], updateGamesAmount: false);
				}
				else
				{
					gamePassGames.Add(arrayGamesScripts[l]);
				}
			}
			if (arrayGamesScripts[l].pubAngebot)
			{
				arrayGamesScripts[l].pubAngebot_AngebotWoche = false;
				if ((mS_.multiplayer && mS_.mpCalls_.isServer) || !mS_.multiplayer)
				{
					arrayGamesScripts[l].pubAngebot_Weeks++;
					if (arrayGamesScripts[l].pubAngebot_Weeks > 25 && Random.Range(0, 100) > 90)
					{
						if (arrayGamesScripts[l].reviewTotal <= 0)
						{
							arrayGamesScripts[l].CalcReview(entwicklungsbericht: false);
							if ((bool)mS_)
							{
								mS_.reviewText_.GetReviewText(arrayGamesScripts[l]);
							}
						}
						else
						{
							arrayGamesScripts[l].date_year = mS_.year;
							arrayGamesScripts[l].date_month = mS_.month;
							if ((bool)mS_)
							{
								mS_.reviewText_.GetReviewText(arrayGamesScripts[l]);
							}
						}
						arrayGamesScripts[l].FindPublisherForGame();
						arrayGamesScripts[l].SetOnMarket();
						if (mS_.newsSetting[0])
						{
							string text = tS_.GetText(494);
							text = text.Replace("<NAME1>", arrayGamesScripts[l].GetPublisherName());
							text = text.Replace("<NAME2>", arrayGamesScripts[l].GetNameWithTag());
							guiMain_.CreateTopNewsInfo(text);
						}
					}
				}
				if (arrayGamesScripts[l].pubAngebot && !arrayGamesScripts[l].pubAnbgebot_Inivs)
				{
					poM_.amountPublishingOffers++;
				}
			}
			if (arrayGamesScripts[l].developerID == mS_.myID && arrayGamesScripts[l].inDevelopment && arrayGamesScripts[l].typ_contractGame && !arrayGamesScripts[l].auftragsspiel_zeitAbgelaufen)
			{
				arrayGamesScripts[l].auftragsspiel_zeitInWochen--;
				if (arrayGamesScripts[l].auftragsspiel_zeitInWochen < 0)
				{
					arrayGamesScripts[l].auftragsspiel_zeitInWochen = 0;
					arrayGamesScripts[l].auftragsspiel_zeitAbgelaufen = true;
				}
			}
			if (((mS_.multiplayer && mS_.mpCalls_.isServer) || !mS_.multiplayer) && arrayGamesScripts[l].auftragsspiel)
			{
				bool flag = false;
				if (!arrayGamesScripts[l].retro)
				{
					if (!arrayGamesScripts[l].gamePlatformScript[0])
					{
						arrayGamesScripts[l].FindMyPlatforms();
					}
					if (arrayGamesScripts[l].gamePlatformScript[0].vomMarktGenommen)
					{
						flag = true;
					}
				}
				arrayGamesScripts[l].auftragsspiel_wochenAlsAngebot++;
				if ((arrayGamesScripts[l].auftragsspiel_wochenAlsAngebot > 25 || flag) && (Random.Range(0, 100) > 90 || flag))
				{
					arrayGamesScripts[l].developerID = arrayGamesScripts[l].publisherID;
					arrayGamesScripts[l].FindMyDeveloper();
					arrayGamesScripts[l].FindMyPublisher();
					if ((bool)arrayGamesScripts[l].pS_)
					{
						arrayGamesScripts[l].pS_.ResetDataForAuftragsspiel(arrayGamesScripts[l]);
					}
					if (arrayGamesScripts[l].reviewTotal <= 0)
					{
						arrayGamesScripts[l].CalcReview(entwicklungsbericht: false);
						if ((bool)mS_)
						{
							mS_.reviewText_.GetReviewText(arrayGamesScripts[l]);
						}
					}
					else
					{
						arrayGamesScripts[l].date_year = mS_.year;
						arrayGamesScripts[l].date_month = mS_.month;
						if ((bool)mS_)
						{
							mS_.reviewText_.GetReviewText(arrayGamesScripts[l]);
						}
					}
					arrayGamesScripts[l].date_year = mS_.year;
					arrayGamesScripts[l].date_month = mS_.month;
					arrayGamesScripts[l].SetOnMarket();
					if (mS_.newsSetting[0])
					{
						string text2 = tS_.GetText(494);
						text2 = text2.Replace("<NAME1>", arrayGamesScripts[l].GetPublisherName());
						text2 = text2.Replace("<NAME2>", arrayGamesScripts[l].GetNameWithTag());
						guiMain_.CreateTopNewsInfo(text2);
					}
				}
			}
			if (num < 10 && ((mS_.multiplayer && mS_.mpCalls_.isServer) || !mS_.multiplayer) && arrayGamesScripts[l].mainIP == arrayGamesScripts[l].myID && arrayGamesScripts[l].OwnerIsNPC())
			{
				if (!arrayGamesScripts[l].ownerS_)
				{
					arrayGamesScripts[l].FindMyOwner();
				}
				if ((bool)arrayGamesScripts[l].ownerS_ && !arrayGamesScripts[l].ownerS_.IsTochterfirma())
				{
					if (!arrayGamesScripts[l].ipToSell)
					{
						if (!arrayGamesScripts[l].sonderIP && arrayGamesScripts[l].ipPunkte > 1f && Random.Range(0, 100) > 95 && (float)Random.Range(0, 500) > Random.Range(0f, arrayGamesScripts[l].ipPunkte) && IsIpFree(arrayGamesScripts[l], messageBox_: false))
						{
							arrayGamesScripts[l].ipToSell = true;
							num++;
							if (mS_.multiplayer && mS_.mpCalls_.isServer)
							{
								mS_.mpCalls_.SERVER_Send_GameIpSell(arrayGamesScripts[l]);
							}
						}
					}
					else
					{
						num++;
						if (Random.Range(0, 100) > 95)
						{
							arrayGamesScripts[l].ipToSell = false;
							num--;
							if (mS_.multiplayer && mS_.mpCalls_.isServer)
							{
								mS_.mpCalls_.SERVER_Send_GameIpSell(arrayGamesScripts[l]);
							}
						}
					}
				}
			}
			if (arrayGamesScripts[l].ownerID == mS_.myID && !arrayGamesScripts[l].typ_contractGame && !arrayGamesScripts[l].auftragsspiel && !arrayGamesScripts[l].pubAngebot && arrayGamesScripts[l].mainIP == arrayGamesScripts[l].myID)
			{
				arrayGamesScripts[l].ipTime++;
				if (arrayGamesScripts[l].ipTime > 250)
				{
					switch (mS_.difficulty)
					{
					case 0:
						arrayGamesScripts[l].AddIpPoints(-20f);
						break;
					case 1:
						arrayGamesScripts[l].AddIpPoints(-40f);
						break;
					case 2:
						arrayGamesScripts[l].AddIpPoints(-50f);
						break;
					case 3:
						arrayGamesScripts[l].AddIpPoints(-80f);
						break;
					case 4:
						arrayGamesScripts[l].AddIpPoints(-90f);
						break;
					case 5:
						arrayGamesScripts[l].AddIpPoints(-100f);
						break;
					default:
						arrayGamesScripts[l].AddIpPoints(-80f);
						break;
					}
				}
				if (mS_.week == 5)
				{
					arrayGamesScripts[l].merchGewinnLetzterMonat = arrayGamesScripts[l].merchGewinnDiesenMonat;
					arrayGamesScripts[l].merchGewinnDiesenMonat = 0L;
					for (int m = 0; m < arrayGamesScripts[l].merchLetzterMonat.Length; m++)
					{
						arrayGamesScripts[l].merchLetzterMonat[m] = arrayGamesScripts[l].merchDiesenMonat[m];
						arrayGamesScripts[l].merchDiesenMonat[m] = 0;
					}
				}
				if (arrayGamesScripts[l].merchGesamtReviewPoints > 0f)
				{
					arrayGamesScripts[l].merchGesamtReviewPoints -= 10f;
					if (arrayGamesScripts[l].merchGesamtReviewPoints < 0f)
					{
						arrayGamesScripts[l].merchGesamtReviewPoints = 0f;
					}
				}
			}
			if (arrayGamesScripts[l].ownerID == mS_.myID || arrayGamesScripts[l].publisherID == mS_.myID || arrayGamesScripts[l].developerID == mS_.myID)
			{
				if (arrayGamesScripts[l].inDevelopment)
				{
					arrayGamesScripts[l].weeksInDevelopment++;
					if (arrayGamesScripts[l].devFortsetzen > 1)
					{
						arrayGamesScripts[l].devFortsetzen--;
					}
				}
				if (((arrayGamesScripts[l].GetProzentGesamt() > 99f && arrayGamesScripts[l].GetHype() > 0f) || arrayGamesScripts[l].schublade) && Random.Range(0, 100) < 10)
				{
					arrayGamesScripts[l].AddHype(-1f);
				}
			}
			if ((arrayGamesScripts[l].ownerID == mS_.myID || arrayGamesScripts[l].publisherID == mS_.myID) && (arrayGamesScripts[l].isOnMarket || arrayGamesScripts[l].inDevelopment || arrayGamesScripts[l].schublade))
			{
				if (arrayGamesScripts[l].specialMarketing[0] == 1)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 0));
				}
				if (arrayGamesScripts[l].specialMarketing[1] == 1)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 1));
				}
				if (arrayGamesScripts[l].specialMarketing[2] == 1)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 2));
				}
				if (arrayGamesScripts[l].specialMarketing[3] == 1)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 3));
				}
				if (arrayGamesScripts[l].specialMarketing[4] == 1)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 4));
				}
				if (arrayGamesScripts[l].specialMarketing[2] == -1 && arrayGamesScripts[l].hype > 100f && arrayGamesScripts[l].isOnMarket && arrayGamesScripts[l].reviewTotal > 0 && arrayGamesScripts[l].reviewTotal < 90 && arrayGamesScripts[l].weeksOnMarket > 0 && !guiMain_.uiObjects[296].activeSelf)
				{
					StartCoroutine(iWaitForSpecialMarketing(arrayGamesScripts[l], 100));
				}
			}
			if (arrayGamesScripts[l].isOnMarket && !arrayGamesScripts[l].inDevelopment && !arrayGamesScripts[l].typ_addon && !arrayGamesScripts[l].typ_mmoaddon && !arrayGamesScripts[l].typ_bundle)
			{
				themes_.themes_MARKT[arrayGamesScripts[l].gameMainTheme]++;
				if (arrayGamesScripts[l].gameSubTheme != -1)
				{
					themes_.themes_MARKT[arrayGamesScripts[l].gameSubTheme]++;
				}
				genres_.genres_MARKT[arrayGamesScripts[l].maingenre]++;
			}
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer && (arrayGamesScripts[l].IsMyGame() || arrayGamesScripts[l].typ_contractGame || (arrayGamesScripts[l].DeveloperIsNPC() && arrayGamesScripts[l].PublisherIsNPC() && arrayGamesScripts[l].OwnerIsNPC())))
				{
					arrayGamesScripts[l].SellGame();
					arrayGamesScripts[l].AutomaticSellLagerbestand();
				}
				if (mS_.mpCalls_.isClient && arrayGamesScripts[l].IsMyGame())
				{
					arrayGamesScripts[l].SellGame();
					arrayGamesScripts[l].AutomaticSellLagerbestand();
				}
			}
			else
			{
				arrayGamesScripts[l].SellGame();
				arrayGamesScripts[l].AutomaticSellLagerbestand();
			}
			if (arrayGamesScripts[l].ownerID == mS_.myID)
			{
				arrayGamesScripts[l].SellMerchandise();
			}
		}
		UpdateAllGamePassFunctions();
		LagerplatzVerteilen();
	}

	private void UpdateAllGamePassFunctions()
	{
		float gamePassInteressted = GetGamePassInteressted();
		float gamePass_AboPreis_ = 0f;
		switch (gpS_.gamePass_AboPreis)
		{
		case 2:
			gamePass_AboPreis_ = 0.95f;
			break;
		case 3:
			gamePass_AboPreis_ = 0.9f;
			break;
		case 4:
			gamePass_AboPreis_ = 0.85f;
			break;
		case 5:
			gamePass_AboPreis_ = 0.75f;
			break;
		case 6:
			gamePass_AboPreis_ = 0.65f;
			break;
		case 7:
			gamePass_AboPreis_ = 0.5f;
			break;
		case 8:
			gamePass_AboPreis_ = 0.4f;
			break;
		case 9:
			gamePass_AboPreis_ = 0.3f;
			break;
		case 10:
			gamePass_AboPreis_ = 0.2f;
			break;
		}
		if (gpS_.gamePass_aktiv)
		{
			for (int i = 0; i < gamePassGames.Count; i++)
			{
				gpS_.UpdateGamesFormMarket(gamePassGames[i], gamePassInteressted, gamePass_AboPreis_);
			}
		}
		gpS_.VerteileAbosAufServer();
		gpS_.gamePass_AmountGames = 0;
		gpS_.gamePass_AmountGamesAktiv = 0;
		gpS_.gamePass_AmountGamesOnMarket = 0;
		for (int j = 0; j < gamePassGames.Count; j++)
		{
			gpS_.GetAmountGamePassGamesFast(gamePassGames[j]);
		}
	}

	private IEnumerator iWaitForSpecialMarketing(gameScript gS_, int kampagne)
	{
		bool done = false;
		while (!done)
		{
			if ((bool)gS_ && !guiMain_.uiObjects[296].activeSelf)
			{
				done = true;
				guiMain_.OpenMenu(hideChars: false);
				guiMain_.ActivateMenu(guiMain_.uiObjects[296]);
				guiMain_.uiObjects[296].GetComponent<Menu_Result_MarketingSpezial>().Init(gS_, kampagne);
			}
			yield return null;
		}
	}

	public int GetAmountOfMMOs()
	{
		int num = 0;
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i])
			{
				gameScript gameScript2 = arrayGamesScripts[i];
				if ((bool)gameScript2 && gameScript2.gameTyp == 1 && gameScript2.isOnMarket && gameScript2.releaseDate <= 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	public int GetAmountOfF2Ps()
	{
		int num = 0;
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i])
			{
				gameScript gameScript2 = arrayGamesScripts[i];
				if ((bool)gameScript2 && gameScript2.gameTyp == 2 && gameScript2.isOnMarket && gameScript2.releaseDate <= 0)
				{
					num++;
				}
			}
		}
		return num;
	}

	public Vector4 GetAmountGamesWithGenreAndTopic(gameScript gS_)
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if (!arrayGamesScripts[i])
			{
				continue;
			}
			gameScript gameScript2 = arrayGamesScripts[i];
			if (!gameScript2 || gS_.myID == gameScript2.myID || !gameScript2.isOnMarket || gameScript2.typ_addon || gameScript2.typ_bundle || gameScript2.typ_mmoaddon)
			{
				continue;
			}
			if (gameScript2.maingenre == gS_.maingenre)
			{
				num++;
				if (SamePlatform(gameScript2.gamePlatform[0], gS_))
				{
					num3++;
				}
				if (SamePlatform(gameScript2.gamePlatform[1], gS_))
				{
					num3++;
				}
				if (SamePlatform(gameScript2.gamePlatform[2], gS_))
				{
					num3++;
				}
				if (SamePlatform(gameScript2.gamePlatform[3], gS_))
				{
					num3++;
				}
			}
			if (gameScript2.gameMainTheme == gS_.gameMainTheme)
			{
				num2++;
				if (SamePlatform(gameScript2.gamePlatform[0], gS_))
				{
					num4++;
				}
				if (SamePlatform(gameScript2.gamePlatform[1], gS_))
				{
					num4++;
				}
				if (SamePlatform(gameScript2.gamePlatform[2], gS_))
				{
					num4++;
				}
				if (SamePlatform(gameScript2.gamePlatform[3], gS_))
				{
					num4++;
				}
			}
		}
		return new Vector4(num, num2, num3, num4);
	}

	public int GetAmountGamesWithGenre_OnMarket(int genreID)
	{
		int num = 0;
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i])
			{
				gameScript gameScript2 = arrayGamesScripts[i];
				if ((bool)gameScript2 && gameScript2.isOnMarket && !gameScript2.typ_addon && !gameScript2.typ_bundle && !gameScript2.typ_mmoaddon && gameScript2.maingenre == genreID)
				{
					num++;
				}
			}
		}
		return num;
	}

	private bool SamePlatform(int platformID, gameScript gS_)
	{
		if (!gS_)
		{
			return false;
		}
		if (platformID != -1)
		{
			for (int i = 0; i < gS_.gamePlatform.Length; i++)
			{
				if (platformID == gS_.gamePlatform[i])
				{
					return true;
				}
			}
			return false;
		}
		return false;
	}

	public long GetFreeLagerplatz()
	{
		long num = 0L;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 9)
			{
				num += mS_.arrayRoomScripts[i].GetFreeLagerplatz();
			}
		}
		return num;
	}

	public void LagerplatzVerteilenEinGame(long menge)
	{
		if (menge > 0)
		{
			SearchLager(menge);
		}
	}

	public void LagerplatzVerteilen()
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 9)
			{
				mS_.arrayRoomScripts[i].lagerplatzUsed = 0L;
			}
		}
		for (int j = 0; j < arrayGamesScripts.Length; j++)
		{
			if ((bool)arrayGamesScripts[j] && arrayGamesScripts[j].publisherID == mS_.myID)
			{
				long lagerbestand = arrayGamesScripts[j].GetLagerbestand();
				if (lagerbestand > 0)
				{
					SearchLager(lagerbestand);
				}
			}
		}
	}

	private void SearchLager(long bestand)
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if (!mS_.arrayRoomScripts[i] || !mS_.arrayRoomScripts[i] || mS_.arrayRoomScripts[i].typ != 9)
			{
				continue;
			}
			long freeLagerplatz = mS_.arrayRoomScripts[i].GetFreeLagerplatz();
			if (freeLagerplatz > 0)
			{
				if (freeLagerplatz >= bestand)
				{
					mS_.arrayRoomScripts[i].lagerplatzUsed += bestand;
					break;
				}
				mS_.arrayRoomScripts[i].lagerplatzUsed += freeLagerplatz;
				bestand -= freeLagerplatz;
			}
		}
	}

	public void SaveLastChartPosition()
	{
		if (!mS_ || !mS_.mpCalls_)
		{
			mS_.FindScripts();
		}
		if (mS_.multiplayer && !mS_.mpCalls_.isServer)
		{
			return;
		}
		for (int i = 0; i < chartsWeekList.Count; i++)
		{
			gameScript script_ = chartsWeekList[i].script_;
			if ((bool)script_)
			{
				script_.lastChartPosition = i;
			}
		}
		for (int j = 0; j < chartsWeekList_Handy.Count; j++)
		{
			gameScript script_2 = chartsWeekList_Handy[j].script_;
			if ((bool)script_2)
			{
				script_2.lastChartPosition = j;
			}
		}
		for (int k = 0; k < chartsWeekList_Arcade.Count; k++)
		{
			gameScript script_3 = chartsWeekList_Arcade[k].script_;
			if ((bool)script_3)
			{
				script_3.lastChartPosition = k;
			}
		}
		for (int l = 0; l < chartsWeekList_F2P.Count; l++)
		{
			gameScript script_4 = chartsWeekList_F2P[l].script_;
			if ((bool)script_4)
			{
				script_4.lastChartPosition = l;
			}
		}
	}

	public void UpdateChartsWeek()
	{
		if (!mS_ || !mS_.mpCalls_)
		{
			mS_.FindScripts();
		}
		chartsWeekList.Clear();
		chartsWeekList_Handy.Clear();
		chartsWeekList_Arcade.Clear();
		chartsWeekList_F2P.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].isOnMarket && arrayGamesScripts[i].sellsPerWeek[0] > 0 && !arrayGamesScripts[i].inDevelopment)
			{
				if (arrayGamesScripts[i].gameTyp != 2 && !arrayGamesScripts[i].handy && !arrayGamesScripts[i].arcade)
				{
					chartsWeekList.Add(new ChartsWeek(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsPerWeek[0], arrayGamesScripts[i]));
				}
				if (arrayGamesScripts[i].gameTyp != 2 && arrayGamesScripts[i].handy)
				{
					chartsWeekList_Handy.Add(new ChartsWeek(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsPerWeek[0], arrayGamesScripts[i]));
				}
				if (arrayGamesScripts[i].gameTyp != 2 && arrayGamesScripts[i].arcade)
				{
					chartsWeekList_Arcade.Add(new ChartsWeek(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsPerWeek[0], arrayGamesScripts[i]));
				}
				if (arrayGamesScripts[i].gameTyp == 2)
				{
					chartsWeekList_F2P.Add(new ChartsWeek(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsPerWeek[0], arrayGamesScripts[i]));
				}
			}
		}
		chartsWeekList = chartsWeekList.OrderByDescending((ChartsWeek chartsWeek) => chartsWeek.sells).ToList();
		chartsWeekList_Handy = chartsWeekList_Handy.OrderByDescending((ChartsWeek chartsWeek) => chartsWeek.sells).ToList();
		chartsWeekList_Arcade = chartsWeekList_Arcade.OrderByDescending((ChartsWeek chartsWeek) => chartsWeek.sells).ToList();
		chartsWeekList_F2P = chartsWeekList_F2P.OrderByDescending((ChartsWeek chartsWeek) => chartsWeek.sells).ToList();
		if (mS_.multiplayer && !mS_.mpCalls_.isServer)
		{
			return;
		}
		for (int num = 0; num < chartsWeekList.Count; num++)
		{
			gameScript script_ = chartsWeekList[num].script_;
			if ((bool)script_ && (script_.bestChartPosition > num + 1 || script_.bestChartPosition <= 0))
			{
				script_.bestChartPosition = num + 1;
			}
		}
		for (int num2 = 0; num2 < chartsWeekList_Handy.Count; num2++)
		{
			gameScript script_2 = chartsWeekList_Handy[num2].script_;
			if ((bool)script_2 && (script_2.bestChartPosition > num2 + 1 || script_2.bestChartPosition <= 0))
			{
				script_2.bestChartPosition = num2 + 1;
			}
		}
		for (int num3 = 0; num3 < chartsWeekList_Arcade.Count; num3++)
		{
			gameScript script_3 = chartsWeekList_Arcade[num3].script_;
			if ((bool)script_3 && (script_3.bestChartPosition > num3 + 1 || script_3.bestChartPosition <= 0))
			{
				script_3.bestChartPosition = num3 + 1;
			}
		}
		for (int num4 = 0; num4 < chartsWeekList_F2P.Count; num4++)
		{
			gameScript script_4 = chartsWeekList_F2P[num4].script_;
			if ((bool)script_4 && (script_4.bestChartPosition > num4 + 1 || script_4.bestChartPosition <= 0))
			{
				script_4.bestChartPosition = num4 + 1;
			}
		}
	}

	public int GetChartsWeekPosition(int gameID_, int lastPos)
	{
		if (lastPos > 0)
		{
			if (chartsWeekList.Count >= lastPos && chartsWeekList[lastPos - 1].gameID == gameID_)
			{
				return lastPos;
			}
			if (chartsWeekList_Handy.Count >= lastPos && chartsWeekList_Handy[lastPos - 1].gameID == gameID_)
			{
				return lastPos;
			}
			if (chartsWeekList_Arcade.Count >= lastPos && chartsWeekList_Arcade[lastPos - 1].gameID == gameID_)
			{
				return lastPos;
			}
			if (chartsWeekList_F2P.Count >= lastPos && chartsWeekList_F2P[lastPos - 1].gameID == gameID_)
			{
				return lastPos;
			}
		}
		for (int i = 0; i < chartsWeekList.Count; i++)
		{
			if (chartsWeekList[i].gameID == gameID_)
			{
				return i + 1;
			}
		}
		for (int j = 0; j < chartsWeekList_Handy.Count; j++)
		{
			if (chartsWeekList_Handy[j].gameID == gameID_)
			{
				return j + 1;
			}
		}
		for (int k = 0; k < chartsWeekList_Arcade.Count; k++)
		{
			if (chartsWeekList_Arcade[k].gameID == gameID_)
			{
				return k + 1;
			}
		}
		for (int l = 0; l < chartsWeekList_F2P.Count; l++)
		{
			if (chartsWeekList_F2P[l].gameID == gameID_)
			{
				return l + 1;
			}
		}
		return -1;
	}

	public void CreateAllTimeCharts(int max)
	{
		chartsList.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].sellsTotal > 0 && (arrayGamesScripts[i].typ_nachfolger || arrayGamesScripts[i].typ_remaster || arrayGamesScripts[i].typ_standard || arrayGamesScripts[i].typ_spinoff) && !arrayGamesScripts[i].inDevelopment && !arrayGamesScripts[i].typ_addon && !arrayGamesScripts[i].typ_mmoaddon && !arrayGamesScripts[i].typ_bundle && !arrayGamesScripts[i].typ_budget && !arrayGamesScripts[i].typ_addonStandalone && arrayGamesScripts[i].gameTyp != 2 && !arrayGamesScripts[i].handy && !arrayGamesScripts[i].arcade)
			{
				chartsList.Add(new ChartsList(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsTotal, arrayGamesScripts[i]));
			}
		}
		chartsList = chartsList.OrderByDescending((ChartsList chartsList) => chartsList.wert).ToList();
		while (chartsList.Count > max)
		{
			chartsList.RemoveAt(chartsList.Count - 1);
		}
	}

	public void CreateBestGamesCharts(int max, int genre, int gameSize)
	{
		chartsList.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].reviewTotal > 0 && (gameSize > 5 || gameSize == arrayGamesScripts[i].gameSize) && (arrayGamesScripts[i].maingenre == genre || genre < 0) && (arrayGamesScripts[i].typ_nachfolger || arrayGamesScripts[i].typ_remaster || arrayGamesScripts[i].typ_standard || arrayGamesScripts[i].typ_spinoff) && !arrayGamesScripts[i].inDevelopment && !arrayGamesScripts[i].pubAngebot && !arrayGamesScripts[i].auftragsspiel && !arrayGamesScripts[i].typ_addon && !arrayGamesScripts[i].typ_mmoaddon && !arrayGamesScripts[i].typ_bundle && !arrayGamesScripts[i].typ_budget && !arrayGamesScripts[i].typ_addonStandalone)
			{
				chartsList.Add(new ChartsList(arrayGamesScripts[i].myID, arrayGamesScripts[i].reviewTotal, arrayGamesScripts[i]));
			}
		}
		chartsList = chartsList.OrderByDescending((ChartsList chartsList) => chartsList.wert).ToList();
		while (chartsList.Count > max)
		{
			chartsList.RemoveAt(chartsList.Count - 1);
		}
	}

	public void CreateAllTimeChartsUmsatz(int max, int genre, int gameSize)
	{
		chartsList.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].umsatzTotal > 0 && (gameSize > 5 || gameSize == arrayGamesScripts[i].gameSize) && (arrayGamesScripts[i].maingenre == genre || genre < 0) && (arrayGamesScripts[i].typ_nachfolger || arrayGamesScripts[i].typ_remaster || arrayGamesScripts[i].typ_standard || arrayGamesScripts[i].typ_spinoff) && !arrayGamesScripts[i].inDevelopment && !arrayGamesScripts[i].pubAngebot && !arrayGamesScripts[i].auftragsspiel && !arrayGamesScripts[i].typ_addon && !arrayGamesScripts[i].typ_mmoaddon && !arrayGamesScripts[i].typ_bundle && !arrayGamesScripts[i].typ_budget && !arrayGamesScripts[i].typ_addonStandalone)
			{
				chartsList.Add(new ChartsList(arrayGamesScripts[i].myID, arrayGamesScripts[i].umsatzTotal, arrayGamesScripts[i]));
			}
		}
		chartsList = chartsList.OrderByDescending((ChartsList chartsList) => chartsList.wert).ToList();
		while (chartsList.Count > max)
		{
			chartsList.RemoveAt(chartsList.Count - 1);
		}
	}

	public void CreateAllTimeChartsHandy(int max)
	{
		chartsList.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].sellsTotal > 0 && (arrayGamesScripts[i].typ_nachfolger || arrayGamesScripts[i].typ_remaster || arrayGamesScripts[i].typ_standard || arrayGamesScripts[i].typ_spinoff) && !arrayGamesScripts[i].inDevelopment && !arrayGamesScripts[i].pubAngebot && !arrayGamesScripts[i].auftragsspiel && !arrayGamesScripts[i].typ_addon && !arrayGamesScripts[i].typ_mmoaddon && !arrayGamesScripts[i].typ_bundle && !arrayGamesScripts[i].typ_budget && !arrayGamesScripts[i].typ_addonStandalone && arrayGamesScripts[i].gameTyp != 2 && arrayGamesScripts[i].handy)
			{
				chartsList.Add(new ChartsList(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsTotal, arrayGamesScripts[i]));
			}
		}
		chartsList = chartsList.OrderByDescending((ChartsList chartsList) => chartsList.wert).ToList();
		while (chartsList.Count > max)
		{
			chartsList.RemoveAt(chartsList.Count - 1);
		}
	}

	public void CreateAllTimeChartsArcade(int max)
	{
		chartsList.Clear();
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].sellsTotal > 0 && (arrayGamesScripts[i].typ_nachfolger || arrayGamesScripts[i].typ_remaster || arrayGamesScripts[i].typ_standard || arrayGamesScripts[i].typ_spinoff) && !arrayGamesScripts[i].inDevelopment && !arrayGamesScripts[i].pubAngebot && !arrayGamesScripts[i].auftragsspiel && !arrayGamesScripts[i].typ_addon && !arrayGamesScripts[i].typ_mmoaddon && !arrayGamesScripts[i].typ_bundle && !arrayGamesScripts[i].typ_budget && !arrayGamesScripts[i].typ_addonStandalone && arrayGamesScripts[i].gameTyp != 2 && arrayGamesScripts[i].arcade)
			{
				chartsList.Add(new ChartsList(arrayGamesScripts[i].myID, arrayGamesScripts[i].sellsTotal, arrayGamesScripts[i]));
			}
		}
		chartsList = chartsList.OrderByDescending((ChartsList chartsList) => chartsList.wert).ToList();
		while (chartsList.Count > max)
		{
			chartsList.RemoveAt(chartsList.Count - 1);
		}
	}

	public bool IsNewGenreCombination(int maingenre, int subgenre)
	{
		if (maingenre <= -1)
		{
			return false;
		}
		if (subgenre <= -1)
		{
			return false;
		}
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].ownerID == mS_.myID && !arrayGamesScripts[i].pubOffer && arrayGamesScripts[i].maingenre == maingenre && arrayGamesScripts[i].subgenre == subgenre)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsNewTopicCombination(int maintopic, int subtopic)
	{
		if (maintopic <= -1)
		{
			return false;
		}
		if (subtopic <= -1)
		{
			return false;
		}
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if ((bool)arrayGamesScripts[i] && arrayGamesScripts[i].ownerID == mS_.myID && !arrayGamesScripts[i].pubOffer && arrayGamesScripts[i].gameMainTheme == maintopic && arrayGamesScripts[i].gameSubTheme == subtopic)
			{
				return false;
			}
		}
		return true;
	}

	public bool IsIpFree(gameScript game_, bool messageBox_)
	{
		for (int i = 0; i < arrayGamesScripts.Length; i++)
		{
			if (!arrayGamesScripts[i] || arrayGamesScripts[i].mainIP != game_.mainIP)
			{
				continue;
			}
			if (arrayGamesScripts[i].inDevelopment || arrayGamesScripts[i].schublade)
			{
				if (messageBox_)
				{
					guiMain_.MessageBox(tS_.GetText(2009), closeMenu: false);
				}
				return false;
			}
			if (arrayGamesScripts[i].pubAngebot || arrayGamesScripts[i].auftragsspiel)
			{
				if (messageBox_)
				{
					guiMain_.MessageBox(tS_.GetText(2010), closeMenu: false);
				}
				return false;
			}
			if (arrayGamesScripts[i].isOnMarket)
			{
				if (messageBox_)
				{
					guiMain_.MessageBox(tS_.GetText(2246), closeMenu: false);
				}
				return false;
			}
		}
		return true;
	}
}
