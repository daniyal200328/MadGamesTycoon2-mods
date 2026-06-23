using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Konsolenpreis : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int verkaufspreis;

	public int devKitPreis;

	public int autoPreisGewinn = 10;

	public int garantie;

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

	public platformScript pS_;

	public taskKonsole task_;

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

	public void Init(platformScript plat_, taskKonsole t_)
	{
		FindScripts();
		pS_ = plat_;
		task_ = t_;
		uiObjects[4].GetComponent<Text>().text = pS_.GetName();
		if (pS_.verkaufspreis <= 0)
		{
			uiObjects[2].GetComponent<Slider>().value = 25f;
			uiObjects[3].GetComponent<Slider>().value = pS_.GetAktuellProductionsCosts() + 10;
			uiObjects[19].GetComponent<Slider>().value = 12f;
			uiObjects[11].GetComponent<Toggle>().isOn = true;
			uiObjects[12].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			if (uiObjects[12].GetComponent<Toggle>().isOn)
			{
				verkaufspreis = pS_.GetAktuellProductionsCosts() + pS_.autoPreisGewinn;
			}
			else
			{
				verkaufspreis = pS_.GetVerkaufspreis();
			}
			devKitPreis = pS_.price;
			uiObjects[2].GetComponent<Slider>().value = pS_.price / 1000;
			uiObjects[3].GetComponent<Slider>().value = verkaufspreis;
			uiObjects[19].GetComponent<Slider>().value = pS_.garantie;
			uiObjects[11].GetComponent<Toggle>().isOn = pS_.thridPartyGames;
			uiObjects[12].GetComponent<Toggle>().isOn = pS_.autoPreis;
		}
		SLIDER_DevKitPreis();
		SLIDER_Verkaufspreis();
		SLIDER_Garantie();
		TOGGLE_DevKit();
		TOGGLE_AutoPreis();
		SetData();
	}

	private void Update()
	{
		if (!pS_)
		{
			return;
		}
		autoPreisGewinn = verkaufspreis - pS_.GetAktuellProductionsCosts();
		if (autoPreisGewinn < 0)
		{
			autoPreisGewinn = 0;
		}
		string text = tS_.GetText(1689);
		text = text.Replace("<NUM>", mS_.GetMoney(autoPreisGewinn, showDollar: true));
		uiObjects[17].GetComponent<Text>().text = text;
		if (uiObjects[12].GetComponent<Toggle>().isOn)
		{
			if (autoPreisGewinn <= 0)
			{
				verkaufspreis = pS_.GetAktuellProductionsCosts();
			}
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis, showDollar: true);
			uiObjects[3].GetComponent<Slider>().value = verkaufspreis;
			uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(GetEinzelhandelspreis(), showDollar: true) + ".99";
		}
		if (pS_.IsOutdatet())
		{
			if (!uiObjects[18].activeSelf)
			{
				uiObjects[18].SetActive(value: true);
			}
		}
		else if (uiObjects[18].activeSelf)
		{
			uiObjects[18].SetActive(value: false);
		}
		uiObjects[21].GetComponent<Text>().text = mS_.GetMoney(pS_.subventionMoney, showDollar: true);
	}

	private int GetEinzelhandelspreis()
	{
		_ = verkaufspreis;
		return Mathf.RoundToInt((float)verkaufspreis * 1.3f + 10f);
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		uiObjects[8].GetComponent<InputField>().text = devKitPreis.ToString();
		uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(pS_.GetAktuellProductionsCosts(), showDollar: true);
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis, showDollar: true);
		uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(GetEinzelhandelspreis(), showDollar: true) + ".99";
		string text = tS_.GetText(2374);
		text = text.Replace("<NUM>", garantie.ToString());
		uiObjects[20].GetComponent<Text>().text = text;
		int num = verkaufspreis - pS_.GetAktuellProductionsCosts();
		uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		if (num < 0)
		{
			uiObjects[7].GetComponent<Text>().color = guiMain_.colors[18];
		}
		else
		{
			uiObjects[7].GetComponent<Text>().color = guiMain_.colors[13];
		}
		uiObjects[9].GetComponent<Text>().text = mS_.Round(pS_.kostenreduktion, 1) + "%";
		uiObjects[10].GetComponent<Image>().fillAmount = pS_.kostenreduktion * 0.01f;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if ((bool)task_)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[326]);
			guiMain_.uiObjects[326].GetComponent<Menu_Dev_KonsoleComplete>().Init(pS_, task_);
			guiMain_.OpenMenu(hideChars: false);
		}
		task_ = null;
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		pS_.price = devKitPreis;
		pS_.verkaufspreis = verkaufspreis;
		pS_.garantie = garantie;
		if (pS_.price < 0)
		{
			pS_.price = 0;
		}
		if (pS_.verkaufspreis < 59)
		{
			pS_.verkaufspreis = 59;
		}
		if (pS_.garantie < 12)
		{
			pS_.garantie = 12;
		}
		pS_.thridPartyGames = uiObjects[11].GetComponent<Toggle>().isOn;
		pS_.autoPreis = uiObjects[12].GetComponent<Toggle>().isOn;
		pS_.autoPreisGewinn = autoPreisGewinn;
		if ((bool)task_)
		{
			if (pS_.proVersion && pS_.GetProName().Length > 0)
			{
				pS_ = ReplaceProKonsole();
				task_.konsoleID = pS_.myID;
				if (!pS_)
				{
					return;
				}
				pS_.price = devKitPreis;
				pS_.verkaufspreis = verkaufspreis;
				pS_.garantie = garantie;
				if (pS_.price < 0)
				{
					pS_.price = 0;
				}
				if (pS_.verkaufspreis < 59)
				{
					pS_.verkaufspreis = 59;
				}
				if (pS_.garantie < 1)
				{
					pS_.garantie = 1;
				}
				pS_.thridPartyGames = uiObjects[11].GetComponent<Toggle>().isOn;
				pS_.autoPreis = uiObjects[12].GetComponent<Toggle>().isOn;
				pS_.autoPreisGewinn = autoPreisGewinn;
			}
			pS_.SetOnMarket();
			guiMain_.ActivateMenu(guiMain_.uiObjects[329]);
			guiMain_.uiObjects[329].GetComponent<Menu_KonsolenReview>().Init(pS_);
		}
		else if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Platform(pS_);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Platform(pS_);
			}
		}
		if ((bool)task_)
		{
			Object.Destroy(task_.gameObject);
		}
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator iMinusDevKit()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusDevKit();
		}
	}

	public void BUTTON_MinusDevKit()
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num--;
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusDevKit());
		SetData();
	}

	private IEnumerator iPlusDevKit()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusDevKit();
		}
	}

	public void BUTTON_PlusDevKit()
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num++;
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusDevKit());
		SetData();
	}

	private IEnumerator iMinusVerkaufspreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusVerkaufspreis(i);
		}
	}

	public void BUTTON_MinusVerkaufspreis(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num -= i;
		uiObjects[3].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusVerkaufspreis(i));
		SetData();
	}

	private IEnumerator iPlusVerkaufspreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusVerkaufspreis(i);
		}
	}

	public void BUTTON_PlusVerkaufspreis(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num += i;
		uiObjects[3].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusVerkaufspreis(i));
		SetData();
	}

	private IEnumerator iMinusGarantie()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusGarantie();
		}
	}

	public void BUTTON_MinusGarantie()
	{
		int num = Mathf.RoundToInt(uiObjects[19].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num--;
		uiObjects[19].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusGarantie());
		SetData();
	}

	private IEnumerator iPlusGarantie()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusGarantie();
		}
	}

	public void BUTTON_PlusGarantie()
	{
		int num = Mathf.RoundToInt(uiObjects[19].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num++;
		uiObjects[19].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusGarantie());
		SetData();
	}

	public void BUTTON_Subvention()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[456]);
		guiMain_.uiObjects[456].GetComponent<Menu_KonsolenSubvention>().Init(pS_, task_);
	}

	public void INPUTFIELD_DevKitPreis()
	{
		if (uiObjects[8].GetComponent<InputField>().text.Length <= 0)
		{
			devKitPreis = 0;
			return;
		}
		devKitPreis = int.Parse(uiObjects[8].GetComponent<InputField>().text);
		if (devKitPreis < 0)
		{
			uiObjects[8].GetComponent<InputField>().text = "0";
			devKitPreis = 0;
		}
		devKitPreis = devKitPreis / 1000 * 1000;
		uiObjects[2].GetComponent<Slider>().value = devKitPreis / 1000;
		SetData();
	}

	public void SLIDER_Garantie()
	{
		garantie = Mathf.RoundToInt(uiObjects[19].GetComponent<Slider>().value);
		SetData();
	}

	public void SLIDER_Verkaufspreis()
	{
		verkaufspreis = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
		SetData();
	}

	public void SLIDER_DevKitPreis()
	{
		devKitPreis = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value) * 1000;
		SetData();
	}

	public void TOGGLE_DevKit()
	{
		if (uiObjects[11].GetComponent<Toggle>().isOn)
		{
			uiObjects[8].GetComponent<InputField>().interactable = true;
			uiObjects[2].GetComponent<Slider>().interactable = true;
			uiObjects[13].GetComponent<Button>().interactable = true;
			uiObjects[14].GetComponent<Button>().interactable = true;
			uiObjects[22].SetActive(value: false);
		}
		else
		{
			uiObjects[8].GetComponent<InputField>().interactable = false;
			uiObjects[2].GetComponent<Slider>().interactable = false;
			uiObjects[13].GetComponent<Button>().interactable = false;
			uiObjects[14].GetComponent<Button>().interactable = false;
			uiObjects[22].SetActive(value: true);
		}
	}

	public void TOGGLE_AutoPreis()
	{
	}

	private platformScript ReplaceProKonsole()
	{
		if (!pS_)
		{
			return null;
		}
		platformScript platformScript2 = null;
		GameObject gameObject = GameObject.Find("PLATFORM_" + task_.proKonsoleID);
		if ((bool)gameObject)
		{
			platformScript2 = gameObject.GetComponent<platformScript>();
		}
		if (!platformScript2)
		{
			return null;
		}
		platformScript2.proName = platformScript2.myName;
		platformScript2.myName = pS_.myName;
		platformScript2.proVersion = true;
		platformScript2.tech = pS_.tech;
		platformScript2.dev_costs = pS_.dev_costs;
		platformScript2.costs_marketing += pS_.costs_marketing;
		platformScript2.costs_mitarbeiter += pS_.costs_mitarbeiter;
		platformScript2.entwicklungsKosten += pS_.entwicklungsKosten;
		platformScript2.weeksInDevelopment += pS_.weeksInDevelopment;
		platformScript2.gameID = pS_.gameID;
		platformScript2.vorinstalledGame_ = null;
		platformScript2.performancePoints = pS_.performancePoints;
		platformScript2.complex = pS_.complex;
		platformScript2.anzController = pS_.anzController;
		platformScript2.consoleColor = pS_.consoleColor;
		platformScript2.component_cpu = pS_.component_cpu;
		platformScript2.component_gfx = pS_.component_gfx;
		platformScript2.component_ram = pS_.component_ram;
		platformScript2.component_hdd = pS_.component_hdd;
		platformScript2.component_sfx = pS_.component_sfx;
		platformScript2.component_cooling = pS_.component_cooling;
		platformScript2.component_disc = pS_.component_disc;
		platformScript2.component_controller = pS_.component_controller;
		platformScript2.component_case = pS_.component_case;
		platformScript2.component_monitor = pS_.component_monitor;
		platformScript2.haltbarkeit = pS_.haltbarkeit;
		platformScript2.devPointsStart = pS_.devPointsStart;
		for (int i = 0; i < platformScript2.haltbarkeitDone.Length; i++)
		{
			platformScript2.haltbarkeitDone[i] = false;
		}
		if (platformScript2.hype < pS_.hype)
		{
			platformScript2.hype = pS_.hype;
		}
		for (int j = 0; j < platformScript2.hwFeatures.Length; j++)
		{
			platformScript2.hwFeatures[j] = pS_.hwFeatures[j];
		}
		for (int k = 0; k < platformScript2.needFeatures.Length; k++)
		{
			platformScript2.needFeatures[k] = pS_.needFeatures[k];
		}
		platformScript2.startProduktionskosten = platformScript2.CalcStartProductionsCosts();
		for (int l = 0; l < guiMain_.uiObjects[81].transform.childCount; l++)
		{
			Transform child = guiMain_.uiObjects[81].transform.GetChild(l);
			if ((bool)child)
			{
				konsoleTab component = child.GetComponent<konsoleTab>();
				if ((bool)component && component.platformID == platformScript2.myID)
				{
					component.UpdateData();
					break;
				}
			}
		}
		platformScript2.subventionMoney = pS_.subventionMoney;
		for (int m = 0; m < platformScript2.subventionGameSize.Length; m++)
		{
			platformScript2.subventionGameSize[m] = pS_.subventionGameSize[m];
		}
		Object.Destroy(pS_.gameObject);
		return platformScript2;
	}
}
