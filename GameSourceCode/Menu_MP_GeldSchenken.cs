using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_GeldSchenken : MonoBehaviour
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

	public int value;

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

	public void BUTTON_Ok()
	{
		if (selectedPlayer == -1 || value < 0)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		if (mS_.money < value)
		{
			guiMain_.ShowNoMoney();
			return;
		}
		mS_.Pay(value, 9);
		if (mpCalls_.isServer)
		{
			mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 0, value, 0, 0);
		}
		else
		{
			mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 0, value, 0, 0);
		}
		string text = tS_.GetText(1328);
		text = text.Replace("<NAME>", mpCalls_.GetCompanyName(mpCalls_.playersMP[selectedPlayer].playerID));
		text = text.Replace("<NUM>", mS_.GetMoney(value, showDollar: true));
		guiMain_.MessageBox(text, closeMenu: false);
		base.gameObject.SetActive(value: false);
	}

	public void SLIDER_Money()
	{
		value = Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value * 10000f);
		SetInputFieldData();
	}

	public void INPUTFIELD_Money()
	{
		if (uiObjects[4].GetComponent<InputField>().text.Length >= 1)
		{
			value = int.Parse(uiObjects[4].GetComponent<InputField>().text);
			if (value < 0)
			{
				value = 0;
				SetInputFieldData();
			}
		}
		else
		{
			value = 0;
		}
	}

	private void SetInputFieldData()
	{
		uiObjects[4].GetComponent<InputField>().text = value.ToString();
	}

	private IEnumerator iMinus()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Minus();
		}
	}

	public void BUTTON_Minus()
	{
		sfx_.PlaySound(3, force: true);
		value -= 10000;
		if (value < 0)
		{
			value = 0;
		}
		StartCoroutine(iMinus());
		SetInputFieldData();
		uiObjects[5].GetComponent<Slider>().value = value / 10000;
	}

	private IEnumerator iPlus()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Plus();
		}
	}

	public void BUTTON_Plus()
	{
		sfx_.PlaySound(3, force: true);
		value += 10000;
		if (value > 99999999)
		{
			value = 99999999;
		}
		StartCoroutine(iPlus());
		SetInputFieldData();
		uiObjects[5].GetComponent<Slider>().value = value / 10000;
	}
}
