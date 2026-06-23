using UnityEngine;
using UnityEngine.UI;

public class Menu_Start : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private settingsScript settings_;

	private cameraMovementScript camMove_;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
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
			if (!settings_)
			{
				settings_ = main_.GetComponent<settingsScript>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!camMove_)
			{
				camMove_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
		}
	}

	private void OnEnable()
	{
		if ((bool)uiObjects[2])
		{
			uiObjects[2].SetActive(value: true);
		}
		FindScripts();
		Init();
	}

	private void OnDisable()
	{
		if ((bool)uiObjects[2])
		{
			uiObjects[2].SetActive(value: false);
		}
		camMove_.disableMovement = false;
		guiMain_.CloseMenu();
		guiMain_.ShowInGameUI(show: true);
	}

	public void Init()
	{
		camMove_.disableMovement = true;
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ShowInGameUI(show: false);
		uiObjects[0].GetComponent<Text>().text = mS_.buildVersion;
		if (PlayerPrefs.HasKey("ResumeGame"))
		{
			uiObjects[1].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[1].GetComponent<Button>().interactable = false;
		}
		uiObjects[3].GetComponent<Dropdown>().value = settings_.language;
	}

	private void Update()
	{
	}

	public void BUTTON_NewGame()
	{
		mS_.multiplayer = false;
		sfx_.PlaySound(3, force: true);
		mS_.LoadContent();
		mS_.myID = 100000;
		mS_.myPubS_ = null;
		guiMain_.ActivateMenu(guiMain_.uiObjects[159]);
	}

	public void BUTTON_Sandbox()
	{
		mS_.multiplayer = false;
		sfx_.PlaySound(3, force: true);
		mS_.LoadContent();
		mS_.myID = 100000;
		mS_.myPubS_ = null;
		guiMain_.ActivateMenu(guiMain_.uiObjects[425]);
	}

	public void BUTTON_Fortsetzen()
	{
		mS_.multiplayer = false;
		sfx_.PlaySound(3, force: true);
		int i = PlayerPrefs.GetInt("ResumeGame");
		guiMain_.uiObjects[150].GetComponent<Menu_LoadGame>().BUTTON_LoadGame(i);
	}

	public void BUTTON_CloseGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[148]);
	}

	public void BUTTON_Credits()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[149]);
	}

	public void BUTTON_LoadGame()
	{
		mS_.multiplayer = false;
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[150]);
	}

	public void BUTTON_Options()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[169]);
	}

	public void BUTTON_Multiplayer()
	{
		Debug.Log("1. BUTTON_Multiplayer()");
		mS_.settings_sandbox = false;
		mS_.multiplayer = true;
		mS_.myID = -1;
		mS_.myPubS_ = null;
		mS_.LoadContent();
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[201]);
		guiMain_.uiObjects[201].GetComponent<mpMain>().BUTTON_StartHost();
	}

	public void BUTTON_MultiplayerJoin()
	{
		Debug.Log("2. BUTTON_MultiplayerJoin()");
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[461]);
	}

	public void BUTTON_OpenMultiplayer()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].SetActive(value: false);
		uiObjects[5].SetActive(value: true);
		uiObjects[7].SetActive(value: false);
	}

	public void BUTTON_Back()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].SetActive(value: true);
		uiObjects[5].SetActive(value: false);
		uiObjects[7].SetActive(value: false);
	}

	public void BUTTON_BackHost()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].SetActive(value: false);
		uiObjects[5].SetActive(value: true);
		uiObjects[7].SetActive(value: false);
	}

	public void BUTTON_OpenMultiplayerHost()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].SetActive(value: false);
		uiObjects[5].SetActive(value: false);
		uiObjects[7].SetActive(value: true);
	}

	public void DROPDOWN_Language()
	{
		settings_.language = uiObjects[3].GetComponent<Dropdown>().value;
		settings_.SaveSettings();
		base.gameObject.SetActive(value: false);
		base.gameObject.SetActive(value: true);
	}
}
