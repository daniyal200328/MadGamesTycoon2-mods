using UnityEngine;
using UnityEngine.UI;

public class Menu_MitarberKuendigt : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private settingsScript settings_;

	private roomScript rS_;

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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
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

	public void Init(characterScript cS_)
	{
		Debug.Log("MITARBEITER KÜNDIGT: " + cS_.myName);
		FindScripts();
		guiMain_.OpenMenu(hideChars: false);
		if ((bool)cS_)
		{
			rS_ = cS_.roomS_;
			sfx_.PlaySound(41, force: false);
			string text = tS_.GetText(509);
			text = text.Replace("<NAME>", cS_.myName);
			uiObjects[0].GetComponent<Text>().text = text;
			cS_.RemoveObjectUsing();
			cS_.Entlassen(eventMitarbeiterMotivation: false);
		}
		else
		{
			BUTTON_Abbrechen();
		}
		if (settings_.hideKuendigungen)
		{
			BUTTON_Abbrechen();
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

	public void BUTTON_JumpToRoom()
	{
		if ((bool)rS_ && (bool)guiMain_ && (bool)guiMain_.camera_)
		{
			guiMain_.camera_.transform.parent.position = new Vector3(rS_.uiPos.x, guiMain_.camera_.transform.parent.position.y, rS_.uiPos.z);
		}
		BUTTON_Abbrechen();
	}
}
