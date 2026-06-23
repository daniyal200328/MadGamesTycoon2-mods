using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ShowKonsoleDetails : MonoBehaviour
{
	public GameObject[] uiPrefabs;

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

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private platforms platforms_;

	private platformScript pS_;

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
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (uiObjects[35].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[36].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(platformScript plat_)
	{
		FindScripts();
		pS_ = plat_;
		uiObjects[0].GetComponent<Text>().text = "<b>" + pS_.GetName() + "</b>";
		if (pS_.proVersion && pS_.GetProName().Length > 0)
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n<size=16>[" + pS_.GetProName() + "]</size>";
		}
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetTypSprite();
		if (pS_.proVersion && pS_.GetProName().Length > 0)
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
		pS_.SetPic(uiObjects[2]);
		if (!pS_.internet)
		{
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
		}
		else
		{
			uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
		}
		uiObjects[4].GetComponent<Image>().sprite = pS_.GetComplexSprite();
		uiObjects[5].GetComponent<Text>().text = pS_.tech.ToString();
		uiObjects[31].GetComponent<Text>().text = mS_.RoundString(pS_.GetHaltbarkeit(), 1);
		uiObjects[6].GetComponent<Text>().text = tS_.GetText(1612) + ": <b><color=blue>" + mS_.GetMoney(pS_.performancePoints, showDollar: false) + "</color></b>";
		for (int i = 0; i < pS_.platformCompatible.Length; i++)
		{
			if (pS_.platformCompatible[i] > -1)
			{
				uiObjects[39 + i].SetActive(value: true);
				if (!pS_.platformCompatibleScript[i])
				{
					pS_.FindMyPlatformsCompatible();
				}
				if ((bool)pS_.platformCompatibleScript[i])
				{
					pS_.platformCompatibleScript[i].SetPic(uiObjects[39 + i]);
					uiObjects[39 + i].GetComponent<tooltip>().c = pS_.platformCompatibleScript[i].GetTooltip();
				}
			}
			else
			{
				uiObjects[39 + i].SetActive(value: false);
			}
		}
		if (!pS_.hwFeatures[1])
		{
			uiObjects[37].SetActive(value: true);
		}
		else
		{
			uiObjects[37].SetActive(value: false);
		}
		uiObjects[7].GetComponent<Text>().text = hardware_.GetName(pS_.component_cpu);
		uiObjects[8].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_cpu].ToString();
		uiObjects[9].GetComponent<Text>().text = hardware_.GetName(pS_.component_ram);
		uiObjects[10].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_ram].ToString();
		uiObjects[11].GetComponent<Text>().text = hardware_.GetName(pS_.component_gfx);
		uiObjects[12].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_gfx].ToString();
		uiObjects[13].GetComponent<Text>().text = hardware_.GetName(pS_.component_sfx);
		uiObjects[14].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_sfx].ToString();
		uiObjects[15].GetComponent<Text>().text = hardware_.GetName(pS_.component_hdd);
		uiObjects[16].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_hdd].ToString();
		uiObjects[17].GetComponent<Text>().text = hardware_.GetName(pS_.component_disc);
		uiObjects[18].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_disc].ToString();
		if (pS_.component_cooling != -1)
		{
			uiObjects[19].GetComponent<Text>().text = hardware_.GetName(pS_.component_cooling);
			uiObjects[20].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_cooling].ToString();
			uiObjects[25].SetActive(value: false);
		}
		else
		{
			uiObjects[19].GetComponent<Text>().text = "-";
			uiObjects[20].GetComponent<Text>().text = "-";
			uiObjects[25].SetActive(value: true);
		}
		if (pS_.component_monitor != -1)
		{
			uiObjects[21].GetComponent<Text>().text = hardware_.GetName(pS_.component_monitor);
			uiObjects[22].GetComponent<Text>().text = hardware_.hardware_TECH[pS_.component_monitor].ToString();
			uiObjects[26].SetActive(value: false);
		}
		else
		{
			uiObjects[21].GetComponent<Text>().text = "-";
			uiObjects[22].GetComponent<Text>().text = "-";
			uiObjects[26].SetActive(value: true);
		}
		uiObjects[23].GetComponent<Text>().text = hardware_.GetName(pS_.component_case);
		if (pS_.typ == 1)
		{
			uiObjects[29].GetComponent<Image>().sprite = guiMain_.uiSprites[42];
		}
		if (pS_.typ == 2)
		{
			uiObjects[29].GetComponent<Image>().sprite = guiMain_.uiSprites[43];
		}
		if (pS_.component_controller != -1)
		{
			uiObjects[24].GetComponent<Text>().text = hardware_.GetName(pS_.component_controller);
			uiObjects[28].GetComponent<Text>().text = pS_.anzController.ToString();
			uiObjects[27].SetActive(value: false);
		}
		else
		{
			uiObjects[24].GetComponent<Text>().text = "-";
			uiObjects[28].GetComponent<Text>().text = "-";
			uiObjects[27].SetActive(value: true);
		}
		if (pS_.vorgaengerID != -1)
		{
			if (!pS_.vorgaengerScript)
			{
				pS_.FindMyVorgaengerScript();
			}
			if ((bool)pS_.vorgaengerScript && pS_.vorgaengerScript.isUnlocked)
			{
				uiObjects[43].GetComponent<Text>().text = pS_.vorgaengerScript.GetName();
			}
		}
		else
		{
			uiObjects[43].GetComponent<Text>().text = tS_.GetText(2315);
		}
		if (pS_.gameID != -1)
		{
			pS_.FindMyGame();
			if ((bool)pS_.vorinstalledGame_)
			{
				uiObjects[30].GetComponent<Text>().text = pS_.vorinstalledGame_.GetNameWithTag();
			}
		}
		else
		{
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(1615);
		}
		for (int j = 0; j < uiObjects[34].transform.childCount; j++)
		{
			Object.Destroy(uiObjects[34].transform.GetChild(j).gameObject);
		}
		for (int k = 0; k < pS_.hwFeatures.Length; k++)
		{
			if (pS_.hwFeatures[k])
			{
				Item_HardwareFeatureShow component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[34].transform).GetComponent<Item_HardwareFeatureShow>();
				component2.myID = k;
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.hardwareFeatures_ = hardwareFeatures_;
			}
		}
		StartCoroutine(ResizeKonsolenFeatures());
	}

	private IEnumerator ResizeKonsolenFeatures()
	{
		uiObjects[34].SetActive(value: false);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[34].SetActive(value: true);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[38].SetActive(value: false);
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		uiObjects[38].SetActive(value: true);
	}

	public void BUTTON_ShowGames()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[340].SetActive(value: true);
		guiMain_.uiObjects[340].GetComponent<Menu_ShowKonsoleGames>().Init(pS_);
	}

	public void BUTTON_Close()
	{
		for (int i = 0; i < uiObjects[34].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[34].transform.GetChild(i).gameObject);
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
