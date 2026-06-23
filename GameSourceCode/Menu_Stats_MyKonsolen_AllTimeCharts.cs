using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_MyKonsolen_AllTimeCharts : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

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
			if (parent_.transform.GetChild(i).GetComponent<Item_MyKonsolen_AllTimeCharts>().pS_.myID == id_)
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

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		SetData();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[4].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(1675));
		list.Add(tS_.GetText(1676));
		list.Add(tS_.GetText(1677));
		uiObjects[4].GetComponent<Dropdown>().ClearOptions();
		uiObjects[4].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[4].GetComponent<Dropdown>().value = value;
	}

	private void SetData()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).transform.gameObject.SetActive(value: false);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				platformScript component = array[j].GetComponent<platformScript>();
				if ((bool)component && CheckKonsoleData(component))
				{
					GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
					Item_MyKonsolen_AllTimeCharts component2 = obj.GetComponent<Item_MyKonsolen_AllTimeCharts>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.pS_ = component;
					obj.name = component.units.ToString();
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckKonsoleData(platformScript script_)
	{
		if ((bool)script_ && script_.isUnlocked && script_.units > 0 && (script_.typ == 1 || script_.typ == 2) && (uiObjects[4].GetComponent<Dropdown>().value == 0 || (uiObjects[4].GetComponent<Dropdown>().value == 1 && script_.typ == 1) || (uiObjects[4].GetComponent<Dropdown>().value == 2 && script_.typ == 2)))
		{
			return true;
		}
		return false;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[4].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[4].name, value);
		SetData();
	}
}
