using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_BuyLicence : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private licences licences_;

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
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
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
				SetData(inBesitz: false);
				break;
			case 1:
				SetData(inBesitz: true);
				break;
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_BuyLicence>().myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		TAB_LicenceBuy(0);
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(218));
		list.Add(tS_.GetText(302));
		list.Add(tS_.GetText(304));
		list.Add(tS_.GetText(305));
		list.Add(tS_.GetText(2019));
		list.Add(tS_.GetText(2020));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	private void Init(bool inBesitz)
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData(inBesitz);
	}

	private void SetData(bool inBesitz)
	{
		for (int i = 0; i < licences_.licence_ANGEBOT.Length; i++)
		{
			if (((!inBesitz && licences_.licence_ANGEBOT[i] > 0 && licences_.licence_GEKAUFT[i] == 0) || (inBesitz && licences_.licence_GEKAUFT[i] > 0)) && !Exists(uiObjects[0], i))
			{
				Item_BuyLicence component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_BuyLicence>();
				component.myID = i;
				component.licences_ = licences_;
				component.genres_ = genres_;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
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
			Item_BuyLicence component = gameObject.GetComponent<Item_BuyLicence>();
			switch (value)
			{
			case 0:
				gameObject.name = licences_.GetName(component.myID);
				break;
			case 1:
				gameObject.name = licences_.GetPrice(component.myID).ToString();
				break;
			case 2:
				gameObject.name = licences_.licence_QUALITY[component.myID].ToString();
				break;
			case 3:
				gameObject.name = licences_.licence_TYP[component.myID].ToString();
				break;
			case 4:
				if (licences_.licence_GEKAUFT[component.myID] > 0)
				{
					gameObject.name = licences_.licence_GEKAUFT[component.myID].ToString();
				}
				else
				{
					gameObject.name = licences_.licence_ANGEBOT[component.myID].ToString();
				}
				break;
			case 5:
				gameObject.name = licences_.licence_GENREGOOD[component.myID].ToString();
				break;
			case 6:
				gameObject.name = licences_.licence_GENREBAD[component.myID].ToString();
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

	public void TAB_LicenceBuy(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(inBesitz: false);
	}

	public void TAB_MyLicence(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		Init(inBesitz: true);
	}
}
