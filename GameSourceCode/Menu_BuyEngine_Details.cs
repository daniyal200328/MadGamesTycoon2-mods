using UnityEngine;
using UnityEngine.UI;

public class Menu_BuyEngine_Details : MonoBehaviour
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

	public void Init(engineScript s)
	{
		FindScripts();
		eS_ = s;
		SetData();
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
			uiObjects[10].GetComponent<Image>().sprite = genres_.GetPic(eS_.spezialgenre);
			guiMain_.DrawStars(uiObjects[3], genres_.genres_LEVEL[eS_.spezialgenre]);
			platformScript spezialPlatformScript = eS_.GetSpezialPlatformScript();
			if ((bool)spezialPlatformScript)
			{
				uiObjects[13].GetComponent<Text>().text = spezialPlatformScript.GetName();
				spezialPlatformScript.SetPic(uiObjects[11]);
				guiMain_.DrawStars(uiObjects[12], spezialPlatformScript.erfahrung);
			}
			if (eS_.ownerID != mS_.myID && !eS_.gekauft)
			{
				uiObjects[8].SetActive(value: true);
				uiObjects[9].SetActive(value: false);
			}
			else
			{
				uiObjects[8].SetActive(value: false);
				uiObjects[9].SetActive(value: true);
			}
		}
	}

	private void Update()
	{
		if (mS_.multiplayer)
		{
			SetData();
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
		guiMain_.uiObjects[42].GetComponent<Menu_BuyEngine>().OnEnable();
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
}
