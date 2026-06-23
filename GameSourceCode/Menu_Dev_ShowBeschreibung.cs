using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ShowBeschreibung : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private Menu_DevGame mDevGame_;

	private gameScript gS_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
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
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		if (gS_ == null)
		{
			uiObjects[1].GetComponent<InputField>().text = tS_.GetText(999);
		}
		else if (gS_.beschreibung == null)
		{
			uiObjects[1].GetComponent<InputField>().text = tS_.GetText(999);
		}
		else if (gS_.beschreibung.Length <= 0)
		{
			uiObjects[1].GetComponent<InputField>().text = tS_.GetText(999);
		}
		else
		{
			uiObjects[1].GetComponent<InputField>().text = gS_.beschreibung;
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		BUTTON_Close();
	}
}
