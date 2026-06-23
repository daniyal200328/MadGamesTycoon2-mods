using UnityEngine;

public class unlockScript : MonoBehaviour
{
	private mainScript mS_;

	private genres genres_;

	private engineFeatures eF_;

	private gameplayFeatures gF_;

	private hardware hardware_;

	private hardwareFeatures hardwareFeatures_;

	private games games_;

	private themes themes_;

	private textScript tS_;

	private roomDataScript rdS_;

	private GUI_Main guiMain_;

	public bool[] unlock;

	public const int roomMotion = 8;

	public const int roomServer = 9;

	public const int gameTypClassic = 20;

	public const int gameTypMMO = 21;

	public const int gameTypF2P = 22;

	public const int gameLicence = 25;

	public const int gameNachfolger = 26;

	public const int gameRemaster = 27;

	public const int gamePlatform2 = 28;

	public const int gamePlatform3 = 29;

	public const int gamePlatform4 = 30;

	public const int gameCopyProtect = 31;

	public const int marketingInternet = 56;

	public const int marketingStreamer = 57;

	public const int createEngine = 58;

	public const int onlineSells = 59;

	public const int inventar2 = 60;

	public const int inventar3 = 61;

	public const int inventar4 = 62;

	public const int inventar5 = 63;

	public const int gameAntiCheat = 64;

	public const int handyGames = 65;

	public const int gameSpinoff = 66;

	public const int gamePorts = 67;

	public const int gamePass = 68;

