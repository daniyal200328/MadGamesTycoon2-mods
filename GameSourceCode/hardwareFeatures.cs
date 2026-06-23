using System.IO;
using System.Text;
using UnityEngine;

public class hardwareFeatures : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	public Sprite hardFeatureSprite;

	private Sprite[] hardFeat_PIC;

	public string[] hardFeat_ICONFILE;

	public int[] hardFeat_RES_POINTS;

	public float[] hardFeat_RES_POINTS_LEFT;

	public int[] hardFeat_PRICE;

	public int[] hardFeat_DEV_COSTS;

	public int[] hardFeat_DATE_YEAR;

	public int[] hardFeat_DATE_MONTH;

	public bool[] hardFeat_UNLOCK;

	public bool[] hardFeat_ONLYSTATIONARY;

	public bool[] hardFeat_ONLYHANDHELD;

	public bool[] hardFeat_NEEDINTERNET;

	public float[] hardFeat_QUALITY;

	public string[] hardFeat_NAME_EN;

	public string[] hardFeat_NAME_GE;

	public string[] hardFeat_NAME_TU;

	public string[] hardFeat_NAME_CH;

	public string[] hardFeat_NAME_FR;

	public string[] hardFeat_NAME_PB;

	public string[] hardFeat_NAME_CT;

	public string[] hardFeat_NAME_HU;

	public string[] hardFeat_NAME_ES;

	public string[] hardFeat_NAME_CZ;

	public string[] hardFeat_NAME_KO;

	public string[] hardFeat_NAME_AR;

	public string[] hardFeat_NAME_RU;

	public string[] hardFeat_NAME_IT;

	public string[] hardFeat_NAME_JA;

	public string[] hardFeat_NAME_PL;

	public string[] hardFeat_NAME_UA;

	public string[] hardFeat_NAME_TH;

	public string[] hardFeat_DESC_EN;

	public string[] hardFeat_DESC_GE;

	public string[] hardFeat_DESC_TU;

	public string[] hardFeat_DESC_CH;

	public string[] hardFeat_DESC_FR;

	public string[] hardFeat_DESC_PB;

	public string[] hardFeat_DESC_CT;

	public string[] hardFeat_DESC_HU;

	public string[] hardFeat_DESC_ES;

	public string[] hardFeat_DESC_CZ;

	public string[] hardFeat_DESC_KO;

	public string[] hardFeat_DESC_AR;

	public string[] hardFeat_DESC_RU;

	public string[] hardFeat_DESC_IT;

	public string[] hardFeat_DESC_JA;

	public string[] hardFeat_DESC_PL;

	public string[] hardFeat_DESC_UA;

	public string[] hardFeat_DESC_TH;

	private string[] data;

	private void Awake()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		GameObject gameObject = GameObject.Find("Main");
		mS_ = gameObject.GetComponent<mainScript>();
		tS_ = gameObject.GetComponent<textScript>();
		settings_ = gameObject.GetComponent<settingsScript>();
	}

	public void Init()
	{
		hardFeat_PIC = new Sprite[hardFeat_UNLOCK.Length];
	}

	public void LoadHardwareFeatures(string filename)
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
		hardFeat_ICONFILE = new string[num2];
		hardFeat_PIC = new Sprite[num2];
		hardFeat_RES_POINTS = new int[num2];
		hardFeat_RES_POINTS_LEFT = new float[num2];
		hardFeat_PRICE = new int[num2];
		hardFeat_DEV_COSTS = new int[num2];
		hardFeat_DATE_YEAR = new int[num2];
		hardFeat_DATE_MONTH = new int[num2];
		hardFeat_UNLOCK = new bool[num2];
		hardFeat_ONLYSTATIONARY = new bool[num2];
		hardFeat_ONLYHANDHELD = new bool[num2];
		hardFeat_NEEDINTERNET = new bool[num2];
		hardFeat_QUALITY = new float[num2];
		hardFeat_NAME_EN = new string[num2];
		hardFeat_NAME_GE = new string[num2];
		hardFeat_NAME_TU = new string[num2];
		hardFeat_NAME_CH = new string[num2];
		hardFeat_NAME_FR = new string[num2];
		hardFeat_NAME_PB = new string[num2];
		hardFeat_NAME_CT = new string[num2];
		hardFeat_NAME_HU = new string[num2];
		hardFeat_NAME_ES = new string[num2];
		hardFeat_NAME_CZ = new string[num2];
		hardFeat_NAME_KO = new string[num2];
		hardFeat_NAME_AR = new string[num2];
		hardFeat_NAME_RU = new string[num2];
		hardFeat_NAME_IT = new string[num2];
		hardFeat_NAME_JA = new string[num2];
		hardFeat_NAME_PL = new string[num2];
		hardFeat_NAME_UA = new string[num2];
		hardFeat_NAME_TH = new string[num2];
		hardFeat_DESC_EN = new string[num2];
		hardFeat_DESC_GE = new string[num2];
		hardFeat_DESC_TU = new string[num2];
		hardFeat_DESC_CH = new string[num2];
		hardFeat_DESC_FR = new string[num2];
		hardFeat_DESC_PB = new string[num2];
		hardFeat_DESC_CT = new string[num2];
		hardFeat_DESC_HU = new string[num2];
		hardFeat_DESC_ES = new string[num2];
		hardFeat_DESC_CZ = new string[num2];
		hardFeat_DESC_KO = new string[num2];
		hardFeat_DESC_AR = new string[num2];
		hardFeat_DESC_RU = new string[num2];
		hardFeat_DESC_IT = new string[num2];
		hardFeat_DESC_JA = new string[num2];
		hardFeat_DESC_PL = new string[num2];
		hardFeat_DESC_UA = new string[num2];
		hardFeat_DESC_TH = new string[num2];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
			}
			if (ParseData("[RES POINTS]", j))
			{
				hardFeat_RES_POINTS[num3] = int.Parse(data[j]);
				hardFeat_RES_POINTS_LEFT[num3] = hardFeat_RES_POINTS[num3];
			}
			if (ParseData("[PRICE]", j))
			{
				hardFeat_PRICE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				hardFeat_DEV_COSTS[num3] = int.Parse(data[j]);
			}
			if (ParseData("[NEEDINTERNET]", j))
			{
				hardFeat_NEEDINTERNET[num3] = true;
			}
			if (ParseData("[QUALITY]", j))
			{
				hardFeat_QUALITY[num3] = int.Parse(data[j]);
			}
			if (ParseData("[ONLY_STATIONARY]", j))
			{
				hardFeat_ONLYSTATIONARY[num3] = true;
			}
			if (ParseData("[ONLY_HANDHELD]", j))
			{
				hardFeat_ONLYHANDHELD[num3] = true;
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					hardFeat_DATE_MONTH[num3] = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					hardFeat_DATE_MONTH[num3] = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					hardFeat_DATE_MONTH[num3] = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					hardFeat_DATE_MONTH[num3] = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					hardFeat_DATE_MONTH[num3] = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					hardFeat_DATE_MONTH[num3] = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					hardFeat_DATE_MONTH[num3] = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					hardFeat_DATE_MONTH[num3] = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					hardFeat_DATE_MONTH[num3] = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					hardFeat_DATE_MONTH[num3] = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					hardFeat_DATE_MONTH[num3] = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					hardFeat_DATE_MONTH[num3] = 12;
				}
				if (hardFeat_DATE_MONTH[num3] <= 0)
				{
					Debug.Log("ERROR: HardwareFeatures.txt -> Incorrect Month: " + hardFeat_NAME_EN[num3]);
				}
				hardFeat_DATE_YEAR[num3] = int.Parse(data[j]);
				if (hardFeat_DATE_YEAR[num3] == 1976 && hardFeat_DATE_MONTH[num3] == 1)
				{
					hardFeat_UNLOCK[num3] = true;
				}
			}
			if (ParseData("[PIC]", j))
			{
				hardFeat_ICONFILE[num3] = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				hardFeat_NAME_GE[num3] = data[j];
			}
			if (ParseData("[NAME EN]", j))
			{
				hardFeat_NAME_EN[num3] = data[j];
			}
			if (ParseData("[NAME TU]", j))
			{
				hardFeat_NAME_TU[num3] = data[j];
			}
			if (ParseData("[NAME CH]", j))
			{
				hardFeat_NAME_CH[num3] = data[j];
			}
			if (ParseData("[NAME FR]", j))
			{
				hardFeat_NAME_FR[num3] = data[j];
			}
			if (ParseData("[NAME PB]", j))
			{
				hardFeat_NAME_PB[num3] = data[j];
			}
			if (ParseData("[NAME CT]", j))
			{
				hardFeat_NAME_CT[num3] = data[j];
			}
			if (ParseData("[NAME HU]", j))
			{
				hardFeat_NAME_HU[num3] = data[j];
			}
			if (ParseData("[NAME ES]", j))
			{
				hardFeat_NAME_ES[num3] = data[j];
			}
			if (ParseData("[NAME CZ]", j))
			{
				hardFeat_NAME_CZ[num3] = data[j];
			}
			if (ParseData("[NAME KO]", j))
			{
				hardFeat_NAME_KO[num3] = data[j];
			}
			if (ParseData("[NAME AR]", j))
			{
				hardFeat_NAME_AR[num3] = data[j];
			}
			if (ParseData("[NAME RU]", j))
			{
				hardFeat_NAME_RU[num3] = data[j];
			}
			if (ParseData("[NAME IT]", j))
			{
				hardFeat_NAME_IT[num3] = data[j];
			}
			if (ParseData("[NAME JA]", j))
			{
				hardFeat_NAME_JA[num3] = data[j];
			}
			if (ParseData("[NAME PL]", j))
			{
				hardFeat_NAME_PL[num3] = data[j];
			}
			if (ParseData("[NAME UA]", j))
			{
				hardFeat_NAME_UA[num3] = data[j];
			}
			if (ParseData("[NAME TH]", j))
			{
				hardFeat_NAME_TH[num3] = data[j];
			}
			if (ParseData("[DESC GE]", j))
			{
				hardFeat_DESC_GE[num3] = data[j];
			}
			if (ParseData("[DESC EN]", j))
			{
				hardFeat_DESC_EN[num3] = data[j];
			}
			if (ParseData("[DESC TU]", j))
			{
				hardFeat_DESC_TU[num3] = data[j];
			}
			if (ParseData("[DESC CH]", j))
			{
				hardFeat_DESC_CH[num3] = data[j];
			}
			if (ParseData("[DESC FR]", j))
			{
				hardFeat_DESC_FR[num3] = data[j];
			}
			if (ParseData("[DESC PB]", j))
			{
				hardFeat_DESC_PB[num3] = data[j];
			}
			if (ParseData("[DESC CT]", j))
			{
				hardFeat_DESC_CT[num3] = data[j];
			}
			if (ParseData("[DESC HU]", j))
			{
				hardFeat_DESC_HU[num3] = data[j];
			}
			if (ParseData("[DESC ES]", j))
			{
				hardFeat_DESC_ES[num3] = data[j];
			}
			if (ParseData("[DESC CZ]", j))
			{
				hardFeat_DESC_CZ[num3] = data[j];
			}
			if (ParseData("[DESC KO]", j))
			{
				hardFeat_DESC_KO[num3] = data[j];
			}
			if (ParseData("[DESC AR]", j))
			{
				hardFeat_DESC_AR[num3] = data[j];
			}
			if (ParseData("[DESC RU]", j))
			{
				hardFeat_DESC_RU[num3] = data[j];
			}
			if (ParseData("[DESC IT]", j))
			{
				hardFeat_DESC_IT[num3] = data[j];
			}
			if (ParseData("[DESC JA]", j))
			{
				hardFeat_DESC_JA[num3] = data[j];
			}
			if (ParseData("[DESC PL]", j))
			{
				hardFeat_DESC_PL[num3] = data[j];
			}
			if (ParseData("[DESC UA]", j))
			{
				hardFeat_DESC_UA[num3] = data[j];
			}
			if (ParseData("[DESC TH]", j))
			{
				hardFeat_DESC_TH[num3] = data[j];
			}
			ParseData("//", j);
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
			text = hardFeat_NAME_EN[i];
			break;
		case 1:
			text = hardFeat_NAME_GE[i];
			break;
		case 2:
			if (hardFeat_NAME_TU.Length != 0)
			{
				text = hardFeat_NAME_TU[i];
			}
			break;
		case 3:
			if (hardFeat_NAME_CH.Length != 0)
			{
				text = hardFeat_NAME_CH[i];
			}
			break;
		case 4:
			if (hardFeat_NAME_FR.Length != 0)
			{
				text = hardFeat_NAME_FR[i];
			}
			break;
		case 5:
			if (hardFeat_NAME_ES.Length != 0)
			{
				text = hardFeat_NAME_ES[i];
			}
			break;
		case 6:
			if (hardFeat_NAME_KO.Length != 0)
			{
				text = hardFeat_NAME_KO[i];
			}
			break;
		case 7:
			if (hardFeat_NAME_PB.Length != 0)
			{
				text = hardFeat_NAME_PB[i];
			}
			break;
		case 8:
			if (hardFeat_NAME_HU.Length != 0)
			{
				text = hardFeat_NAME_HU[i];
			}
			break;
		case 9:
			if (hardFeat_NAME_RU.Length != 0)
			{
				text = hardFeat_NAME_RU[i];
			}
			break;
		case 10:
			if (hardFeat_NAME_CT.Length != 0)
			{
				text = hardFeat_NAME_CT[i];
			}
			break;
		case 11:
			if (hardFeat_NAME_PL.Length != 0)
			{
				text = hardFeat_NAME_PL[i];
			}
			break;
		case 12:
			if (hardFeat_NAME_CZ.Length != 0)
			{
				text = hardFeat_NAME_CZ[i];
			}
			break;
		case 13:
			if (hardFeat_NAME_AR.Length != 0)
			{
				text = hardFeat_NAME_AR[i];
			}
			break;
		case 14:
			if (hardFeat_NAME_IT.Length != 0)
			{
				text = hardFeat_NAME_IT[i];
			}
			break;
		case 16:
			if (hardFeat_NAME_JA.Length != 0)
			{
				text = hardFeat_NAME_JA[i];
			}
			break;
		case 17:
			if (hardFeat_NAME_UA.Length != 0)
			{
				text = hardFeat_NAME_UA[i];
			}
			break;
		case 19:
			if (hardFeat_NAME_TH.Length != 0)
			{
				text = hardFeat_NAME_TH[i];
			}
			break;
		default:
			text = hardFeat_NAME_EN[i];
			break;
		}
		if (text == null)
		{
			return hardFeat_NAME_EN[i];
		}
		if (text.Length <= 0)
		{
			return hardFeat_NAME_EN[i];
		}
		return text;
	}

	public string GetDesc(int i)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = hardFeat_DESC_EN[i];
			break;
		case 1:
			text = hardFeat_DESC_GE[i];
			break;
		case 2:
			if (hardFeat_DESC_TU.Length != 0)
			{
				text = hardFeat_DESC_TU[i];
			}
			break;
		case 3:
			if (hardFeat_DESC_CH.Length != 0)
			{
				text = hardFeat_DESC_CH[i];
			}
			break;
		case 4:
			if (hardFeat_DESC_FR.Length != 0)
			{
				text = hardFeat_DESC_FR[i];
			}
			break;
		case 5:
			if (hardFeat_DESC_ES.Length != 0)
			{
				text = hardFeat_DESC_ES[i];
			}
			break;
		case 6:
			if (hardFeat_DESC_KO.Length != 0)
			{
				text = hardFeat_DESC_KO[i];
			}
			break;
		case 7:
			if (hardFeat_DESC_PB.Length != 0)
			{
				text = hardFeat_DESC_PB[i];
			}
			break;
		case 8:
			if (hardFeat_DESC_HU.Length != 0)
			{
				text = hardFeat_DESC_HU[i];
			}
			break;
		case 9:
			if (hardFeat_DESC_RU.Length != 0)
			{
				text = hardFeat_DESC_RU[i];
			}
			break;
		case 10:
			if (hardFeat_DESC_CT.Length != 0)
			{
				text = hardFeat_DESC_CT[i];
			}
			break;
		case 11:
			if (hardFeat_DESC_PL.Length != 0)
			{
				text = hardFeat_DESC_PL[i];
			}
			break;
		case 12:
			if (hardFeat_DESC_CZ.Length != 0)
			{
				text = hardFeat_DESC_CZ[i];
			}
			break;
		case 13:
			if (hardFeat_DESC_AR.Length != 0)
			{
				text = hardFeat_DESC_AR[i];
			}
			break;
		case 14:
			if (hardFeat_DESC_IT.Length != 0)
			{
				text = hardFeat_DESC_IT[i];
			}
			break;
		case 16:
			if (hardFeat_DESC_JA.Length != 0)
			{
				text = hardFeat_DESC_JA[i];
			}
			break;
		case 17:
			if (hardFeat_DESC_UA.Length != 0)
			{
				text = hardFeat_DESC_UA[i];
			}
			break;
		case 19:
			if (hardFeat_DESC_TH.Length != 0)
			{
				text = hardFeat_DESC_TH[i];
			}
			break;
		default:
			text = hardFeat_DESC_EN[i];
			break;
		}
		if (text == null)
		{
			return "";
		}
		if (text.Length <= 0)
		{
			return hardFeat_DESC_EN[i];
		}
		return text;
	}

	public int GetDevCosts(int i)
	{
		return hardFeat_DEV_COSTS[i];
	}

	public int GetWorkPoints(int i)
	{
		float num = hardFeat_RES_POINTS[i];
		num *= 0.3f;
		return 100 + Mathf.RoundToInt(num);
	}

	public int GetPrice(int i)
	{
		return hardFeat_PRICE[i];
	}

	public bool IsErforscht(int i)
	{
		if (hardFeat_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		return 100f / (float)hardFeat_RES_POINTS[i] * ((float)hardFeat_RES_POINTS[i] - hardFeat_RES_POINTS_LEFT[i]);
	}

	public void UnlockAll()
	{
		for (int i = 0; i < hardFeat_UNLOCK.Length; i++)
		{
			hardFeat_UNLOCK[i] = true;
		}
	}

	public bool ForschungGestartet(int i)
	{
		if (hardFeat_RES_POINTS_LEFT[i] == (float)hardFeat_RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(hardFeat_PRICE[i]))
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
				if ((bool)component && component.slot == s && component.typ == 6)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetDateString(int i)
	{
		return hardFeat_DATE_YEAR[i] + " " + tS_.GetText(hardFeat_DATE_MONTH[i] + 220);
	}

	public string GetTooltip(int i)
	{
		string text = "<b>" + GetName(i) + "</b>\n";
		text = text + "<color=magenta>" + tS_.GetText(1599) + "</color>";
		text = text + "\n" + tS_.GetText(217) + ": " + GetDateString(i);
		string desc = GetDesc(i);
		if (desc.Length > 0)
		{
			text = text + "\n\n" + desc;
		}
		if (hardFeat_NEEDINTERNET[i])
		{
			text = text + "\n\n<b><color=blue>" + tS_.GetText(1618) + "</color></b>";
		}
		if (hardFeat_ONLYSTATIONARY[i])
		{
			text = text + "\n\n<b><color=red>" + tS_.GetText(1603) + "</color></b>";
		}
		if (hardFeat_ONLYHANDHELD[i])
		{
			text = text + "\n\n<b><color=red>" + tS_.GetText(1602) + "</color></b>";
		}
		return text;
	}

	public Sprite GetSprite(int i)
	{
		if (hardFeat_ICONFILE[i] == null)
		{
			return hardFeatureSprite;
		}
		if (string.IsNullOrEmpty(hardFeat_ICONFILE[i]))
		{
			return hardFeatureSprite;
		}
		if (hardFeat_ICONFILE[i].Length > 0)
		{
			if (hardFeat_PIC[i] == null)
			{
				hardFeat_PIC[i] = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_Hardware/" + hardFeat_ICONFILE[i]);
			}
			if ((bool)hardFeat_PIC[i])
			{
				return hardFeat_PIC[i];
			}
		}
		return hardFeatureSprite;
	}

	public void ResearchAll()
	{
		for (int i = 0; i < hardFeat_RES_POINTS_LEFT.Length; i++)
		{
			hardFeat_RES_POINTS_LEFT[i] = 0f;
		}
	}
}
