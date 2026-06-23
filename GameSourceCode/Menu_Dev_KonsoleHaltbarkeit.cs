using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleHaltbarkeit : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiButtons;

	public float[] preise;

	public float[] workPoints;

	public float[] haltbarkeitProKampagne;

	public Sprite[] sprites;

	private bool[] buttonAdds = new bool[9];

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private roomScript rS_;

	private platformScript selectedKonsole;

	private float updateTimer;

	private bool allAdds;

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

	public void Init(roomScript roomS_)
	{
		FindScripts();
		rS_ = roomS_;
		selectedKonsole = FindKonsole();
		DeselectAllButtons();
		SetData();
	}

	private void DeselectAllButtons()
	{
		allAdds = false;
		for (int i = 0; i < uiButtons.Length; i++)
		{
			buttonAdds[i] = false;
			uiButtons[i].GetComponent<Button>().interactable = false;
			uiObjects[58 + i].SetActive(value: false);
			uiObjects[67 + i].SetActive(value: false);
		}
	}

	private void SetData()
	{
		uiObjects[40].GetComponent<Text>().text = tS_.GetText(1589) + " [1]";
		uiObjects[41].GetComponent<Text>().text = tS_.GetText(1589) + " [2]";
		uiObjects[42].GetComponent<Text>().text = tS_.GetText(1589) + " [3]";
		uiObjects[43].GetComponent<Text>().text = tS_.GetText(1590) + " [1]";
		uiObjects[44].GetComponent<Text>().text = tS_.GetText(1590) + " [2]";
		uiObjects[45].GetComponent<Text>().text = tS_.GetText(1590) + " [3]";
		uiObjects[46].GetComponent<Text>().text = tS_.GetText(1588) + " [1]";
		uiObjects[47].GetComponent<Text>().text = tS_.GetText(1588) + " [2]";
		uiObjects[48].GetComponent<Text>().text = tS_.GetText(1588) + " [3]";
		if ((bool)selectedKonsole)
		{
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
			uiObjects[22].GetComponent<Text>().text = selectedKonsole.myName;
			uiObjects[26].GetComponent<Image>().sprite = selectedKonsole.GetTypSprite();
			if (!uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: true);
			}
			selectedKonsole.SetPic(uiObjects[5]);
			uiObjects[0].GetComponent<Image>().fillAmount = selectedKonsole.GetHaltbarkeit() * 0.1f;
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(2377) + ": " + mS_.RoundString(selectedKonsole.GetHaltbarkeit(), 1);
			uiObjects[3].GetComponent<Image>().fillAmount = selectedKonsole.GetHaltbarkeit() * 0.1f;
			for (int i = 0; i < buttonAdds.Length; i++)
			{
				if (buttonAdds[i])
				{
					uiObjects[3].GetComponent<Image>().fillAmount += haltbarkeitProKampagne[i] * 0.1f;
				}
			}
			if (selectedKonsole.isUnlocked)
			{
				uiObjects[23].GetComponent<Text>().text = selectedKonsole.GetDateString();
			}
			else
			{
				uiObjects[23].GetComponent<Text>().text = tS_.GetText(528);
			}
			for (int j = 0; j < 9; j++)
			{
				uiObjects[31 + j].GetComponent<Text>().text = mS_.GetMoney(GetCosts(j, selectedKonsole), showDollar: true);
			}
			for (int k = 0; k < 9; k++)
			{
				if (!selectedKonsole.haltbarkeitDone[k])
				{
					uiObjects[49 + k].GetComponent<Text>().text = "+" + mS_.RoundString(haltbarkeitProKampagne[k], 1);
				}
				else
				{
					uiObjects[49 + k].GetComponent<Text>().text = "";
				}
			}
			for (int l = 0; l < uiButtons.Length; l++)
			{
				if (selectedKonsole.haltbarkeitDone[l])
				{
					uiButtons[l].GetComponent<Button>().interactable = false;
					uiButtons[l].transform.GetChild(4).gameObject.SetActive(value: true);
					uiButtons[l].transform.GetChild(5).gameObject.SetActive(value: false);
				}
				else
				{
					uiButtons[l].GetComponent<Button>().interactable = true;
					uiButtons[l].transform.GetChild(4).gameObject.SetActive(value: false);
					uiButtons[l].transform.GetChild(5).gameObject.SetActive(value: true);
				}
				if (WirdInAnderenRaumBearbeitet(l, selectedKonsole))
				{
					uiButtons[l].GetComponent<Button>().interactable = false;
					uiButtons[l].transform.GetChild(6).gameObject.SetActive(value: true);
				}
				else
				{
					uiButtons[l].transform.GetChild(6).gameObject.SetActive(value: false);
				}
				if (buttonAdds[l])
				{
					uiButtons[l].GetComponent<Image>().color = guiMain_.colors[7];
				}
				else
				{
					uiButtons[l].GetComponent<Image>().color = Color.white;
				}
			}
		}
		else
		{
			if (uiObjects[5].activeSelf)
			{
				uiObjects[5].SetActive(value: false);
			}
			uiObjects[22].GetComponent<Text>().text = tS_.GetText(949);
			uiObjects[23].GetComponent<Text>().text = "---";
			uiObjects[26].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[0].GetComponent<Image>().fillAmount = 0f;
			uiObjects[3].GetComponent<Image>().fillAmount = 0f;
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(2377) + ": 0.0";
			for (int m = 0; m < 9; m++)
			{
				uiObjects[31 + m].GetComponent<Text>().text = "$ ---";
				uiObjects[49 + m].GetComponent<Text>().text = "";
			}
			uiObjects[2].GetComponent<Text>().text = mS_.GetMoney(0L, showDollar: true);
		}
	}

	public bool WirdInAnderenRaumBearbeitet(int slot, platformScript g_)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Task");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				taskKonsoleHaltbarkeit component = array[i].GetComponent<taskKonsoleHaltbarkeit>();
				if ((bool)component && component.targetID == g_.myID && component.adds[slot])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SetKonsole(platformScript script_)
	{
		selectedKonsole = script_;
		SetData();
	}

	public platformScript FindKonsole()
	{
		if ((bool)selectedKonsole && selectedKonsole.ownerID == mS_.myID && selectedKonsole.IsVerfuegbar() && !selectedKonsole.vomMarktGenommen && selectedKonsole.GetHaltbarkeit() < 10f && !WirdInAnderenRaumBearbeitet(0, selectedKonsole) && !WirdInAnderenRaumBearbeitet(1, selectedKonsole) && !WirdInAnderenRaumBearbeitet(2, selectedKonsole) && !WirdInAnderenRaumBearbeitet(3, selectedKonsole) && !WirdInAnderenRaumBearbeitet(4, selectedKonsole) && !WirdInAnderenRaumBearbeitet(5, selectedKonsole) && !WirdInAnderenRaumBearbeitet(6, selectedKonsole) && !WirdInAnderenRaumBearbeitet(7, selectedKonsole) && !WirdInAnderenRaumBearbeitet(8, selectedKonsole))
		{
			return selectedKonsole;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.ownerID == mS_.myID && component.IsVerfuegbar() && !component.vomMarktGenommen && component.GetHaltbarkeit() < 10f && !WirdInAnderenRaumBearbeitet(0, component) && !WirdInAnderenRaumBearbeitet(1, component) && !WirdInAnderenRaumBearbeitet(2, component) && !WirdInAnderenRaumBearbeitet(3, component) && !WirdInAnderenRaumBearbeitet(4, component) && !WirdInAnderenRaumBearbeitet(5, component) && !WirdInAnderenRaumBearbeitet(6, component) && !WirdInAnderenRaumBearbeitet(7, component) && !WirdInAnderenRaumBearbeitet(8, component))
				{
					return component;
				}
			}
		}
		return null;
	}

	public void BUTTON_Abbrechen()
	{
		guiMain_.CloseMenu();
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_SelectKonsole()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[453]);
		guiMain_.uiObjects[453].GetComponent<Menu_Dev_KonsoleHaltbarkeitSelect>().Init();
	}

	public void BUTTON_Adds(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			if (uiButtons[0].GetComponent<Button>().interactable)
			{
				buttonAdds[0] = true;
			}
			buttonAdds[1] = false;
			buttonAdds[2] = false;
			break;
		case 1:
			if (uiButtons[0].GetComponent<Button>().interactable)
			{
				buttonAdds[0] = true;
			}
			if (uiButtons[1].GetComponent<Button>().interactable)
			{
				buttonAdds[1] = true;
			}
			buttonAdds[2] = false;
			break;
		case 2:
			if (uiButtons[0].GetComponent<Button>().interactable)
			{
				buttonAdds[0] = true;
			}
			if (uiButtons[1].GetComponent<Button>().interactable)
			{
				buttonAdds[1] = true;
			}
			if (uiButtons[2].GetComponent<Button>().interactable)
			{
				buttonAdds[2] = true;
			}
			break;
		case 3:
			if (uiButtons[3].GetComponent<Button>().interactable)
			{
				buttonAdds[3] = true;
			}
			buttonAdds[4] = false;
			buttonAdds[5] = false;
			break;
		case 4:
			if (uiButtons[3].GetComponent<Button>().interactable)
			{
				buttonAdds[3] = true;
			}
			if (uiButtons[4].GetComponent<Button>().interactable)
			{
				buttonAdds[4] = true;
			}
			buttonAdds[5] = false;
			break;
		case 5:
			if (uiButtons[3].GetComponent<Button>().interactable)
			{
				buttonAdds[3] = true;
			}
			if (uiButtons[4].GetComponent<Button>().interactable)
			{
				buttonAdds[4] = true;
			}
			if (uiButtons[5].GetComponent<Button>().interactable)
			{
				buttonAdds[5] = true;
			}
			break;
		case 6:
			if (uiButtons[6].GetComponent<Button>().interactable)
			{
				buttonAdds[6] = true;
			}
			buttonAdds[7] = false;
			buttonAdds[8] = false;
			break;
		case 7:
			if (uiButtons[6].GetComponent<Button>().interactable)
			{
				buttonAdds[6] = true;
			}
			if (uiButtons[7].GetComponent<Button>().interactable)
			{
				buttonAdds[7] = true;
			}
			buttonAdds[8] = false;
			break;
		case 8:
			if (uiButtons[6].GetComponent<Button>().interactable)
			{
				buttonAdds[6] = true;
			}
			if (uiButtons[7].GetComponent<Button>().interactable)
			{
				buttonAdds[7] = true;
			}
			if (uiButtons[8].GetComponent<Button>().interactable)
			{
				buttonAdds[8] = true;
			}
			break;
		}
		SetData();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		allAdds = !allAdds;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (uiButtons[i].GetComponent<Button>().interactable)
			{
				buttonAdds[i] = allAdds;
			}
		}
		SetData();
	}

	public void BUTTON_Delete(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			buttonAdds[0] = false;
			buttonAdds[1] = false;
			buttonAdds[2] = false;
			break;
		case 1:
			buttonAdds[3] = false;
			buttonAdds[4] = false;
			buttonAdds[5] = false;
			break;
		case 2:
			buttonAdds[6] = false;
			buttonAdds[7] = false;
			buttonAdds[8] = false;
			break;
		}
		SetData();
	}

	public long GetCosts(int i, platformScript script_)
	{
		return (long)(preise[i] * (float)script_.entwicklungsKosten);
	}

	private long GetDevCosts()
	{
		long num = 0L;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += GetCosts(i, selectedKonsole);
			}
		}
		return num;
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		long num = Mathf.RoundToInt(GetDevCosts());
		if (!selectedKonsole || selectedKonsole.GetHaltbarkeit() >= 10f)
		{
			return;
		}
		if (num <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(2365), closeMenu: false);
			return;
		}
		if (mS_.NotEnoughMoney(num))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		mS_.Pay(num, 10);
		taskKonsoleHaltbarkeit taskKonsoleHaltbarkeit2 = guiMain_.AddTask_KonsoleHaltbarkeit();
		taskKonsoleHaltbarkeit2.Init(fromSavegame: false);
		taskKonsoleHaltbarkeit2.targetID = selectedKonsole.myID;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				taskKonsoleHaltbarkeit2.adds[i] = true;
			}
		}
		taskKonsoleHaltbarkeit2.FindNewAdd();
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskKonsoleHaltbarkeit2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
