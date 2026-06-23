using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_KonsoleVorgaenger : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private platforms platforms_;

	private int platformTyp;

	private float updateTimer;

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
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 5f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevKonsole_Vorgaenger>().pS_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(1612));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(int platTyp_)
	{
		platformTyp = platTyp_;
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && CheckPlatformData(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_DevKonsole_Vorgaenger component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevKonsole_Vorgaenger>();
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
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckPlatformData(platformScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && script_.isUnlocked && script_.typ == platformTyp && script_.nachfolgerID == -1)
		{
			return true;
		}
		return false;
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
			Item_DevKonsole_Vorgaenger component = gameObject.GetComponent<Item_DevKonsole_Vorgaenger>();
			switch (value)
			{
			case 0:
				gameObject.name = component.pS_.GetName();
				break;
			case 1:
			{
				float num = component.pS_.date_month;
				num /= 13f;
				gameObject.name = component.pS_.date_year.ToString() + num;
				if (!component.pS_.isUnlocked)
				{
					gameObject.name = "999999";
				}
				break;
			}
			case 2:
				gameObject.name = component.pS_.tech.ToString();
				break;
			case 3:
				gameObject.name = component.pS_.performancePoints.ToString();
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

	public void BUTTON_RemoveVorgaenger()
	{
		guiMain_.uiObjects[318].GetComponent<Menu_Dev_Konsole>().SetVorgaenger(-1);
		BUTTON_Close();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
