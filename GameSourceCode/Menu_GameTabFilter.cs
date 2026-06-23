using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_GameTabFilter : MonoBehaviour
{
	public GameObject gameTabContent;

	public GameObject[] uiObjects;

	public GameObject[] uiObjects2;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private bool isMenuOpen;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		SetToggles();
	}

	public void Init(bool isMenuOpen_)
	{
		isMenuOpen = isMenuOpen_;
	}

	public void InitDropdowns()
	{
		FindScripts();
		uiObjects2[2].GetComponent<Toggle>().isOn = mS_.gameTabs_invert;
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(1555));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(325));
		uiObjects2[0].GetComponent<Dropdown>().ClearOptions();
		uiObjects2[0].GetComponent<Dropdown>().AddOptions(list);
		uiObjects2[0].GetComponent<Dropdown>().value = mS_.gameTabs_sort;
	}

	private void SetToggles()
	{
		for (int i = 0; i < mS_.gameTabFilter.Length; i++)
		{
			uiObjects[i].GetComponent<Toggle>().isOn = mS_.gameTabFilter[i];
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		if (!isMenuOpen)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		for (int i = 0; i < mS_.gameTabFilter.Length; i++)
		{
			mS_.gameTabFilter[i] = uiObjects[i].GetComponent<Toggle>().isOn;
		}
		gameTabContent.GetComponent<GamesGroupContent>().timer = 10f;
		sfx_.PlaySound(3, force: true);
		BUTTON_Abbrechen();
	}

	public void DROPDOWN_Sort(bool save)
	{
		if (!guiMain_)
		{
			FindScripts();
		}
		bool gameTabs_invert = mS_.gameTabs_invert;
		if (save)
		{
			mS_.gameTabs_sort = uiObjects2[0].GetComponent<Dropdown>().value;
		}
		int childCount = guiMain_.uiObjects[81].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = guiMain_.uiObjects[81].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			gameTab component = gameObject.GetComponent<gameTab>();
			if (!component)
			{
				continue;
			}
			switch (mS_.gameTabs_sort)
			{
			case 0:
				gameObject.name = component.gS_.GetNameSimple();
				break;
			case 1:
			{
				float num = component.gS_.date_month;
				num /= 13f;
				gameObject.name = component.gS_.date_year.ToString() + num;
				if (component.gS_.inDevelopment || component.gS_.schublade)
				{
					gameObject.name = "999999";
				}
				break;
			}
			case 2:
				gameObject.name = component.gS_.reviewTotal.ToString();
				break;
			case 3:
				gameObject.name = component.gS_.GetIpBekanntheit().ToString();
				break;
			case 4:
				gameObject.name = (-component.gS_.GetPlatformTypINT()).ToString();
				break;
			case 5:
				gameObject.name = (-component.gS_.GetTypINT()).ToString();
				break;
			}
		}
		if (mS_.gameTabs_sort == 0)
		{
			if (gameTabs_invert)
			{
				mS_.SortChildrenByNameReverse(guiMain_.uiObjects[81]);
			}
			else
			{
				mS_.SortChildrenByName(guiMain_.uiObjects[81]);
			}
		}
		else if (gameTabs_invert)
		{
			mS_.SortChildrenByFloatReverse(guiMain_.uiObjects[81]);
		}
		else
		{
			mS_.SortChildrenByFloat(guiMain_.uiObjects[81]);
		}
		for (int j = 0; j < childCount; j++)
		{
			GameObject gameObject2 = guiMain_.uiObjects[81].transform.GetChild(j).gameObject;
			if ((bool)gameObject2)
			{
				konsoleTab component2 = gameObject2.GetComponent<konsoleTab>();
				if ((bool)component2)
				{
					component2.transform.SetSiblingIndex(0);
				}
			}
		}
		for (int k = 0; k < childCount; k++)
		{
			GameObject gameObject3 = guiMain_.uiObjects[81].transform.GetChild(k).gameObject;
			if ((bool)gameObject3)
			{
				gamePassTab component3 = gameObject3.GetComponent<gamePassTab>();
				if ((bool)component3)
				{
					component3.transform.SetSiblingIndex(0);
					break;
				}
			}
		}
	}

	public void TOGGLE_Invitiert()
	{
		mS_.gameTabs_invert = uiObjects2[2].GetComponent<Toggle>().isOn;
		DROPDOWN_Sort(save: false);
	}
}
