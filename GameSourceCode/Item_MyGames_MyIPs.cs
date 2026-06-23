using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_MyIPs : MonoBehaviour
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
			uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
		}
	}

	public void SetData()
	{
		if (!game_)
		{
			return;
		}
		if (!guiMain_.uiObjects[387].activeSelf && !guiMain_.uiObjects[315].activeSelf)
		{
			if (game_.ownerID == mS_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else if (game_.IpFromTochterfirma())
			{
				GetComponent<Image>().color = guiMain_.colors[27];
			}
		}
		if (mS_.multiplayer && game_.GameFromMitspieler())
		{
			GetComponent<Image>().color = guiMain_.colors[8];
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
		guiMain_.DrawIpBekanntheit(uiObjects[1], game_);
		float num = game_.GetIpWert();
		uiObjects[2].GetComponent<Text>().text = "$" + mS_.Round(num / 1000000f, 2) + " " + tS_.GetText(1483);
		tooltip_.c = game_.GetTooltipIP();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		if (guiMain_.uiObjects[315].activeSelf)
		{
			guiMain_.uiObjects[316].SetActive(value: true);
			guiMain_.uiObjects[316].GetComponent<Menu_Stats_ShowMyIPs>().Init(game_);
		}
		else if (guiMain_.uiObjects[355].activeSelf || guiMain_.uiObjects[361].activeSelf)
		{
			guiMain_.uiObjects[356].SetActive(value: true);
			guiMain_.uiObjects[356].GetComponent<Menu_Stats_ShowBestIPs>().Init(game_);
		}
	}
}
