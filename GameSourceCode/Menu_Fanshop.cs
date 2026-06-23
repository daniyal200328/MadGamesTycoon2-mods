using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Fanshop : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private gameScript selectedGame;

	public int[] bestellmenge;

	public float[] einkaufspreis;

	public float[] beliebtheit;

	public float[] optimalerPreis;

	public int[] needStars;

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

	public void Init(gameScript gS_)
	{
		FindScripts();
		if (!gS_)
		{
			BUTTON_Abbrechen();
			return;
		}
		selectedGame = gS_;
		uiObjects[70].GetComponent<Toggle>().isOn = selectedGame.merchKeinVerkauf;
		if (selectedGame.merchVerkaufspreis[0] <= 0f)
		{
			for (int i = 0; i < optimalerPreis.Length; i++)
			{
				selectedGame.merchVerkaufspreis[i] = optimalerPreis[i];
			}
		}
		SetData();
		SetUnlocks();
	}

	private void SetUnlocks()
	{
		for (int i = 0; i < needStars.Length; i++)
		{
			if (Mathf.RoundToInt(selectedGame.GetIpBekanntheit()) >= needStars[i])
			{
				if (uiObjects[54 + i].activeSelf)
				{
					uiObjects[54 + i].SetActive(value: false);
				}
				continue;
			}
			string text = tS_.GetText(1847);
			switch (needStars[i])
			{
			case 0:
				text += "<br><size=22>☆☆☆☆☆</size>";
				break;
			case 1:
				text += "<br><size=22>★☆☆☆☆</size>";
				break;
			case 2:
				text += "<br><size=22>★★☆☆☆</size>";
				break;
			case 3:
				text += "<br><size=22>★★★☆☆</size>";
				break;
			case 4:
				text += "<br><size=22>★★★★☆</size>";
				break;
			case 5:
				text += "<br><size=22>★★★★★</size>";
				break;
			}
			uiObjects[54 + i].GetComponent<tooltip>().c = text;
			if (!uiObjects[54 + i].activeSelf)
			{
				uiObjects[54 + i].SetActive(value: true);
			}
		}
	}

	private int UpdateBestellpreis()
	{
		int num = 0;
		for (int i = 0; i < einkaufspreis.Length; i++)
		{
			int bestellpreis = GetBestellpreis(i);
			num += bestellpreis;
			if (bestellpreis > 0)
			{
				uiObjects[10 + i].GetComponent<Text>().text = mS_.GetMoney(bestellpreis, showDollar: true);
			}
			else
			{
				uiObjects[10 + i].GetComponent<Text>().text = "";
			}
		}
		uiObjects[43].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		return num;
	}

	private int GetBestellpreis(int i)
	{
		return Mathf.RoundToInt((float)bestellmenge[i] * einkaufspreis[i]);
	}

	private void SetData()
	{
		if (!selectedGame)
		{
			return;
		}
		uiObjects[0].GetComponent<Text>().text = selectedGame.GetIpName();
		guiMain_.DrawIpBekanntheit(uiObjects[1], selectedGame);
		UpdateBestellpreis();
		for (int i = 0; i < bestellmenge.Length; i++)
		{
			int num = selectedGame.merchBestellungen[i];
			num /= 50;
			guiMain_.DrawStars10_Color(uiObjects[71 + i], num, Color.white);
		}
		uiObjects[44].GetComponent<Text>().text = mS_.GetMoney(selectedGame.merchGesamtGewinn, showDollar: true);
		if (selectedGame.merchGesamtGewinn < 0)
		{
			uiObjects[44].GetComponent<Text>().color = guiMain_.colors[18];
		}
		else
		{
			uiObjects[44].GetComponent<Text>().color = guiMain_.colors[13];
		}
		uiObjects[79].GetComponent<Text>().text = mS_.GetMoney(selectedGame.merchGewinnDiesenMonat, showDollar: true) + " | " + mS_.GetMoney(selectedGame.merchGewinnLetzterMonat, showDollar: true);
		uiObjects[79].GetComponent<Text>().color = guiMain_.colors[13];
		for (int j = 0; j < einkaufspreis.Length; j++)
		{
			uiObjects[2 + j].GetComponent<Text>().text = GetMoneyString(einkaufspreis[j]);
			if (selectedGame.merchVerkaufspreis[j] < GetMindestVerkaufspreis(j))
			{
				selectedGame.merchVerkaufspreis[j] = GetMindestVerkaufspreis(j);
			}
			uiObjects[18 + j].GetComponent<Text>().text = GetMoneyString(selectedGame.merchVerkaufspreis[j]);
			uiObjects[34 + j].GetComponent<Text>().text = mS_.GetMoney(selectedGame.merchDiesenMonat[j], showDollar: false) + " / " + mS_.GetMoney(selectedGame.merchLetzterMonat[j], showDollar: false);
			uiObjects[62 + j].GetComponent<Text>().text = mS_.GetMoney(selectedGame.merchGesamtSells[j], showDollar: false);
		}
	}

	public void INPUTFIELD_Bestellmenge(int i)
	{
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		if ((bool)selectedGame)
		{
			selectedGame.merchKeinVerkauf = uiObjects[70].GetComponent<Toggle>().isOn;
		}
		BUTTON_Abbrechen();
	}

	public void BUTTON_Bestellen()
	{
	}

	private string GetMoneyString(float f)
	{
		string text = "$" + mS_.Round(f, 2);
		switch (text.Length)
		{
		case 2:
			text += ",00";
			break;
		case 4:
			text += "0";
			break;
		}
		if (text[text.Length - 2] == ',')
		{
			text += "0";
		}
		return text;
	}

	public void BUTTON_MinusBestellmenge(int i)
	{
	}

	public void BUTTON_PlusBestellmenge(int i)
	{
	}

	private void SetInputFieldData()
	{
	}

	private IEnumerator iMinusVerkaufspreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusVerkaufspreis(i);
		}
	}

	public void BUTTON_MinusVerkaufspreis(int i)
	{
		sfx_.PlaySound(3, force: true);
		selectedGame.merchVerkaufspreis[i] -= 1f;
		if (selectedGame.merchVerkaufspreis[i] <= GetMindestVerkaufspreis(i))
		{
			selectedGame.merchVerkaufspreis[i] = GetMindestVerkaufspreis(i);
		}
		if (selectedGame.merchVerkaufspreis[i] > 29.99f)
		{
			selectedGame.merchVerkaufspreis[i] = 29.99f;
		}
		StartCoroutine(iMinusVerkaufspreis(i));
		SetData();
	}

	private IEnumerator iPlusVerkaufspreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusVerkaufspreis(i);
		}
	}

	public void BUTTON_PlusVerkaufspreis(int i)
	{
		sfx_.PlaySound(3, force: true);
		selectedGame.merchVerkaufspreis[i] += 1f;
		if (selectedGame.merchVerkaufspreis[i] <= GetMindestVerkaufspreis(i))
		{
			selectedGame.merchVerkaufspreis[i] = GetMindestVerkaufspreis(i);
		}
		if (selectedGame.merchVerkaufspreis[i] > 29.99f)
		{
			selectedGame.merchVerkaufspreis[i] = 29.99f;
		}
		StartCoroutine(iPlusVerkaufspreis(i));
		SetData();
	}

	public float GetMindestVerkaufspreis(int i)
	{
		return (float)Mathf.RoundToInt(einkaufspreis[i]) + 0.99f;
	}

	public void BUTTON_GlobalVerkaufspreis()
	{
		sfx_.PlaySound(3, force: true);
		int num = 1;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && selectedGame.myID != mS_.games_.arrayGamesScripts[i].myID && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num++;
				for (int j = 0; j < mS_.games_.arrayGamesScripts[i].merchVerkaufspreis.Length; j++)
				{
					mS_.games_.arrayGamesScripts[i].merchVerkaufspreis[j] = selectedGame.merchVerkaufspreis[j];
				}
			}
		}
		string text = tS_.GetText(1840);
		text = text.Replace("<NUM>", "<color=blue>" + num + "</color>");
		guiMain_.MessageBox(text, closeMenu: false);
	}

	public void TOGGLE_Automatic()
	{
	}

	public void TOGGLE_VerkaufEinstellen()
	{
	}
}
