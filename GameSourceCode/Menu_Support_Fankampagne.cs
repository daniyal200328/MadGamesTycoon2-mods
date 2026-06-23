using UnityEngine;
using UnityEngine.UI;

public class Menu_Support_Fankampagne : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] preise;

	public int[] fans;

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
		if (selectedKampagne == -1)
		{
			uiObjects[25].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[25].GetComponent<Button>().interactable = true;
		}
	}

	public void Init(roomScript roomS_)
	{
		FindScripts();
		rS_ = roomS_;
		selectedKampagne = -1;
		SetButtonColor(-1);
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(preise[0], showDollar: true);
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(preise[1], showDollar: true);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(preise[2], showDollar: true);
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(preise[3], showDollar: true);
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(preise[4], showDollar: true);
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(preise[5], showDollar: true);
		uiObjects[16].GetComponent<Text>().text = "+" + fans[0];
		uiObjects[17].GetComponent<Text>().text = "+" + fans[1];
		uiObjects[18].GetComponent<Text>().text = "+" + fans[2];
		uiObjects[19].GetComponent<Text>().text = "+" + fans[3];
		uiObjects[20].GetComponent<Text>().text = "+" + fans[4];
		uiObjects[21].GetComponent<Text>().text = "+" + fans[5];
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

	public void BUTTON_OK()
	{
		if (selectedKampagne == -1 || !rS_)
		{
			return;
		}
		if (mS_.NotEnoughMoney(preise[selectedKampagne]))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(preise[selectedKampagne], 16);
		taskFankampagne taskFankampagne2 = guiMain_.AddTask_Fankampagne();
		taskFankampagne2.Init(fromSavegame: false);
		taskFankampagne2.kampagne = selectedKampagne;
		taskFankampagne2.automatic = uiObjects[14].GetComponent<Toggle>().isOn;
		taskFankampagne2.points = workPoints[selectedKampagne];
		taskFankampagne2.pointsLeft = workPoints[selectedKampagne];
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskFankampagne2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void TOGGLE_Auto()
	{
	}
}
