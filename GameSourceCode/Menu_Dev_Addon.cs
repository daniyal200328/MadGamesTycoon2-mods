using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Addon : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

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
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		if ((bool)script_)
		{
			rS_ = script_;
			Unlock(21, uiObjects[1], uiObjects[0]);
			Unlock(22, uiObjects[3], uiObjects[2]);
		}
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			if ((bool)lock_)
			{
				lock_.SetActive(value: false);
			}
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			if ((bool)lock_)
			{
				lock_.SetActive(value: true);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Update()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[105]);
		guiMain_.uiObjects[105].GetComponent<Menu_Dev_UpdateSelectGame>().Init(rS_, 0);
	}

	public void BUTTON_Addon()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[189]);
		guiMain_.uiObjects[189].GetComponent<Menu_Dev_AddonSelectGame>().Init(rS_);
	}

	public void BUTTON_MMOAddon()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[246]);
		guiMain_.uiObjects[246].GetComponent<Menu_Dev_AddonMMOSelectGame>().Init(rS_);
	}

	public void BUTTON_F2PUpdate()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[299]);
		guiMain_.uiObjects[299].GetComponent<Menu_Dev_F2PUpdateSelectGame>().Init(rS_);
	}
}
