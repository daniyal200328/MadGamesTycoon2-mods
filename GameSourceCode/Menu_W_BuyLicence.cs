using UnityEngine;
using UnityEngine.UI;

public class Menu_W_BuyLicence : MonoBehaviour
{
	public GameObject[] uiObjects;

	private platformScript pS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private licences licences_;

	public int myID;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init(int id)
	{
		FindScripts();
		myID = id;
		uiObjects[0].GetComponent<Text>().text = licences_.GetTooltip(myID);
		if ((bool)guiMain_ && guiMain_.IsGameConceptMenuOpen() != uiObjects[1].activeSelf)
		{
			uiObjects[1].SetActive(guiMain_.IsGameConceptMenuOpen());
		}
	}

	private void Update()
	{
		if (licences_.licence_ANGEBOT[myID] <= 0)
		{
			BUTTON_Abbrechen();
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		licences_.Buy(myID);
		guiMain_.uiObjects[52].GetComponent<Menu_BuyLicence>().TAB_LicenceBuy(0);
		BUTTON_Abbrechen();
		if (guiMain_.IsGameConceptMenuOpen() && uiObjects[1].GetComponent<Toggle>().isOn)
		{
			guiMain_.uiObjects[51].SetActive(value: false);
			guiMain_.uiObjects[52].SetActive(value: false);
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetLicence(myID);
			guiMain_.uiObjects[64].GetComponent<Menu_DevGame_LicenceName>().uiObjects[0].GetComponent<Text>().text = licences_.GetName(myID);
			guiMain_.ActivateMenu(guiMain_.uiObjects[64]);
		}
	}
}
