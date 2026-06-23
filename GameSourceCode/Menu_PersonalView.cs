using UnityEngine;
using UnityEngine.UI;

public class Menu_PersonalView : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private characterScript cS_;

	private cameraMovementScript cmS_;

	private mapScript mapS_;

	public GameObject[] uiObjects;

	private float updateTimer;

	private void Start()
	{
		FindScripts();
	}

	private void Update()
	{
		if (!cS_)
		{
			BUTTON_Close();
		}
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	public void Init(characterScript charScript_)
	{
		FindScripts();
		cS_ = charScript_;
		mS_.CreateFoto(cS_, null);
		uiObjects[0].GetComponent<InputField>().text = cS_.myName;
		SetData();
	}

	private void SetData()
	{
		if (!cS_)
		{
			return;
		}
		uiObjects[17].GetComponent<Text>().text = tS_.GetText(137 + cS_.beruf);
		guiMain_.SetBalkenEmployee(uiObjects[1], cS_.s_gamedesign, 0, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[2], cS_.s_programmieren, 1, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[3], cS_.s_grafik, 2, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[4], cS_.s_sound, 3, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[5], cS_.s_pr, 4, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[6], cS_.s_gametests, 5, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[7], cS_.s_technik, 6, cS_);
		guiMain_.SetBalkenEmployee(uiObjects[8], cS_.s_forschen, 7, cS_);
		guiMain_.CreatePerkIcons(cS_, uiObjects[9].transform);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(cS_.GetGehalt(), showDollar: true);
		uiObjects[11].GetComponent<Image>().fillAmount = cS_.s_motivation * 0.01f;
		uiObjects[11].GetComponent<Image>().color = GetValColor(cS_.s_motivation);
		uiObjects[12].GetComponent<Text>().text = Mathf.RoundToInt(cS_.s_motivation).ToString();
		uiObjects[13].GetComponent<Text>().text = cS_.GetGroupString("magenta");
		uiObjects[19].SetActive(value: false);
		uiObjects[20].SetActive(value: false);
		uiObjects[21].SetActive(value: false);
		uiObjects[22].SetActive(value: false);
		uiObjects[23].SetActive(value: false);
		if (cS_.krank > 0)
		{
			uiObjects[19].SetActive(value: true);
		}
		int num = Mathf.RoundToInt(cS_.transform.position.x);
		int num2 = Mathf.RoundToInt(cS_.transform.position.z);
		if (mapS_.IsInMapLimit(num, num2))
		{
			if (!cS_.perks[16])
			{
				if (mapS_.mapMuell[num, num2] > 0f)
				{
					uiObjects[22].SetActive(value: true);
				}
				else
				{
					uiObjects[22].SetActive(value: false);
				}
			}
			if (mapS_.mapRoomID[num, num2] != 0)
			{
				if (!cS_.perks[11])
				{
					if (mapS_.mapWaerme[num, num2] <= 0.2f)
					{
						uiObjects[21].SetActive(value: true);
					}
					else
					{
						uiObjects[21].SetActive(value: false);
					}
				}
				if (!cS_.perks[12])
				{
					if (mapS_.mapAusstattung[num, num2] <= 0.2f)
					{
						uiObjects[23].SetActive(value: true);
					}
					else
					{
						uiObjects[23].SetActive(value: false);
					}
				}
				if (!cS_.perks[17])
				{
					if (cS_.IsUeberfuellt())
					{
						uiObjects[20].SetActive(value: true);
					}
					else
					{
						uiObjects[20].SetActive(value: false);
					}
				}
			}
		}
		uiObjects[14].SetActive(value: true);
		uiObjects[15].SetActive(value: true);
		uiObjects[16].SetActive(value: false);
		uiObjects[18].SetActive(value: false);
		if (cS_.perks[0])
		{
			uiObjects[14].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[14].GetComponent<Button>().interactable = true;
		}
		if (guiMain_.uiObjects[196].activeSelf)
		{
			uiObjects[14].SetActive(value: false);
			uiObjects[15].SetActive(value: false);
			uiObjects[16].SetActive(value: true);
		}
		else if (guiMain_.uiObjects[324].activeSelf)
		{
			uiObjects[14].SetActive(value: false);
			uiObjects[15].SetActive(value: false);
			uiObjects[18].SetActive(value: true);
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
		if ((bool)cS_)
		{
			cS_.myName = uiObjects[0].GetComponent<InputField>().text;
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: false);
		base.gameObject.SetActive(value: false);
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
		if (mS_.multiplayer)
		{
			uiObjects[0].GetComponent<InputField>().interactable = !mS_.multiplayer;
		}
	}

	private void OnDisable()
	{
		FindScripts();
		cmS_.disableMovement = false;
	}

	public void BUTTON_Select()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		if (guiMain_.uiObjects[26].activeSelf)
		{
			guiMain_.uiObjects[26].SetActive(value: false);
		}
		if (guiMain_.uiObjects[29].activeSelf)
		{
			guiMain_.uiObjects[29].SetActive(value: false);
		}
		pcS_.PickFromExternObject(cS_.gameObject);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Entlassen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[27]);
		guiMain_.uiObjects[27].GetComponent<Menu_PersonalEntlassen>().AddCharacter(cS_);
	}

	public void BUTTON_LeitenderDesigner()
	{
		sfx_.PlaySound(3, force: true);
		if (guiMain_.uiObjects[56].activeSelf)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetLeitenderDesigner(cS_, manuellSelectet: true);
			guiMain_.uiObjects[196].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[193].activeSelf)
		{
			guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>().SetLeitenderDesigner(cS_, manuellSelectet: true);
			guiMain_.uiObjects[196].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[73].activeSelf)
		{
			guiMain_.uiObjects[73].GetComponent<Menu_Dev_GameEntwicklungsbericht>().SetLeitenderDesigner(cS_, manuellSelectet: true);
			guiMain_.uiObjects[196].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[247].activeSelf)
		{
			guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>().SetLeitenderDesigner(cS_, manuellSelectet: true);
			guiMain_.uiObjects[196].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_LeitenderTechniker()
	{
		sfx_.PlaySound(3, force: true);
		if (guiMain_.uiObjects[318].activeSelf)
		{
			guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().SetLeitenderTechniker(cS_, manuellSelectet: true);
			guiMain_.uiObjects[324].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[325].activeSelf)
		{
			guiMain_.uiObjects[325].GetComponent<Menu_Dev_KonsoleEntwicklungsbericht>().SetLeitenderTechniker(cS_, manuellSelectet: true);
			guiMain_.uiObjects[324].SetActive(value: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
