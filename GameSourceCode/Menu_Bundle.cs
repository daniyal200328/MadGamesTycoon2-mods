using UnityEngine;
using UnityEngine.UI;

public class Menu_Bundle : MonoBehaviour
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
			uiObjects[12 + slot].GetComponent<Image>().sprite = genres_.GetPic(script_.maingenre);
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
				int num2 = mS_.year - games[i].date_year;
				num2 *= 3;
				num2 = games[i].reviewTotal - num2;
				num = ((num2 <= 0) ? (num + 1f) : (num + (float)num2));
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
		guiMain_.ActivateMenu(guiMain_.uiObjects[268]);
		guiMain_.uiObjects[268].GetComponent<Menu_BundleSelect>().Init(i);
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
			guiMain_.MessageBox(tS_.GetText(1343), closeMenu: false);
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
		CreateBundleGame();
	}

	private void CreateBundleGame()
	{
		for (int i = 0; i < games.Length; i++)
		{
			if ((bool)games[i])
			{
				games[i].bundle_created = true;
			}
		}
		gameScript gameScript2 = games_.CreateNewGame(fromSavegame: false, setDate: true);
		games_.FindGames();
		gameScript2.SetMyName(uiObjects[0].GetComponent<InputField>().text);
		gameScript2.ownerID = mS_.myID;
		gameScript2.typ_standard = false;
		gameScript2.typ_bundle = true;
		gameScript2.warBeiAwards = true;
		gameScript2.developerID = mS_.myID;
		gameScript2.publisherID = mS_.myID;
		gameScript2.date_year = mS_.year;
		gameScript2.date_month = mS_.month;
		gameScript2.usk = 0;
		for (int j = 0; j < games.Length; j++)
		{
			if ((bool)games[j])
			{
				gameScript2.maingenre = games[j].maingenre;
				gameScript2.gameMainTheme = games[j].gameMainTheme;
				break;
			}
		}
		gameScript2.reviewGameplay = Mathf.RoundToInt(GetTotalReview());
		gameScript2.reviewGrafik = Mathf.RoundToInt(GetTotalReview());
		gameScript2.reviewSound = Mathf.RoundToInt(GetTotalReview());
		gameScript2.reviewSteuerung = Mathf.RoundToInt(GetTotalReview());
		gameScript2.reviewTotal = Mathf.RoundToInt(GetTotalReview());
		gameScript2.usk = 0;
		for (int k = 0; k < games.Length; k++)
		{
			if (!games[k])
			{
				continue;
			}
			gameScript2.bundleID[k] = games[k].myID;
			if (games[k].usk > gameScript2.usk)
			{
				gameScript2.usk = games[k].usk;
			}
			for (int l = 0; l < games[k].gameLanguage.Length; l++)
			{
				if (games[k].gameLanguage[l])
				{
					gameScript2.gameLanguage[l] = true;
				}
			}
			gameScript2.points_gameplay += games[k].points_gameplay;
			gameScript2.points_grafik += games[k].points_grafik;
			gameScript2.points_sound += games[k].points_sound;
			gameScript2.points_technik += games[k].points_technik;
		}
		int num = 0;
		for (int m = 0; m < games.Length; m++)
		{
			if (!games[m])
			{
				continue;
			}
			for (int n = 0; n < games[m].gamePlatform.Length; n++)
			{
				if (games[m].gamePlatform[n] != -1 && games[m].gamePlatform[n] != gameScript2.gamePlatform[0] && games[m].gamePlatform[n] != gameScript2.gamePlatform[1] && games[m].gamePlatform[n] != gameScript2.gamePlatform[2] && games[m].gamePlatform[n] != gameScript2.gamePlatform[3] && num < 4)
				{
					gameScript2.gamePlatform[num] = games[m].gamePlatform[n];
					num++;
				}
			}
		}
		for (int num2 = 0; num2 < gameScript2.standard_edition.Length; num2++)
		{
			gameScript2.standard_edition[num2] = false;
			gameScript2.deluxe_edition[num2] = false;
			gameScript2.collectors_edition[num2] = false;
		}
		guiMain_.ActivateMenu(guiMain_.uiObjects[218]);
		guiMain_.uiObjects[218].GetComponent<Menu_Packung>().Init(gameScript2, null, newGame: true, hideClose: false);
	}
}
