using UnityEngine;
using UnityEngine.UI;

public class Menu_MarketingSpezial : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] preise;

	public int[] workPoints;

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
			uiObjects[16].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[16].GetComponent<Button>().interactable = true;
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
		SetButtonColor(-1);
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(preise[0], showDollar: true);
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(preise[1], showDollar: true);
		uiObjects[14].GetComponent<Text>().text = mS_.GetMoney(preise[2], showDollar: true);
		uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(preise[3], showDollar: true);
		uiObjects[20].GetComponent<Text>().text = mS_.GetMoney(preise[4], showDollar: true);
		SetData();
	}

	private void SetData()
	{
		uiObjects[0].GetComponent<Button>().interactable = true;
		uiObjects[4].SetActive(value: false);
		uiObjects[1].GetComponent<Button>().interactable = true;
		uiObjects[5].SetActive(value: false);
		uiObjects[2].GetComponent<Button>().interactable = true;
		uiObjects[6].SetActive(value: false);
		uiObjects[3].GetComponent<Button>().interactable = true;
		uiObjects[7].SetActive(value: false);
		uiObjects[18].GetComponent<Button>().interactable = true;
		uiObjects[19].SetActive(value: false);
		if ((bool)selectedGame)
		{
			uiObjects[9].GetComponent<Text>().text = selectedGame.GetNameWithTag();
			uiObjects[11].GetComponent<Image>().sprite = selectedGame.GetTypSprite();
			uiObjects[8].GetComponent<tooltip>().c = selectedGame.GetTooltip();
			uiObjects[17].GetComponent<Text>().text = Mathf.RoundToInt(selectedGame.GetHype()).ToString();
			uiObjects[10].GetComponent<Text>().text = selectedGame.GetReleaseDateString();
			if (selectedGame.specialMarketing[0] != 0 || (!selectedGame.inDevelopment && !selectedGame.schublade) || WirdInAnderenRaumBearbeitet(0))
			{
				uiObjects[0].GetComponent<Button>().interactable = false;
				if (selectedGame.specialMarketing[0] != 0)
				{
					uiObjects[4].SetActive(value: true);
				}
			}
			if (selectedGame.specialMarketing[1] != 0 || (!selectedGame.inDevelopment && !selectedGame.schublade) || WirdInAnderenRaumBearbeitet(1))
			{
				uiObjects[1].GetComponent<Button>().interactable = false;
				if (selectedGame.specialMarketing[1] != 0)
				{
					uiObjects[5].SetActive(value: true);
				}
			}
			if (selectedGame.specialMarketing[2] != 0 || (!selectedGame.inDevelopment && !selectedGame.schublade) || selectedGame.hype < 90f || WirdInAnderenRaumBearbeitet(2))
			{
				uiObjects[2].GetComponent<Button>().interactable = false;
				if (selectedGame.specialMarketing[2] != 0)
				{
					uiObjects[6].SetActive(value: true);
				}
			}
			if (selectedGame.specialMarketing[3] != 0 || selectedGame.inDevelopment || selectedGame.schublade || WirdInAnderenRaumBearbeitet(3))
			{
				uiObjects[3].GetComponent<Button>().interactable = false;
				if (selectedGame.specialMarketing[3] != 0)
				{
					uiObjects[7].SetActive(value: true);
				}
			}
			if (selectedGame.specialMarketing[4] != 0 || selectedGame.inDevelopment || selectedGame.schublade || WirdInAnderenRaumBearbeitet(4))
			{
				uiObjects[18].GetComponent<Button>().interactable = false;
				if (selectedGame.specialMarketing[4] != 0)
				{
					uiObjects[19].SetActive(value: true);
				}
			}
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = tS_.GetText(611);
			uiObjects[10].GetComponent<Text>().text = "---";
			uiObjects[17].GetComponent<Text>().text = "-";
			uiObjects[11].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
	}

	public void SetGame(gameScript script_)
	{
		selectedKampagne = -1;
		SetButtonColor(-1);
		selectedGame = script_;
		SetData();
	}

	private void SetButtonColor(int i)
	{
		uiObjects[0].GetComponent<Image>().color = Color.white;
		uiObjects[1].GetComponent<Image>().color = Color.white;
		uiObjects[2].GetComponent<Image>().color = Color.white;
		uiObjects[3].GetComponent<Image>().color = Color.white;
		uiObjects[18].GetComponent<Image>().color = Color.white;
		switch (i)
		{
		case 0:
			uiObjects[0].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 1:
			uiObjects[1].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 2:
			uiObjects[2].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 3:
			uiObjects[3].GetComponent<Image>().color = guiMain_.colors[7];
			break;
		case 4:
			uiObjects[18].GetComponent<Image>().color = guiMain_.colors[7];
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
		guiMain_.CloseMenu();
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[295]);
		guiMain_.uiObjects[295].GetComponent<Menu_Marketing_SpezialSelectGame>().Init();
	}

	public void BUTTON_OK()
	{
		if (!selectedGame || selectedKampagne == -1 || !rS_ || selectedGame.specialMarketing[selectedKampagne] != 0)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		if ((selectedKampagne == 0 || selectedKampagne == 1) && !selectedGame.GetEinePlattformReleased())
		{
			guiMain_.MessageBox(tS_.GetText(2438), closeMenu: false);
			return;
		}
		if (mS_.NotEnoughMoney(preise[selectedKampagne]))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		mS_.Pay(preise[selectedKampagne], 12);
		taskMarketingSpezial taskMarketingSpezial2 = guiMain_.AddTask_MarketingSpezial();
		taskMarketingSpezial2.Init(fromSavegame: false);
		taskMarketingSpezial2.targetID = selectedGame.myID;
		taskMarketingSpezial2.kampagne = selectedKampagne;
		taskMarketingSpezial2.points = workPoints[selectedKampagne];
		taskMarketingSpezial2.pointsLeft = workPoints[selectedKampagne];
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskMarketingSpezial2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	private bool WirdInAnderenRaumBearbeitet(int kampagne_)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Task");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				taskMarketingSpezial component = array[i].GetComponent<taskMarketingSpezial>();
				if ((bool)component && component.targetID == selectedGame.myID && component.kampagne == kampagne_)
				{
					return true;
				}
			}
		}
		return false;
	}
}
