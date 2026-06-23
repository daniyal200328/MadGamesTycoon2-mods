using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleComplete : MonoBehaviour
{
	public GameObject[] uiObjects;

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

	private forschungSonstiges forschungSonstiges_;

	private platforms platforms_;

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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(platformScript s1_, taskKonsole s2_)
	{
		FindScripts();
		pS_ = s1_;
		task_ = s2_;
		string text = "";
		if (pS_.proVersion)
		{
			text = tS_.GetText(2323);
			text = text.Replace("<NAME>", "<color=blue>" + pS_.GetProName() + "</color>");
			uiObjects[11].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[11].GetComponent<Text>().text = tS_.GetText(1638);
		}
		uiObjects[0].GetComponent<Text>().text = pS_.myName;
		pS_.SetPic(uiObjects[1]);
		uiObjects[2].GetComponent<Image>().sprite = pS_.GetTypSprite();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(pS_.GetHype()).ToString();
		uiObjects[12].GetComponent<Text>().text = mS_.RoundString(pS_.GetHaltbarkeit(), 1);
		uiObjects[4].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		uiObjects[5].GetComponent<Text>().text = pS_.tech.ToString();
		uiObjects[6].GetComponent<Text>().text = tS_.GetText(1612) + ": <b><color=blue>" + mS_.GetMoney(platforms_.GetPerformance(pS_), showDollar: false) + "</color></b>";
		uiObjects[8].GetComponent<Text>().text = tS_.GetText(6) + " <color=red>" + mS_.GetMoney(pS_.GetGesamtAusgaben(), showDollar: true) + "</color>";
		if (pS_.proVersion)
		{
			if (!uiObjects[10].activeSelf)
			{
				uiObjects[10].SetActive(value: true);
			}
		}
		else if (uiObjects[10].activeSelf)
		{
			uiObjects[10].SetActive(value: false);
		}
		text = tS_.GetText(1775);
		text = text.Replace("<NUM>", pS_.weeksInDevelopment.ToString());
		uiObjects[7].GetComponent<Text>().text = text;
		if (!pS_.internet)
		{
			uiObjects[9].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
		}
		else
		{
			uiObjects[9].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Release()
	{
		sfx_.PlaySound(3, force: true);
		if (!pS_)
		{
			return;
		}
		if (pS_.proVersion && pS_.GetProName().Length > 0)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + task_.proKonsoleID);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component)
				{
					pS_.price = component.price;
					pS_.autoPreis = component.autoPreis;
					pS_.verkaufspreis = component.verkaufspreis;
					pS_.autoPreisGewinn = component.autoPreisGewinn;
					pS_.thridPartyGames = component.thridPartyGames;
					pS_.kostenreduktion = component.kostenreduktion;
					pS_.garantie = component.garantie;
					pS_.subventionMoney = component.subventionMoney;
					for (int i = 0; i < pS_.subventionGameSize.Length; i++)
					{
						pS_.subventionGameSize[i] = component.subventionGameSize[i];
					}
				}
			}
			if (!pS_)
			{
				return;
			}
		}
		pS_.startProduktionskosten = pS_.CalcStartProductionsCosts();
		guiMain_.ActivateMenu(guiMain_.uiObjects[328]);
		guiMain_.uiObjects[328].GetComponent<Menu_Konsolenpreis>().Init(pS_, task_);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Verwerfen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[327]);
		guiMain_.uiObjects[327].GetComponent<Menu_W_Dev_KonsoleVerwerfen>().Init(pS_, task_);
	}

	public void BUTTON_KonsolenDetails()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[339].SetActive(value: true);
		guiMain_.uiObjects[339].GetComponent<Menu_ShowKonsoleDetails>().Init(pS_);
	}
}
