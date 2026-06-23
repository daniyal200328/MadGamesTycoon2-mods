using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_ArcadePreis : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] prodCostsCase;

	public int[] prodCostsMonitor;

	public int[] prodCostsJoystick;

	public int[] prodCostsSound;

	public int setCase;

	public int setMonitor;

	public int setJoystick;

	public int setSound;

	public int orgPreis;

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

	private taskGame task_;

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

	public void Init(gameScript game_, taskGame t_)
	{
		FindScripts();
		gS_ = game_;
		task_ = t_;
		orgPreis = game_.verkaufspreis[0];
		if ((bool)task_)
		{
			uiObjects[19].SetActive(value: true);
			uiObjects[20].SetActive(value: true);
			uiObjects[21].SetActive(value: true);
			uiObjects[22].SetActive(value: true);
			uiObjects[23].SetActive(value: true);
			uiObjects[24].SetActive(value: true);
			uiObjects[25].SetActive(value: true);
			uiObjects[26].SetActive(value: true);
			setCase = 4;
			setMonitor = 4;
			setJoystick = 4;
			setSound = 4;
		}
		else
		{
			uiObjects[19].SetActive(value: false);
			uiObjects[20].SetActive(value: false);
			uiObjects[21].SetActive(value: false);
			uiObjects[22].SetActive(value: false);
			uiObjects[23].SetActive(value: false);
			uiObjects[24].SetActive(value: false);
			uiObjects[25].SetActive(value: false);
			uiObjects[26].SetActive(value: false);
			setCase = gS_.arcadeCase;
			setMonitor = gS_.arcadeMonitor;
			setJoystick = gS_.arcadeJoystick;
			setSound = gS_.arcadeSound;
		}
		uiObjects[0].GetComponent<Text>().text = gS_.GetNameWithTag();
		if (gS_.verkaufspreis[0] <= 0)
		{
			uiObjects[13].GetComponent<Slider>().value = 1000f;
			if (PlayerPrefs.HasKey("ArcadePreis"))
			{
				uiObjects[13].GetComponent<Slider>().value = PlayerPrefs.GetInt("ArcadePreis");
			}
		}
		else
		{
			uiObjects[13].GetComponent<Slider>().value = gS_.verkaufspreis[0];
		}
		SLIDER_Preis();
		SetData();
	}

	private void SetData()
	{
		uiObjects[15].GetComponent<Text>().text = mS_.GetMoney(prodCostsCase[setCase], showDollar: true);
		uiObjects[16].GetComponent<Text>().text = mS_.GetMoney(prodCostsMonitor[setMonitor], showDollar: true);
		uiObjects[17].GetComponent<Text>().text = mS_.GetMoney(prodCostsJoystick[setJoystick], showDollar: true);
		uiObjects[18].GetComponent<Text>().text = mS_.GetMoney(prodCostsSound[setSound], showDollar: true);
		guiMain_.DrawStars(uiObjects[6], setCase + 1);
		guiMain_.DrawStars(uiObjects[7], setMonitor + 1);
		guiMain_.DrawStars(uiObjects[8], setJoystick + 1);
		guiMain_.DrawStars(uiObjects[9], setSound + 1);
		int num = Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value);
		if (gS_.vorbestellungen > 0 && num != orgPreis)
		{
			num = orgPreis;
			uiObjects[13].GetComponent<Slider>().value = orgPreis;
			guiMain_.MessageBox(tS_.GetText(1579), closeMenu: false);
		}
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		int num2 = CalcProdCosts();
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(num2, showDollar: true);
		int num3 = num - num2;
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(num3, showDollar: true);
	}

	public int CalcProdCosts()
	{
		return 0 + prodCostsCase[setCase] + prodCostsMonitor[setMonitor] + prodCostsJoystick[setJoystick] + prodCostsSound[setSound];
	}

	public void BUTTON_Minus_Komponent(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			setCase--;
			if (setCase < 0)
			{
				setCase = 0;
			}
			break;
		case 1:
			setMonitor--;
			if (setMonitor < 0)
			{
				setMonitor = 0;
			}
			break;
		case 2:
			setJoystick--;
			if (setJoystick < 0)
			{
				setJoystick = 0;
			}
			break;
		case 3:
			setSound--;
			if (setSound < 0)
			{
				setSound = 0;
			}
			break;
		}
		SetData();
	}

	public void BUTTON_Plus_Komponent(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			setCase++;
			if (setCase > 4)
			{
				setCase = 4;
			}
			break;
		case 1:
			setMonitor++;
			if (setMonitor > 4)
			{
				setMonitor = 4;
			}
			break;
		case 2:
			setJoystick++;
			if (setJoystick > 4)
			{
				setJoystick = 4;
			}
			break;
		case 3:
			setSound++;
			if (setSound > 4)
			{
				setSound = 4;
			}
			break;
		}
		SetData();
	}

	public void SLIDER_Preis()
	{
		SetData();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if ((bool)task_)
		{
			guiMain_.ActivateMenu(guiMain_.uiObjects[69]);
			guiMain_.uiObjects[69].GetComponent<Menu_DevGame_Complete>().Init(gS_, task_);
			guiMain_.OpenMenu(hideChars: false);
		}
		task_ = null;
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		if (Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value) - CalcProdCosts() < 0)
		{
			guiMain_.MessageBox(tS_.GetText(1534), closeMenu: false);
			return;
		}
		PlayerPrefs.SetInt("ArcadePreis", Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value));
		if ((bool)task_)
		{
			if (gS_.reviewTotal <= 0)
			{
				gS_.CalcReview(entwicklungsbericht: false);
				if ((bool)mS_)
				{
					mS_.reviewText_.GetReviewText(gS_);
				}
			}
			else
			{
				gS_.date_year = mS_.year;
				gS_.date_month = mS_.month;
				if ((bool)mS_)
				{
					mS_.reviewText_.GetReviewText(gS_);
				}
			}
			gS_.SetPublisher(mS_.myID);
			gS_.SetOnMarket();
			guiMain_.ActivateMenu(guiMain_.uiObjects[71]);
			guiMain_.uiObjects[71].GetComponent<Menu_Dev_XP>().Init(gS_);
		}
		if ((bool)task_)
		{
			Object.Destroy(task_.gameObject);
		}
		gS_.verkaufspreis[0] = Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value);
		gS_.arcadeProdCosts = CalcProdCosts();
		gS_.arcadeCase = setCase;
		gS_.arcadeMonitor = setMonitor;
		gS_.arcadeJoystick = setJoystick;
		gS_.arcadeSound = setSound;
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator iMinusPreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusPreis(i);
		}
	}

	public void BUTTON_MinusPreis(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num -= i;
		if (num < 500)
		{
			num = 500;
		}
		uiObjects[13].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusPreis(i));
		SetData();
	}

	private IEnumerator iPlusPreis(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusPreis(i);
		}
	}

	public void BUTTON_PlusPreis(int i)
	{
		int num = Mathf.RoundToInt(uiObjects[13].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num += i;
		if (num > 1500)
		{
			num = 1500;
		}
		uiObjects[13].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusPreis(i));
		SetData();
	}
}
