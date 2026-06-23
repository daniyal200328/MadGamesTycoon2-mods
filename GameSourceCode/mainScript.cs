using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Vectrosity;

public class mainScript : MonoBehaviour
{
	public string buildVersion = "BUILD 2020.08.22A";

	public float[] gameSpeeds;

	public GameObject[] mainMenuObjects;

	public GameObject[] weatherEffects;

	public Light globalLight;

	public Color[] globalLightColors;

	public Font[] fonts;

	public int exklusivVertrag_ID = -1;

	public int exklusivVertrag_laufzeit;

	private publisherScript exkklusivVertragScript_;

	public float record_Gameplay;

	public float record_Grafik;

	public float record_Sound;

	public float record_Technik;

	public float auftragsAnsehen;

	public int studioPoints;

	public int myID = 100000;

	public publisherScript myPubS_;

	public string playerName = "";

	public int bankWarning;

	public bool multiplayer;

	public bool mpLobbyOpen;

	public int multiplayerSaveID;

	public int office;

	public int savegameVersion;

	public int year = 1976;

	public int month = 1;

	public int week = 1;

	public int difficulty;

	public int globalEvent = -1;

	public int globalEventWeeks;

	public int trendGenre;

	public int trendAntiGenre = 1;

	public int trendTheme;

	public int trendAntiTheme = 1;

	public int trendWeeks = 20;

	public int trendNextGenre;

	public int trendNextAntiGenre;

	public int trendNextTheme;

	public int trendNextAntiTheme;

	public float durchschnittsMotivationLastMonth;

	public long money = 200000L;

	public long kredit;

	public bool[] buildings;

	public int anrufe;

	public bool lastGameCommercialFlop;

	public float dayTimer;

	public float gameSpeed = 1f;

	public float gameSpeedOrg;

	private bool pauseGame;

	public int anzKonkurrenten;

	public float speedSetting = 0.05f;

	public int devTimeSetting;

	public int goldeneSchallplatten;

	public int platinSchallplatten;

	public int diamantSchallplatten;

	public int award_GOTY;

	public int award_Studio;

	public int award_Grafik;

	public int award_Sound;

	public int award_Trendsetter;

	public int award_Publisher;

	public int pubOffersAmount;

	public int lastUsedEngine;

	public GameObject charFoto;

	public GameObject guiPops;

	public GameObject guiRooms;

	public GameObject guiMoney;

	public int personal_druck = 1;

	public int personal_pausen = 1;

	public int personal_motivation = 40;

	public int personal_crunch = 90;

	public bool personal_dontLeaveBuilding;

	public bool personal_RobotDontLeaveBuilding;

	public bool personal_ki;

	public bool personal_autoGehaltsverhandlung = true;

	public string[] personal_group_names = new string[12];

	public float[] merchStandardpreis;

	public long[] fanshopverlauf = new long[24];

	public long[] finanzVerlauf = new long[24];

	public long[] verkaufsverlauf = new long[24];

	public long[] verkaufsverlaufKonsolen = new long[24];

	public long[] aboverlauf = new long[24];

	public long[] downloadverlauf = new long[24];

	public long[] fansverlauf = new long[24];

	public long[] finanzenMonat = new long[100];

	public long[] finanzenMonatLast = new long[100];

	public long[] finanzenJahr = new long[100];

	public long[] finanzenJahrLast = new long[100];

	public List<long> finanzVerlaufEinnahmen = new List<long>();

	public List<long> finanzVerlaufAusgaben = new List<long>();

	public List<string> history = new List<string>();

	public List<int> madGamesCon_Jahr = new List<int>();

	public List<int> madGamesCon_BestGrafik = new List<int>();

	public List<int> madGamesCon_BestSound = new List<int>();

	public List<int> madGamesCon_BestStudio = new List<int>();

	public List<int> madGamesCon_BestPublisher = new List<int>();

	public List<int> madGamesCon_BestGame = new List<int>();

	public List<int> madGamesCon_BadGame = new List<int>();

	public bool[] devLegendsInUse;

	public bool[] newsSetting;

	public bool[] gameTabFilter;

	public int[] lastGamesGenre;

	public int gelangweiltGenre;

	public int[] lastSchlechteSpiele;

	public int schlechteSpiele;

	public int sauerBugs;

	public int awardBonus;

	public float awardBonusAmount;

	public int sabotage_pr;

	public int sabotage_motivation;

	public int sabotage_klage;

	public int sabotage_reviews;

	public int sabotage_geruecht;

	public int sabotage_work;

	public int sabotage_dunkel;

	public int sabotage_erwischt;

	public bool sabotage_wurdeErwischt;

	public bool[] achivements;

	public bool[] achivementsDisabled;

	public int[] achivementsBonus;

	public int[] amountAchivementsBonus;

	public string marktforschung_datum = "";

	public float marktforschung_digtal = -1f;

	public float marktforschung_retail = -1f;

	public float marktforschung_deluxe = -1f;

	public float marktforschung_collectors = -1f;

	public float marktforschung_arcade = -1f;

	public float marktforschung_internet = -1f;

	public float marktforschung_gamePass = -1f;

	public int marktforschung_bestPlattform = -1;

	public int marktforschung_badPlattform = -1;

	public int marktforschung_bestPlattformKonsole = -1;

	public int marktforschung_badPlattformKonsole = -1;

	public int marktforschung_bestPlattformHandheld = -1;

	public int marktforschung_badPlattformHandheld = -1;

	public int marktforschung_bestPlattformHandy = -1;

	public int marktforschung_badPlattformHandy = -1;

	public int marktforschung_nextGenre = -1;

	public int marktforschung_nextTopic = -1;

	public int marktforschung_nextBadGenre = -1;

	public int marktforschung_nextBadTopic = -1;

	public bool support_kostenpflichtig;

	public bool automatic_RemoveGameFormMarket;

	public int automatic_RemoveGameFormMarket_Amount;

	public bool settings_autoPauseForMultiplayer;

	public bool settings_arbeitsgeschwindigkeitAnpassen;

	public bool settings_closeNPCs;

	public bool settings_plattformEnd;

	public bool settings_allGamespeed;

	public bool settings_sandbox;

	public bool settings_randomPlattformPop;

	public bool settings_randomPlattformSuit;

	public bool settings_randomGameConcept;

	public bool settings_randomGenreCombination;

	public bool settings_RandomReviews;

	public bool settings_TutorialOff = true;

	public bool settings_sabotageOff;

	public bool settings_tochterfirmaOff;

	public int settings_randomEvents;

	public int settings_startjahr;

	public int settings_competition;

	public int settings_randomPlattformNum = 1;

	public int settings_RandomReviewsNum = 1;

	public bool gameTabs_invert;

	public int gameTabs_sort = 1;

	public bool sandbox_unlimitedMoney = true;

	public bool sandbox_mitarbeiterMotivation = true;

	public bool sandbox_mitarbeiterPause = true;

	public bool sandbox_mitarbeiterKrank = true;

	public bool sandbox_mitarbeiterSkill100 = true;

	public bool sandbox_publisherMaxReleation = true;

	public bool sandbox_allItems = true;

	public bool sandbox_keinIpVerfall = true;

	public bool sandbox_bekannteKonzeptEinstellungen = true;

	public bool sandbox_fitTopicToGenre = true;

	public bool sandbox_allBuildings = true;

	public int sandbox_support;

	public int sandbox_arbeitsmarkt;

	public bool sandbox_tochterfirmaKonsole = true;

	public float sandbox_gameSells = 1f;

	public float sandbox_konsoleSells = 1f;

	public float sandbox_trainingSpeed = 1f;

	public float sandbox_maschineSpeed = 1f;

	public float sandbox_mitarbeiterGehalt = 1f;

	public float sandbox_mitarbeiterSpeed = 1f;

	public float sandbox_npcGameQuality = 1f;

	public int sandbox_bugs = 1;

	public int sandbox_lager = 1;

	public int sandbox_server = 1;

	public string sandbox_string = "";

	public bool badGameThisYear;

	public bool sellLagerbestandAutomatic;

	public AstarPath aStar_;

	public GameObject pathfinding_;

	public Camera myCamera;

	public textScript tS_;

	public settingsScript settings_;

	public mapScript mapScript_;

	public unlockScript unlock_;

	private pickCharacterScript pickChar_;

	public pickObjectScript pickObject_;

	public genres genres_;

	private themes themes_;

	public engineFeatures eF_;

	public gameplayFeatures gF_;

	public hardware hardware_;

	public hardwareFeatures hardwareFeatures_;

	private arbeitsmarkt arbeitsmarkt_;

	public platforms platforms_;

	public copyProtect copyProtect_;

	public anitCheat antiCheat_;

	public licences licences_;

	public games games_;

	public GUI_Main guiMain_;

	private npcEngines npcEngines_;

	private publisher publisher_;

	private createCharScript cCS_;

	public contractWorkMain contractWorkMain_;

	public publishingOfferMain publishingOfferMain_;

	public savegameScript save_;

	public cameraMovementScript cmS_;

	public mainCameraScript mcS_;

	public sfxScript sfx_;

	public NetworkManager manager;

	public mpCalls mpCalls_;

	public achiementScript achScript_;

	public reviewText reviewText_;

	public forschungSonstiges forschungSonstiges_;

	public roomDataScript rdS_;

	public autoInventarScript autoInventar_;

	public gamepassScript gpS_;

	public GameObject[] arrayCharacters;

	public List<GameObject> arrayCharactersForDoors = new List<GameObject>();

	public GameObject[] arrayObjects;

	public GameObject[] arrayRooms;

	public GameObject[] arrayRobots;

	public List<GameObject> arrayRobotsForDoors = new List<GameObject>();

	public GameObject[] arrayMuell;

	public GameObject[] arrayPublisher;

	public GameObject[] arrayEngines;

	public GameObject[] arrayPlatforms;

	public objectScript[] arrayObjectScripts;

	public characterScript[] arrayCharactersScripts;

	public robotScript[] arrayRobotsScripts;

	public roomScript[] arrayRoomScripts;

	public publisherScript[] arrayPublisherScripts;

	public engineScript[] arrayEnginesScripts;

	public platformScript[] arrayPlatformsScripts;

	public GameObject[] miscParticlePrefabs;

	public GameObject[] miscGamePrefabs;

	public Material[] specialMaterials;

	public Material[] floorMaterials;

	public Texture2D[] specialTextures;

	public Shader[] shaders;

	public float objectRotation;

	public GameObject pickedObject;

	public List<GameObject> pickedChars = new List<GameObject>();

	public bool snapObject;

	public bool snapRotation;

	public GameObject cameraPersonalPhoto;

	public VectorLine roomLine;

	public float weatherTimer;

	public int anzSprechblasen;

	public LayerMask layerMask_Floor;

	public const int taskID_taskForschung = 10;

	public const int taskID_taskEngine = 20;

	public const int taskID_taskUpdate = 30;

	public const int taskID_taskGame = 40;

	public const int taskID_taskF2PUpdate = 50;

	public const int taskID_taskMarketing = 60;

	public const int taskID_taskMarketingSpezial = 70;

	public const int taskID_taskMitarbeitersuche = 80;

	public const int taskID_taskTraining = 90;

	public const int taskID_taskSpielbericht = 100;

	public const int taskID_taskGameplayVerbessern = 110;

	public const int taskID_taskBugfixing = 120;

	public const int taskID_taskGrafikVerbessern = 130;

	public const int taskID_taskSoundVerbessern = 140;

	public const int taskID_taskAnimationVerbessern = 150;

	public const int taskID_taskProduction = 160;

	public const int taskID_taskArcadeProduction = 170;

	public const int taskID_taskKonsole = 180;

	public const int taskID_taskFankampagne = 190;

	public const int taskID_taskSupport = 200;

	public const int taskID_taskContractWork = 210;

	public const int taskID_taskContractWait = 220;

	public const int taskID_taskMarktforschung = 230;

	public const int taskID_taskPolishing = 240;

	public const int taskID_taskUnterstuetzen = 250;

	public const int taskID_taskWait = 260;

	public const int taskID_taskFanshop = 270;

	public const int taskID_taskForschungWait = 280;

	public const int taskID_taskKonsoleReduceCosts = 290;

	public const int taskID_taskKonsoleHaltbarkeit = 300;

	public const int taskID_taskAutoForschung = 310;

	public bool findObjects = true;

	public bool findRooms = true;

	public bool findCharacters = true;

	public bool findMuell = true;

	public bool findRobots = true;

	public bool officeLoaded;

	public List<objectScript> object_isArbeitsplatz = new List<objectScript>();

	public List<objectScript> object_canDrink = new List<objectScript>();

	public List<objectScript> object_isMuelleimer = new List<objectScript>();

	public List<objectScript> object_isPlant = new List<objectScript>();

	public List<objectScript> object_isWC = new List<objectScript>();

	public List<objectScript> object_isSink = new List<objectScript>();

	public List<objectScript> object_isHandtrockner = new List<objectScript>();

	public List<objectScript> object_isSeat = new List<objectScript>();

	public List<objectScript> object_isArcade = new List<objectScript>();

	public List<objectScript> object_isSeatAufenthalt = new List<objectScript>();

	public List<objectScript> object_isDart = new List<objectScript>();

	public List<objectScript> object_isMedizinSchrank = new List<objectScript>();

	public List<objectScript> object_isPiano = new List<objectScript>();

	public List<objectScript> object_isFreezer = new List<objectScript>();

	public List<objectScript> object_isTV = new List<objectScript>();

	public List<objectScript> object_isRadio = new List<objectScript>();

	public List<objectScript> object_isMinigolf = new List<objectScript>();

	public List<objectScript> object_isLaufband = new List<objectScript>();

	public List<objectScript> object_Aufenthalsraum = new List<objectScript>();

	private float updateKiTimer;

	private float updateUnkorrekterRoom;

	private float timerStopPopAnimations;

	public int autoSaveInterval = -1;

	private bool autoSaveMultiplayer;

	public float lauf = 0.2f;

	private float filterTimer;

	public float carSpawnTimer;

	public List<GameObject> carList = new List<GameObject>();

	public GameObject[] carPrefabs;

	public GameObject[] carSpawns;

	public List<Transform> listColliderLayer = new List<Transform>();

	private GameObject[] arrCopyProtect;

	private GameObject[] arrAntiCheat;

	private void Start()
	{
		RenameOldSaves();
		FindScripts();
	}

