using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Tochterfirma_GameEntwicklungsbericht : MonoBehaviour
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
		if ((bool)main_)
		{
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
	}

	private void OnEnable()
	{
		FindScripts();
	}

	private void OnDisable()
	{
		FindScripts();
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		GameObject gameObject = GameObject.Find("PUB_" + gS_.developerID);
		if (!gameObject)
		{
			return;
		}
		publisherScript component = gameObject.GetComponent<publisherScript>();
		float entwicklungsFortschritt = component.GetEntwicklungsFortschritt();
		if (gS_.GetNameWithTag().Replace(" <color=green>[★]</color>", "").Contains("<"))
		{
			uiObjects[0].GetComponent<InputField>().text = "";
			uiObjects[0].GetComponent<InputField>().interactable = false;
			uiObjects[22].GetComponent<Text>().text = gS_.GetNameWithTag();
		}
		else
		{
			uiObjects[0].GetComponent<InputField>().text = gS_.GetNameSimple();
			uiObjects[0].GetComponent<InputField>().interactable = true;
			uiObjects[22].GetComponent<Text>().text = "";
		}
		uiObjects[20].GetComponent<Image>().sprite = games_.gameSizeSprites[gS_.gameSize];
		string text = "";
		text = genres_.GetName(gS_.maingenre);
		if (gS_.subgenre != -1)
		{
			text = text + " / " + genres_.GetName(gS_.subgenre);
		}
		text = text + "\n" + tS_.GetThemes(gS_.gameMainTheme);
		if (gS_.gameSubTheme != -1)
		{
			text = text + " / " + tS_.GetThemes(gS_.gameSubTheme);
		}
		uiObjects[1].GetComponent<Text>().text = text;
		uiObjects[2].GetComponent<Image>().fillAmount = entwicklungsFortschritt;
		if (component.newGameInWeeks > 2)
		{
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(1944) + ": " + Mathf.RoundToInt(entwicklungsFortschritt * 100f) + "%";
		}
		else
		{
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(1947);
		}
		int num = component.newGameInWeeks;
		if (num < 2)
		{
			num = 2;
		}
		text = tS_.GetText(1948);
		text = text.Replace("<NUM>", "<color=blue><b>" + num + "</b></color>");
		uiObjects[31].GetComponent<Text>().text = text;
		gS_.CalcReview(entwicklungsbericht: true);
		float f = (float)gS_.reviewTotal * entwicklungsFortschritt;
		int num2 = Mathf.RoundToInt(f) - 10;
		int num3 = Mathf.RoundToInt(f) + 10;
		num2 = num2 / 10 * 10;
		num3 = num3 / 10 * 10;
		if (num2 < 1)
		{
			num2 = 1;
		}
		if (num3 > 100)
		{
			num3 = 100;
		}
		text = " " + num2 + "% - " + num3 + "%";
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(452) + "<color=blue>" + text + "</color>";
		gS_.ClearReview();
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetScreenshot(gS_.maingenre, Mathf.RoundToInt(gS_.points_grafik * entwicklungsFortschritt));
		uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay * entwicklungsFortschritt).ToString();
		uiObjects[8].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik * entwicklungsFortschritt).ToString();
		uiObjects[9].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound * entwicklungsFortschritt).ToString();
		uiObjects[10].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik * entwicklungsFortschritt).ToString();
		uiObjects[35].GetComponent<Text>().text = mS_.Round(gS_.GetIpBekanntheit(), 1).ToString();
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] != -1)
			{
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + gS_.gamePlatform[i]);
				if ((bool)gameObject2)
				{
					platformScript component2 = gameObject2.GetComponent<platformScript>();
					uiObjects[13 + i].SetActive(value: true);
					component2.SetPic(uiObjects[13 + i]);
					uiObjects[13 + i].GetComponent<tooltip>().c = component2.GetTooltip();
				}
			}
			else
			{
				uiObjects[13 + i].SetActive(value: false);
			}
		}
		uiObjects[17].GetComponent<Component_Aufwertungen>().InitNpcGame(gS_, entwicklungsFortschritt);
		uiObjects[18].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[24].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[19].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[25].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
	}

	public void BUTTON_Close()
	{
		ReplaceName();
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	private void ReplaceName()
	{
		if (uiObjects[0].GetComponent<InputField>().interactable && uiObjects[0].GetComponent<InputField>().text.Length > 0)
		{
			gS_.myName = uiObjects[0].GetComponent<InputField>().text;
			guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().UpdateData();
		}
	}
}
