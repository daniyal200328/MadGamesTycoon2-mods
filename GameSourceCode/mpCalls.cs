using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class mpCalls : MonoBehaviour
{
	public struct c_GameSpeed : NetworkMessage
	{
		public int speed;
	}

	public struct c_NpcGameName : NetworkMessage
	{
		public int slot;

		public bool ip;
	}

	public struct c_BlockContractGame : NetworkMessage
	{
		public int myID;

		public bool block;
	}

	public struct c_Publisher : NetworkMessage
	{
		public int myID;

		public bool isUnlocked;

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

		public float stars;

		public int logoID;

		public bool developer;

		public bool publisher;

		public bool onlyMobile;

		public float share;

		public int fanGenre;

		public long firmenwert;

		public bool notForSale;

		public int lockToBuy;

		public bool isPlayer;

		public int ownerID;

		public int country;

		public bool ownPlatform;

		public bool exklusive;

		public int[] awards;
	}

	public struct c_PublisherOwner : NetworkMessage
	{
		public int myID;

		public int ownerID;
	}

	public struct c_PublisherTochterfirmaSettings : NetworkMessage
	{
		public int myID;

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

		public bool tf_ownPublisher;

		public bool tf_publisher;

		public bool tf_developer;

		public int tf_entwicklungsdauer;

		public int tf_gameSize;

		public int tf_gameGenre;

		public int tf_gameTopic;

		public int tf_engine;

		public int tf_autoReleaseVal;

		public int tf_ownPublisherPriority;

		public int[] tf_ipFocus;

		public int[] tf_platformFocus;
	}

	public struct c_Forschung : NetworkMessage
	{
		public int playerID;

		public bool[] forschungSonstiges;

		public bool[] genres;

		public bool[] themes;

		public bool[] engineFeatures;

		public bool[] gameplayFeatures;

		public bool[] hardware;

		public bool[] hardwareFeatures;
	}

	public struct c_Help : NetworkMessage
	{
		public int playerID;

		public int toPlayerID;

		public int what;

		public int valueA;

		public int valueB;

		public int valueC;
	}

	public struct c_ObjectDelete : NetworkMessage
	{
		public int playerID;

		public int objectID;
	}

	public struct c_Object : NetworkMessage
	{
		public int playerID;

		public int objectID;

		public int typ;

		public float x;

		public float y;

		public float rot;
	}

	public struct c_Map : NetworkMessage
	{
		public int playerID;

		public byte x;

		public byte y;

		public int id;

		public int typ;

		public int door;

		public int window;
	}

	public struct c_Trend : NetworkMessage
	{
		public int trendWeeks;

		public int trendTheme;

		public int trendAntiTheme;

		public int trendGenre;

		public int trendAntiGenre;
	}

	public struct c_Payment : NetworkMessage
	{
		public int playerID;

		public int toPlayerID;

		public int what;

		public int money;

		public int objectID;
	}

	public struct c_Engine : NetworkMessage
	{
		public int myID;

		public int ownerID;

		public bool isUnlocked;

		public bool gekauft;

		public string myName;

		public bool[] features;

		public int spezialgenre;

		public int spezialplatform;

		public bool sellEngine;

		public int preis;

		public int gewinnbeteiligung;

		public float marktdominanz;
	}

	public struct c_EnginePublisherBuyed : NetworkMessage
	{
		public int myID;

		public bool[] publisherBuyed;
	}

	public struct c_Platform : NetworkMessage
	{
		public int myID;

		public int date_year;

		public int date_month;

		public int date_year_end;

		public int date_month_end;

		public int price;

		public int dev_costs;

		public int tech;

		public int typ;

		public float marktanteil;

		public int[] needFeatures;

		public int units;

		public int units_max;

		public int minGamePassGames;

		public string name_EN;

		public string name_GE;

		public string name_TU;

		public string name_CH;

		public string name_FR;

		public string name_HU;

		public string name_JA;

		public string name_PL;

		public string name_UA;

		public string name_TH;

		public string manufacturer_EN;

		public string manufacturer_GE;

		public string manufacturer_TU;

		public string manufacturer_CH;

		public string manufacturer_FR;

		public string manufacturer_HU;

		public string manufacturer_JA;

		public string manufacturer_PL;

		public string manufacturer_UA;

		public string manufacturer_TH;

		public string pic1_file;

		public string pic2_file;

		public int pic2_year;

		public int games;

		public int exklusivGames;

		public int erfahrung;

		public bool isUnlocked;

		public bool inBesitz;

		public bool vomMarktGenommen;

		public int complex;

		public bool internet;

		public float powerFromMarket;

		public string myName;

		public int ownerID;

		public int gameID;

		public int anzController;

		public Vector3 consoleColor;

		public int component_cpu;

		public int component_gfx;

		public int component_ram;

		public int component_hdd;

		public int component_sfx;

		public int component_cooling;

		public int component_disc;

		public int component_controller;

		public int component_case;

		public int component_monitor;

		public bool[] hwFeatures;

		public float devPoints;

		public float devPointsStart;

		public long entwicklungsKosten;

		public long einnahmen;

		public float hype;

		public int startProduktionskosten;

		public int verkaufspreis;

		public float kostenreduktion;

		public bool autoPreis;

		public bool thridPartyGames;

		public long umsatzTotal;

		public int[] sellsPerWeek;

		public int weeksOnMarket;

		public float review;

		public int performancePoints;

		public int nachfolgerID;

		public int vorgaengerID;

		public bool proVersion;

		public string proName;

		public int subventionMoney;

		public bool[] subventionGameSize;
	}

	public struct c_PlatformRemoveFromMarket : NetworkMessage
	{
		public int platformID;
	}

	public struct c_Chat : NetworkMessage
	{
		public int playerID;

		public string text;
	}

	public struct c_Command : NetworkMessage
	{
		public int playerID;

		public int command;
	}

	public struct c_Money : NetworkMessage
	{
		public int playerID;

		public long money;

		public int fans;
	}

	public struct c_PlayerInfos : NetworkMessage
	{
		public int playerID;

		public string playerName;

		public bool ready;
	}

	public struct c_DeleteArbeitsmarkt : NetworkMessage
	{
		public int playerID;

		public int objectID;

		public bool eingestellt;
	}

	public struct c_BuyLizenz : NetworkMessage
	{
		public int playerID;

		public int objectID;
	}

	public struct c_exklusivKonsolenSells : NetworkMessage
	{
		public int gameID;

		public long exklusivKonsolenSells;
	}

	public struct c_GameDestroy : NetworkMessage
	{
		public int gameID;
	}

	public struct c_GameRemoveFromMarket : NetworkMessage
	{
		public int gameID;
	}

	public struct c_GameOwner : NetworkMessage
	{
		public int gameID;

		public int ownerID;
	}

	public struct c_GameIpSell : NetworkMessage
	{
		public int gameID;

		public bool ipToSell;
	}

	public struct c_GameIpPoints : NetworkMessage
	{
		public int gameID;

		public float ipPunkte;

		public int ipTime;
	}

	public struct c_GameSell : NetworkMessage
	{
		public int gameID;

		public bool isOnMarket;

		public int weeksOnMarket;

		public long sellsTotal;

		public long sellsTotalStandard;

		public long sellsTotalDeluxe;

		public long sellsTotalCollectors;

		public long sellsTotalOnline;

		public long abonnements;

		public long abonnementsWoche;

		public long bestAbonnements;

		public long exklusivKonsolenSells;

		public int userPositiv;

		public int userNegativ;

		public long costs_entwicklung;

		public long costs_mitarbeiter;

		public long costs_marketing;

		public long costs_enginegebuehren;

		public long costs_server;

		public long costs_production;

		public long costs_updates;

		public float points_gameplay;

		public float points_grafik;

		public float points_sound;

		public float points_technik;

		public float points_bugs;

		public float points_bugsInvis;

		public long umsatzTotal;

		public long umsatzInApp;

		public long umsatzAbos;

		public int bestChartPosition;

		public int lastChartPosition;

		public float f2pInteresse;

		public float mmoInteresse;

		public int vorbestellungen;

		public float realsticPower;

		public float hype;

		public int stornierungen;

		public bool commercialFlop;

		public bool commercialHit;

		public int freigabeBudget;

		public int releaseDate;

		public int inAppPurchaseWeek;

		public int[] sellsPerWeek;

		public int[] verkaufspreis;
	}

	public struct c_Game : NetworkMessage
	{
		public int gameID;

		public string myName;

		public string ipName;

		public bool playerGame;

		public bool inDevelopment;

		public int developerID;

		public int publisherID;

		public int ownerID;

		public int engineID;

		public float hype;

		public bool isOnMarket;

		public bool warBeiAwards;

		public int weeksOnMarket;

		public int usk;

		public int freigabeBudget;

		public int reviewGameplay;

		public int reviewGrafik;

		public int reviewSound;

		public int reviewSteuerung;

		public int reviewTotal;

		public int reviewGameplayText;

		public int reviewGrafikText;

		public int reviewSoundText;

		public int reviewSteuerungText;

		public int reviewTotalText;

		public int date_year;

		public int date_month;

		public int date_start_year;

		public int date_start_month;

		public long sellsTotal;

		public long umsatzTotal;

		public long costs_entwicklung;

		public long costs_mitarbeiter;

		public long costs_marketing;

		public long costs_enginegebuehren;

		public long costs_server;

		public long costs_production;

		public long costs_updates;

		public bool typ_standard;

		public bool typ_nachfolger;

		public int teile;

		public bool typ_contractGame;

		public bool typ_remaster;

		public bool typ_spinoff;

		public bool typ_addon;

		public bool typ_addonStandalone;

		public bool typ_mmoaddon;

		public bool typ_bundle;

		public bool typ_budget;

		public bool typ_bundleAddon;

		public bool typ_goty;

		public int originalGameID;

		public int portID;

		public int mainIP;

		public float ipPunkte;

		public bool exklusiv;

		public bool herstellerExklusiv;

		public bool retro;

		public bool handy;

		public bool arcade;

		public bool goty;

		public bool nachfolger_created;

		public bool remaster_created;

		public bool budget_created;

		public bool goty_created;

		public bool trendsetter;

		public bool spielbericht;

		public int amountUpdates;

		public float bonusSellsUpdates;

		public int amountAddons;

		public float bonusSellsAddons;

		public int amountMMOAddons;

		public float bonusSellsMMOAddons;

		public float addonQuality;

		public int devAktFeature;

		public float devPoints;

		public float devPointsStart;

		public float devPoints_Gesamt;

		public float devPointsStart_Gesamt;

		public float points_gameplay;

		public float points_grafik;

		public float points_sound;

		public float points_technik;

		public float points_bugs;

		public string beschreibung;

		public int gameTyp;

		public int gameSize;

		public int gameZielgruppe;

		public int maingenre;

		public int subgenre;

		public int gameMainTheme;

		public int gameSubTheme;

		public int gameLicence;

		public int gameCopyProtect;

		public int gameAntiCheat;

		public int gameAP_Gameplay;

		public int gameAP_Grafik;

		public int gameAP_Sound;

		public int gameAP_Technik;

		public bool[] gameLanguage;

		public bool[] gameGameplayFeatures;

		public int[] gamePlatform;

		public int[] gameEngineFeature;

		public bool[] gameplayFeatures_DevDone;

		public bool[] engineFeature_DevDone;

		public bool[] gameplayStudio;

		public bool[] grafikStudio;

		public bool[] soundStudio;

		public bool[] motionCaptureStudio;

		public int[] bundleID;

		public bool[] portExist;

		public int[] sellsPerWeek;

		public int[] verkaufspreis;

		public int releaseDate;

		public long abonnements;

		public long abonnementsWoche;

		public int aboPreis;

		public bool pubOffer;

		public bool pubAngebot;

		public int pubAngebot_Weeks;

		public float pubAngebot_Verhandlung;

		public bool pubAngebot_Retail;

		public bool pubAngebot_Digital;

		public int pubAngebot_Garantiesumme;

		public float pubAngebot_Gewinnbeteiligung;

		public bool auftragsspiel;

		public int auftragsspiel_gehalt;

		public int auftragsspiel_bonus;

		public int auftragsspiel_zeitInWochen;

		public int auftragsspiel_wochenAlsAngebot;

		public bool auftragsspiel_zeitAbgelaufen;

		public int auftragsspiel_mindestbewertung;

		public bool f2pConverted;

		public bool angekuendigt;

		public long subvention;

		public bool sonderIP;

		public int sonderIPMindestreview;

		public string myNameTeil1;

		public int engineGewinnbeteiligung;

		public int weeksInDevelopment;

		public int userPositiv;

		public int userNegativ;

		public long bestAbonnements;

		public int bestChartPosition;

		public long exklusivKonsolenSells;

		public int lastChartPosition;

		public bool freeware;

		public long sellsTotalStandard;

		public long sellsTotalDeluxe;

		public long sellsTotalCollectors;

		public long sellsTotalOnline;

		public float points_bugsInvis;

		public long umsatzInApp;

		public long umsatzAbos;

		public float f2pInteresse;

		public float mmoInteresse;

		public int vorbestellungen;

		public float realsticPower;

		public int stornierungen;

		public bool commercialFlop;

		public bool commercialHit;

		public int inAppPurchaseWeek;

		public int arcadeCase;

		public int arcadeMonitor;

		public int arcadeJoystick;

		public int arcadeSound;

		public int arcadeProdCosts;

		public int finanzierung_Grundkosten;

		public int finanzierung_Technology;

		public int finanzierung_Kontent;

		public bool retailVersion;

		public bool digitalVersion;

		public bool newGenreCombination;

		public bool newTopicCombination;

		public int ipTime;

		public bool npcLateinNumbers;

		public bool mmoTOf2p_created;

		public bool bundle_created;

		public int abosAddons;

		public bool[] inAppPurchase;

		public int[] Designschwerpunkt;

		public int[] Designausrichtung;
	}

	public struct s_AddPlayer : NetworkMessage
	{
		public int playerID;
	}

	public struct s_Forschung : NetworkMessage
	{
		public int playerID;

		public bool[] forschungSonstiges;

		public bool[] genres;

		public bool[] themes;

		public bool[] engineFeatures;

		public bool[] gameplayFeatures;

		public bool[] hardware;

		public bool[] hardwareFeatures;
	}

	public struct s_PlayerLeave : NetworkMessage
	{
		public int playerID;
	}

	public struct s_GenreBeliebtheit : NetworkMessage
	{
		public float[] genreBeliebtheit;
	}

	public struct s_GenreCombination : NetworkMessage
	{
		public int genreSlot;

		public bool[] genres_COMBINATION;
	}

	public struct s_GenreDesign : NetworkMessage
	{
		public int genreSlot;

		public int genres_focus0;

		public int genres_focus1;

		public int genres_focus2;

		public int genres_focus3;

		public int genres_focus4;

		public int genres_focus5;

		public int genres_focus6;

		public int genres_focus7;

		public int genres_align0;

		public int genres_align1;

		public int genres_align2;
	}

	public struct s_GenrePlatformSuit : NetworkMessage
	{
		public int genreSlot;

		public int pc_0;

		public int konsole_1;

		public int handheld_2;

		public int handy_3;

		public int arcade_4;
	}

	public struct s_Help : NetworkMessage
	{
		public int playerID;

		public int toPlayerID;

		public int what;

		public int valueA;

		public int valueB;

		public int valueC;
	}

	public struct s_ObjectDelete : NetworkMessage
	{
		public int playerID;

		public int objectID;
	}

	public struct s_Object : NetworkMessage
	{
		public int playerID;

		public int objectID;

		public int typ;

		public float x;

		public float y;

		public float rot;
	}

	public struct s_Map : NetworkMessage
	{
		public int playerID;

		public byte x;

		public byte y;

		public int id;

		public int typ;

		public int door;

		public int window;
	}

	public struct s_Office : NetworkMessage
	{
		public int office;
	}

	public struct s_Difficulty : NetworkMessage
	{
		public int difficulty;
	}

	public struct s_RandomSettings : NetworkMessage
	{
		public int randomSettings;
	}

	public struct s_Wettbewerb : NetworkMessage
	{
		public int competition;

		public int settings_RandomReviewsNum;

		public int settings_randomPlattformNum;
	}

	public struct s_Startjahr : NetworkMessage
	{
		public int startjahr;
	}

	public struct s_Entwicklungsdauer : NetworkMessage
	{
		public int dauer;
	}

	public struct s_AnzahlKonkurrenten : NetworkMessage
	{
		public int anzahl;
	}

	public struct s_Spielgeschwindigkeit : NetworkMessage
	{
		public int gamespeed;
	}

	public struct s_GlobalEvent : NetworkMessage
	{
		public int eventID;

		public int wochen;
	}

	public struct s_EngineAbrechnung : NetworkMessage
	{
		public int toPlayerID;

		public int gameID;
	}

	public struct s_Awards : NetworkMessage
	{
		public int bestGrafik;

		public int bestSound;

		public int bestStudio;

		public int bestPublisher;

		public int bestGame;

		public int badGame;
	}

	public struct s_Payment : NetworkMessage
	{
		public int playerID;

		public int toPlayerID;

		public int what;

		public int money;

		public int objectID;
	}

	public struct s_Engine : NetworkMessage
	{
		public int engineID;

		public int ownerID;

		public bool isUnlocked;

		public bool gekauft;

		public string myName;

		public bool[] features;

		public int spezialgenre;

		public int spezialplatform;

		public bool sellEngine;

		public int preis;

		public int gewinnbeteiligung;

		public float marktdominanz;
	}

	public struct s_EnginePublisherBuyed : NetworkMessage
	{
		public int engineID;

		public bool[] publisherBuyed;
	}

	public struct s_Platform : NetworkMessage
	{
		public int myID;

		public int date_year;

		public int date_month;

		public int date_year_end;

		public int date_month_end;

		public int price;

		public int dev_costs;

		public int tech;

		public int typ;

		public float marktanteil;

		public int[] needFeatures;

		public int units;

		public int units_max;

		public int minGamePassGames;

		public string name_EN;

		public string name_GE;

		public string name_TU;

		public string name_CH;

		public string name_FR;

		public string name_HU;

		public string name_JA;

		public string name_PL;

		public string name_UA;

		public string name_TH;

		public string manufacturer_EN;

		public string manufacturer_GE;

		public string manufacturer_TU;

		public string manufacturer_CH;

		public string manufacturer_FR;

		public string manufacturer_HU;

		public string manufacturer_JA;

		public string manufacturer_PL;

		public string manufacturer_UA;

		public string manufacturer_TH;

		public string pic1_file;

		public string pic2_file;

		public int pic2_year;

		public int games;

		public int exklusivGames;

		public int erfahrung;

		public bool isUnlocked;

		public bool inBesitz;

		public bool vomMarktGenommen;

		public int complex;

		public bool internet;

		public float powerFromMarket;

		public string myName;

		public int ownerID;

		public int gameID;

		public int anzController;

		public Vector3 consoleColor;

		public int component_cpu;

		public int component_gfx;

		public int component_ram;

		public int component_hdd;

		public int component_sfx;

		public int component_cooling;

		public int component_disc;

		public int component_controller;

		public int component_case;

		public int component_monitor;

		public bool[] hwFeatures;

		public float devPoints;

		public float devPointsStart;

		public long entwicklungsKosten;

		public long einnahmen;

		public float hype;

		public int startProduktionskosten;

		public int verkaufspreis;

		public float kostenreduktion;

		public bool autoPreis;

		public bool thridPartyGames;

		public long umsatzTotal;

		public int[] sellsPerWeek;

		public int weeksOnMarket;

		public float review;

		public int performancePoints;

		public int nachfolgerID;

		public int vorgaengerID;

		public bool proVersion;

		public string proName;

		public int subventionMoney;

		public bool[] subventionGameSize;
	}

	public struct s_PlatformData : NetworkMessage
	{
		public int platformID;

		public float marktanteil;

		public int units;

		public int units_max;

		public int date_year_end;
	}

	public struct s_PlatformRemoveFromMarket : NetworkMessage
	{
		public int platformID;
	}

	public struct s_PlatformSubvention : NetworkMessage
	{
		public int platformID;

		public int gameID;

		public int subvention;
	}

	public struct s_Chat : NetworkMessage
	{
		public int playerID;

		public string text;
	}

	public struct s_Money : NetworkMessage
	{
		public int playerID;

		public long money;

		public int fans;
	}

	public struct s_AutoPause : NetworkMessage
	{
		public int playerID;

		public bool pause;
	}

	public struct s_Genres : NetworkMessage
	{
		public float[] genres_BELIEBTHEIT;

		public bool[] genres_BELIEBTHEIT_SOLL;

		public int[] genres_RES_POINTS;

		public float[] genres_RES_POINTS_LEFT;

		public int[] genres_PRICE;

		public int[] genres_DEV_COSTS;

		public int[] genres_DATE_YEAR;

		public int[] genres_DATE_MONTH;

		public int[] genres_LEVEL;

		public bool[] genres_UNLOCK;

		public bool[] genres_SUC_YEAR;

		public bool[] genres_TARGETGROUP;

		public float[] genres_GAMEPLAY;

		public float[] genres_GRAPHIC;

		public float[] genres_SOUND;

		public float[] genres_CONTROL;

		public bool[] genres_COMBINATION;

		public int[] genres_PLATFORM_SELLS;

		public int[] genres_FOCUS;

		public bool[] genres_FOCUS_KNOWN;

		public int[] genres_ALIGN;

		public bool[] genres_ALIGN_KNOWN;

		public string[] genres_ICONFILE;

		public string[] genres_NAME_EN;

		public string[] genres_NAME_GE;

		public string[] genres_NAME_TU;

		public string[] genres_NAME_CH;

		public string[] genres_NAME_FR;

		public string[] genres_NAME_PB;

		public string[] genres_NAME_HU;

		public string[] genres_NAME_CT;

		public string[] genres_NAME_ES;

		public string[] genres_NAME_PL;

		public string[] genres_NAME_CZ;

		public string[] genres_NAME_KO;

		public string[] genres_NAME_IT;

		public string[] genres_NAME_AR;

		public string[] genres_NAME_JA;

		public string[] genres_NAME_UA;

		public string[] genres_NAME_TH;

		public string[] genres_NAME_RU;

		public string[] genres_DESC_EN;

		public string[] genres_DESC_GE;

		public string[] genres_DESC_TU;

		public string[] genres_DESC_CH;

		public string[] genres_DESC_FR;

		public string[] genres_DESC_PB;

		public string[] genres_DESC_HU;

		public string[] genres_DESC_CT;

		public string[] genres_DESC_ES;

		public string[] genres_DESC_PL;

		public string[] genres_DESC_CZ;

		public string[] genres_DESC_KO;

		public string[] genres_DESC_IT;

		public string[] genres_DESC_AR;

		public string[] genres_DESC_JA;

		public string[] genres_DESC_UA;

		public string[] genres_DESC_TH;

		public string[] genres_DESC_RU;

		public int[] genres_FANS;

		public int[] genres_MARKT;
	}

	public struct s_Topics : NetworkMessage
	{
		public int RES_POINTS;

		public bool[] themes_FITGENRE;

		public int[] themes_MGSR;
	}

	public struct s_GameplayFeatures : NetworkMessage
	{
		public int[] gameplayFeatures_TYP;

		public int[] gameplayFeatures_RES_POINTS;

		public float[] gameplayFeatures_RES_POINTS_LEFT;

		public int[] gameplayFeatures_PRICE;

		public int[] gameplayFeatures_DEV_COSTS;

		public int[] gameplayFeatures_DATE_YEAR;

		public int[] gameplayFeatures_DATE_MONTH;

		public int[] gameplayFeatures_GAMEPLAY;

		public int[] gameplayFeatures_GRAPHIC;

		public int[] gameplayFeatures_SOUND;

		public int[] gameplayFeatures_TECHNIK;

		public int[] gameplayFeatures_LEVEL;

		public int[] gameplayFeatures_NEED_GAMEPLAY_FEATURE;

		public bool[] gameplayFeatures_UNLOCK;

		public bool[] gameplayFeatures_INTERNET;

		public string[] gameplayFeatures_ICONFILE;

		public bool[] gameplayFeatures_GOOD;

		public bool[] gameplayFeatures_BAD;

		public bool[] gameplayFeatures_LOCKPLATFORM;

		public string[] gameplayFeatures_NAME_EN;

		public string[] gameplayFeatures_NAME_GE;

		public string[] gameplayFeatures_NAME_TU;

		public string[] gameplayFeatures_NAME_CH;

		public string[] gameplayFeatures_NAME_FR;

		public string[] gameplayFeatures_NAME_PB;

		public string[] gameplayFeatures_NAME_CT;

		public string[] gameplayFeatures_NAME_HU;

		public string[] gameplayFeatures_NAME_ES;

		public string[] gameplayFeatures_NAME_CZ;

		public string[] gameplayFeatures_NAME_KO;

		public string[] gameplayFeatures_NAME_RU;

		public string[] gameplayFeatures_NAME_IT;

		public string[] gameplayFeatures_NAME_AR;

		public string[] gameplayFeatures_NAME_JA;

		public string[] gameplayFeatures_NAME_PL;

		public string[] gameplayFeatures_NAME_UA;

		public string[] gameplayFeatures_NAME_TH;

		public string[] gameplayFeatures_DESC_EN;

		public string[] gameplayFeatures_DESC_GE;

		public string[] gameplayFeatures_DESC_TU;

		public string[] gameplayFeatures_DESC_CH;

		public string[] gameplayFeatures_DESC_FR;

		public string[] gameplayFeatures_DESC_PB;

		public string[] gameplayFeatures_DESC_CT;

		public string[] gameplayFeatures_DESC_HU;

		public string[] gameplayFeatures_DESC_ES;

		public string[] gameplayFeatures_DESC_CZ;

		public string[] gameplayFeatures_DESC_KO;

		public string[] gameplayFeatures_DESC_RU;

		public string[] gameplayFeatures_DESC_IT;

		public string[] gameplayFeatures_DESC_AR;

		public string[] gameplayFeatures_DESC_JA;

		public string[] gameplayFeatures_DESC_PL;

		public string[] gameplayFeatures_DESC_UA;

		public string[] gameplayFeatures_DESC_TH;
	}

	public struct s_EngineFeatures : NetworkMessage
	{
		public int[] engineFeatures_TYP;

		public int[] engineFeatures_RES_POINTS;

		public float[] engineFeatures_RES_POINTS_LEFT;

		public int[] engineFeatures_PRICE;

		public int[] engineFeatures_DEV_COSTS;

		public int[] engineFeatures_TECH;

		public int[] engineFeatures_DATE_YEAR;

		public int[] engineFeatures_DATE_MONTH;

		public int[] engineFeatures_GAMEPLAY;

		public int[] engineFeatures_GRAPHIC;

		public int[] engineFeatures_SOUND;

		public int[] engineFeatures_TECHNIK;

		public int[] engineFeatures_LEVEL;

		public bool[] engineFeatures_UNLOCK;

		public string[] engineFeatures_ICONFILE;

		public string[] engineFeatures_NAME_EN;

		public string[] engineFeatures_NAME_GE;

		public string[] engineFeatures_NAME_TU;

		public string[] engineFeatures_NAME_CH;

		public string[] engineFeatures_NAME_FR;

		public string[] engineFeatures_NAME_PB;

		public string[] engineFeatures_NAME_CT;

		public string[] engineFeatures_NAME_HU;

		public string[] engineFeatures_NAME_ES;

		public string[] engineFeatures_NAME_CZ;

		public string[] engineFeatures_NAME_KO;

		public string[] engineFeatures_NAME_AR;

		public string[] engineFeatures_NAME_RU;

		public string[] engineFeatures_NAME_IT;

		public string[] engineFeatures_NAME_JA;

		public string[] engineFeatures_NAME_PL;

		public string[] engineFeatures_NAME_UA;

		public string[] engineFeatures_NAME_TH;

		public string[] engineFeatures_DESC_EN;

		public string[] engineFeatures_DESC_GE;

		public string[] engineFeatures_DESC_TU;

		public string[] engineFeatures_DESC_CH;

		public string[] engineFeatures_DESC_FR;

		public string[] engineFeatures_DESC_PB;

		public string[] engineFeatures_DESC_CT;

		public string[] engineFeatures_DESC_HU;

		public string[] engineFeatures_DESC_ES;

		public string[] engineFeatures_DESC_CZ;

		public string[] engineFeatures_DESC_KO;

		public string[] engineFeatures_DESC_AR;

		public string[] engineFeatures_DESC_RU;

		public string[] engineFeatures_DESC_IT;

		public string[] engineFeatures_DESC_JA;

		public string[] engineFeatures_DESC_PL;

		public string[] engineFeatures_DESC_UA;

		public string[] engineFeatures_DESC_TH;
	}

	public struct s_HardwareFeatures : NetworkMessage
	{
		public string[] hardFeat_ICONFILE;

		public int[] hardFeat_RES_POINTS;

		public float[] hardFeat_RES_POINTS_LEFT;

		public int[] hardFeat_PRICE;

		public int[] hardFeat_DEV_COSTS;

		public int[] hardFeat_DATE_YEAR;

		public int[] hardFeat_DATE_MONTH;

		public bool[] hardFeat_UNLOCK;

		public bool[] hardFeat_ONLYSTATIONARY;

		public bool[] hardFeat_ONLYHANDHELD;

		public bool[] hardFeat_NEEDINTERNET;

		public float[] hardFeat_QUALITY;

		public string[] hardFeat_NAME_EN;

		public string[] hardFeat_NAME_GE;

		public string[] hardFeat_NAME_TU;

		public string[] hardFeat_NAME_CH;

		public string[] hardFeat_NAME_FR;

		public string[] hardFeat_NAME_PB;

		public string[] hardFeat_NAME_CT;

		public string[] hardFeat_NAME_HU;

		public string[] hardFeat_NAME_ES;

		public string[] hardFeat_NAME_CZ;

		public string[] hardFeat_NAME_KO;

		public string[] hardFeat_NAME_AR;

		public string[] hardFeat_NAME_RU;

		public string[] hardFeat_NAME_IT;

		public string[] hardFeat_NAME_JA;

		public string[] hardFeat_NAME_PL;

		public string[] hardFeat_NAME_UA;

		public string[] hardFeat_NAME_TH;

		public string[] hardFeat_DESC_EN;

		public string[] hardFeat_DESC_GE;

		public string[] hardFeat_DESC_TU;

		public string[] hardFeat_DESC_CH;

		public string[] hardFeat_DESC_FR;

		public string[] hardFeat_DESC_PB;

		public string[] hardFeat_DESC_CT;

		public string[] hardFeat_DESC_HU;

		public string[] hardFeat_DESC_ES;

		public string[] hardFeat_DESC_CZ;

		public string[] hardFeat_DESC_KO;

		public string[] hardFeat_DESC_AR;

		public string[] hardFeat_DESC_RU;

		public string[] hardFeat_DESC_IT;

		public string[] hardFeat_DESC_JA;

		public string[] hardFeat_DESC_PL;

		public string[] hardFeat_DESC_UA;

		public string[] hardFeat_DESC_TH;
	}

	public struct s_Hardware : NetworkMessage
	{
		public string[] hardware_ICONFILE;

		public int[] hardware_TYP;

		public int[] hardware_RES_POINTS;

		public float[] hardware_RES_POINTS_LEFT;

		public int[] hardware_PRICE;

		public int[] hardware_DEV_COSTS;

		public int[] hardware_TECH;

		public int[] hardware_DATE_YEAR;

		public int[] hardware_DATE_MONTH;

		public bool[] hardware_UNLOCK;

		public bool[] hardware_ONLYSTATIONARY;

		public bool[] hardware_ONLYHANDHELD;

		public int[] hardware_NEED1;

		public int[] hardware_NEED2;

		public string[] hardware_NAME_EN;

		public string[] hardware_NAME_GE;

		public string[] hardware_NAME_TU;

		public string[] hardware_NAME_CH;

		public string[] hardware_NAME_FR;

		public string[] hardware_NAME_PB;

		public string[] hardware_NAME_CT;

		public string[] hardware_NAME_HU;

		public string[] hardware_NAME_ES;

		public string[] hardware_NAME_CZ;

		public string[] hardware_NAME_KO;

		public string[] hardware_NAME_AR;

		public string[] hardware_NAME_RU;

		public string[] hardware_NAME_IT;

		public string[] hardware_NAME_JA;

		public string[] hardware_NAME_PL;

		public string[] hardware_NAME_UA;

		public string[] hardware_NAME_TH;

		public string[] hardware_DESC_EN;

		public string[] hardware_DESC_GE;

		public string[] hardware_DESC_TU;

		public string[] hardware_DESC_CH;

		public string[] hardware_DESC_FR;

		public string[] hardware_DESC_PB;

		public string[] hardware_DESC_CT;

		public string[] hardware_DESC_HU;

		public string[] hardware_DESC_ES;

		public string[] hardware_DESC_CZ;

		public string[] hardware_DESC_KO;

		public string[] hardware_DESC_AR;

		public string[] hardware_DESC_RU;

		public string[] hardware_DESC_IT;

		public string[] hardware_DESC_JA;

		public string[] hardware_DESC_PL;

		public string[] hardware_DESC_UA;

		public string[] hardware_DESC_TH;
	}

	public struct s_AntiCheat : NetworkMessage
	{
		public int myID;

		public int date_year;

		public int date_month;

		public int price;

		public int dev_costs;

		public string name_EN;

		public string name_GE;

		public string name_TU;

		public string name_CH;

		public string name_FR;

		public string name_CT;

		public string name_RU;

		public string name_IT;

		public string name_JA;

		public string name_UA;

		public string name_TH;

		public bool isUnlocked;

		public float effekt;

		public bool neverLooseEffect;
	}

	public struct s_CopyProtect : NetworkMessage
	{
		public int myID;

		public int date_year;

		public int date_month;

		public int price;

		public int dev_costs;

		public string name_EN;

		public string name_GE;

		public string name_TU;

		public string name_CH;

		public string name_FR;

		public string name_CT;

		public string name_RU;

		public string name_IT;

		public string name_JA;

		public string name_UA;

		public string name_TH;

		public bool isUnlocked;

		public float effekt;

		public bool neverLooseEffect;
	}

	public struct s_NpcEngine : NetworkMessage
	{
		public int myID;

		public int ownerID;

		public bool isUnlocked;

		public bool gekauft;

		public string myName;

		public int umsatz;

		public string name_EN;

		public string name_GE;

		public string name_TU;

		public string name_CH;

		public string name_FR;

		public string name_HU;

		public string name_CT;

		public string name_CZ;

		public string name_PB;

		public string name_IT;

		public string name_JA;

		public string name_UA;

		public string name_PL;

		public string name_TH;

		public bool[] features;

		public bool[] featuresInDev;

		public int spezialgenre;

		public int spezialplatform;

		public bool sellEngine;

		public int preis;

		public int gewinnbeteiligung;

		public bool updating;

		public float devPoints;

		public float devPointsStart;

		public int date_year;

		public int date_month;

		public bool[] publisherBuyed;

		public bool archiv_engine;
	}

	public struct s_TochterfirmaUmsatz : NetworkMessage
	{
		public int publisherID;

		public int gameID;

		public long money;
	}

	public struct s_Firmenwert : NetworkMessage
	{
		public int[] publisherID;

		public long[] firmenwert;
	}

	public struct s_NpcGameName : NetworkMessage
	{
		public int slot;

		public bool ip;
	}

	public struct s_BlockContractGame : NetworkMessage
	{
		public int myID;

		public bool block;
	}

	public struct s_Publisher : NetworkMessage
	{
		public int myID;

		public bool isUnlocked;

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

		public float stars;

		public int logoID;

		public bool developer;

		public bool publisher;

		public bool onlyMobile;

		public float share;

		public int fanGenre;

		public long firmenwert;

		public bool notForSale;

		public int lockToBuy;

		public bool isPlayer;

		public int ownerID;

		public int country;

		public bool ownPlatform;

		public bool exklusive;

		public int[] awards;
	}

	public struct s_PublisherOwner : NetworkMessage
	{
		public int myID;

		public int ownerID;
	}

	public struct s_PublisherClose : NetworkMessage
	{
		public int myID;
	}

	public struct s_PublisherTochterfirmaSettings : NetworkMessage
	{
		public int myID;

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

		public bool tf_ownPublisher;

		public bool tf_publisher;

		public bool tf_developer;

		public int tf_entwicklungsdauer;

		public int tf_gameSize;

		public int tf_gameGenre;

		public int tf_gameTopic;

		public int tf_engine;

		public int tf_autoReleaseVal;

		public int tf_ownPublisherPriority;

		public int[] tf_ipFocus;

		public int[] tf_platformFocus;
	}

	public struct s_exklusivKonsolenSells : NetworkMessage
	{
		public int gameID;

		public long exklusivKonsolenSells;
	}

	public struct s_GameAnkuendigung : NetworkMessage
	{
		public int gameID;
	}

	public struct s_GameDestroy : NetworkMessage
	{
		public int gameID;
	}

	public struct s_GameRemoveFromMarket : NetworkMessage
	{
		public int gameID;
	}

	public struct s_GameOwner : NetworkMessage
	{
		public int gameID;

		public int ownerID;
	}

	public struct s_GameIpSell : NetworkMessage
	{
		public int gameID;

		public bool ipToSell;
	}

	public struct s_GameIpPoints : NetworkMessage
	{
		public int gameID;

		public float ipPunkte;

		public int ipTime;
	}

	public struct s_GameSell : NetworkMessage
	{
		public int gameID;

		public bool isOnMarket;

		public int weeksOnMarket;

		public long sellsTotal;

		public long sellsTotalStandard;

		public long sellsTotalDeluxe;

		public long sellsTotalCollectors;

		public long sellsTotalOnline;

		public long abonnements;

		public long abonnementsWoche;

		public long bestAbonnements;

		public long exklusivKonsolenSells;

		public int userPositiv;

		public int userNegativ;

		public long costs_entwicklung;

		public long costs_mitarbeiter;

		public long costs_marketing;

		public long costs_enginegebuehren;

		public long costs_server;

		public long costs_production;

		public long costs_updates;

		public float points_gameplay;

		public float points_grafik;

		public float points_sound;

		public float points_technik;

		public float points_bugs;

		public float points_bugsInvis;

		public long umsatzTotal;

		public long umsatzInApp;

		public long umsatzAbos;

		public int bestChartPosition;

		public int lastChartPosition;

		public float f2pInteresse;

		public float mmoInteresse;

		public int vorbestellungen;

		public float realsticPower;

		public float hype;

		public int stornierungen;

		public bool commercialFlop;

		public bool commercialHit;

		public int freigabeBudget;

		public int releaseDate;

		public int inAppPurchaseWeek;

		public int[] sellsPerWeek;

		public int[] verkaufspreis;
	}

	public struct s_Game : NetworkMessage
	{
		public int gameID;

		public string myName;

		public string ipName;

		public bool playerGame;

		public bool inDevelopment;

		public int developerID;

		public int publisherID;

		public int ownerID;

		public int engineID;

		public float hype;

		public bool isOnMarket;

		public bool warBeiAwards;

		public int weeksOnMarket;

		public int usk;

		public int freigabeBudget;

		public int reviewGameplay;

		public int reviewGrafik;

		public int reviewSound;

		public int reviewSteuerung;

		public int reviewTotal;

		public int reviewGameplayText;

		public int reviewGrafikText;

		public int reviewSoundText;

		public int reviewSteuerungText;

		public int reviewTotalText;

		public int date_year;

		public int date_month;

		public int date_start_year;

		public int date_start_month;

		public long sellsTotal;

		public long umsatzTotal;

		public long costs_entwicklung;

		public long costs_mitarbeiter;

		public long costs_marketing;

		public long costs_enginegebuehren;

		public long costs_server;

		public long costs_production;

		public long costs_updates;

		public bool typ_standard;

		public bool typ_nachfolger;

		public int teile;

		public bool typ_contractGame;

		public bool typ_remaster;

		public bool typ_spinoff;

		public bool typ_addon;

		public bool typ_addonStandalone;

		public bool typ_mmoaddon;

		public bool typ_bundle;

		public bool typ_budget;

		public bool typ_bundleAddon;

		public bool typ_goty;

		public int originalGameID;

		public int portID;

		public int mainIP;

		public float ipPunkte;

		public bool exklusiv;

		public bool herstellerExklusiv;

		public bool retro;

		public bool handy;

		public bool arcade;

		public bool goty;

		public bool nachfolger_created;

		public bool remaster_created;

		public bool budget_created;

		public bool goty_created;

		public bool trendsetter;

		public bool spielbericht;

		public int amountUpdates;

		public float bonusSellsUpdates;

		public int amountAddons;

		public float bonusSellsAddons;

		public int amountMMOAddons;

		public float bonusSellsMMOAddons;

		public float addonQuality;

		public int devAktFeature;

		public float devPoints;

		public float devPointsStart;

		public float devPoints_Gesamt;

		public float devPointsStart_Gesamt;

		public float points_gameplay;

		public float points_grafik;

		public float points_sound;

		public float points_technik;

		public float points_bugs;

		public string beschreibung;

		public int gameTyp;

		public int gameSize;

		public int gameZielgruppe;

		public int maingenre;

		public int subgenre;

		public int gameMainTheme;

		public int gameSubTheme;

		public int gameLicence;

		public int gameCopyProtect;

		public int gameAntiCheat;

		public int gameAP_Gameplay;

		public int gameAP_Grafik;

		public int gameAP_Sound;

		public int gameAP_Technik;

		public bool[] gameLanguage;

		public bool[] gameGameplayFeatures;

		public int[] gamePlatform;

		public int[] gameEngineFeature;

		public bool[] gameplayFeatures_DevDone;

		public bool[] engineFeature_DevDone;

		public bool[] gameplayStudio;

		public bool[] grafikStudio;

		public bool[] soundStudio;

		public bool[] motionCaptureStudio;

		public int[] bundleID;

		public bool[] portExist;

		public int[] sellsPerWeek;

		public int[] verkaufspreis;

		public int releaseDate;

		public long abonnements;

		public long abonnementsWoche;

		public int aboPreis;

		public bool pubOffer;

		public bool pubAngebot;

		public int pubAngebot_Weeks;

		public float pubAngebot_Verhandlung;

		public bool pubAngebot_Retail;

		public bool pubAngebot_Digital;

		public int pubAngebot_Garantiesumme;

		public float pubAngebot_Gewinnbeteiligung;

		public bool auftragsspiel;

		public int auftragsspiel_gehalt;

		public int auftragsspiel_bonus;

		public int auftragsspiel_zeitInWochen;

		public int auftragsspiel_wochenAlsAngebot;

		public bool auftragsspiel_zeitAbgelaufen;

		public int auftragsspiel_mindestbewertung;

		public bool f2pConverted;

		public bool angekuendigt;

		public long subvention;

		public bool sonderIP;

		public int sonderIPMindestreview;

		public string myNameTeil1;

		public int engineGewinnbeteiligung;

		public int weeksInDevelopment;

		public int userPositiv;

		public int userNegativ;

		public long bestAbonnements;

		public int bestChartPosition;

		public long exklusivKonsolenSells;

		public int lastChartPosition;

		public bool freeware;

		public long sellsTotalStandard;

		public long sellsTotalDeluxe;

		public long sellsTotalCollectors;

		public long sellsTotalOnline;

		public float points_bugsInvis;

		public long umsatzInApp;

		public long umsatzAbos;

		public float f2pInteresse;

		public float mmoInteresse;

		public int vorbestellungen;

		public float realsticPower;

		public int stornierungen;

		public bool commercialFlop;

		public bool commercialHit;

		public int inAppPurchaseWeek;

		public int arcadeCase;

		public int arcadeMonitor;

		public int arcadeJoystick;

		public int arcadeSound;

		public int arcadeProdCosts;

		public int finanzierung_Grundkosten;

		public int finanzierung_Technology;

		public int finanzierung_Kontent;

		public bool retailVersion;

		public bool digitalVersion;

		public bool newGenreCombination;

		public bool newTopicCombination;

		public int ipTime;

		public bool npcLateinNumbers;

		public bool mmoTOf2p_created;

		public bool bundle_created;

		public int abosAddons;

		public bool[] inAppPurchase;

		public int[] Designschwerpunkt;

		public int[] Designausrichtung;
	}

	public struct s_Lizenz : NetworkMessage
	{
		public int lizenzID;

		public string name;

		public int typ;

		public int angebot;

		public float quality;

		public int licence_GENREGOOD;

		public int licence_GENREBAD;

		public int licence_YEAR;
	}

	public struct s_Trend : NetworkMessage
	{
		public int trendWeeks;

		public int trendTheme;

		public int trendAntiTheme;

		public int trendGenre;

		public int trendAntiGenre;

		public int trendNextGenre;

		public int trendNextAntiGenre;

		public int trendNextTheme;

		public int trendNextAntiTheme;
	}

	public struct s_GameSpeed : NetworkMessage
	{
		public int speed;
	}

	public struct s_Command : NetworkMessage
	{
		public int command;
	}

	public struct s_Save : NetworkMessage
	{
		public int saveID;
	}

	public struct s_Load : NetworkMessage
	{
		public int saveID;
	}

	public struct s_PlayerID : NetworkMessage
	{
		public int id;

		public string version;
	}

	public struct s_PlayerInfos : NetworkMessage
	{
		public int id;

		public string playerName;

		public bool ready;
	}

	public struct s_KillAA : NetworkMessage
	{
		public int charID;

		public int wert2;

		public bool eingestellt;

		public int wert3;
	}

	public struct s_CreateArbeitsmarkt : NetworkMessage
	{
		public int objectID;

		public bool male;

		public string myName;

		public int wochenAmArbeitsmarkt;

		public int legend;

		public int beruf;

		public float s_gamedesign;

		public float s_programmieren;

		public float s_grafik;

		public float s_sound;

		public float s_pr;

		public float s_gametests;

		public float s_technik;

		public float s_forschen;

		public bool[] perks;

		public int model_body;

		public int model_eyes;

		public int model_hair;

		public int model_beard;

		public int model_skinColor;

		public int model_hairColor;

		public int model_beardColor;

		public int model_HoseColor;

		public int model_ShirtColor;

		public int model_Add1Color;
	}

	public bool isServer;

	public bool isClient;

	public bool disableSend;

	public NetworkManager manager;

	public List<player_mp> playersMP = new List<player_mp>();

	public float timer;

	public float timer10Secs;

	public mainScript mS_;

	public GameObject main_;

	public GameObject guiMainGO_;

	public GameObject sfxGO_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private mpMain mpMain_;

	private arbeitsmarkt arbeitsmarkt_;

	private licences licences_;

	private games games_;

	private engineFeatures eF_;

	private genres genres_;

	private savegameScript save_;

	private mapScript mapScript_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private forschungSonstiges fS_;

	private themes themes_;

	private platforms platforms_;

	private publisher publisher_;

	private copyProtect copyProtect_;

	private anitCheat antiCheat_;

	public bool INIT_GENRES;

	public bool INIT_ID;

	public void ResetInit()
	{
		INIT_ID = false;
		INIT_GENRES = false;
	}

	public player_mp FindPlayer(int id_)
	{
		for (int i = 0; i < playersMP.Count; i++)
		{
			if (playersMP[i].playerID == id_)
			{
				return playersMP[i];
			}
		}
		return null;
	}

	public string GetCompanyName(int id_)
	{
		player_mp player_mp2 = FindPlayer(id_);
		if (player_mp2 == null)
		{
			return "---";
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!player_mp2.myPubScript_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == id_)
				{
					player_mp2.myPubScript_ = mS_.arrayPublisherScripts[i];
					break;
				}
			}
		}
		if ((bool)player_mp2.myPubScript_)
		{
			return player_mp2.myPubScript_.GetName();
		}
		return "";
	}

	public string GetPlayerName(int id_)
	{
		player_mp player_mp2 = FindPlayer(id_);
		if (player_mp2 == null)
		{
			return "---";
		}
		return player_mp2.playerName;
	}

	public bool GetReady(int id_)
	{
		return FindPlayer(id_)?.ready ?? false;
	}

	public long GetMoney(int id_)
	{
		return FindPlayer(id_)?.money ?? 0;
	}

	public int GetFans(int id_)
	{
		return FindPlayer(id_)?.fans ?? 0;
	}

	public int GetLogo(int id_)
	{
		player_mp player_mp2 = FindPlayer(id_);
		if (player_mp2 == null)
		{
			return 0;
		}
		if (!player_mp2.myPubScript_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == id_)
				{
					player_mp2.myPubScript_ = mS_.arrayPublisherScripts[i];
					break;
				}
			}
		}
		if ((bool)player_mp2.myPubScript_)
		{
			return player_mp2.myPubScript_.logoID;
		}
		return 0;
	}

	public int GetCountry(int id_)
	{
		player_mp player_mp2 = FindPlayer(id_);
		if (player_mp2 == null)
		{
			return 0;
		}
		if (!player_mp2.myPubScript_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == id_)
				{
					player_mp2.myPubScript_ = mS_.arrayPublisherScripts[i];
					break;
				}
			}
		}
		if ((bool)player_mp2.myPubScript_)
		{
			return player_mp2.myPubScript_.country;
		}
		return 0;
	}

	public bool GetPause(int id_)
	{
		return FindPlayer(id_)?.playerPause ?? false;
	}

	public void SetPause(bool b)
	{
		player_mp player_mp2 = FindPlayer(mS_.myID);
		if (player_mp2 != null)
		{
			player_mp2.playerPause = b;
		}
	}

	public bool GetAllPlayersReady()
	{
		if (!GetAllPlayersReady_RemoveNotExistentClients())
		{
			return false;
		}
		for (int i = 1; i < playersMP.Count; i++)
		{
			if (!playersMP[i].playerReady)
			{
				return false;
			}
		}
		return true;
	}

	public bool GetAllPlayersReady_RemoveNotExistentClients()
	{
		int num = 1;
		for (int i = 1; i < playersMP.Count; i++)
		{
			if (playersMP[i].playerReady)
			{
				num++;
			}
		}
		if (num >= GetConnectionCount() && num < playersMP.Count)
		{
			for (int j = 1; j < playersMP.Count; j++)
			{
				if (playersMP[j].playerReady)
				{
					continue;
				}
				int playerID = playersMP[j].playerID;
				SERVER_Send_PlayerLeave(playersMP[j].playerID);
				playersMP.RemoveAt(j);
				mS_.FindPublishers();
				for (int k = 0; k < mS_.arrayPublisherScripts.Length; k++)
				{
					if ((bool)mS_.arrayPublisherScripts[k] && mS_.arrayPublisherScripts[k].ownerID == playerID)
					{
						mS_.arrayPublisherScripts[k].RemoveTochterfirma();
						mS_.arrayPublisherScripts[k].ResetTochterfirmaSettings();
						if (isServer)
						{
							SERVER_Send_PublisherTochterfirmaSettings(mS_.arrayPublisherScripts[k]);
							SERVER_Send_Publisher(mS_.arrayPublisherScripts[k]);
						}
					}
				}
				for (int l = 0; l < games_.arrayGamesScripts.Length; l++)
				{
					if ((bool)games_.arrayGamesScripts[l] && games_.arrayGamesScripts[l].isOnMarket && (games_.arrayGamesScripts[l].ownerID == playerID || games_.arrayGamesScripts[l].developerID == playerID || games_.arrayGamesScripts[l].publisherID == playerID))
					{
						games_.arrayGamesScripts[l].isOnMarket = false;
						if (isServer)
						{
							SERVER_Send_GameSell(games_.arrayGamesScripts[l]);
						}
					}
				}
				games_.UpdateChartsWeek();
				guiMain_.UpdateCharts();
				return false;
			}
		}
		if (playersMP.Count <= 1)
		{
			mS_.multiplayer = false;
		}
		return true;
	}

	public int GetConnectionCount()
	{
		return NetworkServer.connections.Count;
	}

	public void SetPlayersUnready()
	{
		for (int i = 1; i < playersMP.Count; i++)
		{
			playersMP[i].playerReady = false;
		}
	}

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
		if (!mS_ && (bool)main_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMainGO_ = GameObject.Find("CanvasInGameMenu");
			if ((bool)guiMainGO_)
			{
				guiMain_ = guiMainGO_.GetComponent<GUI_Main>();
			}
		}
		if (!sfx_)
		{
			sfxGO_ = GameObject.Find("SFX");
			if ((bool)sfxGO_)
			{
				sfx_ = sfxGO_.GetComponent<sfxScript>();
			}
		}
		if ((bool)main_)
		{
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!arbeitsmarkt_)
			{
				arbeitsmarkt_ = main_.GetComponent<arbeitsmarkt>();
			}
			if (!licences_)
			{
				licences_ = main_.GetComponent<licences>();
			}
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
			}
			if (!genres_)
			{
				genres_ = main_.GetComponent<genres>();
			}
			if (!save_)
			{
				save_ = main_.GetComponent<savegameScript>();
			}
			if (!mpMain_)
			{
				mpMain_ = guiMain_.uiObjects[201].GetComponent<mpMain>();
			}
			if (!mapScript_)
			{
				mapScript_ = main_.GetComponent<mapScript>();
			}
			if (!gF_)
			{
				gF_ = main_.GetComponent<gameplayFeatures>();
			}
			if (!hardware_)
			{
				hardware_ = main_.GetComponent<hardware>();
			}
			if (!hardwareFeatures_)
			{
				hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
			}
			if (!fS_)
			{
				fS_ = main_.GetComponent<forschungSonstiges>();
			}
			if (!themes_)
			{
				themes_ = main_.GetComponent<themes>();
			}
			if (!platforms_)
			{
				platforms_ = main_.GetComponent<platforms>();
			}
			if (!publisher_)
			{
				publisher_ = main_.GetComponent<publisher>();
			}
			if (!copyProtect_)
			{
				copyProtect_ = main_.GetComponent<copyProtect>();
			}
			if (!antiCheat_)
			{
				antiCheat_ = main_.GetComponent<anitCheat>();
			}
		}
		if (!manager)
		{
			manager = GetComponent<NetworkManager>();
		}
	}

	private void Update()
	{
		FindScripts();
		if (!mS_ || !mS_.multiplayer)
		{
			return;
		}
		if (isClient && mS_.myID != -1 && !NetworkClient.isConnected)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[440]);
			guiMain_.OpenMenu(hideChars: false);
			mpMain_.StopNetwork();
			mS_.multiplayer = false;
			mS_.myID = -1;
			playersMP.Clear();
		}
		_ = isServer;
		Send1Sec();
		Send10Sec();
		for (int i = 1; i < playersMP.Count; i++)
		{
			if (playersMP[i].forschungSonstiges.Length <= 1)
			{
				if (playersMP[i].forschungSonstiges.Length != fS_.RES_POINTS.Length)
				{
					playersMP[i].forschungSonstiges = new bool[fS_.RES_POINTS.Length];
				}
				if (playersMP[i].genres.Length != genres_.genres_UNLOCK.Length)
				{
					playersMP[i].genres = new bool[genres_.genres_UNLOCK.Length];
				}
				if (playersMP[i].themes.Length != themes_.themes_RES_POINTS_LEFT.Length)
				{
					playersMP[i].themes = new bool[themes_.themes_RES_POINTS_LEFT.Length];
				}
				if (playersMP[i].engineFeatures.Length != eF_.engineFeatures_RES_POINTS.Length)
				{
					playersMP[i].engineFeatures = new bool[eF_.engineFeatures_RES_POINTS.Length];
				}
				if (playersMP[i].gameplayFeatures.Length != gF_.gameplayFeatures_RES_POINTS.Length)
				{
					playersMP[i].gameplayFeatures = new bool[gF_.gameplayFeatures_RES_POINTS.Length];
				}
				if (playersMP[i].hardware.Length != hardware_.hardware_RES_POINTS.Length)
				{
					playersMP[i].hardware = new bool[hardware_.hardware_RES_POINTS.Length];
				}
				if (playersMP[i].hardwareFeatures.Length != hardwareFeatures_.hardFeat_RES_POINTS.Length)
				{
					playersMP[i].hardwareFeatures = new bool[hardwareFeatures_.hardFeat_RES_POINTS.Length];
				}
			}
		}
	}

	private void Send1Sec()
	{
		if (mS_.myID == -1)
		{
			return;
		}
		timer += Time.deltaTime;
		if (timer < 1f)
		{
			return;
		}
		timer = 0f;
		player_mp player_mp2 = FindPlayer(mS_.myID);
		if (player_mp2 == null)
		{
			return;
		}
		player_mp2.money = mS_.money;
		player_mp2.fans = genres_.GetAmountFans();
		if (isServer)
		{
			SERVER_Send_Money();
			SERVER_Send_AutoPause();
		}
		if (!isClient)
		{
			return;
		}
		CLIENT_Send_Money();
		if (mS_.settings_autoPauseForMultiplayer)
		{
			if (guiMain_.menuOpen)
			{
				CLIENT_Send_Command(2);
			}
			else
			{
				CLIENT_Send_Command(3);
			}
		}
	}

	private void Send10Sec()
	{
		if (mS_.myID == -1)
		{
			return;
		}
		timer10Secs += Time.deltaTime;
		if (!(timer10Secs < 10f))
		{
			timer10Secs = 0f;
			if (isServer && (bool)guiMain_ && guiMain_.uiObjects[145].activeSelf)
			{
				SERVER_Send_Forschung(mS_.myID);
			}
			if (isClient && (bool)guiMain_ && guiMain_.uiObjects[145].activeSelf)
			{
				CLIENT_Send_Forschung();
			}
		}
	}

	public bool AutoPause()
	{
		if ((bool)mS_ && mS_.multiplayer && mS_.settings_autoPauseForMultiplayer)
		{
			for (int i = 0; i < playersMP.Count; i++)
			{
				if (playersMP[i].playerPause)
				{
					return true;
				}
			}
		}
		return false;
	}

	public int GetSlot()
	{
		if (!GameObject.Find("PUB_100000"))
		{
			return 0;
		}
		if (!GameObject.Find("PUB_100001"))
		{
			return 1;
		}
		if (!GameObject.Find("PUB_100002"))
		{
			return 2;
		}
		if (!GameObject.Find("PUB_100003"))
		{
			return 3;
		}
		return 0;
	}

	public int AddPlayerNew(int connectionID_)
	{
		Debug.Log("AddPlayer()");
		if (!mS_.mpLobbyOpen)
		{
			return -1;
		}
		int num = 100000 + GetSlot();
		playersMP.Add(new player_mp(num));
		publisherScript myPubScript_ = mS_.CreatePlayerPublisher(num);
		player_mp player_mp2 = FindPlayer(num);
		player_mp2.connectionID = connectionID_;
		player_mp2.myPubScript_ = myPubScript_;
		Debug.Log("PlayerID / ConnectionID: " + num + ", " + connectionID_);
		if (playersMP.Count <= 1)
		{
			mS_.myID = num;
			player_mp2.playerID = mS_.myID;
			player_mp2.playerName = mS_.playerName;
			player_mp2.playerReady = true;
			mS_.SetCompanyName(mpMain_.uiObjects[12].GetComponent<InputField>().text);
			mS_.SetCompanyLogoID(0);
			mS_.SetCountryID(0);
			mS_.SetFanGenreID(0);
		}
		else
		{
			SERVER_Send_AddPlayer();
		}
		if (num != 100000)
		{
			SERVER_Send_ID(num, null);
			SERVER_Send_Difficulty();
			SERVER_Send_RandomSettings();
			SERVER_Send_Startjahr();
			SERVER_Send_Entwicklungsdauer();
			SERVER_Send_Office();
			SERVER_Send_Spielgeschwindigkeit();
			SERVER_Send_Genres(num, null);
			SERVER_Send_AnzahlKonkurrenten();
			SERVER_Send_Wettbewerb();
			if (mS_.settings_allGamespeed)
			{
				SERVER_Send_Command(23);
			}
			else
			{
				SERVER_Send_Command(22);
			}
			if (mS_.settings_autoPauseForMultiplayer)
			{
				SERVER_Send_Command(7);
			}
			else
			{
				SERVER_Send_Command(6);
			}
			if (mS_.settings_RandomReviews)
			{
				SERVER_Send_Command(11);
			}
			else
			{
				SERVER_Send_Command(10);
			}
			if (mS_.settings_plattformEnd)
			{
				SERVER_Send_Command(25);
			}
			else
			{
				SERVER_Send_Command(24);
			}
			if (mS_.settings_sabotageOff)
			{
				SERVER_Send_Command(27);
			}
			else
			{
				SERVER_Send_Command(26);
			}
			if (mS_.settings_tochterfirmaOff)
			{
				SERVER_Send_Command(29);
			}
			else
			{
				SERVER_Send_Command(28);
			}
			if (mpMain_.uiObjects[42].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(13);
			}
			else
			{
				SERVER_Send_Command(12);
			}
			if (mpMain_.uiObjects[43].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(15);
			}
			else
			{
				SERVER_Send_Command(14);
			}
			if (mpMain_.uiObjects[45].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(17);
			}
			else
			{
				SERVER_Send_Command(16);
			}
			if (mpMain_.uiObjects[52].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(19);
			}
			else
			{
				SERVER_Send_Command(18);
			}
			if (mpMain_.uiObjects[53].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(31);
			}
			else
			{
				SERVER_Send_Command(30);
			}
			if (mpMain_.uiObjects[62].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(33);
			}
			else
			{
				SERVER_Send_Command(32);
			}
			if (mpMain_.uiObjects[63].GetComponent<Toggle>().isOn)
			{
				SERVER_Send_Command(35);
			}
			else
			{
				SERVER_Send_Command(34);
			}
		}
		return num;
	}

	public void RemovePlayerNew(int connectionID_)
	{
		Debug.Log("RemovePlayerNew (ConnectionID): " + connectionID_);
		int num = -1;
		for (int i = 0; i < playersMP.Count; i++)
		{
			if (playersMP[i].connectionID == connectionID_)
			{
				num = playersMP[i].playerID;
				Debug.Log("RemovePlayerNew (PlayerID) : " + num);
				break;
			}
		}
		if (num == -1)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		for (int j = 0; j < mS_.arrayPublisherScripts.Length; j++)
		{
			if ((bool)mS_.arrayPublisherScripts[j] && mS_.arrayPublisherScripts[j].myID == num)
			{
				if (guiMain_.uiObjects[201].activeSelf)
				{
					Object.Destroy(mS_.arrayPublisherScripts[j].gameObject);
				}
				break;
			}
		}
		for (int k = 0; k < playersMP.Count; k++)
		{
			if (playersMP[k].playerID == num)
			{
				Debug.Log("RemovePlayer (Destroy Player) : " + num);
				playersMP.RemoveAt(k);
				break;
			}
		}
		SERVER_Send_PlayerLeave(num);
	}

	public void SetupClient()
	{
		NetworkClient.RegisterHandler<s_PlayerInfos>(SERVER_Get_PlayerInfos);
		NetworkClient.RegisterHandler<s_PlayerID>(SERVER_Get_ID);
		NetworkClient.RegisterHandler<s_Command>(SERVER_Get_Command);
		NetworkClient.RegisterHandler<s_Save>(SERVER_Get_Save);
		NetworkClient.RegisterHandler<s_Load>(SERVER_Get_Load);
		NetworkClient.RegisterHandler<s_GameSpeed>(SERVER_Get_GameSpeed);
		NetworkClient.RegisterHandler<s_CreateArbeitsmarkt>(SERVER_Get_CreateArbeitsmarkt);
		NetworkClient.RegisterHandler<s_KillAA>(SERVER_Get_KillAA);
		NetworkClient.RegisterHandler<s_Trend>(SERVER_Get_Trend);
		NetworkClient.RegisterHandler<s_Lizenz>(SERVER_Get_Lizenz);
		NetworkClient.RegisterHandler<s_Game>(SERVER_Get_Game);
		NetworkClient.RegisterHandler<s_BlockContractGame>(SERVER_Get_BlockContractGame);
		NetworkClient.RegisterHandler<s_Publisher>(SERVER_Get_Publisher);
		NetworkClient.RegisterHandler<s_PublisherOwner>(SERVER_Get_PublisherOwner);
		NetworkClient.RegisterHandler<s_PublisherClose>(SERVER_Get_PublisherClose);
		NetworkClient.RegisterHandler<s_PublisherTochterfirmaSettings>(SERVER_Get_PublisherTochterfirmaSettings);
		NetworkClient.RegisterHandler<s_GameSell>(SERVER_Get_GameSell);
		NetworkClient.RegisterHandler<s_GameIpPoints>(SERVER_Get_GameIpPoints);
		NetworkClient.RegisterHandler<s_GameOwner>(SERVER_Get_GameOwner);
		NetworkClient.RegisterHandler<s_GameIpSell>(SERVER_Get_GameIpSell);
		NetworkClient.RegisterHandler<s_GameRemoveFromMarket>(SERVER_Get_GameRemoveFromMarket);
		NetworkClient.RegisterHandler<s_GameDestroy>(SERVER_Get_GameDestroy);
		NetworkClient.RegisterHandler<s_Money>(SERVER_Get_Money);
		NetworkClient.RegisterHandler<s_Chat>(SERVER_Get_Chat);
		NetworkClient.RegisterHandler<s_Engine>(SERVER_Get_Engine);
		NetworkClient.RegisterHandler<s_Payment>(SERVER_Get_Payment);
		NetworkClient.RegisterHandler<s_Awards>(SERVER_Get_Awards);
		NetworkClient.RegisterHandler<s_EngineAbrechnung>(SERVER_Get_EngineAbrechnung);
		NetworkClient.RegisterHandler<s_GlobalEvent>(SERVER_Get_GlobalEvent);
		NetworkClient.RegisterHandler<s_Difficulty>(SERVER_Get_Difficulty);
		NetworkClient.RegisterHandler<s_RandomSettings>(SERVER_Get_RandomSettings);
		NetworkClient.RegisterHandler<s_Wettbewerb>(SERVER_Get_Wettbewerb);
		NetworkClient.RegisterHandler<s_Startjahr>(SERVER_Get_Startjahr);
		NetworkClient.RegisterHandler<s_Entwicklungsdauer>(SERVER_Get_Entwicklungsdauer);
		NetworkClient.RegisterHandler<s_AnzahlKonkurrenten>(SERVER_Get_AnzahlKonkurrenten);
		NetworkClient.RegisterHandler<s_Office>(SERVER_Get_Office);
		NetworkClient.RegisterHandler<s_Spielgeschwindigkeit>(SERVER_Get_Spielgeschwindigkeit);
		NetworkClient.RegisterHandler<s_AutoPause>(SERVER_Get_AutoPause);
		NetworkClient.RegisterHandler<s_Map>(SERVER_Get_Map);
		NetworkClient.RegisterHandler<s_Object>(SERVER_Get_Object);
		NetworkClient.RegisterHandler<s_ObjectDelete>(SERVER_Get_ObjectDelete);
		NetworkClient.RegisterHandler<s_PlatformData>(SERVER_Get_PlatformData);
		NetworkClient.RegisterHandler<s_PlatformRemoveFromMarket>(SERVER_Get_PlatformRemoveFromMarket);
		NetworkClient.RegisterHandler<s_Help>(SERVER_Get_Help);
		NetworkClient.RegisterHandler<s_GenreDesign>(SERVER_Get_GenreDesign);
		NetworkClient.RegisterHandler<s_GenrePlatformSuit>(SERVER_Get_GenrePlatformSuit);
		NetworkClient.RegisterHandler<s_GenreCombination>(SERVER_Get_GenreCombination);
		NetworkClient.RegisterHandler<s_GenreBeliebtheit>(SERVER_Get_GenreBeliebtheit);
		NetworkClient.RegisterHandler<s_PlayerLeave>(SERVER_Get_PlayerLeave);
		NetworkClient.RegisterHandler<s_AddPlayer>(SERVER_Get_AddPlayer);
		NetworkClient.RegisterHandler<s_Platform>(SERVER_Get_Platform);
		NetworkClient.RegisterHandler<s_exklusivKonsolenSells>(SERVER_Get_ExklusivKonsolenSells);
		NetworkClient.RegisterHandler<s_CopyProtect>(SERVER_Get_CopyProtect);
		NetworkClient.RegisterHandler<s_AntiCheat>(SERVER_Get_AntiCheat);
		NetworkClient.RegisterHandler<s_Hardware>(SERVER_Get_Hardware);
		NetworkClient.RegisterHandler<s_HardwareFeatures>(SERVER_Get_HardwareFeatures);
		NetworkClient.RegisterHandler<s_EngineFeatures>(SERVER_Get_EngineFeatures);
		NetworkClient.RegisterHandler<s_GameplayFeatures>(SERVER_Get_GameplayFeatures);
		NetworkClient.RegisterHandler<s_NpcEngine>(SERVER_Get_NpcEngine);
		NetworkClient.RegisterHandler<s_Genres>(SERVER_Get_Genres);
		NetworkClient.RegisterHandler<s_Forschung>(SERVER_Get_Forschung);
		NetworkClient.RegisterHandler<s_Firmenwert>(SERVER_Get_Firmenwert);
		NetworkClient.RegisterHandler<s_TochterfirmaUmsatz>(SERVER_Get_TochterfirmaUmsatz);
		NetworkClient.RegisterHandler<s_NpcGameName>(SERVER_Get_NpcGameName);
		NetworkClient.RegisterHandler<s_GameAnkuendigung>(SERVER_Get_GameAnkeundigung);
		NetworkClient.RegisterHandler<s_Topics>(SERVER_Get_Topics);
		NetworkClient.RegisterHandler<s_PlatformSubvention>(SERVER_Get_PlatformSubvention);
		NetworkClient.RegisterHandler<s_EnginePublisherBuyed>(SERVER_Get_EnginePublisherBuyed);
	}

	public void UnregisterClient()
	{
		NetworkClient.UnregisterHandler<s_PlayerInfos>();
		NetworkClient.UnregisterHandler<s_PlayerID>();
		NetworkClient.UnregisterHandler<s_Command>();
		NetworkClient.UnregisterHandler<s_Save>();
		NetworkClient.UnregisterHandler<s_Load>();
		NetworkClient.UnregisterHandler<s_GameSpeed>();
		NetworkClient.UnregisterHandler<s_CreateArbeitsmarkt>();
		NetworkClient.UnregisterHandler<s_KillAA>();
		NetworkClient.UnregisterHandler<s_Trend>();
		NetworkClient.UnregisterHandler<s_Lizenz>();
		NetworkClient.UnregisterHandler<s_Game>();
		NetworkClient.UnregisterHandler<s_BlockContractGame>();
		NetworkClient.UnregisterHandler<s_Publisher>();
		NetworkClient.UnregisterHandler<s_PublisherOwner>();
		NetworkClient.UnregisterHandler<s_PublisherClose>();
		NetworkClient.UnregisterHandler<s_PublisherTochterfirmaSettings>();
		NetworkClient.UnregisterHandler<s_GameSell>();
		NetworkClient.UnregisterHandler<s_GameIpPoints>();
		NetworkClient.UnregisterHandler<s_GameOwner>();
		NetworkClient.UnregisterHandler<s_GameIpSell>();
		NetworkClient.UnregisterHandler<s_GameRemoveFromMarket>();
		NetworkClient.UnregisterHandler<s_GameDestroy>();
		NetworkClient.UnregisterHandler<s_Money>();
		NetworkClient.UnregisterHandler<s_Chat>();
		NetworkClient.UnregisterHandler<s_Engine>();
		NetworkClient.UnregisterHandler<s_Payment>();
		NetworkClient.UnregisterHandler<s_Awards>();
		NetworkClient.UnregisterHandler<s_EngineAbrechnung>();
		NetworkClient.UnregisterHandler<s_Difficulty>();
		NetworkClient.UnregisterHandler<s_RandomSettings>();
		NetworkClient.UnregisterHandler<s_Wettbewerb>();
		NetworkClient.UnregisterHandler<s_Startjahr>();
		NetworkClient.UnregisterHandler<s_Entwicklungsdauer>();
		NetworkClient.UnregisterHandler<s_AnzahlKonkurrenten>();
		NetworkClient.UnregisterHandler<s_Office>();
		NetworkClient.UnregisterHandler<s_Spielgeschwindigkeit>();
		NetworkClient.UnregisterHandler<s_AutoPause>();
		NetworkClient.UnregisterHandler<s_Map>();
		NetworkClient.UnregisterHandler<s_Object>();
		NetworkClient.UnregisterHandler<s_ObjectDelete>();
		NetworkClient.UnregisterHandler<s_PlatformData>();
		NetworkClient.UnregisterHandler<s_PlatformRemoveFromMarket>();
		NetworkClient.UnregisterHandler<s_Help>();
		NetworkClient.UnregisterHandler<s_GenreDesign>();
		NetworkClient.UnregisterHandler<s_GenrePlatformSuit>();
		NetworkClient.UnregisterHandler<s_GenreCombination>();
		NetworkClient.UnregisterHandler<s_GenreBeliebtheit>();
		NetworkClient.UnregisterHandler<s_PlayerLeave>();
		NetworkClient.UnregisterHandler<s_AddPlayer>();
		NetworkClient.UnregisterHandler<s_Platform>();
		NetworkClient.UnregisterHandler<s_exklusivKonsolenSells>();
		NetworkClient.UnregisterHandler<s_CopyProtect>();
		NetworkClient.UnregisterHandler<s_AntiCheat>();
		NetworkClient.UnregisterHandler<s_Hardware>();
		NetworkClient.UnregisterHandler<s_HardwareFeatures>();
		NetworkClient.UnregisterHandler<s_EngineFeatures>();
		NetworkClient.UnregisterHandler<s_GameplayFeatures>();
		NetworkClient.UnregisterHandler<s_NpcEngine>();
		NetworkClient.UnregisterHandler<s_Genres>();
		NetworkClient.UnregisterHandler<s_Forschung>();
		NetworkClient.UnregisterHandler<s_Firmenwert>();
		NetworkClient.UnregisterHandler<s_TochterfirmaUmsatz>();
		NetworkClient.UnregisterHandler<s_NpcGameName>();
		NetworkClient.UnregisterHandler<s_GameAnkuendigung>();
		NetworkClient.UnregisterHandler<s_Topics>();
		NetworkClient.UnregisterHandler<s_PlatformSubvention>();
		NetworkClient.UnregisterHandler<s_EnginePublisherBuyed>();
	}

	public void SetupServer()
	{
		SetupClient();
		NetworkServer.RegisterHandler<c_PlayerInfos>(CLIENT_Get_PlayerInfos);
		NetworkServer.RegisterHandler<c_Command>(CLIENT_Get_Command);
		NetworkServer.RegisterHandler<c_DeleteArbeitsmarkt>(CLIENT_Get_DeleteArbeitsmarkt);
		NetworkServer.RegisterHandler<c_BuyLizenz>(CLIENT_Get_BuyLizenz);
		NetworkServer.RegisterHandler<c_Game>(CLIENT_Get_Game);
		NetworkServer.RegisterHandler<c_GameSell>(CLIENT_Get_GameSell);
		NetworkServer.RegisterHandler<c_GameIpPoints>(CLIENT_Get_GameIpPoints);
		NetworkServer.RegisterHandler<c_GameOwner>(CLIENT_Get_GameOwner);
		NetworkServer.RegisterHandler<c_GameIpSell>(CLIENT_Get_GameIpSell);
		NetworkServer.RegisterHandler<c_GameRemoveFromMarket>(CLIENT_Get_GameRemoveFromMarket);
		NetworkServer.RegisterHandler<c_GameDestroy>(CLIENT_Get_GameDestroy);
		NetworkServer.RegisterHandler<c_Money>(CLIENT_Get_Money);
		NetworkServer.RegisterHandler<c_Chat>(CLIENT_Get_Chat);
		NetworkServer.RegisterHandler<c_Engine>(CLIENT_Get_Engine);
		NetworkServer.RegisterHandler<c_Payment>(CLIENT_Get_Payment);
		NetworkServer.RegisterHandler<c_Trend>(CLIENT_Get_Trend);
		NetworkServer.RegisterHandler<c_Map>(CLIENT_Get_Map);
		NetworkServer.RegisterHandler<c_Object>(CLIENT_Get_Object);
		NetworkServer.RegisterHandler<c_ObjectDelete>(CLIENT_Get_ObjectDelete);
		NetworkServer.RegisterHandler<c_Help>(CLIENT_Get_Help);
		NetworkServer.RegisterHandler<c_Platform>(CLIENT_Get_Platform);
		NetworkServer.RegisterHandler<c_PlatformRemoveFromMarket>(CLIENT_Get_PlatformRemoveFromMarket);
		NetworkServer.RegisterHandler<c_exklusivKonsolenSells>(CLIENT_Get_ExklusivKonsolenSells);
		NetworkServer.RegisterHandler<c_Forschung>(CLIENT_Get_Forschung);
		NetworkServer.RegisterHandler<c_BlockContractGame>(CLIENT_Get_BlockContractGame);
		NetworkServer.RegisterHandler<c_Publisher>(CLIENT_Get_Publisher);
		NetworkServer.RegisterHandler<c_PublisherOwner>(CLIENT_Get_PublisherOwner);
		NetworkServer.RegisterHandler<c_PublisherTochterfirmaSettings>(CLIENT_Get_PublisherTochterfirmaSettings);
		NetworkServer.RegisterHandler<c_GameSpeed>(CLIENT_Get_GameSpeed);
		NetworkServer.RegisterHandler<c_NpcGameName>(CLIENT_Get_NpcGameName);
		NetworkServer.RegisterHandler<c_EnginePublisherBuyed>(CLIENT_Get_EnginePublisherBuyed);
	}

	public void UnregisterServer()
	{
		NetworkServer.UnregisterHandler<c_PlayerInfos>();
		NetworkServer.UnregisterHandler<c_Command>();
		NetworkServer.UnregisterHandler<c_DeleteArbeitsmarkt>();
		NetworkServer.UnregisterHandler<c_BuyLizenz>();
		NetworkServer.UnregisterHandler<c_Game>();
		NetworkServer.UnregisterHandler<c_GameSell>();
		NetworkServer.UnregisterHandler<c_GameIpPoints>();
		NetworkServer.UnregisterHandler<c_GameOwner>();
		NetworkServer.UnregisterHandler<c_GameIpSell>();
		NetworkServer.UnregisterHandler<c_GameRemoveFromMarket>();
		NetworkServer.UnregisterHandler<c_GameDestroy>();
		NetworkServer.UnregisterHandler<c_Money>();
		NetworkServer.UnregisterHandler<c_Chat>();
		NetworkServer.UnregisterHandler<c_Engine>();
		NetworkServer.UnregisterHandler<c_Payment>();
		NetworkServer.UnregisterHandler<c_Trend>();
		NetworkServer.UnregisterHandler<c_Map>();
		NetworkServer.UnregisterHandler<c_Object>();
		NetworkServer.UnregisterHandler<c_ObjectDelete>();
		NetworkServer.UnregisterHandler<c_Help>();
		NetworkServer.UnregisterHandler<c_Platform>();
		NetworkServer.UnregisterHandler<c_PlatformRemoveFromMarket>();
		NetworkServer.UnregisterHandler<c_exklusivKonsolenSells>();
		NetworkServer.UnregisterHandler<c_Forschung>();
		NetworkServer.UnregisterHandler<c_BlockContractGame>();
		NetworkServer.UnregisterHandler<c_Publisher>();
		NetworkServer.UnregisterHandler<c_PublisherOwner>();
		NetworkServer.UnregisterHandler<c_PublisherTochterfirmaSettings>();
		NetworkServer.UnregisterHandler<c_GameSpeed>();
		NetworkServer.UnregisterHandler<c_NpcGameName>();
		NetworkServer.UnregisterHandler<c_EnginePublisherBuyed>();
	}

	public void CLIENT_Send_ObjectDelete(int id_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Object_Delete()");
			FindScripts();
			NetworkClient.Send(new c_ObjectDelete
			{
				playerID = mS_.myID,
				objectID = id_
			});
		}
	}

	public void CLIENT_Get_ObjectDelete(NetworkConnection conn, c_ObjectDelete msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_ObjectDelete()");
		player_mp player_mp2 = FindPlayer(msg.playerID);
		if (player_mp2 == null)
		{
			return;
		}
		for (int i = 0; i < player_mp2.objects.Count; i++)
		{
			if (player_mp2.objects[i] != null && player_mp2.objects[i].id == msg.objectID)
			{
				player_mp2.objects.RemoveAt(i);
				break;
			}
		}
		NetworkServer.SendToAll(new s_ObjectDelete
		{
			playerID = msg.playerID,
			objectID = msg.objectID
		});
	}

	public void CLIENT_Send_Object(int id_, int typ_, float x_, float y_, float rot_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Object()");
			FindScripts();
			NetworkClient.Send(new c_Object
			{
				playerID = mS_.myID,
				objectID = id_,
				typ = typ_,
				x = x_,
				y = y_,
				rot = rot_
			});
		}
	}

	public void CLIENT_Get_Object(NetworkConnection conn, c_Object msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_Object()");
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.objects.Add(new object_mp(msg.objectID, msg.typ, msg.x, msg.y, msg.rot));
				NetworkServer.SendToAll(new s_Object
				{
					playerID = msg.playerID,
					objectID = msg.objectID,
					typ = msg.typ,
					x = msg.x,
					y = msg.y,
					rot = msg.rot
				});
			}
		}
	}

	public void CLIENT_Send_Map(int posx, int posy)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("CLIENT_Send_Map()");
		FindScripts();
		if (mapScript_.IsInMapLimit(posx, posy))
		{
			int typ = 0;
			if ((bool)mapScript_.mapRoomScript[posx, posy])
			{
				typ = mapScript_.mapRoomScript[posx, posy].typ;
			}
			NetworkClient.Send(new c_Map
			{
				playerID = mS_.myID,
				x = (byte)posx,
				y = (byte)posy,
				id = mapScript_.mapRoomID[posx, posy],
				typ = typ,
				door = mapScript_.mapDoors[posx, posy],
				window = mapScript_.mapWindows[posx, posy]
			});
		}
	}

	public void CLIENT_Get_Map(NetworkConnection conn, c_Map msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_Map()");
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null && mapScript_.IsInMapLimit(msg.x, msg.y))
			{
				player_mp2.mapRoomID[msg.x, msg.y] = msg.id;
				player_mp2.mapRoomTyp[msg.x, msg.y] = msg.typ;
				player_mp2.mapDoors[msg.x, msg.y] = msg.door;
				player_mp2.mapWindows[msg.x, msg.y] = msg.window;
				NetworkServer.SendToAll(new s_Map
				{
					playerID = msg.playerID,
					x = msg.x,
					y = msg.y,
					id = msg.id,
					typ = msg.typ,
					door = msg.door,
					window = msg.window
				});
			}
		}
	}

	public void CLIENT_Send_NpcGameName(int slot_, bool ip_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_NpcGameName()");
			NetworkClient.Send(new c_NpcGameName
			{
				slot = slot_,
				ip = ip_
			});
		}
	}

	public void CLIENT_Get_NpcGameName(NetworkConnection conn, c_NpcGameName msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_NpcGameName()");
		if (!tS_)
		{
			FindScripts();
		}
		if (!msg.ip)
		{
			if (tS_.npcGameNameInUse.Length > msg.slot)
			{
				tS_.npcGameNameInUse[msg.slot] = true;
			}
		}
		else if (tS_.npcIPsInUse.Length > msg.slot)
		{
			tS_.npcIPsInUse[msg.slot] = true;
		}
		SERVER_Send_NpcGameName(msg.slot, msg.ip);
	}

	public void CLIENT_Send_GameSpeed(int i)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameSpeed()");
			NetworkClient.Send(new c_GameSpeed
			{
				speed = i
			});
		}
	}

	public void CLIENT_Get_GameSpeed(NetworkConnection conn, c_GameSpeed msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_GameSpeed()");
			FindScripts();
			if ((bool)guiMain_ && !guiMain_.uiObjects[155].activeSelf && !guiMain_.uiObjects[202].activeSelf)
			{
				guiMain_.BUTTON_GameSpeed(msg.speed);
			}
		}
	}

	public void CLIENT_Send_Trend()
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Trend()");
			FindScripts();
			NetworkClient.Send(new c_Trend
			{
				trendWeeks = mS_.trendWeeks,
				trendTheme = mS_.trendTheme,
				trendAntiTheme = mS_.trendAntiTheme,
				trendGenre = mS_.trendGenre,
				trendAntiGenre = mS_.trendAntiGenre
			});
		}
	}

	public void CLIENT_Get_Trend(NetworkConnection conn, c_Trend msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_Trend()");
			FindScripts();
			mS_.trendWeeks = msg.trendWeeks;
			mS_.trendTheme = msg.trendTheme;
			mS_.trendAntiTheme = msg.trendAntiTheme;
			mS_.trendGenre = msg.trendGenre;
			mS_.trendAntiGenre = msg.trendAntiGenre;
			mS_.ShowTrendNews();
			SERVER_Send_Trend();
		}
	}

	public void CLIENT_Send_PublisherTochterfirmaSettings(publisherScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_PublisherTochterfirmaSettings()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_PublisherTochterfirmaSettings
				{
					myID = script_.myID,
					tf_geschlossen = script_.tf_geschlossen,
					tf_autoRelease = script_.tf_autoRelease,
					tf_onlyPlayerConsole = script_.tf_onlyPlayerConsole,
					tf_allowMMO = script_.tf_allowMMO,
					tf_allowF2P = script_.tf_allowF2P,
					tf_allowAddon = script_.tf_allowAddon,
					tf_noArcade = script_.tf_noArcade,
					tf_noHandy = script_.tf_noHandy,
					tf_noRetro = script_.tf_noRetro,
					tf_noPorts = script_.tf_noPorts,
					tf_noBudget = script_.tf_noBudget,
					tf_noGOTY = script_.tf_noGOTY,
					tf_noBundles = script_.tf_noBundles,
					tf_noAddonBundles = script_.tf_noAddonBundles,
					tf_noRemaster = script_.tf_noRemaster,
					tf_noSpinoffs = script_.tf_noSpinoffs,
					tf_autoGamePass = script_.tf_autoGamePass,
					tf_ownPublisher = script_.tf_ownPublisher,
					tf_publisher = script_.tf_publisher,
					tf_developer = script_.tf_developer,
					tf_entwicklungsdauer = script_.tf_entwicklungsdauer,
					tf_gameSize = script_.tf_gameSize,
					tf_gameGenre = script_.tf_gameGenre,
					tf_gameTopic = script_.tf_gameTopic,
					tf_engine = script_.tf_engine,
					tf_autoReleaseVal = script_.tf_autoReleaseVal,
					tf_ownPublisherPriority = script_.tf_ownPublisherPriority,
					tf_ipFocus = (int[])script_.tf_ipFocus.Clone(),
					tf_platformFocus = (int[])script_.tf_platformFocus.Clone()
				});
			}
			else
			{
				Debug.Log("ERROR: CLIENT_Send_PublisherTochterfirmaSettings() -> Missing PublisherScript");
			}
		}
	}

	public void CLIENT_Get_PublisherTochterfirmaSettings(NetworkConnection conn, c_PublisherTochterfirmaSettings msg)
	{
		if (!isServer || save_.loadingSavegame)
		{
			return;
		}
		Debug.Log("CLIENT_Get_PublisherTochterfirmaSettings()");
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPublisherScripts[i].gameObject;
				break;
			}
		}
		publisherScript publisherScript2 = null;
		if ((bool)gameObject)
		{
			publisherScript2 = gameObject.GetComponent<publisherScript>();
			if ((bool)publisherScript2)
			{
				publisherScript2.tf_geschlossen = msg.tf_geschlossen;
				publisherScript2.tf_autoRelease = msg.tf_autoRelease;
				publisherScript2.tf_onlyPlayerConsole = msg.tf_onlyPlayerConsole;
				publisherScript2.tf_allowMMO = msg.tf_allowMMO;
				publisherScript2.tf_allowF2P = msg.tf_allowF2P;
				publisherScript2.tf_allowAddon = msg.tf_allowAddon;
				publisherScript2.tf_noArcade = msg.tf_noArcade;
				publisherScript2.tf_noHandy = msg.tf_noHandy;
				publisherScript2.tf_noRetro = msg.tf_noRetro;
				publisherScript2.tf_noPorts = msg.tf_noPorts;
				publisherScript2.tf_noBudget = msg.tf_noBudget;
				publisherScript2.tf_noGOTY = msg.tf_noGOTY;
				publisherScript2.tf_noBundles = msg.tf_noBundles;
				publisherScript2.tf_noAddonBundles = msg.tf_noAddonBundles;
				publisherScript2.tf_noRemaster = msg.tf_noRemaster;
				publisherScript2.tf_noSpinoffs = msg.tf_noSpinoffs;
				publisherScript2.tf_autoGamePass = msg.tf_autoGamePass;
				publisherScript2.tf_ownPublisher = msg.tf_ownPublisher;
				publisherScript2.tf_publisher = msg.tf_publisher;
				publisherScript2.tf_developer = msg.tf_developer;
				publisherScript2.tf_entwicklungsdauer = msg.tf_entwicklungsdauer;
				publisherScript2.tf_gameSize = msg.tf_gameSize;
				publisherScript2.tf_gameGenre = msg.tf_gameGenre;
				publisherScript2.tf_gameTopic = msg.tf_gameTopic;
				publisherScript2.tf_engine = msg.tf_engine;
				publisherScript2.tf_autoReleaseVal = msg.tf_autoReleaseVal;
				publisherScript2.tf_ownPublisherPriority = msg.tf_ownPublisherPriority;
				publisherScript2.tf_ipFocus = (int[])msg.tf_ipFocus.Clone();
				publisherScript2.tf_platformFocus = (int[])msg.tf_platformFocus.Clone();
				SERVER_Send_PublisherTochterfirmaSettings(publisherScript2);
			}
		}
	}

	public void CLIENT_Send_BlockContractGame(gameScript script_, bool block_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_BlockContractGame()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_BlockContractGame
				{
					myID = script_.myID,
					block = block_
				});
			}
			else
			{
				Debug.Log("ERROR: CLIENT_Send_BlockContractGame() -> Missing GameScript");
			}
		}
	}

	public void CLIENT_Get_BlockContractGame(NetworkConnection conn, c_BlockContractGame msg)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!games_)
		{
			FindScripts();
		}
		if (save_.loadingSavegame)
		{
			return;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == msg.myID)
			{
				games_.arrayGamesScripts[i].auftragsspiel_Blocked = msg.block;
				SERVER_Send_BlockContractGame(games_.arrayGamesScripts[i], msg.block);
				break;
			}
		}
	}

	public void CLIENT_Send_PublisherOwner(publisherScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_PublisherOwner()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_PublisherOwner
				{
					myID = script_.myID,
					ownerID = script_.ownerID
				});
			}
			else
			{
				Debug.Log("ERROR: CLIENT_Send_PublisherOwner() -> Missing PublisherScript");
			}
		}
	}

	public void CLIENT_Get_PublisherOwner(NetworkConnection conn, c_PublisherOwner msg)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (save_.loadingSavegame)
		{
			return;
		}
		if (mS_.exklusivVertrag_ID == msg.myID)
		{
			mS_.RemovePublisherExklusivVertrag();
		}
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPublisherScripts[i].gameObject;
				break;
			}
		}
		publisherScript publisherScript2 = null;
		if (!gameObject)
		{
			return;
		}
		publisherScript2 = gameObject.GetComponent<publisherScript>();
		if ((bool)publisherScript2)
		{
			publisherScript2.ownerID = msg.ownerID;
			if (msg.ownerID == -1)
			{
				publisherScript2.date_year_end = mS_.year + Random.Range(4, 10);
			}
			SERVER_Send_PublisherOwner(publisherScript2);
		}
	}

	public void CLIENT_Send_Publisher(publisherScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Publisher()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_Publisher
				{
					myID = script_.myID,
					isUnlocked = script_.isUnlocked,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_JA = script_.name_JA,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					date_year = script_.date_year,
					date_month = script_.date_month,
					stars = script_.stars,
					logoID = script_.logoID,
					developer = script_.developer,
					publisher = script_.publisher,
					onlyMobile = script_.onlyMobile,
					share = script_.share,
					fanGenre = script_.fanGenre,
					firmenwert = script_.firmenwert,
					notForSale = script_.notForSale,
					lockToBuy = script_.lockToBuy,
					isPlayer = script_.isPlayer,
					ownerID = script_.ownerID,
					country = script_.country,
					ownPlatform = script_.ownPlatform,
					exklusive = script_.exklusive,
					awards = (int[])script_.awards.Clone()
				});
			}
			else
			{
				Debug.Log("ERROR: CLIENT_Send_Publisher() -> Missing PublisherScript");
			}
		}
	}

	public void CLIENT_Get_Publisher(NetworkConnection conn, c_Publisher msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Publisher()");
		if (!mS_)
		{
			FindScripts();
		}
		if (save_.loadingSavegame)
		{
			return;
		}
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPublisherScripts[i].gameObject;
				break;
			}
		}
		publisherScript publisherScript2 = null;
		if ((bool)gameObject)
		{
			publisherScript2 = gameObject.GetComponent<publisherScript>();
			if ((bool)publisherScript2)
			{
				publisherScript2.myID = msg.myID;
				publisherScript2.isUnlocked = msg.isUnlocked;
				publisherScript2.name_EN = msg.name_EN;
				publisherScript2.name_GE = msg.name_GE;
				publisherScript2.name_TU = msg.name_TU;
				publisherScript2.name_CH = msg.name_CH;
				publisherScript2.name_FR = msg.name_FR;
				publisherScript2.name_JA = msg.name_JA;
				publisherScript2.name_UA = msg.name_UA;
				publisherScript2.name_TH = msg.name_TH;
				publisherScript2.date_year = msg.date_year;
				publisherScript2.date_month = msg.date_month;
				publisherScript2.stars = msg.stars;
				publisherScript2.logoID = msg.logoID;
				publisherScript2.developer = msg.developer;
				publisherScript2.publisher = msg.publisher;
				publisherScript2.onlyMobile = msg.onlyMobile;
				publisherScript2.share = msg.share;
				publisherScript2.fanGenre = msg.fanGenre;
				publisherScript2.notForSale = msg.notForSale;
				publisherScript2.lockToBuy = msg.lockToBuy;
				publisherScript2.isPlayer = msg.isPlayer;
				publisherScript2.ownerID = msg.ownerID;
				publisherScript2.country = msg.country;
				publisherScript2.ownPlatform = msg.ownPlatform;
				publisherScript2.exklusive = msg.exklusive;
				publisherScript2.awards = (int[])msg.awards.Clone();
				SERVER_Send_Publisher(publisherScript2);
			}
		}
	}

	public void CLIENT_Send_Payment(int toPlayer, int what, int money, int objectID)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Payment()");
			FindScripts();
			NetworkClient.Send(new c_Payment
			{
				playerID = mS_.myID,
				toPlayerID = toPlayer,
				what = what,
				money = money,
				objectID = objectID
			});
		}
	}

	public void CLIENT_Get_Payment(NetworkConnection conn, c_Payment msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Payment()");
		string text = "";
		FindScripts();
		if (msg.toPlayerID == mS_.myID)
		{
			switch (msg.what)
			{
			case 0:
			{
				text = tS_.GetText(1044);
				text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				mS_.Earn(msg.money, 4);
				for (int j = 0; j < mS_.arrayEnginesScripts.Length; j++)
				{
					if ((bool)mS_.arrayEnginesScripts[j] && mS_.arrayEnginesScripts[j].myID == msg.objectID)
					{
						mS_.arrayEnginesScripts[j].umsatz += msg.money;
						break;
					}
				}
				break;
			}
			case 1:
			{
				text = tS_.GetText(1045);
				text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				mS_.Earn(msg.money, 4);
				for (int k = 0; k < mS_.arrayEnginesScripts.Length; k++)
				{
					if ((bool)mS_.arrayEnginesScripts[k] && mS_.arrayEnginesScripts[k].myID == msg.objectID)
					{
						mS_.arrayEnginesScripts[k].umsatz += msg.money;
						mS_.arrayEnginesScripts[k].amountSellToPlayer++;
						break;
					}
				}
				break;
			}
			case 2:
				text = tS_.GetText(1658);
				text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				mS_.Earn(msg.money, 10);
				break;
			case 3:
			{
				mS_.Earn(msg.money, 4);
				for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
				{
					if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.objectID)
					{
						mS_.arrayEnginesScripts[i].umsatz += msg.money;
						break;
					}
				}
				break;
			}
			case 4:
				mS_.Earn(msg.money, 10);
				break;
			case 5:
				text = tS_.GetText(2249);
				text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				mS_.Earn(msg.money, 31);
				break;
			}
		}
		else
		{
			SERVER_Send_Payment(msg.playerID, msg.toPlayerID, msg.what, msg.money, msg.objectID);
		}
	}

	public void CLIENT_Send_Forschung()
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("CLIENT_Send_Forschung()");
		FindScripts();
		player_mp player_mp2 = FindPlayer(mS_.myID);
		if (player_mp2 == null)
		{
			return;
		}
		if (player_mp2.forschungSonstiges.Length != fS_.RES_POINTS.Length)
		{
			player_mp2.forschungSonstiges = new bool[fS_.RES_POINTS.Length];
		}
		if (player_mp2.genres.Length != genres_.genres_UNLOCK.Length)
		{
			player_mp2.genres = new bool[genres_.genres_UNLOCK.Length];
		}
		if (player_mp2.themes.Length != themes_.themes_RES_POINTS_LEFT.Length)
		{
			player_mp2.themes = new bool[themes_.themes_RES_POINTS_LEFT.Length];
		}
		if (player_mp2.engineFeatures.Length != eF_.engineFeatures_RES_POINTS.Length)
		{
			player_mp2.engineFeatures = new bool[eF_.engineFeatures_RES_POINTS.Length];
		}
		if (player_mp2.gameplayFeatures.Length != gF_.gameplayFeatures_RES_POINTS.Length)
		{
			player_mp2.gameplayFeatures = new bool[gF_.gameplayFeatures_RES_POINTS.Length];
		}
		if (player_mp2.hardware.Length != hardware_.hardware_RES_POINTS.Length)
		{
			player_mp2.hardware = new bool[hardware_.hardware_RES_POINTS.Length];
		}
		if (player_mp2.hardwareFeatures.Length != hardwareFeatures_.hardFeat_RES_POINTS.Length)
		{
			player_mp2.hardwareFeatures = new bool[hardwareFeatures_.hardFeat_RES_POINTS.Length];
		}
		for (int i = 0; i < player_mp2.forschungSonstiges.Length; i++)
		{
			if (fS_.RES_POINTS_LEFT[i] <= 0f)
			{
				player_mp2.forschungSonstiges[i] = true;
			}
			else
			{
				player_mp2.forschungSonstiges[i] = false;
			}
		}
		for (int j = 0; j < player_mp2.genres.Length; j++)
		{
			if (genres_.genres_RES_POINTS_LEFT[j] <= 0f)
			{
				player_mp2.genres[j] = true;
			}
			else
			{
				player_mp2.genres[j] = false;
			}
		}
		for (int k = 0; k < player_mp2.themes.Length; k++)
		{
			if (themes_.themes_RES_POINTS_LEFT[k] <= 0f)
			{
				player_mp2.themes[k] = true;
			}
			else
			{
				player_mp2.themes[k] = false;
			}
		}
		for (int l = 0; l < player_mp2.engineFeatures.Length; l++)
		{
			if (eF_.engineFeatures_RES_POINTS_LEFT[l] <= 0f)
			{
				player_mp2.engineFeatures[l] = true;
			}
			else
			{
				player_mp2.engineFeatures[l] = false;
			}
		}
		for (int m = 0; m < player_mp2.gameplayFeatures.Length; m++)
		{
			if (gF_.gameplayFeatures_RES_POINTS_LEFT[m] <= 0f)
			{
				player_mp2.gameplayFeatures[m] = true;
			}
			else
			{
				player_mp2.gameplayFeatures[m] = false;
			}
		}
		for (int n = 0; n < player_mp2.hardware.Length; n++)
		{
			if (hardware_.hardware_RES_POINTS_LEFT[n] <= 0f)
			{
				player_mp2.hardware[n] = true;
			}
			else
			{
				player_mp2.hardware[n] = false;
			}
		}
		for (int num = 0; num < player_mp2.hardwareFeatures.Length; num++)
		{
			if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[num] <= 0f)
			{
				player_mp2.hardwareFeatures[num] = true;
			}
			else
			{
				player_mp2.hardwareFeatures[num] = false;
			}
		}
		NetworkClient.Send(new c_Forschung
		{
			playerID = mS_.myID,
			forschungSonstiges = (bool[])player_mp2.forschungSonstiges.Clone(),
			genres = (bool[])player_mp2.genres.Clone(),
			themes = (bool[])player_mp2.themes.Clone(),
			engineFeatures = (bool[])player_mp2.engineFeatures.Clone(),
			gameplayFeatures = (bool[])player_mp2.gameplayFeatures.Clone(),
			hardware = (bool[])player_mp2.hardware.Clone(),
			hardwareFeatures = (bool[])player_mp2.hardwareFeatures.Clone()
		});
	}

	public void CLIENT_Get_Forschung(NetworkConnection conn, c_Forschung msg)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		Debug.Log("CLIENT_Get_Forschung()");
		if (!save_.loadingSavegame)
		{
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.forschungSonstiges = (bool[])msg.forschungSonstiges.Clone();
				player_mp2.genres = (bool[])msg.genres.Clone();
				player_mp2.themes = (bool[])msg.themes.Clone();
				player_mp2.engineFeatures = (bool[])msg.engineFeatures.Clone();
				player_mp2.gameplayFeatures = (bool[])msg.gameplayFeatures.Clone();
				player_mp2.hardware = (bool[])msg.hardware.Clone();
				player_mp2.hardwareFeatures = (bool[])msg.hardwareFeatures.Clone();
				SERVER_Send_Forschung(msg.playerID);
			}
		}
	}

	public void CLIENT_Send_Help(int toPlayer, int what, int valueA, int valueB, int valueC)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Help()");
			FindScripts();
			NetworkClient.Send(new c_Help
			{
				playerID = mS_.myID,
				toPlayerID = toPlayer,
				what = what,
				valueA = valueA,
				valueB = valueB,
				valueC = valueC
			});
		}
	}

	public void CLIENT_Get_Help(NetworkConnection conn, c_Help msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Help()");
		string text = "";
		FindScripts();
		if (msg.toPlayerID == mS_.myID)
		{
			switch (msg.what)
			{
			case 0:
				text = tS_.GetText(1327);
				text = text.Replace("<NUM>", mS_.GetMoney(msg.valueA, showDollar: true));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				mS_.Earn(msg.valueA, 1);
				break;
			case 1:
			{
				GameObject gameObject = null;
				for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
				{
					if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.valueA)
					{
						gameObject = mS_.arrayEnginesScripts[i].gameObject;
						break;
					}
				}
				if ((bool)gameObject)
				{
					engineScript component = gameObject.GetComponent<engineScript>();
					if ((bool)component && !component.gekauft)
					{
						component.gekauft = true;
						text = tS_.GetText(1330);
						text = text.Replace("<NAME>", component.GetName());
						guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
					}
				}
				break;
			}
			case 2:
				text = tS_.GetText(1332);
				text = text.Replace("<NAME>", licences_.GetName(msg.valueA));
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				licences_.licence_GEKAUFT[msg.valueA] += msg.valueB;
				break;
			case 3:
				text = tS_.GetText(1339);
				switch (msg.valueB)
				{
				case 0:
					text = text.Replace("<NAME>", genres_.GetName(msg.valueA));
					genres_.genres_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 1:
					text = text.Replace("<NAME>", tS_.GetThemes(msg.valueA));
					themes_.themes_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 2:
					text = text.Replace("<NAME>", eF_.GetName(msg.valueA));
					eF_.engineFeatures_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 3:
					text = text.Replace("<NAME>", gF_.GetName(msg.valueA));
					gF_.gameplayFeatures_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 4:
					text = text.Replace("<NAME>", hardware_.GetName(msg.valueA));
					hardware_.hardware_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 5:
					text = text.Replace("<NAME>", fS_.GetName(msg.valueA));
					fS_.RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				case 6:
					text = text.Replace("<NAME>", hardwareFeatures_.GetName(msg.valueA));
					hardwareFeatures_.hardFeat_RES_POINTS_LEFT[msg.valueA] = 0f;
					break;
				}
				SERVER_Send_Forschung(mS_.myID);
				guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				break;
			case 4:
				mS_.sabotage_pr = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(0);
				break;
			case 5:
				mS_.sabotage_motivation = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(1);
				break;
			case 6:
				mS_.sabotage_klage = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(2);
				break;
			case 7:
				mS_.sabotage_reviews = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(3);
				break;
			case 8:
				mS_.sabotage_geruecht = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(4);
				break;
			case 9:
				mS_.sabotage_work = 24;
				guiMain_.uiObjects[443].SetActive(value: true);
				guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(5);
				break;
			}
		}
		else
		{
			SERVER_Send_Help(msg.playerID, msg.toPlayerID, msg.what, msg.valueA, msg.valueB, msg.valueC);
		}
	}

	public void CLIENT_Send_PlatformRemoveFromMarket(platformScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_RemovePlatformFromMarket()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_PlatformRemoveFromMarket
				{
					platformID = script_.myID
				});
			}
		}
	}

	public void CLIENT_Get_PlatformRemoveFromMarket(NetworkConnection conn, c_PlatformRemoveFromMarket msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_PlatformRemoveFromMarket()");
		FindScripts();
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == msg.platformID)
			{
				mS_.arrayPlatformsScripts[i].RemoveFromMarket();
				SERVER_Send_PlatformRemoveFromMarket(mS_.arrayPlatformsScripts[i]);
				break;
			}
		}
	}

	public void CLIENT_Send_Platform(platformScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Platform()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_Platform
				{
					myID = script_.myID,
					date_year = script_.date_year,
					date_month = script_.date_month,
					date_year_end = script_.date_year_end,
					date_month_end = script_.date_month_end,
					price = script_.price,
					dev_costs = script_.dev_costs,
					tech = script_.tech,
					typ = script_.typ,
					marktanteil = script_.marktanteil,
					needFeatures = (int[])script_.needFeatures.Clone(),
					units = script_.units,
					units_max = script_.units_max,
					minGamePassGames = script_.minGamePassGames,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_HU = script_.name_HU,
					name_JA = script_.name_JA,
					name_PL = script_.name_PL,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					manufacturer_EN = script_.manufacturer_EN,
					manufacturer_GE = script_.manufacturer_GE,
					manufacturer_TU = script_.manufacturer_TU,
					manufacturer_CH = script_.manufacturer_CH,
					manufacturer_FR = script_.manufacturer_FR,
					manufacturer_HU = script_.manufacturer_HU,
					manufacturer_JA = script_.manufacturer_JA,
					manufacturer_PL = script_.manufacturer_PL,
					manufacturer_UA = script_.manufacturer_UA,
					manufacturer_TH = script_.manufacturer_TH,
					pic1_file = script_.pic1_file,
					pic2_file = script_.pic2_file,
					pic2_year = script_.pic2_year,
					games = script_.games,
					exklusivGames = script_.exklusivGames,
					isUnlocked = script_.isUnlocked,
					vomMarktGenommen = script_.vomMarktGenommen,
					complex = script_.complex,
					internet = script_.internet,
					powerFromMarket = script_.powerFromMarket,
					myName = script_.myName,
					ownerID = script_.ownerID,
					gameID = script_.gameID,
					anzController = script_.anzController,
					consoleColor = script_.consoleColor,
					component_cpu = script_.component_cpu,
					component_gfx = script_.component_gfx,
					component_ram = script_.component_ram,
					component_hdd = script_.component_hdd,
					component_sfx = script_.component_sfx,
					component_cooling = script_.component_cooling,
					component_disc = script_.component_disc,
					component_controller = script_.component_controller,
					component_case = script_.component_case,
					component_monitor = script_.component_monitor,
					hwFeatures = (bool[])script_.hwFeatures.Clone(),
					entwicklungsKosten = script_.entwicklungsKosten,
					einnahmen = script_.einnahmen,
					hype = script_.hype,
					startProduktionskosten = script_.startProduktionskosten,
					verkaufspreis = script_.verkaufspreis,
					kostenreduktion = script_.kostenreduktion,
					autoPreis = script_.autoPreis,
					thridPartyGames = script_.thridPartyGames,
					umsatzTotal = script_.umsatzTotal,
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					weeksOnMarket = script_.weeksOnMarket,
					review = script_.review,
					performancePoints = script_.performancePoints,
					vorgaengerID = script_.vorgaengerID,
					nachfolgerID = script_.nachfolgerID,
					proVersion = script_.proVersion,
					proName = script_.proName,
					subventionMoney = script_.subventionMoney,
					subventionGameSize = (bool[])script_.subventionGameSize.Clone()
				});
			}
		}
	}

	public void CLIENT_Get_Platform(NetworkConnection conn, c_Platform msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Platform()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPlatformsScripts[i].gameObject;
				break;
			}
		}
		platformScript platformScript2 = null;
		if ((bool)gameObject)
		{
			platformScript2 = gameObject.GetComponent<platformScript>();
			if ((bool)platformScript2)
			{
				if (platformScript2.ownerID == mS_.myID)
				{
					return;
				}
				platformScript2.myID = msg.myID;
				platformScript2.date_year = msg.date_year;
				platformScript2.date_month = msg.date_month;
				platformScript2.date_year_end = msg.date_year_end;
				platformScript2.date_month_end = msg.date_month_end;
				platformScript2.price = msg.price;
				platformScript2.dev_costs = msg.dev_costs;
				platformScript2.tech = msg.tech;
				platformScript2.typ = msg.typ;
				platformScript2.needFeatures = (int[])msg.needFeatures.Clone();
				platformScript2.units = msg.units;
				platformScript2.units_max = msg.units_max;
				platformScript2.minGamePassGames = msg.minGamePassGames;
				platformScript2.name_EN = msg.name_EN;
				platformScript2.name_GE = msg.name_GE;
				platformScript2.name_TU = msg.name_TU;
				platformScript2.name_CH = msg.name_CH;
				platformScript2.name_FR = msg.name_FR;
				platformScript2.name_HU = msg.name_HU;
				platformScript2.name_JA = msg.name_JA;
				platformScript2.name_PL = msg.name_PL;
				platformScript2.name_UA = msg.name_UA;
				platformScript2.name_TH = msg.name_TH;
				platformScript2.manufacturer_EN = msg.manufacturer_EN;
				platformScript2.manufacturer_GE = msg.manufacturer_GE;
				platformScript2.manufacturer_TU = msg.manufacturer_TU;
				platformScript2.manufacturer_CH = msg.manufacturer_CH;
				platformScript2.manufacturer_FR = msg.manufacturer_FR;
				platformScript2.manufacturer_HU = msg.manufacturer_HU;
				platformScript2.manufacturer_JA = msg.manufacturer_JA;
				platformScript2.manufacturer_PL = msg.manufacturer_PL;
				platformScript2.manufacturer_UA = msg.manufacturer_UA;
				platformScript2.manufacturer_TH = msg.manufacturer_TH;
				platformScript2.pic1_file = msg.pic1_file;
				platformScript2.pic2_file = msg.pic2_file;
				platformScript2.pic2_year = msg.pic2_year;
				platformScript2.games = msg.games;
				platformScript2.exklusivGames = msg.exklusivGames;
				platformScript2.isUnlocked = msg.isUnlocked;
				platformScript2.vomMarktGenommen = msg.vomMarktGenommen;
				platformScript2.complex = msg.complex;
				platformScript2.internet = msg.internet;
				platformScript2.powerFromMarket = msg.powerFromMarket;
				platformScript2.myName = msg.myName;
				platformScript2.ownerID = msg.ownerID;
				platformScript2.gameID = msg.gameID;
				platformScript2.anzController = msg.anzController;
				platformScript2.consoleColor = msg.consoleColor;
				platformScript2.component_cpu = msg.component_cpu;
				platformScript2.component_gfx = msg.component_gfx;
				platformScript2.component_ram = msg.component_ram;
				platformScript2.component_hdd = msg.component_hdd;
				platformScript2.component_sfx = msg.component_sfx;
				platformScript2.component_cooling = msg.component_cooling;
				platformScript2.component_disc = msg.component_disc;
				platformScript2.component_controller = msg.component_controller;
				platformScript2.component_case = msg.component_case;
				platformScript2.component_monitor = msg.component_monitor;
				platformScript2.hwFeatures = (bool[])msg.hwFeatures.Clone();
				platformScript2.entwicklungsKosten = msg.entwicklungsKosten;
				platformScript2.einnahmen = msg.einnahmen;
				platformScript2.hype = msg.hype;
				platformScript2.startProduktionskosten = msg.startProduktionskosten;
				platformScript2.verkaufspreis = msg.verkaufspreis;
				platformScript2.kostenreduktion = msg.kostenreduktion;
				platformScript2.autoPreis = msg.autoPreis;
				platformScript2.thridPartyGames = msg.thridPartyGames;
				platformScript2.umsatzTotal = msg.umsatzTotal;
				platformScript2.weeksOnMarket = msg.weeksOnMarket;
				platformScript2.review = msg.review;
				platformScript2.performancePoints = msg.performancePoints;
				platformScript2.vorgaengerID = msg.vorgaengerID;
				platformScript2.nachfolgerID = msg.nachfolgerID;
				platformScript2.proVersion = msg.proVersion;
				platformScript2.proName = msg.proName;
				platformScript2.subventionMoney = msg.subventionMoney;
				platformScript2.subventionGameSize = (bool[])msg.subventionGameSize.Clone();
			}
		}
		else
		{
			platformScript2 = platforms_.CreatePlatform();
			platformScript2.myID = msg.myID;
			platformScript2.date_year = msg.date_year;
			platformScript2.date_month = msg.date_month;
			platformScript2.date_year_end = msg.date_year_end;
			platformScript2.date_month_end = msg.date_month_end;
			platformScript2.price = msg.price;
			platformScript2.dev_costs = msg.dev_costs;
			platformScript2.tech = msg.tech;
			platformScript2.typ = msg.typ;
			platformScript2.needFeatures = (int[])msg.needFeatures.Clone();
			platformScript2.units = msg.units;
			platformScript2.units_max = msg.units_max;
			platformScript2.minGamePassGames = msg.minGamePassGames;
			platformScript2.name_EN = msg.name_EN;
			platformScript2.name_GE = msg.name_GE;
			platformScript2.name_TU = msg.name_TU;
			platformScript2.name_CH = msg.name_CH;
			platformScript2.name_FR = msg.name_FR;
			platformScript2.name_HU = msg.name_HU;
			platformScript2.name_JA = msg.name_JA;
			platformScript2.name_PL = msg.name_PL;
			platformScript2.name_UA = msg.name_UA;
			platformScript2.name_TH = msg.name_TH;
			platformScript2.manufacturer_EN = msg.manufacturer_EN;
			platformScript2.manufacturer_GE = msg.manufacturer_GE;
			platformScript2.manufacturer_TU = msg.manufacturer_TU;
			platformScript2.manufacturer_CH = msg.manufacturer_CH;
			platformScript2.manufacturer_FR = msg.manufacturer_FR;
			platformScript2.manufacturer_HU = msg.manufacturer_HU;
			platformScript2.manufacturer_JA = msg.manufacturer_JA;
			platformScript2.manufacturer_PL = msg.manufacturer_PL;
			platformScript2.manufacturer_UA = msg.manufacturer_UA;
			platformScript2.manufacturer_TH = msg.manufacturer_TH;
			platformScript2.pic1_file = msg.pic1_file;
			platformScript2.pic2_file = msg.pic2_file;
			platformScript2.pic2_year = msg.pic2_year;
			platformScript2.games = msg.games;
			platformScript2.exklusivGames = msg.exklusivGames;
			platformScript2.isUnlocked = msg.isUnlocked;
			platformScript2.vomMarktGenommen = msg.vomMarktGenommen;
			platformScript2.complex = msg.complex;
			platformScript2.internet = msg.internet;
			platformScript2.powerFromMarket = msg.powerFromMarket;
			platformScript2.myName = msg.myName;
			platformScript2.ownerID = msg.ownerID;
			platformScript2.gameID = msg.gameID;
			platformScript2.anzController = msg.anzController;
			platformScript2.consoleColor = msg.consoleColor;
			platformScript2.component_cpu = msg.component_cpu;
			platformScript2.component_gfx = msg.component_gfx;
			platformScript2.component_ram = msg.component_ram;
			platformScript2.component_hdd = msg.component_hdd;
			platformScript2.component_sfx = msg.component_sfx;
			platformScript2.component_cooling = msg.component_cooling;
			platformScript2.component_disc = msg.component_disc;
			platformScript2.component_controller = msg.component_controller;
			platformScript2.component_case = msg.component_case;
			platformScript2.component_monitor = msg.component_monitor;
			platformScript2.hwFeatures = (bool[])msg.hwFeatures.Clone();
			platformScript2.entwicklungsKosten = msg.entwicklungsKosten;
			platformScript2.einnahmen = msg.einnahmen;
			platformScript2.hype = msg.hype;
			platformScript2.startProduktionskosten = msg.startProduktionskosten;
			platformScript2.verkaufspreis = msg.verkaufspreis;
			platformScript2.kostenreduktion = msg.kostenreduktion;
			platformScript2.autoPreis = msg.autoPreis;
			platformScript2.thridPartyGames = msg.thridPartyGames;
			platformScript2.umsatzTotal = msg.umsatzTotal;
			platformScript2.weeksOnMarket = msg.weeksOnMarket;
			platformScript2.review = msg.review;
			platformScript2.performancePoints = msg.performancePoints;
			platformScript2.vorgaengerID = msg.vorgaengerID;
			platformScript2.nachfolgerID = msg.nachfolgerID;
			platformScript2.proVersion = msg.proVersion;
			platformScript2.proName = msg.proName;
			platformScript2.subventionMoney = msg.subventionMoney;
			platformScript2.subventionGameSize = (bool[])msg.subventionGameSize.Clone();
			platformScript2.Init();
			if (!platformScript2.OwnerIsNPC() && !platformScript2.vomMarktGenommen)
			{
				guiMain_.CreateTopNewsPlatform(platformScript2);
				string text = tS_.GetText(1629);
				text = text.Replace("<NAME>", msg.myName);
				guiMain_.AddChat(msg.ownerID, text);
			}
			if (!platformScript2.OwnerIsNPC() && platformScript2.vomMarktGenommen)
			{
				guiMain_.CreateTopNewsPlatformRemove(platformScript2);
			}
		}
		SERVER_Send_Platform(platformScript2);
	}

	public void CLIENT_Send_EnginePublisherBuyed(engineScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_EnginePublisherBuyed()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_EnginePublisherBuyed
				{
					myID = script_.myID,
					publisherBuyed = (bool[])script_.publisherBuyed.Clone()
				});
			}
		}
	}

	public void CLIENT_Get_EnginePublisherBuyed(NetworkConnection conn, c_EnginePublisherBuyed msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_EnginePublisherBuyed()");
		FindScripts();
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.myID)
			{
				mS_.arrayEnginesScripts[i].publisherBuyed = (bool[])msg.publisherBuyed.Clone();
				SERVER_Send_EnginePublisherBuyed(mS_.arrayEnginesScripts[i]);
				break;
			}
		}
	}

	public void CLIENT_Send_Engine(engineScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Engine()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_Engine
				{
					myID = script_.myID,
					ownerID = script_.ownerID,
					isUnlocked = script_.isUnlocked,
					gekauft = script_.gekauft,
					myName = script_.myName,
					features = script_.features,
					spezialgenre = script_.spezialgenre,
					spezialplatform = script_.spezialplatform,
					sellEngine = script_.sellEngine,
					preis = script_.preis,
					gewinnbeteiligung = script_.gewinnbeteiligung,
					marktdominanz = script_.marktdominanz
				});
			}
		}
	}

	public void CLIENT_Get_Engine(NetworkConnection conn, c_Engine msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Engine()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayEnginesScripts[i].gameObject;
				break;
			}
		}
		engineScript engineScript2 = null;
		if ((bool)gameObject)
		{
			engineScript2 = gameObject.GetComponent<engineScript>();
			if ((bool)engineScript2)
			{
				if (engineScript2.ownerID == mS_.myID)
				{
					return;
				}
				engineScript2.myID = msg.myID;
				engineScript2.ownerID = msg.ownerID;
				engineScript2.isUnlocked = msg.isUnlocked;
				if (msg.ownerID != -1)
				{
					engineScript2.isUnlocked = true;
				}
				engineScript2.gekauft = msg.gekauft;
				engineScript2.myName = msg.myName;
				engineScript2.features = (bool[])msg.features.Clone();
				engineScript2.spezialgenre = msg.spezialgenre;
				engineScript2.spezialplatform = msg.spezialplatform;
				engineScript2.specialPlatformS_ = null;
				engineScript2.sellEngine = msg.sellEngine;
				engineScript2.preis = msg.preis;
				engineScript2.gewinnbeteiligung = msg.gewinnbeteiligung;
				engineScript2.marktdominanz = msg.marktdominanz;
			}
		}
		else
		{
			engineScript2 = eF_.CreateEngine();
			engineScript2.myID = msg.myID;
			engineScript2.ownerID = msg.ownerID;
			engineScript2.isUnlocked = msg.isUnlocked;
			if (msg.ownerID != -1)
			{
				engineScript2.isUnlocked = true;
			}
			engineScript2.gekauft = msg.gekauft;
			engineScript2.myName = msg.myName;
			engineScript2.features = (bool[])msg.features.Clone();
			engineScript2.spezialgenre = msg.spezialgenre;
			engineScript2.spezialplatform = msg.spezialplatform;
			engineScript2.specialPlatformS_ = null;
			engineScript2.sellEngine = msg.sellEngine;
			engineScript2.preis = msg.preis;
			engineScript2.gewinnbeteiligung = msg.gewinnbeteiligung;
			engineScript2.marktdominanz = msg.marktdominanz;
			engineScript2.Init();
			guiMain_.CreateTopNewsNpcEngine(engineScript2.GetName());
			string text = tS_.GetText(1270);
			text = text.Replace("<NAME>", msg.myName);
			guiMain_.AddChat(msg.ownerID, text);
		}
		SERVER_Send_Engine(engineScript2);
	}

	public void CLIENT_Send_Chat(string c)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Chat()");
			FindScripts();
			NetworkClient.Send(new c_Chat
			{
				playerID = mS_.myID,
				text = c
			});
		}
	}

	public void CLIENT_Get_Chat(NetworkConnection conn, c_Chat msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_Chat()");
			FindScripts();
			guiMain_.AddChat(msg.playerID, msg.text);
			SERVER_Send_Chat(msg.playerID, msg.text);
		}
	}

	public void CLIENT_Send_Money()
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Money()");
			FindScripts();
			NetworkClient.Send(new c_Money
			{
				playerID = mS_.myID,
				money = mS_.money,
				fans = genres_.GetAmountFans()
			});
		}
	}

	public void CLIENT_Get_Money(NetworkConnection conn, c_Money msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Money()");
		FindScripts();
		if (!save_.loadingSavegame)
		{
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.timeout = 0f;
				player_mp2.money = msg.money;
				player_mp2.fans = msg.fans;
				player_mp2.connectionID = conn.connectionId;
			}
		}
	}

	public void CLIENT_Send_ExklusivKonsolenSells(gameScript script_, long i)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_ExklusivKonsolenSells()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_exklusivKonsolenSells
				{
					gameID = script_.myID,
					exklusivKonsolenSells = i
				});
			}
		}
	}

	public void CLIENT_Get_ExklusivKonsolenSells(NetworkConnection conn, c_exklusivKonsolenSells msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_ExklusivKonsolenSells()");
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.myID = msg.gameID;
				component.exklusivKonsolenSells = msg.exklusivKonsolenSells;
				SERVER_Send_ExklusivKonsolenSells(component, msg.exklusivKonsolenSells);
			}
		}
	}

	public void CLIENT_Send_GameIpSell(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameIpSell()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameIpSell
				{
					gameID = script_.myID,
					ipToSell = script_.ipToSell
				});
			}
		}
	}

	public void CLIENT_Get_GameIpSell(NetworkConnection conn, c_GameIpSell msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameIpSell()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.ipToSell = msg.ipToSell;
				SERVER_Send_GameIpSell(component);
			}
		}
	}

	public void CLIENT_Send_GameOwner(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameOwner()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameOwner
				{
					gameID = script_.myID,
					ownerID = script_.ownerID
				});
			}
		}
	}

	public void CLIENT_Get_GameOwner(NetworkConnection conn, c_GameOwner msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameOwner()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.ipToSell = false;
				component.ownerS_ = null;
				component.ownerID = msg.ownerID;
				SERVER_Send_GameOwner(component);
			}
		}
	}

	public void CLIENT_Send_GameDestroy(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameDestroy()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameDestroy
				{
					gameID = script_.myID
				});
			}
		}
	}

	public void CLIENT_Get_GameDestroy(NetworkConnection conn, c_GameDestroy msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameDestroy()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				SERVER_Send_GameDestroy(component);
				component.gameObject.tag = "GameRemoved";
				Object.Destroy(game);
				games_.FindGames();
			}
		}
	}

	public void CLIENT_Send_GameRemoveFromMarket(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameRemoveFromMarket()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameRemoveFromMarket
				{
					gameID = script_.myID
				});
			}
		}
	}

	public void CLIENT_Get_GameRemoveFromMarket(NetworkConnection conn, c_GameRemoveFromMarket msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameRemoveFromMarket()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.RemoveFromMarket();
				SERVER_Send_GameRemoveFromMarket(component);
			}
		}
	}

	public void CLIENT_Send_GameIpPoints(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameIpPoints()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameIpPoints
				{
					gameID = script_.myID,
					ipPunkte = script_.ipPunkte,
					ipTime = script_.ipTime
				});
			}
		}
	}

	public void CLIENT_Get_GameIpPoints(NetworkConnection conn, c_GameIpPoints msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameIpPoints()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.ipPunkte = msg.ipPunkte;
				component.ipTime = msg.ipTime;
				CLIENT_Send_GameIpPoints(component);
			}
		}
	}

	public void CLIENT_Send_GameSell(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_GameSell()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_GameSell
				{
					gameID = script_.myID,
					isOnMarket = script_.isOnMarket,
					weeksOnMarket = script_.weeksOnMarket,
					sellsTotal = script_.sellsTotal,
					sellsTotalStandard = script_.sellsTotalStandard,
					sellsTotalDeluxe = script_.sellsTotalDeluxe,
					sellsTotalCollectors = script_.sellsTotalCollectors,
					sellsTotalOnline = script_.sellsTotalOnline,
					abonnements = script_.abonnements,
					abonnementsWoche = script_.abonnementsWoche,
					bestAbonnements = script_.bestAbonnements,
					exklusivKonsolenSells = script_.exklusivKonsolenSells,
					userPositiv = script_.userPositiv,
					userNegativ = script_.userNegativ,
					costs_entwicklung = script_.costs_entwicklung,
					costs_mitarbeiter = script_.costs_mitarbeiter,
					costs_marketing = script_.costs_marketing,
					costs_enginegebuehren = script_.costs_enginegebuehren,
					costs_server = script_.costs_server,
					costs_production = script_.costs_production,
					costs_updates = script_.costs_updates,
					points_gameplay = script_.points_gameplay,
					points_grafik = script_.points_grafik,
					points_sound = script_.points_sound,
					points_technik = script_.points_technik,
					points_bugs = script_.points_bugs,
					points_bugsInvis = script_.points_bugsInvis,
					umsatzTotal = script_.umsatzTotal,
					umsatzInApp = script_.umsatzInApp,
					umsatzAbos = script_.umsatzAbos,
					bestChartPosition = script_.bestChartPosition,
					lastChartPosition = script_.lastChartPosition,
					f2pInteresse = script_.f2pInteresse,
					mmoInteresse = script_.mmoInteresse,
					vorbestellungen = script_.vorbestellungen,
					realsticPower = script_.realsticPower,
					hype = script_.hype,
					stornierungen = script_.stornierungen,
					commercialFlop = script_.commercialFlop,
					commercialHit = script_.commercialHit,
					freigabeBudget = script_.freigabeBudget,
					releaseDate = script_.releaseDate,
					inAppPurchaseWeek = script_.inAppPurchaseWeek,
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					verkaufspreis = (int[])script_.verkaufspreis.Clone()
				});
			}
		}
	}

	public void CLIENT_Get_GameSell(NetworkConnection conn, c_GameSell msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_GameSell()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.isOnMarket = msg.isOnMarket;
				component.weeksOnMarket = msg.weeksOnMarket;
				component.sellsTotal = msg.sellsTotal;
				component.sellsTotalStandard = msg.sellsTotalStandard;
				component.sellsTotalDeluxe = msg.sellsTotalDeluxe;
				component.sellsTotalCollectors = msg.sellsTotalCollectors;
				component.sellsTotalOnline = msg.sellsTotalOnline;
				component.abonnements = msg.abonnements;
				component.abonnementsWoche = msg.abonnementsWoche;
				component.bestAbonnements = msg.bestAbonnements;
				component.exklusivKonsolenSells = msg.exklusivKonsolenSells;
				component.userPositiv = msg.userPositiv;
				component.userNegativ = msg.userNegativ;
				component.costs_entwicklung = msg.costs_entwicklung;
				component.costs_mitarbeiter = msg.costs_mitarbeiter;
				component.costs_marketing = msg.costs_marketing;
				component.costs_enginegebuehren = msg.costs_enginegebuehren;
				component.costs_server = msg.costs_server;
				component.costs_production = msg.costs_production;
				component.costs_updates = msg.costs_updates;
				component.points_gameplay = msg.points_gameplay;
				component.points_grafik = msg.points_grafik;
				component.points_sound = msg.points_sound;
				component.points_technik = msg.points_technik;
				component.points_bugs = msg.points_bugs;
				component.points_bugsInvis = msg.points_bugsInvis;
				component.umsatzTotal = msg.umsatzTotal;
				component.umsatzInApp = msg.umsatzInApp;
				component.umsatzAbos = msg.umsatzAbos;
				component.bestChartPosition = msg.bestChartPosition;
				component.lastChartPosition = msg.lastChartPosition;
				component.f2pInteresse = msg.f2pInteresse;
				component.mmoInteresse = msg.mmoInteresse;
				component.vorbestellungen = msg.vorbestellungen;
				component.realsticPower = msg.realsticPower;
				component.hype = msg.hype;
				component.stornierungen = msg.stornierungen;
				component.commercialFlop = msg.commercialFlop;
				component.commercialHit = msg.commercialHit;
				component.freigabeBudget = msg.freigabeBudget;
				component.releaseDate = msg.releaseDate;
				component.inAppPurchaseWeek = msg.inAppPurchaseWeek;
				component.sellsPerWeek = (int[])msg.sellsPerWeek.Clone();
				component.verkaufspreis = (int[])msg.verkaufspreis.Clone();
				games_.UpdateChartsWeek();
				guiMain_.UpdateCharts();
				SERVER_Send_GameSell(component);
			}
		}
	}

	public void CLIENT_Send_Game(gameScript script_)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Game()");
			if ((bool)script_)
			{
				NetworkClient.Send(new c_Game
				{
					gameID = script_.myID,
					myName = script_.GetNameSimple(),
					ipName = script_.ipName,
					inDevelopment = script_.inDevelopment,
					developerID = script_.developerID,
					publisherID = script_.publisherID,
					ownerID = script_.ownerID,
					engineID = script_.engineID,
					hype = script_.hype,
					isOnMarket = script_.isOnMarket,
					warBeiAwards = script_.warBeiAwards,
					weeksOnMarket = script_.weeksOnMarket,
					usk = script_.usk,
					freigabeBudget = script_.freigabeBudget,
					reviewGameplay = script_.reviewGameplay,
					reviewGrafik = script_.reviewGrafik,
					reviewSound = script_.reviewSound,
					reviewSteuerung = script_.reviewSteuerung,
					reviewTotal = script_.reviewTotal,
					reviewGameplayText = script_.reviewGameplayText,
					reviewGrafikText = script_.reviewGrafikText,
					reviewSoundText = script_.reviewSoundText,
					reviewSteuerungText = script_.reviewSteuerungText,
					reviewTotalText = script_.reviewTotalText,
					date_year = script_.date_year,
					date_month = script_.date_month,
					date_start_year = script_.date_start_year,
					date_start_month = script_.date_start_month,
					sellsTotal = script_.sellsTotal,
					umsatzTotal = script_.umsatzTotal,
					costs_entwicklung = script_.costs_entwicklung,
					costs_mitarbeiter = script_.costs_mitarbeiter,
					costs_marketing = script_.costs_marketing,
					costs_enginegebuehren = script_.costs_enginegebuehren,
					costs_server = script_.costs_server,
					costs_production = script_.costs_production,
					costs_updates = script_.costs_updates,
					typ_standard = script_.typ_standard,
					typ_nachfolger = script_.typ_nachfolger,
					teile = script_.teile,
					typ_contractGame = script_.typ_contractGame,
					typ_remaster = script_.typ_remaster,
					typ_spinoff = script_.typ_spinoff,
					typ_addon = script_.typ_addon,
					typ_addonStandalone = script_.typ_addonStandalone,
					typ_mmoaddon = script_.typ_mmoaddon,
					typ_bundle = script_.typ_bundle,
					typ_budget = script_.typ_budget,
					typ_bundleAddon = script_.typ_bundleAddon,
					typ_goty = script_.typ_goty,
					originalGameID = script_.originalGameID,
					portID = script_.portID,
					mainIP = script_.mainIP,
					ipPunkte = script_.ipPunkte,
					exklusiv = script_.exklusiv,
					herstellerExklusiv = script_.herstellerExklusiv,
					retro = script_.retro,
					handy = script_.handy,
					arcade = script_.arcade,
					goty = script_.goty,
					nachfolger_created = script_.nachfolger_created,
					remaster_created = script_.remaster_created,
					budget_created = script_.budget_created,
					goty_created = script_.goty_created,
					trendsetter = script_.trendsetter,
					spielbericht = script_.spielbericht,
					amountUpdates = script_.amountUpdates,
					bonusSellsUpdates = script_.bonusSellsUpdates,
					amountAddons = script_.amountAddons,
					bonusSellsAddons = script_.bonusSellsAddons,
					amountMMOAddons = script_.amountMMOAddons,
					addonQuality = script_.addonQuality,
					devAktFeature = script_.devAktFeature,
					devPoints = script_.devPoints,
					devPointsStart = script_.devPointsStart,
					devPoints_Gesamt = script_.devPoints_Gesamt,
					devPointsStart_Gesamt = script_.devPointsStart_Gesamt,
					points_gameplay = script_.points_gameplay,
					points_grafik = script_.points_grafik,
					points_sound = script_.points_sound,
					points_technik = script_.points_technik,
					points_bugs = script_.points_bugs,
					beschreibung = script_.beschreibung,
					gameTyp = script_.gameTyp,
					gameSize = script_.gameSize,
					gameZielgruppe = script_.gameZielgruppe,
					maingenre = script_.maingenre,
					subgenre = script_.subgenre,
					gameMainTheme = script_.gameMainTheme,
					gameSubTheme = script_.gameSubTheme,
					gameLicence = script_.gameLicence,
					gameCopyProtect = script_.gameCopyProtect,
					gameAntiCheat = script_.gameAntiCheat,
					gameAP_Gameplay = script_.gameAP_Gameplay,
					gameAP_Grafik = script_.gameAP_Grafik,
					gameAP_Sound = script_.gameAP_Sound,
					gameAP_Technik = script_.gameAP_Technik,
					gameLanguage = (bool[])script_.gameLanguage.Clone(),
					gameGameplayFeatures = (bool[])script_.gameGameplayFeatures.Clone(),
					gamePlatform = (int[])script_.gamePlatform.Clone(),
					gameEngineFeature = (int[])script_.gameEngineFeature.Clone(),
					gameplayFeatures_DevDone = (bool[])script_.gameplayFeatures_DevDone.Clone(),
					engineFeature_DevDone = (bool[])script_.engineFeature_DevDone.Clone(),
					gameplayStudio = (bool[])script_.gameplayStudio.Clone(),
					grafikStudio = (bool[])script_.grafikStudio.Clone(),
					soundStudio = (bool[])script_.soundStudio.Clone(),
					motionCaptureStudio = (bool[])script_.motionCaptureStudio.Clone(),
					bundleID = (int[])script_.bundleID.Clone(),
					portExist = (bool[])script_.portExist.Clone(),
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					verkaufspreis = (int[])script_.verkaufspreis.Clone(),
					releaseDate = script_.releaseDate,
					abonnements = script_.abonnements,
					abonnementsWoche = script_.abonnementsWoche,
					aboPreis = script_.aboPreis,
					pubOffer = script_.pubOffer,
					pubAngebot = script_.pubAngebot,
					pubAngebot_Weeks = script_.pubAngebot_Weeks,
					pubAngebot_Verhandlung = script_.pubAngebot_Verhandlung,
					pubAngebot_Retail = script_.pubAngebot_Retail,
					pubAngebot_Digital = script_.pubAngebot_Digital,
					pubAngebot_Garantiesumme = script_.pubAngebot_Garantiesumme,
					pubAngebot_Gewinnbeteiligung = script_.pubAngebot_Gewinnbeteiligung,
					auftragsspiel = script_.auftragsspiel,
					auftragsspiel_gehalt = script_.auftragsspiel_gehalt,
					auftragsspiel_bonus = script_.auftragsspiel_bonus,
					auftragsspiel_zeitInWochen = script_.auftragsspiel_zeitInWochen,
					auftragsspiel_wochenAlsAngebot = script_.auftragsspiel_wochenAlsAngebot,
					auftragsspiel_zeitAbgelaufen = script_.auftragsspiel_zeitAbgelaufen,
					auftragsspiel_mindestbewertung = script_.auftragsspiel_mindestbewertung,
					f2pConverted = script_.f2pConverted,
					angekuendigt = script_.angekuendigt,
					subvention = script_.subvention,
					sonderIP = script_.sonderIP,
					sonderIPMindestreview = script_.sonderIPMindestreview,
					myNameTeil1 = script_.myNameTeil1,
					engineGewinnbeteiligung = script_.engineGewinnbeteiligung,
					weeksInDevelopment = script_.weeksInDevelopment,
					userPositiv = script_.userPositiv,
					userNegativ = script_.userNegativ,
					bestAbonnements = script_.bestAbonnements,
					bestChartPosition = script_.bestChartPosition,
					exklusivKonsolenSells = script_.exklusivKonsolenSells,
					lastChartPosition = script_.lastChartPosition,
					freeware = script_.freeware,
					sellsTotalStandard = script_.sellsTotalStandard,
					sellsTotalDeluxe = script_.sellsTotalDeluxe,
					sellsTotalCollectors = script_.sellsTotalCollectors,
					sellsTotalOnline = script_.sellsTotalOnline,
					points_bugsInvis = script_.points_bugsInvis,
					umsatzInApp = script_.umsatzInApp,
					umsatzAbos = script_.umsatzAbos,
					f2pInteresse = script_.f2pInteresse,
					mmoInteresse = script_.mmoInteresse,
					vorbestellungen = script_.vorbestellungen,
					realsticPower = script_.realsticPower,
					stornierungen = script_.stornierungen,
					commercialFlop = script_.commercialFlop,
					commercialHit = script_.commercialHit,
					inAppPurchaseWeek = script_.inAppPurchaseWeek,
					arcadeCase = script_.arcadeCase,
					arcadeMonitor = script_.arcadeMonitor,
					arcadeJoystick = script_.arcadeJoystick,
					arcadeSound = script_.arcadeSound,
					arcadeProdCosts = script_.arcadeProdCosts,
					finanzierung_Grundkosten = script_.finanzierung_Grundkosten,
					finanzierung_Technology = script_.finanzierung_Technology,
					finanzierung_Kontent = script_.finanzierung_Kontent,
					retailVersion = script_.retailVersion,
					digitalVersion = script_.digitalVersion,
					newGenreCombination = script_.newGenreCombination,
					newTopicCombination = script_.newTopicCombination,
					ipTime = script_.ipTime,
					npcLateinNumbers = script_.npcLateinNumbers,
					mmoTOf2p_created = script_.mmoTOf2p_created,
					bundle_created = script_.bundle_created,
					abosAddons = script_.abosAddons,
					inAppPurchase = (bool[])script_.inAppPurchase.Clone(),
					Designschwerpunkt = (int[])script_.Designschwerpunkt.Clone(),
					Designausrichtung = (int[])script_.Designausrichtung.Clone()
				});
			}
		}
	}

	public void CLIENT_Get_Game(NetworkConnection conn, c_Game msg)
	{
		if (!isServer)
		{
			return;
		}
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		gameScript gameScript2 = null;
		if (!game)
		{
			gameScript2 = games_.CreateNewGame(fromSavegame: true, setDate: false);
			gameScript2.myID = msg.gameID;
			gameScript2.SetGameObjectName();
			games_.FindGames();
		}
		else
		{
			gameScript2 = game.GetComponent<gameScript>();
		}
		if (!gameScript2)
		{
			return;
		}
		bool isOnMarket = gameScript2.isOnMarket;
		gameScript2.myID = msg.gameID;
		gameScript2.SetMyName(msg.myName);
		gameScript2.ipName = msg.ipName;
		gameScript2.inDevelopment = msg.inDevelopment;
		gameScript2.developerID = msg.developerID;
		gameScript2.publisherID = msg.publisherID;
		gameScript2.ownerID = msg.ownerID;
		gameScript2.engineID = msg.engineID;
		gameScript2.hype = msg.hype;
		gameScript2.isOnMarket = msg.isOnMarket;
		gameScript2.warBeiAwards = msg.warBeiAwards;
		gameScript2.weeksOnMarket = msg.weeksOnMarket;
		gameScript2.usk = msg.usk;
		gameScript2.freigabeBudget = msg.freigabeBudget;
		gameScript2.reviewGameplay = msg.reviewGameplay;
		gameScript2.reviewGrafik = msg.reviewGrafik;
		gameScript2.reviewSound = msg.reviewSound;
		gameScript2.reviewSteuerung = msg.reviewSteuerung;
		gameScript2.reviewTotal = msg.reviewTotal;
		gameScript2.reviewGameplayText = msg.reviewGameplayText;
		gameScript2.reviewGrafikText = msg.reviewGrafikText;
		gameScript2.reviewSoundText = msg.reviewSoundText;
		gameScript2.reviewSteuerungText = msg.reviewSteuerungText;
		gameScript2.reviewTotalText = msg.reviewTotalText;
		gameScript2.date_year = msg.date_year;
		gameScript2.date_month = msg.date_month;
		gameScript2.date_start_year = msg.date_start_year;
		gameScript2.date_start_month = msg.date_start_month;
		gameScript2.sellsTotal = msg.sellsTotal;
		gameScript2.umsatzTotal = msg.umsatzTotal;
		gameScript2.costs_entwicklung = msg.costs_entwicklung;
		gameScript2.costs_mitarbeiter = msg.costs_mitarbeiter;
		gameScript2.costs_marketing = msg.costs_marketing;
		gameScript2.costs_enginegebuehren = msg.costs_enginegebuehren;
		gameScript2.costs_server = msg.costs_server;
		gameScript2.costs_production = msg.costs_production;
		gameScript2.costs_updates = msg.costs_updates;
		gameScript2.typ_standard = msg.typ_standard;
		gameScript2.typ_nachfolger = msg.typ_nachfolger;
		gameScript2.teile = msg.teile;
		gameScript2.typ_contractGame = msg.typ_contractGame;
		gameScript2.typ_remaster = msg.typ_remaster;
		gameScript2.typ_spinoff = msg.typ_spinoff;
		gameScript2.typ_addon = msg.typ_addon;
		gameScript2.typ_addonStandalone = msg.typ_addonStandalone;
		gameScript2.typ_mmoaddon = msg.typ_mmoaddon;
		gameScript2.typ_bundle = msg.typ_bundle;
		gameScript2.typ_budget = msg.typ_budget;
		gameScript2.typ_bundleAddon = msg.typ_bundleAddon;
		gameScript2.typ_goty = msg.typ_goty;
		gameScript2.originalGameID = msg.originalGameID;
		gameScript2.portID = msg.portID;
		gameScript2.mainIP = msg.mainIP;
		gameScript2.ipPunkte = msg.ipPunkte;
		gameScript2.exklusiv = msg.exklusiv;
		gameScript2.herstellerExklusiv = msg.herstellerExklusiv;
		gameScript2.retro = msg.retro;
		gameScript2.handy = msg.handy;
		gameScript2.arcade = msg.arcade;
		gameScript2.goty = msg.goty;
		gameScript2.nachfolger_created = msg.nachfolger_created;
		gameScript2.remaster_created = msg.remaster_created;
		gameScript2.budget_created = msg.budget_created;
		gameScript2.goty_created = msg.goty_created;
		gameScript2.trendsetter = msg.trendsetter;
		gameScript2.spielbericht = msg.spielbericht;
		gameScript2.amountUpdates = msg.amountUpdates;
		gameScript2.bonusSellsUpdates = msg.bonusSellsUpdates;
		gameScript2.amountAddons = msg.amountAddons;
		gameScript2.bonusSellsAddons = msg.bonusSellsAddons;
		gameScript2.amountMMOAddons = msg.amountMMOAddons;
		gameScript2.addonQuality = msg.addonQuality;
		gameScript2.devAktFeature = msg.devAktFeature;
		gameScript2.devPoints = msg.devPoints;
		gameScript2.devPointsStart = msg.devPointsStart;
		gameScript2.devPoints_Gesamt = msg.devPoints_Gesamt;
		gameScript2.devPointsStart_Gesamt = msg.devPointsStart_Gesamt;
		gameScript2.points_gameplay = msg.points_gameplay;
		gameScript2.points_grafik = msg.points_grafik;
		gameScript2.points_sound = msg.points_sound;
		gameScript2.points_technik = msg.points_technik;
		gameScript2.points_bugs = msg.points_bugs;
		gameScript2.beschreibung = msg.beschreibung;
		gameScript2.gameTyp = msg.gameTyp;
		gameScript2.gameSize = msg.gameSize;
		gameScript2.gameZielgruppe = msg.gameZielgruppe;
		gameScript2.maingenre = msg.maingenre;
		gameScript2.subgenre = msg.subgenre;
		gameScript2.gameMainTheme = msg.gameMainTheme;
		gameScript2.gameSubTheme = msg.gameSubTheme;
		gameScript2.gameLicence = msg.gameLicence;
		gameScript2.gameCopyProtect = msg.gameCopyProtect;
		gameScript2.gameAntiCheat = msg.gameAntiCheat;
		gameScript2.gameAP_Gameplay = msg.gameAP_Gameplay;
		gameScript2.gameAP_Grafik = msg.gameAP_Grafik;
		gameScript2.gameAP_Sound = msg.gameAP_Sound;
		gameScript2.gameAP_Technik = msg.gameAP_Technik;
		gameScript2.gameLanguage = (bool[])msg.gameLanguage.Clone();
		gameScript2.gameGameplayFeatures = (bool[])msg.gameGameplayFeatures.Clone();
		gameScript2.gamePlatform = (int[])msg.gamePlatform.Clone();
		gameScript2.gameEngineFeature = (int[])msg.gameEngineFeature.Clone();
		gameScript2.gameplayFeatures_DevDone = (bool[])msg.gameplayFeatures_DevDone.Clone();
		gameScript2.engineFeature_DevDone = (bool[])msg.engineFeature_DevDone.Clone();
		gameScript2.gameplayStudio = (bool[])msg.gameplayStudio.Clone();
		gameScript2.grafikStudio = (bool[])msg.grafikStudio.Clone();
		gameScript2.soundStudio = (bool[])msg.soundStudio.Clone();
		gameScript2.motionCaptureStudio = (bool[])msg.motionCaptureStudio.Clone();
		gameScript2.bundleID = (int[])msg.bundleID.Clone();
		gameScript2.portExist = (bool[])msg.portExist.Clone();
		gameScript2.sellsPerWeek = (int[])msg.sellsPerWeek.Clone();
		gameScript2.verkaufspreis = (int[])msg.verkaufspreis.Clone();
		gameScript2.releaseDate = msg.releaseDate;
		gameScript2.abonnements = msg.abonnements;
		gameScript2.abonnementsWoche = msg.abonnementsWoche;
		gameScript2.aboPreis = msg.aboPreis;
		gameScript2.pubOffer = msg.pubOffer;
		gameScript2.pubAngebot = msg.pubAngebot;
		gameScript2.pubAngebot_Weeks = msg.pubAngebot_Weeks;
		gameScript2.pubAngebot_Verhandlung = msg.pubAngebot_Verhandlung;
		gameScript2.pubAngebot_Retail = msg.pubAngebot_Retail;
		gameScript2.pubAngebot_Digital = msg.pubAngebot_Digital;
		gameScript2.pubAngebot_Garantiesumme = msg.pubAngebot_Garantiesumme;
		gameScript2.pubAngebot_Gewinnbeteiligung = msg.pubAngebot_Gewinnbeteiligung;
		gameScript2.auftragsspiel = msg.auftragsspiel;
		gameScript2.auftragsspiel_gehalt = msg.auftragsspiel_gehalt;
		gameScript2.auftragsspiel_bonus = msg.auftragsspiel_bonus;
		gameScript2.auftragsspiel_zeitInWochen = msg.auftragsspiel_zeitInWochen;
		gameScript2.auftragsspiel_wochenAlsAngebot = msg.auftragsspiel_wochenAlsAngebot;
		gameScript2.auftragsspiel_zeitAbgelaufen = msg.auftragsspiel_zeitAbgelaufen;
		gameScript2.auftragsspiel_mindestbewertung = msg.auftragsspiel_mindestbewertung;
		gameScript2.f2pConverted = msg.f2pConverted;
		gameScript2.angekuendigt = msg.angekuendigt;
		gameScript2.subvention = msg.subvention;
		gameScript2.sonderIP = msg.sonderIP;
		gameScript2.sonderIPMindestreview = msg.sonderIPMindestreview;
		gameScript2.myNameTeil1 = msg.myNameTeil1;
		gameScript2.engineGewinnbeteiligung = msg.engineGewinnbeteiligung;
		gameScript2.weeksInDevelopment = msg.weeksInDevelopment;
		gameScript2.userPositiv = msg.userPositiv;
		gameScript2.userNegativ = msg.userNegativ;
		gameScript2.bestAbonnements = msg.bestAbonnements;
		gameScript2.bestChartPosition = msg.bestChartPosition;
		gameScript2.exklusivKonsolenSells = msg.exklusivKonsolenSells;
		gameScript2.lastChartPosition = msg.lastChartPosition;
		gameScript2.freeware = msg.freeware;
		gameScript2.sellsTotalStandard = msg.sellsTotalStandard;
		gameScript2.sellsTotalDeluxe = msg.sellsTotalDeluxe;
		gameScript2.sellsTotalCollectors = msg.sellsTotalCollectors;
		gameScript2.sellsTotalOnline = msg.sellsTotalOnline;
		gameScript2.points_bugsInvis = msg.points_bugsInvis;
		gameScript2.umsatzInApp = msg.umsatzInApp;
		gameScript2.umsatzAbos = msg.umsatzAbos;
		gameScript2.f2pInteresse = msg.f2pInteresse;
		gameScript2.mmoInteresse = msg.mmoInteresse;
		gameScript2.vorbestellungen = msg.vorbestellungen;
		gameScript2.realsticPower = msg.realsticPower;
		gameScript2.stornierungen = msg.stornierungen;
		gameScript2.commercialFlop = msg.commercialFlop;
		gameScript2.commercialHit = msg.commercialHit;
		gameScript2.inAppPurchaseWeek = msg.inAppPurchaseWeek;
		gameScript2.arcadeCase = msg.arcadeCase;
		gameScript2.arcadeMonitor = msg.arcadeMonitor;
		gameScript2.arcadeJoystick = msg.arcadeJoystick;
		gameScript2.arcadeSound = msg.arcadeSound;
		gameScript2.arcadeProdCosts = msg.arcadeProdCosts;
		gameScript2.finanzierung_Grundkosten = msg.finanzierung_Grundkosten;
		gameScript2.finanzierung_Technology = msg.finanzierung_Technology;
		gameScript2.finanzierung_Kontent = msg.finanzierung_Kontent;
		gameScript2.retailVersion = msg.retailVersion;
		gameScript2.digitalVersion = msg.digitalVersion;
		gameScript2.newGenreCombination = msg.newGenreCombination;
		gameScript2.newTopicCombination = msg.newTopicCombination;
		gameScript2.ipTime = msg.ipTime;
		gameScript2.npcLateinNumbers = msg.npcLateinNumbers;
		gameScript2.mmoTOf2p_created = msg.mmoTOf2p_created;
		gameScript2.bundle_created = msg.bundle_created;
		gameScript2.abosAddons = msg.abosAddons;
		gameScript2.inAppPurchase = (bool[])msg.inAppPurchase.Clone();
		gameScript2.Designschwerpunkt = (int[])msg.Designschwerpunkt.Clone();
		gameScript2.Designausrichtung = (int[])msg.Designausrichtung.Clone();
		gameScript2.InitUI();
		if (!isOnMarket && gameScript2.isOnMarket)
		{
			gameScript2.DONT_SEND_GAME = true;
			if (gameScript2.isOnMarket)
			{
				gameScript2.SetOnMarket();
			}
			gameScript2.DONT_SEND_GAME = false;
			if (mS_.newsSetting[0] && gameScript2.isOnMarket)
			{
				if (gameScript2.GameFromMitspieler())
				{
					string text = tS_.GetText(494);
					text = text.Replace("<NAME1>", gameScript2.GetDeveloperName());
					text = text.Replace("<NAME2>", gameScript2.GetNameWithTag());
					guiMain_.CreateTopNewsInfo(text);
					text = tS_.GetText(1269);
					text = text.Replace("<NAME>", msg.myName);
					guiMain_.AddChat(gameScript2.GetIdFromMitspieler(), text);
				}
				else
				{
					string text2 = tS_.GetText(494);
					text2 = text2.Replace("<NAME1>", gameScript2.GetDeveloperName());
					text2 = text2.Replace("<NAME2>", gameScript2.GetNameWithTag());
					guiMain_.CreateTopNewsInfo(text2);
				}
			}
		}
		if (gameScript2.isOnMarket)
		{
			games_.UpdateChartsWeek();
			guiMain_.UpdateCharts();
		}
		SERVER_Send_Game(gameScript2);
	}

	public void CLIENT_Send_BuyLizenz(int objectID_)
	{
		if (isClient)
		{
			NetworkClient.Send(new c_BuyLizenz
			{
				playerID = mS_.myID,
				objectID = objectID_
			});
		}
	}

	public void CLIENT_Get_BuyLizenz(NetworkConnection conn, c_BuyLizenz msg)
	{
		if (isServer)
		{
			Debug.Log("CLIENT_Get_BuyLizenz");
			FindScripts();
			if (licences_.licence_ANGEBOT.Length > msg.objectID)
			{
				licences_.licence_ANGEBOT[msg.objectID] = 0;
				SERVER_Send_Lizenz(msg.objectID);
			}
		}
	}

	public void CLIENT_Send_DeleteArbeitsmarkt(int objectID_, bool eingestellt)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_DeleteArbeitsmarkt");
			NetworkClient.Send(new c_DeleteArbeitsmarkt
			{
				playerID = mS_.myID,
				objectID = objectID_,
				eingestellt = eingestellt
			});
		}
	}

	public void CLIENT_Get_DeleteArbeitsmarkt(NetworkConnection conn, c_DeleteArbeitsmarkt msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_DeleteArbeitsmarkt");
		GameObject gameObject = GameObject.Find("AA_" + msg.objectID);
		if ((bool)gameObject)
		{
			charArbeitsmarkt component = gameObject.GetComponent<charArbeitsmarkt>();
			if ((bool)component)
			{
				component.RemoveFromArbeitsmarkt(msg.eingestellt);
			}
		}
	}

	public void CLIENT_Send_Command(int command)
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_Command() " + command);
			FindScripts();
			NetworkClient.Send(new c_Command
			{
				playerID = mS_.myID,
				command = command
			});
		}
	}

	public void CLIENT_Get_Command(NetworkConnection conn, c_Command msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_Command");
		player_mp player_mp2 = FindPlayer(msg.playerID);
		if (player_mp2 != null)
		{
			switch (msg.command)
			{
			case 1:
				player_mp2.playerReady = true;
				break;
			case 2:
				player_mp2.playerPause = true;
				break;
			case 3:
				player_mp2.playerPause = false;
				break;
			}
		}
	}

	public void CLIENT_Send_PlayerInfos()
	{
		if (isClient)
		{
			Debug.Log("CLIENT_Send_PlayerInfos");
			FindScripts();
			NetworkClient.Send(new c_PlayerInfos
			{
				playerID = mS_.myID,
				playerName = mS_.playerName,
				ready = mpMain_.uiObjects[51].GetComponent<Toggle>().isOn
			});
		}
	}

	public void CLIENT_Get_PlayerInfos(NetworkConnection conn, c_PlayerInfos msg)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("CLIENT_Get_PlayerInfos");
		FindScripts();
		if (!save_.loadingSavegame)
		{
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.playerName = msg.playerName;
				player_mp2.ready = msg.ready;
				_ = (bool)player_mp2.myPubScript_;
				SERVER_Send_PlayerInfos();
			}
		}
	}

	public void SERVER_Send_PlayerLeave(int playerID_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_PlayerLeave()");
			NetworkServer.SendToAll(new s_PlayerLeave
			{
				playerID = playerID_
			});
		}
	}

	public void SERVER_Get_PlayerLeave(s_PlayerLeave msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PlayerLeave()");
		FindScripts();
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.playerID)
			{
				if (guiMain_.uiObjects[201].activeSelf)
				{
					Object.Destroy(mS_.arrayPublisherScripts[i].gameObject);
				}
				break;
			}
		}
		for (int j = 0; j < playersMP.Count; j++)
		{
			if (playersMP[j].playerID == msg.playerID)
			{
				playersMP.RemoveAt(j);
				break;
			}
		}
	}

	public void SERVER_Send_PlatformSubvention(platformScript plat_, gameScript game_, int amount_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_PlatformSubvention()");
			if ((bool)plat_ && (bool)game_)
			{
				NetworkServer.SendToAll(new s_PlatformSubvention
				{
					platformID = plat_.myID,
					gameID = game_.myID,
					subvention = amount_
				});
			}
		}
	}

	public void SERVER_Get_PlatformSubvention(s_PlatformSubvention msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PlatformSubvention()");
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if (!mS_.arrayPlatformsScripts[i] || mS_.arrayPlatformsScripts[i].myID != msg.platformID)
			{
				continue;
			}
			if (mS_.arrayPlatformsScripts[i].ownerID == mS_.myID)
			{
				mS_.arrayPlatformsScripts[i].subventionMoney -= msg.subvention;
				if (mS_.arrayPlatformsScripts[i].subventionMoney < 0)
				{
					mS_.arrayPlatformsScripts[i].subventionMoney = 0;
				}
				mS_.arrayPlatformsScripts[i].costs_subvention += msg.subvention;
				mS_.Pay(msg.subvention, 33);
			}
			break;
		}
	}

	public void SERVER_Send_PlatformRemoveFromMarket(platformScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_PlatformRemoveFromMarket()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_PlatformRemoveFromMarket
				{
					platformID = script_.myID
				});
			}
		}
	}

	public void SERVER_Get_PlatformRemoveFromMarket(s_PlatformRemoveFromMarket msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PlatformRemoveFromMarket()");
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == msg.platformID)
			{
				if (!mS_.arrayPlatformsScripts[i].vomMarktGenommen)
				{
					mS_.arrayPlatformsScripts[i].RemoveFromMarket();
				}
				break;
			}
		}
	}

	public void SERVER_Send_PlatformData(platformScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_PlatformData()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_PlatformData
				{
					platformID = script_.myID,
					marktanteil = script_.marktanteil,
					units = script_.units,
					units_max = script_.units_max,
					date_year_end = script_.date_year_end
				});
			}
		}
	}

	public void SERVER_Get_PlatformData(s_PlatformData msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PlatformData()");
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == msg.platformID)
			{
				mS_.arrayPlatformsScripts[i].marktanteil = msg.marktanteil;
				if (mS_.arrayPlatformsScripts[i].ownerID != mS_.myID)
				{
					mS_.arrayPlatformsScripts[i].units = msg.units;
					mS_.arrayPlatformsScripts[i].units_max = msg.units_max;
				}
				mS_.arrayPlatformsScripts[i].date_year_end = msg.date_year_end;
				break;
			}
		}
	}

	public void SERVER_Send_ObjectDelete(int objectID_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_ObjectDelete()");
			NetworkServer.SendToAll(new s_ObjectDelete
			{
				playerID = mS_.myID,
				objectID = objectID_
			});
		}
	}

	public void SERVER_Get_ObjectDelete(s_ObjectDelete msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_ObjectDelete()");
		player_mp player_mp2 = FindPlayer(msg.playerID);
		if (player_mp2 == null)
		{
			return;
		}
		for (int i = 0; i < player_mp2.objects.Count; i++)
		{
			if (player_mp2.objects[i] != null && player_mp2.objects[i].id == msg.objectID)
			{
				player_mp2.objects.RemoveAt(i);
				break;
			}
		}
	}

	public void SERVER_Send_Object(int id_, int typ_, float x_, float y_, float rot_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Object()");
			FindScripts();
			NetworkServer.SendToAll(new s_Object
			{
				playerID = mS_.myID,
				objectID = id_,
				typ = typ_,
				x = x_,
				y = y_,
				rot = rot_
			});
		}
	}

	public void SERVER_Get_Object(s_Object msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Object()");
			FindPlayer(msg.playerID)?.objects.Add(new object_mp(msg.objectID, msg.typ, msg.x, msg.y, msg.rot));
		}
	}

	public void SERVER_Send_Map(int posx, int posy)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("SERVER_Send_Map()");
		FindScripts();
		if (mapScript_.IsInMapLimit(posx, posy))
		{
			int typ = 0;
			if ((bool)mapScript_.mapRoomScript[posx, posy])
			{
				typ = mapScript_.mapRoomScript[posx, posy].typ;
			}
			NetworkServer.SendToAll(new s_Map
			{
				playerID = mS_.myID,
				x = (byte)posx,
				y = (byte)posy,
				id = mapScript_.mapRoomID[posx, posy],
				typ = typ,
				door = mapScript_.mapDoors[posx, posy],
				window = mapScript_.mapWindows[posx, posy]
			});
		}
	}

	public void SERVER_Get_Map(s_Map msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Map()");
			FindScripts();
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null && mapScript_.IsInMapLimit(msg.x, msg.y))
			{
				player_mp2.mapRoomID[msg.x, msg.y] = msg.id;
				player_mp2.mapRoomTyp[msg.x, msg.y] = msg.typ;
				player_mp2.mapDoors[msg.x, msg.y] = msg.door;
				player_mp2.mapWindows[msg.x, msg.y] = msg.window;
			}
		}
	}

	public void SERVER_Send_GlobalEvent(int eventID, int wochen)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GlobalEvent()");
			NetworkServer.SendToAll(new s_GlobalEvent
			{
				eventID = eventID,
				wochen = wochen
			});
		}
	}

	public void SERVER_Get_GlobalEvent(s_GlobalEvent msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GlobalEvent()");
			FindScripts();
			mS_.SetGlobalEvent(msg.eventID);
			mS_.globalEventWeeks = msg.wochen;
			StartCoroutine(iSERVER_Get_GlobalEvent(msg.eventID, msg.wochen));
		}
	}

	private IEnumerator iSERVER_Get_GlobalEvent(int eventID, int wochen)
	{
		bool done = false;
		while (!done)
		{
			if ((bool)guiMain_ && !guiMain_.menuOpen)
			{
				done = true;
				guiMain_.uiObjects[216].SetActive(value: true);
				guiMain_.uiObjects[216].GetComponent<Menu_RandomEventGlobal>().Init(eventID);
			}
			yield return null;
		}
	}

	public void SERVER_Send_EngineAbrechnung(int toPlayer, int gameID)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_EngineAbrechnung()");
			NetworkServer.SendToAll(new s_EngineAbrechnung
			{
				toPlayerID = toPlayer,
				gameID = gameID
			});
		}
	}

	public void SERVER_Get_EngineAbrechnung(s_EngineAbrechnung msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_EngineAbrechnung()");
		FindScripts();
		if (msg.toPlayerID != mS_.myID || !guiMain_)
		{
			return;
		}
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				guiMain_.OpenEngineAbrechnung(component);
			}
		}
	}

	public void SERVER_Send_Award(int bestGrafik_, int bestSound_, int bestStudio_, int bestPublisher_, int bestGame_, int badGame_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Award()");
			NetworkServer.SendToAll(new s_Awards
			{
				bestGrafik = bestGrafik_,
				bestSound = bestSound_,
				bestStudio = bestStudio_,
				bestPublisher = bestPublisher_,
				bestGame = bestGame_,
				badGame = badGame_
			});
		}
	}

	public void SERVER_Get_Awards(s_Awards msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Awards()");
			FindScripts();
			Menu_Awards component = guiMain_.uiObjects[143].GetComponent<Menu_Awards>();
			component.gameObject.SetActive(value: true);
			component.Multiplayer_FindWinners(msg.bestGrafik, msg.bestSound, msg.bestStudio, msg.bestPublisher, msg.bestGame, msg.badGame);
			mS_.MadGamesAward(force: true);
		}
	}

	public void SERVER_Send_Payment(int playerID_, int toPlayerID_, int what, int money, int objectID_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Payment()");
			NetworkServer.SendToAll(new s_Payment
			{
				playerID = playerID_,
				toPlayerID = toPlayerID_,
				what = what,
				money = money,
				objectID = objectID_
			});
		}
	}

	public void SERVER_Get_Payment(s_Payment msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Payment()");
		FindScripts();
		if (msg.toPlayerID != mS_.myID)
		{
			return;
		}
		string text = "";
		switch (msg.what)
		{
		case 0:
		{
			text = tS_.GetText(1044);
			text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			mS_.Earn(msg.money, 4);
			for (int j = 0; j < mS_.arrayEnginesScripts.Length; j++)
			{
				if ((bool)mS_.arrayEnginesScripts[j] && mS_.arrayEnginesScripts[j].myID == msg.objectID)
				{
					mS_.arrayEnginesScripts[j].umsatz += msg.money;
					break;
				}
			}
			break;
		}
		case 1:
		{
			text = tS_.GetText(1045);
			text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			mS_.Earn(msg.money, 4);
			for (int k = 0; k < mS_.arrayEnginesScripts.Length; k++)
			{
				if ((bool)mS_.arrayEnginesScripts[k] && mS_.arrayEnginesScripts[k].myID == msg.objectID)
				{
					mS_.arrayEnginesScripts[k].umsatz += msg.money;
					mS_.arrayEnginesScripts[k].amountSellToPlayer++;
					break;
				}
			}
			break;
		}
		case 2:
			text = tS_.GetText(1658);
			text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			mS_.Earn(msg.money, 10);
			break;
		case 3:
		{
			mS_.Earn(msg.money, 4);
			for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
			{
				if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.objectID)
				{
					mS_.arrayEnginesScripts[i].umsatz += msg.money;
					break;
				}
			}
			break;
		}
		case 4:
			mS_.Earn(msg.money, 10);
			break;
		case 5:
			text = tS_.GetText(2249);
			text = text.Replace("<NUM>", mS_.GetMoney(msg.money, showDollar: true));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			mS_.Earn(msg.money, 31);
			break;
		}
	}

	public void SERVER_Send_GenreCombination(int genre_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GenreCombination()");
			FindScripts();
			bool[] array = new bool[genres_.genres_LEVEL.Length];
			for (int i = 0; i < genres_.genres_LEVEL.Length; i++)
			{
				array[i] = genres_.genres_COMBINATION[genre_, i];
			}
			NetworkServer.SendToAll(new s_GenreCombination
			{
				genreSlot = genre_,
				genres_COMBINATION = array
			});
		}
	}

	public void SERVER_Get_GenreCombination(s_GenreCombination msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GenreCombination()");
			FindScripts();
			for (int i = 0; i < msg.genres_COMBINATION.Length; i++)
			{
				genres_.genres_COMBINATION[msg.genreSlot, i] = msg.genres_COMBINATION[i];
			}
		}
	}

	public void SERVER_Send_GenrePlatformSuit(int genre_, int i0, int i1, int i2, int i3, int i4)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GenrePlatformSuit()");
			NetworkServer.SendToAll(new s_GenrePlatformSuit
			{
				genreSlot = genre_,
				pc_0 = i0,
				konsole_1 = i1,
				handheld_2 = i2,
				handy_3 = i3,
				arcade_4 = i4
			});
		}
	}

	public void SERVER_Get_GenrePlatformSuit(s_GenrePlatformSuit msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GenrePlatformSuit()");
			FindScripts();
			genres_.genres_PLATFORM_SELLS[msg.genreSlot, 0] = msg.pc_0;
			genres_.genres_PLATFORM_SELLS[msg.genreSlot, 1] = msg.konsole_1;
			genres_.genres_PLATFORM_SELLS[msg.genreSlot, 2] = msg.handheld_2;
			genres_.genres_PLATFORM_SELLS[msg.genreSlot, 3] = msg.handy_3;
			genres_.genres_PLATFORM_SELLS[msg.genreSlot, 4] = msg.arcade_4;
		}
	}

	public void SERVER_Send_GenreDesign(int genre_, int f0, int f1, int f2, int f3, int f4, int f5, int f6, int f7, int a0, int a1, int a2)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GenreDesign()");
			NetworkServer.SendToAll(new s_GenreDesign
			{
				genreSlot = genre_,
				genres_focus0 = f0,
				genres_focus1 = f1,
				genres_focus2 = f2,
				genres_focus3 = f3,
				genres_focus4 = f4,
				genres_focus5 = f5,
				genres_focus6 = f6,
				genres_focus7 = f7,
				genres_align0 = a0,
				genres_align1 = a1,
				genres_align2 = a2
			});
		}
	}

	public void SERVER_Get_GenreDesign(s_GenreDesign msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GenreDesign()");
			FindScripts();
			genres_.genres_FOCUS[msg.genreSlot, 0] = msg.genres_focus0;
			genres_.genres_FOCUS[msg.genreSlot, 1] = msg.genres_focus1;
			genres_.genres_FOCUS[msg.genreSlot, 2] = msg.genres_focus2;
			genres_.genres_FOCUS[msg.genreSlot, 3] = msg.genres_focus3;
			genres_.genres_FOCUS[msg.genreSlot, 4] = msg.genres_focus4;
			genres_.genres_FOCUS[msg.genreSlot, 5] = msg.genres_focus5;
			genres_.genres_FOCUS[msg.genreSlot, 6] = msg.genres_focus6;
			genres_.genres_FOCUS[msg.genreSlot, 7] = msg.genres_focus7;
			genres_.genres_ALIGN[msg.genreSlot, 0] = msg.genres_align0;
			genres_.genres_ALIGN[msg.genreSlot, 1] = msg.genres_align1;
			genres_.genres_ALIGN[msg.genreSlot, 2] = msg.genres_align2;
		}
	}

	public void SERVER_Send_Forschung(int playerID_)
	{
		if (!isServer || playersMP.Count <= 1)
		{
			return;
		}
		Debug.Log("SERVER_Send_Forschung()");
		FindScripts();
		player_mp player_mp2;
		if (playerID_ == mS_.myID)
		{
			player_mp2 = FindPlayer(mS_.myID);
			if (player_mp2 == null)
			{
				return;
			}
			if (player_mp2.forschungSonstiges.Length != fS_.RES_POINTS.Length)
			{
				player_mp2.forschungSonstiges = new bool[fS_.RES_POINTS.Length];
			}
			if (player_mp2.genres.Length != genres_.genres_UNLOCK.Length)
			{
				player_mp2.genres = new bool[genres_.genres_UNLOCK.Length];
			}
			if (player_mp2.themes.Length != themes_.themes_RES_POINTS_LEFT.Length)
			{
				player_mp2.themes = new bool[themes_.themes_RES_POINTS_LEFT.Length];
			}
			if (player_mp2.engineFeatures.Length != eF_.engineFeatures_RES_POINTS.Length)
			{
				player_mp2.engineFeatures = new bool[eF_.engineFeatures_RES_POINTS.Length];
			}
			if (player_mp2.gameplayFeatures.Length != gF_.gameplayFeatures_RES_POINTS.Length)
			{
				player_mp2.gameplayFeatures = new bool[gF_.gameplayFeatures_RES_POINTS.Length];
			}
			if (player_mp2.hardware.Length != hardware_.hardware_RES_POINTS.Length)
			{
				player_mp2.hardware = new bool[hardware_.hardware_RES_POINTS.Length];
			}
			if (player_mp2.hardwareFeatures.Length != hardwareFeatures_.hardFeat_RES_POINTS.Length)
			{
				player_mp2.hardwareFeatures = new bool[hardwareFeatures_.hardFeat_RES_POINTS.Length];
			}
			for (int i = 0; i < player_mp2.forschungSonstiges.Length; i++)
			{
				if (fS_.RES_POINTS_LEFT[i] <= 0f)
				{
					player_mp2.forschungSonstiges[i] = true;
				}
				else
				{
					player_mp2.forschungSonstiges[i] = false;
				}
			}
			for (int j = 0; j < player_mp2.genres.Length; j++)
			{
				if (genres_.genres_RES_POINTS_LEFT[j] <= 0f)
				{
					player_mp2.genres[j] = true;
				}
				else
				{
					player_mp2.genres[j] = false;
				}
			}
			for (int k = 0; k < player_mp2.themes.Length; k++)
			{
				if (themes_.themes_RES_POINTS_LEFT[k] <= 0f)
				{
					player_mp2.themes[k] = true;
				}
				else
				{
					player_mp2.themes[k] = false;
				}
			}
			for (int l = 0; l < player_mp2.engineFeatures.Length; l++)
			{
				if (eF_.engineFeatures_RES_POINTS_LEFT[l] <= 0f)
				{
					player_mp2.engineFeatures[l] = true;
				}
				else
				{
					player_mp2.engineFeatures[l] = false;
				}
			}
			for (int m = 0; m < player_mp2.gameplayFeatures.Length; m++)
			{
				if (gF_.gameplayFeatures_RES_POINTS_LEFT[m] <= 0f)
				{
					player_mp2.gameplayFeatures[m] = true;
				}
				else
				{
					player_mp2.gameplayFeatures[m] = false;
				}
			}
			for (int n = 0; n < player_mp2.hardware.Length; n++)
			{
				if (hardware_.hardware_RES_POINTS_LEFT[n] <= 0f)
				{
					player_mp2.hardware[n] = true;
				}
				else
				{
					player_mp2.hardware[n] = false;
				}
			}
			for (int num = 0; num < player_mp2.hardwareFeatures.Length; num++)
			{
				if (hardwareFeatures_.hardFeat_RES_POINTS_LEFT[num] <= 0f)
				{
					player_mp2.hardwareFeatures[num] = true;
				}
				else
				{
					player_mp2.hardwareFeatures[num] = false;
				}
			}
		}
		player_mp2 = FindPlayer(playerID_);
		if (player_mp2 != null)
		{
			NetworkServer.SendToAll(new s_Forschung
			{
				playerID = playerID_,
				forschungSonstiges = (bool[])player_mp2.forschungSonstiges.Clone(),
				genres = (bool[])player_mp2.genres.Clone(),
				themes = (bool[])player_mp2.themes.Clone(),
				engineFeatures = (bool[])player_mp2.engineFeatures.Clone(),
				gameplayFeatures = (bool[])player_mp2.gameplayFeatures.Clone(),
				hardware = (bool[])player_mp2.hardware.Clone(),
				hardwareFeatures = (bool[])player_mp2.hardwareFeatures.Clone()
			});
		}
	}

	public void SERVER_Get_Forschung(s_Forschung msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Forschung()");
		FindScripts();
		if (msg.playerID != mS_.myID)
		{
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.forschungSonstiges = (bool[])msg.forschungSonstiges.Clone();
				player_mp2.genres = (bool[])msg.genres.Clone();
				player_mp2.themes = (bool[])msg.themes.Clone();
				player_mp2.engineFeatures = (bool[])msg.engineFeatures.Clone();
				player_mp2.gameplayFeatures = (bool[])msg.gameplayFeatures.Clone();
				player_mp2.hardware = (bool[])msg.hardware.Clone();
				player_mp2.hardwareFeatures = (bool[])msg.hardwareFeatures.Clone();
			}
		}
	}

	public void SERVER_Send_GenreBeliebtheit()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GenreBeliebtheit()");
			FindScripts();
			NetworkServer.SendToAll(new s_GenreBeliebtheit
			{
				genreBeliebtheit = (float[])genres_.genres_BELIEBTHEIT.Clone()
			});
		}
	}

	public void SERVER_Get_GenreBeliebtheit(s_GenreBeliebtheit msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_GenreBeliebtheit()");
			FindScripts();
			genres_.genres_BELIEBTHEIT = (float[])msg.genreBeliebtheit.Clone();
		}
	}

	public void SERVER_Send_Help(int playerID_, int toPlayerID_, int what, int valueA, int valueB, int valueC)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Help()");
			NetworkServer.SendToAll(new s_Help
			{
				playerID = playerID_,
				toPlayerID = toPlayerID_,
				what = what,
				valueA = valueA,
				valueB = valueB,
				valueC = valueC
			});
		}
	}

	public void SERVER_Get_Help(s_Help msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Help()");
		FindScripts();
		if (msg.toPlayerID != mS_.myID)
		{
			return;
		}
		string text = "";
		switch (msg.what)
		{
		case 0:
			text = tS_.GetText(1327);
			text = text.Replace("<NUM>", mS_.GetMoney(msg.valueA, showDollar: true));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			mS_.Earn(msg.valueA, 1);
			break;
		case 1:
		{
			GameObject gameObject = null;
			for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
			{
				if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.valueA)
				{
					gameObject = mS_.arrayEnginesScripts[i].gameObject;
					break;
				}
			}
			if ((bool)gameObject)
			{
				engineScript component = gameObject.GetComponent<engineScript>();
				if ((bool)component && !component.gekauft)
				{
					component.gekauft = true;
					text = tS_.GetText(1330);
					text = text.Replace("<NAME>", component.GetName());
					guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
				}
			}
			break;
		}
		case 2:
			text = tS_.GetText(1332);
			text = text.Replace("<NAME>", licences_.GetName(msg.valueA));
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			licences_.licence_GEKAUFT[msg.valueA] += msg.valueB;
			break;
		case 3:
			text = tS_.GetText(1339);
			switch (msg.valueB)
			{
			case 0:
				text = text.Replace("<NAME>", genres_.GetName(msg.valueA));
				genres_.genres_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 1:
				text = text.Replace("<NAME>", tS_.GetThemes(msg.valueA));
				themes_.themes_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 2:
				text = text.Replace("<NAME>", eF_.GetName(msg.valueA));
				eF_.engineFeatures_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 3:
				text = text.Replace("<NAME>", gF_.GetName(msg.valueA));
				gF_.gameplayFeatures_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 4:
				text = text.Replace("<NAME>", hardware_.GetName(msg.valueA));
				hardware_.hardware_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 5:
				text = text.Replace("<NAME>", fS_.GetName(msg.valueA));
				fS_.RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			case 6:
				text = text.Replace("<NAME>", hardwareFeatures_.GetName(msg.valueA));
				hardwareFeatures_.hardFeat_RES_POINTS_LEFT[msg.valueA] = 0f;
				break;
			}
			CLIENT_Send_Forschung();
			guiMain_.AddChat(msg.playerID, "<color=green>" + text + "</color>");
			break;
		case 4:
			mS_.sabotage_pr = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(0);
			break;
		case 5:
			mS_.sabotage_motivation = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(1);
			break;
		case 6:
			mS_.sabotage_klage = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(2);
			break;
		case 7:
			mS_.sabotage_reviews = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(3);
			break;
		case 8:
			mS_.sabotage_geruecht = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(4);
			break;
		case 9:
			mS_.sabotage_work = 24;
			guiMain_.uiObjects[443].SetActive(value: true);
			guiMain_.uiObjects[443].GetComponent<Menu_Sabotage_Message>().Init(5);
			break;
		}
	}

	public void SERVER_Send_Platform(platformScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Platform()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_Platform
				{
					myID = script_.myID,
					date_year = script_.date_year,
					date_month = script_.date_month,
					date_year_end = script_.date_year_end,
					date_month_end = script_.date_month_end,
					price = script_.price,
					dev_costs = script_.dev_costs,
					tech = script_.tech,
					typ = script_.typ,
					marktanteil = script_.marktanteil,
					needFeatures = (int[])script_.needFeatures.Clone(),
					units = script_.units,
					units_max = script_.units_max,
					minGamePassGames = script_.minGamePassGames,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_HU = script_.name_HU,
					name_JA = script_.name_JA,
					name_PL = script_.name_PL,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					manufacturer_EN = script_.manufacturer_EN,
					manufacturer_GE = script_.manufacturer_GE,
					manufacturer_TU = script_.manufacturer_TU,
					manufacturer_CH = script_.manufacturer_CH,
					manufacturer_FR = script_.manufacturer_FR,
					manufacturer_HU = script_.manufacturer_HU,
					manufacturer_JA = script_.manufacturer_JA,
					manufacturer_PL = script_.manufacturer_PL,
					manufacturer_UA = script_.manufacturer_UA,
					manufacturer_TH = script_.manufacturer_TH,
					pic1_file = script_.pic1_file,
					pic2_file = script_.pic2_file,
					pic2_year = script_.pic2_year,
					games = script_.games,
					exklusivGames = script_.exklusivGames,
					isUnlocked = script_.isUnlocked,
					vomMarktGenommen = script_.vomMarktGenommen,
					complex = script_.complex,
					internet = script_.internet,
					powerFromMarket = script_.powerFromMarket,
					myName = script_.myName,
					ownerID = script_.ownerID,
					gameID = script_.gameID,
					anzController = script_.anzController,
					consoleColor = script_.consoleColor,
					component_cpu = script_.component_cpu,
					component_gfx = script_.component_gfx,
					component_ram = script_.component_ram,
					component_hdd = script_.component_hdd,
					component_sfx = script_.component_sfx,
					component_cooling = script_.component_cooling,
					component_disc = script_.component_disc,
					component_controller = script_.component_controller,
					component_case = script_.component_case,
					component_monitor = script_.component_monitor,
					hwFeatures = (bool[])script_.hwFeatures.Clone(),
					entwicklungsKosten = script_.entwicklungsKosten,
					einnahmen = script_.einnahmen,
					hype = script_.hype,
					startProduktionskosten = script_.startProduktionskosten,
					verkaufspreis = script_.verkaufspreis,
					kostenreduktion = script_.kostenreduktion,
					autoPreis = script_.autoPreis,
					thridPartyGames = script_.thridPartyGames,
					umsatzTotal = script_.umsatzTotal,
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					weeksOnMarket = script_.weeksOnMarket,
					review = script_.review,
					performancePoints = script_.performancePoints,
					vorgaengerID = script_.vorgaengerID,
					nachfolgerID = script_.nachfolgerID,
					proVersion = script_.proVersion,
					proName = script_.proName,
					subventionMoney = script_.subventionMoney,
					subventionGameSize = (bool[])script_.subventionGameSize.Clone()
				});
			}
		}
	}

	public void SERVER_Get_Platform(s_Platform msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Platform()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPlatformsScripts[i].gameObject;
				break;
			}
		}
		if ((bool)gameObject)
		{
			platformScript component = gameObject.GetComponent<platformScript>();
			if ((bool)component && component.ownerID != mS_.myID)
			{
				component.myID = msg.myID;
				component.date_year = msg.date_year;
				component.date_month = msg.date_month;
				component.date_year_end = msg.date_year_end;
				component.date_month_end = msg.date_month_end;
				component.price = msg.price;
				component.dev_costs = msg.dev_costs;
				component.tech = msg.tech;
				component.typ = msg.typ;
				component.marktanteil = msg.marktanteil;
				component.needFeatures = (int[])msg.needFeatures.Clone();
				component.units = msg.units;
				component.units_max = msg.units_max;
				component.minGamePassGames = msg.minGamePassGames;
				component.name_EN = msg.name_EN;
				component.name_GE = msg.name_GE;
				component.name_TU = msg.name_TU;
				component.name_CH = msg.name_CH;
				component.name_FR = msg.name_FR;
				component.name_HU = msg.name_HU;
				component.name_JA = msg.name_JA;
				component.name_PL = msg.name_PL;
				component.name_UA = msg.name_UA;
				component.name_TH = msg.name_TH;
				component.manufacturer_EN = msg.manufacturer_EN;
				component.manufacturer_GE = msg.manufacturer_GE;
				component.manufacturer_TU = msg.manufacturer_TU;
				component.manufacturer_CH = msg.manufacturer_CH;
				component.manufacturer_FR = msg.manufacturer_FR;
				component.manufacturer_HU = msg.manufacturer_HU;
				component.manufacturer_JA = msg.manufacturer_JA;
				component.manufacturer_PL = msg.manufacturer_PL;
				component.manufacturer_UA = msg.manufacturer_UA;
				component.manufacturer_TH = msg.manufacturer_TH;
				component.pic1_file = msg.pic1_file;
				component.pic2_file = msg.pic2_file;
				component.pic2_year = msg.pic2_year;
				component.games = msg.games;
				component.exklusivGames = msg.exklusivGames;
				component.isUnlocked = msg.isUnlocked;
				component.vomMarktGenommen = msg.vomMarktGenommen;
				component.complex = msg.complex;
				component.internet = msg.internet;
				component.powerFromMarket = msg.powerFromMarket;
				component.myName = msg.myName;
				component.ownerID = msg.ownerID;
				component.gameID = msg.gameID;
				component.anzController = msg.anzController;
				component.consoleColor = msg.consoleColor;
				component.component_cpu = msg.component_cpu;
				component.component_gfx = msg.component_gfx;
				component.component_ram = msg.component_ram;
				component.component_hdd = msg.component_hdd;
				component.component_sfx = msg.component_sfx;
				component.component_cooling = msg.component_cooling;
				component.component_disc = msg.component_disc;
				component.component_controller = msg.component_controller;
				component.component_case = msg.component_case;
				component.component_monitor = msg.component_monitor;
				component.hwFeatures = (bool[])msg.hwFeatures.Clone();
				component.entwicklungsKosten = msg.entwicklungsKosten;
				component.einnahmen = msg.einnahmen;
				component.hype = msg.hype;
				component.startProduktionskosten = msg.startProduktionskosten;
				component.verkaufspreis = msg.verkaufspreis;
				component.kostenreduktion = msg.kostenreduktion;
				component.autoPreis = msg.autoPreis;
				component.thridPartyGames = msg.thridPartyGames;
				component.umsatzTotal = msg.umsatzTotal;
				component.weeksOnMarket = msg.weeksOnMarket;
				component.review = msg.review;
				component.performancePoints = msg.performancePoints;
				component.vorgaengerID = msg.vorgaengerID;
				component.nachfolgerID = msg.nachfolgerID;
				component.proVersion = msg.proVersion;
				component.proName = msg.proName;
				component.subventionMoney = msg.subventionMoney;
				component.subventionGameSize = (bool[])msg.subventionGameSize.Clone();
			}
			return;
		}
		platformScript platformScript2 = platforms_.CreatePlatform();
		if ((bool)platformScript2)
		{
			platformScript2.myID = msg.myID;
			platformScript2.date_year = msg.date_year;
			platformScript2.date_month = msg.date_month;
			platformScript2.date_year_end = msg.date_year_end;
			platformScript2.date_month_end = msg.date_month_end;
			platformScript2.price = msg.price;
			platformScript2.dev_costs = msg.dev_costs;
			platformScript2.tech = msg.tech;
			platformScript2.typ = msg.typ;
			platformScript2.marktanteil = msg.marktanteil;
			platformScript2.needFeatures = (int[])msg.needFeatures.Clone();
			platformScript2.units = msg.units;
			platformScript2.units_max = msg.units_max;
			platformScript2.minGamePassGames = msg.minGamePassGames;
			platformScript2.name_EN = msg.name_EN;
			platformScript2.name_GE = msg.name_GE;
			platformScript2.name_TU = msg.name_TU;
			platformScript2.name_CH = msg.name_CH;
			platformScript2.name_FR = msg.name_FR;
			platformScript2.name_HU = msg.name_HU;
			platformScript2.name_JA = msg.name_JA;
			platformScript2.name_PL = msg.name_PL;
			platformScript2.name_UA = msg.name_UA;
			platformScript2.name_TH = msg.name_TH;
			platformScript2.manufacturer_EN = msg.manufacturer_EN;
			platformScript2.manufacturer_GE = msg.manufacturer_GE;
			platformScript2.manufacturer_TU = msg.manufacturer_TU;
			platformScript2.manufacturer_CH = msg.manufacturer_CH;
			platformScript2.manufacturer_FR = msg.manufacturer_FR;
			platformScript2.manufacturer_HU = msg.manufacturer_HU;
			platformScript2.manufacturer_JA = msg.manufacturer_JA;
			platformScript2.manufacturer_PL = msg.manufacturer_PL;
			platformScript2.manufacturer_UA = msg.manufacturer_UA;
			platformScript2.manufacturer_TH = msg.manufacturer_TH;
			platformScript2.pic1_file = msg.pic1_file;
			platformScript2.pic2_file = msg.pic2_file;
			platformScript2.pic2_year = msg.pic2_year;
			platformScript2.games = msg.games;
			platformScript2.exklusivGames = msg.exklusivGames;
			platformScript2.isUnlocked = msg.isUnlocked;
			platformScript2.vomMarktGenommen = msg.vomMarktGenommen;
			platformScript2.complex = msg.complex;
			platformScript2.internet = msg.internet;
			platformScript2.powerFromMarket = msg.powerFromMarket;
			platformScript2.myName = msg.myName;
			platformScript2.ownerID = msg.ownerID;
			platformScript2.gameID = msg.gameID;
			platformScript2.anzController = msg.anzController;
			platformScript2.consoleColor = msg.consoleColor;
			platformScript2.component_cpu = msg.component_cpu;
			platformScript2.component_gfx = msg.component_gfx;
			platformScript2.component_ram = msg.component_ram;
			platformScript2.component_hdd = msg.component_hdd;
			platformScript2.component_sfx = msg.component_sfx;
			platformScript2.component_cooling = msg.component_cooling;
			platformScript2.component_disc = msg.component_disc;
			platformScript2.component_controller = msg.component_controller;
			platformScript2.component_case = msg.component_case;
			platformScript2.component_monitor = msg.component_monitor;
			platformScript2.hwFeatures = (bool[])msg.hwFeatures.Clone();
			platformScript2.entwicklungsKosten = msg.entwicklungsKosten;
			platformScript2.einnahmen = msg.einnahmen;
			platformScript2.hype = msg.hype;
			platformScript2.startProduktionskosten = msg.startProduktionskosten;
			platformScript2.verkaufspreis = msg.verkaufspreis;
			platformScript2.kostenreduktion = msg.kostenreduktion;
			platformScript2.autoPreis = msg.autoPreis;
			platformScript2.thridPartyGames = msg.thridPartyGames;
			platformScript2.umsatzTotal = msg.umsatzTotal;
			platformScript2.weeksOnMarket = msg.weeksOnMarket;
			platformScript2.review = msg.review;
			platformScript2.performancePoints = msg.performancePoints;
			platformScript2.vorgaengerID = msg.vorgaengerID;
			platformScript2.nachfolgerID = msg.nachfolgerID;
			platformScript2.proVersion = msg.proVersion;
			platformScript2.proName = msg.proName;
			platformScript2.subventionMoney = msg.subventionMoney;
			platformScript2.subventionGameSize = (bool[])msg.subventionGameSize.Clone();
			platformScript2.Init();
			if (platformScript2.date_year == 1976 && platformScript2.date_month == 1)
			{
				platformScript2.isUnlocked = true;
				platformScript2.inBesitz = true;
			}
			if (!platformScript2.OwnerIsNPC() && !platformScript2.vomMarktGenommen)
			{
				guiMain_.CreateTopNewsPlatform(platformScript2);
				string text = tS_.GetText(1629);
				text = text.Replace("<NAME>", msg.myName);
				guiMain_.AddChat(msg.ownerID, text);
			}
			if (!platformScript2.OwnerIsNPC() && platformScript2.vomMarktGenommen)
			{
				guiMain_.CreateTopNewsPlatformRemove(platformScript2);
			}
		}
	}

	public void SERVER_Send_EnginePublisherBuyed(engineScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_EnginePublisherBuyed()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_EnginePublisherBuyed
				{
					engineID = script_.myID,
					publisherBuyed = (bool[])script_.publisherBuyed.Clone()
				});
			}
		}
	}

	public void SERVER_Get_EnginePublisherBuyed(s_EnginePublisherBuyed msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_EnginePublisherBuyed()");
		FindScripts();
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.engineID)
			{
				mS_.arrayEnginesScripts[i].publisherBuyed = (bool[])msg.publisherBuyed.Clone();
				break;
			}
		}
	}

	public void SERVER_Send_Engine(engineScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Engine()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_Engine
				{
					engineID = script_.myID,
					ownerID = script_.ownerID,
					isUnlocked = script_.isUnlocked,
					gekauft = script_.gekauft,
					myName = script_.myName,
					features = script_.features,
					spezialgenre = script_.spezialgenre,
					spezialplatform = script_.spezialplatform,
					sellEngine = script_.sellEngine,
					preis = script_.preis,
					gewinnbeteiligung = script_.gewinnbeteiligung,
					marktdominanz = script_.marktdominanz
				});
			}
		}
	}

	public void SERVER_Get_Engine(s_Engine msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Engine()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.engineID)
			{
				gameObject = mS_.arrayEnginesScripts[i].gameObject;
				break;
			}
		}
		if ((bool)gameObject)
		{
			engineScript component = gameObject.GetComponent<engineScript>();
			if ((bool)component && component.ownerID != mS_.myID)
			{
				component.myID = msg.engineID;
				component.ownerID = msg.ownerID;
				component.isUnlocked = msg.isUnlocked;
				if (msg.ownerID != -1)
				{
					component.isUnlocked = true;
				}
				component.gekauft = msg.gekauft;
				component.myName = msg.myName;
				component.features = (bool[])msg.features.Clone();
				component.spezialgenre = msg.spezialgenre;
				component.spezialplatform = msg.spezialplatform;
				component.sellEngine = msg.sellEngine;
				component.preis = msg.preis;
				component.gewinnbeteiligung = msg.gewinnbeteiligung;
				component.specialPlatformS_ = null;
				component.marktdominanz = msg.marktdominanz;
			}
			return;
		}
		engineScript engineScript2 = eF_.CreateEngine();
		if ((bool)engineScript2)
		{
			engineScript2.myID = msg.engineID;
			engineScript2.ownerID = msg.ownerID;
			engineScript2.isUnlocked = msg.isUnlocked;
			if (msg.ownerID != -1)
			{
				engineScript2.isUnlocked = true;
			}
			engineScript2.gekauft = msg.gekauft;
			engineScript2.myName = msg.myName;
			engineScript2.features = (bool[])msg.features.Clone();
			engineScript2.spezialgenre = msg.spezialgenre;
			engineScript2.spezialplatform = msg.spezialplatform;
			engineScript2.sellEngine = msg.sellEngine;
			engineScript2.preis = msg.preis;
			engineScript2.gewinnbeteiligung = msg.gewinnbeteiligung;
			engineScript2.specialPlatformS_ = null;
			engineScript2.marktdominanz = msg.marktdominanz;
			engineScript2.Init();
			guiMain_.CreateTopNewsNpcEngine(engineScript2.GetName());
			string text = tS_.GetText(1270);
			text = text.Replace("<NAME>", msg.myName);
			guiMain_.AddChat(msg.ownerID, text);
		}
	}

	public void SERVER_Send_Chat(int playerID_, string c)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Chat()");
			NetworkServer.SendToAll(new s_Chat
			{
				playerID = playerID_,
				text = c
			});
		}
	}

	public void SERVER_Get_Chat(s_Chat msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Chat()");
			FindScripts();
			guiMain_.AddChat(msg.playerID, msg.text);
		}
	}

	public void SERVER_Send_ExklusivKonsolenSells(gameScript script_, long i)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_ExklusivKonsolenSells()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_exklusivKonsolenSells
				{
					gameID = script_.myID,
					exklusivKonsolenSells = i
				});
			}
		}
	}

	public void SERVER_Get_ExklusivKonsolenSells(s_exklusivKonsolenSells msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_ExklusivKonsolenSells()");
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.myID = msg.gameID;
				component.exklusivKonsolenSells = msg.exklusivKonsolenSells;
			}
		}
	}

	public void SERVER_Send_GameAnkuendigung(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameAnkuendigung()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameAnkuendigung
				{
					gameID = script_.myID
				});
			}
		}
	}

	public void SERVER_Get_GameAnkeundigung(s_GameAnkuendigung msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameAnkuendigung()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component && component.ownerID != mS_.myID && component.publisherID != mS_.myID)
			{
				component.angekuendigt = true;
				string text = tS_.GetText(2058);
				text = text.Replace("<NAME1>", component.GetDeveloperName());
				text = text.Replace("<NAME2>", component.GetNameWithTag());
				guiMain_.CreateTopNewsGameAnkuendigung(text);
			}
		}
	}

	public void SERVER_Send_GameIpSell(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameIpSell()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameIpSell
				{
					gameID = script_.myID,
					ipToSell = script_.ipToSell
				});
			}
		}
	}

	public void SERVER_Get_GameIpSell(s_GameIpSell msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameIpSell()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.ipToSell = msg.ipToSell;
			}
		}
	}

	public void SERVER_Send_GameOwner(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameOwner()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameOwner
				{
					gameID = script_.myID,
					ownerID = script_.ownerID
				});
			}
		}
	}

	public void SERVER_Get_GameOwner(s_GameOwner msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameOwner()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component)
			{
				component.ipToSell = false;
				component.ownerS_ = null;
				component.ownerID = msg.ownerID;
			}
		}
	}

	public void SERVER_Send_GameDestroy(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameDestroy()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameDestroy
				{
					gameID = script_.myID
				});
			}
		}
	}

	public void SERVER_Get_GameDestroy(s_GameDestroy msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameDestroy()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component && component.ownerID != mS_.myID && component.publisherID != mS_.myID)
			{
				component.gameObject.tag = "GameRemoved";
				Object.Destroy(game);
				games_.FindGames();
			}
		}
	}

	public void SERVER_Send_GameRemoveFromMarket(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameRemoveFromMarket()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameRemoveFromMarket
				{
					gameID = script_.myID
				});
			}
		}
	}

	public void SERVER_Get_GameRemoveFromMarket(s_GameRemoveFromMarket msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameRemoveFromMarket()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component && component.ownerID != mS_.myID && component.publisherID != mS_.myID)
			{
				component.RemoveFromMarket();
			}
		}
	}

	public void SERVER_Send_GameIpPoints(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameIpPoints()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameIpPoints
				{
					gameID = script_.myID,
					ipPunkte = script_.ipPunkte,
					ipTime = script_.ipTime
				});
			}
		}
	}

	public void SERVER_Get_GameIpPoints(s_GameIpPoints msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameIpPoints()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if ((bool)game)
		{
			gameScript component = game.GetComponent<gameScript>();
			if ((bool)component && component.ownerID != mS_.myID && component.publisherID != mS_.myID)
			{
				component.ipPunkte = msg.ipPunkte;
				component.ipTime = msg.ipTime;
			}
		}
	}

	public void SERVER_Send_GameSell(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameSell()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_GameSell
				{
					gameID = script_.myID,
					isOnMarket = script_.isOnMarket,
					weeksOnMarket = script_.weeksOnMarket,
					sellsTotal = script_.sellsTotal,
					sellsTotalStandard = script_.sellsTotalStandard,
					sellsTotalDeluxe = script_.sellsTotalDeluxe,
					sellsTotalCollectors = script_.sellsTotalCollectors,
					sellsTotalOnline = script_.sellsTotalOnline,
					abonnements = script_.abonnements,
					abonnementsWoche = script_.abonnementsWoche,
					bestAbonnements = script_.bestAbonnements,
					exklusivKonsolenSells = script_.exklusivKonsolenSells,
					userPositiv = script_.userPositiv,
					userNegativ = script_.userNegativ,
					costs_entwicklung = script_.costs_entwicklung,
					costs_mitarbeiter = script_.costs_mitarbeiter,
					costs_marketing = script_.costs_marketing,
					costs_enginegebuehren = script_.costs_enginegebuehren,
					costs_server = script_.costs_server,
					costs_production = script_.costs_production,
					costs_updates = script_.costs_updates,
					points_gameplay = script_.points_gameplay,
					points_grafik = script_.points_grafik,
					points_sound = script_.points_sound,
					points_technik = script_.points_technik,
					points_bugs = script_.points_bugs,
					points_bugsInvis = script_.points_bugsInvis,
					umsatzTotal = script_.umsatzTotal,
					umsatzInApp = script_.umsatzInApp,
					umsatzAbos = script_.umsatzAbos,
					bestChartPosition = script_.bestChartPosition,
					lastChartPosition = script_.lastChartPosition,
					f2pInteresse = script_.f2pInteresse,
					mmoInteresse = script_.mmoInteresse,
					vorbestellungen = script_.vorbestellungen,
					realsticPower = script_.realsticPower,
					hype = script_.hype,
					stornierungen = script_.stornierungen,
					commercialFlop = script_.commercialFlop,
					commercialHit = script_.commercialHit,
					freigabeBudget = script_.freigabeBudget,
					releaseDate = script_.releaseDate,
					inAppPurchaseWeek = script_.inAppPurchaseWeek,
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					verkaufspreis = (int[])script_.verkaufspreis.Clone()
				});
			}
		}
	}

	public void SERVER_Get_GameSell(s_GameSell msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_GameSell()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		if (!game)
		{
			return;
		}
		gameScript component = game.GetComponent<gameScript>();
		if (!component)
		{
			return;
		}
		bool isOnMarket = component.isOnMarket;
		if (component.ownerID != mS_.myID && component.publisherID != mS_.myID)
		{
			component.isOnMarket = msg.isOnMarket;
			component.weeksOnMarket = msg.weeksOnMarket;
			component.sellsTotal = msg.sellsTotal;
			component.sellsTotalStandard = msg.sellsTotalStandard;
			component.sellsTotalDeluxe = msg.sellsTotalDeluxe;
			component.sellsTotalCollectors = msg.sellsTotalCollectors;
			component.sellsTotalOnline = msg.sellsTotalOnline;
			component.abonnements = msg.abonnements;
			component.abonnementsWoche = msg.abonnementsWoche;
			component.bestAbonnements = msg.bestAbonnements;
			component.exklusivKonsolenSells = msg.exklusivKonsolenSells;
			component.userPositiv = msg.userPositiv;
			component.userNegativ = msg.userNegativ;
			component.costs_entwicklung = msg.costs_entwicklung;
			component.costs_mitarbeiter = msg.costs_mitarbeiter;
			component.costs_marketing = msg.costs_marketing;
			component.costs_enginegebuehren = msg.costs_enginegebuehren;
			component.costs_server = msg.costs_server;
			component.costs_production = msg.costs_production;
			component.costs_updates = msg.costs_updates;
			component.points_gameplay = msg.points_gameplay;
			component.points_grafik = msg.points_grafik;
			component.points_sound = msg.points_sound;
			component.points_technik = msg.points_technik;
			component.points_bugs = msg.points_bugs;
			component.points_bugsInvis = msg.points_bugsInvis;
			component.umsatzTotal = msg.umsatzTotal;
			component.umsatzInApp = msg.umsatzInApp;
			component.umsatzAbos = msg.umsatzAbos;
			component.bestChartPosition = msg.bestChartPosition;
			component.lastChartPosition = msg.lastChartPosition;
			component.f2pInteresse = msg.f2pInteresse;
			component.mmoInteresse = msg.mmoInteresse;
			component.vorbestellungen = msg.vorbestellungen;
			component.realsticPower = msg.realsticPower;
			component.hype = msg.hype;
			component.stornierungen = msg.stornierungen;
			component.commercialFlop = msg.commercialFlop;
			component.commercialHit = msg.commercialHit;
			component.freigabeBudget = msg.freigabeBudget;
			component.releaseDate = msg.releaseDate;
			component.inAppPurchaseWeek = msg.inAppPurchaseWeek;
			component.sellsPerWeek = (int[])msg.sellsPerWeek.Clone();
			component.verkaufspreis = (int[])msg.verkaufspreis.Clone();
			if (component.typ_contractGame && component.developerID == mS_.myID)
			{
				component.AddFans(component.sellsPerWeek[0]);
			}
			if (isOnMarket && !component.isOnMarket && component.GetPublisherOrDeveloperIsTochterfirma() && (bool)guiMain_)
			{
				guiMain_.OpenTochterfirmaAbrechnung(component);
			}
			games_.UpdateChartsWeek();
			guiMain_.UpdateCharts();
		}
	}

	public void SERVER_Send_Topics()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Topics()");
			FindScripts();
			NetworkServer.SendToAll(new s_Topics
			{
				RES_POINTS = themes_.RES_POINTS,
				themes_MGSR = (int[])themes_.themes_MGSR.Clone(),
				themes_FITGENRE = (bool[])themes_.Return1DimensionArray_FITGENRE().Clone()
			});
		}
	}

	public void SERVER_Get_Topics(s_Topics msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Topics()");
			FindScripts();
			themes_.RES_POINTS = msg.RES_POINTS;
			themes_.themes_MGSR = (int[])msg.themes_MGSR.Clone();
			themes_.Copy2DimensionArray_FITGENRE(msg.themes_FITGENRE);
			themes_.InitArrays(themes_.themes_MGSR.Length);
			mS_.MULTIPLAYER_UnlockRandomTopicsForClients();
		}
	}

	public void SERVER_Send_GameplayFeatures()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameplayFeatures()");
			FindScripts();
			NetworkServer.SendToAll(new s_GameplayFeatures
			{
				gameplayFeatures_TYP = (int[])gF_.gameplayFeatures_TYP.Clone(),
				gameplayFeatures_RES_POINTS = (int[])gF_.gameplayFeatures_RES_POINTS.Clone(),
				gameplayFeatures_RES_POINTS_LEFT = (float[])gF_.gameplayFeatures_RES_POINTS_LEFT.Clone(),
				gameplayFeatures_PRICE = (int[])gF_.gameplayFeatures_PRICE.Clone(),
				gameplayFeatures_DEV_COSTS = (int[])gF_.gameplayFeatures_DEV_COSTS.Clone(),
				gameplayFeatures_DATE_YEAR = (int[])gF_.gameplayFeatures_DATE_YEAR.Clone(),
				gameplayFeatures_DATE_MONTH = (int[])gF_.gameplayFeatures_DATE_MONTH.Clone(),
				gameplayFeatures_GAMEPLAY = (int[])gF_.gameplayFeatures_GAMEPLAY.Clone(),
				gameplayFeatures_GRAPHIC = (int[])gF_.gameplayFeatures_GRAPHIC.Clone(),
				gameplayFeatures_SOUND = (int[])gF_.gameplayFeatures_SOUND.Clone(),
				gameplayFeatures_TECHNIK = (int[])gF_.gameplayFeatures_TECHNIK.Clone(),
				gameplayFeatures_LEVEL = (int[])gF_.gameplayFeatures_LEVEL.Clone(),
				gameplayFeatures_NEED_GAMEPLAY_FEATURE = (int[])gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE.Clone(),
				gameplayFeatures_UNLOCK = (bool[])gF_.gameplayFeatures_UNLOCK.Clone(),
				gameplayFeatures_INTERNET = (bool[])gF_.gameplayFeatures_INTERNET.Clone(),
				gameplayFeatures_ICONFILE = (string[])gF_.gameplayFeatures_ICONFILE.Clone(),
				gameplayFeatures_NAME_EN = (string[])gF_.gameplayFeatures_NAME_EN.Clone(),
				gameplayFeatures_NAME_GE = (string[])gF_.gameplayFeatures_NAME_GE.Clone(),
				gameplayFeatures_NAME_TU = (string[])gF_.gameplayFeatures_NAME_TU.Clone(),
				gameplayFeatures_NAME_CH = (string[])gF_.gameplayFeatures_NAME_CH.Clone(),
				gameplayFeatures_NAME_FR = (string[])gF_.gameplayFeatures_NAME_FR.Clone(),
				gameplayFeatures_NAME_PB = (string[])gF_.gameplayFeatures_NAME_PB.Clone(),
				gameplayFeatures_NAME_CT = (string[])gF_.gameplayFeatures_NAME_CT.Clone(),
				gameplayFeatures_NAME_HU = (string[])gF_.gameplayFeatures_NAME_HU.Clone(),
				gameplayFeatures_NAME_ES = (string[])gF_.gameplayFeatures_NAME_ES.Clone(),
				gameplayFeatures_NAME_CZ = (string[])gF_.gameplayFeatures_NAME_CZ.Clone(),
				gameplayFeatures_NAME_KO = (string[])gF_.gameplayFeatures_NAME_KO.Clone(),
				gameplayFeatures_NAME_RU = (string[])gF_.gameplayFeatures_NAME_RU.Clone(),
				gameplayFeatures_NAME_IT = (string[])gF_.gameplayFeatures_NAME_IT.Clone(),
				gameplayFeatures_NAME_AR = (string[])gF_.gameplayFeatures_NAME_AR.Clone(),
				gameplayFeatures_NAME_JA = (string[])gF_.gameplayFeatures_NAME_JA.Clone(),
				gameplayFeatures_NAME_PL = (string[])gF_.gameplayFeatures_NAME_PL.Clone(),
				gameplayFeatures_NAME_UA = (string[])gF_.gameplayFeatures_NAME_UA.Clone(),
				gameplayFeatures_NAME_TH = (string[])gF_.gameplayFeatures_NAME_TH.Clone(),
				gameplayFeatures_DESC_EN = (string[])gF_.gameplayFeatures_DESC_EN.Clone(),
				gameplayFeatures_DESC_GE = (string[])gF_.gameplayFeatures_DESC_GE.Clone(),
				gameplayFeatures_DESC_TU = (string[])gF_.gameplayFeatures_DESC_TU.Clone(),
				gameplayFeatures_DESC_CH = (string[])gF_.gameplayFeatures_DESC_CH.Clone(),
				gameplayFeatures_DESC_FR = (string[])gF_.gameplayFeatures_DESC_FR.Clone(),
				gameplayFeatures_DESC_PB = (string[])gF_.gameplayFeatures_DESC_PB.Clone(),
				gameplayFeatures_DESC_CT = (string[])gF_.gameplayFeatures_DESC_CT.Clone(),
				gameplayFeatures_DESC_HU = (string[])gF_.gameplayFeatures_DESC_HU.Clone(),
				gameplayFeatures_DESC_ES = (string[])gF_.gameplayFeatures_DESC_ES.Clone(),
				gameplayFeatures_DESC_CZ = (string[])gF_.gameplayFeatures_DESC_CZ.Clone(),
				gameplayFeatures_DESC_KO = (string[])gF_.gameplayFeatures_DESC_KO.Clone(),
				gameplayFeatures_DESC_RU = (string[])gF_.gameplayFeatures_DESC_RU.Clone(),
				gameplayFeatures_DESC_IT = (string[])gF_.gameplayFeatures_DESC_IT.Clone(),
				gameplayFeatures_DESC_AR = (string[])gF_.gameplayFeatures_DESC_AR.Clone(),
				gameplayFeatures_DESC_JA = (string[])gF_.gameplayFeatures_DESC_JA.Clone(),
				gameplayFeatures_DESC_PL = (string[])gF_.gameplayFeatures_DESC_PL.Clone(),
				gameplayFeatures_DESC_UA = (string[])gF_.gameplayFeatures_DESC_UA.Clone(),
				gameplayFeatures_DESC_TH = (string[])gF_.gameplayFeatures_DESC_TH.Clone(),
				gameplayFeatures_GOOD = (bool[])gF_.Return1DimensionArray_GOOD().Clone(),
				gameplayFeatures_BAD = (bool[])gF_.Return1DimensionArray_BAD().Clone(),
				gameplayFeatures_LOCKPLATFORM = (bool[])gF_.Return1DimensionArray_LOCKPLATFORM().Clone()
			});
		}
	}

	public void SERVER_Get_GameplayFeatures(s_GameplayFeatures msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GameplayFeatures()");
			FindScripts();
			gF_.gameplayFeatures_TYP = (int[])msg.gameplayFeatures_TYP.Clone();
			gF_.gameplayFeatures_RES_POINTS = (int[])msg.gameplayFeatures_RES_POINTS.Clone();
			gF_.gameplayFeatures_RES_POINTS_LEFT = (float[])msg.gameplayFeatures_RES_POINTS_LEFT.Clone();
			gF_.gameplayFeatures_PRICE = (int[])msg.gameplayFeatures_PRICE.Clone();
			gF_.gameplayFeatures_DEV_COSTS = (int[])msg.gameplayFeatures_DEV_COSTS.Clone();
			gF_.gameplayFeatures_DATE_YEAR = (int[])msg.gameplayFeatures_DATE_YEAR.Clone();
			gF_.gameplayFeatures_DATE_MONTH = (int[])msg.gameplayFeatures_DATE_MONTH.Clone();
			gF_.gameplayFeatures_GAMEPLAY = (int[])msg.gameplayFeatures_GAMEPLAY.Clone();
			gF_.gameplayFeatures_GRAPHIC = (int[])msg.gameplayFeatures_GRAPHIC.Clone();
			gF_.gameplayFeatures_SOUND = (int[])msg.gameplayFeatures_SOUND.Clone();
			gF_.gameplayFeatures_TECHNIK = (int[])msg.gameplayFeatures_TECHNIK.Clone();
			gF_.gameplayFeatures_LEVEL = (int[])msg.gameplayFeatures_LEVEL.Clone();
			gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE = (int[])msg.gameplayFeatures_NEED_GAMEPLAY_FEATURE.Clone();
			gF_.gameplayFeatures_UNLOCK = (bool[])msg.gameplayFeatures_UNLOCK.Clone();
			gF_.gameplayFeatures_INTERNET = (bool[])msg.gameplayFeatures_INTERNET.Clone();
			gF_.gameplayFeatures_ICONFILE = (string[])msg.gameplayFeatures_ICONFILE.Clone();
			gF_.gameplayFeatures_NAME_EN = (string[])msg.gameplayFeatures_NAME_EN.Clone();
			gF_.gameplayFeatures_NAME_GE = (string[])msg.gameplayFeatures_NAME_GE.Clone();
			gF_.gameplayFeatures_NAME_TU = (string[])msg.gameplayFeatures_NAME_TU.Clone();
			gF_.gameplayFeatures_NAME_CH = (string[])msg.gameplayFeatures_NAME_CH.Clone();
			gF_.gameplayFeatures_NAME_FR = (string[])msg.gameplayFeatures_NAME_FR.Clone();
			gF_.gameplayFeatures_NAME_PB = (string[])msg.gameplayFeatures_NAME_PB.Clone();
			gF_.gameplayFeatures_NAME_CT = (string[])msg.gameplayFeatures_NAME_CT.Clone();
			gF_.gameplayFeatures_NAME_HU = (string[])msg.gameplayFeatures_NAME_HU.Clone();
			gF_.gameplayFeatures_NAME_ES = (string[])msg.gameplayFeatures_NAME_ES.Clone();
			gF_.gameplayFeatures_NAME_CZ = (string[])msg.gameplayFeatures_NAME_CZ.Clone();
			gF_.gameplayFeatures_NAME_KO = (string[])msg.gameplayFeatures_NAME_KO.Clone();
			gF_.gameplayFeatures_NAME_RU = (string[])msg.gameplayFeatures_NAME_RU.Clone();
			gF_.gameplayFeatures_NAME_IT = (string[])msg.gameplayFeatures_NAME_IT.Clone();
			gF_.gameplayFeatures_NAME_AR = (string[])msg.gameplayFeatures_NAME_AR.Clone();
			gF_.gameplayFeatures_NAME_JA = (string[])msg.gameplayFeatures_NAME_JA.Clone();
			gF_.gameplayFeatures_NAME_PL = (string[])msg.gameplayFeatures_NAME_PL.Clone();
			gF_.gameplayFeatures_NAME_UA = (string[])msg.gameplayFeatures_NAME_UA.Clone();
			gF_.gameplayFeatures_NAME_TH = (string[])msg.gameplayFeatures_NAME_TH.Clone();
			gF_.gameplayFeatures_DESC_EN = (string[])msg.gameplayFeatures_DESC_EN.Clone();
			gF_.gameplayFeatures_DESC_GE = (string[])msg.gameplayFeatures_DESC_GE.Clone();
			gF_.gameplayFeatures_DESC_TU = (string[])msg.gameplayFeatures_DESC_TU.Clone();
			gF_.gameplayFeatures_DESC_CH = (string[])msg.gameplayFeatures_DESC_CH.Clone();
			gF_.gameplayFeatures_DESC_FR = (string[])msg.gameplayFeatures_DESC_FR.Clone();
			gF_.gameplayFeatures_DESC_PB = (string[])msg.gameplayFeatures_DESC_PB.Clone();
			gF_.gameplayFeatures_DESC_CT = (string[])msg.gameplayFeatures_DESC_CT.Clone();
			gF_.gameplayFeatures_DESC_HU = (string[])msg.gameplayFeatures_DESC_HU.Clone();
			gF_.gameplayFeatures_DESC_ES = (string[])msg.gameplayFeatures_DESC_ES.Clone();
			gF_.gameplayFeatures_DESC_CZ = (string[])msg.gameplayFeatures_DESC_CZ.Clone();
			gF_.gameplayFeatures_DESC_KO = (string[])msg.gameplayFeatures_DESC_KO.Clone();
			gF_.gameplayFeatures_DESC_RU = (string[])msg.gameplayFeatures_DESC_RU.Clone();
			gF_.gameplayFeatures_DESC_IT = (string[])msg.gameplayFeatures_DESC_IT.Clone();
			gF_.gameplayFeatures_DESC_AR = (string[])msg.gameplayFeatures_DESC_AR.Clone();
			gF_.gameplayFeatures_DESC_JA = (string[])msg.gameplayFeatures_DESC_JA.Clone();
			gF_.gameplayFeatures_DESC_PL = (string[])msg.gameplayFeatures_DESC_PL.Clone();
			gF_.gameplayFeatures_DESC_UA = (string[])msg.gameplayFeatures_DESC_UA.Clone();
			gF_.gameplayFeatures_DESC_TH = (string[])msg.gameplayFeatures_DESC_TH.Clone();
			gF_.Copy2DimensionArray_GOOD(msg.gameplayFeatures_GOOD);
			gF_.Copy2DimensionArray_BAD(msg.gameplayFeatures_BAD);
			gF_.Copy2DimensionArray_LOCKPLATFORM(msg.gameplayFeatures_LOCKPLATFORM);
			gF_.Init();
		}
	}

	public void SERVER_Send_Genres(int id_, mpPlayer mpPlayer_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Genres()");
			FindScripts();
			NetworkServer.SendToAll(new s_Genres
			{
				genres_BELIEBTHEIT = (float[])genres_.genres_BELIEBTHEIT.Clone(),
				genres_BELIEBTHEIT_SOLL = (bool[])genres_.genres_BELIEBTHEIT_SOLL.Clone(),
				genres_RES_POINTS = (int[])genres_.genres_RES_POINTS.Clone(),
				genres_RES_POINTS_LEFT = (float[])genres_.genres_RES_POINTS_LEFT.Clone(),
				genres_PRICE = (int[])genres_.genres_PRICE.Clone(),
				genres_DEV_COSTS = (int[])genres_.genres_DEV_COSTS.Clone(),
				genres_DATE_YEAR = (int[])genres_.genres_DATE_YEAR.Clone(),
				genres_DATE_MONTH = (int[])genres_.genres_DATE_MONTH.Clone(),
				genres_LEVEL = (int[])genres_.genres_LEVEL.Clone(),
				genres_UNLOCK = (bool[])genres_.genres_UNLOCK.Clone(),
				genres_SUC_YEAR = (bool[])genres_.genres_SUC_YEAR.Clone(),
				genres_GAMEPLAY = (float[])genres_.genres_GAMEPLAY.Clone(),
				genres_GRAPHIC = (float[])genres_.genres_GRAPHIC.Clone(),
				genres_SOUND = (float[])genres_.genres_SOUND.Clone(),
				genres_CONTROL = (float[])genres_.genres_CONTROL.Clone(),
				genres_ICONFILE = (string[])genres_.genres_ICONFILE.Clone(),
				genres_NAME_EN = (string[])genres_.genres_NAME_EN.Clone(),
				genres_NAME_GE = (string[])genres_.genres_NAME_GE.Clone(),
				genres_NAME_TU = (string[])genres_.genres_NAME_TU.Clone(),
				genres_NAME_CH = (string[])genres_.genres_NAME_CH.Clone(),
				genres_NAME_FR = (string[])genres_.genres_NAME_FR.Clone(),
				genres_NAME_PB = (string[])genres_.genres_NAME_PB.Clone(),
				genres_NAME_HU = (string[])genres_.genres_NAME_HU.Clone(),
				genres_NAME_CT = (string[])genres_.genres_NAME_CT.Clone(),
				genres_NAME_ES = (string[])genres_.genres_NAME_ES.Clone(),
				genres_NAME_PL = (string[])genres_.genres_NAME_PL.Clone(),
				genres_NAME_CZ = (string[])genres_.genres_NAME_CZ.Clone(),
				genres_NAME_KO = (string[])genres_.genres_NAME_KO.Clone(),
				genres_NAME_IT = (string[])genres_.genres_NAME_IT.Clone(),
				genres_NAME_AR = (string[])genres_.genres_NAME_AR.Clone(),
				genres_NAME_JA = (string[])genres_.genres_NAME_JA.Clone(),
				genres_NAME_UA = (string[])genres_.genres_NAME_UA.Clone(),
				genres_NAME_TH = (string[])genres_.genres_NAME_TH.Clone(),
				genres_NAME_RU = (string[])genres_.genres_NAME_RU.Clone(),
				genres_DESC_EN = (string[])genres_.genres_DESC_EN.Clone(),
				genres_DESC_GE = (string[])genres_.genres_DESC_GE.Clone(),
				genres_DESC_TU = (string[])genres_.genres_DESC_TU.Clone(),
				genres_DESC_CH = (string[])genres_.genres_DESC_CH.Clone(),
				genres_DESC_FR = (string[])genres_.genres_DESC_FR.Clone(),
				genres_DESC_PB = (string[])genres_.genres_DESC_PB.Clone(),
				genres_DESC_HU = (string[])genres_.genres_DESC_HU.Clone(),
				genres_DESC_CT = (string[])genres_.genres_DESC_CT.Clone(),
				genres_DESC_ES = (string[])genres_.genres_DESC_ES.Clone(),
				genres_DESC_PL = (string[])genres_.genres_DESC_PL.Clone(),
				genres_DESC_CZ = (string[])genres_.genres_DESC_CZ.Clone(),
				genres_DESC_KO = (string[])genres_.genres_DESC_KO.Clone(),
				genres_DESC_IT = (string[])genres_.genres_DESC_IT.Clone(),
				genres_DESC_AR = (string[])genres_.genres_DESC_AR.Clone(),
				genres_DESC_JA = (string[])genres_.genres_DESC_JA.Clone(),
				genres_DESC_UA = (string[])genres_.genres_DESC_UA.Clone(),
				genres_DESC_TH = (string[])genres_.genres_DESC_TH.Clone(),
				genres_DESC_RU = (string[])genres_.genres_DESC_RU.Clone(),
				genres_FANS = (int[])genres_.genres_FANS.Clone(),
				genres_MARKT = (int[])genres_.genres_MARKT.Clone(),
				genres_TARGETGROUP = (bool[])genres_.Return1DimensionArray_TARGETGROUP().Clone(),
				genres_COMBINATION = (bool[])genres_.Return1DimensionArray_COMBINATION().Clone(),
				genres_PLATFORM_SELLS = (int[])genres_.Return1DimensionArray_PLATFORM_SELLS().Clone(),
				genres_FOCUS = (int[])genres_.Return1DimensionArray_FOCUS().Clone(),
				genres_ALIGN = (int[])genres_.Return1DimensionArray_ALIGN().Clone()
			});
		}
	}

	public void SERVER_Get_Genres(s_Genres msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Genres()");
			if (INIT_GENRES)
			{
				Debug.Log("SERVER_Get_Genres() -> BREAK");
				return;
			}
			INIT_GENRES = true;
			FindScripts();
			genres_.genres_BELIEBTHEIT = (float[])msg.genres_BELIEBTHEIT.Clone();
			genres_.genres_BELIEBTHEIT_SOLL = (bool[])msg.genres_BELIEBTHEIT_SOLL.Clone();
			genres_.genres_RES_POINTS = (int[])msg.genres_RES_POINTS.Clone();
			genres_.genres_RES_POINTS_LEFT = (float[])msg.genres_RES_POINTS_LEFT.Clone();
			genres_.genres_PRICE = (int[])msg.genres_PRICE.Clone();
			genres_.genres_DEV_COSTS = (int[])msg.genres_DEV_COSTS.Clone();
			genres_.genres_DATE_YEAR = (int[])msg.genres_DATE_YEAR.Clone();
			genres_.genres_DATE_MONTH = (int[])msg.genres_DATE_MONTH.Clone();
			genres_.genres_LEVEL = (int[])msg.genres_LEVEL.Clone();
			genres_.genres_UNLOCK = (bool[])msg.genres_UNLOCK.Clone();
			genres_.genres_SUC_YEAR = (bool[])msg.genres_SUC_YEAR.Clone();
			genres_.genres_GAMEPLAY = (float[])msg.genres_GAMEPLAY.Clone();
			genres_.genres_GRAPHIC = (float[])msg.genres_GRAPHIC.Clone();
			genres_.genres_SOUND = (float[])msg.genres_SOUND.Clone();
			genres_.genres_CONTROL = (float[])msg.genres_CONTROL.Clone();
			genres_.genres_ICONFILE = (string[])msg.genres_ICONFILE.Clone();
			genres_.genres_NAME_EN = (string[])msg.genres_NAME_EN.Clone();
			genres_.genres_NAME_GE = (string[])msg.genres_NAME_GE.Clone();
			genres_.genres_NAME_TU = (string[])msg.genres_NAME_TU.Clone();
			genres_.genres_NAME_CH = (string[])msg.genres_NAME_CH.Clone();
			genres_.genres_NAME_FR = (string[])msg.genres_NAME_FR.Clone();
			genres_.genres_NAME_PB = (string[])msg.genres_NAME_PB.Clone();
			genres_.genres_NAME_HU = (string[])msg.genres_NAME_HU.Clone();
			genres_.genres_NAME_CT = (string[])msg.genres_NAME_CT.Clone();
			genres_.genres_NAME_ES = (string[])msg.genres_NAME_ES.Clone();
			genres_.genres_NAME_PL = (string[])msg.genres_NAME_PL.Clone();
			genres_.genres_NAME_CZ = (string[])msg.genres_NAME_CZ.Clone();
			genres_.genres_NAME_KO = (string[])msg.genres_NAME_KO.Clone();
			genres_.genres_NAME_IT = (string[])msg.genres_NAME_IT.Clone();
			genres_.genres_NAME_AR = (string[])msg.genres_NAME_AR.Clone();
			genres_.genres_NAME_JA = (string[])msg.genres_NAME_JA.Clone();
			genres_.genres_NAME_UA = (string[])msg.genres_NAME_UA.Clone();
			genres_.genres_NAME_TH = (string[])msg.genres_NAME_TH.Clone();
			genres_.genres_NAME_RU = (string[])msg.genres_NAME_RU.Clone();
			genres_.genres_DESC_EN = (string[])msg.genres_DESC_EN.Clone();
			genres_.genres_DESC_GE = (string[])msg.genres_DESC_GE.Clone();
			genres_.genres_DESC_TU = (string[])msg.genres_DESC_TU.Clone();
			genres_.genres_DESC_CH = (string[])msg.genres_DESC_CH.Clone();
			genres_.genres_DESC_FR = (string[])msg.genres_DESC_FR.Clone();
			genres_.genres_DESC_PB = (string[])msg.genres_DESC_PB.Clone();
			genres_.genres_DESC_HU = (string[])msg.genres_DESC_HU.Clone();
			genres_.genres_DESC_CT = (string[])msg.genres_DESC_CT.Clone();
			genres_.genres_DESC_ES = (string[])msg.genres_DESC_ES.Clone();
			genres_.genres_DESC_PL = (string[])msg.genres_DESC_PL.Clone();
			genres_.genres_DESC_CZ = (string[])msg.genres_DESC_CZ.Clone();
			genres_.genres_DESC_KO = (string[])msg.genres_DESC_KO.Clone();
			genres_.genres_DESC_IT = (string[])msg.genres_DESC_IT.Clone();
			genres_.genres_DESC_AR = (string[])msg.genres_DESC_AR.Clone();
			genres_.genres_DESC_JA = (string[])msg.genres_DESC_JA.Clone();
			genres_.genres_DESC_UA = (string[])msg.genres_DESC_UA.Clone();
			genres_.genres_DESC_TH = (string[])msg.genres_DESC_TH.Clone();
			genres_.genres_DESC_RU = (string[])msg.genres_DESC_RU.Clone();
			genres_.genres_FANS = (int[])msg.genres_FANS.Clone();
			genres_.genres_MARKT = (int[])msg.genres_MARKT.Clone();
			genres_.Copy2DimensionArray_TARGETGROUP(msg.genres_TARGETGROUP);
			genres_.Copy2DimensionArray_COMBINATION(msg.genres_COMBINATION);
			genres_.Copy2DimensionArray_PLATFORM_SELLS(msg.genres_PLATFORM_SELLS);
			genres_.Copy2DimensionArray_FOCUS(msg.genres_FOCUS);
			genres_.Copy2DimensionArray_ALIGN(msg.genres_ALIGN);
			genres_.Init();
			tS_.LoadContent_Themes();
			mS_.UnlockRandomThemeAndGenre();
			mpMain_.InitDropdowns();
		}
	}

	public void SERVER_Send_EngineFeatures()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_EngineFeatures()");
			FindScripts();
			NetworkServer.SendToAll(new s_EngineFeatures
			{
				engineFeatures_TYP = (int[])eF_.engineFeatures_TYP.Clone(),
				engineFeatures_RES_POINTS = (int[])eF_.engineFeatures_RES_POINTS.Clone(),
				engineFeatures_RES_POINTS_LEFT = (float[])eF_.engineFeatures_RES_POINTS_LEFT.Clone(),
				engineFeatures_PRICE = (int[])eF_.engineFeatures_PRICE.Clone(),
				engineFeatures_DEV_COSTS = (int[])eF_.engineFeatures_DEV_COSTS.Clone(),
				engineFeatures_TECH = (int[])eF_.engineFeatures_TECH.Clone(),
				engineFeatures_DATE_YEAR = (int[])eF_.engineFeatures_DATE_YEAR.Clone(),
				engineFeatures_DATE_MONTH = (int[])eF_.engineFeatures_DATE_MONTH.Clone(),
				engineFeatures_GAMEPLAY = (int[])eF_.engineFeatures_GAMEPLAY.Clone(),
				engineFeatures_GRAPHIC = (int[])eF_.engineFeatures_GRAPHIC.Clone(),
				engineFeatures_SOUND = (int[])eF_.engineFeatures_SOUND.Clone(),
				engineFeatures_TECHNIK = (int[])eF_.engineFeatures_TECHNIK.Clone(),
				engineFeatures_LEVEL = (int[])eF_.engineFeatures_LEVEL.Clone(),
				engineFeatures_UNLOCK = (bool[])eF_.engineFeatures_UNLOCK.Clone(),
				engineFeatures_ICONFILE = (string[])eF_.engineFeatures_ICONFILE.Clone(),
				engineFeatures_NAME_EN = (string[])eF_.engineFeatures_NAME_EN.Clone(),
				engineFeatures_NAME_GE = (string[])eF_.engineFeatures_NAME_GE.Clone(),
				engineFeatures_NAME_TU = (string[])eF_.engineFeatures_NAME_TU.Clone(),
				engineFeatures_NAME_CH = (string[])eF_.engineFeatures_NAME_CH.Clone(),
				engineFeatures_NAME_FR = (string[])eF_.engineFeatures_NAME_FR.Clone(),
				engineFeatures_NAME_PB = (string[])eF_.engineFeatures_NAME_PB.Clone(),
				engineFeatures_NAME_CT = (string[])eF_.engineFeatures_NAME_CT.Clone(),
				engineFeatures_NAME_HU = (string[])eF_.engineFeatures_NAME_HU.Clone(),
				engineFeatures_NAME_ES = (string[])eF_.engineFeatures_NAME_ES.Clone(),
				engineFeatures_NAME_CZ = (string[])eF_.engineFeatures_NAME_CZ.Clone(),
				engineFeatures_NAME_KO = (string[])eF_.engineFeatures_NAME_KO.Clone(),
				engineFeatures_NAME_AR = (string[])eF_.engineFeatures_NAME_AR.Clone(),
				engineFeatures_NAME_RU = (string[])eF_.engineFeatures_NAME_RU.Clone(),
				engineFeatures_NAME_IT = (string[])eF_.engineFeatures_NAME_IT.Clone(),
				engineFeatures_NAME_JA = (string[])eF_.engineFeatures_NAME_JA.Clone(),
				engineFeatures_NAME_PL = (string[])eF_.engineFeatures_NAME_PL.Clone(),
				engineFeatures_NAME_UA = (string[])eF_.engineFeatures_NAME_UA.Clone(),
				engineFeatures_NAME_TH = (string[])eF_.engineFeatures_NAME_TH.Clone(),
				engineFeatures_DESC_EN = (string[])eF_.engineFeatures_DESC_EN.Clone(),
				engineFeatures_DESC_GE = (string[])eF_.engineFeatures_DESC_GE.Clone(),
				engineFeatures_DESC_TU = (string[])eF_.engineFeatures_DESC_TU.Clone(),
				engineFeatures_DESC_CH = (string[])eF_.engineFeatures_DESC_CH.Clone(),
				engineFeatures_DESC_FR = (string[])eF_.engineFeatures_DESC_FR.Clone(),
				engineFeatures_DESC_PB = (string[])eF_.engineFeatures_DESC_PB.Clone(),
				engineFeatures_DESC_CT = (string[])eF_.engineFeatures_DESC_CT.Clone(),
				engineFeatures_DESC_HU = (string[])eF_.engineFeatures_DESC_HU.Clone(),
				engineFeatures_DESC_ES = (string[])eF_.engineFeatures_DESC_ES.Clone(),
				engineFeatures_DESC_CZ = (string[])eF_.engineFeatures_DESC_CZ.Clone(),
				engineFeatures_DESC_KO = (string[])eF_.engineFeatures_DESC_KO.Clone(),
				engineFeatures_DESC_AR = (string[])eF_.engineFeatures_DESC_AR.Clone(),
				engineFeatures_DESC_RU = (string[])eF_.engineFeatures_DESC_RU.Clone(),
				engineFeatures_DESC_IT = (string[])eF_.engineFeatures_DESC_IT.Clone(),
				engineFeatures_DESC_JA = (string[])eF_.engineFeatures_DESC_JA.Clone(),
				engineFeatures_DESC_PL = (string[])eF_.engineFeatures_DESC_PL.Clone(),
				engineFeatures_DESC_UA = (string[])eF_.engineFeatures_DESC_UA.Clone(),
				engineFeatures_DESC_TH = (string[])eF_.engineFeatures_DESC_TH.Clone()
			});
		}
	}

	public void SERVER_Get_EngineFeatures(s_EngineFeatures msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_EngineFeatures()");
			FindScripts();
			eF_.engineFeatures_TYP = (int[])msg.engineFeatures_TYP.Clone();
			eF_.engineFeatures_RES_POINTS = (int[])msg.engineFeatures_RES_POINTS.Clone();
			eF_.engineFeatures_RES_POINTS_LEFT = (float[])msg.engineFeatures_RES_POINTS_LEFT.Clone();
			eF_.engineFeatures_PRICE = (int[])msg.engineFeatures_PRICE.Clone();
			eF_.engineFeatures_DEV_COSTS = (int[])msg.engineFeatures_DEV_COSTS.Clone();
			eF_.engineFeatures_TECH = (int[])msg.engineFeatures_TECH.Clone();
			eF_.engineFeatures_DATE_YEAR = (int[])msg.engineFeatures_DATE_YEAR.Clone();
			eF_.engineFeatures_DATE_MONTH = (int[])msg.engineFeatures_DATE_MONTH.Clone();
			eF_.engineFeatures_GAMEPLAY = (int[])msg.engineFeatures_GAMEPLAY.Clone();
			eF_.engineFeatures_GRAPHIC = (int[])msg.engineFeatures_GRAPHIC.Clone();
			eF_.engineFeatures_SOUND = (int[])msg.engineFeatures_SOUND.Clone();
			eF_.engineFeatures_TECHNIK = (int[])msg.engineFeatures_TECHNIK.Clone();
			eF_.engineFeatures_LEVEL = (int[])msg.engineFeatures_LEVEL.Clone();
			eF_.engineFeatures_UNLOCK = (bool[])msg.engineFeatures_UNLOCK.Clone();
			eF_.engineFeatures_ICONFILE = (string[])msg.engineFeatures_ICONFILE.Clone();
			eF_.engineFeatures_NAME_EN = (string[])msg.engineFeatures_NAME_EN.Clone();
			eF_.engineFeatures_NAME_GE = (string[])msg.engineFeatures_NAME_GE.Clone();
			eF_.engineFeatures_NAME_TU = (string[])msg.engineFeatures_NAME_TU.Clone();
			eF_.engineFeatures_NAME_CH = (string[])msg.engineFeatures_NAME_CH.Clone();
			eF_.engineFeatures_NAME_FR = (string[])msg.engineFeatures_NAME_FR.Clone();
			eF_.engineFeatures_NAME_PB = (string[])msg.engineFeatures_NAME_PB.Clone();
			eF_.engineFeatures_NAME_CT = (string[])msg.engineFeatures_NAME_CT.Clone();
			eF_.engineFeatures_NAME_HU = (string[])msg.engineFeatures_NAME_HU.Clone();
			eF_.engineFeatures_NAME_ES = (string[])msg.engineFeatures_NAME_ES.Clone();
			eF_.engineFeatures_NAME_CZ = (string[])msg.engineFeatures_NAME_CZ.Clone();
			eF_.engineFeatures_NAME_KO = (string[])msg.engineFeatures_NAME_KO.Clone();
			eF_.engineFeatures_NAME_AR = (string[])msg.engineFeatures_NAME_AR.Clone();
			eF_.engineFeatures_NAME_RU = (string[])msg.engineFeatures_NAME_RU.Clone();
			eF_.engineFeatures_NAME_IT = (string[])msg.engineFeatures_NAME_IT.Clone();
			eF_.engineFeatures_NAME_JA = (string[])msg.engineFeatures_NAME_JA.Clone();
			eF_.engineFeatures_NAME_PL = (string[])msg.engineFeatures_NAME_PL.Clone();
			eF_.engineFeatures_NAME_UA = (string[])msg.engineFeatures_NAME_UA.Clone();
			eF_.engineFeatures_NAME_TH = (string[])msg.engineFeatures_NAME_TH.Clone();
			eF_.engineFeatures_DESC_EN = (string[])msg.engineFeatures_DESC_EN.Clone();
			eF_.engineFeatures_DESC_GE = (string[])msg.engineFeatures_DESC_GE.Clone();
			eF_.engineFeatures_DESC_TU = (string[])msg.engineFeatures_DESC_TU.Clone();
			eF_.engineFeatures_DESC_CH = (string[])msg.engineFeatures_DESC_CH.Clone();
			eF_.engineFeatures_DESC_FR = (string[])msg.engineFeatures_DESC_FR.Clone();
			eF_.engineFeatures_DESC_PB = (string[])msg.engineFeatures_DESC_PB.Clone();
			eF_.engineFeatures_DESC_CT = (string[])msg.engineFeatures_DESC_CT.Clone();
			eF_.engineFeatures_DESC_HU = (string[])msg.engineFeatures_DESC_HU.Clone();
			eF_.engineFeatures_DESC_ES = (string[])msg.engineFeatures_DESC_ES.Clone();
			eF_.engineFeatures_DESC_CZ = (string[])msg.engineFeatures_DESC_CZ.Clone();
			eF_.engineFeatures_DESC_KO = (string[])msg.engineFeatures_DESC_KO.Clone();
			eF_.engineFeatures_DESC_AR = (string[])msg.engineFeatures_DESC_AR.Clone();
			eF_.engineFeatures_DESC_RU = (string[])msg.engineFeatures_DESC_RU.Clone();
			eF_.engineFeatures_DESC_IT = (string[])msg.engineFeatures_DESC_IT.Clone();
			eF_.engineFeatures_DESC_JA = (string[])msg.engineFeatures_DESC_JA.Clone();
			eF_.engineFeatures_DESC_PL = (string[])msg.engineFeatures_DESC_PL.Clone();
			eF_.engineFeatures_DESC_UA = (string[])msg.engineFeatures_DESC_UA.Clone();
			eF_.engineFeatures_DESC_TH = (string[])msg.engineFeatures_DESC_TH.Clone();
			eF_.Init();
		}
	}

	public void SERVER_Send_HardwareFeatures()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_HardwareFeatures()");
			FindScripts();
			NetworkServer.SendToAll(new s_HardwareFeatures
			{
				hardFeat_ICONFILE = (string[])hardwareFeatures_.hardFeat_ICONFILE.Clone(),
				hardFeat_RES_POINTS = (int[])hardwareFeatures_.hardFeat_RES_POINTS.Clone(),
				hardFeat_RES_POINTS_LEFT = (float[])hardwareFeatures_.hardFeat_RES_POINTS_LEFT.Clone(),
				hardFeat_PRICE = (int[])hardwareFeatures_.hardFeat_PRICE.Clone(),
				hardFeat_DEV_COSTS = (int[])hardwareFeatures_.hardFeat_DEV_COSTS.Clone(),
				hardFeat_DATE_YEAR = (int[])hardwareFeatures_.hardFeat_DATE_YEAR.Clone(),
				hardFeat_DATE_MONTH = (int[])hardwareFeatures_.hardFeat_DATE_MONTH.Clone(),
				hardFeat_UNLOCK = (bool[])hardwareFeatures_.hardFeat_UNLOCK.Clone(),
				hardFeat_ONLYSTATIONARY = (bool[])hardwareFeatures_.hardFeat_ONLYSTATIONARY.Clone(),
				hardFeat_ONLYHANDHELD = (bool[])hardwareFeatures_.hardFeat_ONLYHANDHELD.Clone(),
				hardFeat_NEEDINTERNET = (bool[])hardwareFeatures_.hardFeat_NEEDINTERNET.Clone(),
				hardFeat_QUALITY = (float[])hardwareFeatures_.hardFeat_QUALITY.Clone(),
				hardFeat_NAME_EN = (string[])hardwareFeatures_.hardFeat_NAME_EN.Clone(),
				hardFeat_NAME_GE = (string[])hardwareFeatures_.hardFeat_NAME_GE.Clone(),
				hardFeat_NAME_TU = (string[])hardwareFeatures_.hardFeat_NAME_TU.Clone(),
				hardFeat_NAME_CH = (string[])hardwareFeatures_.hardFeat_NAME_CH.Clone(),
				hardFeat_NAME_FR = (string[])hardwareFeatures_.hardFeat_NAME_FR.Clone(),
				hardFeat_NAME_PB = (string[])hardwareFeatures_.hardFeat_NAME_PB.Clone(),
				hardFeat_NAME_CT = (string[])hardwareFeatures_.hardFeat_NAME_CT.Clone(),
				hardFeat_NAME_HU = (string[])hardwareFeatures_.hardFeat_NAME_HU.Clone(),
				hardFeat_NAME_ES = (string[])hardwareFeatures_.hardFeat_NAME_ES.Clone(),
				hardFeat_NAME_CZ = (string[])hardwareFeatures_.hardFeat_NAME_CZ.Clone(),
				hardFeat_NAME_KO = (string[])hardwareFeatures_.hardFeat_NAME_KO.Clone(),
				hardFeat_NAME_AR = (string[])hardwareFeatures_.hardFeat_NAME_AR.Clone(),
				hardFeat_NAME_RU = (string[])hardwareFeatures_.hardFeat_NAME_RU.Clone(),
				hardFeat_NAME_IT = (string[])hardwareFeatures_.hardFeat_NAME_IT.Clone(),
				hardFeat_NAME_JA = (string[])hardwareFeatures_.hardFeat_NAME_JA.Clone(),
				hardFeat_NAME_PL = (string[])hardwareFeatures_.hardFeat_NAME_PL.Clone(),
				hardFeat_NAME_UA = (string[])hardwareFeatures_.hardFeat_NAME_UA.Clone(),
				hardFeat_NAME_TH = (string[])hardwareFeatures_.hardFeat_NAME_TH.Clone(),
				hardFeat_DESC_EN = (string[])hardwareFeatures_.hardFeat_DESC_EN.Clone(),
				hardFeat_DESC_GE = (string[])hardwareFeatures_.hardFeat_DESC_GE.Clone(),
				hardFeat_DESC_TU = (string[])hardwareFeatures_.hardFeat_DESC_TU.Clone(),
				hardFeat_DESC_CH = (string[])hardwareFeatures_.hardFeat_DESC_CH.Clone(),
				hardFeat_DESC_FR = (string[])hardwareFeatures_.hardFeat_DESC_FR.Clone(),
				hardFeat_DESC_PB = (string[])hardwareFeatures_.hardFeat_DESC_PB.Clone(),
				hardFeat_DESC_CT = (string[])hardwareFeatures_.hardFeat_DESC_CT.Clone(),
				hardFeat_DESC_HU = (string[])hardwareFeatures_.hardFeat_DESC_HU.Clone(),
				hardFeat_DESC_ES = (string[])hardwareFeatures_.hardFeat_DESC_ES.Clone(),
				hardFeat_DESC_CZ = (string[])hardwareFeatures_.hardFeat_DESC_CZ.Clone(),
				hardFeat_DESC_KO = (string[])hardwareFeatures_.hardFeat_DESC_KO.Clone(),
				hardFeat_DESC_AR = (string[])hardwareFeatures_.hardFeat_DESC_AR.Clone(),
				hardFeat_DESC_RU = (string[])hardwareFeatures_.hardFeat_DESC_RU.Clone(),
				hardFeat_DESC_IT = (string[])hardwareFeatures_.hardFeat_DESC_IT.Clone(),
				hardFeat_DESC_JA = (string[])hardwareFeatures_.hardFeat_DESC_JA.Clone(),
				hardFeat_DESC_PL = (string[])hardwareFeatures_.hardFeat_DESC_PL.Clone(),
				hardFeat_DESC_UA = (string[])hardwareFeatures_.hardFeat_DESC_UA.Clone(),
				hardFeat_DESC_TH = (string[])hardwareFeatures_.hardFeat_DESC_TH.Clone()
			});
		}
	}

	public void SERVER_Get_HardwareFeatures(s_HardwareFeatures msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_HardwareFeatures()");
			FindScripts();
			hardwareFeatures_.hardFeat_ICONFILE = (string[])msg.hardFeat_ICONFILE.Clone();
			hardwareFeatures_.hardFeat_RES_POINTS = (int[])msg.hardFeat_RES_POINTS.Clone();
			hardwareFeatures_.hardFeat_RES_POINTS_LEFT = (float[])msg.hardFeat_RES_POINTS_LEFT.Clone();
			hardwareFeatures_.hardFeat_PRICE = (int[])msg.hardFeat_PRICE.Clone();
			hardwareFeatures_.hardFeat_DEV_COSTS = (int[])msg.hardFeat_DEV_COSTS.Clone();
			hardwareFeatures_.hardFeat_DATE_YEAR = (int[])msg.hardFeat_DATE_YEAR.Clone();
			hardwareFeatures_.hardFeat_DATE_MONTH = (int[])msg.hardFeat_DATE_MONTH.Clone();
			hardwareFeatures_.hardFeat_UNLOCK = (bool[])msg.hardFeat_UNLOCK.Clone();
			hardwareFeatures_.hardFeat_ONLYSTATIONARY = (bool[])msg.hardFeat_ONLYSTATIONARY.Clone();
			hardwareFeatures_.hardFeat_ONLYHANDHELD = (bool[])msg.hardFeat_ONLYHANDHELD.Clone();
			hardwareFeatures_.hardFeat_NEEDINTERNET = (bool[])msg.hardFeat_NEEDINTERNET.Clone();
			hardwareFeatures_.hardFeat_QUALITY = (float[])msg.hardFeat_QUALITY.Clone();
			hardwareFeatures_.hardFeat_NAME_EN = (string[])msg.hardFeat_NAME_EN.Clone();
			hardwareFeatures_.hardFeat_NAME_GE = (string[])msg.hardFeat_NAME_GE.Clone();
			hardwareFeatures_.hardFeat_NAME_TU = (string[])msg.hardFeat_NAME_TU.Clone();
			hardwareFeatures_.hardFeat_NAME_CH = (string[])msg.hardFeat_NAME_CH.Clone();
			hardwareFeatures_.hardFeat_NAME_FR = (string[])msg.hardFeat_NAME_FR.Clone();
			hardwareFeatures_.hardFeat_NAME_PB = (string[])msg.hardFeat_NAME_PB.Clone();
			hardwareFeatures_.hardFeat_NAME_CT = (string[])msg.hardFeat_NAME_CT.Clone();
			hardwareFeatures_.hardFeat_NAME_HU = (string[])msg.hardFeat_NAME_HU.Clone();
			hardwareFeatures_.hardFeat_NAME_ES = (string[])msg.hardFeat_NAME_ES.Clone();
			hardwareFeatures_.hardFeat_NAME_CZ = (string[])msg.hardFeat_NAME_CZ.Clone();
			hardwareFeatures_.hardFeat_NAME_KO = (string[])msg.hardFeat_NAME_KO.Clone();
			hardwareFeatures_.hardFeat_NAME_AR = (string[])msg.hardFeat_NAME_AR.Clone();
			hardwareFeatures_.hardFeat_NAME_RU = (string[])msg.hardFeat_NAME_RU.Clone();
			hardwareFeatures_.hardFeat_NAME_IT = (string[])msg.hardFeat_NAME_IT.Clone();
			hardwareFeatures_.hardFeat_NAME_JA = (string[])msg.hardFeat_NAME_JA.Clone();
			hardwareFeatures_.hardFeat_NAME_PL = (string[])msg.hardFeat_NAME_PL.Clone();
			hardwareFeatures_.hardFeat_NAME_UA = (string[])msg.hardFeat_NAME_UA.Clone();
			hardwareFeatures_.hardFeat_NAME_TH = (string[])msg.hardFeat_NAME_TH.Clone();
			hardwareFeatures_.hardFeat_DESC_EN = (string[])msg.hardFeat_DESC_EN.Clone();
			hardwareFeatures_.hardFeat_DESC_GE = (string[])msg.hardFeat_DESC_GE.Clone();
			hardwareFeatures_.hardFeat_DESC_TU = (string[])msg.hardFeat_DESC_TU.Clone();
			hardwareFeatures_.hardFeat_DESC_CH = (string[])msg.hardFeat_DESC_CH.Clone();
			hardwareFeatures_.hardFeat_DESC_FR = (string[])msg.hardFeat_DESC_FR.Clone();
			hardwareFeatures_.hardFeat_DESC_PB = (string[])msg.hardFeat_DESC_PB.Clone();
			hardwareFeatures_.hardFeat_DESC_CT = (string[])msg.hardFeat_DESC_CT.Clone();
			hardwareFeatures_.hardFeat_DESC_HU = (string[])msg.hardFeat_DESC_HU.Clone();
			hardwareFeatures_.hardFeat_DESC_ES = (string[])msg.hardFeat_DESC_ES.Clone();
			hardwareFeatures_.hardFeat_DESC_CZ = (string[])msg.hardFeat_DESC_CZ.Clone();
			hardwareFeatures_.hardFeat_DESC_KO = (string[])msg.hardFeat_DESC_KO.Clone();
			hardwareFeatures_.hardFeat_DESC_AR = (string[])msg.hardFeat_DESC_AR.Clone();
			hardwareFeatures_.hardFeat_DESC_RU = (string[])msg.hardFeat_DESC_RU.Clone();
			hardwareFeatures_.hardFeat_DESC_IT = (string[])msg.hardFeat_DESC_IT.Clone();
			hardwareFeatures_.hardFeat_DESC_JA = (string[])msg.hardFeat_DESC_JA.Clone();
			hardwareFeatures_.hardFeat_DESC_PL = (string[])msg.hardFeat_DESC_PL.Clone();
			hardwareFeatures_.hardFeat_DESC_UA = (string[])msg.hardFeat_DESC_UA.Clone();
			hardwareFeatures_.hardFeat_DESC_TH = (string[])msg.hardFeat_DESC_TH.Clone();
			hardwareFeatures_.Init();
		}
	}

	public void SERVER_Send_Hardware()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Hardware()");
			FindScripts();
			NetworkServer.SendToAll(new s_Hardware
			{
				hardware_ICONFILE = (string[])hardware_.hardware_ICONFILE.Clone(),
				hardware_TYP = (int[])hardware_.hardware_TYP.Clone(),
				hardware_RES_POINTS = (int[])hardware_.hardware_RES_POINTS.Clone(),
				hardware_RES_POINTS_LEFT = (float[])hardware_.hardware_RES_POINTS_LEFT.Clone(),
				hardware_PRICE = (int[])hardware_.hardware_PRICE.Clone(),
				hardware_DEV_COSTS = (int[])hardware_.hardware_DEV_COSTS.Clone(),
				hardware_TECH = (int[])hardware_.hardware_TECH.Clone(),
				hardware_DATE_YEAR = (int[])hardware_.hardware_DATE_YEAR.Clone(),
				hardware_DATE_MONTH = (int[])hardware_.hardware_DATE_MONTH.Clone(),
				hardware_UNLOCK = (bool[])hardware_.hardware_UNLOCK.Clone(),
				hardware_ONLYSTATIONARY = (bool[])hardware_.hardware_ONLYSTATIONARY.Clone(),
				hardware_ONLYHANDHELD = (bool[])hardware_.hardware_ONLYHANDHELD.Clone(),
				hardware_NEED1 = (int[])hardware_.hardware_NEED1.Clone(),
				hardware_NEED2 = (int[])hardware_.hardware_NEED2.Clone(),
				hardware_NAME_EN = (string[])hardware_.hardware_NAME_EN.Clone(),
				hardware_NAME_GE = (string[])hardware_.hardware_NAME_GE.Clone(),
				hardware_NAME_TU = (string[])hardware_.hardware_NAME_TU.Clone(),
				hardware_NAME_CH = (string[])hardware_.hardware_NAME_CH.Clone(),
				hardware_NAME_FR = (string[])hardware_.hardware_NAME_FR.Clone(),
				hardware_NAME_PB = (string[])hardware_.hardware_NAME_PB.Clone(),
				hardware_NAME_CT = (string[])hardware_.hardware_NAME_CT.Clone(),
				hardware_NAME_HU = (string[])hardware_.hardware_NAME_HU.Clone(),
				hardware_NAME_ES = (string[])hardware_.hardware_NAME_ES.Clone(),
				hardware_NAME_CZ = (string[])hardware_.hardware_NAME_CZ.Clone(),
				hardware_NAME_KO = (string[])hardware_.hardware_NAME_KO.Clone(),
				hardware_NAME_AR = (string[])hardware_.hardware_NAME_AR.Clone(),
				hardware_NAME_RU = (string[])hardware_.hardware_NAME_RU.Clone(),
				hardware_NAME_IT = (string[])hardware_.hardware_NAME_IT.Clone(),
				hardware_NAME_JA = (string[])hardware_.hardware_NAME_JA.Clone(),
				hardware_NAME_PL = (string[])hardware_.hardware_NAME_PL.Clone(),
				hardware_NAME_UA = (string[])hardware_.hardware_NAME_UA.Clone(),
				hardware_NAME_TH = (string[])hardware_.hardware_NAME_TH.Clone(),
				hardware_DESC_EN = (string[])hardware_.hardware_DESC_EN.Clone(),
				hardware_DESC_GE = (string[])hardware_.hardware_DESC_GE.Clone(),
				hardware_DESC_TU = (string[])hardware_.hardware_DESC_TU.Clone(),
				hardware_DESC_CH = (string[])hardware_.hardware_DESC_CH.Clone(),
				hardware_DESC_FR = (string[])hardware_.hardware_DESC_FR.Clone(),
				hardware_DESC_PB = (string[])hardware_.hardware_DESC_PB.Clone(),
				hardware_DESC_CT = (string[])hardware_.hardware_DESC_CT.Clone(),
				hardware_DESC_HU = (string[])hardware_.hardware_DESC_HU.Clone(),
				hardware_DESC_ES = (string[])hardware_.hardware_DESC_ES.Clone(),
				hardware_DESC_CZ = (string[])hardware_.hardware_DESC_CZ.Clone(),
				hardware_DESC_KO = (string[])hardware_.hardware_DESC_KO.Clone(),
				hardware_DESC_AR = (string[])hardware_.hardware_DESC_AR.Clone(),
				hardware_DESC_RU = (string[])hardware_.hardware_DESC_RU.Clone(),
				hardware_DESC_IT = (string[])hardware_.hardware_DESC_IT.Clone(),
				hardware_DESC_JA = (string[])hardware_.hardware_DESC_JA.Clone(),
				hardware_DESC_PL = (string[])hardware_.hardware_DESC_PL.Clone(),
				hardware_DESC_UA = (string[])hardware_.hardware_DESC_UA.Clone(),
				hardware_DESC_TH = (string[])hardware_.hardware_DESC_TH.Clone()
			});
		}
	}

	public void SERVER_Get_Hardware(s_Hardware msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Hardware()");
			FindScripts();
			hardware_.hardware_ICONFILE = (string[])msg.hardware_ICONFILE.Clone();
			hardware_.hardware_TYP = (int[])msg.hardware_TYP.Clone();
			hardware_.hardware_RES_POINTS = (int[])msg.hardware_RES_POINTS.Clone();
			hardware_.hardware_RES_POINTS_LEFT = (float[])msg.hardware_RES_POINTS_LEFT.Clone();
			hardware_.hardware_PRICE = (int[])msg.hardware_PRICE.Clone();
			hardware_.hardware_DEV_COSTS = (int[])msg.hardware_DEV_COSTS.Clone();
			hardware_.hardware_TECH = (int[])msg.hardware_TECH.Clone();
			hardware_.hardware_DATE_YEAR = (int[])msg.hardware_DATE_YEAR.Clone();
			hardware_.hardware_DATE_MONTH = (int[])msg.hardware_DATE_MONTH.Clone();
			hardware_.hardware_UNLOCK = (bool[])msg.hardware_UNLOCK.Clone();
			hardware_.hardware_ONLYSTATIONARY = (bool[])msg.hardware_ONLYSTATIONARY.Clone();
			hardware_.hardware_ONLYHANDHELD = (bool[])msg.hardware_ONLYHANDHELD.Clone();
			hardware_.hardware_NEED1 = (int[])msg.hardware_NEED1.Clone();
			hardware_.hardware_NEED2 = (int[])msg.hardware_NEED2.Clone();
			hardware_.hardware_NAME_EN = (string[])msg.hardware_NAME_EN.Clone();
			hardware_.hardware_NAME_GE = (string[])msg.hardware_NAME_GE.Clone();
			hardware_.hardware_NAME_TU = (string[])msg.hardware_NAME_TU.Clone();
			hardware_.hardware_NAME_CH = (string[])msg.hardware_NAME_CH.Clone();
			hardware_.hardware_NAME_FR = (string[])msg.hardware_NAME_FR.Clone();
			hardware_.hardware_NAME_PB = (string[])msg.hardware_NAME_PB.Clone();
			hardware_.hardware_NAME_CT = (string[])msg.hardware_NAME_CT.Clone();
			hardware_.hardware_NAME_HU = (string[])msg.hardware_NAME_HU.Clone();
			hardware_.hardware_NAME_ES = (string[])msg.hardware_NAME_ES.Clone();
			hardware_.hardware_NAME_CZ = (string[])msg.hardware_NAME_CZ.Clone();
			hardware_.hardware_NAME_KO = (string[])msg.hardware_NAME_KO.Clone();
			hardware_.hardware_NAME_AR = (string[])msg.hardware_NAME_AR.Clone();
			hardware_.hardware_NAME_RU = (string[])msg.hardware_NAME_RU.Clone();
			hardware_.hardware_NAME_IT = (string[])msg.hardware_NAME_IT.Clone();
			hardware_.hardware_NAME_JA = (string[])msg.hardware_NAME_JA.Clone();
			hardware_.hardware_NAME_PL = (string[])msg.hardware_NAME_PL.Clone();
			hardware_.hardware_NAME_UA = (string[])msg.hardware_NAME_UA.Clone();
			hardware_.hardware_NAME_TH = (string[])msg.hardware_NAME_TH.Clone();
			hardware_.hardware_DESC_EN = (string[])msg.hardware_DESC_EN.Clone();
			hardware_.hardware_DESC_GE = (string[])msg.hardware_DESC_GE.Clone();
			hardware_.hardware_DESC_TU = (string[])msg.hardware_DESC_TU.Clone();
			hardware_.hardware_DESC_CH = (string[])msg.hardware_DESC_CH.Clone();
			hardware_.hardware_DESC_FR = (string[])msg.hardware_DESC_FR.Clone();
			hardware_.hardware_DESC_PB = (string[])msg.hardware_DESC_PB.Clone();
			hardware_.hardware_DESC_CT = (string[])msg.hardware_DESC_CT.Clone();
			hardware_.hardware_DESC_HU = (string[])msg.hardware_DESC_HU.Clone();
			hardware_.hardware_DESC_ES = (string[])msg.hardware_DESC_ES.Clone();
			hardware_.hardware_DESC_CZ = (string[])msg.hardware_DESC_CZ.Clone();
			hardware_.hardware_DESC_KO = (string[])msg.hardware_DESC_KO.Clone();
			hardware_.hardware_DESC_AR = (string[])msg.hardware_DESC_AR.Clone();
			hardware_.hardware_DESC_RU = (string[])msg.hardware_DESC_RU.Clone();
			hardware_.hardware_DESC_IT = (string[])msg.hardware_DESC_IT.Clone();
			hardware_.hardware_DESC_JA = (string[])msg.hardware_DESC_JA.Clone();
			hardware_.hardware_DESC_PL = (string[])msg.hardware_DESC_PL.Clone();
			hardware_.hardware_DESC_UA = (string[])msg.hardware_DESC_UA.Clone();
			hardware_.hardware_DESC_TH = (string[])msg.hardware_DESC_TH.Clone();
			hardware_.Init();
		}
	}

	public void SERVER_Send_AntiCheat(antiCheatScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_AntiCheat()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_AntiCheat
				{
					myID = script_.myID,
					date_year = script_.date_year,
					date_month = script_.date_month,
					price = script_.price,
					dev_costs = script_.dev_costs,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_CT = script_.name_CT,
					name_RU = script_.name_RU,
					name_IT = script_.name_IT,
					name_JA = script_.name_JA,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					isUnlocked = script_.isUnlocked,
					effekt = script_.effekt,
					neverLooseEffect = script_.neverLooseEffect
				});
			}
		}
	}

	public void SERVER_Get_AntiCheat(s_AntiCheat msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_AntiCheat()");
		FindScripts();
		GameObject gameObject = GameObject.Find("ANTICHEAT_" + msg.myID);
		if ((bool)gameObject)
		{
			antiCheatScript component = gameObject.GetComponent<antiCheatScript>();
			if ((bool)component)
			{
				component.myID = msg.myID;
				component.date_year = msg.date_year;
				component.date_month = msg.date_month;
				component.price = msg.price;
				component.dev_costs = msg.dev_costs;
				component.name_EN = msg.name_EN;
				component.name_GE = msg.name_GE;
				component.name_TU = msg.name_TU;
				component.name_CH = msg.name_CH;
				component.name_FR = msg.name_FR;
				component.name_CT = msg.name_CT;
				component.name_RU = msg.name_RU;
				component.name_IT = msg.name_IT;
				component.name_JA = msg.name_JA;
				component.name_UA = msg.name_UA;
				component.name_TH = msg.name_TH;
				component.isUnlocked = msg.isUnlocked;
				component.effekt = msg.effekt;
				component.neverLooseEffect = msg.neverLooseEffect;
			}
			return;
		}
		antiCheatScript antiCheatScript2 = antiCheat_.CreateAntiCheat();
		if ((bool)antiCheatScript2)
		{
			antiCheatScript2.myID = msg.myID;
			antiCheatScript2.date_year = msg.date_year;
			antiCheatScript2.date_month = msg.date_month;
			antiCheatScript2.price = msg.price;
			antiCheatScript2.dev_costs = msg.dev_costs;
			antiCheatScript2.name_EN = msg.name_EN;
			antiCheatScript2.name_GE = msg.name_GE;
			antiCheatScript2.name_TU = msg.name_TU;
			antiCheatScript2.name_CH = msg.name_CH;
			antiCheatScript2.name_FR = msg.name_FR;
			antiCheatScript2.name_CT = msg.name_CT;
			antiCheatScript2.name_RU = msg.name_RU;
			antiCheatScript2.name_IT = msg.name_IT;
			antiCheatScript2.name_JA = msg.name_JA;
			antiCheatScript2.name_UA = msg.name_UA;
			antiCheatScript2.name_TH = msg.name_TH;
			antiCheatScript2.isUnlocked = msg.isUnlocked;
			antiCheatScript2.effekt = msg.effekt;
			antiCheatScript2.neverLooseEffect = msg.neverLooseEffect;
			antiCheatScript2.Init();
		}
	}

	public void SERVER_Send_NpcEngine(engineScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_NpcEngine()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_NpcEngine
				{
					myID = script_.myID,
					ownerID = script_.ownerID,
					isUnlocked = script_.isUnlocked,
					gekauft = script_.gekauft,
					myName = script_.myName,
					umsatz = script_.umsatz,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_HU = script_.name_HU,
					name_CT = script_.name_CT,
					name_CZ = script_.name_CZ,
					name_PB = script_.name_PB,
					name_IT = script_.name_IT,
					name_JA = script_.name_JA,
					name_UA = script_.name_UA,
					name_PL = script_.name_PL,
					name_TH = script_.name_TH,
					spezialgenre = script_.spezialgenre,
					spezialplatform = script_.spezialplatform,
					sellEngine = script_.sellEngine,
					preis = script_.preis,
					gewinnbeteiligung = script_.gewinnbeteiligung,
					updating = script_.updating,
					devPoints = script_.devPoints,
					devPointsStart = script_.devPointsStart,
					date_year = script_.date_year,
					date_month = script_.date_month,
					archiv_engine = script_.archiv_engine,
					publisherBuyed = (bool[])script_.publisherBuyed.Clone(),
					features = (bool[])script_.features.Clone(),
					featuresInDev = (bool[])script_.featuresInDev.Clone()
				});
			}
		}
	}

	public void SERVER_Get_NpcEngine(s_NpcEngine msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_NpcEngine()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayEnginesScripts[i].gameObject;
				break;
			}
		}
		if ((bool)gameObject)
		{
			engineScript component = gameObject.GetComponent<engineScript>();
			if ((bool)component)
			{
				component.myID = msg.myID;
				component.ownerID = msg.ownerID;
				component.isUnlocked = msg.isUnlocked;
				component.gekauft = msg.gekauft;
				component.myName = msg.myName;
				component.umsatz = msg.umsatz;
				component.name_EN = msg.name_EN;
				component.name_GE = msg.name_GE;
				component.name_TU = msg.name_TU;
				component.name_CH = msg.name_CH;
				component.name_FR = msg.name_FR;
				component.name_HU = msg.name_HU;
				component.name_CT = msg.name_CT;
				component.name_CZ = msg.name_CZ;
				component.name_PB = msg.name_PB;
				component.name_IT = msg.name_IT;
				component.name_JA = msg.name_JA;
				component.name_UA = msg.name_UA;
				component.name_PL = msg.name_PL;
				component.name_TH = msg.name_TH;
				component.spezialgenre = msg.spezialgenre;
				component.spezialplatform = msg.spezialplatform;
				component.sellEngine = msg.sellEngine;
				component.preis = msg.preis;
				component.gewinnbeteiligung = msg.gewinnbeteiligung;
				component.updating = msg.updating;
				component.devPoints = msg.devPoints;
				component.devPointsStart = msg.devPointsStart;
				component.date_year = msg.date_year;
				component.date_month = msg.date_month;
				component.archiv_engine = msg.archiv_engine;
				component.publisherBuyed = (bool[])msg.publisherBuyed.Clone();
				component.features = (bool[])msg.features.Clone();
				component.featuresInDev = (bool[])msg.featuresInDev.Clone();
			}
			return;
		}
		engineScript engineScript2 = eF_.CreateEngine();
		if ((bool)engineScript2)
		{
			engineScript2.myID = msg.myID;
			engineScript2.ownerID = msg.ownerID;
			engineScript2.isUnlocked = msg.isUnlocked;
			engineScript2.gekauft = msg.gekauft;
			engineScript2.myName = msg.myName;
			engineScript2.umsatz = msg.umsatz;
			engineScript2.name_EN = msg.name_EN;
			engineScript2.name_GE = msg.name_GE;
			engineScript2.name_TU = msg.name_TU;
			engineScript2.name_CH = msg.name_CH;
			engineScript2.name_FR = msg.name_FR;
			engineScript2.name_HU = msg.name_HU;
			engineScript2.name_CT = msg.name_CT;
			engineScript2.name_CZ = msg.name_CZ;
			engineScript2.name_PB = msg.name_PB;
			engineScript2.name_IT = msg.name_IT;
			engineScript2.name_JA = msg.name_JA;
			engineScript2.name_UA = msg.name_UA;
			engineScript2.name_PL = msg.name_PL;
			engineScript2.name_TH = msg.name_TH;
			engineScript2.spezialgenre = msg.spezialgenre;
			engineScript2.spezialplatform = msg.spezialplatform;
			engineScript2.sellEngine = msg.sellEngine;
			engineScript2.preis = msg.preis;
			engineScript2.gewinnbeteiligung = msg.gewinnbeteiligung;
			engineScript2.updating = msg.updating;
			engineScript2.devPoints = msg.devPoints;
			engineScript2.devPointsStart = msg.devPointsStart;
			engineScript2.date_year = msg.date_year;
			engineScript2.date_month = msg.date_month;
			engineScript2.archiv_engine = msg.archiv_engine;
			engineScript2.publisherBuyed = (bool[])msg.publisherBuyed.Clone();
			engineScript2.features = (bool[])msg.features.Clone();
			engineScript2.featuresInDev = (bool[])msg.featuresInDev.Clone();
			engineScript2.Init();
		}
	}

	public void SERVER_Send_CopyProtect(copyProtectScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_CopyProtect()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_CopyProtect
				{
					myID = script_.myID,
					date_year = script_.date_year,
					date_month = script_.date_month,
					price = script_.price,
					dev_costs = script_.dev_costs,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_CT = script_.name_CT,
					name_RU = script_.name_RU,
					name_IT = script_.name_IT,
					name_JA = script_.name_JA,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					isUnlocked = script_.isUnlocked,
					effekt = script_.effekt,
					neverLooseEffect = script_.neverLooseEffect
				});
			}
		}
	}

	public void SERVER_Get_CopyProtect(s_CopyProtect msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_CopyProtect()");
		FindScripts();
		GameObject gameObject = GameObject.Find("COPYPROTECT_" + msg.myID);
		if ((bool)gameObject)
		{
			copyProtectScript component = gameObject.GetComponent<copyProtectScript>();
			if ((bool)component)
			{
				component.myID = msg.myID;
				component.date_year = msg.date_year;
				component.date_month = msg.date_month;
				component.price = msg.price;
				component.dev_costs = msg.dev_costs;
				component.name_EN = msg.name_EN;
				component.name_GE = msg.name_GE;
				component.name_TU = msg.name_TU;
				component.name_CH = msg.name_CH;
				component.name_FR = msg.name_FR;
				component.name_CT = msg.name_CT;
				component.name_RU = msg.name_RU;
				component.name_IT = msg.name_IT;
				component.name_JA = msg.name_JA;
				component.name_UA = msg.name_UA;
				component.name_TH = msg.name_TH;
				component.isUnlocked = msg.isUnlocked;
				component.effekt = msg.effekt;
				component.neverLooseEffect = msg.neverLooseEffect;
			}
			return;
		}
		copyProtectScript copyProtectScript2 = copyProtect_.CreateCopyProtect();
		if ((bool)copyProtectScript2)
		{
			copyProtectScript2.myID = msg.myID;
			copyProtectScript2.date_year = msg.date_year;
			copyProtectScript2.date_month = msg.date_month;
			copyProtectScript2.price = msg.price;
			copyProtectScript2.dev_costs = msg.dev_costs;
			copyProtectScript2.name_EN = msg.name_EN;
			copyProtectScript2.name_GE = msg.name_GE;
			copyProtectScript2.name_TU = msg.name_TU;
			copyProtectScript2.name_CH = msg.name_CH;
			copyProtectScript2.name_FR = msg.name_FR;
			copyProtectScript2.name_CT = msg.name_CT;
			copyProtectScript2.name_RU = msg.name_RU;
			copyProtectScript2.name_IT = msg.name_IT;
			copyProtectScript2.name_JA = msg.name_JA;
			copyProtectScript2.name_UA = msg.name_UA;
			copyProtectScript2.name_TH = msg.name_TH;
			copyProtectScript2.isUnlocked = msg.isUnlocked;
			copyProtectScript2.effekt = msg.effekt;
			copyProtectScript2.neverLooseEffect = msg.neverLooseEffect;
			copyProtectScript2.Init();
		}
	}

	public void SERVER_Send_NpcGameName(int slot_, bool ip_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_NpcGameName()");
			NetworkServer.SendToAll(new s_NpcGameName
			{
				slot = slot_,
				ip = ip_
			});
		}
	}

	public void SERVER_Get_NpcGameName(s_NpcGameName msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_NpcGameName()");
		if (!tS_)
		{
			FindScripts();
		}
		if (!msg.ip)
		{
			if (tS_.npcGameNameInUse.Length > msg.slot)
			{
				tS_.npcGameNameInUse[msg.slot] = true;
			}
		}
		else if (tS_.npcIPsInUse.Length > msg.slot)
		{
			tS_.npcIPsInUse[msg.slot] = true;
		}
	}

	public void SERVER_Send_TochterfirmaUmsatz(publisherScript pubS_, gameScript gameS_, long money_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_TochterfirmaUmsatz()");
			if ((bool)pubS_ && (bool)gameS_ && money_ > 0)
			{
				NetworkServer.SendToAll(new s_TochterfirmaUmsatz
				{
					publisherID = pubS_.myID,
					gameID = gameS_.myID,
					money = money_
				});
			}
		}
	}

	public void SERVER_Get_TochterfirmaUmsatz(s_TochterfirmaUmsatz msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_TochterfirmaUmsatz()");
		if (!mS_)
		{
			FindScripts();
		}
		gameScript gameScript2 = null;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == msg.gameID)
			{
				if (!games_.arrayGamesScripts[i].IsMyGame())
				{
					gameScript2 = games_.arrayGamesScripts[i];
				}
				break;
			}
		}
		for (int j = 0; j < mS_.arrayPublisherScripts.Length; j++)
		{
			if (!mS_.arrayPublisherScripts[j] || mS_.arrayPublisherScripts[j].myID != msg.publisherID)
			{
				continue;
			}
			if (mS_.arrayPublisherScripts[j].IsMyTochterfirma())
			{
				mS_.Earn(msg.money, 13);
				mS_.arrayPublisherScripts[j].AddTochterfirmaUmsatz(msg.money);
				if ((bool)gameScript2)
				{
					gameScript2.tw_gewinnanteil += msg.money;
				}
			}
			break;
		}
	}

	public void SERVER_Send_Firmenwert()
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("SERVER_Send_Firmenwert()");
		if (!mS_)
		{
			FindScripts();
		}
		int[] array = new int[mS_.arrayPublisherScripts.Length];
		long[] array2 = new long[mS_.arrayPublisherScripts.Length];
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i])
			{
				array[i] = mS_.arrayPublisherScripts[i].myID;
				array2[i] = mS_.arrayPublisherScripts[i].firmenwert;
			}
		}
		NetworkServer.SendToAll(new s_Firmenwert
		{
			publisherID = (int[])array.Clone(),
			firmenwert = (long[])array2.Clone()
		});
	}

	public void SERVER_Get_Firmenwert(s_Firmenwert msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Firmenwert()");
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if (!mS_.arrayPublisherScripts[i])
			{
				continue;
			}
			for (int j = 0; j < msg.publisherID.Length; j++)
			{
				if (msg.publisherID[j] == mS_.arrayPublisherScripts[i].myID)
				{
					mS_.arrayPublisherScripts[i].firmenwert = msg.firmenwert[j];
				}
			}
		}
	}

	public void SERVER_Send_PublisherTochterfirmaSettings(publisherScript script_)
	{
		if (!isServer)
		{
			return;
		}
		Debug.Log("SERVER_Send_PublisherTochterfirmaSettings()");
		if ((bool)script_)
		{
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_PublisherTochterfirmaSettings
				{
					myID = script_.myID,
					tf_geschlossen = script_.tf_geschlossen,
					tf_autoRelease = script_.tf_autoRelease,
					tf_onlyPlayerConsole = script_.tf_onlyPlayerConsole,
					tf_allowMMO = script_.tf_allowMMO,
					tf_allowF2P = script_.tf_allowF2P,
					tf_allowAddon = script_.tf_allowAddon,
					tf_noArcade = script_.tf_noArcade,
					tf_noHandy = script_.tf_noHandy,
					tf_noRetro = script_.tf_noRetro,
					tf_noPorts = script_.tf_noPorts,
					tf_noBudget = script_.tf_noBudget,
					tf_noGOTY = script_.tf_noGOTY,
					tf_noBundles = script_.tf_noBundles,
					tf_noAddonBundles = script_.tf_noAddonBundles,
					tf_noRemaster = script_.tf_noRemaster,
					tf_noSpinoffs = script_.tf_noSpinoffs,
					tf_autoGamePass = script_.tf_autoGamePass,
					tf_ownPublisher = script_.tf_ownPublisher,
					tf_publisher = script_.tf_publisher,
					tf_developer = script_.tf_developer,
					tf_entwicklungsdauer = script_.tf_entwicklungsdauer,
					tf_gameSize = script_.tf_gameSize,
					tf_gameGenre = script_.tf_gameGenre,
					tf_gameTopic = script_.tf_gameTopic,
					tf_engine = script_.tf_engine,
					tf_autoReleaseVal = script_.tf_autoReleaseVal,
					tf_ownPublisherPriority = script_.tf_ownPublisherPriority,
					tf_ipFocus = (int[])script_.tf_ipFocus.Clone(),
					tf_platformFocus = (int[])script_.tf_platformFocus.Clone()
				});
			}
			else
			{
				Debug.Log("ERROR: SERVER_Send_PublisherTochterfirmaSettings() -> Missing PublisherScript");
			}
		}
	}

	public void SERVER_Get_PublisherTochterfirmaSettings(s_PublisherTochterfirmaSettings msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PublisherTochterfirmaSettings()");
		FindScripts();
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				mS_.arrayPublisherScripts[i].tf_geschlossen = msg.tf_geschlossen;
				mS_.arrayPublisherScripts[i].tf_autoRelease = msg.tf_autoRelease;
				mS_.arrayPublisherScripts[i].tf_onlyPlayerConsole = msg.tf_onlyPlayerConsole;
				mS_.arrayPublisherScripts[i].tf_allowMMO = msg.tf_allowMMO;
				mS_.arrayPublisherScripts[i].tf_allowF2P = msg.tf_allowF2P;
				mS_.arrayPublisherScripts[i].tf_allowAddon = msg.tf_allowAddon;
				mS_.arrayPublisherScripts[i].tf_noArcade = msg.tf_noArcade;
				mS_.arrayPublisherScripts[i].tf_noHandy = msg.tf_noHandy;
				mS_.arrayPublisherScripts[i].tf_noRetro = msg.tf_noRetro;
				mS_.arrayPublisherScripts[i].tf_noPorts = msg.tf_noPorts;
				mS_.arrayPublisherScripts[i].tf_noBudget = msg.tf_noBudget;
				mS_.arrayPublisherScripts[i].tf_noGOTY = msg.tf_noGOTY;
				mS_.arrayPublisherScripts[i].tf_noBundles = msg.tf_noBundles;
				mS_.arrayPublisherScripts[i].tf_noAddonBundles = msg.tf_noAddonBundles;
				mS_.arrayPublisherScripts[i].tf_noRemaster = msg.tf_noRemaster;
				mS_.arrayPublisherScripts[i].tf_noSpinoffs = msg.tf_noSpinoffs;
				mS_.arrayPublisherScripts[i].tf_autoGamePass = msg.tf_autoGamePass;
				mS_.arrayPublisherScripts[i].tf_ownPublisher = msg.tf_ownPublisher;
				mS_.arrayPublisherScripts[i].tf_publisher = msg.tf_publisher;
				mS_.arrayPublisherScripts[i].tf_developer = msg.tf_developer;
				mS_.arrayPublisherScripts[i].tf_entwicklungsdauer = msg.tf_entwicklungsdauer;
				mS_.arrayPublisherScripts[i].tf_gameSize = msg.tf_gameSize;
				mS_.arrayPublisherScripts[i].tf_gameGenre = msg.tf_gameGenre;
				mS_.arrayPublisherScripts[i].tf_gameTopic = msg.tf_gameTopic;
				mS_.arrayPublisherScripts[i].tf_engine = msg.tf_engine;
				mS_.arrayPublisherScripts[i].tf_autoReleaseVal = msg.tf_autoReleaseVal;
				mS_.arrayPublisherScripts[i].tf_ownPublisherPriority = msg.tf_ownPublisherPriority;
				mS_.arrayPublisherScripts[i].tf_ipFocus = (int[])msg.tf_ipFocus.Clone();
				mS_.arrayPublisherScripts[i].tf_platformFocus = (int[])msg.tf_platformFocus.Clone();
				break;
			}
		}
	}

	public void SERVER_Send_PublisherClose(publisherScript script_)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		Debug.Log("SERVER_Send_PublisherClose()");
		if ((bool)script_)
		{
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_PublisherClose
				{
					myID = script_.myID
				});
			}
			else
			{
				Debug.Log("ERROR: SERVER_Send_PublisherClose() -> Missing PublisherScript");
			}
		}
	}

	public void SERVER_Get_PublisherClose(s_PublisherClose msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PublisherClose()");
		FindScripts();
		if (mS_.exklusivVertrag_ID == msg.myID)
		{
			mS_.RemovePublisherExklusivVertrag();
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				mS_.arrayPublisherScripts[i].tf_geschlossen = true;
				mS_.arrayPublisherScripts[i].lockToBuy = 0;
				break;
			}
		}
	}

	public void SERVER_Send_BlockContractGame(gameScript script_, bool block_)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		Debug.Log("SERVER_Send_BlockContractGame()");
		if ((bool)script_)
		{
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_BlockContractGame
				{
					myID = script_.myID,
					block = block_
				});
			}
			else
			{
				Debug.Log("ERROR: SERVER_Send_BlockContractGame() -> Missing GameScript");
			}
		}
	}

	public void SERVER_Get_BlockContractGame(s_BlockContractGame msg)
	{
		if (!isClient || !games_)
		{
			return;
		}
		Debug.Log("SERVER_Get_BlockContractGame()");
		FindScripts();
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == msg.myID)
			{
				games_.arrayGamesScripts[i].auftragsspiel_Blocked = msg.block;
				break;
			}
		}
	}

	public void SERVER_Send_PublisherOwner(publisherScript script_)
	{
		if (!isServer)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		Debug.Log("SERVER_Send_PublisherOwner()");
		if ((bool)script_)
		{
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_PublisherOwner
				{
					myID = script_.myID,
					ownerID = script_.ownerID
				});
			}
			else
			{
				Debug.Log("ERROR: SERVER_Send_PublisherOwner() -> Missing PublisherScript");
			}
		}
	}

	public void SERVER_Get_PublisherOwner(s_PublisherOwner msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_PublisherOwner()");
		FindScripts();
		if (mS_.exklusivVertrag_ID == msg.myID)
		{
			mS_.RemovePublisherExklusivVertrag();
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				mS_.arrayPublisherScripts[i].ownerID = msg.ownerID;
				break;
			}
		}
	}

	public void SERVER_Send_Publisher(publisherScript script_)
	{
		if (!isServer || playersMP.Count <= 1)
		{
			return;
		}
		Debug.Log("SERVER_Send_Publisher() " + manager.numPlayers);
		if ((bool)script_)
		{
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_Publisher
				{
					myID = script_.myID,
					isUnlocked = script_.isUnlocked,
					name_EN = script_.name_EN,
					name_GE = script_.name_GE,
					name_TU = script_.name_TU,
					name_CH = script_.name_CH,
					name_FR = script_.name_FR,
					name_JA = script_.name_JA,
					name_UA = script_.name_UA,
					name_TH = script_.name_TH,
					date_year = script_.date_year,
					date_month = script_.date_month,
					stars = script_.stars,
					logoID = script_.logoID,
					developer = script_.developer,
					publisher = script_.publisher,
					onlyMobile = script_.onlyMobile,
					share = script_.share,
					fanGenre = script_.fanGenre,
					firmenwert = script_.firmenwert,
					notForSale = script_.notForSale,
					lockToBuy = script_.lockToBuy,
					isPlayer = script_.isPlayer,
					ownerID = script_.ownerID,
					country = script_.country,
					ownPlatform = script_.ownPlatform,
					exklusive = script_.exklusive,
					awards = (int[])script_.awards.Clone()
				});
			}
			else
			{
				Debug.Log("ERROR: SERVER_Send_Publisher() -> Missing PublisherScript");
			}
		}
	}

	public void SERVER_Get_Publisher(s_Publisher msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Publisher()");
		FindScripts();
		GameObject gameObject = null;
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == msg.myID)
			{
				gameObject = mS_.arrayPublisherScripts[i].gameObject;
				break;
			}
		}
		if ((bool)gameObject)
		{
			publisherScript component = gameObject.GetComponent<publisherScript>();
			if ((bool)component)
			{
				component.myID = msg.myID;
				component.isUnlocked = msg.isUnlocked;
				component.name_EN = msg.name_EN;
				component.name_GE = msg.name_GE;
				component.name_TU = msg.name_TU;
				component.name_CH = msg.name_CH;
				component.name_FR = msg.name_FR;
				component.name_JA = msg.name_JA;
				component.name_UA = msg.name_UA;
				component.name_TH = msg.name_TH;
				component.date_year = msg.date_year;
				component.date_month = msg.date_month;
				component.stars = msg.stars;
				component.logoID = msg.logoID;
				component.developer = msg.developer;
				component.publisher = msg.publisher;
				component.onlyMobile = msg.onlyMobile;
				component.share = msg.share;
				component.fanGenre = msg.fanGenre;
				component.firmenwert = msg.firmenwert;
				component.notForSale = msg.notForSale;
				component.lockToBuy = msg.lockToBuy;
				component.isPlayer = msg.isPlayer;
				component.country = msg.country;
				component.ownPlatform = msg.ownPlatform;
				component.exklusive = msg.exklusive;
				component.awards = (int[])msg.awards.Clone();
			}
			return;
		}
		publisherScript publisherScript2 = publisher_.CreatePublisher();
		if ((bool)publisherScript2)
		{
			publisherScript2.myID = msg.myID;
			publisherScript2.isUnlocked = msg.isUnlocked;
			publisherScript2.name_EN = msg.name_EN;
			publisherScript2.name_GE = msg.name_GE;
			publisherScript2.name_TU = msg.name_TU;
			publisherScript2.name_CH = msg.name_CH;
			publisherScript2.name_FR = msg.name_FR;
			publisherScript2.name_JA = msg.name_JA;
			publisherScript2.name_UA = msg.name_UA;
			publisherScript2.name_TH = msg.name_TH;
			publisherScript2.date_year = msg.date_year;
			publisherScript2.date_month = msg.date_month;
			publisherScript2.stars = msg.stars;
			publisherScript2.logoID = msg.logoID;
			publisherScript2.developer = msg.developer;
			publisherScript2.publisher = msg.publisher;
			publisherScript2.onlyMobile = msg.onlyMobile;
			publisherScript2.share = msg.share;
			publisherScript2.fanGenre = msg.fanGenre;
			publisherScript2.firmenwert = msg.firmenwert;
			publisherScript2.notForSale = msg.notForSale;
			publisherScript2.lockToBuy = msg.lockToBuy;
			publisherScript2.isPlayer = msg.isPlayer;
			publisherScript2.ownerID = msg.ownerID;
			publisherScript2.country = msg.country;
			publisherScript2.ownPlatform = msg.ownPlatform;
			publisherScript2.exklusive = msg.exklusive;
			publisherScript2.awards = (int[])msg.awards.Clone();
			publisherScript2.Init();
		}
	}

	public void SERVER_Send_Game(gameScript script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Game()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_Game
				{
					gameID = script_.myID,
					myName = script_.GetNameSimple(),
					ipName = script_.ipName,
					inDevelopment = script_.inDevelopment,
					developerID = script_.developerID,
					publisherID = script_.publisherID,
					ownerID = script_.ownerID,
					engineID = script_.engineID,
					hype = script_.hype,
					isOnMarket = script_.isOnMarket,
					warBeiAwards = script_.warBeiAwards,
					weeksOnMarket = script_.weeksOnMarket,
					usk = script_.usk,
					freigabeBudget = script_.freigabeBudget,
					reviewGameplay = script_.reviewGameplay,
					reviewGrafik = script_.reviewGrafik,
					reviewSound = script_.reviewSound,
					reviewSteuerung = script_.reviewSteuerung,
					reviewTotal = script_.reviewTotal,
					reviewGameplayText = script_.reviewGameplayText,
					reviewGrafikText = script_.reviewGrafikText,
					reviewSoundText = script_.reviewSoundText,
					reviewSteuerungText = script_.reviewSteuerungText,
					reviewTotalText = script_.reviewTotalText,
					date_year = script_.date_year,
					date_month = script_.date_month,
					date_start_year = script_.date_start_year,
					date_start_month = script_.date_start_month,
					sellsTotal = script_.sellsTotal,
					umsatzTotal = script_.umsatzTotal,
					costs_entwicklung = script_.costs_entwicklung,
					costs_mitarbeiter = script_.costs_mitarbeiter,
					costs_marketing = script_.costs_marketing,
					costs_enginegebuehren = script_.costs_enginegebuehren,
					costs_server = script_.costs_server,
					costs_production = script_.costs_production,
					costs_updates = script_.costs_updates,
					typ_standard = script_.typ_standard,
					typ_nachfolger = script_.typ_nachfolger,
					teile = script_.teile,
					typ_contractGame = script_.typ_contractGame,
					typ_remaster = script_.typ_remaster,
					typ_spinoff = script_.typ_spinoff,
					typ_addon = script_.typ_addon,
					typ_addonStandalone = script_.typ_addonStandalone,
					typ_mmoaddon = script_.typ_mmoaddon,
					typ_bundle = script_.typ_bundle,
					typ_budget = script_.typ_budget,
					typ_bundleAddon = script_.typ_bundleAddon,
					typ_goty = script_.typ_goty,
					originalGameID = script_.originalGameID,
					portID = script_.portID,
					mainIP = script_.mainIP,
					ipPunkte = script_.ipPunkte,
					exklusiv = script_.exklusiv,
					herstellerExklusiv = script_.herstellerExklusiv,
					retro = script_.retro,
					handy = script_.handy,
					arcade = script_.arcade,
					goty = script_.goty,
					nachfolger_created = script_.nachfolger_created,
					remaster_created = script_.remaster_created,
					budget_created = script_.budget_created,
					goty_created = script_.goty_created,
					trendsetter = script_.trendsetter,
					spielbericht = script_.spielbericht,
					amountUpdates = script_.amountUpdates,
					bonusSellsUpdates = script_.bonusSellsUpdates,
					amountAddons = script_.amountAddons,
					bonusSellsAddons = script_.bonusSellsAddons,
					amountMMOAddons = script_.amountMMOAddons,
					addonQuality = script_.addonQuality,
					devAktFeature = script_.devAktFeature,
					devPoints = script_.devPoints,
					devPointsStart = script_.devPointsStart,
					devPoints_Gesamt = script_.devPoints_Gesamt,
					devPointsStart_Gesamt = script_.devPointsStart_Gesamt,
					points_gameplay = script_.points_gameplay,
					points_grafik = script_.points_grafik,
					points_sound = script_.points_sound,
					points_technik = script_.points_technik,
					points_bugs = script_.points_bugs,
					beschreibung = script_.beschreibung,
					gameTyp = script_.gameTyp,
					gameSize = script_.gameSize,
					gameZielgruppe = script_.gameZielgruppe,
					maingenre = script_.maingenre,
					subgenre = script_.subgenre,
					gameMainTheme = script_.gameMainTheme,
					gameSubTheme = script_.gameSubTheme,
					gameLicence = script_.gameLicence,
					gameCopyProtect = script_.gameCopyProtect,
					gameAntiCheat = script_.gameAntiCheat,
					gameAP_Gameplay = script_.gameAP_Gameplay,
					gameAP_Grafik = script_.gameAP_Grafik,
					gameAP_Sound = script_.gameAP_Sound,
					gameAP_Technik = script_.gameAP_Technik,
					gameLanguage = (bool[])script_.gameLanguage.Clone(),
					gameGameplayFeatures = (bool[])script_.gameGameplayFeatures.Clone(),
					gamePlatform = (int[])script_.gamePlatform.Clone(),
					gameEngineFeature = (int[])script_.gameEngineFeature.Clone(),
					gameplayFeatures_DevDone = (bool[])script_.gameplayFeatures_DevDone.Clone(),
					engineFeature_DevDone = (bool[])script_.engineFeature_DevDone.Clone(),
					gameplayStudio = (bool[])script_.gameplayStudio.Clone(),
					grafikStudio = (bool[])script_.grafikStudio.Clone(),
					soundStudio = (bool[])script_.soundStudio.Clone(),
					motionCaptureStudio = (bool[])script_.motionCaptureStudio.Clone(),
					bundleID = (int[])script_.bundleID.Clone(),
					portExist = (bool[])script_.portExist.Clone(),
					sellsPerWeek = (int[])script_.sellsPerWeek.Clone(),
					verkaufspreis = (int[])script_.verkaufspreis.Clone(),
					releaseDate = script_.releaseDate,
					abonnements = script_.abonnements,
					abonnementsWoche = script_.abonnementsWoche,
					aboPreis = script_.aboPreis,
					pubOffer = script_.pubOffer,
					pubAngebot = script_.pubAngebot,
					pubAngebot_Weeks = script_.pubAngebot_Weeks,
					pubAngebot_Verhandlung = script_.pubAngebot_Verhandlung,
					pubAngebot_Retail = script_.pubAngebot_Retail,
					pubAngebot_Digital = script_.pubAngebot_Digital,
					pubAngebot_Garantiesumme = script_.pubAngebot_Garantiesumme,
					pubAngebot_Gewinnbeteiligung = script_.pubAngebot_Gewinnbeteiligung,
					auftragsspiel = script_.auftragsspiel,
					auftragsspiel_gehalt = script_.auftragsspiel_gehalt,
					auftragsspiel_bonus = script_.auftragsspiel_bonus,
					auftragsspiel_zeitInWochen = script_.auftragsspiel_zeitInWochen,
					auftragsspiel_wochenAlsAngebot = script_.auftragsspiel_wochenAlsAngebot,
					auftragsspiel_zeitAbgelaufen = script_.auftragsspiel_zeitAbgelaufen,
					auftragsspiel_mindestbewertung = script_.auftragsspiel_mindestbewertung,
					f2pConverted = script_.f2pConverted,
					angekuendigt = script_.angekuendigt,
					subvention = script_.subvention,
					sonderIP = script_.sonderIP,
					sonderIPMindestreview = script_.sonderIPMindestreview,
					myNameTeil1 = script_.myNameTeil1,
					engineGewinnbeteiligung = script_.engineGewinnbeteiligung,
					weeksInDevelopment = script_.weeksInDevelopment,
					userPositiv = script_.userPositiv,
					userNegativ = script_.userNegativ,
					bestAbonnements = script_.bestAbonnements,
					bestChartPosition = script_.bestChartPosition,
					exklusivKonsolenSells = script_.exklusivKonsolenSells,
					lastChartPosition = script_.lastChartPosition,
					freeware = script_.freeware,
					sellsTotalStandard = script_.sellsTotalStandard,
					sellsTotalDeluxe = script_.sellsTotalDeluxe,
					sellsTotalCollectors = script_.sellsTotalCollectors,
					sellsTotalOnline = script_.sellsTotalOnline,
					points_bugsInvis = script_.points_bugsInvis,
					umsatzInApp = script_.umsatzInApp,
					umsatzAbos = script_.umsatzAbos,
					f2pInteresse = script_.f2pInteresse,
					mmoInteresse = script_.mmoInteresse,
					vorbestellungen = script_.vorbestellungen,
					realsticPower = script_.realsticPower,
					stornierungen = script_.stornierungen,
					commercialFlop = script_.commercialFlop,
					commercialHit = script_.commercialHit,
					inAppPurchaseWeek = script_.inAppPurchaseWeek,
					arcadeCase = script_.arcadeCase,
					arcadeMonitor = script_.arcadeMonitor,
					arcadeJoystick = script_.arcadeJoystick,
					arcadeSound = script_.arcadeSound,
					arcadeProdCosts = script_.arcadeProdCosts,
					finanzierung_Grundkosten = script_.finanzierung_Grundkosten,
					finanzierung_Technology = script_.finanzierung_Technology,
					finanzierung_Kontent = script_.finanzierung_Kontent,
					retailVersion = script_.retailVersion,
					digitalVersion = script_.digitalVersion,
					newGenreCombination = script_.newGenreCombination,
					newTopicCombination = script_.newTopicCombination,
					ipTime = script_.ipTime,
					npcLateinNumbers = script_.npcLateinNumbers,
					mmoTOf2p_created = script_.mmoTOf2p_created,
					bundle_created = script_.bundle_created,
					abosAddons = script_.abosAddons,
					inAppPurchase = (bool[])script_.inAppPurchase.Clone(),
					Designschwerpunkt = (int[])script_.Designschwerpunkt.Clone(),
					Designausrichtung = (int[])script_.Designausrichtung.Clone()
				});
			}
		}
	}

	public void SERVER_Get_Game(s_Game msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_Game()");
		FindScripts();
		GameObject game = GetGame(msg.gameID);
		gameScript gameScript2 = null;
		if (!game)
		{
			gameScript2 = games_.CreateNewGame(fromSavegame: true, setDate: false);
			gameScript2.myID = msg.gameID;
			gameScript2.SetGameObjectName();
			games_.FindGames();
		}
		else
		{
			gameScript2 = game.GetComponent<gameScript>();
			if (gameScript2.IsMyGame() || gameScript2.IsMyAuftragsspiel())
			{
				return;
			}
		}
		if (!gameScript2)
		{
			return;
		}
		bool isOnMarket = gameScript2.isOnMarket;
		gameScript2.myID = msg.gameID;
		gameScript2.SetMyName(msg.myName);
		gameScript2.ipName = msg.ipName;
		gameScript2.inDevelopment = msg.inDevelopment;
		gameScript2.developerID = msg.developerID;
		gameScript2.publisherID = msg.publisherID;
		gameScript2.ownerID = msg.ownerID;
		gameScript2.engineID = msg.engineID;
		gameScript2.hype = msg.hype;
		gameScript2.isOnMarket = msg.isOnMarket;
		gameScript2.warBeiAwards = msg.warBeiAwards;
		gameScript2.weeksOnMarket = msg.weeksOnMarket;
		gameScript2.usk = msg.usk;
		gameScript2.freigabeBudget = msg.freigabeBudget;
		gameScript2.reviewGameplay = msg.reviewGameplay;
		gameScript2.reviewGrafik = msg.reviewGrafik;
		gameScript2.reviewSound = msg.reviewSound;
		gameScript2.reviewSteuerung = msg.reviewSteuerung;
		gameScript2.reviewTotal = msg.reviewTotal;
		gameScript2.reviewGameplayText = msg.reviewGameplayText;
		gameScript2.reviewGrafikText = msg.reviewGrafikText;
		gameScript2.reviewSoundText = msg.reviewSoundText;
		gameScript2.reviewSteuerungText = msg.reviewSteuerungText;
		gameScript2.reviewTotalText = msg.reviewTotalText;
		gameScript2.date_year = msg.date_year;
		gameScript2.date_month = msg.date_month;
		gameScript2.date_start_year = msg.date_start_year;
		gameScript2.date_start_month = msg.date_start_month;
		gameScript2.sellsTotal = msg.sellsTotal;
		gameScript2.umsatzTotal = msg.umsatzTotal;
		gameScript2.costs_entwicklung = msg.costs_entwicklung;
		gameScript2.costs_mitarbeiter = msg.costs_mitarbeiter;
		gameScript2.costs_marketing = msg.costs_marketing;
		gameScript2.costs_enginegebuehren = msg.costs_enginegebuehren;
		gameScript2.costs_server = msg.costs_server;
		gameScript2.costs_production = msg.costs_production;
		gameScript2.costs_updates = msg.costs_updates;
		gameScript2.typ_standard = msg.typ_standard;
		gameScript2.typ_nachfolger = msg.typ_nachfolger;
		gameScript2.teile = msg.teile;
		gameScript2.typ_contractGame = msg.typ_contractGame;
		gameScript2.typ_remaster = msg.typ_remaster;
		gameScript2.typ_spinoff = msg.typ_spinoff;
		gameScript2.typ_addon = msg.typ_addon;
		gameScript2.typ_addonStandalone = msg.typ_addonStandalone;
		gameScript2.typ_mmoaddon = msg.typ_mmoaddon;
		gameScript2.typ_bundle = msg.typ_bundle;
		gameScript2.typ_budget = msg.typ_budget;
		gameScript2.typ_bundleAddon = msg.typ_bundleAddon;
		gameScript2.typ_goty = msg.typ_goty;
		gameScript2.originalGameID = msg.originalGameID;
		gameScript2.portID = msg.portID;
		gameScript2.mainIP = msg.mainIP;
		gameScript2.ipPunkte = msg.ipPunkte;
		gameScript2.exklusiv = msg.exklusiv;
		gameScript2.herstellerExklusiv = msg.herstellerExklusiv;
		gameScript2.retro = msg.retro;
		gameScript2.handy = msg.handy;
		gameScript2.arcade = msg.arcade;
		gameScript2.goty = msg.goty;
		gameScript2.nachfolger_created = msg.nachfolger_created;
		gameScript2.remaster_created = msg.remaster_created;
		gameScript2.budget_created = msg.budget_created;
		gameScript2.goty_created = msg.goty_created;
		gameScript2.trendsetter = msg.trendsetter;
		gameScript2.spielbericht = msg.spielbericht;
		gameScript2.amountUpdates = msg.amountUpdates;
		gameScript2.bonusSellsUpdates = msg.bonusSellsUpdates;
		gameScript2.amountAddons = msg.amountAddons;
		gameScript2.bonusSellsAddons = msg.bonusSellsAddons;
		gameScript2.amountMMOAddons = msg.amountMMOAddons;
		gameScript2.addonQuality = msg.addonQuality;
		gameScript2.devAktFeature = msg.devAktFeature;
		gameScript2.devPoints = msg.devPoints;
		gameScript2.devPointsStart = msg.devPointsStart;
		gameScript2.devPoints_Gesamt = msg.devPoints_Gesamt;
		gameScript2.devPointsStart_Gesamt = msg.devPointsStart_Gesamt;
		gameScript2.points_gameplay = msg.points_gameplay;
		gameScript2.points_grafik = msg.points_grafik;
		gameScript2.points_sound = msg.points_sound;
		gameScript2.points_technik = msg.points_technik;
		gameScript2.points_bugs = msg.points_bugs;
		gameScript2.beschreibung = msg.beschreibung;
		gameScript2.gameTyp = msg.gameTyp;
		gameScript2.gameSize = msg.gameSize;
		gameScript2.gameZielgruppe = msg.gameZielgruppe;
		gameScript2.maingenre = msg.maingenre;
		gameScript2.subgenre = msg.subgenre;
		gameScript2.gameMainTheme = msg.gameMainTheme;
		gameScript2.gameSubTheme = msg.gameSubTheme;
		gameScript2.gameLicence = msg.gameLicence;
		gameScript2.gameCopyProtect = msg.gameCopyProtect;
		gameScript2.gameAntiCheat = msg.gameAntiCheat;
		gameScript2.gameAP_Gameplay = msg.gameAP_Gameplay;
		gameScript2.gameAP_Grafik = msg.gameAP_Grafik;
		gameScript2.gameAP_Sound = msg.gameAP_Sound;
		gameScript2.gameAP_Technik = msg.gameAP_Technik;
		gameScript2.gameLanguage = (bool[])msg.gameLanguage.Clone();
		gameScript2.gameGameplayFeatures = (bool[])msg.gameGameplayFeatures.Clone();
		gameScript2.gamePlatform = (int[])msg.gamePlatform.Clone();
		gameScript2.gameEngineFeature = (int[])msg.gameEngineFeature.Clone();
		gameScript2.gameplayFeatures_DevDone = (bool[])msg.gameplayFeatures_DevDone.Clone();
		gameScript2.engineFeature_DevDone = (bool[])msg.engineFeature_DevDone.Clone();
		gameScript2.gameplayStudio = (bool[])msg.gameplayStudio.Clone();
		gameScript2.grafikStudio = (bool[])msg.grafikStudio.Clone();
		gameScript2.soundStudio = (bool[])msg.soundStudio.Clone();
		gameScript2.motionCaptureStudio = (bool[])msg.motionCaptureStudio.Clone();
		gameScript2.bundleID = (int[])msg.bundleID.Clone();
		gameScript2.portExist = (bool[])msg.portExist.Clone();
		gameScript2.sellsPerWeek = (int[])msg.sellsPerWeek.Clone();
		gameScript2.verkaufspreis = (int[])msg.verkaufspreis.Clone();
		gameScript2.releaseDate = msg.releaseDate;
		gameScript2.abonnements = msg.abonnements;
		gameScript2.abonnementsWoche = msg.abonnementsWoche;
		gameScript2.aboPreis = msg.aboPreis;
		gameScript2.pubOffer = msg.pubOffer;
		gameScript2.pubAngebot = msg.pubAngebot;
		gameScript2.pubAngebot_Weeks = msg.pubAngebot_Weeks;
		gameScript2.pubAngebot_Verhandlung = msg.pubAngebot_Verhandlung;
		gameScript2.pubAngebot_Retail = msg.pubAngebot_Retail;
		gameScript2.pubAngebot_Digital = msg.pubAngebot_Digital;
		gameScript2.pubAngebot_Garantiesumme = msg.pubAngebot_Garantiesumme;
		gameScript2.pubAngebot_Gewinnbeteiligung = msg.pubAngebot_Gewinnbeteiligung;
		gameScript2.auftragsspiel = msg.auftragsspiel;
		gameScript2.auftragsspiel_gehalt = msg.auftragsspiel_gehalt;
		gameScript2.auftragsspiel_bonus = msg.auftragsspiel_bonus;
		gameScript2.auftragsspiel_zeitInWochen = msg.auftragsspiel_zeitInWochen;
		gameScript2.auftragsspiel_wochenAlsAngebot = msg.auftragsspiel_wochenAlsAngebot;
		gameScript2.auftragsspiel_zeitAbgelaufen = msg.auftragsspiel_zeitAbgelaufen;
		gameScript2.auftragsspiel_mindestbewertung = msg.auftragsspiel_mindestbewertung;
		gameScript2.f2pConverted = msg.f2pConverted;
		gameScript2.angekuendigt = msg.angekuendigt;
		gameScript2.subvention = msg.subvention;
		gameScript2.sonderIP = msg.sonderIP;
		gameScript2.sonderIPMindestreview = msg.sonderIPMindestreview;
		gameScript2.myNameTeil1 = msg.myNameTeil1;
		gameScript2.engineGewinnbeteiligung = msg.engineGewinnbeteiligung;
		gameScript2.weeksInDevelopment = msg.weeksInDevelopment;
		gameScript2.userPositiv = msg.userPositiv;
		gameScript2.userNegativ = msg.userNegativ;
		gameScript2.bestAbonnements = msg.bestAbonnements;
		gameScript2.bestChartPosition = msg.bestChartPosition;
		gameScript2.exklusivKonsolenSells = msg.exklusivKonsolenSells;
		gameScript2.lastChartPosition = msg.lastChartPosition;
		gameScript2.freeware = msg.freeware;
		gameScript2.sellsTotalStandard = msg.sellsTotalStandard;
		gameScript2.sellsTotalDeluxe = msg.sellsTotalDeluxe;
		gameScript2.sellsTotalCollectors = msg.sellsTotalCollectors;
		gameScript2.sellsTotalOnline = msg.sellsTotalOnline;
		gameScript2.points_bugsInvis = msg.points_bugsInvis;
		gameScript2.umsatzInApp = msg.umsatzInApp;
		gameScript2.umsatzAbos = msg.umsatzAbos;
		gameScript2.f2pInteresse = msg.f2pInteresse;
		gameScript2.mmoInteresse = msg.mmoInteresse;
		gameScript2.vorbestellungen = msg.vorbestellungen;
		gameScript2.realsticPower = msg.realsticPower;
		gameScript2.stornierungen = msg.stornierungen;
		gameScript2.commercialFlop = msg.commercialFlop;
		gameScript2.commercialHit = msg.commercialHit;
		gameScript2.inAppPurchaseWeek = msg.inAppPurchaseWeek;
		gameScript2.arcadeCase = msg.arcadeCase;
		gameScript2.arcadeMonitor = msg.arcadeMonitor;
		gameScript2.arcadeJoystick = msg.arcadeJoystick;
		gameScript2.arcadeSound = msg.arcadeSound;
		gameScript2.arcadeProdCosts = msg.arcadeProdCosts;
		gameScript2.finanzierung_Grundkosten = msg.finanzierung_Grundkosten;
		gameScript2.finanzierung_Technology = msg.finanzierung_Technology;
		gameScript2.finanzierung_Kontent = msg.finanzierung_Kontent;
		gameScript2.retailVersion = msg.retailVersion;
		gameScript2.digitalVersion = msg.digitalVersion;
		gameScript2.newGenreCombination = msg.newGenreCombination;
		gameScript2.newTopicCombination = msg.newTopicCombination;
		gameScript2.ipTime = msg.ipTime;
		gameScript2.npcLateinNumbers = msg.npcLateinNumbers;
		gameScript2.mmoTOf2p_created = msg.mmoTOf2p_created;
		gameScript2.bundle_created = msg.bundle_created;
		gameScript2.abosAddons = msg.abosAddons;
		gameScript2.inAppPurchase = (bool[])msg.inAppPurchase.Clone();
		gameScript2.Designschwerpunkt = (int[])msg.Designschwerpunkt.Clone();
		gameScript2.Designausrichtung = (int[])msg.Designausrichtung.Clone();
		gameScript2.InitUI();
		if (!isOnMarket && gameScript2.isOnMarket)
		{
			gameScript2.DONT_SEND_GAME = true;
			if (gameScript2.isOnMarket)
			{
				gameScript2.SetOnMarket();
			}
			gameScript2.DONT_SEND_GAME = false;
			if (mS_.newsSetting[0] && gameScript2.isOnMarket)
			{
				if (!gameScript2.GameFromMitspieler())
				{
					if (!gameScript2.IsMyGame() && !gameScript2.GetPublisherOrDeveloperIsTochterfirma())
					{
						string text = tS_.GetText(494);
						text = text.Replace("<NAME1>", gameScript2.GetPublisherName());
						text = text.Replace("<NAME2>", gameScript2.GetNameWithTag());
						guiMain_.CreateTopNewsInfo(text);
					}
				}
				else
				{
					string text2 = tS_.GetText(494);
					text2 = text2.Replace("<NAME1>", gameScript2.GetDeveloperName());
					text2 = text2.Replace("<NAME2>", gameScript2.GetNameWithTag());
					guiMain_.CreateTopNewsInfo(text2);
					text2 = tS_.GetText(1269);
					text2 = text2.Replace("<NAME>", msg.myName);
					guiMain_.AddChat(gameScript2.GetIdFromMitspieler(), text2);
				}
			}
		}
		if (gameScript2.isOnMarket)
		{
			games_.UpdateChartsWeek();
			guiMain_.UpdateCharts();
		}
	}

	public void SERVER_Send_Lizenz(int i)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Lizenz()");
			FindScripts();
			if (i >= 0 && licences_.licence_ANGEBOT.Length >= i)
			{
				NetworkServer.SendToAll(new s_Lizenz
				{
					lizenzID = i,
					name = licences_.GetName(i),
					typ = licences_.licence_TYP[i],
					angebot = licences_.licence_ANGEBOT[i],
					quality = licences_.licence_QUALITY[i],
					licence_GENREGOOD = licences_.licence_GENREGOOD[i],
					licence_GENREBAD = licences_.licence_GENREBAD[i],
					licence_YEAR = licences_.licence_YEAR[i]
				});
			}
		}
	}

	public void SERVER_Get_Lizenz(s_Lizenz msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Lizenz()");
			FindScripts();
			if (msg.lizenzID >= 0 && licences_.licence_ANGEBOT.Length >= msg.lizenzID && licences_.licence_ANGEBOT.Length >= msg.lizenzID)
			{
				licences_.licence_EN[msg.lizenzID] = msg.name;
				licences_.licence_TYP[msg.lizenzID] = msg.typ;
				licences_.licence_ANGEBOT[msg.lizenzID] = msg.angebot;
				licences_.licence_QUALITY[msg.lizenzID] = msg.quality;
				licences_.licence_GENREGOOD[msg.lizenzID] = msg.licence_GENREGOOD;
				licences_.licence_GENREBAD[msg.lizenzID] = msg.licence_GENREBAD;
				licences_.licence_YEAR[msg.lizenzID] = msg.licence_YEAR;
			}
		}
	}

	public void SERVER_Send_Wettbewerb()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Wettbewerb()");
			FindScripts();
			NetworkServer.SendToAll(new s_Wettbewerb
			{
				competition = mS_.settings_competition,
				settings_randomPlattformNum = mS_.settings_randomPlattformNum,
				settings_RandomReviewsNum = mS_.settings_RandomReviewsNum
			});
		}
	}

	public void SERVER_Get_Wettbewerb(s_Wettbewerb msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Wettbewerb()");
			FindScripts();
			mS_.settings_competition = msg.competition;
			mS_.settings_randomPlattformNum = msg.settings_randomPlattformNum;
			mS_.settings_RandomReviewsNum = msg.settings_RandomReviewsNum;
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[61].GetComponent<Dropdown>().value = mS_.settings_competition;
				guiMain_.uiObjects[201].GetComponent<mpMain>().UpdateRandomTexts();
			}
		}
	}

	public void SERVER_Send_RandomSettings()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_RandomSettings()");
			FindScripts();
			NetworkServer.SendToAll(new s_RandomSettings
			{
				randomSettings = mS_.settings_randomEvents
			});
		}
	}

	public void SERVER_Get_RandomSettings(s_RandomSettings msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_RandomSettings()");
			FindScripts();
			mS_.settings_randomEvents = msg.randomSettings;
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[58].GetComponent<Dropdown>().value = mS_.settings_randomEvents;
			}
		}
	}

	public void SERVER_Send_Difficulty()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Difficulty()");
			FindScripts();
			NetworkServer.SendToAll(new s_Difficulty
			{
				difficulty = mS_.difficulty
			});
		}
	}

	public void SERVER_Get_Difficulty(s_Difficulty msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Difficulty()");
			FindScripts();
			mS_.difficulty = msg.difficulty;
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[31].GetComponent<Dropdown>().value = mS_.difficulty;
			}
		}
	}

	public void SERVER_Send_Office()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Office()");
			FindScripts();
			NetworkServer.SendToAll(new s_Office
			{
				office = mS_.office
			});
		}
	}

	public void SERVER_Get_Office(s_Office msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Office()");
			FindScripts();
			mS_.office = msg.office;
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[33].GetComponent<Dropdown>().value = mS_.GetDropdownSlotFromMapID(mS_.office);
			}
		}
	}

	public void SERVER_Send_Startjahr()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Startjahr()");
			FindScripts();
			NetworkServer.SendToAll(new s_Startjahr
			{
				startjahr = mpMain_.uiObjects[32].GetComponent<Dropdown>().value
			});
		}
	}

	public void SERVER_Get_Startjahr(s_Startjahr msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Startjahr()");
			FindScripts();
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[32].GetComponent<Dropdown>().value = msg.startjahr;
			}
		}
	}

	public void SERVER_Send_AnzahlKonkurrenten()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_AnzahlKonkurrenten()");
			FindScripts();
			NetworkServer.SendToAll(new s_AnzahlKonkurrenten
			{
				anzahl = mpMain_.uiObjects[56].GetComponent<Dropdown>().value
			});
		}
	}

	public void SERVER_Get_AnzahlKonkurrenten(s_AnzahlKonkurrenten msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_AnzahlKonkurrenten()");
			FindScripts();
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[56].GetComponent<Dropdown>().value = msg.anzahl;
			}
		}
	}

	public void SERVER_Send_Entwicklungsdauer()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Entwicklungsdauer()");
			FindScripts();
			NetworkServer.SendToAll(new s_Entwicklungsdauer
			{
				dauer = mpMain_.uiObjects[55].GetComponent<Dropdown>().value
			});
		}
	}

	public void SERVER_Get_Entwicklungsdauer(s_Entwicklungsdauer msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Entwicklungsdauer()");
			FindScripts();
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[55].GetComponent<Dropdown>().value = msg.dauer;
			}
		}
	}

	public void SERVER_Send_Spielgeschwindigkeit()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Spielgeschwindigkeit()");
			FindScripts();
			NetworkServer.SendToAll(new s_Spielgeschwindigkeit
			{
				gamespeed = mpMain_.uiObjects[44].GetComponent<Dropdown>().value
			});
		}
	}

	public void SERVER_Get_Spielgeschwindigkeit(s_Spielgeschwindigkeit msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Send_Spielgeschwindigkeit()");
			FindScripts();
			if (guiMain_.uiObjects[201].activeSelf)
			{
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[44].GetComponent<Dropdown>().value = msg.gamespeed;
			}
		}
	}

	public void SERVER_Send_Trend()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Trend()");
			FindScripts();
			NetworkServer.SendToAll(new s_Trend
			{
				trendWeeks = mS_.trendWeeks,
				trendTheme = mS_.trendTheme,
				trendAntiTheme = mS_.trendAntiTheme,
				trendGenre = mS_.trendGenre,
				trendAntiGenre = mS_.trendAntiGenre,
				trendNextGenre = mS_.trendNextGenre,
				trendNextAntiGenre = mS_.trendNextAntiGenre,
				trendNextTheme = mS_.trendNextTheme,
				trendNextAntiTheme = mS_.trendNextAntiTheme
			});
		}
	}

	public void SERVER_Get_Trend(s_Trend msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Trend()");
			FindScripts();
			mS_.trendWeeks = msg.trendWeeks;
			mS_.trendTheme = msg.trendTheme;
			mS_.trendAntiTheme = msg.trendAntiTheme;
			mS_.trendGenre = msg.trendGenre;
			mS_.trendAntiGenre = msg.trendAntiGenre;
			mS_.trendNextGenre = msg.trendNextGenre;
			mS_.trendNextAntiGenre = msg.trendNextAntiGenre;
			mS_.trendNextTheme = msg.trendNextTheme;
			mS_.trendNextAntiTheme = msg.trendNextAntiTheme;
			mS_.ShowTrendNews();
		}
	}

	public void SERVER_Send_KillAA(int charID_, int wert2_, bool eingestellt_, int wert3_)
	{
		if (isServer)
		{
			FindScripts();
			Debug.Log("SERVER_Send_KillAA()");
			NetworkServer.SendToAll(new s_KillAA
			{
				charID = charID_,
				wert2 = wert2_,
				eingestellt = eingestellt_,
				wert3 = wert3_
			});
		}
	}

	public void SERVER_Get_KillAA(s_KillAA msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_KillAA()");
		GameObject gameObject = GameObject.Find("AA_" + msg.charID);
		if ((bool)gameObject)
		{
			charArbeitsmarkt component = gameObject.GetComponent<charArbeitsmarkt>();
			if ((bool)component)
			{
				disableSend = true;
				component.RemoveFromArbeitsmarkt(msg.eingestellt);
				disableSend = false;
			}
		}
	}

	public void SERVER_Send_CreateArbeitsmarkt(charArbeitsmarkt script_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_CreateArbeitsmarkt()");
			if ((bool)script_)
			{
				NetworkServer.SendToAll(new s_CreateArbeitsmarkt
				{
					objectID = script_.myID,
					male = script_.male,
					myName = script_.myName,
					wochenAmArbeitsmarkt = script_.wochenAmArbeitsmarkt,
					legend = script_.legend,
					beruf = script_.beruf,
					s_gamedesign = script_.s_gamedesign,
					s_programmieren = script_.s_programmieren,
					s_grafik = script_.s_grafik,
					s_sound = script_.s_sound,
					s_pr = script_.s_pr,
					s_gametests = script_.s_gametests,
					s_technik = script_.s_technik,
					s_forschen = script_.s_forschen,
					perks = (bool[])script_.perks.Clone(),
					model_body = script_.model_body,
					model_eyes = script_.model_eyes,
					model_hair = script_.model_hair,
					model_beard = script_.model_beard,
					model_skinColor = script_.model_skinColor,
					model_hairColor = script_.model_hairColor,
					model_beardColor = script_.model_beardColor,
					model_HoseColor = script_.model_HoseColor,
					model_ShirtColor = script_.model_ShirtColor,
					model_Add1Color = script_.model_Add1Color
				});
			}
		}
	}

	public void SERVER_Get_CreateArbeitsmarkt(s_CreateArbeitsmarkt msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_CreateArbeitsmarkt()");
		FindScripts();
		charArbeitsmarkt charArbeitsmarkt2 = arbeitsmarkt_.CreateArbeitsmarktItem();
		if ((bool)charArbeitsmarkt2)
		{
			charArbeitsmarkt2.myID = msg.objectID;
			charArbeitsmarkt2.male = msg.male;
			charArbeitsmarkt2.myName = msg.myName;
			charArbeitsmarkt2.wochenAmArbeitsmarkt = msg.wochenAmArbeitsmarkt;
			charArbeitsmarkt2.legend = msg.legend;
			charArbeitsmarkt2.beruf = msg.beruf;
			charArbeitsmarkt2.s_gamedesign = msg.s_gamedesign;
			charArbeitsmarkt2.s_programmieren = msg.s_programmieren;
			charArbeitsmarkt2.s_grafik = msg.s_grafik;
			charArbeitsmarkt2.s_sound = msg.s_sound;
			charArbeitsmarkt2.s_pr = msg.s_pr;
			charArbeitsmarkt2.s_gametests = msg.s_gametests;
			charArbeitsmarkt2.s_technik = msg.s_technik;
			charArbeitsmarkt2.s_forschen = msg.s_forschen;
			charArbeitsmarkt2.perks = (bool[])msg.perks.Clone();
			charArbeitsmarkt2.model_body = msg.model_body;
			charArbeitsmarkt2.model_eyes = msg.model_eyes;
			charArbeitsmarkt2.model_hair = msg.model_hair;
			charArbeitsmarkt2.model_beard = msg.model_beard;
			charArbeitsmarkt2.model_skinColor = msg.model_skinColor;
			charArbeitsmarkt2.model_hairColor = msg.model_hairColor;
			charArbeitsmarkt2.model_beardColor = msg.model_beardColor;
			charArbeitsmarkt2.model_HoseColor = msg.model_HoseColor;
			charArbeitsmarkt2.model_ShirtColor = msg.model_ShirtColor;
			charArbeitsmarkt2.model_Add1Color = msg.model_Add1Color;
			charArbeitsmarkt2.gameObject.name = "AA_" + charArbeitsmarkt2.myID;
			if (charArbeitsmarkt2.perks[1] && (bool)guiMain_ && (bool)tS_)
			{
				tS_.GetText(427);
				guiMain_.CreateTopNewsDevLegend(charArbeitsmarkt2.myName, charArbeitsmarkt2.beruf);
			}
		}
	}

	public void SERVER_Send_GameSpeed(int speed)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_GameSpeed()");
			NetworkServer.SendToAll(new s_GameSpeed
			{
				speed = speed
			});
		}
	}

	public void SERVER_Get_GameSpeed(s_GameSpeed msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_GameSpeed()");
			FindScripts();
			mS_.SetGameSpeed(msg.speed);
		}
	}

	public void SERVER_Send_Command(int command)
	{
		if (isServer && playersMP.Count > 1)
		{
			Debug.Log("SERVER_Send_Command() " + command);
			NetworkServer.SendToAll(new s_Command
			{
				command = command
			});
		}
	}

	public void SERVER_Get_Command(s_Command msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Command() " + msg.command);
			FindScripts();
			switch (msg.command)
			{
			case 1:
				guiMain_.uiObjects[162].SetActive(value: true);
				break;
			case 2:
				guiMain_.uiObjects[202].SetActive(value: false);
				guiMain_.uiObjects[238].SetActive(value: false);
				mS_.save_.loadingSavegame = false;
				break;
			case 3:
				mS_.WochenUpdates();
				break;
			case 4:
				mS_.MonatlicheUpdates();
				break;
			case 5:
				guiMain_.uiObjects[238].SetActive(value: true);
				break;
			case 6:
				mS_.settings_autoPauseForMultiplayer = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[36].GetComponent<Toggle>().isOn = false;
				break;
			case 7:
				mS_.settings_autoPauseForMultiplayer = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[36].GetComponent<Toggle>().isOn = true;
				break;
			case 10:
				mS_.settings_RandomReviews = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[41].GetComponent<Toggle>().isOn = false;
				break;
			case 11:
				mS_.settings_RandomReviews = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[41].GetComponent<Toggle>().isOn = true;
				break;
			case 12:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[42].GetComponent<Toggle>().isOn = false;
				break;
			case 13:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[42].GetComponent<Toggle>().isOn = true;
				break;
			case 14:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[43].GetComponent<Toggle>().isOn = false;
				break;
			case 15:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[43].GetComponent<Toggle>().isOn = true;
				break;
			case 16:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[45].GetComponent<Toggle>().isOn = false;
				break;
			case 17:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[45].GetComponent<Toggle>().isOn = true;
				break;
			case 18:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[52].GetComponent<Toggle>().isOn = false;
				break;
			case 19:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[52].GetComponent<Toggle>().isOn = true;
				break;
			case 22:
				mS_.settings_allGamespeed = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[54].GetComponent<Toggle>().isOn = false;
				break;
			case 23:
				mS_.settings_allGamespeed = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[54].GetComponent<Toggle>().isOn = true;
				break;
			case 24:
				mS_.settings_plattformEnd = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[57].GetComponent<Toggle>().isOn = false;
				break;
			case 25:
				mS_.settings_plattformEnd = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[57].GetComponent<Toggle>().isOn = true;
				break;
			case 26:
				mS_.settings_sabotageOff = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[59].GetComponent<Toggle>().isOn = false;
				break;
			case 27:
				mS_.settings_sabotageOff = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[59].GetComponent<Toggle>().isOn = true;
				break;
			case 28:
				mS_.settings_tochterfirmaOff = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[60].GetComponent<Toggle>().isOn = false;
				break;
			case 29:
				mS_.settings_tochterfirmaOff = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[60].GetComponent<Toggle>().isOn = true;
				break;
			case 30:
				mS_.settings_closeNPCs = false;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[53].GetComponent<Toggle>().isOn = false;
				break;
			case 31:
				mS_.settings_closeNPCs = true;
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[53].GetComponent<Toggle>().isOn = true;
				break;
			case 32:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[62].GetComponent<Toggle>().isOn = false;
				break;
			case 33:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[62].GetComponent<Toggle>().isOn = true;
				break;
			case 34:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[63].GetComponent<Toggle>().isOn = false;
				break;
			case 35:
				guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[63].GetComponent<Toggle>().isOn = true;
				break;
			case 8:
			case 9:
			case 20:
			case 21:
				break;
			}
		}
	}

	public void SERVER_Send_Load(int saveID)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Load() " + saveID);
			NetworkServer.SendToAll(new s_Load
			{
				saveID = saveID
			});
		}
	}

	public void SERVER_Get_Load(s_Load msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Load()");
			FindScripts();
			guiMain_.ActivateMenu(guiMain_.uiObjects[150]);
			guiMain_.ActivateMenu(guiMain_.uiObjects[238]);
			guiMain_.uiObjects[150].GetComponent<Menu_LoadGame>().BUTTON_LoadGame(msg.saveID);
		}
	}

	public void SERVER_Send_Save(int saveID)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_Save() " + saveID);
			NetworkServer.SendToAll(new s_Save
			{
				saveID = saveID
			});
		}
	}

	public void SERVER_Get_Save(s_Save msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Save()");
			FindScripts();
			save_.Save(msg.saveID);
		}
	}

	public void SERVER_Send_ID(int id_, mpPlayer mpPlayer_)
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_ID");
			FindScripts();
			player_mp player_mp2 = FindPlayer(id_);
			if (player_mp2 != null)
			{
				NetworkServer.SendToAll(new s_PlayerID
				{
					id = player_mp2.playerID,
					version = mS_.buildVersion
				});
			}
		}
	}

	public void SERVER_Get_ID(s_PlayerID msg)
	{
		if (!isClient)
		{
			return;
		}
		Debug.Log("SERVER_Get_ID()");
		if (INIT_ID)
		{
			Debug.Log("SERVER_Get_ID() -> BREAK");
			return;
		}
		INIT_ID = true;
		FindScripts();
		if (mS_.buildVersion.Contains(msg.version))
		{
			mS_.myID = msg.id;
			CLIENT_Send_PlayerInfos();
		}
		else
		{
			guiMain_.MessageBox(tS_.GetText(1041), closeMenu: false);
			mpMain_.BUTTON_Close();
		}
	}

	public void SERVER_Send_AddPlayer()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_AddPlayer");
			for (int i = 0; i < playersMP.Count; i++)
			{
				NetworkServer.SendToAll(new s_AddPlayer
				{
					playerID = playersMP[i].playerID
				});
			}
		}
	}

	public void SERVER_Get_AddPlayer(s_AddPlayer msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_AddPlayer");
			FindScripts();
			if (FindPlayer(msg.playerID) == null)
			{
				playersMP.Add(new player_mp(msg.playerID));
				publisherScript myPubScript_ = mS_.CreatePlayerPublisher(msg.playerID);
				FindPlayer(msg.playerID).myPubScript_ = myPubScript_;
			}
		}
	}

	public void SERVER_Send_PlayerInfos()
	{
		if (isServer)
		{
			Debug.Log("SERVER_Send_PlayerInfos");
			for (int i = 0; i < playersMP.Count; i++)
			{
				NetworkServer.SendToAll(new s_PlayerInfos
				{
					id = playersMP[i].playerID,
					playerName = playersMP[i].playerName,
					ready = playersMP[i].ready
				});
			}
		}
	}

	public void SERVER_Get_PlayerInfos(s_PlayerInfos msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_PlayerInfos");
			player_mp player_mp2 = FindPlayer(msg.id);
			if (player_mp2 != null)
			{
				player_mp2.playerName = msg.playerName;
				player_mp2.ready = msg.ready;
			}
		}
	}

	public void SERVER_Send_Money()
	{
		if (isServer && playersMP.Count > 1)
		{
			for (int i = 0; i < playersMP.Count; i++)
			{
				NetworkServer.SendToAll(new s_Money
				{
					playerID = playersMP[i].playerID,
					money = playersMP[i].money,
					fans = playersMP[i].fans
				});
			}
		}
	}

	public void SERVER_Get_Money(s_Money msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Money");
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.money = msg.money;
				player_mp2.fans = msg.fans;
			}
		}
	}

	public void SERVER_Send_AutoPause()
	{
		if (isServer && playersMP.Count > 1)
		{
			for (int i = 0; i < playersMP.Count; i++)
			{
				NetworkServer.SendToAll(new s_AutoPause
				{
					playerID = playersMP[i].playerID,
					pause = playersMP[i].playerPause
				});
			}
		}
	}

	public void SERVER_Get_AutoPause(s_AutoPause msg)
	{
		if (isClient)
		{
			Debug.Log("SERVER_Get_Autopause");
			player_mp player_mp2 = FindPlayer(msg.playerID);
			if (player_mp2 != null)
			{
				player_mp2.playerPause = msg.pause;
			}
		}
	}

	private GameObject GetGame(int id_)
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == id_)
			{
				return games_.arrayGamesScripts[i].gameObject;
			}
		}
		return null;
	}
}
