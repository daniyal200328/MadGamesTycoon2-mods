using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_TochterfirmaEngine : MonoBehaviour
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

	private publisherScript pS_;

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
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			GameObject gameObject = parent_.transform.GetChild(i).gameObject;
			if (gameObject.activeSelf && gameObject.GetComponent<Item_TochterfirmaEngine>().eS_.myID == id_)
			{
				return true;
			}
		}
		return false;
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
		list.Add(tS_.GetText(258));
		list.Add(tS_.GetText(260));
		list.Add(tS_.GetText(268));
		list.Add(tS_.GetText(1218));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(publisherScript script_)
	{
		pS_ = script_;
		FindScripts();
		InitDropdowns();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Engine");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				engineScript component = array[i].GetComponent<engineScript>();
				if ((bool)component && !component.archiv_engine && ((component.ownerID == mS_.myID && component.devPointsStart <= 0f) || (component.ownerID == mS_.myID && component.updating)) && !Exists(uiObjects[0], component.myID))
				{
					Item_TochterfirmaEngine component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_TochterfirmaEngine>();
					component2.eS_ = component;
					component2.eF_ = eF_;
					component2.genres_ = genres_;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.pS_ = pS_;
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
			if (!gameObject)
			{
				continue;
			}
			Item_TochterfirmaEngine component = gameObject.GetComponent<Item_TochterfirmaEngine>();
			switch (value)
			{
			case 0:
				gameObject.name = component.eS_.GetName();
				break;
			case 1:
				gameObject.name = (component.eS_.GetTechLevel() * 1000 + component.eS_.GetFeaturesAmount()).ToString();
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
				if (component.eS_.ownerID == mS_.myID)
				{
					gameObject.name = "2";
				}
				else
				{
					gameObject.name = "1";
				}
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
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_RemoveEngine()
	{
		sfx_.PlaySound(3, force: true);
		pS_.tf_engine = -1;
		guiMain_.uiObjects[393].GetComponent<Menu_Stats_TochterfirmaSettings>().UpdateData();
		base.gameObject.SetActive(value: false);
	}
}
