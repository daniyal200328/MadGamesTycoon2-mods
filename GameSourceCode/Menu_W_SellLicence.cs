using UnityEngine;
using UnityEngine.UI;

public class Menu_W_SellLicence : MonoBehaviour
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
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		licences_.Sell(myID);
		guiMain_.uiObjects[54].GetComponent<Menu_SellLicence>().Init();
		BUTTON_Abbrechen();
	}
}
