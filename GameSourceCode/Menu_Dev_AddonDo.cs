using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_AddonDo : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiSides;

	public float[] devCostsPercent;

	public float[] devPointsPercent;

	private bool[] buttonAdds = new bool[8];

	private bool[] sprachen = new bool[11];

	private int seite;

	public bool[] g_GameGameplayFeatures;

	public bool[] g_GameLanguage;

	public characterScript g_leitenderDesigner;

	public string g_Beschreibung;

	public int g_GameCopyProtect = -1;

	public int g_GameAntiCheat = -1;

	public int g_GameAP_Gameplay;

	public int g_GameAP_Grafik;

	public int g_GameAP_Sound;

	public int g_GameAP_Technik;

	public int g_GameSubTheme = -1;

	public copyProtectScript g_GameCopyProtectScript_;

	public antiCheatScript g_GameAntiCheatScript_;

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

	private Menu_DevGame menuDevGame_;

	private forschungSonstiges forschungSonstiges_;

	public gameScript gS_;

	private roomScript rS_;

	private bool allSprachen;

	private bool allAdds;

	private string searchStringA = "";

	private float checkSoftwareTimer;

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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
		string c = tS_.GetText(1439) + "\n\n" + tS_.GetText(2075);
		uiObjects[78].GetComponent<tooltip>().c = c;
	}

	private void OnDisable()
	{
		FindScripts();
		cmS_.disableMovement = false;
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (uiObjects[79].transform.childCount <= 0)
		{
			if (uiObjects[80].activeSelf)
			{
				uiObjects[80].SetActive(value: false);
			}
		}
		else if (!uiObjects[80].activeSelf)
		{
			uiObjects[80].SetActive(value: true);
		}
		CheckNewSoftware();
	}

	public void Init(roomScript roomScript_, gameScript gameScript_)
	{
		FindScripts();
		rS_ = roomScript_;
		gS_ = gameScript_;
		InitDropdowns();
		Init_GameplayFeatures();
		SetLeitenderDesigner(null, manuellSelectet: false);
		allAdds = false;
		SetCopyProtect(-1);
		SetAutomaticBestCopyProtect();
		SetAntiCheat(-1);
		SetAutomaticBestAntiCheat();
		g_GameAP_Gameplay = gS_.gameAP_Gameplay;
		g_GameAP_Grafik = gS_.gameAP_Grafik;
		g_GameAP_Sound = gS_.gameAP_Sound;
		g_GameAP_Technik = gS_.gameAP_Technik;
		uiObjects[42].GetComponent<Slider>().value = g_GameAP_Gameplay;
		uiObjects[43].GetComponent<Slider>().value = g_GameAP_Grafik;
		uiObjects[44].GetComponent<Slider>().value = g_GameAP_Sound;
		uiObjects[45].GetComponent<Slider>().value = g_GameAP_Technik;
		SetAP_Gameplay();
		SetAP_Grafik();
		SetAP_Sound();
		SetAP_Technik();
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			g_GameGameplayFeatures[i] = false;
		}
		for (int j = 0; j < sprachen.Length; j++)
		{
			sprachen[j] = gS_.gameLanguage[j];
		}
		for (int k = 0; k < buttonAdds.Length; k++)
		{
			buttonAdds[k] = false;
		}
		string nameSimple = gS_.GetNameSimple();
		nameSimple = nameSimple.Replace(" <color=green>" + tS_.GetText(1549) + "</color>", string.Empty);
		nameSimple = nameSimple.Replace(" <color=green>" + tS_.GetText(1896) + "</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>[A]</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>[P]</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>", string.Empty);
		nameSimple = nameSimple.Replace("[P]", string.Empty);
		nameSimple = nameSimple.Replace("[A]", string.Empty);
		nameSimple = nameSimple.Replace("</color>", string.Empty);
		nameSimple = nameSimple.Replace("\n", string.Empty);
		nameSimple = nameSimple.Replace("\r", string.Empty);
		nameSimple = nameSimple.Replace("\t", string.Empty);
		nameSimple = nameSimple.Replace(tS_.GetText(1896), string.Empty);
		nameSimple = nameSimple.Replace(tS_.GetText(1549), string.Empty);
		uiObjects[12].GetComponent<InputField>().text = nameSimple + " - " + tS_.GetText(963);
		g_Beschreibung = gS_.beschreibung;
		g_GameSubTheme = gS_.gameSubTheme;
		long num = gS_.GetGesamteEntwicklungskosten();
		if (num > 100000000)
		{
			num = 100000000L;
		}
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[0]), showDollar: true);
		uiObjects[14].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[1]), showDollar: true);
		uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[2]), showDollar: true);
		uiObjects[16].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[3]), showDollar: true);
		uiObjects[17].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[4]), showDollar: true);
		uiObjects[18].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[5]), showDollar: true);
		uiObjects[19].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[6]), showDollar: true);
		uiObjects[20].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[7]), showDollar: true);
		uiObjects[51].GetComponent<Image>().sprite = genres_.GetPic(gS_.maingenre);
		uiObjects[52].GetComponent<Text>().text = genres_.GetName(gS_.maingenre);
		guiMain_.DrawStars(uiObjects[53], genres_.genres_LEVEL[gS_.maingenre]);
		if (gS_.subgenre != -1)
		{
			uiObjects[54].GetComponent<Image>().sprite = genres_.GetPic(gS_.subgenre);
			uiObjects[55].GetComponent<Text>().text = genres_.GetName(gS_.subgenre);
			guiMain_.DrawStars(uiObjects[56], genres_.genres_LEVEL[gS_.subgenre]);
		}
		else
		{
			uiObjects[54].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[55].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[56], 0);
		}
		uiObjects[57].GetComponent<Text>().text = tS_.GetThemes(gS_.gameMainTheme);
		guiMain_.DrawStars(uiObjects[58], themes_.themes_LEVEL[gS_.gameMainTheme]);
		SetSubTheme(gS_.gameSubTheme);
		uiObjects[62].GetComponent<Image>().sprite = gS_.GetSizeSprite();
		Unlock(31, uiObjects[49], uiObjects[48]);
		Unlock(31, null, uiObjects[50]);
		Unlock(64, uiObjects[67], uiObjects[66]);
		Unlock(64, null, uiObjects[68]);
		forschungSonstiges_.Unlock(36, uiObjects[75], uiObjects[74]);
		forschungSonstiges_.Unlock(35, uiObjects[76], null);
		UpdateGesamtGameplayFeatures();
		UpdateGUI();
		OpenSide(0);
		DROPDOWN_AddonTyp();
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

	private void UpdateGUI()
	{
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				uiObjects[21 + i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiObjects[21 + i].GetComponent<Image>().color = Color.white;
			}
		}
		for (int j = 0; j < sprachen.Length; j++)
		{
			if (sprachen[j])
			{
				uiObjects[1 + j].GetComponent<Image>().color = Color.white;
			}
			else
			{
				uiObjects[1 + j].GetComponent<Image>().color = new Color(0.3f, 0.3f, 0.3f, 1f);
			}
		}
		uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		GetGesamtDevPoints();
	}

	private float GetAddonQuality()
	{
		float num = 0f;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += devPointsPercent[i];
			}
		}
		return num;
	}

	private long GetDevCosts()
	{
		long num = 0L;
		long num2 = gS_.GetGesamteEntwicklungskosten();
		if (num2 > 100000000)
		{
			num2 = 100000000L;
		}
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += Mathf.RoundToInt((float)num2 * devCostsPercent[i]);
			}
		}
		int num3 = 0;
		for (int j = 0; j < sprachen.Length; j++)
		{
			if (sprachen[j] && !mS_.Muttersprache(j))
			{
				num3 += gS_.GetGesamtDevPoints() * 5;
				num += gS_.GetGesamtDevPoints() * 5;
			}
		}
		uiObjects[73].GetComponent<Text>().text = mS_.GetMoney(num3, showDollar: true);
		if ((bool)g_GameCopyProtectScript_)
		{
			num += g_GameCopyProtectScript_.GetDevCosts();
		}
		if ((bool)g_GameAntiCheatScript_)
		{
			num += g_GameAntiCheatScript_.GetDevCosts();
		}
		for (int k = 0; k < g_GameGameplayFeatures.Length; k++)
		{
			if (g_GameGameplayFeatures[k] && !gS_.gameplayFeatures_DevDone[k])
			{
				num += gF_.GetDevCosts(k);
			}
		}
		return num;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Adds(int i)
	{
		sfx_.PlaySound(3, force: true);
		buttonAdds[i] = !buttonAdds[i];
		if (uiObjects[63].GetComponent<Dropdown>().value == 1)
		{
			buttonAdds[7] = true;
		}
		UpdateGUI();
	}

	public void BUTTON_Sprache(int i)
	{
		sfx_.PlaySound(3, force: true);
		sprachen[i] = !sprachen[i];
		UpdateGUI();
	}

	public void BUTTON_AlleSprache()
	{
		sfx_.PlaySound(3, force: true);
		allSprachen = !allSprachen;
		for (int i = 0; i < sprachen.Length; i++)
		{
			sprachen[i] = allSprachen;
		}
		UpdateGUI();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		allAdds = !allAdds;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = allAdds;
		}
		UpdateGUI();
	}

	public void BUTTON_CopyProtect()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[194]);
	}

	public void BUTTON_CopyProtectKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[49]);
	}

	public void BUTTON_AntiCheat()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[240]);
	}

	public void BUTTON_AntiCheatKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[234]);
	}

	public void SetBeschreibung(string c)
	{
		g_Beschreibung = c;
	}

	private void SetAutomaticBestAntiCheat()
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

	private void SetAutomaticBestCopyProtect()
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

	public void SetCopyProtect(int i)
	{
		g_GameCopyProtect = i;
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("COPYPROTECT_" + i);
			if ((bool)gameObject)
			{
				copyProtectScript copyProtectScript2 = (g_GameCopyProtectScript_ = gameObject.GetComponent<copyProtectScript>());
				uiObjects[30].GetComponent<Text>().text = copyProtectScript2.GetName();
				uiObjects[31].GetComponent<Text>().text = mS_.GetMoney(copyProtectScript2.GetDevCosts(), showDollar: true);
				uiObjects[32].GetComponent<Image>().fillAmount = copyProtectScript2.effekt * 0.01f;
				uiObjects[33].GetComponent<Text>().text = mS_.Round(copyProtectScript2.effekt, 2) + "%";
				uiObjects[32].GetComponent<Image>().color = GetValColor(copyProtectScript2.effekt);
			}
		}
		else
		{
			g_GameCopyProtectScript_ = null;
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(383);
			uiObjects[31].GetComponent<Text>().text = "";
			uiObjects[32].GetComponent<Image>().fillAmount = 0f;
			uiObjects[33].GetComponent<Text>().text = "0.0%";
			uiObjects[32].GetComponent<Image>().color = GetValColor(0f);
		}
		UpdateGUI();
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
				uiObjects[69].GetComponent<Text>().text = antiCheatScript2.GetName();
				uiObjects[70].GetComponent<Text>().text = mS_.GetMoney(antiCheatScript2.GetDevCosts(), showDollar: true);
				uiObjects[71].GetComponent<Image>().fillAmount = antiCheatScript2.effekt * 0.01f;
				uiObjects[72].GetComponent<Text>().text = mS_.Round(antiCheatScript2.effekt, 2) + "%";
				uiObjects[71].GetComponent<Image>().color = GetValColor(antiCheatScript2.effekt);
			}
		}
		else
		{
			g_GameAntiCheatScript_ = null;
			uiObjects[69].GetComponent<Text>().text = tS_.GetText(1213);
			uiObjects[70].GetComponent<Text>().text = "";
			uiObjects[71].GetComponent<Image>().fillAmount = 0f;
			uiObjects[72].GetComponent<Text>().text = "0.0%";
			uiObjects[71].GetComponent<Image>().color = GetValColor(0f);
		}
		UpdateGUI();
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

	public void BUTTON_Start()
	{
		sfx_.PlaySound(3, force: true);
		if (!gS_ || !rS_)
		{
			return;
		}
		bool flag = false;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				flag = true;
			}
		}
		if (!flag)
		{
			guiMain_.MessageBox(tS_.GetText(972), closeMenu: false);
			OpenSide(0);
			return;
		}
		if (uiObjects[12].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(973), closeMenu: false);
			OpenSide(0);
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				gameScript component = array[j].GetComponent<gameScript>();
				if ((bool)component && component.GetNameSimple() == uiObjects[12].GetComponent<InputField>().text)
				{
					guiMain_.MessageBox(tS_.GetText(618), closeMenu: false);
					OpenSide(0);
					return;
				}
			}
		}
		if (UpdateGesamtArbeitsprioritaet() > 100)
		{
			guiMain_.MessageBox(tS_.GetText(400), closeMenu: false);
			OpenSide(1);
			return;
		}
		if (UpdateGesamtArbeitsprioritaet() < 100)
		{
			guiMain_.MessageBox(tS_.GetText(416), closeMenu: false);
			OpenSide(1);
			return;
		}
		if (AnzahlLanguages() <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(404), closeMenu: false);
			OpenSide(1);
			return;
		}
		if (UpdateGesamtGameplayFeatures() > menuDevGame_.maxFeatures_gameSize[gS_.gameSize])
		{
			guiMain_.MessageBox(tS_.GetText(974), closeMenu: false);
			OpenSide(2);
			return;
		}
		int num = Mathf.RoundToInt(GetDevCosts());
		mS_.Pay(num, 18);
		gameScript gameScript2 = games_.CreateNewGame(fromSavegame: false, setDate: true);
		gameScript2.ownerID = mS_.myID;
		gameScript2.mainIP = gS_.mainIP;
		gameScript2.costs_entwicklung = num;
		gameScript2.inDevelopment = true;
		gameScript2.SetMyName(uiObjects[12].GetComponent<InputField>().text);
		gameScript2.originalGameID = gS_.myID;
		gameScript2.developerID = mS_.myID;
		gameScript2.addonQuality = GetAddonQuality();
		gameScript2.beschreibung = g_Beschreibung;
		if (uiObjects[63].GetComponent<Dropdown>().value == 1)
		{
			gameScript2.typ_addonStandalone = true;
		}
		else
		{
			gameScript2.typ_addon = true;
		}
		gameScript2.gameCopyProtect = g_GameCopyProtect;
		gameScript2.gameAntiCheat = g_GameAntiCheat;
		gameScript2.gameAP_Gameplay = g_GameAP_Gameplay;
		gameScript2.gameAP_Grafik = g_GameAP_Grafik;
		gameScript2.gameAP_Sound = g_GameAP_Sound;
		gameScript2.gameAP_Technik = g_GameAP_Technik;
		gameScript2.hype = (float)gS_.reviewTotal * 0.25f;
		for (int k = 0; k < g_GameGameplayFeatures.Length; k++)
		{
			gameScript2.gameGameplayFeatures[k] = g_GameGameplayFeatures[k];
			if (gS_.gameplayFeatures_DevDone[k])
			{
				gameScript2.gameGameplayFeatures[k] = true;
				gameScript2.gameplayFeatures_DevDone[k] = true;
			}
		}
		for (int l = 0; l < sprachen.Length; l++)
		{
			gameScript2.gameLanguage[l] = sprachen[l];
		}
		gameScript2.usk = gS_.usk;
		gameScript2.engineID = gS_.engineID;
		gameScript2.exklusiv = gS_.exklusiv;
		gameScript2.herstellerExklusiv = gS_.herstellerExklusiv;
		gameScript2.retro = gS_.retro;
		gameScript2.points_gameplay = gS_.points_gameplay;
		gameScript2.points_grafik = gS_.points_grafik;
		gameScript2.points_sound = gS_.points_sound;
		gameScript2.points_technik = gS_.points_technik;
		gameScript2.points_bugs = gS_.points_bugs;
		gameScript2.gameTyp = gS_.gameTyp;
		gameScript2.gameSize = gS_.gameSize;
		gameScript2.gameZielgruppe = gS_.gameZielgruppe;
		gameScript2.maingenre = gS_.maingenre;
		gameScript2.subgenre = gS_.subgenre;
		gameScript2.gameMainTheme = gS_.gameMainTheme;
		themes_.AddUses(gS_.gameMainTheme);
		gameScript2.gameSubTheme = g_GameSubTheme;
		themes_.AddUses(g_GameSubTheme);
		gameScript2.gameLicence = gS_.gameLicence;
		for (int m = 0; m < gS_.Designausrichtung.Length; m++)
		{
			gameScript2.Designausrichtung[m] = gS_.Designausrichtung[m];
		}
		for (int n = 0; n < gS_.Designschwerpunkt.Length; n++)
		{
			gameScript2.Designschwerpunkt[n] = gS_.Designschwerpunkt[n];
		}
		gameScript2.finanzierung_Grundkosten = gS_.finanzierung_Grundkosten;
		gameScript2.finanzierung_Kontent = gS_.finanzierung_Kontent;
		gameScript2.finanzierung_Technology = gS_.finanzierung_Technology;
		for (int num2 = 0; num2 < gS_.gamePlatform.Length; num2++)
		{
			gameScript2.gamePlatform[num2] = gS_.gamePlatform[num2];
		}
		for (int num3 = 0; num3 < gS_.gameEngineFeature.Length; num3++)
		{
			gameScript2.gameEngineFeature[num3] = gS_.gameEngineFeature[num3];
			gameScript2.engineFeature_DevDone[num3] = gS_.engineFeature_DevDone[num3];
		}
		for (int num4 = 0; num4 < gS_.gameplayStudio.Length; num4++)
		{
			gameScript2.gameplayStudio[num4] = gS_.gameplayStudio[num4];
		}
		for (int num5 = 0; num5 < gS_.grafikStudio.Length; num5++)
		{
			gameScript2.grafikStudio[num5] = gS_.grafikStudio[num5];
		}
		for (int num6 = 0; num6 < gS_.soundStudio.Length; num6++)
		{
			gameScript2.soundStudio[num6] = gS_.soundStudio[num6];
		}
		for (int num7 = 0; num7 < gS_.motionCaptureStudio.Length; num7++)
		{
			gameScript2.motionCaptureStudio[num7] = gS_.motionCaptureStudio[num7];
		}
		int devPointsContent = GetDevPointsContent();
		gameScript2.devPointsStart_Gesamt = gameScript2.GetGesamtDevPointsAddon() + devPointsContent;
		gameScript2.devPoints_Gesamt = gameScript2.devPointsStart_Gesamt;
		gameScript2.devAktFeature = -5;
		gameScript2.devPoints = devPointsContent;
		gameScript2.devPointsStart = gameScript2.devPoints;
		taskGame taskGame2 = guiMain_.AddTask_Game();
		taskGame2.Init(fromSavegame: false);
		taskGame2.gameID = gameScript2.myID;
		if ((bool)g_leitenderDesigner && g_leitenderDesigner.myID != -1)
		{
			taskGame2.leitenderDesignerID = g_leitenderDesigner.myID;
			taskGame2.designer_ = g_leitenderDesigner;
		}
		rS_.taskID = taskGame2.myID;
		guiMain_.CloseMenu();
		guiMain_.uiObjects[104].SetActive(value: false);
		guiMain_.uiObjects[189].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public int AnzahlLanguages()
	{
		int num = 0;
		for (int i = 0; i < sprachen.Length; i++)
		{
			if (sprachen[i])
			{
				num++;
			}
		}
		return num;
	}

	public void NextSide(int i)
	{
		seite += i;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > 2)
		{
			seite = 2;
		}
		OpenSide(seite);
		sfx_.PlaySound(3, force: true);
	}

	public void OpenSide(int i)
	{
		sfx_.PlaySound(3, force: false);
		for (int j = 0; j < uiSides.Length; j++)
		{
			if (uiSides[j].activeSelf && j != i)
			{
				uiSides[j].SetActive(value: false);
			}
		}
		seite = i;
		for (int k = 0; k < uiObjects[0].transform.childCount; k++)
		{
			uiObjects[0].transform.GetChild(k).GetComponent<Image>().color = Color.white;
		}
		uiObjects[0].transform.GetChild(i).GetComponent<Image>().color = Color.grey;
		if (!uiSides[i].activeSelf)
		{
			uiSides[i].SetActive(value: true);
		}
		if (i == 2)
		{
			StartCoroutine(iDROPDOWN_SortGameplayFeatures());
		}
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[34].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(6));
		list.Add(tS_.GetText(413));
		list.Add(tS_.GetText(1438));
		uiObjects[34].GetComponent<Dropdown>().ClearOptions();
		uiObjects[34].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[34].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[63].name);
		list.Clear();
		list.Add(tS_.GetText(963));
		list.Add(tS_.GetText(979));
		uiObjects[63].GetComponent<Dropdown>().ClearOptions();
		uiObjects[63].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[63].GetComponent<Dropdown>().value = value;
	}

	private void Init_GameplayFeatures()
	{
		FindScripts();
		if (g_GameGameplayFeatures.Length == 0)
		{
			g_GameGameplayFeatures = new bool[gF_.gameplayFeatures_LEVEL.Length];
		}
		for (int i = 0; i < uiObjects[35].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[35].transform.GetChild(i).gameObject);
		}
		for (int j = 0; j < gF_.gameplayFeatures_LEVEL.Length; j++)
		{
			if (!gF_.IsErforscht(j))
			{
				continue;
			}
			string text = gF_.GetName(j);
			searchStringA = searchStringA.ToLower();
			text = text.ToLower();
			if (uiObjects[77].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[35].transform);
				Item_DevAddon_GameplayFeature component = gameObject.GetComponent<Item_DevAddon_GameplayFeature>();
				component.myID = j;
				component.gF_ = gF_;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.BUTTON_Click();
				component.BUTTON_Click();
				if (gS_.gameplayFeatures_DevDone[j])
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_SortGameplayFeatures();
		guiMain_.KeinEintrag(uiObjects[35], uiObjects[36]);
	}

	public void DROPDOWN_AddonTyp()
	{
		int value = uiObjects[63].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[63].name, value);
		if (uiObjects[63].GetComponent<Dropdown>().value == 1)
		{
			buttonAdds[7] = true;
			UpdateGUI();
		}
	}

	private IEnumerator iDROPDOWN_SortGameplayFeatures()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		DROPDOWN_SortGameplayFeatures();
	}

	public void DROPDOWN_SortGameplayFeatures()
	{
		int value = uiObjects[34].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[34].name, value);
		int childCount = uiObjects[35].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[35].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevAddon_GameplayFeature component = gameObject.GetComponent<Item_DevAddon_GameplayFeature>();
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
			mS_.SortChildrenByName(uiObjects[35]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[35]);
		}
	}

	public bool SetGameplayFeature(int i)
	{
		g_GameGameplayFeatures[i] = !g_GameGameplayFeatures[i];
		GetDevCosts();
		UpdateGesamtGameplayFeatures();
		UpdateGUI();
		return g_GameGameplayFeatures[i];
	}

	private int UpdateGesamtGameplayFeatures()
	{
		int num = 0;
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (gS_.gameGameplayFeatures[i] || g_GameGameplayFeatures[i])
			{
				num++;
			}
		}
		int num2 = 1;
		if (gS_.gameSize > 0)
		{
			num2 = menuDevGame_.maxFeatures_gameSize[gS_.gameSize - 1] / 2;
		}
		if (gS_.gameSize < 5)
		{
			uiObjects[37].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " / " + menuDevGame_.maxFeatures_gameSize[gS_.gameSize] + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		else
		{
			uiObjects[37].GetComponent<Text>().text = tS_.GetText(410) + ": " + num + " <color=grey>(" + tS_.GetText(2071) + " " + num2 + ")</color>";
		}
		if (num > menuDevGame_.maxFeatures_gameSize[gS_.gameSize])
		{
			uiObjects[37].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[37].GetComponent<Text>().color = Color.black;
		}
		return num;
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
		for (int j = 0; j < uiObjects[35].transform.childCount; j++)
		{
			GameObject gameObject = uiObjects[35].transform.GetChild(j).gameObject;
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Item_DevAddon_GameplayFeature>().BUTTON_Click();
			}
		}
	}

	public void BUTTON_AllPassendenGameplayFeatures()
	{
		if (gS_.maingenre < 0)
		{
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
		for (int j = 0; j < uiObjects[35].transform.childCount; j++)
		{
			GameObject gameObject = uiObjects[35].transform.GetChild(j).gameObject;
			if ((bool)gameObject)
			{
				Item_DevAddon_GameplayFeature component = gameObject.GetComponent<Item_DevAddon_GameplayFeature>();
				if (gF_.GetBonus(component.myID, gS_.maingenre, gS_.subgenre) > 0.9f && gF_.gameplayFeatures_GAMEPLAY[component.myID] >= 0)
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
		GetDevCosts();
		UpdateGesamtGameplayFeatures();
		UpdateGUI();
		sfx_.PlaySound(3, force: true);
	}

	public void SetAP_Gameplay()
	{
		g_GameAP_Gameplay = Mathf.RoundToInt(uiObjects[42].GetComponent<Slider>().value);
		uiObjects[38].GetComponent<Text>().text = g_GameAP_Gameplay * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Grafik()
	{
		g_GameAP_Grafik = Mathf.RoundToInt(uiObjects[43].GetComponent<Slider>().value);
		uiObjects[39].GetComponent<Text>().text = g_GameAP_Grafik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Sound()
	{
		g_GameAP_Sound = Mathf.RoundToInt(uiObjects[44].GetComponent<Slider>().value);
		uiObjects[40].GetComponent<Text>().text = g_GameAP_Sound * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Technik()
	{
		g_GameAP_Technik = Mathf.RoundToInt(uiObjects[45].GetComponent<Slider>().value);
		uiObjects[41].GetComponent<Text>().text = g_GameAP_Technik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	private int UpdateGesamtArbeitsprioritaet()
	{
		float value = uiObjects[42].GetComponent<Slider>().value;
		value += uiObjects[43].GetComponent<Slider>().value;
		value += uiObjects[44].GetComponent<Slider>().value;
		value += uiObjects[45].GetComponent<Slider>().value;
		value *= 5f;
		uiObjects[46].GetComponent<Text>().text = Mathf.RoundToInt(value) + "%";
		if (Mathf.RoundToInt(value) > 100)
		{
			uiObjects[46].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[46].GetComponent<Text>().color = Color.white;
		}
		float num = value;
		num *= 0.01f;
		if (num > 1f)
		{
			num = 1f;
		}
		uiObjects[47].GetComponent<Image>().fillAmount = num;
		return Mathf.RoundToInt(value);
	}

	public int GetGesamtDevPoints()
	{
		int num = 0;
		num = GetDevPointsContent();
		for (int i = 0; i < g_GameGameplayFeatures.Length; i++)
		{
			if (g_GameGameplayFeatures[i] && !gS_.gameplayFeatures_DevDone[i])
			{
				num += gF_.GetDevPoints(i);
			}
		}
		uiObjects[64].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: false);
		return num;
	}

	public int GetDevPointsContent()
	{
		float num = 0f;
		float num2 = gS_.GetGesamtDevPoints();
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += devPointsPercent[i];
			}
		}
		num *= num2;
		return Mathf.RoundToInt(num);
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

	public void BUTTON_Beschreibung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[198]);
	}

	public void BUTTON_LeitenderEntwickler()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[196]);
		guiMain_.uiObjects[196].GetComponent<Menu_Dev_LeitenderDesigner>().Init(rS_);
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
			uiObjects[65].GetComponent<Text>().text = "---";
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
		uiObjects[65].GetComponent<Text>().text = charS_.myName;
	}

	public void BUTTON_Thema(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[62]);
		guiMain_.uiObjects[62].GetComponent<Menu_DevGame_Theme>().Init(i);
	}

	public void SetSubTheme(int i)
	{
		g_GameSubTheme = i;
		if (i >= 0)
		{
			uiObjects[60].GetComponent<Text>().text = tS_.GetThemes(i);
			guiMain_.DrawStars(uiObjects[61], themes_.themes_LEVEL[i]);
			uiObjects[59].GetComponent<Image>().sprite = guiMain_.uiSprites[6];
		}
		else
		{
			uiObjects[60].GetComponent<Text>().text = "---";
			guiMain_.DrawStars(uiObjects[61], 0);
			uiObjects[59].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[35].transform.childCount; i++)
			{
				uiObjects[35].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[77].GetComponent<InputField>().text;
			Init_GameplayFeatures();
		}
	}

	public void BUTTON_CloseFanbriefe()
	{
		sfx_.PlaySound(3, force: false);
		for (int i = 0; i < uiObjects[79].transform.childCount; i++)
		{
			uiObjects[79].transform.GetChild(i).gameObject.SetActive(value: false);
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
		if (mS_.IsBetterCopyProtect() && uiObjects[48].GetComponent<Button>().interactable)
		{
			if (!uiObjects[81].activeSelf)
			{
				uiObjects[81].SetActive(value: true);
			}
		}
		else if (uiObjects[81].activeSelf)
		{
			uiObjects[81].SetActive(value: false);
		}
		if (mS_.IsBetterAntiCheat() && uiObjects[66].GetComponent<Button>().interactable)
		{
			if (!uiObjects[82].activeSelf)
			{
				uiObjects[82].SetActive(value: true);
			}
		}
		else if (uiObjects[82].activeSelf)
		{
			uiObjects[82].SetActive(value: false);
		}
	}
}
