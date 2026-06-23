using System.IO;
using System.Text;
using UnityEngine;

public class copyProtect : MonoBehaviour
{
	public GameObject prefabCopyProtect;

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

	public copyProtectScript CreateCopyProtect()
	{
		copyProtectScript component = Object.Instantiate(prefabCopyProtect).GetComponent<copyProtectScript>();
		component.main_ = base.gameObject;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.settings_ = settings_;
		return component;
	}

	public void LoadCopyProtect(string filename)
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
		copyProtectScript copyProtectScript2 = null;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				copyProtectScript2 = CreateCopyProtect();
				copyProtectScript2.myID = int.Parse(data[j]);
				copyProtectScript2.Init();
			}
			if (!copyProtectScript2)
			{
				continue;
			}
			if (ParseData("[PRICE]", j))
			{
				copyProtectScript2.price = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				copyProtectScript2.dev_costs = int.Parse(data[j]);
			}
			if (ParseData("[ENDLESS]", j))
			{
				copyProtectScript2.neverLooseEffect = true;
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					copyProtectScript2.date_month = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					copyProtectScript2.date_month = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					copyProtectScript2.date_month = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					copyProtectScript2.date_month = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					copyProtectScript2.date_month = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					copyProtectScript2.date_month = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					copyProtectScript2.date_month = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					copyProtectScript2.date_month = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					copyProtectScript2.date_month = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					copyProtectScript2.date_month = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					copyProtectScript2.date_month = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					copyProtectScript2.date_month = 12;
				}
				if (copyProtectScript2.date_month <= 0)
				{
					Debug.Log("ERROR: CopyProtect.txt -> Incorrect Month: " + copyProtectScript2.myID);
				}
				copyProtectScript2.date_year = int.Parse(data[j]);
			}
			if (ParseData("[NAME EN]", j))
			{
				copyProtectScript2.name_EN = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				copyProtectScript2.name_GE = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_GE)
				{
					copyProtectScript2.name_GE = "";
				}
			}
			if (ParseData("[NAME TU]", j))
			{
				copyProtectScript2.name_TU = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_TU)
				{
					copyProtectScript2.name_TU = "";
				}
			}
			if (ParseData("[NAME CH]", j))
			{
				copyProtectScript2.name_CH = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_CH)
				{
					copyProtectScript2.name_CH = "";
				}
			}
			if (ParseData("[NAME FR]", j))
			{
				copyProtectScript2.name_FR = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_FR)
				{
					copyProtectScript2.name_FR = "";
				}
			}
			if (ParseData("[NAME CT]", j))
			{
				copyProtectScript2.name_CT = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_CT)
				{
					copyProtectScript2.name_CT = "";
				}
			}
			if (ParseData("[NAME RU]", j))
			{
				copyProtectScript2.name_RU = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_RU)
				{
					copyProtectScript2.name_RU = "";
				}
			}
			if (ParseData("[NAME IT]", j))
			{
				copyProtectScript2.name_IT = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_IT)
				{
					copyProtectScript2.name_IT = "";
				}
			}
			if (ParseData("[NAME JA]", j))
			{
				copyProtectScript2.name_JA = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_JA)
				{
					copyProtectScript2.name_JA = "";
				}
			}
			if (ParseData("[NAME UA]", j))
			{
				copyProtectScript2.name_UA = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_UA)
				{
					copyProtectScript2.name_UA = "";
				}
			}
			if (ParseData("[NAME TH]", j))
			{
				copyProtectScript2.name_TH = data[j];
				if (copyProtectScript2.name_EN == copyProtectScript2.name_TH)
				{
					copyProtectScript2.name_TH = "";
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
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				copyProtectScript component = array[i].GetComponent<copyProtectScript>();
				if ((bool)component)
				{
					component.EffektVerschlechtern();
				}
			}
		}
	}
}
