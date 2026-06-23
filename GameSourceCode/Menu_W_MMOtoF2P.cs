using UnityEngine;
using UnityEngine.UI;

public class Menu_W_MMOtoF2P : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private gameScript game_;

	private games games_;

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
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[1].GetComponent<Text>().text = mS_.GetMoney(GetPrice(), showDollar: true);
	}

	private int GetPrice()
	{
		long num = game_.costs_entwicklung / 3;
		return game_.GetGesamtDevPoints() * 500 + (int)num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		sfx_.PlaySound(3, force: true);
		if ((bool)game_)
		{
			mS_.Pay(GetPrice(), 10);
			CreateGame();
		}
		if (game_.isOnMarket)
		{
			game_.RemoveFromMarket();
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Game(game_);
				}
				if (mS_.mpCalls_.isClient)
				{
					mS_.mpCalls_.CLIENT_Send_Game(game_);
				}
			}
		}
		guiMain_.uiObjects[285].SetActive(value: false);
		base.gameObject.SetActive(value: false);
	}

	private void CreateGame()
	{
		game_.mmoTOf2p_created = true;
		gameScript component = Object.Instantiate(game_.gameObject).GetComponent<gameScript>();
		games_.InitMMOtoF2PGame(component);
		component.SetMyName(game_.GetNameSimple());
		component.gameTyp = 2;
		component.f2pConverted = true;
		component.publisherID = mS_.myID;
		component.pS_ = null;
		component.warBeiAwards = true;
		component.weeksOnMarket = 0;
		component.releaseDate = 0;
		component.vorbestellungen = 0;
		component.date_year = mS_.year;
		component.date_month = mS_.month;
		component.spielbericht_favorit = false;
		component.abonnements = game_.abonnements;
		component.sellsTotal = game_.abonnements;
		component.sellsTotalOnline = 0L;
		component.abonnementsWoche = 0L;
		component.sellsTotalStandard = 0L;
		component.sellsTotalDeluxe = 0L;
		component.sellsTotalCollectors = 0L;
		component.umsatzTotal = 0L;
		component.umsatzInApp = 0L;
		component.umsatzAbos = 0L;
		component.inAppPurchaseWeek = 0;
		component.costs_entwicklung = GetPrice();
		component.costs_mitarbeiter = 0L;
		component.costs_marketing = 0L;
		component.costs_enginegebuehren = 0L;
		component.costs_server = 0L;
		component.costs_production = 0L;
		for (int i = 0; i < component.sellsPerWeek.Length; i++)
		{
			component.sellsPerWeek[i] = 0;
		}
		component.lagerbestand[0] = 0L;
		component.lagerbestand[1] = 0L;
		component.lagerbestand[2] = 0L;
		component.gameplayFeatures_DevDone[57] = true;
		component.gameGameplayFeatures[57] = true;
		component.SetOnMarket();
		guiMain_.uiObjects[278].SetActive(value: true);
		guiMain_.uiObjects[278].GetComponent<Menu_InAppPurchases>().Init(component, closeMenu_: true);
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Game(component);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Game(component);
			}
		}
	}
}
