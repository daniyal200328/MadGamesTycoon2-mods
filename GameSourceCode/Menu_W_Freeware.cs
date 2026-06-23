using UnityEngine;
using UnityEngine.UI;

public class Menu_W_Freeware : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private gameScript game_;

	private games games_;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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
		FindScripts();
		game_ = gS_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = "+" + mS_.GetMoney(gS_.GetFreewareFans(), showDollar: false);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		if (!game_)
		{
			return;
		}
		game_.freeware = true;
		mS_.AddFans((int)game_.GetFreewareFans(), game_.maingenre);
		if (game_.inGamePass)
		{
			mS_.gpS_.GAMEPASS_RemoveGame(game_, updateGamesAmount: true);
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(game_);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(game_);
			}
		}
		guiMain_.uiObjects[446].GetComponent<Menu_FreewareSelect>().Init();
		base.gameObject.SetActive(value: false);
	}
}