	public const int ipHandel = 69;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!mS_)
		{
			mS_ = GetComponent<mainScript>();
		}
		if (!genres_)
		{
			genres_ = GetComponent<genres>();
		}
		if (!eF_)
		{
			eF_ = GetComponent<engineFeatures>();
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
		if (!tS_)
		{
			tS_ = GetComponent<textScript>();
		}
		if (!themes_)
		{
			themes_ = GetComponent<themes>();
		}
		if (!rdS_)
		{
			rdS_ = GetComponent<roomDataScript>();
		}
		if (!guiMain_)
		{
			guiMain_ = GameObject.Find("CanvasInGameMenu").GetComponent<GUI_Main>();
		}
	}

	public void NewGameUnlocks()
	{
		for (int i = 0; i < unlock.Length; i++)
		{
			unlock[i] = false;
		}
		unlock[58] = true;
	}

	public bool Get(int i)
	{
		return unlock[i];
	}

	public void CheckUnlock(bool showMessage)
	{
		FindScripts();
		for (int i = 0; i < genres_.genres_UNLOCK.Length; i++)
		{
			if (!genres_.genres_UNLOCK[i] && genres_.genres_DATE_YEAR[i] == mS_.year && genres_.genres_DATE_MONTH[i] == mS_.month)
			{
				genres_.genres_UNLOCK[i] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsForschung(genres_.GetName(i), genres_.GetPic(i));
				}
			}
		}
		int num = 0;
		for (int j = 0; j < eF_.engineFeatures_UNLOCK.Length; j++)
		{
			if (!eF_.engineFeatures_UNLOCK[j] && eF_.engineFeatures_DATE_YEAR[j] == mS_.year && eF_.engineFeatures_DATE_MONTH[j] == mS_.month)
			{
				eF_.engineFeatures_UNLOCK[j] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsForschung(eF_.GetName(j), eF_.GetTypPic(j));
				}
			}
			if (Get(58))
			{
				continue;
			}
			if (eF_.engineFeatures_UNLOCK[j] && eF_.engineFeatures_RES_POINTS_LEFT[j] <= 0f)
			{
				num++;
			}
			if (num >= 5)
			{
				unlock[58] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsUnlock(tS_.GetText(240), guiMain_.uiSprites[4]);
					guiMain_.UnlockBox(tS_.GetText(878), closeMenu: true);
				}
			}
		}
		for (int k = 0; k < gF_.gameplayFeatures_UNLOCK.Length; k++)
		{
			if (!gF_.gameplayFeatures_UNLOCK[k] && gF_.gameplayFeatures_DATE_YEAR[k] == mS_.year && gF_.gameplayFeatures_DATE_MONTH[k] == mS_.month)
			{
				gF_.gameplayFeatures_UNLOCK[k] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsForschung(gF_.GetName(k), gF_.GetTypSprite(k));
				}
			}
		}
		for (int l = 0; l < hardware_.hardware_UNLOCK.Length; l++)
		{
			if (!hardware_.hardware_UNLOCK[l] && hardware_.hardware_DATE_YEAR[l] == mS_.year && hardware_.hardware_DATE_MONTH[l] == mS_.month)
			{
				hardware_.hardware_UNLOCK[l] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsForschung(hardware_.GetName(l), hardware_.GetTypPic(l));
				}
			}
		}
		for (int m = 0; m < hardwareFeatures_.hardFeat_UNLOCK.Length; m++)
		{
			if (!hardwareFeatures_.hardFeat_UNLOCK[m] && hardwareFeatures_.hardFeat_DATE_YEAR[m] == mS_.year && hardwareFeatures_.hardFeat_DATE_MONTH[m] == mS_.month)
			{
				hardwareFeatures_.hardFeat_UNLOCK[m] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsForschung(hardwareFeatures_.GetName(m), hardwareFeatures_.GetSprite(m));
				}
			}
		}
		for (int n = 0; n < mS_.arrayPlatformsScripts.Length; n++)
		{
			if (!mS_.arrayPlatformsScripts[n] || !mS_.arrayPlatformsScripts[n].OwnerIsNPC())
			{
				continue;
			}
			if (!mS_.arrayPlatformsScripts[n].isUnlocked && !mS_.arrayPlatformsScripts[n].angekuendigt && mS_.year == mS_.arrayPlatformsScripts[n].date_year - 1 && mS_.arrayPlatformsScripts[n].date_month == mS_.month)
			{
				bool flag = false;
				if (!mS_.settings_sandbox && mS_.arrayPlatformsScripts[n].WurdeHerstellerAufgekauft())
				{
					flag = true;
				}
				if (mS_.settings_sandbox && !mS_.sandbox_tochterfirmaKonsole && mS_.arrayPlatformsScripts[n].WurdeHerstellerAufgekauft())
				{
					flag = true;
				}
				if (mS_.settings_sandbox && mS_.sandbox_tochterfirmaKonsole)
				{
					flag = false;
				}
				if (!flag)
				{
					mS_.arrayPlatformsScripts[n].angekuendigt = true;
					if (showMessage)
					{
						guiMain_.CreateTopNewsPlatformAnkuendigung(mS_.arrayPlatformsScripts[n]);
					}
				}
			}
			if (!mS_.arrayPlatformsScripts[n].isUnlocked)
			{
				if (mS_.arrayPlatformsScripts[n].date_year == mS_.year && mS_.arrayPlatformsScripts[n].date_month == mS_.month)
				{
					bool flag2 = false;
					if (!mS_.settings_sandbox && mS_.arrayPlatformsScripts[n].WurdeHerstellerAufgekauft())
					{
						flag2 = true;
					}
					if (mS_.settings_sandbox && !mS_.sandbox_tochterfirmaKonsole && mS_.arrayPlatformsScripts[n].WurdeHerstellerAufgekauft())
					{
						flag2 = true;
					}
					if (mS_.settings_sandbox && mS_.sandbox_tochterfirmaKonsole)
					{
						flag2 = false;
					}
					if (!flag2)
					{
						mS_.arrayPlatformsScripts[n].isUnlocked = true;
						if (showMessage)
						{
							guiMain_.CreateTopNewsPlatform(mS_.arrayPlatformsScripts[n]);
						}
					}
				}
			}
			else if (!mS_.settings_plattformEnd)
			{
				if (!mS_.arrayPlatformsScripts[n].vomMarktGenommen && mS_.arrayPlatformsScripts[n].date_year_end == mS_.year && mS_.arrayPlatformsScripts[n].date_month_end == mS_.month)
				{
					mS_.arrayPlatformsScripts[n].vomMarktGenommen = true;
					if (showMessage)
					{
						guiMain_.CreateTopNewsPlatformRemove(mS_.arrayPlatformsScripts[n]);
					}
				}
			}
			else if (!mS_.arrayPlatformsScripts[n].vomMarktGenommen && mS_.arrayPlatformsScripts[n].GetPlattformEnd())
			{
				mS_.arrayPlatformsScripts[n].vomMarktGenommen = true;
				mS_.arrayPlatformsScripts[n].date_year_end = mS_.year;
				mS_.arrayPlatformsScripts[n].date_month_end = mS_.month;
				if (showMessage)
				{
					if (mS_.multiplayer && (bool)mS_.mpCalls_ && mS_.mpCalls_.isServer)
					{
						mS_.mpCalls_.SERVER_Send_PlatformRemoveFromMarket(mS_.arrayPlatformsScripts[n]);
					}
					guiMain_.CreateTopNewsPlatformRemove(mS_.arrayPlatformsScripts[n]);
				}
			}
			if (!Get(65) && mS_.arrayPlatformsScripts[n].isUnlocked && mS_.arrayPlatformsScripts[n].typ == 3)
			{
				unlock[65] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsUnlock(tS_.GetText(1060), guiMain_.uiSprites[40]);
					guiMain_.UnlockBox(tS_.GetText(1506), closeMenu: true);
					return;
				}
			}
		}
		for (int num2 = 0; num2 < mS_.arrayEnginesScripts.Length; num2++)
		{
			if ((bool)mS_.arrayEnginesScripts[num2] && !mS_.arrayEnginesScripts[num2].isUnlocked && mS_.arrayEnginesScripts[num2].ownerID != mS_.myID && mS_.arrayEnginesScripts[num2].date_year == mS_.year && mS_.arrayEnginesScripts[num2].date_month == mS_.month)
			{
				mS_.arrayEnginesScripts[num2].isUnlocked = true;
				mS_.arrayEnginesScripts[num2].InitNpcEngine();
				if (showMessage)
				{
					guiMain_.CreateTopNewsNpcEngine(mS_.arrayEnginesScripts[num2].GetName());
				}
			}
		}
		GameObject[] array = GameObject.FindGameObjectsWithTag("CopyProtect");
		for (int num3 = 0; num3 < array.Length; num3++)
		{
			if (!array[num3])
			{
				continue;
			}
			copyProtectScript component = array[num3].GetComponent<copyProtectScript>();
			if ((bool)component && !component.isUnlocked && component.date_year == mS_.year && component.date_month == mS_.month)
			{
				component.isUnlocked = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsCopyProtect(component.GetName());
				}
				if (!Get(31))
				{
					unlock[31] = true;
					guiMain_.UnlockBox(tS_.GetText(895), closeMenu: true);
				}
			}
		}
		array = GameObject.FindGameObjectsWithTag("AntiCheat");
		for (int num4 = 0; num4 < array.Length; num4++)
		{
			if (!array[num4])
			{
				continue;
			}
			antiCheatScript component2 = array[num4].GetComponent<antiCheatScript>();
			if ((bool)component2 && !component2.isUnlocked && component2.date_year == mS_.year && component2.date_month == mS_.month)
			{
				component2.isUnlocked = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsAntiCheat(component2.GetName());
				}
				if (!Get(64))
				{
					unlock[64] = true;
					guiMain_.UnlockBox(tS_.GetText(1207), closeMenu: true);
				}
			}
		}
		int num5 = 0;
		if ((bool)mS_)
		{
			for (int num6 = 0; num6 < mS_.arrayPublisherScripts.Length; num6++)
			{
				if ((bool)mS_.arrayPublisherScripts[num6] && mS_.arrayPublisherScripts[num6].isUnlocked && !mS_.arrayPublisherScripts[num6].isPlayer)
				{
					num5++;
				}
			}
		}
		if (num5 < mS_.anzKonkurrenten)
		{
			for (int num7 = 0; num7 < mS_.arrayPublisherScripts.Length; num7++)
			{
				if ((bool)mS_.arrayPublisherScripts[num7] && !mS_.arrayPublisherScripts[num7].isUnlocked && mS_.arrayPublisherScripts[num7].date_year == mS_.year && mS_.arrayPublisherScripts[num7].date_month == mS_.month && num5 < mS_.anzKonkurrenten)
				{
					num5++;
					mS_.arrayPublisherScripts[num7].Unlock();
					if (showMessage && mS_.arrayPublisherScripts[num7].isUnlocked)
					{
						guiMain_.CreateTopNewsNewPublisher(mS_.arrayPublisherScripts[num7].GetName(), mS_.arrayPublisherScripts[num7].GetLogo());
					}
				}
			}
		}
		if ((!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer)) && mS_.settings_closeNPCs)
		{
			for (int num8 = 0; num8 < mS_.arrayPublisherScripts.Length; num8++)
			{
				if (mS_.arrayPublisherScripts[num8].date_year_end != 0 && mS_.arrayPublisherScripts[num8].isUnlocked && !mS_.arrayPublisherScripts[num8].Geschlossen() && HasPassedDate(mS_.arrayPublisherScripts[num8].date_year_end, mS_.arrayPublisherScripts[num8].date_month_end) && !mS_.arrayPublisherScripts[num8].IsTochterfirma() && mS_.arrayPublisherScripts[num8].CloseFree())
				{
					mS_.arrayPublisherScripts[num8].CloseNpcStudio();
					if (showMessage)
					{
						guiMain_.CreateTopNewsRemovePublisher(mS_.arrayPublisherScripts[num8].GetName(), mS_.arrayPublisherScripts[num8].GetLogo());
					}
				}
			}
		}
		if (!Get(60) && mS_.year == 1984 && mS_.month == 12 && mS_.week == 5)
		{
			unlock[60] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1200), guiMain_.uiSprites[29]);
				guiMain_.UnlockBox(tS_.GetText(1196), closeMenu: true);
				return;
			}
		}
		if (!Get(61) && mS_.year == 1994 && mS_.month == 12 && mS_.week == 5)
		{
			unlock[61] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1201), guiMain_.uiSprites[29]);
				guiMain_.UnlockBox(tS_.GetText(1197), closeMenu: true);
				return;
			}
		}
		if (!Get(62) && mS_.year == 2004 && mS_.month == 12 && mS_.week == 5)
		{
			unlock[62] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1202), guiMain_.uiSprites[29]);
				guiMain_.UnlockBox(tS_.GetText(1198), closeMenu: true);
				return;
			}
		}
		if (!Get(63) && mS_.year == 2014 && mS_.month == 12 && mS_.week == 5)
		{
			unlock[63] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1203), guiMain_.uiSprites[29]);
				guiMain_.UnlockBox(tS_.GetText(1199), closeMenu: true);
				return;
			}
		}
		if (!Get(27) && HasPassedDate(1986, 4))
		{
			unlock[27] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(320), games_.gameTypSprites[2]);
				guiMain_.UnlockBox(tS_.GetText(881), closeMenu: true);
				return;
			}
		}
		if (!Get(56) && HasPassedDate(1993, 3))
		{
			unlock[56] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(521), guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>().sprites[3]);
				guiMain_.UnlockBox(tS_.GetText(880), closeMenu: true);
				return;
			}
		}
		if (!Get(8) && HasPassedDate(1993, 11))
		{
			unlock[8] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(28), rdS_.roomData_SPRITE[10]);
				guiMain_.UnlockBox(tS_.GetText(885), closeMenu: true);
				return;
			}
		}
		if (!Get(59) && HasPassedDate(2003, 2))
		{
			unlock[59] = true;
			unlock[22] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1117), games_.gameTypSprites[7]);
				guiMain_.UnlockBox(tS_.GetText(1118), closeMenu: true);
				return;
			}
		}
		if (!Get(68) && HasPassedDate(2004, 6))
		{
			unlock[68] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(1243), games_.gameTypSprites[7]);
				guiMain_.UnlockBox(tS_.GetText(2124), closeMenu: true);
				return;
			}
		}
		if (!Get(69) && HasPassedDate(1986, 2))
		{
			unlock[69] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(2236), guiMain_.uiSprites[64]);
				guiMain_.UnlockBox(tS_.GetText(2244), closeMenu: true);
				return;
			}
		}
		if (!Get(57) && HasPassedDate(2005, 2))
		{
			unlock[57] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(522), guiMain_.uiObjects[89].GetComponent<Menu_Marketing_GameKampagne>().sprites[4]);
				guiMain_.UnlockBox(tS_.GetText(879), closeMenu: true);
				return;
			}
		}
		if (!Get(21) && gF_.gameplayFeatures_RES_POINTS_LEFT.Length != 0 && gF_.gameplayFeatures_RES_POINTS_LEFT[23] == 0f)
		{
			unlock[21] = true;
			unlock[9] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(323), gF_.GetTypSprite(23));
				guiMain_.UnlockBox(tS_.GetText(882), closeMenu: true);
				return;
			}
		}
		if (!Get(25) && mS_.money - mS_.kredit >= 1000000)
		{
			unlock[25] = true;
			if (showMessage)
			{
				guiMain_.CreateTopNewsUnlock(tS_.GetText(570), guiMain_.uiSprites[7]);
				guiMain_.UnlockBox(tS_.GetText(884), closeMenu: true);
				return;
			}
		}
		if (Get(26) && Get(28) && Get(29) && Get(30) && Get(66) && Get(67))
		{
			return;
		}
		array = GameObject.FindGameObjectsWithTag("Game");
		int num9 = 0;
		if (array.Length == 0)
		{
			return;
		}
		for (int num10 = 0; num10 < array.Length; num10++)
		{
			if (!array[num10])
			{
				continue;
			}
			gameScript component3 = array[num10].GetComponent<gameScript>();
			if (!component3 || component3.inDevelopment || component3.schublade || component3.pubAngebot || component3.auftragsspiel)
			{
				continue;
			}
			if (component3.developerID == mS_.myID || component3.IsMyAuftragsspiel())
			{
				num9++;
				if (num9 >= 5 && !Get(28))
				{
					unlock[28] = true;
					if (showMessage)
					{
						guiMain_.CreateTopNewsUnlock(tS_.GetText(361), guiMain_.uiSprites[17]);
						guiMain_.UnlockBox(tS_.GetText(892), closeMenu: true);
						break;
					}
				}
				if (num9 >= 10 && !Get(29))
				{
					unlock[29] = true;
					if (showMessage)
					{
						guiMain_.CreateTopNewsUnlock(tS_.GetText(362), guiMain_.uiSprites[17]);
						guiMain_.UnlockBox(tS_.GetText(893), closeMenu: true);
						break;
					}
				}
				if (num9 >= 15 && !Get(30))
				{
					unlock[30] = true;
					if (showMessage)
					{
						guiMain_.CreateTopNewsUnlock(tS_.GetText(363), guiMain_.uiSprites[17]);
						guiMain_.UnlockBox(tS_.GetText(894), closeMenu: true);
						break;
					}
				}
			}
			if (!Get(26) && component3.developerID == mS_.myID && component3.reviewTotal >= 70)
			{
				unlock[26] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsUnlock(tS_.GetText(319), games_.gameTypSprites[1]);
					guiMain_.UnlockBox(tS_.GetText(896), closeMenu: true);
					break;
				}
			}
			if (!Get(66) && component3.developerID == mS_.myID && component3.sellsTotal >= 50000)
			{
				unlock[66] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsUnlock(tS_.GetText(1535), games_.gameTypSprites[13]);
					guiMain_.UnlockBox(tS_.GetText(1538), closeMenu: true);
					break;
				}
			}
			if (!Get(67) && component3.developerID == mS_.myID && component3.isOnMarket)
			{
				unlock[67] = true;
				if (showMessage)
				{
					guiMain_.CreateTopNewsUnlock(tS_.GetText(1063), games_.gameTypSprites[14]);
					break;
				}
			}
		}
	}

	private bool HasPassedDate(int year_, int month_)
	{
		if (mS_.year >= year_ && (mS_.month >= month_ || mS_.year > year_))
		{
			return true;
		}
		return false;
	}
}
