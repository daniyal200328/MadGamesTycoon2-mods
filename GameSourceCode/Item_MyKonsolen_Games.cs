using UnityEngine;
using UnityEngine.UI;

public class Item_MyKonsolen_Games : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if (mS_.multiplayer)
		{
			uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
			string text = tS_.GetText(297);
			text = text.Replace("<NUM>", mS_.GetMoney(pS_.games, showDollar: false));
			uiObjects[2].GetComponent<Text>().text = text;
			base.gameObject.name = pS_.games.ToString();
		}
	}

	public void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			string text = tS_.GetText(297);
			text = text.Replace("<NUM>", mS_.GetMoney(pS_.games, showDollar: false));
			uiObjects[2].GetComponent<Text>().text = text;
			float sellsBonusForGames = pS_.GetSellsBonusForGames();
			if (sellsBonusForGames < 0f)
			{
				uiObjects[4].GetComponent<Text>().text = "<color=#7D2A32>" + mS_.Round(sellsBonusForGames, 2) + "%</color>";
			}
			else
			{
				uiObjects[4].GetComponent<Text>().text = "<color=#337D29>+" + mS_.Round(sellsBonusForGames, 2) + "%</color>";
			}
			tooltip_.c = pS_.GetTooltip();
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[340].SetActive(value: true);
		guiMain_.uiObjects[340].GetComponent<Menu_ShowKonsoleGames>().Init(pS_);
	}
}
