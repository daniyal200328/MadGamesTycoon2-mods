using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Options : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject mainCanvas;

	private mpCalls mpCalls_;

	private mainScript mS_;

	private GameObject main_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private textScript tS_;

	private settingsScript settings_;

	private int TAB;

	private Resolution[] resolutions;

	public List<Resolution> resFilter = new List<Resolution>();

	private void Awake()
	{
		resolutions = Screen.resolutions;
		for (int i = 0; i < resolutions.Length; i++)
		{
			if (resolutions[i].width < 1024)
			{
				continue;
			}
			if (resFilter.Count > 0)
			{
				if (resolutions[i].width != resFilter[resFilter.Count - 1].width || resolutions[i].height != resFilter[resFilter.Count - 1].height)
				{
					resFilter.Add(resolutions[i]);
				}
			}
			else
			{
				resFilter.Add(resolutions[i]);
			}
		}
	}

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
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	private void Update()
	{
		if (uiObjects[2].GetComponent<Animation>().IsPlaying("openMenu"))
		{
			uiObjects[3].GetComponent<Scrollbar>().value = 1f;
		}
	}

	private void OnEnable()
	{
		FindScripts();
		InitDropdowns();
		Init();
		TAB_Settings(0);
	}

	private void Init()
	{
		if (settings_.masterVolume <= 0f)
		{
			uiObjects[8].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[8].GetComponent<Toggle>().isOn = true;
		}
		if (settings_.musicVolume <= 0f)
		{
			uiObjects[9].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[9].GetComponent<Toggle>().isOn = true;
		}
		uiObjects[10].GetComponent<Slider>().value = settings_.masterVolume;
		uiObjects[11].GetComponent<Slider>().value = settings_.musicVolume;
		uiObjects[13].GetComponent<Toggle>().isOn = settings_.vollbild;
		int num = 0;
		foreach (Resolution item in resFilter)
		{
			if (item.width == settings_.screenX && item.height == settings_.screenY)
			{
				uiObjects[12].GetComponent<Dropdown>().value = num;
				break;
			}
			num++;
		}
		if (settings_.vSync > 0)
		{
			uiObjects[14].GetComponent<Toggle>().isOn = true;
		}
		else
		{
			uiObjects[14].GetComponent<Toggle>().isOn = false;
		}
		switch (settings_.framerate)
		{
		case 30:
			uiObjects[15].GetComponent<Dropdown>().value = 0;
			break;
		case 60:
			uiObjects[15].GetComponent<Dropdown>().value = 1;
			break;
		case 90:
			uiObjects[15].GetComponent<Dropdown>().value = 2;
			break;
		case 120:
			uiObjects[15].GetComponent<Dropdown>().value = 3;
			break;
		case 144:
			uiObjects[15].GetComponent<Dropdown>().value = 4;
			break;
		case 160:
			uiObjects[15].GetComponent<Dropdown>().value = 5;
			break;
		case 165:
			uiObjects[15].GetComponent<Dropdown>().value = 6;
			break;
		case 180:
			uiObjects[15].GetComponent<Dropdown>().value = 7;
			break;
		case 200:
			uiObjects[15].GetComponent<Dropdown>().value = 8;
			break;
		case 240:
			uiObjects[15].GetComponent<Dropdown>().value = 9;
			break;
		}
		uiObjects[1].GetComponent<Dropdown>().value = settings_.fullScreenMode;
		uiObjects[16].GetComponent<Toggle>().isOn = settings_.shadows;
		uiObjects[17].GetComponent<Toggle>().isOn = settings_.SSAO;
		uiObjects[18].GetComponent<Toggle>().isOn = settings_.screenSpaceReflections;
		uiObjects[19].GetComponent<Toggle>().isOn = settings_.bloom;
		uiObjects[20].GetComponent<Toggle>().isOn = settings_.ambientOcclusion;
		uiObjects[21].GetComponent<Toggle>().isOn = settings_.colorGrading;
		uiObjects[22].GetComponent<Dropdown>().value = settings_.language;
		uiObjects[23].GetComponent<Slider>().value = settings_.uiScale;
		uiObjects[24].GetComponent<Toggle>().isOn = settings_.roomConnections;
		uiObjects[25].GetComponent<Toggle>().isOn = settings_.pauseUI;
		uiObjects[26].GetComponent<Toggle>().isOn = settings_.leaderboard;
		uiObjects[27].GetComponent<Toggle>().isOn = settings_.chat;
		uiObjects[28].GetComponent<Toggle>().isOn = settings_.disableWorkIconSound;
		uiObjects[29].GetComponent<Toggle>().isOn = settings_.sprechblasen;
		uiObjects[31].GetComponent<Toggle>().isOn = settings_.scrollScreen;
		uiObjects[32].GetComponent<Toggle>().isOn = settings_.disableEngineAbrechnung;
		uiObjects[33].GetComponent<Toggle>().isOn = settings_.disableWorkIcons;
		uiObjects[34].GetComponent<Toggle>().isOn = settings_.disableArbeiterBeschwerden;
		uiObjects[35].GetComponent<Toggle>().isOn = settings_.disableWetter;
		uiObjects[36].GetComponent<Toggle>().isOn = settings_.singleplayerPause;
		uiObjects[37].GetComponent<Slider>().value = settings_.fanletterTime;
		uiObjects[38].GetComponent<Toggle>().isOn = settings_.gameTabData;
		uiObjects[39].GetComponent<Toggle>().isOn = mS_.settings_autoPauseForMultiplayer;
		uiObjects[40].GetComponent<Dropdown>().value = settings_.saveInterval;
		uiObjects[41].GetComponent<Slider>().value = settings_.newsTime;
		uiObjects[42].GetComponent<Slider>().value = settings_.helligkeit;
		uiObjects[43].GetComponent<Toggle>().isOn = settings_.dontAsk_TaskAbbrechen;
		uiObjects[44].GetComponent<Toggle>().isOn = settings_.middleMouseClose;
		uiObjects[45].GetComponent<Toggle>().isOn = settings_.camera90GradRotation;
		uiObjects[46].GetComponent<Toggle>().isOn = settings_.hideConvention;
		uiObjects[47].GetComponent<Toggle>().isOn = settings_.hideAwards;
		uiObjects[48].GetComponent<Toggle>().isOn = settings_.hideEvents;
		uiObjects[49].GetComponent<Toggle>().isOn = settings_.disableTochterfirmaAbrechnung;
		uiObjects[50].GetComponent<Toggle>().isOn = settings_.hideKuendigungen;
		uiObjects[51].GetComponent<Toggle>().isOn = settings_.tochtefirmaTAG;
		uiObjects[52].GetComponent<Toggle>().isOn = settings_.returnNullBugs;
		uiObjects[53].GetComponent<Toggle>().isOn = settings_.showMarktforschung;
		uiObjects[54].GetComponent<Toggle>().isOn = settings_.advancedPathfinding;
		uiObjects[55].GetComponent<Toggle>().isOn = settings_.moneyPop;
		uiObjects[56].GetComponent<Slider>().value = settings_.beschwerdeTime;
		uiObjects[57].GetComponent<Toggle>().isOn = settings_.disableRobotSound;
		uiObjects[58].GetComponent<Toggle>().isOn = settings_.disableMitarbeiterIcons;
		uiObjects[59].GetComponent<Dropdown>().value = settings_.keyboard;
		uiObjects[60].GetComponent<Toggle>().isOn = settings_.taskComplete;
		uiObjects[61].GetComponent<Toggle>().isOn = settings_.hideJahresabrechnung;
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		foreach (Resolution item in resFilter)
		{
			list.Add(item.width + "x" + item.height);
		}
		uiObjects[12].GetComponent<Dropdown>().ClearOptions();
		uiObjects[12].GetComponent<Dropdown>().AddOptions(list);
		list.Clear();
		list.Add(tS_.GetText(1395));
		list.Add(tS_.GetText(1396));
		list.Add(tS_.GetText(1397));
		uiObjects[1].GetComponent<Dropdown>().ClearOptions();
		uiObjects[1].GetComponent<Dropdown>().AddOptions(list);
		list.Clear();
		list.Add(tS_.GetText(1449));
		list.Add(tS_.GetText(1450));
		list.Add(tS_.GetText(1451));
		list.Add(tS_.GetText(1452));
		list.Add(tS_.GetText(2194));
		list.Add(tS_.GetText(2407));
		uiObjects[40].GetComponent<Dropdown>().ClearOptions();
		uiObjects[40].GetComponent<Dropdown>().AddOptions(list);
	}

	private void Init(bool inBesitz)
	{
		FindScripts();
	}

	public void BUTTON_Close()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void DROPDOWN_Resolution()
	{
	}

	public void BUTTON_OK()
	{
		settings_.masterVolume = uiObjects[10].GetComponent<Slider>().value;
		settings_.musicVolume = uiObjects[11].GetComponent<Slider>().value;
		if (!uiObjects[8].GetComponent<Toggle>().isOn)
		{
			settings_.masterVolume = 0f;
		}
		else if (settings_.masterVolume <= 0f)
		{
			settings_.masterVolume = 0.01f;
		}
		if (!uiObjects[9].GetComponent<Toggle>().isOn)
		{
			settings_.musicVolume = 0f;
		}
		else if (settings_.musicVolume <= 0f)
		{
			settings_.musicVolume = 0.01f;
		}
		settings_.vollbild = uiObjects[13].GetComponent<Toggle>().isOn;
		settings_.uiScale = uiObjects[23].GetComponent<Slider>().value;
		if (settings_.screenX != resFilter[uiObjects[12].GetComponent<Dropdown>().value].width || settings_.screenY != resFilter[uiObjects[12].GetComponent<Dropdown>().value].height)
		{
			settings_.SetAutomaticGuiScale(resFilter[uiObjects[12].GetComponent<Dropdown>().value].width);
		}
		settings_.screenX = resFilter[uiObjects[12].GetComponent<Dropdown>().value].width;
		settings_.screenY = resFilter[uiObjects[12].GetComponent<Dropdown>().value].height;
		if (uiObjects[14].GetComponent<Toggle>().isOn)
		{
			settings_.vSync = 1;
		}
		else
		{
			settings_.vSync = 0;
		}
		switch (uiObjects[15].GetComponent<Dropdown>().value)
		{
		case 0:
			settings_.framerate = 30;
			break;
		case 1:
			settings_.framerate = 60;
			break;
		case 2:
			settings_.framerate = 90;
			break;
		case 3:
			settings_.framerate = 120;
			break;
		case 4:
			settings_.framerate = 144;
			break;
		case 5:
			settings_.framerate = 160;
			break;
		case 6:
			settings_.framerate = 165;
			break;
		case 7:
			settings_.framerate = 180;
			break;
		case 8:
			settings_.framerate = 200;
			break;
		case 9:
			settings_.framerate = 240;
			break;
		}
		settings_.fullScreenMode = uiObjects[1].GetComponent<Dropdown>().value;
		settings_.shadows = uiObjects[16].GetComponent<Toggle>().isOn;
		settings_.SSAO = uiObjects[17].GetComponent<Toggle>().isOn;
		settings_.screenSpaceReflections = uiObjects[18].GetComponent<Toggle>().isOn;
		settings_.bloom = uiObjects[19].GetComponent<Toggle>().isOn;
		settings_.ambientOcclusion = uiObjects[20].GetComponent<Toggle>().isOn;
		settings_.colorGrading = uiObjects[21].GetComponent<Toggle>().isOn;
		settings_.language = uiObjects[22].GetComponent<Dropdown>().value;
		settings_.roomConnections = uiObjects[24].GetComponent<Toggle>().isOn;
		settings_.pauseUI = uiObjects[25].GetComponent<Toggle>().isOn;
		settings_.leaderboard = uiObjects[26].GetComponent<Toggle>().isOn;
		settings_.chat = uiObjects[27].GetComponent<Toggle>().isOn;
		settings_.disableWorkIconSound = uiObjects[28].GetComponent<Toggle>().isOn;
		settings_.sprechblasen = uiObjects[29].GetComponent<Toggle>().isOn;
		settings_.scrollScreen = uiObjects[31].GetComponent<Toggle>().isOn;
		settings_.disableEngineAbrechnung = uiObjects[32].GetComponent<Toggle>().isOn;
		settings_.disableWorkIcons = uiObjects[33].GetComponent<Toggle>().isOn;
		settings_.disableArbeiterBeschwerden = uiObjects[34].GetComponent<Toggle>().isOn;
		settings_.disableWetter = uiObjects[35].GetComponent<Toggle>().isOn;
		settings_.singleplayerPause = uiObjects[36].GetComponent<Toggle>().isOn;
		settings_.fanletterTime = uiObjects[37].GetComponent<Slider>().value;
		settings_.gameTabData = uiObjects[38].GetComponent<Toggle>().isOn;
		mS_.settings_autoPauseForMultiplayer = uiObjects[39].GetComponent<Toggle>().isOn;
		settings_.saveInterval = uiObjects[40].GetComponent<Dropdown>().value;
		settings_.newsTime = uiObjects[41].GetComponent<Slider>().value;
		settings_.helligkeit = uiObjects[42].GetComponent<Slider>().value;
		settings_.dontAsk_TaskAbbrechen = uiObjects[43].GetComponent<Toggle>().isOn;
		settings_.middleMouseClose = uiObjects[44].GetComponent<Toggle>().isOn;
		settings_.camera90GradRotation = uiObjects[45].GetComponent<Toggle>().isOn;
		settings_.hideConvention = uiObjects[46].GetComponent<Toggle>().isOn;
		settings_.hideAwards = uiObjects[47].GetComponent<Toggle>().isOn;
		settings_.hideEvents = uiObjects[48].GetComponent<Toggle>().isOn;
		settings_.disableTochterfirmaAbrechnung = uiObjects[49].GetComponent<Toggle>().isOn;
		settings_.hideKuendigungen = uiObjects[50].GetComponent<Toggle>().isOn;
		settings_.tochtefirmaTAG = uiObjects[51].GetComponent<Toggle>().isOn;
		settings_.returnNullBugs = uiObjects[52].GetComponent<Toggle>().isOn;
		settings_.showMarktforschung = uiObjects[53].GetComponent<Toggle>().isOn;
		settings_.advancedPathfinding = uiObjects[54].GetComponent<Toggle>().isOn;
		settings_.moneyPop = uiObjects[55].GetComponent<Toggle>().isOn;
		settings_.beschwerdeTime = uiObjects[56].GetComponent<Slider>().value;
		settings_.disableRobotSound = uiObjects[57].GetComponent<Toggle>().isOn;
		settings_.disableMitarbeiterIcons = uiObjects[58].GetComponent<Toggle>().isOn;
		settings_.keyboard = uiObjects[59].GetComponent<Dropdown>().value;
		settings_.taskComplete = uiObjects[60].GetComponent<Toggle>().isOn;
		settings_.hideJahresabrechnung = uiObjects[61].GetComponent<Toggle>().isOn;
		switch (settings_.saveInterval)
		{
		case 0:
			mS_.autoSaveInterval = 12;
			break;
		case 1:
			mS_.autoSaveInterval = 9;
			break;
		case 2:
			mS_.autoSaveInterval = 6;
			break;
		case 3:
			mS_.autoSaveInterval = 3;
			break;
		case 4:
			mS_.autoSaveInterval = 1;
			break;
		default:
			mS_.autoSaveInterval = 12;
			break;
		}
		sfx_.SetVolume();
		settings_.UpdateSettings();
		BUTTON_Close();
		GameObject obj = GameObject.Find("CanvasInGameMenu");
		obj.SetActive(value: false);
		obj.SetActive(value: true);
		if (mS_.multiplayer && mpCalls_.isServer)
		{
			if (mS_.settings_autoPauseForMultiplayer)
			{
				mpCalls_.SERVER_Send_Command(7);
			}
			else
			{
				mpCalls_.SERVER_Send_Command(6);
			}
		}
	}

	public void TAB_Settings(int t)
	{
		TAB = t;
		sfx_.PlaySound(3, force: true);
		guiMain_.SetTab(uiObjects[4], t);
		switch (t)
		{
		case 0:
			uiObjects[5].SetActive(value: true);
			uiObjects[6].SetActive(value: false);
			uiObjects[7].SetActive(value: false);
			break;
		case 1:
			uiObjects[5].SetActive(value: false);
			uiObjects[6].SetActive(value: true);
			uiObjects[7].SetActive(value: false);
			break;
		case 2:
			uiObjects[5].SetActive(value: false);
			uiObjects[6].SetActive(value: false);
			uiObjects[7].SetActive(value: true);
			break;
		}
	}

	public void BUTTON_DefaultGuiSize()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[23].GetComponent<Slider>().value = 1f;
		if (!mainCanvas)
		{
			mainCanvas = GameObject.Find("CanvasInGameMenu");
		}
		if ((bool)mainCanvas)
		{
			mainCanvas.GetComponent<CanvasScaler>().scaleFactor = 1f;
		}
	}

	public void SLIDER_GuiSize()
	{
		float value = uiObjects[23].GetComponent<Slider>().value;
		if (Mathf.Abs(uiObjects[30].GetComponent<RectTransform>().anchoredPosition.y * value) + (float)Screen.height * 0.5f > (float)(Screen.height - 64))
		{
			uiObjects[23].GetComponent<Slider>().value -= 0.1f;
			return;
		}
		if (!mainCanvas)
		{
			mainCanvas = GameObject.Find("CanvasInGameMenu");
		}
		if ((bool)mainCanvas)
		{
			mainCanvas.GetComponent<CanvasScaler>().scaleFactor = uiObjects[23].GetComponent<Slider>().value;
		}
	}

	public void SLIDER_Helligkeit()
	{
		float value = uiObjects[42].GetComponent<Slider>().value;
		if ((bool)guiMain_)
		{
			guiMain_.hellgikeitsObjekt.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f - value / 255f);
		}
	}

	public void SLIDER_BeschwerdeTime()
	{
		_ = uiObjects[56].GetComponent<Slider>().value;
	}

	public void SLIDER_FanletterTime()
	{
		_ = uiObjects[37].GetComponent<Slider>().value;
	}

	public void SLIDER_NewsTime()
	{
		_ = uiObjects[41].GetComponent<Slider>().value;
	}

	public void BUTTON_DefaultBeschwerdeTime()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[56].GetComponent<Slider>().value = 5f;
	}

	public void BUTTON_DefaultFanletterTime()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[37].GetComponent<Slider>().value = 5f;
	}

	public void BUTTON_DefaultNewsTime()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[41].GetComponent<Slider>().value = 5f;
	}

	public void BUTTON_DefaultHelligkeit()
	{
		sfx_.PlaySound(3, force: true);
		uiObjects[42].GetComponent<Slider>().value = 255f;
	}
}
