using UnityEngine;
using UnityEngine.UI;

public class Item_MyKonsolen_AllTimeCharts : MonoBehaviour
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
		uiObjects[1].GetComponent<Text>().text = (base.gameObject.transform.GetSiblingIndex() + 1).ToString();
		uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(pS_.units, showDollar: false);
		base.gameObject.name = pS_.units.ToString();
	}

	public void SetData()
	{
		if ((bool)pS_)
		{
			if (pS_.ownerID == mS_.myID)
			{
				GetComponent<Image>().color = guiMain_.colors[4];
			}
			else
			{
				GetComponent<Button>().interactable = false;
				GetComponent<Image>().sprite = null;
			}
			if (mS_.multiplayer && pS_.PlatformFromMitspieler())
			{
				GetComponent<Image>().color = guiMain_.colors[8];
			}
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
