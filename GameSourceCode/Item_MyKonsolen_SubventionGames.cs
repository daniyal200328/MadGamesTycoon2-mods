using UnityEngine;
using UnityEngine.UI;

public class Item_MyKonsolen_SubventionGames : MonoBehaviour
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
		}
	}

	public void SetData()
	{
		if ((bool)pS_)
		{
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			uiObjects[3].GetComponent<Image>().sprite = pS_.GetTypSprite();
			string text = tS_.GetText(297);
			text = text.Replace("<NUM>", mS_.GetMoney(pS_.GetAmountSubventionierteGames(), showDollar: false));
			uiObjects[2].GetComponent<Text>().text = text;
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
		guiMain_.uiObjects[455].SetActive(value: true);
		guiMain_.uiObjects[455].GetComponent<Menu_ShowKonsoleSubventionierteGames>().Init(pS_);
	}
}
