using UnityEngine;
using UnityEngine.UI;

public class Menu_Firmenname : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private int logo = -1;

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
		Init();
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

	public void Init()
	{
		FindScripts();
		uiObjects[0].GetComponent<InputField>().text = mS_.GetCompanyName();
		SetLogo(mS_.GetCompanyLogoID());
	}

	public void BUTTON_Firmenlogo()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[48]);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(814), closeMenu: false);
			return;
		}
		mS_.SetCompanyName(uiObjects[0].GetComponent<InputField>().text);
		mS_.SetCompanyLogoID(logo);
		guiMain_.SetMainGuiData();
		BUTTON_Abbrechen();
	}

	public void SetLogo(int i)
	{
		uiObjects[1].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(i);
		logo = i;
	}
}
