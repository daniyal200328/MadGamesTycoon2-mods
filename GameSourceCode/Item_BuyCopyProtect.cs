using UnityEngine;
using UnityEngine.UI;

public class Item_BuyCopyProtect : MonoBehaviour
{
	public copyProtectScript cpS_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	private void Start()
	{
		if (cpS_.inBesitz)
		{
			GetComponent<Button>().interactable = false;
		}
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = cpS_.GetName();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(cpS_.GetPrice(), showDollar: true);
		uiObjects[2].GetComponent<Text>().text = mS_.Round(cpS_.effekt, 2) + "%";
		uiObjects[3].GetComponent<Image>().fillAmount = cpS_.effekt * 0.01f;
		uiObjects[3].GetComponent<Image>().color = GetValColor(cpS_.effekt);
		tooltip_.c = cpS_.GetTooltip();
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[50]);
		guiMain_.uiObjects[50].GetComponent<Menu_W_BuyCopyProtect>().Init(cpS_);
	}
}
