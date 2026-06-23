using UnityEngine;
using UnityEngine.UI;

public class Item_MyGames_VertriebeneSpiele : MonoBehaviour
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
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		if (mS_.multiplayer)
		{
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(game_.GetGesamtGewinn(), showDollar: true);
			base.gameObject.name = game_.reviewTotal.ToString();
		}
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			long gesamtGewinn = game_.GetGesamtGewinn();
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(gesamtGewinn, showDollar: true);
			if (gesamtGewinn < 0)
			{
				uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
			}
			else
			{
				uiObjects[2].GetComponent<Text>().color = guiMain_.colors[15];
			}
			if (!game_.devS_)
			{
				game_.FindMyDeveloper();
			}
			if ((bool)game_.devS_)
			{
				uiObjects[3].GetComponent<Image>().sprite = game_.devS_.GetLogo();
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[91]);
		guiMain_.uiObjects[91].GetComponent<Menu_Game_Umsatz>().Init(game_);
	}
}
