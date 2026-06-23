using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_PersonalLohnverhandlung : MonoBehaviour
{
	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private pickCharacterScript pcS_;

	private characterScript cS_;

	private cameraMovementScript cmS_;

	private mapScript mapS_;

	public GameObject[] uiObjects;

	private float stimmung = 100f;

	private float gehaltsangebot;

	private float gehaltsangebot_buffer;

	private float gehaltsvorstellung;

	private float gehaltsangebot_min;

	private float updateTimer;

	private void Start()
	{
		FindScripts();
	}

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
		if (!cS_)
		{
			BUTTON_Close();
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
				SetData();
			}
		}
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
		if (!pcS_)
		{
			pcS_ = main_.GetComponent<pickCharacterScript>();
		}
		if (!mapS_)
		{
			mapS_ = main_.GetComponent<mapScript>();
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
		if (!cmS_)
		{
			cmS_ = GameObject.Find("CamMovement").GetComponent<cameraMovementScript>();
		}
	}

	public void Init(characterScript charScript_)
	{
		FindScripts();
		cS_ = charScript_;
		stimmung = 100f;
		gehaltsvorstellung = (float)cS_.CalcGehalt() * (1.1f + Random.Range(0f, 0.1f));
		gehaltsangebot_min = (float)cS_.CalcGehalt() * (0.8f - Random.Range(0f, 0.1f));
		gehaltsangebot = gehaltsvorstellung;
		gehaltsangebot_buffer = 0f;
		uiObjects[26].GetComponent<Slider>().minValue = gehaltsangebot_min;
		uiObjects[26].GetComponent<Slider>().maxValue = gehaltsvorstellung - 1f;
		uiObjects[26].GetComponent<Slider>().value = gehaltsvorstellung + 1f;
		mS_.CreateFoto(cS_, null);
		SetData();
	}

	private void SetData()
	{
		if ((bool)cS_)
		{
			uiObjects[0].GetComponent<Text>().text = cS_.myName;
			uiObjects[17].GetComponent<Text>().text = tS_.GetText(137 + cS_.beruf);
			guiMain_.SetBalkenEmployee(uiObjects[1], cS_.s_gamedesign, 0, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[2], cS_.s_programmieren, 1, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[3], cS_.s_grafik, 2, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[4], cS_.s_sound, 3, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[5], cS_.s_pr, 4, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[6], cS_.s_gametests, 5, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[7], cS_.s_technik, 6, cS_);
			guiMain_.SetBalkenEmployee(uiObjects[8], cS_.s_forschen, 7, cS_);
			guiMain_.CreatePerkIcons(cS_, uiObjects[9].transform);
			uiObjects[10].GetComponent<Text>().text = mS_.GetMoney(cS_.GetGehalt(), showDollar: true);
			uiObjects[13].GetComponent<Text>().text = cS_.GetGroupString("magenta");
			string text = tS_.GetText(2044);
			if (stimmung <= 0f)
			{
				text = tS_.GetText(2051);
			}
			if (gehaltsvorstellung <= gehaltsangebot_buffer)
			{
				text = tS_.GetText(2050);
			}
			float f = GetGehaltsvorstellung();
			text = text.Replace("<NUM>", "<color=red>" + mS_.GetMoney(Mathf.RoundToInt(f), showDollar: true) + "</color>");
			uiObjects[27].GetComponent<Text>().text = text;
			uiObjects[25].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(cS_.RecalculateGehalt(gehaltsangebot)), showDollar: true);
			uiObjects[24].GetComponent<Image>().fillAmount = stimmung * 0.01f;
			uiObjects[29].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(cS_.RecalculateGehalt(gehaltsangebot_min)), showDollar: true);
			uiObjects[30].GetComponent<Text>().text = mS_.GetMoney(Mathf.RoundToInt(cS_.RecalculateGehalt(gehaltsvorstellung) - 1f), showDollar: true);
			if (stimmung < 33f)
			{
				uiObjects[24].GetComponent<Image>().color = guiMain_.colorsBalken[0];
			}
			if (stimmung > 33f && stimmung < 66f)
			{
				uiObjects[24].GetComponent<Image>().color = guiMain_.colorsBalken[1];
			}
			if (stimmung > 66f)
			{
				uiObjects[24].GetComponent<Image>().color = guiMain_.colorsBalken[2];
			}
			if (stimmung <= 0f)
			{
				uiObjects[28].GetComponent<Button>().interactable = false;
				uiObjects[26].GetComponent<Slider>().interactable = false;
				uiObjects[31].GetComponent<Button>().interactable = true;
				uiObjects[32].GetComponent<Text>().text = "<color=red>" + tS_.GetText(2047) + "</color>";
			}
			else
			{
				uiObjects[28].GetComponent<Button>().interactable = true;
				uiObjects[26].GetComponent<Slider>().interactable = true;
				uiObjects[31].GetComponent<Button>().interactable = false;
				uiObjects[32].GetComponent<Text>().text = "<color=white>" + tS_.GetText(2045) + "</color>";
			}
		}
	}

	private Color GetValColor(float val)
	{
		if (val < 30f)
		{
			return guiMain_.colorsBalken[0];
		}
		if (val >= 30f && val < 70f)
		{
			return guiMain_.colorsBalken[1];
		}
		if (val >= 70f)
		{
			return guiMain_.colorsBalken[2];
		}
		return guiMain_.colorsBalken[0];
	}

	public float GetGehaltsvorstellung()
	{
		return cS_.RecalculateGehalt(gehaltsvorstellung);
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: false);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Akzeptieren()
	{
		sfx_.PlaySound(3, force: false);
		cS_.gehalt = Mathf.RoundToInt(gehaltsangebot);
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void SLIDER_Angebot()
	{
		gehaltsangebot = Mathf.RoundToInt(uiObjects[26].GetComponent<Slider>().value);
		SetData();
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
			uiObjects[26].GetComponent<Slider>().value -= 100f;
		}
		else
		{
			uiObjects[26].GetComponent<Slider>().value -= 10f;
		}
		StartCoroutine(iMinus(i));
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
			uiObjects[26].GetComponent<Slider>().value += 100f;
		}
		else
		{
			uiObjects[26].GetComponent<Slider>().value += 10f;
		}
		StartCoroutine(iPlus(i));
	}

	public void BUTTON_Angebot()
	{
		sfx_.PlaySound(3, force: true);
		if (!(stimmung <= 0f))
		{
			gehaltsangebot_buffer = gehaltsangebot;
			float num = cS_.CalcGehalt();
			float num2 = gehaltsangebot;
			num /= 100f;
			float num3 = num2 / num;
			gehaltsangebot_min = gehaltsangebot;
			if (num3 >= 100f)
			{
				stimmung -= Random.Range(5f, 10f);
				gehaltsvorstellung *= Random.Range(0.9f, 0.95f);
			}
			else
			{
				stimmung -= Random.Range(25f, 50f);
				gehaltsvorstellung *= Random.Range(0.95f, 0.98f);
			}
			if (gehaltsangebot_min > gehaltsvorstellung)
			{
				gehaltsangebot_min = gehaltsvorstellung;
				stimmung = 0f;
			}
			if (gehaltsvorstellung < gehaltsangebot)
			{
				gehaltsvorstellung = gehaltsangebot;
			}
			if (stimmung < 5f)
			{
				stimmung = 0f;
			}
			uiObjects[26].GetComponent<Slider>().minValue = gehaltsangebot_min;
			uiObjects[26].GetComponent<Slider>().maxValue = gehaltsvorstellung - 1f;
			uiObjects[26].GetComponent<Slider>().value = gehaltsvorstellung + 1f;
			SetData();
		}
	}
}
