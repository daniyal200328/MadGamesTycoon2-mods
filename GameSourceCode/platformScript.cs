using UnityEngine;
using UnityEngine.UI;

public class platformScript : MonoBehaviour
{
	public GameObject main_;

	public mainScript mS_;

	public settingsScript settings_;

	public textScript tS_;

	public gameplayFeatures gF_;

	public platforms platforms_;

	public hardware hardware_;

	public hardwareFeatures hardwareFeatures_;

	public GUI_Main guiMain_;

	public games games_;

	public gamepassScript gpS_;

	public konsoleTab konsoleTab_;

	public int myID;

	public int ownerID = -1;

	public int date_year;

	public int date_month;

	public int date_year_end;

	public int date_month_end;

	public int price;

	public int dev_costs;

	public int tech;

	public int typ;

	public int minGamePassGames;

	public bool inGamePass;

	public bool inGamePassPassiv;

	public float marktanteil;

	public int[] needFeatures;

	public int units;

	public int units_max;

	public string name_EN = "";

	public string name_GE = "";

	public string name_TU = "";

	public string name_CH = "";

	public string name_FR = "";

	public string name_HU = "";

	public string name_JA = "";

	public string name_PL = "";

	public string name_UA = "";

	public string name_TH = "";

	public string manufacturer_EN = "";

	public string manufacturer_GE = "";

	public string manufacturer_TU = "";

	public string manufacturer_CH = "";

	public string manufacturer_FR = "";

	public string manufacturer_HU = "";

	public string manufacturer_JA = "";

	public string manufacturer_PL = "";

	public string manufacturer_UA = "";

	public string manufacturer_TH = "";

	private Sprite pic1;

	private Sprite pic2;

	public string pic1_file = "";

	public string pic2_file = "";

	public int pic2_year;

	public int games;

	public int exklusivGames;

	public int games_gamePass;

	public int gamesInDev;

	public int games_S0;

	public int exklusivGames_S0;

	public int games_S1;

	public int exklusivGames_S1;

	public int games_S2;

	public int exklusivGames_S2;

	public int games_S3;

	public int exklusivGames_S3;

	public int games_S4;

	public int exklusivGames_S4;

	public int games_S5;

	public int exklusivGames_S5;

	public int erfahrung;

	public bool isUnlocked;

	public bool inBesitz;

	public bool vomMarktGenommen;

	public int complex;

	public bool internet;

	public float powerFromMarket = 1f;

	public bool angekuendigt;

	public string myName;

	public int gameID = -1;

	public gameScript vorinstalledGame_;

	public int anzController;

	public Vector3 consoleColor = new Vector3(0.5f, 0.5f, 0.5f);

	public int component_cpu = -1;

	public int component_gfx = -1;

	public int component_ram = -1;

	public int component_hdd = -1;

	public int component_sfx = -1;

	public int component_cooling = -1;

	public int component_disc = -1;

	public int component_controller = -1;

	public int component_case = -1;

	public int component_monitor = -1;

	public bool[] hwFeatures;

	public float devPoints;

	public float devPointsStart;

	public long entwicklungsKosten;

	public long einnahmen;

	public float hype;

	public int costs_marketing;

	public int costs_mitarbeiter;

	public int costs_preisreduktion;

	public int costs_haltbarkeit;

	public long costs_subvention;

	public long costs_garantie;

	public int startProduktionskosten;

	public int verkaufspreis = 399;

	public float kostenreduktion;

	public bool autoPreis;

	public bool thridPartyGames;

	public long umsatzTotal;

	public long costs_production;

	public int[] sellsPerWeek;

	public int weeksOnMarket;

	public float review;

	public bool[] publisherBuyed;

	public int performancePoints;

	public int autoPreisGewinn;

	public int weeksInDevelopment;

	public int[] platformCompatible = new int[4];

	public platformScript[] platformCompatibleScript = new platformScript[4];

	public int nachfolgerID = -1;

	public int vorgaengerID = -1;

	public platformScript nachfolgerScript;

	public platformScript vorgaengerScript;

	public bool[] kostenreduktionDone = new bool[9];

	public float haltbarkeit;

	public int garantie = 1;

	public int[] garantieMonth = new int[60];

	public int garantiefaelle;

	public long garantiekosten;

	public bool[] haltbarkeitDone = new bool[9];

	public int subventionMoney;

	public bool[] subventionGameSize = new bool[6];

	public bool proVersion;

	public string proName = "";

