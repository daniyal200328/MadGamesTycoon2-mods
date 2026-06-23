using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_AuftragsSpielGehalt : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private gameScript gS_;

	public int myID;

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
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		if (!game_)
		{
			return;
		}
		gS_ = game_;
		string text = tS_.GetText(630);
		text = text.Replace("<NUM1>", mS_.GetMoney(gS_.auftragsspiel_gehalt, showDollar: true));
		text = text.Replace("<NUM2>", mS_.GetMoney(gS_.auftragsspiel_bonus, showDollar: true));
		text = text.Replace("<NUM3>", gS_.auftragsspiel_mindestbewertung + "%");
		uiObjects[0].GetComponent<Text>().text = text;
		GameObject gameObject = GameObject.Find("PUB_" + gS_.publisherID);
		if ((bool)gameObject)
		{
			uiObjects[1].GetComponent<Image>().sprite = gameObject.GetComponent<publisherScript>().GetLogo();
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(gS_);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(gS_);
			}
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_Abbrechen()
	{
		if ((bool)gS_)
		{
			mS_.Earn(gS_.auftragsspiel_gehalt, 6);
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}
}
