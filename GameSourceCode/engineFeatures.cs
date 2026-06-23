using System.IO;
using System.Text;
using UnityEngine;

public class engineFeatures : MonoBehaviour
{
	private mainScript mS_;

	private textScript tS_;

	private settingsScript settings_;

	private GUI_Main guiMain_;

	private genres genres_;

	private games games_;

	private mpCalls mpCalls_;

	public GameObject prefabEngine;

	private Sprite[] engineFeatures_PIC;

	public Sprite[] engineFeatures_PICTYP;

	public int[] engineFeatures_TYP;

	public int[] engineFeatures_RES_POINTS;

	public float[] engineFeatures_RES_POINTS_LEFT;

	public int[] engineFeatures_PRICE;

	public int[] engineFeatures_DEV_COSTS;

	public int[] engineFeatures_TECH;

	public int[] engineFeatures_DATE_YEAR;

	public int[] engineFeatures_DATE_MONTH;

	public int[] engineFeatures_GAMEPLAY;

	public int[] engineFeatures_GRAPHIC;

	public int[] engineFeatures_SOUND;

	public int[] engineFeatures_TECHNIK;

	public int[] engineFeatures_LEVEL;

	public bool[] engineFeatures_UNLOCK;

	public string[] engineFeatures_ICONFILE;

	public string[] engineFeatures_NAME_EN;

	public string[] engineFeatures_NAME_GE;

	public string[] engineFeatures_NAME_TU;

	public string[] engineFeatures_NAME_CH;

	public string[] engineFeatures_NAME_FR;

	public string[] engineFeatures_NAME_PB;

	public string[] engineFeatures_NAME_CT;

	public string[] engineFeatures_NAME_HU;

	public string[] engineFeatures_NAME_ES;

	public string[] engineFeatures_NAME_CZ;

	public string[] engineFeatures_NAME_KO;

	public string[] engineFeatures_NAME_AR;

	public string[] engineFeatures_NAME_RU;

	public string[] engineFeatures_NAME_IT;

	public string[] engineFeatures_NAME_JA;

	public string[] engineFeatures_NAME_PL;

	public string[] engineFeatures_NAME_UA;

	public string[] engineFeatures_NAME_TH;

	public string[] engineFeatures_DESC_EN;

	public string[] engineFeatures_DESC_GE;

	public string[] engineFeatures_DESC_TU;

	public string[] engineFeatures_DESC_CH;

	public string[] engineFeatures_DESC_FR;

	public string[] engineFeatures_DESC_PB;

	public string[] engineFeatures_DESC_CT;

	public string[] engineFeatures_DESC_HU;

	public string[] engineFeatures_DESC_ES;

	public string[] engineFeatures_DESC_CZ;

	public string[] engineFeatures_DESC_KO;

	public string[] engineFeatures_DESC_AR;

	public string[] engineFeatures_DESC_RU;

	public string[] engineFeatures_DESC_IT;

	public string[] engineFeatures_DESC_JA;

	public string[] engineFeatures_DESC_PL;

	public string[] engineFeatures_DESC_UA;

	public string[] engineFeatures_DESC_TH;

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
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	public void Init()
	{
		engineFeatures_PIC = new Sprite[engineFeatures_UNLOCK.Length];
	}

	public int GetOutdatetAmount(int usedFeature_)
	{
		int num = 0;
		for (int i = 0; i < engineFeatures_TYP.Length; i++)
		{
			if (engineFeatures_UNLOCK[i] && engineFeatures_TYP[i] == engineFeatures_TYP[usedFeature_] && engineFeatures_DATE_YEAR[usedFeature_] < engineFeatures_DATE_YEAR[i])
			{
				num++;
			}
		}
		return num;
	}

