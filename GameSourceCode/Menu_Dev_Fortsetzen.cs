using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Fortsetzen : MonoBehaviour
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
		if (game_.devFortsetzen > 0)
		{
			uiObjects[2].GetComponent<Slider>().value = game_.devFortsetzen - 1;
		}
		else
		{
			uiObjects[2].GetComponent<Slider>().value = 0f;
		}
		SetData();
	}

	public void SetData()
	{
		if (uiObjects[2].GetComponent<Slider>().value > 0f)
		{
			string text = tS_.GetText(609);
			text = text.Replace("<NUM>", mS_.GetMoney(Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value), showDollar: false));
			uiObjects[1].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(335);
		}
	}

	public void SLIDER_Wochen()
	{
		SetData();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		if (Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value) > 0)
		{
			gS_.devFortsetzen = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value + 1f);
		}
		else
		{
			gS_.devFortsetzen = 0;
		}
		guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().BUTTON_Close();
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator iMinusWochen(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusWochen(i);
		}
	}

	public void BUTTON_MinusWochen(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num -= i;
		if (num < 0)
		{
			num = 0;
		}
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusWochen(i));
		SetData();
	}

	private IEnumerator iPlusWochen(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusWochen(i);
		}
	}

	public void BUTTON_PlusWochen(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num += i;
		if (num > 48)
		{
			num = 48;
		}
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusWochen(i));
		SetData();
	}
}
