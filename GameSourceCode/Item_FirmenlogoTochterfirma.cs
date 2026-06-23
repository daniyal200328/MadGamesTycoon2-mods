using UnityEngine;
using UnityEngine.UI;

public class Item_FirmenlogoTochterfirma : MonoBehaviour
{
	public int myID = -1;

	public GameObject[] uiObjects;

	public mainScript mS_;

	public textScript tS_;

	public sfxScript sfx_;

	public GUI_Main guiMain_;

	private void Start()
	{
		SetData();
	}

	public void SetData()
	{
		if (guiMain_.uiObjects[391].GetComponent<Menu_TochterfirmaRename>().pS_.logoID == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
			uiObjects[0].GetComponent<Animation>().Play();
		}
		uiObjects[0].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(myID);
	}

	private void OnDisable()
	{
		Object.Destroy(base.gameObject);
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[391].GetComponent<Menu_TochterfirmaRename>().SetLogo(myID);
		guiMain_.uiObjects[392].SetActive(value: false);
	}
}
