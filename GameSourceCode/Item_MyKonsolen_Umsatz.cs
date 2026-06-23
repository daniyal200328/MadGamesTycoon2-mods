using UnityEngine;
using UnityEngine.UI;

public class Item_MyKonsolen_Umsatz : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	public Menu_Stats_MyKonsolen_Umsatz menu_;

	public platformScript pS_;

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
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			if (tooltip_.c.Length <= 0)
			{
				tooltip_.c = pS_.GetTooltip();
			}
			long num = 0L;
			switch (menu_.uiObjects[4].GetComponent<Dropdown>().value)
			{
			case 0:
				num = pS_.GetGesamtGewinn();
				break;
			case 1:
				num = pS_.umsatzTotal;
				break;
			case 2:
				num = pS_.GetEntwicklungskosten();
				break;
			}
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
			if (num < 0)
			{
				uiObjects[2].GetComponent<Text>().color = guiMain_.colors[5];
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[333]);
		guiMain_.uiObjects[333].GetComponent<Menu_Umsatz_Konsole>().Init(pS_);
	}
}
