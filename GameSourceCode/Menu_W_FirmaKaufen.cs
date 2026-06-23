using UnityEngine;
using UnityEngine.UI;

public class Menu_W_FirmaKaufen : MonoBehaviour
{
	public GameObject[] uiObjects;

	private publisherScript pS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private games games_;

	private achiementScript achivement_;

	private settingsScript settings_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!achivement_)
		{
			achivement_ = main_.GetComponent<achiementScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Update()
	{
		if ((bool)mS_ && (bool)pS_ && mS_.multiplayer && pS_.IsTochterfirma())
		{
			BUTTON_Abbrechen();
		}
	}

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		if (!pS_.KaufangebotFree())
		{
			guiMain_.MessageBox(tS_.GetText(1975), closeMenu: false);
			BUTTON_Abbrechen();
		}
		else if ((bool)pS_)
		{
			string text = tS_.GetText(1928);
			text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
			text = text.Replace("<NUM>", "<color=blue>" + pS_.GetFirmenwertString() + "</color>");
			uiObjects[0].GetComponent<Text>().text = text;
			if (!mS_.sandbox_tochterfirmaKonsole && pS_.ownPlatform)
			{
				Text component = uiObjects[0].GetComponent<Text>();
				component.text = component.text + "\n\n<color=red>" + tS_.GetText(2200) + "</color>";
			}
			uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		}
		else
		{
			BUTTON_Abbrechen();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)pS_ && (bool)mS_ && !pS_.IsTochterfirma())
		{
			if (mS_.money < pS_.GetFirmenwert())
			{
				guiMain_.ShowNoMoney();
				return;
			}
			mS_.Pay(pS_.GetFirmenwert(), 28);
			if (!mS_.sandbox_tochterfirmaKonsole && pS_.ownPlatform)
			{
				GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
				for (int i = 0; i < array.Length; i++)
				{
					if (!array[i])
					{
						continue;
					}
					platformScript component = array[i].GetComponent<platformScript>();
					if (!component || !component.OwnerIsNPC() || !component.isUnlocked || component.vomMarktGenommen || component.ownerID != pS_.myID)
					{
						continue;
					}
					component.RemoveFromMarket();
					if (mS_.multiplayer)
					{
						if (mS_.mpCalls_.isServer)
						{
							mS_.mpCalls_.SERVER_Send_PlatformRemoveFromMarket(component);
						}
						else
						{
							mS_.mpCalls_.CLIENT_Send_PlatformRemoveFromMarket(component);
						}
					}
				}
			}
			pS_.SetAsTochterfirma(mS_.myID);
			CheckAchivement();
			pS_.exklusive = false;
			pS_.onlyMobile = false;
			pS_.relation = 100f;
			pS_.tf_geschlossen = false;
			pS_.tf_autoRelease = false;
			pS_.tf_onlyPlayerConsole = false;
			pS_.tf_allowMMO = true;
			pS_.tf_allowF2P = true;
			pS_.tf_allowAddon = true;
			pS_.tf_noArcade = false;
			pS_.tf_noHandy = false;
			pS_.tf_noRetro = false;
			pS_.tf_noPorts = false;
			pS_.tf_noBudget = false;
			pS_.tf_noGOTY = false;
			pS_.tf_noBundles = false;
			pS_.tf_noAddonBundles = false;
			pS_.tf_noRemaster = false;
			pS_.tf_noSpinoffs = false;
			pS_.tf_autoGamePass = false;
			pS_.tf_gameGenre = 0;
			pS_.tf_gameSize = 0;
			pS_.tf_entwicklungsdauer = 1;
			pS_.tf_publisher = pS_.publisher;
			pS_.tf_developer = pS_.developer;
			pS_.tf_ownPublisher = true;
			pS_.tf_gameTopic = -1;
			pS_.tf_autoReleaseVal = 0;
			pS_.tf_ownPublisherPriority = -1;
			pS_.tf_umsatz_allTime = 0L;
			for (int j = 0; j < pS_.tf_ipFocus.Length; j++)
			{
				pS_.tf_ipFocus[j] = -1;
			}
			pS_.tf_engine = -1;
			for (int k = 0; k < pS_.tf_platformFocus.Length; k++)
			{
				pS_.tf_platformFocus[k] = -1;
			}
			for (int l = 0; l < pS_.tf_umsatz.Length; l++)
			{
				pS_.tf_umsatz[l] = 0L;
			}
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_PublisherTochterfirmaSettings(pS_);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_PublisherTochterfirmaSettings(pS_);
				}
				pS_.newGameInWeeks = Random.Range(20, 30);
				pS_.newGameInWeeksORG = pS_.newGameInWeeks;
				mS_.SendSystemMessage("<TOCHTERFIRMA>");
			}
			if (mS_.exklusivVertrag_ID == pS_.myID)
			{
				mS_.RemovePublisherExklusivVertrag();
			}
			if (guiMain_.uiObjects[359].activeSelf)
			{
				guiMain_.uiObjects[359].SetActive(value: false);
			}
			if (guiMain_.uiObjects[373].activeSelf)
			{
				guiMain_.uiObjects[373].SetActive(value: false);
			}
			if (guiMain_.uiObjects[119].activeSelf)
			{
				guiMain_.uiObjects[119].GetComponent<Menu_Statistics_Publisher>().BUTTON_Search();
			}
			if (guiMain_.uiObjects[120].activeSelf)
			{
				guiMain_.uiObjects[120].GetComponent<Menu_Statistics_Developer>().BUTTON_Search();
			}
		}
		BUTTON_Abbrechen();
	}

	public void CheckAchivement()
	{
		int num = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if (component.isUnlocked && component.IsMyTochterfirma())
				{
					num++;
				}
			}
		}
		if (num >= 1)
		{
			achivement_.SetAchivement(69);
		}
		if (num >= 10)
		{
			achivement_.SetAchivement(70);
		}
		if (num >= 30)
		{
			achivement_.SetAchivement(71);
		}
	}
}
