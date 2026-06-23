using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class konsoleTab : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
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

	public RectTransform myRect_;

	public int platformID = -1;

	public platformScript pS_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiBalken;

	public Sprite[] uiSprites;

	private float fillBalken;

	private float sellsPerWeek;

	private float sellsTotal;

	private RectTransform rect;

	private Image myImage;

	private Button myButton;

	private bool fullViewOld;

	private bool updateMyData;

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
	}

	public void Init(int platID_)
	{
		if (!main_)
		{
			FindScripts();
		}
		platformID = platID_;
		updateMyData = true;
		UpdateData();
		BUTTON_Minimize();
		BUTTON_Minimize();
		base.gameObject.transform.SetSiblingIndex(0);
		guiMain_.UpdateGameTabsSortierung();
	}

	private void FindMyPlatform()
	{
		if (platformID != -1 && !pS_)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + platformID);
			if ((bool)gameObject)
			{
				pS_ = gameObject.GetComponent<platformScript>();
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
		if (!pS_)
		{
			FindMyPlatform();
		}
		else if ((myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height) || fullViewOld != pS_.konsoleTab_fullView)
		{
			updateMyData = false;
			fullViewOld = pS_.konsoleTab_fullView;
			updateMyData = !updateMyData;
			if (!updateMyData)
			{
				return;
			}
			float t = 12.8f * Time.deltaTime;
			if (!pS_.konsoleTab_fullView)
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
			if (pS_.konsoleTab_fullView || settings_.gameTabData)
			{
				uiObjects[0].GetComponent<Text>().text = pS_.GetName();
				long gesamtGewinn = pS_.GetGesamtGewinn();
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
			}
			else
			{
				string text = pS_.GetName() + "\n<color=#2980B9>" + mS_.GetMoney(Mathf.RoundToInt(sellsPerWeek), showDollar: false) + "</color><color=black>|</color>";
				long gesamtGewinn2 = pS_.GetGesamtGewinn();
				text = ((gesamtGewinn2 < 0) ? (text + "<color=red>" + mS_.GetMoney(gesamtGewinn2, showDollar: true) + "</color><color=black>|</color>") : (text + "<color=green>" + mS_.GetMoney(gesamtGewinn2, showDollar: true) + "</color><color=black>|</color>"));
				text = text + "<color=magenta>" + mS_.Round(pS_.marktanteil, 1) + "%</color><color=black>|</color>";
				text = text + "<color=#741616>" + Mathf.RoundToInt(pS_.GetHype()) + "</color>";
				uiObjects[0].GetComponent<Text>().text = text;
			}
			uiBalken[0].GetComponent<Image>().fillAmount = Mathf.Lerp(uiBalken[0].GetComponent<Image>().fillAmount, fillBalken, t);
			sellsPerWeek = Mathf.Lerp(sellsPerWeek, pS_.sellsPerWeek[0], t);
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(sellsPerWeek), showDollar: false);
			if ((float)pS_.sellsPerWeek[0] - sellsPerWeek < (float)Mathf.Abs(10))
			{
				uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(pS_.sellsPerWeek[0], showDollar: false);
			}
			sellsTotal = Mathf.Lerp(sellsTotal, pS_.units, t);
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(sellsTotal), showDollar: false);
			if ((float)pS_.units - sellsTotal < (float)Mathf.Abs(10))
			{
				uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(pS_.units, showDollar: false);
			}
			uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(pS_.GetHype()).ToString();
			uiObjects[10].GetComponent<Text>().text = pS_.tech.ToString();
			uiObjects[15].GetComponent<Text>().text = pS_.GetGames() + " [" + pS_.GetGamesInDev() + "]";
			float sellsBonusForGames = pS_.GetSellsBonusForGames();
			if (sellsBonusForGames < 0f)
			{
				uiObjects[25].GetComponent<Text>().text = "<color=#FF7373>" + mS_.RoundString(sellsBonusForGames, 1) + "%</color>";
				uiObjects[18].GetComponent<Image>().fillAmount = 0f;
				uiObjects[18].GetComponent<Image>().color = guiMain_.colors[19];
			}
			else
			{
				uiObjects[25].GetComponent<Text>().text = "<color=white>+" + mS_.RoundString(sellsBonusForGames, 1) + "%</color>";
				uiObjects[18].GetComponent<Image>().fillAmount = sellsBonusForGames * 0.01f / 3f;
				uiObjects[18].GetComponent<Image>().color = guiMain_.colors[20];
			}
			if (sellsBonusForGames >= 300f)
			{
				uiObjects[18].GetComponent<Image>().color = guiMain_.colors[17];
			}
			uiObjects[12].GetComponent<Text>().text = mS_.Round(pS_.marktanteil, 1) + "%";
			uiObjects[20].GetComponent<Image>().fillAmount = pS_.kostenreduktion * 0.01f;
			uiObjects[23].GetComponent<Text>().text = mS_.RoundString(pS_.kostenreduktion, 1) + "%";
			uiObjects[21].GetComponent<Text>().text = mS_.GetMoney(pS_.GetAktuellProductionsCosts(), showDollar: true);
			uiObjects[22].GetComponent<Text>().text = mS_.GetMoney(pS_.GetVerkaufspreis(), showDollar: true);
			uiObjects[26].GetComponent<Image>().fillAmount = pS_.GetHaltbarkeit() * 0.1f;
			uiObjects[24].GetComponent<Text>().text = mS_.RoundString(pS_.GetHaltbarkeit(), 1);
			uiObjects[27].GetComponent<Text>().text = mS_.GetMoney(pS_.garantiefaelle, showDollar: false) + " <color=#950B01>[" + mS_.GetMoney(pS_.garantiekosten, showDollar: true) + "]</color>";
			if (pS_.IsOutdatet())
			{
				if (!uiObjects[13].activeSelf)
				{
					uiObjects[13].SetActive(value: true);
				}
			}
			else if (uiObjects[13].activeSelf)
			{
				uiObjects[13].SetActive(value: false);
			}
			if (pS_.konsoleTab_fullView)
			{
				if (pS_.thridPartyGames && pS_.subventionMoney > 0)
				{
					uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(pS_.subventionMoney, showDollar: true);
					if (!uiObjects[28].activeSelf)
					{
						uiObjects[28].SetActive(value: true);
					}
				}
				else if (uiObjects[28].activeSelf)
				{
					uiObjects[28].SetActive(value: false);
				}
			}
			if (pS_.konsoleTab_fullView)
			{
				float num = 5f;
				float num2 = 316f;
				if (uiObjects[28].activeSelf)
				{
					num2 += 17f + num;
				}
				if (Mathf.RoundToInt(rect.sizeDelta.y) != Mathf.RoundToInt(num2))
				{
					rect.sizeDelta = new Vector2(220f, Mathf.RoundToInt(num2));
				}
			}
		}
		else
		{
			fullViewOld = pS_.konsoleTab_fullView;
		}
	}

	public void UpdateData()
	{
		FindScripts();
		if (!pS_)
		{
			FindMyPlatform();
			if (!pS_)
			{
				return;
			}
		}
		pS_.SetPic(uiObjects[9]);
		uiObjects[6].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[4].GetComponent<Text>().text = pS_.weeksOnMarket.ToString();
		if (pS_.proVersion && pS_.GetProName().Length > 0)
		{
			if (!uiObjects[14].activeSelf)
			{
				uiObjects[14].SetActive(value: true);
			}
		}
		else if (uiObjects[14].activeSelf)
		{
			uiObjects[14].SetActive(value: false);
		}
		float num = 0f;
		for (int i = 0; i < uiBalken.Length; i++)
		{
			if ((float)pS_.sellsPerWeek[i] > num)
			{
				num = pS_.sellsPerWeek[i];
			}
		}
		for (int j = 1; j < uiBalken.Length; j++)
		{
			float num2 = pS_.sellsPerWeek[j];
			if (num2 > 0f)
			{
				uiBalken[j].GetComponent<Image>().fillAmount = num2 / num;
			}
			else
			{
				uiBalken[j].GetComponent<Image>().fillAmount = 0f;
			}
		}
		sellsPerWeek = 0f;
		uiBalken[0].GetComponent<Image>().fillAmount = 0f;
		float num3 = pS_.sellsPerWeek[0];
		if (num3 > 0f)
		{
			fillBalken = num3 / num;
		}
		else
		{
			fillBalken = 0f;
		}
	}

	public void BUTTON_Minimize()
	{
		sfx_.PlaySound(3, force: true);
		pS_.konsoleTab_fullView = !pS_.konsoleTab_fullView;
		uiObjects[7].SetActive(pS_.konsoleTab_fullView);
		if (pS_.konsoleTab_fullView)
		{
			uiObjects[8].GetComponent<Image>().sprite = uiSprites[0];
			UpdateData();
		}
		else
		{
			uiObjects[8].GetComponent<Image>().sprite = uiSprites[1];
		}
	}

	public void BUTTON_Click()
	{
		if ((bool)pS_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[333]);
			guiMain_.uiObjects[333].GetComponent<Menu_Umsatz_Konsole>().Init(pS_);
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		FindScripts();
		FindMyPlatform();
		if ((bool)guiMain_ && (bool)pS_)
		{
			tooltip_.c = pS_.GetTooltip();
			uiObjects[19].GetComponent<tooltip>().c = pS_.GetBonusTooltip();
		}
	}
}
