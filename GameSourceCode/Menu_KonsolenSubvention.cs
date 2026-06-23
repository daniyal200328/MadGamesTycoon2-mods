using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_KonsolenSubvention : MonoBehaviour
{
	public GameObject[] uiObjects;

	private bool[] buttons = new bool[6];

	public GameObject[] uiButtons;

	public int subventionsMoney;

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

	public platformScript pS_;

	public taskKonsole task_;

	private bool all;

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

	public void Init(platformScript plat_, taskKonsole t_)
	{
		FindScripts();
		pS_ = plat_;
		task_ = t_;
		if ((bool)pS_)
		{
			uiObjects[5].GetComponent<Text>().text = pS_.GetAmountSubventionierteGames().ToString();
			uiObjects[6].GetComponent<Text>().text = mS_.GetMoney(pS_.costs_subvention, showDollar: true);
			subventionsMoney = pS_.subventionMoney;
			uiObjects[3].GetComponent<InputField>().text = subventionsMoney.ToString();
			uiObjects[2].GetComponent<Slider>().value = subventionsMoney / 1000;
			for (int i = 0; i < buttons.Length; i++)
			{
				buttons[i] = pS_.subventionGameSize[i];
			}
			SLIDER_Subvention();
			SetData();
		}
	}

	private void Update()
	{
	}

	public void UpdateMoney()
	{
		if (uiObjects[3].GetComponent<InputField>().text.Length > 0)
		{
			long i = long.Parse(uiObjects[3].GetComponent<InputField>().text);
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(i, showDollar: true);
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(0L, showDollar: true);
		}
	}

	public void SetData()
	{
		uiObjects[0].GetComponent<Text>().text = pS_.GetName();
		pS_.SetPic(uiObjects[1]);
		uiObjects[3].GetComponent<InputField>().text = subventionsMoney.ToString();
		uiObjects[4].GetComponent<Text>().text = mS_.GetMoney(subventionsMoney, showDollar: true);
		long num = mS_.GetDevCostsSubvention(0);
		num = num / 1000 * 750;
		uiObjects[7].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		num = mS_.GetDevCostsSubvention(1);
		num = num / 1000 * 750;
		uiObjects[8].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		num = mS_.GetDevCostsSubvention(2);
		num = num / 1000 * 750;
		uiObjects[9].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		num = mS_.GetDevCostsSubvention(3);
		num = num / 1000 * 750;
		uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		num = mS_.GetDevCostsSubvention(4);
		num = num / 1000 * 750;
		uiObjects[11].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		num = mS_.GetDevCostsSubvention(5);
		num = num / 1000 * 750;
		uiObjects[12].GetComponent<Text>().text = mS_.GetMoney(num, showDollar: true);
		for (int i = 0; i < uiButtons.Length; i++)
		{
			if (buttons[i])
			{
				uiButtons[i].GetComponent<Image>().color = guiMain_.colors[7];
			}
			else
			{
				uiButtons[i].GetComponent<Image>().color = Color.white;
			}
		}
		for (int j = 0; j < buttons.Length; j++)
		{
			if (buttons[j])
			{
				if (!uiObjects[13 + j].activeSelf)
				{
					uiObjects[13 + j].SetActive(value: true);
				}
			}
			else if (uiObjects[13 + j].activeSelf)
			{
				uiObjects[13 + j].SetActive(value: false);
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Ok()
	{
		sfx_.PlaySound(3, force: true);
		if (subventionsMoney > 0 && !buttons[0] && !buttons[1] && !buttons[2] && !buttons[3] && !buttons[4] && !buttons[5])
		{
			guiMain_.MessageBox(tS_.GetText(2395), closeMenu: false);
			return;
		}
		pS_.subventionMoney = subventionsMoney;
		for (int i = 0; i < pS_.subventionGameSize.Length; i++)
		{
			pS_.subventionGameSize[i] = buttons[i];
		}
		base.gameObject.SetActive(value: false);
	}

	private IEnumerator iMinusSubvention()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusSubvention();
		}
	}

	public void BUTTON_MinusSubvention()
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num--;
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iMinusSubvention());
		SetData();
	}

	private IEnumerator iPlusSubvention()
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_PlusSubvention();
		}
	}

	public void BUTTON_PlusSubvention()
	{
		int num = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		sfx_.PlaySound(3, force: true);
		num++;
		uiObjects[2].GetComponent<Slider>().value = num;
		StartCoroutine(iPlusSubvention());
		SetData();
	}

	public void INPUTFIELD_Subvention()
	{
		if (uiObjects[3].GetComponent<InputField>().text.Length <= 0)
		{
			subventionsMoney = 0;
			return;
		}
		long num = long.Parse(uiObjects[3].GetComponent<InputField>().text);
		num = num / 1000 * 1000;
		if (num > 1000000000)
		{
			num = 1000000000L;
		}
		subventionsMoney = (int)num;
		uiObjects[2].GetComponent<Slider>().value = subventionsMoney / 1000;
		SetData();
	}

	public void SLIDER_Subvention()
	{
		subventionsMoney = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value) * 1000;
		SetData();
	}

	public void BUTTON_GameSize(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (uiButtons[i].GetComponent<Button>().interactable)
		{
			buttons[i] = !buttons[i];
		}
		SetData();
	}

	public void BUTTON_AlleAdds()
	{
		sfx_.PlaySound(3, force: true);
		all = !all;
		for (int i = 0; i < uiButtons.Length; i++)
		{
			if (uiButtons[i].GetComponent<Button>().interactable)
			{
				buttons[i] = all;
			}
		}
		SetData();
	}
}
