using UnityEngine;
using UnityEngine.UI;

public class Menu_RandomEventBugs : MonoBehaviour
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

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(gameScript gS_)
	{
		Debug.Log("RandomEventBugs()");
		if (!gS_)
		{
			BUTTON_Abbrechen();
		}
		FindScripts();
		sfx_.PlaySound(53, force: true);
		gS_.points_bugs += gS_.points_bugsInvis;
		gS_.points_bugsInvis = 0f;
		string text = tS_.GetText(1760);
		text = text.Replace("<NAME>", gS_.GetNameWithTag());
		uiObjects[0].GetComponent<Text>().text = text;
		if ((bool)mS_.settings_ && mS_.settings_.hideEvents)
		{
			BUTTON_Abbrechen();
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
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
