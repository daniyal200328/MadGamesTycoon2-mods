using UnityEngine;
using UnityEngine.UI;

public class Menu_Engine_Abrechnung : MonoBehaviour
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

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		gS_.FindMyEngineNew();
		Debug.Log("ENGINE ABRECHNUNG: " + game_.myName + " / " + game_.myID);
		if ((bool)gS_.engineS_ && (gS_.IsMyAuftragsspiel() || gS_.engineGewinnbeteiligung <= 0))
		{
			BUTTON_Close();
			return;
		}
		if (!gS_.devS_ && gS_.developerID != -1)
		{
			gS_.FindMyDeveloper();
		}
		if (!gS_.pS_)
		{
			gS_.FindMyPublisher();
		}
		if (!gS_ || !gS_.engineS_ || !gS_.pS_)
		{
			Debug.Log("MENU_ENGINE_ABRECHNUNG(): Abbruch");
			BUTTON_Close();
			return;
		}
		if ((bool)gS_.devS_ && gS_.devS_.IsMyTochterfirma())
		{
			Debug.Log("MENU_ENGINE_ABRECHNUNG(): Abbruch -> Ist meine Tochterfirma");
			BUTTON_Close();
			return;
		}
		if (gS_.engineGewinnbeteiligung > 99)
		{
			gS_.engineGewinnbeteiligung = 99;
		}
		if (gS_.engineGewinnbeteiligung < 0)
		{
			gS_.engineGewinnbeteiligung = 0;
		}
		string newValue = "";
		if ((bool)gS_.devS_)
		{
			newValue = gS_.devS_.GetName();
		}
		uiObjects[0].GetComponent<Text>().text = gS_.engineS_.GetName();
		string text = "";
		text = tS_.GetText(502);
		text = text.Replace("<NAME1>", newValue);
		text = text.Replace("<NAME2>", gS_.GetNameWithTag());
		uiObjects[2].GetComponent<Text>().text = text;
		if (gS_.IsMyAuftragsspiel())
		{
			text = tS_.GetText(1815);
			text = text.Replace("<NAME1>", gS_.GetNameWithTag());
			uiObjects[2].GetComponent<Text>().text = text;
		}
		text = "";
		text = text + tS_.GetText(491) + "\n";
		text = text + tS_.GetText(275) + "\n";
		text += "\n";
		text = text + tS_.GetText(489) + "\n";
		text = text + tS_.GetText(503) + "\n";
		text = text + tS_.GetText(504) + "\n";
		uiObjects[3].GetComponent<Text>().text = text;
		text = "";
		text = text + gS_.weeksOnMarket + "\n";
		text = text + mS_.GetMoney(gS_.sellsTotal, showDollar: false) + "\n";
		text += "\n";
		text = text + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "\n";
		text = text + gS_.engineGewinnbeteiligung + "%\n";
		float num = 0f;
		num = gS_.GetGesamtGewinn();
		num = num / 100f * (float)gS_.engineGewinnbeteiligung / (float)gS_.sellsTotal;
		text = text + mS_.Round(num, 2) + "$\n";
		uiObjects[4].GetComponent<Text>().text = text;
		num = gS_.GetGesamtGewinn();
		num = num / 100f * (float)gS_.engineGewinnbeteiligung;
		uiObjects[5].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(Mathf.RoundToInt(num), showDollar: true) + "</color>";
		if (num <= 0f)
		{
			Debug.Log("ENGINE ABRECHNUNG: ABBRUCH -> " + gS_.myID);
			BUTTON_Close();
			return;
		}
		gS_.engineS_.umsatz += Mathf.RoundToInt(num);
		mS_.Earn(Mathf.RoundToInt(num), 4);
		if (mS_.settings_.disableEngineAbrechnung)
		{
			BUTTON_Close();
		}
		sfx_.PlaySound(40, force: false);
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
		guiMain_.CloseMenu();
	}
}
