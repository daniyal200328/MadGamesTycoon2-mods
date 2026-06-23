using UnityEngine;
using UnityEngine.UI;

public class Menu_ReleaseDate_F2P : MonoBehaviour
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

	private taskGame task_;

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

	public void Init(gameScript game_, taskGame t_)
	{
		FindScripts();
		gS_ = game_;
		task_ = t_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		SLIDER_Wochen();
	}

	public void SLIDER_Wochen()
	{
		if (uiObjects[2].GetComponent<Slider>().value > 1f)
		{
			string text = tS_.GetText(1123);
			text = text.Replace("<NUM>", Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value).ToString());
			uiObjects[1].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1864);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)task_)
		{
			Object.Destroy(task_.gameObject);
		}
		gS_.releaseDate = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		if (gS_.reviewTotal <= 0)
		{
			gS_.CalcReview(entwicklungsbericht: false);
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		else
		{
			gS_.date_year = mS_.year;
			gS_.date_month = mS_.month;
			if ((bool)mS_)
			{
				mS_.reviewText_.GetReviewText(gS_);
			}
		}
		if (gS_.handy && gS_.gameTyp != 2)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[301]);
			guiMain_.uiObjects[301].GetComponent<Menu_HandyPreis>().Init(gS_);
		}
		else
		{
			gS_.SetPublisher(mS_.myID);
			gS_.SetOnMarket();
			guiMain_.ActivateMenu(guiMain_.uiObjects[71]);
			guiMain_.uiObjects[71].GetComponent<Menu_Dev_XP>().Init(gS_);
		}
		guiMain_.uiObjects[69].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}
}
