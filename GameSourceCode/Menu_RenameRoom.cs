using UnityEngine;
using UnityEngine.UI;

public class Menu_RenameRoom : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

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
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		cmS_.disableMovement = true;
		if ((bool)script_)
		{
			rS_ = script_;
			uiObjects[0].GetComponent<InputField>().text = rS_.myName;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		cmS_.disableMovement = false;
	}

	public void BUTTON_Yes()
	{
		if ((bool)rS_)
		{
			rS_.myName = uiObjects[0].GetComponent<InputField>().text;
		}
		BUTTON_Abbrechen();
	}
}
