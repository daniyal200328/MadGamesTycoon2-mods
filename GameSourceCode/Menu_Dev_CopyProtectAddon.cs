using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_CopyProtectAddon : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private Menu_Dev_AddonDo devAddon_;

	private Menu_Dev_MMOAddon devMMOAddon_;

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
		if (!devAddon_)
		{
			devAddon_ = guiMain_.uiObjects[193].GetComponent<Menu_Dev_AddonDo>();
		}
		if (!devMMOAddon_)
		{
			devMMOAddon_ = guiMain_.uiObjects[247].GetComponent<Menu_Dev_MMOAddon>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_Dev_CopyProtectAddon>().cpS_.myID == id_)
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
		Init();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(218));
		list.Add(tS_.GetText(286));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	private void Init()
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		if (guiMain_.uiObjects[193].activeSelf)
		{
			if (devAddon_.g_GameCopyProtect != -1)
			{
				uiObjects[4].GetComponent<Button>().interactable = true;
			}
			else
			{
				uiObjects[4].GetComponent<Button>().interactable = false;
			}
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			if (devMMOAddon_.g_GameCopyProtect != -1)
			{
				uiObjects[4].GetComponent<Button>().interactable = true;
			}
			else
			{
				uiObjects[4].GetComponent<Button>().interactable = false;
			}
		}
		SetData();
	}

	private void SetData()
	{
		bool isOn = uiObjects[5].GetComponent<Toggle>().isOn;
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			copyProtectScript component = array[i].GetComponent<copyProtectScript>();
			if ((bool)component && component.isUnlocked && component.inBesitz && (component.effekt > 0f || !isOn) && !Exists(uiObjects[0], component.myID))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_Dev_CopyProtectAddon component2 = gameObject.GetComponent<Item_Dev_CopyProtectAddon>();
				component2.cpS_ = component;
				component2.mS_ = mS_;
				component2.tS_ = tS_;
				component2.sfx_ = sfx_;
				component2.guiMain_ = guiMain_;
				if (guiMain_.uiObjects[193].activeSelf && devAddon_.g_GameCopyProtect == component.myID)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
				if (guiMain_.uiObjects[247].activeSelf && devMMOAddon_.g_GameCopyProtect == component.myID)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[6]);
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
				Item_Dev_CopyProtectAddon component = gameObject.GetComponent<Item_Dev_CopyProtectAddon>();
				switch (value)
				{
				case 0:
					gameObject.name = component.cpS_.GetName();
					break;
				case 1:
					gameObject.name = component.cpS_.GetPrice().ToString();
					break;
				case 2:
					gameObject.name = component.cpS_.effekt.ToString();
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

	public void TOGGLE_Veraltet()
	{
		Init();
	}

	public void BUTTON_CopyProtectEntfernen()
	{
		if (guiMain_.uiObjects[193].activeSelf)
		{
			devAddon_.SetCopyProtect(-1);
		}
		if (guiMain_.uiObjects[247].activeSelf)
		{
			devMMOAddon_.SetCopyProtect(-1);
		}
		BUTTON_Close();
	}
}
