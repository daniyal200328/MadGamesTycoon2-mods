using System.IO;
using System.Text;
using UnityEngine;

public class npcEngines : MonoBehaviour
{
	public GameObject prefabEngine;

	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private engineFeatures eF_;

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
		if (!eF_)
		{
			eF_ = GetComponent<engineFeatures>();
		}
	}

	public void LoadNpcEngines(string filename)
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
		engineScript engineScript2 = null;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				engineScript2 = eF_.CreateEngine();
				engineScript2.myID = int.Parse(data[j]);
				engineScript2.ownerID = -1;
				engineScript2.sellEngine = true;
				engineScript2.devPoints = 0f;
				engineScript2.Init();
				if (engineScript2.myID == 0)
				{
					engineScript2.gekauft = true;
					engineScript2.isUnlocked = true;
					engineScript2.InitNpcEngine();
				}
			}
			if (!engineScript2)
			{
				continue;
			}
			if (ParseData("[PRICE]", j))
			{
				engineScript2.preis = int.Parse(data[j]);
			}
			if (ParseData("[GENRE]", j))
			{
				engineScript2.spezialgenre = int.Parse(data[j]);
			}
			if (ParseData("[PLATFORM]", j))
			{
				engineScript2.spezialplatform = int.Parse(data[j]);
			}
			if (ParseData("[SHARE]", j))
			{
				engineScript2.gewinnbeteiligung = int.Parse(data[j]);
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					engineScript2.date_month = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					engineScript2.date_month = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					engineScript2.date_month = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					engineScript2.date_month = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					engineScript2.date_month = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					engineScript2.date_month = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					engineScript2.date_month = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					engineScript2.date_month = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					engineScript2.date_month = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					engineScript2.date_month = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					engineScript2.date_month = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					engineScript2.date_month = 12;
				}
				if (engineScript2.date_month <= 0)
				{
					Debug.Log("ERROR: NpcEngines.txt -> Incorrect Month: " + engineScript2.myID);
				}
				engineScript2.date_year = int.Parse(data[j]);
			}
			if (ParseData("[NAME EN]", j))
			{
				engineScript2.name_EN = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				engineScript2.name_GE = data[j];
				if (engineScript2.name_EN == engineScript2.name_GE)
				{
					engineScript2.name_GE = "";
				}
			}
			if (ParseData("[NAME TU]", j))
			{
				engineScript2.name_TU = data[j];
				if (engineScript2.name_EN == engineScript2.name_TU)
				{
					engineScript2.name_TU = "";
				}
			}
			if (ParseData("[NAME CH]", j))
			{
				engineScript2.name_CH = data[j];
				if (engineScript2.name_EN == engineScript2.name_CH)
				{
					engineScript2.name_CH = "";
				}
			}
			if (ParseData("[NAME FR]", j))
			{
				engineScript2.name_FR = data[j];
				if (engineScript2.name_EN == engineScript2.name_FR)
				{
					engineScript2.name_FR = "";
				}
			}
			if (ParseData("[NAME HU]", j))
			{
				engineScript2.name_HU = data[j];
				if (engineScript2.name_EN == engineScript2.name_HU)
				{
					engineScript2.name_HU = "";
				}
			}
			if (ParseData("[NAME CT]", j))
			{
				engineScript2.name_CT = data[j];
				if (engineScript2.name_EN == engineScript2.name_CT)
				{
					engineScript2.name_CT = "";
				}
			}
			if (ParseData("[NAME CZ]", j))
			{
				engineScript2.name_CZ = data[j];
				if (engineScript2.name_EN == engineScript2.name_CZ)
				{
					engineScript2.name_CZ = "";
				}
			}
			if (ParseData("[NAME PB]", j))
			{
				engineScript2.name_PB = data[j];
				if (engineScript2.name_EN == engineScript2.name_PB)
				{
					engineScript2.name_PB = "";
				}
			}
			if (ParseData("[NAME IT]", j))
			{
				engineScript2.name_IT = data[j];
				if (engineScript2.name_EN == engineScript2.name_IT)
				{
					engineScript2.name_IT = "";
				}
			}
			if (ParseData("[NAME JA]", j))
			{
				engineScript2.name_JA = data[j];
				if (engineScript2.name_EN == engineScript2.name_JA)
				{
					engineScript2.name_JA = "";
				}
			}
			if (ParseData("[NAME PL]", j))
			{
				engineScript2.name_PL = data[j];
				if (engineScript2.name_EN == engineScript2.name_PL)
				{
					engineScript2.name_PL = "";
				}
			}
			if (ParseData("[NAME UA]", j))
			{
				engineScript2.name_UA = data[j];
				if (engineScript2.name_EN == engineScript2.name_UA)
				{
					engineScript2.name_UA = "";
				}
			}
			if (ParseData("[NAME TH]", j))
			{
				engineScript2.name_TH = data[j];
				if (engineScript2.name_EN == engineScript2.name_TH)
				{
					engineScript2.name_TH = "";
				}
			}
			if (ParseData("[NAME RU]", j))
			{
				engineScript2.name_RU = data[j];
				if (engineScript2.name_EN == engineScript2.name_RU)
				{
					engineScript2.name_RU = "";
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
}
