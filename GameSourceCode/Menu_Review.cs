using UnityEngine;
using UnityEngine.UI;

public class Menu_Review : MonoBehaviour
{
	public gameScript game_;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private reviewText reviewText_;

	private unlockScript unlock_;

	private games games_;

	public GameObject[] uiObjects;

	public float reviewTotalLerp;

	private bool showContractAbrechnung;

	private bool showGamePassFrage;

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
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!reviewText_)
		{
			reviewText_ = main_.GetComponent<reviewText>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if ((bool)game_)
		{
			float b = game_.reviewTotal;
			reviewTotalLerp = Mathf.Lerp(reviewTotalLerp, b, 2.6f * Time.deltaTime);
			uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(reviewTotalLerp) + "%";
			uiObjects[22].GetComponent<Text>().text = Mathf.RoundToInt(reviewTotalLerp) + "%";
			uiObjects[30].GetComponent<Text>().text = Mathf.RoundToInt(reviewTotalLerp) + "%";
			ShowRewards(playSound: true);
		}
	}

	private void ShowRewards(bool playSound)
	{
		if (game_.typ_addon || game_.typ_addonStandalone || game_.typ_bundle || game_.typ_mmoaddon || game_.typ_bundleAddon)
		{
			return;
		}
		if (Mathf.RoundToInt(reviewTotalLerp) >= 80)
		{
			if (!uiObjects[4].activeSelf)
			{
				uiObjects[4].SetActive(value: true);
				if (playSound)
				{
					sfx_.PlaySound(31, force: false);
				}
			}
		}
		else if (uiObjects[4].activeSelf)
		{
			uiObjects[4].SetActive(value: false);
		}
		if (Mathf.RoundToInt(reviewTotalLerp) == game_.reviewTotal && game_.reviewTotal < 30)
		{
			if (!uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: true);
				if (playSound)
				{
					sfx_.PlaySound(32, force: false);
				}
			}
		}
		else if (uiObjects[12].activeSelf)
		{
			uiObjects[12].SetActive(value: false);
		}
	}

	public void InitContractGame(gameScript s_)
	{
		Init(s_);
		showContractAbrechnung = true;
	}

	public void InitGamePassFrage(gameScript s_)
	{
		Init(s_);
		showGamePassFrage = true;
	}

	public void Init(gameScript s_)
	{
		FindScripts();
		reviewTotalLerp = 0f;
		game_ = s_;
		if (game_.beschreibung.Length <= 0)
		{
			uiObjects[41].SetActive(value: false);
		}
		else
		{
			uiObjects[41].SetActive(value: true);
		}
		uiObjects[14].SetActive(value: true);
		uiObjects[15].SetActive(value: false);
		uiObjects[26].SetActive(value: false);
		if (s_.date_year != mS_.year || s_.date_month != mS_.month)
		{
			reviewTotalLerp = s_.reviewTotal;
			ShowRewards(playSound: false);
		}
		if (game_.handy)
		{
			uiObjects[34].SetActive(value: false);
			uiObjects[35].SetActive(value: true);
			uiObjects[38].SetActive(value: false);
		}
		if (game_.arcade)
		{
			uiObjects[34].SetActive(value: false);
			uiObjects[35].SetActive(value: false);
			uiObjects[38].SetActive(value: true);
		}
		if (!game_.arcade && !game_.handy)
		{
			uiObjects[34].SetActive(value: true);
			uiObjects[35].SetActive(value: false);
			uiObjects[38].SetActive(value: false);
		}
		string text = "";
		if (!game_.gamePlatformScript[0])
		{
			game_.FindMyPlatforms();
		}
		for (int i = 0; i < game_.gamePlatformScript.Length; i++)
		{
			if ((bool)game_.gamePlatformScript[i])
			{
				text = text + "▪ " + game_.gamePlatformScript[i].GetName() + " ";
			}
		}
		uiObjects[42].GetComponent<Text>().text = text;
		uiObjects[13].GetComponent<Text>().text = tS_.GetText(278);
		if (game_.retro)
		{
			uiObjects[13].GetComponent<Text>().text = "<color=green>" + tS_.GetText(908) + "</color>";
		}
		uiObjects[24].GetComponent<Image>().sprite = games_.gamePEGI[game_.usk];
		uiObjects[36].GetComponent<Image>().sprite = games_.gamePEGI[game_.usk];
		uiObjects[39].GetComponent<Image>().sprite = games_.gamePEGI[game_.usk];
		uiObjects[1].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[37].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[40].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[3].GetComponent<Text>().text = reviewText_.GetReviewText(game_);
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[2].GetComponent<Text>().text = game_.GetDeveloperName() + " / " + game_.GetPublisherName();
		if (game_.subgenre != -1)
		{
			uiObjects[6].GetComponent<Text>().text = game_.GetGenreString() + " / " + game_.GetSubGenreString();
		}
		else
		{
			uiObjects[6].GetComponent<Text>().text = game_.GetGenreString();
		}
		uiObjects[8].GetComponent<Text>().text = game_.reviewGameplay + "%";
		uiObjects[9].GetComponent<Text>().text = game_.reviewGrafik + "%";
		uiObjects[10].GetComponent<Text>().text = game_.reviewSound + "%";
		uiObjects[11].GetComponent<Text>().text = game_.reviewSteuerung + "%";
		uiObjects[5].GetComponent<Image>().sprite = game_.GetScreenshot();
		if (game_.typ_addon || game_.typ_addonStandalone || game_.typ_mmoaddon)
		{
			uiObjects[14].SetActive(value: false);
			uiObjects[15].SetActive(value: true);
			uiObjects[26].SetActive(value: false);
			uiObjects[17].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[18].GetComponent<Text>().text = game_.GetReleaseDateString();
			uiObjects[19].GetComponent<Text>().text = game_.GetDeveloperName() + " / " + game_.GetPublisherName();
			uiObjects[25].GetComponent<Image>().sprite = games_.gamePEGI[game_.usk];
			if (game_.subgenre != -1)
			{
				uiObjects[20].GetComponent<Text>().text = game_.GetGenreString() + " / " + game_.GetSubGenreString();
			}
			else
			{
				uiObjects[20].GetComponent<Text>().text = game_.GetGenreString();
			}
			uiObjects[21].GetComponent<Image>().sprite = game_.GetScreenshot();
			gameScript gameScript2 = game_.FindVorgaengerScript();
			if ((bool)gameScript2)
			{
				uiObjects[23].GetComponent<Text>().text = gameScript2.GetNameWithTag();
			}
		}
		if (!game_.typ_bundle && !game_.typ_bundleAddon)
		{
			return;
		}
		uiObjects[14].SetActive(value: false);
		uiObjects[15].SetActive(value: false);
		uiObjects[26].SetActive(value: true);
		uiObjects[27].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[28].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[31].GetComponent<Text>().text = game_.GetDeveloperName() + " / " + game_.GetPublisherName();
		uiObjects[32].GetComponent<Image>().sprite = games_.gamePEGI[game_.usk];
		uiObjects[29].GetComponent<Image>().sprite = game_.GetScreenshot();
		string text2 = "";
		for (int j = 0; j < game_.bundleID.Length; j++)
		{
			gameScript bundleGame = game_.GetBundleGame(j);
			if ((bool)bundleGame)
			{
				text2 = text2 + bundleGame.GetNameWithTag() + "\n";
			}
		}
		uiObjects[33].GetComponent<Text>().text = text2;
	}

	public void BUTTON_Close()
	{
		if ((bool)game_ && Mathf.RoundToInt(reviewTotalLerp) != game_.reviewTotal)
		{
			reviewTotalLerp = game_.reviewTotal;
			return;
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(19);
		}
		if (!unlock_.Get(26))
		{
			unlock_.CheckUnlock(showMessage: true);
		}
		if (!guiMain_.uiObjects[45].activeSelf && !guiMain_.uiObjects[115].activeSelf && !guiMain_.uiObjects[116].activeSelf && !guiMain_.uiObjects[124].activeSelf && !guiMain_.uiObjects[232].activeSelf && !guiMain_.uiObjects[243].activeSelf && !guiMain_.uiObjects[190].activeSelf && !guiMain_.uiObjects[191].activeSelf && !guiMain_.uiObjects[183].activeSelf && !guiMain_.uiObjects[191].activeSelf && !guiMain_.uiObjects[110].activeSelf && !guiMain_.uiObjects[192].activeSelf && !guiMain_.uiObjects[287].activeSelf && !guiMain_.uiObjects[288].activeSelf && !guiMain_.uiObjects[284].activeSelf && !guiMain_.uiObjects[303].activeSelf && !guiMain_.uiObjects[305].activeSelf && !guiMain_.uiObjects[316].activeSelf && !guiMain_.uiObjects[340].activeSelf && !guiMain_.uiObjects[356].activeSelf && !guiMain_.uiObjects[360].activeSelf && !guiMain_.uiObjects[363].activeSelf && !guiMain_.uiObjects[374].activeSelf && !guiMain_.uiObjects[375].activeSelf && !guiMain_.uiObjects[394].activeSelf && !guiMain_.uiObjects[420].activeSelf && !guiMain_.uiObjects[423].activeSelf && !guiMain_.uiObjects[455].activeSelf)
		{
			if (showContractAbrechnung)
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[101]);
				guiMain_.uiObjects[101].GetComponent<Menu_Dev_AuftragsSpielEnd>().Init(game_);
			}
			else if (showGamePassFrage)
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[426]);
				guiMain_.uiObjects[426].GetComponent<Menu_W_AutoGamePass>().Init(game_);
			}
			else
			{
				guiMain_.CloseMenu();
			}
		}
		showContractAbrechnung = false;
		showGamePassFrage = false;
	}

	public void BUTTON_Spielbeschreibung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[199]);
		guiMain_.uiObjects[199].GetComponent<Menu_Dev_ShowBeschreibung>().Init(game_);
	}
}
