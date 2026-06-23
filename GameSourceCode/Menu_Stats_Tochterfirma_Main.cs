using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Tochterfirma_Main : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private publisherScript pS_;

	private gameScript nextGame_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		nextGame_ = script_.FindGameInDevelopment();
		UpdateData();
	}

	private void Update()
	{
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(685) + ": <b>" + pS_.GetFirmenwertString() + "</b>";
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(1934) + ": <b>" + mS_.GetMoney(pS_.GetVerwaltungskosten(), showDollar: true) + "</b>";
	}

	public void ResetGame()
	{
		nextGame_ = null;
		pS_.newGameInWeeks = 0;
		UpdateData();
	}

	public void UpdateData()
	{
		if (!pS_)
		{
			return;
		}
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		guiMain_.DrawStarsColor(uiObjects[2], Mathf.RoundToInt(pS_.stars / 20f), Color.white);
		uiObjects[5].GetComponent<Text>().text = pS_.GetDateString();
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetPic(pS_.fanGenre);
		uiObjects[6].GetComponent<tooltip>().c = tS_.GetText(437) + ": <b>" + genres_.GetName(pS_.fanGenre) + "</b>";
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(685) + ": <b>" + pS_.GetFirmenwertString() + "</b>";
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(1934) + ": <b>" + mS_.GetMoney(pS_.GetVerwaltungskosten(), showDollar: true) + "</b>";
		if (pS_.developer)
		{
			int num = pS_.newGameInWeeks;
			if (num < 2)
			{
				num = 2;
			}
			string text = tS_.GetText(1948);
			text = text.Replace("<NUM>", "<color=blue><b>" + num + "</b></color>");
			uiObjects[28].GetComponent<tooltip>().c = text;
			uiObjects[29].GetComponent<Text>().text = num.ToString();
			uiObjects[19].GetComponent<Image>().fillAmount = pS_.GetEntwicklungsFortschritt();
			if (pS_.newGameInWeeks > 2)
			{
				uiObjects[20].GetComponent<Text>().text = tS_.GetText(1944) + ": " + Mathf.RoundToInt(pS_.GetEntwicklungsFortschritt() * 100f) + "%";
			}
			else
			{
				uiObjects[20].GetComponent<Text>().text = tS_.GetText(1947);
				if ((bool)nextGame_ && nextGame_.HasUnreleasedPlattform())
				{
					uiObjects[20].GetComponent<Text>().text = tS_.GetText(2316);
				}
			}
			if ((bool)nextGame_)
			{
				uiObjects[23].GetComponent<Text>().text = nextGame_.GetNameWithTag();
				uiObjects[26].GetComponent<Text>().text = mS_.Round(nextGame_.GetIpBekanntheit(), 1).ToString();
				uiObjects[24].GetComponent<Image>().sprite = nextGame_.GetTypSprite();
				uiObjects[25].GetComponent<Image>().sprite = nextGame_.GetSizeSprite();
				uiObjects[27].GetComponent<Text>().text = nextGame_.GetGenreString();
				if (nextGame_.subgenre != -1)
				{
					Text component = uiObjects[27].GetComponent<Text>();
					component.text = component.text + " / " + nextGame_.GetSubGenreString();
				}
				uiObjects[30].GetComponent<Button>().interactable = true;
				uiObjects[31].GetComponent<Button>().interactable = true;
			}
			else
			{
				uiObjects[23].GetComponent<Text>().text = "";
				uiObjects[26].GetComponent<Text>().text = "";
				uiObjects[24].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
				uiObjects[25].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
				uiObjects[27].GetComponent<Text>().text = "";
				uiObjects[29].GetComponent<Text>().text = "";
				uiObjects[30].GetComponent<Button>().interactable = false;
				uiObjects[31].GetComponent<Button>().interactable = false;
				uiObjects[20].GetComponent<Text>().text = tS_.GetText(1949);
				uiObjects[19].GetComponent<Image>().fillAmount = 0f;
				uiObjects[28].GetComponent<tooltip>().c = "";
			}
		}
		else
		{
			uiObjects[23].GetComponent<Text>().text = "<i>" + tS_.GetText(2029) + "</i>";
			uiObjects[26].GetComponent<Text>().text = "";
			uiObjects[24].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[25].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[27].GetComponent<Text>().text = "";
			uiObjects[29].GetComponent<Text>().text = "";
			uiObjects[30].GetComponent<Button>().interactable = false;
			uiObjects[31].GetComponent<Button>().interactable = false;
			uiObjects[21].GetComponent<Text>().text = "";
			uiObjects[20].GetComponent<Text>().text = tS_.GetText(1949);
			uiObjects[19].GetComponent<Image>().fillAmount = 0f;
			uiObjects[28].GetComponent<tooltip>().c = "";
		}
		if (pS_.Geschlossen())
		{
			uiObjects[12].SetActive(value: true);
			uiObjects[18].SetActive(value: true);
			uiObjects[0].GetComponent<Text>().text = "<color=red>" + pS_.GetName() + "</color>";
			uiObjects[10].GetComponent<Text>().text = pS_.GetDeveloperPublisherString();
		}
		else
		{
			uiObjects[12].SetActive(value: false);
			uiObjects[18].SetActive(value: false);
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[10].GetComponent<Text>().text = pS_.GetDeveloperPublisherString();
		}
		if (!pS_.publisher)
		{
			uiObjects[13].SetActive(value: true);
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(436) + ": <b>$ -</b>";
		}
		else
		{
			uiObjects[13].SetActive(value: false);
			uiObjects[4].GetComponent<Text>().text = tS_.GetText(436) + ": <b>$" + mS_.Round(pS_.share, 1) + "</b>";
		}
		if (!pS_.tf_publisher)
		{
			uiObjects[8].GetComponent<Button>().interactable = false;
			uiObjects[16].GetComponent<Button>().interactable = true;
			uiObjects[32].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[8].GetComponent<Button>().interactable = true;
			uiObjects[16].GetComponent<Button>().interactable = false;
			uiObjects[32].GetComponent<Button>().interactable = true;
		}
		if (!pS_.tf_developer)
		{
			uiObjects[9].GetComponent<Button>().interactable = false;
			uiObjects[17].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[9].GetComponent<Button>().interactable = true;
			uiObjects[17].GetComponent<Button>().interactable = false;
		}
		if (pS_.stars >= 100f || pS_.GetStarsAmount() >= 5)
		{
			uiObjects[15].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[15].GetComponent<Button>().interactable = true;
		}
		UpdateGewinnanteilTooltip();
	}

	private void UpdateGewinnanteilTooltip()
	{
		string text = tS_.GetText(1991);
		text = text.Replace("<NUM>", "<color=blue><b>" + mS_.GetMoney(Mathf.RoundToInt(pS_.share), showDollar: true) + "</b></color>");
		uiObjects[22].GetComponent<tooltip>().c = text;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[385].GetComponent<Menu_Statistics_Tochterfirmen>().BUTTON_Search();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Entwicklungsbericht()
	{
		if ((bool)nextGame_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[406]);
			guiMain_.uiObjects[406].GetComponent<Menu_Stats_Tochterfirma_GameEntwicklungsbericht>().Init(nextGame_);
		}
	}

	public void BUTTON_Rename()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[391]);
		guiMain_.uiObjects[391].GetComponent<Menu_TochterfirmaRename>().Init(pS_);
	}

	public void BUTTON_Awards()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[144]);
		guiMain_.uiObjects[144].GetComponent<Menu_Stats_Awards>().Init(pS_);
	}

	public void BUTTON_Games()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[360]);
		guiMain_.uiObjects[360].GetComponent<Menu_Stats_Developer_Games>().Init(pS_);
	}

	public void BUTTON_IPs()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[361]);
		guiMain_.uiObjects[361].GetComponent<Menu_Stats_Developer_IPs>().Init(pS_);
	}

	public void BUTTON_IpChange()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[403]);
		guiMain_.uiObjects[403].GetComponent<Menu_Stats_TochterfirmaIpTausch>().Init(pS_);
	}

	public void BUTTON_Settings()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[393]);
		guiMain_.uiObjects[393].GetComponent<Menu_Stats_TochterfirmaSettings>().Init(pS_);
	}

	public void BUTTON_Vertrieben()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[374]);
		guiMain_.uiObjects[374].GetComponent<Menu_Stats_Publisher_Vertrieben>().Init(pS_);
	}

	public void BUTTON_Umsatz()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[396]);
		guiMain_.uiObjects[396].GetComponent<Menu_Stats_TochterfirmaUmsatz>().Init(pS_);
	}

	public void BUTTON_FirmaVerkaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[395]);
		guiMain_.uiObjects[395].GetComponent<Menu_W_FirmaVerkaufen>().Init(pS_);
	}

	public void BUTTON_RemoveFromMarket()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[407]);
		guiMain_.uiObjects[407].GetComponent<Menu_TochterfirmaRemoveGameSelect>().Init(pS_);
	}

	public void BUTTON_FirmaSchiessen()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)pS_)
		{
			pS_.tf_geschlossen = !pS_.tf_geschlossen;
		}
		UpdateData();
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
		}
	}

	public void BUTTON_FirmaAufwerten()
	{
		if ((bool)pS_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[388]);
			guiMain_.uiObjects[388].GetComponent<Menu_W_FirmaAufwerten>().Init(pS_);
		}
	}

	public void BUTTON_FirmaAufwertenPublisher()
	{
		if ((bool)pS_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[389]);
			guiMain_.uiObjects[389].GetComponent<Menu_W_FirmaAufwertenPublisher>().Init(pS_);
		}
	}

	public void BUTTON_FirmaAufwertenDeveloper()
	{
		if ((bool)pS_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[390]);
			guiMain_.uiObjects[390].GetComponent<Menu_W_FirmaAufwertenDeveloper>().Init(pS_);
		}
	}

	private IEnumerator iMinusGewinnbeteiligung(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusGewinnbeteiligung(i);
		}
	}

	public void BUTTON_MinusGewinnbeteiligung(int i)
	{
		if (!pS_)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		pS_.share -= i;
		if (pS_.share < 5f)
		{
			pS_.share = 5f;
		}
		StartCoroutine(iMinusGewinnbeteiligung(i));
		UpdateData();
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Publisher(pS_);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_Publisher(pS_);
			}
		}
	}

	private IEnumerator iPlusGewinnbeteiligung(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusGewinnbeteiligung(i);
		}
	}

	public void BUTTON_PlusGewinnbeteiligung(int i)
	{
		if (!pS_)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		pS_.share += i;
		if (pS_.share > 15f)
		{
			pS_.share = 15f;
		}
		StartCoroutine(iPlusGewinnbeteiligung(i));
		UpdateData();
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Publisher(pS_);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_Publisher(pS_);
			}
		}
	}

	public void BUTTON_GameVerwerfen()
	{
		if ((bool)nextGame_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[93]);
			guiMain_.uiObjects[93].GetComponent<Menu_W_GameVerwerfen>().Init(nextGame_, null);
		}
	}
}
