using System.IO;
using System.Text;
using UnityEngine;

public class anitCheat : MonoBehaviour
{
	public GameObject prefabAntiCheat;

	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

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
	}

	public antiCheatScript CreateAntiCheat()
	{
		antiCheatScript component = Object.Instantiate(prefabAntiCheat).GetComponent<antiCheatScript>();
		component.main_ = base.gameObject;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.settings_ = settings_;
		return component;
	}

	public void LoadAnitCheat(string filename)
	{
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
		antiCheatScript antiCheatScript2 = null;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				antiCheatScript2 = CreateAntiCheat();
				antiCheatScript2.myID = int.Parse(data[j]);
				antiCheatScript2.Init();
			}
			if (!antiCheatScript2)
			{
				continue;
			}
			if (ParseData("[PRICE]", j))
			{
				antiCheatScript2.price = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				antiCheatScript2.dev_costs = int.Parse(data[j]);
			}
			if (ParseData("[ENDLESS]", j))
			{
				antiCheatScript2.neverLooseEffect = true;
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					antiCheatScript2.date_month = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					antiCheatScript2.date_month = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					antiCheatScript2.date_month = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					antiCheatScript2.date_month = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					antiCheatScript2.date_month = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					antiCheatScript2.date_month = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					antiCheatScript2.date_month = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					antiCheatScript2.date_month = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					antiCheatScript2.date_month = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					antiCheatScript2.date_month = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					antiCheatScript2.date_month = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					antiCheatScript2.date_month = 12;
				}
				if (antiCheatScript2.date_month <= 0)
				{
					Debug.Log("ERROR: AntiCheat.txt -> Incorrect Month: " + antiCheatScript2.myID);
				}
				antiCheatScript2.date_year = int.Parse(data[j]);
			}
			if (ParseData("[NAME EN]", j))
			{
				antiCheatScript2.name_EN = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				antiCheatScript2.name_GE = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_GE)
				{
					antiCheatScript2.name_GE = "";
				}
			}
			if (ParseData("[NAME TU]", j))
			{
				antiCheatScript2.name_TU = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_TU)
				{
					antiCheatScript2.name_TU = "";
				}
			}
			if (ParseData("[NAME CH]", j))
			{
				antiCheatScript2.name_CH = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_CH)
				{
					antiCheatScript2.name_CH = "";
				}
			}
			if (ParseData("[NAME FR]", j))
			{
				antiCheatScript2.name_FR = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_FR)
				{
					antiCheatScript2.name_FR = "";
				}
			}
			if (ParseData("[NAME CT]", j))
			{
				antiCheatScript2.name_CT = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_CT)
				{
					antiCheatScript2.name_CT = "";
				}
			}
			if (ParseData("[NAME RU]", j))
			{
				antiCheatScript2.name_RU = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_RU)
				{
					antiCheatScript2.name_RU = "";
				}
			}
			if (ParseData("[NAME IT]", j))
			{
				antiCheatScript2.name_IT = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_IT)
				{
					antiCheatScript2.name_IT = "";
				}
			}
			if (ParseData("[NAME JA]", j))
			{
				antiCheatScript2.name_JA = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_JA)
				{
					antiCheatScript2.name_JA = "";
				}
			}
			if (ParseData("[NAME UA]", j))
			{
				antiCheatScript2.name_UA = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_UA)
				{
					antiCheatScript2.name_UA = "";
				}
			}
			if (ParseData("[NAME TH]", j))
			{
				antiCheatScript2.name_TH = data[j];
				if (antiCheatScript2.name_EN == antiCheatScript2.name_TH)
				{
					antiCheatScript2.name_TH = "";
				}
			}
			if (ParseData("[EOF]", j))
			{
				break;
			}
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

	public void UpdateEffekt()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("AntiCheat");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				antiCheatScript component = array[i].GetComponent<antiCheatScript>();
				if ((bool)component)
				{
					component.EffektVerschlechtern();
				}
			}
		}
	}
}
