using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class gameTab : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public mainScript mS_;

	public GameObject main_;

	public GUI_Main guiMain_;

	public sfxScript sfx_;

	public textScript tS_;

	public themes themes_;

	public genres genres_;

	public tooltip tooltip_;

	public games games_;

	public settingsScript settings_;

	public licences licences_;

	public int gameID = -1;

	public gameScript gS_;

	public gamepassScript gpS_;

	public RectTransform myRect_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiBalken;

	public GameObject[] uiBalkenOnline;

	public Sprite[] uiSprites;

	private float fillBalken;

	private float fillBalkenOnline;

	private float sellsPerWeek;

	private float sellsTotal;

	private RectTransform rect;

	private Image myImage;

	private Button myButton;

	private int wochenChartsPlatz;

	private bool updateMyData;

	private bool fullViewOld;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
		if (!tooltip_)
		{
			tooltip_ = GetComponent<tooltip>();
		}
		if (!rect)
		{
			rect = GetComponent<RectTransform>();
		}
		if (!myImage)
		{
			myImage = GetComponent<Image>();
		}
		if (!myButton)
		{
			myButton = GetComponent<Button>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!myRect_)
		{
			myRect_ = base.gameObject.GetComponent<RectTransform>();
		}
	}

	public void Init(int gameID_)
	{
		if (!main_)
		{
			FindScripts();
		}
		gameID = gameID_;
		updateMyData = false;
		UpdateData();
		BUTTON_Minimize();
		BUTTON_Minimize();
		guiMain_.UpdateGameTabsSortierung();
	}

	private void FindMyGame()
	{
		if (gameID != -1 && !gS_)
		{
			GameObject gameObject = GameObject.Find("GAME_" + gameID);
			if ((bool)gameObject)
			{
				gS_ = gameObject.GetComponent<gameScript>();
			}
			else
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void Update()
	{
		if (guiMain_.menuOpen)
		{
			if (myButton.interactable)
			{
				myButton.interactable = false;
			}
		}
		else if (!myButton.interactable)
		{
			myButton.interactable = true;
		}
		if (!gS_)
		{
			FindMyGame();
		}
		else if ((myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height) || fullViewOld != gS_.gameTab_fullView)
		{
			updateMyData = false;
			fullViewOld = gS_.gameTab_fullView;
			updateMyData = !updateMyData;
			if (!updateMyData)
			{
				return;
			}
			float t = 12.8f * Time.deltaTime;
			if (!gS_.gameTab_fullView)
			{
				if (settings_.gameTabData)
				{
					if (Mathf.RoundToInt(rect.sizeDelta.y) != 21)
					{
						rect.sizeDelta = new Vector2(220f, 21f);
					}
				}
				else if (Mathf.RoundToInt(rect.sizeDelta.y) != 34)
				{
					rect.sizeDelta = new Vector2(220f, 34f);
				}
			}
			if (gS_.gameTab_fullView || settings_.gameTabData)
			{
				uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
				long gesamtGewinn = gS_.GetGesamtGewinn();
				if (gesamtGewinn < 0)
				{
					uiObjects[3].GetComponent<Text>().color = guiMain_.colors[5];
					uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(gesamtGewinn, showDollar: true);
				}
				else
				{
					uiObjects[3].GetComponent<Text>().color = guiMain_.colors[6];
					uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(gesamtGewinn, showDollar: true);
				}
				if (gS_.points_bugs <= 0f)
				{
					if (uiObjects[42].activeSelf)
					{
						uiObjects[42].SetActive(value: false);
					}
				}
				else if (!uiObjects[42].activeSelf)
				{
					uiObjects[42].SetActive(value: true);
				}
				if (gS_.inGamePass && gpS_.gamePass_aktiv)
				{
					if (!uiObjects[44].activeSelf)
					{
						uiObjects[44].SetActive(value: true);
					}
				}
				else if (uiObjects[44].activeSelf)
				{
					uiObjects[44].SetActive(value: false);
				}
				if ((!gS_.commercialFlop || gS_.weeksOnMarket < 4) && uiObjects[41].activeSelf)
				{
					uiObjects[41].SetActive(value: false);
				}
				if (gS_.commercialFlop && gS_.weeksOnMarket >= 4 && !uiObjects[41].activeSelf)
				{
					uiObjects[41].SetActive(value: true);
				}
				if ((!gS_.commercialHit || gS_.weeksOnMarket < 4) && uiObjects[43].activeSelf)
				{
					uiObjects[43].SetActive(value: false);
				}
				if (gS_.commercialHit && gS_.weeksOnMarket >= 4 && !uiObjects[43].activeSelf)
				{
					uiObjects[43].SetActive(value: true);
				}
			}
			else if (!gS_.schublade)
			{
				string text = "";
				text = text + gS_.GetNameWithTag() + "\n<color=#2980B9>" + mS_.GetMoney(Mathf.RoundToInt(sellsPerWeek), showDollar: false) + "</color><color=black>|</color>";
				if (gS_.publisherID == mS_.myID)
				{
					if (gS_.gameTyp != 2 && gS_.retailVersion && !gS_.arcade)
					{
						text = text + "<color=magenta>" + gS_.GetLagerbestandString() + "</color><color=black>|</color>";
					}
					if (gS_.arcade)
					{
						text = text + "<color=#2C3E50>" + gS_.GetVorbestellungen() + "</color><color=black>|</color>";
					}
				}
				text = ((gS_.GetGesamtGewinn() < 0) ? (text + "<color=red>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>") : (text + "<color=green>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>"));
				text = text + "|<color=#741616>" + Mathf.RoundToInt(gS_.GetHype()) + "</color>";
				uiObjects[0].GetComponent<Text>().text = text;
			}
			else
			{
				string text2 = gS_.GetNameWithTag() + "\n<color=#2980B9>" + tS_.GetText(1704) + "</color>";
				uiObjects[0].GetComponent<Text>().text = text2;
			}
			uiBalken[0].GetComponent<Image>().fillAmount = Mathf.Lerp(uiBalken[0].GetComponent<Image>().fillAmount, fillBalken, t);
			uiBalkenOnline[0].GetComponent<Image>().fillAmount = Mathf.Lerp(uiBalkenOnline[0].GetComponent<Image>().fillAmount, fillBalkenOnline, t);
			if (!gS_.schublade)
			{
				sellsPerWeek = Mathf.Lerp(sellsPerWeek, gS_.sellsPerWeek[0], t);
				uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(sellsPerWeek), showDollar: false);
				if ((float)gS_.sellsPerWeek[0] - sellsPerWeek < (float)Mathf.Abs(10))
				{
					uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(gS_.sellsPerWeek[0], showDollar: false);
				}
				sellsTotal = Mathf.Lerp(sellsTotal, gS_.sellsTotal, t);
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(sellsTotal), showDollar: false);
				if ((float)gS_.sellsTotal - sellsTotal < (float)Mathf.Abs(10))
				{
					uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(gS_.sellsTotal, showDollar: false);
				}
				if (uiObjects[39].activeSelf)
				{
					uiObjects[39].SetActive(value: false);
				}
			}
			else
			{
				if (!uiObjects[39].activeSelf)
				{
					uiObjects[39].SetActive(value: true);
				}
				myImage.color = guiMain_.colors[17];
			}
			uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(gS_.GetHype()).ToString();
			if (!gS_.typ_bundle)
			{
				uiObjects[10].GetComponent<Text>().text = mS_.Round(gS_.GetIpBekanntheit(), 1).ToString();
			}
			else
			{
				uiObjects[10].GetComponent<Text>().text = "-";
			}
			uiObjects[23].GetComponent<Text>().text = mS_.GetMoney(gS_.userPositiv, showDollar: false);
			uiObjects[24].GetComponent<Text>().text = mS_.GetMoney(gS_.userNegativ, showDollar: false);
			if (gS_.gameTyp == 1 && !gS_.schublade)
			{
				if (gS_.abonnementsWoche >= 0)
				{
					uiObjects[22].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.abonnements), showDollar: false) + " (+" + mS_.GetMoney(Mathf.RoundToInt(gS_.abonnementsWoche), showDollar: false) + ")";
				}
				else
				{
					uiObjects[22].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.abonnements), showDollar: false) + " <color=red>(" + mS_.GetMoney(Mathf.RoundToInt(gS_.abonnementsWoche), showDollar: false) + ")</color>";
				}
				uiObjects[31].GetComponent<Image>().fillAmount = gS_.mmoInteresse * 0.01f;
				uiObjects[32].GetComponent<Image>().fillAmount = gS_.GetMMOAbnutzung() * 0.01f;
			}
			if (gS_.gameTyp == 2 && !gS_.schublade)
			{
				if (gS_.abonnementsWoche >= 0)
				{
					uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.abonnements), showDollar: false) + " (+" + mS_.GetMoney(Mathf.RoundToInt(gS_.abonnementsWoche), showDollar: false) + ")";
				}
				else
				{
					uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.abonnements), showDollar: false) + " <color=red>(" + mS_.GetMoney(Mathf.RoundToInt(gS_.abonnementsWoche), showDollar: false) + ")</color>";
				}
				uiObjects[29].GetComponent<Image>().fillAmount = gS_.f2pInteresse * 0.01f;
				uiObjects[30].GetComponent<Image>().fillAmount = gS_.GetF2PAbnutzung() * 0.01f;
			}
			int num = (wochenChartsPlatz = games_.GetChartsWeekPosition(gS_.myID, wochenChartsPlatz));
			if (num != -1)
			{
				uiObjects[19].GetComponent<Text>().text = num.ToString();
			}
			else
			{
				uiObjects[19].GetComponent<Text>().text = "-";
			}
			if (gS_.gameTab_fullView && !gS_.schublade)
			{
				if (gS_.HasInAppPurchases())
				{
					float num2 = gS_.inAppPurchaseWeek;
					num2 *= gS_.GetInAppPurchaseMoneyPerWeek();
					uiObjects[26].GetComponent<Text>().text = mS_.GetMoney((long)num2, showDollar: true);
					if (!uiObjects[25].activeSelf)
					{
						uiObjects[25].SetActive(value: true);
					}
				}
				else if (uiObjects[25].activeSelf)
				{
					uiObjects[25].SetActive(value: false);
				}
			}
			if (gS_.arcade && !gS_.schublade)
			{
				uiObjects[34].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.vorbestellungen), showDollar: false);
				uiObjects[38].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(gS_.stornierungen), showDollar: false);
				if (!uiObjects[33].activeSelf)
				{
					uiObjects[33].SetActive(value: true);
				}
			}
			else if (uiObjects[33].activeSelf)
			{
				uiObjects[33].SetActive(value: false);
			}
			if (gS_.gameTab_fullView && gS_.publisherID != mS_.myID && !gS_.schublade)
			{
				if (uiObjects[11].activeSelf)
				{
					uiObjects[11].SetActive(value: false);
				}
				if (!uiObjects[12].activeSelf)
				{
					uiObjects[12].SetActive(value: true);
				}
				if (uiObjects[13].activeSelf)
				{
					uiObjects[13].SetActive(value: false);
				}
				if (gS_.gameTyp == 1)
				{
					if (!uiObjects[21].activeSelf)
					{
						uiObjects[21].SetActive(value: true);
					}
				}
				else if (uiObjects[21].activeSelf)
				{
					uiObjects[21].SetActive(value: false);
				}
				if (gS_.gameTyp == 2)
				{
					if (!uiObjects[27].activeSelf)
					{
						uiObjects[27].SetActive(value: true);
					}
				}
				else if (uiObjects[27].activeSelf)
				{
					uiObjects[27].SetActive(value: false);
				}
			}
			if (gS_.gameTab_fullView && gS_.publisherID == mS_.myID && !gS_.schublade)
			{
				if (gS_.gameTyp != 2 && !gS_.handy && !gS_.arcade)
				{
					if (!uiObjects[11].activeSelf)
					{
						uiObjects[11].SetActive(value: true);
					}
					if (gS_.retailVersion)
					{
						uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(gS_.GetVorbestellungen(), showDollar: false);
						uiObjects[16].GetComponent<Text>().text = gS_.GetLagerbestandString();
						if (uiObjects[17].activeSelf)
						{
							uiObjects[17].SetActive(value: false);
						}
						if (!uiObjects[18].activeSelf)
						{
							uiObjects[18].SetActive(value: true);
						}
					}
					else
					{
						if (uiObjects[18].activeSelf)
						{
							uiObjects[18].SetActive(value: false);
						}
						if (!uiObjects[17].activeSelf)
						{
							uiObjects[17].SetActive(value: true);
						}
					}
				}
				else if (uiObjects[11].activeSelf)
				{
					uiObjects[11].SetActive(value: false);
				}
				if (gS_.releaseDate > 0)
				{
					if (uiObjects[12].activeSelf)
					{
						uiObjects[12].SetActive(value: false);
					}
					if (!uiObjects[13].activeSelf)
					{
						uiObjects[13].SetActive(value: true);
					}
					if (uiObjects[21].activeSelf)
					{
						uiObjects[21].SetActive(value: false);
					}
					if (uiObjects[27].activeSelf)
					{
						uiObjects[27].SetActive(value: false);
					}
					if (gS_.releaseDate > 1)
					{
						string text3 = tS_.GetText(1123);
						text3 = text3.Replace("<NUM>", gS_.releaseDate.ToString());
						uiObjects[14].GetComponent<Text>().text = text3;
					}
					else
					{
						uiObjects[14].GetComponent<Text>().text = tS_.GetText(1864);
					}
				}
				else
				{
					if (!uiObjects[12].activeSelf)
					{
						uiObjects[12].SetActive(value: true);
					}
					if (uiObjects[13].activeSelf)
					{
						uiObjects[13].SetActive(value: false);
					}
					if (gS_.gameTyp == 1)
					{
						if (!uiObjects[21].activeSelf)
						{
							uiObjects[21].SetActive(value: true);
						}
					}
					else if (uiObjects[21].activeSelf)
					{
						uiObjects[21].SetActive(value: false);
					}
					if (gS_.gameTyp == 2)
					{
						if (!uiObjects[27].activeSelf)
						{
							uiObjects[27].SetActive(value: true);
						}
					}
					else if (uiObjects[27].activeSelf)
					{
						uiObjects[27].SetActive(value: false);
					}
				}
			}
			if (gS_.publisherID == mS_.myID && !gS_.schublade)
			{
				if (gS_.retailVersion)
				{
					if (gS_.lagerbestand[0] <= 0 && !gS_.arcade)
					{
						myImage.color = Color.Lerp(guiMain_.colors[14], guiMain_.colors[5], Mathf.PingPong(Time.time, 1f));
					}
					else
					{
						myImage.color = guiMain_.colors[14];
					}
				}
				if (!gS_.retailVersion || gS_.arcade)
				{
					myImage.color = guiMain_.colors[14];
				}
			}
			if (gS_.schublade)
			{
				if (uiObjects[11].activeSelf)
				{
					uiObjects[11].SetActive(value: false);
				}
				if (uiObjects[12].activeSelf)
				{
					uiObjects[12].SetActive(value: false);
				}
				if (uiObjects[13].activeSelf)
				{
					uiObjects[13].SetActive(value: false);
				}
				if (uiObjects[21].activeSelf)
				{
					uiObjects[21].SetActive(value: false);
				}
				if (uiObjects[27].activeSelf)
				{
					uiObjects[27].SetActive(value: false);
				}
				if (uiObjects[33].activeSelf)
				{
					uiObjects[33].SetActive(value: false);
				}
				if (uiObjects[25].activeSelf)
				{
					uiObjects[25].SetActive(value: false);
				}
			}
			if (!gS_.gameTab_fullView)
			{
				return;
			}
			float num3 = 0f;
			float num4 = 5f;
			num3 += 22f;
			num3 += 55f;
			num3 += 25f + num4;
			if (!gS_.schublade)
			{
				if (uiObjects[33].activeSelf)
				{
					num3 += 32f + num4;
				}
				if (uiObjects[11].activeSelf)
				{
					num3 += 32f + num4;
				}
				if (uiObjects[12].activeSelf)
				{
					num3 += 73f + num4;
				}
				if (uiObjects[13].activeSelf)
				{
					num3 += 32f + num4;
				}
				if (uiObjects[21].activeSelf)
				{
					num3 += 32f + num4;
				}
				if (uiObjects[25].activeSelf)
				{
					num3 += 17f + num4;
				}
				if (uiObjects[27].activeSelf)
				{
					num3 += 32f + num4;
				}
			}
			num3 += 20f + num4;
			if (Mathf.RoundToInt(rect.sizeDelta.y) != Mathf.RoundToInt(num3))
			{
				rect.sizeDelta = new Vector2(220f, Mathf.RoundToInt(num3));
			}
		}
		else
		{
			fullViewOld = gS_.gameTab_fullView;
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		FindScripts();
		FindMyGame();
		if ((bool)guiMain_ && (bool)gS_)
		{
			tooltip_.c = gS_.GetTooltip();
		}
	}

	public void UpdateData()
	{
		FindScripts();
		if (!gS_)
		{
			FindMyGame();
			if (!gS_)
			{
				return;
			}
		}
		uiObjects[9].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[35].GetComponent<Image>().sprite = genres_.GetPic(gS_.maingenre);
		uiObjects[36].GetComponent<Image>().sprite = gS_.GetSizeSprite();
		if (!gS_.pubOffer)
		{
			if (uiObjects[40].activeSelf)
			{
				uiObjects[40].SetActive(value: false);
			}
		}
		else if (!uiObjects[40].activeSelf)
		{
			uiObjects[40].SetActive(value: true);
		}
		if (gS_.gameLicence != -1)
		{
			uiObjects[37].GetComponent<Image>().sprite = licences_.licenceSprites[licences_.licence_TYP[gS_.gameLicence]];
		}
		else
		{
			uiObjects[37].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		}
		long gesamtGewinn = gS_.GetGesamtGewinn();
		if (gesamtGewinn < 0)
		{
			uiObjects[3].GetComponent<Text>().color = guiMain_.colors[5];
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(gesamtGewinn, showDollar: true);
		}
		else
		{
			uiObjects[3].GetComponent<Text>().color = guiMain_.colors[6];
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(gesamtGewinn, showDollar: true);
		}
		uiObjects[4].GetComponent<Text>().text = gS_.weeksOnMarket.ToString();
		uiObjects[6].GetComponent<Image>().sprite = gS_.GetTypSprite();
		float num = 0f;
		for (int i = 0; i < uiBalken.Length; i++)
		{
			if ((float)gS_.sellsPerWeek[i] > num)
			{
				num = gS_.sellsPerWeek[i];
			}
		}
		for (int j = 1; j < uiBalken.Length; j++)
		{
			float num2 = gS_.sellsPerWeek[j];
			if (num2 > 0f)
			{
				uiBalken[j].GetComponent<Image>().fillAmount = num2 / num;
				uiBalkenOnline[j].GetComponent<Image>().fillAmount = uiBalken[j].GetComponent<Image>().fillAmount * (gS_.sellsPerWeekOnline[j] * 0.01f);
			}
			else
			{
				uiBalken[j].GetComponent<Image>().fillAmount = 0f;
				uiBalkenOnline[j].GetComponent<Image>().fillAmount = 0f;
			}
		}
		sellsPerWeek = 0f;
		uiBalken[0].GetComponent<Image>().fillAmount = 0f;
		uiBalkenOnline[0].GetComponent<Image>().fillAmount = 0f;
		float num3 = gS_.sellsPerWeek[0];
		if (num3 > 0f)
		{
			fillBalken = num3 / num;
			fillBalkenOnline = fillBalken * (gS_.sellsPerWeekOnline[0] * 0.01f);
		}
		else
		{
			fillBalken = 0f;
			fillBalkenOnline = 0f;
		}
	}

	public void BUTTON_Minimize()
	{
		sfx_.PlaySound(3, force: true);
		if (!gS_)
		{
			return;
		}
		gS_.gameTab_fullView = !gS_.gameTab_fullView;
		uiObjects[7].SetActive(gS_.gameTab_fullView);
		if (gS_.gameTab_fullView)
		{
			if (gS_.publisherID != mS_.myID)
			{
				uiObjects[8].GetComponent<Image>().sprite = uiSprites[0];
			}
			if (gS_.publisherID == mS_.myID)
			{
				uiObjects[8].GetComponent<Image>().sprite = uiSprites[0];
			}
			UpdateData();
		}
		else
		{
			uiObjects[8].GetComponent<Image>().sprite = uiSprites[1];
		}
	}

	public void BUTTON_Click()
	{
		if (!gS_)
		{
			return;
		}
		if (gS_.schublade)
		{
			GameObject gameObject = GameObject.Find("Task_" + gS_.schubladeTaskID);
			if ((bool)gameObject)
			{
				sfx_.PlaySound(3, force: true);
				guiMain_.ActivateMenu(guiMain_.uiObjects[69]);
				guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().Init(gS_, gameObject.GetComponent<taskGame>());
				guiMain_.OpenMenu(hideChars: false);
			}
		}
		else
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[91]);
			guiMain_.uiObjects[91].GetComponent<Menu_Game_Umsatz>().Init(gS_);
			guiMain_.OpenMenu(hideChars: false);
		}
	}
}
