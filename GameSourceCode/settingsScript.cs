using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class settingsScript : MonoBehaviour
{
	private ES3Writer writer;

	private ES3Reader reader;

	public bool splashScreen;

	public int language;

	public int saveInterval = 12;

	public float uiScale = 1f;

	public bool roomConnections = true;

	public bool pauseUI = true;

	public bool leaderboard = true;

	public bool chat = true;

	public bool sprechblasen = true;

	public bool scrollScreen = true;

	public bool disableEngineAbrechnung;

	public bool disableWorkIcons;

	public bool disableMitarbeiterIcons;

	public bool disableArbeiterBeschwerden;

	public bool singleplayerPause;

	public float fanletterTime = 5f;

	public float newsTime = 5f;

	public bool gameTabData;

	public bool dontAsk_TaskAbbrechen;

	public bool middleMouseClose;

	public bool camera90GradRotation;

	public bool hideConvention;

	public bool hideAwards;

	public bool hideEvents;

	public bool hideJahresabrechnung;

	public bool disableTochterfirmaAbrechnung;

	public bool hideKuendigungen;

	public bool tochtefirmaTAG = true;

	public bool returnNullBugs = true;

	public bool showMarktforschung;

	public bool advancedPathfinding;

	public bool moneyPop;

	public bool taskComplete;

	public float beschwerdeTime = 5f;

	public int keyboard;

	public float musicVolume = 0.5f;

	public float masterVolume = 1f;

	public bool disableWorkIconSound;

	public bool disableRobotSound;

	public int screenX = 1024;

	public int screenY = 768;

	public bool vollbild = true;

	public int framerate = 60;

	public int fullScreenMode = 1;

	public int vSync = 1;

	public bool shadows = true;

	public bool SSAO = true;

	public bool screenSpaceReflections = true;

	public bool bloom = true;

	public bool ambientOcclusion = true;

	public bool colorGrading = true;

	public bool disableWetter = true;

	public float helligkeit = 1.81f;

	private GameObject mainCanvas;

	private GUI_Main guiMain_;

	public PostProcessProfile postProcess;

	public PostProcessLayer postLayer;

	private void Awake()
	{
		LoadSettings();
		UpdateSettings();
	}

	public void LoadSettings()
	{
		string filePath = "settings.txt";
		if (!ES3.FileExists(filePath))
		{
			return;
		}
		string text = ES3.LoadRawString(filePath);
		if (text.Length <= 0 || text[0] != '{')
		{
			return;
		}
		reader = ES3Reader.Create(filePath);
		if (reader != null)
		{
			int[] array = new int[100];
			float[] array2 = new float[100];
			bool[] array3 = new bool[100];
			array = reader.Read<int[]>("settings_int");
			array2 = reader.Read<float[]>("settings_float");
			array3 = reader.Read<bool[]>("settings_bool");
			language = array[0];
			screenX = array[1];
			screenY = array[2];
			framerate = array[3];
			vSync = array[4];
			fullScreenMode = array[5];
			saveInterval = array[6];
			keyboard = array[7];
			uiScale = array2[0];
			musicVolume = array2[1];
			masterVolume = array2[2];
			fanletterTime = array2[3];
			if (fanletterTime <= 0f)
			{
				fanletterTime = 5f;
			}
			newsTime = array2[4];
			if (newsTime <= 0f)
			{
				newsTime = 5f;
			}
			helligkeit = array2[5];
			if (helligkeit <= 50f)
			{
				helligkeit = 255f;
			}
			beschwerdeTime = array2[6];
			if (beschwerdeTime <= 0f)
			{
				beschwerdeTime = 5f;
			}
			vollbild = array3[0];
			shadows = array3[1];
			SSAO = array3[2];
			screenSpaceReflections = array3[3];
			bloom = array3[4];
			ambientOcclusion = array3[5];
			colorGrading = array3[6];
			roomConnections = array3[7];
			pauseUI = array3[8];
			leaderboard = array3[9];
			chat = array3[10];
			disableWorkIconSound = array3[11];
			sprechblasen = array3[12];
			scrollScreen = array3[13];
			disableEngineAbrechnung = array3[14];
			disableWorkIcons = array3[15];
			disableArbeiterBeschwerden = array3[16];
			disableWetter = array3[17];
			singleplayerPause = false;
			gameTabData = array3[19];
			dontAsk_TaskAbbrechen = array3[20];
			middleMouseClose = array3[21];
			camera90GradRotation = array3[22];
			hideConvention = array3[23];
			hideAwards = array3[24];
			hideEvents = array3[25];
			disableTochterfirmaAbrechnung = array3[26];
			hideKuendigungen = array3[27];
			tochtefirmaTAG = array3[28];
			returnNullBugs = array3[29];
			showMarktforschung = array3[30];
			advancedPathfinding = array3[31];
			moneyPop = array3[32];
			disableRobotSound = array3[33];
			disableMitarbeiterIcons = array3[34];
			taskComplete = array3[35];
			hideJahresabrechnung = array3[36];
			reader.Dispose();
		}
	}

	public void SaveSettings()
	{
		string filePath = "settings.txt";
		ES3Settings settings = new ES3Settings();
		ES3.DeleteFile(filePath);
		writer = ES3Writer.Create(filePath, settings);
		int[] array = new int[100];
		float[] array2 = new float[100];
		bool[] array3 = new bool[100];
		array[0] = language;
		array[1] = screenX;
		array[2] = screenY;
		array[3] = framerate;
		array[4] = vSync;
		array[5] = fullScreenMode;
		array[6] = saveInterval;
		array[7] = keyboard;
		array2[0] = uiScale;
		array2[1] = musicVolume;
		array2[2] = masterVolume;
		array2[3] = fanletterTime;
		array2[4] = newsTime;
		array2[5] = helligkeit;
		array2[6] = beschwerdeTime;
		array3[0] = vollbild;
		array3[1] = shadows;
		array3[2] = SSAO;
		array3[3] = screenSpaceReflections;
		array3[4] = bloom;
		array3[5] = ambientOcclusion;
		array3[6] = colorGrading;
		array3[7] = roomConnections;
		array3[8] = pauseUI;
		array3[9] = leaderboard;
		array3[10] = chat;
		array3[11] = disableWorkIconSound;
		array3[12] = sprechblasen;
		array3[13] = scrollScreen;
		array3[14] = disableEngineAbrechnung;
		array3[15] = disableWorkIcons;
		array3[16] = disableArbeiterBeschwerden;
		array3[17] = disableWetter;
		array3[18] = singleplayerPause;
		array3[19] = gameTabData;
		array3[20] = dontAsk_TaskAbbrechen;
		array3[21] = middleMouseClose;
		array3[22] = camera90GradRotation;
		array3[23] = hideConvention;
		array3[24] = hideAwards;
		array3[25] = hideEvents;
		array3[26] = disableTochterfirmaAbrechnung;
		array3[27] = hideKuendigungen;
		array3[28] = tochtefirmaTAG;
		array3[29] = returnNullBugs;
		array3[30] = showMarktforschung;
		array3[31] = advancedPathfinding;
		array3[32] = moneyPop;
		array3[33] = disableRobotSound;
		array3[34] = disableMitarbeiterIcons;
		array3[35] = taskComplete;
		array3[36] = hideJahresabrechnung;
		writer.Write<int[]>("settings_int", array);
		writer.Write<float[]>("settings_float", array2);
		writer.Write<bool[]>("settings_bool", array3);
		writer.Save();
		writer.Dispose();
	}

	public void UpdateSettings()
	{
		QualitySettings.vSyncCount = vSync;
		Application.targetFrameRate = framerate;
		if (screenX > 0)
		{
			if (vollbild)
			{
				switch (fullScreenMode)
				{
				case 0:
					Screen.SetResolution(screenX, screenY, FullScreenMode.FullScreenWindow);
					break;
				case 1:
					Screen.SetResolution(screenX, screenY, FullScreenMode.ExclusiveFullScreen);
					break;
				case 2:
					Screen.SetResolution(screenX, screenY, FullScreenMode.MaximizedWindow);
					break;
				default:
					Screen.SetResolution(screenX, screenY, fullscreen: true);
					break;
				}
			}
			else
			{
				Screen.SetResolution(screenX, screenY, fullscreen: false);
			}
		}
		else
		{
			Resolution currentResolution = Screen.currentResolution;
			Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
			SetAutomaticGuiScale(currentResolution.width);
			screenX = currentResolution.width;
			screenY = currentResolution.height;
			vollbild = true;
			fullScreenMode = 0;
		}
		if (!mainCanvas)
		{
			mainCanvas = GameObject.Find("CanvasInGameMenu");
		}
		if ((bool)mainCanvas)
		{
			mainCanvas.GetComponent<CanvasScaler>().scaleFactor = uiScale;
		}
		if ((bool)mainCanvas)
		{
			if (helligkeit < 50f)
			{
				helligkeit = 255f;
			}
			if (helligkeit > 255f)
			{
				helligkeit = 255f;
			}
			mainCanvas.GetComponent<GUI_Main>().hellgikeitsObjekt.GetComponent<Image>().color = new Color(0f, 0f, 0f, 1f - helligkeit / 255f);
		}
		if (shadows)
		{
			QualitySettings.shadows = ShadowQuality.All;
		}
		else
		{
			QualitySettings.shadows = ShadowQuality.Disable;
		}
		if (!splashScreen)
		{
			if (postProcess.TryGetSettings<ScreenSpaceReflections>(out var outSetting))
			{
				outSetting.enabled.value = screenSpaceReflections;
			}
			if (postProcess.TryGetSettings<Bloom>(out var outSetting2))
			{
				outSetting2.enabled.value = bloom;
			}
			if (postProcess.TryGetSettings<AmbientOcclusion>(out var outSetting3))
			{
				outSetting3.enabled.value = ambientOcclusion;
			}
			if (postProcess.TryGetSettings<ColorGrading>(out var outSetting4))
			{
				outSetting4.enabled.value = colorGrading;
			}
		}
		SaveSettings();
	}

	public void SetAutomaticGuiScale(int screen_width)
	{
		switch (screen_width)
		{
		case 1024:
			uiScale = 0.68f;
			break;
		case 1152:
			uiScale = 0.75f;
			break;
		case 1176:
			uiScale = 0.75f;
			break;
		case 1280:
			uiScale = 0.83f;
			break;
		case 1360:
			uiScale = 0.83f;
			break;
		case 1366:
			uiScale = 0.83f;
			break;
		case 1600:
			uiScale = 1f;
			break;
		case 1680:
			uiScale = 1f;
			break;
		case 1720:
			uiScale = 1f;
			break;
		case 1920:
			uiScale = 1f;
			break;
		case 2048:
			uiScale = 1f;
			break;
		case 2560:
			uiScale = 1f;
			break;
		case 3440:
			uiScale = 1.24f;
			break;
		default:
			uiScale = 1f;
			break;
		}
		Debug.Log(uiScale);
	}
}
