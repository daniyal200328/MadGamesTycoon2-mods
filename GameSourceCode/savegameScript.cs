using System;
using System.Collections;
using UnityEngine;

public class savegameScript : MonoBehaviour
{
	public bool loadingSavegame;

	private ES3Writer writer;

	private ES3Reader reader;

	private GameObject main_;

	private settingsScript settings_;

	private mainScript mS_;

	private textScript tS_;

	private sfxScript sfx_;

	private mapScript mapS_;

	private buildRoomScript brS_;

	private games games_;

	private GUI_Main guiMain_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private publisher publisher_;

	private platforms platforms_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private copyProtect copyProtect_;

	private anitCheat antiCheat_;

	private arbeitsmarkt arbeitsmarkt_;

	private createCharScript cCS_;

	private gamepassScript gpS_;

	private forschungSonstiges fS_;

	private contractWorkMain contractWorkMain_;

	private publishingOfferMain publishingOfferMain_;

	private Menu_Packung menuPackung_;

	private Menu_ArcadePreis menuArcadePreis_;

	private Menu_BuyInventar menu_BuyInventar_;

	private mpMain mpMain_;

	public mpCalls mpCalls_;

	public int saveGameVersion = 17;

	private ES3File es3file;

	private bool key_merchStandardpreis;

	private bool key_NpcIPs;

	private bool key_inventarFilter;

	private bool key_fanshopverlauf;

	private bool key_achivements;

	private bool key_gameTabFilter;

	private bool key_npcGameNameInUse;

	private bool key_default_verkaufpreis;

	private bool key_default_verkaufpreisBundle;

	private bool key_legends;

	private bool key_legends_new;

	private bool key_licence_YEAR;

	private bool key_gamePass_aboVerlaufWoche;

	private bool key_gamePass_aboVerlaufMonat;

	private bool key_gamePass_umsatzVerlaufMonat;

	private bool key_themes_USES;

	private bool key_gameplayFeatures_NEED_GAMEPLAY_FEATURE;

	private bool key_genres_SUC_YEAR;

	private bool key_genres_PLATFORM_SELLS;

	private bool key_EN;

	private bool key_GE;

	private bool key_TU;

	private bool key_CH;

	private bool key_FR;

	private bool key_PB;

	private bool key_HU;

	private bool key_CT;

	private bool key_ES;

	private bool key_PL;

	private bool key_CZ;

	private bool key_KO;

	private bool key_AR;

	private bool key_RU;

	private bool key_IT;

	private bool key_JA;

	private bool key_UA;

