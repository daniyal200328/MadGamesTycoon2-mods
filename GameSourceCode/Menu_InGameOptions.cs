using UnityEngine;
using UnityEngine.UI;

public class Menu_InGameOptions : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript camMove_;

	private mpCalls mpCalls_;

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
			if (!camMove_)
			{
				camMove_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
			if (!mpCalls_)
			{
				mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
		if (mS_.multiplayer && mS_.mpCalls_.isServer)
		{
			guiMain_.BUTTON_GameSpeed(0f);
		}
	}

	private void OnDisable()
	{
		camMove_.disableMovement = false;
		guiMain_.CloseMenu();
		guiMain_.ShowInGameUI(show: true);
	}

	private void Update()
	{
		if (base.gameObject.activeSelf && !guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init()
	{
		camMove_.disableMovement = true;
		guiMain_.OpenMenu(hideChars: false);
		guiMain_.ShowInGameUI(show: false);
		uiObjects[0].GetComponent<Text>().text = mS_.buildVersion;
		switch (mS_.difficulty)
		{
		case 0:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(802);
			break;
		}
		case 1:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(803);
			break;
		}
		case 2:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(804);
			break;
		}
		case 3:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(805);
			break;
		}
		case 4:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(1685);
			break;
		}
		case 5:
		{
			Text component = uiObjects[0].GetComponent<Text>();
			component.text = component.text + "\n" + tS_.GetText(798) + ": " + tS_.GetText(806);
			break;
		}
		}
		if (mS_.settings_sandbox)
		{
			Text component2 = uiObjects[0].GetComponent<Text>();
			component2.text = component2.text + " / " + tS_.GetText(2064);
		}
		else
		{
			Text component3 = uiObjects[0].GetComponent<Text>();
			component3.text = component3.text + " / " + tS_.GetText(2063);
		}
		Text component4 = uiObjects[0].GetComponent<Text>();
		component4.text = component4.text + "\n" + tS_.GetText(800) + ": ";
		for (int i = 0; i < mS_.gameSpeeds.Length; i++)
		{
			if (mS_.gameSpeeds[i] == mS_.speedSetting)
			{
				switch (i)
				{
				case 0:
					uiObjects[0].GetComponent<Text>().text += tS_.GetText(1335);
					break;
				case 1:
					uiObjects[0].GetComponent<Text>().text += tS_.GetText(807);
					break;
				case 2:
					uiObjects[0].GetComponent<Text>().text += tS_.GetText(808);
					break;
				case 3:
					uiObjects[0].GetComponent<Text>().text += tS_.GetText(809);
					break;
				case 4:
					uiObjects[0].GetComponent<Text>().text += tS_.GetText(810);
					break;
				}
			}
		}
		if (mS_.multiplayer)
		{
			if (mpCalls_.isClient)
			{
				uiObjects[1].GetComponent<Button>().interactable = false;
			}
			if (mpCalls_.isClient)
			{
				uiObjects[2].GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			uiObjects[1].GetComponent<Button>().interactable = true;
			uiObjects[2].GetComponent<Button>().interactable = true;
		}
	}

	public void BUTTON_ExitToOS()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[148]);
	}

	public void BUTTON_ExitToMainMenu()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[154]);
	}

	public void BUTTON_LoadGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[150]);
	}

	public void BUTTON_SaveGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[156]);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Options()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[169]);
	}

	public void BUTTON_GameSettings()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[414]);
	}
}
