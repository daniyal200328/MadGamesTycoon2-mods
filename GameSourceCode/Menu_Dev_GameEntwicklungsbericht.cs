using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_GameEntwicklungsbericht : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private licences licences_;

	private engineFeatures eF_;

	private cameraMovementScript cmS_;

	private unlockScript unlock_;

	private gameplayFeatures gF_;

	private games games_;

	private gameScript gS_;

	private roomScript rS_;

	private float updateTimer;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!genres_)
			{
				genres_ = main_.GetComponent<genres>();
			}
			if (!themes_)
			{
				themes_ = main_.GetComponent<themes>();
			}
			if (!licences_)
			{
				licences_ = main_.GetComponent<licences>();
			}
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
			}
			if (!cmS_)
			{
				cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
			}
			if (!unlock_)
			{
				unlock_ = main_.GetComponent<unlockScript>();
			}
			if (!gF_)
			{
				gF_ = main_.GetComponent<gameplayFeatures>();
			}
			if (!games_)
			{
				games_ = main_.GetComponent<games>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		cmS_.disableMovement = true;
	}

	private void OnDisable()
	{
		FindScripts();
		cmS_.disableMovement = false;
	}

	private void Update()
	{
		MultiplayerUpdate();
	}

	private void MultiplayerUpdate()
	{
		if (mS_.multiplayer)
		{
			updateTimer += Time.deltaTime;
			if (!(updateTimer < 1f))
			{
				updateTimer = 0f;
				SetData();
			}
		}
	}

	public void Init(gameScript game_, roomScript room_)
	{
		FindScripts();
		gS_ = game_;
		rS_ = room_;
		SetLeitenderDesigner(GetLeitenderEntwickler(), manuellSelectet: false);
		string nameSimple = gS_.GetNameSimple();
		nameSimple = nameSimple.Replace(" <color=green>" + tS_.GetText(1549) + "</color>", string.Empty);
		nameSimple = nameSimple.Replace(" <color=green>" + tS_.GetText(1896) + "</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>[A]</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>[P]</color>", string.Empty);
		nameSimple = nameSimple.Replace("<color=green>", string.Empty);
		nameSimple = nameSimple.Replace("[P]", string.Empty);
		nameSimple = nameSimple.Replace("[A]", string.Empty);
		nameSimple = nameSimple.Replace("</color>", string.Empty);
		nameSimple = nameSimple.Replace("\n", string.Empty);
		nameSimple = nameSimple.Replace("\r", string.Empty);
		nameSimple = nameSimple.Replace("\t", string.Empty);
		nameSimple = nameSimple.Replace(tS_.GetText(1896), string.Empty);
		nameSimple = nameSimple.Replace(tS_.GetText(1549), string.Empty);
		uiObjects[0].GetComponent<InputField>().text = nameSimple;
		uiObjects[22].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[0].SetActive(value: true);
		uiObjects[23].SetActive(value: false);
		uiObjects[20].GetComponent<Image>().sprite = games_.gameSizeSprites[gS_.gameSize];
		string text = "";
		text = genres_.GetName(gS_.maingenre);
		if (gS_.subgenre != -1)
		{
			text = text + " / " + genres_.GetName(gS_.subgenre);
		}
		text = text + "\n" + tS_.GetThemes(gS_.gameMainTheme);
		if (gS_.gameSubTheme != -1)
		{
			text = text + " / " + tS_.GetThemes(gS_.gameSubTheme);
		}
		uiObjects[1].GetComponent<Text>().text = text;
		SetData();
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] != -1)
			{
				GameObject gameObject = GameObject.Find("PLATFORM_" + gS_.gamePlatform[i]);
				if ((bool)gameObject)
				{
					platformScript component = gameObject.GetComponent<platformScript>();
					uiObjects[13 + i].SetActive(value: true);
					component.SetPic(uiObjects[13 + i]);
					uiObjects[13 + i].GetComponent<tooltip>().c = component.GetTooltip();
				}
			}
			else
			{
				uiObjects[13 + i].SetActive(value: false);
			}
		}
		uiObjects[17].GetComponent<Component_Aufwertungen>().Init(gS_);
		uiObjects[18].GetComponent<Image>().sprite = gS_.GetTypSprite();
		uiObjects[24].GetComponent<tooltip>().c = gS_.GetTypString();
		uiObjects[19].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		uiObjects[25].GetComponent<tooltip>().c = gS_.GetPlatformTypString();
		if (gS_.typ_contractGame)
		{
			uiObjects[0].GetComponent<InputField>().interactable = false;
		}
		else
		{
			uiObjects[0].GetComponent<InputField>().interactable = true;
		}
		ShowContractDaten();
	}

	private void SetData()
	{
		uiObjects[2].GetComponent<Image>().fillAmount = gS_.GetProzentGesamt() * 0.01f;
		uiObjects[3].GetComponent<Text>().text = tS_.GetText(450) + " " + mS_.Round(gS_.GetProzentGesamt(), 1) + "%";
		uiObjects[4].GetComponent<Text>().text = tS_.GetText(6) + " <color=red>" + mS_.GetMoney(gS_.GetGesamtGewinn(), showDollar: true) + "</color>";
		string text = tS_.GetText(1775);
		text = text.Replace("<NUM>", gS_.weeksInDevelopment.ToString());
		uiObjects[31].GetComponent<Text>().text = text + " <color=grey> [" + gS_.GetEntwicklungsbeginnDateString() + "]</color>";
		gS_.CalcReview(entwicklungsbericht: true);
		int num = gS_.reviewTotal - 10;
		int num2 = gS_.reviewTotal + 10;
		num = num / 10 * 10;
		num2 = num2 / 10 * 10;
		if (num < 1)
		{
			num = 1;
		}
		if (num2 > 100)
		{
			num2 = 100;
		}
		text = " " + num + "% - " + num2 + "%";
		uiObjects[5].GetComponent<Text>().text = tS_.GetText(452) + "<color=blue>" + text + "</color>";
		gS_.ClearReview();
		uiObjects[6].GetComponent<Image>().sprite = gS_.GetScreenshot();
		uiObjects[7].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay).ToString();
		uiObjects[8].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik).ToString();
		uiObjects[9].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound).ToString();
		uiObjects[10].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik).ToString();
		uiObjects[11].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_bugs).ToString();
		uiObjects[12].GetComponent<Text>().text = Mathf.RoundToInt(gS_.hype).ToString();
		uiObjects[35].GetComponent<Text>().text = mS_.Round(gS_.GetIpBekanntheit(), 1).ToString();
	}

	private void ShowContractDaten()
	{
		if (gS_.typ_contractGame)
		{
			uiObjects[26].SetActive(value: true);
			string text = tS_.GetText(605);
			text = text.Replace("<NUM>", gS_.auftragsspiel_zeitInWochen.ToString());
			uiObjects[27].GetComponent<Text>().text = text;
			text = tS_.GetText(626);
			text = text.Replace("<NUM>", gS_.auftragsspiel_mindestbewertung.ToString());
			uiObjects[28].GetComponent<Text>().text = text;
			uiObjects[29].GetComponent<Text>().text = tS_.GetText(600) + ": " + mS_.GetMoney(gS_.auftragsspiel_gehalt, showDollar: true);
			uiObjects[30].GetComponent<Text>().text = tS_.GetText(627) + ": " + mS_.GetMoney(gS_.auftragsspiel_bonus, showDollar: true);
		}
		else
		{
			uiObjects[26].SetActive(value: false);
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		if (uiObjects[0].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(412), closeMenu: false);
			return;
		}
		if (!gS_.typ_contractGame && gS_.portID == -1)
		{
			GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
			if (array.Length != 0)
			{
				for (int i = 0; i < array.Length; i++)
				{
					gameScript component = array[i].GetComponent<gameScript>();
					if ((bool)component && gS_.myID != component.myID && component.GetNameSimple() == uiObjects[0].GetComponent<InputField>().text)
					{
						guiMain_.MessageBox(tS_.GetText(618), closeMenu: false);
						return;
					}
				}
			}
		}
		if (uiObjects[0].activeSelf && uiObjects[0].GetComponent<InputField>().text.Length > 0 && uiObjects[0].GetComponent<InputField>().interactable)
		{
			gS_.SetMyName(uiObjects[0].GetComponent<InputField>().text);
		}
		BUTTON_Close();
	}

	public void SetLeitenderDesigner(characterScript charS_, bool manuellSelectet)
	{
		taskGame taskGame2 = null;
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame2 = gameObject.GetComponent<taskGame>();
		}
		if (!charS_)
		{
			float num = 0f;
			GameObject[] array = GameObject.FindGameObjectsWithTag("Character");
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i])
				{
					continue;
				}
				characterScript component = array[i].GetComponent<characterScript>();
				if ((bool)component && component.roomID == rS_.myID)
				{
					if (component.s_gamedesign > num)
					{
						num = component.s_gamedesign;
						charS_ = component;
					}
					if (rS_.leitenderGamedesigner == component.myID)
					{
						charS_ = component;
						break;
					}
				}
			}
		}
		if (!charS_)
		{
			uiObjects[21].GetComponent<Text>().text = "---";
			taskGame2.leitenderDesignerID = -1;
			taskGame2.designer_ = null;
			rS_.leitenderGamedesigner = -1;
			return;
		}
		uiObjects[21].GetComponent<Text>().text = charS_.myName;
		taskGame2.leitenderDesignerID = charS_.myID;
		taskGame2.designer_ = charS_;
		if (rS_.leitenderGamedesigner != charS_.myID)
		{
			rS_.leitenderGamedesigner = -1;
		}
		if (manuellSelectet)
		{
			rS_.leitenderGamedesigner = charS_.myID;
		}
	}

	public characterScript GetLeitenderEntwickler()
	{
		GameObject gameObject = GameObject.Find("Task_" + rS_.taskID);
		if ((bool)gameObject)
		{
			taskGame component = gameObject.GetComponent<taskGame>();
			if ((bool)component)
			{
				return component.designer_;
			}
		}
		return null;
	}

	public string GetBeschreibung()
	{
		return gS_.beschreibung;
	}

	public void SetBeschreibung(string c)
	{
		gS_.beschreibung = c;
	}

	public void BUTTON_LeitenderEntwickler()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[196]);
		guiMain_.uiObjects[196].GetComponent<Menu_Dev_LeitenderDesigner>().Init(rS_);
	}

	public void BUTTON_Spielbeschreibung()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[198]);
	}
}
