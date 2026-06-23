using UnityEngine;
using UnityEngine.UI;

public class Menu_Result_IpVerkauf : MonoBehaviour
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

	private publisherScript pub_;

	private long ipWert;

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
		Debug.Log("Menu_Result_IpVerkauf() " + script_.GetNameWithTag());
		FindScripts();
		game_ = script_;
		pub_ = null;
		if (!game_)
		{
			BUTTON_Abbrechen();
			return;
		}
		if (!games_.IsIpFree(game_, messageBox_: false))
		{
			BUTTON_Abbrechen();
			return;
		}
		ipWert = game_.GetIpWert();
		if (ipWert < 1000)
		{
			ipWert = 1000 + Random.Range(0, 10) * 100;
		}
		else
		{
			ipWert /= 1000L;
			ipWert *= Random.Range(800, 1000);
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && !mS_.arrayPublisherScripts[i].isPlayer && mS_.arrayPublisherScripts[i].isUnlocked && mS_.arrayPublisherScripts[i].developer && !mS_.arrayPublisherScripts[i].onlyMobile && !mS_.arrayPublisherScripts[i].IsTochterfirma() && !mS_.arrayPublisherScripts[i].tf_geschlossen && Random.Range(0, 10) == 1)
			{
				pub_ = mS_.arrayPublisherScripts[i];
				break;
			}
		}
		if (!pub_)
		{
			BUTTON_Abbrechen();
			return;
		}
		sfx_.PlaySound(61);
		string text = tS_.GetText(2245);
		text = text.Replace("<NAME1>", pub_.GetName());
		text = text.Replace("<NAME2>", game_.GetIpName());
		uiObjects[0].GetComponent<Text>().text = text;
		uiObjects[1].GetComponent<Text>().text = pub_.GetName();
		uiObjects[2].GetComponent<Image>().sprite = pub_.GetLogo();
		uiObjects[3].GetComponent<Text>().text = game_.GetIpName();
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(ipWert, showDollar: true);
		guiMain_.DrawIpBekanntheit(uiObjects[5], game_);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)game_ && (bool)pub_ && games_.IsIpFree(game_, messageBox_: false))
		{
			mS_.Earn(ipWert, 1);
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
				component.ownerID = pub_.myID;
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
}
