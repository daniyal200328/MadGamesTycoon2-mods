using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_TochterfirmaSettings : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private games games_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public Toggle[] copyToggles;

	public publisherScript pS_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		if (uiObjects[11].GetComponent<Toggle>().isOn)
		{
			uiObjects[15].GetComponent<Toggle>().isOn = true;
			uiObjects[16].GetComponent<Toggle>().isOn = true;
			uiObjects[17].GetComponent<Toggle>().isOn = true;
			uiObjects[18].GetComponent<Toggle>().isOn = true;
			uiObjects[15].GetComponent<Toggle>().interactable = false;
			uiObjects[16].GetComponent<Toggle>().interactable = false;
			uiObjects[17].GetComponent<Toggle>().interactable = false;
			uiObjects[18].GetComponent<Toggle>().interactable = false;
			uiObjects[33].GetComponent<Button>().interactable = false;
			uiObjects[34].GetComponent<Button>().interactable = false;
			uiObjects[35].GetComponent<Button>().interactable = false;
			uiObjects[36].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[15].GetComponent<Toggle>().interactable = true;
			uiObjects[16].GetComponent<Toggle>().interactable = true;
			uiObjects[17].GetComponent<Toggle>().interactable = true;
			uiObjects[18].GetComponent<Toggle>().interactable = true;
			uiObjects[33].GetComponent<Button>().interactable = true;
			uiObjects[34].GetComponent<Button>().interactable = true;
			uiObjects[35].GetComponent<Button>().interactable = true;
			uiObjects[36].GetComponent<Button>().interactable = true;
		}
		if (uiObjects[10].GetComponent<Toggle>().isOn)
		{
			uiObjects[32].GetComponent<Dropdown>().interactable = true;
		}
		else
		{
			uiObjects[32].GetComponent<Dropdown>().interactable = false;
		}
		if (uiObjects[23].GetComponent<Toggle>().isOn)
		{
			uiObjects[44].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[44].GetComponent<Button>().interactable = false;
		}
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		if (pS_.tf_publisher)
		{
			list.Add(tS_.GetText(432));
		}
		else
		{
			list.Add("<color=red>" + tS_.GetText(432) + "</color>");
		}
		if (pS_.tf_developer)
		{
			list.Add(tS_.GetText(274));
		}
		else
		{
			list.Add("<color=red>" + tS_.GetText(274) + "</color>");
		}
		if (pS_.tf_publisher && pS_.tf_developer)
		{
			list.Add(tS_.GetText(432) + " & " + tS_.GetText(274));
		}
		else
		{
			list.Add("<color=red>" + tS_.GetText(432) + " & " + tS_.GetText(274) + "</color>");
		}
		uiObjects[5].GetComponent<Dropdown>().ClearOptions();
		uiObjects[5].GetComponent<Dropdown>().AddOptions(list);
		list = new List<string>();
		list.Add(tS_.GetText(1963));
		list.Add(tS_.GetText(1964));
		list.Add(tS_.GetText(1965));
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		list = new List<string>();
		list.Add(tS_.GetText(1966));
		list.Add(tS_.GetText(329));
		list.Add(tS_.GetText(330));
		list.Add(tS_.GetText(331));
		list.Add(tS_.GetText(332));
		list.Add(tS_.GetText(333));
		list.Add(tS_.GetText(2193));
		uiObjects[7].GetComponent<Dropdown>().ClearOptions();
		uiObjects[7].GetComponent<Dropdown>().AddOptions(list);
		list = new List<string>();
		list.Add(tS_.GetText(1966));
		for (int i = 0; i < genres_.genres_LEVEL.Length; i++)
		{
			if (genres_.genres_UNLOCK[i])
			{
				list.Add(genres_.GetName(i));
			}
			else
			{
				list.Add("<color=red>" + genres_.GetName(i) + "</color>");
			}
		}
		uiObjects[8].GetComponent<Dropdown>().ClearOptions();
		uiObjects[8].GetComponent<Dropdown>().AddOptions(list);
		list = new List<string>();
		list.Add(tS_.GetText(1966));
		list.Add("< 10%");
		list.Add("< 20%");
		list.Add("< 30%");
		list.Add("< 40%");
		list.Add("< 50%");
		list.Add("< 60%");
		list.Add("< 70%");
		list.Add("< 80%");
		list.Add("< 90%");
		uiObjects[32].GetComponent<Dropdown>().ClearOptions();
		uiObjects[32].GetComponent<Dropdown>().AddOptions(list);
	}

	public void Init(publisherScript pubS_)
	{
		pS_ = pubS_;
		FindScripts();
		InitDropdowns();
		SetData();
		LoadCopyToggles();
	}

	private void LoadCopyToggles()
	{
		for (int i = 0; i < copyToggles.Length; i++)
		{
			if (PlayerPrefs.HasKey(copyToggles[i].name))
			{
				if (PlayerPrefs.GetInt(copyToggles[i].name) == 0)
				{
					copyToggles[i].isOn = false;
				}
				else
				{
					copyToggles[i].isOn = true;
				}
			}
		}
	}

	private void SaveCopyToggles()
	{
		for (int i = 0; i < copyToggles.Length; i++)
		{
			if (!copyToggles[i].isOn)
			{
				PlayerPrefs.SetInt(copyToggles[i].name, 0);
			}
			else
			{
				PlayerPrefs.SetInt(copyToggles[i].name, 1);
			}
		}
	}

	private void SetData()
	{
		if (pS_.publisher && !pS_.developer)
		{
			uiObjects[5].GetComponent<Dropdown>().value = 0;
		}
		if (!pS_.publisher && pS_.developer)
		{
			uiObjects[5].GetComponent<Dropdown>().value = 1;
		}
		if (pS_.publisher && pS_.developer)
		{
			uiObjects[5].GetComponent<Dropdown>().value = 2;
		}
		uiObjects[6].GetComponent<Dropdown>().value = pS_.tf_entwicklungsdauer;
		uiObjects[7].GetComponent<Dropdown>().value = pS_.tf_gameSize;
		uiObjects[8].GetComponent<Dropdown>().value = pS_.tf_gameGenre;
		uiObjects[10].GetComponent<Toggle>().isOn = pS_.tf_autoRelease;
		uiObjects[11].GetComponent<Toggle>().isOn = pS_.tf_onlyPlayerConsole;
		uiObjects[12].GetComponent<Toggle>().isOn = pS_.tf_allowMMO;
		uiObjects[13].GetComponent<Toggle>().isOn = pS_.tf_allowF2P;
		uiObjects[14].GetComponent<Toggle>().isOn = pS_.tf_allowAddon;
		uiObjects[15].GetComponent<Toggle>().isOn = pS_.tf_noArcade;
		uiObjects[16].GetComponent<Toggle>().isOn = pS_.tf_noHandy;
		uiObjects[17].GetComponent<Toggle>().isOn = pS_.tf_noRetro;
		uiObjects[18].GetComponent<Toggle>().isOn = pS_.tf_noPorts;
		uiObjects[19].GetComponent<Toggle>().isOn = pS_.tf_noBudget;
		uiObjects[20].GetComponent<Toggle>().isOn = pS_.tf_noGOTY;
		uiObjects[21].GetComponent<Toggle>().isOn = pS_.tf_noRemaster;
		uiObjects[22].GetComponent<Toggle>().isOn = pS_.tf_noSpinoffs;
		uiObjects[23].GetComponent<Toggle>().isOn = pS_.tf_ownPublisher;
		uiObjects[43].GetComponent<Toggle>().isOn = pS_.tf_autoGamePass;
		uiObjects[32].GetComponent<Dropdown>().value = pS_.tf_autoReleaseVal;
		uiObjects[41].GetComponent<Toggle>().isOn = pS_.tf_noBundles;
		uiObjects[42].GetComponent<Toggle>().isOn = pS_.tf_noAddonBundles;
		UpdateData();
	}

	public void UpdateData()
	{
		if (pS_.tf_gameTopic != -1)
		{
			uiObjects[24].GetComponent<Text>().text = "<b>" + tS_.GetThemes(pS_.tf_gameTopic) + "</b>";
		}
		else
		{
			uiObjects[24].GetComponent<Text>().text = tS_.GetText(1966);
		}
		for (int i = 0; i < pS_.tf_ipFocus.Length; i++)
		{
			if (pS_.tf_ipFocus[i] != -1)
			{
				GameObject gameObject = GameObject.Find("GAME_" + pS_.tf_ipFocus[i]);
				if ((bool)gameObject)
				{
					uiObjects[47 + i].GetComponent<Text>().text = "<b>" + gameObject.GetComponent<gameScript>().GetIpName() + "</b>";
					continue;
				}
				pS_.tf_ipFocus[i] = -1;
				uiObjects[47 + i].GetComponent<Text>().text = tS_.GetText(1966);
			}
			else
			{
				uiObjects[47 + i].GetComponent<Text>().text = tS_.GetText(1966);
			}
		}
		if (pS_.tf_engine != -1 && pS_.tf_engine != 0)
		{
			GameObject gameObject2 = GameObject.Find("ENGINE_" + pS_.tf_engine);
			if ((bool)gameObject2)
			{
				engineScript component = gameObject2.GetComponent<engineScript>();
				if ((bool)component)
				{
					uiObjects[31].GetComponent<Text>().text = "<b>" + component.GetName() + "</b>";
				}
			}
		}
		else
		{
			uiObjects[31].GetComponent<Text>().text = tS_.GetText(1966);
		}
		for (int j = 0; j < pS_.tf_platformFocus.Length; j++)
		{
			if (pS_.tf_platformFocus[j] != -1)
			{
				GameObject gameObject3 = GameObject.Find("PLATFORM_" + pS_.tf_platformFocus[j]);
				if ((bool)gameObject3)
				{
					platformScript component2 = gameObject3.GetComponent<platformScript>();
					if ((bool)component2)
					{
						if (!component2.vomMarktGenommen)
						{
							uiObjects[37 + j].GetComponent<Text>().text = "<b>" + component2.GetName() + "</b>";
						}
						else
						{
							uiObjects[37 + j].GetComponent<Text>().text = "<b><color=red>" + component2.GetName() + "</color></b>";
						}
					}
				}
				else
				{
					pS_.tf_platformFocus[j] = -1;
					uiObjects[37 + j].GetComponent<Text>().text = tS_.GetText(1966);
				}
			}
			else
			{
				uiObjects[37 + j].GetComponent<Text>().text = tS_.GetText(1966);
			}
		}
		bool flag = false;
		if (pS_.tf_ownPublisherPriority != -1)
		{
			GameObject gameObject4 = GameObject.Find("PUB_" + pS_.tf_ownPublisherPriority);
			if ((bool)gameObject4)
			{
				publisherScript component3 = gameObject4.GetComponent<publisherScript>();
				if ((bool)component3 && component3.IsMyTochterfirma())
				{
					uiObjects[45].GetComponent<Text>().text = "<b>" + component3.GetName() + "</b>";
					flag = true;
				}
			}
		}
		if (!flag)
		{
			pS_.tf_ownPublisherPriority = -1;
			uiObjects[45].GetComponent<Text>().text = tS_.GetText(1966);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		SaveCopyToggles();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Topic()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[399]);
		guiMain_.uiObjects[399].GetComponent<Menu_Stats_TochterfirmaTopic>().Init(pS_);
	}

	public void BUTTON_IP(int slot)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[400]);
		guiMain_.uiObjects[400].GetComponent<Menu_Stats_TochterfirmaIP>().Init(pS_, slot);
	}

	public void BUTTON_Engine()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[401]);
		guiMain_.uiObjects[401].GetComponent<Menu_Stats_TochterfirmaEngine>().Init(pS_);
	}

	public void BUTTON_Platform(int slot)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[402]);
		guiMain_.uiObjects[402].GetComponent<Menu_Stats_TochterfirmaPlatform>().Init(pS_, slot);
	}

	public void BUTTON_OwnPublisherPriority()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[431]);
		guiMain_.uiObjects[431].GetComponent<Menu_Stats_TochterfirmaOwnPublisher>().Init(pS_);
	}

	public void BUTTON_SettingsForAll()
	{
		sfx_.PlaySound(3, force: true);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if (component.IsMyTochterfirma())
				{
					SetSettings(component, allTochterfirmen: true);
					guiMain_.MessageBox(tS_.GetText(1972), closeMenu: false);
				}
			}
		}
	}

	public void BUTTON_RemoveAll_IP()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < pS_.tf_ipFocus.Length; i++)
		{
			pS_.tf_ipFocus[i] = -1;
		}
		UpdateData();
	}

	public void BUTTON_RemoveAll_Platforms()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < pS_.tf_platformFocus.Length; i++)
		{
			pS_.tf_platformFocus[i] = -1;
		}
		UpdateData();
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		SetSettings(pS_, allTochterfirmen: false);
		SaveCopyToggles();
		if (guiMain_.uiObjects[387].activeSelf)
		{
			guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().UpdateData();
		}
		base.gameObject.SetActive(value: false);
	}

	public void SetSettings(publisherScript script_, bool allTochterfirmen)
	{
		if (!script_)
		{
			return;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[0].isOn))
		{
			if (uiObjects[5].GetComponent<Dropdown>().value == 0 && script_.tf_publisher)
			{
				script_.publisher = true;
				script_.developer = false;
			}
			if (uiObjects[5].GetComponent<Dropdown>().value == 1 && script_.tf_developer)
			{
				script_.publisher = false;
				script_.developer = true;
			}
			if (uiObjects[5].GetComponent<Dropdown>().value == 2 && script_.tf_publisher && script_.tf_developer)
			{
				script_.publisher = true;
				script_.developer = true;
			}
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[1].isOn))
		{
			script_.tf_entwicklungsdauer = uiObjects[6].GetComponent<Dropdown>().value;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[2].isOn))
		{
			script_.tf_gameSize = uiObjects[7].GetComponent<Dropdown>().value;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[3].isOn))
		{
			if (uiObjects[8].GetComponent<Dropdown>().value > 0)
			{
				if (genres_.genres_UNLOCK[uiObjects[8].GetComponent<Dropdown>().value - 1])
				{
					script_.tf_gameGenre = uiObjects[8].GetComponent<Dropdown>().value;
				}
			}
			else
			{
				script_.tf_gameGenre = uiObjects[8].GetComponent<Dropdown>().value;
			}
		}
		if (allTochterfirmen && copyToggles[4].isOn)
		{
			script_.tf_gameTopic = pS_.tf_gameTopic;
		}
		if (allTochterfirmen && copyToggles[5].isOn)
		{
			script_.tf_engine = pS_.tf_engine;
		}
		if (allTochterfirmen && copyToggles[8].isOn)
		{
			for (int i = 0; i < pS_.tf_platformFocus.Length; i++)
			{
				script_.tf_platformFocus[i] = pS_.tf_platformFocus[i];
			}
		}
		if (allTochterfirmen && copyToggles[10].isOn)
		{
			script_.tf_ownPublisherPriority = pS_.tf_ownPublisherPriority;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[6].isOn))
		{
			script_.tf_autoRelease = uiObjects[10].GetComponent<Toggle>().isOn;
			script_.tf_autoReleaseVal = uiObjects[32].GetComponent<Dropdown>().value;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[7].isOn))
		{
			script_.tf_onlyPlayerConsole = uiObjects[11].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[9].isOn))
		{
			script_.tf_ownPublisher = uiObjects[23].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[11].isOn))
		{
			script_.tf_autoGamePass = uiObjects[43].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[12].isOn))
		{
			script_.tf_allowMMO = uiObjects[12].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[13].isOn))
		{
			script_.tf_allowF2P = uiObjects[13].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[14].isOn))
		{
			script_.tf_allowAddon = uiObjects[14].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[15].isOn))
		{
			script_.tf_noArcade = uiObjects[15].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[16].isOn))
		{
			script_.tf_noHandy = uiObjects[16].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[17].isOn))
		{
			script_.tf_noRetro = uiObjects[17].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[18].isOn))
		{
			script_.tf_noRemaster = uiObjects[21].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[19].isOn))
		{
			script_.tf_noSpinoffs = uiObjects[22].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[20].isOn))
		{
			script_.tf_noPorts = uiObjects[18].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[21].isOn))
		{
			script_.tf_noBudget = uiObjects[19].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[22].isOn))
		{
			script_.tf_noGOTY = uiObjects[20].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[23].isOn))
		{
			script_.tf_noBundles = uiObjects[41].GetComponent<Toggle>().isOn;
		}
		if (!allTochterfirmen || (allTochterfirmen && copyToggles[24].isOn))
		{
			script_.tf_noAddonBundles = uiObjects[42].GetComponent<Toggle>().isOn;
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Publisher(script_);
				mS_.mpCalls_.SERVER_Send_PublisherTochterfirmaSettings(script_);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_Publisher(script_);
				mS_.mpCalls_.CLIENT_Send_PublisherTochterfirmaSettings(script_);
			}
		}
	}
}
