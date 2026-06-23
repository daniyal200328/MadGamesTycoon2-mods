using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_MyKonsolen_Umsatz : MonoBehaviour
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
			if (parent_.transform.GetChild(i).GetComponent<Item_MyKonsolen_Umsatz>().pS_.myID == id_)
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
		int value = PlayerPrefs.GetInt(uiObjects[4].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(489));
		list.Add(tS_.GetText(1238));
		list.Add(tS_.GetText(6));
		uiObjects[4].GetComponent<Dropdown>().ClearOptions();
		uiObjects[4].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[4].GetComponent<Dropdown>().value = value;
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.ownerID == mS_.myID && component.isUnlocked && !Exists(uiObjects[0], component.myID))
				{
					Item_MyKonsolen_Umsatz component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_MyKonsolen_Umsatz>();
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.pS_ = component;
					component2.menu_ = this;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
		string text = tS_.GetText(1662);
		text = text.Replace("<NUM>", uiObjects[0].transform.childCount.ToString());
		uiObjects[1].GetComponent<Text>().text = text;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[4].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[4].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_MyKonsolen_Umsatz component = gameObject.GetComponent<Item_MyKonsolen_Umsatz>();
				switch (value)
				{
				case 0:
					gameObject.name = component.pS_.GetGesamtGewinn().ToString();
					break;
				case 1:
					gameObject.name = component.pS_.umsatzTotal.ToString();
					break;
				case 2:
					gameObject.name = component.pS_.GetEntwicklungskosten().ToString();
					break;
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
	}
}
