using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_PublisherAwards : MonoBehaviour
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

	private void OnEnable()
	{
		Init();
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(770));
		list.Add(tS_.GetText(768));
		list.Add(tS_.GetText(769));
		list.Add(tS_.GetText(767));
		list.Add(tS_.GetText(766));
		list.Add(tS_.GetText(279));
		list.Add(tS_.GetText(772));
		list.Add(tS_.GetText(1309));
		list.Add(tS_.GetText(1310));
		list.Add(tS_.GetText(771));
		list.Add(tS_.GetText(280));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		if (uiObjects[0].transform.childCount <= 0)
		{
			SetData();
		}
	}

	private void SetData()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			publisherScript component = array[i].GetComponent<publisherScript>();
			if ((bool)component && component.isUnlocked)
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_PublisherAwards component2 = gameObject.GetComponent<Item_PublisherAwards>();
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				component2.genres_ = genres_;
				component2.pub_ = component;
				component2.sort = value;
				switch (value)
				{
				case 0:
					gameObject.name = component.awards[4].ToString();
					break;
				case 1:
					gameObject.name = component.awards[2].ToString();
					break;
				case 2:
					gameObject.name = component.awards[3].ToString();
					break;
				case 3:
					gameObject.name = component.awards[0].ToString();
					break;
				case 4:
					gameObject.name = component.awards[1].ToString();
					break;
				case 5:
					gameObject.name = component.awards[8].ToString();
					break;
				case 6:
					gameObject.name = component.awards[7].ToString();
					break;
				case 7:
					gameObject.name = component.awards[10].ToString();
					break;
				case 8:
					gameObject.name = component.awards[11].ToString();
					break;
				case 9:
					gameObject.name = component.awards[5].ToString();
					break;
				case 10:
					gameObject.name = component.awards[9].ToString();
					break;
				}
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

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[1].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[1].name, value);
		uiObjects[0].SetActive(value: false);
		uiObjects[0].SetActive(value: true);
		SetData();
	}
}
