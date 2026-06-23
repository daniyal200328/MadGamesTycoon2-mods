using UnityEngine;
using UnityEngine.UI;

public class Menu_QA_Spielbericht : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiDesignschwerpunkte;

	public GameObject[] uiDesignausrichtung;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private engineFeatures eF_;

	private gameScript gS_;

	private themes themes_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
	}

	private void Update()
	{
		if (uiObjects[32].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[33].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = tS_.GetText(217) + ": <color=blue>" + game_.GetReleaseDateString() + "</color>\n" + tS_.GetText(1293) + ": <color=blue>" + game_.GetEntwicklungsbeginnDateString() + "</color>";
		uiObjects[2].GetComponent<Image>().sprite = game_.GetTypSprite();
		uiObjects[3].GetComponent<Image>().sprite = game_.GetPlatformTypSprite();
		uiObjects[36].GetComponent<Image>().sprite = game_.GetSizeSprite();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(genres_.genres_GAMEPLAY[game_.maingenre]) + "%";
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(genres_.genres_GRAPHIC[game_.maingenre]) + "%";
		uiObjects[6].GetComponent<Text>().text = Mathf.RoundToInt(genres_.genres_SOUND[game_.maingenre]) + "%";
		uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(genres_.genres_CONTROL[game_.maingenre]) + "%";
		uiObjects[2].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[3].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
		string text = tS_.GetText(931);
		text = text.Replace("<NAME>", genres_.GetName(game_.maingenre));
		uiObjects[8].GetComponent<Text>().text = text;
		uiObjects[9].GetComponent<Text>().text = tS_.GetThemes(game_.gameMainTheme);
		uiObjects[12].SetActive(themes_.IsThemesFitWithGenre(game_.gameMainTheme, game_.maingenre));
		if (game_.gameSubTheme != -1)
		{
			uiObjects[10].GetComponent<Text>().text = tS_.GetThemes(game_.gameSubTheme);
			uiObjects[13].SetActive(themes_.IsThemesFitWithGenre(game_.gameSubTheme, game_.maingenre));
		}
		else
		{
			uiObjects[10].GetComponent<Text>().text = tS_.GetText(932);
			uiObjects[13].SetActive(value: false);
		}
		uiObjects[11].GetComponent<Text>().text = tS_.GetText(336) + ": " + game_.GetZielgruppeString();
		uiObjects[14].SetActive(genres_.IsTargetGroup(game_.maingenre, game_.gameZielgruppe));
		if (game_.subgenre == -1)
		{
			uiObjects[34].GetComponent<Text>().text = tS_.GetText(1462) + " <color=yellow>[" + game_.GetGenreString() + "]</color>";
		}
		else
		{
			uiObjects[34].GetComponent<Text>().text = tS_.GetText(1462) + " <color=yellow>[" + game_.GetGenreString() + " / " + game_.GetSubGenreString() + "]</color>";
		}
		if (game_.subgenre == -1)
		{
			uiObjects[35].GetComponent<Text>().text = tS_.GetText(1463) + " <color=yellow>[" + game_.GetGenreString() + "]</color>";
		}
		else
		{
			uiObjects[35].GetComponent<Text>().text = tS_.GetText(1463) + " <color=yellow>[" + game_.GetGenreString() + " / " + game_.GetSubGenreString() + "]</color>";
		}
		UpdateDesignSettings();
		text = tS_.GetText(933);
		text = text.Replace("<NAME>", genres_.GetName(game_.maingenre));
		uiObjects[31].GetComponent<Text>().text = text;
		for (int i = 0; i < genres_.genres_LEVEL.Length; i++)
		{
			if (genres_.genres_UNLOCK[i] && genres_.IsGenreCombination(game_.maingenre, i))
			{
				GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[30].transform);
				obj.GetComponent<Image>().sprite = genres_.GetPic(i);
				obj.GetComponent<tooltip>().c = genres_.GetName(i);
			}
		}
	}

	public void UpdateDesignSettings()
	{
		for (int i = 0; i < uiDesignschwerpunkte.Length; i++)
		{
			uiDesignschwerpunkte[i].transform.GetChild(1).GetComponent<Slider>().value = gS_.Designschwerpunkt[i];
			uiDesignschwerpunkte[i].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = gS_.Designschwerpunkt[i].ToString();
			if (gS_.maingenre != -1)
			{
				if (genres_.GetFocus(i, gS_.maingenre, gS_.subgenre) == gS_.Designschwerpunkt[i])
				{
					uiDesignschwerpunkte[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(value: true);
				}
				else
				{
					uiDesignschwerpunkte[i].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(value: false);
				}
			}
		}
		for (int j = 0; j < uiDesignausrichtung.Length; j++)
		{
			uiDesignausrichtung[j].transform.GetChild(1).GetComponent<Slider>().value = gS_.Designausrichtung[j];
			uiDesignausrichtung[j].transform.GetChild(2).transform.GetChild(0).GetComponent<Text>().text = gS_.Designausrichtung[j].ToString();
			if (gS_.maingenre != -1)
			{
				if (genres_.GetAlign(j, gS_.maingenre, gS_.subgenre) == gS_.Designausrichtung[j])
				{
					uiDesignausrichtung[j].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(value: true);
				}
				else
				{
					uiDesignausrichtung[j].transform.GetChild(3).transform.GetChild(0).gameObject.SetActive(value: false);
				}
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
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

	public void BUTTON_Fanbriefe()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[112].SetActive(value: true);
		guiMain_.uiObjects[112].GetComponent<Menu_Dev_Fanbriefe>().Init(gS_);
	}
}
