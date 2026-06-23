using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Menu_Awards : MonoBehaviour
{
	public GameObject[] uiObjects;

	private GameObject main_;

	private mainScript mS_;

	private textScript tS_;

	private GUI_Main guiMain_;

	private sfxScript sfx_;

	private genres genres_;

	private themes themes_;

	private games games_;

	private mpCalls mpCalls_;

	private float timeToWait;

	public gameScript bestGrafik;

	public gameScript bestSound;

	public publisherScript bestStudio;

	public publisherScript bestPublisher;

	public gameScript bestGame;

	public gameScript badGame;

	private void Start()
	{
		FindScripts();
	}

	private void FindScripts()
	{
		if (!main_)
		{
			main_ = GameObject.Find("Main");
		}
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
		if (!themes_)
		{
			themes_ = main_.GetComponent<themes>();
		}
		if (!games_)
		{
			games_ = main_.GetComponent<games>();
		}
		if (!sfx_)
		{
			sfx_ = GameObject.Find("SFX").GetComponent<sfxScript>();
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

	private void Update()
	{
		if (!guiMain_.menuOpen)
		{
			guiMain_.OpenMenu(hideChars: false);
		}
	}

	public void Multiplayer_FindWinners(int IDbestGrafik, int IDbestSound, int IDbestStudio, int IDbestPublisher, int IDbestGame, int IDbadGame)
	{
		bestGrafik = null;
		bestSound = null;
		bestStudio = null;
		bestPublisher = null;
		bestGame = null;
		badGame = null;
		GameObject[] array = GameObject.FindGameObjectsWithTag("Game");
		if (array.Length != 0)
		{
			for (int i = 0; i < array.Length; i++)
			{
				gameScript component = array[i].GetComponent<gameScript>();
				if (IDbestGrafik == component.myID)
				{
					bestGrafik = component;
				}
				if (IDbestSound == component.myID)
				{
					bestSound = component;
				}
				if (IDbestGame == component.myID)
				{
					bestGame = component;
				}
				if (IDbadGame == component.myID)
				{
					badGame = component;
				}
			}
		}
		array = GameObject.FindGameObjectsWithTag("Publisher");
		if (array.Length == 0)
		{
			return;
		}
		for (int j = 0; j < array.Length; j++)
		{
			publisherScript component2 = array[j].GetComponent<publisherScript>();
			if (IDbestStudio == component2.myID)
			{
				bestStudio = component2;
			}
			if (IDbestPublisher == component2.myID)
			{
				bestPublisher = component2;
			}
		}
	}

	public void Init()
	{
		FindScripts();
		sfx_.PlaySound(31);
		timeToWait = 1f;
		if (!mS_.multiplayer || (mS_.multiplayer && mS_.mpCalls_.isServer))
		{
			bestGrafik = null;
			bestSound = null;
			bestStudio = null;
			bestPublisher = null;
			bestGame = null;
			badGame = null;
			bestGrafik = GetBesteGrafik();
			bestSound = GetBesterSound();
			bestStudio = GetBestesStudio();
			bestPublisher = GetBesterPublisher();
			bestGame = GetBestesSpiel();
			badGame = GetBadGame(bestGame);
		}
		if (mS_.multiplayer && mpCalls_.isServer)
		{
			guiMain_.BUTTON_GameSpeed(0f);
			int bestGrafik_ = -1;
			int bestSound_ = -1;
			int bestStudio_ = -1;
			int bestPublisher_ = -1;
			int bestGame_ = -1;
			int badGame_ = -1;
			if ((bool)bestGrafik)
			{
				bestGrafik_ = bestGrafik.myID;
			}
			if ((bool)bestSound)
			{
				bestSound_ = bestSound.myID;
			}
			if ((bool)bestStudio)
			{
				bestStudio_ = bestStudio.myID;
			}
			if ((bool)bestPublisher)
			{
				bestPublisher_ = bestPublisher.myID;
			}
			if ((bool)bestGame)
			{
				bestGame_ = bestGame.myID;
			}
			if ((bool)badGame)
			{
				badGame_ = badGame.myID;
			}
			mpCalls_.SERVER_Send_Award(bestGrafik_, bestSound_, bestStudio_, bestPublisher_, bestGame_, badGame_);
		}
		uiObjects[0].GetComponent<Text>().text = "";
		uiObjects[1].GetComponent<Text>().text = "";
		uiObjects[2].GetComponent<Text>().text = "";
		uiObjects[3].GetComponent<Text>().text = "";
		uiObjects[4].GetComponent<Text>().text = "";
		uiObjects[5].GetComponent<Text>().text = "";
		uiObjects[6].SetActive(value: false);
		uiObjects[7].SetActive(value: false);
		uiObjects[8].SetActive(value: false);
		uiObjects[9].SetActive(value: false);
		uiObjects[10].SetActive(value: false);
		uiObjects[11].SetActive(value: false);
		AddVerlauf();
		SetAsTeilgenommen();
		StartCoroutine(ShowAwards());
	}

	private void AddVerlauf()
	{
		int bestGrafik_ = -1;
		int bestSound_ = -1;
		int bestStudio_ = -1;
		int bestPublisher_ = -1;
		int bestGame_ = -1;
		int badBame_ = -1;
		if ((bool)bestGrafik)
		{
			bestGrafik_ = bestGrafik.myID;
		}
		if ((bool)bestSound)
		{
			bestSound_ = bestSound.myID;
		}
		if ((bool)bestStudio)
		{
			bestStudio_ = bestStudio.myID;
		}
		if ((bool)bestPublisher)
		{
			bestPublisher_ = bestPublisher.myID;
		}
		if ((bool)bestGame)
		{
			bestGame_ = bestGame.myID;
		}
		if ((bool)badGame)
		{
			badBame_ = badGame.myID;
		}
		mS_.AddMadGameConvetionVerlauf(bestGrafik_, bestSound_, bestStudio_, bestPublisher_, bestGame_, badBame_);
	}

	private string GetDevName(gameScript g_)
	{
		return "\n<size=12>[" + g_.GetDeveloperName() + "]</size>\n";
	}

	private IEnumerator ShowAwards()
	{
		if ((bool)mS_.settings_ && mS_.settings_.hideAwards)
		{
			timeToWait = 0f;
		}
		float myFans = genres_.GetAmountFans();
		mS_.awardBonus = 0;
		mS_.awardBonusAmount = 0f;
		yield return new WaitForSeconds(timeToWait);
		gameScript gameScript2 = bestGrafik;
		if ((bool)gameScript2)
		{
			gameScript2.AddHype(50f);
			if (gameScript2.developerID == mS_.myID)
			{
				float num = myFans * 0.02f;
				if (num > 40000f)
				{
					num = 40000 + Random.Range(0, 10000);
				}
				mS_.awardBonus = 20;
				mS_.awardBonusAmount += 0.05f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(38);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=green>+" + mS_.GetMoney(Mathf.RoundToInt(num + 1000f), showDollar: false) + "</color></b>");
				uiObjects[0].GetComponent<Text>().text = "<color=blue><b>" + gameScript2.GetNameWithTag() + "</b></color>" + GetDevName(gameScript2) + text;
				mS_.AddFans(Mathf.RoundToInt(num + 1000f), -1);
				sfx_.PlaySound(44);
				mS_.award_Grafik++;
				mS_.AddStudioPoints(40);
				gameScript2.AddIpPoints(40f);
				uiObjects[6].SetActive(value: true);
			}
			else
			{
				uiObjects[0].GetComponent<Text>().text = "<b>" + gameScript2.GetNameWithTag() + "</b>" + GetDevName(gameScript2);
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[0].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "\n" + mpCalls_.GetCompanyName(gameScript2.ownerID) + "</color>";
				}
				sfx_.PlaySound(45);
				if (gameScript2.OwnerIsNPC())
				{
					gameScript2.AddIpPoints(40f);
				}
			}
		}
		else
		{
			uiObjects[0].GetComponent<Text>().text = "-";
		}
		yield return new WaitForSeconds(timeToWait);
		gameScript2 = bestSound;
		if ((bool)gameScript2)
		{
			gameScript2.AddHype(50f);
			if (gameScript2.developerID == mS_.myID)
			{
				float num2 = myFans * 0.015f;
				if (num2 > 40000f)
				{
					num2 = 40000 + Random.Range(0, 10000);
				}
				mS_.awardBonus = 20;
				mS_.awardBonusAmount += 0.05f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(39);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=green>+" + mS_.GetMoney(Mathf.RoundToInt(num2 + 1000f), showDollar: false) + "</color></b>");
				uiObjects[1].GetComponent<Text>().text = "<color=blue><b>" + gameScript2.GetNameWithTag() + "</b></color>" + GetDevName(gameScript2) + text;
				mS_.AddFans(Mathf.RoundToInt(num2 + 1000f), -1);
				sfx_.PlaySound(44);
				mS_.award_Sound++;
				mS_.AddStudioPoints(40);
				gameScript2.AddIpPoints(40f);
				uiObjects[7].SetActive(value: true);
			}
			else
			{
				uiObjects[1].GetComponent<Text>().text = "<b>" + gameScript2.GetNameWithTag() + "</b>" + GetDevName(gameScript2);
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[1].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "\n" + mpCalls_.GetCompanyName(gameScript2.ownerID) + "</color>";
				}
				sfx_.PlaySound(45);
				if (gameScript2.OwnerIsNPC())
				{
					gameScript2.AddIpPoints(40f);
				}
			}
		}
		else
		{
			uiObjects[1].GetComponent<Text>().text = "-";
		}
		yield return new WaitForSeconds(timeToWait);
		publisherScript publisherScript2 = bestStudio;
		if ((bool)publisherScript2)
		{
			if (publisherScript2.myID != mS_.myID)
			{
				sfx_.PlaySound(45);
				uiObjects[2].GetComponent<Text>().text = publisherScript2.GetName();
				if (publisherScript2.isPlayer)
				{
					uiObjects[2].GetComponent<Text>().text = "<color=magenta>" + publisherScript2.GetName() + "</color>";
				}
			}
			else
			{
				float num3 = myFans * 0.045f;
				if (num3 > 40000f)
				{
					num3 = 40000 + Random.Range(0, 10000);
				}
				mS_.awardBonus = 20;
				mS_.awardBonusAmount += 0.2f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(36);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=green>+" + mS_.GetMoney(Mathf.RoundToInt(num3 + 2500f), showDollar: false) + "</color></b>");
				uiObjects[2].GetComponent<Text>().text = "<color=blue>" + publisherScript2.GetName() + "</color>\n" + text;
				mS_.AddFans(Mathf.RoundToInt(num3 + 2500f), -1);
				sfx_.PlaySound(44);
				mS_.award_Studio++;
				mS_.AddStudioPoints(100);
				uiObjects[8].SetActive(value: true);
			}
		}
		yield return new WaitForSeconds(timeToWait);
		publisherScript2 = bestPublisher;
		if ((bool)publisherScript2)
		{
			if (publisherScript2.myID != mS_.myID)
			{
				uiObjects[3].GetComponent<Text>().text = publisherScript2.GetName();
				sfx_.PlaySound(45);
				if (publisherScript2.isPlayer)
				{
					uiObjects[3].GetComponent<Text>().text = "<color=magenta>" + publisherScript2.GetName() + "</color>";
				}
			}
			else
			{
				float num4 = myFans * 0.04f;
				if (num4 > 40000f)
				{
					num4 = 40000 + Random.Range(0, 10000);
				}
				mS_.awardBonus = 20;
				mS_.awardBonusAmount += 0.2f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(37);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=green>+" + mS_.GetMoney(Mathf.RoundToInt(num4 + 2500f), showDollar: false) + "</color></b>");
				uiObjects[3].GetComponent<Text>().text = "<color=blue>" + publisherScript2.GetName() + "</color>\n" + text;
				mS_.AddFans(Mathf.RoundToInt(num4 + 2500f), -1);
				sfx_.PlaySound(44);
				mS_.award_Publisher++;
				mS_.AddStudioPoints(100);
				uiObjects[9].SetActive(value: true);
			}
		}
		yield return new WaitForSeconds(timeToWait);
		gameScript2 = bestGame;
		if ((bool)gameScript2)
		{
			gameScript2.AddHype(100f);
			gameScript2.goty = true;
			if (gameScript2.developerID == mS_.myID)
			{
				float num5 = myFans * 0.05f;
				if (num5 > 40000f)
				{
					num5 = 40000 + Random.Range(0, 10000);
				}
				mS_.awardBonus = 20;
				mS_.awardBonusAmount += 0.1f;
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(35);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=green>+" + mS_.GetMoney(Mathf.RoundToInt(num5 + 3000f), showDollar: false) + "</color></b>");
				uiObjects[4].GetComponent<Text>().text = "<color=blue><b>" + gameScript2.GetNameWithTag() + "</b></color>" + GetDevName(gameScript2) + text;
				mS_.AddFans(Mathf.RoundToInt(num5 + 3000f), -1);
				sfx_.PlaySound(44);
				mS_.award_GOTY++;
				mS_.AddStudioPoints(75);
				gameScript2.AddIpPoints(100f);
				uiObjects[10].SetActive(value: true);
			}
			else
			{
				uiObjects[4].GetComponent<Text>().text = "<b>" + gameScript2.GetNameWithTag() + "</b>" + GetDevName(gameScript2);
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[4].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "\n" + mpCalls_.GetCompanyName(gameScript2.ownerID) + "</color>";
				}
				sfx_.PlaySound(45);
				if (gameScript2.OwnerIsNPC())
				{
					gameScript2.AddIpPoints(100f);
				}
			}
		}
		else
		{
			uiObjects[4].GetComponent<Text>().text = "-";
		}
		yield return new WaitForSeconds(timeToWait);
		gameScript2 = badGame;
		if ((bool)gameScript2)
		{
			gameScript2.AddHype(-50f);
			if (gameScript2.developerID == mS_.myID)
			{
				float num6 = myFans * 0.05f;
				if (num6 > 40000f)
				{
					num6 = 40000 + Random.Range(0, 10000);
				}
				if ((bool)mS_.achScript_)
				{
					mS_.achScript_.SetAchivement(40);
				}
				string text = tS_.GetText(763);
				text = text.Replace("<NUM>", "<b><color=red>-" + mS_.GetMoney(Mathf.RoundToInt(num6 + 2000f), showDollar: false) + "</color></b>");
				uiObjects[5].GetComponent<Text>().text = "<color=blue><b>" + gameScript2.GetNameWithTag() + "</b></color>" + GetDevName(gameScript2) + text;
				mS_.AddFans(-Mathf.RoundToInt(num6 + 2000f), -1);
				sfx_.PlaySound(45);
				uiObjects[11].SetActive(value: true);
			}
			else
			{
				uiObjects[5].GetComponent<Text>().text = "<b>" + gameScript2.GetNameWithTag() + "</b>" + GetDevName(gameScript2);
				if (mS_.multiplayer && gameScript2.GameFromMitspieler())
				{
					uiObjects[5].GetComponent<Text>().text = "<color=magenta>" + gameScript2.GetNameWithTag() + "\n" + mpCalls_.GetCompanyName(gameScript2.ownerID) + "</color>";
				}
				sfx_.PlaySound(44);
			}
		}
		else
		{
			uiObjects[5].GetComponent<Text>().text = "-";
		}
		timeToWait = 0f;
		if ((bool)mS_.settings_ && mS_.settings_.hideAwards)
		{
			BUTTON_Abbrechen();
		}
	}

	private void SetAsTeilgenommen()
	{
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i].warBeiAwards && CheckGame(games_.arrayGamesScripts[i]))
			{
				games_.arrayGamesScripts[i].warBeiAwards = true;
			}
		}
	}

	private bool CheckGame(gameScript script_)
	{
		if ((bool)script_ && !script_.inDevelopment && !script_.schublade && !script_.pubAngebot && !script_.auftragsspiel && (script_.typ_standard || script_.typ_nachfolger || script_.typ_remaster || script_.typ_spinoff) && !script_.typ_addon && !script_.typ_addonStandalone && !script_.typ_mmoaddon && !script_.typ_bundle && !script_.typ_budget && !script_.typ_bundleAddon && !script_.typ_goty && script_.weeksOnMarket > 0)
		{
			return true;
		}
		return false;
	}

	private gameScript GetBesteGrafik()
	{
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i].warBeiAwards && CheckGame(games_.arrayGamesScripts[i]) && num2 < games_.arrayGamesScripts[i].reviewGrafik)
			{
				num = i;
				num2 = games_.arrayGamesScripts[i].reviewGrafik;
			}
		}
		if (num != -1)
		{
			if (!games_.arrayGamesScripts[num].devS_)
			{
				games_.arrayGamesScripts[num].FindMyDeveloper();
			}
			if ((bool)games_.arrayGamesScripts[num].devS_)
			{
				mS_.AddAwards(0, games_.arrayGamesScripts[num].devS_);
			}
			return games_.arrayGamesScripts[num];
		}
		return null;
	}

	private gameScript GetBesterSound()
	{
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i].warBeiAwards && CheckGame(games_.arrayGamesScripts[i]) && num2 < games_.arrayGamesScripts[i].reviewSound)
			{
				num = i;
				num2 = games_.arrayGamesScripts[i].reviewSound;
			}
		}
		if (num != -1)
		{
			if (!games_.arrayGamesScripts[num].devS_)
			{
				games_.arrayGamesScripts[num].FindMyDeveloper();
			}
			if ((bool)games_.arrayGamesScripts[num].devS_)
			{
				mS_.AddAwards(1, games_.arrayGamesScripts[num].devS_);
			}
			return games_.arrayGamesScripts[num];
		}
		return null;
	}

	private gameScript GetBestesSpiel()
	{
		int num = -1;
		int num2 = -1;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i].warBeiAwards && CheckGame(games_.arrayGamesScripts[i]))
			{
				int num3 = games_.arrayGamesScripts[i].reviewTotal + Mathf.RoundToInt(games_.arrayGamesScripts[i].GetUserReviewPercent());
				if (num2 < num3)
				{
					num = i;
					num2 = num3;
				}
			}
		}
		if (num != -1)
		{
			if (!games_.arrayGamesScripts[num].devS_)
			{
				games_.arrayGamesScripts[num].FindMyDeveloper();
			}
			if ((bool)games_.arrayGamesScripts[num].devS_)
			{
				mS_.AddAwards(4, games_.arrayGamesScripts[num].devS_);
			}
			return games_.arrayGamesScripts[num];
		}
		return null;
	}

	private gameScript GetBadGame(gameScript bestGame_)
	{
		int num = -1;
		int num2 = 999;
		for (int i = 0; i < games_.arrayGamesScripts.Length; i++)
		{
			if (!games_.arrayGamesScripts[i].warBeiAwards && CheckGame(games_.arrayGamesScripts[i]) && num2 > games_.arrayGamesScripts[i].reviewTotal && games_.arrayGamesScripts[i].reviewTotal < 60)
			{
				num = i;
				num2 = games_.arrayGamesScripts[i].reviewTotal;
			}
		}
		if (num != -1)
		{
			if (bestGame_ != games_.arrayGamesScripts[num])
			{
				if (!games_.arrayGamesScripts[num].devS_)
				{
					games_.arrayGamesScripts[num].FindMyDeveloper();
				}
				if ((bool)games_.arrayGamesScripts[num].devS_)
				{
					mS_.AddAwards(5, games_.arrayGamesScripts[num].devS_);
				}
				return games_.arrayGamesScripts[num];
			}
			return null;
		}
		return null;
	}

	private publisherScript GetBestesStudio()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<publisherScript>().awards_bestStudioPoints = 0L;
		}
		for (int j = 0; j < games_.arrayGamesScripts.Length; j++)
		{
			if (games_.arrayGamesScripts[j].warBeiAwards || games_.arrayGamesScripts[j].inDevelopment || games_.arrayGamesScripts[j].schublade || games_.arrayGamesScripts[j].pubAngebot || games_.arrayGamesScripts[j].auftragsspiel || (!games_.arrayGamesScripts[j].typ_standard && !games_.arrayGamesScripts[j].typ_nachfolger && !games_.arrayGamesScripts[j].typ_remaster && !games_.arrayGamesScripts[j].typ_spinoff))
			{
				continue;
			}
			if (!games_.arrayGamesScripts[j].devS_)
			{
				games_.arrayGamesScripts[j].FindMyDeveloper();
			}
			if ((bool)games_.arrayGamesScripts[j].devS_)
			{
				if (!games_.arrayGamesScripts[j].devS_.isPlayer)
				{
					games_.arrayGamesScripts[j].devS_.awards_bestStudioPoints += games_.arrayGamesScripts[j].reviewTotal;
				}
				else if (games_.arrayGamesScripts[j].reviewTotal >= 80)
				{
					games_.arrayGamesScripts[j].devS_.awards_bestStudioPoints += games_.arrayGamesScripts[j].reviewTotal;
				}
			}
		}
		long num = -1L;
		publisherScript publisherScript2 = null;
		for (int k = 0; k < array.Length; k++)
		{
			publisherScript component = array[k].GetComponent<publisherScript>();
			if ((bool)component && component.awards_bestStudioPoints > num)
			{
				num = component.awards_bestStudioPoints;
				publisherScript2 = component;
			}
		}
		if ((bool)publisherScript2)
		{
			publisherScript2.awards[2]++;
		}
		return publisherScript2;
	}

	private publisherScript GetBesterPublisher()
	{
		GameObject[] array = GameObject.FindGameObjectsWithTag("Publisher");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<publisherScript>().awards_bestPublisherPoints = 0L;
		}
		for (int j = 0; j < games_.arrayGamesScripts.Length; j++)
		{
			if (games_.arrayGamesScripts[j].warBeiAwards || games_.arrayGamesScripts[j].inDevelopment || games_.arrayGamesScripts[j].schublade || games_.arrayGamesScripts[j].pubAngebot || games_.arrayGamesScripts[j].auftragsspiel || games_.arrayGamesScripts[j].handy || games_.arrayGamesScripts[j].gameTyp == 2 || (!games_.arrayGamesScripts[j].typ_standard && !games_.arrayGamesScripts[j].typ_nachfolger && !games_.arrayGamesScripts[j].typ_remaster && !games_.arrayGamesScripts[j].typ_spinoff))
			{
				continue;
			}
			if (!games_.arrayGamesScripts[j].pS_)
			{
				games_.arrayGamesScripts[j].FindMyPublisher();
			}
			if ((bool)games_.arrayGamesScripts[j].pS_)
			{
				if (!games_.arrayGamesScripts[j].pS_.isPlayer)
				{
					games_.arrayGamesScripts[j].pS_.awards_bestPublisherPoints += games_.arrayGamesScripts[j].sellsTotal;
				}
				else if (games_.arrayGamesScripts[j].reviewTotal >= 80)
				{
					games_.arrayGamesScripts[j].pS_.awards_bestPublisherPoints += games_.arrayGamesScripts[j].sellsTotal;
				}
			}
		}
		long num = -1L;
		publisherScript publisherScript2 = null;
		for (int k = 0; k < array.Length; k++)
		{
			publisherScript component = array[k].GetComponent<publisherScript>();
			if ((bool)component && component.awards_bestPublisherPoints > num)
			{
				num = component.awards_bestPublisherPoints;
				publisherScript2 = component;
			}
		}
		if ((bool)publisherScript2)
		{
			publisherScript2.awards[3]++;
		}
		return publisherScript2;
	}

	public void BUTTON_Abbrechen()
	{
		sfx_.PlaySound(3, force: true);
		if (timeToWait > 0f)
		{
			timeToWait = 0f;
			return;
		}
		guiMain_.CloseMenu();
		base.gameObject.SetActive(value: false);
	}

	public void BUTTON_Yes()
	{
		BUTTON_Abbrechen();
	}
}
