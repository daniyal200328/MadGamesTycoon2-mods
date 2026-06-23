using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleComponent : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private Menu_Dev_Konsole menu_;

	private hardware hardware_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public int typ;

	public int platformTyp;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!menu_)
		{
			menu_ = guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	public void Init(int compTyp_, int platTyp_)
	{
		typ = compTyp_;
		platformTyp = platTyp_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		FindScripts();
		InitDropdowns();
		switch (typ)
		{
		case 0:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1588);
			break;
		case 1:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1590);
			break;
		case 2:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1589);
			break;
		case 3:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1592);
			break;
		case 4:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1591);
			break;
		case 5:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1593);
			break;
		case 6:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1594);
			break;
		case 7:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1597);
			break;
		case 8:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1598);
			break;
		case 9:
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(1595);
			break;
		}
		CreateItems(compTyp_);
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
	}

	private void CreateItems(int typ_)
	{
		GameObject gameObject = null;
		for (int i = 0; i < hardware_.hardware_UNLOCK.Length; i++)
		{
			if (!hardware_.hardware_UNLOCK[i] || !(hardware_.hardware_RES_POINTS_LEFT[i] <= 0f) || hardware_.hardware_TYP[i] != typ_ || ((platformTyp != 1 || hardware_.hardware_ONLYHANDHELD[i]) && (platformTyp != 2 || hardware_.hardware_ONLYSTATIONARY[i])))
			{
				continue;
			}
			string text = hardware_.GetName(i);
			searchStringA = searchStringA.ToLower();
			text = text.ToLower();
			if (uiObjects[4].GetComponent<InputField>().text.Length > 0 && !text.Contains(searchStringA))
			{
				continue;
			}
			gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
			Item_DevKonsole_Componente component = gameObject.GetComponent<Item_DevKonsole_Componente>();
			component.myID = i;
			component.mS_ = mS_;
			component.tS_ = tS_;
			component.sfx_ = sfx_;
			component.guiMain_ = guiMain_;
			component.hardware_ = hardware_;
			if (!menu_.proScript_)
			{
				continue;
			}
			switch (typ_)
			{
			case 0:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_cpu))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_cpu] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 1:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_gfx))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_gfx] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 2:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_ram))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_ram] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 3:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_hdd))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_hdd] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 4:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_sfx))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_sfx] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 5:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_cooling))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_cooling] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 6:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_disc))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_disc] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 7:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_controller))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 8:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_case))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			case 9:
				if (hardware_.GetPerformance(i) < hardware_.GetPerformance(menu_.proScript_.component_monitor))
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				if (hardware_.hardware_TECH[i] > hardware_.hardware_TECH[menu_.proScript_.component_monitor] + 1)
				{
					gameObject.GetComponent<Button>().interactable = false;
					gameObject.GetComponent<Image>().color = Color.grey;
				}
				break;
			}
		}
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(1604));
		list.Add(tS_.GetText(6));
		list.Add(tS_.GetText(4));
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
			if ((bool)gameObject)
			{
				Item_DevKonsole_Componente component = gameObject.GetComponent<Item_DevKonsole_Componente>();
				switch (value)
				{
				case 0:
					gameObject.name = hardware_.GetName(component.myID).ToString();
					break;
				case 1:
					gameObject.name = hardware_.GetPerformance(component.myID).ToString();
					break;
				case 2:
					gameObject.name = hardware_.GetDevCosts(component.myID).ToString();
					break;
				case 3:
					gameObject.name = hardware_.hardware_TECH[component.myID].ToString();
					break;
				}
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

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[4].GetComponent<InputField>().text;
			Init(typ, platformTyp);
		}
	}
}
