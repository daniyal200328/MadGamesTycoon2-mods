using UnityEngine;
using UnityEngine.UI;

public class Menu_W_AutoGamePass : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private games games_;

	private gamepassScript gpS_;

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

	public void Init(gameScript gS_)
	{
		FindScripts();
		game_ = gS_;
		SetData();
	}

	private void SetData()
	{
		if (!game_)
		{
			BUTTON_Abbrechen();
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_Yes()
	{
		if ((bool)game_ && !game_.inGamePass && gpS_.gamePass_aktiv && game_.CanBeInGamePass())
		{
			gpS_.GAMEPASS_AddGame(game_, updateGamesAmount: true);
		}
		BUTTON_Abbrechen();
	}
}
