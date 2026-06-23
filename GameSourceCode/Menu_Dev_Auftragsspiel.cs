using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Auftragsspiel : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private games games_;

	private themes themes_;

	public roomScript rS_;

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
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_ContractAuftragsspiel>().game_.myID == id_)
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
		list.Add(tS_.GetText(600));
		list.Add(tS_.GetText(627));
		list.Add(tS_.GetText(277));
		list.Add(tS_.GetText(602));
		list.Add(tS_.GetText(327));
		list.Add(tS_.GetText(604));
		list.Add(tS_.GetText(625));
		list.Add(tS_.GetText(273));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
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
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if ((bool)component && component.auftragsspiel && !component.auftragsspiel_Inivs)
			{
				if (!component.pS_)
				{
					component.FindMyPublisher();
				}
				if ((bool)component.pS_ && !component.pS_.IsTochterfirma() && !Exists(uiObjects[0], component.myID))
				{
					Item_ContractAuftragsspiel component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_ContractAuftragsspiel>();
					component2.game_ = component;
					component2.mS_ = mS_;
					component2.tS_ = tS_;
					component2.sfx_ = sfx_;
					component2.guiMain_ = guiMain_;
					component2.genres_ = genres_;
					component2.games_ = games_;
					component2.rS_ = rS_;
					component2.themes_ = themes_;
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
				Item_ContractAuftragsspiel component = gameObject.GetComponent<Item_ContractAuftragsspiel>();
				switch (value)
				{
				case 0:
					gameObject.name = component.game_.auftragsspiel_gehalt.ToString();
					break;
				case 1:
					gameObject.name = component.game_.auftragsspiel_bonus.ToString();
					break;
				case 2:
					gameObject.name = component.game_.auftragsspiel_mindestbewertung.ToString();
					break;
				case 3:
					gameObject.name = component.game_.auftragsspiel_zeitInWochen.ToString();
					break;
				case 4:
					gameObject.name = component.game_.gameSize.ToString();
					break;
				case 5:
					gameObject.name = component.game_.publisherID.ToString();
					break;
				case 6:
					gameObject.name = component.game_.gamePlatform[0].ToString();
					break;
				case 7:
					gameObject.name = component.game_.maingenre.ToString();
					break;
				}
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[95]);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_PlatformKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[33]);
	}
}
