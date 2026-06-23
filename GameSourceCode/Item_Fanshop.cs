using UnityEngine;
using UnityEngine.UI;

public class Item_Fanshop : MonoBehaviour
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
			if (game_.merchKeinVerkauf)
			{
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[13];
				uiObjects[3].GetComponent<tooltip>().c = tS_.GetText(1853);
			}
			else
			{
				uiObjects[3].GetComponent<Image>().sprite = guiMain_.uiSprites[14];
				uiObjects[3].GetComponent<tooltip>().c = tS_.GetText(1854);
			}
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
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
		guiMain_.uiObjects[367].SetActive(value: true);
		guiMain_.uiObjects[367].GetComponent<Menu_Fanshop>().Init(game_);
	}
}
