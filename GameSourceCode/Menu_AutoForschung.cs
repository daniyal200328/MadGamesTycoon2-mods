using UnityEngine;
using UnityEngine.UI;

public class Menu_AutoForschung : MonoBehaviour
{
	public GameObject[] uiObjects;

	private bool[] kategorie = new bool[7];

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

	private forschungSonstiges forschungSonstiges_;

	private roomScript rS_;

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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(roomScript roomScript_)
	{
		FindScripts();
		rS_ = roomScript_;
		DeselectAllButtons();
		if ((bool)rS_)
		{
			if ((bool)rS_.GetTaskAutoForschung())
			{
				taskAutoForschung taskAutoForschung2 = rS_.GetTaskAutoForschung();
				if ((bool)taskAutoForschung2)
				{
					for (int i = 0; i < kategorie.Length; i++)
					{
						kategorie[i] = taskAutoForschung2.kategorie[i];
					}
				}
			}
			if ((bool)rS_.GetTaskForschung())
			{
				taskForschung taskForschung2 = rS_.GetTaskForschung();
				if ((bool)taskForschung2)
				{
					for (int j = 0; j < kategorie.Length; j++)
					{
						kategorie[j] = taskForschung2.kategorie[j];
					}
				}
			}
		}
		UpdateGUI();
	}

	private void DeselectAllButtons()
	{
		for (int i = 0; i < kategorie.Length; i++)
		{
			kategorie[i] = false;
		}
	}

	private void UpdateGUI()
	{
		if (!forschungSonstiges_.IsErforscht(39))
		{
			uiObjects[4].GetComponent<Button>().interactable = false;
			uiObjects[6].GetComponent<Button>().interactable = false;
			uiObjects[7].SetActive(value: true);
			uiObjects[8].SetActive(value: true);
		}
		else
		{
			uiObjects[4].GetComponent<Button>().interactable = true;
			uiObjects[6].GetComponent<Button>().interactable = true;
			uiObjects[7].SetActive(value: false);
			uiObjects[8].SetActive(value: false);
		}
		for (int i = 0; i < kategorie.Length; i++)
		{
			if (kategorie[i])
			{
				uiObjects[i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiObjects[i].GetComponent<Image>().color = Color.white;
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Kategorie(int i)
	{
		sfx_.PlaySound(3, force: true);
		kategorie[i] = !kategorie[i];
		UpdateGUI();
	}

	public void BUTTON_AlleKategorien()
	{
		sfx_.PlaySound(3, force: true);
		bool flag = kategorie[0];
		for (int i = 0; i < kategorie.Length; i++)
		{
			if (uiObjects[i].GetComponent<Button>().interactable)
			{
				kategorie[i] = !flag;
			}
		}
		UpdateGUI();
	}

	public void BUTTON_Start()
	{
		if (!rS_)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		bool flag = false;
		for (int i = 0; i < kategorie.Length; i++)
		{
			if (kategorie[i])
			{
				flag = true;
				break;
			}
		}
		if (!flag)
		{
			guiMain_.MessageBox(tS_.GetText(2421), closeMenu: false);
			return;
		}
		if ((bool)rS_.GetTaskForschung())
		{
			rS_.GetTaskForschung().Abbrechen();
		}
		if ((bool)rS_.GetTaskForschungWait())
		{
			rS_.GetTaskForschungWait().Abbrechen();
		}
		if ((bool)rS_.GetTaskAutoForschung())
		{
			rS_.GetTaskAutoForschung().Abbrechen();
		}
		taskAutoForschung taskAutoForschung2 = guiMain_.AddTask_AutoForschung();
		taskAutoForschung2.Init(fromSavegame: false);
		for (int j = 0; j < kategorie.Length; j++)
		{
			taskAutoForschung2.kategorie[j] = kategorie[j];
		}
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskAutoForschung2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
