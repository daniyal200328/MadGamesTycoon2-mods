using UnityEngine;

public class Menu_DeleteAllSaveGames : MonoBehaviour
{
	private ES3Writer writer;

	private ES3Reader reader;

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

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i <= 40; i++)
		{
			ES3.DeleteFile(mS_.GetSavegameTitle() + i + ".sav");
		}
		if (guiMain_.uiObjects[150].activeSelf)
		{
			guiMain_.uiObjects[150].GetComponent<Menu_LoadGame>().Init();
		}
		if (guiMain_.uiObjects[156].activeSelf)
		{
			guiMain_.uiObjects[156].GetComponent<Menu_SaveGame>().Init();
		}
		BUTTON_Abbrechen();
	}
}
