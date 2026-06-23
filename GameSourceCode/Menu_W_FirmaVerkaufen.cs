using UnityEngine;
using UnityEngine.UI;

public class Menu_W_FirmaVerkaufen : MonoBehaviour
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

	private gamepassScript gpS_;

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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
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
		if ((bool)pS_)
		{
			string text = tS_.GetText(1974);
			text = text.Replace("<NAME>", "<color=blue>" + pS_.GetName() + "</color>");
			text = text.Replace("<NUM>", "<color=blue>" + pS_.GetFirmenwertString() + "</color>");
			uiObjects[0].GetComponent<Text>().text = text;
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
		if ((bool)pS_)
		{
			mS_.Earn(pS_.GetFirmenwert(), 12);
			pS_.RemoveTochterfirma();
			pS_.ResetTochterfirmaSettings();
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_PublisherTochterfirmaSettings(pS_);
					mS_.mpCalls_.SERVER_Send_Publisher(pS_);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_PublisherTochterfirmaSettings(pS_);
					mS_.mpCalls_.CLIENT_Send_Publisher(pS_);
				}
			}
			int num = 0;
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].IsMyIP(pS_) && games_.arrayGamesScripts[i].inGamePass)
				{
					gpS_.GAMEPASS_RemoveGame(games_.arrayGamesScripts[i], updateGamesAmount: false);
					num++;
				}
			}
			gpS_.GetAmountGamePassGames();
			if (num > 0)
			{
				string text = tS_.GetText(2120);
				text = text.Replace("<NUM>", num.ToString());
				guiMain_.MessageBox(text, closeMenu: false);
			}
			if (guiMain_.uiObjects[387].activeSelf)
			{
				guiMain_.uiObjects[387].SetActive(value: false);
			}
			if (guiMain_.uiObjects[385].activeSelf)
			{
				guiMain_.uiObjects[385].GetComponent<Menu_Statistics_Tochterfirmen>().BUTTON_Search();
			}
		}
		BUTTON_Abbrechen();
	}
}
