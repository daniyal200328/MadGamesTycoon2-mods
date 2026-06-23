using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_LeitenderDesigner : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private roomDataScript rdS_;

	private Menu_DevGame menuDevGame_;

	private Menu_Dev_GameEntwicklungsbericht menuEntwicklungsbericht_;

	private Menu_Dev_AddonDo mDevAddon_;

	private Menu_Dev_MMOAddon mDevMMOAddon_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private roomScript rS_;

	private float updateTimer;

	private string searchStringA = "";

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
		if (!menuDevGame_)
		{
			menuDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!menuEntwicklungsbericht_)
		{
			menuEntwicklungsbericht_ = guiMain_.uiObjects[73].GetComponent<Menu_Dev_GameEntwicklungsbericht>();
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
				SetData();
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_LeitenderDesigner>().cS_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void InitDropdowns()
	{
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
	}

	public void Init(roomScript roomS_)
	{
		FindScripts();
		rS_ = roomS_;
		InitDropdowns();
		SetData();
		if (rS_.leitenderGamedesigner == -1)
		{
			uiObjects[8].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[8].GetComponent<Toggle>().isOn = false;
		}
	}

	private void SetData()
	{
		if (!rS_)
		{
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			characterScript component = array[i].GetComponent<characterScript>();
			if (!component || component.roomID != rS_.myID)
			{
				continue;
			}
			string myName = component.myName;
			searchStringA = searchStringA.ToLower();
			myName = myName.ToLower();
			if ((uiObjects[7].GetComponent<InputField>().text.Length <= 0 || myName.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_LeitenderDesigner component2 = gameObject.GetComponent<Item_LeitenderDesigner>();
				component2.characterID = component.myID;
				component2.cS_ = component;
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.rdS_ = rdS_;
				if (menuDevGame_.gameObject.activeSelf && menuDevGame_.g_leitenderDesigner == component)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
				if (mDevAddon_.gameObject.activeSelf && mDevAddon_.g_leitenderDesigner == component)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
				if (menuEntwicklungsbericht_.gameObject.activeSelf && menuEntwicklungsbericht_.GetLeitenderEntwickler() == component)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	public void BUTTON_Close()
	{
		SetAuto();
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void DROPDOWN_Sort()
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
			Item_LeitenderDesigner component = gameObject.GetComponent<Item_LeitenderDesigner>();
			switch (value)
			{
			case 0:
				gameObject.name = component.cS_.myName;
				if (component.cS_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				}
				if (component.cS_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				}
				if (component.cS_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				}
				if (component.cS_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
				}
				if (component.cS_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
				}
				if (component.cS_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				}
				if (component.cS_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
				}
				if (component.cS_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
				}
				break;
			case 1:
				gameObject.name = component.cS_.beruf.ToString();
				if (component.cS_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				}
				if (component.cS_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				}
				if (component.cS_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				}
				if (component.cS_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
				}
				if (component.cS_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
				}
				if (component.cS_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				}
				if (component.cS_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
				}
				if (component.cS_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
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
				if (component.cS_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				}
				if (component.cS_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				}
				if (component.cS_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				}
				if (component.cS_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
				}
				if (component.cS_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
				}
				if (component.cS_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				}
				if (component.cS_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
				}
				if (component.cS_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
				}
				break;
			case 11:
				gameObject.name = component.cS_.s_motivation.ToString();
				if (component.cS_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				}
				if (component.cS_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				}
				if (component.cS_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				}
				if (component.cS_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
				}
				if (component.cS_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
				}
				if (component.cS_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				}
				if (component.cS_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
				}
				if (component.cS_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
				}
				break;
			case 12:
				gameObject.name = component.cS_.GetBestSkillValue().ToString();
				if (component.cS_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.cS_.s_gamedesign);
				}
				if (component.cS_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.cS_.s_programmieren);
				}
				if (component.cS_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.cS_.s_grafik);
				}
				if (component.cS_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.cS_.s_sound);
				}
				if (component.cS_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.cS_.s_pr);
				}
				if (component.cS_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.cS_.s_gametests);
				}
				if (component.cS_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.cS_.s_technik);
				}
				if (component.cS_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.cS_.s_forschen);
				}
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

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf && !string.Equals(searchStringA.ToLower(), uiObjects[7].GetComponent<InputField>().text.ToLower()))
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[7].GetComponent<InputField>().text;
			SetData();
		}
	}

	public void SetAuto()
	{
		if (!rS_)
		{
			return;
		}
		if (uiObjects[8].GetComponent<Toggle>().isOn)
		{
			rS_.leitenderGamedesigner = -1;
			if (menuDevGame_.gameObject.activeSelf)
			{
				menuDevGame_.SetLeitenderDesigner(null, manuellSelectet: false);
			}
			if (mDevAddon_.gameObject.activeSelf)
			{
				mDevAddon_.SetLeitenderDesigner(null, manuellSelectet: false);
			}
			if (menuEntwicklungsbericht_.gameObject.activeSelf)
			{
				menuEntwicklungsbericht_.SetLeitenderDesigner(null, manuellSelectet: false);
			}
		}
		else
		{
			if (menuDevGame_.gameObject.activeSelf && (bool)menuDevGame_.g_leitenderDesigner)
			{
				rS_.leitenderGamedesigner = menuDevGame_.g_leitenderDesigner.myID;
			}
			if (mDevAddon_.gameObject.activeSelf && (bool)menuDevGame_.g_leitenderDesigner)
			{
				rS_.leitenderGamedesigner = menuDevGame_.g_leitenderDesigner.myID;
			}
			if (menuEntwicklungsbericht_.gameObject.activeSelf && (bool)menuEntwicklungsbericht_.GetLeitenderEntwickler())
			{
				rS_.leitenderGamedesigner = menuEntwicklungsbericht_.GetLeitenderEntwickler().myID;
			}
		}
	}

	public void TOGGLE_Automatik()
	{
	}
}
