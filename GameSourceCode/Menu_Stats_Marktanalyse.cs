using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_Marktanalyse : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private engineFeatures eF_;

	private genres genres_;

	private games games_;

	private themes themes_;

	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

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
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
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

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		Init();
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[1].name);
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(1665));
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
				Init();
			}
		}
	}

	public void Init()
	{
		FindScripts();
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		for (int j = 0; j < genres_.genres_PRICE.Length; j++)
		{
			int amountGamesWithGenre_OnMarket = games_.GetAmountGamesWithGenre_OnMarket(j);
			if (amountGamesWithGenre_OnMarket > 0)
			{
				string text = genres_.GetName(j);
				searchStringA = searchStringA.ToLower();
				text = text.ToLower();
				if (uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA))
				{
					Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Stats_Marktanalyse>().Init(genres_.GetName(j), tS_.GetText(271) + ": " + amountGamesWithGenre_OnMarket, genres_.GetPic(j), genres_.GetSpriteMarkt(j), amountGamesWithGenre_OnMarket, 0);
				}
			}
		}
		for (int k = 0; k < themes_.themes_LEVEL.Length; k++)
		{
			int num = themes_.themes_MARKT[k];
			if (num > 0)
			{
				string text2 = tS_.GetThemes(k);
				searchStringA = searchStringA.ToLower();
				text2 = text2.ToLower();
				if (uiObjects[6].GetComponent<InputField>().text.Length <= 0 || text2.Contains(searchStringA))
				{
					Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_Stats_Marktanalyse>().Init(tS_.GetThemes(k), tS_.GetText(271) + ": " + num, guiMain_.uiSprites[6], themes_.GetSpriteMarkt(k), num, 1);
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
			Item_Stats_Marktanalyse component = gameObject.GetComponent<Item_Stats_Marktanalyse>();
			switch (value)
			{
			case 0:
				gameObject.name = component.typ + "_" + component.myName;
				break;
			case 1:
				if (component.typ == 0)
				{
					gameObject.name = (component.anzGames + 100000).ToString();
				}
				else
				{
					gameObject.name = component.anzGames.ToString();
				}
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

	public void BUTTON_Search()
	{
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[0].transform.childCount; i++)
			{
				uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[6].GetComponent<InputField>().text;
			Init();
		}
	}
}
