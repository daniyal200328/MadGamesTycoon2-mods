using UnityEngine;
using UnityEngine.UI;

public class Menu_W_KonsoleFromMarket : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private platformScript pS_;

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

	public void Init(platformScript plat_)
	{
		pS_ = plat_;
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)pS_)
		{
			pS_.RemoveFromMarket();
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Platform(pS_);
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_Platform(pS_);
				}
			}
		}
		guiMain_.uiObjects[331].SetActive(value: false);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
