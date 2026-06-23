using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_AddonBundleSelect : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private Menu_AddonBundle menuAddonBundle_;

	public int slot;

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
		if (!menuAddonBundle_)
		{
			menuAddonBundle_ = guiMain_.uiObjects[271].GetComponent<Menu_AddonBundle>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_AddonBundleSelect>().game_.myID == id_)
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
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(int slot_)
	{
		FindScripts();
		slot = slot_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		SetData();
	}

	private void SetData()
	{
		bool isOn = uiObjects[4].GetComponent<Toggle>().isOn;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if ((bool)component && (!isOn || (isOn && !component.GetIpIsInArchiv())) && CheckGameData(component))
			{
				string nameSimple = component.GetNameSimple();
				searchStringA = searchStringA.ToLower();
				nameSimple = nameSimple.ToLower();
				if ((uiObjects[6].GetComponent<InputField>().text.Length <= 0 || nameSimple.Contains(searchStringA)) && !Exists(uiObjects[0], component.myID))
				{
					Item_AddonBundleSelect component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_AddonBundleSelect>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.menu_ = menuAddonBundle_;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_)
		{
			if (slot == 0)
			{
				if (script_.ownerID == mS_.myID && !script_.inDevelopment && !script_.isOnMarket && script_.gameTyp == 0 && !script_.typ_budget && script_.amountAddons > 0 && !script_.pubOffer && !script_.schublade && (script_.typ_standard || script_.typ_nachfolger || script_.typ_spinoff || script_.typ_remaster) && !script_.handy && !script_.arcade && HasUnusedAddons(script_.myID) && !script_.freeware)
				{
					return true;
				}
			}
			else if (script_.ownerID == mS_.myID && script_.developerID == mS_.myID && !script_.inDevelopment && !script_.isOnMarket && script_.gameTyp == 0 && !script_.bundle_created && (script_.typ_addon || script_.typ_addonStandalone) && script_.originalGameID == menuAddonBundle_.games[0].myID && menuAddonBundle_.games[0] != script_ && menuAddonBundle_.games[1] != script_ && menuAddonBundle_.games[2] != script_ && menuAddonBundle_.games[3] != script_ && menuAddonBundle_.games[4] != script_ && !script_.handy && !script_.arcade && !script_.freeware)
			{
				return true;
			}
		}
		return false;
	}

	private bool HasUnusedAddons(int gameID)
	{
		for (int i = 0; i < mS_.games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = mS_.games_.arrayGamesScripts[i];
			if (gameScript2.ownerID == mS_.myID && gameScript2.developerID == mS_.myID && !gameScript2.inDevelopment && !gameScript2.isOnMarket && !gameScript2.bundle_created && (gameScript2.typ_addon || gameScript2.typ_addonStandalone) && gameScript2.originalGameID == gameID)
			{
				return true;
			}
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
			if ((bool)gameObject)
			{
				Item_AddonBundleSelect component = gameObject.GetComponent<Item_AddonBundleSelect>();
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
			Init(slot);
		}
	}

	public void TOGGLE_Archiv()
	{
		Init(slot);
	}
}
