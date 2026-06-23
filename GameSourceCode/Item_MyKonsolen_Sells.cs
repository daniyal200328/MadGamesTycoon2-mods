using UnityEngine;
using UnityEngine.UI;

public class Item_MyKonsolen_Sells : MonoBehaviour
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
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(pS_.units, showDollar: false);
			base.gameObject.name = pS_.units.ToString();
		}
	}

	public void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(pS_.units, showDollar: false);
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[333]);
		guiMain_.uiObjects[333].GetComponent<Menu_Umsatz_Konsole>().Init(pS_);
	}
}
