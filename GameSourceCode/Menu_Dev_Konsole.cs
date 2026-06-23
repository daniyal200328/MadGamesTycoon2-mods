using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Dev_Konsole : MonoBehaviour
{
	public GameObject[] uiPrefabs;

	public GameObject[] uiObjects;

	public GameObject[] uiSides;

	private int seite;

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

	private forschungSonstiges forschungSonstiges_;

	private platforms platforms_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private roomScript rS_;

	public platformScript proScript_;

	public int platformTyp;

	public int anzController;

	public int gameID = -1;

	public int vorgaengerID = -1;

	public Vector3 consoleColor = new Vector3(0f, 0f, 0f);

	public int component_cpu = -1;

	public int component_gfx = -1;

	public int component_ram = -1;

	public int component_hdd = -1;

	public int component_sfx = -1;

	public int component_cooling = -1;

	public int component_disc = -1;

	public int component_controller = -1;

	public int component_case = -1;

	public int component_monitor = -1;

	public bool[] hwFeatures;

	public characterScript leitenderTechniker;

	public int[] platformCompatible = new int[4];

	private float hypeFromPro;

	private string searchStringA = "";

	private void Start()
	{
		FindScripts();
		uiObjects[51].GetComponent<Slider>().value = 128f;
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
		if (!forschungSonstiges_)
		{
			forschungSonstiges_ = main_.GetComponent<forschungSonstiges>();
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
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
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
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	private void InitConsoleColors()
	{
		if (uiObjects[2].transform.childCount > 0)
		{
			return;
		}
		consoleColor = new Vector3(0.5f, 0.5f, 0.5f);
		for (int i = 0; i < 13; i++)
		{
			for (int j = 3; j <= 10; j++)
			{
				Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[2].transform).GetComponent<Image>().color = new Color((float)i * 0.0384f, (float)j * 0.05f, 0.5f);
			}
		}
		for (int k = 0; k < 8; k++)
		{
			Object.Instantiate(uiPrefabs[1], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[2].transform).GetComponent<Image>().color = new Color(0f, 0f, (95f + (float)k * 5f) / 255f);
		}
	}

	public void InitPro(roomScript roomScript_, platformScript orgPlat_)
	{
		proScript_ = orgPlat_;
		InitConsoleColors();
		ClearData();
		platformTyp = orgPlat_.typ;
		FindScripts();
		rS_ = roomScript_;
		InitDropdowns();
		if (!uiObjects[97].activeSelf)
		{
			uiObjects[97].SetActive(value: true);
		}
		Init_KonsolenFeatures();
		uiObjects[4].GetComponent<InputField>().text = orgPlat_.GetName() + " " + tS_.GetText(2322);
		uiObjects[96].GetComponent<Button>().interactable = false;
		vorgaengerID = orgPlat_.vorgaengerID;
		uiObjects[53].GetComponent<Toggle>().interactable = false;
		uiObjects[53].GetComponent<Toggle>().isOn = orgPlat_.internet;
		hypeFromPro = orgPlat_.hype;
		component_cpu = orgPlat_.component_cpu;
		component_gfx = orgPlat_.component_gfx;
		component_ram = orgPlat_.component_ram;
		component_hdd = orgPlat_.component_hdd;
		component_sfx = orgPlat_.component_sfx;
		component_disc = orgPlat_.component_disc;
		component_case = orgPlat_.component_case;
		component_cooling = orgPlat_.component_cooling;
		component_controller = orgPlat_.component_controller;
		component_monitor = orgPlat_.component_monitor;
		anzController = orgPlat_.anzController;
		gameID = orgPlat_.gameID;
		if (platformTyp == 1)
		{
			uiObjects[23].SetActive(value: false);
			uiObjects[24].GetComponent<Button>().interactable = true;
			uiObjects[43].SetActive(value: false);
			uiObjects[44].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[23].SetActive(value: true);
			uiObjects[24].GetComponent<Button>().interactable = false;
			uiObjects[43].SetActive(value: true);
			uiObjects[44].GetComponent<Button>().interactable = false;
		}
		if (platformTyp == 2)
		{
			uiObjects[21].SetActive(value: false);
			uiObjects[22].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[21].SetActive(value: true);
			uiObjects[22].GetComponent<Button>().interactable = false;
		}
		if (platformTyp == 1)
		{
			uiObjects[46].GetComponent<Image>().sprite = guiMain_.uiSprites[42];
		}
		if (platformTyp == 2)
		{
			uiObjects[46].GetComponent<Image>().sprite = guiMain_.uiSprites[43];
		}
		if (platformTyp == 1)
		{
			uiObjects[94].GetComponent<Image>().sprite = platforms_.typSprites[1];
		}
		if (platformTyp == 2)
		{
			uiObjects[94].GetComponent<Image>().sprite = platforms_.typSprites[2];
		}
		uiObjects[61].GetComponent<Toggle>().isOn = orgPlat_.HasAbwaertkompatible();
		uiObjects[61].GetComponent<Toggle>().interactable = false;
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			SetPlatform(i, orgPlat_.platformCompatible[i]);
		}
		UpdateConsoleColor();
		SetLeitenderTechniker(null, manuellSelectet: false);
		UpdateGUI();
		OpenSide(0);
	}

	public void Init(roomScript roomScript_, int platformTyp_)
	{
		proScript_ = null;
		InitConsoleColors();
		if (platformTyp_ != platformTyp)
		{
			ClearData();
		}
		platformTyp = platformTyp_;
		FindScripts();
		rS_ = roomScript_;
		if (uiObjects[97].activeSelf)
		{
			uiObjects[97].SetActive(value: false);
		}
		InitDropdowns();
		Init_KonsolenFeatures();
		uiObjects[96].GetComponent<Button>().interactable = true;
		uiObjects[53].GetComponent<Toggle>().interactable = true;
		if (component_cpu == -1)
		{
			component_cpu = FindBestComponents(0);
		}
		if (component_gfx == -1)
		{
			component_gfx = FindBestComponents(1);
		}
		if (component_ram == -1)
		{
			component_ram = FindBestComponents(2);
		}
		if (component_hdd == -1)
		{
			component_hdd = FindBestComponents(3);
		}
		if (component_sfx == -1)
		{
			component_sfx = FindBestComponents(4);
		}
		if (component_disc == -1)
		{
			component_disc = FindBestComponents(6);
		}
		if (component_case == -1)
		{
			component_case = FindBestComponents(8);
		}
		if (platformTyp_ == 1)
		{
			if (component_cooling == -1)
			{
				component_cooling = FindBestComponents(5);
			}
			uiObjects[23].SetActive(value: false);
			uiObjects[24].GetComponent<Button>().interactable = true;
			if (component_controller == -1)
			{
				component_controller = FindBestComponents(7);
			}
			uiObjects[43].SetActive(value: false);
			uiObjects[44].GetComponent<Button>().interactable = true;
			if (anzController <= 0)
			{
				anzController = 1;
			}
		}
		else
		{
			component_cooling = -1;
			uiObjects[23].SetActive(value: true);
			uiObjects[24].GetComponent<Button>().interactable = false;
			component_controller = -1;
			uiObjects[43].SetActive(value: true);
			uiObjects[44].GetComponent<Button>().interactable = false;
		}
		if (platformTyp_ == 2)
		{
			if (component_monitor == -1)
			{
				component_monitor = FindBestComponents(9);
			}
			uiObjects[21].SetActive(value: false);
			uiObjects[22].GetComponent<Button>().interactable = true;
			anzController = 0;
		}
		else
		{
			component_monitor = -1;
			uiObjects[21].SetActive(value: true);
			uiObjects[22].GetComponent<Button>().interactable = false;
		}
		if (platformTyp_ == 1)
		{
			uiObjects[46].GetComponent<Image>().sprite = guiMain_.uiSprites[42];
		}
		if (platformTyp_ == 2)
		{
			uiObjects[46].GetComponent<Image>().sprite = guiMain_.uiSprites[43];
		}
		if (platformTyp_ == 1)
		{
			uiObjects[94].GetComponent<Image>().sprite = platforms_.typSprites[1];
		}
		if (platformTyp_ == 2)
		{
			uiObjects[94].GetComponent<Image>().sprite = platforms_.typSprites[2];
		}
		uiObjects[61].GetComponent<Toggle>().interactable = true;
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			SetPlatform(i, platformCompatible[i]);
		}
		UpdateConsoleColor();
		SetLeitenderTechniker(null, manuellSelectet: false);
		UpdateGUI();
		OpenSide(0);
	}

	private void ClearData()
	{
		anzController = 0;
		gameID = -1;
		vorgaengerID = -1;
		component_cpu = -1;
		component_gfx = -1;
		component_ram = -1;
		component_hdd = -1;
		component_sfx = -1;
		component_cooling = -1;
		component_disc = -1;
		component_controller = -1;
		component_case = -1;
		component_monitor = -1;
		leitenderTechniker = null;
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			hwFeatures[i] = false;
		}
		for (int j = 0; j < platformCompatible.Length; j++)
		{
			platformCompatible[j] = -1;
		}
	}

	public void UpdateGUI()
	{
		uiObjects[3].GetComponent<Image>().sprite = platforms_.typSprites[platformTyp];
		UpdateKomponenten();
		uiObjects[38].GetComponent<Text>().text = GetTechLevel().ToString();
		uiObjects[41].GetComponent<Text>().text = tS_.GetText(1612) + ": <b><color=blue>" + mS_.GetMoney(GetPerformance(), showDollar: false) + "</color></b>";
		if ((bool)proScript_)
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true) + " (30%)";
		}
		else
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		}
		if ((bool)proScript_)
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false) + " (30%)";
		}
		else
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false);
		}
		uiObjects[47].GetComponent<Image>().sprite = platforms_.complexSprites[GetKomplexitaet()];
		for (int i = 0; i < uiObjects[45].transform.childCount; i++)
		{
			if (i < anzController)
			{
				uiObjects[45].transform.GetChild(i).GetComponent<Image>().color = Color.white;
			}
			else
			{
				uiObjects[45].transform.GetChild(i).GetComponent<Image>().color = new Color(1f, 1f, 1f, 0.2f);
			}
		}
		uiObjects[49].GetComponent<Image>().sprite = hardware_.GetTypPic(component_case);
		if (vorgaengerID == -1)
		{
			uiObjects[95].GetComponent<Text>().text = tS_.GetText(2315);
		}
		else
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + vorgaengerID);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component)
				{
					uiObjects[95].GetComponent<Text>().text = component.GetName();
				}
			}
		}
		if (gameID != -1)
		{
			GameObject gameObject2 = GameObject.Find("GAME_" + gameID);
			if ((bool)gameObject2)
			{
				gameScript component2 = gameObject2.GetComponent<gameScript>();
				if ((bool)component2)
				{
					uiObjects[52].GetComponent<Text>().text = component2.GetNameWithTag();
				}
			}
		}
		else
		{
			uiObjects[52].GetComponent<Text>().text = tS_.GetText(1615);
		}
		if (hardwareFeatures_.IsErforscht(0) && hardwareFeatures_.hardFeat_UNLOCK[0])
		{
			uiObjects[58].SetActive(value: false);
		}
		else
		{
			uiObjects[58].SetActive(value: true);
			uiObjects[53].GetComponent<Toggle>().isOn = false;
		}
		if (hardwareFeatures_.IsErforscht(1) && hardwareFeatures_.hardFeat_UNLOCK[1])
		{
			uiObjects[60].SetActive(value: false);
		}
		else
		{
			uiObjects[60].SetActive(value: true);
			uiObjects[61].GetComponent<Toggle>().isOn = false;
		}
		TOGGLE_Abwaertskompatibel();
		UpdateConsoleColor();
	}

	public void SetPlatform(int slot, int platform_)
	{
		platformCompatible[slot] = platform_;
		if (platform_ >= 0)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + platform_);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				uiObjects[62 + slot].GetComponent<Text>().text = component.GetName();
				uiObjects[62 + slot].GetComponent<Text>().color = Color.white;
				component.SetPic(uiObjects[66 + slot]);
				uiObjects[66 + slot].SetActive(value: true);
				guiMain_.DrawStars(uiObjects[70 + slot], component.erfahrung);
				uiObjects[74 + slot].GetComponent<Text>().text = component.tech.ToString();
				uiObjects[78 + slot].GetComponent<Image>().sprite = component.GetComplexSprite();
				uiObjects[82 + slot].GetComponent<Image>().sprite = component.GetTypSprite();
				uiObjects[82 + slot].GetComponent<tooltip>().c = component.GetTypString();
				if (component.internet)
				{
					uiObjects[86 + slot].SetActive(value: true);
				}
				else
				{
					uiObjects[86 + slot].SetActive(value: false);
				}
				uiObjects[66 + slot].GetComponent<tooltip>().c = component.GetTooltip();
			}
		}
		else
		{
			uiObjects[62 + slot].GetComponent<Text>().text = tS_.GetText(625) + " " + (slot + 1);
			uiObjects[62 + slot].GetComponent<Text>().color = Color.white;
			uiObjects[66 + slot].GetComponent<Image>().sprite = null;
			uiObjects[66 + slot].SetActive(value: false);
			guiMain_.DrawStars(uiObjects[70 + slot], 0);
			uiObjects[74 + slot].GetComponent<Text>().text = "-";
			uiObjects[78 + slot].GetComponent<Image>().sprite = platforms_.complexSprites[0];
			uiObjects[82 + slot].GetComponent<Image>().sprite = guiMain_.uiSprites[3];
			uiObjects[82 + slot].GetComponent<tooltip>().c = "";
			uiObjects[86 + slot].SetActive(value: false);
		}
	}

	private int FindBestComponents(int compTyp)
	{
		int result = -1;
		int num = -1;
		for (int i = 0; i < hardware_.hardware_TYP.Length; i++)
		{
			if (hardware_.hardware_UNLOCK[i] && hardware_.hardware_RES_POINTS_LEFT[i] <= 0f && hardware_.hardware_TYP[i] == compTyp && ((platformTyp == 1 && !hardware_.hardware_ONLYHANDHELD[i]) || (platformTyp == 2 && !hardware_.hardware_ONLYSTATIONARY[i])) && num <= hardware_.hardware_RES_POINTS[i])
			{
				result = i;
				num = hardware_.hardware_RES_POINTS[i];
			}
		}
		return result;
	}

	private void UpdateKomponenten()
	{
		if (component_cpu != -1)
		{
			uiObjects[5].GetComponent<Text>().text = hardware_.GetName(component_cpu);
			uiObjects[6].GetComponent<Text>().text = hardware_.hardware_TECH[component_cpu].ToString();
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "";
			uiObjects[6].GetComponent<Text>().text = "";
		}
		if (component_gfx != -1)
		{
			uiObjects[9].GetComponent<Text>().text = hardware_.GetName(component_gfx);
			uiObjects[10].GetComponent<Text>().text = hardware_.hardware_TECH[component_gfx].ToString();
		}
		else
		{
			uiObjects[9].GetComponent<Text>().text = "";
			uiObjects[10].GetComponent<Text>().text = "";
		}
		if (component_ram != -1)
		{
			uiObjects[26].GetComponent<Text>().text = hardware_.GetName(component_ram);
			uiObjects[27].GetComponent<Text>().text = hardware_.hardware_TECH[component_ram].ToString();
		}
		else
		{
			uiObjects[26].GetComponent<Text>().text = "";
			uiObjects[27].GetComponent<Text>().text = "";
		}
		if (component_hdd != -1)
		{
			uiObjects[28].GetComponent<Text>().text = hardware_.GetName(component_hdd);
			uiObjects[29].GetComponent<Text>().text = hardware_.hardware_TECH[component_hdd].ToString();
		}
		else
		{
			uiObjects[28].GetComponent<Text>().text = "";
			uiObjects[29].GetComponent<Text>().text = "";
		}
		if (component_sfx != -1)
		{
			uiObjects[30].GetComponent<Text>().text = hardware_.GetName(component_sfx);
			uiObjects[31].GetComponent<Text>().text = hardware_.hardware_TECH[component_sfx].ToString();
		}
		else
		{
			uiObjects[30].GetComponent<Text>().text = "";
			uiObjects[31].GetComponent<Text>().text = "";
		}
		if (component_cooling != -1)
		{
			uiObjects[32].GetComponent<Text>().text = hardware_.GetName(component_cooling);
			uiObjects[33].GetComponent<Text>().text = hardware_.hardware_TECH[component_cooling].ToString();
		}
		else
		{
			uiObjects[32].GetComponent<Text>().text = "";
			uiObjects[33].GetComponent<Text>().text = "";
		}
		if (component_disc != -1)
		{
			uiObjects[34].GetComponent<Text>().text = hardware_.GetName(component_disc);
			uiObjects[35].GetComponent<Text>().text = hardware_.hardware_TECH[component_disc].ToString();
		}
		else
		{
			uiObjects[34].GetComponent<Text>().text = "";
			uiObjects[35].GetComponent<Text>().text = "";
		}
		if (component_monitor != -1)
		{
			uiObjects[36].GetComponent<Text>().text = hardware_.GetName(component_monitor);
			uiObjects[37].GetComponent<Text>().text = hardware_.hardware_TECH[component_monitor].ToString();
		}
		else
		{
			uiObjects[36].GetComponent<Text>().text = "";
			uiObjects[37].GetComponent<Text>().text = "";
		}
		if (component_controller != -1)
		{
			uiObjects[42].GetComponent<Text>().text = hardware_.GetName(component_controller);
		}
		else
		{
			uiObjects[42].GetComponent<Text>().text = "";
		}
		if (component_case != -1)
		{
			uiObjects[48].GetComponent<Text>().text = hardware_.GetName(component_case);
		}
		else
		{
			uiObjects[48].GetComponent<Text>().text = "";
		}
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[317]);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Platform(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[424]);
		guiMain_.uiObjects[424].GetComponent<Menu_Dev_KonsolePlatforms>().Init(i);
	}

	public void BUTTON_Vorgaenger()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[448]);
		guiMain_.uiObjects[448].GetComponent<Menu_Dev_KonsoleVorgaenger>().Init(platformTyp);
	}

	public void BUTTON_RandomName()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[4].GetComponent<InputField>().text = tS_.GetPlatformName();
	}

	public void BUTTON_Komponente(int i)
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[319]);
		guiMain_.uiObjects[319].GetComponent<Menu_Dev_KonsoleComponent>().Init(i, platformTyp);
	}

	public void BUTTON_Game()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[320]);
	}

	public void BUTTON_AnzahlController(int i)
	{
		sfx_.PlaySound(3, force: true);
		anzController += i;
		if (anzController < 1)
		{
			anzController = 1;
		}
		if (anzController > 4)
		{
			anzController = 4;
		}
		if (platformTyp == 2)
		{
			anzController = 0;
		}
		UpdateGUI();
	}

	public void BUTTON_Start()
	{
		sfx_.PlaySound(3, force: true);
		if (mS_.NotEnoughMoney(GetDevCosts()))
		{
			guiMain_.ShowNoMoney();
			return;
		}
		if (uiObjects[4].GetComponent<InputField>().text.Length <= 0)
		{
			guiMain_.MessageBox(tS_.GetText(1624), closeMenu: false);
			OpenSide(0);
			return;
		}
		platformScript platformScript2 = null;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				platformScript2 = array[i].GetComponent<platformScript>();
				if ((bool)platformScript2 && platformScript2.GetName() == uiObjects[4].GetComponent<InputField>().text)
				{
					guiMain_.MessageBox(tS_.GetText(1625), closeMenu: false);
					OpenSide(0);
					return;
				}
			}
		}
		if (uiObjects[61].GetComponent<Toggle>().isOn)
		{
			bool flag = false;
			for (int j = 0; j < platformCompatible.Length; j++)
			{
				if (platformCompatible[j] != -1)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				guiMain_.MessageBox(tS_.GetText(2148), closeMenu: false);
				OpenSide(2);
				return;
			}
		}
		if (uiObjects[61].GetComponent<Toggle>().isOn)
		{
			if (CheckTechLevelFromPlatforms())
			{
				guiMain_.MessageBox(tS_.GetText(2150), closeMenu: false);
				OpenSide(2);
				return;
			}
		}
		else
		{
			for (int k = 0; k < platformCompatible.Length; k++)
			{
				platformCompatible[k] = -1;
			}
		}
		if ((bool)proScript_)
		{
			int num = 0;
			int num2 = 0;
			for (int l = 2; l < hwFeatures.Length; l++)
			{
				if (proScript_.hwFeatures[l])
				{
					num++;
				}
				if (hwFeatures[l])
				{
					num2++;
				}
			}
			if (proScript_.performancePoints >= GetPerformance() && hardware_.GetPerformance(proScript_.component_case) >= hardware_.GetPerformance(component_case) && num >= num2)
			{
				guiMain_.MessageBox(tS_.GetText(2325), closeMenu: false);
				OpenSide(0);
				return;
			}
		}
		mS_.Pay(GetDevCosts(), 22);
		platformScript2 = null;
		platformScript2 = platforms_.CreatePlatform();
		platformScript2.myID = mS_.GetNewID();
		platformScript2.ownerID = mS_.myID;
		platformScript2.typ = platformTyp;
		platformScript2.myName = uiObjects[4].GetComponent<InputField>().text;
		platformScript2.tech = GetTechLevel();
		platformScript2.erfahrung = 5;
		platformScript2.isUnlocked = false;
		platformScript2.inBesitz = true;
		platformScript2.complex = GetKomplexitaet();
		platformScript2.internet = uiObjects[53].GetComponent<Toggle>().isOn;
		platformScript2.devPointsStart = GetWorkPoints();
		platformScript2.devPoints = platformScript2.devPointsStart;
		platformScript2.dev_costs = GetGameDevCosts();
		platformScript2.minGamePassGames = 5;
		platformScript2.gameID = gameID;
		platformScript2.anzController = anzController;
		platformScript2.consoleColor = consoleColor;
		platformScript2.component_cpu = component_cpu;
		platformScript2.component_gfx = component_gfx;
		platformScript2.component_ram = component_ram;
		platformScript2.component_hdd = component_hdd;
		platformScript2.component_sfx = component_sfx;
		platformScript2.component_cooling = component_cooling;
		platformScript2.component_disc = component_disc;
		platformScript2.component_controller = component_controller;
		platformScript2.component_case = component_case;
		platformScript2.component_monitor = component_monitor;
		platformScript2.entwicklungsKosten = GetDevCosts();
		platformScript2.performancePoints = GetPerformance();
		for (int m = 0; m < hwFeatures.Length; m++)
		{
			platformScript2.hwFeatures[m] = hwFeatures[m];
		}
		platformScript2.vorgaengerID = vorgaengerID;
		if (vorgaengerID != -1)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + vorgaengerID);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component)
				{
					component.nachfolgerID = platformScript2.myID;
				}
			}
		}
		if (uiObjects[53].GetComponent<Toggle>().isOn)
		{
			platformScript2.hwFeatures[0] = true;
		}
		if (uiObjects[61].GetComponent<Toggle>().isOn)
		{
			platformScript2.hwFeatures[1] = true;
			for (int n = 0; n < platformCompatible.Length; n++)
			{
				platformScript2.platformCompatible[n] = platformCompatible[n];
				platformScript2.FindMyPlatformsCompatible();
			}
		}
		if (component_controller != -1)
		{
			platformScript2.needFeatures[0] = hardware_.hardware_NEED1[component_controller];
			platformScript2.needFeatures[1] = hardware_.hardware_NEED2[component_controller];
			platformScript2.needFeatures[2] = -1;
		}
		if (component_monitor != -1)
		{
			platformScript2.needFeatures[0] = hardware_.hardware_NEED1[component_monitor];
			platformScript2.needFeatures[1] = hardware_.hardware_NEED2[component_monitor];
			platformScript2.needFeatures[2] = -1;
		}
		for (int num3 = 0; num3 < platformScript2.needFeatures.Length; num3++)
		{
			if (platformScript2.needFeatures[num3] == 0)
			{
				platformScript2.needFeatures[num3] = -1;
			}
		}
		platformScript2.Init();
		taskKonsole taskKonsole2 = guiMain_.AddTask_Konsole();
		taskKonsole2.Init(fromSavegame: false);
		taskKonsole2.konsoleID = platformScript2.myID;
		taskKonsole2.pS_ = platformScript2;
		if ((bool)proScript_)
		{
			proScript_.proVersion = true;
			platformScript2.proVersion = true;
			platformScript2.proName = proScript_.GetName();
			taskKonsole2.proKonsoleID = proScript_.myID;
			platformScript2.hype = hypeFromPro;
		}
		if ((bool)leitenderTechniker)
		{
			taskKonsole2.leitenderTechnikerID = leitenderTechniker.myID;
			taskKonsole2.techniker_ = leitenderTechniker;
		}
		GameObject gameObject2 = GameObject.Find("Room_" + rS_.myID);
		if ((bool)gameObject2)
		{
			gameObject2.GetComponent<roomScript>().taskID = taskKonsole2.myID;
		}
		ClearData();
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public bool CheckTechLevelFromPlatforms()
	{
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			if (platformCompatible[i] <= 0)
			{
				continue;
			}
			GameObject gameObject = GameObject.Find("PLATFORM_" + platformCompatible[i]);
			if ((bool)gameObject)
			{
				platformScript component = gameObject.GetComponent<platformScript>();
				if ((bool)component && component.tech > GetTechLevel())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void SLIDER_Color()
	{
	}

	public void SLIDER_Saturation()
	{
	}

	public void UpdateConsoleColor()
	{
		uiObjects[49].GetComponent<Image>().color = new Color(consoleColor.x, consoleColor.y, consoleColor.z);
		DisableAllShadowsOnColorButtons();
	}

	private void DisableAllShadowsOnColorButtons()
	{
		for (int i = 0; i < uiObjects[2].transform.childCount; i++)
		{
			if (uiObjects[2].transform.GetChild(i).transform.GetChild(0).gameObject.activeSelf)
			{
				uiObjects[2].transform.GetChild(i).transform.GetChild(0).gameObject.SetActive(value: false);
			}
		}
	}

	public void NextSide(int i)
	{
		seite += i;
		if (seite < 0)
		{
			seite = 0;
		}
		if (seite > 3)
		{
			seite = 3;
		}
		OpenSide(seite);
		sfx_.PlaySound(3, force: true);
	}

	public void OpenSide(int i)
	{
		sfx_.PlaySound(3, force: false);
		for (int j = 0; j < uiSides.Length; j++)
		{
			if (uiSides[j].activeSelf && j != i)
			{
				uiSides[j].SetActive(value: false);
			}
		}
		seite = i;
		for (int k = 0; k < uiObjects[0].transform.childCount; k++)
		{
			uiObjects[0].transform.GetChild(k).GetComponent<Image>().color = Color.white;
		}
		uiObjects[0].transform.GetChild(i).GetComponent<Image>().color = Color.grey;
		if (!uiSides[i].activeSelf)
		{
			uiSides[i].SetActive(value: true);
		}
	}

	public void SetGame(int id_)
	{
		gameID = id_;
		UpdateGUI();
	}

	public void SetVorgaenger(int id_)
	{
		vorgaengerID = id_;
		UpdateGUI();
	}

	public void SetComponent(int typ_, int id_)
	{
		switch (typ_)
		{
		case 0:
			component_cpu = id_;
			break;
		case 1:
			component_gfx = id_;
			break;
		case 2:
			component_ram = id_;
			break;
		case 3:
			component_hdd = id_;
			break;
		case 4:
			component_sfx = id_;
			break;
		case 5:
			component_cooling = id_;
			break;
		case 6:
			component_disc = id_;
			break;
		case 7:
			component_controller = id_;
			break;
		case 8:
			component_case = id_;
			break;
		case 9:
			component_monitor = id_;
			break;
		}
		UpdateGUI();
	}

	public int GetKomplexitaet()
	{
		float num = 0f;
		if (component_cpu != -1)
		{
			num += (float)hardware_.hardware_TECH[component_cpu];
		}
		if (component_gfx != -1)
		{
			num += (float)hardware_.hardware_TECH[component_gfx];
		}
		if (component_ram != -1)
		{
			num += (float)hardware_.hardware_TECH[component_ram];
		}
		if (component_hdd != -1)
		{
			num += (float)hardware_.hardware_TECH[component_hdd];
		}
		if (component_sfx != -1)
		{
			num += (float)hardware_.hardware_TECH[component_sfx];
		}
		if (component_cooling != -1)
		{
			num += (float)hardware_.hardware_TECH[component_cooling];
		}
		if (component_disc != -1)
		{
			num += (float)hardware_.hardware_TECH[component_disc];
		}
		if (component_monitor != -1)
		{
			num += (float)hardware_.hardware_TECH[component_monitor];
		}
		num /= 7f;
		int num2 = Mathf.RoundToInt(num);
		int num3 = 0;
		if (component_cpu != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_cpu]);
		}
		if (component_gfx != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_gfx]);
		}
		if (component_ram != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_ram]);
		}
		if (component_hdd != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_hdd]);
		}
		if (component_sfx != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_sfx]);
		}
		if (component_cooling != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_cooling]);
		}
		if (component_disc != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_disc]);
		}
		if (component_monitor != -1)
		{
			num3 += Mathf.Abs(num2 - hardware_.hardware_TECH[component_monitor]);
		}
		if (num3 >= 0 && num3 < 4)
		{
			return 0;
		}
		if (num3 >= 4 && num3 < 8)
		{
			return 1;
		}
		if (num3 >= 8)
		{
			return 2;
		}
		return 0;
	}

	public int GetTechLevel()
	{
		int num = 99;
		if (component_cpu != -1 && hardware_.hardware_TECH[component_cpu] < num)
		{
			num = hardware_.hardware_TECH[component_cpu];
		}
		if (component_gfx != -1 && hardware_.hardware_TECH[component_gfx] < num)
		{
			num = hardware_.hardware_TECH[component_gfx];
		}
		if (component_ram != -1 && hardware_.hardware_TECH[component_ram] < num)
		{
			num = hardware_.hardware_TECH[component_ram];
		}
		if (component_hdd != -1 && hardware_.hardware_TECH[component_hdd] < num)
		{
			num = hardware_.hardware_TECH[component_hdd];
		}
		if (component_sfx != -1 && hardware_.hardware_TECH[component_sfx] < num)
		{
			num = hardware_.hardware_TECH[component_sfx];
		}
		if (component_cooling != -1 && hardware_.hardware_TECH[component_cooling] < num)
		{
			num = hardware_.hardware_TECH[component_cooling];
		}
		if (component_disc != -1 && hardware_.hardware_TECH[component_disc] < num)
		{
			num = hardware_.hardware_TECH[component_disc];
		}
		if (component_monitor != -1 && hardware_.hardware_TECH[component_monitor] < num)
		{
			num = hardware_.hardware_TECH[component_monitor];
		}
		return num;
	}

	private int GetDevCosts()
	{
		int num = 0;
		if (component_cpu != -1)
		{
			num += hardware_.GetDevCosts(component_cpu);
		}
		if (component_gfx != -1)
		{
			num += hardware_.GetDevCosts(component_gfx);
		}
		if (component_ram != -1)
		{
			num += hardware_.GetDevCosts(component_ram);
		}
		if (component_hdd != -1)
		{
			num += hardware_.GetDevCosts(component_hdd);
		}
		if (component_sfx != -1)
		{
			num += hardware_.GetDevCosts(component_sfx);
		}
		if (component_cooling != -1)
		{
			num += hardware_.GetDevCosts(component_cooling);
		}
		if (component_disc != -1)
		{
			num += hardware_.GetDevCosts(component_disc);
		}
		if (component_controller != -1)
		{
			num += hardware_.GetDevCosts(component_controller);
		}
		if (component_case != -1)
		{
			num += hardware_.GetDevCosts(component_case);
		}
		if (component_monitor != -1)
		{
			num += hardware_.GetDevCosts(component_monitor);
		}
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			if (hwFeatures[i])
			{
				num += hardwareFeatures_.GetDevCosts(i);
			}
		}
		num += num * GetKomplexitaetVerteuerung();
		if ((bool)proScript_)
		{
			num /= 100;
			num *= 30;
		}
		return num;
	}

	public int GetKomplexitaetVerteuerung()
	{
		int result = 0;
		switch (GetKomplexitaet())
		{
		case 1:
			result = 2;
			break;
		case 2:
			result = 3;
			break;
		}
		return result;
	}

	private int GetWorkPoints()
	{
		int num = 3000;
		if (component_cpu != -1)
		{
			num += hardware_.GetWorkPoints(component_cpu);
		}
		if (component_gfx != -1)
		{
			num += hardware_.GetWorkPoints(component_gfx);
		}
		if (component_ram != -1)
		{
			num += hardware_.GetWorkPoints(component_ram);
		}
		if (component_hdd != -1)
		{
			num += hardware_.GetWorkPoints(component_hdd);
		}
		if (component_sfx != -1)
		{
			num += hardware_.GetWorkPoints(component_sfx);
		}
		if (component_cooling != -1)
		{
			num += hardware_.GetWorkPoints(component_cooling);
		}
		if (component_disc != -1)
		{
			num += hardware_.GetWorkPoints(component_disc);
		}
		if (component_controller != -1)
		{
			num += hardware_.GetWorkPoints(component_controller);
		}
		if (component_case != -1)
		{
			num += hardware_.GetWorkPoints(component_case);
		}
		if (component_monitor != -1)
		{
			num += hardware_.GetWorkPoints(component_monitor);
		}
		for (int i = 0; i < hwFeatures.Length; i++)
		{
			if (hwFeatures[i])
			{
				num += hardwareFeatures_.GetWorkPoints(i);
			}
		}
		num += num * GetKomplexitaetVerteuerung();
		if ((bool)proScript_)
		{
			num /= 100;
			num *= 30;
		}
		return num;
	}

	private int GetPerformance()
	{
		int num = 0;
		if (component_cpu != -1)
		{
			num += hardware_.GetPerformance(component_cpu);
		}
		if (component_gfx != -1)
		{
			num += hardware_.GetPerformance(component_gfx);
		}
		if (component_ram != -1)
		{
			num += hardware_.GetPerformance(component_ram);
		}
		if (component_hdd != -1)
		{
			num += hardware_.GetPerformance(component_hdd);
		}
		if (component_sfx != -1)
		{
			num += hardware_.GetPerformance(component_sfx);
		}
		if (component_cooling != -1)
		{
			num += hardware_.GetPerformance(component_cooling);
		}
		if (component_disc != -1)
		{
			num += hardware_.GetPerformance(component_disc);
		}
		_ = component_controller;
		_ = -1;
		_ = component_case;
		_ = -1;
		if (component_monitor != -1)
		{
			num += hardware_.GetPerformance(component_monitor);
		}
		return num;
	}

	public void InitDropdowns()
	{
		int value = PlayerPrefs.GetInt(uiObjects[54].name);
		List<string> list = new List<string>();
		list.Add(tS_.GetText(183));
		list.Add(tS_.GetText(6));
		uiObjects[54].GetComponent<Dropdown>().ClearOptions();
		uiObjects[54].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[54].GetComponent<Dropdown>().value = value;
	}

	private void Init_KonsolenFeatures()
	{
		FindScripts();
		if (hwFeatures.Length == 0)
		{
			hwFeatures = new bool[hardwareFeatures_.hardFeat_UNLOCK.Length];
		}
		for (int i = 0; i < uiObjects[55].transform.childCount; i++)
		{
			Object.Destroy(uiObjects[55].transform.GetChild(i).gameObject);
		}
		for (int j = 2; j < hardwareFeatures_.hardFeat_UNLOCK.Length; j++)
		{
			if (!hardwareFeatures_.hardFeat_UNLOCK[j] || !hardwareFeatures_.IsErforscht(j))
			{
				continue;
			}
			string text = hardwareFeatures_.GetName(j);
			text = text.ToLower();
			if (uiObjects[56].GetComponent<InputField>().text.Length <= 0 || text.Contains(searchStringA.ToLower()))
			{
				GameObject gameObject = Object.Instantiate(uiPrefabs[0], new Vector3(0f, 0f, 0f), Quaternion.identity, uiObjects[55].transform);
				Item_DevKonsole_HardwareFeature component = gameObject.GetComponent<Item_DevKonsole_HardwareFeature>();
				component.myID = j;
				component.hardwareFeatures_ = hardwareFeatures_;
				component.mS_ = mS_;
				component.tS_ = tS_;
				component.sfx_ = sfx_;
				component.guiMain_ = guiMain_;
				component.menu_ = this;
				if ((bool)proScript_ && proScript_.hwFeatures[j])
				{
					hwFeatures[j] = true;
					gameObject.GetComponent<Button>().interactable = false;
				}
			}
		}
		DROPDOWN_SortKonsoleneatures();
		guiMain_.KeinEintrag(uiObjects[55], uiObjects[57]);
	}

	public void DROPDOWN_SortKonsoleneatures()
	{
		int value = uiObjects[54].GetComponent<Dropdown>().value;
		PlayerPrefs.SetInt(uiObjects[54].name, value);
		int childCount = uiObjects[55].transform.childCount;
		for (int i = 0; i < childCount; i++)
		{
			GameObject gameObject = uiObjects[55].transform.GetChild(i).gameObject;
			if ((bool)gameObject)
			{
				Item_DevKonsole_HardwareFeature component = gameObject.GetComponent<Item_DevKonsole_HardwareFeature>();
				switch (value)
				{
				case 0:
					gameObject.name = hardwareFeatures_.GetName(component.myID);
					break;
				case 1:
					gameObject.name = hardwareFeatures_.GetDevCosts(component.myID).ToString();
					break;
				}
			}
		}
		if (value == 0)
		{
			mS_.SortChildrenByName(uiObjects[55]);
		}
		else
		{
			mS_.SortChildrenByFloat(uiObjects[55]);
		}
	}

	public void BUTTON_Search()
	{
		sfx_.PlaySound(3, force: true);
		if (base.gameObject.activeSelf)
		{
			for (int i = 0; i < uiObjects[55].transform.childCount; i++)
			{
				uiObjects[55].transform.GetChild(i).gameObject.SetActive(value: false);
			}
			searchStringA = uiObjects[56].GetComponent<InputField>().text;
			Init_KonsolenFeatures();
		}
	}

	public void TOGGLE_Internet()
	{
		if (hardwareFeatures_.IsErforscht(0))
		{
			Init_KonsolenFeatures();
		}
		bool isOn = uiObjects[53].GetComponent<Toggle>().isOn;
		hwFeatures[0] = isOn;
		if ((bool)proScript_)
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true) + " (30%)";
		}
		else
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		}
		if ((bool)proScript_)
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false) + " (30%)";
		}
		else
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false);
		}
	}

	public void TOGGLE_Abwaertskompatibel()
	{
		bool isOn = uiObjects[61].GetComponent<Toggle>().isOn;
		uiObjects[90].GetComponent<Button>().interactable = isOn;
		uiObjects[91].GetComponent<Button>().interactable = isOn;
		uiObjects[92].GetComponent<Button>().interactable = isOn;
		uiObjects[93].GetComponent<Button>().interactable = isOn;
		if ((bool)proScript_)
		{
			uiObjects[90].GetComponent<Button>().interactable = false;
			uiObjects[91].GetComponent<Button>().interactable = false;
			uiObjects[92].GetComponent<Button>().interactable = false;
			uiObjects[93].GetComponent<Button>().interactable = false;
		}
		if (!isOn)
		{
			for (int i = 0; i < platformCompatible.Length; i++)
			{
				SetPlatform(i, -1);
			}
		}
		hwFeatures[1] = isOn;
		if ((bool)proScript_)
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true) + " (30%)";
		}
		else
		{
			uiObjects[39].GetComponent<Text>().text = mS_.GetMoney(GetDevCosts(), showDollar: true);
		}
		if ((bool)proScript_)
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false) + " (30%)";
		}
		else
		{
			uiObjects[40].GetComponent<Text>().text = mS_.GetMoney(GetWorkPoints(), showDollar: false);
		}
	}

	public void BUTTON_AllFeatures()
	{
		sfx_.PlaySound(3, force: true);
		bool flag = false;
		for (int i = 2; i < hwFeatures.Length; i++)
		{
			if ((bool)proScript_)
			{
				if (hwFeatures[i] && !proScript_.hwFeatures[i])
				{
					flag = true;
					break;
				}
			}
			else if (hwFeatures[i])
			{
				flag = true;
				break;
			}
		}
		if (flag)
		{
			for (int j = 2; j < hwFeatures.Length; j++)
			{
				if (hwFeatures[j])
				{
					hwFeatures[j] = false;
				}
				if ((bool)proScript_ && proScript_.hwFeatures[j])
				{
					hwFeatures[j] = true;
				}
			}
			UpdateGUI();
		}
		else
		{
			for (int k = 0; k < uiObjects[55].transform.childCount; k++)
			{
				uiObjects[55].transform.GetChild(k).GetComponent<Item_DevKonsole_HardwareFeature>().BUTTON_Click();
			}
		}
	}

	public void SetLeitenderTechniker(characterScript charS_, bool manuellSelectet)
	{
		if ((bool)charS_ && charS_.roomID != rS_.myID)
		{
			charS_ = null;
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
					if (component.s_technik > num)
					{
						num = component.s_technik;
						charS_ = component;
					}
					if (rS_.leitenderTechniker == component.myID)
					{
						charS_ = component;
						break;
					}
				}
			}
		}
		if (!charS_)
		{
			uiObjects[59].GetComponent<Text>().text = "---";
			leitenderTechniker = null;
			rS_.leitenderTechniker = -1;
			return;
		}
		leitenderTechniker = charS_;
		if (rS_.leitenderTechniker != charS_.myID)
		{
			rS_.leitenderTechniker = -1;
		}
		if (manuellSelectet)
		{
			rS_.leitenderTechniker = charS_.myID;
		}
		uiObjects[59].GetComponent<Text>().text = charS_.myName;
	}

	public void BUTTON_AllePlattformen()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[117]);
	}

	public void BUTTON_KonsolenDetails()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[338]);
	}

	public void BUTTON_LeitenderEntwickler()
	{
		sfx_.PlaySound(3, force: true);
		guiMain_.ActivateMenu(guiMain_.uiObjects[324]);
		guiMain_.uiObjects[324].GetComponent<Menu_LeitenderTechniker>().Init(rS_);
	}

	private int GetGameDevCosts()
	{
		int num = 0;
		int num2 = 0;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.tech == GetTechLevel())
				{
					num++;
					num2 += component.GetDevCosts();
				}
			}
		}
		int num3 = 0;
		for (int j = 0; j < hwFeatures.Length; j++)
		{
			if (hwFeatures[j])
			{
				num3 += 1000;
			}
		}
		float num4 = num2 / num;
		num4 += (float)num3;
		return GetKomplexitaet() switch
		{
			0 => Mathf.RoundToInt(num4 * 0.7f) / 100 * 100, 
			1 => Mathf.RoundToInt(num4 * 1f) / 100 * 100, 
			2 => Mathf.RoundToInt(num4 * 1.3f) / 100 * 100, 
			_ => Mathf.RoundToInt(num4 * 1f) / 100 * 100, 
		};
	}

	public void BUTTON_RemovePlatform(int slot)
	{
		sfx_.PlaySound(3, force: true);
		if (!proScript_ || (proScript_.platformCompatible[0] == -1 && proScript_.platformCompatible[1] == -1 && proScript_.platformCompatible[2] == -1 && proScript_.platformCompatible[3] == -1))
		{
			SetPlatform(slot, -1);
		}
	}
}
