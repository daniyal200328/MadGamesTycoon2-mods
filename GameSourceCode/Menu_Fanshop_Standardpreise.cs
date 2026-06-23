using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Fanshop_Standardpreise : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private unlockScript unlock_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private cameraMovementScript cmS_;

	private Menu_Fanshop menu_;

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
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[367].GetComponent<Menu_Fanshop>();
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

	public void Init()
	{
		FindScripts();
		SetData();
	}

	private int GetBestellpreis(int i)
	{
		return Mathf.RoundToInt((float)menu_.bestellmenge[i] * menu_.einkaufspreis[i]);
	}

	private void SetData()
	{
		uiObjects[44].GetComponent<Text>().text = GetGesamtGewinnAll();
		uiObjects[44].GetComponent<Text>().color = guiMain_.colors[13];
		uiObjects[79].GetComponent<Text>().text = GetGewinnAll();
		uiObjects[79].GetComponent<Text>().color = guiMain_.colors[13];
		for (int i = 0; i < menu_.einkaufspreis.Length; i++)
		{
			uiObjects[2 + i].GetComponent<Text>().text = GetMoneyString(menu_.einkaufspreis[i]);
			if (mS_.merchStandardpreis[i] < GetMindestVerkaufspreis(i))
			{
				mS_.merchStandardpreis[i] = GetMindestVerkaufspreis(i);
			}
			uiObjects[18 + i].GetComponent<Text>().text = GetMoneyString(mS_.merchStandardpreis[i]);
			uiObjects[34 + i].GetComponent<Text>().text = GetGewinnArtikel(i);
			uiObjects[62 + i].GetComponent<Text>().text = GetSellsArtikel(i);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		BUTTON_Abbrechen();
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
		mS_.merchStandardpreis[i] -= 1f;
		if (mS_.merchStandardpreis[i] <= GetMindestVerkaufspreis(i))
		{
			mS_.merchStandardpreis[i] = GetMindestVerkaufspreis(i);
		}
		if (mS_.merchStandardpreis[i] > 29.99f)
		{
			mS_.merchStandardpreis[i] = 29.99f;
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
		mS_.merchStandardpreis[i] += 1f;
		if (mS_.merchStandardpreis[i] <= GetMindestVerkaufspreis(i))
		{
			mS_.merchStandardpreis[i] = GetMindestVerkaufspreis(i);
		}
		if (mS_.merchStandardpreis[i] > 29.99f)
		{
			mS_.merchStandardpreis[i] = 29.99f;
		}
		StartCoroutine(iPlusVerkaufspreis(i));
		SetData();
	}

	public float GetMindestVerkaufspreis(int i)
	{
		return (float)Mathf.RoundToInt(menu_.einkaufspreis[i]) + 0.99f;
	}

	public void BUTTON_GlobalVerkaufspreis()
	{
		sfx_.PlaySound(3, force: true);
		int num = 1;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num++;
				for (int j = 0; j < mS_.games_.arrayGamesScripts[i].merchVerkaufspreis.Length; j++)
				{
					mS_.games_.arrayGamesScripts[i].merchVerkaufspreis[j] = mS_.merchStandardpreis[j];
				}
			}
		}
		string text = tS_.GetText(1840);
		text = text.Replace("<NUM>", "<color=blue>" + num + "</color>");
		guiMain_.MessageBox(text, closeMenu: false);
	}

	private string GetGewinnArtikel(int artikel_)
	{
		long num = 0L;
		long num2 = 0L;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num += mS_.games_.arrayGamesScripts[i].merchDiesenMonat[artikel_];
				num2 += mS_.games_.arrayGamesScripts[i].merchLetzterMonat[artikel_];
			}
		}
		return mS_.GetMoney(num, showDollar: true) + " | " + mS_.GetMoney(num2, showDollar: true);
	}

	private string GetSellsArtikel(int artikel_)
	{
		long num = 0L;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num += mS_.games_.arrayGamesScripts[i].merchGesamtSells[artikel_];
			}
		}
		return mS_.GetMoney(num, showDollar: false);
	}

	private string GetGewinnAll()
	{
		long num = 0L;
		long num2 = 0L;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num += mS_.games_.arrayGamesScripts[i].merchGewinnDiesenMonat;
				num2 += mS_.games_.arrayGamesScripts[i].merchGewinnLetzterMonat;
			}
		}
		return mS_.GetMoney(num, showDollar: true) + " | " + mS_.GetMoney(num2, showDollar: true);
	}

	private string GetGesamtGewinnAll()
	{
		long num = 0L;
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)mS_.games_.arrayGamesScripts[i] && mS_.games_.arrayGamesScripts[i].ownerID == mS_.myID && mS_.games_.arrayGamesScripts[i].mainIP == mS_.games_.arrayGamesScripts[i].myID)
			{
				num += mS_.games_.arrayGamesScripts[i].merchGesamtGewinn;
			}
		}
		return mS_.GetMoney(num, showDollar: true);
	}
}
