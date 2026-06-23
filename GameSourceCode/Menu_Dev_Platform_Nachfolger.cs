using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Platform_Nachfolger : MonoBehaviour
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
		string text = tS_.GetText(1858);
		text = text.Replace("<NAME>", "<color=blue>" + gS_.GetNameWithTag() + "</color>");
		uiObjects[0].GetComponent<Text>().text = text;
		uiObjects[2].GetComponent<Button>().interactable = true;
		uiObjects[4].GetComponent<Button>().interactable = true;
		uiObjects[6].GetComponent<Button>().interactable = true;
		Unlock(65, uiObjects[5], uiObjects[4]);
		forschungSonstiges_.Unlock(38, uiObjects[7], uiObjects[6]);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[97]);
		guiMain_.uiObjects[97].GetComponent<Menu_Dev_NachfolgerSelect>().Init(rS_);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_PCundKonsole()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNachfolger(rS_, gS_.myID, 0);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Handy()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNachfolger(rS_, gS_.myID, 5);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Arcade()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[56]);
		guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().InitNachfolger(rS_, gS_.myID, 4);
		base.gameObject.SetActive(value: false);
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
