using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_PortSelect : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private themes themes_;

	private unlockScript unlock_;

	public roomScript rS_;

	private float updateTimer;

	private string searchStringA = "";

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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!unlock_)
		{
			unlock_ = main_.GetComponent<unlockScript>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevGame_Port>().game_.myID == id_)
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
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(217));
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(273));
		list.Add(tS_.GetText(275));
		list.Add(tS_.GetText(1484));
		list.Add(tS_.GetText(1555));
		list.Add(tS_.GetText(325));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[10].name);
		list.Clear();
		list.Add(tS_.GetText(2211));
		list.Add(tS_.GetText(2212));
		list.Add(tS_.GetText(2213));
		list.Add(tS_.GetText(2214));
		uiObjects[10].GetComponent<Dropdown>().ClearOptions();
		uiObjects[10].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[10].GetComponent<Dropdown>().value = value;
		value = PlayerPrefs.GetInt(uiObjects[11].name);
		list.Clear();
		list.Add(tS_.GetText(1902));
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (genres_.IsErforscht(i))
			{
				list.Add(genres_.GetName(i));
			}
			else
			{
				list.Add("<color=grey>" + genres_.GetName(i) + "</color>");
			}
		}
		uiObjects[11].GetComponent<Dropdown>().ClearOptions();
		uiObjects[11].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[11].GetComponent<Dropdown>().value = value;
	}

	public void Init(roomScript room_)
	{
		FindScripts();
		rS_ = room_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		if (!rS_)
		{
			return;
		}
		int value = uiObjects[11].GetComponent<Dropdown>().value;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if (!component || !CheckGameData(component) || (value - 1 != component.maingenre && value != 0))
			{
				continue;
			}
			bool flag = false;
			if (uiObjects[10].GetComponent<Dropdown>().value != 0)
			{
				if (uiObjects[10].GetComponent<Dropdown>().value == 1 && !component.portExist[0] && (component.handy || component.arcade))
				{
					flag = true;
				}
				if (uiObjects[10].GetComponent<Dropdown>().value == 2 && !component.portExist[1] && !component.handy)
				{
					flag = true;
				}
				if (uiObjects[10].GetComponent<Dropdown>().value == 3)
				{
					if (!component.portExist[2] && !component.arcade)
					{
						flag = true;
					}
					if (component.gameTyp == 1 || component.gameTyp == 2)
					{
						flag = false;
					}
				}
			}
			else
			{
				flag = true;
			}
			if (flag)
			{
				string nameSimple = component.GetNameSimple();
				searchStringA = searchStringA.ToLower();
				nameSimple = nameSimple.ToLower();
				if ((uiObjects[6].GetComponent<InputField>().text.Length <= 0 || nameSimple.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
				{
					Item_DevGame_Port component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevGame_Port>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.rS_ = rS_;
					component2.themes_ = themes_;
					component2.unlock_ = unlock_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.ownerID == mS_.myID && script_.portID == -1)
		{
			int num = 0;
			for (int i = 0; i < script_.portExist.Length; i++)
			{
				if (script_.portExist[i])
				{
					num++;
				}
			}
			if (!mS_.unlock_.Get(65))
			{
				num++;
			}
			if (script_.gameTyp == 1 || script_.gameTyp == 2)
			{
				num++;
			}
			if (num < 2 && !script_.typ_contractGame && !script_.typ_addon && !script_.typ_addonStandalone && !script_.typ_mmoaddon && !script_.typ_bundle && !script_.typ_budget && !script_.typ_bundleAddon && !script_.typ_goty && !script_.auftragsspiel && !script_.f2pConverted && (!script_.pubOffer || script_.ownerID == mS_.myID) && !script_.GetIpIsInArchiv())
			{
				return true;
			}
		}
		return false;
	}

	public void DROPDOWN_Genres()
	{
		int value = uiObjects[11].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[11].name, value);
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
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
			Item_DevGame_Port component = gameObject.GetComponent<Item_DevGame_Port>();
			switch (value)
			{
			case 0:
				gameObject.name = component.game_.GetNameSimple();
				break;
			case 1:
			{
				float num = component.game_.date_month;
				num /= 13f;
				gameObject.name = component.game_.date_year.ToString() + num;
				if (component.game_.inDevelopment || component.game_.schublade)
				{
					gameObject.name = "999999";
				}
				break;
			}
			case 2:
				gameObject.name = component.game_.reviewTotal.ToString();
				break;
			case 3:
				gameObject.name = component.game_.maingenre.ToString();
				break;
			case 4:
				gameObject.name = component.game_.sellsTotal.ToString();
				break;
			case 5:
				gameObject.name = (-component.game_.GetPlatformTypINT()).ToString();
				break;
			case 6:
				gameObject.name = component.game_.GetIpBekanntheit().ToString();
				break;
			case 7:
				gameObject.name = (-component.game_.GetTypINT()).ToString();
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

	public void DROPDOWN_Filter()
	{
		int value = uiObjects[10].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[10].name, value);
		Init(rS_);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[57]);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			Init(rS_);
		}
	}

	public void TOGGLE()
	{
		Init(rS_);
	}
}
