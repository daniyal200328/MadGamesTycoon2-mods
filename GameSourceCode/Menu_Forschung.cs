using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Forschung : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private themes themes_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeature_;

	private unlockScript unlock_;

	private forschungSonstiges fS_;

	private copyProtect copyProtect_;

	public int roomID = -1;

	private int forschungsTyp;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public InputField myInputField;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!hardwareFeature_)
		{
			hardwareFeature_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!copyProtect_)
		{
			copyProtect_ = main_.GetComponent<copyProtect>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
		}
		if (!fS_)
		{
			fS_ = main_.GetComponent<forschungSonstiges>();
		}
		if (!myInputField)
		{
			myInputField = uiObjects[7].GetComponent<InputField>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		if (uiObjects[4].GetComponent<Toggle>().isOn)
		{
			uiObjects[8].GetComponent<Toggle>().interactable = true;
		}
		else
		{
			uiObjects[8].GetComponent<Toggle>().interactable = false;
		}
		uiObjects[10].GetComponent<Button>().interactable = uiObjects[8].GetComponent<Toggle>().isOn;
	}

	private void OnEnable()
	{
		uiObjects[7].GetComponent<InputField>().text = "";
		FindScripts();
	}

	public int GetAmountForschung(int i, bool getUnerforschtesObjekt)
	{
		FindScripts();
		int num = 0;
		switch (i)
		{
		case 0:
		{
			for (int m = 0; m < genres_.genres_RES_POINTS.Length; m++)
			{
				if (genres_.genres_UNLOCK[m] && !genres_.IsErforscht(m) && !genres_.BereitsInAnderenRaumAktiv(m))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return m;
					}
				}
			}
			break;
		}
		case 1:
		{
			for (int l = 0; l < themes_.themes_RES_POINTS_LEFT.Length; l++)
			{
				if (!themes_.IsErforscht(l) && !themes_.BereitsInAnderenRaumAktiv(l))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return l;
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int num2 = 0; num2 < eF_.engineFeatures_RES_POINTS.Length; num2++)
			{
				if (eF_.engineFeatures_UNLOCK[num2] && !eF_.IsErforscht(num2) && !eF_.BereitsInAnderenRaumAktiv(num2))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return num2;
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int k = 0; k < gF_.gameplayFeatures_RES_POINTS.Length; k++)
			{
				if (gF_.gameplayFeatures_UNLOCK[k] && !gF_.IsErforscht(k) && !gF_.BereitsInAnderenRaumAktiv(k))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return k;
					}
				}
			}
			break;
		}
		case 4:
		{
			for (int n = 0; n < hardware_.hardware_RES_POINTS.Length; n++)
			{
				if (hardware_.hardware_UNLOCK[n] && !hardware_.IsErforscht(n) && !hardware_.BereitsInAnderenRaumAktiv(n))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return n;
					}
				}
			}
			break;
		}
		case 5:
			if (!fS_.IsErforscht(0) && !fS_.BereitsInAnderenRaumAktiv(0))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 0;
				}
			}
			if (!fS_.IsErforscht(1) && fS_.IsErforscht(0) && !fS_.BereitsInAnderenRaumAktiv(1))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 1;
				}
			}
			if (!fS_.IsErforscht(2) && fS_.IsErforscht(1) && !fS_.BereitsInAnderenRaumAktiv(2))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 2;
				}
			}
			if (!fS_.IsErforscht(3) && fS_.IsErforscht(2) && !fS_.BereitsInAnderenRaumAktiv(3))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 3;
				}
			}
			if (!fS_.IsErforscht(40) && fS_.IsErforscht(3) && !fS_.BereitsInAnderenRaumAktiv(40))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 40;
				}
			}
			if (!fS_.IsErforscht(35) && !fS_.BereitsInAnderenRaumAktiv(35))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 35;
				}
			}
			if (!fS_.IsErforscht(36) && !fS_.BereitsInAnderenRaumAktiv(36))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 36;
				}
			}
			if (!fS_.IsErforscht(28) && !fS_.BereitsInAnderenRaumAktiv(28))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 28;
				}
			}
			if (!fS_.IsErforscht(29) && !fS_.BereitsInAnderenRaumAktiv(29))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 29;
				}
			}
			if (!fS_.IsErforscht(30) && !fS_.BereitsInAnderenRaumAktiv(30))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 30;
				}
			}
			if (!fS_.IsErforscht(31) && !fS_.BereitsInAnderenRaumAktiv(31))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 31;
				}
			}
			if (!fS_.IsErforscht(32) && !fS_.BereitsInAnderenRaumAktiv(32))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 32;
				}
			}
			if (!fS_.IsErforscht(33) && !fS_.BereitsInAnderenRaumAktiv(33))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 33;
				}
			}
			if (!fS_.IsErforscht(34) && !fS_.BereitsInAnderenRaumAktiv(34))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 34;
				}
			}
			if (!fS_.IsErforscht(38) && !fS_.BereitsInAnderenRaumAktiv(38))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 38;
				}
			}
			if (!fS_.IsErforscht(39) && !fS_.BereitsInAnderenRaumAktiv(39))
			{
				num++;
				if (getUnerforschtesObjekt)
				{
					return 39;
				}
			}
			break;
		case 6:
		{
			for (int j = 0; j < hardwareFeature_.hardFeat_RES_POINTS.Length; j++)
			{
				if (hardwareFeature_.hardFeat_UNLOCK[j] && !hardwareFeature_.IsErforscht(j) && !hardwareFeature_.BereitsInAnderenRaumAktiv(j))
				{
					num++;
					if (getUnerforschtesObjekt)
					{
						return j;
					}
				}
			}
			break;
		}
		}
		if (getUnerforschtesObjekt)
		{
			return -1;
		}
		return num;
	}

	public void Init(int roomID_, int i)
	{
		FindScripts();
		roomID = roomID_;
		forschungsTyp = i;
		InitDropdowns();
		switch (i)
		{
		case 0:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(158);
			break;
		case 1:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(159);
			break;
		case 2:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(160);
			break;
		case 3:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(161);
			break;
		case 4:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(162);
			break;
		case 5:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(163);
			break;
		case 6:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1617);
			break;
		}
		string text = tS_.GetText(2270);
		text = text.Replace("<NAME>", uiObjects[1].GetComponent<Text>().text);
		uiObjects[9].GetComponent<Text>().text = text;
		switch (i)
		{
		case 0:
		{
			for (int m = 0; m < genres_.genres_RES_POINTS.Length; m++)
			{
				if (genres_.genres_UNLOCK[m] && !genres_.IsErforscht(m) && !genres_.BereitsInAnderenRaumAktiv(m))
				{
					string text21 = genres_.GetName(m);
					searchStringA = searchStringA.ToLower();
					text21 = text21.ToLower();
					if (myInputField.text.Length <= 0 || text21.Contains(searchStringA))
					{
						Item_Genre_Forschung component4 = Object.Instantiate(uiPrefabs[1], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Genre_Forschung>();
						component4.myID = m;
						component4.mS_ = mS_;
						component4.tS_ = tS_;
						component4.sfx_ = sfx_;
						component4.guiMain_ = guiMain_;
						component4.genres_ = genres_;
					}
				}
			}
			break;
		}
		case 1:
		{
			for (int num = 0; num < themes_.themes_RES_POINTS_LEFT.Length; num++)
			{
				if (themes_.IsErforscht(num) || themes_.BereitsInAnderenRaumAktiv(num))
				{
					continue;
				}
				bool flag = false;
				if (myInputField.text.Length > 0)
				{
					string text23 = tS_.GetThemes(num);
					searchStringA = searchStringA.ToLower();
					if (text23.ToLower().Contains(searchStringA))
					{
						flag = true;
					}
				}
				else
				{
					flag = true;
				}
				if (flag)
				{
					Item_Themes_Forschung component6 = Object.Instantiate(uiPrefabs[2], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Themes_Forschung>();
					component6.myID = num;
					component6.mS_ = mS_;
					component6.tS_ = tS_;
					component6.sfx_ = sfx_;
					component6.guiMain_ = guiMain_;
					component6.themes_ = themes_;
				}
			}
			break;
		}
		case 2:
		{
			for (int k = 0; k < eF_.engineFeatures_RES_POINTS.Length; k++)
			{
				if (eF_.engineFeatures_UNLOCK[k] && !eF_.IsErforscht(k) && !eF_.BereitsInAnderenRaumAktiv(k))
				{
					string text3 = eF_.GetName(k);
					searchStringA = searchStringA.ToLower();
					text3 = text3.ToLower();
					if (myInputField.text.Length <= 0 || text3.Contains(searchStringA))
					{
						Item_EngineFeatures_Forschung component2 = Object.Instantiate(uiPrefabs[3], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_EngineFeatures_Forschung>();
						component2.myID = k;
						component2.mS_ = mS_;
						component2.tS_ = tS_;
						component2.sfx_ = sfx_;
						component2.guiMain_ = guiMain_;
						component2.eF_ = eF_;
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int n = 0; n < gF_.gameplayFeatures_RES_POINTS.Length; n++)
			{
				if (gF_.gameplayFeatures_UNLOCK[n] && !gF_.IsErforscht(n) && !gF_.BereitsInAnderenRaumAktiv(n))
				{
					string text22 = gF_.GetName(n);
					searchStringA = searchStringA.ToLower();
					text22 = text22.ToLower();
					if (myInputField.text.Length <= 0 || text22.Contains(searchStringA))
					{
						Item_GameplayFeatures_Forschung component5 = Object.Instantiate(uiPrefabs[4], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_GameplayFeatures_Forschung>();
						component5.myID = n;
						component5.mS_ = mS_;
						component5.tS_ = tS_;
						component5.sfx_ = sfx_;
						component5.guiMain_ = guiMain_;
						component5.gF_ = gF_;
					}
				}
			}
			break;
		}
		case 4:
		{
			for (int l = 0; l < hardware_.hardware_RES_POINTS.Length; l++)
			{
				if (hardware_.hardware_UNLOCK[l] && !hardware_.IsErforscht(l) && !hardware_.BereitsInAnderenRaumAktiv(l))
				{
					string text20 = hardware_.GetName(l);
					searchStringA = searchStringA.ToLower();
					text20 = text20.ToLower();
					if (myInputField.text.Length <= 0 || text20.Contains(searchStringA))
					{
						Item_Hardware_Forschung component3 = Object.Instantiate(uiPrefabs[5], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Hardware_Forschung>();
						component3.myID = l;
						component3.mS_ = mS_;
						component3.tS_ = tS_;
						component3.sfx_ = sfx_;
						component3.guiMain_ = guiMain_;
						component3.hardware_ = hardware_;
					}
				}
			}
			break;
		}
		case 5:
			if (!fS_.IsErforscht(0) && !fS_.BereitsInAnderenRaumAktiv(0))
			{
				string text4 = fS_.GetName(0);
				searchStringA = searchStringA.ToLower();
				text4 = text4.ToLower();
				if (myInputField.text.Length <= 0 || text4.Contains(searchStringA))
				{
					CreateItem(0);
				}
			}
			if (!fS_.IsErforscht(1) && fS_.IsErforscht(0) && !fS_.BereitsInAnderenRaumAktiv(1))
			{
				string text5 = fS_.GetName(1);
				searchStringA = searchStringA.ToLower();
				text5 = text5.ToLower();
				if (myInputField.text.Length <= 0 || text5.Contains(searchStringA))
				{
					CreateItem(1);
				}
			}
			if (!fS_.IsErforscht(2) && fS_.IsErforscht(1) && !fS_.BereitsInAnderenRaumAktiv(2))
			{
				string text6 = fS_.GetName(2);
				searchStringA = searchStringA.ToLower();
				text6 = text6.ToLower();
				if (myInputField.text.Length <= 0 || text6.Contains(searchStringA))
				{
					CreateItem(2);
				}
			}
			if (!fS_.IsErforscht(3) && fS_.IsErforscht(2) && !fS_.BereitsInAnderenRaumAktiv(3))
			{
				string text7 = fS_.GetName(3);
				searchStringA = searchStringA.ToLower();
				text7 = text7.ToLower();
				if (myInputField.text.Length <= 0 || text7.Contains(searchStringA))
				{
					CreateItem(3);
				}
			}
			if (!fS_.IsErforscht(40) && fS_.IsErforscht(3) && !fS_.BereitsInAnderenRaumAktiv(40))
			{
				string text8 = fS_.GetName(40);
				searchStringA = searchStringA.ToLower();
				text8 = text8.ToLower();
				if (myInputField.text.Length <= 0 || text8.Contains(searchStringA))
				{
					CreateItem(40);
				}
			}
			if (!fS_.IsErforscht(35) && !fS_.BereitsInAnderenRaumAktiv(35))
			{
				string text9 = fS_.GetName(35);
				searchStringA = searchStringA.ToLower();
				text9 = text9.ToLower();
				if (myInputField.text.Length <= 0 || text9.Contains(searchStringA))
				{
					CreateItem(35);
				}
			}
			if (!fS_.IsErforscht(36) && !fS_.BereitsInAnderenRaumAktiv(36))
			{
				string text10 = fS_.GetName(36);
				searchStringA = searchStringA.ToLower();
				text10 = text10.ToLower();
				if (myInputField.text.Length <= 0 || text10.Contains(searchStringA))
				{
					CreateItem(36);
				}
			}
			if (!fS_.IsErforscht(28) && !fS_.BereitsInAnderenRaumAktiv(28))
			{
				string text11 = fS_.GetName(28);
				searchStringA = searchStringA.ToLower();
				text11 = text11.ToLower();
				if (myInputField.text.Length <= 0 || text11.Contains(searchStringA))
				{
					CreateItem(28);
				}
			}
			if (!fS_.IsErforscht(29) && !fS_.BereitsInAnderenRaumAktiv(29))
			{
				string text12 = fS_.GetName(29);
				searchStringA = searchStringA.ToLower();
				text12 = text12.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text12.Contains(searchStringA))
				{
					CreateItem(29);
				}
			}
			if (!fS_.IsErforscht(30) && !fS_.BereitsInAnderenRaumAktiv(30))
			{
				string text13 = fS_.GetName(30);
				searchStringA = searchStringA.ToLower();
				text13 = text13.ToLower();
				if (myInputField.text.Length <= 0 || text13.Contains(searchStringA))
				{
					CreateItem(30);
				}
			}
			if (!fS_.IsErforscht(31) && !fS_.BereitsInAnderenRaumAktiv(31))
			{
				string text14 = fS_.GetName(31);
				searchStringA = searchStringA.ToLower();
				text14 = text14.ToLower();
				if (myInputField.text.Length <= 0 || text14.Contains(searchStringA))
				{
					CreateItem(31);
				}
			}
			if (!fS_.IsErforscht(32) && !fS_.BereitsInAnderenRaumAktiv(32))
			{
				string text15 = fS_.GetName(32);
				searchStringA = searchStringA.ToLower();
				text15 = text15.ToLower();
				if (myInputField.text.Length <= 0 || text15.Contains(searchStringA))
				{
					CreateItem(32);
				}
			}
			if (!fS_.IsErforscht(33) && !fS_.BereitsInAnderenRaumAktiv(33))
			{
				string text16 = fS_.GetName(33);
				searchStringA = searchStringA.ToLower();
				text16 = text16.ToLower();
				if (myInputField.text.Length <= 0 || text16.Contains(searchStringA))
				{
					CreateItem(33);
				}
			}
			if (!fS_.IsErforscht(34) && !fS_.BereitsInAnderenRaumAktiv(34))
			{
				string text17 = fS_.GetName(34);
				searchStringA = searchStringA.ToLower();
				text17 = text17.ToLower();
				if (myInputField.text.Length <= 0 || text17.Contains(searchStringA))
				{
					CreateItem(34);
				}
			}
			if (!fS_.IsErforscht(38) && !fS_.BereitsInAnderenRaumAktiv(38))
			{
				string text18 = fS_.GetName(38);
				searchStringA = searchStringA.ToLower();
				text18 = text18.ToLower();
				if (myInputField.text.Length <= 0 || text18.Contains(searchStringA))
				{
					CreateItem(38);
				}
			}
			if (!fS_.IsErforscht(39) && !fS_.BereitsInAnderenRaumAktiv(39))
			{
				string text19 = fS_.GetName(39);
				searchStringA = searchStringA.ToLower();
				text19 = text19.ToLower();
				if (myInputField.text.Length <= 0 || text19.Contains(searchStringA))
				{
					CreateItem(39);
				}
			}
			break;
		case 6:
		{
			for (int j = 0; j < hardwareFeature_.hardFeat_RES_POINTS.Length; j++)
			{
				if (hardwareFeature_.hardFeat_UNLOCK[j] && !hardwareFeature_.IsErforscht(j) && !hardwareFeature_.BereitsInAnderenRaumAktiv(j))
				{
					string text2 = hardwareFeature_.GetName(j);
					searchStringA = searchStringA.ToLower();
					text2 = text2.ToLower();
					if (myInputField.text.Length <= 0 || text2.Contains(searchStringA))
					{
						Item_HardFeat_Forschung component = Object.Instantiate(uiPrefabs[7], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_HardFeat_Forschung>();
						component.myID = j;
						component.mS_ = mS_;
						component.tS_ = tS_;
						component.sfx_ = sfx_;
						component.guiMain_ = guiMain_;
						component.hardwareFeatures_ = hardwareFeature_;
					}
				}
			}
			break;
		}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	private void CreateItem(int id_)
	{
		if (!fS_.IsErforscht(id_))
		{
			Item_Sonstiges_Forschung component = Object.Instantiate(uiPrefabs[6], Vector3.zero, Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Sonstiges_Forschung>();
			component.myID = id_;
			component.mS_ = mS_;
			component.tS_ = tS_;
			component.sfx_ = sfx_;
			component.guiMain_ = guiMain_;
			component.unlock_ = unlock_;
			component.fS_ = fS_;
		}
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[7].GetComponent<InputField>().text;
			Init(roomID, forschungsTyp);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void InitDropdowns()
	{
		int value = 0;
		switch (forschungsTyp)
		{
		case 0:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_0");
			break;
		case 1:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_1");
			break;
		case 2:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_2");
			break;
		case 3:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_3");
			break;
		case 4:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_4");
			break;
		case 5:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_5");
			break;
		case 6:
			value = PlayerPrefs.GetInt("DD_Menu_Forschung_6");
			break;
		}
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(414));
		list.Add(tS_.GetText(415));
		if (forschungsTyp == 4)
		{
			list.Add(tS_.GetText(1605));
		}
		if (forschungsTyp == 4)
		{
			list.Add(tS_.GetText(4));
		}
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[6].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[6].GetComponent<Dropdown>().value;
		switch (forschungsTyp)
		{
		case 0:
			PlayerPrefs.SetInt("DD_Menu_Forschung_0", value);
			break;
		case 1:
			PlayerPrefs.SetInt("DD_Menu_Forschung_1", value);
			break;
		case 2:
			PlayerPrefs.SetInt("DD_Menu_Forschung_2", value);
			break;
		case 3:
			PlayerPrefs.SetInt("DD_Menu_Forschung_3", value);
			break;
		case 4:
			PlayerPrefs.SetInt("DD_Menu_Forschung_4", value);
			break;
		case 5:
			PlayerPrefs.SetInt("DD_Menu_Forschung_5", value);
			break;
		case 6:
			PlayerPrefs.SetInt("DD_Menu_Forschung_6", value);
			break;
		}
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_Genre_Forschung component = gameObject.GetComponent<Item_Genre_Forschung>();
			Item_Themes_Forschung component2 = gameObject.GetComponent<Item_Themes_Forschung>();
			Item_EngineFeatures_Forschung component3 = gameObject.GetComponent<Item_EngineFeatures_Forschung>();
			Item_GameplayFeatures_Forschung component4 = gameObject.GetComponent<Item_GameplayFeatures_Forschung>();
			Item_Hardware_Forschung component5 = gameObject.GetComponent<Item_Hardware_Forschung>();
			Item_Sonstiges_Forschung component6 = gameObject.GetComponent<Item_Sonstiges_Forschung>();
			Item_HardFeat_Forschung component7 = gameObject.GetComponent<Item_HardFeat_Forschung>();
			switch (value)
			{
			case 0:
				if ((bool)component)
				{
					gameObject.name = genres_.GetName(component.myID);
				}
				if ((bool)component2)
				{
					gameObject.name = tS_.GetThemes(component2.myID);
				}
				if ((bool)component3)
				{
					gameObject.name = eF_.GetName(component3.myID);
				}
				if ((bool)component4)
				{
					gameObject.name = gF_.GetName(component4.myID);
				}
				if ((bool)component5)
				{
					gameObject.name = hardware_.GetName(component5.myID);
				}
				if ((bool)component6)
				{
					gameObject.name = fS_.GetName(component6.myID);
				}
				if ((bool)component7)
				{
					gameObject.name = hardwareFeature_.GetName(component7.myID);
				}
				break;
			case 1:
				if ((bool)component)
				{
					gameObject.name = genres_.GetPrice(component.myID).ToString();
				}
				if ((bool)component2)
				{
					gameObject.name = themes_.GetPrice(component2.myID).ToString();
				}
				if ((bool)component3)
				{
					gameObject.name = eF_.GetPrice(component3.myID).ToString();
				}
				if ((bool)component4)
				{
					gameObject.name = gF_.GetPrice(component4.myID).ToString();
				}
				if ((bool)component5)
				{
					gameObject.name = hardware_.GetPrice(component5.myID).ToString();
				}
				if ((bool)component6)
				{
					gameObject.name = fS_.RES_PRICE[component6.myID].ToString();
				}
				if ((bool)component7)
				{
					gameObject.name = hardwareFeature_.GetPrice(component7.myID).ToString();
				}
				break;
			case 2:
				if ((bool)component)
				{
					gameObject.name = genres_.genres_RES_POINTS_LEFT[component.myID].ToString();
				}
				if ((bool)component2)
				{
					gameObject.name = themes_.themes_RES_POINTS_LEFT[component2.myID].ToString();
				}
				if ((bool)component3)
				{
					gameObject.name = eF_.engineFeatures_RES_POINTS_LEFT[component3.myID].ToString();
				}
				if ((bool)component4)
				{
					gameObject.name = gF_.gameplayFeatures_RES_POINTS_LEFT[component4.myID].ToString();
				}
				if ((bool)component5)
				{
					gameObject.name = hardware_.hardware_RES_POINTS_LEFT[component5.myID].ToString();
				}
				if ((bool)component6)
				{
					gameObject.name = fS_.RES_POINTS_LEFT[component6.myID].ToString();
				}
				if ((bool)component7)
				{
					gameObject.name = hardwareFeature_.hardFeat_RES_POINTS_LEFT[component7.myID].ToString();
				}
				break;
			case 3:
				if (forschungsTyp == 4)
				{
					gameObject.name = hardware_.hardware_TYP[component5.myID].ToString();
				}
				break;
			case 4:
				if (forschungsTyp == 4)
				{
					gameObject.name = hardware_.hardware_TECH[component5.myID].ToString();
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

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		taskForschungWait taskForschungWait2 = guiMain_.AddTask_ForschungWait();
		taskForschungWait2.Init(fromSavegame: false);
		taskForschungWait2.typ = forschungsTyp;
		GameObject gameObject = GameObject.Find("Room_" + roomID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskForschungWait2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}
}
