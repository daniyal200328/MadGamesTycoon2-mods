using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class textScript : MonoBehaviour
{
	private GameObject main_;

	private mainScript mS_;

	private settingsScript settings_;

	private themes themes_;

	private genres genres_;

	public string[] namesFemale;

	public string[] namesMale;

	public string[] surname;

	public string[] devLegends;

	public string[] randomEngineNames;

	public string[] randomGameNames;

	public string[] randomPlatformNames;

	public string[] randomCompanyNames;

	public string credits;

	public string[] npcGames;

	public bool[] npcGameNameInUse;

	public string[] npcAddons;

	public string[] npcBundles;

	public string[] npcAddonBundles;

	public string[] npcSpinOffs;

	public string[] npcIPs;

	public bool[] npcIPsInUse;

	public string[] text_EN;

	public string[] text_GE;

	public string[] text_TU;

	public string[] text_CH;

	public string[] text_FR;

	public string[] text_ES;

	public string[] text_KO;

	public string[] text_PB;

	public string[] text_HU;

	public string[] text_RU;

	public string[] text_CT;

	public string[] text_PL;

	public string[] text_CZ;

	public string[] text_AR;

	public string[] text_IT;

	public string[] text_RO;

	public string[] text_JA;

	public string[] text_UA;

	public string[] text_LA;

	public string[] text_TH;

	public string[] achivementsName_EN;

	public string[] achivementsName_GE;

	public string[] achivementsName_TU;

	public string[] achivementsName_CH;

	public string[] achivementsName_FR;

	public string[] achivementsName_ES;

	public string[] achivementsName_KO;

	public string[] achivementsName_PB;

	public string[] achivementsName_HU;

	public string[] achivementsName_RU;

	public string[] achivementsName_CT;

	public string[] achivementsName_PL;

	public string[] achivementsName_CZ;

	public string[] achivementsName_AR;

	public string[] achivementsName_IT;

	public string[] achivementsName_RO;

	public string[] achivementsName_JA;

	public string[] achivementsName_UA;

	public string[] achivementsName_LA;

	public string[] achivementsName_TH;

	public string[] achivementsDesc_EN;

	public string[] achivementsDesc_GE;

	public string[] achivementsDesc_TU;

	public string[] achivementsDesc_CH;

	public string[] achivementsDesc_FR;

	public string[] achivementsDesc_ES;

	public string[] achivementsDesc_KO;

	public string[] achivementsDesc_PB;

	public string[] achivementsDesc_HU;

	public string[] achivementsDesc_RU;

	public string[] achivementsDesc_CT;

	public string[] achivementsDesc_PL;

	public string[] achivementsDesc_CZ;

	public string[] achivementsDesc_AR;

	public string[] achivementsDesc_IT;

	public string[] achivementsDesc_RO;

	public string[] achivementsDesc_JA;

	public string[] achivementsDesc_UA;

	public string[] achivementsDesc_LA;

	public string[] achivementsDesc_TH;

	public string[] objects_EN;

	public string[] objects_GE;

	public string[] objects_TU;

	public string[] objects_CH;

	public string[] objects_FR;

	public string[] objects_ES;

	public string[] objects_KO;

	public string[] objects_PB;

	public string[] objects_HU;

	public string[] objects_RU;

	public string[] objects_CT;

	public string[] objects_PL;

	public string[] objects_CZ;

	public string[] objects_AR;

	public string[] objects_IT;

	public string[] objects_RO;

	public string[] objects_JA;

	public string[] objects_UA;

	public string[] objects_LA;

	public string[] objects_TH;

	public string[] objectsTooltip_EN;

	public string[] objectsTooltip_GE;

	public string[] objectsTooltip_TU;

	public string[] objectsTooltip_CH;

	public string[] objectsTooltip_FR;

	public string[] objectsTooltip_ES;

	public string[] objectsTooltip_KO;

	public string[] objectsTooltip_PB;

	public string[] objectsTooltip_HU;

	public string[] objectsTooltip_RU;

	public string[] objectsTooltip_CT;

	public string[] objectsTooltip_PL;

	public string[] objectsTooltip_CZ;

	public string[] objectsTooltip_AR;

	public string[] objectsTooltip_IT;

	public string[] objectsTooltip_RO;

	public string[] objectsTooltip_JA;

	public string[] objectsTooltip_UA;

	public string[] objectsTooltip_LA;

	public string[] objectsTooltip_TH;

	public string[] country_EN;

	public string[] country_GE;

	public string[] country_TU;

	public string[] country_CH;

	public string[] country_FR;

	public string[] country_ES;

	public string[] country_KO;

	public string[] country_PB;

	public string[] country_HU;

	public string[] country_RU;

	public string[] country_CT;

	public string[] country_PL;

	public string[] country_CZ;

	public string[] country_AR;

	public string[] country_IT;

	public string[] country_RO;

	public string[] country_JA;

	public string[] country_UA;

	public string[] country_LA;

	public string[] country_TH;

	public string[] quotes_EN;

	public string[] quotes_GE;

	public string[] quotes_TU;

	public string[] quotes_CH;

	public string[] quotes_FR;

	public string[] quotes_ES;

	public string[] quotes_KO;

	public string[] quotes_PB;

	public string[] quotes_HU;

	public string[] quotes_RU;

	public string[] quotes_CT;

	public string[] quotes_PL;

	public string[] quotes_CZ;

	public string[] quotes_AR;

	public string[] quotes_IT;

	public string[] quotes_RO;

	public string[] quotes_JA;

	public string[] quotes_UA;

	public string[] quotes_LA;

	public string[] quotes_TH;

	public string[] themes_EN;

	public string[] themes_GE;

	public string[] themes_TU;

	public string[] themes_CH;

	public string[] themes_FR;

	public string[] themes_ES;

	public string[] themes_KO;

	public string[] themes_PB;

	public string[] themes_HU;

	public string[] themes_RU;

	public string[] themes_CT;

	public string[] themes_PL;

	public string[] themes_CZ;

	public string[] themes_AR;

	public string[] themes_IT;

	public string[] themes_RO;

	public string[] themes_JA;

	public string[] themes_UA;

	public string[] themes_LA;

	public string[] themes_TH;

	public string[] contractWork_EN;

	public string[] contractWork_GE;

	public string[] contractWork_TU;

	public string[] contractWork_CH;

	public string[] contractWork_FR;

	public string[] contractWork_ES;

	public string[] contractWork_KO;

	public string[] contractWork_PB;

	public string[] contractWork_HU;

	public string[] contractWork_RU;

	public string[] contractWork_CT;

	public string[] contractWork_PL;

	public string[] contractWork_CZ;

	public string[] contractWork_AR;

	public string[] contractWork_IT;

	public string[] contractWork_RO;

	public string[] contractWork_JA;

	public string[] contractWork_UA;

	public string[] contractWork_LA;

	public string[] contractWork_TH;

	public string[] fanLetter_EN;

	public string[] fanLetter_GE;

	public string[] fanLetter_TU;

	public string[] fanLetter_CH;

	public string[] fanLetter_FR;

	public string[] fanLetter_ES;

	public string[] fanLetter_KO;

	public string[] fanLetter_PB;

	public string[] fanLetter_HU;

	public string[] fanLetter_RU;

	public string[] fanLetter_CT;

	public string[] fanLetter_PL;

	public string[] fanLetter_CZ;

	public string[] fanLetter_AR;

	public string[] fanLetter_IT;

	public string[] fanLetter_RO;

	public string[] fanLetter_JA;

	public string[] fanLetter_UA;

	public string[] fanLetter_LA;

	public string[] fanLetter_TH;

	public string[] tutorial_EN;

	public string[] tutorial_GE;

	public string[] tutorial_TU;

	public string[] tutorial_CH;

	public string[] tutorial_FR;

	public string[] tutorial_ES;

	public string[] tutorial_KO;

	public string[] tutorial_PB;

	public string[] tutorial_HU;

	public string[] tutorial_RU;

	public string[] tutorial_CT;

	public string[] tutorial_PL;

	public string[] tutorial_CZ;

	public string[] tutorial_AR;

	public string[] tutorial_IT;

	public string[] tutorial_RO;

	public string[] tutorial_JA;

	public string[] tutorial_UA;

	public string[] tutorial_LA;

	public string[] tutorial_TH;

	private bool textLoaded;

	public int model_body;

	public int model_eyes;

	public int model_hair;

	public int model_beard;

	public int model_skinColor;

	public int model_hairColor;

	public int model_beardColor;

	public int model_HoseColor;

	public int model_ShirtColor;

	public int model_Add1Color;

	private void Awake()
	{
		FindScripts();
		if (!textLoaded)
		{
			textLoaded = true;
			LoadGlobalText();
			LoadTexts_EN("EN/Text_EN.txt");
			LoadTexts_GE("GE/Text_GE.txt");
			LoadTexts_TU("TU/Text_TU.txt");
			LoadTexts_CH("CH/Text_CH.txt");
			LoadTexts_FR("FR/Text_FR.txt");
			LoadTexts_ES("ES/Text_ES.txt");
			LoadTexts_KO("KO/Text_KO.txt");
			LoadTexts_PB("PB/Text_PB.txt");
			LoadTexts_HU("HU/Text_HU.txt");
			LoadTexts_RU("RU/Text_RU.txt");
			LoadTexts_CT("CT/Text_CT.txt");
			LoadTexts_PL("PL/Text_PL.txt");
			LoadTexts_CZ("CZ/Text_CZ.txt");
			LoadTexts_AR("AR/Text_AR.txt");
			LoadTexts_IT("IT/Text_IT.txt");
			LoadTexts_RO("RO/Text_RO.txt");
			LoadTexts_JA("JA/Text_JA.txt");
			LoadTexts_UA("UA/Text_UA.txt");
			LoadTexts_LA("LA/Text_LA.txt");
			LoadTexts_TH("TH/Text_TH.txt");
			LoadAchivements_EN("EN/Achivements_EN.txt");
			LoadAchivements_GE("GE/Achivements_GE.txt");
			LoadAchivements_TU("TU/Achivements_TU.txt");
			LoadAchivements_CH("CH/Achivements_CH.txt");
			LoadAchivements_FR("FR/Achivements_FR.txt");
			LoadAchivements_ES("ES/Achivements_ES.txt");
			LoadAchivements_KO("KO/Achivements_KO.txt");
			LoadAchivements_PB("PB/Achivements_PB.txt");
			LoadAchivements_HU("HU/Achivements_HU.txt");
			LoadAchivements_RU("RU/Achivements_RU.txt");
			LoadAchivements_CT("CT/Achivements_CT.txt");
			LoadAchivements_PL("PL/Achivements_PL.txt");
			LoadAchivements_CZ("CZ/Achivements_CZ.txt");
			LoadAchivements_AR("AR/Achivements_AR.txt");
			LoadAchivements_IT("IT/Achivements_IT.txt");
			LoadAchivements_RO("RO/Achivements_RO.txt");
			LoadAchivements_JA("JA/Achivements_JA.txt");
			LoadAchivements_UA("UA/Achivements_UA.txt");
			LoadAchivements_LA("LA/Achivements_LA.txt");
			LoadAchivements_TH("TH/Achivements_TH.txt");
			LoadObjects_EN("EN/Objects_EN.txt");
			LoadObjects_GE("GE/Objects_GE.txt");
			LoadObjects_TU("TU/Objects_TU.txt");
			LoadObjects_CH("CH/Objects_CH.txt");
			LoadObjects_FR("FR/Objects_FR.txt");
			LoadObjects_ES("ES/Objects_ES.txt");
			LoadObjects_KO("KO/Objects_KO.txt");
			LoadObjects_PB("PB/Objects_PB.txt");
			LoadObjects_HU("HU/Objects_HU.txt");
			LoadObjects_RU("RU/Objects_RU.txt");
			LoadObjects_CT("CT/Objects_CT.txt");
			LoadObjects_PL("PL/Objects_PL.txt");
			LoadObjects_CZ("CZ/Objects_CZ.txt");
			LoadObjects_AR("AR/Objects_AR.txt");
			LoadObjects_IT("IT/Objects_IT.txt");
			LoadObjects_RO("RO/Objects_RO.txt");
			LoadObjects_JA("JA/Objects_JA.txt");
			LoadObjects_UA("UA/Objects_UA.txt");
			LoadObjects_LA("LA/Objects_LA.txt");
			LoadObjects_TH("TH/Objects_TH.txt");
			LoadCountry_EN("EN/Country_EN.txt");
			LoadCountry_GE("GE/Country_GE.txt");
			LoadCountry_TU("TU/Country_TU.txt");
			LoadCountry_CH("CH/Country_CH.txt");
			LoadCountry_FR("FR/Country_FR.txt");
			LoadCountry_ES("ES/Country_ES.txt");
			LoadCountry_KO("KO/Country_KO.txt");
			LoadCountry_PB("PB/Country_PB.txt");
			LoadCountry_HU("HU/Country_HU.txt");
			LoadCountry_RU("RU/Country_RU.txt");
			LoadCountry_CT("CT/Country_CT.txt");
			LoadCountry_PL("PL/Country_PL.txt");
			LoadCountry_CZ("CZ/Country_CZ.txt");
			LoadCountry_AR("AR/Country_AR.txt");
			LoadCountry_IT("IT/Country_IT.txt");
			LoadCountry_RO("RO/Country_RO.txt");
			LoadCountry_JA("JA/Country_JA.txt");
			LoadCountry_UA("UA/Country_UA.txt");
			LoadCountry_LA("LA/Country_LA.txt");
			LoadCountry_TH("TH/Country_TH.txt");
			LoadQuotes_EN("EN/Quotes_EN.txt");
			LoadQuotes_GE("GE/Quotes_GE.txt");
			LoadQuotes_TU("TU/Quotes_TU.txt");
			LoadQuotes_CH("CH/Quotes_CH.txt");
			LoadQuotes_FR("FR/Quotes_FR.txt");
			LoadQuotes_ES("ES/Quotes_ES.txt");
			LoadQuotes_KO("KO/Quotes_KO.txt");
			LoadQuotes_PB("PB/Quotes_PB.txt");
			LoadQuotes_HU("HU/Quotes_HU.txt");
			LoadQuotes_RU("RU/Quotes_RU.txt");
			LoadQuotes_CT("CT/Quotes_CT.txt");
			LoadQuotes_PL("PL/Quotes_PL.txt");
			LoadQuotes_CZ("CZ/Quotes_CZ.txt");
			LoadQuotes_AR("AR/Quotes_AR.txt");
			LoadQuotes_IT("IT/Quotes_IT.txt");
			LoadQuotes_RO("RO/Quotes_RO.txt");
			LoadQuotes_JA("JA/Quotes_JA.txt");
			LoadQuotes_UA("UA/Quotes_UA.txt");
			LoadQuotes_LA("LA/Quotes_LA.txt");
			LoadQuotes_TH("TH/Quotes_TH.txt");
			LoadContractWork_EN("EN/ContractWork_EN.txt");
			LoadContractWork_GE("GE/ContractWork_GE.txt");
			LoadContractWork_TU("TU/ContractWork_TU.txt");
			LoadContractWork_CH("CH/ContractWork_CH.txt");
			LoadContractWork_FR("FR/ContractWork_FR.txt");
			LoadContractWork_ES("ES/ContractWork_ES.txt");
			LoadContractWork_KO("KO/ContractWork_KO.txt");
			LoadContractWork_PB("PB/ContractWork_PB.txt");
			LoadContractWork_HU("HU/ContractWork_HU.txt");
			LoadContractWork_RU("RU/ContractWork_RU.txt");
			LoadContractWork_CT("CT/ContractWork_CT.txt");
			LoadContractWork_PL("PL/ContractWork_PL.txt");
			LoadContractWork_CZ("CZ/ContractWork_CZ.txt");
			LoadContractWork_AR("AR/ContractWork_AR.txt");
			LoadContractWork_IT("IT/ContractWork_IT.txt");
			LoadContractWork_RO("RO/ContractWork_RO.txt");
			LoadContractWork_JA("JA/ContractWork_JA.txt");
			LoadContractWork_UA("UA/ContractWork_UA.txt");
			LoadContractWork_LA("LA/ContractWork_LA.txt");
			LoadContractWork_TH("TH/ContractWork_TH.txt");
			LoadFanLetter_EN("EN/FanLetter_EN.txt");
			LoadFanLetter_GE("GE/FanLetter_GE.txt");
			LoadFanLetter_TU("TU/FanLetter_TU.txt");
			LoadFanLetter_CH("CH/FanLetter_CH.txt");
			LoadFanLetter_FR("FR/FanLetter_FR.txt");
			LoadFanLetter_ES("ES/FanLetter_ES.txt");
			LoadFanLetter_KO("KO/FanLetter_KO.txt");
			LoadFanLetter_PB("PB/FanLetter_PB.txt");
			LoadFanLetter_HU("HU/FanLetter_HU.txt");
			LoadFanLetter_RU("RU/FanLetter_RU.txt");
			LoadFanLetter_CT("CT/FanLetter_CT.txt");
			LoadFanLetter_PL("PL/FanLetter_PL.txt");
			LoadFanLetter_CZ("CZ/FanLetter_CZ.txt");
			LoadFanLetter_AR("AR/FanLetter_AR.txt");
			LoadFanLetter_IT("IT/FanLetter_IT.txt");
			LoadFanLetter_RO("RO/FanLetter_RO.txt");
			LoadFanLetter_JA("JA/FanLetter_JA.txt");
			LoadFanLetter_UA("UA/FanLetter_UA.txt");
			LoadFanLetter_LA("LA/FanLetter_LA.txt");
			LoadFanLetter_TH("TH/FanLetter_TH.txt");
			LoadTutorial_EN("EN/Tutorial_EN.txt");
			LoadTutorial_GE("GE/Tutorial_GE.txt");
			LoadTutorial_TU("TU/Tutorial_TU.txt");
			LoadTutorial_CH("CH/Tutorial_CH.txt");
			LoadTutorial_FR("FR/Tutorial_FR.txt");
			LoadTutorial_ES("ES/Tutorial_ES.txt");
			LoadTutorial_KO("KO/Tutorial_KO.txt");
			LoadTutorial_PB("PB/Tutorial_PB.txt");
			LoadTutorial_HU("HU/Tutorial_HU.txt");
			LoadTutorial_RU("RU/Tutorial_RU.txt");
			LoadTutorial_CT("CT/Tutorial_CT.txt");
			LoadTutorial_PL("PL/Tutorial_PL.txt");
			LoadTutorial_CZ("CZ/Tutorial_CZ.txt");
			LoadTutorial_AR("AR/Tutorial_AR.txt");
			LoadTutorial_IT("IT/Tutorial_IT.txt");
			LoadTutorial_RO("RO/Tutorial_RO.txt");
			LoadTutorial_JA("JA/Tutorial_JA.txt");
			LoadTutorial_UA("UA/Tutorial_UA.txt");
			LoadTutorial_LA("LA/Tutorial_LA.txt");
			LoadTutorial_TH("TH/Tutorial_TH.txt");
		}
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = base.gameObject;
		}
		if (!mS_)
		{
			mS_ = main_.GetComponent<mainScript>();
		}
		if (!settings_)
		{
			settings_ = main_.GetComponent<settingsScript>();
		}
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!genres_)
		{
			genres_ = main_.GetComponent<genres>();
		}
	}

	public void LoadContent_Themes()
	{
		LoadDevLegends("DATA/DevLegends.txt");
		LoadThemes_EN("EN/Themes_EN.txt");
		LoadThemes_GE("GE/Themes_GE.txt");
		LoadThemes_TU("TU/Themes_TU.txt");
		LoadThemes_CH("CH/Themes_CH.txt");
		LoadThemes_FR("FR/Themes_FR.txt");
		LoadThemes_ES("ES/Themes_ES.txt");
		LoadThemes_KO("KO/Themes_KO.txt");
		LoadThemes_PB("PB/Themes_PB.txt");
		LoadThemes_HU("HU/Themes_HU.txt");
		LoadThemes_RU("RU/Themes_RU.txt");
		LoadThemes_CT("CT/Themes_CT.txt");
		LoadThemes_PL("PL/Themes_PL.txt");
		LoadThemes_CZ("CZ/Themes_CZ.txt");
		LoadThemes_AR("AR/Themes_AR.txt");
		LoadThemes_IT("IT/Themes_IT.txt");
		LoadThemes_RO("RO/Themes_RO.txt");
		LoadThemes_JA("JA/Themes_JA.txt");
		LoadThemes_UA("UA/Themes_UA.txt");
		LoadThemes_LA("LA/Themes_LA.txt");
		LoadThemes_TH("TH/Themes_TH.txt");
		themes_.Init();
		themes_.Load_FITGENRE("EN/Themes_EN.txt");
		themes_.Load_THEMES_MGSR("EN/Themes_EN.txt");
	}

	public string GetStudioBewertung(int sterne)
	{
		return sterne switch
		{
			0 => GetText(1467), 
			1 => GetText(1468), 
			2 => GetText(1469), 
			3 => GetText(1470), 
			4 => GetText(1471), 
			5 => GetText(1472), 
			6 => GetText(1473), 
			7 => GetText(1474), 
			8 => GetText(1475), 
			9 => GetText(1476), 
			10 => GetText(1477), 
			_ => GetText(1467), 
		};
	}

	public string GetGameTyp(int i)
	{
		return i switch
		{
			0 => GetText(322), 
			1 => GetText(323), 
			2 => GetText(324), 
			_ => "<Missing Text>", 
		};
	}

	public string GetGameSize(int i)
	{
		return i switch
		{
			0 => GetText(329), 
			1 => GetText(330), 
			2 => GetText(331), 
			3 => GetText(332), 
			4 => GetText(333), 
			5 => GetText(2193), 
			_ => "<Missing Text>", 
		};
	}

	public string GetGameZielgruppe(int i)
	{
		return i switch
		{
			0 => GetText(337), 
			1 => GetText(338), 
			2 => GetText(339), 
			3 => GetText(340), 
			4 => GetText(341), 
			_ => "<Missing Text>", 
		};
	}

	public string GetText(int i)
	{
		FindScripts();
		if (!textLoaded)
		{
			Awake();
		}
		switch (settings_.language)
		{
		case 0:
			if (text_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_EN[i];
		case 1:
			if (text_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_GE[i];
		case 2:
			if (text_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_TU[i];
		case 3:
			if (text_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_CH[i];
		case 4:
			if (text_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_FR[i];
		case 5:
			if (text_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_ES[i];
		case 6:
			if (text_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_KO[i];
		case 7:
			if (text_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_PB[i];
		case 8:
			if (text_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_HU[i];
		case 9:
			if (text_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_RU[i];
		case 10:
			if (text_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_CT[i];
		case 11:
			if (text_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_PL[i];
		case 12:
			if (text_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_CZ[i];
		case 13:
			if (text_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_AR[i];
		case 14:
			if (text_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_IT[i];
		case 15:
			if (text_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_RO[i];
		case 16:
			if (text_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_JA[i];
		case 17:
			if (text_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_UA[i];
		case 18:
			if (text_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_LA[i];
		case 19:
			if (text_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return text_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetAchivementName(int i)
	{
		FindScripts();
		if (!textLoaded)
		{
			Awake();
		}
		switch (settings_.language)
		{
		case 0:
			if (achivementsName_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_EN[i];
		case 1:
			if (achivementsName_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_GE[i];
		case 2:
			if (achivementsName_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_TU[i];
		case 3:
			if (achivementsName_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_CH[i];
		case 4:
			if (achivementsName_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_FR[i];
		case 5:
			if (achivementsName_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_ES[i];
		case 6:
			if (achivementsName_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_KO[i];
		case 7:
			if (achivementsName_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_PB[i];
		case 8:
			if (achivementsName_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_HU[i];
		case 9:
			if (achivementsName_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_RU[i];
		case 10:
			if (achivementsName_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_CT[i];
		case 11:
			if (achivementsName_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_PL[i];
		case 12:
			if (achivementsName_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_CZ[i];
		case 13:
			if (achivementsName_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_AR[i];
		case 14:
			if (achivementsName_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_IT[i];
		case 15:
			if (achivementsName_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_RO[i];
		case 16:
			if (achivementsName_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_JA[i];
		case 17:
			if (achivementsName_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_UA[i];
		case 18:
			if (achivementsName_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_LA[i];
		case 19:
			if (achivementsName_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsName_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetAchivementDesc(int i)
	{
		FindScripts();
		if (!textLoaded)
		{
			Awake();
		}
		switch (settings_.language)
		{
		case 0:
			if (achivementsDesc_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_EN[i];
		case 1:
			if (achivementsDesc_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_GE[i];
		case 2:
			if (achivementsDesc_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_TU[i];
		case 3:
			if (achivementsDesc_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_CH[i];
		case 4:
			if (achivementsDesc_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_FR[i];
		case 5:
			if (achivementsDesc_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_ES[i];
		case 6:
			if (achivementsDesc_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_KO[i];
		case 7:
			if (achivementsDesc_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_PB[i];
		case 8:
			if (achivementsDesc_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_HU[i];
		case 9:
			if (achivementsDesc_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_RU[i];
		case 10:
			if (achivementsDesc_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_CT[i];
		case 11:
			if (achivementsDesc_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_PL[i];
		case 12:
			if (achivementsDesc_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_CZ[i];
		case 13:
			if (achivementsDesc_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_AR[i];
		case 14:
			if (achivementsDesc_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_IT[i];
		case 15:
			if (achivementsDesc_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_RO[i];
		case 16:
			if (achivementsDesc_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_JA[i];
		case 17:
			if (achivementsDesc_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_UA[i];
		case 18:
			if (achivementsDesc_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_LA[i];
		case 19:
			if (achivementsDesc_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return achivementsDesc_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetObjects(int i)
	{
		switch (settings_.language)
		{
		case 0:
			if (objects_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_EN[i];
		case 1:
			if (objects_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_GE[i];
		case 2:
			if (objects_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_TU[i];
		case 3:
			if (objects_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_CH[i];
		case 4:
			if (objects_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_FR[i];
		case 5:
			if (objects_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_ES[i];
		case 6:
			if (objects_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_KO[i];
		case 7:
			if (objects_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_PB[i];
		case 8:
			if (objects_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_HU[i];
		case 9:
			if (objects_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_RU[i];
		case 10:
			if (objects_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_CT[i];
		case 11:
			if (objects_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_PL[i];
		case 12:
			if (objects_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_CZ[i];
		case 13:
			if (objects_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_AR[i];
		case 14:
			if (objects_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_IT[i];
		case 15:
			if (objects_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_RO[i];
		case 16:
			if (objects_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_JA[i];
		case 17:
			if (objects_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_UA[i];
		case 18:
			if (objects_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_LA[i];
		case 19:
			if (objects_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return objects_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetObjectsTooltip(int i)
	{
		switch (settings_.language)
		{
		case 0:
			if (objectsTooltip_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_EN[i];
		case 1:
			if (objectsTooltip_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_GE[i];
		case 2:
			if (objectsTooltip_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_TU[i];
		case 3:
			if (objectsTooltip_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_CH[i];
		case 4:
			if (objectsTooltip_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_FR[i];
		case 5:
			if (objectsTooltip_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_ES[i];
		case 6:
			if (objectsTooltip_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_KO[i];
		case 7:
			if (objectsTooltip_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_PB[i];
		case 8:
			if (objectsTooltip_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_HU[i];
		case 9:
			if (objectsTooltip_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_RU[i];
		case 10:
			if (objectsTooltip_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_CT[i];
		case 11:
			if (objectsTooltip_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_PL[i];
		case 12:
			if (objectsTooltip_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_CZ[i];
		case 13:
			if (objectsTooltip_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_AR[i];
		case 14:
			if (objectsTooltip_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_IT[i];
		case 15:
			if (objectsTooltip_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_RO[i];
		case 16:
			if (objectsTooltip_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_JA[i];
		case 17:
			if (objectsTooltip_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_UA[i];
		case 18:
			if (objectsTooltip_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_LA[i];
		case 19:
			if (objectsTooltip_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return objectsTooltip_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetCountry(int i)
	{
		string result = "";
		switch (settings_.language)
		{
		case 0:
			if (country_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_EN[i];
			break;
		case 1:
			if (country_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_GE[i];
			break;
		case 2:
			if (country_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_TU[i];
			break;
		case 3:
			if (country_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_CH[i];
			break;
		case 4:
			if (country_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_FR[i];
			break;
		case 5:
			if (country_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_ES[i];
			break;
		case 6:
			if (country_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_KO[i];
			break;
		case 7:
			if (country_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_PB[i];
			break;
		case 8:
			if (country_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_HU[i];
			break;
		case 9:
			if (country_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_RU[i];
			break;
		case 10:
			if (country_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_CT[i];
			break;
		case 11:
			if (country_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_PL[i];
			break;
		case 12:
			if (country_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_CZ[i];
			break;
		case 13:
			if (country_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_AR[i];
			break;
		case 14:
			if (country_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_IT[i];
			break;
		case 15:
			if (country_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_RO[i];
			break;
		case 16:
			if (country_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_JA[i];
			break;
		case 17:
			if (country_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_UA[i];
			break;
		case 18:
			if (country_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_LA[i];
			break;
		case 19:
			if (country_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			result = country_TH[i];
			break;
		}
		return result;
	}

	public string GetQuotes()
	{
		return settings_.language switch
		{
			0 => quotes_EN[UnityEngine.Random.Range(0, quotes_EN.Length)], 
			1 => quotes_GE[UnityEngine.Random.Range(0, quotes_GE.Length)], 
			2 => quotes_TU[UnityEngine.Random.Range(0, quotes_TU.Length)], 
			3 => quotes_CH[UnityEngine.Random.Range(0, quotes_CH.Length)], 
			4 => quotes_FR[UnityEngine.Random.Range(0, quotes_FR.Length)], 
			5 => quotes_ES[UnityEngine.Random.Range(0, quotes_ES.Length)], 
			6 => quotes_KO[UnityEngine.Random.Range(0, quotes_KO.Length)], 
			7 => quotes_PB[UnityEngine.Random.Range(0, quotes_PB.Length)], 
			8 => quotes_HU[UnityEngine.Random.Range(0, quotes_HU.Length)], 
			9 => quotes_RU[UnityEngine.Random.Range(0, quotes_RU.Length)], 
			10 => quotes_CT[UnityEngine.Random.Range(0, quotes_CT.Length)], 
			11 => quotes_PL[UnityEngine.Random.Range(0, quotes_PL.Length)], 
			12 => quotes_CZ[UnityEngine.Random.Range(0, quotes_CZ.Length)], 
			13 => quotes_AR[UnityEngine.Random.Range(0, quotes_AR.Length)], 
			14 => quotes_IT[UnityEngine.Random.Range(0, quotes_IT.Length)], 
			15 => quotes_RO[UnityEngine.Random.Range(0, quotes_RO.Length)], 
			16 => quotes_JA[UnityEngine.Random.Range(0, quotes_JA.Length)], 
			17 => quotes_UA[UnityEngine.Random.Range(0, quotes_UA.Length)], 
			18 => quotes_LA[UnityEngine.Random.Range(0, quotes_LA.Length)], 
			19 => quotes_TH[UnityEngine.Random.Range(0, quotes_TH.Length)], 
			_ => "<Missing Text>", 
		};
	}

	public string GetFanLetter(int i)
	{
		switch (settings_.language)
		{
		case 0:
			if (fanLetter_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_EN[i];
		case 1:
			if (fanLetter_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_GE[i];
		case 2:
			if (fanLetter_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_TU[i];
		case 3:
			if (fanLetter_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_CH[i];
		case 4:
			if (fanLetter_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_FR[i];
		case 5:
			if (fanLetter_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_ES[i];
		case 6:
			if (fanLetter_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_KO[i];
		case 7:
			if (fanLetter_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_PB[i];
		case 8:
			if (fanLetter_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_HU[i];
		case 9:
			if (fanLetter_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_RU[i];
		case 10:
			if (fanLetter_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_CT[i];
		case 11:
			if (fanLetter_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_PL[i];
		case 12:
			if (fanLetter_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_CZ[i];
		case 13:
			if (fanLetter_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_AR[i];
		case 14:
			if (fanLetter_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_IT[i];
		case 15:
			if (fanLetter_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_RO[i];
		case 16:
			if (fanLetter_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_JA[i];
		case 17:
			if (fanLetter_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_UA[i];
		case 18:
			if (fanLetter_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_LA[i];
		case 19:
			if (fanLetter_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return fanLetter_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetTutorial(int i)
	{
		switch (settings_.language)
		{
		case 0:
			if (tutorial_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_EN[i];
		case 1:
			if (tutorial_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_GE[i];
		case 2:
			if (tutorial_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_TU[i];
		case 3:
			if (tutorial_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_CH[i];
		case 4:
			if (tutorial_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_FR[i];
		case 5:
			if (tutorial_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_ES[i];
		case 6:
			if (tutorial_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_KO[i];
		case 7:
			if (tutorial_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_PB[i];
		case 8:
			if (tutorial_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_HU[i];
		case 9:
			if (tutorial_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_RU[i];
		case 10:
			if (tutorial_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_CT[i];
		case 11:
			if (tutorial_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_PL[i];
		case 12:
			if (tutorial_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_CZ[i];
		case 13:
			if (tutorial_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_AR[i];
		case 14:
			if (tutorial_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_IT[i];
		case 15:
			if (tutorial_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_RO[i];
		case 16:
			if (tutorial_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_JA[i];
		case 17:
			if (tutorial_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_UA[i];
		case 18:
			if (tutorial_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_LA[i];
		case 19:
			if (tutorial_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return tutorial_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetThemes(int i)
	{
		if (themes_EN.Length == 0)
		{
			return "<Not initalized>";
		}
		switch (settings_.language)
		{
		case 0:
			if (themes_EN.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_EN[i];
		case 1:
			if (themes_GE.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_GE[i];
		case 2:
			if (themes_TU.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_TU[i];
		case 3:
			if (themes_CH.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_CH[i];
		case 4:
			if (themes_FR.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_FR[i];
		case 5:
			if (themes_ES.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_ES[i];
		case 6:
			if (themes_KO.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_KO[i];
		case 7:
			if (themes_PB.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_PB[i];
		case 8:
			if (themes_HU.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_HU[i];
		case 9:
			if (themes_RU.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_RU[i];
		case 10:
			if (themes_CT.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_CT[i];
		case 11:
			if (themes_PL.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_PL[i];
		case 12:
			if (themes_CZ.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_CZ[i];
		case 13:
			if (themes_AR.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_AR[i];
		case 14:
			if (themes_IT.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_IT[i];
		case 15:
			if (themes_RO.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_RO[i];
		case 16:
			if (themes_JA.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_JA[i];
		case 17:
			if (themes_UA.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_UA[i];
		case 18:
			if (themes_LA.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_LA[i];
		case 19:
			if (themes_TH.Length <= i)
			{
				return "<Missing Text>";
			}
			return themes_TH[i];
		default:
			return "<Missing Text>";
		}
	}

	public string GetRandomCharName(bool male)
	{
		if (male)
		{
			return (namesMale[UnityEngine.Random.Range(0, namesMale.Length)] + " " + surname[UnityEngine.Random.Range(0, surname.Length)]).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
		}
		return (namesFemale[UnityEngine.Random.Range(0, namesFemale.Length)] + " " + surname[UnityEngine.Random.Range(0, surname.Length)]).Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
	}

	public string GetRandomNPCAddonBundleName()
	{
		return npcAddonBundles[UnityEngine.Random.Range(0, npcAddonBundles.Length)];
	}

	public string GetRandomNPCBundleName()
	{
		return npcBundles[UnityEngine.Random.Range(0, npcBundles.Length)];
	}

	public string GetRandomNPCAddonName()
	{
		return npcAddons[UnityEngine.Random.Range(0, npcAddons.Length)];
	}

	public string GetRandomNpcSpinoffName(int genre_)
	{
		bool flag = false;
		List<string> list = new List<string>();
		if (UnityEngine.Random.Range(0, 100) < 20)
		{
			for (int i = 0; i < npcSpinOffs.Length; i++)
			{
				if (!flag && npcSpinOffs[i].Contains("[ALL]"))
				{
					flag = true;
				}
				else if (flag)
				{
					if (npcSpinOffs[i].Contains("[END]"))
					{
						break;
					}
					if (npcSpinOffs[i].Length > 0)
					{
						list.Add(npcSpinOffs[i]);
					}
				}
			}
			if (list.Count > 0)
			{
				return list[UnityEngine.Random.Range(0, list.Count)];
			}
		}
		list.Clear();
		flag = false;
		for (int j = 0; j < npcSpinOffs.Length; j++)
		{
			if (!flag && npcSpinOffs[j].Contains("[" + genre_ + "]"))
			{
				flag = true;
			}
			else if (flag)
			{
				if (npcSpinOffs[j].Contains("[END]"))
				{
					break;
				}
				if (npcSpinOffs[j].Length > 0)
				{
					list.Add(npcSpinOffs[j]);
				}
			}
		}
		if (list.Count > 0)
		{
			return list[UnityEngine.Random.Range(0, list.Count)];
		}
		return "";
	}

	public string GetRandomEngineName()
	{
		return randomEngineNames[UnityEngine.Random.Range(0, randomEngineNames.Length)];
	}

	public string GetRandomGameName()
	{
		return randomGameNames[UnityEngine.Random.Range(0, randomGameNames.Length)].Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
	}

	public string GetPlatformName()
	{
		return randomPlatformNames[UnityEngine.Random.Range(0, randomPlatformNames.Length)].Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
	}

	public string GetRandomCompanyName()
	{
		return randomCompanyNames[UnityEngine.Random.Range(0, randomCompanyNames.Length)].Replace("\n", string.Empty).Replace("\r", string.Empty).Replace("\t", string.Empty);
	}

	public string GetContractWork(int nr)
	{
		string text = "";
		switch (settings_.language)
		{
		case 0:
			if (contractWork_EN.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_EN[nr];
			break;
		case 1:
			if (contractWork_GE.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_GE[nr];
			break;
		case 2:
			if (contractWork_TU.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_TU[nr];
			break;
		case 3:
			if (contractWork_CH.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_CH[nr];
			break;
		case 4:
			if (contractWork_FR.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_FR[nr];
			break;
		case 5:
			if (contractWork_ES.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_ES[nr];
			break;
		case 6:
			if (contractWork_KO.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_KO[nr];
			break;
		case 7:
			if (contractWork_PB.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_PB[nr];
			break;
		case 8:
			if (contractWork_HU.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_HU[nr];
			break;
		case 9:
			if (contractWork_RU.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_RU[nr];
			break;
		case 10:
			if (contractWork_CT.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_CT[nr];
			break;
		case 11:
			if (contractWork_PL.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_PL[nr];
			break;
		case 12:
			if (contractWork_CZ.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_CZ[nr];
			break;
		case 13:
			if (contractWork_AR.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_AR[nr];
			break;
		case 14:
			if (contractWork_IT.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_IT[nr];
			break;
		case 15:
			if (contractWork_RO.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_RO[nr];
			break;
		case 16:
			if (contractWork_JA.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_JA[nr];
			break;
		case 17:
			if (contractWork_UA.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_UA[nr];
			break;
		case 18:
			if (contractWork_LA.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_LA[nr];
			break;
		case 19:
			if (contractWork_TH.Length < nr)
			{
				return "<Missing Text>";
			}
			text = contractWork_TH[nr];
			break;
		}
		for (int i = 0; i < 10; i++)
		{
			text = text.Replace("<" + i + ">", "");
		}
		return text;
	}

	public int GetRandomContractNumber(int art_)
	{
		int num = 0;
		int i = 0;
		string text = "";
		for (; i < 10000; i++)
		{
			switch (settings_.language)
			{
			case 0:
				num = UnityEngine.Random.Range(0, contractWork_EN.Length);
				break;
			case 1:
				num = UnityEngine.Random.Range(0, contractWork_GE.Length);
				break;
			case 2:
				num = UnityEngine.Random.Range(0, contractWork_TU.Length);
				break;
			case 3:
				num = UnityEngine.Random.Range(0, contractWork_CH.Length);
				break;
			case 4:
				num = UnityEngine.Random.Range(0, contractWork_FR.Length);
				break;
			case 5:
				num = UnityEngine.Random.Range(0, contractWork_ES.Length);
				break;
			case 6:
				num = UnityEngine.Random.Range(0, contractWork_KO.Length);
				break;
			case 7:
				num = UnityEngine.Random.Range(0, contractWork_PB.Length);
				break;
			case 8:
				num = UnityEngine.Random.Range(0, contractWork_HU.Length);
				break;
			case 9:
				num = UnityEngine.Random.Range(0, contractWork_RU.Length);
				break;
			case 10:
				num = UnityEngine.Random.Range(0, contractWork_CT.Length);
				break;
			case 11:
				num = UnityEngine.Random.Range(0, contractWork_PL.Length);
				break;
			case 12:
				num = UnityEngine.Random.Range(0, contractWork_CZ.Length);
				break;
			case 13:
				num = UnityEngine.Random.Range(0, contractWork_AR.Length);
				break;
			case 14:
				num = UnityEngine.Random.Range(0, contractWork_IT.Length);
				break;
			case 15:
				num = UnityEngine.Random.Range(0, contractWork_RO.Length);
				break;
			case 16:
				num = UnityEngine.Random.Range(0, contractWork_JA.Length);
				break;
			case 17:
				num = UnityEngine.Random.Range(0, contractWork_UA.Length);
				break;
			case 18:
				num = UnityEngine.Random.Range(0, contractWork_LA.Length);
				break;
			case 19:
				num = UnityEngine.Random.Range(0, contractWork_TH.Length);
				break;
			}
			switch (settings_.language)
			{
			case 0:
				text = contractWork_EN[num];
				break;
			case 1:
				text = contractWork_GE[num];
				break;
			case 2:
				text = contractWork_TU[num];
				break;
			case 3:
				text = contractWork_CH[num];
				break;
			case 4:
				text = contractWork_FR[num];
				break;
			case 5:
				text = contractWork_ES[num];
				break;
			case 6:
				text = contractWork_KO[num];
				break;
			case 7:
				text = contractWork_PB[num];
				break;
			case 8:
				text = contractWork_HU[num];
				break;
			case 9:
				text = contractWork_RU[num];
				break;
			case 10:
				text = contractWork_CT[num];
				break;
			case 11:
				text = contractWork_PL[num];
				break;
			case 12:
				text = contractWork_CZ[num];
				break;
			case 13:
				text = contractWork_AR[num];
				break;
			case 14:
				text = contractWork_IT[num];
				break;
			case 15:
				text = contractWork_RO[num];
				break;
			case 16:
				text = contractWork_JA[num];
				break;
			case 17:
				text = contractWork_UA[num];
				break;
			case 18:
				text = contractWork_LA[num];
				break;
			case 19:
				text = contractWork_TH[num];
				break;
			}
			if (text.Contains("<" + art_ + ">"))
			{
				break;
			}
		}
		return num;
	}

	public string GetRandomNpcGame(int genre_, gameScript game_)
	{
		string text = "";
		int gameMainTheme = -1;
		int gameSubTheme = -1;
		int gameZielgruppe = -1;
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		if (npcGameNameInUse.Length == 0)
		{
			npcGameNameInUse = new bool[npcGames.Length];
		}
		for (int i = 0; i < npcGames.Length; i++)
		{
			if (npcGameNameInUse[i] || !npcGames[i].Contains("<" + genre_ + ">"))
			{
				continue;
			}
			text = npcGames[i];
			npcGameNameInUse[i] = true;
			gameMainTheme = GetTopicFromNpcGameNames(i);
			gameSubTheme = GetSubTopicFromNpcGameNames(i);
			gameZielgruppe = GetTargetGroupFromNpcGameNames(i);
			flag = GetRomanFromNpcGameNames(i);
			flag2 = GetArabicFromNpcGameNames(i);
			flag3 = GetNoSpinFromNpcGameNames(i);
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_NpcGameName(i, ip_: false);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_NpcGameName(i, ip_: false);
				}
			}
			break;
		}
		if (text.Length <= 0)
		{
			return "";
		}
		if (text == null)
		{
			return "";
		}
		for (int j = 0; j < genres_.genres_LEVEL.Length; j++)
		{
			text = text.Replace("<" + j + ">", "");
		}
		text = text.Replace("<T" + gameMainTheme + ">", "");
		text = text.Replace("<ST" + gameSubTheme + ">", "");
		text = text.Replace("<TG" + gameZielgruppe + ">", "");
		text = text.Replace("<ROM>", "");
		text = text.Replace("<ARA>", "");
		text = text.Replace("<NOSPIN>", "");
		text = text.Replace("\n", string.Empty);
		text = text.Replace("\r", string.Empty);
		text = text.Replace("\t", string.Empty);
		text = text.Substring(0, text.Length - 1);
		game_.SetMyName(text);
		game_.gameMainTheme = gameMainTheme;
		game_.gameSubTheme = gameSubTheme;
		game_.gameZielgruppe = gameZielgruppe;
		if (flag)
		{
			game_.npcLateinNumbers = true;
		}
		if (flag2)
		{
			game_.npcLateinNumbers = false;
		}
		if (flag3)
		{
			game_.noSpinOff = true;
		}
		return text;
	}

	private int GetGenreFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<G"))
		{
			for (int j = 0; j < genres_.genres_LEVEL.Length; j++)
			{
				if (npcIPs[i].Contains("<G" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private int GetSubGenreFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<SG"))
		{
			for (int j = 0; j < genres_.genres_LEVEL.Length; j++)
			{
				if (npcIPs[i].Contains("<SG" + j + ">"))
				{
					return j;
				}
			}
		}
		return -1;
	}

	private int GetTopicFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<T"))
		{
			for (int j = 0; j < themes_.themes_LEVEL.Length; j++)
			{
				if (npcIPs[i].Contains("<T" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private int GetTopicFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<T"))
		{
			for (int j = 0; j < themes_.themes_LEVEL.Length; j++)
			{
				if (npcGames[i].Contains("<T" + j + ">"))
				{
					return j;
				}
			}
		}
		return -1;
	}

	private int GetSubTopicFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<ST"))
		{
			for (int j = 0; j < themes_.themes_LEVEL.Length; j++)
			{
				if (npcIPs[i].Contains("<ST" + j + ">"))
				{
					return j;
				}
			}
		}
		return -1;
	}

	private int GetSubTopicFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<ST"))
		{
			for (int j = 0; j < themes_.themes_LEVEL.Length; j++)
			{
				if (npcGames[i].Contains("<ST" + j + ">"))
				{
					return j;
				}
			}
		}
		return -1;
	}

	private int GetReviewFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<%"))
		{
			for (int num = 100; num > 0; num--)
			{
				if (npcIPs[i].Contains("<%" + num + ">"))
				{
					return num;
				}
			}
		}
		return 0;
	}

	private int GetYearFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<Y"))
		{
			for (int j = 1976; j < 2099; j++)
			{
				if (npcIPs[i].Contains("<Y" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private int GetPlatformFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<PL"))
		{
			for (int j = 0; j < mS_.arrayPlatformsScripts.Length; j++)
			{
				if (npcIPs[i].Contains("<PL" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private int GetTargetGroupFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<TG"))
		{
			for (int j = 0; j < 5; j++)
			{
				if (npcIPs[i].Contains("<TG" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private int GetTargetGroupFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<TG"))
		{
			for (int j = 0; j < 5; j++)
			{
				if (npcGames[i].Contains("<TG" + j + ">"))
				{
					return j;
				}
			}
		}
		return 0;
	}

	private bool GetExclusivFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<EX>"))
		{
			return true;
		}
		return false;
	}

	private bool GetF2PFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<F2P>"))
		{
			return true;
		}
		return false;
	}

	private bool GetMMOFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<MMO>"))
		{
			return true;
		}
		return false;
	}

	private bool GetPlStaticFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<PLSTATIC>"))
		{
			return true;
		}
		return false;
	}

	private bool GetNoSpinoffFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<NOSPIN>"))
		{
			return true;
		}
		return false;
	}

	private bool GetRomanFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<ROM>"))
		{
			return true;
		}
		return false;
	}

	private bool GetArabicFromSonderIP(int i)
	{
		if (npcIPs[i].Contains("<ARA>"))
		{
			return true;
		}
		return false;
	}

	private bool GetRomanFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<ROM>"))
		{
			return true;
		}
		return false;
	}

	private bool GetArabicFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<ARA>"))
		{
			return true;
		}
		return false;
	}

	private bool GetPlStaticFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<PLSTATIC>"))
		{
			return true;
		}
		return false;
	}

	private bool GetNoSpinFromNpcGameNames(int i)
	{
		if (npcGames[i].Contains("<NOSPIN>"))
		{
			return true;
		}
		return false;
	}

	public string GetRandomNpcIP(int publisher_, gameScript game_)
	{
		string text = "";
		int num = 0;
		int num2 = -1;
		int gameMainTheme = 0;
		int gameSubTheme = -1;
		int sonderIPMindestreview = 0;
		int num3 = 0;
		int gameZielgruppe = 0;
		bool flag = false;
		bool flag2 = false;
		int num4 = -1;
		bool flag3 = false;
		bool flag4 = false;
		bool flag5 = false;
		bool flag6 = false;
		bool flag7 = false;
		if (npcIPsInUse.Length == 0)
		{
			npcIPsInUse = new bool[npcIPs.Length];
		}
		for (int i = 0; i < npcIPs.Length; i++)
		{
			if (npcIPsInUse[i] || !npcIPs[i].Contains("<P" + publisher_ + ">"))
			{
				continue;
			}
			num = GetGenreFromSonderIP(i);
			num2 = GetSubGenreFromSonderIP(i);
			bool flag8 = false;
			if (num2 == -1)
			{
				flag8 = true;
			}
			else if (genres_.genres_UNLOCK[num2])
			{
				flag8 = true;
			}
			if (!genres_.genres_UNLOCK[num] || !flag8)
			{
				continue;
			}
			num3 = GetYearFromSonderIP(i);
			if (mS_.year < num3)
			{
				continue;
			}
			text = npcIPs[i];
			npcIPsInUse[i] = true;
			gameMainTheme = GetTopicFromSonderIP(i);
			gameSubTheme = GetSubTopicFromSonderIP(i);
			sonderIPMindestreview = GetReviewFromSonderIP(i);
			gameZielgruppe = GetTargetGroupFromSonderIP(i);
			flag = GetRomanFromSonderIP(i);
			flag2 = GetArabicFromSonderIP(i);
			num4 = GetPlatformFromSonderIP(i);
			flag3 = GetExclusivFromSonderIP(i);
			flag4 = GetF2PFromSonderIP(i);
			flag5 = GetMMOFromSonderIP(i);
			flag6 = GetPlStaticFromSonderIP(i);
			flag7 = GetNoSpinoffFromSonderIP(i);
			if (mS_.multiplayer)
			{
				if (mS_.mpCalls_.isServer)
				{
					mS_.mpCalls_.SERVER_Send_NpcGameName(i, ip_: true);
				}
				else
				{
					mS_.mpCalls_.CLIENT_Send_NpcGameName(i, ip_: true);
				}
			}
			break;
		}
		if (text.Length <= 0)
		{
			return "";
		}
		if (text == null)
		{
			return "";
		}
		text = text.Replace("<G" + num + ">", "");
		text = text.Replace("<SG" + num2 + ">", "");
		text = text.Replace("<P" + publisher_ + ">", "");
		text = text.Replace("<T" + gameMainTheme + ">", "");
		text = text.Replace("<ST" + gameSubTheme + ">", "");
		text = text.Replace("<%" + sonderIPMindestreview + ">", "");
		text = text.Replace("<Y" + num3 + ">", "");
		text = text.Replace("<TG" + gameZielgruppe + ">", "");
		text = text.Replace("<ROM>", "");
		text = text.Replace("<ARA>", "");
		text = text.Replace("<EX>", "");
		text = text.Replace("<F2P>", "");
		text = text.Replace("<MMO>", "");
		text = text.Replace("<PLSTATIC>", "");
		text = text.Replace("<NOSPIN>", "");
		text = text.Replace("<PL" + num4 + ">", "");
		text = text.Replace("\n", string.Empty);
		text = text.Replace("\r", string.Empty);
		text = text.Replace("\t", string.Empty);
		text = text.Substring(0, text.Length - 1);
		game_.SetMyName(text);
		game_.maingenre = num;
		game_.subgenre = num2;
		game_.gameMainTheme = gameMainTheme;
		game_.gameSubTheme = gameSubTheme;
		game_.sonderIPMindestreview = sonderIPMindestreview;
		game_.gameZielgruppe = gameZielgruppe;
		if (flag)
		{
			game_.npcLateinNumbers = true;
		}
		if (flag2)
		{
			game_.npcLateinNumbers = false;
		}
		if (flag3)
		{
			game_.exklusiv = true;
		}
		if (flag4)
		{
			game_.gameTyp = 2;
		}
		if (flag5)
		{
			game_.gameTyp = 1;
		}
		if (flag6)
		{
			game_.platformStatic = true;
		}
		if (flag7)
		{
			game_.noSpinOff = true;
		}
		if (num4 != -1)
		{
			for (int j = 0; j < mS_.arrayPlatformsScripts.Length; j++)
			{
				if ((bool)mS_.arrayPlatformsScripts[j] && mS_.arrayPlatformsScripts[j].myID == num4 && mS_.arrayPlatformsScripts[j].IsVerfuegbar())
				{
					game_.gamePlatform[0] = num4;
					game_.gamePlatformScript[0] = mS_.arrayPlatformsScripts[j];
					break;
				}
			}
		}
		return text;
	}

	private string OpenFile(string filename)
	{
		StreamReader streamReader = new StreamReader(Application.dataPath + "/Extern/Text/" + filename, Encoding.Unicode);
		string result = streamReader.ReadToEnd();
		streamReader.Close();
		return result;
	}

	private void LoadGlobalText()
	{
		namesMale = OpenFile("DATA/NamesMale.txt").Split("\n"[0]);
		namesMale = RemoveComments(namesMale);
		namesFemale = OpenFile("DATA/NamesFemale.txt").Split("\n"[0]);
		namesFemale = RemoveComments(namesFemale);
		surname = OpenFile("DATA/Surname.txt").Split("\n"[0]);
		surname = RemoveComments(surname);
		randomEngineNames = OpenFile("DATA/RandomEngineNames.txt").Split("\n"[0]);
		randomEngineNames = RemoveComments(randomEngineNames);
		randomGameNames = OpenFile("DATA/RandomGameNames.txt").Split("\n"[0]);
		randomGameNames = RemoveComments(randomGameNames);
		randomPlatformNames = OpenFile("DATA/RandomPlatformNames.txt").Split("\n"[0]);
		randomPlatformNames = RemoveComments(randomPlatformNames);
		randomCompanyNames = OpenFile("DATA/RandomCompanyNames.txt").Split("\n"[0]);
		randomCompanyNames = RemoveComments(randomCompanyNames);
		npcAddons = OpenFile("DATA/npcAddons.txt").Split("\n"[0]);
		npcAddons = RemoveComments(npcAddons);
		npcBundles = OpenFile("DATA/npcBundles.txt").Split("\n"[0]);
		npcBundles = RemoveComments(npcBundles);
		npcAddonBundles = OpenFile("DATA/npcAddonBundles.txt").Split("\n"[0]);
		npcAddonBundles = RemoveComments(npcAddonBundles);
		npcSpinOffs = OpenFile("DATA/npcSpinoffs.txt").Split("\n"[0]);
		npcSpinOffs = RemoveComments(npcSpinOffs);
		credits = OpenFile("DATA/Credits.txt");
	}

	public void LoadContent_NPCGameNames()
	{
		npcGames = OpenFile("DATA/npcGames.txt").Split("\n"[0]);
		npcGames = RemoveComments(npcGames);
		npcGameNameInUse = new bool[npcGames.Length];
		Reshuffle(npcGames);
	}

	public void LoadContent_NpcIPs()
	{
		npcIPs = OpenFile("DATA/npcIPs.txt").Split("\n"[0]);
		npcIPs = RemoveComments(npcIPs);
		npcIPsInUse = new bool[npcIPs.Length];
	}

	private void Reshuffle(string[] texts)
	{
		UnityEngine.Random.InitState(1000);
		for (int i = 0; i < texts.Length; i++)
		{
			string text = texts[i];
			int num = UnityEngine.Random.Range(i, texts.Length);
			texts[i] = texts[num];
			texts[num] = text;
		}
		UnityEngine.Random.InitState((int)DateTime.Now.Ticks);
	}

	private void LoadTexts_GE(string filename)
	{
		int num = 0;
		text_GE = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_GE.Length; i++)
		{
			text_GE[i] = text_GE[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_EN(string filename)
	{
		int num = 0;
		text_EN = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_EN.Length; i++)
		{
			text_EN[i] = text_EN[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_TU(string filename)
	{
		int num = 0;
		text_TU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_TU.Length; i++)
		{
			text_TU[i] = text_TU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_CH(string filename)
	{
		int num = 0;
		text_CH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_CH.Length; i++)
		{
			text_CH[i] = text_CH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_FR(string filename)
	{
		int num = 0;
		text_FR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_FR.Length; i++)
		{
			text_FR[i] = text_FR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_ES(string filename)
	{
		int num = 0;
		text_ES = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_ES.Length; i++)
		{
			text_ES[i] = text_ES[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_KO(string filename)
	{
		int num = 0;
		text_KO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_KO.Length; i++)
		{
			text_KO[i] = text_KO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_PB(string filename)
	{
		int num = 0;
		text_PB = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_PB.Length; i++)
		{
			text_PB[i] = text_PB[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_HU(string filename)
	{
		int num = 0;
		text_HU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_HU.Length; i++)
		{
			text_HU[i] = text_HU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_RU(string filename)
	{
		int num = 0;
		text_RU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_RU.Length; i++)
		{
			text_RU[i] = text_RU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_CT(string filename)
	{
		int num = 0;
		text_CT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_CT.Length; i++)
		{
			text_CT[i] = text_CT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_PL(string filename)
	{
		int num = 0;
		text_PL = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_PL.Length; i++)
		{
			text_PL[i] = text_PL[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_CZ(string filename)
	{
		int num = 0;
		text_CZ = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_CZ.Length; i++)
		{
			text_CZ[i] = text_CZ[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_AR(string filename)
	{
		int num = 0;
		text_AR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_AR.Length; i++)
		{
			text_AR[i] = text_AR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_IT(string filename)
	{
		int num = 0;
		text_IT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_IT.Length; i++)
		{
			text_IT[i] = text_IT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_RO(string filename)
	{
		int num = 0;
		text_RO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_RO.Length; i++)
		{
			text_RO[i] = text_RO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_JA(string filename)
	{
		int num = 0;
		text_JA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_JA.Length; i++)
		{
			text_JA[i] = text_JA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_UA(string filename)
	{
		int num = 0;
		text_UA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_UA.Length; i++)
		{
			text_UA[i] = text_UA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_LA(string filename)
	{
		int num = 0;
		text_LA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_LA.Length; i++)
		{
			text_LA[i] = text_LA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTexts_TH(string filename)
	{
		int num = 0;
		text_TH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < text_TH.Length; i++)
		{
			text_TH[i] = text_TH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_GE(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_GE = new string[array.Length / 2];
		achivementsDesc_GE = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_GE[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_GE[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_EN(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_EN = new string[array.Length / 2];
		achivementsDesc_EN = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_EN[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_EN[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_TU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_TU = new string[array.Length / 2];
		achivementsDesc_TU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_TU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_TU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_CH(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_CH = new string[array.Length / 2];
		achivementsDesc_CH = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_CH[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_CH[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_FR(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_FR = new string[array.Length / 2];
		achivementsDesc_FR = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_FR[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_FR[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_ES(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_ES = new string[array.Length / 2];
		achivementsDesc_ES = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_ES[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_ES[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_KO(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_KO = new string[array.Length / 2];
		achivementsDesc_KO = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_KO[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_KO[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_PB(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_PB = new string[array.Length / 2];
		achivementsDesc_PB = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_PB[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_PB[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_HU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_HU = new string[array.Length / 2];
		achivementsDesc_HU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_HU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_HU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_RU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_RU = new string[array.Length / 2];
		achivementsDesc_RU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_RU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_RU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_CT(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_CT = new string[array.Length / 2];
		achivementsDesc_CT = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_CT[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_CT[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_PL(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_PL = new string[array.Length / 2];
		achivementsDesc_PL = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_PL[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_PL[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_CZ(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_CZ = new string[array.Length / 2];
		achivementsDesc_CZ = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_CZ[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_CZ[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_AR(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_AR = new string[array.Length / 2];
		achivementsDesc_AR = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_AR[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_AR[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_IT(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_IT = new string[array.Length / 2];
		achivementsDesc_IT = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_IT[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_IT[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_RO(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_RO = new string[array.Length / 2];
		achivementsDesc_RO = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_RO[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_RO[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_JA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_JA = new string[array.Length / 2];
		achivementsDesc_JA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_JA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_JA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_UA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_UA = new string[array.Length / 2];
		achivementsDesc_UA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_UA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_UA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_LA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_LA = new string[array.Length / 2];
		achivementsDesc_LA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_LA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_LA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadAchivements_TH(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		achivementsName_TH = new string[array.Length / 2];
		achivementsDesc_TH = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			achivementsName_TH[num] = array[num2].Replace("<br>", "\n");
			num2++;
			achivementsDesc_TH[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_GE(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_GE = new string[array.Length / 2];
		objectsTooltip_GE = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_GE[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_GE[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_EN(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_EN = new string[array.Length / 2];
		objectsTooltip_EN = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_EN[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_EN[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_TU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_TU = new string[array.Length / 2];
		objectsTooltip_TU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_TU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_TU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_CH(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_CH = new string[array.Length / 2];
		objectsTooltip_CH = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_CH[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_CH[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_FR(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_FR = new string[array.Length / 2];
		objectsTooltip_FR = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_FR[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_FR[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_ES(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_ES = new string[array.Length / 2];
		objectsTooltip_ES = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_ES[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_ES[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_KO(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_KO = new string[array.Length / 2];
		objectsTooltip_KO = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_KO[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_KO[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_PB(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_PB = new string[array.Length / 2];
		objectsTooltip_PB = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_PB[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_PB[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_HU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_HU = new string[array.Length / 2];
		objectsTooltip_HU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_HU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_HU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_RU(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_RU = new string[array.Length / 2];
		objectsTooltip_RU = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_RU[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_RU[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_CT(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_CT = new string[array.Length / 2];
		objectsTooltip_CT = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_CT[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_CT[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_PL(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_PL = new string[array.Length / 2];
		objectsTooltip_PL = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_PL[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_PL[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_CZ(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_CZ = new string[array.Length / 2];
		objectsTooltip_CZ = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_CZ[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_CZ[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_AR(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_AR = new string[array.Length / 2];
		objectsTooltip_AR = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_AR[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_AR[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_IT(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_IT = new string[array.Length / 2];
		objectsTooltip_IT = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_IT[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_IT[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_RO(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_RO = new string[array.Length / 2];
		objectsTooltip_RO = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_RO[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_RO[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_JA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_JA = new string[array.Length / 2];
		objectsTooltip_JA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_JA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_JA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_UA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_UA = new string[array.Length / 2];
		objectsTooltip_UA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_UA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_UA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_LA(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_LA = new string[array.Length / 2];
		objectsTooltip_LA = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_LA[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_LA[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadObjects_TH(string filename)
	{
		int num = 0;
		string[] array = OpenFile(filename).Split('\n', ';');
		objects_TH = new string[array.Length / 2];
		objectsTooltip_TH = new string[array.Length / 2];
		int num2;
		for (num2 = 0; num2 < array.Length; num2++)
		{
			objects_TH[num] = array[num2].Replace("<br>", "\n");
			num2++;
			objectsTooltip_TH[num] = array[num2].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadCountry_GE(string filename)
	{
		country_GE = OpenFile(filename).Split("\n"[0]);
		int num = genres_.LoadAmountOfGenres("DATA/Genres.txt");
		for (int i = 0; i < country_GE.Length; i++)
		{
			for (int j = 0; j < num; j++)
			{
				if (country_GE[i].Contains("<" + j + ">"))
				{
					country_GE[i] = country_GE[i].Replace("<" + j + ">", "");
					break;
				}
			}
		}
	}

	private void LoadCountry_EN(string filename)
	{
		country_EN = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_TU(string filename)
	{
		country_TU = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_CH(string filename)
	{
		country_CH = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_FR(string filename)
	{
		country_FR = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_ES(string filename)
	{
		country_ES = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_KO(string filename)
	{
		country_KO = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_PB(string filename)
	{
		country_PB = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_HU(string filename)
	{
		country_HU = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_RU(string filename)
	{
		country_RU = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_CT(string filename)
	{
		country_CT = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_PL(string filename)
	{
		country_PL = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_CZ(string filename)
	{
		country_CZ = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_AR(string filename)
	{
		country_AR = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_IT(string filename)
	{
		country_IT = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_RO(string filename)
	{
		country_RO = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_JA(string filename)
	{
		country_JA = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_UA(string filename)
	{
		country_UA = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_LA(string filename)
	{
		country_LA = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadCountry_TH(string filename)
	{
		country_TH = OpenFile(filename).Split("\n"[0]);
	}

	private void LoadQuotes_GE(string filename)
	{
		int num = 0;
		quotes_GE = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_GE.Length; i++)
		{
			quotes_GE[i] = quotes_GE[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_EN(string filename)
	{
		int num = 0;
		quotes_EN = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_EN.Length; i++)
		{
			quotes_EN[i] = quotes_EN[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_TU(string filename)
	{
		int num = 0;
		quotes_TU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_TU.Length; i++)
		{
			quotes_TU[i] = quotes_TU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_CH(string filename)
	{
		int num = 0;
		quotes_CH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_CH.Length; i++)
		{
			quotes_CH[i] = quotes_CH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_FR(string filename)
	{
		int num = 0;
		quotes_FR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_FR.Length; i++)
		{
			quotes_FR[i] = quotes_FR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_ES(string filename)
	{
		int num = 0;
		quotes_ES = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_ES.Length; i++)
		{
			quotes_ES[i] = quotes_ES[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_KO(string filename)
	{
		int num = 0;
		quotes_KO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_KO.Length; i++)
		{
			quotes_KO[i] = quotes_KO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_PB(string filename)
	{
		int num = 0;
		quotes_PB = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_PB.Length; i++)
		{
			quotes_PB[i] = quotes_PB[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_HU(string filename)
	{
		int num = 0;
		quotes_HU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_HU.Length; i++)
		{
			quotes_HU[i] = quotes_HU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_RU(string filename)
	{
		int num = 0;
		quotes_RU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_RU.Length; i++)
		{
			quotes_RU[i] = quotes_RU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_CT(string filename)
	{
		int num = 0;
		quotes_CT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_CT.Length; i++)
		{
			quotes_CT[i] = quotes_CT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_PL(string filename)
	{
		int num = 0;
		quotes_PL = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_PL.Length; i++)
		{
			quotes_PL[i] = quotes_PL[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_CZ(string filename)
	{
		int num = 0;
		quotes_CZ = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_CZ.Length; i++)
		{
			quotes_CZ[i] = quotes_CZ[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_AR(string filename)
	{
		int num = 0;
		quotes_AR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_AR.Length; i++)
		{
			quotes_AR[i] = quotes_AR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_IT(string filename)
	{
		int num = 0;
		quotes_IT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_IT.Length; i++)
		{
			quotes_IT[i] = quotes_IT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_RO(string filename)
	{
		int num = 0;
		quotes_RO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_RO.Length; i++)
		{
			quotes_RO[i] = quotes_RO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_JA(string filename)
	{
		int num = 0;
		quotes_JA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_JA.Length; i++)
		{
			quotes_JA[i] = quotes_JA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_UA(string filename)
	{
		int num = 0;
		quotes_UA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_UA.Length; i++)
		{
			quotes_UA[i] = quotes_UA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_LA(string filename)
	{
		int num = 0;
		quotes_LA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_LA.Length; i++)
		{
			quotes_LA[i] = quotes_LA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadQuotes_TH(string filename)
	{
		int num = 0;
		quotes_TH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < quotes_TH.Length; i++)
		{
			quotes_TH[i] = quotes_TH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_GE(string filename)
	{
		int num = 0;
		contractWork_GE = OpenFile(filename).Split("\n"[0]);
		contractWork_GE = RemoveComments(contractWork_GE);
		for (int i = 0; i < contractWork_GE.Length; i++)
		{
			contractWork_GE[i] = contractWork_GE[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_EN(string filename)
	{
		int num = 0;
		contractWork_EN = OpenFile(filename).Split("\n"[0]);
		contractWork_EN = RemoveComments(contractWork_EN);
		for (int i = 0; i < contractWork_EN.Length; i++)
		{
			contractWork_EN[i] = contractWork_EN[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_TU(string filename)
	{
		int num = 0;
		contractWork_TU = OpenFile(filename).Split("\n"[0]);
		contractWork_TU = RemoveComments(contractWork_TU);
		for (int i = 0; i < contractWork_TU.Length; i++)
		{
			contractWork_TU[i] = contractWork_TU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_CH(string filename)
	{
		int num = 0;
		contractWork_CH = OpenFile(filename).Split("\n"[0]);
		contractWork_CH = RemoveComments(contractWork_CH);
		for (int i = 0; i < contractWork_CH.Length; i++)
		{
			contractWork_CH[i] = contractWork_CH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_FR(string filename)
	{
		int num = 0;
		contractWork_FR = OpenFile(filename).Split("\n"[0]);
		contractWork_FR = RemoveComments(contractWork_FR);
		for (int i = 0; i < contractWork_FR.Length; i++)
		{
			contractWork_FR[i] = contractWork_FR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_ES(string filename)
	{
		int num = 0;
		contractWork_ES = OpenFile(filename).Split("\n"[0]);
		contractWork_ES = RemoveComments(contractWork_ES);
		for (int i = 0; i < contractWork_ES.Length; i++)
		{
			contractWork_ES[i] = contractWork_ES[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_KO(string filename)
	{
		int num = 0;
		contractWork_KO = OpenFile(filename).Split("\n"[0]);
		contractWork_KO = RemoveComments(contractWork_KO);
		for (int i = 0; i < contractWork_KO.Length; i++)
		{
			contractWork_KO[i] = contractWork_KO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_PB(string filename)
	{
		int num = 0;
		contractWork_PB = OpenFile(filename).Split("\n"[0]);
		contractWork_PB = RemoveComments(contractWork_PB);
		for (int i = 0; i < contractWork_PB.Length; i++)
		{
			contractWork_PB[i] = contractWork_PB[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_HU(string filename)
	{
		int num = 0;
		contractWork_HU = OpenFile(filename).Split("\n"[0]);
		contractWork_HU = RemoveComments(contractWork_HU);
		for (int i = 0; i < contractWork_HU.Length; i++)
		{
			contractWork_HU[i] = contractWork_HU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_RU(string filename)
	{
		int num = 0;
		contractWork_RU = OpenFile(filename).Split("\n"[0]);
		contractWork_RU = RemoveComments(contractWork_RU);
		for (int i = 0; i < contractWork_RU.Length; i++)
		{
			contractWork_RU[i] = contractWork_RU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_CT(string filename)
	{
		int num = 0;
		contractWork_CT = OpenFile(filename).Split("\n"[0]);
		contractWork_CT = RemoveComments(contractWork_CT);
		for (int i = 0; i < contractWork_CT.Length; i++)
		{
			contractWork_CT[i] = contractWork_CT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_PL(string filename)
	{
		int num = 0;
		contractWork_PL = OpenFile(filename).Split("\n"[0]);
		contractWork_PL = RemoveComments(contractWork_PL);
		for (int i = 0; i < contractWork_PL.Length; i++)
		{
			contractWork_PL[i] = contractWork_PL[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_CZ(string filename)
	{
		int num = 0;
		contractWork_CZ = OpenFile(filename).Split("\n"[0]);
		contractWork_CZ = RemoveComments(contractWork_CZ);
		for (int i = 0; i < contractWork_CZ.Length; i++)
		{
			contractWork_CZ[i] = contractWork_CZ[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_AR(string filename)
	{
		int num = 0;
		contractWork_AR = OpenFile(filename).Split("\n"[0]);
		contractWork_AR = RemoveComments(contractWork_AR);
		for (int i = 0; i < contractWork_AR.Length; i++)
		{
			contractWork_AR[i] = contractWork_AR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_IT(string filename)
	{
		int num = 0;
		contractWork_IT = OpenFile(filename).Split("\n"[0]);
		contractWork_IT = RemoveComments(contractWork_IT);
		for (int i = 0; i < contractWork_IT.Length; i++)
		{
			contractWork_IT[i] = contractWork_IT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_RO(string filename)
	{
		int num = 0;
		contractWork_RO = OpenFile(filename).Split("\n"[0]);
		contractWork_RO = RemoveComments(contractWork_RO);
		for (int i = 0; i < contractWork_RO.Length; i++)
		{
			contractWork_RO[i] = contractWork_RO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_JA(string filename)
	{
		int num = 0;
		contractWork_JA = OpenFile(filename).Split("\n"[0]);
		contractWork_JA = RemoveComments(contractWork_JA);
		for (int i = 0; i < contractWork_JA.Length; i++)
		{
			contractWork_JA[i] = contractWork_JA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_UA(string filename)
	{
		int num = 0;
		contractWork_UA = OpenFile(filename).Split("\n"[0]);
		contractWork_UA = RemoveComments(contractWork_UA);
		for (int i = 0; i < contractWork_UA.Length; i++)
		{
			contractWork_UA[i] = contractWork_UA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_LA(string filename)
	{
		int num = 0;
		contractWork_LA = OpenFile(filename).Split("\n"[0]);
		contractWork_LA = RemoveComments(contractWork_LA);
		for (int i = 0; i < contractWork_LA.Length; i++)
		{
			contractWork_LA[i] = contractWork_LA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadContractWork_TH(string filename)
	{
		int num = 0;
		contractWork_TH = OpenFile(filename).Split("\n"[0]);
		contractWork_TH = RemoveComments(contractWork_TH);
		for (int i = 0; i < contractWork_TH.Length; i++)
		{
			contractWork_TH[i] = contractWork_TH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_GE(string filename)
	{
		int num = 0;
		fanLetter_GE = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_GE.Length; i++)
		{
			fanLetter_GE[i] = fanLetter_GE[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_EN(string filename)
	{
		int num = 0;
		fanLetter_EN = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_EN.Length; i++)
		{
			fanLetter_EN[i] = fanLetter_EN[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_TU(string filename)
	{
		int num = 0;
		fanLetter_TU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_TU.Length; i++)
		{
			fanLetter_TU[i] = fanLetter_TU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_CH(string filename)
	{
		int num = 0;
		fanLetter_CH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_CH.Length; i++)
		{
			fanLetter_CH[i] = fanLetter_CH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_FR(string filename)
	{
		int num = 0;
		fanLetter_FR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_FR.Length; i++)
		{
			fanLetter_FR[i] = fanLetter_FR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_ES(string filename)
	{
		int num = 0;
		fanLetter_ES = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_ES.Length; i++)
		{
			fanLetter_ES[i] = fanLetter_ES[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_KO(string filename)
	{
		int num = 0;
		fanLetter_KO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_KO.Length; i++)
		{
			fanLetter_KO[i] = fanLetter_KO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_PB(string filename)
	{
		int num = 0;
		fanLetter_PB = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_PB.Length; i++)
		{
			fanLetter_PB[i] = fanLetter_PB[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_HU(string filename)
	{
		int num = 0;
		fanLetter_HU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_HU.Length; i++)
		{
			fanLetter_HU[i] = fanLetter_HU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_RU(string filename)
	{
		int num = 0;
		fanLetter_RU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_RU.Length; i++)
		{
			fanLetter_RU[i] = fanLetter_RU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_CT(string filename)
	{
		int num = 0;
		fanLetter_CT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_CT.Length; i++)
		{
			fanLetter_CT[i] = fanLetter_CT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_PL(string filename)
	{
		int num = 0;
		fanLetter_PL = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_PL.Length; i++)
		{
			fanLetter_PL[i] = fanLetter_PL[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_CZ(string filename)
	{
		int num = 0;
		fanLetter_CZ = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_CZ.Length; i++)
		{
			fanLetter_CZ[i] = fanLetter_CZ[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_AR(string filename)
	{
		int num = 0;
		fanLetter_AR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_AR.Length; i++)
		{
			fanLetter_AR[i] = fanLetter_AR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_IT(string filename)
	{
		int num = 0;
		fanLetter_IT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_IT.Length; i++)
		{
			fanLetter_IT[i] = fanLetter_IT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_RO(string filename)
	{
		int num = 0;
		fanLetter_RO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_RO.Length; i++)
		{
			fanLetter_RO[i] = fanLetter_RO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_JA(string filename)
	{
		int num = 0;
		fanLetter_JA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_JA.Length; i++)
		{
			fanLetter_JA[i] = fanLetter_JA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_UA(string filename)
	{
		int num = 0;
		fanLetter_UA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_UA.Length; i++)
		{
			fanLetter_UA[i] = fanLetter_UA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_LA(string filename)
	{
		int num = 0;
		fanLetter_LA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_LA.Length; i++)
		{
			fanLetter_LA[i] = fanLetter_LA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadFanLetter_TH(string filename)
	{
		int num = 0;
		fanLetter_TH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < fanLetter_TH.Length; i++)
		{
			fanLetter_TH[i] = fanLetter_TH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_GE(string filename)
	{
		int num = 0;
		tutorial_GE = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_GE.Length; i++)
		{
			tutorial_GE[i] = tutorial_GE[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_EN(string filename)
	{
		int num = 0;
		tutorial_EN = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_EN.Length; i++)
		{
			tutorial_EN[i] = tutorial_EN[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_TU(string filename)
	{
		int num = 0;
		tutorial_TU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_TU.Length; i++)
		{
			tutorial_TU[i] = tutorial_TU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_CH(string filename)
	{
		int num = 0;
		tutorial_CH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_CH.Length; i++)
		{
			tutorial_CH[i] = tutorial_CH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_FR(string filename)
	{
		int num = 0;
		tutorial_FR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_FR.Length; i++)
		{
			tutorial_FR[i] = tutorial_FR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_ES(string filename)
	{
		int num = 0;
		tutorial_ES = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_ES.Length; i++)
		{
			tutorial_ES[i] = tutorial_ES[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_KO(string filename)
	{
		int num = 0;
		tutorial_KO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_KO.Length; i++)
		{
			tutorial_KO[i] = tutorial_KO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_PB(string filename)
	{
		int num = 0;
		tutorial_PB = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_PB.Length; i++)
		{
			tutorial_PB[i] = tutorial_PB[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_HU(string filename)
	{
		int num = 0;
		tutorial_HU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_HU.Length; i++)
		{
			tutorial_HU[i] = tutorial_HU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_RU(string filename)
	{
		int num = 0;
		tutorial_RU = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_RU.Length; i++)
		{
			tutorial_RU[i] = tutorial_RU[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_CT(string filename)
	{
		int num = 0;
		tutorial_CT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_CT.Length; i++)
		{
			tutorial_CT[i] = tutorial_CT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_PL(string filename)
	{
		int num = 0;
		tutorial_PL = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_PL.Length; i++)
		{
			tutorial_PL[i] = tutorial_PL[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_CZ(string filename)
	{
		int num = 0;
		tutorial_CZ = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_CZ.Length; i++)
		{
			tutorial_CZ[i] = tutorial_CZ[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_AR(string filename)
	{
		int num = 0;
		tutorial_AR = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_AR.Length; i++)
		{
			tutorial_AR[i] = tutorial_AR[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_IT(string filename)
	{
		int num = 0;
		tutorial_IT = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_IT.Length; i++)
		{
			tutorial_IT[i] = tutorial_IT[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_RO(string filename)
	{
		int num = 0;
		tutorial_RO = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_RO.Length; i++)
		{
			tutorial_RO[i] = tutorial_RO[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_JA(string filename)
	{
		int num = 0;
		tutorial_JA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_JA.Length; i++)
		{
			tutorial_JA[i] = tutorial_JA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_UA(string filename)
	{
		int num = 0;
		tutorial_UA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_UA.Length; i++)
		{
			tutorial_UA[i] = tutorial_UA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_LA(string filename)
	{
		int num = 0;
		tutorial_LA = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_LA.Length; i++)
		{
			tutorial_LA[i] = tutorial_LA[i].Replace("<br>", "\n");
			num++;
		}
	}

	private void LoadTutorial_TH(string filename)
	{
		int num = 0;
		tutorial_TH = OpenFile(filename).Split("\n"[0]);
		for (int i = 0; i < tutorial_TH.Length; i++)
		{
			tutorial_TH[i] = tutorial_TH[i].Replace("<br>", "\n");
			num++;
		}
	}

	private string RemoveThemesFit(string c)
	{
		int num = genres_.LoadAmountOfGenres("DATA/Genres.txt");
		for (int i = 0; i < num; i++)
		{
			c = c.Replace("<" + i + ">", "");
		}
		for (int j = 0; j < 6; j++)
		{
			c = c.Replace("<M" + j + ">", "");
		}
		c = c.Replace("\n", string.Empty);
		c = c.Replace("\r", string.Empty);
		c = c.Replace("\t", string.Empty);
		if (c[c.Length - 1] == ' ')
		{
			c = c.Remove(c.Length - 1);
		}
		return c;
	}

	public string[] RemoveComments(string[] data)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < data.Length; i++)
		{
			if (!data[i].Contains("//"))
			{
				list.Add(data[i]);
			}
		}
		data = list.ToArray();
		return data;
	}

	private void LoadThemes_GE(string filename)
	{
		int num = 0;
		themes_GE = OpenFile(filename).Split("\n"[0]);
		themes_GE = RemoveComments(themes_GE);
		for (int i = 0; i < themes_GE.Length; i++)
		{
			themes_GE[i] = RemoveThemesFit(themes_GE[i]);
			num++;
		}
	}

	private void LoadThemes_EN(string filename)
	{
		int num = 0;
		themes_EN = OpenFile(filename).Split("\n"[0]);
		themes_EN = RemoveComments(themes_EN);
		for (int i = 0; i < themes_EN.Length; i++)
		{
			themes_EN[i] = RemoveThemesFit(themes_EN[i]);
			num++;
		}
	}

	private void LoadThemes_TU(string filename)
	{
		themes_TU = OpenFile(filename).Split("\n"[0]);
		themes_TU = RemoveComments(themes_TU);
	}

	private void LoadThemes_CH(string filename)
	{
		themes_CH = OpenFile(filename).Split("\n"[0]);
		themes_CH = RemoveComments(themes_CH);
	}

	private void LoadThemes_FR(string filename)
	{
		themes_FR = OpenFile(filename).Split("\n"[0]);
		themes_FR = RemoveComments(themes_FR);
	}

	private void LoadThemes_ES(string filename)
	{
		themes_ES = OpenFile(filename).Split("\n"[0]);
		themes_ES = RemoveComments(themes_ES);
	}

	private void LoadThemes_KO(string filename)
	{
		themes_KO = OpenFile(filename).Split("\n"[0]);
		themes_KO = RemoveComments(themes_KO);
	}

	private void LoadThemes_PB(string filename)
	{
		themes_PB = OpenFile(filename).Split("\n"[0]);
		themes_PB = RemoveComments(themes_PB);
	}

	private void LoadThemes_HU(string filename)
	{
		themes_HU = OpenFile(filename).Split("\n"[0]);
		themes_HU = RemoveComments(themes_HU);
	}

	private void LoadThemes_RU(string filename)
	{
		themes_RU = OpenFile(filename).Split("\n"[0]);
		themes_RU = RemoveComments(themes_RU);
	}

	private void LoadThemes_CT(string filename)
	{
		themes_CT = OpenFile(filename).Split("\n"[0]);
		themes_CT = RemoveComments(themes_CT);
	}

	private void LoadThemes_PL(string filename)
	{
		themes_PL = OpenFile(filename).Split("\n"[0]);
		themes_PL = RemoveComments(themes_PL);
	}

	private void LoadThemes_CZ(string filename)
	{
		themes_CZ = OpenFile(filename).Split("\n"[0]);
		themes_CZ = RemoveComments(themes_CZ);
	}

	private void LoadThemes_AR(string filename)
	{
		themes_AR = OpenFile(filename).Split("\n"[0]);
		themes_AR = RemoveComments(themes_AR);
	}

	private void LoadThemes_IT(string filename)
	{
		themes_IT = OpenFile(filename).Split("\n"[0]);
		themes_IT = RemoveComments(themes_IT);
	}

	private void LoadThemes_RO(string filename)
	{
		themes_RO = OpenFile(filename).Split("\n"[0]);
		themes_RO = RemoveComments(themes_RO);
	}

	private void LoadThemes_JA(string filename)
	{
		themes_JA = OpenFile(filename).Split("\n"[0]);
		themes_JA = RemoveComments(themes_JA);
	}

	private void LoadThemes_UA(string filename)
	{
		themes_UA = OpenFile(filename).Split("\n"[0]);
		themes_UA = RemoveComments(themes_UA);
	}

	private void LoadThemes_LA(string filename)
	{
		themes_LA = OpenFile(filename).Split("\n"[0]);
		themes_LA = RemoveComments(themes_LA);
	}

	private void LoadThemes_TH(string filename)
	{
		themes_TH = OpenFile(filename).Split("\n"[0]);
		themes_TH = RemoveComments(themes_TH);
	}

	private void LoadDevLegends(string filename)
	{
		devLegends = OpenFile(filename).Split("\n"[0]);
		devLegends = RemoveComments(devLegends);
		if (mS_.devLegendsInUse.Length == 0)
		{
			mS_.devLegendsInUse = new bool[devLegends.Length];
		}
		if (mS_.devLegendsInUse.Length != devLegends.Length)
		{
			mS_.devLegendsInUse = new bool[devLegends.Length];
		}
	}

	public int GetDevLegend()
	{
		int num = 0;
		while (num < 100000)
		{
			num++;
			if (devLegends.Length == 0)
			{
				return -1;
			}
			int num2 = UnityEngine.Random.Range(0, devLegends.Length);
			if (!mS_.devLegendsInUse[num2])
			{
				return num2;
			}
		}
		return -1;
	}

	public int GetRandomDevLegend(charArbeitsmarkt charA_)
	{
		string text = "";
		int num = -1;
		int beruf = -1;
		bool flag = false;
		model_body = -2;
		model_eyes = -2;
		model_hair = -2;
		model_beard = -2;
		model_skinColor = -2;
		model_hairColor = -2;
		model_beardColor = -2;
		model_HoseColor = -2;
		model_ShirtColor = -2;
		model_Add1Color = -2;
		if (devLegends.Length == 0)
		{
			return -1;
		}
		if (!charA_)
		{
			return -1;
		}
		int devLegend = GetDevLegend();
		if (devLegend == -1)
		{
			return -1;
		}
		if (!mS_.devLegendsInUse[devLegend])
		{
			text = devLegends[devLegend];
			mS_.devLegendsInUse[devLegend] = true;
			num = devLegend;
			flag = GetFemaleFromDevLegends(devLegend);
			beruf = GetJobFromDevLegends(devLegend);
			int num2 = 0;
			for (int i = 1; i < charA_.perks.Length; i++)
			{
				if (devLegends[devLegend].Contains("<" + i + ">"))
				{
					charA_.perks[i] = true;
					num2++;
					if (num2 >= 5)
					{
						break;
					}
				}
			}
			model_body = GetBodyFromDevLegends(devLegend);
			model_eyes = GetEyesFromDevLegends(devLegend);
			model_hair = GetHairFromDevLegends(devLegend);
			model_beard = GetBeardFromDevLegends(devLegend);
			model_skinColor = GetSkinColorFromDevLegends(devLegend);
			model_hairColor = GetHairColorFromDevLegends(devLegend);
			model_beardColor = model_hairColor;
			model_HoseColor = GetBottomColorFromDevLegends(devLegend);
			model_ShirtColor = GetUpperColorFromDevLegends(devLegend);
			model_Add1Color = GetExtraColorFromDevLegends(devLegend);
		}
		if (num <= -1)
		{
			return -1;
		}
		for (int j = 1; j < charA_.perks.Length; j++)
		{
			text = text.Replace("<" + j + ">", string.Empty);
		}
		text = text.Replace("<BODY" + model_body + ">", "");
		text = text.Replace("<EYES" + model_eyes + ">", "");
		text = text.Replace("<HAIR" + model_hair + ">", "");
		text = text.Replace("<BEARD" + model_beard + ">", "");
		text = text.Replace("<SKINCOL" + model_skinColor + ">", "");
		text = text.Replace("<HAIRCOL" + model_hairColor + ">", "");
		text = text.Replace("<BOTTOMCOL" + model_HoseColor + ">", "");
		text = text.Replace("<UPPERCOL" + model_ShirtColor + ">", "");
		text = text.Replace("<EXTRACOL" + model_Add1Color + ">", "");
		text = text.Replace("<D>", string.Empty);
		text = text.Replace("<P>", string.Empty);
		text = text.Replace("<G>", string.Empty);
		text = text.Replace("<S>", string.Empty);
		text = text.Replace("<O>", string.Empty);
		text = text.Replace("<Q>", string.Empty);
		text = text.Replace("<T>", string.Empty);
		text = text.Replace("<R>", string.Empty);
		text = text.Replace("<f>", string.Empty);
		text = text.Replace("\n", string.Empty);
		text = text.Replace("\r", string.Empty);
		text = text.Replace("\t", string.Empty);
		text = text.Substring(0, text.Length - 1);
		charA_.myName = text;
		charA_.legend = num;
		charA_.beruf = beruf;
		if (flag)
		{
			charA_.male = false;
		}
		else
		{
			charA_.male = true;
		}
		return num;
	}

	private int GetJobFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<D>"))
		{
			return 0;
		}
		if (devLegends[i].Contains("<P>"))
		{
			return 1;
		}
		if (devLegends[i].Contains("<G>"))
		{
			return 2;
		}
		if (devLegends[i].Contains("<S>"))
		{
			return 3;
		}
		if (devLegends[i].Contains("<O>"))
		{
			return 4;
		}
		if (devLegends[i].Contains("<Q>"))
		{
			return 5;
		}
		if (devLegends[i].Contains("<T>"))
		{
			return 6;
		}
		if (devLegends[i].Contains("<R>"))
		{
			return 7;
		}
		return 0;
	}

	private bool GetFemaleFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<f>"))
		{
			return true;
		}
		return false;
	}

	private int GetBodyFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<BODY"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<BODY" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetHairFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<HAIR"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<HAIR" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetEyesFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<EYES"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<EYES" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetBeardFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<BEARD"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<BEARD" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetSkinColorFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<SKINCOL"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<SKINCOL" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetHairColorFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<HAIRCOL"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<HAIRCOL" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetUpperColorFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<UPPERCOL"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<UPPERCOL" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetBottomColorFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<BOTTOMCOL"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<BOTTOMCOL" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}

	private int GetExtraColorFromDevLegends(int i)
	{
		if (devLegends[i].Contains("<EXTRACOL"))
		{
			for (int j = -1; j < 99; j++)
			{
				if (devLegends[i].Contains("<EXTRACOL" + j + ">"))
				{
					return j;
				}
			}
		}
		return -2;
	}
}
