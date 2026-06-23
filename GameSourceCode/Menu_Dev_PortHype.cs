using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_PortHype : MonoBehaviour
{
	public GameObject[] uiObjects;

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

	public void Init(gameScript gS_)
	{
		FindScripts();
		if ((bool)gS_)
		{
			GameObject gameObject = GameObject.Find("GAME_" + gS_.portID);
			if ((bool)gameObject)
			{
				gameScript component = gameObject.GetComponent<gameScript>();
				uiObjects[0].GetComponent<Text>().text = "+" + Mathf.RoundToInt(component.GetHype());
				string text = tS_.GetText(1550);
				text = text.Replace("<NAME>", "<color=blue>" + component.GetNameWithTag() + "</color>");
				uiObjects[1].GetComponent<Text>().text = text;
				gS_.hype = component.GetHype();
			}
		}
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
		guiMain_.CloseMenu();
	}

	public void BUTTON_Yes()
	{
		BUTTON_Abbrechen();
	}
}
