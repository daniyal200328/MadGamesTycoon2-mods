using System.IO;
using System.Text;
using UnityEngine;

public class gameplayFeatures : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private genres genres_;

	private Sprite[] gameplayFeatures_PIC;

	public Sprite[] gameplayfeatures_PICTYP;

	public int[] gameplayFeatures_TYP;

	public int[] gameplayFeatures_RES_POINTS;

	public float[] gameplayFeatures_RES_POINTS_LEFT;

	public int[] gameplayFeatures_PRICE;

	public int[] gameplayFeatures_DEV_COSTS;

	public int[] gameplayFeatures_DATE_YEAR;

	public int[] gameplayFeatures_DATE_MONTH;

	public int[] gameplayFeatures_GAMEPLAY;

	public int[] gameplayFeatures_GRAPHIC;

	public int[] gameplayFeatures_SOUND;

	public int[] gameplayFeatures_TECHNIK;

	public int[] gameplayFeatures_LEVEL;

	public int[] gameplayFeatures_NEED_GAMEPLAY_FEATURE;

	public bool[] gameplayFeatures_UNLOCK;

	public bool[] gameplayFeatures_INTERNET;

	public string[] gameplayFeatures_ICONFILE;

	public bool[,] gameplayFeatures_GOOD;

	public bool[,] gameplayFeatures_BAD;

	public bool[,] gameplayFeatures_LOCKPLATFORM;

	public string[] gameplayFeatures_NAME_EN;

	public string[] gameplayFeatures_NAME_GE;

	public string[] gameplayFeatures_NAME_TU;

	public string[] gameplayFeatures_NAME_CH;

	public string[] gameplayFeatures_NAME_FR;

	public string[] gameplayFeatures_NAME_PB;

	public string[] gameplayFeatures_NAME_CT;

	public string[] gameplayFeatures_NAME_HU;

	public string[] gameplayFeatures_NAME_ES;

	public string[] gameplayFeatures_NAME_CZ;

	public string[] gameplayFeatures_NAME_KO;

	public string[] gameplayFeatures_NAME_RU;

	public string[] gameplayFeatures_NAME_IT;

	public string[] gameplayFeatures_NAME_AR;

	public string[] gameplayFeatures_NAME_JA;

	public string[] gameplayFeatures_NAME_PL;

	public string[] gameplayFeatures_NAME_UA;

	public string[] gameplayFeatures_NAME_TH;

	public string[] gameplayFeatures_DESC_EN;

	public string[] gameplayFeatures_DESC_GE;

	public string[] gameplayFeatures_DESC_TU;

	public string[] gameplayFeatures_DESC_CH;

	public string[] gameplayFeatures_DESC_FR;

	public string[] gameplayFeatures_DESC_PB;

	public string[] gameplayFeatures_DESC_CT;

	public string[] gameplayFeatures_DESC_HU;

	public string[] gameplayFeatures_DESC_ES;

	public string[] gameplayFeatures_DESC_CZ;

	public string[] gameplayFeatures_DESC_KO;

	public string[] gameplayFeatures_DESC_RU;

	public string[] gameplayFeatures_DESC_IT;

	public string[] gameplayFeatures_DESC_AR;

	public string[] gameplayFeatures_DESC_JA;

	public string[] gameplayFeatures_DESC_PL;

	public string[] gameplayFeatures_DESC_UA;

	public string[] gameplayFeatures_DESC_TH;

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
		if (!genres_)
		{
			genres_ = GetComponent<genres>();
		}
	}

	public void Init()
	{
		gameplayFeatures_PIC = new Sprite[gameplayFeatures_UNLOCK.Length];
	}

	public bool[] Return1DimensionArray_GOOD()
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = genres_.genres_UNLOCK.Length;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = gameplayFeatures_GOOD[i, j];
			}
		}
		return array;
	}

	public bool[] Return1DimensionArray_BAD()
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = genres_.genres_UNLOCK.Length;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = gameplayFeatures_BAD[i, j];
			}
		}
		return array;
	}

	public bool[] Return1DimensionArray_LOCKPLATFORM()
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = 5;
		bool[] array = new bool[num * num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				array[i * num2 + j] = gameplayFeatures_LOCKPLATFORM[i, j];
			}
		}
		return array;
	}

	public void Copy2DimensionArray_GOOD(bool[] arr)
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = arr.Length / num;
		gameplayFeatures_GOOD = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				gameplayFeatures_GOOD[i, j] = arr[i * num2 + j];
			}
		}
	}

	public void Copy2DimensionArray_BAD(bool[] arr)
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = arr.Length / num;
		gameplayFeatures_BAD = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				gameplayFeatures_BAD[i, j] = arr[i * num2 + j];
			}
		}
	}

	public void Copy2DimensionArray_LOCKPLATFORM(bool[] arr)
	{
		int num = gameplayFeatures_UNLOCK.Length;
		int num2 = arr.Length / num;
		gameplayFeatures_LOCKPLATFORM = new bool[num, num2];
		for (int i = 0; i < num; i++)
		{
			for (int j = 0; j < num2; j++)
			{
				gameplayFeatures_LOCKPLATFORM[i, j] = arr[i * num2 + j];
			}
		}
	}

	public void LoadGameplayFeatures(string filename)
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
		gameplayFeatures_PIC = new Sprite[num2];
		gameplayFeatures_TYP = new int[num2];
		gameplayFeatures_RES_POINTS = new int[num2];
		gameplayFeatures_RES_POINTS_LEFT = new float[num2];
		gameplayFeatures_PRICE = new int[num2];
		gameplayFeatures_DEV_COSTS = new int[num2];
		gameplayFeatures_GAMEPLAY = new int[num2];
		gameplayFeatures_GRAPHIC = new int[num2];
		gameplayFeatures_SOUND = new int[num2];
		gameplayFeatures_TECHNIK = new int[num2];
		gameplayFeatures_DATE_YEAR = new int[num2];
		gameplayFeatures_DATE_MONTH = new int[num2];
		gameplayFeatures_LEVEL = new int[num2];
		gameplayFeatures_NEED_GAMEPLAY_FEATURE = new int[num2];
		gameplayFeatures_UNLOCK = new bool[num2];
		gameplayFeatures_INTERNET = new bool[num2];
		gameplayFeatures_ICONFILE = new string[num2];
		gameplayFeatures_GOOD = new bool[num2, genres_.genres_UNLOCK.Length];
		gameplayFeatures_BAD = new bool[num2, genres_.genres_UNLOCK.Length];
		gameplayFeatures_LOCKPLATFORM = new bool[num2, 5];
		gameplayFeatures_NAME_EN = new string[num2];
		gameplayFeatures_NAME_GE = new string[num2];
		gameplayFeatures_NAME_TU = new string[num2];
		gameplayFeatures_NAME_CH = new string[num2];
		gameplayFeatures_NAME_FR = new string[num2];
		gameplayFeatures_NAME_PB = new string[num2];
		gameplayFeatures_NAME_CT = new string[num2];
		gameplayFeatures_NAME_HU = new string[num2];
		gameplayFeatures_NAME_ES = new string[num2];
		gameplayFeatures_NAME_CZ = new string[num2];
		gameplayFeatures_NAME_KO = new string[num2];
		gameplayFeatures_NAME_RU = new string[num2];
		gameplayFeatures_NAME_IT = new string[num2];
		gameplayFeatures_NAME_AR = new string[num2];
		gameplayFeatures_NAME_JA = new string[num2];
		gameplayFeatures_NAME_PL = new string[num2];
		gameplayFeatures_NAME_UA = new string[num2];
		gameplayFeatures_NAME_TH = new string[num2];
		gameplayFeatures_DESC_EN = new string[num2];
		gameplayFeatures_DESC_GE = new string[num2];
		gameplayFeatures_DESC_TU = new string[num2];
		gameplayFeatures_DESC_CH = new string[num2];
		gameplayFeatures_DESC_FR = new string[num2];
		gameplayFeatures_DESC_PB = new string[num2];
		gameplayFeatures_DESC_CT = new string[num2];
		gameplayFeatures_DESC_HU = new string[num2];
		gameplayFeatures_DESC_ES = new string[num2];
		gameplayFeatures_DESC_CZ = new string[num2];
		gameplayFeatures_DESC_KO = new string[num2];
		gameplayFeatures_DESC_RU = new string[num2];
		gameplayFeatures_DESC_IT = new string[num2];
		gameplayFeatures_DESC_AR = new string[num2];
		gameplayFeatures_DESC_JA = new string[num2];
		gameplayFeatures_DESC_PL = new string[num2];
		gameplayFeatures_DESC_UA = new string[num2];
		gameplayFeatures_DESC_TH = new string[num2];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
				gameplayFeatures_NEED_GAMEPLAY_FEATURE[num3] = -1;
			}
			if (ParseData("[TYP]", j))
			{
				gameplayFeatures_TYP[num3] = int.Parse(data[j]);
			}
			if (ParseData("[RES POINTS]", j))
			{
				gameplayFeatures_RES_POINTS[num3] = int.Parse(data[j]);
				gameplayFeatures_RES_POINTS_LEFT[num3] = gameplayFeatures_RES_POINTS[num3];
			}
			if (ParseData("[PRICE]", j))
			{
				gameplayFeatures_PRICE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				gameplayFeatures_DEV_COSTS[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GAMEPLAY]", j))
			{
				gameplayFeatures_GAMEPLAY[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GRAPHIC]", j))
			{
				gameplayFeatures_GRAPHIC[num3] = int.Parse(data[j]);
			}
			if (ParseData("[SOUND]", j))
			{
				gameplayFeatures_SOUND[num3] = int.Parse(data[j]);
			}
			if (ParseData("[TECH]", j))
			{
				gameplayFeatures_TECHNIK[num3] = int.Parse(data[j]);
			}
			if (ParseData("[NEED_GPF]", j))
			{
				gameplayFeatures_NEED_GAMEPLAY_FEATURE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[INTERNET]", j))
			{
				gameplayFeatures_INTERNET[num3] = true;
			}
			if (ParseData("[NO_ARCADE]", j))
			{
				gameplayFeatures_LOCKPLATFORM[num3, 4] = true;
			}
			if (ParseData("[NO_MOBILE]", j))
			{
				gameplayFeatures_LOCKPLATFORM[num3, 3] = true;
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					gameplayFeatures_DATE_MONTH[num3] = 12;
				}
				if (gameplayFeatures_DATE_MONTH[num3] <= 0)
				{
					Debug.Log("ERROR: GameplayFeatures.txt -> Incorrect Month: " + gameplayFeatures_NAME_EN[num3]);
				}
				gameplayFeatures_DATE_YEAR[num3] = int.Parse(data[j]);
				if (gameplayFeatures_DATE_YEAR[num3] == 1976 && gameplayFeatures_DATE_MONTH[num3] == 1)
				{
					gameplayFeatures_UNLOCK[num3] = true;
				}
			}
			if (ParseData("[PIC]", j))
			{
				gameplayFeatures_ICONFILE[num3] = data[j];
			}
			if (ParseData("[GOOD]", j))
			{
				for (int k = 0; k < genres_.genres_UNLOCK.Length; k++)
				{
					if (data[j].Contains("<" + k + ">"))
					{
						gameplayFeatures_GOOD[num3, k] = true;
					}
				}
			}
			if (ParseData("[BAD]", j))
			{
				for (int l = 0; l < genres_.genres_UNLOCK.Length; l++)
				{
					if (data[j].Contains("<" + l + ">"))
					{
						gameplayFeatures_BAD[num3, l] = true;
					}
				}
			}
			if (ParseData("[NAME GE]", j))
			{
				gameplayFeatures_NAME_GE[num3] = data[j];
			}
			if (ParseData("[NAME EN]", j))
			{
				gameplayFeatures_NAME_EN[num3] = data[j];
			}
			if (ParseData("[NAME TU]", j))
			{
				gameplayFeatures_NAME_TU[num3] = data[j];
			}
			if (ParseData("[NAME CH]", j))
			{
				gameplayFeatures_NAME_CH[num3] = data[j];
			}
			if (ParseData("[NAME FR]", j))
			{
				gameplayFeatures_NAME_FR[num3] = data[j];
			}
			if (ParseData("[NAME PB]", j))
			{
				gameplayFeatures_NAME_PB[num3] = data[j];
			}
			if (ParseData("[NAME CT]", j))
			{
				gameplayFeatures_NAME_CT[num3] = data[j];
			}
			if (ParseData("[NAME HU]", j))
			{
				gameplayFeatures_NAME_HU[num3] = data[j];
			}
			if (ParseData("[NAME ES]", j))
			{
				gameplayFeatures_NAME_ES[num3] = data[j];
			}
			if (ParseData("[NAME CZ]", j))
			{
				gameplayFeatures_NAME_CZ[num3] = data[j];
			}
			if (ParseData("[NAME KO]", j))
			{
				gameplayFeatures_NAME_KO[num3] = data[j];
			}
			if (ParseData("[NAME RU]", j))
			{
				gameplayFeatures_NAME_RU[num3] = data[j];
			}
			if (ParseData("[NAME IT]", j))
			{
				gameplayFeatures_NAME_IT[num3] = data[j];
			}
			if (ParseData("[NAME AR]", j))
			{
				gameplayFeatures_NAME_AR[num3] = data[j];
			}
			if (ParseData("[NAME JA]", j))
			{
				gameplayFeatures_NAME_JA[num3] = data[j];
			}
			if (ParseData("[NAME PL]", j))
			{
				gameplayFeatures_NAME_PL[num3] = data[j];
			}
			if (ParseData("[NAME UA]", j))
			{
				gameplayFeatures_NAME_UA[num3] = data[j];
			}
			if (ParseData("[NAME TH]", j))
			{
				gameplayFeatures_NAME_TH[num3] = data[j];
			}
			if (ParseData("[DESC GE]", j))
			{
				gameplayFeatures_DESC_GE[num3] = data[j];
			}
			if (ParseData("[DESC EN]", j))
			{
				gameplayFeatures_DESC_EN[num3] = data[j];
			}
			if (ParseData("[DESC TU]", j))
			{
				gameplayFeatures_DESC_TU[num3] = data[j];
			}
			if (ParseData("[DESC CH]", j))
			{
				gameplayFeatures_DESC_CH[num3] = data[j];
			}
			if (ParseData("[DESC FR]", j))
			{
				gameplayFeatures_DESC_FR[num3] = data[j];
			}
			if (ParseData("[DESC PB]", j))
			{
				gameplayFeatures_DESC_PB[num3] = data[j];
			}
			if (ParseData("[DESC CT]", j))
			{
				gameplayFeatures_DESC_CT[num3] = data[j];
			}
			if (ParseData("[DESC HU]", j))
			{
				gameplayFeatures_DESC_HU[num3] = data[j];
			}
			if (ParseData("[DESC ES]", j))
			{
				gameplayFeatures_DESC_ES[num3] = data[j];
			}
			if (ParseData("[DESC CZ]", j))
			{
				gameplayFeatures_DESC_CZ[num3] = data[j];
			}
			if (ParseData("[DESC KO]", j))
			{
				gameplayFeatures_DESC_KO[num3] = data[j];
			}
			if (ParseData("[DESC RU]", j))
			{
				gameplayFeatures_DESC_RU[num3] = data[j];
			}
			if (ParseData("[DESC IT]", j))
			{
				gameplayFeatures_DESC_IT[num3] = data[j];
			}
			if (ParseData("[DESC AR]", j))
			{
				gameplayFeatures_DESC_AR[num3] = data[j];
			}
			if (ParseData("[DESC JA]", j))
			{
				gameplayFeatures_DESC_JA[num3] = data[j];
			}
			if (ParseData("[DESC PL]", j))
			{
				gameplayFeatures_DESC_PL[num3] = data[j];
			}
			if (ParseData("[DESC UA]", j))
			{
				gameplayFeatures_DESC_UA[num3] = data[j];
			}
			if (ParseData("[DESC TH]", j))
			{
				gameplayFeatures_DESC_TH[num3] = data[j];
			}
			if (!ParseData("[EOF]", j))
			{
				num++;
				continue;
			}
			break;
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

	public string GetName(int i)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = gameplayFeatures_NAME_EN[i];
			break;
		case 1:
			text = gameplayFeatures_NAME_GE[i];
			break;
		case 2:
			if (gameplayFeatures_NAME_TU.Length != 0)
			{
				text = gameplayFeatures_NAME_TU[i];
			}
			break;
		case 3:
			if (gameplayFeatures_NAME_CH.Length != 0)
			{
				text = gameplayFeatures_NAME_CH[i];
			}
			break;
		case 4:
			if (gameplayFeatures_NAME_FR.Length != 0)
			{
				text = gameplayFeatures_NAME_FR[i];
			}
			break;
		case 5:
			if (gameplayFeatures_NAME_ES.Length != 0)
			{
				text = gameplayFeatures_NAME_ES[i];
			}
			break;
		case 6:
			if (gameplayFeatures_NAME_KO.Length != 0)
			{
				text = gameplayFeatures_NAME_KO[i];
			}
			break;
		case 7:
			if (gameplayFeatures_NAME_PB.Length != 0)
			{
				text = gameplayFeatures_NAME_PB[i];
			}
			break;
		case 8:
			if (gameplayFeatures_NAME_HU.Length != 0)
			{
				text = gameplayFeatures_NAME_HU[i];
			}
			break;
		case 9:
			if (gameplayFeatures_NAME_RU.Length != 0)
			{
				text = gameplayFeatures_NAME_RU[i];
			}
			break;
		case 10:
			if (gameplayFeatures_NAME_CT.Length != 0)
			{
				text = gameplayFeatures_NAME_CT[i];
			}
			break;
		case 11:
			if (gameplayFeatures_NAME_PL.Length != 0)
			{
				text = gameplayFeatures_NAME_PL[i];
			}
			break;
		case 12:
			if (gameplayFeatures_NAME_CZ.Length != 0)
			{
				text = gameplayFeatures_NAME_CZ[i];
			}
			break;
		case 13:
			if (gameplayFeatures_NAME_AR.Length != 0)
			{
				text = gameplayFeatures_NAME_AR[i];
			}
			break;
		case 14:
			if (gameplayFeatures_NAME_IT.Length != 0)
			{
				text = gameplayFeatures_NAME_IT[i];
			}
			break;
		case 16:
			if (gameplayFeatures_NAME_JA.Length != 0)
			{
				text = gameplayFeatures_NAME_JA[i];
			}
			break;
		case 17:
			if (gameplayFeatures_NAME_UA.Length != 0)
			{
				text = gameplayFeatures_NAME_UA[i];
			}
			break;
		case 19:
			if (gameplayFeatures_NAME_TH.Length != 0)
			{
				text = gameplayFeatures_NAME_TH[i];
			}
			break;
		default:
			text = gameplayFeatures_NAME_EN[i];
			break;
		}
		if (text == null)
		{
			return gameplayFeatures_NAME_EN[i];
		}
		if (text.Length <= 0)
		{
			return gameplayFeatures_NAME_EN[i];
		}
		return text;
	}

	public string GetDesc(int i)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = gameplayFeatures_DESC_EN[i];
			break;
		case 1:
			text = gameplayFeatures_DESC_GE[i];
			break;
		case 2:
			if (gameplayFeatures_DESC_TU.Length != 0)
			{
				text = gameplayFeatures_DESC_TU[i];
			}
			break;
		case 3:
			if (gameplayFeatures_DESC_CH.Length != 0)
			{
				text = gameplayFeatures_DESC_CH[i];
			}
			break;
		case 4:
			if (gameplayFeatures_DESC_FR.Length != 0)
			{
				text = gameplayFeatures_DESC_FR[i];
			}
			break;
		case 5:
			if (gameplayFeatures_DESC_ES.Length != 0)
			{
				text = gameplayFeatures_DESC_ES[i];
			}
			break;
		case 6:
			if (gameplayFeatures_DESC_KO.Length != 0)
			{
				text = gameplayFeatures_DESC_KO[i];
			}
			break;
		case 7:
			if (gameplayFeatures_DESC_PB.Length != 0)
			{
				text = gameplayFeatures_DESC_PB[i];
			}
			break;
		case 8:
			if (gameplayFeatures_DESC_HU.Length != 0)
			{
				text = gameplayFeatures_DESC_HU[i];
			}
			break;
		case 9:
			if (gameplayFeatures_DESC_RU.Length != 0)
			{
				text = gameplayFeatures_DESC_RU[i];
			}
			break;
		case 10:
			if (gameplayFeatures_DESC_CT.Length != 0)
			{
				text = gameplayFeatures_DESC_CT[i];
			}
			break;
		case 11:
			if (gameplayFeatures_DESC_PL.Length != 0)
			{
				text = gameplayFeatures_DESC_PL[i];
			}
			break;
		case 12:
			if (gameplayFeatures_DESC_CZ.Length != 0)
			{
				text = gameplayFeatures_DESC_CZ[i];
			}
			break;
		case 13:
			if (gameplayFeatures_DESC_AR.Length != 0)
			{
				text = gameplayFeatures_DESC_AR[i];
			}
			break;
		case 14:
			if (gameplayFeatures_DESC_IT.Length != 0)
			{
				text = gameplayFeatures_DESC_IT[i];
			}
			break;
		case 16:
			if (gameplayFeatures_DESC_JA.Length != 0)
			{
				text = gameplayFeatures_DESC_JA[i];
			}
			break;
		case 17:
			if (gameplayFeatures_DESC_UA.Length != 0)
			{
				text = gameplayFeatures_DESC_UA[i];
			}
			break;
		case 19:
			if (gameplayFeatures_DESC_TH.Length != 0)
			{
				text = gameplayFeatures_DESC_TH[i];
			}
			break;
		default:
			text = gameplayFeatures_DESC_EN[i];
			break;
		}
		if (text == null)
		{
			return gameplayFeatures_DESC_EN[i];
		}
		if (text.Length <= 0)
		{
			return gameplayFeatures_DESC_EN[i];
		}
		return text;
	}

	public float GetBonus(int i, int maingenre_, int subgenre_)
	{
		if (maingenre_ != -1 && subgenre_ != -1)
		{
			if (gameplayFeatures_GOOD[i, maingenre_])
			{
				return 1.5f;
			}
			if (gameplayFeatures_BAD[i, maingenre_] && gameplayFeatures_BAD[i, subgenre_])
			{
				return 0.1f;
			}
			if (gameplayFeatures_BAD[i, maingenre_] && !gameplayFeatures_BAD[i, subgenre_] && !gameplayFeatures_GOOD[i, subgenre_])
			{
				return 1f;
			}
			if (gameplayFeatures_BAD[i, maingenre_] && !gameplayFeatures_BAD[i, subgenre_] && gameplayFeatures_GOOD[i, subgenre_])
			{
				return 1f;
			}
			if (!gameplayFeatures_BAD[i, maingenre_] && !gameplayFeatures_GOOD[i, maingenre_] && gameplayFeatures_BAD[i, subgenre_])
			{
				return 0.1f;
			}
			if (!gameplayFeatures_BAD[i, maingenre_] && !gameplayFeatures_GOOD[i, maingenre_] && gameplayFeatures_GOOD[i, subgenre_])
			{
				return 1.5f;
			}
		}
		if (maingenre_ != -1)
		{
			if (gameplayFeatures_GOOD[i, maingenre_])
			{
				return 1.5f;
			}
			if (gameplayFeatures_BAD[i, maingenre_])
			{
				return 0.1f;
			}
		}
		return 1f;
	}

	public int GetGameplay(int i, int maingenre_, int subgenre_)
	{
		if (gameplayFeatures_GAMEPLAY[i] <= 0)
		{
			return gameplayFeatures_GAMEPLAY[i];
		}
		float num = (float)gameplayFeatures_LEVEL[i] * 0.1f;
		num = (float)gameplayFeatures_GAMEPLAY[i] * (1f + num);
		num *= GetBonus(i, maingenre_, subgenre_);
		return Mathf.RoundToInt(num);
	}

	public int GetGraphic(int i, int maingenre_, int subgenre_)
	{
		if (gameplayFeatures_GRAPHIC[i] <= 0)
		{
			return gameplayFeatures_GRAPHIC[i];
		}
		float num = (float)gameplayFeatures_LEVEL[i] * 0.1f;
		num = (float)gameplayFeatures_GRAPHIC[i] * (1f + num);
		num *= GetBonus(i, maingenre_, subgenre_);
		return Mathf.RoundToInt(num);
	}

	public int GetSound(int i, int maingenre_, int subgenre_)
	{
		if (gameplayFeatures_SOUND[i] <= 0)
		{
			return gameplayFeatures_SOUND[i];
		}
		float num = (float)gameplayFeatures_LEVEL[i] * 0.1f;
		num = (float)gameplayFeatures_SOUND[i] * (1f + num);
		num *= GetBonus(i, maingenre_, subgenre_);
		return Mathf.RoundToInt(num);
	}

	public int GetTechnik(int i, int maingenre_, int subgenre_)
	{
		if (gameplayFeatures_TECHNIK[i] <= 0)
		{
			return gameplayFeatures_TECHNIK[i];
		}
		float num = (float)gameplayFeatures_LEVEL[i] * 0.1f;
		num = (float)gameplayFeatures_TECHNIK[i] * (1f + num);
		num *= GetBonus(i, maingenre_, subgenre_);
		return Mathf.RoundToInt(num);
	}

	public int GetDevCosts(int i)
	{
		float num = (float)gameplayFeatures_LEVEL[i] * 0.1f;
		num = (float)gameplayFeatures_DEV_COSTS[i] * (1f - num);
		return Mathf.RoundToInt(num);
	}

	public int GetTypGrafik()
	{
		return 0;
	}

	public int GetTypSound()
	{
		return 1;
	}

	public int GetTypKI()
	{
		return 2;
	}

	public int GetTypPhysik()
	{
		return 3;
	}

	public int GetTypGameplay()
	{
		return 4;
	}

	public int GetTypSteuerung()
	{
		return 5;
	}

	public int GetTypMultiplayer()
	{
		return 6;
	}

	public Sprite GetTypSprite(int i)
	{
		if (gameplayFeatures_ICONFILE[i] == null)
		{
			return gameplayfeatures_PICTYP[gameplayFeatures_TYP[i]];
		}
		if (string.IsNullOrEmpty(gameplayFeatures_ICONFILE[i]))
		{
			return gameplayfeatures_PICTYP[gameplayFeatures_TYP[i]];
		}
		if (gameplayFeatures_ICONFILE[i].Length > 0)
		{
			if (gameplayFeatures_PIC[i] == null)
			{
				gameplayFeatures_PIC[i] = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_GameplayFeatures/" + gameplayFeatures_ICONFILE[i]);
			}
			if ((bool)gameplayFeatures_PIC[i])
			{
				return gameplayFeatures_PIC[i];
			}
		}
		return gameplayfeatures_PICTYP[gameplayFeatures_TYP[i]];
	}

	public int GetPrice(int i)
	{
		return gameplayFeatures_PRICE[i];
	}

	public bool IsErforscht(int i)
	{
		if (gameplayFeatures_RES_POINTS_LEFT.Length < i + 1)
		{
			return false;
		}
		if (gameplayFeatures_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		return 100f / (float)gameplayFeatures_RES_POINTS[i] * ((float)gameplayFeatures_RES_POINTS[i] - gameplayFeatures_RES_POINTS_LEFT[i]);
	}

	public int GetDevPoints(int i)
	{
		return 10 + gameplayFeatures_RES_POINTS[i] / 10;
	}

	public void UnlockAll()
	{
		for (int i = 0; i < gameplayFeatures_UNLOCK.Length; i++)
		{
			gameplayFeatures_UNLOCK[i] = true;
			gameplayFeatures_RES_POINTS_LEFT[i] = 0f;
		}
	}

	public void MaxLevelAll()
	{
		for (int i = 0; i < gameplayFeatures_LEVEL.Length; i++)
		{
			gameplayFeatures_LEVEL[i] = 5;
		}
	}

	public bool ForschungGestartet(int i)
	{
		if (gameplayFeatures_RES_POINTS_LEFT[i] == (float)gameplayFeatures_RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(gameplayFeatures_PRICE[i]))
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
				if ((bool)component && component.slot == s && component.typ == 3)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetDateString(int i)
	{
		return gameplayFeatures_DATE_YEAR[i] + " " + tS_.GetText(gameplayFeatures_DATE_MONTH[i] + 220);
	}

	public string GetTooltip(int i, int maingenre_, int subgenre_)
	{
		string text = "<b>" + GetName(i) + "</b>\n";
		switch (gameplayFeatures_TYP[i])
		{
		case 0:
			text = text + "<color=magenta>" + tS_.GetText(9) + "</color>";
			break;
		case 1:
			text = text + "<color=magenta>" + tS_.GetText(10) + "</color>";
			break;
		case 2:
			text = text + "<color=magenta>" + tS_.GetText(11) + "</color>";
			break;
		case 3:
			text = text + "<color=magenta>" + tS_.GetText(12) + "</color>";
			break;
		case 4:
			text = text + "<color=magenta>" + tS_.GetText(13) + "</color>";
			break;
		case 5:
			text = text + "<color=magenta>" + tS_.GetText(14) + "</color>";
			break;
		case 6:
			text = text + "<color=magenta>" + tS_.GetText(15) + "</color>";
			break;
		}
		text = text + "\n" + GetDateString(i) + "\n";
		text = text + "\n" + GetDesc(i) + "\n";
		string text2 = tS_.GetText(254);
		text2 = text2.Replace("<NUM>", GetGameplay(i, maingenre_, subgenre_).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(255);
		text2 = text2.Replace("<NUM>", GetGraphic(i, maingenre_, subgenre_).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(256);
		text2 = text2.Replace("<NUM>", GetSound(i, maingenre_, subgenre_).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(257);
		text2 = text2.Replace("<NUM>", GetTechnik(i, maingenre_, subgenre_).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text += "\n";
		text += "\n<color=green>";
		for (int j = 0; j < genres_.genres_LEVEL.Length; j++)
		{
			if (gameplayFeatures_GOOD[i, j])
			{
				text = text + genres_.GetName(j) + "\n";
			}
		}
		text += "</color>";
		text += "\n<color=red>";
		for (int k = 0; k < genres_.genres_LEVEL.Length; k++)
		{
			if (gameplayFeatures_BAD[i, k])
			{
				text = text + genres_.GetName(k) + "\n";
			}
		}
		text += "</color>";
		if (gameplayFeatures_NEED_GAMEPLAY_FEATURE[i] > -1)
		{
			text2 = tS_.GetText(919);
			text2 = text2.Replace("color=yellow", "color=magenta");
			text2 = text2.Replace("<NAME>", "\n" + GetName(gameplayFeatures_NEED_GAMEPLAY_FEATURE[i]));
			text = text + "\n" + text2;
		}
		if (gameplayFeatures_INTERNET[i])
		{
			text = text + "\n\n<color=blue>" + tS_.GetText(1618) + "</color>";
		}
		return text + "\n\n<b><color=red>" + tS_.GetText(6) + ": " + mS_.GetMoney(GetDevCosts(i), showDollar: true) + "</color></b>";
	}
}
