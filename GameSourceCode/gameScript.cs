using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class gameScript : MonoBehaviour
{
	public class PublisherList
	{
		public long wert;

		public publisherScript script_;

		public PublisherList(long wert_, publisherScript s_)
		{
			wert = wert_;
			script_ = s_;
		}
	}

	public GameObject main_;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public genres genres_;

	public engineFeatures eF_;

	public gameplayFeatures gF_;

	public games games_;

	public themes themes_;

	public unlockScript unlock_;

	public licences licences_;

	public gamepassScript gpS_;

	public gameTab gameTab_;

	public bool debug;

	public int myID;

	public int developerID = -1;

	public int publisherID = -1;

	public int ownerID = -1;

	public int mainIP = -1;

	public publisherScript devS_;

	public publisherScript pS_;

	public publisherScript ownerS_;

	public string myName = "";

	public string myNameTeil1 = "";

	public string ipName = "";

	public bool inDevelopment = true;

	public int engineID;

	public engineScript engineS_;

	public int engineGewinnbeteiligung;

	public float hype;

	public bool isOnMarket;

	public bool warBeiAwards;

	public int weeksOnMarket;

	public int weeksInDevelopment;

	public int usk;

	public int freigabeBudget;

	public int reviewGameplay;

	public int reviewGrafik;

	public int reviewSound;

	public int reviewSteuerung;

	public int reviewTotal;

	public int reviewGameplayText = -1;

	public int reviewGrafikText = -1;

	public int reviewSoundText = -1;

	public int reviewSteuerungText = -1;

	public int reviewTotalText = -1;

	public int date_year;

	public int date_month;

	public int date_start_year;

	public int date_start_month;

	public int userPositiv;

	public int userNegativ;

	public long sellsTotalStandard;

	public long sellsTotalDeluxe;

	public long sellsTotalCollectors;

	public long sellsTotalOnline;

	public long sellsTotal;

	public long sellsStandard_forProduction;

	public long umsatzTotal;

	public long umsatzAbos;

	public long umsatzInApp;

	public long exklusivKonsolenSells;

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

	public gameScript script_vorgaenger;

	public gameScript script_nachfolger;

	public gameScript script_mainIP;

	public gameScript script_portOriginal;

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

	public int originalGameID = -1;

	public int portID = -1;

	public float ipPunkte;

	public int ipTime;

	public int bestChartPosition;

	public int lastChartPosition;

	public bool pubOffer;

	public bool sonderIP;

	public int sonderIPMindestreview;

	public int[] specialMarketing;

	public bool exklusiv;

	public bool herstellerExklusiv;

	public bool retro;

	public bool handy;

	public bool arcade;

	public bool goty;

	public bool[] portExist;

	public bool nachfolger_created;

	public bool remaster_created;

	public bool budget_created;

	public bool goty_created;

	public bool mmoTOf2p_created;

	public bool f2pConverted;

	public bool trendsetter;

	public bool spielbericht;

	public bool spielbericht_favorit;

	public bool angekuendigt;

	public bool inGamePass;

	public int gamePassPlayer;

	public bool freeware;

	public bool platformStatic;

	public bool noSpinOff;

	public bool archiv_spielkonzept;

	public bool archiv_spielbericht;

	public bool archiv_fanbriefe;

	public bool archiv_ip;

	public bool bundle_created;

	public int[] bundleID;

	public gameScript[] bundleGameScript;

	public int amountUpdates;

	public float bonusSellsUpdates;

	public int amountAddons;

	public float bonusSellsAddons;

	public int amountMMOAddons;

	public float addonQuality;

	public float f2pInteresse;

	public float mmoInteresse;

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

	public float points_bugsInvis;

	public string beschreibung = "";

	public int gameTyp;

	public int gameSize;

	public int gameZielgruppe;

	public int maingenre;

	public int subgenre;

	public int gameMainTheme;

	public int gameSubTheme;

	public int gameLicence = -1;

	public int gameCopyProtect = -1;

	public copyProtectScript gameCopyProtectScript_;

	public int gameAntiCheat = -1;

	public antiCheatScript gameAntiCheatScript_;

	public int gameAP_Gameplay;

	public int gameAP_Grafik;

	public int gameAP_Sound;

	public int gameAP_Technik;

	public bool[] gameLanguage;

	public bool[] gameGameplayFeatures;

	public int[] gamePlatform;

	public platformScript[] gamePlatformScript;

	public int[] gameEngineFeature;

	public bool[] gameplayFeatures_DevDone;

	public bool[] engineFeature_DevDone;

	public bool[] gameplayStudio;

	public bool[] grafikStudio;

	public bool[] soundStudio;

	public bool[] motionCaptureStudio;

	public int[] sellsPerWeek;

	public float[] sellsPerWeekOnline;

	public bool[] fanbrief;

	public bool[] inAppPurchase;

	public int releaseDate;

	public bool[] standard_edition;

	public bool[] deluxe_edition;

	public bool[] collectors_edition;

	public int[] verkaufspreis;

	public long[] lagerbestand;

	public bool autoPreis;

	public int vorbestellungen;

	public int stornierungen;

	public bool retailVersion;

	public bool digitalVersion;

	public long abonnements;

	public long abonnementsWoche;

	public int abosAddons;

	public int aboPreis;

	public int aboPreisOld;

	public int inAppPurchaseWeek;

	public long bestAbonnements;

	public int finanzierung_Grundkosten;

	public int finanzierung_Technology;

	public int finanzierung_Kontent;

	public int[] Designschwerpunkt;

	public int[] Designausrichtung;

	public int arcadeCase;

	public int arcadeMonitor;

	public int arcadeJoystick;

	public int arcadeSound;

	public int arcadeProdCosts;

	public bool schublade;

	public int schubladeTaskID = -1;

	public bool commercialFlop;

	public bool commercialHit;

	public bool newGenreCombination;

	public bool newTopicCombination;

	public bool npcLateinNumbers;

	public bool ipToSell;

	public bool pubAngebot;

	public int pubAngebot_Weeks;

	public float pubAngebot_Verhandlung;

	public float pubAngebot_VerhandlungProzent = 100f;

	public float pubAngebot_Stimmung = 100f;

	public bool pubAngebot_Retail;

	public bool pubAngebot_Digital;

	public int pubAngebot_Garantiesumme;

	public float pubAngebot_Gewinnbeteiligung;

	public bool pubAnbgebot_Inivs;

	public bool pubAngebot_AngebotWoche;

	public bool auftragsspiel;

	public int auftragsspiel_gehalt;

	public int auftragsspiel_bonus;

	public int auftragsspiel_zeitInWochen;

	public int auftragsspiel_wochenAlsAngebot;

	public bool auftragsspiel_zeitAbgelaufen;

	public int auftragsspiel_mindestbewertung;

	public bool auftragsspiel_Inivs;

	public bool auftragsspiel_Blocked;

	public float[] merchVerkaufspreis;

	public int[] merchGesamtSells;

	public int[] merchLetzterMonat;

	public int[] merchDiesenMonat;

	public long merchGesamtGewinn;

	public long merchGewinnLetzterMonat;

	public long merchGewinnDiesenMonat;

	public bool merchKeinVerkauf;

	public int[] merchBestellungen;

	public float merchGesamtReviewPoints;

	public long tw_gewinnanteil;

	public long subvention;

	public float realsticPower;

	public int devFortsetzen;

	public bool DONT_SEND_GAME;

	public bool gameTab_fullView = true;

	public long bufferIP_anzahlGames;

	public long bufferIP_sells;

	public long bufferIP_downloads;

	public long bufferIP_umsatz;

	public bool IGNORE_SetAsGameInDevelopmentNPC;

	private Menu_Fanshop menuFanshop_;

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
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
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
			if (!themes_)
			{
				themes_ = main_.GetComponent<themes>();
			}
			if (!unlock_)
			{
				unlock_ = main_.GetComponent<unlockScript>();
			}
			if (!licences_)
			{
				licences_ = main_.GetComponent<licences>();
			}
			if (!gpS_)
			{
				gpS_ = main_.GetComponent<gamepassScript>();
			}
		}
		if (!guiMain_)
		{
			GameObject gameObject = GameObject.Find("CanvasInGameMenu");
			if ((bool)gameObject)
			{
				guiMain_ = gameObject.GetComponent<GUI_Main>();
			}
		}
		if (!sfx_)
		{
			GameObject gameObject2 = GameObject.Find("SFX");
			if ((bool)gameObject2)
			{
				sfx_ = gameObject2.GetComponent<sfxScript>();
			}
		}
	}

	private void OnDestroy()
	{
		if (!games_)
		{
			return;
		}
		if (portID != -1)
		{
			games_.RemovePortFlags(this);
			return;
		}
		if (typ_nachfolger && portID == -1)
		{
			gameScript gameScript2 = FindGameScriptWithID(originalGameID);
			if ((bool)gameScript2)
			{
				gameScript2.nachfolger_created = false;
			}
		}
		if (typ_remaster)
		{
			gameScript gameScript3 = FindGameScriptWithID(originalGameID);
			if ((bool)gameScript3)
			{
				gameScript3.remaster_created = false;
			}
		}
		if (typ_budget)
		{
			gameScript gameScript4 = FindGameScriptWithID(originalGameID);
			if ((bool)gameScript4)
			{
				gameScript4.budget_created = false;
			}
		}
		if (typ_goty)
		{
			gameScript gameScript5 = FindGameScriptWithID(originalGameID);
			if ((bool)gameScript5)
			{
				gameScript5.goty_created = false;
			}
		}
	}

	public void SetGameObjectName()
	{
		base.name = "GAME_" + myID;
	}

	public void InitArrays()
	{
		gameGameplayFeatures = new bool[gF_.gameplayFeatures_UNLOCK.Length];
		gameplayFeatures_DevDone = new bool[gF_.gameplayFeatures_UNLOCK.Length];
		fanbrief = new bool[tS_.fanLetter_GE.Length];
	}

	public void InitUI()
	{
		if (IsMyGame() && !gameTab_ && (isOnMarket || schublade))
		{
			FindScripts();
			GameObject gameObject = UnityEngine.Object.Instantiate(guiMain_.uiPrefabs[11], guiMain_.uiObjects[81].transform);
			gameTab_ = gameObject.GetComponent<gameTab>();
			gameTab_.gS_ = this;
			gameTab_.mS_ = mS_;
			gameTab_.main_ = main_;
			gameTab_.guiMain_ = guiMain_;
			gameTab_.sfx_ = sfx_;
			gameTab_.tS_ = tS_;
			gameTab_.themes_ = themes_;
			gameTab_.genres_ = genres_;
			gameTab_.Init(myID);
		}
	}

	public float GetUserReviewPercent()
	{
		if (userPositiv <= 0 && userNegativ <= 0)
		{
			return 0f;
		}
		if (userPositiv > 1 && userNegativ <= 0)
		{
			return 100f;
		}
		if (userPositiv <= 0 && userNegativ > 0)
		{
			return 0f;
		}
		float num = userPositiv + userNegativ;
		num = 100f / num * (float)userPositiv;
		return mS_.Round(num, 1);
	}

	public string GetTooltipIP()
	{
		FindScripts();
		int num = 0;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].mainIP == mainIP)
			{
				num++;
				bufferIP_anzahlGames++;
				if (gameTyp != 2 && games_.arrayGamesScripts[i].sellsTotal > 0)
				{
					bufferIP_sells += games_.arrayGamesScripts[i].sellsTotal;
					float num5 = games_.arrayGamesScripts[i].sellsTotal;
					num2 += num5 / 1000000f;
				}
				if (gameTyp == 2 && games_.arrayGamesScripts[i].sellsTotal > 0)
				{
					bufferIP_downloads += games_.arrayGamesScripts[i].sellsTotal;
					float num6 = games_.arrayGamesScripts[i].sellsTotal;
					num3 += num6 / 1000000f;
				}
				if (games_.arrayGamesScripts[i].umsatzTotal > 0)
				{
					bufferIP_umsatz += games_.arrayGamesScripts[i].umsatzTotal;
					float num7 = games_.arrayGamesScripts[i].umsatzTotal;
					num4 += num7 / 1000000f;
				}
			}
		}
		if (mainIP != -1)
		{
			string text = "";
			text = text + "<b><size=18>" + GetIpName() + "</size></b>\n";
			text = text + "<b>" + GetOwnerName() + "</b>\n";
			if (!inDevelopment && !schublade)
			{
				text = text + GetReleaseDateString() + "\n\n";
			}
			text = text + tS_.GetText(1555) + ": <color=blue>" + mS_.Round(GetIpBekanntheit(), 1) + "</color>\n";
			text = text + tS_.GetText(1559) + " <color=blue>" + num + "</color>\n";
			text += "\n";
			text = text + tS_.GetText(275) + ": <color=blue>" + mS_.Round(num2, 2) + " " + tS_.GetText(1558) + "</color>\n";
			text = text + tS_.GetText(697) + ": <color=blue>" + mS_.Round(num3, 2) + " " + tS_.GetText(1558) + "</color>\n";
			if (merchGesamtGewinn > 0)
			{
				float value = (float)merchGesamtGewinn / 1000000f;
				text = text + tS_.GetText(1842) + ": <color=blue>$" + mS_.Round(value, 2) + " " + tS_.GetText(1558) + "</color>\n";
			}
			text = text + tS_.GetText(276) + ": <color=blue>$" + mS_.Round(num4, 2) + " " + tS_.GetText(1558) + "</color>\n";
			text = text + "\n" + tS_.GetText(2237) + ": <color=blue>" + mS_.GetMoney(GetIpWert(), showDollar: true) + "</color>\n";
			if (ipTime > 0)
			{
				string text2 = "\n" + tS_.GetText(1899);
				text2 = text2.Replace("<NUM>", "<color=red>" + mS_.GetMoney(ipTime, showDollar: false) + "</color>");
				text += text2;
			}
			if (ipToSell)
			{
				text = text + "\n\n<color=red><b>" + tS_.GetText(2242) + "</b></color>";
			}
			return text;
		}
		return "";
	}

	public string GetTooltip()
	{
		if (pubAngebot)
		{
			return PUBOFFER_GetTooltip(reviewTotal);
		}
		FindMyEngineNew();
		string text = "";
		text = text + "<b><size=18>" + GetNameWithTag() + "</size></b>\n";
		if (IsMyAuftragsspiel())
		{
			text = text + "<color=blue><b>" + tS_.GetText(598) + "</b></color>\n";
		}
		if (pubOffer)
		{
			text = text + "<color=blue><b>" + tS_.GetText(1744) + "</b></color>\n";
			text = text + "<color=blue>" + PUBOFFER_GetRetailDigitalString() + "</color>\n";
			if (publisherID == mS_.myID)
			{
				text = text + "<color=red>" + tS_.GetText(1983) + ": " + Mathf.RoundToInt(PUBOFFER_GetGewinnbeteiligung()) + "%</color>\n";
			}
		}
		text += "<b>";
		if (!pS_)
		{
			FindMyPublisher();
		}
		if (!devS_)
		{
			FindMyDeveloper();
		}
		text = ((!devS_) ? (text + GetDeveloperName()) : ((!devS_.IsMyTochterfirma()) ? (text + GetDeveloperName()) : (text + "<color=green>" + GetDeveloperName() + "</color>")));
		text = ((!pS_) ? (text + " | " + GetPublisherName()) : ((!pS_.IsMyTochterfirma()) ? (text + " | " + GetPublisherName()) : (text + " | <color=green>" + GetPublisherName() + "</color>")));
		text += "\n";
		text = (inDevelopment ? (text + tS_.GetText(528) + "\n") : (text + GetReleaseDateString() + "\n"));
		if (!typ_bundle && !typ_bundleAddon)
		{
			text = text + GetTypString() + " | " + GetPlatformTypString() + "\n";
		}
		if (typ_bundle)
		{
			text = text + GetTypString() + "\n\n";
			for (int i = 0; i < bundleID.Length; i++)
			{
				gameScript bundleGame = GetBundleGame(i);
				if ((bool)bundleGame)
				{
					text = text + bundleGame.GetNameWithTag() + "\n";
				}
			}
		}
		if (typ_bundleAddon)
		{
			text = text + GetTypString() + "\n";
			text = text + GetGenreString() + "\n\n";
			for (int j = 0; j < bundleID.Length; j++)
			{
				gameScript bundleGame2 = GetBundleGame(j);
				if ((bool)bundleGame2)
				{
					text = text + bundleGame2.GetNameWithTag() + "\n";
				}
			}
		}
		text += "</b><size=12>";
		if (!typ_bundle && !typ_bundleAddon && subgenre == -1)
		{
			text = text + GetGenreString() + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && subgenre != -1)
		{
			text = text + GetGenreString() + " / " + GetSubGenreString() + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && gameSubTheme == -1)
		{
			text = text + tS_.GetThemes(gameMainTheme) + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && gameSubTheme != -1)
		{
			text = text + tS_.GetThemes(gameMainTheme) + " / " + tS_.GetThemes(gameSubTheme) + "\n";
		}
		text += "</size>\n";
		bool flag = false;
		if (goty || typ_goty)
		{
			flag = true;
			text = text + "<color=green><b>★ " + tS_.GetText(770) + " ★</b></color>\n";
		}
		if (commercialFlop && weeksOnMarket >= 4)
		{
			flag = true;
			text = text + "<color=red><b>" + tS_.GetText(1757) + "</b></color>\n";
		}
		if (commercialHit && weeksOnMarket >= 4)
		{
			flag = true;
			text = text + "<color=green><b>★ " + tS_.GetText(1762) + " ★</b></color>\n";
		}
		if (newGenreCombination)
		{
			flag = true;
			text = text + "<color=green><b>★ " + tS_.GetText(1768) + " ★</b></color>\n";
		}
		if (newTopicCombination)
		{
			flag = true;
			text = text + "<color=green><b>★ " + tS_.GetText(1769) + " ★</b></color>\n";
		}
		if (flag)
		{
			text += "\n";
		}
		if (subvention > 0)
		{
			text = text + "<color=green>" + tS_.GetText(2385) + " [" + mS_.GetMoney(subvention, showDollar: true) + "]</color>\n";
		}
		text += "<size=12>";
		if (!gamePlatformScript[0])
		{
			FindMyPlatforms();
		}
		for (int k = 0; k < gamePlatformScript.Length; k++)
		{
			if ((bool)gamePlatformScript[k])
			{
				text = ((!gamePlatformScript[k].IsVerfuegbar()) ? (text + "<color=red>" + gamePlatformScript[k].GetName() + "</color>\n") : (text + "<color=grey>" + gamePlatformScript[k].GetName() + "</color>\n"));
			}
		}
		text += "</size>\n";
		if (gameLicence != -1)
		{
			text = text + "<b>" + tS_.GetText(356) + "\n</b>" + licences_.GetName(gameLicence) + "\n\n";
		}
		if (!typ_bundle)
		{
			text = text + tS_.GetText(2405) + ": <color=blue>" + GetIpName_NotTheMaingame() + "</color>\n";
			text = text + tS_.GetText(1555) + ": <color=blue>" + mS_.Round(GetIpBekanntheit(), 1) + "</color>\n";
		}
		if ((developerID == mS_.myID || publisherID == mS_.myID) && !typ_bundle && !typ_bundleAddon)
		{
			text = text + tS_.GetText(1293) + ": <color=blue>" + GetEntwicklungsbeginnDateString() + "</color>\n";
		}
		text = text + tS_.GetText(327) + ": <color=blue>" + GetGamesizeString() + "</color>\n";
		text = text + tS_.GetText(336) + ": <color=blue>" + GetZielgruppeString() + "</color>\n";
		if (!typ_bundle)
		{
			text = ((!DeveloperIsNPC() || engineID != 0) ? (text + tS_.GetText(369) + ": <color=blue>" + engineS_.GetName() + "</color>\n") : (text + tS_.GetText(369) + ": <color=blue>" + tS_.GetText(1561) + "</color>\n"));
		}
		if (!schublade && !inDevelopment)
		{
			text = text + tS_.GetText(990) + ": <color=blue>" + GetUskString() + "</color>\n";
			text = text + tS_.GetText(277) + ": <color=blue>" + Mathf.RoundToInt(reviewTotal) + "%</color>";
		}
		if (IsMyGame() && (isOnMarket || weeksOnMarket <= 0))
		{
			text = ((schublade || inDevelopment) ? (text + tS_.GetText(433) + ": <color=blue>" + Mathf.RoundToInt(GetHype()) + "</color>") : (text + " | " + tS_.GetText(433) + ": <color=blue>" + Mathf.RoundToInt(GetHype()) + "</color>"));
		}
		if (isOnMarket && gameTyp != 2 && !handy)
		{
			text = text + "\n" + tS_.GetText(491) + ": <color=blue>" + weeksOnMarket + "</color>";
		}
		if (isOnMarket && (gameTyp == 2 || handy))
		{
			text = text + "\n" + tS_.GetText(1391) + ": <color=blue>" + weeksOnMarket + "</color>";
		}
		if (bestChartPosition != 0)
		{
			text = text + "\n" + tS_.GetText(1669) + ": <color=blue>" + bestChartPosition + "</color>";
		}
		if (isOnMarket || sellsTotal > 0)
		{
			text = text + "\n" + tS_.GetText(1289) + ": (" + GetUserReviewPercent() + "%)<color=green> " + mS_.GetMoney(userPositiv, showDollar: false) + "</color> | <color=red>" + mS_.GetMoney(userNegativ, showDollar: false) + "</color>";
		}
		if (amountUpdates > 0)
		{
			text = text + "\n" + tS_.GetText(649) + ": <color=blue>" + amountUpdates + "</color>";
		}
		if (amountAddons > 0)
		{
			text = text + "\n" + tS_.GetText(956) + ": <color=blue>" + amountAddons + "</color>";
		}
		if (amountMMOAddons > 0)
		{
			text = text + "\n" + tS_.GetText(956) + ": <color=blue>" + amountMMOAddons + "</color>";
		}
		if (exklusiv && exklusivKonsolenSells > 0)
		{
			if (!gamePlatformScript[0])
			{
				FindMyPlatforms();
			}
			if ((bool)gamePlatformScript[0])
			{
				string text2 = "\n" + tS_.GetText(1313);
				text2 = text2.Replace("<NUM>", mS_.GetMoney(exklusivKonsolenSells, showDollar: false));
				text2 = text2.Replace("<NAME>", gamePlatformScript[0].GetName());
				text += text2;
			}
		}
		if (herstellerExklusiv && exklusivKonsolenSells > 0)
		{
			if (!gamePlatformScript[0])
			{
				FindMyPlatforms();
			}
			if ((bool)gamePlatformScript[0])
			{
				string text3 = "\n" + tS_.GetText(1313);
				text3 = text3.Replace("<NUM>", mS_.GetMoney(exklusivKonsolenSells, showDollar: false));
				text3 = text3.Replace("<NAME>", tS_.GetText(675));
				text += text3;
			}
		}
		text += "\n\n";
		if (!inDevelopment && !schublade)
		{
			if (gameTyp == 2 && (abonnements > 0 || bestAbonnements > 0))
			{
				text = text + tS_.GetText(1392) + ": <color=blue>" + mS_.GetMoney(abonnements, showDollar: false) + "</color> | " + tS_.GetText(1905) + ": <color=blue>" + mS_.GetMoney(bestAbonnements, showDollar: false) + "</color>\n";
			}
			if (gameTyp == 1 && (abonnements > 0 || bestAbonnements > 0))
			{
				text = text + tS_.GetText(1236) + ": <color=blue>" + mS_.GetMoney(abonnements, showDollar: false) + "</color> | " + tS_.GetText(1905) + ": <color=blue>" + mS_.GetMoney(bestAbonnements, showDollar: false) + "</color>\n";
			}
			if (gameTyp != 2 && !handy)
			{
				text = text + tS_.GetText(275) + ": <color=blue>" + mS_.GetMoney(sellsTotal, showDollar: false) + "</color>\n";
			}
			if (gameTyp == 2 || handy)
			{
				text = text + tS_.GetText(697) + ": <color=blue>" + mS_.GetMoney(sellsTotal, showDollar: false) + "</color>\n";
			}
			text = text + tS_.GetText(276) + ": <color=blue>" + mS_.GetMoney(umsatzTotal, showDollar: true) + "</color>\n";
			if (!IsMyGame() && GetPublisherOrDeveloperIsTochterfirma())
			{
				text = text + "\n<color=green><b>" + tS_.GetText(1987) + "</b></color>\n";
				text = text + tS_.GetText(1986) + ": <color=blue>" + mS_.GetMoney(tw_gewinnanteil, showDollar: true) + "</color>\n";
			}
		}
		if (IsMyGame())
		{
			text = ((GetGesamtGewinn() < 0) ? (text + tS_.GetText(724) + ": <color=red>" + mS_.GetMoney(GetGesamtGewinn(), showDollar: true) + "</color>\n") : (text + tS_.GetText(724) + ": <color=blue>" + mS_.GetMoney(GetGesamtGewinn(), showDollar: true) + "</color>\n"));
			if (!schublade && publisherID == mS_.myID && gameTyp != 2 && !handy && !arcade)
			{
				text += "\n";
				text = text + tS_.GetText(1103) + ": <color=blue>" + mS_.GetMoney(sellsTotalStandard, showDollar: false) + "</color>\n";
				if (sellsTotalDeluxe > 0)
				{
					text = text + tS_.GetText(1104) + ": <color=blue>" + mS_.GetMoney(sellsTotalDeluxe, showDollar: false) + "</color>\n";
				}
				if (sellsTotalCollectors > 0)
				{
					text = text + tS_.GetText(1105) + ": <color=blue>" + mS_.GetMoney(sellsTotalCollectors, showDollar: false) + "</color>\n";
				}
				if (sellsTotalOnline > 0)
				{
					text = text + tS_.GetText(1126) + ": <color=blue>" + mS_.GetMoney(sellsTotalOnline, showDollar: false) + "</color>\n";
				}
			}
		}
		if (inGamePass)
		{
			bool flag2 = false;
			for (int l = 0; l < gamePlatformScript.Length; l++)
			{
				if ((bool)gamePlatformScript[l] && (gamePlatformScript[l].inGamePass || gamePlatformScript[l].inGamePassPassiv))
				{
					flag2 = true;
					text = text + "\n<color=blue>" + tS_.GetText(2130) + "</color>";
					break;
				}
			}
			if (!flag2)
			{
				text = text + "\n<color=red>" + tS_.GetText(2133) + "</color>";
			}
		}
		if (IsMyGame() && points_bugs > 0f)
		{
			text = text + "\n<color=red>" + tS_.GetText(1761) + "</color>";
		}
		if (IsRemovedFromMarket())
		{
			text = text + "\n<color=red>" + tS_.GetText(488) + "</color>";
		}
		return text;
	}

	public string GetOwnerName()
	{
		if (!ownerS_)
		{
			FindMyOwner();
		}
		if ((bool)ownerS_)
		{
			return ownerS_.GetName();
		}
		return "";
	}

	public string GetDeveloperName()
	{
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if ((bool)devS_)
		{
			return devS_.GetName();
		}
		return "";
	}

	public string GetPublisherName()
	{
		if (!pS_)
		{
			FindMyPublisher();
		}
		if ((bool)pS_)
		{
			return pS_.GetName();
		}
		return "";
	}

	public string GetGenreString()
	{
		return genres_.GetName(maingenre);
	}

	public string GetSubGenreString()
	{
		if (subgenre != -1)
		{
			return genres_.GetName(subgenre);
		}
		return "";
	}

	public string GetReleaseDateString()
	{
		if (inDevelopment)
		{
			return tS_.GetText(528);
		}
		if (schublade)
		{
			return tS_.GetText(1704);
		}
		return tS_.GetText(date_month + 220) + " " + date_year;
	}

	public string GetEntwicklungsbeginnDateString()
	{
		if (date_start_year == 0)
		{
			return "-";
		}
		return tS_.GetText(date_start_month + 220) + " " + date_start_year;
	}

	public Sprite GetScreenshot()
	{
		return genres_.GetScreenshot(maingenre, Mathf.RoundToInt(points_grafik));
	}

	public Texture2D GetScreenshotTexture2D()
	{
		return genres_.GetScreenshotTexture2D(maingenre, Mathf.RoundToInt(points_grafik));
	}

	public void FindNextFeatureForDevelopment()
	{
		devPointsStart = 0f;
		for (int i = 0; i < gameEngineFeature.Length; i++)
		{
			if (!engineFeature_DevDone[i])
			{
				engineFeature_DevDone[i] = true;
				devAktFeature = i - 4;
				devPointsStart = eF_.GetDevPointsForGame(gameEngineFeature[i]);
				devPointsStart = CalcPlatformComplex(devPointsStart);
				devPoints = devPointsStart;
				return;
			}
		}
		for (int j = 0; j < gameGameplayFeatures.Length; j++)
		{
			if (!gameplayFeatures_DevDone[j] && gameGameplayFeatures[j])
			{
				gameplayFeatures_DevDone[j] = true;
				devAktFeature = j;
				devPointsStart = gF_.GetDevPoints(j);
				devPointsStart = CalcPlatformComplex(devPointsStart);
				devPoints = devPointsStart;
				break;
			}
		}
	}

	public int GetPointsForAdds()
	{
		int num = 0;
		for (int i = 0; i < gameEngineFeature.Length; i++)
		{
			num += eF_.GetDevPointsForGame(gameEngineFeature[i]);
		}
		for (int j = 0; j < gamePlatform.Length; j++)
		{
			if (gamePlatform[j] != -1)
			{
				num += 100;
			}
		}
		num *= 1 + gameSize;
		float num2 = num;
		num2 *= 0.4f;
		return Mathf.RoundToInt(CalcPlatformComplex(num2));
	}

	public int GetGesamtDevPoints()
	{
		int num = 0;
		for (int i = 0; i < gameEngineFeature.Length; i++)
		{
			num += eF_.GetDevPointsForGame(gameEngineFeature[i]);
		}
		for (int j = 0; j < gameGameplayFeatures.Length; j++)
		{
			if (gameGameplayFeatures[j])
			{
				num += gF_.GetDevPoints(j);
			}
		}
		return Mathf.RoundToInt(CalcPlatformComplex(num));
	}

	public int GetGesamtDevPointsAddon()
	{
		int num = 0;
		for (int i = 0; i < gameEngineFeature.Length; i++)
		{
			if (!engineFeature_DevDone[i])
			{
				num += eF_.GetDevPointsForGame(gameEngineFeature[i]);
			}
		}
		for (int j = 0; j < gameGameplayFeatures.Length; j++)
		{
			if (gameGameplayFeatures[j] && !gameplayFeatures_DevDone[j])
			{
				num += gF_.GetDevPoints(j);
			}
		}
		return num;
	}

	public long GetFreewareFans()
	{
		return reviewTotal * 8 + sellsTotal / 4500;
	}

	public int GetRueckggeld()
	{
		int num = 0;
		if (!typ_contractGame)
		{
			return Mathf.RoundToInt((float)((int)costs_entwicklung / 100) * (100f - GetProzentGesamt()));
		}
		return Mathf.RoundToInt((float)(-auftragsspiel_gehalt) * 1.5f);
	}

	public long GetBisherigeEntwicklungskosten()
	{
		long num = GetRueckggeld();
		num = costs_entwicklung - num;
		return num + costs_mitarbeiter;
	}

	public float GetProzentGesamt()
	{
		float num = 100f / devPointsStart_Gesamt * (devPointsStart_Gesamt - devPoints_Gesamt);
		if (num > 100f)
		{
			num = 100f;
		}
		return num;
	}

	public float GetProzentFeature()
	{
		float num = 100f / devPointsStart * (devPointsStart - devPoints);
		if (num > 100f)
		{
			num = 100f;
		}
		return num;
	}

	public long GetEntwicklungskosten()
	{
		return costs_entwicklung + costs_mitarbeiter;
	}

	public long GetGesamteEntwicklungskosten()
	{
		return costs_entwicklung + costs_mitarbeiter + costs_updates;
	}

	public long GetMarketingkosten()
	{
		return costs_marketing;
	}

	public long GetEnginegebuehren()
	{
		return costs_enginegebuehren;
	}

	public long GetUmsatzVerkauf()
	{
		return umsatzTotal - umsatzAbos - umsatzInApp;
	}

	public long GetUmsatzbeteiligung()
	{
		return umsatzTotal / 100 * Mathf.RoundToInt(PUBOFFER_GetGewinnbeteiligung());
	}

	public int SubGewinnbeteiligung(int i)
	{
		float num = (float)i / 100f;
		num *= (float)PUBOFFER_GetGewinnbeteiligung();
		return Mathf.RoundToInt(i - Mathf.RoundToInt(num));
	}

	public void PayGewinnbeteiligung(long i)
	{
		if (PUBOFFER_GetGewinnbeteiligung() > 0)
		{
			float num = (float)i / 100f;
			num *= (float)PUBOFFER_GetGewinnbeteiligung();
			mS_.Pay(Mathf.RoundToInt(num), 26);
		}
	}

	public long GetGesamtAusgaben()
	{
		return costs_entwicklung + costs_mitarbeiter + costs_marketing + costs_enginegebuehren + costs_server + costs_production + costs_updates;
	}

	public long GetGesamtGewinn()
	{
		long num = 0L;
		if (!pubOffer)
		{
			num = costs_entwicklung;
			num += costs_mitarbeiter;
			num += costs_marketing;
			num += costs_enginegebuehren;
			num += costs_server;
			num += costs_production;
			num += costs_updates;
		}
		else
		{
			num = PUBOFFER_GetGarantiesumme();
			num += costs_marketing;
			num += costs_enginegebuehren;
			num += costs_server;
			num += costs_production;
			num += costs_updates;
			if (PUBOFFER_GetGewinnbeteiligung() > 0)
			{
				num += GetUmsatzbeteiligung();
			}
		}
		return umsatzTotal - num;
	}

	public int GetTypINT()
	{
		if (typ_contractGame)
		{
			return 20;
		}
		if (typ_bundle)
		{
			return 19;
		}
		if (typ_bundleAddon)
		{
			return 18;
		}
		if (typ_goty)
		{
			return 17;
		}
		if (typ_budget)
		{
			return 16;
		}
		if (typ_mmoaddon)
		{
			return 15;
		}
		if (typ_addon)
		{
			return 14;
		}
		if (typ_addonStandalone)
		{
			return 13;
		}
		switch (gameTyp)
		{
		case 1:
			return 0;
		case 2:
			return 1;
		default:
			if (typ_nachfolger)
			{
				return 5;
			}
			if (typ_remaster)
			{
				return 6;
			}
			if (typ_spinoff)
			{
				return 7;
			}
			return 3;
		}
	}

	public string GetTypString()
	{
		if (!tS_)
		{
			return "";
		}
		string text = "";
		if (typ_contractGame)
		{
			text = " [" + tS_.GetText(598) + "]";
		}
		if (typ_bundle)
		{
			return tS_.GetText(977) + text;
		}
		if (typ_bundleAddon)
		{
			return tS_.GetText(1354) + text;
		}
		if (typ_goty)
		{
			return tS_.GetText(1359) + text;
		}
		if (typ_budget)
		{
			return tS_.GetText(978) + text;
		}
		if (typ_mmoaddon)
		{
			return tS_.GetText(646) + text;
		}
		if (typ_addon)
		{
			return tS_.GetText(645) + text;
		}
		if (typ_addonStandalone)
		{
			return tS_.GetText(979) + text;
		}
		if (typ_standard || gameTyp == 1 || gameTyp == 2)
		{
			string text2 = "";
			if (typ_nachfolger)
			{
				text2 = "-" + tS_.GetText(319);
			}
			if (typ_remaster)
			{
				text2 = "-" + tS_.GetText(320);
			}
			if (typ_spinoff)
			{
				text2 = "-" + tS_.GetText(1535);
			}
			switch (gameTyp)
			{
			case 0:
				return tS_.GetText(486) + text2 + text;
			case 1:
				return tS_.GetText(1244) + text2 + text;
			case 2:
				return tS_.GetText(1245) + text2 + text;
			}
		}
		if (typ_nachfolger)
		{
			return tS_.GetText(319) + text;
		}
		if (typ_remaster)
		{
			return tS_.GetText(320) + text;
		}
		if (typ_spinoff)
		{
			return tS_.GetText(1535) + text;
		}
		return "";
	}

	public int GetPlatformTypINT()
	{
		if (herstellerExklusiv)
		{
			return 1;
		}
		if (exklusiv)
		{
			return 2;
		}
		if (retro)
		{
			return 5;
		}
		if (handy)
		{
			return 4;
		}
		if (arcade)
		{
			return 3;
		}
		if (!exklusiv && !retro)
		{
			_ = herstellerExklusiv;
			return 0;
		}
		return 0;
	}

	public string GetPlatformTypString()
	{
		if (!tS_)
		{
			return "";
		}
		if (herstellerExklusiv)
		{
			return tS_.GetText(1694);
		}
		if (exklusiv)
		{
			return tS_.GetText(364);
		}
		if (retro)
		{
			return tS_.GetText(903);
		}
		if (handy)
		{
			return tS_.GetText(1060);
		}
		if (arcade)
		{
			return tS_.GetText(1059);
		}
		if (!exklusiv && !retro && !herstellerExklusiv)
		{
			return tS_.GetText(902);
		}
		return "";
	}

	public Sprite GetSizeSprite()
	{
		if (!games_)
		{
			return null;
		}
		return games_.gameSizeSprites[gameSize];
	}

	public Sprite GetTypSprite()
	{
		if (!games_)
		{
			return null;
		}
		if (typ_contractGame)
		{
			return games_.gameTypSprites[3];
		}
		if (typ_bundle)
		{
			return games_.gameTypSprites[8];
		}
		if (typ_bundleAddon)
		{
			return games_.gameTypSprites[11];
		}
		if (typ_goty)
		{
			return games_.gameTypSprites[12];
		}
		if (typ_budget)
		{
			return games_.gameTypSprites[9];
		}
		if (typ_addon)
		{
			return games_.gameTypSprites[4];
		}
		if (typ_mmoaddon)
		{
			return games_.gameTypSprites[5];
		}
		if (typ_addonStandalone)
		{
			return games_.gameTypSprites[10];
		}
		if (typ_standard || gameTyp == 1 || gameTyp == 2)
		{
			switch (gameTyp)
			{
			case 0:
				return games_.gameTypSprites[0];
			case 1:
				return games_.gameTypSprites[6];
			case 2:
				return games_.gameTypSprites[7];
			}
		}
		if (gameTyp == 0)
		{
			if (typ_nachfolger)
			{
				return games_.gameTypSprites[1];
			}
			if (typ_remaster)
			{
				return games_.gameTypSprites[2];
			}
			if (typ_spinoff)
			{
				return games_.gameTypSprites[13];
			}
		}
		return null;
	}

	public Sprite GetPlatformTypSprite()
	{
		if (!games_)
		{
			return null;
		}
		if (herstellerExklusiv)
		{
			return games_.gamePlatformTypSprites[5];
		}
		if (exklusiv)
		{
			return games_.gamePlatformTypSprites[1];
		}
		if (retro)
		{
			return games_.gamePlatformTypSprites[2];
		}
		if (handy)
		{
			return games_.gamePlatformTypSprites[3];
		}
		if (arcade)
		{
			return games_.gamePlatformTypSprites[4];
		}
		return games_.gamePlatformTypSprites[0];
	}

	public void ClearReview()
	{
		date_month = 0;
		date_year = 0;
		reviewGameplay = 0;
		reviewGrafik = 0;
		reviewSound = 0;
		reviewSteuerung = 0;
		reviewTotal = 0;
	}

	public void CalcReview(bool entwicklungsbericht)
	{
		if (reviewTotal > 0)
		{
			return;
		}
		date_month = mS_.month;
		date_year = mS_.year;
		float num = 0f;
		if (developerID != mS_.myID)
		{
			num = (retro ? (4000f * games_.GetReviewCurve()) : (14000f * games_.GetReviewCurve()));
		}
		else if (!retro)
		{
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
		}
		else
		{
			switch (mS_.difficulty)
			{
			case 0:
				num = 2500f * games_.GetReviewCurve();
				break;
			case 1:
				num = 3000f * games_.GetReviewCurve();
				break;
			case 2:
				num = 3500f * games_.GetReviewCurve();
				break;
			case 3:
				num = 4000f * games_.GetReviewCurve();
				break;
			case 4:
				num = 4200f * games_.GetReviewCurve();
				break;
			case 5:
				num = 4500f * games_.GetReviewCurve();
				break;
			}
		}
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		num2 = ((mS_.year < 1979) ? (points_gameplay / (num / 90f)) : (points_gameplay / (num / 100f)));
		num3 = ((mS_.year < 1982) ? (points_grafik / (num / 90f)) : (points_grafik / (num / 100f)));
		num4 = ((mS_.year < 1985) ? (points_sound / (num / 90f)) : (points_sound / (num / 100f)));
		num5 = ((!unlock_.Get(8)) ? (points_technik / (num / 80f)) : (points_technik / (num / 100f)));
		float num6 = 0f;
		if (num2 > 99f)
		{
			num2 = 99f;
		}
		if (num3 > 99f)
		{
			num3 = 99f;
		}
		if (num4 > 99f)
		{
			num4 = 99f;
		}
		if (num5 > 99f)
		{
			num5 = 99f;
		}
		if (developerID != mS_.myID && retro)
		{
			num3 *= 0.7f;
			num4 *= 0.7f;
			num5 *= 0.7f;
		}
		if (developerID == mS_.myID)
		{
			float num7 = 0f;
			int num8 = 0;
			for (int i = 0; i < gamePlatform.Length; i++)
			{
				if (gamePlatform[i] != -1)
				{
					if (!gamePlatformScript[i])
					{
						FindMyPlatforms();
					}
					if ((bool)gamePlatformScript[i])
					{
						num8++;
						num7 += (float)gamePlatformScript[i].erfahrung;
					}
				}
			}
			num7 /= (float)num8;
			num3 -= (5f - num7) * 2f;
			num4 -= (5f - num7) * 2f;
			num5 -= (5f - num7) * 2f;
		}
		else if (!entwicklungsbericht)
		{
			num3 -= UnityEngine.Random.Range(0f, 5f);
			num4 -= UnityEngine.Random.Range(0f, 5f);
			num5 -= UnityEngine.Random.Range(0f, 5f);
		}
		else
		{
			num3 -= 2.5f;
			num4 -= 2.5f;
			num5 -= 2.5f;
		}
		if (developerID == mS_.myID && mS_.year >= 1979)
		{
			if (!gameplayStudio[0])
			{
				num2 -= 1f;
			}
			if (!gameplayStudio[1])
			{
				num2 -= 1f;
			}
			if (!gameplayStudio[2])
			{
				num2 -= 1f;
			}
			if (!gameplayStudio[3])
			{
				num2 -= 1f;
			}
			if (!gameplayStudio[4])
			{
				num2 -= 1f;
			}
			if (!gameplayStudio[5])
			{
				num2 -= 1f;
			}
		}
		if (mS_.year >= 1982)
		{
			if (!grafikStudio[0])
			{
				num3 -= 1f;
			}
			if (!grafikStudio[1])
			{
				num3 -= 1f;
			}
			if (!grafikStudio[2])
			{
				num3 -= 1f;
			}
			if (!grafikStudio[3])
			{
				num3 -= 1f;
			}
			if (!grafikStudio[4])
			{
				num3 -= 1f;
			}
			if (!grafikStudio[5])
			{
				num3 -= 1f;
			}
		}
		if (mS_.year >= 1985)
		{
			if (!soundStudio[0])
			{
				num4 -= 2f;
			}
			if (!soundStudio[1])
			{
				num4 -= 2f;
			}
			if (!soundStudio[2])
			{
				num4 -= 2f;
			}
			if (!soundStudio[3])
			{
				num4 -= 2f;
			}
			if (!soundStudio[4])
			{
				num4 -= 2f;
			}
			if (!soundStudio[5])
			{
				num4 -= 2f;
			}
		}
		if (unlock_.Get(8))
		{
			if (!motionCaptureStudio[0])
			{
				num3 -= 1f;
			}
			if (!motionCaptureStudio[1])
			{
				num3 -= 1f;
			}
			if (!motionCaptureStudio[2])
			{
				num3 -= 1f;
			}
			if (!motionCaptureStudio[3])
			{
				num3 -= 1f;
			}
			if (!motionCaptureStudio[4])
			{
				num3 -= 1f;
			}
			if (!motionCaptureStudio[5])
			{
				num3 -= 1f;
			}
		}
		if (!handy && !retro && developerID == mS_.myID)
		{
			int outdatetAmount = eF_.GetOutdatetAmount(gameEngineFeature[0]);
			int outdatetAmount2 = eF_.GetOutdatetAmount(gameEngineFeature[1]);
			int outdatetAmount3 = eF_.GetOutdatetAmount(gameEngineFeature[2]);
			int outdatetAmount4 = eF_.GetOutdatetAmount(gameEngineFeature[3]);
			if (outdatetAmount > 0)
			{
				num3 -= (float)(outdatetAmount * 2);
			}
			if (outdatetAmount2 > 0)
			{
				num4 -= (float)(outdatetAmount2 * 2);
			}
			if (outdatetAmount3 > 0)
			{
				num2 -= (float)outdatetAmount3;
			}
			if (outdatetAmount4 > 0)
			{
				num2 -= (float)outdatetAmount4;
			}
		}
		if (developerID == mS_.myID)
		{
			if (mS_.year >= 1983 && mS_.year < 1988 && gameSize == 0)
			{
				num2 -= 1f;
				num3 -= 1f;
				num4 -= 1f;
				num5 -= 1f;
			}
			if (mS_.year >= 1988 && mS_.year < 1995)
			{
				if (gameSize == 0)
				{
					num2 -= 2f;
					num3 -= 2f;
					num4 -= 2f;
					num5 -= 2f;
				}
				if (gameSize == 1)
				{
					num2 -= 1f;
					num3 -= 1f;
					num4 -= 1f;
					num5 -= 1f;
				}
			}
			if (mS_.year >= 1995 && mS_.year < 2004)
			{
				if (gameSize == 0)
				{
					num2 -= 3f;
					num3 -= 3f;
					num4 -= 3f;
					num5 -= 3f;
				}
				if (gameSize == 1)
				{
					num2 -= 2f;
					num3 -= 2f;
					num4 -= 2f;
					num5 -= 2f;
				}
				if (gameSize == 2)
				{
					num2 -= 1f;
					num3 -= 1f;
					num4 -= 1f;
					num5 -= 1f;
				}
			}
			if (mS_.year >= 2004 && mS_.year < 2020)
			{
				if (gameSize == 0)
				{
					num2 -= 4f;
					num3 -= 4f;
					num4 -= 4f;
					num5 -= 4f;
				}
				if (gameSize == 1)
				{
					num2 -= 3f;
					num3 -= 3f;
					num4 -= 3f;
					num5 -= 3f;
				}
				if (gameSize == 2)
				{
					num2 -= 2f;
					num3 -= 2f;
					num4 -= 2f;
					num5 -= 2f;
				}
				if (gameSize == 3)
				{
					num2 -= 1f;
					num3 -= 1f;
					num4 -= 1f;
					num5 -= 1f;
				}
			}
			if (mS_.year >= 2020)
			{
				if (gameSize == 0)
				{
					num2 -= 5f;
					num3 -= 5f;
					num4 -= 5f;
					num5 -= 5f;
				}
				if (gameSize == 1)
				{
					num2 -= 4f;
					num3 -= 4f;
					num4 -= 4f;
					num5 -= 4f;
				}
				if (gameSize == 2)
				{
					num2 -= 3f;
					num3 -= 3f;
					num4 -= 3f;
					num5 -= 3f;
				}
				if (gameSize == 3)
				{
					num2 -= 2f;
					num3 -= 2f;
					num4 -= 2f;
					num5 -= 2f;
				}
				if (gameSize == 4)
				{
					num2 -= 1f;
					num3 -= 1f;
					num4 -= 1f;
					num5 -= 1f;
				}
			}
		}
		if (gameLicence != -1)
		{
			if (licences_.licence_GENREGOOD[gameLicence] == maingenre)
			{
				num2 += licences_.licence_QUALITY[gameLicence] * 0.01f * 3f;
			}
			if (licences_.licence_GENREGOOD[gameLicence] != maingenre && licences_.licence_GENREBAD[gameLicence] != maingenre)
			{
				num2 += licences_.licence_QUALITY[gameLicence] * 0.01f * 1f;
			}
			if (licences_.licence_GENREBAD[gameLicence] == maingenre)
			{
				num2 -= licences_.licence_QUALITY[gameLicence] * 0.01f * 5f;
			}
			if (subgenre != -1)
			{
				if (licences_.licence_GENREGOOD[gameLicence] == subgenre)
				{
					num2 += licences_.licence_QUALITY[gameLicence] * 0.01f * 2f;
				}
				if (licences_.licence_GENREGOOD[gameLicence] != subgenre && licences_.licence_GENREBAD[gameLicence] != subgenre)
				{
					num2 += licences_.licence_QUALITY[gameLicence] * 0.01f * 1f;
				}
				if (licences_.licence_GENREBAD[gameLicence] == subgenre)
				{
					num2 -= licences_.licence_QUALITY[gameLicence] * 0.01f * 3f;
				}
			}
		}
		if (developerID != mS_.myID)
		{
			if (!entwicklungsbericht)
			{
				num2 -= UnityEngine.Random.Range(0f, 2f);
				num3 -= UnityEngine.Random.Range(0f, 2f);
				num4 -= UnityEngine.Random.Range(0f, 2f);
				num5 -= UnityEngine.Random.Range(0f, 2f);
			}
			else
			{
				num2 -= 1f;
				num3 -= 1f;
				num4 -= 1f;
				num5 -= 1f;
			}
		}
		num6 += num2 * 0.01f * genres_.genres_GAMEPLAY[maingenre];
		num6 += num3 * 0.01f * genres_.genres_GRAPHIC[maingenre];
		num6 += num4 * 0.01f * genres_.genres_SOUND[maingenre];
		num6 += num5 * 0.01f * genres_.genres_CONTROL[maingenre];
		num3 -= points_bugs * 0.1f;
		num4 -= points_bugs * 0.1f;
		num5 -= points_bugs * 0.1f;
		num2 -= points_bugs * 0.2f;
		num6 -= points_bugs * 0.1f;
		if (!genres_.IsTargetGroup(maingenre, gameZielgruppe))
		{
			num2 -= 4f;
			num6 -= 3f;
		}
		if (!genres_.IsGenreCombination(maingenre, subgenre))
		{
			num2 -= 4f;
			num6 -= 3f;
		}
		if (!themes_.IsThemesFitWithGenre(gameMainTheme, maingenre))
		{
			num2 -= 4f;
			num6 -= 3f;
		}
		if (!themes_.IsThemesFitWithGenre(gameSubTheme, maingenre))
		{
			num2 -= 2f;
			num6 -= 1.5f;
		}
		for (int j = 0; j < Designschwerpunkt.Length; j++)
		{
			if (Designschwerpunkt[j] < genres_.GetFocus(j, maingenre, subgenre))
			{
				float num9 = genres_.GetFocus(j, maingenre, subgenre) - Designschwerpunkt[j];
				switch (mS_.difficulty)
				{
				case 0:
					num2 -= num9 * 0.3f;
					num6 -= num9 * 0.2f;
					break;
				case 1:
					num2 -= num9 * 0.4f;
					num6 -= num9 * 0.3f;
					break;
				case 2:
					num2 -= num9 * 0.5f;
					num6 -= num9 * 0.4f;
					break;
				case 3:
					num2 -= num9 * 0.8f;
					num6 -= num9 * 0.5f;
					break;
				case 4:
					num2 -= num9 * 0.9f;
					num6 -= num9 * 0.6f;
					break;
				case 5:
					num2 -= num9 * 1f;
					num6 -= num9 * 0.7f;
					break;
				}
			}
		}
		float num10 = 0f;
		for (int k = 0; k < Designausrichtung.Length; k++)
		{
			num10 = Mathf.Abs(Designausrichtung[k] - genres_.GetAlign(k, maingenre, subgenre));
			switch (mS_.difficulty)
			{
			case 0:
				num2 -= num10 * 0.3f;
				num6 -= num10 * 0.2f;
				break;
			case 1:
				num2 -= num10 * 0.4f;
				num6 -= num10 * 0.3f;
				break;
			case 2:
				num2 -= num10 * 0.5f;
				num6 -= num10 * 0.4f;
				break;
			case 3:
				num2 -= num10 * 0.8f;
				num6 -= num10 * 0.5f;
				break;
			case 4:
				num2 -= num10 * 0.9f;
				num6 -= num10 * 0.6f;
				break;
			case 5:
				num2 -= num10 * 1f;
				num6 -= num10 * 0.7f;
				break;
			}
		}
		if (developerID == mS_.myID)
		{
			num10 = (5f - (float)genres_.genres_LEVEL[maingenre]) * 0.4f;
			num2 -= num10 * 2f;
			num6 -= num10 * 1.5f;
		}
		if (developerID == mS_.myID)
		{
			if (subgenre >= 0)
			{
				num10 = (5f - (float)genres_.genres_LEVEL[subgenre]) * 0.2f;
				num2 -= num10 * 2f;
				num6 -= num10 * 1.5f;
			}
			else
			{
				num2 -= 2f;
				num6 -= 1.5f;
			}
		}
		if (developerID == mS_.myID)
		{
			num10 = (5f - (float)themes_.themes_LEVEL[gameMainTheme]) * 0.4f;
			num2 -= num10 * 2f;
			num6 -= num10 * 1.5f;
		}
		if (developerID == mS_.myID)
		{
			if (gameSubTheme >= 0)
			{
				num10 = (5f - (float)themes_.themes_LEVEL[gameSubTheme]) * 0.2f;
				num2 -= num10 * 2f;
				num6 -= num10 * 1.5f;
			}
			else
			{
				num2 -= 2f;
				num6 -= 1.5f;
			}
		}
		if (developerID == mS_.myID)
		{
			if (typ_addon || typ_addonStandalone)
			{
				float num11 = 0.4f;
				num11 -= addonQuality;
				num2 -= num2 * num11;
				num6 -= num6 * num11;
			}
			if (typ_mmoaddon)
			{
				float num12 = 0.4f;
				num12 -= addonQuality;
				num2 -= num2 * num12;
				num6 -= num6 * num12;
			}
		}
		if (developerID == mS_.myID && finanzierung_Grundkosten < 100)
		{
			float num13 = finanzierung_Grundkosten;
			num13 *= 0.01f;
			float num14 = num6 - num6 * num13;
			num6 -= num14 * 0.5f;
			num14 = num6 - num2 * num13;
			num2 -= num14 * 0.5f;
			num14 = num3 - num3 * num13;
			num3 -= num14 * 0.2f;
			num14 = num4 - num4 * num13;
			num4 -= num14 * 0.3f;
			num14 = num5 - num5 * num13;
			num5 -= num14 * 0.2f;
		}
		if (!entwicklungsbericht && specialMarketing[1] != 0)
		{
			num2 += (float)specialMarketing[1];
			num3 += (float)specialMarketing[1];
			num4 += (float)specialMarketing[1];
			num5 += (float)specialMarketing[1];
			num6 += (float)specialMarketing[1];
		}
		if (developerID == mS_.myID && mS_.GetFanGenreID() == maingenre)
		{
			num6 += 3f;
		}
		if (developerID == mS_.myID)
		{
			switch (mS_.difficulty)
			{
			case 0:
				num2 += 4f;
				num3 += 4f;
				num4 += 4f;
				num5 += 4f;
				num6 += 4f;
				break;
			case 1:
				num2 += 3f;
				num3 += 3f;
				num4 += 3f;
				num5 += 3f;
				num6 += 3f;
				break;
			case 2:
				num2 += 2f;
				num3 += 2f;
				num4 += 2f;
				num5 += 2f;
				num6 += 2f;
				break;
			case 3:
				num2 += 1f;
				num3 += 1f;
				num4 += 1f;
				num5 += 1f;
				num6 += 1f;
				break;
			case 4:
				num2 += 0.5f;
				num3 += 0.5f;
				num4 += 0.5f;
				num5 += 0.5f;
				num6 += 0.5f;
				break;
			case 5:
				num2 += 0f;
				num3 += 0f;
				num4 += 0f;
				num5 += 0f;
				num6 += 0f;
				break;
			}
		}
		if (!entwicklungsbericht && mS_.settings_RandomReviews)
		{
			num2 += (float)UnityEngine.Random.Range(-(mS_.settings_RandomReviewsNum * 3), mS_.settings_RandomReviewsNum * 3);
			num3 += (float)UnityEngine.Random.Range(-(mS_.settings_RandomReviewsNum * 3), mS_.settings_RandomReviewsNum * 3);
			num4 += (float)UnityEngine.Random.Range(-(mS_.settings_RandomReviewsNum * 3), mS_.settings_RandomReviewsNum * 3);
			num5 += (float)UnityEngine.Random.Range(-(mS_.settings_RandomReviewsNum * 3), mS_.settings_RandomReviewsNum * 3);
			num6 += (float)UnityEngine.Random.Range(-(mS_.settings_RandomReviewsNum * 3), mS_.settings_RandomReviewsNum * 3);
		}
		if (developerID != mS_.myID)
		{
			if (!devS_)
			{
				FindMyDeveloper();
			}
			if ((bool)devS_ && !devS_.IsTochterfirma())
			{
				switch (mS_.difficulty)
				{
				case 0:
					if (!entwicklungsbericht)
					{
						num3 -= (float)UnityEngine.Random.Range(5, 10);
						num4 -= (float)UnityEngine.Random.Range(5, 10);
						num5 -= (float)UnityEngine.Random.Range(5, 10);
						num6 -= (float)UnityEngine.Random.Range(5, 10);
					}
					else
					{
						num3 -= 7.5f;
						num4 -= 7.5f;
						num5 -= 7.5f;
						num6 -= 7.5f;
					}
					break;
				case 1:
					if (!entwicklungsbericht)
					{
						num3 -= (float)UnityEngine.Random.Range(0, 5);
						num4 -= (float)UnityEngine.Random.Range(0, 5);
						num5 -= (float)UnityEngine.Random.Range(0, 5);
						num6 -= (float)UnityEngine.Random.Range(0, 5);
					}
					else
					{
						num3 -= 2.5f;
						num4 -= 2.5f;
						num5 -= 2.5f;
						num6 -= 2.5f;
					}
					break;
				case 2:
					if (!entwicklungsbericht)
					{
						num3 -= (float)UnityEngine.Random.Range(0, 3);
						num4 -= (float)UnityEngine.Random.Range(0, 3);
						num5 -= (float)UnityEngine.Random.Range(0, 3);
						num6 -= (float)UnityEngine.Random.Range(0, 3);
					}
					else
					{
						num3 -= 1.5f;
						num4 -= 1.5f;
						num5 -= 1.5f;
						num6 -= 1.5f;
					}
					break;
				case 3:
					if (!entwicklungsbericht)
					{
						num2 += (float)UnityEngine.Random.Range(0, 8);
						num3 += (float)UnityEngine.Random.Range(0, 2);
						num4 += (float)UnityEngine.Random.Range(0, 2);
						num5 += (float)UnityEngine.Random.Range(0, 2);
						num6 += (float)UnityEngine.Random.Range(0, 4);
					}
					else
					{
						num2 += 4f;
						num3 += 1f;
						num4 += 1f;
						num5 += 1f;
						num6 += 2f;
					}
					break;
				case 4:
					if (!entwicklungsbericht)
					{
						num2 += (float)UnityEngine.Random.Range(0, 12);
						num3 += (float)UnityEngine.Random.Range(0, 4);
						num4 += (float)UnityEngine.Random.Range(0, 4);
						num5 += (float)UnityEngine.Random.Range(0, 4);
						num6 += (float)UnityEngine.Random.Range(0, 8);
					}
					else
					{
						num2 += 6f;
						num3 += 2f;
						num4 += 2f;
						num5 += 2f;
						num6 += 4f;
					}
					break;
				case 5:
					if (!entwicklungsbericht)
					{
						num2 += (float)UnityEngine.Random.Range(0, 16);
						num3 += (float)UnityEngine.Random.Range(0, 6);
						num4 += (float)UnityEngine.Random.Range(0, 6);
						num5 += (float)UnityEngine.Random.Range(0, 6);
						num6 += (float)UnityEngine.Random.Range(0, 12);
					}
					else
					{
						num2 += 8f;
						num3 += 3f;
						num4 += 3f;
						num5 += 3f;
						num6 += 6f;
					}
					break;
				}
			}
			if (sonderIP)
			{
				if (!entwicklungsbericht)
				{
					num2 = UnityEngine.Random.Range(sonderIPMindestreview - 5, sonderIPMindestreview + 5);
					num3 = UnityEngine.Random.Range(sonderIPMindestreview - 5, sonderIPMindestreview + 5);
					num4 = UnityEngine.Random.Range(sonderIPMindestreview - 5, sonderIPMindestreview + 5);
					num5 = UnityEngine.Random.Range(sonderIPMindestreview - 5, sonderIPMindestreview + 5);
					num6 = UnityEngine.Random.Range(sonderIPMindestreview - 5, sonderIPMindestreview + 5);
				}
				else
				{
					num2 = sonderIPMindestreview;
					num3 = sonderIPMindestreview;
					num4 = sonderIPMindestreview;
					num5 = sonderIPMindestreview;
					num6 = sonderIPMindestreview;
				}
				if (!devS_)
				{
					FindMyDeveloper();
				}
				if ((bool)devS_ && devS_.IsTochterfirma())
				{
					switch (devS_.tf_entwicklungsdauer)
					{
					case 0:
						num2 -= 10f;
						num3 -= 10f;
						num4 -= 10f;
						num5 -= 10f;
						num6 -= 10f;
						break;
					case 1:
						num2 -= 5f;
						num3 -= 5f;
						num4 -= 5f;
						num5 -= 5f;
						num6 -= 5f;
						break;
					}
				}
			}
			if (!sonderIP && mS_.settings_sandbox)
			{
				num2 *= mS_.sandbox_npcGameQuality;
				num3 *= mS_.sandbox_npcGameQuality;
				num4 *= mS_.sandbox_npcGameQuality;
				num5 *= mS_.sandbox_npcGameQuality;
				num6 *= mS_.sandbox_npcGameQuality;
			}
		}
		if (!entwicklungsbericht)
		{
			if (num6 >= 98f)
			{
				num6 = 98f;
				if (UnityEngine.Random.Range(0, 25) == 1)
				{
					num6 = 99f;
				}
				if (UnityEngine.Random.Range(0, 50) == 1)
				{
					num6 = 100f;
				}
			}
			if (num2 >= 98f)
			{
				num2 = 98f;
				if (UnityEngine.Random.Range(0, 10) == 1)
				{
					num2 = 99f;
				}
				if (UnityEngine.Random.Range(0, 25) == 1)
				{
					num2 = 100f;
				}
			}
			if (num3 >= 98f)
			{
				num3 = 98f;
				if (UnityEngine.Random.Range(0, 10) == 1)
				{
					num3 = 99f;
				}
				if (UnityEngine.Random.Range(0, 25) == 1)
				{
					num3 = 100f;
				}
			}
			if (num4 >= 98f)
			{
				num4 = 98f;
				if (UnityEngine.Random.Range(0, 10) == 1)
				{
					num4 = 99f;
				}
				if (UnityEngine.Random.Range(0, 25) == 1)
				{
					num4 = 100f;
				}
			}
			if (num5 >= 98f)
			{
				num5 = 98f;
				if (UnityEngine.Random.Range(0, 10) == 1)
				{
					num5 = 99f;
				}
				if (UnityEngine.Random.Range(0, 25) == 1)
				{
					num5 = 100f;
				}
			}
		}
		reviewGameplay = Mathf.RoundToInt(num2);
		reviewGrafik = Mathf.RoundToInt(num3);
		reviewSound = Mathf.RoundToInt(num4);
		reviewSteuerung = Mathf.RoundToInt(num5);
		reviewTotal = Mathf.RoundToInt(num6);
		if (reviewGameplay < 1)
		{
			reviewGameplay = 1;
		}
		if (reviewGrafik < 1)
		{
			reviewGrafik = 1;
		}
		if (reviewSound < 1)
		{
			reviewSound = 1;
		}
		if (reviewSteuerung < 1)
		{
			reviewSteuerung = 1;
		}
		if (reviewTotal < 1)
		{
			reviewTotal = 1;
		}
		if (reviewGameplay > 100)
		{
			reviewGameplay = 100;
		}
		if (reviewGrafik > 100)
		{
			reviewGrafik = 100;
		}
		if (reviewSound > 100)
		{
			reviewSound = 100;
		}
		if (reviewSteuerung > 100)
		{
			reviewSteuerung = 100;
		}
		if (reviewTotal > 100)
		{
			reviewTotal = 100;
		}
		if (developerID != mS_.myID)
		{
			if (!entwicklungsbericht)
			{
				if (reviewGameplay <= 1)
				{
					reviewGameplay = UnityEngine.Random.Range(2, 10);
				}
				if (reviewGrafik <= 1)
				{
					reviewGrafik = UnityEngine.Random.Range(2, 10);
				}
				if (reviewSound <= 1)
				{
					reviewSound = UnityEngine.Random.Range(2, 10);
				}
				if (reviewSteuerung <= 1)
				{
					reviewSteuerung = UnityEngine.Random.Range(2, 10);
				}
				if (reviewTotal <= 1)
				{
					reviewTotal = UnityEngine.Random.Range(2, 10);
				}
			}
			else
			{
				if (reviewGameplay <= 1)
				{
					reviewGameplay = 5;
				}
				if (reviewGrafik <= 1)
				{
					reviewGrafik = 5;
				}
				if (reviewSound <= 1)
				{
					reviewSound = 5;
				}
				if (reviewSteuerung <= 1)
				{
					reviewSteuerung = 5;
				}
				if (reviewTotal <= 1)
				{
					reviewTotal = 5;
				}
			}
		}
		if (!entwicklungsbericht && !typ_addon && !typ_addonStandalone && !typ_budget && !typ_bundle && !typ_mmoaddon && !typ_goty && !typ_bundleAddon)
		{
			if (!devS_)
			{
				FindMyDeveloper();
			}
			if (reviewTotal >= 80)
			{
				mS_.AddAwards(8, devS_);
			}
			if (reviewTotal < 30)
			{
				mS_.AddAwards(9, devS_);
			}
		}
	}

	public void SetPublisher(int id_)
	{
		pS_ = null;
		publisherID = id_;
	}

	public void AddIpPoints(float p)
	{
		if (p < 0f && mS_.settings_sandbox && mS_.sandbox_keinIpVerfall)
		{
			return;
		}
		if (mainIP != -1)
		{
			ipTime = 0;
			if (!script_mainIP)
			{
				FindMainIpScript();
			}
			if ((bool)script_mainIP && !typ_bundle && !typ_budget && !typ_bundleAddon && !typ_goty && !typ_contractGame)
			{
				script_mainIP.ipPunkte += p;
				script_mainIP.ipTime = 0;
				if (script_mainIP.ipPunkte < 0f)
				{
					script_mainIP.ipPunkte = 0f;
				}
				if (developerID == mS_.myID && (bool)mS_.achScript_ && script_mainIP.ipPunkte >= 990f)
				{
					mS_.achScript_.SetAchivement(57);
				}
				if (mS_.multiplayer)
				{
					if (mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_GameIpPoints(script_mainIP);
					}
					if (mS_.mpCalls_.isClient && developerID == mS_.myID)
					{
						mS_.mpCalls_.CLIENT_Send_GameIpPoints(script_mainIP);
					}
				}
			}
		}
		if (script_mainIP.ipPunkte < 0f)
		{
			script_mainIP.ipPunkte = 0f;
		}
	}

	public void AddEngineMarktdominanz()
	{
		if (!engineS_)
		{
			FindMyEngineNew();
		}
		if ((bool)engineS_)
		{
			float num = reviewTotal;
			num *= 0.05f;
			engineS_.AddMarktdominanz(1f + num);
		}
	}

	public void AddIpPointsRelease()
	{
		if (mainIP == -1)
		{
			return;
		}
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if (!script_mainIP || typ_bundle || typ_budget || typ_bundleAddon || typ_goty || typ_contractGame)
		{
			return;
		}
		float num = 0f;
		if (reviewTotal < 40)
		{
			num = reviewTotal;
			if (portID != -1)
			{
				AddIpPoints(0f - num * 0.4f);
			}
			else if (typ_nachfolger)
			{
				AddIpPoints(0f - num);
			}
			else if (typ_addon || typ_addonStandalone)
			{
				AddIpPoints(0f - num * 0.25f);
			}
			else if (typ_mmoaddon)
			{
				AddIpPoints(0f - num * 0.05f);
			}
			else if (typ_remaster)
			{
				AddIpPoints(0f - num * 0.5f);
			}
			else if (typ_spinoff)
			{
				AddIpPoints(0f - num * 0.9f);
			}
			else
			{
				AddIpPoints(0f - num);
			}
			return;
		}
		num = ((developerID != mS_.myID) ? ((float)(reviewTotal * 2 / 2)) : ((float)(reviewTotal * 2 / (mS_.difficulty + 1))));
		if (script_mainIP.developerID == mS_.myID)
		{
			if (script_mainIP.ipTime < 8)
			{
				num *= 0.2f;
			}
			else if ((typ_nachfolger || typ_remaster || typ_spinoff) && script_mainIP.ipTime < 30)
			{
				num *= 0.5f;
			}
		}
		if (portID != -1)
		{
			AddIpPoints(num * 0.4f);
		}
		else if (typ_nachfolger)
		{
			AddIpPoints(num);
		}
		else if (typ_addon || typ_addonStandalone)
		{
			AddIpPoints(num * 0.25f);
		}
		else if (typ_mmoaddon)
		{
			AddIpPoints(num * 0.05f);
		}
		else if (typ_remaster)
		{
			AddIpPoints(num * 0.5f);
		}
		else if (typ_spinoff)
		{
			AddIpPoints(num * 0.9f);
		}
		else
		{
			AddIpPoints(num);
		}
	}

	public void SetAsContractGameAngebot()
	{
		auftragsspiel = true;
		auftragsspiel_wochenAlsAngebot = 0;
		auftragsspiel_zeitAbgelaufen = false;
		auftragsspiel_Inivs = false;
		switch (gameSize)
		{
		case 0:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(1, 5) * 5;
			break;
		case 1:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(2, 7) * 5;
			break;
		case 2:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(6, 11) * 5;
			break;
		case 3:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(10, 15) * 5;
			break;
		case 4:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(13, 18) * 5;
			break;
		case 5:
			auftragsspiel_mindestbewertung = UnityEngine.Random.Range(15, 19) * 5;
			break;
		}
		auftragsspiel_zeitInWochen = 20 + gameSize * 20 + UnityEngine.Random.Range(5, 20);
		Menu_DevGame component = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		float num = 0f;
		num += (float)component.costs_gameSize[gameSize];
		num += (float)component.costs_gameTyp[0];
		switch (mS_.difficulty)
		{
		case 0:
			num *= 1.6f;
			break;
		case 1:
			num *= 1.4f;
			break;
		case 2:
			num *= 1.2f;
			break;
		case 3:
			num *= 1f;
			break;
		case 4:
			num *= 0.9f;
			break;
		case 5:
			num *= 0.8f;
			break;
		}
		float num2 = mS_.year - 1976;
		num2 /= 50f;
		if (num2 > 1f)
		{
			num2 = 1f;
		}
		num *= 1f + num2;
		auftragsspiel_gehalt = 10000 + Mathf.RoundToInt(num) / 100 * auftragsspiel_mindestbewertung;
		auftragsspiel_bonus = 100 * UnityEngine.Random.Range(0, 100) + Mathf.RoundToInt(num * (2.5f + mS_.auftragsAnsehen * 0.01f));
		auftragsspiel_gehalt += UnityEngine.Random.Range(0, 20) * 100;
		auftragsspiel_bonus += UnityEngine.Random.Range(0, 20) * 100;
		if (gameSize != 0)
		{
			auftragsspiel_gehalt += component.costs_gameSize[gameSize] * (mS_.difficulty + 1) / 2;
			auftragsspiel_bonus += component.costs_gameSize[gameSize] * (mS_.difficulty + 1) / 2;
		}
		else
		{
			auftragsspiel_gehalt += component.costs_gameSize[gameSize] / 2;
			auftragsspiel_bonus += component.costs_gameSize[gameSize] / 2;
		}
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			mS_.mpCalls_.SERVER_Send_Game(this);
		}
	}

	public void SetAsGameInDevelopmentNPC()
	{
		if (IGNORE_SetAsGameInDevelopmentNPC)
		{
			return;
		}
		inDevelopment = true;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(this);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(this);
			}
		}
	}

	public void SetAsPublishingAngebot()
	{
		if (!mS_)
		{
			FindScripts();
		}
		pubAngebot = true;
		pubAngebot_Weeks = 0;
		pubAngebot_Stimmung = 100f;
		pubAngebot_VerhandlungProzent = 100f;
		pubAngebot_Verhandlung = UnityEngine.Random.Range(2, 15);
		int num = reviewTotal;
		if (num <= 0)
		{
			CalcReview(entwicklungsbericht: true);
			num = reviewTotal;
			ClearReview();
		}
		if (!unlock_.Get(59))
		{
			pubAngebot_Retail = true;
			pubAngebot_Digital = false;
		}
		else
		{
			pubAngebot_Retail = true;
			pubAngebot_Digital = true;
			if (UnityEngine.Random.Range(0, 100) > 50)
			{
				pubAngebot_Retail = false;
				pubAngebot_Digital = true;
			}
			if (UnityEngine.Random.Range(0, 100) > 50)
			{
				pubAngebot_Retail = true;
				pubAngebot_Digital = false;
			}
			if (!HatEinePlattformInternet())
			{
				pubAngebot_Retail = true;
				pubAngebot_Digital = false;
			}
		}
		switch (gameSize)
		{
		case 0:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (2000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		case 1:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (4000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		case 2:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (8000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		case 3:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (16000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		case 4:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (32000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		case 5:
			pubAngebot_Garantiesumme = Mathf.RoundToInt(num) * (48000 * (mS_.difficulty + 1)) + UnityEngine.Random.Range(1, 100) * 1000;
			break;
		}
		pubAngebot_Gewinnbeteiligung = num / 3 + UnityEngine.Random.Range(1, 5 + mS_.difficulty);
		if (pubAngebot_Gewinnbeteiligung > 50f)
		{
			pubAngebot_Gewinnbeteiligung = 50f;
		}
		if (!pubAngebot_Retail && pubAngebot_Digital)
		{
			pubAngebot_Garantiesumme -= pubAngebot_Garantiesumme / 3;
		}
		if (pubAngebot_Retail && !pubAngebot_Digital)
		{
			pubAngebot_Garantiesumme -= pubAngebot_Garantiesumme / 4;
		}
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			mS_.mpCalls_.SERVER_Send_Game(this);
		}
	}

	public void SetOnMarket()
	{
		bool flag = false;
		inDevelopment = false;
		isOnMarket = true;
		schublade = false;
		pubAngebot = false;
		auftragsspiel = false;
		if (typ_contractGame && developerID == mS_.myID)
		{
			flag = true;
		}
		if ((bool)gameTab_ && (bool)gameTab_.gameObject)
		{
			UnityEngine.Object.Destroy(gameTab_.gameObject);
			gameTab_ = null;
		}
		InitUI();
		AddIpPointsRelease();
		AddEngineMarktdominanz();
		if (points_bugsInvis > 0f)
		{
			if (typ_bundle)
			{
				points_bugsInvis = 0f;
			}
			if (typ_bundleAddon)
			{
				points_bugsInvis = 0f;
			}
			if (typ_contractGame)
			{
				points_bugsInvis = 0f;
			}
			if (typ_budget)
			{
				points_bugsInvis = 0f;
			}
			if (typ_goty)
			{
				points_bugsInvis = 0f;
			}
		}
		if (gameTyp == 2)
		{
			retailVersion = false;
			digitalVersion = true;
			for (int i = 0; i < verkaufspreis.Length; i++)
			{
				verkaufspreis[i] = 0;
			}
		}
		if (handy)
		{
			retailVersion = false;
			digitalVersion = true;
		}
		if (arcade)
		{
			retailVersion = true;
			digitalVersion = false;
			for (int j = 1; j < verkaufspreis.Length; j++)
			{
				verkaufspreis[j] = 0;
			}
		}
		if (developerID == mS_.myID && points_bugs >= 50f)
		{
			mS_.sauerBugs = UnityEngine.Random.Range(4, 16);
		}
		bool flag2 = true;
		int num = 0;
		for (int k = 0; k < gamePlatform.Length; k++)
		{
			if (gamePlatform[k] == -1)
			{
				continue;
			}
			if (!gamePlatformScript[k])
			{
				FindMyPlatforms();
			}
			if (!gamePlatformScript[k])
			{
				continue;
			}
			num++;
			if (gamePlatformScript[k].ownerID != mS_.myID)
			{
				flag2 = false;
			}
			if (!typ_budget && !typ_addon && !typ_bundle && !typ_bundleAddon && !typ_goty && !typ_addonStandalone)
			{
				gamePlatformScript[k].games++;
				if (exklusiv)
				{
					gamePlatformScript[k].exklusivGames++;
				}
			}
		}
		if (IsMyGame() && !exklusiv && !retro && !herstellerExklusiv && !handy && !arcade)
		{
			if (num == 1)
			{
				exklusiv = true;
				if ((bool)gameTab_)
				{
					gameTab_.GetComponent<gameTab>().UpdateData();
				}
			}
			if (num > 1 && flag2)
			{
				herstellerExklusiv = true;
				if ((bool)gameTab_)
				{
					gameTab_.GetComponent<gameTab>().UpdateData();
				}
			}
		}
		if (typ_addon)
		{
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger)
			{
				script_vorgaenger.amountAddons++;
				float num2 = reviewTotal;
				script_vorgaenger.bonusSellsAddons += num2 * 0.01f / (float)script_vorgaenger.amountAddons;
			}
		}
		if (typ_addonStandalone)
		{
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger)
			{
				script_vorgaenger.amountAddons++;
			}
		}
		if (typ_mmoaddon)
		{
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger)
			{
				script_vorgaenger.amountMMOAddons++;
				script_vorgaenger.AddMMOInteresse(reviewTotal);
			}
		}
		if (IsMyGame())
		{
			if ((typ_standard || typ_nachfolger || typ_remaster || typ_spinoff) && !typ_contractGame && !typ_addon && !typ_addonStandalone && !typ_budget && !typ_bundle && !typ_bundleAddon && !typ_mmoaddon && !typ_goty && !arcade && portID == -1)
			{
				mS_.lastGamesGenre[0] = mS_.lastGamesGenre[1];
				mS_.lastGamesGenre[1] = mS_.lastGamesGenre[2];
				mS_.lastGamesGenre[2] = mS_.lastGamesGenre[3];
				mS_.lastGamesGenre[3] = mS_.lastGamesGenre[4];
				mS_.lastGamesGenre[4] = maingenre;
			}
			if (!typ_budget && !typ_bundle && !typ_bundleAddon && !typ_mmoaddon && !typ_goty)
			{
				mS_.lastSchlechteSpiele[0] = mS_.lastSchlechteSpiele[1];
				mS_.lastSchlechteSpiele[1] = mS_.lastSchlechteSpiele[2];
				mS_.lastSchlechteSpiele[2] = mS_.lastSchlechteSpiele[3];
				mS_.lastSchlechteSpiele[3] = mS_.lastSchlechteSpiele[4];
				mS_.lastSchlechteSpiele[4] = reviewTotal;
			}
			float num3 = 1f;
			if (typ_remaster)
			{
				num3 = 0.5f;
			}
			if (portID != -1)
			{
				num3 = 0.2f;
			}
			if (typ_addon || typ_addonStandalone || typ_mmoaddon)
			{
				num3 = 0.1f;
			}
			if (typ_budget || typ_bundle || typ_bundleAddon || typ_goty)
			{
				num3 = 0.05f;
			}
			if (pubOffer)
			{
				num3 = 0.2f;
			}
			if (reviewTotal < 10)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(1f * num3));
			}
			if (reviewTotal >= 10 && reviewTotal < 20)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(1f * num3));
			}
			if (reviewTotal >= 20 && reviewTotal < 30)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(2f * num3));
			}
			if (reviewTotal >= 30 && reviewTotal < 40)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(4f * num3));
			}
			if (reviewTotal >= 40 && reviewTotal < 50)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(6f * num3));
			}
			if (reviewTotal >= 50 && reviewTotal < 60)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(8f * num3));
			}
			if (reviewTotal >= 60 && reviewTotal < 70)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(10f * num3));
			}
			if (reviewTotal >= 70 && reviewTotal < 80)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(12f * num3));
			}
			if (reviewTotal >= 80 && reviewTotal < 90)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(15f * num3));
			}
			if (reviewTotal >= 90 && reviewTotal < 95)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(18f * num3));
			}
			if (reviewTotal >= 95 && reviewTotal < 100)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(25f * num3));
			}
			if (reviewTotal >= 100)
			{
				mS_.AddStudioPoints(Mathf.RoundToInt(50f * num3));
			}
		}
		if (IsMyGame() && (bool)mS_.achScript_)
		{
			if (developerID == mS_.myID)
			{
				if (reviewTotal >= 70)
				{
					if (maingenre == 0)
					{
						mS_.achScript_.SetAchivement(1);
					}
					if (maingenre == 1)
					{
						mS_.achScript_.SetAchivement(2);
					}
					if (maingenre == 2)
					{
						mS_.achScript_.SetAchivement(3);
					}
					if (maingenre == 3)
					{
						mS_.achScript_.SetAchivement(4);
					}
					if (maingenre == 4)
					{
						mS_.achScript_.SetAchivement(5);
					}
					if (maingenre == 5)
					{
						mS_.achScript_.SetAchivement(0);
					}
					if (maingenre == 6)
					{
						mS_.achScript_.SetAchivement(6);
					}
					if (maingenre == 7)
					{
						mS_.achScript_.SetAchivement(7);
					}
					if (maingenre == 8)
					{
						mS_.achScript_.SetAchivement(8);
					}
					if (maingenre == 9)
					{
						mS_.achScript_.SetAchivement(9);
					}
					if (maingenre == 10)
					{
						mS_.achScript_.SetAchivement(10);
					}
					if (maingenre == 11)
					{
						mS_.achScript_.SetAchivement(11);
					}
					if (maingenre == 12)
					{
						mS_.achScript_.SetAchivement(12);
					}
					if (maingenre == 13)
					{
						mS_.achScript_.SetAchivement(13);
					}
					if (maingenre == 14)
					{
						mS_.achScript_.SetAchivement(14);
					}
					if (maingenre == 15)
					{
						mS_.achScript_.SetAchivement(15);
					}
					if (maingenre == 16)
					{
						mS_.achScript_.SetAchivement(16);
					}
					if (maingenre == 17)
					{
						mS_.achScript_.SetAchivement(17);
					}
					if (maingenre == 18)
					{
						mS_.achScript_.SetAchivement(72);
					}
				}
				if (retro)
				{
					mS_.achScript_.SetAchivement(18);
				}
				if (gameTyp == 1)
				{
					mS_.achScript_.SetAchivement(19);
				}
				if (gameTyp == 2)
				{
					mS_.achScript_.SetAchivement(20);
				}
				if (arcade)
				{
					mS_.achScript_.SetAchivement(21);
				}
				if (handy)
				{
					mS_.achScript_.SetAchivement(22);
				}
				if (typ_spinoff)
				{
					mS_.achScript_.SetAchivement(30);
				}
				if (typ_nachfolger)
				{
					mS_.achScript_.SetAchivement(31);
				}
				if (portID != -1)
				{
					mS_.achScript_.SetAchivement(32);
				}
				if (typ_remaster)
				{
					mS_.achScript_.SetAchivement(33);
				}
				if (reviewTotal >= 80)
				{
					mS_.achScript_.SetAchivement(51);
				}
				if (reviewTotal >= 90)
				{
					mS_.achScript_.SetAchivement(52);
				}
				if (reviewTotal >= 100)
				{
					mS_.achScript_.SetAchivement(53);
				}
				if (publisherID == mS_.myID)
				{
					mS_.achScript_.SetAchivement(44);
				}
			}
			if (typ_bundle)
			{
				mS_.achScript_.SetAchivement(27);
			}
			if (typ_budget)
			{
				mS_.achScript_.SetAchivement(28);
			}
			if (typ_bundleAddon)
			{
				mS_.achScript_.SetAchivement(29);
			}
			if (pubOffer)
			{
				mS_.achScript_.SetAchivement(46);
			}
		}
		if (mS_.multiplayer && !DONT_SEND_GAME)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(this);
			}
			if (mS_.mpCalls_.isClient && (IsMyGame() || flag))
			{
				mS_.mpCalls_.CLIENT_Send_Game(this);
			}
		}
		if (reviewTotal <= 0)
		{
			Debug.Log("BUG -> 0% Review: " + GetNameSimple() + " / " + myID);
			Debug.Break();
		}
	}

	public bool IsRemovedFromMarket()
	{
		if (!isOnMarket && !inDevelopment && !schublade)
		{
			return true;
		}
		return false;
	}

	public void RemoveFromMarket()
	{
		FindScripts();
		isOnMarket = false;
		freigabeBudget = 48;
		if ((bool)gameTab_)
		{
			UnityEngine.Object.Destroy(gameTab_.gameObject);
		}
	}

	public bool IsZweitvermarktungAufMarkt()
	{
		FindScripts();
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i] || !games_.arrayGamesScripts[i].isOnMarket)
			{
				continue;
			}
			if (games_.arrayGamesScripts[i].typ_budget && games_.arrayGamesScripts[i].originalGameID == myID)
			{
				return true;
			}
			if (games_.arrayGamesScripts[i].typ_goty && games_.arrayGamesScripts[i].originalGameID == myID)
			{
				return true;
			}
			if (games_.arrayGamesScripts[i].typ_bundleAddon && games_.arrayGamesScripts[i].bundleID[0] == myID)
			{
				return true;
			}
			if (!games_.arrayGamesScripts[i].typ_bundle)
			{
				continue;
			}
			for (int j = 0; j < games_.arrayGamesScripts[i].bundleID.Length; j++)
			{
				if (games_.arrayGamesScripts[i].bundleID[j] != -1)
				{
					gameScript bundleGame = games_.arrayGamesScripts[i].GetBundleGame(j);
					if ((bool)bundleGame && bundleGame.originalGameID == myID)
					{
						return true;
					}
				}
			}
		}
		return false;
	}

	public void AutomaticSellLagerbestand()
	{
		if (mS_.sellLagerbestandAutomatic && publisherID == mS_.myID && !isOnMarket && !inDevelopment && !schublade && GetLagerbestand() > 0)
		{
			guiMain_.uiObjects[226].SetActive(value: true);
			guiMain_.uiObjects[226].GetComponent<Menu_W_Restbestand>().Init(this);
			guiMain_.uiObjects[226].GetComponent<Menu_W_Restbestand>().BUTTON_Yes();
			guiMain_.uiObjects[226].SetActive(value: false);
		}
	}

	public bool GetIpIsInArchiv()
	{
		if (mainIP == -1)
		{
			return false;
		}
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if ((bool)script_mainIP)
		{
			return script_mainIP.archiv_ip;
		}
		return false;
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

	public gameScript FindMainIpScript()
	{
		if ((bool)script_mainIP)
		{
			return script_mainIP;
		}
		if (mainIP == -1)
		{
			return null;
		}
		script_mainIP = FindGameScriptWithID(mainIP);
		if ((bool)script_mainIP)
		{
			return script_mainIP;
		}
		mainIP = -1;
		return null;
	}

	public gameScript FindPortOriginalScript()
	{
		if (originalGameID == -1)
		{
			return null;
		}
		if ((bool)script_portOriginal)
		{
			return script_portOriginal;
		}
		if (portID == -1)
		{
			return null;
		}
		script_portOriginal = FindGameScriptWithID(portID);
		if ((bool)script_portOriginal)
		{
			return script_portOriginal;
		}
		return null;
	}

	public gameScript FindVorgaengerScript()
	{
		if (originalGameID == -1)
		{
			return null;
		}
		if ((bool)script_vorgaenger)
		{
			return script_vorgaenger;
		}
		script_vorgaenger = FindGameScriptWithID(originalGameID);
		if ((bool)script_vorgaenger)
		{
			return script_vorgaenger;
		}
		return null;
	}

	public gameScript FindNachfolgerScript()
	{
		if (originalGameID == -1)
		{
			return null;
		}
		if ((bool)script_nachfolger)
		{
			return script_nachfolger;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].originalGameID == myID)
			{
				script_nachfolger = games_.arrayGamesScripts[i];
				return script_nachfolger;
			}
		}
		return null;
	}

	public int GetContractGameAbgabetermin()
	{
		if (!typ_contractGame)
		{
			return 0;
		}
		return auftragsspiel_zeitInWochen;
	}

	public void FreeGameContract()
	{
		auftragsspiel = true;
		developerID = publisherID;
		FindMyDeveloper();
		typ_contractGame = false;
		auftragsspiel_Inivs = true;
		if (mS_.multiplayer && (bool)mS_.mpCalls_)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_BlockContractGame(this, block_: false);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_BlockContractGame(this, block_: false);
			}
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(this);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(this);
			}
		}
	}

	public void SellMerchandise()
	{
		if (ownerID != mS_.myID || mainIP != myID || schublade || inDevelopment)
		{
			return;
		}
		if (!guiMain_)
		{
			FindScripts();
		}
		float num = Mathf.RoundToInt(GetIpBekanntheit());
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if (!script_mainIP)
		{
			return;
		}
		if (!menuFanshop_)
		{
			menuFanshop_ = guiMain_.uiObjects[367].GetComponent<Menu_Fanshop>();
		}
		if (merchVerkaufspreis[0] <= 0f)
		{
			for (int i = 0; i < merchVerkaufspreis.Length; i++)
			{
				merchVerkaufspreis[i] = mS_.merchStandardpreis[i];
			}
		}
		float num2 = ipPunkte;
		if (num2 > 1000f)
		{
			num2 = 1000f;
		}
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		float num3 = script_mainIP.merchGesamtReviewPoints + 30f;
		if ((double)num3 > 100.0)
		{
			num3 = 100f;
		}
		num2 = num2 * num3 * 0.01f;
		if (mS_.month == 12 || mS_.month == 1)
		{
			num2 *= 1.5f;
		}
		if (mS_.month == 6 || mS_.month == 7)
		{
			num2 *= 0.5f;
		}
		num2 = mS_.difficulty switch
		{
			0 => num2 * 1.5f, 
			1 => num2 * 1.3f, 
			2 => num2 * 1f, 
			3 => num2 * 0.8f, 
			4 => num2 * 0.6f, 
			5 => num2 * 0.4f, 
			_ => num2 * 1f, 
		};
		float num4 = mS_.genres_.GetAmountFans();
		for (int j = 0; j < merchBestellungen.Length; j++)
		{
			merchBestellungen[j] = 0;
			if (!(num >= (float)menuFanshop_.needStars[j]))
			{
				continue;
			}
			float num5 = num4 * 0.5f - (float)merchGesamtSells[j];
			if (num5 < 0f)
			{
				num5 = 0f;
			}
			if (num5 > 1000000f)
			{
				num5 = 1000000f;
			}
			num5 /= 10000f;
			float num6 = num2 * num5 * 0.01f;
			num6 *= menuFanshop_.beliebtheit[j];
			float num7 = menuFanshop_.optimalerPreis[j] - merchVerkaufspreis[j];
			if (num7 < 0f)
			{
				num7 = Mathf.Abs(num7);
				num7 = 0.07f * num7;
				if (num7 > 1f)
				{
					num7 = 0.99f;
				}
				num6 -= num6 * num7;
			}
			else
			{
				num7 = 0.03f * num7;
				num6 += num6 * num7;
			}
			num6 = UnityEngine.Random.Range(num6, num6 * 1.3f);
			num6 *= 0.4f;
			if (mS_.globalEvent == 13)
			{
				num6 *= 0.5f;
			}
			if ((float)merchGesamtSells[j] > num4)
			{
				num6 = 0f;
			}
			int num8 = Mathf.RoundToInt(num6);
			merchBestellungen[j] = num8;
		}
	}

	public void SellGame()
	{
		if (!isOnMarket)
		{
			if (!inDevelopment && freigabeBudget > 0)
			{
				freigabeBudget--;
			}
			return;
		}
		if (publisherID != -1 && !pS_)
		{
			FindMyPublisher();
		}
		if (releaseDate <= 0)
		{
			weeksOnMarket++;
		}
		else
		{
			releaseDate--;
		}
		float num = 0.07f;
		float num2 = 0f;
		float num3 = 0f;
		long num4 = sellsTotal;
		int num5 = reviewTotal;
		float num6 = 1f + GetUserReviewPercent() * 0.01f;
		if (num5 < 3)
		{
			num2 = 5f * num6;
		}
		if (num5 >= 3 && num5 < 5)
		{
			num2 = 30f * num6;
		}
		if (num5 >= 5 && num5 < 10)
		{
			num2 = 200f * num6;
		}
		if (num5 >= 10 && num5 < 20)
		{
			num2 = 300f * num6;
		}
		if (num5 >= 20 && num5 < 30)
		{
			num2 = 500f * num6;
		}
		if (num5 >= 30 && num5 < 40)
		{
			num2 = 1000f * num6;
		}
		if (num5 >= 40 && num5 < 50)
		{
			num2 = 2000f * num6;
		}
		if (num5 >= 50 && num5 < 60)
		{
			num2 = 5000f * num6;
		}
		if (num5 >= 60 && num5 < 70)
		{
			num2 = 8000f * num6;
		}
		if (num5 >= 70 && num5 < 80)
		{
			num2 = 13000f * num6;
		}
		if (num5 >= 80 && num5 < 90)
		{
			num2 = 19000f * num6;
		}
		if (num5 >= 90 && num5 < 95)
		{
			num2 = 25000f * num6;
		}
		if (num5 >= 95 && num5 < 100)
		{
			num2 = 40000f * num6;
		}
		if (num5 >= 100)
		{
			num2 = 100000f * num6;
		}
		num2 *= 0.5f;
		if (debug)
		{
			Debug.Log("GAME " + myName + " A " + num2);
		}
		float num7 = reviewTotal;
		num2 += points_gameplay * (num7 * 0.01f);
		num2 += points_grafik * (num7 * 0.01f);
		num2 += points_sound * (num7 * 0.01f);
		num2 += points_technik * (num7 * 0.01f);
		if (mainIP != -1)
		{
			if (!script_mainIP)
			{
				FindMainIpScript();
			}
			if ((bool)script_mainIP)
			{
				num2 = mS_.difficulty switch
				{
					0 => num2 + num2 * GetIpBekanntheit() * 0.2f, 
					1 => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.8f, 
					2 => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.5f, 
					3 => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.4f, 
					4 => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.37f, 
					5 => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.35f, 
					_ => num2 + num2 * GetIpBekanntheit() * 0.2f * 0.5f, 
				};
				if (script_mainIP.merchGesamtReviewPoints < (float)reviewTotal)
				{
					script_mainIP.merchGesamtReviewPoints = reviewTotal;
				}
			}
		}
		if (ExistAutomatenspiel())
		{
			num2 += num2 * 0.2f;
		}
		if (nachfolger_created)
		{
			if (debug)
			{
				Debug.Log("GAME " + myName + " B " + num2);
			}
			if (!script_nachfolger)
			{
				FindNachfolgerScript();
			}
			if ((bool)script_nachfolger && script_nachfolger.isOnMarket)
			{
				num2 *= 0.6f;
			}
		}
		if (typ_nachfolger)
		{
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger)
			{
				if (weeksOnMarket > 2)
				{
					if (script_vorgaenger.reviewTotal < 5)
					{
						num2 -= 1000f;
					}
					if (script_vorgaenger.reviewTotal >= 5 && script_vorgaenger.reviewTotal < 10)
					{
						num2 -= 500f;
					}
					if (script_vorgaenger.reviewTotal >= 10 && script_vorgaenger.reviewTotal < 20)
					{
						num2 -= 300f;
					}
					if (script_vorgaenger.reviewTotal >= 20 && script_vorgaenger.reviewTotal < 30)
					{
						num2 -= 100f;
					}
					if (script_vorgaenger.reviewTotal >= 30 && script_vorgaenger.reviewTotal < 40)
					{
						num2 -= 50f;
					}
				}
				if (script_vorgaenger.reviewTotal >= 40 && script_vorgaenger.reviewTotal < 50)
				{
					num2 += 50f;
				}
				if (script_vorgaenger.reviewTotal >= 50 && script_vorgaenger.reviewTotal < 60)
				{
					num2 += 1000f;
				}
				if (script_vorgaenger.reviewTotal >= 60 && script_vorgaenger.reviewTotal < 70)
				{
					num2 += 2000f;
				}
				if (script_vorgaenger.reviewTotal >= 70 && script_vorgaenger.reviewTotal < 80)
				{
					num2 += 3000f;
				}
				if (script_vorgaenger.reviewTotal >= 80 && script_vorgaenger.reviewTotal < 90)
				{
					num2 += 4000f;
				}
				if (script_vorgaenger.reviewTotal >= 90 && script_vorgaenger.reviewTotal < 95)
				{
					num2 += 5000f;
				}
				if (script_vorgaenger.reviewTotal >= 95 && script_vorgaenger.reviewTotal < 100)
				{
					num2 += 10000f;
				}
				if (script_vorgaenger.reviewTotal >= 100)
				{
					num2 += 15000f;
				}
			}
		}
		if (IsMyGame())
		{
			float num8 = (float)reviewTotal * 0.01f;
			float num9 = num8 * (float)genres_.GetAmountFans() * 0.005f;
			float num10 = num8 * (float)genres_.genres_FANS[maingenre] * 0.05f;
			float num11 = 0f;
			if (subgenre != -1)
			{
				num11 = num8 * (float)genres_.genres_FANS[subgenre] * 0.01f;
			}
			num2 += num9 + num10 + num11;
		}
		else
		{
			int num12 = mS_.year - 1975;
			float num13 = (float)reviewTotal * 0.01f;
			float num14 = num13 * (float)(50000 * num12) * 0.001f;
			float num15 = num13 * (float)(5000 * num12) * 0.01f;
			num2 += num14 + num15;
		}
		switch (gameTyp)
		{
		case 0:
			if (!arcade)
			{
				float num16 = 1f - (float)weeksOnMarket * 0.01f;
				num2 = ((!(num16 >= 0f)) ? (num2 * 0f) : (num2 * num16));
				num2 -= (float)(weeksOnMarket * weeksOnMarket * 3);
			}
			else
			{
				float num17 = 1f - (float)weeksOnMarket * 0.003f;
				num2 = ((!(num17 >= 0f)) ? (num2 * 0f) : (num2 * num17));
				num2 -= (float)(weeksOnMarket * weeksOnMarket);
			}
			break;
		case 1:
			num2 *= mmoInteresse * 0.01f;
			if (IsMyGame())
			{
				AddMMOInteresse(0f - UnityEngine.Random.Range(0.3f, 0.5f));
			}
			else
			{
				AddMMOInteresse(0f - UnityEngine.Random.Range(0.1f, 0.3f));
			}
			break;
		case 2:
			num2 *= f2pInteresse * 0.01f;
			if (IsMyGame())
			{
				AddF2PInteresse(0f - UnityEngine.Random.Range(0.3f, 0.5f));
			}
			else
			{
				AddF2PInteresse(0f - UnityEngine.Random.Range(0.1f, 0.3f));
			}
			break;
		}
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		num2 = (arcade ? (num2 * games_.GetSellsArcade()) : (num2 * games_.GetSells()));
		if (gameTyp == 1 || gameTyp == 2 || typ_mmoaddon)
		{
			num2 *= games_.GetInternetUser();
		}
		switch (weeksOnMarket)
		{
		case 1:
			num2 *= 1.3f;
			break;
		case 2:
			num2 *= 1.4f;
			break;
		case 3:
			num2 *= 1.5f;
			break;
		case 4:
			num2 *= 1.4f;
			break;
		case 5:
			num2 *= 1.1f;
			break;
		}
		if (IsMyGame())
		{
			if (typ_nachfolger || typ_remaster || typ_spinoff || typ_standard)
			{
				if (!handy && !arcade)
				{
					float num18 = mS_.GetAchivementBonus(0);
					num18 *= 0.01f;
					num3 += num18;
				}
				if (arcade)
				{
					float num19 = mS_.GetAchivementBonus(1);
					num19 *= 0.01f;
					num3 += num19;
				}
				if (handy)
				{
					float num20 = mS_.GetAchivementBonus(2);
					num20 *= 0.01f;
					num3 += num20;
				}
			}
			if (typ_addon || typ_addonStandalone || typ_mmoaddon)
			{
				float num21 = mS_.GetAchivementBonus(3);
				num21 *= 0.01f;
				num3 += num21;
			}
			if (typ_budget || typ_bundle || typ_bundleAddon || typ_goty)
			{
				float num22 = mS_.GetAchivementBonus(4);
				num22 *= 0.01f;
				num3 += num22;
			}
		}
		switch (usk)
		{
		case 0:
			num3 += 0.1f;
			break;
		case 1:
			num3 += 0.1f;
			break;
		case 2:
			num3 += 0.05f;
			break;
		case 3:
			num3 += 0f;
			break;
		case 4:
			num3 -= 0.05f;
			break;
		case 5:
			num3 -= 0.2f;
			break;
		}
		if (!gameLanguage[0])
		{
			num3 -= 0.05f;
		}
		if (!gameLanguage[1])
		{
			num3 -= 0.03f;
		}
		if (!gameLanguage[2])
		{
			num3 -= 0.03f;
		}
		if (!gameLanguage[3])
		{
			num3 -= 0.02f;
		}
		if (!gameLanguage[4])
		{
			num3 -= 0.02f;
		}
		if (!gameLanguage[5])
		{
			num3 -= 0.02f;
		}
		if (!gameLanguage[6])
		{
			num3 -= 0.01f;
		}
		if (!gameLanguage[7])
		{
			num3 -= 0.02f;
		}
		if (!gameLanguage[8])
		{
			num3 -= 0.02f;
		}
		if (!gameLanguage[9])
		{
			num3 -= 0.03f;
		}
		if (!gameLanguage[10])
		{
			num3 -= 0.04f;
		}
		if (!typ_bundle)
		{
			if (mS_.trendGenre == maingenre)
			{
				num3 += 0.33f;
			}
			if (mS_.trendTheme == gameMainTheme)
			{
				num3 += 0.15f;
			}
			if (mS_.trendAntiGenre == maingenre)
			{
				num3 -= 0.33f;
			}
			if (mS_.trendAntiTheme == gameMainTheme)
			{
				num3 -= 0.15f;
			}
			if (mS_.trendGenre == subgenre)
			{
				num3 += 0.1f;
			}
			if (mS_.trendTheme == gameSubTheme)
			{
				num3 += 0.05f;
			}
			if (mS_.trendAntiGenre == subgenre)
			{
				num3 -= 0.1f;
			}
			if (mS_.trendAntiTheme == gameSubTheme)
			{
				num3 -= 0.05f;
			}
		}
		num3 += GetHype() * 0.01f;
		if (!typ_bundle && !arcade)
		{
			if (gameCopyProtect != -1)
			{
				if (!gameCopyProtectScript_)
				{
					GameObject gameObject = GameObject.Find("COPYPROTECT_" + gameCopyProtect);
					if ((bool)gameObject)
					{
						gameCopyProtectScript_ = gameObject.GetComponent<copyProtectScript>();
					}
					else
					{
						gameCopyProtect = -1;
					}
				}
				if ((bool)gameCopyProtectScript_)
				{
					num3 += gameCopyProtectScript_.effekt * 0.002f;
				}
			}
			if (gameAntiCheat != -1 && (gameplayFeatures_DevDone[21] || gameplayFeatures_DevDone[23]))
			{
				if (!gameAntiCheatScript_)
				{
					GameObject gameObject2 = GameObject.Find("ANTICHEAT_" + gameAntiCheat);
					if ((bool)gameObject2)
					{
						gameAntiCheatScript_ = gameObject2.GetComponent<antiCheatScript>();
					}
					else
					{
						gameAntiCheat = -1;
					}
				}
				if ((bool)gameAntiCheatScript_)
				{
					num3 += gameAntiCheatScript_.effekt * 0.003f;
				}
			}
		}
		if (publisherID != mS_.myID && (bool)pS_)
		{
			if (maingenre == pS_.fanGenre)
			{
				num3 += 0.2f;
			}
			num3 += pS_.stars * 0.01f;
		}
		if (!arcade)
		{
			if (mS_.month == 12 || mS_.month == 1)
			{
				num3 += 0.5f;
			}
			if (mS_.month == 6 || mS_.month == 7)
			{
				num3 -= 0.3f;
			}
		}
		if (IsMyGame() && mS_.awardBonus > 0 && mS_.awardBonusAmount > 0f)
		{
			num3 += mS_.awardBonusAmount;
		}
		num3 = (arcade ? (num3 + bonusSellsUpdates * 0.2f) : (num3 + bonusSellsUpdates));
		num3 += bonusSellsAddons;
		num3 += addonQuality;
		if (num3 < -0.5f)
		{
			num3 = -0.5f;
		}
		num2 *= 1f + num3;
		if (!arcade)
		{
			float num23 = 10f;
			if (exklusiv)
			{
				num23 = 25f;
			}
			for (int i = 0; i < gamePlatform.Length; i++)
			{
				if (gamePlatform[i] != -1)
				{
					if (!gamePlatformScript[i])
					{
						FindMyPlatforms();
					}
					if ((bool)gamePlatformScript[i])
					{
						num23 += gamePlatformScript[i].GetMarktanteil() * genres_.GetFloatPlatformSells(maingenre, gamePlatformScript[i].typ);
					}
				}
			}
			num23 *= 0.007f;
			num2 *= num23;
		}
		else
		{
			num2 *= genres_.GetFloatPlatformSells(maingenre, 4);
		}
		if (IsMyGame())
		{
			if (!typ_bundle && !arcade && mS_.gelangweiltGenre != -1)
			{
				if (maingenre == mS_.gelangweiltGenre)
				{
					num2 *= 0.5f;
				}
				else if (subgenre == mS_.gelangweiltGenre)
				{
					num2 *= 0.85f;
				}
			}
			if (mS_.sauerBugs > 0)
			{
				num2 *= 0.7f;
			}
			if (mS_.schlechteSpiele > 0)
			{
				num2 *= 0.6f;
			}
			if (!typ_bundle && !typ_addon && !typ_mmoaddon)
			{
				Vector4 amountGamesWithGenreAndTopic = games_.GetAmountGamesWithGenreAndTopic(this);
				float num24 = 0.00055555557f * (float)genres_.genres_LEVEL.Length;
				float num25 = 3.3333334E-05f * (float)themes_.themes_LEVEL.Length;
				float num26 = amountGamesWithGenreAndTopic.x * num24 + amountGamesWithGenreAndTopic.y * num25 + amountGamesWithGenreAndTopic.z * num24 * 2.5f + amountGamesWithGenreAndTopic.w * num25 * 2.5f;
				switch (mS_.settings_competition)
				{
				case 1:
					num26 *= 2f;
					break;
				case 2:
					num26 *= 3f;
					break;
				}
				switch (mS_.difficulty)
				{
				case 0:
					if (num26 > 0.5f)
					{
						num26 = 0.5f;
					}
					break;
				case 1:
					if (num26 > 0.55f)
					{
						num26 = 0.55f;
					}
					break;
				case 2:
					if (num26 > 0.6f)
					{
						num26 = 0.6f;
					}
					break;
				case 3:
					if (num26 > 0.65f)
					{
						num26 = 0.65f;
					}
					break;
				case 4:
					if (num26 > 0.7f)
					{
						num26 = 0.7f;
					}
					break;
				case 5:
					if (num26 > 0.75f)
					{
						num26 = 0.75f;
					}
					break;
				}
				num2 *= 1f - num26;
			}
		}
		if (gameLicence != -1)
		{
			if (licences_.licence_GENREGOOD[gameLicence] == maingenre)
			{
				num2 += num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.8f);
			}
			if (licences_.licence_GENREGOOD[gameLicence] != maingenre && licences_.licence_GENREBAD[gameLicence] != maingenre)
			{
				num2 += num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.3f);
			}
			if (licences_.licence_GENREBAD[gameLicence] == maingenre)
			{
				num2 -= num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.3f);
			}
			if (subgenre != -1)
			{
				if (licences_.licence_GENREGOOD[gameLicence] == subgenre)
				{
					num2 += num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.2f);
				}
				if (licences_.licence_GENREGOOD[gameLicence] != subgenre && licences_.licence_GENREBAD[gameLicence] != subgenre)
				{
					num2 += num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.05f);
				}
				if (licences_.licence_GENREBAD[gameLicence] == subgenre)
				{
					num2 -= num2 * (licences_.licence_QUALITY[gameLicence] * 0.01f * 0.1f);
				}
			}
		}
		float num27 = genres_.genres_BELIEBTHEIT[maingenre];
		if (maingenre == mS_.trendGenre)
		{
			num27 = 100f;
		}
		if (maingenre == mS_.trendAntiGenre)
		{
			num27 = 20f;
		}
		float num28 = num2 * 0.5f * (num27 * 0.01f);
		num2 *= 0.8f;
		num2 += num28;
		if (!mS_.settings_sabotageOff && mS_.sabotage_erwischt > 0)
		{
			num2 *= 0.5f;
		}
		if (!mS_.settings_sabotageOff && mS_.sabotage_geruecht > 0)
		{
			num2 *= 0.8f;
		}
		if (mS_.globalEvent == 0)
		{
			num2 *= 0.5f;
		}
		if (mS_.globalEvent == 1)
		{
			num2 *= 1.5f;
		}
		if (newGenreCombination)
		{
			num2 *= 1.1f;
		}
		if (newTopicCombination)
		{
			num2 *= 1.03f;
		}
		if (commercialFlop)
		{
			if (reviewTotal >= 70 && !typ_bundle && !typ_bundleAddon && mS_.trendGenre != maingenre)
			{
				num2 *= 0.1f;
			}
			else
			{
				commercialFlop = false;
			}
		}
		if (commercialHit)
		{
			if (reviewTotal >= 70 && reviewTotal < 90)
			{
				num2 *= 2f;
			}
			else
			{
				commercialHit = false;
			}
		}
		switch (mS_.difficulty)
		{
		case 0:
			num2 *= 2f;
			break;
		case 1:
			num2 *= 1.5f;
			break;
		case 2:
			num2 *= 1f;
			break;
		case 3:
			num2 *= 0.5f;
			break;
		case 4:
			num2 *= 0.35f;
			break;
		case 5:
			num2 *= 0.25f;
			break;
		}
		if (IsMyGame() && publisherID == mS_.myID)
		{
			switch (mS_.GetStudioLevel(mS_.studioPoints))
			{
			case 0:
				num2 *= 0.5f;
				break;
			case 1:
				num2 *= 0.55f;
				break;
			case 2:
				num2 *= 0.6f;
				break;
			case 3:
				num2 *= 0.65f;
				break;
			case 4:
				num2 *= 0.7f;
				break;
			case 5:
				num2 *= 0.75f;
				break;
			case 6:
				num2 *= 0.8f;
				break;
			case 7:
				num2 *= 0.85f;
				break;
			case 8:
				num2 *= 0.9f;
				break;
			case 9:
				num2 *= 0.95f;
				break;
			}
		}
		if (mS_.settings_sandbox && IsMyGame() && mS_.sandbox_gameSells > 0f)
		{
			num2 *= mS_.sandbox_gameSells;
		}
		num2 *= num;
		if (typ_addon)
		{
			num2 *= 0.4f;
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger && num2 > 0f)
			{
				if (script_vorgaenger.amountAddons > 0)
				{
					num2 /= (float)script_vorgaenger.amountAddons;
				}
				if (!script_vorgaenger.isOnMarket && script_vorgaenger.publisherID != mS_.myID)
				{
					num2 *= 0.8f;
				}
				if ((float)sellsTotal + num2 + (float)vorbestellungen > (float)script_vorgaenger.sellsTotal)
				{
					num2 = script_vorgaenger.sellsTotal - (sellsTotal + vorbestellungen);
				}
				if (num2 <= 0f)
				{
					num2 = 1f;
				}
				if (!script_vorgaenger.isOnMarket && (float)sellsTotal + num2 > (float)script_vorgaenger.sellsTotal)
				{
					num2 = 0f;
				}
			}
		}
		if (typ_addonStandalone)
		{
			num2 *= 0.5f;
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger && script_vorgaenger.amountAddons > 0)
			{
				num2 /= (float)script_vorgaenger.amountAddons;
			}
		}
		if (typ_mmoaddon)
		{
			num2 *= 0.65f;
			if (!script_vorgaenger)
			{
				FindVorgaengerScript();
			}
			if ((bool)script_vorgaenger && num2 > 0f)
			{
				script_vorgaenger.abosAddons = Mathf.RoundToInt(num2);
				if ((float)sellsTotal + num2 + (float)vorbestellungen > (float)script_vorgaenger.sellsTotal)
				{
					num2 = script_vorgaenger.sellsTotal - (sellsTotal + vorbestellungen);
				}
				if (num2 <= 0f)
				{
					num2 = 1f;
				}
			}
		}
		if (gameTyp == 1)
		{
			if (IsMyGame() && games_.freeServerPlatz <= 0)
			{
				num2 *= 0.05f;
			}
			int amountOfMMOs = games_.GetAmountOfMMOs();
			float num29 = 1f + (float)amountOfMMOs * 0.1f;
			if (amountOfMMOs > 0)
			{
				num2 /= num29;
			}
			float num30 = UnityEngine.Random.Range((num2 + (float)abosAddons) * 0.5f, (num2 + (float)abosAddons) * 0.7f);
			abosAddons = 0;
			float num31 = (float)abonnements - (float)abonnements / 102f * (float)reviewTotal;
			num31 *= 0.25f;
			num31 += (float)weeksOnMarket;
			if (IsMyGame())
			{
				num31 += num31 * ((100f - hype) * 0.01f);
			}
			switch (aboPreis)
			{
			case 1:
				num30 *= 1f;
				break;
			case 2:
				num30 *= 0.95f;
				break;
			case 3:
				num30 *= 0.9f;
				break;
			case 4:
				num30 *= 0.8f;
				break;
			case 5:
				num30 *= 0.7f;
				break;
			case 6:
				num30 *= 0.65f;
				break;
			case 7:
				num30 *= 0.6f;
				break;
			case 8:
				num30 *= 0.5f;
				break;
			case 9:
				num30 *= 0.4f;
				break;
			case 10:
				num30 *= 0.2f;
				break;
			}
			if (amountOfMMOs > 0)
			{
				num30 /= num29;
			}
			num30 = ((!IsMyGame()) ? (num30 * 1.1f) : (num30 * 0.7f));
			abonnementsWoche = abonnements;
			abonnements -= Mathf.RoundToInt(num31);
			switch (aboPreis - aboPreisOld)
			{
			case 1:
				abonnements -= abonnements / 10;
				break;
			case 2:
				abonnements -= abonnements / 9;
				break;
			case 3:
				abonnements -= abonnements / 8;
				break;
			case 4:
				abonnements -= abonnements / 7;
				break;
			case 5:
				abonnements -= abonnements / 6;
				break;
			case 6:
				abonnements -= abonnements / 5;
				break;
			case 7:
				abonnements -= abonnements / 4;
				break;
			case 8:
				abonnements -= abonnements / 3;
				break;
			case 9:
				abonnements -= abonnements / 2;
				break;
			case 10:
				abonnements -= abonnements / 2;
				break;
			}
			aboPreisOld = aboPreis;
			abonnements += Mathf.RoundToInt(num30);
			if (abonnements > sellsTotal)
			{
				abonnements = sellsTotal;
			}
			if (IsMyGame())
			{
				long num32 = abonnements;
				for (int j = 0; j < mS_.arrayRoomScripts.Length; j++)
				{
					if ((bool)mS_.arrayRoomScripts[j] && mS_.arrayRoomScripts[j].typ == 15 && (mS_.arrayRoomScripts[j].serverReservieren == 0 || mS_.arrayRoomScripts[j].serverReservieren == 1))
					{
						num32 = mS_.arrayRoomScripts[j].SetAbos(num32);
						if (num32 <= 0)
						{
							break;
						}
					}
				}
				abonnements -= num32;
				mS_.AddAboverlauf(abonnements);
			}
			abonnementsWoche = abonnements - abonnementsWoche;
			if (abonnements < 0)
			{
				abonnements = 0L;
			}
			if (bestAbonnements < abonnements)
			{
				bestAbonnements = abonnements;
			}
		}
		if (handy && gameTyp == 0)
		{
			num2 *= 2.5f;
		}
		if (arcade)
		{
			if (IsMyGame())
			{
				float num33 = arcadeCase + arcadeMonitor + arcadeJoystick + arcadeSound;
				num33 = 1f + num33 * 0.05f;
				num2 *= num33;
			}
			else
			{
				float num34 = arcadeCase + arcadeMonitor + arcadeJoystick + arcadeSound;
				num34 = 1f + num34 * 0.05f;
				num2 *= num34;
				num2 *= 0.3f;
			}
			num2 *= 0.005f;
			if (num2 < 1f && !IsMyGame() && weeksOnMarket < 2)
			{
				num2 = UnityEngine.Random.Range(1, 4);
			}
		}
		if (gameTyp == 2)
		{
			int amountOfF2Ps = games_.GetAmountOfF2Ps();
			float num35 = 1f + (float)amountOfF2Ps * 0.1f;
			num2 *= 4f;
			if (amountOfF2Ps > 0)
			{
				num2 /= num35;
			}
			float num36 = UnityEngine.Random.Range((num2 + (float)abosAddons) * 0.3f, (num2 + (float)abosAddons) * 0.5f);
			abosAddons = 0;
			float num37 = (float)abonnements - (float)abonnements / 102f * (float)reviewTotal;
			num37 *= 0.25f;
			num37 += (float)weeksOnMarket;
			if (amountOfF2Ps > 0)
			{
				num36 /= (float)amountOfF2Ps;
			}
			abonnementsWoche = abonnements;
			abonnements -= Mathf.RoundToInt(num37);
			abonnements += Mathf.RoundToInt(num36);
			if (abonnements > sellsTotal)
			{
				abonnements = sellsTotal;
			}
			if (IsMyGame())
			{
				long num38 = abonnements;
				for (int k = 0; k < mS_.arrayRoomScripts.Length; k++)
				{
					if ((bool)mS_.arrayRoomScripts[k] && (bool)mS_.arrayRoomScripts[k] && mS_.arrayRoomScripts[k].typ == 15 && (mS_.arrayRoomScripts[k].serverReservieren == 0 || mS_.arrayRoomScripts[k].serverReservieren == 2))
					{
						num38 = mS_.arrayRoomScripts[k].SetAbos(num38);
						if (num38 <= 0)
						{
							break;
						}
					}
				}
				abonnements -= num38;
			}
			abonnementsWoche = abonnements - abonnementsWoche;
			if (abonnements < 0)
			{
				abonnements = 0L;
				abonnementsWoche = 0L;
			}
			if (bestAbonnements < abonnements)
			{
				bestAbonnements = abonnements;
			}
		}
		if (typ_budget)
		{
			num2 *= 0.5f;
			float num39 = mS_.year * (date_start_year - 1);
			num39 *= 0.05f;
			if (num39 > 0.7f)
			{
				num39 = 0.7f;
			}
			num39 = 1f - num39;
			num2 *= num39;
		}
		if (typ_remaster)
		{
			num2 *= 0.7f;
		}
		if (typ_goty)
		{
			num2 *= 0.5f;
			float num40 = mS_.year * (date_start_year - 1);
			num40 *= 0.03f;
			if (num40 > 0.7f)
			{
				num40 = 0.7f;
			}
			num40 = 1f - num40;
			num2 *= num40;
		}
		if (typ_bundle)
		{
			num2 *= 0.2f;
			float num41 = 0.4f;
			if (bundleID[0] != -1)
			{
				num41 += 0.1f;
			}
			if (bundleID[1] != -1)
			{
				num41 += 0.1f;
			}
			if (bundleID[2] != -1)
			{
				num41 += 0.1f;
			}
			if (bundleID[3] != -1)
			{
				num41 += 0.1f;
			}
			if (bundleID[4] != -1)
			{
				num41 += 0.1f;
			}
			num2 *= num41;
		}
		if (typ_bundleAddon)
		{
			float num42 = mS_.year * (date_start_year - 1);
			num42 *= 0.05f;
			if (num42 > 0.7f)
			{
				num42 = 0.7f;
			}
			num42 = 1f - num42;
			num2 *= num42;
			float num43 = 0.5f;
			if (bundleID[0] != -1)
			{
				num43 += 0.05f;
			}
			if (bundleID[1] != -1)
			{
				num43 += 0.05f;
			}
			if (bundleID[2] != -1)
			{
				num43 += 0.05f;
			}
			if (bundleID[3] != -1)
			{
				num43 += 0.05f;
			}
			if (bundleID[4] != -1)
			{
				num43 += 0.05f;
			}
			num2 *= num43;
		}
		if (!arcade)
		{
			long num44 = 0L;
			if (num2 > 0f)
			{
				for (int l = 0; l < gamePlatform.Length; l++)
				{
					if (gamePlatform[l] == -1)
					{
						continue;
					}
					if (!gamePlatformScript[l])
					{
						FindMyPlatforms();
					}
					if (!gamePlatformScript[l])
					{
						continue;
					}
					if (exklusiv && gamePlatformScript[l].OwnerIsNPC() && !gamePlatformScript[l].vomMarktGenommen)
					{
						if (gameTyp != 2)
						{
							int num45 = Mathf.RoundToInt(UnityEngine.Random.Range((float)sellsPerWeek[0] * 0.2f, (float)sellsPerWeek[0] * 0.3f));
							num45 = num45 / 100 * (130 - (int)gamePlatformScript[l].GetMarktanteil());
							if (num45 > sellsPerWeek[0])
							{
								num45 = sellsPerWeek[0];
							}
							exklusivKonsolenSells += num45;
							gamePlatformScript[l].BonusSellsExklusiv(num45);
						}
						else
						{
							int num46 = Mathf.RoundToInt(UnityEngine.Random.Range((float)sellsPerWeek[0] * 0.2f, (float)sellsPerWeek[0] * 0.3f));
							num46 = num46 / 100 * (130 - (int)gamePlatformScript[l].GetMarktanteil());
							if (num46 > sellsPerWeek[0])
							{
								num46 = sellsPerWeek[0];
							}
							exklusivKonsolenSells += num46 / 5;
							gamePlatformScript[l].BonusSellsExklusiv(num46);
						}
					}
					num44 += gamePlatformScript[l].units;
				}
				if ((float)(sellsTotal + vorbestellungen) + num2 > (float)(num44 / 2))
				{
					long num47 = num44 - (sellsTotal + vorbestellungen + Mathf.RoundToInt(num2));
					if (num47 / 10 > sellsTotal)
					{
						sellsTotal = num47 / 10;
					}
				}
				if ((float)(sellsTotal + vorbestellungen) + num2 > (float)num44)
				{
					num2 = num44 - (sellsTotal + vorbestellungen);
					if (num2 <= 0f)
					{
						num2 = 1f;
					}
				}
			}
		}
		if (gameTyp == 0 && sellsPerWeek[0] > 0 && num2 > 0f)
		{
			float num48 = 0.06f;
			if (reviewTotal >= 0 && reviewTotal < 5 && sellsTotal > 10000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 5 && reviewTotal < 10 && sellsTotal > 15000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 10 && reviewTotal < 15 && sellsTotal > 20000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 15 && reviewTotal < 20 && sellsTotal > 25000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 20 && reviewTotal < 25 && sellsTotal > 30000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 25 && reviewTotal < 30 && sellsTotal > 35000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 35 && reviewTotal < 40 && sellsTotal > 50000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 40 && reviewTotal < 45 && sellsTotal > 60000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 45 && reviewTotal < 50 && sellsTotal > 80000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 50 && reviewTotal < 55 && sellsTotal > 150000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 55 && reviewTotal < 60 && sellsTotal > 200000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 60 && reviewTotal < 65 && sellsTotal > 300000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 65 && reviewTotal < 70 && sellsTotal > 500000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 70 && reviewTotal < 75 && sellsTotal > 1000000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 75 && reviewTotal < 80 && sellsTotal > 2000000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (reviewTotal >= 80 && reviewTotal < 85 && sellsTotal > 4000000 / (mS_.difficulty + 1))
			{
				realsticPower += num48;
			}
			if (realsticPower > 0f)
			{
				if (realsticPower > 0.98f)
				{
					realsticPower = 0.98f;
				}
				num2 *= 1f - realsticPower;
			}
			if (num2 < 0f)
			{
				num2 = 0f;
			}
		}
		if (HasInAppPurchases())
		{
			if (gameTyp == 0 && releaseDate <= 0)
			{
				float num49 = num2 * (GetInAppPurchaseHate() * 0.01f) * 0.3f;
				num2 -= num49;
				float inAppPurchaseMoneyPerWeek = GetInAppPurchaseMoneyPerWeek();
				float num50 = UnityEngine.Random.Range(sellsTotal / 100 * 2, sellsTotal / 100 * 3);
				if (IsMyGame())
				{
					float num51 = mS_.GetAchivementBonus(5);
					num51 *= 0.01f;
					num50 += num50 * num51;
				}
				if (num2 <= 0f)
				{
					num50 *= 0.6f;
				}
				float num52 = 1f - (float)weeksOnMarket * 0.01f;
				if (num52 < 0.1f)
				{
					num52 = 0.1f;
				}
				num50 *= num52;
				if (weeksOnMarket > 5)
				{
					num50 -= (float)(weeksOnMarket * 30);
				}
				if (num50 < 0f)
				{
					num50 = 0f;
				}
				if (num50 > 2E+09f)
				{
					num50 = 2000000000 - UnityEngine.Random.Range(0, 100000);
				}
				inAppPurchaseWeek = Mathf.RoundToInt(num50);
				long num53 = (long)(inAppPurchaseMoneyPerWeek * (float)Mathf.RoundToInt(num50));
				umsatzTotal += num53;
				umsatzInApp += num53;
				if (IsMyGame())
				{
					mS_.Earn(num53, 8);
				}
				if (IsMyGame())
				{
					PayGewinnbeteiligung(num53);
				}
				if (!IsMyGame())
				{
					AddTochterfirmaUmsatz(num53);
				}
			}
			if (gameTyp == 1 && releaseDate <= 0)
			{
				float num54 = num2 * (GetInAppPurchaseHate() * 0.01f) * 0.3f;
				num2 -= num54;
				float inAppPurchaseMoneyPerWeek2 = GetInAppPurchaseMoneyPerWeek();
				float num55 = UnityEngine.Random.Range(abonnements / 100 * 4, abonnements / 100 * 5);
				if (IsMyGame())
				{
					float num56 = mS_.GetAchivementBonus(5);
					num56 *= 0.01f;
					num55 += num55 * num56;
				}
				if (num2 <= 0f)
				{
					num55 *= 0.8f;
				}
				if (num55 < 0f)
				{
					num55 = 0f;
				}
				if (num55 > 2E+09f)
				{
					num55 = 2000000000 - UnityEngine.Random.Range(0, 100000);
				}
				inAppPurchaseWeek = Mathf.RoundToInt(num55);
				long num57 = (long)(inAppPurchaseMoneyPerWeek2 * (float)Mathf.RoundToInt(num55));
				umsatzTotal += num57;
				umsatzInApp += num57;
				if (IsMyGame())
				{
					mS_.Earn(num57, 8);
				}
				if (IsMyGame())
				{
					PayGewinnbeteiligung(num57);
				}
				if (!IsMyGame())
				{
					AddTochterfirmaUmsatz(num57);
				}
			}
			if (gameTyp == 2 && releaseDate <= 0)
			{
				float num58 = num2 * (GetInAppPurchaseHate() * 0.01f) * 0.3f;
				num2 -= num58;
				float inAppPurchaseMoneyPerWeek3 = GetInAppPurchaseMoneyPerWeek();
				float num59 = UnityEngine.Random.Range(((float)abonnements + num2) / 100f * 150f, ((float)abonnements + num2) / 100f * 200f);
				if (IsMyGame())
				{
					float num60 = mS_.GetAchivementBonus(5);
					num60 *= 0.01f;
					num59 += num59 * num60;
				}
				if (num2 <= 0f)
				{
					num59 *= 0.8f;
				}
				if (num59 < 0f)
				{
					num59 = 0f;
				}
				if (num59 > 2E+09f)
				{
					num59 = 2000000000 - UnityEngine.Random.Range(0, 100000);
				}
				inAppPurchaseWeek = Mathf.RoundToInt(num59);
				long num61 = (long)(inAppPurchaseMoneyPerWeek3 * (float)Mathf.RoundToInt(num59));
				umsatzTotal += num61;
				umsatzInApp += num61;
				if (IsMyGame())
				{
					mS_.Earn(num61, 8);
				}
				if (IsMyGame())
				{
					PayGewinnbeteiligung(num61);
				}
				if (!IsMyGame())
				{
					AddTochterfirmaUmsatz(num61);
				}
			}
		}
		if (inGamePass)
		{
			float num62 = 0f;
			if (num2 > 0f)
			{
				long num63 = 0L;
				long num64 = 0L;
				for (int m = 0; m < gamePlatformScript.Length; m++)
				{
					if ((bool)gamePlatformScript[m])
					{
						num63 += gamePlatformScript[m].GetAktiveNutzer();
						if (gamePlatformScript[m].inGamePass || gamePlatformScript[m].inGamePassPassiv)
						{
							num64 += gamePlatformScript[m].GetAktiveNutzer();
						}
					}
				}
				if (num63 > 100)
				{
					long num65 = num63 / 100;
					num65 = num64 / num65;
					num62 = num2 * ((float)num65 * 0.01f);
					num62 *= games_.GetGamePassInteressted();
					num62 *= 0.33f;
					switch (gpS_.gamePass_AboPreis)
					{
					case 2:
						num62 *= 0.95f;
						break;
					case 3:
						num62 *= 0.9f;
						break;
					case 4:
						num62 *= 0.85f;
						break;
					case 5:
						num62 *= 0.75f;
						break;
					case 6:
						num62 *= 0.65f;
						break;
					case 7:
						num62 *= 0.5f;
						break;
					case 8:
						num62 *= 0.4f;
						break;
					case 9:
						num62 *= 0.3f;
						break;
					case 10:
						num62 *= 0.2f;
						break;
					}
					num2 -= num62;
				}
			}
			if (GetVorbestellungen() > 500)
			{
				int num66 = UnityEngine.Random.Range(1, GetVorbestellungen() / 33);
				vorbestellungen -= num66;
				num62 += (float)num66;
			}
			gamePassPlayer = Mathf.RoundToInt(num62) + UnityEngine.Random.Range(1, 10);
			gpS_.gamePass_AbosLetzteWoche += Mathf.RoundToInt(num62);
		}
		int num67 = Mathf.RoundToInt(num2);
		if (num67 < 0)
		{
			num67 = 0;
		}
		if ((IsMyGame() || (typ_contractGame && developerID == mS_.myID)) && releaseDate <= 0 && !typ_bundle)
		{
			AddFans(Mathf.RoundToInt(num2));
		}
		float num68 = 0f;
		float num69 = 0f;
		float num70 = 0f;
		float num71 = 0f;
		if (releaseDate <= 0)
		{
			for (int num72 = sellsPerWeek.Length - 1; num72 >= 1; num72--)
			{
				sellsPerWeek[num72] = sellsPerWeek[num72 - 1];
				sellsPerWeekOnline[num72] = sellsPerWeekOnline[num72 - 1];
			}
			if (publisherID != mS_.myID)
			{
				num67 = Mathf.RoundToInt((float)num67 * GetPreisAbzug(0));
				sellsPerWeek[0] = num67;
				sellsPerWeekOnline[0] = games_.GetDigitalSells() * UnityEngine.Random.Range(80f, 100f);
				sellsTotal += num67;
			}
			else if (IsMyGame())
			{
				if (gameTyp != 2)
				{
					if (!arcade)
					{
						float digitalSells = games_.GetDigitalSells();
						if (digitalVersion)
						{
							num71 = (float)num67 * digitalSells * GetPreisAbzug(3);
							if (!retailVersion)
							{
								num71 += (float)num67 * 0.2f * GetPreisAbzug(3);
							}
						}
						if (retailVersion)
						{
							num68 = (float)num67 * (1f - digitalSells) * GetPreisAbzug(0);
							num68 += num68 * GetEditionQualiaet(0);
							num69 = (float)num67 * games_.GetDeluxeCurve() * GetPreisAbzug(1) * GetEditionQualiaet(1);
							num68 -= num69;
							num70 = (float)num67 * games_.GetCollectorsCurve() * GetPreisAbzug(2) * GetEditionQualiaet(2);
							num70 *= 0.5f;
							num68 -= num70;
							if (!digitalVersion)
							{
								num68 += (float)num67 * 0.2f * GetPreisAbzug(0);
							}
							if (lagerbestand[1] <= 0)
							{
								num68 += num69;
								num69 = 0f;
							}
							if (lagerbestand[2] <= 0)
							{
								num68 += num70;
								num70 = 0f;
							}
							if (num68 < 0f)
							{
								num68 = 0f;
							}
						}
						sellsStandard_forProduction = Mathf.RoundToInt(num68);
						num68 += (float)vorbestellungen;
						vorbestellungen = 0;
						if (retailVersion)
						{
							num71 = Mathf.RoundToInt(num71);
							num68 = Mathf.RoundToInt(num68);
							num69 = Mathf.RoundToInt(num69);
							num70 = Mathf.RoundToInt(num70);
							if ((float)lagerbestand[0] < num68)
							{
								vorbestellungen += Mathf.RoundToInt(num68 - (float)lagerbestand[0]);
								num68 = lagerbestand[0];
							}
							lagerbestand[0] -= Mathf.RoundToInt(num68);
							if ((float)lagerbestand[1] < num69)
							{
								num69 = lagerbestand[1];
							}
							lagerbestand[1] -= Mathf.RoundToInt(num69);
							if ((float)lagerbestand[2] < num70)
							{
								num70 = lagerbestand[2];
							}
							lagerbestand[2] -= Mathf.RoundToInt(num70);
						}
						sellsPerWeek[0] = Mathf.RoundToInt(num71 + num68 + num69 + num70);
						sellsPerWeekOnline[0] = 100f / (num71 + num68 + num69 + num70) * num71;
						sellsTotal += Mathf.RoundToInt(num71 + num68 + num69 + num70);
						sellsTotalStandard += Mathf.RoundToInt(num68);
						sellsTotalDeluxe += Mathf.RoundToInt(num69);
						sellsTotalCollectors += Mathf.RoundToInt(num70);
						sellsTotalOnline += Mathf.RoundToInt(num71);
						if (vorbestellungen > 0 && releaseDate <= 0 && num68 <= 0f)
						{
							vorbestellungen -= UnityEngine.Random.Range(0, vorbestellungen / 50 + 3);
							vorbestellungen -= weeksOnMarket * 2;
							if (vorbestellungen < 0)
							{
								vorbestellungen = 0;
							}
						}
					}
					else
					{
						num68 = (float)num67 * GetPreisAbzug(0);
						sellsPerWeek[0] = Mathf.RoundToInt(num68);
						sellsPerWeekOnline[0] = 0f;
						vorbestellungen += Mathf.RoundToInt(num68);
						if (vorbestellungen > 50)
						{
							stornierungen = UnityEngine.Random.Range(0, vorbestellungen / 50 + 3);
							vorbestellungen -= stornierungen;
						}
						else
						{
							stornierungen = 0;
							if (weeksOnMarket > 20 && vorbestellungen > 0)
							{
								stornierungen = 1;
								vorbestellungen--;
							}
						}
					}
				}
				else
				{
					num71 = num67;
					sellsPerWeek[0] = Mathf.RoundToInt(num71);
					sellsPerWeekOnline[0] = 100f;
					sellsTotal += Mathf.RoundToInt(num71);
					sellsTotalOnline += Mathf.RoundToInt(num71);
				}
			}
		}
		else if (retailVersion)
		{
			vorbestellungen += Mathf.RoundToInt((float)num67 * GetPreisAbzug(0) / (float)(releaseDate + 1));
		}
		if (IsMyGame())
		{
			if (hype > 0f && releaseDate <= 0)
			{
				AddHype(0f - UnityEngine.Random.Range(0.1f, 1f));
			}
		}
		else
		{
			hype = 100f;
		}
		if (releaseDate <= 0 && ((sellsPerWeek[0] > 100 && !arcade) || sellsTotal > 100 || (arcade && sellsTotal > 0)) && !typ_budget && !typ_goty)
		{
			float num73 = 0f;
			if (!arcade)
			{
				num73 = sellsPerWeek[0];
				num73 = UnityEngine.Random.Range(num73 * 0.01f, num73 * 0.02f);
			}
			else
			{
				num73 = sellsTotal;
				num73 = UnityEngine.Random.Range(num73 * 0.01f, num73 * 0.02f) + (float)UnityEngine.Random.Range(0, 5);
			}
			float num74 = 0f;
			switch (UnityEngine.Random.Range(0, 5))
			{
			case 0:
				num74 = num73 * (float)reviewGameplay / 100f;
				break;
			case 1:
				num74 = num73 * (float)reviewGrafik / 100f;
				break;
			case 2:
				num74 = num73 * (float)reviewSound / 100f;
				break;
			case 3:
				num74 = num73 * (float)reviewSteuerung / 100f;
				break;
			case 4:
				num74 = num73 * (float)reviewTotal / 100f;
				break;
			}
			num74 -= UnityEngine.Random.Range(0f, points_bugs);
			if (num74 < 0f)
			{
				num74 = 0f;
			}
			if (!mS_.settings_sabotageOff && mS_.sabotage_reviews > 0)
			{
				num74 /= 2f;
			}
			userPositiv += Mathf.RoundToInt(num74);
			userNegativ += Mathf.RoundToInt(num73 - num74);
		}
		if (gameTyp != 2 && !arcade && !typ_addon && !typ_addonStandalone && !typ_mmoaddon && releaseDate <= 0)
		{
			if (!devS_)
			{
				FindMyDeveloper();
			}
			if (!pS_)
			{
				FindMyPublisher();
			}
			if (num4 < 1000000 && sellsTotal >= 1000000)
			{
				mS_.AddAwards(7, devS_);
				if (publisherID != developerID)
				{
					mS_.AddAwards(7, pS_);
				}
				if (IsMyGame() || developerID == mS_.myID)
				{
					guiMain_.CreateTopNewsGoldeneSchallplatte(GetNameWithTag());
					mS_.goldeneSchallplatten++;
				}
			}
			if (num4 < 5000000 && sellsTotal >= 5000000)
			{
				mS_.AddAwards(10, devS_);
				if (publisherID != developerID)
				{
					mS_.AddAwards(10, pS_);
				}
				if (IsMyGame() || developerID == mS_.myID)
				{
					guiMain_.CreateTopNewsPlatinSchallplatte(GetNameWithTag());
					mS_.platinSchallplatten++;
				}
			}
			if (num4 < 10000000 && sellsTotal >= 10000000)
			{
				mS_.AddAwards(11, devS_);
				if (publisherID != developerID)
				{
					mS_.AddAwards(11, pS_);
				}
				if (IsMyGame() || developerID == mS_.myID)
				{
					guiMain_.CreateTopNewsDiamantSchallplatte(GetNameWithTag());
					mS_.diamantSchallplatten++;
				}
			}
		}
		if (IsMyGame())
		{
			if (releaseDate <= 0 && (bool)mS_.achScript_ && gameTyp != 2)
			{
				if (sellsTotal >= 1000000)
				{
					mS_.achScript_.SetAchivement(48);
				}
				if (sellsTotal >= 10000000)
				{
					mS_.achScript_.SetAchivement(49);
				}
				if (sellsTotal >= 50000000)
				{
					mS_.achScript_.SetAchivement(50);
				}
			}
			UpdateFanletter();
			if (!typ_addon && !typ_mmoaddon && !arcade)
			{
				float maxInclusive = (float)num67 * 0.001f + points_bugs;
				maxInclusive = UnityEngine.Random.Range(0f, maxInclusive);
				if (gameTyp == 2)
				{
					maxInclusive /= 5f;
				}
				mS_.AddAnrufe(Mathf.RoundToInt(maxInclusive));
			}
			if (publisherID != mS_.myID)
			{
				if ((bool)pS_)
				{
					mS_.AddVerkaufsverlauf(num67);
					float num75 = 0f;
					num75 = ((mS_.exklusivVertrag_ID != publisherID) ? ((float)(num67 * pS_.GetShare())) : ((float)(num67 * pS_.GetShareExklusiv())));
					int num76 = Mathf.RoundToInt(num75);
					umsatzTotal += num76;
					mS_.Earn(num76, 3);
					PayGewinnbeteiligung(num76);
					long num77 = 0L;
					if (gameTyp == 1 && mS_.week == 5)
					{
						num77 = abonnements * aboPreis;
						umsatzTotal += num77;
						umsatzAbos += num77;
						mS_.Earn(num77, 7);
						PayGewinnbeteiligung(num77);
						costs_server += abonnements / 10;
					}
					PlayerPayEngineLicence(num76 + num77);
					if (hype < 50f && (UnityEngine.Random.Range(0f, 100f + pS_.stars) > 90f || weeksOnMarket <= 1))
					{
						AddHype(UnityEngine.Random.Range(15f, pS_.stars + 15f));
						if (hype < 0f)
						{
							hype = 0f;
						}
						if (hype > 100f)
						{
							hype = 100f;
						}
						string text = tS_.GetText(495);
						text = text.Replace("<NAME1>", GetNameWithTag());
						guiMain_.CreateTopNewsInfo(text);
					}
				}
			}
			else if (!arcade)
			{
				mS_.AddVerkaufsverlauf(Mathf.RoundToInt(num71 + num68 + num69 + num70));
				if (num71 > 0f)
				{
					mS_.AddDownloadverlauf(Mathf.RoundToInt(num71));
				}
				long num78 = 0L;
				if (gameTyp != 2)
				{
					num78 = Convert.ToInt64(num71 * (float)verkaufspreis[3]) + Convert.ToInt64(num68 * (float)verkaufspreis[0]) + Convert.ToInt64(num69 * (float)verkaufspreis[1]) + Convert.ToInt64(num70 * (float)verkaufspreis[2]);
					umsatzTotal += num78;
					mS_.Earn(num78, 3);
					PayGewinnbeteiligung(num78);
				}
				long num79 = 0L;
				if (gameTyp == 1 && mS_.week == 5)
				{
					num79 = abonnements * aboPreis;
					umsatzTotal += num79;
					umsatzAbos += num79;
					mS_.Earn(num79, 7);
					PayGewinnbeteiligung(num79);
					costs_server += abonnements / 10;
				}
				num79 = 0L;
				if (gameTyp == 2 && mS_.week == 5)
				{
					costs_server += abonnements / 10;
				}
				PlayerPayEngineLicence(num78 + num79);
				if (autoPreis && !arcade && !handy)
				{
					UpdateAutoPreis();
				}
			}
			if ((bool)gameTab_)
			{
				gameTab_.UpdateData();
			}
			if ((publisherID != mS_.myID && num67 <= 0 && abonnements <= 0) || (publisherID == mS_.myID && mS_.automatic_RemoveGameFormMarket && releaseDate <= 0 && weeksOnMarket > 4 && vorbestellungen <= 0 && sellsPerWeek[0] <= mS_.automatic_RemoveGameFormMarket_Amount && abonnements <= 0))
			{
				if (!arcade || (arcade && vorbestellungen <= 0))
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[82]);
					guiMain_.uiObjects[82].GetComponent<Menu_GameFromMarket>().Init(this, selbstVomMarktGenommen: false);
					guiMain_.OpenMenu(hideChars: false);
					RemoveFromMarket();
				}
			}
			else
			{
				if (sellsTotal > 0 && weeksOnMarket < 24 && !guiMain_.menuOpen && reviewTotal > 90 && !trendsetter && releaseDate <= 0 && mS_.trendGenre != maingenre && !typ_mmoaddon && !typ_addon && !typ_budget && !typ_bundle && !typ_addonStandalone && !typ_goty && !typ_bundleAddon && UnityEngine.Random.Range(0, 200) == 1)
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[142]);
					guiMain_.uiObjects[142].GetComponent<Menu_Trendsetter>().Init(this);
					guiMain_.OpenMenu(hideChars: false);
					mS_.award_Trendsetter++;
					AddHype(30f);
					AddIpPoints(70f);
					if ((bool)mS_.achScript_)
					{
						mS_.achScript_.SetAchivement(34);
					}
				}
				if (sellsTotal > 0 && commercialFlop && weeksOnMarket == 4 && !guiMain_.menuOpen)
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[352]);
					guiMain_.uiObjects[352].GetComponent<Menu_RandomEventCommercialFlop>().Init(this);
				}
				if (sellsTotal > 0 && commercialHit && weeksOnMarket == 4 && !guiMain_.menuOpen)
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[354]);
					guiMain_.uiObjects[354].GetComponent<Menu_RandomEventCommercialHit>().Init(this);
				}
				if (sellsTotal > 0 && points_bugsInvis > 0f && mS_.difficulty >= 2 && weeksOnMarket >= 4 && weeksOnMarket <= 20 && UnityEngine.Random.Range(0, 200) <= mS_.difficulty && !guiMain_.menuOpen)
				{
					guiMain_.ActivateMenu(guiMain_.uiObjects[353]);
					guiMain_.uiObjects[353].GetComponent<Menu_RandomEventBugs>().Init(this);
				}
			}
		}
		else
		{
			if (gameTyp != 2)
			{
				float f = 0f;
				if (!handy && !arcade)
				{
					if (publisherID != developerID)
					{
						if ((bool)pS_)
						{
							f = (float)num67 * pS_.share;
						}
					}
					else
					{
						f = num67 * verkaufspreis[0];
					}
				}
				if (handy)
				{
					f = num67 * 3;
				}
				if (arcade)
				{
					f = num67 * verkaufspreis[0];
				}
				int num80 = Mathf.RoundToInt(f);
				umsatzTotal += num80;
				AddTochterfirmaUmsatz(num80);
			}
			if (gameTyp == 1 && mS_.week == 5)
			{
				long num81 = abonnements * aboPreis;
				umsatzTotal += num81;
				umsatzAbos += num81;
				AddTochterfirmaUmsatz(num81);
				costs_server += abonnements / 10;
			}
			if (gameTyp == 2 && mS_.week == 5)
			{
				costs_server += abonnements / 10;
			}
			if (!engineS_)
			{
				FindMyEngineNew();
			}
			if ((bool)engineS_ && engineS_.ownerID == mS_.myID && reviewTotal > 50)
			{
				AddFans(num67);
			}
			if ((num67 <= 0 && abonnements < 10) || (gameTyp == 2 && abonnements < 10 && weeksOnMarket > 5))
			{
				if (!typ_bundle)
				{
					FindMyEngineNew();
					if ((bool)engineS_)
					{
						if (engineS_.ownerID == mS_.myID)
						{
							if ((bool)guiMain_)
							{
								guiMain_.OpenEngineAbrechnung(this);
							}
						}
						else if (mS_.multiplayer && engineS_.EngineFromMitspieler() && mS_.mpCalls_.isServer)
						{
							mS_.mpCalls_.SERVER_Send_EngineAbrechnung(engineS_.ownerID, myID);
						}
						if (GetPublisherOrDeveloperIsTochterfirma() && (bool)guiMain_)
						{
							guiMain_.OpenTochterfirmaAbrechnung(this);
						}
					}
				}
				RemoveFromMarket();
			}
		}
		if (typ_mmoaddon)
		{
			gameScript gameScript2 = FindVorgaengerScript();
			if ((bool)gameScript2 && !gameScript2.isOnMarket)
			{
				RemoveFromMarket();
			}
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer && (IsMyGame() || typ_contractGame || (DeveloperIsNPC() && PublisherIsNPC() && OwnerIsNPC())))
			{
				mS_.mpCalls_.SERVER_Send_GameSell(this);
			}
			if (mS_.mpCalls_.isClient && IsMyGame())
			{
				mS_.mpCalls_.CLIENT_Send_GameSell(this);
			}
		}
	}

	public void PlayerPayEngineLicence(long e)
	{
		if (typ_bundle)
		{
			return;
		}
		FindMyEngineNew();
		if (!engineS_ || engineS_.ownerID == mS_.myID || engineGewinnbeteiligung <= 0)
		{
			return;
		}
		long num = e * engineGewinnbeteiligung / 100;
		if (num <= 0)
		{
			return;
		}
		costs_enginegebuehren += num;
		mS_.Pay(num, 11);
		if (engineS_.ownerID != -1)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, engineS_.ownerID, 0, (int)num, engineS_.myID);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Payment(engineS_.ownerID, 0, (int)num, engineS_.myID);
			}
		}
	}

	public void FindMyPlatforms()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] != -1)
			{
				gamePlatformScript[i] = FindPlatformScriptWithID(gamePlatform[i]);
				if (!gamePlatformScript[i])
				{
					gamePlatform[i] = -1;
					gamePlatformScript[i] = null;
				}
			}
		}
	}

	private platformScript FindPlatformScriptWithID(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == id_)
			{
				return mS_.arrayPlatformsScripts[i];
			}
		}
		return null;
	}

	public void FindMyPublisher()
	{
		if (publisherID == -1)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == publisherID)
			{
				pS_ = mS_.arrayPublisherScripts[i];
				break;
			}
		}
	}

	public void FindMyOwner()
	{
		if (ownerID == -1)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == ownerID)
			{
				ownerS_ = mS_.arrayPublisherScripts[i];
				break;
			}
		}
	}

	public void FindMyDeveloper()
	{
		if (developerID == -1)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == developerID)
			{
				devS_ = mS_.arrayPublisherScripts[i];
				break;
			}
		}
	}

	public void FindMyEngineNew()
	{
		if (engineID == -1)
		{
			engineS_ = null;
			return;
		}
		if ((bool)engineS_)
		{
			if (engineID == engineS_.myID)
			{
				return;
			}
			engineS_ = null;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (!mS_)
		{
			return;
		}
		for (int i = 0; i < mS_.arrayEnginesScripts.Length; i++)
		{
			if ((bool)mS_.arrayEnginesScripts[i] && mS_.arrayEnginesScripts[i].myID == engineID)
			{
				engineS_ = mS_.arrayEnginesScripts[i];
				return;
			}
		}
		engineID = 0;
	}

	public void AddF2PInteresse(float f)
	{
		f2pInteresse += f;
		if (f2pInteresse > 100f)
		{
			f2pInteresse = 100f;
		}
		if (weeksOnMarket > 52)
		{
			_ = weeksOnMarket;
			if (f2pInteresse > 100f - GetF2PAbnutzung())
			{
				f2pInteresse = 100f - GetF2PAbnutzung();
			}
		}
		if (f2pInteresse < 0f)
		{
			f2pInteresse = 0f;
		}
	}

	public void AddMMOInteresse(float f)
	{
		mmoInteresse += f;
		if (mmoInteresse > 100f)
		{
			mmoInteresse = 100f;
		}
		if (weeksOnMarket > 52)
		{
			_ = weeksOnMarket;
			if (mmoInteresse > 100f - GetMMOAbnutzung())
			{
				mmoInteresse = 100f - GetMMOAbnutzung();
			}
		}
		if (mmoInteresse < 0f)
		{
			mmoInteresse = 0f;
		}
	}

	public float GetMMOAbnutzung()
	{
		if (weeksOnMarket > 52)
		{
			return (float)(weeksOnMarket - 51) / 15f;
		}
		return 0f;
	}

	public float GetF2PAbnutzung()
	{
		if (weeksOnMarket > 52)
		{
			return (float)(weeksOnMarket - 51) / 10f;
		}
		return 0f;
	}

	public void AddHype(float f)
	{
		if (hype <= 100f)
		{
			hype += f;
			if (hype > 100f)
			{
				hype = 100f;
			}
		}
		if (hype > 100f && f < 0f)
		{
			hype += f;
		}
		if (hype < 0f)
		{
			hype = 0f;
		}
	}

	public float GetHype()
	{
		return hype;
	}

	public float GetIpBekanntheit()
	{
		if (mainIP == -1)
		{
			return 0f;
		}
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if (!script_mainIP)
		{
			mainIP = -1;
			return 0f;
		}
		float num = script_mainIP.ipPunkte;
		if (num > 1000f)
		{
			num = 1000f;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		return num / 200f;
	}

	public long GetIpWert()
	{
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if (!script_mainIP)
		{
			return 0L;
		}
		float num = script_mainIP.ipPunkte;
		if (num > 1000f)
		{
			num = 1000f;
		}
		if (num < 0f)
		{
			num = 0f;
		}
		float num2 = 0f;
		if (num > 0f && num <= 50f)
		{
			num2 = (int)num * 250;
		}
		if (num > 50f && num <= 100f)
		{
			num2 = (int)num * 500;
		}
		if (num > 100f && num <= 150f)
		{
			num2 = (int)num * 1000;
		}
		if (num > 150f && num <= 200f)
		{
			num2 = (int)num * 2000;
		}
		if (num > 200f && num <= 250f)
		{
			num2 = (int)num * 3000;
		}
		if (num > 250f && num <= 300f)
		{
			num2 = (int)num * 4000;
		}
		if (num > 300f && num <= 350f)
		{
			num2 = (int)num * 6000;
		}
		if (num > 350f && num <= 400f)
		{
			num2 = (int)num * 10000;
		}
		if (num > 400f && num <= 450f)
		{
			num2 = (int)num * 15000;
		}
		if (num > 450f && num <= 500f)
		{
			num2 = (int)num * 20000;
		}
		if (num > 500f && num <= 600f)
		{
			num2 = (int)num * 30000;
		}
		if (num > 600f && num <= 700f)
		{
			num2 = (int)num * 40000;
		}
		if (num > 700f && num <= 800f)
		{
			num2 = (int)num * 55000;
		}
		if (num > 800f && num <= 900f)
		{
			num2 = (int)num * 70000;
		}
		if (num > 900f)
		{
			num2 = (int)num * 100000;
		}
		if (!mS_)
		{
			FindScripts();
		}
		float num3 = mS_.PassedMonth();
		if (num3 > 600f)
		{
			num3 = 600f;
		}
		num2 = num2 * 0.1f * (num3 * 0.035f);
		if (num2 > 200000000f)
		{
			num2 = 200000000f;
		}
		return Mathf.RoundToInt(num2);
	}

	public int GetHypeNachfolger()
	{
		if (reviewTotal < 30)
		{
			return 1;
		}
		if (reviewTotal >= 30 && reviewTotal < 40)
		{
			return 5;
		}
		if (reviewTotal >= 40 && reviewTotal < 50)
		{
			return 10;
		}
		if (reviewTotal >= 50 && reviewTotal < 60)
		{
			return 15;
		}
		if (reviewTotal >= 60 && reviewTotal < 70)
		{
			return 25;
		}
		if (reviewTotal >= 70 && reviewTotal < 80)
		{
			return 35;
		}
		if (reviewTotal >= 80 && reviewTotal < 90)
		{
			return 40;
		}
		if (reviewTotal >= 90 && reviewTotal < 98)
		{
			return 50;
		}
		if (reviewTotal >= 98)
		{
			return 60;
		}
		return 1;
	}

	public int GetHypeSpinoff()
	{
		int num = -1;
		float num2 = 0f;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i])
			{
				continue;
			}
			gameScript gameScript2 = games_.arrayGamesScripts[i];
			if ((bool)gameScript2 && gameScript2.mainIP == mainIP && !gameScript2.typ_budget && !gameScript2.typ_goty && !gameScript2.typ_bundle && !gameScript2.typ_bundleAddon && !gameScript2.typ_addon)
			{
				float num3 = gameScript2.date_month;
				num3 /= 13f;
				num3 = (float)gameScript2.date_year + num3;
				if (num2 < num3)
				{
					num2 = num3;
					num = i;
				}
			}
		}
		gameScript gameScript3 = ((num != -1) ? games_.arrayGamesScripts[num] : this);
		if (gameScript3.reviewTotal < 30)
		{
			return 1;
		}
		if (gameScript3.reviewTotal >= 30 && gameScript3.reviewTotal < 40)
		{
			return 3;
		}
		if (gameScript3.reviewTotal >= 40 && gameScript3.reviewTotal < 50)
		{
			return 5;
		}
		if (gameScript3.reviewTotal >= 50 && gameScript3.reviewTotal < 60)
		{
			return 8;
		}
		if (gameScript3.reviewTotal >= 60 && gameScript3.reviewTotal < 70)
		{
			return 10;
		}
		if (gameScript3.reviewTotal >= 70 && gameScript3.reviewTotal < 80)
		{
			return 15;
		}
		if (gameScript3.reviewTotal >= 80 && gameScript3.reviewTotal < 90)
		{
			return 20;
		}
		if (gameScript3.reviewTotal >= 90 && gameScript3.reviewTotal < 98)
		{
			return 25;
		}
		if (gameScript3.reviewTotal >= 98)
		{
			return 30;
		}
		return 1;
	}

	public int GetAmountFanbriefe()
	{
		int num = 0;
		for (int i = 0; i < fanbrief.Length; i++)
		{
			if (fanbrief[i])
			{
				num++;
			}
		}
		return num;
	}

	public string GetZielgruppeString()
	{
		string result = "";
		switch (gameZielgruppe)
		{
		case 0:
			result = tS_.GetText(337);
			break;
		case 1:
			result = tS_.GetText(338);
			break;
		case 2:
			result = tS_.GetText(339);
			break;
		case 3:
			result = tS_.GetText(340);
			break;
		case 4:
			result = tS_.GetText(341);
			break;
		}
		return result;
	}

	private void UpdateFanletter()
	{
		if ((!IsMyGame() && !IsMyAuftragsspiel()) || pubOffer || sellsTotal < 100 || typ_addon || typ_mmoaddon || typ_bundle || typ_budget || typ_addonStandalone || typ_bundleAddon || typ_goty)
		{
			return;
		}
		if (fanbrief.Length == 0)
		{
			fanbrief = new bool[tS_.fanLetter_GE.Length];
		}
		if (fanbrief.Length < tS_.fanLetter_GE.Length)
		{
			fanbrief = new bool[tS_.fanLetter_GE.Length];
		}
		if (!fanbrief[0] && reviewTotal >= 80 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(0, GetNameWithTag());
			fanbrief[0] = true;
			return;
		}
		if (!fanbrief[1] && reviewTotal <= 40 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(1, GetNameWithTag());
			fanbrief[1] = true;
			return;
		}
		if (!fanbrief[2] && reviewGrafik >= 80 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(2, GetNameWithTag());
			fanbrief[2] = true;
			return;
		}
		if (!fanbrief[3] && reviewGrafik <= 40 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(3, GetNameWithTag());
			fanbrief[3] = true;
			return;
		}
		if (!fanbrief[4] && reviewSound >= 80 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(4, GetNameWithTag());
			fanbrief[4] = true;
			return;
		}
		if (!fanbrief[5] && reviewSound <= 40 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(5, GetNameWithTag());
			fanbrief[5] = true;
			return;
		}
		if (!fanbrief[6] && reviewSteuerung >= 80 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(6, GetNameWithTag());
			fanbrief[6] = true;
			return;
		}
		if (!fanbrief[7] && reviewSteuerung <= 40 && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(7, GetNameWithTag());
			fanbrief[7] = true;
			return;
		}
		if (!fanbrief[8])
		{
			int num = 0;
			for (int i = 0; i < gameLanguage.Length; i++)
			{
				if (gameLanguage[i])
				{
					num++;
				}
			}
			if (num <= 3 && UnityEngine.Random.Range(0, 10) == 1)
			{
				guiMain_.ShowFanLetter(8, GetNameWithTag());
				fanbrief[8] = true;
				return;
			}
		}
		if (!fanbrief[12] && Designschwerpunkt[0] < genres_.GetFocus(0, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(12, GetNameWithTag());
			fanbrief[12] = true;
		}
		else if (!fanbrief[15] && Designschwerpunkt[1] < genres_.GetFocus(1, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(15, GetNameWithTag());
			fanbrief[15] = true;
		}
		else if (!fanbrief[16] && Designschwerpunkt[2] < genres_.GetFocus(2, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(16, GetNameWithTag());
			fanbrief[16] = true;
		}
		else if (!fanbrief[19] && Designschwerpunkt[3] < genres_.GetFocus(3, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(19, GetNameWithTag());
			fanbrief[19] = true;
		}
		else if (!fanbrief[11] && Designschwerpunkt[4] < genres_.GetFocus(4, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(11, GetNameWithTag());
			fanbrief[11] = true;
		}
		else if (!fanbrief[20] && Designschwerpunkt[5] < genres_.GetFocus(5, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(20, GetNameWithTag());
			fanbrief[20] = true;
		}
		else if (!fanbrief[21] && Designschwerpunkt[6] < genres_.GetFocus(6, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(21, GetNameWithTag());
			fanbrief[21] = true;
		}
		else if (!fanbrief[22] && Designschwerpunkt[7] < genres_.GetFocus(7, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(22, GetNameWithTag());
			fanbrief[22] = true;
		}
		else if (!fanbrief[17] && Designausrichtung[0] > genres_.GetAlign(0, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(17, GetNameWithTag());
			fanbrief[17] = true;
		}
		else if (!fanbrief[18] && Designausrichtung[0] < genres_.GetAlign(0, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(18, GetNameWithTag());
			fanbrief[18] = true;
		}
		else if (!fanbrief[23] && Designausrichtung[1] < genres_.GetAlign(1, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(23, GetNameWithTag());
			fanbrief[23] = true;
		}
		else if (!fanbrief[24] && Designausrichtung[1] > genres_.GetAlign(1, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(24, GetNameWithTag());
			fanbrief[24] = true;
		}
		else if (!fanbrief[25] && Designausrichtung[2] < genres_.GetAlign(2, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(25, GetNameWithTag());
			fanbrief[25] = true;
		}
		else if (!fanbrief[26] && Designausrichtung[2] > genres_.GetAlign(2, maingenre, subgenre) && UnityEngine.Random.Range(0, 10) == 1)
		{
			guiMain_.ShowFanLetter(26, GetNameWithTag());
			fanbrief[26] = true;
		}
	}

	public void SetSpielbericht()
	{
		spielbericht = true;
	}

	public void SetMyName(string c)
	{
		myName = c;
	}

	public string GetNameSimple()
	{
		return myName;
	}

	public string GetNameWithTag()
	{
		string text = "";
		if ((bool)mS_.settings_)
		{
			text = ((!GetPublisherOrDeveloperIsTochterfirma() || !mS_.settings_.tochtefirmaTAG) ? myName : (myName + " <color=green>[★]</color>"));
		}
		if ((bool)tS_)
		{
			if (portID != -1)
			{
				text = text + " <color=green>" + tS_.GetText(1549) + "</color>";
			}
			if (typ_addon || typ_addonStandalone || typ_mmoaddon)
			{
				text = text + " <color=green>" + tS_.GetText(1896) + "</color>";
			}
			if (f2pConverted)
			{
				text = text + " <color=grey><i>" + tS_.GetText(1401) + "</i></color>";
			}
		}
		return text;
	}

	public string GetIpName()
	{
		if (ipName == null)
		{
			return myName;
		}
		if (ipName.Length <= 0)
		{
			return myName;
		}
		return ipName;
	}

	public string GetIpName_NotTheMaingame()
	{
		if (mainIP == -1)
		{
			return "";
		}
		if (!script_mainIP)
		{
			FindMainIpScript();
		}
		if (!script_mainIP)
		{
			mainIP = -1;
			return "";
		}
		return script_mainIP.GetIpName();
	}

	public long GetLagerbestand()
	{
		long num = 0L;
		for (int i = 0; i < lagerbestand.Length; i++)
		{
			num += lagerbestand[i];
		}
		return num;
	}

	public string GetLagerbestandString()
	{
		if (lagerbestand[1] > 0 || lagerbestand[2] > 0)
		{
			return mS_.GetMoney(lagerbestand[0], showDollar: false) + " [+" + mS_.GetMoney(lagerbestand[1] + lagerbestand[2], showDollar: false) + "]";
		}
		return mS_.GetMoney(lagerbestand[0], showDollar: false);
	}

	public int GetVorbestellungen()
	{
		return vorbestellungen;
	}

	public float GetEditionQualiaet(int i)
	{
		float num = 0f;
		switch (i)
		{
		case 0:
			if (standard_edition[0])
			{
				num += 0.01f;
			}
			if (standard_edition[1])
			{
				num += 0.05f;
			}
			if (standard_edition[2])
			{
				num += 0.02f;
			}
			if (standard_edition[3])
			{
				num += 0.02f;
			}
			if (standard_edition[4])
			{
				num += 0.03f;
			}
			if (standard_edition[5])
			{
				num += 0.03f;
			}
			if (standard_edition[6])
			{
				num += 0.04f;
			}
			if (standard_edition[7])
			{
				num += 0.04f;
			}
			break;
		case 1:
			num = 0f;
			if (deluxe_edition[0])
			{
				num += 0.01f;
			}
			if (deluxe_edition[1])
			{
				num += 0.05f;
			}
			if (deluxe_edition[2])
			{
				num += 0.05f;
			}
			if (deluxe_edition[3])
			{
				num += 0.05f;
			}
			if (deluxe_edition[4])
			{
				num += 0.06f;
			}
			if (deluxe_edition[5])
			{
				num += 0.08f;
			}
			if (deluxe_edition[6])
			{
				num += 0.1f;
			}
			if (deluxe_edition[7])
			{
				num += 0.15f;
			}
			if (deluxe_edition[8])
			{
				num += 0.2f;
			}
			break;
		case 2:
			num = 0f;
			if (collectors_edition[0])
			{
				num += 0.01f;
			}
			if (collectors_edition[1])
			{
				num += 0.05f;
			}
			if (collectors_edition[2])
			{
				num += 0.05f;
			}
			if (collectors_edition[3])
			{
				num += 0.05f;
			}
			if (collectors_edition[4])
			{
				num += 0.06f;
			}
			if (collectors_edition[5])
			{
				num += 0.08f;
			}
			if (collectors_edition[6])
			{
				num += 0.1f;
			}
			if (collectors_edition[7])
			{
				num += 0.15f;
			}
			if (collectors_edition[8])
			{
				num += 0.2f;
			}
			if (collectors_edition[9])
			{
				num += 0.25f;
			}
			break;
		}
		return num;
	}

	public float GetPreisAbzug(int i)
	{
		if (arcade)
		{
			if (verkaufspreis[i] <= 549)
			{
				return 1f;
			}
			if (verkaufspreis[i] >= 550 && verkaufspreis[i] <= 599)
			{
				return 0.98f;
			}
			if (verkaufspreis[i] >= 600 && verkaufspreis[i] <= 649)
			{
				return 0.96f;
			}
			if (verkaufspreis[i] >= 650 && verkaufspreis[i] <= 699)
			{
				return 0.94f;
			}
			if (verkaufspreis[i] >= 700 && verkaufspreis[i] <= 749)
			{
				return 0.92f;
			}
			if (verkaufspreis[i] >= 750 && verkaufspreis[i] <= 799)
			{
				return 0.9f;
			}
			if (verkaufspreis[i] >= 800 && verkaufspreis[i] <= 849)
			{
				return 0.88f;
			}
			if (verkaufspreis[i] >= 850 && verkaufspreis[i] <= 899)
			{
				return 0.86f;
			}
			if (verkaufspreis[i] >= 900 && verkaufspreis[i] <= 949)
			{
				return 0.84f;
			}
			if (verkaufspreis[i] >= 950 && verkaufspreis[i] <= 999)
			{
				return 0.82f;
			}
			if (verkaufspreis[i] >= 1000 && verkaufspreis[i] <= 1049)
			{
				return 0.8f;
			}
			if (verkaufspreis[i] >= 1050 && verkaufspreis[i] <= 1099)
			{
				return 0.75f;
			}
			if (verkaufspreis[i] >= 1100 && verkaufspreis[i] <= 1149)
			{
				return 0.7f;
			}
			if (verkaufspreis[i] >= 1150 && verkaufspreis[i] <= 1199)
			{
				return 0.65f;
			}
			if (verkaufspreis[i] >= 1200 && verkaufspreis[i] <= 1249)
			{
				return 0.6f;
			}
			if (verkaufspreis[i] >= 1250 && verkaufspreis[i] <= 1299)
			{
				return 0.55f;
			}
			if (verkaufspreis[i] >= 1300 && verkaufspreis[i] <= 1349)
			{
				return 0.5f;
			}
			if (verkaufspreis[i] >= 1350 && verkaufspreis[i] <= 1399)
			{
				return 0.45f;
			}
			if (verkaufspreis[i] >= 1400 && verkaufspreis[i] <= 1449)
			{
				return 0.4f;
			}
			if (verkaufspreis[i] >= 1450)
			{
				return 0.3f;
			}
		}
		if (handy)
		{
			if (verkaufspreis[i] == 1)
			{
				return 1f;
			}
			if (verkaufspreis[i] == 2)
			{
				return 0.8f;
			}
			if (verkaufspreis[i] == 3)
			{
				return 0.6f;
			}
			if (verkaufspreis[i] == 4)
			{
				return 0.4f;
			}
			if (verkaufspreis[i] >= 5)
			{
				return 0.25f;
			}
			return 0.5f;
		}
		if (verkaufspreis[i] <= 4)
		{
			return 1f;
		}
		if (verkaufspreis[i] >= 5 && verkaufspreis[i] <= 9)
		{
			return 0.95f;
		}
		if (verkaufspreis[i] >= 10 && verkaufspreis[i] <= 14)
		{
			return 0.9f;
		}
		if (verkaufspreis[i] >= 15 && verkaufspreis[i] <= 19)
		{
			return 0.8f;
		}
		if (verkaufspreis[i] >= 20 && verkaufspreis[i] <= 24)
		{
			return 0.7f;
		}
		if (verkaufspreis[i] >= 25 && verkaufspreis[i] <= 29)
		{
			return 0.6f;
		}
		if (verkaufspreis[i] >= 30 && verkaufspreis[i] <= 34)
		{
			return 0.5f;
		}
		if (verkaufspreis[i] >= 35 && verkaufspreis[i] <= 39)
		{
			return 0.45f;
		}
		if (verkaufspreis[i] >= 40 && verkaufspreis[i] <= 44)
		{
			return 0.3f;
		}
		if (verkaufspreis[i] >= 45 && verkaufspreis[i] <= 49)
		{
			return 0.25f;
		}
		if (verkaufspreis[i] >= 50 && verkaufspreis[i] <= 54)
		{
			return 0.22f;
		}
		if (verkaufspreis[i] >= 55 && verkaufspreis[i] <= 59)
		{
			return 0.2f;
		}
		if (verkaufspreis[i] >= 60 && verkaufspreis[i] <= 64)
		{
			return 0.18f;
		}
		if (verkaufspreis[i] >= 65 && verkaufspreis[i] <= 69)
		{
			return 0.16f;
		}
		if (verkaufspreis[i] >= 70 && verkaufspreis[i] <= 74)
		{
			return 0.15f;
		}
		if (verkaufspreis[i] >= 75 && verkaufspreis[i] <= 79)
		{
			return 0.14f;
		}
		if (verkaufspreis[i] >= 80 && verkaufspreis[i] <= 84)
		{
			return 0.12f;
		}
		if (verkaufspreis[i] >= 85 && verkaufspreis[i] <= 89)
		{
			return 0.1f;
		}
		if (verkaufspreis[i] >= 90 && verkaufspreis[i] <= 94)
		{
			return 0.08f;
		}
		if (verkaufspreis[i] >= 95)
		{
			return 0.05f;
		}
		return 0.5f;
	}

	public float GetProduktionskosten(int edition)
	{
		float num = 0f;
		switch (edition)
		{
		case 0:
		{
			for (int j = 0; j < standard_edition.Length; j++)
			{
				if (standard_edition[j])
				{
					num += games_.preise_inhalt[j];
				}
			}
			break;
		}
		case 1:
		{
			for (int k = 0; k < deluxe_edition.Length; k++)
			{
				if (deluxe_edition[k])
				{
					num += games_.preise_inhalt[k];
				}
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < collectors_edition.Length; i++)
			{
				if (collectors_edition[i])
				{
					num += games_.preise_inhalt[i];
				}
			}
			break;
		}
		case 3:
			return 0f;
		}
		return num + games_.GetGrundkosten();
	}

	public float CalcPlatformComplex(float points)
	{
		float num = 1f;
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] == -1)
			{
				continue;
			}
			if (!gamePlatformScript[i])
			{
				FindMyPlatforms();
			}
			if ((bool)gamePlatformScript[i])
			{
				switch (gamePlatformScript[i].complex)
				{
				case 0:
					num += 0.1f;
					break;
				case 1:
					num += 0.3f;
					break;
				case 2:
					num += 0.6f;
					break;
				}
			}
		}
		return num * points;
	}

	public engineScript GetEngineScript()
	{
		FindMyEngineNew();
		return engineS_;
	}

	private string GetUskString()
	{
		if (!tS_)
		{
			return "";
		}
		string result = "";
		switch (usk)
		{
		case 0:
			result = "0";
			break;
		case 1:
			result = "6";
			break;
		case 2:
			result = "12";
			break;
		case 3:
			result = "16";
			break;
		case 4:
			result = "18";
			break;
		case 5:
			result = tS_.GetText(1306);
			break;
		}
		return result;
	}

	public gameScript GetBundleGame(int slot_)
	{
		if (bundleID[slot_] == -1)
		{
			return null;
		}
		if ((bool)bundleGameScript[slot_])
		{
			return bundleGameScript[slot_];
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == bundleID[slot_])
			{
				bundleGameScript[slot_] = games_.arrayGamesScripts[i];
				return games_.arrayGamesScripts[i];
			}
		}
		return null;
	}

	public bool HasInAppPurchases()
	{
		if (!gameplayFeatures_DevDone[57])
		{
			return false;
		}
		for (int i = 0; i < inAppPurchase.Length; i++)
		{
			if (inAppPurchase[i])
			{
				return true;
			}
		}
		return false;
	}

	public float GetInAppPurchaseHate()
	{
		float num = 0f;
		for (int i = 0; i < inAppPurchase.Length; i++)
		{
			if (inAppPurchase[i])
			{
				num += games_.inAppPurchaseHate[i];
			}
		}
		return num;
	}

	public float GetInAppPurchaseMoneyPerWeek()
	{
		float num = 0f;
		for (int i = 0; i < inAppPurchase.Length; i++)
		{
			if (inAppPurchase[i])
			{
				num += games_.inAppPurchasePrice[i];
			}
		}
		return num * 0.25f;
	}

	public bool ExistAutomatenspiel()
	{
		if (gameTyp == 0 && !typ_addon && !typ_addonStandalone && !typ_bundle && !typ_bundleAddon && !typ_contractGame && !typ_mmoaddon && !arcade)
		{
			if (portExist[2])
			{
				return true;
			}
			if (portID != -1)
			{
				FindPortOriginalScript();
				if ((bool)script_portOriginal && script_portOriginal.arcade)
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsHit()
	{
		if (typ_addon || typ_addonStandalone || typ_budget || typ_bundle || typ_mmoaddon || typ_bundleAddon)
		{
			return false;
		}
		if (reviewTotal >= 80)
		{
			return true;
		}
		return false;
	}

	public bool HasGold()
	{
		if (gameTyp != 2 && !arcade && !typ_addon && !typ_addonStandalone && !typ_mmoaddon && sellsTotal >= 1000000)
		{
			return true;
		}
		return false;
	}

	public bool HasPlatin()
	{
		if (gameTyp != 2 && !arcade && !typ_addon && !typ_addonStandalone && !typ_mmoaddon && sellsTotal >= 5000000)
		{
			return true;
		}
		return false;
	}

	public bool HasDiamant()
	{
		if (gameTyp != 2 && !arcade && !typ_addon && !typ_addonStandalone && !typ_mmoaddon && sellsTotal >= 10000000)
		{
			return true;
		}
		return false;
	}

	public bool GetEinePlattformReleased()
	{
		FindMyPlatforms();
		for (int i = 0; i < gamePlatformScript.Length; i++)
		{
			if ((bool)gamePlatformScript[i] && gamePlatformScript[i].isUnlocked)
			{
				return true;
			}
		}
		return false;
	}

	public bool AllePlattformenReleased()
	{
		FindMyPlatforms();
		for (int i = 0; i < gamePlatformScript.Length; i++)
		{
			if ((bool)gamePlatformScript[i] && !gamePlatformScript[i].isUnlocked)
			{
				return false;
			}
		}
		return true;
	}

	public string GetUnreleasedPlatformsString()
	{
		FindMyPlatforms();
		string text = "";
		for (int i = 0; i < gamePlatformScript.Length; i++)
		{
			if ((bool)gamePlatformScript[i] && !gamePlatformScript[i].isUnlocked)
			{
				text = text + gamePlatformScript[i].GetName() + "\n";
			}
		}
		return text;
	}

	public void UpdateAutoPreis()
	{
		if (typ_budget)
		{
			verkaufspreis[0] = 10;
			verkaufspreis[3] = 10;
			if (weeksOnMarket > 30)
			{
				verkaufspreis[0] -= 3;
				verkaufspreis[3] -= 3;
			}
			if (weeksOnMarket > 60)
			{
				verkaufspreis[0] -= 2;
				verkaufspreis[3] -= 2;
			}
			if (verkaufspreis[0] > 10)
			{
				verkaufspreis[0] = 10;
			}
			if (verkaufspreis[3] > 10)
			{
				verkaufspreis[3] = 10;
			}
			if (verkaufspreis[0] < 5)
			{
				verkaufspreis[0] = 5;
			}
			if (verkaufspreis[3] < 5)
			{
				verkaufspreis[3] = 5;
			}
			return;
		}
		if (typ_goty)
		{
			verkaufspreis[0] = 19;
			verkaufspreis[1] = 29;
			verkaufspreis[2] = 39;
			verkaufspreis[3] = 19;
			if (weeksOnMarket > 30)
			{
				verkaufspreis[0] -= 4;
				verkaufspreis[1] -= 4;
				verkaufspreis[2] -= 4;
				verkaufspreis[3] -= 4;
			}
			if (weeksOnMarket > 60)
			{
				verkaufspreis[0] -= 6;
				verkaufspreis[1] -= 6;
				verkaufspreis[2] -= 6;
				verkaufspreis[3] -= 6;
			}
			if (verkaufspreis[0] > 19)
			{
				verkaufspreis[0] = 19;
			}
			if (verkaufspreis[1] > 29)
			{
				verkaufspreis[1] = 29;
			}
			if (verkaufspreis[2] > 39)
			{
				verkaufspreis[2] = 39;
			}
			if (verkaufspreis[3] > 19)
			{
				verkaufspreis[3] = 19;
			}
			if (verkaufspreis[0] < 5)
			{
				verkaufspreis[0] = 5;
			}
			if (verkaufspreis[1] < 6)
			{
				verkaufspreis[1] = 6;
			}
			if (verkaufspreis[2] < 7)
			{
				verkaufspreis[2] = 7;
			}
			if (verkaufspreis[3] < 5)
			{
				verkaufspreis[3] = 5;
			}
			return;
		}
		if (typ_addon || typ_addonStandalone || typ_mmoaddon)
		{
			verkaufspreis[0] = 29;
			verkaufspreis[1] = 39;
			verkaufspreis[2] = 49;
			verkaufspreis[3] = 29;
			if (weeksOnMarket > 30)
			{
				verkaufspreis[0] -= 5;
				verkaufspreis[1] -= 5;
				verkaufspreis[2] -= 5;
				verkaufspreis[3] -= 5;
			}
			if (weeksOnMarket > 60)
			{
				verkaufspreis[0] -= 5;
				verkaufspreis[1] -= 5;
				verkaufspreis[2] -= 5;
				verkaufspreis[3] -= 5;
			}
			if (verkaufspreis[0] > 29)
			{
				verkaufspreis[0] = 29;
			}
			if (verkaufspreis[1] > 39)
			{
				verkaufspreis[1] = 39;
			}
			if (verkaufspreis[2] > 49)
			{
				verkaufspreis[2] = 49;
			}
			if (verkaufspreis[3] > 29)
			{
				verkaufspreis[3] = 29;
			}
			if (verkaufspreis[0] < 5)
			{
				verkaufspreis[0] = 5;
			}
			if (verkaufspreis[1] < 6)
			{
				verkaufspreis[1] = 6;
			}
			if (verkaufspreis[2] < 7)
			{
				verkaufspreis[2] = 7;
			}
			if (verkaufspreis[3] < 5)
			{
				verkaufspreis[3] = 5;
			}
			return;
		}
		if (reviewTotal <= 50)
		{
			verkaufspreis[0] = 35;
			verkaufspreis[1] = 45;
			verkaufspreis[2] = 55;
			verkaufspreis[3] = 35;
		}
		if (reviewTotal > 50 && reviewTotal <= 70)
		{
			verkaufspreis[0] = 39;
			verkaufspreis[1] = 49;
			verkaufspreis[2] = 59;
			verkaufspreis[3] = 39;
		}
		if (reviewTotal > 70 && reviewTotal <= 90)
		{
			verkaufspreis[0] = 45;
			verkaufspreis[1] = 55;
			verkaufspreis[2] = 65;
			verkaufspreis[3] = 45;
		}
		if (reviewTotal > 90)
		{
			verkaufspreis[0] = 49;
			verkaufspreis[1] = 59;
			verkaufspreis[2] = 69;
			verkaufspreis[3] = 49;
		}
		if (weeksOnMarket > 30)
		{
			verkaufspreis[0] -= 5;
			verkaufspreis[1] -= 5;
			verkaufspreis[2] -= 5;
			verkaufspreis[3] -= 5;
		}
		if (weeksOnMarket > 50)
		{
			verkaufspreis[0] -= 5;
			verkaufspreis[1] -= 5;
			verkaufspreis[2] -= 5;
			verkaufspreis[3] -= 5;
		}
		if (weeksOnMarket > 70)
		{
			verkaufspreis[0] -= 5;
			verkaufspreis[1] -= 5;
			verkaufspreis[2] -= 5;
			verkaufspreis[3] -= 5;
		}
		if (verkaufspreis[0] > 79)
		{
			verkaufspreis[0] = 79;
		}
		if (verkaufspreis[1] > 89)
		{
			verkaufspreis[1] = 89;
		}
		if (verkaufspreis[2] > 99)
		{
			verkaufspreis[2] = 99;
		}
		if (verkaufspreis[3] > 79)
		{
			verkaufspreis[3] = 79;
		}
		if (verkaufspreis[0] < 5)
		{
			verkaufspreis[0] = 5;
		}
		if (verkaufspreis[1] < 6)
		{
			verkaufspreis[1] = 6;
		}
		if (verkaufspreis[2] < 7)
		{
			verkaufspreis[2] = 7;
		}
		if (verkaufspreis[3] < 5)
		{
			verkaufspreis[3] = 5;
		}
	}

	public Sprite GetDeveloperLogo()
	{
		if (!guiMain_)
		{
			FindScripts();
		}
		if ((bool)devS_)
		{
			return devS_.GetLogo();
		}
		if ((bool)mS_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == developerID)
				{
					return mS_.arrayPublisherScripts[i].GetLogo();
				}
			}
		}
		return guiMain_.uiSprites[19];
	}

	public Sprite GetOwnerLogo()
	{
		if (!guiMain_)
		{
			FindScripts();
		}
		if ((bool)ownerS_)
		{
			return ownerS_.GetLogo();
		}
		if ((bool)mS_)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == ownerID)
				{
					return mS_.arrayPublisherScripts[i].GetLogo();
				}
			}
		}
		return guiMain_.uiSprites[19];
	}

	public void FindPublisherForGame()
	{
		int num = -1;
		if (!mS_)
		{
			FindScripts();
		}
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if ((bool)devS_ && devS_.IsMyTochterfirma() && devS_.tf_ownPublisher && devS_.tf_ownPublisherPriority != -1)
		{
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == devS_.tf_ownPublisherPriority && mS_.arrayPublisherScripts[i].isUnlocked && !mS_.arrayPublisherScripts[i].isPlayer && mS_.arrayPublisherScripts[i].publisher && mS_.arrayPublisherScripts[i].IsMyTochterfirma() && !mS_.arrayPublisherScripts[i].TochterfirmaGeschlossen())
				{
					publisherID = mS_.arrayPublisherScripts[i].myID;
					return;
				}
			}
			devS_.tf_ownPublisherPriority = -1;
		}
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if ((bool)devS_ && devS_.IsMyTochterfirma() && devS_.tf_ownPublisher)
		{
			List<PublisherList> list = new List<PublisherList>();
			for (int j = 0; j < mS_.arrayPublisherScripts.Length; j++)
			{
				if ((bool)mS_.arrayPublisherScripts[j] && mS_.arrayPublisherScripts[j].isUnlocked && !mS_.arrayPublisherScripts[j].isPlayer && mS_.arrayPublisherScripts[j].publisher && mS_.arrayPublisherScripts[j].IsMyTochterfirma() && !mS_.arrayPublisherScripts[j].TochterfirmaGeschlossen())
				{
					list.Add(new PublisherList(0L, mS_.arrayPublisherScripts[j]));
				}
			}
			if (list.Count > 0)
			{
				publisherID = list[UnityEngine.Random.Range(0, list.Count)].script_.myID;
				return;
			}
		}
		if (UnityEngine.Random.Range(0, 100) > 50)
		{
			List<PublisherList> list2 = new List<PublisherList>();
			for (int k = 0; k < mS_.arrayPublisherScripts.Length; k++)
			{
				if ((bool)mS_.arrayPublisherScripts[k] && mS_.arrayPublisherScripts[k].isUnlocked && !mS_.arrayPublisherScripts[k].isPlayer && mS_.arrayPublisherScripts[k].publisher && !mS_.arrayPublisherScripts[k].TochterfirmaGeschlossen() && !mS_.arrayPublisherScripts[k].Geschlossen())
				{
					float f = mS_.arrayPublisherScripts[k].share + mS_.arrayPublisherScripts[k].stars * 0.1f;
					list2.Add(new PublisherList(Mathf.RoundToInt(f), mS_.arrayPublisherScripts[k]));
				}
			}
			list2 = list2.OrderByDescending((PublisherList publisherList) => publisherList.wert).ToList();
			for (int num2 = 0; num2 < list2.Count; num2++)
			{
				if (UnityEngine.Random.Range(0, 100) > 80)
				{
					publisherID = list2[num2].script_.myID;
					return;
				}
			}
		}
		for (int num3 = 0; num3 < 10; num3++)
		{
			int num4 = UnityEngine.Random.Range(0, mS_.arrayPublisherScripts.Length - 1);
			if ((bool)mS_.arrayPublisherScripts[num4] && mS_.arrayPublisherScripts[num4].isUnlocked && !mS_.arrayPublisherScripts[num4].isPlayer && mS_.arrayPublisherScripts[num4].publisher && !mS_.arrayPublisherScripts[num4].TochterfirmaGeschlossen() && !mS_.arrayPublisherScripts[num4].Geschlossen())
			{
				publisherID = mS_.arrayPublisherScripts[num4].myID;
				return;
			}
		}
		for (int num5 = 0; num5 < mS_.arrayPublisherScripts.Length; num5++)
		{
			if ((bool)mS_.arrayPublisherScripts[num5] && mS_.arrayPublisherScripts[num5].isUnlocked && !mS_.arrayPublisherScripts[num5].isPlayer && mS_.arrayPublisherScripts[num5].publisher && !mS_.arrayPublisherScripts[num5].TochterfirmaGeschlossen() && !mS_.arrayPublisherScripts[num5].Geschlossen() && (publisherID == -1 || UnityEngine.Random.Range(0, 100) > 70))
			{
				num = mS_.arrayPublisherScripts[num5].myID;
			}
		}
		publisherID = num;
	}

	public int PUBOFFER_GetGarantiesumme()
	{
		if (typ_budget)
		{
			return 0;
		}
		if (typ_bundle)
		{
			return 0;
		}
		if (typ_bundleAddon)
		{
			return 0;
		}
		if (typ_goty)
		{
			return 0;
		}
		float num = pubAngebot_VerhandlungProzent;
		num *= 0.01f;
		return Mathf.RoundToInt((float)pubAngebot_Garantiesumme * num);
	}

	public int PUBOFFER_GetGewinnbeteiligung()
	{
		float num = pubAngebot_VerhandlungProzent;
		num *= 0.01f;
		return Mathf.RoundToInt(pubAngebot_Gewinnbeteiligung * num);
	}

	public string PUBOFFER_GetRetailDigitalString()
	{
		if ((bool)tS_)
		{
			if (pubAngebot_Retail && pubAngebot_Digital)
			{
				return tS_.GetText(1746);
			}
			if (pubAngebot_Retail && !pubAngebot_Digital)
			{
				return tS_.GetText(1747);
			}
			if (!pubAngebot_Retail && pubAngebot_Digital)
			{
				return tS_.GetText(1748);
			}
		}
		return "<missing>";
	}

	public string PUBOFFER_GetTooltip(int reviewTotal_)
	{
		string text = "";
		text = text + "<b><size=18>" + GetNameWithTag() + "</size></b>";
		text = text + "\n<b><color=black>" + GetDeveloperName() + "</color></b>";
		if (!typ_bundle && !typ_bundleAddon)
		{
			text = text + "\n<b>" + GetTypString() + " | " + GetPlatformTypString() + "</b>\n";
		}
		text += "<size=12>";
		if (!typ_bundle && !typ_bundleAddon && subgenre == -1)
		{
			text = text + GetGenreString() + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && subgenre != -1)
		{
			text = text + GetGenreString() + " / " + GetSubGenreString() + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && gameSubTheme == -1)
		{
			text = text + tS_.GetThemes(gameMainTheme) + "\n";
		}
		if (!typ_bundle && !typ_bundleAddon && gameSubTheme != -1)
		{
			text = text + tS_.GetThemes(gameMainTheme) + " / " + tS_.GetThemes(gameSubTheme) + "\n";
		}
		text += "</size>";
		text = text + "\n<color=magenta><b>" + PUBOFFER_GetRetailDigitalString() + "</b></color>\n\n";
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] > 0)
			{
				if (!gamePlatformScript[i])
				{
					FindMyPlatforms();
				}
				if ((bool)gamePlatformScript[i])
				{
					text = ((!gamePlatformScript[i].IsVerfuegbar()) ? (text + "<color=red>" + gamePlatformScript[i].GetName() + "</color>\n") : (text + "<color=grey>" + gamePlatformScript[i].GetName() + "</color>\n"));
				}
			}
		}
		text += "\n";
		text = text + tS_.GetText(327) + ": <color=blue>" + GetGamesizeString() + "</color>\n";
		text = text + tS_.GetText(1730) + ": <color=blue>" + mS_.GetMoney(PUBOFFER_GetGarantiesumme(), showDollar: true) + "</color>\n";
		text = text + tS_.GetText(1731) + ": <color=blue>" + PUBOFFER_GetGewinnbeteiligung() + "%</color>\n";
		text += "\n";
		int i2 = Mathf.RoundToInt(reviewTotal_ / 20);
		text = text + tS_.GetText(1732) + "\n<size=21>" + PUBOFFER_GetQualitatStars(i2) + "</size>\n\n";
		if (!typ_bundle)
		{
			text = text + tS_.GetText(1555) + ": <color=blue>" + mS_.Round(GetIpBekanntheit(), 1) + "</color>\n\n";
		}
		text = text + tS_.GetText(1736) + "\n";
		if (pubAngebot_Stimmung < 33f)
		{
			text = text + "<color=red><b>" + tS_.GetText(1740) + "</b></color>";
		}
		if (pubAngebot_Stimmung > 33f && pubAngebot_Stimmung < 66f)
		{
			text = text + "<color=orange><b>" + tS_.GetText(1741) + "</b></color>";
		}
		if (pubAngebot_Stimmung > 66f)
		{
			text = text + "<color=green><b>" + tS_.GetText(1742) + "</b></color>";
		}
		return text;
	}

	public string GetGamesizeString()
	{
		if (!tS_)
		{
			FindScripts();
		}
		return gameSize switch
		{
			0 => tS_.GetText(329), 
			1 => tS_.GetText(330), 
			2 => tS_.GetText(331), 
			3 => tS_.GetText(332), 
			4 => tS_.GetText(333), 
			5 => tS_.GetText(2193), 
			_ => "", 
		};
	}

	private string PUBOFFER_GetQualitatStars(int i)
	{
		string text = "";
		return i switch
		{
			0 => "☆☆☆☆☆", 
			1 => "★☆☆☆☆", 
			2 => "★★☆☆☆", 
			3 => "★★★☆☆", 
			4 => "★★★★☆", 
			5 => "★★★★★", 
			_ => "☆☆☆☆☆", 
		};
	}

	public bool IsMyAuftragsspiel()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (typ_contractGame && developerID == mS_.myID && ownerID != mS_.myID)
		{
			return true;
		}
		return false;
	}

	public string GetUrsprungsName()
	{
		gameScript gameScript2 = FindVorgaengerScript();
		gameScript gameScript3 = null;
		if ((bool)gameScript2)
		{
			for (int i = 0; i < 10000; i++)
			{
				gameScript3 = gameScript2.FindVorgaengerScript();
				if (!gameScript3)
				{
					break;
				}
				gameScript2 = gameScript3;
			}
			return gameScript2.myName;
		}
		return myName;
	}

	public bool GetDeveloperIsTochtefirma()
	{
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if ((bool)devS_ && devS_.IsMyTochterfirma())
		{
			return true;
		}
		return false;
	}

	public bool GetPublisherIsTochtefirma()
	{
		if (!pS_)
		{
			FindMyPublisher();
		}
		if ((bool)pS_ && pS_.IsMyTochterfirma())
		{
			return true;
		}
		return false;
	}

	public bool GetPublisherOrDeveloperIsTochterfirma()
	{
		if (!pS_)
		{
			FindMyPublisher();
		}
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if ((bool)pS_ && pS_.IsMyTochterfirma())
		{
			return true;
		}
		if ((bool)devS_ && devS_.IsMyTochterfirma())
		{
			return true;
		}
		return false;
	}

	public bool IsMyGame()
	{
		if (typ_contractGame)
		{
			return false;
		}
		if (developerID != mS_.myID && publisherID != mS_.myID)
		{
			return false;
		}
		if (developerID == mS_.myID && publisherID != mS_.myID)
		{
			return true;
		}
		if (developerID == mS_.myID && publisherID == mS_.myID)
		{
			return true;
		}
		if (developerID != mS_.myID && publisherID == mS_.myID)
		{
			return true;
		}
		return false;
	}

	public void AddTochterfirmaUmsatz(long i)
	{
		if (IsMyGame())
		{
			return;
		}
		if (!pS_)
		{
			FindMyPublisher();
		}
		if (!devS_)
		{
			FindMyDeveloper();
		}
		if (!pS_ || !devS_)
		{
			return;
		}
		if (pS_.IsMyTochterfirma() || devS_.IsMyTochterfirma())
		{
			long num = i / 100 * Mathf.RoundToInt(games_.tf_gewinnbeteiligungTochterfirma);
			mS_.Earn(num, 13);
			tw_gewinnanteil += num;
			if (pS_.IsMyTochterfirma() && devS_.IsMyTochterfirma())
			{
				devS_.AddTochterfirmaUmsatz(num);
			}
			else if (pS_.IsMyTochterfirma() && !devS_.IsMyTochterfirma())
			{
				pS_.AddTochterfirmaUmsatz(num);
			}
			else if (!pS_.IsMyTochterfirma() && devS_.IsMyTochterfirma())
			{
				devS_.AddTochterfirmaUmsatz(num);
			}
		}
		else
		{
			if (!mS_.multiplayer || (!pS_.IsTochterfirmaVonMitspieler() && !devS_.IsTochterfirmaVonMitspieler()))
			{
				return;
			}
			long num2 = i / 100 * Mathf.RoundToInt(games_.tf_gewinnbeteiligungTochterfirma);
			mS_.Earn(num2, 13);
			tw_gewinnanteil += num2;
			if (pS_.IsTochterfirmaVonMitspieler() && devS_.IsTochterfirmaVonMitspieler())
			{
				devS_.AddTochterfirmaUmsatz(num2);
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_TochterfirmaUmsatz(devS_, this, num2);
				}
			}
			else if (pS_.IsTochterfirmaVonMitspieler() && !devS_.IsTochterfirmaVonMitspieler())
			{
				pS_.AddTochterfirmaUmsatz(num2);
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_TochterfirmaUmsatz(pS_, this, num2);
				}
			}
			else if (!pS_.IsTochterfirmaVonMitspieler() && devS_.IsTochterfirmaVonMitspieler())
			{
				devS_.AddTochterfirmaUmsatz(num2);
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_TochterfirmaUmsatz(devS_, this, num2);
				}
			}
		}
	}

	public bool IsMyIP(publisherScript script_)
	{
		if (script_.myID == ownerID)
		{
			return true;
		}
		return false;
	}

	public bool OwnerIsNPC()
	{
		if (ownerID < 100000)
		{
			return true;
		}
		return false;
	}

	public bool DeveloperIsNPC()
	{
		if (developerID < 100000)
		{
			return true;
		}
		return false;
	}

	public bool PublisherIsNPC()
	{
		if (publisherID < 100000)
		{
			return true;
		}
		return false;
	}

	public bool GameFromMitspieler()
	{
		if (!mS_.multiplayer)
		{
			return false;
		}
		if (ownerID < 100000 && publisherID < 100000 && developerID < 100000)
		{
			return false;
		}
		if (ownerID == mS_.myID)
		{
			return false;
		}
		if (publisherID == mS_.myID)
		{
			return false;
		}
		if (developerID == mS_.myID)
		{
			return false;
		}
		if (ownerID >= 100000)
		{
			return true;
		}
		if (publisherID >= 100000)
		{
			return true;
		}
		if (developerID >= 100000)
		{
			return true;
		}
		return false;
	}

	public int GetIdFromMitspieler()
	{
		if (!mS_.multiplayer)
		{
			return -1;
		}
		if (ownerID < 100000 && publisherID < 100000 && developerID < 100000)
		{
			return -1;
		}
		if (ownerID >= 100000)
		{
			return ownerID;
		}
		if (developerID >= 100000)
		{
			return developerID;
		}
		if (publisherID >= 100000)
		{
			return publisherID;
		}
		return -1;
	}

	public bool HasUnreleasedPlattform()
	{
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] != -1 && !gamePlatformScript[i])
			{
				FindMyPlatforms();
			}
		}
		for (int j = 0; j < gamePlatformScript.Length; j++)
		{
			if ((bool)gamePlatformScript[j] && !gamePlatformScript[j].isUnlocked)
			{
				return true;
			}
		}
		return false;
	}

	public bool HatEinePlattformInternet()
	{
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] != -1 && !gamePlatformScript[i])
			{
				FindMyPlatforms();
			}
		}
		for (int j = 0; j < gamePlatformScript.Length; j++)
		{
			if ((bool)gamePlatformScript[j] && gamePlatformScript[j].internet)
			{
				return true;
			}
		}
		return false;
	}

	public bool EinePlattformIstAmMarkt()
	{
		for (int i = 0; i < gamePlatform.Length; i++)
		{
			if (gamePlatform[i] != -1 && !gamePlatformScript[i])
			{
				FindMyPlatforms();
			}
		}
		for (int j = 0; j < gamePlatformScript.Length; j++)
		{
			if ((bool)gamePlatformScript[j] && gamePlatformScript[j].IsVerfuegbar())
			{
				return true;
			}
		}
		return false;
	}

	public bool IpFromTochterfirma()
	{
		if (!ownerS_)
		{
			FindMyOwner();
		}
		if ((bool)ownerS_ && ownerS_.IsMyTochterfirma())
		{
			return true;
		}
		return false;
	}

	public bool CanBeInGamePass()
	{
		if (inDevelopment)
		{
			return false;
		}
		if (handy)
		{
			return false;
		}
		if (arcade)
		{
			return false;
		}
		if (freeware)
		{
			return false;
		}
		if (typ_contractGame)
		{
			return false;
		}
		if (typ_addon)
		{
			return false;
		}
		if (typ_addonStandalone)
		{
			return false;
		}
		if (typ_mmoaddon)
		{
			return false;
		}
		if (typ_bundle)
		{
			return false;
		}
		if (typ_budget)
		{
			return false;
		}
		if (typ_bundleAddon)
		{
			return false;
		}
		if (typ_goty)
		{
			return false;
		}
		if (gameTyp != 0)
		{
			return false;
		}
		if ((ownerID == mS_.myID || IpFromTochterfirma()) && (typ_standard || typ_nachfolger || typ_remaster || typ_spinoff))
		{
			return true;
		}
		return false;
	}

	public bool MinEinePlattformIstImGamePass()
	{
		for (int i = 0; i < gamePlatformScript.Length; i++)
		{
			if (gamePlatform[i] != -1)
			{
				if (!gamePlatformScript[i])
				{
					FindMyPlatforms();
				}
				if ((bool)gamePlatformScript[i] && (gamePlatformScript[i].inGamePass || gamePlatformScript[i].inGamePassPassiv))
				{
					return true;
				}
			}
		}
		return false;
	}

	public bool IsKonsoleAbwaertskompatibel(platformScript script_)
	{
		if (!script_.hwFeatures[1])
		{
			return false;
		}
		if ((bool)script_)
		{
			for (int i = 0; i < script_.platformCompatible.Length; i++)
			{
				if (script_.platformCompatible[i] > 0 && (script_.platformCompatible[i] == gamePlatform[0] || script_.platformCompatible[i] == gamePlatform[1] || script_.platformCompatible[i] == gamePlatform[2] || script_.platformCompatible[i] == gamePlatform[3]))
				{
					return true;
				}
			}
		}
		return false;
	}

	public void AddFans(int sells_)
	{
		if (!mS_)
		{
			return;
		}
		int genre_ = maingenre;
		if (subgenre != -1 && UnityEngine.Random.Range(0, 100) > 70)
		{
			genre_ = subgenre;
		}
		float num = mS_.GetAchivementBonus(7);
		num *= 0.01f;
		if (!retro)
		{
			float num2 = sells_;
			if (gameTyp == 2)
			{
				num2 *= 0.1f;
			}
			num2 += num2 * num;
			if (reviewTotal < 10)
			{
				mS_.AddFans(-Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.1f)), genre_);
			}
			if (reviewTotal >= 10 && reviewTotal < 20)
			{
				mS_.AddFans(-Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.05f)), genre_);
			}
			if (reviewTotal >= 20 && reviewTotal < 30)
			{
				mS_.AddFans(-Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.03f)), genre_);
			}
			if (reviewTotal >= 30 && reviewTotal < 40)
			{
				mS_.AddFans(-Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.02f)), genre_);
			}
			if (reviewTotal >= 40 && reviewTotal < 50)
			{
				mS_.AddFans(-Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 50 && reviewTotal < 60)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 60 && reviewTotal < 70)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 70 && reviewTotal < 80)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 80 && reviewTotal < 90)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 90 && reviewTotal < 95)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 95 && reviewTotal < 100)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
			if (reviewTotal >= 100)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num2 * 0.01f)), genre_);
			}
		}
		else
		{
			float num3 = sells_;
			num3 += num3 * num;
			if (reviewTotal < 40)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.01f)), genre_);
			}
			if (reviewTotal >= 40 && reviewTotal < 50)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 50 && reviewTotal < 60)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 60 && reviewTotal < 70)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 70 && reviewTotal < 80)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 80 && reviewTotal < 90)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 90 && reviewTotal < 95)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 95 && reviewTotal < 100)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
			if (reviewTotal >= 100)
			{
				mS_.AddFans(Mathf.RoundToInt(UnityEngine.Random.Range(0f, num3 * 0.05f)), genre_);
			}
		}
	}

	public void SetVerkaufspreisNPC()
	{
		verkaufspreis[0] = 29;
		verkaufspreis[1] = 29;
		verkaufspreis[2] = 29;
		verkaufspreis[3] = 29;
		if (handy)
		{
			verkaufspreis[0] = 2;
		}
		if (arcade)
		{
			verkaufspreis[0] = 700;
			arcadeCase = 4;
			arcadeMonitor = 4;
			arcadeJoystick = 4;
			arcadeSound = 4;
		}
	}
}
