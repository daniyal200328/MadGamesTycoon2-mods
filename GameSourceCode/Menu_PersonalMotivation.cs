using UnityEngine;

public class Menu_PersonalMotivation : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private bool closeMenu;

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

	public void Init()
	{
		FindScripts();
		closeMenu = false;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component)
				{
					component.AddMotivation(-10f);
				}
			}
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
			closeMenu = true;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (closeMenu)
		{
			guiMain_.CloseMenu();
		}
	}

	public void BUTTON_Yes()
	{
		BUTTON_Abbrechen();
	}
}
