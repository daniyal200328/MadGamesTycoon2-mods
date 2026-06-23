using System.IO;
using System.Text;
using UnityEngine;

public class hardware : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	public const int component_cpu = 0;

	public const int component_gfx = 1;

	public const int component_ram = 2;

	public const int component_hdd = 3;

	public const int component_sfx = 4;

	public const int component_cooling = 5;

	public const int component_disc = 6;

	public const int component_controller = 7;

	public const int component_case = 8;

	public const int component_monitor = 9;

	private Sprite[] hardware_PIC;

	public Sprite[] hardware_PICTYP;

	public string[] hardware_ICONFILE;

	public int[] hardware_TYP;

	public int[] hardware_RES_POINTS;

	public float[] hardware_RES_POINTS_LEFT;

	public int[] hardware_PRICE;

	public int[] hardware_DEV_COSTS;

	public int[] hardware_TECH;

	public int[] hardware_DATE_YEAR;

	public int[] hardware_DATE_MONTH;

	public bool[] hardware_UNLOCK;

	public bool[] hardware_ONLYSTATIONARY;

	public bool[] hardware_ONLYHANDHELD;

	public int[] hardware_NEED1;

	public int[] hardware_NEED2;

	public string[] hardware_NAME_EN;

	public string[] hardware_NAME_GE;

	public string[] hardware_NAME_TU;

	public string[] hardware_NAME_CH;

	public string[] hardware_NAME_FR;

	public string[] hardware_NAME_PB;

	public string[] hardware_NAME_CT;

	public string[] hardware_NAME_HU;

	public string[] hardware_NAME_ES;

	public string[] hardware_NAME_CZ;

	public string[] hardware_NAME_KO;

	public string[] hardware_NAME_AR;

	public string[] hardware_NAME_RU;

	public string[] hardware_NAME_IT;

	public string[] hardware_NAME_JA;

	public string[] hardware_NAME_PL;

	public string[] hardware_NAME_UA;

	public string[] hardware_NAME_TH;

	public string[] hardware_DESC_EN;

	public string[] hardware_DESC_GE;

	public string[] hardware_DESC_TU;

	public string[] hardware_DESC_CH;

	public string[] hardware_DESC_FR;

	public string[] hardware_DESC_PB;

	public string[] hardware_DESC_CT;

	public string[] hardware_DESC_HU;

	public string[] hardware_DESC_ES;

	public string[] hardware_DESC_CZ;

	public string[] hardware_DESC_KO;

	public string[] hardware_DESC_AR;

	public string[] hardware_DESC_RU;

	public string[] hardware_DESC_IT;

	public string[] hardware_DESC_JA;

	public string[] hardware_DESC_PL;

	public string[] hardware_DESC_UA;

	public string[] hardware_DESC_TH;

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
		hardware_PIC = new Sprite[hardware_UNLOCK.Length];
	}

	public void LoadHardwareKomponenten(string filename)
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
		hardware_ICONFILE = new string[num2];
		hardware_PIC = new Sprite[num2];
		hardware_TYP = new int[num2];
		hardware_RES_POINTS = new int[num2];
		hardware_RES_POINTS_LEFT = new float[num2];
		hardware_PRICE = new int[num2];
		hardware_DEV_COSTS = new int[num2];
		hardware_TECH = new int[num2];
		hardware_DATE_YEAR = new int[num2];
		hardware_DATE_MONTH = new int[num2];
		hardware_UNLOCK = new bool[num2];
		hardware_ONLYSTATIONARY = new bool[num2];
		hardware_ONLYHANDHELD = new bool[num2];
		hardware_NEED1 = new int[num2];
		hardware_NEED2 = new int[num2];
		hardware_NAME_EN = new string[num2];
		hardware_NAME_GE = new string[num2];
		hardware_NAME_TU = new string[num2];
		hardware_NAME_CH = new string[num2];
		hardware_NAME_FR = new string[num2];
		hardware_NAME_PB = new string[num2];
		hardware_NAME_CT = new string[num2];
		hardware_NAME_HU = new string[num2];
		hardware_NAME_ES = new string[num2];
		hardware_NAME_CZ = new string[num2];
		hardware_NAME_KO = new string[num2];
		hardware_NAME_AR = new string[num2];
		hardware_NAME_RU = new string[num2];
		hardware_NAME_IT = new string[num2];
		hardware_NAME_JA = new string[num2];
		hardware_NAME_PL = new string[num2];
		hardware_NAME_UA = new string[num2];
		hardware_NAME_TH = new string[num2];
		hardware_DESC_EN = new string[num2];
		hardware_DESC_GE = new string[num2];
		hardware_DESC_TU = new string[num2];
		hardware_DESC_CH = new string[num2];
		hardware_DESC_FR = new string[num2];
		hardware_DESC_PB = new string[num2];
		hardware_DESC_CT = new string[num2];
		hardware_DESC_HU = new string[num2];
		hardware_DESC_ES = new string[num2];
		hardware_DESC_CZ = new string[num2];
		hardware_DESC_KO = new string[num2];
		hardware_DESC_AR = new string[num2];
		hardware_DESC_RU = new string[num2];
		hardware_DESC_IT = new string[num2];
		hardware_DESC_JA = new string[num2];
		hardware_DESC_PL = new string[num2];
		hardware_DESC_UA = new string[num2];
		hardware_DESC_TH = new string[num2];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
			}
			if (ParseData("[TYP]", j))
			{
				hardware_TYP[num3] = int.Parse(data[j]);
			}
			if (ParseData("[RES POINTS]", j))
			{
				hardware_RES_POINTS[num3] = int.Parse(data[j]);
				hardware_RES_POINTS_LEFT[num3] = hardware_RES_POINTS[num3];
			}
			if (ParseData("[PRICE]", j))
			{
				hardware_PRICE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				hardware_DEV_COSTS[num3] = int.Parse(data[j]);
			}
			if (ParseData("[TECHLEVEL]", j))
			{
				hardware_TECH[num3] = int.Parse(data[j]);
			}
			if (ParseData("[ONLY_STATIONARY]", j))
			{
				hardware_ONLYSTATIONARY[num3] = true;
			}
			if (ParseData("[ONLY_HANDHELD]", j))
			{
				hardware_ONLYHANDHELD[num3] = true;
			}
			if (ParseData("[NEED-1]", j))
			{
				hardware_NEED1[num3] = int.Parse(data[j]);
			}
			if (ParseData("[NEED-2]", j))
			{
				hardware_NEED2[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					hardware_DATE_MONTH[num3] = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					hardware_DATE_MONTH[num3] = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					hardware_DATE_MONTH[num3] = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					hardware_DATE_MONTH[num3] = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					hardware_DATE_MONTH[num3] = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					hardware_DATE_MONTH[num3] = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					hardware_DATE_MONTH[num3] = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					hardware_DATE_MONTH[num3] = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					hardware_DATE_MONTH[num3] = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					hardware_DATE_MONTH[num3] = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					hardware_DATE_MONTH[num3] = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					hardware_DATE_MONTH[num3] = 12;
				}
				if (hardware_DATE_MONTH[num3] <= 0)
				{
					Debug.Log("ERROR: Hardware.txt -> Incorrect Month: " + hardware_NAME_EN[num3]);
				}
				hardware_DATE_YEAR[num3] = int.Parse(data[j]);
				if (hardware_DATE_YEAR[num3] == 1976 && hardware_DATE_MONTH[num3] == 1)
				{
					hardware_UNLOCK[num3] = true;
				}
			}
			if (ParseData("[PIC]", j))
			{
				hardware_ICONFILE[num3] = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				hardware_NAME_GE[num3] = data[j];
			}
			if (ParseData("[NAME EN]", j))
			{
				hardware_NAME_EN[num3] = data[j];
			}
			if (ParseData("[NAME TU]", j))
			{
				hardware_NAME_TU[num3] = data[j];
			}
			if (ParseData("[NAME CH]", j))
			{
				hardware_NAME_CH[num3] = data[j];
			}
			if (ParseData("[NAME FR]", j))
			{
				hardware_NAME_FR[num3] = data[j];
			}
			if (ParseData("[NAME PB]", j))
			{
				hardware_NAME_PB[num3] = data[j];
			}
			if (ParseData("[NAME CT]", j))
			{
				hardware_NAME_CT[num3] = data[j];
			}
			if (ParseData("[NAME HU]", j))
			{
				hardware_NAME_HU[num3] = data[j];
			}
			if (ParseData("[NAME ES]", j))
			{
				hardware_NAME_ES[num3] = data[j];
			}
			if (ParseData("[NAME CZ]", j))
			{
				hardware_NAME_CZ[num3] = data[j];
			}
			if (ParseData("[NAME KO]", j))
			{
				hardware_NAME_KO[num3] = data[j];
			}
			if (ParseData("[NAME AR]", j))
			{
				hardware_NAME_AR[num3] = data[j];
			}
			if (ParseData("[NAME RU]", j))
			{
				hardware_NAME_RU[num3] = data[j];
			}
			if (ParseData("[NAME IT]", j))
			{
				hardware_NAME_IT[num3] = data[j];
			}
			if (ParseData("[NAME JA]", j))
			{
				hardware_NAME_JA[num3] = data[j];
			}
			if (ParseData("[NAME PL]", j))
			{
				hardware_NAME_PL[num3] = data[j];
			}
			if (ParseData("[NAME UA]", j))
			{
				hardware_NAME_UA[num3] = data[j];
			}
			if (ParseData("[NAME TH]", j))
			{
				hardware_NAME_TH[num3] = data[j];
			}
			if (ParseData("[DESC GE]", j))
			{
				hardware_DESC_GE[num3] = data[j];
			}
			if (ParseData("[DESC EN]", j))
			{
				hardware_DESC_EN[num3] = data[j];
			}
			if (ParseData("[DESC TU]", j))
			{
				hardware_DESC_TU[num3] = data[j];
			}
			if (ParseData("[DESC CH]", j))
			{
				hardware_DESC_CH[num3] = data[j];
			}
			if (ParseData("[DESC FR]", j))
			{
				hardware_DESC_FR[num3] = data[j];
			}
			if (ParseData("[DESC PB]", j))
			{
				hardware_DESC_PB[num3] = data[j];
			}
			if (ParseData("[DESC CT]", j))
			{
				hardware_DESC_CT[num3] = data[j];
			}
			if (ParseData("[DESC HU]", j))
			{
				hardware_DESC_HU[num3] = data[j];
			}
			if (ParseData("[DESC ES]", j))
			{
				hardware_DESC_ES[num3] = data[j];
			}
			if (ParseData("[DESC CZ]", j))
			{
				hardware_DESC_CZ[num3] = data[j];
			}
			if (ParseData("[DESC KO]", j))
			{
				hardware_DESC_KO[num3] = data[j];
			}
			if (ParseData("[DESC AR]", j))
			{
				hardware_DESC_AR[num3] = data[j];
			}
			if (ParseData("[DESC RU]", j))
			{
				hardware_DESC_RU[num3] = data[j];
			}
			if (ParseData("[DESC IT]", j))
			{
				hardware_DESC_IT[num3] = data[j];
			}
			if (ParseData("[DESC JA]", j))
			{
				hardware_DESC_JA[num3] = data[j];
			}
			if (ParseData("[DESC PL]", j))
			{
				hardware_DESC_PL[num3] = data[j];
			}
			if (ParseData("[DESC UA]", j))
			{
				hardware_DESC_UA[num3] = data[j];
			}
			if (ParseData("[DESC TH]", j))
			{
				hardware_DESC_TH[num3] = data[j];
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
			text = hardware_NAME_EN[i];
			break;
		case 1:
			text = hardware_NAME_GE[i];
			break;
		case 2:
			if (hardware_NAME_TU.Length != 0)
			{
				text = hardware_NAME_TU[i];
			}
			break;
		case 3:
			if (hardware_NAME_CH.Length != 0)
			{
				text = hardware_NAME_CH[i];
			}
			break;
		case 4:
			if (hardware_NAME_FR.Length != 0)
			{
				text = hardware_NAME_FR[i];
			}
			break;
		case 5:
			if (hardware_NAME_ES.Length != 0)
			{
				text = hardware_NAME_ES[i];
			}
			break;
		case 6:
			if (hardware_NAME_KO.Length != 0)
			{
				text = hardware_NAME_KO[i];
			}
			break;
		case 7:
			if (hardware_NAME_PB.Length != 0)
			{
				text = hardware_NAME_PB[i];
			}
			break;
		case 8:
			if (hardware_NAME_HU.Length != 0)
			{
				text = hardware_NAME_HU[i];
			}
			break;
		case 9:
			if (hardware_NAME_RU.Length != 0)
			{
				text = hardware_NAME_RU[i];
			}
			break;
		case 10:
			if (hardware_NAME_CT.Length != 0)
			{
				text = hardware_NAME_CT[i];
			}
			break;
		case 11:
			if (hardware_NAME_PL.Length != 0)
			{
				text = hardware_NAME_PL[i];
			}
			break;
		case 12:
			if (hardware_NAME_CZ.Length != 0)
			{
				text = hardware_NAME_CZ[i];
			}
			break;
		case 13:
			if (hardware_NAME_AR.Length != 0)
			{
				text = hardware_NAME_AR[i];
			}
			break;
		case 14:
			if (hardware_NAME_IT.Length != 0)
			{
				text = hardware_NAME_IT[i];
			}
			break;
		case 16:
			if (hardware_NAME_JA.Length != 0)
			{
				text = hardware_NAME_JA[i];
			}
			break;
		case 17:
			if (hardware_NAME_UA.Length != 0)
			{
				text = hardware_NAME_UA[i];
			}
			break;
		case 19:
			if (hardware_NAME_TH.Length != 0)
			{
				text = hardware_NAME_TH[i];
			}
			break;
		default:
			text = hardware_NAME_EN[i];
			break;
		}
		if (text == null)
		{
			return hardware_NAME_EN[i];
		}
		if (text.Length <= 0)
		{
			return hardware_NAME_EN[i];
		}
		return text;
	}

	public string GetDesc(int i)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = hardware_DESC_EN[i];
			break;
		case 1:
			text = hardware_DESC_GE[i];
			break;
		case 2:
			if (hardware_DESC_TU.Length != 0)
			{
				text = hardware_DESC_TU[i];
			}
			break;
		case 3:
			if (hardware_DESC_CH.Length != 0)
			{
				text = hardware_DESC_CH[i];
			}
			break;
		case 4:
			if (hardware_DESC_FR.Length != 0)
			{
				text = hardware_DESC_FR[i];
			}
			break;
		case 5:
			if (hardware_DESC_ES.Length != 0)
			{
				text = hardware_DESC_ES[i];
			}
			break;
		case 6:
			if (hardware_DESC_KO.Length != 0)
			{
				text = hardware_DESC_KO[i];
			}
			break;
		case 7:
			if (hardware_DESC_PB.Length != 0)
			{
				text = hardware_DESC_PB[i];
			}
			break;
		case 8:
			if (hardware_DESC_HU.Length != 0)
			{
				text = hardware_DESC_HU[i];
			}
			break;
		case 9:
			if (hardware_DESC_RU.Length != 0)
			{
				text = hardware_DESC_RU[i];
			}
			break;
		case 10:
			if (hardware_DESC_CT.Length != 0)
			{
				text = hardware_DESC_CT[i];
			}
			break;
		case 11:
			if (hardware_DESC_PL.Length != 0)
			{
				text = hardware_DESC_PL[i];
			}
			break;
		case 12:
			if (hardware_DESC_CZ.Length != 0)
			{
				text = hardware_DESC_CZ[i];
			}
			break;
		case 13:
			if (hardware_DESC_AR.Length != 0)
			{
				text = hardware_DESC_AR[i];
			}
			break;
		case 14:
			if (hardware_DESC_IT.Length != 0)
			{
				text = hardware_DESC_IT[i];
			}
			break;
		case 16:
			if (hardware_DESC_JA.Length != 0)
			{
				text = hardware_DESC_JA[i];
			}
			break;
		case 17:
			if (hardware_DESC_UA.Length != 0)
			{
				text = hardware_DESC_UA[i];
			}
			break;
		case 19:
			if (hardware_DESC_TH.Length != 0)
			{
				text = hardware_DESC_TH[i];
			}
			break;
		default:
			text = hardware_DESC_EN[i];
			break;
		}
		if (text == null)
		{
			return "";
		}
		if (text.Length <= 0)
		{
			return hardware_DESC_EN[i];
		}
		return text;
	}

	public int GetDevCosts(int i)
	{
		return hardware_DEV_COSTS[i];
	}

	public int GetWorkPoints(int i)
	{
		return 100 + hardware_RES_POINTS[i];
	}

	public int GetPrice(int i)
	{
		return hardware_PRICE[i];
	}

	public int GetPerformance(int i)
	{
		return Mathf.RoundToInt(hardware_TECH[i] * (hardware_RES_POINTS[i] + 500) / 100);
	}

	public bool IsErforscht(int i)
	{
		if (hardware_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		return 100f / (float)hardware_RES_POINTS[i] * ((float)hardware_RES_POINTS[i] - hardware_RES_POINTS_LEFT[i]);
	}

	public void UnlockAll()
	{
		for (int i = 0; i < hardware_UNLOCK.Length; i++)
		{
			hardware_UNLOCK[i] = true;
		}
	}

	public bool ForschungGestartet(int i)
	{
		if (hardware_RES_POINTS_LEFT[i] == (float)hardware_RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(hardware_PRICE[i]))
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
				if ((bool)component && component.slot == s && component.typ == 4)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetDateString(int i)
	{
		return hardware_DATE_YEAR[i] + " " + tS_.GetText(hardware_DATE_MONTH[i] + 220);
	}

	public string GetTooltip(int i)
	{
		string text = "<b>" + GetName(i) + "</b>\n";
		text = text + "<color=magenta>" + GetTypString(hardware_TYP[i]) + "</color>";
		text = text + "\n" + tS_.GetText(217) + ": " + GetDateString(i);
		text = text + "\n" + tS_.GetText(1604) + ": " + GetPerformance(i);
		if (IsTechComponent(i))
		{
			text = text + "\n\n<color=green>" + tS_.GetText(1610) + "</color>";
			text = text + "\n<b><color=blue>" + tS_.GetText(4) + " " + hardware_TECH[i] + "</color></b>";
		}
		if (hardware_TYP[i] == 8 || hardware_TYP[i] == 7)
		{
			text = text + "\n\n<color=green>" + tS_.GetText(1611) + "</color>";
		}
		string desc = GetDesc(i);
		if (desc.Length > 0)
		{
			text = text + "\n\n" + desc;
		}
		if (hardware_ONLYSTATIONARY[i])
		{
			text = text + "\n\n<b><color=red>" + tS_.GetText(1603) + "</color></b>";
		}
		if (hardware_ONLYHANDHELD[i])
		{
			text = text + "\n\n<b><color=red>" + tS_.GetText(1602) + "</color></b>";
		}
		return text;
	}

	public Sprite GetTypPic(int i)
	{
		if (hardware_ICONFILE[i] == null)
		{
			return hardware_PICTYP[hardware_TYP[i]];
		}
		if (string.IsNullOrEmpty(hardware_ICONFILE[i]))
		{
			return hardware_PICTYP[hardware_TYP[i]];
		}
		if (hardware_ICONFILE[i].Length > 3)
		{
			if (hardware_PIC[i] == null)
			{
				hardware_PIC[i] = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_Hardware/" + hardware_ICONFILE[i]);
			}
			if ((bool)hardware_PIC[i])
			{
				return hardware_PIC[i];
			}
		}
		return hardware_PICTYP[hardware_TYP[i]];
	}

	public string GetTypString(int i)
	{
		string result = "";
		switch (i)
		{
		case 0:
			result = tS_.GetText(1588);
			break;
		case 1:
			result = tS_.GetText(1590);
			break;
		case 2:
			result = tS_.GetText(1589);
			break;
		case 3:
			result = tS_.GetText(1592);
			break;
		case 4:
			result = tS_.GetText(1591);
			break;
		case 5:
			result = tS_.GetText(1593);
			break;
		case 6:
			result = tS_.GetText(1594);
			break;
		case 7:
			result = tS_.GetText(1597);
			break;
		case 8:
			result = tS_.GetText(1598);
			break;
		case 9:
			result = tS_.GetText(1595);
			break;
		case 10:
			result = tS_.GetText(1599);
			break;
		}
		return result;
	}

	public bool IsTechComponent(int i)
	{
		if (hardware_TYP[i] == 0)
		{
			return true;
		}
		if (hardware_TYP[i] == 1)
		{
			return true;
		}
		if (hardware_TYP[i] == 2)
		{
			return true;
		}
		if (hardware_TYP[i] == 3)
		{
			return true;
		}
		if (hardware_TYP[i] == 4)
		{
			return true;
		}
		if (hardware_TYP[i] == 5)
		{
			return true;
		}
		if (hardware_TYP[i] == 6)
		{
			return true;
		}
		if (hardware_TYP[i] == 9)
		{
			return true;
		}
		return false;
	}

	public void ResearchAll()
	{
		for (int i = 0; i < hardware_RES_POINTS_LEFT.Length; i++)
		{
			hardware_RES_POINTS_LEFT[i] = 0f;
		}
	}
}
