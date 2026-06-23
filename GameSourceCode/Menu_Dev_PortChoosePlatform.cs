using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_PortChoosePlatform : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private roomScript rS_;

	private gameScript gS_;

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
		}
	}

	public void Init(roomScript script_, gameScript game_)
	{
		FindScripts();
		if (!script_ || !game_)
		{
			BUTTON_Abbrechen();
			return;
		}
		rS_ = script_;
		gS_ = game_;
		string text = tS_.GetText(1544);
		text = text.Replace("<NAME>", gS_.GetNameWithTag());
		uiObjects[0].GetComponent<Text>().text = text;
		uiObjects[1].SetActive(value: false);
		uiObjects[2].GetComponent<Button>().interactable = true;
		uiObjects[4].GetComponent<Button>().interactable = true;
		uiObjects[6].GetComponent<Button>().interactable = true;
		uiObjects[8].SetActive(value: false);
		uiObjects[9].SetActive(value: false);
		uiObjects[10].SetActive(value: false);
		Unlock(65, uiObjects[5], uiObjects[4]);
		forschungSonstiges_.Unlock(38, uiObjects[7], uiObjects[6]);
		if (game_.portExist[0])
		{
			uiObjects[2].GetComponent<Button>().interactable = false;
			uiObjects[8].SetActive(value: true);
		}
		if (game_.portExist[1])
		{
			uiObjects[4].GetComponent<Button>().interactable = false;
			uiObjects[9].SetActive(value: true);
		}
		if (game_.portExist[2])
		{
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[10].SetActive(value: true);
		}
		DisableButtons();
		if (game_.gameTyp == 1 || game_.gameTyp == 2)
		{
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[1].SetActive(value: true);
		}
	}

	public void DisableButtons()
	{
		if (gS_.handy)
		{
			uiObjects[4].GetComponent<Button>().interactable = false;
			uiObjects[9].SetActive(value: true);
		}
		else if (gS_.arcade)
		{
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[10].SetActive(value: true);
		}
		else
		{
			uiObjects[2].GetComponent<Button>().interactable = false;
			uiObjects[8].SetActive(value: true);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_PCundKonsole()
	{
		guiMain_.uiObjects[312].SetActive(value: false);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitPort(rS_, gS_.myID, 0);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1063);
	}

	public void BUTTON_Handy()
	{
		guiMain_.uiObjects[312].SetActive(value: false);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitPort(rS_, gS_.myID, 5);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().uiObjects[14].GetComponent<Text>().text = tS_.GetText(1063);
	}

	public void BUTTON_Arcade()
	{
		guiMain_.uiObjects[312].SetActive(value: false);
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitPort(rS_, gS_.myID, 4);
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
