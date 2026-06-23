using UnityEngine;
using UnityEngine.UI;

public class Menu_OverwriteSavegame : MonoBehaviour
{
	private ES3Writer writer;

	private ES3Reader reader;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private int slot;

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

	public void Init(int slot_, string saveGameName)
	{
		slot = slot_;
		uiObjects[0].GetComponent<Text>().text = saveGameName;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		ES3.DeleteFile(mS_.GetSavegameTitle() + slot + ".sav");
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[156].GetComponent<Menu_SaveGame>().BUTTON_SaveGame(slot);
		BUTTON_Abbrechen();
	}
}
