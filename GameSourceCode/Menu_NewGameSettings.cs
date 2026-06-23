using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Menu_NewGameSettings : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private unlockScript unlock_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private forschungSonstiges fS_;

	private publisher publisher_;

	private arbeitsmarkt arbeitsmarkt_;

	private themes themes_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	public int selectedMap;

	private void Start()
	{
		FindScripts();
		Init();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.FindGameObjectWithTag("Main");
		}
		if ((bool)main_)
		{
			if (!mS_)
			{
				mS_ = main_.GetComponent<mainScript>();
			}
			if (!tS_)
			{
				tS_ = main_.GetComponent<textScript>();
			}
			if (!genres_)
			{
				genres_ = main_.GetComponent<genres>();
			}
			if (!unlock_)
			{
				unlock_ = main_.GetComponent<unlockScript>();
			}
			if (!sfx_)
			{
				sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
			}
			if (!guiMain_)
			{
				guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
			}
			if (!eF_)
			{
				eF_ = main_.GetComponent<engineFeatures>();
			}
			if (!gF_)
			{
				gF_ = main_.GetComponent<gameplayFeatures>();
			}
			if (!fS_)
			{
				fS_ = main_.GetComponent<forschungSonstiges>();
			}
			if (!arbeitsmarkt_)
			{
				arbeitsmarkt_ = main_.GetComponent<arbeitsmarkt>();
			}
			if (!publisher_)
			{
				publisher_ = main_.GetComponent<publisher>();
			}
			if (!themes_)
			{
				themes_ = main_.GetComponent<themes>();
			}
			if (!hardware_)
			{
				hardware_ = main_.GetComponent<hardware>();
			}
			if (!hardwareFeatures_)
			{
				hardwareFeatures_ = main_.GetComponent<hardwareFeatures>();
			}
		}
	}

	private void OnEnable()
	{
		FindScripts();
		guiMain_.uiObjects[159].GetComponent<Menu_NewGame>();
		guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>();
	}

	public void Init()
	{
		FindScripts();
		InitDropdowns();
		UpdateMapData();
		UpdateRandomTexts();
		TOGGLE_PlatformPop();
		TOGGLE_RandomReview();
	}

	public void InitDropdowns()
	{
		FindScripts();
		List<string> list = new List<string>();
		list = new List<string>();
		list.Add(tS_.GetText(1770) + " " + tS_.GetText(1772));
		list.Add(tS_.GetText(1771));
		list.Add(tS_.GetText(2012));
		list.Add(tS_.GetText(1773));
		list.Add(tS_.GetText(2079));
		list.Add(tS_.GetText(2250));
		uiObjects[11].GetComponent<Dropdown>().ClearOptions();
		uiObjects[11].GetComponent<Dropdown>().AddOptions(list);
		uiObjects[11].GetComponent<Dropdown>().value = 0;
		if (PlayerPrefs.HasKey("optMap"))
		{
			uiObjects[11].GetComponent<Dropdown>().value = PlayerPrefs.GetInt("optMap");
		}
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_OK()
	{
		FindScripts();
		sfx_.PlaySound(3, force: true);
		Menu_NewGame component = guiMain_.uiObjects[159].GetComponent<Menu_NewGame>();
		Menu_NewGame_Sandbox component2 = guiMain_.uiObjects[425].GetComponent<Menu_NewGame_Sandbox>();
		if (!mS_.multiplayer)
		{
			if (!mS_.settings_sandbox)
			{
				mS_.LoadOffice(mS_.GetMapIDfromDropdown(selectedMap), fromSavegame: false);
				mS_.CreateStartAuto(mS_.GetMapIDfromDropdown(selectedMap));
			}
			else
			{
				mS_.LoadOffice(mS_.GetMapIDfromDropdown(selectedMap), fromSavegame: false);
				mS_.CreateStartAuto(mS_.GetMapIDfromDropdown(selectedMap));
			}
		}
		else
		{
			mS_.LoadOffice(mS_.office, fromSavegame: false);
			mS_.CreateStartAuto(mS_.office);
		}
		mS_.InitNewGame();
		if (!mS_.multiplayer)
		{
			if (!mS_.settings_sandbox)
			{
				mS_.difficulty = component.uiObjects[1].GetComponent<Dropdown>().value;
				if (component.uiObjects[7].GetComponent<Dropdown>().value < 4)
				{
					mS_.anzKonkurrenten = (component.uiObjects[7].GetComponent<Dropdown>().value + 1) * 20;
				}
				else
				{
					mS_.anzKonkurrenten = 99999;
				}
				mS_.settings_randomEvents = component.uiObjects[14].GetComponent<Dropdown>().value;
				mS_.settings_TutorialOff = uiObjects[0].GetComponent<Toggle>().isOn;
				mS_.settings_RandomReviews = uiObjects[2].GetComponent<Toggle>().isOn;
				mS_.settings_plattformEnd = uiObjects[7].GetComponent<Toggle>().isOn;
				mS_.settings_competition = component.uiObjects[15].GetComponent<Dropdown>().value;
				mS_.sandbox_tochterfirmaKonsole = uiObjects[16].GetComponent<Toggle>().isOn;
				mS_.settings_randomPlattformSuit = uiObjects[15].GetComponent<Toggle>().isOn;
				if (uiObjects[15].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformSuit();
				}
				mS_.settings_randomPlattformPop = uiObjects[3].GetComponent<Toggle>().isOn;
				if (uiObjects[3].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformPop();
				}
				mS_.settings_randomGameConcept = uiObjects[4].GetComponent<Toggle>().isOn;
				if (uiObjects[4].GetComponent<Toggle>().isOn)
				{
					for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
					{
						for (int j = 0; j < 8; j++)
						{
							genres_.genres_FOCUS[i, j] = 0;
						}
						for (int k = 0; k < 40; k++)
						{
							int num = Random.Range(0, 8);
							genres_.genres_FOCUS[i, num]++;
							if (genres_.genres_FOCUS[i, num] > 10)
							{
								genres_.genres_FOCUS[i, num]--;
								k--;
							}
						}
						for (int l = 0; l < 3; l++)
						{
							genres_.genres_ALIGN[i, l] = Random.Range(0, 11);
						}
					}
				}
				mS_.settings_randomGenreCombination = uiObjects[5].GetComponent<Toggle>().isOn;
				if (uiObjects[5].GetComponent<Toggle>().isOn)
				{
					for (int m = 0; m < genres_.genres_UNLOCK.Length; m++)
					{
						int num2 = genres_.genres_UNLOCK.Length;
						for (int n = 0; n < num2; n++)
						{
							genres_.genres_COMBINATION[m, n] = false;
						}
						genres_.genres_COMBINATION[m, Random.Range(0, num2)] = true;
						genres_.genres_COMBINATION[m, Random.Range(0, num2)] = true;
						genres_.genres_COMBINATION[m, Random.Range(0, num2)] = true;
						genres_.genres_COMBINATION[m, Random.Range(0, num2)] = true;
						genres_.genres_COMBINATION[m, m] = false;
					}
				}
				mS_.speedSetting = mS_.gameSpeeds[component.uiObjects[3].GetComponent<Dropdown>().value];
				mS_.devTimeSetting = component.uiObjects[13].GetComponent<Dropdown>().value;
				int num3 = component.uiObjects[2].GetComponent<Dropdown>().value + 1976;
				mS_.settings_startjahr = num3;
				UnlockStartjahr(num3);
				for (int num4 = 0; num4 < component.uiObjects[2].GetComponent<Dropdown>().value / 2; num4++)
				{
					arbeitsmarkt_.ArbeitsmarktUpdaten(dontDelete: true);
				}
				InitStartjahr(num3);
				mS_.CreateStartAuftragsspiele();
				UnlockPC();
				publisherScript publisherScript2 = mS_.CreatePlayerPublisher(mS_.myID);
				mS_.myPubS_ = publisherScript2;
				mS_.SetCompanyName(component.uiObjects[0].GetComponent<InputField>().text);
				mS_.SetCompanyLogoID(component.logo);
				mS_.SetCountryID(component.country);
				mS_.SetFanGenreID(component.genre);
				publisherScript2.date_year = mS_.year;
			}
			else
			{
				mS_.difficulty = component2.uiObjects[1].GetComponent<Dropdown>().value;
				if (component2.uiObjects[7].GetComponent<Dropdown>().value < 4)
				{
					mS_.anzKonkurrenten = (component2.uiObjects[7].GetComponent<Dropdown>().value + 1) * 20;
				}
				else
				{
					mS_.anzKonkurrenten = 99999;
				}
				mS_.settings_randomEvents = component2.uiObjects[51].GetComponent<Dropdown>().value;
				mS_.settings_TutorialOff = uiObjects[0].GetComponent<Toggle>().isOn;
				mS_.settings_RandomReviews = uiObjects[2].GetComponent<Toggle>().isOn;
				mS_.settings_plattformEnd = uiObjects[7].GetComponent<Toggle>().isOn;
				mS_.settings_competition = component2.uiObjects[52].GetComponent<Dropdown>().value;
				mS_.sandbox_tochterfirmaKonsole = uiObjects[16].GetComponent<Toggle>().isOn;
				mS_.settings_randomPlattformSuit = uiObjects[15].GetComponent<Toggle>().isOn;
				if (uiObjects[15].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformSuit();
				}
				mS_.settings_randomPlattformPop = uiObjects[3].GetComponent<Toggle>().isOn;
				if (uiObjects[3].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformPop();
				}
				mS_.settings_randomGameConcept = uiObjects[4].GetComponent<Toggle>().isOn;
				if (uiObjects[4].GetComponent<Toggle>().isOn)
				{
					for (int num5 = 0; num5 < genres_.genres_UNLOCK.Length; num5++)
					{
						for (int num6 = 0; num6 < 8; num6++)
						{
							genres_.genres_FOCUS[num5, num6] = 0;
						}
						for (int num7 = 0; num7 < 40; num7++)
						{
							int num8 = Random.Range(0, 8);
							genres_.genres_FOCUS[num5, num8]++;
							if (genres_.genres_FOCUS[num5, num8] > 10)
							{
								genres_.genres_FOCUS[num5, num8]--;
								num7--;
							}
						}
						for (int num9 = 0; num9 < 3; num9++)
						{
							genres_.genres_ALIGN[num5, num9] = Random.Range(0, 11);
						}
					}
				}
				mS_.settings_randomGenreCombination = uiObjects[5].GetComponent<Toggle>().isOn;
				if (uiObjects[5].GetComponent<Toggle>().isOn)
				{
					for (int num10 = 0; num10 < genres_.genres_UNLOCK.Length; num10++)
					{
						int num11 = genres_.genres_UNLOCK.Length;
						for (int num12 = 0; num12 < num11; num12++)
						{
							genres_.genres_COMBINATION[num10, num12] = false;
						}
						genres_.genres_COMBINATION[num10, Random.Range(0, num11)] = true;
						genres_.genres_COMBINATION[num10, Random.Range(0, num11)] = true;
						genres_.genres_COMBINATION[num10, Random.Range(0, num11)] = true;
						genres_.genres_COMBINATION[num10, Random.Range(0, num11)] = true;
						genres_.genres_COMBINATION[num10, num10] = false;
					}
				}
				mS_.speedSetting = mS_.gameSpeeds[component2.uiObjects[3].GetComponent<Dropdown>().value];
				mS_.devTimeSetting = component2.uiObjects[13].GetComponent<Dropdown>().value;
				int num13 = component2.uiObjects[2].GetComponent<Dropdown>().value + 1976;
				mS_.settings_startjahr = num13;
				UnlockStartjahr(num13);
				for (int num14 = 0; num14 < component2.uiObjects[2].GetComponent<Dropdown>().value / 2; num14++)
				{
					arbeitsmarkt_.ArbeitsmarktUpdaten(dontDelete: true);
				}
				if (mS_.sandbox_unlimitedMoney)
				{
					mS_.money = 100000000000L;
				}
				if (!mS_.sandbox_unlimitedMoney)
				{
					switch (component2.uiObjects[24].GetComponent<Dropdown>().value)
					{
					case 1:
						mS_.money = 100000L;
						break;
					case 2:
						mS_.money = 250000L;
						break;
					case 3:
						mS_.money = 500000L;
						break;
					case 4:
						mS_.money = 1000000L;
						break;
					case 5:
						mS_.money = 5000000L;
						break;
					case 6:
						mS_.money = 10000000L;
						break;
					case 7:
						mS_.money = 25000000L;
						break;
					case 8:
						mS_.money = 50000000L;
						break;
					case 9:
						mS_.money = 100000000L;
						break;
					case 10:
						mS_.money = 500000000L;
						break;
					case 11:
						mS_.money = 1000000000L;
						break;
					case 12:
						mS_.money = -100000L;
						break;
					case 13:
						mS_.money = -250000L;
						break;
					case 14:
						mS_.money = -500000L;
						break;
					case 15:
						mS_.money = -1000000L;
						break;
					case 16:
						mS_.money = -5000000L;
						break;
					case 17:
						mS_.money = -10000000L;
						break;
					}
				}
				if (component2.uiObjects[19].GetComponent<Toggle>().isOn)
				{
					fS_.RES_POINTS_LEFT[0] = 0f;
					fS_.RES_POINTS_LEFT[1] = 0f;
					fS_.RES_POINTS_LEFT[2] = 0f;
					fS_.RES_POINTS_LEFT[3] = 0f;
					fS_.RES_POINTS_LEFT[40] = 0f;
					fS_.RES_POINTS_LEFT[28] = 0f;
					fS_.RES_POINTS_LEFT[31] = 0f;
					fS_.RES_POINTS_LEFT[32] = 0f;
					fS_.RES_POINTS_LEFT[29] = 0f;
					fS_.RES_POINTS_LEFT[34] = 0f;
					fS_.RES_POINTS_LEFT[39] = 0f;
					fS_.RES_POINTS_LEFT[30] = 0f;
					fS_.RES_POINTS_LEFT[33] = 0f;
					fS_.RES_POINTS_LEFT[38] = 0f;
					fS_.RES_POINTS_LEFT[35] = 0f;
					fS_.RES_POINTS_LEFT[36] = 0f;
				}
				if (component2.uiObjects[17].GetComponent<Toggle>().isOn)
				{
					unlock_.unlock[26] = true;
					unlock_.unlock[66] = true;
					unlock_.unlock[67] = true;
					unlock_.unlock[28] = true;
					unlock_.unlock[29] = true;
					unlock_.unlock[30] = true;
					unlock_.unlock[25] = true;
				}
				if (component2.uiObjects[16].GetComponent<Toggle>().isOn)
				{
					themes_.ResearchAll();
				}
				if (component2.uiObjects[18].GetComponent<Toggle>().isOn)
				{
					hardware_.ResearchAll();
					hardwareFeatures_.ResearchAll();
				}
				switch (component2.uiObjects[37].GetComponent<Dropdown>().value)
				{
				case 1:
					mS_.AddStudioPoints(31);
					break;
				case 2:
					mS_.AddStudioPoints(151);
					break;
				case 3:
					mS_.AddStudioPoints(301);
					break;
				case 4:
					mS_.AddStudioPoints(601);
					break;
				case 5:
					mS_.AddStudioPoints(801);
					break;
				case 6:
					mS_.AddStudioPoints(1001);
					break;
				case 7:
					mS_.AddStudioPoints(1501);
					break;
				case 8:
					mS_.AddStudioPoints(2001);
					break;
				case 9:
					mS_.AddStudioPoints(3001);
					break;
				case 10:
					mS_.AddStudioPoints(4001);
					break;
				}
				if (component2.uiObjects[34].GetComponent<Toggle>().isOn)
				{
					mS_.auftragsAnsehen = 100f;
				}
				if (mS_.sandbox_allItems)
				{
					unlock_.unlock[60] = true;
					unlock_.unlock[61] = true;
					unlock_.unlock[62] = true;
				}
				if (component2.uiObjects[26].GetComponent<Toggle>().isOn)
				{
					genres_.MaxLevelAll();
					gF_.MaxLevelAll();
					eF_.MaxLevelAll();
					themes_.MaxLevelAll();
					GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
					for (int num15 = 0; num15 < array.Length; num15++)
					{
						if ((bool)array[num15])
						{
							array[num15].GetComponent<platformScript>().erfahrung = 5;
						}
					}
				}
				InitStartjahr(num13);
				if (component2.uiObjects[25].GetComponent<Toggle>().isOn)
				{
					genres_.UnlockAll();
				}
				if (component2.uiObjects[20].GetComponent<Toggle>().isOn)
				{
					genres_.ResearchAll();
				}
				mS_.CreateStartAuftragsspiele();
				UnlockPC();
				publisherScript publisherScript3 = mS_.CreatePlayerPublisher(mS_.myID);
				mS_.myPubS_ = publisherScript3;
				mS_.SetCompanyName(component2.uiObjects[0].GetComponent<InputField>().text);
				mS_.SetCompanyLogoID(component2.logo);
				mS_.SetCountryID(component2.country);
				mS_.SetFanGenreID(component2.genre);
				publisherScript3.date_year = mS_.year;
			}
		}
		else
		{
			mS_.CreateStartAuftragsspiele();
			mS_.year = 1976;
			mS_.month = 1;
			mS_.speedSetting = mS_.gameSpeeds[1];
			mS_.settings_TutorialOff = true;
			guiMain_.uiObjects[201].SetActive(value: false);
			guiMain_.uiObjects[202].SetActive(value: true);
			if (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[56].GetComponent<Dropdown>().value < 4)
			{
				mS_.anzKonkurrenten = (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[56].GetComponent<Dropdown>().value + 1) * 20;
			}
			else
			{
				mS_.anzKonkurrenten = 99999;
			}
			mS_.speedSetting = mS_.gameSpeeds[guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[44].GetComponent<Dropdown>().value];
			mS_.devTimeSetting = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[55].GetComponent<Dropdown>().value;
			mS_.settings_arbeitsgeschwindigkeitAnpassen = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[45].GetComponent<Toggle>().isOn;
			mS_.settings_randomPlattformSuit = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[62].GetComponent<Toggle>().isOn;
			mS_.settings_randomPlattformPop = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[42].GetComponent<Toggle>().isOn;
			mS_.settings_randomGameConcept = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[43].GetComponent<Toggle>().isOn;
			mS_.settings_randomGenreCombination = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[52].GetComponent<Toggle>().isOn;
			mS_.settings_closeNPCs = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[53].GetComponent<Toggle>().isOn;
			if (mS_.mpCalls_.isServer)
			{
				if (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[62].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformSuit();
					for (int num16 = 0; num16 < genres_.genres_UNLOCK.Length; num16++)
					{
						mS_.mpCalls_.SERVER_Send_GenrePlatformSuit(num16, genres_.genres_PLATFORM_SELLS[num16, 0], genres_.genres_PLATFORM_SELLS[num16, 1], genres_.genres_PLATFORM_SELLS[num16, 2], genres_.genres_PLATFORM_SELLS[num16, 3], genres_.genres_PLATFORM_SELLS[num16, 4]);
					}
				}
				if (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[42].GetComponent<Toggle>().isOn)
				{
					SetRandomPlattformPop();
				}
				if (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[43].GetComponent<Toggle>().isOn)
				{
					for (int num17 = 0; num17 < genres_.genres_UNLOCK.Length; num17++)
					{
						for (int num18 = 0; num18 < 8; num18++)
						{
							genres_.genres_FOCUS[num17, num18] = 0;
						}
						for (int num19 = 0; num19 < 40; num19++)
						{
							int num20 = Random.Range(0, 8);
							genres_.genres_FOCUS[num17, num20]++;
							if (genres_.genres_FOCUS[num17, num20] > 10)
							{
								genres_.genres_FOCUS[num17, num20]--;
								num19--;
							}
						}
						for (int num21 = 0; num21 < 3; num21++)
						{
							genres_.genres_ALIGN[num17, num21] = Random.Range(0, 11);
						}
						mS_.mpCalls_.SERVER_Send_GenreDesign(num17, Mathf.RoundToInt(genres_.genres_FOCUS[num17, 0]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 1]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 2]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 3]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 4]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 5]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 6]), Mathf.RoundToInt(genres_.genres_FOCUS[num17, 7]), Mathf.RoundToInt(genres_.genres_ALIGN[num17, 0]), Mathf.RoundToInt(genres_.genres_ALIGN[num17, 1]), Mathf.RoundToInt(genres_.genres_ALIGN[num17, 2]));
					}
				}
				if (guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[52].GetComponent<Toggle>().isOn)
				{
					for (int num22 = 0; num22 < genres_.genres_UNLOCK.Length; num22++)
					{
						int num23 = genres_.genres_UNLOCK.Length;
						for (int num24 = 0; num24 < num23; num24++)
						{
							genres_.genres_COMBINATION[num22, num24] = false;
						}
						genres_.genres_COMBINATION[num22, Random.Range(0, num23)] = true;
						genres_.genres_COMBINATION[num22, Random.Range(0, num23)] = true;
						genres_.genres_COMBINATION[num22, Random.Range(0, num23)] = true;
						genres_.genres_COMBINATION[num22, Random.Range(0, num23)] = true;
						genres_.genres_COMBINATION[num22, num22] = false;
						mS_.mpCalls_.SERVER_Send_GenreCombination(num22);
					}
				}
			}
			int num25 = guiMain_.uiObjects[201].GetComponent<mpMain>().uiObjects[32].GetComponent<Dropdown>().value + 1976;
			mS_.settings_startjahr = num25;
			UnlockStartjahr(num25);
			InitStartjahr(num25);
			if (mS_.mpCalls_.isServer)
			{
				SendAllPlatforms();
				SendAllPublisher();
				SendAllCopyProtect();
				SendAllAntiCheat();
				mS_.mpCalls_.SERVER_Send_Hardware();
				mS_.mpCalls_.SERVER_Send_HardwareFeatures();
				mS_.mpCalls_.SERVER_Send_EngineFeatures();
				mS_.mpCalls_.SERVER_Send_GameplayFeatures();
				mS_.mpCalls_.SERVER_Send_Topics();
				SendAllnpcEngines();
			}
		}
		guiMain_.UpdateOnce();
		Object.Destroy(GameObject.Find("CHARNEWGAME"));
		mS_.DestroyMainMenuObjects();
		guiMain_.uiObjects[151].SetActive(value: false);
		guiMain_.uiObjects[159].SetActive(value: false);
		guiMain_.uiObjects[162].SetActive(value: false);
		guiMain_.uiObjects[425].SetActive(value: false);
		guiMain_.OpenMenu(hideChars: false);
		string text = tS_.GetText(2396);
		text = text.Replace("<NAME>", "<color=blue>" + mS_.GetCompanyName() + "</color>");
		guiMain_.MessageBox(text, closeMenu: true);
		base.gameObject.SetActive(value: false);
	}

	private void InitStartjahr(int startjahr)
	{
		if (startjahr == 1976)
		{
			return;
		}
		int num = 0;
		while (num < 10000)
		{
			num++;
			unlock_.CheckUnlock(showMessage: false);
			mS_.month++;
			for (int i = 0; i < 4; i++)
			{
				mS_.platforms_.UpdatePlatformSells(sendDataToClient: false, forceSendAll: false);
				mS_.copyProtect_.UpdateEffekt();
				mS_.antiCheat_.UpdateEffekt();
			}
			if (mS_.month == 13)
			{
				mS_.month = 1;
				mS_.year++;
				mS_.ResetJahresbilanz();
			}
			if (mS_.year == startjahr)
			{
				unlock_.CheckUnlock(showMessage: false);
				break;
			}
		}
		for (int j = 0; j < gF_.gameplayFeatures_UNLOCK.Length; j++)
		{
			if (gF_.gameplayFeatures_UNLOCK[j])
			{
				gF_.gameplayFeatures_RES_POINTS_LEFT[j] = 0f;
			}
		}
		for (int k = 0; k < eF_.engineFeatures_UNLOCK.Length; k++)
		{
			if (eF_.engineFeatures_UNLOCK[k])
			{
				eF_.engineFeatures_RES_POINTS_LEFT[k] = 0f;
			}
		}
	}

	private void SetFirmenwert(int startjahr)
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if ((bool)component)
				{
					component.firmenwert += component.firmenwert * (startjahr - 1976);
				}
			}
		}
	}

	private void SetRandomPlattformSuit()
	{
		for (int i = 0; i < genres_.genres_LEVEL.Length; i++)
		{
			for (int j = 0; j < 5; j++)
			{
				genres_.genres_PLATFORM_SELLS[i, j] = Random.Range(3, 11) * 10;
			}
			genres_.genres_PLATFORM_SELLS[i, Random.Range(0, 5)] = 100;
			if (Random.Range(0, 100) > 50)
			{
				genres_.genres_PLATFORM_SELLS[i, 0] = 100;
			}
			else
			{
				genres_.genres_PLATFORM_SELLS[i, 1] = 100;
			}
		}
	}

	private void SetRandomPlattformPop()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i])
			{
				continue;
			}
			platformScript component = array[i].GetComponent<platformScript>();
			if (component.typ == 4)
			{
				continue;
			}
			if (mS_.settings_randomPlattformNum == 4)
			{
				int num = Random.Range(0, 4);
				if (component.date_year_end >= 999999)
				{
					num = 3;
				}
				switch (num)
				{
				case 0:
					component.units_max = Random.Range(1000, 100000);
					if (component.date_year_end < 999999)
					{
						component.date_year_end = Random.Range(component.date_year + 2, component.date_year + 4);
					}
					break;
				case 1:
					component.units_max = Random.Range(1000000, 10000000);
					if (component.date_year_end < 999999)
					{
						component.date_year_end = Random.Range(component.date_year + 4, component.date_year + 6);
					}
					break;
				case 2:
					component.units_max = Random.Range(20000000, 30000000);
					if (component.date_year_end < 999999)
					{
						component.date_year_end = Random.Range(component.date_year + 6, component.date_year + 8);
					}
					break;
				case 3:
					component.units_max = Random.Range(60000000, 100000000);
					if (component.date_year_end < 999999)
					{
						component.date_year_end = Random.Range(component.date_year + 8, component.date_year + 10);
					}
					break;
				}
				continue;
			}
			float num2 = 0f;
			switch (mS_.settings_randomPlattformNum)
			{
			case 1:
				num2 = component.units_max;
				num2 *= 0.25f;
				if (component.date_year_end < 999999)
				{
					component.date_year_end += Random.Range(0, 2);
				}
				break;
			case 2:
				num2 = component.units_max;
				num2 *= 0.5f;
				if (component.date_year_end < 999999)
				{
					component.date_year_end += Random.Range(0, 3);
				}
				break;
			case 3:
				num2 = component.units_max;
				num2 *= 0.75f;
				if (component.date_year_end < 999999)
				{
					component.date_year_end += Random.Range(0, 4);
				}
				break;
			}
			if (component.date_year_end >= 999999)
			{
				component.units_max += Random.Range(Mathf.RoundToInt(0f - num2), Mathf.RoundToInt(num2));
			}
			else
			{
				component.units_max += Random.Range(0, Mathf.RoundToInt(num2));
			}
		}
	}

	private void SendAllPlatforms()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component)
				{
					mS_.mpCalls_.SERVER_Send_Platform(component);
				}
			}
		}
	}

	private void SendAllPublisher()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				publisherScript component = array[i].GetComponent<publisherScript>();
				if ((bool)component)
				{
					mS_.mpCalls_.SERVER_Send_Publisher(component);
				}
			}
		}
		for (int j = 0; j < array.Length; j++)
		{
			if ((bool)array[j])
			{
				publisherScript component2 = array[j].GetComponent<publisherScript>();
				if ((bool)component2 && component2.tf_geschlossen)
				{
					mS_.mpCalls_.SERVER_Send_PublisherClose(component2);
				}
			}
		}
	}

	private void SendAllCopyProtect()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				copyProtectScript component = array[i].GetComponent<copyProtectScript>();
				if ((bool)component)
				{
					mS_.mpCalls_.SERVER_Send_CopyProtect(component);
				}
			}
		}
	}

	private void SendAllAntiCheat()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("AntiCheat");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				antiCheatScript component = array[i].GetComponent<antiCheatScript>();
				if ((bool)component)
				{
					mS_.mpCalls_.SERVER_Send_AntiCheat(component);
				}
			}
		}
	}

	private void SendAllnpcEngines()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Engine");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				engineScript component = array[i].GetComponent<engineScript>();
				if ((bool)component)
				{
					mS_.mpCalls_.SERVER_Send_NpcEngine(component);
				}
			}
		}
	}

	private void UnlockPC()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Platform");
		for (int i = 0; i < array.Length; i++)
		{
			if ((bool)array[i])
			{
				platformScript component = array[i].GetComponent<platformScript>();
				if ((bool)component && component.typ == 0 && component.isUnlocked && component.IsVerfuegbar())
				{
					component.inBesitz = true;
					break;
				}
			}
		}
	}

	public void TOGGLE_RandomEvents()
	{
		if (uiObjects[1].GetComponent<Toggle>().isOn)
		{
			uiObjects[6].GetComponent<Toggle>().interactable = false;
			uiObjects[6].GetComponent<Toggle>().isOn = false;
		}
		else
		{
			uiObjects[6].GetComponent<Toggle>().interactable = true;
		}
	}

	private void UnlockStartjahr(int startjahr_)
	{
		if (startjahr_ >= 1985 && startjahr_ < 1995)
		{
			mS_.money = 2000000L;
			fS_.RES_POINTS_LEFT[0] = 0f;
			fS_.RES_POINTS_LEFT[28] = 0f;
			fS_.RES_POINTS_LEFT[35] = 0f;
			fS_.RES_POINTS_LEFT[36] = 0f;
		}
		if (startjahr_ >= 1995 && startjahr_ < 2005)
		{
			mS_.money = 4000000L;
			fS_.RES_POINTS_LEFT[0] = 0f;
			fS_.RES_POINTS_LEFT[1] = 0f;
			fS_.RES_POINTS_LEFT[28] = 0f;
			fS_.RES_POINTS_LEFT[35] = 0f;
			fS_.RES_POINTS_LEFT[36] = 0f;
			unlock_.unlock[28] = true;
		}
		if (startjahr_ >= 2005 && startjahr_ < 2015)
		{
			mS_.money = 8000000L;
			fS_.RES_POINTS_LEFT[0] = 0f;
			fS_.RES_POINTS_LEFT[1] = 0f;
			fS_.RES_POINTS_LEFT[2] = 0f;
			fS_.RES_POINTS_LEFT[28] = 0f;
			fS_.RES_POINTS_LEFT[31] = 0f;
			fS_.RES_POINTS_LEFT[35] = 0f;
			fS_.RES_POINTS_LEFT[36] = 0f;
			unlock_.unlock[28] = true;
			unlock_.unlock[29] = true;
		}
		if (startjahr_ >= 2015 && startjahr_ < 2020)
		{
			mS_.money = 12000000L;
			fS_.RES_POINTS_LEFT[0] = 0f;
			fS_.RES_POINTS_LEFT[1] = 0f;
			fS_.RES_POINTS_LEFT[2] = 0f;
			fS_.RES_POINTS_LEFT[3] = 0f;
			fS_.RES_POINTS_LEFT[28] = 0f;
			fS_.RES_POINTS_LEFT[31] = 0f;
			fS_.RES_POINTS_LEFT[32] = 0f;
			fS_.RES_POINTS_LEFT[30] = 0f;
			fS_.RES_POINTS_LEFT[35] = 0f;
			fS_.RES_POINTS_LEFT[36] = 0f;
			unlock_.unlock[28] = true;
			unlock_.unlock[29] = true;
			unlock_.unlock[30] = true;
		}
		if (startjahr_ >= 2020)
		{
			mS_.money = 25000000L;
			fS_.RES_POINTS_LEFT[0] = 0f;
			fS_.RES_POINTS_LEFT[1] = 0f;
			fS_.RES_POINTS_LEFT[2] = 0f;
			fS_.RES_POINTS_LEFT[3] = 0f;
			fS_.RES_POINTS_LEFT[40] = 0f;
			fS_.RES_POINTS_LEFT[28] = 0f;
			fS_.RES_POINTS_LEFT[31] = 0f;
			fS_.RES_POINTS_LEFT[32] = 0f;
			fS_.RES_POINTS_LEFT[30] = 0f;
			fS_.RES_POINTS_LEFT[29] = 0f;
			fS_.RES_POINTS_LEFT[34] = 0f;
			fS_.RES_POINTS_LEFT[35] = 0f;
			fS_.RES_POINTS_LEFT[36] = 0f;
			unlock_.unlock[28] = true;
			unlock_.unlock[29] = true;
			unlock_.unlock[30] = true;
		}
	}

	public void BUTTON_ChangeMap(int i)
	{
		sfx_.PlaySound(3, force: true);
		selectedMap += i;
		if (selectedMap > 5)
		{
			selectedMap = 0;
		}
		if (selectedMap < 0)
		{
			selectedMap = 5;
		}
		uiObjects[11].GetComponent<Dropdown>().value = selectedMap;
		UpdateMapData();
	}

	public void BUTTON_Minus_PlatformPop()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_randomPlattformNum--;
		if (mS_.settings_randomPlattformNum < 1)
		{
			mS_.settings_randomPlattformNum = 1;
		}
		UpdateRandomTexts();
	}

	public void BUTTON_Plus_PlatformPop()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_randomPlattformNum++;
		if (mS_.settings_randomPlattformNum > 4)
		{
			mS_.settings_randomPlattformNum = 4;
		}
		UpdateRandomTexts();
	}

	public void BUTTON_Minus_RandomReview()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_RandomReviewsNum--;
		if (mS_.settings_RandomReviewsNum < 1)
		{
			mS_.settings_RandomReviewsNum = 1;
		}
		UpdateRandomTexts();
	}

	public void BUTTON_Plus_RandomReview()
	{
		sfx_.PlaySound(3, force: true);
		mS_.settings_RandomReviewsNum++;
		if (mS_.settings_RandomReviewsNum > 4)
		{
			mS_.settings_RandomReviewsNum = 4;
		}
		UpdateRandomTexts();
	}

	public void DROPDOWN_Map()
	{
		selectedMap = uiObjects[11].GetComponent<Dropdown>().value;
		UpdateMapData();
	}

	private void UpdateRandomTexts()
	{
		uiObjects[17].GetComponent<Text>().text = "+/- " + mS_.settings_RandomReviewsNum * 3 + "%";
		switch (mS_.settings_randomPlattformNum)
		{
		case 1:
			uiObjects[18].GetComponent<Text>().text = tS_.GetText(1908);
			break;
		case 2:
			uiObjects[18].GetComponent<Text>().text = tS_.GetText(1909);
			break;
		case 3:
			uiObjects[18].GetComponent<Text>().text = tS_.GetText(1910);
			break;
		case 4:
			uiObjects[18].GetComponent<Text>().text = tS_.GetText(2436);
			break;
		}
		if (!uiObjects[2].GetComponent<Toggle>().isOn)
		{
			uiObjects[17].GetComponent<Text>().text = "---";
		}
		if (!uiObjects[3].GetComponent<Toggle>().isOn)
		{
			uiObjects[18].GetComponent<Text>().text = "---";
		}
	}

	public void TOGGLE_RandomReview()
	{
		if (uiObjects[2].GetComponent<Toggle>().isOn)
		{
			uiObjects[19].GetComponent<Button>().interactable = true;
			uiObjects[20].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[19].GetComponent<Button>().interactable = false;
			uiObjects[20].GetComponent<Button>().interactable = false;
		}
		UpdateRandomTexts();
	}

	public void TOGGLE_PlatformPop()
	{
		if (uiObjects[3].GetComponent<Toggle>().isOn)
		{
			uiObjects[21].GetComponent<Button>().interactable = true;
			uiObjects[22].GetComponent<Button>().interactable = true;
		}
		else
		{
			uiObjects[21].GetComponent<Button>().interactable = false;
			uiObjects[22].GetComponent<Button>().interactable = false;
		}
		UpdateRandomTexts();
	}

	private void UpdateMapData()
	{
		uiObjects[9].GetComponent<Animation>().Play();
		uiObjects[9].GetComponent<Image>().sprite = guiMain_.iconMaps[selectedMap];
		string text = "";
		switch (selectedMap)
		{
		case 0:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "7");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "2312 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 3, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2258);
			break;
		case 1:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "4");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "2564 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 4, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2259);
			break;
		case 2:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "6");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "1356 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 5, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2260);
			break;
		case 3:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "1");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "3850 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 1, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2261);
			break;
		case 4:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "9");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "3831 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 3, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2262);
			break;
		case 5:
			text = tS_.GetText(2256);
			text = text.Replace("<NUM>", "9");
			uiObjects[12].GetComponent<Text>().text = text;
			uiObjects[13].GetComponent<Text>().text = "2363 " + tS_.GetText(72);
			guiMain_.DrawStarsColor(uiObjects[10], 2, Color.white);
			uiObjects[14].GetComponent<Text>().text = tS_.GetText(2263);
			break;
		}
		if (selectedMap != 0)
		{
			uiObjects[0].GetComponent<Toggle>().isOn = true;
			uiObjects[0].GetComponent<Toggle>().interactable = false;
		}
		else
		{
			uiObjects[0].GetComponent<Toggle>().isOn = false;
			uiObjects[0].GetComponent<Toggle>().interactable = true;
		}
	}
}
