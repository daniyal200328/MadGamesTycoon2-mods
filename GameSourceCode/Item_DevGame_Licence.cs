using UnityEngine;
using UnityEngine.UI;

public class Item_DevGame_Licence : MonoBehaviour
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

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = licences_.GetName(myID);
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(licences_.GetSellPrice(myID), showDollar: true);
		uiObjects[5].GetComponent<Image>().sprite = licences_.licenceSprites[licences_.licence_TYP[myID]];
		uiObjects[6].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREGOOD[myID]);
		uiObjects[7].GetComponent<Image>().sprite = genres_.GetPic(licences_.licence_GENREBAD[myID]);
		guiMain_.DrawStars(uiObjects[2], Mathf.RoundToInt(licences_.licence_QUALITY[myID] / 20f));
		string text = tS_.GetText(297);
		text = text.Replace("<NUM>", licences_.licence_GEKAUFT[myID].ToString());
		uiObjects[3].GetComponent<Text>().text = text;
		uiObjects[4].GetComponent<Text>().text = licences_.GetTypString(myID);
		tooltip_.c = licences_.GetTooltip(myID);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetLicence(myID);
		guiMain_.uiObjects[63].GetComponent<Menu_GameDev_Licence>().BUTTON_Close();
		guiMain_.uiObjects[64].GetComponent<Menu_DevGame_LicenceName>().uiObjects[0].GetComponent<Text>().text = licences_.GetName(myID);
		guiMain_.ActivateMenu(guiMain_.uiObjects[64]);
	}
}
