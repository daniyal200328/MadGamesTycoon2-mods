using System.Collections.Generic;
using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class SteamLobby : MonoBehaviour
{
	[SerializeField]
	private GameObject Menu_Multiplayer_Main;

	protected Callback<LobbyCreated_t> lobbyCreated;

	protected Callback<GameLobbyJoinRequested_t> gameLobbyJoinRequested;

	protected Callback<LobbyEnter_t> lobbyEntered;

	protected Callback<LobbyMatchList_t> LobbyList;

	protected Callback<LobbyDataUpdate_t> LobbyDataUpdated;

	public List<CSteamID> lobbyIDs = new List<CSteamID>();

	private const string HostAdressKey = "HostAdress";

	private const string LobbyNameKey = "LobbyName";

	private const string LobbyPasswordKey = "Password";

	public NetworkManager networkManager;

	public ulong lobbyID;

	public mainScript mS_;

	public GUI_Main guiMain_;

	public GameObject main_;

	public GameObject guiMainGO_;

	public mpCalls mpCalls_;

	private void Start()
	{
		FindScripts();
		lobbyCreated = Callback<LobbyCreated_t>.Create(OnLobbyCreated);
		gameLobbyJoinRequested = Callback<GameLobbyJoinRequested_t>.Create(OnGameLobbyJoinRequested);
		lobbyEntered = Callback<LobbyEnter_t>.Create(OnLobbyEntered);
		LobbyList = Callback<LobbyMatchList_t>.Create(OnGetLobbyList);
		LobbyDataUpdated = Callback<LobbyDataUpdate_t>.Create(OnGetLobbyDataUpdated);
	}

	private void FindScripts()
	{
		if (!guiMain_)
		{
			guiMainGO_ = GameObject.Find("CanvasInGameMenu");
			if ((bool)guiMainGO_)
			{
				guiMain_ = guiMainGO_.GetComponent<GUI_Main>();
			}
		}
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_ && (bool)main_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!Menu_Multiplayer_Main && (bool)guiMain_)
		{
			Menu_Multiplayer_Main = guiMain_.uiObjects[201];
		}
		networkManager = GetComponent<NetworkManager>();
		if (!mpCalls_)
		{
			mpCalls_ = GetComponent<mpCalls>();
		}
	}

	public void HostLobby()
	{
		Debug.Log("3. HostLobby()");
		FindScripts();
		mS_.playerName = SteamFriends.GetPersonaName();
		guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[11].GetComponent<InputField>().text = SteamFriends.GetPersonaName();
		SteamMatchmaking.CreateLobby(ELobbyType.k_ELobbyTypePublic, networkManager.maxConnections);
	}

	public void LockLobby(bool b)
	{
		FindScripts();
		SteamMatchmaking.SetLobbyJoinable(new CSteamID(lobbyID), b);
	}

	public void LeaveLobby()
	{
		FindScripts();
		SteamMatchmaking.LeaveLobby(new CSteamID(lobbyID));
	}

	private void OnLobbyCreated(LobbyCreated_t callback)
	{
		Debug.Log("4. OnLobbyCreated()");
		FindScripts();
		if (callback.m_eResult == EResult.k_EResultOK)
		{
			Menu_Multiplayer_Main.GetComponent<mpMain>().StartHost();
			SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAdress", SteamUser.GetSteamID().ToString());
			SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "LobbyName", SteamFriends.GetPersonaName().ToString());
			SteamMatchmaking.SetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "Password", guiMain_.uiObjects[151].GetComponent<Menu_Start>().uiObjects[6].GetComponent<InputField>().text);
			lobbyID = callback.m_ulSteamIDLobby;
		}
	}

	private void OnGameLobbyJoinRequested(GameLobbyJoinRequested_t callback)
	{
		Debug.Log("SteamLobby.cs -> OnGameLobbyJoinRequested()");
	}

	private void OnLobbyEntered(LobbyEnter_t callback)
	{
		lobbyID = callback.m_ulSteamIDLobby;
		if (guiMain_.uiObjects[461].activeSelf)
		{
			guiMain_.uiObjects[461].GetComponent<mpLobbyList>().BUTTON_Close();
		}
		Debug.Log("OnLobbyEntered() -> SteamLobby.cs");
		if (mpCalls_.isServer)
		{
			Debug.Log("OnLobbyEntered() -> SteamLobby.cs (BREAK)");
			return;
		}
		FindScripts();
		if (!NetworkServer.active)
		{
			mS_.playerName = SteamFriends.GetPersonaName();
			guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[11].GetComponent<InputField>().text = SteamFriends.GetPersonaName();
			mS_.multiplayer = true;
			mS_.myID = -1;
			mpCalls_.ResetInit();
			RemoveContentFromClient();
			mS_.LoadContent_MultiplayerClient();
			guiMain_.ActivateMenu(guiMain_.uiObjects[201]);
			string lobbyData = SteamMatchmaking.GetLobbyData(new CSteamID(callback.m_ulSteamIDLobby), "HostAdress");
			Menu_Multiplayer_Main.GetComponent<mpMain>().StartClient_Steam();
			networkManager.networkAddress = lobbyData;
			networkManager.StartClient();
		}
	}

	public void GetLobbyLists()
	{
		FindScripts();
		if (lobbyIDs.Count > 0)
		{
			lobbyIDs.Clear();
		}
		SteamMatchmaking.AddRequestLobbyListResultCountFilter(60);
		SteamMatchmaking.RequestLobbyList();
		Debug.Log("SteamLobby.cs->GetLobbyLists()");
	}

	private void OnGetLobbyList(LobbyMatchList_t result)
	{
		FindScripts();
		Debug.Log("SteamLobby.cs->OnGetLobbyList()");
		guiMain_.uiObjects[461].GetComponent<mpLobbyList>().SetRefreshButton(b: true);
		for (int i = 0; i < result.m_nLobbiesMatching; i++)
		{
			CSteamID lobbyByIndex = SteamMatchmaking.GetLobbyByIndex(i);
			lobbyIDs.Add(lobbyByIndex);
			SteamMatchmaking.RequestLobbyData(lobbyByIndex);
			Debug.Log("OnGetLobbyList() -> ID:" + lobbyByIndex.ToString());
		}
	}

	private void OnGetLobbyDataUpdated(LobbyDataUpdate_t result)
	{
		FindScripts();
		Debug.Log("SteamLobby.cs->OnGetLobbyDataUpdated()");
		guiMain_.uiObjects[461].GetComponent<mpLobbyList>().RemoveAllLobbyItems();
		for (int i = 0; i < lobbyIDs.Count; i++)
		{
			string lobbyData = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "LobbyName");
			int numLobbyMembers = SteamMatchmaking.GetNumLobbyMembers((CSteamID)lobbyIDs[i].m_SteamID);
			string lobbyData2 = SteamMatchmaking.GetLobbyData((CSteamID)lobbyIDs[i].m_SteamID, "Password");
			guiMain_.uiObjects[461].GetComponent<mpLobbyList>().AddLobbyItem(lobbyData, (CSteamID)lobbyIDs[i].m_SteamID, numLobbyMembers, lobbyData2);
		}
	}

	private void RemoveContentFromClient()
	{
		FindScripts();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				Object.Destroy(array[i]);
			}
		}
		array = GameObject.FindGameObjectsWithTag("AntiCheat");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				Object.Destroy(array[j]);
			}
		}
		array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int k = 0; k < array.Length; k++)
		{
			if ((bool)array[k])
			{
				Object.Destroy(array[k]);
			}
		}
		array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int l = 0; l < array.Length; l++)
		{
			if ((bool)array[l])
			{
				Object.Destroy(array[l]);
			}
		}
		array = GameObject.FindGameObjectsWithTag("Engine");
		for (int m = 0; m < array.Length; m++)
		{
			if ((bool)array[m])
			{
				Object.Destroy(array[m]);
			}
		}
	}
}
