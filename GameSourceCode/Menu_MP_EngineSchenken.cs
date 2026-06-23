using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_EngineSchenken : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private genres genres_;

	private mpCalls mpCalls_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiPlayerButtons;

	public int selectedPlayer = -1;

	public engineScript selectedEngine;

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
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void OnEnable()
	{
		selectedEngine = null;
		selectedPlayer = -1;
		InitDropdowns();
		Init();
		InitPlayerButtons();
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
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
		if (!selectedEngine)
		{
			uiObjects[5].GetComponent<Button>().interactable = false;
		}
		else
		{
			uiObjects[5].GetComponent<Button>().interactable = true;
		}
		UpdatePlayerButtons();
		MultiplayerUpdate();
	}

	public void UpdatePlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				if (selectedPlayer == i)
				{
					uiPlayerButtons[i].GetComponent<Image>().color = guiMain_.colors[20];
				}
				else
				{
					uiPlayerButtons[i].GetComponent<Image>().color = Color.white;
				}
			}
		}
	}

	public void InitPlayerButtons()
	{
		for (int i = 0; i < 4; i++)
		{
			if (uiPlayerButtons[i].activeSelf)
			{
				uiPlayerButtons[i].SetActive(value: false);
			}
		}
		for (int j = 0; j < mpCalls_.playersMP.Count; j++)
		{
			int playerID = mpCalls_.playersMP[j].playerID;
			if (playerID == mS_.myID)
			{
				if (uiPlayerButtons[j].activeSelf)
				{
					uiPlayerButtons[j].SetActive(value: false);
				}
				continue;
			}
			if (!uiPlayerButtons[j].activeSelf)
			{
				uiPlayerButtons[j].SetActive(value: true);
			}
			if (selectedPlayer == -1)
			{
				selectedPlayer = j;
			}
			uiPlayerButtons[j].transform.GetChild(1).GetComponent<Image>().sprite = guiMain_.GetCompanyLogo(mpCalls_.GetLogo(playerID));
			uiPlayerButtons[j].transform.GetChild(2).GetComponent<Text>().text = mpCalls_.GetCompanyName(playerID);
		}
	}

	public void BUTTON_Player(int p)
	{
		sfx_.PlaySound(12, force: true);
		selectedPlayer = p;
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_EngineSchenken>().eS_.myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void Init()
	{
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
				if ((bool)component && component.myID != 0 && component.ownerID == mS_.myID && component.Complete() && !Exists(uiObjects[0], component.myID))
				{
					Item_EngineSchenken component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_EngineSchenken>();
					component2.eS_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.eF_ = eF_;
					component2.genres_ = genres_;
					component2.menu_ = this;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
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
				Item_EngineSchenken component = gameObject.GetComponent<Item_EngineSchenken>();
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

	public void BUTTON_Ok()
	{
		if ((bool)selectedEngine && selectedPlayer != -1)
		{
			sfx_.PlaySound(3, force: true);
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 1, selectedEngine.myID, 0, 0);
			}
			else
			{
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 1, selectedEngine.myID, 0, 0);
			}
			string text = tS_.GetText(1329);
			text = text.Replace("<NAME1>", mpCalls_.GetCompanyName(mpCalls_.playersMP[selectedPlayer].playerID));
			text = text.Replace("<NAME2>", selectedEngine.GetName());
			guiMain_.MessageBox(text, closeMenu: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
