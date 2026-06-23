using UnityEngine;
using UnityEngine.UI;

public class Menu_PickCharacter : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private characterScript charS_;

	private pickCharacterScript pcS_;

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
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
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

	private void Update()
	{
		if (!Input.GetKey(KeyCode.LeftShift))
		{
			if (Input.GetKeyUp(KeyCode.F1))
			{
				SetAllToGroup(1);
			}
			if (Input.GetKeyUp(KeyCode.F2))
			{
				SetAllToGroup(2);
			}
			if (Input.GetKeyUp(KeyCode.F3))
			{
				SetAllToGroup(3);
			}
			if (Input.GetKeyUp(KeyCode.F4))
			{
				SetAllToGroup(4);
			}
			if (Input.GetKeyUp(KeyCode.F5))
			{
				SetAllToGroup(5);
			}
			if (Input.GetKeyUp(KeyCode.F6))
			{
				SetAllToGroup(6);
			}
			if (Input.GetKeyUp(KeyCode.F7))
			{
				SetAllToGroup(7);
			}
			if (Input.GetKeyUp(KeyCode.F8))
			{
				SetAllToGroup(8);
			}
			if (Input.GetKeyUp(KeyCode.F9))
			{
				SetAllToGroup(9);
			}
			if (Input.GetKeyUp(KeyCode.F10))
			{
				SetAllToGroup(10);
			}
			if (Input.GetKeyUp(KeyCode.F11))
			{
				SetAllToGroup(11);
			}
			if (Input.GetKeyUp(KeyCode.F12))
			{
				SetAllToGroup(12);
			}
		}
		for (int i = 0; i < mS_.pickedChars.Count; i++)
		{
			if (!mS_.pickedChars[i])
			{
				mS_.pickedChars.RemoveAt(i);
				break;
			}
		}
	}

	private void OnDisable()
	{
		FindScripts();
		guiMain_.DeselectInputField();
	}

	private void OnEnable()
	{
		FindScripts();
		UpdateData();
		if (mS_.multiplayer)
		{
			uiObjects[0].GetComponent<InputField>().interactable = !mS_.multiplayer;
		}
	}

	private void SetAllToGroup(int g)
	{
		if (mS_.pickedChars.Count <= 0)
		{
			return;
		}
		sfx_.PlaySound(30, force: false);
		for (int i = 0; i < mS_.pickedChars.Count; i++)
		{
			if ((bool)mS_.pickedChars[i])
			{
				mS_.pickedChars[i].GetComponent<characterScript>().group = g;
			}
		}
	}

	public void UpdateData()
	{
		if (mS_.pickedChars.Count > 0 && (bool)mS_.pickedChars[0])
		{
			characterScript characterScript2 = (charS_ = mS_.pickedChars[0].GetComponent<characterScript>());
			mS_.CreateFoto(characterScript2, null);
			uiObjects[15].GetComponent<Animation>().Play();
			uiObjects[0].GetComponent<InputField>().text = characterScript2.myName;
			uiObjects[16].GetComponent<Text>().text = tS_.GetText(137 + characterScript2.beruf);
			uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(characterScript2.GetGehalt(), showDollar: true);
			guiMain_.SetBalkenEmployee(uiObjects[1], characterScript2.s_gamedesign, 0, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[2], characterScript2.s_programmieren, 1, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[3], characterScript2.s_grafik, 2, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[4], characterScript2.s_sound, 3, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[5], characterScript2.s_pr, 4, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[6], characterScript2.s_gametests, 5, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[7], characterScript2.s_technik, 6, charS_);
			guiMain_.SetBalkenEmployee(uiObjects[8], characterScript2.s_forschen, 7, charS_);
			guiMain_.CreatePerkIcons(characterScript2, uiObjects[10].transform);
			uiObjects[12].GetComponent<Image>().fillAmount = characterScript2.s_motivation * 0.01f;
			uiObjects[13].GetComponent<Text>().text = mS_.Round(characterScript2.s_motivation, 0).ToString();
			uiObjects[12].GetComponent<Image>().color = GetValColor(characterScript2.s_motivation);
			if (characterScript2.perks[0])
			{
				uiObjects[14].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[14].GetComponent<Button>().interactable = true;
			}
		}
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	public void INPUTFIELD_Name()
	{
		if (mS_.pickedChars.Count > 0)
		{
			characterScript component = mS_.pickedChars[0].GetComponent<characterScript>();
			if (component.myName != uiObjects[0].GetComponent<InputField>().text)
			{
				cmS_.disableMovement = true;
			}
			component.myName = uiObjects[0].GetComponent<InputField>().text;
		}
	}

	public void INPUTFIELD_NameEnd()
	{
		cmS_.disableMovement = false;
	}

	public void BUTTON_Entlassen()
	{
		if (mS_.pickedChars.Count > 0 && (bool)mS_.pickedChars[0])
		{
			mS_.pickedChars[0].GetComponent<characterScript>().Entlassen(eventMitarbeiterMotivation: true);
			mS_.pickedChars.RemoveAt(0);
			sfx_.PlaySound(3, force: true);
			if (mS_.pickedChars.Count == 0)
			{
				guiMain_.DeactivateMenu(guiMain_.uiObjects[15]);
				guiMain_.CloseMenu();
			}
		}
	}

	public void AddCharToList(characterScript cS_)
	{
		FindScripts();
		Item_CharList component = Object.Instantiate(uiPrefabs[0], uiObjects[11].transform).GetComponent<Item_CharList>();
		component.mS_ = mS_;
		component.guiMain_ = guiMain_;
		component.cS_ = cS_;
		component.BUTTON_Click();
	}

	public void BUTTON_Abbrechen()
	{
		pcS_.ESC_DropChar();
	}
}
