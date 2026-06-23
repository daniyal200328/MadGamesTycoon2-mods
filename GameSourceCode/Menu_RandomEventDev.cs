using UnityEngine;
using UnityEngine.UI;

public class Menu_RandomEventDev : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

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

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript gS_)
	{
		Debug.Log("RandomEventDev()");
		if (!gS_)
		{
			BUTTON_Abbrechen();
		}
		FindScripts();
		uiObjects[1].GetComponent<Text>().text = gS_.GetNameWithTag();
		float num = 0f;
		switch (Random.Range(0, 14))
		{
		case 0:
			num = gS_.points_grafik * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_grafik -= num;
			if (gS_.points_grafik < 0f)
			{
				gS_.points_grafik = 0f;
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1066);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[21];
			sfx_.PlaySound(53, force: true);
			break;
		case 1:
			num = gS_.points_sound * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_sound += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1067);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[22];
			sfx_.PlaySound(54, force: true);
			break;
		case 2:
			num = Random.Range(25, 35);
			gS_.points_bugs += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1068);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[24];
			sfx_.PlaySound(53, force: true);
			break;
		case 3:
			num = gS_.points_technik * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_technik += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1069);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[23];
			sfx_.PlaySound(54, force: true);
			break;
		case 4:
			num = gS_.points_gameplay * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_gameplay += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1070);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[20];
			sfx_.PlaySound(54, force: true);
			break;
		case 5:
			num = Random.Range(10, 30);
			gS_.AddHype(0f - num);
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1071);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[25];
			sfx_.PlaySound(53, force: true);
			break;
		case 6:
			num = Random.Range(10, 30);
			gS_.AddHype(num);
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1072);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[25];
			sfx_.PlaySound(54, force: true);
			break;
		case 7:
			num = gS_.points_sound * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_sound -= num;
			if (gS_.points_sound < 0f)
			{
				gS_.points_sound = 0f;
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1073);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[22];
			sfx_.PlaySound(53, force: true);
			break;
		case 8:
			num = gS_.points_technik * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_technik -= num;
			if (gS_.points_technik < 0f)
			{
				gS_.points_technik = 0f;
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1074);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[23];
			sfx_.PlaySound(53, force: true);
			break;
		case 9:
			num = gS_.points_gameplay * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_gameplay -= num;
			if (gS_.points_gameplay < 0f)
			{
				gS_.points_gameplay = 0f;
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1075);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[20];
			sfx_.PlaySound(53, force: true);
			break;
		case 10:
			num = Random.Range(25, 35);
			gS_.points_bugs -= num;
			if (gS_.points_bugs < 0f)
			{
				gS_.points_bugs = 0f;
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1076);
			uiObjects[2].GetComponent<Text>().text = "-" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[24];
			sfx_.PlaySound(54, force: true);
			break;
		case 11:
			num = gS_.points_grafik * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_grafik += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1077);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[21];
			sfx_.PlaySound(54, force: true);
			break;
		case 12:
			num = gS_.points_sound * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_sound += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1078);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[22];
			sfx_.PlaySound(54, force: true);
			break;
		case 13:
			num = gS_.points_technik * Random.Range(0.01f, 0.1f) + (float)Random.Range(5, 10);
			gS_.points_technik += num;
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1079);
			uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
			uiObjects[2].GetComponent<Text>().color = guiMain_.colors[7];
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[23];
			sfx_.PlaySound(54, force: true);
			break;
		}
		if ((bool)mS_.settings_ && mS_.settings_.hideEvents)
		{
			BUTTON_Abbrechen();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
