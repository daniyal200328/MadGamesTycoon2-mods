using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_BuyEngine : MonoBehaviour
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
				SetData(gekauft: false);
				break;
			case 1:
				SetData(gekauft: true);
				break;
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_BuyEngine>().eS_.myID == id_)
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
		TAB_BuyEngine(0);
	}

	public void InitDropdowns()
	{
		FindScripts();
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(4));
		list.Add(tS_.GetText(245));
		list.Add(tS_.GetText(160));
		list.Add(tS_.GetText(261));
		list.Add(tS_.GetText(88));
		list.Add(tS_.GetText(260));
		list.Add(tS_.GetText(268));
		list.Add(tS_.GetText(1218));
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

	private void SetData(bool gekauft)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Engine");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				engineScript component = array[i].GetComponent<engineScript>();
				if ((bool)component && component.isUnlocked && component.ownerID != mS_.myID && component.gekauft == gekauft && (component.OwnerIsNPC() || (component.EngineFromMitspieler() && component.sellEngine)) && component.myID != 0 && !Exists(uiObjects[0], component.myID))
				{
					Item_BuyEngine component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_BuyEngine>();
					component2.eS_ = component;
					component2.eF_ = eF_;
					component2.genres_ = genres_;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
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
				Item_BuyEngine component = gameObject.GetComponent<Item_BuyEngine>();
				switch (value)
				{
				case 0:
					gameObject.name = component.eS_.GetName();
					break;
				case 1:
					gameObject.name = component.eS_.GetTechLevel().ToString();
					break;
				case 2:
					gameObject.name = component.eS_.spezialgenre.ToString();
					break;
				case 3:
					gameObject.name = component.eS_.GetFeaturesAmount().ToString();
					break;
				case 4:
					gameObject.name = component.eS_.GetGamesAmount().ToString();
					break;
				case 5:
					gameObject.name = component.eS_.preis.ToString();
					break;
				case 6:
					gameObject.name = component.eS_.gewinnbeteiligung.ToString();
					break;
				case 7:
					gameObject.name = component.eS_.GetVerkaufteLizenzen().ToString();
					break;
				case 8:
					gameObject.name = component.eS_.spezialplatform.ToString();
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
		if (!guiMain_.uiObjects[56].activeSelf)
		{
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}

	public void TAB_BuyEngine(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(gekauft: false);
	}

	public void TAB_MyEngines(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(gekauft: true);
	}
}
