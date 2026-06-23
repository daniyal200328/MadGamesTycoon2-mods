using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GameFromMarket : MonoBehaviour
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

	private gameScript gS_;

	public List<gameScript> listMenu = new List<gameScript>();

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

	public void Init(gameScript game_, bool selbstVomMarktGenommen)
	{
		FindScripts();
		if (!selbstVomMarktGenommen && (bool)gS_)
		{
			listMenu.Add(game_);
			return;
		}
		sfx_.PlaySound(39, force: false);
		gS_ = game_;
		if (!gS_)
		{
			BUTTON_Close();
			return;
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = gS_.GetDeveloperName() + " / " + gS_.GetPublisherName();
		if (gS_.pubOffer)
		{
			Text component = uiObjects[1].GetComponent<Text>();
			component.text = component.text + "\n<color=blue>" + tS_.GetText(1744) + "</color>";
		}
		string text = "";
		if (!selbstVomMarktGenommen)
		{
			text = tS_.GetText(490);
			text = text.Replace("<NAME>", gS_.GetNameWithTag());
			uiObjects[2].GetComponent<Text>().text = text;
		}
		else
		{
			text = tS_.GetText(1143);
			text = text.Replace("<NAME>", gS_.GetNameWithTag());
			uiObjects[2].GetComponent<Text>().text = text;
		}
		text = "";
		if (gS_.gameTyp != 2)
		{
			text = text + tS_.GetText(491) + "\n";
		}
		if (gS_.gameTyp == 2)
		{
			text = text + tS_.GetText(1391) + "\n";
		}
		if (gS_.bestChartPosition != 0)
		{
			text = text + tS_.GetText(1669) + "\n";
		}
		if (gS_.gameTyp != 2)
		{
			text = text + tS_.GetText(696) + "\n";
		}
		if (gS_.gameTyp == 2)
		{
			text = text + tS_.GetText(697) + "\n";
		}
		if (gS_.gameTyp != 2 && !gS_.handy)
		{
			if (gS_.sellsTotalStandard > 0)
			{
				text = text + "     " + tS_.GetText(1103) + "\n";
			}
			if (gS_.sellsTotalDeluxe > 0)
			{
				text = text + "     " + tS_.GetText(1104) + "\n";
			}
			if (gS_.sellsTotalCollectors > 0)
			{
				text = text + "     " + tS_.GetText(1105) + "\n";
			}
			if (gS_.sellsTotalOnline > 0)
			{
				text = text + "     " + tS_.GetText(1126) + "\n";
			}
		}
		text += "\n";
		text = (gS_.pubOffer ? (text + tS_.GetText(1730) + "\n") : (text + tS_.GetText(6) + "\n"));
		if (gS_.PUBOFFER_GetGewinnbeteiligung() > 0)
		{
			text = text + tS_.GetText(1731) + " (" + Mathf.RoundToInt(gS_.PUBOFFER_GetGewinnbeteiligung()) + "%)\n";
		}
		if (gS_.GetMarketingkosten() > 0)
		{
			text = text + tS_.GetText(492) + "\n";
		}
		if (gS_.costs_enginegebuehren > 0)
		{
			text = text + tS_.GetText(493) + "\n";
		}
		if (gS_.costs_production > 0)
		{
			text = text + tS_.GetText(530) + "\n";
		}
		if (gS_.costs_updates > 0)
		{
			text = text + tS_.GetText(1406) + "\n";
		}
		if (gS_.costs_server > 0)
		{
			text = text + tS_.GetText(531) + "\n";
		}
		text += "\n";
		if (gS_.umsatzAbos > 0)
		{
			text = text + tS_.GetText(1236) + "\n";
		}
		if (gS_.umsatzInApp > 0)
		{
			text = text + tS_.GetText(1177) + "\n";
		}
		if (gS_.gameTyp != 2)
		{
			text += tS_.GetText(1239);
		}
		uiObjects[3].GetComponent<Text>().text = text;
		text = "";
		text = text + gS_.weeksOnMarket + "\n";
		if (gS_.bestChartPosition != 0)
		{
			text = text + gS_.bestChartPosition + "\n";
		}
		text = text + mS_.GetMoney(gS_.sellsTotal, showDollar: false) + "\n";
		if (gS_.gameTyp != 2 && !gS_.handy)
		{
			if (gS_.sellsTotalStandard > 0)
			{
				text = text + mS_.GetMoney(gS_.sellsTotalStandard, showDollar: false) + "\n";
			}
			if (gS_.sellsTotalDeluxe > 0)
			{
				text = text + mS_.GetMoney(gS_.sellsTotalDeluxe, showDollar: false) + "\n";
			}
			if (gS_.sellsTotalCollectors > 0)
			{
				text = text + mS_.GetMoney(gS_.sellsTotalCollectors, showDollar: false) + "\n";
			}
			if (gS_.sellsTotalOnline > 0)
			{
				text = text + mS_.GetMoney(gS_.sellsTotalOnline, showDollar: false) + "\n";
			}
		}
		text += "\n";
		text += "<color=#A40000>";
		text = (gS_.pubOffer ? (text + mS_.GetMoney(gS_.PUBOFFER_GetGarantiesumme(), showDollar: true) + "\n") : (text + mS_.GetMoney(gS_.GetEntwicklungskosten(), showDollar: true) + "\n"));
		if (gS_.PUBOFFER_GetGewinnbeteiligung() > 0)
		{
			text = text + mS_.GetMoney(gS_.GetUmsatzbeteiligung(), showDollar: true) + "\n";
		}
		if (gS_.GetMarketingkosten() > 0)
		{
			text = text + mS_.GetMoney(gS_.GetMarketingkosten(), showDollar: true) + "\n";
		}
		if (gS_.costs_enginegebuehren > 0)
		{
			text = text + mS_.GetMoney(gS_.costs_enginegebuehren, showDollar: true) + "\n";
		}
		if (gS_.costs_production > 0)
		{
			text = text + mS_.GetMoney(gS_.costs_production, showDollar: true) + "\n";
		}
		if (gS_.costs_updates > 0)
		{
			text = text + mS_.GetMoney(gS_.costs_updates, showDollar: true) + "\n";
		}
		if (gS_.costs_server > 0)
		{
			text = text + mS_.GetMoney(gS_.costs_server, showDollar: true) + "\n";
		}
		text += "</color>";
		text += "\n";
		text += "<color=green>";
		if (gS_.umsatzAbos > 0)
		{
			text = text + mS_.GetMoney(gS_.umsatzAbos, showDollar: true) + "\n";
		}
		if (gS_.umsatzInApp > 0)
		{
			text = text + mS_.GetMoney(gS_.umsatzInApp, showDollar: true) + "\n";
		}
		if (gS_.gameTyp != 2)
		{
			text += mS_.GetMoney(gS_.GetUmsatzVerkauf(), showDollar: true);
		}
		text += "</color>";
		uiObjects[4].GetComponent<Text>().text = text;
		if (gS_.GetGesamtGewinn() < 0)
		{
			uiObjects[5].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>";
		}
		if (!gS_.exklusiv && !gS_.herstellerExklusiv)
		{
			if (uiObjects[6].activeSelf)
			{
				uiObjects[6].SetActive(value: false);
			}
			return;
		}
		if (!uiObjects[6].activeSelf)
		{
			uiObjects[6].SetActive(value: true);
		}
		if (!gS_.gamePlatformScript[0])
		{
			gS_.FindMyPlatforms();
		}
		if ((bool)gS_.gamePlatformScript[0])
		{
			if (gS_.exklusiv)
			{
				string text2 = tS_.GetText(1314);
				text2 = text2.Replace("<NUM>", mS_.GetMoney(gS_.exklusivKonsolenSells, showDollar: false));
				text2 = text2.Replace("<NAME>", gS_.gamePlatformScript[0].GetName());
				gS_.gamePlatformScript[0].SetPic(uiObjects[7]);
				uiObjects[6].GetComponent<Text>().text = text2;
			}
			if (!gS_.herstellerExklusiv)
			{
				return;
			}
			string text3 = "[" + gS_.gamePlatformScript[0].GetName();
			for (int i = 1; i < gS_.gamePlatformScript.Length; i++)
			{
				if ((bool)gS_.gamePlatformScript[i])
				{
					text3 = text3 + ", " + gS_.gamePlatformScript[i].GetName();
				}
			}
			text3 += "]";
			string text4 = tS_.GetText(1697);
			text4 = text4.Replace("<NUM>", mS_.GetMoney(gS_.exklusivKonsolenSells, showDollar: false) + " " + text3);
			uiObjects[7].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[6].GetComponent<Text>().text = text4;
		}
		else
		{
			uiObjects[6].GetComponent<Text>().text = "";
			uiObjects[7].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_Close()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		gS_ = null;
		if (listMenu.Count > 0)
		{
			base.gameObject.SetActive(value: false);
			base.gameObject.SetActive(value: true);
			Init(listMenu[0], selbstVomMarktGenommen: false);
			listMenu.RemoveAt(0);
		}
		else
		{
			base.gameObject.SetActive(value: false);
			if (!guiMain_.uiObjects[223].activeSelf && !guiMain_.uiObjects[407].activeSelf)
			{
				guiMain_.CloseMenu();
			}
		}
	}
}
