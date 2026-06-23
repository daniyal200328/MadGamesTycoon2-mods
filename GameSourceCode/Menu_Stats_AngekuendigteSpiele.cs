using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_AngekuendigteSpiele : MonoBehaviour
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
			if (parent_.transform.GetChild(i).GetComponent<Item_AngekuendigteSpiele>().game_.myID == id_)
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
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(274));
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(273));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		SetData();
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && CheckGameData(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_AngekuendigteSpiele component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_AngekuendigteSpiele>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.game_ = component;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
		string text = tS_.GetText(297);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount.ToString());
		uiObjects[4].GetComponent<Text>().text = text;
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.inDevelopment && script_.angekuendigt && !script_.isOnMarket)
		{
			if (!script_.devS_)
			{
				script_.FindMyDeveloper();
			}
			if ((bool)script_.devS_ && !script_.devS_.Geschlossen())
			{
				return true;
			}
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
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_AngekuendigteSpiele component = gameObject.GetComponent<Item_AngekuendigteSpiele>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.GetDeveloperName();
					break;
				case 1:
					gameObject.name = component.game_.GetNameSimple();
					break;
				case 2:
					gameObject.name = component.game_.maingenre.ToString();
					break;
				}
			}
		}
		if (value == 0 || value == 1)
		{
			mS_.SortChildrenByName(uiObjects[0]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[0]);
		}
	}
}
