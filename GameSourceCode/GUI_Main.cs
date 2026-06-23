using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Vectrosity;

public class GUI_Main : MonoBehaviour
{
	public GameObject[] menuToParent;

	public GameObject menuParent;

	public GameObject[] uiPrefabs;

	public GameObject uiPops;

	public GameObject uiRooms;

	public GameObject uiMoney;

	public GameObject[] uiObjects;

	public GameObject[] uiPerks;

	public GameObject[] uiTaskPrefabs;

	public GameObject[] sabotageIcons;

	public Sprite[] uiSprites;

	public Sprite[] inventarSprites;

	public Sprite[] flagSprites;

	public Sprite[] logoSprites;

	public Sprite[] charIcons;

	public Sprite[] iconSupport;

	public Sprite[] iconGlobalEvents;

	public Sprite[] iconAchivements;

	public Sprite[] iconAchivementsOff;

	public Sprite[] iconMaps;

	public Color[] colors;

	public Color[] colorsBalken;

	public Texture2D[] unterstuetzenLine;

	public Material matFill_Window;

	public bool menuOpen;

	public bool disableRoomGUI;

	public bool spacePause;

	public Camera camera_;

	private float durchschnittsMotivation = 100f;

	private float timerUpdateTopLeiste;

	private float timerUpdateContract;

	private float timerUpdateGlobalEvent;

	public GameObject hellgikeitsObjekt;

	private GameObject main_;

	private settingsScript settings_;

	private mainScript mS_;

	private textScript tS_;

	private sfxScript sfx_;

	public GUI_Tooltip guiTooltip;

	private objectTooltip objectTooltip;

	private genres genres_;

	private themes themes_;

	private mapScript mapScript_;

	private unlockScript unlock_;

	private contractWorkMain contractMain_;

	private publishingOfferMain publishingOfferMain_;

	private pickCharacterScript pcS_;

	private mpCalls mpCalls_;

	private forschungSonstiges forschungSonstiges_;

	private gameplayFeatures gF_;

	private games games_;

	private gamepassScript gpS_;

	private PostProcessing postProcess_;

	public GUI_MainButtons guiMainButtons_;

	public GameObject gamePassTab_;

	public gamePassTab gamePassTabScript_;

	private List<GameObject> moneyPopList = new List<GameObject>();

	private float fansTimer;

	private int fansOld;

	private string fansString = "";

	public float supportTimer;

	private float moneyTimer;

	private long moneyOld;

	private string moneyString = "";

	private tooltip moneyTooltip;

	private float updateBankWarningIcon_Timer;

	private float updateExklusivVertragIcon_Timer;

	private float updateStudioBewertungTimer;

	private float timerUpdateMitarbeiterNoJob;

	private float timerUpdateRoomNoJob;

	private bool showAllNews = true;

	private int roomNoJobOld;

	private bool gameTabsFullView = true;

	public bool initVectrocity;

	private GameObject filterGameObject_;

	public int filterToggles = -1;

	public float timerShowBeschwerde = 120f;

	public int UIHotkeySiblingIndex = -1;

	public GameObject UIHotkey;

	public bool selectInputField;

	private List<FansSortList> fansSortList = new List<FansSortList>();

	private void Awake()
	{
		mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		InitVectrocity();
	}

	private void Start()
	{
		FindScripts();
		LoadPlayerAndCompanyName();
	}

	private void Update()
	{
		UpdateUIHotkey();
		if (IsStartMenuActive())
		{
			return;
		}
		UpdateTopNews();
		UpdateTutorial();
		AddGamePassTab();
		UpdateData();
		UpdateStudioBewertung();
		UpdateFilterToggles();
		UpdatePanelMultiplayer();
		if (IsFilterAktiv())
		{
			mS_.DrawFilter(filterToggles, force: false);
		}
		if (!mS_.multiplayer && !menuOpen && Input.GetKeyUp(KeyCode.Space))
		{
			if (mS_.GetGameSpeed() > 0f)
			{
				spacePause = true;
				mS_.PauseGame(p: true);
			}
			else
			{
				spacePause = false;
				mS_.PauseGame(p: false);
			}
		}
	}

	public bool IsFilterAktiv()
	{
		if (!filterGameObject_)
		{
			return false;
		}
		if (filterGameObject_.activeSelf)
		{
			return true;
		}
		return false;
	}

