using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Arbeitsmarkt : MonoBehaviour
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

	private int gehaltSelected;

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
		string text = tS_.GetText(198);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount.ToString());
		uiObjects[4].GetComponent<Text>().text = text;
		uiObjects[7].GetComponent<Text>().text = "(" + GetAmountSelected() + ") <color=#9A221B>" + mS_.GetMoney(gehaltSelected, showDollar: true) + "</color>";
		UpdateMitarbeiterAnzahl();
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

	private void UpdateMitarbeiterAnzahl()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		int num13 = 0;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		int num17 = 0;
		int num18 = 0;
		int num19 = 0;
		int num20 = 0;
		int num21 = 0;
		int num22 = 0;
		int num23 = 0;
		int num24 = 0;
		int num25 = 0;
		int num26 = 0;
		int num27 = 0;
		int num28 = 0;
		int num29 = 0;
		int num30 = 0;
		int num31 = 0;
		int num32 = 0;
		int num33 = 0;
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i])
			{
				switch (mS_.arrayRoomScripts[i].typ)
				{
				case 1:
					num15 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 2:
					num16 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 3:
					num17 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 4:
					num18 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 5:
					num19 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 6:
					num20 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 7:
					num21 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 8:
					num22 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 10:
					num23 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 13:
					num24 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				case 17:
					num25 += mS_.arrayRoomScripts[i].GetArbeitsplaetze();
					break;
				}
			}
		}
		for (int j = 0; j < mS_.arrayCharactersScripts.Length; j++)
		{
			if (!mS_.arrayCharactersScripts[j])
			{
				continue;
			}
			characterScript characterScript2 = mS_.arrayCharactersScripts[j];
			if (!characterScript2)
			{
				continue;
			}
			num++;
			num2 += characterScript2.GetGehalt();
			switch (characterScript2.beruf)
			{
			case 0:
				num26++;
				break;
			case 1:
				num27++;
				break;
			case 2:
				num28++;
				break;
			case 3:
				num29++;
				break;
			case 4:
				num30++;
				break;
			case 5:
				num31++;
				break;
			case 6:
				num32++;
				break;
			case 7:
				num33++;
				break;
			}
			if ((bool)characterScript2.roomS_)
			{
				switch (characterScript2.roomS_.typ)
				{
				case 1:
					num4++;
					break;
				case 2:
					num5++;
					break;
				case 3:
					num6++;
					break;
				case 4:
					num7++;
					break;
				case 5:
					num8++;
					break;
				case 6:
					num9++;
					break;
				case 7:
					num10++;
					break;
				case 8:
					num11++;
					break;
				case 10:
					num12++;
					break;
				case 13:
					num13++;
					break;
				case 17:
					num14++;
					break;
				}
			}
			else
			{
				num3++;
			}
		}
		uiObjects[26].GetComponent<Text>().text = num3.ToString();
		uiObjects[16].GetComponent<Text>().text = num4 + "/" + num15;
		uiObjects[17].GetComponent<Text>().text = num5 + "/" + num16;
		uiObjects[18].GetComponent<Text>().text = num6 + "/" + num17;
		uiObjects[19].GetComponent<Text>().text = num7 + "/" + num18;
		uiObjects[20].GetComponent<Text>().text = num8 + "/" + num19;
		uiObjects[21].GetComponent<Text>().text = num9 + "/" + num20;
		uiObjects[22].GetComponent<Text>().text = num10 + "/" + num21;
		uiObjects[23].GetComponent<Text>().text = num11 + "/" + num22;
		uiObjects[24].GetComponent<Text>().text = num12 + "/" + num23;
		uiObjects[25].GetComponent<Text>().text = num13 + "/" + num24;
		uiObjects[37].GetComponent<Text>().text = num14 + "/" + num25;
		if (num26 > 0)
		{
			uiObjects[29].GetComponent<Text>().text = num26.ToString();
		}
		else
		{
			uiObjects[29].GetComponent<Text>().text = "";
		}
		if (num27 > 0)
		{
			uiObjects[30].GetComponent<Text>().text = num27.ToString();
		}
		else
		{
			uiObjects[30].GetComponent<Text>().text = "";
		}
		if (num28 > 0)
		{
			uiObjects[31].GetComponent<Text>().text = num28.ToString();
		}
		else
		{
			uiObjects[31].GetComponent<Text>().text = "";
		}
		if (num29 > 0)
		{
			uiObjects[32].GetComponent<Text>().text = num29.ToString();
		}
		else
		{
			uiObjects[32].GetComponent<Text>().text = "";
		}
		if (num30 > 0)
		{
			uiObjects[33].GetComponent<Text>().text = num30.ToString();
		}
		else
		{
			uiObjects[33].GetComponent<Text>().text = "";
		}
		if (num31 > 0)
		{
			uiObjects[34].GetComponent<Text>().text = num31.ToString();
		}
		else
		{
			uiObjects[34].GetComponent<Text>().text = "";
		}
		if (num32 > 0)
		{
			uiObjects[35].GetComponent<Text>().text = num32.ToString();
		}
		else
		{
			uiObjects[35].GetComponent<Text>().text = "";
		}
		if (num33 > 0)
		{
			uiObjects[36].GetComponent<Text>().text = num33.ToString();
		}
		else
		{
			uiObjects[36].GetComponent<Text>().text = "";
		}
		if (num > 0)
		{
			uiObjects[27].GetComponent<Text>().text = num.ToString();
		}
		else
		{
			uiObjects[27].GetComponent<Text>().text = "";
		}
		if (num2 > 0)
		{
			uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(num2, showDollar: true);
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = mS_.GetMoney(0L, showDollar: true);
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Arbeitsmarkt>().charAM_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	private void OnEnable()
	{
		Init();
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
		list.Add(tS_.GetText(1764));
		list.Add(tS_.GetText(1765));
		list.Add(tS_.GetText(2273));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init()
	{
		uiObjects[5].GetComponent<Toggle>().isOn = false;
		FindScripts();
		InitDropdowns();
		SetData();
	}

	private void SetData()
	{
		int num = 0;
		bool isOn = uiObjects[38].GetComponent<Toggle>().isOn;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Arbeitsmarkt");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			charArbeitsmarkt component = array[i].GetComponent<charArbeitsmarkt>();
			if ((bool)component && (!isOn || (isOn && !component.HasNegativPerk())))
			{
				num++;
				if (!Exists(uiObjects[0], component.myID))
				{
					Item_Arbeitsmarkt component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Arbeitsmarkt>();
					component2.characterID = component.myID;
					component2.charAM_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
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
			Item_Arbeitsmarkt component = gameObject.GetComponent<Item_Arbeitsmarkt>();
			switch (value)
			{
			case 0:
				gameObject.name = component.charAM_.myName;
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				}
				break;
			case 1:
				gameObject.name = component.charAM_.beruf.ToString();
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				}
				break;
			case 2:
				gameObject.name = component.charAM_.s_gamedesign.ToString();
				component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				break;
			case 3:
				gameObject.name = component.charAM_.s_programmieren.ToString();
				component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				break;
			case 4:
				gameObject.name = component.charAM_.s_grafik.ToString();
				component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				break;
			case 5:
				gameObject.name = component.charAM_.s_sound.ToString();
				component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				break;
			case 6:
				gameObject.name = component.charAM_.s_pr.ToString();
				component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				break;
			case 7:
				gameObject.name = component.charAM_.s_gametests.ToString();
				component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				break;
			case 8:
				gameObject.name = component.charAM_.s_technik.ToString();
				component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				break;
			case 9:
				gameObject.name = component.charAM_.s_forschen.ToString();
				component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				break;
			case 10:
				gameObject.name = component.charAM_.GetGehalt().ToString();
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				}
				break;
			case 11:
				gameObject.name = component.charAM_.GetBestSkillValue().ToString();
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				}
				break;
			case 12:
				gameObject.name = component.charAM_.wochenAmArbeitsmarkt.ToString();
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
				}
				break;
			case 13:
				gameObject.name = component.charAM_.AmountPositivePerks().ToString();
				if (component.charAM_.GetBestSkill() == 0)
				{
					component.SetData(tS_.GetText(119), component.charAM_.s_gamedesign);
				}
				if (component.charAM_.GetBestSkill() == 1)
				{
					component.SetData(tS_.GetText(120), component.charAM_.s_programmieren);
				}
				if (component.charAM_.GetBestSkill() == 2)
				{
					component.SetData(tS_.GetText(121), component.charAM_.s_grafik);
				}
				if (component.charAM_.GetBestSkill() == 3)
				{
					component.SetData(tS_.GetText(122), component.charAM_.s_sound);
				}
				if (component.charAM_.GetBestSkill() == 4)
				{
					component.SetData(tS_.GetText(123), component.charAM_.s_pr);
				}
				if (component.charAM_.GetBestSkill() == 5)
				{
					component.SetData(tS_.GetText(124), component.charAM_.s_gametests);
				}
				if (component.charAM_.GetBestSkill() == 6)
				{
					component.SetData(tS_.GetText(125), component.charAM_.s_technik);
				}
				if (component.charAM_.GetBestSkill() == 7)
				{
					component.SetData(tS_.GetText(126), component.charAM_.s_forschen);
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

	public void TOGGLE_All()
	{
		sfx_.PlaySound(12, force: true);
		bool isOn = uiObjects[5].GetComponent<Toggle>().isOn;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				gameObject.GetComponent<Item_Arbeitsmarkt>().uiObjects[8].GetComponent<Toggle>().isOn = isOn;
			}
		}
	}

	public void TOGGLE_NoNegativPerk()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
		TOGGLE_All();
	}

	public void BUTTON_Entfernen()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_Arbeitsmarkt component = gameObject.GetComponent<Item_Arbeitsmarkt>();
				if ((bool)component && component.uiObjects[8].GetComponent<Toggle>().isOn)
				{
					component.BUTTON_Remove();
				}
			}
		}
	}

	public void BUTTON_Einstellen()
	{
		bool flag = false;
		createCharScript component = main_.GetComponent<createCharScript>();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_Arbeitsmarkt component2 = gameObject.GetComponent<Item_Arbeitsmarkt>();
				if ((bool)component2 && component2.uiObjects[8].GetComponent<Toggle>().isOn)
				{
					characterScript characterScript2 = component.CreateCharacter(component2.charAM_.myID, component2.charAM_.male, component2.charAM_.model_body);
					characterScript2.model_body = component2.charAM_.model_body;
					characterScript2.model_eyes = component2.charAM_.model_eyes;
					characterScript2.model_hair = component2.charAM_.model_hair;
					characterScript2.model_beard = component2.charAM_.model_beard;
					characterScript2.model_skinColor = component2.charAM_.model_skinColor;
					characterScript2.model_hairColor = component2.charAM_.model_hairColor;
					characterScript2.model_beardColor = component2.charAM_.model_beardColor;
					characterScript2.model_HoseColor = component2.charAM_.model_HoseColor;
					characterScript2.model_ShirtColor = component2.charAM_.model_ShirtColor;
					characterScript2.model_Add1Color = component2.charAM_.model_Add1Color;
					characterScript2.gameObject.transform.GetChild(0).GetComponent<characterGFXScript>().Init(forcedClothes: true);
					mS_.CopyArbeitsmarktCharacterData(component2.charAM_, characterScript2);
					pcS_.PickFromExternObject(characterScript2.gameObject);
					component2.charAM_.RemoveFromArbeitsmarkt(eingestellt: true);
					flag = true;
				}
			}
		}
		if (flag)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.CloseMenu();
			base.gameObject.SetActive(value: false);
		}
	}

	public int GetAmountSelected()
	{
		AnzahlBewerber();
		int num = 0;
		gehaltSelected = 0;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject && gameObject.GetComponent<Item_Arbeitsmarkt>().uiObjects[8].GetComponent<Toggle>().isOn)
			{
				gehaltSelected += gameObject.GetComponent<Item_Arbeitsmarkt>().charAM_.GetGehalt();
				num++;
			}
		}
		return num;
	}

	private void AnzahlBewerber()
	{
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		int num7 = 0;
		int num8 = 0;
		float num9 = 0f;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				num9 += 1f;
				switch (gameObject.GetComponent<Item_Arbeitsmarkt>().charAM_.beruf)
				{
				case 0:
					num++;
					break;
				case 1:
					num2++;
					break;
				case 2:
					num3++;
					break;
				case 3:
					num4++;
					break;
				case 4:
					num5++;
					break;
				case 5:
					num6++;
					break;
				case 6:
					num7++;
					break;
				case 7:
					num8++;
					break;
				}
			}
		}
		if (num > 0)
		{
			uiObjects[8].GetComponent<Text>().text = num.ToString();
		}
		else
		{
			uiObjects[8].GetComponent<Text>().text = "";
		}
		if (num2 > 0)
		{
			uiObjects[9].GetComponent<Text>().text = num2.ToString();
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = "";
		}
		if (num3 > 0)
		{
			uiObjects[10].GetComponent<Text>().text = num3.ToString();
		}
		else
		{
			uiObjects[10].GetComponent<Text>().text = "";
		}
		if (num4 > 0)
		{
			uiObjects[11].GetComponent<Text>().text = num4.ToString();
		}
		else
		{
			uiObjects[11].GetComponent<Text>().text = "";
		}
		if (num5 > 0)
		{
			uiObjects[12].GetComponent<Text>().text = num5.ToString();
		}
		else
		{
			uiObjects[12].GetComponent<Text>().text = "";
		}
		if (num6 > 0)
		{
			uiObjects[13].GetComponent<Text>().text = num6.ToString();
		}
		else
		{
			uiObjects[13].GetComponent<Text>().text = "";
		}
		if (num7 > 0)
		{
			uiObjects[14].GetComponent<Text>().text = num7.ToString();
		}
		else
		{
			uiObjects[14].GetComponent<Text>().text = "";
		}
		if (num8 > 0)
		{
			uiObjects[15].GetComponent<Text>().text = num8.ToString();
		}
		else
		{
			uiObjects[15].GetComponent<Text>().text = "";
		}
	}
}
