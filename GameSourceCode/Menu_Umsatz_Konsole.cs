using UnityEngine;
using UnityEngine.UI;

public class Menu_Umsatz_Konsole : MonoBehaviour
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

	private platforms platforms_;

	private platformScript pS_;

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
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public void Init(platformScript plat_)
	{
		FindScripts();
		pS_ = plat_;
		if (!pS_)
		{
			BUTTON_Close();
			return;
		}
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Text>().text = pS_.GetManufacturer();
		pS_.SetPic(uiObjects[7]);
		string text = "";
		text = "";
		text = text + tS_.GetText(491) + "\n";
		text = text + tS_.GetText(696) + "\n";
		text += "\n";
		text = text + tS_.GetText(6) + "\n";
		text = text + tS_.GetText(492) + "\n";
		text = text + tS_.GetText(2369) + "\n";
		text = text + tS_.GetText(530) + "\n";
		text = text + tS_.GetText(2382) + "\n";
		text += "\n";
		text += tS_.GetText(1239);
		uiObjects[3].GetComponent<Text>().text = text;
		text = "";
		text = text + pS_.weeksOnMarket + "\n";
		text = text + mS_.GetMoney(pS_.units, showDollar: false) + "\n";
		text += "\n";
		text += "<color=#A40000>";
		text = text + mS_.GetMoney(pS_.GetEntwicklungskosten(), showDollar: true) + "\n";
		text = text + mS_.GetMoney(pS_.costs_marketing, showDollar: true) + "\n";
		text = text + mS_.GetMoney(pS_.costs_garantie, showDollar: true) + "\n";
		text = text + mS_.GetMoney(pS_.costs_production, showDollar: true) + "\n";
		text = text + mS_.GetMoney(pS_.costs_subvention, showDollar: true) + "\n";
		text += "</color>";
		text += "\n";
		text += "<color=green>";
		text += mS_.GetMoney(pS_.umsatzTotal, showDollar: true);
		text += "</color>";
		uiObjects[4].GetComponent<Text>().text = text;
		if (pS_.GetGesamtGewinn() < 0)
		{
			uiObjects[5].GetComponent<Text>().text = "<color=#A40000>" + mS_.GetMoney(pS_.GetGesamtGewinn(), showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(pS_.GetGesamtGewinn(), showDollar: true) + "</color>";
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
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (!guiMain_.uiObjects[335].activeSelf && !guiMain_.uiObjects[336].activeSelf && !guiMain_.uiObjects[343].activeSelf)
		{
			guiMain_.CloseMenu();
		}
	}
}
