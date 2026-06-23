using System.IO;
using System.Text;
using UnityEngine;

public class platforms : MonoBehaviour
{
	public GameObject prefabPlatform;

	public mainScript mS_;

	private textScript tS_;

	private mpCalls mpCalls_;

	private settingsScript settings_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private GUI_Main guiMain_;

	private games games_;

	private gamepassScript gpS_;

	private string[] data;

	public Sprite[] complexSprites;

	public Sprite[] typSprites;

	public Sprite[] playerConsoleSprites;

	public Sprite[] playerHandheldSprites;

	public Color[] playerPlatformColors;

	public AnimationCurve productionCostsCurve;

	public AnimationCurve sellsCurve;

	public AnimationCurve priceCurve;

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
		if (!gF_)
		{
			gF_ = GetComponent<gameplayFeatures>();
		}
		if (!hardware_)
		{
			hardware_ = GetComponent<hardware>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = GetComponent<hardwareFeatures>();
		}
		if (!games_)
		{
			games_ = GetComponent<games>();
		}
		if (!gpS_)
		{
			gpS_ = GetComponent<gamepassScript>();
		}
		if (!settings_)
		{
			settings_ = GetComponent<settingsScript>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public platformScript CreatePlatform()
	{
		if (!mS_)
		{
			FindScripts();
		}
		platformScript component = Object.Instantiate(prefabPlatform).GetComponent<platformScript>();
		component.main_ = base.gameObject;
		component.mS_ = mS_;
		component.tS_ = tS_;
		component.settings_ = settings_;
		component.gF_ = gF_;
		component.platforms_ = this;
		component.hardware_ = hardware_;
		component.hardwareFeatures_ = hardwareFeatures_;
		component.guiMain_ = guiMain_;
		component.games_ = games_;
		component.gpS_ = gpS_;
		component.hwFeatures = new bool[hardwareFeatures_.hardFeat_UNLOCK.Length];
		mS_.FindPlatforms();
		return component;
	}

	public void LoadPlatforms(string filename)
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
		platformScript platformScript2 = null;
		for (int j = 0; j < data.Length; j++)
		{
			if (ParseData("[ID]", j))
			{
				platformScript2 = CreatePlatform();
				platformScript2.myID = int.Parse(data[j]);
				platformScript2.ownerID = -1;
				platformScript2.nachfolgerID = -1;
				platformScript2.vorgaengerID = -1;
				platformScript2.date_year_end = 999999;
				platformScript2.date_month_end = 1;
				platformScript2.pic2_year = 999999;
				platformScript2.Init();
			}
			if (!platformScript2)
			{
				continue;
			}
			if (ParseData("[PUB]", j))
			{
				platformScript2.ownerID = int.Parse(data[j]);
			}
			if (ParseData("[PRICE]", j))
			{
				platformScript2.price = int.Parse(data[j]);
			}
			if (ParseData("[DEV COSTS]", j))
			{
				platformScript2.dev_costs = int.Parse(data[j]);
			}
			if (ParseData("[TECHLEVEL]", j))
			{
				platformScript2.tech = int.Parse(data[j]);
			}
			if (ParseData("[PRE]", j))
			{
				platformScript2.vorgaengerID = int.Parse(data[j]);
			}
			if (ParseData("[SUC]", j))
			{
				platformScript2.nachfolgerID = int.Parse(data[j]);
			}
			if (ParseData("[TYP]", j))
			{
				platformScript2.typ = int.Parse(data[j]);
			}
			if (ParseData("[GAMEPASS]", j))
			{
				platformScript2.minGamePassGames = int.Parse(data[j]);
			}
			if (ParseData("[UNITS]", j))
			{
				platformScript2.units_max = int.Parse(data[j]);
				if (platformScript2.units_max > 500000000)
				{
					platformScript2.units_max = 500000000;
				}
				platformScript2.units_max += Random.Range(0, platformScript2.units_max / 20);
			}
			if (ParseData("[BACKCOMP-1]", j))
			{
				platformScript2.platformCompatible[0] = int.Parse(data[j]);
			}
			if (ParseData("[BACKCOMP-2]", j))
			{
				platformScript2.platformCompatible[1] = int.Parse(data[j]);
			}
			if (ParseData("[BACKCOMP-3]", j))
			{
				platformScript2.platformCompatible[2] = int.Parse(data[j]);
			}
			if (ParseData("[BACKCOMP-4]", j))
			{
				platformScript2.platformCompatible[3] = int.Parse(data[j]);
			}
			if (ParseData("[NEED-1]", j))
			{
				platformScript2.needFeatures[0] = int.Parse(data[j]);
			}
			if (ParseData("[NEED-2]", j))
			{
				platformScript2.needFeatures[1] = int.Parse(data[j]);
			}
			if (ParseData("[NEED-3]", j))
			{
				platformScript2.needFeatures[2] = int.Parse(data[j]);
			}
			if (ParseData("[DATE]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					platformScript2.date_month = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					platformScript2.date_month = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					platformScript2.date_month = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					platformScript2.date_month = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					platformScript2.date_month = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					platformScript2.date_month = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					platformScript2.date_month = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					platformScript2.date_month = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					platformScript2.date_month = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					platformScript2.date_month = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					platformScript2.date_month = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					platformScript2.date_month = 12;
				}
				if (platformScript2.date_month <= 0)
				{
					platformScript2.date_month = 1;
					Debug.Log("ERROR: Platform.txt -> Incorrect Month: " + platformScript2.myID);
				}
				platformScript2.date_year = int.Parse(data[j]);
			}
			if (ParseData("[DATE END]", j))
			{
				if (ParseDataDontCutLastChar("JAN", j))
				{
					platformScript2.date_month_end = 1;
				}
				if (ParseDataDontCutLastChar("FEB", j))
				{
					platformScript2.date_month_end = 2;
				}
				if (ParseDataDontCutLastChar("MAR", j))
				{
					platformScript2.date_month_end = 3;
				}
				if (ParseDataDontCutLastChar("APR", j))
				{
					platformScript2.date_month_end = 4;
				}
				if (ParseDataDontCutLastChar("MAY", j))
				{
					platformScript2.date_month_end = 5;
				}
				if (ParseDataDontCutLastChar("JUN", j))
				{
					platformScript2.date_month_end = 6;
				}
				if (ParseDataDontCutLastChar("JUL", j))
				{
					platformScript2.date_month_end = 7;
				}
				if (ParseDataDontCutLastChar("AUG", j))
				{
					platformScript2.date_month_end = 8;
				}
				if (ParseDataDontCutLastChar("SEP", j))
				{
					platformScript2.date_month_end = 9;
				}
				if (ParseDataDontCutLastChar("OCT", j))
				{
					platformScript2.date_month_end = 10;
				}
				if (ParseDataDontCutLastChar("NOV", j))
				{
					platformScript2.date_month_end = 11;
				}
				if (ParseDataDontCutLastChar("DEC", j))
				{
					platformScript2.date_month_end = 12;
				}
				if (platformScript2.date_month_end <= 0)
				{
					platformScript2.date_month_end = 1;
					Debug.Log("ERROR: Platform.txt -> Incorrect Month (END): " + platformScript2.myID);
				}
				platformScript2.date_year_end = int.Parse(data[j]);
			}
			if (ParseData("[PIC-1]", j))
			{
				platformScript2.pic1_file = data[j];
			}
			if (ParseData("[PIC-2]", j))
			{
				platformScript2.pic2_file = data[j];
			}
			if (ParseData("[PIC-2 YEAR]", j))
			{
				platformScript2.pic2_year = int.Parse(data[j]);
			}
			if (ParseData("[NAME EN]", j))
			{
				platformScript2.name_EN = data[j];
			}
			if (ParseData("[NAME GE]", j))
			{
				platformScript2.name_GE = data[j];
				if (platformScript2.name_EN == platformScript2.name_GE)
				{
					platformScript2.name_GE = "";
				}
			}
			if (ParseData("[NAME TU]", j))
			{
				platformScript2.name_TU = data[j];
				if (platformScript2.name_EN == platformScript2.name_TU)
				{
					platformScript2.name_TU = "";
				}
			}
			if (ParseData("[NAME CH]", j))
			{
				platformScript2.name_CH = data[j];
				if (platformScript2.name_EN == platformScript2.name_CH)
				{
					platformScript2.name_CH = "";
				}
			}
			if (ParseData("[NAME FR]", j))
			{
				platformScript2.name_FR = data[j];
				if (platformScript2.name_EN == platformScript2.name_FR)
				{
					platformScript2.name_FR = "";
				}
			}
			if (ParseData("[NAME HU]", j))
			{
				platformScript2.name_HU = data[j];
				if (platformScript2.name_EN == platformScript2.name_HU)
				{
					platformScript2.name_HU = "";
				}
			}
			if (ParseData("[NAME JA]", j))
			{
				platformScript2.name_JA = data[j];
				if (platformScript2.name_EN == platformScript2.name_JA)
				{
					platformScript2.name_JA = "";
				}
			}
			if (ParseData("[NAME PL]", j))
			{
				platformScript2.name_PL = data[j];
				if (platformScript2.name_EN == platformScript2.name_PL)
				{
					platformScript2.name_PL = "";
				}
			}
			if (ParseData("[NAME UA]", j))
			{
				platformScript2.name_UA = data[j];
				if (platformScript2.name_EN == platformScript2.name_UA)
				{
					platformScript2.name_UA = "";
				}
			}
			if (ParseData("[NAME TH]", j))
			{
				platformScript2.name_TH = data[j];
				if (platformScript2.name_EN == platformScript2.name_TH)
				{
					platformScript2.name_TH = "";
				}
			}
			if (ParseData("[MANUFACTURER EN]", j))
			{
				platformScript2.manufacturer_EN = data[j];
			}
			if (ParseData("[MANUFACTURER GE]", j))
			{
				platformScript2.manufacturer_GE = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_GE)
				{
					platformScript2.manufacturer_GE = "";
				}
			}
			if (ParseData("[MANUFACTURER TU]", j))
			{
				platformScript2.manufacturer_TU = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_TU)
				{
					platformScript2.manufacturer_TU = "";
				}
			}
			if (ParseData("[MANUFACTURER CH]", j))
			{
				platformScript2.manufacturer_CH = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_CH)
				{
					platformScript2.manufacturer_CH = "";
				}
			}
			if (ParseData("[MANUFACTURER FR]", j))
			{
				platformScript2.manufacturer_FR = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_FR)
				{
					platformScript2.manufacturer_FR = "";
				}
			}
			if (ParseData("[MANUFACTURER HU]", j))
			{
				platformScript2.manufacturer_HU = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_HU)
				{
					platformScript2.manufacturer_HU = "";
				}
			}
			if (ParseData("[MANUFACTURER JA]", j))
			{
				platformScript2.manufacturer_JA = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_JA)
				{
					platformScript2.manufacturer_JA = "";
				}
			}
			if (ParseData("[MANUFACTURER PL]", j))
			{
				platformScript2.manufacturer_PL = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_PL)
				{
					platformScript2.manufacturer_PL = "";
				}
			}
			if (ParseData("[MANUFACTURER UA]", j))
			{
				platformScript2.manufacturer_UA = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_UA)
				{
					platformScript2.manufacturer_UA = "";
				}
			}
			if (ParseData("[MANUFACTURER TH]", j))
			{
				platformScript2.manufacturer_TH = data[j];
				if (platformScript2.manufacturer_EN == platformScript2.manufacturer_TH)
				{
					platformScript2.manufacturer_TH = "";
				}
			}
			if (ParseData("[COMPLEX]", j))
			{
				platformScript2.complex = int.Parse(data[j]);
				if (platformScript2.complex > 2)
				{
					platformScript2.complex = 2;
				}
				if (platformScript2.complex < 0)
				{
					platformScript2.complex = 0;
				}
			}
			if (ParseData("[INTERNET]", j))
			{
				if (int.Parse(data[j]) == 1)
				{
					platformScript2.internet = true;
				}
				else
				{
					platformScript2.internet = false;
				}
			}
			if (ParseData("[STARTPLATFORM]", j))
			{
				platformScript2.inBesitz = true;
			}
			if (ParseData("[END]", j) && platformScript2.date_year == 1976 && platformScript2.date_month == 1)
			{
				platformScript2.isUnlocked = true;
				platformScript2.units = platformScript2.units_max / 2;
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

	public int GetDurchschnittsDevKitPreis()
	{
		int num = 1;
		int num2 = 1000;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].isUnlocked)
			{
				num++;
				num2 += mS_.arrayPlatformsScripts[i].price;
			}
		}
		return num2 / num;
	}

	public void UpdateGamesForPlatforms()
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i])
			{
				mS_.arrayPlatformsScripts[i].ZaehleGames();
			}
		}
	}

	public void UpdatePlatformSells(bool sendDataToClient, bool forceSendAll)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].inGamePass && !mS_.arrayPlatformsScripts[i].CanBeInGamePass(ignoreGames: false))
			{
				gpS_.GAMEPASS_RemovePlatform(mS_.arrayPlatformsScripts[i]);
			}
		}
		if (mS_.multiplayer && !mpCalls_.isServer)
		{
			for (int j = 0; j < mS_.arrayPlatformsScripts.Length; j++)
			{
				if ((bool)mS_.arrayPlatformsScripts[j] && mS_.arrayPlatformsScripts[j].ownerID == mS_.myID)
				{
					if (!mS_.arrayPlatformsScripts[j].isUnlocked)
					{
						mS_.arrayPlatformsScripts[j].weeksInDevelopment++;
					}
					if (mS_.arrayPlatformsScripts[j].isUnlocked && !mS_.arrayPlatformsScripts[j].vomMarktGenommen)
					{
						mS_.arrayPlatformsScripts[j].Sell();
						mpCalls_.CLIENT_Send_Platform(mS_.arrayPlatformsScripts[j]);
					}
				}
			}
			return;
		}
		long num = 0L;
		long num2 = 0L;
		long num3 = 0L;
		long num4 = 0L;
		long num5 = 0L;
		for (int k = 0; k < mS_.arrayPlatformsScripts.Length; k++)
		{
			if (!mS_.arrayPlatformsScripts[k])
			{
				continue;
			}
			if (mS_.arrayPlatformsScripts[k].ownerID == mS_.myID && !mS_.arrayPlatformsScripts[k].isUnlocked)
			{
				mS_.arrayPlatformsScripts[k].weeksInDevelopment++;
			}
			mS_.arrayPlatformsScripts[k].Sell();
			if (mS_.arrayPlatformsScripts[k].isUnlocked && mS_.arrayPlatformsScripts[k].vomMarktGenommen && mS_.arrayPlatformsScripts[k].powerFromMarket > 0f)
			{
				mS_.arrayPlatformsScripts[k].powerFromMarket -= 0.05f;
				if (mS_.arrayPlatformsScripts[k].powerFromMarket < 0f)
				{
					mS_.arrayPlatformsScripts[k].powerFromMarket = 0f;
				}
			}
			if (!mS_.arrayPlatformsScripts[k].OwnerIsNPC() && mS_.arrayPlatformsScripts[k].isUnlocked && !mS_.arrayPlatformsScripts[k].vomMarktGenommen && mS_.arrayPlatformsScripts[k].powerFromMarket > 0f && mS_.arrayPlatformsScripts[k].sellsPerWeek[0] <= 0)
			{
				mS_.arrayPlatformsScripts[k].powerFromMarket -= 0.003f;
				if (mS_.arrayPlatformsScripts[k].powerFromMarket < 0f)
				{
					mS_.arrayPlatformsScripts[k].powerFromMarket = 0f;
				}
			}
			if (!mS_.arrayPlatformsScripts[k].IsVerfuegbar() && (!mS_.arrayPlatformsScripts[k].isUnlocked || !mS_.arrayPlatformsScripts[k].vomMarktGenommen || !(mS_.arrayPlatformsScripts[k].powerFromMarket > 0f)))
			{
				continue;
			}
			if (!mS_.arrayPlatformsScripts[k].vomMarktGenommen)
			{
				switch (mS_.arrayPlatformsScripts[k].typ)
				{
				case 0:
					num += mS_.arrayPlatformsScripts[k].units;
					break;
				case 1:
					num2 += mS_.arrayPlatformsScripts[k].units;
					break;
				case 2:
					num3 += mS_.arrayPlatformsScripts[k].units;
					break;
				case 3:
					num4 += mS_.arrayPlatformsScripts[k].units;
					break;
				case 4:
					num5 += mS_.arrayPlatformsScripts[k].units;
					break;
				default:
					num += mS_.arrayPlatformsScripts[k].units;
					break;
				}
				continue;
			}
			float num6 = mS_.arrayPlatformsScripts[k].units;
			num6 *= mS_.arrayPlatformsScripts[k].powerFromMarket;
			switch (mS_.arrayPlatformsScripts[k].typ)
			{
			case 0:
				num += Mathf.RoundToInt(num6);
				break;
			case 1:
				num2 += Mathf.RoundToInt(num6);
				break;
			case 2:
				num3 += Mathf.RoundToInt(num6);
				break;
			case 3:
				num4 += Mathf.RoundToInt(num6);
				break;
			case 4:
				num5 += Mathf.RoundToInt(num6);
				break;
			default:
				num += Mathf.RoundToInt(num6);
				break;
			}
		}
		for (int l = 0; l < mS_.arrayPlatformsScripts.Length; l++)
		{
			if ((bool)mS_.arrayPlatformsScripts[l])
			{
				switch (mS_.arrayPlatformsScripts[l].typ)
				{
				case 0:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num);
					break;
				case 1:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num2);
					break;
				case 2:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num3);
					break;
				case 3:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num4);
					break;
				case 4:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num5);
					break;
				default:
					mS_.arrayPlatformsScripts[l].SetMarktanteil(num);
					break;
				}
				if (mS_.multiplayer && mpCalls_.isServer && sendDataToClient && (mS_.arrayPlatformsScripts[l].IsVerfuegbar() || (mS_.arrayPlatformsScripts[l].isUnlocked && mS_.arrayPlatformsScripts[l].vomMarktGenommen && mS_.arrayPlatformsScripts[l].powerFromMarket > 0f) || forceSendAll))
				{
					mpCalls_.SERVER_Send_PlatformData(mS_.arrayPlatformsScripts[l]);
				}
			}
		}
		gpS_.GAMEPASS_UpdateMarketshare();
	}

	public Sprite GetPlayerPlatformSprite(int id_, int platTyp_)
	{
		return platTyp_ switch
		{
			1 => playerConsoleSprites[id_], 
			2 => playerConsoleSprites[id_], 
			_ => null, 
		};
	}

	public int GetPerformance(platformScript script_)
	{
		int num = 0;
		if ((bool)script_)
		{
			if (script_.component_cpu != -1)
			{
				num += hardware_.GetPerformance(script_.component_cpu);
			}
			if (script_.component_gfx != -1)
			{
				num += hardware_.GetPerformance(script_.component_gfx);
			}
			if (script_.component_ram != -1)
			{
				num += hardware_.GetPerformance(script_.component_ram);
			}
			if (script_.component_hdd != -1)
			{
				num += hardware_.GetPerformance(script_.component_hdd);
			}
			if (script_.component_sfx != -1)
			{
				num += hardware_.GetPerformance(script_.component_sfx);
			}
			if (script_.component_cooling != -1)
			{
				num += hardware_.GetPerformance(script_.component_cooling);
			}
			if (script_.component_disc != -1)
			{
				num += hardware_.GetPerformance(script_.component_disc);
			}
			_ = script_.component_controller;
			_ = -1;
			_ = script_.component_case;
			_ = -1;
			if (script_.component_monitor != -1)
			{
				num += hardware_.GetPerformance(script_.component_monitor);
			}
		}
		return num;
	}

	public float GetSellsCurve()
	{
		float num = mS_.PassedMonth();
		if (num > 600f)
		{
			num = 600f;
		}
		return sellsCurve.Evaluate(num / 600f) * 1f;
	}

	public float GetPriceCurve(int price)
	{
		float num = price;
		if (price > 1000)
		{
			price = 1000;
		}
		num = 1f - (float)price / 1000f;
		return priceCurve.Evaluate(num);
	}

	public int GetAmountOfPlatformsWithSubvention()
	{
		int num = 0;
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && !mS_.arrayPlatformsScripts[i].OwnerIsNPC() && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].isUnlocked && mS_.arrayPlatformsScripts[i].thridPartyGames && !mS_.arrayPlatformsScripts[i].IsProConsoleInDev() && mS_.arrayPlatformsScripts[i].subventionMoney > 0)
			{
				num++;
			}
		}
		return num;
	}

	public bool ExistInternetReadyConsole()
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].isUnlocked && mS_.arrayPlatformsScripts[i].internet && (mS_.arrayPlatformsScripts[i].typ == 1 || mS_.arrayPlatformsScripts[i].typ == 2))
			{
				return true;
			}
		}
		return false;
	}
}
