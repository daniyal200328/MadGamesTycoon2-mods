using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MitarbeiterUebersicht : MonoBehaviour
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

	private InputField myInputField;

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
		if (!myInputField)
		{
			myInputField = uiObjects[44].GetComponent<InputField>();
		}
	}

	private void Update()
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
		uiObjects[16].GetComponent<Text>().text = num3.ToString();
		uiObjects[6].GetComponent<Text>().text = num4 + "/" + num15;
		uiObjects[7].GetComponent<Text>().text = num5 + "/" + num16;
		uiObjects[8].GetComponent<Text>().text = num6 + "/" + num17;
		uiObjects[9].GetComponent<Text>().text = num7 + "/" + num18;
		uiObjects[10].GetComponent<Text>().text = num8 + "/" + num19;
		uiObjects[11].GetComponent<Text>().text = num9 + "/" + num20;
		uiObjects[12].GetComponent<Text>().text = num10 + "/" + num21;
		uiObjects[13].GetComponent<Text>().text = num11 + "/" + num22;
		uiObjects[14].GetComponent<Text>().text = num12 + "/" + num23;
		uiObjects[15].GetComponent<Text>().text = num13 + "/" + num24;
		uiObjects[45].GetComponent<Text>().text = num14 + "/" + num25;
		string text = tS_.GetText(184);
		text = text.Replace("<NUM>", num.ToString());
		uiObjects[4].GetComponent<Text>().text = text;
		text = tS_.GetText(200);
		text = text.Replace("<NUM>", mS_.GetMoney(num2, showDollar: true));
		uiObjects[17].GetComponent<Text>().text = text;
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		uiObjects[19].GetComponent<Text>().text = "(" + GetAmountSelected() + ")";
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
				SetData(check: true);
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Personal_InRoom>().cS_.myID == id_)
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

	public void Init()
	{
		uiObjects[5].GetComponent<Toggle>().isOn = false;
		FindScripts();
		InitDropdowns();
		SetData(check: false);
	}

	private void SetData(bool check)
	{
		int num = 0;
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
			bool flag = false;
			if (myInputField.text.Length > 0)
			{
				string myName = characterScript2.myName;
				searchStringA = searchStringA.ToLower();
				if (myName.ToLower().Contains(searchStringA))
				{
					flag = true;
				}
			}
			else
			{
				flag = true;
			}
			if (!flag)
			{
				continue;
			}
			num++;
			if (check)
			{
				if (!Exists(uiObjects[0], characterScript2.myID))
				{
					Item_Personal_InRoom component = Object.Instantiate(uiPrefabs[0], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Personal_InRoom>();
					component.characterID = characterScript2.myID;
					component.cS_ = characterScript2;
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.rdS_ = rdS_;
				}
			}
			else
			{
				Item_Personal_InRoom component2 = Object.Instantiate(uiPrefabs[0], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Personal_InRoom>();
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
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[18]);
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf && !string.Equals(searchStringA.ToLower(), uiObjects[44].GetComponent<InputField>().text.ToLower()))
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[44].GetComponent<InputField>().text;
			SetData(check: false);
		}
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
			Item_Personal_InRoom component = gameObject.GetComponent<Item_Personal_InRoom>();
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
				gameObject.GetComponent<Item_Personal_InRoom>().uiObjects[8].GetComponent<Toggle>().isOn = isOn;
			}
		}
	}

	public void BUTTON_Entlassen()
	{
		bool flag = false;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_Personal_InRoom component = gameObject.GetComponent<Item_Personal_InRoom>();
				if ((bool)component && component.cS_.myID != 1 && component.uiObjects[8].GetComponent<Toggle>().isOn)
				{
					guiMain_.uiObjects[27].GetComponent<Menu_PersonalEntlassen>().AddCharacter(component.cS_);
					flag = true;
				}
			}
		}
		if (flag)
		{
			sfx_.PlaySound(3, force: true);
			guiMain_.ActivateMenu(guiMain_.uiObjects[27]);
		}
	}

	public void BUTTON_Select()
	{
		bool flag = false;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_Personal_InRoom component = gameObject.GetComponent<Item_Personal_InRoom>();
				if ((bool)component && component.uiObjects[8].GetComponent<Toggle>().isOn)
				{
					pcS_.PickFromExternObject(component.cS_.gameObject);
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
		DrawBalkenDurchschnitt();
		int num = 0;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject && gameObject.GetComponent<Item_Personal_InRoom>().uiObjects[8].GetComponent<Toggle>().isOn)
			{
				num++;
			}
		}
		return num;
	}

	private void DrawBalkenDurchschnitt()
	{
		float num = 0f;
		float num2 = 0f;
		float num3 = 0f;
		float num4 = 0f;
		float num5 = 0f;
		float num6 = 0f;
		float num7 = 0f;
		float num8 = 0f;
		int num9 = 0;
		int num10 = 0;
		int num11 = 0;
		int num12 = 0;
		int num13 = 0;
		int num14 = 0;
		int num15 = 0;
		int num16 = 0;
		float num17 = 0f;
		for (int i = 0; i < mS_.arrayCharactersScripts.Length; i++)
		{
			if (!mS_.arrayCharactersScripts[i])
			{
				continue;
			}
			characterScript characterScript2 = mS_.arrayCharactersScripts[i];
			if ((bool)characterScript2)
			{
				num17 += 1f;
				num += characterScript2.s_gamedesign;
				num2 += characterScript2.s_programmieren;
				num3 += characterScript2.s_grafik;
				num4 += characterScript2.s_sound;
				num5 += characterScript2.s_pr;
				num6 += characterScript2.s_gametests;
				num7 += characterScript2.s_technik;
				num8 += characterScript2.s_forschen;
				switch (characterScript2.beruf)
				{
				case 0:
					num9++;
					break;
				case 1:
					num10++;
					break;
				case 2:
					num11++;
					break;
				case 3:
					num12++;
					break;
				case 4:
					num13++;
					break;
				case 5:
					num14++;
					break;
				case 6:
					num15++;
					break;
				case 7:
					num16++;
					break;
				}
			}
		}
		SetBalken(uiObjects[20], num / num17);
		SetBalken(uiObjects[21], num2 / num17);
		SetBalken(uiObjects[22], num3 / num17);
		SetBalken(uiObjects[23], num4 / num17);
		SetBalken(uiObjects[24], num5 / num17);
		SetBalken(uiObjects[25], num6 / num17);
		SetBalken(uiObjects[26], num7 / num17);
		SetBalken(uiObjects[27], num8 / num17);
		if (num9 > 0)
		{
			uiObjects[28].GetComponent<Text>().text = num9.ToString();
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = "";
		}
		if (num10 > 0)
		{
			uiObjects[29].GetComponent<Text>().text = num10.ToString();
		}
		else
		{
			uiObjects[29].GetComponent<Text>().text = "";
		}
		if (num11 > 0)
		{
			uiObjects[30].GetComponent<Text>().text = num11.ToString();
		}
		else
		{
			uiObjects[30].GetComponent<Text>().text = "";
		}
		if (num12 > 0)
		{
			uiObjects[31].GetComponent<Text>().text = num12.ToString();
		}
		else
		{
			uiObjects[31].GetComponent<Text>().text = "";
		}
		if (num13 > 0)
		{
			uiObjects[32].GetComponent<Text>().text = num13.ToString();
		}
		else
		{
			uiObjects[32].GetComponent<Text>().text = "";
		}
		if (num14 > 0)
		{
			uiObjects[33].GetComponent<Text>().text = num14.ToString();
		}
		else
		{
			uiObjects[33].GetComponent<Text>().text = "";
		}
		if (num15 > 0)
		{
			uiObjects[34].GetComponent<Text>().text = num15.ToString();
		}
		else
		{
			uiObjects[34].GetComponent<Text>().text = "";
		}
		if (num16 > 0)
		{
			uiObjects[35].GetComponent<Text>().text = num16.ToString();
		}
		else
		{
			uiObjects[35].GetComponent<Text>().text = "";
		}
		uiObjects[36].GetComponent<Text>().text = mS_.Round(num, 1).ToString();
		uiObjects[37].GetComponent<Text>().text = mS_.Round(num2, 1).ToString();
		uiObjects[38].GetComponent<Text>().text = mS_.Round(num3, 1).ToString();
		uiObjects[39].GetComponent<Text>().text = mS_.Round(num4, 1).ToString();
		uiObjects[40].GetComponent<Text>().text = mS_.Round(num5, 1).ToString();
		uiObjects[41].GetComponent<Text>().text = mS_.Round(num6, 1).ToString();
		uiObjects[42].GetComponent<Text>().text = mS_.Round(num7, 1).ToString();
		uiObjects[43].GetComponent<Text>().text = mS_.Round(num8, 1).ToString();
	}

	private void SetBalken(GameObject go, float val)
	{
		go.transform.Find("Value").GetComponent<Text>().text = mS_.Round(val, 1).ToString();
		go.transform.Find("Fill").GetComponent<Image>().fillAmount = val * 0.01f;
		go.transform.Find("Fill").GetComponent<Image>().color = GetValColor(val);
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}
}
