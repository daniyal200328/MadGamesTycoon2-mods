using UnityEngine;
using UnityEngine.UI;

public class Menu_TochterfirmaRename : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private int logo = -1;

	public publisherScript pS_;

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
		if ((bool)main_)
		{
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
			if (!cmS_)
			{
				cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
	}

	private void OnDisable()
	{
		FindScripts();
		if ((bool)cmS_)
		{
			cmS_.disableMovement = false;
		}
	}

	public void Init(publisherScript script_)
	{
		FindScripts();
		pS_ = script_;
		uiObjects[0].GetComponent<InputField>().text = pS_.GetName();
		SetLogo(pS_.logoID);
	}

	public void BUTTON_Firmenlogo()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[392]);
		guiMain_.uiObjects[392].GetComponent<Menu_TochterfirmaLogo>().Init(pS_);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if ((bool)pS_)
		{
			if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
			{
				guiMain_.MessageBox(tS_.GetText(1941), closeMenu: false);
				return;
			}
			pS_.logoID = logo;
			pS_.SetOwnName(uiObjects[0].GetComponent<InputField>().text);
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Publisher(pS_);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_Publisher(pS_);
				}
			}
			if (guiMain_.uiObjects[387].activeSelf)
			{
				guiMain_.uiObjects[387].GetComponent<Menu_Stats_Tochterfirma_Main>().UpdateData();
			}
		}
		BUTTON_Abbrechen();
	}

	public void SetLogo(int i)
	{
		uiObjects[1].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(i);
		logo = i;
	}
}
