using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Personal_InRoom : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private roomDataScript rdS_;

	public int roomID = -1;

	private roomScript rS_;

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

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
	}

	private void Update()
	{
		if ((bool)rS_)
		{
			string text = tS_.GetText(184);
			text = text.Replace("<NUM>", uiObjects[0].transform.childCount + " / " + rS_.GetArbeitsplaetze());
			uiObjects[4].GetComponent<Text>().text = text;
			uiObjects[9].GetComponent<Text>().text = rS_.AnzahlArbeitsplaetzeBisUberfuellt().ToString();
			uiObjects[8].GetComponent<Text>().text = "(" + GetAmountSelected() + ")";
			if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
			{
				uiObjects[3].GetComponent<Scrollbar>().value = 1f;
			}
		}
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

	public void Init(int roomID_)
	{
		FindScripts();
		roomID = roomID_;
		int num = 0;
		GameObject gameObject = GameObject.Find("Room_" + roomID_);
		if ((bool)gameObject)
		{
			rS_ = gameObject.GetComponent<roomScript>();
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.roomID == roomID)
				{
					num++;
					Item_Personal_InRoom component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Personal_InRoom>();
					component2.characterID = component.myID;
					component2.cS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.rdS_ = rdS_;
				}
			}
		}
		DROPDOWN_Sort();
		if ((bool)rS_)
		{
			string text = tS_.GetText(1301);
			text = text.Replace("<NAME>", rdS_.GetName(rS_.typ));
			switch (rS_.typ)
			{
			case 1:
				text = text.Replace("<TEXT>", tS_.GetText(137) + ", " + tS_.GetText(138) + ", " + tS_.GetText(139) + ", " + tS_.GetText(140));
				break;
			case 4:
				text = text.Replace("<TEXT>", tS_.GetText(139));
				break;
			case 5:
				text = text.Replace("<TEXT>", tS_.GetText(140));
				break;
			case 10:
				text = text.Replace("<TEXT>", tS_.GetText(138));
				break;
			case 3:
				text = text.Replace("<TEXT>", tS_.GetText(142));
				break;
			case 7:
				text = text.Replace("<TEXT>", tS_.GetText(141));
				break;
			case 6:
				text = text.Replace("<TEXT>", tS_.GetText(141));
				break;
			case 2:
				text = text.Replace("<TEXT>", tS_.GetText(144));
				break;
			case 13:
				text = tS_.GetText(31);
				break;
			case 17:
				text = text.Replace("<TEXT>", tS_.GetText(143));
				break;
			case 8:
				text = text.Replace("<TEXT>", tS_.GetText(143));
				break;
			}
			uiObjects[7].GetComponent<Text>().text = text;
		}
		uiObjects[5].GetComponent<Toggle>().isOn = false;
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
			Item_Personal_InRoom component = gameObject.GetComponent<Item_Personal_InRoom>();
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
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				num17 += 1f;
				characterScript cS_ = gameObject.GetComponent<Item_Personal_InRoom>().cS_;
				num += cS_.s_gamedesign;
				num2 += cS_.s_programmieren;
				num3 += cS_.s_grafik;
				num4 += cS_.s_sound;
				num5 += cS_.s_pr;
				num6 += cS_.s_gametests;
				num7 += cS_.s_technik;
				num8 += cS_.s_forschen;
				switch (cS_.beruf)
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
		if (num17 > 0f)
		{
			SetBalken(uiObjects[10], num / num17);
			SetBalken(uiObjects[11], num2 / num17);
			SetBalken(uiObjects[12], num3 / num17);
			SetBalken(uiObjects[13], num4 / num17);
			SetBalken(uiObjects[14], num5 / num17);
			SetBalken(uiObjects[15], num6 / num17);
			SetBalken(uiObjects[16], num7 / num17);
			SetBalken(uiObjects[17], num8 / num17);
		}
		else
		{
			SetBalken(uiObjects[10], 0f);
			SetBalken(uiObjects[11], 0f);
			SetBalken(uiObjects[12], 0f);
			SetBalken(uiObjects[13], 0f);
			SetBalken(uiObjects[14], 0f);
			SetBalken(uiObjects[15], 0f);
			SetBalken(uiObjects[16], 0f);
			SetBalken(uiObjects[17], 0f);
		}
		if (num9 > 0)
		{
			uiObjects[18].GetComponent<Text>().text = num9.ToString();
		}
		else
		{
			uiObjects[18].GetComponent<Text>().text = "";
		}
		if (num10 > 0)
		{
			uiObjects[19].GetComponent<Text>().text = num10.ToString();
		}
		else
		{
			uiObjects[19].GetComponent<Text>().text = "";
		}
		if (num11 > 0)
		{
			uiObjects[20].GetComponent<Text>().text = num11.ToString();
		}
		else
		{
			uiObjects[20].GetComponent<Text>().text = "";
		}
		if (num12 > 0)
		{
			uiObjects[21].GetComponent<Text>().text = num12.ToString();
		}
		else
		{
			uiObjects[21].GetComponent<Text>().text = "";
		}
		if (num13 > 0)
		{
			uiObjects[22].GetComponent<Text>().text = num13.ToString();
		}
		else
		{
			uiObjects[22].GetComponent<Text>().text = "";
		}
		if (num14 > 0)
		{
			uiObjects[23].GetComponent<Text>().text = num14.ToString();
		}
		else
		{
			uiObjects[23].GetComponent<Text>().text = "";
		}
		if (num15 > 0)
		{
			uiObjects[24].GetComponent<Text>().text = num15.ToString();
		}
		else
		{
			uiObjects[24].GetComponent<Text>().text = "";
		}
		if (num16 > 0)
		{
			uiObjects[25].GetComponent<Text>().text = num16.ToString();
		}
		else
		{
			uiObjects[25].GetComponent<Text>().text = "";
		}
		uiObjects[26].GetComponent<Text>().text = mS_.Round(num, 1).ToString();
		uiObjects[27].GetComponent<Text>().text = mS_.Round(num2, 1).ToString();
		uiObjects[28].GetComponent<Text>().text = mS_.Round(num3, 1).ToString();
		uiObjects[29].GetComponent<Text>().text = mS_.Round(num4, 1).ToString();
		uiObjects[30].GetComponent<Text>().text = mS_.Round(num5, 1).ToString();
		uiObjects[31].GetComponent<Text>().text = mS_.Round(num6, 1).ToString();
		uiObjects[32].GetComponent<Text>().text = mS_.Round(num7, 1).ToString();
		uiObjects[33].GetComponent<Text>().text = mS_.Round(num8, 1).ToString();
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
