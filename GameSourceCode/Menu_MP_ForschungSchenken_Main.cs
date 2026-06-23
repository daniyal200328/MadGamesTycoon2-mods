using UnityEngine;

public class Menu_MP_ForschungSchenken_Main : MonoBehaviour
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
	}

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		FindScripts();
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Genres()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(0);
	}

	public void BUTTON_Themen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(1);
	}

	public void BUTTON_EngineFeatures()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(2);
	}

	public void BUTTON_GameplayFeatures()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(3);
	}

	public void BUTTON_Hardware()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(4);
	}

	public void BUTTON_Sonstiges()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(5);
	}

	public void BUTTON_HardwareFeatures()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[266]);
		guiMain_.uiObjects[266].GetComponent<Menu_MP_ForschungSchenken>().Init(6);
	}
}
