using UnityEngine;
using UnityEngine.UI;

public class Menu_Result_MarketingSpezial : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript gS_, int kampagne)
	{
		if (!gS_)
		{
			BUTTON_Abbrechen();
		}
		FindScripts();
		sfx_.PlaySound(54, force: true);
		uiObjects[1].GetComponent<Text>().text = gS_.GetNameWithTag();
		if (kampagne < 100)
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiObjects[294].GetComponent<Menu_MarketingSpezial>().sprites[kampagne];
			gS_.specialMarketing[kampagne] = -1;
		}
		else
		{
			uiObjects[4].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		}
		int num = 0;
		int num2 = 0;
		switch (kampagne)
		{
		case 0:
			if ((!gS_.inDevelopment && !gS_.schublade) || gS_.reviewTotal > 0)
			{
				BUTTON_Abbrechen();
				break;
			}
			gS_.CalcReview(entwicklungsbericht: true);
			if (gS_.reviewTotal < 40)
			{
				num = -1 - Random.Range(0, gS_.reviewTotal) / 5;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1426);
				uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(num).ToString();
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
				gS_.specialMarketing[kampagne] = -1;
			}
			else
			{
				num = 1 + Random.Range(0, gS_.reviewTotal) / 5;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1427);
				uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
				gS_.specialMarketing[kampagne] = -1;
			}
			gS_.ClearReview();
			break;
		case 1:
			if ((!gS_.inDevelopment && !gS_.schublade) || gS_.reviewTotal > 0)
			{
				BUTTON_Abbrechen();
				break;
			}
			gS_.CalcReview(entwicklungsbericht: true);
			if (gS_.reviewTotal < 50)
			{
				num = -3;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1428);
				uiObjects[2].GetComponent<Text>().text = "-3%";
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[30];
				gS_.specialMarketing[kampagne] = num;
			}
			else
			{
				num = 3;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1429);
				uiObjects[2].GetComponent<Text>().text = "+3%";
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[30];
				gS_.specialMarketing[kampagne] = num;
			}
			gS_.ClearReview();
			break;
		case 2:
			if (!gS_.inDevelopment && !gS_.schublade)
			{
				BUTTON_Abbrechen();
				break;
			}
			if (Random.Range(0, 100) > 50)
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1430);
				uiObjects[2].GetComponent<Text>().text = "+0";
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
				gS_.specialMarketing[kampagne] = -1;
				break;
			}
			if ((bool)mS_.achScript_)
			{
				mS_.achScript_.SetAchivement(41);
			}
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1431);
			uiObjects[2].GetComponent<Text>().text = "200";
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
			gS_.hype = 200f;
			gS_.specialMarketing[kampagne] = -1;
			break;
		case 3:
			num = Random.Range((gS_.userPositiv + gS_.userNegativ) / 10 + 50, gS_.userPositiv + gS_.userNegativ + 100);
			if (Random.Range(0, 100) > 50)
			{
				num2 = 500 + Random.Range(genres_.GetAmountFans() / 20, genres_.GetAmountFans() / 10);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1432);
				uiObjects[2].GetComponent<Text>().text = "-" + num2;
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[34];
				gS_.specialMarketing[kampagne] = -1;
				mS_.AddFans(-num2, -1);
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1433);
				uiObjects[2].GetComponent<Text>().text = "+" + num;
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[35];
				gS_.userPositiv += num;
				gS_.specialMarketing[kampagne] = -1;
			}
			break;
		case 4:
			if (gS_.reviewTotal < 50)
			{
				num = -1 - Random.Range(0, gS_.reviewTotal) / 5;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1435);
				uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(num).ToString();
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
				gS_.specialMarketing[kampagne] = -1;
			}
			else
			{
				num = 1 + Random.Range(0, gS_.reviewTotal) / 5;
				gS_.AddHype(num);
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1436);
				uiObjects[2].GetComponent<Text>().text = "+" + Mathf.RoundToInt(num);
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[33];
				gS_.specialMarketing[kampagne] = -1;
			}
			break;
		case 100:
			num2 = 2000 + Random.Range(0, 3000);
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1437);
			uiObjects[2].GetComponent<Text>().text = "-" + num2;
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[34];
			gS_.hype = 100f;
			mS_.AddFans(-num2, -1);
			break;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
