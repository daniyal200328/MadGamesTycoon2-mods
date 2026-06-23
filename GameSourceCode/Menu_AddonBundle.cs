using UnityEngine;
using UnityEngine.UI;

public class Menu_AddonBundle : MonoBehaviour
{
	public GameObject[] uiObjects;

	private roomScript rS_;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private games games_;

	public gameScript[] games;

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
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
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

	private void OnEnable()
	{
		Init();
	}

	public void Init()
	{
		FindScripts();
		uiObjects[0].GetComponent<InputField>().text = "";
		for (int i = 0; i < games.Length; i++)
		{
			games[i] = null;
		}
		for (int j = 0; j < games.Length; j++)
		{
			SetGame(j, games[j]);
		}
	}

	public void SetGame(int slot, gameScript script_)
	{
		if (slot == 0)
		{
			BUTTON_Remove(1);
			BUTTON_Remove(2);
			BUTTON_Remove(3);
			BUTTON_Remove(4);
			if ((bool)script_)
			{
				string nameSimple = script_.GetNameSimple();
				nameSimple = nameSimple.Replace("<color=green>[P]</color>", string.Empty);
				nameSimple = nameSimple.Replace("<color=green>", string.Empty);
				nameSimple = nameSimple.Replace("[P]", string.Empty);
				nameSimple = nameSimple.Replace("</color>", string.Empty);
				nameSimple = nameSimple.Replace("\n", string.Empty);
				nameSimple = nameSimple.Replace("\r", string.Empty);
				nameSimple = nameSimple.Replace("\t", string.Empty);
				uiObjects[0].GetComponent<InputField>().text = nameSimple + " - " + tS_.GetText(1358);
			}
		}
		games[slot] = script_;
		if (!script_)
		{
			uiObjects[22 + slot].GetComponent<tooltip>().c = tS_.GetText(1344);
			uiObjects[2 + slot].GetComponent<Text>().text = tS_.GetText(1345);
			uiObjects[7 + slot].GetComponent<Text>().text = tS_.GetText(1344);
			uiObjects[12 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[27 + slot].GetComponent<Text>().text = "";
		}
		else
		{
			uiObjects[22 + slot].GetComponent<tooltip>().c = script_.GetTooltip();
			uiObjects[2 + slot].GetComponent<Text>().text = "<b>" + script_.GetNameWithTag() + "</b>";
			uiObjects[7 + slot].GetComponent<Text>().text = script_.GetReleaseDateString();
			uiObjects[12 + slot].GetComponent<Image>().sprite = script_.GetTypSprite();
			uiObjects[27 + slot].GetComponent<Text>().text = Mathf.RoundToInt(script_.reviewTotal) + "%";
		}
		guiMain_.DrawStarsColor(uiObjects[1], Mathf.RoundToInt(GetQuality()), Color.white);
	}

	public float GetQuality()
	{
		float num = 0f;
		for (int i = 0; i < games.Length; i++)
		{
			if ((bool)games[i])
			{
				num += (float)games[i].reviewTotal;
			}
		}
		if (num > 0f)
		{
			num /= 4f;
		}
		if (num > 100f)
		{
			num = 100f;
		}
		return num / 20f;
	}

	public float GetTotalReview()
	{
		float num = 0f;
		for (int i = 0; i < games.Length; i++)
		{
			if ((bool)games[i])
			{
				num += (float)games[i].reviewTotal;
			}
		}
		if (num > 0f)
		{
			num /= 4f;
		}
		if (num > 95f)
		{
			num = 95f;
		}
		return num;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Game(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (i != 0 && !games[0])
		{
			guiMain_.MessageBox(tS_.GetText(1355), closeMenu: false);
			return;
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[272]);
		guiMain_.uiObjects[272].GetComponent<Menu_AddonBundleSelect>().Init(i);
	}

	public void BUTTON_Remove(int i)
	{
		sfx_.PlaySound(3, force: true);
		SetGame(i, null);
	}

	public void BUTTON_OK()
	{
		int num = 0;
		for (int i = 0; i < games.Length; i++)
		{
			if ((bool)games[i])
			{
				num++;
			}
		}
		if (num <= 1)
		{
			guiMain_.MessageBox(tS_.GetText(1357), closeMenu: false);
			return;
		}
		if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1346), closeMenu: false);
			return;
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int j = 0; j < array.Length; j++)
			{
				gameScript component = array[j].GetComponent<gameScript>();
				if ((bool)component && component.GetNameSimple() == uiObjects[0].GetComponent<InputField>().text)
				{
					guiMain_.MessageBox(tS_.GetText(618), closeMenu: false);
					return;
				}
			}
		}
		CreateAddonBundleGame();
	}

	private void CreateAddonBundleGame()
	{
		for (int i = 1; i < games.Length; i++)
		{
			if ((bool)games[i])
			{
				games[i].bundle_created = true;
			}
		}
		gameScript component = Object.Instantiate(games[0].gameObject).GetComponent<gameScript>();
		games_.InitAddonBundle(component);
		component.SetMyName(uiObjects[0].GetComponent<InputField>().text);
		component.ownerID = mS_.myID;
		component.developerID = mS_.myID;
		component.publisherID = -1;
		component.typ_standard = false;
		component.typ_nachfolger = false;
		component.nachfolger_created = false;
		component.typ_remaster = false;
		component.typ_budget = false;
		component.typ_addon = false;
		component.typ_addonStandalone = false;
		component.typ_bundle = false;
		component.typ_mmoaddon = false;
		component.typ_bundleAddon = true;
		component.warBeiAwards = true;
		component.weeksOnMarket = 0;
		component.releaseDate = 0;
		component.vorbestellungen = 0;
		component.date_year = mS_.year;
		component.date_month = mS_.month;
		component.spielbericht = false;
		component.spielbericht_favorit = false;
		component.userPositiv = 0;
		component.userNegativ = 0;
		component.reviewGameplayText = 0;
		component.reviewGrafikText = 0;
		component.reviewSoundText = 0;
		component.reviewSteuerungText = 0;
		component.reviewTotalText = 0;
		component.sellsTotalStandard = 0L;
		component.sellsTotalDeluxe = 0L;
		component.sellsTotalCollectors = 0L;
		component.sellsTotalOnline = 0L;
		component.sellsTotal = 0L;
		component.umsatzTotal = 0L;
		component.costs_entwicklung = 0L;
		component.costs_mitarbeiter = 0L;
		component.costs_marketing = 0L;
		component.costs_enginegebuehren = 0L;
		component.costs_server = 0L;
		component.costs_production = 0L;
		for (int j = 0; j < component.sellsPerWeek.Length; j++)
		{
			component.sellsPerWeek[j] = 0;
		}
		component.lagerbestand[0] = 0L;
		component.lagerbestand[1] = 0L;
		component.lagerbestand[2] = 0L;
		for (int k = 0; k < games.Length; k++)
		{
			if ((bool)games[k])
			{
				component.bundleID[k] = games[k].myID;
			}
		}
		component.reviewTotal -= 16;
		if ((bool)games[1])
		{
			component.reviewTotal += 4;
		}
		if ((bool)games[2])
		{
			component.reviewTotal += 4;
		}
		if ((bool)games[3])
		{
			component.reviewTotal += 4;
		}
		if ((bool)games[4])
		{
			component.reviewTotal += 4;
		}
		component.reviewTotal -= (mS_.year - component.date_start_year) * 2;
		if (component.reviewTotal <= 0)
		{
			component.reviewTotal = 1;
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[218]);
		guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(component, null, newGame: true, hideClose: false);
	}
}
