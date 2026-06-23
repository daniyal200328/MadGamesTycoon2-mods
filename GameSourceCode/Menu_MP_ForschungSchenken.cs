using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_ForschungSchenken : MonoBehaviour
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

	private mpCalls mpCalls_;

	private int forschungsTyp;

	public int selectedPlayer = -1;

	public int selectedForschung = -1;

	public GameObject[] uiPlayerButtons;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void OnEnable()
	{
		selectedForschung = -1;
		selectedPlayer = -1;
		uiObjects[7].GetComponent<InputField>().text = "";
		FindScripts();
		InitDropdowns();
		InitPlayerButtons();
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		if (selectedForschung == -1)
		{
			uiObjects[8].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[8].GetComponent<Button>().interactable = true;
		}
		UpdatePlayerButtons();
	}

	public void UpdatePlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				if (selectedPlayer == i)
				{
					uiPlayerButtons[i].GetComponent<Image>().color = guiMain_.colors[20];
				}
				else
				{
					uiPlayerButtons[i].GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	public void InitPlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				uiPlayerButtons[i].SetActive(value: false);
			}
		}
		for (int j = 0; j < mpCalls_.playersMP.Count; j++)
		{
			int playerID = mpCalls_.playersMP[j].playerID;
			if (playerID == mS_.myID)
			{
				if (uiPlayerButtons[j].activeSelf)
				{
					uiPlayerButtons[j].SetActive(value: false);
				}
				continue;
			}
			if (!uiPlayerButtons[j].activeSelf)
			{
				uiPlayerButtons[j].SetActive(value: true);
			}
			if (selectedPlayer == -1)
			{
				selectedPlayer = j;
			}
			uiPlayerButtons[j].transform.GetChild(1).GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mpCalls_.GetLogo(playerID));
			uiPlayerButtons[j].transform.GetChild(2).GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
		}
	}

	public void BUTTON_Player(int p)
	{
		sfx_.PlaySound(12, force: true);
		selectedPlayer = p;
	}

	public void Init(int i)
	{
		FindScripts();
		forschungsTyp = i;
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
		switch (i)
		{
		case 0:
		{
			for (int k = 0; k < genres_.genres_RES_POINTS.Length; k++)
			{
				if (genres_.genres_UNLOCK[k] && genres_.IsErforscht(k))
				{
					string text2 = genres_.GetName(k);
					text2 = text2.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text2.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component2.myID = k;
						component2.art = i;
						component2.mS_ = mS_;
						component2.tS_ = tS_;
						component2.sfx_ = sfx_;
						component2.guiMain_ = guiMain_;
						component2.genres_ = genres_;
						component2.menu_ = this;
					}
				}
			}
			break;
		}
		case 1:
		{
			for (int n = 0; n < themes_.themes_RES_POINTS_LEFT.Length; n++)
			{
				if (themes_.IsErforscht(n))
				{
					string text21 = tS_.GetThemes(n);
					text21 = text21.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text21.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component5 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component5.myID = n;
						component5.art = i;
						component5.mS_ = mS_;
						component5.tS_ = tS_;
						component5.sfx_ = sfx_;
						component5.guiMain_ = guiMain_;
						component5.themes_ = themes_;
						component5.menu_ = this;
					}
				}
			}
			break;
		}
		case 2:
		{
			for (int m = 0; m < eF_.engineFeatures_RES_POINTS.Length; m++)
			{
				if (eF_.engineFeatures_UNLOCK[m] && eF_.IsErforscht(m))
				{
					string text20 = eF_.GetName(m);
					text20 = text20.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text20.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component4 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component4.myID = m;
						component4.art = i;
						component4.mS_ = mS_;
						component4.tS_ = tS_;
						component4.sfx_ = sfx_;
						component4.guiMain_ = guiMain_;
						component4.eF_ = eF_;
						component4.menu_ = this;
					}
				}
			}
			break;
		}
		case 3:
		{
			for (int l = 0; l < gF_.gameplayFeatures_RES_POINTS.Length; l++)
			{
				if (gF_.gameplayFeatures_UNLOCK[l] && gF_.IsErforscht(l))
				{
					string text19 = gF_.GetName(l);
					text19 = text19.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text19.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component3 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component3.myID = l;
						component3.art = i;
						component3.mS_ = mS_;
						component3.tS_ = tS_;
						component3.sfx_ = sfx_;
						component3.guiMain_ = guiMain_;
						component3.gF_ = gF_;
						component3.menu_ = this;
					}
				}
			}
			break;
		}
		case 4:
		{
			for (int num = 0; num < hardware_.hardware_RES_POINTS.Length; num++)
			{
				if (hardware_.hardware_UNLOCK[num] && hardware_.IsErforscht(num))
				{
					string text22 = hardware_.GetName(num);
					text22 = text22.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text22.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component6 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component6.myID = num;
						component6.art = i;
						component6.mS_ = mS_;
						component6.tS_ = tS_;
						component6.sfx_ = sfx_;
						component6.guiMain_ = guiMain_;
						component6.hardware_ = hardware_;
						component6.menu_ = this;
					}
				}
			}
			break;
		}
		case 5:
			if (fS_.IsErforscht(0))
			{
				string text3 = fS_.GetName(0);
				text3 = text3.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text3.Contains(searchStringA.ToLower()))
				{
					CreateItem(0);
				}
			}
			if (fS_.IsErforscht(1))
			{
				string text4 = fS_.GetName(1);
				text4 = text4.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text4.Contains(searchStringA.ToLower()))
				{
					CreateItem(1);
				}
			}
			if (fS_.IsErforscht(2))
			{
				string text5 = fS_.GetName(2);
				text5 = text5.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text5.Contains(searchStringA.ToLower()))
				{
					CreateItem(2);
				}
			}
			if (fS_.IsErforscht(3))
			{
				string text6 = fS_.GetName(3);
				text6 = text6.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text6.Contains(searchStringA.ToLower()))
				{
					CreateItem(3);
				}
			}
			if (fS_.IsErforscht(40))
			{
				string text7 = fS_.GetName(40);
				text7 = text7.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text7.Contains(searchStringA.ToLower()))
				{
					CreateItem(40);
				}
			}
			if (fS_.IsErforscht(35))
			{
				string text8 = fS_.GetName(35);
				text8 = text8.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text8.Contains(searchStringA.ToLower()))
				{
					CreateItem(35);
				}
			}
			if (fS_.IsErforscht(36))
			{
				string text9 = fS_.GetName(36);
				text9 = text9.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text9.Contains(searchStringA.ToLower()))
				{
					CreateItem(36);
				}
			}
			if (fS_.IsErforscht(28))
			{
				string text10 = fS_.GetName(28);
				text10 = text10.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text10.Contains(searchStringA.ToLower()))
				{
					CreateItem(28);
				}
			}
			if (fS_.IsErforscht(29))
			{
				string text11 = fS_.GetName(29);
				text11 = text11.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text11.Contains(searchStringA.ToLower()))
				{
					CreateItem(29);
				}
			}
			if (fS_.IsErforscht(30))
			{
				string text12 = fS_.GetName(30);
				text12 = text12.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text12.Contains(searchStringA.ToLower()))
				{
					CreateItem(30);
				}
			}
			if (fS_.IsErforscht(31))
			{
				string text13 = fS_.GetName(31);
				text13 = text13.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text13.Contains(searchStringA.ToLower()))
				{
					CreateItem(31);
				}
			}
			if (fS_.IsErforscht(32))
			{
				string text14 = fS_.GetName(32);
				text14 = text14.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text14.Contains(searchStringA.ToLower()))
				{
					CreateItem(32);
				}
			}
			if (fS_.IsErforscht(33))
			{
				string text15 = fS_.GetName(33);
				text15 = text15.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text15.Contains(searchStringA.ToLower()))
				{
					CreateItem(33);
				}
			}
			if (fS_.IsErforscht(34))
			{
				string text16 = fS_.GetName(34);
				text16 = text16.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text16.Contains(searchStringA.ToLower()))
				{
					CreateItem(34);
				}
			}
			if (fS_.IsErforscht(38))
			{
				string text17 = fS_.GetName(38);
				text17 = text17.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text17.Contains(searchStringA.ToLower()))
				{
					CreateItem(38);
				}
			}
			if (fS_.IsErforscht(39))
			{
				string text18 = fS_.GetName(39);
				text18 = text18.ToLower();
				if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text18.Contains(searchStringA.ToLower()))
				{
					CreateItem(39);
				}
			}
			break;
		case 6:
		{
			for (int j = 0; j < hardwareFeature_.hardFeat_RES_POINTS.Length; j++)
			{
				if (hardwareFeature_.hardFeat_UNLOCK[j] && hardwareFeature_.IsErforscht(j))
				{
					string text = hardware_.GetName(j);
					text = text.ToLower();
					if (uiObjects[7].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA.ToLower()))
					{
						Item_ForschungSchenken component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
						component.myID = j;
						component.art = i;
						component.mS_ = mS_;
						component.tS_ = tS_;
						component.sfx_ = sfx_;
						component.guiMain_ = guiMain_;
						component.hardwareFeature_ = hardwareFeature_;
						component.menu_ = this;
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
		if (fS_.IsErforscht(id_))
		{
			Item_ForschungSchenken component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ForschungSchenken>();
			component.myID = id_;
			component.art = 5;
			component.mS_ = mS_;
			component.tS_ = tS_;
			component.sfx_ = sfx_;
			component.guiMain_ = guiMain_;
			component.fS_ = fS_;
			component.menu_ = this;
		}
	}

	public void BUTTON_Search()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		searchStringA = uiObjects[7].GetComponent<InputField>().text;
		Init(forschungsTyp);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[6].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(414));
		list.Add(tS_.GetText(415));
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[6].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[6].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[6].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_ForschungSchenken component = gameObject.GetComponent<Item_ForschungSchenken>();
			switch (value)
			{
			case 0:
				switch (component.art)
				{
				case 0:
					gameObject.name = genres_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = tS_.GetThemes(component.myID);
					break;
				case 2:
					gameObject.name = eF_.GetName(component.myID);
					break;
				case 3:
					gameObject.name = gF_.GetName(component.myID);
					break;
				case 4:
					gameObject.name = hardware_.GetName(component.myID);
					break;
				case 5:
					gameObject.name = fS_.GetName(component.myID);
					break;
				}
				break;
			case 1:
				switch (component.art)
				{
				case 0:
					gameObject.name = genres_.GetPrice(component.myID).ToString();
					break;
				case 1:
					gameObject.name = themes_.GetPrice(component.myID).ToString();
					break;
				case 2:
					gameObject.name = eF_.GetPrice(component.myID).ToString();
					break;
				case 3:
					gameObject.name = gF_.GetPrice(component.myID).ToString();
					break;
				case 4:
					gameObject.name = hardware_.GetPrice(component.myID).ToString();
					break;
				case 5:
					gameObject.name = fS_.RES_PRICE[component.myID].ToString();
					break;
				}
				break;
			case 2:
				switch (component.art)
				{
				case 0:
					gameObject.name = genres_.genres_RES_POINTS_LEFT[component.myID].ToString();
					break;
				case 1:
					gameObject.name = themes_.themes_RES_POINTS_LEFT[component.myID].ToString();
					break;
				case 2:
					gameObject.name = eF_.engineFeatures_RES_POINTS_LEFT[component.myID].ToString();
					break;
				case 3:
					gameObject.name = gF_.gameplayFeatures_RES_POINTS_LEFT[component.myID].ToString();
					break;
				case 4:
					gameObject.name = hardware_.hardware_RES_POINTS_LEFT[component.myID].ToString();
					break;
				case 5:
					gameObject.name = fS_.RES_POINTS_LEFT[component.myID].ToString();
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

	public void BUTTON_Ok()
	{
		if (selectedForschung != -1 && selectedPlayer != -1)
		{
			sfx_.PlaySound(3, force: true);
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 3, selectedForschung, forschungsTyp, 0);
			}
			else
			{
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 3, selectedForschung, forschungsTyp, 0);
			}
			string text = tS_.GetText(1338);
			text = text.Replace("<NAME1>", mpCalls_.GetCompanyName(mpCalls_.playersMP[selectedPlayer].playerID));
			switch (forschungsTyp)
			{
			case 0:
				text = text.Replace("<NAME2>", genres_.GetName(selectedForschung));
				break;
			case 1:
				text = text.Replace("<NAME2>", tS_.GetThemes(selectedForschung));
				break;
			case 2:
				text = text.Replace("<NAME2>", eF_.GetName(selectedForschung));
				break;
			case 3:
				text = text.Replace("<NAME2>", gF_.GetName(selectedForschung));
				break;
			case 4:
				text = text.Replace("<NAME2>", hardware_.GetName(selectedForschung));
				break;
			case 5:
				text = text.Replace("<NAME2>", fS_.GetName(selectedForschung));
				break;
			case 6:
				text = text.Replace("<NAME2>", hardwareFeature_.GetName(selectedForschung));
				break;
			}
			guiMain_.MessageBox(text, closeMenu: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
