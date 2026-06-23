using UnityEngine;
using UnityEngine.UI;

public class Item_PublisherExklusiv : MonoBehaviour
{
	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public genres genres_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pS_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		uiObjects[1].GetComponent<Image>().sprite = pS_.GetLogo();
		uiObjects[5].GetComponent<Text>().text = mS_.GetMoney(pS_.GetShareExklusiv(), showDollar: true);
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(pS_.GetMoneyExklusiv(), showDollar: true);
		guiMain_.DrawStars(uiObjects[3], Mathf.RoundToInt(pS_.stars / 20f));
		string text = tS_.GetText(1048);
		text = text.Replace("<NUM>", pS_.exklusivLaufzeit.ToString());
		uiObjects[7].GetComponent<Text>().text = text;
		tooltip_.c = pS_.GetTooltip();
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[200].GetComponent<Menu_PublisherExklusiv>().SelectPublisher(pS_);
	}
}