	public void FindScripts()
	{
		if (!myCamera)
		{
			myCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!tS_)
		{
			tS_ = base.gameObject.GetComponent<textScript>();
		}
		if (!mapScript_)
		{
			mapScript_ = base.gameObject.GetComponent<mapScript>();
		}
		if (!settings_)
		{
			settings_ = base.gameObject.GetComponent<settingsScript>();
		}
		if (!pickChar_)
		{
			pickChar_ = base.gameObject.GetComponent<pickCharacterScript>();
		}
		if (!pickObject_)
		{
			pickObject_ = base.gameObject.GetComponent<pickObjectScript>();
		}
		if (!unlock_)
		{
			unlock_ = base.gameObject.GetComponent<unlockScript>();
		}
		if (!arbeitsmarkt_)
		{
			arbeitsmarkt_ = base.gameObject.GetComponent<arbeitsmarkt>();
		}
		if (!genres_)
		{
			genres_ = base.gameObject.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = base.gameObject.GetComponent<themes>();
		}
		if (!eF_)
		{
			eF_ = base.gameObject.GetComponent<engineFeatures>();
		}
		if (!gF_)
		{
			gF_ = base.gameObject.GetComponent<gameplayFeatures>();
		}
		if (!hardware_)
		{
			hardware_ = base.gameObject.GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = base.gameObject.GetComponent<hardwareFeatures>();
		}
		if (!platforms_)
		{
			platforms_ = base.gameObject.GetComponent<platforms>();
		}
		if (!copyProtect_)
		{
			copyProtect_ = base.gameObject.GetComponent<copyProtect>();
		}
		if (!antiCheat_)
		{
			antiCheat_ = base.gameObject.GetComponent<anitCheat>();
		}
		if (!licences_)
		{
			licences_ = base.gameObject.GetComponent<licences>();
		}
		if (!games_)
		{
			games_ = base.gameObject.GetComponent<games>();
		}
		if (!npcEngines_)
		{
			npcEngines_ = base.gameObject.GetComponent<npcEngines>();
		}
		if (!publisher_)
		{
			publisher_ = base.gameObject.GetComponent<publisher>();
		}
		if (!cCS_)
		{
			cCS_ = base.gameObject.GetComponent<createCharScript>();
		}
		if (!achScript_)
		{
			achScript_ = base.gameObject.GetComponent<achiementScript>();
		}
		if (!reviewText_)
		{
			reviewText_ = base.gameObject.GetComponent<reviewText>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = base.gameObject.GetComponent<forschungSonstiges>();
		}
		if (!contractWorkMain_)
		{
			contractWorkMain_ = base.gameObject.GetComponent<contractWorkMain>();
		}
		if (!publishingOfferMain_)
		{
			publishingOfferMain_ = base.gameObject.GetComponent<publishingOfferMain>();
		}
		if (!autoInventar_)
		{
			autoInventar_ = base.gameObject.GetComponent<autoInventarScript>();
		}
		if (!gpS_)
		{
			gpS_ = base.gameObject.GetComponent<gamepassScript>();
		}
		if (!rdS_)
		{
			rdS_ = base.gameObject.GetComponent<roomDataScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!save_)
		{
			save_ = base.gameObject.GetComponent<savegameScript>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!manager)
		{
			manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
		if (!mcS_)
		{
			mcS_ = GameObject.FindWithTag("MainCamera").GetComponent<mainCameraScript>();
		}
	}

	private void Update()
	{
		if (!guiMain_.IsStartMenuActive())
		{
			if (findRobots)
			{
				FindRobots();
				findRobots = false;
			}
			if (findMuell)
			{
				FindMuell();
				findMuell = false;
			}
			if (findCharacters)
			{
				FindCharacters();
				findCharacters = false;
			}
			if (findObjects)
			{
				FindObjects();
				findObjects = false;
			}
			if (findRooms)
			{
				FindRooms();
				findRooms = false;
			}
			UpdateObjectsAndChars();
			UpdateTime();
			UpdateSpecialMaterials();
			UpdateCars();
			UpdateAchivementBonus();
			AutoSaveMultiplayer();
		}
	}

	private void DEBUG_CheckGamesArray()
	{
	}

	public void LoadOffice(int i, bool fromSavegame)
	{
		FindScripts();
		if (i != -1)
		{
			officeLoaded = false;
			office = i;
			switch (i)
			{
			case 3:
				aStar_.data.gridGraph.center = new Vector3(40f, -0.5f, 26f);
				aStar_.data.gridGraph.SetDimensions(228, 150, 0.35f);
				aStar_.Scan();
				break;
			case 4:
				aStar_.data.gridGraph.center = new Vector3(40f, -0.5f, 26f);
				aStar_.data.gridGraph.SetDimensions(228, 150, 0.35f);
				aStar_.Scan();
				break;
			case 5:
				aStar_.data.gridGraph.center = new Vector3(40f, -0.5f, 26f);
				aStar_.data.gridGraph.SetDimensions(228, 150, 0.35f);
				aStar_.Scan();
				break;
			case 6:
				aStar_.data.gridGraph.center = new Vector3(40f, -0.5f, 27f);
				aStar_.data.gridGraph.SetDimensions(200, 125, 0.35f);
				aStar_.Scan();
				break;
			case 7:
				aStar_.data.gridGraph.center = new Vector3(45f, -0.5f, 40f);
				aStar_.data.gridGraph.SetDimensions(228, 200, 0.35f);
				aStar_.Scan();
				break;
			case 8:
				aStar_.data.gridGraph.center = new Vector3(40f, -0.5f, 29f);
				aStar_.data.gridGraph.SetDimensions(228, 169, 0.35f);
				aStar_.Scan();
				break;
			}
			SceneManager.LoadScene("sceneOffice" + i, LoadSceneMode.Additive);
			StartCoroutine(iInitScene(fromSavegame));
		}
		else
		{
			if (multiplayer)
			{
				guiMain_.uiObjects[201].SetActive(value: true);
				guiMain_.uiObjects[201].GetComponent<mpMain>().StopNetwork();
				guiMain_.uiObjects[201].SetActive(value: false);
			}
			guiMain_.uiObjects[152].SetActive(value: false);
			guiMain_.uiObjects[151].SetActive(value: true);
			guiMain_.uiObjects[155].SetActive(value: false);
		}
	}

	public int GetMapIDfromDropdown(int value)
	{
		int result = 0;
		switch (value)
		{
		case 0:
			result = 3;
			break;
		case 1:
			result = 4;
			break;
		case 2:
			result = 6;
			break;
		case 3:
			result = 5;
			break;
		case 4:
			result = 7;
			break;
		case 5:
			result = 8;
			break;
		}
		return result;
	}

	public int GetDropdownSlotFromMapID(int id_)
	{
		int result = 0;
		switch (id_)
		{
		case 3:
			result = 0;
			break;
		case 4:
			result = 1;
			break;
		case 6:
			result = 2;
			break;
		case 5:
			result = 3;
			break;
		case 7:
			result = 4;
			break;
		case 8:
			result = 5;
			break;
		}
		return result;
	}

	public void CreateStartAuto(int officeID)
	{
		if (officeID == 3)
		{
			GameObject obj = UnityEngine.Object.Instantiate(mapScript_.prefabsInventar[92]);
			obj.transform.position = new Vector3(3.5f, 0f, 4f);
			objectScript component = obj.GetComponent<objectScript>();
			component.myID = 1;
			component.typ = 92;
			component.InitObjectFromSavegame();
			objectRotation = 0f;
			component.PlatziereObject(new Vector3(3.5f, 0f, 5f), fromSavegame: true, updatePathfinding: true, autoInventar: false, partikel: false);
		}
	}

	private IEnumerator iInitScene(bool fromSavegame)
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		mapScript_.InitBuilding(fromSavegame);
		GameObject.Find("CamMovement").GetComponent<cameraMovementScript>().FindCameraLimits();
		officeLoaded = true;
	}

