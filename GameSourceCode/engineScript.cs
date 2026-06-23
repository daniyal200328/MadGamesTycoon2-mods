using UnityEngine;
using UnityEngine.UI;

public class engineScript : MonoBehaviour
{
	public mainScript mS_;

	public GameObject main_;

	public GUI_Main guiMain_;

	public textScript tS_;

	public settingsScript settings_;

	public engineFeatures eF_;

	public genres genres_;

	public games games_;

	public platformScript specialPlatformS_;

	public mpCalls mpCalls_;

	public int myID;

	public int ownerID;

	public bool isUnlocked;

	public bool gekauft;

	public string myName;

	public int umsatz;

	public float marktdominanz;

	public string name_EN;

	public string name_GE;

	public string name_TU;

	public string name_CH;

	public string name_FR;

	public string name_HU;

	public string name_CT;

	public string name_CZ;

	public string name_PB;

	public string name_IT;

	public string name_JA;

	public string name_PL;

	public string name_UA;

	public string name_TH;

	public string name_RU;

	public bool[] features;

	public bool[] featuresInDev;

	public int spezialgenre;

	public int spezialplatform;

	public int spezialplatformUpdate;

	public bool sellEngine;

	public int preis;

	public int gewinnbeteiligung;

	public bool updating;

	public float devPoints;

	public float devPointsStart;

	public int date_year;

	public int date_month;

	public bool[] publisherBuyed;

	public bool archiv_engine;

