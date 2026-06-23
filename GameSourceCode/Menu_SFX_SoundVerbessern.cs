using UnityEngine;
using UnityEngine.UI;

public class Menu_SFX_SoundVerbessern : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] costs;

	public float[] points;

	public float[] pointsInPercent;

	private bool[] buttonAdds = new bool[6];

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

	private gameScript selectedGame;

	private roomScript rS_;

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
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
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
				UpdateGUI();
			}
		}
	}

	public void Init(roomScript roomScript_)
	{
		FindScripts();
		rS_ = roomScript_;
		selectedGame = FindGame();
		DeselectAllButtons();
		UpdateGUI();
	}

	private void DeselectAllButtons()
	{
		allAdds = false;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = false;
			uiObjects[i].transform.Find("TextPreis").GetComponent<Text>().text = mS_.GetMoney(GetCosts(i, selectedGame), showDollar: true);
			uiObjects[i].GetComponent<Button>().interactable = false;
			uiObjects[i].transform.GetChild(4).gameObject.SetActive(value: false);
			uiObjects[i].transform.GetChild(5).gameObject.SetActive(value: false);
			uiObjects[i].transform.GetChild(6).gameObject.SetActive(value: false);
		}
	}

	public int GetCosts(int i, gameScript script_)
	{
		if (!script_)
		{
			return 0;
		}
		int num = costs[i] * script_.GetPointsForAdds();
		num = num / 1000 * 1000;
		if (num < 1000)
		{
			num = 1000;
		}
		return num;
	}

	private void UpdateGUI()
	{
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if ((bool)selectedGame)
			{
				if (selectedGame.soundStudio[i])
				{
					uiObjects[i].GetComponent<Button>().interactable = false;
					uiObjects[i].transform.GetChild(4).gameObject.SetActive(value: true);
				}
				else
				{
					uiObjects[i].GetComponent<Button>().interactable = true;
					uiObjects[i].transform.GetChild(4).gameObject.SetActive(value: false);
				}
				if (WirdInAnderenRaumBearbeitet(i))
				{
					uiObjects[i].GetComponent<Button>().interactable = false;
					uiObjects[i].transform.GetChild(5).gameObject.SetActive(value: true);
				}
			}
			if (buttonAdds[i])
			{
				uiObjects[i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiObjects[i].GetComponent<Image>().color = Color.white;
			}
		}
		if ((bool)selectedGame && !selectedGame.gameGameplayFeatures[41])
		{
			uiObjects[5].GetComponent<Button>().interactable = false;
			uiObjects[5].transform.GetChild(6).gameObject.SetActive(value: true);
			string text = tS_.GetText(919);
			text = text.Replace("<NAME>", gF_.GetName(41));
			uiObjects[5].transform.GetChild(6).GetChild(0).GetComponent<Text>()
				.text = text;
		}
		uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		if ((bool)selectedGame)
		{
			uiObjects[7].GetComponent<Text>().text = selectedGame.GetNameWithTag();
			uiObjects[8].GetComponent<Image>().sprite = selectedGame.GetTypSprite();
			uiObjects[9].GetComponent<Text>().text = selectedGame.GetGenreString();
			uiObjects[10].GetComponent<Image>().sprite = selectedGame.GetPlatformTypSprite();
		}
		else
		{
			uiObjects[7].GetComponent<Text>().text = tS_.GetText(611);
			uiObjects[9].GetComponent<Text>().text = "---";
			uiObjects[8].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[10].GetComponent<Image>().sprite = games_.gamePlatformTypSprites[0];
		}
	}

	public void SetGame(gameScript script_)
	{
		selectedGame = script_;
		DeselectAllButtons();
		UpdateGUI();
	}

	public void BUTTON_SelectGame()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[177]);
		guiMain_.uiObjects[177].GetComponent<Menu_SFX_SoundVerbessernSelectGame>().Init();
	}

	public gameScript FindGame()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && component.developerID == mS_.myID && !component.isOnMarket && component.inDevelopment)
				{
					return component;
				}
			}
		}
		return null;
	}

	private long GetDevCosts()
	{
		long num = 0L;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += GetCosts(i, selectedGame);
			}
		}
		return num;
	}

	private bool WirdInAnderenRaumBearbeitet(int slot)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Task");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				taskSoundVerbessern component = array[i].GetComponent<taskSoundVerbessern>();
				if ((bool)component && component.targetID == selectedGame.myID && component.adds[slot])
				{
					return true;
				}
			}
		}
		return false;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Adds(int i)
	{
		sfx_.PlaySound(3, force: true);
		buttonAdds[i] = !buttonAdds[i];
		UpdateGUI();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		allAdds = !allAdds;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (uiObjects[i].GetComponent<Button>().interactable)
			{
				buttonAdds[i] = allAdds;
			}
		}
		UpdateGUI();
	}

	public void BUTTON_Start()
	{
		int num = Mathf.RoundToInt(GetDevCosts());
		if (!selectedGame || !rS_)
		{
			return;
		}
		if (num <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(914), closeMenu: false);
			return;
		}
		if (mS_.NotEnoughMoney(num))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(num, 10);
		taskSoundVerbessern taskSoundVerbessern2 = guiMain_.AddTask_SoundVerbessern();
		taskSoundVerbessern2.Init(fromSavegame: false);
		taskSoundVerbessern2.targetID = selectedGame.myID;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				taskSoundVerbessern2.adds[i] = true;
			}
		}
		taskSoundVerbessern2.FindNewAdd();
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskSoundVerbessern2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
