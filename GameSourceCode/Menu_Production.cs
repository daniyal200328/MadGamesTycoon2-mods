using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Production : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiShortButtons;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private roomScript rS_;

	private gameScript selectedGame;

	public int[] produktionsmenge;

	private float updateTimer;

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
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
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
		if (!selectedGame.isOnMarket)
		{
			BUTTON_Abbrechen();
			return;
		}
		MultiplayerUpdate();
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(selectedGame.GetProduktionskosten(0) * (float)produktionsmenge[0]), showDollar: true);
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(selectedGame.GetProduktionskosten(1) * (float)produktionsmenge[1]), showDollar: true);
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(selectedGame.GetProduktionskosten(2) * (float)produktionsmenge[2]), showDollar: true);
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

	public void Init(roomScript roomS_, gameScript gS_)
	{
		if (!roomS_ || !gS_)
		{
			BUTTON_Abbrechen();
			return;
		}
		FindScripts();
		rS_ = roomS_;
		selectedGame = gS_;
		if (selectedGame.typ_budget || selectedGame.typ_bundle || selectedGame.typ_goty || selectedGame.typ_addon || selectedGame.typ_mmoaddon || selectedGame.typ_addonStandalone)
		{
			uiObjects[5].GetComponent<Slider>().interactable = false;
			uiObjects[6].GetComponent<Slider>().interactable = false;
			uiObjects[8].GetComponent<InputField>().interactable = false;
			uiObjects[9].GetComponent<InputField>().interactable = false;
			for (int i = 5; i < uiShortButtons.Length; i++)
			{
				uiShortButtons[i].GetComponent<Button>().interactable = false;
			}
			produktionsmenge[1] = 0;
			produktionsmenge[2] = 0;
			SetInputFieldData();
		}
		else
		{
			uiObjects[5].GetComponent<Slider>().interactable = true;
			uiObjects[6].GetComponent<Slider>().interactable = true;
			uiObjects[8].GetComponent<InputField>().interactable = true;
			uiObjects[9].GetComponent<InputField>().interactable = true;
			for (int j = 5; j < uiShortButtons.Length; j++)
			{
				uiShortButtons[j].GetComponent<Button>().interactable = true;
			}
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		SetData();
		SetInputFieldData();
	}

	private void SetData()
	{
		if ((bool)selectedGame)
		{
			uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(selectedGame.lagerbestand[0], showDollar: false);
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(selectedGame.lagerbestand[1], showDollar: false);
			uiObjects[3].GetComponent<Text>().text = mS_.GetMoney(selectedGame.lagerbestand[2], showDollar: false);
		}
	}

	private void SetInputFieldData()
	{
		uiObjects[7].GetComponent<InputField>().text = produktionsmenge[0].ToString();
		uiObjects[8].GetComponent<InputField>().text = produktionsmenge[1].ToString();
		uiObjects[9].GetComponent<InputField>().text = produktionsmenge[2].ToString();
	}

	public void INPUTFIELD_Production(int i)
	{
		if (uiObjects[7 + i].GetComponent<InputField>().text.Length <= 0)
		{
			produktionsmenge[i] = 0;
			return;
		}
		if (int.Parse(uiObjects[7 + i].GetComponent<InputField>().text) < 0)
		{
			uiObjects[7 + i].GetComponent<InputField>().text = "0";
		}
		produktionsmenge[i] = int.Parse(uiObjects[7 + i].GetComponent<InputField>().text);
	}

	public void SLIDER_Production(int i)
	{
		produktionsmenge[i] = Mathf.RoundToInt(uiObjects[4 + i].GetComponent<Slider>().value * 1000f);
		SetInputFieldData();
	}

	public void BUTTON_SetProduction10K(int i)
	{
		sfx_.PlaySound(3, force: true);
		produktionsmenge[i] = 10000;
		SetInputFieldData();
	}

	public void BUTTON_SetProduction50K(int i)
	{
		sfx_.PlaySound(3, force: true);
		produktionsmenge[i] = 50000;
		SetInputFieldData();
	}

	public void BUTTON_SetProduction100K(int i)
	{
		sfx_.PlaySound(3, force: true);
		produktionsmenge[i] = 100000;
		SetInputFieldData();
	}

	public void BUTTON_SetProduction500K(int i)
	{
		sfx_.PlaySound(3, force: true);
		produktionsmenge[i] = 500000;
		SetInputFieldData();
	}

	public void BUTTON_SetProduction1000K(int i)
	{
		sfx_.PlaySound(3, force: true);
		produktionsmenge[i] = 1000000;
		SetInputFieldData();
	}

	private IEnumerator iMinusProduction(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusProduction(i);
		}
	}

	public void BUTTON_MinusProduction(int i)
	{
		if ((!selectedGame.typ_budget && !selectedGame.typ_bundle && !selectedGame.typ_goty && !selectedGame.typ_addon && !selectedGame.typ_mmoaddon && !selectedGame.typ_addonStandalone) || (i != 1 && i != 2))
		{
			sfx_.PlaySound(3, force: true);
			produktionsmenge[i] -= 1000;
			if (produktionsmenge[i] < 0)
			{
				produktionsmenge[i] = 0;
			}
			StartCoroutine(iMinusProduction(i));
			SetInputFieldData();
			uiObjects[4 + i].GetComponent<Slider>().value = produktionsmenge[i] / 1000;
		}
	}

	private IEnumerator iPlusProduction(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusProduction(i);
		}
	}

	public void BUTTON_PlusProduction(int i)
	{
		if ((!selectedGame.typ_budget && !selectedGame.typ_bundle && !selectedGame.typ_goty && !selectedGame.typ_addon && !selectedGame.typ_mmoaddon && !selectedGame.typ_addonStandalone) || (i != 1 && i != 2))
		{
			sfx_.PlaySound(3, force: true);
			produktionsmenge[i] += 1000;
			if (produktionsmenge[i] > 99999999)
			{
				produktionsmenge[i] = 99999999;
			}
			StartCoroutine(iPlusProduction(i));
			SetInputFieldData();
			uiObjects[4 + i].GetComponent<Slider>().value = produktionsmenge[i] / 1000;
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		if (!selectedGame || !rS_)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		if (!uiObjects[13].GetComponent<Toggle>().isOn && produktionsmenge[0] == 0 && produktionsmenge[1] == 0 && produktionsmenge[2] == 0)
		{
			guiMain_.MessageBox(tS_.GetText(1136), closeMenu: false);
			return;
		}
		taskProduction taskProduction2 = guiMain_.AddTask_Production();
		taskProduction2.Init(fromSavegame: false);
		taskProduction2.targetID = selectedGame.myID;
		taskProduction2.automatic = uiObjects[13].GetComponent<Toggle>().isOn;
		taskProduction2.amountStandard = produktionsmenge[0];
		taskProduction2.amountDeluxe = produktionsmenge[1];
		taskProduction2.amountCollectors = produktionsmenge[2];
		taskProduction2.gesamtProduktion = produktionsmenge[0] + produktionsmenge[1] + produktionsmenge[2];
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskProduction2.myID;
		}
		guiMain_.CloseMenu();
		guiMain_.uiObjects[221].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void TOGGLE_Auto()
	{
	}
}
