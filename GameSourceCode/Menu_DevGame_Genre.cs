using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_DevGame_Genre : MonoBehaviour
{
	public int genreArt;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private genres genres_;

	private Menu_DevGame mDevGame_;

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
		if (!mDevGame_)
		{
			mDevGame_ = guiMain_.uiObjects[56].GetComponent<Menu_DevGame>();
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
				SetData(genreArt);
			}
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_DevGame_Genre>().myID == id_)
			{
				return true;
			}
		}
		return false;
	}

	public void Init(int g)
	{
		FindScripts();
		genreArt = g;
		InitDropdowns();
		SetData(genreArt);
	}

	private void SetData(int g)
	{
		if (g == 0)
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(343);
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = tS_.GetText(344);
		}
		uiObjects[6].GetComponent<Button>().interactable = true;
		if (g == 0)
		{
			if (mDevGame_.g_GameMainGenre == -1)
			{
				uiObjects[6].GetComponent<Button>().interactable = false;
			}
		}
		else if (mDevGame_.g_GameSubGenre == -1)
		{
			uiObjects[6].GetComponent<Button>().interactable = false;
		}
		string text = tS_.GetText(812);
		text = text.Replace("<TEXT>", "<color=yellow>" + genres_.GetName(mS_.GetFanGenreID()) + "</color>");
		uiObjects[5].GetComponent<Text>().text = text;
		text = tS_.GetText(1911);
		text = text.Replace("<NAME>", "<color=blue>" + genres_.GetName(mS_.GetFanGenreID()) + "</color>");
		uiObjects[8].GetComponent<tooltip>().c = text;
		for (int i = 0; i < 5; i++)
		{
			if (mS_.lastGamesGenre[i] >= 0)
			{
				uiObjects[9 + i].transform.GetChild(0).GetComponent<Image>().sprite = genres_.GetPic(mS_.lastGamesGenre[i]);
				uiObjects[9 + i].GetComponent<tooltip>().c = genres_.GetName(mS_.lastGamesGenre[i]);
			}
			else
			{
				uiObjects[9 + i].transform.GetChild(0).GetComponent<Image>().sprite = guiMain_.uiSprites[19];
				uiObjects[9 + i].GetComponent<tooltip>().c = "";
			}
		}
		for (int j = 0; j < genres_.genres_RES_POINTS.Length; j++)
		{
			if (genres_.genres_UNLOCK[j] && genres_.IsErforscht(j) && !Exists(uiObjects[0], j))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_DevGame_Genre component = gameObject.GetComponent<Item_DevGame_Genre>();
				component.myID = j;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.genres_ = genres_;
				component.genreArt = g;
				if (j == mDevGame_.g_GameSubGenre || j == mDevGame_.g_GameMainGenre)
				{
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_GenreEntfernen()
	{
		if (genreArt == 0)
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetMainGenre(-1);
		}
		else
		{
			guiMain_.uiObjects[56].GetComponent<Menu_DevGame>().SetSubGenre(-1);
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_GenreBeliebtheit()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[280].SetActive(value: true);
	}

	public void BUTTON_Marktanalyse()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.uiObjects[136].SetActive(value: true);
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[7].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(355));
		list.Add(tS_.GetText(5));
		list.Add(tS_.GetText(1380));
		list.Add(tS_.GetText(1665));
		uiObjects[7].GetComponent<Dropdown>().ClearOptions();
		uiObjects[7].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[7].GetComponent<Dropdown>().value = value;
	}

	public void DROPDOWN_Sort()
	{
		int value = uiObjects[7].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[7].name, value);
		int childCount = uiObjects[0].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[0].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevGame_Genre component = gameObject.GetComponent<Item_DevGame_Genre>();
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
}
