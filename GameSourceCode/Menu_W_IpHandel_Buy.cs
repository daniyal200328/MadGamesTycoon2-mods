using UnityEngine;
using UnityEngine.UI;

public class Menu_W_IpHandel_Buy : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private games games_;

	private gameScript game_;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript script_)
	{
		FindScripts();
		game_ = script_;
		if (!game_)
		{
			BUTTON_Abbrechen();
			return;
		}
		if (!games_.IsIpFree(game_, messageBox_: false) || !game_.ipToSell)
		{
			BUTTON_Abbrechen();
			return;
		}
		string text = tS_.GetText(2248);
		text = text.Replace("<NAME>", game_.GetIpName());
		uiObjects[0].GetComponent<Text>().text = text;
		uiObjects[1].GetComponent<Text>().text = game_.GetIpName();
		uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(game_.GetIpWert(), showDollar: true);
		guiMain_.DrawIpBekanntheit(uiObjects[2], game_);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)game_ && games_.IsIpFree(game_, messageBox_: false) && game_.ipToSell)
		{
			int num = (int)game_.GetIpWert();
			mS_.Pay(num, 31);
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, game_.ownerID, 5, num, 0);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_Payment(game_.ownerID, 5, num, 0);
				}
			}
			GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				gameScript component = array[i].GetComponent<gameScript>();
				if (!component || component.mainIP != game_.mainIP)
				{
					continue;
				}
				component.ipToSell = false;
				component.ownerS_ = null;
				component.ownerID = mS_.myID;
				if (mS_.multiplayer)
				{
					if (mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_GameOwner(component);
					}
					else
					{
						mS_.mpCalls_.CLIENT_Send_GameOwner(component);
					}
				}
			}
		}
		BUTTON_Abbrechen();
	}

	public void BUTTON_No()
	{
		BUTTON_Abbrechen();
	}

	public void BUTTON_IpDetails()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[356].SetActive(value: true);
		guiMain_.uiObjects[356].GetComponent<Menu_Stats_ShowBestIPs>().Init(game_);
	}
}
