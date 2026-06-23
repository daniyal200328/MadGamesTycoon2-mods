using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ChangeDesignproritaet : MonoBehaviour
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

	private platforms platforms_;

	public gameScript gS_;

	public int g_GameAP_Gameplay;

	public int g_GameAP_Grafik;

	public int g_GameAP_Sound;

	public int g_GameAP_Technik;

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
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay).ToString();
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik).ToString();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound).ToString();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik).ToString();
		g_GameAP_Gameplay = gS_.gameAP_Gameplay;
		g_GameAP_Grafik = gS_.gameAP_Grafik;
		g_GameAP_Sound = gS_.gameAP_Sound;
		g_GameAP_Technik = gS_.gameAP_Technik;
		uiObjects[5].GetComponent<Slider>().value = g_GameAP_Gameplay;
		uiObjects[6].GetComponent<Slider>().value = g_GameAP_Grafik;
		uiObjects[7].GetComponent<Slider>().value = g_GameAP_Sound;
		uiObjects[8].GetComponent<Slider>().value = g_GameAP_Technik;
	}

	private int UpdateGesamtArbeitsprioritaet()
	{
		float value = uiObjects[5].GetComponent<Slider>().value;
		value += uiObjects[6].GetComponent<Slider>().value;
		value += uiObjects[7].GetComponent<Slider>().value;
		value += uiObjects[8].GetComponent<Slider>().value;
		value *= 5f;
		uiObjects[13].GetComponent<Text>().text = Mathf.RoundToInt(value) + "%";
		if (Mathf.RoundToInt(value) > 100)
		{
			uiObjects[13].GetComponent<Text>().color = Color.red;
		}
		else
		{
			uiObjects[13].GetComponent<Text>().color = guiMain_.colors[6];
		}
		float num = value;
		num *= 0.01f;
		if (num > 1f)
		{
			num = 1f;
		}
		uiObjects[14].GetComponent<Image>().fillAmount = num;
		return Mathf.RoundToInt(value);
	}

	public void SetAP_Gameplay()
	{
		g_GameAP_Gameplay = Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value);
		uiObjects[9].GetComponent<Text>().text = g_GameAP_Gameplay * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Grafik()
	{
		g_GameAP_Grafik = Mathf.RoundToInt(uiObjects[6].GetComponent<Slider>().value);
		uiObjects[10].GetComponent<Text>().text = g_GameAP_Grafik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Sound()
	{
		g_GameAP_Sound = Mathf.RoundToInt(uiObjects[7].GetComponent<Slider>().value);
		uiObjects[11].GetComponent<Text>().text = g_GameAP_Sound * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void SetAP_Technik()
	{
		g_GameAP_Technik = Mathf.RoundToInt(uiObjects[8].GetComponent<Slider>().value);
		uiObjects[12].GetComponent<Text>().text = g_GameAP_Technik * 5 + "%";
		UpdateGesamtArbeitsprioritaet();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_OK()
	{
		if ((bool)gS_)
		{
			sfx_.PlaySound(3, force: true);
			if (UpdateGesamtArbeitsprioritaet() > 100)
			{
				guiMain_.MessageBox(tS_.GetText(400), closeMenu: false);
				return;
			}
			if (UpdateGesamtArbeitsprioritaet() < 100)
			{
				guiMain_.MessageBox(tS_.GetText(416), closeMenu: false);
				return;
			}
			gS_.gameAP_Gameplay = g_GameAP_Gameplay;
			gS_.gameAP_Grafik = g_GameAP_Grafik;
			gS_.gameAP_Sound = g_GameAP_Sound;
			gS_.gameAP_Technik = g_GameAP_Technik;
			base.gameObject.SetActive(value: false);
			guiMain_.CloseMenu();
		}
	}
}
