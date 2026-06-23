using UnityEngine;
using UnityEngine.UI;

public class Menu_W_UpdateAllObjects : MonoBehaviour
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

	public void Init()
	{
		FindScripts();
		long num = 0L;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i])
			{
				mS_.arrayRoomScripts[i].UpdateInventar(buy: false);
				num += mS_.arrayRoomScripts[i].updateCosts;
			}
		}
		uiObjects[0].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		if (num <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1286), closeMenu: true);
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
		FindScripts();
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && (bool)mS_.arrayRoomScripts[i])
			{
				mS_.arrayRoomScripts[i].UpdateInventar(buy: true);
			}
		}
		BUTTON_Abbrechen();
	}
}
