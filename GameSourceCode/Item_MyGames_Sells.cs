using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_Sells : MonoBehaviour
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
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.sellsTotal, showDollar: false);
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			if (tooltip_.c.Length <= 0)
			{
				tooltip_.c = game_.GetTooltip();
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
