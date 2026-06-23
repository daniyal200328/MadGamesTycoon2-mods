using UnityEngine;
using UnityEngine.UI;

public class Menu_MessageBoxSave : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private bool closeMenu;

	private int id;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init(int id_, bool close)
	{
		FindScripts();
		guiMain_.OpenMenu(hideChars: false);
		id = id_;
		closeMenu = close;
		uiObjects[1].GetComponent<Toggle>().isOn = false;
		if (PlayerPrefs.GetInt("MessageBoxSave_" + id) == 1)
		{
			BUTTON_Yes();
		}
		switch (id)
		{
		case 0:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1442);
			break;
		case 1:
			uiObjects[0].GetComponent<Text>().text = tS_.GetText(1443);
			break;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if (uiObjects[1].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("MessageBoxSave_" + id, 1);
		}
		if (closeMenu)
		{
			guiMain_.CloseMenu();
		}
		BUTTON_Abbrechen();
	}

	public void BUTTON_No()
	{
		if (uiObjects[1].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("MessageBoxSave_" + id, 1);
		}
		switch (id)
		{
		case 0:
			guiMain_.uiObjects[3].GetComponent<Toggle>().isOn = false;
			break;
		case 1:
			guiMain_.uiObjects[2].GetComponent<Toggle>().isOn = false;
			break;
		}
		if (closeMenu)
		{
			guiMain_.CloseMenu();
		}
		BUTTON_Abbrechen();
	}
}
