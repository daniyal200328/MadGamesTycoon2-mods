using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class gamePassTab : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
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

	public gamepassScript gpS_;

	public RectTransform myRect_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiBalken;

	public Sprite[] uiSprites;

	private float fillBalken;

	public bool fullView = true;

	private RectTransform rect;

	private Image myImage;

	private Button myButton;

	public bool updateAmountOfGames;

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
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
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
	}

	public void Init()
	{
		if (!main_)
		{
			FindScripts();
		}
		updateMyData = true;
		UpdateData();
		BUTTON_Minimize();
		BUTTON_Minimize();
		base.gameObject.transform.SetAsFirstSibling();
		guiMain_.UpdateGameTabsSortierung();
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
		if (base.gameObject.transform.GetSiblingIndex() != 0)
		{
			base.gameObject.transform.SetAsFirstSibling();
		}
		if ((myRect_.position.y >= 0f && myRect_.position.y <= (float)Screen.height) || fullViewOld != fullView)
		{
			updateMyData = false;
			fullViewOld = fullView;
			updateMyData = !updateMyData;
			if (!updateMyData)
			{
				return;
			}
			float t = 12.8f * Time.deltaTime;
			if (!fullView)
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
			if (fullView || settings_.gameTabData)
			{
				uiObjects[0].GetComponent<Text>().text = gpS_.gamePass_name;
				uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(gpS_.gamePass_UmsatzJahr, showDollar: true);
				UpdateMarketshare();
				uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(gpS_.gamePass_Abos, showDollar: false);
				if (gpS_.gamePass_AbosLetzteWoche >= 0)
				{
					uiObjects[9].GetComponent<Text>().text = "+" + mS_.GetMoney(gpS_.gamePass_AbosLetzteWoche, showDollar: false);
				}
				else
				{
					uiObjects[9].GetComponent<Text>().text = "<color=red>" + mS_.GetMoney(gpS_.gamePass_AbosLetzteWoche, showDollar: false) + "</color>";
				}
				uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(gpS_.gamePass_AmountGamesAktiv, showDollar: false) + " / " + mS_.GetMoney(gpS_.gamePass_AmountGames, showDollar: false);
				guiMain_.DrawStarsColor(uiObjects[11], Mathf.RoundToInt(gpS_.GAMEPASS_GetAktualitaet() * 5f), Color.white);
			}
			else
			{
				string text = gpS_.gamePass_name + "\n";
				text = text + "<color=#2980B9>" + mS_.GetMoney(gpS_.gamePass_Abos, showDollar: false) + "</color><color=black>|</color>";
				text = ((gpS_.gamePass_AbosLetzteWoche < 0) ? (text + "<color=red>" + mS_.GetMoney(gpS_.gamePass_AbosLetzteWoche, showDollar: false) + "</color><color=black>|</color>") : (text + "<color=green>" + mS_.GetMoney(gpS_.gamePass_AbosLetzteWoche, showDollar: false) + "</color><color=black>|</color>"));
				text = text + "<color=magenta>" + mS_.Round((float)gpS_.gamePass_userGesamt / 1000000f, 1) + " " + tS_.GetText(1483) + "</color><color=black>|</color>";
				text += mS_.GetMoney(gpS_.gamePass_UmsatzJahr, showDollar: true);
				uiObjects[0].GetComponent<Text>().text = text;
			}
			uiBalken[0].GetComponent<Image>().fillAmount = Mathf.Lerp(uiBalken[0].GetComponent<Image>().fillAmount, fillBalken, t);
			if (fullView)
			{
				float f = 257f;
				if (Mathf.RoundToInt(rect.sizeDelta.y) != Mathf.RoundToInt(f))
				{
					rect.sizeDelta = new Vector2(220f, Mathf.RoundToInt(f));
				}
			}
		}
		else
		{
			fullViewOld = fullView;
		}
	}

	public void UpdateMarketshare()
	{
		uiObjects[3].GetComponent<Text>().text = mS_.Round(gpS_.gamePass_marktPC, 1) + "%";
		uiObjects[4].GetComponent<Text>().text = mS_.Round(gpS_.gamePass_marktKonsole, 1) + "%";
		uiObjects[5].GetComponent<Text>().text = mS_.Round(gpS_.gamePass_marktHandheld, 1) + "%";
		uiObjects[6].GetComponent<Text>().text = mS_.Round((float)gpS_.gamePass_userGesamt / 1000000f, 1) + " " + tS_.GetText(1483);
	}

	public void UpdateData()
	{
		FindScripts();
		updateAmountOfGames = true;
		float num = 0f;
		for (int i = 0; i < uiBalken.Length; i++)
		{
			if ((float)gpS_.gamePass_aboVerlaufWoche[i] > num)
			{
				num = gpS_.gamePass_aboVerlaufWoche[i];
			}
		}
		for (int j = 1; j < uiBalken.Length; j++)
		{
			float num2 = gpS_.gamePass_aboVerlaufWoche[j];
			if (num2 > 0f)
			{
				uiBalken[j].GetComponent<Image>().fillAmount = num2 / num;
			}
			else
			{
				uiBalken[j].GetComponent<Image>().fillAmount = 0f;
			}
		}
		uiBalken[0].GetComponent<Image>().fillAmount = 0f;
		float num3 = gpS_.gamePass_aboVerlaufWoche[0];
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
		fullView = !fullView;
		uiObjects[1].SetActive(fullView);
		if (fullView)
		{
			uiObjects[2].GetComponent<Image>().sprite = uiSprites[0];
			UpdateData();
		}
		else
		{
			uiObjects[2].GetComponent<Image>().sprite = uiSprites[1];
		}
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[422]);
		guiMain_.OpenMenu(hideChars: false);
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		FindScripts();
		if ((bool)guiMain_)
		{
			tooltip_.c = gpS_.GetTooltip();
		}
	}
}
