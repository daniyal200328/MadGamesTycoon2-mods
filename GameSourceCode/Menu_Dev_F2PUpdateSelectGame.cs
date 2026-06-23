using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_F2PUpdateSelectGame : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	public roomScript rS_;

	private float updateTimer;

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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevGame_f2PUpdate>().game_.myID == id_)
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
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(697));
		list.Add(tS_.GetText(1392));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[7].name);
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

	public void Init(roomScript room_)
	{
		FindScripts();
		rS_ = room_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		if (!rS_)
		{
			return;
		}
		int value = uiObjects[7].GetComponent<Dropdown>().value;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if ((bool)component && CheckGameData(component) && (value - 1 == component.maingenre || value == 0))
			{
				string nameSimple = component.GetNameSimple();
				searchStringA = searchStringA.ToLower();
				nameSimple = nameSimple.ToLower();
				if ((uiObjects[6].GetComponent<InputField>().text.Length <= 0 || nameSimple.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
				{
					Item_DevGame_f2PUpdate component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_f2PUpdate>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.rS_ = rS_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && script_.developerID == mS_.myID && !script_.inDevelopment && script_.isOnMarket && script_.gameTyp == 2 && !script_.pubOffer && (script_.typ_standard || script_.typ_nachfolger || script_.typ_spinoff) && !script_.GetIpIsInArchiv())
		{
			return true;
		}
		return false;
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
				Item_DevGame_f2PUpdate component = gameObject.GetComponent<Item_DevGame_f2PUpdate>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.GetNameSimple();
					break;
				case 1:
				{
					float num = component.game_.date_month;
					num /= 13f;
					gameObject.name = component.game_.date_year.ToString() + num;
					break;
				}
				case 2:
					gameObject.name = component.game_.reviewTotal.ToString();
					break;
				case 3:
					gameObject.name = component.game_.maingenre.ToString();
					break;
				case 4:
					gameObject.name = component.game_.sellsTotalOnline.ToString();
					break;
				case 5:
					gameObject.name = component.game_.abonnements.ToString();
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[104]);
		guiMain_.uiObjects[104].GetComponent<Menu_Dev_Addon>().Init(rS_);
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
			Init(rS_);
		}
	}
}
