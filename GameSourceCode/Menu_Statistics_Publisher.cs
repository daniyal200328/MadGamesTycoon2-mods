using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Statistics_Publisher : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private Menu_DevGame mDevGame_;

	private genres genres_;

	private gameScript gS_;

	private taskGame task_;

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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void OnEnable()
	{
		InitDropdowns();
		Init();
	}

	public void Init()
	{
		FindScripts();
		SetData();
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			publisherScript component = array[i].GetComponent<publisherScript>();
			if (!component.isUnlocked || !component.publisher || component.onlyMobile || component.myID == mS_.myID)
			{
				continue;
			}
			bool flag = false;
			if (mS_.multiplayer && component.isPlayer)
			{
				player_mp player_mp2 = mS_.mpCalls_.FindPlayer(component.myID);
				if (player_mp2 != null && player_mp2.forschungSonstiges[33])
				{
					flag = true;
				}
			}
			if (!flag && (!uiObjects[1].GetComponent<Toggle>().isOn || !component.IsTochterfirma()) && (!uiObjects[7].GetComponent<Toggle>().isOn || (uiObjects[7].GetComponent<Toggle>().isOn && component.KaufangebotFree())))
			{
				string text = component.GetName();
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Item_Stats_Publisher component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Stats_Publisher>();
					component2.pS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(434));
		list.Add(tS_.GetText(435));
		list.Add(tS_.GetText(436));
		list.Add(tS_.GetText(437));
		list.Add(tS_.GetText(1745));
		list.Add(tS_.GetText(685));
		list.Add(tS_.GetText(1923));
		list.Add(tS_.GetText(1969));
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
			if (!gameObject)
			{
				continue;
			}
			Item_Stats_Publisher component = gameObject.GetComponent<Item_Stats_Publisher>();
			switch (value)
			{
			case 0:
				gameObject.name = component.pS_.GetName();
				break;
			case 1:
				gameObject.name = component.pS_.stars.ToString();
				break;
			case 2:
				gameObject.name = component.pS_.GetRelation().ToString();
				break;
			case 3:
				gameObject.name = component.pS_.share.ToString();
				break;
			case 4:
				gameObject.name = component.pS_.fanGenre.ToString();
				break;
			case 5:
				gameObject.name = component.pS_.GetAmountVertriebeneSpiele().ToString();
				break;
			case 6:
				gameObject.name = component.pS_.GetFirmenwert().ToString();
				break;
			case 7:
				if (component.pS_.IsMyTochterfirma())
				{
					gameObject.name = "1";
				}
				else
				{
					gameObject.name = "0";
				}
				break;
			case 8:
				if (component.pS_.Geschlossen())
				{
					gameObject.name = "1";
				}
				else
				{
					gameObject.name = "0";
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

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void TOGGLE_Tochterfirmen()
	{
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData();
	}

	public void TOGGLE_NotForSale()
	{
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData();
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			Init();
		}
	}
}
