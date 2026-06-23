using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_MyGames_Review : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
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
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			if (game_.reviewTotal <= 0)
			{
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(2251);
			}
			base.gameObject.name = game_.reviewTotal.ToString();
		}
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
			uiObjects[2].GetComponent<Text>().text = tS_.GetText(1291) + ": " + game_.GetUserReviewPercent() + "%  " + tS_.GetText(1292) + ": " + game_.reviewTotal + "%";
			if (game_.reviewTotal <= 0)
			{
				uiObjects[2].GetComponent<Text>().text = tS_.GetText(2251);
			}
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			if (guiMain_.uiObjects[387].activeSelf && !game_.isOnMarket)
			{
				GetComponent<Image>().color = guiMain_.colors[26];
			}
			if (guiMain_.uiObjects[340].activeSelf && game_.IsMyGame())
			{
				GetComponent<Image>().color = guiMain_.colors[4];
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

	public void OnPointerEnter(PointerEventData eventData)
	{
		if ((bool)game_)
		{
			tooltip_.c = game_.GetTooltip();
			if (!tooltip_.enabled)
			{
				tooltip_.enabled = true;
			}
		}
	}
}
