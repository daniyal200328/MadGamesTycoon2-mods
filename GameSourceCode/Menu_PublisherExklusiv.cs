using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_PublisherExklusiv : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private themes themes_;

	private Menu_DevGame mDevGame_;

	private genres genres_;

	private gameScript gS_;

	private taskGame task_;

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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_PublisherExklusiv>().pS_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	private void OnEnable()
	{
		InitDropdowns();
		Init();
	}

	public void Init()
	{
		FindScripts();
		if (mS_.exklusivVertrag_ID != -1)
		{
			guiMain_.uiObjects[382].SetActive(value: true);
			base.gameObject.SetActive(value: false);
		}
		else
		{
			SetData();
		}
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if (component.isUnlocked && component.publisher && !component.isPlayer && component.GetRelation() >= 100f && !component.IsTochterfirma() && !Exists(uiObjects[0], component.myID))
				{
					Item_PublisherExklusiv component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_PublisherExklusiv>();
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
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(434));
		list.Add(tS_.GetText(436));
		list.Add(tS_.GetText(1024));
		uiObjects[5].GetComponent<Dropdown>().ClearOptions();
		uiObjects[5].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[5].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[5].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[5].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_PublisherExklusiv component = gameObject.GetComponent<Item_PublisherExklusiv>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetName();
					break;
				case 1:
					gameObject.name = component.pS_.stars.ToString();
					break;
				case 2:
					gameObject.name = component.pS_.share.ToString();
					break;
				case 3:
					gameObject.name = component.pS_.GetMoneyExklusiv().ToString();
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
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void SelectPublisher(publisherScript pS_)
	{
		if ((bool)pS_)
		{
			guiMain_.uiObjects[211].SetActive(value: true);
			guiMain_.uiObjects[211].GetComponent<Menu_W_PublisherExklusiv>().Init(pS_);
		}
	}
}
