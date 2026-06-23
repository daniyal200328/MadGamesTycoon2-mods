using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class MenuPublishingOfferVerhandlung : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private games games_;

	private gameScript game_;

	private int garantiesummeAngebot;

	private float gewinnbeteiligungAngebot;

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
		if (!game_.pubAngebot_AngebotWoche && uiObjects[3].GetComponent<Slider>().value > 0f)
		{
			uiObjects[5].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[5].GetComponent<Button>().interactable = false;
		}
		if (!game_.pubAngebot || game_.isOnMarket)
		{
			BUTTON_Abbrechen();
		}
	}

	public void Init(gameScript pO_)
	{
		if (!pO_)
		{
			BUTTON_Abbrechen();
		}
		game_ = pO_;
		FindScripts();
		uiObjects[3].GetComponent<Slider>().value = 0f;
		string text = tS_.GetText(1735);
		text = text.Replace("<NAME1>", game_.GetDeveloperName());
		text = text.Replace("<NAME2>", game_.GetNameWithTag());
		uiObjects[0].GetComponent<Text>().text = text;
		garantiesummeAngebot = game_.PUBOFFER_GetGarantiesumme();
		gewinnbeteiligungAngebot = game_.PUBOFFER_GetGewinnbeteiligung();
		SetData();
	}

	private void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(garantiesummeAngebot, showDollar: true);
			uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gewinnbeteiligungAngebot) + "%";
			uiObjects[4].GetComponent<Image>().fillAmount = game_.pubAngebot_Stimmung * 0.01f;
			uiObjects[6].GetComponent<Image>().sprite = game_.GetDeveloperLogo();
			if (game_.pubAngebot_Stimmung < 33f)
			{
				uiObjects[4].GetComponent<Image>().color = guiMain_.colorsBalken[0];
			}
			if (game_.pubAngebot_Stimmung > 33f && game_.pubAngebot_Stimmung < 66f)
			{
				uiObjects[4].GetComponent<Image>().color = guiMain_.colorsBalken[1];
			}
			if (game_.pubAngebot_Stimmung > 66f)
			{
				uiObjects[4].GetComponent<Image>().color = guiMain_.colorsBalken[2];
			}
		}
	}

	public void SLIDER_Angebot()
	{
		float num = game_.pubAngebot_VerhandlungProzent - uiObjects[3].GetComponent<Slider>().value;
		num *= 0.01f;
		garantiesummeAngebot = Mathf.RoundToInt((float)game_.pubAngebot_Garantiesumme * num);
		gewinnbeteiligungAngebot = Mathf.RoundToInt(game_.pubAngebot_Gewinnbeteiligung * num);
		SetData();
	}

	private IEnumerator iMinus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Minus(i);
		}
	}

	public void BUTTON_Minus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			uiObjects[3].GetComponent<Slider>().value -= 10f;
		}
		else
		{
			uiObjects[3].GetComponent<Slider>().value -= 1f;
		}
		StartCoroutine(iMinus(i));
	}

	private IEnumerator iPlus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Plus(i);
		}
	}

	public void BUTTON_Plus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			uiObjects[3].GetComponent<Slider>().value += 10f;
		}
		else
		{
			uiObjects[3].GetComponent<Slider>().value += 1f;
		}
		StartCoroutine(iPlus(i));
	}

	public void BUTTON_Angebot()
	{
		if (!(uiObjects[3].GetComponent<Slider>().value <= 0f))
		{
			sfx_.PlaySound(3, force: true);
			game_.pubAngebot_VerhandlungProzent -= uiObjects[3].GetComponent<Slider>().value;
			game_.pubAngebot_Stimmung -= uiObjects[3].GetComponent<Slider>().value * Random.Range(game_.pubAngebot_Verhandlung, game_.pubAngebot_Verhandlung + 2f);
			uiObjects[3].GetComponent<Slider>().value = 0f;
			if (game_.pubAngebot_Stimmung < 0f)
			{
				game_.pubAngebot_Stimmung = 0f;
			}
			if (game_.pubAngebot_Stimmung > 100f)
			{
				game_.pubAngebot_Stimmung = 100f;
			}
			if (game_.pubAngebot_VerhandlungProzent < 0f)
			{
				game_.pubAngebot_VerhandlungProzent = 0f;
			}
			if (game_.pubAngebot_VerhandlungProzent > 100f)
			{
				game_.pubAngebot_VerhandlungProzent = 100f;
			}
			game_.pubAngebot_AngebotWoche = true;
			SLIDER_Angebot();
			if (game_.pubAngebot_Stimmung <= 0f)
			{
				BUTTON_Abbrechen();
				sfx_.PlaySound(53, force: true);
				guiMain_.MessageBox(tS_.GetText(1738), closeMenu: false);
				game_.pubAnbgebot_Inivs = true;
				mS_.publishingOfferMain_.amountPublishingOffers--;
			}
			else
			{
				sfx_.PlaySound(54, force: true);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (game_.reviewTotal <= 0)
		{
			game_.CalcReview(entwicklungsbericht: false);
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(game_);
			}
		}
		else
		{
			game_.date_year = mS_.year;
			game_.date_month = mS_.month;
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(game_);
			}
		}
		CreateGame();
	}

	private void CreateGame()
	{
		mS_.Pay(game_.PUBOFFER_GetGarantiesumme(), 25);
		mS_.publishingOfferMain_.amountPublishingOffers--;
		game_.pubAngebot = false;
		game_.pubOffer = true;
		game_.costs_marketing = 0L;
		game_.costs_mitarbeiter = 0L;
		game_.costs_server = 0L;
		game_.hype = Random.Range(0, 15);
		game_.date_month = mS_.month;
		game_.date_year = mS_.year;
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
		guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(game_, null, newGame: true, hideClose: true);
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
		guiMain_.uiObjects[349].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}
}
