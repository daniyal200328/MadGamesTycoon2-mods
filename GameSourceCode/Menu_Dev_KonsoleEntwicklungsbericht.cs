using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleEntwicklungsbericht : MonoBehaviour
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

	private platforms platforms_;

	private hardware hardware_;

	private platformScript pS_;

	private roomScript rS_;

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
		if ((bool)main_)
		{
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
			if (!platforms_)
			{
				platforms_ = main_.GetComponent<platforms>();
			}
			if (!hardware_)
			{
				hardware_ = main_.GetComponent<hardware>();
			}
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

	public void Init(platformScript plat_, roomScript room_)
	{
		FindScripts();
		pS_ = plat_;
		rS_ = room_;
		SetLeitenderTechniker(GetLeitenderTechniker(), manuellSelectet: false);
		uiObjects[0].GetComponent<InputField>().text = pS_.myName;
		pS_.SetPic(uiObjects[2]);
		if (!pS_.internet)
		{
			uiObjects[10].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
		}
		else
		{
			uiObjects[10].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
		}
		if (pS_.proVersion)
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
		uiObjects[12].GetComponent<Text>().text = Mathf.RoundToInt(pS_.GetHype()).ToString();
		uiObjects[32].GetComponent<Text>().text = mS_.RoundString(pS_.GetHaltbarkeit(), 1);
		uiObjects[11].GetComponent<Text>().text = pS_.GetTypString();
		uiObjects[3].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		uiObjects[4].GetComponent<Text>().text = pS_.tech.ToString();
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(1612) + ": <b><color=blue>" + mS_.GetMoney(platforms_.GetPerformance(pS_), showDollar: false) + "</color></b>";
		uiObjects[6].GetComponent<Image>().fillAmount = pS_.GetProzent() * 0.01f;
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(450) + " " + mS_.Round(pS_.GetProzent(), 1) + "%";
		uiObjects[8].GetComponent<Text>().text = tS_.GetText(6) + " <color=red>" + mS_.GetMoney(pS_.GetGesamtAusgaben(), showDollar: true) + "</color>";
		string text = tS_.GetText(1775);
		text = text.Replace("<NUM>", pS_.weeksInDevelopment.ToString());
		uiObjects[9].GetComponent<Text>().text = text;
		uiObjects[14].GetComponent<Text>().text = hardware_.GetName(pS_.component_cpu);
		uiObjects[15].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_cpu].ToString();
		uiObjects[16].GetComponent<Text>().text = hardware_.GetName(pS_.component_ram);
		uiObjects[17].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_ram].ToString();
		uiObjects[18].GetComponent<Text>().text = hardware_.GetName(pS_.component_gfx);
		uiObjects[19].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_gfx].ToString();
		uiObjects[20].GetComponent<Text>().text = hardware_.GetName(pS_.component_sfx);
		uiObjects[21].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_sfx].ToString();
		uiObjects[22].GetComponent<Text>().text = hardware_.GetName(pS_.component_hdd);
		uiObjects[23].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_hdd].ToString();
		uiObjects[24].GetComponent<Text>().text = hardware_.GetName(pS_.component_disc);
		uiObjects[25].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_disc].ToString();
		if (pS_.component_cooling != -1)
		{
			uiObjects[26].GetComponent<Text>().text = hardware_.GetName(pS_.component_cooling);
			uiObjects[27].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_cooling].ToString();
			uiObjects[30].SetActive(value: false);
		}
		else
		{
			uiObjects[26].GetComponent<Text>().text = "-";
			uiObjects[27].GetComponent<Text>().text = "-";
			uiObjects[30].SetActive(value: true);
		}
		if (pS_.component_monitor != -1)
		{
			uiObjects[28].GetComponent<Text>().text = hardware_.GetName(pS_.component_monitor);
			uiObjects[29].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_monitor].ToString();
			uiObjects[31].SetActive(value: false);
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = "-";
			uiObjects[29].GetComponent<Text>().text = "-";
			uiObjects[31].SetActive(value: true);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if (uiObjects[0].GetComponent<InputField>().text.Length > 0)
		{
			pS_.myName = uiObjects[0].GetComponent<InputField>().text;
		}
		BUTTON_Close();
	}

	public void SetLeitenderTechniker(characterScript charS_, bool manuellSelectet)
	{
		taskKonsole taskKonsole2 = null;
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskKonsole2 = gameObject.GetComponent<taskKonsole>();
		}
		if (!charS_)
		{
			float num = 0f;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.roomID == rS_.myID)
				{
					if (component.s_technik > num)
					{
						num = component.s_technik;
						charS_ = component;
					}
					if (rS_.leitenderTechniker == component.myID)
					{
						charS_ = component;
						break;
					}
				}
			}
		}
		if (!charS_)
		{
			uiObjects[1].GetComponent<Text>().text = "---";
			taskKonsole2.leitenderTechnikerID = -1;
			taskKonsole2.techniker_ = null;
			rS_.leitenderTechniker = -1;
			return;
		}
		uiObjects[1].GetComponent<Text>().text = charS_.myName;
		taskKonsole2.leitenderTechnikerID = charS_.myID;
		taskKonsole2.techniker_ = charS_;
		if (rS_.leitenderTechniker != charS_.myID)
		{
			rS_.leitenderTechniker = -1;
		}
		if (manuellSelectet)
		{
			rS_.leitenderTechniker = charS_.myID;
		}
	}

	public characterScript GetLeitenderTechniker()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskKonsole component = gameObject.GetComponent<taskKonsole>();
			if ((bool)component)
			{
				return component.techniker_;
			}
		}
		return null;
	}

	public void BUTTON_LeitenderEntwickler()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[324]);
		guiMain_.uiObjects[324].GetComponent<Menu_LeitenderTechniker>().Init(rS_);
	}

	public void BUTTON_KonsolenDetails()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[339].SetActive(value: true);
		guiMain_.uiObjects[339].GetComponent<Menu_ShowKonsoleDetails>().Init(pS_);
	}

	public void BUTTON_RandomName()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[0].GetComponent<InputField>().text = tS_.GetPlatformName();
	}
}
