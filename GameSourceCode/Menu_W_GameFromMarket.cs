using UnityEngine;
using UnityEngine.UI;

public class Menu_W_GameFromMarket : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private gameScript game_;

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

	public void Init(gameScript gS_)
	{
		game_ = gS_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)game_)
		{
			if (game_.IsMyGame())
			{
				guiMain_.ActivateMenu(guiMain_.uiObjects[82]);
				guiMain_.uiObjects[82].GetComponent<Menu_GameFromMarket>().Init(game_, selbstVomMarktGenommen: true);
				game_.RemoveFromMarket();
				if (mS_.multiplayer)
				{
					if (mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_GameRemoveFromMarket(game_);
					}
					if (mS_.mpCalls_.isClient)
					{
						mS_.mpCalls_.CLIENT_Send_GameRemoveFromMarket(game_);
					}
				}
			}
			else
			{
				if (game_.GetPublisherOrDeveloperIsTochterfirma() && (bool)guiMain_)
				{
					guiMain_.OpenTochterfirmaAbrechnung(game_);
				}
				game_.RemoveFromMarket();
				if (mS_.multiplayer)
				{
					if (mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_GameRemoveFromMarket(game_);
					}
					if (mS_.mpCalls_.isClient)
					{
						mS_.mpCalls_.CLIENT_Send_GameRemoveFromMarket(game_);
					}
				}
			}
		}
		base.gameObject.SetActive(value: false);
	}
}
