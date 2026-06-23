using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_PersonalGroups : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private roomDataScript rdS_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
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
	}

	private void Update()
	{
		uiObjects[9].GetComponent<Text>().text = GetAmountInGroup().ToString();
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
			uiObjects[5].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void OnEnable()
	{
		Init(updateGroup: false);
		Init(updateGroup: true);
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(1018));
		list.Add(tS_.GetText(119));
		list.Add(tS_.GetText(120));
		list.Add(tS_.GetText(121));
		list.Add(tS_.GetText(122));
		list.Add(tS_.GetText(123));
		list.Add(tS_.GetText(124));
		list.Add(tS_.GetText(125));
		list.Add(tS_.GetText(126));
		list.Add(tS_.GetText(127));
		list.Add(tS_.GetText(190));
		list.Add(tS_.GetText(1764));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		List<string> list2 = new List<string>();
		for (int i = 1; i <= mS_.personal_group_names.Length; i++)
		{
			if (mS_.personal_group_names[i - 1].Length <= 0)
			{
				list2.Add(tS_.GetText(202) + " " + i);
			}
			else
			{
				list2.Add("(" + i + ") " + mS_.personal_group_names[i - 1]);
			}
		}
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list2);
	}

	public void Init(bool updateGroup)
	{
		FindScripts();
		if (!updateGroup)
		{
			InitDropdowns();
		}
		SetData(updateGroup);
	}

	private void SetData(bool updateGroup)
	{
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if (!mS_.arrayCharactersScripts[i])
			{
				continue;
			}
			characterScript characterScript2 = mS_.arrayCharactersScripts[i];
			if (!characterScript2)
			{
				continue;
			}
			if (!updateGroup)
			{
				if (characterScript2.group == -1)
				{
					Item_PersonalGroup component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_PersonalGroup>();
					component.characterID = characterScript2.myID;
					component.cS_ = characterScript2;
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.rdS_ = rdS_;
				}
			}
			else if (characterScript2.group == uiObjects[6].GetComponent<Dropdown>().value + 1)
			{
				Item_PersonalGroup component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[4].transform).GetComponent<Item_PersonalGroup>();
				component2.characterID = characterScript2.myID;
				component2.cS_ = characterScript2;
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.rdS_ = rdS_;
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[7]);
		guiMain_.KeinEintrag(uiObjects[4], uiObjects[8]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void DROPDOWN_Group()
	{
		int childCount = uiObjects[4].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[4].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				gameObject.SetActive(value: false);
			}
		}
		Init(updateGroup: true);
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		Sort(0);
		Sort(4);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[7]);
		guiMain_.KeinEintrag(uiObjects[4], uiObjects[8]);
	}

	private void Sort(int element)
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		int childCount = uiObjects[element].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[element].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_PersonalGroup component = gameObject.GetComponent<Item_PersonalGroup>();
			switch (value)
			{
			case 0:
				gameObject.name = component.cS_.myName;
				switch (component.cS_.GetBestSkill())
				{
				case 0:
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
					break;
				case 1:
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
					break;
				case 2:
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
					break;
				case 3:
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
					break;
				case 4:
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
					break;
				case 5:
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
					break;
				case 6:
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
					break;
				case 7:
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
					break;
				}
				break;
			case 1:
				gameObject.name = component.cS_.beruf.ToString();
				switch (component.cS_.GetBestSkill())
				{
				case 0:
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
					break;
				case 1:
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
					break;
				case 2:
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
					break;
				case 3:
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
					break;
				case 4:
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
					break;
				case 5:
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
					break;
				case 6:
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
					break;
				case 7:
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
					break;
				}
				break;
			case 2:
				gameObject.name = component.cS_.s_gamedesign.ToString();
				component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				break;
			case 3:
				gameObject.name = component.cS_.s_programmieren.ToString();
				component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				break;
			case 4:
				gameObject.name = component.cS_.s_grafik.ToString();
				component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				break;
			case 5:
				gameObject.name = component.cS_.s_sound.ToString();
				component.SetData(tS_.GetText(122), component.cS_.s_sound);
				break;
			case 6:
				gameObject.name = component.cS_.s_pr.ToString();
				component.SetData(tS_.GetText(123), component.cS_.s_pr);
				break;
			case 7:
				gameObject.name = component.cS_.s_gametests.ToString();
				component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				break;
			case 8:
				gameObject.name = component.cS_.s_technik.ToString();
				component.SetData(tS_.GetText(125), component.cS_.s_technik);
				break;
			case 9:
				gameObject.name = component.cS_.s_forschen.ToString();
				component.SetData(tS_.GetText(126), component.cS_.s_forschen);
				break;
			case 10:
				gameObject.name = component.cS_.GetGehalt().ToString();
				switch (component.cS_.GetBestSkill())
				{
				case 0:
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
					break;
				case 1:
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
					break;
				case 2:
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
					break;
				case 3:
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
					break;
				case 4:
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
					break;
				case 5:
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
					break;
				case 6:
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
					break;
				case 7:
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
					break;
				}
				break;
			case 11:
				gameObject.name = component.cS_.s_motivation.ToString();
				switch (component.cS_.GetBestSkill())
				{
				case 0:
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
					break;
				case 1:
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
					break;
				case 2:
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
					break;
				case 3:
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
					break;
				case 4:
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
					break;
				case 5:
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
					break;
				case 6:
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
					break;
				case 7:
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
					break;
				}
				break;
			case 12:
				gameObject.name = component.cS_.GetBestSkillValue().ToString();
				switch (component.cS_.GetBestSkill())
				{
				case 0:
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
					break;
				case 1:
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
					break;
				case 2:
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
					break;
				case 3:
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
					break;
				case 4:
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
					break;
				case 5:
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
					break;
				case 6:
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
					break;
				case 7:
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
					break;
				}
				break;
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[element]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[element]);
		}
	}

	public void BUTTON_Select()
	{
		if (uiObjects[4].transform.childCount <= 0)
		{
			return;
		}
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		for (int i = 0; i < uiObjects[4].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[4].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_PersonalGroup component = gameObject.GetComponent<Item_PersonalGroup>();
				if ((bool)component)
				{
					pcS_.PickFromExternObject(component.cS_.gameObject);
				}
			}
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Rename()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[282].SetActive(value: true);
		guiMain_.uiObjects[282].GetComponent<Menu_PersonalGroupName>().Init(uiObjects[6].GetComponent<Dropdown>().value);
	}

	public int GetAmountInGroup()
	{
		return uiObjects[4].transform.childCount;
	}

	public void BUTTON_AddAll()
	{
		sfx_.PlaySound(3, force: true);
		Item_PersonalGroup[] componentsInChildren = uiObjects[4].GetComponentsInChildren<Item_PersonalGroup>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(all: true);
			}
		}
		DROPDOWN_Sort();
	}

	public void BUTTON_RemoveAll()
	{
		sfx_.PlaySound(3, force: true);
		Item_PersonalGroup[] componentsInChildren = uiObjects[0].GetComponentsInChildren<Item_PersonalGroup>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			if ((bool)componentsInChildren[i])
			{
				componentsInChildren[i].BUTTON_Click(all: true);
			}
		}
		DROPDOWN_Sort();
	}
}
