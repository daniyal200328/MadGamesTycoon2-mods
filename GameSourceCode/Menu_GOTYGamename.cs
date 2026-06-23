using UnityEngine;
using UnityEngine.UI;

public class Menu_GOTYGamename : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private games games_;

	private gameScript game_;

	private int lastName;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init(gameScript gS_)
	{
		FindScripts();
		game_ = gS_;
		BUTTON_Name(lastName);
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Name(int i)
	{
		lastName = i;
		sfx_.PlaySound(3, force: true);
		if ((bool)game_)
		{
			switch (i)
			{
			case 0:
				uiObjects[0].GetComponent<Text>().text = game_.GetNameSimple() + " <color=orange><i>" + tS_.GetText(1359) + "</i></color>";
				break;
			case 1:
				uiObjects[0].GetComponent<Text>().text = game_.GetNameSimple() + " <color=orange><i>" + tS_.GetText(1361) + "</i></color>";
				break;
			}
			for (int j = 0; j < uiObjects[1].transform.childCount; j++)
			{
				uiObjects[1].transform.GetChild(j).GetComponent<Button>().interactable = true;
			}
			uiObjects[1].transform.GetChild(i).GetComponent<Button>().interactable = false;
		}
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if ((bool)component)
				{
					if (component.GetNameSimple() == uiObjects[0].GetComponent<Text>().text)
					{
						guiMain_.MessageBox(tS_.GetText(618), closeMenu: false);
						return;
					}
					if (component.isOnMarket && component.typ_budget && component.originalGameID == game_.myID)
					{
						guiMain_.MessageBox(tS_.GetText(1404), closeMenu: false);
						return;
					}
				}
			}
		}
		CreateGotyGame();
	}

	private void CreateGotyGame()
	{
		game_.goty_created = true;
		gameScript component = Object.Instantiate(game_.gameObject).GetComponent<gameScript>();
		games_.InitGotyGame(component);
		component.originalGameID = game_.myID;
		component.SetMyName(uiObjects[0].GetComponent<Text>().text);
		component.typ_standard = false;
		component.typ_goty = true;
		component.typ_nachfolger = false;
		component.nachfolger_created = false;
		component.typ_remaster = false;
		component.typ_budget = false;
		component.typ_addon = false;
		component.typ_addonStandalone = false;
		component.typ_bundle = false;
		component.typ_mmoaddon = false;
		component.typ_bundleAddon = false;
		component.spielbericht = false;
		component.spielbericht_favorit = false;
		component.warBeiAwards = true;
		component.weeksOnMarket = 0;
		component.releaseDate = 0;
		component.vorbestellungen = 0;
		component.date_year = mS_.year;
		component.date_month = mS_.month;
		component.sellsTotalStandard = 0L;
		component.sellsTotalDeluxe = 0L;
		component.sellsTotalCollectors = 0L;
		component.sellsTotalOnline = 0L;
		component.sellsTotal = 0L;
		component.umsatzTotal = 0L;
		component.umsatzInApp = 0L;
		component.umsatzAbos = 0L;
		component.tw_gewinnanteil = 0L;
		component.costs_entwicklung = 0L;
		component.costs_mitarbeiter = 0L;
		component.costs_marketing = 0L;
		component.costs_enginegebuehren = 0L;
		component.costs_server = 0L;
		component.costs_production = 0L;
		component.costs_updates = 0L;
		component.bestChartPosition = 0;
		component.lastChartPosition = 0;
		for (int i = 0; i < component.sellsPerWeek.Length; i++)
		{
			component.sellsPerWeek[i] = 0;
		}
		component.lagerbestand[0] = 0L;
		component.lagerbestand[1] = 0L;
		component.lagerbestand[2] = 0L;
		guiMain_.ActivateMenu(guiMain_.uiObjects[218]);
		guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(component, null, newGame: true, hideClose: false);
	}
}
