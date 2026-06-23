using UnityEngine;
using UnityEngine.UI;

public class Item_IpHandel_Sell : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	private void Start()
	{
		SetData();
	}

	public void Update()
	{
		if ((bool)game_)
		{
			if (!game_.ipToSell)
			{
				uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[64];
				GetComponent<Image>().color = Color.white;
			}
			else
			{
				uiObjects[2].GetComponent<Image>().sprite = guiMain_.uiSprites[66];
				GetComponent<Image>().color = guiMain_.colors[29];
			}
			if (game_.ownerID != mS_.myID)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			if (!game_.ipToSell)
			{
				uiObjects[4].GetComponent<Toggle>().isOn = false;
			}
			else
			{
				uiObjects[4].GetComponent<Toggle>().isOn = true;
			}
			uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(2237) + " " + mS_.GetMoney(game_.GetIpWert(), showDollar: true);
			guiMain_.DrawIpBekanntheit(uiObjects[1], game_);
			tooltip_.c = game_.GetTooltipIP();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[356].SetActive(value: true);
		guiMain_.uiObjects[356].GetComponent<Menu_Stats_ShowBestIPs>().Init(game_);
	}

	public void TOGGLE_Sell()
	{
		if (!game_)
		{
			return;
		}
		game_.ipToSell = uiObjects[4].GetComponent<Toggle>().isOn;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_GameIpSell(game_);
			}
			else
			{
				mS_.mpCalls_.CLIENT_Send_GameIpSell(game_);
			}
		}
	}
}
