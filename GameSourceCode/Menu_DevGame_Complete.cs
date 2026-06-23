using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Complete : MonoBehaviour
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

	private gameScript gS_;

	private taskGame task_;

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
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(14);
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript s1_, taskGame s2_)
	{
		FindScripts();
		gS_ = s1_;
		task_ = s2_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay).ToString();
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik).ToString();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound).ToString();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik).ToString();
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_bugs).ToString();
		uiObjects[13].GetComponent<Text>().text = Mathf.RoundToInt(gS_.GetHype()).ToString();
		uiObjects[6].GetComponent<Image>().sprite = gS_.GetScreenshot();
		uiObjects[31].GetComponent<Text>().text = tS_.GetText(6) + " <color=red>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>";
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
		string text = " " + num + "% - " + num2 + "%";
		uiObjects[32].GetComponent<Text>().text = tS_.GetText(452) + "<color=blue>" + text + "</color>";
		gS_.ClearReview();
		if (mS_.record_Gameplay < gS_.points_gameplay)
		{
			mS_.record_Gameplay = gS_.points_gameplay;
			uiObjects[7].SetActive(value: true);
		}
		else
		{
			uiObjects[7].SetActive(value: false);
		}
		if (mS_.record_Grafik < gS_.points_grafik)
		{
			mS_.record_Grafik = gS_.points_grafik;
			uiObjects[8].SetActive(value: true);
		}
		else
		{
			uiObjects[8].SetActive(value: false);
		}
		if (mS_.record_Sound < gS_.points_sound)
		{
			mS_.record_Sound = gS_.points_sound;
			uiObjects[9].SetActive(value: true);
		}
		else
		{
			uiObjects[9].SetActive(value: false);
		}
		if (mS_.record_Technik < gS_.points_technik)
		{
			mS_.record_Technik = gS_.points_technik;
			uiObjects[10].SetActive(value: true);
		}
		else
		{
			uiObjects[10].SetActive(value: false);
		}
		uiObjects[14].GetComponent<Component_Aufwertungen>().Init(gS_);
		uiObjects[15].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[15].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[20].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[20].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
		UpdateSpecialMarketingIcon();
		uiObjects[11].GetComponent<Button>().interactable = true;
		forschungSonstiges_.Unlock(33, uiObjects[12], uiObjects[11]);
		if (mS_.exklusivVertrag_ID == -1)
		{
			uiObjects[16].transform.GetChild(1).GetComponent<Text>().text = tS_.GetText(420);
		}
		else
		{
			string text2 = tS_.GetText(1052);
			text2 = text2.Replace("<NAME>", mS_.GetExklusivPublisher().GetName());
			uiObjects[16].transform.GetChild(1).GetComponent<Text>().text = text2;
		}
		if (gS_.typ_standard || gS_.typ_remaster || gS_.typ_nachfolger || gS_.typ_spinoff)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetText(417);
			uiObjects[19].GetComponent<Text>().text = tS_.GetText(418);
			uiObjects[11].SetActive(value: true);
			uiObjects[16].SetActive(value: true);
			uiObjects[17].SetActive(value: true);
			uiObjects[18].SetActive(value: false);
			uiObjects[22].SetActive(value: false);
			uiObjects[23].SetActive(value: false);
			if (mS_.exklusivVertrag_ID != -1)
			{
				uiObjects[11].GetComponent<Button>().interactable = false;
			}
		}
		if (gS_.arcade)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetText(417);
			uiObjects[19].GetComponent<Text>().text = tS_.GetText(418);
			uiObjects[16].SetActive(value: false);
			uiObjects[11].SetActive(value: true);
			uiObjects[11].GetComponent<Button>().interactable = true;
			uiObjects[12].SetActive(value: false);
		}
		if (gS_.gameTyp == 2 || gS_.handy)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetText(417);
			uiObjects[19].GetComponent<Text>().text = tS_.GetText(418);
			uiObjects[11].SetActive(value: false);
			uiObjects[16].SetActive(value: false);
			uiObjects[23].SetActive(value: true);
		}
		if (gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_mmoaddon)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetText(981);
			uiObjects[19].GetComponent<Text>().text = tS_.GetText(982);
			uiObjects[11].SetActive(value: true);
			uiObjects[16].SetActive(value: false);
			uiObjects[17].SetActive(value: true);
			uiObjects[18].SetActive(value: false);
			uiObjects[22].SetActive(value: true);
			uiObjects[23].SetActive(value: false);
			gameScript gameScript2 = gS_.FindVorgaengerScript();
			if ((bool)gameScript2 && gameScript2.publisherID != mS_.myID)
			{
				uiObjects[22].transform.GetChild(1).GetComponent<Text>().text = tS_.GetText(984) + " <color=blue>[" + gameScript2.GetPublisherName() + "]</color>";
				uiObjects[11].GetComponent<Button>().interactable = false;
				gameScript2.FindMyPublisher();
				if ((bool)gameScript2.pS_)
				{
					if (gameScript2.pS_.IsMyTochterfirma())
					{
						forschungSonstiges_.Unlock(33, uiObjects[12], uiObjects[11]);
						if (gameScript2.pS_.TochterfirmaGeschlossen() || !gameScript2.pS_.publisher)
						{
							uiObjects[22].SetActive(value: false);
							uiObjects[16].SetActive(value: true);
						}
					}
					if (gameScript2.pS_.Geschlossen())
					{
						uiObjects[11].GetComponent<Button>().interactable = true;
						if (gameScript2.pS_.Geschlossen() || !gameScript2.pS_.publisher)
						{
							uiObjects[22].SetActive(value: false);
							uiObjects[16].SetActive(value: true);
						}
					}
				}
				if (mS_.multiplayer)
				{
					gameScript2.FindMyPublisher();
					if ((bool)gameScript2.pS_ && gameScript2.pS_.IsTochterfirmaVonMitspieler())
					{
						uiObjects[22].SetActive(value: false);
						uiObjects[16].SetActive(value: true);
					}
				}
			}
			if (mS_.exklusivVertrag_ID != -1)
			{
				uiObjects[11].GetComponent<Button>().interactable = false;
			}
		}
		if (gS_.typ_contractGame)
		{
			uiObjects[21].GetComponent<Text>().text = tS_.GetText(417);
			uiObjects[19].GetComponent<Text>().text = tS_.GetText(629);
			uiObjects[11].SetActive(value: false);
			uiObjects[16].SetActive(value: false);
			uiObjects[17].SetActive(value: false);
			uiObjects[18].SetActive(value: true);
			uiObjects[22].SetActive(value: false);
			uiObjects[23].SetActive(value: false);
			uiObjects[24].SetActive(value: false);
			uiObjects[26].SetActive(value: true);
			uiObjects[25].SetActive(value: true);
			ShowContractDaten();
		}
		else
		{
			uiObjects[26].SetActive(value: false);
		}
		if (!gS_.typ_contractGame)
		{
			if (gS_.schublade)
			{
				uiObjects[24].SetActive(value: false);
				uiObjects[25].SetActive(value: false);
			}
			else
			{
				uiObjects[24].SetActive(value: true);
				uiObjects[25].SetActive(value: true);
			}
		}
	}

	private void UpdateSpecialMarketingIcon()
	{
		if (!gS_)
		{
			return;
		}
		uiObjects[33].SetActive(value: false);
		for (int i = 0; i < gS_.specialMarketing.Length; i++)
		{
			if (gS_.specialMarketing[i] == 1)
			{
				uiObjects[33].SetActive(value: true);
				break;
			}
		}
	}

	private void ShowContractDaten()
	{
		if (gS_.typ_contractGame)
		{
			string text = tS_.GetText(605);
			text = text.Replace("<NUM>", gS_.auftragsspiel_zeitInWochen.ToString());
			uiObjects[27].GetComponent<Text>().text = text;
			text = tS_.GetText(626);
			text = text.Replace("<NUM>", gS_.auftragsspiel_mindestbewertung.ToString());
			uiObjects[28].GetComponent<Text>().text = text;
			uiObjects[29].GetComponent<Text>().text = tS_.GetText(600) + ": " + mS_.GetMoney(gS_.auftragsspiel_gehalt, showDollar: true);
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(627) + ": " + mS_.GetMoney(gS_.auftragsspiel_bonus, showDollar: true);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Schublade()
	{
		sfx_.PlaySound(3, force: true);
		if (HasNoMainPlatform())
		{
			guiMain_.MessageBox(tS_.GetText(1660), closeMenu: false);
		}
		else
		{
			if (IsSpezialMarketingInBearbeitung())
			{
				return;
			}
			gS_.schublade = true;
			gS_.schubladeTaskID = task_.myID;
			gS_.inDevelopment = false;
			gS_.InitUI();
			GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
			for (int i = 0; i < array.Length; i++)
			{
				roomScript component = array[i].GetComponent<roomScript>();
				if ((bool)component && component.taskID == task_.myID)
				{
					component.taskID = -1;
					component.taskGameObject = null;
					break;
				}
			}
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_PublisherSuchen()
	{
		sfx_.PlaySound(3, force: true);
		if (HasNoMainPlatform())
		{
			guiMain_.MessageBox(tS_.GetText(1660), closeMenu: false);
		}
		else if (!gS_.AllePlattformenReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1634) + "\n\n<color=red>" + gS_.GetUnreleasedPlatformsString() + "</color>", closeMenu: false);
		}
		else if (!IsAddon_IsMainGameReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1504), closeMenu: false);
		}
		else if (Port_MainGameIsNotPublished(gS_))
		{
			guiMain_.MessageBox(tS_.GetText(2404), closeMenu: false);
		}
		else if (!IsSpezialMarketingInBearbeitung())
		{
			gS_.CalcReview(entwicklungsbericht: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[70]);
			guiMain_.uiObjects[70].GetComponent<Menu_Dev_SelectPublisher>().Init(gS_, task_);
			gS_.ClearReview();
			if (mS_.exklusivVertrag_ID != -1)
			{
				guiMain_.uiObjects[70].GetComponent<Menu_Dev_SelectPublisher>().SelectPublisher(mS_.exklusivVertrag_ID);
			}
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_AddonPublisher()
	{
		sfx_.PlaySound(3, force: true);
		if (HasNoMainPlatform())
		{
			guiMain_.MessageBox(tS_.GetText(1660), closeMenu: false);
		}
		else if (!gS_.AllePlattformenReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1634) + "\n\n<color=red>" + gS_.GetUnreleasedPlatformsString() + "</color>", closeMenu: false);
		}
		else if (!IsAddon_IsMainGameReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1504), closeMenu: false);
		}
		else if (!IsSpezialMarketingInBearbeitung())
		{
			gS_.CalcReview(entwicklungsbericht: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[70]);
			guiMain_.uiObjects[70].GetComponent<Menu_Dev_SelectPublisher>().Init(gS_, task_);
			gS_.ClearReview();
			gameScript gameScript2 = gS_.FindVorgaengerScript();
			if ((bool)gameScript2 && gameScript2.publisherID != mS_.myID)
			{
				guiMain_.uiObjects[70].GetComponent<Menu_Dev_SelectPublisher>().SelectPublisher(gameScript2.publisherID);
			}
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_OnlineVertreiben()
	{
		sfx_.PlaySound(3, force: true);
		if (HasNoMainPlatform())
		{
			guiMain_.MessageBox(tS_.GetText(1660), closeMenu: false);
		}
		else if (!gS_.AllePlattformenReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1634) + "\n\n<color=red>" + gS_.GetUnreleasedPlatformsString() + "</color>", closeMenu: false);
		}
		else if (!IsAddon_IsMainGameReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1504), closeMenu: false);
		}
		else if (Port_MainGameIsNotPublished(gS_))
		{
			guiMain_.MessageBox(tS_.GetText(2404), closeMenu: false);
		}
		else if (!IsSpezialMarketingInBearbeitung())
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[283]);
			guiMain_.uiObjects[283].GetComponent<Menu_ReleaseDate_F2P>().Init(gS_, task_);
		}
	}

	public void BUTTON_SelfPublish()
	{
		sfx_.PlaySound(3, force: true);
		if (HasNoMainPlatform())
		{
			guiMain_.MessageBox(tS_.GetText(1660), closeMenu: false);
		}
		else if (!gS_.AllePlattformenReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1634) + "\n\n<color=red>" + gS_.GetUnreleasedPlatformsString() + "</color>", closeMenu: false);
		}
		else if (!IsAddon_IsMainGameReleased())
		{
			guiMain_.MessageBox(tS_.GetText(1504), closeMenu: false);
		}
		else if (Port_MainGameIsNotPublished(gS_))
		{
			guiMain_.MessageBox(tS_.GetText(2404), closeMenu: false);
		}
		else if (!IsSpezialMarketingInBearbeitung())
		{
			if (gS_.arcade)
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[307]);
				guiMain_.uiObjects[307].GetComponent<Menu_ArcadePreis>().Init(gS_, task_);
			}
			else
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[218]);
				guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(gS_, task_, newGame: true, hideClose: false);
			}
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_Verwerfen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[93]);
		guiMain_.uiObjects[93].GetComponent<Menu_W_GameVerwerfen>().Init(gS_, task_);
	}

	public void BUTTON_Fortsetzen()
	{
		sfx_.PlaySound(3, force: true);
		if (!mS_.multiplayer)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[457]);
			guiMain_.uiObjects[457].GetComponent<Menu_Dev_Fortsetzen>().Init(gS_);
		}
		else
		{
			BUTTON_Close();
		}
	}

	public void BUTTON_ContractGame()
	{
		sfx_.PlaySound(3, force: true);
		gS_.CalcReview(entwicklungsbericht: false);
		Object.Destroy(task_.gameObject);
		gS_.SetOnMarket();
		guiMain_.ActivateMenu(guiMain_.uiObjects[71]);
		guiMain_.uiObjects[71].GetComponent<Menu_Dev_XP>().Init(gS_);
		base.gameObject.SetActive(value: false);
	}

	private bool IsAddon_IsMainGameReleased()
	{
		if ((gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_mmoaddon) && gS_.originalGameID != -1)
		{
			GameObject gameObject = GameObject.Find("GAME_" + gS_.originalGameID);
			if ((bool)gameObject)
			{
				if (gameObject.GetComponent<gameScript>().releaseDate > 0)
				{
					return false;
				}
				return true;
			}
		}
		return true;
	}

	private bool IsSpezialMarketingInBearbeitung()
	{
		return false;
	}

	private bool HasNoMainPlatform()
	{
		if (gS_.gamePlatform[0] == -1)
		{
			return true;
		}
		gS_.FindMyPlatforms();
		if (!gS_.gamePlatformScript[0])
		{
			return true;
		}
		return false;
	}

	private bool Port_MainGameIsNotPublished(gameScript game_)
	{
		if (!game_)
		{
			return false;
		}
		if (game_.portID == -1)
		{
			return false;
		}
		if (game_.portID != -1)
		{
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if (games_.arrayGamesScripts[i].myID == game_.portID && (games_.arrayGamesScripts[i].inDevelopment || games_.arrayGamesScripts[i].schublade))
				{
					return true;
				}
			}
		}
		return false;
	}
}
