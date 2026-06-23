using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_GenreBeliebtheit : MonoBehaviour
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

	public bool closeMenu;

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
		Init();
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
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(245) + "<color=blue><b>\n" + genres_.GetName(mS_.GetFanGenreID()) + "</b></color>";
		uiObjects[15].GetComponent<Image>().sprite = genres_.GetPic(mS_.GetFanGenreID());
		string text = tS_.GetText(481);
		text = text.Replace("<TIME>", mS_.trendWeeks.ToString());
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
		for (int j = 0; j < uiObjects[0].transform.childCount; j++)
		{
			uiObjects[0].transform.GetChild(j).gameObject.SetActive(value: false);
		}
		for (int k = 0; k < genres_.genres_PRICE.Length; k++)
		{
			if (genres_.genres_UNLOCK[k])
			{
				GameObject obj = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform);
				Item_Stats_GenreBeliebtheit component = obj.GetComponent<Item_Stats_GenreBeliebtheit>();
				component.guiMain_ = guiMain_;
				component.tS_ = tS_;
				float prozent = 0f;
				if (mS_.trendGenre != k && mS_.trendAntiGenre != k)
				{
					prozent = genres_.genres_BELIEBTHEIT[k];
				}
				if (mS_.trendAntiGenre == k)
				{
					prozent = 20f;
				}
				if (mS_.trendGenre == k)
				{
					prozent = 100f;
				}
				component.Init(genres_.GetName(k), prozent, genres_.GetPic(k));
				obj.name = prozent.ToString();
			}
		}
		mS_.SortChildrenByFloat(uiObjects[0]);
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[4]);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		if (closeMenu)
		{
			closeMenu = false;
			guiMain_.CloseMenu();
		}
		base.gameObject.SetActive(value: false);
	}
}