	private void Cheat()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			guiMain_.uiObjects[216].SetActive(value: true);
			guiMain_.uiObjects[216].GetComponent<Menu_RandomEventGlobal>().Init(13);
		}
	}

	public void LoadContent()
	{
		tS_.LoadContent_NPCGameNames();
		tS_.LoadContent_NpcIPs();
		if (!GameObject.FindGameObjectWithTag("Platform"))
		{
			genres_.LoadGenres("DATA/Genres.txt");
			tS_.LoadContent_Themes();
			gF_.LoadGameplayFeatures("DATA/GameplayFeatures.txt");
			eF_.LoadEngineFeatures("DATA/EngineFeatures.txt");
			npcEngines_.LoadNpcEngines("DATA/NpcEngines.txt");
			publisher_.LoadPublisher("DATA/Publisher.txt");
			hardware_.LoadHardwareKomponenten("DATA/Hardware.txt");
			hardwareFeatures_.LoadHardwareFeatures("DATA/HardwareFeatures.txt");
			platforms_.LoadPlatforms("DATA/Platforms.txt");
			copyProtect_.LoadCopyProtect("DATA/CopyProtect.txt");
			antiCheat_.LoadAnitCheat("DATA/AntiCheat.txt");
			licences_.LoadLicences("DATA/Licence.txt");
		}
	}

	public void LoadContent_MultiplayerClient()
	{
		tS_.LoadContent_NPCGameNames();
		tS_.LoadContent_NpcIPs();
		licences_.LoadLicences("DATA/Licence.txt");
	}

	public void InitNewGame()
	{
		PlayerPrefs.SetInt("Toggle_Walls", 0);
		PlayerPrefs.SetInt("Toggle_PickChars", 0);
		PlayerPrefs.SetInt("Toggle_PickObjects", 0);
		PlayerPrefs.SetInt("Toggle_RoomUI", 0);
		PlayerPrefs.SetInt("Toggle_Ausstattung", 0);
		PlayerPrefs.SetInt("Toggle_Muell", 0);
		PlayerPrefs.SetInt("Toggle_Waerme", 0);
		arbeitsmarkt_.ArbeitsmarktUpdaten(dontDelete: true);
		licences_.LizenzenUpdaten();
		unlock_.NewGameUnlocks();
		unlock_.CheckUnlock(showMessage: false);
		Menu_NewGameCEO component = guiMain_.uiObjects[162].GetComponent<Menu_NewGameCEO>();
		characterScript obj = CreatePlayer(component.male, component.body, component.eyes, component.hair, component.beard, component.colorSkin, component.colorHair, component.colorHair, component.colorHose, component.colorShirt, component.colorAdd1);
		obj.myName = component.uiObjects[12].GetComponent<InputField>().text;
		obj.male = component.male;
		obj.beruf = component.beruf;
		obj.perks = (bool[])component.perks.Clone();
		obj.s_gamedesign = component.s_gamedesign;
		obj.s_programmieren = component.s_programmieren;
		obj.s_grafik = component.s_grafik;
		obj.s_sound = component.s_sound;
		obj.s_pr = component.s_pr;
		obj.s_gametests = component.s_gametests;
		obj.s_technik = component.s_technik;
		obj.s_forschen = component.s_forschen;
		UnlockRandomThemeAndGenre();
		if (!multiplayer || (multiplayer && mpCalls_.isServer))
		{
			UpdateTrend(newGame: true);
		}
		contractWorkMain_.UpdateContractWork(forceNewContract: true);
		platforms_.UpdatePlatformSells(sendDataToClient: true, forceSendAll: false);
		for (int i = 0; i < newsSetting.Length; i++)
		{
			newsSetting[i] = true;
		}
	}

	public void CreateStartAuftragsspiele()
	{
		int num = 0;
		for (int i = 0; i < arrayPublisherScripts.Length; i++)
		{
			if (!arrayPublisherScripts[i])
			{
				continue;
			}
			gameScript gameScript2 = arrayPublisherScripts[i].CreateNewGame2(forceContractGame: true, newTryForTochterfirma: false);
			if ((bool)gameScript2)
			{
				if (gameScript2.auftragsspiel)
				{
					num++;
				}
				if (num > 2)
				{
					break;
				}
			}
		}
	}

	public void MULTIPLAYER_UnlockRandomTopicsForClients()
	{
		bool flag = false;
		while (!flag)
		{
			themes_.themes_RES_POINTS_LEFT[UnityEngine.Random.Range(0, themes_.themes_RES_POINTS_LEFT.Length)] = 0f;
			int num = 0;
			for (int i = 0; i < themes_.themes_RES_POINTS_LEFT.Length; i++)
			{
				if (themes_.themes_RES_POINTS_LEFT[i] <= 0f)
				{
					num++;
				}
				if (num >= 3)
				{
					flag = true;
					break;
				}
			}
		}
	}

	public void UnlockRandomThemeAndGenre()
	{
		if (themes_.themes_RES_POINTS_LEFT == null || genres_.genres_UNLOCK == null || themes_.themes_RES_POINTS_LEFT.Length == 0 || genres_.genres_UNLOCK.Length == 0)
		{
			return;
		}
		if (multiplayer)
		{
			for (int i = 0; i < themes_.themes_RES_POINTS_LEFT.Length; i++)
			{
				themes_.themes_RES_POINTS_LEFT[i] = themes_.RES_POINTS;
			}
			for (int j = 0; j < genres_.genres_UNLOCK.Length; j++)
			{
				genres_.genres_RES_POINTS_LEFT[j] = genres_.genres_RES_POINTS[j];
			}
		}
		bool flag = false;
		while (!flag)
		{
			themes_.themes_RES_POINTS_LEFT[UnityEngine.Random.Range(0, themes_.themes_RES_POINTS_LEFT.Length)] = 0f;
			int num = 0;
			for (int k = 0; k < themes_.themes_RES_POINTS_LEFT.Length; k++)
			{
				if (themes_.themes_RES_POINTS_LEFT[k] <= 0f)
				{
					num++;
				}
				if (num >= 3)
				{
					flag = true;
					break;
				}
			}
		}
		List<int> list = new List<int>();
		for (int l = 0; l < genres_.genres_UNLOCK.Length; l++)
		{
			if (genres_.genres_UNLOCK[l])
			{
				list.Add(l);
			}
		}
		int num2 = list[UnityEngine.Random.Range(0, list.Count)];
		genres_.genres_RES_POINTS_LEFT[num2] = 0f;
	}

	public characterScript CreatePlayer(bool male_, int body_, int eyes_, int hair_, int beard_, int skinC_, int hairC_, int beardC_, int hoseC_, int shirtC_, int add1C_)
	{
		characterScript obj = cCS_.CreateCharacter(1, male_, body_);
		obj.myID = 1;
		obj.model_body = body_;
		obj.model_eyes = eyes_;
		obj.model_hair = hair_;
		obj.model_beard = beard_;
		obj.model_skinColor = skinC_;
		obj.model_hairColor = hairC_;
		obj.model_beardColor = beardC_;
		obj.model_HoseColor = hoseC_;
		obj.model_ShirtColor = shirtC_;
		obj.model_Add1Color = add1C_;
		obj.gameObject.transform.GetChild(0).GetComponent<characterGFXScript>().Init(forcedClothes: true);
		obj.gameObject.transform.position = new Vector3(10f, 0f, 10f);
		obj.gameObject.transform.eulerAngles = new Vector3(0f, 0f, -180f);
		return obj;
	}

	public void FindCharacters()
	{
		arrayCharacters = GameObject.FindGameObjectsWithTag("Character");
		arrayCharactersScripts = new characterScript[arrayCharacters.Length];
		for (int i = 0; i < arrayCharacters.Length; i++)
		{
			if ((bool)arrayCharacters[i])
			{
				arrayCharactersScripts[i] = arrayCharacters[i].GetComponent<characterScript>();
			}
		}
		if ((bool)achScript_)
		{
			if (arrayCharacters.Length >= 20)
			{
				achScript_.SetAchivement(60);
			}
			if (arrayCharacters.Length >= 50)
			{
				achScript_.SetAchivement(61);
			}
			if (arrayCharacters.Length >= 100)
			{
				achScript_.SetAchivement(62);
			}
		}
	}

	private void FindRobots()
	{
		arrayRobots = GameObject.FindGameObjectsWithTag("Robot");
		arrayRobotsScripts = new robotScript[arrayRobots.Length];
		for (int i = 0; i < arrayRobots.Length; i++)
		{
			if ((bool)arrayRobots[i])
			{
				arrayRobotsScripts[i] = arrayRobots[i].GetComponent<robotScript>();
			}
		}
	}

	private void FindMuell()
	{
		arrayMuell = GameObject.FindGameObjectsWithTag("Muell");
		if ((bool)guiMain_ && arrayMuell.Length > 10)
		{
			guiMain_.ShowBeschwerde(10, "");
		}
	}

	public void FindObjects()
	{
		arrayObjects = GameObject.FindGameObjectsWithTag("Object");
		object_isArbeitsplatz = new List<objectScript>();
		object_canDrink = new List<objectScript>();
		object_isMuelleimer = new List<objectScript>();
		object_isPlant = new List<objectScript>();
		object_isWC = new List<objectScript>();
		object_isSink = new List<objectScript>();
		object_isHandtrockner = new List<objectScript>();
		object_isSeat = new List<objectScript>();
		object_isArcade = new List<objectScript>();
		object_isSeatAufenthalt = new List<objectScript>();
		object_isDart = new List<objectScript>();
		object_isMedizinSchrank = new List<objectScript>();
		object_isPiano = new List<objectScript>();
		object_isFreezer = new List<objectScript>();
		object_isTV = new List<objectScript>();
		object_isRadio = new List<objectScript>();
		object_isMinigolf = new List<objectScript>();
		object_isLaufband = new List<objectScript>();
		object_Aufenthalsraum = new List<objectScript>();
		arrayObjectScripts = new objectScript[arrayObjects.Length];
		for (int i = 0; i < arrayObjects.Length; i++)
		{
			if ((bool)arrayObjects[i])
			{
				arrayObjectScripts[i] = arrayObjects[i].GetComponent<objectScript>();
				if (arrayObjectScripts[i].isArbeitsplatz)
				{
					object_isArbeitsplatz.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].canDrink)
				{
					object_canDrink.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isMuelleimer)
				{
					object_isMuelleimer.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isPlant)
				{
					object_isPlant.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isWC)
				{
					object_isWC.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isSink)
				{
					object_isSink.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isHandtrockner)
				{
					object_isHandtrockner.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isSeat)
				{
					object_isSeat.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isArcade)
				{
					object_isArcade.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isSeatAufenthalt)
				{
					object_isSeatAufenthalt.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isDart)
				{
					object_isDart.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isMedizinSchrank)
				{
					object_isMedizinSchrank.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isPiano)
				{
					object_isPiano.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isFreezer)
				{
					object_isFreezer.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isTV)
				{
					object_isTV.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isRadio)
				{
					object_isRadio.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isMinigolf)
				{
					object_isMinigolf.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
				if (arrayObjectScripts[i].isLaufband)
				{
					object_isLaufband.Add(arrayObjectScripts[i]);
					object_Aufenthalsraum.Add(arrayObjectScripts[i]);
				}
			}
		}
		mapScript_.UpdateMapFilter(force: true);
	}

	public void FindPublishers()
	{
		arrayPublisher = GameObject.FindGameObjectsWithTag("Publisher");
		arrayPublisherScripts = new publisherScript[arrayPublisher.Length];
		for (int i = 0; i < arrayPublisher.Length; i++)
		{
			if ((bool)arrayPublisher[i])
			{
				arrayPublisherScripts[i] = arrayPublisher[i].GetComponent<publisherScript>();
			}
		}
	}

	public void FindEngines()
	{
		arrayEngines = GameObject.FindGameObjectsWithTag("Engine");
		arrayEnginesScripts = new engineScript[arrayEngines.Length];
		for (int i = 0; i < arrayEngines.Length; i++)
		{
			if ((bool)arrayEngines[i])
			{
				arrayEnginesScripts[i] = arrayEngines[i].GetComponent<engineScript>();
			}
		}
	}

	public void FindPlatforms()
	{
		arrayPlatforms = GameObject.FindGameObjectsWithTag("Platform");
		arrayPlatformsScripts = new platformScript[arrayPlatforms.Length];
		for (int i = 0; i < arrayPlatforms.Length; i++)
		{
			if ((bool)arrayPlatforms[i])
			{
				arrayPlatformsScripts[i] = arrayPlatforms[i].GetComponent<platformScript>();
			}
		}
	}

	public void FindRooms()
	{
		arrayRooms = GameObject.FindGameObjectsWithTag("Room");
		arrayRoomScripts = new roomScript[arrayRooms.Length];
		for (int i = 0; i < arrayRooms.Length; i++)
		{
			if ((bool)arrayRooms[i])
			{
				arrayRoomScripts[i] = arrayRooms[i].GetComponent<roomScript>();
			}
		}
	}

	private void UpdateObjectsAndChars()
	{
		bool flag = false;
		updateUnkorrekterRoom += Time.deltaTime;
		if (updateUnkorrekterRoom > 0.3f)
		{
			updateUnkorrekterRoom = 0f;
			flag = true;
			if (guiMain_.menuOpen)
			{
				flag = false;
			}
			if (multiplayer)
			{
				if (guiMain_.uiObjects[19].activeSelf)
				{
					flag = false;
				}
				if (guiMain_.uiObjects[20].activeSelf)
				{
					flag = false;
				}
			}
		}
		for (int i = 0; i < arrayRoomScripts.Length; i++)
		{
			if ((bool)arrayRoomScripts[i])
			{
				arrayRoomScripts[i].mitarbeiterZugeteilt = 0;
			}
		}
		for (int j = 0; j < arrayObjectScripts.Length; j++)
		{
			if ((bool)arrayObjectScripts[j])
			{
				arrayObjectScripts[j].inUse = false;
				if (arrayObjectScripts[j].picked)
				{
					arrayObjectScripts[j].MouseMovement();
				}
				if (flag)
				{
					arrayObjectScripts[j].UpdateUnkorrekterRoom();
				}
			}
		}
		for (int k = 0; k < arrayCharactersScripts.Length; k++)
		{
			if ((bool)arrayCharactersScripts[k])
			{
				if ((bool)arrayCharactersScripts[k].objectUsingS_)
				{
					arrayCharactersScripts[k].objectUsingS_.inUse = true;
				}
				if ((bool)arrayCharactersScripts[k].roomS_)
				{
					arrayCharactersScripts[k].roomS_.mitarbeiterZugeteilt++;
				}
			}
		}
		timerStopPopAnimations += Time.deltaTime;
		bool flag2 = false;
		if (timerStopPopAnimations > 5f)
		{
			timerStopPopAnimations = 0f;
			flag2 = true;
		}
		arrayCharactersForDoors.Clear();
		for (int l = 0; l < arrayCharactersScripts.Length; l++)
		{
			if ((bool)arrayCharactersScripts[l])
			{
				arrayCharactersScripts[l].UpdateMe();
				if (flag2)
				{
					arrayCharactersScripts[l].StopPopAnimations();
				}
				if (!arrayCharactersScripts[l].iDoWork && arrayCharactersScripts[l].gameObject.transform.position.y < 0.001f && (bool)arrayCharactersScripts[l].moveS_ && arrayCharactersScripts[l].moveS_.waitForceAnimation <= 0f)
				{
					arrayCharactersForDoors.Add(arrayCharactersScripts[l].gameObject);
				}
			}
		}
		arrayRobotsForDoors.Clear();
		for (int m = 0; m < arrayRobotsScripts.Length; m++)
		{
			if ((bool)arrayRobotsScripts[m])
			{
				arrayRobotsScripts[m].UpdateMe();
				if ((bool)arrayRobotsScripts[m].myTarget)
				{
					arrayRobotsForDoors.Add(arrayRobotsScripts[m].gameObject);
				}
			}
		}
		updateKiTimer += Time.deltaTime;
		if (updateKiTimer < 1f)
		{
			return;
		}
		updateKiTimer = 0f;
		if (!personal_ki)
		{
			return;
		}
		for (int n = 0; n < arrayCharactersScripts.Length; n++)
		{
			if ((bool)arrayCharactersScripts[n])
			{
				arrayCharactersScripts[n].UpdateKI(roomSpecific: true);
			}
		}
		for (int num = 0; num < arrayCharactersScripts.Length; num++)
		{
			if ((bool)arrayCharactersScripts[num])
			{
				arrayCharactersScripts[num].UpdateKI(roomSpecific: false);
			}
		}
	}

	public bool IsForcedPause()
	{
		return pauseGame;
	}

	public void PauseGame(bool p)
	{
		if (multiplayer)
		{
			return;
		}
		pauseGame = p;
		if (p)
		{
			if (gameSpeed > 0f)
			{
				gameSpeedOrg = gameSpeed;
				gameSpeed = 0f;
			}
			return;
		}
		if (gameSpeedOrg > 0f)
		{
			gameSpeed = gameSpeedOrg;
			gameSpeedOrg = 0f;
		}
		if (guiMain_.spacePause)
		{
			gameSpeedOrg = gameSpeed;
			gameSpeed = 0f;
			pauseGame = true;
		}
	}

	private void UpdateTime()
	{
		dayTimer += Time.deltaTime * speedSetting * GetGameSpeed();
		UpdateWeatherEffects();
		if (multiplayer && mpCalls_.isClient)
		{
			if (dayTimer >= 1f)
			{
				dayTimer = 1f;
			}
		}
		else if (dayTimer >= 1f)
		{
			Autosave();
			WochenUpdates();
			if (week >= 5)
			{
				MonatlicheUpdates();
			}
		}
	}

	public void WochenUpdates()
	{
		if (multiplayer && mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Command(3);
		}
		dayTimer = 0f;
		week++;
		ResetGamePass();
		contractWorkMain_.UpdateContractWork(forceNewContract: false);
		arbeitsmarkt_.ArbeitsmarktUpdaten(dontDelete: false);
		platforms_.UpdatePlatformSells(sendDataToClient: true, forceSendAll: false);
		copyProtect_.UpdateEffekt();
		antiCheat_.UpdateEffekt();
		licences_.LizenzenUpdaten();
		UpdateTrend(newGame: false);
		games_.SaveLastChartPosition();
		games_.SellAllGames();
		games_.UpdateChartsWeek();
		UpdateAnrufeWeekly();
		UpdateGlobalEvent();
		UpdateGenreBeliebtheit();
		guiMain_.UpdateCharts();
		UpdateGamePass();
		if (sauerBugs > 0)
		{
			sauerBugs--;
		}
		if (sauerBugs < 0)
		{
			sauerBugs = 0;
		}
		if (awardBonus > 0)
		{
			awardBonus--;
		}
		if (awardBonus < 0)
		{
			awardBonus = 0;
		}
		UpdateSabotage();
		UpdateSchlechteSpiele();
		for (int i = 0; i < arrayCharactersScripts.Length; i++)
		{
			if ((bool)arrayCharactersScripts[i])
			{
				arrayCharactersScripts[i].UpdateKuendigen(force: false);
				arrayCharactersScripts[i].UpdateKrank();
				arrayCharactersScripts[i].Gehaltsverhandlung();
			}
		}
		for (int j = 0; j < arrayPublisherScripts.Length; j++)
		{
			if ((bool)arrayPublisherScripts[j] && !arrayPublisherScripts[j].isPlayer)
			{
				arrayPublisherScripts[j].amountTrys = 0;
				arrayPublisherScripts[j].CreateNewGame2(forceContractGame: false, newTryForTochterfirma: false);
			}
		}
		unlock_.CheckUnlock(showMessage: true);
	}

	private void ResetGamePass()
	{
		gpS_.gamePass_AbosLetzteWoche = 0L;
		for (int num = gpS_.gamePass_aboVerlaufWoche.Length - 1; num >= 1; num--)
		{
			gpS_.gamePass_aboVerlaufWoche[num] = gpS_.gamePass_aboVerlaufWoche[num - 1];
		}
	}

	private void UpdateSabotage()
	{
		if (sabotage_pr > 0)
		{
			sabotage_pr--;
		}
		if (sabotage_pr < 0)
		{
			sabotage_pr = 0;
		}
		if (sabotage_motivation > 0)
		{
			sabotage_motivation--;
		}
		if (sabotage_motivation < 0)
		{
			sabotage_motivation = 0;
		}
		if (sabotage_klage > 0)
		{
			sabotage_klage--;
		}
		if (sabotage_klage < 0)
		{
			sabotage_klage = 0;
		}
		if (sabotage_reviews > 0)
		{
			sabotage_reviews--;
		}
		if (sabotage_reviews < 0)
		{
			sabotage_reviews = 0;
		}
		if (sabotage_geruecht > 0)
		{
			sabotage_geruecht--;
		}
		if (sabotage_geruecht < 0)
		{
			sabotage_geruecht = 0;
		}
		if (sabotage_work > 0)
		{
			sabotage_work--;
		}
		if (sabotage_work < 0)
		{
			sabotage_work = 0;
		}
		if (sabotage_dunkel > 0)
		{
			sabotage_dunkel--;
		}
		if (sabotage_dunkel < 0)
		{
			sabotage_dunkel = 0;
		}
		if (sabotage_erwischt > 0)
		{
			sabotage_erwischt--;
		}
		if (sabotage_erwischt < 0)
		{
			sabotage_erwischt = 0;
		}
		if (!settings_sabotageOff && sabotage_klage > 0)
		{
			long num = money / 100;
			if (num < 1000)
			{
				num = 1000L;
			}
			Pay(num, 9);
		}
		if (!settings_sabotageOff && sabotage_dunkel <= 0 && sabotage_wurdeErwischt)
		{
			sabotage_wurdeErwischt = false;
			sabotage_dunkel = 0;
			sabotage_erwischt = 24;
			guiMain_.uiObjects[444].SetActive(value: true);
			guiMain_.uiObjects[444].GetComponent<Menu_SabotageErwicht_Message>().Init();
		}
	}

	private void UpdateGamePass()
	{
		gpS_.gamePass_aboVerlaufWoche[0] = gpS_.gamePass_Abos;
		if (week >= 5)
		{
			long num = gpS_.gamePass_Abos * gpS_.gamePass_AboPreis;
			gpS_.gamePass_UmsatzJahr += num;
			Earn(num, 15);
		}
		if (month >= 13)
		{
			gpS_.gamePass_UmsatzJahr = 0L;
		}
		if ((bool)guiMain_ && (bool)guiMain_.gamePassTab_)
		{
			guiMain_.gamePassTab_.GetComponent<gamePassTab>().UpdateData();
		}
	}

	private void UpdateSchlechteSpiele()
	{
		if (schlechteSpiele > 0)
		{
			schlechteSpiele--;
			if (schlechteSpiele <= 0)
			{
				for (int i = 0; i < lastSchlechteSpiele.Length; i++)
				{
					lastSchlechteSpiele[i] = 0;
				}
			}
			return;
		}
		if (schlechteSpiele < 0)
		{
			schlechteSpiele = 0;
		}
		int num = 0;
		if (lastSchlechteSpiele[0] > 0 && lastSchlechteSpiele[0] < 30)
		{
			num++;
		}
		if (lastSchlechteSpiele[1] > 0 && lastSchlechteSpiele[1] < 30)
		{
			num++;
		}
		if (lastSchlechteSpiele[2] > 0 && lastSchlechteSpiele[2] < 30)
		{
			num++;
		}
		if (lastSchlechteSpiele[3] > 0 && lastSchlechteSpiele[3] < 30)
		{
			num++;
		}
		if (lastSchlechteSpiele[4] > 0 && lastSchlechteSpiele[4] < 30)
		{
			num++;
		}
		if (num >= 3)
		{
			schlechteSpiele = UnityEngine.Random.Range(12, 24);
		}
	}

	public void MonatlicheUpdates()
	{
		if (multiplayer && mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Command(4);
		}
		week = 1;
		month++;
		autoSaveInterval--;
		for (int i = 0; i < arrayObjectScripts.Length; i++)
		{
			if ((bool)arrayObjectScripts[i])
			{
				arrayObjectScripts[i].Monatskosten();
				arrayObjectScripts[i].AddAufladungen();
			}
		}
		for (int j = 0; j < arrayCharactersScripts.Length; j++)
		{
			if ((bool)arrayCharactersScripts[j])
			{
				arrayCharactersScripts[j].Monatskosten();
				durchschnittsMotivationLastMonth += arrayCharactersScripts[j].s_motivation;
			}
		}
		durchschnittsMotivationLastMonth /= arrayCharacters.Length;
		for (int k = 0; k < arrayPublisherScripts.Length; k++)
		{
			if ((bool)arrayPublisherScripts[k] && !arrayPublisherScripts[k].isPlayer)
			{
				arrayPublisherScripts[k].UpdateRandomData();
				arrayPublisherScripts[k].VerwaltungskostenBezahlen();
				arrayPublisherScripts[k].UpdateTochterfirmaUmsatzVerlauf();
			}
		}
		unlock_.CheckUnlock(showMessage: true);
		PayBankZinsen();
		UpdateStatsVerlaeufe();
		AddFanverlauf(genres_.GetAmountFans());
		ResetMonatsbilanz();
		MadGamesAward(force: false);
		MadGamesConvention();
		UpdateExklusivPublisher();
		UpdateBankWarning();
		UpdateTasks();
		ResetOwnPlatformGarantie();
		if (month >= 13)
		{
			month = 1;
			year++;
			badGameThisYear = false;
			pubOffersAmount = 0;
			gpS_.gamePass_UmsatzJahr = 0L;
			if (year >= 2050)
			{
				achScript_.SetAchivement(45);
			}
			EnginesReduceMarktdominanz();
			if (!multiplayer && !settings_.hideJahresabrechnung)
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[460]);
			}
		}
	}

	private void ResetOwnPlatformGarantie()
	{
		for (int i = 0; i < arrayPlatformsScripts.Length; i++)
		{
			if ((bool)arrayPlatformsScripts[i] && arrayPlatformsScripts[i].ownerID == myID)
			{
				arrayPlatformsScripts[i].ResetGarantieMonth();
			}
		}
	}

	private void EnginesReduceMarktdominanz()
	{
		for (int i = 0; i < arrayEnginesScripts.Length; i++)
		{
			if ((bool)arrayEnginesScripts[i] && arrayEnginesScripts[i].GetMarktdominanz() > 0f)
			{
				arrayEnginesScripts[i].AddMarktdominanz(-10f + UnityEngine.Random.Range(0f, 3f));
			}
		}
	}

	private void UpdateTasks()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Task");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				taskFanshop component = array[i].GetComponent<taskFanshop>();
				if ((bool)component)
				{
					component.ResetData();
				}
			}
		}
	}

	public void Autosave()
	{
		if (settings_.saveInterval == 5)
		{
			return;
		}
		if (autoSaveInterval <= -1)
		{
			switch (settings_.saveInterval)
			{
			case 0:
				autoSaveInterval = 12;
				break;
			case 1:
				autoSaveInterval = 9;
				break;
			case 2:
				autoSaveInterval = 6;
				break;
			case 3:
				autoSaveInterval = 3;
				break;
			case 4:
				autoSaveInterval = 1;
				break;
			default:
				autoSaveInterval = 12;
				break;
			}
		}
		if (autoSaveInterval == 0)
		{
			switch (settings_.saveInterval)
			{
			case 0:
				autoSaveInterval = 12;
				break;
			case 1:
				autoSaveInterval = 9;
				break;
			case 2:
				autoSaveInterval = 6;
				break;
			case 3:
				autoSaveInterval = 3;
				break;
			case 4:
				autoSaveInterval = 1;
				break;
			default:
				autoSaveInterval = 12;
				break;
			}
			if (!multiplayer)
			{
				save_.Save(0);
				guiMain_.ShowGameHasSaved();
				GC.Collect();
				Resources.UnloadUnusedAssets();
			}
			else
			{
				autoSaveMultiplayer = true;
			}
		}
	}

	public void AutoSaveMultiplayer()
	{
		if (settings_.saveInterval != 5 && (bool)guiMain_ && multiplayer && mpCalls_.isServer && !guiMain_.menuOpen && autoSaveMultiplayer && dayTimer > 0.5f && guiMain_.uiObjects[1].activeSelf)
		{
			autoSaveMultiplayer = false;
			save_.SaveMultiplayer(0);
			GC.Collect();
			Resources.UnloadUnusedAssets();
		}
	}

	public void MadGamesAward(bool force)
	{
		if ((!multiplayer || (multiplayer && mpCalls_.isServer) || force) && (month == 12 || force) && (week == 1 || force))
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[143]);
			guiMain_.uiObjects[143].GetComponent<Menu_Awards>().Init();
			guiMain_.OpenMenu(hideChars: false);
			sfx_.PlaySound(50, force: false);
		}
	}

	private void MadGamesConvention()
	{
		if (month == 7 && week == 1)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[185]);
			guiMain_.uiObjects[185].GetComponent<Menu_Messe>().Init();
		}
	}

	public string GetMoney(long i, bool showDollar)
	{
		if (!tS_)
		{
			FindScripts();
		}
		string text = ".";
		if (settings_.language == 0)
		{
			text = ",";
		}
		string text2 = i.ToString();
		text2 = text2.Replace("-", "");
		string text3 = "";
		int num = 0;
		for (int num2 = text2.Length - 1; num2 >= 0; num2--)
		{
			text3 = text2[num2] + text3;
			if (num2 != 0)
			{
				num++;
				if (num >= 3)
				{
					text3 = text + text3;
					num = 0;
				}
			}
		}
		if (i < 0)
		{
			text3 = "-" + text3;
		}
		if (showDollar)
		{
			return tS_.GetText(7) + text3;
		}
		return text3;
	}

	public Sprite LoadPNG(string filePath)
	{
		Texture2D texture2D = null;
		if (File.Exists(filePath))
		{
			byte[] data = File.ReadAllBytes(filePath);
			texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
			texture2D.LoadImage(data);
		}
		else
		{
			byte[] data = File.ReadAllBytes(Application.dataPath + "/Extern/Icons_Platforms/missing.png");
			texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: false);
			texture2D.LoadImage(data);
			Debug.Log("LoadPNG() -> Missing File: " + filePath);
		}
		return Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), Vector2.zero);
	}

	public Texture2D LoadTexture(string filePath)
	{
		Texture2D texture2D = null;
		if (File.Exists(filePath))
		{
			byte[] data = File.ReadAllBytes(filePath);
			texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, mipChain: true);
			texture2D.LoadImage(data);
		}
		else
		{
			Debug.Log("LoadPNG() -> Missing File: " + filePath);
		}
		return texture2D;
	}

	public void CopyArbeitsmarktCharacterData(charArbeitsmarkt cA_, characterScript cS_)
	{
		cS_.myID = cA_.myID;
		cS_.male = cA_.male;
		cS_.myName = cA_.myName;
		cS_.legend = cA_.legend;
		cS_.beruf = cA_.beruf;
		cS_.s_motivation = 100f;
		cS_.s_gamedesign = cA_.s_gamedesign;
		cS_.s_programmieren = cA_.s_programmieren;
		cS_.s_grafik = cA_.s_grafik;
		cS_.s_sound = cA_.s_sound;
		cS_.s_pr = cA_.s_pr;
		cS_.s_gametests = cA_.s_gametests;
		cS_.s_technik = cA_.s_technik;
		cS_.s_forschen = cA_.s_forschen;
		cS_.perks = (bool[])cA_.perks.Clone();
		cS_.durst = UnityEngine.Random.Range(15f, 100f);
		cS_.hunger = UnityEngine.Random.Range(15f, 100f);
		cS_.klo = UnityEngine.Random.Range(15f, 100f);
		cS_.muell = UnityEngine.Random.Range(15f, 100f);
		cS_.giessen = UnityEngine.Random.Range(15f, 100f);
	}

	public long finanzenJahr_GetGewinn()
	{
		long num = 0L;
		for (int i = 0; i < 50; i++)
		{
			num += finanzenJahr[i];
		}
		long num2 = 0L;
		for (int j = 50; j < 100; j++)
		{
			num2 += finanzenJahr[j];
		}
		return num2 - num;
	}

	public long finanzenJahrLast_GetGewinn()
	{
		long num = 0L;
		for (int i = 0; i < 50; i++)
		{
			num += finanzenJahrLast[i];
		}
		long num2 = 0L;
		for (int j = 50; j < 100; j++)
		{
			num2 += finanzenJahrLast[j];
		}
		return num2 - num;
	}

	public long finanzenMonat_GetGewinn()
	{
		long num = 0L;
		for (int i = 0; i < 50; i++)
		{
			num += finanzenMonat[i];
		}
		long num2 = 0L;
		for (int j = 50; j < 100; j++)
		{
			num2 += finanzenMonat[j];
		}
		return num2 - num;
	}

	public long finanzenMonatLast_GetGewinn()
	{
		long num = 0L;
		for (int i = 0; i < 50; i++)
		{
			num += finanzenMonatLast[i];
		}
		long num2 = 0L;
		for (int j = 50; j < 100; j++)
		{
			num2 += finanzenMonatLast[j];
		}
		return num2 - num;
	}

	private void ResetMonatsbilanz()
	{
		finanzVerlauf[0] = finanzenMonat_GetGewinn();
		if (month >= 13)
		{
			ResetJahresbilanz();
		}
		for (int i = 0; i < finanzenMonat.Length; i++)
		{
			finanzenMonatLast[i] = finanzenMonat[i];
			finanzenMonat[i] = 0L;
		}
	}

	public void ResetJahresbilanz()
	{
		long num = 0L;
		for (int i = 0; i < 50; i++)
		{
			num += finanzenJahr[i];
		}
		long num2 = 0L;
		for (int j = 50; j < 100; j++)
		{
			num2 += finanzenJahr[j];
		}
		finanzVerlaufEinnahmen.Add(num2);
		finanzVerlaufAusgaben.Add(num);
		for (int k = 0; k < finanzenJahr.Length; k++)
		{
			finanzenJahrLast[k] = finanzenJahr[k];
			finanzenJahr[k] = 0L;
		}
	}

	public void AddMadGameConvetionVerlauf(int bestGrafik_, int bestSound_, int bestStudio_, int bestPublisher_, int bestGame_, int badBame_)
	{
		madGamesCon_Jahr.Add(year);
		madGamesCon_BestGrafik.Add(bestGrafik_);
		madGamesCon_BestSound.Add(bestSound_);
		madGamesCon_BestStudio.Add(bestStudio_);
		madGamesCon_BestPublisher.Add(bestPublisher_);
		madGamesCon_BestGame.Add(bestGame_);
		madGamesCon_BadGame.Add(badBame_);
	}

	public void Pay(long amount, int what)
	{
		money -= amount;
		switch (what)
		{
		case 0:
			finanzenMonat[0] += amount;
			finanzenJahr[0] += amount;
			break;
		case 1:
			finanzenMonat[1] += amount;
			finanzenJahr[1] += amount;
			break;
		case 2:
			finanzenMonat[3] += amount;
			finanzenJahr[3] += amount;
			break;
		case 3:
			finanzenMonat[6] += amount;
			finanzenJahr[6] += amount;
			break;
		case 4:
			finanzenMonat[2] += amount;
			finanzenJahr[2] += amount;
			break;
		case 5:
			finanzenMonat[6] += amount;
			finanzenJahr[6] += amount;
			break;
		case 6:
			finanzenMonat[6] += amount;
			finanzenJahr[6] += amount;
			break;
		case 7:
			finanzenMonat[7] += amount;
			finanzenJahr[7] += amount;
			break;
		case 8:
			finanzenMonat[9] += amount;
			finanzenJahr[9] += amount;
			break;
		case 9:
			finanzenMonat[4] += amount;
			finanzenJahr[4] += amount;
			break;
		case 10:
			finanzenMonat[2] += amount;
			finanzenJahr[2] += amount;
			break;
		case 11:
			finanzenMonat[7] += amount;
			finanzenJahr[7] += amount;
			break;
		case 12:
			finanzenMonat[5] += amount;
			finanzenJahr[5] += amount;
			break;
		case 13:
			finanzenMonat[9] += amount;
			finanzenJahr[9] += amount;
			break;
		case 14:
			finanzenMonat[8] += amount;
			finanzenJahr[8] += amount;
			break;
		case 15:
			finanzenMonat[2] += amount;
			finanzenJahr[2] += amount;
			break;
		case 16:
			finanzenMonat[5] += amount;
			finanzenJahr[5] += amount;
			break;
		case 17:
			finanzenMonat[5] += amount;
			finanzenJahr[5] += amount;
			break;
		case 18:
			finanzenMonat[2] += amount;
			finanzenJahr[2] += amount;
			break;
		case 19:
			finanzenMonat[10] += amount;
			finanzenJahr[10] += amount;
			break;
		case 20:
			finanzenMonat[11] += amount;
			finanzenJahr[11] += amount;
			break;
		case 21:
			finanzenMonat[12] += amount;
			finanzenJahr[12] += amount;
			break;
		case 22:
			finanzenMonat[13] += amount;
			finanzenJahr[13] += amount;
			break;
		case 23:
			finanzenMonat[14] += amount;
			finanzenJahr[14] += amount;
			break;
		case 24:
			finanzenMonat[4] += amount;
			finanzenJahr[4] += amount;
			break;
		case 25:
			finanzenMonat[9] += amount;
			finanzenJahr[9] += amount;
			break;
		case 26:
			finanzenMonat[9] += amount;
			finanzenJahr[9] += amount;
			break;
		case 27:
			finanzenMonat[15] += amount;
			finanzenJahr[15] += amount;
			break;
		case 28:
			finanzenMonat[16] += amount;
			finanzenJahr[16] += amount;
			break;
		case 29:
			finanzenMonat[16] += amount;
			finanzenJahr[16] += amount;
			break;
		case 30:
			finanzenMonat[16] += amount;
			finanzenJahr[16] += amount;
			break;
		case 31:
			finanzenMonat[9] += amount;
			finanzenJahr[9] += amount;
			break;
		case 32:
			finanzenMonat[14] += amount;
			finanzenJahr[14] += amount;
			break;
		case 33:
			finanzenMonat[18] += amount;
			finanzenJahr[18] += amount;
			break;
		case 34:
			finanzenMonat[17] += amount;
			finanzenJahr[17] += amount;
			break;
		}
		if (settings_sandbox && sandbox_unlimitedMoney)
		{
			money = 100000000000L;
		}
	}

	public void Earn(long amount, int what)
	{
		money += amount;
		switch (what)
		{
		case 0:
			finanzenMonat[54] += amount;
			finanzenJahr[54] += amount;
			break;
		case 1:
			finanzenMonat[56] += amount;
			finanzenJahr[56] += amount;
			break;
		case 2:
			finanzenMonat[55] += amount;
			finanzenJahr[55] += amount;
			break;
		case 3:
			finanzenMonat[50] += amount;
			finanzenJahr[50] += amount;
			break;
		case 4:
			finanzenMonat[51] += amount;
			finanzenJahr[51] += amount;
			break;
		case 5:
			finanzenMonat[52] += amount;
			finanzenJahr[52] += amount;
			break;
		case 6:
			finanzenMonat[53] += amount;
			finanzenJahr[53] += amount;
			break;
		case 7:
			finanzenMonat[57] += amount;
			finanzenJahr[57] += amount;
			break;
		case 8:
			finanzenMonat[58] += amount;
			finanzenJahr[58] += amount;
			break;
		case 9:
			finanzenMonat[59] += amount;
			finanzenJahr[59] += amount;
			break;
		case 10:
			finanzenMonat[56] += amount;
			finanzenJahr[56] += amount;
			break;
		case 11:
			finanzenMonat[60] += amount;
			finanzenJahr[60] += amount;
			break;
		case 12:
			finanzenMonat[61] += amount;
			finanzenJahr[61] += amount;
			break;
		case 13:
			finanzenMonat[61] += amount;
			finanzenJahr[61] += amount;
			break;
		case 14:
			finanzenMonat[56] += amount;
			finanzenJahr[56] += amount;
			break;
		case 15:
			finanzenMonat[62] += amount;
			finanzenJahr[62] += amount;
			break;
		}
		if ((bool)achScript_)
		{
			if (money >= 50000000)
			{
				achScript_.SetAchivement(63);
			}
			if (money >= 500000000)
			{
				achScript_.SetAchivement(64);
			}
			if (money >= 1000000000)
			{
				achScript_.SetAchivement(65);
			}
		}
		if (settings_sandbox && sandbox_unlimitedMoney)
		{
			money = 100000000000L;
		}
	}

	private void UpdateAnrufeWeekly()
	{
		if (settings_sandbox && sandbox_support == 1)
		{
			return;
		}
		int amountFans = genres_.GetAmountFans();
		if (amountFans < 5000)
		{
			return;
		}
		float num = amountFans / 3000;
		AddAnrufe(UnityEngine.Random.Range(0, (int)num));
		switch (GetAnrufeAmount())
		{
		case 2:
		{
			float num4 = UnityEngine.Random.Range(0f, (float)amountFans * 0.001f);
			if (support_kostenpflichtig)
			{
				num4 *= 3f;
			}
			AddFans(-UnityEngine.Random.Range(0, Mathf.RoundToInt(num4)), -1);
			break;
		}
		case 3:
		{
			float num2 = UnityEngine.Random.Range(0f, (float)amountFans * 0.001f);
			float num3 = UnityEngine.Random.Range(0f, (float)amountFans * 0.002f);
			if (support_kostenpflichtig)
			{
				num2 *= 3f;
				num3 *= 3f;
			}
			AddFans(-UnityEngine.Random.Range(Mathf.RoundToInt(num2), Mathf.RoundToInt(num3)), -1);
			break;
		}
		case 0:
		case 1:
			break;
		}
	}

	public float GetAnrufe100Prozent()
	{
		int num = genres_.GetAmountFans();
		if (num < 5000)
		{
			num = 5000;
		}
		float num2 = (float)num * 0.01f;
		if (num2 > 0f)
		{
			num2 = (float)anrufe / num2 * 5f;
		}
		return num2;
	}

	public int GetAnrufeAmount()
	{
		if (genres_.GetAmountFans() <= 0)
		{
			return 0;
		}
		if (anrufe <= 0)
		{
			return 0;
		}
		float anrufe100Prozent = GetAnrufe100Prozent();
		if (anrufe100Prozent >= 0f && anrufe100Prozent <= 25f)
		{
			return 0;
		}
		if (anrufe100Prozent >= 25f && anrufe100Prozent <= 50f)
		{
			return 1;
		}
		if (anrufe100Prozent >= 50f && anrufe100Prozent <= 75f)
		{
			return 2;
		}
		if (anrufe100Prozent >= 75f)
		{
			return 3;
		}
		return 0;
	}

	public void AddAnrufe(int i)
	{
		float num = i;
		switch (difficulty)
		{
		case 0:
			num *= 0.2f;
			break;
		case 1:
			num *= 0.3f;
			break;
		case 2:
			num *= 0.4f;
			break;
		case 3:
			num *= 0.6f;
			break;
		case 4:
			num *= 0.8f;
			break;
		case 5:
			num *= 1f;
			break;
		}
		i = Mathf.RoundToInt(num);
		if (settings_sandbox)
		{
			if (sandbox_support == 1)
			{
				return;
			}
			float num2 = i;
			switch (sandbox_support)
			{
			case 1:
				i = 0;
				break;
			case 2:
				num2 *= 0.9f;
				break;
			case 3:
				num2 *= 0.8f;
				break;
			case 4:
				num2 *= 0.7f;
				break;
			case 5:
				num2 *= 0.6f;
				break;
			case 6:
				num2 *= 0.5f;
				break;
			case 7:
				num2 *= 0.4f;
				break;
			case 8:
				num2 *= 0.3f;
				break;
			case 9:
				num2 *= 0.2f;
				break;
			case 10:
				num2 *= 0.1f;
				break;
			case 11:
				num2 *= 1.2f;
				break;
			case 12:
				num2 *= 1.4f;
				break;
			case 13:
				num2 *= 1.6f;
				break;
			case 14:
				num2 *= 1.8f;
				break;
			case 15:
				num2 *= 2f;
				break;
			case 16:
				num2 *= 2.2f;
				break;
			case 17:
				num2 *= 2.4f;
				break;
			case 18:
				num2 *= 2.6f;
				break;
			case 19:
				num2 *= 2.8f;
				break;
			case 20:
				num2 *= 3f;
				break;
			}
			i = Mathf.RoundToInt(num2);
		}
		int amountFans = genres_.GetAmountFans();
		anrufe += i;
		if (anrufe < 0)
		{
			anrufe = 0;
		}
		if (anrufe > amountFans)
		{
			anrufe = amountFans;
		}
	}

	public void AddFans(int i, int genre_)
	{
		if (((gelangweiltGenre != -1 || sauerBugs > 0 || schlechteSpiele > 0) && genre_ != -1 && i >= 0) || (!settings_sabotageOff && sabotage_erwischt > 0 && i >= 0))
		{
			return;
		}
		if (!settings_sabotageOff && sabotage_pr > 0 && i > 0)
		{
			i /= 2;
		}
		if (genre_ != -1)
		{
			genres_.genres_FANS[genre_] += i;
			if (genres_.genres_FANS[genre_] < 0)
			{
				genres_.genres_FANS[genre_] = 0;
			}
		}
		else if (i < 0)
		{
			int num = genres_.GetAmountFans();
			i = Mathf.Abs(i);
			while (i > 0)
			{
				for (int j = 0; j < genres_.genres_FANS.Length; j++)
				{
					if (genres_.genres_FANS[j] > 0 && i > 0)
					{
						genres_.genres_FANS[j]--;
						i--;
						num--;
					}
				}
				if (i <= 0 || num <= 0)
				{
					break;
				}
			}
			for (int k = 0; k < genres_.genres_FANS.Length; k++)
			{
				if (genres_.genres_FANS[k] < 0)
				{
					genres_.genres_FANS[k] = 0;
				}
			}
		}
		else
		{
			while (i > 0 && i > 0)
			{
				int num2 = UnityEngine.Random.Range(0, genres_.genres_FANS.Length);
				if (genres_.genres_UNLOCK[num2])
				{
					if (i >= 10)
					{
						genres_.genres_FANS[num2] += 10;
						i -= 10;
					}
					else
					{
						genres_.genres_FANS[num2]++;
						i--;
					}
				}
			}
		}
		for (int l = 0; l < genres_.genres_FANS.Length; l++)
		{
			if (genres_.genres_FANS[l] > 20000000)
			{
				genres_.genres_FANS[l] = 20000000;
			}
		}
		if (genres_.GetAmountFans() >= 1000000)
		{
			achScript_.SetAchivement(54);
		}
	}

	public void UpdatePathfindingForAll()
	{
		for (int i = 0; i < arrayCharactersScripts.Length; i++)
		{
			if ((bool)arrayCharactersScripts[i] && !arrayCharactersScripts[i].iDoWork && (bool)arrayCharactersScripts[i].moveS_)
			{
				arrayCharactersScripts[i].moveS_.RecalculatePath();
			}
		}
		for (int j = 0; j < arrayRobotsScripts.Length; j++)
		{
			if ((bool)arrayRobotsScripts[j])
			{
				arrayRobotsScripts[j].RecalculatePath();
			}
		}
	}

	public void UpdatePathfindingNextFrameExtern()
	{
		StartCoroutine(UpdatePathfindingNextFrame());
	}

	public IEnumerator UpdatePathfindingNextFrame()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		Debug.Log("mainScript -> UpdatePathfindingNextFrame()");
		if ((bool)mapScript_.aStar_)
		{
			mapScript_.aStar_.Scan();
		}
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		UpdatePathfindingForAll();
	}

	public void DisableSelectLayer(GameObject go)
	{
		StartCoroutine(iDisableSelectLayer(go));
	}

	private IEnumerator iDisableSelectLayer(GameObject go)
	{
		yield return new WaitForEndOfFrame();
		if ((bool)go && (bool)go.transform.GetChild(0))
		{
			SetLayer(0, go.transform.GetChild(0));
		}
	}

	public void SetLayer(int newLayer, Transform trans)
	{
		trans.gameObject.layer = newLayer;
		foreach (Transform tran in trans)
		{
			tran.gameObject.layer = newLayer;
			if (tran.childCount > 0)
			{
				SetLayer(newLayer, tran.transform);
			}
		}
	}

	public void RemovePickedChar(GameObject go)
	{
		for (int i = 0; i < pickedChars.Count; i++)
		{
			if (pickedChars[i] == go)
			{
				pickedChars.RemoveAt(i);
				break;
			}
		}
	}

	public void AddPickedChar(GameObject go)
	{
		pickedChars.Add(go);
	}

	public void SetGameSpeed(float f)
	{
		gameSpeed = f;
	}

	public float GetGameSpeed()
	{
		float result = gameSpeed;
		if (multiplayer && settings_autoPauseForMultiplayer && mpCalls_.AutoPause())
		{
			result = 0f;
		}
		return result;
	}

	public float GetDeltaTime()
	{
		return Time.deltaTime * GetGameSpeed();
	}

	public string RoundString(float value, int digits)
	{
		float num = Mathf.Pow(10f, digits);
		return (Mathf.Round(value * num) / num).ToString("#0.0");
	}

	public float Round(float value, int digits)
	{
		float num = Mathf.Pow(10f, digits);
		return Mathf.Round(value * num) / num;
	}

	public void SortChildrenByName(GameObject obj)
	{
		List<Transform> list = new List<Transform>();
		for (int num = obj.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = obj.transform.GetChild(num);
			list.Add(child);
		}
		list.Sort((Transform t1, Transform t2) => t1.name.CompareTo(t2.name));
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			list[num2].SetSiblingIndex(num2);
		}
	}

	public void SortChildrenByNameReverse(GameObject obj)
	{
		List<Transform> list = new List<Transform>();
		for (int num = obj.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = obj.transform.GetChild(num);
			list.Add(child);
		}
		list.Sort((Transform t1, Transform t2) => t2.name.CompareTo(t1.name));
		for (int num2 = 0; num2 < list.Count; num2++)
		{
			list[num2].SetSiblingIndex(num2);
		}
	}

	private float GetFloatMax(string stringValue)
	{
		float result = 1f;
		float.TryParse(stringValue, out result);
		return 0f - result;
	}

	private float GetFloatMin(string stringValue)
	{
		float result = 1f;
		float.TryParse(stringValue, out result);
		return result;
	}

	public void SortChildrenByFloat(GameObject obj)
	{
		List<Transform> list = new List<Transform>();
		for (int num = obj.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = obj.transform.GetChild(num);
			list.Add(child);
		}
		list.Sort(CompareFloat);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetSiblingIndex(i);
		}
	}

	private int CompareFloat(Transform t1, Transform t2)
	{
		int num = GetFloatMax(t1.name).CompareTo(GetFloatMax(t2.name));
		if (num == 0)
		{
			if (t1.GetInstanceID() > t2.GetInstanceID())
			{
				return -1;
			}
			return 1;
		}
		return num;
	}

	public void SortChildrenByFloatReverse(GameObject obj)
	{
		List<Transform> list = new List<Transform>();
		for (int num = obj.transform.childCount - 1; num >= 0; num--)
		{
			Transform child = obj.transform.GetChild(num);
			list.Add(child);
		}
		list.Sort(CompareFloatReverse);
		for (int i = 0; i < list.Count; i++)
		{
			list[i].SetSiblingIndex(i);
		}
	}

	private int CompareFloatReverse(Transform t1, Transform t2)
	{
		int num = GetFloatMax(t2.name).CompareTo(GetFloatMax(t1.name));
		if (num == 0)
		{
			if (t1.GetInstanceID() > t2.GetInstanceID())
			{
				return -1;
			}
			return 1;
		}
		return num;
	}

	public int GetNewID()
	{
		return UnityEngine.Random.Range(1, 2000000000);
	}

	private void UpdateSpecialMaterials()
	{
		float x = Mathf.Cos(Time.time * GetGameSpeed()) * 0.5f + 1f;
		float y = Mathf.Sin(Time.time * GetGameSpeed()) * 0.5f + 1f;
		specialMaterials[0].mainTextureScale = new Vector2(x, y);
		specialMaterials[1].mainTextureOffset = new Vector2(Time.time * (GetGameSpeed() * 0.3f), 0f);
		specialMaterials[2].mainTextureOffset = new Vector2(0f, (0f - Time.time) * (GetGameSpeed() * 0.26f));
		specialMaterials[3].mainTextureOffset = new Vector2(0f, (0f - Time.time) * (GetGameSpeed() * 1.5f));
		specialMaterials[8].mainTextureOffset = new Vector2(0f, (0f - Time.time) * (GetGameSpeed() * 1.5f));
	}

	private void ResetGenreBeliebtheit()
	{
		for (int i = 0; i < genres_.genres_BELIEBTHEIT.Length; i++)
		{
			if (genres_.genres_BELIEBTHEIT[i] <= 0f)
			{
				genres_.genres_BELIEBTHEIT[i] = UnityEngine.Random.Range(40, 80);
			}
			if (UnityEngine.Random.Range(0, 100) > 50)
			{
				genres_.genres_BELIEBTHEIT_SOLL[i] = true;
			}
			else
			{
				genres_.genres_BELIEBTHEIT_SOLL[i] = false;
			}
		}
		int num = 0;
		int num2 = 0;
		for (int j = 0; j < genres_.genres_BELIEBTHEIT.Length; j++)
		{
			if (j != trendAntiGenre && j != trendGenre && genres_.genres_UNLOCK[j])
			{
				if (genres_.genres_BELIEBTHEIT_SOLL[j])
				{
					num++;
				}
				else
				{
					num2++;
				}
			}
		}
		if (num <= 0)
		{
			int num3;
			do
			{
				num3 = UnityEngine.Random.Range(0, genres_.genres_BELIEBTHEIT.Length);
			}
			while (num3 == trendAntiGenre || num3 == trendGenre || !genres_.genres_UNLOCK[num3]);
			genres_.genres_BELIEBTHEIT_SOLL[num3] = true;
		}
	}

	private void UpdateGenreBeliebtheit()
	{
		if (multiplayer && mpCalls_.isClient)
		{
			return;
		}
		for (int i = 0; i < genres_.genres_BELIEBTHEIT.Length; i++)
		{
			if (genres_.genres_BELIEBTHEIT_SOLL[i])
			{
				genres_.genres_BELIEBTHEIT[i] += 0.3f;
				if (genres_.genres_BELIEBTHEIT[i] > 80f)
				{
					genres_.genres_BELIEBTHEIT[i] = 80f;
				}
			}
			else
			{
				genres_.genres_BELIEBTHEIT[i] -= 0.3f;
				if (genres_.genres_BELIEBTHEIT[i] < 40f)
				{
					genres_.genres_BELIEBTHEIT[i] = 40f;
				}
			}
		}
		if (multiplayer && mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_GenreBeliebtheit();
		}
	}

	private void UpdateTrend(bool newGame)
	{
		trendWeeks--;
		if ((multiplayer && mpCalls_.isClient) || trendWeeks >= 0)
		{
			return;
		}
		trendWeeks = UnityEngine.Random.Range(50, 100);
		ResetGenreBeliebtheit();
		UpdateGenreBeliebtheit();
		if (UnityEngine.Random.Range(0, 100) < 20 || trendNextTheme == -1)
		{
			trendTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
			trendAntiTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
			trendNextTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
			trendNextAntiTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
		}
		else
		{
			trendTheme = trendNextTheme;
			trendAntiTheme = trendNextAntiTheme;
			trendNextTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
			trendNextAntiTheme = UnityEngine.Random.Range(0, themes_.themes_LEVEL.Length);
		}
		if (trendAntiTheme == trendTheme)
		{
			if (trendAntiTheme > 0)
			{
				trendAntiTheme--;
			}
			else
			{
				trendAntiTheme++;
			}
		}
		if (trendNextAntiTheme == trendNextTheme)
		{
			if (trendNextAntiTheme > 0)
			{
				trendNextAntiTheme--;
			}
			else
			{
				trendNextAntiTheme++;
			}
		}
		int num = 0;
		bool flag = false;
		if (UnityEngine.Random.Range(0, 100) < 20 || trendNextGenre == -1)
		{
			while (!flag)
			{
				trendGenre = UnityEngine.Random.Range(0, genres_.genres_LEVEL.Length);
				if (genres_.genres_UNLOCK[trendGenre])
				{
					flag = true;
				}
				num++;
				if (num > 10000)
				{
					flag = true;
				}
			}
		}
		else
		{
			trendGenre = trendNextGenre;
		}
		num = 0;
		flag = false;
		while (!flag)
		{
			trendNextGenre = UnityEngine.Random.Range(0, genres_.genres_LEVEL.Length);
			if (genres_.genres_UNLOCK[trendNextGenre])
			{
				flag = true;
			}
			num++;
			if (num > 10000)
			{
				flag = true;
			}
		}
		num = 0;
		flag = false;
		if (UnityEngine.Random.Range(0, 100) < 20 || trendNextAntiGenre == -1)
		{
			int num2 = 0;
			if (newGame && !multiplayer)
			{
				for (int i = 0; i < genres_.genres_RES_POINTS_LEFT.Length; i++)
				{
					if (genres_.genres_RES_POINTS_LEFT[i] <= 0f)
					{
						num2 = i;
						break;
					}
				}
			}
			while (!flag)
			{
				trendAntiGenre = UnityEngine.Random.Range(0, genres_.genres_LEVEL.Length);
				if (genres_.genres_UNLOCK[trendAntiGenre] && trendAntiGenre != trendGenre)
				{
					flag = true;
				}
				if (newGame && !multiplayer && trendAntiGenre == num2)
				{
					Debug.Log("Nicht das eigene Fangenre wählen!");
					flag = false;
				}
				num++;
				if (num > 10000)
				{
					flag = true;
				}
			}
		}
		else
		{
			trendAntiGenre = trendNextAntiGenre;
		}
		num = 0;
		flag = false;
		while (!flag)
		{
			trendNextAntiGenre = UnityEngine.Random.Range(0, genres_.genres_LEVEL.Length);
			if (genres_.genres_UNLOCK[trendNextAntiGenre] && trendNextAntiGenre != trendNextGenre)
			{
				flag = true;
			}
			num++;
			if (num > 10000)
			{
				flag = true;
			}
		}
		if (trendGenre == trendAntiGenre)
		{
			for (int j = 0; j < genres_.genres_LEVEL.Length; j++)
			{
				if (genres_.genres_UNLOCK[j] && trendGenre != j)
				{
					trendAntiGenre = j;
					break;
				}
			}
		}
		if (trendTheme == trendNextAntiTheme)
		{
			for (int k = 0; k < themes_.themes_LEVEL.Length; k++)
			{
				if (trendTheme != k)
				{
					trendNextAntiTheme = k;
					break;
				}
			}
		}
		ShowTrendNews();
		if (multiplayer && mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Trend();
		}
	}

	public void ShowTrendNews()
	{
		if (year != 1976 || month != 1)
		{
			guiMain_.CreateTopNewsTrend(genres_.GetName(trendGenre) + " / " + tS_.GetThemes(trendTheme), genres_.GetPic(trendGenre));
		}
	}

	public int PassedMonth()
	{
		return (year - 1976) * 12 + month;
	}

	public void DrawFilter(int mode, bool force)
	{
		if (!force)
		{
			filterTimer += Time.deltaTime;
			if (filterTimer < 1f)
			{
				return;
			}
			filterTimer = 0f;
		}
		Color32 color = new Color32(byte.MaxValue, byte.MaxValue, byte.MaxValue, 0);
		Color32[] pixels = specialTextures[0].GetPixels32();
		for (int i = 0; i < pixels.Length; i++)
		{
			pixels[i] = color;
		}
		specialTextures[0].SetPixels32(pixels);
		switch (mode)
		{
		case 0:
		{
			for (int l = 0; l < mapScript.mapSizeX; l++)
			{
				for (int m = 0; m < mapScript.mapSizeY; m++)
				{
					float num3 = mapScript_.mapAusstattung[l, m];
					if (num3 > 1f)
					{
						num3 = 1f;
					}
					specialTextures[0].SetPixel(l, m, new Color(0f, 1f, 0f, num3));
				}
			}
			break;
		}
		case 1:
		{
			for (int n = 0; n < mapScript.mapSizeX; n++)
			{
				for (int num4 = 0; num4 < mapScript.mapSizeY; num4++)
				{
					float num5 = mapScript_.mapMuell[n, num4];
					if (num5 > 1f)
					{
						num5 = 1f;
					}
					specialTextures[0].SetPixel(n, num4, new Color(1f, 0f, 0f, num5));
				}
			}
			break;
		}
		case 2:
		{
			for (int j = 0; j < mapScript.mapSizeX; j++)
			{
				for (int k = 0; k < mapScript.mapSizeY; k++)
				{
					float num = mapScript_.mapWaerme[j, k];
					if (num > 1f)
					{
						num = 1f;
					}
					int num2 = 0;
					if ((bool)mapScript_.mapRoomScript[j, k])
					{
						num2 = mapScript_.mapRoomScript[j, k].typ;
					}
					if (num2 != 15)
					{
						specialTextures[0].SetPixel(j, k, new Color(0f, 1f, 0f, num));
					}
					else
					{
						specialTextures[0].SetPixel(j, k, new Color(1f, 0f, 0f, num));
					}
				}
			}
			break;
		}
		}
		specialTextures[0].Apply();
	}

	public void SetAllFloorTextures(int mode)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Floor");
		if (array.Length == 0)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			floorScript component = array[i].GetComponent<floorScript>();
			if ((bool)component && IsMyBuilding(mapScript_.mapBuilding[Mathf.RoundToInt(array[i].transform.position.x), Mathf.RoundToInt(array[i].transform.position.z)]))
			{
				if (mode == 0)
				{
					component.SetStandardTexture();
				}
				if (mode == 1)
				{
					component.SetFilterTexture();
				}
			}
		}
	}

	public void SetBuildGrid(bool b)
	{
		if (!b)
		{
			floorMaterials[2].EnableKeyword("_DETAIL_MULX2");
			floorMaterials[2].SetTexture("_DetailAlbedoMap", null);
		}
		else
		{
			floorMaterials[2].EnableKeyword("_DETAIL_MULX2");
			floorMaterials[2].SetTexture("_DetailAlbedoMap", specialTextures[1]);
		}
	}

	public void AddFanshopverlauf(long i)
	{
		fanshopverlauf[0] += i;
	}

	public void AddVerkaufsverlaufKonsolen(long i)
	{
		verkaufsverlaufKonsolen[0] += i;
	}

	public void AddVerkaufsverlauf(long i)
	{
		verkaufsverlauf[0] += i;
	}

	public void AddDownloadverlauf(long i)
	{
		downloadverlauf[0] += i;
	}

	public void AddFanverlauf(long i)
	{
		fansverlauf[0] = i;
	}

	public void AddAboverlauf(long i)
	{
		aboverlauf[0] = i;
	}

	public void AddFinanzverlauf(long i)
	{
		finanzVerlauf[0] = i;
	}

	private void UpdateStatsVerlaeufe()
	{
		for (int num = fanshopverlauf.Length - 1; num > 0; num--)
		{
			fanshopverlauf[num] = fanshopverlauf[num - 1];
		}
		fanshopverlauf[0] = 0L;
		for (int num2 = finanzVerlauf.Length - 1; num2 > 0; num2--)
		{
			finanzVerlauf[num2] = finanzVerlauf[num2 - 1];
		}
		finanzVerlauf[0] = 0L;
		for (int num3 = verkaufsverlauf.Length - 1; num3 > 0; num3--)
		{
			verkaufsverlauf[num3] = verkaufsverlauf[num3 - 1];
		}
		verkaufsverlauf[0] = 0L;
		for (int num4 = verkaufsverlaufKonsolen.Length - 1; num4 > 0; num4--)
		{
			verkaufsverlaufKonsolen[num4] = verkaufsverlaufKonsolen[num4 - 1];
		}
		verkaufsverlaufKonsolen[0] = 0L;
		for (int num5 = aboverlauf.Length - 1; num5 > 0; num5--)
		{
			aboverlauf[num5] = aboverlauf[num5 - 1];
		}
		aboverlauf[0] = 0L;
		for (int num6 = downloadverlauf.Length - 1; num6 > 0; num6--)
		{
			downloadverlauf[num6] = downloadverlauf[num6 - 1];
		}
		downloadverlauf[0] = 0L;
		for (int num7 = fansverlauf.Length - 1; num7 > 0; num7--)
		{
			fansverlauf[num7] = fansverlauf[num7 - 1];
		}
		fansverlauf[0] = 0L;
		for (int num8 = gpS_.gamePass_aboVerlaufMonat.Length - 1; num8 > 0; num8--)
		{
			gpS_.gamePass_aboVerlaufMonat[num8] = gpS_.gamePass_aboVerlaufMonat[num8 - 1];
		}
		gpS_.gamePass_aboVerlaufMonat[0] = gpS_.gamePass_Abos;
		for (int num9 = gpS_.gamePass_umsatzVerlaufMonat.Length - 1; num9 > 0; num9--)
		{
			gpS_.gamePass_umsatzVerlaufMonat[num9] = gpS_.gamePass_umsatzVerlaufMonat[num9 - 1];
		}
		gpS_.gamePass_umsatzVerlaufMonat[0] = gpS_.gamePass_Abos * gpS_.gamePass_AboPreis;
	}

	public long GetKreditlimit()
	{
		long result = 250000 * (year - 1975);
		switch (difficulty)
		{
		case 0:
			result = 300000 * (year - 1975);
			result += 30000 * studioPoints;
			result += 30000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		case 1:
			result = 250000 * (year - 1975);
			result += 25000 * studioPoints;
			result += 25000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		case 2:
			result = 200000 * (year - 1975);
			result += 20000 * studioPoints;
			result += 20000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		case 3:
			result = 150000 * (year - 1975);
			result += 15000 * studioPoints;
			result += 15000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		case 4:
			result = 100000 * (year - 1975);
			result += 10000 * studioPoints;
			result += 10000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		case 5:
			result = 50000 * (year - 1975);
			result += 5000 * studioPoints;
			result += 5000 * Mathf.RoundToInt(auftragsAnsehen);
			break;
		}
		return result;
	}

	public long GetKredit()
	{
		return kredit;
	}

	public int GetKreditZinsen()
	{
		if (kredit <= 0)
		{
			return 0;
		}
		int result = 0;
		switch (difficulty)
		{
		case 0:
			result = Mathf.RoundToInt(kredit / 140);
			break;
		case 1:
			result = Mathf.RoundToInt(kredit / 120);
			break;
		case 2:
			result = Mathf.RoundToInt(kredit / 100);
			break;
		case 3:
			result = Mathf.RoundToInt(kredit / 80);
			break;
		case 4:
			result = Mathf.RoundToInt(kredit / 60);
			break;
		case 5:
			result = Mathf.RoundToInt(kredit / 40);
			break;
		}
		return result;
	}

	public void CreateFoto(characterScript cSPhoto_, charArbeitsmarkt aSPhoto_)
	{
		if (!cameraPersonalPhoto.activeSelf)
		{
			cameraPersonalPhoto.SetActive(value: true);
		}
		if ((bool)charFoto)
		{
			UnityEngine.Object.Destroy(charFoto);
		}
		characterScript characterScript2 = null;
		characterScript2 = ((!cSPhoto_) ? CreatePlayer(aSPhoto_.male, aSPhoto_.model_body, aSPhoto_.model_eyes, aSPhoto_.model_hair, aSPhoto_.model_beard, aSPhoto_.model_skinColor, aSPhoto_.model_hairColor, aSPhoto_.model_beardColor, aSPhoto_.model_HoseColor, aSPhoto_.model_ShirtColor, aSPhoto_.model_Add1Color) : CreatePlayer(cSPhoto_.male, cSPhoto_.model_body, cSPhoto_.model_eyes, cSPhoto_.model_hair, cSPhoto_.model_beard, cSPhoto_.model_skinColor, cSPhoto_.model_hairColor, cSPhoto_.model_beardColor, cSPhoto_.model_HoseColor, cSPhoto_.model_ShirtColor, cSPhoto_.model_Add1Color));
		charFoto = characterScript2.gameObject.transform.GetChild(0).gameObject;
		charFoto.name = "CHARFOTO";
		charFoto.transform.SetParent(null);
		charFoto.transform.position = new Vector3(0f, 0f, 0f);
		charFoto.transform.eulerAngles = new Vector3(0f, 0f, 0f);
		SetLayer(4, charFoto.transform);
		charFoto.GetComponent<Animator>().CrossFade("idle", 0.1f, 0, 0f, 0.4f);
		UnityEngine.Object.Destroy(characterScript2.gameObject);
		StartCoroutine(RemovePhoto());
	}

	private IEnumerator RemovePhoto()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		if (cameraPersonalPhoto.activeSelf)
		{
			cameraPersonalPhoto.SetActive(value: false);
		}
		if ((bool)charFoto)
		{
			UnityEngine.Object.Destroy(charFoto);
		}
	}

	public void DestroyMainMenuObjects()
	{
		StartCoroutine(DestroyMainMenuObjectsAfterOneFrame());
	}

	private IEnumerator DestroyMainMenuObjectsAfterOneFrame()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < mainMenuObjects.Length; i++)
		{
			if ((bool)mainMenuObjects[i])
			{
				UnityEngine.Object.Destroy(mainMenuObjects[i]);
			}
		}
	}

	private void UpdateExklusivPublisher()
	{
		if (exklusivVertrag_ID == -1)
		{
			return;
		}
		exklusivVertrag_laufzeit--;
		if (exklusivVertrag_laufzeit < 0)
		{
			publisherScript exklusivPublisher = GetExklusivPublisher();
			if ((bool)exklusivPublisher)
			{
				string text = tS_.GetText(1053);
				text = text.Replace("<NAME>", "<color=blue>" + exklusivPublisher.GetName() + "</color>");
				guiMain_.OpenMenu(hideChars: false);
				guiMain_.MessageBox(text, closeMenu: true);
			}
			RemovePublisherExklusivVertrag();
		}
	}

	public void RemovePublisherExklusivVertrag()
	{
		exklusivVertrag_ID = -1;
		exklusivVertrag_laufzeit = 0;
		exkklusivVertragScript_ = null;
	}

	public publisherScript GetExklusivPublisher()
	{
		if (exklusivVertrag_ID == -1)
		{
			return null;
		}
		if (!exkklusivVertragScript_)
		{
			for (int i = 0; i < arrayPublisherScripts.Length; i++)
			{
				if ((bool)arrayPublisherScripts[i] && arrayPublisherScripts[i].myID == exklusivVertrag_ID)
				{
					exkklusivVertragScript_ = arrayPublisherScripts[i];
					break;
				}
			}
		}
		if ((bool)exkklusivVertragScript_)
		{
			return exkklusivVertragScript_;
		}
		return null;
	}

	public bool IsMyBuilding(int id_)
	{
		return buildings[id_];
	}

	public void UpdateGlobalEvent()
	{
		if (settings_randomEvents == 2 || year <= 1976)
		{
			return;
		}
		if (globalEvent == -1)
		{
			if ((multiplayer && (!multiplayer || !mpCalls_.isServer)) || guiMain_.menuOpen)
			{
				return;
			}
			if (settings_randomEvents == 0)
			{
				if (UnityEngine.Random.Range(0, 100) <= 1)
				{
					guiMain_.uiObjects[216].SetActive(value: true);
					guiMain_.uiObjects[216].GetComponent<Menu_RandomEventGlobal>().Init(-1);
					if (multiplayer && mpCalls_.isServer)
					{
						mpCalls_.SERVER_Send_GlobalEvent(globalEvent, globalEventWeeks);
						guiMain_.BUTTON_GameSpeed(0f);
					}
				}
			}
			else
			{
				guiMain_.uiObjects[216].SetActive(value: true);
				guiMain_.uiObjects[216].GetComponent<Menu_RandomEventGlobal>().History();
				if (globalEvent != -1 && multiplayer && mpCalls_.isServer)
				{
					mpCalls_.SERVER_Send_GlobalEvent(globalEvent, globalEventWeeks);
					guiMain_.BUTTON_GameSpeed(0f);
				}
			}
		}
		else
		{
			globalEventWeeks--;
			if (globalEventWeeks <= 0)
			{
				globalEvent = -1;
				globalEventWeeks = 0;
			}
		}
	}

	public void SetGlobalEvent(int i)
	{
		globalEvent = i;
		globalEventWeeks = UnityEngine.Random.Range(16, 32);
	}

	private void PayBankZinsen()
	{
		if (globalEvent == 4)
		{
			Pay(GetKreditZinsen() * 3, 20);
		}
		else
		{
			Pay(GetKreditZinsen(), 20);
		}
	}

	public void NewMarktforschung()
	{
		marktforschung_datum = guiMain_.GetDate();
		if (unlock_.Get(59))
		{
			marktforschung_digtal = games_.GetDigitalSells();
			marktforschung_retail = 1f - marktforschung_digtal;
		}
		else
		{
			marktforschung_digtal = 0f;
			marktforschung_retail = 1f;
		}
		marktforschung_deluxe = games_.GetDeluxeCurve();
		marktforschung_collectors = games_.GetCollectorsCurve();
		marktforschung_arcade = games_.GetArcadeCurve();
		if (year >= 1991)
		{
			marktforschung_internet = games_.GetInternetUser();
		}
		else
		{
			marktforschung_internet = 0f;
		}
		if (unlock_.Get(68))
		{
			marktforschung_gamePass = games_.GetGamePassInteressted();
		}
		else
		{
			marktforschung_gamePass = 0f;
		}
		marktforschung_bestPlattform = -1;
		marktforschung_bestPlattformKonsole = -1;
		marktforschung_bestPlattformHandheld = -1;
		marktforschung_bestPlattformHandy = -1;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			platformScript component = array[i].GetComponent<platformScript>();
			if (!component.IsVerfuegbar())
			{
				continue;
			}
			if (component.OwnerIsNPC())
			{
				if (component.typ == 0 && num < component.units_max)
				{
					num = component.units_max;
					marktforschung_bestPlattform = component.myID;
				}
				if (component.typ == 1 && num2 < component.units_max)
				{
					num2 = component.units_max;
					marktforschung_bestPlattformKonsole = component.myID;
				}
				if (component.typ == 2 && num3 < component.units_max)
				{
					num3 = component.units_max;
					marktforschung_bestPlattformHandheld = component.myID;
				}
				if (component.typ == 3 && num4 < component.units_max)
				{
					num4 = component.units_max;
					marktforschung_bestPlattformHandy = component.myID;
				}
			}
			else
			{
				if (component.typ == 0 && num < component.units)
				{
					num = component.units;
					marktforschung_bestPlattform = component.myID;
				}
				if (component.typ == 1 && num2 < component.units)
				{
					num2 = component.units;
					marktforschung_bestPlattformKonsole = component.myID;
				}
				if (component.typ == 2 && num3 < component.units)
				{
					num3 = component.units;
					marktforschung_bestPlattformHandheld = component.myID;
				}
				if (component.typ == 3 && num4 < component.units)
				{
					num4 = component.units;
					marktforschung_bestPlattformHandy = component.myID;
				}
			}
		}
		marktforschung_badPlattform = -1;
		marktforschung_badPlattformKonsole = -1;
		marktforschung_badPlattformHandheld = -1;
		marktforschung_badPlattformHandy = -1;
		num = -1;
		num2 = -1;
		num3 = -1;
		num4 = -1;
		for (int j = 0; j < array.Length; j++)
		{
			if (!array[j])
			{
				continue;
			}
			platformScript component2 = array[j].GetComponent<platformScript>();
			if (component2.OwnerIsNPC() && component2.IsVerfuegbar())
			{
				if (component2.typ == 0 && (num > component2.units_max || num == -1))
				{
					num = component2.units_max;
					marktforschung_badPlattform = component2.myID;
				}
				if (component2.typ == 1 && (num2 > component2.units_max || num2 == -1))
				{
					num2 = component2.units_max;
					marktforschung_badPlattformKonsole = component2.myID;
				}
				if (component2.typ == 2 && (num3 > component2.units_max || num3 == -1))
				{
					num3 = component2.units_max;
					marktforschung_badPlattformHandheld = component2.myID;
				}
				if (component2.typ == 3 && (num4 > component2.units_max || num4 == -1))
				{
					num4 = component2.units_max;
					marktforschung_badPlattformHandy = component2.myID;
				}
			}
		}
		marktforschung_nextGenre = trendNextGenre;
		marktforschung_nextTopic = trendNextTheme;
		marktforschung_nextBadGenre = trendNextAntiGenre;
		marktforschung_nextBadTopic = trendNextAntiTheme;
	}

	private void UpdateCars()
	{
		for (int i = 0; i < carList.Count; i++)
		{
			if ((bool)carList[i])
			{
				carList[i].transform.Translate(Vector3.forward * GetDeltaTime() * 20f);
				if (carList[i].transform.position.x < -300f || carList[i].transform.position.x > 300f || carList[i].transform.position.z < -300f || carList[i].transform.position.z > 300f)
				{
					UnityEngine.Object.Destroy(carList[i]);
				}
			}
		}
		for (int j = 0; j < carList.Count; j++)
		{
			if (!carList[j])
			{
				carList.RemoveAt(j);
			}
		}
		carSpawnTimer += GetDeltaTime();
		if (carSpawnTimer < 2f)
		{
			return;
		}
		carSpawnTimer = 0f;
		if (carSpawns.Length == 0)
		{
			carSpawns = GameObject.FindGameObjectsWithTag("CarSpawn");
		}
		if (carSpawns.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, carSpawns.Length);
			if ((bool)carSpawns[num] && UnityEngine.Random.Range(0, 100) > 50)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate(carPrefabs[UnityEngine.Random.Range(0, carPrefabs.Length)]);
				gameObject.transform.position = new Vector3(carSpawns[num].transform.position.x, -0.54f, carSpawns[num].transform.position.z);
				gameObject.transform.eulerAngles = carSpawns[num].transform.eulerAngles;
				gameObject.transform.Translate(-Vector3.forward * UnityEngine.Random.Range(0f, 1f));
				carList.Add(gameObject);
			}
		}
	}

	public string GetSavegameTitle()
	{
		if (!multiplayer)
		{
			return "savegame";
		}
		return "mp";
	}

	private void RenameOldSaves()
	{
		for (int i = 0; i < 21; i++)
		{
			if (ES3.FileExists("savegame" + i + ".txt"))
			{
				ES3.RenameFile("savegame" + i + ".txt", "savegame" + i + ".sav");
			}
		}
	}

	public void SetRandomMultiplayerSaveID()
	{
		multiplayerSaveID = UnityEngine.Random.Range(100, 9999999);
	}

	public void SendSystemMessage(string c)
	{
		if (multiplayer && (bool)mpCalls_)
		{
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Chat(myID, c);
			}
			else
			{
				mpCalls_.CLIENT_Send_Chat(c);
			}
		}
	}

	public void UpdateWeatherEffects()
	{
		if ((bool)globalLight)
		{
			if (weatherEffects[0].activeSelf)
			{
				globalLight.color = Color.Lerp(globalLight.color, globalLightColors[1], 0.01f);
			}
			else
			{
				globalLight.color = Color.Lerp(globalLight.color, globalLightColors[0], 0.01f);
			}
		}
		if (settings_.disableWetter)
		{
			for (int i = 0; i < weatherEffects.Length; i++)
			{
				if ((bool)weatherEffects[i] && weatherEffects[i].activeSelf)
				{
					weatherEffects[i].SetActive(value: false);
				}
			}
			return;
		}
		if (weatherTimer > 0f)
		{
			weatherTimer -= GetDeltaTime();
			return;
		}
		for (int j = 0; j < weatherEffects.Length; j++)
		{
			if ((bool)weatherEffects[j] && weatherEffects[j].activeSelf)
			{
				weatherEffects[j].SetActive(value: false);
				weatherTimer = UnityEngine.Random.Range(20f, 40f);
				return;
			}
		}
		if (UnityEngine.Random.Range(0, 5) == 1)
		{
			weatherEffects[0].SetActive(value: true);
			weatherTimer = UnityEngine.Random.Range(20f, 40f);
		}
		else if (office != 4 && UnityEngine.Random.Range(0, 3) == 1 && (month == 11 || month == 12 || month == 1 || month == 2))
		{
			weatherEffects[1].SetActive(value: true);
			weatherTimer = UnityEngine.Random.Range(20f, 40f);
		}
		else
		{
			weatherTimer = UnityEngine.Random.Range(20f, 40f);
		}
	}

	private void UpdateBankWarning()
	{
		if (money < 0)
		{
			bankWarning++;
			if (bankWarning >= 19)
			{
				bankWarning = 18;
				if (!multiplayer)
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[251]);
				}
				else
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[412]);
				}
				guiMain_.OpenMenu(hideChars: false);
				sfx_.PlaySound(57, force: false);
			}
		}
		else
		{
			bankWarning = 0;
		}
		if (bankWarning == 6)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[249]);
			guiMain_.OpenMenu(hideChars: false);
			sfx_.PlaySound(57, force: false);
		}
	}

	public void CloseMultiplayerView()
	{
		GameObject.Find("ROOMS").transform.position = new Vector3(0f, 0f, 0f);
		guiMain_.uiObjects[164].SetActive(value: true);
		for (int i = 0; i < mapScript_.ROOMS_MP.transform.childCount; i++)
		{
			Transform child = mapScript_.ROOMS_MP.transform.GetChild(i);
			if ((bool)child)
			{
				UnityEngine.Object.Destroy(child.gameObject);
			}
		}
		for (int j = 0; j < arrayCharacters.Length; j++)
		{
			if ((bool)arrayCharacters[j])
			{
				arrayCharacters[j].transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		for (int k = 0; k < arrayObjects.Length; k++)
		{
			if ((bool)arrayObjects[k])
			{
				arrayObjects[k].transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		for (int l = 0; l < arrayRobots.Length; l++)
		{
			if ((bool)arrayRobots[l])
			{
				arrayRobots[l].transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
		for (int m = 0; m < arrayMuell.Length; m++)
		{
			if ((bool)arrayMuell[m])
			{
				arrayMuell[m].transform.localScale = new Vector3(1f, 1f, 1f);
			}
		}
	}

	public void ShowMultiplayerView(int slot)
	{
		if (slot > mpCalls_.playersMP.Count)
		{
			return;
		}
		int playerID = mpCalls_.playersMP[slot].playerID;
		if (playerID == myID || guiMain_.uiObjects[252].activeSelf)
		{
			return;
		}
		guiMain_.OpenMenu(hideChars: true);
		guiMain_.uiObjects[252].SetActive(value: true);
		guiMain_.uiObjects[252].GetComponent<Menu_MultiplayerView>().Init(playerID);
		GameObject.Find("ROOMS").transform.position = new Vector3(0f, 1000f, 0f);
		mapScript_.CreateWalls_Multiplayer(playerID);
		guiMain_.uiObjects[164].SetActive(value: false);
		for (int i = 0; i < arrayCharacters.Length; i++)
		{
			if ((bool)arrayCharacters[i])
			{
				arrayCharacters[i].transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
		for (int j = 0; j < arrayObjects.Length; j++)
		{
			if ((bool)arrayObjects[j])
			{
				arrayObjects[j].transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
		for (int k = 0; k < arrayRobots.Length; k++)
		{
			if ((bool)arrayRobots[k])
			{
				arrayRobots[k].transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
		for (int l = 0; l < arrayMuell.Length; l++)
		{
			if ((bool)arrayMuell[l])
			{
				arrayMuell[l].transform.localScale = new Vector3(0f, 0f, 0f);
			}
		}
		player_mp player_mp2 = mpCalls_.FindPlayer(playerID);
		if (player_mp2 != null)
		{
			for (int m = 0; m < player_mp2.objects.Count; m++)
			{
				CreateMultiplayerObject(player_mp2.objects[m].id, player_mp2.objects[m].typ, player_mp2.objects[m].posX, player_mp2.objects[m].posY, player_mp2.objects[m].rotation);
			}
		}
	}

	public void Multiplayer_SendMap(int x, int y)
	{
		if (multiplayer)
		{
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Map(x, y);
			}
			else
			{
				mpCalls_.SERVER_Send_Map(x, y);
			}
		}
	}

	public void Multiplayer_SendObject(int id, int typ, float posX, float posY, float rot)
	{
		if (multiplayer)
		{
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Object(id, typ, posX, posY, rot);
			}
			else
			{
				mpCalls_.SERVER_Send_Object(id, typ, posX, posY, rot);
			}
		}
	}

	public void Multiplayer_SendObjectDelete(int id)
	{
		if (multiplayer && (!save_ || !save_.loadingSavegame) && (bool)guiMain_ && !guiMain_.uiObjects[202].activeSelf && !guiMain_.uiObjects[238].activeSelf && !guiMain_.uiObjects[152].activeSelf)
		{
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_ObjectDelete(id);
			}
			else
			{
				mpCalls_.SERVER_Send_ObjectDelete(id);
			}
		}
	}

	public void CreateMultiplayerObject(int id, int typ, float posX, float posY, float rot)
	{
		GameObject obj = UnityEngine.Object.Instantiate(mapScript_.prefabsInventar[typ]);
		obj.transform.position = new Vector3(posX, 0f, posY);
		obj.transform.eulerAngles = new Vector3(0f, rot, 0f);
		objectScript component = obj.GetComponent<objectScript>();
		component.multiplayerObject = true;
		if ((bool)component.footprint)
		{
			UnityEngine.Object.Destroy(component.footprint);
		}
		obj.transform.GetChild(0).transform.parent = mapScript_.ROOMS_MP.transform;
		UnityEngine.Object.Destroy(obj);
	}

	public void AddColliderLayer(Transform go)
	{
		listColliderLayer.Add(go);
	}

	public void ResetAllColliderLayer()
	{
		for (int i = 0; i < listColliderLayer.Count; i++)
		{
			if ((bool)listColliderLayer[i])
			{
				SetLayer(0, listColliderLayer[i]);
			}
		}
		listColliderLayer.Clear();
	}

	public void AddStudioPoints(int i)
	{
		int studioLevel = GetStudioLevel(studioPoints);
		studioPoints += i;
		if (studioPoints < 0)
		{
			studioPoints = 0;
		}
		int studioLevel2 = GetStudioLevel(studioPoints);
		if ((bool)myPubS_)
		{
			myPubS_.stars = studioLevel2 * 10;
		}
		if (studioLevel != studioLevel2)
		{
			guiMain_.CreateTopNewsStudiobewertung(tS_.GetStudioBewertung(studioLevel2));
			if (multiplayer)
			{
				FindMyPublisherScript();
				if (mpCalls_.isClient)
				{
					mpCalls_.CLIENT_Send_Publisher(myPubS_);
				}
				if (mpCalls_.isServer)
				{
					mpCalls_.SERVER_Send_Publisher(myPubS_);
				}
			}
		}
		if (studioLevel2 == 10)
		{
			achScript_.SetAchivement(58);
		}
	}

	public int GetStudioLevel(int points)
	{
		int result = 0;
		if (points > 30)
		{
			result = 1;
		}
		if (points > 150)
		{
			result = 2;
		}
		if (points > 300)
		{
			result = 3;
		}
		if (points > 600)
		{
			result = 4;
		}
		if (points > 800)
		{
			result = 5;
		}
		if (points > 1000)
		{
			result = 6;
		}
		if (points > 1500)
		{
			result = 7;
		}
		if (points > 2000)
		{
			result = 8;
		}
		if (points > 3000)
		{
			result = 9;
		}
		if (points > 4000)
		{
			result = 10;
		}
		return result;
	}

	public bool Muttersprache(int i)
	{
		int countryID = GetCountryID();
		switch (i)
		{
		case 0:
			if (countryID == 0 || countryID == 2 || countryID == 13 || countryID == 7 || countryID == 40)
			{
				return true;
			}
			break;
		case 1:
			if (countryID == 5 || countryID == 14 || countryID == 16)
			{
				return true;
			}
			break;
		case 2:
			if (countryID == 6)
			{
				return true;
			}
			break;
		case 3:
			if (countryID == 10)
			{
				return true;
			}
			break;
		case 4:
			if (countryID == 9 || countryID == 22)
			{
				return true;
			}
			break;
		case 5:
			if (countryID == 11)
			{
				return true;
			}
			break;
		case 6:
			if (countryID == 12)
			{
				return true;
			}
			break;
		case 7:
			if (countryID == 8)
			{
				return true;
			}
			break;
		case 8:
			if (countryID == 3)
			{
				return true;
			}
			break;
		case 9:
			if (countryID == 4)
			{
				return true;
			}
			break;
		case 10:
			if (countryID == 1)
			{
				return true;
			}
			break;
		}
		return false;
	}

	public bool NotEnoughMoney(long wantToPay)
	{
		if (money + GetKreditlimit() < wantToPay)
		{
			return true;
		}
		return false;
	}

	public void Multiplayer_SendDataAfterGameStart()
	{
	}

	public GameObject CreateMuell(int id, int gfx, Vector3 pos)
	{
		for (int i = 0; i < arrayMuell.Length; i++)
		{
			if ((bool)arrayMuell[i] && Vector3.Distance(pos, arrayMuell[i].transform.position) < 1f)
			{
				return null;
			}
		}
		GameObject obj = UnityEngine.Object.Instantiate(miscGamePrefabs[id]);
		obj.transform.position = pos;
		obj.transform.eulerAngles = new Vector3(0f, UnityEngine.Random.Range(0f, 360f), 0f);
		muellScript component = obj.GetComponent<muellScript>();
		component.myGFXSlot = gfx;
		component.mS_ = this;
		component.main_ = base.gameObject;
		findMuell = true;
		return obj;
	}

	public int GetAchivementBonus(int id_)
	{
		return amountAchivementsBonus[id_];
	}

	public void UpdateAchivementBonus()
	{
		for (int i = 0; i < amountAchivementsBonus.Length; i++)
		{
			amountAchivementsBonus[i] = 0;
		}
		for (int j = 0; j < achivementsBonus.Length; j++)
		{
			if (!achivementsDisabled[j] && achivements[j])
			{
				amountAchivementsBonus[achivementsBonus[j]]++;
			}
		}
	}

	public int GetAmountContracts(int contractTyp_)
	{
		return contractTyp_ switch
		{
			0 => contractWorkMain_.anzProduction, 
			1 => contractWorkMain_.anzQA, 
			2 => contractWorkMain_.anzGFX, 
			3 => contractWorkMain_.anzSFX, 
			4 => contractWorkMain_.anzMotion, 
			5 => contractWorkMain_.anzWerkstatt, 
			6 => contractWorkMain_.anzHardware, 
			7 => contractWorkMain_.anzDEV, 
			_ => 0, 
		};
	}

	public void FindMyPublisherScript()
	{
		if (!myPubS_)
		{
			GameObject gameObject = GameObject.Find("PUB_" + myID);
			if ((bool)gameObject)
			{
				myPubS_ = gameObject.GetComponent<publisherScript>();
			}
		}
	}

	public string GetCompanyName()
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if ((bool)myPubS_)
		{
			return myPubS_.GetName();
		}
		return "";
	}

	public void SetCompanyName(string c)
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if (!myPubS_)
		{
			return;
		}
		myPubS_.name_EN = c;
		if (multiplayer)
		{
			FindMyPublisherScript();
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Publisher(myPubS_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(myPubS_);
			}
		}
	}

	public int GetCompanyLogoID()
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if ((bool)myPubS_)
		{
			return myPubS_.logoID;
		}
		return 0;
	}

	public void SetCompanyLogoID(int i)
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if (!myPubS_)
		{
			return;
		}
		myPubS_.logoID = i;
		if (multiplayer)
		{
			FindMyPublisherScript();
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Publisher(myPubS_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(myPubS_);
			}
		}
	}

	public int GetAwards(int i)
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if ((bool)myPubS_)
		{
			return myPubS_.awards[i];
		}
		return 0;
	}

	public void AddAwards(int i, publisherScript script_)
	{
		if (!script_)
		{
			return;
		}
		script_.awards[i]++;
		if (multiplayer)
		{
			FindMyPublisherScript();
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Publisher(script_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(script_);
			}
		}
	}

	public int GetCountryID()
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if ((bool)myPubS_)
		{
			return myPubS_.country;
		}
		return 0;
	}

	public void SetCountryID(int i)
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if (!myPubS_)
		{
			return;
		}
		myPubS_.country = i;
		if (multiplayer)
		{
			FindMyPublisherScript();
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Publisher(myPubS_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(myPubS_);
			}
		}
	}

	public int GetFanGenreID()
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if ((bool)myPubS_)
		{
			return myPubS_.fanGenre;
		}
		return 0;
	}

	public void SetFanGenreID(int i)
	{
		if (!myPubS_)
		{
			FindMyPublisherScript();
		}
		if (!myPubS_)
		{
			return;
		}
		myPubS_.fanGenre = i;
		if (multiplayer)
		{
			FindMyPublisherScript();
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Publisher(myPubS_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(myPubS_);
			}
		}
	}

	public publisherScript CreatePlayerPublisher(int id_)
	{
		FindPublishers();
		for (int i = 0; i < arrayPublisherScripts.Length; i++)
		{
			if ((bool)arrayPublisherScripts[i] && arrayPublisherScripts[i].myID == id_)
			{
				return arrayPublisherScripts[i];
			}
		}
		publisherScript obj = publisher_.CreatePublisher();
		obj.myID = id_;
		obj.Init();
		obj.isPlayer = true;
		obj.isUnlocked = true;
		obj.date_year = 1976;
		obj.date_month = 1;
		obj.stars = 1f;
		obj.developer = true;
		obj.publisher = true;
		obj.relation = 0f;
		obj.share = 1f;
		obj.notForSale = true;
		return obj;
	}

	public bool IsBetterCopyProtect()
	{
		float num = 0f;
		if (arrCopyProtect == null)
		{
			arrCopyProtect = GameObject.FindGameObjectsWithTag("CopyProtect");
		}
		for (int i = 0; i < arrCopyProtect.Length; i++)
		{
			if ((bool)arrCopyProtect[i])
			{
				copyProtectScript component = arrCopyProtect[i].GetComponent<copyProtectScript>();
				if ((bool)component && component.isUnlocked && component.inBesitz && component.effekt > num)
				{
					num = component.effekt;
				}
			}
		}
		for (int j = 0; j < arrCopyProtect.Length; j++)
		{
			if ((bool)arrCopyProtect[j])
			{
				copyProtectScript component2 = arrCopyProtect[j].GetComponent<copyProtectScript>();
				if ((bool)component2 && component2.isUnlocked && !component2.inBesitz && component2.effekt > num)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsBetterAntiCheat()
	{
		float num = 0f;
		if (arrAntiCheat == null)
		{
			arrAntiCheat = GameObject.FindGameObjectsWithTag("AntiCheat");
		}
		for (int i = 0; i < arrAntiCheat.Length; i++)
		{
			if ((bool)arrAntiCheat[i])
			{
				antiCheatScript component = arrAntiCheat[i].GetComponent<antiCheatScript>();
				if ((bool)component && component.isUnlocked && component.inBesitz && component.effekt > num)
				{
					num = component.effekt;
				}
			}
		}
		for (int j = 0; j < arrAntiCheat.Length; j++)
		{
			if ((bool)arrAntiCheat[j])
			{
				antiCheatScript component2 = arrAntiCheat[j].GetComponent<antiCheatScript>();
				if ((bool)component2 && component2.isUnlocked && !component2.inBesitz && component2.effekt > num)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetDevCostsSubvention(int gameSize_)
	{
		if (!guiMain_)
		{
			return 0;
		}
		Menu_DevGame component = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		int num = 10000;
		if (gameSize_ != 0)
		{
			return num + component.costs_gameSize[gameSize_] * (difficulty + 1);
		}
		return num + component.costs_gameSize[gameSize_];
	}

	public void Multiplayer_LockLobby()
	{
		mpLobbyOpen = false;
		manager.GetComponent<SteamLobby>().LockLobby(b: false);
	}
}
