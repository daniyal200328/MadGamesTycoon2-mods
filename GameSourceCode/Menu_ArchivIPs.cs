using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ArchivIPs : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private genres genres_;

	private int TAB;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
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
		if (!mS_.multiplayer)
		{
			return;
		}
		updateTimer += Time.deltaTime;
		if (!(updateTimer < 5f))
		{
			updateTimer = 0f;
			switch (TAB)
			{
			case 0:
				SetData(archiv_: false);
				break;
			case 1:
				SetData(archiv_: true);
				break;
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_ArchivIP>().game_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		TAB_NoArchiv(0);
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(1555));
		list.Add(tS_.GetText(1846));
		list.Add(tS_.GetText(1898));
		list.Add(tS_.GetText(1553));
		list.Add(tS_.GetText(1554));
		list.Add(tS_.GetText(2036));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	private void Init(bool gekauft)
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData(gekauft);
	}

	private void SetData(bool archiv_)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component && component.archiv_ip == archiv_ && CheckGameData(component) && !Exists(uiObjects[0], component.myID))
				{
					Item_ArchivIP component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ArchivIP>();
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
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && script_.mainIP == script_.myID)
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
			Item_ArchivIP component = gameObject.GetComponent<Item_ArchivIP>();
			switch (value)
			{
			case 0:
				gameObject.name = component.game_.GetIpName();
				break;
			case 1:
			{
				float num = component.game_.date_month;
				num /= 13f;
				gameObject.name = component.game_.date_year.ToString() + num;
				if (component.game_.inDevelopment)
				{
					gameObject.name = "999999";
				}
				break;
			}
			case 2:
				gameObject.name = component.game_.ipPunkte.ToString();
				break;
			case 3:
				gameObject.name = component.game_.merchGesamtGewinn.ToString();
				break;
			case 4:
				gameObject.name = component.game_.ipTime.ToString();
				break;
			case 5:
				gameObject.name = component.game_.bufferIP_sells.ToString();
				break;
			case 6:
				gameObject.name = component.game_.bufferIP_umsatz.ToString();
				break;
			case 7:
				gameObject.name = component.game_.bufferIP_anzahlGames.ToString();
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

	public void BUTTON_All()
	{
		sfx_.PlaySound(3, force: true);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_ArchivIP component = gameObject.GetComponent<Item_ArchivIP>();
				if ((bool)component)
				{
					component.BUTTON_Click();
				}
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void TAB_NoArchiv(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(gekauft: false);
	}

	public void TAB_Archiv(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(gekauft: true);
	}

	public void TOGGLE_AuftragsspieleAusblenden()
	{
		if (TAB == 0)
		{
			Init(gekauft: false);
		}
		else
		{
			Init(gekauft: true);
		}
	}
}
