using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame : MonoBehaviour
{
	public int[] costs_gameTyp;

	public int[] costs_gameSize;

	public int[] maxFeatures_gameSize;

	public bool typ_standard;

	public bool typ_nachfolger;

	public bool typ_spinoff;

	public bool typ_remaster;

	public bool typ_contractGame;

	public bool typ_addon;

	public bool typ_bundle;

	public bool typ_budget;

	public bool typ_addonStandalone;

	public bool typ_mmoaddon;

	public characterScript g_leitenderDesigner;

	public string g_myNameTeil1 = "";

	public string g_Beschreibung = "";

	public int g_GameTyp;

	public int g_GameSize;

	public int g_GameZielgruppe = 4;

	public int g_GameMainGenre;

	public int g_GameSubGenre;

	public int g_GameMainTheme;

	public int g_GameSubTheme;

	public int g_GameLicence = -1;

	public int g_GameEngine = -1;

	public engineScript g_GameEngineScript_;

	public int g_mainIP = -1;

	public int g_originalGameID = -1;

	public int g_portIP = -1;

	public int g_teil = 1;

	public int[] g_GamePlatform;

	public int[] g_GameEngineFeature;

	public int g_GameCopyProtect = -1;

	public copyProtectScript g_GameCopyProtectScript_;

	public int g_GameAntiCheat = -1;

	public antiCheatScript g_GameAntiCheatScript_;

	public int g_GameAP_Gameplay;

	public int g_GameAP_Grafik;

	public int g_GameAP_Sound;

	public int g_GameAP_Technik;

	public bool[] g_GameLanguage;

	public bool[] g_GameGameplayFeatures;

	public int g_finanzierung_Grundkosten;

	public int g_finanzierung_Technology;

	public int g_finanzierung_Kontent;

	public bool[] g_InAppPurchase;

	public GameObject[] uiDesignschwerpunkte;

	public int[] g_Designschwerpunkt;

	public GameObject[] uiDesignausrichtung;

	public int[] g_Designausrichtung;

	private int seite;

	public Color languageColor;

	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private roomScript rS_;

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

	private games games_;

	private forschungSonstiges forschungSonstiges_;

	private platforms platforms_;

	public const int dropdown_multiplatform = 0;

	public const int dropdown_exklusiv = 1;

	public const int dropdown_herstellerExklusiv = 2;

	public const int dropdown_retro = 3;

	public const int dropdown_arcade = 4;

	public const int dropdown_handy = 5;

	private string orignal_name = "";

	private gameScript contractAuftragsspiel_;

	private float checkSoftwareTimer;

	private string searchStringA = "";

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
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
			if (!platforms_)
			{
				platforms_ = main_.GetComponent<platforms>();
			}
			if (!forschungSonstiges_)
			{
				forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(11);
		}
		cmS_.disableMovement = true;
		uiObjects[0].GetComponent<InputField>().characterLimit = 50;
		uiObjects[204].GetComponent<InputField>().text = "";
		searchStringA = "";
		CalcDevCosts();
		InitDropdowns_GameplayFeatures();
		InitDropdown_PlattformTyp(-1);
		string c = tS_.GetText(904) + "\n\n" + tS_.GetText(365) + "\n\n" + tS_.GetText(1695) + "\n\n" + tS_.GetText(905) + "\n\n" + tS_.GetText(1061) + "\n\n" + tS_.GetText(1062);
		uiObjects[147].GetComponent<tooltip>().c = c;
		c = tS_.GetText(1439) + "\n\n" + tS_.GetText(2075);
		uiObjects[225].GetComponent<tooltip>().c = c;
		uiObjects[0].GetComponent<InputField>().interactable = true;
		uiObjects[124].SetActive(value: true);
		uiObjects[125].SetActive(value: false);
		uiObjects[15].GetComponent<Button>().interactable = true;
		uiObjects[16].GetComponent<Button>().interactable = true;
		uiObjects[17].GetComponent<Button>().interactable = true;
		uiObjects[126].GetComponent<Button>().interactable = true;
		uiObjects[124].GetComponent<Button>().interactable = true;
		uiObjects[127].GetComponent<Button>().interactable = true;
		uiObjects[128].GetComponent<Button>().interactable = true;
		uiObjects[129].GetComponent<Button>().interactable = true;
		LockDesign(b: true);
		uiObjects[139].GetComponent<Button>().interactable = true;
		uiObjects[140].GetComponent<Button>().interactable = true;
		uiObjects[33].GetComponent<Button>().interactable = true;
		uiObjects[34].GetComponent<Button>().interactable = true;
		uiObjects[35].GetComponent<Button>().interactable = true;
		uiObjects[146].GetComponent<Dropdown>().interactable = true;
		uiObjects[146].GetComponent<Dropdown>().value = 0;
		uiObjects[84].GetComponent<Button>().interactable = true;
		uiObjects[85].GetComponent<Button>().interactable = true;
		uiObjects[173].GetComponent<Button>().interactable = true;
		uiObjects[172].GetComponent<Button>().interactable = true;
		uiObjects[167].SetActive(value: false);
		uiObjects[214].SetActive(value: false);
	}

	private void OnDisable()
	{
		FindScripts();
		cmS_.disableMovement = false;
	}

	private void Update()
	{
		if (uiObjects[227].transform.childCount <= 0)
		{
			if (uiObjects[228].activeSelf)
			{
				uiObjects[228].SetActive(value: false);
			}
		}
		else if (!uiObjects[228].activeSelf)
		{
			uiObjects[228].SetActive(value: true);
		}
		CheckNewSoftware();
	}

	private void InitDropdown_PlattformTyp(int platTyp_)
	{
		List<string> list = new List<string>();
		list = new List<string>();
		list.Add(tS_.GetText(902));
		list.Add(tS_.GetText(364));
		list.Add(tS_.GetText(1694));
		list.Add(tS_.GetText(903));
		if (platTyp_ == -1 || platTyp_ == 4 || platTyp_ == 5)
		{
			list.Add(tS_.GetText(1059));
			list.Add(tS_.GetText(1060));
		}
		uiObjects[146].GetComponent<Dropdown>().ClearOptions();
		uiObjects[146].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[146].GetComponent<Dropdown>().value = 0;
	}

	private void LockDesign(bool b)
	{
		for (int i = 0; i < uiDesignschwerpunkte.Length; i++)
		{
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().interactable = b;
		}
		for (int j = 0; j < uiDesignausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().interactable = b;
		}
		uiObjects[222].GetComponent<Button>().interactable = b;
	}

	public void SetAndLockPlatformTyp(int platTyp_)
	{
		if (platTyp_ != 5 && platTyp_ != 4)
		{
			InitDropdown_PlattformTyp(0);
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
		}
		else
		{
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
		}
		uiObjects[146].GetComponent<Dropdown>().value = platTyp_;
	}

	public void CopyAndLockPlatformTyp(gameScript script_)
	{
		if (!script_.handy && !script_.arcade)
		{
			InitDropdown_PlattformTyp(0);
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
		}
		else
		{
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
		}
		uiObjects[146].GetComponent<Dropdown>().value = 0;
		if (script_.exklusiv)
		{
			uiObjects[146].GetComponent<Dropdown>().value = 1;
		}
		if (script_.herstellerExklusiv)
		{
			uiObjects[146].GetComponent<Dropdown>().value = 2;
		}
		if (script_.retro)
		{
			uiObjects[146].GetComponent<Dropdown>().value = 3;
		}
		if (script_.arcade)
		{
			uiObjects[146].GetComponent<Dropdown>().value = 4;
		}
		if (script_.handy)
		{
			uiObjects[146].GetComponent<Dropdown>().value = 5;
		}
	}

	private void DrawIpStars()
	{
		if (g_mainIP != -1)
		{
			GameObject gameObject = GameObject.Find("GAME_" + g_mainIP);
			if ((bool)gameObject)
			{
				guiMain_.DrawIpBekanntheit(uiObjects[215], gameObject.GetComponent<gameScript>());
			}
		}
		else
		{
			guiMain_.DrawIpBekanntheit(uiObjects[215], null);
		}
	}

	public void InitSondereinstellungen(int platTyp_)
	{
		if (platTyp_ == 4)
		{
			SetGameTyp(0);
			uiObjects[126].GetComponent<Button>().interactable = false;
			uiObjects[185].SetActive(value: false);
			uiObjects[214].SetActive(value: true);
			uiObjects[84].GetComponent<Button>().interactable = false;
			uiObjects[85].GetComponent<Button>().interactable = false;
			uiObjects[173].GetComponent<Button>().interactable = false;
			uiObjects[172].GetComponent<Button>().interactable = false;
			g_InAppPurchase[0] = true;
			BUTTON_AlleInAppPurchase();
			SetCopyProtect(-1);
			SetAntiCheat(-1);
		}
	}

	public void InitNewGame(roomScript script_, int platTyp_)
	{
		Init(script_);
		InitSondereinstellungen(platTyp_);
		typ_standard = true;
		g_teil = 1;
		g_originalGameID = -1;
		g_portIP = -1;
		g_mainIP = -1;
		DrawIpStars();
		InitDropdown_PlattformTyp(platTyp_);
		uiObjects[146].GetComponent<Dropdown>().value = platTyp_;
		switch (platTyp_)
		{
		case 0:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 1:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 2:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 3:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 4:
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
			break;
		case 5:
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
			break;
		}
		DROPDOWN_PlattformTyp();
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[0];
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void InitPort(roomScript rscript_, int orgIP_, int platTyp_)
	{
		DeleteAllDatas();
		gameScript gameScript2 = null;
		GameObject gameObject = GameObject.Find("GAME_" + orgIP_);
		if ((bool)gameObject)
		{
			gameScript2 = gameObject.GetComponent<gameScript>();
			uiObjects[0].GetComponent<InputField>().text = gameScript2.GetNameSimple();
			g_myNameTeil1 = gameScript2.myNameTeil1;
			g_Beschreibung = gameScript2.beschreibung;
			g_GameTyp = gameScript2.gameTyp;
			g_GameZielgruppe = gameScript2.gameZielgruppe;
			g_GameMainGenre = gameScript2.maingenre;
			g_GameSubGenre = gameScript2.subgenre;
			g_GameMainTheme = gameScript2.gameMainTheme;
			g_GameSubTheme = gameScript2.gameSubTheme;
			g_GameSize = gameScript2.gameSize;
			if (g_GameSize == 1 && !forschungSonstiges_.IsErforscht(0))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 2 && !forschungSonstiges_.IsErforscht(1))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 3 && !forschungSonstiges_.IsErforscht(2))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 4 && !forschungSonstiges_.IsErforscht(3))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 5 && !forschungSonstiges_.IsErforscht(40))
			{
				g_GameSize = 0;
			}
			if (g_GameMainGenre != -1 && !genres_.IsErforscht(g_GameMainGenre))
			{
				g_GameMainGenre = -1;
			}
			if (g_GameSubGenre != -1 && !genres_.IsErforscht(g_GameSubGenre))
			{
				g_GameSubGenre = -1;
			}
			if (g_GameMainTheme != -1 && !themes_.IsErforscht(g_GameMainTheme))
			{
				g_GameMainTheme = -1;
			}
			if (g_GameSubTheme != -1 && !themes_.IsErforscht(g_GameSubTheme))
			{
				g_GameSubTheme = -1;
			}
			if (g_GameTyp == 1 && !unlock_.Get(21))
			{
				g_GameTyp = 0;
			}
			if (g_GameTyp == 2 && !unlock_.Get(22))
			{
				g_GameTyp = 0;
			}
			CopyDesignSettings(gameScript2);
		}
		Init(rscript_);
		g_mainIP = gameScript2.mainIP;
		DrawIpStars();
		g_originalGameID = gameScript2.originalGameID;
		typ_standard = gameScript2.typ_standard;
		typ_nachfolger = gameScript2.typ_nachfolger;
		typ_spinoff = gameScript2.typ_spinoff;
		typ_remaster = gameScript2.typ_remaster;
		typ_contractGame = gameScript2.typ_contractGame;
		typ_addon = gameScript2.typ_addon;
		typ_bundle = gameScript2.typ_bundle;
		typ_budget = gameScript2.typ_budget;
		typ_addonStandalone = gameScript2.typ_addonStandalone;
		typ_mmoaddon = gameScript2.typ_mmoaddon;
		g_portIP = orgIP_;
		SetLicence(gameScript2.gameLicence);
		InitDropdown_PlattformTyp(platTyp_);
		uiObjects[146].GetComponent<Dropdown>().value = platTyp_;
		switch (platTyp_)
		{
		case 0:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 1:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 2:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 3:
			uiObjects[146].GetComponent<Dropdown>().interactable = true;
			break;
		case 4:
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
			break;
		case 5:
			uiObjects[146].GetComponent<Dropdown>().interactable = false;
			break;
		}
		DROPDOWN_PlattformTyp();
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[14];
		uiObjects[15].GetComponent<Button>().interactable = false;
		uiObjects[16].GetComponent<Button>().interactable = false;
		uiObjects[17].GetComponent<Button>().interactable = false;
		uiObjects[126].GetComponent<Button>().interactable = false;
		uiObjects[127].GetComponent<Button>().interactable = false;
		uiObjects[128].GetComponent<Button>().interactable = false;
		uiObjects[129].GetComponent<Button>().interactable = false;
		LockDesign(b: false);
		InitSondereinstellungen(uiObjects[146].GetComponent<Dropdown>().value);
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void InitNachfolger(roomScript rscript_, int orgID_, int platTyp_)
	{
		DeleteAllDatas();
		gameScript gameScript2 = null;
		GameObject gameObject = GameObject.Find("GAME_" + orgID_);
		if ((bool)gameObject)
		{
			gameScript2 = gameObject.GetComponent<gameScript>();
			gameScript2.nachfolger_created = true;
			g_teil = gameScript2.teile + 1;
			uiObjects[0].GetComponent<InputField>().text = gameScript2.GetNameSimple();
			if (g_teil <= 2)
			{
				g_myNameTeil1 = gameScript2.GetNameSimple();
			}
			else
			{
				g_myNameTeil1 = gameScript2.myNameTeil1;
			}
			g_Beschreibung = gameScript2.beschreibung;
			g_GameTyp = gameScript2.gameTyp;
			g_GameZielgruppe = gameScript2.gameZielgruppe;
			g_GameMainGenre = gameScript2.maingenre;
			g_GameSubGenre = gameScript2.subgenre;
			g_GameMainTheme = gameScript2.gameMainTheme;
			g_GameSubTheme = gameScript2.gameSubTheme;
			g_GameSize = gameScript2.gameSize;
			if (g_GameSize == 1 && !forschungSonstiges_.IsErforscht(0))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 2 && !forschungSonstiges_.IsErforscht(1))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 3 && !forschungSonstiges_.IsErforscht(2))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 4 && !forschungSonstiges_.IsErforscht(3))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 5 && !forschungSonstiges_.IsErforscht(40))
			{
				g_GameSize = 0;
			}
			if (g_GameMainGenre != -1 && !genres_.IsErforscht(g_GameMainGenre))
			{
				g_GameMainGenre = -1;
			}
			if (g_GameSubGenre != -1 && !genres_.IsErforscht(g_GameSubGenre))
			{
				g_GameSubGenre = -1;
			}
			if (g_GameMainTheme != -1 && !themes_.IsErforscht(g_GameMainTheme))
			{
				g_GameMainTheme = -1;
			}
			if (g_GameSubTheme != -1 && !themes_.IsErforscht(g_GameSubTheme))
			{
				g_GameSubTheme = -1;
			}
			if (g_GameTyp == 1 && !unlock_.Get(21))
			{
				g_GameTyp = 0;
			}
			if (g_GameTyp == 2 && !unlock_.Get(22))
			{
				g_GameTyp = 0;
			}
			CopyDesignSettings(gameScript2);
		}
		Init(rscript_);
		typ_nachfolger = true;
		g_mainIP = gameScript2.mainIP;
		DrawIpStars();
		g_originalGameID = orgID_;
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[1];
		uiObjects[128].GetComponent<Button>().interactable = false;
		SetAndLockPlatformTyp(platTyp_);
		for (int i = 0; i < g_GameLanguage.Length; i++)
		{
			g_GameLanguage[i] = !gameScript2.gameLanguage[i];
			SetLanguage(i);
		}
		for (int j = 0; j < g_GameGameplayFeatures.Length; j++)
		{
			g_GameGameplayFeatures[j] = !gameScript2.gameGameplayFeatures[j];
			SetGameplayFeature(j);
		}
		InitSondereinstellungen(uiObjects[146].GetComponent<Dropdown>().value);
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void InitSpinoff(roomScript rscript_, int orgID_, int platTyp_)
	{
		DeleteAllDatas();
		gameScript gameScript2 = null;
		GameObject gameObject = GameObject.Find("GAME_" + orgID_);
		if ((bool)gameObject)
		{
			gameScript2 = gameObject.GetComponent<gameScript>();
			uiObjects[0].GetComponent<InputField>().text = gameScript2.GetNameSimple();
			g_myNameTeil1 = gameScript2.myNameTeil1;
			g_Beschreibung = gameScript2.beschreibung;
			g_GameTyp = gameScript2.gameTyp;
			g_GameZielgruppe = gameScript2.gameZielgruppe;
			g_GameMainGenre = gameScript2.maingenre;
			g_GameSubGenre = gameScript2.subgenre;
			g_GameMainTheme = gameScript2.gameMainTheme;
			g_GameSubTheme = gameScript2.gameSubTheme;
			g_GameSize = gameScript2.gameSize;
			if (g_GameSize == 1 && !forschungSonstiges_.IsErforscht(0))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 2 && !forschungSonstiges_.IsErforscht(1))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 3 && !forschungSonstiges_.IsErforscht(2))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 4 && !forschungSonstiges_.IsErforscht(3))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 5 && !forschungSonstiges_.IsErforscht(40))
			{
				g_GameSize = 0;
			}
			if (g_GameMainGenre != -1 && !genres_.IsErforscht(g_GameMainGenre))
			{
				g_GameMainGenre = -1;
			}
			if (g_GameSubGenre != -1 && !genres_.IsErforscht(g_GameSubGenre))
			{
				g_GameSubGenre = -1;
			}
			if (g_GameMainTheme != -1 && !themes_.IsErforscht(g_GameMainTheme))
			{
				g_GameMainTheme = -1;
			}
			if (g_GameSubTheme != -1 && !themes_.IsErforscht(g_GameSubTheme))
			{
				g_GameSubTheme = -1;
			}
			if (g_GameTyp == 1 && !unlock_.Get(21))
			{
				g_GameTyp = 0;
			}
			if (g_GameTyp == 2 && !unlock_.Get(22))
			{
				g_GameTyp = 0;
			}
			CopyDesignSettings(gameScript2);
		}
		Init(rscript_);
		typ_spinoff = true;
		g_mainIP = gameScript2.mainIP;
		DrawIpStars();
		g_originalGameID = orgID_;
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[13];
		SetAndLockPlatformTyp(platTyp_);
		InitSondereinstellungen(uiObjects[146].GetComponent<Dropdown>().value);
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void InitRemaster(roomScript rscript_, int orgID_, int platTyp_)
	{
		DeleteAllDatas();
		gameScript gameScript2 = null;
		GameObject gameObject = GameObject.Find("GAME_" + orgID_);
		if ((bool)gameObject)
		{
			gameScript2 = gameObject.GetComponent<gameScript>();
			gameScript2.remaster_created = true;
			orignal_name = gameScript2.GetNameSimple();
			BUTTON_RemasterName(0);
			g_myNameTeil1 = gameScript2.myNameTeil1;
			g_Beschreibung = gameScript2.beschreibung;
			g_GameTyp = gameScript2.gameTyp;
			g_GameZielgruppe = gameScript2.gameZielgruppe;
			g_GameMainGenre = gameScript2.maingenre;
			g_GameSubGenre = gameScript2.subgenre;
			g_GameMainTheme = gameScript2.gameMainTheme;
			g_GameSubTheme = gameScript2.gameSubTheme;
			g_GameSize = gameScript2.gameSize;
			if (g_GameSize == 1 && !forschungSonstiges_.IsErforscht(0))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 2 && !forschungSonstiges_.IsErforscht(1))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 3 && !forschungSonstiges_.IsErforscht(2))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 4 && !forschungSonstiges_.IsErforscht(3))
			{
				g_GameSize = 0;
			}
			if (g_GameSize == 5 && !forschungSonstiges_.IsErforscht(40))
			{
				g_GameSize = 0;
			}
			if (g_GameMainGenre != -1 && !genres_.IsErforscht(g_GameMainGenre))
			{
				g_GameMainGenre = -1;
			}
			if (g_GameSubGenre != -1 && !genres_.IsErforscht(g_GameSubGenre))
			{
				g_GameSubGenre = -1;
			}
			if (g_GameMainTheme != -1 && !themes_.IsErforscht(g_GameMainTheme))
			{
				g_GameMainTheme = -1;
			}
			if (g_GameSubTheme != -1 && !themes_.IsErforscht(g_GameSubTheme))
			{
				g_GameSubTheme = -1;
			}
			if (g_GameTyp == 1 && !unlock_.Get(21))
			{
				g_GameTyp = 0;
			}
			if (g_GameTyp == 2 && !unlock_.Get(22))
			{
				g_GameTyp = 0;
			}
			CopyDesignSettings(gameScript2);
		}
		Init(rscript_);
		typ_remaster = true;
		g_mainIP = gameScript2.mainIP;
		DrawIpStars();
		g_originalGameID = orgID_;
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[2];
		uiObjects[15].GetComponent<Button>().interactable = false;
		uiObjects[16].GetComponent<Button>().interactable = false;
		uiObjects[17].GetComponent<Button>().interactable = false;
		uiObjects[126].GetComponent<Button>().interactable = false;
		uiObjects[127].GetComponent<Button>().interactable = false;
		uiObjects[128].GetComponent<Button>().interactable = false;
		uiObjects[129].GetComponent<Button>().interactable = false;
		LockDesign(b: false);
		SetAndLockPlatformTyp(platTyp_);
		InitSondereinstellungen(uiObjects[146].GetComponent<Dropdown>().value);
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void InitContractGame(roomScript rscript_, gameScript contractGame_)
	{
		DeleteAllDatas();
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(598);
		if ((bool)contractGame_)
		{
			contractAuftragsspiel_ = contractGame_;
			uiObjects[0].GetComponent<InputField>().text = contractGame_.GetNameSimple();
			g_GameTyp = 0;
			g_GameSize = contractGame_.gameSize;
			g_GameMainGenre = contractGame_.maingenre;
			g_GamePlatform[0] = contractGame_.gamePlatform[0];
			if (contractGame_.portID != -1)
			{
				g_GameSubGenre = contractGame_.subgenre;
				g_GameMainTheme = contractGame_.gameMainTheme;
				g_GameSubTheme = contractGame_.gameSubTheme;
			}
		}
		Init(rscript_);
		g_mainIP = -1;
		DrawIpStars();
		typ_contractGame = true;
		typ_standard = true;
		uiObjects[123].GetComponent<Image>().sprite = games_.gameTypSprites[3];
		uiObjects[0].GetComponent<InputField>().interactable = false;
		uiObjects[17].GetComponent<Button>().interactable = false;
		uiObjects[126].GetComponent<Button>().interactable = false;
		uiObjects[128].GetComponent<Button>().interactable = false;
		uiObjects[124].GetComponent<Button>().interactable = false;
		uiObjects[139].GetComponent<Button>().interactable = false;
		uiObjects[140].GetComponent<Button>().interactable = false;
		uiObjects[33].GetComponent<Button>().interactable = false;
		uiObjects[34].GetComponent<Button>().interactable = false;
		uiObjects[35].GetComponent<Button>().interactable = false;
		uiObjects[146].GetComponent<Dropdown>().interactable = false;
		uiObjects[146].GetComponent<Dropdown>().value = 1;
		ResetDesignSettings();
		if (contractGame_.portID != -1)
		{
			uiObjects[15].GetComponent<Button>().interactable = false;
			uiObjects[16].GetComponent<Button>().interactable = false;
			uiObjects[17].GetComponent<Button>().interactable = false;
			uiObjects[126].GetComponent<Button>().interactable = false;
			uiObjects[127].GetComponent<Button>().interactable = false;
			uiObjects[128].GetComponent<Button>().interactable = false;
			uiObjects[129].GetComponent<Button>().interactable = false;
			CopyDesignSettings(contractGame_);
			LockDesign(b: false);
		}
		uiObjects[162].SetActive(value: true);
		uiObjects[185].SetActive(value: false);
		uiObjects[200].SetActive(value: true);
		g_InAppPurchase[0] = true;
		BUTTON_AlleInAppPurchase();
		uiObjects[167].SetActive(value: true);
		string text = tS_.GetText(605);
		text = text.Replace("<NUM>", contractGame_.auftragsspiel_zeitInWochen.ToString());
		uiObjects[163].GetComponent<Text>().text = text;
		text = tS_.GetText(626);
		text = text.Replace("<NUM>", contractGame_.auftragsspiel_mindestbewertung.ToString());
		uiObjects[164].GetComponent<Text>().text = text;
		uiObjects[165].GetComponent<Text>().text = tS_.GetText(600) + ": " + mS_.GetMoney(contractGame_.auftragsspiel_gehalt, showDollar: true);
		uiObjects[166].GetComponent<Text>().text = tS_.GetText(627) + ": " + mS_.GetMoney(contractGame_.auftragsspiel_bonus, showDollar: true);
		InitSondereinstellungen(uiObjects[146].GetComponent<Dropdown>().value);
		Init_GameplayFeatures();
		CalcDevCosts();
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		rS_ = script_;
		typ_standard = false;
		typ_nachfolger = false;
		typ_remaster = false;
		typ_addon = false;
		typ_bundle = false;
		typ_budget = false;
		typ_addonStandalone = false;
		typ_mmoaddon = false;
		typ_spinoff = false;
		Unlock(31, uiObjects[86], uiObjects[84]);
		Unlock(31, null, uiObjects[85]);
		forschungSonstiges_.Unlock(35, uiObjects[18], uiObjects[15]);
		forschungSonstiges_.Unlock(36, uiObjects[19], uiObjects[16]);
		Unlock(64, uiObjects[174], uiObjects[173]);
		Unlock(64, null, uiObjects[172]);
		Unlock(25, uiObjects[20], uiObjects[17]);
		Unlock(25, null, uiObjects[62]);
		Unlock(28, uiObjects[36], uiObjects[33]);
		Unlock(29, uiObjects[37], uiObjects[34]);
		Unlock(30, uiObjects[38], uiObjects[35]);
		uiObjects[200].SetActive(value: false);
		if (gF_.IsErforscht(57))
		{
			if (uiObjects[185].activeSelf)
			{
				uiObjects[185].SetActive(value: false);
			}
		}
		else if (!uiObjects[185].activeSelf)
		{
			uiObjects[185].SetActive(value: true);
		}
		uiObjects[186].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[0], 1) + "0";
		uiObjects[187].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[1], 1) + "0";
		uiObjects[188].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[2], 1) + "0";
		uiObjects[189].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[3], 1) + "0";
		uiObjects[190].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[4], 1) + "0";
		uiObjects[191].GetComponent<Text>().text = "$" + mS_.Round(games_.inAppPurchasePrice[5], 1) + "0";
		SetLeitenderDesigner(null, manuellSelectet: false);
		SetGameTyp(g_GameTyp);
		SetGameSize(g_GameSize);
		SetZielgruppe(g_GameZielgruppe);
		SetMainGenre(g_GameMainGenre);
		SetSubGenre(g_GameSubGenre);
		SetMainTheme(g_GameMainTheme);
		SetSubTheme(g_GameSubTheme);
		SetLicence(g_GameLicence);
		SetPlatform(0, g_GamePlatform[0]);
		SetPlatform(1, g_GamePlatform[1]);
		SetPlatform(2, g_GamePlatform[2]);
		SetPlatform(3, g_GamePlatform[3]);
		SetEngine(mS_.lastUsedEngine);
		SetCopyProtect(g_GameCopyProtect);
		SetAutomaticBestCopyProtect();
		SetAntiCheat(g_GameAntiCheat);
		SetAutomaticBestAntiCheat();
		UpdateDesignSettings();
		UpdateDesignSlider();
		SetAP_Gameplay();
		SetAP_Grafik();
		SetAP_Sound();
		SetAP_Technik();
		SetMaxVerdienstInApp();
		for (int i = 0; i < g_GameLanguage.Length; i++)
		{
			SetLanguage(i);
		}
		for (int j = 0; j < g_GameLanguage.Length; j++)
		{
			SetLanguage(j);
		}
		uiObjects[159].GetComponent<Slider>().value = 100f;
		uiObjects[160].GetComponent<Slider>().value = 100f;
		uiObjects[161].GetComponent<Slider>().value = 100f;
		g_finanzierung_Grundkosten = 100;
		g_finanzierung_Kontent = 100;
		g_finanzierung_Technology = 100;
		uiObjects[162].SetActive(value: false);
		SLIDER_Finanzierung(0);
		SLIDER_Finanzierung(1);
		SLIDER_Finanzierung(2);
		OpenSide(0);
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			if ((bool)lock_)
			{
				lock_.SetActive(value: false);
			}
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			if ((bool)lock_)
			{
				lock_.SetActive(value: true);
			}
		}
	}

	public void OpenSide(int i)
	{
		sfx_.PlaySound(3, force: false);
		UpdateDesignSettings();
		if (uiObjects[12].activeSelf && i != 0)
		{
			uiObjects[12].SetActive(value: false);
		}
		if (uiObjects[13].activeSelf && i != 1)
		{
			uiObjects[13].SetActive(value: false);
		}
		if (uiObjects[67].activeSelf && i != 2)
		{
			uiObjects[67].SetActive(value: false);
		}
		if (uiObjects[91].activeSelf && i != 3)
		{
			uiObjects[91].SetActive(value: false);
		}
		if (uiObjects[118].activeSelf && i != 4)
		{
			uiObjects[118].SetActive(value: false);
		}
		if (uiObjects[151].activeSelf && i != 5)
		{
			uiObjects[151].SetActive(value: false);
		}
		seite = i;
		for (int j = 0; j < uiObjects[32].transform.childCount; j++)
		{
			uiObjects[32].transform.GetChild(j).GetComponent<Image>().color = Color.white;
		}
		uiObjects[32].transform.GetChild(i).GetComponent<Image>().color = Color.grey;
		switch (i)
		{
		case 0:
			if (!uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: true);
			}
			break;
		case 1:
			if (!uiObjects[13].activeSelf)
			{
				uiObjects[13].SetActive(value: true);
			}
			break;
		case 2:
			if (!uiObjects[67].activeSelf)
			{
				uiObjects[67].SetActive(value: true);
			}
			break;
		case 3:
			if (!uiObjects[91].activeSelf)
			{
				uiObjects[91].SetActive(value: true);
			}
			break;
		case 4:
			if (!uiObjects[118].activeSelf)
			{
				uiObjects[118].SetActive(value: true);
				StartCoroutine(iDROPDOWN_SortGameplayFeatures());
			}
			break;
		case 5:
			if (!uiObjects[151].activeSelf)
			{
				uiObjects[151].SetActive(value: true);
			}
			break;
		}
	}

	private IEnumerator iDROPDOWN_SortGameplayFeatures()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		DROPDOWN_SortGameplayFeatures();
	}

	public void NextSide(int i)
	{
		seite += i;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > 5)
		{
			seite = 5;
		}
		OpenSide(seite);
		sfx_.PlaySound(3, force: true);
	}

	private bool AktivateGameplayFeature(int id_)
	{
		uiObjects[204].GetComponent<InputField>().text = "";
		BUTTON_Search();
		for (int i = 0; i < uiObjects[120].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[120].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_GameplayFeature component = gameObject.GetComponent<Item_DevGame_GameplayFeature>();
				if (component.myID == id_ && gF_.IsErforscht(id_))
				{
					component.BUTTON_Click();
					return true;
				}
			}
		}
		return false;
	}

	public void BUTTON_Beschreibung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[198]);
	}

	public void BUTTON_Spielkonzepte()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[109]);
	}

	public void BUTTON_Spielberichte()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[182]);
		guiMain_.uiObjects[182].GetComponent<Menu_QA_ShowSpielberichtSelectGame>().Init();
	}

	public void BUTTON_Fanbriefe()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[111]);
	}

	public void BUTTON_Marktumfrage()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[230]);
		guiMain_.uiObjects[230].GetComponent<Menu_Marktforschung>().Init(null);
	}

	public void BUTTON_Start()
	{
		sfx_.PlaySound(3, force: true);
		if (mS_.multiplayer && typ_contractGame && (bool)contractAuftragsspiel_ && !contractAuftragsspiel_.auftragsspiel)
		{
			guiMain_.MessageBox(tS_.GetText(1812), closeMenu: false);
			return;
		}
		if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(412), closeMenu: false);
			OpenSide(0);
			return;
		}
		if (!typ_contractGame && g_portIP == -1)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
			if (array.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					gameScript component = array[i].GetComponent<gameScript>();
					if ((bool)component && component.GetNameSimple() == uiObjects[0].GetComponent<InputField>().text)
					{
						guiMain_.MessageBox(tS_.GetText(618), closeMenu: false);
						OpenSide(0);
						return;
					}
				}
			}
		}
		if (g_GameMainGenre < 0)
		{
			guiMain_.MessageBox(tS_.GetText(401), closeMenu: false);
			OpenSide(0);
			return;
		}
		if (g_GameMainTheme < 0)
		{
			guiMain_.MessageBox(tS_.GetText(402), closeMenu: false);
			OpenSide(0);
			return;
		}
		if (g_GamePlatform[0] < 0)
		{
			guiMain_.MessageBox(tS_.GetText(403), closeMenu: false);
			OpenSide(1);
			return;
		}
		if (EngineFeatureToHighTechLevel())
		{
			guiMain_.MessageBox(tS_.GetText(1691), closeMenu: false);
			OpenSide(1);
			return;
		}
		if (UpdateGesamtArbeitsprioritaet() > 100)
		{
			guiMain_.MessageBox(tS_.GetText(400), closeMenu: false);
			OpenSide(3);
			return;
		}
		if (UpdateGesamtArbeitsprioritaet() < 100)
		{
			guiMain_.MessageBox(tS_.GetText(416), closeMenu: false);
			OpenSide(3);
			return;
		}
		if (AnzahlLanguages() <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(404), closeMenu: false);
			OpenSide(2);
			return;
		}
		if (GetAmountDesignschwerpunkte() > 0)
		{
			guiMain_.MessageBox(tS_.GetText(1464), closeMenu: false);
			OpenSide(3);
			return;
		}
		if (GetAmountDesignschwerpunkte() < 0)
		{
			guiMain_.MessageBox(tS_.GetText(1465), closeMenu: false);
			OpenSide(3);
			return;
		}
		if (UpdateGesamtGameplayFeatures() <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(846), closeMenu: false);
			OpenSide(4);
			return;
		}
		if (UpdateGesamtGameplayFeatures() > maxFeatures_gameSize[g_GameSize])
		{
			guiMain_.MessageBox(tS_.GetText(411), closeMenu: false);
			OpenSide(4);
			return;
		}
		if (g_GameSize > 0 && UpdateGesamtGameplayFeatures() < maxFeatures_gameSize[g_GameSize - 1] / 2)
		{
			string text = tS_.GetText(2070);
			text = text.Replace("<NAME>", tS_.GetGameSize(g_GameSize));
			text = text.Replace("<NUM>", (maxFeatures_gameSize[g_GameSize - 1] / 2).ToString());
			guiMain_.MessageBox(text, closeMenu: false);
			OpenSide(4);
			return;
		}
		bool flag = true;
		for (int j = 0; j < g_GamePlatform.Length; j++)
		{
			if (g_GamePlatform[j] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[j]);
			if ((bool)gameObject)
			{
				platformScript component2 = gameObject.GetComponent<platformScript>();
				if ((bool)component2 && !component2.internet)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			string text2 = "";
			for (int k = 0; k < g_GameGameplayFeatures.Length; k++)
			{
				if (g_GameGameplayFeatures[k] && gF_.gameplayFeatures_INTERNET[k])
				{
					text2 = text2 + "\n<color=blue>" + gF_.GetName(k) + "</color>";
				}
			}
			if (text2.Length > 0)
			{
				guiMain_.MessageBox(tS_.GetText(2198) + text2, closeMenu: false);
				OpenSide(4);
				return;
			}
		}
		for (int l = 0; l < g_GameGameplayFeatures.Length; l++)
		{
			if (g_GameGameplayFeatures[l] && gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[l] != -1 && !g_GameGameplayFeatures[gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[l]])
			{
				string text3 = tS_.GetText(2197);
				text3 = text3.Replace("<NAME1>", "<color=blue>" + gF_.GetName(l) + "</color>");
				text3 = text3.Replace("<NAME2>", "<color=blue>" + gF_.GetName(gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[l]) + "</color>");
				if (AktivateGameplayFeature(gF_.gameplayFeatures_NEED_GAMEPLAY_FEATURE[l]))
				{
					text3 = text3 + "\n\n<color=green>[" + tS_.GetText(2014) + "]</color>\n";
				}
				guiMain_.MessageBox(text3, closeMenu: false);
				OpenSide(4);
				return;
			}
		}
		if ((g_GameTyp == 1 || g_GameTyp == 2) && !g_GameGameplayFeatures[23])
		{
			guiMain_.MessageBox(tS_.GetText(1235), closeMenu: false);
			OpenSide(4);
			return;
		}
		if (g_GameTyp == 2 && SetMaxVerdienstInApp() <= 0f)
		{
			guiMain_.MessageBox(tS_.GetText(1388), closeMenu: false);
			OpenSide(5);
			return;
		}
		if (g_GameTyp == 2 && (!g_GameGameplayFeatures[57] || !g_GameGameplayFeatures[23]))
		{
			guiMain_.MessageBox(tS_.GetText(2380), closeMenu: false);
			OpenSide(5);
			return;
		}
		string text4 = "";
		for (int m = 0; m < g_GamePlatform.Length; m++)
		{
			if (g_GamePlatform[m] == -1)
			{
				continue;
			}
			GameObject gameObject2 = GameObject.Find("PLATFORM_" + g_GamePlatform[m]);
			if (!gameObject2)
			{
				continue;
			}
			platformScript component3 = gameObject2.GetComponent<platformScript>();
			if ((bool)component3)
			{
				if (component3.needFeatures[0] != -1 && !g_GameGameplayFeatures[component3.needFeatures[0]])
				{
					text4 = ((!AktivateGameplayFeature(component3.needFeatures[0])) ? (text4 + "<color=red>[" + tS_.GetText(2015) + "]</color>\n") : (text4 + "<color=green>[" + tS_.GetText(2014) + "]</color>\n"));
					text4 = text4 + component3.GetName() + ": " + gF_.GetName(component3.needFeatures[0]) + "\n\n";
				}
				if (component3.needFeatures[1] != -1 && !g_GameGameplayFeatures[component3.needFeatures[1]])
				{
					text4 = ((!AktivateGameplayFeature(component3.needFeatures[1])) ? (text4 + "<color=red>[" + tS_.GetText(2015) + "]</color>\n") : (text4 + "<color=green>[" + tS_.GetText(2014) + "]</color>\n"));
					text4 = text4 + component3.GetName() + ": " + gF_.GetName(component3.needFeatures[1]) + "\n\n";
				}
				if (component3.needFeatures[2] != -1 && !g_GameGameplayFeatures[component3.needFeatures[2]])
				{
					text4 = ((!AktivateGameplayFeature(component3.needFeatures[2])) ? (text4 + "<color=red>[" + tS_.GetText(2015) + "]</color>\n") : (text4 + "<color=green>[" + tS_.GetText(2014) + "]</color>\n"));
					text4 = text4 + component3.GetName() + ": " + gF_.GetName(component3.needFeatures[2]) + "\n\n";
				}
				if ((g_GameTyp == 1 || g_GameTyp == 2) && !component3.internet)
				{
					guiMain_.MessageBox(tS_.GetText(1262), closeMenu: false);
					OpenSide(1);
					return;
				}
			}
		}
		if (text4.Length > 0)
		{
			guiMain_.MessageBox(tS_.GetText(1020) + "\n\n<color=blue>" + text4 + "</color>", closeMenu: false);
			OpenSide(4);
			return;
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 3)
		{
			for (int n = 0; n < g_GamePlatform.Length; n++)
			{
				if (g_GamePlatform[n] == -1)
				{
					continue;
				}
				GameObject gameObject3 = GameObject.Find("PLATFORM_" + g_GamePlatform[n]);
				if ((bool)gameObject3)
				{
					platformScript component4 = gameObject3.GetComponent<platformScript>();
					if ((bool)component4 && !component4.vomMarktGenommen)
					{
						guiMain_.MessageBox(tS_.GetText(906), closeMenu: false);
						OpenSide(1);
						return;
					}
				}
			}
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 2)
		{
			for (int num = 0; num < g_GamePlatform.Length; num++)
			{
				if (g_GamePlatform[num] == -1)
				{
					continue;
				}
				GameObject gameObject4 = GameObject.Find("PLATFORM_" + g_GamePlatform[num]);
				if ((bool)gameObject4)
				{
					platformScript component5 = gameObject4.GetComponent<platformScript>();
					if ((bool)component5 && component5.ownerID != mS_.myID)
					{
						guiMain_.MessageBox(tS_.GetText(1696), closeMenu: false);
						OpenSide(1);
						return;
					}
				}
			}
		}
		int num2 = CalcDevCosts();
		mS_.Pay(num2, 10);
		gameScript gameScript2 = null;
		if (!typ_contractGame)
		{
			gameScript2 = games_.CreateNewGame(fromSavegame: false, setDate: true);
			gameScript2.ownerID = mS_.myID;
		}
		else
		{
			gameScript2 = contractAuftragsspiel_;
		}
		gameScript2.costs_entwicklung = num2;
		gameScript2.inDevelopment = true;
		gameScript2.developerID = mS_.myID;
		gameScript2.devS_ = mS_.myPubS_;
		gameScript2.typ_contractGame = typ_contractGame;
		if (!typ_contractGame)
		{
			gameScript2.SetMyName(uiObjects[0].GetComponent<InputField>().text);
			gameScript2.myNameTeil1 = g_myNameTeil1;
			gameScript2.beschreibung = g_Beschreibung;
			gameScript2.myNameTeil1 = g_myNameTeil1;
			gameScript2.typ_standard = typ_standard;
			gameScript2.typ_nachfolger = typ_nachfolger;
			gameScript2.typ_spinoff = typ_spinoff;
			gameScript2.typ_remaster = typ_remaster;
			gameScript2.typ_addon = typ_addon;
			gameScript2.typ_bundle = typ_bundle;
			gameScript2.typ_budget = typ_budget;
			gameScript2.typ_addonStandalone = false;
			gameScript2.typ_mmoaddon = false;
			if (uiObjects[146].GetComponent<Dropdown>().value == 1)
			{
				gameScript2.exklusiv = true;
			}
			if (uiObjects[146].GetComponent<Dropdown>().value == 2)
			{
				gameScript2.herstellerExklusiv = true;
			}
			if (uiObjects[146].GetComponent<Dropdown>().value == 3)
			{
				gameScript2.retro = true;
			}
			if (uiObjects[146].GetComponent<Dropdown>().value == 4)
			{
				gameScript2.arcade = true;
			}
			if (uiObjects[146].GetComponent<Dropdown>().value == 5)
			{
				gameScript2.handy = true;
			}
			gameScript2.mainIP = g_mainIP;
			if (gameScript2.mainIP == -1)
			{
				gameScript2.mainIP = gameScript2.myID;
				if (mS_.settings_randomEvents != 2)
				{
					if (!gameScript2.arcade && Random.Range(0, 100) < mS_.difficulty && mS_.difficulty >= 3)
					{
						if (!mS_.lastGameCommercialFlop)
						{
							gameScript2.commercialFlop = true;
							mS_.lastGameCommercialFlop = true;
						}
						else
						{
							mS_.lastGameCommercialFlop = false;
						}
					}
					if (!gameScript2.commercialFlop && !gameScript2.handy && !gameScript2.typ_addon && !gameScript2.typ_budget && !gameScript2.typ_bundleAddon && !gameScript2.typ_contractGame && !gameScript2.typ_goty && !gameScript2.typ_mmoaddon && Random.Range(0, 100) == 1)
					{
						gameScript2.commercialHit = true;
					}
				}
			}
		}
		if (!gameScript2.typ_contractGame)
		{
			if (games_.IsNewGenreCombination(g_GameMainGenre, g_GameSubGenre))
			{
				gameScript2.newGenreCombination = true;
			}
			if (games_.IsNewTopicCombination(g_GameMainTheme, g_GameSubTheme))
			{
				gameScript2.newTopicCombination = true;
			}
		}
		if (!typ_contractGame)
		{
			gameScript2.originalGameID = g_originalGameID;
			gameScript2.portID = g_portIP;
			games_.SetPortFlags(gameScript2);
			gameScript2.teile = g_teil;
		}
		gameScript2.gameTyp = g_GameTyp;
		gameScript2.gameSize = g_GameSize;
		gameScript2.gameZielgruppe = g_GameZielgruppe;
		gameScript2.maingenre = g_GameMainGenre;
		gameScript2.subgenre = g_GameSubGenre;
		gameScript2.gameMainTheme = g_GameMainTheme;
		themes_.AddUses(g_GameMainTheme);
		gameScript2.gameSubTheme = g_GameSubTheme;
		themes_.AddUses(g_GameSubTheme);
		gameScript2.gameLicence = g_GameLicence;
		gameScript2.engineID = g_GameEngine;
		if (g_GameEngineScript_.ownerID != mS_.myID)
		{
			gameScript2.engineGewinnbeteiligung = g_GameEngineScript_.gewinnbeteiligung;
		}
		gameScript2.gameCopyProtect = g_GameCopyProtect;
		gameScript2.gameAntiCheat = g_GameAntiCheat;
		if (g_GameLicence != -1 && gameScript2.portID == -1 && !gameScript2.typ_addon && !gameScript2.typ_mmoaddon)
		{
			if (licences_.licence_GEKAUFT[g_GameLicence] > 0)
			{
				licences_.licence_GEKAUFT[g_GameLicence]--;
			}
			else
			{
				gameScript2.gameLicence = -1;
			}
		}
		for (int num3 = 0; num3 < g_Designschwerpunkt.Length; num3++)
		{
			gameScript2.Designschwerpunkt[num3] = g_Designschwerpunkt[num3];
		}
		for (int num4 = 0; num4 < g_Designausrichtung.Length; num4++)
		{
			gameScript2.Designausrichtung[num4] = g_Designausrichtung[num4];
		}
		gameScript2.gameAP_Gameplay = g_GameAP_Gameplay;
		gameScript2.gameAP_Grafik = g_GameAP_Grafik;
		gameScript2.gameAP_Sound = g_GameAP_Sound;
		gameScript2.gameAP_Technik = g_GameAP_Technik;
		gameScript2.finanzierung_Grundkosten = g_finanzierung_Grundkosten;
		gameScript2.finanzierung_Technology = g_finanzierung_Technology;
		gameScript2.finanzierung_Kontent = g_finanzierung_Kontent;
		for (int num5 = 0; num5 < g_InAppPurchase.Length; num5++)
		{
			gameScript2.inAppPurchase[num5] = g_InAppPurchase[num5];
		}
		SetMGSR_Result(gameScript2, g_Designausrichtung[1]);
		for (int num6 = 0; num6 < g_GameLanguage.Length; num6++)
		{
			gameScript2.gameLanguage[num6] = g_GameLanguage[num6];
		}
		for (int num7 = 0; num7 < g_GameGameplayFeatures.Length; num7++)
		{
			gameScript2.gameGameplayFeatures[num7] = g_GameGameplayFeatures[num7];
		}
		for (int num8 = 0; num8 < g_GamePlatform.Length; num8++)
		{
			gameScript2.gamePlatform[num8] = g_GamePlatform[num8];
		}
		for (int num9 = 0; num9 < g_GameEngineFeature.Length; num9++)
		{
			gameScript2.gameEngineFeature[num9] = g_GameEngineFeature[num9];
		}
		if (typ_contractGame)
		{
			gameScript2.auftragsspiel = false;
			gameScript2.points_gameplay = 0f;
			gameScript2.points_grafik = 0f;
			gameScript2.points_sound = 0f;
			gameScript2.points_technik = 0f;
			gameScript2.points_bugs = 0f;
			gameScript2.points_bugsInvis = 0f;
			gameScript2.hype = 0f;
			gameScript2.costs_mitarbeiter = 0L;
			gameScript2.costs_marketing = 0L;
			gameScript2.costs_enginegebuehren = 0L;
			gameScript2.costs_server = 0L;
			gameScript2.costs_production = 0L;
			gameScript2.costs_updates = 0L;
			for (int num10 = 0; num10 < gameScript2.gameplayStudio.Length; num10++)
			{
				gameScript2.gameplayStudio[num10] = false;
			}
			for (int num11 = 0; num11 < gameScript2.grafikStudio.Length; num11++)
			{
				gameScript2.grafikStudio[num11] = false;
			}
			for (int num12 = 0; num12 < gameScript2.soundStudio.Length; num12++)
			{
				gameScript2.soundStudio[num12] = false;
			}
			for (int num13 = 0; num13 < gameScript2.motionCaptureStudio.Length; num13++)
			{
				gameScript2.motionCaptureStudio[num13] = false;
			}
			for (int num14 = 0; num14 < gameScript2.gameplayFeatures_DevDone.Length; num14++)
			{
				gameScript2.gameplayFeatures_DevDone[num14] = false;
			}
			for (int num15 = 0; num15 < gameScript2.engineFeature_DevDone.Length; num15++)
			{
				gameScript2.engineFeature_DevDone[num15] = false;
			}
		}
		gameScript2.devPointsStart_Gesamt = gameScript2.GetGesamtDevPoints();
		gameScript2.devPoints_Gesamt = gameScript2.devPointsStart_Gesamt;
		gameScript2.FindNextFeatureForDevelopment();
		taskGame taskGame2 = guiMain_.AddTask_Game();
		taskGame2.Init(fromSavegame: false);
		taskGame2.gameID = gameScript2.myID;
		if ((bool)g_leitenderDesigner)
		{
			taskGame2.leitenderDesignerID = g_leitenderDesigner.myID;
			taskGame2.designer_ = g_leitenderDesigner;
		}
		rS_.taskID = taskGame2.myID;
		if (typ_contractGame && (bool)contractAuftragsspiel_)
		{
			guiMain_.uiObjects[100].SetActive(value: true);
			guiMain_.uiObjects[100].GetComponent<Menu_Dev_AuftragsSpielGehalt>().Init(contractAuftragsspiel_);
		}
		if (!gameScript2.typ_contractGame)
		{
			if (gameScript2.portID == -1)
			{
				if (typ_nachfolger)
				{
					guiMain_.OpenMenu(hideChars: false);
					guiMain_.uiObjects[233].SetActive(value: true);
					guiMain_.uiObjects[233].GetComponent<Menu_Dev_NachfolgerHype>().Init(gameScript2);
				}
				if (typ_spinoff)
				{
					guiMain_.OpenMenu(hideChars: false);
					guiMain_.uiObjects[311].SetActive(value: true);
					guiMain_.uiObjects[311].GetComponent<Menu_Dev_SpinoffHype>().Init(gameScript2);
				}
			}
			else
			{
				guiMain_.OpenMenu(hideChars: false);
				guiMain_.uiObjects[314].SetActive(value: true);
				guiMain_.uiObjects[314].GetComponent<Menu_Dev_PortHype>().Init(gameScript2);
			}
		}
		DeleteAllDatas();
		ResetDesignSettings();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(12);
		}
		if (guiMain_.uiObjects[312].activeSelf)
		{
			guiMain_.uiObjects[312].SetActive(value: false);
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void DeleteAllDatas()
	{
		Debug.Log("GameConcept: DeleteAllDatas()");
		typ_standard = false;
		typ_nachfolger = false;
		typ_remaster = false;
		typ_contractGame = false;
		typ_addon = false;
		typ_bundle = false;
		typ_budget = false;
		typ_addonStandalone = false;
		typ_mmoaddon = false;
		typ_spinoff = false;
		g_myNameTeil1 = "";
		g_Beschreibung = "";
		g_leitenderDesigner = null;
		g_GameTyp = 0;
		g_GameSize = 0;
		g_GameZielgruppe = 4;
		g_GameMainGenre = -1;
		g_GameSubGenre = -1;
		g_GameMainTheme = -1;
		g_GameSubTheme = -1;
		g_GameLicence = -1;
		g_GameEngine = 0;
		g_mainIP = -1;
		g_originalGameID = -1;
		g_portIP = -1;
		g_teil = 1;
		g_GameCopyProtect = -1;
		g_GameAntiCheat = -1;
		g_GameAP_Gameplay = 0;
		g_GameAP_Grafik = 0;
		g_GameAP_Sound = 0;
		g_GameAP_Technik = 0;
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			g_Designschwerpunkt[i] = 5;
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			g_Designausrichtung[j] = 5;
		}
		for (int k = 0; k < g_GameLanguage.Length; k++)
		{
			g_GameLanguage[k] = false;
		}
		for (int l = 0; l < g_GameGameplayFeatures.Length; l++)
		{
			g_GameGameplayFeatures[l] = false;
		}
		for (int m = 0; m < g_GamePlatform.Length; m++)
		{
			g_GamePlatform[m] = -1;
		}
		for (int n = 0; n < g_GameEngineFeature.Length; n++)
		{
			g_GameEngineFeature[n] = -1;
		}
		for (int num = 0; num < g_InAppPurchase.Length; num++)
		{
			g_InAppPurchase[num] = false;
		}
		uiObjects[0].GetComponent<InputField>().text = "";
	}

	public void BUTTON_Abbrechen()
	{
		if (typ_spinoff)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[57]);
			DeleteAllDatas();
		}
		if (typ_nachfolger)
		{
			if (g_originalGameID != -1 && g_portIP == -1)
			{
				GameObject gameObject = GameObject.Find("GAME_" + g_originalGameID);
				if ((bool)gameObject)
				{
					gameObject.GetComponent<gameScript>().nachfolger_created = false;
				}
			}
			guiMain_.ActivateMenu(guiMain_.uiObjects[57]);
			DeleteAllDatas();
		}
		if (typ_remaster)
		{
			if (g_originalGameID != -1)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + g_originalGameID);
				if ((bool)gameObject2)
				{
					gameObject2.GetComponent<gameScript>().remaster_created = false;
				}
			}
			guiMain_.ActivateMenu(guiMain_.uiObjects[57]);
			DeleteAllDatas();
		}
		if (typ_contractGame)
		{
			if ((bool)contractAuftragsspiel_ && mS_.multiplayer && (bool)mS_.mpCalls_)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_BlockContractGame(contractAuftragsspiel_, block_: false);
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_BlockContractGame(contractAuftragsspiel_, block_: false);
				}
			}
			guiMain_.ActivateMenu(guiMain_.uiObjects[99]);
			guiMain_.uiObjects[99].GetComponent<Menu_Dev_Auftragsspiel>().Init(rS_);
			DeleteAllDatas();
		}
		if (typ_standard)
		{
			guiMain_.uiObjects[57].SetActive(value: true);
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_AllGameplayFeatures()
	{
		bool flag = false;
		for (int i = 1; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i])
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			DisableAllGameplayFeatures();
			return;
		}
		for (int j = 0; j < uiObjects[120].transform.childCount; j++)
		{
			GameObject gameObject = uiObjects[120].transform.GetChild(j).gameObject;
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Item_DevGame_GameplayFeature>().BUTTON_Click();
			}
		}
	}

	public void BUTTON_AllPassendenGameplayFeatures()
	{
		if (g_GameMainGenre < 0)
		{
			guiMain_.MessageBox(tS_.GetText(2048), closeMenu: false);
			return;
		}
		bool flag = false;
		for (int i = 1; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i])
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			DisableAllGameplayFeatures();
			return;
		}
		for (int j = 0; j < uiObjects[120].transform.childCount; j++)
		{
			GameObject gameObject = uiObjects[120].transform.GetChild(j).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_GameplayFeature component = gameObject.GetComponent<Item_DevGame_GameplayFeature>();
				if (gF_.GetBonus(component.myID, g_GameMainGenre, g_GameSubGenre) > 0.9f && gF_.gameplayFeatures_GAMEPLAY[component.myID] >= 0)
				{
					component.BUTTON_Click();
				}
			}
		}
	}

	public void DisableAllGameplayFeatures()
	{
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			g_GameGameplayFeatures[i] = false;
		}
		CalcDevCosts();
		GetGesamtDevPoints();
		UpdateGesamtGameplayFeatures();
		sfx_.PlaySound(3, force: true);
	}

	public void BUTTON_RandomGameName()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[0].GetComponent<InputField>().text = tS_.GetRandomGameName();
	}

	public void BUTTON_LeitenderEntwickler()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[196]);
		guiMain_.uiObjects[196].GetComponent<Menu_Dev_LeitenderDesigner>().Init(rS_);
	}

	public void BUTTON_GameTyp()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[58]);
	}

	public void BUTTON_GameSize()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[59]);
	}

	public void BUTTON_Zielgruppe()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[60]);
	}

	public void BUTTON_Genre(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[61]);
		guiMain_.uiObjects[61].GetComponent<Menu_DevGame_Genre>().Init(i);
	}

	public void BUTTON_Thema(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[62]);
		guiMain_.uiObjects[62].GetComponent<Menu_DevGame_Theme>().Init(i);
	}

	public void BUTTON_Platform(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[66]);
		guiMain_.uiObjects[66].GetComponent<Menu_DevGame_Platform>().Init(i);
	}

	public void BUTTON_EngineFeature(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[67]);
		guiMain_.uiObjects[67].GetComponent<Menu_DevGame_EngineFeature>().Init(i);
	}

	public void BUTTON_Lizenz()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[63]);
	}

	public void BUTTON_Engine()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[65]);
	}

	public void BUTTON_EngineKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[42]);
	}

	public void BUTTON_PlatformKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[33]);
	}

	public void BUTTON_LizenzKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[51]);
	}

	public void BUTTON_CopyProtect()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[68]);
	}

	public void BUTTON_CopyProtectKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[49]);
	}

	public void BUTTON_AntiCheat()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[236]);
	}

	public void BUTTON_AntiCheatKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[234]);
	}

	public void BUTTON_AlleSprachen()
	{
		sfx_.PlaySound(3, force: true);
		bool flag = g_GameLanguage[0];
		for (int i = 0; i < g_GameLanguage.Length; i++)
		{
			g_GameLanguage[i] = flag;
			SetLanguage(i);
		}
	}

	public void DROPDOWN_PlattformTyp()
	{
		if (uiObjects[146].GetComponent<Dropdown>().value == 1 || uiObjects[146].GetComponent<Dropdown>().value == 4)
		{
			SetPlatform(1, -1);
			SetPlatform(2, -1);
			SetPlatform(3, -1);
			uiObjects[33].GetComponent<Button>().interactable = false;
			uiObjects[34].GetComponent<Button>().interactable = false;
			uiObjects[35].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[33].GetComponent<Button>().interactable = true;
			uiObjects[34].GetComponent<Button>().interactable = true;
			uiObjects[35].GetComponent<Button>().interactable = true;
			Unlock(28, uiObjects[36], uiObjects[33]);
			Unlock(29, uiObjects[37], uiObjects[34]);
			Unlock(30, uiObjects[38], uiObjects[35]);
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 0 || uiObjects[146].GetComponent<Dropdown>().value == 1 || uiObjects[146].GetComponent<Dropdown>().value == 2)
		{
			for (int i = 0; i < g_GamePlatform.Length; i++)
			{
				if (g_GamePlatform[i] != -1)
				{
					GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
					if ((bool)gameObject && (gameObject.GetComponent<platformScript>().typ == 3 || gameObject.GetComponent<platformScript>().typ == 4))
					{
						SetPlatform(i, -1);
					}
				}
			}
			CalcDevCosts();
		}
		else if (uiObjects[146].GetComponent<Dropdown>().value == 3)
		{
			for (int j = 0; j < g_GamePlatform.Length; j++)
			{
				if (g_GamePlatform[j] == -1)
				{
					continue;
				}
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + g_GamePlatform[j]);
				if ((bool)gameObject2)
				{
					if (!gameObject2.GetComponent<platformScript>().vomMarktGenommen)
					{
						SetPlatform(j, -1);
					}
					if (gameObject2.GetComponent<platformScript>().typ == 3 || gameObject2.GetComponent<platformScript>().typ == 4)
					{
						SetPlatform(j, -1);
					}
				}
			}
			CalcDevCosts();
		}
		else if (uiObjects[146].GetComponent<Dropdown>().value == 5)
		{
			for (int k = 0; k < g_GamePlatform.Length; k++)
			{
				if (g_GamePlatform[k] != -1)
				{
					GameObject gameObject3 = GameObject.Find("PLATFORM_" + g_GamePlatform[k]);
					if ((bool)gameObject3 && gameObject3.GetComponent<platformScript>().typ != 3)
					{
						SetPlatform(k, -1);
					}
				}
			}
			CalcDevCosts();
		}
		else
		{
			if (uiObjects[146].GetComponent<Dropdown>().value != 4)
			{
				return;
			}
			for (int l = 0; l < g_GamePlatform.Length; l++)
			{
				if (g_GamePlatform[l] != -1)
				{
					GameObject gameObject4 = GameObject.Find("PLATFORM_" + g_GamePlatform[l]);
					if ((bool)gameObject4 && gameObject4.GetComponent<platformScript>().typ != 4)
					{
						SetPlatform(l, -1);
					}
				}
			}
			CalcDevCosts();
		}
	}

	public int GetEngineTechLevel()
	{
		if ((bool)g_GameEngineScript_)
		{
			return g_GameEngineScript_.GetTechLevel();
		}
		return 0;
	}

	public void SetEngine(int i)
	{
		g_GameEngine = i;
		GameObject gameObject = GameObject.Find("ENGINE_" + i);
		if ((bool)gameObject)
		{
			mS_.lastUsedEngine = i;
			engineScript engineScript2 = (g_GameEngineScript_ = gameObject.GetComponent<engineScript>());
			uiObjects[63].GetComponent<Text>().text = engineScript2.GetName();
			uiObjects[64].GetComponent<Text>().text = engineScript2.GetTechLevel().ToString();
			uiObjects[216].GetComponent<Text>().text = engineScript2.GetTechLevel().ToString();
			if (engineScript2.ownerID != mS_.myID)
			{
				uiObjects[65].GetComponent<Text>().text = tS_.GetText(260) + ": " + engineScript2.gewinnbeteiligung + "%";
			}
			else
			{
				uiObjects[65].GetComponent<Text>().text = tS_.GetText(262);
			}
			uiObjects[66].GetComponent<Image>().sprite = genres_.GetPic(engineScript2.spezialgenre);
			engineScript2.SetSpezialPlatformSprite(uiObjects[179]);
			SetEngineFeature(eF_.GetTypGrafik(), g_GameEngineScript_.GetBestFeature(eF_.GetTypGrafik()));
			SetEngineFeature(eF_.GetTypSound(), g_GameEngineScript_.GetBestFeature(eF_.GetTypSound()));
			SetEngineFeature(eF_.GetTypKI(), g_GameEngineScript_.GetBestFeature(eF_.GetTypKI()));
			SetEngineFeature(eF_.GetTypPhysik(), g_GameEngineScript_.GetBestFeature(eF_.GetTypPhysik()));
		}
		else
		{
			g_GameEngineScript_ = null;
		}
		CalcDevCosts();
	}

	public void SetEngineFeatureSimple(int featureArt_, int featureNr_)
	{
		if (featureNr_ >= 0)
		{
			uiObjects[68 + featureArt_].GetComponent<Text>().text = eF_.GetName(featureNr_);
			guiMain_.DrawStars(uiObjects[72 + featureArt_], eF_.engineFeatures_LEVEL[featureNr_]);
			uiObjects[76 + featureArt_].GetComponent<Text>().text = eF_.engineFeatures_TECH[featureNr_].ToString();
			uiObjects[80 + featureArt_].GetComponent<tooltip>().c = eF_.GetTooltip(featureNr_);
			g_GameEngineFeature[featureArt_] = featureNr_;
		}
	}

	public void SetEngineFeature(int featureArt_, int featureNr_)
	{
		if (featureNr_ >= 0)
		{
			uiObjects[68 + featureArt_].GetComponent<Text>().text = eF_.GetName(featureNr_);
			guiMain_.DrawStars(uiObjects[72 + featureArt_], eF_.engineFeatures_LEVEL[featureNr_]);
			uiObjects[76 + featureArt_].GetComponent<Text>().text = eF_.engineFeatures_TECH[featureNr_].ToString();
			uiObjects[80 + featureArt_].GetComponent<tooltip>().c = eF_.GetTooltip(featureNr_);
			g_GameEngineFeature[featureArt_] = featureNr_;
		}
		GetGesamtDevPoints();
		CalcDevCosts();
	}

	public void SetBeschreibung(string c)
	{
		g_Beschreibung = c;
	}

	public void SetGameTyp(int i)
	{
		g_GameTyp = i;
		uiObjects[1].GetComponent<Text>().text = tS_.GetGameTyp(i);
		uiObjects[149].GetComponent<Image>().sprite = GetTypSprite();
		CalcDevCosts();
	}

	public void SetGameSize(int i)
	{
		g_GameSize = i;
		uiObjects[3].GetComponent<Text>().text = tS_.GetGameSize(i);
		CalcDevCosts();
		UpdateGesamtGameplayFeatures();
	}

	public void SetZielgruppe(int i)
	{
		g_GameZielgruppe = i;
		uiObjects[4].GetComponent<Text>().text = tS_.GetGameZielgruppe(i);
	}

	private void SetAutomaticBestAntiCheat()
	{
		if (uiObjects[146].GetComponent<Dropdown>().value == 4)
		{
			SetAntiCheat(-1);
		}
		else
		{
			if (g_GameAntiCheat != -1)
			{
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
					if ((bool)component && component.inBesitz && component.effekt > num)
					{
						num2 = component.myID;
						num = component.effekt;
					}
				}
			}
			if (num2 != -1)
			{
				SetAntiCheat(num2);
			}
		}
	}

	public void SetAntiCheat(int i)
	{
		g_GameAntiCheat = i;
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("ANTICHEAT_" + i);
			if ((bool)gameObject)
			{
				antiCheatScript antiCheatScript2 = (g_GameAntiCheatScript_ = gameObject.GetComponent<antiCheatScript>());
				uiObjects[175].GetComponent<Text>().text = antiCheatScript2.GetName();
				uiObjects[176].GetComponent<Text>().text = mS_.GetMoney(antiCheatScript2.GetDevCosts(), showDollar: true);
				uiObjects[177].GetComponent<Image>().fillAmount = antiCheatScript2.effekt * 0.01f;
				uiObjects[178].GetComponent<Text>().text = mS_.Round(antiCheatScript2.effekt, 2) + "%";
				uiObjects[177].GetComponent<Image>().color = GetValColor(antiCheatScript2.effekt);
			}
		}
		else
		{
			g_GameAntiCheatScript_ = null;
			uiObjects[175].GetComponent<Text>().text = tS_.GetText(1213);
			uiObjects[176].GetComponent<Text>().text = "";
			uiObjects[177].GetComponent<Image>().fillAmount = 0f;
			uiObjects[178].GetComponent<Text>().text = "0.0%";
			uiObjects[177].GetComponent<Image>().color = GetValColor(0f);
		}
		CalcDevCosts();
	}

	private void SetAutomaticBestCopyProtect()
	{
		if (uiObjects[146].GetComponent<Dropdown>().value == 4)
		{
			SetCopyProtect(-1);
		}
		else
		{
			if (g_GameCopyProtect != -1)
			{
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
					if ((bool)component && component.inBesitz && component.effekt > num)
					{
						num2 = component.myID;
						num = component.effekt;
					}
				}
			}
			if (num2 != -1)
			{
				SetCopyProtect(num2);
			}
		}
	}

	private void CheckNewSoftware()
	{
		checkSoftwareTimer += Time.deltaTime;
		if ((double)checkSoftwareTimer < 1.0)
		{
			return;
		}
		checkSoftwareTimer = 0f;
		if (mS_.IsBetterCopyProtect() && uiObjects[84].GetComponent<Button>().interactable)
		{
			if (!uiObjects[229].activeSelf)
			{
				uiObjects[229].SetActive(value: true);
			}
		}
		else if (uiObjects[229].activeSelf)
		{
			uiObjects[229].SetActive(value: false);
		}
		if (mS_.IsBetterAntiCheat() && uiObjects[173].GetComponent<Button>().interactable)
		{
			if (!uiObjects[230].activeSelf)
			{
				uiObjects[230].SetActive(value: true);
			}
		}
		else if (uiObjects[230].activeSelf)
		{
			uiObjects[230].SetActive(value: false);
		}
	}

	public void SetCopyProtect(int i)
	{
		g_GameCopyProtect = i;
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("COPYPROTECT_" + i);
			if ((bool)gameObject)
			{
				copyProtectScript copyProtectScript2 = (g_GameCopyProtectScript_ = gameObject.GetComponent<copyProtectScript>());
				uiObjects[87].GetComponent<Text>().text = copyProtectScript2.GetName();
				uiObjects[88].GetComponent<Text>().text = mS_.GetMoney(copyProtectScript2.GetDevCosts(), showDollar: true);
				uiObjects[89].GetComponent<Image>().fillAmount = copyProtectScript2.effekt * 0.01f;
				uiObjects[90].GetComponent<Text>().text = mS_.Round(copyProtectScript2.effekt, 2) + "%";
				uiObjects[89].GetComponent<Image>().color = GetValColor(copyProtectScript2.effekt);
			}
		}
		else
		{
			g_GameCopyProtectScript_ = null;
			uiObjects[87].GetComponent<Text>().text = tS_.GetText(383);
			uiObjects[88].GetComponent<Text>().text = "";
			uiObjects[89].GetComponent<Image>().fillAmount = 0f;
			uiObjects[90].GetComponent<Text>().text = "0.0%";
			uiObjects[89].GetComponent<Image>().color = GetValColor(0f);
		}
		CalcDevCosts();
	}

	public void SetMainGenre(int i)
	{
		g_GameMainGenre = i;
		if (i >= 0)
		{
			uiObjects[5].GetComponent<Text>().text = genres_.GetName(i);
			guiMain_.DrawStars(uiObjects[7], genres_.genres_LEVEL[i]);
			uiObjects[9].GetComponent<Image>().sprite = genres_.GetPic(i);
			uiObjects[11].GetComponent<Image>().sprite = guiMain_.uiSprites[1];
			uiObjects[9].GetComponent<tooltip>().c = genres_.GetTooltip(i);
			uiObjects[218].GetComponent<Image>().sprite = genres_.GetPic(i);
			uiObjects[218].GetComponent<tooltip>().c = genres_.GetTooltip(i);
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[7], 0);
			uiObjects[9].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[11].GetComponent<Image>().sprite = guiMain_.uiSprites[0];
			uiObjects[9].GetComponent<tooltip>().c = "";
			uiObjects[218].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[218].GetComponent<tooltip>().c = "";
		}
		if (games_.IsNewGenreCombination(g_GameMainGenre, g_GameSubGenre) && !typ_contractGame)
		{
			if (!uiObjects[220].activeSelf)
			{
				uiObjects[220].SetActive(value: true);
			}
		}
		else if (uiObjects[220].activeSelf)
		{
			uiObjects[220].SetActive(value: false);
		}
	}

	public void SetSubGenre(int i)
	{
		if (typ_contractGame && i >= 0 && g_GameMainGenre == i)
		{
			return;
		}
		g_GameSubGenre = i;
		if (i >= 0)
		{
			uiObjects[6].GetComponent<Text>().text = genres_.GetName(i);
			guiMain_.DrawStars(uiObjects[8], genres_.genres_LEVEL[i]);
			uiObjects[10].GetComponent<Image>().sprite = genres_.GetPic(i);
			uiObjects[10].GetComponent<tooltip>().c = genres_.GetTooltip(i);
			uiObjects[219].GetComponent<Image>().sprite = genres_.GetPic(i);
			uiObjects[219].GetComponent<tooltip>().c = genres_.GetTooltip(i);
		}
		else
		{
			uiObjects[6].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[8], 0);
			uiObjects[10].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[10].GetComponent<tooltip>().c = "";
			uiObjects[219].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[219].GetComponent<tooltip>().c = "";
		}
		if (games_.IsNewGenreCombination(g_GameMainGenre, g_GameSubGenre) && !typ_contractGame)
		{
			if (!uiObjects[220].activeSelf)
			{
				uiObjects[220].SetActive(value: true);
			}
		}
		else if (uiObjects[220].activeSelf)
		{
			uiObjects[220].SetActive(value: false);
		}
	}

	public void SetMainTheme(int i)
	{
		g_GameMainTheme = i;
		if (i >= 0)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetThemes(i);
			guiMain_.DrawStars(uiObjects[22], themes_.themes_LEVEL[i]);
			uiObjects[25].GetComponent<Image>().sprite = guiMain_.uiSprites[6];
			uiObjects[27].GetComponent<Image>().sprite = guiMain_.uiSprites[1];
		}
		else
		{
			uiObjects[21].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[22], 0);
			uiObjects[25].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[27].GetComponent<Image>().sprite = guiMain_.uiSprites[0];
		}
		if (games_.IsNewTopicCombination(g_GameMainTheme, g_GameSubTheme) && !typ_contractGame)
		{
			if (!uiObjects[221].activeSelf)
			{
				uiObjects[221].SetActive(value: true);
			}
		}
		else if (uiObjects[221].activeSelf)
		{
			uiObjects[221].SetActive(value: false);
		}
	}

	public void SetSubTheme(int i)
	{
		g_GameSubTheme = i;
		if (i >= 0)
		{
			uiObjects[23].GetComponent<Text>().text = tS_.GetThemes(i);
			guiMain_.DrawStars(uiObjects[24], themes_.themes_LEVEL[i]);
			uiObjects[26].GetComponent<Image>().sprite = guiMain_.uiSprites[6];
		}
		else
		{
			uiObjects[23].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[24], 0);
			uiObjects[26].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
		if (games_.IsNewTopicCombination(g_GameMainTheme, g_GameSubTheme) && !typ_contractGame)
		{
			if (!uiObjects[221].activeSelf)
			{
				uiObjects[221].SetActive(value: true);
			}
		}
		else if (uiObjects[221].activeSelf)
		{
			uiObjects[221].SetActive(value: false);
		}
	}

	public void SetLicence(int i)
	{
		g_GameLicence = i;
		if (i >= 0)
		{
			uiObjects[28].GetComponent<Text>().text = licences_.GetName(i);
			guiMain_.DrawStars(uiObjects[29], Mathf.RoundToInt(licences_.licence_QUALITY[i] / 20f));
			uiObjects[30].GetComponent<Image>().sprite = licences_.licenceSprites[licences_.licence_TYP[i]];
			uiObjects[31].GetComponent<Text>().text = licences_.GetTypString(i);
			uiObjects[223].SetActive(value: true);
			uiObjects[224].SetActive(value: true);
			uiObjects[223].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREGOOD[i]);
			uiObjects[224].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREBAD[i]);
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = tS_.GetText(358);
			guiMain_.DrawStars(uiObjects[29], 0);
			uiObjects[30].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[31].GetComponent<Text>().text = "";
			uiObjects[223].SetActive(value: false);
			uiObjects[224].SetActive(value: false);
			uiObjects[223].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[224].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
	}

	public void SetLicenceName()
	{
		if (g_GameLicence != -1)
		{
			uiObjects[0].GetComponent<InputField>().text = licences_.GetName(g_GameLicence);
		}
	}

	public void SetMGSR_Result(gameScript script_, int value)
	{
		FindScripts();
		switch (value)
		{
		case 0:
			script_.usk = 0;
			break;
		case 1:
			script_.usk = 0;
			if (Random.Range(0, 100) > 70)
			{
				script_.usk = 1;
			}
			break;
		case 2:
			script_.usk = 1;
			break;
		case 3:
			script_.usk = 1;
			if (Random.Range(0, 100) > 70)
			{
				script_.usk = 2;
			}
			break;
		case 4:
			script_.usk = 2;
			break;
		case 5:
			script_.usk = 2;
			if (Random.Range(0, 100) > 70)
			{
				script_.usk = 3;
			}
			break;
		case 6:
			script_.usk = 3;
			break;
		case 7:
			script_.usk = 3;
			if (Random.Range(0, 100) > 70)
			{
				script_.usk = 4;
			}
			break;
		case 8:
			script_.usk = 4;
			break;
		case 9:
			script_.usk = 4;
			if (Random.Range(0, 100) > 80)
			{
				script_.usk = 5;
			}
			break;
		case 10:
			script_.usk = 5;
			if (Random.Range(0, 100) > 80)
			{
				script_.usk = 4;
			}
			break;
		}
		if (script_.gameMainTheme != -1 && script_.usk < themes_.themes_MGSR[script_.gameMainTheme])
		{
			script_.usk = themes_.themes_MGSR[script_.gameMainTheme];
		}
		if (script_.gameSubTheme != -1 && script_.usk < themes_.themes_MGSR[script_.gameSubTheme])
		{
			script_.usk = themes_.themes_MGSR[script_.gameSubTheme];
		}
	}

	private void SetMGSR()
	{
		switch (g_Designausrichtung[1])
		{
		case 0:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[0];
			break;
		case 1:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[0];
			break;
		case 2:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[1];
			break;
		case 3:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[1];
			break;
		case 4:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[2];
			break;
		case 5:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[2];
			break;
		case 6:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[3];
			break;
		case 7:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[3];
			break;
		case 8:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[4];
			break;
		case 9:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[4];
			break;
		case 10:
			uiObjects[203].GetComponent<Image>().sprite = games_.gamePEGI[5];
			break;
		}
	}

	private void ResetDesignSettings()
	{
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			g_Designschwerpunkt[i] = 5;
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value = g_Designschwerpunkt[i];
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			g_Designausrichtung[j] = 5;
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().value = g_Designausrichtung[j];
		}
		g_GameAP_Gameplay = 5;
		g_GameAP_Grafik = 5;
		g_GameAP_Sound = 5;
		g_GameAP_Technik = 5;
		uiObjects[97].GetComponent<Slider>().value = g_GameAP_Gameplay;
		uiObjects[98].GetComponent<Slider>().value = g_GameAP_Grafik;
		uiObjects[99].GetComponent<Slider>().value = g_GameAP_Sound;
		uiObjects[100].GetComponent<Slider>().value = g_GameAP_Technik;
		SetMGSR();
		UpdateDesignSettings();
	}

	public void CopyDesignSettings(gameScript script_)
	{
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			g_Designschwerpunkt[i] = script_.Designschwerpunkt[i];
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value = g_Designschwerpunkt[i];
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			g_Designausrichtung[j] = script_.Designausrichtung[j];
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().value = g_Designausrichtung[j];
		}
		g_GameAP_Gameplay = script_.gameAP_Gameplay;
		g_GameAP_Grafik = script_.gameAP_Grafik;
		g_GameAP_Sound = script_.gameAP_Sound;
		g_GameAP_Technik = script_.gameAP_Technik;
		uiObjects[97].GetComponent<Slider>().value = g_GameAP_Gameplay;
		uiObjects[98].GetComponent<Slider>().value = g_GameAP_Grafik;
		uiObjects[99].GetComponent<Slider>().value = g_GameAP_Sound;
		uiObjects[100].GetComponent<Slider>().value = g_GameAP_Technik;
		SetMGSR();
		UpdateDesignSettings();
	}

	public int GetAmountDesignschwerpunkte()
	{
		int num = 40;
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			num -= g_Designschwerpunkt[i];
		}
		return num;
	}

	public void UpdateDesignSlider()
	{
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value = g_Designschwerpunkt[i];
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().value = g_Designausrichtung[j];
		}
	}

	public void BUTTON_AutoEngineFeature()
	{
		sfx_.PlaySound(3, force: true);
		if (!g_GameEngineScript_)
		{
			return;
		}
		int num = 99;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] != -1)
			{
				int platformTechLevel = GetPlatformTechLevel(g_GamePlatform[i]);
				if (platformTechLevel < num)
				{
					num = platformTechLevel;
				}
			}
		}
		for (int j = 0; j < g_GameEngineFeature.Length; j++)
		{
			for (int k = 0; k < g_GameEngineScript_.features.Length; k++)
			{
				if (g_GameEngineScript_.features[k] && eF_.engineFeatures_TYP[k] == j && eF_.engineFeatures_TECH[k] <= num)
				{
					SetEngineFeatureSimple(j, k);
				}
			}
		}
		GetGesamtDevPoints();
		CalcDevCosts();
	}

	public void BUTTON_AutoDesignSettings()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			if (g_GameMainGenre != -1 && genres_.GetFocusKnown(i, g_GameMainGenre, g_GameSubGenre))
			{
				g_Designschwerpunkt[i] = genres_.GetFocus(i, g_GameMainGenre, g_GameSubGenre);
			}
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = g_Designausrichtung[j].ToString();
			if (g_GameMainGenre != -1 && genres_.GetAlignKnown(j, g_GameMainGenre, g_GameSubGenre))
			{
				g_Designausrichtung[j] = genres_.GetAlign(j, g_GameMainGenre, g_GameSubGenre);
			}
		}
		UpdateDesignSettings();
		UpdateDesignSlider();
	}

	public void UpdateDesignSettings()
	{
		for (int i = 0; i < g_Designschwerpunkt.Length; i++)
		{
			uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = g_Designschwerpunkt[i].ToString();
			if (g_GameMainGenre != -1)
			{
				if (genres_.GetFocusKnown(i, g_GameMainGenre, g_GameSubGenre))
				{
					Text component = uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
					component.text = component.text + " <color=green>[" + genres_.GetFocus(i, g_GameMainGenre, g_GameSubGenre) + "]</color>";
				}
				else if (genres_.GetFocusTested(i, g_GameMainGenre, g_GameSubGenre, g_Designschwerpunkt[i]))
				{
					uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>" + g_Designschwerpunkt[i] + "</color>";
				}
			}
		}
		for (int j = 0; j < g_Designausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = g_Designausrichtung[j].ToString();
			if (g_GameMainGenre != -1)
			{
				if (genres_.GetAlignKnown(j, g_GameMainGenre, g_GameSubGenre))
				{
					Text component2 = uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>();
					component2.text = component2.text + " <color=green>[" + genres_.GetAlign(j, g_GameMainGenre, g_GameSubGenre) + "]</color>";
				}
				else if (genres_.GetAlignTested(j, g_GameMainGenre, g_GameSubGenre, g_Designausrichtung[j]))
				{
					uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=red>" + g_Designausrichtung[j] + "</color>";
				}
			}
		}
		int amountDesignschwerpunkte = GetAmountDesignschwerpunkte();
		uiObjects[202].GetComponent<Text>().text = amountDesignschwerpunkte.ToString();
		if (amountDesignschwerpunkte < 0)
		{
			uiObjects[202].GetComponent<Text>().color = guiMain_.colors[18];
		}
		else
		{
			uiObjects[202].GetComponent<Text>().color = guiMain_.colors[6];
		}
		float num = amountDesignschwerpunkte;
		uiObjects[201].GetComponent<Image>().fillAmount = num / 40f;
		for (int k = 0; k < g_Designschwerpunkt.Length; k++)
		{
			if (amountDesignschwerpunkte < 0)
			{
				uiDesignschwerpunkte[k].transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = guiMain_.colors[5];
			}
			else
			{
				uiDesignschwerpunkte[k].transform.GetChild(1).transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = guiMain_.colors[20];
			}
		}
		SetMGSR();
	}

	public void SLIDER_Designausrichtung(int i)
	{
		g_Designausrichtung[i] = Mathf.RoundToInt(uiDesignausrichtung[i].transform.GetChild(1).GetComponent<Slider>().value);
		UpdateDesignSettings();
		if (i == 1)
		{
			uiObjects[203].GetComponent<Animation>().Play();
		}
	}

	public void SLIDER_Designschwerpunkt(int i)
	{
		g_Designschwerpunkt[i] = Mathf.RoundToInt(uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value);
		UpdateDesignSettings();
	}

	public void SetAP_Gameplay()
	{
		g_GameAP_Gameplay = Mathf.RoundToInt(uiObjects[97].GetComponent<Slider>().value);
		uiObjects[101].GetComponent<Text>().text = g_GameAP_Gameplay * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Grafik()
	{
		g_GameAP_Grafik = Mathf.RoundToInt(uiObjects[98].GetComponent<Slider>().value);
		uiObjects[102].GetComponent<Text>().text = g_GameAP_Grafik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Sound()
	{
		g_GameAP_Sound = Mathf.RoundToInt(uiObjects[99].GetComponent<Slider>().value);
		uiObjects[103].GetComponent<Text>().text = g_GameAP_Sound * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Technik()
	{
		g_GameAP_Technik = Mathf.RoundToInt(uiObjects[100].GetComponent<Slider>().value);
		uiObjects[104].GetComponent<Text>().text = g_GameAP_Technik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetLanguage(int i)
	{
		sfx_.PlaySound(3, force: false);
		g_GameLanguage[i] = !g_GameLanguage[i];
		if (g_GameLanguage[i])
		{
			uiObjects[107 + i].GetComponent<Image>().color = Color.white;
		}
		else
		{
			uiObjects[107 + i].GetComponent<Image>().color = languageColor;
		}
		CalcDevCosts();
	}

	public int AnzahlLanguages()
	{
		int num = 0;
		for (int i = 0; i < g_GameLanguage.Length; i++)
		{
			if (g_GameLanguage[i])
			{
				num++;
			}
		}
		return num;
	}

	private int GetPlatformTechLevel(int platform_)
	{
		GameObject gameObject = GameObject.Find("PLATFORM_" + platform_);
		if ((bool)gameObject)
		{
			platformScript component = gameObject.GetComponent<platformScript>();
			if ((bool)component)
			{
				return component.tech;
			}
		}
		return 0;
	}

	private float GetExklusivGameBonus()
	{
		if (typ_contractGame)
		{
			return 0f;
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 1)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[0]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component)
				{
					return component.GetExklusivBonus();
				}
			}
		}
		return 0f;
	}

	public void SetPlatform(int slot, int platform_)
	{
		g_GamePlatform[slot] = platform_;
		if (platform_ >= 0)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + platform_);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				uiObjects[39 + slot].GetComponent<Text>().text = component.GetName();
				component.SetPic(uiObjects[43 + slot]);
				uiObjects[43 + slot].SetActive(value: true);
				guiMain_.DrawStars(uiObjects[47 + slot], component.erfahrung);
				uiObjects[51 + slot].GetComponent<Text>().text = component.tech.ToString();
				uiObjects[55 + slot].GetComponent<Text>().text = component.GetMarktanteilString();
				if (component.ownerID == mS_.myID && !component.isUnlocked)
				{
					uiObjects[55 + slot].GetComponent<Text>().text = "<color=red>[" + tS_.GetText(528) + "]</color>";
				}
				uiObjects[168 + slot].GetComponent<Image>().sprite = component.GetComplexSprite();
				uiObjects[205 + slot].GetComponent<Image>().sprite = component.GetTypSprite();
				uiObjects[205 + slot].GetComponent<tooltip>().c = component.GetTypString();
				if (component.internet)
				{
					uiObjects[180 + slot].SetActive(value: true);
				}
				else
				{
					uiObjects[180 + slot].SetActive(value: false);
				}
				uiObjects[43 + slot].GetComponent<tooltip>().c = component.GetTooltip();
				if (slot == 0)
				{
					uiObjects[59].GetComponent<Image>().sprite = guiMain_.uiSprites[1];
				}
			}
		}
		else
		{
			uiObjects[39 + slot].GetComponent<Text>().text = tS_.GetText(360 + slot);
			uiObjects[43 + slot].GetComponent<Image>().sprite = null;
			uiObjects[43 + slot].SetActive(value: false);
			guiMain_.DrawStars(uiObjects[47 + slot], 0);
			uiObjects[51 + slot].GetComponent<Text>().text = "-";
			uiObjects[55 + slot].GetComponent<Text>().text = "";
			uiObjects[168 + slot].GetComponent<Image>().sprite = platforms_.complexSprites[0];
			uiObjects[205 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[205 + slot].GetComponent<tooltip>().c = "";
			uiObjects[180 + slot].SetActive(value: false);
			if (slot == 0)
			{
				uiObjects[59].GetComponent<Image>().sprite = guiMain_.uiSprites[0];
			}
		}
		uiObjects[209].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(0), 1) + "%";
		uiObjects[210].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(1), 1) + "%";
		uiObjects[211].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(2), 1) + "%";
		uiObjects[212].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(3), 1) + "%";
		long num = 0L;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] >= 0)
			{
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
				if ((bool)gameObject2)
				{
					platformScript component2 = gameObject2.GetComponent<platformScript>();
					num += component2.GetAktiveNutzer();
				}
			}
		}
		uiObjects[213].GetComponent<Text>().text = mS_.Round((float)num / 1000000f, 1) + " " + tS_.GetText(1483);
		if (uiObjects[146].GetComponent<Dropdown>().value == 4)
		{
			uiObjects[213].GetComponent<Text>().text = "---";
		}
		uiObjects[217].GetComponent<Text>().text = GetLowestPlatformTechLevel().ToString();
		CalcDevCosts();
		GetGesamtDevPoints();
	}

	public bool SetGameplayFeature(int i)
	{
		g_GameGameplayFeatures[i] = !g_GameGameplayFeatures[i];
		if (uiObjects[146].GetComponent<Dropdown>().value == 4 && gF_.gameplayFeatures_LOCKPLATFORM[i, 4])
		{
			g_GameGameplayFeatures[i] = false;
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 5 && gF_.gameplayFeatures_LOCKPLATFORM[i, 3])
		{
			g_GameGameplayFeatures[i] = false;
		}
		CalcDevCosts();
		GetGesamtDevPoints();
		UpdateGesamtGameplayFeatures();
		return g_GameGameplayFeatures[i];
	}

	public bool DisableGameplayFeature(int i)
	{
		g_GameGameplayFeatures[i] = false;
		CalcDevCosts();
		GetGesamtDevPoints();
		UpdateGesamtGameplayFeatures();
		return g_GameGameplayFeatures[i];
	}

	private int UpdateGesamtGameplayFeatures()
	{
		int num = 0;
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i])
			{
				num++;
			}
		}
		int num2 = 1;
		if (g_GameSize > 0)
		{
			num2 = maxFeatures_gameSize[g_GameSize - 1] / 2;
		}
		if (g_GameSize < 5)
		{
			uiObjects[122].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " / " + maxFeatures_gameSize[g_GameSize] + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		else
		{
			uiObjects[122].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		if (num > maxFeatures_gameSize[g_GameSize])
		{
			uiObjects[122].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[122].GetComponent<Text>().color = Color.black;
		}
		return num;
	}

	private int UpdateGesamtArbeitsprioritaet()
	{
		float value = uiObjects[97].GetComponent<Slider>().value;
		value += uiObjects[98].GetComponent<Slider>().value;
		value += uiObjects[99].GetComponent<Slider>().value;
		value += uiObjects[100].GetComponent<Slider>().value;
		value *= 5f;
		uiObjects[106].GetComponent<Text>().text = Mathf.RoundToInt(value) + "%";
		if (Mathf.RoundToInt(value) > 100)
		{
			uiObjects[106].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[106].GetComponent<Text>().color = guiMain_.colors[6];
		}
		float num = value;
		num *= 0.01f;
		if (num > 1f)
		{
			num = 1f;
		}
		uiObjects[105].GetComponent<Image>().fillAmount = num;
		return Mathf.RoundToInt(value);
	}

	public float GetGesamtMarktanteil(int platformTyp)
	{
		FindScripts();
		float num = 0f;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if (component.typ == platformTyp)
				{
					num += component.GetMarktanteil();
				}
			}
		}
		return num;
	}

	private float GetPreisnachlass()
	{
		float num = 0f;
		if (g_portIP != -1)
		{
			num += 0.15f;
		}
		if (typ_remaster)
		{
			num += 0.2f;
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 5)
		{
			num += 0.25f;
		}
		num += GetExklusivGameBonus() * 0.01f;
		if (num > 0.8f)
		{
			num = 0.8f;
		}
		return num;
	}

	private int CalcDevCosts_Grundkosten()
	{
		int num = 0;
		num = ((g_GameTyp == 0) ? (num + costs_gameTyp[g_GameTyp]) : (num + costs_gameTyp[g_GameTyp] * (mS_.difficulty + 1)));
		num = ((g_GameSize == 0) ? (num + costs_gameSize[g_GameSize]) : (num + costs_gameSize[g_GameSize] * (mS_.difficulty + 1)));
		return Mathf.RoundToInt((float)num * (1f - GetPreisnachlass()) * (uiObjects[159].GetComponent<Slider>().value * 0.01f));
	}

	private int CalcDevCosts_Technology()
	{
		int num = 0;
		if ((bool)g_GameEngineScript_)
		{
			if (g_GameEngineFeature[0] >= 0)
			{
				num += eF_.GetDevCosts(g_GameEngineFeature[0]);
			}
			if (g_GameEngineFeature[1] >= 0)
			{
				num += eF_.GetDevCosts(g_GameEngineFeature[1]);
			}
			if (g_GameEngineFeature[2] >= 0)
			{
				num += eF_.GetDevCosts(g_GameEngineFeature[2]);
			}
			if (g_GameEngineFeature[3] >= 0)
			{
				num += eF_.GetDevCosts(g_GameEngineFeature[3]);
			}
		}
		return Mathf.RoundToInt((float)num * (1f - GetPreisnachlass()) * (uiObjects[160].GetComponent<Slider>().value * 0.01f));
	}

	private int CalcDevCosts_Kontent()
	{
		int num = 0;
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i])
			{
				num += gF_.GetDevCosts(i);
			}
		}
		return Mathf.RoundToInt((float)num * (1f - GetPreisnachlass()) * (uiObjects[161].GetComponent<Slider>().value * 0.01f));
	}

	private int CalcDevCosts_Sonstiges()
	{
		int num = 0;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] != -1)
			{
				GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
				if ((bool)gameObject)
				{
					num += gameObject.GetComponent<platformScript>().GetDevCosts();
				}
			}
		}
		if ((bool)g_GameCopyProtectScript_)
		{
			num += g_GameCopyProtectScript_.GetDevCosts();
		}
		if ((bool)g_GameAntiCheatScript_)
		{
			num += g_GameAntiCheatScript_.GetDevCosts();
		}
		int num2 = 0;
		for (int j = 0; j < g_GameLanguage.Length; j++)
		{
			if (g_GameLanguage[j] && !mS_.Muttersprache(j))
			{
				num2 += GetGesamtDevPoints() * 5;
				num += GetGesamtDevPoints() * 5;
			}
		}
		uiObjects[184].GetComponent<Text>().text = mS_.GetMoney(num2, showDollar: true);
		if (typ_remaster)
		{
			num /= 2;
		}
		if (uiObjects[146].GetComponent<Dropdown>().value == 5)
		{
			num /= 4;
		}
		return Mathf.RoundToInt((float)num * (1f - GetPreisnachlass()));
	}

	private int CalcDevCosts()
	{
		int num = 0;
		num += CalcDevCosts_Grundkosten();
		num += CalcDevCosts_Technology();
		num += CalcDevCosts_Kontent();
		num += CalcDevCosts_Sonstiges();
		uiObjects[152].GetComponent<Text>().text = mS_.GetMoney(CalcDevCosts_Grundkosten(), showDollar: true);
		uiObjects[153].GetComponent<Text>().text = mS_.GetMoney(CalcDevCosts_Technology(), showDollar: true);
		uiObjects[154].GetComponent<Text>().text = mS_.GetMoney(CalcDevCosts_Kontent(), showDollar: true);
		uiObjects[155].GetComponent<Text>().text = mS_.GetMoney(CalcDevCosts_Sonstiges(), showDollar: true);
		float num2 = GetPreisnachlass() * 100f;
		if (num2 > 0f)
		{
			tS_.GetText(624).Replace("<NUM>", mS_.GetMoney(num, showDollar: true));
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true) + " (" + Mathf.RoundToInt(100f - num2) + "%)";
			return num;
		}
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		return num;
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	public void InitDropdowns_GameplayFeatures()
	{
		int value = PlayerPrefs.GetInt(uiObjects[119].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(6));
		list.Add(tS_.GetText(413));
		list.Add(tS_.GetText(1438));
		uiObjects[119].GetComponent<Dropdown>().ClearOptions();
		uiObjects[119].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[119].GetComponent<Dropdown>().value = value;
	}

	private void Init_GameplayFeatures()
	{
		FindScripts();
		if (g_GameGameplayFeatures.Length == 0)
		{
			g_GameGameplayFeatures = new bool[gF_.gameplayFeatures_LEVEL.Length];
		}
		for (int i = 0; i < uiObjects[120].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[120].transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < gF_.gameplayFeatures_LEVEL.Length; j++)
		{
			if (gF_.IsErforscht(j))
			{
				string text = gF_.GetName(j);
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[204].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Item_DevGame_GameplayFeature component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[120].transform).GetComponent<Item_DevGame_GameplayFeature>();
					component.myID = j;
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.gF_ = gF_;
					component.menuDevGame_ = this;
					component.BUTTON_Click();
					component.BUTTON_Click();
				}
			}
		}
		DROPDOWN_SortGameplayFeatures();
		guiMain_.KeinEintrag(uiObjects[120], uiObjects[121]);
	}

	public void DROPDOWN_SortGameplayFeatures()
	{
		int value = uiObjects[119].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[119].name, value);
		int childCount = uiObjects[120].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[120].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_GameplayFeature component = gameObject.GetComponent<Item_DevGame_GameplayFeature>();
				switch (value)
				{
				case 0:
					gameObject.name = gF_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = gF_.GetDevCosts(component.myID).ToString();
					break;
				case 2:
					gameObject.name = gF_.gameplayFeatures_TYP[component.myID].ToString();
					break;
				case 3:
					gameObject.name = component.goodBad.ToString();
					break;
				}
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[120]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[120]);
		}
	}

	public void BUTTON_RemasterName(int i)
	{
		uiObjects[0].GetComponent<InputField>().characterLimit = 99;
		uiObjects[0].GetComponent<InputField>().text = orignal_name + " " + tS_.GetText(620 + i);
		uiObjects[135].GetComponent<Button>().interactable = true;
		uiObjects[136].GetComponent<Button>().interactable = true;
		uiObjects[137].GetComponent<Button>().interactable = true;
		uiObjects[138].GetComponent<Button>().interactable = true;
		uiObjects[135 + i].GetComponent<Button>().interactable = false;
	}

	public int GetGesamtDevPoints()
	{
		int num = 0;
		for (int i = 0; i < g_GameEngineFeature.Length; i++)
		{
			num += eF_.GetDevPointsForGame(g_GameEngineFeature[i]);
		}
		for (int j = 0; j < g_GameGameplayFeatures.Length; j++)
		{
			if (g_GameGameplayFeatures[j])
			{
				num += gF_.GetDevPoints(j);
			}
		}
		float num2 = 1f;
		for (int k = 0; k < g_GamePlatform.Length; k++)
		{
			if (g_GamePlatform[k] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[k]);
			if (!gameObject)
			{
				continue;
			}
			platformScript component = gameObject.GetComponent<platformScript>();
			if ((bool)component)
			{
				switch (component.complex)
				{
				case 0:
					num2 += 0.1f;
					break;
				case 1:
					num2 += 0.3f;
					break;
				case 2:
					num2 += 0.6f;
					break;
				}
			}
		}
		num2 *= (float)num;
		num = Mathf.RoundToInt(num2);
		uiObjects[148].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: false);
		return num;
	}

	public Sprite GetTypSprite()
	{
		if (!games_)
		{
			return null;
		}
		return g_GameTyp switch
		{
			0 => games_.gameTypSprites[0], 
			1 => games_.gameTypSprites[6], 
			2 => games_.gameTypSprites[7], 
			_ => null, 
		};
	}

	public void SetLeitenderDesigner(characterScript charS_, bool manuellSelectet)
	{
		if ((bool)charS_ && charS_.roomID != rS_.myID)
		{
			charS_ = null;
		}
		if (!charS_)
		{
			float num = 0f;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.roomID == rS_.myID)
				{
					if (component.s_gamedesign > num)
					{
						num = component.s_gamedesign;
						charS_ = component;
					}
					if (rS_.leitenderGamedesigner == component.myID)
					{
						charS_ = component;
						break;
					}
				}
			}
		}
		if (!charS_)
		{
			uiObjects[150].GetComponent<Text>().text = "---";
			g_leitenderDesigner = null;
			rS_.leitenderGamedesigner = -1;
			return;
		}
		g_leitenderDesigner = charS_;
		if (rS_.leitenderGamedesigner != charS_.myID)
		{
			rS_.leitenderGamedesigner = -1;
		}
		if (manuellSelectet)
		{
			rS_.leitenderGamedesigner = charS_.myID;
		}
		uiObjects[150].GetComponent<Text>().text = charS_.myName;
	}

	public void SLIDER_Finanzierung(int i)
	{
		switch (i)
		{
		case 0:
			uiObjects[156].GetComponent<Text>().text = Mathf.RoundToInt(uiObjects[159].GetComponent<Slider>().value) + "%";
			g_finanzierung_Grundkosten = Mathf.RoundToInt(uiObjects[159].GetComponent<Slider>().value);
			break;
		case 1:
			uiObjects[157].GetComponent<Text>().text = Mathf.RoundToInt(uiObjects[160].GetComponent<Slider>().value) + "%";
			g_finanzierung_Technology = Mathf.RoundToInt(uiObjects[160].GetComponent<Slider>().value);
			break;
		case 2:
			uiObjects[158].GetComponent<Text>().text = Mathf.RoundToInt(uiObjects[161].GetComponent<Slider>().value) + "%";
			g_finanzierung_Kontent = Mathf.RoundToInt(uiObjects[161].GetComponent<Slider>().value);
			break;
		}
		CalcDevCosts();
	}

	public void BUTTON_Finanzierung_Minus(int i)
	{
		if (!typ_contractGame)
		{
			sfx_.PlaySound(3, force: true);
			switch (i)
			{
			case 0:
				uiObjects[159].GetComponent<Slider>().value--;
				break;
			case 1:
				uiObjects[160].GetComponent<Slider>().value--;
				break;
			case 2:
				uiObjects[161].GetComponent<Slider>().value--;
				break;
			}
		}
	}

	public void BUTTON_Finanzierung_Plus(int i)
	{
		if (!typ_contractGame)
		{
			sfx_.PlaySound(3, force: true);
			switch (i)
			{
			case 0:
				uiObjects[159].GetComponent<Slider>().value++;
				break;
			case 1:
				uiObjects[160].GetComponent<Slider>().value++;
				break;
			case 2:
				uiObjects[161].GetComponent<Slider>().value++;
				break;
			}
		}
	}

	private float SetMaxVerdienstInApp()
	{
		float num = 0f;
		float num2 = 0f;
		for (int i = 0; i < 6; i++)
		{
			if (g_InAppPurchase[i])
			{
				num += games_.inAppPurchasePrice[i];
				num2 += games_.inAppPurchaseHate[i];
				uiObjects[193 + i].GetComponent<Image>().color = guiMain_.colors[20];
			}
			else
			{
				uiObjects[193 + i].GetComponent<Image>().color = Color.white;
			}
		}
		uiObjects[192].GetComponent<Text>().text = "$" + mS_.Round(num, 2);
		uiObjects[199].GetComponent<Image>().fillAmount = num2 * 0.01f;
		uiObjects[199].GetComponent<Image>().color = GetValColor(100f - num2);
		return num;
	}

	public void BUTTON_InAppPurchase(int i)
	{
		sfx_.PlaySound(3, force: true);
		g_InAppPurchase[i] = !g_InAppPurchase[i];
		SetMaxVerdienstInApp();
	}

	public void BUTTON_AlleInAppPurchase()
	{
		sfx_.PlaySound(3, force: true);
		bool flag = g_InAppPurchase[0];
		for (int i = 0; i < 6; i++)
		{
			g_InAppPurchase[i] = !flag;
		}
		SetMaxVerdienstInApp();
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[120].transform.childCount; i++)
			{
				uiObjects[120].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[204].GetComponent<InputField>().text;
			Init_GameplayFeatures();
		}
	}

	public bool EngineFeatureToHighTechLevel()
	{
		int num = 99;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] != -1)
			{
				int platformTechLevel = GetPlatformTechLevel(g_GamePlatform[i]);
				if (platformTechLevel < num)
				{
					num = platformTechLevel;
				}
			}
		}
		Debug.Log("TECH Level: " + num);
		for (int j = 0; j < g_GameEngineFeature.Length; j++)
		{
			if (eF_.engineFeatures_TECH[g_GameEngineFeature[j]] > num)
			{
				return true;
			}
		}
		return false;
	}

	public int GetLowestPlatformTechLevel()
	{
		int num = 99;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] != -1)
			{
				int platformTechLevel = GetPlatformTechLevel(g_GamePlatform[i]);
				if (platformTechLevel < num)
				{
					num = platformTechLevel;
				}
			}
		}
		if (num >= 99)
		{
			num = 1;
		}
		return num;
	}

	public void BUTTON_RemovePlatform(int slot)
	{
		sfx_.PlaySound(3, force: true);
		if ((slot != 0 || uiObjects[140].GetComponent<Button>().interactable) && (slot != 1 || uiObjects[33].GetComponent<Button>().interactable) && (slot != 2 || uiObjects[34].GetComponent<Button>().interactable) && (slot != 3 || uiObjects[35].GetComponent<Button>().interactable))
		{
			SetPlatform(slot, -1);
		}
	}

	public void BUTTON_CloseFanbriefe()
	{
		sfx_.PlaySound(3, force: false);
		for (int i = 0; i < uiObjects[227].transform.childCount; i++)
		{
			uiObjects[227].transform.GetChild(i).gameObject.SetActive(value: false);
		}
	}
}
