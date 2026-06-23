using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class publisherScript : MonoBehaviour
{
	public class PlatformList
	{
		public platformScript script_;

		public float marktanteil;

		public PlatformList(platformScript s_, float markt)
		{
			script_ = s_;
			marktanteil = markt;
		}
	}

	public mainScript mS_;

	public GameObject main_;

	public textScript tS_;

	public settingsScript settings_;

	public GUI_Main guiMain_;

	public genres genres_;

	public games games_;

	public gameplayFeatures gF_;

	public engineFeatures eF_;

	public unlockScript unlock_;

	public reviewText reviewText_;

	public platforms platforms_;

	public forschungSonstiges fS_;

	public Menu_DevGame menuDevGame_;

	public licences licences_;

	public themes themes_;

	public gamepassScript gpS_;

	public int myID;

	public bool isUnlocked;

	public bool isPlayer;

	public int ownerID;

	public string name_EN;

	public string name_GE;

	public string name_TU;

	public string name_CH;

	public string name_FR;

	public string name_JA;

	public string name_UA;

	public string name_TH;

	public int date_year;

	public int date_month;

	public int date_year_end;

	public int date_month_end;

	public float stars;

	public int logoID;

	public bool developer;

	public bool publisher;

	public float relation;

	public float share;

	public int fanGenre;

	public int newGameInWeeks;

	public int newGameInWeeksORG;

	public int exklusivLaufzeit;

	public bool onlyMobile;

	public bool ownPlatform;

	public bool exklusive;

	public int developmentSpeed;

	public long firmenwert;

	public bool notForSale;

	public int lockToBuy;

	public int country;

	public int[] awards;

	public bool tf_geschlossen;

	public bool tf_autoRelease;

	public bool tf_onlyPlayerConsole;

	public bool tf_allowMMO;

	public bool tf_allowF2P;

	public bool tf_allowAddon;

	public bool tf_noArcade;

	public bool tf_noHandy;

	public bool tf_noRetro;

	public bool tf_noPorts;

	public bool tf_noBudget;

	public bool tf_noGOTY;

	public bool tf_noBundles;

	public bool tf_noAddonBundles;

	public bool tf_noRemaster;

	public bool tf_noSpinoffs;

	public bool tf_autoGamePass;

	public bool tf_publisher;

	public bool tf_developer;

	public int tf_entwicklungsdauer;

	public int tf_gameSize;

	public int tf_gameGenre;

	public long[] tf_umsatz;

	public long tf_umsatz_allTime;

	public bool tf_ownPublisher;

	public int tf_gameTopic;

	public int[] tf_ipFocus;

	public int tf_engine;

	public int tf_autoReleaseVal;

	public int[] tf_platformFocus;

	public int tf_ownPublisherPriority;

	public long awards_bestStudioPoints;

	public long awards_bestPublisherPoints;

	private bool SETTING_SUBVENTION;

	private bool SETTING_SUBVENTION_EXKLUSIV;

	public int amountTrys;

	private bool nextGameAddon;

	private bool nextGameMMOAddon;

	public List<PlatformList> platformList = new List<PlatformList>();

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!reviewText_)
		{
			reviewText_ = main_.GetComponent<reviewText>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
	}

	public void Init()
	{
		FindScripts();
		base.name = "PUB_" + myID;
	}

	private bool PlayerHasNpcReplaced()
	{
		return false;
	}

	public void Unlock()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (PlayerHasNpcReplaced())
		{
			return;
		}
		isUnlocked = true;
		lockToBuy = Random.Range(12, 36);
		if (mS_.multiplayer)
		{
			lockToBuy = 24;
		}
		if (genres_.genres_UNLOCK[fanGenre])
		{
			return;
		}
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (genres_.genres_UNLOCK[i])
			{
				fanGenre = i;
				if (Random.Range(0, 10) == 1)
				{
					break;
				}
			}
		}
	}

	public void CloseNpcStudio()
	{
		if (!mS_)
		{
			FindScripts();
		}
		tf_geschlossen = true;
		if (mS_.exklusivVertrag_ID == myID)
		{
			mS_.RemovePublisherExklusivVertrag();
		}
		lockToBuy = 0;
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			mS_.mpCalls_.SERVER_Send_PublisherClose(this);
		}
	}

	public string GetName()
	{
		string text = "";
		text = settings_.language switch
		{
			0 => name_EN, 
			1 => name_GE, 
			2 => name_TU, 
			3 => name_CH, 
			4 => name_FR, 
			16 => name_JA, 
			17 => name_UA, 
			19 => name_TH, 
			_ => name_EN, 
		};
		if (text == null)
		{
			return name_EN;
		}
		if (text.Length <= 0)
		{
			return name_EN;
		}
		return text;
	}

	public void SetOwnName(string c)
	{
		if (c != null && c.Length > 0)
		{
			name_EN = c;
			name_GE = c;
			name_TU = c;
			name_CH = c;
			name_FR = c;
			name_JA = c;
			name_UA = c;
			name_TH = c;
		}
	}

	public Sprite GetLogo()
	{
		if (logoID < 0)
		{
			return null;
		}
		return guiMain_.GetCompanyLogo(logoID);
	}

	public float GetRelation()
	{
		if (mS_.settings_sandbox && mS_.sandbox_publisherMaxReleation)
		{
			return 100f;
		}
		if (myID == mS_.myID)
		{
			return 100f;
		}
		if (IsMyTochterfirma())
		{
			return 100f;
		}
		return relation;
	}

	public float GetMinimalReviewPoints()
	{
		float num = stars * 0.9f - GetRelation() / 5f;
		if (num < 20f)
		{
			num = 0f;
		}
		return num;
	}

	public bool IsTochterfirma()
	{
		if (isPlayer)
		{
			return false;
		}
		if (myID >= 100000)
		{
			return false;
		}
		if (ownerID == -1)
		{
			return false;
		}
		return true;
	}

	public bool IsMyTochterfirma()
	{
		if (isPlayer)
		{
			return false;
		}
		if (myID >= 100000)
		{
			return false;
		}
		if (ownerID == mS_.myID)
		{
			return true;
		}
		return false;
	}

	public bool IsTochterfirmaVonMitspieler()
	{
		if (isPlayer)
		{
			return false;
		}
		if (myID >= 100000)
		{
			return false;
		}
		if (!IsTochterfirma())
		{
			return false;
		}
		if (IsMyTochterfirma())
		{
			return false;
		}
		return true;
	}

	public void SetAsTochterfirma(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		ownerID = id_;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_PublisherOwner(this);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_PublisherOwner(this);
			}
		}
	}

	public void RemoveTochterfirma()
	{
		if (!mS_)
		{
			FindScripts();
		}
		ownerID = -1;
		if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
		{
			date_year_end = mS_.year + Random.Range(4, 10);
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_PublisherOwner(this);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_PublisherOwner(this);
			}
		}
	}

	public void ResetTochterfirmaSettings()
	{
		exklusive = false;
		onlyMobile = false;
		relation = 0f;
		share = Random.Range(8, 13);
		tf_geschlossen = false;
		tf_autoRelease = false;
		tf_onlyPlayerConsole = false;
		tf_allowMMO = true;
		tf_allowF2P = true;
		tf_allowAddon = true;
		tf_noArcade = false;
		tf_noHandy = false;
		tf_noRetro = false;
		tf_noPorts = false;
		tf_noBudget = false;
		tf_noGOTY = false;
		tf_noBundles = false;
		tf_noAddonBundles = false;
		tf_noRemaster = false;
		tf_noSpinoffs = false;
		tf_autoGamePass = false;
		tf_gameGenre = 0;
		tf_gameSize = 0;
		tf_entwicklungsdauer = 1;
		publisher = tf_publisher;
		developer = tf_developer;
		tf_ownPublisher = false;
		tf_gameTopic = -1;
		tf_autoReleaseVal = 0;
		tf_ownPublisherPriority = -1;
		for (int i = 0; i < tf_ipFocus.Length; i++)
		{
			tf_ipFocus[i] = -1;
		}
		tf_engine = -1;
		for (int j = 0; j < tf_platformFocus.Length; j++)
		{
			tf_platformFocus[j] = -1;
		}
	}

	public void VerwaltungskostenBezahlen()
	{
		if ((bool)mS_ && IsMyTochterfirma() && !tf_geschlossen)
		{
			mS_.Pay(GetVerwaltungskosten(), 30);
		}
	}

	public long GetVerwaltungskosten()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (tf_geschlossen)
		{
			return 0L;
		}
		long num = GetFirmenwert();
		switch (mS_.difficulty)
		{
		case 0:
			num = num / 1000000 * 2000;
			break;
		case 1:
			num = num / 1000000 * 3000;
			break;
		case 2:
			num = num / 1000000 * 4000;
			break;
		case 3:
			num = num / 1000000 * 5000;
			break;
		case 4:
			num = num / 1000000 * 6000;
			break;
		case 5:
			num = num / 1000000 * 7000;
			break;
		}
		return num + 30000;
	}

	public long GetFirmenwert()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (firmenwert > 30000000)
		{
			firmenwert = 30000000L;
		}
		long num = mS_.year - 1975;
		if (num > 50)
		{
			num = 50L;
		}
		long num2 = num * firmenwert;
		if (ownPlatform)
		{
			num2 *= 3;
		}
		num2 = mS_.difficulty switch
		{
			0 => num2 / 5, 
			1 => num2 / 4, 
			2 => num2 / 3, 
			3 => num2 / 2, 
			4 => num2, 
			5 => num2 * 2, 
			_ => num2, 
		};
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].sellsTotal > 0 && !games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].schublade && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].IsMyIP(this) && games_.arrayGamesScripts[i].mainIP == games_.arrayGamesScripts[i].myID)
			{
				num2 += games_.arrayGamesScripts[i].GetIpWert();
			}
		}
		return num2;
	}

	public string GetFirmenwertString()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (!tS_)
		{
			FindScripts();
		}
		if (!notForSale)
		{
			return mS_.GetMoney(GetFirmenwert(), showDollar: true);
		}
		return tS_.GetText(1912);
	}

	private void SetNewGameInWeeks_ForAddons()
	{
		newGameInWeeks /= 4;
		newGameInWeeksORG = newGameInWeeks;
		if (newGameInWeeks < 4)
		{
			newGameInWeeks = 4;
		}
		if (newGameInWeeksORG < 4)
		{
			newGameInWeeksORG = 4;
		}
	}

	public void SetNewGameInWeeks(int force)
	{
		switch (force)
		{
		case 0:
		{
			float num = (mS_.year - 1976) * 2 + 10;
			if (num > 48f)
			{
				num = 48f;
			}
			if (!mS_)
			{
				FindScripts();
			}
			if (mS_.multiplayer)
			{
				num *= 3f;
			}
			num -= (float)(developmentSpeed * 2);
			if (IsTochterfirma())
			{
				switch (tf_entwicklungsdauer)
				{
				case 1:
					num *= 1.5f;
					break;
				case 2:
					num *= 2f;
					break;
				}
			}
			if (num < 10f)
			{
				num = 10f;
			}
			newGameInWeeks = Random.Range(Mathf.RoundToInt(num), Mathf.RoundToInt(num * 2f));
			break;
		}
		default:
			newGameInWeeks = force;
			if (newGameInWeeks < 5)
			{
				newGameInWeeks = 5;
			}
			break;
		case 9999:
			newGameInWeeks = 1;
			break;
		}
		newGameInWeeksORG = newGameInWeeks;
	}

	public gameScript CreateNewGame2(bool forceContractGame, bool newTryForTochterfirma)
	{
		SETTING_SUBVENTION = false;
		SETTING_SUBVENTION_EXKLUSIV = false;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isClient && !IsMyTochterfirma())
			{
				return null;
			}
			if (mS_.mpCalls_.isServer && IsTochterfirmaVonMitspieler())
			{
				return null;
			}
		}
		if (!isUnlocked)
		{
			return null;
		}
		if (IsTochterfirma() && tf_geschlossen)
		{
			return null;
		}
		if (Geschlossen())
		{
			return null;
		}
		if (!developer)
		{
			return null;
		}
		if (!forceContractGame)
		{
			newGameInWeeks--;
			if (newGameInWeeks > 0)
			{
				if (!IsTochterfirma() && Random.Range(0, 10) == 1)
				{
					gameScript gameScript2 = FindGameInDevelopment();
					if ((bool)gameScript2 && !gameScript2.angekuendigt && !gameScript2.typ_addon && !gameScript2.typ_addonStandalone && !gameScript2.typ_budget && !gameScript2.typ_bundle && !gameScript2.typ_bundleAddon && !gameScript2.typ_goty && !gameScript2.typ_mmoaddon)
					{
						gameScript2.angekuendigt = true;
						NewsAnkuendigung(gameScript2);
					}
				}
				return null;
			}
			gameScript gameScript3 = FindGameInDevelopment();
			if ((bool)gameScript3 && gameScript3.HasUnreleasedPlattform())
			{
				return null;
			}
			if (!newTryForTochterfirma)
			{
				ReleaseGameInDevelopment();
			}
			SetNewGameInWeeks(0);
		}
		else
		{
			if (!publisher)
			{
				return null;
			}
			if (onlyMobile)
			{
				return null;
			}
		}
		if (onlyMobile && !unlock_.Get(65))
		{
			return null;
		}
		bool flag = false;
		gameScript gameScript4 = games_.CreateNewGame(fromSavegame: false, setDate: true);
		gameScript4.inDevelopment = false;
		gameScript4.developerID = myID;
		gameScript4.ownerID = myID;
		gameScript4.hype = Random.Range(stars * 0.5f, stars);
		gameScript4.costs_marketing = Mathf.RoundToInt(gameScript4.hype * 7500f);
		gameScript4.mainIP = gameScript4.myID;
		gameScript4.finanzierung_Grundkosten = 100;
		gameScript4.finanzierung_Technology = 100;
		gameScript4.finanzierung_Kontent = 100;
		gameScript4.gameAP_Gameplay = 5;
		gameScript4.gameAP_Grafik = 5;
		gameScript4.gameAP_Sound = 5;
		gameScript4.gameAP_Technik = 5;
		int num = Random.Range(0, 13);
		if (Random.Range(0, 100) > 50)
		{
			num = 6;
		}
		if (Random.Range(0, 100) > 50)
		{
			num = 0;
		}
		if (!IsTochterfirma() && publisher)
		{
			if (Random.Range(0, 100) > 80)
			{
				num = 4;
			}
			if (Random.Range(0, 100) > 90)
			{
				num = 5;
			}
			if (Random.Range(0, 100) > 90)
			{
				num = 12;
			}
		}
		if (nextGameAddon)
		{
			num = 2;
			nextGameAddon = false;
		}
		if (nextGameMMOAddon)
		{
			num = 3;
			nextGameMMOAddon = false;
		}
		if (IsTochterfirma())
		{
			if (!tf_allowAddon && num == 2)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (!tf_allowAddon && num == 3)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_onlyPlayerConsole && num == 2)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_onlyPlayerConsole && num == 3)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noPorts && num == 9)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noPorts && num == 10)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noPorts && num == 11)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_onlyPlayerConsole && num == 9)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_onlyPlayerConsole && num == 10)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_onlyPlayerConsole && num == 11)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noBudget && num == 4)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noGOTY && num == 8)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noRemaster && num == 1)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noSpinoffs && num == 7)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noBundles && num == 5)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (tf_noAddonBundles && num == 12)
			{
				num = 6;
				if (Random.Range(0, 100) > 50)
				{
					num = 0;
				}
			}
			if (num == 6)
			{
				for (int i = 0; i < tf_ipFocus.Length; i++)
				{
					if (tf_ipFocus[i] != -1)
					{
						num = 0;
						if (!tf_noSpinoffs && Random.Range(0, 100) > 50)
						{
							num = 7;
						}
						break;
					}
				}
			}
		}
		if (forceContractGame)
		{
			num = 6;
		}
		switch (num)
		{
		case 0:
		{
			gameScript gameForNachfolger = GetGameForNachfolger();
			if (!gameForNachfolger)
			{
				break;
			}
			flag = true;
			gameScript4.teile = gameForNachfolger.teile + 1;
			gameForNachfolger.nachfolger_created = true;
			gameScript4.mainIP = gameForNachfolger.mainIP;
			gameScript4.maingenre = gameForNachfolger.maingenre;
			gameScript4.subgenre = gameForNachfolger.subgenre;
			gameScript4.gameZielgruppe = gameForNachfolger.gameZielgruppe;
			gameScript4.originalGameID = gameForNachfolger.myID;
			gameScript4.script_vorgaenger = gameForNachfolger;
			gameScript4.npcLateinNumbers = gameForNachfolger.npcLateinNumbers;
			gameScript4.sonderIP = gameForNachfolger.sonderIP;
			gameScript4.sonderIPMindestreview = gameForNachfolger.sonderIPMindestreview;
			gameScript4.platformStatic = gameForNachfolger.platformStatic;
			gameForNachfolger.FindMainIpScript();
			string text = "";
			if (gameScript4.teile <= 2)
			{
				text = (gameScript4.myNameTeil1 = gameForNachfolger.GetNameSimple());
			}
			else
			{
				text = gameForNachfolger.myNameTeil1;
				gameScript4.myNameTeil1 = gameForNachfolger.myNameTeil1;
			}
			if (text.Length <= 0)
			{
				text = gameForNachfolger.script_mainIP.GetNameSimple();
			}
			if ((bool)gameForNachfolger.script_mainIP)
			{
				if (!genres_.genres_SUC_YEAR[gameScript4.maingenre])
				{
					if (!gameForNachfolger.npcLateinNumbers)
					{
						gameScript4.SetMyName(text + " " + gameScript4.teile);
					}
					else
					{
						switch (gameScript4.teile)
						{
						case 1:
							gameScript4.SetMyName(text + " I");
							break;
						case 2:
							gameScript4.SetMyName(text + " II");
							break;
						case 3:
							gameScript4.SetMyName(text + " III");
							break;
						case 4:
							gameScript4.SetMyName(text + " IV");
							break;
						case 5:
							gameScript4.SetMyName(text + " V");
							break;
						case 6:
							gameScript4.SetMyName(text + " VI");
							break;
						case 7:
							gameScript4.SetMyName(text + " VII");
							break;
						case 8:
							gameScript4.SetMyName(text + " VIII");
							break;
						case 9:
							gameScript4.SetMyName(text + " IX");
							break;
						case 10:
							gameScript4.SetMyName(text + " X");
							break;
						case 11:
							gameScript4.SetMyName(text + " XI");
							break;
						case 12:
							gameScript4.SetMyName(text + " XII");
							break;
						case 13:
							gameScript4.SetMyName(text + " XIII");
							break;
						case 14:
							gameScript4.SetMyName(text + " XIV");
							break;
						case 15:
							gameScript4.SetMyName(text + " XV");
							break;
						case 16:
							gameScript4.SetMyName(text + " XVI");
							break;
						case 17:
							gameScript4.SetMyName(text + " XVII");
							break;
						case 18:
							gameScript4.SetMyName(text + " XVIII");
							break;
						case 19:
							gameScript4.SetMyName(text + " XIX");
							break;
						case 20:
							gameScript4.SetMyName(text + " XX");
							break;
						default:
							gameScript4.SetMyName(text + " " + gameScript4.teile);
							break;
						}
					}
				}
				else if (mS_.year < 2000)
				{
					gameScript4.SetMyName(text + " " + (mS_.year - 1900));
				}
				else
				{
					gameScript4.SetMyName(text + " " + mS_.year);
				}
				gameScript4.typ_nachfolger = true;
				if (gameScript4.platformStatic)
				{
					gameScript4.gamePlatform[0] = gameForNachfolger.gamePlatform[0];
					gameScript4.gamePlatformScript[0] = gameForNachfolger.gamePlatformScript[0];
				}
				int platTyp = SetPlatformTyp(gameScript4, forceContractGame);
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetGameSize(gameScript4);
				SetMMOorF2P(gameScript4, platTyp);
				if (!gameScript4.sonderIP)
				{
					SetLicence(gameScript4);
				}
				if (!gameScript4.sonderIP)
				{
					SetTheme(gameScript4);
					gameScript4.gameMainTheme = gameForNachfolger.gameMainTheme;
				}
				else
				{
					gameScript4.gameMainTheme = gameForNachfolger.gameMainTheme;
					gameScript4.gameSubTheme = gameForNachfolger.gameSubTheme;
				}
				SetDesignSlider(gameScript4);
				SetLanguages(gameScript4);
				SetStudioAufwertungen(gameScript4);
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMGSR_Result(gameScript4, gameScript4.Designausrichtung[1]);
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				FindEngineForGameNew(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetPoints(gameScript4);
				if (gameScript4.points_gameplay < gameForNachfolger.points_gameplay)
				{
					gameScript4.points_gameplay = gameForNachfolger.points_gameplay;
				}
				if (gameScript4.points_grafik < gameForNachfolger.points_grafik)
				{
					gameScript4.points_grafik = gameForNachfolger.points_grafik;
				}
				if (gameScript4.points_sound < gameForNachfolger.points_sound)
				{
					gameScript4.points_sound = gameForNachfolger.points_sound;
				}
				if (gameScript4.points_technik < gameForNachfolger.points_technik)
				{
					gameScript4.points_gameplay = gameForNachfolger.points_technik;
				}
				if (gameScript4.gameTyp == 0 && !nextGameAddon && Random.Range(0, 100) < 30)
				{
					nextGameAddon = true;
					SetNewGameInWeeks(Random.Range(10, 20) - developmentSpeed);
				}
				if (gameScript4.gameTyp == 1 && !nextGameMMOAddon && Random.Range(0, 100) < 80)
				{
					nextGameMMOAddon = true;
					SetNewGameInWeeks(Random.Range(10, 20) - developmentSpeed);
				}
			}
			else
			{
				flag = false;
			}
			break;
		}
		case 1:
		{
			if (mS_.year < 1987)
			{
				break;
			}
			gameScript remaster = GetRemaster();
			if ((bool)remaster)
			{
				flag = true;
				remaster.remaster_created = true;
				gameScript4.typ_standard = false;
				gameScript4.typ_remaster = true;
				gameScript4.SetMyName(remaster.GetNameSimple() + " " + tS_.GetText(620));
				gameScript4.mainIP = remaster.mainIP;
				gameScript4.maingenre = remaster.maingenre;
				gameScript4.subgenre = remaster.subgenre;
				gameScript4.gameMainTheme = remaster.gameMainTheme;
				gameScript4.gameSubTheme = remaster.gameSubTheme;
				gameScript4.gameZielgruppe = remaster.gameZielgruppe;
				gameScript4.gameSize = remaster.gameSize;
				gameScript4.npcLateinNumbers = remaster.npcLateinNumbers;
				gameScript4.gameLicence = remaster.gameLicence;
				gameScript4.sonderIP = remaster.sonderIP;
				gameScript4.sonderIPMindestreview = remaster.sonderIPMindestreview;
				gameScript4.exklusiv = remaster.exklusiv;
				gameScript4.herstellerExklusiv = remaster.herstellerExklusiv;
				gameScript4.handy = remaster.handy;
				gameScript4.arcade = remaster.arcade;
				gameScript4.retro = remaster.retro;
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetLanguages(gameScript4);
				SetStudioAufwertungen(gameScript4);
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMGSR_Result(gameScript4, gameScript4.Designausrichtung[1]);
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				FindEngineForGameNew(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetPoints(gameScript4);
				if (gameScript4.points_gameplay < remaster.points_gameplay)
				{
					gameScript4.points_gameplay = remaster.points_gameplay;
				}
				if (gameScript4.points_grafik < remaster.points_grafik)
				{
					gameScript4.points_grafik = remaster.points_grafik;
				}
				if (gameScript4.points_sound < remaster.points_sound)
				{
					gameScript4.points_sound = remaster.points_sound;
				}
				if (gameScript4.points_technik < remaster.points_technik)
				{
					gameScript4.points_gameplay = remaster.points_technik;
				}
			}
			break;
		}
		case 2:
		{
			gameScript addon = GetAddon();
			if (!addon)
			{
				break;
			}
			addon.FindMyEngineNew();
			if ((bool)addon.engineS_ && addon.engineS_.OwnerIsNPC())
			{
				flag = true;
				gameScript4.SetMyName(addon.GetNameSimple() + " - " + tS_.GetRandomNPCAddonName());
				gameScript4.mainIP = addon.mainIP;
				gameScript4.typ_standard = false;
				gameScript4.typ_addon = true;
				gameScript4.maingenre = addon.maingenre;
				gameScript4.subgenre = addon.subgenre;
				gameScript4.gameMainTheme = addon.gameMainTheme;
				gameScript4.gameSubTheme = addon.gameSubTheme;
				gameScript4.gameZielgruppe = addon.gameZielgruppe;
				gameScript4.gameSize = addon.gameSize;
				gameScript4.addonQuality = 0.4f;
				gameScript4.points_gameplay = addon.points_gameplay * 1.1f;
				gameScript4.points_grafik = addon.points_grafik * 1.1f;
				gameScript4.points_sound = addon.points_sound * 1.1f;
				gameScript4.points_technik = addon.points_technik * 1.1f;
				gameScript4.publisherID = addon.publisherID;
				gameScript4.pS_ = addon.pS_;
				gameScript4.originalGameID = addon.myID;
				gameScript4.usk = addon.usk;
				gameScript4.npcLateinNumbers = addon.npcLateinNumbers;
				gameScript4.sonderIP = addon.sonderIP;
				gameScript4.sonderIPMindestreview = addon.sonderIPMindestreview;
				gameScript4.exklusiv = addon.exklusiv;
				gameScript4.herstellerExklusiv = addon.herstellerExklusiv;
				if (Random.Range(0, 100) < 30)
				{
					gameScript4.typ_addon = false;
					gameScript4.typ_addonStandalone = true;
				}
				gameScript4.engineID = addon.engineID;
				gameScript4.engineS_ = addon.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = addon.engineS_.GetBestFeature(eF_.GetTypGrafik());
				gameScript4.gameEngineFeature[1] = addon.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = addon.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = addon.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int num29 = 0; num29 < addon.gamePlatform.Length; num29++)
				{
					gameScript4.gamePlatform[num29] = addon.gamePlatform[num29];
					gameScript4.gamePlatformScript[num29] = addon.gamePlatformScript[num29];
				}
				for (int num30 = 0; num30 < gameScript4.gameLanguage.Length; num30++)
				{
					gameScript4.gameLanguage[num30] = addon.gameLanguage[num30];
				}
				for (int num31 = 0; num31 < gameScript4.gameplayStudio.Length; num31++)
				{
					gameScript4.gameplayStudio[num31] = addon.gameplayStudio[num31];
				}
				for (int num32 = 0; num32 < gameScript4.grafikStudio.Length; num32++)
				{
					gameScript4.grafikStudio[num32] = addon.grafikStudio[num32];
				}
				for (int num33 = 0; num33 < gameScript4.soundStudio.Length; num33++)
				{
					gameScript4.soundStudio[num33] = addon.soundStudio[num33];
				}
				for (int num34 = 0; num34 < gameScript4.motionCaptureStudio.Length; num34++)
				{
					gameScript4.motionCaptureStudio[num34] = addon.motionCaptureStudio[num34];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetNewGameInWeeks_ForAddons();
			}
			break;
		}
		case 3:
		{
			gameScript mMOAddon = GetMMOAddon();
			if (!mMOAddon)
			{
				break;
			}
			mMOAddon.FindMyEngineNew();
			if ((bool)mMOAddon.engineS_ && mMOAddon.engineS_.OwnerIsNPC())
			{
				flag = true;
				gameScript4.SetMyName(mMOAddon.GetNameSimple() + " - " + tS_.GetRandomNPCAddonName());
				gameScript4.mainIP = mMOAddon.mainIP;
				gameScript4.typ_standard = false;
				gameScript4.typ_mmoaddon = true;
				gameScript4.maingenre = mMOAddon.maingenre;
				gameScript4.subgenre = mMOAddon.subgenre;
				gameScript4.gameMainTheme = mMOAddon.gameMainTheme;
				gameScript4.gameSubTheme = mMOAddon.gameSubTheme;
				gameScript4.gameZielgruppe = mMOAddon.gameZielgruppe;
				gameScript4.gameSize = mMOAddon.gameSize;
				gameScript4.addonQuality = 0.4f;
				gameScript4.points_gameplay = mMOAddon.points_gameplay * 1.1f;
				gameScript4.points_grafik = mMOAddon.points_grafik * 1.1f;
				gameScript4.points_sound = mMOAddon.points_sound * 1.1f;
				gameScript4.points_technik = mMOAddon.points_technik * 1.1f;
				gameScript4.publisherID = mMOAddon.publisherID;
				gameScript4.pS_ = mMOAddon.pS_;
				gameScript4.originalGameID = mMOAddon.myID;
				gameScript4.usk = mMOAddon.usk;
				gameScript4.npcLateinNumbers = mMOAddon.npcLateinNumbers;
				gameScript4.sonderIP = mMOAddon.sonderIP;
				gameScript4.sonderIPMindestreview = mMOAddon.sonderIPMindestreview;
				gameScript4.exklusiv = mMOAddon.exklusiv;
				gameScript4.herstellerExklusiv = mMOAddon.herstellerExklusiv;
				gameScript4.engineID = mMOAddon.engineID;
				gameScript4.engineS_ = mMOAddon.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = mMOAddon.engineS_.GetBestFeature(eF_.GetTypGrafik());
				gameScript4.gameEngineFeature[1] = mMOAddon.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = mMOAddon.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = mMOAddon.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int num23 = 0; num23 < mMOAddon.gamePlatform.Length; num23++)
				{
					gameScript4.gamePlatform[num23] = mMOAddon.gamePlatform[num23];
					gameScript4.gamePlatformScript[num23] = mMOAddon.gamePlatformScript[num23];
				}
				for (int num24 = 0; num24 < gameScript4.gameLanguage.Length; num24++)
				{
					gameScript4.gameLanguage[num24] = mMOAddon.gameLanguage[num24];
				}
				for (int num25 = 0; num25 < gameScript4.gameplayStudio.Length; num25++)
				{
					gameScript4.gameplayStudio[num25] = mMOAddon.gameplayStudio[num25];
				}
				for (int num26 = 0; num26 < gameScript4.grafikStudio.Length; num26++)
				{
					gameScript4.grafikStudio[num26] = mMOAddon.grafikStudio[num26];
				}
				for (int num27 = 0; num27 < gameScript4.soundStudio.Length; num27++)
				{
					gameScript4.soundStudio[num27] = mMOAddon.soundStudio[num27];
				}
				for (int num28 = 0; num28 < gameScript4.motionCaptureStudio.Length; num28++)
				{
					gameScript4.motionCaptureStudio[num28] = mMOAddon.motionCaptureStudio[num28];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetNewGameInWeeks_ForAddons();
			}
			break;
		}
		case 4:
		{
			gameScript gameForBudget = GetGameForBudget();
			if (!gameForBudget || !publisher)
			{
				break;
			}
			gameForBudget.FindMyEngineNew();
			if ((bool)gameForBudget.engineS_ && gameForBudget.engineS_.OwnerIsNPC())
			{
				flag = true;
				gameForBudget.budget_created = true;
				gameScript4.SetMyName(gameForBudget.GetNameSimple() + " <color=grey><i>" + tS_.GetText(1154 + Random.Range(0, 4)) + "</i></color>");
				gameScript4.mainIP = gameForBudget.mainIP;
				gameScript4.typ_standard = false;
				gameScript4.typ_budget = true;
				gameScript4.maingenre = gameForBudget.maingenre;
				gameScript4.subgenre = gameForBudget.subgenre;
				gameScript4.gameMainTheme = gameForBudget.gameMainTheme;
				gameScript4.gameSubTheme = gameForBudget.gameSubTheme;
				gameScript4.gameZielgruppe = gameForBudget.gameZielgruppe;
				gameScript4.gameSize = gameForBudget.gameSize;
				gameScript4.points_gameplay = gameForBudget.points_gameplay;
				gameScript4.points_grafik = gameForBudget.points_grafik;
				gameScript4.points_sound = gameForBudget.points_sound;
				gameScript4.points_technik = gameForBudget.points_technik;
				gameScript4.publisherID = gameForBudget.publisherID;
				gameScript4.pS_ = gameForBudget.pS_;
				gameScript4.originalGameID = gameForBudget.myID;
				gameScript4.usk = gameForBudget.usk;
				gameScript4.npcLateinNumbers = gameForBudget.npcLateinNumbers;
				gameScript4.gameLicence = gameForBudget.gameLicence;
				gameScript4.sonderIP = gameForBudget.sonderIP;
				gameScript4.sonderIPMindestreview = gameForBudget.sonderIPMindestreview;
				gameScript4.exklusiv = gameForBudget.exklusiv;
				gameScript4.herstellerExklusiv = gameForBudget.herstellerExklusiv;
				gameScript4.engineID = gameForBudget.engineID;
				gameScript4.engineS_ = gameForBudget.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = gameForBudget.gameEngineFeature[0];
				gameScript4.gameEngineFeature[1] = gameForBudget.gameEngineFeature[1];
				gameScript4.gameEngineFeature[2] = gameForBudget.gameEngineFeature[2];
				gameScript4.gameEngineFeature[3] = gameForBudget.gameEngineFeature[3];
				for (int num40 = 0; num40 < gameForBudget.gamePlatform.Length; num40++)
				{
					gameScript4.gamePlatform[num40] = gameForBudget.gamePlatform[num40];
					gameScript4.gamePlatformScript[num40] = gameForBudget.gamePlatformScript[num40];
				}
				for (int num41 = 0; num41 < gameScript4.gameLanguage.Length; num41++)
				{
					gameScript4.gameLanguage[num41] = gameForBudget.gameLanguage[num41];
				}
				for (int num42 = 0; num42 < gameScript4.gameplayStudio.Length; num42++)
				{
					gameScript4.gameplayStudio[num42] = gameForBudget.gameplayStudio[num42];
				}
				for (int num43 = 0; num43 < gameScript4.grafikStudio.Length; num43++)
				{
					gameScript4.grafikStudio[num43] = gameForBudget.grafikStudio[num43];
				}
				for (int num44 = 0; num44 < gameScript4.soundStudio.Length; num44++)
				{
					gameScript4.soundStudio[num44] = gameForBudget.soundStudio[num44];
				}
				for (int num45 = 0; num45 < gameScript4.motionCaptureStudio.Length; num45++)
				{
					gameScript4.motionCaptureStudio[num45] = gameForBudget.motionCaptureStudio[num45];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = gameForBudget.gameLicence;
				gameScript4.gameAntiCheat = gameForBudget.gameAntiCheat;
				gameScript4.gameCopyProtect = gameForBudget.gameCopyProtect;
				gameScript4.Designschwerpunkt = (int[])gameForBudget.Designschwerpunkt.Clone();
				gameScript4.Designausrichtung = (int[])gameForBudget.Designausrichtung.Clone();
				gameScript4.gameGameplayFeatures = (bool[])gameForBudget.gameGameplayFeatures.Clone();
				gameScript4.gameplayFeatures_DevDone = (bool[])gameForBudget.gameplayFeatures_DevDone.Clone();
				gameScript4.userNegativ = gameForBudget.userNegativ;
				gameScript4.userPositiv = gameForBudget.userPositiv;
				gameScript4.reviewGameplay = gameForBudget.reviewGameplay;
				gameScript4.reviewGrafik = gameForBudget.reviewGrafik;
				gameScript4.reviewSound = gameForBudget.reviewSound;
				gameScript4.reviewSteuerung = gameForBudget.reviewSteuerung;
				gameScript4.reviewTotal = gameForBudget.reviewTotal;
				gameScript4.reviewGameplayText = gameForBudget.reviewGameplayText;
				gameScript4.reviewGrafikText = gameForBudget.reviewGrafikText;
				gameScript4.reviewSoundText = gameForBudget.reviewSoundText;
				gameScript4.reviewSteuerungText = gameForBudget.reviewSteuerungText;
				gameScript4.reviewTotalText = gameForBudget.reviewTotalText;
			}
			break;
		}
		case 5:
		{
			gameScript[] array2 = new gameScript[5]
			{
				GetGameForBundle(),
				null,
				null,
				null,
				null
			};
			if ((bool)array2[0])
			{
				array2[0].bundle_created = true;
			}
			if ((bool)array2[0])
			{
				array2[1] = GetGameForBundle();
				if ((bool)array2[1])
				{
					array2[1].bundle_created = true;
				}
				array2[2] = GetGameForBundle();
				if ((bool)array2[2])
				{
					array2[2].bundle_created = true;
				}
				array2[3] = GetGameForBundle();
				if ((bool)array2[3])
				{
					array2[3].bundle_created = true;
				}
				array2[4] = GetGameForBundle();
				if ((bool)array2[4])
				{
					array2[4].bundle_created = true;
				}
			}
			if ((bool)array2[0] && (bool)array2[1])
			{
				if (!publisher)
				{
					break;
				}
				gameScript4.mainIP = -1;
				gameScript4.engineID = 0;
				gameScript4.FindMyEngineNew();
				if (!gameScript4.engineS_)
				{
					break;
				}
				flag = true;
				string randomNPCBundleName = tS_.GetRandomNPCBundleName();
				randomNPCBundleName = randomNPCBundleName.Replace("<NAME>", GetName());
				if (mS_.year < 2000)
				{
					gameScript4.SetMyName(randomNPCBundleName + " '" + (mS_.year - 1900));
				}
				else
				{
					gameScript4.SetMyName(randomNPCBundleName + " " + mS_.year);
				}
				gameScript4.typ_standard = false;
				gameScript4.typ_bundle = true;
				gameScript4.warBeiAwards = true;
				gameScript4.usk = 0;
				gameScript4.maingenre = array2[0].maingenre;
				gameScript4.gameMainTheme = array2[0].gameMainTheme;
				float num4 = 0f;
				for (int num5 = 0; num5 < array2.Length; num5++)
				{
					if ((bool)array2[num5])
					{
						num4 += (float)array2[num5].reviewTotal;
					}
				}
				if (num4 > 0f)
				{
					num4 /= 3f;
				}
				if (num4 > 90f)
				{
					num4 = 90f;
				}
				gameScript4.reviewGameplay = Mathf.RoundToInt(num4);
				gameScript4.reviewGrafik = Mathf.RoundToInt(num4);
				gameScript4.reviewSound = Mathf.RoundToInt(num4);
				gameScript4.reviewSteuerung = Mathf.RoundToInt(num4);
				gameScript4.reviewTotal = Mathf.RoundToInt(num4);
				gameScript4.usk = 0;
				for (int num6 = 0; num6 < array2.Length; num6++)
				{
					if (!array2[num6])
					{
						continue;
					}
					gameScript4.bundleID[num6] = array2[num6].myID;
					if (array2[num6].usk > gameScript4.usk)
					{
						gameScript4.usk = array2[num6].usk;
					}
					for (int num7 = 0; num7 < array2[num6].gameLanguage.Length; num7++)
					{
						if (array2[num6].gameLanguage[num7])
						{
							gameScript4.gameLanguage[num7] = true;
						}
					}
					gameScript4.points_gameplay += array2[num6].points_gameplay;
					gameScript4.points_grafik += array2[num6].points_grafik;
					gameScript4.points_sound += array2[num6].points_sound;
					gameScript4.points_technik += array2[num6].points_technik;
				}
				int num8 = 0;
				for (int num9 = 0; num9 < array2.Length; num9++)
				{
					if (!array2[num9])
					{
						continue;
					}
					for (int num10 = 0; num10 < array2[num9].gamePlatform.Length; num10++)
					{
						if (array2[num9].gamePlatform[num10] != -1 && array2[num9].gamePlatform[num10] != gameScript4.gamePlatform[0] && array2[num9].gamePlatform[num10] != gameScript4.gamePlatform[1] && array2[num9].gamePlatform[num10] != gameScript4.gamePlatform[2] && array2[num9].gamePlatform[num10] != gameScript4.gamePlatform[3] && num8 < 4)
						{
							gameScript4.gamePlatform[num8] = array2[num9].gamePlatform[num10];
							num8++;
						}
					}
				}
				for (int num11 = 0; num11 < gameScript4.standard_edition.Length; num11++)
				{
					gameScript4.standard_edition[num11] = false;
					gameScript4.deluxe_edition[num11] = false;
					gameScript4.collectors_edition[num11] = false;
				}
			}
			else
			{
				if ((bool)array2[0])
				{
					array2[0].bundle_created = false;
				}
				if ((bool)array2[1])
				{
					array2[1].bundle_created = false;
				}
				if ((bool)array2[2])
				{
					array2[2].bundle_created = false;
				}
				if ((bool)array2[3])
				{
					array2[3].bundle_created = false;
				}
				if ((bool)array2[4])
				{
					array2[4].bundle_created = false;
				}
			}
			break;
		}
		case 6:
			if (Random.Range(0, 100) > 60)
			{
				gameScript4.npcLateinNumbers = true;
			}
			gameScript4.gameTyp = 0;
			if (!forceContractGame)
			{
				tS_.GetRandomNpcIP(myID, gameScript4);
			}
			if (gameScript4.GetNameSimple().Length <= 0)
			{
				SetGenre(gameScript4);
				tS_.GetRandomNpcGame(gameScript4.maingenre, gameScript4);
			}
			else
			{
				gameScript4.sonderIP = true;
			}
			if (gameScript4.GetNameSimple().Length > 0)
			{
				flag = true;
				gameScript4.teile = 1;
				gameScript4.typ_standard = true;
				int platTyp2 = SetPlatformTyp(gameScript4, forceContractGame);
				if (!gameScript4.sonderIP && gameScript4.gameZielgruppe == -1)
				{
					gameScript4.gameZielgruppe = Random.Range(0, 5);
				}
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetGameSize(gameScript4);
				SetMMOorF2P(gameScript4, platTyp2);
				if (!gameScript4.sonderIP)
				{
					SetLicence(gameScript4);
				}
				if (!gameScript4.sonderIP && gameScript4.gameMainTheme == -1)
				{
					SetTheme(gameScript4);
				}
				SetDesignSlider(gameScript4);
				SetLanguages(gameScript4);
				SetStudioAufwertungen(gameScript4);
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMGSR_Result(gameScript4, gameScript4.Designausrichtung[1]);
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				FindEngineForGameNew(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetPoints(gameScript4);
				if (gameScript4.gameTyp == 0 && !gameScript4.handy && !gameScript4.arcade && !nextGameAddon && Random.Range(0, 100) < 30)
				{
					nextGameAddon = true;
					SetNewGameInWeeks(Random.Range(10, 20) - developmentSpeed);
				}
				if (gameScript4.gameTyp == 1 && !gameScript4.handy && !gameScript4.arcade && !nextGameMMOAddon && Random.Range(0, 100) < 80)
				{
					nextGameMMOAddon = true;
					SetNewGameInWeeks(Random.Range(10, 20) - developmentSpeed);
				}
			}
			break;
		case 7:
		{
			gameScript spinoff = GetSpinoff();
			if (!spinoff)
			{
				break;
			}
			SetGenre(gameScript4);
			string randomNpcSpinoffName = tS_.GetRandomNpcSpinoffName(gameScript4.maingenre);
			randomNpcSpinoffName = randomNpcSpinoffName.Replace("<NAME>", spinoff.GetNameSimple());
			gameScript4.SetMyName(randomNpcSpinoffName);
			if (gameScript4.GetNameSimple().Length > 0)
			{
				flag = true;
				gameScript4.teile = 1;
				gameScript4.mainIP = spinoff.mainIP;
				if (Random.Range(0, 100) > 60)
				{
					gameScript4.npcLateinNumbers = true;
				}
				gameScript4.typ_standard = false;
				gameScript4.typ_spinoff = true;
				gameScript4.originalGameID = spinoff.myID;
				int platTyp3 = SetPlatformTyp(gameScript4, forceContractGame);
				gameScript4.gameTyp = 0;
				gameScript4.gameZielgruppe = Random.Range(0, 5);
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetGameSize(gameScript4);
				SetMMOorF2P(gameScript4, platTyp3);
				SetTheme(gameScript4);
				SetDesignSlider(gameScript4);
				SetLanguages(gameScript4);
				SetStudioAufwertungen(gameScript4);
				guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMGSR_Result(gameScript4, gameScript4.Designausrichtung[1]);
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				FindEngineForGameNew(gameScript4);
				SetGameplayFeatures(gameScript4);
				SetPoints(gameScript4);
			}
			break;
		}
		case 8:
		{
			gameScript gOTY = GetGOTY();
			if (!gOTY || !publisher)
			{
				break;
			}
			gOTY.FindMyEngineNew();
			if ((bool)gOTY.engineS_ && gOTY.engineS_.OwnerIsNPC())
			{
				flag = true;
				gOTY.goty_created = true;
				gameScript4.SetMyName(gOTY.GetNameSimple() + " " + tS_.GetText(1361));
				gameScript4.mainIP = gOTY.mainIP;
				gameScript4.typ_standard = false;
				gameScript4.typ_goty = true;
				gameScript4.maingenre = gOTY.maingenre;
				gameScript4.subgenre = gOTY.subgenre;
				gameScript4.gameMainTheme = gOTY.gameMainTheme;
				gameScript4.gameSubTheme = gOTY.gameSubTheme;
				gameScript4.gameZielgruppe = gOTY.gameZielgruppe;
				gameScript4.gameSize = gOTY.gameSize;
				gameScript4.points_gameplay = gOTY.points_gameplay;
				gameScript4.points_grafik = gOTY.points_grafik;
				gameScript4.points_sound = gOTY.points_sound;
				gameScript4.points_technik = gOTY.points_technik;
				gameScript4.publisherID = gOTY.publisherID;
				gameScript4.pS_ = gOTY.pS_;
				gameScript4.originalGameID = gOTY.myID;
				gameScript4.usk = gOTY.usk;
				gameScript4.npcLateinNumbers = gOTY.npcLateinNumbers;
				gameScript4.gameLicence = gOTY.gameLicence;
				gameScript4.sonderIP = gOTY.sonderIP;
				gameScript4.sonderIPMindestreview = gOTY.sonderIPMindestreview;
				gameScript4.exklusiv = gOTY.exklusiv;
				gameScript4.herstellerExklusiv = gOTY.herstellerExklusiv;
				gameScript4.engineID = gOTY.engineID;
				gameScript4.engineS_ = gOTY.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = gOTY.gameEngineFeature[0];
				gameScript4.gameEngineFeature[1] = gOTY.gameEngineFeature[1];
				gOTY.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = gOTY.gameEngineFeature[2];
				gOTY.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = gOTY.gameEngineFeature[3];
				gOTY.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int num12 = 0; num12 < gOTY.gamePlatform.Length; num12++)
				{
					gameScript4.gamePlatform[num12] = gOTY.gamePlatform[num12];
					gameScript4.gamePlatformScript[num12] = gOTY.gamePlatformScript[num12];
				}
				for (int num13 = 0; num13 < gameScript4.gameLanguage.Length; num13++)
				{
					gameScript4.gameLanguage[num13] = gOTY.gameLanguage[num13];
				}
				for (int num14 = 0; num14 < gameScript4.gameplayStudio.Length; num14++)
				{
					gameScript4.gameplayStudio[num14] = gOTY.gameplayStudio[num14];
				}
				for (int num15 = 0; num15 < gameScript4.grafikStudio.Length; num15++)
				{
					gameScript4.grafikStudio[num15] = gOTY.grafikStudio[num15];
				}
				for (int num16 = 0; num16 < gameScript4.soundStudio.Length; num16++)
				{
					gameScript4.soundStudio[num16] = gOTY.soundStudio[num16];
				}
				for (int num17 = 0; num17 < gameScript4.motionCaptureStudio.Length; num17++)
				{
					gameScript4.motionCaptureStudio[num17] = gOTY.motionCaptureStudio[num17];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = gOTY.gameLicence;
				gameScript4.gameAntiCheat = gOTY.gameAntiCheat;
				gameScript4.gameCopyProtect = gOTY.gameCopyProtect;
				gameScript4.Designschwerpunkt = (int[])gOTY.Designschwerpunkt.Clone();
				gameScript4.Designausrichtung = (int[])gOTY.Designausrichtung.Clone();
				gameScript4.gameGameplayFeatures = (bool[])gOTY.gameGameplayFeatures.Clone();
				gameScript4.gameplayFeatures_DevDone = (bool[])gOTY.gameplayFeatures_DevDone.Clone();
				gameScript4.userNegativ = gOTY.userNegativ;
				gameScript4.userPositiv = gOTY.userPositiv;
				gameScript4.reviewGameplay = gOTY.reviewGameplay;
				gameScript4.reviewGrafik = gOTY.reviewGrafik;
				gameScript4.reviewSound = gOTY.reviewSound;
				gameScript4.reviewSteuerung = gOTY.reviewSteuerung;
				gameScript4.reviewTotal = gOTY.reviewTotal;
				gameScript4.reviewGameplayText = gOTY.reviewGameplayText;
				gameScript4.reviewGrafikText = gOTY.reviewGrafikText;
				gameScript4.reviewSoundText = gOTY.reviewSoundText;
				gameScript4.reviewSteuerungText = gOTY.reviewSteuerungText;
				gameScript4.reviewTotalText = gOTY.reviewTotalText;
			}
			break;
		}
		case 9:
		{
			gameScript portForHandy = GetPortForHandy();
			if (!portForHandy || !unlock_.Get(65))
			{
				break;
			}
			portForHandy.FindMyEngineNew();
			if ((bool)portForHandy.engineS_ && portForHandy.engineS_.OwnerIsNPC())
			{
				flag = true;
				portForHandy.portExist[1] = true;
				gameScript4.SetMyName(portForHandy.GetNameSimple());
				gameScript4.handy = true;
				gameScript4.mainIP = portForHandy.mainIP;
				gameScript4.typ_standard = portForHandy.typ_standard;
				gameScript4.typ_nachfolger = portForHandy.typ_nachfolger;
				gameScript4.typ_remaster = portForHandy.typ_remaster;
				gameScript4.typ_spinoff = portForHandy.typ_spinoff;
				gameScript4.typ_goty = portForHandy.typ_goty;
				gameScript4.maingenre = portForHandy.maingenre;
				gameScript4.subgenre = portForHandy.subgenre;
				gameScript4.gameMainTheme = portForHandy.gameMainTheme;
				gameScript4.gameSubTheme = portForHandy.gameSubTheme;
				gameScript4.gameZielgruppe = portForHandy.gameZielgruppe;
				gameScript4.gameSize = portForHandy.gameSize;
				gameScript4.points_gameplay = portForHandy.points_gameplay;
				gameScript4.points_grafik = portForHandy.points_grafik;
				gameScript4.points_sound = portForHandy.points_sound;
				gameScript4.points_technik = portForHandy.points_technik;
				gameScript4.publisherID = portForHandy.publisherID;
				gameScript4.pS_ = portForHandy.pS_;
				gameScript4.portID = portForHandy.myID;
				gameScript4.usk = portForHandy.usk;
				gameScript4.npcLateinNumbers = portForHandy.npcLateinNumbers;
				gameScript4.gameLicence = portForHandy.gameLicence;
				gameScript4.sonderIP = portForHandy.sonderIP;
				gameScript4.sonderIPMindestreview = portForHandy.sonderIPMindestreview;
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				gameScript4.engineID = portForHandy.engineID;
				gameScript4.engineS_ = portForHandy.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = portForHandy.engineS_.GetBestFeature(eF_.GetTypGrafik());
				gameScript4.gameEngineFeature[1] = portForHandy.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = portForHandy.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = portForHandy.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int num35 = 0; num35 < gameScript4.gameLanguage.Length; num35++)
				{
					gameScript4.gameLanguage[num35] = portForHandy.gameLanguage[num35];
				}
				for (int num36 = 0; num36 < gameScript4.gameplayStudio.Length; num36++)
				{
					gameScript4.gameplayStudio[num36] = portForHandy.gameplayStudio[num36];
				}
				for (int num37 = 0; num37 < gameScript4.grafikStudio.Length; num37++)
				{
					gameScript4.grafikStudio[num37] = portForHandy.grafikStudio[num37];
				}
				for (int num38 = 0; num38 < gameScript4.soundStudio.Length; num38++)
				{
					gameScript4.soundStudio[num38] = portForHandy.soundStudio[num38];
				}
				for (int num39 = 0; num39 < gameScript4.motionCaptureStudio.Length; num39++)
				{
					gameScript4.motionCaptureStudio[num39] = portForHandy.motionCaptureStudio[num39];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetGameplayFeatures(gameScript4);
			}
			break;
		}
		case 10:
		{
			if (onlyMobile)
			{
				break;
			}
			gameScript portForArcade = GetPortForArcade();
			if (!portForArcade || !publisher)
			{
				break;
			}
			portForArcade.FindMyEngineNew();
			if ((bool)portForArcade.engineS_ && portForArcade.engineS_.OwnerIsNPC())
			{
				flag = true;
				portForArcade.portExist[2] = true;
				gameScript4.SetMyName(portForArcade.GetNameSimple());
				gameScript4.arcade = true;
				gameScript4.mainIP = portForArcade.mainIP;
				gameScript4.typ_standard = portForArcade.typ_standard;
				gameScript4.typ_nachfolger = portForArcade.typ_nachfolger;
				gameScript4.typ_remaster = portForArcade.typ_remaster;
				gameScript4.typ_spinoff = portForArcade.typ_spinoff;
				gameScript4.typ_goty = portForArcade.typ_goty;
				gameScript4.maingenre = portForArcade.maingenre;
				gameScript4.subgenre = portForArcade.subgenre;
				gameScript4.gameMainTheme = portForArcade.gameMainTheme;
				gameScript4.gameSubTheme = portForArcade.gameSubTheme;
				gameScript4.gameZielgruppe = portForArcade.gameZielgruppe;
				gameScript4.gameSize = portForArcade.gameSize;
				gameScript4.points_gameplay = portForArcade.points_gameplay;
				gameScript4.points_grafik = portForArcade.points_grafik;
				gameScript4.points_sound = portForArcade.points_sound;
				gameScript4.points_technik = portForArcade.points_technik;
				gameScript4.publisherID = portForArcade.publisherID;
				gameScript4.pS_ = portForArcade.pS_;
				gameScript4.portID = portForArcade.myID;
				gameScript4.usk = portForArcade.usk;
				gameScript4.npcLateinNumbers = portForArcade.npcLateinNumbers;
				gameScript4.gameLicence = portForArcade.gameLicence;
				gameScript4.sonderIP = portForArcade.sonderIP;
				gameScript4.sonderIPMindestreview = portForArcade.sonderIPMindestreview;
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				gameScript4.engineID = portForArcade.engineID;
				gameScript4.engineS_ = portForArcade.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = portForArcade.engineS_.GetBestFeature(eF_.GetTypGrafik());
				gameScript4.gameEngineFeature[1] = portForArcade.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = portForArcade.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = portForArcade.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int num18 = 0; num18 < gameScript4.gameLanguage.Length; num18++)
				{
					gameScript4.gameLanguage[num18] = portForArcade.gameLanguage[num18];
				}
				for (int num19 = 0; num19 < gameScript4.gameplayStudio.Length; num19++)
				{
					gameScript4.gameplayStudio[num19] = portForArcade.gameplayStudio[num19];
				}
				for (int num20 = 0; num20 < gameScript4.grafikStudio.Length; num20++)
				{
					gameScript4.grafikStudio[num20] = portForArcade.grafikStudio[num20];
				}
				for (int num21 = 0; num21 < gameScript4.soundStudio.Length; num21++)
				{
					gameScript4.soundStudio[num21] = portForArcade.soundStudio[num21];
				}
				for (int num22 = 0; num22 < gameScript4.motionCaptureStudio.Length; num22++)
				{
					gameScript4.motionCaptureStudio[num22] = portForArcade.motionCaptureStudio[num22];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetGameplayFeatures(gameScript4);
			}
			break;
		}
		case 11:
		{
			if (onlyMobile)
			{
				break;
			}
			gameScript portForPC = GetPortForPC();
			if (!portForPC || !publisher)
			{
				break;
			}
			portForPC.FindMyEngineNew();
			if ((bool)portForPC.engineS_ && portForPC.engineS_.OwnerIsNPC())
			{
				flag = true;
				portForPC.portExist[0] = true;
				gameScript4.SetMyName(portForPC.GetNameSimple());
				gameScript4.mainIP = portForPC.mainIP;
				gameScript4.typ_standard = portForPC.typ_standard;
				gameScript4.typ_nachfolger = portForPC.typ_nachfolger;
				gameScript4.typ_remaster = portForPC.typ_remaster;
				gameScript4.typ_spinoff = portForPC.typ_spinoff;
				gameScript4.typ_goty = portForPC.typ_goty;
				gameScript4.maingenre = portForPC.maingenre;
				gameScript4.subgenre = portForPC.subgenre;
				gameScript4.gameMainTheme = portForPC.gameMainTheme;
				gameScript4.gameSubTheme = portForPC.gameSubTheme;
				gameScript4.gameZielgruppe = portForPC.gameZielgruppe;
				gameScript4.gameSize = portForPC.gameSize;
				gameScript4.points_gameplay = portForPC.points_gameplay;
				gameScript4.points_grafik = portForPC.points_grafik;
				gameScript4.points_sound = portForPC.points_sound;
				gameScript4.points_technik = portForPC.points_technik;
				gameScript4.publisherID = portForPC.publisherID;
				gameScript4.pS_ = portForPC.pS_;
				gameScript4.portID = portForPC.myID;
				gameScript4.usk = portForPC.usk;
				gameScript4.npcLateinNumbers = portForPC.npcLateinNumbers;
				gameScript4.gameLicence = portForPC.gameLicence;
				gameScript4.sonderIP = portForPC.sonderIP;
				gameScript4.sonderIPMindestreview = portForPC.sonderIPMindestreview;
				FindPlatformsForGameNew(gameScript4, forceContractGame);
				gameScript4.engineID = portForPC.engineID;
				gameScript4.engineS_ = portForPC.engineS_;
				gameScript4.engineFeature_DevDone[0] = true;
				gameScript4.engineFeature_DevDone[1] = true;
				gameScript4.engineFeature_DevDone[2] = true;
				gameScript4.engineFeature_DevDone[3] = true;
				gameScript4.gameEngineFeature[0] = portForPC.engineS_.GetBestFeature(eF_.GetTypGrafik());
				gameScript4.gameEngineFeature[1] = portForPC.engineS_.GetBestFeature(eF_.GetTypSound());
				gameScript4.gameEngineFeature[2] = portForPC.engineS_.GetBestFeature(eF_.GetTypKI());
				gameScript4.gameEngineFeature[3] = portForPC.engineS_.GetBestFeature(eF_.GetTypPhysik());
				for (int l = 0; l < gameScript4.gameLanguage.Length; l++)
				{
					gameScript4.gameLanguage[l] = portForPC.gameLanguage[l];
				}
				for (int m = 0; m < gameScript4.gameplayStudio.Length; m++)
				{
					gameScript4.gameplayStudio[m] = portForPC.gameplayStudio[m];
				}
				for (int n = 0; n < gameScript4.grafikStudio.Length; n++)
				{
					gameScript4.grafikStudio[n] = portForPC.grafikStudio[n];
				}
				for (int num2 = 0; num2 < gameScript4.soundStudio.Length; num2++)
				{
					gameScript4.soundStudio[num2] = portForPC.soundStudio[num2];
				}
				for (int num3 = 0; num3 < gameScript4.motionCaptureStudio.Length; num3++)
				{
					gameScript4.motionCaptureStudio[num3] = portForPC.motionCaptureStudio[num3];
				}
				gameScript4.gameTyp = 0;
				gameScript4.gameLicence = -1;
				SetCopyProtect(gameScript4);
				SetAntiCheat(gameScript4);
				SetDesignSlider(gameScript4);
				SetGameplayFeatures(gameScript4);
			}
			break;
		}
		case 12:
		{
			gameScript[] array = new gameScript[5]
			{
				GetGameForAddonBundle(hauptgame_: true, -1),
				null,
				null,
				null,
				null
			};
			if ((bool)array[0])
			{
				array[1] = GetGameForAddonBundle(hauptgame_: false, array[0].myID);
				if ((bool)array[1])
				{
					array[1].bundle_created = true;
				}
				array[2] = GetGameForAddonBundle(hauptgame_: false, array[0].myID);
				if ((bool)array[2])
				{
					array[2].bundle_created = true;
				}
				array[3] = GetGameForAddonBundle(hauptgame_: false, array[0].myID);
				if ((bool)array[3])
				{
					array[3].bundle_created = true;
				}
				array[4] = GetGameForAddonBundle(hauptgame_: false, array[0].myID);
				if ((bool)array[4])
				{
					array[4].bundle_created = true;
				}
			}
			if ((bool)array[0] && (bool)array[1])
			{
				if (!publisher)
				{
					break;
				}
				flag = true;
				if ((bool)gameScript4)
				{
					gameScript4.tag = "GameRemoved";
					Object.Destroy(gameScript4.gameObject);
				}
				gameScript4 = Object.Instantiate(array[0].gameObject).GetComponent<gameScript>();
				games_.InitAddonBundle(gameScript4);
				string randomNPCAddonBundleName = tS_.GetRandomNPCAddonBundleName();
				randomNPCAddonBundleName = randomNPCAddonBundleName.Replace("<NAME>", array[0].GetNameSimple());
				gameScript4.SetMyName(randomNPCAddonBundleName);
				gameScript4.inDevelopment = false;
				gameScript4.developerID = myID;
				gameScript4.ownerID = myID;
				gameScript4.publisherID = -1;
				gameScript4.typ_standard = false;
				gameScript4.typ_nachfolger = false;
				gameScript4.typ_remaster = false;
				gameScript4.typ_budget = false;
				gameScript4.typ_addon = false;
				gameScript4.typ_addonStandalone = false;
				gameScript4.typ_bundle = false;
				gameScript4.typ_mmoaddon = false;
				gameScript4.typ_bundleAddon = true;
				gameScript4.warBeiAwards = true;
				gameScript4.weeksOnMarket = 0;
				gameScript4.releaseDate = 0;
				gameScript4.vorbestellungen = 0;
				gameScript4.spielbericht = false;
				gameScript4.spielbericht_favorit = false;
				gameScript4.userPositiv = 0;
				gameScript4.userNegativ = 0;
				gameScript4.reviewGameplayText = 0;
				gameScript4.reviewGrafikText = 0;
				gameScript4.reviewSoundText = 0;
				gameScript4.reviewSteuerungText = 0;
				gameScript4.reviewTotalText = 0;
				gameScript4.sellsTotalStandard = 0L;
				gameScript4.sellsTotalDeluxe = 0L;
				gameScript4.sellsTotalCollectors = 0L;
				gameScript4.sellsTotalOnline = 0L;
				gameScript4.sellsTotal = 0L;
				gameScript4.umsatzTotal = 0L;
				gameScript4.costs_entwicklung = 0L;
				gameScript4.costs_mitarbeiter = 0L;
				gameScript4.costs_marketing = 0L;
				gameScript4.costs_enginegebuehren = 0L;
				gameScript4.costs_server = 0L;
				gameScript4.costs_production = 0L;
				for (int j = 0; j < gameScript4.sellsPerWeek.Length; j++)
				{
					gameScript4.sellsPerWeek[j] = 0;
				}
				gameScript4.lagerbestand[0] = 0L;
				gameScript4.lagerbestand[1] = 0L;
				gameScript4.lagerbestand[2] = 0L;
				for (int k = 0; k < array.Length; k++)
				{
					if ((bool)array[k])
					{
						gameScript4.bundleID[k] = array[k].myID;
					}
				}
				gameScript4.reviewTotal -= 16;
				if ((bool)array[1])
				{
					gameScript4.reviewTotal += 4;
				}
				if ((bool)array[2])
				{
					gameScript4.reviewTotal += 4;
				}
				if ((bool)array[3])
				{
					gameScript4.reviewTotal += 4;
				}
				if ((bool)array[4])
				{
					gameScript4.reviewTotal += 4;
				}
				gameScript4.reviewTotal -= (mS_.year - gameScript4.date_start_year) * 2;
				if (gameScript4.reviewTotal <= 0)
				{
					gameScript4.reviewTotal = 1;
				}
			}
			else
			{
				if ((bool)array[0])
				{
					array[0].bundle_created = false;
				}
				if ((bool)array[1])
				{
					array[1].bundle_created = false;
				}
				if ((bool)array[2])
				{
					array[2].bundle_created = false;
				}
				if ((bool)array[3])
				{
					array[3].bundle_created = false;
				}
				if ((bool)array[4])
				{
					array[4].bundle_created = false;
				}
			}
			break;
		}
		}
		if (!flag)
		{
			SetNewGameInWeeks(9999);
			if ((bool)gameScript4)
			{
				gameScript4.gameObject.tag = "GameRemoved";
				Object.Destroy(gameScript4.gameObject);
				games_.FindGames();
			}
			if (IsTochterfirma() && amountTrys < 10)
			{
				amountTrys++;
				newGameInWeeks = 1;
				CreateNewGame2(forceContractGame, newTryForTochterfirma: true);
			}
			return null;
		}
		ReleaseTheGame(gameScript4, forceContractGame, ignoreTochterfirma: false);
		return gameScript4;
	}

	public void ReleaseTheGame(gameScript script_, bool forceContractGame, bool ignoreTochterfirma)
	{
		if (!ignoreTochterfirma && IsTochterfirma() && IsMyTochterfirma() && (script_.gameTyp == 0 || script_.gameTyp == 1) && !script_.arcade && !script_.handy && (script_.typ_standard || script_.typ_remaster || script_.typ_nachfolger || script_.typ_spinoff || script_.typ_budget || script_.typ_goty || script_.typ_addon || script_.typ_addonStandalone || script_.typ_bundle || script_.typ_bundleAddon))
		{
			script_.SetAsGameInDevelopmentNPC();
			if (script_.typ_budget || script_.typ_goty || script_.typ_bundle || script_.typ_bundleAddon)
			{
				newGameInWeeks = 2;
				newGameInWeeksORG = 2;
			}
			return;
		}
		if (publisher)
		{
			script_.publisherID = -1;
			if (IsMyTochterfirma() && tf_ownPublisher && tf_ownPublisherPriority != -1)
			{
				script_.FindPublisherForGame();
			}
			if (script_.publisherID == -1)
			{
				script_.publisherID = myID;
				script_.pS_ = this;
			}
			if (script_.typ_budget || script_.typ_goty || script_.typ_bundle || script_.typ_bundleAddon)
			{
				newGameInWeeks = 2;
				newGameInWeeksORG = 2;
			}
			script_.SetVerkaufspreisNPC();
			if (!SETTING_SUBVENTION && ((mS_.globalEvent != 2 && !script_.sonderIP && !IsTochterfirma()) || forceContractGame) && script_.engineID == 0 && script_.gameTyp == 0 && !script_.arcade && !script_.handy && !script_.retro && !script_.typ_remaster && !script_.typ_budget && !script_.typ_goty && !script_.typ_bundle && !script_.typ_bundleAddon && !script_.typ_addon && !script_.typ_addonStandalone && !script_.typ_mmoaddon && (script_.typ_standard || script_.typ_nachfolger || script_.typ_spinoff) && (bool)mS_.contractWorkMain_ && (Random.Range(0, 100) > 80 || (Random.Range(0, 100) > 50 && mS_.contractWorkMain_.anzContractGames < 3) || forceContractGame))
			{
				script_.auftragsspiel = true;
				script_.exklusiv = true;
				script_.gameLicence = -1;
				for (int i = 1; i < script_.gamePlatform.Length; i++)
				{
					script_.gamePlatform[i] = -1;
					script_.gamePlatformScript[i] = null;
				}
				nextGameAddon = false;
				nextGameMMOAddon = false;
			}
		}
		else if (!SETTING_SUBVENTION && !IsTochterfirma() && mS_.pubOffersAmount < mS_.GetStudioLevel(mS_.studioPoints) / 2 + 1 && Random.Range(0, 100) > 50 && script_.gameTyp == 0 && !script_.arcade && !script_.handy && !script_.typ_budget && !script_.typ_goty && !script_.typ_bundle && !script_.typ_bundleAddon && !script_.typ_addon && !script_.typ_addonStandalone && !script_.typ_mmoaddon && (script_.typ_standard || script_.typ_remaster || script_.typ_nachfolger || script_.typ_spinoff))
		{
			mS_.pubOffersAmount++;
			script_.pubAngebot = true;
			nextGameAddon = false;
			nextGameMMOAddon = false;
		}
		else
		{
			script_.FindPublisherForGame();
			script_.SetVerkaufspreisNPC();
		}
		if (!script_.pubAngebot && !script_.auftragsspiel)
		{
			script_.SetAsGameInDevelopmentNPC();
			return;
		}
		if (script_.pubAngebot)
		{
			script_.SetAsPublishingAngebot();
		}
		if (script_.auftragsspiel)
		{
			script_.SetAsContractGameAngebot();
		}
	}

	public gameScript FindAngekuendigtesGame()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].angekuendigt && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].developerID == myID)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	public gameScript FindGameInDevelopment()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].developerID == myID)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private void ReleaseGameInDevelopment()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i] || !games_.arrayGamesScripts[i].inDevelopment || games_.arrayGamesScripts[i].isOnMarket || games_.arrayGamesScripts[i].auftragsspiel || games_.arrayGamesScripts[i].pubAngebot || games_.arrayGamesScripts[i].developerID != myID)
			{
				continue;
			}
			RemoveUnreleasedKonsolen(games_.arrayGamesScripts[i]);
			if (IsTochterfirma() && IsMyTochterfirma())
			{
				gameScript gameScript2 = games_.arrayGamesScripts[i];
				if ((gameScript2.gameTyp == 0 || gameScript2.gameTyp == 1) && !gameScript2.arcade && !gameScript2.handy && (gameScript2.typ_standard || gameScript2.typ_remaster || gameScript2.typ_nachfolger || gameScript2.typ_spinoff || gameScript2.typ_budget || gameScript2.typ_goty || gameScript2.typ_addon || gameScript2.typ_addonStandalone))
				{
					int reviewTotal = gameScript2.reviewTotal;
					if (gameScript2.reviewTotal <= 0)
					{
						gameScript2.CalcReview(entwicklungsbericht: true);
						reviewTotal = gameScript2.reviewTotal;
						gameScript2.ClearReview();
					}
					if (!tf_autoRelease || (tf_autoRelease && tf_autoReleaseVal != 0 && reviewTotal >= tf_autoReleaseVal * 10))
					{
						Debug.Log("Coroutine: " + games_.arrayGamesScripts[i].myName);
						StartCoroutine(iWaitTochterfirmaReleaseGame(gameScript2, this));
						break;
					}
					if (games_.arrayGamesScripts[i].reviewTotal <= 0)
					{
						games_.arrayGamesScripts[i].CalcReview(entwicklungsbericht: false);
						reviewText_.GetReviewText(games_.arrayGamesScripts[i]);
					}
					else
					{
						games_.arrayGamesScripts[i].date_year = mS_.year;
						games_.arrayGamesScripts[i].date_month = mS_.month;
						reviewText_.GetReviewText(games_.arrayGamesScripts[i]);
					}
					ReleaseTheGame(gameScript2, forceContractGame: false, ignoreTochterfirma: true);
					SetGameOnMarket(gameScript2);
					if (tf_autoGamePass && gpS_.gamePass_aktiv && gameScript2.CanBeInGamePass())
					{
						gpS_.GAMEPASS_AddGame(gameScript2, updateGamesAmount: true);
					}
					break;
				}
			}
			if (games_.arrayGamesScripts[i].reviewTotal <= 0)
			{
				games_.arrayGamesScripts[i].CalcReview(entwicklungsbericht: false);
				reviewText_.GetReviewText(games_.arrayGamesScripts[i]);
			}
			else
			{
				games_.arrayGamesScripts[i].date_year = mS_.year;
				games_.arrayGamesScripts[i].date_month = mS_.month;
				reviewText_.GetReviewText(games_.arrayGamesScripts[i]);
			}
			if (games_.arrayGamesScripts[i].publisherID == -1)
			{
				Debug.Log("Bugfix: " + games_.arrayGamesScripts[i].myID + " / " + games_.arrayGamesScripts[i].GetNameSimple());
				ReleaseTheGame(games_.arrayGamesScripts[i], forceContractGame: false, ignoreTochterfirma: true);
			}
			SetGameOnMarket(games_.arrayGamesScripts[i]);
		}
	}

	public void RemoveUnreleasedKonsolen(gameScript script_)
	{
		if (!script_)
		{
			return;
		}
		script_.FindMyPlatforms();
		for (int i = 0; i < script_.gamePlatformScript.Length; i++)
		{
			if ((bool)script_.gamePlatformScript[i] && !script_.gamePlatformScript[i].isUnlocked)
			{
				script_.gamePlatform[i] = -1;
				script_.gamePlatformScript[i] = null;
			}
		}
	}

	public void SetGameOnMarket(gameScript script_)
	{
		if (!script_)
		{
			return;
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isClient)
			{
				if (IsMyTochterfirma())
				{
					script_.isOnMarket = true;
					mS_.mpCalls_.CLIENT_Send_Game(script_);
				}
			}
			else
			{
				script_.SetOnMarket();
			}
		}
		else
		{
			script_.SetOnMarket();
		}
		bool flag = false;
		script_.FindMyPlatforms();
		for (int i = 0; i < script_.gamePlatformScript.Length; i++)
		{
			if ((bool)script_.gamePlatformScript[i] && script_.gamePlatformScript[i].ownerID == mS_.myID)
			{
				flag = true;
				break;
			}
		}
		if (mS_.newsSetting[0] || (mS_.newsSetting[9] && flag))
		{
			string text = tS_.GetText(494);
			text = ((!script_.GetPublisherIsTochtefirma()) ? text.Replace("<NAME1>", script_.GetPublisherName()) : text.Replace("<NAME1>", "<color=green><b>" + script_.GetPublisherName() + "</b></color>"));
			text = ((!script_.GetDeveloperIsTochtefirma()) ? text.Replace("<NAME2>", script_.GetNameWithTag()) : text.Replace("<NAME2>", "<color=green><b>" + script_.GetNameWithTag() + "</b></color>"));
			guiMain_.CreateTopNewsInfo(text);
		}
	}

	private void NewsAnkuendigung(gameScript script_)
	{
		if ((bool)script_)
		{
			if (mS_.newsSetting[10])
			{
				string text = tS_.GetText(2058);
				text = text.Replace("<NAME1>", script_.GetDeveloperName());
				text = text.Replace("<NAME2>", script_.GetNameWithTag());
				guiMain_.CreateTopNewsGameAnkuendigung(text);
			}
			if (mS_.multiplayer && mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_GameAnkuendigung(script_);
			}
		}
	}

	private void SetMMOorF2P(gameScript script_, int platTyp)
	{
		if (platTyp != 0 && platTyp != 1 && platTyp != 5 && platTyp != 2)
		{
			return;
		}
		int num = Random.Range(0, 20);
		if (onlyMobile && Random.Range(0, 100) > 50)
		{
			num = 1;
		}
		if (IsTochterfirma())
		{
			if ((!tf_allowMMO && num == 0) || (!tf_allowF2P && num == 1) || (tf_onlyPlayerConsole && num == 1))
			{
				return;
			}
		}
		else
		{
			if (script_.gameTyp == 1)
			{
				num = 0;
			}
			if (script_.gameTyp == 2)
			{
				num = 1;
			}
		}
		switch (num)
		{
		case 0:
			if (gF_.gameplayFeatures_UNLOCK[23])
			{
				script_.gameTyp = 1;
				script_.aboPreis = 5;
			}
			break;
		case 1:
			if (publisher && unlock_.Get(22))
			{
				script_.gameTyp = 2;
				script_.exklusiv = false;
				script_.herstellerExklusiv = false;
				script_.verkaufspreis[0] = 0;
				script_.inAppPurchase[0] = true;
				script_.inAppPurchase[1] = true;
				script_.inAppPurchase[2] = true;
				script_.inAppPurchase[3] = true;
				if (Random.Range(0, 100) > 50)
				{
					script_.inAppPurchase[4] = true;
				}
				if (Random.Range(0, 100) > 50)
				{
					script_.inAppPurchase[5] = true;
				}
			}
			break;
		}
	}

	private int SetPlatformTyp(gameScript script_, bool forceContractGame)
	{
		int num = 0;
		if (Random.Range(0, 100) < 20)
		{
			num = 1;
		}
		if (Random.Range(0, 100) < 20 && publisher && unlock_.Get(65))
		{
			num = 5;
		}
		if (Random.Range(0, 100) < 20 && publisher)
		{
			num = 4;
		}
		if (Random.Range(0, 100) < 20 && mS_.year > 1995)
		{
			num = 3;
		}
		if (script_.gamePlatform[0] != 0)
		{
			if ((bool)script_.gamePlatformScript[0])
			{
				switch (script_.gamePlatformScript[0].typ)
				{
				case 0:
					num = (script_.exklusiv ? 1 : 0);
					script_.exklusiv = false;
					break;
				case 1:
					num = (script_.exklusiv ? 1 : 0);
					script_.exklusiv = false;
					break;
				case 2:
					num = (script_.exklusiv ? 1 : 0);
					script_.exklusiv = false;
					break;
				case 3:
					num = 5;
					break;
				case 4:
					num = 4;
					break;
				}
			}
			if (!publisher && (num == 5 || num == 4))
			{
				num = 0;
				script_.gamePlatform[0] = -1;
				script_.gamePlatformScript[0] = null;
			}
		}
		if (IsTochterfirma())
		{
			if (tf_noArcade && num == 4)
			{
				num = 0;
			}
			if (tf_noHandy && num == 5)
			{
				num = 0;
			}
			if (tf_noRetro && num == 3)
			{
				num = 0;
			}
		}
		if (onlyMobile && unlock_.Get(65))
		{
			num = 5;
		}
		if (script_.sonderIP && num == 3)
		{
			num = ((Random.Range(0, 100) <= 50) ? 1 : 0);
		}
		if (IsTochterfirma() && tf_onlyPlayerConsole)
		{
			num = 1;
			if (Random.Range(0, 100) > 20)
			{
				num = 2;
			}
		}
		if (forceContractGame)
		{
			num = 1;
		}
		if (num == 1)
		{
			script_.exklusiv = true;
		}
		if (num == 5)
		{
			script_.handy = true;
		}
		if (num == 4)
		{
			script_.arcade = true;
		}
		if (num == 3)
		{
			script_.retro = true;
		}
		if (num == 2)
		{
			script_.herstellerExklusiv = true;
		}
		return num;
	}

	private void SetGameSize(gameScript script_)
	{
		if (!IsTochterfirma())
		{
			if (mS_.year >= 1979 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 1;
			}
			if (mS_.year >= 1984 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 2;
			}
			if (!script_.retro && mS_.year >= 1986 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 3;
			}
			if (!script_.retro && mS_.year >= 1990 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 4;
			}
			if (!script_.retro && mS_.year >= 2020 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 5;
			}
			return;
		}
		if (tf_gameSize == 0)
		{
			if (mS_.year >= 1978 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 1;
			}
			if (mS_.year >= 1982 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 2;
			}
			if (!script_.retro && mS_.year >= 1986 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 3;
			}
			if (!script_.retro && mS_.year >= 1990 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 4;
			}
			if (!script_.retro && mS_.year >= 2020 && (Random.Range(0, 100) > 50 || script_.sonderIP))
			{
				script_.gameSize = 5;
			}
			return;
		}
		if (mS_.year >= 1978 && (Random.Range(0, 100) > 50 || script_.sonderIP))
		{
			script_.gameSize = 1;
		}
		if (mS_.year >= 1982 && (Random.Range(0, 100) > 50 || script_.sonderIP))
		{
			script_.gameSize = 2;
		}
		if (!script_.retro && mS_.year >= 1986 && (Random.Range(0, 100) > 50 || script_.sonderIP))
		{
			script_.gameSize = 3;
		}
		if (!script_.retro && mS_.year >= 1990 && (Random.Range(0, 100) > 50 || script_.sonderIP))
		{
			script_.gameSize = 4;
		}
		if (!script_.retro && mS_.year >= 1990 && (Random.Range(0, 100) > 50 || script_.sonderIP))
		{
			script_.gameSize = 5;
		}
		if (!script_.retro && !script_.sonderIP)
		{
			script_.gameSize = tf_gameSize - 1;
		}
	}

	private void SetGameplayFeatures(gameScript script_)
	{
		script_.gameGameplayFeatures = new bool[gF_.gameplayFeatures_LEVEL.Length];
		script_.gameplayFeatures_DevDone = new bool[gF_.gameplayFeatures_LEVEL.Length];
		bool flag = true;
		script_.FindMyPlatforms();
		for (int i = 0; i < script_.gamePlatformScript.Length; i++)
		{
			if ((bool)script_.gamePlatformScript[i] && !script_.gamePlatformScript[i].internet)
			{
				flag = false;
				break;
			}
		}
		if (script_.gameTyp == 1 || script_.gameTyp == 2)
		{
			script_.gameGameplayFeatures[23] = true;
			script_.gameplayFeatures_DevDone[23] = true;
			script_.costs_entwicklung += gF_.GetDevCosts(0);
		}
		int num = 0;
		if (tf_gameSize > 0)
		{
			num = menuDevGame_.maxFeatures_gameSize[tf_gameSize - 1] / 2;
		}
		int num2 = 0;
		int num3 = menuDevGame_.maxFeatures_gameSize[script_.gameSize];
		for (int j = 0; j < script_.gameGameplayFeatures.Length; j++)
		{
			if (gF_.gameplayFeatures_UNLOCK[j] && (Random.Range(0, 100) > 30 || script_.sonderIP || num2 < num) && num2 < num3)
			{
				bool flag2 = false;
				if (script_.arcade && gF_.gameplayFeatures_LOCKPLATFORM[j, 4])
				{
					flag2 = true;
				}
				if (script_.handy && gF_.gameplayFeatures_LOCKPLATFORM[j, 3])
				{
					flag2 = true;
				}
				if (!flag && gF_.gameplayFeatures_INTERNET[j])
				{
					flag2 = true;
				}
				if (!flag2)
				{
					num2++;
					script_.gameGameplayFeatures[j] = true;
					script_.gameplayFeatures_DevDone[j] = true;
					script_.points_gameplay += gF_.GetGameplay(j, script_.maingenre, script_.subgenre);
					script_.points_grafik += gF_.GetGraphic(j, script_.maingenre, script_.subgenre);
					script_.points_sound += gF_.GetSound(j, script_.maingenre, script_.subgenre);
					script_.points_technik += gF_.GetTechnik(j, script_.maingenre, script_.subgenre);
					script_.costs_entwicklung += gF_.GetDevCosts(j);
					script_.costs_mitarbeiter += Random.Range(1000, 10000);
				}
			}
		}
		for (int k = 0; k < script_.gameGameplayFeatures.Length; k++)
		{
			if (script_.gameGameplayFeatures[k] && gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k] > -1)
			{
				script_.gameGameplayFeatures[gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k]] = true;
				script_.gameplayFeatures_DevDone[gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[k]] = true;
			}
		}
		if (script_.gameTyp != 0)
		{
			script_.costs_entwicklung += menuDevGame_.costs_gameTyp[script_.gameTyp] * (mS_.difficulty + 1);
		}
		else
		{
			script_.costs_entwicklung += menuDevGame_.costs_gameTyp[script_.gameTyp];
		}
		if (script_.gameSize != 0)
		{
			script_.costs_entwicklung += menuDevGame_.costs_gameSize[script_.gameSize] * (mS_.difficulty + 1);
		}
		else
		{
			script_.costs_entwicklung += menuDevGame_.costs_gameSize[script_.gameSize];
		}
	}

	private void FindEngineForGameNew(gameScript gS_)
	{
		engineScript engineScript2 = null;
		int num = 0;
		float num2 = 0f;
		engineScript engineScript3 = null;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].isUnlocked && mS_.arrayEnginesScripts[i].OwnerIsNPC())
			{
				if (num < mS_.arrayEnginesScripts[i].preis)
				{
					num = mS_.arrayEnginesScripts[i].preis;
					engineScript3 = mS_.arrayEnginesScripts[i];
				}
				if (num2 < (float)mS_.arrayEnginesScripts[i].gewinnbeteiligung)
				{
					num2 = mS_.arrayEnginesScripts[i].gewinnbeteiligung;
				}
			}
		}
		num += 20000;
		num2 += 1f;
		for (int j = 0; j < mS_.arrayEnginesScripts.Length; j++)
		{
			if (!mS_.arrayEnginesScripts[j])
			{
				continue;
			}
			if (mS_.arrayEnginesScripts[j].myID == 0)
			{
				engineScript2 = mS_.arrayEnginesScripts[j];
				if (Random.Range(0, 100) < 50 || gS_.gameTyp != 0)
				{
					break;
				}
			}
			if (Random.Range(0, 100) >= 50)
			{
				continue;
			}
			bool flag = false;
			if (!mS_.arrayEnginesScripts[j].OwnerIsNPC())
			{
				flag = true;
			}
			int num3 = Mathf.RoundToInt(mS_.arrayEnginesScripts[j].GetMarktdominanz());
			if (((mS_.arrayEnginesScripts[j].isUnlocked && !flag) || (flag && !mS_.arrayEnginesScripts[j].updating && mS_.arrayEnginesScripts[j].Complete() && Random.Range(0, mS_.difficulty) == 0)) && mS_.arrayEnginesScripts[j].GetTechLevel() >= engineScript2.GetTechLevel() && Random.Range(0, mS_.arrayEnginesScripts[j].GetFeaturesAmount() + num3) > Random.Range(0, engineScript2.GetFeaturesAmount()) && ((mS_.arrayEnginesScripts[j].GetPreis() - num3 * 1000 < Random.Range(0, num) && (float)mS_.arrayEnginesScripts[j].GetGewinnbeteiligung() < Random.Range(0f, num2)) || !flag))
			{
				if (!flag)
				{
					engineScript2 = mS_.arrayEnginesScripts[j];
				}
				else if (mS_.arrayEnginesScripts[j].sellEngine)
				{
					engineScript2 = mS_.arrayEnginesScripts[j];
				}
			}
		}
		if (IsTochterfirma() && tf_engine != -1 && tf_engine != 0)
		{
			for (int k = 0; k < mS_.arrayEnginesScripts.Length; k++)
			{
				if ((bool)mS_.arrayEnginesScripts[k] && mS_.arrayEnginesScripts[k].myID == tf_engine)
				{
					engineScript2 = mS_.arrayEnginesScripts[k];
					break;
				}
			}
		}
		if ((bool)engineScript2)
		{
			gS_.engineID = engineScript2.myID;
			gS_.engineS_ = engineScript2;
			gS_.engineGewinnbeteiligung = engineScript2.GetGewinnbeteiligung();
			gS_.engineFeature_DevDone[0] = true;
			gS_.engineFeature_DevDone[1] = true;
			gS_.engineFeature_DevDone[2] = true;
			gS_.engineFeature_DevDone[3] = true;
			gS_.gameEngineFeature[0] = engineScript2.GetBestFeature(eF_.GetTypGrafik());
			gS_.gameEngineFeature[1] = engineScript2.GetBestFeature(eF_.GetTypSound());
			gS_.gameEngineFeature[2] = engineScript2.GetBestFeature(eF_.GetTypKI());
			gS_.gameEngineFeature[3] = engineScript2.GetBestFeature(eF_.GetTypPhysik());
			if (engineScript2.myID == 0 && (bool)engineScript3)
			{
				gS_.gameEngineFeature[0] = engineScript3.GetBestFeature(eF_.GetTypGrafik());
				gS_.gameEngineFeature[1] = engineScript3.GetBestFeature(eF_.GetTypSound());
				gS_.gameEngineFeature[2] = engineScript3.GetBestFeature(eF_.GetTypKI());
				gS_.gameEngineFeature[3] = engineScript3.GetBestFeature(eF_.GetTypPhysik());
			}
			gS_.costs_entwicklung += eF_.GetDevCosts(gS_.gameEngineFeature[0]);
			gS_.costs_entwicklung += eF_.GetDevCosts(gS_.gameEngineFeature[1]);
			gS_.costs_entwicklung += eF_.GetDevCosts(gS_.gameEngineFeature[2]);
			gS_.costs_entwicklung += eF_.GetDevCosts(gS_.gameEngineFeature[3]);
			if (!engineScript2.OwnerIsNPC())
			{
				engineScript2.SellPlayerEngine(this);
			}
		}
	}

	private void FindPlatformsForGameNew(gameScript gS_, bool forceContractGame_)
	{
		platformScript platformScript2 = null;
		int num = 0;
		if (!mS_)
		{
			FindScripts();
		}
		if (!IsTochterfirma())
		{
			if (gS_.gamePlatform[0] > 0 && !gS_.typ_nachfolger)
			{
				num++;
				if ((bool)gS_.gamePlatformScript[0])
				{
					gS_.costs_entwicklung += gS_.gamePlatformScript[0].dev_costs;
				}
				if (gS_.exklusiv)
				{
					ClearPlatforms(gS_);
					return;
				}
			}
			if (gS_.gamePlatform[0] > 0 && gS_.typ_nachfolger && (bool)gS_.gamePlatformScript[0] && gS_.gamePlatformScript[0].IsVerfuegbar())
			{
				num++;
				gS_.costs_entwicklung += gS_.gamePlatformScript[0].dev_costs;
				if (gS_.exklusiv)
				{
					ClearPlatforms(gS_);
					return;
				}
			}
		}
		else
		{
			gS_.gamePlatform[0] = -1;
			gS_.gamePlatformScript[0] = null;
		}
		if (IsTochterfirma() && !tf_onlyPlayerConsole && (tf_platformFocus[0] != -1 || tf_platformFocus[1] != -1 || tf_platformFocus[2] != -1 || tf_platformFocus[3] != -1))
		{
			for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
			{
				if (!mS_.arrayPlatformsScripts[i] || (tf_platformFocus[0] != mS_.arrayPlatformsScripts[i].myID && tf_platformFocus[1] != mS_.arrayPlatformsScripts[i].myID && tf_platformFocus[2] != mS_.arrayPlatformsScripts[i].myID && tf_platformFocus[3] != mS_.arrayPlatformsScripts[i].myID) || mS_.arrayPlatformsScripts[i].IsProConsoleInDev() || ((!gS_.handy || mS_.arrayPlatformsScripts[i].typ != 3) && (!gS_.arcade || mS_.arrayPlatformsScripts[i].typ != 4) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[i].typ != 0) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[i].typ != 1) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[i].typ != 2)) || ((!gS_.retro || !mS_.arrayPlatformsScripts[i].vomMarktGenommen) && (gS_.retro || mS_.arrayPlatformsScripts[i].vomMarktGenommen)) || (gS_.gameTyp != 0 && (gS_.gameTyp != 1 || !mS_.arrayPlatformsScripts[i].internet) && (gS_.gameTyp != 2 || !mS_.arrayPlatformsScripts[i].internet)))
				{
					continue;
				}
				if (mS_.arrayPlatformsScripts[i].isUnlocked)
				{
					gS_.gamePlatform[num] = mS_.arrayPlatformsScripts[i].myID;
					gS_.gamePlatformScript[num] = mS_.arrayPlatformsScripts[i];
					gS_.costs_entwicklung += mS_.arrayPlatformsScripts[i].dev_costs;
					if (gS_.exklusiv)
					{
						ClearPlatforms(gS_);
						return;
					}
					num++;
					if (num >= 4)
					{
						ClearPlatforms(gS_);
						return;
					}
				}
				else if (mS_.arrayPlatformsScripts[i].ownerID == mS_.myID && num >= 1)
				{
					Debug.Log("Platform (unreleased): " + mS_.arrayPlatformsScripts[i].GetName());
					gS_.gamePlatform[num] = mS_.arrayPlatformsScripts[i].myID;
					gS_.gamePlatformScript[num] = mS_.arrayPlatformsScripts[i];
					gS_.costs_entwicklung += mS_.arrayPlatformsScripts[i].dev_costs;
					num++;
					if (num >= 4)
					{
						ClearPlatforms(gS_);
						return;
					}
				}
			}
		}
		if (IsTochterfirma())
		{
			for (int j = 0; j < mS_.arrayPlatformsScripts.Length; j++)
			{
				if ((bool)mS_.arrayPlatformsScripts[j] && gS_.gamePlatform[0] != mS_.arrayPlatformsScripts[j].myID && gS_.gamePlatform[1] != mS_.arrayPlatformsScripts[j].myID && gS_.gamePlatform[2] != mS_.arrayPlatformsScripts[j].myID && gS_.gamePlatform[3] != mS_.arrayPlatformsScripts[j].myID && ((gS_.handy && mS_.arrayPlatformsScripts[j].typ == 3) || (gS_.arcade && mS_.arrayPlatformsScripts[j].typ == 4) || (!gS_.handy && !gS_.arcade && mS_.arrayPlatformsScripts[j].typ == 0) || (!gS_.handy && !gS_.arcade && mS_.arrayPlatformsScripts[j].typ == 1) || (!gS_.handy && !gS_.arcade && mS_.arrayPlatformsScripts[j].typ == 2)) && mS_.arrayPlatformsScripts[j].isUnlocked && mS_.arrayPlatformsScripts[j].ownerID == mS_.myID && !mS_.arrayPlatformsScripts[j].IsProConsoleInDev() && ((gS_.retro && mS_.arrayPlatformsScripts[j].vomMarktGenommen) || (!gS_.retro && !mS_.arrayPlatformsScripts[j].vomMarktGenommen)) && (gS_.gameTyp == 0 || (gS_.gameTyp == 1 && mS_.arrayPlatformsScripts[j].internet) || (gS_.gameTyp == 2 && mS_.arrayPlatformsScripts[j].internet)))
				{
					platformScript2 = mS_.arrayPlatformsScripts[j];
					gS_.gamePlatform[num] = mS_.arrayPlatformsScripts[j].myID;
					gS_.gamePlatformScript[num] = mS_.arrayPlatformsScripts[j];
					gS_.costs_entwicklung += mS_.arrayPlatformsScripts[j].dev_costs;
					if (gS_.exklusiv)
					{
						ClearPlatforms(gS_);
						return;
					}
					num++;
					if (num >= 4)
					{
						ClearPlatforms(gS_);
						return;
					}
				}
			}
		}
		if (IsTochterfirma() && tf_onlyPlayerConsole)
		{
			if (num != 0)
			{
				if (num == 1 && gS_.herstellerExklusiv)
				{
					gS_.herstellerExklusiv = false;
					gS_.exklusiv = true;
				}
				ClearPlatforms(gS_);
				return;
			}
			if (gS_.herstellerExklusiv)
			{
				gS_.herstellerExklusiv = false;
				gS_.exklusiv = true;
			}
		}
		if (ownPlatform)
		{
			for (int k = 0; k < mS_.arrayPlatformsScripts.Length; k++)
			{
				if (!mS_.arrayPlatformsScripts[k] || gS_.gamePlatform[0] == mS_.arrayPlatformsScripts[k].myID || gS_.gamePlatform[1] == mS_.arrayPlatformsScripts[k].myID || gS_.gamePlatform[2] == mS_.arrayPlatformsScripts[k].myID || gS_.gamePlatform[3] == mS_.arrayPlatformsScripts[k].myID || ((!gS_.handy || mS_.arrayPlatformsScripts[k].typ != 3) && (!gS_.arcade || mS_.arrayPlatformsScripts[k].typ != 4) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[k].typ != 0) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[k].typ != 1) && (gS_.handy || gS_.arcade || mS_.arrayPlatformsScripts[k].typ != 2)) || !mS_.arrayPlatformsScripts[k].isUnlocked || mS_.arrayPlatformsScripts[k].IsProConsoleInDev() || ((!gS_.retro || !mS_.arrayPlatformsScripts[k].vomMarktGenommen) && (gS_.retro || mS_.arrayPlatformsScripts[k].vomMarktGenommen)) || (gS_.gameTyp != 0 && (gS_.gameTyp != 1 || !mS_.arrayPlatformsScripts[k].internet) && (gS_.gameTyp != 2 || !mS_.arrayPlatformsScripts[k].internet)) || !mS_.arrayPlatformsScripts[k].OwnerIsNPC())
				{
					continue;
				}
				platformScript2 = mS_.arrayPlatformsScripts[k];
				bool flag = false;
				if (mS_.arrayPlatformsScripts[k].ownerID == myID)
				{
					flag = true;
				}
				if (flag)
				{
					gS_.gamePlatform[num] = mS_.arrayPlatformsScripts[k].myID;
					gS_.gamePlatformScript[num] = mS_.arrayPlatformsScripts[k];
					gS_.costs_entwicklung += mS_.arrayPlatformsScripts[k].dev_costs;
					if (gS_.exklusiv)
					{
						ClearPlatforms(gS_);
						return;
					}
					num++;
					if (num >= 4)
					{
						ClearPlatforms(gS_);
						return;
					}
				}
			}
		}
		if ((!exklusive && num <= 3) || num == 0)
		{
			platformList.Clear();
			for (int l = 0; l < mS_.arrayPlatformsScripts.Length; l++)
			{
				if ((bool)mS_.arrayPlatformsScripts[l] && mS_.arrayPlatformsScripts[l].isUnlocked && !mS_.arrayPlatformsScripts[l].IsProConsoleInDev() && gS_.gamePlatform[0] != mS_.arrayPlatformsScripts[l].myID && gS_.gamePlatform[1] != mS_.arrayPlatformsScripts[l].myID && gS_.gamePlatform[2] != mS_.arrayPlatformsScripts[l].myID && gS_.gamePlatform[3] != mS_.arrayPlatformsScripts[l].myID && ((gS_.retro && mS_.arrayPlatformsScripts[l].vomMarktGenommen) || (!gS_.retro && !mS_.arrayPlatformsScripts[l].vomMarktGenommen)))
				{
					if (mS_.arrayPlatformsScripts[l].OwnerIsNPC())
					{
						platformList.Add(new PlatformList(mS_.arrayPlatformsScripts[l], mS_.arrayPlatformsScripts[l].GetMarktanteil()));
					}
					else if (mS_.arrayPlatformsScripts[l].thridPartyGames)
					{
						platformList.Add(new PlatformList(mS_.arrayPlatformsScripts[l], mS_.arrayPlatformsScripts[l].GetMarktanteil()));
					}
				}
			}
			platformList = platformList.OrderByDescending((PlatformList platformList) => platformList.marktanteil).ToList();
			if (!ownPlatform && !IsTochterfirma())
			{
				for (int num2 = 0; num2 < platformList.Count; num2++)
				{
					platformScript script_ = platformList[num2].script_;
					if (!script_ || gS_.gamePlatform[0] == script_.myID || gS_.gamePlatform[1] == script_.myID || gS_.gamePlatform[2] == script_.myID || gS_.gamePlatform[3] == script_.myID || ((!gS_.handy || script_.typ != 3) && (!gS_.arcade || script_.typ != 4) && (gS_.handy || gS_.arcade || script_.typ != 0) && (gS_.handy || gS_.arcade || script_.typ != 1) && (gS_.handy || gS_.arcade || script_.typ != 2)) || !script_.isUnlocked || mS_.arrayPlatformsScripts[num2].IsProConsoleInDev() || gS_.retro || script_.vomMarktGenommen || (gS_.gameTyp != 0 && (gS_.gameTyp != 1 || !script_.internet) && (gS_.gameTyp != 2 || !script_.internet)) || !SelectPlayerPlatform(script_, gS_, forceContractGame_, searchSubventionPlatform: true))
					{
						continue;
					}
					platformScript2 = script_;
					if (!script_.OwnerIsNPC())
					{
						gS_.gamePlatform[num] = script_.myID;
						gS_.gamePlatformScript[num] = script_;
						gS_.costs_entwicklung += script_.dev_costs;
						script_.SellPlayerKonsoleToNPC(this);
						if (SETTING_SUBVENTION && SETTING_SUBVENTION_EXKLUSIV)
						{
							gS_.exklusiv = true;
							gS_.gamePlatform[0] = script_.myID;
							gS_.gamePlatformScript[0] = script_;
							gS_.gamePlatform[1] = 0;
							gS_.gamePlatformScript[1] = null;
							gS_.gamePlatform[2] = 0;
							gS_.gamePlatformScript[2] = null;
							gS_.gamePlatform[3] = 0;
							gS_.gamePlatformScript[3] = null;
						}
						if (gS_.exklusiv)
						{
							ClearPlatforms(gS_);
							return;
						}
						num++;
						if (num >= 4)
						{
							ClearPlatforms(gS_);
							return;
						}
					}
				}
			}
			for (int num3 = 0; num3 < platformList.Count; num3++)
			{
				platformScript script_2 = platformList[num3].script_;
				if (!script_2 || gS_.gamePlatform[0] == script_2.myID || gS_.gamePlatform[1] == script_2.myID || gS_.gamePlatform[2] == script_2.myID || gS_.gamePlatform[3] == script_2.myID || ((!gS_.handy || script_2.typ != 3) && (!gS_.arcade || script_2.typ != 4) && (gS_.handy || gS_.arcade || script_2.typ != 0) && (gS_.handy || gS_.arcade || script_2.typ != 1) && (gS_.handy || gS_.arcade || script_2.typ != 2)) || !script_2.isUnlocked || mS_.arrayPlatformsScripts[num3].IsProConsoleInDev() || ((!gS_.retro || !script_2.vomMarktGenommen) && (gS_.retro || script_2.vomMarktGenommen)) || (gS_.gameTyp != 0 && (gS_.gameTyp != 1 || !script_2.internet) && (gS_.gameTyp != 2 || !script_2.internet)) || (!script_2.OwnerIsNPC() && !SelectPlayerPlatform(script_2, gS_, forceContractGame_, searchSubventionPlatform: false)))
				{
					continue;
				}
				platformScript2 = script_2;
				bool flag2 = false;
				if (ownPlatform && script_2.ownerID == myID)
				{
					flag2 = true;
				}
				if (Random.Range(0, 100) > 60 || flag2)
				{
					gS_.gamePlatform[num] = script_2.myID;
					gS_.gamePlatformScript[num] = script_2;
					gS_.costs_entwicklung += script_2.dev_costs;
					if (!script_2.OwnerIsNPC())
					{
						script_2.SellPlayerKonsoleToNPC(this);
					}
					if (gS_.exklusiv)
					{
						ClearPlatforms(gS_);
						return;
					}
					num++;
					if (num >= 4)
					{
						ClearPlatforms(gS_);
						return;
					}
				}
			}
		}
		if (num == 0 && (bool)platformScript2)
		{
			gS_.gamePlatform[0] = platformScript2.myID;
			if (!platformScript2.OwnerIsNPC())
			{
				platformScript2.SellPlayerKonsoleToNPC(this);
			}
		}
		ClearPlatforms(gS_);
	}

	private bool SelectPlayerPlatform(platformScript script_, gameScript gameS_, bool forceContractGame_, bool searchSubventionPlatform)
	{
		if (script_.OwnerIsNPC())
		{
			return false;
		}
		if (!script_.thridPartyGames)
		{
			return false;
		}
		bool flag = false;
		if (script_.publisherBuyed.Length != 0 && script_.publisherBuyed[myID])
		{
			flag = true;
		}
		if (searchSubventionPlatform)
		{
			if (Random.Range(0, 100) < 30 && !forceContractGame_ && !ownPlatform && !IsTochterfirma() && !script_.vomMarktGenommen && script_.subventionMoney > 0 && script_.subventionGameSize[gameS_.gameSize] && !script_.IsOutdatet() && Random.Range(0, platforms_.GetAmountOfPlatformsWithSubvention()) == 0 && (script_.subventionMoney > script_.price || flag))
			{
				int devCostsSubvention = mS_.GetDevCostsSubvention(gameS_.gameSize);
				devCostsSubvention = Random.Range(devCostsSubvention / 2, devCostsSubvention);
				if (mS_.myID == script_.ownerID)
				{
					if (mS_.money >= devCostsSubvention && script_.subventionMoney >= devCostsSubvention)
					{
						SETTING_SUBVENTION = true;
						SETTING_SUBVENTION_EXKLUSIV = true;
						script_.subventionMoney -= devCostsSubvention;
						if (script_.subventionMoney < 0)
						{
							script_.subventionMoney = 0;
						}
						script_.costs_subvention += devCostsSubvention;
						gameS_.subvention = devCostsSubvention;
						mS_.Pay(devCostsSubvention, 33);
						if (mS_.newsSetting[10])
						{
							string text = tS_.GetText(2383);
							text = text.Replace("<NAME1>", GetName());
							text = text.Replace("<NAME2>", gameS_.GetNameWithTag());
							text = text.Replace("<NAME3>", script_.GetName());
							guiMain_.CreateTopNewsGameSubvention(text);
						}
						Debug.Log("Subvention: " + gameS_.myID + ", " + devCostsSubvention);
						return true;
					}
				}
				else if (mS_.multiplayer && script_.PlatformFromMitspieler() && mS_.mpCalls_.GetMoney(script_.ownerID) >= devCostsSubvention && script_.subventionMoney >= devCostsSubvention)
				{
					SETTING_SUBVENTION = true;
					SETTING_SUBVENTION_EXKLUSIV = true;
					script_.subventionMoney -= devCostsSubvention;
					if (script_.subventionMoney < 0)
					{
						script_.subventionMoney = 0;
					}
					gameS_.subvention = devCostsSubvention;
					if (mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_PlatformSubvention(script_, gameS_, devCostsSubvention);
					}
					_ = mS_.mpCalls_.isClient;
					return true;
				}
			}
			return false;
		}
		if (!flag && Random.Range(0, script_.price) > Random.Range(0, platforms_.GetDurchschnittsDevKitPreis()))
		{
			return false;
		}
		if (Random.Range(0f, 100f + script_.GetMarktanteil()) > 60f || script_.GetMarktanteil() > 30f)
		{
			return true;
		}
		return false;
	}

	private void ClearPlatforms(gameScript gS_)
	{
		for (int i = 1; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] == 0)
			{
				gS_.gamePlatform[i] = -1;
				gS_.gamePlatformScript[i] = null;
			}
		}
		if (gS_.arcade)
		{
			for (int j = 1; j < gS_.gamePlatform.Length; j++)
			{
				gS_.gamePlatform[j] = -1;
				gS_.gamePlatformScript[j] = null;
			}
		}
		if (!gS_.handy && !gS_.arcade && !gS_.retro && gS_.gamePlatform[1] == 0)
		{
			gS_.exklusiv = true;
		}
	}

	private void SetStudioAufwertungen(gameScript script_)
	{
		if (mS_.year > 1977)
		{
			for (int i = 0; i < script_.gameplayStudio.Length; i++)
			{
				if (Random.Range(stars * 0.3f, 100f) > 50f || script_.sonderIP)
				{
					script_.gameplayStudio[i] = true;
				}
			}
		}
		if (mS_.year > 1983)
		{
			for (int j = 0; j < script_.grafikStudio.Length; j++)
			{
				if (Random.Range(stars * 0.3f, 100f) > 50f || script_.sonderIP)
				{
					script_.grafikStudio[j] = true;
				}
			}
		}
		if (mS_.year > 1980)
		{
			for (int k = 0; k < script_.soundStudio.Length; k++)
			{
				if (Random.Range(stars * 0.3f, 100f) > 50f || script_.sonderIP)
				{
					script_.soundStudio[k] = true;
				}
			}
		}
		if (unlock_.Get(8) && (Random.Range(stars * 0.3f, 100f) > 50f || script_.sonderIP))
		{
			for (int l = 0; l < script_.motionCaptureStudio.Length; l++)
			{
				script_.motionCaptureStudio[l] = true;
			}
		}
	}

	private void SetLanguages(gameScript script_)
	{
		script_.gameLanguage[0] = true;
		for (int i = 0; i < script_.gameLanguage.Length; i++)
		{
			if (Random.Range(0, 100) > 70 || script_.sonderIP)
			{
				script_.gameLanguage[i] = true;
				script_.costs_entwicklung += 5000L;
			}
		}
	}

	private void SetPoints(gameScript script_)
	{
		float num = 0f;
		switch (mS_.difficulty)
		{
		case 0:
			num = 7000f * games_.GetReviewCurve();
			break;
		case 1:
			num = 10000f * games_.GetReviewCurve();
			break;
		case 2:
			num = 15000f * games_.GetReviewCurve();
			break;
		case 3:
			num = 18000f * games_.GetReviewCurve();
			break;
		case 4:
			num = 22000f * games_.GetReviewCurve();
			break;
		case 5:
			num = 30000f * games_.GetReviewCurve();
			break;
		}
		script_.points_gameplay = num * 1f;
		script_.points_grafik = num * 1f;
		script_.points_sound = num * 1f;
		script_.points_technik = num * 1f;
		float num2 = stars * 0.01f;
		for (int i = 0; i < script_.gameGameplayFeatures.Length; i++)
		{
			if (script_.gameGameplayFeatures[i])
			{
				script_.points_gameplay += gF_.gameplayFeatures_GAMEPLAY[i];
				script_.points_grafik += gF_.gameplayFeatures_GRAPHIC[i];
				script_.points_sound += gF_.gameplayFeatures_SOUND[i];
				script_.points_technik += gF_.gameplayFeatures_TECHNIK[i];
			}
		}
		script_.points_gameplay = Random.Range(num2 * script_.points_gameplay, script_.points_gameplay);
		script_.points_grafik = Random.Range(num2 * script_.points_grafik, script_.points_grafik);
		script_.points_sound = Random.Range(num2 * script_.points_sound, script_.points_sound);
		script_.points_technik = Random.Range(num2 * script_.points_technik, script_.points_technik);
		if (IsTochterfirma())
		{
			switch (tf_entwicklungsdauer)
			{
			case 0:
				script_.points_gameplay *= 0.5f;
				script_.points_grafik *= 0.5f;
				script_.points_sound *= 0.5f;
				script_.points_technik *= 0.5f;
				break;
			case 1:
				script_.points_gameplay *= 0.7f;
				script_.points_grafik *= 0.7f;
				script_.points_sound *= 0.7f;
				script_.points_technik *= 0.7f;
				break;
			}
		}
		if (Random.Range(0, 100) > 70 && stars < 70f && !script_.sonderIP && !IsTochterfirma())
		{
			script_.points_gameplay *= 0.3f;
			script_.points_grafik *= 0.3f;
			script_.points_sound *= 0.3f;
			script_.points_technik *= 0.3f;
		}
		script_.points_bugs = 0f;
		if (publisher && !script_.sonderIP && !IsTochterfirma() && !script_.typ_addon && !script_.typ_addonStandalone && !script_.typ_budget && !script_.typ_bundle && !script_.typ_bundleAddon && !script_.typ_goty && !script_.typ_mmoaddon && ((!mS_.badGameThisYear && Random.Range(0, 100) > 70) || (!mS_.badGameThisYear && mS_.month > 3)))
		{
			mS_.badGameThisYear = true;
			script_.points_bugs = Random.Range(250, 500);
			script_.points_gameplay *= 0.1f;
			script_.points_grafik *= 0.1f;
			script_.points_sound *= 0.1f;
			script_.points_technik *= 0.1f;
		}
		if (script_.points_gameplay > 99999f)
		{
			script_.points_gameplay = 99999f;
		}
		if (script_.points_grafik > 99999f)
		{
			script_.points_grafik = 99999f;
		}
		if (script_.points_sound > 99999f)
		{
			script_.points_sound = 99999f;
		}
		if (script_.points_technik > 99999f)
		{
			script_.points_technik = 99999f;
		}
		if (script_.points_gameplay < 1f)
		{
			script_.points_gameplay = 1f;
		}
		if (script_.points_grafik < 1f)
		{
			script_.points_grafik = 1f;
		}
		if (script_.points_sound < 1f)
		{
			script_.points_sound = 1f;
		}
		if (script_.points_technik < 1f)
		{
			script_.points_technik = 1f;
		}
	}

	private void SetDesignSlider(gameScript script_)
	{
		if (!script_.sonderIP)
		{
			for (int i = 0; i < script_.Designschwerpunkt.Length; i++)
			{
				script_.Designschwerpunkt[i] = 0;
			}
			int num = 0;
			for (int j = 0; j < 10000; j++)
			{
				int num2 = Random.Range(0, script_.Designschwerpunkt.Length);
				if (script_.Designschwerpunkt[num2] < 10)
				{
					script_.Designschwerpunkt[num2]++;
					num++;
				}
				if (num >= 40)
				{
					break;
				}
				if (j >= 9999)
				{
					Debug.Log("NOTAUSSTIEG! " + script_.GetNameSimple() + " / " + script_.myID);
					break;
				}
			}
			for (int k = 0; k < script_.Designausrichtung.Length; k++)
			{
				script_.Designausrichtung[k] = Random.Range(0, 11);
			}
		}
		else
		{
			for (int l = 0; l < script_.Designschwerpunkt.Length; l++)
			{
				script_.Designschwerpunkt[l] = genres_.genres_FOCUS[script_.maingenre, l];
			}
			for (int m = 0; m < script_.Designausrichtung.Length; m++)
			{
				script_.Designausrichtung[m] = Random.Range(0, 11);
			}
		}
	}

	private void SetGenre(gameScript script_)
	{
		script_.maingenre = -1;
		script_.subgenre = -1;
		int num = 0;
		bool flag = false;
		while (!flag)
		{
			int num2 = Random.Range(0, genres_.genres_UNLOCK.Length);
			if (genres_.genres_UNLOCK[num2])
			{
				script_.maingenre = num2;
				flag = true;
				break;
			}
			num++;
			if (num > 10000)
			{
				flag = true;
				break;
			}
		}
		if (genres_.genres_UNLOCK[fanGenre] && Random.Range(0, 100) > 70)
		{
			script_.maingenre = fanGenre;
		}
		if (IsTochterfirma() && tf_gameGenre != 0 && genres_.genres_UNLOCK[tf_gameGenre - 1])
		{
			script_.maingenre = tf_gameGenre - 1;
		}
		if (Random.Range(0, 100) <= 30)
		{
			return;
		}
		num = 0;
		flag = false;
		while (!flag)
		{
			int num3 = Random.Range(0, genres_.genres_UNLOCK.Length);
			if (num3 != script_.maingenre && genres_.genres_UNLOCK[num3])
			{
				script_.subgenre = num3;
				flag = true;
				break;
			}
			num++;
			if (num > 10000)
			{
				flag = true;
				break;
			}
		}
	}

	private void SetCopyProtect(gameScript script_)
	{
		if (script_.arcade)
		{
			script_.gameCopyProtect = -1;
			return;
		}
		float num = 0f;
		int num2 = -1;
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				copyProtectScript component = array[i].GetComponent<copyProtectScript>();
				if ((bool)component && component.isUnlocked && component.effekt > num)
				{
					num2 = component.myID;
					num = component.effekt;
				}
			}
		}
		if (num2 != -1)
		{
			script_.gameCopyProtect = num2;
		}
	}

	private void SetAntiCheat(gameScript script_)
	{
		if (script_.arcade)
		{
			script_.gameAntiCheat = -1;
			return;
		}
		float num = 0f;
		int num2 = -1;
		GameObject[] array = GameObject.FindGameObjectsWithTag("AntiCheat");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				antiCheatScript component = array[i].GetComponent<antiCheatScript>();
				if ((bool)component && component.isUnlocked && component.effekt > num)
				{
					num2 = component.myID;
					num = component.effekt;
				}
			}
		}
		if (num2 != -1)
		{
			script_.gameAntiCheat = num2;
		}
	}

	private void SetTheme(gameScript script_)
	{
		for (int i = 0; i < 500; i++)
		{
			script_.gameMainTheme = Random.Range(0, themes_.themes_LEVEL.Length);
			if (themes_.IsThemesFitWithGenre(script_.gameMainTheme, script_.maingenre))
			{
				break;
			}
		}
		if (IsTochterfirma() && tf_gameTopic != -1)
		{
			script_.gameMainTheme = tf_gameTopic;
		}
		if (Random.Range(0, 100) > 50)
		{
			for (int j = 0; j < 500; j++)
			{
				script_.gameSubTheme = Random.Range(0, themes_.themes_LEVEL.Length);
				if (themes_.IsThemesFitWithGenre(script_.gameSubTheme, script_.maingenre))
				{
					break;
				}
			}
			if (script_.gameMainTheme == script_.gameSubTheme)
			{
				script_.gameSubTheme = -1;
			}
		}
		else
		{
			script_.gameSubTheme = -1;
		}
	}

	private void SetLicence(gameScript script_)
	{
		if (Random.Range(0, 100) < 5)
		{
			script_.gameLicence = Random.Range(0, licences_.licence_QUALITY.Length);
		}
	}

	private bool IpFokusCheck(gameScript script_)
	{
		if (!IsTochterfirma())
		{
			return true;
		}
		if (tf_ipFocus[0] == -1 && tf_ipFocus[1] == -1 && tf_ipFocus[2] == -1 && tf_ipFocus[3] == -1 && tf_ipFocus[4] == -1 && tf_ipFocus[5] == -1)
		{
			return true;
		}
		for (int i = 0; i < tf_ipFocus.Length; i++)
		{
			if (script_.mainIP == tf_ipFocus[i])
			{
				return true;
			}
		}
		return false;
	}

	private gameScript GetSpinoff()
	{
		List<gameScript> list = new List<gameScript>();
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].nachfolger_created && !games_.arrayGamesScripts[i].noSpinOff && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && games_.arrayGamesScripts[i].typ_standard && games_.arrayGamesScripts[i].mainIP == games_.arrayGamesScripts[i].myID)
			{
				list.Add(games_.arrayGamesScripts[i]);
				if (games_.arrayGamesScripts[i].reviewTotal > 70)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 80)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 85)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 90)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
			}
		}
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		return null;
	}

	private gameScript GetGameForNachfolger()
	{
		List<gameScript> list = new List<gameScript>();
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].nachfolger_created && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].date_start_year != mS_.year && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff))
			{
				list.Add(games_.arrayGamesScripts[i]);
				if (games_.arrayGamesScripts[i].reviewTotal > 70)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 80)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 85)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].reviewTotal > 90)
				{
					list.Add(games_.arrayGamesScripts[i]);
				}
				if (games_.arrayGamesScripts[i].sonderIP)
				{
					list.Add(games_.arrayGamesScripts[i]);
					list.Add(games_.arrayGamesScripts[i]);
					list.Add(games_.arrayGamesScripts[i]);
					list.Add(games_.arrayGamesScripts[i]);
				}
			}
		}
		if (list.Count > 0)
		{
			return list[Random.Range(0, list.Count)];
		}
		return null;
	}

	private gameScript GetRemaster()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].remaster_created && mS_.year - games_.arrayGamesScripts[i].date_year >= 3 && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetGOTY()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].goty_created && games_.arrayGamesScripts[i].goty && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade && IstEinePlattformAufDemMarkt(games_.arrayGamesScripts[i]))
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetPortForHandy()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].portExist[1] && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetPortForArcade()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].portExist[2] && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetPortForPC()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].portExist[0] && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetGameForAddonBundle(bool hauptgame_, int hauptgameID_)
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i] || !games_.arrayGamesScripts[i].IsMyIP(this) || !IpFokusCheck(games_.arrayGamesScripts[i]))
			{
				continue;
			}
			if (hauptgame_)
			{
				if (!games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].isOnMarket && !games_.arrayGamesScripts[i].pubOffer && !games_.arrayGamesScripts[i].schublade && games_.arrayGamesScripts[i].amountAddons > 0 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].typ_mmoaddon && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_addonStandalone && games_.arrayGamesScripts[i].sellsTotal > 10 && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade && HasUnusedAddons(games_.arrayGamesScripts[i].myID))
				{
					return games_.arrayGamesScripts[i];
				}
			}
			else if (games_.arrayGamesScripts[i].ownerID == myID && games_.arrayGamesScripts[i].developerID == myID && !games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].bundle_created && (games_.arrayGamesScripts[i].typ_addon || games_.arrayGamesScripts[i].typ_addonStandalone) && games_.arrayGamesScripts[i].originalGameID == hauptgameID_ && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private bool HasUnusedAddons(int gameID)
	{
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = mS_.games_.arrayGamesScripts[i];
			if (gameScript2.ownerID == myID && gameScript2.developerID == myID && !gameScript2.inDevelopment && !gameScript2.isOnMarket && !gameScript2.bundle_created && (gameScript2.typ_addon || gameScript2.typ_addonStandalone) && gameScript2.originalGameID == gameID)
			{
				return true;
			}
		}
		return false;
	}

	private gameScript GetGameForBundle()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].bundle_created && !games_.arrayGamesScripts[i].pubOffer && !games_.arrayGamesScripts[i].schublade && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].typ_mmoaddon && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_addonStandalone && games_.arrayGamesScripts[i].sellsTotal > 10 && mS_.year - games_.arrayGamesScripts[i].date_year >= 2 && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetGameForBudget()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && !games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].sellsTotal > 1000 && !games_.arrayGamesScripts[i].budget_created && mS_.year - games_.arrayGamesScripts[i].date_year >= 2 && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade && (IstEinePlattformAufDemMarkt(games_.arrayGamesScripts[i]) || !IsTochterfirma()))
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetAddon()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 0 && games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].sellsTotal > 1000 && games_.arrayGamesScripts[i].amountAddons <= 0 && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private gameScript GetMMOAddon()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(this) && IpFokusCheck(games_.arrayGamesScripts[i]) && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].portID == -1 && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel && games_.arrayGamesScripts[i].gameTyp == 1 && games_.arrayGamesScripts[i].isOnMarket && games_.arrayGamesScripts[i].sellsTotal > 1000 && games_.arrayGamesScripts[i].amountAddons <= 0 && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].f2pConverted && (games_.arrayGamesScripts[i].typ_standard || games_.arrayGamesScripts[i].typ_nachfolger || games_.arrayGamesScripts[i].typ_spinoff || games_.arrayGamesScripts[i].typ_remaster) && !games_.arrayGamesScripts[i].handy && !games_.arrayGamesScripts[i].arcade)
			{
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	private bool IstEinePlattformAufDemMarkt(gameScript script_)
	{
		script_.FindMyPlatforms();
		for (int i = 0; i < script_.gamePlatformScript.Length; i++)
		{
			if ((bool)script_.gamePlatformScript[i] && script_.gamePlatformScript[i].IsVerfuegbar())
			{
				return true;
			}
		}
		return false;
	}

	public void UpdateRandomData()
	{
		if (!isUnlocked)
		{
			return;
		}
		if (lockToBuy > 0)
		{
			lockToBuy--;
			if (lockToBuy < 0)
			{
				lockToBuy = 0;
			}
		}
		if (Random.Range(0, 20) != 1)
		{
			return;
		}
		exklusivLaufzeit = 12 * Random.Range(2, 11);
		if (!IsTochterfirma())
		{
			int num = Random.Range(0, genres_.genres_LEVEL.Length);
			if (genres_.genres_UNLOCK[num])
			{
				fanGenre = num;
			}
		}
		else if (tf_gameGenre != 0)
		{
			fanGenre = tf_gameGenre - 1;
		}
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			mS_.mpCalls_.SERVER_Send_Publisher(this);
		}
	}

	public bool Geschlossen()
	{
		if (tf_geschlossen)
		{
			return true;
		}
		return false;
	}

	public bool TochterfirmaGeschlossen()
	{
		if (!IsTochterfirma())
		{
			return false;
		}
		if (IsTochterfirma() && !tf_geschlossen)
		{
			return false;
		}
		if (IsTochterfirma() && tf_geschlossen)
		{
			return true;
		}
		return false;
	}

	public int GetMoneyExklusiv()
	{
		return Mathf.RoundToInt((float)((mS_.year - 1975) * 250000) * (stars / 100f) / 120f * (float)exklusivLaufzeit);
	}

	public int GetShareExklusiv()
	{
		return Mathf.RoundToInt(share * 1.5f);
	}

	public int GetShare()
	{
		if (IsMyTochterfirma())
		{
			return 20;
		}
		return Mathf.RoundToInt(share);
	}

	public string GetDeveloperPublisherString()
	{
		if (developer && !publisher)
		{
			return tS_.GetText(274);
		}
		if (!developer && publisher)
		{
			return tS_.GetText(432);
		}
		if (developer && publisher)
		{
			return tS_.GetText(432) + " & " + tS_.GetText(274);
		}
		return "";
	}

	public int GetStarsAmount()
	{
		return Mathf.RoundToInt(stars / 20f);
	}

	public string GetOwnerString()
	{
		FindScripts();
		if ((bool)mS_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == ownerID)
				{
					return mS_.arrayPublisherScripts[i].GetName();
				}
			}
		}
		return "";
	}

	public string GetTooltip()
	{
		if (!mS_)
		{
			FindScripts();
		}
		string text = "<b><size=18>" + GetName() + "</size></b>";
		if (isPlayer)
		{
			text = text + "\n<b><size=15><color=green>" + tS_.GetText(2004) + "</color></size></b>";
		}
		if (IsMyTochterfirma())
		{
			text = text + "\n<b><size=15><color=green>" + tS_.GetText(1924) + "</color></size></b>";
			text = text + "\n<b><color=green>[" + GetOwnerString() + "]</color></b>";
		}
		if (mS_.multiplayer && IsTochterfirmaVonMitspieler())
		{
			text = text + "\n<b><size=15><color=red>" + tS_.GetText(1924) + "</color></size></b>";
			text = text + "\n<b><color=red>[" + GetOwnerString() + "]</color></b>";
		}
		text = text + "\n<b>" + GetDeveloperPublisherString() + "</b>";
		text = text + "\n<b>" + tS_.GetCountry(country) + "</b>";
		text = text + "\n<b>" + GetDateString() + "</b>";
		if (publisher)
		{
			text = text + "\n\n" + tS_.GetText(434) + ": <color=blue><size=20>";
			for (int i = 0; i < Mathf.RoundToInt(stars / 20f); i++)
			{
				text += "★";
			}
			text += "</size></color>";
			text = text + "\n" + tS_.GetText(437) + ": <color=blue>" + genres_.GetName(fanGenre) + "</color>";
			text = text + "\n" + tS_.GetText(271) + ": <color=blue>" + mS_.GetMoney(Mathf.RoundToInt(GetAmountGames()), showDollar: false) + "</color>";
			text = text + "\n" + tS_.GetText(1745) + ": <color=blue>" + mS_.GetMoney(Mathf.RoundToInt(GetAmountVertriebeneSpiele()), showDollar: false) + "</color>";
			text = text + "\n" + tS_.GetText(435) + ": <color=blue>" + mS_.Round(GetRelation(), 2) + "</color>";
			if (!IsMyTochterfirma())
			{
				if (!isPlayer)
				{
					text = text + "\n" + tS_.GetText(436) + ": <color=blue>" + mS_.GetMoney(Mathf.RoundToInt(share), showDollar: true) + "</color>";
				}
			}
			else
			{
				text = text + "\n" + tS_.GetText(436) + ": <color=green><b>" + mS_.GetMoney(GetShare(), showDollar: true) + "</b></color><color=blue> [" + mS_.GetMoney(Mathf.RoundToInt(share), showDollar: true) + "]</color>";
			}
		}
		else
		{
			text = text + "\n\n" + tS_.GetText(434) + ": <color=blue><size=20>";
			for (int j = 0; j < Mathf.RoundToInt(stars / 20f); j++)
			{
				text += "★";
			}
			text += "</size></color>";
			text = text + "\n" + tS_.GetText(271) + ": <color=blue>" + mS_.GetMoney(Mathf.RoundToInt(GetAmountGames()), showDollar: false) + "</color>";
		}
		if (IsMyTochterfirma())
		{
			text = text + "\n\n" + tS_.GetText(1930) + ": <color=green><b>" + mS_.GetMoney(tf_umsatz_allTime, showDollar: true) + "</b></color>";
			text = text + "\n" + tS_.GetText(698) + ": <color=green><b>" + mS_.GetMoney(GetTochterfirmaUmsatz24Monate(), showDollar: true) + "</b></color>";
			text = text + "\n" + tS_.GetText(1934) + ": <color=red><b>" + mS_.GetMoney(GetVerwaltungskosten(), showDollar: true) + "</b></color>";
		}
		if (Geschlossen())
		{
			text = text + "\n\n<b><color=red>" + tS_.GetText(1932) + "</color></b>";
		}
		return text;
	}

	public string GetDateString()
	{
		return tS_.GetText(date_month - 1 + 221) + " " + date_year;
	}

	public int GetAmountGames()
	{
		int num = 0;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].developerID == myID && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAmountVertriebeneSpiele()
	{
		int num = 0;
		if ((bool)games_)
		{
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if ((bool)games_.arrayGamesScripts[i] && !games_.arrayGamesScripts[i].inDevelopment && games_.arrayGamesScripts[i].publisherID == myID && games_.arrayGamesScripts[i].ownerID != myID && !games_.arrayGamesScripts[i].pubAngebot && !games_.arrayGamesScripts[i].auftragsspiel)
				{
					num++;
				}
			}
		}
		return num;
	}

	public void ResetDataForAuftragsspiel(gameScript script_)
	{
		if (script_.gameMainTheme == -1)
		{
			SetTheme(script_);
		}
		SetDesignSlider(script_);
		SetLanguages(script_);
		SetStudioAufwertungen(script_);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMGSR_Result(script_, script_.Designausrichtung[1]);
		script_.FindMyPlatforms();
		if ((bool)script_.gamePlatformScript[0])
		{
			if (!script_.gamePlatformScript[0].isUnlocked)
			{
				FindPlatformsForGameNew(script_, forceContractGame_: false);
			}
			if (script_.gamePlatformScript[0].units <= 0)
			{
				FindPlatformsForGameNew(script_, forceContractGame_: false);
			}
		}
		else
		{
			FindPlatformsForGameNew(script_, forceContractGame_: false);
		}
		FindEngineForGameNew(script_);
		SetGameplayFeatures(script_);
		SetPoints(script_);
	}

	private IEnumerator iWaitTochterfirmaReleaseGame(gameScript s1_, publisherScript s2_)
	{
		bool done = false;
		while (!done)
		{
			if ((bool)s1_ && !guiMain_.uiObjects[397].activeSelf)
			{
				done = true;
				guiMain_.OpenMenu(hideChars: false);
				guiMain_.ActivateMenu(guiMain_.uiObjects[397]);
				guiMain_.uiObjects[397].GetComponent<Menu_Dev_TochterfirmaGameComplete>().Init(s1_, s2_);
			}
			yield return null;
		}
	}

	public void AddTochterfirmaUmsatz(long l)
	{
		tf_umsatz[0] += l;
		tf_umsatz_allTime += l;
	}

	public void UpdateTochterfirmaUmsatzVerlauf()
	{
		for (int num = tf_umsatz.Length - 1; num > 0; num--)
		{
			tf_umsatz[num] = tf_umsatz[num - 1];
		}
		tf_umsatz[0] = 0L;
	}

	public long GetTochterfirmaUmsatz24Monate()
	{
		long num = 0L;
		for (int i = 0; i < tf_umsatz.Length; i++)
		{
			num += tf_umsatz[i];
		}
		return num;
	}

	public float GetEntwicklungsFortschritt()
	{
		float num = newGameInWeeksORG;
		if (num <= (float)newGameInWeeks)
		{
			num = newGameInWeeks;
		}
		num = 100f / num;
		num = 100f - num * (float)newGameInWeeks;
		if (newGameInWeeks <= 2)
		{
			return 1f;
		}
		return num * 0.01f;
	}

	public bool CloseFree()
	{
		if (IsTochterfirma())
		{
			return false;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].publisherID == myID && (games_.arrayGamesScripts[i].auftragsspiel || games_.arrayGamesScripts[i].pubAngebot))
			{
				return false;
			}
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].publisherID == myID && games_.arrayGamesScripts[i].isOnMarket && !games_.arrayGamesScripts[i].DeveloperIsNPC())
			{
				return false;
			}
		}
		return true;
	}

	public bool KaufangebotFree()
	{
		if (notForSale)
		{
			return false;
		}
		if (IsTochterfirma())
		{
			return false;
		}
		if (lockToBuy > 0)
		{
			return false;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].publisherID == myID && (games_.arrayGamesScripts[i].auftragsspiel || games_.arrayGamesScripts[i].pubAngebot))
			{
				return false;
			}
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].publisherID == myID && games_.arrayGamesScripts[i].isOnMarket && !games_.arrayGamesScripts[i].DeveloperIsNPC())
			{
				return false;
			}
		}
		return true;
	}
}
