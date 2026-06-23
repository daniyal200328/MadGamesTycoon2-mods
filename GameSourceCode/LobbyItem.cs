using Steamworks;
using UnityEngine;
using UnityEngine.UI;

public class LobbyItem : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public mpLobbyList mpLobbyList_;

	public CSteamID lobbyID;

	public string password;

	public int members;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
	}

	public void BUTTON_Click()
	{
		if (members < 4)
		{
			if (password == null || password.Length <= 0 || mpLobbyList_.uiObjects[4].GetComponent<InputField>().text.Equals(password))
			{
				mpLobbyList_.DisableAllLobbyItems();
				mpLobbyList_.SetRefreshButton(b: false);
				SteamMatchmaking.JoinLobby(lobbyID);
			}
			else
			{
				guiMain_.MessageBox(tS_.GetText(2447), closeMenu: false);
			}
		}
	}
}
