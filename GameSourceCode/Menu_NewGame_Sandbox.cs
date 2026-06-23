using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_NewGame_Sandbox : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	public int logo = -1;

	public int country;

	public int genre;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!genres_)
			{
				genres_ = main_.GetComponent<genres>();
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
	}

	private void OnEnable()
	{
		FindScripts();
		if (logo == -1)
		{
			logo = Random.Range(0, 30);
			if (PlayerPrefs.HasKey("optLogo"))
			{
				logo = PlayerPrefs.GetInt("optLogo");
			}
		}
		if (PlayerPrefs.HasKey("optCountry"))
		{
			country = PlayerPrefs.GetInt("optCountry");
		}
		if (PlayerPrefs.HasKey("optGenre"))
		{
			genre = PlayerPrefs.GetInt("optGenre");
		}
		if (PlayerPrefs.HasKey("CompanyName"))
		{
			uiObjects[0].GetComponent<InputField>().text = PlayerPrefs.GetString("CompanyName");
		}
		Init();
		cmS_.disableMovement = true;
	}

	private void Update()
	{
		if (uiObjects[28].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[29].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init()
	{
		InitDropdowns();
		SetLogo(logo);
		SetCountry(country);
		SetGenre(genre);
	}

	private void OnDisable()
	{
		FindScripts();
		if ((bool)cmS_)
		{
			cmS_.disableMovement = false;
		}
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(802));
		list.Add(tS_.GetText(803));
		list.Add(tS_.GetText(804));
		list.Add(tS_.GetText(805));
		list.Add(tS_.GetText(1685));
		list.Add(tS_.GetText(806));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = 2;
		if (PlayerPrefs.HasKey("optDifficulty"))
		{
			uiObjects[1].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optDifficulty");
		}
		list = new List<string>();
		for (int i = 1976; i < 2031; i++)
		{
			if (i == 1976 || i == 1985 || i == 1995 || i == 2005 || i == 2015 || i == 2020)
			{
				list.Add("<b>" + i + "</b>");
			}
			else
			{
				list.Add(i.ToString());
			}
		}
		uiObjects[2].GetComponent<Dropdown>().ClearOptions();
		uiObjects[2].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[2].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optYear"))
		{
			uiObjects[2].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optYear");
		}
		list = new List<string>();
		list.Add(tS_.GetText(1335));
		list.Add(tS_.GetText(807));
		list.Add(tS_.GetText(808));
		list.Add(tS_.GetText(809));
		list.Add(tS_.GetText(810));
		uiObjects[3].GetComponent<Dropdown>().ClearOptions();
		uiObjects[3].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[3].GetComponent<Dropdown>().value = 2;
		if (PlayerPrefs.HasKey("optSpeed"))
		{
			uiObjects[3].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optSpeed");
		}
		list = new List<string>();
		list.Add(tS_.GetText(2073));
		list.Add(tS_.GetText(1001));
		list.Add(tS_.GetText(837));
		uiObjects[51].GetComponent<Dropdown>().ClearOptions();
		uiObjects[51].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[51].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optRandom"))
		{
			uiObjects[51].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optRandom");
		}
		list = new List<string>();
		list.Add(tS_.GetText(2073));
		list.Add(tS_.GetText(2074));
		list.Add(tS_.GetText(2269));
		uiObjects[13].GetComponent<Dropdown>().ClearOptions();
		uiObjects[13].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[13].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optDevSpeed"))
		{
			uiObjects[13].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optDevSpeed");
		}
		list = new List<string>();
		list.Add(tS_.GetText(2398));
		list.Add(tS_.GetText(2399));
		list.Add(tS_.GetText(2400));
		uiObjects[52].GetComponent<Dropdown>().ClearOptions();
		uiObjects[52].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[52].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optCompetition"))
		{
			uiObjects[52].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optCompetition");
		}
		int value = PlayerPrefs.GetInt(uiObjects[24].name);
		list = new List<string>();
		list.Add("<color=green>" + tS_.GetText(2152) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(100000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(250000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(500000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(1000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(5000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(10000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(25000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(50000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(100000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(500000000L, showDollar: true) + "</color>");
		list.Add("<color=green>" + mS_.GetMoney(1000000000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-100000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-250000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-500000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-1000000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-5000000L, showDollar: true) + "</color>");
		list.Add("<color=red>" + mS_.GetMoney(-10000000L, showDollar: true) + "</color>");
		uiObjects[24].GetComponent<Dropdown>().ClearOptions();
		uiObjects[24].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[24].GetComponent<Dropdown>().value = value;
		string text = "";
		value = PlayerPrefs.GetInt(uiObjects[31].name);
		list = new List<string>();
		text = tS_.GetText(2166);
		text = text.Replace("<NUM>", "0");
		list.Add(text);
		for (int j = 1; j < 9; j++)
		{
			text = tS_.GetText(2166);
			text = text.Replace("+<NUM>%", "<color=green>+" + j * 25 + "%</color>");
			list.Add(text);
		}
		for (int k = 1; k < 10; k++)
		{
			text = tS_.GetText(2166);
			text = text.Replace("+<NUM>%", "<color=red>-" + k * 10 + "%</color>");
			list.Add(text);
		}
		uiObjects[31].GetComponent<Dropdown>().ClearOptions();
		uiObjects[31].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[31].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[32].name);
		list = new List<string>();
		text = tS_.GetText(2167);
		text = text.Replace("<NUM>", "0");
		list.Add(text);
		for (int l = 1; l < 9; l++)
		{
			text = tS_.GetText(2167);
			text = text.Replace("+<NUM>%", "<color=green>+" + l * 25 + "%</color>");
			list.Add(text);
		}
		for (int m = 1; m < 10; m++)
		{
			text = tS_.GetText(2167);
			text = text.Replace("+<NUM>%", "<color=red>-" + m * 10 + "%</color>");
			list.Add(text);
		}
		uiObjects[32].GetComponent<Dropdown>().ClearOptions();
		uiObjects[32].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[32].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[36].name);
		list = new List<string>();
		text = tS_.GetText(2173);
		text = text.Replace("<NUM>", "0");
		list.Add(text);
		for (int n = 1; n < 9; n++)
		{
			text = tS_.GetText(2173);
			text = text.Replace("+<NUM>%", "<color=green>+" + n * 50 + "%</color>");
			list.Add(text);
		}
		uiObjects[36].GetComponent<Dropdown>().ClearOptions();
		uiObjects[36].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[36].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[46].name);
		list = new List<string>();
		text = tS_.GetText(2196);
		text = text.Replace("<NUM>", "0");
		list.Add(text);
		for (int num = 1; num < 9; num++)
		{
			text = tS_.GetText(2196);
			text = text.Replace("+<NUM>%", "<color=green>+" + num * 50 + "%</color>");
			list.Add(text);
		}
		uiObjects[46].GetComponent<Dropdown>().ClearOptions();
		uiObjects[46].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[46].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[37].name);
		list = new List<string>();
		list.Add(tS_.GetText(1467));
		for (int num2 = 1; num2 < 11; num2++)
		{
			list.Add("<color=green>" + tS_.GetText(1467 + num2) + "</color>");
		}
		uiObjects[37].GetComponent<Dropdown>().ClearOptions();
		uiObjects[37].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[37].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[42].name);
		list = new List<string>();
		for (int num3 = 0; num3 < 11; num3++)
		{
			text = tS_.GetText(2183);
			int num4 = num3 * 10;
			text = ((num4 <= 0) ? text.Replace("<NUM>", tS_.GetText(2185)) : text.Replace("<NUM>", "<color=green>x" + num4 + "</color>"));
			list.Add(text);
		}
		uiObjects[42].GetComponent<Dropdown>().ClearOptions();
		uiObjects[42].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[42].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[43].name);
		list = new List<string>();
		for (int num5 = 0; num5 < 11; num5++)
		{
			text = tS_.GetText(2184);
			int num6 = num5 * 5;
			text = ((num6 <= 0) ? text.Replace("<NUM>", tS_.GetText(2185)) : text.Replace("<NUM>", "<color=green>x" + num6 + "</color>"));
			list.Add(text);
		}
		uiObjects[43].GetComponent<Dropdown>().ClearOptions();
		uiObjects[43].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[43].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[47].name);
		list = new List<string>();
		list.Add(tS_.GetText(2206));
		for (int num7 = 1; num7 < 10; num7++)
		{
			text = tS_.GetText(2203);
			text = text.Replace("-<NUM>%", "<color=green>-" + num7 * 10 + "%</color>");
			list.Add(text);
		}
		list.Add("<color=green>" + tS_.GetText(2202) + "</color>");
		uiObjects[47].GetComponent<Dropdown>().ClearOptions();
		uiObjects[47].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[47].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[48].name);
		list = new List<string>();
		list.Add(tS_.GetText(2204));
		list.Add("<color=green>" + tS_.GetText(2235) + "</color>");
		for (int num8 = 1; num8 < 10; num8++)
		{
			text = tS_.GetText(2205);
			text = text.Replace("-<NUM>%", "<color=green>-" + num8 * 10 + "%</color>");
			list.Add(text);
		}
		for (int num9 = 1; num9 < 11; num9++)
		{
			text = tS_.GetText(2205);
			text = text.Replace("-<NUM>%", "<color=red>+" + num9 * 50 + "%</color>");
			list.Add(text);
		}
		uiObjects[48].GetComponent<Dropdown>().ClearOptions();
		uiObjects[48].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[48].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[55].name);
		list = new List<string>();
		list.Add(tS_.GetText(2430));
		for (int num10 = 1; num10 < 10; num10++)
		{
			text = tS_.GetText(2431);
			text = text.Replace("-<NUM>%", "<color=red>-" + num10 * 10 + "%</color>");
			list.Add(text);
		}
		for (int num11 = 1; num11 < 11; num11++)
		{
			text = tS_.GetText(2431);
			text = text.Replace("-<NUM>%", "<color=green>+" + num11 * 50 + "%</color>");
			list.Add(text);
		}
		uiObjects[55].GetComponent<Dropdown>().ClearOptions();
		uiObjects[55].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[55].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[56].name);
		list = new List<string>();
		list.Add(tS_.GetText(2434));
		for (int num12 = 1; num12 < 11; num12++)
		{
			text = tS_.GetText(2435);
			text = text.Replace("-<NUM>%", "<color=green>-" + num12 * 5 + "%</color>");
			list.Add(text);
		}
		for (int num13 = 1; num13 < 11; num13++)
		{
			text = tS_.GetText(2435);
			text = text.Replace("-<NUM>%", "<color=red>+" + num13 * 5 + "%</color>");
			list.Add(text);
		}
		uiObjects[56].GetComponent<Dropdown>().ClearOptions();
		uiObjects[56].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[56].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[50].name);
		list = new List<string>();
		list.Add(tS_.GetText(2218));
		list.Add("<color=green>" + tS_.GetText(2195) + "</color>");
		for (int num14 = 1; num14 < 10; num14++)
		{
			text = tS_.GetText(2219);
			text = text.Replace("-<NUM>%", "<color=green>-" + num14 * 10 + "%</color>");
			list.Add(text);
		}
		for (int num15 = 1; num15 < 11; num15++)
		{
			text = tS_.GetText(2219);
			text = text.Replace("-<NUM>%", "<color=red>+" + num15 * 20 + "%</color>");
			list.Add(text);
		}
		uiObjects[50].GetComponent<Dropdown>().ClearOptions();
		uiObjects[50].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[50].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[54].name);
		list = new List<string>();
		text = tS_.GetText(2428);
		text = text.Replace("<NUM>", "~30");
		list.Add(text);
		text = tS_.GetText(2428);
		text = text.Replace("<NUM>", "<color=green>~50</color>");
		list.Add(text);
		text = tS_.GetText(2428);
		text = text.Replace("<NUM>", "<color=green>~80</color>");
		list.Add(text);
		text = tS_.GetText(2428);
		text = text.Replace("<NUM>", "<color=green>~100</color>");
		list.Add(text);
		text = tS_.GetText(2428);
		text = text.Replace("<NUM>", "<color=green>~150</color>");
		list.Add(text);
		uiObjects[54].GetComponent<Dropdown>().ClearOptions();
		uiObjects[54].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[54].GetComponent<Dropdown>().value = value;
		StartCoroutine(DropdownAfterOneFrame());
		if (PlayerPrefs.GetInt("sb_toggle2") == 0)
		{
			uiObjects[17].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[17].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle3") == 0)
		{
			uiObjects[18].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[18].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle4") == 0)
		{
			uiObjects[19].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[19].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle5") == 0)
		{
			uiObjects[20].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[20].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle6") == 0)
		{
			uiObjects[21].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[21].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle7") == 0)
		{
			uiObjects[22].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[22].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle8") == 0)
		{
			uiObjects[23].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[23].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle9") == 0)
		{
			uiObjects[25].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[25].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle10") == 0)
		{
			uiObjects[26].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[26].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle11") == 0)
		{
			uiObjects[27].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[27].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle12") == 0)
		{
			uiObjects[30].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[30].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle13") == 0)
		{
			uiObjects[33].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[33].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle14") == 0)
		{
			uiObjects[34].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[34].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle15") == 0)
		{
			uiObjects[35].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[35].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle16") == 0)
		{
			uiObjects[38].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[38].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle17") == 0)
		{
			uiObjects[39].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[39].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle18") == 0)
		{
			uiObjects[41].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[41].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle19") == 0)
		{
			uiObjects[44].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[44].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle20") == 0)
		{
			uiObjects[45].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[45].GetComponent<Toggle>().isOn = true;
		}
		if (PlayerPrefs.GetInt("sb_toggle21") == 0)
		{
			uiObjects[49].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[49].GetComponent<Toggle>().isOn = true;
		}
	}

	public IEnumerator DropdownAfterOneFrame()
	{
		yield return new WaitForEndOfFrame();
		new List<string>();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		List<string> list = new List<string>();
		list.Add("20");
		list.Add("40");
		list.Add("60");
		list.Add("80");
		list.Add(array.Length.ToString());
		uiObjects[7].GetComponent<Dropdown>().ClearOptions();
		uiObjects[7].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[7].GetComponent<Dropdown>().value = 4;
		if (PlayerPrefs.HasKey("optOpponent"))
		{
			uiObjects[7].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optOpponent");
		}
	}

	public void SetLogo(int i)
	{
		uiObjects[4].GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(i);
		logo = i;
	}

	public void SetCountry(int i)
	{
		country = i;
		uiObjects[6].GetComponent<Text>().text = tS_.GetCountry(i);
		uiObjects[5].GetComponent<Image>().sprite = guiMain_.flagSprites[i];
	}

	public void SetGenre(int i)
	{
		genre = i;
		uiObjects[10].GetComponent<Text>().text = genres_.GetName(genre);
		uiObjects[11].GetComponent<Image>().sprite = genres_.GetPic(genre);
	}

	public void BUTTON_Abbrechen()
	{
		SaveOptions();
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_RandomCompanyName()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[0].GetComponent<InputField>().text = tS_.GetRandomCompanyName();
	}

	public void BUTTON_Firmenlogo()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[48]);
	}

	public void BUTTON_Standort()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[161]);
	}

	public void BUTTON_Genre()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[298]);
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(814), closeMenu: false);
			return;
		}
		PlayerPrefs.SetInt(uiObjects[24].name, uiObjects[24].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[31].name, uiObjects[31].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[32].name, uiObjects[32].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[36].name, uiObjects[36].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[46].name, uiObjects[46].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[37].name, uiObjects[37].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[42].name, uiObjects[42].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[43].name, uiObjects[43].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[47].name, uiObjects[47].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[48].name, uiObjects[48].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[50].name, uiObjects[50].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[54].name, uiObjects[54].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[55].name, uiObjects[55].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[56].name, uiObjects[56].GetComponent<Dropdown>().value);
		mS_.settings_arbeitsgeschwindigkeitAnpassen = uiObjects[9].GetComponent<Toggle>().isOn;
		mS_.settings_closeNPCs = uiObjects[53].GetComponent<Toggle>().isOn;
		if (uiObjects[24].GetComponent<Dropdown>().value == 0)
		{
			mS_.sandbox_unlimitedMoney = true;
		}
		else
		{
			mS_.sandbox_unlimitedMoney = false;
		}
		mS_.sandbox_mitarbeiterMotivation = uiObjects[21].GetComponent<Toggle>().isOn;
		mS_.sandbox_mitarbeiterPause = uiObjects[22].GetComponent<Toggle>().isOn;
		mS_.sandbox_mitarbeiterKrank = uiObjects[30].GetComponent<Toggle>().isOn;
		mS_.sandbox_mitarbeiterSkill100 = uiObjects[39].GetComponent<Toggle>().isOn;
		mS_.sandbox_publisherMaxReleation = uiObjects[27].GetComponent<Toggle>().isOn;
		mS_.sandbox_allItems = uiObjects[23].GetComponent<Toggle>().isOn;
		mS_.sandbox_keinIpVerfall = uiObjects[35].GetComponent<Toggle>().isOn;
		mS_.sandbox_bekannteKonzeptEinstellungen = uiObjects[38].GetComponent<Toggle>().isOn;
		mS_.sandbox_fitTopicToGenre = uiObjects[41].GetComponent<Toggle>().isOn;
		mS_.sandbox_allBuildings = uiObjects[44].GetComponent<Toggle>().isOn;
		float num = 0f;
		mS_.sandbox_gameSells = uiObjects[31].GetComponent<Dropdown>().value;
		if (mS_.sandbox_gameSells < 9f)
		{
			mS_.sandbox_gameSells = 1f + 0.25f * mS_.sandbox_gameSells;
		}
		if (mS_.sandbox_gameSells >= 9f)
		{
			mS_.sandbox_gameSells = 1f - 0.1f * (mS_.sandbox_gameSells - 8f);
		}
		mS_.sandbox_konsoleSells = uiObjects[32].GetComponent<Dropdown>().value;
		if (mS_.sandbox_konsoleSells < 9f)
		{
			mS_.sandbox_konsoleSells = 1f + 0.25f * mS_.sandbox_konsoleSells;
		}
		if (mS_.sandbox_konsoleSells >= 9f)
		{
			mS_.sandbox_konsoleSells = 1f - 0.1f * (mS_.sandbox_konsoleSells - 8f);
		}
		num = uiObjects[36].GetComponent<Dropdown>().value;
		num *= 0.5f;
		mS_.sandbox_trainingSpeed = 1f + num;
		num = uiObjects[46].GetComponent<Dropdown>().value;
		num *= 0.5f;
		mS_.sandbox_maschineSpeed = 1f + num;
		mS_.sandbox_lager = uiObjects[42].GetComponent<Dropdown>().value * 10;
		if (mS_.sandbox_lager < 1)
		{
			mS_.sandbox_lager = 1;
		}
		mS_.sandbox_server = uiObjects[43].GetComponent<Dropdown>().value * 5;
		if (mS_.sandbox_server < 1)
		{
			mS_.sandbox_server = 1;
		}
		if (uiObjects[48].GetComponent<Dropdown>().value == 1)
		{
			mS_.sandbox_mitarbeiterGehalt = 100f;
		}
		else if (uiObjects[48].GetComponent<Dropdown>().value == 0)
		{
			mS_.sandbox_mitarbeiterGehalt = 1f;
		}
		else
		{
			mS_.sandbox_mitarbeiterGehalt = uiObjects[48].GetComponent<Dropdown>().value;
			if (mS_.sandbox_mitarbeiterGehalt <= 10f)
			{
				mS_.sandbox_mitarbeiterGehalt = 1f - 0.1f * (mS_.sandbox_mitarbeiterGehalt - 1f);
			}
			if (mS_.sandbox_mitarbeiterGehalt > 10f)
			{
				mS_.sandbox_mitarbeiterGehalt = 1f + 0.5f * (mS_.sandbox_mitarbeiterGehalt - 10f);
			}
		}
		if (uiObjects[55].GetComponent<Dropdown>().value != 0)
		{
			mS_.sandbox_mitarbeiterSpeed = uiObjects[55].GetComponent<Dropdown>().value;
			if (mS_.sandbox_mitarbeiterSpeed <= 9f)
			{
				mS_.sandbox_mitarbeiterSpeed = 1f - 0.1f * (mS_.sandbox_mitarbeiterSpeed - 0f);
			}
			if (mS_.sandbox_mitarbeiterSpeed > 9f)
			{
				mS_.sandbox_mitarbeiterSpeed = 1f + 0.5f * (mS_.sandbox_mitarbeiterSpeed - 9f);
			}
		}
		else
		{
			mS_.sandbox_mitarbeiterSpeed = 1f;
		}
		if (uiObjects[56].GetComponent<Dropdown>().value != 0)
		{
			mS_.sandbox_npcGameQuality = uiObjects[56].GetComponent<Dropdown>().value;
			if (mS_.sandbox_npcGameQuality <= 10f)
			{
				mS_.sandbox_npcGameQuality = 1f - 0.05f * (mS_.sandbox_npcGameQuality - 0f);
			}
			if (mS_.sandbox_npcGameQuality > 10f)
			{
				mS_.sandbox_npcGameQuality = 1f + 0.05f * (mS_.sandbox_npcGameQuality - 10f);
			}
		}
		else
		{
			mS_.sandbox_npcGameQuality = 1f;
		}
		mS_.sandbox_support = uiObjects[50].GetComponent<Dropdown>().value;
		mS_.sandbox_arbeitsmarkt = uiObjects[54].GetComponent<Dropdown>().value;
		mS_.sandbox_bugs = uiObjects[47].GetComponent<Dropdown>().value;
		mS_.settings_sandbox = true;
		CreateSandboxString();
		SaveOptions();
		guiMain_.uiObjects[162].SetActive(value: true);
	}

	public void TOGGLE_AllesNichts()
	{
		Toggle[] componentsInChildren = uiObjects[12].transform.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i] && componentsInChildren[i].name != "ToggleAllesNichts")
			{
				componentsInChildren[i].isOn = uiObjects[40].GetComponent<Toggle>().isOn;
			}
		}
	}

	private void CreateSandboxString()
	{
		string text = "";
		text = text + "▪ " + uiObjects[24].transform.GetChild(0).GetComponent<Text>().text + "\n";
		if (uiObjects[37].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[37].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[31].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[31].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[32].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[32].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[36].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[36].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[46].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[46].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[42].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[42].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[43].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[43].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[47].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[47].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[48].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[48].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[55].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[55].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[56].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[56].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[50].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[50].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		if (uiObjects[54].GetComponent<Dropdown>().value > 0)
		{
			text = text + "▪ " + uiObjects[54].transform.GetChild(0).GetComponent<Text>().text + "\n";
		}
		Toggle[] componentsInChildren = uiObjects[12].transform.GetComponentsInChildren<Toggle>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i] && componentsInChildren[i].isOn && componentsInChildren[i].name != "ToggleAllesNichts")
			{
				if (componentsInChildren[i].isOn)
				{
					PlayerPrefs.SetInt(componentsInChildren[i].name, 1);
				}
				else
				{
					PlayerPrefs.SetInt(componentsInChildren[i].name, 0);
				}
				text = text + "▪ " + componentsInChildren[i].transform.GetChild(1).GetComponent<Text>().text + "\n";
			}
		}
		mS_.sandbox_string = text;
	}

	private void SaveOptions()
	{
		PlayerPrefs.SetInt("optDifficulty", uiObjects[1].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optYear", uiObjects[2].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optSpeed", uiObjects[3].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optOpponent", uiObjects[7].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optLogo", logo);
		PlayerPrefs.SetInt("optCountry", country);
		PlayerPrefs.SetInt("optGenre", genre);
		PlayerPrefs.SetInt("optDevSpeed", uiObjects[13].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optRandom", uiObjects[51].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optCompetition", uiObjects[52].GetComponent<Dropdown>().value);
		PlayerPrefs.SetString("CompanyName", uiObjects[0].GetComponent<InputField>().text);
		if (uiObjects[17].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle2", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle2", 0);
		}
		if (uiObjects[18].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle3", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle3", 0);
		}
		if (uiObjects[19].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle4", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle4", 0);
		}
		if (uiObjects[20].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle5", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle5", 0);
		}
		if (uiObjects[21].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle6", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle6", 0);
		}
		if (uiObjects[22].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle7", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle7", 0);
		}
		if (uiObjects[23].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle8", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle8", 0);
		}
		if (uiObjects[25].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle9", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle9", 0);
		}
		if (uiObjects[26].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle10", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle10", 0);
		}
		if (uiObjects[27].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle11", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle11", 0);
		}
		if (uiObjects[30].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle12", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle12", 0);
		}
		if (uiObjects[33].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle13", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle13", 0);
		}
		if (uiObjects[34].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle14", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle14", 0);
		}
		if (uiObjects[35].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle15", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle15", 0);
		}
		if (uiObjects[38].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle16", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle16", 0);
		}
		if (uiObjects[39].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle17", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle17", 0);
		}
		if (uiObjects[41].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle18", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle18", 0);
		}
		if (uiObjects[44].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle19", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle19", 0);
		}
		if (uiObjects[45].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle20", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle20", 0);
		}
		if (uiObjects[49].GetComponent<Toggle>().isOn)
		{
			PlayerPrefs.SetInt("sb_toggle21", 1);
		}
		else
		{
			PlayerPrefs.SetInt("sb_toggle21", 0);
		}
	}
}
