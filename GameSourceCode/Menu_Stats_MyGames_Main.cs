using UnityEngine;

public class Menu_Stats_MyGames_Main : MonoBehaviour
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

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Reviews()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[124]);
	}

	public void BUTTON_Sells()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[231]);
	}

	public void BUTTON_SellsHandy()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[302]);
	}

	public void BUTTON_SellsArcade()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[306]);
	}

	public void BUTTON_F2PDownloads()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[357]);
	}

	public void BUTTON_Umsatz()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[125]);
	}

	public void BUTTON_Konzepte()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[109]);
	}

	public void BUTTON_MyIPs()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[315]);
	}

	public void BUTTON_Fanbriefe()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[111]);
	}

	public void BUTTON_Spielberichte()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[182]);
		guiMain_.uiObjects[182].GetComponent<Menu_QA_ShowSpielberichtSelectGame>().Init();
	}

	public void BUTTON_Bundles()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[270]);
	}

	public void BUTTON_Auftragsspiele()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[287]);
	}

	public void BUTTON_VertriebeneSpiele()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[351]);
	}

	public void BUTTON_Tochterfirmen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[394]);
	}
}
