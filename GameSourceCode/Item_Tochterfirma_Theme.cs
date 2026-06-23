using UnityEngine;
using UnityEngine.UI;

public class Item_Tochterfirma_Theme : MonoBehaviour
{
	public int myID;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public themes themes_;

	public GUI_Main guiMain_;

	public tooltip tooltip_;

	public publisherScript pS_;

	private void Start()
	{
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = tS_.GetThemes(myID);
		if (themes_.themes_MGSR[myID] != 0)
		{
			uiObjects[1].GetComponent<Image>().sprite = mS_.games_.gamePEGI[themes_.themes_MGSR[myID]];
		}
		if (pS_.tf_gameTopic == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
		}
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		pS_.tf_gameTopic = myID;
		guiMain_.uiObjects[393].GetComponent<Menu_Stats_TochterfirmaSettings>().UpdateData();
		guiMain_.uiObjects[399].GetComponent<Menu_Stats_TochterfirmaTopic>().BUTTON_Close();
	}
}
