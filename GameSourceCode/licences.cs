using System.IO;
using System.Text;
using UnityEngine;

public class licences : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private genres genres_;

	public string[] licence_EN;

	public int[] licence_TYP;

	public float[] licence_QUALITY;

	public int[] licence_ANGEBOT;

	public int[] licence_GEKAUFT;

	public int[] licence_GENREGOOD;

	public int[] licence_GENREBAD;

	public int[] licence_YEAR;

	public Sprite[] licenceSprites;

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
		if (!genres_)
		{
			genres_ = GetComponent<genres>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
	}

	public void LoadLicences(string filename)
	{
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string text = streamReader.ReadToEnd();
		streamReader.Close();
		data = text.Split("\n"[0]);
		data = tS_.RemoveComments(data);
		int num = data.Length;
		licence_EN = new string[num];
		licence_TYP = new int[num];
		licence_QUALITY = new float[num];
		licence_ANGEBOT = new int[num];
		licence_GEKAUFT = new int[num];
		licence_GENREGOOD = new int[num];
		licence_GENREBAD = new int[num];
		licence_YEAR = new int[num];
		for (int i = 0; i < data.Length; i++)
		{
			if (ParseData("<MOVIE>", i))
			{
				licence_TYP[i] = 0;
			}
			else if (ParseData("<BOOK>", i))
			{
				licence_TYP[i] = 1;
			}
			else if (ParseData("<SPORT>", i))
			{
				licence_TYP[i] = 2;
			}
			else if (ParseData("<TOY>", i))
			{
				licence_TYP[i] = 3;
			}
			else if (ParseData("<BOARD>", i))
			{
				licence_TYP[i] = 4;
			}
			else if (ParseData("<COMIC>", i))
			{
				licence_TYP[i] = 5;
			}
			if (data[i].Contains("<Y"))
			{
				for (int j = 1976; j < 2050; j++)
				{
					if (ParseData("<Y" + j + ">", i))
					{
						licence_YEAR[i] = j;
						break;
					}
				}
			}
			else
			{
				licence_YEAR[i] = 1976;
			}
			licence_GENREGOOD[i] = -1;
			licence_GENREBAD[i] = -1;
			if (data[i].Contains("<G+"))
			{
				int num2 = genres_.genres_UNLOCK.Length;
				for (int k = 0; k < num2; k++)
				{
					if (ParseData("<G+" + k + ">", i))
					{
						licence_GENREGOOD[i] = k;
						break;
					}
				}
			}
			if (data[i].Contains("<G-"))
			{
				for (int l = 0; l < genres_.genres_UNLOCK.Length; l++)
				{
					if (ParseData("<G-" + l + ">", i))
					{
						licence_GENREBAD[i] = l;
						break;
					}
				}
			}
			SetRandomGoodAndBadGenre(i);
			if (data[i].Contains("<Q"))
			{
				for (int m = 1; m <= 10; m++)
				{
					if (ParseData("<Q" + m + ">", i))
					{
						licence_QUALITY[i] = m * 10;
						break;
					}
				}
			}
			else
			{
				licence_QUALITY[i] = Random.Range(10f, 100f);
			}
			licence_EN[i] = data[i];
		}
	}

	private bool ParseData(string c, int i)
	{
		if (data[i].Contains(c))
		{
			data[i] = data[i].Replace("\n", string.Empty);
			data[i] = data[i].Replace("\r", string.Empty);
			data[i] = data[i].Replace("\t", string.Empty);
			data[i] = data[i].Replace(c, "");
			string text = data[i];
			if (text[text.Length - 1] == ' ')
			{
				text = text.Remove(text.Length - 1);
				data[i] = text;
			}
			return true;
		}
		return false;
	}

	public void Buy(int i)
	{
		mS_.Pay(GetPrice(i), 7);
		licence_GEKAUFT[i] = licence_ANGEBOT[i];
		licence_ANGEBOT[i] = 0;
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_BuyLizenz(i);
			}
			else
			{
				mS_.mpCalls_.SERVER_Send_Lizenz(i);
			}
		}
	}

	public void SetRandomGoodAndBadGenre(int i)
	{
		if (licence_GENREGOOD[i] == -1 && licence_GENREBAD[i] == -1)
		{
			if (licence_GENREGOOD[i] == -1)
			{
				licence_GENREGOOD[i] = Random.Range(0, genres_.genres_UNLOCK.Length);
			}
			if (licence_GENREBAD[i] == -1)
			{
				licence_GENREBAD[i] = Random.Range(0, genres_.genres_UNLOCK.Length);
			}
			if (licence_GENREGOOD[i] == licence_GENREBAD[i])
			{
				if (licence_GENREGOOD[i] > 0)
				{
					licence_GENREGOOD[i]--;
				}
				else
				{
					licence_GENREGOOD[i]++;
				}
			}
		}
		else if (licence_GENREGOOD[i] != -1 && licence_GENREBAD[i] == -1)
		{
			licence_GENREBAD[i] = Random.Range(0, genres_.genres_UNLOCK.Length);
			if (licence_GENREGOOD[i] == licence_GENREBAD[i])
			{
				if (licence_GENREBAD[i] > 0)
				{
					licence_GENREBAD[i]--;
				}
				else
				{
					licence_GENREBAD[i]++;
				}
			}
		}
		else
		{
			if (licence_GENREGOOD[i] != -1 || licence_GENREBAD[i] == -1)
			{
				return;
			}
			licence_GENREGOOD[i] = Random.Range(0, genres_.genres_UNLOCK.Length);
			if (licence_GENREGOOD[i] == licence_GENREBAD[i])
			{
				if (licence_GENREGOOD[i] > 0)
				{
					licence_GENREGOOD[i]--;
				}
				else
				{
					licence_GENREGOOD[i]++;
				}
			}
		}
	}

	public void Sell(int i)
	{
		mS_.Earn(GetSellPrice(i), 2);
		licence_GEKAUFT[i] = 0;
	}

	public string GetName(int i)
	{
		return licence_EN[i];
	}

	public int GetPrice(int i)
	{
		int num = 0;
		int num2 = mS_.year - 1976;
		if (num2 < 1)
		{
			num2 = 1;
		}
		if (num2 > 50)
		{
			num2 = 50;
		}
		if (licence_QUALITY[i] >= 0f && licence_QUALITY[i] < 33f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * (float)(1500 + num2 * 50) * (float)licence_ANGEBOT[i]);
		}
		if (licence_QUALITY[i] >= 33f && licence_QUALITY[i] < 66f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * (float)(4000 + num2 * 100) * (float)licence_ANGEBOT[i]);
		}
		if (licence_QUALITY[i] >= 66f && licence_QUALITY[i] < 80f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * (float)(7000 + num2 * 200) * (float)licence_ANGEBOT[i]);
		}
		if (licence_QUALITY[i] >= 80f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * (float)(11000 + num2 * 300) * (float)licence_ANGEBOT[i]);
		}
		return num / 500 * 500;
	}

	public int GetSellPrice(int i)
	{
		int num = 0;
		if (licence_QUALITY[i] >= 0f && licence_QUALITY[i] < 33f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * 1000f * (float)licence_GEKAUFT[i]);
		}
		if (licence_QUALITY[i] >= 33f && licence_QUALITY[i] < 66f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * 3000f * (float)licence_GEKAUFT[i]);
		}
		if (licence_QUALITY[i] >= 66f && licence_QUALITY[i] < 80f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * 6000f * (float)licence_GEKAUFT[i]);
		}
		if (licence_QUALITY[i] >= 80f)
		{
			num = Mathf.RoundToInt(licence_QUALITY[i] * 8000f * (float)licence_GEKAUFT[i]);
		}
		return num / 500 * 500;
	}

	public string GetTypString(int i)
	{
		string result = "";
		switch (licence_TYP[i])
		{
		case 0:
			result = tS_.GetText(298);
			break;
		case 1:
			result = tS_.GetText(299);
			break;
		case 2:
			result = tS_.GetText(300);
			break;
		case 3:
			result = tS_.GetText(2021);
			break;
		case 4:
			result = tS_.GetText(2022);
			break;
		case 5:
			result = tS_.GetText(2023);
			break;
		}
		return result;
	}

	public string GetTooltip(int i)
	{
		string text = "<b>" + GetName(i) + "</b>";
		text = text + "\n<color=magenta>" + GetTypString(i) + "</color>\n";
		text = text + "\n" + tS_.GetText(217) + ": " + licence_YEAR[i];
		text = ((GetPrice(i) <= 0) ? (text + "\n" + tS_.GetText(88) + ": " + mS_.GetMoney(GetSellPrice(i), showDollar: true)) : (text + "\n" + tS_.GetText(218) + ": " + mS_.GetMoney(GetPrice(i), showDollar: true)));
		text = text + "\n" + tS_.GetText(302) + ": " + Mathf.RoundToInt(licence_QUALITY[i]) + "%";
		text = text + "\n\n" + tS_.GetText(2019) + "\n<color=green><b>" + genres_.GetName(licence_GENREGOOD[i]) + "</b></color>";
		text = text + "\n\n" + tS_.GetText(2020) + "\n<color=red><b>" + genres_.GetName(licence_GENREBAD[i]) + "</b></color>";
		string text2 = tS_.GetText(301);
		text2 = ((licence_GEKAUFT[i] != 0) ? text2.Replace("<NUM>", licence_GEKAUFT[i].ToString()) : text2.Replace("<NUM>", licence_ANGEBOT[i].ToString()));
		return text + "\n\n<b>" + text2 + "</b>";
	}

	public void LizenzenUpdaten()
	{
		if (mS_.multiplayer && mS_.mpCalls_.isClient)
		{
			return;
		}
		int num = 0;
		int num2 = 0;
		int num3 = 0;
		int num4 = 0;
		int num5 = 0;
		int num6 = 0;
		for (int i = 0; i < licence_ANGEBOT.Length; i++)
		{
			if (licence_ANGEBOT[i] <= 0)
			{
				continue;
			}
			switch (licence_TYP[i])
			{
			case 0:
				num++;
				break;
			case 1:
				num2++;
				break;
			case 2:
				num3++;
				break;
			case 3:
				num4++;
				break;
			case 4:
				num5++;
				break;
			case 5:
				num6++;
				break;
			}
			if (Random.Range(0, 10) == 1)
			{
				licence_ANGEBOT[i] = 0;
				switch (licence_TYP[i])
				{
				case 0:
					num--;
					break;
				case 1:
					num2--;
					break;
				case 2:
					num3--;
					break;
				case 3:
					num4--;
					break;
				case 4:
					num5--;
					break;
				case 5:
					num6--;
					break;
				}
				if (mS_.multiplayer && mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Lizenz(i);
				}
			}
		}
		if (mS_.globalEvent == 10)
		{
			return;
		}
		if (num < 20)
		{
			for (int j = 0; j < licence_ANGEBOT.Length; j++)
			{
				if (mS_.year >= licence_YEAR[j] && genres_.genres_UNLOCK[licence_GENREGOOD[j]] && genres_.genres_UNLOCK[licence_GENREBAD[j]] && licence_TYP[j] == 0 && licence_ANGEBOT[j] == 0 && licence_GEKAUFT[j] == 0 && Random.Range(0, 100) == 1)
				{
					licence_ANGEBOT[j] = Random.Range(1, 6);
					num++;
					if (mS_.multiplayer && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_Lizenz(j);
					}
				}
				if (num >= 20)
				{
					break;
				}
			}
		}
		if (num2 < 5)
		{
			for (int k = 0; k < licence_ANGEBOT.Length; k++)
			{
				if (mS_.year >= licence_YEAR[k] && genres_.genres_UNLOCK[licence_GENREGOOD[k]] && genres_.genres_UNLOCK[licence_GENREBAD[k]] && licence_TYP[k] == 1 && licence_ANGEBOT[k] == 0 && licence_GEKAUFT[k] == 0 && (Random.Range(0, 100) <= 2 || num2 == 0))
				{
					licence_ANGEBOT[k] = Random.Range(1, 6);
					num2++;
					if (mS_.multiplayer && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_Lizenz(k);
					}
				}
				if (num2 >= 5)
				{
					break;
				}
			}
		}
		if (num3 < 5)
		{
			for (int l = 0; l < licence_ANGEBOT.Length; l++)
			{
				if (mS_.year >= licence_YEAR[l] && genres_.genres_UNLOCK[licence_GENREGOOD[l]] && genres_.genres_UNLOCK[licence_GENREBAD[l]] && licence_TYP[l] == 2 && licence_ANGEBOT[l] == 0 && licence_GEKAUFT[l] == 0 && (Random.Range(0, 100) <= 2 || num3 == 0))
				{
					licence_ANGEBOT[l] = Random.Range(1, 6);
					num3++;
					if (mS_.multiplayer && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_Lizenz(l);
					}
				}
				if (num3 >= 5)
				{
					break;
				}
			}
		}
		if (num4 < 3)
		{
			for (int m = 0; m < licence_ANGEBOT.Length; m++)
			{
				if (mS_.year >= licence_YEAR[m] && genres_.genres_UNLOCK[licence_GENREGOOD[m]] && genres_.genres_UNLOCK[licence_GENREBAD[m]] && licence_TYP[m] == 3 && licence_ANGEBOT[m] == 0 && licence_GEKAUFT[m] == 0 && (Random.Range(0, 100) <= 2 || num4 == 0))
				{
					licence_ANGEBOT[m] = Random.Range(1, 6);
					num4++;
					if (mS_.multiplayer && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_Lizenz(m);
					}
				}
				if (num4 >= 5)
				{
					break;
				}
			}
		}
		if (num5 < 3)
		{
			for (int n = 0; n < licence_ANGEBOT.Length; n++)
			{
				if (mS_.year >= licence_YEAR[n] && genres_.genres_UNLOCK[licence_GENREGOOD[n]] && genres_.genres_UNLOCK[licence_GENREBAD[n]] && licence_TYP[n] == 4 && licence_ANGEBOT[n] == 0 && licence_GEKAUFT[n] == 0 && (Random.Range(0, 100) <= 2 || num5 == 0))
				{
					licence_ANGEBOT[n] = Random.Range(1, 6);
					num5++;
					if (mS_.multiplayer && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_Lizenz(n);
					}
				}
				if (num5 >= 5)
				{
					break;
				}
			}
		}
		if (num6 >= 3)
		{
			return;
		}
		for (int num7 = 0; num7 < licence_ANGEBOT.Length; num7++)
		{
			if (mS_.year >= licence_YEAR[num7] && genres_.genres_UNLOCK[licence_GENREGOOD[num7]] && genres_.genres_UNLOCK[licence_GENREBAD[num7]] && licence_TYP[num7] == 5 && licence_ANGEBOT[num7] == 0 && licence_GEKAUFT[num7] == 0 && (Random.Range(0, 100) <= 2 || num6 == 0))
			{
				licence_ANGEBOT[num7] = Random.Range(1, 6);
				num6++;
				if (mS_.multiplayer && mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_Lizenz(num7);
				}
			}
			if (num6 >= 5)
			{
				break;
			}
		}
	}
}
