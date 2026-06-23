using Mirror;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MultiplayerLobbyInGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] price;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private mpCalls mpCalls_;

	private NetworkManager manager;

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
		if (!manager)
		{
			manager = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	public void OnEnable()
	{
		FindScripts();
		if (mS_.multiplayer)
		{
			if (mpCalls_.isServer)
			{
				mS_.SetGameSpeed(0f);
				mpCalls_.SetPlayersUnready();
			}
			if (mpCalls_.isClient)
			{
				mpCalls_.CLIENT_Send_Command(1);
			}
		}
	}

	private void Update()
	{
		for (int i = 0; i < mpCalls_.playersMP.Count; i++)
		{
			int playerID = mpCalls_.playersMP[i].playerID;
			uiObjects[i].GetComponent<Text>().text = mpCalls_.GetPlayerName(playerID);
			uiObjects[4 + i].GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
			if (mpCalls_.GetLogo(playerID) != -1)
			{
				uiObjects[8 + i].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mpCalls_.GetLogo(playerID));
			}
			else
			{
				uiObjects[8 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			}
			if (mpCalls_.GetCountry(playerID) != -1)
			{
				uiObjects[12 + i].GetComponent<Image>().sprite = guiMain_.flagSprites[mpCalls_.GetCountry(playerID)];
			}
			else
			{
				uiObjects[12 + i].GetComponent<Image>().sprite = guiMain_.uiSprites[19];
			}
		}
	}
}
