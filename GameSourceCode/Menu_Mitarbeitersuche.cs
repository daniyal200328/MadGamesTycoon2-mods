using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Mitarbeitersuche : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private roomDataScript rdS_;

	private roomScript rS_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public int[] price;

	public float[] chance;

	public float[] workPoints;

	private int perk;

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

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[0].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(137));
		list.Add(tS_.GetText(138));
		list.Add(tS_.GetText(139));
		list.Add(tS_.GetText(140));
		list.Add(tS_.GetText(141));
		list.Add(tS_.GetText(142));
		list.Add(tS_.GetText(143));
		list.Add(tS_.GetText(144));
		uiObjects[0].GetComponent<Dropdown>().ClearOptions();
		uiObjects[0].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[0].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[1].name);
		list = new List<string>();
		list.Add("<b>[30-35]</b> " + tS_.GetText(1710));
		list.Add("<b>[50-55]</b> " + tS_.GetText(1711));
		list.Add("<b>[70-75]</b> " + tS_.GetText(1712));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[9].name);
		list = new List<string>();
		list.Add(tS_.GetText(1966));
		list.Add(tS_.GetText(825));
		list.Add(tS_.GetText(826));
		uiObjects[9].GetComponent<Dropdown>().ClearOptions();
		uiObjects[9].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[9].GetComponent<Dropdown>().value = value;
	}

	public void Init(roomScript room_)
	{
		rS_ = room_;
		FindScripts();
		InitDropdowns();
		SetData();
		int num = PlayerPrefs.GetInt("mitarbeitersuche_perks");
		if (num == 0)
		{
			num = 2;
		}
		BUTTON_Perk(num);
	}

	private void SetData()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		string text = tS_.GetText(1716);
		text = text.Replace("<NUM>", mS_.GetMoney(price[value], showDollar: true));
		uiObjects[2].GetComponent<Text>().text = text;
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(GetChance(value)).ToString();
		uiObjects[5].GetComponent<Image>().fillAmount = GetChance(value) * 0.01f;
		uiObjects[5].GetComponent<Image>().color = GetValColor(GetChance(value));
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(workPoints[value]).ToString();
		uiObjects[6].GetComponent<Image>().fillAmount = workPoints[value] * 0.01f;
		uiObjects[6].GetComponent<Image>().color = GetValColor(workPoints[value]);
	}

	public float GetChance(int i)
	{
		FindScripts();
		float num = chance[i];
		num += (float)mS_.GetStudioLevel(mS_.studioPoints) * 1.5f;
		num += (float)(mS_.year - 1976) * 0.3f;
		num -= (float)mS_.difficulty;
		if (num > 100f)
		{
			num = 100f;
		}
		if (uiObjects[10].GetComponent<Toggle>().isOn)
		{
			num *= 0.5f;
		}
		if (num < 1f)
		{
			num = 1f;
		}
		if (num > 100f)
		{
			num = 100f;
		}
		return num;
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

	public void DROPDOWN_Erfahrung()
	{
		PlayerPrefs.SetInt(uiObjects[0].name, uiObjects[0].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[1].name, uiObjects[1].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[9].name, uiObjects[9].GetComponent<Dropdown>().value);
		SetData();
	}

	public void DROPDOWN_Profession()
	{
		PlayerPrefs.SetInt(uiObjects[0].name, uiObjects[0].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[1].name, uiObjects[1].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[9].name, uiObjects[9].GetComponent<Dropdown>().value);
		SetData();
	}

	public void DROPDOWN_Geschlecht()
	{
		PlayerPrefs.SetInt(uiObjects[0].name, uiObjects[0].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[1].name, uiObjects[1].GetComponent<Dropdown>().value);
		PlayerPrefs.SetInt(uiObjects[9].name, uiObjects[9].GetComponent<Dropdown>().value);
		SetData();
	}

	public void TOGGLE_NoBadPerks()
	{
		SetData();
	}

	public void BUTTON_Perk(int i)
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		perk = i;
		for (int j = 0; j < uiObjects[8].transform.childCount; j++)
		{
			if ((bool)uiObjects[8].transform.GetChild(j))
			{
				uiObjects[8].transform.GetChild(j).GetComponent<Image>().color = Color.white;
			}
		}
		if ((bool)uiObjects[8].transform.GetChild(i))
		{
			uiObjects[8].transform.GetChild(i).GetComponent<Image>().color = guiMain_.colors[0];
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		if (!rS_)
		{
			return;
		}
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		if (mS_.NotEnoughMoney(price[value]))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(price[value], 24);
		taskMitarbeitersuche taskMitarbeitersuche2 = guiMain_.AddTask_Mitarbeitersuche();
		taskMitarbeitersuche2.Init(fromSavegame: false);
		taskMitarbeitersuche2.beruf = uiObjects[0].GetComponent<Dropdown>().value;
		taskMitarbeitersuche2.automatic = uiObjects[7].GetComponent<Toggle>().isOn;
		taskMitarbeitersuche2.points = workPoints[uiObjects[1].GetComponent<Dropdown>().value];
		taskMitarbeitersuche2.pointsLeft = workPoints[uiObjects[1].GetComponent<Dropdown>().value];
		taskMitarbeitersuche2.berufserfahrung = uiObjects[1].GetComponent<Dropdown>().value;
		taskMitarbeitersuche2.perk = perk;
		taskMitarbeitersuche2.geschlecht = uiObjects[9].GetComponent<Dropdown>().value;
		taskMitarbeitersuche2.noBadPerks = uiObjects[10].GetComponent<Toggle>().isOn;
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskMitarbeitersuche2.myID;
		}
		PlayerPrefs.SetInt("mitarbeitersuche_perks", perk);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
