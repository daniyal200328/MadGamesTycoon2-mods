using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Engine : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	public engineScript eSUpdate_;

	public int spezialgenre = -1;

	public int spezialplatform = -1;

	public int featureAnzahl;

	public int preis;

	public int techLevel;

	public bool[] features;

	public bool[] featuresLock;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
	}

	private void OnDisable()
	{
		FindScripts();
		cmS_.disableMovement = false;
	}

	private void Update()
	{
		UpdateFeatureData();
		uiObjects[9].GetComponent<Text>().text = tS_.GetText(6) + ": <b>" + mS_.GetMoney(preis, showDollar: true) + "</b>";
		uiObjects[10].GetComponent<Text>().text = featureAnzahl + " " + tS_.GetText(160);
		uiObjects[11].GetComponent<Text>().text = tS_.GetText(4) + " " + techLevel;
	}

	private void UpdateLockedFeatures()
	{
		for (int i = 0; i < eF_.engineFeatures_UNLOCK.Length; i++)
		{
			if (eF_.engineFeatures_DATE_YEAR[i] == 1976 && eF_.engineFeatures_DATE_MONTH[i] == 1)
			{
				featuresLock[i] = true;
			}
		}
	}

	private int UpdateFeatureData()
	{
		featureAnzahl = 0;
		preis = 0;
		techLevel = 0;
		for (int i = 0; i < features.Length; i++)
		{
			if (features[i] || featuresLock[i])
			{
				featureAnzahl++;
				if (features[i])
				{
					preis += eF_.GetDevCostsForEngine(i);
				}
				if (techLevel < eF_.engineFeatures_TECH[i])
				{
					techLevel = eF_.engineFeatures_TECH[i];
				}
			}
		}
		return featureAnzahl;
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		rS_ = script_;
		eSUpdate_ = null;
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(241);
		uiObjects[15].GetComponent<Button>().interactable = true;
		uiObjects[23].GetComponent<Text>().text = tS_.GetText(2338) + ": <b>---</b>";
		uiObjects[24].GetComponent<Image>().fillAmount = 0f;
		uiObjects[29].GetComponent<Text>().text = "-";
		uiObjects[30].GetComponent<Text>().text = "-";
		uiObjects[31].SetActive(value: false);
		InitArray();
		SLIDER_Preis();
		SLIDER_Gewinnbeteiligung();
		SetSpezialgenre(spezialgenre);
		SetSpezialplatform(spezialplatform);
		UpdateLockedFeatures();
	}

	public void SELECT_InputField()
	{
		RemoveVersionsNummer();
	}

	public void DESELECT_InputField()
	{
		RemoveVersionsNummer();
		UpdateEngineName();
		uiObjects[15].GetComponent<Button>().Select();
	}

	public void TOGGLE_AutoVersion()
	{
		if (!uiObjects[21].GetComponent<Toggle>().isOn)
		{
			RemoveVersionsNummer();
			return;
		}
		RemoveVersionsNummer();
		UpdateEngineName();
	}

	private void RemoveVersionsNummer()
	{
		string text = uiObjects[4].GetComponent<InputField>().text;
		if (text.Contains(GetVersionString()))
		{
			text = text.Replace(GetVersionString(), "");
			uiObjects[4].GetComponent<InputField>().text = text;
		}
	}

	private void UpdateEngineName()
	{
		if (uiObjects[21].GetComponent<Toggle>().isOn)
		{
			string text = uiObjects[4].GetComponent<InputField>().text;
			text = text.Replace(GetVersionString(), "");
			text += GetVersionString();
			uiObjects[4].GetComponent<InputField>().text = text;
		}
	}

	public string GetVersionString()
	{
		if (UpdateFeatureData() <= 9)
		{
			return " " + techLevel + ".0" + UpdateFeatureData();
		}
		return " " + techLevel + "." + UpdateFeatureData();
	}

	public void InitUpdateEngine(roomScript scriptRoom_, engineScript scriptEngine_)
	{
		FindScripts();
		rS_ = scriptRoom_;
		eSUpdate_ = scriptEngine_;
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(242);
		uiObjects[15].GetComponent<Button>().interactable = false;
		uiObjects[23].GetComponent<Text>().text = tS_.GetText(2338) + ": <b>" + eSUpdate_.GetMarktdominanzString() + "</b>";
		uiObjects[24].GetComponent<Image>().fillAmount = eSUpdate_.GetMarktdominanz() * 0.01f;
		uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(eSUpdate_.umsatz, showDollar: true);
		uiObjects[30].GetComponent<Text>().text = eSUpdate_.GetVerkaufteLizenzen().ToString();
		uiObjects[31].SetActive(value: true);
		InitArray();
		uiObjects[4].GetComponent<InputField>().text = eSUpdate_.GetName();
		spezialgenre = eSUpdate_.spezialgenre;
		spezialplatform = eSUpdate_.spezialplatform;
		uiObjects[2].GetComponent<Slider>().value = eSUpdate_.preis / 1000;
		uiObjects[3].GetComponent<Slider>().value = eSUpdate_.gewinnbeteiligung;
		uiObjects[13].GetComponent<Toggle>().isOn = eSUpdate_.sellEngine;
		for (int i = 0; i < featuresLock.Length; i++)
		{
			if (eSUpdate_.features[i])
			{
				featuresLock[i] = true;
				features[i] = false;
			}
		}
		UpdateEngineName();
		SLIDER_Preis();
		SLIDER_Gewinnbeteiligung();
		SetSpezialgenre(spezialgenre);
		SetSpezialplatform(spezialplatform);
		UpdateLockedFeatures();
	}

	private void InitArray()
	{
		if (features.Length != eF_.engineFeatures_TYP.Length)
		{
			features = new bool[eF_.engineFeatures_TYP.Length];
		}
		if (featuresLock.Length != eF_.engineFeatures_TYP.Length)
		{
			featuresLock = new bool[eF_.engineFeatures_TYP.Length];
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)eSUpdate_)
		{
			DeleteAllDatas();
			guiMain_.ActivateMenu(guiMain_.uiObjects[41]);
			guiMain_.uiObjects[41].GetComponent<Menu_Dev_Engine_SelectOld>().Init(rS_);
		}
		else
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[36]);
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_RandomEngineName()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].GetComponent<InputField>().text = tS_.GetRandomEngineName();
		RemoveVersionsNummer();
		UpdateEngineName();
	}

	public void BUTTON_Preis(int i)
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[2].GetComponent<Slider>().value += i;
	}

	public void BUTTON_Gewinnbeteiligung(int i)
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[3].GetComponent<Slider>().value += i;
	}

	public void BUTTON_Spezialgenre()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[38]);
		guiMain_.uiObjects[38].GetComponent<Menu_Dev_Engine_Genre>().Init();
	}

	public void BUTTON_Spezialplatform()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[237]);
		guiMain_.uiObjects[237].GetComponent<Menu_Dev_EnginePlatform>().Init();
	}

	public void BUTTON_Features()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[39]);
		guiMain_.uiObjects[39].GetComponent<Menu_Dev_EngineFeatures>().Init();
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (spezialgenre == -1)
		{
			guiMain_.MessageBox(tS_.GetText(251), closeMenu: false);
			return;
		}
		if (uiObjects[4].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(252), closeMenu: false);
			return;
		}
		if (spezialplatform == -1)
		{
			guiMain_.MessageBox(tS_.GetText(1219), closeMenu: false);
			return;
		}
		if (spezialplatform != -1)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + spezialplatform);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if (techLevel > component.tech)
				{
					guiMain_.MessageBox(tS_.GetText(1220), closeMenu: false);
					return;
				}
			}
		}
		if (preis <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(253), closeMenu: false);
			return;
		}
		if (uiObjects[13].GetComponent<Toggle>().isOn && Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value) <= 0 && Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value) <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1174), closeMenu: false);
			return;
		}
		UpdateFeatureData();
		mS_.Pay(preis, 4);
		if (!eSUpdate_)
		{
			engineScript engineScript2 = eF_.CreateEngine();
			engineScript2.myID = mS_.GetNewID();
			engineScript2.myName = uiObjects[4].GetComponent<InputField>().text;
			engineScript2.ownerID = mS_.myID;
			engineScript2.spezialgenre = spezialgenre;
			engineScript2.spezialplatform = spezialplatform;
			engineScript2.spezialplatformUpdate = spezialplatform;
			engineScript2.sellEngine = uiObjects[13].GetComponent<Toggle>().isOn;
			engineScript2.preis = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value * 1000f);
			engineScript2.gewinnbeteiligung = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
			engineScript2.features = new bool[features.Length];
			engineScript2.featuresInDev = new bool[features.Length];
			for (int i = 0; i < features.Length; i++)
			{
				if (features[i])
				{
					engineScript2.featuresInDev[i] = true;
					engineScript2.devPoints += eF_.GetDevPointsForEngine(i);
				}
				if (featuresLock[i])
				{
					engineScript2.features[i] = true;
				}
			}
			if (engineScript2.devPoints <= 0f)
			{
				engineScript2.devPoints = 20f;
			}
			engineScript2.devPointsStart = engineScript2.devPoints;
			engineScript2.Init();
			taskEngine taskEngine2 = guiMain_.AddTask_Engine();
			taskEngine2.Init(fromSavegame: false);
			taskEngine2.engineID = engineScript2.myID;
			GameObject gameObject2 = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject2)
			{
				gameObject2.GetComponent<roomScript>().taskID = taskEngine2.myID;
			}
		}
		else
		{
			eSUpdate_.myName = uiObjects[4].GetComponent<InputField>().text;
			eSUpdate_.sellEngine = uiObjects[13].GetComponent<Toggle>().isOn;
			eSUpdate_.preis = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value * 1000f);
			eSUpdate_.gewinnbeteiligung = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
			eSUpdate_.featuresInDev = new bool[features.Length];
			eSUpdate_.spezialplatformUpdate = spezialplatform;
			eSUpdate_.updating = true;
			for (int j = 0; j < features.Length; j++)
			{
				if (features[j])
				{
					eSUpdate_.featuresInDev[j] = true;
					eSUpdate_.devPoints += eF_.GetDevPointsForEngine(j);
				}
			}
			eSUpdate_.devPointsStart = eSUpdate_.devPoints;
			taskEngine taskEngine3 = guiMain_.AddTask_Engine();
			taskEngine3.Init(fromSavegame: false);
			taskEngine3.engineID = eSUpdate_.myID;
			GameObject gameObject3 = GameObject.Find("Room_" + rS_.myID);
			if ((bool)gameObject3)
			{
				gameObject3.GetComponent<roomScript>().taskID = taskEngine3.myID;
			}
		}
		DeleteAllDatas();
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void SLIDER_Preis()
	{
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value * 1000f), showDollar: true);
	}

	public void SLIDER_Gewinnbeteiligung()
	{
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value) + "%";
	}

	public void TOGGLE_EngineVerkaufen()
	{
		sfx_.PlaySound(12, force: true);
	}

	public void SetSpezialgenre(int i)
	{
		if (i >= 0)
		{
			spezialgenre = i;
			uiObjects[5].GetComponent<Image>().sprite = genres_.GetPic(spezialgenre);
			uiObjects[6].GetComponent<Text>().text = genres_.GetName(spezialgenre);
			uiObjects[7].GetComponent<Image>().sprite = guiMain_.uiSprites[1];
			guiMain_.DrawStars(uiObjects[8], genres_.genres_LEVEL[spezialgenre]);
		}
		else
		{
			uiObjects[5].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[6].GetComponent<Text>().text = tS_.GetText(248);
			uiObjects[7].GetComponent<Image>().sprite = guiMain_.uiSprites[0];
			guiMain_.DrawStars(uiObjects[8], 0);
		}
	}

	public void SetSpezialplatform(int i)
	{
		if (i >= 0)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + i);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				spezialplatform = i;
				component.SetPic(uiObjects[16]);
				uiObjects[17].GetComponent<Text>().text = component.GetName();
				uiObjects[19].GetComponent<Image>().sprite = guiMain_.uiSprites[1];
				guiMain_.DrawStars(uiObjects[18], component.erfahrung);
				uiObjects[20].GetComponent<Text>().text = component.tech.ToString();
			}
			else
			{
				SetSpezialplatform(-1);
			}
		}
		else
		{
			uiObjects[16].GetComponent<Image>().material = null;
			uiObjects[16].GetComponent<Image>().color = Color.white;
			uiObjects[16].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[17].GetComponent<Text>().text = tS_.GetText(248);
			uiObjects[19].GetComponent<Image>().sprite = guiMain_.uiSprites[0];
			guiMain_.DrawStars(uiObjects[18], 0);
			uiObjects[20].GetComponent<Text>().text = "-";
		}
	}

	public void SetFeature(int i, bool b)
	{
		RemoveVersionsNummer();
		features[i] = b;
		UpdateFeatureData();
		UpdateEngineName();
	}

	private void DeleteAllDatas()
	{
		uiObjects[4].GetComponent<InputField>().text = "";
		spezialgenre = -1;
		spezialplatform = -1;
		eSUpdate_ = null;
		for (int i = 0; i < features.Length; i++)
		{
			features[i] = false;
			featuresLock[i] = false;
		}
	}

	public void BUTTON_PlatformKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[33]);
	}

	public void BUTTON_GenreBeliebtheit()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[280].SetActive(value: true);
	}

	public void DROPDOWN_Vertriebsart()
	{
		int value = uiObjects[22].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[22].name, value);
		ShowVertriebsartWindow();
	}

	private void ShowVertriebsartWindow()
	{
		if (!tS_)
		{
			FindScripts();
		}
		int value = uiObjects[22].GetComponent<Dropdown>().value;
		uiObjects[25].SetActive(value: false);
		uiObjects[26].SetActive(value: false);
		uiObjects[27].SetActive(value: false);
		switch (value)
		{
		case 0:
			uiObjects[27].SetActive(value: true);
			break;
		case 1:
			uiObjects[25].SetActive(value: true);
			if ((bool)eSUpdate_)
			{
				uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(eSUpdate_.umsatz, showDollar: true);
				uiObjects[30].GetComponent<Text>().text = eSUpdate_.GetVerkaufteLizenzen().ToString();
			}
			else
			{
				uiObjects[29].GetComponent<Text>().text = "0";
				uiObjects[30].GetComponent<Text>().text = "0";
			}
			break;
		case 2:
		{
			uiObjects[26].SetActive(value: true);
			string text = tS_.GetText(2342);
			text = ((!eSUpdate_) ? text.Replace("<NUM>", "<b><color=#34495E>0</color></b>") : text.Replace("<NUM>", "<b><color=#34495E>" + eSUpdate_.GetGamesAmountOnMarket() + "</color></b>"));
			uiObjects[28].GetComponent<Text>().text = text;
			break;
		}
		}
	}

	public void BUTTON_ShowGames()
	{
		if ((bool)eSUpdate_)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[45]);
			guiMain_.uiObjects[45].GetComponent<Menu_Engine_ShowGames>().Init(eSUpdate_);
		}
	}
}