	public void UpdateOnce()
	{
		SetMainGuiData();
		UpdateAuftragsansehen(0f);
		InitToggles();
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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!camera_)
		{
			camera_ = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
		}
		if (!guiMainButtons_)
		{
			guiMainButtons_ = GameObject.Find("GUI_MainButtons").GetComponent<GUI_MainButtons>();
		}
		if (!guiTooltip)
		{
			guiTooltip = GetComponent<GUI_Tooltip>();
		}
		if (!objectTooltip)
		{
			objectTooltip = GetComponent<objectTooltip>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!mapScript_)
		{
			mapScript_ = main_.GetComponent<mapScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!contractMain_)
		{
			contractMain_ = main_.GetComponent<contractWorkMain>();
		}
		if (!publishingOfferMain_)
		{
			publishingOfferMain_ = main_.GetComponent<publishingOfferMain>();
		}
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!postProcess_)
		{
			postProcess_ = camera_.GetComponent<PostProcessing>();
		}
	}

	public bool IsStartMenuActive()
	{
		return uiObjects[151].activeSelf;
	}

	public void ActivateMenu(GameObject go)
	{
		if (!go.activeSelf)
		{
			go.SetActive(value: true);
		}
	}

	public void DeactivateMenu(GameObject go)
	{
		if (go.activeSelf)
		{
			go.SetActive(value: false);
		}
	}

	public Vector2 GetAnchoredPosition(Vector2 v)
	{
		return v / settings_.uiScale;
	}

	public bool IsMouseOverGUI()
	{
		return EventSystem.current.IsPointerOverGameObject();
	}

	public IEnumerator MoneyPopEnumerate(int i, Vector3 pos, bool green)
	{
		if (!settings_.moneyPop)
		{
			yield return new WaitForSeconds(Random.Range(0f, 1f));
			MoneyPop(i, pos, green);
		}
	}

	public void MoneyPop(int i, Vector3 pos, bool green)
	{
		if (settings_.moneyPop)
		{
			return;
		}
		Vector2 vector = camera_.WorldToScreenPoint(pos);
		if (!(vector.x >= 0f) || !(vector.x <= (float)Screen.width) || !(vector.y >= 0f) || !(vector.y <= (float)Screen.height))
		{
			return;
		}
		GameObject gameObject = null;
		for (int j = 0; j < moneyPopList.Count; j++)
		{
			if ((bool)moneyPopList[j] && moneyPopList[j].transform.position.x >= 99998f)
			{
				gameObject = moneyPopList[j];
				break;
			}
		}
		if (!gameObject)
		{
			gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(99999f, 99999f, 0f), Quaternion.identity);
			moneyPopList.Add(gameObject);
			gameObject.transform.SetParent(uiMoney.transform);
		}
		moneyPop component = gameObject.GetComponent<moneyPop>();
		component.myTimer = 0f;
		component.camera_ = camera_;
		component.myPosition = pos;
		component.settings_ = settings_;
		component.Init(new Vector2(vector.x, vector.y - (float)Screen.height));
		if ((bool)component.text_)
		{
			component.text_.text = mS_.GetMoney(i, showDollar: true);
			if (green)
			{
				component.text_.color = colors[24];
			}
			else
			{
				component.text_.color = colors[19];
			}
		}
	}

	public void ShowTooltip(string c)
	{
		if (!guiTooltip.tooltipEnabled || c != guiTooltip.myText.text)
		{
			guiTooltip.SetActive(c);
		}
	}

	public void DisableTooltip()
	{
		if (guiTooltip.tooltipEnabled)
		{
			guiTooltip.SetInactive();
		}
	}

	public void ShowObjectTooltip(objectScript script_)
	{
		if (!script_)
		{
			DisableObjectTooltip();
		}
		else if (!objectTooltip.tooltipEnabled)
		{
			objectTooltip.SetActive(script_);
		}
	}

	public void DisableObjectTooltip()
	{
		if (objectTooltip.tooltipEnabled)
		{
			objectTooltip.SetInactive();
		}
	}

	public void OpenMenu(bool hideChars)
	{
		FindScripts();
		menuOpen = true;
		disableRoomGUI = true;
		if (!mS_.multiplayer && !settings_.singleplayerPause)
		{
			mS_.PauseGame(p: true);
		}
		if (hideChars)
		{
			StartCoroutine(HideChars());
		}
		uiObjects[1].GetComponent<GUI_MainButtons>().CloseAllDropdowns();
		MainButtonsInteractable(b: false);
		CloseAllRoomButtons();
		if (mS_.multiplayer && mS_.settings_autoPauseForMultiplayer)
		{
			if (mpCalls_.isClient)
			{
				mpCalls_.SetPause(b: true);
				mpCalls_.CLIENT_Send_Command(2);
			}
			else if (mpCalls_.isServer)
			{
				mpCalls_.SetPause(b: true);
				mpCalls_.SERVER_Send_AutoPause();
			}
		}
	}

	public void CloseMenu()
	{
		menuOpen = false;
		disableRoomGUI = false;
		mS_.PauseGame(p: false);
		if (base.gameObject.activeSelf)
		{
			StartCoroutine(UnhideChars());
		}
		MainButtonsInteractable(b: true);
		if (mS_.multiplayer && mS_.settings_autoPauseForMultiplayer && mS_.myID != -1)
		{
			if (mpCalls_.isClient)
			{
				mpCalls_.SetPause(b: false);
				mpCalls_.CLIENT_Send_Command(3);
			}
			else
			{
				mpCalls_.SetPause(b: false);
				mpCalls_.SERVER_Send_AutoPause();
			}
		}
	}

	private void MainButtonsInteractable(bool b)
	{
		for (int i = 0; i < guiMainButtons_.transform.childCount; i++)
		{
			Transform child = guiMainButtons_.transform.GetChild(i);
			if ((bool)child)
			{
				child.GetComponent<Button>().interactable = b;
				if (b)
				{
					child.GetChild(1).GetComponent<Image>().color = Color.white;
				}
				else
				{
					child.GetChild(1).GetComponent<Image>().color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
				}
			}
		}
		uiObjects[106].GetComponent<Button>().interactable = b;
		uiObjects[107].GetComponent<Button>().interactable = b;
		uiObjects[160].GetComponent<Button>().interactable = b;
		uiObjects[346].GetComponent<Button>().interactable = b;
		uiObjects[347].GetComponent<Button>().interactable = b;
		uiObjects[376].GetComponent<Button>().interactable = b;
		uiObjects[381].GetComponent<Button>().interactable = b;
	}

	private IEnumerator UnhideChars()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if ((bool)mS_.arrayCharactersScripts[i])
			{
				mS_.arrayCharactersScripts[i].UnhideChar();
			}
		}
	}

	private IEnumerator HideChars()
	{
		yield return new WaitForEndOfFrame();
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if ((bool)mS_.arrayCharactersScripts[i])
			{
				mS_.arrayCharactersScripts[i].HideChar();
			}
		}
	}

	public void SetMainGuiData()
	{
		uiObjects[6].GetComponent<Text>().text = mS_.GetCompanyName();
		uiObjects[5].GetComponent<Image>().sprite = flagSprites[mS_.GetCountryID()];
		uiObjects[5].GetComponent<tooltip>().c = tS_.GetCountry(mS_.GetCountryID());
		if (logoSprites.Length > mS_.GetCompanyLogoID())
		{
			uiObjects[11].GetComponent<Image>().sprite = GetCompanyLogo(mS_.GetCompanyLogoID());
		}
		UpdateTrend();
		UpdateStudioBewertung();
	}

	public void UpdateFans()
	{
		fansTimer += Time.deltaTime;
		int amountFans = genres_.GetAmountFans();
		if (fansTimer > 1f)
		{
			fansTimer = 0f;
			uiObjects[229].GetComponent<tooltip>().c = GetFansTooltip();
			if (fansOld > amountFans)
			{
				fansString = "<color=red>" + tS_.GetText(100) + "</color>";
			}
			if (fansOld < amountFans)
			{
				fansString = "<color=green>" + tS_.GetText(99) + "</color>";
			}
			fansOld = amountFans;
		}
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(amountFans, showDollar: false);
		uiObjects[8].GetComponent<Text>().text += fansString;
	}

	public void UpdateSupportIcon()
	{
		supportTimer += Time.deltaTime;
		if (supportTimer > 1f)
		{
			supportTimer = 0f;
			uiObjects[140].GetComponent<Image>().sprite = iconSupport[mS_.GetAnrufeAmount()];
		}
	}

	public void UpdateMoney()
	{
		if (mS_.settings_sandbox && mS_.sandbox_unlimitedMoney)
		{
			if ((bool)tS_)
			{
				uiObjects[7].GetComponent<Text>().text = tS_.GetText(2065);
			}
			return;
		}
		moneyTimer += Time.deltaTime;
		if (moneyTimer > 1f)
		{
			moneyTimer = 0f;
			if (moneyOld > mS_.money)
			{
				moneyString = "<color=red>" + tS_.GetText(100) + "</color>";
			}
			if (moneyOld < mS_.money)
			{
				moneyString = "<color=green>" + tS_.GetText(99) + "</color>";
			}
			moneyOld = mS_.money;
		}
		if (mS_.money >= 0)
		{
			uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(mS_.money, showDollar: false);
		}
		else
		{
			uiObjects[7].GetComponent<Text>().text = "<color=red>" + mS_.GetMoney(mS_.money, showDollar: false) + "</color>";
		}
		uiObjects[7].GetComponent<Text>().text += moneyString;
		if (!moneyTooltip)
		{
			moneyTooltip = uiObjects[376].GetComponent<tooltip>();
			return;
		}
		moneyTooltip.c = "<b>" + tS_.GetText(704) + "</b>\n";
		if (mS_.finanzenMonat_GetGewinn() < 0)
		{
			tooltip tooltip2 = moneyTooltip;
			tooltip2.c = tooltip2.c + tS_.GetText(699) + ": <color=red>" + mS_.GetMoney(mS_.finanzenMonat_GetGewinn(), showDollar: true) + "</color>\n";
		}
		else
		{
			tooltip tooltip2 = moneyTooltip;
			tooltip2.c = tooltip2.c + tS_.GetText(699) + ": <color=green>" + mS_.GetMoney(mS_.finanzenMonat_GetGewinn(), showDollar: true) + "</color>\n";
		}
		if (mS_.finanzenMonatLast_GetGewinn() < 0)
		{
			tooltip tooltip2 = moneyTooltip;
			tooltip2.c = tooltip2.c + tS_.GetText(701) + ": <color=red>" + mS_.GetMoney(mS_.finanzenMonatLast_GetGewinn(), showDollar: true) + "</color>\n";
		}
		else
		{
			tooltip tooltip2 = moneyTooltip;
			tooltip2.c = tooltip2.c + tS_.GetText(701) + ": <color=green>" + mS_.GetMoney(mS_.finanzenMonatLast_GetGewinn(), showDollar: true) + "</color>\n";
		}
	}

	public void UpdateCharts()
	{
		int num = 9;
		uiObjects[377].GetComponent<Text>().text = "";
		int num2 = 0;
		switch (uiObjects[380].GetComponent<Dropdown>().value)
		{
		case 0:
			num2 = mS_.games_.chartsWeekList.Count;
			break;
		case 1:
			num2 = mS_.games_.chartsWeekList_Handy.Count;
			break;
		case 2:
			num2 = mS_.games_.chartsWeekList_Arcade.Count;
			break;
		case 3:
			num2 = mS_.games_.chartsWeekList_F2P.Count;
			break;
		}
		for (int i = 0; i < num2; i++)
		{
			gameScript gameScript2 = null;
			switch (uiObjects[380].GetComponent<Dropdown>().value)
			{
			case 0:
				gameScript2 = mS_.games_.chartsWeekList[i].script_;
				break;
			case 1:
				gameScript2 = mS_.games_.chartsWeekList_Handy[i].script_;
				break;
			case 2:
				gameScript2 = mS_.games_.chartsWeekList_Arcade[i].script_;
				break;
			case 3:
				gameScript2 = mS_.games_.chartsWeekList_F2P[i].script_;
				break;
			}
			if ((bool)gameScript2)
			{
				int num3 = i;
				string text = "";
				if (gameScript2.lastChartPosition < num3)
				{
					text = "<color=red>▼</color> ";
				}
				if (gameScript2.lastChartPosition > num3)
				{
					text = "<color=green>▲</color> ";
				}
				if (gameScript2.lastChartPosition == num3)
				{
					text = "<color=black>●</color> ";
				}
				if (gameScript2.lastChartPosition == -1)
				{
					text = "<color=blue>◆</color> ";
				}
				uiObjects[377].GetComponent<Text>().text += text;
				if (gameScript2.IsMyGame())
				{
					Text component = uiObjects[377].GetComponent<Text>();
					component.text = component.text + "<color=blue>" + gameScript2.GetNameWithTag() + "</color>\n";
				}
				else if (gameScript2.GameFromMitspieler())
				{
					Text component2 = uiObjects[377].GetComponent<Text>();
					component2.text = component2.text + "<color=#E74C3C>" + gameScript2.GetNameWithTag() + "</color>\n";
				}
				else if (!gameScript2.GetPublisherOrDeveloperIsTochterfirma())
				{
					Text component3 = uiObjects[377].GetComponent<Text>();
					component3.text = component3.text + gameScript2.GetNameWithTag() + "\n";
				}
				else
				{
					Text component4 = uiObjects[377].GetComponent<Text>();
					component4.text = component4.text + "<color=#9F1F8F>" + gameScript2.GetNameWithTag() + "</color>\n";
				}
				if (i >= num)
				{
					break;
				}
			}
		}
	}

	private void UpdateMainGuiCharts()
	{
		if (uiObjects[379].GetComponent<Toggle>().isOn && !uiObjects[0].activeSelf && !uiObjects[15].activeSelf && !uiObjects[19].activeSelf && !uiObjects[20].activeSelf && !uiObjects[428].activeSelf && !uiObjects[429].activeSelf)
		{
			if (!uiObjects[378].activeSelf)
			{
				uiObjects[378].SetActive(value: true);
				int value = uiObjects[380].GetComponent<Dropdown>().value;
				List<string> list = new List<string>();
				list.Add(tS_.GetText(1780));
				list.Add(tS_.GetText(1796));
				list.Add(tS_.GetText(2052));
				list.Add(tS_.GetText(1779));
				uiObjects[380].GetComponent<Dropdown>().ClearOptions();
				uiObjects[380].GetComponent<Dropdown>().AddOptions(list);
				uiObjects[380].GetComponent<Dropdown>().value = value;
			}
		}
		else if (uiObjects[378].activeSelf)
		{
			uiObjects[378].SetActive(value: false);
		}
	}

	public void UpdateData()
	{
		if (!uiObjects[145].activeSelf)
		{
			return;
		}
		if (mS_.GetGameSpeed() > 0f && timerShowBeschwerde > 0f)
		{
			timerShowBeschwerde -= Time.deltaTime;
		}
		UpdateMoney();
		UpdateMainGuiCharts();
		UpdateFans();
		UpdateSupportIcon();
		uiObjects[9].GetComponent<Text>().text = GetDate();
		uiObjects[10].GetComponent<Image>().fillAmount = mS_.dayTimer;
		uiObjects[10].GetComponent<Image>().color = colors[17];
		if (mS_.multiplayer && mS_.settings_autoPauseForMultiplayer && mpCalls_.AutoPause())
		{
			uiObjects[10].GetComponent<Image>().color = colors[18];
		}
		UpdateGameSpeedButtons();
		if (uiObjects[23].transform.childCount > 3)
		{
			if (!uiObjects[24].activeSelf)
			{
				uiObjects[24].SetActive(value: true);
			}
		}
		else if (uiObjects[24].activeSelf)
		{
			uiObjects[24].SetActive(value: false);
		}
		if (uiObjects[22].transform.childCount > 0)
		{
			if (!uiObjects[413].activeSelf)
			{
				uiObjects[413].SetActive(value: true);
			}
			if (!uiObjects[430].activeSelf)
			{
				uiObjects[430].SetActive(value: true);
			}
			if (!showAllNews)
			{
				uiObjects[430].transform.GetChild(0).GetComponent<Image>().sprite = uiSprites[59];
			}
			else
			{
				uiObjects[430].transform.GetChild(0).GetComponent<Image>().sprite = uiSprites[60];
			}
		}
		else
		{
			if (uiObjects[413].activeSelf)
			{
				uiObjects[413].SetActive(value: false);
			}
			if (uiObjects[430].activeSelf)
			{
				uiObjects[430].SetActive(value: false);
			}
		}
		timerUpdateTopLeiste += Time.deltaTime;
		if (timerUpdateTopLeiste > 1f)
		{
			UpdateTrend();
			timerUpdateTopLeiste = 0f;
			durchschnittsMotivation = 0f;
			for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
			{
				if ((bool)mS_.arrayCharactersScripts[i])
				{
					durchschnittsMotivation += mS_.arrayCharactersScripts[i].s_motivation;
				}
			}
			durchschnittsMotivation /= mS_.arrayCharacters.Length;
			if (durchschnittsMotivation >= mS_.durchschnittsMotivationLastMonth)
			{
				uiObjects[75].GetComponent<Text>().text = "<color=green>" + tS_.GetText(99) + "</color> " + mS_.Round(durchschnittsMotivation, 1) + "%";
			}
			else
			{
				uiObjects[75].GetComponent<Text>().text = "<color=red>" + tS_.GetText(100) + "</color> " + mS_.Round(durchschnittsMotivation, 1) + "%";
			}
		}
		timerUpdateContract += Time.deltaTime;
		if (timerUpdateContract > 1f)
		{
			timerUpdateContract = 0f;
			contractMain_.UpdateGUI();
			publishingOfferMain_.UpdateGUI();
			UpdateArbeitsmarktIcon();
		}
		timerUpdateGlobalEvent += Time.deltaTime;
		if (timerUpdateGlobalEvent > 1f)
		{
			timerUpdateGlobalEvent = 0f;
			if (mS_.globalEvent != -1)
			{
				if (!uiObjects[217].activeSelf)
				{
					uiObjects[217].SetActive(value: true);
				}
				uiObjects[217].GetComponent<Image>().sprite = iconGlobalEvents[mS_.globalEvent];
				switch (mS_.globalEvent)
				{
				case 0:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1080);
					break;
				case 1:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1081);
					break;
				case 2:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1082);
					break;
				case 3:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1083);
					break;
				case 4:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1084);
					break;
				case 5:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1085);
					break;
				case 6:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1086);
					break;
				case 7:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1087);
					break;
				case 8:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1316);
					break;
				case 9:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1088);
					break;
				case 10:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1377);
					break;
				case 11:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1384);
					break;
				case 12:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1089);
					break;
				case 13:
					uiObjects[217].GetComponent<tooltip>().c = tS_.GetText(1889);
					break;
				}
			}
			else if (uiObjects[217].activeSelf)
			{
				uiObjects[217].SetActive(value: false);
			}
		}
		UpdateRoomNoJob();
		UpdateMitarbeiterNoJob();
		UpdateExklusivVertragIcon();
		UpdateBankWarningIcon();
		UpdateWeihnachtSommerIcon();
		UpdateLangweiligIcon();
		UpdateSauerBugsIcon();
		UpdateSabotageIcons();
		UpdateAwardBonusIcon();
		UpdateSchlechteSpiele();
	}

	private void UpdateBankWarningIcon()
	{
		updateBankWarningIcon_Timer += Time.deltaTime;
		if (updateBankWarningIcon_Timer < 1f)
		{
			return;
		}
		updateBankWarningIcon_Timer = 0f;
		if (mS_.bankWarning < 6)
		{
			if (uiObjects[250].activeSelf)
			{
				uiObjects[250].SetActive(value: false);
			}
			return;
		}
		if (!uiObjects[250].activeSelf)
		{
			uiObjects[250].SetActive(value: true);
		}
		uiObjects[250].transform.GetChild(0).GetComponent<Text>().text = (18 - mS_.bankWarning).ToString();
	}

	private void UpdateWeihnachtSommerIcon()
	{
		if (mS_.month == 12 || mS_.month == 1)
		{
			if (!uiObjects[256].activeSelf)
			{
				uiObjects[256].SetActive(value: true);
			}
			if (uiObjects[257].activeSelf)
			{
				uiObjects[257].SetActive(value: false);
			}
		}
		else if (mS_.month == 6 || mS_.month == 7)
		{
			if (uiObjects[256].activeSelf)
			{
				uiObjects[256].SetActive(value: false);
			}
			if (!uiObjects[257].activeSelf)
			{
				uiObjects[257].SetActive(value: true);
			}
		}
		else
		{
			if (uiObjects[256].activeSelf)
			{
				uiObjects[256].SetActive(value: false);
			}
			if (uiObjects[257].activeSelf)
			{
				uiObjects[257].SetActive(value: false);
			}
		}
	}

	private void UpdateLangweiligIcon()
	{
		mS_.gelangweiltGenre = -1;
		if (mS_.difficulty <= 2)
		{
			if (uiObjects[258].activeSelf)
			{
				uiObjects[258].SetActive(value: false);
			}
			return;
		}
		int num = 0;
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (mS_.GetFanGenreID() == i)
			{
				continue;
			}
			num = 0;
			switch (mS_.difficulty)
			{
			case 3:
				if (mS_.lastGamesGenre[0] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[1] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[2] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[3] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[4] == i)
				{
					num++;
				}
				if (num >= 4)
				{
					mS_.gelangweiltGenre = i;
				}
				break;
			case 4:
				if (mS_.lastGamesGenre[0] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[1] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[2] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[3] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[4] == i)
				{
					num++;
				}
				if (num >= 4)
				{
					mS_.gelangweiltGenre = i;
				}
				break;
			case 5:
				if (mS_.lastGamesGenre[0] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[1] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[2] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[3] == i)
				{
					num++;
				}
				if (mS_.lastGamesGenre[4] == i)
				{
					num++;
				}
				if (num >= 3)
				{
					mS_.gelangweiltGenre = i;
				}
				break;
			}
		}
		if (mS_.gelangweiltGenre != -1)
		{
			if (!uiObjects[258].activeSelf)
			{
				uiObjects[258].SetActive(value: true);
			}
			string text = tS_.GetText(1302);
			text = text.Replace("<NAME>", genres_.GetName(mS_.gelangweiltGenre));
			uiObjects[258].GetComponent<tooltip>().c = text;
		}
		else if (uiObjects[258].activeSelf)
		{
			uiObjects[258].SetActive(value: false);
		}
	}

	private void UpdateSauerBugsIcon()
	{
		if (mS_.sauerBugs > 0)
		{
			if (!uiObjects[259].activeSelf)
			{
				uiObjects[259].SetActive(value: true);
			}
		}
		else if (uiObjects[259].activeSelf)
		{
			uiObjects[259].SetActive(value: false);
		}
	}

	private void UpdateSabotageIcons()
	{
		if (mS_.sabotage_pr > 0)
		{
			if (!sabotageIcons[0].activeSelf)
			{
				sabotageIcons[0].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2289) + "</b>\n" + tS_.GetText(2282);
				sabotageIcons[0].SetActive(value: true);
			}
		}
		else if (sabotageIcons[0].activeSelf)
		{
			sabotageIcons[0].SetActive(value: false);
		}
		if (mS_.sabotage_motivation > 0)
		{
			if (!sabotageIcons[1].activeSelf)
			{
				sabotageIcons[1].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2290) + "</b>\n" + tS_.GetText(2283);
				sabotageIcons[1].SetActive(value: true);
			}
		}
		else if (sabotageIcons[1].activeSelf)
		{
			sabotageIcons[1].SetActive(value: false);
		}
		if (mS_.sabotage_klage > 0)
		{
			if (!sabotageIcons[2].activeSelf)
			{
				sabotageIcons[2].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2291) + "</b>\n" + tS_.GetText(2284);
				sabotageIcons[2].SetActive(value: true);
			}
		}
		else if (sabotageIcons[2].activeSelf)
		{
			sabotageIcons[2].SetActive(value: false);
		}
		if (mS_.sabotage_reviews > 0)
		{
			if (!sabotageIcons[3].activeSelf)
			{
				sabotageIcons[3].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2292) + "</b>\n" + tS_.GetText(2285);
				sabotageIcons[3].SetActive(value: true);
			}
		}
		else if (sabotageIcons[3].activeSelf)
		{
			sabotageIcons[3].SetActive(value: false);
		}
		if (mS_.sabotage_geruecht > 0)
		{
			if (!sabotageIcons[4].activeSelf)
			{
				sabotageIcons[4].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2293) + "</b>\n" + tS_.GetText(2286);
				sabotageIcons[4].SetActive(value: true);
			}
		}
		else if (sabotageIcons[4].activeSelf)
		{
			sabotageIcons[4].SetActive(value: false);
		}
		if (mS_.sabotage_work > 0)
		{
			if (!sabotageIcons[5].activeSelf)
			{
				sabotageIcons[5].GetComponent<tooltip>().c = "<b>" + tS_.GetText(2294) + "</b>\n" + tS_.GetText(2287);
				sabotageIcons[5].SetActive(value: true);
			}
		}
		else if (sabotageIcons[5].activeSelf)
		{
			sabotageIcons[5].SetActive(value: false);
		}
		if (mS_.sabotage_dunkel > 0)
		{
			if (!sabotageIcons[6].activeSelf)
			{
				sabotageIcons[6].SetActive(value: true);
			}
		}
		else if (sabotageIcons[6].activeSelf)
		{
			sabotageIcons[6].SetActive(value: false);
		}
		if (mS_.sabotage_erwischt > 0)
		{
			if (!sabotageIcons[7].activeSelf)
			{
				sabotageIcons[7].SetActive(value: true);
			}
		}
		else if (sabotageIcons[7].activeSelf)
		{
			sabotageIcons[7].SetActive(value: false);
		}
	}

	private void UpdateSchlechteSpiele()
	{
		if (mS_.schlechteSpiele > 0)
		{
			if (!uiObjects[410].activeSelf)
			{
				uiObjects[410].SetActive(value: true);
			}
		}
		else if (uiObjects[410].activeSelf)
		{
			uiObjects[410].SetActive(value: false);
		}
	}

	private void UpdateAwardBonusIcon()
	{
		if (mS_.awardBonus > 0)
		{
			if (!uiObjects[404].activeSelf)
			{
				uiObjects[404].SetActive(value: true);
			}
			string text = tS_.GetText(2011);
			text = text.Replace("<NUM>", Mathf.RoundToInt(mS_.awardBonusAmount * 100f).ToString());
			uiObjects[404].GetComponent<tooltip>().c = text;
		}
		else if (uiObjects[404].activeSelf)
		{
			uiObjects[404].SetActive(value: false);
		}
	}

	private void UpdateExklusivVertragIcon()
	{
		updateExklusivVertragIcon_Timer += Time.deltaTime;
		if (updateExklusivVertragIcon_Timer < 1f)
		{
			return;
		}
		updateExklusivVertragIcon_Timer = 0f;
		if (mS_.exklusivVertrag_ID == -1)
		{
			if (uiObjects[212].activeSelf)
			{
				uiObjects[212].SetActive(value: false);
			}
			return;
		}
		publisherScript exklusivPublisher = mS_.GetExklusivPublisher();
		if ((bool)exklusivPublisher)
		{
			if (!uiObjects[212].activeSelf)
			{
				uiObjects[212].SetActive(value: true);
			}
			uiObjects[212].GetComponent<Image>().sprite = exklusivPublisher.GetLogo();
			uiObjects[212].transform.GetChild(0).GetComponent<Text>().text = mS_.exklusivVertrag_laufzeit.ToString();
			string text = tS_.GetText(1048);
			text = text.Replace("<NUM>", mS_.exklusivVertrag_laufzeit.ToString());
			string text2 = "";
			text2 = "<b>" + tS_.GetText(1021) + "\n\n";
			text2 = text2 + exklusivPublisher.GetName() + "</b>\n";
			text2 = text2 + tS_.GetText(436) + ": <color=blue><b>" + mS_.GetMoney(exklusivPublisher.GetShareExklusiv(), showDollar: true) + "</b></color>\n";
			text2 = text2 + tS_.GetText(1023) + ": <color=blue><b>" + text + "</b></color>\n";
			uiObjects[212].GetComponent<tooltip>().c = text2;
		}
	}

	public void UpdateAuftragsansehen(float f)
	{
		mS_.auftragsAnsehen += f;
		if (mS_.auftragsAnsehen < 0f)
		{
			mS_.auftragsAnsehen = 0f;
		}
		if (mS_.auftragsAnsehen > 100f)
		{
			mS_.auftragsAnsehen = 100f;
			if ((bool)mS_.achScript_)
			{
				mS_.achScript_.SetAchivement(43);
			}
		}
		if (f >= 0f)
		{
			uiObjects[76].GetComponent<Text>().text = mS_.Round(mS_.auftragsAnsehen, 1) + "% <color=green>" + tS_.GetText(99) + "</color>";
		}
		else
		{
			uiObjects[76].GetComponent<Text>().text = mS_.Round(mS_.auftragsAnsehen, 1) + "% <color=red>" + tS_.GetText(100) + "</color>";
		}
	}

	public void UpdateTrend()
	{
		uiObjects[77].GetComponent<Image>().sprite = genres_.GetPic(mS_.trendGenre);
		uiObjects[78].GetComponent<Image>().sprite = genres_.GetPic(mS_.trendAntiGenre);
		uiObjects[79].GetComponent<Text>().text = "<color=green>" + tS_.GetThemes(mS_.trendTheme) + "</color>\n<color=red>" + tS_.GetThemes(mS_.trendAntiTheme) + "</color>";
		string text = "";
		text += tS_.GetText(479);
		text = text.Replace("<NAME1>", genres_.GetName(mS_.trendGenre));
		text = text.Replace("<NAME2>", tS_.GetThemes(mS_.trendTheme));
		text += "\n\n";
		text += tS_.GetText(480);
		text = text.Replace("<NAME1>", genres_.GetName(mS_.trendAntiGenre));
		text = text.Replace("<NAME2>", tS_.GetThemes(mS_.trendAntiTheme));
		text += "\n\n";
		text += tS_.GetText(481);
		text = text.Replace("<TIME>", mS_.trendWeeks.ToString());
		uiObjects[80].GetComponent<tooltip>().c = text;
	}

	public void UpdateStudioBewertung()
	{
		updateStudioBewertungTimer -= Time.deltaTime;
		if (!(updateStudioBewertungTimer > 3f))
		{
			updateStudioBewertungTimer = 3f;
			int studioLevel = mS_.GetStudioLevel(mS_.studioPoints);
			uiObjects[16].GetComponent<tooltip>().c = "<b>" + tS_.GetStudioBewertung(studioLevel) + "</b>\n" + tS_.GetText(1493);
			for (int i = 0; i < uiObjects[16].transform.childCount; i++)
			{
				uiObjects[16].transform.GetChild(i).GetComponent<Image>().fillAmount = 0f;
			}
			if (studioLevel >= 1)
			{
				uiObjects[16].transform.GetChild(0).GetComponent<Image>().fillAmount = 0.5f;
			}
			if (studioLevel >= 2)
			{
				uiObjects[16].transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
			}
			if (studioLevel >= 3)
			{
				uiObjects[16].transform.GetChild(1).GetComponent<Image>().fillAmount = 0.5f;
			}
			if (studioLevel >= 4)
			{
				uiObjects[16].transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
			}
			if (studioLevel >= 5)
			{
				uiObjects[16].transform.GetChild(2).GetComponent<Image>().fillAmount = 0.5f;
			}
			if (studioLevel >= 6)
			{
				uiObjects[16].transform.GetChild(2).GetComponent<Image>().fillAmount = 1f;
			}
			if (studioLevel >= 7)
			{
				uiObjects[16].transform.GetChild(3).GetComponent<Image>().fillAmount = 0.5f;
			}
			if (studioLevel >= 8)
			{
				uiObjects[16].transform.GetChild(3).GetComponent<Image>().fillAmount = 1f;
			}
			if (studioLevel >= 9)
			{
				uiObjects[16].transform.GetChild(4).GetComponent<Image>().fillAmount = 0.5f;
			}
			if (studioLevel >= 10)
			{
				uiObjects[16].transform.GetChild(4).GetComponent<Image>().fillAmount = 1f;
			}
		}
	}

	public void UpdateGameSpeedButtons()
	{
		if (mS_.multiplayer)
		{
			if (mpCalls_.isClient)
			{
				if (!mS_.settings_allGamespeed)
				{
					uiObjects[203].GetComponent<Button>().interactable = false;
					uiObjects[204].GetComponent<Button>().interactable = false;
					uiObjects[205].GetComponent<Button>().interactable = false;
				}
				else
				{
					uiObjects[203].GetComponent<Button>().interactable = true;
					uiObjects[204].GetComponent<Button>().interactable = true;
					uiObjects[205].GetComponent<Button>().interactable = true;
				}
			}
			else
			{
				uiObjects[203].GetComponent<Button>().interactable = true;
				uiObjects[204].GetComponent<Button>().interactable = true;
				uiObjects[205].GetComponent<Button>().interactable = true;
			}
		}
		uiObjects[12].GetComponent<Image>().color = colors[16];
		if (mS_.GetGameSpeed() == 0f)
		{
			if (!uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: true);
			}
			if (uiObjects[13].activeSelf)
			{
				uiObjects[13].SetActive(value: false);
			}
			if (uiObjects[14].activeSelf)
			{
				uiObjects[14].SetActive(value: false);
			}
			if (!mS_.multiplayer && mS_.IsForcedPause())
			{
				uiObjects[12].GetComponent<Image>().color = Color.grey;
			}
		}
		else if (mS_.GetGameSpeed() == 1f)
		{
			if (uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: false);
			}
			if (!uiObjects[13].activeSelf)
			{
				uiObjects[13].SetActive(value: true);
			}
			if (uiObjects[14].activeSelf)
			{
				uiObjects[14].SetActive(value: false);
			}
		}
		else
		{
			if (uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: false);
			}
			if (uiObjects[13].activeSelf)
			{
				uiObjects[13].SetActive(value: false);
			}
			if (!uiObjects[14].activeSelf)
			{
				uiObjects[14].SetActive(value: true);
			}
		}
	}

	public void AddHistory(string c)
	{
		mS_.history.Add("<b>" + GetDate() + "</b>\n" + c);
	}

	public void CreateTopNewsStudiobewertung(string text_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		Object.Instantiate(uiPrefabs[19], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = text_;
		AddHistory(tS_.GetText(1478) + "\n<color=blue>" + text_ + "</color>");
	}

	public void CreateTopNewsUnlock(string name_, Sprite sprite_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		GameObject obj = Object.Instantiate(uiPrefabs[13], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
		obj.transform.Find("Text").GetComponent<Text>().text = name_;
		obj.transform.Find("Icon").GetComponent<Image>().sprite = sprite_;
		AddHistory(tS_.GetText(527) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsInfo(string text_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		Object.Instantiate(uiPrefabs[12], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = text_;
		AddHistory(text_);
	}

	public void CreateTopNewsGameAnkuendigung(string text_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		Object.Instantiate(uiPrefabs[23], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = text_;
		AddHistory(text_);
	}

	public void CreateTopNewsGameSubvention(string text_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		Object.Instantiate(uiPrefabs[26], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = text_;
		AddHistory(tS_.GetText(2384) + "\n" + text_);
	}

	public void CreateTopNewsAchivement(int id_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		GameObject obj = Object.Instantiate(uiPrefabs[22], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
		string text = "<color=blue>" + tS_.GetAchivementName(id_) + "</color>\n";
		text += tS_.GetAchivementDesc(id_);
		obj.transform.Find("Text").GetComponent<Text>().text = text;
	}

	public void CreateTopNewsTrend(string name_, Sprite sprite_)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		GameObject obj = Object.Instantiate(uiPrefabs[10], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
		obj.transform.Find("Text").GetComponent<Text>().text = name_;
		obj.transform.Find("Icon").GetComponent<Image>().sprite = sprite_;
		AddHistory(tS_.GetText(482) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsGoldeneSchallplatte(string name_)
	{
		if (mS_.newsSetting[7])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[14], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(759) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsPlatinSchallplatte(string name_)
	{
		if (mS_.newsSetting[7])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[17], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(1311) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsDiamantSchallplatte(string name_)
	{
		if (mS_.newsSetting[7])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[18], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(1312) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsNewPublisher(string name_, Sprite sprite_)
	{
		if (mS_.newsSetting[1])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject obj = Object.Instantiate(uiPrefabs[9], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			obj.transform.Find("Text").GetComponent<Text>().text = name_;
			obj.transform.Find("Icon").GetComponent<Image>().sprite = sprite_;
		}
		AddHistory(tS_.GetText(431) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsRemovePublisher(string name_, Sprite sprite_)
	{
		if (mS_.newsSetting[12])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject obj = Object.Instantiate(uiPrefabs[27], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			obj.transform.Find("Text").GetComponent<Text>().text = name_;
			obj.transform.Find("Icon").GetComponent<Image>().sprite = sprite_;
		}
		AddHistory(tS_.GetText(2410) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsCopyProtect(string name_)
	{
		if (mS_.newsSetting[2])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[7], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(289) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsAntiCheat(string name_)
	{
		if (mS_.newsSetting[8])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[16], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(1216) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsNpcEngine(string name_)
	{
		if (mS_.newsSetting[3])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			Object.Instantiate(uiPrefabs[6], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = name_;
		}
		AddHistory(tS_.GetText(265) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsForschung(string name_, Sprite sprite_)
	{
		if (mS_.newsSetting[4])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject obj = Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			obj.transform.Find("Text").GetComponent<Text>().text = name_;
			obj.transform.Find("Icon").GetComponent<Image>().sprite = sprite_;
		}
		AddHistory(tS_.GetText(164) + "\n<color=blue>" + name_ + "</color>");
	}

	public void CreateTopNewsPlatform(platformScript platS_)
	{
		if (mS_.newsSetting[5])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject gameObject = Object.Instantiate(uiPrefabs[4], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			gameObject.transform.Find("Text").GetComponent<Text>().text = platS_.GetName();
			platS_.SetPic(gameObject.transform.Find("Icon").gameObject);
		}
		AddHistory(tS_.GetText(236) + "\n<color=blue>" + platS_.GetName() + "</color>");
	}

	public void CreateTopNewsPlatformAnkuendigung(platformScript platS_)
	{
		if (mS_.newsSetting[11])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject gameObject = Object.Instantiate(uiPrefabs[25], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			gameObject.transform.Find("Text").GetComponent<Text>().text = platS_.GetName();
			platS_.SetPic(gameObject.transform.Find("Icon").gameObject);
		}
		AddHistory(tS_.GetText(2333) + "\n<color=blue>" + platS_.GetName() + "</color>");
	}

	public void CreateTopNewsPlatformRemove(platformScript platS_)
	{
		if (mS_.newsSetting[6])
		{
			FindScripts();
			sfx_.PlaySound(27, force: true);
			GameObject gameObject = Object.Instantiate(uiPrefabs[5], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform);
			gameObject.transform.Find("Text").GetComponent<Text>().text = platS_.GetName();
			platS_.SetPic(gameObject.transform.Find("Icon").gameObject);
		}
		AddHistory(tS_.GetText(237) + "\n<color=blue>" + platS_.GetName() + "</color>");
	}

	public void CreateTopNewsDevLegend(string text_, int beruf)
	{
		FindScripts();
		sfx_.PlaySound(27, force: true);
		Object.Instantiate(uiPrefabs[8], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[22].transform).transform.Find("Text").GetComponent<Text>().text = text_ + "\n<size=14><color=green>[" + tS_.GetText(beruf + 137) + "]</color></size>";
		AddHistory(tS_.GetText(426) + "\n<color=blue>" + text_ + "</color>");
	}

	public void CreateLeftNews(int roomID_, Sprite sprite_, string tooltip_, Sprite smallSprite)
	{
		if (!settings_.taskComplete)
		{
			FindScripts();
			sfx_.PlaySound(28, force: true);
			Object.Instantiate(uiPrefabs[2], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[23].transform).GetComponent<LeftNews>().Init(roomID_, sprite_, tooltip_, smallSprite);
			AddHistory(tooltip_);
		}
	}

	public string GetDate()
	{
		return string.Concat(string.Concat(tS_.GetText(102) + mS_.year, " ", tS_.GetText(103), mS_.month.ToString()), " ", tS_.GetText(104), mS_.week.ToString());
	}

	private void UpdateMitarbeiterNoJob()
	{
		timerUpdateMitarbeiterNoJob += Time.deltaTime;
		if (timerUpdateMitarbeiterNoJob < 1f)
		{
			return;
		}
		timerUpdateMitarbeiterNoJob = 0f;
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if ((bool)mS_.arrayCharactersScripts[i] && mS_.arrayCharactersScripts[i].roomID == -1 && !mS_.arrayCharactersScripts[i].picked)
			{
				uiObjects[103].GetComponent<Image>().color = colors[5];
				uiObjects[103].GetComponent<Animation>().Play();
				return;
			}
		}
		uiObjects[103].GetComponent<Image>().color = colors[6];
		uiObjects[103].GetComponent<Animation>().Stop();
	}

	private void UpdateRoomNoJob()
	{
		timerUpdateRoomNoJob += Time.deltaTime;
		if (timerUpdateRoomNoJob < 1f)
		{
			return;
		}
		timerUpdateRoomNoJob = 0f;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == -1 && mS_.arrayRoomScripts[i].myID >= 100 && mS_.arrayRoomScripts[i].typ != 0 && mS_.arrayRoomScripts[i].typ != 9 && mS_.arrayRoomScripts[i].typ != 11 && mS_.arrayRoomScripts[i].typ != 12 && mS_.arrayRoomScripts[i].typ != 15)
			{
				uiObjects[281].GetComponent<Image>().color = colors[5];
				uiObjects[281].GetComponent<Animation>().Play();
				return;
			}
		}
		uiObjects[281].GetComponent<Image>().color = colors[6];
		uiObjects[281].GetComponent<Animation>().Stop();
	}

	public void BUTTON_DeleteAllNews()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < uiObjects[22].transform.childCount; i++)
		{
			if ((bool)uiObjects[22].transform.GetChild(i))
			{
				Object.Destroy(uiObjects[22].transform.GetChild(i).gameObject);
			}
		}
	}

	public void BUTTON_ShowAllNews()
	{
		showAllNews = !showAllNews;
		sfx_.PlaySound(3, force: true);
	}

	public void BUTTON_NewsSetting()
	{
		sfx_.PlaySound(3, force: true);
		OpenMenu(hideChars: false);
		uiObjects[168].SetActive(value: true);
	}

	public void BUTTON_Options()
	{
		sfx_.PlaySound(3, force: true);
		OpenMenu(hideChars: false);
		uiObjects[155].SetActive(value: true);
	}

	public void BUTTON_UpdateAllObjects()
	{
		sfx_.PlaySound(3, force: true);
		OpenMenu(hideChars: false);
		uiObjects[342].SetActive(value: true);
		uiObjects[342].GetComponent<Menu_W_UpdateAllObjects>().Init();
	}

	public void BUTTON_Money()
	{
		sfx_.PlaySound(3, force: true);
		OpenMenu(hideChars: false);
		uiObjects[132].SetActive(value: true);
	}

	public void BUTTON_Trend()
	{
		sfx_.PlaySound(3, force: true);
		OpenMenu(hideChars: false);
		uiObjects[280].SetActive(value: true);
		uiObjects[280].GetComponent<Menu_Stats_GenreBeliebtheit>().closeMenu = true;
	}

	public void BUTTON_RoomNoJob()
	{
		sfx_.PlaySound(3, force: true);
		if (roomNoJobOld >= mS_.arrayRoomScripts.Length)
		{
			roomNoJobOld = 0;
		}
		for (int i = roomNoJobOld; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].taskID == -1 && mS_.arrayRoomScripts[i].myID >= 100 && mS_.arrayRoomScripts[i].typ != 0 && mS_.arrayRoomScripts[i].typ != 9 && mS_.arrayRoomScripts[i].typ != 11 && mS_.arrayRoomScripts[i].typ != 12 && mS_.arrayRoomScripts[i].typ != 15)
			{
				roomNoJobOld = i + 1;
				camera_.transform.parent.position = new Vector3(mS_.arrayRoomScripts[i].uiPos.x, camera_.transform.parent.position.y, mS_.arrayRoomScripts[i].uiPos.z);
				break;
			}
		}
	}

	public void BUTTON_MitarbeiterNoJob()
	{
		sfx_.PlaySound(3, force: true);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.roomID == -1)
				{
					pcS_.PickFromExternObject(array[i]);
				}
			}
		}
	}

	public void BUTTON_GameSpeed(float f)
	{
		FindScripts();
		spacePause = false;
		if (mS_.multiplayer)
		{
			if (mpCalls_.isClient)
			{
				if (mS_.settings_allGamespeed)
				{
					mpCalls_.CLIENT_Send_GameSpeed(Mathf.RoundToInt(f));
				}
				return;
			}
			if (mpCalls_.isServer)
			{
				if (!mS_.mpCalls_.GetAllPlayersReady())
				{
					uiObjects[210].SetActive(value: true);
					return;
				}
				mpCalls_.SERVER_Send_GameSpeed(Mathf.RoundToInt(f));
				mS_.SetGameSpeed(f);
			}
		}
		if (!menuOpen)
		{
			mS_.PauseGame(p: false);
			uiObjects[1].GetComponent<GUI_MainButtons>().CloseAllDropdowns();
			CloseAllRoomButtons();
			mS_.SetGameSpeed(f);
			if (!mS_.settings_TutorialOff && f == 3f)
			{
				SetTutorialStep(13);
			}
		}
	}

	public bool IsRoomMenuOpen()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("RoomButton");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i] && array[i].GetComponent<roomButtonScript>().IsMenuOpen())
			{
				return true;
			}
		}
		return false;
	}

	public void CreatePerkIconsNewGame(Menu_NewGameCEO script_, Transform parentTransform)
	{
		for (int i = 0; i < parentTransform.childCount; i++)
		{
			Object.Destroy(parentTransform.GetChild(i).gameObject);
		}
		for (int j = 0; j < script_.perks.Length; j++)
		{
			if (script_.perks[j] && (bool)uiPerks[j])
			{
				Object.Instantiate(uiPerks[j], parentTransform);
			}
		}
	}

	public void CreatePerkIcons(characterScript cS_, Transform parentTransform)
	{
		for (int i = 0; i < parentTransform.childCount; i++)
		{
			Object.Destroy(parentTransform.GetChild(i).gameObject);
		}
		for (int j = 0; j < cS_.perks.Length; j++)
		{
			if (cS_.perks[j] && (bool)uiPerks[j])
			{
				Object.Instantiate(uiPerks[j], parentTransform);
			}
		}
	}

	public void CreatePerkIconsArbeitsmarkt(charArbeitsmarkt cS_, Transform parentTransform)
	{
		for (int i = 0; i < parentTransform.childCount; i++)
		{
			Object.Destroy(parentTransform.GetChild(i).gameObject);
		}
		for (int j = 0; j < cS_.perks.Length; j++)
		{
			if (cS_.perks[j])
			{
				Object.Instantiate(uiPerks[j], parentTransform);
			}
		}
	}

	public void CloseAllRoomButtons()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("RoomButton");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				array[i].GetComponent<roomButtonScript>().CloseAllMenus();
			}
		}
	}

	public void DROPDOWN_BuildRoom(int i)
	{
		switch (i)
		{
		case 3:
			if (!forschungSonstiges_.IsErforscht(28))
			{
				return;
			}
			break;
		case 4:
			if (!forschungSonstiges_.IsErforscht(31))
			{
				return;
			}
			break;
		case 5:
			if (!forschungSonstiges_.IsErforscht(32))
			{
				return;
			}
			break;
		case 6:
			if (!forschungSonstiges_.IsErforscht(30))
			{
				return;
			}
			break;
		case 7:
			if (!forschungSonstiges_.IsErforscht(29))
			{
				return;
			}
			break;
		case 8:
			if (!forschungSonstiges_.IsErforscht(39))
			{
				return;
			}
			break;
		case 9:
			if (!forschungSonstiges_.IsErforscht(33))
			{
				return;
			}
			break;
		case 10:
			if (!unlock_.Get(8))
			{
				return;
			}
			break;
		case 13:
			if (!forschungSonstiges_.IsErforscht(34))
			{
				return;
			}
			break;
		case 14:
			if (!forschungSonstiges_.IsErforscht(33))
			{
				return;
			}
			break;
		case 15:
			if (!unlock_.Get(9))
			{
				return;
			}
			break;
		case 17:
			if (!forschungSonstiges_.IsErforscht(38))
			{
				return;
			}
			break;
		}
		ActivateMenu(uiObjects[19]);
		uiObjects[19].GetComponent<Menu_BuildRoom>().uiObjects[33].SetActive(value: true);
		uiObjects[19].GetComponent<Menu_BuildRoom>().BUTTON_SelectRoom(i);
		OpenMenu(hideChars: true);
	}

	public void DROPDOWN_BuyInventar(int i)
	{
		switch (i)
		{
		case 3:
			if (!forschungSonstiges_.IsErforscht(28))
			{
				return;
			}
			break;
		case 4:
			if (!forschungSonstiges_.IsErforscht(31))
			{
				return;
			}
			break;
		case 5:
			if (!forschungSonstiges_.IsErforscht(32))
			{
				return;
			}
			break;
		case 6:
			if (!forschungSonstiges_.IsErforscht(30))
			{
				return;
			}
			break;
		case 7:
			if (!forschungSonstiges_.IsErforscht(29))
			{
				return;
			}
			break;
		case 8:
			if (!forschungSonstiges_.IsErforscht(39))
			{
				return;
			}
			break;
		case 9:
			if (!forschungSonstiges_.IsErforscht(33))
			{
				return;
			}
			break;
		case 10:
			if (!unlock_.Get(8))
			{
				return;
			}
			break;
		case 13:
			if (!forschungSonstiges_.IsErforscht(34))
			{
				return;
			}
			break;
		case 14:
			if (!forschungSonstiges_.IsErforscht(33))
			{
				return;
			}
			break;
		case 15:
			if (!unlock_.Get(9))
			{
				return;
			}
			break;
		case 17:
			if (!forschungSonstiges_.IsErforscht(38))
			{
				return;
			}
			break;
		}
		ActivateMenu(uiObjects[20]);
		uiObjects[20].GetComponent<Menu_BuyInventar>().BUTTON_SelectInventar(i);
		OpenMenu(hideChars: true);
	}

	public void DROPDOWN_PersonalUebersicht()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[29]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_PreisUndPackung()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[220]);
			uiObjects[220].GetComponent<Menu_PackungSelect>().Init(null);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_PublishingOffer()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[349]);
			uiObjects[349].GetComponent<Menu_PublishingOfferSelect>().Init();
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_BudgetGame()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[227]);
			uiObjects[227].GetComponent<Menu_BudgetSelect>().Init();
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_GotyGame()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[274]);
			uiObjects[274].GetComponent<Menu_GOTYSelect>().Init();
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Bundle()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[267]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_BundleAddon()
	{
		if (forschungSonstiges_.IsErforscht(33))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[271]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_RemoveGameFromMarket()
	{
		if (forschungSonstiges_.IsErforscht(33) || forschungSonstiges_.IsErforscht(38) || unlock_.Get(65))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[223]);
			uiObjects[223].GetComponent<Menu_RemoveGameSelect>().Init();
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_InAppPurchaseVerwalten()
	{
		if (gF_.IsErforscht(57))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[277]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_MmoAboPreise()
	{
		if (unlock_.Get(21))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[245]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_GamePass()
	{
		if (unlock_.Get(68))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[415]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Freeware()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[446]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_HandyPreise()
	{
		if (unlock_.Get(65))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[308]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_ArcadePreise()
	{
		if (forschungSonstiges_.IsErforscht(38))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[309]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_KonsolePreis()
	{
		if (forschungSonstiges_.IsErforscht(39))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[330]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_KonsoleVomMarktNehmen()
	{
		if (forschungSonstiges_.IsErforscht(39))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[331]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Personaleinstellungen()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[197]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Arbeitsmarkt()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[30]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_PersonalGroups()
	{
		if (uiObjects[1].transform.GetChild(0).GetComponent<Button>().IsInteractable())
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[32]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Fanshop()
	{
		if (forschungSonstiges_.IsErforscht(29))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[366]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Bank()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[138]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Archiv()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[289]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Marktforschung()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[230]);
		uiObjects[230].GetComponent<Menu_Marktforschung>().Init(null);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Kundensupport()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[141]);
		uiObjects[141].GetComponent<Menu_Support_Anrufe>().Init(null);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Multiplayer()
	{
		if (mS_.multiplayer)
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[254]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_BuyDevKits()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[33]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_BuyEngine()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[42]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_BuyCopyProtect()
	{
		if (unlock_.Get(31))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[49]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_BuyAntiCheat()
	{
		if (unlock_.Get(64))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[234]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_BuyLicence()
	{
		if (unlock_.Get(25))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[51]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_PublisherExklusivvertrag()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[200]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_IpHandel()
	{
		if (unlock_.Get(69))
		{
			sfx_.PlaySound(3, force: false);
			ActivateMenu(uiObjects[435]);
			OpenMenu(hideChars: false);
		}
	}

	public void DROPDOWN_Firmenname()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[47]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Achivements()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[364]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Charts()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[114]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Platforms()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[117]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_DevPubs()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[118]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Engines()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[121]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Tochterfirmen()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[385]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_MyGames()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[123]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_MyKonsolen()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[334]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Infos()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[126]);
		OpenMenu(hideChars: false);
	}

	public void DROPDOWN_Statstics_Finanzen()
	{
		sfx_.PlaySound(3, force: false);
		ActivateMenu(uiObjects[131]);
		OpenMenu(hideChars: false);
	}

	public void BUTTON_CloseOpenAllGameTabs()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < uiObjects[81].transform.childCount; i++)
		{
			if ((bool)uiObjects[81].transform.GetChild(i).GetComponent<gameTab>())
			{
				if ((bool)uiObjects[81].transform.GetChild(i).GetComponent<gameTab>().gS_)
				{
					uiObjects[81].transform.GetChild(i).GetComponent<gameTab>().gS_.gameTab_fullView = gameTabsFullView;
				}
				uiObjects[81].transform.GetChild(i).GetComponent<gameTab>().BUTTON_Minimize();
			}
			else if ((bool)uiObjects[81].transform.GetChild(i).GetComponent<konsoleTab>())
			{
				if ((bool)uiObjects[81].transform.GetChild(i).GetComponent<konsoleTab>().pS_)
				{
					uiObjects[81].transform.GetChild(i).GetComponent<konsoleTab>().pS_.konsoleTab_fullView = gameTabsFullView;
				}
				uiObjects[81].transform.GetChild(i).GetComponent<konsoleTab>().BUTTON_Minimize();
			}
			else if ((bool)uiObjects[81].transform.GetChild(i).GetComponent<gamePassTab>())
			{
				uiObjects[81].transform.GetChild(i).GetComponent<gamePassTab>().fullView = gameTabsFullView;
				uiObjects[81].transform.GetChild(i).GetComponent<gamePassTab>().BUTTON_Minimize();
			}
		}
		gameTabsFullView = !gameTabsFullView;
	}

	public void UpdateGameTabsSortierung()
	{
		uiObjects[358].GetComponent<Menu_GameTabFilter>().DROPDOWN_Sort(save: false);
	}

	public void BUTTON_GameTabFilter()
	{
		if (!uiObjects[358].activeSelf)
		{
			sfx_.PlaySound(3, force: true);
			uiObjects[358].GetComponent<Menu_GameTabFilter>().Init(menuOpen);
			ActivateMenu(uiObjects[358]);
			OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_DeleteAllLeftNews()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 1; i < uiObjects[23].transform.childCount; i++)
		{
			Transform child = uiObjects[23].transform.GetChild(i);
			if ((bool)child && (bool)child.GetComponent<LeftNews>())
			{
				Object.Destroy(child.gameObject);
			}
		}
	}

	public taskAutoForschung AddTask_AutoForschung()
	{
		return Object.Instantiate(uiTaskPrefabs[30]).GetComponent<taskAutoForschung>();
	}

	public taskMitarbeitersuche AddTask_Mitarbeitersuche()
	{
		return Object.Instantiate(uiTaskPrefabs[25]).GetComponent<taskMitarbeitersuche>();
	}

	public taskKonsole AddTask_Konsole()
	{
		return Object.Instantiate(uiTaskPrefabs[22]).GetComponent<taskKonsole>();
	}

	public taskArcadeProduction AddTask_ArcadeProduction()
	{
		return Object.Instantiate(uiTaskPrefabs[21]).GetComponent<taskArcadeProduction>();
	}

	public taskF2PUpdate AddTask_F2PUpdate()
	{
		return Object.Instantiate(uiTaskPrefabs[20]).GetComponent<taskF2PUpdate>();
	}

	public taskMarketingSpezial AddTask_MarketingSpezial()
	{
		return Object.Instantiate(uiTaskPrefabs[19]).GetComponent<taskMarketingSpezial>();
	}

	public taskPolishing AddTask_Polishing()
	{
		return Object.Instantiate(uiTaskPrefabs[18]).GetComponent<taskPolishing>();
	}

	public taskMarktforschung AddTask_Marktforschung()
	{
		return Object.Instantiate(uiTaskPrefabs[17]).GetComponent<taskMarktforschung>();
	}

	public taskProduction AddTask_Production()
	{
		return Object.Instantiate(uiTaskPrefabs[16]).GetComponent<taskProduction>();
	}

	public taskSpielbericht AddTask_Spielbericht()
	{
		return Object.Instantiate(uiTaskPrefabs[15]).GetComponent<taskSpielbericht>();
	}

	public taskAnimationVerbessern AddTask_AnimationVerbessern()
	{
		return Object.Instantiate(uiTaskPrefabs[14]).GetComponent<taskAnimationVerbessern>();
	}

	public taskKonsoleReduceCosts AddTask_KonsoleReduceCosts()
	{
		return Object.Instantiate(uiTaskPrefabs[28]).GetComponent<taskKonsoleReduceCosts>();
	}

	public taskKonsoleHaltbarkeit AddTask_KonsoleHaltbarkeit()
	{
		return Object.Instantiate(uiTaskPrefabs[29]).GetComponent<taskKonsoleHaltbarkeit>();
	}

	public taskSoundVerbessern AddTask_SoundVerbessern()
	{
		return Object.Instantiate(uiTaskPrefabs[13]).GetComponent<taskSoundVerbessern>();
	}

	public taskGrafikVerbessern AddTask_GrafikVerbessern()
	{
		return Object.Instantiate(uiTaskPrefabs[12]).GetComponent<taskGrafikVerbessern>();
	}

	public taskGameplayVerbessern AddTask_GameplayVerbessern()
	{
		return Object.Instantiate(uiTaskPrefabs[11]).GetComponent<taskGameplayVerbessern>();
	}

	public taskUpdate AddTask_Update()
	{
		return Object.Instantiate(uiTaskPrefabs[7]).GetComponent<taskUpdate>();
	}

	public taskBugfixing AddTask_Bugfixing()
	{
		return Object.Instantiate(uiTaskPrefabs[10]).GetComponent<taskBugfixing>();
	}

	public taskForschung AddTask_Forschung()
	{
		return Object.Instantiate(uiTaskPrefabs[0]).GetComponent<taskForschung>();
	}

	public taskForschungWait AddTask_ForschungWait()
	{
		return Object.Instantiate(uiTaskPrefabs[27]).GetComponent<taskForschungWait>();
	}

	public taskEngine AddTask_Engine()
	{
		return Object.Instantiate(uiTaskPrefabs[1]).GetComponent<taskEngine>();
	}

	public taskGame AddTask_Game()
	{
		return Object.Instantiate(uiTaskPrefabs[2]).GetComponent<taskGame>();
	}

	public taskUnterstuetzen AddTask_Unterstuetzen()
	{
		return Object.Instantiate(uiTaskPrefabs[3]).GetComponent<taskUnterstuetzen>();
	}

	public taskMarketing AddTask_Marketing()
	{
		return Object.Instantiate(uiTaskPrefabs[4]).GetComponent<taskMarketing>();
	}

	public taskFankampagne AddTask_Fankampagne()
	{
		return Object.Instantiate(uiTaskPrefabs[8]).GetComponent<taskFankampagne>();
	}

	public taskSupport AddTask_Support()
	{
		return Object.Instantiate(uiTaskPrefabs[9]).GetComponent<taskSupport>();
	}

	public taskFanshop AddTask_Fanshop()
	{
		return Object.Instantiate(uiTaskPrefabs[26]).GetComponent<taskFanshop>();
	}

	public taskTraining AddTask_Training()
	{
		return Object.Instantiate(uiTaskPrefabs[5]).GetComponent<taskTraining>();
	}

	public taskContractWork AddTask_ContractWork()
	{
		return Object.Instantiate(uiTaskPrefabs[6]).GetComponent<taskContractWork>();
	}

	public taskContractWait AddTask_ContractWait()
	{
		taskContractWait component = Object.Instantiate(uiTaskPrefabs[23]).GetComponent<taskContractWait>();
		component.main_ = main_;
		component.mS_ = mS_;
		component.guiMain_ = this;
		component.tS_ = tS_;
		component.rdS_ = mS_.rdS_;
		return component;
	}

	public taskWait AddTask_Wait()
	{
		return Object.Instantiate(uiTaskPrefabs[24]).GetComponent<taskWait>();
	}

	public Sprite Get3Stars(int i)
	{
		return uiSprites[45 + i];
	}

	public void DrawIpBekanntheit(GameObject element, gameScript gS_)
	{
		int num = 0;
		if ((bool)gS_)
		{
			num = Mathf.RoundToInt(gS_.GetIpBekanntheit() * 2f);
		}
		for (int i = 0; i < element.transform.childCount; i++)
		{
			element.transform.GetChild(i).GetComponent<Image>().fillAmount = 0f;
		}
		if (num >= 1)
		{
			element.transform.GetChild(0).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (num >= 2)
		{
			element.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
		}
		if (num >= 3)
		{
			element.transform.GetChild(1).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (num >= 4)
		{
			element.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
		}
		if (num >= 5)
		{
			element.transform.GetChild(2).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (num >= 6)
		{
			element.transform.GetChild(2).GetComponent<Image>().fillAmount = 1f;
		}
		if (num >= 7)
		{
			element.transform.GetChild(3).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (num >= 8)
		{
			element.transform.GetChild(3).GetComponent<Image>().fillAmount = 1f;
		}
		if (num >= 9)
		{
			element.transform.GetChild(4).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (num >= 10)
		{
			element.transform.GetChild(4).GetComponent<Image>().fillAmount = 1f;
		}
	}

	public void DrawStars10_Color(GameObject element, int b, Color color_)
	{
		for (int i = 0; i < element.transform.childCount; i++)
		{
			element.transform.GetChild(i).GetComponent<Image>().fillAmount = 0f;
			element.transform.GetChild(i).GetComponent<Image>().color = color_;
		}
		if (b >= 1)
		{
			element.transform.GetChild(0).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (b >= 2)
		{
			element.transform.GetChild(0).GetComponent<Image>().fillAmount = 1f;
		}
		if (b >= 3)
		{
			element.transform.GetChild(1).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (b >= 4)
		{
			element.transform.GetChild(1).GetComponent<Image>().fillAmount = 1f;
		}
		if (b >= 5)
		{
			element.transform.GetChild(2).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (b >= 6)
		{
			element.transform.GetChild(2).GetComponent<Image>().fillAmount = 1f;
		}
		if (b >= 7)
		{
			element.transform.GetChild(3).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (b >= 8)
		{
			element.transform.GetChild(3).GetComponent<Image>().fillAmount = 1f;
		}
		if (b >= 9)
		{
			element.transform.GetChild(4).GetComponent<Image>().fillAmount = 0.5f;
		}
		if (b >= 10)
		{
			element.transform.GetChild(4).GetComponent<Image>().fillAmount = 1f;
		}
	}

	public void DrawStarsColor(GameObject element, int i, Color color_)
	{
		element.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(1).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(2).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(3).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(4).GetComponent<Image>().color = Color.black;
		if (i >= 1)
		{
			element.transform.GetChild(0).GetComponent<Image>().color = color_;
		}
		if (i >= 2)
		{
			element.transform.GetChild(1).GetComponent<Image>().color = color_;
		}
		if (i >= 3)
		{
			element.transform.GetChild(2).GetComponent<Image>().color = color_;
		}
		if (i >= 4)
		{
			element.transform.GetChild(3).GetComponent<Image>().color = color_;
		}
		if (i >= 5)
		{
			element.transform.GetChild(4).GetComponent<Image>().color = color_;
		}
	}

	public void DrawStars(GameObject element, int i)
	{
		element.transform.GetChild(0).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(1).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(2).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(3).GetComponent<Image>().color = Color.black;
		element.transform.GetChild(4).GetComponent<Image>().color = Color.black;
		if (i >= 1)
		{
			element.transform.GetChild(0).GetComponent<Image>().color = colors[0];
		}
		if (i >= 2)
		{
			element.transform.GetChild(1).GetComponent<Image>().color = colors[0];
		}
		if (i >= 3)
		{
			element.transform.GetChild(2).GetComponent<Image>().color = colors[0];
		}
		if (i >= 4)
		{
			element.transform.GetChild(3).GetComponent<Image>().color = colors[0];
		}
		if (i >= 5)
		{
			element.transform.GetChild(4).GetComponent<Image>().color = colors[0];
		}
	}

	private IEnumerator IE_CreateStars(GameObject element, int i)
	{
		yield return new WaitForSeconds(Random.Range(0f, 0.3f));
		Object.Instantiate(uiPrefabs[21], new Vector3(0f, 0f, 0f), Quaternion.identity, element.transform);
		Object.Instantiate(uiPrefabs[21], new Vector3(0f, 0f, 0f), Quaternion.identity, element.transform);
		Object.Instantiate(uiPrefabs[21], new Vector3(0f, 0f, 0f), Quaternion.identity, element.transform);
		Object.Instantiate(uiPrefabs[21], new Vector3(0f, 0f, 0f), Quaternion.identity, element.transform);
		Object.Instantiate(uiPrefabs[21], new Vector3(0f, 0f, 0f), Quaternion.identity, element.transform);
		DrawStars(element, i);
	}

	public void DisableTabs(GameObject p)
	{
		for (int i = 0; i < p.transform.childCount; i++)
		{
			p.transform.GetChild(i).GetComponent<Image>().color = Color.white;
		}
	}

	public void SetTab(GameObject p, int t)
	{
		DisableTabs(p);
		p.transform.GetChild(t).GetComponent<Image>().color = colors[1];
	}

	public void ShowNoMoney()
	{
		sfx_.PlaySound(29, force: false);
		uiObjects[34].SetActive(value: true);
	}

	public void ShowGameHasSaved()
	{
		sfx_.PlaySound(49, force: false);
		uiObjects[158].SetActive(value: true);
	}

	public void MessageBox(string c, bool closeMenu)
	{
		ActivateMenu(uiObjects[40]);
		uiObjects[40].GetComponent<Menu_Messagebox>().Init(c, closeMenu);
	}

	public void MessageBoxSave(int menuID, bool closeMenu)
	{
		ActivateMenu(uiObjects[297]);
		uiObjects[297].GetComponent<Menu_MessageBoxSave>().Init(menuID, closeMenu);
	}

	public void UnlockBox(string c, bool closeMenu)
	{
		OpenMenu(hideChars: false);
		ActivateMenu(uiObjects[170]);
		uiObjects[170].GetComponent<Menu_Unlock>().Init(c, closeMenu);
	}

	public void KeinEintrag(GameObject content, GameObject go)
	{
		StartCoroutine(IE_KeinEintrag(content, go));
	}

	private IEnumerator IE_KeinEintrag(GameObject content, GameObject go)
	{
		yield return new WaitForEndOfFrame();
		if (content.transform.childCount <= 0)
		{
			go.SetActive(value: true);
		}
		else
		{
			go.SetActive(value: false);
		}
	}

	public void InitVectrocity()
	{
		initVectrocity = true;
		VectorLine.SetEndCap("Arrows", EndCap.Both, unterstuetzenLine[0], unterstuetzenLine[1], unterstuetzenLine[2]);
		VectorLine.SetEndCap("ArrowsCharRoom", EndCap.Both, unterstuetzenLine[3], unterstuetzenLine[4], unterstuetzenLine[5]);
		VectorLine.SetEndCap("RoomLine", EndCap.Both, unterstuetzenLine[6], unterstuetzenLine[7], unterstuetzenLine[8]);
		VectorLine.SetEndCap("VerschiebeTask", EndCap.Both, unterstuetzenLine[9], unterstuetzenLine[10], unterstuetzenLine[11]);
	}

	public void RemoveVectrocity()
	{
		initVectrocity = false;
		VectorLine.SetEndCap("Arrows", EndCap.None);
		VectorLine.SetEndCap("ArrowsCharRoom", EndCap.None);
		VectorLine.SetEndCap("RoomLine", EndCap.None);
		VectorLine.SetEndCap("VerschiebeTask", EndCap.None);
	}

	public void UpdateFilterToggles()
	{
		if (!filterGameObject_)
		{
			filterGameObject_ = GameObject.Find("FilterView");
			if (!filterGameObject_)
			{
				return;
			}
			filterGameObject_.SetActive(value: false);
		}
		if (uiObjects[85].GetComponent<Toggle>().isOn && filterToggles != 0)
		{
			filterToggles = 0;
			uiObjects[86].GetComponent<Toggle>().isOn = false;
			uiObjects[87].GetComponent<Toggle>().isOn = false;
			filterGameObject_.SetActive(value: true);
			mapScript_.UpdateMapFilter(force: true);
			mS_.DrawFilter(filterToggles, force: true);
			mS_.SetAllFloorTextures(1);
		}
		else if (uiObjects[86].GetComponent<Toggle>().isOn && filterToggles != 1)
		{
			filterToggles = 1;
			uiObjects[85].GetComponent<Toggle>().isOn = false;
			uiObjects[87].GetComponent<Toggle>().isOn = false;
			filterGameObject_.SetActive(value: true);
			mapScript_.UpdateMapMuell(force: true);
			mS_.DrawFilter(filterToggles, force: true);
			mS_.SetAllFloorTextures(1);
		}
		else if (uiObjects[87].GetComponent<Toggle>().isOn && filterToggles != 2)
		{
			filterToggles = 2;
			uiObjects[85].GetComponent<Toggle>().isOn = false;
			uiObjects[86].GetComponent<Toggle>().isOn = false;
			filterGameObject_.SetActive(value: true);
			mapScript_.UpdateMapFilter(force: true);
			mS_.DrawFilter(filterToggles, force: true);
			mS_.SetAllFloorTextures(1);
		}
		else if (!uiObjects[85].GetComponent<Toggle>().isOn && !uiObjects[86].GetComponent<Toggle>().isOn && !uiObjects[87].GetComponent<Toggle>().isOn && filterToggles != -1)
		{
			filterToggles = -1;
			filterGameObject_.SetActive(value: false);
			mS_.SetAllFloorTextures(0);
		}
	}

	public void ShowFanLetter(int i, string gameName)
	{
		if (!uiObjects[113].activeSelf)
		{
			string fanLetter = tS_.GetFanLetter(i);
			fanLetter = fanLetter.Replace("<NAME>", gameName);
			uiObjects[113].SetActive(value: true);
			uiObjects[113].transform.GetChild(0).GetComponent<Text>().text = fanLetter;
			sfx_.PlaySound(43);
			StartCoroutine(DeactivateFanLetter());
		}
	}

	private IEnumerator DeactivateFanLetter()
	{
		yield return new WaitForSeconds(settings_.fanletterTime);
		uiObjects[113].SetActive(value: false);
	}

	public void ShowBeschwerde(int i, string mitarbeiterName)
	{
		if (settings_.disableArbeiterBeschwerden || !(timerShowBeschwerde <= 0f))
		{
			return;
		}
		timerShowBeschwerde = 180f;
		if (!uiObjects[434].activeSelf)
		{
			string text = "";
			switch (i)
			{
			case 0:
				text = tS_.GetText(2220);
				break;
			case 1:
				text = tS_.GetText(2221);
				break;
			case 2:
				text = tS_.GetText(2222);
				break;
			case 3:
				text = tS_.GetText(2223);
				break;
			case 4:
				text = tS_.GetText(2224);
				break;
			case 5:
				text = tS_.GetText(2225);
				break;
			case 6:
				text = tS_.GetText(2226);
				break;
			case 7:
				text = tS_.GetText(2227);
				break;
			case 8:
				text = tS_.GetText(2228);
				break;
			case 9:
				text = tS_.GetText(2229);
				break;
			case 10:
				text = tS_.GetText(2230);
				break;
			case 11:
				text = tS_.GetText(2231);
				text = text.Replace("<NAME>", mitarbeiterName);
				break;
			}
			text = text.Replace("<NAME>", mitarbeiterName);
			uiObjects[434].SetActive(value: true);
			uiObjects[434].transform.GetChild(0).GetComponent<Text>().text = text;
			sfx_.PlaySound(60);
			StartCoroutine(DeactivateBeschwerde());
		}
	}

	private IEnumerator DeactivateBeschwerde()
	{
		yield return new WaitForSeconds(settings_.beschwerdeTime);
		uiObjects[434].SetActive(value: false);
	}

	public void ShowInGameUI(bool show)
	{
		uiObjects[1].SetActive(show);
		uiObjects[145].SetActive(show);
		uiObjects[146].SetActive(show);
		uiObjects[147].SetActive(show);
		uiObjects[164].SetActive(show);
		uiObjects[165].SetActive(show);
		uiObjects[166].SetActive(show);
		uiObjects[167].SetActive(show);
	}

	private void UpdateUIHotkey()
	{
		if ((bool)UIHotkey)
		{
			UIHotkey.GetComponent<Button>().onClick.Invoke();
			UIHotkey = null;
			UIHotkeySiblingIndex = -1;
		}
	}

	public void SetUIHotkey(GameObject go)
	{
		if (go.transform.GetSiblingIndex() > UIHotkeySiblingIndex)
		{
			UIHotkey = go;
		}
	}

	public void SelectInputField()
	{
		selectInputField = true;
	}

	public void DeselectInputField()
	{
		selectInputField = false;
	}

	public void EVENT_MitarbeiterMotivation()
	{
		ActivateMenu(uiObjects[184]);
		uiObjects[184].GetComponent<Menu_PersonalMotivation>().Init();
		OpenMenu(hideChars: false);
	}

	public void AddChat(int id_, string c)
	{
		if (c.Contains("<IMMOBILIE>"))
		{
			c = tS_.GetText(1268);
		}
		if (c.Contains("<TOCHTERFIRMA>"))
		{
			c = tS_.GetText(2201);
		}
		GameObject gameObject = null;
		if (uiObjects[208].activeInHierarchy)
		{
			gameObject = Object.Instantiate(uiPrefabs[15], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[208].transform);
		}
		if (uiObjects[201].activeInHierarchy)
		{
			gameObject = Object.Instantiate(uiPrefabs[15], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[201].GetComponent<mpMain>().uiObjects[71].transform);
		}
		if ((bool)gameObject && id_ != -1)
		{
			if (id_ != mS_.myID)
			{
				gameObject.GetComponent<Text>().text = "<color=cyan>" + mpCalls_.GetPlayerName(id_) + " [" + mpCalls_.GetCompanyName(id_) + "]:</color> " + c;
			}
			else
			{
				gameObject.GetComponent<Text>().text = "<color=orange>" + mpCalls_.GetPlayerName(id_) + " [" + mpCalls_.GetCompanyName(id_) + "]:</color> " + c;
			}
		}
	}

	public void INPUTFIELD_Chat()
	{
		string text = uiObjects[209].GetComponent<InputField>().text;
		if (text.Length > 0)
		{
			uiObjects[209].GetComponent<InputField>().text = "";
			if (mpCalls_.isServer)
			{
				AddChat(mS_.myID, text);
				mpCalls_.SERVER_Send_Chat(mS_.myID, text);
			}
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Chat(text);
			}
		}
	}

	public void UpdatePanelMultiplayer()
	{
		if (!mS_.multiplayer)
		{
			if (uiObjects[206].activeSelf)
			{
				uiObjects[206].SetActive(value: false);
			}
			if (uiObjects[207].activeSelf)
			{
				uiObjects[207].SetActive(value: false);
			}
		}
		else
		{
			if (mS_.myID == -1)
			{
				return;
			}
			if (uiObjects[206].activeSelf != settings_.leaderboard)
			{
				uiObjects[206].SetActive(settings_.leaderboard);
			}
			if (uiObjects[207].activeSelf != settings_.chat)
			{
				uiObjects[207].SetActive(settings_.chat);
			}
			for (int i = 0; i < 4; i++)
			{
				if (mpCalls_.playersMP.Count - 1 >= i)
				{
					if (!uiObjects[206].transform.GetChild(i).gameObject.activeSelf)
					{
						uiObjects[206].transform.GetChild(i).gameObject.SetActive(value: true);
					}
				}
				else if (uiObjects[206].transform.GetChild(i).gameObject.activeSelf)
				{
					uiObjects[206].transform.GetChild(i).gameObject.SetActive(value: false);
				}
			}
			if (!settings_.leaderboard)
			{
				return;
			}
			for (int j = 0; j < mpCalls_.playersMP.Count; j++)
			{
				int playerID = mpCalls_.playersMP[j].playerID;
				Transform child = uiObjects[206].transform.GetChild(j).transform.GetChild(0);
				child.GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
				ColorBlock colorBlock = child.GetComponent<Button>().colors;
				if (mS_.settings_autoPauseForMultiplayer)
				{
					if (mpCalls_.GetPause(playerID))
					{
						colorBlock.normalColor = colors[19];
					}
					else
					{
						colorBlock.normalColor = Color.white;
					}
				}
				else
				{
					colorBlock.normalColor = Color.white;
				}
				if (playerID == mS_.myID)
				{
					colorBlock.normalColor = colors[0];
				}
				child.GetComponent<Button>().colors = colorBlock;
				child.GetChild(0).GetComponent<Text>().text = mS_.GetMoney(mpCalls_.GetMoney(playerID), showDollar: true);
				if (mpCalls_.GetMoney(playerID) >= 0)
				{
					child.GetChild(0).GetComponent<Text>().color = colors[4];
				}
				else
				{
					child.GetChild(0).GetComponent<Text>().color = colors[8];
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", mS_.GetMoney(mpCalls_.GetFans(playerID), showDollar: false));
				child.GetChild(1).GetComponent<Text>().text = text;
				child.GetChild(2).GetComponent<Image>().sprite = GetCompanyLogo(mpCalls_.GetLogo(playerID));
			}
		}
	}

	public void OpenEngineAbrechnung(gameScript script_)
	{
		ActivateMenu(uiObjects[83]);
		OpenMenu(hideChars: false);
		uiObjects[83].GetComponent<Menu_Engine_Abrechnung>().Init(script_);
	}

	public void OpenTochterfirmaAbrechnung(gameScript script_)
	{
		ActivateMenu(uiObjects[398]);
		OpenMenu(hideChars: false);
		uiObjects[398].GetComponent<Menu_TochterfirmaAbrechnung>().Init(script_);
	}

	public string GetFansTooltip()
	{
		string text = "<b>" + tS_.GetText(97) + "</b>\n" + tS_.GetText(1160) + "\n";
		SortFans();
		for (int i = 0; i < fansSortList.Count; i++)
		{
			text = ((fansSortList[i].fans >= 20000000) ? (text + "\n" + genres_.GetName(fansSortList[i].myID) + ": <color=green><b>" + mS_.GetMoney(fansSortList[i].fans, showDollar: false) + "</b></color>") : (text + "\n" + genres_.GetName(fansSortList[i].myID) + ": <color=blue><b>" + mS_.GetMoney(fansSortList[i].fans, showDollar: false) + "</b></color>"));
		}
		return text;
	}

	public void SortFans()
	{
		fansSortList.Clear();
		for (int i = 0; i < genres_.genres_FANS.Length; i++)
		{
			if (genres_.genres_FANS[i] > 0)
			{
				fansSortList.Add(new FansSortList(i, genres_.genres_FANS[i]));
			}
		}
		fansSortList = fansSortList.OrderByDescending((FansSortList fansSortList) => fansSortList.fans).ToList();
	}

	public void InitToggles()
	{
		if (PlayerPrefs.GetInt("Toggle_Walls") == 1)
		{
			uiObjects[241].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[241].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_PickChars") == 1)
		{
			uiObjects[2].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[2].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_RoomUI") == 1)
		{
			uiObjects[4].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[4].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_Ausstattung") == 1)
		{
			uiObjects[85].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[85].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_Muell") == 1)
		{
			uiObjects[86].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[86].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_Waerme") == 1)
		{
			uiObjects[87].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[87].GetComponent<Toggle>().isOn = false;
		}
		if (PlayerPrefs.GetInt("Toggle_Charts") == 1)
		{
			uiObjects[379].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[379].GetComponent<Toggle>().isOn = false;
		}
		if (!mS_.settings_TutorialOff)
		{
			uiObjects[2].GetComponent<Toggle>().isOn = false;
			uiObjects[3].GetComponent<Toggle>().isOn = false;
			uiObjects[4].GetComponent<Toggle>().isOn = false;
		}
	}

	public void TOGGLE_Charts()
	{
		if ((bool)mS_)
		{
			mS_.games_.UpdateChartsWeek();
			UpdateCharts();
		}
		if (!uiObjects[379].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_Charts", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_Charts", 1);
		}
	}

	public void TOGGLE_GameTab_TABS()
	{
		uiObjects[362].GetComponent<GamesGroupContent>().timer = 10f;
	}

	public void TOGGLE_Walls()
	{
		ShowWalls(uiObjects[241].GetComponent<Toggle>().isOn);
		if (!uiObjects[241].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_Walls", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_Walls", 1);
		}
	}

	public void TOGGLE_PickChars()
	{
		if (!mS_.settings_TutorialOff)
		{
			uiObjects[2].GetComponent<Toggle>().isOn = false;
			return;
		}
		if (!uiObjects[297].activeSelf && uiObjects[2].GetComponent<Toggle>().isOn)
		{
			if (menuOpen)
			{
				MessageBoxSave(1, closeMenu: false);
			}
			else
			{
				MessageBoxSave(1, closeMenu: true);
			}
		}
		if (!uiObjects[2].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_PickChars", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_PickChars", 1);
		}
	}

	public void TOGGLE_PickObjects()
	{
		if (!mS_.settings_TutorialOff)
		{
			uiObjects[3].GetComponent<Toggle>().isOn = false;
			return;
		}
		if (!uiObjects[297].activeSelf && uiObjects[3].GetComponent<Toggle>().isOn)
		{
			if (menuOpen)
			{
				MessageBoxSave(0, closeMenu: false);
			}
			else
			{
				MessageBoxSave(0, closeMenu: true);
			}
		}
		if (!uiObjects[3].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_PickObjects", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_PickObjects", 1);
		}
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i])
			{
				mS_.arrayObjectScripts[i].MouseLeave();
			}
		}
	}

	public void TOGGLE_RoomUI()
	{
		if (!mS_.settings_TutorialOff)
		{
			uiObjects[4].GetComponent<Toggle>().isOn = false;
		}
		else if (!uiObjects[4].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_RoomUI", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_RoomUI", 1);
		}
	}

	public void TOGGLE_Ausstattung()
	{
		if (!uiObjects[85].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_Ausstattung", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_Ausstattung", 1);
		}
	}

	public void TOGGLE_Muell()
	{
		if (!uiObjects[86].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_Muell", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_Muell", 1);
		}
	}

	public void TOGGLE_Waerme()
	{
		if (!uiObjects[87].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("Toggle_Waerme", 0);
		}
		else
		{
			PlayerPrefs.SetInt("Toggle_Waerme", 1);
		}
	}

	public void ShowWalls(bool show)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("HideWall");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			Vector3 position = array[i].transform.position;
			int num = Mathf.RoundToInt(array[i].transform.eulerAngles.y);
			bool flag = false;
			if (num == 180 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x) + 1, Mathf.RoundToInt(position.z)] == 0)
			{
				flag = true;
			}
			if (num == 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x) - 1, Mathf.RoundToInt(position.z)] == 0)
			{
				flag = true;
			}
			if ((num == -90 || num == 270) && mapScript_.mapBuilding[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z) - 1] == 0)
			{
				flag = true;
			}
			if (num == 90 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z) + 1] == 0)
			{
				flag = true;
			}
			if ((mapScript_.mapBuilding[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x + 1f), Mathf.RoundToInt(position.z)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x - 1f), Mathf.RoundToInt(position.z)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z + 1f)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x), Mathf.RoundToInt(position.z - 1f)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x + 1f), Mathf.RoundToInt(position.z + 1f)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x - 1f), Mathf.RoundToInt(position.z - 1f)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x + 1f), Mathf.RoundToInt(position.z - 1f)] != 0 && mapScript_.mapBuilding[Mathf.RoundToInt(position.x - 1f), Mathf.RoundToInt(position.z + 1f)] != 0) || flag)
			{
				if (!show)
				{
					array[i].transform.position = new Vector3(array[i].transform.position.x, 0f, array[i].transform.position.z);
				}
				else
				{
					array[i].transform.position = new Vector3(array[i].transform.position.x, -1.45f, array[i].transform.position.z);
				}
			}
		}
		array = GameObject.FindGameObjectsWithTag("DoorOben");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				array[j].GetComponent<MeshRenderer>().enabled = !show;
			}
		}
		array = GameObject.FindGameObjectsWithTag("WallBeton");
		for (int k = 0; k < array.Length; k++)
		{
			if ((bool)array[k])
			{
				array[k].GetComponent<MeshRenderer>().enabled = show;
			}
		}
	}

	private void LoadPlayerAndCompanyName()
	{
		uiObjects[159].GetComponent<Menu_NewGame>().uiObjects[0].GetComponent<InputField>().text = PlayerPrefs.GetString("CompanyName");
	}

	private void UpdateTutorial()
	{
		if (mS_.settings_TutorialOff)
		{
			if (uiObjects[248].activeSelf)
			{
				uiObjects[248].SetActive(value: false);
			}
		}
		else if (!uiObjects[248].activeSelf)
		{
			uiObjects[248].SetActive(value: true);
		}
	}

	public void SetTutorialStep(int i)
	{
		if (uiObjects[248].GetComponent<Menu_Tutorial>().step == i - 1)
		{
			uiObjects[248].GetComponent<Menu_Tutorial>().SetStep(i);
		}
	}

	public int GetTutorialStep()
	{
		return uiObjects[248].GetComponent<Menu_Tutorial>().step;
	}

	public void CameraBlend()
	{
		postProcess_.BlendIn();
	}

	public bool IsGameConceptMenuOpen()
	{
		return uiObjects[56].activeSelf;
	}

	public Sprite GetCompanyLogo(int i)
	{
		if (i < 0)
		{
			return logoSprites[0];
		}
		if (i > logoSprites.Length)
		{
			return logoSprites[0];
		}
		return logoSprites[i];
	}

	public void UpdateArbeitsmarktIcon()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
		if (array.Length != 0)
		{
			if (!uiObjects[384].activeSelf)
			{
				uiObjects[384].SetActive(value: true);
			}
			uiObjects[384].transform.GetChild(0).gameObject.GetComponent<Text>().text = array.Length.ToString();
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			int num7 = 0;
			int num8 = 0;
			for (int i = 0; i < array.Length; i++)
			{
				charArbeitsmarkt component = array[i].GetComponent<charArbeitsmarkt>();
				if ((bool)component)
				{
					switch (component.beruf)
					{
					case 0:
						num++;
						break;
					case 1:
						num2++;
						break;
					case 2:
						num3++;
						break;
					case 3:
						num4++;
						break;
					case 4:
						num5++;
						break;
					case 5:
						num6++;
						break;
					case 6:
						num7++;
						break;
					case 7:
						num8++;
						break;
					}
				}
			}
			string text = "<b>" + tS_.GetText(191) + "</b>\n\n";
			if (num > 0)
			{
				text = text + tS_.GetText(137) + ": <color=blue><b>" + num + "</b></color>\n";
			}
			if (num2 > 0)
			{
				text = text + tS_.GetText(138) + ": <color=blue><b>" + num2 + "</b></color>\n";
			}
			if (num3 > 0)
			{
				text = text + tS_.GetText(139) + ": <color=blue><b>" + num3 + "</b></color>\n";
			}
			if (num4 > 0)
			{
				text = text + tS_.GetText(140) + ": <color=blue><b>" + num4 + "</b></color>\n";
			}
			if (num5 > 0)
			{
				text = text + tS_.GetText(141) + ": <color=blue><b>" + num5 + "</b></color>\n";
			}
			if (num6 > 0)
			{
				text = text + tS_.GetText(142) + ": <color=blue><b>" + num6 + "</b></color>\n";
			}
			if (num7 > 0)
			{
				text = text + tS_.GetText(143) + ": <color=blue><b>" + num7 + "</b></color>\n";
			}
			if (num8 > 0)
			{
				text = text + tS_.GetText(144) + ": <color=blue><b>" + num8 + "</b></color>\n";
			}
			uiObjects[384].GetComponent<tooltip>().c = text;
		}
		else if (uiObjects[384].activeSelf)
		{
			uiObjects[384].SetActive(value: false);
		}
	}

	public void AddGamePassTab()
	{
		if (!gpS_.gamePass_aktiv)
		{
			RemoveGamePassTab();
		}
		else if (!gamePassTab_)
		{
			gamePassTab_ = Object.Instantiate(uiPrefabs[24], uiObjects[81].transform);
			gamePassTabScript_ = gamePassTab_.GetComponent<gamePassTab>();
			gamePassTabScript_.mS_ = mS_;
			gamePassTabScript_.main_ = main_;
			gamePassTabScript_.guiMain_ = this;
			gamePassTabScript_.sfx_ = sfx_;
			gamePassTabScript_.tS_ = tS_;
			gamePassTabScript_.themes_ = themes_;
			gamePassTabScript_.genres_ = genres_;
			gamePassTabScript_.games_ = games_;
			gamePassTabScript_.settings_ = settings_;
			gamePassTabScript_.Init();
		}
	}

	public void RemoveGamePassTab()
	{
		if ((bool)gamePassTab_)
		{
			Object.Destroy(gamePassTab_);
			gamePassTab_ = null;
			gamePassTabScript_ = null;
		}
	}

	public void SetBalkenEmployee(GameObject go, float val, int beruf_, characterScript script_)
	{
		go.transform.Find("Value").GetComponent<Text>().text = mS_.Round(val, 1).ToString();
		go.transform.Find("Fill").GetComponent<Image>().fillAmount = val * 0.01f;
		go.transform.Find("Fill").GetComponent<Image>().color = GetValColorEmployee(val);
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSkill100)
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
		}
		else if (script_.beruf != beruf_)
		{
			if (script_.perks[15])
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.6f;
			}
			else
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.5f;
			}
		}
		else
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
		}
	}

	public void SetBalkenArbeitsmarkt(GameObject go, float val, int beruf_, charArbeitsmarkt script_)
	{
		go.transform.Find("Value").GetComponent<Text>().text = mS_.Round(val, 1).ToString();
		go.transform.Find("Fill").GetComponent<Image>().fillAmount = val * 0.01f;
		go.transform.Find("Fill").GetComponent<Image>().color = GetValColorEmployee(val);
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSkill100)
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
		}
		else if (script_.beruf != beruf_)
		{
			if (script_.perks[15])
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.6f;
			}
			else
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.5f;
			}
		}
		else
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
		}
	}

	private Color GetValColorEmployee(float val)
	{
		if (val < 30f)
		{
			return colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return colorsBalken[1];
		}
		if (val >= 70f)
		{
			return colorsBalken[2];
		}
		return colorsBalken[0];
	}

	private void UpdateTopNews()
	{
		if (!uiObjects[22] || uiObjects[22].transform.childCount <= 0)
		{
			return;
		}
		for (int i = 0; i < uiObjects[22].transform.childCount; i++)
		{
			Transform child = uiObjects[22].transform.GetChild(i);
			if ((bool)child)
			{
				child.GetComponent<newsTimer>().UpdateMe(showAllNews);
			}
		}
	}
}
