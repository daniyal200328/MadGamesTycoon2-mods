using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GamePass_Games : MonoBehaviour
{
	public GameObject[] uiObjects;

	public GameObject[] uiPrefabs;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private unlockScript unlock_;

	private forschungSonstiges forschungSonstiges_;

	private genres genres_;

	private gamepassScript gpS_;

	private games games_;

	public Dropdown dropdownPlatform;

	private List<platformScript> arrayPlatforms = new List<platformScript>();

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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	public void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		if (uiObjects[10].GetComponent<Dropdown>().value == 0)
		{
			SetData_OutPass();
			SetData_InPass();
		}
	}

	private void Update()
	{
		string text = tS_.GetText(297);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount + " / " + gpS_.gamePass_AmountGames);
		uiObjects[9].GetComponent<Text>().text = text;
		guiMain_.DrawStarsColor(uiObjects[12], Mathf.RoundToInt(gpS_.GAMEPASS_GetAktualitaet() * 5f), Color.white);
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
			uiObjects[5].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(275));
		list.Add(tS_.GetText(274));
		list.Add(tS_.GetText(2134));
		list.Add(tS_.GetText(2144));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[6].name);
		list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(275));
		list.Add(tS_.GetText(274));
		list.Add(tS_.GetText(2134));
		list.Add(tS_.GetText(2144));
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[6].GetComponent<Dropdown>().value = value;
		arrayPlatforms = new List<platformScript>();
		value = PlayerPrefs.GetInt(uiObjects[10].name);
		list = new List<string>();
		list.Add(tS_.GetText(2118));
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].CanBeInGamePass(ignoreGames: true))
			{
				if (mS_.arrayPlatformsScripts[i].ownerID == mS_.myID)
				{
					list.Add("<color=blue>" + mS_.arrayPlatformsScripts[i].GetName() + "</color>");
				}
				else
				{
					list.Add(mS_.arrayPlatformsScripts[i].GetName());
				}
				arrayPlatforms.Add(mS_.arrayPlatformsScripts[i]);
			}
		}
		uiObjects[10].GetComponent<Dropdown>().ClearOptions();
		uiObjects[10].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[10].GetComponent<Dropdown>().value = value;
	}

	public void ResetData()
	{
		Item_GamePass_Games[] componentsInChildren = uiObjects[0].GetComponentsInChildren<Item_GamePass_Games>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				Object.Destroy(componentsInChildren[i].gameObject);
			}
		}
		componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_GamePass_Games>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			if ((bool)componentsInChildren[j])
			{
				Object.Destroy(componentsInChildren[j].gameObject);
			}
		}
		StartCoroutine(SetData_AfterOneFrame());
	}

	private IEnumerator SetData_AfterOneFrame()
	{
		yield return new WaitForEndOfFrame();
		SetData_OutPass();
		SetData_InPass();
	}

	private void SetData_OutPass()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && CheckGameData_OutPass(games_.arrayGamesScripts[i]))
			{
				Item_GamePass_Games component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[4].transform).GetComponent<Item_GamePass_Games>();
				component.game_ = games_.arrayGamesScripts[i];
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
				component.SetData();
			}
		}
		DROPDOWN_Sort_OutPass();
		guiMain_.KeinEintrag(uiObjects[4], uiObjects[8]);
	}

	private void SetData_InPass()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && CheckGameData_InPass(games_.arrayGamesScripts[i]))
			{
				Item_GamePass_Games component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_GamePass_Games>();
				component.game_ = games_.arrayGamesScripts[i];
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
				component.SetData();
			}
		}
		DROPDOWN_Sort_InPass();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[7]);
	}

	public bool CheckGameData_OutPass(gameScript script_)
	{
		if ((bool)script_ && !script_.inGamePass && script_.CanBeInGamePass())
		{
			if (dropdownPlatform.value == 0)
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[0])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[1])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[2])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[3])
			{
				return true;
			}
			if (script_.IsKonsoleAbwaertskompatibel(arrayPlatforms[dropdownPlatform.value - 1]))
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckGameData_InPass(gameScript script_)
	{
		if ((bool)script_ && script_.inGamePass && script_.CanBeInGamePass())
		{
			if (dropdownPlatform.value == 0)
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[0])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[1])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[2])
			{
				return true;
			}
			if (arrayPlatforms[dropdownPlatform.value - 1].myID == script_.gamePlatform[3])
			{
				return true;
			}
			if (script_.IsKonsoleAbwaertskompatibel(arrayPlatforms[dropdownPlatform.value - 1]))
			{
				return true;
			}
		}
		return false;
	}

	public void DROPDOWN_Sort_OutPass()
	{
		int value = uiObjects[6].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[6].name, value);
		_ = uiObjects[4].transform.childCount;
		Item_GamePass_Games[] componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_GamePass_Games>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if (!componentsInChildren[i])
			{
				continue;
			}
			GameObject gameObject = componentsInChildren[i].gameObject;
			Item_GamePass_Games item_GamePass_Games = componentsInChildren[i];
			switch (value)
			{
			case 0:
				gameObject.name = item_GamePass_Games.game_.GetNameSimple();
				break;
			case 1:
			{
				float num = item_GamePass_Games.game_.date_month;
				num /= 13f;
				gameObject.name = item_GamePass_Games.game_.date_year.ToString() + num;
				break;
			}
			case 2:
				gameObject.name = item_GamePass_Games.game_.reviewTotal.ToString();
				break;
			case 3:
				gameObject.name = item_GamePass_Games.game_.maingenre.ToString();
				break;
			case 4:
				gameObject.name = item_GamePass_Games.game_.sellsTotal.ToString();
				break;
			case 5:
				gameObject.name = item_GamePass_Games.game_.GetDeveloperName();
				break;
			case 6:
				if (!item_GamePass_Games.uiObjects[5].activeSelf)
				{
					gameObject.name = "0";
				}
				else
				{
					gameObject.name = "1";
				}
				break;
			case 7:
				if (!item_GamePass_Games.game_.isOnMarket)
				{
					gameObject.name = "0";
				}
				else
				{
					gameObject.name = "1";
				}
				break;
			}
		}
		if (value == 0 || value == 5)
		{
			mS_.SortChildrenByName(uiObjects[4]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[4]);
		}
	}

	public void DROPDOWN_Sort_InPass()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_GamePass_Games component = gameObject.GetComponent<Item_GamePass_Games>();
			switch (value)
			{
			case 0:
				gameObject.name = component.game_.GetNameSimple();
				break;
			case 1:
			{
				float num = component.game_.date_month;
				num /= 13f;
				gameObject.name = component.game_.date_year.ToString() + num;
				break;
			}
			case 2:
				gameObject.name = component.game_.reviewTotal.ToString();
				break;
			case 3:
				gameObject.name = component.game_.maingenre.ToString();
				break;
			case 4:
				gameObject.name = component.game_.sellsTotal.ToString();
				break;
			case 5:
				gameObject.name = component.game_.GetDeveloperName();
				break;
			case 6:
				if (!component.uiObjects[5].activeSelf)
				{
					gameObject.name = "0";
				}
				else
				{
					gameObject.name = "1";
				}
				break;
			case 7:
				if (!component.game_.isOnMarket)
				{
					gameObject.name = "0";
				}
				else
				{
					gameObject.name = "1";
				}
				break;
			}
		}
		if (value == 0 || value == 5)
		{
			mS_.SortChildrenByName(uiObjects[0]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[0]);
		}
	}

	public void DROPDOWN_Platform()
	{
		int value = uiObjects[10].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[10].name, value);
		ResetData();
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_AddAllGames()
	{
		sfx_.PlaySound(3, force: true);
		Item_GamePass_Games[] componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_GamePass_Games>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(allGames: true);
			}
		}
		gpS_.GetAmountGamePassGames();
		MESSAGEBOX_PlatformRemoved();
		ResetData();
	}

	public void BUTTON_RemoveAllGames()
	{
		sfx_.PlaySound(3, force: true);
		Item_GamePass_Games[] componentsInChildren = uiObjects[0].GetComponentsInChildren<Item_GamePass_Games>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(allGames: true);
			}
		}
		gpS_.GetAmountGamePassGames();
		MESSAGEBOX_PlatformRemoved();
		ResetData();
	}

	public void MESSAGEBOX_PlatformRemoved()
	{
		string text = "";
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.inGamePass && !component.CanBeInGamePass(ignoreGames: false))
				{
					gpS_.GAMEPASS_RemovePlatform(component);
					text = text + "<color=red>" + component.GetName() + "</color>\n";
				}
			}
		}
		if (text.Length > 0)
		{
			guiMain_.MessageBox(tS_.GetText(2113) + "\n\n" + text, closeMenu: false);
		}
		StartCoroutine(SortAfterOneFrame());
	}

	private IEnumerator SortAfterOneFrame()
	{
		yield return new WaitForEndOfFrame();
		DROPDOWN_Sort_InPass();
		DROPDOWN_Sort_OutPass();
	}
}
