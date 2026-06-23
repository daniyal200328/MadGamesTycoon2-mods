using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Stats_ShowBestIPs : MonoBehaviour
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

	public gameScript gS_;

	private int gameAnzahl;

	private int gameAnzahlForReview;

	private int numGOTY;

	private int numHit;

	private int numTrend;

	private int numGold;

	private int numPlatin;

	private int numDiamant;

	private int gesamtReview;

	private int bestReview;

	private int badReview;

	private string bestReviewName = "";

	private string badReviewName = "";

	private float gesamtSells;

	private float gesamtDownloads;

	private float gesamtAbos;

	private float gesamtUmsatz;

	private float gesamtAusgaben;

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
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private bool Exists(GameObject parent_, int id_)
	{
		for (int i = 0; i < parent_.transform.childCount; i++)
		{
			if (parent_.transform.GetChild(i).gameObject.activeSelf && parent_.transform.GetChild(i).GetComponent<Item_MyGames_ShowIP>().game_.myID == id_)
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
		list.Add(tS_.GetText(325));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[1].GetComponent<Dropdown>().value = value;
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		for (int i = 0; i < uiObjects[0].transform.childCount; i++)
		{
			uiObjects[0].transform.GetChild(i).gameObject.SetActive(value: false);
		}
		string text = tS_.GetText(1557);
		text = text.Replace("<NAME>", gS_.GetIpName());
		uiObjects[4].GetComponent<Text>().text = text;
		guiMain_.DrawIpBekanntheit(uiObjects[6], gS_);
		uiObjects[18].GetComponent<InputField>().text = "";
		if (gS_.ipName != null && gS_.ipName.Length > 0)
		{
			uiObjects[18].GetComponent<InputField>().text = gS_.ipName;
		}
		SetData();
	}

	private void ResetDaten()
	{
		gameAnzahl = 0;
		gameAnzahlForReview = 0;
		numGOTY = 0;
		numHit = 0;
		numTrend = 0;
		numGold = 0;
		numPlatin = 0;
		numDiamant = 0;
		gesamtReview = 0;
		bestReview = -1;
		badReview = 9999;
		bestReviewName = "";
		badReviewName = "";
		gesamtSells = 0f;
		gesamtDownloads = 0f;
		gesamtAbos = 0f;
		gesamtUmsatz = 0f;
		gesamtAusgaben = 0f;
	}

	private void ShowDaten()
	{
		string text = "";
		uiObjects[19].GetComponent<Text>().text = gS_.GetOwnerName();
		uiObjects[20].GetComponent<Image>().sprite = gS_.GetOwnerLogo();
		float num = gS_.GetIpWert();
		num /= 1000000f;
		uiObjects[21].GetComponent<Text>().text = mS_.Round(num, 2) + " " + tS_.GetText(1483);
		text = tS_.GetText(297);
		text = text.Replace("<NUM>", gameAnzahl.ToString());
		uiObjects[7].GetComponent<Text>().text = text;
		uiObjects[8].GetComponent<Text>().text = numGOTY + "x";
		uiObjects[9].GetComponent<Text>().text = numHit + "x";
		uiObjects[10].GetComponent<Text>().text = numTrend + "x";
		uiObjects[11].GetComponent<Text>().text = numGold + "x";
		uiObjects[12].GetComponent<Text>().text = numPlatin + "x";
		uiObjects[13].GetComponent<Text>().text = numDiamant + "x";
		uiObjects[14].GetComponent<Text>().text = tS_.GetText(1571) + ": <color=green>" + bestReviewName + "</color>\n" + tS_.GetText(1572) + ": <color=red>" + badReviewName + "</color>\n" + tS_.GetText(1573);
		if (gameAnzahlForReview > 0)
		{
			uiObjects[15].GetComponent<Text>().text = "<color=green>" + bestReview + "%</color>\n<color=red>" + badReview + "%</color>\n" + gesamtReview / gameAnzahlForReview + "%";
		}
		else
		{
			uiObjects[15].GetComponent<Text>().text = "<color=green>--%</color>\n<color=red>--%</color>\n--%";
		}
		uiObjects[16].GetComponent<Text>().text = tS_.GetText(696) + "\n" + tS_.GetText(697) + " (" + tS_.GetText(1245) + ")\n" + tS_.GetText(702) + "\n\n" + tS_.GetText(1238) + "\n" + tS_.GetText(1575) + "\n\n<size=18>" + tS_.GetText(724) + "</size>";
		if (gesamtUmsatz - gesamtAusgaben >= 0f)
		{
			uiObjects[17].GetComponent<Text>().text = mS_.Round(gesamtSells, 2) + " " + tS_.GetText(1483) + "\n" + mS_.Round(gesamtDownloads, 2) + " " + tS_.GetText(1483) + "\n" + mS_.Round(gesamtAbos, 2) + " " + tS_.GetText(1483) + "\n\n<color=green>" + mS_.Round(gesamtUmsatz, 2) + " " + tS_.GetText(1483) + "</color>\n<color=red>" + mS_.Round(gesamtAusgaben, 2) + " " + tS_.GetText(1483) + "</color>\n\n<size=18><color=green>" + mS_.Round(gesamtUmsatz - gesamtAusgaben, 2) + " " + tS_.GetText(1483) + "</color></size>";
		}
		else
		{
			uiObjects[17].GetComponent<Text>().text = mS_.Round(gesamtSells, 2) + " " + tS_.GetText(1483) + "\n" + mS_.Round(gesamtDownloads, 2) + " " + tS_.GetText(1483) + "\n" + mS_.Round(gesamtAbos, 2) + " " + tS_.GetText(1483) + "\n\n<color=green>" + mS_.Round(gesamtUmsatz, 2) + " " + tS_.GetText(1483) + "</color>\n<color=red>" + mS_.Round(gesamtAusgaben, 2) + " " + tS_.GetText(1483) + "</color>\n\n<size=18><color=red>" + mS_.Round(gesamtUmsatz - gesamtAusgaben, 2) + " " + tS_.GetText(1483) + "</color></size>";
		}
	}

	private void SetData()
	{
		if (!gS_)
		{
			return;
		}
		ResetDaten();
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			gameScript component = array[i].GetComponent<gameScript>();
			if (!component || !CheckGameData(component) || Exists(uiObjects[0], component.myID))
			{
				continue;
			}
			Item_MyGames_ShowIP component2 = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[0].transform).GetComponent<Item_MyGames_ShowIP>();
			component2.game_ = component;
			component2.mS_ = mS_;
			component2.tS_ = tS_;
			component2.sfx_ = sfx_;
			component2.guiMain_ = guiMain_;
			component2.genres_ = genres_;
			gameAnzahl++;
			if (component.inDevelopment)
			{
				continue;
			}
			if (component.HasGold())
			{
				numGold++;
			}
			if (component.HasPlatin())
			{
				numPlatin++;
			}
			if (component.HasDiamant())
			{
				numDiamant++;
			}
			float num = 0f;
			if (component.gameTyp == 2)
			{
				num = component.sellsTotal;
				num /= 1000000f;
				gesamtDownloads += num;
			}
			else
			{
				num = component.sellsTotal;
				num /= 1000000f;
				gesamtSells += num;
			}
			if (component.gameTyp == 1)
			{
				num = component.abonnements;
				num /= 1000000f;
				gesamtAbos += num;
			}
			num = component.umsatzTotal;
			num /= 1000000f;
			gesamtUmsatz += num;
			num = component.GetGesamtAusgaben();
			num /= 1000000f;
			gesamtAusgaben += num;
			if (!component.typ_budget && !component.typ_bundle && !component.typ_bundleAddon)
			{
				gameAnzahlForReview++;
				gesamtReview += component.reviewTotal;
				if (component.goty)
				{
					numGOTY++;
				}
				if (component.trendsetter)
				{
					numTrend++;
				}
				if (component.IsHit())
				{
					numHit++;
				}
				if (bestReview < component.reviewTotal)
				{
					bestReview = component.reviewTotal;
					bestReviewName = component.GetNameWithTag();
				}
				if (badReview > component.reviewTotal)
				{
					badReview = component.reviewTotal;
					badReviewName = component.GetNameWithTag();
				}
			}
		}
		DROPDOWN_Sort();
		guiMain_.KeinEintrag(uiObjects[0], uiObjects[5]);
		ShowDaten();
	}

	public bool CheckGameData(gameScript script_)
	{
		if ((bool)script_ && script_.mainIP == gS_.myID && !script_.pubAngebot && !script_.auftragsspiel && !script_.inDevelopment)
		{
			return true;
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
			if (!gameObject)
			{
				continue;
			}
			Item_MyGames_ShowIP component = gameObject.GetComponent<Item_MyGames_ShowIP>();
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
				if (component.game_.inDevelopment)
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
				gameObject.name = component.game_.GetPlatformTypString();
				break;
			case 6:
				gameObject.name = component.game_.GetTypString();
				break;
			}
		}
		if (value == 0 || value == 5 || value == 6)
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
		if (uiObjects[18].GetComponent<InputField>().text.Length > 0)
		{
			gS_.ipName = uiObjects[18].GetComponent<InputField>().text;
		}
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}
}
