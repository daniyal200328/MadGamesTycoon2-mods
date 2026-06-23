using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_USK : MonoBehaviour
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

	private gamepassScript gpS_;

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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(17);
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		string text = tS_.GetText(991);
		text = text.Replace("<NAME>", gS_.GetNameWithTag());
		uiObjects[0].GetComponent<Text>().text = text;
		uiObjects[1].GetComponent<Image>().sprite = games_.gamePEGI[gS_.usk];
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(18);
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[46]);
		if (gS_.typ_contractGame)
		{
			guiMain_.uiObjects[46].GetComponent<Menu_Review>().InitContractGame(gS_);
		}
		else if (gpS_.gamePass_aktiv && !gS_.inGamePass && gS_.CanBeInGamePass())
		{
			guiMain_.uiObjects[46].GetComponent<Menu_Review>().InitGamePassFrage(gS_);
		}
		else
		{
			guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(gS_);
		}
	}
}
