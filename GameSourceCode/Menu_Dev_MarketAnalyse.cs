using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_MarketAnalyse : MonoBehaviour
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

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(16);
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		string text = tS_.GetText(441);
		text = text.Replace("<GENRE>", genres_.GetName(gS_.maingenre));
		text = text.Replace("<THEME>", tS_.GetThemes(gS_.gameMainTheme));
		uiObjects[10].GetComponent<Text>().text = text;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(gS_.maingenre);
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] != -1)
			{
				GameObject gameObject = GameObject.Find("PLATFORM_" + gS_.gamePlatform[i]);
				if ((bool)gameObject)
				{
					platformScript component = gameObject.GetComponent<platformScript>();
					uiObjects[6 + i].SetActive(value: true);
					component.SetPic(uiObjects[6 + i]);
					uiObjects[6 + i].GetComponent<tooltip>().c = component.GetTooltip();
				}
			}
			else
			{
				uiObjects[6 + i].SetActive(value: false);
			}
		}
		Vector4 amountGamesWithGenreAndTopic = games_.GetAmountGamesWithGenreAndTopic(gS_);
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(amountGamesWithGenreAndTopic.x).ToString();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(amountGamesWithGenreAndTopic.y).ToString();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(amountGamesWithGenreAndTopic.z).ToString();
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(amountGamesWithGenreAndTopic.w).ToString();
		if (gS_.typ_contractGame)
		{
			BUTTON_Close();
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[195]);
		guiMain_.uiObjects[195].GetComponent<Menu_Dev_USK>().Init(gS_);
	}
}
