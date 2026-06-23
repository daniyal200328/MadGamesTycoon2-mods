using System.Collections;
using System.Collections.Generic;
using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class mpMain : MonoBehaviour
{
	public NetworkManager manager;

	private mpCalls mpCalls_;

	public GameObject[] uiObjects;

	private int numPlayerCheck = 1;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private float timerPlayerInfos;

	private void Awake()
	{
		manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
	}

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
		if (!manager)
		{
			manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		SetLogo(0);
		uiObjects[12].GetComponent<InputField>().text = "";
		INPUT_PlayerName();
		INPUT_CompanyName();
		mpCalls_.isServer = false;
		mpCalls_.isClient = false;
		mpCalls_.playersMP.Clear();
		uiObjects[1].SetActive(value: false);
		uiObjects[2].SetActive(value: false);
		uiObjects[3].SetActive(value: true);
		uiObjects[4].SetActive(value: false);
		uiObjects[5].SetActive(value: false);
		UpdateRandomTexts();
	}

	public void InitDropdowns()
	{
		List<string> list = new List<string>();
		for (int i = 0; i < tS_.country_GE.Length; i++)
		{
			list.Add(tS_.GetCountry(i));
		}
		uiObjects[28].GetComponent<Dropdown>().ClearOptions();
		uiObjects[28].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[28].GetComponent<Dropdown>().value = 0;
		list = new List<string>();
		list.Add(tS_.GetText(802));
		list.Add(tS_.GetText(803));
		list.Add(tS_.GetText(804));
		list.Add(tS_.GetText(805));
		list.Add(tS_.GetText(1685));
		list.Add(tS_.GetText(806));
		uiObjects[31].GetComponent<Dropdown>().ClearOptions();
		uiObjects[31].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[31].GetComponent<Dropdown>().value = 2;
		mS_.difficulty = 2;
		list = new List<string>();
		for (int j = 1976; j < 2021; j++)
		{
			if (j == 1976 || j == 1985 || j == 1995 || j == 2005 || j == 2015 || j == 2020)
			{
				list.Add("<b>" + j + "</b>");
			}
			else
			{
				list.Add(j.ToString());
			}
		}
		uiObjects[32].GetComponent<Dropdown>().ClearOptions();
		uiObjects[32].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[32].GetComponent<Dropdown>().value = 0;
		list = new List<string>();
		list.Add(tS_.GetText(1770));
		list.Add(tS_.GetText(1771));
		list.Add(tS_.GetText(2012));
		list.Add(tS_.GetText(1773));
		list.Add(tS_.GetText(2079));
		list.Add(tS_.GetText(2250));
		uiObjects[33].GetComponent<Dropdown>().ClearOptions();
		uiObjects[33].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[33].GetComponent<Dropdown>().value = 0;
		mS_.office = 3;
		list = new List<string>();
		list.Add(tS_.GetText(1335));
		list.Add(tS_.GetText(807));
		list.Add(tS_.GetText(808));
		list.Add(tS_.GetText(809));
		list.Add(tS_.GetText(810));
		uiObjects[44].GetComponent<Dropdown>().ClearOptions();
		uiObjects[44].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[44].GetComponent<Dropdown>().value = 2;
		list = new List<string>();
		list.Add(tS_.GetText(2073));
		list.Add(tS_.GetText(2074));
		list.Add(tS_.GetText(2269));
		uiObjects[55].GetComponent<Dropdown>().ClearOptions();
		uiObjects[55].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[55].GetComponent<Dropdown>().value = 0;
		list = new List<string>();
		list.Add(tS_.GetText(2073));
		list.Add(tS_.GetText(1001));
		list.Add(tS_.GetText(837));
		uiObjects[58].GetComponent<Dropdown>().ClearOptions();
		uiObjects[58].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[58].GetComponent<Dropdown>().value = 0;
		list = new List<string>();
		list.Add(tS_.GetText(2398));
		list.Add(tS_.GetText(2399));
		list.Add(tS_.GetText(2400));
		uiObjects[61].GetComponent<Dropdown>().ClearOptions();
		uiObjects[61].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[61].GetComponent<Dropdown>().value = 0;
		list = new List<string>();
		for (int k = 0; k < genres_.genres_UNLOCK.Length; k++)
		{
			list.Add(genres_.GetName(k));
		}
		uiObjects[46].GetComponent<Dropdown>().ClearOptions();
		uiObjects[46].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[46].GetComponent<Dropdown>().value = 0;
		StartCoroutine(DropdownAfterOneFrame());
	}

	public IEnumerator DropdownAfterOneFrame()
	{
		yield return new WaitForEndOfFrame();
		new List<string>();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		List<string> list = new List<string>();
		list.Add("20");
		list.Add("40");
		list.Add("60");
		list.Add("80");
		if (mpCalls_.isServer)
		{
			list.Add(array.Length.ToString());
		}
		else
		{
			list.Add(tS_.GetText(661));
		}
		uiObjects[56].GetComponent<Dropdown>().ClearOptions();
		uiObjects[56].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[56].GetComponent<Dropdown>().value = 4;
	}

	private void UpdatePlayerInfos()
	{
		timerPlayerInfos += Time.deltaTime;
		if (!(timerPlayerInfos > 1f))
		{
			return;
		}
		Debug.Log("NumPlayer: " + mpCalls_.GetConnectionCount());
		timerPlayerInfos = 0f;
		if (mpCalls_.isClient && NetworkClient.isConnected && !guiMain_.uiObjects[238].activeSelf)
		{
			mpCalls_.CLIENT_Send_PlayerInfos();
		}
		if (mpCalls_.isServer)
		{
			player_mp player_mp2 = mS_.mpCalls_.FindPlayer(mS_.myID);
			if (player_mp2 != null)
			{
				player_mp2.playerName = mS_.playerName;
				player_mp2.ready = true;
			}
			if (mpCalls_.playersMP.Count > 1)
			{
				mpCalls_.SERVER_Send_Difficulty();
				mpCalls_.SERVER_Send_RandomSettings();
				mpCalls_.SERVER_Send_Startjahr();
				mpCalls_.SERVER_Send_Entwicklungsdauer();
				mpCalls_.SERVER_Send_Office();
				mpCalls_.SERVER_Send_Spielgeschwindigkeit();
				mpCalls_.SERVER_Send_AnzahlKonkurrenten();
				mpCalls_.SERVER_Send_Wettbewerb();
			}
		}
		if ((bool)mS_.myPubS_)
		{
			if (mpCalls_.isClient && !guiMain_.uiObjects[238].activeSelf)
			{
				mpCalls_.CLIENT_Send_Publisher(mS_.myPubS_);
			}
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Publisher(mS_.myPubS_);
			}
		}
	}

	private void Update()
	{
		if (NetworkServer.active)
		{
			uiObjects[1].SetActive(value: true);
		}
		if (NetworkClient.isConnected)
		{
			uiObjects[2].SetActive(value: true);
			uiObjects[2].GetComponent<Text>().text = "Client: address=" + manager.networkAddress;
			if (!uiObjects[4].activeSelf)
			{
				uiObjects[5].SetActive(value: false);
				uiObjects[4].SetActive(value: true);
			}
		}
		if (mpCalls_.isClient && uiObjects[4].activeSelf && !NetworkClient.isConnected)
		{
			BUTTON_Close();
			guiMain_.uiObjects[162].SetActive(value: false);
			guiMain_.MessageBox(tS_.GetText(1039), closeMenu: false);
		}
		for (int i = 0; i < 4; i++)
		{
			uiObjects[7 + i].GetComponent<Text>().text = "";
			uiObjects[13 + i].GetComponent<Text>().text = "";
			uiObjects[18 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[22 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			uiObjects[47 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		}
		if (mpCalls_.isClient)
		{
			if (mS_.GetCompanyName().Length <= 0 || mS_.playerName.Length <= 0)
			{
				uiObjects[51].GetComponent<Toggle>().interactable = false;
				uiObjects[51].GetComponent<Toggle>().isOn = false;
			}
			else
			{
				uiObjects[51].GetComponent<Toggle>().interactable = true;
			}
		}
		UpdatePlayerInfos();
		bool flag = false;
		for (int j = 0; j < mpCalls_.playersMP.Count; j++)
		{
			int playerID = mpCalls_.playersMP[j].playerID;
			uiObjects[7 + j].GetComponent<Text>().text = mpCalls_.GetPlayerName(playerID);
			uiObjects[13 + j].GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
			if (mpCalls_.GetLogo(playerID) != -1)
			{
				uiObjects[18 + j].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mpCalls_.GetLogo(playerID));
			}
			else
			{
				uiObjects[18 + j].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			}
			if (mpCalls_.GetCountry(playerID) != -1)
			{
				uiObjects[22 + j].GetComponent<Image>().sprite = guiMain_.flagSprites[mpCalls_.GetCountry(playerID)];
			}
			else
			{
				uiObjects[22 + j].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			}
			if (mpCalls_.GetReady(playerID))
			{
				uiObjects[47 + j].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
				continue;
			}
			flag = true;
			uiObjects[47 + j].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
		}
		if (mpCalls_.isClient)
		{
			uiObjects[31].GetComponent<Dropdown>().interactable = false;
			uiObjects[32].GetComponent<Dropdown>().interactable = false;
			uiObjects[33].GetComponent<Dropdown>().interactable = false;
			uiObjects[36].GetComponent<Toggle>().interactable = false;
			uiObjects[41].GetComponent<Toggle>().interactable = false;
			uiObjects[42].GetComponent<Toggle>().interactable = false;
			uiObjects[43].GetComponent<Toggle>().interactable = false;
			uiObjects[44].GetComponent<Dropdown>().interactable = false;
			uiObjects[45].GetComponent<Toggle>().interactable = false;
			uiObjects[52].GetComponent<Toggle>().interactable = false;
			uiObjects[53].GetComponent<Toggle>().interactable = false;
			uiObjects[54].GetComponent<Toggle>().interactable = false;
			uiObjects[55].GetComponent<Dropdown>().interactable = false;
			uiObjects[56].GetComponent<Dropdown>().interactable = false;
			uiObjects[57].GetComponent<Toggle>().interactable = false;
			uiObjects[58].GetComponent<Dropdown>().interactable = false;
			uiObjects[59].GetComponent<Toggle>().interactable = false;
			uiObjects[60].GetComponent<Toggle>().interactable = false;
			uiObjects[61].GetComponent<Dropdown>().interactable = false;
			uiObjects[62].GetComponent<Toggle>().interactable = false;
			uiObjects[63].GetComponent<Toggle>().interactable = false;
			uiObjects[66].GetComponent<Button>().interactable = false;
			uiObjects[67].GetComponent<Button>().interactable = false;
			uiObjects[68].GetComponent<Button>().interactable = false;
			uiObjects[69].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[31].GetComponent<Dropdown>().interactable = true;
			uiObjects[32].GetComponent<Dropdown>().interactable = true;
			uiObjects[33].GetComponent<Dropdown>().interactable = true;
			uiObjects[36].GetComponent<Toggle>().interactable = true;
			uiObjects[41].GetComponent<Toggle>().interactable = true;
			uiObjects[42].GetComponent<Toggle>().interactable = true;
			uiObjects[43].GetComponent<Toggle>().interactable = true;
			uiObjects[44].GetComponent<Dropdown>().interactable = true;
			uiObjects[45].GetComponent<Toggle>().interactable = true;
			uiObjects[52].GetComponent<Toggle>().interactable = true;
			uiObjects[53].GetComponent<Toggle>().interactable = true;
			uiObjects[54].GetComponent<Toggle>().interactable = true;
			uiObjects[55].GetComponent<Dropdown>().interactable = true;
			uiObjects[56].GetComponent<Dropdown>().interactable = true;
			uiObjects[57].GetComponent<Toggle>().interactable = true;
			uiObjects[58].GetComponent<Dropdown>().interactable = true;
			uiObjects[59].GetComponent<Toggle>().interactable = true;
			uiObjects[60].GetComponent<Toggle>().interactable = true;
			uiObjects[61].GetComponent<Dropdown>().interactable = true;
			uiObjects[62].GetComponent<Toggle>().interactable = true;
			uiObjects[63].GetComponent<Toggle>().interactable = true;
			uiObjects[66].GetComponent<Button>().interactable = true;
			uiObjects[67].GetComponent<Button>().interactable = true;
			uiObjects[68].GetComponent<Button>().interactable = true;
			uiObjects[69].GetComponent<Button>().interactable = true;
		}
		if (mpCalls_.isServer)
		{
			if (mpCalls_.playersMP.Count <= numPlayerCheck || flag)
			{
				uiObjects[17].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[17].GetComponent<Button>().interactable = true;
			}
			if (mpCalls_.playersMP.Count < numPlayerCheck + 3)
			{
				uiObjects[27].GetComponent<Text>().text = tS_.GetText(1026);
			}
			else
			{
				uiObjects[27].GetComponent<Text>().text = tS_.GetText(1030);
			}
		}
		else
		{
			uiObjects[27].GetComponent<Text>().text = tS_.GetText(1031);
		}
		if (mpCalls_.isServer && (mS_.GetCompanyName().Length <= 0 || mS_.playerName.Length <= 0))
		{
			uiObjects[17].GetComponent<Button>().interactable = false;
		}
		if ((bool)mS_.achScript_)
		{
			if (mpCalls_.playersMP.Count >= 2)
			{
				mS_.achScript_.SetAchivement(55);
			}
			if (mpCalls_.playersMP.Count >= 4)
			{
				mS_.achScript_.SetAchivement(56);
			}
		}
	}

	public void StartHost()
	{
		Debug.Log("5. StartHost()");
		FindScripts();
		mS_.mpLobbyOpen = true;
		mpCalls_.SetupServer();
		mpCalls_.isServer = true;
		manager.StartHost();
		uiObjects[3].SetActive(value: false);
		uiObjects[17].SetActive(value: true);
		uiObjects[34].SetActive(value: true);
		uiObjects[51].SetActive(value: false);
		uiObjects[51].GetComponent<Toggle>().isOn = true;
	}

	public void BUTTON_StartHost()
	{
		Debug.Log("2. BUTTON_StartHost()");
		FindScripts();
		sfx_.PlaySound(3, force: true);
		manager.GetComponent<SteamLobby>().HostLobby();
	}

	public void BUTTON_LoadMultiplayerSavegame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[150]);
	}

	public void BUTTON_StartClient()
	{
		sfx_.PlaySound(3, force: true);
		if (mS_.playerName.Length <= 0 || mS_.GetCompanyName().Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1033), closeMenu: false);
			return;
		}
		if (mS_.playerName.Length <= 0)
		{
			mS_.playerName = "<Missing Player Name>";
		}
		if (mS_.GetCompanyName().Length <= 0)
		{
			mS_.SetCompanyName("<Missing Company Name>");
		}
		PlayerPrefs.SetString("PlayerName", uiObjects[11].GetComponent<InputField>().text);
		PlayerPrefs.SetString("CompanyName", uiObjects[12].GetComponent<InputField>().text);
		PlayerPrefs.SetString("MP_IP", uiObjects[0].GetComponent<InputField>().text);
		mpCalls_.SetupClient();
		mS_.myID = -1;
		mpCalls_.isClient = true;
		manager.networkAddress = uiObjects[0].GetComponent<InputField>().text;
		manager.StartClient();
		uiObjects[3].SetActive(value: false);
		uiObjects[5].SetActive(value: true);
		uiObjects[17].SetActive(value: false);
		uiObjects[34].SetActive(value: false);
		uiObjects[51].SetActive(value: true);
		uiObjects[6].GetComponent<Text>().text = "Connecting:\n" + manager.networkAddress;
	}

	public void StartClient_Steam()
	{
		PlayerPrefs.SetString("PlayerName", uiObjects[11].GetComponent<InputField>().text);
		PlayerPrefs.SetString("CompanyName", uiObjects[12].GetComponent<InputField>().text);
		PlayerPrefs.SetString("MP_IP", uiObjects[0].GetComponent<InputField>().text);
		mpCalls_.SetupClient();
		mS_.myID = -1;
		mpCalls_.isClient = true;
		uiObjects[3].SetActive(value: false);
		uiObjects[5].SetActive(value: true);
		uiObjects[17].SetActive(value: false);
		uiObjects[34].SetActive(value: false);
		uiObjects[51].SetActive(value: true);
		uiObjects[6].GetComponent<Text>().text = "Connecting...";
	}

	public void StopNetwork()
	{
		Debug.Log("StopNetwork() -> mpMain.cs");
		FindScripts();
		manager.GetComponent<SteamLobby>().LeaveLobby();
		mS_.myID = -1;
		mS_.myPubS_ = null;
		mpCalls_.isServer = false;
		mpCalls_.isClient = false;
		mpCalls_.playersMP.Clear();
		for (int i = 0; i < 8; i++)
		{
			GameObject gameObject = GameObject.Find("PUB_" + (100000 + i));
			if ((bool)gameObject)
			{
				Object.Destroy(gameObject);
			}
		}
		if (NetworkServer.active && NetworkClient.isConnected)
		{
			manager.StopHost();
			Debug.Log("StopHost()");
		}
		else if (NetworkClient.isConnected)
		{
			manager.StopClient();
			Debug.Log("StopClient()");
		}
		else if (NetworkServer.active)
		{
			manager.StopServer();
			Debug.Log("StopServer()");
		}
		if (!NetworkClient.active)
		{
			manager.StopClient();
			Debug.Log("StopClient()");
		}
	}

	public void BUTTON_Close()
	{
		StopNetwork();
		mS_.multiplayer = false;
		mS_.mpLobbyOpen = false;
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void INPUT_PlayerName()
	{
		mS_.playerName = uiObjects[11].GetComponent<InputField>().text;
	}

	public void INPUT_CompanyName()
	{
		mS_.SetCompanyName(uiObjects[12].GetComponent<InputField>().text);
	}

	public void DROPDOWN_Country()
	{
		mS_.SetCountryID(uiObjects[28].GetComponent<Dropdown>().value);
	}

	public void DROPDOWN_Genre()
	{
		mS_.SetFanGenreID(uiObjects[46].GetComponent<Dropdown>().value);
	}

	public void BUTTON_Firmenlogo()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[48]);
	}

	public void TOGGLE_AutoPause()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_autoPauseForMultiplayer = uiObjects[36].GetComponent<Toggle>().isOn;
			if (uiObjects[36].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(7);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(6);
			}
		}
	}

	public void TOGGLE_AllGamespeed()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_allGamespeed = uiObjects[54].GetComponent<Toggle>().isOn;
			if (uiObjects[54].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(23);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(22);
			}
		}
	}

	public void TOGGLE_TochterfirmaKonsole()
	{
		if (mpCalls_.isServer)
		{
			mS_.sandbox_tochterfirmaKonsole = uiObjects[63].GetComponent<Toggle>().isOn;
			if (uiObjects[63].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(35);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(34);
			}
		}
	}

	public void TOGGLE_RandomGenreCombination()
	{
		if (mpCalls_.isServer)
		{
			if (uiObjects[52].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(19);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(18);
			}
		}
	}

	public void TOGGLE_RandomPlatformSuit()
	{
		if (mpCalls_.isServer)
		{
			if (uiObjects[62].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(33);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(32);
			}
		}
	}

	public void TOGGLE_Sabotage()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_sabotageOff = uiObjects[59].GetComponent<Toggle>().isOn;
			if (uiObjects[59].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(27);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(26);
			}
		}
	}

	public void TOGGLE_Tochterfirma()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_tochterfirmaOff = uiObjects[60].GetComponent<Toggle>().isOn;
			if (uiObjects[60].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(29);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(28);
			}
		}
	}

	public void TOGGLE_RandomGameConcept()
	{
		if (mpCalls_.isServer)
		{
			if (uiObjects[43].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(15);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(14);
			}
		}
	}

	public void TOGGLE_PlatformEnd()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_plattformEnd = uiObjects[57].GetComponent<Toggle>().isOn;
			if (uiObjects[57].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(25);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(24);
			}
		}
	}

	public void TOGGLE_SpeedAnpassen()
	{
		if (mpCalls_.isServer)
		{
			if (uiObjects[45].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(17);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(16);
			}
		}
	}

	public void TOGGLE_NpcClose()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_closeNPCs = uiObjects[53].GetComponent<Toggle>().isOn;
			if (uiObjects[53].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(31);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(30);
			}
		}
	}

	public void SetLogo(int i)
	{
		uiObjects[29].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(i);
		mS_.SetCompanyLogoID(i);
	}

	public void BUTTON_StartGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[162].SetActive(value: true);
		mS_.Multiplayer_LockLobby();
		mpCalls_.SERVER_Send_Command(1);
	}

	public void DROPDOWN_Difficulty()
	{
		mS_.difficulty = uiObjects[31].GetComponent<Dropdown>().value;
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Difficulty();
		}
	}

	public void DROPDOWN_Random()
	{
		mS_.settings_randomEvents = uiObjects[58].GetComponent<Dropdown>().value;
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_RandomSettings();
		}
	}

	public void DROPDOWN_Wettbewerb()
	{
		mS_.settings_competition = uiObjects[61].GetComponent<Dropdown>().value;
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Wettbewerb();
		}
	}

	public void DROPDOWN_Office()
	{
		mS_.office = mS_.GetMapIDfromDropdown(uiObjects[33].GetComponent<Dropdown>().value);
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Office();
		}
	}

	public void DROPDOWN_Startjahr()
	{
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Startjahr();
		}
	}

	public void DROPDOWN_Speed()
	{
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Spielgeschwindigkeit();
		}
	}

	public void DROPDOWN_Entwicklungsdauer()
	{
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Entwicklungsdauer();
		}
	}

	public void DROPDOWN_Konkurrenten()
	{
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_AnzahlKonkurrenten();
		}
	}

	public void TOGGLE_RandomReviews()
	{
		if (mpCalls_.isServer)
		{
			mS_.settings_RandomReviews = uiObjects[41].GetComponent<Toggle>().isOn;
			if (uiObjects[41].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(11);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(10);
			}
			mpCalls_.SERVER_Send_Wettbewerb();
			UpdateRandomTexts();
		}
	}

	public void TOGGLE_RandomPlatformPop()
	{
		if (mpCalls_.isServer)
		{
			if (uiObjects[42].GetComponent<Toggle>().isOn)
			{
				mpCalls_.SERVER_Send_Command(13);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(12);
			}
			mpCalls_.SERVER_Send_Wettbewerb();
			UpdateRandomTexts();
		}
	}

	public void BUTTON_Minus_PlatformPop()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_randomPlattformNum--;
		if (mS_.settings_randomPlattformNum < 1)
		{
			mS_.settings_randomPlattformNum = 1;
		}
		UpdateRandomTexts();
		mpCalls_.SERVER_Send_Wettbewerb();
	}

	public void BUTTON_Plus_PlatformPop()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_randomPlattformNum++;
		if (mS_.settings_randomPlattformNum > 4)
		{
			mS_.settings_randomPlattformNum = 4;
		}
		UpdateRandomTexts();
		mpCalls_.SERVER_Send_Wettbewerb();
	}

	public void BUTTON_Minus_RandomReview()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_RandomReviewsNum--;
		if (mS_.settings_RandomReviewsNum < 1)
		{
			mS_.settings_RandomReviewsNum = 1;
		}
		UpdateRandomTexts();
		mpCalls_.SERVER_Send_Wettbewerb();
	}

	public void BUTTON_Plus_RandomReview()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_RandomReviewsNum++;
		if (mS_.settings_RandomReviewsNum > 4)
		{
			mS_.settings_RandomReviewsNum = 4;
		}
		UpdateRandomTexts();
		mpCalls_.SERVER_Send_Wettbewerb();
	}

	public void UpdateRandomTexts()
	{
		uiObjects[64].GetComponent<Text>().text = "+/- " + mS_.settings_RandomReviewsNum * 3 + "%";
		switch (mS_.settings_randomPlattformNum)
		{
		case 1:
			uiObjects[65].GetComponent<Text>().text = tS_.GetText(1908);
			break;
		case 2:
			uiObjects[65].GetComponent<Text>().text = tS_.GetText(1909);
			break;
		case 3:
			uiObjects[65].GetComponent<Text>().text = tS_.GetText(1910);
			break;
		case 4:
			uiObjects[65].GetComponent<Text>().text = tS_.GetText(2436);
			break;
		}
		if (!uiObjects[41].GetComponent<Toggle>().isOn)
		{
			uiObjects[64].GetComponent<Text>().text = "---";
		}
		if (!uiObjects[42].GetComponent<Toggle>().isOn)
		{
			uiObjects[65].GetComponent<Text>().text = "---";
		}
	}

	public void INPUTFIELD_Chat()
	{
		if (!guiMain_)
		{
			return;
		}
		string text = uiObjects[70].GetComponent<InputField>().text;
		if (text.Length > 0)
		{
			uiObjects[70].GetComponent<InputField>().text = "";
			if (mpCalls_.isServer)
			{
				guiMain_.AddChat(mS_.myID, text);
				mpCalls_.SERVER_Send_Chat(mS_.myID, text);
			}
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Chat(text);
			}
		}
	}
}
