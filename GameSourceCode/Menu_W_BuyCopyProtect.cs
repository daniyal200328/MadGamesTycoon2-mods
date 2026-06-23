using UnityEngine;
using UnityEngine.UI;

public class Menu_W_BuyCopyProtect : MonoBehaviour
{
	public GameObject[] uiObjects;

	private copyProtectScript cpS_;

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

	public void Init(copyProtectScript script_)
	{
		FindScripts();
		if (!script_)
		{
			return;
		}
		cpS_ = script_;
		uiObjects[0].GetComponent<Text>().text = cpS_.GetTooltip();
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
		cpS_.inBesitz = true;
		mS_.Pay(cpS_.GetPrice(), 6);
		guiMain_.uiObjects[49].GetComponent<Menu_BuyCopyProtect>().TAB_CopyProtectBuy(0);
		BUTTON_Abbrechen();
		if (guiMain_.IsGameConceptMenuOpen() && uiObjects[1].GetComponent<Toggle>().isOn)
		{
			guiMain_.uiObjects[49].SetActive(value: false);
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetCopyProtect(cpS_.myID);
		}
		if (guiMain_.uiObjects[365].activeSelf && uiObjects[1].GetComponent<Toggle>().isOn)
		{
			guiMain_.uiObjects[49].SetActive(value: false);
			guiMain_.uiObjects[365].GetComponent<Menu_Dev_ChangeCopyProtect>().SetCopyProtect(cpS_.myID);
		}
	}
}
