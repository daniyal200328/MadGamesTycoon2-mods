using UnityEngine;
using UnityEngine.UI;

public class Item_BuyLicence : MonoBehaviour
{
	public int myID = -1;

	public licences licences_;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public genres genres_;

	private void Start()
	{
		SetData();
	}

	private void Update()
	{
		if (licences_.licence_ANGEBOT[myID] <= 0 && licences_.licence_GEKAUFT[myID] <= 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = licences_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(licences_.GetPrice(myID), showDollar: true);
		uiObjects[5].GetComponent<Image>().sprite = licences_.licenceSprites[licences_.licence_TYP[myID]];
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREGOOD[myID]);
		uiObjects[7].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREBAD[myID]);
		guiMain_.DrawStars(uiObjects[2], Mathf.RoundToInt(licences_.licence_QUALITY[myID] / 20f));
		uiObjects[4].GetComponent<Text>().text = licences_.GetTypString(myID);
		tooltip_.c = licences_.GetTooltip(myID);
		if (licences_.licence_GEKAUFT[myID] > 0)
		{
			GetComponent<Button>().interactable = false;
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(307);
			string text = tS_.GetText(297);
			text = text.Replace("<NUM>", licences_.licence_GEKAUFT[myID].ToString());
			uiObjects[3].GetComponent<Text>().text = text;
		}
		else
		{
			string text2 = tS_.GetText(297);
			text2 = text2.Replace("<NUM>", licences_.licence_ANGEBOT[myID].ToString());
			uiObjects[3].GetComponent<Text>().text = text2;
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[53]);
		guiMain_.uiObjects[53].GetComponent<Menu_W_BuyLicence>().Init(myID);
	}
}
