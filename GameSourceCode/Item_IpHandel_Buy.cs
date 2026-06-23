using UnityEngine;
using UnityEngine.UI;

public class Item_IpHandel_Buy : MonoBehaviour
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
		if ((bool)game_ && !game_.ipToSell)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			if (mS_.multiplayer && game_.GameFromMitspieler())
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
			uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
			uiObjects[3].GetComponent<Text>().text = tS_.GetText(2237) + " " + mS_.GetMoney(game_.GetIpWert(), showDollar: true);
			guiMain_.DrawIpBekanntheit(uiObjects[1], game_);
			if (!game_.ownerS_)
			{
				game_.FindMyOwner();
			}
			if ((bool)game_.ownerS_)
			{
				uiObjects[5].GetComponent<Image>().sprite = game_.ownerS_.GetLogo();
			}
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
		guiMain_.uiObjects[439].SetActive(value: true);
		guiMain_.uiObjects[439].GetComponent<Menu_W_IpHandel_Buy>().Init(game_);
	}
}