	public void LoadEngineFeatures(string filename)
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
		engineFeatures_PIC = new Sprite[num2];
		engineFeatures_TYP = new int[num2];
		engineFeatures_RES_POINTS = new int[num2];
		engineFeatures_RES_POINTS_LEFT = new float[num2];
		engineFeatures_PRICE = new int[num2];
		engineFeatures_DEV_COSTS = new int[num2];
		engineFeatures_TECH = new int[num2];
		engineFeatures_GAMEPLAY = new int[num2];
		engineFeatures_GRAPHIC = new int[num2];
		engineFeatures_SOUND = new int[num2];
		engineFeatures_TECHNIK = new int[num2];
		engineFeatures_DATE_YEAR = new int[num2];
		engineFeatures_DATE_MONTH = new int[num2];
		engineFeatures_LEVEL = new int[num2];
		engineFeatures_UNLOCK = new bool[num2];
		engineFeatures_ICONFILE = new string[num2];
		engineFeatures_NAME_EN = new string[num2];
		engineFeatures_NAME_GE = new string[num2];
		engineFeatures_NAME_TU = new string[num2];
		engineFeatures_NAME_CH = new string[num2];
		engineFeatures_NAME_FR = new string[num2];
		engineFeatures_NAME_PB = new string[num2];
		engineFeatures_NAME_CT = new string[num2];
		engineFeatures_NAME_HU = new string[num2];
		engineFeatures_NAME_ES = new string[num2];
		engineFeatures_NAME_CZ = new string[num2];
		engineFeatures_NAME_KO = new string[num2];
		engineFeatures_NAME_AR = new string[num2];
		engineFeatures_NAME_RU = new string[num2];
		engineFeatures_NAME_IT = new string[num2];
		engineFeatures_NAME_JA = new string[num2];
		engineFeatures_NAME_PL = new string[num2];
		engineFeatures_NAME_UA = new string[num2];
		engineFeatures_NAME_TH = new string[num2];
		engineFeatures_DESC_EN = new string[num2];
		engineFeatures_DESC_GE = new string[num2];
		engineFeatures_DESC_TU = new string[num2];
		engineFeatures_DESC_CH = new string[num2];
		engineFeatures_DESC_FR = new string[num2];
		engineFeatures_DESC_PB = new string[num2];
		engineFeatures_DESC_CT = new string[num2];
		engineFeatures_DESC_HU = new string[num2];
		engineFeatures_DESC_ES = new string[num2];
		engineFeatures_DESC_CZ = new string[num2];
		engineFeatures_DESC_KO = new string[num2];
		engineFeatures_DESC_AR = new string[num2];
		engineFeatures_DESC_RU = new string[num2];
		engineFeatures_DESC_IT = new string[num2];
		engineFeatures_DESC_JA = new string[num2];
		engineFeatures_DESC_PL = new string[num2];
		engineFeatures_DESC_UA = new string[num2];
		engineFeatures_DESC_TH = new string[num2];
		int num3 = -1;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				num3 = int.Parse(data[j]);
			}
			if (ParseData("[TYP]", j))
			{
				engineFeatures_TYP[num3] = int.Parse(data[j]);
			}
			if (ParseData("[RES POINTS]", j))
			{
				engineFeatures_RES_POINTS[num3] = int.Parse(data[j]);
				engineFeatures_RES_POINTS_LEFT[num3] = engineFeatures_RES_POINTS[num3];
			}
			if (ParseData("[PRICE]", j))
			{
				engineFeatures_PRICE[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				engineFeatures_DEV_COSTS[num3] = int.Parse(data[j]);
			}
			if (ParseData("[TECHLEVEL]", j))
			{
				engineFeatures_TECH[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GAMEPLAY]", j))
			{
				engineFeatures_GAMEPLAY[num3] = int.Parse(data[j]);
			}
			if (ParseData("[GRAPHIC]", j))
			{
				engineFeatures_GRAPHIC[num3] = int.Parse(data[j]);
			}
			if (ParseData("[SOUND]", j))
			{
				engineFeatures_SOUND[num3] = int.Parse(data[j]);
			}
			if (ParseData("[TECH]", j))
			{
				engineFeatures_TECHNIK[num3] = int.Parse(data[j]);
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					engineFeatures_DATE_MONTH[num3] = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					engineFeatures_DATE_MONTH[num3] = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					engineFeatures_DATE_MONTH[num3] = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					engineFeatures_DATE_MONTH[num3] = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					engineFeatures_DATE_MONTH[num3] = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					engineFeatures_DATE_MONTH[num3] = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					engineFeatures_DATE_MONTH[num3] = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					engineFeatures_DATE_MONTH[num3] = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					engineFeatures_DATE_MONTH[num3] = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					engineFeatures_DATE_MONTH[num3] = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					engineFeatures_DATE_MONTH[num3] = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					engineFeatures_DATE_MONTH[num3] = 12;
				}
				if (engineFeatures_DATE_MONTH[num3] <= 0)
				{
					Debug.Log("ERROR: EngineFeatures.txt -> Incorrect Month: " + engineFeatures_NAME_EN[num3]);
				}
				engineFeatures_DATE_YEAR[num3] = int.Parse(data[j]);
				if (engineFeatures_DATE_YEAR[num3] == 1976 && engineFeatures_DATE_MONTH[num3] == 1)
				{
					engineFeatures_UNLOCK[num3] = true;
				}
			}
			if (ParseData("[PIC]", j))
			{
				engineFeatures_ICONFILE[num3] = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				engineFeatures_NAME_GE[num3] = data[j];
			}
			if (ParseData("[NAME EN]", j))
			{
				engineFeatures_NAME_EN[num3] = data[j];
			}
			if (ParseData("[NAME TU]", j))
			{
				engineFeatures_NAME_TU[num3] = data[j];
			}
			if (ParseData("[NAME CH]", j))
			{
				engineFeatures_NAME_CH[num3] = data[j];
			}
			if (ParseData("[NAME FR]", j))
			{
				engineFeatures_NAME_FR[num3] = data[j];
			}
			if (ParseData("[NAME PB]", j))
			{
				engineFeatures_NAME_PB[num3] = data[j];
			}
			if (ParseData("[NAME CT]", j))
			{
				engineFeatures_NAME_CT[num3] = data[j];
			}
			if (ParseData("[NAME HU]", j))
			{
				engineFeatures_NAME_HU[num3] = data[j];
			}
			if (ParseData("[NAME ES]", j))
			{
				engineFeatures_NAME_ES[num3] = data[j];
			}
			if (ParseData("[NAME CZ]", j))
			{
				engineFeatures_NAME_CZ[num3] = data[j];
			}
			if (ParseData("[NAME KO]", j))
			{
				engineFeatures_NAME_KO[num3] = data[j];
			}
			if (ParseData("[NAME AR]", j))
			{
				engineFeatures_NAME_AR[num3] = data[j];
			}
			if (ParseData("[NAME RU]", j))
			{
				engineFeatures_NAME_RU[num3] = data[j];
			}
			if (ParseData("[NAME IT]", j))
			{
				engineFeatures_NAME_IT[num3] = data[j];
			}
			if (ParseData("[NAME JA]", j))
			{
				engineFeatures_NAME_JA[num3] = data[j];
			}
			if (ParseData("[NAME PL]", j))
			{
				engineFeatures_NAME_PL[num3] = data[j];
			}
			if (ParseData("[NAME UA]", j))
			{
				engineFeatures_NAME_UA[num3] = data[j];
			}
			if (ParseData("[NAME TH]", j))
			{
				engineFeatures_NAME_TH[num3] = data[j];
			}
			if (ParseData("[DESC GE]", j))
			{
				engineFeatures_DESC_GE[num3] = data[j];
			}
			if (ParseData("[DESC EN]", j))
			{
				engineFeatures_DESC_EN[num3] = data[j];
			}
			if (ParseData("[DESC TU]", j))
			{
				engineFeatures_DESC_TU[num3] = data[j];
			}
			if (ParseData("[DESC CH]", j))
			{
				engineFeatures_DESC_CH[num3] = data[j];
			}
			if (ParseData("[DESC FR]", j))
			{
				engineFeatures_DESC_FR[num3] = data[j];
			}
			if (ParseData("[DESC PB]", j))
			{
				engineFeatures_DESC_PB[num3] = data[j];
			}
			if (ParseData("[DESC CT]", j))
			{
				engineFeatures_DESC_CT[num3] = data[j];
			}
			if (ParseData("[DESC HU]", j))
			{
				engineFeatures_DESC_HU[num3] = data[j];
			}
			if (ParseData("[DESC ES]", j))
			{
				engineFeatures_DESC_ES[num3] = data[j];
			}
			if (ParseData("[DESC CZ]", j))
			{
				engineFeatures_DESC_CZ[num3] = data[j];
			}
			if (ParseData("[DESC KO]", j))
			{
				engineFeatures_DESC_KO[num3] = data[j];
			}
			if (ParseData("[DESC AR]", j))
			{
				engineFeatures_DESC_AR[num3] = data[j];
			}
			if (ParseData("[DESC RU]", j))
			{
				engineFeatures_DESC_RU[num3] = data[j];
			}
			if (ParseData("[DESC IT]", j))
			{
				engineFeatures_DESC_IT[num3] = data[j];
			}
			if (ParseData("[DESC JA]", j))
			{
				engineFeatures_DESC_JA[num3] = data[j];
			}
			if (ParseData("[DESC PL]", j))
			{
				engineFeatures_DESC_PL[num3] = data[j];
			}
			if (ParseData("[DESC UA]", j))
			{
				engineFeatures_DESC_UA[num3] = data[j];
			}
			if (ParseData("[DESC TH]", j))
			{
				engineFeatures_DESC_TH[num3] = data[j];
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
			text = engineFeatures_NAME_EN[i];
			break;
		case 1:
			text = engineFeatures_NAME_GE[i];
			break;
		case 2:
			if (engineFeatures_NAME_TU.Length != 0)
			{
				text = engineFeatures_NAME_TU[i];
			}
			break;
		case 3:
			if (engineFeatures_NAME_CH.Length != 0)
			{
				text = engineFeatures_NAME_CH[i];
			}
			break;
		case 4:
			if (engineFeatures_NAME_FR.Length != 0)
			{
				text = engineFeatures_NAME_FR[i];
			}
			break;
		case 5:
			if (engineFeatures_NAME_ES.Length != 0)
			{
				text = engineFeatures_NAME_ES[i];
			}
			break;
		case 6:
			if (engineFeatures_NAME_KO.Length != 0)
			{
				text = engineFeatures_NAME_KO[i];
			}
			break;
		case 7:
			if (engineFeatures_NAME_PB.Length != 0)
			{
				text = engineFeatures_NAME_PB[i];
			}
			break;
		case 8:
			if (engineFeatures_NAME_HU.Length != 0)
			{
				text = engineFeatures_NAME_HU[i];
			}
			break;
		case 9:
			if (engineFeatures_NAME_RU.Length != 0)
			{
				text = engineFeatures_NAME_RU[i];
			}
			break;
		case 10:
			if (engineFeatures_NAME_CT.Length != 0)
			{
				text = engineFeatures_NAME_CT[i];
			}
			break;
		case 11:
			if (engineFeatures_NAME_PL.Length != 0)
			{
				text = engineFeatures_NAME_PL[i];
			}
			break;
		case 12:
			if (engineFeatures_NAME_CZ.Length != 0)
			{
				text = engineFeatures_NAME_CZ[i];
			}
			break;
		case 13:
			if (engineFeatures_NAME_AR.Length != 0)
			{
				text = engineFeatures_NAME_AR[i];
			}
			break;
		case 14:
			if (engineFeatures_NAME_IT.Length != 0)
			{
				text = engineFeatures_NAME_IT[i];
			}
			break;
		case 16:
			if (engineFeatures_NAME_JA.Length != 0)
			{
				text = engineFeatures_NAME_JA[i];
			}
			break;
		case 17:
			if (engineFeatures_NAME_UA.Length != 0)
			{
				text = engineFeatures_NAME_UA[i];
			}
			break;
		case 19:
			if (engineFeatures_NAME_TH.Length != 0)
			{
				text = engineFeatures_NAME_TH[i];
			}
			break;
		default:
			text = engineFeatures_NAME_EN[i];
			break;
		}
		if (text == null)
		{
			return engineFeatures_NAME_EN[i];
		}
		if (text.Length <= 0)
		{
			return engineFeatures_NAME_EN[i];
		}
		return text;
	}

	public string GetDesc(int i)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			text = engineFeatures_DESC_EN[i];
			break;
		case 1:
			text = engineFeatures_DESC_GE[i];
			break;
		case 2:
			if (engineFeatures_DESC_TU.Length != 0)
			{
				text = engineFeatures_DESC_TU[i];
			}
			break;
		case 3:
			if (engineFeatures_DESC_CH.Length != 0)
			{
				text = engineFeatures_DESC_CH[i];
			}
			break;
		case 4:
			if (engineFeatures_DESC_FR.Length != 0)
			{
				text = engineFeatures_DESC_FR[i];
			}
			break;
		case 5:
			if (engineFeatures_DESC_ES.Length != 0)
			{
				text = engineFeatures_DESC_ES[i];
			}
			break;
		case 6:
			if (engineFeatures_DESC_KO.Length != 0)
			{
				text = engineFeatures_DESC_KO[i];
			}
			break;
		case 7:
			if (engineFeatures_DESC_PB.Length != 0)
			{
				text = engineFeatures_DESC_PB[i];
			}
			break;
		case 8:
			if (engineFeatures_DESC_HU.Length != 0)
			{
				text = engineFeatures_DESC_HU[i];
			}
			break;
		case 9:
			if (engineFeatures_DESC_RU.Length != 0)
			{
				text = engineFeatures_DESC_RU[i];
			}
			break;
		case 10:
			if (engineFeatures_DESC_CT.Length != 0)
			{
				text = engineFeatures_DESC_CT[i];
			}
			break;
		case 11:
			if (engineFeatures_DESC_PL.Length != 0)
			{
				text = engineFeatures_DESC_PL[i];
			}
			break;
		case 12:
			if (engineFeatures_DESC_CZ.Length != 0)
			{
				text = engineFeatures_DESC_CZ[i];
			}
			break;
		case 13:
			if (engineFeatures_DESC_AR.Length != 0)
			{
				text = engineFeatures_DESC_AR[i];
			}
			break;
		case 14:
			if (engineFeatures_DESC_IT.Length != 0)
			{
				text = engineFeatures_DESC_IT[i];
			}
			break;
		case 16:
			if (engineFeatures_DESC_JA.Length != 0)
			{
				text = engineFeatures_DESC_JA[i];
			}
			break;
		case 17:
			if (engineFeatures_DESC_UA.Length != 0)
			{
				text = engineFeatures_DESC_UA[i];
			}
			break;
		case 19:
			if (engineFeatures_DESC_TH.Length != 0)
			{
				text = engineFeatures_DESC_TH[i];
			}
			break;
		default:
			text = engineFeatures_DESC_EN[i];
			break;
		}
		if (text == null)
		{
			return engineFeatures_DESC_EN[i];
		}
		if (text.Length <= 0)
		{
			return engineFeatures_DESC_EN[i];
		}
		return text;
	}

	public int GetGameplay(int i)
	{
		float num = (float)engineFeatures_LEVEL[i] * 0.1f;
		num = (float)engineFeatures_GAMEPLAY[i] * (1f + num);
		return Mathf.RoundToInt(num);
	}

	public int GetGraphic(int i)
	{
		float num = (float)engineFeatures_LEVEL[i] * 0.1f;
		num = (float)engineFeatures_GRAPHIC[i] * (1f + num);
		return Mathf.RoundToInt(num);
	}

	public int GetSound(int i)
	{
		float num = (float)engineFeatures_LEVEL[i] * 0.1f;
		num = (float)engineFeatures_SOUND[i] * (1f + num);
		return Mathf.RoundToInt(num);
	}

	public int GetTechnik(int i)
	{
		float num = (float)engineFeatures_LEVEL[i] * 0.1f;
		num = (float)engineFeatures_TECHNIK[i] * (1f + num);
		return Mathf.RoundToInt(num);
	}

	public int GetDevCosts(int i)
	{
		float num = (float)engineFeatures_LEVEL[i] * 0.1f;
		num = (float)engineFeatures_DEV_COSTS[i] * (1f - num);
		return Mathf.RoundToInt(num);
	}

	public int GetDevCostsForEngine(int i)
	{
		float num = mS_.difficulty;
		return Mathf.RoundToInt(Mathf.RoundToInt((float)engineFeatures_DEV_COSTS[i] * (1.25f + num * 0.2f)) / 200 * 200);
	}

	public int GetPrice(int i)
	{
		return engineFeatures_PRICE[i];
	}

	public int GetDevPointsForEngine(int i)
	{
		if (i == -1)
		{
			return 0;
		}
		return 10 + engineFeatures_RES_POINTS[i] / 5;
	}

	public int GetDevPointsForGame(int i)
	{
		if (i == -1)
		{
			return 0;
		}
		return 10 + engineFeatures_RES_POINTS[i] / 10;
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

	public bool IsErforscht(int i)
	{
		if (engineFeatures_RES_POINTS_LEFT[i] <= 0f)
		{
			return true;
		}
		return false;
	}

	public float GetProzent(int i)
	{
		return 100f / (float)engineFeatures_RES_POINTS[i] * ((float)engineFeatures_RES_POINTS[i] - engineFeatures_RES_POINTS_LEFT[i]);
	}

	public void UnlockAll()
	{
		for (int i = 0; i < engineFeatures_UNLOCK.Length; i++)
		{
			engineFeatures_UNLOCK[i] = true;
			engineFeatures_RES_POINTS_LEFT[i] = 0f;
		}
	}

	public void MaxLevelAll()
	{
		for (int i = 0; i < engineFeatures_LEVEL.Length; i++)
		{
			engineFeatures_LEVEL[i] = 5;
		}
	}

	public bool ForschungGestartet(int i)
	{
		if (engineFeatures_RES_POINTS_LEFT[i] == (float)engineFeatures_RES_POINTS[i])
		{
			return false;
		}
		return true;
	}

	public bool Pay(int i)
	{
		if (!ForschungGestartet(i))
		{
			if (mS_.NotEnoughMoney(engineFeatures_PRICE[i]))
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
				if ((bool)component && component.slot == s && component.typ == 2)
				{
					return true;
				}
			}
		}
		return false;
	}

	public string GetDateString(int i)
	{
		return engineFeatures_DATE_YEAR[i] + " " + tS_.GetText(engineFeatures_DATE_MONTH[i] + 220);
	}

	public string GetTooltip(int i)
	{
		string text = "<b>" + GetName(i) + "</b>\n";
		switch (engineFeatures_TYP[i])
		{
		case 0:
			text = text + "<color=black>" + tS_.GetText(9) + "</color>";
			break;
		case 1:
			text = text + "<color=black>" + tS_.GetText(10) + "</color>";
			break;
		case 2:
			text = text + "<color=black>" + tS_.GetText(11) + "</color>";
			break;
		case 3:
			text = text + "<color=black>" + tS_.GetText(12) + "</color>";
			break;
		}
		text = text + "\n<b><color=magenta>" + tS_.GetText(4) + " " + engineFeatures_TECH[i] + "</color></b>";
		text = text + "\n" + GetDateString(i);
		text = text + "\n\n" + GetDesc(i) + "\n";
		string text2 = tS_.GetText(254);
		text2 = text2.Replace("<NUM>", GetGameplay(i).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(255);
		text2 = text2.Replace("<NUM>", GetGraphic(i).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(256);
		text2 = text2.Replace("<NUM>", GetSound(i).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		text2 = tS_.GetText(257);
		text2 = text2.Replace("<NUM>", GetTechnik(i).ToString());
		text = text + "\n<b>" + text2 + "</b>";
		return text + "\n\n<b><color=red>" + tS_.GetText(6) + ": " + mS_.GetMoney(GetDevCosts(i), showDollar: true) + "</color></b>";
	}

	public engineScript CreateEngine()
	{
		if (!mS_)
		{
			FindScripts();
		}
		engineScript component = Object.Instantiate(prefabEngine).GetComponent<engineScript>();
		component.main_ = base.gameObject;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.eF_ = this;
		component.guiMain_ = guiMain_;
		component.settings_ = settings_;
		component.genres_ = genres_;
		component.games_ = games_;
		component.mpCalls_ = mpCalls_;
		mS_.FindEngines();
		return component;
	}

	public Sprite GetTypPic(int i)
	{
		if (engineFeatures_ICONFILE[i] == null)
		{
			return engineFeatures_PICTYP[engineFeatures_TYP[i]];
		}
		if (string.IsNullOrEmpty(engineFeatures_ICONFILE[i]))
		{
			return engineFeatures_PICTYP[engineFeatures_TYP[i]];
		}
		if (engineFeatures_ICONFILE[i].Length > 0)
		{
			if (engineFeatures_PIC[i] == null)
			{
				engineFeatures_PIC[i] = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_EngineFeatures/" + engineFeatures_ICONFILE[i]);
			}
			if ((bool)engineFeatures_PIC[i])
			{
				return engineFeatures_PIC[i];
			}
		}
		return engineFeatures_PICTYP[engineFeatures_TYP[i]];
	}
}
