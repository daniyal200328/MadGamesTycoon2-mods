using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_TochterfirmaGameComplete : MonoBehaviour
{
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

	private forschungSonstiges forschungSonstiges_;

	private gamepassScript gpS_;

	public gameScript gS_;

	public publisherScript pS_;

	private long buffer_costs_marketing;

	private long buffer_costs_mitarbeiter;

	private long buffer_costs_server;

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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
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
	}

	public void Init(gameScript s1_, publisherScript s2_)
	{
		FindScripts();
		sfx_.PlaySound(40, force: true);
		gS_ = s1_;
		pS_ = s2_;
		if (gS_.GetNameWithTag().Replace(" <color=green>[★]</color>", "").Contains("<"))
		{
			uiObjects[0].GetComponent<InputField>().text = "";
			uiObjects[0].GetComponent<InputField>().interactable = false;
			uiObjects[7].GetComponent<Text>().text = gS_.GetNameWithTag();
		}
		else
		{
			uiObjects[0].GetComponent<InputField>().text = gS_.GetNameSimple();
			uiObjects[0].GetComponent<InputField>().interactable = true;
			uiObjects[7].GetComponent<Text>().text = "";
		}
		uiObjects[1].GetComponent<Image>().sprite = gS_.GetScreenshot();
		string text = tS_.GetText(1976);
		text = text.Replace("<NAME>", "<color=blue><b>" + pS_.GetName() + "</b></color>");
		uiObjects[2].GetComponent<Text>().text = text;
		text = "";
		if (!gS_.typ_bundle && !gS_.typ_bundleAddon && gS_.subgenre == -1)
		{
			text += gS_.GetGenreString();
		}
		if (!gS_.typ_bundle && !gS_.typ_bundleAddon && gS_.subgenre != -1)
		{
			text = text + gS_.GetGenreString() + " / " + gS_.GetSubGenreString();
		}
		uiObjects[3].GetComponent<Text>().text = text;
		uiObjects[4].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[4].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[5].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[5].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
		uiObjects[6].GetComponent<Image>().sprite = gS_.GetSizeSprite();
		if (gS_.reviewTotal <= 0)
		{
			gS_.CalcReview(entwicklungsbericht: true);
			int num = gS_.reviewTotal - 10;
			int num2 = gS_.reviewTotal + 10;
			num = num / 10 * 10;
			num2 = num2 / 10 * 10;
			if (num < 1)
			{
				num = 1;
			}
			if (num2 > 100)
			{
				num2 = 100;
			}
			string text2 = " " + num + "% - " + num2 + "%";
			uiObjects[8].GetComponent<Text>().text = tS_.GetText(1980) + "<color=blue>" + text2 + "</color>";
			gS_.ClearReview();
			guiMain_.DrawStars(uiObjects[9], Mathf.RoundToInt(num2 / 20));
		}
		else
		{
			uiObjects[8].GetComponent<Text>().text = tS_.GetText(277) + ": <color=blue>" + gS_.reviewTotal + "%</color>";
			guiMain_.DrawStars(uiObjects[9], Mathf.RoundToInt(gS_.reviewTotal / 20));
		}
		uiObjects[10].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[18].GetComponent<Text>().text = mS_.Round(gS_.GetIpBekanntheit(), 1).ToString();
		gS_.FindMyPlatforms();
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			platformScript platformScript2 = gS_.gamePlatformScript[i];
			if ((bool)platformScript2)
			{
				if (!uiObjects[11 + i].activeSelf)
				{
					uiObjects[11 + i].SetActive(value: true);
				}
				platformScript2.SetPic(uiObjects[11 + i]);
				uiObjects[11 + i].GetComponent<tooltip>().c = platformScript2.GetTooltip();
			}
			else if (uiObjects[11 + i].activeSelf)
			{
				uiObjects[11 + i].SetActive(value: false);
			}
		}
		forschungSonstiges_.Unlock(33, uiObjects[15], uiObjects[16]);
		text = "";
		text = tS_.GetText(1981);
		text = text.Replace("<NUM>", Mathf.RoundToInt(games_.tf_gewinnbeteiligungTochterfirma).ToString());
		uiObjects[17].GetComponent<tooltip>().c = text + "\n\n";
		text = "";
		text = tS_.GetText(1982);
		text = text.Replace("<NUM>", Mathf.RoundToInt(100f - games_.tf_gewinnbeteiligungSelfPublish).ToString());
		uiObjects[17].GetComponent<tooltip>().c += text;
	}

	public void BUTTON_TochterfirmaUeberlassen()
	{
		if ((bool)sfx_)
		{
			sfx_.PlaySound(3, force: true);
		}
		ReplaceName();
		if (gS_.reviewTotal <= 0)
		{
			gS_.CalcReview(entwicklungsbericht: false);
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		else
		{
			gS_.date_year = mS_.year;
			gS_.date_month = mS_.month;
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		gS_.IGNORE_SetAsGameInDevelopmentNPC = true;
		pS_.ReleaseTheGame(gS_, forceContractGame: false, ignoreTochterfirma: true);
		gS_.IGNORE_SetAsGameInDevelopmentNPC = false;
		pS_.SetGameOnMarket(gS_);
		if (gpS_.gamePass_aktiv && gS_.CanBeInGamePass())
		{
			gpS_.GAMEPASS_AddGame(gS_, updateGamesAmount: true);
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_SelfPublish()
	{
		sfx_.PlaySound(3, force: true);
		ReplaceName();
		if (gS_.reviewTotal <= 0)
		{
			gS_.CalcReview(entwicklungsbericht: false);
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		else
		{
			gS_.date_year = mS_.year;
			gS_.date_month = mS_.month;
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		SelfpublishGame(gS_);
	}

	private void ReplaceName()
	{
		if (uiObjects[0].GetComponent<InputField>().interactable && uiObjects[0].GetComponent<InputField>().text.Length > 0)
		{
			gS_.myName = uiObjects[0].GetComponent<InputField>().text;
		}
	}

	public void BUTTON_Verwerfen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[93]);
		guiMain_.uiObjects[93].GetComponent<Menu_W_GameVerwerfen>().Init(gS_, null);
	}

	public void SelfpublishGameAbbruch(gameScript game_)
	{
		game_.pubOffer = false;
		game_.costs_marketing = buffer_costs_marketing;
		game_.costs_mitarbeiter = buffer_costs_mitarbeiter;
		game_.costs_server = buffer_costs_server;
		game_.hype = Random.Range(0, 15);
		game_.pubAngebot_Retail = false;
		game_.pubAngebot_Digital = false;
		game_.pubAngebot_Garantiesumme = 0;
		game_.pubAngebot_Gewinnbeteiligung = 0f;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(game_);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(game_);
			}
		}
	}

	private void SelfpublishGame(gameScript game_)
	{
		buffer_costs_marketing = game_.costs_marketing;
		buffer_costs_mitarbeiter = game_.costs_mitarbeiter;
		buffer_costs_server = game_.costs_server;
		game_.pubOffer = true;
		game_.costs_marketing = 0L;
		game_.costs_mitarbeiter = 0L;
		game_.costs_server = 0L;
		game_.hype = Random.Range(0, 15);
		game_.pubAngebot_Retail = true;
		game_.pubAngebot_Digital = true;
		game_.pubAngebot_Garantiesumme = 0;
		game_.pubAngebot_Gewinnbeteiligung = games_.tf_gewinnbeteiligungSelfPublish;
		if (game_.date_start_year <= 0)
		{
			game_.date_start_month = Random.Range(1, 10);
			game_.date_start_year = mS_.year - Random.Range(2, 4);
			if (game_.date_start_year < 1976)
			{
				game_.date_start_year = 1976;
				game_.date_start_month = 1;
			}
		}
		if (mS_.settings_randomEvents != 2)
		{
			if (game_.reviewTotal >= 70 && Random.Range(0, 100) < mS_.difficulty)
			{
				game_.commercialFlop = true;
			}
			if (!game_.commercialFlop && !game_.handy && !game_.typ_addon && !game_.typ_budget && !game_.typ_bundleAddon && !game_.typ_contractGame && !game_.typ_goty && !game_.typ_mmoaddon && Random.Range(0, 100) == 1)
			{
				game_.commercialHit = true;
			}
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[218]);
		guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(game_, null, newGame: true, hideClose: false);
	}
}
