using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Engine_Genre : MonoBehaviour
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
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevEngine_Genre>().myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		SetData();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[5].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(5));
		list.Add(tS_.GetText(1380));
		list.Add(tS_.GetText(1665));
		uiObjects[5].GetComponent<Dropdown>().ClearOptions();
		uiObjects[5].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[5].GetComponent<Dropdown>().value = value;
	}

	private void SetData()
	{
		for (int i = 0; i < genres_.genres_RES_POINTS.Length; i++)
		{
			if (genres_.genres_UNLOCK[i] && genres_.IsErforscht(i) && !Exists(uiObjects[0], i))
			{
				Item_DevEngine_Genre component = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_DevEngine_Genre>();
				component.myID = i;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[5].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[5].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevEngine_Genre component = gameObject.GetComponent<Item_DevEngine_Genre>();
				switch (value)
				{
				case 0:
					gameObject.name = genres_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = genres_.genres_LEVEL[component.myID].ToString();
					break;
				case 2:
					gameObject.name = genres_.GetFloatBeliebtheit(component.myID).ToString();
					break;
				case 3:
					gameObject.name = (-genres_.genres_MARKT[component.myID]).ToString();
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
}
