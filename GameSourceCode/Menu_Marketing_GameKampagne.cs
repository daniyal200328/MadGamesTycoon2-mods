using UnityEngine;
using UnityEngine.UI;

public class Menu_Marketing_GameKampagne : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] preise;

	public int[] maxHype;

	public int[] workPoints;

	public int[] hypeProKampagne;

	public Sprite[] sprites;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private roomScript rS_;

	private int selectedKampagne = -1;

	private gameScript selectedGame;

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
		if (selectedKampagne == -1 || selectedGame == null)
		{
			uiObjects[25].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[25].GetComponent<Button>().interactable = true;
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

	public void Init(roomScript roomS_)
	{
		FindScripts();
		rS_ = roomS_;
		selectedKampagne = -1;
		selectedGame = FindGame();
		if (unlock_.Get(56))
		{
			uiObjects[0].SetActive(value: false);
			uiObjects[5].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[0].SetActive(value: true);
			uiObjects[5].GetComponent<Button>().interactable = false;
		}
		if (unlock_.Get(57))
		{
			uiObjects[1].SetActive(value: false);
			uiObjects[6].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[1].SetActive(value: true);
			uiObjects[6].GetComponent<Button>().interactable = false;
		}
		SetButtonColor(-1);
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(preise[0], showDollar: true);
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(preise[1], showDollar: true);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(preise[2], showDollar: true);
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(preise[3], showDollar: true);
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(preise[4], showDollar: true);
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(preise[5], showDollar: true);
		uiObjects[16].GetComponent<Text>().text = "+" + hypeProKampagne[0] + " (" + tS_.GetText(661) + " " + maxHype[0] + ")";
		uiObjects[17].GetComponent<Text>().text = "+" + hypeProKampagne[1] + " (" + tS_.GetText(661) + " " + maxHype[1] + ")";
		uiObjects[18].GetComponent<Text>().text = "+" + hypeProKampagne[2] + " (" + tS_.GetText(661) + " " + maxHype[2] + ")";
		uiObjects[19].GetComponent<Text>().text = "+" + hypeProKampagne[3] + " (" + tS_.GetText(661) + " " + maxHype[3] + ")";
		uiObjects[20].GetComponent<Text>().text = "+" + hypeProKampagne[4] + " (" + tS_.GetText(661) + " " + maxHype[4] + ")";
		uiObjects[21].GetComponent<Text>().text = "+" + hypeProKampagne[5] + " (" + tS_.GetText(661) + " " + maxHype[5] + ")";
		SetData();
	}

	private void SetData()
	{
		if ((bool)selectedGame)
		{
			uiObjects[22].GetComponent<Text>().text = selectedGame.GetNameWithTag();
			uiObjects[24].GetComponent<Text>().text = Mathf.RoundToInt(selectedGame.GetHype()).ToString();
			uiObjects[26].GetComponent<Image>().sprite = selectedGame.GetTypSprite();
			uiObjects[23].GetComponent<Text>().text = selectedGame.GetReleaseDateString();
		}
		else
		{
			uiObjects[22].GetComponent<Text>().text = tS_.GetText(611);
			uiObjects[24].GetComponent<Text>().text = "-";
			uiObjects[23].GetComponent<Text>().text = "---";
			uiObjects[26].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
	}

	public void SetGame(gameScript script_)
	{
		selectedGame = script_;
		SetData();
	}

	private void SetButtonColor(int i)
	{
		uiObjects[2].GetComponent<Image>().color = Color.white;
		uiObjects[3].GetComponent<Image>().color = Color.white;
		uiObjects[4].GetComponent<Image>().color = Color.white;
		uiObjects[5].GetComponent<Image>().color = Color.white;
		uiObjects[6].GetComponent<Image>().color = Color.white;
		uiObjects[7].GetComponent<Image>().color = Color.white;
		switch (i)
		{
		case 0:
			uiObjects[2].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 1:
			uiObjects[3].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 2:
			uiObjects[4].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 3:
			uiObjects[5].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 4:
			uiObjects[6].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 5:
			uiObjects[7].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		}
	}

	public gameScript FindGame()
	{
		if ((bool)selectedGame && (selectedGame.developerID == mS_.myID || selectedGame.publisherID == mS_.myID) && (selectedGame.isOnMarket || selectedGame.inDevelopment) && !selectedGame.typ_contractGame)
		{
			return selectedGame;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && (component.developerID == mS_.myID || component.publisherID == mS_.myID) && (component.isOnMarket || component.inDevelopment) && !component.typ_contractGame)
				{
					return component;
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				gameScript component2 = array[j].GetComponent<gameScript>();
				if ((bool)component2 && (component2.developerID == mS_.myID || component2.publisherID == mS_.myID) && component2.schublade && !component2.typ_contractGame)
				{
					return component2;
				}
			}
		}
		return null;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Select(int i)
	{
		sfx_.PlaySound(3, force: true);
		selectedKampagne = i;
		SetButtonColor(i);
	}

	public void BUTTON_SelectGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[90]);
		guiMain_.uiObjects[90].GetComponent<Menu_Marketing_SelectGame>().Init();
	}

	public void BUTTON_OK()
	{
		if (!selectedGame || selectedKampagne == -1 || !rS_)
		{
			return;
		}
		if (mS_.NotEnoughMoney(preise[selectedKampagne]))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(preise[selectedKampagne], 12);
		taskMarketing taskMarketing2 = guiMain_.AddTask_Marketing();
		taskMarketing2.Init(fromSavegame: false);
		taskMarketing2.targetID = selectedGame.myID;
		taskMarketing2.typ = 0;
		taskMarketing2.kampagne = selectedKampagne;
		taskMarketing2.automatic = uiObjects[14].GetComponent<Toggle>().isOn;
		taskMarketing2.stopAutomatic = uiObjects[15].GetComponent<Toggle>().isOn;
		taskMarketing2.disableWarten = uiObjects[27].GetComponent<Toggle>().isOn;
		taskMarketing2.points = workPoints[selectedKampagne];
		taskMarketing2.pointsLeft = workPoints[selectedKampagne];
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskMarketing2.myID;
		}
		guiMain_.CloseMenu();
		guiMain_.uiObjects[88].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	public void TOGGLE_Auto()
	{
		if (!uiObjects[14].GetComponent<Toggle>().isOn)
		{
			uiObjects[15].GetComponent<Toggle>().interactable = false;
			uiObjects[15].GetComponent<Toggle>().isOn = false;
			uiObjects[27].GetComponent<Toggle>().interactable = false;
			uiObjects[27].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[15].GetComponent<Toggle>().interactable = true;
			uiObjects[27].GetComponent<Toggle>().interactable = true;
		}
	}
}
