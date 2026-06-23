using UnityEngine;
using UnityEngine.UI;

public class Item_TochterfirmaIP : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public publisherScript pS_;

	public int slot;

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
		if ((bool)game_)
		{
			if (game_.ownerID == mS_.myID || game_.publisherID == mS_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			if (mS_.multiplayer && game_.GameFromMitspieler())
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
			uiObjects[0].GetComponent<Text>().text = game_.GetIpName();
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
		pS_.tf_ipFocus[slot] = game_.myID;
		guiMain_.uiObjects[393].GetComponent<Menu_Stats_TochterfirmaSettings>().UpdateData();
		guiMain_.uiObjects[400].GetComponent<Menu_Stats_TochterfirmaIP>().BUTTON_Close();
	}
}
