using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_NewGame : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private cameraMovementScript cmS_;

	public int logo = -1;

	public int country;

	public int genre;

	public int devSpeed;

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
		for (int i = 1976; i < 2021; i++)
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
		uiObjects[14].GetComponent<Dropdown>().ClearOptions();
		uiObjects[14].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[14].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optRandom"))
		{
			uiObjects[14].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optRandom");
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
		uiObjects[15].GetComponent<Dropdown>().ClearOptions();
		uiObjects[15].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[15].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optCompetition"))
		{
			uiObjects[15].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optCompetition");
		}
		StartCoroutine(DropdownAfterOneFrame());
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
		mS_.settings_arbeitsgeschwindigkeitAnpassen = uiObjects[9].GetComponent<Toggle>().isOn;
		mS_.settings_closeNPCs = uiObjects[16].GetComponent<Toggle>().isOn;
		mS_.settings_sandbox = false;
		SaveOptions();
		guiMain_.uiObjects[162].SetActive(value: true);
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
		PlayerPrefs.SetInt("optDevSpeed", devSpeed);
		PlayerPrefs.SetInt("optRandom", uiObjects[14].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt("optCompetition", uiObjects[15].GetComponent<Dropdown>().value);
		PlayerPrefs.SetString("CompanyName", uiObjects[0].GetComponent<InputField>().text);
	}
}
