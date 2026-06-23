using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Engine_View : MonoBehaviour
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

	private engineScript eS_;

	private float updateTimer;

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
	}

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	public void Init(engineScript s)
	{
		FindScripts();
		eS_ = s;
		SetData();
		uiObjects[20].GetComponent<Button>().interactable = false;
		if (eS_.ownerID == mS_.myID)
		{
			uiObjects[8].SetActive(value: true);
			uiObjects[16].SetActive(value: false);
			uiObjects[9].GetComponent<Slider>().value = eS_.preis / 1000;
			uiObjects[10].GetComponent<Slider>().value = eS_.gewinnbeteiligung;
			uiObjects[11].GetComponent<Toggle>().isOn = eS_.sellEngine;
			uiObjects[17].GetComponent<Text>().text = mS_.GetMoney(eS_.umsatz, showDollar: true);
			uiObjects[18].GetComponent<Text>().text = eS_.GetVerkaufteLizenzen().ToString();
			uiObjects[22].GetComponent<Text>().text = tS_.GetText(2338) + ": <b>" + eS_.GetMarktdominanzString() + "</b>";
			uiObjects[21].GetComponent<Image>().fillAmount = eS_.GetMarktdominanz() * 0.01f;
		}
		else
		{
			uiObjects[8].SetActive(value: false);
			uiObjects[16].SetActive(value: true);
			if (eS_.gekauft)
			{
				uiObjects[20].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[20].GetComponent<Button>().interactable = true;
			}
		}
	}

	private void SetData()
	{
		if ((bool)eS_)
		{
			uiObjects[0].GetComponent<Text>().text = eS_.GetName();
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(4) + " " + eS_.GetTechLevel();
			uiObjects[2].GetComponent<Text>().text = genres_.GetName(eS_.spezialgenre);
			uiObjects[4].GetComponent<Text>().text = eS_.GetGamesAmount() + " " + tS_.GetText(271);
			uiObjects[5].GetComponent<Text>().text = eS_.GetFeaturesAmount() + " " + tS_.GetText(272);
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(eS_.preis, showDollar: true);
			uiObjects[7].GetComponent<Text>().text = eS_.gewinnbeteiligung + "%";
			uiObjects[12].GetComponent<Image>().sprite = genres_.GetPic(eS_.spezialgenre);
			if (eS_.date_year > 0)
			{
				uiObjects[19].GetComponent<Text>().text = eS_.GetReleaseDateString();
			}
			else
			{
				uiObjects[19].GetComponent<Text>().text = "";
			}
			guiMain_.DrawStars(uiObjects[3], genres_.genres_LEVEL[eS_.spezialgenre]);
			platformScript spezialPlatformScript = eS_.GetSpezialPlatformScript();
			if ((bool)spezialPlatformScript)
			{
				uiObjects[15].GetComponent<Text>().text = spezialPlatformScript.GetName();
				spezialPlatformScript.SetPic(uiObjects[13]);
				guiMain_.DrawStars(uiObjects[14], spezialPlatformScript.erfahrung);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Kaufen()
	{
		sfx_.PlaySound(3, force: true);
		mS_.Pay(eS_.preis, 5);
		eS_.gekauft = true;
		if (eS_.ownerID != -1)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, eS_.ownerID, 1, eS_.preis, eS_.myID);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Payment(eS_.ownerID, 1, eS_.preis, eS_.myID);
			}
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_ShowFeatures()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[44]);
		guiMain_.uiObjects[44].GetComponent<Menu_Engine_ShowFeatures>().Init(eS_);
	}

	public void BUTTON_ShowGames()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[45]);
		guiMain_.uiObjects[45].GetComponent<Menu_Engine_ShowGames>().Init(eS_);
	}

	public void SLIDER_Preis()
	{
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(uiObjects[9].GetComponent<Slider>().value * 1000f), showDollar: true);
	}

	public void SLIDER_Gewinnbeteiligung()
	{
		uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(uiObjects[10].GetComponent<Slider>().value) + "%";
	}

	public void TOGGLE_EngineVerkaufen()
	{
		sfx_.PlaySound(12, force: true);
	}

	public void BUTTON_Preis(int i)
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[9].GetComponent<Slider>().value += i;
	}

	public void BUTTON_Gewinnbeteiligung(int i)
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[10].GetComponent<Slider>().value += i;
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (eS_.ownerID == mS_.myID && uiObjects[11].GetComponent<Toggle>().isOn && Mathf.RoundToInt(uiObjects[9].GetComponent<Slider>().value) <= 0 && Mathf.RoundToInt(uiObjects[10].GetComponent<Slider>().value) <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1174), closeMenu: false);
			return;
		}
		if (eS_.ownerID == mS_.myID)
		{
			eS_.preis = Mathf.RoundToInt(uiObjects[9].GetComponent<Slider>().value * 1000f);
			eS_.gewinnbeteiligung = Mathf.RoundToInt(uiObjects[10].GetComponent<Slider>().value);
			eS_.sellEngine = uiObjects[11].GetComponent<Toggle>().isOn;
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Engine(eS_);
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_Engine(eS_);
				}
			}
		}
		base.gameObject.SetActive(value: false);
	}
}
