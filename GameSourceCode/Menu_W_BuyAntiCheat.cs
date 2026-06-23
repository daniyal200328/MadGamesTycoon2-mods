using UnityEngine;
using UnityEngine.UI;

public class Menu_W_BuyAntiCheat : MonoBehaviour
{
	public GameObject[] uiObjects;

	private antiCheatScript acS_;

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

	public void Init(antiCheatScript script_)
	{
		FindScripts();
		if (!script_)
		{
			return;
		}
		acS_ = script_;
		uiObjects[0].GetComponent<Text>().text = acS_.GetTooltip();
		if ((bool)guiMain_ && guiMain_.IsGameConceptMenuOpen() != uiObjects[1].activeSelf)
		{
			uiObjects[1].SetActive(guiMain_.IsGameConceptMenuOpen());
		}
		if ((bool)guiMain_)
		{
			bool activeSelf = guiMain_.uiObjects[365].activeSelf;
			if (activeSelf != uiObjects[1].activeSelf)
			{
				uiObjects[1].SetActive(activeSelf);
			}
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		acS_.inBesitz = true;
		mS_.Pay(acS_.GetPrice(), 6);
		guiMain_.uiObjects[234].GetComponent<Menu_BuyAntiCheat>().TAB_AntiCheatBuy(0);
		BUTTON_Abbrechen();
		if (guiMain_.IsGameConceptMenuOpen() && uiObjects[1].GetComponent<Toggle>().isOn)
		{
			guiMain_.uiObjects[234].SetActive(value: false);
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetAntiCheat(acS_.myID);
		}
		if (guiMain_.uiObjects[365].activeSelf && uiObjects[1].GetComponent<Toggle>().isOn)
		{
			guiMain_.uiObjects[234].SetActive(value: false);
			guiMain_.uiObjects[365].GetComponent<Menu_Dev_ChangeCopyProtect>().SetAntiCheat(acS_.myID);
		}
	}
}
