using UnityEngine;
using UnityEngine.UI;

public class Item_Firmenlogo : MonoBehaviour
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
		if (guiMain_.uiObjects[47].activeSelf && mS_.GetCompanyLogoID() == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
			uiObjects[0].GetComponent<Animation>().Play();
		}
		if (guiMain_.uiObjects[159].activeSelf && guiMain_.uiObjects[159].GetComponent<Menu_NewGame>().logo == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
			uiObjects[0].GetComponent<Animation>().Play();
		}
		if (guiMain_.uiObjects[425].activeSelf && guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>().logo == myID)
		{
			GetComponent<Image>().color = guiMain_.colors[4];
			uiObjects[0].GetComponent<Animation>().Play();
		}
		if (guiMain_.uiObjects[201].activeSelf && mS_.GetCompanyLogoID() == myID)
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
		if (guiMain_.uiObjects[47].activeSelf)
		{
			guiMain_.uiObjects[47].GetComponent<Menu_Firmenname>().SetLogo(myID);
			guiMain_.uiObjects[48].SetActive(value: false);
		}
		else if (guiMain_.uiObjects[159].activeSelf)
		{
			guiMain_.uiObjects[159].GetComponent<Menu_NewGame>().SetLogo(myID);
			guiMain_.uiObjects[48].SetActive(value: false);
		}
		else if (guiMain_.uiObjects[425].activeSelf)
		{
			guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>().SetLogo(myID);
			guiMain_.uiObjects[48].SetActive(value: false);
		}
		else if (guiMain_.uiObjects[201].activeSelf)
		{
			guiMain_.uiObjects[201].GetComponent<mpMain>().SetLogo(myID);
			guiMain_.uiObjects[48].SetActive(value: false);
		}
	}
}
