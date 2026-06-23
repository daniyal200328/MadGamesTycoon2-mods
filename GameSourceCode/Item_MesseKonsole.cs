using UnityEngine;
using UnityEngine.UI;

public class Item_MesseKonsole : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public platformScript pS_;

	public int slot;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			if (pS_.isUnlocked)
			{
				uiObjects[1].GetComponent<Text>().text = pS_.GetDateString();
			}
			else
			{
				uiObjects[1].GetComponent<Text>().text = tS_.GetText(528);
			}
			pS_.SetPic(uiObjects[2]);
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(pS_.GetHype()).ToString();
			Menu_MesseSelect component = guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>();
			if (component.konsolen[0] == pS_ || component.konsolen[1] == pS_)
			{
				GetComponent<Button>().interactable = false;
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
		guiMain_.uiObjects[186].GetComponent<Menu_MesseSelect>().SetKonsole(slot, pS_);
		guiMain_.uiObjects[323].SetActive(value: false);
	}
}