	public int amountSellToPlayer;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindWithTag("Main");
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
		if (!tS_)
		{
			tS_ = main_.GetComponent<textScript>();
		}
		if (!eF_)
		{
			eF_ = main_.GetComponent<engineFeatures>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!mpCalls_)
		{
			mpCalls_ = GameObject.Find("NetworkManager").GetComponent<mpCalls>();
		}
	}

	public void Init()
	{
		FindScripts();
		base.name = "ENGINE_" + myID;
	}

	public void InitNpcEngine()
	{
		FindScripts();
		features = new bool[eF_.engineFeatures_UNLOCK.Length];
		featuresInDev = new bool[eF_.engineFeatures_UNLOCK.Length];
		for (int i = 0; i < eF_.engineFeatures_UNLOCK.Length; i++)
		{
			if (eF_.engineFeatures_DATE_YEAR[i] == 1976 && eF_.engineFeatures_DATE_MONTH[i] == 1)
			{
				features[i] = true;
			}
		}
		for (int j = 0; j < eF_.engineFeatures_UNLOCK.Length; j++)
		{
			if (eF_.engineFeatures_UNLOCK[j])
			{
				features[j] = true;
			}
		}
	}

	public bool Complete()
	{
		if (devPoints <= 0f)
		{
			return true;
		}
		return false;
	}

	public int GetGamesAmount()
	{
		int num = 0;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].engineID == myID && !games_.arrayGamesScripts[i].inDevelopment)
			{
				num++;
			}
		}
		return num;
	}

	public int GetGamesAmountOnMarket()
	{
		int num = 0;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if ((bool)games_.arrayGamesScripts[i] && games_.arrayGamesScripts[i].engineID == myID && games_.arrayGamesScripts[i].isOnMarket && !games_.arrayGamesScripts[i].IsRemovedFromMarket())
			{
				num++;
			}
		}
		return num;
	}

	public void CopyFeatures()
	{
		features = new bool[eF_.engineFeatures_PRICE.Length];
		for (int i = 0; i < features.Length; i++)
		{
			features[i] = featuresInDev[i];
		}
	}

	public int GetTechLevel()
	{
		int num = 0;
		for (int i = 0; i < features.Length; i++)
		{
			if (features[i] && eF_.engineFeatures_TECH[i] > num)
			{
				num = eF_.engineFeatures_TECH[i];
			}
		}
		return num;
	}

	public int GetFeaturesAmount()
	{
		int num = 0;
		for (int i = 0; i < features.Length; i++)
		{
			if (features[i])
			{
				num++;
			}
		}
		return num;
	}

	public int GetBestFeature(int art)
	{
		int num = -1;
		int result = 0;
		for (int i = 0; i < features.Length; i++)
		{
			if (features[i] && eF_.engineFeatures_TYP[i] == art && eF_.engineFeatures_RES_POINTS[i] > num)
			{
				num = eF_.engineFeatures_RES_POINTS[i];
				result = i;
			}
		}
		return result;
	}

	private void FindSpecialPlatformScript()
	{
		if (spezialplatform != -1 && !specialPlatformS_)
		{
			GameObject gameObject = GameObject.Find("PLATFORM_" + spezialplatform);
			if ((bool)gameObject)
			{
				specialPlatformS_ = gameObject.GetComponent<platformScript>();
			}
			else
			{
				spezialplatform = -1;
			}
		}
	}

	public string GetReleaseDateString()
	{
		return tS_.GetText(date_month + 220) + " " + date_year;
	}

	public string GetTooltip()
	{
		FindSpecialPlatformScript();
		string text = "<b>" + GetName() + "</b>";
		text = text + "\n<color=magenta>" + tS_.GetText(4) + " " + GetTechLevel() + "</color>\n";
		if (date_year > 0)
		{
			text = text + "<color=blue>" + GetReleaseDateString() + "</color>\n";
		}
		if (mS_.multiplayer && ownerID != -1)
		{
			text = text + "<color=blue>" + mpCalls_.GetCompanyName(ownerID) + "</color>\n";
		}
		text = ((!specialPlatformS_) ? (text + "\n" + tS_.GetText(1218) + ": <color=blue>" + tS_.GetText(1221) + "</color>") : (text + "\n" + tS_.GetText(1218) + ": <color=blue>" + specialPlatformS_.GetName() + "</color>"));
		text = text + "\n" + tS_.GetText(245) + ": <color=blue>" + genres_.GetName(spezialgenre) + "</color>";
		text = text + "\n" + tS_.GetText(160) + ": <color=blue>" + GetFeaturesAmount() + "</color>";
		text = text + "\n" + tS_.GetText(261) + ": <color=blue>" + GetGamesAmount() + "</color>\n";
		if (sellEngine)
		{
			text = text + "\n" + tS_.GetText(88) + ": <color=blue>" + mS_.GetMoney(preis, showDollar: true) + "</color>";
			text = text + "\n" + tS_.GetText(260) + ": <color=blue>" + gewinnbeteiligung + "%</color>";
		}
		if (ownerID == mS_.myID)
		{
			text = text + "\n" + tS_.GetText(268) + ": <color=blue>" + GetVerkaufteLizenzen() + "</color>";
			text = text + "\n" + tS_.GetText(276) + ": <color=blue>" + mS_.GetMoney(umsatz, showDollar: true) + "</color>";
			text = text + "\n\n<color=blue><b>" + tS_.GetText(262) + "</b></color>";
		}
		return text;
	}

	public string GetVersionString()
	{
		return GetTechLevel() + "." + GetFeaturesAmount();
	}

	public string GetName()
	{
		if (mS_.multiplayer && EngineFromMitspieler())
		{
			return myName;
		}
		if (OwnerIsNPC())
		{
			string text = "";
			text = settings_.language switch
			{
				0 => name_EN, 
				1 => name_GE, 
				2 => name_TU, 
				3 => name_CH, 
				4 => name_FR, 
				7 => name_PB, 
				8 => name_HU, 
				9 => name_RU, 
				10 => name_CT, 
				11 => name_PL, 
				12 => name_CZ, 
				14 => name_IT, 
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
		return myName;
	}

	public platformScript GetSpezialPlatformScript()
	{
		FindSpecialPlatformScript();
		return specialPlatformS_;
	}

	public void SetSpezialPlatformSprite(GameObject go)
	{
		FindSpecialPlatformScript();
		if ((bool)specialPlatformS_)
		{
			specialPlatformS_.SetPic(go);
			return;
		}
		go.GetComponent<Image>().material = null;
		go.GetComponent<Image>().sprite = guiMain_.uiSprites[3];
		go.GetComponent<Image>().color = Color.white;
	}

	public float GetProzent()
	{
		return 100f / devPointsStart * (devPointsStart - devPoints);
	}

	public void SetComplete()
	{
		date_month = mS_.month;
		date_year = mS_.year;
		specialPlatformS_ = null;
		spezialplatform = spezialplatformUpdate;
		for (int i = 0; i < featuresInDev.Length; i++)
		{
			if (featuresInDev[i])
			{
				features[i] = true;
			}
		}
		EntwicklungBeenden();
		if (mS_.multiplayer)
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Engine(this);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Engine(this);
			}
		}
	}

	public void EntwicklungBeenden()
	{
		devPoints = 0f;
		devPointsStart = 0f;
		updating = false;
		for (int i = 0; i < featuresInDev.Length; i++)
		{
			featuresInDev[i] = false;
		}
	}

	public void SellPlayerEngine(publisherScript pS_)
	{
		if (!pS_ || pS_.isPlayer)
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
		bool flag = false;
		if (pS_.IsTochterfirma() && pS_.tf_engine == myID)
		{
			flag = true;
		}
		if (flag)
		{
			return;
		}
		if (mS_.myID == ownerID)
		{
			mS_.Earn(preis, 4);
			umsatz += preis;
			string text = tS_.GetText(500);
			text = text.Replace("<NAME1>", pS_.GetName());
			text = text.Replace("<NAME2>", GetName());
			text = text + "\n<color=green><b>+" + mS_.GetMoney(preis, showDollar: true) + "</b></color>";
			guiMain_.CreateTopNewsInfo(text);
		}
		else if (mS_.multiplayer && EngineFromMitspieler())
		{
			if (mS_.mpCalls_.isServer)
			{
				mS_.mpCalls_.SERVER_Send_Payment(mS_.myID, ownerID, 3, preis, myID);
				mS_.mpCalls_.SERVER_Send_EnginePublisherBuyed(this);
			}
			if (mS_.mpCalls_.isClient)
			{
				mS_.mpCalls_.CLIENT_Send_Payment(ownerID, 3, preis, myID);
				mS_.mpCalls_.CLIENT_Send_EnginePublisherBuyed(this);
			}
		}
	}

	public int GetVerkaufteLizenzen()
	{
		int num = 0;
		if (publisherBuyed.Length != 0)
		{
			for (int i = 0; i < publisherBuyed.Length; i++)
			{
				if (publisherBuyed[i])
				{
					num++;
				}
			}
		}
		return num + amountSellToPlayer;
	}

	public bool OwnerIsNPC()
	{
		if (ownerID < 100000)
		{
			return true;
		}
		return false;
	}

	public bool EngineFromMitspieler()
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

	public float GetMarktdominanz()
	{
		return marktdominanz;
	}

	public string GetMarktdominanzString()
	{
		if (!tS_)
		{
			FindScripts();
		}
		if ((bool)tS_)
		{
			if (marktdominanz < 33f)
			{
				return tS_.GetText(1908);
			}
			if (marktdominanz >= 33f && marktdominanz < 66f)
			{
				return tS_.GetText(1909);
			}
			if (marktdominanz >= 66f)
			{
				return tS_.GetText(1910);
			}
		}
		return "";
	}

	public void AddMarktdominanz(float f)
	{
		marktdominanz += f;
		if (marktdominanz < 0f)
		{
			marktdominanz = 0f;
		}
		if (marktdominanz > 100f)
		{
			marktdominanz = 100f;
		}
	}

	public int GetPreis()
	{
		return preis;
	}

	public int GetGewinnbeteiligung()
	{
		return gewinnbeteiligung;
	}
}
