using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Statistics_Tochterfirmen : MonoBehaviour
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

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Stats_Tochterfirma>().pS_.myID == id_)
			{
				return true;
			}
		}
		return false;
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
		long num = 0L;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			publisherScript component = array[i].GetComponent<publisherScript>();
			if (component.isUnlocked && component.IsMyTochterfirma())
			{
				string text = component.GetName();
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if ((uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
				{
					Item_Stats_Tochterfirma component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Stats_Tochterfirma>();
					component2.pS_ = component;
					component2.playerID = -1;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					num += component.GetVerwaltungskosten();
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
		uiObjects[7].GetComponent<Text>().text = tS_.GetText(1934) + ": <b><color=red>" + mS_.GetMoney(num, showDollar: true) + "</color></b>";
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(687));
		list.Add(tS_.GetText(685));
		list.Add(tS_.GetText(271));
		list.Add(tS_.GetText(1944));
		list.Add(tS_.GetText(1934));
		list.Add(tS_.GetText(1930));
		list.Add(tS_.GetText(698));
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
			Item_Stats_Tochterfirma component = gameObject.GetComponent<Item_Stats_Tochterfirma>();
			if ((bool)component.pS_)
			{
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
					gameObject.name = component.pS_.stars.ToString();
					break;
				case 2:
					gameObject.name = component.pS_.GetFirmenwert().ToString();
					break;
				case 3:
					gameObject.name = component.pS_.GetAmountGames().ToString();
					break;
				case 4:
					gameObject.name = component.pS_.GetEntwicklungsFortschritt().ToString();
					break;
				case 5:
					gameObject.name = component.pS_.GetVerwaltungskosten().ToString();
					break;
				case 6:
					gameObject.name = component.pS_.tf_umsatz_allTime.ToString();
					break;
				case 7:
					gameObject.name = component.pS_.GetTochterfirmaUmsatz24Monate().ToString();
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
		if (!guiMain_.uiObjects[118].activeSelf)
		{
			guiMain_.CloseMenu();
		}
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
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			Init();
		}
	}
}
