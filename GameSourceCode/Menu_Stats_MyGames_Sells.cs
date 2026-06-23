using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_MyGames_Sells : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		string text = tS_.GetText(297);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount.ToString());
		uiObjects[1].GetComponent<Text>().text = text;
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_MyGames_Sells>().game_.myID == id_)
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
		FindScripts();
		List<string> list = new List<string>();
		int value = PlayerPrefs.GetInt(uiObjects[7].name);
		list.Clear();
		list.Add(tS_.GetText(1902));
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (genres_.IsErforscht(i))
			{
				list.Add(genres_.GetName(i));
			}
			else
			{
				list.Add("<color=grey>" + genres_.GetName(i) + "</color>");
			}
		}
		uiObjects[7].GetComponent<Dropdown>().ClearOptions();
		uiObjects[7].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[7].GetComponent<Dropdown>().value = value;
	}

	private void SetData()
	{
		int value = uiObjects[7].GetComponent<Dropdown>().value;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && CheckGameData(component) && (value - 1 == component.maingenre || value == 0) && !Exists(uiObjects[0], component.myID))
				{
					GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
					Item_MyGames_Sells component2 = obj.GetComponent<Item_MyGames_Sells>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.game_ = component;
					obj.name = component.sellsTotal.ToString();
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && (script_.ownerID == mS_.myID || script_.publisherID == mS_.myID))
		{
			if (uiObjects[6].GetComponent<Toggle>().isOn && script_.developerID != mS_.myID)
			{
				return false;
			}
			if (!script_.inDevelopment && !script_.schublade && script_.gameTyp != 2 && !script_.handy && !script_.arcade)
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

	public void DROPDOWN_Genres()
	{
		int value = uiObjects[7].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[7].name, value);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	public void TOGGLE_OnlyMyGames()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}
}
