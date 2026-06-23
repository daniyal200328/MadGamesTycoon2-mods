using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleMain : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		if (!script_)
		{
			BUTTON_Abbrechen();
		}
		else
		{
			rS_ = script_;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_NewKonsole()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[318]);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().Init(rS_, 1);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().uiObjects[1].GetComponent<Text>().text = tS_.GetText(1583);
	}

	public void BUTTON_NewHandheld()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[318]);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().Init(rS_, 2);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().uiObjects[1].GetComponent<Text>().text = tS_.GetText(1584);
	}

	public void BUTTON_ProKonsole()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[449]);
		guiMain_.uiObjects[449].GetComponent<Menu_Dev_KonsoleProSelect>().Init(rS_);
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().uiObjects[1].GetComponent<Text>().text = tS_.GetText(2319);
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			lock_.SetActive(value: false);
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			lock_.SetActive(value: true);
		}
	}
}
