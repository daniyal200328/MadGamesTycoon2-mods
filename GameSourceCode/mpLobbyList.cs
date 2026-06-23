using Mirror;
using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class mpLobbyList : MonoBehaviour
{
	public NetworkManager manager;

	private mpCalls mpCalls_;

	private SteamLobby steamLobby_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private void Awake()
	{
		manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		steamLobby_ = GameObject.Find("NetworkManager").GetComponent<SteamLobby>();
	}

	private void Start()
	{
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
		RefreshLobbyList();
	}

	public void DisableAllLobbyItems()
	{
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			Transform child = uiObjects[3].transform.GetChild(i);
			if ((bool)child)
			{
				child.GetComponent<Button>().interactable = false;
			}
		}
	}

	public void RemoveAllLobbyItems()
	{
		for (int i = 0; i < uiObjects[3].transform.childCount; i++)
		{
			Transform child = uiObjects[3].transform.GetChild(i);
			if ((bool)child)
			{
				Object.Destroy(child.gameObject);
			}
		}
	}

	public void RefreshLobbyList()
	{
		RemoveAllLobbyItems();
		SetRefreshButton(b: false);
		steamLobby_.GetLobbyLists();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void AddLobbyItem(string c, CSteamID lobbyID, int members, string password)
	{
		LobbyItem component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[3].transform).GetComponent<LobbyItem>();
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.sfx_ = sfx_;
		component.guiMain_ = guiMain_;
		component.lobbyID = lobbyID;
		component.mpLobbyList_ = this;
		component.password = password;
		component.members = members;
		component.uiObjects[0].GetComponent<Text>().text = c;
		component.uiObjects[1].GetComponent<Text>().text = members + " / 4";
		if (password.Length <= 0)
		{
			component.uiObjects[2].GetComponent<Text>().text = "";
		}
		else
		{
			component.uiObjects[2].GetComponent<Text>().text = tS_.GetText(2446);
		}
	}

	public void SetRefreshButton(bool b)
	{
		uiObjects[1].GetComponent<Button>().interactable = b;
	}
}
