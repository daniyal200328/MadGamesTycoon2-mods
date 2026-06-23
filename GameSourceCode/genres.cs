using System.IO;
using System.Text;
using UnityEngine;

public class genres : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private GUI_Main guiMain_;

	private games games_;

	private themes themes_;

	private Sprite[] genres_PIC;

	public float[] genres_BELIEBTHEIT;

	public bool[] genres_BELIEBTHEIT_SOLL;

	public int[] genres_RES_POINTS;

	public float[] genres_RES_POINTS_LEFT;

	public int[] genres_PRICE;

	public int[] genres_DEV_COSTS;

	public int[] genres_DATE_YEAR;

	public int[] genres_DATE_MONTH;

	public int[] genres_LEVEL;

	public bool[] genres_UNLOCK;

	public bool[] genres_SUC_YEAR;

	private Sprite[,] genres_SCREENSHOTS;

	private Texture2D[,] genres_SCREENSHOTS_TEXTURE;

	private int[] genres_SCREENSHOTS_AMOUNT;

	public bool[,] genres_TARGETGROUP;

	public float[] genres_GAMEPLAY;

	public float[] genres_GRAPHIC;

	public float[] genres_SOUND;

	public float[] genres_CONTROL;

	public bool[,] genres_COMBINATION;

	public int[,] genres_PLATFORM_SELLS;

	public int[,] genres_FOCUS;

	public int[,] genres_ALIGN;

	public string[] genres_ICONFILE;

	public string[] genres_NAME_EN;

	public string[] genres_NAME_GE;

	public string[] genres_NAME_TU;

	public string[] genres_NAME_CH;

	public string[] genres_NAME_FR;

	public string[] genres_NAME_PB;

	public string[] genres_NAME_HU;

	public string[] genres_NAME_CT;

	public string[] genres_NAME_ES;

	public string[] genres_NAME_PL;

	public string[] genres_NAME_CZ;

	public string[] genres_NAME_KO;

	public string[] genres_NAME_IT;

	public string[] genres_NAME_AR;

	public string[] genres_NAME_JA;

	public string[] genres_NAME_UA;

	public string[] genres_NAME_TH;

	public string[] genres_NAME_RU;

	public string[] genres_DESC_EN;

	public string[] genres_DESC_GE;

	public string[] genres_DESC_TU;

	public string[] genres_DESC_CH;

	public string[] genres_DESC_FR;

	public string[] genres_DESC_PB;

	public string[] genres_DESC_HU;

	public string[] genres_DESC_CT;

	public string[] genres_DESC_ES;

	public string[] genres_DESC_PL;

	public string[] genres_DESC_CZ;

	public string[] genres_DESC_KO;

	public string[] genres_DESC_IT;

	public string[] genres_DESC_AR;

	public string[] genres_DESC_JA;

	public string[] genres_DESC_UA;

	public string[] genres_DESC_TH;

	public string[] genres_DESC_RU;

	public int[] genres_FANS;

	public int[] genres_MARKT;

	private string[] data;

	private void Awake()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!tS_)
		{
			tS_ = GetComponent<textScript>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
		if (!themes_)
		{
			themes_ = GetComponent<themes>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init()
	{
		genres_PIC = new Sprite[genres_LEVEL.Length];
		genres_SCREENSHOTS = new Sprite[genres_LEVEL.Length, 99];
		genres_SCREENSHOTS_TEXTURE = new Texture2D[genres_LEVEL.Length, 99];
		genres_SCREENSHOTS_AMOUNT = new int[genres_LEVEL.Length];
	}

	public bool[] Return1DimensionArray_TARGETGROUP()
	{
		int num = genres_UNLOCK.Length;
		int num2 = 5;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = genres_TARGETGROUP[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_TARGETGROUP(bool[] arr)
	{
		int num = genres_UNLOCK.Length;
		int num2 = arr.Length / num;
		genres_TARGETGROUP = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				genres_TARGETGROUP[i, j] = arr[i * num2 + j];
			}
		}
	}

	public bool[] Return1DimensionArray_COMBINATION()
	{
		int num = genres_UNLOCK.Length;
		int num2 = genres_UNLOCK.Length;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = genres_COMBINATION[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_COMBINATION(bool[] arr)
	{
		int num = genres_UNLOCK.Length;
		int num2 = arr.Length / num;
		genres_COMBINATION = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				genres_COMBINATION[i, j] = arr[i * num2 + j];
			}
		}
	}

	public int[] Return1DimensionArray_FOCUS()
	{
		int num = genres_UNLOCK.Length;
		int num2 = 8;
		int[] array = new int[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = genres_FOCUS[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_FOCUS(int[] arr)
	{
		int num = genres_UNLOCK.Length;
		int num2 = arr.Length / num;
		genres_FOCUS = new int[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				genres_FOCUS[i, j] = arr[i * num2 + j];
			}
		}
	}

	public int[] Return1DimensionArray_ALIGN()
	{
		int num = genres_UNLOCK.Length;
		int num2 = 3;
		int[] array = new int[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = genres_ALIGN[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_ALIGN(int[] arr)
	{
		int num = genres_UNLOCK.Length;
		int num2 = arr.Length / num;
		genres_ALIGN = new int[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				genres_ALIGN[i, j] = arr[i * num2 + j];
			}
		}
	}

	public int[] Return1DimensionArray_PLATFORM_SELLS()
	{
		int num = genres_UNLOCK.Length;
		int num2 = 5;
		int[] array = new int[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = genres_PLATFORM_SELLS[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_PLATFORM_SELLS(int[] arr)
	{
		int num = genres_UNLOCK.Length;
		int num2 = arr.Length / num;
		genres_PLATFORM_SELLS = new int[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				genres_PLATFORM_SELLS[i, j] = arr[i * num2 + j];
			}
		}
	}

	public int LoadAmountOfGenres(string filename)
	{
		if (genres_UNLOCK.Length != 0)
		{
			return genres_UNLOCK.Length;
		}
		if (!tS_)
		{
			FindScripts();
		}
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		data = text.Split("\n"[0]);
		data = tS_.RemoveComments(data);
		int num = 0;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].Contains("[ID]"))
			{
				num++;
			}
		}
		return num;
	}

	public void LoadGenres(string filename)
	{
		int num = 0;
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		data = text.Split("\n"[0]);
		data = tS_.RemoveComments(data);
		int num2 = 0;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].Contains("[ID]"))
			{
				num2++;
			}
		}
		genres_BELIEBTHEIT = new float[num2];
		genres_BELIEBTHEIT_SOLL = new bool[num2];
		genres_PIC = new Sprite[num2];
		genres_RES_POINTS = new int[num2];
		genres_RES_POINTS_LEFT = new float[num2];
		genres_PRICE = new int[num2];
		genres_DEV_COSTS = new int[num2];
		genres_DATE_YEAR = new int[num2];
		genres_DATE_MONTH = new int[num2];
		genres_LEVEL = new int[num2];
		genres_UNLOCK = new bool[num2];
		genres_SUC_YEAR = new bool[num2];
		genres_GAMEPLAY = new float[num2];
		genres_GRAPHIC = new float[num2];
		genres_SOUND = new float[num2];
		genres_CONTROL = new float[num2];
		genres_FOCUS = new int[num2, 8];
		genres_ALIGN = new int[num2, 3];
		genres_ICONFILE = new string[num2];
		genres_NAME_EN = new string[num2];
		genres_NAME_GE = new string[num2];
		genres_NAME_TU = new string[num2];
		genres_NAME_CH = new string[num2];
		genres_NAME_FR = new string[num2];
		genres_NAME_PB = new string[num2];
		genres_NAME_HU = new string[num2];
		genres_NAME_CT = new string[num2];
		genres_NAME_ES = new string[num2];
		genres_NAME_PL = new string[num2];
		genres_NAME_CZ = new string[num2];
		genres_NAME_KO = new string[num2];
		genres_NAME_IT = new string[num2];
		genres_NAME_AR = new string[num2];
		genres_NAME_JA = new string[num2];
		genres_NAME_UA = new string[num2];
		genres_NAME_TH = new string[num2];
		genres_NAME_RU = new string[num2];
		genres_DESC_EN = new string[num2];
		genres_DESC_GE = new string[num2];
		genres_DESC_TU = new string[num2];
		genres_DESC_CH = new string[num2];
		genres_DESC_FR = new string[num2];
		genres_DESC_PB = new string[num2];
		genres_DESC_HU = new string[num2];
		genres_DESC_CT = new string[num2];
		genres_DESC_ES = new string[num2];
		genres_DESC_PL = new string[num2];
		genres_DESC_CZ = new string[num2];
		genres_DESC_KO = new string[num2];
		genres_DESC_IT = new string[num2];
		genres_DESC_AR = new string[num2];
		genres_DESC_JA = new string[num2];
		genres_DESC_UA = new string[num2];
		genres_DESC_TH = new string[num2];
		genres_DESC_RU = new string[num2];
		genres_SCREENSHOTS = new Sprite[num2, 99];
		genres_SCREENSHOTS_TEXTURE = new Texture2D[num2, 99];
		genres_SCREENSHOTS_AMOUNT = new int[num2];
		genres_TARGETGROUP = new bool[num2, 5];
		genres_COMBINATION = new bool[num2, num2];
		genres_PLATFORM_SELLS = new int[num2, 5];
		genres_FANS = new int[num2];
		genres_MARKT = new int[num2];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
			}
			if (ParseData("[TGROUP]", j))
			{
				if (ParseDataDontCutLastChar("<KID>", j))
				{
					genres_TARGETGROUP[num3, 0] = true;
				}
				if (ParseDataDontCutLastChar("<TEEN>", j))
				{
					genres_TARGETGROUP[num3, 1] = true;
				}
				if (ParseDataDontCutLastChar("<ADULT>", j))
				{
					genres_TARGETGROUP[num3, 2] = true;
				}
				if (ParseDataDontCutLastChar("<OLD>", j))
				{
					genres_TARGETGROUP[num3, 3] = true;
				}
				if (ParseDataDontCutLastChar("<ALL>", j))
				{
					genres_TARGETGROUP[num3, 4] = true;
				}
			}
			if (ParseData("[RES POINTS]", j))
			{
				genres_RES_POINTS[num3] = int.Parse(data[j]);
				genres_RES_POINTS_LEFT[num3] = genres_RES_POINTS[num3];
			}
			if (ParseData("[PRICE]", j))
			{
				genres_PRICE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[SUC YEAR]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					genres_SUC_YEAR[num3] = true;
				}
				else
				{
					genres_SUC_YEAR[num3] = false;
				}
			}
			if (ParseData("[DEV COSTS]", j))
			{
				genres_DEV_COSTS[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GAMEPLAY]", j))
			{
				genres_GAMEPLAY[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GRAPHIC]", j))
			{
				genres_GRAPHIC[num3] = int.Parse(data[j]);
			}
			if (ParseData("[SOUND]", j))
			{
				genres_SOUND[num3] = int.Parse(data[j]);
			}
			if (ParseData("[CONTROL]", j))
			{
				genres_CONTROL[num3] = int.Parse(data[j]);
			}
			for (int k = 0; k < 8; k++)
			{
				if (ParseData("[FOCUS" + k + "]", j))
				{
					genres_FOCUS[num3, k] = int.Parse(data[j]);
				}
			}
			for (int l = 0; l < 3; l++)
			{
				if (ParseData("[ALIGN" + l + "]", j))
				{
					genres_ALIGN[num3, l] = int.Parse(data[j]);
				}
			}
			if (ParseData("[GENRE COMB]", j))
			{
				for (int m = 0; m < genres_LEVEL.Length; m++)
				{
					if (ParseDataDontCutLastChar("<" + m + ">", j))
					{
						genres_COMBINATION[num3, m] = true;
					}
				}
			}
			if (ParseData("[P_PC]", j))
			{
				genres_PLATFORM_SELLS[num3, 0] = int.Parse(data[j]);
				Debug.Log("P_PC: " + num3 + ", " + genres_PLATFORM_SELLS[num3, 0]);
			}
			if (ParseData("[P_CONSOLE]", j))
			{
				genres_PLATFORM_SELLS[num3, 1] = int.Parse(data[j]);
				Debug.Log("P_CONSOLE: " + num3 + ", " + genres_PLATFORM_SELLS[num3, 1]);
			}
			if (ParseData("[P_HANDHELD]", j))
			{
				genres_PLATFORM_SELLS[num3, 2] = int.Parse(data[j]);
				Debug.Log("P_HANDHELD: " + num3 + ", " + genres_PLATFORM_SELLS[num3, 2]);
			}
			if (ParseData("[P_PHONE]", j))
			{
				genres_PLATFORM_SELLS[num3, 3] = int.Parse(data[j]);
				Debug.Log("P_PHONE: " + num3 + ", " + genres_PLATFORM_SELLS[num3, 3]);
			}
			if (ParseData("[P_ARCADE]", j))
			{
				genres_PLATFORM_SELLS[num3, 4] = int.Parse(data[j]);
				Debug.Log("P_ARCADE: " + num3 + ", " + genres_PLATFORM_SELLS[num3, 4]);
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					genres_DATE_MONTH[num3] = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					genres_DATE_MONTH[num3] = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					genres_DATE_MONTH[num3] = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					genres_DATE_MONTH[num3] = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					genres_DATE_MONTH[num3] = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					genres_DATE_MONTH[num3] = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					genres_DATE_MONTH[num3] = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					genres_DATE_MONTH[num3] = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					genres_DATE_MONTH[num3] = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					genres_DATE_MONTH[num3] = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					genres_DATE_MONTH[num3] = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					genres_DATE_MONTH[num3] = 12;
				}
				if (genres_DATE_MONTH[num3] <= 0)
				{
					Debug.Log("ERROR: Genres.txt -> Incorrect Month: " + genres_NAME_EN[num3]);
				}
				genres_DATE_YEAR[num3] = int.Parse(data[j]);
				if (genres_DATE_YEAR[num3] == 1976 && genres_DATE_MONTH[num3] == 1)
				{
					genres_UNLOCK[num3] = true;
				}
			}
			if (ParseData("[PIC]", j))
			{
				genres_ICONFILE[num3] = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				genres_NAME_GE[num3] = data[j];
			}
			if (ParseData("[NAME EN]", j))
			{
				genres_NAME_EN[num3] = data[j];
			}
			if (ParseData("[NAME TU]", j))
			{
				genres_NAME_TU[num3] = data[j];
			}
			if (ParseData("[NAME CH]", j))
			{
				genres_NAME_CH[num3] = data[j];
			}
			if (ParseData("[NAME FR]", j))
			{
				genres_NAME_FR[num3] = data[j];
			}
			if (ParseData("[NAME PB]", j))
			{
				genres_NAME_PB[num3] = data[j];
			}
			if (ParseData("[NAME HU]", j))
			{
				genres_NAME_HU[num3] = data[j];
			}
			if (ParseData("[NAME CT]", j))
			{
				genres_NAME_CT[num3] = data[j];
			}
			if (ParseData("[NAME ES]", j))
			{
				genres_NAME_ES[num3] = data[j];
			}
			if (ParseData("[NAME PL]", j))
			{
				genres_NAME_PL[num3] = data[j];
			}
			if (ParseData("[NAME CZ]", j))
			{
				genres_NAME_CZ[num3] = data[j];
			}
			if (ParseData("[NAME KO]", j))
			{
				genres_NAME_KO[num3] = data[j];
			}
			if (ParseData("[NAME IT]", j))
			{
				genres_NAME_IT[num3] = data[j];
			}
			if (ParseData("[NAME AR]", j))
			{
				genres_NAME_AR[num3] = data[j];
			}
			if (ParseData("[NAME JA]", j))
			{
				genres_NAME_JA[num3] = data[j];
			}
			if (ParseData("[NAME UA]", j))
			{
				genres_NAME_UA[num3] = data[j];
			}
			if (ParseData("[NAME TH]", j))
			{
				genres_NAME_TH[num3] = data[j];
			}
			if (ParseData("[NAME RU]", j))
			{
				genres_NAME_RU[num3] = data[j];
			}
			if (ParseData("[DESC GE]", j))
			{
				genres_DESC_GE[num3] = data[j];
			}
			if (ParseData("[DESC EN]", j))
			{
				genres_DESC_EN[num3] = data[j];
			}
			if (ParseData("[DESC TU]", j))
			{
				genres_DESC_TU[num3] = data[j];
			}
			if (ParseData("[DESC CH]", j))
			{
				genres_DESC_CH[num3] = data[j];
			}
			if (ParseData("[DESC FR]", j))
			{
				genres_DESC_FR[num3] = data[j];
			}
			if (ParseData("[DESC PB]", j))
			{
				genres_DESC_PB[num3] = data[j];
			}
			if (ParseData("[DESC HU]", j))
			{
				genres_DESC_HU[num3] = data[j];
			}
			if (ParseData("[DESC CT]", j))
			{
				genres_DESC_CT[num3] = data[j];
			}
			if (ParseData("[DESC ES]", j))
			{
				genres_DESC_ES[num3] = data[j];
			}
			if (ParseData("[DESC PL]", j))
			{
				genres_DESC_PL[num3] = data[j];
			}
			if (ParseData("[DESC CZ]", j))
			{
				genres_DESC_CZ[num3] = data[j];
			}
			if (ParseData("[DESC KO]", j))
			{
				genres_DESC_KO[num3] = data[j];
			}
			if (ParseData("[DESC IT]", j))
			{
				genres_DESC_IT[num3] = data[j];
			}
			if (ParseData("[DESC AR]", j))
			{
				genres_DESC_AR[num3] = data[j];
			}
			if (ParseData("[DESC JA]", j))
			{
				genres_DESC_JA[num3] = data[j];
			}
			if (ParseData("[DESC UA]", j))
			{
				genres_DESC_UA[num3] = data[j];
			}
			if (ParseData("[DESC TH]", j))
			{
				genres_DESC_TH[num3] = data[j];
			}
			if (ParseData("[DESC RU]", j))
			{
				genres_DESC_RU[num3] = data[j];
			}
			if (!ParseData("[EOF]", j))
			{
				num++;
				continue;
			}
			break;
		}
	}

	public void LoadGenresettingsForOldSavegeames(string filename)
	{
		int num = 0;
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		data = text.Split("\n"[0]);
		int num2 = 0;
		for (int i = 0; i < data.Length; i++)
		{
			if (data[i].Contains("[ID]"))
			{
				num2++;
			}
		}
		genres_FOCUS = new int[num2, 8];
		genres_ALIGN = new int[num2, 3];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
			}
			for (int k = 0; k < 8; k++)
			{
				if (ParseData("[FOCUS" + k + "]", j))
				{
					genres_FOCUS[num3, k] = int.Parse(data[j]);
				}
			}
			for (int l = 0; l < 3; l++)
			{
				if (ParseData("[ALIGN" + l + "]", j))
				{
					genres_ALIGN[num3, l] = int.Parse(data[j]);
				}
			}
			if (ParseData("[EOF]", j))
			{
				Debug.Log("Genres.txt -> EOF");
				break;
			}
			num++;
		}
	}

	private bool ParseData(string c, int i)
	{
		if (data[i].Contains(c))
		{
			data[i] = data[i].Remove(data[i].Length - 1, 1);
			data[i] = data[i].Replace(c, "");
			return true;
		}
		return false;
	}

	private bool ParseDataDontCutLastChar(string c, int i)
	{
		if (data[i].Contains(c))
		{
			data[i] = data[i].Replace(c, "");
			return true;
		}
		return false;
	}

	public Sprite GetScreenshot(int genre_, int grafikPoints)
	{
		int num = 0;
		if (genres_SCREENSHOTS_AMOUNT[genre_] <= 0)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (!File.Exists(Application.dataPath + "/Extern/Screenshots/" + genre_ + "/" + i + ".png"))
				{
					break;
				}
				num = i;
			}
		}
		else
		{
			num = genres_SCREENSHOTS_AMOUNT[genre_];
		}
		int num2 = 35000 / (num + 1);
		num2 = grafikPoints / num2;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > num)
		{
			num2 = num;
		}
		if ((bool)genres_SCREENSHOTS[genre_, num2])
		{
			return genres_SCREENSHOTS[genre_, num2];
		}
		genres_SCREENSHOTS[genre_, num2] = mS_.LoadPNG(Application.dataPath + "/Extern/Screenshots/" + genre_ + "/" + num2 + ".png");
		return genres_SCREENSHOTS[genre_, num2];
	}

	public Texture2D GetScreenshotTexture2D(int genre_, int grafikPoints)
	{
		int num = 0;
		if (genres_SCREENSHOTS_AMOUNT[genre_] <= 0)
		{
			for (int i = 0; i < 1000; i++)
			{
				if (!File.Exists(Application.dataPath + "/Extern/Screenshots/" + genre_ + "/" + i + ".png"))
				{
					break;
				}
				num = i;
				genres_SCREENSHOTS_AMOUNT[genre_] = i;
			}
		}
		else
		{
			num = genres_SCREENSHOTS_AMOUNT[genre_];
		}
		int num2 = 30000 / (num + 1);
		num2 = grafikPoints / num2;
		if (num2 < 0)
		{
			num2 = 0;
		}
		if (num2 > num)
		{
			num2 = num;
		}
		if ((bool)genres_SCREENSHOTS_TEXTURE[genre_, num2])
		{
			return genres_SCREENSHOTS_TEXTURE[genre_, num2];
		}
		genres_SCREENSHOTS_TEXTURE[genre_, num2] = mS_.LoadTexture(Application.dataPath + "/Extern/Screenshots/" + genre_ + "/" + num2 + ".png");
		return genres_SCREENSHOTS_TEXTURE[genre_, num2];
	}

	public string GetName(int i)
	{
		if (genres_PIC == null)
		{
			return "<Not initialized>";
		}
		if (genres_NAME_EN.Length == 0)
		{
			return "<Not initialized>";
		}
		if (i < 0)
		{
			return tS_.GetText(688);
		}
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = genres_NAME_EN[i];
			break;
		case 1:
			text = genres_NAME_GE[i];
			break;
		case 2:
			if (genres_NAME_TU.Length != 0)
			{
				text = genres_NAME_TU[i];
			}
			break;
		case 3:
			if (genres_NAME_CH.Length != 0)
			{
				text = genres_NAME_CH[i];
			}
			break;
		case 4:
			if (genres_NAME_FR.Length != 0)
			{
				text = genres_NAME_FR[i];
			}
			break;
		case 5:
			if (genres_NAME_ES.Length != 0)
			{
				text = genres_NAME_ES[i];
			}
			break;
		case 6:
			if (genres_NAME_KO.Length != 0)
			{
				text = genres_NAME_KO[i];
			}
			break;
		case 7:
			if (genres_NAME_PB.Length != 0)
			{
				text = genres_NAME_PB[i];
			}
			break;
		case 8:
			if (genres_NAME_HU.Length != 0)
			{
				text = genres_NAME_HU[i];
			}
			break;
		case 9:
			if (genres_NAME_RU.Length != 0)
			{
				text = genres_NAME_RU[i];
			}
			break;
		case 10:
			if (genres_NAME_CT.Length != 0)
			{
				text = genres_NAME_CT[i];
			}
			break;
		case 11:
			if (genres_NAME_PL.Length != 0)
			{
				text = genres_NAME_PL[i];
			}
			break;
		case 12:
			if (genres_NAME_CZ.Length != 0)
			{
				text = genres_NAME_CZ[i];
			}
			break;
		case 13:
			if (genres_NAME_AR.Length != 0)
			{
				text = genres_NAME_AR[i];
			}
			break;
		case 14:
			if (genres_NAME_IT.Length != 0)
			{
				text = genres_NAME_IT[i];
			}
			break;
		case 16:
			if (genres_NAME_JA.Length != 0)
			{
				text = genres_NAME_JA[i];
			}
			break;
		case 17:
			if (genres_NAME_UA.Length != 0)
			{
				text = genres_NAME_UA[i];
			}
			break;
		case 19:
			if (genres_NAME_TH.Length != 0)
			{
				text = genres_NAME_TH[i];
			}
			break;
		default:
			text = genres_NAME_EN[i];
			break;
		}
		if (text == null)
		{
			return genres_NAME_EN[i];
		}
		if (text.Length <= 0)
		{
			return genres_NAME_EN[i];
		}
		return text;
	}

	public string GetDesc(int i)
	{
		if (genres_PIC == null)
		{
			return "<Not initialized>";
		}
		if (i < 0)
		{
			return "";
		}
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = genres_DESC_EN[i];
			break;
		case 1:
			text = genres_DESC_GE[i];
			break;
		case 2:
			if (genres_DESC_TU.Length != 0)
			{
				text = genres_DESC_TU[i];
			}
			break;
		case 3:
			if (genres_DESC_CH.Length != 0)
			{
				text = genres_DESC_CH[i];
			}
			break;
		case 4:
			if (genres_DESC_FR.Length != 0)
			{
				text = genres_DESC_FR[i];
			}
			break;
		case 5:
			if (genres_DESC_ES.Length != 0)
			{
				text = genres_DESC_ES[i];
			}
			break;
		case 6:
			if (genres_DESC_KO.Length != 0)
			{
				text = genres_DESC_KO[i];
			}
			break;
		case 7:
			if (genres_DESC_PB.Length != 0)
			{
				text = genres_DESC_PB[i];
			}
			break;
		case 8:
			if (genres_DESC_HU.Length != 0)
			{
				text = genres_DESC_HU[i];
			}
			break;
		case 9:
			if (genres_DESC_RU.Length != 0)
			{
				text = genres_DESC_RU[i];
			}
			break;
		case 10:
			if (genres_DESC_CT.Length != 0)
			{
				text = genres_DESC_CT[i];
			}
			break;
		case 11:
			if (genres_DESC_PL.Length != 0)
			{
				text = genres_DESC_PL[i];
			}
			break;
		case 12:
			if (genres_DESC_CZ.Length != 0)
			{
				text = genres_DESC_CZ[i];
			}
			break;
		case 13:
			if (genres_DESC_AR.Length != 0)
			{
				text = genres_DESC_AR[i];
			}
			break;
		case 14:
			if (genres_DESC_IT.Length != 0)
			{
				text = genres_DESC_IT[i];
			}
			break;
		case 16:
			if (genres_DESC_JA.Length != 0)
			{
				text = genres_DESC_JA[i];
			}
			break;
		case 17:
			if (genres_DESC_UA.Length != 0)
			{
				text = genres_DESC_UA[i];
			}
			break;
		case 19:
			if (genres_DESC_TH.Length != 0)
			{
				text = genres_DESC_TH[i];
			}
			break;
		default:
			text = genres_DESC_EN[i];
			break;
		}
		if (text == null)
		{
			return genres_DESC_EN[i];
		}
		if (text.Length <= 0)
		{
			return genres_DESC_EN[i];
		}
		return text;
	}

	public int GetDevCosts(int i)
	{
		float num = (float)genres_LEVEL[i] * 0.1f;
		num = (float)genres_DEV_COSTS[i] * (1f - num);
		return Mathf.RoundToInt(num);
	}

	public int GetPrice(int i)
	{
		return genres_PRICE[i];
	}

	public Sprite GetPic(int i)
	{
		if (genres_PIC == null)
		{
			return guiMain_.uiSprites[3];
		}
		if (i > -1)
		{
			if (genres_ICONFILE[i].Length > 0 && !genres_PIC[i])
			{
				genres_PIC[i] = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_Genres/" + genres_ICONFILE[i]);
			}
			if ((bool)genres_PIC[i])
			{
				return genres_PIC[i];
			}
		}
		return guiMain_.uiSprites[3];
	}

	public bool IsErforscht(int i)
	{
		if (genres_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		return 100f / (float)genres_RES_POINTS[i] * ((float)genres_RES_POINTS[i] - genres_RES_POINTS_LEFT[i]);
	}

	public void MaxLevelAll()
	{
		for (int i = 0; i < genres_LEVEL.Length; i++)
		{
			genres_LEVEL[i] = 5;
		}
	}

	public void UnlockAll()
	{
		for (int i = 0; i < genres_UNLOCK.Length; i++)
		{
			genres_UNLOCK[i] = true;
		}
	}

	public void ResearchAll()
	{
		for (int i = 0; i < genres_UNLOCK.Length; i++)
		{
			if (genres_UNLOCK[i])
			{
				genres_RES_POINTS_LEFT[i] = 0f;
			}
		}
	}

	public bool ForschungGestartet(int i)
	{
		if (genres_RES_POINTS_LEFT[i] == (float)genres_RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool IsTargetGroup(int genre_, int group_)
	{
		if (genre_ < 0 || group_ < 0)
		{
			return false;
		}
		return genres_TARGETGROUP[genre_, group_];
	}

	public bool IsGenreCombination(int genre_, int subgenre_)
	{
		if (genre_ < 0 || subgenre_ < 0)
		{
			return false;
		}
		return genres_COMBINATION[genre_, subgenre_];
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(genres_PRICE[i]))
			{
				return false;
			}
			mS_.Pay(GetPrice(i), 2);
		}
		return true;
	}

	public bool BereitsInAnderenRaumAktiv(int s)
	{
		for (int i = 0; i < mS_.arrayRoomScripts.Length; i++)
		{
			if ((bool)mS_.arrayRoomScripts[i] && mS_.arrayRoomScripts[i].typ == 2 && (bool)mS_.arrayRoomScripts[i].taskGameObject)
			{
				taskForschung component = mS_.arrayRoomScripts[i].taskGameObject.GetComponent<taskForschung>();
				if ((bool)component && component.slot == s && component.typ == 0)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetTooltip(int i)
	{
		return string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat(string.Concat("<b><size=20>" + GetName(i) + "</size></b>", "\n", GetDesc(i)), "\n"), "\n<color=black><b>", tS_.GetText(2415), "</b></color>"), "\n<color=blue>", tS_.GetText(1479), "</color>"), "\n<size=19>", GetTooltipStars(genres_PLATFORM_SELLS[i, 0] / 20), "</size>"), "\n<color=blue>", tS_.GetText(1480), "</color>"), "\n<size=19>", GetTooltipStars(genres_PLATFORM_SELLS[i, 1] / 20), "</size>"), "\n<color=blue>", tS_.GetText(1481), "</color>"), "\n<size=19>", GetTooltipStars(genres_PLATFORM_SELLS[i, 2] / 20), "</size>"), "\n<color=blue>", tS_.GetText(1482), "</color>"), "\n<size=19>", GetTooltipStars(genres_PLATFORM_SELLS[i, 3] / 20), "</size>"), "\n<color=blue>", tS_.GetText(1797), "</color>"), "\n<size=19>", GetTooltipStars(genres_PLATFORM_SELLS[i, 4] / 20), "</size>");
	}

	private string GetTooltipStars(int i)
	{
		string text = "";
		return i switch
		{
			0 => "☆☆☆☆☆", 
			1 => "★☆☆☆☆", 
			2 => "★★☆☆☆", 
			3 => "★★★☆☆", 
			4 => "★★★★☆", 
			5 => "★★★★★", 
			_ => "☆☆☆☆☆", 
		};
	}

	public int GetAmountFans()
	{
		int num = 0;
		for (int i = 0; i < genres_FANS.Length; i++)
		{
			num += genres_FANS[i];
		}
		return num;
	}

	public string GetStringBeliebtheit(int i, bool kurz)
	{
		if (i == mS_.trendGenre)
		{
			if (!kurz)
			{
				return tS_.GetText(1381);
			}
			return "";
		}
		if (i == mS_.trendAntiGenre)
		{
			if (!kurz)
			{
				return tS_.GetText(1382);
			}
			return "";
		}
		return Mathf.RoundToInt(genres_BELIEBTHEIT[i]) + "%";
	}

	public float GetFloatBeliebtheit(int i)
	{
		if (i == mS_.trendGenre)
		{
			return 100f;
		}
		if (i == mS_.trendAntiGenre)
		{
			return 0f;
		}
		return genres_BELIEBTHEIT[i];
	}

	public float GetFloatPlatformSells(int genre_, int platTyp_)
	{
		return (float)genres_PLATFORM_SELLS[genre_, platTyp_] * 0.01f;
	}

	public bool GetFocusKnown(int slot, int maingenre, int subgenre)
	{
		if (mS_.settings_sandbox && mS_.sandbox_bekannteKonzeptEinstellungen)
		{
			return true;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = games_.arrayGamesScripts[i];
			if ((gameScript2.ownerID == mS_.myID || gameScript2.developerID == mS_.myID) && gameScript2.spielbericht && gameScript2.maingenre == maingenre && gameScript2.subgenre == subgenre && gameScript2.Designschwerpunkt[slot] == GetFocus(slot, maingenre, subgenre))
			{
				return true;
			}
		}
		return false;
	}

	public bool GetFocusTested(int slot, int maingenre, int subgenre, int wert)
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = games_.arrayGamesScripts[i];
			if ((gameScript2.ownerID == mS_.myID || gameScript2.developerID == mS_.myID) && gameScript2.spielbericht && gameScript2.maingenre == maingenre && gameScript2.subgenre == subgenre && gameScript2.Designschwerpunkt[slot] == wert)
			{
				return true;
			}
		}
		return false;
	}

	public int GetFocus(int slot, int maingenre, int subgenre)
	{
		int[] array = new int[8];
		for (int i = 0; i < array.Length; i++)
		{
			if (maingenre != -1)
			{
				array[i] += genres_FOCUS[maingenre, i];
			}
			if (subgenre != -1)
			{
				array[i] += genres_FOCUS[subgenre, i] / 2;
				float num = array[i];
				num /= 1.5f;
				array[i] = Mathf.RoundToInt(num);
			}
		}
		int num2 = 0;
		for (int j = 0; j < array.Length; j++)
		{
			num2 += array[j];
		}
		num2 = 40 - num2;
		if (num2 > 0)
		{
			while (num2 > 0)
			{
				for (int k = 0; k < array.Length; k++)
				{
					if (num2 > 0 && array[k] < 10)
					{
						array[k]++;
						num2--;
					}
				}
			}
		}
		if (num2 < 0)
		{
			while (num2 < 0)
			{
				for (int l = 0; l < array.Length; l++)
				{
					if (num2 < 0 && array[l] > 0)
					{
						array[l]--;
						num2++;
					}
				}
			}
		}
		return array[slot];
	}

	public bool GetAlignKnown(int slot, int maingenre, int subgenre)
	{
		if (mS_.settings_sandbox && mS_.sandbox_bekannteKonzeptEinstellungen)
		{
			return true;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = games_.arrayGamesScripts[i];
			if ((gameScript2.ownerID == mS_.myID || gameScript2.developerID == mS_.myID) && gameScript2.spielbericht && gameScript2.maingenre == maingenre && gameScript2.subgenre == subgenre && gameScript2.Designausrichtung[slot] == GetAlign(slot, maingenre, subgenre))
			{
				return true;
			}
		}
		return false;
	}

	public bool GetAlignTested(int slot, int maingenre, int subgenre, int wert)
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			gameScript gameScript2 = games_.arrayGamesScripts[i];
			if ((gameScript2.ownerID == mS_.myID || gameScript2.developerID == mS_.myID) && gameScript2.spielbericht && gameScript2.maingenre == maingenre && gameScript2.subgenre == subgenre && gameScript2.Designausrichtung[slot] == wert)
			{
				return true;
			}
		}
		return false;
	}

	public int GetAlign(int slot, int maingenre, int subgenre)
	{
		int[] array = new int[3];
		for (int i = 0; i < array.Length; i++)
		{
			if (maingenre != -1)
			{
				array[i] += genres_ALIGN[maingenre, i];
			}
			if (subgenre != -1)
			{
				array[i] += genres_ALIGN[subgenre, i] / 2;
				float num = array[i];
				num /= 1.5f;
				array[i] = Mathf.RoundToInt(num);
			}
		}
		return array[slot];
	}

	public Sprite GetSpriteMarkt(int i)
	{
		int num = genres_MARKT[i];
		if (num <= 5)
		{
			return themes_.spriteMarkt[0];
		}
		if (num > 5 && num < 10)
		{
			return themes_.spriteMarkt[1];
		}
		if (num >= 10)
		{
			return themes_.spriteMarkt[2];
		}
		return themes_.spriteMarkt[0];
	}

	public string GetStringMarktsaettigung(int i)
	{
		int num = genres_MARKT[i];
		if (num < 5)
		{
			return tS_.GetText(1908);
		}
		if (num >= 5 && num < 10)
		{
			return tS_.GetText(1909);
		}
		if (num >= 10)
		{
			return tS_.GetText(1910);
		}
		return tS_.GetText(1908);
	}
}
