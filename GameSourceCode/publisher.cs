using System.IO;
using System.Text;
using UnityEngine;

public class publisher : MonoBehaviour
{
	public GameObject prefabPublisher;

	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private gameplayFeatures gF_;

	private engineFeatures eF_;

	private games games_;

	private unlockScript unlock_;

	private genres genres_;

	public GUI_Main guiMain_;

	public platforms platforms_;

	public reviewText reviewText_;

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
		if (!gF_)
		{
			gF_ = GetComponent<gameplayFeatures>();
		}
		if (!eF_)
		{
			eF_ = GetComponent<engineFeatures>();
		}
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
		if (!unlock_)
		{
			unlock_ = GetComponent<unlockScript>();
		}
		if (!genres_)
		{
			genres_ = GetComponent<genres>();
		}
		if (!platforms_)
		{
			platforms_ = GetComponent<platforms>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!reviewText_)
		{
			reviewText_ = GetComponent<reviewText>();
		}
	}

	public publisherScript CreatePublisher()
	{
		FindScripts();
		publisherScript component = Object.Instantiate(prefabPublisher).GetComponent<publisherScript>();
		component.main_ = base.gameObject;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.eF_ = eF_;
		component.guiMain_ = guiMain_;
		component.settings_ = settings_;
		component.genres_ = genres_;
		component.games_ = games_;
		component.gF_ = gF_;
		component.unlock_ = unlock_;
		component.platforms_ = platforms_;
		component.reviewText_ = reviewText_;
		mS_.FindPublishers();
		return component;
	}

	public void LoadPublisher(string filename)
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
		publisherScript publisherScript2 = null;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				publisherScript2 = CreatePublisher();
				publisherScript2.myID = int.Parse(data[j]);
				publisherScript2.newGameInWeeks = 1;
				publisherScript2.newGameInWeeksORG = 1;
				publisherScript2.Init();
			}
			if (!publisherScript2)
			{
				continue;
			}
			if (ParseData("[MARKET]", j))
			{
				publisherScript2.stars = int.Parse(data[j]);
			}
			if (ParseData("[PIC]", j))
			{
				publisherScript2.logoID = int.Parse(data[j]);
			}
			if (ParseData("[SHARE]", j))
			{
				publisherScript2.share = int.Parse(data[j]);
			}
			if (ParseData("[GENRE]", j))
			{
				publisherScript2.fanGenre = int.Parse(data[j]);
			}
			if (ParseData("[SPEED]", j))
			{
				publisherScript2.developmentSpeed = int.Parse(data[j]);
			}
			if (ParseData("[COMVAL]", j))
			{
				publisherScript2.firmenwert = int.Parse(data[j]);
			}
			if (ParseData("[COUNTRY]", j))
			{
				publisherScript2.country = int.Parse(data[j]);
			}
			if (ParseData("[DEVELOPER]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.developer = true;
				}
				else
				{
					publisherScript2.developer = false;
				}
			}
			if (ParseData("[PUBLISHER]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.publisher = true;
				}
				else
				{
					publisherScript2.publisher = false;
				}
			}
			if (ParseData("[ONLYMOBILE]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.onlyMobile = true;
					publisherScript2.publisher = true;
				}
				else
				{
					publisherScript2.onlyMobile = false;
				}
			}
			if (ParseData("[PLATFORM]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.ownPlatform = true;
				}
				else
				{
					publisherScript2.ownPlatform = false;
				}
			}
			if (ParseData("[EXCLUSIVE]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.exklusive = true;
				}
				else
				{
					publisherScript2.exklusive = false;
				}
			}
			if (ParseData("[NOTFORSALE]", j))
			{
				if (ParseDataDontCutLastChar("true", j))
				{
					publisherScript2.notForSale = true;
				}
				else
				{
					publisherScript2.notForSale = false;
				}
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					publisherScript2.date_month = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					publisherScript2.date_month = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					publisherScript2.date_month = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					publisherScript2.date_month = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					publisherScript2.date_month = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					publisherScript2.date_month = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					publisherScript2.date_month = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					publisherScript2.date_month = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					publisherScript2.date_month = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					publisherScript2.date_month = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					publisherScript2.date_month = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					publisherScript2.date_month = 12;
				}
				if (publisherScript2.date_month <= 0)
				{
					Debug.Log("ERROR: Publisher.txt -> Incorrect Month: " + publisherScript2.myID);
				}
				publisherScript2.date_year = int.Parse(data[j]);
				if (publisherScript2.date_year == 1976 && publisherScript2.date_month == 1)
				{
					publisherScript2.isUnlocked = true;
					publisherScript2.lockToBuy = Random.Range(12, 36);
				}
			}
			if (ParseData("[DATE END]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					publisherScript2.date_month_end = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					publisherScript2.date_month_end = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					publisherScript2.date_month_end = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					publisherScript2.date_month_end = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					publisherScript2.date_month_end = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					publisherScript2.date_month_end = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					publisherScript2.date_month_end = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					publisherScript2.date_month_end = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					publisherScript2.date_month_end = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					publisherScript2.date_month_end = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					publisherScript2.date_month_end = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					publisherScript2.date_month_end = 12;
				}
				if (publisherScript2.date_month <= 0)
				{
					Debug.Log("ERROR: Publisher.txt -> Incorrect Month: " + publisherScript2.myID);
				}
				publisherScript2.date_year_end = int.Parse(data[j]);
			}
			if (ParseData("[NAME EN]", j))
			{
				publisherScript2.name_EN = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				publisherScript2.name_GE = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_GE)
				{
					publisherScript2.name_GE = "";
				}
			}
			if (ParseData("[NAME TU]", j))
			{
				publisherScript2.name_TU = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_TU)
				{
					publisherScript2.name_TU = "";
				}
			}
			if (ParseData("[NAME CH]", j))
			{
				publisherScript2.name_CH = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_CH)
				{
					publisherScript2.name_CH = "";
				}
			}
			if (ParseData("[NAME FR]", j))
			{
				publisherScript2.name_FR = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_FR)
				{
					publisherScript2.name_FR = "";
				}
			}
			if (ParseData("[NAME JA]", j))
			{
				publisherScript2.name_JA = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_JA)
				{
					publisherScript2.name_JA = "";
				}
			}
			if (ParseData("[NAME UA]", j))
			{
				publisherScript2.name_UA = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_UA)
				{
					publisherScript2.name_UA = "";
				}
			}
			if (ParseData("[NAME TH]", j))
			{
				publisherScript2.name_TH = data[j];
				if (publisherScript2.name_EN == publisherScript2.name_TH)
				{
					publisherScript2.name_TH = "";
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
