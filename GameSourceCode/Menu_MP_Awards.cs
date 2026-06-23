using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_Awards : MonoBehaviour
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
	}

	private void Update()
	{
		UpdatePlayerButtons();
		SetData(selectedPlayer);
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

	private void SetData(int p)
	{
		if (p < mpCalls_.playersMP.Count)
		{
			GameObject gameObject = GameObject.Find("PUB_" + mpCalls_.playersMP[p].playerID);
			if ((bool)gameObject)
			{
				publisherScript component = gameObject.GetComponent<publisherScript>();
				uiObjects[0].GetComponent<Text>().text = component.awards[4] + "x";
				uiObjects[1].GetComponent<Text>().text = component.awards[2] + "x";
				uiObjects[2].GetComponent<Text>().text = component.awards[3] + "x";
				uiObjects[3].GetComponent<Text>().text = component.awards[0] + "x";
				uiObjects[4].GetComponent<Text>().text = component.awards[1] + "x";
				uiObjects[5].GetComponent<Text>().text = component.awards[8] + "x";
				uiObjects[6].GetComponent<Text>().text = component.awards[7] + "x";
				uiObjects[7].GetComponent<Text>().text = component.awards[6] + "x";
				uiObjects[8].GetComponent<Text>().text = component.awards[5] + "x";
				uiObjects[9].GetComponent<Text>().text = component.awards[9] + "x";
				uiObjects[10].GetComponent<Text>().text = component.awards[10] + "x";
				uiObjects[11].GetComponent<Text>().text = component.awards[11] + "x";
			}
		}
	}
}
