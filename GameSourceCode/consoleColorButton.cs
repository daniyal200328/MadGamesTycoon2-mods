using UnityEngine;
using UnityEngine.UI;

public class consoleColorButton : MonoBehaviour
{
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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void BUTTON_Click()
	{
		sfx_.PlaySound(3, force: true);
		Menu_Dev_Konsole component = guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>();
		component.consoleColor = new Vector3(GetComponent<Image>().color.r, GetComponent<Image>().color.g, GetComponent<Image>().color.b);
		component.UpdateConsoleColor();
		base.transform.GetChild(0).gameObject.SetActive(value: true);
	}
}
