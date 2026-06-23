using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Statistics_BestGames : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private games games_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiPool;

	private float updateTimer;

	private int tabButton_gameSize = 6;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
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
				SetData(check: true);
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			GameObject gameObject = parent_.transform.GetChild(i).gameObject;
			if (gameObject.activeSelf)
			{
				Item_BestGames component = gameObject.GetComponent<Item_BestGames>();
				if (component.game_.myID == id_)
				{
					gameObject.name = component.game_.reviewTotal.ToString();
					return true;
				}
			}
		}
		return false;
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(1290));
		list.Add(tS_.GetText(1289));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[6].name);
		list.Clear();
		list.Add(tS_.GetText(1902));
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (genres_.genres_UNLOCK[i])
			{
				list.Add("<color=black>" + genres_.GetName(i) + "</color>");
			}
			else
			{
				list.Add("<color=grey>" + genres_.GetName(i) + "</color>");
			}
		}
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[6].GetComponent<Dropdown>().value = value;
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		if (uiObjects[0].transform.childCount <= 0)
		{
			SetData(check: false);
		}
	}

	private void SetData(bool check)
	{
		int genre = uiObjects[6].GetComponent<Dropdown>().value - 1;
		games_.CreateBestGamesCharts(500, genre, tabButton_gameSize);
		for (int i = 0; i < games_.chartsList.Count; i++)
		{
			gameScript script_ = games_.chartsList[i].script_;
			if (!script_)
			{
				continue;
			}
			if (check)
			{
				if (!Exists(uiObjects[0], script_.myID))
				{
					Item_BestGames component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_BestGames>();
					component.mS_ = mS_;
					component.tS_ = tS_;
					component.sfx_ = sfx_;
					component.guiMain_ = guiMain_;
					component.genres_ = genres_;
					component.game_ = script_;
				}
			}
			else
			{
				Item_BestGames component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_BestGames>();
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.genres_ = genres_;
				component2.game_ = script_;
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	private GameObject GetObjectFromPool(GameObject pool_, GameObject newParent_)
	{
		if ((bool)pool_ && pool_.transform.childCount > 0)
		{
			Transform child = pool_.transform.GetChild(0);
			if ((bool)child)
			{
				child.transform.SetParent(newParent_.transform);
				return child.gameObject;
			}
		}
		return null;
	}

	private void Unpool(GameObject pool_, GameObject content_)
	{
		Item_BestGames[] componentsInChildren = content_.transform.GetComponentsInChildren<Item_BestGames>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].transform.SetParent(pool_.transform);
		}
	}

	public void BUTTON_Close()
	{
		Unpool(uiPool[0], uiObjects[0]);
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
				Item_BestGames component = gameObject.GetComponent<Item_BestGames>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.reviewTotal.ToString();
					break;
				case 1:
					gameObject.name = component.game_.GetUserReviewPercent().ToString();
					break;
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
	}

	public void DROPDOWN_Genre()
	{
		int value = uiObjects[6].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[6].name, value);
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData(check: false);
	}

	public void TABBUTTON_GameSize(int i)
	{
		tabButton_gameSize = i;
		for (int j = 0; j < uiObjects[7].transform.childCount; j++)
		{
			Transform child = uiObjects[7].transform.GetChild(j);
			if ((bool)child)
			{
				if (tabButton_gameSize == j)
				{
					child.GetComponent<Image>().color = guiMain_.colors[20];
				}
				else
				{
					child.GetComponent<Image>().color = Color.white;
				}
			}
		}
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData(check: false);
	}
}
