using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_GamePass_MostPlayed : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public gameScript game_;

	public genres genres_;

	public Button button_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
	}

	public void SetData()
	{
		if ((bool)game_)
		{
			uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
			uiObjects[3].GetComponent<Image>().sprite = game_.GetTypSprite();
			string text = tS_.GetText(2129);
			text = text.Replace("<NUM>", mS_.GetMoney(game_.gamePassPlayer, showDollar: false));
			uiObjects[2].GetComponent<Text>().text = text;
			if (!game_.isOnMarket)
			{
				GetComponent<Image>().color = guiMain_.colors[26];
			}
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_RemoveFromGamePass()
	{
		sfx_.PlaySound(3, force: true);
		mS_.gpS_.GAMEPASS_RemoveGame(game_, updateGamesAmount: true);
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[46].SetActive(value: true);
		guiMain_.uiObjects[46].GetComponent<Menu_Review>().Init(game_);
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
			if (!button_.enabled)
			{
				button_.enabled = true;
			}
		}
	}
}
