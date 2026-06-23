using UnityEngine;
using UnityEngine.UI;

public class Menu_BuildRoom : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	public Color[] colors;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private mapScript mapS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private buildRoomScript buildRoomScript_;

	private roomDataScript rdS_;

	private mainCameraScript mCamS_;

	private sfxScript sfx_;

	private copyRoomScript cpS_;

	public int roomTyp;

	public int autoInventar_Quality = 5;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
			mS_ = main_.GetComponent<mainScript>();
			tS_ = main_.GetComponent<textScript>();
			mapS_ = main_.GetComponent<mapScript>();
			unlock_ = main_.GetComponent<unlockScript>();
			rdS_ = main_.GetComponent<roomDataScript>();
			buildRoomScript_ = main_.GetComponent<buildRoomScript>();
			cpS_ = main_.GetComponent<copyRoomScript>();
			mCamS_ = GameObject.Find("Camera").GetComponent<mainCameraScript>();
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		if (!mS_.settings_TutorialOff)
		{
			guiMain_.SetTutorialStep(5);
		}
	}

	private void OnDisable()
	{
		uiObjects[27].SetActive(value: false);
		uiObjects[29].SetActive(value: false);
	}

	private void Update()
	{
		if (mS_.multiplayer && !guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		Update_DesignRoomMenu();
		if ((bool)guiMain_)
		{
			guiMain_.DrawStarsColor(uiObjects[34], autoInventar_Quality, Color.white);
		}
	}

	private void Update_DesignRoomMenu()
	{
		if (!Input.GetMouseButton(0) && buildRoomScript_.modus == 0 && Input.GetKeyDown(KeyCode.LeftShift))
		{
			BUTTON_RemoveRoom();
		}
		if (buildRoomScript_.modus == 1 && Input.GetKeyUp(KeyCode.LeftShift))
		{
			buildRoomScript_.Remove_DeleteMap();
			BUTTON_SetRoom();
		}
		int num = buildRoomScript_.AmountTiles();
		uiObjects[2].GetComponent<Image>().sprite = rdS_.roomData_SPRITE[roomTyp];
		uiObjects[23].GetComponent<Text>().text = num + " " + tS_.GetText(72);
		uiObjects[24].GetComponent<Text>().text = mS_.GetMoney(rdS_.GetPrice(roomTyp), showDollar: true);
		uiObjects[25].GetComponent<Text>().text = mS_.GetMoney(GetRoomPrice(), showDollar: true);
		uiObjects[26].GetComponent<Text>().text = rdS_.GetName(roomTyp);
		if (!rdS_.KeineMitarbeiter(roomTyp))
		{
			string text = tS_.GetText(1681);
			text = text.Replace("<NUM>", AnzahlArbeitsplaetze().ToString());
			uiObjects[31].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[31].GetComponent<Text>().text = "";
		}
		UpdateSizePanel();
		UpdateAcceptButton();
		UpdateCloseButton();
		if (buildRoomScript_.modus == 0 || buildRoomScript_.modus == 1 || buildRoomScript_.modus == 3)
		{
			if (!uiObjects[0].activeSelf)
			{
				uiObjects[0].SetActive(value: true);
			}
			if (uiObjects[1].activeSelf)
			{
				uiObjects[1].SetActive(value: false);
			}
		}
		else
		{
			if (uiObjects[0].activeSelf)
			{
				uiObjects[0].SetActive(value: false);
			}
			if (!uiObjects[1].activeSelf)
			{
				uiObjects[1].SetActive(value: true);
			}
		}
	}

	public int AnzahlArbeitsplaetze()
	{
		float num = 3.3f;
		switch (roomTyp)
		{
		case 10:
			num = 10f;
			break;
		case 5:
			num = 5f;
			break;
		case 13:
			num = 2.7f;
			break;
		}
		int num2 = Mathf.FloorToInt((float)buildRoomScript_.AmountTiles() / num);
		if (num2 < 0)
		{
			num2 = 0;
		}
		return num2;
	}

	public int GetRoomPrice()
	{
		if (!main_)
		{
			FindScripts();
		}
		int num = buildRoomScript_.AmountTiles();
		int num2 = rdS_.GetPrice(roomTyp) * num;
		num2 -= buildRoomScript_.moneyRedesign;
		if (num2 < 0)
		{
			num2 = 0;
		}
		return num2;
	}

	private void UpdateSizePanel()
	{
		if ((buildRoomScript_.modus == 0 || buildRoomScript_.modus == 1) && !guiMain_.IsMouseOverGUI() && Input.GetMouseButton(0))
		{
			if (!uiObjects[27].activeSelf)
			{
				uiObjects[27].SetActive(value: true);
			}
			Vector2 v = Input.mousePosition;
			v.x += 10f;
			v.y += 10f;
			uiObjects[27].GetComponent<RectTransform>().anchoredPosition = guiMain_.GetAnchoredPosition(v);
			uiObjects[28].GetComponent<Text>().text = Mathf.Abs(buildRoomScript_.roomStartX - buildRoomScript_.posX) + 1 + "x" + (Mathf.Abs(buildRoomScript_.roomStartY - buildRoomScript_.posY) + 1);
		}
		else if (uiObjects[27].activeSelf)
		{
			uiObjects[27].SetActive(value: false);
		}
	}

	private void UpdateCloseButton()
	{
		if (buildRoomScript_.replaceRoomID == -1)
		{
			if (!uiObjects[16].activeSelf)
			{
				uiObjects[16].SetActive(value: true);
			}
			if (!uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: true);
			}
		}
		else
		{
			if (uiObjects[16].activeSelf)
			{
				uiObjects[16].SetActive(value: false);
			}
			if (uiObjects[3].activeSelf)
			{
				uiObjects[3].SetActive(value: false);
			}
		}
	}

	private void UpdateAcceptButton()
	{
		Button component = uiObjects[17].GetComponent<Button>();
		int num = 3;
		if (roomTyp == 5)
		{
			num = 4;
		}
		if (roomTyp == 10)
		{
			num = 5;
		}
		if (roomTyp == 14)
		{
			num = 5;
		}
		if (roomTyp == 9)
		{
			num = 2;
		}
		if (!buildRoomScript_.ExistRoom())
		{
			component.interactable = false;
			uiObjects[18].GetComponent<Image>().color = Color.grey;
		}
		else if (buildRoomScript_.GetBiggestRoomQuad() < (float)num)
		{
			string text = tS_.GetText(75);
			text = text.Replace("<NUM>", num + "x" + num);
			component.interactable = false;
			uiObjects[18].GetComponent<Image>().color = Color.grey;
			if (!uiObjects[29].activeSelf)
			{
				uiObjects[29].SetActive(value: true);
			}
			uiObjects[30].GetComponent<Text>().text = text;
		}
		else if (!buildRoomScript_.IsDoor())
		{
			component.interactable = false;
			uiObjects[18].GetComponent<Image>().color = Color.grey;
			if (!uiObjects[29].activeSelf)
			{
				uiObjects[29].SetActive(value: true);
			}
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(76);
		}
		else if (buildRoomScript_.noPath)
		{
			component.interactable = false;
			uiObjects[18].GetComponent<Image>().color = Color.grey;
			if (!uiObjects[29].activeSelf)
			{
				uiObjects[29].SetActive(value: true);
			}
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(73);
		}
		else
		{
			component.interactable = true;
			uiObjects[18].GetComponent<Image>().color = Color.white;
			if (uiObjects[29].activeSelf)
			{
				uiObjects[29].SetActive(value: false);
			}
		}
	}

	public void BUTTON_AcceptRoomDesign()
	{
		if (!mS_.settings_TutorialOff && roomTyp == 1)
		{
			guiMain_.SetTutorialStep(6);
		}
		sfx_.PlaySound(7, force: true);
		buildRoomScript_.CreateRoom(roomTyp, GetRoomPrice());
		buildRoomScript_.SetInactive();
		buildRoomScript_.modus = 0;
		buildRoomScript_.moneyRedesign = 0;
		Close_DesignRoom();
		guiMain_.TOGGLE_Walls();
		guiMain_.DROPDOWN_BuyInventar(roomTyp);
		for (int i = 0; i < mS_.arrayObjectScripts.Length; i++)
		{
			if ((bool)mS_.arrayObjectScripts[i])
			{
				mS_.arrayObjectScripts[i].WakeUpObject();
			}
		}
	}

	private void ResetButtonColors()
	{
		uiObjects[19].GetComponent<Image>().color = colors[1];
		uiObjects[20].GetComponent<Image>().color = colors[1];
		uiObjects[21].GetComponent<Image>().color = colors[1];
		uiObjects[22].GetComponent<Image>().color = colors[1];
	}

	public void BUTTON_SetRoom()
	{
		sfx_.PlaySound(3, force: true);
		buildRoomScript_.modus = 0;
		ResetButtonColors();
		uiObjects[19].GetComponent<Image>().color = colors[0];
	}

	public void BUTTON_RemoveRoom()
	{
		sfx_.PlaySound(3, force: true);
		buildRoomScript_.modus = 1;
		ResetButtonColors();
		uiObjects[20].GetComponent<Image>().color = colors[0];
	}

	public void BUTTON_SetDoor()
	{
		sfx_.PlaySound(3, force: true);
		buildRoomScript_.modus = 2;
		ResetButtonColors();
		uiObjects[21].GetComponent<Image>().color = colors[0];
	}

	public void BUTTON_SetWindow()
	{
		sfx_.PlaySound(3, force: true);
		buildRoomScript_.modus = 3;
		ResetButtonColors();
		uiObjects[22].GetComponent<Image>().color = colors[0];
	}

	public void BUTTON_Grab()
	{
		buildRoomScript_.modus = 4;
	}

	public void BUTTON_SelectRoom(int i)
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		uiObjects[27].SetActive(value: false);
		uiObjects[29].SetActive(value: false);
		roomTyp = i;
		buildRoomScript_.noPath = true;
		buildRoomScript_.SetActive();
		BUTTON_SetRoom();
		mS_.SetBuildGrid(b: true);
	}

	public void BUTTON_AutoInventar_Quality_Plus()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		autoInventar_Quality++;
		if (autoInventar_Quality > 5)
		{
			autoInventar_Quality = 5;
		}
	}

	public void BUTTON_AutoInventar_Quality_Minus()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		autoInventar_Quality--;
		if (autoInventar_Quality < 1)
		{
			autoInventar_Quality = 1;
		}
	}

	public void Close_DesignRoom()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Room");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				array[i].GetComponent<roomScript>().SetListGameObjectsLayer(0);
			}
		}
		sfx_.PlaySound(3, force: true);
		buildRoomScript_.SetInactive();
		buildRoomScript_.modus = 0;
		StartCoroutine(mS_.UpdatePathfindingNextFrame());
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
		mS_.SetBuildGrid(b: false);
	}
}
