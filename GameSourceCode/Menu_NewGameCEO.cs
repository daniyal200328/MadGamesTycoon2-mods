using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_NewGameCEO : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private characterScript cS_;

	private GameObject character;

	private createCharScript cCS_;

	private clothScript cloth_;

	private bool sliderEvent = true;

	private float camRotate;

	public bool male = true;

	public int body = -2;

	public int hair = -2;

	public int eyes = -2;

	public int beard = -2;

	public int colorSkin = -2;

	public int colorHair = -2;

	public int colorShirt = -2;

	public int colorHose = -2;

	public int colorAdd1 = -2;

	public int beruf;

	public float s_skills;

	public float s_gamedesign;

	public float s_programmieren;

	public float s_grafik;

	public float s_sound;

	public float s_pr;

	public float s_gametests;

	public float s_technik;

	public float s_forschen;

	public bool[] perks;

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
			if (!cCS_)
			{
				cCS_ = main_.GetComponent<createCharScript>();
			}
			if (!cloth_)
			{
				cloth_ = main_.GetComponent<clothScript>();
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
	}

	private void Update()
	{
		if (s_skills > 50f)
		{
			s_skills = 50f;
			s_gamedesign = 15f;
			s_programmieren = 15f;
			s_grafik = 15f;
			s_sound = 15f;
			s_pr = 15f;
			s_gametests = 15f;
			s_technik = 15f;
			s_forschen = 15f;
		}
	}

	private void OnEnable()
	{
		FindScripts();
		Init();
	}

	public void Init()
	{
		InitDropdowns();
		if (uiObjects[12].GetComponent<InputField>().text.Length <= 0)
		{
			s_skills = 50f;
			s_gamedesign = 15f;
			s_programmieren = 15f;
			s_grafik = 15f;
			s_sound = 15f;
			s_pr = 15f;
			s_gametests = 15f;
			s_technik = 15f;
			s_forschen = 15f;
			LoadData();
			uiObjects[23].GetComponent<Dropdown>().value = beruf;
			InitSkills();
			InitSlider();
			CreateChar();
			BUTTON_Perk(0);
		}
		if (male)
		{
			uiObjects[9].GetComponent<Image>().color = guiMain_.colors[0];
			uiObjects[10].GetComponent<Image>().color = Color.white;
		}
		else
		{
			uiObjects[9].GetComponent<Image>().color = Color.white;
			uiObjects[10].GetComponent<Image>().color = guiMain_.colors[0];
		}
		if (mS_.multiplayer)
		{
			uiObjects[12].GetComponent<InputField>().interactable = false;
			uiObjects[12].GetComponent<InputField>().text = mS_.playerName;
		}
		else
		{
			uiObjects[12].GetComponent<InputField>().interactable = true;
		}
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		list.Add(tS_.GetText(137));
		list.Add(tS_.GetText(138));
		list.Add(tS_.GetText(139));
		list.Add(tS_.GetText(140));
		list.Add(tS_.GetText(141));
		list.Add(tS_.GetText(142));
		list.Add(tS_.GetText(143));
		list.Add(tS_.GetText(144));
		uiObjects[23].GetComponent<Dropdown>().ClearOptions();
		uiObjects[23].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[23].GetComponent<Dropdown>().value = beruf;
	}

	private void InitSkills()
	{
		SetBalken(uiObjects[13], s_gamedesign, 0);
		SetBalken(uiObjects[14], s_programmieren, 1);
		SetBalken(uiObjects[15], s_grafik, 2);
		SetBalken(uiObjects[16], s_sound, 3);
		SetBalken(uiObjects[17], s_pr, 4);
		SetBalken(uiObjects[18], s_gametests, 5);
		SetBalken(uiObjects[19], s_technik, 6);
		SetBalken(uiObjects[20], s_forschen, 7);
		string text = tS_.GetText(830);
		text = text.Replace("<NUM>", Mathf.RoundToInt(s_skills).ToString());
		uiObjects[21].GetComponent<Text>().text = text;
	}

	private void InitSlider()
	{
		sliderEvent = false;
		if (male)
		{
			uiObjects[0].GetComponent<Slider>().maxValue = cCS_.charGfxMales.Length - 1;
		}
		else
		{
			uiObjects[0].GetComponent<Slider>().maxValue = cCS_.charGfxFemales.Length - 1;
		}
		if (body == -2)
		{
			body = Mathf.RoundToInt(Random.Range(0f, uiObjects[0].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[0].GetComponent<Slider>().value = body;
		body = Mathf.RoundToInt(uiObjects[0].GetComponent<Slider>().value);
		if (male)
		{
			uiObjects[1].GetComponent<Slider>().maxValue = cloth_.prefabMaleHairs.Length - 1;
		}
		else
		{
			uiObjects[1].GetComponent<Slider>().maxValue = cloth_.prefabFemaleHairs.Length - 1;
		}
		if (hair == -2)
		{
			hair = Mathf.RoundToInt(Random.Range(0f, uiObjects[1].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[1].GetComponent<Slider>().value = hair;
		hair = Mathf.RoundToInt(uiObjects[1].GetComponent<Slider>().value);
		if (male)
		{
			uiObjects[2].GetComponent<Slider>().maxValue = cloth_.prefabMaleEyes.Length - 1;
		}
		else
		{
			uiObjects[2].GetComponent<Slider>().maxValue = cloth_.prefabFemaleEyes.Length - 1;
		}
		if (eyes == -2)
		{
			eyes = Mathf.RoundToInt(Random.Range(0f, uiObjects[2].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[2].GetComponent<Slider>().value = eyes;
		eyes = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
		if (male)
		{
			uiObjects[3].GetComponent<Slider>().interactable = true;
			uiObjects[3].GetComponent<Slider>().maxValue = cloth_.prefabBeards.Length - 1;
			if (beard == -2)
			{
				beard = Mathf.RoundToInt(Random.Range(0f, uiObjects[3].GetComponent<Slider>().maxValue - 1f));
			}
			uiObjects[3].GetComponent<Slider>().value = beard;
		}
		else
		{
			uiObjects[3].GetComponent<Slider>().interactable = false;
			uiObjects[3].GetComponent<Slider>().maxValue = 0f;
			beard = 0;
			uiObjects[3].GetComponent<Slider>().value = 0f;
		}
		uiObjects[4].GetComponent<Slider>().maxValue = cloth_.matColor_Skin.Length - 1;
		if (colorSkin == -2)
		{
			colorSkin = Mathf.RoundToInt(Random.Range(0f, uiObjects[4].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[4].GetComponent<Slider>().value = colorSkin;
		if (male)
		{
			uiObjects[5].GetComponent<Slider>().maxValue = cloth_.matColor_MaleHair.Length - 1;
		}
		else
		{
			uiObjects[5].GetComponent<Slider>().maxValue = cloth_.matColor_FemaleHair.Length - 1;
		}
		if (colorHair == -2)
		{
			colorHair = Mathf.RoundToInt(Random.Range(0f, uiObjects[5].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[5].GetComponent<Slider>().value = colorHair;
		colorHair = Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value);
		if (male)
		{
			uiObjects[6].GetComponent<Slider>().maxValue = cloth_.matColor_MaleShirt.Length - 1;
		}
		else
		{
			uiObjects[6].GetComponent<Slider>().maxValue = cloth_.matColor_FemaleShirt.Length - 1;
		}
		if (colorShirt == -2)
		{
			colorShirt = Mathf.RoundToInt(Random.Range(0f, uiObjects[6].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[6].GetComponent<Slider>().value = colorShirt;
		colorShirt = Mathf.RoundToInt(uiObjects[6].GetComponent<Slider>().value);
		if (male)
		{
			uiObjects[7].GetComponent<Slider>().maxValue = cloth_.matColor_MaleHose.Length - 1;
		}
		else
		{
			uiObjects[7].GetComponent<Slider>().maxValue = cloth_.matColor_FemaleHose.Length - 1;
		}
		if (colorHose == -2)
		{
			colorHose = Mathf.RoundToInt(Random.Range(0f, uiObjects[7].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[7].GetComponent<Slider>().value = colorHose;
		colorHose = Mathf.RoundToInt(uiObjects[7].GetComponent<Slider>().value);
		uiObjects[8].GetComponent<Slider>().maxValue = cloth_.matColor_AllColors.Length - 1;
		if (colorAdd1 == -2)
		{
			colorAdd1 = Mathf.RoundToInt(Random.Range(0f, uiObjects[8].GetComponent<Slider>().maxValue - 1f));
		}
		uiObjects[8].GetComponent<Slider>().value = colorAdd1;
		sliderEvent = true;
	}

	private void CreateChar()
	{
		if ((bool)character)
		{
			Object.Destroy(character);
		}
		cS_ = mS_.CreatePlayer(male, body, eyes, hair, beard, colorSkin, colorHair, colorHair, colorHose, colorShirt, colorAdd1);
		character = cS_.gameObject.transform.GetChild(0).gameObject;
		character.name = "CHARNEWGAME";
		character.transform.SetParent(null);
		character.transform.position = new Vector3(0f, 0f, 0f);
		character.transform.eulerAngles = new Vector3(0f, camRotate, 0f);
		SetLayer(4, character.transform);
		character.GetComponent<Animator>().CrossFade("idle", 0.1f, 0, 0f, 0.4f);
		Object.Destroy(cS_.gameObject);
	}

	private void SetLayer(int newLayer, Transform trans)
	{
		trans.gameObject.layer = newLayer;
		foreach (Transform tran in trans)
		{
			tran.gameObject.layer = newLayer;
			if (tran.childCount > 0)
			{
				SetLayer(newLayer, tran.transform);
			}
		}
	}

	public void SetBalken(GameObject go, float val, int beruf_)
	{
		go.transform.Find("Value").GetComponent<Text>().text = mS_.Round(val, 1).ToString();
		go.transform.Find("Fill").GetComponent<Image>().fillAmount = val * 0.01f;
		go.transform.Find("Fill").GetComponent<Image>().color = GetValColor(val);
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSkill100)
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
		}
		else if (beruf != beruf_)
		{
			if (perks[15])
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.6f;
			}
			else
			{
				go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 0.5f;
			}
		}
		else
		{
			go.transform.Find("FillMax").GetComponent<Image>().fillAmount = 1f;
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

	public void SLIDER_Body()
	{
		if (sliderEvent)
		{
			body = Mathf.RoundToInt(uiObjects[0].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_Hair()
	{
		if (sliderEvent)
		{
			hair = Mathf.RoundToInt(uiObjects[1].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_Eyes()
	{
		if (sliderEvent)
		{
			eyes = Mathf.RoundToInt(uiObjects[2].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_Beard()
	{
		if (sliderEvent)
		{
			beard = Mathf.RoundToInt(uiObjects[3].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_ColorSkin()
	{
		if (sliderEvent)
		{
			colorSkin = Mathf.RoundToInt(uiObjects[4].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_ColorHair()
	{
		if (sliderEvent)
		{
			colorHair = Mathf.RoundToInt(uiObjects[5].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_ColorShirt()
	{
		if (sliderEvent)
		{
			colorShirt = Mathf.RoundToInt(uiObjects[6].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_ColorHose()
	{
		if (sliderEvent)
		{
			colorHose = Mathf.RoundToInt(uiObjects[7].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_ColorAdd1()
	{
		if (sliderEvent)
		{
			colorAdd1 = Mathf.RoundToInt(uiObjects[8].GetComponent<Slider>().value);
			CreateChar();
		}
	}

	public void SLIDER_CamRotate()
	{
		camRotate = uiObjects[11].GetComponent<Slider>().value;
		if ((bool)character)
		{
			character.transform.eulerAngles = new Vector3(0f, camRotate, 0f);
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		if (mS_.multiplayer)
		{
			guiMain_.uiObjects[201].GetComponent<mpMain>().BUTTON_Close();
		}
	}

	public void BUTTON_OK()
	{
		sfx_.PlaySound(3, force: true);
		if (uiObjects[12].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(824), closeMenu: false);
			return;
		}
		if (s_skills > 0f)
		{
			guiMain_.MessageBox(tS_.GetText(831), closeMenu: false);
			return;
		}
		int num = 0;
		for (int i = 0; i < perks.Length; i++)
		{
			if (perks[i])
			{
				num++;
			}
		}
		if (num < 5)
		{
			guiMain_.MessageBox(tS_.GetText(1056), closeMenu: false);
			return;
		}
		SaveData();
		guiMain_.uiObjects[163].SetActive(value: true);
		if (mS_.multiplayer)
		{
			guiMain_.uiObjects[163].GetComponent<Menu_NewGameSettings>().BUTTON_OK();
		}
	}

	private void SaveData()
	{
		PlayerPrefs.SetString("PlayerName", uiObjects[12].GetComponent<InputField>().text);
		PlayerPrefs.SetInt("setup_beruf", beruf);
		PlayerPrefs.SetFloat("setup_s_skills", s_skills);
		PlayerPrefs.SetFloat("setup_s_gamedesign", s_gamedesign);
		PlayerPrefs.SetFloat("setup_s_programmieren", s_programmieren);
		PlayerPrefs.SetFloat("setup_s_grafik", s_grafik);
		PlayerPrefs.SetFloat("setup_s_sound", s_sound);
		PlayerPrefs.SetFloat("setup_s_pr", s_pr);
		PlayerPrefs.SetFloat("setup_s_gametests", s_gametests);
		PlayerPrefs.SetFloat("setup_s_technik", s_technik);
		PlayerPrefs.SetFloat("setup_s_forschen", s_forschen);
		for (int i = 0; i < perks.Length; i++)
		{
			if (perks[i])
			{
				PlayerPrefs.SetInt("setup_s_perks_" + i, 1);
			}
			else
			{
				PlayerPrefs.SetInt("setup_s_perks_" + i, 0);
			}
		}
		if (male)
		{
			PlayerPrefs.SetInt("setup_male", 1);
		}
		else
		{
			PlayerPrefs.SetInt("setup_male", 0);
		}
		PlayerPrefs.SetInt("setup_body", body);
		PlayerPrefs.SetInt("setup_hair", hair);
		PlayerPrefs.SetInt("setup_eyes", eyes);
		PlayerPrefs.SetInt("setup_beard", beard);
		PlayerPrefs.SetInt("setup_colorSkin", colorSkin);
		PlayerPrefs.SetInt("setup_colorHair", colorHair);
		PlayerPrefs.SetInt("setup_colorShirt", colorShirt);
		PlayerPrefs.SetInt("setup_colorHose", colorHose);
		PlayerPrefs.SetInt("setup_colorAdd1", colorAdd1);
	}

	private void LoadData()
	{
		uiObjects[12].GetComponent<InputField>().text = PlayerPrefs.GetString("PlayerName");
		if (PlayerPrefs.HasKey("setup_beruf"))
		{
			beruf = PlayerPrefs.GetInt("setup_beruf");
		}
		if (PlayerPrefs.HasKey("setup_s_skills"))
		{
			s_skills = PlayerPrefs.GetFloat("setup_s_skills");
			s_gamedesign = PlayerPrefs.GetFloat("setup_s_gamedesign");
			s_programmieren = PlayerPrefs.GetFloat("setup_s_programmieren");
			s_grafik = PlayerPrefs.GetFloat("setup_s_grafik");
			s_sound = PlayerPrefs.GetFloat("setup_s_sound");
			s_pr = PlayerPrefs.GetFloat("setup_s_pr");
			s_gametests = PlayerPrefs.GetFloat("setup_s_gametests");
			s_technik = PlayerPrefs.GetFloat("setup_s_technik");
			s_forschen = PlayerPrefs.GetFloat("setup_s_forschen");
		}
		if (PlayerPrefs.HasKey("setup_s_perks_0"))
		{
			for (int i = 0; i < perks.Length; i++)
			{
				if (PlayerPrefs.GetInt("setup_s_perks_" + i) > 0)
				{
					perks[i] = true;
				}
				else
				{
					perks[i] = false;
				}
			}
		}
		if (PlayerPrefs.HasKey("setup_male"))
		{
			if (PlayerPrefs.GetInt("setup_male") == 1)
			{
				male = true;
			}
			else
			{
				male = false;
			}
			body = PlayerPrefs.GetInt("setup_body");
			hair = PlayerPrefs.GetInt("setup_hair");
			eyes = PlayerPrefs.GetInt("setup_eyes");
			beard = PlayerPrefs.GetInt("setup_beard");
			colorSkin = PlayerPrefs.GetInt("setup_colorSkin");
			colorHair = PlayerPrefs.GetInt("setup_colorHair");
			colorShirt = PlayerPrefs.GetInt("setup_colorShirt");
			colorHose = PlayerPrefs.GetInt("setup_colorHose");
			colorAdd1 = PlayerPrefs.GetInt("setup_colorAdd1");
		}
	}

	public void BUTTON_Male()
	{
		sfx_.PlaySound(3, force: true);
		male = true;
		InitSlider();
		CreateChar();
		uiObjects[9].GetComponent<Image>().color = guiMain_.colors[0];
		uiObjects[10].GetComponent<Image>().color = Color.white;
	}

	public void BUTTON_Female()
	{
		sfx_.PlaySound(3, force: true);
		male = false;
		InitSlider();
		CreateChar();
		uiObjects[9].GetComponent<Image>().color = Color.white;
		uiObjects[10].GetComponent<Image>().color = guiMain_.colors[0];
	}

	public void BUTTON_Random()
	{
		sfx_.PlaySound(3, force: true);
		body = -2;
		hair = -2;
		eyes = -2;
		beard = -2;
		colorSkin = -2;
		colorHair = -2;
		colorShirt = -2;
		colorHose = -2;
		colorAdd1 = -2;
		if (male)
		{
			BUTTON_Male();
		}
		else
		{
			BUTTON_Female();
		}
	}

	private IEnumerator iAddStats(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_AddStats(i);
		}
	}

	public void BUTTON_AddStats(int i)
	{
		sfx_.PlaySound(3, force: true);
		if (s_skills <= 0f)
		{
			return;
		}
		switch (i)
		{
		case 0:
			if (s_gamedesign < 100f && beruf == 0)
			{
				s_gamedesign += 5f;
				s_skills -= 5f;
			}
			if (s_gamedesign < GetSkillCap() && beruf != 0)
			{
				s_gamedesign += 5f;
				s_skills -= 5f;
			}
			break;
		case 1:
			if (s_programmieren < 100f && beruf == 1)
			{
				s_programmieren += 5f;
				s_skills -= 5f;
			}
			if (s_programmieren < GetSkillCap() && beruf != 1)
			{
				s_programmieren += 5f;
				s_skills -= 5f;
			}
			break;
		case 2:
			if (s_grafik < 100f && beruf == 2)
			{
				s_grafik += 5f;
				s_skills -= 5f;
			}
			if (s_grafik < GetSkillCap() && beruf != 2)
			{
				s_grafik += 5f;
				s_skills -= 5f;
			}
			break;
		case 3:
			if (s_sound < 100f && beruf == 3)
			{
				s_sound += 5f;
				s_skills -= 5f;
			}
			if (s_sound < GetSkillCap() && beruf != 3)
			{
				s_sound += 5f;
				s_skills -= 5f;
			}
			break;
		case 4:
			if (s_pr < 100f && beruf == 4)
			{
				s_pr += 5f;
				s_skills -= 5f;
			}
			if (s_pr < GetSkillCap() && beruf != 4)
			{
				s_pr += 5f;
				s_skills -= 5f;
			}
			break;
		case 5:
			if (s_gametests < 100f && beruf == 5)
			{
				s_gametests += 5f;
				s_skills -= 5f;
			}
			if (s_gametests < GetSkillCap() && beruf != 5)
			{
				s_gametests += 5f;
				s_skills -= 5f;
			}
			break;
		case 6:
			if (s_technik < 100f && beruf == 6)
			{
				s_technik += 5f;
				s_skills -= 5f;
			}
			if (s_technik < GetSkillCap() && beruf != 6)
			{
				s_technik += 5f;
				s_skills -= 5f;
			}
			break;
		case 7:
			if (s_forschen < 100f && beruf == 7)
			{
				s_forschen += 5f;
				s_skills -= 5f;
			}
			if (s_forschen < GetSkillCap() && beruf != 7)
			{
				s_forschen += 5f;
				s_skills -= 5f;
			}
			break;
		}
		InitSkills();
		StartCoroutine(iAddStats(i));
	}

	private IEnumerator iMinusStats(int i)
	{
		yield return new WaitForSeconds(0.2f);
		if (Input.GetMouseButton(0))
		{
			BUTTON_MinusStats(i);
		}
	}

	public void BUTTON_MinusStats(int i)
	{
		sfx_.PlaySound(3, force: true);
		switch (i)
		{
		case 0:
			if (s_gamedesign > 15f)
			{
				s_gamedesign -= 5f;
				s_skills += 5f;
			}
			break;
		case 1:
			if (s_programmieren > 15f)
			{
				s_programmieren -= 5f;
				s_skills += 5f;
			}
			break;
		case 2:
			if (s_grafik > 15f)
			{
				s_grafik -= 5f;
				s_skills += 5f;
			}
			break;
		case 3:
			if (s_sound > 15f)
			{
				s_sound -= 5f;
				s_skills += 5f;
			}
			break;
		case 4:
			if (s_pr > 15f)
			{
				s_pr -= 5f;
				s_skills += 5f;
			}
			break;
		case 5:
			if (s_gametests > 15f)
			{
				s_gametests -= 5f;
				s_skills += 5f;
			}
			break;
		case 6:
			if (s_technik > 15f)
			{
				s_technik -= 5f;
				s_skills += 5f;
			}
			break;
		case 7:
			if (s_forschen > 15f)
			{
				s_forschen -= 5f;
				s_skills += 5f;
			}
			break;
		}
		InitSkills();
		StartCoroutine(iMinusStats(i));
	}

	public void BUTTON_RandomPerks()
	{
	}

	public void BUTTON_Perk(int i)
	{
		sfx_.PlaySound(3, force: true);
		perks[i] = !perks[i];
		perks[0] = true;
		int num = 0;
		for (int j = 0; j < perks.Length; j++)
		{
			if (perks[j])
			{
				if (uiObjects[24].transform.childCount > j)
				{
					uiObjects[24].transform.GetChild(j).GetComponent<Image>().color = guiMain_.colors[0];
					num++;
				}
			}
			else if (uiObjects[24].transform.childCount > j)
			{
				uiObjects[24].transform.GetChild(j).GetComponent<Image>().color = Color.white;
			}
		}
		string text = tS_.GetText(1682);
		text = text.Replace("<NUM>", (5 - num).ToString());
		uiObjects[25].GetComponent<Text>().text = text;
		if (num >= 5)
		{
			for (int k = 0; k < perks.Length; k++)
			{
				if (uiObjects[24].transform.childCount > k && !perks[k])
				{
					uiObjects[24].transform.GetChild(k).GetComponent<Button>().interactable = false;
				}
			}
		}
		else
		{
			for (int l = 0; l < uiObjects[24].transform.childCount; l++)
			{
				if (uiObjects[24].transform.childCount > l)
				{
					uiObjects[24].transform.GetChild(l).GetComponent<Button>().interactable = true;
				}
			}
		}
		DROPDOWN_Beruf();
		InitSkills();
	}

	private float GetSkillCap()
	{
		if (mS_.settings_sandbox && mS_.sandbox_mitarbeiterSkill100)
		{
			return 100f;
		}
		if (!perks[15])
		{
			return 50f;
		}
		return 60f;
	}

	public void DROPDOWN_Beruf()
	{
		sfx_.PlaySound(3, force: true);
		beruf = uiObjects[23].GetComponent<Dropdown>().value;
		if (s_gamedesign > GetSkillCap() && beruf != 0)
		{
			float f = s_gamedesign - GetSkillCap();
			s_gamedesign = GetSkillCap();
			s_skills += Mathf.RoundToInt(f);
		}
		if (s_programmieren > GetSkillCap() && beruf != 1)
		{
			float f2 = s_programmieren - GetSkillCap();
			s_programmieren = GetSkillCap();
			s_skills += Mathf.RoundToInt(f2);
		}
		if (s_grafik > GetSkillCap() && beruf != 2)
		{
			float f3 = s_grafik - GetSkillCap();
			s_grafik = GetSkillCap();
			s_skills += Mathf.RoundToInt(f3);
		}
		if (s_sound > GetSkillCap() && beruf != 3)
		{
			float f4 = s_sound - GetSkillCap();
			s_sound = GetSkillCap();
			s_skills += Mathf.RoundToInt(f4);
		}
		if (s_pr > GetSkillCap() && beruf != 4)
		{
			float f5 = s_pr - GetSkillCap();
			s_pr = GetSkillCap();
			s_skills += Mathf.RoundToInt(f5);
		}
		if (s_gametests > GetSkillCap() && beruf != 5)
		{
			float f6 = s_gametests - GetSkillCap();
			s_gametests = GetSkillCap();
			s_skills += Mathf.RoundToInt(f6);
		}
		if (s_technik > GetSkillCap() && beruf != 6)
		{
			float f7 = s_technik - GetSkillCap();
			s_technik = GetSkillCap();
			s_skills += Mathf.RoundToInt(f7);
		}
		if (s_forschen > GetSkillCap() && beruf != 7)
		{
			float f8 = s_forschen - GetSkillCap();
			s_forschen = GetSkillCap();
			s_skills += Mathf.RoundToInt(f8);
		}
		InitSkills();
	}
}
