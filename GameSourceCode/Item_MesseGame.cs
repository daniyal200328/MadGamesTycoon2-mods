using UnityEngine;
using UnityEngine.UI;

public class Item_MesseGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public int slot;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			if (game_.isOnMarket)
			{
				uiObjects[1].GetComponent<Text>().text = game_.GetReleaseDateString();
			}
			else
			{
				uiObjects[1].GetComponent<Text>().text = tS_.GetText(528);
			}
			uiObjects[2].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(game_.GetHype()).ToString();
			Menu_MesseSelect component = guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>();
			if (component.games[0] == game_ || component.games[1] == game_ || component.games[2] == game_)
			{
				GetComponent<Button>().interactable = false;
			}
			tooltip_.c = game_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>().SetGame(slot, game_);
		guiMain_.uiObjects[187].SetActive(value: false);
	}
}
