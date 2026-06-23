using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Packung : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private Menu_DevGame mDevGame_;

	private genres genres_;

	private games games_;

	private unlockScript unlock_;

	private gameScript gS_;

	private taskGame task_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] packObjects;

	public bool[] standard_edition;

	public bool[] deluxe_edition;

	public bool[] collectors_edition;

	public int[] verkaufspreis;

	public int[] verkaufspreis_default_bundle;

	public int[] verkaufspreis_default_bundleAddon;

	public int[] verkaufspreis_default_addon;

	public int[] verkaufspreis_default_budget;

	public int[] verkaufspreis_default_goty;

	public int[] verkaufspreis_default_standard;

	public bool[] standard_default_bundleAddon;

	public bool[] deluxe_default_bundleAddon;

	public bool[] collectors_default_bundleAddon;

	public bool[] standard_default_bundle;

	public bool[] deluxe_default_bundle;

	public bool[] collectors_default_bundle;

	public bool[] standard_default_addon;

	public bool[] deluxe_default_addon;

	public bool[] collectors_default_addon;

	public bool[] standard_default_budget;

	public bool[] deluxe_default_budget;

	public bool[] collectors_default_budget;

	public bool[] standard_default_goty;

	public bool[] deluxe_default_goty;

	public bool[] collectors_default_goty;

	public bool[] standard_default_standard;

	public bool[] deluxe_default_standard;

	public bool[] collectors_default_standard;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
	}

	private void Update()
	{
		if (!gS_)
		{
			return;
		}
		UpdatePackung();
		uiObjects[22].GetComponent<Text>().text = GetMoneyString(GetProduktionskosten(0));
		uiObjects[23].GetComponent<Text>().text = GetMoneyString(GetProduktionskosten(1));
		uiObjects[24].GetComponent<Text>().text = GetMoneyString(GetProduktionskosten(2));
		uiObjects[25].GetComponent<Text>().text = "-";
		uiObjects[26].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis[0], showDollar: true);
		uiObjects[27].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis[1], showDollar: true);
		uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis[2], showDollar: true);
		uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(verkaufspreis[3], showDollar: true);
		uiObjects[34].GetComponent<Text>().text = "$" + Mathf.RoundToInt((float)verkaufspreis[0] * 1.4f) + ".99";
		uiObjects[35].GetComponent<Text>().text = "$" + Mathf.RoundToInt((float)verkaufspreis[1] * 1.4f) + ".99";
		uiObjects[36].GetComponent<Text>().text = "$" + Mathf.RoundToInt((float)verkaufspreis[2] * 1.4f) + ".99";
		uiObjects[37].GetComponent<Text>().text = "$" + Mathf.RoundToInt((float)verkaufspreis[3] * 1.4f) + ".99";
		verkaufspreis[3] = verkaufspreis[0];
		if (verkaufspreis[1] <= verkaufspreis[0] + 10)
		{
			verkaufspreis[1] = verkaufspreis[0] + 10;
		}
		if (verkaufspreis[2] <= verkaufspreis[1] + 10)
		{
			verkaufspreis[2] = verkaufspreis[1] + 10;
		}
		uiObjects[30].GetComponent<Text>().text = GetMoneyString(GetGewinn(0));
		uiObjects[31].GetComponent<Text>().text = GetMoneyString(GetGewinn(1));
		uiObjects[32].GetComponent<Text>().text = GetMoneyString(GetGewinn(2));
		uiObjects[33].GetComponent<Text>().text = GetMoneyString(GetGewinn(3));
		if (gS_.pubOffer)
		{
			Text component = uiObjects[30].GetComponent<Text>();
			component.text = component.text + "<color=red> (-" + Mathf.RoundToInt(gS_.PUBOFFER_GetGewinnbeteiligung()) + "%)</color>";
			Text component2 = uiObjects[31].GetComponent<Text>();
			component2.text = component2.text + "<color=red> (-" + Mathf.RoundToInt(gS_.PUBOFFER_GetGewinnbeteiligung()) + "%)</color>";
			Text component3 = uiObjects[32].GetComponent<Text>();
			component3.text = component3.text + "<color=red> (-" + Mathf.RoundToInt(gS_.PUBOFFER_GetGewinnbeteiligung()) + "%)</color>";
			Text component4 = uiObjects[33].GetComponent<Text>();
			component4.text = component4.text + "<color=red> (-" + Mathf.RoundToInt(gS_.PUBOFFER_GetGewinnbeteiligung()) + "%)</color>";
		}
		if (GetGewinn(0) >= 0f)
		{
			uiObjects[30].GetComponent<Text>().color = guiMain_.colors[13];
		}
		else
		{
			uiObjects[30].GetComponent<Text>().color = guiMain_.colors[5];
		}
		if (GetGewinn(1) >= 0f)
		{
			uiObjects[31].GetComponent<Text>().color = guiMain_.colors[13];
		}
		else
		{
			uiObjects[31].GetComponent<Text>().color = guiMain_.colors[5];
		}
		if (GetGewinn(2) >= 0f)
		{
			uiObjects[32].GetComponent<Text>().color = guiMain_.colors[13];
		}
		else
		{
			uiObjects[32].GetComponent<Text>().color = guiMain_.colors[5];
		}
		if (GetGewinn(3) >= 0f)
		{
			uiObjects[33].GetComponent<Text>().color = guiMain_.colors[13];
		}
		else
		{
			uiObjects[33].GetComponent<Text>().color = guiMain_.colors[5];
		}
		if (verkaufspreis[0] <= 5)
		{
			uiObjects[45].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[45].GetComponent<Button>().interactable = true;
		}
		if (verkaufspreis[0] >= 79)
		{
			uiObjects[48].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[48].GetComponent<Button>().interactable = true;
		}
		if (!gS_.typ_budget && !gS_.typ_bundle && !gS_.typ_goty && !gS_.typ_addon && !gS_.typ_mmoaddon && !gS_.typ_addonStandalone)
		{
			if (verkaufspreis[1] <= verkaufspreis[0] + 10)
			{
				uiObjects[46].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[46].GetComponent<Button>().interactable = true;
			}
			if (verkaufspreis[1] >= 89)
			{
				uiObjects[49].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[49].GetComponent<Button>().interactable = true;
			}
			if (verkaufspreis[2] <= verkaufspreis[1] + 10)
			{
				uiObjects[47].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[47].GetComponent<Button>().interactable = true;
			}
			if (verkaufspreis[2] >= 99)
			{
				uiObjects[50].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[50].GetComponent<Button>().interactable = true;
			}
		}
		if (verkaufspreis[3] <= 5)
		{
			uiObjects[53].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[53].GetComponent<Button>().interactable = true;
		}
		if (verkaufspreis[3] >= 79)
		{
			uiObjects[54].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[54].GetComponent<Button>().interactable = true;
		}
		if (gS_.typ_budget || gS_.typ_bundle || gS_.typ_goty || gS_.typ_addon || gS_.typ_mmoaddon || gS_.typ_addonStandalone)
		{
			uiObjects[46].GetComponent<Button>().interactable = false;
			uiObjects[49].GetComponent<Button>().interactable = false;
			uiObjects[47].GetComponent<Button>().interactable = false;
			uiObjects[50].GetComponent<Button>().interactable = false;
			uiObjects[23].GetComponent<Text>().text = "-";
			uiObjects[27].GetComponent<Text>().text = "-";
			uiObjects[35].GetComponent<Text>().text = "-";
			uiObjects[31].GetComponent<Text>().text = "-";
			uiObjects[24].GetComponent<Text>().text = "-";
			uiObjects[28].GetComponent<Text>().text = "-";
			uiObjects[36].GetComponent<Text>().text = "-";
			uiObjects[32].GetComponent<Text>().text = "-";
		}
		if (uiObjects[39].GetComponent<Toggle>().isOn)
		{
			uiObjects[51].GetComponent<Image>().color = Color.white;
			uiObjects[52].GetComponent<Image>().color = Color.white;
		}
		else
		{
			uiObjects[51].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
			uiObjects[52].GetComponent<Image>().color = new Color(0f, 0f, 0f, 0f);
		}
	}

	private float GetGewinn(int i)
	{
		float num = (float)verkaufspreis[i] - GetProduktionskosten(i);
		if (gS_.pubOffer && gS_.PUBOFFER_GetGewinnbeteiligung() > 0)
		{
			num = gS_.SubGewinnbeteiligung(Mathf.RoundToInt(num));
		}
		return num;
	}

	public void Init(gameScript game_, taskGame t_, bool newGame, bool hideClose)
	{
		FindScripts();
		gS_ = game_;
		task_ = t_;
		if (hideClose)
		{
			uiObjects[56].SetActive(value: false);
		}
		else
		{
			uiObjects[56].SetActive(value: true);
		}
		InitDropdowns();
		uiObjects[39].GetComponent<Toggle>().interactable = true;
		uiObjects[42].GetComponent<Toggle>().interactable = true;
		Unlock(59, uiObjects[38], uiObjects[42]);
		uiObjects[61].SetActive(value: false);
		if (uiObjects[42].GetComponent<Toggle>().interactable && !gS_.HatEinePlattformInternet())
		{
			uiObjects[61].SetActive(value: true);
			uiObjects[42].GetComponent<Toggle>().interactable = false;
			uiObjects[42].GetComponent<Toggle>().isOn = false;
		}
		uiObjects[43].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[51].GetComponent<Image>().sprite = guiMain_.uiSprites[27];
		uiObjects[52].GetComponent<Image>().sprite = guiMain_.uiSprites[27];
		if (game_.typ_budget || game_.typ_bundle || gS_.typ_goty || gS_.typ_addon || gS_.typ_mmoaddon || gS_.typ_addonStandalone)
		{
			uiObjects[51].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[52].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		}
		if (newGame)
		{
			uiObjects[39].GetComponent<Toggle>().isOn = true;
			if (uiObjects[42].GetComponent<Toggle>().interactable)
			{
				uiObjects[42].GetComponent<Toggle>().isOn = true;
			}
			verkaufspreis[0] = 29;
			verkaufspreis[1] = 39;
			verkaufspreis[2] = 49;
			verkaufspreis[3] = 29;
			for (int i = 0; i < standard_edition.Length; i++)
			{
				standard_edition[i] = false;
				deluxe_edition[i] = false;
				collectors_edition[i] = false;
			}
			standard_edition[0] = true;
			deluxe_edition[1] = true;
			deluxe_edition[2] = true;
			deluxe_edition[3] = true;
			deluxe_edition[4] = true;
			collectors_edition[1] = true;
			collectors_edition[2] = true;
			collectors_edition[3] = true;
			collectors_edition[4] = true;
			collectors_edition[5] = true;
			collectors_edition[6] = true;
			collectors_edition[7] = true;
			collectors_edition[8] = true;
			standard_edition[0] = true;
			deluxe_edition[0] = true;
			collectors_edition[0] = true;
			if (game_.typ_budget)
			{
				verkaufspreis[0] = 9;
				verkaufspreis[1] = 0;
				verkaufspreis[2] = 0;
				verkaufspreis[3] = 9;
				for (int j = 0; j < standard_edition.Length; j++)
				{
					deluxe_edition[j] = false;
					collectors_edition[j] = false;
				}
			}
			if (game_.typ_bundle)
			{
				verkaufspreis[0] = 29;
				verkaufspreis[1] = 0;
				verkaufspreis[2] = 0;
				verkaufspreis[3] = 29;
				for (int k = 0; k < standard_edition.Length; k++)
				{
					deluxe_edition[k] = false;
					collectors_edition[k] = false;
				}
			}
			if (game_.typ_bundleAddon)
			{
				verkaufspreis[0] = 29;
				verkaufspreis[1] = 39;
				verkaufspreis[2] = 49;
				verkaufspreis[3] = 29;
			}
			if (game_.typ_goty || game_.typ_addon || game_.typ_addonStandalone || game_.typ_mmoaddon)
			{
				verkaufspreis[0] = 19;
				verkaufspreis[1] = 0;
				verkaufspreis[2] = 0;
				verkaufspreis[3] = 19;
				for (int l = 0; l < standard_edition.Length; l++)
				{
					deluxe_edition[l] = false;
					collectors_edition[l] = false;
				}
			}
			LoadData();
		}
		else
		{
			uiObjects[39].GetComponent<Toggle>().isOn = game_.retailVersion;
			uiObjects[42].GetComponent<Toggle>().isOn = game_.digitalVersion;
			standard_edition = (bool[])gS_.standard_edition.Clone();
			deluxe_edition = (bool[])gS_.deluxe_edition.Clone();
			collectors_edition = (bool[])gS_.collectors_edition.Clone();
			verkaufspreis = (int[])gS_.verkaufspreis.Clone();
			uiObjects[58].GetComponent<Toggle>().isOn = gS_.autoPreis;
		}
		uiObjects[21].GetComponent<Text>().text = GetMoneyString(games_.GetGrundkosten());
		uiObjects[0].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[0]);
		uiObjects[1].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[1]);
		uiObjects[2].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[2]);
		uiObjects[3].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[3]);
		uiObjects[4].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[4]);
		uiObjects[5].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[5]);
		uiObjects[6].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[6]);
		uiObjects[7].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[7]);
		uiObjects[8].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[8]);
		uiObjects[9].GetComponent<Text>().text = GetMoneyString(games_.preise_inhalt[9]);
		uiObjects[10].GetComponent<Toggle>().isOn = standard_edition[0];
		uiObjects[11].GetComponent<Toggle>().isOn = standard_edition[1];
		uiObjects[12].GetComponent<Toggle>().isOn = standard_edition[2];
		uiObjects[13].GetComponent<Toggle>().isOn = standard_edition[3];
		uiObjects[14].GetComponent<Toggle>().isOn = standard_edition[4];
		uiObjects[15].GetComponent<Toggle>().isOn = standard_edition[5];
		uiObjects[16].GetComponent<Toggle>().isOn = standard_edition[6];
		uiObjects[17].GetComponent<Toggle>().isOn = standard_edition[7];
		uiObjects[18].GetComponent<Toggle>().isOn = standard_edition[8];
		uiObjects[19].GetComponent<Toggle>().isOn = standard_edition[9];
		uiObjects[59].SetActive(value: false);
		uiObjects[60].SetActive(value: false);
		if (gS_.pubOffer)
		{
			if (!gS_.pubAngebot_Retail)
			{
				uiObjects[59].SetActive(value: true);
				uiObjects[39].GetComponent<Toggle>().isOn = false;
				uiObjects[39].GetComponent<Toggle>().interactable = false;
			}
			if (!gS_.pubAngebot_Digital)
			{
				uiObjects[60].SetActive(value: true);
				uiObjects[38].SetActive(value: false);
				uiObjects[61].SetActive(value: false);
				uiObjects[42].GetComponent<Toggle>().isOn = false;
				uiObjects[42].GetComponent<Toggle>().interactable = false;
			}
		}
	}

	private void Unlock(int id_, GameObject lock_, GameObject toggle_)
	{
		if (unlock_.unlock[id_])
		{
			toggle_.GetComponent<Toggle>().interactable = true;
			lock_.SetActive(value: false);
		}
		else
		{
			toggle_.GetComponent<Toggle>().interactable = false;
			lock_.SetActive(value: true);
		}
	}

	private string GetMoneyString(float f)
	{
		string text = "$" + mS_.Round(f, 2);
		text = text.Replace(",", ".");
		if (text.Length == 2)
		{
			text += ".00";
		}
		if (text[text.Length - 2] == '.')
		{
			text += "0";
		}
		return text;
	}

	private float GetProduktionskosten(int edition)
	{
		float num = 0f;
		switch (edition)
		{
		case 0:
		{
			for (int j = 0; j < standard_edition.Length; j++)
			{
				if (standard_edition[j])
				{
					num += games_.preise_inhalt[j];
				}
			}
			break;
		}
		case 1:
		{
			for (int k = 0; k < deluxe_edition.Length; k++)
			{
				if (deluxe_edition[k])
				{
					num += games_.preise_inhalt[k];
				}
			}
			break;
		}
		case 2:
		{
			for (int i = 0; i < collectors_edition.Length; i++)
			{
				if (collectors_edition[i])
				{
					num += games_.preise_inhalt[i];
				}
			}
			break;
		}
		case 3:
			return 0f;
		}
		return num + games_.GetGrundkosten();
	}

	public void InitDropdowns()
	{
		List<string> list = new List<string>();
		list.Add(tS_.GetText(1103));
		if (!gS_.typ_budget && !gS_.typ_bundle && !gS_.typ_goty && !gS_.typ_addon && !gS_.typ_mmoaddon && !gS_.typ_addonStandalone)
		{
			list.Add(tS_.GetText(1104));
			list.Add(tS_.GetText(1105));
		}
		uiObjects[20].GetComponent<Dropdown>().ClearOptions();
		uiObjects[20].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[20].GetComponent<Dropdown>().value = 0;
		DROPDOWN_Edition();
	}

	public void TOGGLE_ManualSW()
	{
		if (uiObjects[11].GetComponent<Toggle>().isOn)
		{
			uiObjects[11].GetComponent<Toggle>().isOn = false;
		}
	}

	public void TOGGLE_ManualColor()
	{
		if (uiObjects[10].GetComponent<Toggle>().isOn)
		{
			uiObjects[10].GetComponent<Toggle>().isOn = false;
		}
	}

	private void UpdatePackung()
	{
		if (packObjects[0].activeSelf != uiObjects[10].GetComponent<Toggle>().isOn)
		{
			packObjects[0].SetActive(uiObjects[10].GetComponent<Toggle>().isOn);
		}
		if (packObjects[1].activeSelf != uiObjects[11].GetComponent<Toggle>().isOn)
		{
			packObjects[1].SetActive(uiObjects[11].GetComponent<Toggle>().isOn);
		}
		if (packObjects[2].activeSelf != uiObjects[12].GetComponent<Toggle>().isOn)
		{
			packObjects[2].SetActive(uiObjects[12].GetComponent<Toggle>().isOn);
		}
		if (packObjects[3].activeSelf != uiObjects[13].GetComponent<Toggle>().isOn)
		{
			packObjects[3].SetActive(uiObjects[13].GetComponent<Toggle>().isOn);
		}
		if (packObjects[4].activeSelf != uiObjects[14].GetComponent<Toggle>().isOn)
		{
			packObjects[4].SetActive(uiObjects[14].GetComponent<Toggle>().isOn);
		}
		if (packObjects[5].activeSelf != uiObjects[15].GetComponent<Toggle>().isOn)
		{
			packObjects[5].SetActive(uiObjects[15].GetComponent<Toggle>().isOn);
		}
		if (packObjects[6].activeSelf != uiObjects[16].GetComponent<Toggle>().isOn)
		{
			packObjects[6].SetActive(uiObjects[16].GetComponent<Toggle>().isOn);
		}
		if (packObjects[7].activeSelf != uiObjects[17].GetComponent<Toggle>().isOn)
		{
			packObjects[7].SetActive(uiObjects[17].GetComponent<Toggle>().isOn);
		}
		if (mS_.year <= 1990)
		{
			if (packObjects[8].activeSelf != uiObjects[18].GetComponent<Toggle>().isOn)
			{
				packObjects[8].SetActive(uiObjects[18].GetComponent<Toggle>().isOn);
				packObjects[9].SetActive(value: false);
			}
		}
		else if (packObjects[9].activeSelf != uiObjects[18].GetComponent<Toggle>().isOn)
		{
			packObjects[9].SetActive(uiObjects[18].GetComponent<Toggle>().isOn);
			packObjects[8].SetActive(value: false);
		}
		if (packObjects[10].activeSelf != uiObjects[19].GetComponent<Toggle>().isOn)
		{
			packObjects[10].SetActive(uiObjects[19].GetComponent<Toggle>().isOn);
		}
		switch (uiObjects[20].GetComponent<Dropdown>().value)
		{
		case 0:
			standard_edition[0] = uiObjects[10].GetComponent<Toggle>().isOn;
			standard_edition[1] = uiObjects[11].GetComponent<Toggle>().isOn;
			standard_edition[2] = uiObjects[12].GetComponent<Toggle>().isOn;
			standard_edition[3] = uiObjects[13].GetComponent<Toggle>().isOn;
			standard_edition[4] = uiObjects[14].GetComponent<Toggle>().isOn;
			standard_edition[5] = uiObjects[15].GetComponent<Toggle>().isOn;
			standard_edition[6] = uiObjects[16].GetComponent<Toggle>().isOn;
			standard_edition[7] = uiObjects[17].GetComponent<Toggle>().isOn;
			standard_edition[8] = uiObjects[18].GetComponent<Toggle>().isOn;
			standard_edition[9] = uiObjects[19].GetComponent<Toggle>().isOn;
			break;
		case 1:
			deluxe_edition[0] = uiObjects[10].GetComponent<Toggle>().isOn;
			deluxe_edition[1] = uiObjects[11].GetComponent<Toggle>().isOn;
			deluxe_edition[2] = uiObjects[12].GetComponent<Toggle>().isOn;
			deluxe_edition[3] = uiObjects[13].GetComponent<Toggle>().isOn;
			deluxe_edition[4] = uiObjects[14].GetComponent<Toggle>().isOn;
			deluxe_edition[5] = uiObjects[15].GetComponent<Toggle>().isOn;
			deluxe_edition[6] = uiObjects[16].GetComponent<Toggle>().isOn;
			deluxe_edition[7] = uiObjects[17].GetComponent<Toggle>().isOn;
			deluxe_edition[8] = uiObjects[18].GetComponent<Toggle>().isOn;
			deluxe_edition[9] = uiObjects[19].GetComponent<Toggle>().isOn;
			break;
		case 2:
			collectors_edition[0] = uiObjects[10].GetComponent<Toggle>().isOn;
			collectors_edition[1] = uiObjects[11].GetComponent<Toggle>().isOn;
			collectors_edition[2] = uiObjects[12].GetComponent<Toggle>().isOn;
			collectors_edition[3] = uiObjects[13].GetComponent<Toggle>().isOn;
			collectors_edition[4] = uiObjects[14].GetComponent<Toggle>().isOn;
			collectors_edition[5] = uiObjects[15].GetComponent<Toggle>().isOn;
			collectors_edition[6] = uiObjects[16].GetComponent<Toggle>().isOn;
			collectors_edition[7] = uiObjects[17].GetComponent<Toggle>().isOn;
			collectors_edition[8] = uiObjects[18].GetComponent<Toggle>().isOn;
			collectors_edition[9] = uiObjects[19].GetComponent<Toggle>().isOn;
			break;
		}
		for (int i = 2; i < standard_edition.Length; i++)
		{
			if (standard_edition[i])
			{
				deluxe_edition[i] = true;
			}
			if (deluxe_edition[i])
			{
				collectors_edition[i] = true;
			}
		}
		if (standard_edition[1])
		{
			deluxe_edition[0] = false;
			deluxe_edition[1] = true;
		}
		if (deluxe_edition[1])
		{
			collectors_edition[0] = false;
			collectors_edition[1] = true;
		}
		if (gS_.lagerbestand[0] > 0 || gS_.lagerbestand[1] > 0 || gS_.lagerbestand[2] > 0 || gS_.typ_budget || gS_.typ_bundle)
		{
			if (!uiObjects[55].activeSelf)
			{
				uiObjects[55].SetActive(value: true);
			}
			uiObjects[57].GetComponent<Text>().text = tS_.GetText(1140);
			if (gS_.typ_budget)
			{
				uiObjects[57].GetComponent<Text>().text = tS_.GetText(1159);
			}
			if (gS_.typ_bundle)
			{
				uiObjects[57].GetComponent<Text>().text = tS_.GetText(1347);
			}
			for (int j = 0; j < standard_edition.Length; j++)
			{
				uiObjects[10 + j].GetComponent<Toggle>().interactable = false;
			}
			return;
		}
		if (uiObjects[55].activeSelf)
		{
			uiObjects[55].SetActive(value: false);
		}
		for (int k = 1; k < standard_edition.Length; k++)
		{
			switch (uiObjects[20].GetComponent<Dropdown>().value)
			{
			case 0:
				uiObjects[10 + k].GetComponent<Toggle>().interactable = true;
				break;
			case 1:
				if (standard_edition[k])
				{
					uiObjects[10 + k].GetComponent<Toggle>().interactable = false;
				}
				else
				{
					uiObjects[10 + k].GetComponent<Toggle>().interactable = true;
				}
				break;
			case 2:
				if (standard_edition[k] || deluxe_edition[k])
				{
					uiObjects[10 + k].GetComponent<Toggle>().interactable = false;
				}
				else
				{
					uiObjects[10 + k].GetComponent<Toggle>().interactable = true;
				}
				break;
			}
		}
		switch (uiObjects[20].GetComponent<Dropdown>().value)
		{
		case 0:
			uiObjects[10].GetComponent<Toggle>().interactable = true;
			break;
		case 1:
			if (standard_edition[1])
			{
				uiObjects[10].GetComponent<Toggle>().interactable = false;
			}
			else
			{
				uiObjects[10].GetComponent<Toggle>().interactable = true;
			}
			break;
		case 2:
			if (deluxe_edition[1])
			{
				uiObjects[10].GetComponent<Toggle>().interactable = false;
			}
			else
			{
				uiObjects[10].GetComponent<Toggle>().interactable = true;
			}
			break;
		}
	}

	public void DROPDOWN_Edition()
	{
		switch (uiObjects[20].GetComponent<Dropdown>().value)
		{
		case 0:
			uiObjects[10].GetComponent<Toggle>().isOn = standard_edition[0];
			uiObjects[11].GetComponent<Toggle>().isOn = standard_edition[1];
			uiObjects[12].GetComponent<Toggle>().isOn = standard_edition[2];
			uiObjects[13].GetComponent<Toggle>().isOn = standard_edition[3];
			uiObjects[14].GetComponent<Toggle>().isOn = standard_edition[4];
			uiObjects[15].GetComponent<Toggle>().isOn = standard_edition[5];
			uiObjects[16].GetComponent<Toggle>().isOn = standard_edition[6];
			uiObjects[17].GetComponent<Toggle>().isOn = standard_edition[7];
			uiObjects[18].GetComponent<Toggle>().isOn = standard_edition[8];
			uiObjects[19].GetComponent<Toggle>().isOn = standard_edition[9];
			uiObjects[18].SetActive(value: false);
			uiObjects[19].SetActive(value: false);
			break;
		case 1:
			uiObjects[10].GetComponent<Toggle>().isOn = deluxe_edition[0];
			uiObjects[11].GetComponent<Toggle>().isOn = deluxe_edition[1];
			uiObjects[12].GetComponent<Toggle>().isOn = deluxe_edition[2];
			uiObjects[13].GetComponent<Toggle>().isOn = deluxe_edition[3];
			uiObjects[14].GetComponent<Toggle>().isOn = deluxe_edition[4];
			uiObjects[15].GetComponent<Toggle>().isOn = deluxe_edition[5];
			uiObjects[16].GetComponent<Toggle>().isOn = deluxe_edition[6];
			uiObjects[17].GetComponent<Toggle>().isOn = deluxe_edition[7];
			uiObjects[18].GetComponent<Toggle>().isOn = deluxe_edition[8];
			uiObjects[19].GetComponent<Toggle>().isOn = deluxe_edition[9];
			uiObjects[18].SetActive(value: true);
			uiObjects[19].SetActive(value: false);
			break;
		case 2:
			uiObjects[10].GetComponent<Toggle>().isOn = collectors_edition[0];
			uiObjects[11].GetComponent<Toggle>().isOn = collectors_edition[1];
			uiObjects[12].GetComponent<Toggle>().isOn = collectors_edition[2];
			uiObjects[13].GetComponent<Toggle>().isOn = collectors_edition[3];
			uiObjects[14].GetComponent<Toggle>().isOn = collectors_edition[4];
			uiObjects[15].GetComponent<Toggle>().isOn = collectors_edition[5];
			uiObjects[16].GetComponent<Toggle>().isOn = collectors_edition[6];
			uiObjects[17].GetComponent<Toggle>().isOn = collectors_edition[7];
			uiObjects[18].GetComponent<Toggle>().isOn = collectors_edition[8];
			uiObjects[19].GetComponent<Toggle>().isOn = collectors_edition[9];
			uiObjects[18].SetActive(value: true);
			uiObjects[19].SetActive(value: true);
			break;
		}
	}

	private IEnumerator iMinusPreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusPreis(i);
		}
	}

	public void BUTTON_MinusPreis(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			verkaufspreis[i] -= 10;
		}
		else
		{
			verkaufspreis[i]--;
		}
		if (verkaufspreis[0] < 5)
		{
			verkaufspreis[0] = 5;
		}
		if (verkaufspreis[1] < 6)
		{
			verkaufspreis[1] = 6;
		}
		if (verkaufspreis[2] < 7)
		{
			verkaufspreis[2] = 7;
		}
		if (verkaufspreis[3] < 5)
		{
			verkaufspreis[3] = 5;
		}
		StartCoroutine(iMinusPreis(i));
	}

	private IEnumerator iPlusPreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusPreis(i);
		}
	}

	public void BUTTON_PlusPreis(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			verkaufspreis[i] += 10;
		}
		else
		{
			verkaufspreis[i]++;
		}
		if (gS_.typ_budget)
		{
			if (verkaufspreis[0] > 10)
			{
				verkaufspreis[0] = 10;
			}
			if (verkaufspreis[3] > 10)
			{
				verkaufspreis[3] = 10;
			}
			StartCoroutine(iPlusPreis(i));
		}
		else if (gS_.typ_goty)
		{
			if (verkaufspreis[0] > 19)
			{
				verkaufspreis[0] = 19;
			}
			if (verkaufspreis[1] > 29)
			{
				verkaufspreis[1] = 29;
			}
			if (verkaufspreis[2] > 39)
			{
				verkaufspreis[2] = 39;
			}
			if (verkaufspreis[3] > 19)
			{
				verkaufspreis[3] = 19;
			}
			StartCoroutine(iPlusPreis(i));
		}
		else if (gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_mmoaddon)
		{
			if (verkaufspreis[0] > 29)
			{
				verkaufspreis[0] = 29;
			}
			if (verkaufspreis[1] > 39)
			{
				verkaufspreis[1] = 39;
			}
			if (verkaufspreis[2] > 49)
			{
				verkaufspreis[2] = 49;
			}
			if (verkaufspreis[3] > 29)
			{
				verkaufspreis[3] = 29;
			}
			StartCoroutine(iPlusPreis(i));
		}
		else
		{
			if (verkaufspreis[0] > 79)
			{
				verkaufspreis[0] = 79;
			}
			if (verkaufspreis[1] > 89)
			{
				verkaufspreis[1] = 89;
			}
			if (verkaufspreis[2] > 99)
			{
				verkaufspreis[2] = 99;
			}
			if (verkaufspreis[3] > 79)
			{
				verkaufspreis[3] = 79;
			}
			StartCoroutine(iPlusPreis(i));
		}
	}

	private float GetQualityPackung(int i)
	{
		return (GetProduktionskosten(i) - games_.GetGrundkosten()) * 10f;
	}

	public void BUTTON_Close()
	{
		if (guiMain_.uiObjects[397].activeSelf)
		{
			guiMain_.uiObjects[397].GetComponent<Menu_Dev_TochterfirmaGameComplete>().SelfpublishGameAbbruch(gS_);
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[267].activeSelf)
		{
			for (int i = 0; i < gS_.bundleID.Length; i++)
			{
				GameObject gameObject = GameObject.Find("GAME_" + gS_.bundleID[i]);
				if ((bool)gameObject)
				{
					gameObject.GetComponent<gameScript>().bundle_created = false;
				}
			}
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[271].activeSelf)
		{
			for (int j = 0; j < gS_.bundleID.Length; j++)
			{
				GameObject gameObject2 = GameObject.Find("GAME_" + gS_.bundleID[j]);
				if ((bool)gameObject2)
				{
					gameObject2.GetComponent<gameScript>().bundle_created = false;
				}
			}
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[228].activeSelf)
		{
			GameObject gameObject3 = GameObject.Find("GAME_" + gS_.originalGameID);
			if ((bool)gameObject3)
			{
				gameObject3.GetComponent<gameScript>().budget_created = false;
			}
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
		else if (guiMain_.uiObjects[275].activeSelf)
		{
			GameObject gameObject4 = GameObject.Find("GAME_" + gS_.originalGameID);
			if ((bool)gameObject4)
			{
				gameObject4.GetComponent<gameScript>().goty_created = false;
			}
			gS_.gameObject.tag = "GameRemoved";
			Object.Destroy(gS_.gameObject);
			games_.FindGames();
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
		else
		{
			if (!guiMain_.uiObjects[220].activeSelf)
			{
				gS_.ClearReview();
				guiMain_.ActivateMenu(guiMain_.uiObjects[69]);
				guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().Init(gS_, task_);
			}
			sfx_.PlaySound(3, force: true);
			base.gameObject.SetActive(value: false);
		}
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (!uiObjects[39].GetComponent<Toggle>().isOn && !uiObjects[42].GetComponent<Toggle>().isOn)
		{
			guiMain_.MessageBox(tS_.GetText(1119), closeMenu: false);
			return;
		}
		if (!gS_.typ_budget && !gS_.typ_bundle && !gS_.typ_goty && !gS_.typ_addon && !gS_.typ_mmoaddon && !gS_.typ_addonStandalone && uiObjects[39].GetComponent<Toggle>().isOn)
		{
			if (GetProduktionskosten(1) <= GetProduktionskosten(0))
			{
				guiMain_.MessageBox(tS_.GetText(1120), closeMenu: false);
				return;
			}
			if (GetProduktionskosten(2) <= GetProduktionskosten(0))
			{
				guiMain_.MessageBox(tS_.GetText(1121), closeMenu: false);
				return;
			}
			if (GetProduktionskosten(2) <= GetProduktionskosten(1))
			{
				guiMain_.MessageBox(tS_.GetText(1121), closeMenu: false);
				return;
			}
		}
		int num = 0;
		for (int i = 0; i < gS_.verkaufspreis.Length; i++)
		{
			num += verkaufspreis[i] - gS_.verkaufspreis[i];
		}
		gS_.standard_edition = (bool[])standard_edition.Clone();
		gS_.deluxe_edition = (bool[])deluxe_edition.Clone();
		gS_.collectors_edition = (bool[])collectors_edition.Clone();
		gS_.verkaufspreis = (int[])verkaufspreis.Clone();
		gS_.digitalVersion = uiObjects[42].GetComponent<Toggle>().isOn;
		gS_.retailVersion = uiObjects[39].GetComponent<Toggle>().isOn;
		gS_.autoPreis = uiObjects[58].GetComponent<Toggle>().isOn;
		if (!guiMain_.uiObjects[220].activeSelf)
		{
			SaveData();
			guiMain_.ActivateMenu(guiMain_.uiObjects[219]);
			guiMain_.uiObjects[219].GetComponent<Menu_ReleaseDate>().Init(gS_, task_);
			return;
		}
		base.gameObject.SetActive(value: false);
		if (gS_.vorbestellungen <= 0 || num <= 0)
		{
			return;
		}
		int num2 = gS_.vorbestellungen / 100 * num;
		if (num2 > gS_.vorbestellungen)
		{
			num2 = gS_.vorbestellungen;
		}
		if (num2 > 0)
		{
			gS_.vorbestellungen -= num2;
			if (gS_.vorbestellungen < 0)
			{
				gS_.vorbestellungen = 0;
			}
			string text = tS_.GetText(2013);
			text = text.Replace("<NUM>", "<color=red>" + mS_.GetMoney(num2, showDollar: false) + "</color>");
			guiMain_.MessageBox(text, closeMenu: false);
		}
	}

	private void SaveData()
	{
		if (gS_.typ_bundle)
		{
			verkaufspreis_default_bundle = (int[])verkaufspreis.Clone();
			standard_default_bundle = (bool[])standard_edition.Clone();
			deluxe_default_bundle = (bool[])deluxe_edition.Clone();
			collectors_default_bundle = (bool[])collectors_edition.Clone();
		}
		else if (gS_.typ_bundleAddon)
		{
			verkaufspreis_default_bundleAddon = (int[])verkaufspreis.Clone();
			standard_default_bundleAddon = (bool[])standard_edition.Clone();
			deluxe_default_bundleAddon = (bool[])deluxe_edition.Clone();
			collectors_default_bundleAddon = (bool[])collectors_edition.Clone();
		}
		else if (gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_mmoaddon)
		{
			verkaufspreis_default_addon = (int[])verkaufspreis.Clone();
			standard_default_addon = (bool[])standard_edition.Clone();
			deluxe_default_addon = (bool[])deluxe_edition.Clone();
			collectors_default_addon = (bool[])collectors_edition.Clone();
		}
		else if (gS_.typ_budget)
		{
			verkaufspreis_default_budget = (int[])verkaufspreis.Clone();
			standard_default_budget = (bool[])standard_edition.Clone();
			deluxe_default_budget = (bool[])deluxe_edition.Clone();
			collectors_default_budget = (bool[])collectors_edition.Clone();
		}
		else if (gS_.typ_goty)
		{
			verkaufspreis_default_goty = (int[])verkaufspreis.Clone();
			standard_default_goty = (bool[])standard_edition.Clone();
			deluxe_default_goty = (bool[])deluxe_edition.Clone();
			collectors_default_goty = (bool[])collectors_edition.Clone();
		}
		else
		{
			verkaufspreis_default_standard = (int[])verkaufspreis.Clone();
			standard_default_standard = (bool[])standard_edition.Clone();
			deluxe_default_standard = (bool[])deluxe_edition.Clone();
			collectors_default_standard = (bool[])collectors_edition.Clone();
		}
	}

	private void LoadData()
	{
		if (gS_.typ_bundle)
		{
			if (verkaufspreis_default_bundle[0] != 0)
			{
				verkaufspreis = (int[])verkaufspreis_default_bundle.Clone();
				standard_edition = (bool[])standard_default_bundle.Clone();
				deluxe_edition = (bool[])deluxe_default_bundle.Clone();
				collectors_edition = (bool[])collectors_default_bundle.Clone();
			}
		}
		else if (gS_.typ_bundleAddon)
		{
			if (verkaufspreis_default_bundleAddon[0] != 0)
			{
				verkaufspreis = (int[])verkaufspreis_default_bundleAddon.Clone();
				standard_edition = (bool[])standard_default_bundleAddon.Clone();
				deluxe_edition = (bool[])deluxe_default_bundleAddon.Clone();
				collectors_edition = (bool[])collectors_default_bundleAddon.Clone();
			}
		}
		else if (gS_.typ_addon || gS_.typ_addonStandalone || gS_.typ_mmoaddon)
		{
			if (verkaufspreis_default_addon[0] != 0)
			{
				verkaufspreis = (int[])verkaufspreis_default_addon.Clone();
				standard_edition = (bool[])standard_default_addon.Clone();
				deluxe_edition = (bool[])deluxe_default_addon.Clone();
				collectors_edition = (bool[])collectors_default_addon.Clone();
			}
		}
		else if (gS_.typ_budget)
		{
			if (verkaufspreis_default_budget[0] != 0)
			{
				verkaufspreis = (int[])verkaufspreis_default_budget.Clone();
				standard_edition = (bool[])standard_default_budget.Clone();
				deluxe_edition = (bool[])deluxe_default_budget.Clone();
				collectors_edition = (bool[])collectors_default_budget.Clone();
			}
		}
		else if (gS_.typ_goty)
		{
			if (verkaufspreis_default_goty[0] != 0)
			{
				verkaufspreis = (int[])verkaufspreis_default_goty.Clone();
				standard_edition = (bool[])standard_default_goty.Clone();
				deluxe_edition = (bool[])deluxe_default_goty.Clone();
				collectors_edition = (bool[])collectors_default_goty.Clone();
			}
		}
		else if (verkaufspreis_default_standard[0] != 0)
		{
			verkaufspreis = (int[])verkaufspreis_default_standard.Clone();
			standard_edition = (bool[])standard_default_standard.Clone();
			deluxe_edition = (bool[])deluxe_default_standard.Clone();
			collectors_edition = (bool[])collectors_default_standard.Clone();
		}
	}

	public void TOGGLE_Autopreis()
	{
		if (uiObjects[58].GetComponent<Toggle>().isOn)
		{
			gS_.UpdateAutoPreis();
			verkaufspreis = (int[])gS_.verkaufspreis.Clone();
		}
	}
}
