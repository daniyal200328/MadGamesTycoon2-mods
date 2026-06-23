using UnityEngine;
using UnityEngine.UI;

public class Menu_W_BuyDevKit : MonoBehaviour
{
	public GameObject[] uiObjects;

	private platformScript pS_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init(platformScript script_)
	{
		if ((bool)script_)
		{
			pS_ = script_;
			uiObjects[0].GetComponent<Text>().text = pS_.GetName();
			pS_.SetPic(uiObjects[1]);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		pS_.inBesitz = true;
		sfx_.PlaySound(3, force: true);
		BUTTON_Abbrechen();
		mS_.Pay(pS_.GetPrice(), 3);
		if (mS_.multiplayer && !pS_.OwnerIsNPC())
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, pS_.ownerID, 2, pS_.price, pS_.myID);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Payment(pS_.ownerID, 2, pS_.price, pS_.myID);
			}
		}
		guiMain_.uiObjects[33].GetComponent<Menu_BuyDevKit>().TAB_DevKitsBuy(0);
	}
}
