using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGameMain : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(10);
			mS_.SetGameSpeed(0f);
			mS_.PauseGame(p: true);
			mS_.gameSpeedOrg = 0f;
		}
	}

	public void Init(roomScript script_)
	{
		FindScripts();
		if (!script_)
		{
			BUTTON_Abbrechen();
			return;
		}
		rS_ = script_;
		Unlock(26, uiObjects[1], uiObjects[0]);
		Unlock(27, uiObjects[3], uiObjects[2]);
		Unlock(65, uiObjects[5], uiObjects[4]);
		Unlock(66, uiObjects[9], uiObjects[8]);
		Unlock(67, uiObjects[11], uiObjects[10]);
		forschungSonstiges_.Unlock(38, uiObjects[7], uiObjects[6]);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_NewGame()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNewGame(rS_, 0);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(318);
	}

	public void BUTTON_Nachfolger()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[97]);
		guiMain_.uiObjects[97].GetComponent<Menu_Dev_NachfolgerSelect>().Init(rS_);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(319);
	}

	public void BUTTON_Remaster()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[98]);
		guiMain_.uiObjects[98].GetComponent<Menu_Dev_RemasterSelect>().Init(rS_);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(320);
	}

	public void BUTTON_Handy()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNewGame(rS_, 5);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1499);
	}

	public void BUTTON_Arcade()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNewGame(rS_, 4);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1500);
	}

	public void BUTTON_SpinOff()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[310]);
		guiMain_.uiObjects[310].GetComponent<Menu_Dev_SpinoffSelect>().Init(rS_);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1535);
	}

	public void BUTTON_Port()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[312]);
		guiMain_.uiObjects[312].GetComponent<Menu_Dev_PortSelect>().Init(rS_);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1063);
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			lock_.SetActive(value: false);
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			lock_.SetActive(value: true);
		}
	}
}
