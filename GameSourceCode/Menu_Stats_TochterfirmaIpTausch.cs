using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_TochterfirmaIpTausch : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private roomDataScript rdS_;

	private genres genres_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public List<publisherScript> publisherList = new List<publisherScript>();

	public bool init;

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
		if (!rdS_)
		{
			rdS_ = main_.GetComponent<roomDataScript>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
			uiObjects[3].GetComponent<Scrollbar>().value = 5f;
		}
	}

	public void InitDropdowns(publisherScript selected_)
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[9].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(1555));
		list.Add(tS_.GetText(1553));
		list.Add(tS_.GetText(1554));
		list.Add(tS_.GetText(2036));
		uiObjects[9].GetComponent<Dropdown>().ClearOptions();
		uiObjects[9].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[9].GetComponent<Dropdown>().value = value;
		publisherList.Clear();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if ((bool)component && component.isUnlocked && component.IsMyTochterfirma())
				{
					publisherList.Add(component);
				}
			}
		}
		publisherList.Sort((publisherScript a, publisherScript b) => a.GetName().CompareTo(b.GetName()));
		publisherList.Insert(0, mS_.myPubS_);
		num = 0;
		if ((bool)selected_)
		{
			for (int num2 = 0; num2 < publisherList.Count; num2++)
			{
				if ((bool)publisherList[num2] && selected_ == publisherList[num2])
				{
					num = num2;
					break;
				}
			}
		}
		List<string> list2 = new List<string>();
		for (int num3 = 0; num3 < publisherList.Count; num3++)
		{
			list2.Add(publisherList[num3].GetName());
		}
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list2);
		uiObjects[1].GetComponent<Dropdown>().value = num;
		List<string> list3 = new List<string>();
		for (int num4 = 0; num4 < publisherList.Count; num4++)
		{
			list3.Add(publisherList[num4].GetName());
		}
		uiObjects[6].GetComponent<Dropdown>().ClearOptions();
		uiObjects[6].GetComponent<Dropdown>().AddOptions(list3);
		uiObjects[6].GetComponent<Dropdown>().value = 0;
	}

	public void Init(publisherScript script_)
	{
		init = false;
		FindScripts();
		InitDropdowns(script_);
		SetDataLeft();
		SetDataRight();
		IsSamePublisher();
		StartCoroutine(iInit());
	}

	public void SetDataLeft()
	{
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				gameScript component = array[j].GetComponent<gameScript>();
				if ((bool)component && CheckGameDataLeft(component))
				{
					Item_TochterfirmaIpTausch component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_TochterfirmaIpTausch>();
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
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[7]);
		IsSamePublisher();
	}

	public void SetDataRight()
	{
		for (int i = 0; i < uiObjects[4].transform.childCount; i++)
		{
			uiObjects[4].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				gameScript component = array[j].GetComponent<gameScript>();
				if ((bool)component && CheckGameDataRight(component))
				{
					Item_TochterfirmaIpTausch component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[4].transform).GetComponent<Item_TochterfirmaIpTausch>();
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
		guiMain_.KeinEintrag(uiObjects[4], uiObjects[8]);
		IsSamePublisher();
	}

	public bool CheckGameDataLeft(gameScript script_)
	{
		if ((bool)script_ && !script_.inDevelopment && !script_.pubAngebot && !script_.auftragsspiel && script_.IsMyIP(GetLeftPublisher()) && script_.mainIP == script_.myID)
		{
			return true;
		}
		return false;
	}

	public bool CheckGameDataRight(gameScript script_)
	{
		if ((bool)script_ && !script_.inDevelopment && !script_.pubAngebot && !script_.auftragsspiel && script_.IsMyIP(GetRightPublisher()) && script_.mainIP == script_.myID)
		{
			return true;
		}
		return false;
	}

	private void IsSamePublisher()
	{
		if (GetLeftPublisher() == GetRightPublisher())
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.GetComponent<Button>().interactable = false;
			}
			for (int j = 0; j < uiObjects[4].transform.childCount; j++)
			{
				uiObjects[4].transform.GetChild(j).gameObject.GetComponent<Button>().interactable = false;
			}
		}
		else
		{
			for (int k = 0; k < uiObjects[0].transform.childCount; k++)
			{
				uiObjects[0].transform.GetChild(k).gameObject.GetComponent<Button>().interactable = true;
			}
			for (int l = 0; l < uiObjects[4].transform.childCount; l++)
			{
				uiObjects[4].transform.GetChild(l).gameObject.GetComponent<Button>().interactable = true;
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public publisherScript GetLeftPublisher()
	{
		return publisherList[uiObjects[1].GetComponent<Dropdown>().value];
	}

	public publisherScript GetRightPublisher()
	{
		return publisherList[uiObjects[6].GetComponent<Dropdown>().value];
	}

	public void DROPDOWN_LeftPublisher()
	{
		if (init)
		{
			SetDataLeft();
		}
	}

	public void DROPDOWN_RightPublisher()
	{
		if (init)
		{
			SetDataRight();
		}
	}

	public void BUTTON_AllGamesLeft()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			Transform child = uiObjects[0].transform.GetChild(i);
			if ((bool)child)
			{
				child.GetComponent<Item_TochterfirmaIpTausch>().BUTTON_Click(allIps: true);
			}
		}
		SetDataLeft();
		SetDataRight();
	}

	public void BUTTON_AllGamesRight()
	{
		sfx_.PlaySound(3, force: true);
		for (int i = 0; i < uiObjects[4].transform.childCount; i++)
		{
			Transform child = uiObjects[4].transform.GetChild(i);
			if ((bool)child)
			{
				child.GetComponent<Item_TochterfirmaIpTausch>().BUTTON_Click(allIps: true);
			}
		}
		SetDataLeft();
		SetDataRight();
	}

	private IEnumerator iInit()
	{
		yield return new WaitForEndOfFrame();
		yield return new WaitForEndOfFrame();
		init = true;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[9].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[9].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if (!gameObject)
			{
				continue;
			}
			Item_TochterfirmaIpTausch component = gameObject.GetComponent<Item_TochterfirmaIpTausch>();
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
				gameObject.name = component.game_.bufferIP_sells.ToString();
				break;
			case 4:
				gameObject.name = component.game_.bufferIP_umsatz.ToString();
				break;
			case 5:
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
		childCount = uiObjects[4].transform.childCount;
		for (int j = 0; j < childCount; j++)
		{
			GameObject gameObject2 = uiObjects[4].transform.GetChild(j).gameObject;
			if (!gameObject2)
			{
				continue;
			}
			Item_TochterfirmaIpTausch component2 = gameObject2.GetComponent<Item_TochterfirmaIpTausch>();
			switch (value)
			{
			case 0:
				gameObject2.name = component2.game_.GetIpName();
				break;
			case 1:
			{
				float num2 = component2.game_.date_month;
				num2 /= 13f;
				gameObject2.name = component2.game_.date_year.ToString() + num2;
				if (component2.game_.inDevelopment)
				{
					gameObject2.name = "999999";
				}
				break;
			}
			case 2:
				gameObject2.name = component2.game_.ipPunkte.ToString();
				break;
			case 3:
				gameObject2.name = component2.game_.bufferIP_sells.ToString();
				break;
			case 4:
				gameObject2.name = component2.game_.bufferIP_umsatz.ToString();
				break;
			case 5:
				gameObject2.name = component2.game_.bufferIP_anzahlGames.ToString();
				break;
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[4]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[4]);
		}
	}
}
