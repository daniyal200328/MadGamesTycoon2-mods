using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Item_FreewareSelect : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler
{
	public gameScript game_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_FreewareSelect menu_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
	}

	private void SetData()
	{
		if (!game_)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = game_.GetNameWithTag();
		uiObjects[3].GetComponent<Text>().text = game_.GetReleaseDateString();
		uiObjects[2].GetComponent<Image>().sprite = game_.GetScreenshot();
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(275) + ": " + mS_.GetMoney(game_.sellsTotal, showDollar: false);
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(277) + ": " + Mathf.RoundToInt(game_.reviewTotal) + "%";
		uiObjects[1].GetComponent<Image>().sprite = genres_.GetPic(game_.maingenre);
		uiObjects[6].GetComponent<Image>().sprite = game_.GetTypSprite();
		uiObjects[7].GetComponent<Text>().text = "+" + mS_.GetMoney(game_.GetFreewareFans(), showDollar: false);
		if (game_.freeware)
		{
			GetComponent<Button>().interactable = false;
		}
		else if (game_.IsZweitvermarktungAufMarkt())
		{
			if (!uiObjects[8].activeSelf)
			{
				uiObjects[8].SetActive(value: true);
			}
			GetComponent<Button>().interactable = false;
			uiObjects[5].GetComponent<Text>().text = "<b><color=#741C1C>" + tS_.GetText(2312) + "</color></b>";
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[447]);
		guiMain_.uiObjects[447].GetComponent<Menu_W_Freeware>().Init(game_);
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