	private bool key_TH;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = base.gameObject;
		}
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = GetComponent<textScript>();
		}
		if (!mapS_)
		{
			mapS_ = GetComponent<mapScript>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
		if (!brS_)
		{
			brS_ = GetComponent<buildRoomScript>();
		}
		if (!games_)
		{
			games_ = GetComponent<games>();
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
			genres_ = GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = GetComponent<gameplayFeatures>();
		}
		if (!publisher_)
		{
			publisher_ = GetComponent<publisher>();
		}
		if (!hardware_)
		{
			hardware_ = GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = GetComponent<hardwareFeatures>();
		}
		if (!platforms_)
		{
			platforms_ = GetComponent<platforms>();
		}
		if (!copyProtect_)
		{
			copyProtect_ = GetComponent<copyProtect>();
		}
		if (!antiCheat_)
		{
			antiCheat_ = GetComponent<anitCheat>();
		}
		if (!arbeitsmarkt_)
		{
			arbeitsmarkt_ = GetComponent<arbeitsmarkt>();
		}
		if (!cCS_)
		{
			cCS_ = GetComponent<createCharScript>();
		}
		if (!fS_)
		{
			fS_ = GetComponent<forschungSonstiges>();
		}
		if (!gpS_)
		{
			gpS_ = GetComponent<gamepassScript>();
		}
		if (!menuPackung_)
		{
			menuPackung_ = guiMain_.uiObjects[218].GetComponent<Menu_Packung>();
		}
		if (!menuArcadePreis_)
		{
			menuArcadePreis_ = guiMain_.uiObjects[307].GetComponent<Menu_ArcadePreis>();
		}
		if (!menu_BuyInventar_)
		{
			menu_BuyInventar_ = guiMain_.uiObjects[20].GetComponent<Menu_BuyInventar>();
		}
		if (!mpMain_)
		{
			mpMain_ = GetComponent<mpMain>();
		}
		if (!contractWorkMain_)
		{
			contractWorkMain_ = GetComponent<contractWorkMain>();
		}
		if (!publishingOfferMain_)
		{
			publishingOfferMain_ = GetComponent<publishingOfferMain>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	public void SaveMultiplayer(int i)
	{
		StartCoroutine(SaveMultiplayerDelay(i));
	}

	public IEnumerator SaveMultiplayerDelay(int i)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (mpCalls_.isServer)
		{
			mS_.SetGameSpeed(0f);
			mpCalls_.SERVER_Send_GameSpeed(Mathf.RoundToInt(0f));
			mpCalls_.SERVER_Send_Command(5);
			mpCalls_.SetPlayersUnready();
			guiMain_.uiObjects[202].SetActive(value: true);
			guiMain_.uiObjects[202].GetComponent<Menu_WaitForPlayers>().SetText(tS_.GetText(2234));
			yield return new WaitForSeconds(10f);
		}
		else
		{
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
			yield return new WaitForEndOfFrame();
		}
		Debug.Log("SaveMultiplayerDelay()");
		Save(i);
	}

	public void Save(int i)
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (mS_.multiplayer)
		{
			if (mpCalls_.isServer)
			{
				mS_.SetRandomMultiplayerSaveID();
				mpCalls_.SERVER_Send_Save(mS_.multiplayerSaveID);
			}
		}
		else
		{
			PlayerPrefs.SetInt("ResumeGame", i);
		}
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		ES3Settings settings = new ES3Settings();
		ES3.DeleteFile(filePath);
		writer = ES3Writer.Create(filePath, settings);
		SaveGlobals(writer);
		SaveNPCGameNames(writer);
		SaveLicences(writer);
		SaveGenres(writer);
		SaveThemes(writer);
		SaveGameplayFeatures(writer);
		SaveEngineFeatures(writer);
		SaveHardware(writer);
		SaveHardwareFeatures(writer);
		SaveRooms(writer);
		SaveArbeitsmarkt(writer);
		SaveObjects(writer);
		SaveMuell(writer);
		SaveMitarbeiter(writer);
		SavePublisher(writer);
		SaveCopyProtect(writer);
		SaveAntiCheat(writer);
		SavePlatforms(writer);
		SaveEngines(writer);
		SaveGames(writer);
		SaveContractWork(writer);
		SaveTasks(writer);
		SaveHistory(writer);
		writer.Save();
		writer.Dispose();
		Debug.Log("Save Complete: " + i);
		if (mS_.multiplayer && mpCalls_.isClient)
		{
			mpCalls_.CLIENT_Send_Command(1);
		}
	}

	public int GetOfficeFromSavegame(int i)
	{
		if (!mS_)
		{
			FindScripts();
		}
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		reader = ES3Reader.Create(filePath);
		if (reader == null)
		{
			guiMain_.MessageBox(tS_.GetText(1226), closeMenu: false);
			Debug.Log("MISSING SAVE FILE!");
			return -1;
		}
		_ = new int[100];
		int[] array = reader.Read<int[]>("globals_int");
		reader.Dispose();
		return array[21];
	}

	public void Load(int i)
	{
		loadingSavegame = true;
		string text = mS_.GetSavegameTitle() + i + ".sav";
		KeysAbfragen(text);
		es3file = new ES3File(text);
		reader = ES3Reader.Create(text);
		LoadGlobals(reader, text);
		LoadNPCGameNames(reader, text);
		LoadGenres(reader, text);
		LoadLicences(reader, text);
		LoadThemes(reader, text);
		LoadGameplayFeatures(reader, text);
		LoadEngineFeatures(reader, text);
		LoadHardware(reader, text);
		LoadHardwareFeatures(reader, text);
		LoadRooms(reader, text);
		LoadArbeitsmarkt(reader, text);
		LoadObjects(reader, text);
		LoadMuell(reader, text);
		LoadMitarbeiter(reader, text);
		LoadPublisher(reader, text);
		LoadCopyProtect(reader, text);
		LoadAntiCheat(reader, text);
		LoadPlatforms(reader, text);
		LoadEngines(reader, text);
		LoadGames(reader, text);
		LoadContractWork(reader, text);
		LoadTasks(reader, text);
		LoadHistory(reader, text);
		mS_.FindRooms();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				array[j].GetComponent<roomScript>().UpdateListInventar();
			}
		}
		games_.LagerplatzVerteilen();
		games_.UpdateChartsWeek();
		platforms_.UpdateGamesForPlatforms();
		gpS_.GAMEPASS_UpdateMarketshare();
		gpS_.GetAmountGamePassGames();
		reader.Dispose();
		es3file.Clear();
		guiMain_.UpdateOnce();
		UpdateCharactersNew();
		mS_.settings_TutorialOff = true;
		if (!mS_.multiplayer)
		{
			loadingSavegame = false;
		}
	}

	private void UpdateCharactersNew()
	{
		mS_.FindCharacters();
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if ((bool)mS_.arrayCharactersScripts[i] && (bool)mS_.arrayCharactersScripts[i].moveS_)
			{
				mS_.arrayCharactersScripts[i].moveS_.InitUpdate();
			}
		}
		mS_.SetGameSpeed(0f);
	}

	public string LoadSaveGameName(int i)
	{
		FindScripts();
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		reader = ES3Reader.Create(filePath);
		if (reader == null)
		{
			return "<Missing File>";
		}
		_ = new long[100];
		int[] array = new int[100];
		_ = new float[100];
		string[] array2 = new string[100];
		array = reader.Read<int[]>("globals_int");
		reader.Read<float[]>("globals_float");
		reader.Read<long[]>("globals_long");
		array2 = reader.Read<string[]>("globals_string");
		string text = tS_.GetText(102) + array[3];
		text = text + " " + tS_.GetText(103) + array[4];
		text = text + " " + tS_.GetText(104) + array[5];
		if (mS_.multiplayer)
		{
			string text2 = "";
			for (int j = 0; j < 4; j++)
			{
				if (reader.Read<int>("MP_playerID" + j) != -1)
				{
					if (j != 0)
					{
						text2 += " ▪ ";
					}
					text2 += reader.Read<string>("MP_playerName" + j);
				}
			}
			reader.Dispose();
			return "<b>" + array2[2] + " ▪ " + text + "</b>\n" + text2;
		}
		reader.Dispose();
		return array2[2] + "\n<b>" + text + " ▪ " + array2[0] + "</b>";
	}

	public bool IsSaveGameOutdatet(int i)
	{
		FindScripts();
		string filePath = mS_.GetSavegameTitle() + i + ".sav";
		reader = ES3Reader.Create(filePath);
		if (reader == null)
		{
			return false;
		}
		_ = new int[100];
		int[] array = reader.Read<int[]>("globals_int");
		reader.Dispose();
		if (array[45] != saveGameVersion)
		{
			return true;
		}
		return false;
	}

	private void SaveHistory(ES3Writer writer)
	{
		string[] array = mS_.history.ToArray();
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Replace("</color>", "</c>");
			array[i] = array[i].Replace("<color=blue>", "<c1>");
			array[i] = array[i].Replace("<color=green>", "<c2>");
			array[i] = array[i].Replace("<color=grey>", "<c3>");
		}
		writer.Write<string[]>("history", array);
	}

	private void LoadHistory(ES3Reader reader, string filename)
	{
		string[] array = reader.Read<string[]>("history");
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = array[i].Replace("</c>", "</color>");
			array[i] = array[i].Replace("<c1>", "<color=blue>");
			array[i] = array[i].Replace("<c2>", "<color=green>");
			array[i] = array[i].Replace("<c3>", "<color=grey>");
			mS_.history.Add(array[i]);
		}
	}

	private void SaveGlobals(ES3Writer writer)
	{
		long[] array = new long[100];
		int[] array2 = new int[100];
		float[] array3 = new float[100];
		string[] array4 = new string[100];
		bool[] array5 = new bool[100];
		array2[2] = 0;
		array2[3] = mS_.year;
		array2[4] = mS_.month;
		array2[5] = mS_.week;
		array2[6] = mS_.trendGenre;
		array2[7] = mS_.trendAntiGenre;
		array2[8] = mS_.trendTheme;
		array2[9] = mS_.trendAntiTheme;
		array2[10] = mS_.trendWeeks;
		array2[11] = mS_.anrufe;
		array2[12] = mS_.goldeneSchallplatten;
		array2[13] = mS_.difficulty;
		array2[14] = mS_.anzKonkurrenten;
		array2[15] = mS_.personal_druck;
		array2[16] = mS_.personal_pausen;
		array2[17] = mS_.personal_motivation;
		array2[18] = mS_.personal_crunch;
		array2[19] = mS_.exklusivVertrag_ID;
		array2[20] = mS_.exklusivVertrag_laufzeit;
		array2[21] = mS_.office;
		array2[22] = mS_.globalEvent;
		array2[23] = mS_.globalEventWeeks;
		array2[24] = mS_.marktforschung_bestPlattform;
		array2[25] = mS_.marktforschung_badPlattform;
		array2[26] = mS_.marktforschung_nextGenre;
		array2[27] = mS_.marktforschung_nextTopic;
		array2[28] = mS_.marktforschung_nextBadGenre;
		array2[29] = mS_.marktforschung_nextBadTopic;
		array2[30] = mS_.trendNextGenre;
		array2[31] = mS_.trendNextAntiGenre;
		array2[32] = mS_.trendNextTheme;
		array2[33] = mS_.trendNextAntiTheme;
		array2[34] = mS_.multiplayerSaveID;
		array2[35] = mS_.bankWarning;
		array2[36] = mS_.lastGamesGenre[0];
		array2[37] = mS_.lastGamesGenre[1];
		array2[38] = mS_.lastGamesGenre[2];
		array2[39] = mS_.lastGamesGenre[3];
		array2[40] = mS_.lastGamesGenre[4];
		array2[41] = mS_.gelangweiltGenre;
		array2[42] = mS_.platinSchallplatten;
		array2[43] = mS_.diamantSchallplatten;
		array2[44] = mS_.sauerBugs;
		if (mS_.savegameVersion <= 0)
		{
			array2[45] = saveGameVersion;
		}
		else
		{
			array2[45] = mS_.savegameVersion;
		}
		array2[46] = mS_.award_GOTY;
		array2[47] = mS_.award_Studio;
		array2[48] = mS_.gameTabs_sort;
		array2[49] = mS_.myID;
		array2[50] = mS_.award_Grafik;
		array2[51] = mS_.award_Sound;
		array2[52] = mS_.award_Trendsetter;
		array2[53] = mS_.studioPoints;
		array2[54] = mS_.award_Publisher;
		array2[55] = mS_.marktforschung_bestPlattformKonsole;
		array2[56] = mS_.marktforschung_badPlattformKonsole;
		array2[57] = mS_.marktforschung_bestPlattformHandheld;
		array2[58] = mS_.marktforschung_badPlattformHandheld;
		array2[59] = mS_.marktforschung_bestPlattformHandy;
		array2[60] = mS_.marktforschung_badPlattformHandy;
		array2[61] = mS_.pubOffersAmount;
		array2[62] = publishingOfferMain_.amountPublishingOffers;
		array2[63] = menuArcadePreis_.setCase;
		array2[64] = menuArcadePreis_.setMonitor;
		array2[65] = menuArcadePreis_.setJoystick;
		array2[66] = menuArcadePreis_.setSound;
		array2[67] = mS_.awardBonus;
		array2[68] = mS_.lastUsedEngine;
		array2[69] = mS_.automatic_RemoveGameFormMarket_Amount;
		array2[70] = mS_.schlechteSpiele;
		array2[71] = mS_.lastSchlechteSpiele[0];
		array2[72] = mS_.lastSchlechteSpiele[1];
		array2[73] = mS_.lastSchlechteSpiele[2];
		array2[74] = mS_.lastSchlechteSpiele[3];
		array2[75] = mS_.lastSchlechteSpiele[4];
		array2[76] = mS_.devTimeSetting;
		array2[77] = mS_.settings_startjahr;
		array2[78] = gpS_.gamePass_AboPreis;
		array2[79] = gpS_.gamePass_AboPreisOld;
		array2[80] = mS_.sandbox_lager;
		array2[81] = mS_.sandbox_server;
		array2[82] = mS_.sandbox_bugs;
		array2[83] = mS_.sandbox_support;
		array2[84] = mS_.settings_randomEvents;
		array2[85] = mS_.sabotage_pr;
		array2[86] = mS_.sabotage_motivation;
		array2[87] = mS_.sabotage_klage;
		array2[88] = mS_.sabotage_reviews;
		array2[89] = mS_.sabotage_geruecht;
		array2[90] = mS_.sabotage_work;
		array2[91] = mS_.sabotage_dunkel;
		array2[92] = mS_.sabotage_erwischt;
		array2[93] = mS_.settings_competition;
		array2[94] = mS_.sandbox_arbeitsmarkt;
		array2[95] = mS_.settings_RandomReviewsNum;
		array2[96] = mS_.settings_RandomReviewsNum;
		array3[0] = mS_.auftragsAnsehen;
		if (!mS_.multiplayer)
		{
			array3[1] = mS_.dayTimer;
		}
		array3[2] = mS_.record_Gameplay;
		array3[3] = mS_.record_Grafik;
		array3[4] = mS_.record_Sound;
		array3[5] = mS_.record_Technik;
		array3[6] = mS_.speedSetting;
		array3[7] = mS_.marktforschung_digtal;
		array3[8] = mS_.marktforschung_retail;
		array3[9] = mS_.marktforschung_deluxe;
		array3[10] = mS_.marktforschung_collectors;
		array3[11] = mS_.marktforschung_arcade;
		array3[12] = mS_.awardBonusAmount;
		array3[13] = mS_.marktforschung_internet;
		array3[14] = mS_.sandbox_gameSells;
		array3[15] = mS_.sandbox_konsoleSells;
		array3[16] = mS_.sandbox_trainingSpeed;
		array3[17] = mS_.marktforschung_gamePass;
		array3[18] = mS_.sandbox_maschineSpeed;
		array3[19] = mS_.sandbox_mitarbeiterGehalt;
		array3[20] = mS_.sandbox_mitarbeiterSpeed;
		array3[21] = mS_.sandbox_npcGameQuality;
		array[0] = mS_.money;
		array[1] = mS_.kredit;
		array[2] = gpS_.gamePass_UmsatzJahr;
		array[3] = gpS_.gamePass_Abos;
		array[4] = gpS_.gamePass_AbosLetzteWoche;
		array4[0] = mS_.GetCompanyName();
		array4[1] = mS_.playerName;
		array4[2] = DateTime.Now.ToShortDateString() + " ▪ " + DateTime.Now.ToShortTimeString();
		array4[3] = mS_.marktforschung_datum;
		array4[4] = gpS_.gamePass_name;
		array4[5] = mS_.sandbox_string;
		array5[0] = mS_.settings_TutorialOff;
		array5[1] = mS_.settings_closeNPCs;
		array5[2] = mS_.automatic_RemoveGameFormMarket;
		array5[3] = mS_.multiplayer;
		array5[4] = mS_.settings_autoPauseForMultiplayer;
		array5[5] = mS_.settings_RandomReviews;
		array5[6] = mS_.settings_arbeitsgeschwindigkeitAnpassen;
		array5[7] = mS_.personal_dontLeaveBuilding;
		array5[8] = mS_.personal_RobotDontLeaveBuilding;
		array5[9] = mS_.personal_ki;
		array5[10] = mS_.badGameThisYear;
		array5[11] = mS_.sellLagerbestandAutomatic;
		array5[12] = mS_.lastGameCommercialFlop;
		array5[13] = mS_.gameTabs_invert;
		array5[14] = mS_.settings_plattformEnd;
		array5[15] = mS_.settings_allGamespeed;
		array5[16] = mS_.support_kostenpflichtig;
		array5[17] = mS_.personal_autoGehaltsverhandlung;
		array5[18] = mS_.settings_sandbox;
		array5[19] = mS_.settings_randomPlattformPop;
		array5[20] = mS_.settings_randomGameConcept;
		array5[21] = mS_.settings_randomGenreCombination;
		array5[22] = gpS_.gamePass_aktiv;
		array5[23] = mS_.sandbox_unlimitedMoney;
		array5[24] = mS_.sandbox_mitarbeiterMotivation;
		array5[25] = mS_.sandbox_mitarbeiterPause;
		array5[26] = mS_.sandbox_publisherMaxReleation;
		array5[27] = mS_.sandbox_allItems;
		array5[28] = mS_.sandbox_mitarbeiterKrank;
		array5[29] = mS_.sandbox_keinIpVerfall;
		array5[30] = mS_.sandbox_bekannteKonzeptEinstellungen;
		array5[31] = mS_.sandbox_mitarbeiterSkill100;
		array5[32] = mS_.sandbox_fitTopicToGenre;
		array5[33] = mS_.sandbox_allBuildings;
		array5[35] = mS_.sandbox_tochterfirmaKonsole;
		array5[36] = mS_.settings_sabotageOff;
		array5[37] = mS_.sabotage_wurdeErwischt;
		array5[38] = mS_.settings_tochterfirmaOff;
		array5[39] = mS_.settings_randomPlattformSuit;
		writer.Write<int[]>("globals_int", array2);
		writer.Write<float[]>("globals_float", array3);
		writer.Write<long[]>("globals_long", array);
		writer.Write<string[]>("globals_string", array4);
		writer.Write<bool[]>("globals_bool", array5);
		writer.Write<bool[]>("unlock->unlock", unlock_.unlock);
		writer.Write<bool[]>("devLegendsInUse", mS_.devLegendsInUse);
		writer.Write<string[]>("tS->devLegends", tS_.devLegends);
		writer.Write<float[]>("fS->RES_POINTS_LEFT", fS_.RES_POINTS_LEFT);
		writer.Write<float[]>("merchStandardpreis", mS_.merchStandardpreis);
		writer.Write<long[]>("fanshopverlauf", mS_.fanshopverlauf);
		writer.Write<long[]>("finanzVerlauf", mS_.finanzVerlauf);
		writer.Write<long[]>("verkaufsverlauf", mS_.verkaufsverlauf);
		writer.Write<long[]>("verkaufsverlaufKonsolen", mS_.verkaufsverlaufKonsolen);
		writer.Write<long[]>("aboverlauf", mS_.aboverlauf);
		writer.Write<long[]>("downloadverlauf", mS_.downloadverlauf);
		writer.Write<long[]>("fansverlauf", mS_.fansverlauf);
		writer.Write<long[]>("finanzenMonat", mS_.finanzenMonat);
		writer.Write<long[]>("finanzenMonatLast", mS_.finanzenMonatLast);
		writer.Write<long[]>("finanzenJahr", mS_.finanzenJahr);
		writer.Write<long[]>("finanzenJahrLast", mS_.finanzenJahrLast);
		writer.Write<bool[]>("newsSetting", mS_.newsSetting);
		writer.Write<bool[]>("gameTabFilter", mS_.gameTabFilter);
		writer.Write<bool[]>("buildings", mS_.buildings);
		writer.Write<string[]>("personal_group_names", mS_.personal_group_names);
		writer.Write<int[]>("verkaufspreis_default_addon", menuPackung_.verkaufspreis_default_addon);
		writer.Write<int[]>("verkaufspreis_default_budget", menuPackung_.verkaufspreis_default_budget);
		writer.Write<int[]>("verkaufspreis_default_goty", menuPackung_.verkaufspreis_default_goty);
		writer.Write<int[]>("verkaufspreis_default_standard", menuPackung_.verkaufspreis_default_standard);
		writer.Write<bool[]>("standard_default_addon", menuPackung_.standard_default_addon);
		writer.Write<bool[]>("deluxe_default_addon", menuPackung_.deluxe_default_addon);
		writer.Write<bool[]>("collectors_default_addon", menuPackung_.collectors_default_addon);
		writer.Write<bool[]>("standard_default_budget", menuPackung_.standard_default_budget);
		writer.Write<bool[]>("deluxe_default_budget", menuPackung_.deluxe_default_budget);
		writer.Write<bool[]>("collectors_default_budget", menuPackung_.collectors_default_budget);
		writer.Write<bool[]>("standard_default_goty", menuPackung_.standard_default_goty);
		writer.Write<bool[]>("deluxe_default_goty", menuPackung_.deluxe_default_goty);
		writer.Write<bool[]>("collectors_default_goty", menuPackung_.collectors_default_goty);
		writer.Write<bool[]>("standard_default_standard", menuPackung_.standard_default_standard);
		writer.Write<bool[]>("deluxe_default_standard", menuPackung_.deluxe_default_standard);
		writer.Write<bool[]>("collectors_default_standard", menuPackung_.collectors_default_standard);
		writer.Write<int[]>("verkaufspreis_default_bundle", menuPackung_.verkaufspreis_default_bundle);
		writer.Write<int[]>("verkaufspreis_default_bundleAddon", menuPackung_.verkaufspreis_default_bundleAddon);
		writer.Write<bool[]>("standard_default_bundleAddon", menuPackung_.standard_default_bundleAddon);
		writer.Write<bool[]>("deluxe_default_bundleAddon", menuPackung_.deluxe_default_bundleAddon);
		writer.Write<bool[]>("collectors_default_bundleAddon", menuPackung_.collectors_default_bundleAddon);
		writer.Write<bool[]>("standard_default_bundle", menuPackung_.standard_default_bundle);
		writer.Write<bool[]>("deluxe_default_bundle", menuPackung_.deluxe_default_bundle);
		writer.Write<bool[]>("collectors_default_bundle", menuPackung_.collectors_default_bundle);
		writer.Write<long[]>("gamePass_aboVerlaufWoche", gpS_.gamePass_aboVerlaufWoche);
		writer.Write<long[]>("gamePass_aboVerlaufMonat", gpS_.gamePass_aboVerlaufMonat);
		writer.Write<long[]>("gamePass_umsatzVerlaufMonat", gpS_.gamePass_umsatzVerlaufMonat);
		writer.Write<bool[]>("achivements", mS_.achivements);
		writer.Write<bool[]>("inventarFilter", menu_BuyInventar_.filter);
		writer.Write<string[]>("npcIPs", tS_.npcIPs);
		writer.Write<bool[]>("npcIPsInUse", tS_.npcIPsInUse);
		long[] value = mS_.finanzVerlaufEinnahmen.ToArray();
		writer.Write<long[]>("finanzVerlaufEinnahmen", value);
		long[] value2 = mS_.finanzVerlaufAusgaben.ToArray();
		writer.Write<long[]>("finanzVerlaufAusgaben", value2);
		int[] value3 = mS_.madGamesCon_Jahr.ToArray();
		writer.Write<int[]>("madGamesCon_Jahr", value3);
		int[] value4 = mS_.madGamesCon_BestGrafik.ToArray();
		writer.Write<int[]>("madGamesCon_BestGrafik", value4);
		int[] value5 = mS_.madGamesCon_BestSound.ToArray();
		writer.Write<int[]>("madGamesCon_BestSound", value5);
		int[] value6 = mS_.madGamesCon_BestStudio.ToArray();
		writer.Write<int[]>("madGamesCon_BestStudio", value6);
		int[] value7 = mS_.madGamesCon_BestPublisher.ToArray();
		writer.Write<int[]>("madGamesCon_BestPublisher", value7);
		int[] value8 = mS_.madGamesCon_BestGame.ToArray();
		writer.Write<int[]>("madGamesCon_BestGame", value8);
		int[] value9 = mS_.madGamesCon_BadGame.ToArray();
		writer.Write<int[]>("madGamesCon_BadGame", value9);
		if (!mS_.multiplayer)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (mpCalls_.playersMP.Count > i)
			{
				player_mp player_mp2 = mpCalls_.playersMP[i];
				writer.Write<int>("MP_playerID" + i, player_mp2.playerID);
				writer.Write<string>("MP_playerName" + i, player_mp2.playerName);
				writer.Write<int[,]>("MP_mapRoomID" + i, player_mp2.mapRoomID);
				writer.Write<int[,]>("MP_mapRoomTyp" + i, player_mp2.mapRoomTyp);
				writer.Write<int[,]>("MP_mapDoors" + i, player_mp2.mapDoors);
				writer.Write<int[,]>("MP_mapWindows" + i, player_mp2.mapWindows);
				int count = player_mp2.objects.Count;
				int[] array6 = new int[count];
				int[] array7 = new int[count];
				Vector3[] array8 = new Vector3[count];
				for (int j = 0; j < count; j++)
				{
					if (player_mp2.objects[j] != null)
					{
						array6[j] = player_mp2.objects[j].id;
						array7[j] = player_mp2.objects[j].typ;
						array8[j] = new Vector3(player_mp2.objects[j].posX, player_mp2.objects[j].posY, player_mp2.objects[j].rotation);
					}
				}
				writer.Write<int[]>("MP_object_id" + i, array6);
				writer.Write<int[]>("MP_object_typ" + i, array7);
				writer.Write<Vector3[]>("MP_object_pos" + i, array8);
			}
			else
			{
				writer.Write<int>("MP_playerID" + i, -1);
			}
		}
	}

	private void LoadGlobals(ES3Reader reader, string filename)
	{
		long[] array = new long[100];
		int[] array2 = new int[100];
		float[] array3 = new float[100];
		string[] array4 = new string[100];
		bool[] array5 = new bool[100];
		array2 = reader.Read<int[]>("globals_int");
		array3 = reader.Read<float[]>("globals_float");
		array = reader.Read<long[]>("globals_long");
		array4 = reader.Read<string[]>("globals_string");
		array5 = reader.Read<bool[]>("globals_bool");
		mS_.year = array2[3];
		mS_.month = array2[4];
		mS_.week = array2[5];
		mS_.trendGenre = array2[6];
		mS_.trendAntiGenre = array2[7];
		mS_.trendTheme = array2[8];
		mS_.trendAntiTheme = array2[9];
		mS_.trendWeeks = array2[10];
		mS_.anrufe = array2[11];
		mS_.goldeneSchallplatten = array2[12];
		mS_.difficulty = array2[13];
		mS_.anzKonkurrenten = array2[14];
		mS_.personal_druck = array2[15];
		mS_.personal_pausen = array2[16];
		mS_.personal_motivation = array2[17];
		mS_.personal_crunch = array2[18];
		mS_.exklusivVertrag_ID = array2[19];
		mS_.exklusivVertrag_laufzeit = array2[20];
		mS_.office = array2[21];
		mS_.globalEvent = array2[22];
		mS_.globalEventWeeks = array2[23];
		mS_.marktforschung_bestPlattform = array2[24];
		mS_.marktforschung_badPlattform = array2[25];
		mS_.marktforschung_nextGenre = array2[26];
		mS_.marktforschung_nextTopic = array2[27];
		mS_.marktforschung_nextBadGenre = array2[28];
		mS_.marktforschung_nextBadTopic = array2[29];
		mS_.trendNextGenre = array2[30];
		mS_.trendNextAntiGenre = array2[31];
		mS_.trendNextTheme = array2[32];
		mS_.trendNextAntiTheme = array2[33];
		mS_.multiplayerSaveID = array2[34];
		mS_.bankWarning = array2[35];
		mS_.lastGamesGenre[0] = array2[36];
		mS_.lastGamesGenre[1] = array2[37];
		mS_.lastGamesGenre[2] = array2[38];
		mS_.lastGamesGenre[3] = array2[39];
		mS_.lastGamesGenre[4] = array2[40];
		mS_.gelangweiltGenre = array2[41];
		mS_.platinSchallplatten = array2[42];
		mS_.diamantSchallplatten = array2[43];
		mS_.sauerBugs = array2[44];
		mS_.savegameVersion = array2[45];
		mS_.award_GOTY = array2[46];
		mS_.award_Studio = array2[47];
		mS_.gameTabs_sort = array2[48];
		mS_.myID = array2[49];
		mS_.award_Grafik = array2[50];
		mS_.award_Sound = array2[51];
		mS_.award_Trendsetter = array2[52];
		mS_.studioPoints = array2[53];
		mS_.award_Publisher = array2[54];
		mS_.marktforschung_bestPlattformKonsole = array2[55];
		mS_.marktforschung_badPlattformKonsole = array2[56];
		mS_.marktforschung_bestPlattformHandheld = array2[57];
		mS_.marktforschung_badPlattformHandheld = array2[58];
		mS_.marktforschung_bestPlattformHandy = array2[59];
		mS_.marktforschung_badPlattformHandy = array2[60];
		mS_.pubOffersAmount = array2[61];
		publishingOfferMain_.amountPublishingOffers = array2[62];
		menuArcadePreis_.setCase = array2[63];
		menuArcadePreis_.setMonitor = array2[64];
		menuArcadePreis_.setJoystick = array2[65];
		menuArcadePreis_.setSound = array2[66];
		mS_.awardBonus = array2[67];
		mS_.lastUsedEngine = array2[68];
		mS_.automatic_RemoveGameFormMarket_Amount = array2[69];
		mS_.schlechteSpiele = array2[70];
		mS_.lastSchlechteSpiele[0] = array2[71];
		mS_.lastSchlechteSpiele[1] = array2[72];
		mS_.lastSchlechteSpiele[2] = array2[73];
		mS_.lastSchlechteSpiele[3] = array2[74];
		mS_.lastSchlechteSpiele[4] = array2[75];
		mS_.devTimeSetting = array2[76];
		mS_.settings_startjahr = array2[77];
		gpS_.gamePass_AboPreis = array2[78];
		gpS_.gamePass_AboPreisOld = array2[79];
		mS_.sandbox_lager = array2[80];
		mS_.sandbox_server = array2[81];
		mS_.sandbox_bugs = array2[82];
		mS_.sandbox_support = array2[83];
		mS_.settings_randomEvents = array2[84];
		mS_.sabotage_pr = array2[85];
		mS_.sabotage_motivation = array2[86];
		mS_.sabotage_klage = array2[87];
		mS_.sabotage_reviews = array2[88];
		mS_.sabotage_geruecht = array2[89];
		mS_.sabotage_work = array2[90];
		mS_.sabotage_dunkel = array2[91];
		mS_.sabotage_erwischt = array2[92];
		mS_.settings_competition = array2[93];
		mS_.sandbox_arbeitsmarkt = array2[94];
		if (mS_.sandbox_arbeitsmarkt <= 0)
		{
			mS_.sandbox_arbeitsmarkt = 30;
		}
		mS_.settings_RandomReviewsNum = array2[95];
		if (mS_.settings_RandomReviewsNum <= 0)
		{
			mS_.settings_RandomReviewsNum = 1;
		}
		mS_.settings_randomPlattformNum = array2[96];
		if (mS_.settings_randomPlattformNum <= 0)
		{
			mS_.settings_randomPlattformNum = 1;
		}
		mS_.auftragsAnsehen = array3[0];
		if (!mS_.multiplayer)
		{
			mS_.dayTimer = array3[1];
		}
		mS_.record_Gameplay = array3[2];
		mS_.record_Grafik = array3[3];
		mS_.record_Sound = array3[4];
		mS_.record_Technik = array3[5];
		mS_.speedSetting = array3[6];
		mS_.marktforschung_digtal = array3[7];
		mS_.marktforschung_retail = array3[8];
		mS_.marktforschung_deluxe = array3[9];
		mS_.marktforschung_collectors = array3[10];
		mS_.marktforschung_arcade = array3[11];
		mS_.awardBonusAmount = array3[12];
		mS_.marktforschung_internet = array3[13];
		mS_.sandbox_gameSells = array3[14];
		mS_.sandbox_konsoleSells = array3[15];
		mS_.sandbox_trainingSpeed = array3[16];
		mS_.marktforschung_gamePass = array3[17];
		mS_.sandbox_maschineSpeed = array3[18];
		mS_.sandbox_mitarbeiterGehalt = array3[19];
		mS_.sandbox_mitarbeiterSpeed = array3[20];
		if (mS_.sandbox_mitarbeiterSpeed <= 0f)
		{
			mS_.sandbox_mitarbeiterSpeed = 1f;
		}
		mS_.sandbox_npcGameQuality = array3[21];
		if (mS_.sandbox_npcGameQuality <= 0f)
		{
			mS_.sandbox_npcGameQuality = 1f;
		}
		mS_.money = array[0];
		mS_.kredit = array[1];
		gpS_.gamePass_UmsatzJahr = array[2];
		gpS_.gamePass_Abos = array[3];
		gpS_.gamePass_AbosLetzteWoche = array[4];
		mS_.playerName = array4[1];
		mS_.marktforschung_datum = array4[3];
		gpS_.gamePass_name = array4[4];
		mS_.sandbox_string = array4[5];
		mS_.settings_TutorialOff = array5[0];
		mS_.settings_closeNPCs = array5[1];
		mS_.automatic_RemoveGameFormMarket = array5[2];
		mS_.multiplayer = array5[3];
		mS_.settings_autoPauseForMultiplayer = array5[4];
		mS_.settings_RandomReviews = array5[5];
		mS_.settings_arbeitsgeschwindigkeitAnpassen = array5[6];
		mS_.personal_dontLeaveBuilding = array5[7];
		mS_.personal_RobotDontLeaveBuilding = array5[8];
		mS_.personal_ki = array5[9];
		mS_.badGameThisYear = array5[10];
		mS_.sellLagerbestandAutomatic = array5[11];
		mS_.lastGameCommercialFlop = array5[12];
		mS_.gameTabs_invert = array5[13];
		mS_.settings_plattformEnd = array5[14];
		mS_.settings_allGamespeed = array5[15];
		mS_.support_kostenpflichtig = array5[16];
		mS_.personal_autoGehaltsverhandlung = array5[17];
		mS_.settings_sandbox = array5[18];
		mS_.settings_randomPlattformPop = array5[19];
		mS_.settings_randomGameConcept = array5[20];
		mS_.settings_randomGenreCombination = array5[21];
		gpS_.gamePass_aktiv = array5[22];
		mS_.sandbox_unlimitedMoney = array5[23];
		mS_.sandbox_mitarbeiterMotivation = array5[24];
		mS_.sandbox_mitarbeiterPause = array5[25];
		mS_.sandbox_publisherMaxReleation = array5[26];
		mS_.sandbox_allItems = array5[27];
		mS_.sandbox_mitarbeiterKrank = array5[28];
		mS_.sandbox_keinIpVerfall = array5[29];
		mS_.sandbox_bekannteKonzeptEinstellungen = array5[30];
		mS_.sandbox_mitarbeiterSkill100 = array5[31];
		mS_.sandbox_fitTopicToGenre = array5[32];
		mS_.sandbox_allBuildings = array5[33];
		mS_.sandbox_tochterfirmaKonsole = array5[35];
		mS_.settings_sabotageOff = array5[36];
		mS_.sabotage_wurdeErwischt = array5[37];
		mS_.settings_tochterfirmaOff = array5[38];
		mS_.settings_randomPlattformSuit = array5[39];
		unlock_.unlock = reader.Read<bool[]>("unlock->unlock");
		mS_.devLegendsInUse = reader.Read<bool[]>("devLegendsInUse");
		tS_.devLegends = reader.Read<string[]>("tS->devLegends");
		fS_.RES_POINTS_LEFT = reader.Read<float[]>("fS->RES_POINTS_LEFT");
		if (fS_.RES_POINTS_LEFT.Length < fS_.RES_POINTS.Length)
		{
			fS_.RES_POINTS_LEFT = new float[fS_.RES_POINTS.Length];
			float[] array6 = reader.Read<float[]>("fS->RES_POINTS_LEFT");
			for (int i = 0; i < array6.Length; i++)
			{
				fS_.RES_POINTS_LEFT[i] = array6[i];
			}
		}
		if (key_merchStandardpreis)
		{
			mS_.merchStandardpreis = reader.Read<float[]>("merchStandardpreis");
		}
		if (key_fanshopverlauf)
		{
			mS_.fanshopverlauf = reader.Read<long[]>("fanshopverlauf");
		}
		mS_.finanzVerlauf = reader.Read<long[]>("finanzVerlauf");
		mS_.verkaufsverlauf = reader.Read<long[]>("verkaufsverlauf");
		mS_.verkaufsverlaufKonsolen = reader.Read<long[]>("verkaufsverlaufKonsolen");
		mS_.aboverlauf = reader.Read<long[]>("aboverlauf");
		mS_.downloadverlauf = reader.Read<long[]>("downloadverlauf");
		mS_.fansverlauf = reader.Read<long[]>("fansverlauf");
		mS_.finanzenMonat = reader.Read<long[]>("finanzenMonat");
		mS_.finanzenMonatLast = reader.Read<long[]>("finanzenMonatLast");
		mS_.finanzenJahr = reader.Read<long[]>("finanzenJahr");
		mS_.finanzenJahrLast = reader.Read<long[]>("finanzenJahrLast");
		mS_.newsSetting = reader.Read<bool[]>("newsSetting");
		if (key_gameTabFilter)
		{
			int num = mS_.gameTabFilter.Length;
			mS_.gameTabFilter = reader.Read<bool[]>("gameTabFilter");
			if (num != mS_.gameTabFilter.Length)
			{
				mS_.gameTabFilter = new bool[num];
				for (int j = 0; j < mS_.gameTabFilter.Length; j++)
				{
					mS_.gameTabFilter[j] = true;
				}
				mS_.gameTabFilter[11] = false;
			}
		}
		mS_.buildings = reader.Read<bool[]>("buildings");
		mS_.personal_group_names = reader.Read<string[]>("personal_group_names");
		if (key_default_verkaufpreis)
		{
			menuPackung_.verkaufspreis_default_addon = reader.Read<int[]>("verkaufspreis_default_addon");
			menuPackung_.verkaufspreis_default_budget = reader.Read<int[]>("verkaufspreis_default_budget");
			menuPackung_.verkaufspreis_default_goty = reader.Read<int[]>("verkaufspreis_default_goty");
			menuPackung_.verkaufspreis_default_standard = reader.Read<int[]>("verkaufspreis_default_standard");
			menuPackung_.standard_default_addon = reader.Read<bool[]>("standard_default_addon");
			menuPackung_.deluxe_default_addon = reader.Read<bool[]>("deluxe_default_addon");
			menuPackung_.collectors_default_addon = reader.Read<bool[]>("collectors_default_addon");
			menuPackung_.standard_default_budget = reader.Read<bool[]>("standard_default_budget");
			menuPackung_.deluxe_default_budget = reader.Read<bool[]>("deluxe_default_budget");
			menuPackung_.collectors_default_budget = reader.Read<bool[]>("collectors_default_budget");
			menuPackung_.standard_default_goty = reader.Read<bool[]>("standard_default_goty");
			menuPackung_.deluxe_default_goty = reader.Read<bool[]>("deluxe_default_goty");
			menuPackung_.collectors_default_goty = reader.Read<bool[]>("collectors_default_goty");
			menuPackung_.standard_default_standard = reader.Read<bool[]>("standard_default_standard");
			menuPackung_.deluxe_default_standard = reader.Read<bool[]>("deluxe_default_standard");
			menuPackung_.collectors_default_standard = reader.Read<bool[]>("collectors_default_standard");
		}
		if (key_default_verkaufpreisBundle)
		{
			menuPackung_.verkaufspreis_default_bundle = reader.Read<int[]>("verkaufspreis_default_bundle");
			menuPackung_.verkaufspreis_default_bundleAddon = reader.Read<int[]>("verkaufspreis_default_bundleAddon");
			menuPackung_.standard_default_bundleAddon = reader.Read<bool[]>("standard_default_bundleAddon");
			menuPackung_.deluxe_default_bundleAddon = reader.Read<bool[]>("deluxe_default_bundleAddon");
			menuPackung_.collectors_default_bundleAddon = reader.Read<bool[]>("collectors_default_bundleAddon");
			menuPackung_.standard_default_bundle = reader.Read<bool[]>("standard_default_bundle");
			menuPackung_.deluxe_default_bundle = reader.Read<bool[]>("deluxe_default_bundle");
			menuPackung_.collectors_default_bundle = reader.Read<bool[]>("collectors_default_bundle");
		}
		if (key_achivements)
		{
			mS_.achivements = reader.Read<bool[]>("achivements");
			if (mS_.achivements.Length != mS_.achivementsBonus.Length)
			{
				bool[] array7 = reader.Read<bool[]>("achivements");
				mS_.achivements = new bool[mS_.achivementsBonus.Length];
				for (int k = 0; k < array7.Length; k++)
				{
					mS_.achivements[k] = array7[k];
				}
			}
		}
		if (key_gamePass_aboVerlaufWoche)
		{
			gpS_.gamePass_aboVerlaufWoche = reader.Read<long[]>("gamePass_aboVerlaufWoche");
			if (gpS_.gamePass_aboVerlaufWoche.Length < 20)
			{
				gpS_.gamePass_aboVerlaufWoche = new long[20];
			}
		}
		else
		{
			gpS_.gamePass_aboVerlaufWoche = new long[20];
		}
		if (key_gamePass_aboVerlaufMonat)
		{
			gpS_.gamePass_aboVerlaufMonat = reader.Read<long[]>("gamePass_aboVerlaufMonat");
			if (gpS_.gamePass_aboVerlaufMonat.Length < 24)
			{
				gpS_.gamePass_aboVerlaufMonat = new long[24];
			}
		}
		else
		{
			gpS_.gamePass_aboVerlaufMonat = new long[24];
		}
		if (key_gamePass_umsatzVerlaufMonat)
		{
			gpS_.gamePass_umsatzVerlaufMonat = reader.Read<long[]>("gamePass_umsatzVerlaufMonat");
			if (gpS_.gamePass_umsatzVerlaufMonat.Length < 24)
			{
				gpS_.gamePass_umsatzVerlaufMonat = new long[24];
			}
		}
		else
		{
			gpS_.gamePass_umsatzVerlaufMonat = new long[24];
		}
		if (key_inventarFilter)
		{
			menu_BuyInventar_.filter = reader.Read<bool[]>("inventarFilter");
		}
		if (key_NpcIPs)
		{
			tS_.npcIPs = reader.Read<string[]>("npcIPs");
			tS_.npcIPsInUse = reader.Read<bool[]>("npcIPsInUse");
		}
		long[] array8 = reader.Read<long[]>("finanzVerlaufEinnahmen");
		for (int l = 0; l < array8.Length; l++)
		{
			mS_.finanzVerlaufEinnahmen.Add(array8[l]);
		}
		long[] array9 = reader.Read<long[]>("finanzVerlaufAusgaben");
		for (int m = 0; m < array9.Length; m++)
		{
			mS_.finanzVerlaufAusgaben.Add(array9[m]);
		}
		int[] array10 = reader.Read<int[]>("madGamesCon_Jahr");
		for (int n = 0; n < array10.Length; n++)
		{
			mS_.madGamesCon_Jahr.Add(array10[n]);
		}
		int[] array11 = reader.Read<int[]>("madGamesCon_BestGrafik");
		for (int num2 = 0; num2 < array11.Length; num2++)
		{
			mS_.madGamesCon_BestGrafik.Add(array11[num2]);
		}
		int[] array12 = reader.Read<int[]>("madGamesCon_BestSound");
		for (int num3 = 0; num3 < array12.Length; num3++)
		{
			mS_.madGamesCon_BestSound.Add(array12[num3]);
		}
		int[] array13 = reader.Read<int[]>("madGamesCon_BestStudio");
		for (int num4 = 0; num4 < array13.Length; num4++)
		{
			mS_.madGamesCon_BestStudio.Add(array13[num4]);
		}
		int[] array14 = reader.Read<int[]>("madGamesCon_BestPublisher");
		for (int num5 = 0; num5 < array14.Length; num5++)
		{
			mS_.madGamesCon_BestPublisher.Add(array14[num5]);
		}
		int[] array15 = reader.Read<int[]>("madGamesCon_BestGame");
		for (int num6 = 0; num6 < array15.Length; num6++)
		{
			mS_.madGamesCon_BestGame.Add(array15[num6]);
		}
		int[] array16 = reader.Read<int[]>("madGamesCon_BadGame");
		for (int num7 = 0; num7 < array16.Length; num7++)
		{
			mS_.madGamesCon_BadGame.Add(array16[num7]);
		}
		if (!mS_.multiplayer)
		{
			return;
		}
		mpCalls_.playersMP.Clear();
		for (int num8 = 0; num8 < 4; num8++)
		{
			int num9 = reader.Read<int>("MP_playerID" + num8);
			if (num9 != -1)
			{
				mpCalls_.playersMP.Add(new player_mp(num9));
				player_mp player_mp2 = mpCalls_.FindPlayer(num9);
				player_mp2.playerName = reader.Read<string>("MP_playerName" + num8);
				player_mp2.mapRoomID = reader.Read<int[,]>("MP_mapRoomID" + num8);
				player_mp2.mapRoomTyp = reader.Read<int[,]>("MP_mapRoomTyp" + num8);
				player_mp2.mapDoors = reader.Read<int[,]>("MP_mapDoors" + num8);
				player_mp2.mapWindows = reader.Read<int[,]>("MP_mapWindows" + num8);
				int[] array17 = reader.Read<int[]>("MP_object_id" + num8);
				int[] array18 = reader.Read<int[]>("MP_object_typ" + num8);
				Vector3[] array19 = reader.Read<Vector3[]>("MP_object_pos" + num8);
				for (int num10 = 0; num10 < array17.Length; num10++)
				{
					player_mp2.objects.Add(new object_mp(array17[num10], array18[num10], array19[num10].x, array19[num10].y, array19[num10].z));
				}
			}
		}
	}

	private void SaveMitarbeiter(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		int num = array.Length;
		writer.Write<int>("anzCharacter", num);
		int[] array2 = new int[19 * num];
		float[] array3 = new float[24 * num];
		string[] array4 = new string[num];
		bool[] array5 = new bool[num];
		int num2 = 0;
		if (array.Length != 0 && (bool)array[0])
		{
			characterScript component = array[0].GetComponent<characterScript>();
			if ((bool)component)
			{
				num2 = component.perks.Length;
			}
		}
		bool[] array6 = new bool[num2 * num];
		for (int i = 0; i < array.Length; i++)
		{
			characterScript component2 = array[i].GetComponent<characterScript>();
			int num3 = i * (array4.Length / num);
			int num4 = i * (array2.Length / num);
			int num5 = i * (array3.Length / num);
			int num6 = i * (array5.Length / num);
			array2[num4] = component2.myID;
			array2[1 + num4] = component2.group;
			array2[2 + num4] = component2.roomID;
			array2[3 + num4] = component2.objectUsingID;
			array2[4 + num4] = component2.objectBelegtID;
			array2[5 + num4] = component2.legend;
			array2[6 + num4] = component2.model_body;
			array2[7 + num4] = component2.model_eyes;
			array2[8 + num4] = component2.model_hair;
			array2[9 + num4] = component2.model_beard;
			array2[10 + num4] = component2.model_skinColor;
			array2[11 + num4] = component2.model_hairColor;
			array2[12 + num4] = component2.model_beardColor;
			array2[13 + num4] = component2.model_HoseColor;
			array2[14 + num4] = component2.model_ShirtColor;
			array2[15 + num4] = component2.model_Add1Color;
			array2[16 + num4] = component2.krank;
			array2[17 + num4] = component2.beruf;
			array2[18 + num4] = component2.gehalt;
			array3[num5] = array[i].transform.position.x;
			array3[1 + num5] = array[i].transform.position.y;
			array3[2 + num5] = array[i].transform.position.z;
			array3[3 + num5] = component2.s_motivation;
			array3[4 + num5] = component2.s_gamedesign;
			array3[5 + num5] = component2.s_programmieren;
			array3[6 + num5] = component2.s_grafik;
			array3[7 + num5] = component2.s_sound;
			array3[8 + num5] = component2.s_pr;
			array3[9 + num5] = component2.s_gametests;
			array3[10 + num5] = component2.s_technik;
			array3[11 + num5] = component2.s_forschen;
			array3[12 + num5] = component2.workProgress;
			array3[13 + num5] = component2.durst;
			array3[14 + num5] = component2.hunger;
			array3[15 + num5] = component2.klo;
			array3[16 + num5] = component2.waschbecken;
			array3[17 + num5] = component2.muell;
			array3[18 + num5] = component2.giessen;
			array3[19 + num5] = component2.pause;
			array3[20 + num5] = array[i].transform.eulerAngles.x;
			array3[21 + num5] = array[i].transform.eulerAngles.y;
			array3[22 + num5] = array[i].transform.eulerAngles.z;
			array3[23 + num5] = component2.freezer;
			array4[num3] = component2.myName;
			array5[num6] = component2.male;
			int num7 = i * num2;
			for (int j = 0; j < component2.perks.Length; j++)
			{
				array6[j + num7] = component2.perks[j];
			}
		}
		writer.Write<int[]>("characters_I", array2);
		writer.Write<float[]>("characters_F", array3);
		writer.Write<string[]>("characters_S", array4);
		writer.Write<bool[]>("characters_B", array5);
		writer.Write<bool[]>("characters_perks", array6);
	}

	private void LoadMitarbeiter(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzCharacter", -1);
		if (num <= 0)
		{
			return;
		}
		_ = new long[0];
		int[] array = new int[0];
		float[] array2 = new float[0];
		string[] array3 = new string[0];
		bool[] array4 = new bool[0];
		bool[] array5 = new bool[0];
		array = reader.Read<int[]>("characters_I");
		array2 = reader.Read<float[]>("characters_F");
		array3 = reader.Read<string[]>("characters_S");
		array4 = reader.Read<bool[]>("characters_B");
		int num2 = array.Length / num;
		int num3 = array2.Length / num;
		int num4 = array4.Length / num;
		int num5 = array3.Length / num;
		if (mS_.savegameVersion >= 16)
		{
			array5 = reader.Read<bool[]>("characters_perks");
		}
		for (int i = 0; i < num; i++)
		{
			int num6 = i * num2;
			int num7 = i * num3;
			int num8 = i * num4;
			int num9 = i * num5;
			characterScript characterScript2 = cCS_.CreateCharacter(array[num6], array4[num8], array[6 + num6]);
			characterScript2.myID = array[num6];
			characterScript2.group = array[1 + num6];
			characterScript2.roomID = array[2 + num6];
			characterScript2.objectUsingID = array[3 + num6];
			characterScript2.objectBelegtID = array[4 + num6];
			characterScript2.legend = array[5 + num6];
			characterScript2.model_body = array[6 + num6];
			characterScript2.model_eyes = array[7 + num6];
			characterScript2.model_hair = array[8 + num6];
			characterScript2.model_beard = array[9 + num6];
			characterScript2.model_skinColor = array[10 + num6];
			characterScript2.model_hairColor = array[11 + num6];
			characterScript2.model_beardColor = array[12 + num6];
			characterScript2.model_HoseColor = array[13 + num6];
			characterScript2.model_ShirtColor = array[14 + num6];
			characterScript2.model_Add1Color = array[15 + num6];
			characterScript2.krank = array[16 + num6];
			characterScript2.beruf = array[17 + num6];
			if (num2 > 18)
			{
				characterScript2.gehalt = array[18 + num6];
			}
			characterScript2.s_motivation = array2[3 + num7];
			characterScript2.s_gamedesign = array2[4 + num7];
			characterScript2.s_programmieren = array2[5 + num7];
			characterScript2.s_grafik = array2[6 + num7];
			characterScript2.s_sound = array2[7 + num7];
			characterScript2.s_pr = array2[8 + num7];
			characterScript2.s_gametests = array2[9 + num7];
			characterScript2.s_technik = array2[10 + num7];
			characterScript2.s_forschen = array2[11 + num7];
			characterScript2.workProgress = array2[12 + num7];
			characterScript2.durst = array2[13 + num7];
			characterScript2.hunger = array2[14 + num7];
			characterScript2.klo = array2[15 + num7];
			characterScript2.waschbecken = array2[16 + num7];
			characterScript2.muell = array2[17 + num7];
			characterScript2.giessen = array2[18 + num7];
			characterScript2.pause = array2[19 + num7];
			characterScript2.freezer = array2[23 + num7];
			characterScript2.myName = array3[num9];
			characterScript2.male = array4[num8];
			characterScript2.perks = new bool[array5.Length / num];
			int num10 = i * (array5.Length / num);
			for (int j = 0; j < characterScript2.perks.Length; j++)
			{
				characterScript2.perks[j] = array5[j + num10];
			}
			characterScript2.gameObject.transform.GetChild(0).GetComponent<characterGFXScript>().Init(forcedClothes: true);
			characterScript2.gameObject.transform.position = new Vector3(array2[num7], array2[1 + num7], array2[2 + num7]);
			characterScript2.gameObject.transform.eulerAngles = new Vector3(array2[20 + num7], array2[21 + num7], array2[22 + num7]);
			if (characterScript2.objectBelegtID != -1)
			{
				characterScript2.gameObject.GetComponent<movementScript>().FindObjectInRoom(-1, GameObject.Find("O_" + characterScript2.objectBelegtID), onlyInRoom: false);
			}
		}
	}

	private void SaveObjects(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Object");
		int num = array.Length;
		writer.Write<int>("anzObjects", num);
		int[] array2 = new int[5 * num];
		float[] array3 = new float[5 * num];
		bool[] array4 = new bool[num];
		for (int i = 0; i < array.Length; i++)
		{
			objectScript component = array[i].GetComponent<objectScript>();
			if (component.gekauft)
			{
				int num2 = i * (array2.Length / num);
				int num3 = i * (array3.Length / num);
				int num4 = i * (array4.Length / num);
				array2[num2] = component.myID;
				array2[1 + num2] = component.typ;
				array2[2 + num2] = component.typGhost;
				array2[3 + num2] = component.besetztCharID;
				array2[4 + num2] = component.aufladungenAkt;
				array3[num3] = array[i].transform.position.x;
				array3[1 + num3] = 0f;
				array3[2 + num3] = array[i].transform.position.z;
				array3[3 + num3] = array[i].transform.eulerAngles.y;
				array3[4 + num3] = component.maschieneTimer;
				array4[num4] = component.inUse;
			}
		}
		writer.Write<int[]>("objects_I", array2);
		writer.Write<float[]>("objects_F", array3);
		writer.Write<bool[]>("objects_B", array4);
	}

	private void LoadObjects(ES3Reader reader, string filename)
	{
		mS_.FindScripts();
		int num = reader.Read("anzObjects", -1);
		Debug.Log("anzObjects: " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		array2 = reader.Read<int[]>("objects_I");
		array3 = reader.Read<float[]>("objects_F");
		array5 = reader.Read<bool[]>("objects_B");
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			if (array2[num7] != 0 && !float.IsNaN(array3[num8]))
			{
				GameObject gameObject = null;
				if (array2[1 + num7] != -1)
				{
					gameObject = UnityEngine.Object.Instantiate(mapS_.prefabsInventar[array2[1 + num7]]);
				}
				if (array2[2 + num7] != -1)
				{
					gameObject = UnityEngine.Object.Instantiate(mS_.miscGamePrefabs[array2[2 + num7]]);
				}
				gameObject.transform.position = new Vector3(array3[num8], 0f, array3[2 + num8]);
				objectScript component = gameObject.GetComponent<objectScript>();
				component.mS_ = mS_;
				component.sfx_ = sfx_;
				component.tS_ = tS_;
				component.mapS_ = mapS_;
				component.myID = array2[num7];
				component.typ = array2[1 + num7];
				component.typGhost = array2[2 + num7];
				component.besetztCharID = array2[3 + num7];
				component.aufladungenAkt = array2[4 + num7];
				component.maschieneTimer = array3[4 + num8];
				component.inUse = array5[num9];
				component.InitObjectFromSavegame();
				mS_.objectRotation = array3[3 + num8];
				component.PlatziereObject(new Vector3(array3[num8], 0f, array3[2 + num8]), fromSavegame: true, updatePathfinding: false, autoInventar: false, partikel: false);
				component.ConsumeAufladung(0);
			}
		}
	}

	private void SaveRooms(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		int num = array.Length;
		writer.Write<int>("anzRooms", num);
		int[] array2 = new int[7 * num];
		float[] array3 = new float[4 * num];
		string[] array4 = new string[num];
		bool[] array5 = new bool[3 * num];
		for (int i = 0; i < array.Length; i++)
		{
			roomScript component = array[i].GetComponent<roomScript>();
			int num2 = i * (array4.Length / num);
			int num3 = i * (array2.Length / num);
			int num4 = i * (array3.Length / num);
			int num5 = i * (array5.Length / num);
			array2[num3] = component.myID;
			array2[1 + num3] = component.typ;
			array2[2 + num3] = component.taskID;
			array2[3 + num3] = (int)component.serverplatzUsed;
			array2[4 + num3] = component.leitenderGamedesigner;
			array2[5 + num3] = component.leitenderTechniker;
			array2[6 + num3] = component.serverReservieren;
			array3[num4] = array[i].transform.position.x;
			array3[1 + num4] = array[i].transform.position.y;
			array3[2 + num4] = array[i].transform.position.z;
			array3[3 + num4] = array[i].transform.eulerAngles.y;
			array4[num2] = component.myName;
			array5[num5] = component.pause;
			array5[1 + num5] = component.lockKI;
			array5[2 + num5] = component.serverDown;
		}
		writer.Write<int[]>("rooms_I", array2);
		writer.Write<float[]>("rooms_F", array3);
		writer.Write<string[]>("rooms_S", array4);
		writer.Write<bool[]>("rooms_B", array5);
		writer.Write<int[,]>("mapRoomID", mapS_.mapRoomID);
		writer.Write<int[,]>("mapDoors", mapS_.mapDoors);
		writer.Write<int[,]>("mapWindows", mapS_.mapWindows);
	}

	private void LoadRooms(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzRooms", -1);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		_ = new float[0];
		string[] array3 = new string[0];
		bool[] array4 = new bool[0];
		array2 = reader.Read<int[]>("rooms_I");
		float[] array5 = reader.Read<float[]>("rooms_F");
		array3 = reader.Read<string[]>("rooms_S");
		array4 = reader.Read<bool[]>("rooms_B");
		int num2 = array2.Length / num;
		int num3 = array5.Length / num;
		int num4 = array4.Length / num;
		int num5 = array3.Length / num;
		int num6 = array.Length / num;
		mapS_.mapRoomID = reader.Read<int[,]>("mapRoomID");
		mapS_.mapDoors = reader.Read<int[,]>("mapDoors");
		mapS_.mapWindows = reader.Read<int[,]>("mapWindows");
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num4;
			int num9 = i * num5;
			GameObject obj = UnityEngine.Object.Instantiate(brS_.roomMainObject, new Vector3(0f, 0f, 0f), Quaternion.identity);
			roomScript component = obj.GetComponent<roomScript>();
			obj.name = "Room_" + array2[num7];
			component.myID = array2[num7];
			component.typ = array2[1 + num7];
			component.taskID = array2[2 + num7];
			component.serverplatzUsed = array2[3 + num7];
			component.leitenderGamedesigner = array2[4 + num7];
			component.leitenderTechniker = array2[5 + num7];
			if (num2 > 6)
			{
				component.serverReservieren = array2[6 + num7];
			}
			component.myName = array3[num9];
			component.pause = array4[num8];
			component.lockKI = array4[1 + num8];
			component.serverDown = array4[2 + num8];
			component.uiPos = brS_.FindUiPositionExtern(component.myID);
			obj.transform.position = new Vector3(component.uiPos.x, 0f, component.uiPos.z);
			SetRoomScripts(component);
		}
		mapS_.CreateWalls(-1);
	}

	private void SaveMuell(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Muell");
		int num = array.Length;
		writer.Write<int>("anzMuell", num);
		int[] array2 = new int[num];
		float[] array3 = new float[4 * num];
		for (int i = 0; i < array.Length; i++)
		{
			muellScript component = array[i].GetComponent<muellScript>();
			int num2 = i * (array2.Length / num);
			int num3 = i * (array3.Length / num);
			array2[num2] = component.myGFXSlot;
			array3[num3] = array[i].transform.position.x;
			array3[1 + num3] = array[i].transform.position.y;
			array3[2 + num3] = array[i].transform.position.z;
			array3[3 + num3] = array[i].transform.eulerAngles.y;
		}
		writer.Write<int[]>("muell_I", array2);
		writer.Write<float[]>("muell_F", array3);
	}

	private void LoadMuell(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzMuell", -1);
		Debug.Log("Load: (anzMuell) " + num);
		if (num > 0)
		{
			long[] array = new long[0];
			int[] array2 = new int[0];
			float[] array3 = new float[0];
			string[] array4 = new string[0];
			bool[] array5 = new bool[0];
			array2 = reader.Read<int[]>("muell_I");
			array3 = reader.Read<float[]>("muell_F");
			int num2 = array2.Length / num;
			int num3 = array3.Length / num;
			int num4 = array5.Length / num;
			int num5 = array4.Length / num;
			int num6 = array.Length / num;
			for (int i = 0; i < num; i++)
			{
				int num7 = i * num2;
				int num8 = i * num3;
				mS_.CreateMuell(array2[num7], array2[num7], new Vector3(array3[num8], array3[1 + num8], array3[2 + num8]));
			}
		}
	}

	private void SaveTasks(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Task");
		int num = array.Length;
		writer.Write<int>("anzTasks", num);
		int[] array2 = new int[20 * num];
		bool[] array3 = new bool[20 * num];
		float[] array4 = new float[20 * num];
		for (int i = 0; i < array.Length; i++)
		{
			int num2 = i * 20;
			int num3 = i * 20;
			int num4 = i * 20;
			taskForschung component = array[i].GetComponent<taskForschung>();
			if ((bool)component)
			{
				array2[num2] = 0;
				array2[1 + num2] = component.myID;
				array2[2 + num2] = component.typ;
				array2[3 + num2] = component.slot;
				array3[num3] = component.automatic;
				array3[1 + num3] = component.automaticWait;
				array3[2 + num3] = component.autoForschung;
				array3[3 + num3] = component.kategorie[0];
				array3[4 + num3] = component.kategorie[1];
				array3[5 + num3] = component.kategorie[2];
				array3[6 + num3] = component.kategorie[3];
				array3[7 + num3] = component.kategorie[4];
				array3[8 + num3] = component.kategorie[5];
				array3[9 + num3] = component.kategorie[6];
			}
			taskEngine component2 = array[i].GetComponent<taskEngine>();
			if ((bool)component2)
			{
				array2[num2] = 1;
				array2[1 + num2] = component2.myID;
				array2[2 + num2] = component2.engineID;
			}
			taskGame component3 = array[i].GetComponent<taskGame>();
			if ((bool)component3)
			{
				array2[num2] = 2;
				array2[1 + num2] = component3.myID;
				array2[2 + num2] = component3.gameID;
				array2[3 + num2] = component3.leitenderDesignerID;
				array3[num3] = component3.randomEvent;
			}
			taskUnterstuetzen component4 = array[i].GetComponent<taskUnterstuetzen>();
			if ((bool)component4)
			{
				array2[num2] = 3;
				array2[1 + num2] = component4.myID;
				array2[2 + num2] = component4.roomID;
			}
			taskMarketing component5 = array[i].GetComponent<taskMarketing>();
			if ((bool)component5)
			{
				array2[num2] = 4;
				array2[1 + num2] = component5.myID;
				array2[2 + num2] = component5.typ;
				array2[3 + num2] = component5.targetID;
				array2[4 + num2] = component5.kampagne;
				array3[num3] = component5.automatic;
				array3[1 + num3] = component5.stopAutomatic;
				array3[2 + num3] = component5.disableWarten;
				array4[num4] = component5.points;
				array4[1 + num4] = component5.pointsLeft;
			}
			taskTraining component6 = array[i].GetComponent<taskTraining>();
			if ((bool)component6)
			{
				array2[num2] = 5;
				array2[1 + num2] = component6.myID;
				array2[2 + num2] = component6.slot;
				array3[num3] = component6.automatic;
				array4[num4] = component6.points;
				array4[1 + num4] = component6.pointsLeft;
			}
			taskContractWork component7 = array[i].GetComponent<taskContractWork>();
			if ((bool)component7)
			{
				array2[num2] = 6;
				array2[1 + num2] = component7.myID;
				array2[2 + num2] = component7.contractID;
				array3[num3] = component7.automatic;
				array3[1 + num3] = component7.automaticWait;
				array4[num4] = component7.points;
				array4[1 + num4] = component7.pointsLeft;
			}
			taskUpdate component8 = array[i].GetComponent<taskUpdate>();
			if ((bool)component8)
			{
				array2[num2] = 7;
				array2[1 + num2] = component8.myID;
				array2[2 + num2] = component8.targetID;
				array2[3 + num2] = component8.devCosts;
				array2[4 + num2] = component8.pointsGameplay;
				array2[5 + num2] = component8.pointsSound;
				array2[6 + num2] = component8.pointsGrafik;
				array2[7 + num2] = component8.pointsTechnik;
				array2[8 + num2] = component8.pointsBugs;
				array2[9 + num2] = component8.autoAmount;
				array3[num3] = component8.sprachen[0];
				array3[1 + num3] = component8.sprachen[1];
				array3[2 + num3] = component8.sprachen[2];
				array3[3 + num3] = component8.sprachen[3];
				array3[4 + num3] = component8.sprachen[4];
				array3[5 + num3] = component8.sprachen[5];
				array3[6 + num3] = component8.sprachen[6];
				array3[7 + num3] = component8.sprachen[7];
				array3[8 + num3] = component8.sprachen[8];
				array3[9 + num3] = component8.sprachen[9];
				array3[10 + num3] = component8.sprachen[10];
				array3[11 + num3] = component8.automatic;
				array4[num4] = component8.points;
				array4[1 + num4] = component8.pointsLeft;
				array4[2 + num4] = component8.quality;
			}
			taskFankampagne component9 = array[i].GetComponent<taskFankampagne>();
			if ((bool)component9)
			{
				array2[num2] = 8;
				array2[1 + num2] = component9.myID;
				array2[2 + num2] = component9.kampagne;
				array3[num3] = component9.automatic;
				array4[num4] = component9.points;
				array4[1 + num4] = component9.pointsLeft;
			}
			taskSupport component10 = array[i].GetComponent<taskSupport>();
			if ((bool)component10)
			{
				array2[num2] = 9;
				array2[1 + num2] = component10.myID;
			}
			taskBugfixing component11 = array[i].GetComponent<taskBugfixing>();
			if ((bool)component11)
			{
				array2[num2] = 10;
				array2[1 + num2] = component11.myID;
				array2[2 + num2] = component11.targetID;
				array4[num4] = component11.points;
				array4[1 + num4] = component11.pointsLeft;
			}
			taskGameplayVerbessern component12 = array[i].GetComponent<taskGameplayVerbessern>();
			if ((bool)component12)
			{
				array2[num2] = 11;
				array2[1 + num2] = component12.myID;
				array2[2 + num2] = component12.targetID;
				array2[3 + num2] = component12.aktuellerAdd;
				array4[num4] = component12.points;
				array4[1 + num4] = component12.pointsLeft;
				array3[num3] = component12.adds[0];
				array3[1 + num3] = component12.adds[1];
				array3[2 + num3] = component12.adds[2];
				array3[3 + num3] = component12.adds[3];
				array3[4 + num3] = component12.adds[4];
				array3[5 + num3] = component12.adds[5];
				array3[6 + num3] = component12.autoBugfix;
			}
			taskGrafikVerbessern component13 = array[i].GetComponent<taskGrafikVerbessern>();
			if ((bool)component13)
			{
				array2[num2] = 12;
				array2[1 + num2] = component13.myID;
				array2[2 + num2] = component13.targetID;
				array2[3 + num2] = component13.aktuellerAdd;
				array4[num4] = component13.points;
				array4[1 + num4] = component13.pointsLeft;
				array3[num3] = component13.adds[0];
				array3[1 + num3] = component13.adds[1];
				array3[2 + num3] = component13.adds[2];
				array3[3 + num3] = component13.adds[3];
				array3[4 + num3] = component13.adds[4];
				array3[5 + num3] = component13.adds[5];
			}
			taskSoundVerbessern component14 = array[i].GetComponent<taskSoundVerbessern>();
			if ((bool)component14)
			{
				array2[num2] = 13;
				array2[1 + num2] = component14.myID;
				array2[2 + num2] = component14.targetID;
				array2[3 + num2] = component14.aktuellerAdd;
				array4[num4] = component14.points;
				array4[1 + num4] = component14.pointsLeft;
				array3[num3] = component14.adds[0];
				array3[1 + num3] = component14.adds[1];
				array3[2 + num3] = component14.adds[2];
				array3[3 + num3] = component14.adds[3];
				array3[4 + num3] = component14.adds[4];
				array3[5 + num3] = component14.adds[5];
			}
			taskAnimationVerbessern component15 = array[i].GetComponent<taskAnimationVerbessern>();
			if ((bool)component15)
			{
				array2[num2] = 14;
				array2[1 + num2] = component15.myID;
				array2[2 + num2] = component15.targetID;
				array2[3 + num2] = component15.aktuellerAdd;
				array4[num4] = component15.points;
				array4[1 + num4] = component15.pointsLeft;
				array3[num3] = component15.adds[0];
				array3[1 + num3] = component15.adds[1];
				array3[2 + num3] = component15.adds[2];
				array3[3 + num3] = component15.adds[3];
				array3[4 + num3] = component15.adds[4];
				array3[5 + num3] = component15.adds[5];
			}
			taskSpielbericht component16 = array[i].GetComponent<taskSpielbericht>();
			if ((bool)component16)
			{
				array2[num2] = 15;
				array2[1 + num2] = component16.myID;
				array2[2 + num2] = component16.targetID;
				array4[num4] = component16.points;
				array4[1 + num4] = component16.pointsLeft;
				array3[num3] = component16.automatic;
				array3[1 + num3] = component16.automaticWait;
			}
			taskProduction component17 = array[i].GetComponent<taskProduction>();
			if ((bool)component17)
			{
				array2[num2] = 16;
				array2[1 + num2] = component17.myID;
				array2[2 + num2] = component17.targetID;
				array2[3 + num2] = component17.amountStandard;
				array2[4 + num2] = component17.amountDeluxe;
				array2[5 + num2] = component17.amountCollectors;
				array2[6 + num2] = component17.gesamtProduktion;
				array3[num3] = component17.automatic;
				array3[1 + num3] = component17.produceAutomatikAllGames;
			}
			taskMarktforschung component18 = array[i].GetComponent<taskMarktforschung>();
			if ((bool)component18)
			{
				array2[num2] = 17;
				array2[1 + num2] = component18.myID;
				array4[num4] = component18.points;
				array4[1 + num4] = component18.pointsLeft;
			}
			taskPolishing component19 = array[i].GetComponent<taskPolishing>();
			if ((bool)component19)
			{
				array2[num2] = 18;
				array2[1 + num2] = component19.myID;
				array2[2 + num2] = component19.targetID;
				array4[num4] = component19.points;
				array4[1 + num4] = component19.pointsLeft;
			}
			taskMarketingSpezial component20 = array[i].GetComponent<taskMarketingSpezial>();
			if ((bool)component20)
			{
				array2[num2] = 19;
				array2[1 + num2] = component20.myID;
				array2[2 + num2] = component20.targetID;
				array2[3 + num2] = component20.kampagne;
				array4[num4] = component20.points;
				array4[1 + num4] = component20.pointsLeft;
			}
			taskF2PUpdate component21 = array[i].GetComponent<taskF2PUpdate>();
			if ((bool)component21)
			{
				array2[num2] = 20;
				array2[1 + num2] = component21.myID;
				array2[2 + num2] = component21.targetID;
				array2[3 + num2] = component21.devCosts;
				array2[4 + num2] = component21.autoAmount;
				array3[num3] = component21.automatic;
				array4[num4] = component21.points;
				array4[1 + num4] = component21.pointsLeft;
				array4[2 + num4] = component21.quality;
			}
			taskArcadeProduction component22 = array[i].GetComponent<taskArcadeProduction>();
			if ((bool)component22)
			{
				array2[num2] = 21;
				array2[1 + num2] = component22.myID;
				array2[2 + num2] = component22.targetID;
				array3[num3] = component22.produceAutomatikAllGames;
				array4[num4] = component22.points;
				array4[1 + num4] = component22.pointsLeft;
			}
			taskKonsole component23 = array[i].GetComponent<taskKonsole>();
			if ((bool)component23)
			{
				array2[num2] = 22;
				array2[1 + num2] = component23.myID;
				array2[2 + num2] = component23.konsoleID;
				array2[3 + num2] = component23.leitenderTechnikerID;
				array2[4 + num2] = component23.proKonsoleID;
			}
			taskContractWait component24 = array[i].GetComponent<taskContractWait>();
			if ((bool)component24)
			{
				array2[num2] = 23;
				array2[1 + num2] = component24.myID;
				array2[2 + num2] = component24.art;
			}
			taskWait component25 = array[i].GetComponent<taskWait>();
			if ((bool)component25)
			{
				array2[num2] = 24;
				array2[1 + num2] = component25.myID;
				array2[2 + num2] = component25.art;
			}
			taskMitarbeitersuche component26 = array[i].GetComponent<taskMitarbeitersuche>();
			if ((bool)component26)
			{
				array2[num2] = 25;
				array2[1 + num2] = component26.myID;
				array2[2 + num2] = component26.beruf;
				array2[3 + num2] = component26.berufserfahrung;
				array2[4 + num2] = component26.perk;
				array2[5 + num2] = component26.geschlecht;
				array3[num3] = component26.automatic;
				array3[1 + num3] = component26.noBadPerks;
				array4[num4] = component26.points;
				array4[1 + num4] = component26.pointsLeft;
			}
			taskFanshop component27 = array[i].GetComponent<taskFanshop>();
			if ((bool)component27)
			{
				array2[num2] = 26;
				array2[1 + num2] = component27.myID;
				array2[2 + num2] = component27.verdienst;
				array2[3 + num2] = component27.bestellungen[0];
				array2[4 + num2] = component27.bestellungen[1];
				array2[5 + num2] = component27.bestellungen[2];
				array2[6 + num2] = component27.bestellungen[3];
				array2[7 + num2] = component27.bestellungen[4];
				array2[8 + num2] = component27.bestellungen[5];
				array2[9 + num2] = component27.bestellungen[6];
				array2[10 + num2] = component27.bestellungen[7];
			}
			taskForschungWait component28 = array[i].GetComponent<taskForschungWait>();
			if ((bool)component28)
			{
				array2[num2] = 27;
				array2[1 + num2] = component28.myID;
				array2[2 + num2] = component28.typ;
			}
			taskKonsoleReduceCosts component29 = array[i].GetComponent<taskKonsoleReduceCosts>();
			if ((bool)component29)
			{
				array2[num2] = 28;
				array2[1 + num2] = component29.myID;
				array2[2 + num2] = component29.targetID;
				array2[3 + num2] = component29.aktuellerAdd;
				array4[num4] = component29.points;
				array4[1 + num4] = component29.pointsLeft;
				array3[num3] = component29.adds[0];
				array3[1 + num3] = component29.adds[1];
				array3[2 + num3] = component29.adds[2];
				array3[3 + num3] = component29.adds[3];
				array3[4 + num3] = component29.adds[4];
				array3[5 + num3] = component29.adds[5];
				array3[6 + num3] = component29.adds[6];
				array3[7 + num3] = component29.adds[7];
				array3[8 + num3] = component29.adds[8];
			}
			taskKonsoleHaltbarkeit component30 = array[i].GetComponent<taskKonsoleHaltbarkeit>();
			if ((bool)component30)
			{
				array2[num2] = 29;
				array2[1 + num2] = component30.myID;
				array2[2 + num2] = component30.targetID;
				array2[3 + num2] = component30.aktuellerAdd;
				array4[num4] = component30.points;
				array4[1 + num4] = component30.pointsLeft;
				array3[num3] = component30.adds[0];
				array3[1 + num3] = component30.adds[1];
				array3[2 + num3] = component30.adds[2];
				array3[3 + num3] = component30.adds[3];
				array3[4 + num3] = component30.adds[4];
				array3[5 + num3] = component30.adds[5];
				array3[6 + num3] = component30.adds[6];
				array3[7 + num3] = component30.adds[7];
				array3[8 + num3] = component30.adds[8];
			}
			taskAutoForschung component31 = array[i].GetComponent<taskAutoForschung>();
			if ((bool)component31)
			{
				array2[num2] = 30;
				array2[1 + num2] = component31.myID;
				array3[num3] = component31.kategorie[0];
				array3[1 + num3] = component31.kategorie[1];
				array3[2 + num3] = component31.kategorie[2];
				array3[3 + num3] = component31.kategorie[3];
				array3[4 + num3] = component31.kategorie[4];
				array3[5 + num3] = component31.kategorie[5];
				array3[6 + num3] = component31.kategorie[6];
			}
		}
		writer.Write<int[]>("task_I", array2);
		writer.Write<bool[]>("task_B", array3);
		writer.Write<float[]>("task_F", array4);
	}

	private void LoadTasks(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzTasks", -1);
		Debug.Log("Load: (anzTasks) " + num);
		if (num <= 0)
		{
			return;
		}
		int[] array = new int[20 * num];
		bool[] array2 = new bool[20 * num];
		float[] array3 = new float[20 * num];
		array = reader.Read<int[]>("task_I");
		array2 = reader.Read<bool[]>("task_B");
		array3 = reader.Read<float[]>("task_F");
		for (int i = 0; i < num; i++)
		{
			int num2 = i * 20;
			int num3 = i * 20;
			int num4 = i * 20;
			switch (array[num2])
			{
			case 0:
			{
				taskForschung component2 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[0]).GetComponent<taskForschung>();
				component2.myID = array[1 + num2];
				component2.typ = array[2 + num2];
				component2.slot = array[3 + num2];
				component2.automatic = array2[num3];
				component2.automaticWait = array2[1 + num3];
				component2.autoForschung = array2[2 + num3];
				component2.kategorie[0] = array2[3 + num3];
				component2.kategorie[1] = array2[4 + num3];
				component2.kategorie[2] = array2[5 + num3];
				component2.kategorie[3] = array2[6 + num3];
				component2.kategorie[4] = array2[7 + num3];
				component2.kategorie[5] = array2[8 + num3];
				component2.kategorie[6] = array2[9 + num3];
				component2.Init(fromSavegame: true);
				if (component2.myID <= 0)
				{
					UnityEngine.Object.Destroy(component2.gameObject);
				}
				break;
			}
			case 1:
			{
				taskEngine component19 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[1]).GetComponent<taskEngine>();
				component19.myID = array[1 + num2];
				component19.engineID = array[2 + num2];
				component19.Init(fromSavegame: true);
				if (component19.myID <= 0)
				{
					UnityEngine.Object.Destroy(component19.gameObject);
				}
				break;
			}
			case 2:
			{
				taskGame component24 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[2]).GetComponent<taskGame>();
				component24.myID = array[1 + num2];
				component24.gameID = array[2 + num2];
				component24.leitenderDesignerID = array[3 + num2];
				component24.randomEvent = array2[num3];
				component24.Init(fromSavegame: true);
				if (component24.myID <= 0)
				{
					UnityEngine.Object.Destroy(component24.gameObject);
				}
				break;
			}
			case 3:
			{
				taskUnterstuetzen component10 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[3]).GetComponent<taskUnterstuetzen>();
				component10.myID = array[1 + num2];
				component10.roomID = array[2 + num2];
				component10.Init(fromSavegame: true);
				if (component10.myID <= 0)
				{
					UnityEngine.Object.Destroy(component10.gameObject);
				}
				break;
			}
			case 4:
			{
				taskMarketing component28 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[4]).GetComponent<taskMarketing>();
				component28.myID = array[1 + num2];
				component28.typ = array[2 + num2];
				component28.targetID = array[3 + num2];
				component28.kampagne = array[4 + num2];
				component28.automatic = array2[num3];
				component28.stopAutomatic = array2[1 + num3];
				component28.disableWarten = array2[2 + num3];
				component28.points = array3[num4];
				component28.pointsLeft = array3[1 + num4];
				component28.Init(fromSavegame: true);
				if (component28.myID <= 0)
				{
					UnityEngine.Object.Destroy(component28.gameObject);
				}
				break;
			}
			case 5:
			{
				taskTraining component20 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[5]).GetComponent<taskTraining>();
				component20.myID = array[1 + num2];
				component20.slot = array[2 + num2];
				component20.automatic = array2[num3];
				component20.points = array3[num4];
				component20.pointsLeft = array3[1 + num4];
				component20.Init(fromSavegame: true);
				if (component20.myID <= 0)
				{
					UnityEngine.Object.Destroy(component20.gameObject);
				}
				break;
			}
			case 6:
			{
				taskContractWork component14 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[6]).GetComponent<taskContractWork>();
				component14.myID = array[1 + num2];
				component14.contractID = array[2 + num2];
				component14.automatic = array2[num3];
				component14.automaticWait = array2[1 + num3];
				component14.points = array3[num4];
				component14.pointsLeft = array3[1 + num4];
				component14.Init(fromSavegame: true);
				if (component14.myID <= 0)
				{
					UnityEngine.Object.Destroy(component14.gameObject);
				}
				break;
			}
			case 7:
			{
				taskUpdate component6 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[7]).GetComponent<taskUpdate>();
				component6.myID = array[1 + num2];
				component6.targetID = array[2 + num2];
				component6.devCosts = array[3 + num2];
				component6.pointsGameplay = array[4 + num2];
				component6.pointsSound = array[5 + num2];
				component6.pointsGrafik = array[6 + num2];
				component6.pointsTechnik = array[7 + num2];
				component6.pointsBugs = array[8 + num2];
				component6.autoAmount = array[9 + num2];
				component6.sprachen[0] = array2[num3];
				component6.sprachen[1] = array2[1 + num3];
				component6.sprachen[2] = array2[2 + num3];
				component6.sprachen[3] = array2[3 + num3];
				component6.sprachen[4] = array2[4 + num3];
				component6.sprachen[5] = array2[5 + num3];
				component6.sprachen[6] = array2[6 + num3];
				component6.sprachen[7] = array2[7 + num3];
				component6.sprachen[8] = array2[8 + num3];
				component6.sprachen[9] = array2[9 + num3];
				component6.sprachen[10] = array2[10 + num3];
				component6.automatic = array2[11 + num3];
				component6.points = array3[num4];
				component6.pointsLeft = array3[1 + num4];
				component6.quality = array3[2 + num4];
				component6.Init(fromSavegame: true);
				if (component6.myID <= 0)
				{
					UnityEngine.Object.Destroy(component6.gameObject);
				}
				break;
			}
			case 8:
			{
				taskFankampagne component30 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[8]).GetComponent<taskFankampagne>();
				component30.myID = array[1 + num2];
				component30.kampagne = array[2 + num2];
				component30.automatic = array2[num3];
				component30.points = array3[num4];
				component30.pointsLeft = array3[1 + num4];
				component30.Init(fromSavegame: true);
				if (component30.myID <= 0)
				{
					UnityEngine.Object.Destroy(component30.gameObject);
				}
				break;
			}
			case 9:
			{
				taskSupport component26 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[9]).GetComponent<taskSupport>();
				component26.myID = array[1 + num2];
				component26.Init(fromSavegame: true);
				if (component26.myID <= 0)
				{
					UnityEngine.Object.Destroy(component26.gameObject);
				}
				break;
			}
			case 10:
			{
				taskBugfixing component22 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[10]).GetComponent<taskBugfixing>();
				component22.myID = array[1 + num2];
				component22.targetID = array[2 + num2];
				component22.points = array3[num4];
				component22.pointsLeft = array3[1 + num4];
				component22.Init(fromSavegame: true);
				if (component22.myID <= 0)
				{
					UnityEngine.Object.Destroy(component22.gameObject);
				}
				break;
			}
			case 11:
			{
				taskGameplayVerbessern component18 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[11]).GetComponent<taskGameplayVerbessern>();
				component18.myID = array[1 + num2];
				component18.targetID = array[2 + num2];
				component18.aktuellerAdd = array[3 + num2];
				component18.points = array3[num4];
				component18.pointsLeft = array3[1 + num4];
				component18.adds[0] = array2[num3];
				component18.adds[1] = array2[1 + num3];
				component18.adds[2] = array2[2 + num3];
				component18.adds[3] = array2[3 + num3];
				component18.adds[4] = array2[4 + num3];
				component18.adds[5] = array2[5 + num3];
				component18.autoBugfix = array2[6 + num3];
				component18.Init(fromSavegame: true);
				if (component18.myID <= 0)
				{
					UnityEngine.Object.Destroy(component18.gameObject);
				}
				break;
			}
			case 12:
			{
				taskGrafikVerbessern component16 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[12]).GetComponent<taskGrafikVerbessern>();
				component16.myID = array[1 + num2];
				component16.targetID = array[2 + num2];
				component16.aktuellerAdd = array[3 + num2];
				component16.points = array3[num4];
				component16.pointsLeft = array3[1 + num4];
				component16.adds[0] = array2[num3];
				component16.adds[1] = array2[1 + num3];
				component16.adds[2] = array2[2 + num3];
				component16.adds[3] = array2[3 + num3];
				component16.adds[4] = array2[4 + num3];
				component16.adds[5] = array2[5 + num3];
				component16.Init(fromSavegame: true);
				if (component16.myID <= 0)
				{
					UnityEngine.Object.Destroy(component16.gameObject);
				}
				break;
			}
			case 13:
			{
				taskSoundVerbessern component12 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[13]).GetComponent<taskSoundVerbessern>();
				component12.myID = array[1 + num2];
				component12.targetID = array[2 + num2];
				component12.aktuellerAdd = array[3 + num2];
				component12.points = array3[num4];
				component12.pointsLeft = array3[1 + num4];
				component12.adds[0] = array2[num3];
				component12.adds[1] = array2[1 + num3];
				component12.adds[2] = array2[2 + num3];
				component12.adds[3] = array2[3 + num3];
				component12.adds[4] = array2[4 + num3];
				component12.adds[5] = array2[5 + num3];
				component12.Init(fromSavegame: true);
				if (component12.myID <= 0)
				{
					UnityEngine.Object.Destroy(component12.gameObject);
				}
				break;
			}
			case 14:
			{
				taskAnimationVerbessern component8 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[14]).GetComponent<taskAnimationVerbessern>();
				component8.myID = array[1 + num2];
				component8.targetID = array[2 + num2];
				component8.aktuellerAdd = array[3 + num2];
				component8.points = array3[num4];
				component8.pointsLeft = array3[1 + num4];
				component8.adds[0] = array2[num3];
				component8.adds[1] = array2[1 + num3];
				component8.adds[2] = array2[2 + num3];
				component8.adds[3] = array2[3 + num3];
				component8.adds[4] = array2[4 + num3];
				component8.adds[5] = array2[5 + num3];
				component8.Init(fromSavegame: true);
				if (component8.myID <= 0)
				{
					UnityEngine.Object.Destroy(component8.gameObject);
				}
				break;
			}
			case 15:
			{
				taskSpielbericht component4 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[15]).GetComponent<taskSpielbericht>();
				component4.myID = array[1 + num2];
				component4.targetID = array[2 + num2];
				component4.points = array3[num4];
				component4.pointsLeft = array3[1 + num4];
				component4.automatic = array2[num3];
				component4.automaticWait = array2[1 + num3];
				component4.Init(fromSavegame: true);
				if (component4.myID <= 0)
				{
					UnityEngine.Object.Destroy(component4.gameObject);
				}
				break;
			}
			case 16:
			{
				taskProduction component31 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[16]).GetComponent<taskProduction>();
				component31.myID = array[1 + num2];
				component31.targetID = array[2 + num2];
				component31.amountStandard = array[3 + num2];
				component31.amountDeluxe = array[4 + num2];
				component31.amountCollectors = array[5 + num2];
				component31.gesamtProduktion = array[6 + num2];
				component31.automatic = array2[num3];
				component31.produceAutomatikAllGames = array2[1 + num3];
				component31.Init(fromSavegame: true);
				if (component31.myID <= 0)
				{
					UnityEngine.Object.Destroy(component31.gameObject);
				}
				break;
			}
			case 17:
			{
				taskMarktforschung component29 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[17]).GetComponent<taskMarktforschung>();
				component29.myID = array[1 + num2];
				component29.points = array3[num4];
				component29.pointsLeft = array3[1 + num4];
				component29.Init(fromSavegame: true);
				if (component29.myID <= 0)
				{
					UnityEngine.Object.Destroy(component29.gameObject);
				}
				break;
			}
			case 18:
			{
				taskPolishing component27 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[18]).GetComponent<taskPolishing>();
				component27.myID = array[1 + num2];
				component27.targetID = array[2 + num2];
				component27.points = array3[num4];
				component27.pointsLeft = array3[1 + num4];
				component27.Init(fromSavegame: true);
				if (component27.myID <= 0)
				{
					UnityEngine.Object.Destroy(component27.gameObject);
				}
				break;
			}
			case 19:
			{
				taskMarketingSpezial component25 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[19]).GetComponent<taskMarketingSpezial>();
				component25.myID = array[1 + num2];
				component25.targetID = array[2 + num2];
				component25.kampagne = array[3 + num2];
				component25.points = array3[num4];
				component25.pointsLeft = array3[1 + num4];
				component25.Init(fromSavegame: true);
				if (component25.myID <= 0)
				{
					UnityEngine.Object.Destroy(component25.gameObject);
				}
				break;
			}
			case 20:
			{
				taskF2PUpdate component23 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[20]).GetComponent<taskF2PUpdate>();
				component23.myID = array[1 + num2];
				component23.targetID = array[2 + num2];
				component23.devCosts = array[3 + num2];
				component23.autoAmount = array[4 + num2];
				component23.automatic = array2[num3];
				component23.points = array3[num4];
				component23.pointsLeft = array3[1 + num4];
				component23.quality = array3[2 + num4];
				component23.Init(fromSavegame: true);
				if (component23.myID <= 0)
				{
					UnityEngine.Object.Destroy(component23.gameObject);
				}
				break;
			}
			case 21:
			{
				taskArcadeProduction component21 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[21]).GetComponent<taskArcadeProduction>();
				component21.myID = array[1 + num2];
				component21.targetID = array[2 + num2];
				component21.produceAutomatikAllGames = array2[num3];
				component21.points = array3[num4];
				component21.pointsLeft = array3[1 + num4];
				component21.Init(fromSavegame: true);
				if (component21.myID <= 0)
				{
					UnityEngine.Object.Destroy(component21.gameObject);
				}
				break;
			}
			case 22:
			{
				taskKonsole component17 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[22]).GetComponent<taskKonsole>();
				component17.myID = array[1 + num2];
				component17.konsoleID = array[2 + num2];
				component17.leitenderTechnikerID = array[3 + num2];
				component17.proKonsoleID = array[4 + num2];
				if (component17.proKonsoleID == 0)
				{
					component17.proKonsoleID = -1;
				}
				component17.Init(fromSavegame: true);
				if (component17.myID <= 0)
				{
					UnityEngine.Object.Destroy(component17.gameObject);
				}
				break;
			}
			case 23:
			{
				taskContractWait component15 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[23]).GetComponent<taskContractWait>();
				component15.myID = array[1 + num2];
				component15.art = array[2 + num2];
				component15.Init(fromSavegame: true);
				if (component15.myID <= 0)
				{
					UnityEngine.Object.Destroy(component15.gameObject);
				}
				break;
			}
			case 24:
			{
				taskWait component13 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[24]).GetComponent<taskWait>();
				component13.myID = array[1 + num2];
				component13.art = array[2 + num2];
				component13.Init(fromSavegame: true);
				if (component13.myID <= 0)
				{
					UnityEngine.Object.Destroy(component13.gameObject);
				}
				break;
			}
			case 25:
			{
				taskMitarbeitersuche component11 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[25]).GetComponent<taskMitarbeitersuche>();
				component11.myID = array[1 + num2];
				component11.beruf = array[2 + num2];
				component11.berufserfahrung = array[3 + num2];
				component11.perk = array[4 + num2];
				component11.geschlecht = array[5 + num2];
				component11.automatic = array2[num3];
				component11.noBadPerks = array2[1 + num3];
				component11.points = array3[num4];
				component11.pointsLeft = array3[1 + num4];
				component11.Init(fromSavegame: true);
				if (component11.myID <= 0)
				{
					UnityEngine.Object.Destroy(component11.gameObject);
				}
				break;
			}
			case 26:
			{
				taskFanshop component9 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[26]).GetComponent<taskFanshop>();
				component9.myID = array[1 + num2];
				component9.verdienst = array[2 + num2];
				component9.bestellungen[0] = array[3 + num2];
				component9.bestellungen[1] = array[4 + num2];
				component9.bestellungen[2] = array[5 + num2];
				component9.bestellungen[3] = array[6 + num2];
				component9.bestellungen[4] = array[7 + num2];
				component9.bestellungen[5] = array[8 + num2];
				component9.bestellungen[6] = array[9 + num2];
				component9.bestellungen[7] = array[10 + num2];
				component9.Init(fromSavegame: true);
				if (component9.myID <= 0)
				{
					UnityEngine.Object.Destroy(component9.gameObject);
				}
				break;
			}
			case 27:
			{
				taskForschungWait component7 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[27]).GetComponent<taskForschungWait>();
				component7.myID = array[1 + num2];
				component7.typ = array[2 + num2];
				component7.Init(fromSavegame: true);
				if (component7.myID <= 0)
				{
					UnityEngine.Object.Destroy(component7.gameObject);
				}
				break;
			}
			case 28:
			{
				taskKonsoleReduceCosts component5 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[28]).GetComponent<taskKonsoleReduceCosts>();
				component5.myID = array[1 + num2];
				component5.targetID = array[2 + num2];
				component5.aktuellerAdd = array[3 + num2];
				component5.points = array3[num4];
				component5.pointsLeft = array3[1 + num4];
				component5.adds[0] = array2[num3];
				component5.adds[1] = array2[1 + num3];
				component5.adds[2] = array2[2 + num3];
				component5.adds[3] = array2[3 + num3];
				component5.adds[4] = array2[4 + num3];
				component5.adds[5] = array2[5 + num3];
				component5.adds[6] = array2[6 + num3];
				component5.adds[7] = array2[7 + num3];
				component5.adds[8] = array2[8 + num3];
				component5.Init(fromSavegame: true);
				if (component5.myID <= 0)
				{
					UnityEngine.Object.Destroy(component5.gameObject);
				}
				break;
			}
			case 29:
			{
				taskKonsoleHaltbarkeit component3 = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[29]).GetComponent<taskKonsoleHaltbarkeit>();
				component3.myID = array[1 + num2];
				component3.targetID = array[2 + num2];
				component3.aktuellerAdd = array[3 + num2];
				component3.points = array3[num4];
				component3.pointsLeft = array3[1 + num4];
				component3.adds[0] = array2[num3];
				component3.adds[1] = array2[1 + num3];
				component3.adds[2] = array2[2 + num3];
				component3.adds[3] = array2[3 + num3];
				component3.adds[4] = array2[4 + num3];
				component3.adds[5] = array2[5 + num3];
				component3.adds[6] = array2[6 + num3];
				component3.adds[7] = array2[7 + num3];
				component3.adds[8] = array2[8 + num3];
				component3.Init(fromSavegame: true);
				if (component3.myID <= 0)
				{
					UnityEngine.Object.Destroy(component3.gameObject);
				}
				break;
			}
			case 30:
			{
				taskAutoForschung component = UnityEngine.Object.Instantiate(guiMain_.uiTaskPrefabs[30]).GetComponent<taskAutoForschung>();
				component.myID = array[1 + num2];
				component.kategorie[0] = array2[num3];
				component.kategorie[1] = array2[1 + num3];
				component.kategorie[2] = array2[2 + num3];
				component.kategorie[3] = array2[3 + num3];
				component.kategorie[4] = array2[4 + num3];
				component.kategorie[5] = array2[5 + num3];
				component.kategorie[6] = array2[6 + num3];
				component.Init(fromSavegame: true);
				if (component.myID <= 0)
				{
					UnityEngine.Object.Destroy(component.gameObject);
				}
				break;
			}
			}
		}
	}

	private void SaveHardware(ES3Writer writer)
	{
		writer.Write<int[]>("hardware_TYP", hardware_.hardware_TYP);
		writer.Write<int[]>("hardware_RES_POINTS", hardware_.hardware_RES_POINTS);
		writer.Write<float[]>("hardware_RES_POINTS_LEFT", hardware_.hardware_RES_POINTS_LEFT);
		writer.Write<int[]>("hardware_PRICE", hardware_.hardware_PRICE);
		writer.Write<int[]>("hardware_DEV_COSTS", hardware_.hardware_DEV_COSTS);
		writer.Write<int[]>("hardware_TECH", hardware_.hardware_TECH);
		writer.Write<int[]>("hardware_DATE_YEAR", hardware_.hardware_DATE_YEAR);
		writer.Write<int[]>("hardware_DATE_MONTH", hardware_.hardware_DATE_MONTH);
		writer.Write<bool[]>("hardware_UNLOCK", hardware_.hardware_UNLOCK);
		writer.Write<string[]>("hardware_ICONFILE", hardware_.hardware_ICONFILE);
		writer.Write<int[]>("hardware_NEED1", hardware_.hardware_NEED1);
		writer.Write<int[]>("hardware_NEED2", hardware_.hardware_NEED2);
		writer.Write<bool[]>("hardware_ONLYSTATIONARY", hardware_.hardware_ONLYSTATIONARY);
		writer.Write<bool[]>("hardware_ONLYHANDHELD", hardware_.hardware_ONLYHANDHELD);
		writer.Write<string[]>("hardware_NAME_EN", hardware_.hardware_NAME_EN);
		writer.Write<string[]>("hardware_NAME_GE", hardware_.hardware_NAME_GE);
		writer.Write<string[]>("hardware_NAME_TU", hardware_.hardware_NAME_TU);
		writer.Write<string[]>("hardware_NAME_CH", hardware_.hardware_NAME_CH);
		writer.Write<string[]>("hardware_NAME_FR", hardware_.hardware_NAME_FR);
		writer.Write<string[]>("hardware_NAME_PB", hardware_.hardware_NAME_PB);
		writer.Write<string[]>("hardware_NAME_CT", hardware_.hardware_NAME_CT);
		writer.Write<string[]>("hardware_NAME_HU", hardware_.hardware_NAME_HU);
		writer.Write<string[]>("hardware_NAME_ES", hardware_.hardware_NAME_ES);
		writer.Write<string[]>("hardware_NAME_CZ", hardware_.hardware_NAME_CZ);
		writer.Write<string[]>("hardware_NAME_KO", hardware_.hardware_NAME_KO);
		writer.Write<string[]>("hardware_NAME_AR", hardware_.hardware_NAME_AR);
		writer.Write<string[]>("hardware_NAME_RU", hardware_.hardware_NAME_RU);
		writer.Write<string[]>("hardware_NAME_IT", hardware_.hardware_NAME_IT);
		writer.Write<string[]>("hardware_NAME_JA", hardware_.hardware_NAME_JA);
		writer.Write<string[]>("hardware_NAME_PL", hardware_.hardware_NAME_PL);
		writer.Write<string[]>("hardware_NAME_UA", hardware_.hardware_NAME_UA);
		writer.Write<string[]>("hardware_NAME_TH", hardware_.hardware_NAME_TH);
		writer.Write<string[]>("hardware_DESC_EN", hardware_.hardware_DESC_EN);
		writer.Write<string[]>("hardware_DESC_GE", hardware_.hardware_DESC_GE);
		writer.Write<string[]>("hardware_DESC_TU", hardware_.hardware_DESC_TU);
		writer.Write<string[]>("hardware_DESC_CH", hardware_.hardware_DESC_CH);
		writer.Write<string[]>("hardware_DESC_FR", hardware_.hardware_DESC_FR);
		writer.Write<string[]>("hardware_DESC_PB", hardware_.hardware_DESC_PB);
		writer.Write<string[]>("hardware_DESC_CT", hardware_.hardware_DESC_CT);
		writer.Write<string[]>("hardware_DESC_HU", hardware_.hardware_DESC_HU);
		writer.Write<string[]>("hardware_DESC_ES", hardware_.hardware_DESC_ES);
		writer.Write<string[]>("hardware_DESC_CZ", hardware_.hardware_DESC_CZ);
		writer.Write<string[]>("hardware_DESC_KO", hardware_.hardware_DESC_KO);
		writer.Write<string[]>("hardware_DESC_AR", hardware_.hardware_DESC_AR);
		writer.Write<string[]>("hardware_DESC_RU", hardware_.hardware_DESC_RU);
		writer.Write<string[]>("hardware_DESC_IT", hardware_.hardware_DESC_IT);
		writer.Write<string[]>("hardware_DESC_JA", hardware_.hardware_DESC_JA);
		writer.Write<string[]>("hardware_DESC_PL", hardware_.hardware_DESC_PL);
		writer.Write<string[]>("hardware_DESC_UA", hardware_.hardware_DESC_UA);
		writer.Write<string[]>("hardware_DESC_TH", hardware_.hardware_DESC_TH);
	}

	private void LoadHardware(ES3Reader reader, string filename)
	{
		hardware_.hardware_TYP = reader.Read<int[]>("hardware_TYP");
		hardware_.hardware_RES_POINTS = reader.Read<int[]>("hardware_RES_POINTS");
		hardware_.hardware_RES_POINTS_LEFT = reader.Read<float[]>("hardware_RES_POINTS_LEFT");
		hardware_.hardware_PRICE = reader.Read<int[]>("hardware_PRICE");
		hardware_.hardware_DEV_COSTS = reader.Read<int[]>("hardware_DEV_COSTS");
		hardware_.hardware_TECH = reader.Read<int[]>("hardware_TECH");
		hardware_.hardware_DATE_YEAR = reader.Read<int[]>("hardware_DATE_YEAR");
		hardware_.hardware_DATE_MONTH = reader.Read<int[]>("hardware_DATE_MONTH");
		hardware_.hardware_UNLOCK = reader.Read<bool[]>("hardware_UNLOCK");
		hardware_.hardware_ICONFILE = reader.Read<string[]>("hardware_ICONFILE");
		hardware_.hardware_NEED1 = reader.Read<int[]>("hardware_NEED1");
		hardware_.hardware_NEED2 = reader.Read<int[]>("hardware_NEED2");
		hardware_.hardware_ONLYSTATIONARY = reader.Read<bool[]>("hardware_ONLYSTATIONARY");
		hardware_.hardware_ONLYHANDHELD = reader.Read<bool[]>("hardware_ONLYHANDHELD");
		if (key_EN)
		{
			hardware_.hardware_NAME_EN = reader.Read<string[]>("hardware_NAME_EN");
		}
		if (key_GE)
		{
			hardware_.hardware_NAME_GE = reader.Read<string[]>("hardware_NAME_GE");
		}
		if (key_TU)
		{
			hardware_.hardware_NAME_TU = reader.Read<string[]>("hardware_NAME_TU");
		}
		if (key_CH)
		{
			hardware_.hardware_NAME_CH = reader.Read<string[]>("hardware_NAME_CH");
		}
		if (key_FR)
		{
			hardware_.hardware_NAME_FR = reader.Read<string[]>("hardware_NAME_FR");
		}
		if (key_PB)
		{
			hardware_.hardware_NAME_PB = reader.Read<string[]>("hardware_NAME_PB");
		}
		if (key_CT)
		{
			hardware_.hardware_NAME_CT = reader.Read<string[]>("hardware_NAME_CT");
		}
		if (key_HU)
		{
			hardware_.hardware_NAME_HU = reader.Read<string[]>("hardware_NAME_HU");
		}
		if (key_ES)
		{
			hardware_.hardware_NAME_ES = reader.Read<string[]>("hardware_NAME_ES");
		}
		if (key_CZ)
		{
			hardware_.hardware_NAME_CZ = reader.Read<string[]>("hardware_NAME_CZ");
		}
		if (key_KO)
		{
			hardware_.hardware_NAME_KO = reader.Read<string[]>("hardware_NAME_KO");
		}
		if (key_AR)
		{
			hardware_.hardware_NAME_AR = reader.Read<string[]>("hardware_NAME_AR");
		}
		if (key_RU)
		{
			hardware_.hardware_NAME_RU = reader.Read<string[]>("hardware_NAME_RU");
		}
		if (key_IT)
		{
			hardware_.hardware_NAME_IT = reader.Read<string[]>("hardware_NAME_IT");
		}
		if (key_JA)
		{
			hardware_.hardware_NAME_JA = reader.Read<string[]>("hardware_NAME_JA");
		}
		if (key_PL)
		{
			hardware_.hardware_NAME_PL = reader.Read<string[]>("hardware_NAME_PL");
		}
		if (key_UA)
		{
			hardware_.hardware_NAME_UA = reader.Read<string[]>("hardware_NAME_UA");
		}
		if (key_TH)
		{
			hardware_.hardware_NAME_TH = reader.Read<string[]>("hardware_NAME_TH");
		}
		if (key_EN)
		{
			hardware_.hardware_DESC_EN = reader.Read<string[]>("hardware_DESC_EN");
		}
		if (key_GE)
		{
			hardware_.hardware_DESC_GE = reader.Read<string[]>("hardware_DESC_GE");
		}
		if (key_TU)
		{
			hardware_.hardware_DESC_TU = reader.Read<string[]>("hardware_DESC_TU");
		}
		if (key_CH)
		{
			hardware_.hardware_DESC_CH = reader.Read<string[]>("hardware_DESC_CH");
		}
		if (key_FR)
		{
			hardware_.hardware_DESC_FR = reader.Read<string[]>("hardware_DESC_FR");
		}
		if (key_PB)
		{
			hardware_.hardware_DESC_PB = reader.Read<string[]>("hardware_DESC_PB");
		}
		if (key_CT)
		{
			hardware_.hardware_DESC_CT = reader.Read<string[]>("hardware_DESC_CT");
		}
		if (key_HU)
		{
			hardware_.hardware_DESC_HU = reader.Read<string[]>("hardware_DESC_HU");
		}
		if (key_ES)
		{
			hardware_.hardware_DESC_ES = reader.Read<string[]>("hardware_DESC_ES");
		}
		if (key_CZ)
		{
			hardware_.hardware_DESC_CZ = reader.Read<string[]>("hardware_DESC_CZ");
		}
		if (key_KO)
		{
			hardware_.hardware_DESC_KO = reader.Read<string[]>("hardware_DESC_KO");
		}
		if (key_AR)
		{
			hardware_.hardware_DESC_AR = reader.Read<string[]>("hardware_DESC_AR");
		}
		if (key_RU)
		{
			hardware_.hardware_DESC_RU = reader.Read<string[]>("hardware_DESC_RU");
		}
		if (key_IT)
		{
			hardware_.hardware_DESC_IT = reader.Read<string[]>("hardware_DESC_IT");
		}
		if (key_JA)
		{
			hardware_.hardware_DESC_JA = reader.Read<string[]>("hardware_DESC_JA");
		}
		if (key_PL)
		{
			hardware_.hardware_DESC_PL = reader.Read<string[]>("hardware_DESC_PL");
		}
		if (key_UA)
		{
			hardware_.hardware_DESC_UA = reader.Read<string[]>("hardware_DESC_UA");
		}
		if (key_TH)
		{
			hardware_.hardware_DESC_TH = reader.Read<string[]>("hardware_DESC_TH");
		}
		hardware_.Init();
	}

	private void SaveHardwareFeatures(ES3Writer writer)
	{
		writer.Write<int[]>("hardFeat_RES_POINTS", hardwareFeatures_.hardFeat_RES_POINTS);
		writer.Write<float[]>("hardFeat_RES_POINTS_LEFT", hardwareFeatures_.hardFeat_RES_POINTS_LEFT);
		writer.Write<int[]>("hardFeat_PRICE", hardwareFeatures_.hardFeat_PRICE);
		writer.Write<int[]>("hardFeat_DEV_COSTS", hardwareFeatures_.hardFeat_DEV_COSTS);
		writer.Write<int[]>("hardFeat_DATE_YEAR", hardwareFeatures_.hardFeat_DATE_YEAR);
		writer.Write<int[]>("hardFeat_DATE_MONTH", hardwareFeatures_.hardFeat_DATE_MONTH);
		writer.Write<bool[]>("hardFeat_UNLOCK", hardwareFeatures_.hardFeat_UNLOCK);
		writer.Write<string[]>("hardFeat_ICONFILE", hardwareFeatures_.hardFeat_ICONFILE);
		writer.Write<bool[]>("hardFeat_ONLYSTATIONARY", hardwareFeatures_.hardFeat_ONLYSTATIONARY);
		writer.Write<bool[]>("hardFeat_ONLYHANDHELD", hardwareFeatures_.hardFeat_ONLYHANDHELD);
		writer.Write<bool[]>("hardFeat_NEEDINTERNET", hardwareFeatures_.hardFeat_NEEDINTERNET);
		writer.Write<float[]>("hardFeat_QUALITY", hardwareFeatures_.hardFeat_QUALITY);
		writer.Write<string[]>("hardFeat_NAME_EN", hardwareFeatures_.hardFeat_NAME_EN);
		writer.Write<string[]>("hardFeat_NAME_GE", hardwareFeatures_.hardFeat_NAME_GE);
		writer.Write<string[]>("hardFeat_NAME_TU", hardwareFeatures_.hardFeat_NAME_TU);
		writer.Write<string[]>("hardFeat_NAME_CH", hardwareFeatures_.hardFeat_NAME_CH);
		writer.Write<string[]>("hardFeat_NAME_FR", hardwareFeatures_.hardFeat_NAME_FR);
		writer.Write<string[]>("hardFeat_NAME_PB", hardwareFeatures_.hardFeat_NAME_PB);
		writer.Write<string[]>("hardFeat_NAME_CT", hardwareFeatures_.hardFeat_NAME_CT);
		writer.Write<string[]>("hardFeat_NAME_HU", hardwareFeatures_.hardFeat_NAME_HU);
		writer.Write<string[]>("hardFeat_NAME_ES", hardwareFeatures_.hardFeat_NAME_ES);
		writer.Write<string[]>("hardFeat_NAME_CZ", hardwareFeatures_.hardFeat_NAME_CZ);
		writer.Write<string[]>("hardFeat_NAME_KO", hardwareFeatures_.hardFeat_NAME_KO);
		writer.Write<string[]>("hardFeat_NAME_AR", hardwareFeatures_.hardFeat_NAME_AR);
		writer.Write<string[]>("hardFeat_NAME_RU", hardwareFeatures_.hardFeat_NAME_RU);
		writer.Write<string[]>("hardFeat_NAME_IT", hardwareFeatures_.hardFeat_NAME_IT);
		writer.Write<string[]>("hardFeat_NAME_JA", hardwareFeatures_.hardFeat_NAME_JA);
		writer.Write<string[]>("hardFeat_NAME_PL", hardwareFeatures_.hardFeat_NAME_PL);
		writer.Write<string[]>("hardFeat_NAME_UA", hardwareFeatures_.hardFeat_NAME_UA);
		writer.Write<string[]>("hardFeat_NAME_TH", hardwareFeatures_.hardFeat_NAME_TH);
		writer.Write<string[]>("hardFeat_DESC_EN", hardwareFeatures_.hardFeat_DESC_EN);
		writer.Write<string[]>("hardFeat_DESC_GE", hardwareFeatures_.hardFeat_DESC_GE);
		writer.Write<string[]>("hardFeat_DESC_TU", hardwareFeatures_.hardFeat_DESC_TU);
		writer.Write<string[]>("hardFeat_DESC_CH", hardwareFeatures_.hardFeat_DESC_CH);
		writer.Write<string[]>("hardFeat_DESC_FR", hardwareFeatures_.hardFeat_DESC_FR);
		writer.Write<string[]>("hardFeat_DESC_PB", hardwareFeatures_.hardFeat_DESC_PB);
		writer.Write<string[]>("hardFeat_DESC_CT", hardwareFeatures_.hardFeat_DESC_CT);
		writer.Write<string[]>("hardFeat_DESC_HU", hardwareFeatures_.hardFeat_DESC_HU);
		writer.Write<string[]>("hardFeat_DESC_ES", hardwareFeatures_.hardFeat_DESC_ES);
		writer.Write<string[]>("hardFeat_DESC_CZ", hardwareFeatures_.hardFeat_DESC_CZ);
		writer.Write<string[]>("hardFeat_DESC_KO", hardwareFeatures_.hardFeat_DESC_KO);
		writer.Write<string[]>("hardFeat_DESC_AR", hardwareFeatures_.hardFeat_DESC_AR);
		writer.Write<string[]>("hardFeat_DESC_RU", hardwareFeatures_.hardFeat_DESC_RU);
		writer.Write<string[]>("hardFeat_DESC_IT", hardwareFeatures_.hardFeat_DESC_IT);
		writer.Write<string[]>("hardFeat_DESC_JA", hardwareFeatures_.hardFeat_DESC_JA);
		writer.Write<string[]>("hardFeat_DESC_PL", hardwareFeatures_.hardFeat_DESC_PL);
		writer.Write<string[]>("hardFeat_DESC_UA", hardwareFeatures_.hardFeat_DESC_UA);
		writer.Write<string[]>("hardFeat_DESC_TH", hardwareFeatures_.hardFeat_DESC_TH);
	}

	private void LoadHardwareFeatures(ES3Reader reader, string filename)
	{
		hardwareFeatures_.hardFeat_RES_POINTS = reader.Read<int[]>("hardFeat_RES_POINTS");
		hardwareFeatures_.hardFeat_RES_POINTS_LEFT = reader.Read<float[]>("hardFeat_RES_POINTS_LEFT");
		hardwareFeatures_.hardFeat_PRICE = reader.Read<int[]>("hardFeat_PRICE");
		hardwareFeatures_.hardFeat_DEV_COSTS = reader.Read<int[]>("hardFeat_DEV_COSTS");
		hardwareFeatures_.hardFeat_DATE_YEAR = reader.Read<int[]>("hardFeat_DATE_YEAR");
		hardwareFeatures_.hardFeat_DATE_MONTH = reader.Read<int[]>("hardFeat_DATE_MONTH");
		hardwareFeatures_.hardFeat_UNLOCK = reader.Read<bool[]>("hardFeat_UNLOCK");
		hardwareFeatures_.hardFeat_ICONFILE = reader.Read<string[]>("hardFeat_ICONFILE");
		hardwareFeatures_.hardFeat_ONLYSTATIONARY = reader.Read<bool[]>("hardFeat_ONLYSTATIONARY");
		hardwareFeatures_.hardFeat_ONLYHANDHELD = reader.Read<bool[]>("hardFeat_ONLYHANDHELD");
		hardwareFeatures_.hardFeat_NEEDINTERNET = reader.Read<bool[]>("hardFeat_NEEDINTERNET");
		hardwareFeatures_.hardFeat_QUALITY = reader.Read<float[]>("hardFeat_QUALITY");
		if (key_EN)
		{
			hardwareFeatures_.hardFeat_NAME_EN = reader.Read<string[]>("hardFeat_NAME_EN");
		}
		if (key_GE)
		{
			hardwareFeatures_.hardFeat_NAME_GE = reader.Read<string[]>("hardFeat_NAME_GE");
		}
		if (key_TU)
		{
			hardwareFeatures_.hardFeat_NAME_TU = reader.Read<string[]>("hardFeat_NAME_TU");
		}
		if (key_CH)
		{
			hardwareFeatures_.hardFeat_NAME_CH = reader.Read<string[]>("hardFeat_NAME_CH");
		}
		if (key_FR)
		{
			hardwareFeatures_.hardFeat_NAME_FR = reader.Read<string[]>("hardFeat_NAME_FR");
		}
		if (key_PB)
		{
			hardwareFeatures_.hardFeat_NAME_PB = reader.Read<string[]>("hardFeat_NAME_PB");
		}
		if (key_CT)
		{
			hardwareFeatures_.hardFeat_NAME_CT = reader.Read<string[]>("hardFeat_NAME_CT");
		}
		if (key_HU)
		{
			hardwareFeatures_.hardFeat_NAME_HU = reader.Read<string[]>("hardFeat_NAME_HU");
		}
		if (key_ES)
		{
			hardwareFeatures_.hardFeat_NAME_ES = reader.Read<string[]>("hardFeat_NAME_ES");
		}
		if (key_CZ)
		{
			hardwareFeatures_.hardFeat_NAME_CZ = reader.Read<string[]>("hardFeat_NAME_CZ");
		}
		if (key_KO)
		{
			hardwareFeatures_.hardFeat_NAME_KO = reader.Read<string[]>("hardFeat_NAME_KO");
		}
		if (key_AR)
		{
			hardwareFeatures_.hardFeat_NAME_AR = reader.Read<string[]>("hardFeat_NAME_AR");
		}
		if (key_RU)
		{
			hardwareFeatures_.hardFeat_NAME_RU = reader.Read<string[]>("hardFeat_NAME_RU");
		}
		if (key_IT)
		{
			hardwareFeatures_.hardFeat_NAME_IT = reader.Read<string[]>("hardFeat_NAME_IT");
		}
		if (key_JA)
		{
			hardwareFeatures_.hardFeat_NAME_JA = reader.Read<string[]>("hardFeat_NAME_JA");
		}
		if (key_PL)
		{
			hardwareFeatures_.hardFeat_NAME_PL = reader.Read<string[]>("hardFeat_NAME_PL");
		}
		if (key_UA)
		{
			hardwareFeatures_.hardFeat_NAME_UA = reader.Read<string[]>("hardFeat_NAME_UA");
		}
		if (key_TH)
		{
			hardwareFeatures_.hardFeat_NAME_TH = reader.Read<string[]>("hardFeat_NAME_TH");
		}
		if (key_EN)
		{
			hardwareFeatures_.hardFeat_DESC_EN = reader.Read<string[]>("hardFeat_DESC_EN");
		}
		if (key_GE)
		{
			hardwareFeatures_.hardFeat_DESC_GE = reader.Read<string[]>("hardFeat_DESC_GE");
		}
		if (key_TU)
		{
			hardwareFeatures_.hardFeat_DESC_TU = reader.Read<string[]>("hardFeat_DESC_TU");
		}
		if (key_CH)
		{
			hardwareFeatures_.hardFeat_DESC_CH = reader.Read<string[]>("hardFeat_DESC_CH");
		}
		if (key_FR)
		{
			hardwareFeatures_.hardFeat_DESC_FR = reader.Read<string[]>("hardFeat_DESC_FR");
		}
		if (key_PB)
		{
			hardwareFeatures_.hardFeat_DESC_PB = reader.Read<string[]>("hardFeat_DESC_PB");
		}
		if (key_CT)
		{
			hardwareFeatures_.hardFeat_DESC_CT = reader.Read<string[]>("hardFeat_DESC_CT");
		}
		if (key_HU)
		{
			hardwareFeatures_.hardFeat_DESC_HU = reader.Read<string[]>("hardFeat_DESC_HU");
		}
		if (key_ES)
		{
			hardwareFeatures_.hardFeat_DESC_ES = reader.Read<string[]>("hardFeat_DESC_ES");
		}
		if (key_CZ)
		{
			hardwareFeatures_.hardFeat_DESC_CZ = reader.Read<string[]>("hardFeat_DESC_CZ");
		}
		if (key_KO)
		{
			hardwareFeatures_.hardFeat_DESC_KO = reader.Read<string[]>("hardFeat_DESC_KO");
		}
		if (key_AR)
		{
			hardwareFeatures_.hardFeat_DESC_AR = reader.Read<string[]>("hardFeat_DESC_AR");
		}
		if (key_RU)
		{
			hardwareFeatures_.hardFeat_DESC_RU = reader.Read<string[]>("hardFeat_DESC_RU");
		}
		if (key_IT)
		{
			hardwareFeatures_.hardFeat_DESC_IT = reader.Read<string[]>("hardFeat_DESC_IT");
		}
		if (key_JA)
		{
			hardwareFeatures_.hardFeat_DESC_JA = reader.Read<string[]>("hardFeat_DESC_JA");
		}
		if (key_PL)
		{
			hardwareFeatures_.hardFeat_DESC_PL = reader.Read<string[]>("hardFeat_DESC_PL");
		}
		if (key_UA)
		{
			hardwareFeatures_.hardFeat_DESC_UA = reader.Read<string[]>("hardFeat_DESC_UA");
		}
		if (key_TH)
		{
			hardwareFeatures_.hardFeat_DESC_TH = reader.Read<string[]>("hardFeat_DESC_TH");
		}
		hardwareFeatures_.Init();
	}

	private void SaveEngineFeatures(ES3Writer writer)
	{
		writer.Write<int[]>("engineFeatures_TYP", eF_.engineFeatures_TYP);
		writer.Write<int[]>("engineFeatures_RES_POINTS", eF_.engineFeatures_RES_POINTS);
		writer.Write<float[]>("engineFeatures_RES_POINTS_LEFT", eF_.engineFeatures_RES_POINTS_LEFT);
		writer.Write<int[]>("engineFeatures_PRICE", eF_.engineFeatures_PRICE);
		writer.Write<int[]>("engineFeatures_DEV_COSTS", eF_.engineFeatures_DEV_COSTS);
		writer.Write<int[]>("engineFeatures_TECH", eF_.engineFeatures_TECH);
		writer.Write<int[]>("engineFeatures_DATE_YEAR", eF_.engineFeatures_DATE_YEAR);
		writer.Write<int[]>("engineFeatures_DATE_MONTH", eF_.engineFeatures_DATE_MONTH);
		writer.Write<int[]>("engineFeatures_GAMEPLAY", eF_.engineFeatures_GAMEPLAY);
		writer.Write<int[]>("engineFeatures_GRAPHIC", eF_.engineFeatures_GRAPHIC);
		writer.Write<int[]>("engineFeatures_SOUND", eF_.engineFeatures_SOUND);
		writer.Write<int[]>("engineFeatures_TECHNIK", eF_.engineFeatures_TECHNIK);
		writer.Write<int[]>("engineFeatures_LEVEL", eF_.engineFeatures_LEVEL);
		writer.Write<bool[]>("engineFeatures_UNLOCK", eF_.engineFeatures_UNLOCK);
		writer.Write<string[]>("engineFeatures_ICONFILE", eF_.engineFeatures_ICONFILE);
		writer.Write<string[]>("engineFeatures_NAME_EN", eF_.engineFeatures_NAME_EN);
		writer.Write<string[]>("engineFeatures_NAME_GE", eF_.engineFeatures_NAME_GE);
		writer.Write<string[]>("engineFeatures_NAME_TU", eF_.engineFeatures_NAME_TU);
		writer.Write<string[]>("engineFeatures_NAME_CH", eF_.engineFeatures_NAME_CH);
		writer.Write<string[]>("engineFeatures_NAME_FR", eF_.engineFeatures_NAME_FR);
		writer.Write<string[]>("engineFeatures_NAME_PB", eF_.engineFeatures_NAME_PB);
		writer.Write<string[]>("engineFeatures_NAME_CT", eF_.engineFeatures_NAME_CT);
		writer.Write<string[]>("engineFeatures_NAME_HU", eF_.engineFeatures_NAME_HU);
		writer.Write<string[]>("engineFeatures_NAME_ES", eF_.engineFeatures_NAME_ES);
		writer.Write<string[]>("engineFeatures_NAME_CZ", eF_.engineFeatures_NAME_CZ);
		writer.Write<string[]>("engineFeatures_NAME_KO", eF_.engineFeatures_NAME_KO);
		writer.Write<string[]>("engineFeatures_NAME_AR", eF_.engineFeatures_NAME_AR);
		writer.Write<string[]>("engineFeatures_NAME_RU", eF_.engineFeatures_NAME_RU);
		writer.Write<string[]>("engineFeatures_NAME_IT", eF_.engineFeatures_NAME_IT);
		writer.Write<string[]>("engineFeatures_NAME_JA", eF_.engineFeatures_NAME_JA);
		writer.Write<string[]>("engineFeatures_NAME_PL", eF_.engineFeatures_NAME_PL);
		writer.Write<string[]>("engineFeatures_NAME_UA", eF_.engineFeatures_NAME_UA);
		writer.Write<string[]>("engineFeatures_NAME_TH", eF_.engineFeatures_NAME_TH);
		writer.Write<string[]>("engineFeatures_DESC_EN", eF_.engineFeatures_DESC_EN);
		writer.Write<string[]>("engineFeatures_DESC_GE", eF_.engineFeatures_DESC_GE);
		writer.Write<string[]>("engineFeatures_DESC_TU", eF_.engineFeatures_DESC_TU);
		writer.Write<string[]>("engineFeatures_DESC_CH", eF_.engineFeatures_DESC_CH);
		writer.Write<string[]>("engineFeatures_DESC_FR", eF_.engineFeatures_DESC_FR);
		writer.Write<string[]>("engineFeatures_DESC_PB", eF_.engineFeatures_DESC_PB);
		writer.Write<string[]>("engineFeatures_DESC_CT", eF_.engineFeatures_DESC_CT);
		writer.Write<string[]>("engineFeatures_DESC_HU", eF_.engineFeatures_DESC_HU);
		writer.Write<string[]>("engineFeatures_DESC_ES", eF_.engineFeatures_DESC_ES);
		writer.Write<string[]>("engineFeatures_DESC_CZ", eF_.engineFeatures_DESC_CZ);
		writer.Write<string[]>("engineFeatures_DESC_KO", eF_.engineFeatures_DESC_KO);
		writer.Write<string[]>("engineFeatures_DESC_AR", eF_.engineFeatures_DESC_AR);
		writer.Write<string[]>("engineFeatures_DESC_RU", eF_.engineFeatures_DESC_RU);
		writer.Write<string[]>("engineFeatures_DESC_IT", eF_.engineFeatures_DESC_IT);
		writer.Write<string[]>("engineFeatures_DESC_JA", eF_.engineFeatures_DESC_JA);
		writer.Write<string[]>("engineFeatures_DESC_PL", eF_.engineFeatures_DESC_PL);
		writer.Write<string[]>("engineFeatures_DESC_UA", eF_.engineFeatures_DESC_UA);
		writer.Write<string[]>("engineFeatures_DESC_TH", eF_.engineFeatures_DESC_TH);
	}

	private void LoadEngineFeatures(ES3Reader reader, string filename)
	{
		eF_.engineFeatures_TYP = reader.Read<int[]>("engineFeatures_TYP");
		eF_.engineFeatures_RES_POINTS = reader.Read<int[]>("engineFeatures_RES_POINTS");
		eF_.engineFeatures_RES_POINTS_LEFT = reader.Read<float[]>("engineFeatures_RES_POINTS_LEFT");
		eF_.engineFeatures_PRICE = reader.Read<int[]>("engineFeatures_PRICE");
		eF_.engineFeatures_DEV_COSTS = reader.Read<int[]>("engineFeatures_DEV_COSTS");
		eF_.engineFeatures_TECH = reader.Read<int[]>("engineFeatures_TECH");
		eF_.engineFeatures_DATE_YEAR = reader.Read<int[]>("engineFeatures_DATE_YEAR");
		eF_.engineFeatures_DATE_MONTH = reader.Read<int[]>("engineFeatures_DATE_MONTH");
		eF_.engineFeatures_GAMEPLAY = reader.Read<int[]>("engineFeatures_GAMEPLAY");
		eF_.engineFeatures_GRAPHIC = reader.Read<int[]>("engineFeatures_GRAPHIC");
		eF_.engineFeatures_SOUND = reader.Read<int[]>("engineFeatures_SOUND");
		eF_.engineFeatures_TECHNIK = reader.Read<int[]>("engineFeatures_TECHNIK");
		eF_.engineFeatures_LEVEL = reader.Read<int[]>("engineFeatures_LEVEL");
		eF_.engineFeatures_UNLOCK = reader.Read<bool[]>("engineFeatures_UNLOCK");
		eF_.engineFeatures_ICONFILE = reader.Read<string[]>("engineFeatures_ICONFILE");
		if (key_EN)
		{
			eF_.engineFeatures_NAME_EN = reader.Read<string[]>("engineFeatures_NAME_EN");
		}
		if (key_GE)
		{
			eF_.engineFeatures_NAME_GE = reader.Read<string[]>("engineFeatures_NAME_GE");
		}
		if (key_TU)
		{
			eF_.engineFeatures_NAME_TU = reader.Read<string[]>("engineFeatures_NAME_TU");
		}
		if (key_CH)
		{
			eF_.engineFeatures_NAME_CH = reader.Read<string[]>("engineFeatures_NAME_CH");
		}
		if (key_FR)
		{
			eF_.engineFeatures_NAME_FR = reader.Read<string[]>("engineFeatures_NAME_FR");
		}
		if (key_PB)
		{
			eF_.engineFeatures_NAME_PB = reader.Read<string[]>("engineFeatures_NAME_PB");
		}
		if (key_CT)
		{
			eF_.engineFeatures_NAME_CT = reader.Read<string[]>("engineFeatures_NAME_CT");
		}
		if (key_HU)
		{
			eF_.engineFeatures_NAME_HU = reader.Read<string[]>("engineFeatures_NAME_HU");
		}
		if (key_ES)
		{
			eF_.engineFeatures_NAME_ES = reader.Read<string[]>("engineFeatures_NAME_ES");
		}
		if (key_CZ)
		{
			eF_.engineFeatures_NAME_CZ = reader.Read<string[]>("engineFeatures_NAME_CZ");
		}
		if (key_KO)
		{
			eF_.engineFeatures_NAME_KO = reader.Read<string[]>("engineFeatures_NAME_KO");
		}
		if (key_AR)
		{
			eF_.engineFeatures_NAME_AR = reader.Read<string[]>("engineFeatures_NAME_AR");
		}
		if (key_RU)
		{
			eF_.engineFeatures_NAME_RU = reader.Read<string[]>("engineFeatures_NAME_RU");
		}
		if (key_IT)
		{
			eF_.engineFeatures_NAME_IT = reader.Read<string[]>("engineFeatures_NAME_IT");
		}
		if (key_JA)
		{
			eF_.engineFeatures_NAME_JA = reader.Read<string[]>("engineFeatures_NAME_JA");
		}
		if (key_PL)
		{
			eF_.engineFeatures_NAME_PL = reader.Read<string[]>("engineFeatures_NAME_PL");
		}
		if (key_UA)
		{
			eF_.engineFeatures_NAME_UA = reader.Read<string[]>("engineFeatures_NAME_UA");
		}
		if (key_TH)
		{
			eF_.engineFeatures_NAME_TH = reader.Read<string[]>("engineFeatures_NAME_TH");
		}
		if (key_EN)
		{
			eF_.engineFeatures_DESC_EN = reader.Read<string[]>("engineFeatures_DESC_EN");
		}
		if (key_GE)
		{
			eF_.engineFeatures_DESC_GE = reader.Read<string[]>("engineFeatures_DESC_GE");
		}
		if (key_TU)
		{
			eF_.engineFeatures_DESC_TU = reader.Read<string[]>("engineFeatures_DESC_TU");
		}
		if (key_CH)
		{
			eF_.engineFeatures_DESC_CH = reader.Read<string[]>("engineFeatures_DESC_CH");
		}
		if (key_FR)
		{
			eF_.engineFeatures_DESC_FR = reader.Read<string[]>("engineFeatures_DESC_FR");
		}
		if (key_PB)
		{
			eF_.engineFeatures_DESC_PB = reader.Read<string[]>("engineFeatures_DESC_PB");
		}
		if (key_CT)
		{
			eF_.engineFeatures_DESC_CT = reader.Read<string[]>("engineFeatures_DESC_CT");
		}
		if (key_HU)
		{
			eF_.engineFeatures_DESC_HU = reader.Read<string[]>("engineFeatures_DESC_HU");
		}
		if (key_ES)
		{
			eF_.engineFeatures_DESC_ES = reader.Read<string[]>("engineFeatures_DESC_ES");
		}
		if (key_CZ)
		{
			eF_.engineFeatures_DESC_CZ = reader.Read<string[]>("engineFeatures_DESC_CZ");
		}
		if (key_KO)
		{
			eF_.engineFeatures_DESC_KO = reader.Read<string[]>("engineFeatures_DESC_KO");
		}
		if (key_AR)
		{
			eF_.engineFeatures_DESC_AR = reader.Read<string[]>("engineFeatures_DESC_AR");
		}
		if (key_RU)
		{
			eF_.engineFeatures_DESC_RU = reader.Read<string[]>("engineFeatures_DESC_RU");
		}
		if (key_IT)
		{
			eF_.engineFeatures_DESC_IT = reader.Read<string[]>("engineFeatures_DESC_IT");
		}
		if (key_JA)
		{
			eF_.engineFeatures_DESC_JA = reader.Read<string[]>("engineFeatures_DESC_JA");
		}
		if (key_PL)
		{
			eF_.engineFeatures_DESC_PL = reader.Read<string[]>("engineFeatures_DESC_PL");
		}
		if (key_UA)
		{
			eF_.engineFeatures_DESC_UA = reader.Read<string[]>("engineFeatures_DESC_UA");
		}
		if (key_TH)
		{
			eF_.engineFeatures_DESC_TH = reader.Read<string[]>("engineFeatures_DESC_TH");
		}
		eF_.Init();
	}

	private void SaveGameplayFeatures(ES3Writer writer)
	{
		writer.Write<int[]>("gameplayFeatures_TYP", gF_.gameplayFeatures_TYP);
		writer.Write<int[]>("gameplayFeatures_RES_POINTS", gF_.gameplayFeatures_RES_POINTS);
		writer.Write<float[]>("gameplayFeatures_RES_POINTS_LEFT", gF_.gameplayFeatures_RES_POINTS_LEFT);
		writer.Write<int[]>("gameplayFeatures_PRICE", gF_.gameplayFeatures_PRICE);
		writer.Write<int[]>("gameplayFeatures_DEV_COSTS", gF_.gameplayFeatures_DEV_COSTS);
		writer.Write<int[]>("gameplayFeatures_DATE_YEAR", gF_.gameplayFeatures_DATE_YEAR);
		writer.Write<int[]>("gameplayFeatures_DATE_MONTH", gF_.gameplayFeatures_DATE_MONTH);
		writer.Write<int[]>("gameplayFeatures_GAMEPLAY", gF_.gameplayFeatures_GAMEPLAY);
		writer.Write<int[]>("gameplayFeatures_GRAPHIC", gF_.gameplayFeatures_GRAPHIC);
		writer.Write<int[]>("gameplayFeatures_SOUND", gF_.gameplayFeatures_SOUND);
		writer.Write<int[]>("gameplayFeatures_TECHNIK", gF_.gameplayFeatures_TECHNIK);
		writer.Write<int[]>("gameplayFeatures_LEVEL", gF_.gameplayFeatures_LEVEL);
		writer.Write<int[]>("gameplayFeatures_NEED_GAMEPLAY_FEATURE", gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE);
		writer.Write<bool[]>("gameplayFeatures_INTERNET", gF_.gameplayFeatures_INTERNET);
		writer.Write<bool[]>("gameplayFeatures_UNLOCK", gF_.gameplayFeatures_UNLOCK);
		writer.Write<bool[,]>("gameplayFeatures_GOOD", gF_.gameplayFeatures_GOOD);
		writer.Write<bool[,]>("gameplayFeatures_BAD", gF_.gameplayFeatures_BAD);
		writer.Write<bool[,]>("gameplayFeatures_LOCKPLATFORM", gF_.gameplayFeatures_LOCKPLATFORM);
		writer.Write<string[]>("gameplayFeatures_NAME_EN", gF_.gameplayFeatures_NAME_EN);
		writer.Write<string[]>("gameplayFeatures_NAME_GE", gF_.gameplayFeatures_NAME_GE);
		writer.Write<string[]>("gameplayFeatures_NAME_TU", gF_.gameplayFeatures_NAME_TU);
		writer.Write<string[]>("gameplayFeatures_NAME_CH", gF_.gameplayFeatures_NAME_CH);
		writer.Write<string[]>("gameplayFeatures_NAME_FR", gF_.gameplayFeatures_NAME_FR);
		writer.Write<string[]>("gameplayFeatures_NAME_PB", gF_.gameplayFeatures_NAME_PB);
		writer.Write<string[]>("gameplayFeatures_NAME_CT", gF_.gameplayFeatures_NAME_CT);
		writer.Write<string[]>("gameplayFeatures_NAME_HU", gF_.gameplayFeatures_NAME_HU);
		writer.Write<string[]>("gameplayFeatures_NAME_ES", gF_.gameplayFeatures_NAME_ES);
		writer.Write<string[]>("gameplayFeatures_NAME_CZ", gF_.gameplayFeatures_NAME_CZ);
		writer.Write<string[]>("gameplayFeatures_NAME_KO", gF_.gameplayFeatures_NAME_KO);
		writer.Write<string[]>("gameplayFeatures_NAME_RU", gF_.gameplayFeatures_NAME_RU);
		writer.Write<string[]>("gameplayFeatures_NAME_IT", gF_.gameplayFeatures_NAME_IT);
		writer.Write<string[]>("gameplayFeatures_NAME_AR", gF_.gameplayFeatures_NAME_AR);
		writer.Write<string[]>("gameplayFeatures_NAME_JA", gF_.gameplayFeatures_NAME_JA);
		writer.Write<string[]>("gameplayFeatures_NAME_PL", gF_.gameplayFeatures_NAME_PL);
		writer.Write<string[]>("gameplayFeatures_NAME_UA", gF_.gameplayFeatures_NAME_UA);
		writer.Write<string[]>("gameplayFeatures_NAME_TH", gF_.gameplayFeatures_NAME_TH);
		writer.Write<string[]>("gameplayFeatures_DESC_EN", gF_.gameplayFeatures_DESC_EN);
		writer.Write<string[]>("gameplayFeatures_DESC_GE", gF_.gameplayFeatures_DESC_GE);
		writer.Write<string[]>("gameplayFeatures_DESC_TU", gF_.gameplayFeatures_DESC_TU);
		writer.Write<string[]>("gameplayFeatures_DESC_CH", gF_.gameplayFeatures_DESC_CH);
		writer.Write<string[]>("gameplayFeatures_DESC_FR", gF_.gameplayFeatures_DESC_FR);
		writer.Write<string[]>("gameplayFeatures_DESC_PB", gF_.gameplayFeatures_DESC_PB);
		writer.Write<string[]>("gameplayFeatures_DESC_CT", gF_.gameplayFeatures_DESC_CT);
		writer.Write<string[]>("gameplayFeatures_DESC_HU", gF_.gameplayFeatures_DESC_HU);
		writer.Write<string[]>("gameplayFeatures_DESC_ES", gF_.gameplayFeatures_DESC_ES);
		writer.Write<string[]>("gameplayFeatures_DESC_CZ", gF_.gameplayFeatures_DESC_CZ);
		writer.Write<string[]>("gameplayFeatures_DESC_KO", gF_.gameplayFeatures_DESC_KO);
		writer.Write<string[]>("gameplayFeatures_DESC_RU", gF_.gameplayFeatures_DESC_RU);
		writer.Write<string[]>("gameplayFeatures_DESC_IT", gF_.gameplayFeatures_DESC_IT);
		writer.Write<string[]>("gameplayFeatures_DESC_AR", gF_.gameplayFeatures_DESC_AR);
		writer.Write<string[]>("gameplayFeatures_DESC_JA", gF_.gameplayFeatures_DESC_JA);
		writer.Write<string[]>("gameplayFeatures_DESC_PL", gF_.gameplayFeatures_DESC_PL);
		writer.Write<string[]>("gameplayFeatures_DESC_UA", gF_.gameplayFeatures_DESC_UA);
		writer.Write<string[]>("gameplayFeatures_DESC_TH", gF_.gameplayFeatures_DESC_TH);
		writer.Write<string[]>("gameplayFeatures_ICONFILE", gF_.gameplayFeatures_ICONFILE);
	}

	private void LoadGameplayFeatures(ES3Reader reader, string filename)
	{
		gF_.gameplayFeatures_TYP = reader.Read<int[]>("gameplayFeatures_TYP");
		gF_.gameplayFeatures_RES_POINTS = reader.Read<int[]>("gameplayFeatures_RES_POINTS");
		gF_.gameplayFeatures_RES_POINTS_LEFT = reader.Read<float[]>("gameplayFeatures_RES_POINTS_LEFT");
		gF_.gameplayFeatures_PRICE = reader.Read<int[]>("gameplayFeatures_PRICE");
		gF_.gameplayFeatures_DEV_COSTS = reader.Read<int[]>("gameplayFeatures_DEV_COSTS");
		gF_.gameplayFeatures_DATE_YEAR = reader.Read<int[]>("gameplayFeatures_DATE_YEAR");
		gF_.gameplayFeatures_DATE_MONTH = reader.Read<int[]>("gameplayFeatures_DATE_MONTH");
		gF_.gameplayFeatures_GAMEPLAY = reader.Read<int[]>("gameplayFeatures_GAMEPLAY");
		gF_.gameplayFeatures_GRAPHIC = reader.Read<int[]>("gameplayFeatures_GRAPHIC");
		gF_.gameplayFeatures_SOUND = reader.Read<int[]>("gameplayFeatures_SOUND");
		gF_.gameplayFeatures_TECHNIK = reader.Read<int[]>("gameplayFeatures_TECHNIK");
		gF_.gameplayFeatures_LEVEL = reader.Read<int[]>("gameplayFeatures_LEVEL");
		if (key_gameplayFeatures_NEED_GAMEPLAY_FEATURE)
		{
			gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE = reader.Read<int[]>("gameplayFeatures_NEED_GAMEPLAY_FEATURE");
			gF_.gameplayFeatures_INTERNET = reader.Read<bool[]>("gameplayFeatures_INTERNET");
		}
		else
		{
			gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE = new int[gF_.gameplayFeatures_LEVEL.Length];
			for (int i = 0; i < gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE.Length; i++)
			{
				gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[i] = -1;
			}
			gF_.gameplayFeatures_INTERNET = new bool[gF_.gameplayFeatures_LEVEL.Length];
		}
		gF_.gameplayFeatures_UNLOCK = reader.Read<bool[]>("gameplayFeatures_UNLOCK");
		gF_.gameplayFeatures_GOOD = reader.Read<bool[,]>("gameplayFeatures_GOOD");
		gF_.gameplayFeatures_BAD = reader.Read<bool[,]>("gameplayFeatures_BAD");
		gF_.gameplayFeatures_LOCKPLATFORM = reader.Read<bool[,]>("gameplayFeatures_LOCKPLATFORM");
		if (key_EN)
		{
			gF_.gameplayFeatures_NAME_EN = reader.Read<string[]>("gameplayFeatures_NAME_EN");
		}
		if (key_GE)
		{
			gF_.gameplayFeatures_NAME_GE = reader.Read<string[]>("gameplayFeatures_NAME_GE");
		}
		if (key_TU)
		{
			gF_.gameplayFeatures_NAME_TU = reader.Read<string[]>("gameplayFeatures_NAME_TU");
		}
		if (key_CH)
		{
			gF_.gameplayFeatures_NAME_CH = reader.Read<string[]>("gameplayFeatures_NAME_CH");
		}
		if (key_FR)
		{
			gF_.gameplayFeatures_NAME_FR = reader.Read<string[]>("gameplayFeatures_NAME_FR");
		}
		if (key_PB)
		{
			gF_.gameplayFeatures_NAME_PB = reader.Read<string[]>("gameplayFeatures_NAME_PB");
		}
		if (key_CT)
		{
			gF_.gameplayFeatures_NAME_CT = reader.Read<string[]>("gameplayFeatures_NAME_CT");
		}
		if (key_HU)
		{
			gF_.gameplayFeatures_NAME_HU = reader.Read<string[]>("gameplayFeatures_NAME_HU");
		}
		if (key_ES)
		{
			gF_.gameplayFeatures_NAME_ES = reader.Read<string[]>("gameplayFeatures_NAME_ES");
		}
		if (key_CZ)
		{
			gF_.gameplayFeatures_NAME_CZ = reader.Read<string[]>("gameplayFeatures_NAME_CZ");
		}
		if (key_KO)
		{
			gF_.gameplayFeatures_NAME_KO = reader.Read<string[]>("gameplayFeatures_NAME_KO");
		}
		if (key_RU)
		{
			gF_.gameplayFeatures_NAME_RU = reader.Read<string[]>("gameplayFeatures_NAME_RU");
		}
		if (key_IT)
		{
			gF_.gameplayFeatures_NAME_IT = reader.Read<string[]>("gameplayFeatures_NAME_IT");
		}
		if (key_AR)
		{
			gF_.gameplayFeatures_NAME_AR = reader.Read<string[]>("gameplayFeatures_NAME_AR");
		}
		if (key_JA)
		{
			gF_.gameplayFeatures_NAME_JA = reader.Read<string[]>("gameplayFeatures_NAME_JA");
		}
		if (key_PL)
		{
			gF_.gameplayFeatures_NAME_PL = reader.Read<string[]>("gameplayFeatures_NAME_PL");
		}
		if (key_UA)
		{
			gF_.gameplayFeatures_NAME_UA = reader.Read<string[]>("gameplayFeatures_NAME_UA");
		}
		if (key_TH)
		{
			gF_.gameplayFeatures_NAME_TH = reader.Read<string[]>("gameplayFeatures_NAME_TH");
		}
		if (key_EN)
		{
			gF_.gameplayFeatures_DESC_EN = reader.Read<string[]>("gameplayFeatures_DESC_EN");
		}
		if (key_GE)
		{
			gF_.gameplayFeatures_DESC_GE = reader.Read<string[]>("gameplayFeatures_DESC_GE");
		}
		if (key_TU)
		{
			gF_.gameplayFeatures_DESC_TU = reader.Read<string[]>("gameplayFeatures_DESC_TU");
		}
		if (key_CH)
		{
			gF_.gameplayFeatures_DESC_CH = reader.Read<string[]>("gameplayFeatures_DESC_CH");
		}
		if (key_FR)
		{
			gF_.gameplayFeatures_DESC_FR = reader.Read<string[]>("gameplayFeatures_DESC_FR");
		}
		if (key_PB)
		{
			gF_.gameplayFeatures_DESC_PB = reader.Read<string[]>("gameplayFeatures_DESC_PB");
		}
		if (key_CT)
		{
			gF_.gameplayFeatures_DESC_CT = reader.Read<string[]>("gameplayFeatures_DESC_CT");
		}
		if (key_HU)
		{
			gF_.gameplayFeatures_DESC_HU = reader.Read<string[]>("gameplayFeatures_DESC_HU");
		}
		if (key_ES)
		{
			gF_.gameplayFeatures_DESC_ES = reader.Read<string[]>("gameplayFeatures_DESC_ES");
		}
		if (key_CZ)
		{
			gF_.gameplayFeatures_DESC_CZ = reader.Read<string[]>("gameplayFeatures_DESC_CZ");
		}
		if (key_KO)
		{
			gF_.gameplayFeatures_DESC_KO = reader.Read<string[]>("gameplayFeatures_DESC_KO");
		}
		if (key_RU)
		{
			gF_.gameplayFeatures_DESC_RU = reader.Read<string[]>("gameplayFeatures_DESC_RU");
		}
		if (key_IT)
		{
			gF_.gameplayFeatures_DESC_IT = reader.Read<string[]>("gameplayFeatures_DESC_IT");
		}
		if (key_AR)
		{
			gF_.gameplayFeatures_DESC_AR = reader.Read<string[]>("gameplayFeatures_DESC_AR");
		}
		if (key_JA)
		{
			gF_.gameplayFeatures_DESC_JA = reader.Read<string[]>("gameplayFeatures_DESC_JA");
		}
		if (key_PL)
		{
			gF_.gameplayFeatures_DESC_PL = reader.Read<string[]>("gameplayFeatures_DESC_PL");
		}
		if (key_UA)
		{
			gF_.gameplayFeatures_DESC_UA = reader.Read<string[]>("gameplayFeatures_DESC_UA");
		}
		if (key_TH)
		{
			gF_.gameplayFeatures_DESC_TH = reader.Read<string[]>("gameplayFeatures_DESC_TH");
		}
		gF_.gameplayFeatures_ICONFILE = reader.Read<string[]>("gameplayFeatures_ICONFILE");
		gF_.Init();
	}

	private void SaveThemes(ES3Writer writer)
	{
		writer.Write<float[]>("themes_RES_POINTS_LEFT", themes_.themes_RES_POINTS_LEFT);
		writer.Write<int[]>("themes_LEVEL", themes_.themes_LEVEL);
		writer.Write<int[]>("themes_MARKT", themes_.themes_MARKT);
		writer.Write<int[]>("themes_USES", themes_.themes_USES);
	}

	private void LoadThemes(ES3Reader reader, string filename)
	{
		tS_.LoadContent_Themes();
		themes_.themes_RES_POINTS_LEFT = reader.Read<float[]>("themes_RES_POINTS_LEFT");
		themes_.themes_LEVEL = reader.Read<int[]>("themes_LEVEL");
		themes_.themes_MARKT = reader.Read<int[]>("themes_MARKT");
		if (key_themes_USES)
		{
			themes_.themes_USES = reader.Read<int[]>("themes_USES");
		}
		else
		{
			themes_.themes_USES = new int[themes_.themes_LEVEL.Length];
		}
		if (tS_.themes_GE.Length != themes_.themes_RES_POINTS_LEFT.Length)
		{
			themes_.themes_MGSR = new int[themes_.themes_LEVEL.Length];
			themes_.themes_USES = new int[themes_.themes_LEVEL.Length];
		}
	}

	private void SaveNPCGameNames(ES3Writer writer)
	{
		writer.Write<string[]>("npcGames", tS_.npcGames);
		writer.Write<bool[]>("npcGameNameInUse", tS_.npcGameNameInUse);
	}

	private void LoadNPCGameNames(ES3Reader reader, string filename)
	{
		tS_.npcGames = reader.Read<string[]>("npcGames");
		if (key_npcGameNameInUse)
		{
			tS_.npcGameNameInUse = reader.Read<bool[]>("npcGameNameInUse");
		}
	}

	private void SaveGenres(ES3Writer writer)
	{
		writer.Write<int[]>("genres_RES_POINTS", genres_.genres_RES_POINTS);
		writer.Write<float[]>("genres_RES_POINTS_LEFT", genres_.genres_RES_POINTS_LEFT);
		writer.Write<int[]>("genres_PRICE", genres_.genres_PRICE);
		writer.Write<int[]>("genres_DEV_COSTS", genres_.genres_DEV_COSTS);
		writer.Write<int[]>("genres_DATE_YEAR", genres_.genres_DATE_YEAR);
		writer.Write<int[]>("genres_DATE_MONTH", genres_.genres_DATE_MONTH);
		writer.Write<int[]>("genres_LEVEL", genres_.genres_LEVEL);
		writer.Write<bool[]>("genres_UNLOCK", genres_.genres_UNLOCK);
		writer.Write<bool[]>("genres_SUC_YEAR", genres_.genres_SUC_YEAR);
		writer.Write<bool[,]>("genres_TARGETGROUP", genres_.genres_TARGETGROUP);
		writer.Write<float[]>("genres_GAMEPLAY", genres_.genres_GAMEPLAY);
		writer.Write<float[]>("genres_GRAPHIC", genres_.genres_GRAPHIC);
		writer.Write<float[]>("genres_SOUND", genres_.genres_SOUND);
		writer.Write<float[]>("genres_CONTROL", genres_.genres_CONTROL);
		writer.Write<bool[,]>("genres_COMBINATION", genres_.genres_COMBINATION);
		writer.Write<int[,]>("genres_PLATFORM_SELLS", genres_.genres_PLATFORM_SELLS);
		writer.Write<float[]>("genres_BELIEBTHEIT", genres_.genres_BELIEBTHEIT);
		writer.Write<bool[]>("genres_BELIEBTHEIT_SOLL", genres_.genres_BELIEBTHEIT_SOLL);
		writer.Write<int[,]>("genres_FOCUS", genres_.genres_FOCUS);
		writer.Write<int[,]>("genres_ALIGN", genres_.genres_ALIGN);
		writer.Write<string[]>("genres_NAME_EN", genres_.genres_NAME_EN);
		writer.Write<string[]>("genres_NAME_GE", genres_.genres_NAME_GE);
		writer.Write<string[]>("genres_NAME_TU", genres_.genres_NAME_TU);
		writer.Write<string[]>("genres_NAME_CH", genres_.genres_NAME_CH);
		writer.Write<string[]>("genres_NAME_FR", genres_.genres_NAME_FR);
		writer.Write<string[]>("genres_NAME_PB", genres_.genres_NAME_PB);
		writer.Write<string[]>("genres_NAME_HU", genres_.genres_NAME_HU);
		writer.Write<string[]>("genres_NAME_CT", genres_.genres_NAME_CT);
		writer.Write<string[]>("genres_NAME_ES", genres_.genres_NAME_ES);
		writer.Write<string[]>("genres_NAME_PL", genres_.genres_NAME_PL);
		writer.Write<string[]>("genres_NAME_CZ", genres_.genres_NAME_CZ);
		writer.Write<string[]>("genres_NAME_KO", genres_.genres_NAME_KO);
		writer.Write<string[]>("genres_NAME_IT", genres_.genres_NAME_IT);
		writer.Write<string[]>("genres_NAME_AR", genres_.genres_NAME_AR);
		writer.Write<string[]>("genres_NAME_JA", genres_.genres_NAME_JA);
		writer.Write<string[]>("genres_NAME_UA", genres_.genres_NAME_UA);
		writer.Write<string[]>("genres_NAME_TH", genres_.genres_NAME_TH);
		writer.Write<string[]>("genres_NAME_RU", genres_.genres_NAME_RU);
		writer.Write<string[]>("genres_DESC_EN", genres_.genres_DESC_EN);
		writer.Write<string[]>("genres_DESC_GE", genres_.genres_DESC_GE);
		writer.Write<string[]>("genres_DESC_TU", genres_.genres_DESC_TU);
		writer.Write<string[]>("genres_DESC_CH", genres_.genres_DESC_CH);
		writer.Write<string[]>("genres_DESC_FR", genres_.genres_DESC_FR);
		writer.Write<string[]>("genres_DESC_PB", genres_.genres_DESC_PB);
		writer.Write<string[]>("genres_DESC_HU", genres_.genres_DESC_HU);
		writer.Write<string[]>("genres_DESC_CT", genres_.genres_DESC_CT);
		writer.Write<string[]>("genres_DESC_ES", genres_.genres_DESC_ES);
		writer.Write<string[]>("genres_DESC_PL", genres_.genres_DESC_PL);
		writer.Write<string[]>("genres_DESC_CZ", genres_.genres_DESC_CZ);
		writer.Write<string[]>("genres_DESC_KO", genres_.genres_DESC_KO);
		writer.Write<string[]>("genres_DESC_IT", genres_.genres_DESC_IT);
		writer.Write<string[]>("genres_DESC_AR", genres_.genres_DESC_AR);
		writer.Write<string[]>("genres_DESC_JA", genres_.genres_DESC_JA);
		writer.Write<string[]>("genres_DESC_UA", genres_.genres_DESC_UA);
		writer.Write<string[]>("genres_DESC_TH", genres_.genres_DESC_TH);
		writer.Write<string[]>("genres_DESC_RU", genres_.genres_DESC_RU);
		writer.Write<string[]>("genres_ICONFILE", genres_.genres_ICONFILE);
		writer.Write<int[]>("genres_FANS", genres_.genres_FANS);
		writer.Write<int[]>("genres_MARKT", genres_.genres_MARKT);
	}

	private void LoadGenres(ES3Reader reader, string filename)
	{
		genres_.genres_RES_POINTS = reader.Read<int[]>("genres_RES_POINTS");
		genres_.genres_RES_POINTS_LEFT = reader.Read<float[]>("genres_RES_POINTS_LEFT");
		genres_.genres_PRICE = reader.Read<int[]>("genres_PRICE");
		genres_.genres_DEV_COSTS = reader.Read<int[]>("genres_DEV_COSTS");
		genres_.genres_DATE_YEAR = reader.Read<int[]>("genres_DATE_YEAR");
		genres_.genres_DATE_MONTH = reader.Read<int[]>("genres_DATE_MONTH");
		genres_.genres_LEVEL = reader.Read<int[]>("genres_LEVEL");
		genres_.genres_UNLOCK = reader.Read<bool[]>("genres_UNLOCK");
		if (key_genres_SUC_YEAR)
		{
			genres_.genres_SUC_YEAR = reader.Read<bool[]>("genres_SUC_YEAR");
		}
		else
		{
			genres_.genres_SUC_YEAR = new bool[genres_.genres_UNLOCK.Length];
		}
		genres_.genres_TARGETGROUP = reader.Read<bool[,]>("genres_TARGETGROUP");
		genres_.genres_GAMEPLAY = reader.Read<float[]>("genres_GAMEPLAY");
		genres_.genres_GRAPHIC = reader.Read<float[]>("genres_GRAPHIC");
		genres_.genres_SOUND = reader.Read<float[]>("genres_SOUND");
		genres_.genres_CONTROL = reader.Read<float[]>("genres_CONTROL");
		genres_.genres_COMBINATION = reader.Read<bool[,]>("genres_COMBINATION");
		if (key_genres_PLATFORM_SELLS)
		{
			genres_.genres_PLATFORM_SELLS = reader.Read<int[,]>("genres_PLATFORM_SELLS");
		}
		else
		{
			genres_.genres_PLATFORM_SELLS = new int[genres_.genres_UNLOCK.Length, 5];
			for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
			{
				for (int j = 0; j < 5; j++)
				{
					genres_.genres_PLATFORM_SELLS[i, j] = 100;
				}
			}
		}
		genres_.genres_FOCUS = reader.Read<int[,]>("genres_FOCUS");
		genres_.genres_ALIGN = reader.Read<int[,]>("genres_ALIGN");
		genres_.genres_BELIEBTHEIT = reader.Read<float[]>("genres_BELIEBTHEIT");
		genres_.genres_BELIEBTHEIT_SOLL = reader.Read<bool[]>("genres_BELIEBTHEIT_SOLL");
		if (key_EN)
		{
			genres_.genres_NAME_EN = reader.Read<string[]>("genres_NAME_EN");
		}
		if (key_GE)
		{
			genres_.genres_NAME_GE = reader.Read<string[]>("genres_NAME_GE");
		}
		if (key_TU)
		{
			genres_.genres_NAME_TU = reader.Read<string[]>("genres_NAME_TU");
		}
		if (key_CH)
		{
			genres_.genres_NAME_CH = reader.Read<string[]>("genres_NAME_CH");
		}
		if (key_FR)
		{
			genres_.genres_NAME_FR = reader.Read<string[]>("genres_NAME_FR");
		}
		if (key_PB)
		{
			genres_.genres_NAME_PB = reader.Read<string[]>("genres_NAME_PB");
		}
		if (key_HU)
		{
			genres_.genres_NAME_HU = reader.Read<string[]>("genres_NAME_HU");
		}
		if (key_CT)
		{
			genres_.genres_NAME_CT = reader.Read<string[]>("genres_NAME_CT");
		}
		if (key_ES)
		{
			genres_.genres_NAME_ES = reader.Read<string[]>("genres_NAME_ES");
		}
		if (key_PL)
		{
			genres_.genres_NAME_PL = reader.Read<string[]>("genres_NAME_PL");
		}
		if (key_CZ)
		{
			genres_.genres_NAME_CZ = reader.Read<string[]>("genres_NAME_CZ");
		}
		if (key_KO)
		{
			genres_.genres_NAME_KO = reader.Read<string[]>("genres_NAME_KO");
		}
		if (key_IT)
		{
			genres_.genres_NAME_IT = reader.Read<string[]>("genres_NAME_IT");
		}
		if (key_AR)
		{
			genres_.genres_NAME_AR = reader.Read<string[]>("genres_NAME_AR");
		}
		if (key_JA)
		{
			genres_.genres_NAME_JA = reader.Read<string[]>("genres_NAME_JA");
		}
		if (key_UA)
		{
			genres_.genres_NAME_UA = reader.Read<string[]>("genres_NAME_UA");
		}
		if (key_TH)
		{
			genres_.genres_NAME_TH = reader.Read<string[]>("genres_NAME_TH");
		}
		if (key_RU)
		{
			genres_.genres_NAME_RU = reader.Read<string[]>("genres_NAME_RU");
		}
		if (key_EN)
		{
			genres_.genres_DESC_EN = reader.Read<string[]>("genres_DESC_EN");
		}
		if (key_GE)
		{
			genres_.genres_DESC_GE = reader.Read<string[]>("genres_DESC_GE");
		}
		if (key_TU)
		{
			genres_.genres_DESC_TU = reader.Read<string[]>("genres_DESC_TU");
		}
		if (key_CH)
		{
			genres_.genres_DESC_CH = reader.Read<string[]>("genres_DESC_CH");
		}
		if (key_FR)
		{
			genres_.genres_DESC_FR = reader.Read<string[]>("genres_DESC_FR");
		}
		if (key_PB)
		{
			genres_.genres_DESC_PB = reader.Read<string[]>("genres_DESC_PB");
		}
		if (key_HU)
		{
			genres_.genres_DESC_HU = reader.Read<string[]>("genres_DESC_HU");
		}
		if (key_CT)
		{
			genres_.genres_DESC_CT = reader.Read<string[]>("genres_DESC_CT");
		}
		if (key_ES)
		{
			genres_.genres_DESC_ES = reader.Read<string[]>("genres_DESC_ES");
		}
		if (key_PL)
		{
			genres_.genres_DESC_PL = reader.Read<string[]>("genres_DESC_PL");
		}
		if (key_CZ)
		{
			genres_.genres_DESC_CZ = reader.Read<string[]>("genres_DESC_CZ");
		}
		if (key_KO)
		{
			genres_.genres_DESC_KO = reader.Read<string[]>("genres_DESC_KO");
		}
		if (key_IT)
		{
			genres_.genres_DESC_IT = reader.Read<string[]>("genres_DESC_IT");
		}
		if (key_AR)
		{
			genres_.genres_DESC_AR = reader.Read<string[]>("genres_DESC_AR");
		}
		if (key_JA)
		{
			genres_.genres_DESC_JA = reader.Read<string[]>("genres_DESC_JA");
		}
		if (key_UA)
		{
			genres_.genres_DESC_UA = reader.Read<string[]>("genres_DESC_UA");
		}
		if (key_TH)
		{
			genres_.genres_DESC_TH = reader.Read<string[]>("genres_DESC_TH");
		}
		if (key_RU)
		{
			genres_.genres_DESC_RU = reader.Read<string[]>("genres_DESC_RU");
		}
		genres_.genres_ICONFILE = reader.Read<string[]>("genres_ICONFILE");
		genres_.genres_FANS = reader.Read<int[]>("genres_FANS");
		if (es3file.KeyExists("genres_MARKT"))
		{
			genres_.genres_MARKT = reader.Read<int[]>("genres_MARKT");
		}
		else
		{
			genres_.genres_MARKT = new int[genres_.genres_LEVEL.Length];
		}
		genres_.Init();
	}

	private void SaveLicences(ES3Writer writer)
	{
		writer.Write<string[]>("licence_EN", licences_.licence_EN);
		writer.Write<int[]>("licence_TYP", licences_.licence_TYP);
		writer.Write<float[]>("licence_QUALITY", licences_.licence_QUALITY);
		writer.Write<int[]>("licence_ANGEBOT", licences_.licence_ANGEBOT);
		writer.Write<int[]>("licence_GEKAUFT", licences_.licence_GEKAUFT);
		writer.Write<int[]>("licence_GENREGOOD", licences_.licence_GENREGOOD);
		writer.Write<int[]>("licence_GENREBAD", licences_.licence_GENREBAD);
		writer.Write<int[]>("licence_YEAR", licences_.licence_YEAR);
	}

	private void LoadLicences(ES3Reader reader, string filename)
	{
		licences_.licence_EN = reader.Read<string[]>("licence_EN");
		licences_.licence_TYP = reader.Read<int[]>("licence_TYP");
		licences_.licence_QUALITY = reader.Read<float[]>("licence_QUALITY");
		licences_.licence_ANGEBOT = reader.Read<int[]>("licence_ANGEBOT");
		licences_.licence_GEKAUFT = reader.Read<int[]>("licence_GEKAUFT");
		if (key_licence_YEAR)
		{
			licences_.licence_GENREGOOD = reader.Read<int[]>("licence_GENREGOOD");
			licences_.licence_GENREBAD = reader.Read<int[]>("licence_GENREBAD");
			licences_.licence_YEAR = reader.Read<int[]>("licence_YEAR");
			return;
		}
		licences_.licence_GENREGOOD = new int[licences_.licence_EN.Length];
		licences_.licence_GENREBAD = new int[licences_.licence_EN.Length];
		licences_.licence_YEAR = new int[licences_.licence_EN.Length];
		for (int i = 0; i < licences_.licence_EN.Length; i++)
		{
			licences_.SetRandomGoodAndBadGenre(i);
			licences_.licence_YEAR[i] = 1976;
		}
	}

	private void SaveCopyProtect(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		int num = array.Length;
		writer.Write<int>("anzCopyProtect", num);
		int[] array2 = new int[5 * num];
		float[] array3 = new float[num];
		string[] array4 = new string[11 * num];
		bool[] array5 = new bool[3 * num];
		for (int i = 0; i < array.Length; i++)
		{
			copyProtectScript component = array[i].GetComponent<copyProtectScript>();
			int num2 = i * (array4.Length / num);
			int num3 = i * (array2.Length / num);
			int num4 = i * (array3.Length / num);
			int num5 = i * (array5.Length / num);
			array4[num2] = component.name_EN;
			array4[1 + num2] = component.name_GE;
			array4[2 + num2] = component.name_TU;
			array4[3 + num2] = component.name_CH;
			array4[4 + num2] = component.name_FR;
			array4[5 + num2] = component.name_CT;
			array4[6 + num2] = component.name_RU;
			array4[7 + num2] = component.name_IT;
			array4[8 + num2] = component.name_JA;
			array4[9 + num2] = component.name_UA;
			array4[10 + num2] = component.name_TH;
			array2[num3] = component.myID;
			array2[1 + num3] = component.date_year;
			array2[2 + num3] = component.date_month;
			array2[3 + num3] = component.price;
			array2[4 + num3] = component.dev_costs;
			array3[num4] = component.effekt;
			array5[num5] = component.isUnlocked;
			array5[1 + num5] = component.inBesitz;
			array5[2 + num5] = component.neverLooseEffect;
		}
		writer.Write<int[]>("copyProtect_I", array2);
		writer.Write<float[]>("copyProtect_F", array3);
		writer.Write<string[]>("copyProtect_S", array4);
		writer.Write<bool[]>("copyProtect_B", array5);
	}

	private void LoadCopyProtect(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzCopyProtect", -1);
		Debug.Log("Load: (anzCopyProtect) " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		array2 = reader.Read<int[]>("copyProtect_I");
		array3 = reader.Read<float[]>("copyProtect_F");
		array4 = reader.Read<string[]>("copyProtect_S");
		array5 = reader.Read<bool[]>("copyProtect_B");
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			int num10 = i * num5;
			copyProtectScript copyProtectScript2 = copyProtect_.CreateCopyProtect();
			copyProtectScript2.name_EN = array4[num10];
			copyProtectScript2.name_GE = array4[1 + num10];
			copyProtectScript2.name_TU = array4[2 + num10];
			copyProtectScript2.name_CH = array4[3 + num10];
			copyProtectScript2.name_FR = array4[4 + num10];
			copyProtectScript2.name_CT = array4[5 + num10];
			copyProtectScript2.name_RU = array4[6 + num10];
			copyProtectScript2.name_IT = array4[7 + num10];
			copyProtectScript2.name_JA = array4[8 + num10];
			if (num5 > 9)
			{
				copyProtectScript2.name_UA = array4[9 + num10];
			}
			if (num5 > 10)
			{
				copyProtectScript2.name_TH = array4[10 + num10];
			}
			copyProtectScript2.myID = array2[num7];
			copyProtectScript2.date_year = array2[1 + num7];
			copyProtectScript2.date_month = array2[2 + num7];
			copyProtectScript2.price = array2[3 + num7];
			copyProtectScript2.dev_costs = array2[4 + num7];
			copyProtectScript2.effekt = array3[num8];
			copyProtectScript2.isUnlocked = array5[num9];
			copyProtectScript2.inBesitz = array5[1 + num9];
			copyProtectScript2.neverLooseEffect = array5[2 + num9];
			copyProtectScript2.Init();
		}
	}

	private void SaveAntiCheat(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("AntiCheat");
		int num = array.Length;
		writer.Write<int>("anzAntiCheat", num);
		int[] array2 = new int[5 * num];
		float[] array3 = new float[num];
		string[] array4 = new string[11 * num];
		bool[] array5 = new bool[3 * num];
		for (int i = 0; i < array.Length; i++)
		{
			antiCheatScript component = array[i].GetComponent<antiCheatScript>();
			int num2 = i * (array4.Length / num);
			int num3 = i * (array2.Length / num);
			int num4 = i * (array3.Length / num);
			int num5 = i * (array5.Length / num);
			array4[num2] = component.name_EN;
			array4[1 + num2] = component.name_GE;
			array4[2 + num2] = component.name_TU;
			array4[3 + num2] = component.name_CH;
			array4[4 + num2] = component.name_FR;
			array4[5 + num2] = component.name_CT;
			array4[6 + num2] = component.name_RU;
			array4[7 + num2] = component.name_IT;
			array4[8 + num2] = component.name_JA;
			array4[9 + num2] = component.name_UA;
			array4[10 + num2] = component.name_TH;
			array2[num3] = component.myID;
			array2[1 + num3] = component.date_year;
			array2[2 + num3] = component.date_month;
			array2[3 + num3] = component.price;
			array2[4 + num3] = component.dev_costs;
			array3[num4] = component.effekt;
			array5[num5] = component.isUnlocked;
			array5[1 + num5] = component.inBesitz;
			array5[2 + num5] = component.neverLooseEffect;
		}
		writer.Write<int[]>("antiCheat_I", array2);
		writer.Write<float[]>("antiCheat_F", array3);
		writer.Write<string[]>("antiCheat_S", array4);
		writer.Write<bool[]>("antiCheat_B", array5);
	}

	private void LoadAntiCheat(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzAntiCheat", -1);
		Debug.Log("Load: (anzAntiCheat) " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		array2 = reader.Read<int[]>("antiCheat_I");
		array3 = reader.Read<float[]>("antiCheat_F");
		array4 = reader.Read<string[]>("antiCheat_S");
		array5 = reader.Read<bool[]>("antiCheat_B");
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			int num10 = i * num5;
			antiCheatScript antiCheatScript2 = antiCheat_.CreateAntiCheat();
			antiCheatScript2.name_EN = array4[num10];
			antiCheatScript2.name_GE = array4[1 + num10];
			antiCheatScript2.name_TU = array4[2 + num10];
			antiCheatScript2.name_CH = array4[3 + num10];
			antiCheatScript2.name_FR = array4[4 + num10];
			antiCheatScript2.name_CT = array4[5 + num10];
			antiCheatScript2.name_RU = array4[6 + num10];
			antiCheatScript2.name_IT = array4[7 + num10];
			antiCheatScript2.name_JA = array4[8 + num10];
			if (num5 > 9)
			{
				antiCheatScript2.name_UA = array4[9 + num10];
			}
			if (num5 > 10)
			{
				antiCheatScript2.name_TH = array4[10 + num10];
			}
			antiCheatScript2.myID = array2[num7];
			antiCheatScript2.date_year = array2[1 + num7];
			antiCheatScript2.date_month = array2[2 + num7];
			antiCheatScript2.price = array2[3 + num7];
			antiCheatScript2.dev_costs = array2[4 + num7];
			antiCheatScript2.effekt = array3[num8];
			antiCheatScript2.isUnlocked = array5[num9];
			antiCheatScript2.inBesitz = array5[1 + num9];
			antiCheatScript2.neverLooseEffect = array5[2 + num9];
			antiCheatScript2.Init();
		}
	}

	private void SaveContractWork(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("ContractWork");
		int num = array.Length;
		writer.Write<int>("anzContractWork", num);
		int[] array2 = new int[8 * num];
		float[] array3 = new float[num];
		bool[] array4 = new bool[num];
		for (int i = 0; i < array.Length; i++)
		{
			contractWork component = array[i].GetComponent<contractWork>();
			int num2 = i * (array2.Length / num);
			int num3 = i * (array3.Length / num);
			int num4 = i * (array4.Length / num);
			array2[num2] = component.myID;
			array2[1 + num2] = component.typ;
			array2[2 + num2] = component.gehalt;
			array2[3 + num2] = component.strafe;
			array2[4 + num2] = component.auftraggeberID;
			array2[5 + num2] = component.zeitInWochen;
			array2[6 + num2] = component.wochenAlsAngebot;
			array2[7 + num2] = component.art;
			array3[num3] = component.points;
			array4[num4] = component.angenommen;
		}
		writer.Write<int[]>("contractWork_I", array2);
		writer.Write<float[]>("contractWork_F", array3);
		writer.Write<bool[]>("contractWork_B", array4);
	}

	private void LoadContractWork(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzContractWork", -1);
		Debug.Log("Load: (anzContractWork) " + num);
		if (num > 0)
		{
			long[] array = new long[0];
			int[] array2 = new int[0];
			float[] array3 = new float[0];
			string[] array4 = new string[0];
			bool[] array5 = new bool[0];
			array2 = reader.Read<int[]>("contractWork_I");
			array3 = reader.Read<float[]>("contractWork_F");
			array5 = reader.Read<bool[]>("contractWork_B");
			int num2 = array2.Length / num;
			int num3 = array3.Length / num;
			int num4 = array5.Length / num;
			int num5 = array4.Length / num;
			int num6 = array.Length / num;
			for (int i = 0; i < num; i++)
			{
				int num7 = i * num2;
				int num8 = i * num3;
				int num9 = i * num4;
				contractWork obj = contractWorkMain_.CreateContractWork();
				obj.myID = array2[num7];
				obj.typ = array2[1 + num7];
				obj.gehalt = array2[2 + num7];
				obj.strafe = array2[3 + num7];
				obj.auftraggeberID = array2[4 + num7];
				obj.zeitInWochen = array2[5 + num7];
				obj.wochenAlsAngebot = array2[6 + num7];
				obj.art = array2[7 + num7];
				obj.points = array3[num8];
				obj.angenommen = array5[num9];
				obj.Init();
			}
		}
	}

	private void SavePlatforms(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		int num = array.Length;
		writer.Write<int>("anzPlatforms", num);
		int[] array2 = new int[20 * num];
		int[] array3 = new int[40 * num];
		float[] array4 = new float[13 * num];
		string[] array5 = new string[40 * num];
		bool[] array6 = new bool[36 * num];
		long[] array7 = new long[20 * num];
		for (int i = 0; i < array.Length; i++)
		{
			platformScript component = array[i].GetComponent<platformScript>();
			int num2 = i * (array5.Length / num);
			int num3 = i * (array2.Length / num);
			int num4 = i * (array3.Length / num);
			int num5 = i * (array4.Length / num);
			int num6 = i * (array6.Length / num);
			int num7 = i * (array7.Length / num);
			array5[num2] = component.name_EN;
			array5[1 + num2] = component.name_GE;
			array5[2 + num2] = component.manufacturer_EN;
			array5[3 + num2] = component.manufacturer_GE;
			array5[4 + num2] = component.pic1_file;
			array5[5 + num2] = component.pic2_file;
			array5[6 + num2] = component.name_TU;
			array5[7 + num2] = component.manufacturer_TU;
			array5[8 + num2] = component.name_CH;
			array5[9 + num2] = component.manufacturer_CH;
			array5[10 + num2] = component.name_FR;
			array5[11 + num2] = component.manufacturer_FR;
			array5[12 + num2] = component.name_HU;
			array5[13 + num2] = component.manufacturer_HU;
			array5[14 + num2] = component.name_JA;
			array5[15 + num2] = component.manufacturer_JA;
			array5[16 + num2] = component.myName;
			array5[17 + num2] = component.name_PL;
			array5[18 + num2] = component.manufacturer_PL;
			array5[19 + num2] = component.name_UA;
			array5[20 + num2] = component.manufacturer_UA;
			array5[21 + num2] = component.name_TH;
			array5[22 + num2] = component.manufacturer_TH;
			array5[23 + num2] = component.proName;
			array2[num3] = component.myID;
			array2[1 + num3] = component.date_year;
			array2[2 + num3] = component.date_month;
			array2[3 + num3] = component.date_year_end;
			array2[4 + num3] = component.date_month_end;
			array2[5 + num3] = component.price;
			array2[6 + num3] = component.dev_costs;
			array2[7 + num3] = component.tech;
			array2[8 + num3] = component.units;
			array2[9 + num3] = component.units_max;
			array2[10 + num3] = component.erfahrung;
			array2[11 + num3] = component.pic2_year;
			array2[12 + num3] = component.games;
			array2[13 + num3] = component.needFeatures[0];
			array2[14 + num3] = component.needFeatures[1];
			array2[15 + num3] = component.needFeatures[2];
			array2[16 + num3] = component.complex;
			array2[17 + num3] = component.typ;
			array2[18 + num3] = component.ownerID;
			array2[19 + num3] = component.anzController;
			array3[num4] = component.component_cpu;
			array3[1 + num4] = component.component_gfx;
			array3[2 + num4] = component.component_ram;
			array3[3 + num4] = component.component_hdd;
			array3[4 + num4] = component.component_sfx;
			array3[5 + num4] = component.component_cooling;
			array3[6 + num4] = component.component_disc;
			array3[7 + num4] = component.component_controller;
			array3[8 + num4] = component.component_case;
			array3[9 + num4] = component.component_monitor;
			array3[10 + num4] = component.gameID;
			array3[11 + num4] = component.costs_marketing;
			array3[12 + num4] = component.costs_mitarbeiter;
			array3[13 + num4] = component.startProduktionskosten;
			array3[14 + num4] = component.verkaufspreis;
			array3[15 + num4] = component.weeksOnMarket;
			array3[16 + num4] = component.performancePoints;
			array3[17 + num4] = component.autoPreisGewinn;
			array3[18 + num4] = component.weeksInDevelopment;
			array3[19 + num4] = component.exklusivGames;
			array3[20 + num4] = component.minGamePassGames;
			array3[21 + num4] = component.platformCompatible[0];
			array3[22 + num4] = component.platformCompatible[1];
			array3[23 + num4] = component.platformCompatible[2];
			array3[24 + num4] = component.platformCompatible[3];
			array3[25 + num4] = component.nachfolgerID;
			array3[26 + num4] = component.vorgaengerID;
			array3[27 + num4] = component.costs_preisreduktion;
			array3[28 + num4] = component.garantie;
			array3[29 + num4] = component.gamesInDev;
			array3[34 + num4] = component.garantiefaelle;
			array3[35 + num4] = component.costs_haltbarkeit;
			array3[36 + num4] = component.subventionMoney;
			array4[num5] = component.marktanteil;
			array4[1 + num5] = component.powerFromMarket;
			array4[2 + num5] = component.consoleColor.x;
			array4[3 + num5] = component.consoleColor.y;
			array4[4 + num5] = component.devPointsStart;
			array4[5 + num5] = component.devPoints;
			array4[6 + num5] = component.hype;
			array4[7 + num5] = component.kostenreduktion;
			array4[8 + num5] = component.review;
			array4[9 + num5] = component.consoleColor.z;
			array4[10 + num5] = component.haltbarkeit;
			array6[num6] = component.inGamePass;
			array6[1 + num6] = component.isUnlocked;
			array6[2 + num6] = component.inBesitz;
			array6[3 + num6] = component.inGamePassPassiv;
			array6[4 + num6] = component.vomMarktGenommen;
			array6[5 + num6] = component.internet;
			array6[6 + num6] = component.angekuendigt;
			array6[7 + num6] = component.autoPreis;
			array6[8 + num6] = component.thridPartyGames;
			array6[9 + num6] = component.proVersion;
			array6[10 + num6] = component.kostenreduktionDone[0];
			array6[11 + num6] = component.kostenreduktionDone[1];
			array6[12 + num6] = component.kostenreduktionDone[2];
			array6[13 + num6] = component.kostenreduktionDone[3];
			array6[14 + num6] = component.kostenreduktionDone[4];
			array6[15 + num6] = component.kostenreduktionDone[5];
			array6[16 + num6] = component.kostenreduktionDone[6];
			array6[17 + num6] = component.kostenreduktionDone[7];
			array6[18 + num6] = component.kostenreduktionDone[8];
			array6[19 + num6] = true;
			array6[20 + num6] = component.haltbarkeitDone[0];
			array6[21 + num6] = component.haltbarkeitDone[1];
			array6[22 + num6] = component.haltbarkeitDone[2];
			array6[23 + num6] = component.haltbarkeitDone[3];
			array6[24 + num6] = component.haltbarkeitDone[4];
			array6[25 + num6] = component.haltbarkeitDone[5];
			array6[26 + num6] = component.haltbarkeitDone[6];
			array6[27 + num6] = component.haltbarkeitDone[7];
			array6[28 + num6] = component.haltbarkeitDone[8];
			array6[29 + num6] = component.subventionGameSize[0];
			array6[30 + num6] = component.subventionGameSize[1];
			array6[31 + num6] = component.subventionGameSize[2];
			array6[32 + num6] = component.subventionGameSize[3];
			array6[33 + num6] = component.subventionGameSize[4];
			array6[34 + num6] = component.subventionGameSize[5];
			array6[35 + num6] = component.konsoleTab_fullView;
			array7[num7] = component.einnahmen;
			array7[1 + num7] = component.entwicklungsKosten;
			array7[2 + num7] = component.umsatzTotal;
			array7[3 + num7] = component.costs_production;
			array7[4 + num7] = component.costs_garantie;
			array7[5 + num7] = component.garantiekosten;
			array7[6 + num7] = component.costs_subvention;
			if (!component.OwnerIsNPC())
			{
				writer.Write<bool[]>("platformA1_" + component.myID, component.hwFeatures);
				writer.Write<int[]>("platformA2_" + component.myID, component.sellsPerWeek);
				writer.Write<bool[]>("platformA3_" + component.myID, component.publisherBuyed);
				writer.Write<int[]>("platformA4_" + component.myID, component.garantieMonth);
			}
		}
		writer.Write<int[]>("platform_I", array2);
		writer.Write<int[]>("platform_I2", array3);
		writer.Write<float[]>("platform_F", array4);
		writer.Write<string[]>("platform_S", array5);
		writer.Write<bool[]>("platform_B", array6);
		writer.Write<long[]>("platform_L", array7);
	}

	private void LoadPlatforms(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzPlatforms", -1);
		Debug.Log("Load: (anzPlatforms) " + num);
		if (num <= 0)
		{
			return;
		}
		int[] array = new int[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		long[] array6 = new long[0];
		array = reader.Read<int[]>("platform_I");
		array2 = reader.Read<int[]>("platform_I2");
		array3 = reader.Read<float[]>("platform_F");
		array6 = reader.Read<long[]>("platform_L");
		array4 = reader.Read<string[]>("platform_S");
		array5 = reader.Read<bool[]>("platform_B");
		int num2 = array.Length / num;
		int num3 = array2.Length / num;
		int num4 = array3.Length / num;
		int num5 = array4.Length / num;
		int num6 = array5.Length / num;
		int num7 = array6.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num8 = i * num2;
			int num9 = i * num3;
			int num10 = i * num4;
			int num11 = i * num6;
			int num12 = i * num5;
			int num13 = i * num7;
			platformScript platformScript2 = platforms_.CreatePlatform();
			platformScript2.name_EN = array4[num12];
			platformScript2.name_GE = array4[1 + num12];
			platformScript2.manufacturer_EN = array4[2 + num12];
			platformScript2.manufacturer_GE = array4[3 + num12];
			platformScript2.pic1_file = array4[4 + num12];
			platformScript2.pic2_file = array4[5 + num12];
			platformScript2.name_TU = array4[6 + num12];
			platformScript2.manufacturer_TU = array4[7 + num12];
			platformScript2.name_CH = array4[8 + num12];
			platformScript2.manufacturer_CH = array4[9 + num12];
			platformScript2.name_FR = array4[10 + num12];
			platformScript2.manufacturer_FR = array4[11 + num12];
			platformScript2.name_HU = array4[12 + num12];
			platformScript2.manufacturer_HU = array4[13 + num12];
			platformScript2.name_JA = array4[14 + num12];
			platformScript2.manufacturer_JA = array4[15 + num12];
			platformScript2.myName = array4[16 + num12];
			platformScript2.name_PL = array4[17 + num12];
			platformScript2.manufacturer_PL = array4[18 + num12];
			platformScript2.name_UA = array4[19 + num12];
			platformScript2.manufacturer_UA = array4[20 + num12];
			platformScript2.name_TH = array4[21 + num12];
			platformScript2.manufacturer_TH = array4[22 + num12];
			platformScript2.proName = array4[23 + num12];
			platformScript2.myID = array[num8];
			platformScript2.date_year = array[1 + num8];
			platformScript2.date_month = array[2 + num8];
			platformScript2.date_year_end = array[3 + num8];
			platformScript2.date_month_end = array[4 + num8];
			platformScript2.price = array[5 + num8];
			platformScript2.dev_costs = array[6 + num8];
			platformScript2.tech = array[7 + num8];
			platformScript2.units = array[8 + num8];
			platformScript2.units_max = array[9 + num8];
			platformScript2.erfahrung = array[10 + num8];
			platformScript2.pic2_year = array[11 + num8];
			platformScript2.games = array[12 + num8];
			platformScript2.needFeatures[0] = array[13 + num8];
			platformScript2.needFeatures[1] = array[14 + num8];
			platformScript2.needFeatures[2] = array[15 + num8];
			platformScript2.complex = array[16 + num8];
			platformScript2.typ = array[17 + num8];
			platformScript2.ownerID = array[18 + num8];
			platformScript2.anzController = array[19 + num8];
			platformScript2.component_cpu = array2[num9];
			platformScript2.component_gfx = array2[1 + num9];
			platformScript2.component_ram = array2[2 + num9];
			platformScript2.component_hdd = array2[3 + num9];
			platformScript2.component_sfx = array2[4 + num9];
			platformScript2.component_cooling = array2[5 + num9];
			platformScript2.component_disc = array2[6 + num9];
			platformScript2.component_controller = array2[7 + num9];
			platformScript2.component_case = array2[8 + num9];
			platformScript2.component_monitor = array2[9 + num9];
			platformScript2.gameID = array2[10 + num9];
			platformScript2.costs_marketing = array2[11 + num9];
			platformScript2.costs_mitarbeiter = array2[12 + num9];
			platformScript2.startProduktionskosten = array2[13 + num9];
			platformScript2.verkaufspreis = array2[14 + num9];
			platformScript2.weeksOnMarket = array2[15 + num9];
			platformScript2.performancePoints = array2[16 + num9];
			platformScript2.autoPreisGewinn = array2[17 + num9];
			platformScript2.weeksInDevelopment = array2[18 + num9];
			platformScript2.exklusivGames = array2[19 + num9];
			platformScript2.minGamePassGames = array2[20 + num9];
			platformScript2.platformCompatible[0] = array2[21 + num9];
			platformScript2.platformCompatible[1] = array2[22 + num9];
			platformScript2.platformCompatible[2] = array2[23 + num9];
			platformScript2.platformCompatible[3] = array2[24 + num9];
			platformScript2.nachfolgerID = array2[25 + num9];
			if (platformScript2.nachfolgerID == 0)
			{
				platformScript2.nachfolgerID = -1;
			}
			platformScript2.vorgaengerID = array2[26 + num9];
			if (platformScript2.vorgaengerID == 0)
			{
				platformScript2.vorgaengerID = -1;
			}
			platformScript2.costs_preisreduktion = array2[27 + num9];
			platformScript2.garantie = array2[28 + num9];
			if (platformScript2.garantie <= 0 && platformScript2.weeksOnMarket > 0)
			{
				platformScript2.garantie = 12;
			}
			platformScript2.gamesInDev = array2[29 + num9];
			platformScript2.garantiefaelle = array2[34 + num9];
			platformScript2.costs_haltbarkeit = array2[35 + num9];
			platformScript2.subventionMoney = array2[36 + num9];
			platformScript2.marktanteil = array3[num10];
			platformScript2.powerFromMarket = array3[1 + num10];
			platformScript2.consoleColor.x = array3[2 + num10];
			platformScript2.consoleColor.y = array3[3 + num10];
			platformScript2.devPointsStart = array3[4 + num10];
			platformScript2.devPoints = array3[5 + num10];
			platformScript2.hype = array3[6 + num10];
			platformScript2.kostenreduktion = array3[7 + num10];
			platformScript2.review = array3[8 + num10];
			platformScript2.consoleColor.z = array3[9 + num10];
			if (platformScript2.consoleColor.z <= 0f)
			{
				platformScript2.consoleColor = new Vector3(0.5f, 0.5f, 0.5f);
			}
			if (num4 > 10)
			{
				platformScript2.haltbarkeit = array3[10 + num10];
			}
			if (platformScript2.haltbarkeit <= 0f && platformScript2.weeksOnMarket > 0)
			{
				platformScript2.haltbarkeit = platformScript2.devPointsStart * 5f;
			}
			platformScript2.inGamePass = array5[num11];
			platformScript2.isUnlocked = array5[1 + num11];
			platformScript2.inBesitz = array5[2 + num11];
			platformScript2.inGamePassPassiv = array5[3 + num11];
			platformScript2.vomMarktGenommen = array5[4 + num11];
			platformScript2.internet = array5[5 + num11];
			platformScript2.angekuendigt = array5[6 + num11];
			platformScript2.autoPreis = array5[7 + num11];
			platformScript2.thridPartyGames = array5[8 + num11];
			platformScript2.proVersion = array5[9 + num11];
			if (num6 > 10)
			{
				platformScript2.kostenreduktionDone[0] = array5[10 + num11];
			}
			if (num6 > 11)
			{
				platformScript2.kostenreduktionDone[1] = array5[11 + num11];
			}
			if (num6 > 12)
			{
				platformScript2.kostenreduktionDone[2] = array5[12 + num11];
			}
			if (num6 > 13)
			{
				platformScript2.kostenreduktionDone[3] = array5[13 + num11];
			}
			if (num6 > 14)
			{
				platformScript2.kostenreduktionDone[4] = array5[14 + num11];
			}
			if (num6 > 15)
			{
				platformScript2.kostenreduktionDone[5] = array5[15 + num11];
			}
			if (num6 > 16)
			{
				platformScript2.kostenreduktionDone[6] = array5[16 + num11];
			}
			if (num6 > 17)
			{
				platformScript2.kostenreduktionDone[7] = array5[17 + num11];
			}
			if (num6 > 18)
			{
				platformScript2.kostenreduktionDone[8] = array5[18 + num11];
			}
			bool flag = false;
			if (num6 > 19)
			{
				flag = array5[19 + num11];
			}
			if (num6 > 20)
			{
				platformScript2.haltbarkeitDone[0] = array5[20 + num11];
			}
			if (num6 > 21)
			{
				platformScript2.haltbarkeitDone[1] = array5[21 + num11];
			}
			if (num6 > 22)
			{
				platformScript2.haltbarkeitDone[2] = array5[22 + num11];
			}
			if (num6 > 23)
			{
				platformScript2.haltbarkeitDone[3] = array5[23 + num11];
			}
			if (num6 > 24)
			{
				platformScript2.haltbarkeitDone[4] = array5[24 + num11];
			}
			if (num6 > 25)
			{
				platformScript2.haltbarkeitDone[5] = array5[25 + num11];
			}
			if (num6 > 26)
			{
				platformScript2.haltbarkeitDone[6] = array5[26 + num11];
			}
			if (num6 > 27)
			{
				platformScript2.haltbarkeitDone[7] = array5[27 + num11];
			}
			if (num6 > 28)
			{
				platformScript2.haltbarkeitDone[8] = array5[28 + num11];
			}
			if (num6 > 29)
			{
				platformScript2.subventionGameSize[0] = array5[29 + num11];
			}
			if (num6 > 30)
			{
				platformScript2.subventionGameSize[1] = array5[30 + num11];
			}
			if (num6 > 31)
			{
				platformScript2.subventionGameSize[2] = array5[31 + num11];
			}
			if (num6 > 32)
			{
				platformScript2.subventionGameSize[3] = array5[32 + num11];
			}
			if (num6 > 33)
			{
				platformScript2.subventionGameSize[4] = array5[33 + num11];
			}
			if (num6 > 34)
			{
				platformScript2.subventionGameSize[5] = array5[34 + num11];
			}
			if (num6 > 35)
			{
				platformScript2.konsoleTab_fullView = array5[35 + num11];
			}
			platformScript2.einnahmen = array6[num13];
			platformScript2.entwicklungsKosten = array6[1 + num13];
			platformScript2.umsatzTotal = array6[2 + num13];
			platformScript2.costs_production = array6[3 + num13];
			platformScript2.costs_garantie = array6[4 + num13];
			platformScript2.garantiekosten = array6[5 + num13];
			platformScript2.costs_subvention = array6[6 + num13];
			if (!platformScript2.OwnerIsNPC())
			{
				platformScript2.hwFeatures = reader.Read<bool[]>("platformA1_" + platformScript2.myID);
				platformScript2.sellsPerWeek = reader.Read<int[]>("platformA2_" + platformScript2.myID);
				platformScript2.publisherBuyed = reader.Read<bool[]>("platformA3_" + platformScript2.myID);
				if (flag)
				{
					platformScript2.garantieMonth = reader.Read<int[]>("platformA4_" + platformScript2.myID);
				}
			}
			platformScript2.Init();
			platformScript2.InitUI();
		}
	}

	private void SaveArbeitsmarkt(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
		int num = array.Length;
		writer.Write<int>("anzArbeitsmarkt", num);
		int[] array2 = new int[14 * num];
		float[] array3 = new float[8 * num];
		string[] array4 = new string[num];
		bool[] array5 = new bool[2 * num];
		int num2 = 0;
		if (array.Length != 0 && (bool)array[0])
		{
			charArbeitsmarkt component = array[0].GetComponent<charArbeitsmarkt>();
			if ((bool)component)
			{
				num2 = component.perks.Length;
			}
		}
		bool[] array6 = new bool[num2 * num];
		for (int i = 0; i < array.Length; i++)
		{
			charArbeitsmarkt component2 = array[i].GetComponent<charArbeitsmarkt>();
			int num3 = i * (array4.Length / num);
			int num4 = i * (array2.Length / num);
			int num5 = i * (array3.Length / num);
			int num6 = i * (array5.Length / num);
			array4[num3] = component2.myName;
			array2[num4] = component2.myID;
			array2[1 + num4] = component2.wochenAmArbeitsmarkt;
			array2[2 + num4] = component2.legend;
			array2[3 + num4] = component2.model_body;
			array2[4 + num4] = component2.model_eyes;
			array2[5 + num4] = component2.model_hair;
			array2[6 + num4] = component2.model_beard;
			array2[7 + num4] = component2.model_skinColor;
			array2[8 + num4] = component2.model_hairColor;
			array2[9 + num4] = component2.model_beardColor;
			array2[10 + num4] = component2.model_HoseColor;
			array2[11 + num4] = component2.model_ShirtColor;
			array2[12 + num4] = component2.model_Add1Color;
			array2[13 + num4] = component2.beruf;
			array3[num5] = component2.s_gamedesign;
			array3[1 + num5] = component2.s_programmieren;
			array3[2 + num5] = component2.s_grafik;
			array3[3 + num5] = component2.s_sound;
			array3[4 + num5] = component2.s_pr;
			array3[5 + num5] = component2.s_gametests;
			array3[6 + num5] = component2.s_technik;
			array3[7 + num5] = component2.s_forschen;
			array5[num6] = component2.male;
			array5[1 + num6] = component2.mitarbeitersuche;
			int num7 = i * num2;
			for (int j = 0; j < component2.perks.Length; j++)
			{
				array6[j + num7] = component2.perks[j];
			}
		}
		writer.Write<int[]>("arbeitsmarkt_I", array2);
		writer.Write<float[]>("arbeitsmarkt_F", array3);
		writer.Write<string[]>("arbeitsmarkt_S", array4);
		writer.Write<bool[]>("arbeitsmarkt_B", array5);
		writer.Write<bool[]>("arbeitsmarkt_perks", array6);
	}

	private void LoadArbeitsmarkt(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzArbeitsmarkt", -1);
		Debug.Log("Load: (anzArbeitsmarkt) " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		bool[] array6 = new bool[0];
		array2 = reader.Read<int[]>("arbeitsmarkt_I");
		array3 = reader.Read<float[]>("arbeitsmarkt_F");
		array4 = reader.Read<string[]>("arbeitsmarkt_S");
		array5 = reader.Read<bool[]>("arbeitsmarkt_B");
		array6 = reader.Read<bool[]>("arbeitsmarkt_perks");
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			int num10 = i * num5;
			charArbeitsmarkt charArbeitsmarkt2 = arbeitsmarkt_.CreateArbeitsmarktItem();
			charArbeitsmarkt2.myName = array4[num10];
			charArbeitsmarkt2.myID = array2[num7];
			charArbeitsmarkt2.wochenAmArbeitsmarkt = array2[1 + num7];
			charArbeitsmarkt2.legend = array2[2 + num7];
			charArbeitsmarkt2.model_body = array2[3 + num7];
			charArbeitsmarkt2.model_eyes = array2[4 + num7];
			charArbeitsmarkt2.model_hair = array2[5 + num7];
			charArbeitsmarkt2.model_beard = array2[6 + num7];
			charArbeitsmarkt2.model_skinColor = array2[7 + num7];
			charArbeitsmarkt2.model_hairColor = array2[8 + num7];
			charArbeitsmarkt2.model_beardColor = array2[9 + num7];
			charArbeitsmarkt2.model_HoseColor = array2[10 + num7];
			charArbeitsmarkt2.model_ShirtColor = array2[11 + num7];
			charArbeitsmarkt2.model_Add1Color = array2[12 + num7];
			charArbeitsmarkt2.beruf = array2[13 + num7];
			charArbeitsmarkt2.s_gamedesign = array3[num8];
			charArbeitsmarkt2.s_programmieren = array3[1 + num8];
			charArbeitsmarkt2.s_grafik = array3[2 + num8];
			charArbeitsmarkt2.s_sound = array3[3 + num8];
			charArbeitsmarkt2.s_pr = array3[4 + num8];
			charArbeitsmarkt2.s_gametests = array3[5 + num8];
			charArbeitsmarkt2.s_technik = array3[6 + num8];
			charArbeitsmarkt2.s_forschen = array3[7 + num8];
			charArbeitsmarkt2.male = array5[num9];
			if (num4 > 1)
			{
				charArbeitsmarkt2.mitarbeitersuche = array5[1 + num9];
			}
			charArbeitsmarkt2.perks = new bool[array6.Length / num];
			int num11 = i * (array6.Length / num);
			for (int j = 0; j < charArbeitsmarkt2.perks.Length; j++)
			{
				charArbeitsmarkt2.perks[j] = array6[j + num11];
			}
			charArbeitsmarkt2.gameObject.name = "AA_" + charArbeitsmarkt2.myID;
		}
	}

	private void SavePublisher(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		int num = array.Length;
		writer.Write<int>("anzPublisher", num);
		long[] array2 = new long[26 * num];
		int[] array3 = new int[61 * num];
		float[] array4 = new float[3 * num];
		string[] array5 = new string[8 * num];
		bool[] array6 = new bool[28 * num];
		for (int i = 0; i < array.Length; i++)
		{
			publisherScript component = array[i].GetComponent<publisherScript>();
			int num2 = i * (array5.Length / num);
			int num3 = i * (array3.Length / num);
			int num4 = i * (array4.Length / num);
			int num5 = i * (array6.Length / num);
			int num6 = i * (array2.Length / num);
			array5[num2] = component.name_EN;
			array5[1 + num2] = component.name_GE;
			array5[2 + num2] = component.name_TU;
			array5[3 + num2] = component.name_CH;
			array5[4 + num2] = component.name_FR;
			array5[5 + num2] = component.name_JA;
			array5[6 + num2] = component.name_UA;
			array5[7 + num2] = component.name_TH;
			array3[num3] = component.myID;
			array3[1 + num3] = component.date_year;
			array3[2 + num3] = component.date_month;
			array3[3 + num3] = component.logoID;
			array3[4 + num3] = component.fanGenre;
			array3[5 + num3] = component.newGameInWeeks;
			array3[6 + num3] = component.exklusivLaufzeit;
			array3[7 + num3] = component.developmentSpeed;
			array3[8 + num3] = component.ownerID;
			array3[9 + num3] = component.newGameInWeeksORG;
			array3[10 + num3] = component.tf_entwicklungsdauer;
			array3[11 + num3] = component.tf_gameSize;
			array3[12 + num3] = component.tf_gameGenre;
			array3[13 + num3] = component.tf_gameTopic;
			array3[14 + num3] = component.tf_ipFocus[0];
			array3[15 + num3] = component.lockToBuy;
			array3[16 + num3] = component.tf_ipFocus[1];
			array3[17 + num3] = component.tf_ipFocus[2];
			array3[18 + num3] = component.tf_engine;
			array3[19 + num3] = component.tf_autoReleaseVal;
			array3[20 + num3] = component.tf_platformFocus[0];
			array3[21 + num3] = component.tf_platformFocus[1];
			array3[22 + num3] = component.tf_platformFocus[2];
			array3[23 + num3] = component.tf_platformFocus[3];
			array3[24 + num3] = component.country;
			array3[25 + num3] = component.awards[0];
			array3[26 + num3] = component.awards[1];
			array3[27 + num3] = component.awards[2];
			array3[28 + num3] = component.awards[3];
			array3[29 + num3] = component.awards[4];
			array3[30 + num3] = component.awards[5];
			array3[31 + num3] = component.awards[6];
			array3[32 + num3] = component.awards[7];
			array3[33 + num3] = component.awards[8];
			array3[34 + num3] = component.awards[9];
			array3[35 + num3] = component.awards[10];
			array3[36 + num3] = component.awards[11];
			array3[37 + num3] = component.awards[12];
			array3[38 + num3] = component.awards[13];
			array3[39 + num3] = component.awards[14];
			array3[40 + num3] = component.awards[15];
			array3[41 + num3] = component.awards[16];
			array3[42 + num3] = component.awards[17];
			array3[43 + num3] = component.awards[18];
			array3[44 + num3] = component.awards[19];
			array3[45 + num3] = component.awards[20];
			array3[46 + num3] = component.awards[21];
			array3[47 + num3] = component.awards[22];
			array3[48 + num3] = component.awards[23];
			array3[49 + num3] = component.awards[24];
			array3[50 + num3] = component.awards[25];
			array3[51 + num3] = component.awards[26];
			array3[52 + num3] = component.awards[27];
			array3[53 + num3] = component.awards[28];
			array3[54 + num3] = component.awards[29];
			array3[55 + num3] = component.tf_ownPublisherPriority;
			array3[56 + num3] = component.date_year_end;
			array3[57 + num3] = component.date_month_end;
			array3[58 + num3] = component.tf_ipFocus[3];
			array3[59 + num3] = component.tf_ipFocus[4];
			array3[60 + num3] = component.tf_ipFocus[5];
			array4[num4] = component.stars;
			array4[1 + num4] = component.relation;
			array4[2 + num4] = component.share;
			array6[num5] = component.isUnlocked;
			array6[1 + num5] = component.developer;
			array6[2 + num5] = component.publisher;
			array6[3 + num5] = component.ownPlatform;
			array6[4 + num5] = component.exklusive;
			array6[5 + num5] = component.notForSale;
			array6[7 + num5] = component.tf_geschlossen;
			array6[8 + num5] = component.tf_autoRelease;
			array6[9 + num5] = component.tf_onlyPlayerConsole;
			array6[10 + num5] = component.tf_allowMMO;
			array6[11 + num5] = component.tf_allowF2P;
			array6[12 + num5] = component.tf_allowAddon;
			array6[13 + num5] = component.tf_noArcade;
			array6[14 + num5] = component.tf_noHandy;
			array6[15 + num5] = component.tf_noRetro;
			array6[16 + num5] = component.tf_noPorts;
			array6[17 + num5] = component.tf_noBudget;
			array6[18 + num5] = component.tf_noGOTY;
			array6[19 + num5] = component.tf_publisher;
			array6[20 + num5] = component.tf_developer;
			array6[21 + num5] = component.tf_noRemaster;
			array6[22 + num5] = component.tf_noSpinoffs;
			array6[23 + num5] = component.tf_ownPublisher;
			array6[24 + num5] = component.isPlayer;
			array6[25 + num5] = component.tf_noBundles;
			array6[26 + num5] = component.tf_noAddonBundles;
			array6[27 + num5] = component.tf_autoGamePass;
			array2[num6] = component.firmenwert;
			array2[1 + num6] = component.tf_umsatz[0];
			array2[2 + num6] = component.tf_umsatz[1];
			array2[3 + num6] = component.tf_umsatz[2];
			array2[4 + num6] = component.tf_umsatz[3];
			array2[5 + num6] = component.tf_umsatz[4];
			array2[6 + num6] = component.tf_umsatz[5];
			array2[7 + num6] = component.tf_umsatz[6];
			array2[8 + num6] = component.tf_umsatz[7];
			array2[9 + num6] = component.tf_umsatz[8];
			array2[10 + num6] = component.tf_umsatz[9];
			array2[11 + num6] = component.tf_umsatz[10];
			array2[12 + num6] = component.tf_umsatz[11];
			array2[13 + num6] = component.tf_umsatz[12];
			array2[14 + num6] = component.tf_umsatz[13];
			array2[15 + num6] = component.tf_umsatz[14];
			array2[16 + num6] = component.tf_umsatz[15];
			array2[17 + num6] = component.tf_umsatz[16];
			array2[18 + num6] = component.tf_umsatz[17];
			array2[19 + num6] = component.tf_umsatz[18];
			array2[20 + num6] = component.tf_umsatz[19];
			array2[21 + num6] = component.tf_umsatz[20];
			array2[22 + num6] = component.tf_umsatz[21];
			array2[23 + num6] = component.tf_umsatz[22];
			array2[24 + num6] = component.tf_umsatz[23];
			array2[25 + num6] = component.tf_umsatz_allTime;
		}
		writer.Write<int[]>("publisher_I", array3);
		writer.Write<float[]>("publisher_F", array4);
		writer.Write<string[]>("publisher_S", array5);
		writer.Write<bool[]>("publisher_B", array6);
		writer.Write<long[]>("publisher_L", array2);
	}

	private void LoadPublisher(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzPublisher", -1);
		Debug.Log("Load: (anzPublisher) " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		array2 = reader.Read<int[]>("publisher_I");
		array3 = reader.Read<float[]>("publisher_F");
		array4 = reader.Read<string[]>("publisher_S");
		array5 = reader.Read<bool[]>("publisher_B");
		if (es3file.KeyExists("publisher_L"))
		{
			array = reader.Read<long[]>("publisher_L");
		}
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			int num10 = i * num5;
			int num11 = i * num6;
			publisherScript publisherScript2 = publisher_.CreatePublisher();
			publisherScript2.name_EN = array4[num10];
			publisherScript2.name_GE = array4[1 + num10];
			publisherScript2.name_TU = array4[2 + num10];
			publisherScript2.name_CH = array4[3 + num10];
			publisherScript2.name_FR = array4[4 + num10];
			publisherScript2.name_JA = array4[5 + num10];
			if (num5 > 6)
			{
				publisherScript2.name_UA = array4[6 + num10];
			}
			if (num5 > 7)
			{
				publisherScript2.name_TH = array4[7 + num10];
			}
			publisherScript2.myID = array2[num7];
			publisherScript2.date_year = array2[1 + num7];
			publisherScript2.date_month = array2[2 + num7];
			publisherScript2.logoID = array2[3 + num7];
			publisherScript2.fanGenre = array2[4 + num7];
			publisherScript2.newGameInWeeks = array2[5 + num7];
			publisherScript2.exklusivLaufzeit = array2[6 + num7];
			publisherScript2.developmentSpeed = array2[7 + num7];
			publisherScript2.ownerID = array2[8 + num7];
			publisherScript2.newGameInWeeksORG = array2[9 + num7];
			if (num2 > 10)
			{
				publisherScript2.tf_entwicklungsdauer = array2[10 + num7];
			}
			if (num2 > 11)
			{
				publisherScript2.tf_gameSize = array2[11 + num7];
			}
			if (num2 > 12)
			{
				publisherScript2.tf_gameGenre = array2[12 + num7];
			}
			if (num2 > 13)
			{
				publisherScript2.tf_gameTopic = array2[13 + num7];
			}
			else
			{
				publisherScript2.tf_gameTopic = -1;
			}
			if (num2 > 14)
			{
				publisherScript2.tf_ipFocus[0] = array2[14 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[0] = -1;
			}
			if (num2 > 15)
			{
				publisherScript2.lockToBuy = array2[15 + num7];
			}
			if (num2 > 16)
			{
				publisherScript2.tf_ipFocus[1] = array2[16 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[1] = -1;
			}
			if (num2 > 17)
			{
				publisherScript2.tf_ipFocus[2] = array2[17 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[2] = -1;
			}
			if (num2 > 18)
			{
				publisherScript2.tf_engine = array2[18 + num7];
			}
			else
			{
				publisherScript2.tf_engine = -1;
			}
			if (num2 > 19)
			{
				publisherScript2.tf_autoReleaseVal = array2[19 + num7];
			}
			if (num2 > 20)
			{
				publisherScript2.tf_platformFocus[0] = array2[20 + num7];
			}
			else
			{
				publisherScript2.tf_platformFocus[0] = -1;
			}
			if (num2 > 21)
			{
				publisherScript2.tf_platformFocus[1] = array2[21 + num7];
			}
			else
			{
				publisherScript2.tf_platformFocus[1] = -1;
			}
			if (num2 > 22)
			{
				publisherScript2.tf_platformFocus[2] = array2[22 + num7];
			}
			else
			{
				publisherScript2.tf_platformFocus[2] = -1;
			}
			if (num2 > 23)
			{
				publisherScript2.tf_platformFocus[3] = array2[23 + num7];
			}
			else
			{
				publisherScript2.tf_platformFocus[3] = -1;
			}
			if (num2 > 24)
			{
				publisherScript2.country = array2[24 + num7];
			}
			if (num2 > 25)
			{
				publisherScript2.awards[0] = array2[25 + num7];
			}
			if (num2 > 26)
			{
				publisherScript2.awards[1] = array2[26 + num7];
			}
			if (num2 > 27)
			{
				publisherScript2.awards[2] = array2[27 + num7];
			}
			if (num2 > 28)
			{
				publisherScript2.awards[3] = array2[28 + num7];
			}
			if (num2 > 29)
			{
				publisherScript2.awards[4] = array2[29 + num7];
			}
			if (num2 > 30)
			{
				publisherScript2.awards[5] = array2[30 + num7];
			}
			if (num2 > 31)
			{
				publisherScript2.awards[6] = array2[31 + num7];
			}
			if (num2 > 32)
			{
				publisherScript2.awards[7] = array2[32 + num7];
			}
			if (num2 > 33)
			{
				publisherScript2.awards[8] = array2[33 + num7];
			}
			if (num2 > 34)
			{
				publisherScript2.awards[9] = array2[34 + num7];
			}
			if (num2 > 35)
			{
				publisherScript2.awards[10] = array2[35 + num7];
			}
			if (num2 > 36)
			{
				publisherScript2.awards[11] = array2[36 + num7];
			}
			if (num2 > 37)
			{
				publisherScript2.awards[12] = array2[37 + num7];
			}
			if (num2 > 38)
			{
				publisherScript2.awards[13] = array2[38 + num7];
			}
			if (num2 > 39)
			{
				publisherScript2.awards[14] = array2[39 + num7];
			}
			if (num2 > 40)
			{
				publisherScript2.awards[15] = array2[40 + num7];
			}
			if (num2 > 41)
			{
				publisherScript2.awards[16] = array2[41 + num7];
			}
			if (num2 > 42)
			{
				publisherScript2.awards[17] = array2[42 + num7];
			}
			if (num2 > 43)
			{
				publisherScript2.awards[18] = array2[43 + num7];
			}
			if (num2 > 44)
			{
				publisherScript2.awards[19] = array2[44 + num7];
			}
			if (num2 > 45)
			{
				publisherScript2.awards[20] = array2[45 + num7];
			}
			if (num2 > 46)
			{
				publisherScript2.awards[21] = array2[46 + num7];
			}
			if (num2 > 47)
			{
				publisherScript2.awards[22] = array2[47 + num7];
			}
			if (num2 > 48)
			{
				publisherScript2.awards[23] = array2[48 + num7];
			}
			if (num2 > 49)
			{
				publisherScript2.awards[24] = array2[49 + num7];
			}
			if (num2 > 50)
			{
				publisherScript2.awards[25] = array2[50 + num7];
			}
			if (num2 > 51)
			{
				publisherScript2.awards[26] = array2[51 + num7];
			}
			if (num2 > 52)
			{
				publisherScript2.awards[27] = array2[52 + num7];
			}
			if (num2 > 53)
			{
				publisherScript2.awards[28] = array2[53 + num7];
			}
			if (num2 > 54)
			{
				publisherScript2.awards[29] = array2[54 + num7];
			}
			if (num2 > 55)
			{
				publisherScript2.tf_ownPublisherPriority = array2[55 + num7];
			}
			else
			{
				publisherScript2.tf_ownPublisherPriority = -1;
			}
			if (num2 > 56)
			{
				publisherScript2.date_year_end = array2[56 + num7];
			}
			if (num2 > 57)
			{
				publisherScript2.date_month_end = array2[57 + num7];
			}
			if (num2 > 58)
			{
				publisherScript2.tf_ipFocus[3] = array2[58 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[3] = -1;
			}
			if (num2 > 59)
			{
				publisherScript2.tf_ipFocus[4] = array2[59 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[4] = -1;
			}
			if (num2 > 60)
			{
				publisherScript2.tf_ipFocus[5] = array2[60 + num7];
			}
			else
			{
				publisherScript2.tf_ipFocus[5] = -1;
			}
			publisherScript2.stars = array3[num8];
			publisherScript2.relation = array3[1 + num8];
			publisherScript2.share = array3[2 + num8];
			publisherScript2.isUnlocked = array5[num9];
			publisherScript2.developer = array5[1 + num9];
			publisherScript2.publisher = array5[2 + num9];
			publisherScript2.ownPlatform = array5[3 + num9];
			publisherScript2.exklusive = array5[4 + num9];
			publisherScript2.notForSale = array5[5 + num9];
			publisherScript2.tf_geschlossen = array5[7 + num9];
			publisherScript2.tf_autoRelease = array5[8 + num9];
			publisherScript2.tf_onlyPlayerConsole = array5[9 + num9];
			if (num4 > 10)
			{
				publisherScript2.tf_allowMMO = array5[10 + num9];
			}
			if (num4 > 11)
			{
				publisherScript2.tf_allowF2P = array5[11 + num9];
			}
			if (num4 > 12)
			{
				publisherScript2.tf_allowAddon = array5[12 + num9];
			}
			if (num4 > 13)
			{
				publisherScript2.tf_noArcade = array5[13 + num9];
			}
			if (num4 > 14)
			{
				publisherScript2.tf_noHandy = array5[14 + num9];
			}
			if (num4 > 15)
			{
				publisherScript2.tf_noRetro = array5[15 + num9];
			}
			if (num4 > 16)
			{
				publisherScript2.tf_noPorts = array5[16 + num9];
			}
			if (num4 > 17)
			{
				publisherScript2.tf_noBudget = array5[17 + num9];
			}
			if (num4 > 18)
			{
				publisherScript2.tf_noGOTY = array5[18 + num9];
			}
			if (num4 > 19)
			{
				publisherScript2.tf_publisher = array5[19 + num9];
			}
			if (num4 > 20)
			{
				publisherScript2.tf_developer = array5[20 + num9];
			}
			if (num4 > 21)
			{
				publisherScript2.tf_noRemaster = array5[21 + num9];
			}
			if (num4 > 22)
			{
				publisherScript2.tf_noSpinoffs = array5[22 + num9];
			}
			if (num4 > 23)
			{
				publisherScript2.tf_ownPublisher = array5[23 + num9];
			}
			if (num4 > 24)
			{
				publisherScript2.isPlayer = array5[24 + num9];
			}
			if (num4 > 25)
			{
				publisherScript2.tf_noBundles = array5[25 + num9];
			}
			if (num4 > 26)
			{
				publisherScript2.tf_noAddonBundles = array5[26 + num9];
			}
			if (num4 > 27)
			{
				publisherScript2.tf_autoGamePass = array5[27 + num9];
			}
			if (num6 > 0)
			{
				publisherScript2.firmenwert = array[num11];
			}
			if (num6 > 1)
			{
				publisherScript2.tf_umsatz[0] = array[1 + num11];
			}
			if (num6 > 2)
			{
				publisherScript2.tf_umsatz[1] = array[2 + num11];
			}
			if (num6 > 3)
			{
				publisherScript2.tf_umsatz[2] = array[3 + num11];
			}
			if (num6 > 4)
			{
				publisherScript2.tf_umsatz[3] = array[4 + num11];
			}
			if (num6 > 5)
			{
				publisherScript2.tf_umsatz[4] = array[5 + num11];
			}
			if (num6 > 6)
			{
				publisherScript2.tf_umsatz[5] = array[6 + num11];
			}
			if (num6 > 7)
			{
				publisherScript2.tf_umsatz[6] = array[7 + num11];
			}
			if (num6 > 8)
			{
				publisherScript2.tf_umsatz[7] = array[8 + num11];
			}
			if (num6 > 9)
			{
				publisherScript2.tf_umsatz[8] = array[9 + num11];
			}
			if (num6 > 10)
			{
				publisherScript2.tf_umsatz[9] = array[10 + num11];
			}
			if (num6 > 11)
			{
				publisherScript2.tf_umsatz[10] = array[11 + num11];
			}
			if (num6 > 12)
			{
				publisherScript2.tf_umsatz[11] = array[12 + num11];
			}
			if (num6 > 13)
			{
				publisherScript2.tf_umsatz[12] = array[13 + num11];
			}
			if (num6 > 14)
			{
				publisherScript2.tf_umsatz[13] = array[14 + num11];
			}
			if (num6 > 15)
			{
				publisherScript2.tf_umsatz[14] = array[15 + num11];
			}
			if (num6 > 16)
			{
				publisherScript2.tf_umsatz[15] = array[16 + num11];
			}
			if (num6 > 17)
			{
				publisherScript2.tf_umsatz[16] = array[17 + num11];
			}
			if (num6 > 18)
			{
				publisherScript2.tf_umsatz[17] = array[18 + num11];
			}
			if (num6 > 19)
			{
				publisherScript2.tf_umsatz[18] = array[19 + num11];
			}
			if (num6 > 20)
			{
				publisherScript2.tf_umsatz[19] = array[20 + num11];
			}
			if (num6 > 21)
			{
				publisherScript2.tf_umsatz[20] = array[21 + num11];
			}
			if (num6 > 22)
			{
				publisherScript2.tf_umsatz[21] = array[22 + num11];
			}
			if (num6 > 23)
			{
				publisherScript2.tf_umsatz[22] = array[23 + num11];
			}
			if (num6 > 24)
			{
				publisherScript2.tf_umsatz[23] = array[24 + num11];
			}
			if (num6 > 25)
			{
				publisherScript2.tf_umsatz_allTime = array[25 + num11];
			}
			publisherScript2.Init();
		}
		RecalculateShare();
	}

	private void RecalculateShare()
	{
		bool flag = false;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			publisherScript component = array[i].GetComponent<publisherScript>();
			if ((bool)component && !component.IsTochterfirma() && !component.isPlayer && component.share <= 5f)
			{
				flag = true;
				Debug.Log("RecalculateShare() -> " + component.GetName());
				break;
			}
		}
		if (!flag)
		{
			return;
		}
		Debug.Log("RecalculateShare()");
		for (int j = 0; j < array.Length; j++)
		{
			publisherScript component2 = array[j].GetComponent<publisherScript>();
			if ((bool)component2 && !component2.IsTochterfirma() && !component2.isPlayer)
			{
				switch (Mathf.RoundToInt(component2.share))
				{
				case 4:
					component2.share = 8f;
					break;
				case 5:
					component2.share = 8f;
					break;
				case 6:
					component2.share = 10f;
					break;
				case 7:
					component2.share = 11f;
					break;
				case 8:
					component2.share = 12f;
					break;
				case 9:
					component2.share = 13f;
					break;
				}
			}
		}
	}

	private void SaveEngines(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Engine");
		int num = array.Length;
		writer.Write<int>("anzEngines", num);
		int[] array2 = new int[11 * num];
		float[] array3 = new float[3 * num];
		string[] array4 = new string[16 * num];
		bool[] array5 = new bool[6 * num];
		int num2 = eF_.engineFeatures_UNLOCK.Length;
		int num3 = eF_.engineFeatures_UNLOCK.Length;
		int num4 = GameObject.FindGameObjectsWithTag("Publisher").Length;
		bool[] array6 = new bool[num2 * num];
		bool[] array7 = new bool[num3 * num];
		bool[] array8 = new bool[num4 * num];
		for (int i = 0; i < array.Length; i++)
		{
			engineScript component = array[i].GetComponent<engineScript>();
			int num5 = i * (array4.Length / num);
			int num6 = i * (array2.Length / num);
			int num7 = i * (array3.Length / num);
			int num8 = i * (array5.Length / num);
			array4[num5] = component.myName;
			array4[1 + num5] = component.name_EN;
			array4[2 + num5] = component.name_GE;
			array4[3 + num5] = component.name_TU;
			array4[4 + num5] = component.name_CH;
			array4[5 + num5] = component.name_FR;
			array4[6 + num5] = component.name_HU;
			array4[7 + num5] = component.name_CT;
			array4[8 + num5] = component.name_CZ;
			array4[9 + num5] = component.name_PB;
			array4[10 + num5] = component.name_IT;
			array4[11 + num5] = component.name_JA;
			array4[12 + num5] = component.name_PL;
			array4[13 + num5] = component.name_UA;
			array4[14 + num5] = component.name_TH;
			array4[15 + num5] = component.name_RU;
			array2[num6] = component.myID;
			array2[1 + num6] = component.spezialgenre;
			array2[2 + num6] = component.preis;
			array2[3 + num6] = component.gewinnbeteiligung;
			array2[4 + num6] = component.date_year;
			array2[5 + num6] = component.date_month;
			array2[6 + num6] = component.ownerID;
			array2[7 + num6] = component.spezialplatform;
			array2[8 + num6] = component.umsatz;
			array2[9 + num6] = component.spezialplatformUpdate;
			array2[10 + num6] = component.amountSellToPlayer;
			array3[num7] = component.devPoints;
			array3[1 + num7] = component.devPointsStart;
			array3[2 + num7] = component.marktdominanz;
			array5[1 + num8] = component.isUnlocked;
			array5[2 + num8] = component.gekauft;
			array5[3 + num8] = component.sellEngine;
			array5[4 + num8] = component.updating;
			array5[5 + num8] = component.archiv_engine;
			if (component.features == null || component.features.Length == 0)
			{
				component.features = new bool[num2];
			}
			int num9 = i * num2;
			for (int j = 0; j < num2; j++)
			{
				array6[j + num9] = component.features[j];
			}
			if (component.featuresInDev == null || component.featuresInDev.Length == 0)
			{
				component.featuresInDev = new bool[num3];
			}
			num9 = i * num3;
			for (int k = 0; k < num3; k++)
			{
				array7[k + num9] = component.featuresInDev[k];
			}
			if (component.publisherBuyed == null || component.publisherBuyed.Length == 0)
			{
				component.publisherBuyed = new bool[num4];
			}
			num9 = i * num4;
			for (int l = 0; l < num4; l++)
			{
				array8[l + num9] = component.publisherBuyed[l];
			}
		}
		writer.Write<int[]>("engines_I", array2);
		writer.Write<float[]>("engines_F", array3);
		writer.Write<string[]>("engines_S", array4);
		writer.Write<bool[]>("engines_B", array5);
		writer.Write<bool[]>("engines_features", array6);
		writer.Write<bool[]>("engines_featuresInDev", array7);
		writer.Write<bool[]>("engines_publisherBuyed", array8);
	}

	private void LoadEngines(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzEngines", -1);
		Debug.Log("Load: (anzEngines) " + num);
		if (num <= 0)
		{
			return;
		}
		long[] array = new long[0];
		int[] array2 = new int[0];
		float[] array3 = new float[0];
		string[] array4 = new string[0];
		bool[] array5 = new bool[0];
		bool[] array6 = new bool[0];
		bool[] array7 = new bool[0];
		bool[] array8 = new bool[0];
		array2 = reader.Read<int[]>("engines_I");
		array3 = reader.Read<float[]>("engines_F");
		array4 = reader.Read<string[]>("engines_S");
		array5 = reader.Read<bool[]>("engines_B");
		int num2 = array2.Length / num;
		int num3 = array3.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array.Length / num;
		array6 = reader.Read<bool[]>("engines_features");
		array7 = reader.Read<bool[]>("engines_featuresInDev");
		array8 = reader.Read<bool[]>("engines_publisherBuyed");
		for (int i = 0; i < num; i++)
		{
			int num7 = i * num2;
			int num8 = i * num3;
			int num9 = i * num4;
			int num10 = i * num5;
			engineScript engineScript2 = eF_.CreateEngine();
			engineScript2.myName = array4[num10];
			engineScript2.name_EN = array4[1 + num10];
			engineScript2.name_GE = array4[2 + num10];
			engineScript2.name_TU = array4[3 + num10];
			engineScript2.name_CH = array4[4 + num10];
			engineScript2.name_FR = array4[5 + num10];
			engineScript2.name_HU = array4[6 + num10];
			engineScript2.name_CT = array4[7 + num10];
			engineScript2.name_CZ = array4[8 + num10];
			engineScript2.name_PB = array4[9 + num10];
			engineScript2.name_IT = array4[10 + num10];
			engineScript2.name_JA = array4[11 + num10];
			engineScript2.name_PL = array4[12 + num10];
			if (num5 > 13)
			{
				engineScript2.name_UA = array4[13 + num10];
			}
			if (num5 > 14)
			{
				engineScript2.name_TH = array4[14 + num10];
			}
			if (num5 > 15)
			{
				engineScript2.name_RU = array4[15 + num10];
			}
			engineScript2.myID = array2[num7];
			engineScript2.spezialgenre = array2[1 + num7];
			engineScript2.preis = array2[2 + num7];
			engineScript2.gewinnbeteiligung = array2[3 + num7];
			engineScript2.date_year = array2[4 + num7];
			engineScript2.date_month = array2[5 + num7];
			engineScript2.ownerID = array2[6 + num7];
			engineScript2.spezialplatform = array2[7 + num7];
			engineScript2.umsatz = array2[8 + num7];
			engineScript2.spezialplatformUpdate = array2[9 + num7];
			if (num2 > 10)
			{
				engineScript2.amountSellToPlayer = array2[10 + num7];
			}
			engineScript2.devPoints = array3[num8];
			engineScript2.devPointsStart = array3[1 + num8];
			if (num3 > 2)
			{
				engineScript2.marktdominanz = array3[2 + num8];
			}
			engineScript2.isUnlocked = array5[1 + num9];
			engineScript2.gekauft = array5[2 + num9];
			engineScript2.sellEngine = array5[3 + num9];
			engineScript2.updating = array5[4 + num9];
			engineScript2.archiv_engine = array5[5 + num9];
			engineScript2.features = new bool[array6.Length / num];
			engineScript2.featuresInDev = new bool[array7.Length / num];
			engineScript2.publisherBuyed = new bool[array8.Length / num];
			int num11 = i * (array6.Length / num);
			for (int j = 0; j < engineScript2.features.Length; j++)
			{
				engineScript2.features[j] = array6[j + num11];
			}
			num11 = i * (array7.Length / num);
			for (int k = 0; k < engineScript2.featuresInDev.Length; k++)
			{
				engineScript2.featuresInDev[k] = array7[k + num11];
			}
			num11 = i * (array8.Length / num);
			for (int l = 0; l < engineScript2.publisherBuyed.Length; l++)
			{
				engineScript2.publisherBuyed[l] = array8[l + num11];
			}
			engineScript2.Init();
		}
	}

	private void SaveGames(ES3Writer writer)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		int num = array.Length;
		writer.Write<int>("anzGames", num);
		long[] array2 = new long[64 * num];
		int[] array3 = new int[136 * num];
		float[] array4 = new float[52 * num];
		string[] array5 = new string[4 * num];
		bool[] array6 = new bool[141 * num];
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		if (array.Length != 0 && (bool)array[0])
		{
			gameScript component = array[0].GetComponent<gameScript>();
			if ((bool)component)
			{
				num2 = component.gameGameplayFeatures.Length;
				num3 = component.gameplayFeatures_DevDone.Length;
				num4 = component.fanbrief.Length;
			}
		}
		bool[] array7 = new bool[num2 * num];
		bool[] array8 = new bool[num3 * num];
		bool[] array9 = new bool[num4 * num];
		for (int i = 0; i < array.Length; i++)
		{
			gameScript component2 = array[i].GetComponent<gameScript>();
			int num5 = i * (array5.Length / num);
			array5[num5] = component2.GetNameSimple();
			array5[1 + num5] = component2.beschreibung;
			array5[2 + num5] = component2.ipName;
			array5[3 + num5] = component2.myNameTeil1;
			int num6 = i * (array3.Length / num);
			array3[num6] = component2.myID;
			array3[1 + num6] = component2.developerID;
			array3[2 + num6] = component2.engineID;
			array3[3 + num6] = component2.reviewGameplay;
			array3[4 + num6] = component2.reviewGrafik;
			array3[5 + num6] = component2.reviewSound;
			array3[6 + num6] = component2.reviewSteuerung;
			array3[7 + num6] = component2.reviewTotal;
			array3[8 + num6] = component2.reviewGameplayText;
			array3[9 + num6] = component2.reviewGrafikText;
			array3[10 + num6] = component2.reviewSoundText;
			array3[11 + num6] = component2.reviewSteuerungText;
			array3[12 + num6] = component2.reviewTotalText;
			array3[13 + num6] = component2.date_year;
			array3[14 + num6] = component2.date_month;
			array3[15 + num6] = component2.aboPreisOld;
			array3[16 + num6] = component2.devAktFeature;
			array3[17 + num6] = component2.gameTyp;
			array3[18 + num6] = component2.gameSize;
			array3[19 + num6] = component2.gameZielgruppe;
			array3[20 + num6] = component2.maingenre;
			array3[21 + num6] = component2.subgenre;
			array3[22 + num6] = component2.gameMainTheme;
			array3[23 + num6] = component2.gameSubTheme;
			array3[24 + num6] = component2.gameLicence;
			array3[25 + num6] = component2.gameCopyProtect;
			array3[26 + num6] = component2.auftragsspiel_gehalt;
			array3[27 + num6] = component2.auftragsspiel_bonus;
			array3[28 + num6] = component2.auftragsspiel_zeitInWochen;
			array3[29 + num6] = component2.auftragsspiel_wochenAlsAngebot;
			array3[30 + num6] = component2.auftragsspiel_mindestbewertung;
			array3[31 + num6] = component2.gameAP_Gameplay;
			array3[32 + num6] = component2.gameAP_Grafik;
			array3[33 + num6] = component2.gameAP_Sound;
			array3[34 + num6] = component2.gameAP_Technik;
			array3[35 + num6] = component2.gameEngineFeature[0];
			array3[36 + num6] = component2.gameEngineFeature[1];
			array3[37 + num6] = component2.gameEngineFeature[2];
			array3[38 + num6] = component2.gameEngineFeature[3];
			array3[39 + num6] = component2.gamePlatform[0];
			array3[40 + num6] = component2.gamePlatform[1];
			array3[41 + num6] = component2.gamePlatform[2];
			array3[42 + num6] = component2.gamePlatform[3];
			array3[43 + num6] = component2.publisherID;
			array3[44 + num6] = component2.weeksOnMarket;
			array3[45 + num6] = component2.engineGewinnbeteiligung;
			array3[46 + num6] = component2.teile;
			array3[48 + num6] = component2.amountUpdates;
			array3[49 + num6] = component2.amountAddons;
			array3[50 + num6] = component2.amountMMOAddons;
			array3[51 + num6] = component2.usk;
			array3[52 + num6] = component2.devFortsetzen;
			array3[53 + num6] = component2.verkaufspreis[0];
			array3[54 + num6] = component2.verkaufspreis[1];
			array3[55 + num6] = component2.verkaufspreis[2];
			array3[56 + num6] = component2.verkaufspreis[3];
			array3[57 + num6] = component2.releaseDate;
			array3[58 + num6] = component2.vorbestellungen;
			array3[66 + num6] = component2.freigabeBudget;
			array3[67 + num6] = component2.sellsPerWeek[0];
			array3[68 + num6] = component2.sellsPerWeek[1];
			array3[69 + num6] = component2.sellsPerWeek[2];
			array3[70 + num6] = component2.sellsPerWeek[3];
			array3[71 + num6] = component2.sellsPerWeek[4];
			array3[72 + num6] = component2.sellsPerWeek[5];
			array3[73 + num6] = component2.sellsPerWeek[6];
			array3[74 + num6] = component2.sellsPerWeek[7];
			array3[75 + num6] = component2.sellsPerWeek[8];
			array3[76 + num6] = component2.sellsPerWeek[9];
			array3[77 + num6] = component2.sellsPerWeek[10];
			array3[78 + num6] = component2.sellsPerWeek[11];
			array3[79 + num6] = component2.sellsPerWeek[12];
			array3[80 + num6] = component2.sellsPerWeek[13];
			array3[81 + num6] = component2.sellsPerWeek[14];
			array3[82 + num6] = component2.sellsPerWeek[15];
			array3[83 + num6] = component2.sellsPerWeek[16];
			array3[84 + num6] = component2.sellsPerWeek[17];
			array3[85 + num6] = component2.sellsPerWeek[18];
			array3[86 + num6] = component2.sellsPerWeek[19];
			array3[87 + num6] = component2.finanzierung_Grundkosten;
			array3[88 + num6] = component2.finanzierung_Technology;
			array3[89 + num6] = component2.finanzierung_Kontent;
			array3[90 + num6] = component2.gameAntiCheat;
			array3[93 + num6] = component2.aboPreis;
			array3[94 + num6] = component2.abosAddons;
			array3[95 + num6] = component2.lastChartPosition;
			array3[96 + num6] = component2.date_start_year;
			array3[97 + num6] = component2.date_start_month;
			array3[98 + num6] = component2.userPositiv;
			array3[99 + num6] = component2.userNegativ;
			array3[100 + num6] = component2.merchBestellungen[0];
			array3[101 + num6] = component2.merchBestellungen[1];
			array3[102 + num6] = component2.merchBestellungen[2];
			array3[103 + num6] = component2.merchBestellungen[3];
			array3[104 + num6] = component2.merchBestellungen[4];
			array3[105 + num6] = component2.merchBestellungen[5];
			array3[106 + num6] = component2.merchBestellungen[6];
			array3[107 + num6] = component2.merchBestellungen[7];
			array3[108 + num6] = component2.merchGesamtSells[0];
			array3[109 + num6] = component2.merchGesamtSells[1];
			array3[110 + num6] = component2.merchGesamtSells[2];
			array3[111 + num6] = component2.merchGesamtSells[3];
			array3[112 + num6] = component2.merchGesamtSells[4];
			array3[113 + num6] = component2.merchGesamtSells[5];
			array3[114 + num6] = component2.merchGesamtSells[6];
			array3[115 + num6] = component2.merchGesamtSells[7];
			array3[116 + num6] = component2.merchDiesenMonat[0];
			array3[117 + num6] = component2.merchDiesenMonat[1];
			array3[118 + num6] = component2.merchDiesenMonat[2];
			array3[119 + num6] = component2.merchDiesenMonat[3];
			array3[120 + num6] = component2.merchDiesenMonat[4];
			array3[121 + num6] = component2.merchDiesenMonat[5];
			array3[122 + num6] = component2.merchDiesenMonat[6];
			array3[123 + num6] = component2.merchDiesenMonat[7];
			array3[124 + num6] = component2.merchLetzterMonat[0];
			array3[125 + num6] = component2.merchLetzterMonat[1];
			array3[126 + num6] = component2.merchLetzterMonat[2];
			array3[127 + num6] = component2.merchLetzterMonat[3];
			array3[128 + num6] = component2.merchLetzterMonat[4];
			array3[129 + num6] = component2.merchLetzterMonat[5];
			array3[130 + num6] = component2.merchLetzterMonat[6];
			array3[131 + num6] = component2.merchLetzterMonat[7];
			array3[132 + num6] = component2.sonderIPMindestreview;
			array3[134 + num6] = component2.ownerID;
			array3[135 + num6] = component2.gamePassPlayer;
			int num7 = i * (array4.Length / num);
			array4[num7] = component2.hype;
			array4[1 + num7] = component2.devPoints;
			array4[2 + num7] = component2.devPointsStart;
			array4[3 + num7] = component2.devPoints_Gesamt;
			array4[4 + num7] = component2.devPointsStart_Gesamt;
			array4[5 + num7] = component2.points_gameplay;
			array4[6 + num7] = component2.points_grafik;
			array4[7 + num7] = component2.points_sound;
			array4[8 + num7] = component2.points_technik;
			array4[9 + num7] = component2.points_bugs;
			array4[10 + num7] = component2.bonusSellsUpdates;
			array4[11 + num7] = component2.bonusSellsAddons;
			array4[12 + num7] = 0f;
			array4[13 + num7] = component2.addonQuality;
			array4[14 + num7] = component2.f2pInteresse;
			array4[15 + num7] = component2.mmoInteresse;
			array4[16 + num7] = component2.ipPunkte;
			array4[17 + num7] = component2.merchGesamtReviewPoints;
			array4[18 + num7] = component2.points_bugsInvis;
			array4[19 + num7] = component2.pubAngebot_Verhandlung;
			array4[20 + num7] = component2.pubAngebot_VerhandlungProzent;
			array4[21 + num7] = component2.pubAngebot_Stimmung;
			array4[22 + num7] = component2.pubAngebot_Gewinnbeteiligung;
			array4[23 + num7] = component2.merchVerkaufspreis[0];
			array4[24 + num7] = component2.merchVerkaufspreis[1];
			array4[25 + num7] = component2.merchVerkaufspreis[2];
			array4[26 + num7] = component2.merchVerkaufspreis[3];
			array4[27 + num7] = component2.merchVerkaufspreis[4];
			array4[28 + num7] = component2.merchVerkaufspreis[5];
			array4[29 + num7] = component2.merchVerkaufspreis[6];
			array4[30 + num7] = component2.merchVerkaufspreis[7];
			array4[31 + num7] = component2.sellsPerWeekOnline[0];
			array4[32 + num7] = component2.sellsPerWeekOnline[1];
			array4[33 + num7] = component2.sellsPerWeekOnline[2];
			array4[34 + num7] = component2.sellsPerWeekOnline[3];
			array4[35 + num7] = component2.sellsPerWeekOnline[4];
			array4[36 + num7] = component2.sellsPerWeekOnline[5];
			array4[37 + num7] = component2.sellsPerWeekOnline[6];
			array4[38 + num7] = component2.sellsPerWeekOnline[7];
			array4[39 + num7] = component2.sellsPerWeekOnline[8];
			array4[40 + num7] = component2.sellsPerWeekOnline[9];
			array4[41 + num7] = component2.sellsPerWeekOnline[10];
			array4[42 + num7] = component2.sellsPerWeekOnline[11];
			array4[43 + num7] = component2.sellsPerWeekOnline[12];
			array4[44 + num7] = component2.sellsPerWeekOnline[13];
			array4[45 + num7] = component2.sellsPerWeekOnline[14];
			array4[46 + num7] = component2.sellsPerWeekOnline[15];
			array4[47 + num7] = component2.sellsPerWeekOnline[16];
			array4[48 + num7] = component2.sellsPerWeekOnline[17];
			array4[49 + num7] = component2.sellsPerWeekOnline[18];
			array4[50 + num7] = component2.sellsPerWeekOnline[19];
			array4[51 + num7] = component2.realsticPower;
			int num8 = i * (array6.Length / num);
			array6[num8] = component2.platformStatic;
			array6[1 + num8] = component2.inDevelopment;
			array6[2 + num8] = component2.typ_standard;
			array6[3 + num8] = component2.typ_nachfolger;
			array6[4 + num8] = component2.typ_remaster;
			array6[5 + num8] = component2.typ_addon;
			array6[6 + num8] = component2.typ_bundle;
			array6[7 + num8] = component2.typ_budget;
			array6[8 + num8] = component2.engineFeature_DevDone[0];
			array6[9 + num8] = component2.engineFeature_DevDone[1];
			array6[10 + num8] = component2.engineFeature_DevDone[2];
			array6[11 + num8] = component2.engineFeature_DevDone[3];
			array6[12 + num8] = component2.exklusiv;
			array6[13 + num8] = component2.isOnMarket;
			array6[14 + num8] = component2.nachfolger_created;
			array6[15 + num8] = component2.remaster_created;
			array6[16 + num8] = component2.typ_contractGame;
			array6[17 + num8] = component2.trendsetter;
			array6[18 + num8] = component2.warBeiAwards;
			array6[19 + num8] = component2.retro;
			array6[20 + num8] = component2.spielbericht;
			array6[21 + num8] = component2.typ_addonStandalone;
			array6[22 + num8] = component2.digitalVersion;
			array6[23 + num8] = component2.retailVersion;
			array6[24 + num8] = component2.budget_created;
			array6[25 + num8] = component2.gameplayStudio[0];
			array6[26 + num8] = component2.gameplayStudio[1];
			array6[27 + num8] = component2.gameplayStudio[2];
			array6[28 + num8] = component2.gameplayStudio[3];
			array6[29 + num8] = component2.gameplayStudio[4];
			array6[30 + num8] = component2.gameplayStudio[5];
			array6[31 + num8] = component2.grafikStudio[0];
			array6[32 + num8] = component2.grafikStudio[1];
			array6[33 + num8] = component2.grafikStudio[2];
			array6[34 + num8] = component2.grafikStudio[3];
			array6[35 + num8] = component2.grafikStudio[4];
			array6[36 + num8] = component2.grafikStudio[5];
			array6[37 + num8] = component2.soundStudio[0];
			array6[38 + num8] = component2.soundStudio[1];
			array6[39 + num8] = component2.soundStudio[2];
			array6[40 + num8] = component2.soundStudio[3];
			array6[41 + num8] = component2.soundStudio[4];
			array6[42 + num8] = component2.soundStudio[5];
			array6[43 + num8] = component2.motionCaptureStudio[0];
			array6[44 + num8] = component2.motionCaptureStudio[1];
			array6[45 + num8] = component2.motionCaptureStudio[2];
			array6[46 + num8] = component2.motionCaptureStudio[3];
			array6[47 + num8] = component2.motionCaptureStudio[4];
			array6[48 + num8] = component2.motionCaptureStudio[5];
			array6[49 + num8] = component2.standard_edition[0];
			array6[50 + num8] = component2.standard_edition[1];
			array6[51 + num8] = component2.standard_edition[2];
			array6[52 + num8] = component2.standard_edition[3];
			array6[53 + num8] = component2.standard_edition[4];
			array6[54 + num8] = component2.standard_edition[5];
			array6[55 + num8] = component2.standard_edition[6];
			array6[56 + num8] = component2.standard_edition[7];
			array6[57 + num8] = component2.standard_edition[8];
			array6[58 + num8] = component2.standard_edition[9];
			array6[59 + num8] = component2.deluxe_edition[0];
			array6[60 + num8] = component2.deluxe_edition[1];
			array6[61 + num8] = component2.deluxe_edition[2];
			array6[62 + num8] = component2.deluxe_edition[3];
			array6[63 + num8] = component2.deluxe_edition[4];
			array6[64 + num8] = component2.deluxe_edition[5];
			array6[65 + num8] = component2.deluxe_edition[6];
			array6[66 + num8] = component2.deluxe_edition[7];
			array6[67 + num8] = component2.deluxe_edition[8];
			array6[68 + num8] = component2.deluxe_edition[9];
			array6[69 + num8] = component2.collectors_edition[0];
			array6[70 + num8] = component2.collectors_edition[1];
			array6[71 + num8] = component2.collectors_edition[2];
			array6[72 + num8] = component2.collectors_edition[3];
			array6[73 + num8] = component2.collectors_edition[4];
			array6[74 + num8] = component2.collectors_edition[5];
			array6[75 + num8] = component2.collectors_edition[6];
			array6[76 + num8] = component2.collectors_edition[7];
			array6[77 + num8] = component2.collectors_edition[8];
			array6[78 + num8] = component2.collectors_edition[9];
			array6[79 + num8] = component2.gameLanguage[0];
			array6[80 + num8] = component2.gameLanguage[1];
			array6[81 + num8] = component2.gameLanguage[2];
			array6[82 + num8] = component2.gameLanguage[3];
			array6[83 + num8] = component2.gameLanguage[4];
			array6[84 + num8] = component2.gameLanguage[5];
			array6[85 + num8] = component2.gameLanguage[6];
			array6[86 + num8] = component2.gameLanguage[7];
			array6[87 + num8] = component2.gameLanguage[8];
			array6[88 + num8] = component2.gameLanguage[9];
			array6[89 + num8] = component2.gameLanguage[10];
			array6[90 + num8] = component2.typ_mmoaddon;
			array6[91 + num8] = component2.spielbericht_favorit;
			array6[92 + num8] = component2.bundle_created;
			array6[93 + num8] = component2.typ_bundleAddon;
			array6[94 + num8] = component2.typ_goty;
			array6[95 + num8] = component2.goty;
			array6[96 + num8] = component2.goty_created;
			array6[97 + num8] = component2.inAppPurchase[0];
			array6[98 + num8] = component2.inAppPurchase[1];
			array6[99 + num8] = component2.inAppPurchase[2];
			array6[100 + num8] = component2.inAppPurchase[3];
			array6[101 + num8] = component2.inAppPurchase[4];
			array6[102 + num8] = component2.inAppPurchase[5];
			array6[103 + num8] = component2.mmoTOf2p_created;
			array6[104 + num8] = component2.archiv_spielkonzept;
			array6[105 + num8] = component2.archiv_spielbericht;
			array6[106 + num8] = component2.archiv_fanbriefe;
			array6[107 + num8] = component2.handy;
			array6[108 + num8] = component2.arcade;
			array6[109 + num8] = component2.typ_spinoff;
			array6[110 + num8] = component2.merchKeinVerkauf;
			array6[111 + num8] = component2.portExist[0];
			array6[112 + num8] = component2.portExist[1];
			array6[113 + num8] = component2.portExist[2];
			array6[114 + num8] = component2.herstellerExklusiv;
			array6[115 + num8] = component2.schublade;
			array6[116 + num8] = component2.autoPreis;
			array6[117 + num8] = component2.pubOffer;
			array6[118 + num8] = component2.noSpinOff;
			array6[120 + num8] = component2.commercialFlop;
			array6[121 + num8] = component2.commercialHit;
			array6[122 + num8] = component2.newGenreCombination;
			array6[123 + num8] = component2.newTopicCombination;
			array6[124 + num8] = component2.npcLateinNumbers;
			array6[125 + num8] = component2.pubAngebot;
			array6[126 + num8] = component2.pubAngebot_Retail;
			array6[127 + num8] = component2.pubAngebot_Digital;
			array6[128 + num8] = component2.pubAnbgebot_Inivs;
			array6[129 + num8] = component2.pubAngebot_AngebotWoche;
			array6[130 + num8] = component2.auftragsspiel;
			array6[131 + num8] = component2.auftragsspiel_zeitAbgelaufen;
			array6[132 + num8] = component2.auftragsspiel_Inivs;
			array6[133 + num8] = component2.sonderIP;
			array6[134 + num8] = component2.f2pConverted;
			array6[135 + num8] = component2.angekuendigt;
			array6[136 + num8] = component2.inGamePass;
			array6[137 + num8] = component2.archiv_ip;
			array6[138 + num8] = component2.ipToSell;
			array6[139 + num8] = component2.freeware;
			array6[140 + num8] = component2.gameTab_fullView;
			int num9 = i * (array2.Length / num);
			array2[num9] = component2.umsatzTotal;
			array2[1 + num9] = component2.costs_entwicklung;
			array2[2 + num9] = component2.costs_mitarbeiter;
			array2[3 + num9] = component2.costs_marketing;
			array2[4 + num9] = component2.costs_enginegebuehren;
			array2[5 + num9] = component2.costs_server;
			array2[6 + num9] = component2.costs_production;
			array2[7 + num9] = component2.umsatzAbos;
			array2[8 + num9] = component2.umsatzInApp;
			array2[9 + num9] = component2.exklusivKonsolenSells;
			array2[10 + num9] = component2.bundleID[0];
			array2[11 + num9] = component2.bundleID[1];
			array2[12 + num9] = component2.bundleID[2];
			array2[13 + num9] = component2.bundleID[3];
			array2[14 + num9] = component2.bundleID[4];
			array2[15 + num9] = component2.inAppPurchaseWeek;
			array2[16 + num9] = component2.originalGameID;
			array2[17 + num9] = component2.costs_updates;
			array2[18 + num9] = component2.specialMarketing[0];
			array2[19 + num9] = component2.specialMarketing[1];
			array2[20 + num9] = component2.specialMarketing[2];
			array2[21 + num9] = component2.specialMarketing[3];
			array2[22 + num9] = component2.specialMarketing[4];
			array2[23 + num9] = component2.Designschwerpunkt[0];
			array2[24 + num9] = component2.Designschwerpunkt[1];
			array2[25 + num9] = component2.Designschwerpunkt[2];
			array2[26 + num9] = component2.Designschwerpunkt[3];
			array2[27 + num9] = component2.Designschwerpunkt[4];
			array2[28 + num9] = component2.Designschwerpunkt[5];
			array2[29 + num9] = component2.Designschwerpunkt[6];
			array2[30 + num9] = component2.Designschwerpunkt[7];
			array2[31 + num9] = component2.Designausrichtung[0];
			array2[32 + num9] = component2.Designausrichtung[1];
			array2[33 + num9] = component2.Designausrichtung[2];
			array2[34 + num9] = component2.arcadeCase;
			array2[35 + num9] = component2.arcadeMonitor;
			array2[36 + num9] = component2.arcadeJoystick;
			array2[37 + num9] = component2.arcadeSound;
			array2[38 + num9] = component2.arcadeProdCosts;
			array2[39 + num9] = component2.portID;
			array2[40 + num9] = component2.mainIP;
			array2[41 + num9] = component2.ipTime;
			array2[42 + num9] = component2.bestChartPosition;
			array2[43 + num9] = component2.stornierungen;
			array2[44 + num9] = component2.schubladeTaskID;
			array2[45 + num9] = component2.merchGesamtGewinn;
			array2[46 + num9] = component2.weeksInDevelopment;
			array2[47 + num9] = component2.pubAngebot_Weeks;
			array2[48 + num9] = component2.pubAngebot_Garantiesumme;
			array2[49 + num9] = component2.sellsTotal;
			array2[50 + num9] = component2.sellsTotalStandard;
			array2[51 + num9] = component2.sellsTotalDeluxe;
			array2[52 + num9] = component2.sellsTotalCollectors;
			array2[53 + num9] = component2.sellsTotalOnline;
			array2[54 + num9] = component2.tw_gewinnanteil;
			array2[55 + num9] = component2.merchGewinnLetzterMonat;
			array2[56 + num9] = component2.merchGewinnDiesenMonat;
			array2[57 + num9] = component2.lagerbestand[0];
			array2[58 + num9] = component2.lagerbestand[1];
			array2[59 + num9] = component2.lagerbestand[2];
			array2[60 + num9] = component2.abonnements;
			array2[61 + num9] = component2.abonnementsWoche;
			array2[62 + num9] = component2.bestAbonnements;
			array2[63 + num9] = component2.subvention;
			int num10 = i * num2;
			for (int j = 0; j < component2.gameGameplayFeatures.Length; j++)
			{
				array7[j + num10] = component2.gameGameplayFeatures[j];
			}
			num10 = i * num3;
			for (int k = 0; k < component2.gameplayFeatures_DevDone.Length; k++)
			{
				array8[k + num10] = component2.gameplayFeatures_DevDone[k];
			}
			num10 = i * num4;
			for (int l = 0; l < component2.fanbrief.Length; l++)
			{
				array9[l + num10] = component2.fanbrief[l];
			}
		}
		writer.Write<int[]>("games_I", array3);
		writer.Write<float[]>("games_F", array4);
		writer.Write<long[]>("games_L", array2);
		writer.Write<string[]>("games_S", array5);
		writer.Write<bool[]>("games_B", array6);
		writer.Write<bool[]>("games_gameplayFeatures", array7);
		writer.Write<bool[]>("games_gameplayFeaturesDevDone", array8);
		writer.Write<bool[]>("games_fanbrief", array9);
	}

	private void LoadGames(ES3Reader reader, string filename)
	{
		int num = reader.Read("anzGames", -1);
		Debug.Log("Load: (anzGames) " + num);
		if (num <= 0)
		{
			return;
		}
		int[] array = reader.Read<int[]>("games_I");
		float[] array2 = reader.Read<float[]>("games_F");
		long[] array3 = reader.Read<long[]>("games_L");
		string[] array4 = reader.Read<string[]>("games_S");
		bool[] array5 = reader.Read<bool[]>("games_B");
		bool[] array6 = reader.Read<bool[]>("games_gameplayFeatures");
		bool[] array7 = reader.Read<bool[]>("games_gameplayFeaturesDevDone");
		bool[] array8 = reader.Read<bool[]>("games_fanbrief");
		int num2 = array.Length / num;
		int num3 = array2.Length / num;
		int num4 = array5.Length / num;
		int num5 = array4.Length / num;
		int num6 = array3.Length / num;
		for (int i = 0; i < num; i++)
		{
			int num7 = i * (array.Length / num);
			int num8 = i * (array2.Length / num);
			int num9 = i * (array5.Length / num);
			int num10 = i * (array4.Length / num);
			int num11 = i * (array3.Length / num);
			if (array4[num10].Length > 0)
			{
				gameScript gameScript2 = games_.CreateNewGame(fromSavegame: true, setDate: false);
				array4[num10] = array4[num10].Replace(" <color=green>" + tS_.GetText(1549) + "</color>", string.Empty);
				array4[num10] = array4[num10].Replace(" <color=green>" + tS_.GetText(1896) + "</color>", string.Empty);
				gameScript2.SetMyName(array4[num10]);
				gameScript2.beschreibung = array4[1 + num10];
				gameScript2.ipName = array4[2 + num10];
				if (num5 > 3)
				{
					gameScript2.myNameTeil1 = array4[3 + num10];
				}
				gameScript2.myID = array[num7];
				gameScript2.developerID = array[1 + num7];
				gameScript2.engineID = array[2 + num7];
				gameScript2.reviewGameplay = array[3 + num7];
				gameScript2.reviewGrafik = array[4 + num7];
				gameScript2.reviewSound = array[5 + num7];
				gameScript2.reviewSteuerung = array[6 + num7];
				gameScript2.reviewTotal = array[7 + num7];
				gameScript2.reviewGameplayText = array[8 + num7];
				gameScript2.reviewGrafikText = array[9 + num7];
				gameScript2.reviewSoundText = array[10 + num7];
				gameScript2.reviewSteuerungText = array[11 + num7];
				gameScript2.reviewTotalText = array[12 + num7];
				gameScript2.date_year = array[13 + num7];
				gameScript2.date_month = array[14 + num7];
				gameScript2.aboPreisOld = array[15 + num7];
				gameScript2.devAktFeature = array[16 + num7];
				gameScript2.gameTyp = array[17 + num7];
				gameScript2.gameSize = array[18 + num7];
				gameScript2.gameZielgruppe = array[19 + num7];
				gameScript2.maingenre = array[20 + num7];
				gameScript2.subgenre = array[21 + num7];
				gameScript2.gameMainTheme = array[22 + num7];
				gameScript2.gameSubTheme = array[23 + num7];
				gameScript2.gameLicence = array[24 + num7];
				gameScript2.gameCopyProtect = array[25 + num7];
				gameScript2.auftragsspiel_gehalt = array[26 + num7];
				gameScript2.auftragsspiel_bonus = array[27 + num7];
				gameScript2.auftragsspiel_zeitInWochen = array[28 + num7];
				gameScript2.auftragsspiel_wochenAlsAngebot = array[29 + num7];
				gameScript2.auftragsspiel_mindestbewertung = array[30 + num7];
				gameScript2.gameAP_Gameplay = array[31 + num7];
				gameScript2.gameAP_Grafik = array[32 + num7];
				gameScript2.gameAP_Sound = array[33 + num7];
				gameScript2.gameAP_Technik = array[34 + num7];
				gameScript2.gameEngineFeature[0] = array[35 + num7];
				gameScript2.gameEngineFeature[1] = array[36 + num7];
				gameScript2.gameEngineFeature[2] = array[37 + num7];
				gameScript2.gameEngineFeature[3] = array[38 + num7];
				gameScript2.gamePlatform[0] = array[39 + num7];
				gameScript2.gamePlatform[1] = array[40 + num7];
				gameScript2.gamePlatform[2] = array[41 + num7];
				gameScript2.gamePlatform[3] = array[42 + num7];
				gameScript2.publisherID = array[43 + num7];
				gameScript2.weeksOnMarket = array[44 + num7];
				gameScript2.engineGewinnbeteiligung = array[45 + num7];
				gameScript2.teile = array[46 + num7];
				gameScript2.amountUpdates = array[48 + num7];
				gameScript2.amountAddons = array[49 + num7];
				gameScript2.amountMMOAddons = array[50 + num7];
				gameScript2.usk = array[51 + num7];
				gameScript2.devFortsetzen = array[52 + num7];
				gameScript2.verkaufspreis[0] = array[53 + num7];
				gameScript2.verkaufspreis[1] = array[54 + num7];
				gameScript2.verkaufspreis[2] = array[55 + num7];
				gameScript2.verkaufspreis[3] = array[56 + num7];
				gameScript2.releaseDate = array[57 + num7];
				gameScript2.vorbestellungen = array[58 + num7];
				gameScript2.lagerbestand[0] = array[59 + num7];
				gameScript2.lagerbestand[1] = array[60 + num7];
				gameScript2.lagerbestand[2] = array[61 + num7];
				gameScript2.freigabeBudget = array[66 + num7];
				gameScript2.sellsPerWeek[0] = array[67 + num7];
				gameScript2.sellsPerWeek[1] = array[68 + num7];
				gameScript2.sellsPerWeek[2] = array[69 + num7];
				gameScript2.sellsPerWeek[3] = array[70 + num7];
				gameScript2.sellsPerWeek[4] = array[71 + num7];
				gameScript2.sellsPerWeek[5] = array[72 + num7];
				gameScript2.sellsPerWeek[6] = array[73 + num7];
				gameScript2.sellsPerWeek[7] = array[74 + num7];
				gameScript2.sellsPerWeek[8] = array[75 + num7];
				gameScript2.sellsPerWeek[9] = array[76 + num7];
				gameScript2.sellsPerWeek[10] = array[77 + num7];
				gameScript2.sellsPerWeek[11] = array[78 + num7];
				gameScript2.sellsPerWeek[12] = array[79 + num7];
				gameScript2.sellsPerWeek[13] = array[80 + num7];
				gameScript2.sellsPerWeek[14] = array[81 + num7];
				gameScript2.sellsPerWeek[15] = array[82 + num7];
				gameScript2.sellsPerWeek[16] = array[83 + num7];
				gameScript2.sellsPerWeek[17] = array[84 + num7];
				gameScript2.sellsPerWeek[18] = array[85 + num7];
				gameScript2.sellsPerWeek[19] = array[86 + num7];
				gameScript2.finanzierung_Grundkosten = array[87 + num7];
				gameScript2.finanzierung_Technology = array[88 + num7];
				gameScript2.finanzierung_Kontent = array[89 + num7];
				gameScript2.gameAntiCheat = array[90 + num7];
				gameScript2.abonnements = array[91 + num7];
				gameScript2.abonnementsWoche = array[92 + num7];
				gameScript2.aboPreis = array[93 + num7];
				gameScript2.abosAddons = array[94 + num7];
				gameScript2.lastChartPosition = array[95 + num7];
				gameScript2.date_start_year = array[96 + num7];
				gameScript2.date_start_month = array[97 + num7];
				gameScript2.userPositiv = array[98 + num7];
				gameScript2.userNegativ = array[99 + num7];
				if (num2 > 100)
				{
					gameScript2.merchBestellungen[0] = array[100 + num7];
				}
				if (num2 > 101)
				{
					gameScript2.merchBestellungen[1] = array[101 + num7];
				}
				if (num2 > 102)
				{
					gameScript2.merchBestellungen[2] = array[102 + num7];
				}
				if (num2 > 103)
				{
					gameScript2.merchBestellungen[3] = array[103 + num7];
				}
				if (num2 > 104)
				{
					gameScript2.merchBestellungen[4] = array[104 + num7];
				}
				if (num2 > 105)
				{
					gameScript2.merchBestellungen[5] = array[105 + num7];
				}
				if (num2 > 106)
				{
					gameScript2.merchBestellungen[6] = array[106 + num7];
				}
				if (num2 > 107)
				{
					gameScript2.merchBestellungen[7] = array[107 + num7];
				}
				if (num2 > 108)
				{
					gameScript2.merchGesamtSells[0] = array[108 + num7];
				}
				if (num2 > 109)
				{
					gameScript2.merchGesamtSells[1] = array[109 + num7];
				}
				if (num2 > 110)
				{
					gameScript2.merchGesamtSells[2] = array[110 + num7];
				}
				if (num2 > 111)
				{
					gameScript2.merchGesamtSells[3] = array[111 + num7];
				}
				if (num2 > 112)
				{
					gameScript2.merchGesamtSells[4] = array[112 + num7];
				}
				if (num2 > 113)
				{
					gameScript2.merchGesamtSells[5] = array[113 + num7];
				}
				if (num2 > 114)
				{
					gameScript2.merchGesamtSells[6] = array[114 + num7];
				}
				if (num2 > 115)
				{
					gameScript2.merchGesamtSells[7] = array[115 + num7];
				}
				if (num2 > 116)
				{
					gameScript2.merchDiesenMonat[0] = array[116 + num7];
				}
				if (num2 > 117)
				{
					gameScript2.merchDiesenMonat[1] = array[117 + num7];
				}
				if (num2 > 118)
				{
					gameScript2.merchDiesenMonat[2] = array[118 + num7];
				}
				if (num2 > 119)
				{
					gameScript2.merchDiesenMonat[3] = array[119 + num7];
				}
				if (num2 > 120)
				{
					gameScript2.merchDiesenMonat[4] = array[120 + num7];
				}
				if (num2 > 121)
				{
					gameScript2.merchDiesenMonat[5] = array[121 + num7];
				}
				if (num2 > 122)
				{
					gameScript2.merchDiesenMonat[6] = array[122 + num7];
				}
				if (num2 > 123)
				{
					gameScript2.merchDiesenMonat[7] = array[123 + num7];
				}
				if (num2 > 124)
				{
					gameScript2.merchLetzterMonat[0] = array[124 + num7];
				}
				if (num2 > 125)
				{
					gameScript2.merchLetzterMonat[1] = array[125 + num7];
				}
				if (num2 > 126)
				{
					gameScript2.merchLetzterMonat[2] = array[126 + num7];
				}
				if (num2 > 127)
				{
					gameScript2.merchLetzterMonat[3] = array[127 + num7];
				}
				if (num2 > 128)
				{
					gameScript2.merchLetzterMonat[4] = array[128 + num7];
				}
				if (num2 > 129)
				{
					gameScript2.merchLetzterMonat[5] = array[129 + num7];
				}
				if (num2 > 130)
				{
					gameScript2.merchLetzterMonat[6] = array[130 + num7];
				}
				if (num2 > 131)
				{
					gameScript2.merchLetzterMonat[7] = array[131 + num7];
				}
				if (num2 > 132)
				{
					gameScript2.sonderIPMindestreview = array[132 + num7];
				}
				if (num2 > 133)
				{
					gameScript2.bestAbonnements = array[133 + num7];
				}
				if (num2 > 134)
				{
					gameScript2.ownerID = array[134 + num7];
				}
				if (num2 > 135)
				{
					gameScript2.gamePassPlayer = array[135 + num7];
				}
				gameScript2.hype = array2[num8];
				gameScript2.devPoints = array2[1 + num8];
				gameScript2.devPointsStart = array2[2 + num8];
				gameScript2.devPoints_Gesamt = array2[3 + num8];
				gameScript2.devPointsStart_Gesamt = array2[4 + num8];
				gameScript2.points_gameplay = array2[5 + num8];
				gameScript2.points_grafik = array2[6 + num8];
				gameScript2.points_sound = array2[7 + num8];
				gameScript2.points_technik = array2[8 + num8];
				gameScript2.points_bugs = array2[9 + num8];
				gameScript2.bonusSellsUpdates = array2[10 + num8];
				gameScript2.bonusSellsAddons = array2[11 + num8];
				gameScript2.addonQuality = array2[13 + num8];
				gameScript2.f2pInteresse = array2[14 + num8];
				gameScript2.mmoInteresse = array2[15 + num8];
				gameScript2.ipPunkte = array2[16 + num8];
				gameScript2.merchGesamtReviewPoints = array2[17 + num8];
				gameScript2.points_bugsInvis = array2[18 + num8];
				gameScript2.pubAngebot_Verhandlung = array2[19 + num8];
				gameScript2.pubAngebot_VerhandlungProzent = array2[20 + num8];
				gameScript2.pubAngebot_Stimmung = array2[21 + num8];
				gameScript2.pubAngebot_Gewinnbeteiligung = array2[22 + num8];
				if (num3 > 23)
				{
					gameScript2.merchVerkaufspreis[0] = array2[23 + num8];
				}
				if (num3 > 24)
				{
					gameScript2.merchVerkaufspreis[1] = array2[24 + num8];
				}
				if (num3 > 25)
				{
					gameScript2.merchVerkaufspreis[2] = array2[25 + num8];
				}
				if (num3 > 26)
				{
					gameScript2.merchVerkaufspreis[3] = array2[26 + num8];
				}
				if (num3 > 27)
				{
					gameScript2.merchVerkaufspreis[4] = array2[27 + num8];
				}
				if (num3 > 28)
				{
					gameScript2.merchVerkaufspreis[5] = array2[28 + num8];
				}
				if (num3 > 29)
				{
					gameScript2.merchVerkaufspreis[6] = array2[29 + num8];
				}
				if (num3 > 30)
				{
					gameScript2.merchVerkaufspreis[7] = array2[30 + num8];
				}
				if (num3 > 31)
				{
					gameScript2.sellsPerWeekOnline[0] = array2[31 + num8];
				}
				if (num3 > 32)
				{
					gameScript2.sellsPerWeekOnline[1] = array2[32 + num8];
				}
				if (num3 > 33)
				{
					gameScript2.sellsPerWeekOnline[2] = array2[33 + num8];
				}
				if (num3 > 34)
				{
					gameScript2.sellsPerWeekOnline[3] = array2[34 + num8];
				}
				if (num3 > 35)
				{
					gameScript2.sellsPerWeekOnline[4] = array2[35 + num8];
				}
				if (num3 > 36)
				{
					gameScript2.sellsPerWeekOnline[5] = array2[36 + num8];
				}
				if (num3 > 37)
				{
					gameScript2.sellsPerWeekOnline[6] = array2[37 + num8];
				}
				if (num3 > 38)
				{
					gameScript2.sellsPerWeekOnline[7] = array2[38 + num8];
				}
				if (num3 > 39)
				{
					gameScript2.sellsPerWeekOnline[8] = array2[39 + num8];
				}
				if (num3 > 40)
				{
					gameScript2.sellsPerWeekOnline[9] = array2[40 + num8];
				}
				if (num3 > 41)
				{
					gameScript2.sellsPerWeekOnline[10] = array2[41 + num8];
				}
				if (num3 > 42)
				{
					gameScript2.sellsPerWeekOnline[11] = array2[42 + num8];
				}
				if (num3 > 43)
				{
					gameScript2.sellsPerWeekOnline[12] = array2[43 + num8];
				}
				if (num3 > 44)
				{
					gameScript2.sellsPerWeekOnline[13] = array2[44 + num8];
				}
				if (num3 > 45)
				{
					gameScript2.sellsPerWeekOnline[14] = array2[45 + num8];
				}
				if (num3 > 46)
				{
					gameScript2.sellsPerWeekOnline[15] = array2[46 + num8];
				}
				if (num3 > 47)
				{
					gameScript2.sellsPerWeekOnline[16] = array2[47 + num8];
				}
				if (num3 > 48)
				{
					gameScript2.sellsPerWeekOnline[17] = array2[48 + num8];
				}
				if (num3 > 49)
				{
					gameScript2.sellsPerWeekOnline[18] = array2[49 + num8];
				}
				if (num3 > 50)
				{
					gameScript2.sellsPerWeekOnline[19] = array2[50 + num8];
				}
				if (num3 > 51)
				{
					gameScript2.realsticPower = array2[51 + num8];
				}
				gameScript2.platformStatic = array5[num9];
				gameScript2.inDevelopment = array5[1 + num9];
				gameScript2.typ_standard = array5[2 + num9];
				gameScript2.typ_nachfolger = array5[3 + num9];
				gameScript2.typ_remaster = array5[4 + num9];
				gameScript2.typ_addon = array5[5 + num9];
				gameScript2.typ_bundle = array5[6 + num9];
				gameScript2.typ_budget = array5[7 + num9];
				gameScript2.engineFeature_DevDone[0] = array5[8 + num9];
				gameScript2.engineFeature_DevDone[1] = array5[9 + num9];
				gameScript2.engineFeature_DevDone[2] = array5[10 + num9];
				gameScript2.engineFeature_DevDone[3] = array5[11 + num9];
				gameScript2.exklusiv = array5[12 + num9];
				gameScript2.isOnMarket = array5[13 + num9];
				gameScript2.nachfolger_created = array5[14 + num9];
				gameScript2.remaster_created = array5[15 + num9];
				gameScript2.typ_contractGame = array5[16 + num9];
				gameScript2.trendsetter = array5[17 + num9];
				gameScript2.warBeiAwards = array5[18 + num9];
				gameScript2.retro = array5[19 + num9];
				gameScript2.spielbericht = array5[20 + num9];
				gameScript2.typ_addonStandalone = array5[21 + num9];
				gameScript2.digitalVersion = array5[22 + num9];
				gameScript2.retailVersion = array5[23 + num9];
				gameScript2.budget_created = array5[24 + num9];
				gameScript2.gameplayStudio[0] = array5[25 + num9];
				gameScript2.gameplayStudio[1] = array5[26 + num9];
				gameScript2.gameplayStudio[2] = array5[27 + num9];
				gameScript2.gameplayStudio[3] = array5[28 + num9];
				gameScript2.gameplayStudio[4] = array5[29 + num9];
				gameScript2.gameplayStudio[5] = array5[30 + num9];
				gameScript2.grafikStudio[0] = array5[31 + num9];
				gameScript2.grafikStudio[1] = array5[32 + num9];
				gameScript2.grafikStudio[2] = array5[33 + num9];
				gameScript2.grafikStudio[3] = array5[34 + num9];
				gameScript2.grafikStudio[4] = array5[35 + num9];
				gameScript2.grafikStudio[5] = array5[36 + num9];
				gameScript2.soundStudio[0] = array5[37 + num9];
				gameScript2.soundStudio[1] = array5[38 + num9];
				gameScript2.soundStudio[2] = array5[39 + num9];
				gameScript2.soundStudio[3] = array5[40 + num9];
				gameScript2.soundStudio[4] = array5[41 + num9];
				gameScript2.soundStudio[5] = array5[42 + num9];
				gameScript2.motionCaptureStudio[0] = array5[43 + num9];
				gameScript2.motionCaptureStudio[1] = array5[44 + num9];
				gameScript2.motionCaptureStudio[2] = array5[45 + num9];
				gameScript2.motionCaptureStudio[3] = array5[46 + num9];
				gameScript2.motionCaptureStudio[4] = array5[47 + num9];
				gameScript2.motionCaptureStudio[5] = array5[48 + num9];
				gameScript2.standard_edition[0] = array5[49 + num9];
				gameScript2.standard_edition[1] = array5[50 + num9];
				gameScript2.standard_edition[2] = array5[51 + num9];
				gameScript2.standard_edition[3] = array5[52 + num9];
				gameScript2.standard_edition[4] = array5[53 + num9];
				gameScript2.standard_edition[5] = array5[54 + num9];
				gameScript2.standard_edition[6] = array5[55 + num9];
				gameScript2.standard_edition[7] = array5[56 + num9];
				gameScript2.standard_edition[8] = array5[57 + num9];
				gameScript2.standard_edition[9] = array5[58 + num9];
				gameScript2.deluxe_edition[0] = array5[59 + num9];
				gameScript2.deluxe_edition[1] = array5[60 + num9];
				gameScript2.deluxe_edition[2] = array5[61 + num9];
				gameScript2.deluxe_edition[3] = array5[62 + num9];
				gameScript2.deluxe_edition[4] = array5[63 + num9];
				gameScript2.deluxe_edition[5] = array5[64 + num9];
				gameScript2.deluxe_edition[6] = array5[65 + num9];
				gameScript2.deluxe_edition[7] = array5[66 + num9];
				gameScript2.deluxe_edition[8] = array5[67 + num9];
				gameScript2.deluxe_edition[9] = array5[68 + num9];
				gameScript2.collectors_edition[0] = array5[69 + num9];
				gameScript2.collectors_edition[1] = array5[70 + num9];
				gameScript2.collectors_edition[2] = array5[71 + num9];
				gameScript2.collectors_edition[3] = array5[72 + num9];
				gameScript2.collectors_edition[4] = array5[73 + num9];
				gameScript2.collectors_edition[5] = array5[74 + num9];
				gameScript2.collectors_edition[6] = array5[75 + num9];
				gameScript2.collectors_edition[7] = array5[76 + num9];
				gameScript2.collectors_edition[8] = array5[77 + num9];
				gameScript2.collectors_edition[9] = array5[78 + num9];
				gameScript2.gameLanguage[0] = array5[79 + num9];
				gameScript2.gameLanguage[1] = array5[80 + num9];
				gameScript2.gameLanguage[2] = array5[81 + num9];
				gameScript2.gameLanguage[3] = array5[82 + num9];
				gameScript2.gameLanguage[4] = array5[83 + num9];
				gameScript2.gameLanguage[5] = array5[84 + num9];
				gameScript2.gameLanguage[6] = array5[85 + num9];
				gameScript2.gameLanguage[7] = array5[86 + num9];
				gameScript2.gameLanguage[8] = array5[87 + num9];
				gameScript2.gameLanguage[9] = array5[88 + num9];
				gameScript2.gameLanguage[10] = array5[89 + num9];
				gameScript2.typ_mmoaddon = array5[90 + num9];
				gameScript2.spielbericht_favorit = array5[91 + num9];
				gameScript2.bundle_created = array5[92 + num9];
				gameScript2.typ_bundleAddon = array5[93 + num9];
				gameScript2.typ_goty = array5[94 + num9];
				gameScript2.goty = array5[95 + num9];
				gameScript2.goty_created = array5[96 + num9];
				gameScript2.inAppPurchase[0] = array5[97 + num9];
				gameScript2.inAppPurchase[1] = array5[98 + num9];
				gameScript2.inAppPurchase[2] = array5[99 + num9];
				gameScript2.inAppPurchase[3] = array5[100 + num9];
				gameScript2.inAppPurchase[4] = array5[101 + num9];
				gameScript2.inAppPurchase[5] = array5[102 + num9];
				gameScript2.mmoTOf2p_created = array5[103 + num9];
				gameScript2.archiv_spielkonzept = array5[104 + num9];
				gameScript2.archiv_spielbericht = array5[105 + num9];
				gameScript2.archiv_fanbriefe = array5[106 + num9];
				gameScript2.handy = array5[107 + num9];
				gameScript2.arcade = array5[108 + num9];
				gameScript2.typ_spinoff = array5[109 + num9];
				gameScript2.merchKeinVerkauf = array5[110 + num9];
				gameScript2.portExist[0] = array5[111 + num9];
				gameScript2.portExist[1] = array5[112 + num9];
				gameScript2.portExist[2] = array5[113 + num9];
				gameScript2.herstellerExklusiv = array5[114 + num9];
				gameScript2.schublade = array5[115 + num9];
				gameScript2.autoPreis = array5[116 + num9];
				gameScript2.pubOffer = array5[117 + num9];
				gameScript2.noSpinOff = array5[118 + num9];
				gameScript2.commercialFlop = array5[120 + num9];
				gameScript2.commercialHit = array5[121 + num9];
				gameScript2.newGenreCombination = array5[122 + num9];
				gameScript2.newTopicCombination = array5[123 + num9];
				gameScript2.npcLateinNumbers = array5[124 + num9];
				gameScript2.pubAngebot = array5[125 + num9];
				gameScript2.pubAngebot_Retail = array5[126 + num9];
				gameScript2.pubAngebot_Digital = array5[127 + num9];
				gameScript2.pubAnbgebot_Inivs = array5[128 + num9];
				gameScript2.pubAngebot_AngebotWoche = array5[129 + num9];
				gameScript2.auftragsspiel = array5[130 + num9];
				gameScript2.auftragsspiel_zeitAbgelaufen = array5[131 + num9];
				gameScript2.auftragsspiel_Inivs = array5[132 + num9];
				if (num4 > 133)
				{
					gameScript2.sonderIP = array5[133 + num9];
				}
				if (num4 > 134)
				{
					gameScript2.f2pConverted = array5[134 + num9];
				}
				if (num4 > 135)
				{
					gameScript2.angekuendigt = array5[135 + num9];
				}
				if (num4 > 136)
				{
					gameScript2.inGamePass = array5[136 + num9];
				}
				if (num4 > 137)
				{
					gameScript2.archiv_ip = array5[137 + num9];
				}
				if (num4 > 138)
				{
					gameScript2.ipToSell = array5[138 + num9];
				}
				if (num4 > 139)
				{
					gameScript2.freeware = array5[139 + num9];
				}
				if (num4 > 140)
				{
					gameScript2.gameTab_fullView = array5[140 + num9];
				}
				gameScript2.umsatzTotal = array3[num11];
				gameScript2.costs_entwicklung = array3[1 + num11];
				gameScript2.costs_mitarbeiter = array3[2 + num11];
				gameScript2.costs_marketing = array3[3 + num11];
				gameScript2.costs_enginegebuehren = array3[4 + num11];
				gameScript2.costs_server = array3[5 + num11];
				gameScript2.costs_production = array3[6 + num11];
				gameScript2.umsatzAbos = array3[7 + num11];
				gameScript2.umsatzInApp = array3[8 + num11];
				gameScript2.exklusivKonsolenSells = array3[9 + num11];
				gameScript2.bundleID[0] = (int)array3[10 + num11];
				gameScript2.bundleID[1] = (int)array3[11 + num11];
				gameScript2.bundleID[2] = (int)array3[12 + num11];
				gameScript2.bundleID[3] = (int)array3[13 + num11];
				gameScript2.bundleID[4] = (int)array3[14 + num11];
				gameScript2.inAppPurchaseWeek = (int)array3[15 + num11];
				gameScript2.originalGameID = (int)array3[16 + num11];
				gameScript2.costs_updates = array3[17 + num11];
				gameScript2.specialMarketing[0] = (int)array3[18 + num11];
				gameScript2.specialMarketing[1] = (int)array3[19 + num11];
				gameScript2.specialMarketing[2] = (int)array3[20 + num11];
				gameScript2.specialMarketing[3] = (int)array3[21 + num11];
				gameScript2.specialMarketing[4] = (int)array3[22 + num11];
				gameScript2.Designschwerpunkt[0] = (int)array3[23 + num11];
				gameScript2.Designschwerpunkt[1] = (int)array3[24 + num11];
				gameScript2.Designschwerpunkt[2] = (int)array3[25 + num11];
				gameScript2.Designschwerpunkt[3] = (int)array3[26 + num11];
				gameScript2.Designschwerpunkt[4] = (int)array3[27 + num11];
				gameScript2.Designschwerpunkt[5] = (int)array3[28 + num11];
				gameScript2.Designschwerpunkt[6] = (int)array3[29 + num11];
				gameScript2.Designschwerpunkt[7] = (int)array3[30 + num11];
				gameScript2.Designausrichtung[0] = (int)array3[31 + num11];
				gameScript2.Designausrichtung[1] = (int)array3[32 + num11];
				gameScript2.Designausrichtung[2] = (int)array3[33 + num11];
				gameScript2.arcadeCase = (int)array3[34 + num11];
				gameScript2.arcadeMonitor = (int)array3[35 + num11];
				gameScript2.arcadeJoystick = (int)array3[36 + num11];
				gameScript2.arcadeSound = (int)array3[37 + num11];
				gameScript2.arcadeProdCosts = (int)array3[38 + num11];
				gameScript2.portID = (int)array3[39 + num11];
				if (gameScript2.portID == 0)
				{
					gameScript2.portID = -1;
				}
				gameScript2.mainIP = (int)array3[40 + num11];
				gameScript2.ipTime = (int)array3[41 + num11];
				gameScript2.bestChartPosition = (int)array3[42 + num11];
				gameScript2.stornierungen = (int)array3[43 + num11];
				gameScript2.schubladeTaskID = (int)array3[44 + num11];
				gameScript2.merchGesamtGewinn = (int)array3[45 + num11];
				gameScript2.weeksInDevelopment = (int)array3[46 + num11];
				gameScript2.pubAngebot_Weeks = (int)array3[47 + num11];
				gameScript2.pubAngebot_Garantiesumme = (int)array3[48 + num11];
				if (num6 > 50)
				{
					gameScript2.sellsTotal = array3[49 + num11];
				}
				if (num6 > 50)
				{
					gameScript2.sellsTotalStandard = array3[50 + num11];
				}
				if (num6 > 50)
				{
					gameScript2.sellsTotalDeluxe = array3[51 + num11];
				}
				if (num6 > 50)
				{
					gameScript2.sellsTotalCollectors = array3[52 + num11];
				}
				if (num6 > 50)
				{
					gameScript2.sellsTotalOnline = array3[53 + num11];
				}
				if (num6 > 54)
				{
					gameScript2.tw_gewinnanteil = array3[54 + num11];
				}
				if (num6 > 55)
				{
					gameScript2.merchGewinnLetzterMonat = array3[55 + num11];
				}
				if (num6 > 56)
				{
					gameScript2.merchGewinnDiesenMonat = array3[56 + num11];
				}
				if (num6 > 57)
				{
					gameScript2.lagerbestand[0] = array3[57 + num11];
				}
				if (num6 > 58)
				{
					gameScript2.lagerbestand[1] = array3[58 + num11];
				}
				if (num6 > 59)
				{
					gameScript2.lagerbestand[2] = array3[59 + num11];
				}
				if (num6 > 60)
				{
					gameScript2.abonnements = array3[60 + num11];
				}
				if (num6 > 61)
				{
					gameScript2.abonnementsWoche = array3[61 + num11];
				}
				if (num6 > 62)
				{
					gameScript2.bestAbonnements = array3[62 + num11];
				}
				if (num6 > 63)
				{
					gameScript2.subvention = array3[63 + num11];
				}
				int num12 = i * (array6.Length / num);
				for (int j = 0; j < gameScript2.gameGameplayFeatures.Length; j++)
				{
					gameScript2.gameGameplayFeatures[j] = array6[j + num12];
				}
				num12 = i * (array7.Length / num);
				for (int k = 0; k < gameScript2.gameplayFeatures_DevDone.Length; k++)
				{
					gameScript2.gameplayFeatures_DevDone[k] = array7[k + num12];
				}
				num12 = i * (array8.Length / num);
				for (int l = 0; l < gameScript2.fanbrief.Length; l++)
				{
					gameScript2.fanbrief[l] = array8[l + num12];
				}
				gameScript2.SetGameObjectName();
				gameScript2.InitUI();
			}
		}
		games_.FindGames();
	}

	private void SetRoomScripts(roomScript script_)
	{
		if (!script_)
		{
			return;
		}
		for (int i = 0; i < mapScript.mapSizeX; i++)
		{
			for (int j = 0; j < mapScript.mapSizeY; j++)
			{
				if (mapS_.mapRoomID[i, j] == script_.myID)
				{
					mapS_.mapRoomScript[i, j] = script_;
				}
			}
		}
	}

	private void KeysAbfragen(string filename)
	{
		key_merchStandardpreis = false;
		key_NpcIPs = false;
		key_inventarFilter = false;
		key_fanshopverlauf = false;
		key_achivements = false;
		key_npcGameNameInUse = false;
		key_default_verkaufpreis = false;
		key_default_verkaufpreisBundle = false;
		key_legends = false;
		key_legends_new = false;
		key_licence_YEAR = false;
		key_gamePass_aboVerlaufWoche = false;
		key_gamePass_aboVerlaufMonat = false;
		key_gamePass_umsatzVerlaufMonat = false;
		key_themes_USES = false;
		key_gameplayFeatures_NEED_GAMEPLAY_FEATURE = false;
		key_genres_SUC_YEAR = false;
		key_genres_PLATFORM_SELLS = false;
		key_EN = false;
		key_GE = false;
		key_TU = false;
		key_CH = false;
		key_FR = false;
		key_PB = false;
		key_HU = false;
		key_CT = false;
		key_PL = false;
		key_CZ = false;
		key_KO = false;
		key_AR = false;
		key_RU = false;
		key_IT = false;
		key_JA = false;
		key_UA = false;
		key_TH = false;
		if (ES3.KeyExists("merchStandardpreis", filename))
		{
			key_merchStandardpreis = true;
		}
		if (ES3.KeyExists("npcIPs", filename))
		{
			key_NpcIPs = true;
		}
		if (ES3.KeyExists("inventarFilter", filename))
		{
			key_inventarFilter = true;
		}
		if (ES3.KeyExists("fanshopverlauf", filename))
		{
			key_fanshopverlauf = true;
		}
		if (ES3.KeyExists("gameTabFilter", filename))
		{
			key_gameTabFilter = true;
		}
		if (ES3.KeyExists("devLegendsDesigner", filename))
		{
			key_legends = true;
		}
		if (ES3.KeyExists("devLegendsGametester", filename))
		{
			key_legends_new = true;
		}
		if (ES3.KeyExists("verkaufspreis_default_addon", filename))
		{
			key_default_verkaufpreis = true;
		}
		if (ES3.KeyExists("verkaufspreis_default_bundle", filename))
		{
			key_default_verkaufpreisBundle = true;
		}
		if (ES3.KeyExists("npcGameNameInUse", filename))
		{
			key_npcGameNameInUse = true;
		}
		if (ES3.KeyExists("achivements", filename))
		{
			key_achivements = true;
		}
		if (ES3.KeyExists("licence_YEAR", filename))
		{
			key_licence_YEAR = true;
		}
		if (ES3.KeyExists("gamePass_aboVerlaufWoche", filename))
		{
			key_gamePass_aboVerlaufWoche = true;
		}
		if (ES3.KeyExists("gamePass_aboVerlaufMonat", filename))
		{
			key_gamePass_aboVerlaufMonat = true;
		}
		if (ES3.KeyExists("gamePass_umsatzVerlaufMonat", filename))
		{
			key_gamePass_umsatzVerlaufMonat = true;
		}
		if (ES3.KeyExists("themes_USES", filename))
		{
			key_themes_USES = true;
		}
		if (ES3.KeyExists("gameplayFeatures_NEED_GAMEPLAY_FEATURE", filename))
		{
			key_gameplayFeatures_NEED_GAMEPLAY_FEATURE = true;
		}
		if (ES3.KeyExists("genres_SUC_YEAR", filename))
		{
			key_genres_SUC_YEAR = true;
		}
		if (ES3.KeyExists("genres_PLATFORM_SELLS", filename))
		{
			key_genres_PLATFORM_SELLS = true;
		}
		if (ES3.KeyExists("genres_NAME_EN", filename))
		{
			key_EN = true;
		}
		if (ES3.KeyExists("genres_NAME_GE", filename))
		{
			key_GE = true;
		}
		if (ES3.KeyExists("genres_NAME_TU", filename))
		{
			key_TU = true;
		}
		if (ES3.KeyExists("genres_NAME_CH", filename))
		{
			key_CH = true;
		}
		if (ES3.KeyExists("genres_NAME_FR", filename))
		{
			key_FR = true;
		}
		if (ES3.KeyExists("genres_NAME_PB", filename))
		{
			key_PB = true;
		}
		if (ES3.KeyExists("genres_NAME_HU", filename))
		{
			key_HU = true;
		}
		if (ES3.KeyExists("genres_NAME_TH", filename))
		{
			key_TH = true;
		}
		if (ES3.KeyExists("genres_NAME_UA", filename))
		{
			key_UA = true;
		}
		if (ES3.KeyExists("genres_NAME_CT", filename))
		{
			key_CT = true;
		}
		if (ES3.KeyExists("genres_NAME_ES", filename))
		{
			key_ES = true;
		}
		if (ES3.KeyExists("genres_NAME_PL", filename))
		{
			key_PL = true;
		}
		if (ES3.KeyExists("genres_NAME_CZ", filename))
		{
			key_CZ = true;
		}
		if (ES3.KeyExists("genres_NAME_KO", filename))
		{
			key_KO = true;
		}
		if (ES3.KeyExists("genres_NAME_AR", filename))
		{
			key_AR = true;
		}
		if (ES3.KeyExists("genres_NAME_RU", filename))
		{
			key_RU = true;
		}
		if (ES3.KeyExists("genres_NAME_IT", filename))
		{
			key_IT = true;
		}
		if (ES3.KeyExists("genres_NAME_JA", filename))
		{
			key_JA = true;
		}
	}
}
