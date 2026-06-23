using UnityEngine;
using UnityEngine.UI;

public class Menu_TochterfirmaAbrechnung : MonoBehaviour
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
		if (!gS_)
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
		if (!gS_ || !gS_.pS_ || !gS_.devS_)
		{
			Debug.Log("MENU_TOCHTERFIRMA_ABRECHNUNG(): Abbruch -> Script fehlt");
			BUTTON_Close();
			return;
		}
		if (!gS_.GetPublisherOrDeveloperIsTochterfirma())
		{
			Debug.Log("MENU_TOCHTERFIRMA_ABRECHNUNG(): Abbruch -> Ist nicht meine Tochterfirma");
			BUTTON_Close();
			return;
		}
		publisherScript publisherScript2 = null;
		if (gS_.GetPublisherIsTochtefirma())
		{
			publisherScript2 = gS_.pS_;
		}
		if (gS_.GetDeveloperIsTochtefirma())
		{
			publisherScript2 = gS_.devS_;
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[6].GetComponent<Image>().sprite = publisherScript2.GetLogo();
		string text = "";
		text = tS_.GetText(1988);
		if (gS_.GetPublisherIsTochtefirma())
		{
			text = text.Replace("<NAME1>", "<color=blue>" + gS_.pS_.GetName() + "</color>");
		}
		if (gS_.GetDeveloperIsTochtefirma())
		{
			text = text.Replace("<NAME1>", "<color=blue>" + gS_.devS_.GetName() + "</color>");
		}
		text = text.Replace("<NAME2>", "<color=blue>" + gS_.GetNameWithTag() + "</color>");
		uiObjects[2].GetComponent<Text>().text = text;
		text = "";
		text = text + tS_.GetText(491) + "\n";
		text = text + tS_.GetText(275) + "\n";
		text += "\n";
		text = text + tS_.GetText(276) + "\n";
		text = text + tS_.GetText(1990) + "\n";
		text = text + tS_.GetText(503) + "\n";
		uiObjects[3].GetComponent<Text>().text = text;
		text = "";
		text = text + gS_.weeksOnMarket + "\n";
		text = text + mS_.GetMoney(gS_.sellsTotal, showDollar: false) + "\n";
		text += "\n";
		text = text + mS_.GetMoney(gS_.umsatzTotal, showDollar: true) + "\n";
		text = text + "<color=red>" + mS_.GetMoney(gS_.umsatzTotal - gS_.tw_gewinnanteil * 2, showDollar: true) + "</color>\n";
		text = text + Mathf.RoundToInt(games_.tf_gewinnbeteiligungTochterfirma) + "%\n";
		uiObjects[4].GetComponent<Text>().text = text;
		uiObjects[5].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(gS_.tw_gewinnanteil, showDollar: true) + "</color>";
		if (mS_.settings_.disableTochterfirmaAbrechnung)
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
