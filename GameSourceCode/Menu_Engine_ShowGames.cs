using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Engine_ShowGames : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineScript eS_;

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
			if (parent_.transform.GetChild(i).GetComponent<Item_GamesList>().game_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	private void OnEnable()
	{
		InitDropdowns();
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(274));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(275));
		list.Add(tS_.GetText(276));
		list.Add(tS_.GetText(277));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(engineScript s_)
	{
		FindScripts();
		eS_ = s_;
		SetData();
	}

	private void SetData()
	{
		if (!eS_)
		{
			return;
		}
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(eS_.GetGamesAmount(), showDollar: false);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && component.engineID == eS_.myID && !component.inDevelopment && !Exists(uiObjects[0], component.myID))
				{
					Item_GamesList component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_GamesList>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.game_ = component;
					component2.genres_ = genres_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
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
				Item_GamesList component = gameObject.GetComponent<Item_GamesList>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.GetNameSimple();
					component.SetData(component.game_.GetDeveloperName());
					break;
				case 1:
					gameObject.name = component.game_.maingenre.ToString();
					component.SetData(component.game_.GetGenreString());
					break;
				case 2:
					gameObject.name = component.game_.developerID.ToString();
					component.SetData(component.game_.GetDeveloperName());
					break;
				case 3:
				{
					float num = component.game_.date_month;
					num /= 13f;
					gameObject.name = component.game_.date_year.ToString() + num;
					component.SetData(component.game_.GetReleaseDateString());
					break;
				}
				case 4:
					gameObject.name = component.game_.sellsTotal.ToString();
					component.SetData(mS_.GetMoney(component.game_.sellsTotal, showDollar: false));
					break;
				case 5:
					gameObject.name = component.game_.umsatzTotal.ToString();
					component.SetData(mS_.GetMoney(component.game_.umsatzTotal, showDollar: true));
					break;
				case 6:
					gameObject.name = component.game_.reviewTotal.ToString();
					component.SetData(component.game_.reviewTotal + "%");
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
}
