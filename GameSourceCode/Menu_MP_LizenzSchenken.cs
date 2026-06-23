using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_MP_LizenzSchenken : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private genres genres_;

	private mpCalls mpCalls_;

	private licences licences_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiPlayerButtons;

	public int selectedPlayer = -1;

	public int selectedLizenz = -1;

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
		if (!licences_)
		{
			licences_ = main_.GetComponent<licences>();
		}
	}

	private void OnEnable()
	{
		selectedLizenz = -1;
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
		list.Add(tS_.GetText(88));
		list.Add(tS_.GetText(302));
		list.Add(tS_.GetText(304));
		list.Add(tS_.GetText(305));
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
		if (selectedLizenz <= -1)
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_LizenzVerschenken>().myID == id_)
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
		for (int i = 0; i < licences_.licence_GEKAUFT.Length; i++)
		{
			if (licences_.licence_GEKAUFT[i] > 0 && !Exists(uiObjects[0], i))
			{
				Item_LizenzVerschenken component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_LizenzVerschenken>();
				component.myID = i;
				component.licences_ = licences_;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.menu_ = this;
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
				Item_LizenzVerschenken component = gameObject.GetComponent<Item_LizenzVerschenken>();
				switch (value)
				{
				case 0:
					gameObject.name = licences_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = licences_.GetSellPrice(component.myID).ToString();
					break;
				case 2:
					gameObject.name = licences_.licence_QUALITY[component.myID].ToString();
					break;
				case 3:
					gameObject.name = licences_.licence_TYP[component.myID].ToString();
					break;
				case 4:
					gameObject.name = licences_.licence_GEKAUFT[component.myID].ToString();
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
		if (selectedLizenz > -1 && selectedPlayer != -1)
		{
			sfx_.PlaySound(3, force: true);
			if (mpCalls_.isServer)
			{
				mpCalls_.SERVER_Send_Help(mS_.myID, mpCalls_.playersMP[selectedPlayer].playerID, 2, selectedLizenz, licences_.licence_GEKAUFT[selectedLizenz], 0);
			}
			else
			{
				mpCalls_.CLIENT_Send_Help(mpCalls_.playersMP[selectedPlayer].playerID, 2, selectedLizenz, licences_.licence_GEKAUFT[selectedLizenz], 0);
			}
			licences_.licence_GEKAUFT[selectedLizenz] = 0;
			string text = tS_.GetText(1331);
			text = text.Replace("<NAME1>", mpCalls_.GetCompanyName(mpCalls_.playersMP[selectedPlayer].playerID));
			text = text.Replace("<NAME2>", licences_.GetName(selectedLizenz));
			guiMain_.MessageBox(text, closeMenu: false);
			base.gameObject.SetActive(value: false);
		}
	}
}
