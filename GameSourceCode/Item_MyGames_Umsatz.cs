using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_Umsatz : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public Menu_Stats_MyGames_Umsatz menu_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			if (game_.pubOffer)
			{
				uiObjects[0].GetComponent<Text>().color = guiMain_.colors[23];
			}
			uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			if (tooltip_.c.Length <= 0)
			{
				tooltip_.c = game_.GetTooltip();
			}
			long num = 0L;
			switch (menu_.uiObjects[4].GetComponent<Dropdown>().value)
			{
			case 0:
				num = game_.GetGesamtGewinn();
				break;
			case 1:
				num = game_.umsatzTotal;
				break;
			case 2:
				num = game_.umsatzAbos;
				break;
			case 3:
				num = game_.umsatzInApp;
				break;
			case 4:
				num = game_.GetEntwicklungskosten();
				break;
			}
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
			if (num < 0)
			{
				uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			}
			else
			{
				uiObjects[2].GetComponent<Text>().color = guiMain_.colors[15];
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[91]);
		guiMain_.uiObjects[91].GetComponent<Menu_Game_Umsatz>().Init(game_);
	}
}
