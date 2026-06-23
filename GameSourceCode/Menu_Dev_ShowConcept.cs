using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ShowConcept : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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

	private gameScript gS_;

	public GameObject[] uiDesignschwerpunkte;

	public GameObject[] uiDesignausrichtung;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (uiObjects[35].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[36].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(gameScript gameScript_)
	{
		FindScripts();
		gS_ = gameScript_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[7].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[39].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay).ToString();
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik).ToString();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound).ToString();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik).ToString();
		uiObjects[40].GetComponent<Image>().sprite = games_.gamePEGI[gS_.usk];
		uiObjects[7].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[39].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[24].GetComponent<Image>().sprite = gS_.GetSizeSprite();
		uiObjects[41].GetComponent<Component_Aufwertungen>().Init(gS_);
		string text = "";
		text = "<b>" + tS_.GetText(158) + ":</b> " + genres_.GetName(gS_.maingenre);
		if (gS_.subgenre != -1)
		{
			text = text + " / " + genres_.GetName(gS_.subgenre);
		}
		text = text + "\n<b>" + tS_.GetText(159) + ":</b> " + tS_.GetThemes(gS_.gameMainTheme);
		if (gS_.gameSubTheme != -1)
		{
			text = text + " / " + tS_.GetThemes(gS_.gameSubTheme);
		}
		uiObjects[8].GetComponent<Text>().text = text;
		Text component = uiObjects[8].GetComponent<Text>();
		component.text = component.text + "\n<b>" + tS_.GetText(336) + ":</b> " + tS_.GetText(337 + gS_.gameZielgruppe);
		if (gS_.gameLicence != -1)
		{
			component = uiObjects[8].GetComponent<Text>();
			component.text = component.text + "\n<b>" + tS_.GetText(356) + ":</b> " + licences_.GetName(gS_.gameLicence);
		}
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] != -1)
			{
				GameObject gameObject = GameObject.Find("PLATFORM_" + gS_.gamePlatform[i]);
				if ((bool)gameObject)
				{
					platformScript component2 = gameObject.GetComponent<platformScript>();
					uiObjects[9 + i].SetActive(value: true);
					component2.SetPic(uiObjects[9 + i]);
					uiObjects[9 + i].GetComponent<tooltip>().c = component2.GetTooltip();
				}
			}
			else
			{
				uiObjects[9 + i].SetActive(value: false);
			}
		}
		for (int j = 0; j < gS_.gameLanguage.Length; j++)
		{
			if (gS_.gameLanguage[j])
			{
				uiObjects[13 + j].GetComponent<Image>().color = Color.white;
			}
			else
			{
				uiObjects[13 + j].GetComponent<Image>().color = guiMain_.colors[6];
			}
		}
		UpdateDesignSettings();
		uiObjects[30].GetComponent<Slider>().value = gS_.gameAP_Gameplay;
		uiObjects[31].GetComponent<Slider>().value = gS_.gameAP_Grafik;
		uiObjects[32].GetComponent<Slider>().value = gS_.gameAP_Sound;
		uiObjects[33].GetComponent<Slider>().value = gS_.gameAP_Technik;
		uiObjects[42].GetComponent<Text>().text = gS_.gameAP_Gameplay * 5 + "%";
		uiObjects[43].GetComponent<Text>().text = gS_.gameAP_Grafik * 5 + "%";
		uiObjects[44].GetComponent<Text>().text = gS_.gameAP_Sound * 5 + "%";
		uiObjects[45].GetComponent<Text>().text = gS_.gameAP_Technik * 5 + "%";
		for (int k = 0; k < uiObjects[34].transform.childCount; k++)
		{
			Object.Destroy(uiObjects[34].transform.GetChild(k).gameObject);
		}
		for (int l = 0; l < gF_.gameplayFeatures_LEVEL.Length; l++)
		{
			if (gS_.gameplayFeatures_DevDone[l])
			{
				Item_GameConcept_GameplayFeature component3 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[34].transform).GetComponent<Item_GameConcept_GameplayFeature>();
				component3.myID = l;
				component3.gF_ = gF_;
				component3.mS_ = mS_;
				component3.tS_ = tS_;
				component3.sfx_ = sfx_;
				component3.guiMain_ = guiMain_;
				component3.gS_ = gS_;
			}
		}
		if (!guiMain_.uiObjects[56].activeSelf || (guiMain_.uiObjects[56].activeSelf && guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().typ_remaster) || (guiMain_.uiObjects[56].activeSelf && guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().g_portIP != -1))
		{
			if (uiObjects[37].activeSelf)
			{
				uiObjects[37].SetActive(value: false);
			}
			if (uiObjects[46].activeSelf)
			{
				uiObjects[46].SetActive(value: false);
			}
		}
		else
		{
			if (!uiObjects[37].activeSelf)
			{
				uiObjects[37].SetActive(value: true);
			}
			if (!uiObjects[46].activeSelf)
			{
				uiObjects[46].SetActive(value: true);
			}
		}
		StartCoroutine(ResizeGameplayFeatures());
	}

	private IEnumerator ResizeGameplayFeatures()
	{
		uiObjects[34].SetActive(value: false);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[34].SetActive(value: true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[38].SetActive(value: false);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[38].SetActive(value: true);
	}

	public void BUTTON_Close()
	{
		for (int i = 0; i < uiObjects[34].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[34].transform.GetChild(i).gameObject);
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_CopyDesigneinstellungen()
	{
		if ((bool)gS_)
		{
			sfx_.PlaySound(3, force: true);
			Menu_DevGame component = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
			component.CopyDesignSettings(gS_);
			component.uiObjects[97].GetComponent<Slider>().value = gS_.gameAP_Gameplay;
			component.SetAP_Gameplay();
			component.uiObjects[98].GetComponent<Slider>().value = gS_.gameAP_Grafik;
			component.SetAP_Grafik();
			component.uiObjects[99].GetComponent<Slider>().value = gS_.gameAP_Sound;
			component.SetAP_Sound();
			component.uiObjects[100].GetComponent<Slider>().value = gS_.gameAP_Technik;
			component.SetAP_Technik();
			guiMain_.uiObjects[109].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_CopyConcept()
	{
		if (!gS_)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		Menu_DevGame component = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		if (!component.typ_contractGame)
		{
			component.SetGameTyp(gS_.gameTyp);
			component.SetGameSize(gS_.gameSize);
			if (!component.typ_nachfolger)
			{
				component.SetMainGenre(gS_.maingenre);
			}
		}
		component.SetZielgruppe(gS_.gameZielgruppe);
		component.SetSubGenre(gS_.subgenre);
		component.SetMainTheme(gS_.gameMainTheme);
		component.SetSubTheme(gS_.gameSubTheme);
		component.CopyDesignSettings(gS_);
		component.uiObjects[97].GetComponent<Slider>().value = gS_.gameAP_Gameplay;
		component.SetAP_Gameplay();
		component.uiObjects[98].GetComponent<Slider>().value = gS_.gameAP_Grafik;
		component.SetAP_Grafik();
		component.uiObjects[99].GetComponent<Slider>().value = gS_.gameAP_Sound;
		component.SetAP_Sound();
		component.uiObjects[100].GetComponent<Slider>().value = gS_.gameAP_Technik;
		component.SetAP_Technik();
		for (int i = 0; i < gS_.gameLanguage.Length; i++)
		{
			component.g_GameLanguage[i] = !gS_.gameLanguage[i];
			component.SetLanguage(i);
		}
		for (int j = 0; j < gS_.gameGameplayFeatures.Length; j++)
		{
			component.g_GameGameplayFeatures[j] = !gS_.gameGameplayFeatures[j];
			component.SetGameplayFeature(j);
		}
		guiMain_.uiObjects[109].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Spielbeschreibung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[199]);
		guiMain_.uiObjects[199].GetComponent<Menu_Dev_ShowBeschreibung>().Init(gS_);
	}

	public void BUTTON_Review()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(gS_);
	}

	public void UpdateDesignSettings()
	{
		for (int i = 0; i < uiDesignschwerpunkte.Length; i++)
		{
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value = gS_.Designschwerpunkt[i];
			uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = gS_.Designschwerpunkt[i].ToString();
			if (gS_.maingenre != -1 && genres_.GetFocusKnown(i, gS_.maingenre, gS_.subgenre) && genres_.GetFocus(i, gS_.maingenre, gS_.subgenre) == gS_.Designschwerpunkt[i])
			{
				uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=green>" + gS_.Designschwerpunkt[i] + "</color>";
			}
		}
		for (int j = 0; j < uiDesignausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().value = gS_.Designausrichtung[j];
			uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = gS_.Designausrichtung[j].ToString();
			if (gS_.maingenre != -1 && genres_.GetAlignKnown(j, gS_.maingenre, gS_.subgenre) && genres_.GetAlign(j, gS_.maingenre, gS_.subgenre) == gS_.Designausrichtung[j])
			{
				uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = "<color=green>" + gS_.Designausrichtung[j] + "</color>";
			}
		}
	}
}
