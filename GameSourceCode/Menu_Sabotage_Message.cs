using UnityEngine;
using UnityEngine.UI;

public class Menu_Sabotage_Message : MonoBehaviour
{
	public GameObject[] uiObjects;

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

	public void Init(int sabotageAktion)
	{
		FindScripts();
		sfx_.PlaySound(62, force: true);
		string text = tS_.GetText(2300);
		text += "\n\n";
		switch (sabotageAktion)
		{
		case 0:
			text = text + tS_.GetText(2289) + "\n" + tS_.GetText(2282);
			break;
		case 1:
			text = text + tS_.GetText(2290) + "\n" + tS_.GetText(2283);
			break;
		case 2:
			text = text + tS_.GetText(2291) + "\n" + tS_.GetText(2284);
			break;
		case 3:
			text = text + tS_.GetText(2292) + "\n" + tS_.GetText(2285);
			break;
		case 4:
			text = text + tS_.GetText(2293) + "\n" + tS_.GetText(2286);
			break;
		case 5:
			text = text + tS_.GetText(2294) + "\n" + tS_.GetText(2287);
			break;
		}
		uiObjects[0].GetComponent<Text>().text = text;
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		guiMain_.CloseMenu();
		BUTTON_Abbrechen();
	}
}
