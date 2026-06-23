using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Theme : MonoBehaviour
{
	public int themeArt;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private games games_;

	private Menu_DevGame mDevGame_;

	private Menu_Dev_AddonDo mDevAddon_;

	private Menu_Dev_MMOAddon mDevMMOAddon_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private float updateTimer;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!mDevAddon_)
		{
			mDevAddon_ = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>();
		}
		if (!mDevMMOAddon_)
		{
			mDevMMOAddon_ = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
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
				SetData(themeArt);
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		if (!mS_.multiplayer)
		{
			return false;
		}
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevGame_Theme>().myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void Init(int g)
	{
		uiObjects[7].GetComponent<InputField>().text = "";
		FindScripts();
		InitDropdowns();
		themeArt = g;
		SetData(themeArt);
	}

	private void SetData(int g)
	{
		if (g == 0)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(352);
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(353);
		}
		uiObjects[6].GetComponent<Button>().interactable = true;
		if (g == 0)
		{
			if (guiMain_.uiObjects[56].activeSelf && mDevGame_.g_GameMainTheme == -1)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			if (guiMain_.uiObjects[56].activeSelf && mDevGame_.g_GameSubTheme == -1)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
			}
			if (guiMain_.uiObjects[193].activeSelf && mDevAddon_.g_GameSubTheme == -1)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
			}
			if (guiMain_.uiObjects[247].activeSelf && mDevMMOAddon_.g_GameSubTheme == -1)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
			}
		}
		string text = uiObjects[7].GetComponent<InputField>().text;
		int length = uiObjects[7].GetComponent<InputField>().text.Length;
		for (int i = 0; i < themes_.themes_LEVEL.Length; i++)
		{
			if (!themes_.IsErforscht(i) || Exists(uiObjects[0], i))
			{
				continue;
			}
			bool flag = false;
			if (length > 0 && tS_.GetThemes(i).ToLower().Contains(text.ToLower()))
			{
				flag = true;
			}
			if (length <= 0 || flag)
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_DevGame_Theme component = gameObject.GetComponent<Item_DevGame_Theme>();
				component.myID = i;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.themes_ = themes_;
				component.themeArt = g;
				component.fitGenre = FitGenre(i);
				if (guiMain_.uiObjects[56].activeSelf && (i == mDevGame_.g_GameSubTheme || i == mDevGame_.g_GameMainTheme))
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
				if (guiMain_.uiObjects[193].activeSelf && (i == mDevAddon_.g_GameSubTheme || i == mDevAddon_.gS_.gameMainTheme))
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
				if (guiMain_.uiObjects[247].activeSelf && (i == mDevMMOAddon_.g_GameSubTheme || i == mDevMMOAddon_.gS_.gameMainTheme))
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	private int FitGenre(int theme_)
	{
		int num = -1;
		if (guiMain_.uiObjects[56].activeSelf)
		{
			num = mDevGame_.g_GameMainGenre;
		}
		if (guiMain_.uiObjects[193].activeSelf)
		{
			num = mDevAddon_.gS_.maingenre;
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			num = mDevMMOAddon_.gS_.maingenre;
		}
		if (num != -1)
		{
			if (mS_.settings_sandbox && mS_.sandbox_fitTopicToGenre)
			{
				if (themes_.IsThemesFitWithGenre(theme_, num))
				{
					return 1;
				}
				return -1;
			}
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].spielbericht && games_.arrayGamesScripts[i].maingenre == num && (games_.arrayGamesScripts[i].ownerID == mS_.myID || games_.arrayGamesScripts[i].developerID == mS_.myID) && (games_.arrayGamesScripts[i].gameMainTheme == theme_ || games_.arrayGamesScripts[i].gameSubTheme == theme_))
				{
					if (themes_.IsThemesFitWithGenre(theme_, num))
					{
						return 1;
					}
					return -1;
				}
			}
		}
		return 0;
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(5));
		list.Add(tS_.GetText(1665));
		int num = -1;
		if (guiMain_.uiObjects[56].activeSelf)
		{
			num = mDevGame_.g_GameMainGenre;
		}
		if (guiMain_.uiObjects[193].activeSelf)
		{
			num = mDevAddon_.gS_.maingenre;
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			num = mDevMMOAddon_.gS_.maingenre;
		}
		if (num != -1)
		{
			list.Add(tS_.GetText(1894));
			list.Add(tS_.GetText(1895));
		}
		else
		{
			list.Add("<color=grey>" + tS_.GetText(1894) + "</color>");
			list.Add("<color=grey>" + tS_.GetText(1895) + "</color>");
		}
		list.Add(tS_.GetText(305));
		uiObjects[5].GetComponent<Dropdown>().ClearOptions();
		uiObjects[5].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[5].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[5].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[5].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_DevGame_Theme component = gameObject.GetComponent<Item_DevGame_Theme>();
			switch (value)
			{
			case 0:
				gameObject.name = tS_.GetThemes(component.myID);
				break;
			case 1:
				gameObject.name = themes_.themes_LEVEL[component.myID].ToString();
				break;
			case 2:
				gameObject.name = (-themes_.themes_MARKT[component.myID]).ToString();
				break;
			case 3:
				if (component.fitGenre == 1)
				{
					gameObject.name = "2";
				}
				if (component.fitGenre == 0)
				{
					gameObject.name = "1";
				}
				if (component.fitGenre == -1)
				{
					gameObject.name = "0";
				}
				break;
			case 4:
				if (component.fitGenre == -1)
				{
					gameObject.name = "2";
				}
				if (component.fitGenre == 0)
				{
					gameObject.name = "1";
				}
				if (component.fitGenre == 1)
				{
					gameObject.name = "0";
				}
				break;
			case 5:
				gameObject.name = themes_.themes_USES[component.myID].ToString();
				break;
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[0]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[0]);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_ThemaEntfernen()
	{
		if (themeArt == 0)
		{
			if (guiMain_.uiObjects[56].activeSelf)
			{
				mDevGame_.SetMainTheme(-1);
			}
		}
		else
		{
			if (guiMain_.uiObjects[56].activeSelf)
			{
				mDevGame_.SetSubTheme(-1);
			}
			if (guiMain_.uiObjects[193].activeSelf)
			{
				mDevAddon_.SetSubTheme(-1);
			}
			if (guiMain_.uiObjects[247].activeSelf)
			{
				mDevMMOAddon_.SetSubTheme(-1);
			}
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Search()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData(themeArt);
	}

	public void BUTTON_Marktanalyse()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[136].SetActive(value: true);
	}
}
