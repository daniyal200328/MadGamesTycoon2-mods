using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_Sabotage : MonoBehaviour
{
	public GameObject[] uiPlayerButtons;

	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private mpCalls mpCalls_;

	public int selectedPlayer = -1;

	public int sabotageNummer;

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
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		selectedPlayer = -1;
		FindScripts();
		InitPlayerButtons();
		BUTTON_Sabotage(0);
	}

	private void Update()
	{
		UpdatePlayerButtons();
		if (mS_.sabotage_dunkel > 0 || mS_.sabotage_erwischt > 0)
		{
			uiObjects[6].GetComponent<Button>().interactable = false;
			if (!uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: true);
			}
			uiObjects[0].GetComponent<Button>().interactable = false;
			uiObjects[1].GetComponent<Button>().interactable = false;
			uiObjects[2].GetComponent<Button>().interactable = false;
			uiObjects[3].GetComponent<Button>().interactable = false;
			uiObjects[4].GetComponent<Button>().interactable = false;
			uiObjects[5].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[6].GetComponent<Button>().interactable = true;
			if (uiObjects[7].activeSelf)
			{
				uiObjects[7].SetActive(value: false);
			}
			uiObjects[0].GetComponent<Button>().interactable = true;
			uiObjects[1].GetComponent<Button>().interactable = true;
			uiObjects[2].GetComponent<Button>().interactable = true;
			uiObjects[3].GetComponent<Button>().interactable = true;
			uiObjects[4].GetComponent<Button>().interactable = true;
			uiObjects[5].GetComponent<Button>().interactable = true;
		}
	}

	public void UpdatePlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				if (selectedPlayer == i)
				{
					uiPlayerButtons[i].GetComponent<Image>().color = guiMain_.colors[20];
				}
				else
				{
					uiPlayerButtons[i].GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	public void InitPlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				uiPlayerButtons[i].SetActive(value: false);
			}
		}
		for (int j = 0; j < mpCalls_.playersMP.Count; j++)
		{
			int playerID = mpCalls_.playersMP[j].playerID;
			if (playerID == mS_.myID)
			{
				if (uiPlayerButtons[j].activeSelf)
				{
					uiPlayerButtons[j].SetActive(value: false);
				}
				continue;
			}
			if (!uiPlayerButtons[j].activeSelf)
			{
				uiPlayerButtons[j].SetActive(value: true);
			}
			if (selectedPlayer == -1)
			{
				selectedPlayer = j;
			}
			uiPlayerButtons[j].transform.GetChild(1).GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mpCalls_.GetLogo(playerID));
			uiPlayerButtons[j].transform.GetChild(2).GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
		}
	}

	public void BUTTON_Player(int p)
	{
		sfx_.PlaySound(12, force: true);
		selectedPlayer = p;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Sabotage(int i)
	{
		sfx_.PlaySound(3, force: true);
		sabotageNummer = i;
		uiObjects[0].GetComponent<Image>().color = Color.white;
		uiObjects[1].GetComponent<Image>().color = Color.white;
		uiObjects[2].GetComponent<Image>().color = Color.white;
		uiObjects[3].GetComponent<Image>().color = Color.white;
		uiObjects[4].GetComponent<Image>().color = Color.white;
		uiObjects[5].GetComponent<Image>().color = Color.white;
		switch (i)
		{
		case 0:
			uiObjects[0].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		case 1:
			uiObjects[1].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		case 2:
			uiObjects[2].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		case 3:
			uiObjects[3].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		case 4:
			uiObjects[4].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		case 5:
			uiObjects[5].GetComponent<Image>().color = guiMain_.colors[20];
			break;
		}
	}

	public void BUTTON_Ok()
	{
		if (selectedPlayer == -1)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		mS_.sabotage_dunkel = 48;
		mS_.sabotage_wurdeErwischt = false;
		switch (sabotageNummer)
		{
		case 0:
			if (Random.Range(0, 100) >= 95)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		case 1:
			if (Random.Range(0, 100) >= 90)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		case 2:
			if (Random.Range(0, 100) >= 75)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		case 3:
			if (Random.Range(0, 100) >= 85)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		case 4:
			if (Random.Range(0, 100) >= 70)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		case 5:
			if (Random.Range(0, 100) >= 80)
			{
				mS_.sabotage_wurdeErwischt = true;
				mS_.sabotage_dunkel -= Random.Range(0, 36);
			}
			break;
		}
		if (mpCalls_.isServer)
		{
			switch (sabotageNummer)
			{
			case 0:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 4, 0, 0, 0);
				break;
			case 1:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 5, 0, 0, 0);
				break;
			case 2:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 6, 0, 0, 0);
				break;
			case 3:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 7, 0, 0, 0);
				break;
			case 4:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 8, 0, 0, 0);
				break;
			case 5:
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 9, 0, 0, 0);
				break;
			}
		}
		else
		{
			switch (sabotageNummer)
			{
			case 0:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 4, 0, 0, 0);
				break;
			case 1:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 5, 0, 0, 0);
				break;
			case 2:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 6, 0, 0, 0);
				break;
			case 3:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 7, 0, 0, 0);
				break;
			case 4:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 8, 0, 0, 0);
				break;
			case 5:
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 9, 0, 0, 0);
				break;
			}
		}
		string text = tS_.GetText(2297);
		text = text.Replace("<NAME1>", mpCalls_.GetCompanyName(mpCalls_.playersMP[selectedPlayer].playerID));
		switch (sabotageNummer)
		{
		case 0:
			text = text.Replace("<NAME2>", tS_.GetText(2276));
			break;
		case 1:
			text = text.Replace("<NAME2>", tS_.GetText(2277));
			break;
		case 2:
			text = text.Replace("<NAME2>", tS_.GetText(2278));
			break;
		case 3:
			text = text.Replace("<NAME2>", tS_.GetText(2279));
			break;
		case 4:
			text = text.Replace("<NAME2>", tS_.GetText(2280));
			break;
		case 5:
			text = text.Replace("<NAME2>", tS_.GetText(2281));
			break;
		}
		guiMain_.MessageBox(text, closeMenu: false);
		base.gameObject.SetActive(value: false);
	}
}
