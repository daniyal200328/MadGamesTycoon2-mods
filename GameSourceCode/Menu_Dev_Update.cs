using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Update : MonoBehaviour
{
	public GameObject[] uiObjects;

	public float[] devCostsPercent;

	private bool[] buttonAdds = new bool[8];

	private bool[] sprachen = new bool[11];

	private int amount;

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

	private bool allSprachen;

	private bool allAdds;

	private bool allBugs;

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

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Init(roomScript roomScript_, gameScript gameScript_)
	{
		FindScripts();
		rS_ = roomScript_;
		gS_ = gameScript_;
		allAdds = false;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = false;
		}
		for (int j = 0; j < sprachen.Length; j++)
		{
			sprachen[j] = false;
			if (gS_.gameLanguage[j])
			{
				uiObjects[26 + j].GetComponent<Button>().interactable = false;
			}
			else
			{
				uiObjects[26 + j].GetComponent<Button>().interactable = true;
			}
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		long num = gS_.costs_entwicklung + gS_.costs_updates;
		if (num > 50000000)
		{
			num = 50000000L;
		}
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[0]), showDollar: true);
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[1]), showDollar: true);
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[2]), showDollar: true);
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[3]), showDollar: true);
		uiObjects[13].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[4]), showDollar: true);
		uiObjects[14].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[5]), showDollar: true);
		uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[6]), showDollar: true);
		uiObjects[16].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt((float)num * devCostsPercent[7]), showDollar: true);
		UpdateGUI();
	}

	private void UpdateGUI()
	{
		uiObjects[1].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_gameplay) + "+" + GetP_Gameplay();
		uiObjects[2].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_grafik) + "+" + GetP_Grafik();
		uiObjects[3].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_sound) + "+" + GetP_Sound();
		uiObjects[4].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_technik) + "+" + GetP_Technik();
		uiObjects[5].GetComponent<Text>().text = Mathf.RoundToInt(gS_.points_bugs) + "-" + GetP_Bugs();
		uiObjects[7].GetComponent<Text>().text = uiObjects[6].GetComponent<Slider>().value * 10f + "%";
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(GetCosts_Bugs(), showDollar: true);
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				uiObjects[17 + i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiObjects[17 + i].GetComponent<Image>().color = Color.white;
			}
		}
		for (int j = 0; j < sprachen.Length; j++)
		{
			if (sprachen[j])
			{
				uiObjects[26 + j].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiObjects[26 + j].GetComponent<Image>().color = Color.white;
			}
		}
		uiObjects[25].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		if (amount <= 0)
		{
			string text = tS_.GetText(2065);
			text = text.Replace("<size=20>", "<size=30>");
			uiObjects[38].GetComponent<Text>().text = text;
		}
		else
		{
			uiObjects[38].GetComponent<Text>().text = "x" + amount;
		}
	}

	private int GetP_Gameplay()
	{
		float num = 0f;
		if (buttonAdds[0])
		{
			num += 1f;
			num += gS_.points_gameplay * 0.02f;
		}
		if (buttonAdds[1])
		{
			num += 1f;
			num += gS_.points_gameplay * 0.02f;
		}
		return Mathf.RoundToInt(num);
	}

	private int GetP_Grafik()
	{
		float num = 0f;
		if (buttonAdds[2])
		{
			num += 1f;
			num += gS_.points_grafik * 0.02f;
		}
		if (buttonAdds[3])
		{
			num += 1f;
			num += gS_.points_grafik * 0.02f;
		}
		return Mathf.RoundToInt(num);
	}

	private int GetP_Sound()
	{
		float num = 0f;
		if (buttonAdds[4])
		{
			num += 1f;
			num += gS_.points_sound * 0.02f;
		}
		if (buttonAdds[5])
		{
			num += 1f;
			num += gS_.points_sound * 0.02f;
		}
		return Mathf.RoundToInt(num);
	}

	private int GetP_Technik()
	{
		float num = 0f;
		if (buttonAdds[6])
		{
			num += 1f;
			num += gS_.points_technik * 0.02f;
		}
		if (buttonAdds[7])
		{
			num += 1f;
			num += gS_.points_technik * 0.02f;
		}
		return Mathf.RoundToInt(num);
	}

	private int GetP_Bugs()
	{
		float points_bugs = gS_.points_bugs;
		float value = uiObjects[6].GetComponent<Slider>().value;
		value *= 0.1f;
		return Mathf.RoundToInt(points_bugs * value);
	}

	private long GetCosts_Bugs()
	{
		return GetP_Bugs() * (gS_.costs_entwicklung / 7500);
	}

	private long GetDevCosts()
	{
		long num = GetCosts_Bugs();
		long num2 = gS_.costs_entwicklung + gS_.costs_updates;
		if (num2 > 50000000)
		{
			num2 = 50000000L;
		}
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			if (buttonAdds[i])
			{
				num += Mathf.RoundToInt((float)num2 * devCostsPercent[i]);
			}
		}
		for (int j = 0; j < sprachen.Length; j++)
		{
			if (sprachen[j] && !mS_.Muttersprache(j))
			{
				num += gS_.GetGesamtDevPoints() * 5;
			}
		}
		return num;
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[105]);
		guiMain_.uiObjects[105].GetComponent<Menu_Dev_UpdateSelectGame>().Init(rS_, 0);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Adds(int i)
	{
		sfx_.PlaySound(3, force: true);
		buttonAdds[i] = !buttonAdds[i];
		UpdateGUI();
	}

	public void BUTTON_Sprache(int i)
	{
		sfx_.PlaySound(3, force: true);
		sprachen[i] = !sprachen[i];
		UpdateGUI();
	}

	public void BUTTON_AlleSprache()
	{
		sfx_.PlaySound(3, force: true);
		allSprachen = !allSprachen;
		for (int i = 0; i < sprachen.Length; i++)
		{
			if (uiObjects[26 + i].GetComponent<Button>().interactable)
			{
				sprachen[i] = allSprachen;
			}
		}
		UpdateGUI();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		allAdds = !allAdds;
		for (int i = 0; i < buttonAdds.Length; i++)
		{
			buttonAdds[i] = allAdds;
		}
		UpdateGUI();
	}

	public void BUTTON_AllBugs()
	{
		sfx_.PlaySound(3, force: true);
		allBugs = !allBugs;
		if (allBugs)
		{
			uiObjects[6].GetComponent<Slider>().value = 10f;
		}
		else
		{
			uiObjects[6].GetComponent<Slider>().value = 0f;
		}
	}

	public void BUTTON_Start()
	{
		int num = Mathf.RoundToInt(GetDevCosts());
		if (!gS_ || !rS_)
		{
			return;
		}
		if (num <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(662), closeMenu: false);
			return;
		}
		if (uiObjects[37].GetComponent<Toggle>().isOn)
		{
			bool flag = false;
			for (int i = 0; i < buttonAdds.Length; i++)
			{
				if (buttonAdds[i])
				{
					flag = true;
				}
			}
			if (!flag)
			{
				guiMain_.MessageBox(tS_.GetText(727), closeMenu: false);
				return;
			}
		}
		sfx_.PlaySound(3, force: true);
		mS_.Pay(num, 15);
		taskUpdate taskUpdate2 = guiMain_.AddTask_Update();
		taskUpdate2.Init(fromSavegame: false);
		taskUpdate2.targetID = gS_.myID;
		taskUpdate2.devCosts = Mathf.RoundToInt(GetDevCosts());
		taskUpdate2.pointsGameplay = GetP_Gameplay();
		taskUpdate2.pointsGrafik = GetP_Grafik();
		taskUpdate2.pointsSound = GetP_Sound();
		taskUpdate2.pointsTechnik = GetP_Technik();
		taskUpdate2.pointsBugs = GetP_Bugs();
		taskUpdate2.automatic = uiObjects[37].GetComponent<Toggle>().isOn;
		if (amount > 0)
		{
			taskUpdate2.autoAmount = amount + 1;
		}
		float num2 = gS_.GetGesamtDevPoints();
		num2 *= 0.1f;
		taskUpdate2.points = Mathf.RoundToInt(num2);
		for (int j = 0; j < buttonAdds.Length; j++)
		{
			if (buttonAdds[j])
			{
				taskUpdate2.quality += 0.1f;
				taskUpdate2.points += Mathf.RoundToInt((float)gS_.GetGesamtDevPoints() * 0.02f);
			}
		}
		for (int k = 0; k < sprachen.Length; k++)
		{
			if (sprachen[k])
			{
				taskUpdate2.quality += 0.02f;
				taskUpdate2.sprachen[k] = sprachen[k];
				taskUpdate2.points += 10f;
			}
		}
		taskUpdate2.points += (float)GetP_Bugs() * 0.3f;
		taskUpdate2.pointsLeft = taskUpdate2.points;
		GameObject gameObject = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject)
		{
			gameObject.GetComponent<roomScript>().taskID = taskUpdate2.myID;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void SLIDER_Bugs()
	{
		UpdateGUI();
	}

	public void TOGGLE_Auto()
	{
	}

	private IEnumerator iMinus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Minus(i);
		}
	}

	public void BUTTON_Minus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			amount -= 10;
		}
		else
		{
			amount--;
		}
		if (amount < 0)
		{
			amount = 0;
		}
		StartCoroutine(iMinus(i));
		UpdateGUI();
	}

	private IEnumerator iPlus(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_Plus(i);
		}
	}

	public void BUTTON_Plus(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			amount += 10;
		}
		else
		{
			amount++;
		}
		if (amount > 99)
		{
			amount = 99;
		}
		StartCoroutine(iPlus(i));
		UpdateGUI();
	}
}
