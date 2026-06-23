using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Charts_BestGamesUmsatz : MonoBehaviour
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
				Item_AllTimeCharts component = gameObject.GetComponent<Item_AllTimeCharts>();
				if (component.game_.myID == id_)
				{
					gameObject.name = component.game_.sellsTotal.ToString();
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
		List<string> list = new List<string>();
		int value = PlayerPrefs.GetInt(uiObjects[6].name);
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
		games_.CreateAllTimeChartsUmsatz(500, genre, tabButton_gameSize);
		for (int i = 0; i < games_.chartsList.Count; i++)
		{
			gameScript script_ = games_.chartsList[i].script_;
			if ((bool)script_ && !Exists(uiObjects[0], script_.myID))
			{
				GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_AllTimeCharts component = obj.GetComponent<Item_AllTimeCharts>();
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
				component.game_ = script_;
				obj.name = script_.umsatzTotal.ToString();
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
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
