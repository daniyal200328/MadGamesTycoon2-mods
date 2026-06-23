using UnityEngine;
using UnityEngine.UI;

public class Menu_Abo_Preis : MonoBehaviour
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
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		if (game_.aboPreis <= 0)
		{
			game_.aboPreisOld = 5;
			uiObjects[2].GetComponent<Slider>().value = 5f;
		}
		else
		{
			uiObjects[2].GetComponent<Slider>().value = gS_.aboPreis;
		}
		SLIDER_AboPreis();
	}

	public void SLIDER_AboPreis()
	{
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value), showDollar: true);
	}

	public void BUTTON_Close()
	{
		BUTTON_Ok();
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		gS_.aboPreis = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		if (!guiMain_.uiObjects[245].activeSelf)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[71]);
			guiMain_.uiObjects[71].GetComponent<Menu_Dev_XP>().Init(gS_);
		}
		base.gameObject.SetActive(value: false);
	}
}
