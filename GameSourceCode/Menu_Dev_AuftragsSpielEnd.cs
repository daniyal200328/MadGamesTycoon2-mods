using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_AuftragsSpielEnd : MonoBehaviour
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
		long num = 0L;
		if ((bool)mS_.achScript_)
		{
			mS_.achScript_.SetAchivement(26);
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		float num2 = (float)gS_.reviewTotal / 20f;
		if (!gS_.auftragsspiel_zeitAbgelaufen)
		{
			uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
			uiObjects[3].GetComponent<Text>().text = "<color=green>+" + mS_.Round(num2, 2) + "</color>";
			guiMain_.UpdateAuftragsansehen(num2);
		}
		else
		{
			uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			uiObjects[3].GetComponent<Text>().text = "<color=red>-5.0%</color>";
			guiMain_.UpdateAuftragsansehen(-5f);
		}
		string text = tS_.GetText(626);
		text = text.Replace("<NUM>", gS_.auftragsspiel_mindestbewertung.ToString());
		uiObjects[4].GetComponent<Text>().text = text;
		if (gS_.reviewTotal >= gS_.auftragsspiel_mindestbewertung)
		{
			uiObjects[5].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
			uiObjects[6].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(gS_.auftragsspiel_bonus, showDollar: true) + "</color>";
			mS_.Earn(gS_.auftragsspiel_bonus, 6);
			uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(gS_.auftragsspiel_bonus + gS_.auftragsspiel_gehalt, showDollar: true);
			num = gS_.auftragsspiel_bonus + gS_.auftragsspiel_gehalt - gS_.GetEntwicklungskosten();
		}
		else
		{
			uiObjects[5].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
			uiObjects[6].GetComponent<Text>().text = "<color=black>$0</color>";
			uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(gS_.auftragsspiel_gehalt, showDollar: true);
			num = gS_.auftragsspiel_gehalt - gS_.GetEntwicklungskosten();
		}
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(gS_.GetEntwicklungskosten(), showDollar: true);
		if (num > 0)
		{
			uiObjects[9].GetComponent<Text>().text = "<color=green>" + mS_.GetMoney(num, showDollar: true) + "</color>";
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = "<color=red>" + mS_.GetMoney(num, showDollar: true) + "</color>";
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
		guiMain_.CloseMenu();
	}
}
