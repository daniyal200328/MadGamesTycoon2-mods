using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_ChangePlatform : MonoBehaviour
{
	public GameObject[] uiObjects;

	public int[] g_GamePlatform;

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

	private platforms platforms_;

	public gameScript gS_;

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
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
	}

	public void Init(gameScript game_)
	{
		FindScripts();
		gS_ = game_;
		gS_.FindMyPlatforms();
		string c = tS_.GetText(904) + "\n\n" + tS_.GetText(365) + "\n\n" + tS_.GetText(1695) + "\n\n" + tS_.GetText(905) + "\n\n" + tS_.GetText(1061) + "\n\n" + tS_.GetText(1062);
		uiObjects[54].GetComponent<tooltip>().c = c;
		uiObjects[28].GetComponent<Text>().text = tS_.GetText(635);
		if (gS_.retro)
		{
			uiObjects[28].GetComponent<Text>().text = "<color=red>" + tS_.GetText(907) + "</color>";
		}
		if (gS_.exklusiv)
		{
			uiObjects[28].GetComponent<Text>().text = "<color=red>" + tS_.GetText(1919) + "</color>";
		}
		uiObjects[20].GetComponent<Text>().text = gS_.GetNameWithTag();
		uiObjects[26].GetComponent<Text>().text = tS_.GetText(376) + ": " + GetEngineTechLevel();
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			SetPlatform(i, game_.gamePlatform[i], init: true);
		}
		uiObjects[27].GetComponent<Text>().text = gS_.GetPlatformTypString();
		uiObjects[29].GetComponent<Image>().sprite = gS_.GetPlatformTypSprite();
		Unlock(28, uiObjects[47], uiObjects[22]);
		Unlock(29, uiObjects[48], uiObjects[23]);
		Unlock(30, uiObjects[49], uiObjects[24]);
	}

	private void Update()
	{
		if (!gS_)
		{
			return;
		}
		if (gS_.retro || gS_.exklusiv || gS_.arcade)
		{
			uiObjects[21].GetComponent<Button>().interactable = false;
			uiObjects[22].GetComponent<Button>().interactable = false;
			uiObjects[23].GetComponent<Button>().interactable = false;
			uiObjects[24].GetComponent<Button>().interactable = false;
		}
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] <= 0)
			{
				continue;
			}
			if (!gS_.gamePlatformScript[i])
			{
				gS_.FindMyPlatforms();
			}
			if ((bool)gS_.gamePlatformScript[i])
			{
				if (gS_.gamePlatformScript[i].vomMarktGenommen)
				{
					uiObjects[21 + i].GetComponent<Button>().interactable = true;
				}
				else
				{
					uiObjects[21 + i].GetComponent<Button>().interactable = false;
				}
			}
		}
	}

	private void Unlock(int id_, GameObject lock_, GameObject button_)
	{
		if (unlock_.unlock[id_])
		{
			button_.GetComponent<Button>().interactable = true;
			if ((bool)lock_)
			{
				lock_.SetActive(value: false);
			}
		}
		else
		{
			button_.GetComponent<Button>().interactable = false;
			if ((bool)lock_)
			{
				lock_.SetActive(value: true);
			}
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
		guiMain_.CloseMenu();
	}

	public void BUTTON_Ok()
	{
		bool flag = true;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component && !component.internet)
				{
					flag = false;
					break;
				}
			}
		}
		if (!flag)
		{
			string text = "";
			for (int j = 0; j < gS_.gameGameplayFeatures.Length; j++)
			{
				if (gS_.gameGameplayFeatures[j] && gF_.gameplayFeatures_INTERNET[j])
				{
					text = text + "\n<color=blue>" + gF_.GetName(j) + "</color>";
				}
			}
			if (text.Length > 0)
			{
				guiMain_.MessageBox(tS_.GetText(2198) + text, closeMenu: false);
				return;
			}
		}
		string text2 = "";
		for (int k = 0; k < g_GamePlatform.Length; k++)
		{
			if (g_GamePlatform[k] == -1)
			{
				continue;
			}
			GameObject gameObject2 = GameObject.Find("PLATFORM_" + g_GamePlatform[k]);
			if (!gameObject2)
			{
				continue;
			}
			platformScript component2 = gameObject2.GetComponent<platformScript>();
			if ((bool)component2)
			{
				if (component2.needFeatures[0] != -1 && !gS_.gameGameplayFeatures[component2.needFeatures[0]])
				{
					text2 = text2 + component2.GetName() + ": " + gF_.GetName(component2.needFeatures[0]) + "\n";
				}
				if (component2.needFeatures[1] != -1 && !gS_.gameGameplayFeatures[component2.needFeatures[1]])
				{
					text2 = text2 + component2.GetName() + ": " + gF_.GetName(component2.needFeatures[1]) + "\n";
				}
				if (component2.needFeatures[2] != -1 && !gS_.gameGameplayFeatures[component2.needFeatures[2]])
				{
					text2 = text2 + component2.GetName() + ": " + gF_.GetName(component2.needFeatures[2]) + "\n";
				}
				if ((gS_.gameTyp == 1 || gS_.gameTyp == 2) && !component2.internet)
				{
					guiMain_.MessageBox(tS_.GetText(1262), closeMenu: false);
					return;
				}
			}
		}
		if (text2.Length > 0)
		{
			guiMain_.MessageBox(tS_.GetText(1020) + "\n<color=blue>" + text2 + "</color>", closeMenu: false);
			return;
		}
		int num = CalcDevCosts();
		mS_.Pay(num, 10);
		gS_.costs_entwicklung += num;
		for (int l = 0; l < g_GamePlatform.Length; l++)
		{
			gS_.gamePlatform[l] = g_GamePlatform[l];
		}
		BUTTON_Close();
	}

	public void BUTTON_PlatformKaufen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[33]);
	}

	public void BUTTON_Platform(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[66]);
		guiMain_.uiObjects[66].GetComponent<Menu_DevGame_Platform>().Init(i);
	}

	public void BUTTON_RemovePlatform(int slot)
	{
		sfx_.PlaySound(3, force: true);
		if ((slot != 0 || uiObjects[21].GetComponent<Button>().interactable) && (slot != 1 || uiObjects[22].GetComponent<Button>().interactable) && (slot != 2 || uiObjects[23].GetComponent<Button>().interactable) && (slot != 3 || uiObjects[24].GetComponent<Button>().interactable))
		{
			SetPlatform(slot, -1, init: false);
		}
	}

	public void SetPlatform(int slot, int platform_, bool init)
	{
		g_GamePlatform[slot] = platform_;
		if (platform_ >= 0)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + platform_);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				uiObjects[slot].GetComponent<Text>().text = component.GetName();
				component.SetPic(uiObjects[4 + slot]);
				uiObjects[4 + slot].SetActive(value: true);
				guiMain_.DrawStars(uiObjects[8 + slot], component.erfahrung);
				uiObjects[12 + slot].GetComponent<Text>().text = component.tech.ToString();
				uiObjects[16 + slot].GetComponent<Text>().text = component.GetMarktanteilString();
				if (component.ownerID == mS_.myID && !component.isUnlocked)
				{
					uiObjects[16 + slot].GetComponent<Text>().text = "<color=red>[" + tS_.GetText(528) + "]</color>";
				}
				uiObjects[30 + slot].GetComponent<Image>().sprite = component.GetComplexSprite();
				uiObjects[38 + slot].GetComponent<Image>().sprite = component.GetTypSprite();
				uiObjects[38 + slot].GetComponent<tooltip>().c = component.GetTypString();
				if (component.internet)
				{
					uiObjects[34 + slot].SetActive(value: true);
				}
				else
				{
					uiObjects[34 + slot].SetActive(value: false);
				}
				uiObjects[4 + slot].GetComponent<tooltip>().c = component.GetTooltip();
				if (init)
				{
					if (!component.vomMarktGenommen)
					{
						uiObjects[21 + slot].GetComponent<Button>().interactable = false;
					}
					else
					{
						uiObjects[21 + slot].GetComponent<Button>().interactable = true;
					}
				}
			}
			else
			{
				uiObjects[slot].GetComponent<Text>().text = tS_.GetText(360 + slot);
				uiObjects[4 + slot].GetComponent<Image>().sprite = null;
				uiObjects[4 + slot].SetActive(value: false);
				guiMain_.DrawStars(uiObjects[8 + slot], 0);
				uiObjects[12 + slot].GetComponent<Text>().text = "-";
				uiObjects[16 + slot].GetComponent<Text>().text = "";
				uiObjects[30 + slot].GetComponent<Image>().sprite = platforms_.complexSprites[0];
				uiObjects[38 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
				uiObjects[38 + slot].GetComponent<tooltip>().c = "";
				uiObjects[34 + slot].SetActive(value: false);
				uiObjects[4 + slot].GetComponent<tooltip>().c = "";
				if (init)
				{
					uiObjects[21 + slot].GetComponent<Button>().interactable = true;
				}
			}
		}
		else
		{
			uiObjects[slot].GetComponent<Text>().text = tS_.GetText(360 + slot);
			uiObjects[4 + slot].GetComponent<Image>().sprite = null;
			uiObjects[4 + slot].SetActive(value: false);
			guiMain_.DrawStars(uiObjects[8 + slot], 0);
			uiObjects[12 + slot].GetComponent<Text>().text = "-";
			uiObjects[16 + slot].GetComponent<Text>().text = "";
			uiObjects[30 + slot].GetComponent<Image>().sprite = platforms_.complexSprites[0];
			uiObjects[38 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[38 + slot].GetComponent<tooltip>().c = "";
			uiObjects[34 + slot].SetActive(value: false);
			if (init)
			{
				uiObjects[21 + slot].GetComponent<Button>().interactable = true;
			}
			if (slot == 0)
			{
				uiObjects[21 + slot].GetComponent<Button>().interactable = true;
			}
		}
		uiObjects[42].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(0), 1) + "%";
		uiObjects[43].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(1), 1) + "%";
		uiObjects[44].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(2), 1) + "%";
		uiObjects[45].GetComponent<Text>().text = mS_.Round(GetGesamtMarktanteil(3), 1) + "%";
		long num = 0L;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] >= 0)
			{
				GameObject gameObject2 = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
				if ((bool)gameObject2)
				{
					platformScript component2 = gameObject2.GetComponent<platformScript>();
					num += component2.GetAktiveNutzer();
				}
			}
		}
		uiObjects[46].GetComponent<Text>().text = mS_.Round((float)num / 1000000f, 1) + " " + tS_.GetText(1483);
		uiObjects[25].GetComponent<Text>().text = mS_.GetMoney(CalcDevCosts(), showDollar: true);
	}

	public float GetGesamtMarktanteil(int platformTyp)
	{
		if (!gS_)
		{
			return 0f;
		}
		FindScripts();
		float num = 0f;
		for (int i = 0; i < gS_.gamePlatform.Length; i++)
		{
			if (g_GamePlatform[i] == -1)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if (component.typ == platformTyp)
				{
					num += component.GetMarktanteil();
				}
			}
		}
		return num;
	}

	public int GetEngineTechLevel()
	{
		gS_.FindMyEngineNew();
		if ((bool)gS_.engineS_)
		{
			return gS_.engineS_.GetTechLevel();
		}
		return 0;
	}

	private int CalcDevCosts()
	{
		int num = 0;
		for (int i = 0; i < g_GamePlatform.Length; i++)
		{
			if (gS_.gamePlatform[i] != g_GamePlatform[i] && g_GamePlatform[i] != -1)
			{
				GameObject gameObject = GameObject.Find("PLATFORM_" + g_GamePlatform[i]);
				if ((bool)gameObject)
				{
					num += gameObject.GetComponent<platformScript>().GetDevCosts();
				}
			}
		}
		return num;
	}
}