	public bool konsoleTab_fullView = true;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!platforms_)
		{
			platforms_ = main_.GetComponent<platforms>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!gF_)
		{
			gF_ = main_.GetComponent<gameplayFeatures>();
		}
		if (!hardware_)
		{
			hardware_ = main_.GetComponent<hardware>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!gpS_)
		{
			gpS_ = main_.GetComponent<gamepassScript>();
		}
		if (!hardwareFeatures_)
		{
			hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void Init()
	{
		base.name = "PLATFORM_" + myID;
		if (proName == null)
		{
			proName = "";
		}
	}

	public float GetHype()
	{
		return hype;
	}

	public string GetName()
	{
		if (!OwnerIsNPC())
		{
			return myName;
		}
		string text = "";
		text = settings_.language switch
		{
			0 => name_EN, 
			1 => name_GE, 
			2 => name_TU, 
			3 => name_CH, 
			4 => name_FR, 
			8 => name_HU, 
			11 => name_PL, 
			16 => name_JA, 
			17 => name_UA, 
			19 => name_TH, 
			_ => name_EN, 
		};
		if (text == null)
		{
			return name_EN;
		}
		if (text.Length <= 0)
		{
			return name_EN;
		}
		return text;
	}

	public bool IsProKonsole()
	{
		if (!proVersion)
		{
			return false;
		}
		if (proVersion && GetProName().Length > 0)
		{
			return true;
		}
		return false;
	}

	public string GetProName()
	{
		if (proName == null)
		{
			return "";
		}
		if (proName.Length <= 0)
		{
			return "";
		}
		return proName;
	}

	public string GetManufacturer()
	{
		if (ownerID == mS_.myID)
		{
			return mS_.GetCompanyName();
		}
		if (mS_.multiplayer)
		{
			if (!OwnerIsNPC() && (bool)mS_.mpCalls_)
			{
				return mS_.mpCalls_.GetCompanyName(ownerID);
			}
			return "";
		}
		string text = "";
		text = settings_.language switch
		{
			0 => manufacturer_EN, 
			1 => manufacturer_GE, 
			2 => manufacturer_TU, 
			3 => manufacturer_CH, 
			4 => manufacturer_FR, 
			8 => manufacturer_HU, 
			11 => manufacturer_PL, 
			16 => manufacturer_JA, 
			17 => manufacturer_UA, 
			19 => manufacturer_TH, 
			_ => manufacturer_EN, 
		};
		if (text == null)
		{
			return manufacturer_EN;
		}
		if (text.Length <= 0)
		{
			return manufacturer_EN;
		}
		return text;
	}

	public void SetPic(GameObject go)
	{
		if (!OwnerIsNPC())
		{
			if (component_case != -1)
			{
				go.GetComponent<Image>().material = mS_.specialMaterials[6];
				go.GetComponent<Image>().sprite = hardware_.GetTypPic(component_case);
				go.GetComponent<Image>().color = new Color(consoleColor.x, consoleColor.y, consoleColor.z);
			}
			return;
		}
		if (pic1_file.Length > 0 && !pic1)
		{
			pic1 = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_Platforms/" + pic1_file);
		}
		if (pic2_file.Length > 0 && !pic2)
		{
			pic2 = mS_.LoadPNG(Application.dataPath + "/Extern/Icons_Platforms/" + pic2_file);
		}
		if (!pic2)
		{
			go.GetComponent<Image>().material = null;
			go.GetComponent<Image>().color = Color.white;
			go.GetComponent<Image>().sprite = pic1;
		}
		else if (mS_.year >= pic2_year)
		{
			go.GetComponent<Image>().material = null;
			go.GetComponent<Image>().color = Color.white;
			go.GetComponent<Image>().sprite = pic2;
		}
		else
		{
			go.GetComponent<Image>().material = null;
			go.GetComponent<Image>().color = Color.white;
			go.GetComponent<Image>().sprite = pic1;
		}
	}

	public int GetVerkaufspreis()
	{
		return verkaufspreis;
	}

	public int GetAktuellProductionsCosts()
	{
		float num = startProduktionskosten - (50 + anzController * 5);
		num -= num * (kostenreduktion * 0.01f);
		num += (float)(50 + anzController * 5);
		if (num < 59f)
		{
			num = 59f;
		}
		if (num > 2000f)
		{
			num = 2000f;
		}
		if (mS_.globalEvent == 12)
		{
			num += 100f;
		}
		return Mathf.RoundToInt(num);
	}

	public int CalcStartProductionsCosts()
	{
		float num = 0f;
		float num2 = 0f;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID != myID && mS_.arrayPlatformsScripts[i].isUnlocked && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && (mS_.arrayPlatformsScripts[i].typ == 1 || mS_.arrayPlatformsScripts[i].typ == 2))
			{
				num += 1f;
				num2 += (float)mS_.arrayPlatformsScripts[i].tech;
			}
		}
		float num3 = 0f;
		for (int j = 0; j < hwFeatures.Length; j++)
		{
			if (hwFeatures[j])
			{
				num3 += 5f;
			}
		}
		float num4 = tech;
		if (num > 0f)
		{
			num4 = num2 / num;
		}
		float num5 = (float)tech - num4;
		num5 += 10f;
		num5 /= 20f;
		float num6 = platforms_.productionCostsCurve.Evaluate(num5);
		float num7 = 0f;
		num7 = mS_.difficulty switch
		{
			0 => (800f + num3) * num6, 
			1 => (900f + num3) * num6, 
			2 => (1000f + num3) * num6, 
			3 => (1100f + num3) * num6, 
			4 => (1150f + num3) * num6, 
			5 => (1200f + num3) * num6, 
			_ => (1200f + num3) * num6, 
		};
		if (typ == 1)
		{
			num7 += (float)(anzController * 15);
		}
		if (num7 < 80f || num7 > 5000f)
		{
			num7 = 80f;
		}
		return Mathf.RoundToInt(num7);
	}

	public int GetPrice()
	{
		return price;
	}

	public int GetDevCosts()
	{
		return dev_costs;
	}

	public long GetGesamtAusgaben()
	{
		return entwicklungsKosten + costs_mitarbeiter + costs_marketing + costs_production + costs_preisreduktion + costs_haltbarkeit + costs_garantie + costs_subvention;
	}

	public long GetEntwicklungskosten()
	{
		return entwicklungsKosten + costs_mitarbeiter + costs_preisreduktion + costs_haltbarkeit;
	}

	public long GetGesamtGewinn()
	{
		return umsatzTotal - GetGesamtAusgaben();
	}

	public float GetMarktanteil()
	{
		if (typ == 4)
		{
			return 100f;
		}
		return marktanteil;
	}

	public float GetProzent()
	{
		return 100f / devPointsStart * (devPointsStart - devPoints);
	}

	public string GetMarktanteilString()
	{
		if (typ == 4)
		{
			return "-";
		}
		return mS_.Round(marktanteil, 1) + "%|" + mS_.Round((float)units / 1000000f * powerFromMarket, 1) + " " + tS_.GetText(1483);
	}

	public int GetAktiveNutzer()
	{
		return Mathf.RoundToInt((float)units * powerFromMarket);
	}

	public void SetMarktanteil(long gesamtUnits)
	{
		marktanteil = 0f;
		if (IsVerfuegbar() || (vomMarktGenommen && isUnlocked && powerFromMarket > 0f))
		{
			if (!vomMarktGenommen)
			{
				float num = gesamtUnits;
				float num2 = units;
				num2 *= powerFromMarket;
				marktanteil = num2 / (num / 100f);
			}
			else
			{
				float num3 = gesamtUnits;
				float num4 = units;
				num4 *= powerFromMarket;
				marktanteil = num4 / (num3 / 100f);
			}
		}
	}

	public bool IsProConsoleInDev()
	{
		if (proVersion && !isUnlocked)
		{
			return true;
		}
		return false;
	}

	public int GetGames()
	{
		return games;
	}

	public int GetGamesInDev()
	{
		return gamesInDev;
	}

	public int GetExklusivGames()
	{
		return exklusivGames;
	}

	public string GetDateString()
	{
		if (!isUnlocked)
		{
			return tS_.GetText(528);
		}
		return date_year + " " + tS_.GetText(date_month + 220);
	}

	public string GetDateStringEnd()
	{
		if (!isUnlocked)
		{
			return tS_.GetText(528);
		}
		return date_year_end + " " + tS_.GetText(date_month_end + 220);
	}

	public string GetTooltip()
	{
		if (!mS_)
		{
			FindScripts();
		}
		string text = "<b><size=18>" + GetName() + "</size></b>";
		if (proVersion && GetProName().Length > 0)
		{
			text = text + "\n<size=12>[" + GetProName() + "]</size>";
		}
		if (isUnlocked && !vomMarktGenommen)
		{
			text = text + "\n<b>" + GetDateString() + "</b>";
		}
		if (isUnlocked && vomMarktGenommen)
		{
			string text2 = tS_.GetText(1673);
			text2 = text2.Replace("<DATE1>", GetDateString());
			text2 = text2.Replace("<DATE2>", GetDateStringEnd());
			text = text + "\n<b>" + text2 + "</b>";
		}
		text = text + "\n<b><color=blue>" + GetTypString() + "</color></b>";
		text = text + "\n<b><color=magenta>" + tS_.GetText(4) + " " + tech + "</color></b>\n";
		if (!OwnerIsNPC())
		{
			text = text + "<color=green>" + tS_.GetText(1612) + ": " + performancePoints + " </color>\n";
		}
		text = text + "\n" + tS_.GetText(216) + ": <color=blue>" + GetManufacturer() + "</color>";
		if (IsAngekuendigt())
		{
			text = text + "\n" + tS_.GetText(217) + ": <color=blue>" + date_year + " " + tS_.GetText(date_month + 220) + "</color>";
		}
		if (typ != 4 && units > 0)
		{
			text = text + "\n" + tS_.GetText(275) + ": <color=blue>" + mS_.GetMoney(units, showDollar: false) + "</color>";
		}
		if (isUnlocked)
		{
			text = text + "\n" + tS_.GetText(219) + ": <color=blue>" + GetMarktanteilString() + "</color>";
		}
		if (isUnlocked)
		{
			text = text + "\n" + tS_.GetText(220) + ": <color=blue>" + mS_.GetMoney(GetGames(), showDollar: false) + "</color>";
		}
		if (isUnlocked)
		{
			text = text + "\n" + tS_.GetText(1863) + ": <color=blue>" + mS_.GetMoney(GetExklusivGames(), showDollar: false) + "</color>";
		}
		if (isUnlocked && ownerID == mS_.myID)
		{
			text = text + "\n" + tS_.GetText(2439) + ": <color=blue>" + mS_.GetMoney(GetGamesInDev(), showDollar: false) + "</color>";
		}
		if (isUnlocked && !OwnerIsNPC())
		{
			text = text + "\n" + tS_.GetText(1653) + ": <color=blue>" + mS_.Round(review, 1) + " / 10</color>";
			if (ownerID == mS_.myID)
			{
				text = text + "\n\n" + tS_.GetText(2368) + ": <color=blue>" + mS_.Round(GetHaltbarkeit(), 1) + " / 10</color>";
				text = text + "\n" + tS_.GetText(530) + ": <color=blue>" + mS_.GetMoney(GetAktuellProductionsCosts(), showDollar: true) + " [" + mS_.Round(kostenreduktion, 1) + "%]</color>";
				text = text + "\n" + tS_.GetText(627) + ": <color=blue>" + mS_.Round(GetSellsBonusForGames(), 1) + "%</color>";
			}
		}
		text += "\n";
		if (isUnlocked)
		{
			text = text + "\n" + tS_.GetText(218) + ": <color=blue>" + mS_.GetMoney(price, showDollar: true) + "</color>";
		}
		if (isUnlocked)
		{
			text = text + "\n" + tS_.GetText(6) + ": <color=blue>" + mS_.GetMoney(dev_costs, showDollar: true) + "</color>";
		}
		if (typ != 3 && typ != 4 && isUnlocked && GetExklusivBonus() > 0f)
		{
			text = text + "\n" + tS_.GetText(1926) + ": <color=blue>-" + Mathf.RoundToInt(GetExklusivBonus()) + "%</color>";
		}
		if (ownerID == mS_.myID && isUnlocked)
		{
			text += "\n\n";
			text = text + tS_.GetText(1575) + ": <color=red>" + mS_.GetMoney(GetGesamtAusgaben(), showDollar: true) + "</color>\n";
			text = text + tS_.GetText(276) + ": <color=blue>" + mS_.GetMoney(umsatzTotal, showDollar: true) + "</color>\n";
			text = ((GetGesamtGewinn() < 0) ? (text + tS_.GetText(724) + ": <color=red>" + mS_.GetMoney(GetGesamtGewinn(), showDollar: true) + "</color>\n") : (text + tS_.GetText(724) + ": <color=blue>" + mS_.GetMoney(GetGesamtGewinn(), showDollar: true) + "</color>\n"));
		}
		text += "\n";
		if (vorgaengerID != -1)
		{
			if (!vorgaengerScript)
			{
				FindMyVorgaengerScript();
			}
			if ((bool)vorgaengerScript && vorgaengerScript.isUnlocked)
			{
				text = text + "\n" + tS_.GetText(2317) + "\n";
				text += "<size=12>";
				text = ((!vorgaengerScript.IsVerfuegbar()) ? (text + "<color=red>" + vorgaengerScript.GetName() + "</color>\n") : (text + "<color=grey>" + vorgaengerScript.GetName() + "</color>\n"));
				text += "</size>";
			}
		}
		if (nachfolgerID != -1)
		{
			if (!nachfolgerScript)
			{
				FindMyNachfolgerScript();
			}
			if ((bool)nachfolgerScript && nachfolgerScript.isUnlocked)
			{
				text = text + "\n" + tS_.GetText(2318) + "\n";
				text += "<size=12>";
				text = ((!nachfolgerScript.IsVerfuegbar()) ? (text + "<color=red>" + nachfolgerScript.GetName() + "</color>\n") : (text + "<color=grey>" + nachfolgerScript.GetName() + "</color>\n"));
				text += "</size>";
			}
		}
		if (HasAbwaertkompatible())
		{
			text = text + "\n" + tS_.GetText(2145) + "\n";
			text += "<size=12>";
			if (!platformCompatibleScript[0])
			{
				FindMyPlatformsCompatible();
			}
			for (int i = 0; i < platformCompatibleScript.Length; i++)
			{
				if ((bool)platformCompatibleScript[i])
				{
					text = ((!platformCompatibleScript[i].IsVerfuegbar()) ? (text + "<color=red>" + platformCompatibleScript[i].GetName() + "</color>\n") : (text + "<color=grey>" + platformCompatibleScript[i].GetName() + "</color>\n"));
				}
			}
			text += "</size>";
		}
		if (internet)
		{
			text = text + "\n<b><color=magenta>" + tS_.GetText(1261) + "</color></b>";
		}
		if (inGamePass && gpS_.gamePass_aktiv)
		{
			text = text + "\n<b><color=magenta>" + tS_.GetText(2130) + "</color></b>";
		}
		text = text + "\n\n<b>" + tS_.GetText(1019) + "<color=blue>";
		if (needFeatures[0] != -1)
		{
			text = text + "\n" + gF_.GetName(needFeatures[0]);
		}
		if (needFeatures[1] != -1)
		{
			text = text + "\n" + gF_.GetName(needFeatures[1]);
		}
		if (needFeatures[2] != -1)
		{
			text = text + "\n" + gF_.GetName(needFeatures[2]);
		}
		text += "</color></b>";
		if (vomMarktGenommen)
		{
			text = text + "<b>\n<color=red>" + tS_.GetText(233) + "</color></b>";
		}
		return text;
	}

	public bool HasAbwaertkompatible()
	{
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			if (platformCompatible[i] != -1)
			{
				return true;
			}
		}
		return false;
	}

	public void FindMyNachfolgerScript()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (nachfolgerID != -1 && !nachfolgerScript)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + nachfolgerID);
			if ((bool)gameObject)
			{
				nachfolgerScript = gameObject.GetComponent<platformScript>();
			}
		}
	}

	public void FindMyVorgaengerScript()
	{
		if (!mS_)
		{
			FindScripts();
		}
		if (vorgaengerID != -1 && !vorgaengerScript)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + vorgaengerID);
			if ((bool)gameObject)
			{
				vorgaengerScript = gameObject.GetComponent<platformScript>();
			}
		}
	}

	public void FindMyPlatformsCompatible()
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			if (platformCompatible[i] != -1)
			{
				platformCompatibleScript[i] = FindPlatformScriptWithID(platformCompatible[i]);
				if (!platformCompatibleScript[i])
				{
					platformCompatible[i] = -1;
					platformCompatibleScript[i] = null;
				}
			}
		}
	}

	private platformScript FindPlatformScriptWithID(int id_)
	{
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID == id_)
			{
				return mS_.arrayPlatformsScripts[i];
			}
		}
		return null;
	}

	public bool IsVerfuegbar()
	{
		if (!isUnlocked)
		{
			return false;
		}
		if (vomMarktGenommen)
		{
			return false;
		}
		return true;
	}

	public void Sell()
	{
		if (IsVerfuegbar())
		{
			weeksOnMarket++;
		}
		if (OwnerIsNPC())
		{
			SellNPC();
		}
		else
		{
			SellPlayer();
		}
	}

	public void BonusSellsExklusiv(int i)
	{
		if (IsVerfuegbar() && OwnerIsNPC())
		{
			units += i;
			units_max += i;
			if (units > 400000000)
			{
				units = 400000000;
			}
			if (units_max > 400000000)
			{
				units_max = 400000000;
			}
		}
	}

	private void SellNPC()
	{
		if (IsVerfuegbar())
		{
			int num = date_year_end - date_year;
			if (num > 10)
			{
				num = 10;
			}
			int num2 = units_max / num / 48;
			if (weeksOnMarket <= 10)
			{
				num2 *= 5;
			}
			if (weeksOnMarket > 10 && weeksOnMarket <= 20)
			{
				num2 *= 5;
			}
			if (weeksOnMarket > 20 && weeksOnMarket <= 30)
			{
				num2 *= 4;
			}
			if (weeksOnMarket > 30 && weeksOnMarket <= 40)
			{
				num2 *= 4;
			}
			if (weeksOnMarket > 40 && weeksOnMarket <= 50)
			{
				num2 *= 3;
			}
			if (weeksOnMarket > 50 && weeksOnMarket <= 60)
			{
				num2 *= 3;
			}
			if (weeksOnMarket > 60 && weeksOnMarket <= 70)
			{
				num2 *= 2;
			}
			if (weeksOnMarket > 70 && weeksOnMarket <= 80)
			{
				num2 *= 2;
			}
			if (weeksOnMarket > 80 && weeksOnMarket <= 90)
			{
				num2 *= 2;
			}
			units += num2;
			if (units > units_max)
			{
				units = units_max;
			}
			if (units > 400000000)
			{
				units = 400000000;
			}
			if (units_max > 400000000)
			{
				units_max = 400000000;
			}
		}
	}

	public void AutoPreis()
	{
		if (autoPreis)
		{
			verkaufspreis = GetAktuellProductionsCosts() + autoPreisGewinn;
		}
	}

	private void SellPlayer()
	{
		if (ownerID != mS_.myID || vomMarktGenommen || !isUnlocked)
		{
			return;
		}
		ZaehleGames();
		for (int num = sellsPerWeek.Length - 1; num >= 1; num--)
		{
			sellsPerWeek[num] = sellsPerWeek[num - 1];
		}
		float num2 = 0f;
		AutoPreis();
		float num3 = 0f;
		if (vorgaengerID != -1)
		{
			if (!vorgaengerScript)
			{
				FindMyVorgaengerScript();
			}
			if ((bool)vorgaengerScript)
			{
				num3 = (float)vorgaengerScript.units / 500f;
			}
		}
		num2 = 400000f + num3 + (float)(performancePoints * 300);
		num2 -= (float)(weeksOnMarket * 400);
		if (nachfolgerID != -1)
		{
			if (!nachfolgerScript)
			{
				FindMyNachfolgerScript();
			}
			if ((bool)nachfolgerScript && nachfolgerScript.IsVerfuegbar())
			{
				num2 *= 0.8f;
			}
		}
		num2 -= (float)(verkaufspreis * 50);
		num2 += (float)Random.Range(1000, 2000);
		if (num2 < 10000f)
		{
			num2 = 10000f;
		}
		num2 *= platforms_.GetSellsCurve();
		switch (weeksOnMarket)
		{
		case 1:
			num2 *= 1.3f;
			break;
		case 2:
			num2 *= 1.4f;
			break;
		case 3:
			num2 *= 1.5f;
			break;
		case 4:
			num2 *= 1.4f;
			break;
		case 5:
			num2 *= 1.1f;
			break;
		}
		float num4 = weeksOnMarket;
		if (proVersion && GetProName().Length > 0)
		{
			num4 *= 0.7f;
		}
		num4 *= 0.002f;
		if (num4 > 0.99f)
		{
			num4 = 0.99f;
		}
		num2 -= num2 * num4;
		int amountOfKonsolesSameTyp = GetAmountOfKonsolesSameTyp();
		num2 -= num2 * 0.03f * (float)amountOfKonsolesSameTyp;
		int amountOfOwnKonsolesSameTyp = GetAmountOfOwnKonsolesSameTyp();
		if (amountOfOwnKonsolesSameTyp > 1)
		{
			num2 /= (float)amountOfOwnKonsolesSameTyp;
		}
		int amountOfOwnKonsolesSameTypRemoved = GetAmountOfOwnKonsolesSameTypRemoved();
		if (amountOfOwnKonsolesSameTypRemoved >= 1)
		{
			num2 /= (float)amountOfOwnKonsolesSameTypRemoved;
		}
		float techDifference = GetTechDifference();
		switch (Mathf.RoundToInt(techDifference))
		{
		case -10:
			num2 -= num2 * 0.99f;
			break;
		case -9:
			num2 -= num2 * 0.99f;
			break;
		case -8:
			num2 -= num2 * 0.99f;
			break;
		case -7:
			num2 -= num2 * 0.99f;
			break;
		case -6:
			num2 -= num2 * 0.99f;
			break;
		case -5:
			num2 -= num2 * 0.98f;
			break;
		case -4:
			num2 -= num2 * 0.95f;
			break;
		case -3:
			num2 -= num2 * 0.85f;
			break;
		case -2:
			num2 -= num2 * 0.75f;
			break;
		case -1:
			num2 -= num2 * 0.5f;
			break;
		case 0:
			num2 *= 1f;
			break;
		case 1:
			num2 *= 1.5f;
			break;
		case 2:
			num2 *= 2f;
			break;
		case 3:
			num2 *= 2.5f;
			break;
		case 4:
			num2 *= 3f;
			break;
		case 5:
			num2 *= 3.5f;
			break;
		case 6:
			num2 *= 4f;
			break;
		case 7:
			num2 *= 4.5f;
			break;
		case 8:
			num2 *= 5f;
			break;
		case 9:
			num2 *= 5.5f;
			break;
		case 10:
			num2 *= 5f;
			break;
		}
		num2 = ((!(techDifference >= 0f)) ? (num2 + num2 * (techDifference * 0.09f)) : (num2 + num2 * (techDifference * 0.09f)));
		if (!internet && hardwareFeatures_.hardFeat_UNLOCK[0] && platforms_.ExistInternetReadyConsole())
		{
			num2 *= 0.5f;
		}
		for (int i = 1; i < hwFeatures.Length; i++)
		{
			if (!hardwareFeatures_.hardFeat_UNLOCK[i] || hwFeatures[i])
			{
				continue;
			}
			if (typ == 1)
			{
				if (!hardwareFeatures_.hardFeat_ONLYHANDHELD[i])
				{
					num2 *= 1f - hardwareFeatures_.hardFeat_QUALITY[i] * 0.01f;
				}
			}
			else if (!hardwareFeatures_.hardFeat_ONLYSTATIONARY[i])
			{
				num2 *= 1f - hardwareFeatures_.hardFeat_QUALITY[i] * 0.01f;
			}
		}
		if (typ == 1)
		{
			switch (anzController)
			{
			case 1:
				num2 += num2 * 0.02f;
				break;
			case 2:
				num2 += num2 * 0.04f;
				break;
			case 3:
				num2 += num2 * 0.06f;
				break;
			case 4:
				num2 += num2 * 0.08f;
				break;
			}
		}
		switch (garantie)
		{
		case 2:
			num2 += num2 * 0.05f;
			break;
		case 3:
			num2 += num2 * 0.1f;
			break;
		case 4:
			num2 += num2 * 0.15f;
			break;
		case 5:
			num2 += num2 * 0.2f;
			break;
		}
		float num5 = 0f;
		for (int j = 0; j < hardware_.hardware_UNLOCK.Length; j++)
		{
			if (!hardware_.hardware_UNLOCK[j])
			{
				continue;
			}
			if (typ == 1)
			{
				if (hardware_.hardware_TYP[j] == 8 && hardware_.hardware_ONLYSTATIONARY[j] && hardware_.hardware_RES_POINTS[j] > hardware_.hardware_RES_POINTS[component_case])
				{
					num5 += 1f;
				}
			}
			else if (hardware_.hardware_TYP[j] == 8 && hardware_.hardware_ONLYHANDHELD[j] && hardware_.hardware_RES_POINTS[j] > hardware_.hardware_RES_POINTS[component_case])
			{
				num5 += 1f;
			}
		}
		num2 -= num2 * (num5 * 0.03f);
		if (gameID != -1)
		{
			if (!vorinstalledGame_)
			{
				FindMyGame();
			}
			if ((bool)vorinstalledGame_)
			{
				num2 += num2 * ((float)vorinstalledGame_.reviewTotal * 0.0005f);
			}
		}
		num2 *= GetPriceAbzug();
		num2 += num2 * (GetHype() * 0.03f);
		switch (mS_.GetStudioLevel(mS_.studioPoints))
		{
		case 0:
			num2 *= 0.1f;
			break;
		case 1:
			num2 *= 0.15f;
			break;
		case 2:
			num2 *= 0.2f;
			break;
		case 3:
			num2 *= 0.25f;
			break;
		case 4:
			num2 *= 0.3f;
			break;
		case 5:
			num2 *= 0.4f;
			break;
		case 6:
			num2 *= 0.5f;
			break;
		case 7:
			num2 *= 0.6f;
			break;
		case 8:
			num2 *= 0.7f;
			break;
		case 9:
			num2 *= 0.8f;
			break;
		}
		if (mS_.month == 12 || mS_.month == 1)
		{
			num2 *= 1.5f;
		}
		if (mS_.month == 6 || mS_.month == 7)
		{
			num2 *= 0.7f;
		}
		if (!mS_.settings_sabotageOff && mS_.sabotage_erwischt > 0)
		{
			num2 *= 0.5f;
		}
		if (!mS_.settings_sabotageOff && mS_.sabotage_geruecht > 0)
		{
			num2 *= 0.8f;
		}
		if (mS_.globalEvent == 0)
		{
			num2 *= 0.5f;
		}
		if (mS_.globalEvent == 1)
		{
			num2 *= 1.5f;
		}
		float num6 = mS_.GetAchivementBonus(6);
		num6 *= 0.01f;
		num2 += num2 * num6;
		num2 += num2 * (GetSellsBonusForGames() * 0.01f);
		switch (mS_.difficulty)
		{
		case 0:
			num2 *= 1.8f;
			break;
		case 1:
			num2 *= 1.3f;
			break;
		case 2:
			num2 *= 1f;
			break;
		case 3:
			num2 *= 0.7f;
			break;
		case 4:
			num2 *= 0.5f;
			break;
		case 5:
			num2 *= 0.3f;
			break;
		}
		num2 *= 1.5f;
		if (mS_.settings_sandbox && mS_.sandbox_konsoleSells > 0f)
		{
			num2 *= mS_.sandbox_konsoleSells;
		}
		if (units >= 1000000000)
		{
			num2 = ((!(num2 > 100f)) ? 0f : ((float)Random.Range(1, 100)));
		}
		else
		{
			for (int k = 0; k < games_.arrayGamesScripts.Length; k++)
			{
				if (!games_.arrayGamesScripts[k] || !games_.arrayGamesScripts[k].isOnMarket || (!games_.arrayGamesScripts[k].exklusiv && !games_.arrayGamesScripts[k].herstellerExklusiv))
				{
					continue;
				}
				for (int l = 0; l < games_.arrayGamesScripts[k].gamePlatform.Length; l++)
				{
					if (games_.arrayGamesScripts[k].gamePlatform[l] != myID)
					{
						continue;
					}
					float num7 = num2 * 0.3f;
					if (games_.arrayGamesScripts[k].herstellerExklusiv)
					{
						num7 = num2 * 0.15f;
					}
					num7 *= GetPriceAbzug();
					float num8 = Mathf.RoundToInt(Random.Range((float)games_.arrayGamesScripts[k].sellsPerWeek[0] * 0.2f, (float)games_.arrayGamesScripts[k].sellsPerWeek[0] * 0.3f));
					num8 = num8 / 100f * (130f - GetMarktanteil());
					if (num8 > (float)sellsPerWeek[0])
					{
						num8 = sellsPerWeek[0];
					}
					if (num7 >= num8)
					{
						if (!(num8 > 0f))
						{
							continue;
						}
						games_.arrayGamesScripts[k].exklusivKonsolenSells += Mathf.RoundToInt(num8);
						num2 += num8;
						if (mS_.multiplayer)
						{
							if (mS_.mpCalls_.isServer)
							{
								mS_.mpCalls_.SERVER_Send_ExklusivKonsolenSells(games_.arrayGamesScripts[k], games_.arrayGamesScripts[k].exklusivKonsolenSells);
							}
							if (mS_.mpCalls_.isClient)
							{
								mS_.mpCalls_.CLIENT_Send_ExklusivKonsolenSells(games_.arrayGamesScripts[k], games_.arrayGamesScripts[k].exklusivKonsolenSells);
							}
						}
					}
					else
					{
						if (!(num7 > 0f))
						{
							continue;
						}
						games_.arrayGamesScripts[k].exklusivKonsolenSells += Mathf.RoundToInt(num7);
						num2 += num7;
						if (mS_.multiplayer)
						{
							if (mS_.mpCalls_.isServer)
							{
								mS_.mpCalls_.SERVER_Send_ExklusivKonsolenSells(games_.arrayGamesScripts[k], games_.arrayGamesScripts[k].exklusivKonsolenSells);
							}
							if (mS_.mpCalls_.isClient)
							{
								mS_.mpCalls_.CLIENT_Send_ExklusivKonsolenSells(games_.arrayGamesScripts[k], games_.arrayGamesScripts[k].exklusivKonsolenSells);
							}
						}
					}
				}
			}
		}
		if (num2 < 0f)
		{
			num2 = 0f;
		}
		sellsPerWeek[0] = Mathf.RoundToInt(num2);
		units += Mathf.RoundToInt(num2);
		garantieMonth[60 - garantie] += Mathf.RoundToInt(num2);
		garantiefaelle = 0;
		float num9 = 0f;
		for (int m = 0; m < garantieMonth.Length; m++)
		{
			num9 += (float)(garantieMonth[m] / 5000);
		}
		num9 = Random.Range(num9 * 0.8f, num9 * 1f);
		num9 /= GetHaltbarkeit() * 0.5f;
		switch (mS_.difficulty)
		{
		case 0:
			num9 *= 0.5f;
			break;
		case 1:
			num9 *= 0.7f;
			break;
		case 2:
			num9 *= 1f;
			break;
		case 3:
			num9 *= 1.5f;
			break;
		case 4:
			num9 *= 2f;
			break;
		case 5:
			num9 *= 2.5f;
			break;
		}
		garantiefaelle = Mathf.RoundToInt(num9);
		garantiekosten = garantiefaelle * GetAktuellProductionsCosts();
		mS_.Pay(garantiekosten, 32);
		costs_garantie += garantiekosten;
		float f = garantiefaelle;
		mS_.AddAnrufe(Mathf.RoundToInt(f));
		mS_.AddVerkaufsverlaufKonsolen(Mathf.RoundToInt(num2));
		long num10 = Mathf.RoundToInt(num2);
		num10 *= GetAktuellProductionsCosts();
		mS_.Pay(num10, 23);
		costs_production += num10;
		long num11 = Mathf.RoundToInt(num2);
		num11 *= verkaufspreis;
		mS_.Earn(num11, 9);
		umsatzTotal += num11;
		if (isUnlocked)
		{
			AddHype(0f - Random.Range(0.1f, 1f));
		}
		if (!IsOutdatet())
		{
			if (kostenreduktion < 100f)
			{
				kostenreduktion += Random.Range(0.05f, 0.1f);
				if (kostenreduktion > 100f)
				{
					kostenreduktion = 100f;
				}
			}
		}
		else
		{
			kostenreduktion -= Random.Range(0.1f, 0.3f);
			if (kostenreduktion < 0f)
			{
				kostenreduktion = 0f;
			}
		}
		AutoPreis();
		mS_.AddFans(Mathf.RoundToInt(Random.Range(0f, num2 * 0.01f)), -1);
		if ((bool)konsoleTab_)
		{
			konsoleTab_.UpdateData();
		}
		if ((bool)mS_.achScript_)
		{
			if (units >= 10000000)
			{
				mS_.achScript_.SetAchivement(66);
			}
			if (units >= 50000000)
			{
				mS_.achScript_.SetAchivement(67);
			}
		}
	}

	public void ResetGarantieMonth()
	{
		for (int num = garantieMonth.Length - 1; num >= 1; num--)
		{
			garantieMonth[num] = garantieMonth[num - 1];
		}
		garantieMonth[0] = 0;
	}

	public bool IsOutdatet()
	{
		if (IsProKonsole())
		{
			if (weeksOnMarket < 600)
			{
				return false;
			}
		}
		else if (weeksOnMarket < 500)
		{
			return false;
		}
		return true;
	}

	public int GetAmountSubventionierteGames()
	{
		int num = 0;
		if ((bool)games_)
		{
			for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
			{
				if (!games_.arrayGamesScripts[i] || games_.arrayGamesScripts[i].subvention <= 0 || games_.arrayGamesScripts[i].auftragsspiel || games_.arrayGamesScripts[i].typ_budget || games_.arrayGamesScripts[i].typ_addon || games_.arrayGamesScripts[i].typ_bundle || games_.arrayGamesScripts[i].typ_bundleAddon || games_.arrayGamesScripts[i].typ_goty || games_.arrayGamesScripts[i].typ_addonStandalone)
				{
					continue;
				}
				for (int j = 0; j < games_.arrayGamesScripts[i].gamePlatform.Length; j++)
				{
					if (games_.arrayGamesScripts[i].gamePlatform[j] == myID)
					{
						num++;
						break;
					}
				}
			}
		}
		return num;
	}

	private float GetPriceAbzug()
	{
		if (!platforms_)
		{
			return 0.1f;
		}
		return platforms_.GetPriceCurve(verkaufspreis);
	}

	public void AddHype(float f)
	{
		if (hype <= 100f)
		{
			hype += f;
			if (hype > 100f)
			{
				hype = 100f;
			}
		}
		if (hype > 100f && f < 0f)
		{
			hype += f;
		}
		if (hype < 0f)
		{
			hype = 0f;
		}
	}

	public Sprite GetComplexSprite()
	{
		return platforms_.complexSprites[complex];
	}

	public Sprite GetTypSprite()
	{
		return platforms_.typSprites[typ];
	}

	public string GetTypString()
	{
		return typ switch
		{
			0 => tS_.GetText(1479), 
			1 => tS_.GetText(1480), 
			2 => tS_.GetText(1481), 
			3 => tS_.GetText(1482), 
			4 => tS_.GetText(1513), 
			_ => tS_.GetText(1479), 
		};
	}

	public void RemoveFromMarket()
	{
		vomMarktGenommen = true;
		date_year_end = mS_.year;
		date_month_end = mS_.month;
		if ((bool)guiMain_)
		{
			guiMain_.CreateTopNewsPlatformRemove(this);
		}
		if ((bool)konsoleTab_)
		{
			Object.Destroy(konsoleTab_.gameObject);
		}
	}

	public void SetOnMarket()
	{
		FindScripts();
		isUnlocked = true;
		if (!proVersion)
		{
			date_year = mS_.year;
			date_month = mS_.month;
			InitUI();
		}
		if (ownerID == mS_.myID)
		{
			mS_.AddStudioPoints(Mathf.RoundToInt(5f * review));
			if (!proVersion && (bool)mS_.achScript_)
			{
				if (typ == 1)
				{
					mS_.achScript_.SetAchivement(24);
				}
				if (typ == 2)
				{
					mS_.achScript_.SetAchivement(25);
				}
			}
		}
		if ((bool)guiMain_)
		{
			guiMain_.CreateTopNewsPlatform(this);
		}
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Platform(this);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Platform(this);
			}
		}
	}

	public void InitUI()
	{
		if (ownerID == mS_.myID && !vomMarktGenommen && isUnlocked)
		{
			FindScripts();
			GameObject gameObject = Object.Instantiate(guiMain_.uiPrefabs[20], guiMain_.uiObjects[81].transform);
			konsoleTab_ = gameObject.GetComponent<konsoleTab>();
			konsoleTab_.pS_ = this;
			konsoleTab_.mS_ = mS_;
			konsoleTab_.main_ = main_;
			konsoleTab_.guiMain_ = guiMain_;
			konsoleTab_.tS_ = tS_;
			konsoleTab_.Init(myID);
		}
	}

	public void AddHaltbarkeitFloat(float f)
	{
		haltbarkeit += devPointsStart * f;
	}

	public void AddHaltbarkeit(float f)
	{
		haltbarkeit += f * f * 1.8f;
	}

	public float GetHaltbarkeit()
	{
		float num = haltbarkeit / devPointsStart;
		if (num < 0f)
		{
			return 0f;
		}
		if (num > 10f)
		{
			return 10f;
		}
		return num;
	}

	public float GetTechDifference()
	{
		float num = 0f;
		float num2 = 0f;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].myID != myID && mS_.arrayPlatformsScripts[i].isUnlocked && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].typ == typ)
			{
				num += 1f;
				num2 += (float)mS_.arrayPlatformsScripts[i].tech;
			}
		}
		if (num == 0f)
		{
			return 0f;
		}
		float num3 = num2 / num;
		return (float)tech - num3;
	}

	public int GetAmountOfOwnKonsolesSameTyp()
	{
		int num = 0;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].ownerID == mS_.myID && mS_.arrayPlatformsScripts[i].isUnlocked && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].typ == typ && mS_.arrayPlatformsScripts[i].tech >= tech)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAmountOfOwnKonsolesSameTypRemoved()
	{
		int num = 0;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].ownerID == mS_.myID && mS_.arrayPlatformsScripts[i].isUnlocked && mS_.arrayPlatformsScripts[i].typ == typ && mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].tech > tech)
			{
				num++;
			}
		}
		return num;
	}

	public int GetAmountOfKonsolesSameTyp()
	{
		int num = 0;
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPlatformsScripts.Length; i++)
		{
			if ((bool)mS_.arrayPlatformsScripts[i] && mS_.arrayPlatformsScripts[i].isUnlocked && !mS_.arrayPlatformsScripts[i].vomMarktGenommen && mS_.arrayPlatformsScripts[i].typ == typ)
			{
				num++;
			}
		}
		return num;
	}

	public void FindMyGame()
	{
		if (gameID == -1 || (bool)vorinstalledGame_)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if ((bool)vorinstalledGame_)
		{
			return;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].myID == gameID)
			{
				vorinstalledGame_ = games_.arrayGamesScripts[i];
				break;
			}
		}
		if (!vorinstalledGame_)
		{
			gameID = -1;
		}
	}

	public void SellPlayerKonsoleToNPC(publisherScript pS_)
	{
		if (!pS_)
		{
			return;
		}
		if (!mS_)
		{
			FindScripts();
		}
		if (pS_.isPlayer)
		{
			return;
		}
		if (publisherBuyed == null || publisherBuyed.Length == 0)
		{
			publisherBuyed = new bool[mS_.arrayPublisher.Length];
		}
		if (publisherBuyed[pS_.myID])
		{
			return;
		}
		publisherBuyed[pS_.myID] = true;
		if (ownerID == mS_.myID)
		{
			mS_.Earn(price, 10);
			string text = tS_.GetText(1659);
			text = text.Replace("<NAME1>", pS_.GetName());
			text = text.Replace("<NAME2>", GetName());
			text = text + "\n<color=green><b>+" + mS_.GetMoney(price, showDollar: true) + "</b></color>";
			guiMain_.CreateTopNewsInfo(text);
		}
		else if (mS_.multiplayer && PlatformFromMitspieler())
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, ownerID, 4, price, myID);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Payment(ownerID, 4, price, myID);
			}
		}
	}

	public bool GetPlattformEnd()
	{
		int num = 0;
		int num2 = 0;
		if (typ != 4)
		{
			int num3 = Mathf.RoundToInt(GetMarktanteil()) * 2;
			int num4 = num3 / 12;
			num3 -= num4 * 12;
			num = date_year_end + num4;
			num2 = date_month_end + num3;
			if (num2 > 12)
			{
				num2 = 1;
				num++;
			}
		}
		else
		{
			num = date_year_end;
			num2 = date_month_end;
		}
		if (mS_.year >= num)
		{
			if (mS_.month >= num2)
			{
				return true;
			}
			if (mS_.year > num)
			{
				return true;
			}
		}
		return false;
	}

	public float GetExklusivBonus()
	{
		float result = 0f;
		int num = Mathf.RoundToInt(GetMarktanteil());
		if (num <= 3)
		{
			result = 75f;
		}
		if (num > 3 && num <= 5)
		{
			result = 50f;
		}
		if (num > 5 && num <= 10)
		{
			result = 45f;
		}
		if (num > 10 && num <= 15)
		{
			result = 40f;
		}
		if (num > 15 && num <= 20)
		{
			result = 35f;
		}
		if (num > 20 && num <= 25)
		{
			result = 30f;
		}
		if (num > 25 && num <= 30)
		{
			result = 25f;
		}
		if (num > 30 && num <= 35)
		{
			result = 20f;
		}
		if (num > 35 && num <= 40)
		{
			result = 15f;
		}
		if (num > 40 && num <= 45)
		{
			result = 10f;
		}
		if (num > 45 && num <= 50)
		{
			result = 5f;
		}
		if (num > 50)
		{
			result = 0f;
		}
		if (!OwnerIsNPC())
		{
			result = 0f;
		}
		return result;
	}

	public bool WurdeHerstellerAufgekauft()
	{
		if (ownerID == -1)
		{
			return false;
		}
		if (!mS_)
		{
			FindScripts();
		}
		for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
		{
			if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == ownerID && mS_.arrayPublisherScripts[i].IsTochterfirma())
			{
				Debug.Log("Wurde aufgekauft: " + mS_.arrayPublisherScripts[i].name_EN + " / " + GetName());
				return true;
			}
		}
		return false;
	}

	public bool OwnerIsNPC()
	{
		if (ownerID < 100000)
		{
			return true;
		}
		return false;
	}

	public bool PlatformFromMitspieler()
	{
		if (!mS_.multiplayer)
		{
			return false;
		}
		if (ownerID < 100000)
		{
			return false;
		}
		if (ownerID == mS_.myID)
		{
			return false;
		}
		if (ownerID >= 100000)
		{
			return true;
		}
		return false;
	}

	public int GetAmountGamePassGames()
	{
		int num = 0;
		if (!games_)
		{
			FindScripts();
		}
		if (!internet)
		{
			return 0;
		}
		if (!IsVerfuegbar())
		{
			return 0;
		}
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].inGamePass && (games_.arrayGamesScripts[i].gamePlatform[0] == myID || games_.arrayGamesScripts[i].gamePlatform[1] == myID || games_.arrayGamesScripts[i].gamePlatform[2] == myID || games_.arrayGamesScripts[i].gamePlatform[3] == myID || GameIsCompatible(games_.arrayGamesScripts[i])))
			{
				num++;
			}
		}
		return num;
	}

	public bool GameIsCompatible(gameScript script_)
	{
		for (int i = 0; i < platformCompatible.Length; i++)
		{
			if (platformCompatible[i] > 0 && (script_.gamePlatform[0] == platformCompatible[i] || script_.gamePlatform[1] == platformCompatible[i] || script_.gamePlatform[2] == platformCompatible[i] || script_.gamePlatform[3] == platformCompatible[i]))
			{
				return true;
			}
		}
		return false;
	}

	public bool CanBeInGamePass(bool ignoreGames)
	{
		if (inBesitz && internet && IsVerfuegbar() && (typ == 0 || typ == 1 || typ == 2) && (GetAmountGamePassGames() >= minGamePassGames || ignoreGames))
		{
			return true;
		}
		return false;
	}

	public bool IsAngekuendigt()
	{
		if (!mS_)
		{
			return false;
		}
		if (!OwnerIsNPC())
		{
			return false;
		}
		if (!isUnlocked && angekuendigt)
		{
			if (ownerID == -1)
			{
				return true;
			}
			for (int i = 0; i < mS_.arrayPublisherScripts.Length; i++)
			{
				if ((bool)mS_.arrayPublisherScripts[i] && mS_.arrayPublisherScripts[i].myID == ownerID && !mS_.arrayPublisherScripts[i].IsTochterfirma())
				{
					return true;
				}
			}
		}
		return false;
	}

	public void ZaehleGames()
	{
		games = 0;
		exklusivGames = 0;
		games_gamePass = 0;
		gamesInDev = 0;
		games_S0 = 0;
		exklusivGames_S0 = 0;
		games_S1 = 0;
		exklusivGames_S1 = 0;
		games_S2 = 0;
		exklusivGames_S2 = 0;
		games_S3 = 0;
		exklusivGames_S3 = 0;
		games_S4 = 0;
		exklusivGames_S4 = 0;
		games_S5 = 0;
		exklusivGames_S5 = 0;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i])
			{
				continue;
			}
			if (games_.arrayGamesScripts[i].inDevelopment && !games_.arrayGamesScripts[i].typ_budget && !games_.arrayGamesScripts[i].typ_addon && !games_.arrayGamesScripts[i].typ_bundle && !games_.arrayGamesScripts[i].typ_bundleAddon && !games_.arrayGamesScripts[i].typ_goty && !games_.arrayGamesScripts[i].typ_addonStandalone)
			{
				for (int j = 0; j < games_.arrayGamesScripts[i].gamePlatform.Length; j++)
				{
					if (games_.arrayGamesScripts[i].gamePlatform[j] == myID)
					{
						gamesInDev++;
					}
				}
			}
			if (games_.arrayGamesScripts[i].inDevelopment || games_.arrayGamesScripts[i].schublade || games_.arrayGamesScripts[i].auftragsspiel || games_.arrayGamesScripts[i].typ_budget || games_.arrayGamesScripts[i].typ_addon || games_.arrayGamesScripts[i].typ_bundle || games_.arrayGamesScripts[i].typ_bundleAddon || games_.arrayGamesScripts[i].typ_goty || games_.arrayGamesScripts[i].typ_addonStandalone)
			{
				continue;
			}
			for (int k = 0; k < games_.arrayGamesScripts[i].gamePlatform.Length; k++)
			{
				if (games_.arrayGamesScripts[i].gamePlatform[k] != myID)
				{
					continue;
				}
				games++;
				if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
				{
					exklusivGames++;
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 1 && games_.arrayGamesScripts[i].reviewTotal <= 29)
				{
					games_S0++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S0++;
					}
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 30 && games_.arrayGamesScripts[i].reviewTotal <= 49)
				{
					games_S1++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S1++;
					}
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 50 && games_.arrayGamesScripts[i].reviewTotal <= 69)
				{
					games_S2++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S2++;
					}
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 70 && games_.arrayGamesScripts[i].reviewTotal <= 79)
				{
					games_S3++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S3++;
					}
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 80 && games_.arrayGamesScripts[i].reviewTotal <= 89)
				{
					games_S4++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S4++;
					}
				}
				if (games_.arrayGamesScripts[i].reviewTotal >= 90)
				{
					games_S5++;
					if (games_.arrayGamesScripts[i].exklusiv || games_.arrayGamesScripts[i].herstellerExklusiv)
					{
						exklusivGames_S5++;
					}
				}
			}
		}
		if (inGamePass && gpS_.gamePass_aktiv)
		{
			games_gamePass = GetAmountGamePassGames();
		}
	}

	public float GetSellsBonusForGames()
	{
		if (GetGames() <= 0)
		{
			return -50f;
		}
		float num = 0f;
		num += (float)(games_S0 - exklusivGames_S0) * 0.1f;
		num += (float)exklusivGames_S0 * 0.2f;
		num += (float)(games_S1 - exklusivGames_S1) * 0.2f;
		num += (float)exklusivGames_S1 * 0.3f;
		num += (float)(games_S2 - exklusivGames_S2) * 0.3f;
		num += (float)exklusivGames_S2 * 0.5f;
		num += (float)(games_S3 - exklusivGames_S3) * 0.5f;
		num += (float)exklusivGames_S3 * 1f;
		num += (float)(games_S4 - exklusivGames_S4) * 1f;
		num += (float)exklusivGames_S4 * 2f;
		num += (float)(games_S5 - exklusivGames_S5) * 2f;
		num += (float)exklusivGames_S5 * 4f;
		if (inGamePass && gpS_.gamePass_aktiv)
		{
			num += (float)games_gamePass * 0.1f;
		}
		if (num > 300f)
		{
			num = 300f;
		}
		return num;
	}

	public string GetBonusTooltip()
	{
		if (!tS_)
		{
			FindScripts();
		}
		string text = "<b>" + tS_.GetText(2343) + "</b>\n\n";
		string text2 = "";
		text += "<size=12>";
		text = text + tS_.GetText(2351) + "\n";
		text += "<color=red><b>-50%</b></color>\n\n";
		text2 = tS_.GetText(2354) + " [1-29%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.1%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S0 - exklusivGames_S0) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.2%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S0 + "]</color>\n";
		text += "\n";
		text2 = tS_.GetText(2348) + " [30-49%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.2%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S1 - exklusivGames_S1) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.3%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S1 + "]</color>\n";
		text += "\n";
		text2 = tS_.GetText(2349) + " [50-69%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.3%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S2 - exklusivGames_S2) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.5%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S2 + "]</color>\n";
		text += "\n";
		text2 = tS_.GetText(2350) + " [70-79%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+0.5%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S3 - exklusivGames_S3) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+1.0%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S3 + "]</color>\n";
		text += "\n";
		text2 = tS_.GetText(2355) + " [80-89%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+1.0%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S4 - exklusivGames_S4) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+2.0%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S4 + "]</color>\n";
		text += "\n";
		text2 = tS_.GetText(2356) + " [90-100%]";
		text = text + text2 + "\n";
		text2 = tS_.GetText(2345);
		text2 = text2.Replace("<NUM>", "<color=green><b>+2.0%</b></color>");
		text = text + text2 + " <color=magenta>[x" + (games_S5 - exklusivGames_S5) + "]</color>\n";
		text2 = tS_.GetText(2346);
		text2 = text2.Replace("<NUM>", "<color=green><b>+4.0%</b></color>");
		text = text + text2 + " <color=magenta>[x" + exklusivGames_S5 + "]</color>\n";
		text += "\n";
		if (inGamePass && gpS_.gamePass_aktiv)
		{
			text2 = "<color=blue>" + tS_.GetText(1243) + "</color>";
			text = text + text2 + "\n";
			text2 = tS_.GetText(2357);
			text2 = text2.Replace("<NUM>", "<color=green><b>+0.1%</b></color>");
			text = text + text2 + " <color=magenta>[x" + games_gamePass + "]</color>\n";
			text += "\n";
		}
		text += "</size>";
		text2 = tS_.GetText(2353);
		text2 = text2.Replace("<NUM>", "<color=blue><b>300%</b></color>");
		text += text2;
		if (GetSellsBonusForGames() > 0f)
		{
			text2 = tS_.GetText(2347);
			text2 = text2.Replace("<NUM>", "<color=blue><b>+" + GetSellsBonusForGames() + "%</b></color>");
			return text + "\n" + text2 + "\n";
		}
		text2 = tS_.GetText(2347);
		text2 = text2.Replace("<NUM>", "<color=red><b>" + GetSellsBonusForGames() + "%</b></color>");
		return text + "\n" + text2 + "\n";
	}
}
