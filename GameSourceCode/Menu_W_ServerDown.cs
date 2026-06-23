using UnityEngine;
using UnityEngine.UI;

public class Menu_W_ServerDown : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

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

	public void Init(roomScript script_)
	{
		FindScripts();
		rS_ = script_;
		if ((bool)rS_)
		{
			if (!rS_.serverDown)
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1240);
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = tS_.GetText(1241);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)rS_)
		{
			rS_.ServerAbschalten(!rS_.serverDown);
		}
		BUTTON_Abbrechen();
	}
}
