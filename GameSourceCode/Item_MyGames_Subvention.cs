using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_Subvention : MonoBehaviour
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
		if (mS_.multiplayer)
		{
			base.gameObject.name = game_.subvention.ToString();
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.subvention, showDollar: true);
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			tooltip_.c = game_.GetTooltip();
			if (!game_.isOnMarket && !game_.inDevelopment)
			{
				GetComponent<Image>().color = guiMain_.colors[26];
			}
			if (game_.inDevelopment)
			{
				GetComponent<Button>().interactable = false;
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		if (game_.reviewTotal > 0)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.uiObjects[46].SetActive(value: true);
			guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
		}
	}
}
