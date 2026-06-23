using UnityEngine;

public class GUI_MainButtons : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	public Color[] uiColors;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private sfxScript sfx_;

	private GUI_Main guiMain_;

	private settingsScript settings_;

	private buildRoomScript buildRoomScript_;

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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!buildRoomScript_)
		{
			buildRoomScript_ = main_.GetComponent<buildRoomScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	private void Update()
	{
		CloseDropdownsWithClick();
		CloseRoomMenuWithClick();
		UpdatePressedButtons();
	}

	private void CloseDropdownsWithClick()
	{
		if (Input.GetMouseButtonDown(0) && !guiMain_.IsMouseOverGUI() && !guiMain_.uiObjects[19].activeSelf && !guiMain_.uiObjects[20].activeSelf && !guiMain_.uiObjects[0].activeSelf && !guiMain_.uiObjects[15].activeSelf && !guiMain_.uiObjects[428].activeSelf && !guiMain_.uiObjects[429].activeSelf && (uiObjects[4].activeSelf || uiObjects[5].activeSelf || uiObjects[7].activeSelf || uiObjects[9].activeSelf || uiObjects[11].activeSelf || uiObjects[13].activeSelf))
		{
			mS_.PauseGame(p: false);
			CloseAllDropdowns();
		}
	}

	private void CloseRoomMenuWithClick()
	{
		if (Input.GetMouseButtonDown(0) && !guiMain_.IsMouseOverGUI() && guiMain_.IsRoomMenuOpen())
		{
			CloseAllDropdowns();
			guiMain_.CloseAllRoomButtons();
			mS_.PauseGame(p: false);
		}
	}

	public void CloseAllDropdowns()
	{
		FindScripts();
		if (uiObjects[4].activeSelf)
		{
			uiObjects[4].SetActive(value: false);
		}
		if (uiObjects[5].activeSelf)
		{
			uiObjects[5].SetActive(value: false);
		}
		if (uiObjects[7].activeSelf)
		{
			uiObjects[7].SetActive(value: false);
		}
		if (uiObjects[9].activeSelf)
		{
			uiObjects[9].SetActive(value: false);
		}
		if (uiObjects[11].activeSelf)
		{
			uiObjects[11].SetActive(value: false);
		}
		if (uiObjects[13].activeSelf)
		{
			uiObjects[13].SetActive(value: false);
		}
	}

	private void UpdatePressedButtons()
	{
		if (uiObjects[4].activeSelf)
		{
			if (!uiObjects[2].activeSelf)
			{
				uiObjects[2].SetActive(value: true);
			}
		}
		else if (uiObjects[2].activeSelf)
		{
			uiObjects[2].SetActive(value: false);
		}
		if (uiObjects[5].activeSelf)
		{
			if (!uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: true);
			}
		}
		else if (uiObjects[3].activeSelf)
		{
			uiObjects[3].SetActive(value: false);
		}
		if (uiObjects[7].activeSelf)
		{
			if (!uiObjects[6].activeSelf)
			{
				uiObjects[6].SetActive(value: true);
			}
		}
		else if (uiObjects[6].activeSelf)
		{
			uiObjects[6].SetActive(value: false);
		}
		if (uiObjects[9].activeSelf)
		{
			if (!uiObjects[8].activeSelf)
			{
				uiObjects[8].SetActive(value: true);
			}
		}
		else if (uiObjects[8].activeSelf)
		{
			uiObjects[8].SetActive(value: false);
		}
		if (uiObjects[13].activeSelf)
		{
			if (!uiObjects[12].activeSelf)
			{
				uiObjects[12].SetActive(value: true);
			}
		}
		else if (uiObjects[12].activeSelf)
		{
			uiObjects[12].SetActive(value: false);
		}
		if (uiObjects[11].activeSelf)
		{
			if (!uiObjects[10].activeSelf)
			{
				uiObjects[10].SetActive(value: true);
			}
		}
		else if (uiObjects[10].activeSelf)
		{
			uiObjects[10].SetActive(value: false);
		}
	}

	private void CheckPause(bool b)
	{
		if (!b)
		{
			mS_.PauseGame(settings_.pauseUI);
		}
		else
		{
			mS_.PauseGame(p: false);
		}
	}

	public void BUTTON_BuildRoom()
	{
		if (mS_.settings_TutorialOff || guiMain_.GetTutorialStep() >= 4)
		{
			sfx_.PlaySound(5, force: true);
			bool activeSelf = uiObjects[4].activeSelf;
			CloseAllDropdowns();
			guiMain_.CloseAllRoomButtons();
			uiObjects[4].SetActive(!activeSelf);
			CheckPause(activeSelf);
		}
	}

	public void BUTTON_BuyInventar()
	{
		sfx_.PlaySound(5, force: true);
		bool activeSelf = uiObjects[5].activeSelf;
		CloseAllDropdowns();
		guiMain_.CloseAllRoomButtons();
		uiObjects[5].SetActive(!activeSelf);
		CheckPause(activeSelf);
	}

	public void BUTTON_Personal()
	{
		sfx_.PlaySound(5, force: true);
		bool activeSelf = uiObjects[7].activeSelf;
		CloseAllDropdowns();
		guiMain_.CloseAllRoomButtons();
		uiObjects[7].SetActive(!activeSelf);
		CheckPause(activeSelf);
	}

	public void BUTTON_Management()
	{
		if (!mS_.multiplayer)
		{
			if ((bool)uiObjects[15])
			{
				Object.Destroy(uiObjects[15]);
				uiObjects[15] = null;
			}
		}
		else if (uiObjects[14].activeSelf)
		{
			uiObjects[14].SetActive(value: false);
		}
		sfx_.PlaySound(5, force: true);
		bool activeSelf = uiObjects[9].activeSelf;
		CloseAllDropdowns();
		guiMain_.CloseAllRoomButtons();
		uiObjects[9].SetActive(!activeSelf);
		CheckPause(activeSelf);
	}

	public void BUTTON_Publishing()
	{
		sfx_.PlaySound(5, force: true);
		bool activeSelf = uiObjects[13].activeSelf;
		CloseAllDropdowns();
		guiMain_.CloseAllRoomButtons();
		uiObjects[13].SetActive(!activeSelf);
		CheckPause(activeSelf);
	}

	public void BUTTON_Statistics()
	{
		sfx_.PlaySound(5, force: true);
		bool activeSelf = uiObjects[11].activeSelf;
		CloseAllDropdowns();
		guiMain_.CloseAllRoomButtons();
		uiObjects[11].SetActive(!activeSelf);
		CheckPause(activeSelf);
	}
}
